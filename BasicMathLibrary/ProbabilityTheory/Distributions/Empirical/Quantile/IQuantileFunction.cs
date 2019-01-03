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
using System.Collections.Generic;

namespace Dodoni.MathLibrary.ProbabilityTheory.Distributions.Empirical
{
    /// <summary>Provides methods for the caluclation of specified quantiles.
    /// </summary>
    public interface IQuantileFunction
    {
        /// <summary>Gets the length of the sample.
        /// </summary>
        /// <value>The length of the sample.</value>
        int SampleSize
        {
            get;
        }

        /// <summary>Gets the (original) sample.
        /// </summary>
        /// <value>The (original) sample.</value>
        IEnumerable<double> Sample
        {
            get;
        }

        /// <summary>Gets the sample sorted in ascending order.
        /// </summary>
        /// <value>The sample sorted in ascending order.</value>
        IList<double> SortedSample
        {
            get;
        }

        /// <summary>Gets a specific quantile.
        /// </summary>
        /// <param name="probability">The probability.</param>
        /// <returns>The quantile with respect to the specified <paramref name="probability"/>.</returns>
        double this[double probability]
        {
            get;
        }

        /// <summary>Gets a specific quantile.
        /// </summary>
        /// <param name="probability">The probability.</param>
        /// <returns>The quantile with respect to the specified <paramref name="probability"/>.</returns>
        double GetValue(double probability);
    }
}