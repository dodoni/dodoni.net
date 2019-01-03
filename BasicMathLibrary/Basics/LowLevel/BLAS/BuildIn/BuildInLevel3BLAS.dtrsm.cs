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
        /// <summary>Solves a triangular matrix equation, i.e. op(A) * X = \alpha * B or X * op(A) = \alpha *B, where A is a unit or non-unit upper or lower triangular matrix.
        /// </summary>
        /// <param name="m">The number of rows of matrix B.</param>
        /// <param name="n">The number of column of matrix B.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The triangular matrix A supplied column-by-column of dimension (<paramref name="lda"/>, k), where k is <paramref name="m"/> if to calculate op(A) * X = \alpha * B; <paramref name="n"/> otherwise.</param>
        /// <param name="b">The matrix B supplied column-by-column of dimension (<paramref name="ldb"/>, <paramref name="n"/>).</param>
        /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="m"/>) if to calculate op(A) * X = \alpha * B; max(1,<paramref name="n"/>) otherwise.</param>
        /// <param name="ldb">The leading dimension of <paramref name="b"/>, must be at least max(1,<paramref name="m"/>).</param>
        /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
        /// <param name="side">A value indicating whether to calculate op(A) * X = \alpha * B or X * op(A) = \alpha *B.</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        public void dtrsm(int m, int n, double alpha, double[] a, double[] b, int lda, int ldb, bool isUnitTriangular = true, BLAS.Side side = BLAS.Side.Left, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose)
        {
            if (n == 0)
            {
                return; // nothing to do
            }

            if (side == BLAS.Side.Left)
            {
                if (transpose == BLAS.MatrixTransposeState.NoTranspose)  // A * X = \alpha * B, i.e. X [=:B] = \alpha * Inv(A) * B
                {
                    if (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            for (int i = 0; i < m; i++)
                            {
                                b[i + j * ldb] = alpha * b[i + j * ldb];
                            }

                            for (int k = m - 1; k >= 0; k--)
                            {
                                if (b[k + j * ldb] != 0.0)
                                {
                                    if (isUnitTriangular == false)
                                    {
                                        b[k + j * ldb] /= a[k + k * lda];
                                    }
                                    for (int i = 0; i <= k - 1; i++)
                                    {
                                        b[i + j * ldb] -= b[k + j * ldb] * a[i + k * lda];
                                    }
                                }
                            }
                        }
                    }
                    else  // lower triangular matrix
                    {
                        for (int j = 0; j < n; j++)
                        {
                            for (int i = 0; i < m; i++)
                            {
                                b[i + j * ldb] = alpha * b[i + j * ldb];
                            }

                            for (int k = 0; k < m; k++)
                            {
                                if (isUnitTriangular == false)
                                {
                                    b[k + j * ldb] /= a[k + k * lda];
                                }
                                for (int i = k + 1; i < m; i++)
                                {
                                    b[i + j * ldb] -= b[k + j * ldb] * a[i + k * lda];
                                }
                            }
                        }
                    }
                }
                else  // A' * X = \alpha * B, i.e. X [=: B] = \alpha * inv(A') * B
                {
                    if (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            for (int i = 0; i < m; i++)
                            {
                                double temp = alpha * b[i + j * ldb];
                                for (int k = 0; k <= i - 1; k++)
                                {
                                    temp -= a[k + i * lda] * b[k + j * ldb];
                                }
                                if (isUnitTriangular == false)
                                {
                                    temp /= a[i + i * lda];
                                }
                                b[i + j * ldb] = temp;
                            }
                        }
                    }
                    else  // lower triangular matrix
                    {
                        for (int j = 0; j < n; j++)
                        {
                            for (int i = m - 1; i >= 0; i--)
                            {
                                double temp = alpha * b[i + j * ldb];
                                for (int k = i + 1; k < m; k++)
                                {
                                    temp -= a[k + i * lda] * b[k + j * ldb];
                                }
                                if (isUnitTriangular == false)
                                {
                                    temp /= a[i + i * lda];
                                }
                                b[i + j * ldb] = temp;
                            }
                        }
                    }
                }
            }
            else  // side == BLAS.Side.Right
            {
                if (transpose == BLAS.MatrixTransposeState.NoTranspose)  // X  * A = \alpha * B, i.e. X [=:B] = \alpha * B * Inv(A)
                {
                    if (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            for (int i = 0; i < m; i++)
                            {
                                b[i + j * ldb] = alpha * b[i + j * ldb];
                            }

                            for (int k = 0; k <= j - 1; k++)
                            {
                                for (int i = 0; i < m; i++)
                                {
                                    b[i + j * ldb] -= a[k + j * lda] * b[i + k * ldb];
                                }
                            }
                            if (isUnitTriangular == false)
                            {
                                double temp = 1.0 / a[j + j * lda];
                                for (int i = 0; i < m; i++)
                                {
                                    b[i + j * ldb] = temp * b[i + j * ldb];
                                }
                            }
                        }
                    }
                    else  // lower triangular matrix
                    {
                        for (int j = n - 1; j >= 0; j--)
                        {
                            for (int i = 0; i < m; i++)
                            {
                                b[i + j * ldb] = alpha * b[i + j * ldb];
                            }
                            for (int k = j + 1; k < n; k++)
                            {
                                for (int i = 0; i < m; i++)
                                {
                                    b[i + j * ldb] -= a[k + j * lda] * b[i + k * ldb];
                                }
                            }

                            if (isUnitTriangular == false)
                            {
                                double temp = 1.0 / a[j + j * lda];
                                for (int i = 0; i < m; i++)
                                {
                                    b[i + j * ldb] = temp * b[i + j * ldb];
                                }
                            }
                        }
                    }
                }
                else  // X  * A' = \alpha * B, i.e. X [=:B] = \alpha * B * Inv(A')
                {
                    if (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix)
                    {
                        for (int k = n - 1; k >= 0; k--)
                        {
                            if (isUnitTriangular == false)
                            {
                                double temp = 1.0 / a[k + k * lda];
                                for (int i = 0; i < m; i++)
                                {
                                    b[i + k * ldb] = temp * b[i + k * ldb];
                                }
                            }
                            for (int j = 0; j <= k - 1; j++)
                            {
                                double temp = a[j + k * lda];
                                for (int i = 0; i < m; i++)
                                {
                                    b[i + j * ldb] -= temp * b[i + k * ldb];
                                }
                            }

                            for (int i = 0; i < m; i++)
                            {
                                b[i + k * ldb] = alpha * b[i + k * ldb];
                            }
                        }
                    }
                    else  // lower triangular matrix
                    {
                        for (int k = 0; k < n; k++)
                        {
                            if (isUnitTriangular == false)
                            {
                                double temp = 1.0 / a[k + k * lda];
                                for (int i = 0; i < m; i++)
                                {
                                    b[i + k * ldb] = temp * b[i + k * ldb];
                                }
                            }
                            for (int j = k + 1; j < n; j++)
                            {
                                double temp = a[j + k * lda];
                                for (int i = 0; i < m; i++)
                                {
                                    b[i + j * ldb] -= temp * b[i + k * ldb];
                                }
                            }

                            for (int i = 0; i < m; i++)
                            {
                                b[i + k * ldb] = alpha * b[i + k * ldb];
                            }
                        }
                    }
                }
            }
        }
    }
}