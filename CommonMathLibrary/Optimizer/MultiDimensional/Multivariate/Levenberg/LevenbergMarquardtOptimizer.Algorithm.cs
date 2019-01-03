/* MIT License
Copyright (c) 2011-2019 Markus Wendt (http://www.dodoni-project.net)

All rights reserved.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 
Please see http://www.dodoni-project.net/ for more information concerning the Dodoni.net project. 
*/
using System;
using System.Linq;
using System.Collections.Generic;

using Dodoni.BasicComponents;
using Dodoni.MathLibrary.Basics;
using Dodoni.MathLibrary.Miscellaneous;

namespace Dodoni.MathLibrary.Optimizer.MultiDimensional
{
    public partial class LevenbergMarquardtOptimizer
    {
        /// <summary>Represents the implementation of the algorithm.
        /// </summary>
        private class Algorithm : IMultiDimOptimizerAlgorithm
        {
            #region private members

            /// <summary>The <see cref="LevenbergMarquardtOptimizer"/> object that serves as factory of the current object.
            /// </summary>
            private LevenbergMarquardtOptimizer m_Optimizer;

            /// <summary>The objective function in its <see cref="MultivariateFunction"/> representation.
            /// </summary>
            private MultivariateFunction m_ObjectiveFunction;

            /// <summary>A projection onto the feasible set.
            /// </summary>
            private FeasibleSetProjection m_ProjectionOntoFeasibleSet;
            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="Algorithm"/> class.
            /// </summary>
            /// <param name="optimizer">The <see cref="LevenbergMarquardtOptimizer"/> object that serves as factory of the current object.</param>
            /// <param name="dimension">The dimension of the feasible region.</param>
            internal Algorithm(LevenbergMarquardtOptimizer optimizer, int dimension)
            {
                m_Optimizer = optimizer ?? throw new ArgumentNullException(nameof(optimizer));
                if (dimension <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(dimension));
                }
                m_ProjectionOntoFeasibleSet = FeasibleSetProjection.Create(dimension);
            }

            /// <summary>Initializes a new instance of the <see cref="Algorithm"/> class.
            /// </summary>
            /// <param name="optimizer">The <see cref="LevenbergMarquardtOptimizer"/> object that serves as factory of the current object.</param>
            /// <param name="constraints">A collection of contraints for the optimization algorithm represented by the current instance, where each constraint has been created via a specific function of property <see cref="MultiDimOptimizer.Constraint"/>.</param>
            internal Algorithm(LevenbergMarquardtOptimizer optimizer, IConstraint[] constraints)
            {
                m_Optimizer = optimizer ?? throw new ArgumentNullException(nameof(optimizer));

                /* each constraint should be specified in its MultiDimOptimizerConstraint representation which contains the RegionRepresentation */
                m_ProjectionOntoFeasibleSet = FeasibleSetProjection.Create(constraints.Cast<MultiDimOptimizerConstraint>().Select(x => x.RegionRepresentation), new GoldfarbIdanaQuadraticProgram());
            }

            /// <summary>Initializes a new instance of the <see cref="Algorithm"/> class.
            /// </summary>
            /// <param name="optimizer">The <see cref="LevenbergMarquardtOptimizer"/> object that serves as factory of the current object.</param>
            /// <param name="boxConstraints">The box constraints.</param>
            internal Algorithm(LevenbergMarquardtOptimizer optimizer, MultiDimRegion.Interval boxConstraints)
            {
                m_Optimizer = optimizer ?? throw new ArgumentNullException(nameof(optimizer));
                m_ProjectionOntoFeasibleSet = FeasibleSetProjection.Create(boxConstraints);
            }
            #endregion

            #region public properties

            #region IMultiDimOptimizerAlgorithm Members

