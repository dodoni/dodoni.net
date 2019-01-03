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
    /// <summary>Represents a real polynomial which has degree 3, i.e. P(x) = a*x^3 + b*x^2 +c*x +d with a != 0.0.
    /// </summary>
    internal class RealDegreeThreePolynomial : IRealPolynomial
    {
        #region private members

        /// <summary>The absolute coefficient of the polynomial.
        /// </summary>
        private double m_AbsoluteCoefficient;

        /// <summary>The coefficient for degree 1, i.e. the first order coefficient.
        /// </summary>
        private double m_FirstOrderCoefficient;

        /// <summary>The coefficient for degree 2, i.e. the second order coefficient.
        /// </summary>
        private double m_SecondOrderCoefficient;

        /// <summary>The coefficient for degree 3, i.e. the third order coefficient.
        /// </summary>
        private double m_ThirdOrderCoefficient;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="RealDegreeThreePolynomial"/> class.
        /// </summary>
        /// <param name="absoluteCoefficient">The absolute coefficient.</param>
        /// <param name="firstOrderCoefficient">The first order coefficient.</param>
        /// <param name="secondOrderCoefficient">The second order coefficient.</param>
        /// <param name="thirdOrderCoefficient">The third order coefficient.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if one of the parameter is not a valid real number.</exception>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="thirdOrderCoefficient"/> is <c>0.0</c>.</exception>
        internal RealDegreeThreePolynomial(double absoluteCoefficient, double firstOrderCoefficient, double secondOrderCoefficient, double thirdOrderCoefficient)
        {
            RealPolynomial.CheckCoefficient(absoluteCoefficient, nameof(absoluteCoefficient), "Absolute coefficient");
            m_AbsoluteCoefficient = absoluteCoefficient;

            RealPolynomial.CheckCoefficient(firstOrderCoefficient, nameof(firstOrderCoefficient), "1st order coefficient");
            m_FirstOrderCoefficient = firstOrderCoefficient;

            RealPolynomial.CheckCoefficient(secondOrderCoefficient, nameof(secondOrderCoefficient), "2. order coefficient");
            m_SecondOrderCoefficient = secondOrderCoefficient;

            RealPolynomial.CheckCoefficient(thirdOrderCoefficient, nameof(thirdOrderCoefficient), "3. order coefficient");
            if (thirdOrderCoefficient == 0.0)
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentIsInvalid, "0.0"), nameof(thirdOrderCoefficient));
            }
            m_ThirdOrderCoefficient = thirdOrderCoefficient;
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
            get { return 3; }
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
            Complex value = m_ThirdOrderCoefficient;
            value = m_SecondOrderCoefficient + pointToEvaluate * value;
            value = m_FirstOrderCoefficient + pointToEvaluate * value;
            return m_AbsoluteCoefficient + pointToEvaluate * value;
        }

        /// <summary>Gets the value at a specific argument.
        /// </summary>
        /// <param name="pointToEvaluate">The point to evaluate.</param>
        /// <param name="value">The value at <paramref name="pointToEvaluate" />.</param>
        /// <returns>A value indicating whether <paramref name="value" /> contains valid data.</returns>
        public bool TryGetValue(double pointToEvaluate, out double value)
        {
            value = GetValue(pointToEvaluate);
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
            return pointToEvaluate * (3.0 * m_ThirdOrderCoefficient * pointToEvaluate + 2.0 * m_SecondOrderCoefficient) + m_FirstOrderCoefficient;
        }

        /// <summary>Gets the derivative at a specific point.
        /// </summary>
        /// <param name="pointToEvaluate">The point to evaluate.</param>
        /// <returns>The value of the derivative at the <paramref name="pointToEvaluate" />.</returns>
        Complex IDifferentiableComplexValuedCurve.GetDerivative(double pointToEvaluate)
        {
            return pointToEvaluate * (3.0 * m_ThirdOrderCoefficient * pointToEvaluate + 2.0 * m_SecondOrderCoefficient) + m_FirstOrderCoefficient;
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
            double value = m_ThirdOrderCoefficient;
            value = m_SecondOrderCoefficient + pointToEvaluate * value;
            value = m_FirstOrderCoefficient + pointToEvaluate * value;
            return m_AbsoluteCoefficient + pointToEvaluate * value;
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
            double upperValue = m_ThirdOrderCoefficient / 4.0;
            double lowerValue = upperValue;

            double temp = m_SecondOrderCoefficient / 3.0;
            upperValue = temp + upperBound * upperValue;
            lowerValue = temp + lowerBound * lowerValue;

            temp = m_FirstOrderCoefficient / 2.0;
            upperValue = temp + upperBound * upperValue;
            lowerValue = temp + lowerBound * lowerValue;

            temp = m_AbsoluteCoefficient;
            upperValue = temp + upperBound * upperValue;
            lowerValue = temp + lowerBound * lowerValue;

            return upperBound * upperValue - lowerBound * lowerValue;
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
            return pointToEvaluate * (3.0 * m_ThirdOrderCoefficient * pointToEvaluate + 2.0 * m_SecondOrderCoefficient) + m_FirstOrderCoefficient;
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
            switch (order)
            {
                case 0: return m_AbsoluteCoefficient;
                case 1: return m_FirstOrderCoefficient;
                case 2: return m_SecondOrderCoefficient;
                case 3: return m_ThirdOrderCoefficient;
                default:
                    throw new IndexOutOfRangeException(nameof(order));
            }
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
            switch (order)
            {
                case 0: return new Complex(m_AbsoluteCoefficient, 0.0);
                case 1: return new Complex(m_FirstOrderCoefficient, 0.0);
                case 2: return new Complex(m_SecondOrderCoefficient, 0.0);
                case 3: return new Complex(m_ThirdOrderCoefficient, 0.0);
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
            GetRoots(m_AbsoluteCoefficient, m_FirstOrderCoefficient, m_SecondOrderCoefficient, m_ThirdOrderCoefficient, out Complex firstRoot, out Complex secondRoot, out Complex thirdRoot);

            roots.Add(firstRoot);
            roots.Add(secondRoot);
            roots.Add(thirdRoot);
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
            return GetRealRoots(m_AbsoluteCoefficient, m_FirstOrderCoefficient, m_SecondOrderCoefficient, m_ThirdOrderCoefficient, realRoots, imaginaryZeroTolerance);
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
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.</returns>
        public override string ToString()
        {
            return "P(x) = " + m_ThirdOrderCoefficient.ToString() + "x^3 + " + m_SecondOrderCoefficient + "x^2 + " + m_FirstOrderCoefficient.ToString() + "x + " + m_AbsoluteCoefficient.ToString();
        }
        #endregion

        #region internal static methods

        /// <summary>Gets the complex roots of a real-valued polynomial of degree 3.
        /// </summary>
        /// <param name="absoluteCoefficient">The absolute coefficient.</param>
        /// <param name="firstOrderCoefficient">The first order coefficient.</param>
        /// <param name="secondOrderCoefficient">The second order coefficient.</param>
        /// <param name="thirdOrderCoefficient">The third order coefficient.</param>
        /// <param name="firstRoot">The first root (output).</param>
        /// <param name="secondRoot">The second root (output).</param>
        /// <param name="thirdRoot">The third root (output).</param>
        /// <remarks>The implementation is based on §5.6, Numerical recipes, H. Press.</remarks>
        internal static void GetRoots(double absoluteCoefficient, double firstOrderCoefficient, double secondOrderCoefficient, double thirdOrderCoefficient, out Complex firstRoot, out Complex secondRoot, out Complex thirdRoot)
        {
            // normalized coefficients, i.e. x^3 + a * x^2 + b * x + c
            double a = secondOrderCoefficient / thirdOrderCoefficient;
            double b = firstOrderCoefficient / thirdOrderCoefficient;
            double c = absoluteCoefficient / thirdOrderCoefficient;

            double q = (a * a - 3.0 * b) / 9.0;
            double r = (2 * a * a * a - 9.0 * a * b + 27.0 * c) / 54.0;

            double thirdPowerOfQ = q * q * q;
            double squareOfR = r * r;
            if (squareOfR < thirdPowerOfQ)  // three real roots are given in this case
            {
                double theta = Math.Acos(r / Math.Sqrt(thirdPowerOfQ));
                double squareRootOfQ_timesMinusTwo = -2.0 * Math.Sqrt(q);

                firstRoot = squareRootOfQ_timesMinusTwo * Math.Cos(theta / 3.0) - a / 3.0;
                secondRoot = squareRootOfQ_timesMinusTwo * Math.Cos((theta + MathConsts.TwoPi) / 3.0) - a / 3.0;
                thirdRoot = squareRootOfQ_timesMinusTwo * Math.Cos((theta - MathConsts.TwoPi) / 3.0) - a / 3.0;
            }
            else // Here, we have two complex roots
            {
                double A = -Math.Sign(r) * Math.Pow(Math.Abs(r) + Math.Sqrt(squareOfR - thirdPowerOfQ), MathConsts.OneOverThree);
                double B = (A == 0.0) ? 0.0 : q / A;  // It is not necessary to take into account any tolerance here

                firstRoot = (A + B) - a / 3.0;

                double realPartRoot = -0.5 * (A + B) - a / 3.0;
                double imaginaryPartRoot = MathConsts.Sqrt3 / 2.0 * (A - B);
                secondRoot = new Complex(realPartRoot, imaginaryPartRoot);
                thirdRoot = new Complex(realPartRoot, -imaginaryPartRoot);
            }
        }

        /// <summary>Gets the real roots of a real-valued polynomial of degree 3.
        /// </summary>
        /// <param name="absoluteCoefficient">The absolute coefficient.</param>
        /// <param name="firstOrderCoefficient">The first order coefficient.</param>
        /// <param name="secondOrderCoefficient">The second order coefficient.</param>
        /// <param name="thirdOrderCoefficient">The third order coefficient.</param>
        /// <param name="realRoots">The real-valued roots (output).</param>
        /// <param name="imaginaryZeroTolerance">The (positive) tolerance taken into account to indicate whether a complex number is interpreted as real number.</param>
        /// <returns>The number or real roots added to <paramref name="realRoots"/>.</returns>
        /// <remarks>The real roots of the polynomial represented by the coefficients are added into <paramref name="realRoots"/>. Roots are added with respect to their
        /// multiplicity, i.e. a root may add more than once in <paramref name="realRoots"/>.
        /// <para><paramref name="realRoots"/> will <c>not</c> be cleared before adding real roots.
        /// Implementations use the minimum of <paramref name="imaginaryZeroTolerance"/> and <see cref="MachineConsts.Epsilon"/> to indicate whether the imaginary part of a complex 
        /// number is assumed to be <c>0.0</c> and a real number is given.
        /// </para>
        /// </remarks>
        /// <exception cref="NullReferenceException">Thrown, if <paramref name="realRoots"/> is <c>null</c>.</exception>
        internal static int GetRealRoots(double absoluteCoefficient, double firstOrderCoefficient, double secondOrderCoefficient, double thirdOrderCoefficient, IList<double> realRoots, double imaginaryZeroTolerance)
        {
            // the implementation is based on §5.6, Numerical recipes, H. Press

            // normalized coefficients, i.e. x^3 + a * x^2 + b * x + c
            double a = secondOrderCoefficient / thirdOrderCoefficient;
            double b = firstOrderCoefficient / thirdOrderCoefficient;
            double c = absoluteCoefficient / thirdOrderCoefficient;

            double q = (a * a - 3.0 * b) / 9.0;
            double r = (2 * a * a * a - 9.0 * a * b + 27.0 * c) / 54.0;

            double thirdPowerOfQ = q * q * q;
            double squareOfR = r * r;
            if (squareOfR < thirdPowerOfQ)
            {
                double theta = Math.Acos(r / Math.Sqrt(thirdPowerOfQ));
                double squareRootOfQ_timesMinusTwo = -2.0 * Math.Sqrt(q);

                realRoots.Add(squareRootOfQ_timesMinusTwo * Math.Cos(theta / 3.0) - a / 3.0);
                realRoots.Add(squareRootOfQ_timesMinusTwo * Math.Cos((theta + MathConsts.TwoPi) / 3.0) - a / 3.0);
                realRoots.Add(squareRootOfQ_timesMinusTwo * Math.Cos((theta - MathConsts.TwoPi) / 3.0) - a / 3.0);
                return 3;
            }

            double A = -Math.Sign(r) * Math.Sqrt(Math.Abs(r) + Math.Sqrt(squareOfR - thirdPowerOfQ));
            double B = (A == 0.0) ? 0.0 : q / A;

            realRoots.Add((A + B) - a / 3.0);

            double imaginaryPartRoot = MathConsts.Sqrt3 / 2.0 * (A - B);
            if (Math.Abs(imaginaryPartRoot) < Math.Min(MachineConsts.Epsilon, Math.Max(0, imaginaryZeroTolerance)))
            {
                double realPartRoot = -0.5 * (A + B) - a / 3.0;

                realRoots.Add(realPartRoot);
                realRoots.Add(realPartRoot);
                return 3;
            }
            return 1;
        }
        #endregion
    }
}