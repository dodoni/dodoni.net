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
using System.Data;
using System.Globalization;
using System.Collections.Generic;

using Dodoni.BasicComponents;
using Dodoni.MathLibrary.Basics;
using Dodoni.MathLibrary.Miscellaneous;
using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary
{
    /// <summary>Serves as factory for (ordinary) polynomials.
    /// </summary>
    public static class Polynomial
    {
        #region nested classes

        /// <summary>Serves as factory for root finder of a specified polynomial.
        /// </summary>
        public class RootFinder
        {
            #region nested classes

            /// <summary>Provides analytical methods for finding roots of a specified polynomial.
            /// </summary>
            public static class Analytical
            {
                #region analytic root finding methods (complex coefficients)

                /// <summary>Gets the (complex) roots of a specified polynomial of degree up to 2.
                /// </summary>
                /// <param name="absoluteCoefficient">The absolute coefficient.</param>
                /// <param name="firstOrderCoefficient">The first order coefficient.</param>
                /// <param name="secondOrderCoefficient">The second order coefficient.</param>
                /// <param name="firstRoot">The first root (output).</param>
                /// <param name="secondRoot">The second root (output).</param>
                /// <returns>The order of the polynomial represented by the coefficients, i.e. the number of roots.</returns>
                public static int GetRoots(System.Numerics.Complex absoluteCoefficient, System.Numerics.Complex firstOrderCoefficient, System.Numerics.Complex secondOrderCoefficient, out System.Numerics.Complex firstRoot, out System.Numerics.Complex secondRoot)
                {
                    if (secondOrderCoefficient.Equals(System.Numerics.Complex.Zero) == false)   // degree 2
                    {
                        ComplexDegreeTwoPolynomial.GetRoots(absoluteCoefficient, firstOrderCoefficient, secondOrderCoefficient, out firstRoot, out secondRoot);
                        return 2;
                    }
                    else if (firstOrderCoefficient.Equals(System.Numerics.Complex.Zero) == false)  // degree 1
                    {
                        firstRoot = unchecked(-absoluteCoefficient / firstOrderCoefficient);  // the highest coefficient is always != 0.0, but maybe near 0.0
                        secondRoot = new System.Numerics.Complex(Double.NaN, Double.NaN);
                        return 1;
                    }
                    else  // degree 0
                    {
                        firstRoot = secondRoot = new System.Numerics.Complex(Double.NaN, Double.NaN);
                        return 0;
                    }
                }

                /// <summary>Gets the (complex) roots of a specified polynomial of degree up to 2.
                /// </summary>
                /// <param name="absoluteCoefficient">The absolute coefficient.</param>
                /// <param name="firstOrderCoefficient">The first order coefficient.</param>
                /// <param name="secondOrderCoefficient">The second order coefficient.</param>
                /// <param name="thirdOrderCoefficient">The third order coefficient.</param>
                /// <param name="firstRoot">The first root (output).</param>
                /// <param name="secondRoot">The second root (output).</param>
                /// <param name="thirdRoot">The third root (output).</param>
                /// <returns>The order of the polynomial represented by the coefficients, i.e. the number of roots.</returns>
                public static int GetRoots(System.Numerics.Complex absoluteCoefficient, System.Numerics.Complex firstOrderCoefficient, System.Numerics.Complex secondOrderCoefficient, System.Numerics.Complex thirdOrderCoefficient, out System.Numerics.Complex firstRoot, out System.Numerics.Complex secondRoot, out System.Numerics.Complex thirdRoot)
                {
                    if (thirdOrderCoefficient.Equals(System.Numerics.Complex.Zero) == false)  // degree 3
                    {
                        ComplexDegreeThreePolynomial.GetRoots(absoluteCoefficient, firstOrderCoefficient, secondOrderCoefficient, thirdOrderCoefficient, out firstRoot, out secondRoot, out thirdRoot);
                        return 3;
                    }
                    if (secondOrderCoefficient.Equals(System.Numerics.Complex.Zero) == false)   // degree 2
                    {
                        ComplexDegreeTwoPolynomial.GetRoots(absoluteCoefficient, firstOrderCoefficient, secondOrderCoefficient, out firstRoot, out secondRoot);
                        thirdRoot = new System.Numerics.Complex(Double.NaN, Double.NaN);
                        return 2;
                    }
                    else if (firstOrderCoefficient.Equals(System.Numerics.Complex.Zero) == false)  // degree 1
                    {
                        firstRoot = unchecked(-absoluteCoefficient / firstOrderCoefficient);  // the highest coefficient is always != 0.0, but maybe near 0.0
                        secondRoot = thirdRoot = new System.Numerics.Complex(Double.NaN, Double.NaN);
                        return 1;
                    }
                    else  // degree 0
                    {
                        firstRoot = secondRoot = thirdRoot = new System.Numerics.Complex(Double.NaN, Double.NaN);
                        return 0;
                    }
                }
                #endregion

                #region analytic complex root finding methods (real coefficients)

                /// <summary>Gets the (complex) roots of a specified polynomial of degree up to 2.
                /// </summary>
                /// <param name="absoluteCoefficient">The absolute coefficient.</param>
                /// <param name="firstOrderCoefficient">The first order coefficient.</param>
                /// <param name="secondOrderCoefficient">The second order coefficient.</param>
                /// <param name="firstRoot">The first root (output).</param>
                /// <param name="secondRoot">The second root (output).</param>
                /// <returns>The order of the polynomial represented by the coefficients, i.e. the number of roots.</returns>
                public static int GetRoots(double absoluteCoefficient, double firstOrderCoefficient, double secondOrderCoefficient, out System.Numerics.Complex firstRoot, out System.Numerics.Complex secondRoot)
                {
                    if (secondOrderCoefficient != 0.0)   // degree 2
                    {
                        RealDegreeTwoPolynomial.GetRoots(absoluteCoefficient, firstOrderCoefficient, secondOrderCoefficient, out firstRoot, out secondRoot);
                        return 2;
                    }
                    else if (firstOrderCoefficient != 0.0)  // degree 1
                    {
                        firstRoot = unchecked(-absoluteCoefficient / firstOrderCoefficient);  // the highest coefficient is always != 0.0, but maybe near 0.0
                        secondRoot = new System.Numerics.Complex(Double.NaN, Double.NaN);
                        return 1;
                    }
                    else  // degree 0
                    {
                        firstRoot = secondRoot = new System.Numerics.Complex(Double.NaN, Double.NaN);
                        return 0;
                    }
                }

                /// <summary>Gets the (complex) roots of a specified polynomial of degree up to 3.
                /// </summary>
                /// <param name="absoluteCoefficient">The absolute coefficient.</param>
                /// <param name="firstOrderCoefficient">The first order coefficient.</param>
                /// <param name="secondOrderCoefficient">The second order coefficient.</param>
                /// <param name="thirdOrderCoefficient">The third order coefficient.</param>
                /// <param name="firstRoot">The first root (output).</param>
                /// <param name="secondRoot">The second root (output).</param>
                /// <param name="thirdRoot">The third root (output).</param>
                /// <returns>The order of the polynomial represented by the coefficients, i.e. the number of roots.</returns>
                public static int GetRoots(double absoluteCoefficient, double firstOrderCoefficient, double secondOrderCoefficient, double thirdOrderCoefficient, out System.Numerics.Complex firstRoot, out System.Numerics.Complex secondRoot, out System.Numerics.Complex thirdRoot)
                {
                    if (thirdOrderCoefficient != 0.0)  // degree 3
                    {
                        RealDegreeThreePolynomial.GetRoots(absoluteCoefficient, firstOrderCoefficient, secondOrderCoefficient, thirdOrderCoefficient, out firstRoot, out secondRoot, out thirdRoot);
                        return 3;
                    }
                    if (secondOrderCoefficient != 0.0)   // degree 2
                    {
                        RealDegreeTwoPolynomial.GetRoots(absoluteCoefficient, firstOrderCoefficient, secondOrderCoefficient, out firstRoot, out secondRoot);
                        thirdRoot = new System.Numerics.Complex(Double.NaN, Double.NaN);
                        return 2;
                    }
                    else if (firstOrderCoefficient != 0.0)  // degree 1
                    {
                        firstRoot = unchecked(-absoluteCoefficient / firstOrderCoefficient);  // the highest coefficient is always != 0.0, but maybe near 0.0
                        secondRoot = thirdRoot = new System.Numerics.Complex(Double.NaN, Double.NaN);
                        return 1;
                    }
                    else  // degree 0
                    {
                        firstRoot = secondRoot = thirdRoot = new System.Numerics.Complex(Double.NaN, Double.NaN);
                        return 0;
                    }
                }

                /// <summary>Gets the (complex) roots of a specified polynomial of degree up to 4.
                /// </summary>
                /// <param name="absoluteCoefficient">The absolute coefficient.</param>
                /// <param name="firstOrderCoefficient">The first order coefficient.</param>
                /// <param name="secondOrderCoefficient">The second order coefficient.</param>
                /// <param name="thirdOrderCoefficient">The third order coefficient.</param>
                /// <param name="fourthOrderCoefficient">The fourth order coefficient.</param>
                /// <param name="firstRoot">The first root (output).</param>
                /// <param name="secondRoot">The second root (output).</param>
                /// <param name="thirdRoot">The third root (output).</param>
                /// <param name="fourthRoot">The fourth root (output).</param>
                /// <returns>The order of the polynomial represented by the coefficients, i.e. the number of roots.</returns>
                public static int GetRoots(double absoluteCoefficient, double firstOrderCoefficient, double secondOrderCoefficient, double thirdOrderCoefficient, double fourthOrderCoefficient, out System.Numerics.Complex firstRoot, out System.Numerics.Complex secondRoot, out System.Numerics.Complex thirdRoot, out System.Numerics.Complex fourthRoot)
                {
                    if (fourthOrderCoefficient != 0.0) // degree 4
                    {
                        RealDegreeFourPolynomial.GetRoots(absoluteCoefficient, firstOrderCoefficient, secondOrderCoefficient, thirdOrderCoefficient, fourthOrderCoefficient, out firstRoot, out secondRoot, out thirdRoot, out fourthRoot);
                        return 4;
                    }
                    if (thirdOrderCoefficient != 0.0)  // degree 3
                    {
                        RealDegreeThreePolynomial.GetRoots(absoluteCoefficient, firstOrderCoefficient, secondOrderCoefficient, thirdOrderCoefficient, out firstRoot, out secondRoot, out thirdRoot);
                        fourthRoot = new System.Numerics.Complex(Double.NaN, Double.NaN);
                        return 3;
                    }
                    if (secondOrderCoefficient != 0.0)   // degree 2
                    {
                        RealDegreeTwoPolynomial.GetRoots(absoluteCoefficient, firstOrderCoefficient, secondOrderCoefficient, out firstRoot, out secondRoot);
                        thirdRoot = fourthRoot = new System.Numerics.Complex(Double.NaN, Double.NaN);
                        return 2;
                    }
                    else if (firstOrderCoefficient != 0.0)  // degree 1
                    {
                        firstRoot = unchecked(-absoluteCoefficient / firstOrderCoefficient);  // the highest coefficient is always != 0.0, but maybe near 0.0
                        secondRoot = thirdRoot = fourthRoot = new System.Numerics.Complex(Double.NaN, Double.NaN);
                        return 1;
                    }
                    else  // degree 0
                    {
                        firstRoot = secondRoot = thirdRoot = fourthRoot = new System.Numerics.Complex(Double.NaN, Double.NaN);
                        return 0;
                    }
                }
                #endregion

                #region analytic real root finding methods (real coefficients)

                /// <summary>Gets the real roots of a specified polynomial of degree up to 2.
                /// </summary>
                /// <param name="absoluteCoefficient">The absolute coefficient.</param>
                /// <param name="firstOrderCoefficient">The first order coefficient.</param>
                /// <param name="secondOrderCoefficient">The second order coefficient.</param>
                /// <param name="realRoots">The real roots (output).</param>
                /// <param name="imaginaryZeroTolerance">The (positive) tolerance taken into account to indicate whether a complex number is interpreted as real number.</param>
                /// <returns>The number of real roots.</returns>
                /// <remarks>The real roots of the polynomial represented by the coefficients will be added to <paramref name="realRoots"/>. Roots are added with respect to their
                /// multiplicity, i.e. a root may add more than once in <paramref name="realRoots"/>.
                /// <para><paramref name="realRoots"/> will <c>not</c> be cleared before adding real roots.
                /// Implementations use the minimum of <paramref name="imaginaryZeroTolerance"/> and <see cref="MachineConsts.Epsilon"/> to indicate whether the imaginary part of a complex 
                /// number is assumed to be <c>0.0</c> and a real number is given.
                /// </para>
                /// </remarks>
                /// <exception cref="NullReferenceException">Thrown, if <paramref name="realRoots"/> is <c>null</c>.</exception>
                public static int GetRealRoots(double absoluteCoefficient, double firstOrderCoefficient, double secondOrderCoefficient, IList<double> realRoots, double imaginaryZeroTolerance = MachineConsts.Epsilon)
                {
                    if (secondOrderCoefficient != 0.0)   // degree 2
                    {
                        return RealDegreeTwoPolynomial.GetRealRoots(absoluteCoefficient, firstOrderCoefficient, secondOrderCoefficient, realRoots, imaginaryZeroTolerance);
                    }
                    else if (firstOrderCoefficient != 0.0)  // degree 1
                    {
                        realRoots.Add(unchecked(-absoluteCoefficient / firstOrderCoefficient));  // the highest coefficient is always != 0.0, but maybe near 0.0                
                        return 1;
                    }
                    else  // degree 0
                    {
                        return 0;
                    }
                }

                /// <summary>Gets the real roots of a specified polynomial of degree up to 3.
                /// </summary>
                /// <param name="absoluteCoefficient">The absolute coefficient.</param>
                /// <param name="firstOrderCoefficient">The first order coefficient.</param>
                /// <param name="secondOrderCoefficient">The second order coefficient.</param>
                /// <param name="thirdOrderCoefficient">The third order coefficient.</param>
                /// <param name="realRoots">The real roots (output).</param>
                /// <param name="imaginaryZeroTolerance">The (positive) tolerance taken into account to indicate whether a complex number is interpreted as real number.</param>
                /// <returns>The number of real roots.</returns>
                /// <remarks>The real roots of the polynomial represented by the coefficients will be added to <paramref name="realRoots"/>. Roots are added with respect to their
                /// multiplicity, i.e. a root may add more than once in <paramref name="realRoots"/>.
                /// <para><paramref name="realRoots"/> will <c>not</c> be cleared before adding real roots.
                /// Implementations use the minimum of <paramref name="imaginaryZeroTolerance"/> and <see cref="MachineConsts.Epsilon"/> to indicate whether the imaginary part of a complex 
                /// number is assumed to be <c>0.0</c> and a real number is given.
                /// </para>
                /// </remarks>
                /// <exception cref="NullReferenceException">Thrown, if <paramref name="realRoots"/> is <c>null</c>.</exception>
                public static int GetRealRoots(double absoluteCoefficient, double firstOrderCoefficient, double secondOrderCoefficient, double thirdOrderCoefficient, IList<double> realRoots, double imaginaryZeroTolerance = MachineConsts.Epsilon)
                {
                    if (thirdOrderCoefficient != 0.0)  // degree 3
                    {
                        return RealDegreeThreePolynomial.GetRealRoots(absoluteCoefficient, firstOrderCoefficient, secondOrderCoefficient, thirdOrderCoefficient, realRoots, imaginaryZeroTolerance);
                    }
                    if (secondOrderCoefficient != 0.0)   // degree 2
                    {
                        return RealDegreeTwoPolynomial.GetRealRoots(absoluteCoefficient, firstOrderCoefficient, secondOrderCoefficient, realRoots, imaginaryZeroTolerance);
                    }
                    else if (firstOrderCoefficient != 0.0)  // degree 1
                    {
                        realRoots.Add(unchecked(-absoluteCoefficient / firstOrderCoefficient));  // the highest coefficient is always != 0.0, but maybe near 0.0                
                        return 1;
                    }
                    else  // degree 0
                    {
                        return 0;
                    }
                }

                /// <summary>Gets the real roots of a specified polynomial of degree up to 4.
                /// </summary>
                /// <param name="absoluteCoefficient">The absolute coefficient.</param>
                /// <param name="firstOrderCoefficient">The first order coefficient.</param>
                /// <param name="secondOrderCoefficient">The second order coefficient.</param>
                /// <param name="thirdOrderCoefficient">The third order coefficient.</param>
                /// <param name="fourthOrderCoefficient">The fourth order coefficient.</param>
                /// <param name="realRoots">The real roots (output).</param>
                /// <param name="imaginaryZeroTolerance">The (positive) tolerance taken into account to indicate whether a complex number is interpreted as real number.</param>
                /// <returns>The number of real roots.</returns>
                /// <remarks>The real roots of the polynomial represented by the coefficients will be added to <paramref name="realRoots"/>. Roots are added with respect to their
                /// multiplicity, i.e. a root may add more than once in <paramref name="realRoots"/>.
                /// <para><paramref name="realRoots"/> will <c>not</c> be cleared before adding real roots.
                /// Implementations use the minimum of <paramref name="imaginaryZeroTolerance"/> and <see cref="MachineConsts.Epsilon"/> to indicate whether the imaginary part of a complex 
                /// number is assumed to be <c>0.0</c> and a real number is given.
                /// </para>
                /// </remarks>
                /// <exception cref="NullReferenceException">Thrown, if <paramref name="realRoots"/> is <c>null</c>.</exception>
                public static int GetRealRoots(double absoluteCoefficient, double firstOrderCoefficient, double secondOrderCoefficient, double thirdOrderCoefficient, double fourthOrderCoefficient, IList<double> realRoots, double imaginaryZeroTolerance = MachineConsts.Epsilon)
                {
                    if (fourthOrderCoefficient != 0.0) // degree 4
                    {
                        return RealDegreeFourPolynomial.GetRealRoots(absoluteCoefficient, firstOrderCoefficient, secondOrderCoefficient, thirdOrderCoefficient, fourthOrderCoefficient, realRoots, imaginaryZeroTolerance);
                    }
                    if (thirdOrderCoefficient != 0.0)  // degree 3
                    {
                        return RealDegreeThreePolynomial.GetRealRoots(absoluteCoefficient, firstOrderCoefficient, secondOrderCoefficient, thirdOrderCoefficient, realRoots, imaginaryZeroTolerance);
                    }
                    if (secondOrderCoefficient != 0.0)   // degree 2
                    {
                        return RealDegreeTwoPolynomial.GetRealRoots(absoluteCoefficient, firstOrderCoefficient, secondOrderCoefficient, realRoots, imaginaryZeroTolerance);
                    }
                    else if (firstOrderCoefficient != 0.0)  // degree 1
                    {
                        realRoots.Add(unchecked(-absoluteCoefficient / firstOrderCoefficient));  // the highest coefficient is always != 0.0, but maybe near 0.0
                        return 1;
                    }
                    else  // degree 0
                    {
                        return 0;
                    }
                }
                #endregion
            }
            #endregion

            /// <summary>Represents the root finder approach for Polynomials based on Eigenvalue calculation.
            /// </summary>
            public static readonly EigenvaluePolynomialRootFinder EigenvalueApproach = new EigenvaluePolynomialRootFinder();

            /// <summary>Represents Laguerre's root finder method for Polynomials.
            /// </summary>
            public static readonly LaguerrePolynomialRootFinder LaguerreApproach = new LaguerrePolynomialRootFinder();

            /// <summary>Initializes the <see cref="RootFinder" /> class.
            /// </summary>
            static RootFinder()
            {
            }
        }
        #endregion

        #region public static members

        /// <summary>Serves as factory for real-valued polynomials.
        /// </summary>
        public static readonly RealPolynomialFactory Real = new RealPolynomialFactory();

        /// <summary>Serves as factory for complex-valued polynomials.
        /// </summary>
        public static readonly ComplexPolynomialFactory Complex = new ComplexPolynomialFactory();
        #endregion

        #region public (static) methods

        /// <summary>Gets the degree of a sequence of coefficients which represents a polynomial.
        /// </summary>
        /// <param name="coefficients">The coefficients, where the null-based index corresponds to the power of the argument, i.e. starting from absolute coefficient, first oder coefficient etc.</param>
        /// <returns>The degree of the polynomial which coefficients are specified by <paramref name="coefficients"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="coefficients"/> is <c>null</c>.</exception>
        public static int GetDegree(IList<double> coefficients)
        {
            if (coefficients == null)
            {
                throw new ArgumentNullException(nameof(coefficients), String.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentNull, nameof(coefficients)));
            }
            for (int j = coefficients.Count - 1; j >= 0; j--)
            {
                double coefficient = coefficients[j];
                if ((Double.IsNaN(coefficient) == false) && (Double.IsInfinity(coefficient) == false) && (coefficient != 0.0))
                {
                    return j;
                }
            }
            return 0;
        }

        /// <summary>Gets the degree of a sequence of coefficients which represents a polynomial.
        /// </summary>
        /// <param name="coefficients">The coefficients, where the null-based index corresponds to the power of the argument, i.e. starting from absolute coefficient, first oder coefficient etc.</param>
        /// <returns>The degree of the polynomial which coefficients are specified by <paramref name="coefficients"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="coefficients"/> is <c>null</c>.</exception>
        public static int GetDegree(IList<System.Numerics.Complex> coefficients)
        {
            if (coefficients == null)
            {
                throw new ArgumentNullException(nameof(coefficients), String.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentNull, nameof(coefficients)));
            }
            for (int j = coefficients.Count - 1; j >= 0; j--)
            {
                System.Numerics.Complex coefficient = coefficients[j];
                if ((coefficient.IsNaN() == false) && (Double.IsInfinity(coefficient.Real) == false) && (Double.IsInfinity(coefficient.Imaginary) == false) && (coefficient != 0.0))
                {
                    return j;
                }
            }
            return 0;
        }
        #endregion

        #region internal (static) methods

        /// <summary>Stores informations of an <see cref="IPolynomial"/> object into a specified <see cref="InfoOutputPackage"/>.
        /// </summary>
        /// <param name="polynomial">The <see cref="IPolynomial"/> object.</param>
        /// <param name="infoOutputPackage">The <see cref="InfoOutputPackage"/> object to store the degree and coefficients of <paramref name="polynomial"/>.</param>
        internal static void FillInfoOutput(IPolynomial polynomial, InfoOutputPackage infoOutputPackage)
        {
            infoOutputPackage.Add("Degree", polynomial.Degree);

            DataTable coefficientDataTable = new DataTable("Coefficients");
            coefficientDataTable.Columns.Add("Order", typeof(int));
            coefficientDataTable.Columns.Add("Coefficient.Real", typeof(double));
            coefficientDataTable.Columns.Add("Coefficient.Imaginary", typeof(double));

            for (int order = 0; order <= polynomial.Degree; order++)
            {
                var coefficient = polynomial.GetCoefficient(order);

                coefficientDataTable.Rows.Add(order, coefficient.Real, coefficient.Imaginary);
            }
            infoOutputPackage.Add(coefficientDataTable);
        }
        #endregion
    }
}