            /// <summary>Gets the factory for further <see cref="IMultiDimOptimizerAlgorithm"/> objects of the same type, i.e. with the same stopping condition etc.
            /// </summary>
            /// <value>The factory for further <see cref="IMultiDimOptimizerAlgorithm"/> objects of the same type.</value>
            public MultiDimOptimizer Factory
            {
                get { return m_Optimizer; }
            }

            /// <summary>Gets the dimension of the feasible region.
            /// </summary>
            /// <value>The dimension.</value>
            public int Dimension
            {
                get { return m_ProjectionOntoFeasibleSet.Dimension; }
            }

            /// <summary>Gets or sets the objective function in its <see cref="MultiDimOptimizer.IFunction"/> representation.
            /// </summary>
            /// <value>The objective function.</value>
            public MultiDimOptimizer.IFunction Function
            {
                get { return m_ObjectiveFunction; }
                set
                {
                    if (value is MultivariateFunction)
                    {
                        m_ObjectiveFunction = (MultivariateFunction)value;
                    }
                    else
                    {
                        throw new InvalidCastException();
                    }
                }
            }
            #endregion

            #endregion

            #region private properties

            /// <summary>Gets the abort (stopping) condition of the algorithm.
            /// </summary>
            /// <value>The abort (stopping) condition of the algorithm.</value>
            public LevenbergMarquardtAbortCondition AbortCondition
            {
                get { return m_Optimizer.AbortCondition; }
            }
            #endregion

            #region public methods

            #region IMultiDimOptimizerAlgorithm Members

