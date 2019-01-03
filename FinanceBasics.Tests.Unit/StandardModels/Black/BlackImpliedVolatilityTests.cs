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
using NUnit.Framework;

namespace Dodoni.Finance.StandardModels.Black
{
    /// <summary>Serves as abstract unit test class for Black implied volatility algorithms.
    /// </summary>
    /// <remarks>This implementation compares the volatilities \sigma not integrated volatilites \sigma * \sqrt{T}.</remarks>
    public abstract class BlackImpliedVolatilityTests
    {
        /// <summary>A value indicating whether to ignore a specific test.
        /// </summary>
        public enum ToleranceLevel
        {
            /// <summary>Do not ignore any test.
            /// </summary>
            DoNotIgnoreAnyTest,

            /// <summary>Ignore a test if the input of the implied Black approach is not suitable for the algorithm. For example the rational approximation is available for a specific range only.
            /// </summary>
            IgnoreTestIfUnsuitableProblem
        }

        /// <summary>Serves as unit test for <see cref="BlackEuropeanCall.IImpliedVolatilityApproach.TryGetValue(double,double,double,double, out double)"/>.</summary>
        /// <param name="optionSpecification">The specification of the option.</param>
        /// <param name="optionValue">The value of the option.</param>
        [TestCaseSource(typeof(ImpliedBlackVolaTestCases), "CallOptions")]
        public void TryGetValue_CallTestCase_BenchmarkVolatility(ConstantVolatilityStandardEuropeanOptionConfiguration optionSpecification, double optionValue)
        {
            double tolerance;
            Func<ConstantVolatilityStandardEuropeanOptionConfiguration, double, ToleranceLevel> testLevel;

            var objectUnderTest = GetCallOptionImpliedVolatilityApproach(out tolerance, out testLevel);
            Assume.That(objectUnderTest, Is.Not.Null, "Object under test does not support the specific option type.");

            double actual;
            var actualState = objectUnderTest.TryGetValue(optionSpecification.Strike, optionSpecification.Forward, optionSpecification.TimeToExpiry, optionValue / optionSpecification.DiscountFactor, out actual);

            if (testLevel != null)
            {
                var toleranceLevel = testLevel(optionSpecification, optionValue);
                if (toleranceLevel == ToleranceLevel.IgnoreTestIfUnsuitableProblem)
                {
                    Assume.That(actualState, Is.EqualTo(ImpliedCalculationResultState.ProperResult) | Is.EqualTo(ImpliedCalculationResultState.InputError), "Algorithm does not support the implied volatility calculation of the specific option! The algorithm is somehow restricted!");
                }
            }
            Assert.That(actual, Is.EqualTo(optionSpecification.Volatility).Within(tolerance));
            Assert.That(actualState, Is.EqualTo(ImpliedCalculationResultState.ProperResult));
        }

        /// <summary>Gets the object under test.
        /// </summary>
        /// <param name="tolerance">The tolerance to take into account (output).</param>
        /// <param name="testLevel">A delegate that returns for a specific test scenario a specific <see cref="BlackImpliedVolatilityTests.ToleranceLevel"/> object. The second argument is the option value. Perhaps <c>null</c>.</param>
        /// <returns>The object under test.</returns>
        protected abstract BlackEuropeanCall.IImpliedVolatilityApproach GetCallOptionImpliedVolatilityApproach(out double tolerance, out Func<ConstantVolatilityStandardEuropeanOptionConfiguration, double, ToleranceLevel> testLevel);

