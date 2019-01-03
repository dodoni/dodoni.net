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
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Dodoni.BasicComponents;
using Dodoni.MathLibrary.Basics;
using Dodoni.BasicComponents.Containers;
using Dodoni.MathLibrary.Surfaces.MissingValueReplenishments;

namespace Dodoni.MathLibrary.Surfaces
{
    /// <summary>Serves as factory for <see cref="LabelMatrix&lt;THorizontalLabel,TVerticalLabel&gt;"/> objects.
    /// </summary>
    public static class LabelMatrix
    {
        #region nested classes

        /// <summary>Represents the order of the input parameters taken into account for creating a specific <see cref="LabelMatrix&lt;THorizontalLabel,TVerticalLabel&gt;"/> object.
        /// </summary>
        [Flags]
        public enum OrderOfInput
        {
            /// <summary>The <see cref="System.Double"/> representation of horizontal and vertical labels are in ascending order.
            /// </summary>
            AscendingOrder = 0x00,

            /// <summary>The <see cref="System.Double"/> representation of horizontal labels are disordered.
            /// </summary>
            DisorderedHorizontalLabels = 0x01,

            /// <summary>The <see cref="System.Double"/> representation of vertical labels are disordered.
            /// </summary>
            DisorderedVerticalLabels = 0x02,

            /// <summary>The <see cref="System.Double"/> representation of horizontal and vertical labels are disordered.
            /// </summary>
            Disordered = DisorderedHorizontalLabels | DisorderedVerticalLabels
        }

        /// <summary>Serves as abstract basis class for missing value replenishments, i.e. as factory for <see cref="IMissingValueReplenishment"/> objects. 
        /// Moverover it provides some basic missing value replenishment approaches.
        /// </summary>
        public abstract class MissingValueReplenishment : IIdentifierNameable, IAnnotatable
        {
            #region public static (readonly) members

            /// <summary>Do not replenish any missing value.
            /// </summary>
            public static readonly MissingValueReplenishment None = new MatrixReplenishRuleNone();

            /// <summary>Serves as factory for a missing value replenishment, where for each missing value a interpolation along x-axis and y-axis of the 
            /// nearest (valid) grid points takes place. Missing values are specified as a convex combination of both interpolations.
            /// </summary>
            public static readonly WeightedNearestGridPointMissingValueReplenishment WeightedNearestGridPoints = new WeightedNearestGridPointMissingValueReplenishment();
            #endregion

            #region private members

            /// <summary>A short description of the missing value replenishment.
            /// </summary>
            private string m_Annotation;
            #endregion

            #region static constructor

            /// <summary>Initializes the <see cref="MissingValueReplenishment"/> class.
            /// </summary>
            static MissingValueReplenishment()
            {
            }
            #endregion

            #region protected constructors

            /// <summary>Initializes a new instance of the <see cref="MissingValueReplenishment"/> class.
            /// </summary>
            protected MissingValueReplenishment()
            {
                m_Annotation = String.Empty;
            }

            /// <summary>Initializes a new instance of the <see cref="MissingValueReplenishment"/> class.
            /// </summary>
            /// <param name="annotation">The annotation, i.e. short description, of the missing value replenishment.</param>
            protected MissingValueReplenishment(string annotation)
            {
                m_Annotation = annotation ?? String.Empty;
            }
            #endregion

            #region public properties

            #region IIdentifierNameable Members

            /// <summary>Gets the name of the missing value replenishment.
            /// </summary>
            /// <value>The language independent name of the missing value replenishment.</value>
            public IdentifierString Name
            {
                get { return GetName(); }
            }

            /// <summary>Gets the long name of the missing value replenishment.
            /// </summary>
            /// <value>The (perhaps) language dependent long name of the missing value replenishment.</value>
            public IdentifierString LongName
            {
                get { return GetLongName(); }
            }
            #endregion

            #region IAnnotatable Members

            /// <summary>Gets a value indicating whether the annotation is read-only.
            /// </summary>
            /// <value><c>true</c> if the annotation of this instance is readonly; otherwise, <c>false</c>.</value>
            public bool HasReadOnlyAnnotation
            {
                get { return false; }
            }

            /// <summary>Gets the annotation of the current instance.
            /// </summary>
            /// <value>The annotation of the current instance.</value>
            public string Annotation
            {
                get { return m_Annotation; }
            }
            #endregion

            #endregion

            #region public methods

            #region IAnnotatable Members

            /// <summary>Sets the annotation of the current instance.
            /// </summary>
            /// <param name="annotation">The annotation.</param>
            /// <returns>A value indicating whether the <see cref="Annotation"/> has been changed.</returns>
            public bool TrySetAnnotation(string annotation)
            {
                m_Annotation = annotation ?? String.Empty;
                return true;
            }
            #endregion

            /// <summary>Creates a <see cref="IMissingValueReplenishment"/> object that represents the implementation of the missing value replenishment.
            /// </summary>
            /// <returns>A <see cref="IMissingValueReplenishment"/> object that represents the implementation of the missing value replenishment.</returns>
            public abstract IMissingValueReplenishment Create();

            /// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
            /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
            public override string ToString()
            {
                return Name.String;
            }
            #endregion

            #region protected methods

