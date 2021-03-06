﻿/* MIT License
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
    /// <summary>Represents the Black(-Scholes) model for an european digital call option, i.e. pays 1 unit if the underlying (forward) is above the strike at the exercise date.
    /// </summary>
    public class BlackCashOrNothingDigitalCall : CommonEuropeanBlackOption, IConstantVolatilityStandardEuropeanOption
    {
        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="BlackCashOrNothingDigitalCall"/> class.
        /// </summary>
        public BlackCashOrNothingDigitalCall()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="BlackCashOrNothingDigitalCall"/> class.
        /// </summary>
        /// <param name="strike">The strike.</param>
        /// <param name="forward">The forward.</param>
        /// <param name="timeToExpiry">The time span between valuation date and expiry date in its <see cref="System.Double"/> representation.</param>
        /// <param name="discountFactor">The discount factor at <paramref name="timeToExpiry"/>.</param>
        public BlackCashOrNothingDigitalCall(double strike, double forward, double timeToExpiry, double discountFactor)
            : base(strike, forward, timeToExpiry, discountFactor)
        {
        }
        #endregion

        #region public properties

        #region IConstantVolatilityStandardOption Members

        /// <summary>Gets the type of the option, i.e. a value indicating whether a call or put option is given.
        /// </summary>
        /// <value>The type of the option.</value>
        public OptionType OptionType
        {
            get { return OptionType.Call; }
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
        /// <returns>A value indicating whether <paramref name="impliedVolatility" /> contains valid data.</returns>
        /// <remarks>This method is the inverse function of <see cref="IConstantVolatilityStandardOption.GetValue(double)" />.</remarks>
        public ImpliedCalculationResultState TryGetImpliedVolatility(double optionValue, out double impliedVolatility)
        {
            return TryGetImpliedVolatilityOfNonDiscountedValue(optionValue / m_DiscountFactor, out impliedVolatility);
        }

        /// <summary>Gets the vega of the option, i.e. the partial derivative of the option value formula with respect to the volatility.
        /// </summary>
        /// <param name="volatility">The volatility.</param>
        /// <returns>The vega of the option, i.e. the partial derivative of the option value formula with respect to the volatility.</returns>
        public double GetVega(double volatility)
        {
            if ((m_Strike < MachineConsts.Epsilon) || (volatility * volatility * m_TimeToExpiration < MachineConsts.Epsilon))
            {
                return 0.0;
            }
            double dminus = (LogOfMoneyness - 0.5 * volatility * volatility * m_TimeToExpiration) / (volatility * SqrtOfTimeToExpiration);
            return -m_DiscountFactor * StandardNormalDistribution.GetPdfValue(dminus) * (SqrtOfTimeToExpiration + dminus / volatility);
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
            double volaSquare = volatility * volatility;
            double dminus = (LogOfMoneyness - 0.5 * volaSquare * m_TimeToExpiration) / (volatility * SqrtOfTimeToExpiration);
            double densityAtDminus = StandardNormalDistribution.GetPdfValue(dminus);

            // compute the derivative of \phi(d_), where \phi is the density of the standard normal distribution:
            double densityDerivativeAtDminus = dminus * densityAtDminus * (0.5 * SqrtOfTimeToExpiration + LogOfMoneyness / (SqrtOfTimeToExpiration * volaSquare));
            return -m_DiscountFactor * (0.5 * SqrtOfTimeToExpiration * densityDerivativeAtDminus + (densityDerivativeAtDminus * volatility - densityAtDminus - densityAtDminus) * LogOfMoneyness / (SqrtOfTimeToExpiration * volaSquare * volatility));
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
            double dminus = (LogOfMoneyness - 0.5 * volatility * volatility * m_TimeToExpiration) / (volatility * SqrtOfTimeToExpiration);
            return -m_DiscountFactor * StandardNormalDistribution.GetPdfValue(dminus) * (LogOfMoneyness / (2.0 * volatility * m_TimeToExpiration * SqrtOfTimeToExpiration) + 0.25 * volatility / SqrtOfTimeToExpiration);
        }
        #endregion

        #region IConstantVolatilityStandardEuropeanOption Members

        /// <summary>Gets the price of the option <c>at time of expiry</c>, i.e. not discounted.
        /// </summary>
        /// <param name="volatility">The volatility.</param>
        /// <returns>The value of the option <c>at the time of expiry</c>, thus not discounted. To get the price just multiply the return value with the discount factor.</returns>
        public double GetNoneDiscountedValue(double volatility)
        {
            /* mind numerical traps: */
            if ((m_Strike < MachineConsts.Epsilon) || (volatility * volatility * m_TimeToExpiration < MachineConsts.Epsilon))
            {
                return (m_Forward >= m_Strike ? 1.0 : 0.0);
            }
            double dminus = (LogOfMoneyness - 0.5 * volatility * volatility * m_TimeToExpiration) / (volatility * SqrtOfTimeToExpiration);
            return StandardNormalDistribution.GetCdfValue(dminus);
        }

        /// <summary>Gets the intrinsic value of the option.
        /// </summary>
        /// <returns>The intrisic value of the option.</returns>
        public double GetIntrinsicValue()
        {
            return m_DiscountFactor * (m_Forward >= m_Strike ? 1.0 : 0.0);
        }

        /// <summary>Gets the implied volatility for a specific non-discounted option price.
        /// </summary>
        /// <param name="noneDiscountedValue">The value of the option at the time of expiry, thus the price but <b>not</b> discounted to time 0.</param>
        /// <param name="impliedVolatility">The implied volatility (output).</param>
        /// <returns>A value indicating whether <paramref name="impliedVolatility" /> contains valid data.</returns>
        /// <remarks>This method is the inverse function of <see cref="IConstantVolatilityStandardEuropeanOption.GetNoneDiscountedValue(double)" />.</remarks>
        public ImpliedCalculationResultState TryGetImpliedVolatilityOfNonDiscountedValue(double noneDiscountedValue, out double impliedVolatility)
        {
            double x = Math.Log(m_Forward / m_Strike);
            double NInverse = StandardNormalDistribution.GetInverseCdfValue(noneDiscountedValue);
            if (NInverse * NInverse + 2 * x >= 0)
            {
                double root = Math.Sqrt(NInverse * NInverse + 2 * x);
                impliedVolatility = -NInverse + root;

                if (-NInverse - root >= 0)
                {
                    impliedVolatility = -NInverse - root;
                }
                impliedVolatility /= Math.Sqrt(m_TimeToExpiration);
                return ImpliedCalculationResultState.ProperResult;
            }
            impliedVolatility = 0.0;
            return ImpliedCalculationResultState.InputError;
        }

        /// <summary>Gets the implied strike for a specific option price.
        /// </summary>
        /// <param name="optionValue">The value of the option.</param>
        /// <param name="volatility">The volatility.</param>
        /// <param name="impliedStrike">The implied strike (output).</param>
        /// <returns>A value indicating whether <paramref name="impliedStrike"/> contains valid data.</returns>
        public ImpliedCalculationResultState TryGetImpliedStrike(double optionValue, double volatility, out double impliedStrike)
        {
            if (m_TimeToExpiration < MachineConsts.Epsilon)
            {
                impliedStrike = m_Forward;
                return ImpliedCalculationResultState.ProperResult;
            }
            impliedStrike = m_Forward * Math.Exp(-StandardNormalDistribution.GetInverseCdfValue(optionValue / m_DiscountFactor) * volatility * Math.Sqrt(m_TimeToExpiration) - 0.5 * volatility * volatility * m_TimeToExpiration);
            return (Double.IsNaN(impliedStrike) == false) ? ImpliedCalculationResultState.ProperResult : ImpliedCalculationResultState.NoProperResult;
        }

        /// <summary>Gets the forward-delta of the option, i.e. the partial derivative of the option value formula with respect to the forward.
        /// </summary>
        /// <param name="volatility">The volatility.</param>
        /// <returns>The forward-delta of the option, i.e. the partial derivative of the option value formula with respect to the forward.</returns>
        public double GetForwardDelta(double volatility)
        {
            if ((m_Strike < MachineConsts.Epsilon) || (volatility * volatility * m_TimeToExpiration < MachineConsts.Epsilon))
            {
                return 0.0;
            }
            double dminus = (LogOfMoneyness - 0.5 * volatility * volatility * m_TimeToExpiration) / (volatility * SqrtOfTimeToExpiration);
            return m_DiscountFactor * StandardNormalDistribution.GetPdfValue(dminus) / (volatility * SqrtOfTimeToExpiration * m_Forward);
        }

        /// <summary>Gets the forward-gamma of the option, i.e. the second partial derivative of the option value formula with respect to the forward.
        /// </summary>
        /// <param name="volatility">The volatility.</param>
        /// <returns>The forward-gamma of the option, i.e. the second partial derivative of the option value formula with respect to the forward.
        /// </returns>
        /// <remarks>The initial value of the underlying is equal to the forward times the discount factor.</remarks>
        public double GetForwardGamma(double volatility)
        {
            if ((m_Strike < MachineConsts.Epsilon) || (volatility * volatility * m_TimeToExpiration < MachineConsts.Epsilon))
            {
                return 0.0;
            }
            double dminus = (LogOfMoneyness - 0.5 * volatility * volatility * m_TimeToExpiration) / (volatility * SqrtOfTimeToExpiration);
            return m_DiscountFactor * StandardNormalDistribution.GetPdfValue(dminus) / (volatility * m_Forward * m_Forward) * (-dminus / (volatility * m_TimeToExpiration) - 1.0 / (SqrtOfTimeToExpiration));
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
                return 0;
            }
            double dminus = (LogOfMoneyness - 0.5 * volatility * volatility * m_TimeToExpiration) / (volatility * SqrtOfTimeToExpiration);
            return -m_DiscountFactor * StandardNormalDistribution.GetPdfValue(dminus) * (1.0 / volatility - dminus * (SqrtOfTimeToExpiration + dminus / volatility)) / (m_Forward * volatility * SqrtOfTimeToExpiration);
        }

        /// <summary>Gets the strike-delta of the option, i.e. the partial derivative of the option value formula with
        /// respect to the strike.
        /// </summary>
        /// <param name="volatility">The volatility.</param>
        /// <returns>The strike-delta of the option, i.e. the partial derivative of the option value formula with
        /// respect to the strike.
        /// </returns>
        public double GetStrikeDelta(double volatility)
        {
            if ((m_Strike < MachineConsts.Epsilon) || (volatility * volatility * m_TimeToExpiration < MachineConsts.Epsilon))
            {
                return 0.0;
            }
            double dminus = (LogOfMoneyness - 0.5 * volatility * volatility * m_TimeToExpiration) / (volatility * SqrtOfTimeToExpiration);
            return -m_DiscountFactor * StandardNormalDistribution.GetPdfValue(dminus) / (volatility * SqrtOfTimeToExpiration * m_Strike);
        }
        #endregion

        /// <summary>Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return String.Format("Cash-or-nothing digital call (Black); Strike: {0}; Forward: {1}; Time-To-Expiration: {2}; Discontfactor: {3}.", m_Strike, m_Forward, m_TimeToExpiration, m_DiscountFactor);
        }
        #endregion
    }
}