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
    /// <summary>Represents a real polynomial which has degree 1, i.e. P(x) = a*x +b with a != 0.0.
    /// </summary>
    internal class RealDegreeOnePolynomial : IRealPolynomial
    {
        #region private members

        /// <summary>The absolute coefficient of the polynomial.
        /// </summary>
        private double m_AbsoluteCoefficient;

        /// <summary>The coefficient for degree 1, i.e. the first order coefficient.
        /// </summary>
        private double m_FirstOrderCoefficient;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="RealDegreeOnePolynomial"/> class.
        /// </summary>
        /// <param name="absoluteCoefficient">The absolute coefficient.</param>
        /// <param name="firstOrderCoefficient">The first order coefficient.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if one of the parameter is not a valid real number.</exception>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="firstOrderCoefficient"/> is <c>0.0</c>.</exception>
        internal RealDegreeOnePolynomial(double absoluteCoefficient, double firstOrderCoefficient)
        {
            RealPolynomial.CheckCoefficient(absoluteCoefficient, nameof(absoluteCoefficient), "Absolute coefficient");
            m_AbsoluteCoefficient = absoluteCoefficient;

            RealPolynomial.CheckCoefficient(firstOrderCoefficient, nameof(firstOrderCoefficient), "First order coefficient");
            if (firstOrderCoefficient == 0.0)
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentIsInvalid, "0.0"), nameof(firstOrderCoefficient));
            }
            m_FirstOrderCoefficient = firstOrderCoefficient;
        }
        #endregion

        #region public properties

        #region IOperable Members

        /// <summary>Gets a value indicating whether this instance is operable.
        /// </summary>
        /// <value><c>true</c> if this instance is operable; otherwise, <c>false</c>.</value>
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

        #region IRealValuedCurve Members

        /// <summary>Gets the lower bound of the domain of definition.
        /// </summary>
        /// <value>The lower bound of the domain of definition, i.e. <see cref="System.Double.NegativeInfinity"/>.</value>
        double IRealValuedCurve.LowerBound
        {
            get { return Double.NegativeInfinity; }
        }

        /// <summary>Gets the upper bound of the domain of definition.
        /// </summary>
        /// <value>The upper bound of the domain of definition, i.e. <see cref="System.Double.PositiveInfinity"/>.</value>
        double IRealValuedCurve.UpperBound
        {
            get { return Double.PositiveInfinity; }
        }
        #endregion

        #region IPolynomial Members

        /// <summary>Gets the degree of the polynomial.
        /// </summary>
        /// <value>The degree of the polynomial.</value>
        public int Degree
        {
            get { return 1; }
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
        /// <returns>The value of the curve at <paramref name="pointToEvaluate" />.</returns>
        public Complex GetValue(Complex pointToEvaluate)
        {
            return m_AbsoluteCoefficient + pointToEvaluate * m_FirstOrderCoefficient;
        }

        /// <summary>Gets the value at a specific argument.
        /// </summary>
        /// <param name="pointToEvaluate">The point to evaluate.</param>
        /// <param name="value">The value at <paramref name="pointToEvaluate" />.</param>
        /// <returns>A value indicating whether <paramref name="value" /> contains valid data.</returns>
        public bool TryGetValue(double pointToEvaluate, out double value)
        {
            value = m_AbsoluteCoefficient + pointToEvaluate * m_FirstOrderCoefficient;
            return true;
        }
        #endregion

        #region IDifferentiableComplexValuedCurve Members

        /// <summary>Gets the derivative at a specific point.
        /// </summary>
        /// <param name="pointToEvaluate">The point to evaluate.</param>
        /// <returns>The value of the derivative at the <paramref name="pointToEvaluate" />.</returns>
        public Complex GetDerivative(Complex pointToEvaluate)
        {
            return m_FirstOrderCoefficient; ;
        }

        /// <summary>Gets the derivative at a specific point.
        /// </summary>
        /// <param name="pointToEvaluate">The point to evaluate.</param>
        /// <returns>The value of the derivative at the <paramref name="pointToEvaluate" />.</returns>
        Complex IDifferentiableComplexValuedCurve.GetDerivative(double pointToEvaluate)
        {
            return m_FirstOrderCoefficient;
        }
        #endregion

        #region IRealValuedCurve Members

        /// <summary>Gets the value at a specific argument.
        /// </summary>
        /// <param name="pointToEvaluate">The point to evaluate.</param>
        /// <returns>The value of the curve at <paramref name="pointToEvaluate" />.</returns>
        /// <remarks>
        /// The argument must be an element of the domain of definition, represented by <see cref="IRealValuedCurve.LowerBound" /> and <see cref="IRealValuedCurve.UpperBound" />.
        /// </remarks>
        public double GetValue(double pointToEvaluate)
        {
            return m_AbsoluteCoefficient + pointToEvaluate * m_FirstOrderCoefficient;
        }

        /// <summary>Gets the value of the integral \int_a^b f(x) dx.
        /// </summary>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="upperBound">The upper bound.</param>
        /// <returns>The value of \int_a^b f(x) dx.</returns>
        /// <remarks>
        /// The arguments must be elements of the domain of definition, represented by <see cref="IRealValuedCurve.LowerBound" /> and <see cref="IRealValuedCurve.UpperBound" />.
        /// </remarks>
        public double GetIntegral(double lowerBound, double upperBound)
        {
            return (upperBound - lowerBound) * m_AbsoluteCoefficient + 0.5 * m_FirstOrderCoefficient * (upperBound * upperBound - lowerBound * lowerBound);
        }
        #endregion

        #region IDifferentiableRealValuedCurve Members

        /// <summary>Gets the derivative at a specific point.
        /// </summary>
        /// <param name="pointToEvaluate">The point to evaluate.</param>
        /// <returns>The value of the derivative at the <paramref name="pointToEvaluate" />.</returns>
        /// <remarks>
        /// The argument must be an element of the domain of definition, represented by <see cref="IRealValuedCurve.LowerBound" /> and <see cref="IRealValuedCurve.UpperBound" />.
        /// </remarks>
        public double GetDerivative(double pointToEvaluate)
        {
            return m_FirstOrderCoefficient;
        }
        #endregion

        #region IRealPolynomial Members

        /// <summary>Gets the real coefficient for a specific <paramref name="order"/>.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns>The real coefficient with respect to the specified <paramref name="order"/>.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown, if <paramref name="order"/> is negative or <paramref name="order"/> is greater than the <see cref="IPolynomial.Degree"/>.</exception>
        public double GetCoefficient(int order)
        {
            if (order == 0)
            {
                return m_AbsoluteCoefficient;
            }
            else if (order == 1)
            {
                return m_FirstOrderCoefficient;
            }
            throw new IndexOutOfRangeException(nameof(order));
        }
        #endregion

        #region IPolynomial Members

        /// <summary>Gets the complex coefficient for a specific <paramref name="order"/>.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns>The complex coefficient with respect to the specified <paramref name="order"/>.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown, if <paramref name="order"/> is negative or <paramref name="order"/> is greater than the <see cref="IPolynomial.Degree"/>.</exception>
        Complex IPolynomial.GetCoefficient(int order)
        {
            if (order == 0)
            {
                return new Complex(m_AbsoluteCoefficient, 0.0);
            }
            else if (order == 1)
            {
                return m_FirstOrderCoefficient; ;
            }
            throw new IndexOutOfRangeException(nameof(order));
        }

        /// <summary>Gets the (complex) roots of the polynomial.
        /// </summary>
        /// <param name="roots">The roots (output).</param>
        /// <param name="rootFinderApproach">The root finder approach. Some implementations for polynomials with degree up to 3 ignore this parameter because of known analytic solutions.</param>
        /// <remarks>
        /// Each root of the polynomial given by the current instance will be added to <paramref name="roots" />, i.e. <see cref="Degree" /> elements.
        /// <para><paramref name="roots" /> will <c>not</c> be cleared before adding roots.</para>
        /// </remarks>
        public void GetRoots(IList<Complex> roots, IPolynomialRootFinder rootFinderApproach)
        {
            roots.Add(unchecked(m_AbsoluteCoefficient / m_FirstOrderCoefficient));  // the highest coefficient is always != 0.0, but maybe near 0.0
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
            realRoots.Add(unchecked(m_AbsoluteCoefficient / m_FirstOrderCoefficient));
            return 1;
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
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return "P(x) = " + m_FirstOrderCoefficient.ToString() + "x + " + m_AbsoluteCoefficient.ToString();
        }
        #endregion
    }
}