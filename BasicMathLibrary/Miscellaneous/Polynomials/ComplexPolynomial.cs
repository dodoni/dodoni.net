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
using System.Globalization;
using System.Collections.Generic;

using Dodoni.BasicComponents;
using Dodoni.MathLibrary.Basics;
using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.Miscellaneous
{
    /// <summary>Represents a (general) polynomial with complex coefficients.
    /// </summary>
    internal class ComplexPolynomial : IPolynomial
    {
        #region private members

        /// <summary>The coefficients of the polynmial P(z) = a_0 + a_1 * z + a_2 * z^2 + ... + a_n * z^n.
        /// </summary>
        private Complex[] m_Coefficients;

        /// <summary>The degree of the polynomial.
        /// </summary>
        private int m_Degree;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="RealPolynomial"/> class.
        /// </summary>
        /// <param name="degree">The degree.</param>
        /// <param name="coefficients">The coefficients, where the null-based index corresponds to the power of the argument, i.e. starting from the absolute coefficient, first oder coefficient etc.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="coefficients"/> is <c>null</c>.</exception>
        /// <remarks>This constructor stores the reference of the <paramref name="coefficients"/>.</remarks>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="coefficients"/> does not represents a polynom of degree <paramref name="degree"/>.</exception>
        internal ComplexPolynomial(int degree, Complex[] coefficients)
        {
            if (coefficients == null)
            {
                throw new ArgumentNullException(nameof(coefficients), String.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentNull, nameof(coefficients)));
            }
            m_Coefficients = coefficients;
            m_Degree = degree;

            if (coefficients[degree].Equals(Complex.Zero) == true)
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentIsInvalid, "0.0"), nameof(coefficients));
            }
        }

        /// <summary>Initializes a new instance of the <see cref="RealPolynomial"/> class.
        /// </summary>
        /// <param name="degree">The degree of the polynomial.</param>
        /// <param name="coefficients">The coefficients, where the null-based index corresponds to the power of the argument, i.e. starting from the absolute coefficient, first oder coefficient etc.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="coefficients"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if one of the coefficients is not a number or +/- infinity.</exception>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="coefficients"/> does not represents a polynom of degree <paramref name="degree"/>.</exception>
        internal ComplexPolynomial(int degree, IList<Complex> coefficients)
        {
            if (coefficients == null)
            {
                throw new ArgumentNullException(nameof(coefficients), String.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentNull, nameof(coefficients)));
            }
            m_Degree = degree;
            m_Coefficients = new Complex[degree + 1];
            for (int j = degree; j >= 0; j--)
            {
                Complex coefficient = m_Coefficients[j] = coefficients[j];
                if ((coefficient.IsNaN() == true) || Double.IsInfinity(coefficient.Real) || Double.IsInfinity(coefficient.Imaginary))
                {
                    throw new ArgumentOutOfRangeException(nameof(coefficients));
                }
            }
            if (coefficients[degree].Equals(Complex.Zero) == true)
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentIsInvalid, "0.0"), nameof(coefficients));
            }
        }

        /// <summary>Initializes a new instance of the <see cref="ComplexPolynomial"/> class.
        /// </summary>
        /// <param name="degree">The degree of the polynomial.</param>
        /// <param name="coefficients">The (real) coefficients, where the null-based index corresponds to the power of the argument, i.e. starting from the absolute coefficient, first oder coefficient etc.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="coefficients"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if one of the coefficients is not a number or +/- infinity.</exception>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="coefficients"/> does not represents a polynom of degree <paramref name="degree"/>.</exception>
        internal ComplexPolynomial(int degree, IList<double> coefficients)
        {
            if (coefficients == null)
            {
                throw new ArgumentNullException(nameof(coefficients), String.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentNull, nameof(coefficients)));
            }
            m_Degree = degree;
            m_Coefficients = new Complex[degree + 1];

            for (int j = degree; j >= 0; j--)
            {
                double coefficient = coefficients[j];

                m_Coefficients[j] = coefficient;
                if ((Double.IsNaN(coefficient) || Double.IsInfinity(coefficient)))
                {
                    throw new ArgumentOutOfRangeException(nameof(coefficients));
                }
            }
            if (coefficients[degree].Equals(Complex.Zero) == true)
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentIsInvalid, "0.0"), nameof(coefficients));
            }
        }

        /// <summary>Initializes a new instance of the <see cref="ComplexPolynomial"/> class.
        /// </summary>
        /// <param name="degree">The degree of the polynomial.</param>
        /// <param name="coefficients">The (real) coefficients, where the null-based index corresponds to the power of the argument, i.e. starting from the absolute coefficient, first oder coefficient etc.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="coefficients"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if one of the coefficients is not a number or +/- infinity.</exception>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="coefficients"/> does not represents a polynom of degree <paramref name="degree"/>.</exception>
        internal ComplexPolynomial(int degree, double[] coefficients)
        {
            if (coefficients == null)
            {
                throw new ArgumentNullException(nameof(coefficients), String.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentNull, nameof(coefficients)));
            }
            m_Degree = degree;
            m_Coefficients = new Complex[degree + 1];

            for (int j = degree; j >= 0; j--)
            {
                double coefficient = coefficients[j];

                m_Coefficients[j] = coefficient;

                if ((Double.IsNaN(coefficient) || Double.IsInfinity(coefficient)))
                {
                    throw new ArgumentOutOfRangeException(nameof(coefficients));
                }
            }
            if (coefficients[degree].Equals(Complex.Zero) == true)
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentIsInvalid, "0.0"), nameof(coefficients));
            }
        }
        #endregion

        #region public properties

        #region IOperable Members

        /// <summary>Gets a value indicating whether this instance is operable.
        /// </summary>
        /// <value><c>true</c> if this instance is operable; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>This implementation returns <c>true</c>.</remarks>
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
            get { return m_Degree; }
        }

        /// <summary>Gets a value indicating whether this instance is the zero polynomial.
        /// </summary>
        /// <value><c>true</c> if this instance is zero; otherwise, <c>false</c>.</value>
        public bool IsZero
        {
            get { return ((m_Degree == 0) && (m_Coefficients[0] == 0.0)); }
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
            // Here we use the Horner scheme for evaluating the polynomial: 
            Complex value = m_Coefficients[m_Degree];
            for (int j = m_Degree - 1; j >= 0; j--)
            {
                value = m_Coefficients[j] + pointToEvaluate * value;
            }
            return value;
        }

        /// <summary>Gets the value at a specific argument.
        /// </summary>
        /// <param name="pointToEvaluate">The point to evaluate.</param>
        /// <param name="value">The value at <paramref name="pointToEvaluate" />.</param>
        /// <returns>A value indicating whether <paramref name="value" /> contains valid data.
        /// </returns>
        public bool TryGetValue(double pointToEvaluate, out double value)
        {
            Complex complexValue = GetValue(pointToEvaluate);
            value = complexValue.Real;

            return (Math.Abs(complexValue.Imaginary) < MachineConsts.Epsilon);
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
            Complex value = m_Coefficients[m_Degree] * m_Degree;
            for (int j = m_Degree - 1; j >= 1; j--)
            {
                value = m_Coefficients[j] * j + value * pointToEvaluate;
            }
            return value;
        }

        /// <summary>Gets the derivative at a specific point.
        /// </summary>
        /// <param name="pointToEvaluate">The point to evaluate.</param>
        /// <returns>The value of the derivative at the <paramref name="pointToEvaluate" />.
        /// </returns>
        public Complex GetDerivative(double pointToEvaluate)
        {
            Complex value = m_Coefficients[m_Degree] * m_Degree;
            for (int j = m_Degree - 1; j >= 1; j--)
            {
                value = m_Coefficients[j] * j + value * pointToEvaluate;
            }
            return value;
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
            if (order <= m_Degree)
            {
                return m_Coefficients[order];
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
            rootFinderApproach.GetRoots(m_Degree, m_Coefficients, roots);
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
            return rootFinderApproach.GetRealRoots(m_Degree, m_Coefficients, realRoots, imaginaryZeroTolerance);
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
            StringBuilder strBuilder = new StringBuilder();
            for (int j = m_Degree; j >= 0; j--)
            {
                strBuilder.Append(m_Coefficients[j]);
                if (j > 0)
                {
                    strBuilder.Append("z^");
                    strBuilder.Append(j);
                    strBuilder.Append(" + ");
                }
            }
            return strBuilder.ToString();
        }
        #endregion

        #region internal static methods

        /// <summary>Checks a specified coefficient input.
        /// </summary>
        /// <param name="coefficient">The coefficient to check.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="coefficient"/> is not a number or +/- infinity.</exception>
        internal static void CheckCoefficient(Complex coefficient, string argumentName, string errorMessage)
        {
            if ((coefficient.IsNaN() == true) || Double.IsInfinity(coefficient.Real) || Double.IsInfinity(coefficient.Imaginary))
            {
                throw new ArgumentOutOfRangeException(argumentName, String.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentIsInvalid, errorMessage));
            }
        }
        #endregion
    }
}