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
        /// <summary>Represents the implementation of the implied (Black) volatility approach based on <para>'By implication', Peter Jäckel.</para></summary>
        public class Jaeckel : BlackEuropeanCall.IImpliedVolatilityApproach, BlackEuropeanPut.IImpliedVolatilityApproach, BlackEuropeanStraddle.IImpliedVolatilityApproach
        {
            #region private members

            /// <summary>The maximal number of iterations for the computation of the implied volatility.
            /// </summary>
            private int m_MaxNumberOfIterations;

            /// <summary>The tolerance taken into account for the exit condition.
            /// </summary>
            private double m_Tolerance;
            #endregion

            #region private constructors

            /// <summary>Initializes a new instance of the <see cref="Jaeckel"/> class.
            /// </summary>
            /// <param name="maxNumberOfIterations">The maximal number of iterations.</param>
            /// <param name="tolerance">The tolerance taken into account for the exit condition.</param>
            private Jaeckel(int maxNumberOfIterations = 10, double tolerance = MachineConsts.TinyEpsilon)
            {
                m_MaxNumberOfIterations = maxNumberOfIterations;
                m_Tolerance = tolerance;
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
                return TryGetPlainVanillaImpliedVolatility(1, strike, forward, timeToExpiry, noneDiscountedValue, out value);
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
                return TryGetPlainVanillaImpliedVolatility(-1, strike, forward, timeToExpiry, noneDiscountedValue, out value);
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
                /* The value of a straddle option is the sum of a call and put option price. It is equivalent to a transformed call option, the value is:
                 * 2 * [none-discounted] callValue + discfactor * (strike - forward) */
                return TryGetPlainVanillaImpliedVolatility(1, strike, forward, timeToExpiry, noneDiscountedValue / 2.0 - 0.5 * (strike - forward), out value);
            }
            #endregion

            /// <summary>Returns a <see cref="System.String"/> that represents this instance.
            /// </summary>
            /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
            public override string ToString()
            {
                return "Jaeckel Implied Black Volatility appoach";
            }
            #endregion

            #region public static methods

            /// <summary>Creates a new <see cref="Jaeckel"/> instance.
            /// </summary>
            /// <param name="maxNumberOfIterations">The maximal number of iterations.</param>
            /// <param name="tolerance">The tolerance taken into account for the exit condition.</param>
            public static Jaeckel Create(int maxNumberOfIterations = 10, double tolerance = MachineConsts.TinyEpsilon)
            {
                return new Jaeckel(maxNumberOfIterations, tolerance);
            }
            #endregion

            #region private methods

            /// <summary>Gets the implied Black-Scholes volatility for a given value (price) of a specific call or put option.
            /// </summary>
            /// <param name="theta">A value indicating whether a call (=1) or put (=-1) is given.</param>
            /// <param name="strike">The strike.</param>
            /// <param name="forward">The forward.</param>
            /// <param name="timeToExpiry">The time span between valuation date and expiry date in its <see cref="System.Double"/> representation.</param>
            /// <param name="noneDiscountedValue">The value of the option at the time of expiry, thus the price but <b>not</b> discounted to time 0.</param>
            /// <param name="impliedBlackVolatility">The implied Black volatility (output).</param>
            /// <returns>A value indicating whether <paramref name="impliedBlackVolatility"/> contains valid data.</returns>
            private ImpliedCalculationResultState TryGetPlainVanillaImpliedVolatility(int theta, double strike, double forward, double timeToExpiry, double noneDiscountedValue, out double impliedBlackVolatility)
            {
                impliedBlackVolatility = 0.0;

                /* calculate some constants only once: */
                double x = Math.Log(forward / strike);
                if (Double.IsNaN(x))  // strike <= 0?
                {
                    return ImpliedCalculationResultState.InputError;
                }

                double beta = noneDiscountedValue / Math.Sqrt(forward * strike);  // normalized value: the given value is already discounted

                /* 1. case: at-the-money option:
                 * Compute the implied vola via inverse commulative distribution function */
                if (Math.Abs(x) < MachineConsts.Epsilon)  // at-the-money option
                {
                    impliedBlackVolatility = -2.0 * StandardNormalDistribution.GetInverseCdfValue(0.5 - 0.5 * beta);
                    return ImpliedCalculationResultState.ProperResult;
                }

                /* 2. case: in-the-money option: 
                 * subtracting the normalized intrinsic value from the normalized value if necessary */

                double expOfXDivTwo = Math.Sqrt(forward / strike);  // =  Math.Exp(x / 2.0);
                double iota = theta * DoMath.HeavisideFunction(theta * x) * (expOfXDivTwo - 1 / expOfXDivTwo);

                /* the normalized value has to be an element of the interval
                 *           [iota, exp( \theta * x/2)],
                 * otherwise no implied value can be calculated! 
                 */
                if (beta < iota)
                {
                    return ImpliedCalculationResultState.InputError;  // one may sets 'beta = iota;'
                }
                if (beta > ((theta == 1) ? expOfXDivTwo : 1 / expOfXDivTwo))
                {
                    return ImpliedCalculationResultState.InputError;  // one may sets 'beta = ((theta == 1) ? expOfXDivTwo : 1 / expOfXDivTwo);'
                }
                bool isInTheMoneyOption = false;  // indicates if it is a in-the-money option

                if (theta == -1)
                {
                    if (x < 0)
                    {
                        isInTheMoneyOption = true;
                    }
                }
                else
                {
                    if (x > 0)  // strike < forward
                    {
                        isInTheMoneyOption = true;
                    }
                }
                if (isInTheMoneyOption == true)
                {
                    beta -= iota;
                    theta = 1 - 2 * DoMath.HeavisideFunction(x);

                    /* now consider some out-of-the money option and recalculate the intrinsic value */
                    iota = theta * DoMath.HeavisideFunction(theta * x) * (expOfXDivTwo - 1 / expOfXDivTwo);
                }

                /* 3.) Set initual guess: */
                double bc = NormalizedCallPutValue(theta, x, expOfXDivTwo, Math.Sqrt(2 * Math.Abs(x)));

                /* 3a.) sigmaLow and sigmaHigh are given by formula (3.6)-(3.7) */
                double tempForSigmaHighExp = (theta == 1) ? expOfXDivTwo : 1 / expOfXDivTwo;            // = Math.Exp(theta * x / 2.0)
                double tempForSigmaHighCDF = StandardNormalDistribution.GetCdfValue(-Math.Sqrt(Math.Abs(x) / 2.0));

                double sigmaLow = Math.Sqrt(2 * x * x / (Math.Abs(x) - 4 * Math.Log((beta - iota) / (bc - iota))));
                double sigmaHigh = -2 * StandardNormalDistribution.GetInverseCdfValue((tempForSigmaHighExp - beta) / (tempForSigmaHighExp - bc) * tempForSigmaHighCDF);

                /* 2b.) calculate the gamma which is given in formula (4.7): */
                double sigmaStar = -2 * StandardNormalDistribution.GetInverseCdfValue(tempForSigmaHighExp / (tempForSigmaHighExp - bc) * tempForSigmaHighCDF);
                double bStar = NormalizedCallPutValue(theta, x, expOfXDivTwo, sigmaStar);

                double sigmaLowStar = Math.Sqrt(2 * x * x / (Math.Abs(x) - 4 * Math.Log((bStar - iota) / (bc - iota))));
                double sigmaHighStar = -2 * StandardNormalDistribution.GetInverseCdfValue((tempForSigmaHighExp - bStar) / (tempForSigmaHighExp - bc) * tempForSigmaHighCDF);

                // the weight w^* given by formula (4.8)
                double w = Math.Pow(Math.Min(Math.Max((sigmaStar - sigmaLowStar) / (sigmaHighStar - sigmaLowStar), 0), 1.0), Math.Log(bc / beta) / Math.Log(bc / bStar));

                /* the code based on the 2006 edition of the paper, i.e. using formula (4.1) in "By implication", 24.11.2010:
                // double gamma = Math.Log((sigmaStar - sigmaLowStar) / (sigmaHighStar - sigmaLowStar)) / Math.Log(bStar / bc);
                // double w = Math.Min(1, Math.Pow(beta / bc, gamma));   // its the weight given by (4.2)

                /* 2c.) now the initial value is given; see formula (4.1):  */
                if (Double.IsNaN(sigmaLow) == false)
                {
                    impliedBlackVolatility = sigmaLow * (1 - w) + sigmaHigh * w;
                }
                else
                {
                    /* it is not hard to see that if \sigma_low is not defined then 'beta/bc' > 1
                     * which implies w = 1.0 */
                    impliedBlackVolatility = sigmaHigh;
                }

                /* 3.) Do the Halley's iteration method: */
                double terminatingCondition = 1.0;
                for (int i = 1; i <= m_MaxNumberOfIterations; i++)
                {
                    /* A.) compute nu^_n(x,\sigma_n,\Theta) via formula (4.10) & (4.13): */

                    // first compute nu_n (without 'hat') via (3.10): 
                    double b = NormalizedCallPutValue(theta, x, expOfXDivTwo, impliedBlackVolatility);
                    double impliedBSVolaSquare = impliedBlackVolatility * impliedBlackVolatility;

                    double bDash = MathConsts.OneOverSqrtTwoPi * Math.Exp(-0.5 * x * x / impliedBSVolaSquare - 0.125 * impliedBSVolaSquare);

                    double nuN = 0.0;
                    if (beta < bc)
                    {
                        nuN = Math.Log((beta - iota) / (b - iota)) * Math.Log(b - iota) / Math.Log(beta - iota) * (b - iota) / bDash;
                    }
                    else
                    {
                        nuN = (beta - b) / bDash;
                    }
                    double nuHat = Math.Max(nuN, -impliedBlackVolatility / 2.0);

                    /* B. compute eta^ via formula (4.14) & (4.15) & (4.11): */
                    double etaHat = x * x / (impliedBSVolaSquare * impliedBlackVolatility) - impliedBlackVolatility / 4.0;
                    if (beta < bc)
                    {
                        double logOfbMinusIota = Math.Log(b - iota);
                        etaHat -= (2 + logOfbMinusIota) / logOfbMinusIota * bDash / (b - iota);
                    }
                    etaHat = Math.Max(-0.75, etaHat / 2.0 * nuHat);

                    /* C. next step and store the terminating condition */
                    terminatingCondition = impliedBlackVolatility;  // store the volatility which is used in the step before
                    impliedBlackVolatility = impliedBlackVolatility + Math.Max(nuHat / (1 + etaHat), -impliedBlackVolatility / 2.0);

                    terminatingCondition = Math.Abs(impliedBlackVolatility / terminatingCondition - 1);

                    if (Double.IsNaN(terminatingCondition))
                    {
                        impliedBlackVolatility = Double.NaN;
                        return ImpliedCalculationResultState.NoProperResult;
                    }
                    if (terminatingCondition < m_Tolerance)
                    {
                        impliedBlackVolatility /= Math.Sqrt(timeToExpiry);
                        return ImpliedCalculationResultState.ProperResult;
                    }
                }
                impliedBlackVolatility /= Math.Sqrt(timeToExpiry);
                return ImpliedCalculationResultState.NoProperResult;
            }

            /// <summary>Gets the normalized value (price) of a call or put option.
            /// </summary>
            /// <param name="theta">Indicates if a call (=1) or put (=-1) is given.</param>
            /// <param name="x">The logarithm of 'forward / strike'.</param>
            /// <param name="expOfXDivTwo"><paramref name="x"/> divided by two and applied to the exponential function. This
            /// argument will be computed once only (performance reason).</param>
            /// <param name="normalisedVolatility">The normalized volatility, i.e. the volatility divided by the 
            /// square root of the time span between valuation and expiry date.</param>
            /// <returns>The normalized value of the call/put option, thus multiplying the result
            /// by the specific discount factor and the square root of forward * strike gives the value of the option.</returns>
            /// <remarks>
            /// The normalized price (value) of the option is given by 
            /// <para>
            ///      value / (discount factor * Sqrt(forward * strike))
            /// </para>
            /// thus multiply the result by the discount factor * Sqrt(forward * strike)
            /// to get the correct price.
            /// <para>The implementation is based on 'By implication', Peter Jäckel, formula (2.2)-(2.3).</para>
            /// </remarks>
            private static double NormalizedCallPutValue(int theta, double x, double expOfXDivTwo, double normalisedVolatility)
            {
                if (Math.Abs(x) < MachineConsts.Epsilon)
                {
                    return 1 - 2 * StandardNormalDistribution.GetCdfValue(-normalisedVolatility / 2.0);
                }
                return theta * (expOfXDivTwo * StandardNormalDistribution.GetCdfValue(theta * (x / normalisedVolatility + normalisedVolatility / 2.0))
                    - 1.0 / expOfXDivTwo * StandardNormalDistribution.GetCdfValue(theta * (x / normalisedVolatility - normalisedVolatility / 2.0)));
            }
            #endregion
        }
    }
}