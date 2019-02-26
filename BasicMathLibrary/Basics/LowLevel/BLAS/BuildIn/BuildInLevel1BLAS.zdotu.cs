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
    /// <summary>Serves as managed code implementation of BLAS level 1 operations.
    /// </summary>
    /// <remarks>Some of the methods are straightforward ports of the Fortran implementation (http://www.netlib.org/blas). It is recommended to use wrapper of a native code implementation.</remarks>
    internal partial class BuildInLevel1BLAS
    {
        /// <summary>Computes a vector-vector product, i.e. \sum x * y.
        /// </summary>
        /// <param name="n">The number of elements of <paramref name="x" /> and <paramref name="y" />.</param>
        /// <param name="x">The vector 'x' with at least <paramref name="n" /> elements.</param>
        /// <param name="y">The vector 'y' with at least <paramref name="n" /> elements.</param>
        /// <param name="incX">The increment for <paramref name="x" />.</param>
        /// <param name="incY">The increment for <paramref name="y" />.</param>
        /// <returns>The vector-vector product of <paramref name="x" /> and <paramref name="y" />, i.e. \sum x * y.</returns>
        public Complex zdotu(int n, ReadOnlySpan<Complex> x, ReadOnlySpan<Complex> y, int incX = 1, int incY = 1)
        {
            if (n <= 0)
            {
                return Complex.Zero;
            }

            Complex value = Complex.Zero;
            if (incX == 1 && incY == 1)
            {
                for (int i = 0; i < n; i++)
                {
                    value = value + x[i] * y[i];
                }
            }
            else
            {
                int ix = (incX >= 0) ? 0 : (-n + 1) * incX;
                int iy = (incY >= 0) ? 0 : (-n + 1) * incY;

                for (int i = 0; i < n; i++)
                {
                    value = value + x[ix] * y[iy];
                    ix = ix + incX;
                    iy = iy + incY;
                }
            }
            return value;
        }
    }
}