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
        /// <summary>Performs a rank-1 update of a Hermitian packed matrix, i.e. A := \alpha * x * conjg(x^t) + A.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n" />-1) * |<paramref name="incX" />| elements.</param>
        /// <param name="aPacked">The Hermitian packed matrix A with dimension at least (<paramref name="n" /> * (<paramref name="n" /> + 1) ) / 2.</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x" />.</param>
        public void zhpr(int n, double alpha, ReadOnlySpan<Complex> x, Span<Complex> aPacked, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1)
        {
            if (n == 0 || alpha == 0.0)
            {
                return;
            }

            int kx = 1;
            if (incX <= 0)
            {
                kx = 1 - (n - 1) * incX;
            }
            int kk = 1;

            if (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix)
            {
                int jx = kx;
                for (int j = 1; j <= n; j++)
                {
                    Complex temp = alpha * Complex.Conjugate(x[jx - 1]);
                    int ix = kx;
                    for (int k = kk; k <= kk + j - 2; k++)
                    {
                        aPacked[k - 1] += x[ix - 1] * temp;
                        ix += incX;
                    }
                    aPacked[kk + j - 2] = (aPacked[kk + j - 2] + x[jx - 1] * temp).Real;
                    jx += incX;
                    kk += j;
                }
            }
            else
            {
                int jx = kx;
                for (int j = 1; j <= n; j++)
                {
                    Complex temp = alpha * Complex.Conjugate(x[jx - 1]);

                    aPacked[kk - 1] = (aPacked[kk - 1] + temp * x[jx - 1]).Real;
                    int ix = jx;
                    for (int k = kk + 1; k <= kk + n - j; k++)
                    {
                        ix += incX;
                        aPacked[k - 1] += x[ix - 1] * temp;
                    }
                    jx += incX;
                    kk = kk + n - j + 1;
                }
            }
        }
    }
}