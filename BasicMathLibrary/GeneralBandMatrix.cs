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
    /// <summary>Represents a general band matrix.
    /// </summary>
    /// <remarks>The matrix entries are provided column-by-column, where <b>each</b> column contains exactly m elements, where m is the 
    /// number of sub-diagonals plus the number of super diagonals plus  one (for the diagonal element) - the general band matrix storage.</remarks>
    public partial class GeneralBandMatrix : IMatrix, IInfoOutputQueriable
    {
        #region private members

        /// <summary>The number of rows, i.e. the first dimension. This member is always specified with respect to the non-transposed matrix <see cref="m_Data"/>.
        /// </summary>
        private int m_RowCount;

        /// <summary>The number of columns, i.e. the second dimension. This member is always specified with respect to the non-transposed matrix <see cref="m_Data"/>.
        /// </summary>
        private int m_ColumnCount;

        /// <summary>The matrix entries, provided column-by-column. Caution: This member does always represents the non-transposed data.
        /// </summary>
        /// <remarks>All private members are always specified with respect to the non-transposed data!</remarks>
        private double[] m_Data;

        /// <summary>The number of sub-diagonals of the matrix. This member is always specified with respect to the non-transposed matrix <see cref="m_Data"/>.
        /// </summary>
        private int m_SubDiagonalCount;

        /// <summary>The number of super-diagonals of the matrix. This member is always specified with respect to the non-transposed matrix <see cref="m_Data"/>.
        /// </summary>
        private int m_SuperDiagonalCount;

        /// <summary>A flag indicating whether one has to apply a specific <see cref="BLAS.MatrixTransposeState"/> in the matrix calculation.  
        /// </summary>
        private BLAS.MatrixTransposeState m_TransposeState;
        #endregion

        #region private constructors

        /// <summary>Initializes a new instance of the <see cref="GeneralBandMatrix"/> class.
        /// </summary>
        /// <param name="rowCount">The number of rows.</param>
        /// <param name="columnCount">The number of columns.</param>
        /// <param name="subDiagonalCount">The number of sub-diagonal elements.</param>
        /// <param name="superDiagonalCount">The number of super-diagonal elements.</param>
        /// <param name="transposeState">A value indicating whether die general band matrix represented by the current instance should be interpreted as a transposed matrix.</param>
        private GeneralBandMatrix(int rowCount, int columnCount, int subDiagonalCount, int superDiagonalCount, BLAS.MatrixTransposeState transposeState)
        {
            if (rowCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(rowCount));
            }
            m_RowCount = rowCount;

            if (columnCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(columnCount));
            }
            m_ColumnCount = columnCount;

            if (subDiagonalCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(subDiagonalCount));
            }
            m_SubDiagonalCount = subDiagonalCount;

            if (superDiagonalCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(superDiagonalCount));
            }
            m_SuperDiagonalCount = superDiagonalCount;
            m_TransposeState = transposeState;
        }
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="GeneralBandMatrix"/> class.
        /// </summary>
        /// <param name="rowCount">The number of rows.</param>
        /// <param name="columnCount">The number of columns.</param>
        /// <param name="subDiagonalCount">The number of sub-diagonals of the matrix.</param>
        /// <param name="superDiagonalCount">The number of super-diagonals of the matrix.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="rowCount"/> or <paramref name="columnCount"/> is less or 
        /// equal <c>0</c> or <paramref name="subDiagonalCount"/> or <paramref name="superDiagonalCount"/> is less than <c>0</c>.</exception>
        public GeneralBandMatrix(int rowCount, int columnCount, int subDiagonalCount, int superDiagonalCount)
            : this(rowCount, columnCount, subDiagonalCount, superDiagonalCount, BLAS.MatrixTransposeState.NoTranspose)
        {
            m_Data = new double[DataLength];
        }

        /// <summary>Initializes a new instance of the <see cref="GeneralBandMatrix"/> class.
        /// </summary>
        /// <param name="rowCount">The number of rows.</param>
        /// <param name="columnCount">The number of columns.</param>
        /// <param name="subDiagonalCount">The number of sub-diagonals of the matrix.</param>
        /// <param name="superDiagonalCount">The number of super-diagonals of the matrix.</param>
        /// <param name="data">The general band matrix provided column-by-column, where <b>each</b> column contains exactly <paramref name="subDiagonalCount"/> + <paramref name="superDiagonalCount"/> + 1 elements (general band matrix storage).</param>
        /// <param name="createDeepCopyOfArgument">A value indicating whether a deep copy of <paramref name="data"/> will be created and stored in the current object.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="data"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="rowCount"/> or <paramref name="columnCount"/> is less or equal <c>0</c> or <paramref name="subDiagonalCount"/> or <paramref name="superDiagonalCount"/> is less than <c>0</c>.</exception>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="data"/> contains less elements than <paramref name="columnCount"/> * ( <paramref name="subDiagonalCount"/> + <paramref name="superDiagonalCount"/> + 1) elements.</exception>
        public GeneralBandMatrix(int rowCount, int columnCount, int subDiagonalCount, int superDiagonalCount, double[] data, bool createDeepCopyOfArgument = false)
            : this(rowCount, columnCount, subDiagonalCount, superDiagonalCount, BLAS.MatrixTransposeState.NoTranspose)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            if (data.Length < DataLength)
            {
                throw new ArgumentException(nameof(data));
            }
            if (createDeepCopyOfArgument == false)
            {
                m_Data = data;
            }
            else
            {
                m_Data = new double[DataLength];
                BLAS.Level1.dcopy(DataLength, data, m_Data);
            }
        }

        /// <summary>Initializes a new instance of the <see cref="GeneralBandMatrix"/> class.
        /// </summary>
        /// <param name="subDiagonalCount">The number of sub-diagonals of the matrix.</param>
        /// <param name="superDiagonalCount">The number of super-diagonals of the matrix.</param>
        /// <param name="denseBandMatrixData">The dense band matrix, where the first null-based index corresponds to the row, the second to the column index.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="denseBandMatrixData"/> is <c>null</c>.</exception>
        /// <exception cref="IndexOutOfRangeException">Thrown, if <paramref name="denseBandMatrixData"/> does not represents a dense band matrix.</exception>
        /// <remarks>This constructor creates a deep copy of its argument.</remarks>
        public GeneralBandMatrix(int subDiagonalCount, int superDiagonalCount, double[][] denseBandMatrixData)
            : this(denseBandMatrixData.GetLength(0), denseBandMatrixData[0].Length, subDiagonalCount, superDiagonalCount, BLAS.MatrixTransposeState.NoTranspose)
        {
            m_Data = new double[DataLength];
            BLAS.MatrixStorageConversion.CreateGeneralBandMatrix(denseBandMatrixData, m_RowCount, m_ColumnCount, m_SubDiagonalCount, m_SuperDiagonalCount, m_Data);
        }

        /// <summary>Initializes a new instance of the <see cref="GeneralBandMatrix"/> class.
        /// </summary>
        /// <param name="subDiagonalCount">The number of sub-diagonals of the matrix.</param>
        /// <param name="superDiagonalCount">The number of super-diagonals of the matrix.</param>
        /// <param name="denseBandMatrixData">The dense band matrix, where the first null-based index corresponds to the row, the second to the column index.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="denseBandMatrixData"/> is <c>null</c>.</exception>
        /// <exception cref="IndexOutOfRangeException">Thrown, if <paramref name="denseBandMatrixData"/> does not represents a dense band matrix.</exception>
        /// <remarks>This constructor creates a deep copy of its argument.</remarks>
        public GeneralBandMatrix(int subDiagonalCount, int superDiagonalCount, double[,] denseBandMatrixData)
            : this(denseBandMatrixData.GetLength(0), denseBandMatrixData.GetLength(1), subDiagonalCount, superDiagonalCount, BLAS.MatrixTransposeState.NoTranspose)
        {
            m_Data = new double[DataLength];
            BLAS.MatrixStorageConversion.CreateGeneralBandMatrix(denseBandMatrixData, m_RowCount, m_ColumnCount, m_SubDiagonalCount, m_SuperDiagonalCount, m_Data);
        }
        #endregion

        #region protected constructors

        /// <summary>Initializes a new instance of the <see cref="GeneralBandMatrix"/> class.
        /// </summary>
        /// <param name="generalBandMatrix">The general band matrix.</param>
        /// <param name="transposeState">A flag indicating whether <paramref name="generalBandMatrix"/> should be interpreted as transposed matrix in matrix operations.</param>
        /// <remarks>A shallow copy of the arguments will be stored only.</remarks>
        protected GeneralBandMatrix(GeneralBandMatrix generalBandMatrix, BLAS.MatrixTransposeState transposeState)
        {
            m_SubDiagonalCount = generalBandMatrix.m_SubDiagonalCount;
            m_SuperDiagonalCount = generalBandMatrix.m_SuperDiagonalCount;
            m_ColumnCount = generalBandMatrix.m_ColumnCount;
            m_RowCount = generalBandMatrix.m_RowCount;
            m_TransposeState = transposeState;

            m_Data = generalBandMatrix.m_Data;
        }

        /// <summary>Initializes a new instance of the <see cref="GeneralBandMatrix"/> class.
        /// </summary>
        /// <param name="rowCount">The number of rows.</param>
        /// <param name="columnCount">The number of columns.</param>
        /// <param name="subDiagonalCount">The number of sub-diagonals of the matrix.</param>
        /// <param name="superDiagonalCount">The number of super-diagonals of the matrix.</param>
        /// <param name="data">The general band matrix provided column-by-column, where <b>each</b> column contains exactly <paramref name="subDiagonalCount"/> + <paramref name="superDiagonalCount"/> + 1 elements (general band matrix storage).</param>
        /// <param name="transposeState">A flag indicating whether <paramref name="data"/> should be interpreted as transposed matrix in matrix operations.</param>
        /// <param name="createDeepCopyOfArgument">A value indicating whether a deep copy of <paramref name="data"/> will be created and stored in the current object.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="data"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="rowCount"/> or <paramref name="columnCount"/> is less or equal <c>0</c> or <paramref name="subDiagonalCount"/> or <paramref name="superDiagonalCount"/> is less than <c>0</c>.</exception>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="data"/> contains less elements than <paramref name="columnCount"/> * ( <paramref name="subDiagonalCount"/> + <paramref name="superDiagonalCount"/> + 1) elements.</exception>
        protected GeneralBandMatrix(int rowCount, int columnCount, int subDiagonalCount, int superDiagonalCount, double[] data, BLAS.MatrixTransposeState transposeState, bool createDeepCopyOfArgument = false)
            : this(rowCount, columnCount, subDiagonalCount, superDiagonalCount, transposeState)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            if (data.Length < DataLength)
            {
                throw new ArgumentException(nameof(data));
            }
            if (createDeepCopyOfArgument == false)
            {
                m_Data = data;
            }
            else
            {
                m_Data = new double[DataLength];
                BLAS.Level1.dcopy(DataLength, data, m_Data);
            }
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
        /// <value><c>true</c> if this instance is quadratic; otherwise, <c>false</c>.</value>
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
                if ((rowIndex > columnIndex + SubDiagonalCount) || (columnIndex > rowIndex + SuperDiagonalCount))  // i-j > kl or j-i > ku
                {
                    return 0.0;
                }

                if (m_TransposeState == BLAS.MatrixTransposeState.NoTranspose)
                {
                    return m_Data[rowIndex - columnIndex + m_SuperDiagonalCount + columnIndex * (m_SubDiagonalCount + m_SuperDiagonalCount + 1)];
                }
                else if (m_TransposeState == BLAS.MatrixTransposeState.Transpose)
                {
                    return m_Data[columnIndex - rowIndex + m_SuperDiagonalCount + rowIndex * (m_SubDiagonalCount + m_SuperDiagonalCount + 1)];
                }
                throw new InvalidOperationException();
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

        /// <summary>Gets the number of sub-diagonals of the current <see cref="GeneralBandMatrix"/> object.
        /// </summary>
        public int SubDiagonalCount
        {
            get { return (m_TransposeState == BLAS.MatrixTransposeState.NoTranspose) ? m_SubDiagonalCount : m_SuperDiagonalCount; }
        }

        /// <summary>The number of super-diagonals of the current <see cref="GeneralBandMatrix"/> object.
        /// </summary>
        public int SuperDiagonalCount
        {
            get { return (m_TransposeState == BLAS.MatrixTransposeState.NoTranspose) ? m_SuperDiagonalCount : m_SubDiagonalCount; }
        }

        /// <summary>Gets a <b>shallow</b> copy of the current instance that represents the transposed matrix of the current instance.
        /// </summary>
        /// <value>A shallow copy of the current instance that represents the transposed matrix of the current instance.</value>
        public GeneralBandMatrix T
        {
            get { return new GeneralBandMatrix(this, (m_TransposeState == BLAS.MatrixTransposeState.NoTranspose) ? BLAS.MatrixTransposeState.Transpose : BLAS.MatrixTransposeState.NoTranspose); }
        }

        /// <summary>Gets the reference to the internal data, i.e. the matrix entries in general matrix storage, i.e. <b>each</b> column contains
        /// exactly <see cref="SuperDiagonalCount"/> + <see cref="SubDiagonalCount"/> + 1 elements. <b>Even if the current instance represents a transposed matrix, 
        /// this array contains the original data.</b>
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

        /// <summary>Gets the relevant number of elements of <see cref="Data"/>
        /// </summary>
        /// <value>The number of relevant elements in <see cref="Data"/>.</value>
        public int DataLength
        {
            get { return m_ColumnCount * (1 + m_SubDiagonalCount + m_SuperDiagonalCount); }
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
            int subPlusSuperPlusMainDiagonalCount = m_SubDiagonalCount + m_SuperDiagonalCount + 1;

            int denseMatrixIndex = 0;

            if (m_TransposeState == BLAS.MatrixTransposeState.NoTranspose)
            {
                for (int j = 0; j < m_ColumnCount; j++)
                {
                    int iMin = j - m_SuperDiagonalCount;
                    int iMax = j + m_SubDiagonalCount;

                    for (int i = 0; i < m_RowCount; i++)
                    {
                        if ((i > iMax) || (i < iMin))  // i-j > kl or j-i > ku
                        {
                            dataOfNewDenseMatrix[denseMatrixIndex++] = 0.0;
                        }
                        else
                        {
                            dataOfNewDenseMatrix[denseMatrixIndex++] = m_Data[i - j + m_SuperDiagonalCount + j * subPlusSuperPlusMainDiagonalCount];
                        }
                    }
                }
            }
            else
            {
                for (int j = 0; j < m_RowCount; j++)
                {
                    int iMin = j - m_SubDiagonalCount;
                    int iMax = j + m_SuperDiagonalCount;

                    for (int i = 0; i < m_ColumnCount; i++)
                    {
                        if ((i > iMax) || (i < iMin))  // i-j > kl or j-i > ku
                        {
                            dataOfNewDenseMatrix[denseMatrixIndex++] = 0.0;
                        }
                        else
                        {
                            dataOfNewDenseMatrix[denseMatrixIndex++] = m_Data[j - i + m_SuperDiagonalCount + i * subPlusSuperPlusMainDiagonalCount];
                        }
                    }
                }
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
            int rowCount = RowCount;

            double[] column = matrixData;
            if ((column == null) || (column.Length < rowCount))
            {
                column = new double[rowCount];
            }

            int iMin = columnIndex - SuperDiagonalCount;
            int iMax = columnIndex + SubDiagonalCount;
            int iCount = Math.Min(iMax + 1, RowCount) - Math.Max(iMin, 0);  // number of relevant non-zero entries in current column 
            int k = m_SubDiagonalCount + m_SuperDiagonalCount + 1;

            BLAS.Level1.dscal(Math.Max(0, iMin), 0.0, column);
            BLAS.Level1.dscal(RowCount - iCount - Math.Max(0, iMin), 0.0, column.AsSpan().Slice(Math.Max(0, iMin) + iCount));

            switch (m_TransposeState)
            {
                case BLAS.MatrixTransposeState.NoTranspose:
                    BLAS.Level1.dcopy(iCount, m_Data.AsSpan().Slice(Math.Max(0, iMin) - columnIndex + m_SuperDiagonalCount + columnIndex * k), column.AsSpan().Slice(Math.Max(0, iMin)));
                    break;

                case BLAS.MatrixTransposeState.Transpose:
                    BLAS.Level1.dcopy(iCount, m_Data.AsSpan().Slice(columnIndex + m_SuperDiagonalCount + Math.Max(0, iMin) * (k - 1)), column.AsSpan().Slice(Math.Max(0, iMin)), k - 1, 1);
                    break;

                default: throw new InvalidOperationException();
            }
            return new DenseMatrix(RowCount, 1, column);
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
            int columnCount = ColumnCount;
            int k = m_SuperDiagonalCount + m_SubDiagonalCount + 1;

            if ((row == null) || (row.Length < columnCount))
            {
                row = new double[columnCount];
            }
            BLAS.Level1.dscal(Math.Max(0, rowIndex - SubDiagonalCount), 0.0, row);
            BLAS.Level1.dscal(Math.Max(0, columnCount - rowIndex - SuperDiagonalCount - 1), 0.0, row.AsSpan().Slice(rowIndex + SuperDiagonalCount + 1));

            switch (m_TransposeState)
            {
                case BLAS.MatrixTransposeState.NoTranspose:
                    BLAS.Level1.dcopy(columnCount - Math.Max(0, rowIndex - SubDiagonalCount) - Math.Max(0, columnCount - rowIndex - SuperDiagonalCount - 1), m_Data.AsSpan().Slice(rowIndex + SuperDiagonalCount + Math.Max(0, rowIndex - SubDiagonalCount) * (k - 1)), row.AsSpan().Slice(Math.Max(0, rowIndex - SubDiagonalCount)), k - 1, 1);
                    break;

                case BLAS.MatrixTransposeState.Transpose:
                    BLAS.Level1.dcopy(columnCount - Math.Max(0, rowIndex - SubDiagonalCount) - Math.Max(0, columnCount - rowIndex - SuperDiagonalCount - 1), m_Data.AsSpan().Slice(Math.Max(0, rowIndex - SubDiagonalCount) - rowIndex + m_SuperDiagonalCount + rowIndex * k), row.AsSpan().Slice(Math.Max(0, rowIndex - SubDiagonalCount)));
                    break;

                default: throw new InvalidOperationException();
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

            int index = 0;
            for (int j = startColumnIndex; j <= endColumnIndex; j++)  // can be improved, if desired
            {
                for (int i = startRowIndex; i <= endRowIndex; i++)
                {
                    subMatrix[index++] = this[i, j];
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
            if (IsQuadratic == false)
            {
                return false;
            }

            int subPlusSuperPlusMainDiagonalCount = m_SubDiagonalCount + m_SuperDiagonalCount + 1;
            for (int j = 0; j < m_ColumnCount; j++)
            {
                for (int i = Math.Max(0, j - m_SuperDiagonalCount); i < Math.Min(j + m_SubDiagonalCount, m_RowCount); j++)
                {
                    int index1 = i - j + m_SuperDiagonalCount + j * subPlusSuperPlusMainDiagonalCount;
                    int index2 = j - i + m_SuperDiagonalCount + i * subPlusSuperPlusMainDiagonalCount;

                    // ignore the entries which are not needed for the matrix representation:
                    if ((index1 >= m_SuperDiagonalCount) && Math.Abs(m_Data[index1] - m_Data[index2]) > tolerance)
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
                trace += m_Data[m_SuperDiagonalCount + j * (m_SubDiagonalCount + m_SuperDiagonalCount + 1)];
            }
            return trace;
        }

        /// <summary>Gets a specific norm of the current matrix object.
        /// </summary>
        /// <param name="normType">The type of the (matrix) norm.</param>
        /// <returns>The norm of the current instance with respect to the <paramref name="normType"/>.</returns>
        public double GetNorm(MatrixNormType normType)
        {
            if (IsQuadratic == true)  // apply LAPACK function if available
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
                return LAPACK.AuxiliaryRoutines.Matrix.dlangb(adjNormType, m_RowCount, m_SubDiagonalCount, m_SuperDiagonalCount, m_Data, work);
            }

            var norm = 0.0;
            int k = m_SubDiagonalCount + m_SuperDiagonalCount + 1;
            switch (normType)
            {
                case MatrixNormType.Frobenius:
                    NormLoop(x => { norm += x * x; });
                    return Math.Sqrt(norm);

                case MatrixNormType.LargestAbsoluteValue:
                    norm = Double.MinValue;
                    NormLoop(x => { norm = ((Math.Abs(x) > norm) ? Math.Abs(x) : norm); });
                    return norm;

                case MatrixNormType.Infinity:  // max. row sum
                    norm = Double.MinValue;

                    if (m_TransposeState == BLAS.MatrixTransposeState.NoTranspose)
                    {
                        for (int i = 0; i < m_RowCount; i++)
                        {
                            double temp = 0.0;
                            for (int j = Math.Max(0, i - SubDiagonalCount); j <= Math.Min(m_ColumnCount - 1, i + SuperDiagonalCount); j++)
                            {
                                temp += Math.Abs(m_Data[i - j + m_SuperDiagonalCount + j * k]);
                            }
                            norm = (temp > norm) ? temp : norm;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < m_ColumnCount; i++)
                        {
                            double temp = 0.0;
                            for (int j = Math.Max(0, i - SubDiagonalCount); j <= Math.Min(m_RowCount - 1, i + SuperDiagonalCount); j++)
                            {
                                temp += Math.Abs(m_Data[j - i + m_SuperDiagonalCount + i * k]);
                            }
                            norm = (temp > norm) ? temp : norm;
                        }
                    }
                    return norm;

                case MatrixNormType.OneNorm: // max. column sum
                    if (m_TransposeState == BLAS.MatrixTransposeState.NoTranspose)
                    {
                        for (int j = 0; j < m_ColumnCount; j++)
                        {
                            int iMin = j - m_SuperDiagonalCount;
                            int iMax = j + m_SubDiagonalCount;

                            var temp = 0.0;
                            for (int i = Math.Max(0, iMin); i < Math.Min(m_RowCount, iMax); i++)
                            {
                                temp += Math.Abs(m_Data[i - j + m_SuperDiagonalCount + j * k]);
                            }
                            norm = (temp > norm) ? temp : norm;
                        }
                    }
                    else
                    {
                        for (int j = 0; j < m_RowCount; j++)
                        {
                            int iMin = j - m_SubDiagonalCount;
                            int iMax = j + m_SuperDiagonalCount;

                            var temp = 0.0;
                            for (int i = Math.Max(0, iMin); i < Math.Min(m_ColumnCount, iMax); i++)
                            {
                                temp += Math.Abs(m_Data[j - i + m_SuperDiagonalCount + i * k]);
                            }
                            norm = (temp > norm) ? temp : norm;
                        }
                    }
                    return norm;

                default: throw new NotImplementedException();
            }
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
                                     new InfoOutputProperty("ColumnCount", ColumnCount),
                                     new InfoOutputProperty("SubDiagonalCount", SubDiagonalCount),
                                     new InfoOutputProperty("SuperDiagonalCount", SuperDiagonalCount));

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

        /// <summary>Gets a singular value decomposition, i.e. 'A = U * \Sigma *V^t', where the diagonal elements of '\Sigma' are the singular values in descending order.
        /// </summary>
        /// <param name="u">The left singular vectors 'U', i.e. a 'm x m' unitary matrix.</param>
        /// <param name="vt">The right singular vectors 'V^t', i.e. a 'n x n' unitary matrix.</param>
        /// <returns>The singular values in descending order.</returns>
        /// <remarks>The singular values are the roots of the non-negative eigenvalues of A^t*A.
        /// <para>This method changes the current instance!</para></remarks>
        public double[] GetSingularValueDecomposition(out DenseMatrix u, out DenseMatrix vt)
        {
            var uArray = new double[m_RowCount * m_RowCount];
            var vtArray = new double[m_ColumnCount * m_ColumnCount];
            var s = new double[Math.Min(m_RowCount, m_ColumnCount)];

            LAPACK.EigenValues.SingularValueDecomposition.dgbsvd(true, m_RowCount, m_ColumnCount, m_SubDiagonalCount, m_SuperDiagonalCount, m_Data, s, uArray, vtArray);

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

        /// <summary>Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            var strBuilder = new StringBuilder();
            strBuilder.AppendLine(String.Format("Row count: {0:g}, Column count: {1:g}, sub-diagonal count: {2:g}, super-diagonal count: {3:g} ", RowCount, ColumnCount, SubDiagonalCount, SuperDiagonalCount));
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

        #region private methods

        /// <summary>A method used for norm caluclation.
        /// </summary>
        /// <param name="action">A delegate applied in each step, where the argument is the value of the specific matrix entry.</param>
        private void NormLoop(Action<double> action)
        {
            int k = m_SubDiagonalCount + m_SuperDiagonalCount + 1;

            if (m_TransposeState == BLAS.MatrixTransposeState.NoTranspose)
            {
                for (int j = 0; j < m_ColumnCount; j++)
                {
                    int iMin = j - m_SuperDiagonalCount;
                    int iMax = j + m_SubDiagonalCount;

                    for (int i = Math.Max(0, iMin); i < Math.Min(m_RowCount, iMax); i++)
                    {
                        action(m_Data[i - j + m_SuperDiagonalCount + j * k]);
                    }
                }
            }
            else
            {
                for (int j = 0; j < m_RowCount; j++)
                {
                    int iMin = j - m_SubDiagonalCount;
                    int iMax = j + m_SuperDiagonalCount;

                    for (int i = Math.Max(0, iMin); i < Math.Min(m_ColumnCount, iMax); i++)
                    {
                        action(m_Data[j - i + m_SuperDiagonalCount + i * k]);
                    }
                }
            }
        }
        #endregion
    }
}