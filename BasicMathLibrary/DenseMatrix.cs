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
using System.Globalization;

using Dodoni.BasicComponents;
using Dodoni.MathLibrary.Basics;
using Dodoni.BasicComponents.Containers;
using Dodoni.MathLibrary.Basics.LowLevel;

namespace Dodoni.MathLibrary
{
    /// <summary>Represents a dense matrix.
    /// </summary>
    /// <remarks>This class does not implement all BLAS, LAPACK etc. functionality. One has to apply the specified BLAS, Lapack etc. routine to the <see cref="DenseMatrix.Data"/> property.</remarks>
    public partial class DenseMatrix : IMatrix, IInfoOutputQueriable
    {
        #region private members

        /// <summary>The number of rows, i.e. the first dimension. This member is always specified with respect to the non-transposed matrix <see cref="m_Data"/>.
        /// </summary>
        private int m_RowCount;

        /// <summary>The number of columns, i.e. the second dimension. This member is always specified with respect to the non-transposed matrix <see cref="m_Data"/>
        /// </summary>
        private int m_ColumnCount;

        /// <summary>The matrix entries, provided column-by-column. Caution: This member does always represents the non-transposed data.
        /// </summary>
        /// <remarks><see cref="m_RowCount"/> and <see cref="m_ColumnCount"/> are always specified with respect to the non-transposed data.</remarks>
        private double[] m_Data;

