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
    /// <summary>Serves as factory for <see cref="MultiDimOptimizer.IConstraint"/> objects.
    /// </summary>
    public class MultiDimOptimizerConstraintFactory : MultiDimOptimizer.IConstraintFactory
    {
        #region public nested enumerations

        /// <summary>Represents the constraint type represented by the specified n-dimensional optimizer algorithm.
        /// </summary>
        [Flags]
        public enum ConstraintType
        {
            /// <summary>The specified optimizer does support the non-constraint case.
            /// </summary>
            None = 0x01,

            /// <summary>Box constraints are supported by the specified optimizer algorithm.
            /// </summary>
            Box = 0x02,

            /// <summary>Constraints specified by a linear inequations are supported by the specified optimizer algorithm.
            /// </summary>
            LinearInEquation = 0x04,

            /// <summary>Constraints specified by a linear equations are supported by the specified optimizer algorithm.
            /// </summary>
            LinearEquation = 0x08,

            /// <summary>Constraints specified by a polynomial are supported by the specified optimizer algorithm.
            /// </summary>
            Polynomial = 0x10,

            /// <summary>A point x will lie in the feasible set iff c(x) &lt; \tolerance, where c is the specific constraint function.
            /// </summary>
            Inequality = 0x20,

            /// <summary>The specified optimizer algorithm supports each of the following constraints: Box constraints, Linear (In) Equations, Polynomial.
            /// </summary>
            All = Box | LinearInEquation | LinearEquation | Polynomial | Inequality
        }
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="MultiDimOptimizerConstraintFactory"/> class.
        /// </summary>
        /// <param name="supportedConstraints">The constraint types which are supported by the specified optimizer algorithm.</param>
        public MultiDimOptimizerConstraintFactory(ConstraintType supportedConstraints)
        {
            SupportedConstraints = supportedConstraints;
        }
        #endregion

        #region public properties

        #region IConstraintFactory Members

        /// <summary>Gets the minimum dimension supported by the optimizer algorithm.
        /// </summary>
        /// <value>The minimum dimension.</value>
        /// <remarks>For example some algorithms are not able to do a one-dimensional optimization.</remarks>
        public int MinimumDimension
        {
            get { return 2; }
        }

        /// <summary>Gets the maximum dimension supported by the optimizer algorithm.
        /// </summary>
        /// <value>The maximum dimension.</value>
        /// <remarks>Perhaps a algorithm for a fixed dimension (for example two) is represented by the current instance.</remarks>
        public int MaximumDimension
        {
            get { return Int32.MaxValue; }
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

        /// <summary>Gets the constraint types which are supported by the specified optimizer algorithm.
        /// </summary>
        /// <value>The constraint types which are supported as by the specified optimizer algorithm.</value>
        public ConstraintType SupportedConstraints
        {
            get;
            private set;
        }
        #endregion

        #region public methods

        #region IConstraintFactory Members

        /// <summary>Creates a new <see cref="MultiDimOptimizer.IConstraint"/> object.
        /// </summary>
        /// <param name="boxConstraint">The specific box constraint, i.e. the argument of the objective function are constrainted to lie in a specified hyperrectangle.</param>
        /// <returns>A specific <see cref="MultiDimOptimizer.IConstraint"/> object with respect to the specified optimization algorithm.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if the optimization algorithm does not support this kind of constraints.</exception>
        public MultiDimOptimizer.IConstraint Create(MultiDimRegion.Interval boxConstraint)
        {
            if (SupportedConstraints.HasFlag(ConstraintType.Box) == true)
            {
                return new MultiDimOptimizerConstraint(this, boxConstraint);
            }
            throw new InvalidOperationException();
        }

        /// <summary>Creates a new <see cref="MultiDimOptimizer.IConstraint"/> object.
        /// </summary>
        /// <param name="linearInequalityConstraint">The specific constraints in its <see cref="MultiDimRegion.LinearInequality"/> representation.</param>
        /// <returns>A specific <see cref="MultiDimOptimizer.IConstraint"/> object with respect to the specified optimization algorithm.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if the optimization algorithm does not support this kind of constraints.</exception>
        public MultiDimOptimizer.IConstraint Create(MultiDimRegion.LinearInequality linearInequalityConstraint)
        {
            if (SupportedConstraints.HasFlag(ConstraintType.LinearInEquation) == true)
            {
                return new MultiDimOptimizerConstraint(this, linearInequalityConstraint);
            }
            throw new InvalidOperationException();
        }

        /// <summary>Creates a new <see cref="MultiDimOptimizer.IConstraint"/> object.
        /// </summary>
        /// <param name="linearEqualityConstraint">The specific constraints in its <see cref="MultiDimRegion.LinearEquality"/> representation.</param>
        /// <returns>A specific <see cref="MultiDimOptimizer.IConstraint"/> object with respect to the specified optimization algorithm.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if the optimization algorithm does not support this kind of constraints.</exception>
        public MultiDimOptimizer.IConstraint Create(MultiDimRegion.LinearEquality linearEqualityConstraint)
        {
            if (SupportedConstraints.HasFlag(ConstraintType.LinearEquation) == true)
            {
                return new MultiDimOptimizerConstraint(this, linearEqualityConstraint);
            }
            throw new InvalidOperationException();
        }

        /// <summary>Creates a new <see cref="MultiDimOptimizer.IConstraint"/> object.
        /// </summary>
        /// <param name="polynomialConstraint">The specific constraints in its <see cref="MultiDimRegion.Polynomial"/> representation.</param>
        /// <returns>A specific <see cref="MultiDimOptimizer.IConstraint"/> object with respect to the specified optimization algorithm.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if the optimization algorithm does not support this kind of constraints.</exception>
        public MultiDimOptimizer.IConstraint Create(MultiDimRegion.Polynomial polynomialConstraint)
        {
            if (SupportedConstraints.HasFlag(ConstraintType.Polynomial) == true)
            {
                return new MultiDimOptimizerConstraint(this, polynomialConstraint);
            }
            throw new InvalidOperationException();
        }

        /// <summary>Creates a new <see cref="MultiDimOptimizer.IConstraint"/> object.
        /// </summary>
        /// <param name="inequality">The specific constraints in its <see cref="MultiDimRegion.Inequality"/> representation.</param>
        /// <returns>A specific <see cref="MultiDimOptimizer.IConstraint"/> object with respect to the specified optimization algorithm.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if the optimization algorithm does not support this kind of constraints.</exception>
       public MultiDimOptimizer.IConstraint Create(MultiDimRegion.Inequality inequality)
        {
            if (SupportedConstraints.HasFlag(ConstraintType.Inequality) == true)
            {
                return new MultiDimOptimizerConstraint(this, inequality);
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
            infoOutputPackage.Add("Supported Constraints", SupportedConstraints);
        }
        #endregion

        #endregion
    }
}