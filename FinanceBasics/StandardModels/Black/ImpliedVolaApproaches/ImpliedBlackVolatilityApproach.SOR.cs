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
using Dodoni.MathLibrary.ProbabilityTheory.Distributions;

namespace Dodoni.Finance.StandardModels.Black
{
    public partial class ImpliedBlackVolatilityApproach
    {
        /// <summary>Represents the Successive Over-relaxation method (SOR algorithm), based on <para>M. Lee, K. Lee, 'An adaptive successive over-relaxation method for computing the Black-Scholes Implied Volatility', 2008.</para>
        /// </summary>
        public class SOR : BlackEuropeanCall.IImpliedVolatilityApproach, BlackEuropeanPut.IImpliedVolatilityApproach, BlackEuropeanStraddle.IImpliedVolatilityApproach
        {
            #region private members

            /// <summary>The maximal number of iterations for the computation of the implied volatility.
            /// </summary>
            private int m_MaxNumberOfIterations;

            /// <summary>The tolerance.
            /// </summary>
            private double m_Tolerance;

            /// <summary>The relaxation parameter 'w'.
            /// </summary>
            private double m_RelaxationParameter;

            private const double m00 = -0.00006103098165;
            private const double m01 = 5.33967643357688;
            private const double m10 = -0.40661990365427;
            private const double m02 = 3.25023425332360;
            private const double m11 = -36.19405221599028;
            private const double m20 = 0.08975394404851;
            private const double m03 = 83.84593224417796;
            private const double m12 = 41.21772632732834;
            private const double m21 = 3.83815885394565;
            private const double m30 = -0.21619763215668;

            private const double n00 = 1.0;
            private const double n01 = 22.96302109010794;
            private const double n10 = -0.48466536361620;
            private const double n02 = -0.77268824532468;
            private const double n11 = -1.34102279982050;
            private const double n20 = 0.43027619553168;
            private const double n03 = -5.70531500645109;
            private const double n12 = 2.45782574294244;
            private const double n21 = -0.04763802358853;
            private const double n30 = -0.03326944290044;
            #endregion

            #region private constructors

            /// <summary>Initializes a new instance of the <see cref="SOR"/> class.
            /// </summary>
            /// <param name="maxNumberOfIterations">The maximal number of iterations.</param>
            /// <param name="tolerance">The tolerance taken into account as exit condition with respect to the dimensionless option prices.</param>
            /// <param name="w">The relaxation parameter 'w'.</param>
            private SOR(int maxNumberOfIterations, double tolerance, double w)
            {
                m_MaxNumberOfIterations = maxNumberOfIterations;
                m_Tolerance = tolerance;
                m_RelaxationParameter = w;
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
                var c0 = noneDiscountedValue / forward;  // dimensionless option price

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
                double c0 = noneDiscountedValue / forward + 1.0 - strike / forward;

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

            /// <summary>Returns a <see cref="System.String" /> that represents this instance.
            /// </summary>
            /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
            public override string ToString()
            {
                return "SOR Implied Black volatility approach";
            }
            #endregion

            #region public static methods

            /// <summary>Creates a new <see cref="SOR"/> instance.
            /// </summary>
            /// <param name="maxNumberOfIterations">The maximal number of iterations.</param>
            /// <param name="tolerance">The tolerance taken into account as exit condition with respect to the dimensionless option prices.</param>
            /// <param name="w">The relaxation parameter 'w'.</param>
            public static SOR Create(int maxNumberOfIterations = 10, double tolerance = MachineConsts.Epsilon, double w = 1.0)
            {
                return new SOR(maxNumberOfIterations, tolerance, w);
            }
            #endregion

            #region internal static methods

            /// <summary>Gets the initial total volatility v = \sigma * \sqrt{time-to-expiry}.
            /// </summary>
            /// <param name="x">The log-moneyness, i.e. log(Forward/Strike).</param>
            /// <param name="c">The normalized call option price.</param>
            /// <returns>A total volatility v that implies a Black price that is approximative <paramref name="c"/>.</returns>
            internal static double GetInitialCallOptionTotalVolatility(double x, double c)
            {
                var numerator = m00 + m01 * c + m02 * c * c + m03 * c * c * c + m10 * x + m11 * x * c + m12 * x * c * c + m20 * x * x + m21 * x * x * c + m30 * x * x * x;
                var denumerator = n00 + n01 * c + n02 * c * c + n03 * c * c * c + n10 * x + n11 * x * c + n12 * x * c * c + n20 * x * x + n21 * x * x * c + n30 * x * x * x;

                return numerator / denumerator;
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
                var x = Math.Log(forward / strike);
                var expOfMinusx = strike / forward;

                if (x > 0) // "in-out" duality
                {
                    x = -x;
                    expOfMinusx = forward / strike;

                    c0 = expOfMinusx * c0 + 1 - expOfMinusx;  // expOfMinusx = exp( [original] x )
                }
                var v0 = GetInitialCallOptionTotalVolatility(x, c0);
                var oneOverOnePlusW = 1.0 / (1 + m_RelaxationParameter);

                var v = v0;
                for (int j = 0; j < m_MaxNumberOfIterations; j++)
                {
                    var nPlus = StandardNormalDistribution.GetCdfValue(x / v + 0.5 * v);
                    var nMinus = expOfMinusx * StandardNormalDistribution.GetCdfValue(x / v - 0.5 * v);
                    double c = nPlus - nMinus;  // undiscounted normalized option value

                    var temp = StandardNormalDistribution.GetInverseCdfValue((c0 + nMinus + m_RelaxationParameter * nPlus) * oneOverOnePlusW);
                    v = temp + Math.Sqrt(temp * temp + 2.0 * Math.Abs(x));

                    if (Math.Abs(c - c0) < m_Tolerance)
                    {
                        value = v / Math.Sqrt(timeToExpiry);  // here, we assume that the 'new' total volatility estimation is better than the one used for the calculation of 'c'
                        return ImpliedCalculationResultState.ProperResult;
                    }
                }
                value = v / Math.Sqrt(timeToExpiry);
                return ImpliedCalculationResultState.NoProperResult;
            }
            #endregion
        }
    }
}