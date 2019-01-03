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

using Dodoni.MathLibrary.GridPointCurves;

namespace Dodoni.MathLibrary.Surfaces
{
    internal abstract partial class HorizontalVerticalWiseSurface2d<THorizontalLabel, TVerticalLabel> : IGridPointSurface2d<THorizontalLabel, TVerticalLabel>
        where THorizontalLabel : IComparable<THorizontalLabel>, IEquatable<THorizontalLabel>
        where TVerticalLabel : IComparable<TVerticalLabel>
    {
        /// <summary>Represents a two-dimensional grid point surface, where a interpolation (and extrapolation) takes place in vertical direction.
        /// </summary>
        internal class VInterpolation : HorizontalVerticalWiseSurface2d<THorizontalLabel, TVerticalLabel>
        {
            #region private members

            /// <summary>The interpolator factory along vertical direction.
            /// </summary>
            private GridPointCurve.Interpolator m_VerticalInterpolatorFactory;

            /// <summary>The interpolation approach along vertical direction.
            /// </summary>
            private ICurveDataFitting m_VerticalInterpolator;

            /// <summary>The extrapolation factory in vertical direction above the grid points.
            /// </summary>
            private GridPointCurve.Extrapolator m_VerticalAboveExtrapolatorFactory;

            /// <summary>The extrapolation approach in vertical direction above the grid points.            
            /// </summary>
            private ICurveExtrapolator m_VerticalAboveExtrapolator;

            /// <summary>The extrapolation factory in vertical direction below the grid points.
            /// </summary>
            private GridPointCurve.Extrapolator m_VerticalBelowExtrapolatorFactory;

            /// <summary>The extrapolation approach in vertical direction below the grid points.
            /// </summary>
            private ICurveExtrapolator m_VerticalBelowExtrapolator;

            /// <summary>A temporary array that contains the values for a interpolation etc. along vertical direction, i.e. at most <c>m_VerticalDoubleLabels.Length</c> elements.
            /// </summary>
            /// <remarks>This member is used for performance reason.</remarks>
            private double[] m_TempValuesForVerticalEvaluation;

            /// <summary>A temporary array that contains the <see cref="System.Double"/> representation of some vertical labels.
            /// </summary>
            /// <remarks>This member is used for performance reason.</remarks>
            private double[] m_TempVerticalDoubleLabels;
            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="VInterpolation"/> class.
            /// </summary>
            /// <param name="gridPointMatrix">The grid point matrix.</param>
            /// <param name="horizontalCurveFactory">A factory for grid point curves along horizontal direction, i.e. taken into account a specified interpolation, parametrization etc.</param>
            /// <param name="verticalInterpolator">The interpolation approach along vertical direction.</param>
            /// <param name="verticalAboveExtrapolator">The extrapolation approach in vertical direction above the grid points.</param>
            /// <param name="verticalBelowExtrapolator">The extrapolation approach in vertical direction below the grid points.</param>
            internal VInterpolation(LabelMatrix<THorizontalLabel, TVerticalLabel> gridPointMatrix, Func<TVerticalLabel, IGridPointCurveFactory<THorizontalLabel>> horizontalCurveFactory, GridPointCurve.Interpolator verticalInterpolator, GridPointCurve.Extrapolator verticalAboveExtrapolator, GridPointCurve.Extrapolator verticalBelowExtrapolator)
                : base(gridPointMatrix, horizontalCurveFactory)
            {
                m_VerticalInterpolatorFactory = verticalInterpolator ?? throw new NullReferenceException(nameof(verticalInterpolator));
                m_VerticalInterpolator = verticalInterpolator.Create();

                if (verticalAboveExtrapolator == null)
                {
                    throw new NullReferenceException(nameof(verticalAboveExtrapolator));
                }
                if (verticalAboveExtrapolator.ExtrapolationBuildingDirection != GridPointCurve.Extrapolator.BuildingDirection.FromFirstGridPoint)
                {
                    throw new ArgumentException(String.Format("Invalid building direction of extrapolation above grid points."), nameof(verticalAboveExtrapolator));
                }
                m_VerticalAboveExtrapolatorFactory = verticalAboveExtrapolator;
                m_VerticalAboveExtrapolator = verticalAboveExtrapolator.Create(m_VerticalInterpolator);

                if (verticalBelowExtrapolator == null)
                {
                    throw new NullReferenceException(nameof(verticalBelowExtrapolator));
                }
                if (verticalBelowExtrapolator.ExtrapolationBuildingDirection != GridPointCurve.Extrapolator.BuildingDirection.FromLastGridPoint)
                {
                    throw new ArgumentException(String.Format("Invalid building direction of extrapolation below grid points."), nameof(verticalBelowExtrapolator));
                }
                m_VerticalBelowExtrapolatorFactory = verticalBelowExtrapolator;
                m_VerticalBelowExtrapolator = verticalBelowExtrapolator.Create(m_VerticalInterpolator);

                m_TempValuesForVerticalEvaluation = new double[m_VerticalDoubleLabels.Length];
                m_TempVerticalDoubleLabels = new double[m_GridPointMatrix.RowCount];
            }
            #endregion

            #region public methods

            /// <summary>Gets a value of the surface at a specified (x,y) coordinate.
            /// </summary>
            /// <param name="x">The x coordinate of the point.</param>
            /// <param name="y">The y coordinate of the point.</param>
            /// <returns>The value of the surface at (<paramref name="x"/>, <paramref name="y"/>).</returns>
            public override double GetValue(double x, double y)
            {
                int rowCount = m_VerticalDoubleLabels.Length;

                int firstRowIndex = 0;
                int lastRowIndex = rowCount - 1;
                int relevantRowCount = rowCount;

                /* if the interpolation along vertical direction is a local approach a few function evaluations are needed only: */
                if (m_VerticalInterpolatorFactory.IsLocalApproach == true)
                {
                    if (y < m_VerticalDoubleLabels[0])  // above the grid points
                    {
                        lastRowIndex = Math.Min(rowCount - 1, m_VerticalAboveExtrapolatorFactory.GetLevelOfGridPointDependency(rowCount));
                    }
                    else if (y > m_VerticalDoubleLabels[rowCount - 1])  // below the grid points
                    {
                        firstRowIndex = Math.Max(0, rowCount - m_VerticalBelowExtrapolatorFactory.GetLevelOfGridPointDependency(rowCount) - 1);
                    }
                    else // inside the grid points
                    {
                        int nonLastLeftGridPointIndex = GridPointCurve.Utilities.GetNonLastNearestIndex(y, m_VerticalDoubleLabels, m_VerticalDoubleLabels.Length);

                        int upperLocalnessLevel = m_VerticalInterpolatorFactory.GetLeftLocalnessLevel(nonLastLeftGridPointIndex, rowCount);
                        int lowerLocalnessLevel = m_VerticalInterpolatorFactory.GetRightLocalnessLevel(nonLastLeftGridPointIndex, rowCount);

                        firstRowIndex = Math.Max(nonLastLeftGridPointIndex - upperLocalnessLevel, 0);
                        lastRowIndex = Math.Min(nonLastLeftGridPointIndex + lowerLocalnessLevel, rowCount - 1);
                    }
                    relevantRowCount = lastRowIndex - firstRowIndex + 1;

                    /* perhaps one may take into account less rows than required for the interpolation, i.e. one has to adjust the first/last row index: */
                    if (relevantRowCount < m_VerticalInterpolatorFactory.MinimalRequiredNumberOfGridPoints)
                    {
                        int numberOfAdditionalRequiredRows = m_VerticalInterpolatorFactory.MinimalRequiredNumberOfGridPoints - relevantRowCount;

                        firstRowIndex = Math.Max(0, firstRowIndex - numberOfAdditionalRequiredRows);
                        lastRowIndex = Math.Min(rowCount - 1, lastRowIndex + numberOfAdditionalRequiredRows);

                        relevantRowCount = lastRowIndex - firstRowIndex + 1;
                    }
                }

                /* compute values w.r.t. to the x-coordinate and each relevant row, i.e. store values to take into account for vertical 
                 * interpolation. In the case of a non-local approach, one may improve performance by indicating that the labels will not be changed: */
                int k = 0;
                for (int j = firstRowIndex; j <= lastRowIndex; j++)
                {
                    m_TempValuesForVerticalEvaluation[k++] = m_CurvesAlongHorizontalDirection[j].GetValue(x);
                }

                GridPointCurve.State verticalCurveState = GridPointCurve.State.GridPointChanged;
                if ((m_VerticalInterpolatorFactory.IsLocalApproach == false) && (m_VerticalInterpolator.IsOperable == true) && (m_VerticalInterpolator.GridPointCount == rowCount))
                {
                    verticalCurveState = GridPointCurve.State.GridPointValueChanged;
                }
                m_VerticalInterpolator.Update(relevantRowCount, m_VerticalDoubleLabels, m_TempValuesForVerticalEvaluation, verticalCurveState, gridPointArgumentStartIndex: firstRowIndex);

                if (y < m_VerticalInterpolator.LowerBound)
                {
                    m_VerticalAboveExtrapolator.Update();
                    return m_VerticalAboveExtrapolator.GetValue(y);
                }
                else if (y > m_VerticalInterpolator.UpperBound)
                {
                    m_VerticalBelowExtrapolator.Update();
                    return m_VerticalBelowExtrapolator.GetValue(y);
                }
                return m_VerticalInterpolator.GetValue(y);
            }

            /// <summary>Gets a value at a specified point (<paramref name="columnIndex"/>, <paramref name="y"/>).
            /// </summary>
            /// <param name="columnIndex">The null-based column index of the point.</param>
            /// <param name="y">The y coordinate of the point.</param>
            /// <returns>The value of the surface at (<paramref name="columnIndex"/>, <paramref name="y"/>).</returns>
            public override double GetValue(int columnIndex, double y)
            {
                if (m_HorizontalExactFitToGridPoints == false)
                {
                    if (m_VerticalLabelMapping == null)
                    {
                        return GetValue(m_GridPointMatrix.HorizontalDoubleLabels[columnIndex], y);
                    }
                    else
                    {
                        return GetValue(m_GridPointMatrix.HorizontalDoubleLabels[m_VerticalLabelMapping[columnIndex]], y);
                    }
                }

                /* if the horizontal curve fitting meets the grid points, an evaluation of the horizontal curve fitting is not necessary. Here, we do not distinguish 
                 * between local and non-local interpolation approach and we take into account all values (!= NaN) along vertical direction: */

                int numberOfRelevantRows = 0;

                for (int j = 0; j < m_GridPointMatrix.RowCount; j++)
                {
                    double gridValue = m_GridPointMatrix[j, columnIndex];
                    if (Double.IsNaN(gridValue) == false)
                    {
                        m_TempValuesForVerticalEvaluation[numberOfRelevantRows] = gridValue;
                        m_TempVerticalDoubleLabels[numberOfRelevantRows] = m_GridPointMatrix.VerticalDoubleLabels[j];
                        numberOfRelevantRows++;
                    }
                }
                GridPointCurve.State verticalCurveState = GridPointCurve.State.GridPointChanged;
                if ((m_VerticalInterpolator.IsOperable == true) && (m_VerticalInterpolator.GridPointCount == m_GridPointMatrix.RowCount) && (numberOfRelevantRows == m_GridPointMatrix.RowCount))
                {
                    verticalCurveState = GridPointCurve.State.GridPointValueChanged;
                }
                m_VerticalInterpolator.Update(numberOfRelevantRows, m_TempVerticalDoubleLabels, m_TempValuesForVerticalEvaluation, verticalCurveState);

                if (y < m_VerticalInterpolator.LowerBound)
                {
                    m_VerticalAboveExtrapolator.Update();
                    return m_VerticalAboveExtrapolator.GetValue(y);
                }
                else if (y > m_VerticalInterpolator.UpperBound)
                {
                    m_VerticalBelowExtrapolator.Update();
                    return m_VerticalBelowExtrapolator.GetValue(y);
                }
                return m_VerticalInterpolator.GetValue(y);
            }
            #endregion
        }
    }
}