            /// <summary>Gets the name of the missing value replenishment.
            /// </summary>
            /// <returns>The name of the missing value replenishment.</returns>
            protected abstract IdentifierString GetName();

            /// <summary>Gets the long name of the missing value replenishment.
            /// </summary>
            /// <returns>The (perhaps) language dependent long name of the missing value replenishment.</returns>
            protected abstract IdentifierString GetLongName();
            #endregion
        }
        #endregion

        #region public (static) methods

        /// <summary>Creates a new <see cref="LabelMatrix&lt;THorizontalLabel,TVerticalLabel&gt;"/> object.
        /// </summary>
        /// <typeparam name="THorizontalLabel">The type of the horizontal label.</typeparam>
        /// <typeparam name="TVerticalLabel">The type of the vertical label.</typeparam>
        /// <param name="rowCount">The number of rows.</param>
        /// <param name="columnCount">The number of columns.</param>
        /// <param name="data">The data, i.e. the grid points provided column-by-column. <see cref="System.Double.NaN"/> entries are allowed.</param>
        /// <param name="horizontalLabels">The horizontal labels.</param>
        /// <param name="horizontalDoubleLabels">The horizontal labels in its <see cref="System.Double"/> representation.</param>
        /// <param name="verticalLabels">The vertical labels.</param>
        /// <param name="verticalDoubleLabels">The vertical labels in its <see cref="System.Double"/> representation.</param>
        /// <param name="missingValueReplenishment">The missing value replenishment; if <c>null</c> and if <paramref name="data"/> contains missing 
        /// values it cannot be ensured that the surface is well-defined for each point (depends on interpolation/extrapolation etc. and on the location of missing values).</param>
        /// <param name="orderOfInput">A value indicating whether the input, i.e. the <see cref="System.Double"/> representation of the labels are in ascending order.</param>
        /// <param name="UpperLeftCornerLabel">The label in the 'upper left corner', i.e. some optional additional header label.</param>
        /// <returns>An object that represents a matrix with some additional horizontal and vertical header.</returns>
        /// <remarks>No missing value replenishment will take place.</remarks>
        public static LabelMatrix<THorizontalLabel, TVerticalLabel> Create<THorizontalLabel, TVerticalLabel>(int rowCount, int columnCount, IList<double> data, IList<THorizontalLabel> horizontalLabels, IList<double> horizontalDoubleLabels, IList<TVerticalLabel> verticalLabels, IList<double> verticalDoubleLabels, MissingValueReplenishment missingValueReplenishment = null, OrderOfInput orderOfInput = OrderOfInput.AscendingOrder, string UpperLeftCornerLabel = "")
            where THorizontalLabel : IComparable<THorizontalLabel>
            where TVerticalLabel : IComparable<TVerticalLabel>
        {
            if (missingValueReplenishment != null)
            {
                return new LabelMatrix<THorizontalLabel, TVerticalLabel>(rowCount, columnCount, data, horizontalLabels, horizontalDoubleLabels, verticalLabels, verticalDoubleLabels, missingValueReplenishment.Create(), orderOfInput, UpperLeftCornerLabel);
            }
            else
            {
                return new LabelMatrix<THorizontalLabel, TVerticalLabel>(rowCount, columnCount, data, horizontalLabels, horizontalDoubleLabels, verticalLabels, verticalDoubleLabels, orderOfInput: orderOfInput, upperLeftCornerLabel: UpperLeftCornerLabel);
            }
        }

        /// <summary>Creates a new <c>LabelMatrix&lt;double,double&gt;</c> object.
        /// </summary>
        /// <param name="rowCount">The number of rows.</param>
        /// <param name="columnCount">The number of columns.</param>
        /// <param name="data">The data, i.e. the grid points provided column-by-column. <see cref="System.Double.NaN"/> entries are allowed.</param>
        /// <param name="horizontalLabels">The horizontal labels.</param>
        /// <param name="verticalLabels">The vertical labels.</param>
        /// <param name="missingValueReplenishment">The missing value replenishment; if <c>null</c> and if <paramref name="data"/> contains missing 
        /// values it cannot be ensured that the surface is well-defined for each point (depends on interpolation/extrapolation etc. and on the location of missing values).</param>        
        /// <param name="orderOfInput">A value indicating whether the input, i.e. the <see cref="System.Double"/> representation of the labels are in ascending order.</param>
        /// <param name="UpperLeftCornerLabel">The label in the 'upper left corner', i.e. some optional additional header label.</param>
        /// <returns>An object that represents a matrix with some additional horizontal and vertical header.</returns>
        /// <remarks>No missing value replenishment will take place.</remarks>
        public static LabelMatrix<double, double> Create(int rowCount, int columnCount, IList<double> data, IList<double> horizontalLabels, IList<double> verticalLabels, MissingValueReplenishment missingValueReplenishment = null, OrderOfInput orderOfInput = OrderOfInput.AscendingOrder, string UpperLeftCornerLabel = "")
        {
            if (missingValueReplenishment != null)
            {
                return new LabelMatrix<double, double>(rowCount, columnCount, data, horizontalLabels, horizontalLabels, verticalLabels, verticalLabels, missingValueReplenishment.Create(), orderOfInput, UpperLeftCornerLabel);
            }
            return new LabelMatrix<double, double>(rowCount, columnCount, data, horizontalLabels, horizontalLabels, verticalLabels, verticalLabels, orderOfInput: orderOfInput, upperLeftCornerLabel: UpperLeftCornerLabel);
        }
        #endregion
    }

