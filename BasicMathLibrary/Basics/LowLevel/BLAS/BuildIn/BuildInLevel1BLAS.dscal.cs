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
using System.Runtime.CompilerServices;

using Dodoni.MathLibrary.Basics;
using Dodoni.MathLibrary.Basics.LowLevel;

namespace Dodoni.MathLibrary.Basics.LowLevel.BuildIn
{
    /// <summary>Serves as managed code implementation of BLAS level 1 operations.
    /// </summary>
    /// <remarks>Some of the methods are straightforward ports of the Fortran implementation (http://www.netlib.org/blas). It is recommended to use wrapper of a native code implementation.</remarks>
    internal partial class BuildInLevel1BLAS
    {
        /// <summary>Computes the product of a vector by a scalar, i.e. x = a * x.
        /// </summary>
        /// <param name="n">The number of elements of <paramref name="x"/>.</param>
        /// <param name="a">The scalar factor 'a'.</param>
        /// <param name="x">The vector 'x' with at least <paramref name="n"/> elements, contains the updated vector after function call.</param>
        public void dscal(int n, double a, double[] x)
        {
            dscal(a, x);
        }

        /// <summary>Computes the product of a vector by a scalar, i.e. x[iStart + j * increment] = a * x[iStart + j * increment], for j = 0,..., n-1.
        /// </summary>
        /// <param name="n">The number of elements of <paramref name="x"/>.</param>
        /// <param name="a">The scalar factor 'a'.</param>
        /// <param name="x">The vector 'x' with at least <paramref name="startIndex"/> + 1 + (<paramref name="n"/> -1) * <paramref name="increment"/> elements.</param>
        /// <param name="startIndex">The null-based start index.</param>
        /// <param name="increment">The increment.</param>
        public void dscal(int n, double a, double[] x, int increment, int startIndex)
        {
            for (int j = 0; j < n; j++)
            {
                x[startIndex + j * increment] *= a;
            }
        }

        /// <summary>Computes the product of a vector by a scalar, i.e. x = a * x.
        /// </summary>
        /// <param name="a">The scalar factor 'a'.</param>
        /// <param name="x">The vector 'x', contains the updated vector after function call.</param>
        public void dscal(double a, Span<double> x)
        {
            for (int j = 0; j < x.Length; j++)
            {
                x[j] *= a;
            }
        }
    }
}