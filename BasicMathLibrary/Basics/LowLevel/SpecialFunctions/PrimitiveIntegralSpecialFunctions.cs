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

namespace Dodoni.MathLibrary.Basics.LowLevel
{
    /// <summary>Provides methods for calculation of antiderivatives (i.e. Primitive Integral, Indefinite Integral) of elementary functions.
    /// </summary>
    public abstract class PrimitiveIntegralSpecialFunctions
    {
        #region protected constructors

        /// <summary>Initializes a new instance of the <see cref="PrimitiveIntegralSpecialFunctions" /> class.
        /// </summary>
        protected PrimitiveIntegralSpecialFunctions()
        {
        }
        #endregion

        #region public methods

        /// <summary>Gets a specific value of the error function erf(x) = 2/\sqrt{PI} * \int_0^x e^{-t^2} dt.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <returns>The specified value of the error function erf(x) = 2/\sqrt{PI} * \int_0^x e^{-t^2} dt.</returns>
        public abstract double Erf(double x);

        /// <summary>Gets a specific value of the inverse of the error function erf(x) = 2/\sqrt{PI} * \int_0^x e^{-t^2} dt.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <returns>The specified value of the inverse of the error function erf(x) = 2/\sqrt{PI} * \int_0^x e^{-t^2} dt.</returns>
        public abstract double InvErf(double x);

        /// <summary>Gets a specific value of the complementary error function erfc(x) = 1- erf(x) = 2/\sqrt{PI} * \int_x^\infty e^{-t^2} dt.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <returns>The specified value of the complementary error function erfc(x) = 1- erf(x) = 2/\sqrt{PI} * \int_x^\infty e^{-t^2} dt.</returns>
        public abstract double Erfc(double x);

        /// <summary>Gets a specific value of the inverse of the complementary error function erfc(x) = 1- erf(x) = 2/\sqrt{PI} * \int_x^\infty e^{-t^2} dt.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <returns>The specified value of the inverse of the complementary error function erfc(x) = 1- erf(x) = 2/\sqrt{PI} * \int_x^\infty e^{-t^2} dt.</returns>
        public abstract double InvErfc(double x);

        /// <summary>Gets a specific value of the exponentially scaled complementary error function erfce(x) = exp(x^2) * ( 1- erf(x) ) = exp(x^2) * 2/\sqrt{PI} * \int_x^\infty e^{-t^2} dt.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <returns>The specified value of the exponentially scaled complementary error function erfce(x) = exp(x^2) * ( 1- erf(x) )  = exp(x^2) * 2/\sqrt{PI} * \int_x^\infty e^{-t^2} dt.</returns>
        /// <remarks>Use this method to avoid arithmetic underflow.</remarks>
        public abstract double Erfce(double x);

        /// <summary>Evaluate the cummulative distribution function of a standard normal random variable at a specified point.
        /// </summary>
        /// <param name="x">The value where to evaluate.</param>
        /// <returns>A <see cref="System.Double"/> which reflects the value of the cummulative distribution function.</returns>
        /// <remarks>This method is used for <see cref="Dodoni.MathLibrary.ProbabilityTheory.Distributions.StandardNormalDistribution.GetCdfValue(double)"/> and allows
        /// a different implementation than based on <see cref="Erf(double)"/>.</remarks>
        public virtual double GetStandardNormalCdfValue(double x)
        {
            return 0.5 + 0.5 * Erf(x / MathConsts.Sqrt2);
        }

        /// <summary>Gets the inverse of the standard cummulative distribution function.
        /// </summary>
        /// <param name="probability">The probability where to evaluate.</param>
        /// <returns>A <see cref="System.Double"/> which reflects the value of the inverse cummulative distribution function.</returns>
        /// <remarks>This method is used for <see cref="Dodoni.MathLibrary.ProbabilityTheory.Distributions.StandardNormalDistribution.GetInverseCdfValue(double)"/> and allows
        /// a different implementation than based on <see cref="InvErf(double)"/>.</remarks>
        public virtual double GetStandardNormalInverseCdfValue(double probability)
        {
            return MathConsts.Sqrt2 / Erf(2 * probability - 1.0);
        }
        #endregion
    }
}