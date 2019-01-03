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
    /// <summary>Serves as managed code implementation of BLAS level 2 operations.
    /// </summary>
    /// <remarks>Some of the methods are straightforward ports of the Fortran implementation (http://www.netlib.org/blas). It is recommended to use wrapper of a native code implementation.
    /// </remarks>
    internal partial class BuildInLevel2BLAS : ILevel2BLAS
    {
        /// <summary>Solves a system of linear equations whose coefficients are in a triangular band matrix, i.e. op(A) * x = b, where op(A) = A, op(A) = A ^t or op(A) = A^h.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="k">The number of super-diagonals of A if the matrix A is provided in its upper triangular representation; the number of sub-diagonals otherwise.</param>
        /// <param name="a">The triangular band matrix with dimension (<paramref name="lda" />, <paramref name="n" />).</param>
        /// <param name="x">The vector b (input), x (output) with at least 1 + (<paramref name="n" /> - 1) * | <paramref name="incX" /> | elements (input/output).</param>
        /// <param name="lda">The leading dimension of <paramref name="a" />, must be at least (1  + <paramref name="k" />).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A', 'op(A)=A^t' or 'op(A)=A^h'.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x" />.</param>
        public void ztbsv(int n, int k, Complex[] a, Complex[] x, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1)
        {
            if (n == 0)
            {
                return;
            }
            int kx = 1;
            if (incX <= 0)
            {
                kx = 1 - (n - 1) * incX;
            }
            else if (incX != 1)
            {
                kx = 1;
            }

            if (transpose == BLAS.MatrixTransposeState.NoTranspose)  // x := Inv( A ) * x
            {
                if (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix)
                {
                    kx += (n - 1) * incX;
                    int jx = kx;
                    for (int j = n; j >= 1; j--)
                    {
                        kx -= incX;
                        int ix = kx;
                        int ell = k + 1 - j;
                        if (isUnitTriangular == false)
                        {
                            x[jx - 1] /= a[k + (j - 1) * lda];
                        }
                        Complex temp = x[jx - 1];

                        for (int i = j - 1; i >= Math.Max(1, j - k); i--)
                        {
                            x[ix - 1] -= temp * a[ell + i - 1 + (j - 1) * lda];
                            ix -= incX;
                        }
                        jx -= incX;
                    }
                }
                else
                {
                    int jx = kx;
                    for (int j = 1; j <= n; j++)
                    {
                        kx += incX;
                        int ix = kx;
                        int ell = 1 - j;
                        if (isUnitTriangular == false)
                        {
                            x[jx - 1] /= a[(j - 1) * lda];
                        }
                        Complex temp = x[jx - 1];

                        for (int i = j + 1; i <= Math.Min(n, j + k); i++)
                        {
                            x[ix - 1] -= temp * a[ell + i - 1 + (j - 1) * lda];
                            ix += incX;
                        }
                        jx += incX;
                    }
                }
            }
            else if (transpose == BLAS.MatrixTransposeState.Transpose) // x := Inv( A' ) * x
            {
                if (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix)
                {
                    int jx = kx;
                    for (int j = 1; j <= n; j++)
                    {
                        Complex temp = x[jx - 1];
                        int ix = kx;
                        int ell = k + 1 - j;

                        for (int i = Math.Max(1, j - k); i <= j - 1; i++)
                        {
                            temp -= a[ell + i - 1 + (j - 1) * lda] * x[ix - 1];
                            ix += incX;
                        }
                        if (isUnitTriangular == false)
                        {
                            temp /= a[k + (j - 1) * lda];
                        }
                        x[jx - 1] = temp;
                        jx += incX;
                        if (j > k)
                        {
                            kx += incX;
                        }
                    }
                }
                else
                {
                    kx += (n - 1) * incX;
                    int jx = kx;

                    for (int j = n; j >= 1; j--)
                    {
                        Complex temp = x[jx - 1];
                        int ix = kx;
                        int ell = 1 - j;

                        for (int i = Math.Min(n, j + k); i >= j + 1; i--)
                        {
                            temp -= a[ell + i - 1 + (j - 1) * lda] * x[ix - 1];
                            ix -= incX;
                        }
                        if (isUnitTriangular == false)
                        {
                            temp /= a[(j - 1) * lda];
                        }
                        x[jx - 1] = temp;
                        jx -= incX;
                        if (n - j >= k)
                        {
                            kx -= incX;
                        }
                    }
                }
            }
            else // x := Inv( conj(A)' ) * x
            {
                if (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix)
                {
                    int jx = kx;
                    for (int j = 1; j <= n; j++)
                    {
                        Complex temp = x[jx - 1];
                        int ix = kx;
                        int ell = k + 1 - j;

                        for (int i = Math.Max(1, j - k); i <= j - 1; i++)
                        {
                            temp -= Complex.Conjugate(a[ell + i - 1 + (j - 1) * lda]) * x[ix - 1];
                            ix += incX;
                        }
                        if (isUnitTriangular == false)
                        {
                            temp /= Complex.Conjugate(a[k + (j - 1) * lda]);
                        }
                        x[jx - 1] = temp;
                        jx += incX;
                        if (j > k)
                        {
                            kx += incX;
                        }
                    }
                }
                else
                {
                    kx += (n - 1) * incX;
                    int jx = kx;

                    for (int j = n; j >= 1; j--)
                    {
                        Complex temp = x[jx - 1];
                        int ix = kx;
                        int ell = 1 - j;

                        for (int i = Math.Min(n, j + k); i >= j + 1; i--)
                        {
                            temp -= Complex.Conjugate(a[ell + i - 1 + (j - 1) * lda]) * x[ix - 1];
                            ix -= incX;
                        }
                        if (isUnitTriangular == false)
                        {
                            temp /= Complex.Conjugate(a[(j - 1) * lda]);
                        }
                        x[jx - 1] = temp;
                        jx -= incX;
                        if (n - j >= k)
                        {
                            kx -= incX;
                        }
                    }
                }
            }
        }
    }
}