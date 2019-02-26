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
        /// <summary>Performs a rank-1 update (conjuaged) of a general matrix, i.e. A := \alpha * x * conj(y^t) + A.
        /// </summary>
        /// <param name="m">The number of rows of matrix A.</param>
        /// <param name="n">The number of columns of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="m" />-1) * |<paramref name="incX" />| elements.</param>
        /// <param name="y">The vector y with at least 1 + (<paramref name="n" />-1) * |<paramref name="incY" />| elements.</param>
        /// <param name="a">The matrix A of dimension (<paramref name="lda" />, <paramref name="n" />) supplied column-by-column.</param>
        /// <param name="lda">The leading dimension of <paramref name="a" />, must be at least max(1,<paramref name="m" />).</param>
        /// <param name="incX">The increment for the elements of <paramref name="x" />.</param>
        /// <param name="incY">The increment for the elements of <paramref name="y" />.</param>
        public void zgerc(int m, int n, Complex alpha, ReadOnlySpan<Complex> x, ReadOnlySpan<Complex> y, Span<Complex> a, int lda, int incX = 1, int incY = 1)
        {
            if (m == 0 || n == 0 || alpha == 0.0)
            {
                return;
            }
            int jy = 0;
            int kx = 0;
            for (int j = 0; j < n; j++)
            {
                Complex temp = alpha * Complex.Conjugate(y[jy]);
                int ix = kx;
                for (int i = 0; i < m; i++)
                {
                    a[i + j * lda] += x[ix] * temp;
                    ix += incX;
                }
                jy += incY;
            }
        }
    }
}