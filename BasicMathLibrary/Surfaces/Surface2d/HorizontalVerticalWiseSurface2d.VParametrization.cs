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
        /// <summary>Represents a two-dimensional grid point surface, where a parametrization takes place in vertical direction.
        /// </summary>
        internal class VParametrization : HorizontalVerticalWiseSurface2d<THorizontalLabel, TVerticalLabel>
        {
            #region private members

            /// <summary>The parametrization factory along vertical direction.
            /// </summary>
            private GridPointCurve.Parametrization m_VerticalParametrizationFactory;

            /// <summary>The parametrization approach along vertical direction.
            /// </summary>
            private ICurveDataFitting m_VerticalParametrization;

            /// <summary>A temporary array that contains the values for a parametrization etc. along vertical direction, i.e. at most <c>m_VerticalDoubleLabels.Length</c> elements.
            /// </summary>
            /// <remarks>This member is used for performance reason.</remarks>
            private double[] m_TempValuesForVerticalEvaluation;

            /// <summary>A temporary array that contains the <see cref="System.Double"/> representation of some vertical labels.
            /// </summary>
            /// <remarks>This member is used for performance reason.</remarks>
            private double[] m_TempVerticalDoubleLabels;
            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="VParametrization"/> class.
            /// </summary>
            /// <param name="gridPointMatrix">The grid point matrix.</param>
            /// <param name="horizontalCurveFactory">A factory for grid point curves along horizontal direction, i.e. taken into account a specified interpolation, parametrization etc.</param>
            /// <param name="verticalParametrization">The parametrization approach along vertical direction.</param>
            internal VParametrization(LabelMatrix<THorizontalLabel, TVerticalLabel> gridPointMatrix, Func<TVerticalLabel, IGridPointCurveFactory<THorizontalLabel>> horizontalCurveFactory, GridPointCurve.Parametrization verticalParametrization)
                : base(gridPointMatrix, horizontalCurveFactory)
            {
                m_VerticalParametrizationFactory = verticalParametrization ?? throw new NullReferenceException(nameof(verticalParametrization));
                m_VerticalParametrization = verticalParametrization.Create();

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

                int k = 0;
                for (int j = 0; j < rowCount; j++)
                {
                    m_TempValuesForVerticalEvaluation[k++] = m_CurvesAlongHorizontalDirection[j].GetValue(x);
                }

                GridPointCurve.State verticalCurveState = GridPointCurve.State.GridPointChanged;
                if ((m_VerticalParametrization.IsOperable == true) && (m_VerticalParametrization.GridPointCount == rowCount))
                {
                    verticalCurveState = GridPointCurve.State.GridPointValueChanged;
                }
                m_VerticalParametrization.Update(rowCount, m_VerticalDoubleLabels, m_TempValuesForVerticalEvaluation, verticalCurveState);

                if ((y < m_VerticalParametrization.LowerBound) || (y > m_VerticalParametrization.UpperBound))
                {
                    throw new ArgumentOutOfRangeException();
                }
                return m_VerticalParametrization.GetValue(y);
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

                /* if the horizontal curve fitting meets the grid points, an evaluation of the horizontal curve fitting is not necessary. We take into account all values (!= NaN) along vertical direction: */
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
                if ((m_VerticalParametrization.IsOperable == true) && (m_VerticalParametrization.GridPointCount == m_GridPointMatrix.RowCount) && (numberOfRelevantRows == m_GridPointMatrix.RowCount))
                {
                    verticalCurveState = GridPointCurve.State.GridPointValueChanged;
                }
                m_VerticalParametrization.Update(numberOfRelevantRows, m_TempVerticalDoubleLabels, m_TempValuesForVerticalEvaluation, verticalCurveState);

                if ((y < m_VerticalParametrization.LowerBound) || (y > m_VerticalParametrization.UpperBound))
                {
                    throw new ArgumentOutOfRangeException();
                }
                return m_VerticalParametrization.GetValue(y);
            }
            #endregion
        }
    }
}