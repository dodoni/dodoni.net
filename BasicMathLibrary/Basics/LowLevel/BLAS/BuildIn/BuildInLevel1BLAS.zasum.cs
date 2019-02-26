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
        /// <summary>Compute the sum of the magnitudes of elements of some vector, i.e.
        /// <para>|Re x(0)|+ |Im x(0)| + |Re x(incX)| + |Im x(incX)| + |Re x(2 * incX)| + |Im x(2 * incX)| + ...  + |Re x(k * incX)| + |x(k * incX)|.</para>
        /// </summary>
        /// <param name="n">The number of elements in <paramref name="x" /> with at least  <c>(1 + (<paramref name="n" />-1)*abs(<paramref name="incX" />))</c> elements.</param>
        /// <param name="x">The vector.</param>
        /// <param name="incX">The increment for indexing vector <paramref name="x" />.</param>
        /// <returns>The sum of magnitudes of elements of the vector <paramref name="x" />.</returns>
        public double zasum(int n, ReadOnlySpan<Complex> x, int incX = 1)
        {
            double value = 0.0;

            for (int j = 0; j < n; j++)
            {
                Complex temp = x[j * incX];

                value += Math.Abs(temp.Real);
                value += Math.Abs(temp.Imaginary);
            }
            return value;
        }
    }
}