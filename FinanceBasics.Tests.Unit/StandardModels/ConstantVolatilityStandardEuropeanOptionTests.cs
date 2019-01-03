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

using NUnit.Framework;
using Dodoni.MathLibrary;

namespace Dodoni.Finance.StandardModels
{
    /// <summary>Serves as abstract unit test class for <see cref="IConstantVolatilityStandardEuropeanOption"/> objects.
    /// </summary>
    public abstract class ConstantVolatilityStandardEuropeanOptionTests
    {
        #region protected constructors

        /// <summary>Initializes a new instance of the <see cref="ConstantVolatilityStandardEuropeanOptionTests"/> class.
        /// </summary>
        protected ConstantVolatilityStandardEuropeanOptionTests()
        {
        }
        #endregion

        #region public unit test methods / test case data

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
        public virtual void GetOptionValue_TestCase(double strike, double forward, double timeToExpiry, double discountFactor, double volatiliy, double expectedOptionValue, double tolerance)
        {
            var option = CreateOption(strike, forward, timeToExpiry, discountFactor);

            double actualOptionValue = option.GetValue(volatiliy);

            Assert.That(actualOptionValue, Is.EqualTo(expectedOptionValue).Within(tolerance));
        }

        /// <summary>Serves as unit test for <see cref="IConstantVolatilityStandardOption.TryGetImpliedVolatility(double, out double)"/>. First we compute the option value
        /// with respect to a specific volatility, afterwards <see cref="IConstantVolatilityStandardOption.TryGetImpliedVolatility(double, out double)"/> will be called.
        /// </summary>
        /// <param name="optionConfiguration">The option configuration of the option to take into account for the unit test.</param>
        [TestCaseSource(nameof(TestCaseDataOptions))]
        public void TryGetImpliedVolatility_ComputedOptionValue_Volatility(ConstantVolatilityStandardEuropeanOptionConfiguration optionConfiguration)
        {
            var optionUnderTest = CreateOption(optionConfiguration.Strike, optionConfiguration.Forward, optionConfiguration.TimeToExpiry, optionConfiguration.DiscountFactor);

            double optionValue = optionUnderTest.GetValue(optionConfiguration.Volatility);
            Assume.That(IsVolatilityInvertibleOptionValue(optionValue, optionUnderTest) == true, String.Format("Option value {0} can not be inverted; intrinsic value {1}", optionValue, optionUnderTest.GetIntrinsicValue()));


            double actualImpliedVolatility;
            Assert.That(optionUnderTest.TryGetImpliedVolatility(optionValue, out actualImpliedVolatility), Is.EqualTo(ImpliedCalculationResultState.ProperResult), String.Format("Option value was: {0}, Intrinsic value: {1}.", optionValue, optionUnderTest.GetIntrinsicValue()));

            Assert.That(actualImpliedVolatility, Is.EqualTo(optionConfiguration.Volatility).Within(1E-8), String.Format("Option value was: {0}, Intrinsic value: {1}", optionValue, optionUnderTest.GetIntrinsicValue()));
        }

