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
using System.Globalization;

using Dodoni.BasicComponents;
using Dodoni.MathLibrary.Basics;

namespace Dodoni.MathLibrary
{
    public partial class DenseMatrix
    {
        #region public methods

        /// <summary>Scales the current matrix, i.e. C = \alpha * C, where 'C' represents the matrix specified by the current instance. The current instance will be changed.
        /// </summary>
        /// <param name="alpha">The (real) scalar.</param>
        /// <returns>The result of the operation, i.e. \alpha * C, which is a reference to the current instance.</returns>
        /// <remarks>The current instance will be changed and a reference to the current object will be returned.</remarks>
        public DenseMatrix Scale(double alpha)
        {
            BLAS.Level1.dscal(m_RowCount * m_ColumnCount, alpha, m_Data);
            return this;
        }

        /// <summary>Gets 'C = C + \alpha * A, where 'C' represents the matrix specified by the current instance. The current instance will be changed.
        /// </summary>
        /// <param name="a">The dense matrix 'A' to add.</param>
        /// <param name="alpha">The scalar factor \alpha.</param>
        /// <returns>The result of the operation, i.e. '\alpha * A + this', which is a reference to the current instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="a"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown, if the number of rows/columns of <paramref name="a"/> does not coincide with the number of rows/columns of the matrix given by the current instance.</exception>
        /// <remarks>The current object will be changed and a reference to the current object will be returned.
        /// <para>
        ///    Use this method for some '+=' or '-=' operator which may performs better than some separate addition.
        /// </para></remarks>
        public DenseMatrix AddAssignment(DenseMatrix a, double alpha = 1.0)
        {
            MatrixSpecialFunction.TestAdditionInput(this, a);
            if (m_TransposeState == BLAS.MatrixTransposeState.NoTranspose)
            {
                switch (a.m_TransposeState)
                {
                    case BLAS.MatrixTransposeState.NoTranspose:
                        BLAS.Level1.daxpy(m_RowCount * m_ColumnCount, alpha, a.m_Data, m_Data);
                        break;

                    case BLAS.MatrixTransposeState.Transpose: // c[i+j*m_RowCount] += \alpha * a[j+i*a.m_RowCount] for i=0,..,m_RowCount-1, j=0,..,m_ColumnCount-1
                        double[] aData = a.Data;
                        for (int i = 0; i < m_RowCount; i++)
                        {
                            BLAS.Level1.daxpy(m_ColumnCount, alpha, aData.AsSpan().Slice(i * a.m_RowCount), m_Data.AsSpan().Slice(i), 1, m_RowCount);
                        }
                        break;
                    default: throw new InvalidOperationException();
                }
            }
            else if (m_TransposeState == BLAS.MatrixTransposeState.Transpose)
            {
                switch (a.m_TransposeState)
                {
                    case BLAS.MatrixTransposeState.Transpose:
                        BLAS.Level1.daxpy(m_RowCount * m_ColumnCount, alpha, a.m_Data, m_Data);
                        break;

                    case BLAS.MatrixTransposeState.NoTranspose:
                        for (int i = 0; i < m_RowCount; i++)
                        {
                            BLAS.Level1.daxpy(m_ColumnCount, alpha, a.m_Data.AsSpan().Slice(i), m_Data.AsSpan().Slice(i * m_RowCount), a.m_RowCount, 1); //    m_Data[j + i * m_RowCount] += alpha * a.m_Data[i + j * a.m_RowCount] for j \in [0, m_ColumnCount), 
                        }
                        break;
                    default: throw new InvalidOperationException();
                }
            }
            else
            {
                throw new InvalidOperationException();
            }
            return this;
        }

