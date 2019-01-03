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
using Dodoni.MathLibrary.ProbabilityTheory.Distributions.Empirical;

namespace Dodoni.MathLibrary.ProbabilityTheory.Distributions
{

    // achtung! hier ausschließlich erwartungstreue Schätzer einbauen und die Klassenbezeichnung ändern, da bereits in Basic Math vorhanden!
    /// <summary>Serves as factory for the standard empirical moment estimator.
    /// </summary>
    internal partial class StandardEmpiricalMomentEstimator : EmpiricalMomentEstimator
    {
        #region private members

        private IdentifierString m_Name;
        private IdentifierString m_LongName;
        private string m_Annotation;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="StandardEmpiricalMomentEstimator" /> class.
        /// </summary>
        internal StandardEmpiricalMomentEstimator()
        {
            m_Name = m_LongName = new IdentifierString("Standard empirical moment estimator");
            m_Annotation = String.Empty;
        }
        #endregion

        #region public properties

        /// <summary>Gets the name of the current instance.
        /// </summary>
        /// <value>The language independent name of the current instance.</value>
        public override IdentifierString Name
        {
            get { return m_Name; }
        }

        /// <summary>Gets the long name of the current instance.
        /// </summary>
        /// <value>The (perhaps) language dependent long name of the current instance.</value>
        public override IdentifierString LongName
        {
            get { return m_LongName; }
        }

        /// <summary>Gets a value indicating whether the annotation is read-only.
        /// </summary>
        /// <value><c>true</c> if the annotation of this instance is readonly; otherwise, <c>false</c>.</value>
        public override bool HasReadOnlyAnnotation
        {
            get { return true; }
        }

        /// <summary>Gets the annotation of the current instance.
        /// </summary>
        /// <value>The annotation of the current instance.</value>
        public override string Annotation
        {
            get { return m_Annotation; }
        }

        /// <summary>Gets a value indicating whether the moment estimator is a stochastic approach.
        /// </summary>
        /// <value><c>true</c> if this instance represents a moment estimator with a stochastic approach; otherwise, <c>false</c>.</value>
        public override bool IsStochasticApproach
        {
            get { return false; }
        }
        #endregion

        #region public methods

        /// <summary>Sets the annotation of the current instance.
        /// </summary>
        /// <param name="annotation">The annotation.</param>
        /// <returns>A value indicating whether the <see cref="Annotation" /> has been changed.</returns>
        public override bool TrySetAnnotation(string annotation)
        {
            return false;
        }

        /// <summary>Creates a moment estimator for a specific sample.
        /// </summary>
        /// <param name="sample">The sample.</param>
        /// <returns>A new moment calculator w.r.t. the empirical distribution in its <see cref="ProbabilityDistributionMoments" /> representation.</returns>
        public override ProbabilityDistributionMoments Create(IEnumerable<double> sample)
        {
            double skewness, kurtosis, variance, mean, min, max; // here we do not use minimum, maximum
            var sampleSize = GetMainVariables(sample, out mean, out min, out max, out variance, out skewness, out kurtosis);

            return new Algorithm(sample, sampleSize, mean, variance, skewness, kurtosis);
        }
        #endregion

        #region public static methods

        /// <summary>Gets the skewness, kurtosis, variance and mean of a specified sample.
        /// </summary>
        /// <param name="sample">The sample.</param>
        /// <param name="mean">The mean (output).</param>
        /// <param name="minimum">The minimum (output).</param>
        /// <param name="maximum">The maximum (output).</param>
        /// <param name="variance">The variance (output).</param>
        /// <param name="skewness">The skewness (output).</param>
        /// <param name="kurtosis">The kurtosis (output).</param>
        /// <returns>The sample size, i.e. the number of elements of <paramref name="sample"/>.</returns>
        /// <remarks>The implementation is based on http://en.wikipedia.org/wiki/Kurtosis,  http://en.wikipedia.org/wiki/Skewness and http://en.wikipedia.org/wiki/Algorithms_for_calculating_variance. </remarks>
        public static long GetMainVariables(IEnumerable<double> sample, out double mean, out double minimum, out double maximum, out double variance, out double skewness, out double kurtosis)
        {
            minimum = Double.MaxValue;
            maximum = Double.MinValue;
            long m = 0;
            mean = 0;
            var M2 = 0.0;
            var M3 = 0.0;
            var M4 = 0.0;

            foreach (var value in sample)
            {
                long m1 = m;
                m = m + 1;
                var delta = value - mean;
                var delta_m = delta / m;
                var delta_m2 = delta_m * delta_m;
                var term1 = delta * delta_m * m1;
                mean = mean + delta_m;
                M4 = M4 + term1 * delta_m2 * (m * m - 3 * m + 3) + 6 * delta_m2 * M2 - 4 * delta_m * M3;
                M3 = M3 + term1 * delta_m * (m - 2) - 3 * delta_m * M2;
                M2 = M2 + term1;

                if (value > maximum)
                {
                    maximum = value;
                }
                if (value < minimum)
                {
                    minimum = value;
                }
            }

            variance = M2 / (m - 1);
            kurtosis = m * (m + 1.0) / ((m - 1.0) * (m - 2.0) * (m - 3.0)) * M4 / (variance * variance) - 3.0 * (m - 1.0) * (m - 1.0) / ((m - 2.0) * (m - 3.0));

            M2 /= m;
            M3 /= m;
            skewness = Math.Sqrt(m * (m - 1)) / (m - 2) * M3 / Math.Pow(M2, 1.5);
            return m;
        }
        #endregion
    }
}