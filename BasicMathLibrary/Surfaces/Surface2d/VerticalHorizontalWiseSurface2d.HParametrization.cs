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
        /// <summary>Represents a two-dimensional grid point surface, where a parametrization takes place in horizontal direction.
        /// </summary>
        internal class HParametrization : VerticalHorizontalWiseSurface2d<THorizontalLabel, TVerticalLabel>
        {
            #region private members

            /// <summary>The interpolator factory along horizontal direction.
            /// </summary>
            private GridPointCurve.Parametrization m_HorizontalParametrizationFactory;

            /// <summary>The interpolation approach along vertical direction.
            /// </summary>
            private ICurveDataFitting m_HorizontalParametrization;

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

            /// <summary>Initializes a new instance of the <see cref="HParametrization"/> class.
            /// </summary>
            /// <param name="gridPointMatrix">The grid point matrix.</param>
            /// <param name="verticalCurveFactory">A factory for grid point curves along vertical direction, i.e. taken into account a specified interpolation, parametrization etc.</param>
            /// <param name="horizontalParametrization">The interpolation approach along horizontal direction.</param>
            internal HParametrization(LabelMatrix<THorizontalLabel, TVerticalLabel> gridPointMatrix, Func<THorizontalLabel, IGridPointCurveFactory<TVerticalLabel>> verticalCurveFactory, GridPointCurve.Parametrization horizontalParametrization)
                : base(gridPointMatrix, verticalCurveFactory)
            {
                m_HorizontalParametrizationFactory = horizontalParametrization ?? throw new NullReferenceException(nameof(horizontalParametrization));
                m_HorizontalParametrization = horizontalParametrization.Create();

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

                /* compute values w.r.t. to the y-coordinate and each relevant column, i.e. store values to take into account for horizontal 
                 * interpolation. In the case of a non-local approach, one may improve performance by indicating that the labels will not be changed: */
                int k = 0;
                for (int j = 0; j < columnCount; j++)
                {
                    m_TempValuesForHorizontalEvaluation[k++] = m_CurvesAlongVerticalDirection[j].GetValue(y);
                }

                GridPointCurve.State horizontalCurveState = GridPointCurve.State.GridPointChanged;
                if ((m_HorizontalParametrization.IsOperable == true) && (m_HorizontalParametrization.GridPointCount == columnCount))
                {
                    horizontalCurveState = GridPointCurve.State.GridPointValueChanged;
                }
                m_HorizontalParametrization.Update(columnCount, m_HorizontalDoubleLabels, m_TempValuesForHorizontalEvaluation, horizontalCurveState);
                if ((x < m_HorizontalParametrization.LowerBound) || (x > m_HorizontalParametrization.UpperBound))
                {
                    throw new ArgumentOutOfRangeException();
                }
                return m_HorizontalParametrization.GetValue(x);
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
                if ((m_HorizontalParametrization.GridPointCount == m_GridPointMatrix.ColumnCount) && (numberOfRelevantColumns == m_GridPointMatrix.ColumnCount))
                {
                    horizontalCurveState = GridPointCurve.State.GridPointValueChanged;
                }
                m_HorizontalParametrization.Update(numberOfRelevantColumns, m_TempHorizontalDoubleLabels, m_TempValuesForHorizontalEvaluation, horizontalCurveState);

                if ((x < m_HorizontalParametrization.LowerBound) || (x > m_HorizontalParametrization.UpperBound))
                {
                    throw new ArgumentOutOfRangeException();
                }
                return m_HorizontalParametrization.GetValue(x);
            }
            #endregion
        }
    }
}