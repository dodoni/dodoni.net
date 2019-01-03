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

using Dodoni.MathLibrary.Basics;

namespace Dodoni.MathLibrary.Optimizer.MultiDimensional
{
    public partial class NelderMeadOptimizer
    {
        /// <summary>Represents the implementation of the algorithm.
        /// </summary>
        private class Algorithm : IMultiDimOptimizerAlgorithm
        {
            #region private consts

            /// <summary>The value which is used for a reflection step.
            /// </summary>
            private const double sm_Alpha = -1.0;

            /// <summary>The value which is used for a contraction step.
            /// </summary>
            private const double sm_Beta = 0.5;

            /// <summary>The value which is used for a extrapolation step.
            /// </summary>
            private const double sm_Gamma = 2.0;
            #endregion

            #region private members

            /// <summary>The <see cref="NelderMeadOptimizer"/> object that serves as factory of the current object.
            /// </summary>
            private NelderMeadOptimizer m_Optimizer;

            /// <summary>The objective function in its <see cref="OrdinaryMultiDimOptimizerFunction"/> representation.
            /// </summary>
            private OrdinaryMultiDimOptimizerFunction m_ObjectiveFunction;

            /// <summary>The positions of the simplex with n+1 nodes, i.e. the first index corresponds to the null-based index of
            /// the points in the plain (unconstraint solving space) and the second index corresponds to the coordinate, i.e. a (n+1) x n matrix.
            /// </summary>
            private double[][] m_Simplex;

            /// <summary>The function values of the simplex with n+1 nodes given by <see cref="m_Simplex"/>.
            /// </summary>
            private double[] m_SimplexValues;

            /// <summary>A temporary variable which will be used for <see cref="TryChangeHighestPoint(double[][],double[],int,double,double[])"/>.
            /// </summary>
            private double[] m_pTryTemp;
            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="Algorithm"/> class.
            /// </summary>
            /// <param name="optimizer">The <see cref="NelderMeadOptimizer"/> object that serves as factory of the current object.</param>
            /// <param name="dimension">The dimension of the feasible region.</param>
            internal Algorithm(NelderMeadOptimizer optimizer, int dimension)
            {
                m_Optimizer = optimizer;
                Dimension = dimension;

                m_pTryTemp = new double[dimension];
                m_Simplex = new double[dimension + 1][];
                m_SimplexValues = new double[dimension + 1];
                for (int j = 0; j <= dimension; j++)
                {
                    m_Simplex[j] = new double[dimension];
                }
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
                get;
                private set;
            }

