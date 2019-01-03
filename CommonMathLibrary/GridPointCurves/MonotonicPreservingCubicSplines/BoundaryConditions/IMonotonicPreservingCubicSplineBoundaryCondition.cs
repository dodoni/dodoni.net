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
using System.Collections.Generic;

namespace Dodoni.MathLibrary.GridPointCurves
{
    /// <summary>Serves as interface for the boundary conditions of <see cref="CurveInterpolationMonotonicPreservingCubicSpline"/> instances.
    /// </summary>
    /// <remarks>Here, we use the same notation as in 
    /// <para>
    ///   Patrick S. Hagan, Graeme West, Interpolation methods for curve construction, Applied Mathematical Finance, Vol. 13, No. 2, p.89-129, June 2006.
    /// </para>
    /// More precisely here, we determind the boundary coefficient 'b_1', 'b_n', see equation (25).
    /// </remarks>
    public interface IMonotonicPreservingCubicSplineBoundaryCondition
    {
        /// <summary>Updates the current boundary condition.
        /// </summary>
        /// <param name="gridPointCount">The number of grid points, i.e. the number of relevant elements of <paramref name="gridPointArguments"/> and <paramref name="gridPointValues"/> to take into account.</param>
        /// <param name="gridPointArguments">The arguments of the grid points, thus labels of the curve in its <see cref="System.Double"/> representation.</param>
        /// <param name="gridPointValues">The values of the grid points corresponding to <paramref name="gridPointArguments"/>.</param>
        /// <param name="state">The state of the grid points, i.e. <paramref name="gridPointArguments"/> and <paramref name="gridPointValues"/>, with respect to the previous function call.</param>
        /// <param name="gridPointArgumentsStartIndex">The null-based start index of <paramref name="gridPointArguments"/> to take into account.</param>
        /// <param name="gridPointValuesStartIndex">The null-based start index of <paramref name="gridPointValues"/> to take into account.</param>
        /// <param name="gridPointArgumentIncrement">The increment for <paramref name="gridPointArguments"/>.</param>
        /// <param name="gridPointValueIncrement">The increment for <paramref name="gridPointValues"/>.</param>
        /// <remarks>This method should be called if grid points have been changed, added, removed etc. and before evaluating the grid point curve at a specified point.
        /// </remarks>
        void Update(int gridPointCount, IList<double> gridPointArguments, IList<double> gridPointValues, GridPointCurve.State state, int gridPointArgumentsStartIndex = 0, int gridPointValuesStartIndex = 0, int gridPointArgumentIncrement = 1, int gridPointValueIncrement = 1);

        /// <summary>Gets the first boundary coefficient, i.e. b_1.
        /// </summary>
        /// <returns>The value of 'b_1', i.e. the first boundary coefficient.</returns>
        double GetFirstBoundaryCoefficient();

        /// <summary>Gets the last boundary coefficient, i.e. b_n.
        /// </summary>
        /// <returns>The value of 'b_n', i.e. the last boundary coefficient.</returns>
        double GetLastBoundaryCoefficient();
    }
}