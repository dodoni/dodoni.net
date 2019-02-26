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
    /// <summary>A straightforward implementation of level 3 sparse BLAS routines.
    /// </summary>    
    internal class BuildInLevel3SparseBLAS : ILevel3SparseBLAS
    {
        /// <summary>Perform the matrix-matrix operation 'C = \alpha*A*B + \beta*C', where A,B,C are general band matrices.
        /// </summary>
        /// <param name="m">The number of rows of matrix A.</param>
        /// <param name="n">The number of columns of matrix B.</param>
        /// <param name="k">The number of columns of matrix A and the number of rows of matrix B.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="klA">Specifies the number of sub-diagonals of the matrix A.</param>
        /// <param name="kuA">Specifies the number of super-diagonals of the matrix A.</param>
        /// <param name="a">The general band matrix A given column-by-column, i.e. at least (<paramref name="klA"/> + <paramref name="kuA"/> + 1) * <paramref name="k"/> elements.</param>
        /// <param name="klB">Specifies the number of sub-diagonals of the matrix B.</param>
        /// <param name="kuB">Specifies the number of super-diagonals of the matrix B.</param>
        /// <param name="b">The general band matrix B given column-by-column, i.e. at least (<paramref name="klB"/> + <paramref name="kuB"/> + 1) * <paramref name="n"/> elements.</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="c">The general band matrix C given column-by-column where the number of sub-diagonals is klC and the number of
        /// super-diagonals is kuC, where klC = max { <paramref name="klA"/> - <paramref name="kuB"/>, 0} and 
        /// kuC = max { <paramref name="kuA"/> - <paramref name="klB"/>, 0}, i.e. at least (klC + kuC + 1) * <paramref name="n"/> elements (output).</param>
        /// <remarks>The general band matrix storage is given in the same way as in BLAS level 3, i.e. perhaps for the first/last
        /// column the array contains more elements than necessary.</remarks>
        public void dgbmm(int m, int n, int k, double alpha, int klA, int kuA, ReadOnlySpan<double> a, int klB, int kuB, ReadOnlySpan<double> b, double beta, Span<double> c)
        {
            // we do not need 'm' and 'k' here, perhaps for some later implementation it seems to be reasonable to pass these parameters as well

            int klC = Math.Max(0, klA - kuB);
            int kuC = Math.Max(0, kuA - klB);

            int uA = kuA + klA + 1;
            int uB = kuB + klB + 1;
            int uC = kuC + klC + 1;

            for (int j = 0; j < n; j++)  // ToDo: kann parallelisiert werden
            {
                for (int i = j - kuC; i < j + klC; i++)
                {
                    double value = 0;
                    for (int r = Math.Max(i - klA, j - kuB); r < Math.Min(i + kuA, j + klB); r++)
                    {
                        value += a[i - k + kuA + k * uA] * b[k - j + kuB + j * uB];
                    }
                    c[i - j + kuC + j * uC] = alpha * value + beta * c[i - j + kuC + j * uC];
                }
            }
        }

        /// <summary>Perform the matrix-matrix operation 'C = \alpha*A*B', where A,B,C are general band matrices.
        /// </summary>
        /// <param name="m">The number of rows of matrix A.</param>
        /// <param name="n">The number of columns of matrix B.</param>
        /// <param name="k">The number of columns of matrix A and the number of rows of matrix B.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="klA">Specifies the number of sub-diagonals of the matrix A.</param>
        /// <param name="kuA">Specifies the number of super-diagonals of the matrix A.</param>
        /// <param name="a">The general band matrix A given column-by-column, i.e. at least (<paramref name="klA"/> + <paramref name="kuA"/> + 1) * <paramref name="k"/> elements.</param>
        /// <param name="klB">Specifies the number of sub-diagonals of the matrix B.</param>
        /// <param name="kuB">Specifies the number of super-diagonals of the matrix B.</param>
        /// <param name="b">The general band matrix B given column-by-column, i.e. at least (<paramref name="klB"/> + <paramref name="kuB"/> + 1) * <paramref name="n"/> elements.</param>
        /// <param name="c">The general band matrix C given column-by-column where the number of sub-diagonals is klC and the number of
        /// super-diagonals is kuC, where klC = max { <paramref name="klA"/> - <paramref name="kuB"/>, 0} and 
        /// kuC = max { <paramref name="kuA"/> - <paramref name="klB"/>, 0}, i.e. at least (klC + kuC + 1) * <paramref name="n"/> elements (output).</param>
        /// <remarks>The general band matrix storage is given in the same way as in BLAS level 3, i.e. perhaps for the first/last
        /// column the array contains more elements than necessary.</remarks>
        public void dgbmm(int m, int n, int k, double alpha, int klA, int kuA, ReadOnlySpan<double> a, int klB, int kuB, ReadOnlySpan<double> b, Span<double> c)
        {
            // we do not need 'm' and 'k' here, perhaps for some later implementation it seems to be reasonable to pass these parameters as well

            int klC = Math.Max(0, klA - kuB);
            int kuC = Math.Max(0, kuA - klB);

            int uA = kuA + klA + 1;
            int uB = kuB + klB + 1;
            int uC = kuC + klC + 1;

            for (int j = 0; j < n; j++)  // ToDo: kann parallelisiert werden
            {
                for (int i = j - kuC; i < j + klC; i++)
                {
                    double value = 0;
                    for (int r = Math.Max(i - klA, j - kuB); r < Math.Min(i + kuA, j + klB); r++)
                    {
                        value += a[i - k + kuA + k * uA] * b[k - j + kuB + j * uB];
                    }
                    c[i - j + kuC + j * uC] = alpha * value;
                }
            }
        }
    }
}