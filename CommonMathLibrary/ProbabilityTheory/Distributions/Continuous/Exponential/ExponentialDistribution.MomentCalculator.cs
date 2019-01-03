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

using Dodoni.BasicComponents;
using Dodoni.MathLibrary.NumericalIntegrators;

namespace Dodoni.MathLibrary.ProbabilityTheory.Distributions
{
    public partial class ExponentialDistribution
    {
        /// <summary>Provides methods for calculation of specific moments with respect to a Exponential distribution.
        /// </summary>
        private class MomentCalculator : ProbabilityDistributionMoments
        {
            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="MomentCalculator" /> class.
            /// </summary>
            /// <param name="distribution">The Exponential distribution in its <see cref="ExponentialDistribution"/> representation.</param>
            internal MomentCalculator(ExponentialDistribution distribution)
                : base(Int32.MaxValue)
            {
                Distribution = distribution;
                Integrator = new GaussLaguerreConstAbscissaIntegrator(order: 250);
            }
            #endregion

            #region private properties

            /// <summary>Gets the <see cref="ExponentialDistribution"/> representation.
            /// </summary>
            /// <value>The <see cref="ExponentialDistribution"/> representation.</value>
            private ExponentialDistribution Distribution
            {
                get;
                set;
            }

            /// <summary>Gets the <see cref="GaussLaguerreConstAbscissaIntegrator"/> object used for the calculation of absolute n-th central moments. 
            /// </summary>
            /// <value>The <see cref="GaussLaguerreConstAbscissaIntegrator"/> object used for the calculation of absolute n-th central moments.</value>
            private GaussLaguerreConstAbscissaIntegrator Integrator
            {
                get;
                set;
            }
            #endregion

            #region public properties

            /// <summary>Gets the sample mean, i.e. the first moment.
            /// </summary>
            /// <value>The sample mean, i.e. the first moment.</value>
            public override double Expectation
            {
                get { return Distribution.Beta; }
            }

            /// <summary>Gets the sample variance, i.e. the second central moment.
            /// </summary>
            /// <value>The sample variance.</value>
            public override double Variance
            {
                get { return Distribution.Beta * Distribution.Beta; }
            }
            #endregion

            #region public methods

            /// <summary>Gets the n-th moment, i.e. E[X^n], where E is the expectation operator.
            /// </summary>
            /// <param name="order">The order of the moment.</param>
            /// <returns>The value of the n-th moment, i.e. E[X^n], where E is the expectation operator.</returns>
            public override double GetValue(int order)
            {
                /* The expectation is equal to n! / \lambda^n  = n! * \beta^n */
                var value = 1.0;
                for (int j = 1; j <= order; j++)
                {
                    value *= j * Distribution.Beta;
                }
                return value;
            }

            /// <summary>Gets the absolute n-th moment, i.e. E[|X|^n], where E is the expectation operator.
            /// </summary>
            /// <param name="order">The order of the moment.</param>
            /// <returns>The value of the absolute n-th moment, i.e. E[|X|^n], where E is the expectation operator.</returns>
            public override double GetAbsValue(int order)
            {
                return GetValue(order); // already positive
            }

            /// <summary>Gets the n-th central moment, i.e. E[(X- E[X])^n], where E is the expectation operator.
            /// </summary>
            /// <param name="order">The order of the central moment.</param>
            /// <returns>The value of the n-th central moment, i.e. E[(X- E[X])^n], where E is the expectation operator.</returns>
            public override double GetCentralValue(int order)
            {
                if (order < 0)
                {
                    throw new ArgumentOutOfRangeException("order", String.Format(ExceptionMessages.ArgumentOutOfRangeGreaterEqual, "order", 0));
                }
                else if (order == 0)
                {
                    return 1.0;
                }
                else
                {
                    /* It holds E[ (X-EX)^n ] = \Gamma(n+1,-1) / (e * \lambda^n), where \Gamma is the incomplete Gamma-Function. We have
                     *                        = !n * \beta^n, where !n is the subfactorial, because !n = \Gamma(n+1,-1)/e
                     * The n-th subfactorial (derangement number) is the number of permutations of n objects in which no object appears in its natural place (i.e. derangements). 
                     * We have !n = nearestInteger( n! / e ), i.e. 0, 1, 2,9,44, 265, 1854, 14833
                     */
                    var powerOfBeta = 1.0;  // = \beta^n

                    var factorial = 1.0;  // = n!
                    for (int j = 1; j <= order; j++)
                    {
                        factorial *= j;
                        powerOfBeta *= Distribution.Beta;
                    }
                    return Math.Round(factorial / Math.E) * powerOfBeta;
                }
            }

            /// <summary>Gets the absolute n-th central moment, i.e. E[|X- E[X]|^n], where E is the expectation operator.
            /// </summary>
            /// <param name="order">The order of the central moment.</param>
            /// <returns>The value of the absolute n-th central moment, i.e. E[|X- E[X]|^n], where E is the expectation operator.</returns>
            /// <remarks>The implementation is based on a numerical integral approach (Gauss-Laguerre).</remarks>
            public override double GetAbsCentralValue(int order)
            {
                var algorithm = Integrator.Create();
                algorithm.FunctionToIntegrate = (xk, k) => DoMath.Pow(Math.Abs(xk - 1.0), order); // |\beta * x - \beta|^n = \beta^n * |x-1.0|^n

                return algorithm.GetValue() * DoMath.Pow(Distribution.Beta, order);
            }

            /// <summary>Gets a specific value of the moment-generating function E[exp(t*X)]
            /// </summary>
            /// <param name="t">The argument where to evaluate.</param>
            /// <param name="value">The specified value of the moment-generating function (output).</param>
            /// <returns>A value indicating whether <paramref name="value" /> contains valid data.</returns>
            public override bool TryGetMgfValue(double t, out double value)
            {
                if (t < Distribution.Lambda)
                {
                    value = Distribution.Lambda / (Distribution.Lambda - t);
                    return true;
                }
                value = Double.NaN;
                return false;
            }
            #endregion

            public override double StandardDeviation
            {
                get { throw new NotImplementedException(); }
            }

            public override double Skewness
            {
                get { throw new NotImplementedException(); }
            }

            public override double Kurtosis
            {
                get { throw new NotImplementedException(); }
            }
        }
    }
}