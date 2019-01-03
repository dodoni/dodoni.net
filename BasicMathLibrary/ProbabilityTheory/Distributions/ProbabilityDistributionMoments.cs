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

using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.ProbabilityTheory.Distributions
{
    /// <summary>Provides methods for the calculation of (raw, central etc.) moments of a specific probability distribution.
    /// </summary>
    public abstract class ProbabilityDistributionMoments : IInfoOutputQueriable
    {
        #region protected constructors

        /// <summary>Initializes a new instance of the <see cref="ProbabilityDistributionMoments" /> class.
        /// </summary>
        /// <param name="maximalMomentDegree">The maximal degree n for which the moment E[X^n] exist, perhaps <see cref="System.Int32.MaxValue"/>.</param>
        protected ProbabilityDistributionMoments(int maximalMomentDegree)
        {
            if (maximalMomentDegree < 0)
            {
                throw new ArgumentOutOfRangeException("maximalMomentDegree");
            }
            MaximalMomentDegree = maximalMomentDegree;
        }
        #endregion

        #region public properties

        #region IInfoOutputQueriable Members

        /// <summary>Gets the info-output level of detail.
        /// </summary>
        /// <value>The info-output level of detail.</value>
        public InfoOutputDetailLevel InfoOutputDetailLevel
        {
            get { return InfoOutputDetailLevel.High; }
        }
        #endregion

        /// <summary>Gets the expectation, i.e. the first moment, or <see cref="System.Double.NaN"/> if the first moment does not exist.
        /// </summary>
        /// <value>The expectation, i.e. the first moment, or <see cref="System.Double.NaN"/> if the first moment does not exist.</value>
        public abstract double Expectation
        {
            get;
        }

        /// <summary>Gets the variance, i.e. the second central moment, or <see cref="System.Double.NaN"/> if the second central moment does not exit.
        /// </summary>
        /// <value>The variance, i.e. the second central moment, or <see cref="System.Double.NaN"/> if the second central moment does not exit.</value>
        public abstract double Variance
        {
            get;
        }

        /// <summary>Gets the standard deviation, i.e. the square-root of the <see cref="ProbabilityDistributionMoments.Variance"/>.
        /// </summary>
        /// <value>The standard deviation, i.e. the square-root of the <see cref="ProbabilityDistributionMoments.Variance"/>.</value>
        public abstract double StandardDeviation
        {
            get;
        }

        /// <summary>Gets the maximal degree n for which the moment E[X^n] exist, perhaps <see cref="System.Int32.MaxValue"/>.
        /// </summary>
        /// <value>The maximal degree n for which the moment E[X^n] exist, perhaps <see cref="System.Int32.MaxValue"/>.</value>
        public int MaximalMomentDegree
        {
            get;
            private set;
        }

        /// <summary>Gets a specific (raw) moment, i.e. E[X^n], where E is the expectation operator.
        /// </summary>
        /// <value>The specific (raw) moment, i.e. E[X^n], where E is the expectation operator.</value>
        /// <param name="order">The order of the moment.</param>
        /// <returns>The value of the n-th moment, i.e. E[X^n], where E is the expectation operator.</returns>
        public double this[int order]
        {
            get { return GetValue(order); }
        }

        /// <summary>Gets the skewness (3-th central moment over the 3-th power of the standard deviation) or <see cref="System.Double.NaN"/> if the skewness is not well-defined.
        /// </summary>
        /// <value>The skewness or <see cref="System.Double.NaN"/> if the skewness is not well-defined.</value>
        public abstract double Skewness
        {
            get;
        }

        /// <summary>Gets the kurtosis (Excess Kurtosis; 4-th central moment over the 4-th power of the standard deviation) or <see cref="System.Double.NaN"/> if the kurtosis is not well-defined.
        /// </summary>
        /// <value>The kurtosis (Excess Kurtosis) or <see cref="System.Double.NaN"/> if the kurtosis is not well-defined.</value>
        public abstract double Kurtosis
        {
            get;
        }
        #endregion

        #region public methods

        #region IInfoOutputQueriable Members

        /// <summary>Sets the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> property.
        /// </summary>
        /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
        /// <returns>A value indicating whether the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> has been set to <paramref name="infoOutputDetailLevel" />.</returns>
        public bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
        {
            return (infoOutputDetailLevel == InfoOutputDetailLevel.High);
        }

        /// <summary>Gets informations of the current object as a specific <see cref="InfoOutput" /> instance.
        /// </summary>
        /// <param name="infoOutput">The <see cref="InfoOutput" /> object which is to be filled with informations concering the current instance.</param>
        /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
        public virtual void FillInfoOutput(InfoOutput infoOutput, string categoryName = InfoOutput.GeneralCategoryName)
        {
            var infoOutputPackage = infoOutput.AcquirePackage(categoryName);

            infoOutputPackage.Add("Expectation", Expectation);
            infoOutputPackage.Add("Variance", Variance);
            infoOutputPackage.Add("Standard deviation", StandardDeviation);
            infoOutputPackage.Add("Skewness", Skewness);
            infoOutputPackage.Add("Kurtosis", Kurtosis);
        }
        #endregion

        /// <summary>Gets the n-th moment, i.e. E[X^n], where E is the expectation operator.
        /// </summary>
        /// <param name="order">The order of the moment.</param>
        /// <returns>The value of the n-th moment, i.e. E[X^n], where E is the expectation operator.</returns>
        /// <exception cref="ArgumentException">Thrown, if the specified moment does not exist, see <see cref="ProbabilityDistributionMoments.MaximalMomentDegree"/>.</exception>
        public abstract double GetValue(int order);

        /// <summary>Gets the absolute n-th moment, i.e. E[|X|^n], where E is the expectation operator.
        /// </summary>
        /// <param name="order">The order of the moment.</param>
        /// <returns>The value of the absolute n-th moment, i.e. E[|X|^n], where E is the expectation operator.</returns>
        /// <exception cref="ArgumentException">Thrown, if the specified moment does not exist, see <see cref="ProbabilityDistributionMoments.MaximalMomentDegree"/>.</exception>
        public abstract double GetAbsValue(int order);

        /// <summary>Gets the n-th central moment, i.e. E[(X- E[X])^n], where E is the expectation operator.
        /// </summary>
        /// <param name="order">The order of the central moment.</param>
        /// <returns>The value of the n-th central moment, i.e. E[(X- E[X])^n], where E is the expectation operator.</returns>
        /// <remarks>The second central moment is identical to the <see cref="ProbabilityDistributionMoments.Variance"/>.</remarks>
        public abstract double GetCentralValue(int order);

        /// <summary>Gets the absolute n-th central moment, i.e. E[|X- E[X]|^n], where E is the expectation operator.
        /// </summary>
        /// <param name="order">The order of the central moment.</param>
        /// <returns>The value of the absolute n-th central moment, i.e. E[|X- E[X]|^n], where E is the expectation operator.</returns>
        public abstract double GetAbsCentralValue(int order);

        /// <summary>Gets a specific value of the moment-generating function E[exp(t*X)]
        /// </summary>
        /// <param name="t">The argument where to evaluate.</param>
        /// <param name="value">The specified value of the moment-generating function (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public abstract bool TryGetMgfValue(double t, out double value);
        #endregion
    }
}