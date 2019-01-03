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

namespace Dodoni.MathLibrary.ProbabilityTheory.Distributions
{
    internal partial class StandardEmpiricalMomentEstimator
    {
        /// <summary>Provides methods for the calculation of (raw, central etc.) moments with respect to the empirical probability distribution of a specified sample.
        /// </summary>
        private class Algorithm : ProbabilityDistributionMoments
        {
            #region private members

            private double m_Mean;
            private double m_Variance;
            private double m_Skewness;
            private double m_Kurtosis;
            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="Algorithm" /> class.
            /// </summary>
            /// <param name="sample">The sample.</param>
            /// <param name="sampleSize">The sample size.</param>
            /// <param name="mean">The (pre-calculated) mean of the sample.</param>
            /// <param name="variance">The (pre-calculated) variance of the sample.</param>
            /// <param name="skewness">The skewness of the sample.</param>
            /// <param name="kurtosis">The kurtosis of the sample.</param>
            internal Algorithm(IEnumerable<double> sample, long sampleSize, double mean, double variance, double skewness, double kurtosis)
                : base(maximalMomentDegree: Int32.MaxValue)
            {
                Sample = sample;
                SampleSize = sampleSize;

                m_Mean = mean; // =  DoMath.Average(sample);
                m_Variance = variance; // =  GetVariance(sample);
                m_Skewness = skewness;
                m_Kurtosis = kurtosis;
            }
            #endregion

            #region public properties

            /// <summary>Gets the (original) sample.
            /// </summary>
            /// <value>The (original) sample.</value>
            public IEnumerable<double> Sample
            {
                get;
                private set;
            }

            /// <summary>Gets the sample size, i.e. the number of elements of <see cref="Sample"/>.
            /// </summary>
            /// <value>The sample size, i.e. the number of elements of <see cref="Sample"/>.</value>
            public long SampleSize
            {
                get;
                private set;
            }

            /// <summary>Gets the sample mean, i.e. the first moment.
            /// </summary>
            /// <value>The sample mean, i.e. the first moment.</value>
            public override double Expectation
            {
                get { return m_Mean; }
            }

            /// <summary>Gets the sample variance, i.e. the second central moment.
            /// </summary>
            /// <value>The sample variance.</value>
            public override double Variance
            {
                get { return m_Variance; }
            }

            /// <summary>Gets the standard deviation, i.e. the square-root of the <see cref="ProbabilityDistributionMoments.Variance"/>.
            /// </summary>
            /// <value>The standard deviation, i.e. the square-root of the <see cref="ProbabilityDistributionMoments.Variance"/>.</value>
            public override double StandardDeviation
            {
                get { return Math.Sqrt(m_Variance); }
            }

            /// <summary>Gets the skewness (3-th central moment over the 3-th power of the standard deviation) or <see cref="System.Double.NaN" /> if the skewness is not well-defined.
            /// </summary>
            /// <value>The skewness or <see cref="System.Double.NaN" /> if the skewness is not well-defined.</value>
            public override double Skewness
            {
                get { return m_Skewness; }
            }

            /// <summary>Gets the kurtosis (Excess Kurtosis; 4-th central moment over the 4-th power of the standard deviation) or <see cref="System.Double.NaN" /> if the kurtosis is not well-defined.
            /// </summary>
            /// <value>The kurtosis (Excess Kurtosis) or <see cref="System.Double.NaN" /> if the kurtosis is not well-defined.</value>
            public override double Kurtosis
            {
                get { return m_Kurtosis; }
            }
            #endregion

            #region public methods

            /// <summary>Gets the n-th moment, i.e. E[X^n], where E is the expectation operator.
            /// </summary>
            /// <param name="order">The order of the moment.</param>
            /// <returns>The value of the n-th moment, i.e. E[X^n], where E is the expectation operator.</returns>
            public override double GetValue(int order)
            {
                return DoMath.Average(Sample.Select(x => DoMath.Pow(x, order)));
            }