        /// <summary>Serves as unit test for <see cref="IConstantVolatilityStandardEuropeanOption.TryGetImpliedVolatilityOfNonDiscountedValue(double, out double)"/>. First we compute the undiscounted option value
        /// with respect to a specific volatility, afterwards <see cref="IConstantVolatilityStandardEuropeanOption.TryGetImpliedVolatilityOfNonDiscountedValue(double, out double)"/> will be called.
        /// </summary>
        /// <param name="optionConfiguration">The option configuration of the option to take into account for the unit test.</param>
        [TestCaseSource(nameof(TestCaseDataOptions))]
        public void TryGetImpliedVolatilityOfUndiscountedValue_ComputedOptionValue_Volatility(ConstantVolatilityStandardEuropeanOptionConfiguration optionConfiguration)
        {
            var optionUnderTest = CreateOption(optionConfiguration.Strike, optionConfiguration.Forward, optionConfiguration.TimeToExpiry, optionConfiguration.DiscountFactor);

            double undiscountedOptionValue = optionUnderTest.GetNoneDiscountedValue(optionConfiguration.Volatility);
            Assume.That(IsVolatilityInvertibleOptionValue(undiscountedOptionValue * optionUnderTest.DiscountFactor, optionUnderTest) == true, String.Format("Option value {0} can not be inverted; intrinsic value {1}", undiscountedOptionValue * optionUnderTest.DiscountFactor, optionUnderTest.GetIntrinsicValue()));


            double actualImpliedVolatility;
            Assert.That(optionUnderTest.TryGetImpliedVolatilityOfNonDiscountedValue(undiscountedOptionValue, out actualImpliedVolatility), Is.EqualTo(ImpliedCalculationResultState.ProperResult), String.Format("Undiscounted Option value was: {0}, undiscounted Intrinsic value: {1}", undiscountedOptionValue, optionUnderTest.GetIntrinsicValue() / optionUnderTest.DiscountFactor));

            Assert.That(actualImpliedVolatility, Is.EqualTo(optionConfiguration.Volatility).Within(1E-8), String.Format("Undiscounted Option value was: {0}, undiscounted Intrinsic value: {1}", undiscountedOptionValue, optionUnderTest.GetIntrinsicValue() / optionUnderTest.DiscountFactor));
        }

        /// <summary>Serves as unit test for <see cref="IConstantVolatilityStandardEuropeanOption.GetForwardDelta(double)"/>. The outcome
        /// is compared to a approximation based on a differential quotient.
        /// </summary>
        /// <param name="optionConfiguration">The option configuration of the option to take into account for the unit test.</param>
        [TestCaseSource(nameof(TestCaseDataOptions))]
        public void GetForwardDelta_TestCase_ViaDifferentialQuotientApproximation(ConstantVolatilityStandardEuropeanOptionConfiguration optionConfiguration)
        {
            var optionUnderTest = CreateOption(optionConfiguration.Strike, optionConfiguration.Forward, optionConfiguration.TimeToExpiry, optionConfiguration.DiscountFactor);

            double actualForwardDelta = optionUnderTest.GetForwardDelta(optionConfiguration.Volatility);

            double expectedForwardDelta = DifferentialQuotientGreekApproximation.GetOptionForwardDelta(optionConfiguration, (K, F, t, df, sigma) => CreateOption(K, F, t, df).GetValue(sigma));

            Assert.That(actualForwardDelta, Is.EqualTo(expectedForwardDelta).Within(1E-5), optionUnderTest.ToString());
        }

        /// <summary>Serves as unit test for <see cref="IConstantVolatilityStandardOption.GetTheta(double)"/>. The outcome
        /// is compared to a approximation based on a differential quotient.
        /// </summary>
        /// <param name="optionConfiguration">The option configuration of the option to take into account for the unit test.</param>
        [TestCaseSource(nameof(TestCaseDataOptions))]
        public void GetTheta_TestCase_ViaDifferentialQuotientApproximation(ConstantVolatilityStandardEuropeanOptionConfiguration optionConfiguration)
        {
            var optionUnderTest = CreateOption(optionConfiguration.Strike, optionConfiguration.Forward, optionConfiguration.TimeToExpiry, optionConfiguration.DiscountFactor);

            double actualTheta = optionUnderTest.GetTheta(optionConfiguration.Volatility);

            double expectedTheta = DifferentialQuotientGreekApproximation.GetOptionTheta(optionConfiguration, (K, F, t, df, sigma) => CreateOption(K, F, t, df).GetValue(sigma));

            Assert.That(actualTheta, Is.EqualTo(expectedTheta).Within(1E-5), optionUnderTest.ToString());
        }

