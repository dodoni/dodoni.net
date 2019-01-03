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
using System.Text;
using System.Collections.Generic;

using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Dodoni.Finance.StandardModels.Bachelier
{
    /// <summary>Serves as class for unit tests with respect to <see cref="BachelierEuropeanPut"/>.
    /// </summary>
    [TestFixture]
    public class BachelierEuropeanPutTests : ConstantVolatilityStandardEuropeanOptionTests
    {
        /// <summary>Serves as unit test for <see cref="IConstantVolatilityStandardOption.GetValue(double)"/> method.
        /// </summary>
        /// <param name="strike">The strike.</param>
        /// <param name="forward">The forward.</param>
        /// <param name="timeToExpiry">The time span between valuation date and expiry date.</param>
        /// <param name="discountFactor">The discount factor at <paramref name="timeToExpiry"/>.</param>
        /// <param name="volatiliy">The volatility.</param>
        /// <param name="expectedOptionValue">The expected option value.</param>
        /// <param name="tolerance">The tolerance to apply for the comparison of the <paramref name="expectedOptionValue"/> and the calculated option value. This is
        /// required because many benchmark results in the literature are rounded up to a specific number of digits.</param>
        /// <remarks>Often in the literature the parameters are specified in a different way, for example one
        /// has to compute the forward, discount factor etc.</remarks>
        //[TestCase(90.0, 91.7354422, 0.75, 0.977751237, 0.29, 9.75, 1E-2, TestName = "K=90,F=91.73544,t=0.75,df=0.97775,sigma=0.29: http://www.fbmn.h-da.de/~ochs/pdf/black-scholes.pdf")]
        //[TestCase(65.0, 61.2120804016053, 0.25, 0.980198673306755, 0.30, 2.1334, 1E-3, TestName = "E. G. Haug, The complete Guide to option pricing formulas 2ed, p.3")]
        //[TestCase(19.0, 19.0, 0.75, 0.927743486328553, 0.28, 1.7011, 1E-3, TestName = "E. G. Haug, The complete Guide to option pricing formulas 2ed, p.5")]
        //[TestCase(40, 44.153386047793, 0.5, 0.951229424500714, 0.20, 4.76, 1E-2, TestName = "J. Hull, Option, Futures & other Derivatives, 5.ed, Example 12.7")]
        //[TestCase(40.0, 44.15338605, 0.5, 0.951229425, 0.2, 4.76, 1E-2, TestName = "A. Irle, Finanzmathematik. Die Bewertung von Derivaten, p.156")]
        public override void GetOptionValue_TestCase(double strike, double forward, double timeToExpiry, double discountFactor, double volatiliy, double expectedOptionValue, double tolerance)
        {
            base.GetOptionValue_TestCase(strike, forward, timeToExpiry, discountFactor, volatiliy, expectedOptionValue, tolerance);
        }

        #region protected methods

        /// <summary>Creates a specific <see cref="IConstantVolatilityStandardEuropeanOption"/> option.
        /// </summary>
        /// <param name="strike">The strike.</param>
        /// <param name="forward">The forward.</param>
        /// <param name="timeToExpiry">The time to expiry.</param>
        /// <param name="discountFactor">The discount factor.</param>
        /// <returns>A <see cref="IConstantVolatilityStandardEuropeanOption"/> object with the desired parameters.
        /// </returns>
        protected override IConstantVolatilityStandardEuropeanOption CreateOption(double strike, double forward, double timeToExpiry, double discountFactor)
        {
            return new BachelierEuropeanPut(strike, forward, timeToExpiry, discountFactor);
        }

        /// <summary>Gets a collection of parameters for the options to take into account for the unit tests
        /// based on the Greek approximation by differential quotients.
        /// </summary>
        /// <value>The collection of parameters of the options to take into account for the unit tests.
        /// </value>
        protected override IEnumerable<ConstantVolatilityStandardEuropeanOptionConfiguration> TestOptionParametersForApproximationTests
        {
            get
            {
                yield return new ConstantVolatilityStandardEuropeanOptionConfiguration() { Strike = 100, Forward = 101, TimeToExpiry = 4.5, DiscountFactor = 0.76, Volatility = 0.14 };
                yield return new ConstantVolatilityStandardEuropeanOptionConfiguration() { Strike = 120, Forward = 100, TimeToExpiry = 3.5, DiscountFactor = 0.5, Volatility = 0.05 };
                yield return new ConstantVolatilityStandardEuropeanOptionConfiguration() { Strike = 0.95, Forward = 1.02, TimeToExpiry = 0.64, DiscountFactor = 0.87, Volatility = 0.03 };
                yield return new ConstantVolatilityStandardEuropeanOptionConfiguration() { Strike = 40.0, Forward = 44.15338605, TimeToExpiry = 0.5, DiscountFactor = 0.951229425, Volatility = 0.2 };
            }
        }

        /// <summary>Determines whether a volatility should be computable that implies a specified option value.
        /// </summary>
        /// <param name="optionValue">The option value.</param>
        /// <param name="option">The option.</param>
        /// <returns>
        /// 	<c>true</c> if there should be an implied volatility for the specified option value be available; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>The implied volatility of Far-out-of-the-money options can often not be computed. One can design the individual test cases or one may override this
        /// method in an individual manner to avoid questionable results in the unit tests.</remarks>
        protected override bool IsVolatilityInvertibleOptionValue(double optionValue, IConstantVolatilityStandardEuropeanOption option)
        {
            /* the option value should be >= 0 and the (undiscounted) option price should not be Strike-Forward, see remarks in
             * 'Numerical approximation of the implied volatility under arithmetic Brownian motion', J. Choi, K. Kim, M. Kwak (2007)
             * and apply the call-put parity.
             */
            return base.IsVolatilityInvertibleOptionValue(optionValue, option) && (optionValue / option.DiscountFactor != option.Strike - option.Forward);
        }
        #endregion
    }
}