            /// <summary>Gets or sets the objective function in its <see cref="MultiDimOptimizer.IFunction"/> representation.
            /// </summary>
            /// <value>The objective function.</value>
            public MultiDimOptimizer.IFunction Function
            {
                get { return m_ObjectiveFunction; }
                set
                {
                    if (value is OrdinaryMultiDimOptimizerFunction)
                    {
                        var ordinaryObjectiveFunction = value as OrdinaryMultiDimOptimizerFunction;
                        if (ordinaryObjectiveFunction.Dimension != Dimension)
                        {
                            throw new InvalidOperationException(String.Format("Inconsistent dimension of feasible region and objective function."));
                        }
                        m_ObjectiveFunction = ordinaryObjectiveFunction;
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
            public NelderMeadOptimizerAbortCondition AbortCondition
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
                /* compute the values at the knots of the simplex: */

                /* create the N+1 points, i.e. the start simplex, which is P_i = P_0 + Q + c/sqrt(2) * e_{i-1}, j = 1,...,n where e_j is the unit vector with respect to the j-th coordinate,
                 * P_0 is the initial guess; Q = c/Sqrt(2) * (Sqrt(n+1) - 1)/n * (1,...,1) and c is the InitialScaleFactor
                 */
                int n = Dimension;
                var q = Enumerable.Repeat(m_Optimizer.InitialScaleFactor * (Math.Sqrt(n + 1) - 1.0) / (MathConsts.Sqrt2 * n), n).ToArray();
                
                for (int i = 1; i <= n; i++)
                {
                    BLAS.Level1.dcopy(n, x, m_Simplex[i]);
                    VectorUnit.Basics.Add(n, m_Simplex[i], q);

                    m_Simplex[i][i - 1] += m_Optimizer.InitialScaleFactor / MathConsts.Sqrt2;
                    m_SimplexValues[i] = m_ObjectiveFunction.GetValue(m_Simplex[i]);
                }
                BLAS.Level1.dcopy(n, x, m_Simplex[0]);
                m_SimplexValues[0] = m_ObjectiveFunction.GetValue(m_Simplex[0]);
                
                int lowestPosition;

                var state = EvaluateNelderMead(m_Simplex, m_SimplexValues, out lowestPosition);
                BLAS.Level1.dcopy(n, m_Simplex[lowestPosition], x);
                minimum = m_SimplexValues[lowestPosition];
                return state;
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

            #region protected methods

            /// <summary>Starts the Nelder-Mead-Simplex algorithm with a specified start simplex and corresponding function values.
            /// </summary><param name="simplex">The initial simplex, i.e. the first null-based index corresponds to the index of the point in the plain and the second null-based index corresponds to 
            /// the coordinate, i.e. (n+1) x n matrix; on exit the <paramref name="lowestPosition"/> entry represents the estimated argMin of the specified objective function.</param>
            /// <param name="simplexValues">The function values of the points with respect to <paramref name="simplex"/>, i.e. at least n+1 elements; on exit the <paramref name="lowestPosition"/> entry represents the estimated minimum of the specified objective function.</param>
            /// <param name="lowestPosition">The null-based index of <paramref name="simplex"/> and <paramref name="simplexValues"/> that represents the minimum, argMin respectively (output).</param>
            /// <returns>The state of the algorithm, i.e. an indicating whether <paramref name="lowestPosition"/> contain valid data.</returns>
            protected State EvaluateNelderMead(double[][] simplex, double[] simplexValues, out int lowestPosition)
            {
                int n = Dimension;
                var psum = new Double[n];
                int numberOfSatisfiedConditions = 0;

                int evaluationsNeeded = n + 1;

                /* instead of:
                 *             
                   for (int j = 0; j <= n - 1; j++) {
                     double sum = 0.0;
                     for (int i = 0; i <= n; i++) {
                        sum += simplex[i][j];
                     }
                     psum[j] = sum;            
                   } 
                 * we use an equivalent BLAS level 1 implementation:
                 */
                for (int k = 0; k <= n; k++)
                {
                    BLAS.Level1.daxpy(n, 1.0, simplex[k], psum);
                }

                int highestPosition = 0;
                lowestPosition = 0;

                for (int k = 1; k <= AbortCondition.MaxIterations; k++)
                {
                    /* determine which point is the highest, next-highest and lowest: */
                    int nextHighestPosition;

                    lowestPosition = 0;
                    if (simplexValues[0] > simplexValues[1])
                    {
                        nextHighestPosition = 1;
                        highestPosition = 0;
                    }
                    else
                    {
                        nextHighestPosition = 0;
                        highestPosition = 1;
                    }
                    for (int i = 0; i <= n; i++)
                    {
                        if (simplexValues[i] <= simplexValues[lowestPosition])
                        {
                            lowestPosition = i;
                        }
                        if (simplexValues[i] > simplexValues[highestPosition])
                        {
                            nextHighestPosition = highestPosition;
                            highestPosition = i;
                        }
                        else if ((simplexValues[i] > simplexValues[nextHighestPosition]) && (i != highestPosition))
                        {
                            nextHighestPosition = i;
                        }
                    }

                    if (AbortCondition.IsSatisfied(n, simplex, simplexValues, lowestPosition, highestPosition, ref numberOfSatisfiedConditions) == true)
                    {
                        return State.Create(OptimizerErrorClassification.ProperResult, m_SimplexValues[lowestPosition], evaluationsNeeded, k);
                    }
                    if (evaluationsNeeded > AbortCondition.MaxEvaluations)
                    {
                        return State.Create(OptimizerErrorClassification.EvaluationLimitExceeded, m_SimplexValues[lowestPosition], evaluationsNeeded, k);
                    }

                    /* a additional iteration step is necessary, thus change the simplex, i.e. the position of 
                     * the edge with the greates value via reflection, reflection and expansion or contraction. */
                    evaluationsNeeded += 2;

                    double yTry = TryChangeHighestPoint(simplex, simplexValues, highestPosition, sm_Alpha, psum); /* reflection */
                    if (yTry <= simplexValues[lowestPosition])
                    {
                        yTry = TryChangeHighestPoint(simplex, simplexValues, highestPosition, sm_Gamma, psum); /* extrapolation by factor 2 */
                    }
                    else if (yTry >= simplexValues[nextHighestPosition])
                    {
                        double yTemp = simplexValues[highestPosition];
                        yTry = TryChangeHighestPoint(simplex, simplexValues, highestPosition, sm_Beta, psum);  /* the reflected point is worse than the second-highest, so look for an intermediate lower point, i.e. do a one-dimensional contraction */
                        if (yTry >= yTemp)
                        {
                            for (int i = 0; i <= n; i++)  /* contration with respect to the best (smalles) value: */
                            {
                                if (i != lowestPosition)
                                {
                                    for (int j = 0; j <= n - 1; j++)
                                    {
                                        simplex[i][j] = psum[j] = 0.5 * (simplex[i][j] + simplex[lowestPosition][j]);
                                    }
                                    simplexValues[i] = m_ObjectiveFunction.GetValue(psum);
                                }
                            }
                            evaluationsNeeded += n; /* change n of the (n+1) nodes  */

                            /* instead of:
                             *
                              for (int j = 0; j <= n - 1; j++) {
                                double sum = 0.0;
                                for (int i = 0; i <= n; i++) {
                                    sum += simplex[i][j];
                                }
                                psum[j] = sum;
                              }
                             * we use an equivalent BLAS level 1 implementation: */

                            BLAS.Level1.dscal(n, 0.0, psum);
                            for (int m = 0; m <= n; m++)
                            {
                                BLAS.Level1.daxpy(n, 1.0, simplex[m], psum);
                            }
                        }
                    }
                    else
                    {
                        evaluationsNeeded--;
                    }
                }
                return State.Create(OptimizerErrorClassification.IterationLimitExceeded, simplexValues[lowestPosition], evaluationsNeeded, AbortCondition.MaxIterations);
            }

            /// <summary>Changes the position of the node with the largest value via a given factor which corresponds to a reflection, expansion or contraction.
            /// </summary>
            /// <param name="p">The nodes of the simplex, p[i,:], i = 0,..,n is the i-th node.</param>
            /// <param name="y">The values of the nodes given by <paramref name="p"/>.</param>
            /// <param name="positionOfHighestValue">The nullbased index of the node in <paramref name="p"/> (and <paramref name="y"/>) with the largest value.</param>
            /// <param name="factor">The factor which is used for the mirroring of the node.</param>
            /// <param name="psum"></param>
            /// <returns>The value of the node which had the largest value before changing positions.</returns>
            /// <remarks>After execution <paramref name="p"/> contains the new simplex and <paramref name="y"/> contains the corresponding values at the points of the simplex.</remarks>
            private double TryChangeHighestPoint(double[][] p, double[] y, int positionOfHighestValue, double factor, double[] psum)
            {
                int n = Dimension;

                /* change the position of the edge with the highest value: */
                double fac1 = (1.0 - factor) / n;
                double fac2 = fac1 - factor;
                for (int j = 0; j <= n - 1; j++)
                {
                    m_pTryTemp[j] = psum[j] * fac1 - p[positionOfHighestValue][j] * fac2;
                }

                double yTry = m_ObjectiveFunction.GetValue(m_pTryTemp);
                if (yTry < y[positionOfHighestValue])  // new edge has a lower value?
                {
                    y[positionOfHighestValue] = yTry;   /* ... then use this edge */
                    for (int j = 0; j <= n - 1; j++)
                    {
                        psum[j] += m_pTryTemp[j] - p[positionOfHighestValue][j];
                        p[positionOfHighestValue][j] = m_pTryTemp[j];
                    }
                }
                return yTry;
            }
            #endregion
        }
    }
}