        /// <summary>Serves as unit test for <see cref="IConstantVolatilityStandardEuropeanOption.GetStrikeDelta(double)"/>. The outcome
        /// is compared to a approximation based on a differential quotient.
        /// </summary>
        /// <param name="optionConfiguration">The option configuration of the option to take into account for the unit test.</param>
        [TestCaseSource(nameof(TestCaseDataOptions))]
        public void GetStrikeDelta_TestCase_ViaDifferentialQuotientApproximation(ConstantVolatilityStandardEuropeanOptionConfiguration optionConfiguration)
        {
            var optionUnderTest = CreateOption(optionConfiguration.Strike, optionConfiguration.Forward, optionConfiguration.TimeToExpiry, optionConfiguration.DiscountFactor);

            double actualStrikeDelta = optionUnderTest.GetStrikeDelta(optionConfiguration.Volatility);

            double expectedStrikeDelta = DifferentialQuotientGreekApproximation.GetOptionKappa(optionConfiguration, (K, F, t, df, sigma) => CreateOption(K, F, t, df).GetValue(sigma));

            Assert.That(actualStrikeDelta, Is.EqualTo(expectedStrikeDelta).Within(1E-5), optionUnderTest.ToString());
        }

        /// <summary>Serves as unit test for <see cref="IConstantVolatilityStandardEuropeanOption.GetForwardGamma(double)"/>. The outcome
        /// is compared to a approximation based on a differential quotient.
        /// </summary>
        /// <param name="optionConfiguration">The option configuration of the option to take into account for the unit test.</param>
        [TestCaseSource(nameof(TestCaseDataOptions))]
        public void GetForwardGamma_TestCase_ViaDifferentialQuotientApproximation(ConstantVolatilityStandardEuropeanOptionConfiguration optionConfiguration)
        {
            var optionUnderTest = CreateOption(optionConfiguration.Strike, optionConfiguration.Forward, optionConfiguration.TimeToExpiry, optionConfiguration.DiscountFactor);

            double actualForwardGamma = optionUnderTest.GetForwardGamma(optionConfiguration.Volatility);

            double expectedForwardGamma = DifferentialQuotientGreekApproximation.GetOptionForwardGamma(optionConfiguration, (K, F, t, df, sigma) => CreateOption(K, F, t, df).GetValue(sigma));

            Assert.That(actualForwardGamma, Is.EqualTo(expectedForwardGamma).Within(1E-4), String.Format("{0}; option value: {1}", optionUnderTest.ToString(), optionUnderTest.GetValue(optionConfiguration.Volatility)));
        }

        /// <summary>Serves as unit test for <see cref="IConstantVolatilityStandardOption.GetVega(double)"/>. The outcome
        /// is compared to a approximation based on a differential quotient.
        /// </summary>
        /// <param name="optionConfiguration">The option configuration of the option to take into account for the unit test.</param>
        [TestCaseSource(nameof(TestCaseDataOptions))]
        public void GetForwardVega_TestCase_ViaDifferentialQuotientApproximation(ConstantVolatilityStandardEuropeanOptionConfiguration optionConfiguration)
        {
            var optionUnderTest = CreateOption(optionConfiguration.Strike, optionConfiguration.Forward, optionConfiguration.TimeToExpiry, optionConfiguration.DiscountFactor);

            double actualVega = optionUnderTest.GetVega(optionConfiguration.Volatility);

            double expectedVega = DifferentialQuotientGreekApproximation.GetOptionVega(optionConfiguration, (K, F, t, df, sigma) => CreateOption(K, F, t, df).GetValue(sigma));

            Assert.That(actualVega, Is.EqualTo(expectedVega).Within(1E-5), optionUnderTest.ToString());
        }

        /// <summary>Serves as unit test for <see cref="IConstantVolatilityStandardOption.GetVolga(double)"/>. The outcome
        /// is compared to a approximation based on a differential quotient.
        /// </summary>
        /// <param name="optionConfiguration">The option configuration of the option to take into account for the unit test.</param>
        [TestCaseSource(nameof(TestCaseDataOptions))]
        public void GetForwardVolga_TestCase_ViaDifferentialQuotientApproximation(ConstantVolatilityStandardEuropeanOptionConfiguration optionConfiguration)
        {
            var optionUnderTest = CreateOption(optionConfiguration.Strike, optionConfiguration.Forward, optionConfiguration.TimeToExpiry, optionConfiguration.DiscountFactor);

            double actualVolga = optionUnderTest.GetVolga(optionConfiguration.Volatility);
            double expectedVolga = DifferentialQuotientGreekApproximation.GetOptionVolga(optionConfiguration, (K, F, t, df, sigma) => CreateOption(K, F, t, df).GetValue(sigma));

            Assert.That(actualVolga, Is.EqualTo(expectedVolga).Within(1E-4), optionUnderTest.ToString());
        }

