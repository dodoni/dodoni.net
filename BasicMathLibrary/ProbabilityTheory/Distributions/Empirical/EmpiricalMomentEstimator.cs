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

using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.ProbabilityTheory.Distributions.Empirical
{
    /// <summary>Serves as factory for <see cref="ProbabilityDistributionMoments"/> object that represents the moment calculator with respect to the empirical distribution, for example a bootstrap approach.
    /// </summary>
    public abstract class EmpiricalMomentEstimator : IIdentifierNameable, IAnnotatable
    {
        #region protected constructors

        /// <summary>Initializes a new instance of the <see cref="EmpiricalMomentEstimator" /> class.
        /// </summary>
        protected EmpiricalMomentEstimator()
        {
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the current instance.
        /// </summary>
        /// <value>The language independent name of the current instance.</value>
        public abstract IdentifierString Name
        {
            get;
        }

        /// <summary>Gets the long name of the current instance.
        /// </summary>
        /// <value>The (perhaps) language dependent long name of the current instance.</value>
        public abstract IdentifierString LongName
        {
            get;
        }
        #endregion

        #region IAnnotatable Members

        /// <summary>Gets a value indicating whether the annotation is read-only.
        /// </summary>
        /// <value><c>true</c> if the annotation of this instance is readonly; otherwise, <c>false</c>.</value>
        public abstract bool HasReadOnlyAnnotation
        {
            get;
        }

        /// <summary>Gets the annotation of the current instance.
        /// </summary>
        /// <value>The annotation of the current instance.</value>
        public abstract string Annotation
        {
            get;
        }
        #endregion

        /// <summary>Gets a value indicating whether the moment estimator is a stochastic approach. 
        /// </summary>
        /// <value><c>true</c> if this instance represents a moment estimator with a stochastic approach; otherwise, <c>false</c>.</value>
        public abstract bool IsStochasticApproach
        {
            get;
        }
        #endregion

        #region public methods

        #region IAnnotatable Members

        /// <summary>Sets the annotation of the current instance.
        /// </summary>
        /// <param name="annotation">The annotation.</param>
        /// <returns>A value indicating whether the <see cref="Annotation" /> has been changed.</returns>
        public abstract bool TrySetAnnotation(string annotation);
        #endregion

        /// <summary>Creates a moment estimator for a specific sample.
        /// </summary>
        /// <param name="sample">The sample.</param>
        /// <returns>A new moment calculator w.r.t. the empirical distribution in its <see cref="ProbabilityDistributionMoments"/> representation.</returns>
        public abstract ProbabilityDistributionMoments Create(IEnumerable<double> sample);
        #endregion
    }
}