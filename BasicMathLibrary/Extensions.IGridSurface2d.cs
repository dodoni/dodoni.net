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

using Dodoni.BasicComponents;
using Dodoni.MathLibrary.Surfaces;

namespace Dodoni.MathLibrary
{
    public static partial class Extensions
    {
        /// <summary>Gets a value at a specified point (<paramref name="horizontalLabel"/>, <paramref name="y"/>).
        /// </summary>
        /// <param name="gridPointSurface2d">The gridPointSurface2d object.</param>
        /// <param name="horizontalLabel">The horizontal label (i.e. name of the column) of the point.</param>
        /// <param name="y">The y coordinate of the point.</param>
        /// <returns>The value of the surface at (<paramref name="horizontalLabel"/>, <paramref name="y"/>).</returns>
        public static double GetValue<THorizontalLabel, TVerticalLabel>(this IGridPointSurface2d<THorizontalLabel, TVerticalLabel> gridPointSurface2d, THorizontalLabel horizontalLabel, double y)
            where THorizontalLabel : IComparable<THorizontalLabel>
            where TVerticalLabel : IComparable<TVerticalLabel>
        {
            if (gridPointSurface2d.TryGetValue(horizontalLabel, y, out double value) == true)
            {
                return value;
            }
            throw new ArgumentOutOfRangeException(nameof(horizontalLabel), String.Format(ExceptionMessages.ArgumentIsInvalidForObject, horizontalLabel, gridPointSurface2d.GridPointMatrix.ToString() + "/Horizontal labels"));
        }

        /// <summary>Gets a value at a specified point (<paramref name="x"/>, <paramref name="verticalLabel"/>).
        /// </summary>
        /// <param name="gridPointSurface2d">The gridPointSurface2d object.</param>
        /// <param name="x">The x coordinate of the point.</param>
        /// <param name="verticalLabel">The vertical label of the point.</param>
        /// <returns>The value of the surface at (<paramref name="x"/>, <paramref name="verticalLabel"/>).</returns>
        public static double GetValue<THorizontalLabel, TVerticalLabel>(this IGridPointSurface2d<THorizontalLabel, TVerticalLabel> gridPointSurface2d, double x, TVerticalLabel verticalLabel)
            where THorizontalLabel : IComparable<THorizontalLabel>
            where TVerticalLabel : IComparable<TVerticalLabel>
        {
            if (gridPointSurface2d.TryGetValue(x, verticalLabel, out double value) == true)
            {
                return value;
            }
            throw new ArgumentOutOfRangeException(nameof(verticalLabel), String.Format(ExceptionMessages.ArgumentIsInvalidForObject, verticalLabel, gridPointSurface2d.GridPointMatrix.ToString() + "/Vertical labels"));
        }
    }
}