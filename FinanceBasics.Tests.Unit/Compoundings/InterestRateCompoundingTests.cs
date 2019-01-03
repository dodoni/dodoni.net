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

namespace Dodoni.Finance.Compoundings
{
    /// <summary>Serves as class for unit tests with respect to <see cref="IInterestRateCompounding"/>.
    /// </summary>
    [TestFixture]
    public class InterestRateCompoundingTests
    {
        /// <summary>Serves as unit test for <see cref="IInterestRateCompounding.GetInterestAmount(double, double)"/>. The input is output of
        /// <see cref="IInterestRateCompounding.GetImpliedInterestRate(double, double)"/>, therefore this test checks whether the execution of one method after the
        /// other returns the original input values.
        /// </summary>
        /// <param name="interestRate">The interest rate.</param>
        /// <param name="interestRateCompounding">The interest rate compounding.</param>
        /// <param name="interestPeriodLength">The length of the interest period.</param>
        /// <param name="expectedInterestAmount">The expected interest amount.</param>
        [TestCaseSource(nameof(TestCaseDataDestinationInterestRateSourceNormalizedInterest))]
        public void GetNormalizedInterest_destinationInterestRate_sourceNormalizedInterest(double interestRate, IInterestRateCompounding interestRateCompounding, double interestPeriodLength, double expectedNormalizedInterest)
        {
            double actualInterestAmount = interestRateCompounding.GetInterestAmount(interestRate, interestPeriodLength);

            Assert.That(actualInterestAmount, Is.EqualTo(expectedNormalizedInterest).Within(1E-8));
        }

        /// <summary>Gets the test data for <see cref="GetNormalizedInterest_destinationInterestRate_sourceNormalizedInterest(double, IInterestRateCompounding, double, double)"/>.
        /// </summary>
        /// <value>Test data for <see cref="GetNormalizedInterest_destinationInterestRate_sourceNormalizedInterest(double, IInterestRateCompounding, double, double)"/>.</value>
        public static IEnumerable<TestCaseData> TestCaseDataDestinationInterestRateSourceNormalizedInterest
        {
            get
            {
                double[] interestAmounts = new double[] { 1.0, 0.65, 0.28, 1.15 };
                double[] interestPeriodLengths = new double[] { 2.12, 23.75 };

                foreach (double interestAmount in interestAmounts)
                {
                    foreach (IInterestRateCompounding interestRateCompounding in Compounding.InterestRate)
                    {
                        foreach (double interestPeriodLength in interestPeriodLengths)
                        {
                            double destinationInterestRate = interestRateCompounding.GetImpliedInterestRate(interestAmount, interestPeriodLength);

                            yield return new TestCaseData(destinationInterestRate, interestRateCompounding, interestPeriodLength, interestAmount);
                        }
                    }
                }
            }
        }

        /// <summary>Serves as unit test for <see cref="IInterestRateCompounding.GetImpliedInterestRate(double, double)"/>. The input is output of
        /// <see cref="IInterestRateCompounding.GetInterestAmount(double, double)"/>, therefore this test checks whether the execution of one method after the
        /// other returns the original input values.
        /// </summary>
        /// <param name="interestAmount">The normalized interest.</param>
        /// <param name="interestRateCompounding">The interest rate compounding.</param>
        /// <param name="interestPeriodLength">The length of the interest period.</param>
        /// <param name="expectedInterestRate">The expected interest rate.</param>
        [TestCaseSource(nameof(TestCaseDataDestinationNormalizedInterestSourceInterestRate))]
        public void TryGetImpliedInterestRate_destinationNormalizedInterest_sourceInterestRate(double normalizedInterest, IInterestRateCompounding interestRateCompounding, double interestPeriodLength, double expectedInterestRate)
        {
            double actualInterestRate = interestRateCompounding.GetImpliedInterestRate(normalizedInterest, interestPeriodLength);

            Assert.That(actualInterestRate, Is.EqualTo(expectedInterestRate).Within(1E-8));
        }

        /// <summary>Gets the test data for <see cref="TryGetImpliedInterestRate_destinationNormalizedInterest_sourceInterestRate(double, IInterestRateCompounding, double, double)"/>.
        /// </summary>
        /// <value>Test data for <see cref="TryGetImpliedInterestRate_destinationNormalizedInterest_sourceInterestRate(double, IInterestRateCompounding, double, double)"/>.</value>
        public static IEnumerable<TestCaseData> TestCaseDataDestinationNormalizedInterestSourceInterestRate
        {
            get
            {
                double[] interestRates = new double[] { 0.025, 0.08 };
                double[] interestPeriodLengths = new double[] { 1.52, 14.8 };

                foreach (double sourceInterestRate in interestRates)
                {
                    foreach (IInterestRateCompounding interestRateCompounding in Compounding.InterestRate)
                    {
                        foreach (double interestPeriodLength in interestPeriodLengths)
                        {
                            double destinationNormalizedInterest = interestRateCompounding.GetInterestAmount(sourceInterestRate, interestPeriodLength);

                            yield return new TestCaseData(destinationNormalizedInterest, interestRateCompounding, interestPeriodLength, sourceInterestRate);
                        }
                    }
                }
            }
        }

