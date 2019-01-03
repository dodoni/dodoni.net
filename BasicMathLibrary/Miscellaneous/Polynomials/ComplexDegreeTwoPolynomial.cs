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
using System.Globalization;
using System.Collections.Generic;

using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.Miscellaneous
{
    /// <summary>Represents a complex polynomial which has degree 2, i.e. P(z) = a*z^2 + b*z +c with a != 0.
    /// </summary>
    internal class ComplexDegreeTwoPolynomial : IPolynomial
    {
        #region private members

        /// <summary>The absolute coefficient of the polynomial.
        /// </summary>
        private Complex m_AbsoluteCoefficient;

        /// <summary>The coefficient for degree 1, i.e. the first order coefficient.
        /// </summary>
        private Complex m_FirstOrderCoefficient;

        /// <summary>The coefficient for degree 2, i.e. the second order coefficient.
        /// </summary>
        private Complex m_SecondOrderCoefficient;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="ComplexDegreeTwoPolynomial"/> class.
        /// </summary>
        /// <param name="absoluteCoefficient">The absolute coefficient.</param>
        /// <param name="firstOrderCoefficient">The first order coefficient.</param>
        /// <param name="secondOrderCoefficient">The second order coefficient.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if one of the parameter is not a valid complex number.</exception>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="secondOrderCoefficient"/> is <c>0.0</c>.</exception>
        internal ComplexDegreeTwoPolynomial(Complex absoluteCoefficient, Complex firstOrderCoefficient, Complex secondOrderCoefficient)
        {
            ComplexPolynomial.CheckCoefficient(absoluteCoefficient, nameof(absoluteCoefficient), "Absolute coefficient");
            m_AbsoluteCoefficient = absoluteCoefficient;

            ComplexPolynomial.CheckCoefficient(absoluteCoefficient, nameof(firstOrderCoefficient), "First order coefficient");
            m_FirstOrderCoefficient = firstOrderCoefficient;

            ComplexPolynomial.CheckCoefficient(absoluteCoefficient, nameof(secondOrderCoefficient), "Second order coefficient");
            if (secondOrderCoefficient == 0.0)
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentIsInvalid, "0.0"), nameof(secondOrderCoefficient));
            }
            m_SecondOrderCoefficient = secondOrderCoefficient;
        }
        #endregion

        #region public properties

        #region IOperable Members

        /// <summary>Gets a value indicating whether this instance is operable.
        /// </summary>
        /// <value><c>true</c> if this instance is operable; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>This implementation returns <c>true</c>.
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
            get { return 2; }
        }

        /// <summary>Gets a value indicating whether this instance is the zero polynomial.
        /// </summary>
        /// <value><c>true</c> if this instance is zero; otherwise, <c>false</c>.</value>
        public bool IsZero
        {
            get { return false; }
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
            Complex value = m_SecondOrderCoefficient;
            value = m_FirstOrderCoefficient + pointToEvaluate * value;
            return m_AbsoluteCoefficient + pointToEvaluate * value;
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
            return 2.0 * m_SecondOrderCoefficient * pointToEvaluate + m_FirstOrderCoefficient;
        }

        /// <summary>Gets the derivative at a specific point.
        /// </summary>
        /// <param name="pointToEvaluate">The point to evaluate.</param>
        /// <returns>The value of the derivative at the <paramref name="pointToEvaluate" />.
        /// </returns>
        public Complex GetDerivative(double pointToEvaluate)
        {
            return 2.0 * m_SecondOrderCoefficient * pointToEvaluate + m_FirstOrderCoefficient;
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
            switch (order)
            {
                case 0: return m_AbsoluteCoefficient;
                case 1: return m_FirstOrderCoefficient;
                case 2: return m_SecondOrderCoefficient;
                default:
                    throw new IndexOutOfRangeException(nameof(order));
            }
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
            Complex firstRoot, secondRoot;
            GetRoots(m_AbsoluteCoefficient, m_FirstOrderCoefficient, m_SecondOrderCoefficient, out firstRoot, out secondRoot);

            roots.Add(firstRoot);
            roots.Add(secondRoot);
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
            // the implementation is based on §5.6, Numerical recipes, H. Press
            Complex sqrt = Complex.Sqrt(m_FirstOrderCoefficient * m_FirstOrderCoefficient - 4.0 * m_SecondOrderCoefficient * m_AbsoluteCoefficient);

            // compute the correct sign of the sqrt:
            if ((m_FirstOrderCoefficient * sqrt).Real < 0)
            {
                sqrt *= -1.0;
            }
            Complex q = -0.5 * (m_FirstOrderCoefficient + sqrt);
            Complex x1 = q / m_SecondOrderCoefficient;
            Complex x2 = m_AbsoluteCoefficient / q;

            int numberOfRealRoots = 0;
            imaginaryZeroTolerance = Math.Min(MachineConsts.Epsilon, Math.Max(0, imaginaryZeroTolerance));

            if (Math.Abs(x1.Imaginary) < imaginaryZeroTolerance)
            {
                realRoots.Add(x1.Real);
                numberOfRealRoots++;
            }
            if (Math.Abs(x2.Imaginary) < imaginaryZeroTolerance)
            {
                realRoots.Add(x2.Real);
                numberOfRealRoots++;
            }
            return numberOfRealRoots;
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
            return "P(z) = " + m_SecondOrderCoefficient + "z^2 + " + m_FirstOrderCoefficient.ToString() + "z + " + m_AbsoluteCoefficient.ToString();
        }
        #endregion

        #region internal static methods

        /// <summary>Gets the complex root of a polynom of degree two.
        /// </summary>
        /// <param name="absoluteCoefficient">The absolute coefficient.</param>
        /// <param name="firstOrderCoefficient">The first order coefficient.</param>
        /// <param name="secondOrderCoefficient">The second order coefficient.</param>
        /// <param name="firstRoot">The first root (output).</param>
        /// <param name="secondRoot">The second root (output).</param>
        /// <remarks>The implementation is based on §5.6, Numerical recipes, H. Press.</remarks>
        internal static void GetRoots(Complex absoluteCoefficient, Complex firstOrderCoefficient, Complex secondOrderCoefficient, out Complex firstRoot, out Complex secondRoot)
        {
            Complex sqrt = Complex.Sqrt(firstOrderCoefficient * firstOrderCoefficient - 4.0 * secondOrderCoefficient * absoluteCoefficient);

            // compute the correct sign of the sqrt:
            if ((firstOrderCoefficient * sqrt).Real < 0)
            {
                sqrt *= -1.0;
            }
            Complex q = -0.5 * (firstOrderCoefficient + sqrt);

            firstRoot = q / secondOrderCoefficient;
            secondRoot = absoluteCoefficient / q;
        }
        #endregion
    }
}