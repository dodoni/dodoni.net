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

using Dodoni.MathLibrary.Basics;
using Dodoni.MathLibrary.Miscellaneous;

namespace Dodoni.MathLibrary.Optimizer.MultiDimensional.ConstraintProviders
{
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
    internal class BoxTransformationAlgorithm : IMultiDimOptimizerAlgorithm
    {
        #region private members

        /// <summary>The object that serves as factory of the current object.
        /// </summary>
        private MultiDimOptimizer m_Optimizer;

        /// <summary>The objective function in its <see cref="OrdinaryMultiDimOptimizerFunction"/> representation.
        /// </summary>
        private OrdinaryMultiDimOptimizerFunction m_ObjectiveFunction;

        /// <summary>The Box constraint, i.e. a multi-dimensional interval.
        /// </summary>
        private MultiDimRegion.Interval m_BoxConstraint;

        /// <summary>The 'inner' optimizer algorithm without constraints.
        /// </summary>
        private IMultiDimOptimizerAlgorithm m_InnerOptimizerAlgorithm;

        /// <summary>A temporary variable for the argument of the objective function.
        /// </summary>
        private double[] m_TempFunctionArgument;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="BoxTransformationAlgorithm"/> class.
        /// </summary>
        /// <param name="optimizer">The <see cref="MultiDimOptimizer"/> object that serves as factory of the current object and solve optimization problems without constraints.</param>
        /// <param name="boxConstraint">The box constraint.</param>
        internal BoxTransformationAlgorithm(MultiDimOptimizer optimizer, MultiDimRegion.Interval boxConstraint)
        {
            m_Optimizer = optimizer ?? throw new ArgumentNullException(nameof(optimizer));
            m_BoxConstraint = boxConstraint ?? throw new ArgumentNullException(nameof(boxConstraint));
            m_TempFunctionArgument = new double[boxConstraint.Dimension];
            m_InnerOptimizerAlgorithm = optimizer.Create(boxConstraint.Dimension);
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
            get { return m_BoxConstraint.Dimension; }
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
                    m_InnerOptimizerAlgorithm.SetFunction(z =>
                    {
                        BLAS.Level1.dcopy(Dimension, z, m_TempFunctionArgument);
                        TransformationIntoBoxConstraint(m_TempFunctionArgument);

                        return m_ObjectiveFunction.GetValue(m_TempFunctionArgument);
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
            TransformationIntoUnconstraintRegion(x);
            var state = m_InnerOptimizerAlgorithm.FindMinimum(x, out minimum);
            TransformationIntoBoxConstraint(x);

            return state;  // perhaps the state contains information w.r.t the transformed problem etc.
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
            return String.Format("{0}; Box constraint {1}", m_InnerOptimizerAlgorithm.Factory.Name.String, m_BoxConstraint.ToString());
        }
        #endregion

        #region private methods

        /// <summary>Transform a specific vector into a vector that satisfied the box constraints.
        /// </summary>
        /// <param name="t">The specific vector to transform into a vector that satisifed the box constraints.</param>
        private void TransformationIntoBoxConstraint(double[] t)
        {
            for (int j = 0; j < Dimension; j++)
            {
                var lowerBound = m_BoxConstraint.LowerBounds[j];
                var upperBound = m_BoxConstraint.UpperBounds[j];

                switch (m_BoxConstraint.IntervalBoundTypes[j])
                {
                    case Interval.BoundaryType.Bounded:
                        var sinValue = Math.Sin(t[j]);
                        t[j] = lowerBound + (upperBound - lowerBound) * sinValue * sinValue;
                        break;

                    case Interval.BoundaryType.LowerBounded:
                        t[j] = lowerBound + t[j] * t[j];
                        break;

                    case Interval.BoundaryType.UpperBounded:
                        t[j] = upperBound - t[j] * t[j];
                        break;

                    case Interval.BoundaryType.Unbounded:
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        /// <summary>Transform a specific vector that satisifed the box constraints into a 'unconstraint' vector.
        /// </summary>
        /// <param name="t">The specific vector to transform.</param>
        private void TransformationIntoUnconstraintRegion(double[] t)
        {
            /* search for a solution x_j which satisfied 
       *  'a_j + (b_j - a_j) * \sin^2 x_j = functionArgument[j]', if a_j, b_j are bounded
       *  'a_j + x_j^2', if b_j is unbounded, a_j is bounded
       *  'b_j - x_j^2', if a_j is unbounded, b_j is bounded
       *   'x_j' if a_j and b_j are unbounded
       */
            for (int j = 0; j < Dimension; j++)
            {
                var lowerBound = m_BoxConstraint.LowerBounds[j];
                var upperBound = m_BoxConstraint.UpperBounds[j];

                switch (m_BoxConstraint.IntervalBoundTypes[j])
                {
                    case Interval.BoundaryType.Bounded:
                        t[j] = Math.Asin(Math.Sqrt((t[j] - lowerBound) / (upperBound - lowerBound)));
                        break;

                    case Interval.BoundaryType.LowerBounded:
                        t[j] = Math.Sqrt(t[j] - lowerBound);
                        break;

                    case Interval.BoundaryType.UpperBounded:
                        t[j] = Math.Sqrt(upperBound - t[j]);
                        break;

                    case Interval.BoundaryType.Unbounded:
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
        }
        #endregion
    }
}