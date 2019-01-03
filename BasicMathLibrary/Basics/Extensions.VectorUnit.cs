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
using System.Numerics;

using Dodoni.MathLibrary.Basics.LowLevel;

namespace Dodoni.MathLibrary.Basics
{
    public static partial class Extensions
    {
        /// <summary>Performs subtraction of vector <paramref name="a"/> and double precision number <paramref name="b"/>.
        /// </summary>
        /// <param name="vectorOperations">The <see cref="IVectorUnitBasics"/> object.</param>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input double precision number b.</param>
        /// <param name="y">The output vector a-b.</param>
        public static void Sub(this IVectorUnitBasics vectorOperations, int n, double[] a, double b, double[] y)
        {
            vectorOperations.Add(n, a, -b, y);
        }

        /// <summary>Performs subtraction of vector <paramref name="a"/> and double precision number <paramref name="b"/>, i.e. a_j = a_j - b [in-place].
        /// </summary>
        /// <param name="vectorOperations">The <see cref="IVectorUnitBasics"/> object.</param>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a - b.</param>
        /// <param name="b">The input double precision number b.</param>
        public static void Sub(this IVectorUnitBasics vectorOperations, int n, double[] a, double b)
        {
            vectorOperations.Add(n, a, -b);
        }

        /// <summary>Performs element by element multiplication of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="vectorOperations">The <see cref="IVectorUnitBasics"/> object.</param>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector 'factor * a[j]*b[j]', j=0,...,<paramref name="n"/>-1.</param>
        /// <param name="factor">The specified factor.</param>
        public static void Mul(this IVectorUnitBasics vectorOperations, int n, double[] a, double[] b, double[] y, double factor)
        {
            vectorOperations.Mul(n, a, b, y);
            BLAS.Level1.dscal(n, factor, y);
        }

        /// <summary>Performs element by element multiplication of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="vectorOperations">The <see cref="IVectorUnitBasics"/> object.</param>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] := factor * a[<paramref name="startIndexA"/> + j] * b[<paramref name="startIndexY"/> + j] for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexB">The null-based start index of <paramref name="b"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        /// <param name="factor">The specified factor.</param>
        public static void Mul(this IVectorUnitBasics vectorOperations, int n, double[] a, double[] b, double[] y, int startIndexA, int startIndexB, int startIndexY, double factor)
        {
            vectorOperations.Mul(n, a, b, y, startIndexA, startIndexB, startIndexY);
            BLAS.Level1.dscal(n, factor, y, 1, startIndexY);
        }

        /// <summary>Performs element by element multiplication of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="vectorOperations">The <see cref="IVectorUnitBasics"/> object.</param>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector 'factor * a[j]*b[j]', j=0,...,<paramref name="n"/>-1.</param>
        /// <param name="factor">The specified factor.</param>
        public static void Mul(this IVectorUnitBasics vectorOperations, int n, Complex[] a, Complex[] b, Complex[] y, double factor)
        {
            vectorOperations.Mul(n, a, b, y);
            BLAS.Level1.zdscal(n, factor, y);
        }

        /// <summary>Performs element by element multiplication of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="vectorOperations">The <see cref="IVectorUnitBasics"/> object.</param>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] := factor * a[<paramref name="startIndexA"/> + j] * b[<paramref name="startIndexY"/> + j] for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexB">The null-based start index of <paramref name="b"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        /// <param name="factor">The specified factor.</param>
        public static void Mul(this IVectorUnitBasics vectorOperations, int n, Complex[] a, Complex[] b, Complex[] y, int startIndexA, int startIndexB, int startIndexY, double factor)
        {
            vectorOperations.Mul(n, a, b, y, startIndexA, startIndexB, startIndexY);
            BLAS.Level1.zdscal(n, factor, y, 1, startIndexY);
        }
    }
}