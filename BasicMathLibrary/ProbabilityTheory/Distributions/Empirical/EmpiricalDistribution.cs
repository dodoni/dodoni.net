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
using System.Data;
using System.Numerics;
using System.Threading.Tasks;
using System.Collections.Generic;

using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Utilities;
using Dodoni.BasicComponents.Containers;
using Dodoni.MathLibrary.ProbabilityTheory.Distributions.Empirical;

namespace Dodoni.MathLibrary.ProbabilityTheory.Distributions
{
    /// <summary>Represents the empirical distribution, i.e. based on a specific sample
    /// </summary> 
    /// <remarks>For big data projects, one should keep in mind that especially the calculation of quantiles a sorted sample will be generated. Moreover the constructor is in general
    /// a O(n) operation for calculation of mean, variance etc.</remarks>
    public partial class EmpiricalDistribution : IProbabilityDistribution
    {
        #region public static (readonly) members

        /// <summary>Represents the standard implementation for quantile calculation.
        /// </summary>
        public static readonly EmpiricalQuantile StandardEmpiricalQuantile;

        /// <summary>Represents the standard implementation for density estimator, i.e. a histogram, where the square root of the sample size is taken as the total number of bins.
        /// </summary>
        /// <remarks>See http://en.wikipedia.org/wiki/Histogram (square-root choice).</remarks>
        public static readonly DensityEstimator StandardDensityEstimator;

        /// <summary>Represents the standard moment estimator (i.e. without bootstrapping).
        /// </summary>
        public static readonly EmpiricalMomentEstimator StandardMomentEstimator;
        #endregion

        #region private members

        /// <summary>The Moment calculator.
        /// </summary>
        private Lazy<ProbabilityDistributionMoments> m_MomentCalculator;

        /// <summary>The quantile function.
        /// </summary>
        /// <remarks>Perhaps a sorted sample is not required, therefore a lazy construction is used.</remarks>
        private Lazy<IQuantileFunction> m_Quantile;

        /// <summary>The median, a lazy construction is used.
        /// </summary>
        /// <remarks>Perhaps a sorted sample is not required, therefore a lazy construction is used.</remarks>
        private Lazy<double> m_Median;

        /// <summary>The density estimator.
        /// </summary>
        private IDensityEstimatorAlgorithm m_DensityEstimator;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="EmpiricalDistribution" /> class.
        /// </summary>
        /// <param name="sample">The sample.</param>
        /// <param name="isSorted">A value indicating whether the values in the <paramref name="sample"/> are sorted in ascending order.</param>
        public EmpiricalDistribution(IEnumerable<double> sample, bool isSorted = false)
            : this(sample, StandardEmpiricalQuantile, isSorted, StandardDensityEstimator, StandardMomentEstimator)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="EmpiricalDistribution" /> class.
        /// </summary>
        /// <param name="sample">The sample.</param>
        /// <param name="empiricalQuantileMethod">The empirical quantile method.</param>
        /// <param name="isSorted">A value indicating whether the values in the <paramref name="sample"/> are sorted in ascending order.</param>
        public EmpiricalDistribution(IEnumerable<double> sample, EmpiricalQuantile empiricalQuantileMethod, bool isSorted = false)
            : this(sample, empiricalQuantileMethod, isSorted, StandardDensityEstimator, StandardMomentEstimator)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="EmpiricalDistribution" /> class.
        /// </summary>
        /// <param name="sample">The sample.</param>
        /// <param name="empiricalQuantileMethod">The empirical quantile method.</param>
        /// <param name="isSorted">A value indicating whether the values in the <paramref name="sample"/> are sorted in ascending order.</param>
        /// <param name="densityEstimator">The density estimator.</param>
        /// <param name="empiricalMomentEstimator">A empirical moment estimator, used for example for a bootstrap approach.</param>
        public EmpiricalDistribution(IEnumerable<double> sample, EmpiricalQuantile empiricalQuantileMethod, bool isSorted, DensityEstimator densityEstimator, EmpiricalMomentEstimator empiricalMomentEstimator)
        {
            if (sample == null)
            {
                throw new ArgumentNullException("sample");
            }
            Sample = sample.Skip(0);  // is used to avoid a cast to the original sample!

            InfoOutputDetailLevel = InfoOutputDetailLevel.Middle;
            m_Quantile = new Lazy<IQuantileFunction>(() => empiricalQuantileMethod.Create(sample, isSorted));  // sorted sample will be computed on demand only! 
            m_Median = new Lazy<double>(() => GetSampleMedian(SortedSample));  // required sorted sample, i.e. will create quantile calculation before

            m_MomentCalculator = new Lazy<ProbabilityDistributionMoments>(() => empiricalMomentEstimator.Create(sample));
            m_DensityEstimator = densityEstimator.Create(this);

            Name = new IdentifierString("Empirical distribution");
            LongName = new IdentifierString("Empirical distribution");
        }
        #endregion

