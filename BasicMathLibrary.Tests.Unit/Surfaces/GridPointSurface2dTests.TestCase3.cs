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

using Dodoni.MathLibrary.GridPointCurves;

namespace Dodoni.MathLibrary.Surfaces
{
    public partial class GridPointSurface2dTests
    {
        /*
         *  Example input matrix:  
         *  
         *    1    6
         *    2    7
         *    3    8
         *    4    9
         *    5   10
         */

        /// <summary>A test function for the GetValue method of a specific GridPointSurface2d object.
        /// </summary>
        /// <param name="horizontalInterpolator">The interpolation approach along horizontal direction.</param>
        /// <param name="horizontalLeftExtrapolator">The extrapolation approach in horizontal direction on the left.</param>
        /// <param name="horizontalRightExtrapolator">The extrapolation approach in horizontal direction on the right.</param>
        /// <param name="verticalInterpolator">The interpolation approach along vertical direction.</param>
        /// <param name="verticalAboveExtrapolator">The extrapolation approach in vertical direction above the grid points.</param>
        /// <param name="verticalBelowExtrapolator">The extrapolation approach in vertical direction below the grid points.</param>
        /// <param name="constructionOrder">A value indicating the order of the vertical and horizontal interpolation, extrapolation etc.</param>
        [TestCaseSource("GetValue_AtGridPoint_TestCaseConfiguration")]
        public void GetValue_AtGridPoint_GridPointValue_TestCase3(GridPointCurve.Interpolator horizontalInterpolator, GridPointCurve.Extrapolator horizontalLeftExtrapolator, GridPointCurve.Extrapolator horizontalRightExtrapolator, GridPointCurve.Interpolator verticalInterpolator, GridPointCurve.Extrapolator verticalAboveExtrapolator, GridPointCurve.Extrapolator verticalBelowExtrapolator, GridPointSurface2d.ConstructionOrder constructionOrder)
        {
            int rowCount = 5;
            int columnCount = 2;
            var xLabels = new double[] { 1.1, 2.7 };
            var yLabels = new double[] { 1.4, 2, 3.7, 4, 5 };

            var matrixEntries = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };  // matrix is provided column-by-column

            var labelMatrix = LabelMatrix.Create(rowCount, columnCount, matrixEntries, xLabels, yLabels);

            var surface = GridPointSurface2d.Create(labelMatrix, horizontalInterpolator, horizontalLeftExtrapolator, horizontalRightExtrapolator, verticalInterpolator, verticalAboveExtrapolator, verticalBelowExtrapolator, constructionOrder);

            for (int j = 0; j < rowCount; j++)
            {
                for (int k = 0; k < columnCount; k++)
                {
                    double expected = labelMatrix[j, k];

                    double actual = surface.GetValue(x: labelMatrix.HorizontalDoubleLabels[k], y: labelMatrix.VerticalDoubleLabels[j]);
                    Assert.That(actual, Is.EqualTo(expected).Within(1E-7), String.Format("Row: {0}, Column: {1}, expected: {2}, actual: {3}", j, k, expected, actual));
                }
            }
        }

        /// <summary>A test function for the GetValue method of a specific GridPointSurface2d object.
        /// </summary>
        /// <param name="horizontalInterpolator">The interpolation approach along horizontal direction.</param>
        /// <param name="horizontalLeftExtrapolator">The extrapolation approach in horizontal direction on the left.</param>
        /// <param name="horizontalRightExtrapolator">The extrapolation approach in horizontal direction on the right.</param>
        /// <param name="verticalInterpolator">The interpolation approach along vertical direction.</param>
        /// <param name="verticalAboveExtrapolator">The extrapolation approach in vertical direction above the grid points.</param>
        /// <param name="verticalBelowExtrapolator">The extrapolation approach in vertical direction below the grid points.</param>
        /// <param name="constructionOrder">A value indicating the order of the vertical and horizontal interpolation, extrapolation etc.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="expected">The expected value.</param>
        [TestCaseSource("TestCase3Data")]
        public void GetValue_TestCase3(GridPointCurve.Interpolator horizontalInterpolator, GridPointCurve.Extrapolator horizontalLeftExtrapolator, GridPointCurve.Extrapolator horizontalRightExtrapolator, GridPointCurve.Interpolator verticalInterpolator, GridPointCurve.Extrapolator verticalAboveExtrapolator, GridPointCurve.Extrapolator verticalBelowExtrapolator, GridPointSurface2d.ConstructionOrder constructionOrder, double x, double y, double expected)
        {
            int rowCount = 5;
            int columnCount = 2;
            var xLabels = new double[] { 1.1, 2.7 };
            var yLabels = new double[] { 1.4, 2, 3.7, 4, 5 };

            var matrixEntries = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };  // matrix is provided column-by-column

            var labelMatrix = LabelMatrix.Create(rowCount, columnCount, matrixEntries, xLabels, yLabels);

            var surface = GridPointSurface2d.Create(labelMatrix, horizontalInterpolator, horizontalLeftExtrapolator, horizontalRightExtrapolator, verticalInterpolator, verticalAboveExtrapolator, verticalBelowExtrapolator, constructionOrder);

