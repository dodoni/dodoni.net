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
    /// <summary>Serves as abstract basis class for Bachelier (=Normal Black) common european options, i.e. european call, put, digital call/put etc. The underlying
    /// is assumed to be normal distributed with a constant volatility etc.
    /// </summary>
    /// <remarks>See
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
    public class CommonEuropeanBachelierOption 
    {
        #region private consts

        /// <summary>The maximal number of iterations for the computation of the implied strike.
        /// </summary>
        private const int MaxNumberIterationImpliedStrike = 1000;
        #endregion

        #region private members

        /// <summary>The square-root of the time-to-expiry.
        /// </summary>
        /// <remarks>This value is frequently used and cached for performance reason.</remarks>
        private double m_SqrtOfTimeToExpiration;
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

        /// <summary>Initializes a new instance of the <see cref="CommonEuropeanBachelierOption"/> class.
        /// </summary>
        protected CommonEuropeanBachelierOption()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="CommonEuropeanBachelierOption"/> class.
        /// </summary>
        /// <param name="strike">The strike.</param>
        /// <param name="forward">The forward.</param>
        /// <param name="timeToExpiry">The time span between valuation date and expiry date.</param>
        /// <param name="discountFactor">The discount factor at <paramref name="timeToExpiry"/>.</param>
        protected CommonEuropeanBachelierOption(double strike, double forward, double timeToExpiry, double discountFactor)
        {
            m_Strike = strike;
            m_Forward = forward;
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
            set { m_Strike = value; }
        }

        /// <summary>Gets or sets the forward at <see cref="TimeToExpiry"/>.
        /// </summary>
        /// <value>The forward at <see cref="TimeToExpiry"/>.
        /// </value>
        public double Forward
        {
            get { return m_Forward; }
            set { m_Forward = value; }
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

        /// <summary>Gets the square-root of the time-to-expiry.
        /// </summary>
        /// <value>The square-root of the time-to-expiry..</value>
        protected double SqrtOfTimeToExpiration
        {
            get { return m_SqrtOfTimeToExpiration; }
        }
        #endregion

        #region protected static methods

        /// <summary>Gets the implied strike for a given value (price) and Bachelier (=Normal Black) volatility of a specific european call or put option.
        /// </summary>
        /// <param name="theta">A value indicating whether a call (=1) or put (=-1) is given.</param>
        /// <param name="forward">The forward.</param>
        /// <param name="undiscountedValue">The value of the option at the time of expiry, thus the price but <b>not</b> discounted to time 0.</param>
        /// <param name="bachelierVolatility">The Bachelier (=Normal Black) volatility.</param>
        /// <param name="sqrtOfTime">The square root of the time span between valuation date and expiry date.</param>
        /// <param name="impliedStrike">The implied strike (output), input represents the initial value of the Newton-Method.</param>
        /// <returns>Returns a value indicating whether <paramref name="impliedStrike"/> contains valid data.
        /// </returns>
        /// <remarks>The implementation is based on the Newton approach and the forward is used as initial guess.</remarks>
        protected static ImpliedCalculationResultState TryGetPlainVanillaImpliedStrike(int theta, double forward, double undiscountedValue, double bachelierVolatility, double sqrtOfTime, ref double impliedStrike)
        {
            /* we apply Newtons method to the function
             *     H(K) = ln( f(K) / c),
             * where f(K) is the (undiscounted) value of the option and c is the given undiscounted value 
             * of the option. It holds H'(K) = f'(K)/f(K) and Newtons method gives
             * 
             *      K_{n+1} = K_n - H(K_n)/H'(K_n) = K_n - ln(f(K)/c) * f(K) / f'(K).
             */
            double absOfValueAtExpiry = Math.Abs(undiscountedValue);
            double optionValueForGivenStrike = undiscountedValue;

            double d, CDF_Theta_d;
            for (int i = 1; i <= MaxNumberIterationImpliedStrike; i++)
            {
                if (impliedStrike < MachineConsts.SuperTinyEpsilon)
                {
                    optionValueForGivenStrike = forward + impliedStrike;
                    impliedStrike = forward - theta * undiscountedValue;
                }
                else
                {
                    d = (forward - impliedStrike) / (bachelierVolatility * sqrtOfTime);
                    CDF_Theta_d = StandardNormalDistribution.GetCdfValue(theta * d);
                    optionValueForGivenStrike = theta * (forward - impliedStrike) * CDF_Theta_d + bachelierVolatility * sqrtOfTime * StandardNormalDistribution.GetPdfValue(theta * d);

                    impliedStrike += theta * optionValueForGivenStrike * Math.Log(optionValueForGivenStrike / undiscountedValue) / CDF_Theta_d;
                }
                if (Math.Abs(undiscountedValue - optionValueForGivenStrike) < absOfValueAtExpiry * MachineConsts.TinyEpsilon)
                {
                    return ImpliedCalculationResultState.ProperResult;
                }
            }
            return ImpliedCalculationResultState.NoProperResult;
        }
        #endregion
    }
}