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
    /// <summary>Serves as abstract base class for an empirical quantile calculator.
    /// </summary>
    public abstract partial class EmpiricalQuantile : IIdentifierNameable, IAnnotatable
    {
        #region protected constructor

        /// <summary>Initializes a new instance of the <see cref="EmpiricalQuantile"/> class.
        /// </summary>
        /// <param name="name">The name of the approach for the empirical quantile calculation.</param>
        /// <param name="longName">The long name of the approach for the empirical quantile calculation.</param>
        /// <param name="annotation">An optional annotation of the empirical quantile calculation algorithm.</param>
        protected EmpiricalQuantile(IdentifierString name, IdentifierString longName = null, string annotation = null)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            Name = name;
            LongName = (longName != null) ? longName : name;
            Annotation = (annotation != null) ? annotation : String.Empty;
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the long name of the current instance.
        /// </summary>
        /// <value>The long name of the current instance.</value>
        public IdentifierString LongName
        {
            get;
            private set;
        }

        /// <summary>Gets the name of the current instance.
        /// </summary>
        /// <value>The name of the current instance.</value>
        public IdentifierString Name
        {
            get;
            private set;
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
        /// <value>A value indicating whether the annotation is read-only.</value>
        public virtual bool HasReadOnlyAnnotation
        {
            get { return true; }
        }

        /// <summary>Sets the annotation of the current instance.
        /// </summary>
        /// <param name="annotation">The annotation.</param>
        /// <returns> A value indicating whether the <see cref="Dodoni.BasicComponents.IAnnotatable.Annotation"/> has been changed.</returns>
        public virtual bool TrySetAnnotation(string annotation)
        {
            return false;
        }
        #endregion

        #endregion

        #region public methods

        /// <summary>Creates a new <see cref="IQuantileFunction"/> object for a specific sample.
        /// </summary>
        /// <param name="sample">The sample.</param>
        /// <param name="isSorted">A value indicating whether the values in the <paramref name="sample"/> are sorted in ascending order.</param>
        /// <returns>A new <see cref="IQuantileFunction"/> object for the specified sample.</returns>
        public abstract IQuantileFunction Create(IEnumerable<double> sample, bool isSorted = false);
        #endregion
    }
}