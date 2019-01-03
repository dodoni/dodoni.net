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

namespace Dodoni.MathLibrary.Miscellaneous
{
    /// <summary>Serves as factory for real-valued polynomials.
    /// </summary>
    public class RealPolynomialFactory
    {
        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="RealPolynomialFactory" /> class.
        /// </summary>
        internal RealPolynomialFactory()
        {
        }
        #endregion

        #region public methods

        #region polynomial factory methods

        /// <summary>Creates a real-valued polynomial.
        /// </summary>
        /// <param name="absoluteCoefficient">The absolute coefficient.</param>
        /// <returns>A polynomial with respect to the specified coefficients.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="absoluteCoefficient"/> is not a number of +/- infinity.</exception>
        public IRealPolynomial Create(double absoluteCoefficient)
        {
            return new RealDegreeNullPolynomial(absoluteCoefficient);
        }

        /// <summary>Creates a real-valued polynomial.
        /// </summary>
        /// <param name="absoluteCoefficient">The absolute coefficient.</param>
        /// <param name="firstOrderCoefficient">The first order coeffient.</param>
        /// <returns>A polynomial with respect to the specified coefficients.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if all arguments are 'not a number' or +/- infinity.</exception>
        public IRealPolynomial Create(double absoluteCoefficient, double firstOrderCoefficient)
        {
            if (IsNullCoefficient(firstOrderCoefficient))
            {
                return new RealDegreeNullPolynomial(absoluteCoefficient);
            }
            return new RealDegreeOnePolynomial(absoluteCoefficient, firstOrderCoefficient);
        }

        /// <summary>Creates a real-valued polynomial.
        /// </summary>
        /// <param name="absoluteCoefficient">The absolute coefficient.</param>
        /// <param name="firstOrderCoefficient">The first order coeffient.</param>
        /// <param name="secondOrderCoefficient">The second order coeffient.</param>
        /// <returns>A polynomial with respect to the specified coefficients.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if all arguments are 'not a number' or +/- infinity.</exception>
        public IRealPolynomial Create(double absoluteCoefficient, double firstOrderCoefficient, double secondOrderCoefficient)
        {
            if (IsNullCoefficient(secondOrderCoefficient))
            {
                if (IsNullCoefficient(firstOrderCoefficient))
                {
                    return new RealDegreeNullPolynomial(absoluteCoefficient);
                }
                return new RealDegreeOnePolynomial(absoluteCoefficient, firstOrderCoefficient);
            }
            return new RealDegreeTwoPolynomial(absoluteCoefficient, firstOrderCoefficient, secondOrderCoefficient);
        }

        /// <summary>Creates a real-valued polynomial.
        /// </summary>
        /// <param name="absoluteCoefficient">The absolute coefficient.</param>
        /// <param name="firstOrderCoefficient">The first order coeffient.</param>
        /// <param name="secondOrderCoefficient">The second order coeffient.</param>
        /// <param name="thirdOrderCoefficent">The 3. order coeffient.</param>
        /// <returns>A polynomial with respect to the specified coefficients.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if all arguments are 'not a number' or +/- infinity.</exception>
        public IRealPolynomial Create(double absoluteCoefficient, double firstOrderCoefficient, double secondOrderCoefficient, double thirdOrderCoefficent)
        {
            if (IsNullCoefficient(thirdOrderCoefficent))
            {
                if (IsNullCoefficient(secondOrderCoefficient))
                {
                    if (IsNullCoefficient(firstOrderCoefficient))
                    {
                        return new RealDegreeNullPolynomial(absoluteCoefficient);
                    }
                    return new RealDegreeOnePolynomial(absoluteCoefficient, firstOrderCoefficient);
                }
                return new RealDegreeTwoPolynomial(absoluteCoefficient, firstOrderCoefficient, secondOrderCoefficient);
            }
            return new RealDegreeThreePolynomial(absoluteCoefficient, firstOrderCoefficient, secondOrderCoefficient, thirdOrderCoefficent);
        }