        /// <summary>Serves as unit test for <see cref="IInterestRateCompounding.GetInterestAmount(double,double)"/>.
        /// </summary>
        /// <param name="interestRate">The interest rate.</param>
        /// <param name="interestRateCompounding">The interest rate compounding.</param>
        /// <param name="interestPeriodLength">The length of the interest period.</param>
        /// <param name="expectedInterestAmount">The expected interest amount.</param>
        [TestCaseSource(nameof(TestCaseDataInterestRateExamples))]
        public void GetNormalizedInterest_TestCase(double interestRate, IInterestRateCompounding interestRateCompounding, double interestPeriodLength, double expectedInterestAmount)
        {
            double actualInterestAmount = interestRateCompounding.GetInterestAmount(interestRate, interestPeriodLength);

            Assert.That(actualInterestAmount, Is.EqualTo(expectedInterestAmount).Within(1E-8));
        }

        /// <summary>Gets the test data for <see cref="GetNormalizedInterest_TestCase(double, IInterestRateCompounding, double, double)"/>.
        /// </summary>
        /// <value>The test data for <see cref="GetNormalizedInterest_TestCase(double, IInterestRateCompounding, double, double)"/>.</value>
        public static IEnumerable<TestCaseData> TestCaseDataInterestRateExamples
        {
            get
            {
                double continouslyInterestRate = 0.032;
                double continouslyInterestPeriodLength = 2.85;
                double continouslyExpectedNormalizedInterest = Math.Exp(continouslyInterestPeriodLength * continouslyInterestRate);
                yield return new TestCaseData(continouslyInterestRate, Compounding.InterestRate.Continuously, continouslyInterestPeriodLength, continouslyExpectedNormalizedInterest);

                double annuallyInterestRate = 0.041;
                double annuallyInterestPeriodLength = 2.77;
                double annuallyExpectedNormalizedInterest = Math.Pow(1 + annuallyInterestRate, annuallyInterestPeriodLength);
                yield return new TestCaseData(annuallyInterestRate, Compounding.InterestRate.Annually, annuallyInterestPeriodLength, annuallyExpectedNormalizedInterest);

                double semiAnnuallyInterestRate = 0.0146;
                double semiAnuallyInterestPeriodLength = 11.52;
                double semiAnnuallyExpectedNormalizedInterest = Math.Pow(1 + semiAnnuallyInterestRate / 2.0, semiAnuallyInterestPeriodLength * 2.0);
                yield return new TestCaseData(semiAnnuallyInterestRate, Compounding.InterestRate.SemiAnnually, semiAnuallyInterestPeriodLength, semiAnnuallyExpectedNormalizedInterest);

                double quarterlyInterestRate = 0.064;
                double quarterlyInterestPeriodLength = 1.92;
                double quarterlyExpectedNormalizedInterest = Math.Pow(1 + quarterlyInterestRate / 4.0, quarterlyInterestPeriodLength * 4.0);
                yield return new TestCaseData(quarterlyInterestRate, Compounding.InterestRate.Quarterly, quarterlyInterestPeriodLength, quarterlyExpectedNormalizedInterest);

                double biMonthlyInterestRate = 0.16;
                double biMonthlyInterestPeriodLength = 4.13;
                double biMonthlyExpectedNormalizedInterest = Math.Pow(1 + biMonthlyInterestRate / 6.0, biMonthlyInterestPeriodLength * 6.0);
                yield return new TestCaseData(biMonthlyInterestRate, Compounding.InterestRate.BiMonthly, biMonthlyInterestPeriodLength, biMonthlyExpectedNormalizedInterest);

                double monthlyInterestRate = 0.075;
                double monthlyInterestPeriodLength = 2.78;
                double monthlyExpectedNormalizedInterest = Math.Pow(1 + monthlyInterestRate / 12.0, monthlyInterestPeriodLength * 12.0);
                yield return new TestCaseData(monthlyInterestRate, Compounding.InterestRate.Monthly, monthlyInterestPeriodLength, monthlyExpectedNormalizedInterest);

                double dailyInterestRate = 0.027;
                double dailyInterestPeriodLength = 20.246;
                double dailyExpectedNormalizedInterest = Math.Pow(1 + dailyInterestRate / 365.0, dailyInterestPeriodLength * 365.0);
                yield return new TestCaseData(dailyInterestRate, Compounding.InterestRate.Daily, dailyInterestPeriodLength, dailyExpectedNormalizedInterest);
            }
        }

    }
}