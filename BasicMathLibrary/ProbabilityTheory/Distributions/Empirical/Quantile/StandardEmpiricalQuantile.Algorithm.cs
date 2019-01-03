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
using System.Collections.ObjectModel;

namespace Dodoni.MathLibrary.ProbabilityTheory.Distributions.Empirical
{
    internal partial class StandardEmpiricalQuantile
    {
        /// <summary>Serves as implementation of the quantile function.
        /// </summary>
        private class Algorithm : IQuantileFunction
        {
            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="Algorithm"/> class.
            /// </summary>
            /// <param name="sample">The sample.</param>
            /// <param name="isSorted">A value indicating whether the values in the <paramref name="sample" /> are sorted in ascending order.</param>
            internal Algorithm(IEnumerable<double> sample, bool isSorted = false)
            {
                Sample = sample;
                if (isSorted == true)  // in any case create a copy of the sample!
                {
                    SortedSample = new ReadOnlyCollection<double>(sample.ToArray());
                }
                else
                {
                    var hiddenArray = sample.ToArray();
                    Array.Sort(hiddenArray);
                    SortedSample = new ReadOnlyCollection<double>(hiddenArray);
                }
            }
            #endregion

            #region public properties

            #region IQuantileFunction Members

            /// <summary>Gets the length of the sample.
            /// </summary>
            /// <value>The length of the sample.</value>
            public int SampleSize
            {
                get { return SortedSample.Count; }
            }

            /// <summary>Gets the (original) sample.
            /// </summary>
            /// <value>The (original) sample.</value>
            public IEnumerable<double> Sample
            {
                get;
                private set;
            }

            /// <summary>Gets the sample sorted in ascending order.
            /// </summary>
            /// <value>The sample sorted in ascending order.</value>
            public IList<double> SortedSample
            {
                get;
                private set;
            }

            /// <summary>Gets a specific quantile.
            /// </summary>
            /// <param name="probability">The probability.</param>
            /// <returns>The quantile with respect to the specified <paramref name="probability"/>.</returns>
            public double this[double probability]
            {
                get { return GetValue(probability); }
            }
            #endregion

            #endregion

            #region public methods

            #region IQuantileFunction Members

            /// <summary>Gets a specific quantile.
            /// </summary>
            /// <param name="probability">The probability.</param>
            /// <returns>The quantile with respect to the specified probability.</returns>
            public double GetValue(double probability)
            {
                /* do a linear interpolation between (p_k, x_k), k=0,\ldots, n-1, where p_k = k/(n-1) and x_k is the kth order statistic, i.e. p_k = mode[F(x_k)]. */
                int leftIndex = (int)(probability * (SampleSize - 1));

                if (leftIndex == SampleSize - 1)
                {
                    return SortedSample[SampleSize - 1];
                }
                return ((SampleSize - 1) * probability - leftIndex - 1) * (SortedSample[leftIndex + 1] - SortedSample[leftIndex]) + SortedSample[leftIndex + 1];
            }
            #endregion

            #endregion
        }
    }
}