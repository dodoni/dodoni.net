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
using System.Collections.Generic;

using Dodoni.BasicComponents.Containers;
using Dodoni.MathLibrary.Miscellaneous;

namespace Dodoni.MathLibrary
{
    /// <summary>Serves as interface for complex or real polynomials.
    /// </summary>
    public interface IPolynomial : IDifferentiableComplexValuedCurve, IInfoOutputQueriable
    {
        #region properties

        /// <summary>Gets the degree of the polynomial.
        /// </summary>
        /// <value>The degree of the polynomial.</value>
        int Degree { get; }

        /// <summary>Gets a value indicating whether this instance is the zero polynomial.
        /// </summary>
        /// <value><c>true</c> if this instance is zero; otherwise, <c>false</c>.</value>
        bool IsZero { get; }
        #endregion

        #region methods

        /// <summary>Gets the complex coefficient for a specific <paramref name="order"/>.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns>The complex coefficient with respect to the specified <paramref name="order"/>.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown, if <paramref name="order"/> is negative or <paramref name="order"/> is greater than the <see cref="IPolynomial.Degree"/>.</exception>
        Complex GetCoefficient(int order);

        /// <summary>Gets the (complex) roots of the polynomial.
        /// </summary>
        /// <param name="roots">The roots (output).</param>
        /// <param name="rootFinderApproach">The root finder approach. Some implementations for polynomials with degree up to 3 ignore this parameter because of known analytic solutions.</param>
        /// <remarks>Each root of the polynomial given by the current instance will be added to <paramref name="roots"/>, i.e. <see cref="Degree"/> elements.
        /// <para>
        /// <paramref name="roots"/> will <c>not</c> be cleared before adding roots.
        /// </para></remarks>
        /// <exception cref="NullReferenceException">Thrown, if <paramref name="roots"/> is <c>null</c>.</exception>
        void GetRoots(IList<Complex> roots, IPolynomialRootFinder rootFinderApproach);

        /// <summary>Gets the real roots of the polynomial.
        /// </summary>
        /// <param name="realRoots">The real-valued roots (output).</param>
        /// <param name="rootFinderApproach">The root finder approach. Some implementations for polynomials with degree up to 3 ignore this parameter because of known analytic solutions.</param>
        /// <param name="imaginaryZeroTolerance">The (positive) tolerance taken into account to indicate whether a complex number is interpreted as real number.</param>
        /// <returns>The number or real roots added to <paramref name="realRoots"/>.</returns>
        /// <remarks>The real roots of the polynomial given by the current instance will be added to <paramref name="realRoots"/>. Roots are added with respect to their
        /// multiplicity, i.e. a root may add more than once in <paramref name="realRoots"/>.
        /// <para><paramref name="realRoots"/> will <c>not</c> be cleared before adding real roots.</para>
        /// </remarks>
        /// <exception cref="NullReferenceException">Thrown, if <paramref name="realRoots"/> is <c>null</c>.</exception>
        int GetRealRoots(IList<double> realRoots, IPolynomialRootFinder rootFinderApproach, double imaginaryZeroTolerance = MachineConsts.Epsilon);
        #endregion
    }
}