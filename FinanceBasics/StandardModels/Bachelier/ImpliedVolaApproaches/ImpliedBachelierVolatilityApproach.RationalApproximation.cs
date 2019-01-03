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

using Dodoni.MathLibrary;

namespace Dodoni.Finance.StandardModels.Bachelier
{
    public partial class ImpliedBachelierVolatilityApproach
    {
        /// <summary>Represents the implementation of the implied Bachelier ('normal' Black) volatility approach based on
        /// <para>'Numerical approximation of the implied volatility under arithmetic Brownian motion', Jaehyuk Choi, Kwangmoon Kim, and Minsuk Kwak, 2007.</para>
        /// </summary>
        public class RationalApproximation : BachelierEuropeanCall.IImpliedVolatilityApproach, BachelierEuropeanPut.IImpliedVolatilityApproach, BachelierEuropeanStraddle.IImpliedVolatilityApproach
        {
            #region private const members

            /// <summary>The lower bound of \eta with respect to 'Numerical approximation of the implied volatility under arithmetic Brownian motion', J. Choi, K. Kim, M. Kwak, 2007.
            /// </summary>
            private const double m_EtaLowerBound = 0.049;

            /// <summary>The upper bound of \eta with respect to 'Numerical approximation of the implied volatility under arithmetic Brownian motion', J. Choi, K. Kim, M. Kwak, 2007.
            /// </summary>
            private const double m_EtaUpperBound = 1.0;

            /// <summary>Constant with respect to 'Numerical approximation of the implied volatility under arithmetic Brownian motion', J. Choi, K. Kim, M. Kwak, 2007.
            /// </summary>
            private const double a0 = 3.994961687345134E-1;

            /// <summary>Constant with respect to 'Numerical approximation of the implied volatility under arithmetic Brownian motion', J. Choi, K. Kim, M. Kwak, 2007.
            /// </summary>
            private const double a1 = 2.100960795068497E+1;

            /// <summary>Constant with respect to 'Numerical approximation of the implied volatility under arithmetic Brownian motion', J. Choi, K. Kim, M. Kwak, 2007.
            /// </summary>
            private const double a2 = 4.980340217855084E+1;

            /// <summary>Constant with respect to 'Numerical approximation of the implied volatility under arithmetic Brownian motion', J. Choi, K. Kim, M. Kwak, 2007.
            /// </summary>
            private const double a3 = 5.988761102690991E+2;

            /// <summary>Constant with respect to 'Numerical approximation of the implied volatility under arithmetic Brownian motion', J. Choi, K. Kim, M. Kwak, 2007.
            /// </summary>
            private const double a4 = 1.848489695437094E+3;

            /// <summary>Constant with respect to 'Numerical approximation of the implied volatility under arithmetic Brownian motion', J. Choi, K. Kim, M. Kwak, 2007.
            /// </summary>
            private const double a5 = 6.106322407867059E+3;

            /// <summary>Constant with respect to 'Numerical approximation of the implied volatility under arithmetic Brownian motion', J. Choi, K. Kim, M. Kwak, 2007.
            /// </summary>
            private const double a6 = 2.493415285349361E+4;

            /// <summary>Constant with respect to 'Numerical approximation of the implied volatility under arithmetic Brownian motion', J. Choi, K. Kim, M. Kwak, 2007.
            /// </summary>
            private const double a7 = 1.266458051348246E+4;

            /// <summary>Constant with respect to 'Numerical approximation of the implied volatility under arithmetic Brownian motion', J. Choi, K. Kim, M. Kwak, 2007.
            /// </summary>
            private const double b0 = 1.000000000000000E+0;

            /// <summary>Constant with respect to 'Numerical approximation of the implied volatility under arithmetic Brownian motion', J. Choi, K. Kim, M. Kwak, 2007.
            /// </summary>
            private const double b1 = 4.990534153589422E+1;

            /// <summary>Constant with respect to 'Numerical approximation of the implied volatility under arithmetic Brownian motion', J. Choi, K. Kim, M. Kwak, 2007.
            /// </summary>
            private const double b2 = 3.093573936743112E+1;

            /// <summary>Constant with respect to 'Numerical approximation of the implied volatility under arithmetic Brownian motion', J. Choi, K. Kim, M. Kwak, 2007.
            /// </summary>
            private const double b3 = 1.495105008310999E+3;

