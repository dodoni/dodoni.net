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
    /// <summary>Represents a two-dimensional surface where an interpolation, parametrization etc. takes place in horizontal direction and 
    /// afterwards an interpolation, parametrization etc. takes place in vertical direction.
    /// </summary>
    internal abstract partial class HorizontalVerticalWiseSurface2d<THorizontalLabel, TVerticalLabel> : IGridPointSurface2d<THorizontalLabel, TVerticalLabel>
        where THorizontalLabel : IComparable<THorizontalLabel>, IEquatable<THorizontalLabel>
        where TVerticalLabel : IComparable<TVerticalLabel>
    {
        #region protected members

        /// <summary>The grid point matrix.
        /// </summary>
        protected readonly LabelMatrix<THorizontalLabel, TVerticalLabel> m_GridPointMatrix;

        /// <summary>A value indicating whether the interpolation, parametrization etc. in horizontal direction meets exactly the grid points.
        /// </summary>
        protected readonly bool m_HorizontalExactFitToGridPoints;

        /// <summary>The collection of <see cref="System.Double"/> of vertical labels.
        /// </summary>
        /// <remarks>Grid point rows which contains invalid numbers (Not-a-Number) will be ignored.</remarks>
        protected readonly double[] m_VerticalDoubleLabels;

        /// <summary>A mapping to the vertical label of the grid point matrix. 
        /// <para>Returns for the null-based index of a vertical label w.r.t. the grid point matrix, the corresponding null-based index for <see cref="m_VerticalDoubleLabels"/>.</para>
        /// This member is relevant for the case that at least one grid point row will be ignored; otherwise <c>null</c>.
        /// </summary>
        protected readonly Dictionary<int, int> m_VerticalLabelMapping = null;

        /// <summary>For each grid point row with respect to <see cref="m_VerticalDoubleLabels"/> a curve along horizontal direction.
        /// </summary>
        protected readonly IGridPointCurve<THorizontalLabel>[] m_CurvesAlongHorizontalDirection;
        #endregion

        #region protected constructors

        /// <summary>Initializes a new instance of the <see cref="HorizontalVerticalWiseSurface2d&lt;THorizontalLabel, TVerticalLabel&gt;"/> class.
        /// </summary>
        /// <param name="gridPointMatrix">The grid point matrix.</param>
        /// <param name="horizontalCurveFactory">A factory for grid point curves along horizontal direction for each vertical label, i.e. taken into account a specified interpolation, parametrization etc.</param>
        protected HorizontalVerticalWiseSurface2d(LabelMatrix<THorizontalLabel, TVerticalLabel> gridPointMatrix, Func<TVerticalLabel, IGridPointCurveFactory<THorizontalLabel>> horizontalCurveFactory)
        {
            m_GridPointMatrix = gridPointMatrix ?? throw new ArgumentNullException(nameof(gridPointMatrix));

            if (horizontalCurveFactory == null)
            {
                throw new ArgumentNullException(nameof(horizontalCurveFactory));
            }

            m_HorizontalExactFitToGridPoints = true;
            if (gridPointMatrix.IsCompletelyDefined == true)
            {
                /* all grid points are valid numbers, i.e. take into account all rows and columns: */
                m_VerticalDoubleLabels = gridPointMatrix.VerticalDoubleLabels.ToArray();

                m_CurvesAlongHorizontalDirection = new IGridPointCurve<THorizontalLabel>[gridPointMatrix.RowCount];
                for (int j = 0; j < gridPointMatrix.RowCount; j++)
                {
                    var curveFactory = horizontalCurveFactory(gridPointMatrix.VerticalLabels[j]);
                    var curve = curveFactory.Create(gridPointMatrix.ColumnCount, gridPointMatrix.HorizontalLabels, gridPointMatrix.HorizontalDoubleLabels, gridPointMatrix.Data, gridPointValueStartIndex: j, gridPointValueIncrement: gridPointMatrix.RowCount);

                    m_CurvesAlongHorizontalDirection[j] = curve;
                    m_HorizontalExactFitToGridPoints &= (curveFactory.FittingQuality == FittingQuality.Exact);
                }
            }
            else /* ignore rows that contains to many Not-a-Number entries: */
            {
                var verticalDoubleLabels = new List<double>();
                m_VerticalLabelMapping = new Dictionary<int, int>();
                var curvesAlongHorizontalDirection = new List<IGridPointCurve<THorizontalLabel>>();

                for (int j = 0; j < gridPointMatrix.RowCount; j++)
                {
                    var curveFactory = horizontalCurveFactory(gridPointMatrix.VerticalLabels[j]);
                    var curve = curveFactory.Create();
                    foreach (var (HorizontalLabel, HorizontalDoubleLabel, Value) in gridPointMatrix.GetRowWithLabels(j))
                    {
                        if (Double.IsNaN(Value) == false)
                        {
                            curve.Add(HorizontalLabel, HorizontalDoubleLabel, Value);
                        }
                    }
                    int minimalRequiredNumberOfGridPoints = curveFactory.MinimalRequiredNumberOfGridPoints;

                    if (curve.GridPointCount >= minimalRequiredNumberOfGridPoints)
                    {
                        curve.Update();
                        if (curve.IsOperable == false)
                        {
                            throw new NotOperableException(String.Format("Horizontal curve, row index = {0} is not operable.", j));
                        }
                        curvesAlongHorizontalDirection.Add(curve);
                        verticalDoubleLabels.Add(gridPointMatrix.VerticalDoubleLabels[j]);
                        m_HorizontalExactFitToGridPoints &= (curveFactory.FittingQuality == FittingQuality.Exact);

                        m_VerticalLabelMapping.Add(j, curvesAlongHorizontalDirection.Count - 1);
                    }
                    // else: ignore the horizontal curve
                }
                m_VerticalDoubleLabels = verticalDoubleLabels.ToArray();
                m_CurvesAlongHorizontalDirection = curvesAlongHorizontalDirection.ToArray();
            }

            if (m_CurvesAlongHorizontalDirection.Length < 2)
            {
                throw new ArgumentException(String.Format("Only {0} valid rows available.", m_CurvesAlongHorizontalDirection.Length), "gridPointMatrix");
            }
        }
        #endregion

        #region public properties

        #region IOperable Members

        /// <summary>Gets a value indicating whether this instance is operable.
        /// </summary>
        /// <value><c>true</c> if this instance is operable; otherwise, <c>false</c>.</value>
        public bool IsOperable
        {
            get { return true; }
        }
        #endregion

        #region IGridPointSurface2d<THorizontalLabel,TVerticalLabel> Members

        /// <summary>Gets the (read-only) grid point matrix. Missing values are perhaps filled with respect to a specific replenish approach.
        /// </summary>
        /// <value>The grid points of the two-dimensional surface.</value>
        public LabelMatrix<THorizontalLabel, TVerticalLabel> GridPointMatrix
        {
            get { return m_GridPointMatrix; }
        }
        #endregion

        #endregion

        #region public methods

        #region ISurface2d Members

        /// <summary>Gets a value of the surface at a specified (x,y) coordinate.
        /// </summary>
        /// <param name="x">The x coordinate of the point.</param>
        /// <param name="y">The y coordinate of the point.</param>
        /// <returns>The value of the surface at (<paramref name="x"/>, <paramref name="y"/>).</returns>
        public abstract double GetValue(double x, double y);
        #endregion

        #region IGridPointSurface2d<THorizontalLabel,TVerticalLabel> Members

        /// <summary>Gets a value at a specified point (<paramref name="columnIndex"/>, <paramref name="y"/>).
        /// </summary>
        /// <param name="columnIndex">The null-based column index of the point.</param>
        /// <param name="y">The y coordinate of the point.</param>
        /// <returns>The value of the surface at (<paramref name="columnIndex"/>, <paramref name="y"/>).</returns>
        public abstract double GetValue(int columnIndex, double y);

        /// <summary>Gets a value at a specified point (<paramref name="horizontalLabel"/>, <paramref name="y"/>).
        /// </summary>
        /// <param name="horizontalLabel">The horizontal label (i.e. name of the column) of the point.</param>
        /// <param name="y">The y coordinate of the point.</param>
        /// <param name="value">The value of the surface at (<paramref name="horizontalLabel"/>, <paramref name="y"/>).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public bool TryGetValue(THorizontalLabel horizontalLabel, double y, out double value)
        {
            if (m_GridPointMatrix.TryGetHorizontalLabelIndex(horizontalLabel, out int horizontalLabelIndex) == true)
            {
                value = GetValue(columnIndex: horizontalLabelIndex, y: y);
                return true;
            }
            value = Double.NaN;
            return false;
        }

        /// <summary>Gets a value at a specified point (<paramref name="x"/>, <paramref name="rowIndex"/>).
        /// </summary>
        /// <param name="x">The x coordinate of the point.</param>
        /// <param name="rowIndex">The null-based row index of the point.</param>
        /// <returns>The value of the surface at (<paramref name="x"/>, <paramref name="rowIndex"/>).</returns>
        public double GetValue(double x, int rowIndex)
        {
            if (m_VerticalLabelMapping == null)
            {
                return m_CurvesAlongHorizontalDirection[rowIndex].GetValue(x);
            }
            if (m_VerticalLabelMapping.ContainsKey(rowIndex) == true)
            {
                return m_CurvesAlongHorizontalDirection[m_VerticalLabelMapping[rowIndex]].GetValue(x);
            }
            return GetValue(x, m_GridPointMatrix.VerticalDoubleLabels[rowIndex]);
        }

        /// <summary>Gets a value at a specified point (<paramref name="x"/>, <paramref name="verticalLabel"/>).
        /// </summary>
        /// <param name="x">The x coordinate of the point.</param>
        /// <param name="verticalLabel">The vertical label of the point.</param>
        /// <param name="value">The value of the surface at (<paramref name="x"/>, <paramref name="verticalLabel"/>).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public bool TryGetValue(double x, TVerticalLabel verticalLabel, out double value)
        {
            if (m_GridPointMatrix.TryGetVerticalLabelIndex(verticalLabel, out int originalVerticalLabelIndex) == true)
            {
                value = GetValue(x, originalVerticalLabelIndex);
                return true;
            }
            value = Double.NaN;
            return false;
        }
        #endregion

        #endregion
    }
}