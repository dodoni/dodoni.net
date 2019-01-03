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

namespace Dodoni.MathLibrary.NumericalIntegrators
{
    public partial class AdaptiveGaussKronrodIntegrator
    {
        /// <summary>Provides exit condition for <see cref="AdaptiveGaussKronrodIntegrator"/> objects.
        /// </summary>
        public class StoppingCriteria : ExitCondition
        {
            #region public constructors

            /// <summary>Initializes a new instance of the <see cref="StoppingCriteria"/> class.
            /// </summary>
            /// <param name="maxIterations">The maximal number of iterations.</param>
            /// <param name="maxEvaluations">The maximal number of function evaluations.</param>
            /// <param name="tolerance">The tolerance to take into account as exit condition; or <see cref="System.Double.NaN"/>.</param>
            /// <param name="toleranceType">A value indicating how to interprete <paramref name="tolerance"/>.</param>
            public StoppingCriteria(int maxIterations, int maxEvaluations = Int32.MaxValue, double tolerance = Double.NaN, ToleranceType toleranceType = ToleranceType.None)
                : base(maxIterations, maxEvaluations, tolerance, toleranceType)
            {
            }

            /// <summary>Initializes a new instance of the <see cref="StoppingCriteria"/> class.
            /// </summary>
            /// <param name="maxIterations">The maximal number of iterations.</param>
            /// <param name="absoluteTolerance">A absolute tolerance.</param>
            /// <param name="relativeTolerance">A relative tolerance.</param>
            /// <param name="maxEvaluations">The maximal number of function evaluations.</param>
            public StoppingCriteria(int maxIterations, double absoluteTolerance, double relativeTolerance, int maxEvaluations = Int32.MaxValue)
                : base(maxIterations, absoluteTolerance, relativeTolerance, maxEvaluations)
            {
            }
            #endregion

            #region public methods

            /// <summary>Checks the convergence criterion for a given sub-integral.
            /// </summary>
            /// <param name="value">The value of the (sub-)integral.</param>
            /// <param name="benchmarkValue">The benchmark value of the (sub-)integral.</param>
            /// <param name="factor">A positive factor which is used for the convergence test, i.e. instead of decrease the absolute/relative tolerance, the values are multiplied with this factor.</param>
            /// <returns><c>true</c> if the estimated <paramref name="value"/> and the <paramref name="benchmarkValue"/> are small with respect to the specified convergence criterion; <c>false</c> otherwise.
            /// </returns>
            public bool CheckConvergenceCriterion(double value, double benchmarkValue, ulong factor)
            {
                if (Double.IsNaN(AbsoluteTolerance) == false)
                {
                    if (factor * Math.Abs(benchmarkValue - value) < AbsoluteTolerance)
                    {
                        return true;
                    }
                }

                if (Double.IsNaN(RelativeTolerance) == false)
                {
                    double estimatedRelativeError = factor * Math.Abs(benchmarkValue - value);
                    if (Math.Abs(benchmarkValue) > 0.0)
                    {
                        estimatedRelativeError /= Math.Abs(benchmarkValue);
                    }
                    if (estimatedRelativeError < RelativeTolerance)
                    {
                        return true;
                    }
                }
                return false;
            }
            #endregion

            #region public static methods

            /// <summary>Creates a new <see cref="StoppingCriteria"/> object.
            /// </summary>
            /// <param name="maxIterations">The maximal number of iterations.</param>
            /// <param name="maxEvaluations">The maximal number of function evaluations.</param>
            /// <param name="tolerance">The tolerance to take into account as exit condition; or <see cref="System.Double.NaN"/>.</param>
            /// <param name="toleranceType">A value indicating how to interprete <paramref name="tolerance"/>.</param>
            /// <returns>A specific <see cref="StoppingCriteria"/> object.</returns>
            public static new StoppingCriteria Create(int maxIterations, int maxEvaluations = Int32.MaxValue, double tolerance = Double.NaN, ToleranceType toleranceType = ToleranceType.None)
            {
                return new StoppingCriteria(maxIterations, maxEvaluations, tolerance, toleranceType);
            }

            /// <summary>Creates a new <see cref="StoppingCriteria"/> object.
            /// </summary>
            /// <param name="maxIterations">The maximal number of iterations.</param>
            /// <param name="absoluteTolerance">A absolute tolerance.</param>
            /// <param name="relativeTolerance">A relative tolerance.</param>
            /// <param name="maxEvaluations">The maximal number of function evaluations.</param>
            /// <returns>A specific <see cref="StoppingCriteria"/> object.</returns>
            public static new StoppingCriteria Create(int maxIterations, double absoluteTolerance, double relativeTolerance, int maxEvaluations = Int32.MaxValue)
            {
                return new StoppingCriteria(maxIterations, absoluteTolerance, relativeTolerance, maxEvaluations);
            }
            #endregion
        }
    }
}