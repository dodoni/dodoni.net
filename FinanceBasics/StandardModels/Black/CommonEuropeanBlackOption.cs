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
    /// <summary>Serves as abstract basis class for european options where a strike is involved and the single underlying is based on the Black model.
    /// </summary>
    /// <remarks>Instead of the spot values one has to enter the forward which may contains some dividend or a different kind of discount factor etc. In the
    /// Black-Scholes framework the forward is defined by <para>Forward = exp(T * (r+d)),</para>, where T represents the exercise date, r is the yield 
    /// and d may represents a dividend, foreign yield etc, thus exp(-(r+d)*T) is equal to the spot value of the underlying. 'd' is not an input, but the forward is.</remarks>
    public abstract class CommonEuropeanBlackOption
    {
        #region private consts

        /// <summary>The maximal number of iterations for the computation of the implied strike.
        /// </summary>
        private const int MaxNumberIterationImpliedStrike = 1000;
        #endregion

        #region private members

        /// <summary>The logarithm of the moneyness, i.e. the logarithm of forward over strike.
        /// </summary>
        /// <remarks>This value is frequently used and cached for performance reason.</remarks>
        private double m_LogOfMoneyness = Double.NaN;

        /// <summary>The square-root of the time-to-expiry.
        /// </summary>
        /// <remarks>This value is frequently used and cached for performance reason.</remarks>
        private double m_SqrtOfTimeToExpiration = Double.NaN;
        #endregion

        #region protected members

        /// <summary>The strike of the option.
        /// </summary>
        /// <remarks>This member is protected for performance reason only.</remarks>
        protected double m_Strike = Double.NaN;

        /// <summary>The forward.
        /// </summary>
        /// <remarks>This member is protected for performance reason only.</remarks>
        protected double m_Forward = Double.NaN;

        /// <summary>The time to expiry, i.e. the span between valuation date and expiry date in its <see cref="System.Double"/> representation.
        /// </summary>
        /// <remarks>This member is protected for performance reason only.</remarks>
        protected double m_TimeToExpiration = Double.NaN;

        /// <summary>The discount factor.
        /// </summary>
        /// <remarks>This member is protected for performance reason only.</remarks>
        protected double m_DiscountFactor = Double.NaN;
        #endregion

        #region protected constructors

        /// <summary>Initializes a new instance of the <see cref="CommonEuropeanBlackOption"/> class.
        /// </summary>
        protected CommonEuropeanBlackOption()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="CommonEuropeanBlackOption"/> class.
        /// </summary>
        /// <param name="strike">The strike.</param>
        /// <param name="forward">The forward.</param>
        /// <param name="timeToExpiry">The time span between valuation date and expiry date.</param>
        /// <param name="discountFactor">The discount factor at <paramref name="timeToExpiry"/>.</param>
        protected CommonEuropeanBlackOption(double strike, double forward, double timeToExpiry, double discountFactor)
        {
            m_Strike = strike;
            m_Forward = forward;
            m_LogOfMoneyness = Math.Log(forward / strike);
            m_TimeToExpiration = timeToExpiry;
            m_SqrtOfTimeToExpiration = Math.Sqrt(timeToExpiry);
            m_DiscountFactor = discountFactor;
        }
        #endregion

        #region public properties

        /// <summary>Gets or sets the time to expiry, i.e. time span between valuation date and expiry date
        /// in its <see cref="System.Double"/> representation.
        /// </summary>
        /// <value>The time to expiry.</value>
        public double TimeToExpiry
        {
            get { return m_TimeToExpiration; }
            set
            {
                m_TimeToExpiration = value;
                m_SqrtOfTimeToExpiration = Math.Sqrt(value);
            }
        }

        /// <summary>Gets or sets the strike of the option.
        /// </summary>
        /// <value>The strike.</value>
        public double Strike
        {
            get { return m_Strike; }
            set
            {
                m_Strike = value;
                m_LogOfMoneyness = Math.Log(m_Forward / value);
            }
        }

        /// <summary>Gets or sets the forward at <see cref="TimeToExpiry"/>.
        /// </summary>
        /// <value>The forward at <see cref="TimeToExpiry"/>.
        /// </value>
        public double Forward
        {
            get { return m_Forward; }
            set
            {
                m_Forward = value;
                m_LogOfMoneyness = Math.Log(value / m_Strike);
            }
        }

        /// <summary>Gets or sets the discount factor at <see cref="TimeToExpiry"/>.
        /// </summary>
        /// <value>The discount factor at <see cref="TimeToExpiry"/>.
        /// </value>
        public double DiscountFactor
        {
            get { return m_DiscountFactor; }
            set { m_DiscountFactor = value; }
        }
        #endregion

        #region protected properties

        /// <summary>Gets the logarithm of the moneyness, i.e. the logarithm of forward over strike.
        /// </summary>
        /// <value>The log-moneyness.</value>
        protected double LogOfMoneyness
        {
            get { return m_LogOfMoneyness; }
        }

        /// <summary>Gets the square-root of the time-to-expiry.
        /// </summary>
        /// <value>The square-root of the time-to-expiry..</value>
        protected double SqrtOfTimeToExpiration
        {
            get { return m_SqrtOfTimeToExpiration; }
        }
        #endregion

        #region protected static methods

        /// <summary>Gets the implied strike for a given value (price) and Black-Scholes volatility of a specific european call or put option.
        /// </summary>
        /// <param name="theta">A value indicating whether a call (=1) or put (=-1) is given.</param>
        /// <param name="forward">The forward.</param>
        /// <param name="timeToExpiry">The time span between valuation date and expiry date in its <see cref="System.Double"/> representation.</param>
        /// <param name="noneDiscountedValue">The value of the option at the time of expiry, thus the price but <b>not</b> discounted to time 0.</param>
        /// <param name="blackVolatility">The Black volatility.</param>
        /// <param name="sqrtOfTime">The square root of <paramref name="timeToExpiry"/>.</param>
        /// <param name="impliedStrike">The implied strike (output).</param>
        /// <returns>Returns a value indicating whether <paramref name="impliedStrike"/> contains valid data.</returns>
        /// <remarks>The implementation is based on the Newton approach.</remarks>
        protected static ImpliedCalculationResultState TryGetPlainVanillaImpliedStrike(int theta, double forward, double timeToExpiry, double noneDiscountedValue, double blackVolatility, double sqrtOfTime, out double impliedStrike)
        {
            /* we apply Newtons method to the function
             *     H(K) = ln( f(K) / c),
             * where f(K) is the (undiscounted) value of the option and c is the given undiscounted value 
             * of the option. It holds H'(K) = f'(K)/f(K) and Newtons method gives
             * 
             *      K_{n+1} = K_n - H(K_n)/H'(K_n) = K_n - ln(f(K)/c) * f(K) / f'(K).
             */

            impliedStrike = forward;
            double absOfValueAtExpiry = Math.Abs(noneDiscountedValue);
            double optionValueForGivenStrike = noneDiscountedValue;

            double dplus, CDFAtThetaTimesdMinus;

            for (int i = 1; i <= MaxNumberIterationImpliedStrike; i++)
            {
                if (impliedStrike < MachineConsts.SuperTinyEpsilon)
                {
                    optionValueForGivenStrike = forward + impliedStrike;
                    impliedStrike = forward - theta * noneDiscountedValue;
                }
                else
                {
                    dplus = (Math.Log(forward / impliedStrike) + 0.5 * blackVolatility * blackVolatility * timeToExpiry) / (blackVolatility * sqrtOfTime);
                    CDFAtThetaTimesdMinus = StandardNormalDistribution.GetCdfValue(theta * (dplus - blackVolatility * sqrtOfTime));

                    optionValueForGivenStrike = (theta * forward * StandardNormalDistribution.GetCdfValue(theta * dplus) - theta * impliedStrike * CDFAtThetaTimesdMinus);
                    impliedStrike += theta * optionValueForGivenStrike * Math.Log(optionValueForGivenStrike / noneDiscountedValue) / CDFAtThetaTimesdMinus;
                }
                if (Math.Abs(noneDiscountedValue - optionValueForGivenStrike) < absOfValueAtExpiry * MachineConsts.TinyEpsilon)
                {
                    return ImpliedCalculationResultState.ProperResult;
                }
            }
            return ImpliedCalculationResultState.NoProperResult;
        }
        #endregion
    }
}