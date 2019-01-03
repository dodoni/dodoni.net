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

namespace Dodoni.MathLibrary
{
    /// <summary>Represents a diagonal matrix.
    /// </summary>
    public partial class DiagonalMatrix : IMatrix, IInfoOutputQueriable
    {
        #region public (readonly) members

        /// <summary>The diagonal entries of the matrix.
        /// </summary>
        /// <remarks>This member can be used to apply BLAS or LAPACK functions.</remarks>
        public readonly double[] Data;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="DiagonalMatrix" /> class.
        /// </summary>
        /// <param name="n">The dimension of the diagonal matrix.</param>
        /// <param name="data">The entries of the diagonal matrix, i.e. the diagonal entries.</param>
        public DiagonalMatrix(int n, double[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            if (data.Length < n)
            {
                throw new ArgumentOutOfRangeException(nameof(data));
            }
            Dimension = n;
            Data = data;
        }
        #endregion

        #region public properties

        #region IInfoOutputQueriable Members

        /// <summary>Gets the info-output level of detail.
        /// </summary>
        /// <value>The info-output level of detail.</value>
        public InfoOutputDetailLevel InfoOutputDetailLevel
        {
            get { return InfoOutputDetailLevel.Full; }
        }
        #endregion

        #region IMatrix Members

        /// <summary>Gets the number of rows of the current matrix object.
        /// </summary>
        /// <value>The number of rows.</value>
        public int RowCount
        {
            get { return Dimension; }
        }

        /// <summary>Gets the number of columns of the current matrix object.
        /// </summary>
        /// <value>The number of columns.</value>
        public int ColumnCount
        {
            get { return Dimension; }
        }

        /// <summary>Gets a specified value.
        /// </summary>
        /// <param name="rowIndex">The null-based row index.</param>
        /// <param name="columnIndex">The null-based column index.</param>
        /// <value>The value of the matrix at the specified coordinate.</value>
        /// <exception cref="IndexOutOfRangeException">Thrown, if the row-/column position is invalid.</exception>
        public double this[int rowIndex, int columnIndex]
        {
            get
            {
                if ((rowIndex < 0) || (rowIndex >= Dimension))
                {
                    throw new ArgumentOutOfRangeException(nameof(rowIndex));
                }
                if ((columnIndex < 0) || (columnIndex >= Dimension))
                {
                    throw new ArgumentOutOfRangeException(nameof(columnIndex));
                }

                if (rowIndex == columnIndex)
                {
                    return Data[rowIndex];
                }
                return 0.0;
            }
        }

        /// <summary>Gets a value indicating whether this instance represents a quadratic matrix.
        /// </summary>
        /// <value><c>true</c> if this instance is quadratic; otherwise, <c>false</c>.</value>
        public bool IsQuadratic
        {
            get { return true; }
        }
        #endregion

        /// <summary>Gets the dimension of the diagonal matrix.
        /// </summary>
        /// <value>The dimension.</value>
        public int Dimension
        {
            get;
            private set;
        }
        #endregion

        #region public methods

        #region IMatrix Members

        /// <summary>Creates a <see cref="DenseMatrix" /> object from the current object.
        /// </summary>
        /// <param name="matrixData">The optional (internal) memory allocation to take into account for the <see cref="DenseMatrix" /> object, i.e. with at
        /// least <see cref="IMatrix.RowCount" /> * <see cref="IMatrix.ColumnCount" /> elements.</param>
        /// <returns>A <see cref="DenseMatrix" /> object that represents the current object.</returns>
        /// <remarks>The argument <paramref name="matrixData" /> can be used to reduce reallocation of memory.</remarks>
        public DenseMatrix ToDenseMatrix(double[] matrixData = null)
        {
            if (matrixData == null)
            {
                matrixData = new double[Dimension * Dimension];
            }
            else
            {
                BLAS.Level1.dscal(Dimension * Dimension, 0.0, matrixData);  // set all entries to 0.0
            }

            for (int i = 0; i < Dimension; i++)  // set diagonal elements only
            {
                int denseMatrixDataEntryIndex = i * (1 + Dimension);
                matrixData[denseMatrixDataEntryIndex] = Data[i];
            }
            return new DenseMatrix(Dimension, Dimension, matrixData);
        }

        /// <summary>Gets a specific column.
        /// </summary>
        /// <param name="columnIndex">The null-based index of the column.</param>
        /// <param name="matrixData">The optional (internal) memory allocation to take into account for the <see cref="DenseMatrix" /> object, i.e. with at
        /// least <see cref="IMatrix.RowCount" /> elements.</param>
        /// <returns>The specified column vector in its <see cref="DenseMatrix" /> representation.</returns>
        /// <remarks>The argument <paramref name="matrixData" /> can be used to reduce reallocation of memory.</remarks>
        public DenseMatrix GetColumn(int columnIndex, double[] matrixData = null)
        {
            if (matrixData == null)
            {
                matrixData = new double[Dimension];
            }
            int k = 0;
            for (int rowIndex = 0; rowIndex < Dimension; rowIndex++)
            {
                matrixData[k] = this[rowIndex, columnIndex];
                k++;
            }
            return new DenseMatrix(Dimension, 1, matrixData);
        }

        /// <summary>Gets a specific transposed row.
        /// </summary>
        /// <param name="rowIndex">The null-based index of the row.</param>
        /// <param name="matrixData">The optional (internal) memory allocation to take into account for the <see cref="DenseMatrix" /> object, i.e. with at
        /// least <see cref="IMatrix.ColumnCount" /> elements.</param>
        /// <returns>The specified transposed row vector in its <see cref="DenseMatrix" /> representation.</returns>
        /// <remarks>The argument <paramref name="matrixData" /> can be used to reduce reallocation of memory.</remarks>
        public DenseMatrix GetTransposedRow(int rowIndex, double[] matrixData = null)
        {
            if (matrixData == null)
            {
                matrixData = new double[Dimension];
            }
            int k = 0;
            for (int columnIndex = 0; columnIndex < Dimension; columnIndex++)
            {
                matrixData[k] = this[rowIndex, columnIndex];
                k++;
            }
            return new DenseMatrix(Dimension, 1, matrixData);
        }

        /// <summary>Gets a specific sub-matrix.
        /// </summary>
        /// <param name="startRowIndex">The null-based index of the upper row.</param>
        /// <param name="endRowIndex">The null-based index of the lower row.</param>
        /// <param name="startColumnIndex">The null-based index of the left column.</param>
        /// <param name="endColumnIndex">The null-based index of the right column.</param>
        /// <param name="matrixData">The optional (internal) memory allocation to take into account for the <see cref="DenseMatrix" /> object, i.e. with at
        /// least (<paramref name="endRowIndex" /> - <paramref name="startRowIndex" /> + 1) * (<paramref name="endColumnIndex" /> - <paramref name="startColumnIndex" /> + 1) elements.</param>
        /// <returns>The specified sub-matrix in its <see cref="DenseMatrix" /> representation.</returns>
        /// <remarks>The argument <paramref name="matrixData" /> can be used to reduce reallocation of memory.</remarks>
        public DenseMatrix GetSubMatrix(int startRowIndex, int endRowIndex, int startColumnIndex, int endColumnIndex, double[] matrixData = null)
        {
            int subColumnCount = endColumnIndex - startColumnIndex + 1;
            int subRowCount = endRowIndex - startRowIndex + 1;

            if ((subRowCount > Dimension) || (subColumnCount > Dimension))
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentCombinationInvalid, "sub-matrix range and given matrix"));
            }
            double[] subMatrix = matrixData;
            if ((subMatrix == null) || (subMatrix.Length < subRowCount * subColumnCount))
            {
                subMatrix = new double[subRowCount * subColumnCount];
            }

