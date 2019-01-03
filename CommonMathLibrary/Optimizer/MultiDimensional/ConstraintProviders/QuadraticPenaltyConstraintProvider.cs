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

namespace Dodoni.MathLibrary.Optimizer.MultiDimensional.ConstraintProviders
{
    /// <summary>Serves as factory for the configuration of a quadratic penalty approach in a multi-dimensional optimization algorithm; If an argument is outside the constraint, the return value is
    /// <para> <c>LastFunctionValueWithNoPenalty</c> + distance(x, Constraints)^2 * relativePenaltyWeight + absolutePenaltyWeight.</para>
    /// </summary>
    public class QuadraticPenaltyConstraintProvider : MultiDimOptimizerConstraintProvider
    {
        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="QuadraticPenaltyConstraintProvider"/> class.
        /// </summary>
        /// <param name="relativePenaltyWeight">The relative weight of the penalty function.</param>
        /// <param name="absolutePenaltyWeight">The absolute weight of the penalty function.</param>
        internal QuadraticPenaltyConstraintProvider(double relativePenaltyWeight = 1e20, double absolutePenaltyWeight = 0.0)
        {
            RelativePenaltyWeight = relativePenaltyWeight;
            AbsolutePenaltyWeight = absolutePenaltyWeight;
        }
        #endregion

        #region public properties

        /// <summary>Gets the constraint types which are supported.
        /// </summary>
        /// <value>The constraint types which are supported.</value>
        public override MultiDimOptimizerConstraintFactory.ConstraintType SupportedConstraints
        {
            get { return MultiDimOptimizerConstraintFactory.ConstraintType.All; }
        }

        /// <summary>Gets the relative weight of the penalty function.            
        /// </summary>
        /// <value>The relative weight of the penalty function.</value>
        public double RelativePenaltyWeight
        {
            get;
            private set;
        }

        /// <summary>Gets the absolute weight of the penalty function.
        /// </summary>
        /// <value>The absolute weight of the penalty function.</value>
        public double AbsolutePenaltyWeight
        {
            get;
            private set;
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
            return new QuadraticPenaltyAlgorithm(optimizer, multiDimRegionCollection, RelativePenaltyWeight, AbsolutePenaltyWeight);
        }

        /// <summary>Creates a specific <see cref="MultiDimOptimizerConstraintProvider"/> object.
        /// </summary>
        /// <param name="relativePenaltyWeight">The relative weight of the penalty function.</param>
        /// <param name="absolutePenaltyWeight">The absolute weight of the penalty function.</param>
        /// <returns>The specified <see cref="MultiDimOptimizerConstraintProvider"/> object.</returns>
        public MultiDimOptimizerConstraintProvider Create(double relativePenaltyWeight = 1e20, double absolutePenaltyWeight = 0.0)
        {
            return new QuadraticPenaltyConstraintProvider(relativePenaltyWeight, absolutePenaltyWeight);
        }

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return "Quadratic penalty transformation";
        }
        #endregion
    }
}
