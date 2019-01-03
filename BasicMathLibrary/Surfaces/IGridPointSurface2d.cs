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

namespace Dodoni.MathLibrary.Surfaces
{
    /// <summary>Serves as interface for grid points that represents the building block for a two-dimensional surface.
    /// </summary>
    /// <typeparam name="THorizontalLabel">The type of the horizontal label.</typeparam>
    /// <typeparam name="TVerticalLabel">The type of the vertical label.</typeparam>
    public interface IGridPointSurface2d<THorizontalLabel, TVerticalLabel> : ISurface2d
        where THorizontalLabel : IComparable<THorizontalLabel>
        where TVerticalLabel : IComparable<TVerticalLabel>
    {
        /// <summary>Gets the (read-only) grid point matrix. Missing values are perhaps filled with respect to a specific replenish approach.
        /// </summary>
        /// <value>The grid points of the two-dimensional surface.</value>
        LabelMatrix<THorizontalLabel, TVerticalLabel> GridPointMatrix
        {
            get;
        }

        /// <summary>Gets a value at a specified point (<paramref name="columnIndex"/>, <paramref name="y"/>).
        /// </summary>
        /// <param name="columnIndex">The null-based column index of the point.</param>
        /// <param name="y">The y coordinate of the point.</param>
        /// <returns>The value of the surface at (<paramref name="columnIndex"/>, <paramref name="y"/>).</returns>
        double GetValue(int columnIndex, double y);

        /// <summary>Gets a value at a specified point (<paramref name="horizontalLabel"/>, <paramref name="y"/>).
        /// </summary>
        /// <param name="horizontalLabel">The horizontal label (i.e. name of the column) of the point.</param>
        /// <param name="y">The y coordinate of the point.</param>
        /// <param name="value">The value of the surface at (<paramref name="horizontalLabel"/>, <paramref name="y"/>).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        bool TryGetValue(THorizontalLabel horizontalLabel, double y, out double value);

        /// <summary>Gets a value at a specified point (<paramref name="x"/>, <paramref name="rowIndex"/>).
        /// </summary>
        /// <param name="x">The x coordinate of the point.</param>
        /// <param name="rowIndex">The null-based row index of the point.</param>
        /// <returns>The value of the surface at (<paramref name="x"/>, <paramref name="rowIndex"/>).</returns>
        double GetValue(double x, int rowIndex);

        /// <summary>Gets a value at a specified point (<paramref name="x"/>, <paramref name="verticalLabel"/>).
        /// </summary>
        /// <param name="x">The x coordinate of the point.</param>
        /// <param name="verticalLabel">The vertical label of the point.</param>
        /// <param name="value">The value of the surface at (<paramref name="x"/>, <paramref name="verticalLabel"/>).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        bool TryGetValue(double x, TVerticalLabel verticalLabel, out double value);
    }
}