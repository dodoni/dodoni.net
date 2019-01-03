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
    /// <remarks>Some of the methods are straightforward ports of the Fortran implementation (http://www.netlib.org/blas). It is recommended to use wrapper of a native code implementation.</remarks>
    internal partial class BuildInLevel2BLAS : ILevel2BLAS
    {
        /// <summary>Solves a system of linear equations whose coefficients are in a triangular matrix, i.e. op(A) * x = b, where op(A) = A or op(A) = A^t.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="a">The triangular matrix A with dimension (<paramref name="lda" />, <paramref name="n" />).</param>
        /// <param name="x">The vector b (input), x (output) with at least 1 + (<paramref name="n" /> - 1) * | <paramref name="incX" /> | elements (input/output).</param>
        /// <param name="lda">The leading dimension of <paramref name="a" />, must be at least max(1, <paramref name="n" />).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
        /// <param name="transpose">A value indicating whether op(A) = A or op(A) = A^t.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x" />.</param>
        public void dtrsv(int n, double[] a, double[] x, int lda, BLAS.TriangularMatrixType triangularMatrixType, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1)
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

            if (transpose == BLAS.MatrixTransposeState.NoTranspose) // x := Inv( A ) *x
            {
                if (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix)
                {
                    int jx = kx + (n - 1) * incX;
                    for (int j = n; j >= 1; j--)
                    {
                        if (isUnitTriangular == false)
                        {
                            x[jx - 1] /= a[j - 1 + (j - 1) * lda];
                        }
                        double temp = x[jx - 1];
                        int ix = jx;
                        for (int i = j - 1; i >= 1; i--)
                        {
                            ix -= incX;
                            x[ix - 1] -= temp * a[i - 1 + (j - 1) * lda];
                        }
                        jx -= incX;
                    }
                }
                else
                {
                    int jx = kx;
                    for (int j = 1; j <= n; j++)
                    {
                        if (isUnitTriangular == false)
                        {
                            x[jx - 1] /= a[j - 1 + (j - 1) * lda];
                        }
                        double temp = x[jx - 1];
                        int ix = jx;
                        for (int i = j + 1; i <= n; i++)
                        {
                            ix += incX;
                            x[ix - 1] -= temp * a[i - 1 + (j - 1) * lda];
                        }
                        jx += incX;
                    }
                }
            }
            else  // x := Inv( A' ) * x
            {
                if (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix)
                {
                    int jx = kx;
                    for (int j = 1; j <= n; j++)
                    {
                        double temp = x[jx - 1];
                        int ix = kx;
                        for (int i = 1; i <= j - 1; i++)
                        {
                            temp -= a[i - 1 + (j - 1) * lda] * x[ix - 1];
                            ix += incX;
                        }
                        if (isUnitTriangular == false)
                        {
                            temp /= a[j - 1 + (j - 1) * lda];
                        }
                        x[jx - 1] = temp;
                        jx += incX;
                    }
                }
                else
                {
                    kx += (n - 1) * incX;
                    int jx = kx;
                    for (int j = n; j >= 1; j--)
                    {
                        double temp = x[jx - 1];
                        int ix = kx;
                        for (int i = n; i >= j + 1; i--)
                        {
                            temp -= a[i - 1 + (j - 1) * lda] * x[ix - 1];
                            ix -= incX;
                        }
                        if (isUnitTriangular == false)
                        {
                            temp /= a[j - 1 + (j - 1) * lda];
                        }
                        x[jx - 1] = temp;
                        jx -= incX;
                    }
                }
            }
        }
    }
}