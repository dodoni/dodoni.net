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

namespace Dodoni.MathLibrary.Optimizer.OneDimensional
{
    /// <summary>Serves as factory for <see cref="OneDimOptimizerConstraint"/> objects.
    /// </summary>
    public class OneDimOptimizerConstraintFactory : OneDimOptimizer.IConstraintFactory
    {
        #region public nested enumerations

        /// <summary>Represents the constraint type represented by the specified one-dimensional optimizer algorithm.
        /// </summary>
        [Flags]
        public enum ConstraintType
        {
            /// <summary>The real axis ]-\infty, \infty[ is supported by the specified optimizer algorithm.
            /// </summary>
            RealAxis = 0x01,

            /// <summary>The bounded interval [a,b] is supported by the specified optimizer algorithm.
            /// </summary>
            BoundedInterval = 0x02,

            /// <summary>The left bounded interval [a, \infty[ is supported by the specified optimizer algorithm.
            /// </summary>
            LeftClosedInterval = 0x04,

            /// <summary>The right bounded interval ]-\infty, b] is supported by the specified optimizer algorithm.
            /// </summary>
            RightClosedInterval = 0x08,

            /// <summary>The specified optimizer algorithm supports each of the following constraints: real axis, bounded interval, left bounded interval, right bounded interval.
            /// </summary>
            All = RealAxis | BoundedInterval | LeftClosedInterval | RightClosedInterval
        }
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="OneDimOptimizerConstraintFactory"/> class.
        /// </summary>
        /// <param name="supportedIntervalType">The interval types which are supported as constraints by the specified optimizer algorithm.</param>
        public OneDimOptimizerConstraintFactory(ConstraintType supportedIntervalType)
        {
            SupportedIntervalType = supportedIntervalType;
        }
        #endregion

        #region public properties

        #region IInfoOutputQueriable Members

        /// <summary>Gets the info-output level of detail.
        /// </summary>
        /// <value>The info-output level of detail.</value>
        public InfoOutputDetailLevel InfoOutputDetailLevel
        {
            get { return InfoOutputDetailLevel.Full; }
        }
        #endregion

        /// <summary>Gets the interval types which are supported as constraints by the specified optimizer algorithm.
        /// </summary>
        /// <value>The interval types which are supported as constraints by the specified optimizer algorithm.</value>
        public ConstraintType SupportedIntervalType
        {
            get;
            private set;
        }
        #endregion

        #region public methods

        #region IConstraintFactory Members

        /// <summary>Creates a new <see cref="OneDimOptimizer.IConstraint"/> object.
        /// </summary>
        /// <param name="constraint">The specific constraint, i.e. the argument of the objective function are constrainted to lie in a specified interval.</param>
        /// <returns>A specific <see cref="OneDimOptimizer.IConstraint"/> object with respect to the specified optimization algorithm.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if the optimization algorithm does not support this kind of constraints.</exception>
        public OneDimOptimizer.IConstraint Create(Interval.Bounded constraint)
        {
            if (SupportedIntervalType.HasFlag(ConstraintType.BoundedInterval) == true)
            {
                if (constraint == null)
                {
                    throw new ArgumentNullException(nameof(constraint));
                }
                return new OneDimOptimizerConstraint(this, constraint);
            }
            throw new InvalidOperationException();
        }

        /// <summary>Creates a new <see cref="OneDimOptimizer.IConstraint"/> object.
        /// </summary>
        /// <param name="constraint">The specific constraint, i.e. the argument of the objective function are constrainted to lie in a specified interval.</param>
        /// <returns>A specific <see cref="OneDimOptimizer.IConstraint"/> object with respect to the specified optimization algorithm.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if the optimization algorithm does not support this kind of constraints.</exception>
        public OneDimOptimizer.IConstraint Create(Interval.LeftBounded constraint)
        {
            if (SupportedIntervalType.HasFlag(ConstraintType.LeftClosedInterval) == true)
            {
                if (constraint == null)
                {
                    throw new ArgumentNullException(nameof(constraint));
                }
                return new OneDimOptimizerConstraint(this, constraint);
            }
            throw new InvalidOperationException();
        }

        /// <summary>Creates a new <see cref="OneDimOptimizer.IConstraint"/> object.
        /// </summary>
        /// <param name="constraint">The specific constraint, i.e. the argument of the objective function are constrainted to lie in a specified interval.</param>
        /// <returns>A specific <see cref="OneDimOptimizer.IConstraint"/> object with respect to the specified optimization algorithm.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if the optimization algorithm does not support this kind of constraints.</exception>
        public OneDimOptimizer.IConstraint Create(Interval.RightBounded constraint)
        {
            if (SupportedIntervalType.HasFlag(ConstraintType.RightClosedInterval) == true)
            {
                if (constraint == null)
                {
                    throw new ArgumentNullException(nameof(constraint));
                }
                return new OneDimOptimizerConstraint(this, constraint);
            }
            throw new InvalidOperationException();
        }

        /// <summary>Creates a new <see cref="OneDimOptimizer.IConstraint" /> object.
        /// </summary>
        /// <param name="constraint">The specific constraint, i.e. the argument of the objective function are constrainted to lie in a specified interval.</param>
        /// <returns>A specific <see cref="OneDimOptimizer.IConstraint" /> object with respect to the specified optimization algorithm.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if the optimization algorithm does not support this kind of constraints.</exception>
        public OneDimOptimizer.IConstraint Create(RealAxis constraint)
        {
            if (SupportedIntervalType.HasFlag(ConstraintType.RealAxis) == true)
            {
                return new OneDimOptimizerConstraint(this, constraint);
            }
            throw new InvalidOperationException();
        }
        #endregion

        #region IInfoOutputQueriable Members

        /// <summary>Sets the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel"/> property.
        /// </summary>
        /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
        /// <returns>A value indicating whether the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel"/> has been set to <paramref name="infoOutputDetailLevel"/>.</returns>
        public bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
        {
            return (infoOutputDetailLevel == InfoOutputDetailLevel.Full);
        }

        /// <summary>Gets informations of the current object as a specific <see cref="InfoOutput"/> instance.
        /// </summary>
        /// <param name="infoOutput">The <see cref="InfoOutput"/> object which is to be filled with informations concering the current instance.</param>
        /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
        public void FillInfoOutput(InfoOutput infoOutput, string categoryName = InfoOutput.GeneralCategoryName)
        {
            var infoOutputPackage = infoOutput.AcquirePackage(categoryName);
            infoOutputPackage.Add("SupportedIntervalType", SupportedIntervalType);
        }
        #endregion

        #endregion
    }
}