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

using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary
{
    /// <summary>Serves as interface for real-valued matrices.
    /// </summary>
    /// <remarks>For special matrices and problems one may apply BLAS, LAPACK functions etc. directly without using specific wrapper classes. 
    /// Therefore we establish wrapper classes for some matrix categories only.</remarks>
    public interface IMatrix
    {
        #region properties

        /// <summary>Gets the number of rows of the current matrix object.
        /// </summary>
        /// <value>The number of rows.</value>
        int RowCount
        {
            get;
        }

        /// <summary>Gets the number of columns of the current matrix object.
        /// </summary>
        /// <value>The number of columns.</value>
        int ColumnCount
        {
            get;
        }

        /// <summary>Gets a specified value.
        /// </summary>
        /// <param name="rowIndex">The null-based row index.</param>
        /// <param name="columnIndex">The null-based column index.</param>
        /// <value>The value of the matrix at the specified coordinate.</value>
        /// <exception cref="IndexOutOfRangeException">Thrown, if the row-/column position is invalid.</exception>
        double this[int rowIndex, int columnIndex]
        {
            get;
        }

        /// <summary>Gets a value indicating whether this instance represents a quadratic matrix.
        /// </summary>
        /// <value><c>true</c> if this instance is quadratic; otherwise, <c>false</c>.</value>
        bool IsQuadratic
        {
            get;
        }
        #endregion

        #region methods

        /// <summary>Creates a <see cref="DenseMatrix"/> object from the current object.
        /// </summary>
        /// <param name="matrixData">The optional memory allocation to take into account for the <see cref="DenseMatrix"/> object, i.e. with at 
        /// least <see cref="IMatrix.RowCount"/> * <see cref="IMatrix.ColumnCount"/> elements.</param>
        /// <returns>A <see cref="DenseMatrix"/> object that represents the current object.</returns>
        /// <remarks>The argument <paramref name="matrixData"/> can be used to reduce reallocation of memory.</remarks>
        DenseMatrix ToDenseMatrix(double[] matrixData = null);

        /// <summary>Gets a specific column.
        /// </summary>
        /// <param name="columnIndex">The null-based index of the column.</param>
        /// <param name="matrixData">The optional (internal) memory allocation to take into account for the <see cref="DenseMatrix"/> object, i.e. with at 
        /// least <see cref="IMatrix.RowCount"/> elements.</param>
        /// <returns>The specified column vector in its <see cref="DenseMatrix"/> representation.</returns>
        /// <remarks>The argument <paramref name="matrixData"/> can be used to reduce reallocation of memory.</remarks>
        DenseMatrix GetColumn(int columnIndex, double[] matrixData = null);

        /// <summary>Gets a specific transposed row.
        /// </summary>
        /// <param name="rowIndex">The null-based index of the row.</param>
        /// <param name="matrixData">The optional (internal) memory allocation to take into account for the <see cref="DenseMatrix"/> object, i.e. with at 
        /// least <see cref="IMatrix.ColumnCount"/> elements.</param>
        /// <returns>The specified transposed row vector in its <see cref="DenseMatrix"/> representation.</returns>
        /// <remarks>The argument <paramref name="matrixData"/> can be used to reduce reallocation of memory.</remarks>
        DenseMatrix GetTransposedRow(int rowIndex, double[] matrixData = null);

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
        DenseMatrix GetSubMatrix(int startRowIndex, int endRowIndex, int startColumnIndex, int endColumnIndex, double[] matrixData = null);

        /// <summary>Determines whether the matrix is symmetric.
        /// </summary>
        /// <param name="tolerance">A tolerance taken into account, i.e. if abs(a_{i,j} - a{j,i}) is less than <paramref name="tolerance"/> the values are assumed to be equal.</param>
        /// <returns>
        /// 	<c>true</c> if this instance is symmetric; otherwise, <c>false</c>.
        /// </returns>
        bool IsSymmetric(double tolerance = MachineConsts.Epsilon);

        /// <summary>Gets the trace of a quadratic matrix.
        /// </summary>
        /// <returns>The trace of the matrix, i.e. the sum of the diagonal elements.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if the current object does not represent a quadratic matrix.</exception>
        double GetTrace();

        /// <summary>Gets a specific norm of the current matrix object.
        /// </summary>
        /// <param name="normType">The type of the (matrix) norm.</param>
        /// <returns>The norm of the current instance with respect to the <paramref name="normType"/>.</returns>
        double GetNorm(MatrixNormType normType);
        #endregion
    }
}