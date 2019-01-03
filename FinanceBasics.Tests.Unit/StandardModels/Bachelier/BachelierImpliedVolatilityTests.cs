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
using NUnit.Framework;

namespace Dodoni.Finance.StandardModels.Bachelier
{
    /// <summary>Serves as abstract unit test class for Bachelier implied volatility algorithms.
    /// </summary>
    /// <remarks>This implementation compares the volatilities \sigma not integrated volatilites \sigma * \sqrt{T}.</remarks>
    public abstract class BachelierImpliedVolatilityTests
    {
        /// <summary>Serves as unit test for <see cref="BachelierEuropeanCall.IImpliedVolatilityApproach.TryGetValue(double,double,double,double, out double)"/>.</summary>
        /// <param name="optionSpecification">The specification of the option.</param>
        /// <param name="optionValue">The value of the option.</param>
        [TestCaseSource(typeof(ImpliedBachelierVolaTestCases), "CallOptions")]
        public void TryGetValue_CallTestCase_BenchmarkVolatility(ConstantVolatilityStandardEuropeanOptionConfiguration optionSpecification, double optionValue)
        {
            double tolerance;
            var objectUnderTest = GetCallOptionImpliedVolatilityApproach(out tolerance);
            Assume.That(objectUnderTest, Is.Not.Null);

            double actual;
            var actualState = objectUnderTest.TryGetValue(optionSpecification.Strike, optionSpecification.Forward, optionSpecification.TimeToExpiry, optionValue / optionSpecification.DiscountFactor, out actual);

            Assert.That(actual, Is.EqualTo(optionSpecification.Volatility).Within(tolerance));
            Assert.That(actualState, Is.EqualTo(ImpliedCalculationResultState.ProperResult));
        }

        /// <summary>Gets the object under test.
        /// </summary>
        /// <param name="tolerance">The tolerance to take into account.</param>
        /// <returns>The object under test.</returns>
        protected abstract BachelierEuropeanCall.IImpliedVolatilityApproach GetCallOptionImpliedVolatilityApproach(out double tolerance);

        /// <summary>Serves as unit test for <see cref="BachelierEuropeanPut.IImpliedVolatilityApproach.TryGetValue(double,double,double,double, out double)"/>.</summary>
        /// <param name="optionSpecification">The specification of the option.</param>
        /// <param name="optionValue">The value of the option.</param>
        [TestCaseSource(typeof(ImpliedBachelierVolaTestCases), "PutOptions")]
        public void TryGetValue_PutTestCase_BenchmarkVolatility(ConstantVolatilityStandardEuropeanOptionConfiguration optionSpecification, double optionValue)
        {
            double tolerance;
            var objectUnderTest = GetPutOptionImpliedVolatilityApproach(out tolerance);
            Assume.That(objectUnderTest, Is.Not.Null);

            double actual;
            var actualState = objectUnderTest.TryGetValue(optionSpecification.Strike, optionSpecification.Forward, optionSpecification.TimeToExpiry, optionValue / optionSpecification.DiscountFactor, out actual);

            Assert.That(actual, Is.EqualTo(optionSpecification.Volatility).Within(tolerance));
            Assert.That(actualState, Is.EqualTo(ImpliedCalculationResultState.ProperResult));
        }

        /// <summary>Gets the object under test.
        /// </summary>
        /// <param name="tolerance">The tolerance to take into account.</param>
        /// <returns>The object under test.</returns>
        protected abstract BachelierEuropeanPut.IImpliedVolatilityApproach GetPutOptionImpliedVolatilityApproach(out double tolerance);

        /// <summary>Serves as unit test for <see cref="BachelierEuropeanStraddle.IImpliedVolatilityApproach.TryGetValue(double,double,double,double, out double)"/>.</summary>
        /// <param name="optionSpecification">The specification of the option.</param>
        /// <param name="optionValue">The value of the option.</param>
        [TestCaseSource(typeof(ImpliedBachelierVolaTestCases), "StraddleOptions")]
        public void TryGetValue_StraddleTestCase_BenchmarkVolatility(ConstantVolatilityStandardEuropeanOptionConfiguration optionSpecification, double optionValue)
        {
            double tolerance;
            var objectUnderTest = GetStraddleOptionImpliedVolatilityApproach(out tolerance);
            Assume.That(objectUnderTest, Is.Not.Null);

            double actual;
            var actualState = objectUnderTest.TryGetValue(optionSpecification.Strike, optionSpecification.Forward, optionSpecification.TimeToExpiry, optionValue / optionSpecification.DiscountFactor, out actual);

            Assert.That(actual, Is.EqualTo(optionSpecification.Volatility).Within(tolerance));
            Assert.That(actualState, Is.EqualTo(ImpliedCalculationResultState.ProperResult));
        }

        /// <summary>Gets the object under test.
        /// </summary>
        /// <param name="tolerance">The tolerance to take into account.</param>
        /// <returns>The object under test.</returns>
        protected abstract BachelierEuropeanStraddle.IImpliedVolatilityApproach GetStraddleOptionImpliedVolatilityApproach(out double tolerance);
    }
}