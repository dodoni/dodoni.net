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
using System.Text;
using System.Linq;
using System.Collections.Generic;

using Dodoni.MathLibrary.Miscellaneous;

namespace Dodoni.MathLibrary.Optimizer.MultiDimensional.ConstraintProviders
{
    /// <summary>Represents a simple quadratic penalty approach for constraint optimization; If an argument is outside the constraint, the return value is
    /// <para> <c>LastFunctionValueWithNoPenalty</c> + distance(x, Constraints)^2 * relativePenaltyWeight + absolutePenaltyWeight.</para>
    /// </summary>
    internal class QuadraticPenaltyAlgorithm : IMultiDimOptimizerAlgorithm
    {
        #region private members

        /// <summary>The object that serves as factory of the current object.
        /// </summary>
        private MultiDimOptimizer m_Optimizer;

        /// <summary>The objective function in its <see cref="OrdinaryMultiDimOptimizerFunction"/> representation.
        /// </summary>
        private OrdinaryMultiDimOptimizerFunction m_ObjectiveFunction;

        /// <summary>The constraints in its <see cref="IMultiDimRegion"/> representation.
        /// </summary>
        private IEnumerable<IMultiDimRegion> m_MultiDimRegions;

        /// <summary>The 'inner' optimizer algorithm without constraints.
        /// </summary>
        private IMultiDimOptimizerAlgorithm m_InnerOptimizerAlgorithm;

        /// <summary>The relative weight for the penalty function.
        /// </summary>
        private double m_RelativePenaltyWeight;

        /// <summary>The absolute weight for the penalty function.
        /// </summary>
        private double m_AbsolutePenaltyWeight;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="QuadraticPenaltyAlgorithm"/> class.
        /// </summary>
        /// <param name="optimizer">The <see cref="MultiDimOptimizer"/> object that serves as factory of the current object and solve optimization problems without constraints.</param>
        /// <param name="multiDimRegionCollection">The constraints in its <see cref="MultiDimOptimizer.IConstraint"/> representation.</param>
        /// <param name="relativePenaltyWeight">The relative weight of the penalty function.</param>
        /// <param name="absolutePenaltyWeight">The absolute weight of the penalty function.</param>
        internal QuadraticPenaltyAlgorithm(MultiDimOptimizer optimizer, IEnumerable<MultiDimOptimizer.IConstraint> multiDimRegionCollection, double relativePenaltyWeight = 1e20, double absolutePenaltyWeight = 0.0)
        {
            m_Optimizer = optimizer ?? throw new ArgumentNullException(nameof(optimizer));
            m_MultiDimRegions = CheckConstraintConsistency(multiDimRegionCollection);
            Dimension = multiDimRegionCollection.FirstOrDefault().Dimension;

            m_InnerOptimizerAlgorithm = optimizer.Create(Dimension);
            m_RelativePenaltyWeight = relativePenaltyWeight;
            m_AbsolutePenaltyWeight = absolutePenaltyWeight;
        }
        #endregion

        #region public properties

        #region IMultiDimOptimizerAlgorithm Members

        /// <summary>Gets the factory for further <see cref="IMultiDimOptimizerAlgorithm"/> objects of the same type, i.e. with the same stopping condition etc.
        /// </summary>
        /// <value>The factory for further <see cref="IMultiDimOptimizerAlgorithm"/> objects of the same type.</value>
        public MultiDimOptimizer Factory
        {
            get { return m_Optimizer; }
        }

        /// <summary>Gets the dimension of the feasible region.
        /// </summary>
        /// <value>The dimension.</value>
        public int Dimension
        {
            get;
            private set;
        }