            /// <summary>Gets the n-th central moment, i.e. E[(X- E[X])^n], where E is the expectation operator.
            /// </summary>
            /// <param name="order">The order of the central moment.</param>
            /// <returns>The value of the n-th central moment, i.e. E[(X- E[X])^n], where E is the expectation operator.</returns>
            public override double GetCentralValue(int order)
            {
                switch (order)
                {
                    case 0:
                        return 1.0;

                    case 1:
                        return 0.0;

                    case 2:
                        return Variance;

                    case 3: /* the unbiased estimator is taken from "Optimal unbiased estimation of some population central moments", M. R. Espejo, M. D. Pineda, S. Nadarajah, Metron (2013) 71, page 39-63 */
                        long n = SampleSize;

                        return n * n / (n * n - 3.0 * n + 2.0) * (GetValue(3) - 3.0 * GetValue(2) * Expectation + 2.0 * Expectation * Expectation * Expectation);

                    case 4: /* the unbiased estimator is taken from "Optimal unbiased estimation of some population central moments", M. R. Espejo, M. D. Pineda, S. Nadarajah, Metron (2013) 71, page 39-63 */
                        double m = SampleSize;
                        double denominator = (m - 3.0) * (m - 2.0) * (m - 1.0);

                        var alpha2 = GetValue(2);
                        var alpha3 = GetValue(3);
                        var alpha4 = GetValue(4);

                        var centralValue4 = -3 * DoMath.Pow(m, 3) * DoMath.Pow(Expectation, 4) / denominator;
                        centralValue4 += 6 * DoMath.Pow(m, 3) * Expectation * Expectation * alpha2 / denominator;
                        centralValue4 += (9 - 6 * m) * m * alpha2 * alpha2 / denominator;

                        centralValue4 += (-12 + 8 * m - 4 * m * m) * m * Expectation * alpha3 / denominator;
                        centralValue4 += alpha4 * (3 * m - 2 * m * m + m * m * m) / denominator;
                        return centralValue4;

                    default:
                        throw new NotImplementedException();  // the following estimator is not unbiased: DoMath.Sum(Sample.Select(x => DoMath.Pow(x - Mean, order)));
                }
            }

            /// <summary>Gets the absolute n-th moment, i.e. E[|X|^n], where E is the expectation operator.
            /// </summary>
            /// <param name="order">The order of the moment.</param>
            /// <returns>The value of the absolute n-th moment, i.e. E[|X|^n], where E is the expectation operator.</returns>
            public override double GetAbsValue(int order)
            {
                return DoMath.Average(Sample.Select(x => DoMath.Pow(Math.Abs(x), order)));
            }

            /// <summary>Gets the absolute n-th central moment, i.e. E[|X- E[X]|^n], where E is the expectation operator.
            /// </summary>
            /// <param name="order">The order of the central moment.</param>
            /// <returns>The value of the absolute n-th central moment, i.e. E[|X- E[X]|^n], where E is the expectation operator.</returns>
            public override double GetAbsCentralValue(int order)
            {
                throw new NotImplementedException();

                // die nachfolgende Implementierung ist kein erwartungstreuer schätzer!

                return DoMath.Average(Sample.Select(x => DoMath.Pow(Math.Abs(x - Expectation), order)));
            }

            /// <summary>Gets a specific value of the moment-generating function E[exp(t*X)]
            /// </summary>
            /// <param name="t">The argument where to evaluate.</param>
            /// <param name="value">The specified value of the moment-generating function (output).</param>
            /// <returns>A value indicating whether <paramref name="value" /> contains valid data.</returns>
            public override bool TryGetMgfValue(double t, out double value)
            {
                value = DoMath.Average(Sample.Select(x => Math.Exp(t * x)));
                return true;
            }
            #endregion

            #region public static methods

            /// <summary>Gets a numerical stable implementation for the calculation of the variance.
            /// </summary>
            /// <param name="sample">The sample.</param>
            /// <returns>The estimated variance.</returns>
            /// <remarks>The implementation is based on 'Numerische Mathematik 1', Tomas Sauer, 2005, Uni Giessen.</remarks>
            [Obsolete]
            public static double GetVariance(IEnumerable<double> sample)
            {
                if (sample == null)
                {
                    throw new ArgumentNullException("sample");
                }
                var m = 0.0;
                var q = 0.0;

                long i = 1;
                foreach (var value in sample)
                {
                    q += (i - 1) * (value - m) * (value - m) / i;
                    m += (value - m) / i;
                    i++;
                }
                if (i == 2)  // sample size is 0
                {
                    return 0.0;
                }
                return q / (i - 2);
            }
            #endregion
        }
    }
}