        /// <summary>Creates a real-valued polynomial.
        /// </summary>
        /// <param name="absoluteCoefficient">The absolute coefficient.</param>
        /// <param name="firstOrderCoefficient">The first order coeffient.</param>
        /// <param name="secondOrderCoefficient">The second order coeffient.</param>
        /// <param name="thirdOrderCoefficent">The 3. order coeffient.</param>
        /// <param name="fourthOrderCoefficient">The fourth order coefficient.</param>
        /// <returns>A polynomial with respect to the specified coefficients.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if all arguments are 'not a number' or +/- infinity.</exception>
        public IRealPolynomial Create(double absoluteCoefficient, double firstOrderCoefficient, double secondOrderCoefficient, double thirdOrderCoefficent, double fourthOrderCoefficient)
        {
            if (IsNullCoefficient(fourthOrderCoefficient))
            {
                if (IsNullCoefficient(thirdOrderCoefficent))
                {
                    if (IsNullCoefficient(secondOrderCoefficient))
                    {
                        if (IsNullCoefficient(firstOrderCoefficient))
                        {
                            return new RealDegreeNullPolynomial(absoluteCoefficient);
                        }
                        return new RealDegreeOnePolynomial(absoluteCoefficient, firstOrderCoefficient);
                    }
                    return new RealDegreeTwoPolynomial(absoluteCoefficient, firstOrderCoefficient, secondOrderCoefficient);
                }
                return new RealDegreeThreePolynomial(absoluteCoefficient, firstOrderCoefficient, secondOrderCoefficient, thirdOrderCoefficent);
            }
            return new RealDegreeFourPolynomial(absoluteCoefficient, firstOrderCoefficient, secondOrderCoefficient, thirdOrderCoefficent, fourthOrderCoefficient);
        }

        /// <summary>Creates a real-valued polynomial.
        /// </summary>
        /// <param name="coefficients">The coefficients, where the null-based index corresponds to the power of the argument,
        /// i.e. starting from the absolute coefficient, first oder coefficient etc.</param>
        /// <returns>A polynomial with respect to the specified coefficients.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if all arguments are 'not a number' or +/- infinity.</exception>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="coefficients"/> is <c>null</c>.</exception>
        /// <remarks>If the degree is greater than 4 this method stores the reference of the <paramref name="coefficients"/>.</remarks>
        public IRealPolynomial Create(params double[] coefficients)
        {
            int degree = Polynomial.GetDegree(coefficients);
            if (degree == 0)
            {
                return new RealDegreeNullPolynomial(coefficients[0]);
            }
            else if (degree == 1)
            {
                return new RealDegreeOnePolynomial(coefficients[0], coefficients[1]);
            }
            else if (degree == 2)
            {
                return new RealDegreeTwoPolynomial(coefficients[0], coefficients[1], coefficients[2]);
            }
            else if (degree == 3)
            {
                return new RealDegreeThreePolynomial(coefficients[0], coefficients[1], coefficients[2], coefficients[3]);
            }
            else if (degree == 4)
            {
                return new RealDegreeFourPolynomial(coefficients[0], coefficients[1], coefficients[2], coefficients[3], coefficients[4]);
            }
            else
            {
                return new RealPolynomial(degree, coefficients);
            }
        }

        /// <summary>Creates a real-valued polynomial.
        /// </summary>
        /// <param name="degree">The degree of the polynomial.</param>
        /// <param name="coefficients">The <paramref name="degree"/> + 1 coefficients, where the null-based index corresponds to the power of the argument,
        /// i.e. starting from the absolute coefficient, first oder coefficient etc.</param>
        /// <returns>A polynomial with respect to the specified coefficients.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if all arguments are 'not a number' or +/- infinity.</exception>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="coefficients"/> is <c>null</c>.</exception>
        /// <remarks>If the degree is greater than 4 this method stores the reference of the <paramref name="coefficients"/>.</remarks>
        public IRealPolynomial Create(int degree, double[] coefficients)
        {
            if (degree == 0)
            {
                return new RealDegreeNullPolynomial(coefficients[0]);
            }
            else if (degree == 1)
            {
                return new RealDegreeOnePolynomial(coefficients[0], coefficients[1]);
            }
            else if (degree == 2)
            {
                return new RealDegreeTwoPolynomial(coefficients[0], coefficients[1], coefficients[2]);
            }
            else if (degree == 3)
            {
                return new RealDegreeThreePolynomial(coefficients[0], coefficients[1], coefficients[2], coefficients[3]);
            }
            else if (degree == 4)
            {
                return new RealDegreeFourPolynomial(coefficients[0], coefficients[1], coefficients[2], coefficients[3], coefficients[4]);
            }
            else
            {
                return new RealPolynomial(degree, coefficients);
            }
        }

