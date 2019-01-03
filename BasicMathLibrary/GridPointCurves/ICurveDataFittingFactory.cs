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
using System.Text;
using System.Collections.Generic;

using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.GridPointCurves
{
    /// <summary>Serves as interface for a <see cref="ICurveDataFitting"/> factory, i.e. for interpolations and (curve) parametrizations.
    /// </summary>
    public interface ICurveDataFittingFactory : IIdentifierNameable, IAnnotatable
    {
        /// <summary>Gets a value indicating the fitting quality, i.e. whether grid points are meet exactly. 
        /// </summary>
        /// <value>A value indicating the fitting quality, i.e. whether grid points are meet exactly.</value>
        FittingQuality FittingQuality
        {
            get;
        }

        /// <summary>Gets the minimal number of grid points required for a successfully application of the interpolation/parametrization approach.
        /// </summary>
        /// <value>The minimal number of grid points required for a successfully application of the interpolation/parametrization approach.</value>
        /// <remarks>In general two grid points are required.</remarks>
        int MinimalRequiredNumberOfGridPoints
        {
            get;
        }

        /// <summary>Gets a value indicating whether this instance represents a local approach.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is local approach; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>In the case of a local approach call <see cref="ICurveDataFittingFactory.GetLeftLocalnessLevel(int, int)"/> and <see cref="ICurveDataFittingFactory.GetRightLocalnessLevel(int, int)"/> 
        /// for the left and right localness level.
        /// <para>In the case of a global approach all grid points are required for the curve interpolation.</para></remarks>
        bool IsLocalApproach
        {
            get;
        }

        /// <summary>Creates a <see cref="ICurveDataFitting"/> object that represents the implementation of the interpolation approach.
        /// </summary>
        /// <returns>A <see cref="ICurveDataFitting"/> object that represents the implementation of the interpolation approach.</returns>
        ICurveDataFitting Create();

        /// <summary>Gets the left localness level for a specific grid point, i.e.
        /// changing grid point (t_j,x_j) implies changes on the interval ]t_{j-leftLocalnessLevel}, t_{j+rightLocalnessLevel}[.
        /// </summary>
        /// <param name="gridPointIndex">The null-based index of the grid point (t_j,x_j).</param>
        /// <param name="gridPointCount">The number of grid points.</param>
        /// <returns>The left localness level with respect to the grid point (t_j,x_j), where j is represented by <paramref name="gridPointIndex"/>
        /// i.e. changing grid point (t_j,x_j) implies changes on the interval ]t_{j-leftLocalnessLevel}, t_{j+rightLocalnessLevel}[.</returns>
        int GetLeftLocalnessLevel(int gridPointIndex, int gridPointCount);

        /// <summary>Gets the right localness level for a specific grid point, i.e.
        /// changing grid point (t_j,x_j) implies changes on the interval ]t_{j-leftLocalnessLevel}, t_{j+rightLocalnessLevel}[.
        /// </summary>
        /// <param name="gridPointIndex">The null-based index of the grid point (t_j,x_j).</param>
        /// <param name="gridPointCount">The number of grid points.</param>
        /// <returns>The right localness level with respect to the grid point (t_j,x_j), where j is represented by <paramref name="gridPointIndex"/>
        /// i.e. changing grid point (t_j,x_j) implies changes on the interval ]t_{j-leftLocalnessLevel}, t_{j+rightLocalnessLevel}[.</returns>
        int GetRightLocalnessLevel(int gridPointIndex, int gridPointCount);
    }
}