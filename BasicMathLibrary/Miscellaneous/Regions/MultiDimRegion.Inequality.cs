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

namespace Dodoni.MathLibrary.Miscellaneous
{
    public partial class MultiDimRegion
    {
        /// <summary>Represents a specific multi-dimensional region that is defined via a function c(x), i.e. a point x will be accepted to lie in the region if and only if 
        /// c(x) is smaller than a specific tolerance level.
        /// </summary>
        public class Inequality : IMultiDimRegion
        {
            #region private members

            /// <summary>The delegate that points to the constraint function c(x).
            /// </summary>
            private Func<double[], double[], double> m_ConstraintFunction;
            #endregion

            #region public constructors

            /// <summary>Initializes a new instance of the <see cref="Inequality" /> class.
            /// </summary>
            /// <param name="dimension">The dimension of the feasible set.</param>
            /// <param name="constraintFunction">The inequality constraint function c(x), i.e. a argument x will be accepted iff c(x) &lt; <paramref name="tolerance"/>,
            /// where the first argument is the point where to evalute and the second argument contains the gradient (perhaps <c>null</c>), i.e. the partial derivatives of the function at the first argument; 
            /// the return value is the value of the function at the first argument.</param>
            /// <param name="tolerance">The tolerance to take into account.</param>
            public Inequality(int dimension, Func<double[], double[], double> constraintFunction, double tolerance = MachineConsts.Epsilon)
            {
                if (dimension < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(dimension));
                }
                Dimension = dimension;
                if (tolerance < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(tolerance));
                }
                Tolerance = tolerance;
                m_ConstraintFunction = constraintFunction ?? throw new ArgumentNullException(nameof(constraintFunction));
            }
            #endregion

            #region public properties

            #region IMultiDimRegion Members

            /// <summary>Gets the dimension of the region.
            /// </summary>
            /// <value>The dimension.</value>
            public int Dimension
            {
                get;
                private set;
            }
            #endregion

            /// <summary>Gets the tolerance.
            /// </summary>
            /// <value>The tolerance.</value>
            public double Tolerance
            {
                get;
                private set;
            }
            #endregion

            #region public methods

            #region IMultiDimRegion Members

            /// <summary>Gets a value indicating whether a specific point is inside the region.
            /// </summary>
            /// <param name="x">The argument, i.e. a point of dimension <see cref="IMultiDimRegion.Dimension" />.</param>
            /// <param name="tolerance">Some tolerance which will not be taken into account in this implementation.</param>
            /// <returns>A value indicating whether <paramref name="x" /> is inside the represented region.</returns>
            public PointRegionRelation GetPointPosition(double[] x, double tolerance = MachineConsts.Epsilon)
            {
                return (GetValue(x) <= Tolerance) ? PointRegionRelation.InsideOrBoundaryPoint : PointRegionRelation.Outside;
            }

            /// <summary>Gets a value indicating whether a specific point is inside the region.
            /// </summary>
            /// <param name="x">The argument, i.e. a point of dimension <see cref="IMultiDimRegion.Dimension" />.</param>
            /// <param name="distance">The distance to the region represented by the current instance (output).</param>
            /// <param name="tolerance">Some tolerance which will not be taken into account in this implementation.</param>
            /// <returns>A value indicating whether <paramref name="x" /> is inside the region and has some strictly positive distance to the region represented by the current instance.</returns>
            public PointRegionRelation GetDistance(double[] x, out double distance, double tolerance = MachineConsts.Epsilon)
            {
                distance = GetValue(x);
                return (distance <= Tolerance) ? PointRegionRelation.InsideOrBoundaryPoint : PointRegionRelation.Outside;
            }
            #endregion

            /// <summary>Gets the value of the constraint function c(x).
            /// </summary>
            /// <param name="x">The argument x.</param>
            /// <param name="gradient">If != <c>null</c> the gradient of the constraint function at <paramref name="x"/> will be stored on exit (output).</param>
            /// <returns>The value of the constraint function c at point <paramref name="x"/>.</returns>
            public double GetValue(double[] x, double[] gradient = null)
            {
                return m_ConstraintFunction(x, gradient);
            }
            #endregion

            #region public static methods

            /// <summary>Creates a specified <see cref="Inequality"/> object.
            /// </summary>
            /// <param name="dimension">The dimension of the feasible set.</param>
            /// <param name="constraintFunction">The inequality constraint function c(x), i.e. a argument x will be accepted iff c(x) &lt; <paramref name="tolerance"/>,
            /// where the first argument is the point where to evalute and the second argument contains the gradient (perhaps <c>null</c>), i.e. the partial derivatives of the function at the first argument; 
            /// the return value is the value of the function at the first argument.</param>
            /// <param name="tolerance">The tolerance to take into account.</param>
            /// <returns>The specified <see cref="Inequality"/> object.</returns>
            public static Inequality Create(int dimension, Func<double[], double[], double> constraintFunction, double tolerance = MachineConsts.Epsilon)
            {
                return new Inequality(dimension, constraintFunction, tolerance);
            }
            #endregion
        }
    }
}