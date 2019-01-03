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
using System.Collections.Generic;

using Dodoni.MathLibrary;
using Dodoni.MathLibrary.ProbabilityTheory.Distributions;

namespace Dodoni.Finance.StandardModels.Bachelier
{
    /// <summary>Represents the implementation of the Bachelier model (=Normal Black model) for an european straddle, 
    /// i.e. the payoff is |spot -strike| and the price is given by the price of an european call + put option; assuming a 
    /// normal-distribution of the underlying.
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
    public class BachelierEuropeanStraddle : CommonEuropeanBachelierOption, IConstantVolatilityStandardEuropeanOption
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

        #region private consts

        /// <summary>The maximal number of iterations for the computation of the implied strike.
        /// </summary>
        private const int MaxNumberIterationImpliedStrike = 1000;
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

        /// <summary>Initializes the <see cref="BachelierEuropeanStraddle" /> class.
        /// </summary>
        static BachelierEuropeanStraddle()
        {
            sm_StandardImpliedVolatilityApproach = ImpliedBachelierVolatilityApproach.SOR.Create();
        }
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="BachelierEuropeanStraddle"/> class.
        /// </summary>
        /// <param name="impliedVolatilityApproach">The algorithm used to calculate implied volatility.</param>
        public BachelierEuropeanStraddle(IImpliedVolatilityApproach impliedVolatilityApproach = null)
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

