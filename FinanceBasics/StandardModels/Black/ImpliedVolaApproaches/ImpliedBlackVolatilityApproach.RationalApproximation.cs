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

namespace Dodoni.Finance.StandardModels.Black
{
    public partial class ImpliedBlackVolatilityApproach
    {
        /// <summary>Represents a fast but perhaps not accurate implementation of the implied Black volatility based on a rational function approximation.
        /// </summary>
        /// <remarks>The implementation is based on <para>'You don't have to Bother Newton for Implied volatility', Minquiang Li, November 2009.</para></remarks>
        public class RationalApproximation : BlackEuropeanCall.IImpliedVolatilityApproach, BlackEuropeanPut.IImpliedVolatilityApproach, BlackEuropeanStraddle.IImpliedVolatilityApproach
        {
            #region private members

            private const double p1 = -0.969271876255;
            private const double p2 = 0.097428338274;
            private const double p3 = 1.750081126685;

            private const double n_0_1 = -0.068098378725;
            private const double m_0_1 = 6.268456292246;
            private const double n_1_0 = 0.440639436211;
            private const double m_1_0 = -6.284840445036;
            private const double n_0_2 = -0.263473754689;
            private const double m_0_2 = 30.068281276567;
            private const double n_1_1 = -5.792537721792;
            private const double m_1_1 = -11.780036995036;
            private const double n_2_0 = -5.267481008429;
            private const double m_2_0 = -2.310966989723;
            private const double n_0_3 = 4.714393825758;
            private const double m_0_3 = -11.473184324152;
            private const double n_1_2 = 3.529944137559;
            private const double m_1_2 = -230.101682610568;
            private const double n_2_1 = -23.636495876611;
            private const double m_2_1 = 86.127219899668;
            private const double n_3_0 = -9.020361771283;
            private const double m_3_0 = 3.730181294225;
            private const double n_0_4 = 14.749084301452;
            private const double m_0_4 = -13.954993561151;
            private const double n_1_3 = -32.570660102526;
            private const double m_1_3 = 261.950288864225;
            private const double n_2_2 = 76.398155779133;
            private const double m_2_2 = 20.090690444187;
            private const double n_3_1 = 41.855161781749;
            private const double m_3_1 = -50.117067019539;
            private const double n_4_0 = -12.150611865704;
            private const double m_4_0 = 13.723711519422;
            #endregion

            #region private constructors

            /// <summary>Initializes a new instance of the <see cref="RationalApproximation"/> class.
            /// </summary>
            private RationalApproximation()
            {
            }
            #endregion

            #region public methods

            #region BlackEuropeanCall.IImpliedVolatilityApproach Members

            /// <summary>Gets the implied Black volatility of a specific european call option.
            /// </summary>
            /// <param name="strike">The strike.</param>
            /// <param name="forward">The forward.</param>
            /// <param name="timeToExpiry">The time span between valuation date and expiry date in its <see cref="System.Double"/> representation.</param>
            /// <param name="noneDiscountedValue">The value of the option at the time of expiry, thus the price but <b>not</b> discounted to time 0.</param>
            /// <param name="value">The implied Black volatility (output).</param>
            /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
            ImpliedCalculationResultState BlackEuropeanCall.IImpliedVolatilityApproach.TryGetValue(double strike, double forward, double timeToExpiry, double noneDiscountedValue, out double value)
            {
                var c0 = noneDiscountedValue / forward; // normalized call option price
                return TryGetImpliedCallVolatility(strike, forward, timeToExpiry, c0, out value);
            }
            #endregion

            #region BlackEuropeanPut.IImpliedVolatilityApproach Members

            /// <summary>Gets the implied Black volatility for european put option.
            /// </summary>
            /// <param name="strike">The strike.</param>
            /// <param name="forward">The forward.</param>
            /// <param name="timeToExpiry">The time span between valuation date and expiry date in its <see cref="System.Double"/> representation.</param>
            /// <param name="noneDiscountedValue">The value of the option at the time of expiry, thus the price but <b>not</b> discounted to time 0.</param>
            /// <param name="value">The implied Black volatility (output).</param>
            /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
            ImpliedCalculationResultState BlackEuropeanPut.IImpliedVolatilityApproach.TryGetValue(double strike, double forward, double timeToExpiry, double noneDiscountedValue, out double value)
            {
                // apply call-put parity
                var c0 = noneDiscountedValue / forward + 1.0 - strike / forward;
                return TryGetImpliedCallVolatility(strike, forward, timeToExpiry, c0, out value);
            }
            #endregion

            #region BlackEuropeanStraddle.IImpliedVolatilityApproach Members

