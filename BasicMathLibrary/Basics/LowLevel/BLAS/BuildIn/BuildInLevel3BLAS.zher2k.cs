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
        /// <summary>Performs a Hermitian rank-2 update, i.e. C := \alpha*A*B^h + conjg(\alpha)*B*A^h + \beta * C or C := \alpha*B^h*A + conjg(\alpha)*A^h*B + beta*C, where C is an Hermitian matrix.
        /// </summary>
        /// <param name="n">The order of matrix C.</param>
        /// <param name="k">The number of columns of matrix A if to calculate C := \alpha*A*B^h + conjg(\alpha)*B*A^h + \beta * C; the number of rows of the matrix A otherwise.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The matrix A supplied column-by-column of dimension (<paramref name="lda" />, ka), where ka equals to <paramref name="k" /> if to calculate C := \alpha*A*B^h + conjg(\alpha)*B*A^h + \beta * C; <paramref name="n" /> otherwise.</param>
        /// <param name="b">The matrix B supplied column-by-column of dimension (<paramref name="ldb" />, kb), where kb equals to <paramref name="k" /> if to calculate C := \alpha*A*B^h + conjg(\alpha)*B*A^h + \beta * C; <paramref name="n" /> otherwise.</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="c">The Hermitian matrix C supplied column-by-column of dimension (<paramref name="ldc" />, <paramref name="n" />).</param>
        /// <param name="lda">The leading dimension of <paramref name="a" />, must be at least max(1,<paramref name="n" />) if to calculate C := \alpha*A*B^h + conjg(\alpha)*B*A^h + \beta * C; max(1, <paramref name="k" />) otherwise.</param>
        /// <param name="ldb">The leading dimension of <paramref name="b" />, must be at least max(1,<paramref name="n" />) if to calculate C := \alpha*A*B^h + conjg(\alpha)*B*A^h + \beta * C; max(1, <paramref name="k" />) otherwise.</param>
        /// <param name="ldc">The leading dimension of <paramref name="c" />, must be at least max(1, <paramref name="n" />).</param>
        /// <param name="triangularMatrixType">A value whether matrix C is in its upper or lower triangular representation.</param>
        /// <param name="operation">A value indicating whether to calculate C := \alpha*A*B^h + conjg(\alpha)*B*A^h + \beta * C or C := \alpha*B^h*A + conjg(\alpha)*A^h*B + beta*C.</param>
        public void zher2k(int n, int k, Complex alpha, Complex[] a, Complex[] b, double beta, Complex[] c, int lda, int ldb, int ldc, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.Zher2kOperation operation = BLAS.Zher2kOperation.ATimesBHermitePlusBTimesAHermite)
        {
            if (n == 0 || ((alpha == 0.0 || k == 0) && (beta == 1.0)))
            {
                return; // nothing to do
            }

            if (operation == BLAS.Zher2kOperation.ATimesBHermitePlusBTimesAHermite)  // C := \alpha * A*conjg(B') + conjg(\alpha) *B*conjg(A') + \beta * C
            {
                if (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix)
                {
                    for (int j = 0; j < n; j++)
                    {
                        for (int i = 0; i <= j - 1; i++)
                        {
                            c[i + j * ldc] *= beta;
                        }
                        c[j + j * ldc] = beta * c[j + j * ldc].Real;

                        for (int ell = 0; ell < k; ell++)
                        {
                            Complex temp = alpha * Complex.Conjugate(b[j + ell * ldb]);
                            Complex temp2 = Complex.Conjugate(alpha * a[j + ell * lda]);

                            for (int i = 0; i <= j - 1; i++)
                            {
                                c[i + j * ldc] += a[i + ell * lda] * temp + b[i + ell * ldb] * temp2;
                            }
                            c[j + j * ldc] += a[j + ell * lda] * temp + b[j + ell * ldb] * temp2;
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < n; j++)
                    {
                        for (int i = j + 1; i < n; i++)
                        {
                            c[i + j * ldc] *= beta;
                        }
                        c[j + j * ldc] = beta * c[j + j * ldc].Real;

                        for (int ell = 0; ell < k; ell++)
                        {

                            Complex temp = alpha * Complex.Conjugate(b[j + ell * ldb]);
                            Complex temp2 = Complex.Conjugate(alpha * a[j + ell * lda]);

                            for (int i = j + 1; i < n; i++)
                            {
                                c[i + j * ldc] += a[i + ell * lda] * temp + b[i + ell * ldb] * temp2;
                            }
                            c[j + j * ldc] += a[j + ell * lda] * temp + b[j + ell * ldb] * temp2;
                        }
                    }
                }
            }
            else  // C := \alpha * conjg(A')*B + conjg(\alpha) * conjg(B')*A + C
            {
                if (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix)
                {
                    for (int j = 0; j < n; j++)
                    {
                        for (int i = 0; i <= j; i++)
                        {
                            Complex temp = 0.0;
                            Complex temp2 = 0.0;

                            for (int ell = 0; ell < k; ell++)
                            {
                                temp += Complex.Conjugate(a[ell + i * lda]) * b[ell + j * ldb];
                                temp2 += Complex.Conjugate(b[ell + i * ldb]) * a[ell + j * lda];
                            }

                            if (i == j)
                            {
                                c[j + j * ldc] = (beta * c[j + j * ldc] + alpha * temp + Complex.Conjugate(alpha) * temp2).Real;
                            }
                            else
                            {
                                c[i + j * ldc] = beta * c[i + j * ldc] + alpha * temp + Complex.Conjugate(alpha) * temp2;
                            }
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < n; j++)
                    {
                        for (int i = j; i < n; i++)
                        {
                            Complex temp = 0.0;
                            Complex temp2 = 0.0;

                            for (int ell = 0; ell < k; ell++)
                            {
                                temp += Complex.Conjugate(a[ell + i * lda]) * b[ell + j * ldb];
                                temp2 += Complex.Conjugate(b[ell + i * ldb]) * a[ell + j * lda];
                            }

                            if (i == j)
                            {
                                c[j + j * ldc] = (beta * c[j + j * ldc] + alpha * temp + Complex.Conjugate(alpha) * temp2).Real;
                            }
                            else
                            {
                                c[i + j * ldc] = beta * c[i + j * ldc] + alpha * temp + Complex.Conjugate(alpha) * temp2;
                            }
                        }
                    }
                }
            }
        }
    }
}