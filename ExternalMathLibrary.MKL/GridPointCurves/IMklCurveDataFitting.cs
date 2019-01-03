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
using Dodoni.MathLibrary.GridPointCurves;

namespace Dodoni.MathLibrary.Native.GridPointCurves
{
    /// <summary>Serves as interface for interpolations/parametrizations of a set of grid points, i.e. a curve data fitting, with respect to Intel's MKL Library.
    /// </summary>
    public interface IMklCurveDataFitting : ICurveDataFitting
    {
        /// <summary>Updates the current curve fitting object.
        /// </summary>
        /// <param name="gridPointCount">The number of grid points, i.e. the number of relevant elements of <paramref name="gridPointArguments"/> and <paramref name="gridPointValues"/> to take into account.</param>
        /// <param name="gridPointArguments">The arguments of the grid points, thus labels of the curve in its <see cref="System.Double"/> representation in ascending order.</param>
        /// <param name="gridPointValues">The values of the grid points corresponding to <paramref name="gridPointArguments"/>.</param>
        /// <param name="gridPointArgumentHint">Describes the structure of the grid point arguments.</param>
        /// <param name="gridPointValueHint">Describes the structure of the grid point values.</param>
        /// <param name="state">The state of the grid points, i.e. <paramref name="gridPointArguments"/> and <paramref name="gridPointValues"/>, with respect to the previous function call.</param>
        /// <param name="gridPointArgumentStartIndex">The null-based start index of <paramref name="gridPointArguments"/> to take into account.</param>
        /// <param name="gridPointValueStartIndex">The null-based start index of <paramref name="gridPointValues"/> to take into account.</param>
        /// <param name="gridPointArgumentIncrement">The increment for <paramref name="gridPointArguments"/>.</param>
        /// <param name="gridPointValueIncrement">The increment for <paramref name="gridPointValues"/>.</param>
        /// <remarks>This method should be called if grid points have been changed, added, removed etc. and before evaluating the grid point curve at a specified point.
        /// <para>If no problem occurred, the flag <see cref="IOperable.IsOperable"/> will be set to <c>true</c>.</para>
        /// <para>This method should always store all required data for later use, i.e. creates deep copies of the arguments.</para>
        /// </remarks>
        void Update(int gridPointCount, IList<double> gridPointArguments, IList<double> gridPointValues, MklGridPointCurve.xHintValue gridPointArgumentHint, MklGridPointCurve.yHintValue gridPointValueHint, GridPointCurve.State state = GridPointCurve.State.GridPointChanged, int gridPointArgumentStartIndex = 0, int gridPointValueStartIndex = 0, int gridPointArgumentIncrement = 1, int gridPointValueIncrement = 1);

        /// <summary>Runs data fitting computations.
        /// </summary>
        /// <param name="n">The Number of interpolation sites.</param>
        /// <param name="site">Array of interpolation sites of size <paramref name="n" />. The structure of the array is defined by the sitehint parameter:
        /// <list type=" ´bullet">
        /// <item><description>If sites form a non-uniform partition, the array should contain nsite values.</description></item>
        /// <item><description>If sites form a uniform partition, the array should contain two entries that represent the left and the right interpolation sites. The first entry
        /// of the array contains the left-most interpolation point. The second entry of the array contains the right-most interpolation point.</description></item>
        /// </list></param>
        /// <param name="result">Array that contains results of computations at the interpolation sites (output). If you do not need spline-based interpolation or integration, set this pointer to NULL.</param>
        /// <param name="ndorder">Maximal derivative order increased by one to be computed at interpolation sites.</param>
        /// <param name="dorder">Array of size <paramref name="ndorder" /> that defines the order of the derivatives to be computed at the interpolation sites. If all the elements in are zero, the library computes the spline values only. If you do not need interpolation computations, set <paramref name="ndorder" /> to zero and pass a <c>null</c> pointer.</param>
        /// <param name="siteHint">A flag describing the structure of the interpolation sites.</param>
        /// <param name="resultHint">A flag describing the structure of the results.</param>
        /// <param name="estimationType">Type of spline-based computations.</param>
        /// <param name="cell">Array of cell indices in partition x that contain the interpolation sites. If you do not need cell indices, set this parameter to <c>null</c>.</param>
        /// <param name="dataHint">Array that contains additional information about the structure of partition x and interpolation sites. This data helps to speed up the computation. If you provide a <c>null</c> pointer, the routine uses the default settings for computations. For details on the datahint array, see table "Structure of the datahint Array" in Intel Math Kernel Library Reference Manual.</param>
        /// <remarks>The argument must be an element of the domain of definition, represented by <see cref="IRealValuedCurve.LowerBound"/> and <see cref="IRealValuedCurve.UpperBound"/>.</remarks>
        void GetValues(int n, double[] site, double[] result, int ndorder, int[] dorder, MklGridPointCurve.SiteHint siteHint = MklGridPointCurve.SiteHint.DF_NO_HINT, MklGridPointCurve.ResultHint resultHint = MklGridPointCurve.ResultHint.DF_NO_HINT, MklGridPointCurve.EstimationType estimationType = MklGridPointCurve.EstimationType.DF_INTERP, int[] cell = null, double[] dataHint = null);

        /// <summary>Gets the value of the integral \int_a^b f(x) dx.
        /// </summary>
        /// <param name="n">Number of pairs of integraion limits.</param>
        /// <param name="lowerBounds">The lower bounds.</param>
        /// <param name="upperBounds">The upper bounds.</param>
        /// <param name="result">Array of integration results (output). The size of the array should be sufficient to hold <paramref name="n"/>* ny values, where ny is the dimension of the vector-valued function. The integration results are packed according to the settings in <paramref name="resultHint"/>.</param>
        /// <param name="lowerBoundHint">A flag describing the structure of the left-side integration limits.</param>
        /// <param name="upperBoundHint">A flag describing the structure of the right-side integration limits.</param>
        /// <param name="resultHint">A flag describing the structure of the results.</param>
        /// <param name="ldataHint">Array that contains additional information about the structure of partition x and left-side integration limits. For details see table "Structure of the datahint Array" in the description of the df?Intepolate1D function in Intel Math Kernel Library Reference Manual.</param>
        /// <param name="rdataHint">Array that contains additional information about the structure of partition x and right-side integration limits. For details see table "Structure of the datahint Array" in the description of the df?Intepolate1D function in Intel Math Kernel Library Reference Manual.</param>
        /// <remarks>The arguments must be elements of the domain of definition, represented by <see cref="IRealValuedCurve.LowerBound"/> and <see cref="IRealValuedCurve.UpperBound"/>.</remarks>
        void GetIntegral(int n, double[] lowerBounds, double[] upperBounds, double[] result, MklGridPointCurve.IntegrationLimitHint lowerBoundHint = MklGridPointCurve.IntegrationLimitHint.DF_NO_HINT, MklGridPointCurve.IntegrationLimitHint upperBoundHint = MklGridPointCurve.IntegrationLimitHint.DF_NO_HINT, MklGridPointCurve.IntegralResultHint resultHint = MklGridPointCurve.IntegralResultHint.DF_NO_HINT, double[] ldataHint = null, double[] rdataHint = null);
    }
}