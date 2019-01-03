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
using Dodoni.MathLibrary.Optimizer.MultiDimensional.ConstraintProviders;

namespace Dodoni.MathLibrary.Optimizer.MultiDimensional
{
    /// <summary>Serves as factory for <see cref="IMultiDimOptimizerAlgorithm"/> based on an multi-dimensional optimization algorithm that supports optimization problems
    /// without constraints (only); i.e. apply for example a specific transformation to the objective function etc.
    /// </summary>
    public abstract class MultiDimOptimizerConstraintProvider
    {
        #region nested classes

        /// <summary>Serves as factory for <see cref="BoxTransformationAlgorithm"/> objects.
        /// </summary>
        private class BoxTransformationFactory : MultiDimOptimizerConstraintProvider
        {
            #region public properties

            /// <summary>Gets the constraint types which are supported.
            /// </summary>
            /// <value>The constraint types which are supported.</value>
            public override MultiDimOptimizerConstraintFactory.ConstraintType SupportedConstraints
            {
                get { return MultiDimOptimizerConstraintFactory.ConstraintType.Box | MultiDimOptimizerConstraintFactory.ConstraintType.None; }
            }
            #endregion

            #region public methods

            /// <summary>Creates a specific <see cref="IMultiDimOptimizerAlgorithm"/> object.
            /// </summary>
            /// <param name="optimizer">The <see cref="MultiDimOptimizer"/> object that supports a optimization problems without constraints.</param>
            /// <param name="multiDimRegionCollection">The constraints in its <see cref="MultiDimOptimizer.IConstraint"/> representation.</param>
            /// <returns>The specific <see cref="IMultiDimOptimizerAlgorithm"/> object.</returns>
            public override IMultiDimOptimizerAlgorithm Create(MultiDimOptimizer optimizer, MultiDimOptimizer.IConstraint[] multiDimRegionCollection)
            {
                if (multiDimRegionCollection == null)
                {
                    throw new ArgumentNullException(nameof(multiDimRegionCollection));
                }
                foreach (var constraint in multiDimRegionCollection)
                {
                    if (constraint is MultiDimOptimizerConstraint)
                    {
                        var con = (MultiDimOptimizerConstraint)constraint;
                        if (con.RegionRepresentation is MultiDimRegion.Interval)
                        {
                            return new BoxTransformationAlgorithm(optimizer, con.RegionRepresentation as MultiDimRegion.Interval);
                        }
                    }
                }
                throw new InvalidOperationException();
            }

            /// <summary>Returns a <see cref="System.String" /> that represents this instance.
            /// </summary>
            /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
            public override string ToString()
            {
                return "Box transformation";
            }
            #endregion
        }
        #endregion

        #region public static (readonly) members

        /// <summary>Represents the transformation of a Box constraint, i.e. a multi-dimensional interval to the unconstraint
        /// region, i.e. \R^n via the transformation, x =(x_0,...,x_n) \in \R^n:
        /// <list type="bullet">
        ///   <listheader>
        ///       <term>Transformation</term>
        ///       <description>Condition</description>
        ///   </listheader>
        ///  <item>
        ///    <term>a_j + (b_j - a_j) * sin^2 x_j,</term>
        ///    <description>if [a_j, b_j] is a bounded interval.</description>
        ///  </item>
        ///  <item>
        ///    <term>x_j,</term>
        ///    <description>if a_j and b_j are unbounded.</description>
        ///  </item>
        ///  <item>
        ///    <term>a_j + x^2,</term>
        ///    <description>if a_j is bounded and b_j is unbounded.</description>
        ///  </item>
        ///  <item>
        ///    <term>b_j - x^2,</term>
        ///    <description>if a_j is unbounded and b_j is bounded.</description>
        ///  </item>
        /// </list>
        /// </summary>
        /// <remarks>The implementation is based on
        /// <para>'A comparison of several current optimization methods, and the use of transformations in constrained problems',  M. J. Box, The Computer Journal 1966 9(1):67-77, 1966.</para>
        /// </remarks>
        public static MultiDimOptimizerConstraintProvider BoxTransformation;

        /// <summary>If an argument is outside the constraints, the return value is
        /// <para> <c>LastFunctionValueWithNoPenalty</c> + distance(x, Constraints)^2 * relativePenaltyWeight + absolutePenaltyWeight.</para>
        /// </summary>
        public static QuadraticPenaltyConstraintProvider QuadraticPenalty;
        #endregion

        #region static constructor

        /// <summary>Initializes the <see cref="MultiDimOptimizerConstraintProvider" /> class.
        /// </summary>
        static MultiDimOptimizerConstraintProvider()
        {
            BoxTransformation = new BoxTransformationFactory();
            QuadraticPenalty = new QuadraticPenaltyConstraintProvider();
        }
        #endregion

        #region public properties

        /// <summary>Gets the constraint types which are supported.
        /// </summary>
        /// <value>The constraint types which are supported.</value>
        public abstract MultiDimOptimizerConstraintFactory.ConstraintType SupportedConstraints
        {
            get;
        }
        #endregion

        #region public methods

        /// <summary>Creates a specific <see cref="IMultiDimOptimizerAlgorithm"/> object.
        /// </summary>
        /// <param name="optimizer">The <see cref="MultiDimOptimizer"/> object that supports a optimization problems without constraints.</param>
        /// <param name="multiDimRegionCollection">The constraints in its <see cref="MultiDimOptimizer.IConstraint"/> representation.</param>
        /// <returns>The specific <see cref="IMultiDimOptimizerAlgorithm"/> object.</returns>
        public abstract IMultiDimOptimizerAlgorithm Create(MultiDimOptimizer optimizer, MultiDimOptimizer.IConstraint[] multiDimRegionCollection);
        #endregion
    }
}