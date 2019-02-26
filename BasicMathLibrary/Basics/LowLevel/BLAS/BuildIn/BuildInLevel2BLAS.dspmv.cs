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

namespace Dodoni.MathLibrary.Basics.LowLevel.BuildIn
{
    /// <summary>Serves as managed code implementation of BLAS level 2 operations.
    /// </summary>
    /// <remarks>Some of the methods are straightforward ports of the Fortran implementation (http://www.netlib.org/blas). It is recommended to use wrapper of a native code implementation.</remarks>
    internal partial class BuildInLevel2BLAS : ILevel2BLAS
    {
        /// <summary>Computes a matrix-vector product using a symmetric packed matrix, i.e. y := \alpha * A * x + \beta * y.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="aPacked">The symmetric packed matrix A with dimension at least (<paramref name="n" /> * (<paramref name="n" /> + 1) ) / 2, i.e. in the upper triangular representation: aPacked[0] = a[0,0], aPacked[1] = a[0,1], aPacked[2] = a[1,1]; otherwise aPacked[0] = a[0,0], aPacked[1] = a[1,0], aPacked[2] = a[2,0] etc.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n" /> - 1) * | <paramref name="incX" /> | elements.</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="y">The vector y with at least 1 + (<paramref name="n" /> - 1) * | <paramref name="incY" /> | elements (input/output).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x" />.</param>
        /// <param name="incY">The increment for the elements of <paramref name="y" />.</param>
        public void dspmv(int n, double alpha, ReadOnlySpan<double> aPacked, ReadOnlySpan<double> x, double beta, Span<double> y, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1, int incY = 1)
        {
            if (n == 0 || alpha == 0.0 && beta == 1.0)
            {
                return;
            }

            int kx = 1;
            int ky = 1;

            if (incX <= 0)
            {
                kx = 1 - (n - 1) * incX;
            }
            if (incY <= 0)
            {
                ky = 1 - (n - 1) * incY;
            }

            /* compute y:= \beta *y: */

            int iy = ky;
            for (int i = 1; i <= n; i++)
            {
                y[iy - 1] = beta * y[iy - 1];
                iy += incY;
            }

            int kk = 1;
            if (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix)
            {
                int jx = kx;
                int jy = ky;
                for (int j = 1; j <= n; j++)
                {
                    double temp1 = alpha * x[jx - 1];
                    double temp2 = 0.0;
                    int ix = kx;
                    iy = ky;
                    for (int k = kk; k <= kk + j - 2; k++)
                    {
                        y[iy - 1] += temp1 * aPacked[k - 1];
                        temp2 += aPacked[k - 1] * x[ix - 1];
                        ix += incX;
                        iy += incY;
                    }
                    y[jy - 1] = y[jy - 1] + temp1 * aPacked[kk + j - 2] + alpha * temp2;
                    jx += incX;
                    jy += incY;
                    kk += j;
                }
            }
            else
            {
                int jx = kx;
                int jy = ky;
                for (int j = 1; j <= n; j++)
                {
                    double temp1 = alpha * x[jx - 1];
                    double temp2 = 0.0;
                    y[jy - 1] += temp1 * aPacked[kk - 1];
                    int ix = jx;
                    iy = jy;
                    for (int k = kk + 1; k <= kk + n - j; k++)
                    {
                        ix += incX;
                        iy += incY;
                        y[iy - 1] += temp1 * aPacked[k - 1];
                        temp2 += aPacked[k - 1] * x[ix - 1];
                    }
                    y[jy - 1] += alpha * temp2;
                    jx += incX;
                    jy += incY;
                    kk += n - j + 1;
                }
            }
        }
    }
}