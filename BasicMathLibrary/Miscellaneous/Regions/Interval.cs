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
    /// <summary>Serves as factory for one-dimensional intervals in its <see cref="IOneDimRegion"/> representation.
    /// </summary>
    public partial class Interval
    {
        #region nested enumerations

        /// <summary>The type of the boundary of a specific one-dimensional interval.
        /// </summary>
        [Flags]
        public enum BoundaryType
        {
            /// <summary>The interval is unbounded, i.e. \R.
            /// </summary>
            Unbounded = 0x00,

            /// <summary>The interval is lower-bounded, i.e. [a,\infty[.
            /// </summary>
            LowerBounded = 0x01,

            /// <summary>The interval is upper-bounded, i.e. ]-\infty, b].
            /// </summary>
            UpperBounded = 0x02,

            /// <summary>The interval is bounded, i.e. [a,b].
            /// </summary>
            Bounded = LowerBounded | UpperBounded
        }
        #endregion

        #region public (static) readonly members

        /// <summary>Represents the real axis.
        /// </summary>
        public static readonly RealAxis RealAxis = new RealAxis();

        /// <summary>Presents [0, \infty[.
        /// </summary>
        public static readonly Interval.LeftBounded PositiveNumbers;

        /// <summary>Presents [\epsilon, \infty[, where \epsilon is represented by <see cref="MachineConsts.Epsilon"/>.
        /// </summary>
        public static readonly Interval.LeftBounded StrictPositiveNumbers;
        #endregion

        #region (static) constructor

        /// <summary>Initializes the <see cref="Interval" /> class.
        /// </summary>
        static Interval()
        {
            PositiveNumbers = new Interval.LeftBounded(0.0);
            StrictPositiveNumbers = new Interval.LeftBounded(MachineConsts.Epsilon);
        }
        #endregion

        #region public (static) methods

        /// <summary>Creates a specific <see cref="IOneDimRegion"/> object.
        /// </summary>
        /// <param name="lowerBound">The lower bound; <see cref="System.Double.NaN"/> and <see cref="System.Double.NegativeInfinity"/> are allowed.</param>
        /// <param name="upperBound">The upper bound; <see cref="System.Double.NaN"/> and <see cref="System.Double.PositiveInfinity"/> are allowed.</param>
        /// <returns>The specified <see cref="IOneDimRegion"/> object.</returns>
        public static IOneDimRegion Create(double lowerBound = Double.NegativeInfinity, double upperBound = Double.PositiveInfinity)
        {
            if (Double.IsNaN(lowerBound) || Double.IsNegativeInfinity(lowerBound))
            {
                if (Double.IsNaN(upperBound) || Double.IsPositiveInfinity(upperBound))
                {
                    return RealAxis;
                }
                return new Interval.RightBounded(upperBound);
            }
            else // lowerBound is a specific number (or the invalid value +\infinity)
            {
                if ((Double.IsNaN(upperBound) == false) && (Double.IsPositiveInfinity(upperBound) == false))
                {
                    return new Interval.Bounded(lowerBound, upperBound);
                }
                return new Interval.LeftBounded(lowerBound);
            }
        }

        /// <summary>Creates a specific <see cref="IOneDimRegion"/> object that represents the intersection of two intervals.
        /// </summary>
        /// <param name="interval1">The first interval; ignored if <c>null</c>.</param>
        /// <param name="interval2">The second interval; ignored if <c>null</c>.</param>
        /// <returns>The specific <see cref="IOneDimRegion"/> that represents the intersection of <paramref name="interval1"/> and <paramref name="interval2"/>.</returns>
        public static IOneDimRegion Create(IOneDimRegion interval1, IOneDimRegion interval2)
        {
            if ((interval1 == null) || (interval1 is RealAxis))
            {
                return (interval2 == null) ? RealAxis : interval2;
            }
            if ((interval2 == null) || (interval2 is RealAxis))
            {
                return (interval1 == null) ? RealAxis : interval1;
            }
            double a = DoMath.Max(interval1.Infimum, interval2.Infimum);
            double b = DoMath.Min(interval1.Supremum, interval2.Supremum);
            return Create(a, b);
        }
        #endregion
    }
}