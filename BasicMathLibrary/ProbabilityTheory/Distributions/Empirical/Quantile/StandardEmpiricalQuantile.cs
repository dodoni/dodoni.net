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
using System.Collections.Generic;

using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.ProbabilityTheory.Distributions.Empirical
{
    /// <summary>Serves as standard implementation for the empirical quantile function, i.e. do a linear interpolation with respect to  (p_k, x_k), k=0,...,n-1, 
    /// where (x_k) represents the sorted sample and p_k = k/(n-1).
    /// </summary>
    internal partial class StandardEmpiricalQuantile : EmpiricalQuantile
    {
        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="StandardEmpiricalQuantile"/> class.
        /// </summary>
        internal StandardEmpiricalQuantile()
            : base(new IdentifierString("Standard empirical Quantile"))
        {
        }
        #endregion

        #region public methods

        /// <summary>Creates a new <see cref="IQuantileFunction" /> object for a specific sample.
        /// </summary>
        /// <param name="sample">The sample.</param>
        /// <param name="isSorted">A value indicating whether the values in the <paramref name="sample" /> are sorted in ascending order.</param>
        /// <returns>A new <see cref="IQuantileFunction" /> object for the specified sample.</returns>
        public override IQuantileFunction Create(IEnumerable<double> sample, bool isSorted = false)
        {
            return new Algorithm(sample, isSorted);
        }
        #endregion
    }
}