        #region protected constructors

        /// <summary>Initializes a new instance of the <see cref="EmpiricalDistribution" /> class.
        /// </summary>
        /// <param name="empiricalDistribution">The empirical distribution.</param>
        /// <param name="densityEstimator">The density estimator.</param>
        protected EmpiricalDistribution(EmpiricalDistribution empiricalDistribution, DensityEstimator densityEstimator)
        {
            m_Median = empiricalDistribution.m_Median;
            m_Quantile = empiricalDistribution.m_Quantile;
            m_MomentCalculator = empiricalDistribution.m_MomentCalculator;

            Name = empiricalDistribution.Name;
            LongName = empiricalDistribution.LongName;
            m_DensityEstimator = densityEstimator.Create(this);
            InfoOutputDetailLevel = empiricalDistribution.InfoOutputDetailLevel;
        }
        #endregion

        #region static constructor

        /// <summary>Initializes the <see cref="EmpiricalDistribution" /> class.
        /// </summary>
        static EmpiricalDistribution()
        {
            StandardDensityEstimator = new SquareRootChoiceDensityEstimator();
            StandardEmpiricalQuantile = new StandardEmpiricalQuantile();
            StandardMomentEstimator = new StandardEmpiricalMomentEstimator();
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the current instance.
        /// </summary>
        /// <value>The language independent name of the current instance.</value>
        public IdentifierString Name
        {
            get;
            private set;
        }

        /// <summary>Gets the long name of the current instance.
        /// </summary>
        /// <value>The (perhaps) language dependent long name of the current instance.</value>
        public IdentifierString LongName
        {
            get;
            private set;
        }
        #endregion

        #region IInfoOutputQueriable Members

        /// <summary>Gets the info-output level of detail.
        /// </summary>
        /// <value>The info-output level of detail.</value>
        public InfoOutputDetailLevel InfoOutputDetailLevel
        {
            get;
            private set;
        }
        #endregion

        #region IProbabilityDistribution Members

        /// <summary>Gets the infimum of the support of the distribution.
        /// </summary>
        /// <value>The infimum of the support of the distribution.</value>
        double IProbabilityDistribution.Infimum
        {
            get { return Minimum; }
        }

        /// <summary>Gets the supremum of the support of the distribution.
        /// </summary>
        /// <value>The supremum of the support of the distribution.</value>
        double IProbabilityDistribution.Supremum
        {
            get { return Maximum; }
        }

        /// <summary>Gets the (raw, central etc.) moments of the probability distribution.
        /// </summary>
        /// <value>The (raw, central etc.) moments of the probability distribution.</value>
        public ProbabilityDistributionMoments Moment
        {
            get { return m_MomentCalculator.Value; }
        }

        /// <summary>Gets the sample median, i.e. the value in the middle of the sorted sample if sample size odd; otherwise the mean of two middle values.
        /// </summary>
        /// <value>The sample median, i.e. the value in the middle of the sorted sample if sample size odd; otherwise the mean of two middle values.</value>
        public double Median
        {
            get { return m_Median.Value; }
        }
        #endregion

        /// <summary>Gets the (original) sample.
        /// </summary>
        /// <value>The (original) sample.</value>
        public IEnumerable<double> Sample
        {
            get;
            private set;
        }

        /// <summary>Gets the sample size.
        /// </summary>
        /// <value>The sample size.</value>
        public int SampleSize
        {
            get { return m_Quantile.Value.SampleSize; }
        }

        /// <summary>Gets the sample sorted in ascending order.
        /// </summary>
        /// <value>The sample sorted in ascending order.</value>
        public IList<double> SortedSample
        {
            get { return m_Quantile.Value.SortedSample; }
        }

        /// <summary>Gets the sample mean, i.e. the first moment, or <see cref="System.Double.NaN" /> if the first moment does not exist.
        /// </summary>
        /// <value>The sample mean, i.e. the first moment, or <see cref="System.Double.NaN" /> if the first moment does not exist.</value>
        public double Mean
        {
            get { return m_MomentCalculator.Value.Expectation; }  // identical to "Expectation", just another name
        }

        /// <summary>Gets the minimum, i.e. the smallest value in the sample.
        /// </summary>
        /// <value>The minimum, i.e. the smallest value in the sample.</value>
        public double Minimum
        {
            get { return SortedSample[0]; }
        }

        /// <summary>Gets the maximum, i.e. the largest value in the sample.
        /// </summary>
        /// <value>The maximum, i.e. the largest value in the sample.</value>
        public double Maximum
        {
            get { return SortedSample[SampleSize - 1]; }
        }

        /// <summary>Gets the quantile function.
        /// </summary>
        /// <value>The quantile function.</value>
        public IQuantileFunction Quantile
        {
            get { return m_Quantile.Value; }
        }
        #endregion

        #region public methods

        #region IProbabilityDistribution Members

        /// <summary>Gets a specific value of the cummulative distribution function.
        /// </summary>
        /// <param name="x">The value where to evaluate.</param>
        /// <returns>The specified value of the cummulative distribution function.</returns>
        public double GetCdfValue(double x)
        {
            /* search for n with x_n <= x, where (x_n) is sorted and n is maximal with this property */
            int upperBoundIndex = SortedSample.BinarySearch(x);

            if (upperBoundIndex >= 0) // perhaps there are other items in the sample with the same value
            {
                int k = upperBoundIndex;

                while ((k < SampleSize) && (SortedSample[k] <= x))
                {
                    k++;
                }
                upperBoundIndex = k - 1;
            }
            else
            {
                upperBoundIndex = Math.Max(0, ~upperBoundIndex - 1);
            }
            return (upperBoundIndex + 1.0) / SampleSize;
        }

        /// <summary>Gets specific values of the cummulative distribution function.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The arguments where to evaluate the cummulative distribution function.</param>
        /// <param name="y">The specified values of the cummulative distribution function (output).</param>
        public void GetCdfValue(int n, double[] a, double[] y)
        {
            Parallel.For(0, n, k =>
            {
                y[k] = GetCdfValue(a[k]);
            });
        }

        /// <summary>Gets a specific value of the inverse of the cummulative distribution function.
        /// </summary>
        /// <param name="probability">The probability where to evaluate.</param>
        /// <returns>The specified value of the inverse of the cummulative distribution function.</returns>
        /// <remarks>This method returns the same values as the property <see cref="Quantile"/>.</remarks>
        public double GetInverseCdfValue(double probability)
        {
            return Quantile.GetValue(probability);
        }

        /// <summary>Gets specific values of the inverse of the cummulative distribution function, i.e. quantile function.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The probabilities where to evaluate the inverse of the cummulative distribution function.</param>
        /// <param name="y">The specified values of the inverse of the cummulative distribution function (output).</param>
        public void GetInverseCdfValue(int n, double[] a, double[] y)
        {
            Parallel.For(0, n, k =>
            {
                y[k] = GetInverseCdfValue(a[k]);
            });
        }

        /// <summary>Gets a specific value of the probability density function.
        /// </summary>
        /// <param name="x">The point where to evaluate.</param>
        /// <returns>The specified value of the probability density function.</returns>
        /// <remarks>A specific density estimator is taken into account.</remarks>
        public double GetPdfValue(double x)
        {
            return m_DensityEstimator.GetValue(x);
        }

        /// <summary>Gets specific values of the probability density function.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The arguments where to evaluate the probability density function.</param>
        /// <param name="y">The specified values of the probability density function (output).</param>
        public void GetPdfValue(int n, double[] a, double[] y)
        {
            Parallel.For(0, n, k =>
            {
                y[k] = GetPdfValue(a[k]);
            });
        }

        /// <summary>Gets a specific value of the characteristic function E[exp(itX)].
        /// </summary>
        /// <param name="t">The point where to evaluate the characteristic function.</param>
        /// <returns>The specified value of the characteristic function E[exp(itX)].</returns>
        public Complex GetChfValue(double t)
        {
            return DoMath.Average(Sample.Select(x => Complex.Exp(Complex.ImaginaryOne * t * x)));
        }

        /// <summary>Gets specific values of the characteristic function E[exp(itX)].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The arguments where to evaluate the characteristic function.</param>
        /// <param name="y">The specified values of the characteristic function E[exp(itX)] (output).</param>
        public void GetChfValue(int n, double[] a, Complex[] y)
        {
            Parallel.For(0, n, k =>
            {
                y[k] = GetChfValue(a[k]);
            });
        }
        #endregion

        #region IInfoOutputQueriable Members

        /// <summary>Sets the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> property.
        /// </summary>
        /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
        /// <returns>A value indicating whether the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> has been set to <paramref name="infoOutputDetailLevel" />.</returns>
        public bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
        {
            InfoOutputDetailLevel = infoOutputDetailLevel;
            return true;
        }

        /// <summary>Gets informations of the current object as a specific <see cref="InfoOutput" /> instance.
        /// </summary>
        /// <param name="infoOutput">The <see cref="InfoOutput" /> object which is to be filled with informations concering the current instance.</param>
        /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
        public void FillInfoOutput(InfoOutput infoOutput, string categoryName = InfoOutput.GeneralCategoryName)
        {
            var infoOutputPackage = infoOutput.AcquirePackage(categoryName);
            infoOutputPackage.Add("Distribution", "Empirical");
            infoOutputPackage.Add("Sample Size", SampleSize);
            infoOutputPackage.Add("Mean", Mean);

            infoOutputPackage.Add("Minimum", Minimum);
            infoOutputPackage.Add("Maximum", Maximum);
            infoOutputPackage.Add("Media", Median);

            if (InfoOutputDetailLevel.IsAtLeastAsComprehensiveAs(InfoOutputDetailLevel.High) == true)
            {
                var sampleDataTable = new DataTable("Sample");
                sampleDataTable.Columns.Add("Value", typeof(double));
                foreach (var value in Sample)
                {
                    var row = sampleDataTable.NewRow();
                    row[0] = value;
                    sampleDataTable.Rows.Add(row);
                }
                infoOutputPackage.Add(sampleDataTable);
            }
            m_MomentCalculator.Value.FillInfoOutput(infoOutput, categoryName + ".Moments");
            m_DensityEstimator.FillInfoOutput(infoOutput, categoryName + ".Density");
        }
        #endregion

        /// <summary>Gets the relative number of values inside a specific range, for example the relative number of positive values in the specified sample.
        /// </summary>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="upperBound">The upper bound.</param>
        /// <returns>The relative number of values inside the range specified by <paramref name="lowerBound"/> and <paramref name="upperBound"/>.</returns>
        public double GetRangeStatistics(double lowerBound = 0.0, double upperBound = Double.MaxValue)
        {
            int lowerBoundIndex = SortedSample.BinarySearch(lowerBound);
            if (lowerBoundIndex < 0)
            {
                lowerBoundIndex = ~lowerBoundIndex;

                if (lowerBoundIndex >= SampleSize)  // each element is less than the specified lower bound
                {
                    return 0.0;
                }
            }
            int upperBoundIndex = SortedSample.BinarySearch(upperBound);
            if (upperBoundIndex < 0)
            {
                upperBoundIndex = Math.Max(0, ~upperBoundIndex - 1);
            }
            return (upperBoundIndex - lowerBoundIndex + 1.0) / SampleSize;
        }

        /// <summary>Creates a new <see cref="EmpiricalDistribution"/> with respect to a speicific density estimator.
        /// </summary>
        /// <param name="densityEstimator">The density estimator.</param>
        /// <returns>A new <see cref="EmpiricalDistribution"/> object that encapsulates the same sample as the current instance, but a different density estimator is taken into account.</returns>
        public EmpiricalDistribution Create(DensityEstimator densityEstimator)
        {
            return new EmpiricalDistribution(this, densityEstimator);
        }
        #endregion

        #region public static methods

        /// <summary>Creates a new <see cref="EmpiricalDistribution"/> object.
        /// </summary>
        /// <param name="sample">The sample.</param>
        /// <param name="isSorted">A value indicating whether the values in the <paramref name="sample"/> are sorted in ascending order.</param>
        /// <returns>A new <see cref="EmpiricalDistribution"/> object.</returns>
        public static EmpiricalDistribution Create(IEnumerable<double> sample, bool isSorted = false)
        {
            return new EmpiricalDistribution(sample, isSorted);
        }

        /// <summary>Creates a new <see cref="EmpiricalDistribution"/> object.
        /// </summary>
        /// <param name="sample">The sample.</param>
        /// <param name="empiricalQuantileMethod">The empirical quantile method.</param>
        /// <param name="isSorted">A value indicating whether the values in the <paramref name="sample"/> are sorted in ascending order.</param>
        /// <returns>A new <see cref="EmpiricalDistribution"/> object.</returns>
        public static EmpiricalDistribution Create(IEnumerable<double> sample, EmpiricalQuantile empiricalQuantileMethod, bool isSorted = false)
        {
            return new EmpiricalDistribution(sample, empiricalQuantileMethod, isSorted);
        }

        /// <summary>Creates a new <see cref="EmpiricalDistribution"/> object.
        /// </summary>
        /// <param name="sample">The sample.</param>
        /// <param name="empiricalQuantileMethod">The empirical quantile method.</param>
        /// <param name="isSorted">A value indicating whether the values in the <paramref name="sample"/> are sorted in ascending order.</param>
        /// <param name="densityEstimator">The density estimator.</param>
        /// <param name="empiricalMomentEstimator">The empirical moment estimator.</param>
        /// <returns>A new <see cref="EmpiricalDistribution"/> object.</returns>
        public static EmpiricalDistribution Create(IEnumerable<double> sample, EmpiricalQuantile empiricalQuantileMethod, bool isSorted, DensityEstimator densityEstimator, EmpiricalMomentEstimator empiricalMomentEstimator)
        {
            return new EmpiricalDistribution(sample, empiricalQuantileMethod, isSorted, densityEstimator, empiricalMomentEstimator);
        }

        /// <summary>Gets an unbiased covariance estimator of two random samples.
        /// </summary>
        /// <param name="sample1">The first sample.</param>
        /// <param name="sample2">The second sample.</param>
        /// <returns>The estimated covariance of the two specified samples.</returns>
        /// <remarks>The implementation is based on "Formulas for Robust, One-pass parallel computation of Covariances and arbitrary-order statistic Moments", P. Pebay, Sandia National Laboratories, September 2008.</remarks>
        public static double GetCovariance(EmpiricalDistribution sample1, EmpiricalDistribution sample2)
        {
            var c2 = 0.0;
            var mean1 = 0.0;
            var mean2 = 0.0;

            var sample2Enumerator = sample2.Sample.GetEnumerator();

            int n = 1;
            foreach (var value1 in sample1.Sample)
            {
                sample2Enumerator.MoveNext();
                var value2 = sample2Enumerator.Current;

                c2 += (value1 - mean1) * (value2 - mean2) * (n - 1) / n;

                mean1 += (value1 - mean1) / n;
                mean2 += (value2 - mean2) / n;
                n++;
            }
            return c2 / (n - 2);  // n is sample length + 1 at this time
        }

        /// <summary>Gets the estimated Pearson's correlation coefficient of two samples.
        /// </summary>
        /// <param name="sample1">The first sample.</param>
        /// <param name="sample2">The second sample.</param>
        /// <returns>The estimated Pearson's correlation coefficient, i.e. the covariance of the two specified samples divided by the product of their standard deviations.</returns>
        /// <remarks>The implementation is based on "Formulas for Robust, One-pass parallel computation of Covariances and arbitrary-order statistic Moments", P. Pebay, Sandia National Laboratories, September 2008.</remarks>
        public static double GetCorrelationCoefficient(EmpiricalDistribution sample1, EmpiricalDistribution sample2)
        {
            return GetCovariance(sample1, sample2) / (sample1.Moment.StandardDeviation * sample2.Moment.StandardDeviation);
        }
        #endregion

        #region private static methods

        /// <summary>Gets the median of a specified sample.
        /// </summary>
        /// <param name="sortedSample">The sorted sample.</param>
        /// <returns>The median of the <paramref name="sortedSample"/>.</returns>
        private static double GetSampleMedian(IList<double> sortedSample)
        {
            if (sortedSample == null)
            {
                throw new ArgumentNullException("sortedSample");
            }
            int n = sortedSample.Count;

            if (n == 0)
            {
                throw new ArgumentException("sortedSample");
            }
            if (n % 2 == 1) // sample size is odd 
            {
                return sortedSample[n / 2];
            }
            else
            {
                /* calculate mean of two middle values */
                int leftIndex = n / 2 - 1;

                return 0.5 * (sortedSample[leftIndex] + sortedSample[leftIndex + 1]);
            }
        }
        #endregion
    }
}