            /// <summary>Constant with respect to 'Numerical approximation of the implied volatility under arithmetic Brownian motion', J. Choi, K. Kim, M. Kwak, 2007.
            /// </summary>
            private const double b4 = 1.323614537899738E+3;

            /// <summary>Constant with respect to 'Numerical approximation of the implied volatility under arithmetic Brownian motion', J. Choi, K. Kim, M. Kwak, 2007.
            /// </summary>
            private const double b5 = 1.598919697679745E+4;

            /// <summary>Constant with respect to 'Numerical approximation of the implied volatility under arithmetic Brownian motion', J. Choi, K. Kim, M. Kwak, 2007.
            /// </summary>
            private const double b6 = 2.392008891720782E+4;

            /// <summary>Constant with respect to 'Numerical approximation of the implied volatility under arithmetic Brownian motion', J. Choi, K. Kim, M. Kwak, 2007.
            /// </summary>
            private const double b7 = 3.608817108375034E+3;

            /// <summary>Constant with respect to 'Numerical approximation of the implied volatility under arithmetic Brownian motion', J. Choi, K. Kim, M. Kwak, 2007.
            /// </summary>
            private const double b8 = -2.067719486400926E+2;

            /// <summary>Constant with respect to 'Numerical approximation of the implied volatility under arithmetic Brownian motion', J. Choi, K. Kim, M. Kwak, 2007.
            /// </summary>
            private const double b9 = 1.174240599306013E+1;
            #endregion

            #region private constructors

            /// <summary>Initializes a new instance of the <see cref="RationalApproximation" /> class.
            /// </summary>
            private RationalApproximation()
            {
            }
            #endregion

            #region public methods

            #region BachelierEuropeanCall.IImpliedVolatilityApproach Members

            /// <summary>Gets the implied Black volatility of a specific european call option.
            /// </summary>
            /// <param name="strike">The strike.</param>
            /// <param name="forward">The forward.</param>
            /// <param name="timeToExpiry">The time span between valuation date and expiry date in its <see cref="System.Double"/> representation.</param>
            /// <param name="noneDiscountedValue">The value of the option at the time of expiry, thus the price but <b>not</b> discounted to time 0.</param>
            /// <param name="value">The implied Black volatility (output).</param>
            /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
            ImpliedCalculationResultState BachelierEuropeanCall.IImpliedVolatilityApproach.TryGetValue(double strike, double forward, double timeToExpiry, double noneDiscountedValue, out double value)
            {
                /* use the call-put parity to construct a straddle, i.e. the value of the put is equal to
                 *     'noneDiscountedValue + strike - forward', i.e. the value of the straddle is equal to 'noneDiscountedValue + strike - forward + noneDiscountedValue'.
                 */
                double strikeMinusForward = strike - forward;

                return TryGetStraddleImpliedVolatility(strikeMinusForward, Math.Sqrt(timeToExpiry), 2.0 * noneDiscountedValue + strikeMinusForward, out value);
            }
            #endregion

            #region BachelierEuropeanPut.IImpliedVolatilityApproach Members

            /// <summary>Gets the implied Black volatility of a specific european put option.
            /// </summary>
            /// <param name="strike">The strike.</param>
            /// <param name="forward">The forward.</param>
            /// <param name="timeToExpiry">The time span between valuation date and expiry date in its <see cref="System.Double"/> representation.</param>
            /// <param name="noneDiscountedValue">The value of the option at the time of expiry, thus the price but <b>not</b> discounted to time 0.</param>
            /// <param name="value">The implied Black volatility (output).</param>
            /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
            ImpliedCalculationResultState BachelierEuropeanPut.IImpliedVolatilityApproach.TryGetValue(double strike, double forward, double timeToExpiry, double noneDiscountedValue, out double value)
            {
                /* use the call-put parity to construct a straddle, i.e. the value of the call is equal to
                 * 'noneDiscountedValue + forward - strike',  i.e. the value of the straddle is equal to 'noneDiscountedValue + forward - strike + noneDiscountedValue'.
                 */
                return TryGetStraddleImpliedVolatility(strike - forward, Math.Sqrt(timeToExpiry), 2.0 * noneDiscountedValue + forward - strike, out value);
            }
            #endregion

            #region BachelierEuropeanStraddle.IImpliedVolatilityApproach Members

