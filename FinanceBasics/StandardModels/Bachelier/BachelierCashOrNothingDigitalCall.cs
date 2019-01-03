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
    /// <summary>Represents the Bachelier (=Normal Black) model for an european digital call option, i.e. pays 1 unit if the underlying (forward) is above the strike at the exercise date.
    /// </summary>
    /// <remarks>This implementation is based on 
    /// <list type="bullet">
    /// <item>
    /// <description>Kazuhiro Iwasawa, 'Analytic formula for the European Normal Black Scholes Formula', Dec. 2001,</description></item>
    /// <item>
    /// <description>M. Musiela, M. Rutkowski, 'Martingal Methods in Finance Modelling', Springer-Verlag, second edition, 123-128,</description></item>
    /// </list>
    /// We implement the case r=0 and the initial value will be interpreted as forward, i.e. we assume
    /// <para>
    ///    d St = F - S_0 + \sigma dW_t, thus S_t = F + \sigma \cdot W_t,
    /// </para>
    /// where F denote the forward, W_t is a Brownian motion and S_t is the model based value of the underlying at time t.
    /// </remarks>
    public class BachelierCashOrNothingDigitalCall : CommonEuropeanBachelierOption, IConstantVolatilityStandardEuropeanOption
    {
        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="BachelierCashOrNothingDigitalCall"/> class.
        /// </summary>
        public BachelierCashOrNothingDigitalCall()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="BachelierCashOrNothingDigitalCall"/> class.
        /// </summary>
        /// <param name="strike">The strike.</param>
        /// <param name="forward">The forward.</param>
        /// <param name="timeToExpiry">The time span between valuation date and expiry date.</param>
        /// <param name="discountFactor">The discount factor at <paramref name="timeToExpiry"/>.</param>
        public BachelierCashOrNothingDigitalCall(double strike, double forward, double timeToExpiry, double discountFactor)
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
        /// <returns>The vega (also called lambda) of the option, i.e. the partial derivative of the option value formula with respect to the volatility.</returns>
        public double GetVega(double volatility)
        {
            if (volatility * volatility * m_TimeToExpiration < MachineConsts.Epsilon)
            {
                return 0.0;
            }
            double d = (m_Strike - m_Forward) / (volatility * SqrtOfTimeToExpiration);
            return m_DiscountFactor * d * StandardNormalDistribution.GetPdfValue(d) / volatility;
        }

        /// <summary>Gets the volga of the option, i.e. the second partial derivative of the option value formula with respect to the volatility.
        /// </summary>
        /// <param name="volatility">The volatility.</param>
        /// <returns>The volga of the option, i.e. the second partial derivative of the option value formula with respect to the volatility.</returns>
        public double GetVolga(double volatility)
        {
            if (volatility * volatility * m_TimeToExpiration < MachineConsts.Epsilon)
            {
                return 0.0;
            }
            double d = (m_Strike - m_Forward) / (volatility * SqrtOfTimeToExpiration);
            return m_DiscountFactor * StandardNormalDistribution.GetPdfValue(d) * d * (d * d - 2.0) / (volatility * volatility);
        }

        /// <summary>Gets the theta of the option, i.e. the partial derivative of the option value formula with respect to the time to maturity.
        /// </summary>
        /// <param name="volatility">The volatility.</param>
        /// <returns>The theta of the option, i.e. the partial derivative of the option value formula with respect to the time to expiry.</returns>
        public double GetTheta(double volatility)
        {
            if (volatility * volatility * m_TimeToExpiration < MachineConsts.Epsilon)
            {
                return 0.0;
            }
            double d = (m_Strike - m_Forward) / (volatility * SqrtOfTimeToExpiration);
            return m_DiscountFactor * 0.5 * StandardNormalDistribution.GetPdfValue(d) * d / m_TimeToExpiration;
        }
        #endregion

        #region IConstantVolatilityStandardEuropeanOption Members

        /// <summary>Gets the price of the option <c>at time of expiry</c>, i.e. not discounted.
        /// </summary>
        /// <param name="volatility">The volatility.</param>
        /// <returns>The value of the option <c>at the time of expiry</c>, thus not discounted. To get the price just multiply the return value with the discount factor.</returns>
        public double GetNoneDiscountedValue(double volatility)
        {
            if ((m_Strike < MachineConsts.Epsilon) || (volatility * volatility * m_TimeToExpiration < MachineConsts.Epsilon))
            {
                return (m_Forward >= m_Strike ? 1.0 : 0.0);
            }
            double d = (m_Forward - m_Strike) / (volatility * SqrtOfTimeToExpiration);
            return StandardNormalDistribution.GetCdfValue(d);
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
            impliedVolatility = (m_Forward - m_Strike) / (SqrtOfTimeToExpiration * StandardNormalDistribution.GetInverseCdfValue(noneDiscountedValue));
            return (Double.IsInfinity(impliedVolatility) || Double.IsNaN(impliedVolatility)) ? ImpliedCalculationResultState.InputError : ImpliedCalculationResultState.ProperResult;
        }

        /// <summary>Gets the implied strike for a specific option price.
        /// </summary>
        /// <param name="optionValue">The value of the option.</param>
        /// <param name="volatility">The volatility.</param>
        /// <param name="impliedStrike">The implied strike (output).</param>
        /// <returns>A value indicating whether <paramref name="impliedStrike"/> contains valid data.</returns>
        public ImpliedCalculationResultState TryGetImpliedStrike(double optionValue, double volatility, out double impliedStrike)
        {
            impliedStrike = m_Forward - volatility * SqrtOfTimeToExpiration * StandardNormalDistribution.GetInverseCdfValue(optionValue / m_DiscountFactor);
            return (Double.IsInfinity(impliedStrike) || Double.IsNaN(impliedStrike)) ? ImpliedCalculationResultState.InputError : ImpliedCalculationResultState.ProperResult;
        }

        /// <summary>Gets the forward-delta of the option, i.e. the partial derivative of the option value formula with respect to the forward.
        /// </summary>
        /// <param name="volatility">The volatility.</param>
        /// <returns>The forward-delta of the option, i.e. the partial derivative of the option value formula with respect to the forward.</returns>
        public double GetForwardDelta(double volatility)
        {
            if ((m_Strike < MachineConsts.Epsilon) || (volatility * volatility * m_TimeToExpiration < MachineConsts.Epsilon))
            {
                return 0;
            }
            double d = (m_Forward - m_Strike) / (volatility * SqrtOfTimeToExpiration);
            return m_DiscountFactor * StandardNormalDistribution.GetPdfValue(d) / (volatility * SqrtOfTimeToExpiration);
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
            double d = (m_Strike - m_Forward) / (volatility * SqrtOfTimeToExpiration);
            return m_DiscountFactor * d * StandardNormalDistribution.GetPdfValue(d) / (volatility * volatility * m_TimeToExpiration);
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
            double d = (m_Strike - m_Forward) / (volatility * SqrtOfTimeToExpiration);
            return m_DiscountFactor * StandardNormalDistribution.GetPdfValue(d) * (d * d - 1) / (SqrtOfTimeToExpiration * volatility * volatility);
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
            double d = (m_Strike - m_Forward) / (volatility * SqrtOfTimeToExpiration);
            return -m_DiscountFactor * StandardNormalDistribution.GetPdfValue(d) / (volatility * SqrtOfTimeToExpiration);
        }
        #endregion

        /// <summary>Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return String.Format("Cash-or-nothing digital call (Bachelier); Strike: {0}; Forward: {1}; Time-To-Expiration: {2}; Discontfactor: {3}.", m_Strike, m_Forward, m_TimeToExpiration, m_DiscountFactor);
        }
        #endregion
    }
}