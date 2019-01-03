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

namespace Dodoni.MathLibrary
{
    public static partial class Extensions
    {
        /// <summary>Copies all the elements of the current (real-valued) matrix to the specified two-dimensional Array.
        /// </summary>
        /// <param name="matrix">The <see cref="IMatrix"/> object.</param>
        /// <param name="array">The array, the first null-based index is the row, the second null-based
        /// index represents the column (output).</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="matrix"/> is <c>null</c>.</exception>
        /// <exception cref="IndexOutOfRangeException">Thrown, if <paramref name="array"/> contains less rows/columns than <paramref name="matrix"/>.</exception>
        public static void CopyTo(this IMatrix matrix, double[,] array)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException(nameof(matrix));
            }
            for (int j = 0; j < matrix.RowCount; j++)
            {
                for (int k = 0; k < matrix.ColumnCount; k++)
                {
                    array[j, k] = matrix[j, k];
                }
            }
        }

        /// <summary>Copies all the elements of the current (real-valued) matrix to the specified two-dimensional Array.
        /// </summary>
        /// <param name="matrix">The <see cref="IMatrix"/> object.</param>
        /// <param name="array">The array, the first null-based index is the row, the second null-based
        /// index represents the column (output).</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="matrix"/> is <c>null</c>.</exception>
        /// <exception cref="IndexOutOfRangeException">Thrown, if <paramref name="array"/> contains less rows/columns than <paramref name="matrix"/>.</exception>
        public static void CopyTo(this IMatrix matrix, object[,] array)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException(nameof(matrix));
            }
            for (int j = 0; j < matrix.RowCount; j++)
            {
                for (int k = 0; k < matrix.ColumnCount; k++)
                {
                    array[j, k] = matrix[j, k];
                }
            }
        }

        /// <summary>Copies all the elements of the current (real-valued) matrix to the specified two-dimensional Array.
        /// </summary>
        /// <param name="matrix">The <see cref="IMatrix"/> object.</param>
        /// <param name="array">The array, the first null-based index is the row, the second null-based
        /// index represents the column (output).</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="matrix"/> is <c>null</c>.</exception>
        /// <exception cref="IndexOutOfRangeException">Thrown, if <paramref name="array"/> contains less rows/columns than <paramref name="matrix"/>.</exception>
        public static void CopyTo(this IMatrix matrix, double[][] array)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException(nameof(matrix));
            }
            for (int j = 0; j < matrix.RowCount; j++)
            {
                for (int k = 0; k < matrix.ColumnCount; k++)
                {
                    array[j][k] = matrix[j, k];
                }
            }
        }
    }
}