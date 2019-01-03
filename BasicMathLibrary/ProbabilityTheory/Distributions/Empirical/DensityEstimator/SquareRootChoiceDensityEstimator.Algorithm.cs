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

using Dodoni.BasicComponents.Utilities;
using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.ProbabilityTheory.Distributions.Empirical
{
    internal partial class SquareRootChoiceDensityEstimator
    {
        /// <summary>Serves as implementation of the density estimator algorithm.
        /// </summary>
        /// <remarks>\hat{f}(x) = 1/n * \sum_{j=1}^n 1_{ ]t_k, t_k+1[ }(x_j), where n is the sample size, t_k = x_0 + k / h and h is the total number of bins. k is specified
        /// by x \in ]t_k, t_{k+1}] and x_0 is the smallest value in the sample. For x = x_0, the interval {t_0} is considered only.</remarks>
        private class Algorithm : IDensityEstimatorAlgorithm
        {
            #region private members

            /// <summary>The factory of the current instance.
            /// </summary>
            private SquareRootChoiceDensityEstimator m_Factory;

            /// <summary>The sample.
            /// </summary>
            private EmpiricalDistribution m_EmpiricalDistribution;

            /// <summary>The total number of bins.
            /// </summary>
            private int m_TotalNumberOfBins;

            /// <summary>The bin width.
            /// </summary>
            private double m_BinWidth;

            /// <summary>One over the number of sample times the number of bins.
            /// </summary>
            private double m_Delta;
            #endregion

            #region internal constructors

            /// <summary>Creates a new <see cref="Algorithm"/>.
            /// </summary>
            /// <param name="empiricalDistribution">The empirical distribution in its <see cref="EmpiricalDistribution" /> representation.</param>
            /// <param name="factory">The <see cref="DensityEstimator" /> object that serves as factory of the current object.</param>
            internal Algorithm(EmpiricalDistribution empiricalDistribution, SquareRootChoiceDensityEstimator factory)
            {
                m_Factory = factory;
                m_EmpiricalDistribution = empiricalDistribution;

                m_TotalNumberOfBins = (int)Math.Floor(Math.Sqrt(empiricalDistribution.SampleSize));
                m_BinWidth = (empiricalDistribution.Maximum - empiricalDistribution.Minimum) / m_TotalNumberOfBins;
                m_Delta = 1.0 / empiricalDistribution.SampleSize;
            }
            #endregion

            #region public properties

            #region IInfoOutputQueriable Members

            /// <summary>Gets the info-output level of detail.
            /// </summary>
            /// <value>The info-output level of detail.</value>
            public InfoOutputDetailLevel InfoOutputDetailLevel
            {
                get { return InfoOutputDetailLevel.Full; }
            }
            #endregion

            #region IDensityEstimatorAlgorithm Members

            /// <summary>Gets the <see cref="DensityEstimator" /> object that serves as factory of the current object.
            /// </summary>
            /// <value>The <see cref="DensityEstimator" /> object that serves as factory of the current object.</value>
            public DensityEstimator Factory
            {
                get { return m_Factory; }
            }
            #endregion

            #endregion

            #region public methods

            #region IDensityEstimatorAlgorithm Members

            /// <summary>Gets the estimated density at a specific point.
            /// </summary>
            /// <param name="x">The argument.</param>
            /// <returns>The estimated density at <paramref name="x" />.</returns>
            public double GetValue(double x)
            {
                if ((x < m_EmpiricalDistribution.Minimum) || (x > m_EmpiricalDistribution.Maximum))
                {
                    return 0.0;
                }
                if (x == m_EmpiricalDistribution.Minimum)
                {
                    return m_Delta * m_EmpiricalDistribution.SortedSample.TakeWhile(sample => sample == x).Count();
                }
                else
                {
                    var binPositionOfX = Math.Min(Math.Truncate((x - m_EmpiricalDistribution.Minimum) / m_BinWidth), m_TotalNumberOfBins - 1);
                    var t = m_EmpiricalDistribution.Minimum + binPositionOfX * m_BinWidth;  // it should hold x \in ]t, t + binWidth]
                    if (x <= t)
                    {
                        binPositionOfX--;
                        t = m_EmpiricalDistribution.Minimum + binPositionOfX * m_BinWidth;  // it should hold x \in ]t, t + binWidth]
                    }

                    /* calculate the number of data points in the sample that are smaller or equal than the boundary 't': */
                    int numberOfSmallerOrEqualThanLowerBound = 0;

                    int indexLeft = m_EmpiricalDistribution.SortedSample.BinarySearch(t);
                    if (indexLeft >= 0)
                    {
                        /* perhaps there exists more than one value in the sample which is identical to boundary 't' */

                        numberOfSmallerOrEqualThanLowerBound = indexLeft;

                        int k = indexLeft;
                        while ((k < m_EmpiricalDistribution.SampleSize) && (m_EmpiricalDistribution.SortedSample[k] == t))
                        {
                            numberOfSmallerOrEqualThanLowerBound++;
                            k++;
                        }
                    }
                    else
                    {
                        numberOfSmallerOrEqualThanLowerBound = ~indexLeft;  // take index of next greater value
                    }

                    /* calculate the number of data points in the sample that are strictly greater than the boundary 't + binWidth: */
                    int numberOfGreaterThanUpperBound = 0;

                    var tNext = t + m_BinWidth;
                    int indexRight = m_EmpiricalDistribution.SortedSample.BinarySearch(tNext);
                    if (indexRight >= 0)
                    {
                        numberOfGreaterThanUpperBound = indexRight;

                        int k = indexRight;
                        while ((k < m_EmpiricalDistribution.SampleSize) && (m_EmpiricalDistribution.SortedSample[k] == tNext))
                        {
                            numberOfGreaterThanUpperBound++;
                            k++;
                        }
                    }
                    else
                    {
                        numberOfGreaterThanUpperBound = Math.Min(~indexRight, m_EmpiricalDistribution.SampleSize - 1);
                    }
                    return m_Delta * Math.Max(0, numberOfGreaterThanUpperBound - numberOfSmallerOrEqualThanLowerBound);  // correspond to the number of sample points within ]t, t + binWidth]
                }
            }
            #endregion

            #region IInfoOutputQueriable Members

            /// <summary>Sets the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> property.
            /// </summary>
            /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
            /// <returns>A value indicating whether the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> has been set to <paramref name="infoOutputDetailLevel" />.</returns>
            public bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
            {
                return (infoOutputDetailLevel == InfoOutputDetailLevel.Full);
            }

            /// <summary>Gets informations of the current object as a specific <see cref="InfoOutput" /> instance.
            /// </summary>
            /// <param name="infoOutput">The <see cref="InfoOutput" /> object which is to be filled with informations concering the current instance.</param>
            /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
            public void FillInfoOutput(InfoOutput infoOutput, string categoryName = InfoOutput.GeneralCategoryName)
            {
                var infoOutputPackage = infoOutput.AcquirePackage(categoryName);
                infoOutputPackage.Add("Density estimator", "Square-root choice");

                var binRangeDataTable = new DataTable("BinRange");
                binRangeDataTable.Columns.Add("Value", typeof(double));


                var bin = m_EmpiricalDistribution.Minimum;
                for (int k = 0; k < m_TotalNumberOfBins; k++)
                {
                    var row = binRangeDataTable.NewRow();
                    row[0] = bin;
                    binRangeDataTable.Rows.Add(row);

                    bin += m_BinWidth;
                }
                infoOutputPackage.Add(binRangeDataTable);
            }
            #endregion

            #endregion
        }
    }
}