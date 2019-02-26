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
    public partial class GeneralBandMatrix
    {
        #region public methods

        /// <summary>Scales the current matrix, i.e. C = \alpha * C, where 'C' represents the matrix specified by the current instance. The current instance will be changed.
        /// </summary>
        /// <param name="alpha">The (real) scalar.</param>
        /// <returns>The result of the operation, i.e. '\alpha * this', which is a reference to the current instance.</returns>
        /// <remarks>The current instance will be changed and a reference to the current object will be returned.</remarks>
        public GeneralBandMatrix Scale(double alpha)
        {
            BLAS.Level1.dscal(m_RowCount * m_ColumnCount, alpha, m_Data);
            return this;
        }

        /// <summary>Gets A = D * A, where D is a diagonal matrix and the matrix A is specified by the current instance. The current instance will be changed.
        /// </summary>
        /// <param name="diagonalMatrix">The diagonal matrix, i.e. an array with at least <see cref="IMatrix.RowCount"/> elements.</param>
        /// <returns>The result of the operator, i.e. 'A = D * A', where 'A' represents the matrix specified by the current instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="diagonalMatrix"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown, if the number of elements of <paramref name="diagonalMatrix"/> is less than the number of rows of the current object.</exception>
        public GeneralBandMatrix LeftMultiplyDiagonalMatrixAssignment(Span<double> diagonalMatrix)
        {
            if (diagonalMatrix == null)
            {
                throw new ArgumentNullException(nameof(diagonalMatrix), String.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentNull, "'diagonalMatrix'"));
            }
            if (diagonalMatrix.Length < RowCount)
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentHasWrongDimension, "diagonalMatrix (length < number of rows)"), nameof(diagonalMatrix));
            }

            int subPlusSuperDiagonalCount = m_SubDiagonalCount + m_SuperDiagonalCount;
            int k = subPlusSuperDiagonalCount + 1;

            switch (m_TransposeState)
            {
                case BLAS.MatrixTransposeState.NoTranspose:
                    /* set 'a_{i,j} = d_{i,i} * a_{i,j}' and it holds
                     *    a_{ij} = m_Data[i - j + m_SuperDiagonalCount + j*(m_SuperDiagonalCount + m_SubDiagonalCount +1)]
                     * for i=0,...,m_RowCount-1 and j=i-m_SubDiagonalCount,..., i+m_SuperDiagonalCount. 
                     *
                     * Substitute j = (i-m_SubDiagonalCount) + k with k =0,..,k=m_SubDiagonalCount + m_SuperDiagonalCount,
                     * i.e. 
                     *    a_{ij} = m_Data[i+m_SuperDiagonalCount + (i-m_SubDiagonalCount)*(m_SuperDiagonalCount+m_SubDiagonalCount) + k * (m_SuperDiagonalCount+m_SubDiagonalCount)]
                     */
                    for (int i = 0; i < m_RowCount; i++)
                    {
                        BLAS.Level1.dscal(k, diagonalMatrix[i], m_Data.AsSpan().Slice(i + m_SuperDiagonalCount + (i - m_SubDiagonalCount) * subPlusSuperDiagonalCount), incX: subPlusSuperDiagonalCount);
                    }
                    break;

                case BLAS.MatrixTransposeState.Transpose:  // a_{j,i} += d_{i,i}, i.e. a[j-i+m_SuperDiagonalCount + i * k] += d[i]
                    for (int i = 0; i < m_ColumnCount; i++)
                    {
                        for (int j = Math.Max(0, i - SubDiagonalCount); j <= Math.Min(m_RowCount - 1, i + SuperDiagonalCount); j++)
                        {
                            m_Data[j - i + m_SuperDiagonalCount + i * k] += diagonalMatrix[i];
                        }
                    }
                    break;

                default: throw new InvalidOperationException();
            }
            return this;
        }

        /// <summary>Solves the system of linear equations Ax = b.
        /// </summary>
        /// <param name="b">The vector b, whose single unique column is the right-hand side for the systems of equations (output: Overwritten by the solution vector x).</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="b"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Thrown, if the current instance does not represent a square matrix.</exception>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="b"/> contains less elements than the number of rows/columns of the current instance.</exception>
        /// <remarks>The current instance will not be changed.</remarks>
        public void SolveSystemOfLinearEquations(double[] b)
        {
            if (m_RowCount != m_ColumnCount)
            {
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, ExceptionMessages.ObjectIsInvalid, "General band matrix", "not quadratic"));
            }
            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }
            if (b.Length < m_RowCount)
            {
                throw new ArgumentException(nameof(b));
            }
            var ipivot = new int[m_RowCount];
            int length = m_ColumnCount * (1 + m_SubDiagonalCount + m_SuperDiagonalCount);

            var data = new double[length];  // create a deep copy of the specific matrix
            BLAS.Level1.dcopy(length, m_Data, data);

            LAPACK.LinearEquations.Solver.driver_dgbsv(m_RowCount, m_SubDiagonalCount, m_SuperDiagonalCount, data, ipivot, b);
        }

        /// <summary>Gets the vector product y = \alpha * C * x + \beta * y, where C represents the matrix specified by the current instance and x is some vector.
        /// </summary>
        /// <param name="x">The vector x with at least <see cref="IMatrix.RowCount"/> elements.</param>
        /// <param name="alpha">The (real) scalar factor \alpha.</param>
        /// <param name="beta">The (real) scalar factor \beta.</param>
        /// <param name="y">The vector y with at least <see cref="IMatrix.RowCount"/> elements if op(C) = C or at least <see cref="IMatrix.ColumnCount"/> elements if op(C) = C' (output).</param>
        public void GetVectorProduct(double[] x, double alpha, double beta, double[] y)
        {
            if (x == null)
            {
                throw new ArgumentNullException(nameof(x), String.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentNull, "'x'"));
            }

            int minLengthX = ColumnCount;
            int minLengthY = RowCount;

            if (x.Length < minLengthX)
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentOutOfRangeGreater, "x.Length", minLengthX));
            }
            if (y == null)
            {
                throw new ArgumentNullException(nameof(y), String.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentNull, "'y'"));
            }
            if (y.Length < minLengthY)
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentOutOfRangeGreater, "y.Length", minLengthY));
            }
            BLAS.Level2.dgbmv(m_RowCount, m_ColumnCount, m_SubDiagonalCount, m_SuperDiagonalCount, alpha, m_Data, x, beta, y, m_TransposeState);
        }
        #endregion

        #region public static methods

        /// <summary>The multiplication of two <see cref="GeneralBandMatrix"/> objects, i.e. A * B.
        /// </summary>
        /// <param name="a">The first general band matrix A.</param>
        /// <param name="b">The second general band matrix B.</param>
        /// <returns>The result of the operator, i.e. 'A * B'.</returns>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown, if the number of columns of <paramref name="a"/> does not coincide with the number of rows of <paramref name="b"/>.</exception>
        public static GeneralBandMatrix operator *(GeneralBandMatrix a, GeneralBandMatrix b)
        {
            MatrixSpecialFunction.TestMultiplicationInput(a, b);

            int n = b.ColumnCount;  // number of columns of op(B) and C
            int m = a.RowCount; // number of rows of op(A) and C
            int k = a.ColumnCount; // number of columns of op(A) = number of rows of op(B)

            int subDiagonalCountOpA = a.SubDiagonalCount;
            int superDiagonalCountOpA = a.SuperDiagonalCount;
            int subDiagonalCountOpB = b.SubDiagonalCount;
            int superDiagonalCountOpB = b.SuperDiagonalCount;

            if (b.RowCount != k)
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentCombinationInvalid, "A.ColumnCount, B.RowCount"));
            }

            // determind the parameters for the result object and allocate memory
            int subDiagonalCount = Math.Max(0, subDiagonalCountOpA - superDiagonalCountOpB);
            int superDiagonalCount = Math.Max(0, superDiagonalCountOpA - subDiagonalCountOpB);

            int length = m * (subDiagonalCount + superDiagonalCount + 1);
            double[] c = new double[length];

            double[] aData = a.m_Data;
            double[] bData = b.m_Data;

            // do a second switch and calculate the result for each case:
            if ((a.m_TransposeState == BLAS.MatrixTransposeState.Transpose) && (b.m_TransposeState == BLAS.MatrixTransposeState.Transpose))
            {
                for (int j = 0; j < n; j++)
                {
                    int offsetMatrixA = j * (subDiagonalCountOpA + superDiagonalCountOpA + 1);
                    int offsetMatrixC = j * (subDiagonalCount + superDiagonalCount + 1);

                    for (int i = j - superDiagonalCount; i <= j + subDiagonalCount; i++)
                    {
                        double value = 0.0;
                        for (int s = Math.Max(i - subDiagonalCountOpA, j - superDiagonalCountOpB); s <= Math.Min(k, Math.Min(i + superDiagonalCountOpA, j + subDiagonalCountOpB)); s++)
                        {
                            value += aData[s - i + superDiagonalCountOpA + offsetMatrixA] * bData[j - s + superDiagonalCountOpB + s * (subDiagonalCountOpB + superDiagonalCountOpB + 1)];
                        }
                        c[i - j + superDiagonalCount + offsetMatrixC] = value;
                    }
                }
            }
            else if (a.m_TransposeState == BLAS.MatrixTransposeState.Transpose)
            {
                for (int j = 0; j < n; j++)
                {
                    int offsetMatrixA = j * (subDiagonalCountOpA + superDiagonalCountOpA + 1);
                    int offsetMatrixB = j * (subDiagonalCountOpB + superDiagonalCountOpB + 1);
                    int offsetMatrixC = j * (subDiagonalCount + superDiagonalCount + 1);

                    for (int i = j - superDiagonalCount; i <= j + subDiagonalCount; i++)
                    {
                        double value = 0.0;
                        for (int s = Math.Max(i - subDiagonalCountOpA, j - superDiagonalCountOpB); s <= Math.Min(k, Math.Min(i + superDiagonalCountOpA, j + subDiagonalCountOpB)); s++)
                        {
                            value += aData[s - i + superDiagonalCountOpA + offsetMatrixA] * bData[s - j + superDiagonalCountOpB + offsetMatrixB];
                        }
                        c[i - j + superDiagonalCount + offsetMatrixC] = value;
                    }
                }
            }
            else if (b.m_TransposeState == BLAS.MatrixTransposeState.Transpose)
            {
                for (int j = 0; j < n; j++)
                {
                    int offsetMatrixC = j * (subDiagonalCount + superDiagonalCount + 1);

                    for (int i = j - superDiagonalCount; i <= j + subDiagonalCount; i++)
                    {
                        double value = 0.0;
                        for (int s = Math.Max(i - subDiagonalCountOpA, j - superDiagonalCountOpB); s <= Math.Min(k, Math.Min(i + superDiagonalCountOpA, j + subDiagonalCountOpB)); s++)
                        {
                            value += aData[i - s + superDiagonalCountOpA + s * (subDiagonalCountOpA + superDiagonalCountOpA + 1)] * bData[j - s + superDiagonalCountOpB + s * (subDiagonalCountOpB + superDiagonalCountOpB + 1)];
                        }
                        c[i - j + superDiagonalCount + offsetMatrixC] = value;
                    }
                }
            }
            else  // op(A) = A, op(B) = B
            {
                for (int j = 0; j < n; j++)
                {
                    int offsetMatrixC = j * (subDiagonalCount + superDiagonalCount + 1);
                    int offsetMatrixB = j * (subDiagonalCountOpB + superDiagonalCountOpB + 1);

                    for (int i = j - superDiagonalCount; i <= j + subDiagonalCount; i++)
                    {
                        double value = 0.0;
                        for (int s = Math.Max(i - subDiagonalCountOpA, j - superDiagonalCountOpB); s <= Math.Min(k, Math.Min(i + superDiagonalCountOpA, j + subDiagonalCountOpB)); s++)
                        {
                            value += aData[i - s + superDiagonalCountOpA + s * (subDiagonalCountOpA + superDiagonalCountOpA + 1)] * bData[s - j + superDiagonalCountOpB + offsetMatrixB];
                        }
                        c[i - j + superDiagonalCount + offsetMatrixC] = value;
                    }
                }
            }
            return new GeneralBandMatrix(m, n, subDiagonalCount, superDiagonalCount, c);
        }

        /// <summary>The multiplication of a <see cref="GeneralBandMatrix"/> and a specific scalar.
        /// </summary>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The general band matrix A.</param>
        /// <returns>The result of the operator, i.e. \alpha * A.</returns>
        public static GeneralBandMatrix operator *(double alpha, GeneralBandMatrix a)
        {
            int length = a.DataLength;
            var c = new double[length];
            BLAS.Level1.dcopy(length, a.m_Data, c);
            BLAS.Level1.dscal(length, alpha, c);

            return new GeneralBandMatrix(a.m_RowCount, a.m_ColumnCount, a.m_SubDiagonalCount, a.m_SuperDiagonalCount, c, a.m_TransposeState);
        }

        /// <summary>The multiplication of a <see cref="GeneralBandMatrix"/> and a specific scalar.
        /// </summary>
        /// <param name="a">The general band matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <returns>The result of the operator, i.e. \alpha * A.</returns>
        public static GeneralBandMatrix operator *(GeneralBandMatrix a, double alpha)
        {
            int length = a.DataLength;
            double[] c = new double[length];
            BLAS.Level1.dcopy(length, a.m_Data, c);
            BLAS.Level1.dscal(length, alpha, c);

            return new GeneralBandMatrix(a.m_RowCount, a.m_ColumnCount, a.m_SubDiagonalCount, a.m_SuperDiagonalCount, c, a.m_TransposeState);
        }

        /// <summary>The addition of two <see cref="GeneralBandMatrix"/> objects.
        /// </summary>
        /// <param name="a">The first general band matrix A.</param>
        /// <param name="b">The second general band matrix B.</param>
        /// <returns>The result of the operator, i.e. 'A + B'.</returns>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown, if the number of columns of <paramref name="a"/> does not coincide with the number of rows of <paramref name="b"/>.</exception>
        public static GeneralBandMatrix operator +(GeneralBandMatrix a, GeneralBandMatrix b)
        {
            return Add(a, b, 1.0);
        }

        /// <summary>The subtraction of two <see cref="GeneralBandMatrix"/> objects.
        /// </summary>
        /// <param name="a">The first general band matrix A.</param>
        /// <param name="b">The second general band matrix B.</param>
        /// <returns>The result of the operator, i.e. 'a - b'.</returns>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown, if the number of columns of <paramref name="a"/> does not coincide with the number of rows of <paramref name="b"/>.</exception>
        public static GeneralBandMatrix operator -(GeneralBandMatrix a, GeneralBandMatrix b)
        {
            return Add(a, b, -1.0);
        }
        #endregion

        #region private static methods

        /// <summary>The addition of two <see cref="GeneralBandMatrix"/> objects, i.e. A + \alpha * B.
        /// </summary>
        /// <param name="a">The first general band matrix A.</param>
        /// <param name="b">The second general band matrix B.</param>
        /// <param name="scalar">The scalar \alpha.</param>
        /// <returns>The result of the operator, i.e. A + \alpha * B.</returns>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown, if the number of columns of <paramref name="a"/> does not coincide with the number of rows of <paramref name="b"/>.</exception>
        private static GeneralBandMatrix Add(GeneralBandMatrix a, GeneralBandMatrix b, double scalar)
        {
            MatrixSpecialFunction.TestAdditionInput(a, b);
            int subDiagonalCount = Math.Max(a.SubDiagonalCount, b.SubDiagonalCount);
            int superDiagonalCount = Math.Max(a.SuperDiagonalCount, b.SuperDiagonalCount);

            int rowCount = a.RowCount;
            int columnCount = a.ColumnCount;

            int length = rowCount * (subDiagonalCount + superDiagonalCount + 1);
            var c = new double[length];

            switch (a.m_TransposeState)
            {
                case BLAS.MatrixTransposeState.NoTranspose:
                    if (b.m_TransposeState == BLAS.MatrixTransposeState.NoTranspose)
                    {
                        for (int j = 0; j < columnCount; j++)
                        {
                            int offSetMatrixA = j * (a.m_SubDiagonalCount + a.m_SuperDiagonalCount + 1);
                            int offSetMatrixB = j * (b.m_SubDiagonalCount + b.m_SuperDiagonalCount + 1);
                            int offsetMatrixC = j * (subDiagonalCount + superDiagonalCount + 1);

                            for (int i = Math.Max(0, j - superDiagonalCount); i <= Math.Min(rowCount - 1, j + subDiagonalCount); i++)
                            {
                                double value = 0.0;
                                if ((i - j <= a.m_SubDiagonalCount) && (j - i <= a.m_SuperDiagonalCount))
                                {
                                    value += a.m_Data[i - j + a.m_SuperDiagonalCount + offSetMatrixA];
                                }
                                if ((i - j <= b.m_SubDiagonalCount) && (j - i <= b.m_SuperDiagonalCount))
                                {
                                    value += scalar * b.m_Data[i - j + b.m_SuperDiagonalCount + offSetMatrixB];
                                }
                                c[i - j + superDiagonalCount + offsetMatrixC] = value;
                            }
                        }
                    }
                    else
                    {
                        int k = b.m_SubDiagonalCount + b.m_SuperDiagonalCount + 1;
                        for (int j = 0; j < columnCount; j++)
                        {
                            int offSetMatrixA = j * (a.m_SubDiagonalCount + a.m_SuperDiagonalCount + 1);
                            int offsetMatrixC = j * (subDiagonalCount + superDiagonalCount + 1);

                            for (int i = Math.Max(0, j - superDiagonalCount); i <= Math.Min(rowCount - 1, j + subDiagonalCount); i++)
                            {
                                double value = 0.0;
                                if ((i - j <= a.m_SubDiagonalCount) && (j - i <= a.m_SuperDiagonalCount))
                                {
                                    value += a.m_Data[i - j + a.m_SuperDiagonalCount + offSetMatrixA];
                                }
                                if ((i - j <= b.m_SubDiagonalCount) && (j - i <= b.m_SuperDiagonalCount))
                                {
                                    value += scalar * b.m_Data[j - i + b.m_SuperDiagonalCount + i * k];
                                }
                                c[i - j + superDiagonalCount + offsetMatrixC] = value;
                            }
                        }
                    }
                    break;

                case BLAS.MatrixTransposeState.Transpose:
                    if (b.m_TransposeState == BLAS.MatrixTransposeState.NoTranspose)
                    {
                        int k = a.m_SubDiagonalCount + a.m_SuperDiagonalCount + 1;

                        for (int j = 0; j < columnCount; j++)
                        {
                            int offSetMatrixB = j * (b.m_SubDiagonalCount + b.m_SuperDiagonalCount + 1);
                            int offsetMatrixC = j * (subDiagonalCount + superDiagonalCount + 1);

                            for (int i = Math.Max(0, j - superDiagonalCount); i <= Math.Min(rowCount - 1, j + subDiagonalCount); i++)
                            {
                                double value = 0.0;
                                if ((i - j <= a.m_SubDiagonalCount) && (j - i <= a.m_SuperDiagonalCount))
                                {
                                    value += a.m_Data[j - i + a.m_SuperDiagonalCount + i * k];
                                }
                                if ((i - j <= b.m_SubDiagonalCount) && (j - i <= b.m_SuperDiagonalCount))
                                {
                                    value += scalar * b.m_Data[i - j + b.m_SuperDiagonalCount + offSetMatrixB];
                                }
                                c[i - j + superDiagonalCount + offsetMatrixC] = value;
                            }
                        }
                    }
                    else
                    {
                        int ka = a.m_SubDiagonalCount + a.m_SuperDiagonalCount + 1;
                        int kb = b.m_SubDiagonalCount + b.m_SuperDiagonalCount + 1;
                        for (int j = 0; j < columnCount; j++)
                        {
                            int offsetMatrixC = j * (subDiagonalCount + superDiagonalCount + 1);

                            for (int i = Math.Max(0, j - superDiagonalCount); i <= Math.Min(rowCount - 1, j + subDiagonalCount); i++)
                            {
                                double value = 0.0;
                                if ((i - j <= a.m_SubDiagonalCount) && (j - i <= a.m_SuperDiagonalCount))
                                {
                                    value += a.m_Data[j - i + a.m_SuperDiagonalCount + i * ka];
                                }
                                if ((i - j <= b.m_SubDiagonalCount) && (j - i <= b.m_SuperDiagonalCount))
                                {
                                    value += scalar * b.m_Data[j - i + b.m_SuperDiagonalCount + i * kb];
                                }
                                c[i - j + superDiagonalCount + offsetMatrixC] = value;
                            }
                        }
                    }
                    break;

                default: throw new InvalidOperationException();
            }
            return new GeneralBandMatrix(rowCount, columnCount, subDiagonalCount, superDiagonalCount, c);
        }
        #endregion
    }
}