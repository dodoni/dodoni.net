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
namespace Dodoni.MathLibrary.Miscellaneous
{
    /// <summary>Represents the relationship of a specific point with respect to a <see cref="IMultiDimRegion"/> instance that represents a set of points, thus a [mathematical] region/domain.
    /// </summary>
    /// <remarks>Because of possible rounding errors we do not distinguish between some boundary point and points which are inside the region. Therefore we do not distinguish between open and closed sets.</remarks>
    public enum PointRegionRelation
    {
        /// <summary>The point is outside the region and has some strictly positive distance to the region.
        /// </summary>
        /// <remarks>The point is neither an element of the region nor a boundary point.</remarks>
        Outside = 0,

        /// <summary>The point is inside the region or some boundary point, in any case the distance to the region is <c>0.0</c>.
        /// </summary>
        InsideOrBoundaryPoint = 1
    }
}