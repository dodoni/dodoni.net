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
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

using Dodoni.MathLibrary.Basics.LowLevel;

namespace Dodoni.MathLibrary.Basics
{
    /// <summary>Provides extension methods for complex numbers, BLAS functions etc.
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static partial class Extensions
    {
        #region for complex/real numbers

        /// <summary>Returns a value indicating whether a <see cref="Complex"/> number is undefined, i.e. <c>Not a Number</c>.
        /// </summary>
        /// <param name="z">The <see cref="Complex"/> number.</param>
        /// <returns><c>true</c> if the real or imaginary part is <c>NaN</c>; otherwise <c>false</c>.
        /// </returns>
        public static bool IsNaN(this Complex z)
        {
            return (Double.IsNaN(z.Real) || Double.IsNaN(z.Imaginary));
        }

        /// <summary>Gets a value indicating whether this complex number is real.
        /// </summary>
        /// <param name="z">The complex number.</param>
        /// <param name="tolerance">The (positive) tolerance.</param>
        /// <returns><c>true</c> if the the complex represented by the current instance is real, i.e. the absolute value of the imaginary part is less or equal <paramref name="tolerance"/>; otherwise <c>false</c>.
        /// </returns>
        public static bool IsReal(this Complex z, double tolerance = MachineConsts.Epsilon)
        {
            return (Math.Abs(z.Imaginary) <= tolerance);
        }

        /// <summary>Gets a value indicating whether this complex number is pure imaginary.
        /// </summary>
        /// <param name="z">The complex number.</param>
        /// <param name="tolerance">The (positive) tolerance.</param>
        /// <value><c>true</c> if this instance is pure imaginary, i.e. the absolute value of the real part is less or equal <paramref name="tolerance"/>; otherwise <c>false</c>.
        /// </value>
        public static bool IsImaginary(this Complex z, double tolerance = MachineConsts.Epsilon)
        {
            return (Math.Abs(z.Real) <= tolerance);
        }

        /// <summary>Gets a value indicating whether this complex number is zero.
        /// </summary>
        /// <param name="z">The complex number.</param>
        /// <param name="tolerance">The (positive) tolerance.</param>
        /// <value><c>true</c> if this instance is zero, i.e. the absolute value of the real as well as of the imaginary part is less or equal <paramref name="tolerance"/>; otherwise <c>false</c>.</value>
        public static bool IsZero(this Complex z, double tolerance = MachineConsts.Epsilon)
        {
            return ((Math.Abs(z.Real) <= tolerance) && (Math.Abs(z.Imaginary) <= tolerance));
        }

        /// <summary>Returns a value indicating whether the current object and a specified <see cref="Complex"/> number represent the same value, taking into account a specified (absolute) tolerance.
        /// </summary>
        /// <param name="z">The complex number.</param>
        /// <param name="other">A <see cref="Complex"/> number to compare to the current instance.</param>
        /// <param name="tolerance">The (positive) tolerance to take into account.</param>
        /// <returns>A value indicating whether the current instance and <paramref name="other"/> represents the same value, taking into account the (absolute) tolerance <paramref name="tolerance"/>.</returns>
        public static bool IsNumericalEqualTo(this Complex z, Complex other, double tolerance = MachineConsts.Epsilon)
        {
            return (Math.Abs(z.Real - other.Real) <= tolerance) && (Math.Abs(z.Imaginary - other.Imaginary) <= tolerance);
        }

        /// <summary>Returns a value indicating whether the current object and a specified <see cref="Double"/> object represent the same value, taking into account a specified (absolute) tolerance.
        /// </summary>
        /// <param name="value">The current instance.</param>
        /// <param name="other">A <see cref="System.Double"/> object to compare to the current instance.</param>
        /// <param name="tolerance">The (positive) tolerance to take into account.</param>
        /// <returns>A value indicating whether the current instance and <paramref name="other"/> represents the same value, taking into account the (absolute) tolerance <paramref name="tolerance"/>.</returns>
        public static bool IsNumericalEqualTo(this double value, double other, double tolerance = MachineConsts.Epsilon)
        {
            return Math.Abs(value - other) <= tolerance;
        }

        /// <summary>Gets a value indicating whether this floating-point number is zero.
        /// </summary>
        /// <param name="x">The floating-point number.</param>
        /// <param name="tolerance">The (positive) tolerance.</param>
        /// <value><c>true</c> if this instance is zero, i.e. the absolute value is less or equal <paramref name="tolerance"/>; otherwise <c>false</c>.</value>
        public static bool IsZero(this double x, double tolerance = MachineConsts.Epsilon)
        {
            return (Math.Abs(x) <= tolerance);
        }
        #endregion

        /// <summary>Copies the elements of the <see cref="ICollection{T}"/> to an <see cref="System.Array"/>, starting at a particular index.</summary>
        /// <typeparam name="T">The type of the data.</typeparam>
        /// <param name="source">The source in its <see cref="ICollection{T}"/> representation.</param>
        /// <param name="destination">The one-dimensional <see cref="System.Array"/> that is the destination of the elements copied from <see cref="ICollection{T}"/>.</param>
        /// <param name="count">The number of elements to copy.</param>
        /// <param name="sourceStartIndex">The zero-based index in array at which copying begins.</param>
        /// <param name="destinationStartIndex">The zero-based index of the <paramref name="destination"/> at which copying begins.</param>
        /// <param name="sourceIncrement">The increment for <paramref name="source"/>.</param>
        /// <param name="destinationIncrement">The increment for <paramref name="destination"/>.</param>
        public static void CopyTo<T>(this ICollection<T> source, T[] destination, int count, int sourceStartIndex = 0, int destinationStartIndex = 0, int sourceIncrement = 1, int destinationIncrement = 1)
        {
            if (source is double[])  // try to improve performance
            {
                BLAS.Level1.dcopy(count, source as double[], destination as double[], sourceIncrement, destinationIncrement, sourceStartIndex, destinationStartIndex);
            }
            else if (source is IList<T> sourceAsList)
            {
                int destinationIndex = destinationStartIndex;
                int sourceIndex = sourceStartIndex;
                for (int j = 0; j < count; j++)
                {
                    destination[destinationIndex] = sourceAsList[sourceIndex];

                    destinationIndex += destinationIncrement;
                    sourceIndex += sourceIncrement;
                }
            }
            else
            {
                int destinationIndex = destinationStartIndex;

                foreach (var value in source.Skip(sourceStartIndex).Take(count).Where((x, k) => (k == 0 || k % destinationIncrement == 0)))
                {
                    destination[destinationIndex] = value;
                    destinationIndex += destinationIncrement;
                }
            }
        }
    }
}