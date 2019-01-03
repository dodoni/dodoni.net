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
using Dodoni.MathLibrary.Miscellaneous;
using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.Optimizer.OneDimensional
{
    /// <summary>Serves as generic <see cref="OneDimOptimizer.IConstraint"/> implementation that encapsulates a specific <see cref="IOneDimRegion"/> object.
    /// </summary>
    public class OneDimOptimizerConstraint : OneDimOptimizer.IConstraint
    {
        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="OneDimOptimizerConstraint" /> class.
        /// </summary>
        /// <param name="constraintDescriptor">The constraint descriptor in its <see cref="OneDimOptimizerConstraintFactory"/> representation that serves as factory for the current object.</param>
        /// <param name="constraint">The constraint of the optimizer in its <see cref="IOneDimRegion"/> representation.</param>
        internal OneDimOptimizerConstraint(OneDimOptimizerConstraintFactory constraintDescriptor, IOneDimRegion constraint)
        {
            Factory = constraintDescriptor;
            IntervalRepresentation = constraint;
        }
        #endregion

        #region public properties

        #region IConstraint Members

        /// <summary>Gets the <see cref="OneDimOptimizer.IConstraintFactory" /> instance that has been used as factory for the current object.
        /// </summary>
        /// <value>The <see cref="OneDimOptimizer.IConstraintFactory" /> instance that has been used as factory for the current object.</value>
        public OneDimOptimizer.IConstraintFactory Factory
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
            get { return InfoOutputDetailLevel.Full; }
        }
        #endregion

        /// <summary>Gets the <see cref="IOneDimRegion"/> representation of the current object.
        /// </summary>
        /// <value>The <see cref="IOneDimRegion"/> representation of the current object.</value>
        public IOneDimRegion IntervalRepresentation
        {
            get;
            private set;
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
        }
        #endregion

        #endregion
    }
}