        /// <summary>Gets or sets the objective function in its <see cref="MultiDimOptimizer.IFunction"/> representation.
        /// </summary>
        /// <value>The objective function.</value>
        public MultiDimOptimizer.IFunction Function
        {
            get { return m_ObjectiveFunction; }
            set
            {
                if (value is OrdinaryMultiDimOptimizerFunction)
                {
                    m_ObjectiveFunction = (OrdinaryMultiDimOptimizerFunction)value;

                    double lastFunctionValueWithNoPenalty = Double.MaxValue;  // the first function argument should satisifed the constraints
                    m_InnerOptimizerAlgorithm.SetFunction(
                        z =>
                        {
                            double minDistance = Double.MaxValue;
                            bool isOutSide = false;

                            foreach (var multiDimRegion in m_MultiDimRegions)
                            {
                                double distance;
                                if (multiDimRegion.GetDistance(z, out distance) == PointRegionRelation.Outside)
                                {
                                    minDistance = Math.Min(minDistance, distance);
                                    isOutSide = true;
                                }
                            }

                            if (isOutSide == false)
                            {
                                lastFunctionValueWithNoPenalty = m_ObjectiveFunction.GetValue(z);
                                return lastFunctionValueWithNoPenalty;
                            }
                            return lastFunctionValueWithNoPenalty + m_RelativePenaltyWeight * minDistance * minDistance + m_AbsolutePenaltyWeight;
                        });
                }
                else
                {
                    throw new InvalidCastException();
                }
            }
        }
        #endregion

        #endregion

        #region public methods

        #region IMultiDimOptimizerAlgorithm Members

        /// <summary>Finds the minimum and argmin of <see cref="IMultiDimOptimizerAlgorithm.Function"/>.
        /// </summary>
        /// <param name="x">An array with at least <see cref="IMultiDimOptimizerAlgorithm.Dimension"/> elements which is perhaps taken into account as an initial guess of the algorithm; on exit this argument contains the argmin.</param>
        /// <param name="minimum">The minimum, i.e. the function value with respect to <paramref name="x"/> which represents the argmin (output).</param>
        /// <returns>The state of the algorithm, i.e. an indicating whether <paramref name="x"/> and <paramref name="minimum"/> contain valid data.</returns>
        public MultiDimOptimizer.State FindMinimum(double[] x, out double minimum)
        {
            return m_InnerOptimizerAlgorithm.FindMinimum(x, out minimum);
        }
        #endregion

        #region IDisposable Members

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // nothing to do here
        }
        #endregion

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            var str = new StringBuilder();
            str.AppendFormat("{0}; Quadratic penalty transformation", m_InnerOptimizerAlgorithm.Factory.Name.String);

            foreach (var constraint in m_MultiDimRegions)
            {
                str.AppendFormat("; {0}", constraint.ToString());
            }
            return str.ToString();
        }
        #endregion

        #region private methods

        /// <summary>Check whether all constrains have the same dimension etc.
        /// </summary>
        /// <param name="multiDimRegionCollection">The constraints in its <see cref="IMultiDimRegion"/> representation.</param>
        /// <returns>The dimension of each constraint.</returns>
        private static IEnumerable<IMultiDimRegion> CheckConstraintConsistency(IEnumerable<MultiDimOptimizer.IConstraint> multiDimRegionCollection)
        {
            if (multiDimRegionCollection == null)
            {
                throw new ArgumentNullException(nameof(multiDimRegionCollection));
            }

            var firstRegion = multiDimRegionCollection.FirstOrDefault();
            if (firstRegion == null)
            {
                throw new ArgumentException(nameof(multiDimRegionCollection));
            }
            var dimension = firstRegion.Dimension;
            foreach (var region in multiDimRegionCollection)
            {
                if (region.Dimension != dimension)
                {
                    throw new ArgumentException(nameof(multiDimRegionCollection));
                }
                if (region is MultiDimOptimizerConstraint)
                {
                    var con = (MultiDimOptimizerConstraint)region;
                    yield return con.RegionRepresentation;
                }
                else
                {
                    throw new ArgumentException(nameof(multiDimRegionCollection));
                }
            }
        }
        #endregion
    }
}