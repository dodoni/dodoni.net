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
using System.Text;

using Dodoni.BasicComponents;
using System.Collections.Generic;

namespace Dodoni.MathLibrary.ProbabilityTheory
{
    /// <summary>A collection of methods concerning moments and variance of samples.
    /// </summary>
    [Obsolete]
    public static class BasicStatistics
    {
        /// <summary>Computes the average of a sequence of <see cref="System.Double"/> values.
        /// </summary>
        /// <param name="source">A sequence of System.Double values to calculate the average of.</param>
        /// <param name="sampleSize">The sample size.</param>
        /// <returns>The average of the sequence of values.</returns>
        public static double Average(this IList<double> source, int sampleSize)
        {
            var value = 0.0;
            for (int k = 0; k < sampleSize; k++)
            {
                value += source[k];
            }
            return value / sampleSize;
        }

        /// <summary>Gets the specified moment of some sample.
        /// </summary>
        /// <param name="sample">The sample.</param>
        /// <param name="sampleLength">The number of relevant elements of <paramref name="sample"/> to take into account.</param>
        /// <param name="degree">The degree of the moment.</param>
        /// <returns>The moment of degree <paramref name="degree"/> with respect to the <paramref name="sample"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="sample"/> is null.</exception>
        public static double NthMoment(this double[] sample, int sampleLength, int degree)
        {
            if (sample == null)
            {
                throw new ArgumentNullException("sample", string.Format(null, ExceptionMessages.ArgumentNull, "sample"));
            }
            double returnValue = 0.0;

            for (int j = 0; j < sampleLength; j++)
            {
                returnValue += DoMath.Pow(sample[j], degree);
            }
            return returnValue / sampleLength;
        }

        /// <summary>Gets the variance of same sample.
        /// </summary>
        /// <param name="sample">The sample.</param>
        /// <param name="sampleLength">The number of relevant elements of <paramref name="sample"/> to take into account.</param>
        /// <returns>The variance of the <paramref name="sample"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="sample"/> is <c>null</c>.</exception>
        public static double Variance(this double[] sample, int sampleLength)
        {
            return Variance(sample, sampleLength, NthMoment(sample, sampleLength, 1));
        }

        /// <summary>Gets the standard deviation of the elements of a specified list of <see cref="System.Double"/>.
        /// </summary>
        /// <param name="sample">The sample.</param>
        /// <param name="sampleLength">The number of relevant elements of <paramref name="sample"/> to take into account.</param>
        /// <returns>The standard deviation of the <paramref name="sample"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="sample"/> is <c>null</c>.</exception>
        public static double StandardDeviation(this double[] sample, int sampleLength)
        {
            return Math.Sqrt(Variance(sample, sampleLength, NthMoment(sample, sampleLength, 1)));
        }

        /// <summary>Gets the covariance between two samples.
        /// </summary>
        /// <param name="sample1">The first sample.</param>
        /// <param name="sample2">The second sample.</param>
        /// <param name="sampleLength">The number of relevant elements of <paramref name="sample1"/> and <paramref name="sample2"/> to take into account.</param>
        /// <returns>The covariance between <paramref name="sample1"/> and <paramref name="sample2"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="sample1"/>  or <paramref name="sample2"/> is <c>null</c>.</exception>
        public static double Covariance(double[] sample1, double[] sample2, int sampleLength)
        {
            return Covariance(sample1, sample2, sampleLength, NthMoment(sample1, sampleLength, 1), NthMoment(sample2, sampleLength, 1));
        }

        /// <summary>Gets the correlation coefficient between two samples.
        /// </summary>
        /// <param name="sample1">The first sample.</param>
        /// <param name="sample2">The second sample.</param>
        /// <param name="sampleLength">The number of relevant elements of <paramref name="sample1"/> and <paramref name="sample2"/> to take into account.</param>
        /// <returns>The correlation between the two samples.</returns>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="sample1"/> or <paramref name="sample2"/> is <c>null</c>.</exception>
        public static double Correlation(double[] sample1, double[] sample2, int sampleLength)
        {
            double expectedValue1 = NthMoment(sample1, sampleLength, 1);
            double expectedValue2 = NthMoment(sample2, sampleLength, 1);
            double variance1 = Variance(sample1, sampleLength, expectedValue1);
            double variance2 = Variance(sample2, sampleLength, expectedValue2);

            return Covariance(sample1, sample2, sampleLength, expectedValue1, expectedValue2) / Math.Sqrt(variance1 * variance2);
        }

        /// <summary>Gets the correlation coefficient between two samples.
        /// </summary>
        /// <param name="sample1">The first sample.</param>
        /// <param name="sample2">The second sample.</param>
        /// <param name="sampleLength">The number of relevant elements of <paramref name="sample1"/> and <paramref name="sample2"/> to take into account.</param>
        /// <param name="expectedValue1">The expected value of <paramref name="sample1"/>.</param>
        /// <param name="expectedValue2">The expected value of <paramref name="sample2"/>.</param>
        /// <returns>The correlation between <paramref name="sample1"/> and <paramref name="sample2"/>.</returns>
        public static double Correlation(double[] sample1, double[] sample2, int sampleLength, double expectedValue1, double expectedValue2)
        {
            double variance1 = Variance(sample1, sampleLength, expectedValue1);
            double variance2 = Variance(sample2, sampleLength, expectedValue2);

            return Covariance(sample1, sample2, sampleLength, expectedValue1, expectedValue2) / Math.Sqrt(variance1 * variance2);
        }

        /// <summary>Gets the variances of some sample.
        /// </summary>
        /// <param name="sample">The sample.</param>
        /// <param name="sampleLength">The number of relevant elements of <paramref name="sample"/> to take into account.</param>
        /// <param name="expectedValue">The expected value.</param>
        /// <returns>The variance between with respect to the <paramref name="sample"/> where the expectation is already calculated before.</returns>
        public static double Variance(double[] sample, int sampleLength, double expectedValue)
        {
            return Covariance(sample, sample, sampleLength, expectedValue, expectedValue);
        }

        /// <summary>Gets the covariance of two samples.
        /// </summary>
        /// <param name="sample1">The first sample.</param>
        /// <param name="sample2">The second sample.</param>
        /// <param name="sampleLength">The number of relevant elements of <paramref name="sample1"/> and <paramref name="sample2"/> to take into account.</param>
        /// <param name="expectedValue1">The expected value of <paramref name="sample1"/>.</param>
        /// <param name="expectedValue2">The expected value of <paramref name="sample2"/>.</param>
        /// <returns>The covariance of <paramref name="sample1"/> and <paramref name="sample2"/>, where the expectations are already calculated.</returns>
        public static double Covariance(double[] sample1, double[] sample2, int sampleLength, double expectedValue1, double expectedValue2)
        {
            double value = 0.0;
            for (int i = 0; i < sampleLength; i++)
            {
                value += (sample1[i] - expectedValue1) * (sample2[i] - expectedValue2);
            }
            return value / (sampleLength - 1);
        }
    }
}