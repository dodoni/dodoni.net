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
    public partial class NormalDistribution
    {
        /// <summary>Provides methods for calculation of specific moments with respect to a Normal distribution.
        /// </summary>
        /// <remarks>The implementation is based on "Moments and Absolute Moments of the Normal Distribution", Andreas Winkelbauer, Institute of Telecommunications, Vienna University of Technology, 15.07.2014.</remarks>
        private class MomentCalculator : ProbabilityDistributionMoments
        {
            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="MomentCalculator" /> class.
            /// </summary>
            /// <param name="normalDistribution">The normal distribution.</param>
            internal MomentCalculator(NormalDistribution normalDistribution)
                : base(Int32.MaxValue)
            {
                Mu = normalDistribution.Mu;
                Sigma = normalDistribution.Sigma;
            }
            #endregion

            #region private properties

            /// <summary>Gets the mean of the normal distribution.
            /// </summary>
            /// <value>The mean of the normal distribution.</value>
            private double Mu
            {
                get;
                set;
            }

            /// <summary>Gets the standard deviation of the normal distribution.
            /// </summary>
            /// <value>The standard deviation of the normal distribution.</value>
            private double Sigma
            {
                get;
                set;
            }
            #endregion

            #region public properties

            /// <summary>Gets the expectation, i.e. the first moment, or <see cref="System.Double.NaN" /> if the first moment does not exist.
            /// </summary>
            /// <value>The expectation, i.e. the first moment, or <see cref="System.Double.NaN" /> if the first moment does not exist.</value>
            public override double Expectation
            {
                get { return Mu; }
            }

            /// <summary>Gets the variance, i.e. the second central moment, or <see cref="System.Double.NaN" /> if the second central moment does not exit.
            /// </summary>
            /// <value>The variance, i.e. the second central moment, or <see cref="System.Double.NaN" /> if the second central moment does not exit.</value>
            public override double Variance
            {
                get { return Sigma * Sigma; }
            }

            /// <summary>Gets the standard deviation, i.e. the square-root of the <see cref="ProbabilityDistributionMoments.Variance" />.
            /// </summary>
            /// <value>The standard deviation, i.e. the square-root of the <see cref="ProbabilityDistributionMoments.Variance" />.</value>
            public override double StandardDeviation
            {
                get { return Sigma; }
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
                /* It holds
                 *  E[X^n] = E[ (\sigma * Z + \mu)^n] = \sum_{k=0}^n (n over k) * \sigma^k * E[Z^k] * \mu^{n-k}
                 *    =\sum_{j=0}^{n/2} (n over 2 * j) * (2j-1)!! * \sigma^{2j} * \mu^{n-2*j}
                 *    
                 *  For higher order we use a representation with respect to the hypergeometric function, see "Moments and Absolute Moments of the Normal Distribution", Andreas Winkelbauer, Institute of Telecommunications, Vienna University of Technology, 15.07.2014.
                 */
                switch (order)
                {
                    case 0:
                        return 1.0;
                    case 1:
                        return Mu;
                    case 2:
                        return Mu * Mu + Sigma * Sigma;
                    case 3:
                        return Mu * Mu * Mu + 3.0 * Mu * Sigma * Sigma;
                    case 4:
                        return DoMath.Pow(Mu, 4) + 6.0 * Mu * Mu * Sigma * Sigma + 3.0 * DoMath.Pow(Sigma, 4);
                    case 5:
                        return DoMath.Pow(Mu, 5) + 10.0 * Mu * Mu * Mu * Sigma * Sigma + 15.0 * Mu * DoMath.Pow(Sigma, 4);
                    case 6:
                        return DoMath.Pow(Mu, 6) + 15 * DoMath.Pow(Mu, 4) * Sigma * Sigma + 45 * Mu * Mu * DoMath.Pow(Sigma, 4) + 15 * DoMath.Pow(Sigma, 6);
                    case 7:
                        return DoMath.Pow(Mu, 7) + 21 * DoMath.Pow(Mu, 5) * Sigma * Sigma + 105 * DoMath.Pow(Mu, 3) * DoMath.Pow(Sigma, 4) + 105 * Mu * DoMath.Pow(Sigma, 6);
                    case 8:
                        return DoMath.Pow(Mu, 8) + 28 * DoMath.Pow(Mu, 6) * Sigma * Sigma + 210 * DoMath.Pow(Mu, 4) * DoMath.Pow(Sigma, 4) + 420 * Mu * Mu * DoMath.Pow(Sigma, 6) + 105 * DoMath.Pow(Sigma, 8);
                    case 9:
                        return DoMath.Pow(Mu, 9) + 36 * DoMath.Pow(Mu, 7) * Sigma * Sigma + 378 * DoMath.Pow(Mu, 5) * DoMath.Pow(Sigma, 4) + 1260 * DoMath.Pow(Mu, 3) * DoMath.Pow(Sigma, 6) + 945 * Mu * DoMath.Pow(Sigma, 8);
                    case 10:
                        return DoMath.Pow(Mu, 10) + 45 * DoMath.Pow(Mu, 8) * Sigma * Sigma + 630 * DoMath.Pow(Mu, 6) * DoMath.Pow(Sigma, 4) + 3150 * DoMath.Pow(Mu, 4) * DoMath.Pow(Sigma, 6) + 4725 * Mu * Mu * DoMath.Pow(Sigma, 8) + 945 * DoMath.Pow(Sigma, 10);
                    default:
                        if (order % 2 == 0)
                        {
                            return Math.Exp(order * Math.Log(Sigma) + order / 2.0 * MathConsts.Log2 + SpecialFunction.Gamma.GetLogValue((order + 1) / 2.0)) * MathConsts.OneOverSqrtPi * SpecialFunction.Hypergeometric.One_F_One(-order / 2.0, 0.5, -Mu * Mu / (2 * Sigma * Sigma));
                        }
                        else
                        {
                            return Mu * Math.Exp((order - 1) * Math.Log(Sigma) + (order + 1) / 2.0 * MathConsts.Log2 + SpecialFunction.Gamma.GetLogValue((order / 2.0 + 1))) * MathConsts.OneOverSqrtPi * SpecialFunction.Hypergeometric.One_F_One((1 - order) / 2.0, 1.5, -Mu * Mu / (2 * Sigma * Sigma));
                        }
                }
            }

            /// <summary>Gets the absolute n-th moment, i.e. E[|X|^n], where E is the expectation operator.
            /// </summary>
            /// <param name="order">The order of the moment.</param>
            /// <returns>The value of the absolute n-th moment, i.e. E[|X|^n], where E is the expectation operator.</returns>
            public override double GetAbsValue(int order)
            {
                // see "Moments and Absolute Moments of the Normal Distribution", Andreas Winkelbauer, Institute of Telecommunications, Vienna University of Technology, 15.07.2014.
                return Math.Exp(order * Math.Log(Sigma) + order / 2.0 * MathConsts.Log2 + SpecialFunction.Gamma.GetLogValue((order + 1) / 2.0)) * MathConsts.OneOverSqrtPi * SpecialFunction.Hypergeometric.One_F_One(-order / 2.0, 0.5, -Mu * Mu / (2 * Sigma * Sigma));
            }

            /// <summary>Gets the n-th central moment, i.e. E[(X- E[X])^n], where E is the expectation operator.
            /// </summary>
            /// <param name="order">The order of the central moment.</param>
            /// <returns>The value of the n-th central moment, i.e. E[(X- E[X])^n], where E is the expectation operator.</returns>
            public override double GetCentralValue(int order)
            {
                if (order == 0)
                {
                    return 1;
                }
                if (order % 2 == 1)
                {
                    return 0.0;
                }
                else
                {
                    /* it holds: CentralMoment_n = (n-1)!! * \sigma^n, where (n-1)!! := (n-1) *(n-3) ... 3 * 1 */
                    var value = DoMath.Pow(Sigma, order);
                    int n = 3;
                    while (n <= order - 1)
                    {
                        value *= n;
                        n += 2;
                    }
                    return value;
                }
            }

            /// <summary>Gets the absolute n-th central moment, i.e. E[|X- E[X]|^n], where E is the expectation operator.
            /// </summary>
            /// <param name="order">The order of the central moment.</param>
            /// <returns>The value of the absolute n-th central moment, i.e. E[|X- E[X]|^n], where E is the expectation operator.</returns>
            public override double GetAbsCentralValue(int order)
            {
                // see "Moments and Absolute Moments of the Normal Distribution", Andreas Winkelbauer, Institute of Telecommunications, Vienna University of Technology, 15.07.2014.
                return Math.Exp(order * Math.Log(Sigma) + order / 2.0 * MathConsts.Log2 + SpecialFunction.Gamma.GetLogValue((order + 1) / 2.0)) * MathConsts.OneOverSqrtPi;
            }

            /// <summary>Gets a specific value of the moment-generating function E[exp(t*X)]
            /// </summary>
            /// <param name="t">The argument where to evaluate.</param>
            /// <param name="value">The specified value of the moment-generating function (output).</param>
            /// <returns>A value indicating whether <paramref name="value" /> contains valid data.</returns>
            public override bool TryGetMgfValue(double t, out double value)
            {
                value = Math.Exp(Mu * t + 0.5 * Sigma * Sigma * t * t);
                return true;
            }
            #endregion
        }
    }
}