        /// <summary>Gets C = C + \alpha * A, where C represents the matrix specified by the current instance. The current instance will be changed.
        /// </summary>
        /// <param name="a">The general band matrix A to add.</param>
        /// <param name="alpha">The scalar factor \alpha.</param>
        /// <returns>The result of the operation, i.e. \alpha * A + this, which is just a reference of the current instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="a"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown, if the number of rows/columns of A does not coincide with the number of rows/columns of the matrix given by the current instance.</exception>
        /// <remarks>The current object will be changed and a reference to the current object will be returned.
        /// <para>
        ///    Use this method for some '+=' or '-=' operator which may performs better than some separate addition.
        /// </para></remarks>
        public DenseMatrix AddAssignment(GeneralBandMatrix a, double alpha = 1.0)
        {
            MatrixSpecialFunction.TestAdditionInput(this, a);

            int superPlusSubPlusMainDiagonalCount = a.SuperDiagonalCount + a.SubDiagonalCount + 1;

            if (m_TransposeState == BLAS.MatrixTransposeState.NoTranspose)
            {
                switch (a.DataTransposeState)
                {
                    case BLAS.MatrixTransposeState.NoTranspose:
                        for (int j = 0; j < m_ColumnCount; j++)
                        {
                            int iMin = System.Math.Max(0, j - a.SuperDiagonalCount);
                            int iMax = System.Math.Min(m_RowCount - 1, j + a.SubDiagonalCount);

                            for (int i = iMin; i <= iMax; i++)
                            {
                                m_Data[i + m_RowCount * j] += alpha * a.Data[j * superPlusSubPlusMainDiagonalCount + (i - j + a.SuperDiagonalCount)];
                            }
                        }
                        break;

                    case BLAS.MatrixTransposeState.Transpose:
                        for (int j = 0; j < m_ColumnCount; j++)
                        {
                            int iMin = System.Math.Max(0, j - a.SuperDiagonalCount);
                            int iMax = System.Math.Min(m_RowCount - 1, j + a.SubDiagonalCount);

                            for (int i = iMin; i <= iMax; i++)
                            {
                                m_Data[i + m_RowCount * j] += alpha * a.Data[i * superPlusSubPlusMainDiagonalCount + (j - i + a.SuperDiagonalCount)];
                            }
                        }
                        break;

                    default: throw new InvalidOperationException();
                }
            }
            else if (m_TransposeState == BLAS.MatrixTransposeState.Transpose)
            {
                switch (a.DataTransposeState)
                {
                    case BLAS.MatrixTransposeState.NoTranspose:
                        for (int j = 0; j < ColumnCount; j++)  // Property 'ColumnCount' returns the value of m_RowCount
                        {
                            int iMin = System.Math.Max(0, j - a.SuperDiagonalCount);
                            int iMax = System.Math.Min(RowCount - 1, j + a.SubDiagonalCount);

                            for (int i = iMin; i <= iMax; i++)
                            {
                                m_Data[j + m_RowCount * i] += alpha * a.Data[j * superPlusSubPlusMainDiagonalCount + (i - j + a.SuperDiagonalCount)];
                            }
                        }
                        break;

                    case BLAS.MatrixTransposeState.Transpose:
                        for (int j = 0; j < ColumnCount; j++)  // Property 'ColumnCount' returns the value of m_RowCount
                        {
                            int iMin = System.Math.Max(0, j - a.SuperDiagonalCount);
                            int iMax = System.Math.Min(RowCount - 1, j + a.SubDiagonalCount);

                            for (int i = iMin; i <= iMax; i++)
                            {
                                m_Data[j + m_RowCount * i] += alpha * a.Data[i * superPlusSubPlusMainDiagonalCount + (j - i + a.SuperDiagonalCount)];
                            }
                        }
                        break;

                    default: throw new InvalidOperationException();
                }
            }
            else
            {
                throw new InvalidOperationException();
            }
            return this;
        }

