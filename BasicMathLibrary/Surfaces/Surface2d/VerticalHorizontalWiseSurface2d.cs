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
    /// <summary>Represents a two-dimensional surface where an interpolation, parametrization etc. takes place in verical direction and 
    /// afterwards an interpolation, parametrization etc. takes place in horizontal direction.
    /// </summary>
    internal abstract partial class VerticalHorizontalWiseSurface2d<THorizontalLabel, TVerticalLabel> : IGridPointSurface2d<THorizontalLabel, TVerticalLabel>
        where THorizontalLabel : IComparable<THorizontalLabel>
        where TVerticalLabel : IComparable<TVerticalLabel>, IEquatable<TVerticalLabel>
    {
        #region protected members

        /// <summary>The grid point matrix.
        /// </summary>
        protected readonly LabelMatrix<THorizontalLabel, TVerticalLabel> m_GridPointMatrix;

        /// <summary>A value indicating whether the interpolation, parametrization etc. in horizontal direction meets exactly the grid points.
        /// </summary>
        protected readonly bool m_VerticalExactFitToGridPoints;

        /// <summary>The collection of <see cref="System.Double"/> of horizontal labels.
        /// </summary>
        /// <remarks>Grid point rows which contains invalid numbers (Not-a-Number) will be ignored.</remarks>
        protected readonly double[] m_HorizontalDoubleLabels;

        /// <summary>A mapping to the horizontal label of the grid point matrix. 
        /// <para>Returns for the null-based index of a horizontal label (column index) w.r.t. the grid point matrix, the corresponding null-based index for <see cref="m_HorizontalDoubleLabels"/>.</para>
        /// This member is relevant for the case that at least one grid point column will be ignored; otherwise <c>null</c>.
        /// </summary>
        protected readonly Dictionary<int, int> m_HorizontalLabelMapping = null;

        /// <summary>For each grid point column with respect to <see cref="m_HorizontalDoubleLabels"/> a curve along vertical direction.
        /// </summary>
        protected readonly IGridPointCurve<TVerticalLabel>[] m_CurvesAlongVerticalDirection;
        #endregion

        #region protected constructors

        /// <summary>Initializes a new instance of the <see cref="VerticalHorizontalWiseSurface2d&lt;THorizontalLabel, TVerticalLabel&gt;"/> class.
        /// </summary>
        /// <param name="gridPointMatrix">The grid point matrix.</param>
        /// <param name="verticalCurveFactory">A factory for grid point curves along vertical direction, i.e. taken into account a specified interpolation, parametrization etc.</param>
        protected VerticalHorizontalWiseSurface2d(LabelMatrix<THorizontalLabel, TVerticalLabel> gridPointMatrix, Func<THorizontalLabel, IGridPointCurveFactory<TVerticalLabel>> verticalCurveFactory)
        {
            m_GridPointMatrix = gridPointMatrix ?? throw new ArgumentNullException(nameof(GridPointMatrix));

            if (verticalCurveFactory == null)
            {
                throw new ArgumentNullException(nameof(verticalCurveFactory));
            }

            m_VerticalExactFitToGridPoints = true;
            if (gridPointMatrix.IsCompletelyDefined == true)
            {
                /* all grid points are valid numbers, i.e. take into account all rows and columns: */
                m_HorizontalDoubleLabels = gridPointMatrix.HorizontalDoubleLabels.ToArray();

                m_CurvesAlongVerticalDirection = new IGridPointCurve<TVerticalLabel>[gridPointMatrix.ColumnCount];
                for (int j = 0; j < gridPointMatrix.ColumnCount; j++)
                {
                    var curveFactory = verticalCurveFactory(gridPointMatrix.HorizontalLabels[j]);
                    var curve = curveFactory.Create(gridPointMatrix.RowCount, gridPointMatrix.VerticalLabels, gridPointMatrix.VerticalDoubleLabels, gridPointMatrix.Data, gridPointValueStartIndex: j * gridPointMatrix.RowCount, gridPointValueIncrement: 1);

                    m_CurvesAlongVerticalDirection[j] = curve;
                    m_VerticalExactFitToGridPoints &= (curveFactory.FittingQuality == FittingQuality.Exact);
                }
            }
            else /* ignore columns that contains to many Not-a-Number entries: */
            {
                var horizontalDoubleLabels = new List<double>();
                m_HorizontalLabelMapping = new Dictionary<int, int>();
                var curvesAlongVerticalDirection = new List<IGridPointCurve<TVerticalLabel>>();

                for (int j = 0; j < gridPointMatrix.ColumnCount; j++)
                {
                    var curveFactory = verticalCurveFactory(gridPointMatrix.HorizontalLabels[j]);
                    var curve = curveFactory.Create();

                    foreach (var (VerticalLabel, VerticalDoubleLabel, Value) in gridPointMatrix.GetColumnWithLabels(j))
                    {
                        if (Double.IsNaN(Value) == false)
                        {
                            curve.Add(VerticalLabel, VerticalDoubleLabel, Value);
                        }
                    }
                    int minimalRequiredNumberOfGridPoints = curveFactory.MinimalRequiredNumberOfGridPoints;

                    if (curve.GridPointCount >= minimalRequiredNumberOfGridPoints)
                    {
                        curve.Update();
                        if (curve.IsOperable == false)
                        {
                            throw new NotOperableException(String.Format("Vertical curve, column index = {0} is not operable.", j));
                        }
                        curvesAlongVerticalDirection.Add(curve);
                        horizontalDoubleLabels.Add(gridPointMatrix.HorizontalDoubleLabels[j]);
                        m_VerticalExactFitToGridPoints &= (curveFactory.FittingQuality == FittingQuality.Exact);

                        m_HorizontalLabelMapping.Add(j, curvesAlongVerticalDirection.Count - 1);
                    }
                    // else: ignore the vertical curve
                }
                m_HorizontalDoubleLabels = horizontalDoubleLabels.ToArray();
                m_CurvesAlongVerticalDirection = curvesAlongVerticalDirection.ToArray();
            }

            if (m_CurvesAlongVerticalDirection.Length < 2)
            {
                throw new ArgumentException(String.Format("Only {0} valid column available.", m_CurvesAlongVerticalDirection.Length), "gridPointMatrix");
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
        public double GetValue(int columnIndex, double y)
        {
            if (m_HorizontalLabelMapping == null)
            {
                return m_CurvesAlongVerticalDirection[columnIndex].GetValue(y);
            }
            if (m_HorizontalLabelMapping.ContainsKey(columnIndex) == true)
            {
                return m_CurvesAlongVerticalDirection[m_HorizontalLabelMapping[columnIndex]].GetValue(y);
            }
            return GetValue(m_GridPointMatrix.HorizontalDoubleLabels[columnIndex], y);
        }

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
        public abstract double GetValue(double x, int rowIndex);

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