            /// <summary>Gets the implied Black volatility of a specific european Straddle option.
            /// </summary>
            /// <param name="strike">The strike.</param>
            /// <param name="forward">The forward.</param>
            /// <param name="timeToExpiry">The time span between valuation date and expiry date in its <see cref="System.Double"/> representation.</param>
            /// <param name="noneDiscountedValue">The value of the option at the time of expiry, thus the price but <b>not</b> discounted to time 0.</param>
            /// <param name="value">The implied Black volatility (output).</param>
            /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
            ImpliedCalculationResultState BlackEuropeanStraddle.IImpliedVolatilityApproach.TryGetValue(double strike, double forward, double timeToExpiry, double noneDiscountedValue, out double value)
            {
                var adjNormalizedCallPrice = 0.5 * (noneDiscountedValue / forward - strike / forward + 1); // value of straddle is call + put
                return TryGetImpliedCallVolatility(strike, forward, timeToExpiry, adjNormalizedCallPrice, out value);
            }
            #endregion

            /// <summary>Returns a <see cref="System.String"/> that represents this instance.
            /// </summary>
            /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
            public override string ToString()
            {
                return "Rational approximation Black Implied volatility";
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

            #region private methods

            /// <summary>Gets the implied Black volatility of a specific european call option.
            /// </summary>
            /// <param name="strike">The strike.</param>
            /// <param name="forward">The forward.</param>
            /// <param name="timeToExpiry">The time span between valuation date and expiry date in its <see cref="System.Double"/> representation.</param>
            /// <param name="c0">The dimensionless option price, i.e. option price divided by forward and discount factor.</param>
            /// <param name="value">The implied Black volatility (output).</param>
            /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
            private ImpliedCalculationResultState TryGetImpliedCallVolatility(double strike, double forward, double timeToExpiry, double c0, out double value)
            {
                value = Double.NaN;
                double x = Math.Log(forward / strike); // log-moneyness 

                if (Double.IsNaN(x))  // strike <= 0?
                {
                    return ImpliedCalculationResultState.InputError;
                }

                /* check whether the given value is inside the valid range (intrinsic value etc.) It follows a reformulation of the condition in 'By implication', Jäckel: */
                if (c0 > 1)  // i.e. OptionPrice > Forward * discountFactor
                {
                    return ImpliedCalculationResultState.InputError;
                }
                if (c0 < DoMath.HeavisideFunction(x) * (1 - strike / forward))
                {
                    return ImpliedCalculationResultState.InputError;
                }

                if (x > 0) // use "in-out" duality, i.e. arguments should be inside domain D_
                {
                    c0 = forward / strike * c0 + 1.0 - forward / strike;
                    x = -x;
                }

                /* check whether the arguments are inside the domain D_ */
                double lowerBound = (-0.00424532412773 * x + 0.00099075112125 * x * x) / (1 + 0.26674393279214 * x + 0.03360553011959 * x * x); // approx. = c(x,-x/2), formula (17)
                double upperBound = (0.38292495908775 + 0.31382372544666 * x + 0.07116503261172 * x * x) / (1 + 0.01380361926221 * x + 0.11791124749938 * x * x);  // approx. = c(x,1), formula (18)

                if ((x < -0.5) || (c0 < lowerBound) || (c0 > upperBound))
                {
                    return ImpliedCalculationResultState.NoProperResult;  // approximation not applictable
                }

                double sqrtOfC = Math.Sqrt(c0);

                double numerator = 0;
                double denominator = 0;

                // i=0:
                numerator = sqrtOfC * (n_0_1 + sqrtOfC * (n_0_2 + sqrtOfC * (n_0_3 + sqrtOfC * n_0_4)));
                denominator = sqrtOfC * (m_0_1 + sqrtOfC * (m_0_2 + sqrtOfC * (m_0_3 + sqrtOfC * m_0_4)));

                // i=1:
                numerator += x * (n_1_0 + sqrtOfC * (n_1_1 + sqrtOfC * (n_1_2 + sqrtOfC * n_1_3)));
                denominator += x * (m_1_0 + sqrtOfC * (m_1_1 + sqrtOfC * (m_1_2 + sqrtOfC * m_1_3)));

                // i=2:
                double xx = x * x;
                numerator += xx * (n_2_0 + sqrtOfC * (n_2_1 + sqrtOfC * n_2_2));
                denominator += xx * (m_2_0 + sqrtOfC * (m_2_1 + sqrtOfC * m_2_2));

                // i=3:
                numerator += xx * x * (n_3_0 + sqrtOfC * n_3_1);
                denominator += xx * x * (m_3_0 + sqrtOfC * m_3_1);

                value = (p1 * x + p2 * sqrtOfC + p3 * c0 + numerator / (1.0 + denominator)) / Math.Sqrt(timeToExpiry);

                return ImpliedCalculationResultState.ProperResult;
            }
            #endregion
        }
    }
}