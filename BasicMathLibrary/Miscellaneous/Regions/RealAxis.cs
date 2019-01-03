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
    /// <summary>Represents the real axis, i.e. the set of real numbers.
    /// </summary>
    public struct RealAxis : IOneDimRegion
    {
        #region public properties

        #region IOneDimRegion Members

        /// <summary>Gets the infimum.
        /// </summary>
        /// <value>The infimum of the feasible region, perhaps <see cref="System.Double.NegativeInfinity"/>.</value>
        public double Infimum
        {
            get { return Double.NegativeInfinity; }
        }

        /// <summary>Gets the supremum.
        /// </summary>
        /// <value>The supremum of the feasible region, perhaps <see cref="System.Double.PositiveInfinity"/>.</value>
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
            return PointRegionRelation.InsideOrBoundaryPoint;
        }

        /// <summary>Gets a value indicating whether a specific point is inside the region.
        /// </summary>
        /// <param name="x">The argument, i.e. a point of dimension <see cref="IMultiDimRegion.Dimension" />.</param>
        /// <param name="distance">The distance to the region represented by the current instance (output).</param>
        /// <returns>A value indicating whether <paramref name="x" /> is inside the region and has some strictly positive distance to the region represented by the current instance.</returns>
        public PointRegionRelation GetDistance(double x, out double distance)
        {
            distance = 0.0;
            return PointRegionRelation.InsideOrBoundaryPoint;
        }
        #endregion

        /// <summary>Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> containing a fully qualified type name.</returns>
        public override string ToString()
        {
            return @"]-\infty, \infty[";
        }
        #endregion
    }
}