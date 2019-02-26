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
using System.Numerics;

namespace Dodoni.MathLibrary.Basics.LowLevel.BuildIn
{
    /// <summary>Serves as managed code implementation of BLAS level 2 operations.
    /// </summary>
    /// <remarks>Some of the methods are straightforward ports of the Fortran implementation (http://www.netlib.org/blas). It is recommended to use wrapper of a native code implementation.
    /// </remarks>
    internal partial class BuildInLevel2BLAS : ILevel2BLAS
    {
        /// <summary>Computes a matrix-vector product using a triangular packed matrix, i.e. x := op(A) * x, where op(A) = A, op(A) = A ^t or op(A) = A^h.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="aPacked">The triangular packed matrix A with dimension at least (<paramref name="n" /> * (<paramref name="n" /> + 1) ) / 2.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n" /> - 1) * | <paramref name="incX" /> | elements.</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A', 'op(A)=A^t' or 'op(A)=A^h'.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x" />.</param>
        public void ztpmv(int n, ReadOnlySpan<Complex> aPacked, Span<Complex> x, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1)
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

            if (transpose == BLAS.MatrixTransposeState.NoTranspose)  // x := A * x
            {
                if (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix)
                {
                    int kk = 1;

                    int jx = kx;
                    for (int j = 1; j <= n; j++)
                    {
                        Complex temp = x[jx - 1];
                        int ix = kx;
                        for (int k = kk; k <= kk + j - 2; k++)
                        {
                            x[ix - 1] += temp * aPacked[k - 1];
                            ix += incX;
                        }
                        if (isUnitTriangular == false)
                        {
                            x[jx - 1] *= aPacked[kk + j - 2];
                        }
                        jx += incX;
                        kk += j;
                    }
                }
                else
                {
                    int kk = n * (n + 1) / 2;

                    kx += (n - 1) * incX;
                    int jx = kx;
                    for (int j = n; j >= 1; j--)
                    {
                        Complex temp = x[jx - 1];
                        int ix = kx;
                        for (int k = kk; k >= kk - (n - (j + 1)); k--)
                        {
                            x[ix - 1] += temp * aPacked[k - 1];
                            ix -= incX;
                        }
                        if (isUnitTriangular == false)
                        {
                            x[jx - 1] *= aPacked[kk - n + j - 1];
                        }
                        jx -= incX;
                        kk -= n - j + 1;
                    }
                }
            }
            else if (transpose == BLAS.MatrixTransposeState.Transpose)  // x := A' * x
            {
                if (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix)
                {
                    int kk = n * (n + 1) / 2;

                    int jx = kx + (n - 1) * incX;
                    for (int j = n; j >= 1; j--)
                    {
                        Complex temp = x[jx - 1];
                        int ix = jx;
                        if (isUnitTriangular == false)
                        {
                            temp *= aPacked[kk - 1];
                        }
                        for (int k = kk - 1; k >= kk - j + 1; k--)
                        {
                            ix -= incX;
                            temp += aPacked[k - 1] * x[ix - 1];
                        }
                        x[jx - 1] = temp;
                        jx -= incX;
                        kk -= j;
                    }
                }
                else
                {
                    int kk = 1;

                    int jx = kx;
                    for (int j = 1; j <= n; j++)
                    {
                        Complex temp = x[jx - 1];
                        int ix = jx;
                        if (isUnitTriangular == false)
                        {
                            temp *= aPacked[kk - 1];
                        }
                        for (int k = kk + 1; k <= kk + n - j; k++)
                        {
                            ix += incX;
                            temp += aPacked[k - 1] * x[ix - 1];
                        }
                        x[jx - 1] = temp;
                        jx += incX;
                        kk += n - j + 1;
                    }
                }
            }
            else  // x := conj(A)' * x
            {
                if (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix)
                {
                    int kk = n * (n + 1) / 2;

                    int jx = kx + (n - 1) * incX;
                    for (int j = n; j >= 1; j--)
                    {
                        Complex temp = x[jx - 1];
                        int ix = jx;
                        if (isUnitTriangular == false)
                        {
                            temp *= Complex.Conjugate(aPacked[kk - 1]);
                        }
                        for (int k = kk - 1; k >= kk - j + 1; k--)
                        {
                            ix -= incX;
                            temp += Complex.Conjugate(aPacked[k - 1]) * x[ix - 1];
                        }
                        x[jx - 1] = temp;
                        jx -= incX;
                        kk -= j;
                    }
                }
                else
                {
                    int kk = 1;

                    int jx = kx;
                    for (int j = 1; j <= n; j++)
                    {
                        Complex temp = x[jx - 1];
                        int ix = jx;
                        if (isUnitTriangular == false)
                        {
                            temp *= Complex.Conjugate(aPacked[kk - 1]);
                        }
                        for (int k = kk + 1; k <= kk + n - j; k++)
                        {
                            ix += incX;
                            temp += Complex.Conjugate(aPacked[k - 1]) * x[ix - 1];
                        }
                        x[jx - 1] = temp;
                        jx += incX;
                        kk += n - j + 1;
                    }
                }
            }
        }
    }
}