        /// <summary>Gets C = \alpha * (A * B) + \beta * C, where C represents the matrix specified by the current instance. The current instance will be changed.
        /// </summary>
        /// <param name="a">The dense matrix A.</param>
        /// <param name="b">The dense matrix B.</param>
        /// <param name="alpha">The scalar factor \alpha.</param>
        /// <param name="beta">The scalar factor \beta.</param>
        /// <returns>The result of the operation, i.e. \alpha * (A * B) + \beta * this, which is a reference to the current instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown, if the number of row/columns of <paramref name="a"/> and <paramref name="b"/> is not allowed for the operation.</exception>
        /// <remarks>The current object will be changed.</remarks>
        public DenseMatrix AddAssignment(DenseMatrix a, DenseMatrix b, double alpha = 1.0, double beta = 1.0)
        {
            MatrixSpecialFunction.TestMultiplicationInput(a, b);

            if (m_TransposeState == BLAS.MatrixTransposeState.Transpose)
            {
                T_InPlace();  // re-sort the matrix represented by the current instance and swap #rows vs. #columns. This allows the application of BLAS level 3 routine in the next step
            }
            int m = a.RowCount;
            int n = b.ColumnCount;
            int k = a.ColumnCount;

            if (m != m_RowCount)
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentCombinationInvalid, "RowCount, A.RowCount"));
            }
            if (n != m_ColumnCount)
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentCombinationInvalid, "ColumnCount, B.ColumnCount"));
            }
            BLAS.Level3.dgemm(m, n, k, alpha, a.m_Data, b.m_Data, beta, m_Data, a.m_TransposeState, b.m_TransposeState);
            return this;
        }

        /// <summary>Gets the vector product y := \alpha * C * x, where C represents the matrix specified by the current instance and x is some vector.
        /// </summary>
        /// <param name="x">The vector x with at least <see cref="IMatrix.ColumnCount"/> elements.</param>
        /// <param name="alpha">The (real) scalar \alpha.</param>
        /// <param name="y">The vector y with at least <see cref="IMatrix.RowCount"/> elements (output).</param>
        public void GetVectorProduct(double[] x, double alpha, double[] y)
        {
            if (x == null)
            {
                throw new ArgumentNullException(nameof(x), String.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentNull, "'x'"));
            }
            if (x.Length < m_ColumnCount)
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentOutOfRangeGreater, "x.Length", m_ColumnCount));
            }
            if (y == null)
            {
                throw new ArgumentNullException(nameof(y), String.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentNull, "'y'"));
            }
            if (y.Length < m_RowCount)
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentOutOfRangeGreater, "y.Length", m_RowCount));
            }
            BLAS.Level2.dgemv(m_RowCount, m_ColumnCount, alpha, m_Data, x, 0, y, m_TransposeState);
        }

        /// <summary>Gets C = D * C, where D is a diagonal matrix and the matrix C is specified by the current instance. The current instance will be changed.
        /// </summary>
        /// <param name="diagonalMatrix">The diagonal matrix, i.e. an array with at least <see cref="IMatrix.RowCount"/> elements.</param>
        /// <returns>The result of the operator, i.e. 'C = D * C', where 'C' represents the matrix specified by the current instance.</returns>
        public DenseMatrix LeftMultiplyDiagonalMatrixAssignment(double[] diagonalMatrix)
        {
            switch (m_TransposeState)
            {
                case BLAS.MatrixTransposeState.NoTranspose:  // c_{i,j} = d_{i,i} * c_{i,j}:                    
                    for (int i = 0; i < m_RowCount; i++)
                    {
                        BLAS.Level1.dscal(m_ColumnCount, diagonalMatrix[i], m_Data.AsSpan().Slice(i), m_RowCount);
                    }
                    break;
                case BLAS.MatrixTransposeState.Transpose:  // c_{j,i} = d_{i,i} * c_{j,i}:
                    for (int i = 0; i < m_ColumnCount; i++)
                    {
                        BLAS.Level1.dscal(m_RowCount, diagonalMatrix[i], m_Data.AsSpan().Slice(m_RowCount * i), 1);
                    }
                    break;

                default: throw new NotOperableException();
            }
            return this;
        }

        /// <summary>Gets C = C * D, where D is a diagonal matrix and the matrix C is specified by the current instance. The current instance will be changed.
        /// </summary>
        /// <param name="diagonalMatrix">The diagonal matrix, i.e. an array with at least <see cref="IMatrix.ColumnCount"/> elements.</param>
        /// <returns>The result of the operator, i.e. C = C * D, where C represents the matrix specified by the current instance.</returns>
        public DenseMatrix RightMultiplyDiagonalMatrixAssignment(double[] diagonalMatrix)
        {
            switch (m_TransposeState)
            {
                case BLAS.MatrixTransposeState.NoTranspose: // c_{i,j} = c_{i,j} * d_j
                    for (int j = 0; j < m_ColumnCount; j++)
                    {
                        BLAS.Level1.dscal(m_RowCount, diagonalMatrix[j], m_Data.AsSpan().Slice(m_RowCount * j), 1);
                    }
                    break;

                case BLAS.MatrixTransposeState.Transpose: // c_{j,i} = c_{j,i} * d_j
                    for (int j = 0; j < m_RowCount; j++)
                    {
                        BLAS.Level1.dscal(m_ColumnCount, diagonalMatrix[j], m_Data.AsSpan().Slice(j), m_RowCount);
                    }
                    break;

                default: throw new NotOperableException();
            }
            return this;
        }
        #endregion

        #region public static methods

        #region multiplication operator methods

        /// <summary>The multiplication of two <see cref="DenseMatrix"/> objects, i.e. A * B.
        /// </summary>
        /// <param name="a">The first dense matrix A.</param>
        /// <param name="b">The second dense matrix B.</param>
        /// <returns>The result of the operator, i.e. A * B.</returns>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown, if the matrix operation A * B can not be place because of unsuited arguments.</exception>
        public static DenseMatrix operator *(DenseMatrix a, DenseMatrix b)
        {
            MatrixSpecialFunction.TestMultiplicationInput(a, b);

            int m = a.RowCount; // number of rows of op(A) and C
            int n = b.ColumnCount;  // number of columns of op(B) and C
            int k = a.ColumnCount; // number of columns of op(A) = number of rows of op(B)

            var c = new double[n * m];
            BLAS.Level3.dgemm(m, n, k, 1.0, a.m_Data, b.m_Data, 0.0, c, a.m_TransposeState, b.m_TransposeState);

            return new DenseMatrix(m, n, c);
        }

        /// <summary>The multiplication of a <see cref="DenseMatrix"/> and a specific scalar.
        /// </summary>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The dense matrix A.</param>
        /// <returns>The result of the operator, i.e. \alpha * A.</returns>
        public static DenseMatrix operator *(double alpha, DenseMatrix a)
        {
            var length = a.m_RowCount * a.m_ColumnCount;
            var c = new double[length];
            BLAS.Level1.dcopy(length, a.m_Data, c);
            BLAS.Level1.dscal(length, alpha, c);

            return new DenseMatrix(a.m_RowCount, a.m_ColumnCount, c, a.m_TransposeState);
        }

        /// <summary>The multiplication of a <see cref="DenseMatrix"/> and a specific scalar.
        /// </summary>
        /// <param name="a">The dense matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <returns>The result of the operator, i.e. \alpha * A.</returns>
        public static DenseMatrix operator *(DenseMatrix a, double alpha)
        {
            return alpha * a;
        }
        #endregion

        #region additive operator methods

        /// <summary>Computes the sum of two specific <see cref="DenseMatrix"/> objects, i.e. A + B.
        /// </summary>
        /// <param name="a">The first matrix, i.e. 'A'.</param>
        /// <param name="b">The second matrix, i.e. 'B'.</param>
        /// <returns>The sum A + B in its <see cref="DenseMatrix"/> representation.</returns>       
        public static DenseMatrix operator +(DenseMatrix a, DenseMatrix b)
        {
            return Add(a, b, scalar: 1.0);
        }

        /// <summary>The subtraction of two <see cref="DenseMatrix"/> objects.
        /// </summary>
        /// <param name="a">The first dense matrix A.</param>
        /// <param name="b">The second dense matrix B.</param>
        /// <returns>The result of the operator, i.e. 'a - b'.</returns>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown, if the number of columns of <paramref name="a"/> does not coincide with the number of rows of <paramref name="b"/>.</exception>
        public static DenseMatrix operator -(DenseMatrix a, DenseMatrix b)
        {
            return Add(a, b, scalar: -1.0);
        }
        #endregion

        /// <summary>Calculates the bilinear form x^t * A * x, where A is a quadratic matrix.
        /// </summary>
        /// <param name="quadraticDenseMatrix">The quadratic matrix 'A'.</param>
        /// <param name="x">The vector 'x'.</param>
        /// <returns>The value of the bilinear form x^t*A*x.</returns>
        public static double GetBilinearForm(DenseMatrix quadraticDenseMatrix, double[] x)
        {
            if (quadraticDenseMatrix == null)
            {
                throw new ArgumentNullException(nameof(quadraticDenseMatrix));
            }
            int n = quadraticDenseMatrix.RowCount;
            double value = 0.0;

            switch (quadraticDenseMatrix.m_TransposeState)
            {
                case BLAS.MatrixTransposeState.NoTranspose:
                    for (int i = 0; i < n; i++)
                    {
                        value += x[i] * BLAS.Level1.ddot(n, x, quadraticDenseMatrix.Data.AsSpan().Slice(i), 1, n);
                    }
                    break;
                case BLAS.MatrixTransposeState.Transpose:
                    for (int i = 0; i < n; i++)
                    {
                        value += x[i] * BLAS.Level1.ddot(n, x, quadraticDenseMatrix.Data.AsSpan().Slice(i * n));
                    }
                    break;
                default:
                    throw new InvalidOperationException();
            }
            return value;
        }
        #endregion

        #region private static methods

        /// <summary>Computes the sum of two specific <see cref="DenseMatrix"/> objects, i.e. A + \alpha * B.
        /// </summary>
        /// <param name="a">The first matrix, i.e. 'A'.</param>
        /// <param name="b">The second matrix, i.e. 'B'.</param>
        /// <param name="scalar">The scalar \alpha.</param>
        /// <returns>The sum A + \alpha * B in its <see cref="DenseMatrix"/> representation.</returns>       
        private static DenseMatrix Add(DenseMatrix a, DenseMatrix b, double scalar)
        {
            MatrixSpecialFunction.TestAdditionInput(a, b);

            int n = a.RowCount * a.ColumnCount;
            var y = new double[n];

            int rowCount = a.RowCount;
            int columnCount = a.ColumnCount;

            if (a.m_TransposeState == BLAS.MatrixTransposeState.NoTranspose)
            {
                switch (b.m_TransposeState)
                {
                    case BLAS.MatrixTransposeState.NoTranspose:
                        BLAS.Level1.dcopy(n, a.Data, y);
                        BLAS.Level1.daxpy(n, scalar, b.Data, y);
                        break;

                    case BLAS.MatrixTransposeState.Transpose:
                        BLAS.Level1.dcopy(n, a.Data, y);
                        for (int i = 0; i < rowCount; i++)
                        {
                            BLAS.Level1.daxpy(columnCount, scalar, b.Data.AsSpan().Slice(i * b.m_RowCount), y.AsSpan().Slice(i), 1, rowCount);
                        }
                        break;

                    default: throw new InvalidOperationException();
                }
                return new DenseMatrix(rowCount, columnCount, y);
            }
            else  // A^t
            {
                switch (b.m_TransposeState)
                {
                    case BLAS.MatrixTransposeState.NoTranspose:
                        BLAS.Level1.dcopy(n, a.Data, y);
                        for (int i = 0; i < rowCount; i++)
                        {
                            BLAS.Level1.daxpy(columnCount, scalar, b.Data.AsSpan().Slice(i * b.m_RowCount), y.AsSpan().Slice(i), 1, rowCount);
                        }
                        break;

                    case BLAS.MatrixTransposeState.Transpose:
                        BLAS.Level1.dcopy(n, a.Data, y);
                        BLAS.Level1.daxpy(n, scalar, b.Data, y);
                        break;

                    default: throw new InvalidOperationException();
                }
                return new DenseMatrix(rowCount, columnCount, y, BLAS.MatrixTransposeState.Transpose);
            }
        }
        #endregion
    }
}