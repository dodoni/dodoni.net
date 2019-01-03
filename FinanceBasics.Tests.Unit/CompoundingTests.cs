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
using System.Linq;
using System.Collections.Generic;

using NUnit.Framework;
using Dodoni.Finance.Compoundings;

namespace Dodoni.Finance
{
    /// <summary>Serves as class for unit tests with respect to <see cref="Compounding"/>.
    /// </summary>
    [TestFixture]
    public class CompoundingTests
    {
        #region Zero rate convertion tests

        /// <summary>Serves as unit test for <see cref="Compounding.GetConvertedZeroRate(double, IZeroRateCompounding, IZeroRateCompounding, double)"/>. The input is output of 
        /// <see cref="Compounding.GetConvertedZeroRate(double, IZeroRateCompounding, IZeroRateCompounding, double)"/> as well, but the zero rate conversion has been done in the other direction.
        /// </summary>
        [TestCaseSource(nameof(ZeroRateTestCaseDataDestinationSourceZeroRates))]
        public void GetConvertedZeroRate_DestinationZeroRate_SourceZeroRate(double zeroRate, IZeroRateCompounding sourceCompounding, IZeroRateCompounding destinationCompounding, double timeToMaturity, double expectedZeroRate)
        {
            double actualZeroRate = Compounding.GetConvertedZeroRate(zeroRate, sourceCompounding, destinationCompounding, timeToMaturity);

            Assert.That(actualZeroRate, Is.EqualTo(expectedZeroRate).Within(1E-8));
        }

