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

using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.Basics.LowLevel
{
    /// <summary>A managed implementation of <see cref="PrimitiveIntegralSpecialFunctions"/> that implements (inverse) cummulative standard normal distribution only.
    /// </summary>
    internal class PartialBuildInPrimitiveIntegralSpecialFunctions : PrimitiveIntegralSpecialFunctions
    {
        #region private members

        /* constants for the inverse cummulative standard normal distribution function: */

        /// <summary>The border for the lower region in the algorithm for the inverse cummulative standard normal distribution function.
        /// </summary>
        private static readonly double sm_LOW = 0.02425;

        /// <summary>The border for the upper region in the algorithm for the inverse cummulative standard normal distribution function.
        /// </summary>
        private static readonly double sm_HIGH = 0.97575;

        /// <summary>Coefficients in rational approximation of the inverse cummulative standard normal distribution function.
        /// </summary>
        private static readonly double[] sm_InvCDFCoefficientA = {
        -3.969683028665376e+01,
         2.209460984245205e+02,
        -2.759285104469687e+02,
         1.383577518672690e+02,
        -3.066479806614716e+01,
         2.506628277459239e+00};

        /// <summary>Coefficients in rational approximation of the inverse cummulative standard normal distribution function.
        /// </summary>
        private static readonly double[] sm_InvCDFCoefficientB = {
        -5.447609879822406e+01,
         1.615858368580409e+02,
        -1.556989798598866e+02,
         6.680131188771972e+01,
        -1.328068155288572e+01};

        /// <summary>Coefficients in rational approximation of the inverse cummulative standard normal distribution function.
        /// </summary>
        private static readonly double[] sm_InvCDFCoefficientC ={
        -7.784894002430293e-03,
        -3.223964580411365e-01,
        -2.400758277161838e+00,
        -2.549732539343734e+00,
         4.374664141464968e+00,
         2.938163982698783e+00};

        /// <summary>Coefficients in rational approximation of the inverse cummulative standard normal distribution function.
        /// </summary>
        private static readonly double[] sm_InvCDFCoefficientD ={
        7.784695709041462e-03,
        3.224671290700398e-01,
        2.445134137142996e+00,
        3.754408661907416e+00};
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="PartialBuildInPrimitiveIntegralSpecialFunctions" /> class.
        /// </summary>
        internal PartialBuildInPrimitiveIntegralSpecialFunctions()
        {
        }
        #endregion

        #region public methods

        /// <summary>Gets a specific value of the error function erf(x) = 2/\sqrt{PI} * \int_0^x e^{-t^2} dt.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <returns>The specified value of the error function erf(x) = 2/\sqrt{PI} * \int_0^x e^{-t^2} dt.</returns>
        public override double Erf(double x)
        {
            return 2.0 * GetStandardNormalCdfValue(x * MathConsts.Sqrt2) - 1.0;
        }

        /// <summary>Gets a specific value of the complementary error function erfc(x) = 1- erf(x) = 2/\sqrt{PI} * \int_x^\infty e^{-t^2} dt.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <returns>The specified value of the complementary error function erfc(x) = 1- erf(x) = 2/\sqrt{PI} * \int_x^\infty e^{-t^2} dt.</returns>
        public override double Erfc(double x)
        {
            return 1.0 - Erf(x);
        }

        /// <summary>Gets a specific value of the exponentially scaled complementary error function erfce(x) = exp(x^2) * ( 1- erf(x) ) = exp(x^2) * 2/\sqrt{PI} * \int_x^\infty e^{-t^2} dt.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <returns>The specified value of the exponentially scaled complementary error function erfce(x) = exp(x^2) * ( 1- erf(x) )  = exp(x^2) * 2/\sqrt{PI} * \int_x^\infty e^{-t^2} dt.</returns>
        public override double Erfce(double x)
        {
            return Math.Exp(x * x) * (1.0 - Erf(x));
        }

        /// <summary>Gets a specific value of the inverse of the error function erf(x) = 2/\sqrt{PI} * \int_0^x e^{-t^2} dt.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <returns>The specified value of the inverse of the error function erf(x) = 2/\sqrt{PI} * \int_0^x e^{-t^2} dt.</returns>
        public override double InvErf(double x)
        {
            return GetStandardNormalInverseCdfValue(0.5 * x + 0.5) / MathConsts.Sqrt2;
        }

        /// <summary>Gets a specific value of the inverse of the complementary error function erfc(x) = 1- erf(x) = 2/\sqrt{PI} * \int_x^\infty e^{-t^2} dt.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <returns>The specified value of the inverse of the complementary error function erfc(x) = 1- erf(x) = 2/\sqrt{PI} * \int_x^\infty e^{-t^2} dt.</returns>
        public override double InvErfc(double x)
        {
            return InvErf(1.0 - x);
        }

        /// <summary>Evaluate the cummulative distribution function of a standard normal random variable at a specified point.
        /// </summary>
        /// <param name="x">The value where to evaluate.</param>
        /// <returns>A <see cref="System.Double" /> which reflects the value of the cummulative distribution function.</returns>
        /// <remarks>The implementation is based on <para>'Better approximations to cumulative normal functions', G. West.</para></remarks>
        public override double GetStandardNormalCdfValue(double x)
        {
            double xAbs = Math.Abs(x);
            double cdfValue = 0.0;
            double build;

            if (xAbs <= 37)
            {
                double expontential = Math.Exp(-xAbs * xAbs / 2.0);
                if (xAbs < 7.07106781186547)
                {
                    build = 3.52624965998911E-02 * xAbs + 0.700383064443688;
                    build = build * xAbs + 6.37396220353165;
                    build = build * xAbs + 33.912866078383;
                    build = build * xAbs + 112.079291497871;
                    build = build * xAbs + 221.213596169931;
                    build = build * xAbs + 220.206867912376;
                    cdfValue = expontential * build;
                    build = 8.83883476483184E-02 * xAbs + 1.75566716318264;
                    build = build * xAbs + 16.064177579207;
                    build = build * xAbs + 86.7807322029461;
                    build = build * xAbs + 296.564248779674;
                    build = build * xAbs + 637.333633378831;
                    build = build * xAbs + 793.826512519948;
                    build = build * xAbs + 440.413735824752;
                    cdfValue = cdfValue / build;
                }
                else
                {
                    build = xAbs + 0.65;
                    build = xAbs + 4 / build;
                    build = xAbs + 3 / build;
                    build = xAbs + 2 / build;
                    build = xAbs + 1 / build;
                    cdfValue = expontential / build / 2.506628274631;
                }
            }
            if (x > 0)
            {
                return 1.0 - cdfValue;
            }
            return cdfValue;
        }

        /// <summary>Gets the inverse of the standard cummulative distribution function.
        /// </summary>
        /// <param name="probability">The probability where to evaluate.</param>
        /// <returns>A <see cref="System.Double" /> which reflects the value of the inverse cummulative distribution function.</returns>
        /// <remarks>The algorithm uses a minimax approximation by rational functions and the result has a relative error whose absolute value is less 
        /// than 1.15e-9. Adapted from Peter's Perl version by Chad Sprouse (http://home.online.no/~pjacklam/notes/invnorm/impl/sprouse/).</remarks>
        public override double GetStandardNormalInverseCdfValue(double probability)
        {
            double q, r;

            if ((probability < 0.0) || (probability > 1))
            {
                throw new ArgumentOutOfRangeException(nameof(probability), String.Format(ExceptionMessages.ArgumentOutOfConstraint, probability, "[0;1]"));
            }
            else if (probability == 0)
            {
                return double.MinValue;
            }
            else if (probability == 1)
            {
                return double.MaxValue;
            }
            else if (probability < sm_LOW) /* Rational approximation for lower region */
            {
                q = Math.Sqrt(-2 * Math.Log(probability));
                return (((((sm_InvCDFCoefficientC[0] * q + sm_InvCDFCoefficientC[1]) * q + sm_InvCDFCoefficientC[2]) * q + sm_InvCDFCoefficientC[3]) * q + sm_InvCDFCoefficientC[4]) * q + sm_InvCDFCoefficientC[5]) /
                    ((((sm_InvCDFCoefficientD[0] * q + sm_InvCDFCoefficientD[1]) * q + sm_InvCDFCoefficientD[2]) * q + sm_InvCDFCoefficientD[3]) * q + 1);
            }
            else if (probability > sm_HIGH) /* Rational approximation for upper region */
            {
                q = Math.Sqrt(-2 * Math.Log(1 - probability));
                return -(((((sm_InvCDFCoefficientC[0] * q + sm_InvCDFCoefficientC[1]) * q + sm_InvCDFCoefficientC[2]) * q + sm_InvCDFCoefficientC[3]) * q + sm_InvCDFCoefficientC[4]) * q + sm_InvCDFCoefficientC[5]) /
                    ((((sm_InvCDFCoefficientD[0] * q + sm_InvCDFCoefficientD[1]) * q + sm_InvCDFCoefficientD[2]) * q + sm_InvCDFCoefficientD[3]) * q + 1);
            }
            else
            {
                /* Rational approximation for central region */
                q = probability - 0.5;
                r = q * q;
                return (((((sm_InvCDFCoefficientA[0] * r + sm_InvCDFCoefficientA[1]) * r + sm_InvCDFCoefficientA[2]) * r + sm_InvCDFCoefficientA[3]) * r + sm_InvCDFCoefficientA[4]) * r + sm_InvCDFCoefficientA[5]) * q /
                    (((((sm_InvCDFCoefficientB[0] * r + sm_InvCDFCoefficientB[1]) * r + sm_InvCDFCoefficientB[2]) * r + sm_InvCDFCoefficientB[3]) * r + sm_InvCDFCoefficientB[4]) * r + 1);
            }
        }
        #endregion
    }
}