            /// <summary>Gets the implied Black volatility of a specific european Straddle option.
            /// </summary>
            /// <param name="strike">The strike.</param>
            /// <param name="forward">The forward.</param>
            /// <param name="timeToExpiry">The time span between valuation date and expiry date in its <see cref="System.Double"/> representation.</param>
            /// <param name="noneDiscountedValue">The value of the option at the time of expiry, thus the price but <b>not</b> discounted to time 0.</param>
            /// <param name="value">The implied Black volatility (output).</param>
            /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
            ImpliedCalculationResultState BachelierEuropeanStraddle.IImpliedVolatilityApproach.TryGetValue(double strike, double forward, double timeToExpiry, double noneDiscountedValue, out double value)
            {
                return TryGetStraddleImpliedVolatility(strike - forward, Math.Sqrt(timeToExpiry), noneDiscountedValue, out value);
            }
            #endregion

            /// <summary>Returns a <see cref="System.String" /> that represents this instance.
            /// </summary>
            /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
            public override string ToString()
            {
                return "Rational Approximation Bachelier Implied Volatility approach";
            }
            #endregion

            #region public static methods

            /// <summary>Creates a new <see cref="RationalApproximation"/> instance.
            /// </summary>
            public static RationalApproximation Create()
            {
                return new RationalApproximation();
            }
            #endregion

            #region internal static methods

            /// <summary>Gets the implied Bachelier (=Normal Black) volatility for a specific value (price) of a straddle option.
            /// </summary>
            /// <param name="strikeMinusForward">'Strike minus forward'.</param>
            /// <param name="sqrtOfTime">The square root of the <see cref="System.Double"/> representation of the time span between valuation date and expiry date.</param>
            /// <param name="noneDiscountedValue">The value of the straddle at the time of expiry, thus the price but <b>not</b> discounted to time 0.</param>
            /// <param name="impliedVolatility">The implied Bachelier (i.e. Normal Black) volatility (output).</param>
            /// <returns>A value indicating whether <paramref name="impliedVolatility"/> contains valid data.
            /// </returns>
            /// <remarks>The implementation is based on
            /// <para>'Numerical approximation of the implied volatility under arithmetic Brownian motion', 
            /// Jaehyuk Choi, Kwangmoon Kim, and Minsuk Kwak, 2007.</para></remarks>
            public static ImpliedCalculationResultState TryGetStraddleImpliedVolatility(double strikeMinusForward, double sqrtOfTime, double noneDiscountedValue, out double impliedVolatility)
            {
                double forwardMinusStrikeDivValue = -strikeMinusForward / noneDiscountedValue;

                /* use formula (14) & (15) and the fact 
                 * tanh^{-1}(x) = 0.5 * log( (1+x) / (1-x) ), |x| < 1
                 */

                if (Math.Abs(forwardMinusStrikeDivValue) < 1.0)  // a far-out-of-the-money option?
                {
                    double eta = 2.0 * forwardMinusStrikeDivValue / Math.Log((1.0 + forwardMinusStrikeDivValue) / (1.0 - forwardMinusStrikeDivValue));
                    if ((eta >= m_EtaLowerBound) && (eta <= m_EtaUpperBound))  // is false if NaN
                    {
                        impliedVolatility = MathConsts.SqrtPiOverTwo / sqrtOfTime * noneDiscountedValue * GetValueOfImpliedStraddleVolaHelperFunction(eta);
                        return ImpliedCalculationResultState.ProperResult;
                    }
                }
                impliedVolatility = Double.NaN;
                return ImpliedCalculationResultState.InputError;
            }
            #endregion

            #region private static methods

            /// <summary>Gets the value of the implied Bachelier (=Normal Black) volatility helper function 'h' with
            /// respect to equation (15) in 'Numerical approximation of the implied volatility under arithmetic Brownian motion', 
            /// Jaehyuk Choi, Kwangmoon Kim, and Minsuk Kwak, 2007.
            /// </summary>
            /// <param name="eta">The parameter \eta.</param>
            /// <returns>The value of h(\eta) with respect to equation (15).</returns>
            private static double GetValueOfImpliedStraddleVolaHelperFunction(double eta)
            {
                double nominator = a0 + eta * (a1 + eta * (a2 + eta * (a3 + eta * (a4 + eta * (a5 + eta * (a6 + eta * a7))))));
                double denominator = b0 + eta * (b1 + eta * (b2 + eta * (b3 + eta * (b4 + eta * (b5 + eta * (b6 + eta * (b7 + eta * (b8 + eta * b9))))))));

                return Math.Sqrt(eta) * nominator / denominator;
            }
            #endregion
        }
    }
}