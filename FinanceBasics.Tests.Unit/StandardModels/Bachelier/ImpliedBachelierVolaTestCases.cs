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
using System.Collections.Generic;

using NUnit.Framework;

namespace Dodoni.Finance.StandardModels.Bachelier
{
    /// <summary>Provides test cases for calculation of implied volatilities with respect to the Bachelier model.
    /// </summary>
    public class ImpliedBachelierVolaTestCases
    {
        /// <summary>Gets a collection of test cases for a european call option with respect to the Bachelier model.
        /// </summary>
        /// <value>A collection of test cases for a european call option with respect to the Bachelier model.</value>
        public static IEnumerable<TestCaseData> CallOptions
        {
            get
            {
                var callSpecification_1 = new ConstantVolatilityStandardEuropeanOptionConfiguration() { Strike = 1.0, Forward = 1.05, TimeToExpiry = 4.5, DiscountFactor = 0.76, Volatility = 0.14 };
                yield return new TestCaseData(callSpecification_1, GetCalllValue(callSpecification_1));

                var callSpecification_2 = new ConstantVolatilityStandardEuropeanOptionConfiguration { Strike = 1.20, Forward = 1.3, TimeToExpiry = 3.5, DiscountFactor = 0.5, Volatility = 0.05 };
                yield return new TestCaseData(callSpecification_2, GetCalllValue(callSpecification_2));

                var callSpecification_3 = new ConstantVolatilityStandardEuropeanOptionConfiguration() { Strike = 0.95, Forward = 1.02, TimeToExpiry = 0.64, DiscountFactor = 0.87, Volatility = 0.03 };
                yield return new TestCaseData(callSpecification_3, GetCalllValue(callSpecification_3));
            }
        }

        /// <summary>Gets the price of the specific option.
        /// </summary>
        /// <param name="optionSpecification">The specification of the option.</param>
        /// <returns>The value of the specific option.</returns>
        private static double GetCalllValue(ConstantVolatilityStandardEuropeanOptionConfiguration optionSpecification)
        {
            return new BachelierEuropeanCall(optionSpecification.Strike, optionSpecification.Forward, optionSpecification.TimeToExpiry, optionSpecification.DiscountFactor).GetValue(optionSpecification.Volatility);
        }

        /// <summary>Gets a collection of test cases for a european put option with respect to the Bachelier model.
        /// </summary>
        /// <value>A collection of test cases for a european put option with respect to the Bachelier model.</value>
        public static IEnumerable<TestCaseData> PutOptions
        {
            get
            {
                var putSpecification_1 = new ConstantVolatilityStandardEuropeanOptionConfiguration() { Strike = 1.0, Forward = 1.05, TimeToExpiry = 4.5, DiscountFactor = 0.76, Volatility = 0.14 };
                yield return new TestCaseData(putSpecification_1, GetPutValue(putSpecification_1));

                var putSpecification_2 = new ConstantVolatilityStandardEuropeanOptionConfiguration { Strike = 1.20, Forward = 1.3, TimeToExpiry = 3.5, DiscountFactor = 0.5, Volatility = 0.05 };
                yield return new TestCaseData(putSpecification_2, GetPutValue(putSpecification_2));

                var putSpecification_3 = new ConstantVolatilityStandardEuropeanOptionConfiguration() { Strike = 0.95, Forward = 1.02, TimeToExpiry = 0.64, DiscountFactor = 0.87, Volatility = 0.03 };
                yield return new TestCaseData(putSpecification_3, GetPutValue(putSpecification_3));
            }
        }

        /// <summary>Gets the price of the specific option.
        /// </summary>
        /// <param name="optionSpecification">The specification of the option.</param>
        /// <returns>The value of the specific option.</returns>
        private static double GetPutValue(ConstantVolatilityStandardEuropeanOptionConfiguration optionSpecification)
        {
            return new BachelierEuropeanPut(optionSpecification.Strike, optionSpecification.Forward, optionSpecification.TimeToExpiry, optionSpecification.DiscountFactor).GetValue(optionSpecification.Volatility);
        }

        /// <summary>Gets a collection of test cases for a european straddle option with respect to the Bachelier model.
        /// </summary>
        /// <value>A collection of test cases for a european straddle option with respect to the Bachelier model.</value>
        public static IEnumerable<TestCaseData> StraddleOptions
        {
            get
            {
                var straddleSpecification_1 = new ConstantVolatilityStandardEuropeanOptionConfiguration() { Strike = 1.0, Forward = 1.05, TimeToExpiry = 4.5, DiscountFactor = 0.76, Volatility = 0.14 };
                yield return new TestCaseData(straddleSpecification_1, GetStraddleValue(straddleSpecification_1));

                var straddleSpecification_2 = new ConstantVolatilityStandardEuropeanOptionConfiguration { Strike = 1.20, Forward = 1.3, TimeToExpiry = 3.5, DiscountFactor = 0.5, Volatility = 0.05 };
                yield return new TestCaseData(straddleSpecification_2, GetStraddleValue(straddleSpecification_2));

                var straddleSpecification_3 = new ConstantVolatilityStandardEuropeanOptionConfiguration() { Strike = 0.95, Forward = 1.02, TimeToExpiry = 0.64, DiscountFactor = 0.87, Volatility = 0.03 };
                yield return new TestCaseData(straddleSpecification_3, GetStraddleValue(straddleSpecification_3));
            }
        }

        /// <summary>Gets the price of the specific option.
        /// </summary>
        /// <param name="optionSpecification">The specification of the option.</param>
        /// <returns>The value of the specific option.</returns>
        private static double GetStraddleValue(ConstantVolatilityStandardEuropeanOptionConfiguration optionSpecification)
        {
            return new BachelierEuropeanStraddle(optionSpecification.Strike, optionSpecification.Forward, optionSpecification.TimeToExpiry, optionSpecification.DiscountFactor).GetValue(optionSpecification.Volatility);
        }
    }
}