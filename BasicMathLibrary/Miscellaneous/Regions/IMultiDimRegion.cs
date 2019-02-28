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
    /// <summary>Serves as interface for a multi-dimensional (closed) region; for example the domain of a specific objective function with respect to a specific optimization algorithm etc.
    /// </summary>
    public interface IMultiDimRegion
    {
        /// <summary>Gets the dimension of the region.
        /// </summary>
        /// <value>The dimension.</value>
        int Dimension { get; }

        /// <summary>Gets a value indicating whether a specific point is inside the region.
        /// </summary>
        /// <param name="x">The argument, i.e. a point of dimension <see cref="IMultiDimRegion.Dimension"/>.</param>
        /// <param name="tolerance">Some tolerance which will be perhaps taken into account.</param>
        /// <returns>A value indicating whether <paramref name="x"/> is inside the represented region.</returns>
        PointRegionRelation GetPointPosition(double[] x, double tolerance = MachineConsts.Epsilon);

        /// <summary>Gets a value indicating whether a specific point is inside the region.
        /// </summary>
        /// <param name="x">The argument, i.e. a point of dimension <see cref="IMultiDimRegion.Dimension"/>.</param>
        /// <param name="distance">The distance to the region represented by the current instance (output).</param>
        /// <param name="tolerance">Some tolerance which will be perhaps taken into account.</param>
        /// <returns>A value indicating whether <paramref name="x"/> is inside the region and has some strictly positive distance to the region represented by the current instance.</returns>
        PointRegionRelation GetDistance(double[] x, out double distance, double tolerance = MachineConsts.Epsilon);
    }
}