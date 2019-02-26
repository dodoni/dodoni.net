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

using Dodoni.MathLibrary.Basics.LowLevel;

namespace Dodoni.MathLibrary.Basics
{
    public static partial class Extensions
    {
        /// <summary>Computes the square of the Euclidean norm of a specific vector, i.e. ||x||^2.
        /// </summary>
        /// <param name="level1">The BLAS level 1 implementation.</param>
        /// <param name="n">The number of elements of <paramref name="x"/>.</param>
        /// <param name="x">The vector 'x' with at least <paramref name="n"/> elements.</param>
        /// <param name="incX">The increment for <paramref name="x"/>.</param>
        /// <returns>The square of the euclidian norm of x, i.e. x_0^2 + ... + x_n^2.</returns>
        public static double dnrm2sq(this ILevel1BLAS level1, int n, Span<double> x, int incX = 1)
        {
            var norm = level1.dnrm2(n, x, incX);
            return norm * norm;
        }
    }
}