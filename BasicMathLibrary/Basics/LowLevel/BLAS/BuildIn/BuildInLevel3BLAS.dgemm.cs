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
        /// <summary>Computes a matrix-matrix product with a general matrix, i.e. C := \alpha * op(A)*op(B) + \beta * C, where where op(.) is the identity or the transpose operation.
        /// </summary>
        /// <param name="m">The number of rows of the matrix op(A) and of the matrix C.</param>
        /// <param name="n">The number of columns of the matrix op(B) and of the matrix C.</param>
        /// <param name="k">The number of columns of the matrix op(A) and the number of rows of the matrix op(B).</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The matrix A supplied column-by-column of dimension (<paramref name="lda"/>, ka), where ka is <paramref name="k"/> if op(A) = A; <paramref name="m"/> otherwise.</param>
        /// <param name="b">The matrix B supplied column-by-column of dimension (<paramref name="ldb"/>, kb), where kb is <paramref name="n"/> if op(B) = B; <paramref name="k"/> otherwise.</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="c">The matrix C supplied column-by-column of dimension (<paramref name="ldc"/>, <paramref name="n"/>).</param>
        /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="m"/>) if op(A) = A; max(1, <paramref name="k"/>) otherwise.</param>
        /// <param name="ldb">The leading dimension of <paramref name="b"/>, must be at least max(1,<paramref name="k"/>) if op(B) = B; max(1, <paramref name="n"/>) otherwise.</param>
        /// <param name="ldc">The leading dimension of <paramref name="c"/>, must be at least max(1, <paramref name="m"/>).</param>
        /// <param name="transposeA">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        /// <param name="transposeB">A value indicating whether 'op(B)=B' or 'op(B)=B^t'.</param>
        public void dgemm(int m, int n, int k, double alpha, double[] a, double[] b, double beta, double[] c, int lda, int ldb, int ldc, BLAS.MatrixTransposeState transposeA = BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState transposeB = BLAS.MatrixTransposeState.NoTranspose)
        {
            if (m == 0 || n == 0 || ((alpha == 0.0 || k == 0) && beta == 1.0))
            {
                return; // nothing to do
            }

            if (a.Length < lda * ((transposeA == BLAS.MatrixTransposeState.NoTranspose) ? k : m))
            {
                throw new ArgumentException("a");
            }
            if (b.Length < ldb * ((transposeB == BLAS.MatrixTransposeState.NoTranspose) ? n : k))
            {
                throw new ArgumentException("b");
            }
            if (c.Length < ldc * n)
            {
                throw new ArgumentException("c");
            }

            if (transposeA == BLAS.MatrixTransposeState.NoTranspose)
            {
                if (transposeB == BLAS.MatrixTransposeState.NoTranspose) // C = \alpha * A * B + \beta * C
                {
                    for (int j = 0; j < n; j++)
                    {
                        for (int i = 0; i < m; i++)
                        {
                            double temp = 0.0;  // = (A*B)_{i,j}
                            for (int r = 0; r < k; r++)
                            {
                                temp += a[i + r * lda] * b[r + j * ldb];
                            }
                            c[i + j * ldc] = alpha * temp + c[i + j * ldc] * beta;
                        }
                    }
                }
                else  // C = \alpha * A * B' + \beta * C
                {
                    for (int j = 0; j < n; j++)
                    {
                        for (int i = 0; i < m; i++)
                        {
                            double temp = 0.0;  // =(A*B')_{i,j}

                            for (int r = 0; r < k; r++)
                            {
                                temp += a[i + r * lda] * b[j + r * ldb];
                            }
                            c[i + j * ldc] = alpha * temp + c[i + j * ldc] * beta;
                        }
                    }
                }
            }
            else
            {
                if (transposeB == BLAS.MatrixTransposeState.NoTranspose)  // C = \alpha * A' * B + \beta * C
                {
                    for (int j = 0; j < n; j++)
                    {
                        for (int i = 0; i < m; i++)
                        {
                            double temp = 0.0;  // = (A'*B)_{i,j}
                            for (int r = 0; r < k; r++)
                            {
                                temp += a[r + i * lda] * b[r + j * ldb];
                            }
                            c[i + j * ldc] = alpha * temp + c[i + j * ldc] * beta;
                        }
                    }
                }
                else  // C = \alpha * A' * B' + \beta * C 
                {
                    for (int j = 0; j < n; j++)
                    {
                        for (int i = 0; i < m; i++)
                        {
                            double temp = 0.0;  // = (A'*B')_{i,j}
                            for (int r = 0; r < k; r++)
                            {
                                temp += a[r + i * lda] * b[j + r * ldb];
                            }
                            c[i + j * ldc] = alpha * temp + c[i + j * ldc] * beta;
                        }
                    }
                }
            }
        }

        /// <summary>Computes a matrix-matrix product with a general matrix, i.e. C := \alpha * op(A)*op(B) + \beta * C, where op(.) is the identity or the transpose operation.
        /// </summary>
        /// <param name="m">The number of rows of the matrix op(A) and of the matrix C.</param>
        /// <param name="n">The number of columns of the matrix op(B) and of the matrix C.</param>
        /// <param name="k">The number of columns of the matrix op(A) and the number of rows of the matrix op(B).</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The matrix A supplied column-by-column of dimension (<paramref name="lda"/>, ka), where ka is <paramref name="k"/> if op(A) = A; <paramref name="m"/> otherwise.</param>
        /// <param name="b">The matrix B supplied column-by-column of dimension (<paramref name="ldb"/>, kb), where kb is <paramref name="n"/> if op(B) = B; <paramref name="k"/> otherwise.</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="c">The matrix C supplied column-by-column of dimension (<paramref name="ldc"/>, <paramref name="n"/>).</param>
        /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="m"/>) if op(A) = A; max(1, <paramref name="k"/>) otherwise.</param>
        /// <param name="ldb">The leading dimension of <paramref name="b"/>, must be at least max(1,<paramref name="k"/>) if op(B) = B; max(1, <paramref name="n"/>) otherwise.</param>
        /// <param name="ldc">The leading dimension of <paramref name="c"/>, must be at least max(1, <paramref name="m"/>).</param>
        /// <param name="transposeA">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        /// <param name="transposeB">A value indicating whether 'op(B)=B' or 'op(B)=B^t'.</param>
        /// <param name="startIndexA">The null-based start index for <paramref name="a"/></param>
        /// <param name="startIndexB">The null-based start index for <paramref name="b"/></param>
        /// <param name="startIndexC">The null-based start index for <paramref name="c"/></param>
        public void dgemm(int m, int n, int k, double alpha, double[] a, double[] b, double beta, double[] c, int lda, int ldb, int ldc, int startIndexA, int startIndexB, BLAS.MatrixTransposeState transposeA = BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState transposeB = BLAS.MatrixTransposeState.NoTranspose, int startIndexC = 0)
        {
            if (m == 0 || n == 0 || ((alpha == 0.0 || k == 0) && beta == 1.0))
            {
                return; // nothing to do
            }

            if (a.Length < startIndexA + lda * ((transposeA == BLAS.MatrixTransposeState.NoTranspose) ? k : m))
            {
                throw new ArgumentException("a");
            }
            if (b.Length < startIndexB + ldb * ((transposeB == BLAS.MatrixTransposeState.NoTranspose) ? n : k))
            {
                throw new ArgumentException("b");
            }
            if (c.Length < startIndexC + ldc * n)
            {
                throw new ArgumentException("c");
            }

            if (transposeA == BLAS.MatrixTransposeState.NoTranspose)
            {
                if (transposeB == BLAS.MatrixTransposeState.NoTranspose) // C = \alpha * A * B + \beta * C
                {
                    for (int j = 0; j < n; j++)
                    {
                        for (int i = 0; i < m; i++)
                        {
                            double temp = 0.0;  // = (A*B)_{i,j}
                            for (int r = 0; r < k; r++)
                            {
                                temp += a[startIndexA + i + r * lda] * b[startIndexB + r + j * ldb];
                            }
                            c[startIndexC + i + j * ldc] = alpha * temp + c[startIndexC + i + j * ldc] * beta;
                        }
                    }
                }
                else  // C = \alpha * A * B' + \beta * C
                {
                    for (int j = 0; j < n; j++)
                    {
                        for (int i = 0; i < m; i++)
                        {
                            double temp = 0.0;  // =(A*B')_{i,j}

                            for (int r = 0; r < k; r++)
                            {
                                temp += a[startIndexA + i + r * lda] * b[startIndexB + j + r * ldb];
                            }
                            c[startIndexC + i + j * ldc] = alpha * temp + c[startIndexC + i + j * ldc] * beta;
                        }
                    }
                }
            }
            else
            {
                if (transposeB == BLAS.MatrixTransposeState.NoTranspose)  // C = \alpha * A' * B + \beta * C
                {
                    for (int j = 0; j < n; j++)
                    {
                        for (int i = 0; i < m; i++)
                        {
                            double temp = 0.0;  // = (A'*B)_{i,j}
                            for (int r = 0; r < k; r++)
                            {
                                temp += a[startIndexA + r + i * lda] * b[startIndexB + r + j * ldb];
                            }
                            c[startIndexC + i + j * ldc] = alpha * temp + c[startIndexC + i + j * ldc] * beta;
                        }
                    }
                }
                else  // C = \alpha * A' * B' + \beta * C 
                {
                    for (int j = 0; j < n; j++)
                    {
                        for (int i = 0; i < m; i++)
                        {
                            double temp = 0.0;  // = (A'*B')_{i,j}
                            for (int r = 0; r < k; r++)
                            {
                                temp += a[startIndexA + r + i * lda] * b[startIndexB + j + r * ldb];
                            }
                            c[startIndexC + i + j * ldc] = alpha * temp + c[startIndexC + i + j * ldc] * beta;
                        }
                    }
                }
            }
        }
    }
}