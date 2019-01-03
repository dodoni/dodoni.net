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
        /// <summary>Represents the Successive Over-relaxation method - Transformation of sequence (SOR-TS algorithm), based on <para>An adaptive successive over-relaxation method for computing the Black-Scholes Implied Volatility, 2008.</para>
        /// </summary>
        public class SOR_TS : BlackEuropeanCall.IImpliedVolatilityApproach, BlackEuropeanPut.IImpliedVolatilityApproach, BlackEuropeanStraddle.IImpliedVolatilityApproach
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

            /// <summary>Initializes a new instance of the <see cref="SOR_TS"/> class.
            /// </summary>
            /// <param name="w">The relaxation parameter 'w'.</param>
            /// <param name="maxNumberOfIterations">The maximal number of iterations.</param>
            /// <param name="tolerance">The tolerance taken into account as exit condition with respect to the dimensionless option prices.</param>
            private SOR_TS(double w, int maxNumberOfIterations, double tolerance)
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
                return "SOR-TS Implied Black Volatility approach";
            }
            #endregion

            #region public static methods

            /// <summary>Creates a new <see cref="SOR_TS"/> instance.
            /// </summary>
            /// <param name="maxNumberOfIterations">The maximal number of iterations.</param>
            /// <param name="tolerance">The tolerance taken into account as exit condition with respect to the dimensionless option prices.</param>
            /// <param name="w">The relaxation parameter 'w'.</param>
            public static SOR_TS Create(int maxNumberOfIterations = 7, double tolerance = MachineConsts.Epsilon, double w = 1.0)
            {
                return new SOR_TS(w, maxNumberOfIterations, tolerance);
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
                var v0 = SOR.GetInitialCallOptionTotalVolatility(x, c0);
                var alpha0 = (1 + m_RelaxationParameter) / (1 + (v0 * v0 - 2 * Math.Abs(x)) / (v0 * v0 + 2 * Math.Abs(x)));
                var oneOverOnePlusW = 1.0 / (1 + m_RelaxationParameter);

                var v = v0;
                var alpha = alpha0;
                for (int j = 0; j < m_MaxNumberOfIterations; j++)
                {
                    var nPlus = StandardNormalDistribution.GetCdfValue(x / v + 0.5 * v);
                    var nMinus = expOfMinusx * StandardNormalDistribution.GetCdfValue(x / v - 0.5 * v);
                    double c = nPlus - nMinus;  // undiscounted normalized option value

                    var temp = StandardNormalDistribution.GetInverseCdfValue((c0 + nMinus + m_RelaxationParameter * nPlus) * oneOverOnePlusW);

                    v = alpha * (temp + Math.Sqrt(temp * temp + 2.0 * Math.Abs(x))) + (1 - alpha) * v;
                    alpha = (1 + m_RelaxationParameter) / (1 + (v * v - 2 * Math.Abs(x)) / (v * v + 2 * Math.Abs(x)));

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