        /// <summary>Serves as unit test for <see cref="IConstantVolatilityStandardEuropeanOption.GetForwardVanna(double)"/>. The outcome
        /// is compared to a approximation based on a differential quotient.
        /// </summary>
        /// <param name="optionConfiguration">The option configuration of the option to take into account for the unit test.</param>
        [TestCaseSource(nameof(TestCaseDataOptions))]
        public void GetForwardVanna_TestCase_ViaDifferentialQuotientApproximation(ConstantVolatilityStandardEuropeanOptionConfiguration optionConfiguration)
        {
            var optionUnderTest = CreateOption(optionConfiguration.Strike, optionConfiguration.Forward, optionConfiguration.TimeToExpiry, optionConfiguration.DiscountFactor);

            double actualForwardVanna = optionUnderTest.GetForwardVanna(optionConfiguration.Volatility);

            double expectedForwardVanna = DifferentialQuotientGreekApproximation.GetOptionForwardVanna(optionConfiguration, (K, F, t, df, sigma) => CreateOption(K, F, t, df).GetValue(sigma));

            Assert.That(actualForwardVanna, Is.EqualTo(expectedForwardVanna).Within(1E-4), optionUnderTest.ToString());
        }

        /// <summary>Gets the test case data, i.e. a collection of parameters of the options to take into account for the unit tests.
        /// </summary>
        /// <value>The test case data.</value>
        public  IEnumerable<TestCaseData> TestCaseDataOptions
        {
            get
            {
                foreach (var optionParameters in TestOptionParametersForApproximationTests)
                {
                    yield return new TestCaseData(optionParameters);
                }
            }
        }
        #endregion

        #region protected methods/properties

        /// <summary>Determines whether a volatility should be computable that implies a specified option value.
        /// </summary>
        /// <param name="optionValue">The option value.</param>
        /// <param name="option">The option.</param>
        /// <returns>
        /// 	<c>true</c> if there should be an implied volatility for the specified option value be available; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>The implied volatility of Far-out-of-the-money options can often not be computed. One can design the individual test cases or one may override this
        /// method in an individual manner to avoid questionable results in the unit tests.</remarks>
        protected virtual bool IsVolatilityInvertibleOptionValue(double optionValue, IConstantVolatilityStandardEuropeanOption option)
        {
            return (optionValue > MachineConsts.SuperTinyEpsilon);
        }

        /// <summary>Gets a collection of parameters for the options to take into account for the unit tests, for example for the calculation of the greeks 
        /// based on an approximation by differential quotients etc.
        /// </summary>
        /// <value>The collection of parameters of the options to take into account for the unit tests.</value>
        protected abstract IEnumerable<ConstantVolatilityStandardEuropeanOptionConfiguration> TestOptionParametersForApproximationTests
        {
            get;
        }

        /// <summary>Creates a specific <see cref="IConstantVolatilityStandardEuropeanOption"/> option.
        /// </summary>
        /// <param name="strike">The strike.</param>
        /// <param name="forward">The forward.</param>
        /// <param name="timeToExpiry">The time to expiry.</param>
        /// <param name="discountFactor">The discount factor.</param>
        /// <returns>A <see cref="IConstantVolatilityStandardEuropeanOption"/> object with the desired parameters.</returns>
        protected abstract IConstantVolatilityStandardEuropeanOption CreateOption(double strike, double forward, double timeToExpiry, double discountFactor);
        #endregion
    }
}