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

namespace Dodoni.Finance.StandardModels.Bachelier
{
    public partial class ImpliedBachelierVolatilityApproach
    {
        /// <summary>Represents the Successive Over-relaxation method (SOR algorithm), based on <para>M. Lee, K. Lee, 'An adaptive successive over-relaxation method for computing the Black-Scholes Implied Volatility', 2008.</para>
        /// </summary>
        /// <remarks>The initial value is computed with respect to the rational approximation, based on 'Numerical approximation of the implied volatility under arithmetic Brownian motion', Jaehyuk Choi, Kwangmoon Kim, and Minsuk Kwak, 2007.</remarks>
        public class SOR : BachelierEuropeanCall.IImpliedVolatilityApproach, BachelierEuropeanPut.IImpliedVolatilityApproach, BachelierEuropeanStraddle.IImpliedVolatilityApproach
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
                var c0 = noneDiscountedValue / forward;  // dimensionless option price

                return TryGetImpliedCallVolatility(strike, forward, timeToExpiry, c0, out value);
            }
            #endregion

            #region BachelierEuropeanPut.IImpliedVolatilityApproach Members

            /// <summary>Gets the implied Black volatility for european put option.
            /// </summary>
            /// <param name="strike">The strike.</param>
            /// <param name="forward">The forward.</param>
            /// <param name="timeToExpiry">The time span between valuation date and expiry date in its <see cref="System.Double"/> representation.</param>
            /// <param name="noneDiscountedValue">The value of the option at the time of expiry, thus the price but <b>not</b> discounted to time 0.</param>
            /// <param name="value">The implied Black volatility (output).</param>
            /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
            ImpliedCalculationResultState BachelierEuropeanPut.IImpliedVolatilityApproach.TryGetValue(double strike, double forward, double timeToExpiry, double noneDiscountedValue, out double value)
            {
                // apply call-put parity
                double c0 = noneDiscountedValue / forward + 1.0 - strike / forward;

                return TryGetImpliedCallVolatility(strike, forward, timeToExpiry, c0, out value);
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
                var adjNormalizedCallPrice = 0.5 * (noneDiscountedValue / forward - strike / forward + 1); // value of straddle is call + put
                return TryGetImpliedCallVolatility(strike, forward, timeToExpiry, adjNormalizedCallPrice, out value);
            }
            #endregion

            /// <summary>Returns a <see cref="System.String" /> that represents this instance.
            /// </summary>
            /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
            public override string ToString()
            {
                return "SOR Implied Bachelier volatility approach";
            }
            #endregion

            #region public static methods

            /// <summary>Creates a new <see cref="SOR"/> instance.
            /// </summary>
            /// <param name="maxNumberOfIterations">The maximal number of iterations.</param>
            /// <param name="tolerance">The tolerance taken into account as exit condition with respect to the dimensionless option prices.</param>
            /// <param name="w">The relaxation parameter 'w'.</param>
            public static SOR Create(int maxNumberOfIterations = 10, double tolerance = MachineConsts.Epsilon, double w = 0.0)
            {
                return new SOR(maxNumberOfIterations, tolerance, w);
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
                /* Initial value is taken from rational approximation of a straddle option. The algorithm converges with quaratic order of convergence, therefore
                 * for an arbitrary point for which the algorithm is well-defined. Because of the good initial point, often it is one iteration required only. */

                double v0;
                double sqrtOfTime = Math.Sqrt(timeToExpiry);
                if (RationalApproximation.TryGetStraddleImpliedVolatility(strike - forward, sqrtOfTime, 2.0 * c0 * forward + strike - forward, out value) == ImpliedCalculationResultState.ProperResult)
                {
                    v0 = value * sqrtOfTime / forward;
                }
                else
                {
                    v0 = 0.1 * sqrtOfTime / forward;  // default if approximation fails.
                }

                var x = 1 - strike / forward;

                if (x > 0) // "in-out" duality
                {
                    c0 = c0 - x;
                    x = -x;
                }
                var oneOverOnePlusW = 1.0 / (1 + m_RelaxationParameter);

                var v = v0;
                for (int j = 0; j < m_MaxNumberOfIterations; j++)
                {
                    var n = StandardNormalDistribution.GetPdfValue(x / v);
                    var N = StandardNormalDistribution.GetCdfValue(x / v);
                    double c = v * n + x * N;  // undiscounted normalized option value

                    v = (c0 - x * N + m_RelaxationParameter * v * n) * oneOverOnePlusW / n;

                    if (Math.Abs(c - c0) < m_Tolerance)
                    {
                        value = forward * v / sqrtOfTime;  // here, we assume that the 'new' total volatility estimation is better than the one used for the calculation of 'c'
                        return ImpliedCalculationResultState.ProperResult;
                    }
                }
                value = v / sqrtOfTime;
                return ImpliedCalculationResultState.NoProperResult;
            }
            #endregion
        }
    }
}