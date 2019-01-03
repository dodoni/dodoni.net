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
        /// <summary>Represents a two-sided bounded one-dimensional interval.
        /// </summary>
        public class Bounded : IOneDimRegion
        {
            #region private members

            /// <summary>The left limit, i.e. the lower bound.
            /// </summary>
            private double m_LowerBound;

            /// <summary>The right limit, i.e. the upper bound.
            /// </summary>
            private double m_UpperBound;
            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="Bounded"/> class.
            /// </summary>
            /// <param name="lowerBound">The lower bound.</param>
            /// <param name="upperBound">The upper bound.</param>
            /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="upperBound"/> is lower or equal than <paramref name="lowerBound"/> or the interval length is smaller than <see cref="MachineConsts.Epsilon"/>, or
            /// one of the arguments is 'not a number' or (minus) infinity.</exception>
            internal Bounded(double lowerBound, double upperBound)
            {
                if (Double.IsNaN(lowerBound) || Double.IsInfinity(lowerBound))
                {
                    throw new ArgumentOutOfRangeException(nameof(lowerBound), String.Format(CultureInfo.InvariantCulture, ExceptionMessages.ArgumentIsInvalid, lowerBound));
                }
                if (Double.IsNaN(upperBound) || Double.IsInfinity(upperBound))
                {
                    throw new ArgumentOutOfRangeException(nameof(upperBound), String.Format(CultureInfo.InvariantCulture, ExceptionMessages.ArgumentIsInvalid, upperBound));
                }
                if ((upperBound <= lowerBound) || (upperBound - lowerBound < MachineConsts.Epsilon))
                {
                    throw new ArgumentOutOfRangeException(nameof(lowerBound) + "," + nameof(upperBound), String.Format(CultureInfo.InvariantCulture, ExceptionMessages.ArgumentOutOfRangeGreaterEqual, upperBound, lowerBound));
                }
                m_LowerBound = lowerBound;
                m_UpperBound = upperBound;
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
                get { return m_UpperBound; }
            }
            #endregion

            /// <summary>Gets the diameter, i.e. the length of the (bounded) interval.
            /// </summary>
            /// <value>The (finite) diameter of the one-dimensional interval.</value>
            /// <remarks>This property will return <see cref="Supremum"/> minus <see cref="Infimum"/>.</remarks>
            public double Diameter
            {
                get { return m_UpperBound - m_LowerBound; }
            }
            #endregion

            #region public methods

            #region IOneDimRegion Members

            /// <summary>Gets a value indicating whether a specific point is inside the region.
            /// </summary>
            /// <param name="x">The argument.</param>
            /// <returns>A value indicating whether <paramref name="x" /> is inside the region.</returns>
            public PointRegionRelation GetPointPosition(double x)
            {
                if ((x < m_LowerBound) || (x > m_UpperBound))  // 'false' if 'm_LowerBound' = NaN and 'm_UpperBound' = NaN
                {
                    return PointRegionRelation.Outside;
                }
                return PointRegionRelation.InsideOrBoundaryPoint;
            }

            /// <summary>Gets a value indicating whether a specific point is inside the region.
            /// </summary>
            /// <param name="x">The argument, i.e. a point of dimension <see cref="IMultiDimRegion.Dimension" />.</param>
            /// <param name="distance">The distance to the region represented by the current instance (output).</param>
            /// <returns>A value indicating whether <paramref name="x" /> is inside the region and has some strictly positive distance to the region represented by the current instance.</returns>
            public PointRegionRelation GetDistance(double x, out double distance)
            {
                if (x < m_LowerBound)
                {
                    distance = m_LowerBound - x;
                    return PointRegionRelation.Outside;
                }
                if (x > m_UpperBound)
                {
                    distance = x - m_UpperBound;
                    return PointRegionRelation.Outside;
                }
                distance = 0.0;
                return PointRegionRelation.InsideOrBoundaryPoint;
            }
            #endregion

            /// <summary>Returns a <see cref="System.String"/> that represents the current <see cref="Bounded"/> instance.
            /// </summary>
            /// <returns>A <see cref="System.String"/> that represents the current <see cref="Bounded"/> instance.</returns>
            public override string ToString()
            {
                return String.Format("[{0};{1}]", m_LowerBound.ToString(), m_UpperBound.ToString());
            }
            #endregion

            #region public static methods

            /// <summary>Creates a specific <see cref="Interval.Bounded"/> object.
            /// </summary>
            /// <param name="lowerBound">The lower bound.</param>
            /// <param name="upperBound">The upper bound.</param>
            /// <returns>The specified <see cref="Interval.Bounded"/> object.</returns>
            /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="upperBound"/> is lower or equal than <paramref name="lowerBound"/> or the interval length is smaller than <see cref="MachineConsts.Epsilon"/>, or
            /// one of the arguments is 'not a number' or (minus) infinity.</exception>
            public static Bounded Create(double lowerBound, double upperBound)
            {
                return new Bounded(lowerBound, upperBound);
            }
            #endregion
        }
    }
}