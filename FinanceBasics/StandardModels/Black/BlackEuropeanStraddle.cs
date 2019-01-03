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
    /// <summary>Represents the implementation of the Black(-Scholes) model for an european straddle option, i.e. the payoff is |spot -strike| and the 
    /// price is given by the price of an european call + put option.
    /// </summary>
    public class BlackEuropeanStraddle : CommonEuropeanBlackOption, IConstantVolatilityStandardEuropeanOption
    {
        #region nested interfaces etc.

        /// <summary>Serves as interface for the calculation of the implied volatility.
        /// </summary>
        public interface IImpliedVolatilityApproach
        {
            /// <summary>Gets the implied Black volatility of a specific european Straddle option.
            /// </summary>
            /// <param name="strike">The strike.</param>
            /// <param name="forward">The forward.</param>
            /// <param name="timeToExpiry">The time span between valuation date and expiry date in its <see cref="System.Double"/> representation.</param>
            /// <param name="noneDiscountedValue">The value of the option at the time of expiry, thus the price but <b>not</b> discounted to time 0.</param>
            /// <param name="value">The implied Black volatility (output).</param>
            /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
            ImpliedCalculationResultState TryGetValue(double strike, double forward, double timeToExpiry, double noneDiscountedValue, out double value);
        }
        #endregion

        #region private (static) consts

        /// <summary>The maximal number of iterations for the computation of the implied strike.
        /// </summary>
        private static readonly int MaxNumberIterationImpliedStrike = 1000;
        #endregion

        #region private (static) members

        /// <summary>The approach how to calculate the implied volatility.
        /// </summary>
        private IImpliedVolatilityApproach m_ImpliedVolatilityApproach;

        /// <summary>The Standard Implied volatility approach.
        /// </summary>
        private static readonly IImpliedVolatilityApproach sm_StandardImpliedVolatilityApproach;
        #endregion

        #region static constructor

        /// <summary>Initializes the <see cref="BlackEuropeanStraddle" /> class.
        /// </summary>
        static BlackEuropeanStraddle()
        {
            sm_StandardImpliedVolatilityApproach = ImpliedBlackVolatilityApproach.SOR_TS.Create();
        }
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="BlackEuropeanStraddle"/> class.
        /// </summary>
        /// <param name="impliedVolatilityApproach">The algorithm used to calculate implied volatility.</param>
        public BlackEuropeanStraddle(IImpliedVolatilityApproach impliedVolatilityApproach = null)
        {
            if (impliedVolatilityApproach == null)
            {
                m_ImpliedVolatilityApproach = sm_StandardImpliedVolatilityApproach;
            }
            else
            {
                m_ImpliedVolatilityApproach = impliedVolatilityApproach;
            }
        }

        /// <summary>Initializes a new instance of the <see cref="BlackEuropeanStraddle"/> class.
        /// </summary>
        /// <param name="strike">The strike.</param>
        /// <param name="forward">The forward.</param>
        /// <param name="timeToExpiry">The time span between valuation date and expiry date.</param>
        /// <param name="discountFactor">The discount factor at <paramref name="timeToExpiry"/>.</param>
        /// <param name="impliedVolatilityApproach">The algorithm used to calculate implied volatility.</param>
        public BlackEuropeanStraddle(double strike, double forward, double timeToExpiry, double discountFactor, IImpliedVolatilityApproach impliedVolatilityApproach = null)
            : base(strike, forward, timeToExpiry, discountFactor)
        {
            if (impliedVolatilityApproach == null)
            {
                m_ImpliedVolatilityApproach = sm_StandardImpliedVolatilityApproach;
            }
            else
            {
                m_ImpliedVolatilityApproach = impliedVolatilityApproach;
            }
        }
        #endregion

        #region public properties

        #region IConstantVolatilityStandardOption Members

        /// <summary>Gets the type of the option, i.e. a value indicating whether a call or put option is given.
        /// </summary>
        /// <value>The type of the option.</value>
        public OptionType OptionType
        {
            get { return OptionType.Unspecific; }
        }
        #endregion

        #endregion

        #region public methods

        #region IConstantVolatilityStandardOption Members

        /// <summary>Gets the price of the option.
        /// </summary>
        /// <param name="volatility">The volatility.</param>
        /// <returns>The value of the option.</returns>
        public double GetValue(double volatility)
        {
            return m_DiscountFactor * GetNoneDiscountedValue(volatility);
        }

        /// <summary>Gets the implied volatility for a specific option price.
        /// </summary>
        /// <param name="optionValue">The value of the option.</param>
        /// <param name="impliedVolatility">The implied volatility (output).</param>
        /// <returns>A value indicating whether <paramref name="impliedVolatility"/> contains valid data.</returns>
        /// <remarks>This method is the inverse function of <see cref="IConstantVolatilityStandardOption.GetValue(double)"/>.</remarks>
        public ImpliedCalculationResultState TryGetImpliedVolatility(double optionValue, out double impliedVolatility)
        {
            return m_ImpliedVolatilityApproach.TryGetValue(m_Strike, m_Forward, m_TimeToExpiration, optionValue / m_DiscountFactor, out impliedVolatility);
        }

        /// <summary>Gets the vega of the option, i.e. the partial derivative of the option value formula with respect to the volatility.
        /// </summary>
        /// <param name="volatility">The volatility.</param>
        /// <returns>The vega of the option, i.e. the partial derivative of the option value formula with respect to the volatility.</returns>
        public double GetVega(double volatility)
        {
            if ((m_Strike < MachineConsts.Epsilon) || (volatility * volatility * m_TimeToExpiration < MachineConsts.Epsilon))
            {
                return m_DiscountFactor * (m_Strike - m_Forward);
            }
            double dplus = (LogOfMoneyness + 0.5 * volatility * volatility * m_TimeToExpiration) / (volatility * SqrtOfTimeToExpiration);
            return 2.0 * m_DiscountFactor * m_Forward * SqrtOfTimeToExpiration * StandardNormalDistribution.GetPdfValue(dplus);
        }

        /// <summary>Gets the volga of the option, i.e. the second partial derivative of the option value formula with respect to the volatility.
        /// </summary>
        /// <param name="volatility">The volatility.</param>
        /// <returns>The volga of the option, i.e. the second partial derivative of the option value formula with respect to the volatility.</returns>
        public double GetVolga(double volatility)
        {
            if ((m_Strike < MachineConsts.Epsilon) || (volatility * volatility * m_TimeToExpiration < MachineConsts.Epsilon))
            {
                return 0.0;
            }
            double dplus = (LogOfMoneyness + 0.5 * volatility * volatility * m_TimeToExpiration) / (volatility * SqrtOfTimeToExpiration);
            double dminus = dplus - volatility * SqrtOfTimeToExpiration;

            return 2.0 * m_DiscountFactor * m_Forward * SqrtOfTimeToExpiration * StandardNormalDistribution.GetPdfValue(dplus) * dplus * dminus / volatility;
        }

        /// <summary>Gets the theta of the option, i.e. the partial derivative of the option value formula with respect to the time to maturity.
        /// </summary>
        /// <param name="volatility">The volatility.</param>
        /// <returns>The theta of the option, i.e. the partial derivative of the option value formula with respect to the time to expiry.</returns>
        public double GetTheta(double volatility)
        {
            if ((m_Strike < MachineConsts.Epsilon) || (volatility * volatility * m_TimeToExpiration < MachineConsts.Epsilon))
            {
                return 0.0;
            }
            double dplus = (LogOfMoneyness + 0.5 * volatility * volatility * m_TimeToExpiration) / (volatility * SqrtOfTimeToExpiration);
            double dminus = dplus - volatility * SqrtOfTimeToExpiration;

            double tempTerm1 = 0.25 * volatility / SqrtOfTimeToExpiration;
            double tempTerm2 = LogOfMoneyness / (2.0 * volatility * m_TimeToExpiration * SqrtOfTimeToExpiration);

            return 2.0 * m_DiscountFactor * (m_Forward * StandardNormalDistribution.GetPdfValue(dplus) * (tempTerm1 - tempTerm2) + m_Strike * StandardNormalDistribution.GetPdfValue(dminus) * (tempTerm1 + tempTerm2));
        }
        #endregion

        #region IConstantVolatilityStandardEuropeanOption Members

        /// <summary>Gets the price of the option <c>at time of expiry</c>, i.e. not discounted.
        /// </summary>
        /// <param name="volatility">The volatility.</param>
        /// <returns>The value of the option <c>at the time of expiry</c>, thus not discounted. To get
        /// the price just multiply the return value with the discount factor.
        /// </returns>
        public double GetNoneDiscountedValue(double volatility)
        {
            if ((m_Strike < MachineConsts.Epsilon) || (volatility * volatility * m_TimeToExpiration < MachineConsts.Epsilon))
            {
                return Math.Abs(m_Strike - m_Forward);
            }
            double dplus = (LogOfMoneyness + 0.5 * volatility * volatility * m_TimeToExpiration) / (volatility * SqrtOfTimeToExpiration);
            double dminus = dplus - volatility * SqrtOfTimeToExpiration;

            /* the value is given by: (K: strike, f: forward), use N(-t) = 1-N(t)
             *   discfactor * [ f * N(d+) - K * N(d-) + K * N(-d-) - f * N(-d+) ], i.e. call + put value
             *   = discfactor * [ f * N(d+) - K * N(d-) + K* (1-N(d-)) - f * (1-N(d+))
             *   = discfactor * [2 * f * N(d+) - 2 * K * N(d-) + K -f] */
            return (m_Strike - m_Forward + 2.0 * m_Forward * StandardNormalDistribution.GetCdfValue(dplus) - 2.0 * m_Strike * StandardNormalDistribution.GetCdfValue(dminus));
        }

        /// <summary>Gets the intrinsic value of the option.
        /// </summary>
        /// <returns>The intrisic value of the option.</returns>
        public double GetIntrinsicValue()
        {
            return m_DiscountFactor * Math.Abs(m_Strike - m_Forward);
        }

        /// <summary>Gets the implied volatility for a specific non-discounted option price.
        /// </summary>
        /// <param name="noneDiscountedValue">The value of the option at the time of expiry, thus the price but <b>not</b> discounted to time 0.</param>
        /// <param name="impliedVolatility">The implied volatility (output).</param>
        /// <returns>A value indicating whether <paramref name="impliedVolatility" /> contains valid data.</returns>
        /// <remarks>This method is the inverse function of <see cref="IConstantVolatilityStandardEuropeanOption.GetNoneDiscountedValue(double)" />.</remarks>
        public ImpliedCalculationResultState TryGetImpliedVolatilityOfNonDiscountedValue(double noneDiscountedValue, out double impliedVolatility)
        {
            return m_ImpliedVolatilityApproach.TryGetValue(m_Strike, m_Forward, m_TimeToExpiration, noneDiscountedValue, out impliedVolatility);
        }

        /// <summary>Gets the implied strike for a specific option price.
        /// </summary>
        /// <param name="optionValue">The value of the option.</param>
        /// <param name="volatility">The volatility.</param>
        /// <param name="impliedStrike">The implied strike (output).</param>
        /// <returns>A value indicating whether <paramref name="impliedStrike"/> contains valid data.</returns>
        /// <remarks>The implied strike is in general not unique.</remarks>
        public ImpliedCalculationResultState TryGetImpliedStrike(double optionValue, double volatility, out double impliedStrike)
        {
            return TryGetImpliedStrike(m_Forward, m_TimeToExpiration, optionValue / m_DiscountFactor, volatility, m_Forward, out impliedStrike);
        }

        /// <summary>Gets the forward-delta of the option, i.e. the partial derivative of the option value formula with respect to the forward.
        /// </summary>
        /// <param name="volatility">The volatility.</param>
        /// <returns>The forward-delta of the option, i.e. the partial derivative of the option value formula with respect to the forward.</returns>
        public double GetForwardDelta(double volatility)
        {
            if ((m_Strike < MachineConsts.Epsilon) || (volatility * volatility * m_TimeToExpiration < MachineConsts.Epsilon))
            {
                return ((m_Forward >= m_Strike) ? 1 : -1);
            }
            double dplus = (LogOfMoneyness + 0.5 * volatility * volatility * m_TimeToExpiration) / (volatility * SqrtOfTimeToExpiration);
            return m_DiscountFactor * (2.0 * StandardNormalDistribution.GetCdfValue(dplus) - 1.0);
        }

        /// <summary>Gets the forward-gamma of the option, i.e. the second partial derivative of the option value formula with respect to the forward.
        /// </summary>
        /// <param name="volatility">The volatility.</param>
        /// <returns>The forward-gamma of the option, i.e. the second partial derivative of the option value formula with respect to the forward.</returns>
        /// <remarks>The initial value of the underlying is equal to the forward times the discount factor.</remarks>
        public double GetForwardGamma(double volatility)
        {
            if ((m_Strike < MachineConsts.Epsilon) || (volatility * volatility * m_TimeToExpiration < MachineConsts.Epsilon))
            {
                return 0.0;
            }
            double dplus = (LogOfMoneyness + 0.5 * volatility * volatility * m_TimeToExpiration) / (volatility * SqrtOfTimeToExpiration);

            // is just two times the gamma of an call/put option
            return 2.0 * m_DiscountFactor * StandardNormalDistribution.GetPdfValue(dplus) / (volatility * SqrtOfTimeToExpiration * m_Forward);
        }

        /// <summary>Gets the (forward-)vanna of the option, i.e. the partial derivative of the option value formual with respect
        /// to the forward and with respect to the volatility, i.e. '\partial\sigma \partial F'.
        /// </summary>
        /// <param name="volatility">The volatility.</param>
        /// <returns>The (forward-)vanna of the option, i.e. the partial derivative of the option value formual with respect
        /// to the forward and with respect to the volatility, i.e. '\partial\sigma \partial F'.
        /// </returns>
        public double GetForwardVanna(double volatility)
        {
            if ((m_Strike < MachineConsts.Epsilon) || (volatility * volatility * m_TimeToExpiration < MachineConsts.Epsilon))
            {
                return 0.0;
            }
            double dplus = (LogOfMoneyness + 0.5 * volatility * volatility * m_TimeToExpiration) / (volatility * SqrtOfTimeToExpiration);
            return 2.0 * m_DiscountFactor * StandardNormalDistribution.GetPdfValue(dplus) * (SqrtOfTimeToExpiration - dplus / volatility);
        }

        /// <summary>Gets the strike-delta of the option, i.e. the partial derivative of the option value formula with
        /// respect to the strike.
        /// </summary>
        /// <param name="volatility">The volatility.</param>
        /// <returns>The strike-delta of the option, i.e. the partial derivative of the option value formula with respect to the strike.</returns>
        public double GetStrikeDelta(double volatility)
        {
            if ((m_Strike < MachineConsts.Epsilon) || (volatility * volatility * m_TimeToExpiration < MachineConsts.Epsilon))
            {
                return 0.0;
            }
            double dplus = (LogOfMoneyness + 0.5 * volatility * volatility * m_TimeToExpiration) / (volatility * SqrtOfTimeToExpiration);
            double dminus = dplus - volatility * SqrtOfTimeToExpiration;

            return m_DiscountFactor * (-2.0 * m_Forward / (volatility * SqrtOfTimeToExpiration * m_Strike) * StandardNormalDistribution.GetPdfValue(dplus) - (2 * StandardNormalDistribution.GetCdfValue(dminus) - 1) + 2.0 * StandardNormalDistribution.GetPdfValue(dminus) / (volatility * SqrtOfTimeToExpiration));
        }
        #endregion

        /// <summary>Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return String.Format("European Straddle (Black); Strike: {0}; Forward: {1}; Time-To-Expiration: {2}; Discontfactor: {3}.", m_Strike, m_Forward, m_TimeToExpiration, m_DiscountFactor);
        }
        #endregion

        #region protected static methods

        /// <summary>Gets the implied strike for a specific european straddle.
        /// </summary>
        /// <param name="forward">The forward.</param>
        /// <param name="timeToExpiry">The time span between valuation date and expiry date in its <see cref="System.Double"/> representation.</param>
        /// <param name="undiscountedValue">The value of the option at the time of expiry, thus the price but <b>not</b> discounted to time 0.</param>
        /// <param name="blackVolatility">The Black volatility.</param>
        /// <param name="initialStrike">The initial strike used as initial guess for the Newton method.</param>
        /// <param name="impliedStrike">The implied strike (output).</param>
        /// <returns>
        ///    Returns a value indicating whether <paramref name="impliedStrike"/> contains valid data.
        /// </returns>
        /// <remarks>This method checks some academic conditions and is based on the Newton approach.
        ///    <para>The implied strike is not unique.</para>
        /// </remarks>
        protected static ImpliedCalculationResultState TryGetImpliedStrike(double forward, double timeToExpiry, double undiscountedValue, double blackVolatility, double initialStrike, out double impliedStrike)
        {
            /* the value of a straddle is given by K-F + 2 * CallValue(K) thus we do it in a simular way as in the case of a call option */
            impliedStrike = initialStrike;
            double optionValueForGivenStrike = undiscountedValue;
            double absUndiscountedValue = Math.Abs(undiscountedValue);
            double sqrtOfTime = Math.Sqrt(timeToExpiry);

            double dplus, CDFAtdMinus;

            for (int i = 1; i <= MaxNumberIterationImpliedStrike; i++)
            {
                if (impliedStrike < MachineConsts.SuperTinyEpsilon)
                {
                    optionValueForGivenStrike = Math.Abs(forward - impliedStrike);
                    impliedStrike = forward - undiscountedValue;
                }
                else
                {
                    dplus = (Math.Log(forward / impliedStrike) + 0.5 * blackVolatility * blackVolatility * timeToExpiry) / (blackVolatility * sqrtOfTime);
                    CDFAtdMinus = StandardNormalDistribution.GetCdfValue(dplus - blackVolatility * sqrtOfTime);

                    optionValueForGivenStrike = impliedStrike - forward + 2 * (forward * StandardNormalDistribution.GetCdfValue(dplus) - impliedStrike * CDFAtdMinus);
                    impliedStrike += optionValueForGivenStrike * Math.Log(optionValueForGivenStrike / undiscountedValue) / (1 + 2.0 * CDFAtdMinus);
                }
                if (Math.Abs(undiscountedValue - optionValueForGivenStrike) < absUndiscountedValue * MachineConsts.TinyEpsilon)
                {
                    return ImpliedCalculationResultState.ProperResult;
                }
            }
            return ImpliedCalculationResultState.NoProperResult;
        }
        #endregion
    }
}