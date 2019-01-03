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
        /// <summary>Computes a matrix-vector product for a general matrix, i.e. y = \alpha * op(A)*x + \beta*y, where op(A) = A, op(A) = A^t or op(A) = A^h.
        /// </summary>
        /// <param name="m">The number of rows of matrix A.</param>
        /// <param name="n">The number of columns of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The matrix A of dimension (<paramref name="lda" />, <paramref name="n" />) supplied column-by-column.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n" />-1) * |<paramref name="incX" />| elements if 'op(A)=A'; 1 + (<paramref name="m" />-1) * |<paramref name="incY" />| elements otherwise.</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="y">The vector y with at least 1 + (<paramref name="m" />-1) * |<paramref name="incY" />| elements if 'op(A)=A'; 1 + (<paramref name="n" />-1) * | <paramref name="incX" />| otherwise (input/output).</param>
        /// <param name="lda">The leading dimension of <paramref name="a" />, must be at least max(1,<paramref name="m" />).</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x" />.</param>
        /// <param name="incY">The increment for the elements of <paramref name="y" />.</param>
        public void zgemv(int m, int n, Complex alpha, Complex[] a, Complex[] x, Complex beta, Complex[] y, int lda, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1, int incY = 1)
        {
            if (m == 0 || n == 0 || alpha == 0.0 && beta == 1.0)
            {
                return;
            }

            int lenx, leny;
            if (transpose == BLAS.MatrixTransposeState.NoTranspose)
            {
                lenx = n;
                leny = m;
            }
            else
            {
                lenx = m;
                leny = n;
            }
            int kx = 1;
            int ky = 1;

            if (incX <= 0)
            {
                kx = 1 - (lenx - 1) * incX;
            }
            if (incY <= 0)
            {
                ky = 1 - (leny - 1) * incY;
            }


            // y := \beta * y
            int iy = ky;
            for (int i = 1; i <= leny; i++)
            {
                y[iy - 1] *= beta;
                iy += incY;
            }

            if (transpose == BLAS.MatrixTransposeState.NoTranspose)  // y := \alpha * A * x + y
            {
                int jx = kx;
                for (int j = 1; j <= n; ++j)
                {
                    Complex temp = alpha * x[jx - 1];
                    iy = ky;

                    for (int i = 1; i <= m; ++i)
                    {
                        y[iy - 1] += temp * a[i - 1 + (j - 1) * lda];
                        iy += incY;
                    }
                    jx += incX;
                }
            }
            else if (transpose == BLAS.MatrixTransposeState.Transpose) // y := \alpha * A' *x + y  
            {
                int jy = ky;
                for (int j = 1; j <= n; j++)
                {
                    Complex temp = 0.0;
                    int ix = kx;
                    for (int i = 1; i <= m; i++)
                    {
                        temp += a[i - 1 + (j - 1) * lda] * x[ix - 1];
                        ix += incX;
                    }
                    y[jy - 1] += alpha * temp;
                    jy += incY;
                }
            }
            else // y := \alpha *conj(A)' * x + y
            {
                int jy = ky;
                for (int j = 1; j <= n; j++)
                {
                    Complex temp = 0.0;
                    int ix = kx;
                    for (int i = 1; i <= m; i++)
                    {
                        temp += Complex.Conjugate(a[i - 1 + (j - 1) * lda]) * x[ix - 1];
                        ix += incX;
                    }
                    y[jy - 1] += alpha * temp;
                    jy += incY;
                }
            }
        }
    }
}