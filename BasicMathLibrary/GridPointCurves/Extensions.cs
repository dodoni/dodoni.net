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
using System.Linq;
using System.Collections.Generic;

namespace Dodoni.MathLibrary.GridPointCurves
{
    /// <summary>Provides extension methods for classes of the namespace <c>Dodoni.MathLibrary.GridPointCurves</c>.
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static partial class Extensions
    {
        /// <summary>Adds a specific grid point.
        /// </summary>
        /// <param name="gridPointCurve">The grid point curve object.</param>
        /// <param name="argument">The argument, i.e. the <see cref="System.Double"/> representation of the label (x-coordinate).</param>
        /// <param name="value">The value (i.e. y-coordinate).</param>
        /// <returns>The null-based index of the grid point in the curve, the grid points should be ordered with respect to the argument.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if <see cref="IGridPointCurve.IsReadOnly"/> is <c>true</c>.</exception>
        public static int Add(this IGridPointCurve<double> gridPointCurve, double argument, double value)
        {
            return gridPointCurve.Add(argument, argument, value);
        }

        /// <summary>Adds a collection of grid points.
        /// </summary>
        /// <param name="gridPointCurve">The grid point curve object.</param>
        /// <param name="values">A collection of grid points, where the first component is the argument, i.e. the <see cref="System.Double"/> representation of the label,
        /// and the second component of the triple is the value.</param>
        /// <param name="isSorted">A value indicating whether the <see cref="System.Double"/> representation of the labels in <paramref name="values"/> are sorted in ascending order.</param>
        /// <exception cref="InvalidOperationException">Thrown, if <see cref="IGridPointCurve.IsReadOnly"/> is <c>true</c>.</exception>
        public static void AddRange(this IGridPointCurve<double> gridPointCurve, IEnumerable<Tuple<double, double>> values, bool isSorted = false)
        {
            gridPointCurve.AddRange(values.Select(xy => Tuple.Create(xy.Item1, xy.Item1, xy.Item2)), isSorted);
        }
    }
}