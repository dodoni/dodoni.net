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
using System.Numerics;
using System.Collections.Generic;

using Dodoni.MathLibrary.Basics;
using Dodoni.MathLibrary.Basics.LowLevel;

namespace Dodoni.MathLibrary.Basics.LowLevel.BuildIn
{
    /// <summary>Serves as (primitive) managed code implementation of BLAS level 3 operations (fall-back solution).
    /// </summary>
    /// <remarks>This implementation is based on the C code of the BLAS implementation, see http://www.netlib.org/clapack/cblas. </remarks>
    internal partial class BuildInLevel3BLAS
    {
        /// <summary>Computes a matrix-matrix product where one input matrix is triangular, i.e. B := \alpha * op(A)*B or B:= \alpha *B * op(A), where A is a unit or non-unit upper or lower triangular matrix.
        /// </summary>
        /// <param name="m">The number of rows of matrix B.</param>
        /// <param name="n">The number of columns of matrix B.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The triangular matrix A supplied column-by-column of dimension (<paramref name="lda" />, k), where k is <paramref name="m" /> if to calculate B := \alpha * op(A)*B; <paramref name="n" /> otherwise.</param>
        /// <param name="b">The matrix B supplied column-by-column of dimension (<paramref name="ldb" />, <paramref name="n" />).</param>
        /// <param name="lda">The leading dimension of <paramref name="a" />, must be at least max(1,<paramref name="m" />) if to calculate B := \alpha * op(A)*B; max(1,<paramref name="n" />) otherwise.</param>
        /// <param name="ldb">The leading dimension of <paramref name="b" />, must be at least max(1,<paramref name="m" />).</param>
        /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
        /// <param name="side">A value indicating whether to calculate B := \alpha * op(A)*B or B:= \alpha *B * op(A).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        public void ztrmm(int m, int n, Complex alpha, Complex[] a, Complex[] b, int lda, int ldb, bool isUnitTriangular = true, BLAS.Side side = BLAS.Side.Left, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose)
        {
            if (side == BLAS.Side.Left)
            {
                if (transpose == BLAS.MatrixTransposeState.NoTranspose)  // B := \alpha * A * B
                {
                    if (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            for (int k = 0; k < m; k++)
                            {
                                Complex temp = alpha * b[k + j * ldb];
                                for (int i = 0; i <= k - 1; i++)
                                {
                                    b[i + j * ldb] += temp * a[i + k * lda];
                                }
                                if (isUnitTriangular == false)
                                {
                                    temp = temp * a[k + k * lda];
                                }
                                b[k + j * ldb] = temp;
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < n; j++)
                        {
                            for (int k = m - 1; k >= 0; k--)
                            {
                                Complex temp = alpha * b[k + j * ldb];

                                b[k + j * ldb] = temp;
                                if (isUnitTriangular == false)
                                {
                                    b[k + j * ldb] = b[k + j * ldb] * a[k + k * lda];
                                }
                                for (int i = k + 1; i < m; i++)
                                {
                                    b[i + j * ldb] += temp * a[i + k * lda];
                                }
                            }
                        }
                    }
                }
                else  // B := \alpha * A' * B or B := \alpha * conjg( A' ) * B
                {
                    if (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            for (int i = m - 1; i >= 0; i--)
                            {
                                Complex temp = b[i + j * ldb];
                                if (transpose == BLAS.MatrixTransposeState.Transpose)
                                {
                                    if (isUnitTriangular == false)
                                    {
                                        temp = temp * a[i + i * lda];
                                    }
                                    for (int k = 0; k <= i - 1; k++)
                                    {
                                        temp += a[k + i * lda] * b[k + j * ldb];
                                    }
                                }
                                else
                                {
                                    if (isUnitTriangular == false)
                                    {
                                        temp = temp * Complex.Conjugate(a[i + i * lda]);
                                    }
                                    for (int k = 0; k <= i - 1; k++)
                                    {
                                        temp += Complex.Conjugate(a[k + i * lda]) * b[k + j * ldb];
                                    }
                                }
                                b[i + j * ldb] = alpha * temp;
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < n; j++)
                        {
                            for (int i = 0; i < m; i++)
                            {
                                Complex temp = b[i + j * ldb];
                                if (transpose == BLAS.MatrixTransposeState.Transpose)
                                {
                                    if (isUnitTriangular == false)
                                    {
                                        temp = temp * a[i + i * lda];
                                    }
                                    for (int k = i + 1; k < m; k++)
                                    {
                                        temp += a[k + i * lda] * b[k + j * ldb];
                                    }
                                }
                                else
                                {
                                    if (isUnitTriangular == false)
                                    {
                                        temp = temp * Complex.Conjugate(a[i + i * lda]);
                                    }
                                    for (int k = i + 1; k < m; k++)
                                    {
                                        temp += Complex.Conjugate(a[k + i * lda]) * b[k + j * ldb];
                                    }
                                }
                                b[i + j * ldb] = alpha * temp;
                            }
                        }
                    }
                }
            }
            else
            {
                if (transpose == BLAS.MatrixTransposeState.NoTranspose)  // B:= \alpha * B * A
                {
                    if (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix)
                    {
                        for (int j = n - 1; j >= 0; j--)
                        {
                            Complex temp = alpha;
                            if (isUnitTriangular == false)
                            {
                                temp = temp * a[j + j * lda];
                            }
                            for (int i = 0; i < m; i++)
                            {
                                b[i + j * ldb] = temp * b[i + j * ldb];
                            }
                            for (int k = 0; k <= j - 1; k++)
                            {
                                temp = alpha * a[k + j * lda];
                                for (int i = 0; i < m; i++)
                                {
                                    b[i + j * ldb] += temp * b[i + k * ldb];
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < n; j++)
                        {
                            Complex temp = alpha;
                            if (isUnitTriangular == false)
                            {
                                temp = temp * a[j + j * lda];
                            }
                            for (int i = 0; i < m; i++)
                            {
                                b[i + j * ldb] = temp * b[i + j * ldb];
                            }
                            for (int k = j + 1; k < n; k++)
                            {
                                temp = alpha * a[k + j * lda];
                                for (int i = 0; i < m; i++)
                                {
                                    b[i + j * ldb] += temp * b[i + k * ldb];
                                }
                            }
                        }
                    }
                }
                else  // B := \alpha * B * A', B := \alpha * B * \conjg( A' )
                {
                    if (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix)
                    {
                        Complex temp = 0.0;

                        for (int k = 0; k < n; k++)
                        {
                            for (int j = 0; j <= k - 1; j++)
                            {
                                if (transpose == BLAS.MatrixTransposeState.Transpose)
                                {
                                    temp = alpha * a[j + k * lda];
                                }
                                else
                                {
                                    temp = alpha * Complex.Conjugate(a[j + k * lda]);
                                }
                                for (int i = 0; i < m; i++)
                                {
                                    b[i + j * ldb] += temp * b[i + k * ldb];
                                }
                            }

                            temp = alpha;
                            if (isUnitTriangular == false)
                            {
                                if (transpose == BLAS.MatrixTransposeState.Transpose)
                                {
                                    temp = temp * a[k + k * lda];
                                }
                                else
                                {
                                    temp = temp * Complex.Conjugate(a[k + k * lda]);
                                }
                            }
                            for (int i = 0; i < m; i++)
                            {
                                b[i + k * ldb] = temp * b[i + k * ldb];
                            }
                        }
                    }
                    else
                    {
                        Complex temp = 0.0;
                        for (int k = n - 1; k >= 0; k--)
                        {
                            for (int j = k + 1; j < n; j++)
                            {
                                if (transpose == BLAS.MatrixTransposeState.Transpose)
                                {
                                    temp = alpha * a[j + k * lda];
                                }
                                else
                                {
                                    temp = alpha * Complex.Conjugate(a[j + k * lda]);
                                }
                                for (int i = 0; i < m; i++)
                                {
                                    b[i + j * ldb] += temp * b[i + k * ldb];
                                }
                            }
                            temp = alpha;
                            if (isUnitTriangular == false)
                            {
                                if (transpose == BLAS.MatrixTransposeState.Transpose)
                                {
                                    temp = temp * a[k + k * lda];
                                }
                                else
                                {
                                    temp = temp * Complex.Conjugate(a[k + k * lda]);
                                }
                            }
                            for (int i = 0; i < m; i++)
                            {
                                b[i + k * ldb] = temp * b[i + k * ldb];
                            }
                        }
                    }
                }
            }
        }
    }
}
