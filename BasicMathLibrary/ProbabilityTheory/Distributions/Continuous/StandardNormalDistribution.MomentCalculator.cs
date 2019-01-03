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

namespace Dodoni.MathLibrary.ProbabilityTheory.Distributions
{
    public partial class StandardNormalDistribution
    {
        /// <summary>Provides methods for calculation of specific moments with respect to a Normal distribution.
        /// </summary>
        /// <remarks>The implementation is based on "Moments and Absolute Moments of the Normal Distribution", Andreas Winkelbauer, Institute of Telecommunications, Vienna University of Technology, 15.07.2014.</remarks>
        private class MomentCalculator : ProbabilityDistributionMoments
        {
            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="MomentCalculator" /> class.
            /// </summary>
            internal MomentCalculator()
                : base(maximalMomentDegree: Int32.MaxValue)
            {
            }
            #endregion

            #region public properties

            /// <summary>Gets the expectation, i.e. the first moment, or <see cref="System.Double.NaN" /> if the first moment does not exist.
            /// </summary>
            /// <value>The expectation, i.e. the first moment, or <see cref="System.Double.NaN" /> if the first moment does not exist.</value>
            public override double Expectation
            {
                get { return 0.0; }
            }

            /// <summary>Gets the variance, i.e. the second central moment, thus \sigma^2.
            /// </summary>
            /// <value>The variance, i.e. the second central moment, or <see cref="System.Double.NaN" /> if the second central moment does not exit.</value>
            public override double Variance
            {
                get { return 1.0; }
            }

            /// <summary>Gets the standard deviation, i.e. the square-root of the <see cref="ProbabilityDistributionMoments.Variance"/>.
            /// </summary>
            /// <value>The standard deviation, i.e. the square-root of the <see cref="ProbabilityDistributionMoments.Variance"/>.</value>
            public override double StandardDeviation
            {
                get { return 1.0; }
            }

            /// <summary>Gets the skewness (3-th central moment over the 3-th power of the standard deviation) or <see cref="System.Double.NaN" /> if the skewness is not well-defined.
            /// </summary>
            /// <value>The skewness or <see cref="System.Double.NaN" /> if the skewness is not well-defined.</value>
            public override double Skewness
            {
                get { return 0.0; }
            }

            /// <summary>Gets the kurtosis (Excess Kurtosis; 4-th central moment over the 4-th power of the standard deviation) or <see cref="System.Double.NaN" /> if the kurtosis is not well-defined.
            /// </summary>
            /// <value>The kurtosis (Excess Kurtosis) or <see cref="System.Double.NaN" /> if the kurtosis is not well-defined.</value>
            public override double Kurtosis
            {
                get { return 3.0; }
            }
            #endregion

            #region public methods

            /// <summary>Gets the n-th moment, i.e. E[X^n], where E is the expectation operator.
            /// </summary>
            /// <param name="order">The order of the moment.</param>
            /// <returns>The value of the n-th moment, i.e. E[X^n], where E is the expectation operator.</returns>
            public override double GetValue(int order)
            {
                /* see for example "Moments and Absolute Moments of the Normal Distribution", Andreas Winkelbauer, Institute of Telecommunications, Vienna University of Technology, 15.07.2014.
                 */
                if (order == 0)
                {
                    return 1.0;
                }
                else if (order % 2 == 1)  // if odd order
                {
                    return 0.0;
                }
                return SpecialFunction.Gamma.DoubleFactorial[order - 1];
            }

            /// <summary>Gets the absolute n-th moment, i.e. E[|X|^n], where E is the expectation operator.
            /// </summary>
            /// <param name="order">The order of the moment.</param>
            /// <returns>The value of the absolute n-th moment, i.e. E[|X|^n], where E is the expectation operator.</returns>
            public override double GetAbsValue(int order)
            {
                /* see for example "Moments and Absolute Moments of the Normal Distribution", Andreas Winkelbauer, Institute of Telecommunications, Vienna University of Technology, 15.07.2014.
                 */
                if (order == 0)
                {
                    return 1.0;
                }
                else if (order % 2 == 1) // if odd order
                {
                    return SpecialFunction.Gamma.DoubleFactorial[order - 1] / MathConsts.SqrtPiOverTwo;
                }
                return SpecialFunction.Gamma.DoubleFactorial[order - 1];
            }

            /// <summary>Gets the n-th central moment, i.e. E[(X- E[X])^n], where E is the expectation operator.
            /// </summary>
            /// <param name="order">The order of the central moment.</param>
            /// <returns>The value of the n-th central moment, i.e. E[(X- E[X])^n], where E is the expectation operator.</returns>
            public override double GetCentralValue(int order)
            {
                return GetValue(order);  // it holds E[X] = 0.0
            }

            /// <summary>Gets the absolute n-th central moment, i.e. E[|X- E[X]|^n], where E is the expectation operator.
            /// </summary>
            /// <param name="order">The order of the central moment.</param>
            /// <returns>The value of the absolute n-th central moment, i.e. E[|X- E[X]|^n], where E is the expectation operator.</returns>
            public override double GetAbsCentralValue(int order)
            {
                return GetAbsValue(order);  // it holds E[X] = 0.0
            }

            /// <summary>Gets a specific value of the moment-generating function E[exp(t*X)]
            /// </summary>
            /// <param name="t">The argument where to evaluate.</param>
            /// <param name="value">The specified value of the moment-generating function (output).</param>
            /// <returns>A value indicating whether <paramref name="value" /> contains valid data.</returns>
            public override bool TryGetMgfValue(double t, out double value)
            {
                value = Math.Exp(0.5 * t * t);
                return true;
            }
            #endregion
        }
    }
}