            /// <summary>Finds the minimum and argmin of <see cref="IMultiDimOptimizerAlgorithm.Function"/>.
            /// </summary>
            /// <param name="x">An array with at least <see cref="IMultiDimOptimizerAlgorithm.Dimension"/> elements which is perhaps taken into account as an initial guess of the algorithm; on exit this argument contains the argmin.</param>
            /// <param name="minimum">The minimum, i.e. the function value with respect to <paramref name="x"/> which represents the argmin (output).</param>
            /// <returns>The state of the algorithm, i.e. an indicating whether <paramref name="x"/> and <paramref name="minimum"/> contain valid data.</returns>
            public State FindMinimum(double[] x, out double minimum)
            {
                /* todo: unterstelle zunächst, dass die Jakobimatrix verfügbar ist */

                if (m_ObjectiveFunction == null)
                {
                    throw new InvalidOperationException("No objective function");
                }
                int n = Dimension;
                int k = m_ObjectiveFunction.CodomainDimension;

                var J = new DenseMatrix(k, n);  // = Jacobian matrix
                var y = new DenseMatrix(k, 1); // = f(x)

                m_ObjectiveFunction.GetValue(x, J.Data, y.Data);
                int evaluationsNeeded = 1;

                var A = J.T * J;  // n x n matrix
                var g = J.T * y;  // = J' * f(x) 

                var delta = new DenseMatrix(n, 1); // the solution \delta of (A + \mu * I) * \delta = g 
                var p = new double[n];  // a temporary argument of the function
                var tempA = new double[n * n]; // a temporary copy of matrix A

                var mu = AbortCondition.Tau * A.Data.Where((v, index) => index % (k + 1) == 0).Max();  // \tau * max_j A[j,j]

                if (g.GetNorm(MatrixNormType.Infinity) <= AbortCondition.Tolerance1)
                {
                    minimum = BLAS.Level1.dnrm2sq(k, y.Data);
                    return State.Create(OptimizerErrorClassification.ProperResult, minimum, evaluationsNeeded, iterationsNeeded: 0);
                }

                /* remarks:
                 *  \mu is not adjusted as in the original paper
                 */
                var nu = 2.0;
                var iPivotTemp = new int[k];
                for (int iter = 1; iter <= AbortCondition.MaxIterations; iter++)
                {
                    double rho;
                    do
                    {
                        /* solve (A + \mu * I) * \delta = g */
                        BLAS.Level1.dcopy(A.RowCount * A.ColumnCount, A.Data, tempA);
                        for (int j = 0; j < n; j++) // set A = A + \mu * I
                        {
                            tempA[j + j * n] += mu;
                        }
                        BLAS.Level1.dcopy(n, g.Data, delta.Data);
                        BLAS.Level1.dscal(n, -1.0, delta.Data);  // delta  = -g
                        LAPACK.LinearEquations.Solver.driver_dgesv(n, tempA, iPivotTemp, delta.Data);  // tempA will be changed

                        if (delta.GetNorm() <= AbortCondition.Tolerance2 * BLAS.Level1.dnrm2(n, x))
                        {
                            m_ObjectiveFunction.GetValue(x, null, y.Data);
                            minimum = BLAS.Level1.dnrm2sq(k, y.Data);

                            return State.Create(OptimizerErrorClassification.ProperResult, minimum, evaluationsNeeded + 1, iter);
                        }
                        BLAS.Level1.dcopy(n, x, p);
                        VectorUnit.Basics.Add(n, p, delta.Data); // p = x + \delta

                        m_ProjectionOntoFeasibleSet.GetValue(p); /* p is perhaps not inside the feasible region, i.e. apply the projection to the point, i.e. set p = P_X(x+\delta), where P_X is the projection onto feasible set X */

                        var normOfy = y.GetNorm(MatrixNormType.Frobenius);
                        m_ObjectiveFunction.GetValue(p, J.Data, y.Data);
                        evaluationsNeeded++;

                        double tempRho = mu * BLAS.Level1.ddot(n, delta.Data, delta.Data) - BLAS.Level1.ddot(n, delta.Data, g.Data); // = \delta' * (\mu *  \delta + g)

                        rho = (normOfy * normOfy - y.GetNorm(MatrixNormType.Frobenius) * y.GetNorm(MatrixNormType.Frobenius)) / tempRho;
                        if (rho > 0)
                        {
                            BLAS.Level1.dcopy(n, p, x);  // set x = p

                            A.AddAssignment(J.T, J, beta: 0.0);    //  A = * J.T * J
                            g.AddAssignment(J.T, y, beta: 0.0);  // g = J.T * y

                            if ((g.GetNorm(MatrixNormType.Infinity) <= AbortCondition.Tolerance1) || (y.GetNorm(MatrixNormType.Frobenius) < AbortCondition.Tolerance3))
                            {
                                minimum = BLAS.Level1.dnrm2sq(k, y.Data);
                                return State.Create(OptimizerErrorClassification.ProperResult, minimum, evaluationsNeeded, iter);
                            }
                            mu *= Math.Max(1.0 / 3.0, 1 - (2.0 * rho - 1.0) * (2.0 * rho - 1.0) * (2.0 * rho - 1.0));
                            nu = 2.0;
                        }
                        else
                        {
                            mu *= nu;
                            nu *= 2.0;
                        }

                        if (evaluationsNeeded > AbortCondition.MaxEvaluations)
                        {
                            m_ObjectiveFunction.GetValue(x, null, y.Data);
                            minimum = BLAS.Level1.dnrm2sq(k, y.Data);

                            return State.Create(OptimizerErrorClassification.EvaluationLimitExceeded, minimum, evaluationsNeeded + 1, iter);
                        }
                    } while (rho <= 0.0);
                }

                m_ObjectiveFunction.GetValue(x, null, y.Data);
                minimum = BLAS.Level1.dnrm2sq(k, y.Data);

                return State.Create(OptimizerErrorClassification.IterationLimitExceeded, minimum, evaluationsNeeded + 1, AbortCondition.MaxIterations);
            }
            #endregion

            #region IDisposable Members

            /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                // nothing to do here
            }
            #endregion

            /// <summary>Returns a <see cref="System.String" /> that represents this instance.
            /// </summary>
            /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
            public override string ToString()
            {
                return String.Format("{0}; dimension = {1}", Factory.Name.String, Dimension);
            }
            #endregion
        }
    }
}