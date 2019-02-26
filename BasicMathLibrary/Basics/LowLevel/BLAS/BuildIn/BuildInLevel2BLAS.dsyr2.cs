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
        /// <summary>Performs a rank-2 update of a symmetric matrix, i.e. A := \alpha * x * y^t + \alpha * y * x^t + A.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n" /> - 1) * | <paramref name="incX" /> | elements.</param>
        /// <param name="y">The vector y with at least 1 + (<paramref name="n" /> - 1) * | <paramref name="incY" /> | elements.</param>
        /// <param name="a">The symmetric matrix A of dimension (<paramref name="lda" />, <paramref name="n" />) supplied column-by-column (input/output).</param>
        /// <param name="lda">The leading dimension of <paramref name="a" />, must be at least max(1, <paramref name="n" />).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x" />.</param>
        /// <param name="incY">The increment for the elements of <paramref name="y" />.</param>
        public void dsyr2(int n, double alpha, ReadOnlySpan<double> x, ReadOnlySpan<double> y, Span<double> a, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1, int incY = 1)
        {
            if (n == 0 || alpha == 0.0)
            {
                return;
            }
            int kx = 1;
            int ky = 1;
            int jx = 1;
            int jy = 1;

            if (incX != 1 || incY != 1)
            {
                if (incX <= 0)
                {
                    kx = 1 - (n - 1) * incX;
                }
                if (incY <= 0)
                {
                    ky = 1 - (n - 1) * incY;
                }
                jx = kx;
                jy = ky;
            }

            if (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix)
            {
                for (int j = 1; j <= n; j++)
                {
                    double temp1 = alpha * y[jy - 1];
                    double temp2 = alpha * x[jx - 1];
                    int ix = kx;
                    int iy = ky;

                    for (int i = 1; i <= j; i++)
                    {
                        a[i - 1 + (j - 1) * lda] = a[i - 1 + (j - 1) * lda] + x[ix - 1] * temp1 + y[iy - 1] * temp2;
                        ix += incX;
                        iy += incY;
                    }
                    jx += incX;
                    jy += incY;
                }
            }
            else
            {
                for (int j = 1; j <= n; ++j)
                {
                    double temp1 = alpha * y[jy - 1];
                    double temp2 = alpha * x[jx - 1];
                    int ix = jx;
                    int iy = jy;

                    for (int i = j; i <= n; i++)
                    {
                        a[i - 1 + (j - 1) * lda] = a[i - 1 + (j - 1) * lda] + x[ix - 1] * temp1 + y[iy - 1] * temp2;
                        ix += incX;
                        iy += incY;
                    }
                    jx += incX;
                    jy += incY;
                }
            }
        }
    }
}