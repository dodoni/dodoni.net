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
        /// <summary>Computes a matrix-vector product using a general band matrix, i.e. y := \alpha * op(A) * x + \beta * y, where op(A) = A or op(A) = A^t.
        /// </summary>
        /// <param name="m">The number of rows of matrix A.</param>
        /// <param name="n">The number of columns of matrix A.</param>
        /// <param name="kl">The number of sub-diagonals of matrix A.</param>
        /// <param name="ku">The number of super-diagonals of matrix A.</param>
        /// <param name="alpha">The scalar factor \alpha.</param>
        /// <param name="a">The general band matrix A of dimension (<paramref name="lda" />, <paramref name="n" />). The leading (<paramref name="kl" /> + <paramref name="ku" /> + 1) by <paramref name="n" /> part
        /// must contain the matrix of coefficients (supplied column-by-column) with the leading diagonal of the matrix
        /// in row (<paramref name="ku" /> + 1) of the array, the first super-diagonal starting at position 2 in row ku,
        /// the first sub-diagonal starting at position 1 in row (ku + 2), etc.
        /// The following program segment transfers a band matrix from conventional full matrix storage to band storage:
        /// <code>
        /// for j = 0 to n-1
        /// k = ku - j
        /// for i = max(0, j-ku) to min(m-1, j+kl-1)
        /// a(k+i, j) = matrix(i,j)
        /// end
        /// end
        /// </code></param>
        /// <param name="x">The vector x with at least 1+(<paramref name="n" />-1) * |<paramref name="incX" />| elements if 'op(A)=A'; 1+(<paramref name="m" />-1) *|<paramref name="incX" />| otherwise.</param>
        /// <param name="beta">The scalar factor \beta.</param>
        /// <param name="y">The vector y with at least 1+(<paramref name="m" />-1) *|<paramref name="incY" />| elements if 'op(A)=A'; 1+(<paramref name="n" />-1) * |<paramref name="incY" />| otherwise (input/output).</param>
        /// <param name="lda">The leading dimension, must be at least <paramref name="kl" /> + <paramref name="ku" /> +1.</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x" />.</param>
        /// <param name="incY">The increment for the elements of <paramref name="y" />.</param>
        public void dgbmv(int m, int n, int kl, int ku, double alpha, double[] a, double[] x, double beta, double[] y, int lda, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1, int incY = 1)
        {
            if (m == 0 || n == 0 || alpha == 0.0 && beta == 1.0)
            {
                return;
            }

            int lenX, lenY;
            if (transpose == BLAS.MatrixTransposeState.NoTranspose)
            {
                lenX = n;
                lenY = m;
            }
            else
            {
                lenX = m;
                lenY = n;
            }
            int kx = 1;
            int ky = 1;

            if (incX <= 0)
            {
                kx = 1 - (lenX - 1) * incX;
            }
            if (incY <= 0)
            {
                ky = 1 - (lenY - 1) * incY;
            }

            /* calculate y := \beta * y: */
            int iy = ky;
            for (int i = 1; i <= lenY; i++)
            {
                y[iy - 1] *= beta;
                iy += incY;
            }

            int kup1 = ku + 1;
            if (transpose == BLAS.MatrixTransposeState.NoTranspose)  // calculate y:= \alpha *A*x + y:
            {
                int jx = kx;
                for (int j = 1; j <= n; j++)
                {
                    double temp = alpha * x[jx - 1];
                    iy = ky;
                    int k = kup1 - j;
                    for (int i = Math.Max(1, j - ku); i <= Math.Min(m, j + kl); i++)
                    {
                        y[iy - 1] += temp * a[k + i - 1 + lda * (j - 1)];
                        iy += incY;
                    }
                    jx += incX;
                    if (j > ku)
                    {
                        ky += incY;
                    }
                }
            }
            else  // calculate y := \alpha *A'*x + y:
            {
                int jy = ky;
                for (int j = 1; j <= n; j++)
                {
                    double temp = 0.0;
                    int ix = kx;
                    int k = kup1 - j;
                    for (int i = Math.Max(1, j - ku); i <= Math.Min(m, j + kl); i++)
                    {
                        temp += a[k + i - 1 + (j - 1) * lda] * x[ix - 1];
                        ix += incX;
                    }
                    y[jy - 1] += alpha * temp;
                    jy += incY;
                    if (j > ku)
                    {
                        kx += incX;
                    }
                }
            }
        }
    }
}