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
    /// <summary>Serves as abstract base class for density estimator, i.e. histogram, kernel density estimator etc.
    /// </summary>
    public abstract class DensityEstimator : IIdentifierNameable, IAnnotatable, IInfoOutputQueriable
    {
        #region protected constructors

        /// <summary>Initializes a new instance of the <see cref="DensityEstimator" /> class.
        /// </summary>
        /// <param name="infoOutputDetailLevel">The info output detail level.</param>
        /// <param name="annotation">The annotation of the current instance.</param>
        protected DensityEstimator(InfoOutputDetailLevel infoOutputDetailLevel = InfoOutputDetailLevel.High, string annotation = "")
        {
            Annotation = annotation;
            InfoOutputDetailLevel = infoOutputDetailLevel;
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the current instance.
        /// </summary>
        /// <value>The language independent name of the current instance.</value>
        public IdentifierString Name
        {
            get { return GetName(); }
        }

        /// <summary>Gets the long name of the current instance.
        /// </summary>
        /// <value>The (perhaps) language dependent long name of the current instance.</value>
        public IdentifierString LongName
        {
            get { return GetLongName(); }
        }
        #endregion

        #region IAnnotatable Members

        /// <summary>Gets the annotation of the current instance.
        /// </summary>
        /// <value>The annotation of the current instance.</value>
        public string Annotation
        {
            get;
            private set;
        }

        /// <summary>Gets a value indicating whether the annotation is read-only.
        /// </summary>
        /// <value><c>true</c> if the annotation of this instance is readonly; otherwise, <c>false</c>.</value>
        public bool HasReadOnlyAnnotation
        {
            get { return true; }
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

        #endregion

        #region public methods

        #region IAnnotatable Members

        /// <summary>Sets the annotation of the current instance.
        /// </summary>
        /// <param name="annotation">The annotation.</param>
        /// <returns>A value indicating whether the <see cref="Annotation" /> has been changed.</returns>
        public bool TrySetAnnotation(string annotation)
        {
            return false;
        }
        #endregion

        #region IInfoOutputQueriable Members

        /// <summary>Sets the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> property.
        /// </summary>
        /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
        /// <returns>A value indicating whether the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> has been set to <paramref name="infoOutputDetailLevel" />.</returns>
        public virtual bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
        {
            return (infoOutputDetailLevel == InfoOutputDetailLevel);
        }

        /// <summary>Gets informations of the current object as a specific <see cref="InfoOutput" /> instance.
        /// </summary>
        /// <param name="infoOutput">The <see cref="InfoOutput" /> object which is to be filled with informations concering the current instance.</param>
        /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
        public abstract void FillInfoOutput(InfoOutput infoOutput, string categoryName = InfoOutput.GeneralCategoryName);
        #endregion

        /// <summary>Creates a new <see cref="IDensityEstimatorAlgorithm"/>.
        /// </summary>
        /// <param name="empiricalDistribution">The empirical distribution in its <see cref="EmpiricalDistribution"/> representation.</param>
        /// <returns>A new <see cref="IDensityEstimatorAlgorithm"/> object.</returns>
        public abstract IDensityEstimatorAlgorithm Create(EmpiricalDistribution empiricalDistribution);
        #endregion

        #region protected methods

        /// <summary>Gets the name of the current instance.
        /// </summary>
        /// <returns>The language independent name of the current instance.</returns>
        protected abstract IdentifierString GetName();

        /// <summary>Gets the long name of the current instance.
        /// </summary>
        /// <returns>The (perhaps) language dependent long name of the current instance.</returns>
        protected abstract IdentifierString GetLongName();

        /// <summary>Sets the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> property.
        /// </summary>
        /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
        protected void SetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
        {
            InfoOutputDetailLevel = infoOutputDetailLevel;
        }
        #endregion
    }
}