            int k = 0;
            for (int columnIndex = 0; columnIndex < subColumnCount; columnIndex++)
            {
                for (int rowIndex = 0; rowIndex < subRowCount; rowIndex++)
                {
                    matrixData[k++] = this[rowIndex + startRowIndex, columnIndex + startColumnIndex];
                }
            }
            return new DenseMatrix(subRowCount, subColumnCount, subMatrix);
        }

        /// <summary>Determines whether the matrix is symmetric.
        /// </summary>
        /// <param name="tolerance">A tolerance taken into account, i.e. if abs(a_{i,j} - a{j,i}) is less than <paramref name="tolerance" /> the values are assumed to be equal.</param>
        /// <returns><c>true</c> if this instance is symmetric; otherwise, <c>false</c>.</returns>
        public bool IsSymmetric(double tolerance = MachineConsts.Epsilon)
        {
            return true;
        }

        /// <summary>Gets the trace of a quadratic matrix.
        /// </summary>
        /// <returns>The trace of the matrix, i.e. the sum of the diagonal elements.</returns>
        public double GetTrace()
        {
            double trace = 0.0;
            for (int j = 0; j < Dimension; j++)
            {
                trace += Data[j];
            }
            return trace;
        }

        /// <summary>Gets a specific norm of the current matrix object.
        /// </summary>
        /// <param name="normType">The type of the (matrix) norm.</param>
        /// <returns>The norm of the current instance with respect to the <paramref name="normType" />.</returns>
        public double GetNorm(MatrixNormType normType)
        {
            switch (normType)
            {
                case MatrixNormType.OneNorm:  // for a diagonal matrix some matrix norms are identical
                case MatrixNormType.Infinity:
                case MatrixNormType.LargestAbsoluteValue:
                    return Data[BLAS.Level1.idamax(Dimension, Data)];

                case MatrixNormType.Frobenius:
                    return BLAS.Level1.dnrm2(Dimension, Data);

                default: throw new NotImplementedException();
            }
        }
        #endregion

        #region IInfoOutputQueriable Members

        /// <summary>Sets the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> property.
        /// </summary>
        /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
        /// <returns>A value indicating whether the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> has been set to <paramref name="infoOutputDetailLevel" />.</returns>
        public bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
        {
            return (infoOutputDetailLevel == InfoOutputDetailLevel.Full);
        }

        /// <summary>Gets informations of the current object as a specific <see cref="InfoOutput" /> instance.
        /// </summary>
        /// <param name="infoOutput">The <see cref="InfoOutput" /> object which is to be filled with informations concering the current instance.</param>
        /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
        public void FillInfoOutput(InfoOutput infoOutput, string categoryName = InfoOutput.GeneralCategoryName)
        {
            InfoOutputPackage infoOutputCollection = infoOutput.AcquirePackage(categoryName);
            infoOutputCollection.Add(new InfoOutputProperty("RowCount", RowCount),
                                     new InfoOutputProperty("ColumnCount", ColumnCount),
                                     new InfoOutputProperty("Dimension", Dimension),
                                     new InfoOutputProperty("IsSymmetric", true));

            var dataTable = new System.Data.DataTable("Data");
            for (int j = 0; j < Dimension; j++)
            {
                dataTable.Columns.Add(j.ToString(), typeof(double));
            }

            for (int k = 0; k < Dimension; k++)
            {
                var row = dataTable.NewRow();
                for (int j = 0; j < Dimension; j++)
                {
                    row[j] = this[k, j];
                }
                dataTable.Rows.Add(row);
            }
            infoOutputCollection.Add(dataTable);
        }
        #endregion

        /// <summary>Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.</returns>
        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendLine(string.Format("Dimension: {0:g} ", Dimension));
            strBuilder.AppendLine("{");
            for (int j = 0; j < RowCount; j++)
            {
                strBuilder.Append("[");
                for (int k = 0; k < ColumnCount; k++)
                {
                    strBuilder.Append(string.Format("{0:g} ", this[j, k]));
                }
                strBuilder.AppendLine("]");
            }
            strBuilder.AppendLine("}");
            return strBuilder.ToString();
        }
        #endregion
    }
}