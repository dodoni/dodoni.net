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

using Dodoni.MathLibrary.Miscellaneous;
using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.Optimizer.MultiDimensional
{
    /// <summary>Represents a constraint of a specific multi-dimensional optimization algorithm.
    /// </summary>
    public class MultiDimOptimizerConstraint : MultiDimOptimizer.IConstraint
    {
        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="MultiDimOptimizerConstraint" /> class.
        /// </summary>
        /// <param name="constraintDescriptor">The constraint descriptor.</param>
        /// <param name="constraint">The constraint of the optimizer in its <see cref="IMultiDimRegion"/> representation.</param>
        public MultiDimOptimizerConstraint(MultiDimOptimizerConstraintFactory constraintDescriptor, IMultiDimRegion constraint)
        {
            Factory = constraintDescriptor ?? throw new ArgumentNullException(nameof(constraintDescriptor));
            RegionRepresentation = constraint ?? throw new ArgumentNullException(nameof(constraint));
        }
        #endregion

        #region public properties

        #region IConstraint Members

        /// <summary>Gets the <see cref="MultiDimOptimizer.IConstraintFactory" /> instance that has been used as factory for the current object.
        /// </summary>
        /// <value>The <see cref="MultiDimOptimizer.IConstraintFactory" /> instance that has been used as factory for the current object.</value>
        public MultiDimOptimizer.IConstraintFactory Factory
        {
            get;
            private set;
        }

        /// <summary>Gets the dimension of the constraint, i.e. of the function domain.
        /// </summary>
        /// <value>The dimension of the constraint.</value>
        public int Dimension
        {
            get { return RegionRepresentation.Dimension; }
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

        /// <summary>Gets the <see cref="IMultiDimRegion"/> representation of the current object.
        /// </summary>
        /// <value>The <see cref="IMultiDimRegion"/> representation of the current object.</value>
        public IMultiDimRegion RegionRepresentation
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

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return RegionRepresentation.ToString();
        }
        #endregion
    }
}