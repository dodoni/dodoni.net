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
    public partial class ShiftedLogNormalDistribution
    {
        /// <summary>Provides methods for calculation of specific moments with respect to a Shifted Normal distribution.
        /// </summary>
        private class MomentCalculator : ProbabilityDistributionMoments
        {
            #region private members

            private double m_Mean;
            private double m_Variance;
            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="MomentCalculator" /> class.
            /// </summary>
            /// <param name="logNormalDistribution">The log-normal distribution.</param>
            internal MomentCalculator(ShiftedLogNormalDistribution logNormalDistribution)
                : base(Int32.MaxValue)
            {
                Distribution = logNormalDistribution;
                m_Mean = Math.Exp(logNormalDistribution.Mu + 0.5 * logNormalDistribution.Sigma * logNormalDistribution.Sigma);
                m_Variance = Math.Exp(2 * logNormalDistribution.Mu + logNormalDistribution.Sigma * logNormalDistribution.Sigma);
            }
            #endregion

            #region private properties

            /// <summary>Gets the distribution in its <see cref="ShiftedLogNormalDistribution"/> representation.
            /// </summary>
            /// <value>The distribution in its <see cref="ShiftedLogNormalDistribution"/> representation.</value>
            private ShiftedLogNormalDistribution Distribution
            {
                get;
                set;
            }

            /// <summary>Gets the mean of the log-normal distribution.
            /// </summary>
            /// <value>The mean of the log-normal distribution.</value>
            private double Mu
            {
                get { return Distribution.Mu; }
            }

            /// <summary>Gets the standard deviation of the log-normal distribution.
            /// </summary>
            /// <value>The standard deviation of the log-normal distribution.</value>
            private double Sigma
            {
                get { return Distribution.Sigma; }
            }

            /// <summary>Gets the shift parameter of the shifted log-normal distribution.
            /// </summary>
            /// <value>The shift parameter of the shifted log-normal distribution.</value>
            private double Shift
            {
                get { return Distribution.Shift; }
            }
            #endregion

            #region public properties

            public override double Expectation
            {
                get { return m_Mean; }
            }

            public override double Variance
            {
                get { return m_Variance; }
            }
            #endregion

            #region public methods

            /// <summary>Gets the n-th moment, i.e. E[X^n], where E is the expectation operator.
            /// </summary>
            /// <param name="order">The order of the moment.</param>
            /// <returns>The value of the n-th moment, i.e. E[X^n], where E is the expectation operator.</returns>
            public override double GetValue(int order)
            {
                // todo: Korrekt implementieren!
                return Math.Exp(order * Mu + 0.5 * order * order * Sigma * Sigma);  // E X^n = e^{n * \mu + 0.5 * n^2 * sigma^2}
            }

            public override double GetAbsValue(int order)
            {
                throw new NotImplementedException();
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
                throw new NotImplementedException();
            }

            public override double GetAbsCentralValue(int order)
            {
                throw new NotImplementedException();
            }

            /// <summary>Gets a specific value of the moment-generating function E[exp(t*X)]
            /// </summary>
            /// <param name="t">The argument where to evaluate.</param>
            /// <param name="value">The specified value of the moment-generating function (output).</param>
            /// <returns>A value indicating whether <paramref name="value" /> contains valid data.</returns>
            /// <remarks>The implementation is based on "On the Laplace transformation of the Lognormal distribution", S. Asmussen, J. L. Jensen, L. Rojas-Nandayapa.</remarks>
            public override bool TryGetMgfValue(double t, out double value)
            {
                if (t < 0)
                {
                    value = Double.NaN;
                    return false;
                }
                var w = SpecialFunction.IteratedExponential.LambertW(t * Sigma * Sigma * Distribution.m_ExpOfMu);

                value = Math.Exp(t * Shift - (w * w + 2 * w) / (2 * Sigma * Sigma)) / Math.Sqrt(1 + w);
                return true;
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