        /// <summary>Serves as unit test for <see cref="BlackEuropeanPut.IImpliedVolatilityApproach.TryGetValue(double,double,double,double, out double)"/>.</summary>
        /// <param name="optionSpecification">The specification of the option.</param>
        /// <param name="optionValue">The value of the option.</param>
        [TestCaseSource(typeof(ImpliedBlackVolaTestCases), "PutOptions")]
        public void TryGetValue_PutTestCase_BenchmarkVolatility(ConstantVolatilityStandardEuropeanOptionConfiguration optionSpecification, double optionValue)
        {
            double tolerance;
            Func<ConstantVolatilityStandardEuropeanOptionConfiguration, double, ToleranceLevel> testLevel;

            var objectUnderTest = GetPutOptionImpliedVolatilityApproach(out tolerance, out testLevel);
            Assume.That(objectUnderTest, Is.Not.Null, "Object under test does not support the specific option type.");

            double actual;
            var actualState = objectUnderTest.TryGetValue(optionSpecification.Strike, optionSpecification.Forward, optionSpecification.TimeToExpiry, optionValue / optionSpecification.DiscountFactor, out actual);

            if (testLevel != null)
            {
                var toleranceLevel = testLevel(optionSpecification, optionValue);
                if (toleranceLevel == ToleranceLevel.IgnoreTestIfUnsuitableProblem)
                {
                    Assume.That(actualState, Is.EqualTo(ImpliedCalculationResultState.ProperResult) | Is.EqualTo(ImpliedCalculationResultState.InputError), "Algorithm does not support the implied volatility calculation of the specific option! The algorithm is somehow restricted!");
                }
            }
            Assert.That(actual, Is.EqualTo(optionSpecification.Volatility).Within(tolerance));
            Assert.That(actualState, Is.EqualTo(ImpliedCalculationResultState.ProperResult));
        }

        /// <summary>Gets the object under test.
        /// </summary>
        /// <param name="tolerance">The tolerance to take into account.</param>
        /// <param name="testLevel">A delegate that returns for a specific test scenario a specific <see cref="BlackImpliedVolatilityTests.ToleranceLevel"/> object. The second argument is the option value. Perhaps <c>null</c>.</param>
        /// <returns>The object under test.</returns>
        protected abstract BlackEuropeanPut.IImpliedVolatilityApproach GetPutOptionImpliedVolatilityApproach(out double tolerance, out Func<ConstantVolatilityStandardEuropeanOptionConfiguration, double, ToleranceLevel> testLevel);

        /// <summary>Serves as unit test for <see cref="BlackEuropeanStraddle.IImpliedVolatilityApproach.TryGetValue(double,double,double,double, out double)"/>.</summary>
        /// <param name="optionSpecification">The specification of the option.</param>
        /// <param name="optionValue">The value of the option.</param>
        [TestCaseSource(typeof(ImpliedBlackVolaTestCases), "StraddleOptions")]
        public void TryGetValue_StraddleTestCase_BenchmarkVolatility(ConstantVolatilityStandardEuropeanOptionConfiguration optionSpecification, double optionValue)
        {
            double tolerance;
            Func<ConstantVolatilityStandardEuropeanOptionConfiguration, double, ToleranceLevel> testLevel;

            var objectUnderTest = GetStraddleOptionImpliedVolatilityApproach(out tolerance, out testLevel);
            Assume.That(objectUnderTest, Is.Not.Null, "Object under test does not support the specific option type.");

            double actual;
            var actualState = objectUnderTest.TryGetValue(optionSpecification.Strike, optionSpecification.Forward, optionSpecification.TimeToExpiry, optionValue / optionSpecification.DiscountFactor, out actual);

            if (testLevel != null)
            {
                var toleranceLevel = testLevel(optionSpecification, optionValue);
                if (toleranceLevel == ToleranceLevel.IgnoreTestIfUnsuitableProblem)
                {
                    Assume.That(actualState, Is.EqualTo(ImpliedCalculationResultState.ProperResult) | Is.EqualTo(ImpliedCalculationResultState.InputError), "Algorithm does not support the implied volatility calculation of the specific option! The algorithm is somehow restricted!");
                }
            }
            Assert.That(actual, Is.EqualTo(optionSpecification.Volatility).Within(tolerance));
            Assert.That(actualState, Is.EqualTo(ImpliedCalculationResultState.ProperResult));
        }

        /// <summary>Gets the object under test.
        /// </summary>
        /// <param name="tolerance">The tolerance to take into account.</param>
        /// <param name="testLevel">A delegate that returns for a specific test scenario a specific <see cref="BlackImpliedVolatilityTests.ToleranceLevel"/> object. The second argument is the option value. Perhaps <c>null</c>.</param>
        /// <returns>The object under test.</returns>
        protected abstract BlackEuropeanStraddle.IImpliedVolatilityApproach GetStraddleOptionImpliedVolatilityApproach(out double tolerance, out Func<ConstantVolatilityStandardEuropeanOptionConfiguration, double, ToleranceLevel> testLevel);
    }
}