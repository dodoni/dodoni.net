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
using System.Collections.Generic;

using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.Miscellaneous
{
    /// <summary>Provides methods to compute roots of a specified polynomial.
    /// </summary>
    public interface IPolynomialRootFinder : IIdentifierNameable
    {
        /// <summary>Gets the roots of a specified polynomial.
        /// </summary>
        /// <param name="degree">The degree of the polynomial.</param>
        /// <param name="coefficients">The coefficients of the polynmial P(z) = a_0 + a_1 * z + a_2 * z^2 + ... + a_n * z^n where the null-based index corresponds 
        /// to the power of the argument, i.e. starting from the absolute coefficient, first oder coefficient etc.</param>
        /// <param name="roots">The <paramref name="degree"/> roots (output).</param>
        /// <remarks><paramref name="roots"/> will <c>not</c> be cleared before adding roots.</remarks>
        void GetRoots(int degree, IList<Complex> coefficients, IList<Complex> roots);

        /// <summary>Gets the roots of a specified (real-valued) polynomial.
        /// </summary>
        /// <param name="degree">The degree of the polynomial.</param>
        /// <param name="coefficients">The coefficients of the polynmial P(z) = a_0 + a_1 * z + a_2 * z^2 + ... + a_n * z^n where the null-based index corresponds 
        /// to the power of the argument, i.e. starting from the absolute coefficient, first oder coefficient etc.</param>
        /// <param name="roots">The <paramref name="degree"/> roots (output).</param>
        /// <remarks><paramref name="roots"/> will <c>not</c> be cleared before adding roots.</remarks>
        void GetRoots(int degree, IList<double> coefficients, IList<Complex> roots);

        /// <summary>Gets the real roots of a specified polynomial.
        /// </summary>
        /// <param name="degree">The degree of the polynomial.</param>
        /// <param name="coefficients">The coefficients of the polynmial P(z) = a_0 + a_1 * z + a_2 * z^2 + ... + a_n * z^n where the null-based index corresponds 
        /// to the power of the argument, i.e. starting from the absolute coefficient, first oder coefficient etc.</param>
        /// <param name="realRoots">The real-valued roots (output).</param>
        /// <param name="rootImaginaryZeroTolerance">The tolerance taken into account to indicate whether a complex number is interpreted as real number.</param>
        /// <returns>The number or real roots added to <paramref name="realRoots"/>.</returns>
        /// <remarks>Roots are added with respect to their multiplicity, i.e. a root may add more than once in <paramref name="realRoots"/>.
        /// <para><paramref name="realRoots"/> will <c>not</c> be cleared before adding real roots.</para>
        /// </remarks>
        int GetRealRoots(int degree, IList<Complex> coefficients, IList<double> realRoots, double rootImaginaryZeroTolerance = MachineConsts.Epsilon);

        /// <summary>Gets the real roots of a specified polynomial.
        /// </summary>
        /// <param name="degree">The degree of the polynomial.</param>
        /// <param name="coefficients">The coefficients of the polynmial P(z) = a_0 + a_1 * z + a_2 * z^2 + ... + a_n * z^n where the null-based index corresponds 
        /// to the power of the argument, i.e. starting from the absolute coefficient, first oder coefficient etc.</param>
        /// <param name="realRoots">The real-valued roots (output).</param>
        /// <param name="rootImaginaryZeroTolerance">The tolerance taken into account to indicate whether a complex number is interpreted as real number.</param>
        /// <returns>The number or real roots added to <paramref name="realRoots"/>.</returns>
        /// <remarks>Roots are added with respect to their multiplicity, i.e. a root may add more than once in <paramref name="realRoots"/>.
        /// <para><paramref name="realRoots"/> will <c>not</c> be cleared before adding real roots.</para>
        /// </remarks>
        int GetRealRoots(int degree, IList<double> coefficients, IList<double> realRoots, double rootImaginaryZeroTolerance = MachineConsts.Epsilon);
    }
}