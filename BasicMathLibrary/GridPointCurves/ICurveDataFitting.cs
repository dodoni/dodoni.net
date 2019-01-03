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
using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.GridPointCurves
{
    /// <summary>Serves as interface for interpolations/parametrizations of a set of grid points, i.e. a curve data fitting.
    /// </summary>
    public interface ICurveDataFitting : IRealValuedCurve, IOperable, IInfoOutputQueriable
    {
        /// <summary>Gets the number of grid points.
        /// </summary>
        /// <value>The number of grid points.</value>
        int GridPointCount
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

        /// <summary>Gets the grid point values with respect to <see cref="ICurveDataFitting.GridPointArguments"/>.
        /// </summary>
        /// <value>The grid point values.</value>
        IList<double> GridPointValues
        {
            get;
        }

        /// <summary>Gets the factory of <see cref="ICurveDataFitting"/> objects of the same type and configuration.
        /// </summary>
        /// <value>The factory of <see cref="ICurveDataFitting"/> objects of the same type and configuration.</value>
        ICurveDataFittingFactory Factory
        {
            get;
        }

        /// <summary>Updates the current curve fitting object.
        /// </summary>
        /// <param name="gridPointCount">The number of grid points, i.e. the number of relevant elements of <paramref name="gridPointArguments"/> and <paramref name="gridPointValues"/> to take into account.</param>
        /// <param name="gridPointArguments">The arguments of the grid points, thus labels of the curve in its <see cref="System.Double"/> representation in ascending order.</param>
        /// <param name="gridPointValues">The values of the grid points corresponding to <paramref name="gridPointArguments"/>.</param>
        /// <param name="state">The state of the grid points, i.e. <paramref name="gridPointArguments"/> and <paramref name="gridPointValues"/>, with respect to the previous function call.</param>
        /// <param name="gridPointArgumentStartIndex">The null-based start index of <paramref name="gridPointArguments"/> to take into account.</param>
        /// <param name="gridPointValueStartIndex">The null-based start index of <paramref name="gridPointValues"/> to take into account.</param>
        /// <param name="gridPointArgumentIncrement">The increment for <paramref name="gridPointArguments"/>.</param>
        /// <param name="gridPointValueIncrement">The increment for <paramref name="gridPointValues"/>.</param>
        /// <remarks>This method should be called if grid points have been changed, added, removed etc. and before evaluating the grid point curve at a specified point.
        /// <para>If no problem occurred, the flag <see cref="IOperable.IsOperable"/> will be set to <c>true</c>.</para>
        /// <para>This method should always store all required data for later use, i.e. creates deep copies of the arguments.</para>
        /// </remarks>
        void Update(int gridPointCount, IList<double> gridPointArguments, IList<double> gridPointValues, GridPointCurve.State state = GridPointCurve.State.GridPointChanged, int gridPointArgumentStartIndex = 0, int gridPointValueStartIndex = 0, int gridPointArgumentIncrement = 1, int gridPointValueIncrement = 1);

        /// <summary>Gets the value of the integral \int_a^b f(x) dx inside two specific grid points. 
        /// </summary>
        /// <param name="lowerBound">The lower bound; between the grid point arguments specified by <paramref name="leftGridPointIndex"/> and <paramref name="leftGridPointIndex"/> + 1.</param>
        /// <param name="upperBound">The upper bound; between the grid point arguments specified by <paramref name="leftGridPointIndex"/> and <paramref name="leftGridPointIndex"/> + 1.</param>
        /// <param name="leftGridPointIndex">The null-based index of the left grid point index.</param>
        /// <returns>The value of \int_a^b f(x) dx.</returns>
        double GetIntegral(double lowerBound, double upperBound, int leftGridPointIndex);
    }
}