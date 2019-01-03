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
    /// <summary>Serves as interface for the boundary condition of cubic splines.
    /// </summary>
    /// <remarks>The general calculation of the cubic spline coefficient can be reduced to the solution of
    /// <para>
    ///     A * y = b,
    /// </para>
    /// where y=(y_0,...y_n) are the second derivatives and A is some tri-diagonal matrix. The boundary
    /// constraints are specified by the first and last element of b as well as the elements a_{1,1}, a_{1,2}
    /// and a_{n,n-1}, a_{n,n} of A=(a_{i,j}).
    /// </remarks>
    public interface ICubicSplineBoundaryCondition
    {
        /// <summary>Updates the current boundary condition.
        /// </summary>
        /// <param name="gridPointCount">The number of grid points, i.e. the number of relevant elements of <paramref name="gridPointArguments"/> and <paramref name="gridPointValues"/> to take into account.</param>
        /// <param name="gridPointArguments">The arguments of the grid points, thus labels of the curve in its <see cref="System.Double"/> representation.</param>
        /// <param name="gridPointValues">The values of the grid points corresponding to <paramref name="gridPointArguments"/>.</param>
        /// <param name="state">The state of the grid points, i.e. <paramref name="gridPointArguments"/> and <paramref name="gridPointValues"/>, with respect to the previous function call.</param>
        /// <param name="gridPointArgumentStartIndex">The null-based start index of <paramref name="gridPointArguments"/> to take into account.</param>
        /// <param name="gridPointValueStartIndex">The null-based start index of <paramref name="gridPointValues"/> to take into account.</param>
        /// <param name="gridPointArgumentIncrement">The increment for <paramref name="gridPointArguments" />.</param>
        /// <param name="gridPointValueIncrement">The increment for <paramref name="gridPointValues" />.</param>
        /// <remarks>This method should be called if grid points have been changed, added, removed etc. and before evaluating the grid point curve at a specified point.
        /// </remarks>
        void Update(int gridPointCount, IList<double> gridPointArguments, IList<double> gridPointValues, GridPointCurve.State state, int gridPointArgumentStartIndex = 0, int gridPointValueStartIndex = 0, int gridPointArgumentIncrement = 1, int gridPointValueIncrement = 1);

        /// <summary>Gets the remaining matrix elements of A = (a_{i,j}), where A*y=b represents
        /// the cubic spline coefficent equation.
        /// </summary>
        /// <param name="firstDiagonalElement">The first diagonal element, i.e. a_{1,1} (output).</param>
        /// <param name="firstSuperDiagonalElement">The first super diagonal element, i.e. a_{1,2} (output).</param>
        /// <param name="lastSubDiagonalElement">The last sub diagonal element, i.e. a_{n,n-1} (output).</param>
        /// <param name="lastDiagonalElement">The last diagonal element, i.e. a_{n,n} (output).</param>
        /// <remarks>Here, we assume that the remaining matrix elements do not depend on the grid points (output).</remarks>
        void GetRemainingMatrixElements(out double firstDiagonalElement, out double firstSuperDiagonalElement, out double lastSubDiagonalElement, out double lastDiagonalElement);

        /// <summary>Gets the remaining right hand side elements, i.e. the first and last element of 'b',
        /// where A*y=b represents the cubic spline coefficent equation.
        /// </summary>
        /// <param name="firstElement">The first element of b (output).</param>
        /// <param name="lastElement">The last element of b (output).</param>
        void GetRemainingRightHandSideElements(out double firstElement, out double lastElement);
    }
}