        /// <summary>Gets the test data for <see cref="GetConvertedZeroRate_DestinationZeroRate_SourceZeroRate(double, IZeroRateCompounding, IZeroRateCompounding, double, double)"/>,
        /// i.e. converts zero rates from one compounding into another and return the source and destination compounding in reverse order.
        /// </summary>
        /// <value>Test data for <see cref="GetConvertedZeroRate_DestinationZeroRate_SourceZeroRate(double, IZeroRateCompounding, IZeroRateCompounding, double, double)"/>.</value>
        public static IEnumerable<TestCaseData> ZeroRateTestCaseDataDestinationSourceZeroRates
        {
            get
            {
                double[] zeroRateInputs = new double[] { 0.05, 0.12 };
                double[] timetoMaturities = new double[] { 2.12, 23.75 }; // the tests should be independent from the time-to-maturity

                foreach (double sourceZeroRate in zeroRateInputs)
                {
                    foreach (IZeroRateCompounding sourceCompounding in Compounding.ZeroRate)
                    {
                        foreach (IZeroRateCompounding destinationCompounding in Compounding.ZeroRate)
                        {
                            foreach (double timeToMaturity in timetoMaturities)
                            {
                                double destinationZeroRate = Compounding.GetConvertedZeroRate(sourceZeroRate, sourceCompounding, destinationCompounding);

                                yield return new TestCaseData(destinationZeroRate, destinationCompounding, sourceCompounding, timeToMaturity, sourceZeroRate);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>Serves as unit test for <see cref="Compounding.GetConvertedZeroRate(double, ContinuouslyZeroRateCompounding, PeriodicZeroRateCompounding, double)"/>.
        /// The output will be checked against <see cref="Compounding.GetConvertedZeroRate(double, IZeroRateCompounding, IZeroRateCompounding, double)"/> with the same arguments.
        /// </summary>
        /// <param name="zeroRate">The input zero rate.</param>
        /// <param name="timeToMaturity">The time to maturity.</param>
        [TestCase(0.074, 3.753)]
        [TestCase(0.13, 7.32)]
        public void GetConvertedZeroRateExplizit_ContinouslyZeroRate_SemiAnnuallyZeroRate(double zeroRate, double timeToMaturity)
        {
            double actual = Compounding.GetConvertedZeroRate(zeroRate, Compounding.ZeroRate.Continuously, Compounding.ZeroRate.SemiAnnually, timeToMaturity);

            double expected = Compounding.GetConvertedZeroRate(zeroRate, (IZeroRateCompounding)Compounding.ZeroRate.Continuously, (IZeroRateCompounding)Compounding.ZeroRate.SemiAnnually, timeToMaturity);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-9));
        }

        /// <summary>Serves as unit test for <see cref="Compounding.GetConvertedZeroRate(double, PeriodicZeroRateCompounding, ContinuouslyZeroRateCompounding, double)"/>.
        /// The output will be checked against <see cref="Compounding.GetConvertedZeroRate(double, IZeroRateCompounding, IZeroRateCompounding, double)"/> with the same arguments.
        /// </summary>
        /// <param name="zeroRate">The input zero rate.</param>
        /// <param name="timeToMaturity">The time to maturity.</param>
        [TestCase(0.074, 3.753)]
        [TestCase(0.13, 7.32)]
        public void GetConvertedZeroRateExplizit_QuarterlyCompoundingZeroRate_ContinuouslyZeroRate(double zeroRate, double timeToMaturity)
        {
            double actual = Compounding.GetConvertedZeroRate(zeroRate, Compounding.ZeroRate.Quarterly, Compounding.ZeroRate.Continuously, timeToMaturity);

            double expected = Compounding.GetConvertedZeroRate(zeroRate, (IZeroRateCompounding)Compounding.ZeroRate.Quarterly, (IZeroRateCompounding)Compounding.ZeroRate.Continuously, timeToMaturity);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-9));
        }
        #endregion

        #region Interest rate conversion tests

        /// <summary>Serves as unit test for <see cref="Compounding.GetConvertedInterestRate(double, IInterestRateCompounding, IInterestRateCompounding, double)"/>. The input is output of 
        /// <see cref="Compounding.GetConvertedInterestRate(double, IInterestRateCompounding, IInterestRateCompounding, double)"/> as well, but the interest rate conversion has been done in the other direction.
        /// </summary>
        [TestCaseSource(nameof(InterestRateTestCaseDataDestinationSourceZeroRates))]
        public void GetConvertedInterestRate_DestinationInterestRate_SourceInterestRate(double interestRate, IInterestRateCompounding sourceInterestRateCompounding, IInterestRateCompounding destinationInterestRateCompounding, double interestPeriodLength, double expectedInterestRate)
        {
            double actualInterestRate = Compounding.GetConvertedInterestRate(interestRate, sourceInterestRateCompounding, destinationInterestRateCompounding, interestPeriodLength);

            Assert.That(actualInterestRate, Is.EqualTo(expectedInterestRate).Within(1E-8));
        }

        /// <summary>Gets the test data for <see cref="GetConvertedInterestRate_DestinationInterestRate_SourceInterestRate(double, IInterestRateCompounding, IInterestRateCompounding, double, double)"/>,
        /// i.e. converts interest rate from one compounding into another and return the source and destination compounding in reverse order.
        /// </summary>
        /// <value>Test data for <see cref="GetConvertedInterestRate_DestinationInterestRate_SourceInterestRate(double, IInterestRateCompounding, IInterestRateCompounding, double, double)"/>.</value>
        public static IEnumerable<TestCaseData> InterestRateTestCaseDataDestinationSourceZeroRates
        {
            get
            {
                double[] interestRates = new double[] { 0.016, 0.085 };
                double[] periodLengths = new double[] { 0.65, 12.75 };

                foreach (double sourceInterestRate in interestRates)
                {
                    foreach (IInterestRateCompounding sourceCompounding in Compounding.InterestRate)
                    {
                        foreach (IInterestRateCompounding destinationCompounding in Compounding.InterestRate)
                        {
                            foreach (double periodLength in periodLengths)
                            {
                                double destinationInterestRate = Compounding.GetConvertedInterestRate(sourceInterestRate, sourceCompounding, destinationCompounding, periodLength);

                                yield return new TestCaseData(destinationInterestRate, destinationCompounding, sourceCompounding, periodLength, sourceInterestRate);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>Serves as unit test for <see cref="Compounding.GetConvertedInterestRate(double, ContinuouslyInterestCompounding, PeriodicInterestCompounding, double)"/>.
        /// The output will be checked against <see cref="Compounding.GetConvertedInterestRate(double, IInterestRateCompounding, IInterestRateCompounding, double)"/> with the same arguments.
        /// </summary>
        /// <param name="interestRate">The input interest rate.</param>
        /// <param name="interestPeriodLength">The length of the interest period.</param>
        [TestCase(0.028, 5.142)]
        [TestCase(0.081, 0.591)]
        public void GetConvertedInterestRateExplizit_ContinuouslyCompoundingInterestRate_BiMonthlyCompoundedInterestRate(double interestRate, double interestPeriodLength)
        {
            double actual = Compounding.GetConvertedInterestRate(interestRate, Compounding.InterestRate.Continuously, Compounding.InterestRate.BiMonthly, interestPeriodLength);

            double expected = Compounding.GetConvertedInterestRate(interestRate, (IInterestRateCompounding)Compounding.InterestRate.Continuously, (IInterestRateCompounding) Compounding.InterestRate.BiMonthly, interestPeriodLength);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-9));
        }

        /// <summary>Serves as unit test for <see cref="Compounding.GetConvertedInterestRate(double, ContinuouslyInterestCompounding, SimpleInterestCompounding, double)"/>.
        /// The output will be checked against <see cref="Compounding.GetConvertedInterestRate(double, IInterestRateCompounding, IInterestRateCompounding, double)"/> with the same arguments.
        /// </summary>
        /// <param name="interestRate">The input interest rate.</param>
        /// <param name="interestPeriodLength">The length of the interest period.</param>
        [TestCase(0.0714, 2.426)]
        [TestCase(0.142, 8.821)]
        public void GetConvertedInterestRateExplizit_ContinuouslyCompoundingInterestRate_SimpleCompoundedInterestRate(double interestRate, double interestPeriodLength)
        {
            double actual = Compounding.GetConvertedInterestRate(interestRate, Compounding.InterestRate.Continuously, Compounding.InterestRate.Simple, interestPeriodLength);

            double expected = Compounding.GetConvertedInterestRate(interestRate, (IInterestRateCompounding)Compounding.InterestRate.Continuously, (IInterestRateCompounding)Compounding.InterestRate.Simple, interestPeriodLength);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-9));
        }

        /// <summary>Serves as unit test for <see cref="Compounding.GetConvertedInterestRate(double, PeriodicInterestCompounding, ContinuouslyInterestCompounding, double)"/>.
        /// The output will be checked against <see cref="Compounding.GetConvertedInterestRate(double, IInterestRateCompounding, IInterestRateCompounding, double)"/> with the same arguments.
        /// </summary>
        /// <param name="interestRate">The input interest rate.</param>
        /// <param name="interestPeriodLength">The length of the interest period.</param>
        [TestCase(0.0241, 14.618)]
        [TestCase(0.192, 6.191)]
        public void GetConvertedInterestRateExplizit_QuarterlyCompoundingInterestRate_ContinuouslyCompoundedInterestRate(double interestRate, double interestPeriodLength)
        {
            double actual = Compounding.GetConvertedInterestRate(interestRate, Compounding.InterestRate.Quarterly, Compounding.InterestRate.Continuously, interestPeriodLength);

            double expected = Compounding.GetConvertedInterestRate(interestRate, (IInterestRateCompounding)Compounding.InterestRate.Quarterly, (IInterestRateCompounding)Compounding.InterestRate.Continuously, interestPeriodLength);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-9));
        }

        /// <summary>Serves as unit test for <see cref="Compounding.GetConvertedInterestRate(double, PeriodicInterestCompounding, SimpleInterestCompounding, double)"/>.
        /// The output will be checked against <see cref="Compounding.GetConvertedInterestRate(double, IInterestRateCompounding, IInterestRateCompounding, double)"/> with the same arguments.
        /// </summary>
        /// <param name="interestRate">The input interest rate.</param>
        /// <param name="interestPeriodLength">The length of the interest period.</param>
        [TestCase(0.0387, 7.914)]
        [TestCase(0.091, 3.716)]
        public void GetConvertedInterestRateExplizit_QuarterlyCompoundingInterestRate_SimpleCompoundedInterestRate(double interestRate, double interestPeriodLength)
        {
            double actual = Compounding.GetConvertedInterestRate(interestRate, Compounding.InterestRate.Quarterly, Compounding.InterestRate.Simple, interestPeriodLength);

            double expected = Compounding.GetConvertedInterestRate(interestRate, (IInterestRateCompounding)Compounding.InterestRate.Quarterly, (IInterestRateCompounding)Compounding.InterestRate.Simple, interestPeriodLength);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-9));
        }

