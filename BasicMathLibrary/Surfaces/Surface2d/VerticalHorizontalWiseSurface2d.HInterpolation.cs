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
using System.Linq;
using System.Collections.Generic;

using Dodoni.BasicComponents;
using Dodoni.MathLibrary.GridPointCurves;

namespace Dodoni.MathLibrary.Surfaces
{
    internal abstract partial class VerticalHorizontalWiseSurface2d<THorizontalLabel, TVerticalLabel> : IGridPointSurface2d<THorizontalLabel, TVerticalLabel>
        where THorizontalLabel : IComparable<THorizontalLabel>
        where TVerticalLabel : IComparable<TVerticalLabel>, IEquatable<TVerticalLabel>
    {
        /// <summary>Represents a two-dimensional grid point surface, where a interpolation (and extrapolation) takes place in horizontal direction.
        /// </summary>
        internal class HInterpolation : VerticalHorizontalWiseSurface2d<THorizontalLabel, TVerticalLabel>
        {
            #region private members

            /// <summary>The interpolator factory along horizontal direction.
            /// </summary>
            private GridPointCurve.Interpolator m_HorizontalInterpolatorFactory;

            /// <summary>The interpolation approach along horizontal direction.
            /// </summary>
            private ICurveDataFitting m_HorizontalInterpolator;

            /// <summary>The extrapolation factory in horizontal direction on the left side of the grid points.
            /// </summary>
            private GridPointCurve.Extrapolator m_HorizontalLeftExtrapolatorFactory;

            /// <summary>The extrapolation approach in horizontal direction on the left side of the grid points.            
            /// </summary>
            private ICurveExtrapolator m_HorizontalLeftExtrapolator;

            /// <summary>The extrapolation factory in horizontal direction on the right side of the grid points.
            /// </summary>
            private GridPointCurve.Extrapolator m_HorizontalRightExtrapolatorFactory;

            /// <summary>The extrapolation approach in horizontal direction on the right side of the grid points.
            /// </summary>
            private ICurveExtrapolator m_HorizontalRightExtrapolator;

            /// <summary>A temporary array that contains the values for a interpolation etc. along horizontal direction, i.e. at most <c>m_HorizontalDoubleLabels.Length</c> elements.
            /// </summary>
            /// <remarks>This member is used for performance reason.</remarks>
            private double[] m_TempValuesForHorizontalEvaluation;

            /// <summary>A temporary array that contains the <see cref="System.Double"/> representation of some horizontal labels.
            /// </summary>
            /// <remarks>This member is used for performance reason.</remarks>
            private double[] m_TempHorizontalDoubleLabels;
            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="HInterpolation"/> class.
            /// </summary>
            /// <param name="gridPointMatrix">The grid point matrix.</param>
            /// <param name="verticalCurveFactory">A factory for grid point curves along vertical direction, i.e. taken into account a specified interpolation, parametrization etc.</param>
            /// <param name="horizontalInterpolator">The interpolation approach along horizontal direction.</param>
            /// <param name="horizontalLeftExtrapolator">The extrapolation approach in horizontal direction on the left side of the grid points.</param>
            /// <param name="horizontalRightExtrapolator">The extrapolation approach in horizontal direction on the right side of the grid points.</param>
            internal HInterpolation(LabelMatrix<THorizontalLabel, TVerticalLabel> gridPointMatrix, Func<THorizontalLabel, IGridPointCurveFactory<TVerticalLabel>> verticalCurveFactory, GridPointCurve.Interpolator horizontalInterpolator, GridPointCurve.Extrapolator horizontalLeftExtrapolator, GridPointCurve.Extrapolator horizontalRightExtrapolator)
                : base(gridPointMatrix, verticalCurveFactory)
            {
                m_HorizontalInterpolatorFactory = horizontalInterpolator ?? throw new NullReferenceException(nameof(horizontalInterpolator));
                m_HorizontalInterpolator = horizontalInterpolator.Create();

                if (horizontalLeftExtrapolator == null)
                {
                    throw new NullReferenceException(nameof(horizontalLeftExtrapolator));
                }
                if (horizontalLeftExtrapolator.ExtrapolationBuildingDirection != GridPointCurve.Extrapolator.BuildingDirection.FromFirstGridPoint)
                {
                    throw new ArgumentException(String.Format("Invalid building direction of extrapolation on left side of the grid points."), nameof(horizontalLeftExtrapolator));
                }
                m_HorizontalLeftExtrapolatorFactory = horizontalLeftExtrapolator;
                m_HorizontalLeftExtrapolator = horizontalLeftExtrapolator.Create(m_HorizontalInterpolator);

                if (horizontalRightExtrapolator == null)
                {
                    throw new NullReferenceException(nameof(horizontalRightExtrapolator));
                }
                if (horizontalRightExtrapolator.ExtrapolationBuildingDirection != GridPointCurve.Extrapolator.BuildingDirection.FromLastGridPoint)
                {
                    throw new ArgumentException(String.Format("Invalid building direction of extrapolation on the right side of the grid points."), nameof(horizontalRightExtrapolator));
                }
                m_HorizontalRightExtrapolatorFactory = horizontalRightExtrapolator;
                m_HorizontalRightExtrapolator = horizontalRightExtrapolator.Create(m_HorizontalInterpolator);

                m_TempValuesForHorizontalEvaluation = new double[m_HorizontalDoubleLabels.Length];
                m_TempHorizontalDoubleLabels = new double[m_GridPointMatrix.ColumnCount];
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
                int columnCount = m_HorizontalDoubleLabels.Length;

                int firstColumnIndex = 0;
                int lastColumnIndex = columnCount - 1;
                int relevantColumnCount = columnCount;

                /* if the interpolation along horizontal direction is a local approach a few function evaluations are needed only: */
                if (m_HorizontalInterpolatorFactory.IsLocalApproach == true)
                {
                    if (x < m_HorizontalDoubleLabels[0])  // on the left side of the grid points
                    {
                        lastColumnIndex = Math.Min(columnCount - 1, m_HorizontalLeftExtrapolatorFactory.GetLevelOfGridPointDependency(columnCount));
                    }
                    else if (x > m_HorizontalDoubleLabels[columnCount - 1])  // on the right side of the grid points
                    {
                        firstColumnIndex = Math.Max(0, columnCount - m_HorizontalRightExtrapolatorFactory.GetLevelOfGridPointDependency(columnCount) - 1);
                    }
                    else // inside the grid points
                    {
                        int nonLastLeftGridPointIndex = GridPointCurve.Utilities.GetNonLastNearestIndex(x, m_HorizontalDoubleLabels, m_HorizontalDoubleLabels.Length);

                        int upperLocalnessLevel = m_HorizontalInterpolatorFactory.GetLeftLocalnessLevel(nonLastLeftGridPointIndex, columnCount);
                        int lowerLocalnessLevel = m_HorizontalInterpolatorFactory.GetRightLocalnessLevel(nonLastLeftGridPointIndex, columnCount);

                        firstColumnIndex = Math.Max(nonLastLeftGridPointIndex - upperLocalnessLevel, 0);
                        lastColumnIndex = Math.Min(nonLastLeftGridPointIndex + lowerLocalnessLevel, columnCount - 1);
                    }
                    relevantColumnCount = lastColumnIndex - firstColumnIndex + 1;

                    /* perhaps one may take into account less columns than required for the interpolation, i.e. one has to adjust the first/last column index: */
                    if (relevantColumnCount < m_HorizontalInterpolatorFactory.MinimalRequiredNumberOfGridPoints)
                    {
                        int numberOfAdditionalRequiredColumns = m_HorizontalInterpolatorFactory.MinimalRequiredNumberOfGridPoints - relevantColumnCount;

                        firstColumnIndex = Math.Max(0, firstColumnIndex - numberOfAdditionalRequiredColumns);
                        lastColumnIndex = Math.Min(columnCount - 1, lastColumnIndex + numberOfAdditionalRequiredColumns);

                        relevantColumnCount = lastColumnIndex - firstColumnIndex + 1;
                    }
                }

                /* compute values w.r.t. to the y-coordinate and each relevant column, i.e. store values to take into account for horizontal 
                 * interpolation. In the case of a non-local approach, one may improve performance by indicating that the labels will not be changed: */
                int k = 0;
                for (int j = firstColumnIndex; j <= lastColumnIndex; j++)
                {
                    m_TempValuesForHorizontalEvaluation[k++] = m_CurvesAlongVerticalDirection[j].GetValue(y);
                }

                var horizontalCurveState = GridPointCurve.State.GridPointChanged;
                if ((m_HorizontalInterpolatorFactory.IsLocalApproach == false) && (m_HorizontalInterpolator.IsOperable == true) && (m_HorizontalInterpolator.GridPointCount == columnCount))
                {
                    horizontalCurveState = GridPointCurve.State.GridPointValueChanged;
                }
                m_HorizontalInterpolator.Update(relevantColumnCount, m_HorizontalDoubleLabels, m_TempValuesForHorizontalEvaluation, horizontalCurveState, gridPointArgumentStartIndex: firstColumnIndex);

                if (x < m_HorizontalInterpolator.LowerBound)
                {
                    m_HorizontalLeftExtrapolator.Update();
                    return m_HorizontalLeftExtrapolator.GetValue(x);
                }
                else if (x > m_HorizontalInterpolator.UpperBound)
                {
                    m_HorizontalRightExtrapolator.Update();
                    return m_HorizontalRightExtrapolator.GetValue(x);
                }
                return m_HorizontalInterpolator.GetValue(x);
            }

            /// <summary>Gets a value at a specified point (<paramref name="x"/>, <paramref name="rowIndex"/>).
            /// </summary>
            /// <param name="x">The x coordinate of the point.</param>
            /// <param name="rowIndex">The null-based row index of the point.</param>
            /// <returns>The value of the surface at (<paramref name="x"/>, <paramref name="rowIndex"/>).</returns>
            public override double GetValue(double x, int rowIndex)
            {
                if (m_VerticalExactFitToGridPoints == false)
                {
                    if (m_HorizontalLabelMapping == null)
                    {
                        return GetValue(x, m_GridPointMatrix.VerticalDoubleLabels[rowIndex]);
                    }
                    else
                    {
                        return GetValue(x, m_GridPointMatrix.VerticalDoubleLabels[m_HorizontalLabelMapping[rowIndex]]);
                    }
                }

                /* if the vertical curve fitting meets the grid points, an evaluation of the vertical curve fitting is not necessary. Here, we do not distinguish 
                 * between local and non-local interpolation approach and we take into account all values (!= NaN) along horizontal direction: */

                int numberOfRelevantColumns = 0;

                for (int j = 0; j < m_GridPointMatrix.ColumnCount; j++)
                {
                    double gridValue = m_GridPointMatrix[rowIndex, j];
                    if (Double.IsNaN(gridValue) == false)
                    {
                        m_TempValuesForHorizontalEvaluation[numberOfRelevantColumns] = gridValue;
                        m_TempHorizontalDoubleLabels[numberOfRelevantColumns] = m_GridPointMatrix.HorizontalDoubleLabels[j];
                        numberOfRelevantColumns++;
                    }
                }
                var horizontalCurveState = GridPointCurve.State.GridPointChanged;
                if ((m_HorizontalInterpolator.IsOperable == true) && (m_HorizontalInterpolator.GridPointCount == m_GridPointMatrix.ColumnCount) && (numberOfRelevantColumns == m_GridPointMatrix.ColumnCount))
                {
                    horizontalCurveState = GridPointCurve.State.GridPointValueChanged;
                }
                m_HorizontalInterpolator.Update(numberOfRelevantColumns, m_TempHorizontalDoubleLabels, m_TempValuesForHorizontalEvaluation, horizontalCurveState);

                if (x < m_HorizontalInterpolator.LowerBound)
                {
                    m_HorizontalLeftExtrapolator.Update();
                    return m_HorizontalLeftExtrapolator.GetValue(x);
                }
                else if (x > m_HorizontalInterpolator.UpperBound)
                {
                    m_HorizontalRightExtrapolator.Update();
                    return m_HorizontalRightExtrapolator.GetValue(x);
                }
                return m_HorizontalInterpolator.GetValue(x);
            }
            #endregion
        }
    }
}