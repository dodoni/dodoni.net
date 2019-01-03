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

namespace Dodoni.MathLibrary.Miscellaneous
{
    /// <summary>Represents a complex polynomial of degree 0, i.e. it is some complex constant.
    /// </summary>
    internal class ComplexDegreeNullPolynomial : IPolynomial
    {
        #region private members

        /// <summary>The absolute coefficient of the polynomial.
        /// </summary>
        private Complex m_AbsoluteCoefficient;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="ComplexDegreeNullPolynomial"/> class.
        /// </summary>
        /// <param name="absoluteCoefficient">The absolute coefficient.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="absoluteCoefficient"/> is not a valid complex number.</exception>
        internal ComplexDegreeNullPolynomial(Complex absoluteCoefficient)
        {
            ComplexPolynomial.CheckCoefficient(absoluteCoefficient, nameof(absoluteCoefficient), "Absolute coefficient");
            m_AbsoluteCoefficient = absoluteCoefficient;
        }
        #endregion

        #region public properties

        #region IOperable Members

        /// <summary>Gets a value indicating whether this instance is operable.
        /// </summary>
        /// <value><c>true</c> if this instance is operable; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>This implementation returns always <c>true</c>.
        /// </remarks>
        public bool IsOperable
        {
            get { return true; }
        }
        #endregion

        #region IInfoOutputQueriable Members

        /// <summary>Gets the info-output level of detail.
        /// </summary>
        /// <value>The info-output level of detail.</value>
        public InfoOutputDetailLevel InfoOutputDetailLevel
        {
            get { return InfoOutputDetailLevel.Full; }
        }
        #endregion

        #region IPolynomial Members

        /// <summary>Gets the degree of the polynomial.
        /// </summary>
        /// <value>The degree of the polynomial.</value>
        public int Degree
        {
            get { return 0; }
        }

        /// <summary>Gets a value indicating whether this instance is the zero polynomial.
        /// </summary>
        /// <value><c>true</c> if this instance is zero; otherwise, <c>false</c>.</value>
        public bool IsZero
        {
            get { return ((Math.Abs(m_AbsoluteCoefficient.Real) < MachineConsts.Epsilon) && (Math.Abs(m_AbsoluteCoefficient.Imaginary) < MachineConsts.Epsilon)); }
        }
        #endregion

        #endregion

        #region public methods

        #region IComplexValuedCurve Members

        /// <summary>Gets the value at a specific argument.
        /// </summary>
        /// <param name="pointToEvaluate">The point to evaluate.</param>
        /// <returns>The value of the curve at <paramref name="pointToEvaluate" />.
        /// </returns>
        public Complex GetValue(Complex pointToEvaluate)
        {
            return m_AbsoluteCoefficient;
        }

        /// <summary>Gets the value at a specific argument.
        /// </summary>
        /// <param name="pointToEvaluate">The point to evaluate.</param>
        /// <param name="value">The value at <paramref name="pointToEvaluate" />.</param>
        /// <returns>A value indicating whether <paramref name="value" /> contains valid data.
        /// </returns>
        /// <remarks>This implementation returns <c>true</c> and sets <paramref name="value"/> to the absolute coefficient.</remarks>
        public bool TryGetValue(double pointToEvaluate, out double value)
        {
            value = m_AbsoluteCoefficient.Real;
            return (Math.Abs(m_AbsoluteCoefficient.Imaginary) < MachineConsts.Epsilon);
        }
        #endregion

        #region IDifferentiableComplexValuedCurve Members

        /// <summary>Gets the derivative at a specific point.
        /// </summary>
        /// <param name="pointToEvaluate">The point to evaluate.</param>
        /// <returns>The value of the derivative at the <paramref name="pointToEvaluate" />.
        /// </returns>
        public Complex GetDerivative(Complex pointToEvaluate)
        {
            return Complex.Zero;
        }

        /// <summary>Gets the derivative at a specific point.
        /// </summary>
        /// <param name="pointToEvaluate">The point to evaluate.</param>
        /// <returns>The value of the derivative at the <paramref name="pointToEvaluate" />.
        /// </returns>
        public Complex GetDerivative(double pointToEvaluate)
        {
            return Complex.Zero;
        }
        #endregion

        #region IPolynomial Members

        /// <summary>Gets the complex coefficient for a specific <paramref name="order"/>.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns>The complex coefficient with respect to the specified <paramref name="order"/>.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown, if <paramref name="order"/> is negative or <paramref name="order"/> is greater than the <see cref="IPolynomial.Degree"/>.</exception>
        public Complex GetCoefficient(int order)
        {
            if (order == 0)
            {
                return m_AbsoluteCoefficient;
            }
            throw new IndexOutOfRangeException(nameof(order));
        }

        /// <summary>Gets the (complex) roots of the polynomial.
        /// </summary>
        /// <param name="roots">The roots (output).</param>
        /// <param name="rootFinderApproach">The root finder approach. Some implementations for polynomials with degree up to 3 ignore this parameter because of known analytic solutions.</param>
        /// <remarks>Each root of the polynomial given by the current instance will be added to <paramref name="roots" />, i.e. <see cref="Degree" /> elements.
        /// <para><paramref name="roots" /> will <c>not</c> be cleared before adding roots.</para>
        /// </remarks>
        public void GetRoots(IList<Complex> roots, IPolynomialRootFinder rootFinderApproach)
        {
        }

        /// <summary>Gets the real roots of the polynomial.
        /// </summary>
        /// <param name="realRoots">The real-valued roots (output).</param>
        /// <param name="rootFinderApproach">The root finder approach. Some implementations for polynomials with degree up to 3 ignore this parameter because of known analytic solutions.</param>
        /// <param name="imaginaryZeroTolerance">The (positive) tolerance taken into account to indicate whether a complex number is interpreted as real number.</param>
        /// <returns>The number or real roots added to <paramref name="realRoots" />.</returns>
        /// <remarks>
        /// The real roots of the polynomial given by the current instance will be added to <paramref name="realRoots" />. Roots are added with respect to their
        /// multiplicity, i.e. a root may add more than once in <paramref name="realRoots" />.
        /// <para><paramref name="realRoots" /> will <c>not</c> be cleared before adding real roots.</para>
        /// </remarks>
        public int GetRealRoots(IList<double> realRoots, IPolynomialRootFinder rootFinderApproach, double imaginaryZeroTolerance = MachineConsts.Epsilon)
        {
            return 0;
        }
        #endregion

        #region IInfoOutputQueriable Members

        /// <summary>Sets the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> property.
        /// </summary>
        /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
        /// <returns>A value indicating whether the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> has been set to <paramref name="infoOutputDetailLevel" />.</returns>
        public bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
        {
            return (infoOutputDetailLevel == InfoOutputDetailLevel.Full);
        }

        /// <summary>Gets informations of the current object as a specific <see cref="InfoOutput" /> instance.
        /// </summary>
        /// <param name="infoOutput">The <see cref="InfoOutput" /> object which is to be filled with informations concering the current instance.</param>
        /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
        public void FillInfoOutput(InfoOutput infoOutput, string categoryName = InfoOutput.GeneralCategoryName)
        {
            Polynomial.FillInfoOutput(this, infoOutput.AcquirePackage(categoryName));
        }
        #endregion

        /// <summary>Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return "P(z) = " + m_AbsoluteCoefficient.ToString();
        }
        #endregion
    }
}