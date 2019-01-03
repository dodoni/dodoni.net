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
    /// <summary>Represents the implementation of the Bachelier model (=Normal Black model) for an european put option, i.e. (K - S_T)^+,
    /// assuming a normal-distribution of the underlying.
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
    /// where F denote the forward, W_t is a Brownian motion and S_t is the model based 
    /// value of the underlying at time t.
    /// </remarks>
    public class BachelierEuropeanPut : CommonEuropeanBachelierOption, IConstantVolatilityStandardEuropeanOption
    {
        #region nested interfaces etc.

        /// <summary>Serves as interface for the calculation of the implied volatility.
        /// </summary>
        public interface IImpliedVolatilityApproach
        {
            /// <summary>Gets the implied Black volatility of a specific european put option.
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

        #region private (static) members

        /// <summary>The approach how to calculate the implied volatility.
        /// </summary>
        private IImpliedVolatilityApproach m_ImpliedVolatilityApproach;

        /// <summary>The Standard Implied volatility approach.
        /// </summary>
        private static readonly IImpliedVolatilityApproach sm_StandardImpliedVolatilityApproach;
        #endregion

        #region static constructor

        /// <summary>Initializes the <see cref="BachelierEuropeanPut" /> class.
        /// </summary>
        static BachelierEuropeanPut()
        {
            sm_StandardImpliedVolatilityApproach = ImpliedBachelierVolatilityApproach.SOR.Create();
        }
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="BachelierEuropeanPut"/> class.
        /// </summary>
        /// <param name="impliedVolatilityApproach">The algorithm used to calculate implied volatility.</param>
        public BachelierEuropeanPut(IImpliedVolatilityApproach impliedVolatilityApproach = null)
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

        /// <summary>Initializes a new instance of the <see cref="BachelierEuropeanPut"/> class.
        /// </summary>
        /// <param name="strike">The strike.</param>
        /// <param name="forward">The forward.</param>
        /// <param name="timeToExpiry">The time span between valuation date and expiry date.</param>
        /// <param name="discountFactor">The discount factor at <paramref name="timeToExpiry"/>.</param>
        /// <param name="impliedVolatilityApproach">The algorithm used to calculate implied volatility.</param>
        public BachelierEuropeanPut(double strike, double forward, double timeToExpiry, double discountFactor, IImpliedVolatilityApproach impliedVolatilityApproach = null)
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
            get { return OptionType.Put; }
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
            return m_ImpliedVolatilityApproach.TryGetValue(m_Strike, m_Forward, m_TimeToExpiration, optionValue / m_DiscountFactor, out impliedVolatility);
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
            return m_DiscountFactor * SqrtOfTimeToExpiration * StandardNormalDistribution.GetPdfValue(d);
        }

        /// <summary>Gets the volga of the option, i.e. the second partial derivative of the option value formula with respect to the volatility.
        /// </summary>
        /// <param name="volatility">The volatility.</param>
        /// <returns>The volga of the option, i.e. the second partial derivative of the option value formula with respect to the volatility.
        /// </returns>
        public double GetVolga(double volatility)
        {
            if (volatility * volatility * m_TimeToExpiration < MachineConsts.Epsilon)
            {
                return 0.0;
            }
            double d = (m_Strike - m_Forward) / (volatility * SqrtOfTimeToExpiration);
            return m_DiscountFactor * d * d * SqrtOfTimeToExpiration / volatility * StandardNormalDistribution.GetPdfValue(d);
        }

        /// <summary>Gets the theta of the option, i.e. the partial derivative of the option value formula with respect to the time to maturity.
        /// </summary>
        /// <param name="volatility">The volatility.</param>
        /// <returns>The theta of the option, i.e. the partial derivative of the option value formula with respect to the time to expiry.
        /// </returns>
        public double GetTheta(double volatility)
        {
            if (volatility * volatility * m_TimeToExpiration < MachineConsts.Epsilon)
            {
                return 0.0;
            }
            double d = (m_Forward - m_Strike) / (volatility * SqrtOfTimeToExpiration);
            return m_DiscountFactor * 0.5 * StandardNormalDistribution.GetPdfValue(d) * (-d * (d * volatility / SqrtOfTimeToExpiration + (m_Strike - m_Forward) / m_TimeToExpiration) + volatility / SqrtOfTimeToExpiration);
        }
        #endregion

        #region IConstantVolatilityStandardEuropeanOption Members

        /// <summary>Gets the price of the option <c>at time of expiry</c>, i.e. not discounted.
        /// </summary>
        /// <param name="volatility">The volatility.</param>
        /// <returns>The value of the option <c>at the time of expiry</c>, thus not discounted. To get the price just multiply the return value with the discount factor.</returns>
        public double GetNoneDiscountedValue(double volatility)
        {
            if (volatility * volatility * m_TimeToExpiration < MachineConsts.Epsilon)
            {
                return Math.Max(0, m_Strike - m_Forward);
            }
            double d = (m_Strike - m_Forward) / (volatility * SqrtOfTimeToExpiration);
            return (m_Strike - m_Forward) * StandardNormalDistribution.GetCdfValue(d) + volatility * SqrtOfTimeToExpiration * StandardNormalDistribution.GetPdfValue(d);
        }

        /// <summary>Gets the intrinsic value of the option.
        /// </summary>
        /// <returns>The intrisic value of the option.</returns>
        public double GetIntrinsicValue()
        {
            return m_DiscountFactor * Math.Max(0, m_Strike - m_Forward);
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
        /// <returns>A value indicating whether <paramref name="impliedStrike"/> contains valid data.
        /// </returns>
        /// <remarks>This method is based on Newtons method where the initial guess is equal to the approximation given in
        ///   <para>How close are the option pricing formulas of Bachelier and Black-Merton-Scholes, W. Schachermayer, J. Teichmann.</para>
        /// </remarks>
        public ImpliedCalculationResultState TryGetImpliedStrike(double optionValue, double volatility, out double impliedStrike)
        {
            double undiscountedValue = optionValue / m_DiscountFactor;

            if (m_TimeToExpiration < MachineConsts.Epsilon)
            {
                impliedStrike = m_Forward - undiscountedValue;
                return ImpliedCalculationResultState.ProperResult;
            }

            /* first-order approximation (eqn. 3.5)
             *    P(m) \approx  a + m/2, 
             * where strike = forward + m and the left hand side is already given.
             */
            double a = volatility * SqrtOfTimeToExpiration / MathConsts.SqrtTwoPi;
            impliedStrike = m_Forward + 2.0 * (undiscountedValue - a);

            return TryGetPlainVanillaImpliedStrike(-1, m_Forward, undiscountedValue, volatility, SqrtOfTimeToExpiration, ref impliedStrike);
        }

        /// <summary>Gets the forward-delta of the option, i.e. the partial derivative of the option value formula with respect to the forward.
        /// </summary>
        /// <param name="volatility">The volatility.</param>
        /// <returns>The forward-delta of the option, i.e. the partial derivative of the option value formula with respect to the forward.
        /// </returns>
        public double GetForwardDelta(double volatility)
        {
            if ((m_Strike < MachineConsts.Epsilon) || (volatility * volatility * m_TimeToExpiration < MachineConsts.Epsilon))
            {
                return (m_Strike > m_Forward) ? 1 : 0;
            }
            double d = (m_Strike - m_Forward) / (volatility * SqrtOfTimeToExpiration);
            return -m_DiscountFactor * StandardNormalDistribution.GetCdfValue(d);
        }

        /// <summary>Gets the forward-gamma of the option, i.e. the second partial derivative of the option value formula with respect to the forward.
        /// </summary>
        /// <param name="volatility">The volatility.</param>
        /// <returns>The forward-gamma of the option, i.e. the second partial derivative of the option value
        /// formula with respect to the forward.
        /// </returns>
        /// <remarks>The initial value of the underlying is equal to the forward times the discount factor.</remarks>
        public double GetForwardGamma(double volatility)
        {
            if ((m_Strike < MachineConsts.Epsilon) || (volatility * volatility * m_TimeToExpiration < MachineConsts.Epsilon))
            {
                return 0.0;
            }
            double d = (m_Strike - m_Forward) / (volatility * SqrtOfTimeToExpiration);
            return m_DiscountFactor * StandardNormalDistribution.GetPdfValue(d) / (volatility * SqrtOfTimeToExpiration);
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
            return m_DiscountFactor * d * StandardNormalDistribution.GetPdfValue(d) / volatility;
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
            double d = (m_Strike - m_Forward) / (volatility * SqrtOfTimeToExpiration);
            return m_DiscountFactor * StandardNormalDistribution.GetCdfValue(d);
        }
        #endregion

        /// <summary>Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return String.Format("European Put (Bachelier); Strike: {0}; Forward: {1}; Time-To-Expiration: {2}; Discontfactor: {3}.", m_Strike, m_Forward, m_TimeToExpiration, m_DiscountFactor);
        }
        #endregion
    }
}