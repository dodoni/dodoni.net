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

using Dodoni.MathLibrary.Optimizer;
using Dodoni.BasicComponents.Containers;
using Dodoni.MathLibrary.Optimizer.MultiDimensional;

namespace Dodoni.MathLibrary.Native.NLopt
{
    /// <summary>Serves as constraint for a specific NLopt optimization algorithm.
    /// </summary>
    public class NLoptConstraint : MultiDimOptimizer.IConstraint
    {
        #region private members

        /// <summary>The factory of the current instance in its <see cref="NLoptConstraintFactory"/> representation.
        /// </summary>
        private NLoptConstraintFactory m_ConstraintDescriptor;

        /// <summary>A delegate to set the constraint for a specific NLopt algorithm represented by a specific <see cref="NLoptPtr"/> object.
        /// </summary>
        private Action<NLoptPtr> m_ApplyToNLoptPtr;

        /// <summary>A delegate used to fill a specific <see cref="InfoOutputPackage"/> object.
        /// </summary>
        private Action<InfoOutputPackage> m_InfoOutputAction;

        /// <summary>The <see cref="System.String"/> representation of the current object.
        /// </summary>
        private string m_StringRepresentation;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="NLoptConstraint" /> class.
        /// </summary>
        /// <param name="nloptConstraintDescriptor">The <see cref="NLoptConstraintFactory"/> object that has been used as factory of the current object.</param>
        /// <param name="dimension">The dimension of the feasible region.</param>
        /// <param name="constraintName">A <see cref="System.String"/> representation of the constraint, i.e a short name.</param>
        /// <param name="applytoNLoptPtr">A delegate to set the constraint for a specific NLopt algorithm represented by a specific <see cref="NLoptPtr"/> object.</param>
        /// <param name="infoOutputAction">A delegate used to fill a specific <see cref="InfoOutputPackage"/> object.</param>
        internal NLoptConstraint(NLoptConstraintFactory nloptConstraintDescriptor, int dimension, string constraintName, Action<NLoptPtr> applytoNLoptPtr, Action<InfoOutputPackage> infoOutputAction = null)
        {
            m_ConstraintDescriptor = nloptConstraintDescriptor;
            Dimension = dimension;
            m_ApplyToNLoptPtr = applytoNLoptPtr;
            m_InfoOutputAction = infoOutputAction;
            m_StringRepresentation = String.Format("NLopt {0}; dimension: {1}", constraintName, dimension);
        }
        #endregion

        #region public properties

        #region IConstraint Members

        /// <summary>Gets the dimension of the constraint, i.e. of the function domain.
        /// </summary>
        /// <value>The dimension of the constraint.</value>
        public int Dimension
        {
            get;
            private set;
        }

        /// <summary>Gets the <see cref="MultiDimOptimizer.IConstraintFactory" /> instance that has been used as factory for the current object.
        /// </summary>
        /// <value>The <see cref="MultiDimOptimizer.IConstraintFactory" /> instance that has been used as factory for the current object.</value>
        MultiDimOptimizer.IConstraintFactory MultiDimOptimizer.IConstraint.Factory
        {
            get { return m_ConstraintDescriptor; }
        }
        #endregion

        #region IInfoOutputQueriable Members

        /// <summary>Gets the info-output level of detail.
        /// </summary>
        /// <value>The info-output level of detail.</value>
        public InfoOutputDetailLevel InfoOutputDetailLevel
        {
            get { return InfoOutputDetailLevel.Full; }
        }
        #endregion

        /// <summary>Gets the <see cref="NLoptConstraintFactory" /> instance that has been used as factory for the current object.
        /// </summary>
        /// <value>The <see cref="NLoptConstraintFactory" /> instance that has been used as factory for the current object.</value>
        public NLoptConstraintFactory Factory
        {
            get { return m_ConstraintDescriptor; }
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
            return (infoOutputDetailLevel == InfoOutputDetailLevel.Full);
        }

        /// <summary>Gets informations of the current object as a specific <see cref="InfoOutput" /> instance.
        /// </summary>
        /// <param name="infoOutput">The <see cref="InfoOutput" /> object which is to be filled with informations concering the current instance.</param>
        /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
        public void FillInfoOutput(InfoOutput infoOutput, string categoryName = InfoOutput.GeneralCategoryName)
        {
            if (m_InfoOutputAction != null)
            {
                var infoOutputPackage = infoOutput.AcquirePackage(categoryName);
                m_InfoOutputAction(infoOutputPackage);
            }
        }
        #endregion

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return m_StringRepresentation;
        }
        #endregion

        #region internal methods

        /// <summary>Applies the constraints represented by the current instance to a specific NLopt algorithm in its <see cref="NLoptPtr"/> representation.
        /// </summary>
        /// <param name="nloptPtr">The NLopt algorithm to apply the abort (stopping) condition in its <see cref="NLoptPtr"/> representation.</param>
        internal void ApplyTo(NLoptPtr nloptPtr)
        {
            m_ApplyToNLoptPtr(nloptPtr);
        }
        #endregion
    }
}