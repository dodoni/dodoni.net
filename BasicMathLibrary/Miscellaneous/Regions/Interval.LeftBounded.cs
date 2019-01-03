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
using System.Globalization;

using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.Miscellaneous
{
    public partial class Interval
    {
        /// <summary>Represents a left-bounded one-dimensional interval [a, \infty[.
        /// </summary>
        public class LeftBounded : IOneDimRegion
        {
            #region private members

            /// <summary>The lower bound of the interval represented by the current object.
            /// </summary>
            private double m_LowerBound;
            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="LeftBounded"/> struct.
            /// </summary>
            /// <param name="lowerBound">The lower bound.</param>
            /// <exception cref="ArgumentException">Thrown, if <paramref name="lowerBound"/> is 'not a number' (NaN) or represents +/-'\infty'.</exception>
            internal LeftBounded(double lowerBound)
            {
                if (Double.IsNaN(lowerBound))
                {
                    throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, ExceptionMessages.ArgumentIsNaN, "lower bound"), nameof(lowerBound));
                }
                if (Double.IsPositiveInfinity(lowerBound))
                {
                    throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, ExceptionMessages.ArgumentIsInfinity, "lower bound"), nameof(lowerBound));
                }
                if (Double.IsNegativeInfinity(lowerBound))
                {
                    throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, ExceptionMessages.ArgumentIsMinusInfinity, "lower bound"), nameof(lowerBound));
                }
                m_LowerBound = lowerBound;
            }
            #endregion

            #region public properties

            #region IOneDimRegion Members

            /// <summary>Gets the infimum.
            /// </summary>
            /// <value>The infimum of the feasible region, perhaps <see cref="System.Double.NegativeInfinity" />.</value>
            public double Infimum
            {
                get { return m_LowerBound; }
            }

            /// <summary>Gets the supremum.
            /// </summary>
            /// <value>The supremum of the feasible region, perhaps <see cref="System.Double.PositiveInfinity" />.</value>
            public double Supremum
            {
                get { return Double.PositiveInfinity; }
            }
            #endregion

            #endregion

            #region public methods

            #region IOneDimRegion Members

            /// <summary>Gets a value indicating whether a specific point is inside the region.
            /// </summary>
            /// <param name="x">The argument.</param>
            /// <returns>A value indicating whether <paramref name="x" /> is inside the region.</returns>
            public PointRegionRelation GetPointPosition(double x)
            {
                return (x >= m_LowerBound) ? PointRegionRelation.InsideOrBoundaryPoint : PointRegionRelation.Outside;
            }

            /// <summary>Gets a value indicating whether a specific point is inside the region.
            /// </summary>
            /// <param name="x">The argument, i.e. a point of dimension <see cref="IMultiDimRegion.Dimension" />.</param>
            /// <param name="distance">The distance to the region represented by the current instance (output).</param>
            /// <returns>A value indicating whether <paramref name="x" /> is inside the region and has some strictly positive distance to the region represented by the current instance.</returns>
            public PointRegionRelation GetDistance(double x, out double distance)
            {
                if (x >= m_LowerBound)
                {
                    distance = 0.0;
                    return PointRegionRelation.InsideOrBoundaryPoint;
                }
                distance = m_LowerBound - x;
                return PointRegionRelation.Outside;
            }
            #endregion

            /// <summary>Returns the fully qualified type name of this instance.
            /// </summary>
            /// <returns>A <see cref="T:System.String"/> containing a fully qualified type name.</returns>
            public override string ToString()
            {
                return String.Format(@"[{0}; \infty[", m_LowerBound.ToString());
            }
            #endregion

            #region public static methods

            /// <summary>Creates a specific <see cref="Interval.LeftBounded"/> object.
            /// </summary>
            /// <param name="lowerBound">The lower bound.</param>
            /// <returns>The specified <see cref="Interval.LeftBounded"/> object.</returns>
            /// <exception cref="ArgumentException">Thrown, if <paramref name="lowerBound"/> is 'not a number' (NaN) or represents +/-'\infty'.</exception>
            public static LeftBounded Create(double lowerBound)
            {
                return new LeftBounded(lowerBound);
            }
            #endregion
        }
    }
}