    /// <summary>Represents a real-valued matrix with additional labels and missing value replenish approach.
    /// </summary>
    /// <typeparam name="THorizontalLabel">The type of the horizontal labels.</typeparam>
    /// <typeparam name="TVerticalLabel">The type of the vertical labels.</typeparam>
    public class LabelMatrix<THorizontalLabel, TVerticalLabel> : IMatrix, IInfoOutputQueriable
        where THorizontalLabel : IComparable<THorizontalLabel>
        where TVerticalLabel : IComparable<TVerticalLabel>
    {
        #region public (readonly) members

        /// <summary>The label in the 'upper left corner', i.e. some additional header label.
        /// </summary>
        public readonly string UpperLeftCornerLabel;

        /// <summary>A value indicating whether all entries in the matrix are valid numbers, i.e. no entry is <see cref="System.Double.NaN"/>.
        /// </summary>
        public readonly bool IsCompletelyDefined;

        /// <summary>The data provided column-by-column, i.e. '[i, j] = Data[i + RowCount * j]', where i: null-based row index, j: null-based column index.
        /// </summary>
        public readonly ReadOnlyCollection<double> Data;

        /// <summary>A read-only collection for the <see cref="System.Double"/> representation of the horizontal labels.
        /// </summary>
        public readonly ReadOnlyCollection<double> HorizontalDoubleLabels;

        /// <summary>A read-only collection of the horizontal labels.
        /// </summary>
        public readonly ReadOnlyCollection<THorizontalLabel> HorizontalLabels;

        /// <summary>A read-only collection for the <see cref="System.Double"/> representation of the vertical labels.
        /// </summary>
        public readonly ReadOnlyCollection<double> VerticalDoubleLabels;

        /// <summary>A read-only collection of the vertical labels.
        /// </summary>
        public readonly ReadOnlyCollection<TVerticalLabel> VerticalLabels;
        #endregion

        #region private members

        /// <summary>The number of rows.
        /// </summary>
        private int m_RowCount;

        /// <summary>The number of columns.
        /// </summary>
        private int m_ColumnCount;

        /// <summary>The data provided column-by-column, i.e. '[i, j] = m_Data[i + m_RowCount * j]'.
        /// </summary>
        private double[] m_Data;

        /// <summary>The horizontal labels (i.e. the header).
        /// </summary>
        private THorizontalLabel[] m_HorizontalLabels;

        /// <summary>The horizontal labels in its <see cref="System.Double"/> representation.
        /// </summary>
        private double[] m_HorizontalDoubleLabels;

        ///<summary>The vertical labels.
        /// </summary>
        private TVerticalLabel[] m_VerticalLabels;

        /// <summary>The vertical labels in its <see cref="System.Double"/> representation.
        /// </summary>
        private double[] m_VerticalDoubleLabels;

        /// <summary>A collection of null-based row (key) and column (values) indices of grid points that has been filled with respect to the specified missing value replenishment; perhaps <c>null</c>.
        /// </summary>
        private Dictionary<int, HashSet<int>> m_ReplenishIndices = null;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="LabelMatrix&lt;THorizontalLabel, TVerticalLabel&gt;"/> class.
        /// </summary>
        /// <param name="rowCount">The row count.</param>
        /// <param name="columnCount">The column count.</param>
        /// <param name="data">The entries of the matrix provided column-by-column.</param>
        /// <param name="horizontalLabels">The horizontal labels (i.e. header).</param>
        /// <param name="horizontalDoubleLabels">The <see cref="System.Double"/> representation of the horizontal labels.</param>
        /// <param name="verticalLabels">The vertical labels.</param>
        /// <param name="verticalDoubleLabels">The <see cref="System.Double"/> representation of the vertical double labels.</param>
        /// <param name="missingValueReplenishment">The missing value replenishment.</param>
        /// <param name="orderOfInput">A value indicating whether the input, i.e. the <see cref="System.Double"/> representation of the labels are in ascending order.</param>
        /// <param name="upperLeftCornerLabel">The label in the 'upper left corner', i.e. some additional header label.</param>
        public LabelMatrix(int rowCount, int columnCount, IList<double> data, IList<THorizontalLabel> horizontalLabels, IList<double> horizontalDoubleLabels, IList<TVerticalLabel> verticalLabels, IList<double> verticalDoubleLabels, IMissingValueReplenishment missingValueReplenishment = null, LabelMatrix.OrderOfInput orderOfInput = LabelMatrix.OrderOfInput.AscendingOrder, string upperLeftCornerLabel = "")
        {
            UpperLeftCornerLabel = upperLeftCornerLabel;

            if (horizontalLabels == null)
            {
                throw new ArgumentNullException(nameof(horizontalLabels), String.Format(ExceptionMessages.ArgumentNull, "Horizontal labels"));
            }
            else if (horizontalLabels.Count == 0)
            {
                throw new ArgumentException(nameof(horizontalLabels), String.Format(ExceptionMessages.ArgumentHasWrongDimension, "Horizontal labels"));
            }
            if (verticalLabels == null)
            {
                throw new ArgumentNullException(nameof(verticalLabels), String.Format(ExceptionMessages.ArgumentNull, "Vertical labels"));
            }
            else if (verticalLabels.Count == 0)
            {
                throw new ArgumentException(nameof(verticalLabels), String.Format(ExceptionMessages.ArgumentHasWrongDimension, "Vertical labels"));
            }
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data), String.Format(ExceptionMessages.ArgumentNull, "Data matrix"));
            }
            m_RowCount = rowCount;
            m_ColumnCount = columnCount;

            /* copy all data: */
            m_HorizontalLabels = horizontalLabels.ToArray();
            m_VerticalLabels = verticalLabels.ToArray();

            m_VerticalDoubleLabels = new double[rowCount];
            verticalDoubleLabels.CopyTo(m_VerticalDoubleLabels, count: rowCount);

            m_HorizontalDoubleLabels = new double[columnCount];
            horizontalDoubleLabels.CopyTo(m_HorizontalDoubleLabels, count: columnCount);

            m_Data = new double[rowCount * columnCount];
            data.CopyTo(m_Data, count: rowCount * columnCount);

            /* maybe one has to order the data in a correct way: */
            if (orderOfInput.HasFlag(LabelMatrix.OrderOfInput.DisorderedHorizontalLabels))
            {
                Array.Sort(m_HorizontalDoubleLabels, m_HorizontalLabels, 0, columnCount);

                /* sort the columns of the matrix entries in the correct way, i.e. for each column j set
                 * 
                 *    m_Data[i+ j * rowCount] = m_Data[i + k_j * rowCount] for i = 0,...,rowCount-1,
                 *    
                 * where k_j is the original position of the jth column.
                 * */
                for (int j = 0; j < columnCount; j++)
                {
                    var horizontalLabel = m_HorizontalLabels[j];
                    int originalColumnIndex = horizontalLabels.IndexOf(horizontalLabel);

                    if (originalColumnIndex > j)
                    {
                        BLAS.Level1.dswap(rowCount, m_Data, 1, m_Data, 1, j * rowCount, originalColumnIndex * rowCount);
                    }
                }
            }
            else if (orderOfInput.HasFlag(LabelMatrix.OrderOfInput.DisorderedVerticalLabels))
            {
                Array.Sort(m_VerticalDoubleLabels, m_VerticalLabels, 0, rowCount);

                /* sort the rows of the matrix entries in the correct way, i.e. for each row i set
                 * 
                 *   m_Data[i+ j * rowCount] = m_Data[k_i + j * rowCount] for j = 0,...,columnCount-1,
                 *   
                 *   where k_i is the original position of the ith row.
                 */
                for (int i = 0; i < m_RowCount; i++)
                {
                    var verticalLabel = m_VerticalLabels[i];
                    int originalRowIndex = verticalLabels.IndexOf(verticalLabel);
                    if (originalRowIndex > i)
                    {
                        BLAS.Level1.dswap(columnCount, m_Data, rowCount, m_Data, rowCount, i, originalRowIndex);
                    }
                }
            }

            /* check wheter one entry is Not-a-Number: */
            IsCompletelyDefined = true;
            for (int k = 0; k < rowCount * columnCount; k++)
            {
                if (Double.IsNaN(m_Data[k]) == true)
                {
                    IsCompletelyDefined = false;
                    break;
                }
            }
            if ((IsCompletelyDefined == false) && (missingValueReplenishment != null))
            {
                var replenishIndices = missingValueReplenishment.Replenish(rowCount, columnCount, m_Data, m_HorizontalDoubleLabels, m_VerticalDoubleLabels);
                if (replenishIndices != null)
                {
                    m_ReplenishIndices = new Dictionary<int, HashSet<int>>();

                    foreach (var (RowIndex, ColumnIndex) in replenishIndices)
                    {
                        if (m_ReplenishIndices.ContainsKey(RowIndex) == false)
                        {
                            m_ReplenishIndices.Add(RowIndex, new HashSet<int>());
                        }
                        m_ReplenishIndices[RowIndex].Add(ColumnIndex);
                    }
                }
            }
            HorizontalDoubleLabels = new ReadOnlyCollection<double>(m_HorizontalDoubleLabels);
            HorizontalLabels = new ReadOnlyCollection<THorizontalLabel>(m_HorizontalLabels);
            VerticalDoubleLabels = new ReadOnlyCollection<double>(m_VerticalDoubleLabels);
            VerticalLabels = new ReadOnlyCollection<TVerticalLabel>(m_VerticalLabels);
            Data = new ReadOnlyCollection<double>(m_Data);
        }
        #endregion

        #region public properties

        #region IMatrix Members

        /// <summary>Gets the number of rows of the current matrix object.
        /// </summary>
        /// <value>The number of rows.</value>
        public int RowCount
        {
            get { return m_RowCount; }
        }

        /// <summary>Gets the number of columns of the current matrix object.
        /// </summary>
        /// <value>The number of columns.</value>
        public int ColumnCount
        {
            get { return m_ColumnCount; }
        }

        /// <summary>Gets a value indicating whether this instance represents a quadratic matrix.
        /// </summary>
        /// <value><c>true</c> if this instance is quadratic; otherwise, <c>false</c>.</value>
        public bool IsQuadratic
        {
            get { return (m_RowCount == m_ColumnCount); }
        }

        /// <summary>Gets a specified value.
        /// </summary>
        /// <param name="rowIndex">The null-based row index.</param>
        /// <param name="columnIndex">The null-based column index.</param>
        /// <value>The value of the matrix at the specified coordinate.</value>
        public double this[int rowIndex, int columnIndex]
        {
            get { return m_Data[rowIndex + m_RowCount * columnIndex]; }
        }
        #endregion

        #region IInfoOutputQueriable Members

        /// <summary>Gets the info-output level of detail.
        /// </summary>
        /// <value>The info-output level of detail.</value>
        public InfoOutputDetailLevel InfoOutputDetailLevel
        {
            get { return InfoOutputDetailLevel.Full; }
        }
        #endregion

        /// <summary>Gets a specified value.
        /// </summary>
        /// <param name="rowLabel">The row label of the grid point, i.e. the specific label with respect to the vertical labels.</param>
        /// <param name="columnLabel">The column label of the grid point, i.e. the specific label with respect to the horizontal labels (=header).</param>
        /// <value>The value of the matrix at the specified coordinate.</value>
        public double this[TVerticalLabel rowLabel, THorizontalLabel columnLabel]
        {
            get
            {
                int rowIndex = Array.BinarySearch<TVerticalLabel>(m_VerticalLabels, 0, m_RowCount, rowLabel);
                if (rowIndex >= 0)
                {
                    int columnIndex = Array.BinarySearch<THorizontalLabel>(m_HorizontalLabels, 0, m_ColumnCount, columnLabel);
                    if (columnIndex >= 0)
                    {
                        return m_Data[rowIndex + m_RowCount * columnIndex];
                    }
                }
                throw new ArgumentOutOfRangeException("Could not find a value for the given (x,y)-labels.");
            }
        }

        /// <summary>Gets a value indicating whether at least one element in the matrix is the result of a specified missing value replenishment.
        /// </summary>
        /// <value>A value indicating whether at least one element in the matrix is the result of a specified missing value replenishment.</value>
        public bool IsReplenished
        {
            get { return (m_ReplenishIndices != null) && (m_ReplenishIndices.Count > 0); }
        }
        #endregion

        #region public methods

        #region IMatrix Members

        /// <summary>Creates a <see cref="DenseMatrix"/> object from the current object.
        /// </summary>
        /// <param name="matrixData">The optional (internal) memory allocation to take into account for the <see cref="DenseMatrix"/> object, i.e. with at
        /// least <see cref="IMatrix.RowCount"/> * <see cref="IMatrix.ColumnCount"/> elements.</param>
        /// <returns>A <see cref="DenseMatrix"/> object that represents the current object.</returns>
        /// <remarks>The argument <paramref name="matrixData"/> can be used to reduce reallocation of memory.</remarks>
        public DenseMatrix ToDenseMatrix(double[] matrixData = null)
        {
            var dataOfNewDenseMatrix = matrixData;

            if ((dataOfNewDenseMatrix == null) || (dataOfNewDenseMatrix.Length < m_RowCount * m_ColumnCount))
            {
                dataOfNewDenseMatrix = new double[m_RowCount * m_ColumnCount];
            }
            BLAS.Level1.dcopy(m_RowCount * m_ColumnCount, m_Data, dataOfNewDenseMatrix);
            return new DenseMatrix(m_RowCount, m_ColumnCount, dataOfNewDenseMatrix);
        }

        /// <summary>Gets a specific column.
        /// </summary>
        /// <param name="columnIndex">The null-based index of the column.</param>
        /// <param name="matrixData">The optional (internal) memory allocation to take into account for the <see cref="DenseMatrix"/> object, i.e. with at
        /// least <see cref="IMatrix.RowCount"/> elements.</param>
        /// <returns>The specified column vector in its <see cref="DenseMatrix"/> representation.</returns>
        /// <remarks>The argument <paramref name="matrixData"/> can be used to reduce reallocation of memory.</remarks>
        public DenseMatrix GetColumn(int columnIndex, double[] matrixData = null)
        {
            var column = matrixData;
            if ((column == null) || (column.Length < m_RowCount))
            {
                column = new double[m_RowCount];
            }
            // set 'column[j] = m_Data[j + m_RowCount * columnIndex]' for j=0,..,m_RowCount-1:
            BLAS.Level1.dcopy(m_RowCount, m_Data, column, 1, 1, m_RowCount * columnIndex, 0);
            return new DenseMatrix(m_RowCount, 1, column);
        }

        /// <summary>Gets a specific transposed row.
        /// </summary>
        /// <param name="rowIndex">The null-based index of the row.</param>
        /// <param name="matrixData">The optional (internal) memory allocation to take into account for the <see cref="DenseMatrix"/> object, i.e. with at
        /// least <see cref="IMatrix.ColumnCount"/> elements.</param>
        /// <returns>The specified transposed row vector in its <see cref="DenseMatrix"/> representation.</returns>
        /// <remarks>The argument <paramref name="matrixData"/> can be used to reduce reallocation of memory.</remarks>
        public DenseMatrix GetTransposedRow(int rowIndex, double[] matrixData = null)
        {
            double[] row = matrixData;
            if ((row == null) || (row.Length < m_ColumnCount))
            {
                row = new double[m_ColumnCount];
            }
            // set 'row[j] = m_Data[rowIndex + m_RowCount * j]' for j =0,...,m_ColumnCount-1:
            BLAS.Level1.dcopy(m_ColumnCount, m_Data, row, m_RowCount, 1, rowIndex, 0);
            return new DenseMatrix(m_ColumnCount, 1, row);
        }

        /// <summary>Gets a specific sub-matrix.
        /// </summary>
        /// <param name="startRowIndex">The null-based index of the upper row.</param>
        /// <param name="endRowIndex">The null-based index of the lower row.</param>
        /// <param name="startColumnIndex">The null-based index of the left column.</param>
        /// <param name="endColumnIndex">The null-based index of the right column.</param>
        /// <param name="matrixData">The optional (internal) memory allocation to take into account for the <see cref="DenseMatrix"/> object, i.e. with at
        /// least (<paramref name="endRowIndex"/> - <paramref name="startRowIndex"/> + 1) * (<paramref name="endColumnIndex"/> - <paramref name="startColumnIndex"/> + 1) elements.</param>
        /// <returns>The specified sub-matrix in its <see cref="DenseMatrix"/> representation.</returns>
        /// <remarks>The argument <paramref name="matrixData"/> can be used to reduce reallocation of memory.</remarks>
        public DenseMatrix GetSubMatrix(int startRowIndex, int endRowIndex, int startColumnIndex, int endColumnIndex, double[] matrixData = null)
        {
            int subColumnCount = endColumnIndex - startColumnIndex + 1;
            int subRowCount = endRowIndex - startRowIndex + 1;

            if ((subRowCount > m_RowCount) || (subColumnCount > m_ColumnCount))
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentCombinationInvalid, "sub-matrix range and given matrix"));
            }
            double[] subMatrix = matrixData;
            if ((subMatrix == null) || (subMatrix.Length < subRowCount * subColumnCount))
            {
                subMatrix = new double[subRowCount * subColumnCount];
            }

            int sourceArrayOffset = startRowIndex + startColumnIndex * m_RowCount;
            for (int i = 0; i < subRowCount; i++)
            {
                BLAS.Level1.dcopy(subColumnCount, m_Data, subMatrix, m_RowCount, subRowCount, i + sourceArrayOffset, i);
            }
            return new DenseMatrix(subRowCount, subColumnCount, subMatrix);
        }

        /// <summary>Determines whether the matrix is symmetric.
        /// </summary>
        /// <param name="tolerance">A tolerance taken into account, i.e. if abs(a_{i,j} - a{j,i}) is less than <paramref name="tolerance"/> the values are assumed to be equal.</param>
        /// <returns><c>true</c> if this instance is symmetric; otherwise, <c>false</c>.</returns>
        public bool IsSymmetric(double tolerance)
        {
            if (m_RowCount != m_ColumnCount)
            {
                return false;
            }
            for (int j = 0; j < m_ColumnCount; j++)
            {
                for (int k = j; k <= m_RowCount; k++)
                {
                    if (Math.Abs(m_Data[k + j * m_RowCount] - m_Data[j + k * m_RowCount]) > tolerance)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>Gets the trace of a quadratic matrix.
        /// </summary>
        /// <returns>The trace of the matrix, i.e. the sum of the diagonal elements.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if the current object does not represent a quadratic matrix.</exception>
        public double GetTrace()
        {
            if (IsQuadratic == false)
            {
                throw new InvalidOperationException();
            }
            double trace = 0.0;
            for (int j = 0; j < m_RowCount; j++)
            {
                trace += m_Data[j + j * m_RowCount];  // = j * (1 + m_RowCount);
            }
            return trace;
        }

        /// <summary>Gets a specific norm of the current matrix object.
        /// </summary>
        /// <param name="normType">The type of the (matrix) norm.</param>
        /// <returns>The norm of the current instance with respect to the <paramref name="normType"/>.</returns>
        public double GetNorm(MatrixNormType normType)
        {
            double[] work = (normType != MatrixNormType.Infinity) ? null : new double[m_RowCount];

            return LAPACK.AuxiliaryRoutines.Matrix.dlange(normType, m_RowCount, m_ColumnCount, m_Data, work);
        }
        #endregion

        #region IInfoOutputQueriable Members

        /// <summary>Gets informations of the current object as a specific <see cref="T:Dodoni.BasicComponents.Containers.InfoOutput"/> instance.
        /// </summary>
        /// <param name="infoOutput">The <see cref="T:Dodoni.BasicComponents.Containers.InfoOutput"/> object which is to be filled with informations concering the current instance.</param>
        /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
        public void FillInfoOutput(InfoOutput infoOutput, string categoryName = "General")
        {
            InfoOutputPackage infoOutputCollection = infoOutput.AcquirePackage(categoryName);
            infoOutputCollection.Add(new InfoOutputProperty("RowCount", m_RowCount), new InfoOutputProperty("ColumnCount", m_ColumnCount));
            var dataTable = new System.Data.DataTable("Data");
            for (int j = 0; j < m_ColumnCount; j++)
            {
                dataTable.Columns.Add(j.ToString(), typeof(double));
            }

            for (int k = 0; k < m_RowCount; k++)
            {
                var row = dataTable.NewRow();
                for (int j = 0; j < m_ColumnCount; j++)
                {
                    row[j] = this[k, j];
                }
                dataTable.Rows.Add(row);
            }
            infoOutputCollection.Add(dataTable);

            var replenishedValueTable = new System.Data.DataTable("ReplenishedValues");
            replenishedValueTable.Columns.Add("RowIndex", typeof(int));
            replenishedValueTable.Columns.Add("ColumnIndex", typeof(int));
            replenishedValueTable.Columns.Add("Value", typeof(double));

            foreach (var replenishedValue in GetReplenishedValues())
            {
                var row = replenishedValueTable.NewRow();
                row[0] = replenishedValue.RowIndex;
                row[1] = replenishedValue.ColumnIndex;
                row[2] = replenishedValue.Value;
                replenishedValueTable.Rows.Add(row);
            }
            infoOutputCollection.Add(replenishedValueTable);
        }

        /// <summary>Sets the <see cref="P:Dodoni.BasicComponents.Containers.IInfoOutputQueriable.InfoOutputDetailLevel"/> property.
        /// </summary>
        /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
        /// <returns>A value indicating whether the <see cref="P:Dodoni.BasicComponents.Containers.IInfoOutputQueriable.InfoOutputDetailLevel"/> has been set to <paramref name="infoOutputDetailLevel"/>.</returns>
        public bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
        {
            return (infoOutputDetailLevel == InfoOutputDetailLevel.Full);
        }
        #endregion

        /// <summary>Gets the null-based index of a specific vertical label.
        /// </summary>
        /// <param name="verticalLabel">The vertical label.</param>
        /// <returns>The null-based row index with respect to <paramref name="verticalLabel"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="verticalLabel"/> does not represents a vertical label.</exception>
        public int GetVerticalLabelIndex(TVerticalLabel verticalLabel)
        {
            int rowIndex = Array.BinarySearch<TVerticalLabel>(m_VerticalLabels, 0, m_RowCount, verticalLabel);
            if (rowIndex >= 0)
            {
                return rowIndex;
            }
            throw new ArgumentOutOfRangeException(nameof(verticalLabel), String.Format(ExceptionMessages.ArgumentIsInvalidForObject, verticalLabel, this.ToString() + "/Vertical labels"));
        }

        /// <summary>Gets the null-based index of a specific vertical label.
        /// </summary>
        /// <param name="verticalLabel">The vertical label.</param>
        /// <param name="value">The null-based row index with respect to <paramref name="verticalLabel"/>.</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public bool TryGetVerticalLabelIndex(TVerticalLabel verticalLabel, out int value)
        {
            value = Array.BinarySearch<TVerticalLabel>(m_VerticalLabels, 0, m_RowCount, verticalLabel);
            return (value >= 0);
        }

        /// <summary>Gets the null-based index of a specific horizontal label.
        /// </summary>
        /// <param name="horizontalLabel">The horizontal label.</param>
        /// <returns>The null-based column index with respect to <paramref name="horizontalLabel"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="horizontalLabel"/> does not represents a horizontal label.</exception>
        public int GetHorizontalLabelIndex(THorizontalLabel horizontalLabel)
        {
            int columnIndex = Array.BinarySearch<THorizontalLabel>(m_HorizontalLabels, 0, m_ColumnCount, horizontalLabel);
            if (columnIndex >= 0)
            {
                return columnIndex;
            }
            throw new ArgumentOutOfRangeException(nameof(horizontalLabel), string.Format(ExceptionMessages.ArgumentIsInvalidForObject, horizontalLabel, this.ToString() + "/Horizontal labels"));
        }

        /// <summary>Gets the null-based index of a specific horizontal label.
        /// </summary>
        /// <param name="horizontalLabel">The horizontal label.</param>
        /// <param name="value">The null-based column index with respect to <paramref name="horizontalLabel"/>.</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public bool TryGetHorizontalLabelIndex(THorizontalLabel horizontalLabel, out int value)
        {
            value = Array.BinarySearch<THorizontalLabel>(m_HorizontalLabels, 0, m_ColumnCount, horizontalLabel);
            return (value >= 0);
        }

        /// <summary>Gets the null-based index of the nearest left horizontal label which is not the last label, i.e. ignoring the last column.
        /// </summary>
        /// <param name="value">The value which should be compared to the <see cref="System.Double"/> representation of the horizontal labels.</param>
        /// <returns>The null-based index of the nearest left horizontal label with respect to <paramref name="value"/>. If <paramref name="value"/> is identical to the 
        /// last horizontal label the number of horizontal labels -2 will returned.</returns>
        public int GetNonLastLeftHorizontalLabelIndex(double value)
        {
            int nonLastLeftGridPointIndex = Array.BinarySearch<double>(m_HorizontalDoubleLabels, 0, m_ColumnCount, value);
            if (nonLastLeftGridPointIndex < 0)
            {
                nonLastLeftGridPointIndex = (~nonLastLeftGridPointIndex) - 1;
            }
            else if (nonLastLeftGridPointIndex == m_ColumnCount)
            {
                nonLastLeftGridPointIndex--;
            }
            return nonLastLeftGridPointIndex;
        }

        /// <summary>Gets the null-based index of the nearest (above) vertical label which is not the last label, i.e. ignoring the last row.
        /// </summary>
        /// <param name="value">The value which should be compared to the <see cref="System.Double"/> representation of the vertical labels.</param>
        /// <returns>The null-based index of the nearest (above) vertical label with respect to <paramref name="value"/>. If <paramref name="value"/> is identical to the 
        /// last vertical label the number of vertical labels -2 will returned.</returns>
        public int GetNonLastAboveVerticalLabelIndex(double value)
        {
            int nonLastLeftGridPointIndex = Array.BinarySearch<double>(m_VerticalDoubleLabels, 0, m_RowCount, value);
            if (nonLastLeftGridPointIndex < 0)
            {
                nonLastLeftGridPointIndex = (~nonLastLeftGridPointIndex) - 1;
            }
            else if (nonLastLeftGridPointIndex == m_RowCount)
            {
                nonLastLeftGridPointIndex--;
            }
            return nonLastLeftGridPointIndex;
        }

        /// <summary>Determines whether the specified point (<paramref name="x"/>,<paramref name="y"/>) is inside the grid point matrix.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns><c>true</c> if the specified point (<paramref name="x"/>,<paramref name="y"/>) is inside the grid point matrix; otherwise, <c>false</c>.</returns>
        public bool IsInside(double x, double y)
        {
            return ((m_ColumnCount > 0) && (x >= m_HorizontalDoubleLabels[0]) && (x <= m_HorizontalDoubleLabels[m_ColumnCount - 1]) && (m_RowCount > 0) && (y >= m_VerticalDoubleLabels[0]) && (y <= m_VerticalDoubleLabels[m_RowCount - 1]));
        }

        /// <summary>Gets a collection of the value in the matrix that have been filled with respect to a specific missing value replenishment.
        /// </summary>
        /// <returns>A collection of null-based row and column index of a values in the matrix that have been filled with respect to a specific missing value replenishment;
        /// the third component of the tuple is the value at the position; or <c>null</c> if no such position exists.</returns>
        public IEnumerable<(int RowIndex, int ColumnIndex, double Value)> GetReplenishedValues()
        {
            if (m_ReplenishIndices != null)
            {
                foreach (var position in m_ReplenishIndices)
                {
                    int rowIndex = position.Key;
                    foreach (var columnIndex in position.Value)
                    {
                        yield return (rowIndex, columnIndex, m_Data[rowIndex + columnIndex * m_RowCount]);
                    }
                }
            }
        }

        /// <summary>Gets a specific row of the matrix and the corresponding labels.
        /// </summary>
        /// <param name="rowIndex">The null-based index of the row.</param>
        /// <returns>A collection of entries of the specified row, where the first item is the horizontal label, the second component is the <see cref="System.Double"/> of the horizontal label and
        /// the third component is the value of the matrix entry. The collection is ordered in a way that the <see cref="System.Double"/> representation of the horizontal labels are in ascending order.</returns>
        public IEnumerable<(THorizontalLabel HorizontalLabel, double HorizontalDoubleLabel, double Value)> GetRowWithLabels(int rowIndex)
        {
            for (int j = 0; j < m_ColumnCount; j++)
            {
                yield return (m_HorizontalLabels[j], m_HorizontalDoubleLabels[j], m_Data[rowIndex + j * m_RowCount]);
            }
        }

        /// <summary>Gets a specific column of the matrix and the corresponding labels.
        /// </summary>
        /// <param name="columnIndex">The null-based index of the column.</param>
        /// <returns>A collection of entries of the specified column, where the first item is the vertical label, the second component is the <see cref="System.Double"/> of the vertical label and
        /// the third component is the value of the matrix entry.</returns>
        public IEnumerable<(TVerticalLabel VerticalLabel, double VerticalDoubleLabel, double Value)> GetColumnWithLabels(int columnIndex)
        {
            for (int j = 0; j < m_RowCount; j++)
            {
                yield return (m_VerticalLabels[j], m_VerticalDoubleLabels[j], m_Data[j + columnIndex * m_RowCount]);
            }
        }

        /// <summary>Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendLine(string.Format("Row count: {0:g}, Column count: {1:g} ", m_RowCount, m_ColumnCount));
            strBuilder.AppendLine("{");
            for (int j = 0; j < m_RowCount; j++)
            {
                strBuilder.Append("[");
                for (int k = 0; k < m_ColumnCount; k++)
                {
                    if ((m_ReplenishIndices != null) && (m_ReplenishIndices.ContainsKey(j) == true) && (m_ReplenishIndices[j].Contains(k) == true))
                    {
                        strBuilder.Append(string.Format("({0:g}) ", m_Data[j + m_RowCount * k]));
                    }
                    else
                    {
                        strBuilder.Append(string.Format("{0:g} ", m_Data[j + m_RowCount * k]));
                    }
                }
                strBuilder.AppendLine("]");
            }
            strBuilder.AppendLine("}");
            return strBuilder.ToString();
        }
        #endregion
    }
}