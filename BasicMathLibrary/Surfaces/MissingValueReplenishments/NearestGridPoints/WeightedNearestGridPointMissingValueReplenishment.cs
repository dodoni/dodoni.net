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

using Dodoni.MathLibrary.GridPointCurves;

namespace Dodoni.MathLibrary.Surfaces.MissingValueReplenishments
{
    /// <summary>Serves as factory for a missing value replenishment, where for each missing value a interpolation along x-axis and y-axis of the 
    /// nearest (valid) grid points takes place. Missing values are specified as a convex combination of both interpolations.
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public partial class WeightedNearestGridPointMissingValueReplenishment
    {
        #region public (readonly) members

        /// <summary>Apply a interpolation along the x-Axis between nearest (valid) grid points to replenish missing values.
        /// </summary>
        public readonly AlongXAxis xAxis;

        /// <summary>Apply a interpolation along the y-Axis between nearest (valid) grid points to replenish missing values.
        /// </summary>
        public readonly AlongYAxis yAxis;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="WeightedNearestGridPointMissingValueReplenishment"/> class.
        /// </summary>
        internal WeightedNearestGridPointMissingValueReplenishment()
        {
            xAxis = new AlongXAxis();
            yAxis = new AlongYAxis();
        }
        #endregion

        #region public methods

        /// <summary>Creates a new <see cref="LabelMatrix.MissingValueReplenishment"/> object.
        /// </summary>
        /// <param name="horizontalInterpolator">The (curve) interpolator along x-axis.</param>
        /// <param name="verticalInterpolator">The (curve) interpolator along y-axis.</param>
        /// <param name="weight">The weight for the convex combination of the interpolated values along x-axis and y-axis; (1.0 - <paramref name="weight"/>) * estimatedValueInXDirection + <paramref name="weight"/> * estimatedValueInYDirection.</param>
        /// <returns>A <see cref="LabelMatrix.MissingValueReplenishment"/> object that replenish missing values via 
        /// <para>(1.0 - <paramref name="weight"/>) * estimatedValueInXDirection + <paramref name="weight"/> * estimatedValueInYDirection,</para>
        /// where <c>estimatedValueInXDirection</c> is the result of a interpolation along x-axis w.r.t. nearest (valid) grid points and 
        /// <c>estimatedValueInYDirection</c> is the result of a interpolation along y-axis w.r.t. nearest (valid) grid points.</returns>
        public LabelMatrix.MissingValueReplenishment Create(GridPointCurve.Interpolator horizontalInterpolator, GridPointCurve.Interpolator verticalInterpolator, double weight = 0.5)
        {
            return new NearestWeightedReplenishment(horizontalInterpolator, verticalInterpolator, weight);
        }
        #endregion

        #region internal static methods

        /// <summary>Gets the estimation of a specified missing value via a interpolation along the x-axis.
        /// </summary>
        /// <param name="rowIndex">The null-based index of the row that describes the position of the missing value in <paramref name="matrix"/>.</param>
        /// <param name="columnIndex">The null-based index of the column that describes the position of the missing value in <paramref name="matrix"/>.</param>
        /// <param name="matrix">The matrix.</param>
        /// <param name="rowCount">The number of rows.</param>
        /// <param name="columnCount">The number of columns.</param>
        /// <param name="xAxisLabeling">The labels of the x-axis in its <see cref="System.Double"/> representation, i.e. at least <paramref name="columnCount"/> elements.</param>
        /// <param name="curveInterpolator">The (curve) interpolator along x-axis.</param>
        /// <returns>The estimated value for position (<paramref name="rowIndex"/>, <paramref name="columnIndex"/>).</returns>
        /// <exception cref="InvalidOperationException">Thrown, if in the interpolation failed.</exception>
        internal static double GetInterpolatedValueAlongXAxis(int rowIndex, int columnIndex, IList<double> matrix, int rowCount, int columnCount, IList<double> xAxisLabeling, ICurveDataFitting curveInterpolator)
        {
            int leftIndex = columnIndex;
            bool foundLeftValue = false;

            while ((leftIndex >= 1) && (foundLeftValue == false))
            {
                leftIndex--;

                if (Double.IsNaN(matrix[rowIndex + leftIndex * rowCount]) == false)  // matrix[rowIndex][leftIndex]
                {
                    foundLeftValue = true;
                }
            }

            int rightIndex = columnIndex;
            bool foundRightValue = false;
            while ((rightIndex <= columnCount - 2) && (foundRightValue == false))
            {
                rightIndex++;
                if (Double.IsNaN(matrix[rowIndex + rightIndex * rowCount]) == false)  // matrix[rowIndex][rightIndex]
                {
                    foundRightValue = true;
                }
            }

            if (foundLeftValue && foundRightValue)
            {
                curveInterpolator.Update(2, xAxisLabeling, matrix, GridPointCurve.State.GridPointChanged, leftIndex, rowIndex + leftIndex * rowCount, rightIndex - leftIndex, rowCount * (rightIndex - leftIndex));
                return curveInterpolator.GetValue(xAxisLabeling[columnIndex]);
            }
            else if (foundLeftValue)
            {
                return matrix[rowIndex + leftIndex * rowCount];  // matrix[rowIndex][leftIndex]
            }
            else if (foundRightValue)
            {
                return matrix[rowIndex + rightIndex * rowCount];  // matrix[rowIndex][rightIndex]
            }
            throw new InvalidOperationException(String.Format("Replenish failed at position ({0};{1}).", rowIndex, columnIndex));
        }

        /// <summary>Gets the estimation of a specified missing value via a interpolation along the y-axis.
        /// </summary>
        /// <param name="rowIndex">The null-based index of the row that describes the position of the missing value in <paramref name="matrix"/>.</param>
        /// <param name="columnIndex">The null-based index of the column that describes the position of the missing value in <paramref name="matrix"/>.</param>
        /// <param name="matrix">The matrix.</param>
        /// <param name="rowCount">The number of rows.</param>
        /// <param name="yAxisLabeling">The labels of the y-axis in its <see cref="System.Double"/> representation, i.e. at least <paramref name="rowCount"/> elements.</param>
        /// <param name="curveInterpolator">The (curve) interpolator along y-axis.</param>
        /// <returns>The estimated value for position (<paramref name="rowIndex"/>, <paramref name="columnIndex"/>).</returns>
        /// <exception cref="InvalidOperationException">Thrown, if in the interpolation failed.</exception>
        internal static double GetInterpolatedValueAlongYAxis(int rowIndex, int columnIndex, IList<double> matrix, int rowCount, IList<double> yAxisLabeling, ICurveDataFitting curveInterpolator)
        {
            int lowIndex = rowIndex;
            bool foundLowValue = false;

            int arrayOffset = columnIndex * rowCount;

            while ((lowIndex >= 1) && (foundLowValue == false))
            {
                lowIndex--;
                if (Double.IsNaN(matrix[lowIndex + arrayOffset]) == false)  // matrix[lowIndex][columnIndex]
                {
                    foundLowValue = true;
                }
            }

            int higherIndex = rowIndex;
            bool foundHigherValue = false;
            while ((higherIndex <= rowCount - 2) && (foundHigherValue == false))
            {
                higherIndex++;
                if (Double.IsNaN(matrix[higherIndex + arrayOffset]) == false)  // matrix[higherIndex][columnIndex]
                {
                    foundHigherValue = true;
                }
            }

            if (foundLowValue && foundHigherValue)
            {
                curveInterpolator.Update(2, yAxisLabeling, matrix, GridPointCurve.State.GridPointChanged, lowIndex, lowIndex + arrayOffset, higherIndex - lowIndex, higherIndex - lowIndex);
                return curveInterpolator.GetValue(yAxisLabeling[rowIndex]);
            }
            else if (foundLowValue)
            {
                return matrix[lowIndex + arrayOffset];   // matrix[lowIndex][columnIndex]
            }
            else if (foundHigherValue)
            {
                return matrix[higherIndex + arrayOffset];  // matrix[higherIndex][columnIndex]
            }
            throw new InvalidOperationException(String.Format("Replenish failed at position ({0};{1}).", rowIndex, columnIndex));
        }
        #endregion
    }
}