        /// <summary>Initializes a new instance of the <see cref="BachelierEuropeanStraddle"/> class.
        /// </summary>
        /// <param name="strike">The strike.</param>
        /// <param name="forward">The forward.</param>
        /// <param name="timeToExpiry">The time span between valuation date and expiry date.</param>
        /// <param name="discountFactor">The discount factor at <paramref name="timeToExpiry"/>.</param>
        /// <param name="impliedVolatilityApproach">The algorithm used to calculate implied volatility.</param>
        public BachelierEuropeanStraddle(double strike, double forward, double timeToExpiry, double discountFactor, IImpliedVolatilityApproach impliedVolatilityApproach = null)
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
        /// <returns>A value indicating whether <paramref name="impliedVolatility"/> contains valid data.
        /// </returns>
        /// <remarks>This method is the inverse function of <see cref="IConstantVolatilityStandardOption.GetValue(double)"/>.</remarks>
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
                return m_DiscountFactor * (m_Strike - m_Forward);
            }
            double d = (m_Strike - m_Forward) / (volatility * SqrtOfTimeToExpiration);
            return m_DiscountFactor * 2.0 * SqrtOfTimeToExpiration * StandardNormalDistribution.GetPdfValue(d);
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
            return m_DiscountFactor * 2.0 * d * d * SqrtOfTimeToExpiration / volatility * StandardNormalDistribution.GetPdfValue(d);
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
            return m_DiscountFactor * StandardNormalDistribution.GetPdfValue(d) * volatility / SqrtOfTimeToExpiration;
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
                return Math.Abs(m_Strike - m_Forward);
            }
            // here, we use the fact N(-d) = 1 - N(d):
            double d = (m_Forward - m_Strike) / (volatility * SqrtOfTimeToExpiration);
            return 2.0 * (volatility * SqrtOfTimeToExpiration * StandardNormalDistribution.GetPdfValue(d) + (m_Strike - m_Forward) * (0.5 - StandardNormalDistribution.GetCdfValue(d)));
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
        /// <returns>A value indicating whether <paramref name="impliedStrike"/> contains valid data.
        /// </returns>
        /// <remarks>This method is based on Newtons method where the initial guess is equal to the approximation given in
        ///   <para>How close are the option pricing formulas of Bachelier and Black-Merton-Scholes, W. Schachermayer, J. Teichmann.</para>
        /// <para>The implied strike is not unique for straddle options.</para>
        /// </remarks>
        public ImpliedCalculationResultState TryGetImpliedStrike(double optionValue, double volatility, out double impliedStrike)
        {
            double undiscountedValue = optionValue / m_DiscountFactor;

            if (m_TimeToExpiration < MachineConsts.Epsilon)
            {
                impliedStrike = m_Forward - undiscountedValue;
                return ImpliedCalculationResultState.ProperResult;
            }

            /* compute initial guess:
             *    Straddle(m) = C(m) + P(m) \approx 2 * a + m^2/( 2 pi * a), 
             * see eqn.(3.4)&(3.5), where the left hand side is already given.
             */
            double a = volatility * SqrtOfTimeToExpiration / MathConsts.SqrtTwoPi;
            double m = Math.Sqrt(Math.Abs((undiscountedValue - 2 * a) * MathConsts.TwoPi * a));
            impliedStrike = m_Forward + m;

            /* we apply Newtons method to the function
             *     H(K) = ln( f(K) / c),
             * where f(K) is the (undiscounted) value of the option and c is the given undiscounted value 
             * of the option. It holds H'(K) = f'(K)/f(K) and Newtons method gives
             * 
             *      K_{n+1} = K_n - H(K_n)/H'(K_n) = K_n - ln(f(K)/c) * f(K) / f'(K),
             *      
             * here f'(K) = 1 - 2 * N(d) which will be equal to 0, if K=forward.
             */

            double absOfValueAtExpiry = Math.Abs(undiscountedValue);
            double optionValueForGivenStrike = undiscountedValue;
            double sqrtOfTime = SqrtOfTimeToExpiration;

            double d, CDF_d;
            for (int i = 1; i <= MaxNumberIterationImpliedStrike; i++)
            {
                d = (m_Forward - impliedStrike) / (volatility * sqrtOfTime);
                CDF_d = StandardNormalDistribution.GetCdfValue(d);
                optionValueForGivenStrike = 2.0 * (volatility * sqrtOfTime * StandardNormalDistribution.GetPdfValue(d) + (impliedStrike - m_Forward) * (0.5 - CDF_d));

                if (Math.Abs(d) < MachineConsts.Epsilon) // forward = strike and f'(K) = 0, i.e. denominator = 0
                {
                    if (Math.Abs(undiscountedValue - optionValueForGivenStrike) < absOfValueAtExpiry * MachineConsts.TinyEpsilon)
                    {
                        return ImpliedCalculationResultState.ProperResult;
                    }
                    /* here, we do a better approximation for the initial guess of the implied strike,
                     * i.e. straddle = C(m) + P(m) = 2 a + m^2/(2Pi a) - m^4/(48 * Pi^2 a^3), we use
                     * a root-finding approach the these polynomials and we use the root 'm' (= strike - forward) 
                     * where Straddle(m) - undiscountedValue is minimal with respect to each root.
                     */
                    IRealPolynomial polynomial = Polynomial.Real.Create(2 * a, 0, 1.0 / (a * MathConsts.TwoPi), 0, -1.0 / (a * a * a * 48 * MathConsts.PiSquared));
                    List<double> roots = new List<double>();
                    polynomial.GetRealRoots(roots, Polynomial.RootFinder.LaguerreApproach.StandardPolishing);

                    if (roots.Count == 0)
                    {
                        throw new ArithmeticException("Straddle approximation polynomial of degree 4 has no real root.");
                    }
                    else
                    {
                        impliedStrike = m_Forward + roots[0];
                        double minDistance = Math.Abs(undiscountedValue - GetUndiscountedOptionValue(roots[0], volatility, sqrtOfTime));
                        for (int j = 1; j < roots.Count; j++)
                        {
                            double distance = Math.Abs(undiscountedValue - GetUndiscountedOptionValue(roots[j], volatility, sqrtOfTime));
                            if (distance < minDistance)
                            {
                                impliedStrike = m_Forward + roots[j];
                                minDistance = distance;
                            }
                        }
                    }
                }
                else
                {
                    impliedStrike -= optionValueForGivenStrike * Math.Log(optionValueForGivenStrike / undiscountedValue) / (1 - 2 * CDF_d);
                }
                if (Math.Abs(undiscountedValue - optionValueForGivenStrike) < absOfValueAtExpiry * MachineConsts.TinyEpsilon)
                {
                    return ImpliedCalculationResultState.ProperResult;
                }
            }
            return ImpliedCalculationResultState.NoProperResult;
        }

        /// <summary>Gets the forward-delta of the option, i.e. the partial derivative of the option value formula with respect to the forward.
        /// </summary>
        /// <param name="volatility">The volatility.</param>
        /// <returns>The forward-delta of the option, i.e. the partial derivative of the option value formula with respect to the forward.
        /// </returns>
        public double GetForwardDelta(double volatility)
        {
            if (volatility * volatility * m_TimeToExpiration < MachineConsts.Epsilon)
            {
                return (m_Forward >= m_Strike) ? 1 : -1;
            }
            double d = (m_Forward - m_Strike) / (volatility * SqrtOfTimeToExpiration);
            return m_DiscountFactor * (2 * StandardNormalDistribution.GetCdfValue(d) - 1);
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
            if (volatility * volatility * m_TimeToExpiration < MachineConsts.Epsilon)
            {
                return 0.0;
            }
            double d = (m_Strike - m_Forward) / (volatility * SqrtOfTimeToExpiration);
            return 2.0 * m_DiscountFactor * StandardNormalDistribution.GetPdfValue(d) / (volatility * SqrtOfTimeToExpiration);
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
            if (volatility * volatility * m_TimeToExpiration < MachineConsts.Epsilon)
            {
                return 0;
            }
            double d = (m_Strike - m_Forward) / (volatility * SqrtOfTimeToExpiration);
            return m_DiscountFactor * 2.0 * d * StandardNormalDistribution.GetPdfValue(d) / volatility;
        }

        /// <summary>Gets the strike-delta of the option, i.e. the partial derivative of the option value formula with respect to the strike.
        /// </summary>
        /// <param name="volatility">The volatility.</param>
        /// <returns>The strike-delta of the option, i.e. the partial derivative of the option value formula with respect to the strike.</returns>
        public double GetStrikeDelta(double volatility)
        {
            if (volatility * volatility * m_TimeToExpiration < MachineConsts.Epsilon)
            {
                return 0.0;
            }
            double d = (m_Forward - m_Strike) / (volatility * SqrtOfTimeToExpiration);
            return m_DiscountFactor * (1.0 - 2.0 * StandardNormalDistribution.GetCdfValue(d));
        }
        #endregion

        /// <summary>Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return String.Format("European Straddle (Bachelier); Strike: {0}; Forward: {1}; Time-To-Expiration: {2}; Discontfactor: {3}.", m_Strike, m_Forward, m_TimeToExpiration, m_DiscountFactor);
        }
        #endregion

        #region private static methods

        /// <summary>Gets the undiscounted option value using the Bachelier (=Normal Black) model thus the value <c>at expiry</c> of the option.
        /// </summary>
        /// <param name="strikeMinusForward">'Strike minus forward'.</param>
        /// <param name="volatility">The Bachelier (=Normal Black) volatility.</param>
        /// <param name="sqrtOfTime">The square root of the <see cref="System.Double"/> representation of the time span between valuation date and expiry date.</param>
        /// <returns>The <b>undiscounted</b> Bachelier (=Normal Black) price of the call option, thus the value <c>at time of expiry</c>, i.e. 
        /// to get the price just multiply the return value with the discount factor.</returns>
        private static double GetUndiscountedOptionValue(double strikeMinusForward, double volatility, double sqrtOfTime)
        {
            // here, we use the fact N(-d) = 1 - N(d):
            double d = -strikeMinusForward / (volatility * sqrtOfTime);
            return 2.0 * (volatility * sqrtOfTime * StandardNormalDistribution.GetPdfValue(d) + strikeMinusForward * (0.5 - StandardNormalDistribution.GetCdfValue(d)));
        }
        #endregion
    }
}