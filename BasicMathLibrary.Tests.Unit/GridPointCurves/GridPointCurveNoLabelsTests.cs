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

using NUnit.Framework;

namespace Dodoni.MathLibrary.GridPointCurves
{
    /// <summary>Provides unit tests for <see cref="IGridPointCurve"/> objects (without labels).
    /// </summary>
    [TestFixture]
    public class GridPointCurveNoLabelsTests
    {
        /// <summary>A test function for the GetValue method of a grid point curve where the interpolation/extrapolation fits the grid point values exact.
        /// </summary>
        /// <param name="gridPointCurve">A grid point curve, where the interpolation/extrapolation fits the grid point values exact.</param>
        [TestCaseSource(nameof(ExactFittingGridPointCurves))]
        public void GetValue_GridPointArgument_GridPointValue(IGridPointCurve gridPointCurve)
        {
            Assume.That(gridPointCurve.FittingQuality == FittingQuality.Exact);

            for (int j = 0; j < gridPointCurve.GridPointCount; j++)
            {
                double expected = gridPointCurve.GridPointValues[j];
                double actual = gridPointCurve.GetValue(gridPointCurve.GridPointArguments[j]);

                Assert.That(actual, Is.EqualTo(expected).Within(1E-6), String.Format("Grid Point argument: {0}; Grid Point value: {1}; null-based Grid Point index {2}.", gridPointCurve.GridPointArguments[j], gridPointCurve.GridPointValues[j], j));
            }
        }

        /// <summary>Gets a collection of <see cref="IGridPointCurve"/> test objects, where the grid points are exactly fitted by the interpolation/parametrization.
        /// </summary>
        /// <value>The exact fitting grid point curves.</value>
        public static IEnumerable<TestCaseData> ExactFittingGridPointCurves
        {
            get
            {
                foreach (var interpolation in new[]{ GridPointCurve.Interpolator.Linear,
                                                     GridPointCurve.Interpolator.LogLinear,
                                                     GridPointCurve.Interpolator.None,
                                                     GridPointCurve.Interpolator.PiecewiseConstant,
                                                     GridPointCurve.Interpolator.Splines.BesselCubicSpline,
                                                     GridPointCurve.Interpolator.Splines.NaturalCubicSpline,
                                                     GridPointCurve.Interpolator.Splines.Create(1.0, -0.75)})
                {
                    foreach (var leftExtrapolation in new[] {GridPointCurve.Extrapolator.Constant.First,
                                                             GridPointCurve.Extrapolator.Constant.Create(GridPointCurve.Extrapolator.BuildingDirection.FromFirstGridPoint, 0.5),
                                                             GridPointCurve.Extrapolator.Linear.SlopeOfFirstTwoGridPoints,
                                                             GridPointCurve.Extrapolator.None.First})
                    {
                        foreach (var rightExtrapolation in new[] {GridPointCurve.Extrapolator.Constant.Last,
                                                                  GridPointCurve.Extrapolator.Constant.Create(GridPointCurve.Extrapolator.BuildingDirection.FromLastGridPoint, 1.5),
                                                                  GridPointCurve.Extrapolator.Linear.SlopeOfLastTwoGridPoints,
                                                                  GridPointCurve.Extrapolator.None.Last})
                        {
                            var gridPointCurve = GridPointCurve.Create(interpolation, leftExtrapolation, rightExtrapolation);

                            gridPointCurve.Add(1.5, 2.0);
                            gridPointCurve.Add(0.5, 1.25);
                            gridPointCurve.Add(5.75, 9.75);
                            gridPointCurve.Add(4.0, 12.25);
                            gridPointCurve.Add(8.0, 7.5);

                            gridPointCurve.Update();
                            yield return new TestCaseData(gridPointCurve);
                        }
                    }
                }
            }
        }
    }
}