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
using System.Collections.Generic;

using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.GridPointCurves
{
    /// <summary>Serves as interface for a real valued curve specified by a set of grid points and a interpolation/extrapolation or curve parametrization etc.
    /// </summary>
    public interface IGridPointCurve : IRealValuedCurve, IOperable, IInfoOutputQueriable
    {
        /// <summary>Gets the number of grid points.
        /// </summary>
        /// <value>The number of grid points.</value>
        int GridPointCount
        {
            get;
        }

        /// <summary>Gets a value indicating whether this instance is read-only.
        /// </summary>
        /// <value><c>true</c> if this instance is read-only; otherwise, <c>false</c>.</value>
        bool IsReadOnly
        {
            get;
        }

        /// <summary>Gets the grid point arguments, i.e. the labels (on the x-axis) of the curve in its <see cref="System.Double"/> representation.
        /// </summary>
        /// <value>The grid point arguments.</value>
        IList<double> GridPointArguments
        {
            get;
        }

        /// <summary>Gets the grid point values with respect to <see cref="IGridPointCurve.GridPointArguments"/>.
        /// </summary>
        /// <value>The grid point values.</value>
        IList<double> GridPointValues
        {
            get;
        }

        /// <summary>Gets a value indicating the fitting quality, i.e. whether grid points are meet exactly. 
        /// </summary>
        /// <value>A value indicating the fitting quality, i.e. whether grid points are meet exactly.</value>
        FittingQuality FittingQuality
        {
            get;
        }

        /// <summary>Gets the left localness level for a specific grid point, i.e.
        /// changing grid point (t_j,x_j) implies changes on the interval ]t_{j-leftLocalnessLevel}, t_{j+rightLocalnessLevel}[.
        /// </summary>
        /// <param name="gridPointIndex">The null-based index of the grid point (t_j,x_j).</param>
        /// <returns>The left localness level with respect to the grid point (t_j,x_j), where j is represented by <paramref name="gridPointIndex"/>
        /// i.e. changing grid point (t_j,x_j) implies changes on the interval ]t_{j-leftLocalnessLevel}, t_{j+rightLocalnessLevel}[.</returns>
        int GetLeftLocalnessLevel(int gridPointIndex);

        /// <summary>Gets the right localness level for a specific grid point, i.e.
        /// changing grid point (t_j,x_j) implies changes on the interval ]t_{j-leftLocalnessLevel}, t_{j+rightLocalnessLevel}[.
        /// </summary>
        /// <param name="gridPointIndex">The null-based index of the grid point (t_j,x_j).</param>
        /// <returns>The right localness level with respect to the grid point (t_j,x_j), where j is represented by <paramref name="gridPointIndex"/>
        /// i.e. changing grid point (t_j,x_j) implies changes on the interval ]t_{j-leftLocalnessLevel}, t_{j+rightLocalnessLevel}[.</returns>
        int GetRightLocalnessLevel(int gridPointIndex);

        /// <summary>Removes all elements from the <see cref="IGridPointCurve"/> object.
        /// </summary>
        /// <remarks>This method set the <see cref="IOperable.IsOperable"/> flag to <c>false</c>.</remarks>
        /// <exception cref="InvalidOperationException">Thrown, if <see cref="IGridPointCurve.IsReadOnly"/> is <c>true</c>.</exception>
        void Clear();

        /// <summary>Removes a specific grid point.
        /// </summary>
        /// <param name="gridPointIndex">The null-based index of the grid point.</param>
        /// <remarks>This method set the <see cref="IOperable.IsOperable"/> flag to <c>false</c>.</remarks>
        /// <exception cref="InvalidOperationException">Thrown, if <see cref="IGridPointCurve.IsReadOnly"/> is <c>true</c>.</exception>
        void RemoveAt(int gridPointIndex);

        /// <summary>Changes a specific grid point argument.
        /// </summary>
        /// <param name="gridPointIndex">The null-based index of the grid point.</param>
        /// <param name="argument">The argument of the specified grid point, i.e. the <see cref="System.Double"/> representation of the label of the x-axis.</param>
        /// <remarks>This method set the <see cref="IOperable.IsOperable"/> flag to <c>false</c>.</remarks>
        /// <exception cref="InvalidOperationException">Thrown, if <see cref="IGridPointCurve.IsReadOnly"/> is <c>true</c>.</exception>
        void SetGridPointArgument(int gridPointIndex, double argument);

        /// <summary>Changes the value component of a specific grid point.
        /// </summary>
        /// <param name="gridPointIndex">The null-based index of the grid point.</param>
        /// <param name="value">The value.</param>
        /// <remarks>This method set the <see cref="IOperable.IsOperable"/> flag to <c>false</c>.</remarks>
        /// <exception cref="InvalidOperationException">Thrown, if <see cref="IGridPointCurve.IsReadOnly"/> is <c>true</c>.</exception>
        void SetValue(int gridPointIndex, double value);

        /// <summary>Updates the current instance. This method may change <see cref="IOperable.IsOperable"/>.
        /// </summary>
        /// <remarks>Call this method after grid points have been removed, modified or added and before trying to compute a value at a specified argument.
        /// <para>In general this method sets the <see cref="IOperable.IsOperable"/> flag to <c>true</c>.</para></remarks>
        void Update();
    }

    /// <summary>Serves as interface for a real valued curve specified by a set of grid points and a interpolation/extrapolation or curve parametrization etc.
    /// </summary>
    /// <typeparam name="TLabel">The type of the labels.</typeparam>
    public interface IGridPointCurve<TLabel> : IGridPointCurve
    {
        /// <summary>Gets the labels of the grid points
        /// </summary>
        /// <value>The grid point labels.</value>
        IList<TLabel> GridPointLabels
        {
            get;
        }

        /// <summary>Adds a specific grid point.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="argument">The argument, i.e. the <see cref="System.Double"/> representation of <paramref name="label"/>.</param>
        /// <param name="value">The value.</param>
        /// <returns>The null-based index of the grid point in the curve, the grid points should be ordered with respect to the argument.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if <see cref="IGridPointCurve.IsReadOnly"/> is <c>true</c>.</exception>
        int Add(TLabel label, double argument, double value);

        /// <summary>Adds a collection of grid points.
        /// </summary>
        /// <param name="values">A collection of grid points, where the first component is the label, the second component is the argument, i.e. the <see cref="System.Double"/> representation of the label,
        /// and the third component of the triple is the value.</param>
        /// <param name="isSorted">A value indicating whether the <see cref="System.Double"/> representation of the labels in <paramref name="values"/> are sorted in ascending order.</param>
        /// <exception cref="InvalidOperationException">Thrown, if <see cref="IGridPointCurve.IsReadOnly"/> is <c>true</c>.</exception>
        void AddRange(IEnumerable<Tuple<TLabel, double, double>> values, bool isSorted = false);

        /// <summary>Removes a specific grid point.
        /// </summary>
        /// <param name="label">The label of the grid point to remove.</param>
        /// <returns>A value indicating whether the grid point with respect to <paramref name="label"/> has been removed.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if <see cref="IGridPointCurve.IsReadOnly"/> is <c>true</c>.</exception>
        bool TryRemove(TLabel label);

        /// <summary>Changes the argument of a specific grid point.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="argument">The argument, i.e. the <see cref="System.Double"/> representation of <paramref name="label"/>.</param>
        /// <returns>A value indicating whether the argument of the specific grid point has been changed to <paramref name="argument"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if <see cref="IGridPointCurve.IsReadOnly"/> is <c>true</c>.</exception>
        bool TrySetGridPointArgument(TLabel label, double argument);

        /// <summary>Changes the value component of a specific grid point.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="value">The value.</param>
        /// <returns>A value indicating whether the <paramref name="label"/> exists in the curve and the value component has been changed to <paramref name="value"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if <see cref="IGridPointCurve.IsReadOnly"/> is <c>true</c>.</exception>
        bool TrySetValue(TLabel label, double value);

        /// <summary>Returns a read-only <see cref="IGridPointCurve{TLabel}"/> wrapper for the current instance.
        /// </summary>
        /// <returns>A <see cref="IGridPointCurve{TLabel}"/> object that acts as a read-only wrapper around the current instance.</returns>
        IGridPointCurve<TLabel> AsReadOnly();
    }
}