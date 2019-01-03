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
        /// <summary>Performs a symmetric rank-k update, i.e. C:= \alpha*A*A^t + \beta *C or C:= \alpha*A^t*A + \beta*C.
        /// </summary>
        /// <param name="n">The order of matrix C.</param>
        /// <param name="k">The number of columns of matrix A if to calculate C:= \alpha*A*A^t + \beta *C; otherwise the number of rows of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The matrix A supplied column-by-column of dimension (<paramref name="lda" />, ka), where ka is <paramref name="k" /> if to calculate C:= \alpha*A*A^t + \beta *C; otherwise <paramref name="n" />.</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="c">The symmetric matrix C supplied column-by-column of dimension (<paramref name="ldc" />, <paramref name="n" />).</param>
        /// <param name="lda">The leading dimension of <paramref name="a" />, must be at least max(1,<paramref name="n" />) if to calculate C:= \alpha*A*A^t + \beta *C; max(1,<paramref name="k" />) otherwise.</param>
        /// <param name="ldc">The leading dimension of <paramref name="c" />, must be at least max(1,<paramref name="n" />).</param>
        /// <param name="triangularMatrixType">A value whether matrix C is in its upper or lower triangular representation.</param>
        /// <param name="operation">A value indicating whether to calculate C:= \alpha*A*A^t + \beta *C or C:= \alpha*A^t*A + \beta*C.</param>
        public void zsyrk(int n, int k, Complex alpha, Complex[] a, Complex beta, Complex[] c, int lda, int ldc, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.XsyrkOperation operation = BLAS.XsyrkOperation.ATimesATranspose)
        {
            if (operation == BLAS.XsyrkOperation.ATimesATranspose)
            {
                if (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix)
                {
                    for (int j = 0; j < n; j++)
                    {
                        for (int i = 0; i <= j; i++)
                        {
                            c[i + j * ldc] *= beta;
                        }
                        for (int ell = 0; ell < k; ell++)
                        {
                            Complex temp = alpha * a[j + ell * lda];
                            for (int i = 0; i <= j; i++)
                            {
                                c[i + j * ldc] += temp * a[i + ell * lda];
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
                            c[i + j * ldc] *= beta;
                        }
                        for (int ell = 0; ell < k; ell++)
                        {
                            Complex temp = alpha * a[j + ell * lda];
                            for (int i = j; i < n; i++)
                            {
                                c[i + j * ldc] += temp * a[i + ell * lda];
                            }
                        }
                    }
                }
            }
            else  // C:= \alpha * A' * A + \beta * C
            {
                if (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix)
                {
                    for (int j = 0; j < n; j++)
                    {
                        for (int i = 0; i <= j; i++)
                        {
                            Complex temp = 0.0;
                            for (int ell = 0; ell < k; ell++)
                            {
                                temp += a[ell + i * lda] * a[ell + j * lda];
                            }
                            c[i + j * ldc] = alpha * temp + beta * c[i + j * ldc];
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
                            for (int ell = 0; ell < k; ell++)
                            {
                                temp += a[ell + i * lda] * a[ell + j * lda];
                            }
                            c[i + j * ldc] = alpha * temp + beta * c[i + j * ldc];
                        }
                    }
                }
            }
        }
    }
}