        /// <summary>Serves as unit test for <see cref="Compounding.GetConvertedInterestRate(double, SimpleInterestCompounding, ContinuouslyInterestCompounding, double)"/>.
        /// The output will be checked against <see cref="Compounding.GetConvertedInterestRate(double, IInterestRateCompounding, IInterestRateCompounding, double)"/> with the same arguments.
        /// </summary>
        /// <param name="interestRate">The input interest rate.</param>
        /// <param name="interestPeriodLength">The length of the interest period.</param>
        [TestCase(0.0281, 2.716)]
        [TestCase(0.0751, 12.817)]
        public void GetConvertedInterestRateExplizit_SimpleCompoundingInterestRate_ContinuouslyCompoundedInterestRate(double interestRate, double interestPeriodLength)
        {
            double actual = Compounding.GetConvertedInterestRate(interestRate, Compounding.InterestRate.Simple, Compounding.InterestRate.Continuously, interestPeriodLength);

            double expected = Compounding.GetConvertedInterestRate(interestRate, (IInterestRateCompounding)Compounding.InterestRate.Simple, (IInterestRateCompounding)Compounding.InterestRate.Continuously, interestPeriodLength);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-9));
        }

        /// <summary>Serves as unit test for <see cref="Compounding.GetConvertedInterestRate(double, SimpleInterestCompounding, PeriodicInterestCompounding, double)"/>.
        /// The output will be checked against <see cref="Compounding.GetConvertedInterestRate(double, IInterestRateCompounding, IInterestRateCompounding, double)"/> with the same arguments.
        /// </summary>
        /// <param name="interestRate">The input interest rate.</param>
        /// <param name="interestPeriodLength">The length of the interest period.</param>
        [TestCase(0.0187, 4.6151)]
        [TestCase(0.0661, 26.917)]
        public void GetConvertedInterestRateExplizit_SimpleCompoundingInterestRate_MonthlyCompoundedInterestRate(double interestRate, double interestPeriodLength)
        {
            double actual = Compounding.GetConvertedInterestRate(interestRate, Compounding.InterestRate.Simple, Compounding.InterestRate.Monthly, interestPeriodLength);

            double expected = Compounding.GetConvertedInterestRate(interestRate, (IInterestRateCompounding)Compounding.InterestRate.Simple, (IInterestRateCompounding)Compounding.InterestRate.Monthly, interestPeriodLength);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-9));
        }
        #endregion
    }
}