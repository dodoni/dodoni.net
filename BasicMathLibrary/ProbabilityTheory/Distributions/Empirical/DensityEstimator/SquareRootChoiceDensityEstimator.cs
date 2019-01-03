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
using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.ProbabilityTheory.Distributions.Empirical
{
    /// <summary>Represents a histogram as a probability density estimator, where the square root of the number of data points in the sample is taken for the total number of bins.
    /// </summary>
    /// <remarks>See http://en.wikipedia.org/wiki/Histogram (square-root choice). The implementation should give the same result than Micosoft Excel 2007 (up to rounding errors in the definition of the bins) etc.</remarks>
    internal partial class SquareRootChoiceDensityEstimator : DensityEstimator
    {
        #region private members

        /// <summary>The name of the current instance.
        /// </summary>
        private IdentifierString m_Name;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="SquareRootChoiceDensityEstimator" /> class.</summary>
        public SquareRootChoiceDensityEstimator()
        {
            m_Name = new IdentifierString("Square-root choice density estimator");
        }
        #endregion

        #region public methods

        /// <summary>Gets informations of the current object as a specific <see cref="InfoOutput" /> instance.
        /// </summary>
        /// <param name="infoOutput">The <see cref="InfoOutput" /> object which is to be filled with informations concering the current instance.</param>
        /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
        public override void FillInfoOutput(InfoOutput infoOutput, string categoryName = InfoOutput.GeneralCategoryName)
        {
            var infoOutputPackage = infoOutput.AcquirePackage(categoryName);
            infoOutputPackage.Add("Density estimator", "Square-root choice");
        }

        /// <summary>Creates a new <see cref="IDensityEstimatorAlgorithm" />.
        /// </summary>
        /// <param name="empiricalDistribution">The empirical distribution in its <see cref="EmpiricalDistribution" /> representation.</param>
        /// <returns>A new <see cref="IDensityEstimatorAlgorithm" /> object.</returns>
        public override IDensityEstimatorAlgorithm Create(EmpiricalDistribution empiricalDistribution)
        {
            return new Algorithm(empiricalDistribution, this);
        }
        #endregion

        #region protected methods

        /// <summary>Gets the name of the current instance.
        /// </summary>
        /// <returns>The language independent name of the current instance.</returns>
        protected override IdentifierString GetName()
        {
            return m_Name;
        }

        /// <summary>Gets the long name of the current instance.
        /// </summary>
        /// <returns>The (perhaps) language dependent long name of the current instance.</returns>
        protected override IdentifierString GetLongName()
        {
            return m_Name;
        }
        #endregion
    }
}