        /// <summary>A flag indicating whether one has to apply a specific <see cref="BLAS.MatrixTransposeState"/> in the matrix calculation.  
        /// </summary>
        private BLAS.MatrixTransposeState m_TransposeState;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="DenseMatrix"/> class.
        /// </summary>
        /// <param name="rowCount">The number of rows.</param>
        /// <param name="columnCount">The number of columns.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if one of the argument is less or equal <c>0</c>.</exception>
        public DenseMatrix(int rowCount, int columnCount)
        {
            if (rowCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(rowCount));
            }
            if (columnCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(columnCount));
            }
            m_RowCount = rowCount;
            m_ColumnCount = columnCount;
            m_Data = new double[m_RowCount * m_ColumnCount];
            m_TransposeState = BLAS.MatrixTransposeState.NoTranspose;
        }

        /// <summary>Initializes a new instance of the <see cref="DenseMatrix"/> class.
        /// </summary>
        /// <param name="rowCount">The number of rows.</param>
        /// <param name="columnCount">The number of columns.</param>
        /// <param name="data">The matrix entries provided column-by-column with at least <paramref name="columnCount"/> * <paramref name="rowCount"/> elements.</param>
        /// <param name="createDeepCopyOfArgument">A value indicating whether a deep copy of <paramref name="data"/> will be created and stored in the current object.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="data"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="rowCount"/> or <paramref name="columnCount"/> is less or equal <c>0</c>.</exception>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="data"/> contains less elements than <paramref name="rowCount"/> * <paramref name="columnCount"/>.</exception>
        public DenseMatrix(int rowCount, int columnCount, double[] data, bool createDeepCopyOfArgument = false)
        {
            if (rowCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(rowCount));
            }
            if (columnCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(columnCount));
            }
            m_RowCount = rowCount;
            m_ColumnCount = columnCount;

            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            if (data.Length < rowCount * columnCount)
            {
                throw new ArgumentException(nameof(data));
            }
            if (createDeepCopyOfArgument == false)
            {
                m_Data = data;
            }
            else
            {
                m_Data = new double[rowCount * columnCount];
                BLAS.Level1.dcopy(rowCount * columnCount, data, m_Data);
            }
            m_TransposeState = BLAS.MatrixTransposeState.NoTranspose;
        }

        /// <summary>Initializes a new instance of the <see cref="DenseMatrix"/> class.
        /// </summary>
        /// <param name="data">The dense matrix entries, where the first null-based index corresponds to the row and the second index corresponds to the column.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="data"/> is <c>null</c>.</exception>
        /// <exception cref="IndexOutOfRangeException">Thrown, if <paramref name="data"/> does not represents a matrix.</exception>
        /// <remarks>This constructor creates a deep copy of its argument.</remarks>
        public DenseMatrix(double[][] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            m_RowCount = data.GetLength(0);
            m_ColumnCount = data[0].Length;

            m_Data = new double[m_RowCount * m_ColumnCount];
            for (int j = 0; j < m_RowCount; j++)
            {
                if (data[j].Length != m_ColumnCount)
                {
                    throw new IndexOutOfRangeException(String.Format(ExceptionMessages.ArgumentHasWrongDimension, nameof(data)));
                }
                for (int k = 0; k < m_ColumnCount; k++)
                {
                    m_Data[j + m_RowCount * k] = data[j][k];
                }
            }
            m_TransposeState = BLAS.MatrixTransposeState.NoTranspose;
        }

        /// <summary>Initializes a new instance of the <see cref="DenseMatrix"/> class.
        /// </summary>
        /// <param name="data">The dense matrix entries, where the first null-based index corresponds to the row and the second index corresponds to the column.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="data"/> is <c>null</c>.</exception>
        /// <remarks>This constructor creates a deep copy of its argument.</remarks>
        public DenseMatrix(double[,] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            m_RowCount = data.GetLength(0);
            m_ColumnCount = data.GetLength(1);

            m_Data = new double[m_RowCount * m_ColumnCount];
            for (int j = 0; j < m_RowCount; j++)
            {
                for (int k = 0; k < m_ColumnCount; k++)
                {
                    m_Data[j + m_RowCount * k] = data[j, k];
                }
            }
            m_TransposeState = BLAS.MatrixTransposeState.NoTranspose;
        }
        #endregion        

        #region protected constructors

        /// <summary>Initializes a new instance of the <see cref="DenseMatrix"/> class.
        /// </summary>
        /// <param name="denseMatrix">The dense matrix.</param>
        /// <param name="transposeState">A flag indicating whether <paramref name="denseMatrix"/> should be interpreted as transposed matrix in matrix operations.</param>
        /// <remarks>A shallow copy of the arguments will be stored only.</remarks>
        protected DenseMatrix(DenseMatrix denseMatrix, BLAS.MatrixTransposeState transposeState)
        {
            if (denseMatrix == null)
            {
                throw new ArgumentNullException(nameof(denseMatrix));
            }
            m_TransposeState = transposeState;
            m_RowCount = denseMatrix.m_RowCount;
            m_ColumnCount = denseMatrix.m_ColumnCount;
            m_Data = denseMatrix.m_Data;
        }

        /// <summary>Initializes a new instance of the <see cref="DenseMatrix"/> class.
        /// </summary>
        /// <param name="rowCount">The number of rows.</param>
        /// <param name="columnCount">The number of columns.</param>
        /// <param name="data">The matrix entries provided column-by-column with at least <paramref name="columnCount"/> * <paramref name="rowCount"/> elements.</param>
        /// <param name="transposeState">A flag indicating whether <paramref name="data"/> should be interpreted as transposed matrix in matrix operations.</param>
        /// <param name="createDeepCopyOfArgument">A value indicating whether a deep copy of <paramref name="data"/> will be created and stored in the current object.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="data"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="rowCount"/> or <paramref name="columnCount"/> is less or equal <c>0</c>.</exception>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="data"/> contains less elements than <paramref name="rowCount"/> * <paramref name="columnCount"/>.</exception>
        /// <remarks>A shallow copy of the arguments will be stored only.</remarks>
        protected DenseMatrix(int rowCount, int columnCount, double[] data, BLAS.MatrixTransposeState transposeState, bool createDeepCopyOfArgument = false)
            : this(rowCount, columnCount, data, createDeepCopyOfArgument)
        {
            m_TransposeState = transposeState;
        }
        #endregion

        #region public properties

        #region IMaxtrix Members

        /// <summary>Gets the number of rows of the current matrix object.
        /// </summary>
        /// <value>The number of rows.</value>
        public int RowCount
        {
            get { return (m_TransposeState == BLAS.MatrixTransposeState.NoTranspose) ? m_RowCount : m_ColumnCount; }
        }

        /// <summary>Gets the number of columns of the current matrix object.
        /// </summary>
        /// <value>The number of columns.</value>
        public int ColumnCount
        {
            get { return (m_TransposeState == BLAS.MatrixTransposeState.NoTranspose) ? m_ColumnCount : m_RowCount; }
        }

        /// <summary>Gets a value indicating whether this instance represents a quadratic matrix.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is quadratic; otherwise, <c>false</c>.
        /// </value>
        public bool IsQuadratic
        {
            get { return (m_ColumnCount == m_RowCount); }
        }

        /// <summary>Gets or sets a specified value.
        /// </summary>
        /// <param name="rowIndex">The null-based row index.</param>
        /// <param name="columnIndex">The null-based column index.</param>
        /// <value>The value of the matrix at the specified coordinate.</value>
        /// <exception cref="IndexOutOfRangeException">Thrown, if the row-/column position is invalid.</exception>
        public double this[int rowIndex, int columnIndex]
        {
            get
            {
                if (m_TransposeState == BLAS.MatrixTransposeState.NoTranspose)
                {
                    return m_Data[rowIndex + m_RowCount * columnIndex];
                }
                return m_Data[columnIndex + m_RowCount * rowIndex];
            }
            set
            {
                if (m_TransposeState == BLAS.MatrixTransposeState.NoTranspose)
                {
                    m_Data[rowIndex + m_RowCount * columnIndex] = value;
                }
                else
                {
                    m_Data[columnIndex + m_RowCount * rowIndex] = value;
                }
            }
        }
        #endregion

        #region IInfoOutputQueriable Members

        /// <summary>Gets the info-output level of detail.
        /// </summary>
        /// <value>The info-output level of detail.</value>
        InfoOutputDetailLevel IInfoOutputQueriable.InfoOutputDetailLevel
        {
            get { return InfoOutputDetailLevel.Full; }
        }
        #endregion

        /// <summary>Gets a <b>shallow</b> copy of the current instance that represents the transposed matrix of the current instance.
        /// </summary>
        /// <value>A shallow copy of the current instance that represents the transposed matrix of the current instance.</value>
        public DenseMatrix T
        {
            get { return new DenseMatrix(this, (m_TransposeState == BLAS.MatrixTransposeState.NoTranspose) ? BLAS.MatrixTransposeState.Transpose : BLAS.MatrixTransposeState.NoTranspose); }
        }

        /// <summary>Gets the reference to the internal data, i.e. the matrix entries provided column-by-column. <b>Even if the current instance represents a transposed matrix, 
        /// this array contains the original data.</b> The first <see cref="RowCount"/> * <see cref="ColumnCount"/> are relevant only.
        /// </summary>
        /// <value>The reference to the internal data.</value>
        /// <remarks>Take into account <see cref="DenseMatrix.DataTransposeState"/> in BLAS or LAPACK routines etc.</remarks>
        public double[] Data
        {
            get { return m_Data; }
        }

        /// <summary>Gets a flag indicating whether one has to apply a specific <see cref="BLAS.MatrixTransposeState"/> for matrix operations with respect to <see cref="DenseMatrix.Data"/>.
        /// </summary>
        /// <value>A flag indicating whether one has to apply a specific <see cref="BLAS.MatrixTransposeState"/> for matrix operations with respect to <see cref="DenseMatrix.Data"/>.</value>
        public BLAS.MatrixTransposeState DataTransposeState
        {
            get { return m_TransposeState; }
        }
        #endregion

        #region public methods

        #region IMatrix Members

        /// <summary>Creates a <see cref="DenseMatrix"/> object from the current object.
        /// </summary>
        /// <param name="matrixData">The optional memory allocation to take into account for the <see cref="DenseMatrix"/> object, i.e. with at
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
            switch (m_TransposeState)
            {
                case BLAS.MatrixTransposeState.NoTranspose:
                    BLAS.Level1.dcopy(m_RowCount * m_ColumnCount, m_Data, dataOfNewDenseMatrix);
                    break;

                case BLAS.MatrixTransposeState.Transpose:
                    /* create a deep copy of the current object and reorder the matrix entries with respect to the transposed representation */
                    for (int i = 0; i < m_ColumnCount; i++)
                    {
                        BLAS.Level1.dcopy(m_RowCount, m_Data.AsSpan().Slice(m_RowCount * i), dataOfNewDenseMatrix.AsSpan().Slice(i), 1, m_ColumnCount);  // new[i + m_ColumnCount * j] = old[j + m_RowCount * i] for j = 0,..,m_RowCount-1
                    }
                    break;
                default:
                    throw new InvalidOperationException();
            }
            return new DenseMatrix(RowCount, ColumnCount, dataOfNewDenseMatrix);
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
            int rowCount = RowCount;  // = m_RowCount or m_ColumnCount

            if ((column == null) || (column.Length < rowCount))
            {
                column = new double[rowCount];
            }
            switch (m_TransposeState)
            {
                case BLAS.MatrixTransposeState.NoTranspose:
                    BLAS.Level1.dcopy(rowCount, m_Data.AsSpan().Slice(m_RowCount * columnIndex), column); // column[j] = m_Data[j + m_RowCount(!) * columnIndex] for j=0,..,rowCount-1
                    break;

                case BLAS.MatrixTransposeState.Transpose:
                    BLAS.Level1.dcopy(rowCount, m_Data.AsSpan().Slice(columnIndex), column, m_RowCount, 1); // column[j] = m_Data[columnIndex + m_RowCount * j] for j=0,..,rowCount-1
                    break;

                default:
                    throw new InvalidOperationException();
            }
            return new DenseMatrix(rowCount, 1, column);
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
            var row = matrixData;
            int columnCount = ColumnCount;// = m_ColumnCount or m_RowCount

            if ((row == null) || (row.Length < ColumnCount))
            {
                row = new double[ColumnCount];
            }
            switch (m_TransposeState)
            {
                case BLAS.MatrixTransposeState.NoTranspose:
                    BLAS.Level1.dcopy(columnCount, m_Data.AsSpan().Slice(rowIndex), row, m_RowCount, 1);  // row[j] = m_Data[rowIndex + m_RowCount * j] for j =0,...,columnCount-1
                    break;

                case BLAS.MatrixTransposeState.Transpose:
                    BLAS.Level1.dcopy(columnCount, m_Data.AsSpan().Slice(m_RowCount * rowIndex), row); // row[j] = m_Data[j + m_RowCount * rowIndex] for j =0,...,columnCount-1
                    break;

                default:
                    throw new InvalidOperationException();
            }
            return new DenseMatrix(columnCount, 1, row);
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

            if ((subRowCount > RowCount) || (subColumnCount > ColumnCount))
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentCombinationInvalid, "sub-matrix range and given matrix"));
            }
            double[] subMatrix = matrixData;
            if ((subMatrix == null) || (subMatrix.Length < subRowCount * subColumnCount))
            {
                subMatrix = new double[subRowCount * subColumnCount];
            }

            if (m_TransposeState == BLAS.MatrixTransposeState.NoTranspose)
            {
                //  subMatrix[rowIndex + subRowCount * columnIndex] = m_Data[offset + rowIndex + m_RowCount * columnIndex]
                int sourceArrayOffset = startRowIndex + startColumnIndex * m_RowCount;
                for (int i = 0; i < subRowCount; i++)
                {
                    BLAS.Level1.dcopy(subColumnCount, m_Data.AsSpan().Slice(i + sourceArrayOffset), subMatrix.AsSpan().Slice(i), m_RowCount, subRowCount);
                }
            }
            else
            {
                //  subMatrix[rowIndex + subRowCount * columnIndex] = m_Data[offset + columnIndex + m_RowCount * rowIndex]
                int sourceArrayOffset = startColumnIndex + startRowIndex * m_RowCount;
                for (int j = 0; j < subColumnCount; j++)
                {
                    BLAS.Level1.dcopy(subColumnCount, m_Data.AsSpan().Slice(j + sourceArrayOffset), subMatrix.AsSpan().Slice(j * subRowCount), m_RowCount, 1);
                }
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
                    if (System.Math.Abs(m_Data[k + j * m_RowCount] - m_Data[j + k * m_RowCount]) > tolerance)
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
        public double GetNorm(MatrixNormType normType = MatrixNormType.Frobenius)
        {
            /* in the case of a transposed matrix: all norms are identical except 1- and Infinity matrix norm which have to be replaced by each other: */
            MatrixNormType adjNormType = normType;

            if (m_TransposeState == BLAS.MatrixTransposeState.Transpose)
            {
                if (normType == MatrixNormType.Infinity)
                {
                    adjNormType = MatrixNormType.OneNorm;
                }
                else if (normType == MatrixNormType.OneNorm)
                {
                    adjNormType = MatrixNormType.Infinity;
                }
            }
            double[] work = (adjNormType != MatrixNormType.Infinity) ? null : new double[m_RowCount];

            return LAPACK.AuxiliaryRoutines.Matrix.dlange(adjNormType, m_RowCount, m_ColumnCount, m_Data, work);
        }
        #endregion

        #region IInfoOutputQueriable Members

        /// <summary>Gets informations of the current object as a specific <see cref="InfoOutput"/> instance.
        /// </summary>
        /// <param name="infoOutput">The <see cref="InfoOutput"/> object which is to be filled with informations concering the current instance.</param>
        /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
        void IInfoOutputQueriable.FillInfoOutput(InfoOutput infoOutput, string categoryName)
        {
            InfoOutputPackage infoOutputCollection = infoOutput.AcquirePackage(categoryName);
            infoOutputCollection.Add(new InfoOutputProperty("RowCount", RowCount),
                                     new InfoOutputProperty("ColumnCount", ColumnCount));

            var dataTable = new System.Data.DataTable("Data");
            for (int j = 0; j < ColumnCount; j++)
            {
                dataTable.Columns.Add(j.ToString(), typeof(double));
            }

            for (int k = 0; k < RowCount; k++)
            {
                var row = dataTable.NewRow();
                for (int j = 0; j < ColumnCount; j++)
                {
                    row[j] = this[k, j];
                }
                dataTable.Rows.Add(row);
            }
            infoOutputCollection.Add(dataTable);
        }

        /// <summary>Sets the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel"/> property.
        /// </summary>
        /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
        /// <returns>A value indicating whether the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel"/> has been set to <paramref name="infoOutputDetailLevel"/>.</returns>
        bool IInfoOutputQueriable.TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
        {
            return (infoOutputDetailLevel == InfoOutputDetailLevel.Full);
        }
        #endregion

        /// <summary>Apply a in-place transposition of the matrix represented by the current object.
        /// </summary>
        /// <param name="work">A workspace array. Apply <c>BLAS.Level3.aux_dgetransQuery</c> for optimal workspace length.</param>
        /// <returns>Returns a reference to the current object.</returns>
        public DenseMatrix T_InPlace(double[] work = null)
        {
            BLAS.Level3.aux_dgetrans(m_RowCount, m_ColumnCount, m_Data, work);
            int temp = m_RowCount;
            m_RowCount = m_ColumnCount;
            m_ColumnCount = temp;

            m_TransposeState = BLAS.MatrixTransposeState.NoTranspose;
            return this;
        }

        /// <summary>Gets a singular value decomposition, i.e. A = U * \Sigma * V', where the diagonal elements of '\Sigma' are the singular values in descending order.
        /// </summary>
        /// <param name="u">The left singular vectors 'U', i.e. a 'm x m' unitary matrix (output).</param>
        /// <param name="vt">The right singular vectors 'V^t', i.e. a 'n x n' unitary matrix (output).</param>
        /// <returns>The singular values in descending order.</returns>
        /// <remarks>The singular values are the roots of the non-negative eigenvalues of A^t*A.
        /// <para>This method changes the current instance</para></remarks>
        public double[] GetSingularValueDecomposition(out DenseMatrix u, out DenseMatrix vt)
        {
            var uArray = new double[m_RowCount * m_RowCount];
            var vtArray = new double[m_ColumnCount * m_ColumnCount];
            var s = new double[System.Math.Min(m_RowCount, m_ColumnCount)];
            var lwork = LAPACK.EigenValues.SingularValueDecomposition.driver_dgesvdQuery(m_RowCount, m_ColumnCount, LapackEigenvalues.SVDleftSingularVectorsJob.All, LapackEigenvalues.SVDrightSingularVectorsJob.All);
            var work = new double[lwork];
            LAPACK.EigenValues.SingularValueDecomposition.driver_dgesvd(m_RowCount, m_ColumnCount, m_Data, s, uArray, vtArray, work, LapackEigenvalues.SVDleftSingularVectorsJob.All, LapackEigenvalues.SVDrightSingularVectorsJob.All);

            if (m_TransposeState == BLAS.MatrixTransposeState.NoTranspose)
            {
                u = new DenseMatrix(m_RowCount, m_RowCount, uArray);
                vt = new DenseMatrix(m_ColumnCount, m_ColumnCount, vtArray);
            }
            else  // switch U and V', because A' = V * \Sigma * U'
            {
                vt = new DenseMatrix(m_RowCount, m_RowCount, uArray);
                u = new DenseMatrix(m_ColumnCount, m_ColumnCount, vtArray);
            }
            return s;
        }

        /// <summary>Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.</returns>
        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendLine(string.Format("Row count: {0:g}, Column count: {1:g} ", RowCount, ColumnCount));
            strBuilder.AppendLine("{");
            for (int j = 0; j < RowCount; j++)
            {
                strBuilder.Append("[");
                for (int k = 0; k < ColumnCount; k++)
                {
                    strBuilder.Append(String.Format("{0:g} ", this[j, k]));
                }
                strBuilder.AppendLine("]");
            }
            strBuilder.AppendLine("}");
            return strBuilder.ToString();
        }
        #endregion
    }
}