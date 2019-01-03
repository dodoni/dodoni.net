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

using Dodoni.MathLibrary.Basics;
using Dodoni.MathLibrary.Optimizer.OneDimensional;

namespace Dodoni.MathLibrary.Optimizer.MultiDimensional
{
    public partial class PowellOptimizer
    {
        /// <summary>Represents the implementation of the algorithm.
        /// </summary>
        private class Algorithm : IMultiDimOptimizerAlgorithm
        {
            #region private members

            /// <summary>The <see cref="PowellOptimizer"/> object that serves as factory of the current object.
            /// </summary>
            private PowellOptimizer m_Optimizer;

            /// <summary>The objective function in its <see cref="OrdinaryMultiDimOptimizerFunction"/> representation.
            /// </summary>
            private OrdinaryMultiDimOptimizerFunction m_ObjectiveFunction;

            /// <summary>The <see cref="IOneDimOptimizerAlgorithm"/> representation of the line search optimizer.
            /// </summary>
            private IOneDimOptimizerAlgorithm m_LineSearchOptimizer;

            /// <summary>The column-by-column provided matrix, where the columns contain the (initial/current) set of directions taken into account for the optimization procedure.
            /// </summary>
            private double[] m_DirectionMatrix;

            /// <summary>The number of iteration needed to reset the direction matrix if desired.
            /// </summary>
            /// <remarks>Sometime it is better to reset the direction matrix periodically and set it to the unity base.</remarks>
            private int m_IterationsNeededToResetDirectionMatrix = 0;

            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="Algorithm"/> class.
            /// </summary>
            /// <param name="optimizer">The <see cref="PowellOptimizer"/> object that serves as factory of the current object.</param>
            /// <param name="dimension">The dimension of the feasible region.</param>
            internal Algorithm(PowellOptimizer optimizer, int dimension)
            {
                m_Optimizer = optimizer;
                Dimension = dimension;

                m_DirectionMatrix = new double[dimension * dimension];
                m_LineSearchOptimizer = optimizer.LineSearchOptimizer.Create();
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
            public PowellOptimizerAbortCondition AbortCondition
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
                int n = Dimension;
                var x0 = new double[n];
                BLAS.Level1.dcopy(n, x, x0);

                int nMinusOneTimesN = (n - 1) * n;
                int nMinusTwoTimesN = (n - 2) * n;

                int currentDirectionIndex = -1; // The null-based index of the column of 'lineSearchDirection' indicating the direction vector which is used in the current 1-dim optimization procedure step
                double[] lineSearchDirection = null; // a pointer to the direction matrix or to a specific copy of the last row of the direction matrix

                var tempPointOnLine = new double[n];

                /* initial objective function of line search algorithm as t => f(z), where  z_j = x[j] + t * DirectionMatrix[currentDirectionIndex, j] */
                m_LineSearchOptimizer.Function = m_Optimizer.LineSearchOptimizer.Function.Create(
                    t =>
                    {
                        BLAS.Level1.dcopy(n, x, tempPointOnLine);
                        BLAS.Level1.daxpy(n, t, lineSearchDirection, tempPointOnLine, 1, 1, currentDirectionIndex * n, 0);

                        return m_ObjectiveFunction.GetValue(tempPointOnLine);
                    });

                ResetDirectionMatrix();
                minimum = m_ObjectiveFunction.GetValue(x);

                int evaluationsNeeded = 1;

                var xExtrapolated = new double[n];
                var tempLastDirectory = new double[n];

                for (int iteration = 1; iteration <= AbortCondition.MaxIterations; iteration++)
                {
                    if ((m_IterationsNeededToResetDirectionMatrix > 0) && (iteration % m_IterationsNeededToResetDirectionMatrix == 0))
                    {
                        ResetDirectionMatrix();
                    }

                    var delta = 0.0;  // the largest function decrease
                    var y0 = minimum;
                    int indexOfLargestDecrease = -1;

                    var lineArgMin = 1.0;
                    var lineMinimum = minimum;

                    lineSearchDirection = m_DirectionMatrix; // apply the direction matrix in the 1-dimensional optimization

                    for (currentDirectionIndex = 0; currentDirectionIndex < n; currentDirectionIndex++)
                    {
                        /* do a one-dimensional optimization in the direction indicating by m_DirectionMatrix[currentDirectionIndex,:] through the current point: */
                        var lineSearchResult = m_LineSearchOptimizer.FindMinimum(0, out lineArgMin, out lineMinimum);  // we take the result in any case, even it is not a "proper" result
                        evaluationsNeeded += lineSearchResult.EvaluationsNeeded;

                        BLAS.Level1.daxpy(n, lineArgMin, m_DirectionMatrix, x, 1, 1, n * currentDirectionIndex, 0); // x[i] += lineArgMin * m_DirectionMatrix[currentDirectionIndex, i]

                        if (minimum - lineMinimum > delta)
                        {
                            delta = minimum - lineMinimum;
                            indexOfLargestDecrease = currentDirectionIndex;
                        }
                        minimum = lineMinimum;

                    }
                    /* create a copy of the last direction and scale it with the result of the last line optimization */
                    BLAS.Level1.dcopy(n, m_DirectionMatrix, tempLastDirectory, 1, 1, nMinusOneTimesN, 0);
                    BLAS.Level1.dscal(n, lineArgMin, tempLastDirectory);

                    if (AbortCondition.IsSatisfied(y0, minimum) == true)
                    {
                        return State.Create(OptimizerErrorClassification.ProperResult, minimum, evaluationsNeeded, iteration);
                    }
                    if (evaluationsNeeded >= AbortCondition.MaxEvaluations)
                    {
                        return State.Create(OptimizerErrorClassification.EvaluationLimitExceeded, minimum, evaluationsNeeded, iteration);
                    }

                    /* Construct the extrapolated point and set x_0 := x_N [=x]: */
                    for (int i = 0; i < n; i++)
                    {
                        xExtrapolated[i] = 2.0 * x[i] - x0[i];
                        tempLastDirectory[i] = x[i] - x0[i];
                        x0[i] = x[i];
                    }
                    var yExtrapolated = m_ObjectiveFunction.GetValue(xExtrapolated);
                    evaluationsNeeded++;

                    if (yExtrapolated < y0)
                    {
                        var t = (y0 - minimum - delta);
                        t = 2.0 * (y0 - 2.0 * minimum + yExtrapolated) * t * t - delta * (y0 - yExtrapolated) * (y0 - yExtrapolated);

                        if (t < 0.0)
                        {
                            /* apply the 1-dim optimizer to the scaled copy of the last direction, i.e. set pointer and offset: */
                            currentDirectionIndex = 0;
                            lineSearchDirection = tempLastDirectory;


                            var lineSearchResult = m_LineSearchOptimizer.FindMinimum(0, out lineArgMin, out lineMinimum);
                            evaluationsNeeded += lineSearchResult.EvaluationsNeeded;
                            BLAS.Level1.daxpy(n, lineArgMin, tempLastDirectory, x); // x[i] += lineArgMin * lastDirection
                            BLAS.Level1.dscal(n, lineArgMin, tempLastDirectory);  // lastDirection *= lineArgMin

                            minimum = lineMinimum;

                            /* replace the direction with the largest decrease by the 'last direction' and the set the 'last direction' to the direction 
                             * of P_N - P_0 which is also stored in the 'm_DirectionMatrix': */
                            for (int j = 0; j < n; j++)
                            {
                                m_DirectionMatrix[indexOfLargestDecrease * n + j] = m_DirectionMatrix[nMinusOneTimesN + j];
                                m_DirectionMatrix[nMinusOneTimesN + j] = tempLastDirectory[j];
                            }
                        }
                    }
                    evaluationsNeeded++;
                }
                return State.Create(OptimizerErrorClassification.IterationLimitExceeded, evaluationsNeeded, AbortCondition.MaxIterations);
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

            #region private methods

            /// <summary>Store the unit vectors into the matrix <see cref="m_DirectionMatrix"/> which contains the directions which are used.
            /// </summary>
            private void ResetDirectionMatrix()
            {
                BLAS.Level1.dscal(Dimension * Dimension, 0.0, m_DirectionMatrix);
                for (int j = 0; j < Dimension; j++)
                {
                    m_DirectionMatrix[j * Dimension + j] = 1.0;  // unit matrix
                }
            }
            #endregion
        }
    }
}