            double actual = surface.GetValue(x, y);
            Assert.That(actual, Is.EqualTo(expected).Within(1E-7), String.Format("expected: {0}, actual: {1}", expected, actual));
        }


        /// <summary>Gets a collection of Interpolator etc. objects used as input for the test case object.
        /// </summary>
        /// <value>The parameter input for the test case objects.</value>
        public static IEnumerable<TestCaseData> TestCase3Data
        {
            get
            {
                //yield return new TestCaseData(
                //    GridPointCurve.Interpolator.Linear, GridPointCurve.Extrapolator.None.First, GridPointCurve.Extrapolator.None.Last,
                //    GridPointCurve.Interpolator.Linear, GridPointCurve.Extrapolator.None.First, GridPointCurve.Extrapolator.None.Last,
                //    GridPointSurface2d.ConstructionOrder.HorizontalVertical,
                //    0.5, 3.0,
                //    1.0).Throws(typeof(ArgumentException));

                yield return new TestCaseData(
                    GridPointCurve.Interpolator.Linear, GridPointCurve.Extrapolator.Constant.First, GridPointCurve.Extrapolator.Constant.Last,
                    GridPointCurve.Interpolator.Linear, GridPointCurve.Extrapolator.Constant.First, GridPointCurve.Extrapolator.Constant.Last,
                    GridPointSurface2d.ConstructionOrder.HorizontalVertical,
                    0.75, 1.4,
                    1.0); // on the left side of the first row

                yield return new TestCaseData(
                    GridPointCurve.Interpolator.Linear, GridPointCurve.Extrapolator.Constant.First, GridPointCurve.Extrapolator.Constant.Last,
                    GridPointCurve.Interpolator.Linear, GridPointCurve.Extrapolator.Constant.First, GridPointCurve.Extrapolator.Constant.Last,
                    GridPointSurface2d.ConstructionOrder.HorizontalVertical,
                    0.5, 0.05,
                    1.0);  // left upper corner

                yield return new TestCaseData(
                    GridPointCurve.Interpolator.Linear, GridPointCurve.Extrapolator.Constant.First, GridPointCurve.Extrapolator.Constant.Last,
                    GridPointCurve.Interpolator.Linear, GridPointCurve.Extrapolator.Constant.First, GridPointCurve.Extrapolator.Constant.Last,
                    GridPointSurface2d.ConstructionOrder.HorizontalVertical,
                    3.0, 0.05,
                    6.0);  // right upper corner

                yield return new TestCaseData(
                    GridPointCurve.Interpolator.Linear, GridPointCurve.Extrapolator.Constant.First, GridPointCurve.Extrapolator.Constant.Last,
                    GridPointCurve.Interpolator.Linear, GridPointCurve.Extrapolator.Constant.First, GridPointCurve.Extrapolator.Constant.Last,
                    GridPointSurface2d.ConstructionOrder.HorizontalVertical,
                    6.5, 7.0,
                    10.0);  // right lower corner

                yield return new TestCaseData(
                    GridPointCurve.Interpolator.Linear, GridPointCurve.Extrapolator.Constant.First, GridPointCurve.Extrapolator.Constant.Last,
                    GridPointCurve.Interpolator.Linear, GridPointCurve.Extrapolator.Constant.First, GridPointCurve.Extrapolator.Constant.Last,
                    GridPointSurface2d.ConstructionOrder.HorizontalVertical,
                    6.25, 2.0,
                    7.0);  // on the right of the second row

                yield return new TestCaseData(
                    GridPointCurve.Interpolator.Linear, GridPointCurve.Extrapolator.Constant.First, GridPointCurve.Extrapolator.Constant.Last,
                    GridPointCurve.Interpolator.Linear, GridPointCurve.Extrapolator.Constant.First, GridPointCurve.Extrapolator.Constant.Last,
                    GridPointSurface2d.ConstructionOrder.HorizontalVertical,
                    5.0, 1.75,
                    (7.0 - 6.0) / (2.0 - 1.4) * (1.75 - 1.4) + 6.0);  // on the right between first and second row, i.e. a linear interpolation between the (constant extrapolated) values '6' and '7' w.r.t. labels 1.4 and 2.0

                yield return new TestCaseData(
                    GridPointCurve.Interpolator.Linear, GridPointCurve.Extrapolator.Constant.First, GridPointCurve.Extrapolator.Constant.Last,
                    GridPointCurve.Interpolator.Linear, GridPointCurve.Extrapolator.Constant.First, GridPointCurve.Extrapolator.Constant.Last,
                    GridPointSurface2d.ConstructionOrder.VerticalHorizontal,
                    5.0, 1.75,
                    (7.0 - 6.0) / (2.0 - 1.4) * (1.75 - 1.4) + 6.0);  // first apply a linear interpolation between a values '6' and '7' w.r.t. labels 1.4 and 2.0 and apply a constant extrapolation
            }
        }
    }
}