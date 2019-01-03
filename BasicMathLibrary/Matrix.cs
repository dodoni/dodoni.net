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
using System.Globalization;

using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary
{
    /// <summary>Represents as abstract basis class for matrix implementations.
    /// </summary>
    public static class Matrix
    {
        /// <summary>Represents the type of a matrix norm.
        /// </summary>
        public enum NormType
        {
            /// <summary>Computes the largest absolute value of the specific matrix.
            /// </summary>
            LargestAbsoluteValue,

            /// <summary>Computes the 1-norm of the specific matrix (maximum column sum).
            /// </summary>
            OneNorm,

            /// <summary>Computes the infinity norm of the specific matrix (maximum row sum).
            /// </summary>
            Infinity,

            /// <summary>Computes the Frobenius norm of the specific matrix (square root of sum of squares, i.e. Euclidian norm).
            /// </summary>
            Frobenius
        }

        #region static internal methods

        /// <summary>Tests the input for some matrix multiplication, i.e. test the input for the operation 'A * B'.
        /// </summary>
        /// <param name="a">The first matrix.</param>
        /// <param name="b">The second matrix.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown, if the number of columns of <paramref name="a"/> does not coincide with
        /// the number of rows of <paramref name="b"/>.</exception>
        internal static void TestMultiplicationInput(IMatrix a, IMatrix b)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a), String.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentNull, "'a'"));
            }
            if (b == null)
            {
                throw new ArgumentNullException(nameof(b), String.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentNull, "'b'"));
            }
            if (a.ColumnCount != b.RowCount)
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentCombinationInvalid, nameof(a.ColumnCount) + "," + nameof(b.RowCount)));
            }
        }

        /// <summary>Tests the input for some matrix addition/subtraction.
        /// </summary>
        /// <param name="a">The first matrix.</param>
        /// <param name="b">The second matrix.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown, if the number of columns/rows of <paramref name="a"/> does not coincide with
        /// the number of columns/rows of <paramref name="b"/>.</exception>
        internal static void TestAdditionInput(IMatrix a, IMatrix b)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a), string.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentNull, "'a'"));
            }
            if (b == null)
            {
                throw new ArgumentNullException(nameof(b), string.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentNull, "'b'"));
            }
            if (a.ColumnCount != b.ColumnCount)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentCombinationInvalid, "a.ColumnCount != b.ColumnCount"));
            }
            if (a.RowCount != b.RowCount)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentCombinationInvalid, "a.RowCount != b.RowCount"));
            }
        }
        #endregion
    }
}