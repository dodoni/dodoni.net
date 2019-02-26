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
    /// <summary>Serves as managed code implementation of BLAS level 1 operations.
    /// </summary>
    /// <remarks>Some of the methods are straightforward ports of the Fortran implementation (http://www.netlib.org/blas). It is recommended to use wrapper of a native code implementation.</remarks>
    internal partial class BuildInLevel1BLAS
    {
        /// <summary>Computes a vector-vector dot product, i.e. \sum_j x_{j * <paramref name="incX" />} * y_{j * <paramref name="incY" />).
        /// </summary>
        /// <param name="n">The number of elements of <paramref name="x" /> and <paramref name="y" />.</param>
        /// <param name="x">The vector 'x' with at least <paramref name="n" /> elements.</param>
        /// <param name="y">The vector 'y' with at least <paramref name="n" /> elements.</param>
        /// <param name="incX">The increment for <paramref name="x" />.</param>
        /// <param name="incY">The increment for <paramref name="y" />.</param>
        /// <returns>The dot product of <paramref name="x" /> and <paramref name="y" />, i.e. \sum_j x_{j * <paramref name="incX" />} * y_{j * <paramref name="incY" />).</returns>
        public double ddot(int n, ReadOnlySpan<double> x, ReadOnlySpan<double> y, int incX = 1, int incY = 1)
        {
            double value = 0.0;
            for (int j = 0; j < n; j++)
            {
                value += x[j * incX] * y[j * incY];
            }
            return value;
        }
    }
}