        /// <summary>Creates a real-valued polynomial.
        /// </summary>
        /// <param name="coefficients">The coefficients, where the null-based index corresponds to the power of the argument,
        /// i.e. starting from the absolute coefficient, first oder coefficient etc.</param>
        /// <returns>A polynomial with respect to the specified coefficients.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if all arguments are 'not a number' or +/- infinity.</exception>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="coefficients"/> is <c>null</c>.</exception>
        public IRealPolynomial Create(IList<double> coefficients)
        {
            int degree = Polynomial.GetDegree(coefficients);
            if (degree == 0)
            {
                return new RealDegreeNullPolynomial(coefficients[0]);
            }
            else if (degree == 1)
            {
                return new RealDegreeOnePolynomial(coefficients[0], coefficients[1]);
            }
            else if (degree == 2)
            {
                return new RealDegreeTwoPolynomial(coefficients[0], coefficients[1], coefficients[2]);
            }
            else if (degree == 3)
            {
                return new RealDegreeThreePolynomial(coefficients[0], coefficients[1], coefficients[2], coefficients[3]);
            }
            else
            {
                return new RealPolynomial(degree, coefficients);
            }
        }
        #endregion

        /// <summary>Gets the coefficients of a polynomial with specified roots.
        /// </summary>
        /// <param name="degree">The degree of the polynomial, i.e. the number of roots.</param>
        /// <param name="roots">The collection of roots.</param>
        /// <param name="coefficients">The coefficients of a polynomial that has exactly <paramref name="degree"/> roots specified by <paramref name="roots"/>, where the null-based index corresponds to the power of the argument, i.e. starting from the 
        /// absolute coefficient, first oder coefficient etc. (output; at least <paramref name="degree"/> + 1 elements).</param>
        /// <param name="highestOrderCoefficient">The coefficient of the highest order.</param>
        /// <param name="imaginaryZeroTolerance">The (positive) tolerance taken into account to indicate whether a complex number is interpreted as real number.</param>
        /// <remarks>The implementation is based on 'Numerische Mathematik', Werner Neundorf, § 6.1.3.</remarks>
        public void GetCoefficientsByRoots(int degree, IList<Complex> roots, IList<double> coefficients, double highestOrderCoefficient = 1.0, double imaginaryZeroTolerance = MachineConsts.Epsilon)
        {
            var temp = new Complex[degree + 1];

            temp[degree] = highestOrderCoefficient;
            for (int j = degree - 1; j >= 0; j--)
            {
                temp[j] = 0.0;
                for (int k = j; k <= degree - 1; k++)
                {
                    temp[k] -= roots[j] * temp[k + 1];
                }
            }

            for (int j = 0; j < degree + 1; j++)
            {
                if (Math.Abs(temp[j].Imaginary) < imaginaryZeroTolerance)
                {
                    coefficients[j] = temp[j].Real;
                }
                else
                {
                    throw new ArgumentException("The roots does not specified a polynomial with real coefficients.", nameof(roots));
                }
            }
        }

        /// <summary>Gets the coefficients of a polynomial with specified roots.
        /// </summary>
        /// <param name="degree">The degree of the polynomial, i.e. the number of roots.</param>
        /// <param name="roots">The collection of roots.</param>
        /// <param name="coefficients">The coefficients of a polynomial that has exactly <paramref name="degree"/> roots specified by <paramref name="roots"/>, where the null-based index corresponds to the power of the argument, i.e. starting from the 
        /// absolute coefficient, first oder coefficient etc. (output; at least <paramref name="degree"/> + 1 elements).</param>
        /// <param name="highestOrderCoefficient">The coefficient of the highest order.</param>
        /// <remarks>The implementation is based on 'Numerische Mathematik', Werner Neundorf, § 6.1.3.</remarks>
        public void GetCoefficientsByRoots(int degree, IList<double> roots, IList<double> coefficients, double highestOrderCoefficient = 1.0)
        {
            coefficients[degree] = highestOrderCoefficient;
            for (int j = degree - 1; j >= 0; j--)
            {
                coefficients[j] = 0.0;
                for (int k = j; k <= degree - 1; k++)
                {
                    coefficients[k] -= roots[j] * coefficients[k + 1];
                }
            }
        }
        #endregion

        #region private (static) methods

        /// <summary>Gets a value indicating whether a specified coefficient is 0.0, <see cref="Double.NaN"/>, <see cref="Double.NegativeInfinity"/> or <see cref="Double.PositiveInfinity"/>.
        /// </summary>
        /// <param name="coefficient">The coefficient.</param>
        /// <returns>
        ///   <c>true</c> if <paramref name="coefficient"/> is 0.0, <see cref="Double.NaN"/>, <see cref="Double.NegativeInfinity"/> or <see cref="Double.PositiveInfinity"/>; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsNullCoefficient(double coefficient)
        {
            return (Double.IsNaN(coefficient) || Double.IsInfinity(coefficient) || (coefficient == 0.0));
        }
        #endregion
    }
}