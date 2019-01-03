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
    /// <summary>Serves as class for unit tests with respect to <see cref="IZeroRateCompounding"/>.
    /// </summary>
    [TestFixture]
    public class ZeroRateCompoundingTests
    {
        /// <summary>Serves as unit test for <see cref="IZeroRateCompounding.GetZeroCouponBondPrice(double, double)"/>. The input is output of
        /// <see cref="IZeroRateCompounding.GetZeroRate(double, double)"/>, therefore this test checks whether the execution of one method after the
        /// other returns the original input values.
        /// </summary>
        /// <param name="zeroRate">The zero rate.</param>
        /// <param name="zeroRateCompounding">The zero rate compounding.</param>
        /// <param name="timeToMaturity">The time-to-maturity.</param>
        /// <param name="expectedZeroCouponBondPrice">The expected zero coupon bond price.</param>
        [TestCaseSource(nameof(TestCaseDataDestinationZeroRateSourceZeroCouponBondPrice))]
        public void GetConvertedZeroRate_destinationZeroRate_sourceZeroCouponBondPrice(double zeroRate, IZeroRateCompounding zeroRateCompounding, double timeToMaturity, double expectedZeroCouponBondPrice)
        {
            double actualZeroCouponBondPrice = zeroRateCompounding.GetZeroCouponBondPrice(zeroRate, timeToMaturity);

            Assert.That(actualZeroCouponBondPrice, Is.EqualTo(expectedZeroCouponBondPrice).Within(1E-8));
        }

        /// <summary>Gets the test data for <see cref="GetConvertedZeroRate_destinationZeroRate_sourceZeroCouponBondPrice(double, IZeroRateCompounding, double, double)"/>.
        /// </summary>
        /// <value>Test data for <see cref="GetConvertedZeroRate_destinationZeroRate_sourceZeroCouponBondPrice(double, IZeroRateCompounding, double, double)"/>.</value>
        public static IEnumerable<TestCaseData> TestCaseDataDestinationZeroRateSourceZeroCouponBondPrice
        {
            get
            {
                double[] zeroCouponBondPrices = new double[] { 1.0, 0.65, 0.28, 1.15 };  // we allow P(t,T) > 1 as well! (for example inflation)
                double[] timetoMaturities = new double[] { 2.12, 23.75 }; // the tests should be independent from the time-to-maturity

                foreach (double sourceZeroCouponBondPrice in zeroCouponBondPrices)
                {
                    foreach (IZeroRateCompounding zeroRateCompounding in Compounding.ZeroRate)
                    {
                        foreach (double timeToMaturity in timetoMaturities)
                        {
                            double destinationZeroRate = zeroRateCompounding.GetZeroRate(sourceZeroCouponBondPrice, timeToMaturity);

                            yield return new TestCaseData(destinationZeroRate, zeroRateCompounding, timeToMaturity, sourceZeroCouponBondPrice);
                        }
                    }
                }
            }
        }

        /// <summary>Serves as unit test for <see cref="IZeroRateCompounding.GetZeroRate(double, double)"/>. The input is output of
        /// <see cref="IZeroRateCompounding.GetZeroCouponBondPrice(double, double)"/>, therefore this test checks whether the execution of one method after the
        /// other returns the original input values.
        /// </summary>
        /// <param name="zeroCouponBondPrice">The zero coupon bond price.</param>
        /// <param name="zeroRateCompounding">The zero rate compounding.</param>
        /// <param name="timeToMaturity">The time-to-maturity.</param>
        /// <param name="expectedZeroRate">The expected zero rate.</param>
        [TestCaseSource(nameof(TestCaseDataDestinationZeroCouponBondPriceSourceZeroRate))]
        public void GetConvertedZeroRate_destinationZeroCouponPrice_sourceZeroRate(double zeroCouponBondPrice, IZeroRateCompounding zeroRateCompounding, double timeToMaturity, double expectedZeroRate)
        {
            double actualZeroRate = zeroRateCompounding.GetZeroRate(zeroCouponBondPrice, timeToMaturity);

            Assert.That(actualZeroRate, Is.EqualTo(expectedZeroRate).Within(1E-8));
        }

        /// <summary>Gets the test data for <see cref="GetConvertedZeroRate_destinationZeroCouponPrice_sourceZeroRate(double, IZeroRateCompounding, double, double)"/>.
        /// </summary>
        /// <value>Test data for <see cref="GetConvertedZeroRate_destinationZeroCouponPrice_sourceZeroRate(double, IZeroRateCompounding, double, double)"/>.</value>
        public static IEnumerable<TestCaseData> TestCaseDataDestinationZeroCouponBondPriceSourceZeroRate
        {
            get
            {
                double[] zeroRates = new double[] { 0.025, 0.08 };
                double[] timetoMaturities = new double[] { 1.52, 14.8 }; // the tests should be independent from the time-to-maturity

                foreach (double sourceZeroRate in zeroRates)
                {
                    foreach (IZeroRateCompounding zeroRateCompounding in Compounding.ZeroRate)
                    {
                        foreach (double timeToMaturity in timetoMaturities)
                        {
                            double destinationZeroCouponBondPrice = zeroRateCompounding.GetZeroCouponBondPrice(sourceZeroRate, timeToMaturity);

                            yield return new TestCaseData(destinationZeroCouponBondPrice, zeroRateCompounding, timeToMaturity, sourceZeroRate);
                        }
                    }
                }
            }
        }

        /// <summary>Serves as unit test for <see cref="IZeroRateCompounding.GetZeroCouponBondPrice(double,double)"/>.
        /// </summary>
        /// <param name="zeroRate">The zero rate.</param>
        /// <param name="zeroRateCompounding">The zero rate compounding.</param>
        /// <param name="timeToMaturity">The time-to-maturity.</param>
        /// <param name="expectedZeroCouponBondPrice">The expected zero coupon bond price.</param>
        [TestCaseSource(nameof(TestCaseDataZeroRateExamples))]
        public void GetZeroCouponBondPrice_TestCase(double zeroRate, IZeroRateCompounding zeroRateCompounding, double timeToMaturity, double expectedZeroCouponBondPrice)
        {
            double actualZeroCouponBond = zeroRateCompounding.GetZeroCouponBondPrice(zeroRate, timeToMaturity);

            Assert.That(actualZeroCouponBond, Is.EqualTo(expectedZeroCouponBondPrice).Within(1E-8));
        }

        /// <summary>Gets the test data for <see cref="GetZeroCouponBondPrice_TestCase(double, IZeroRateCompounding, double, double)"/>.
        /// </summary>
        /// <value>The test data for <see cref="GetZeroCouponBondPrice_TestCase(double, IZeroRateCompounding, double, double)"/>.</value>
        public static IEnumerable<TestCaseData> TestCaseDataZeroRateExamples
        {
            get
            {
                double continouslyZeroRate = 0.035;
                double continouslyTimeToMaturity = 2.65;
                double continouslyExpectedZeroCouponBondPrice = Math.Exp(-continouslyTimeToMaturity * continouslyZeroRate);
                yield return new TestCaseData(continouslyZeroRate, Compounding.ZeroRate.Continuously, continouslyTimeToMaturity, continouslyExpectedZeroCouponBondPrice);

                double annuallyZeroRate = 0.054;
                double annuallyTimeToMaturity = 4.67;
                double annuallyExpectedZeroCouponBondPrice = Math.Pow(1 + annuallyZeroRate, -annuallyTimeToMaturity);
                yield return new TestCaseData(annuallyZeroRate, Compounding.ZeroRate.Annually, annuallyTimeToMaturity, annuallyExpectedZeroCouponBondPrice);

                double semiAnnuallyZeroRate = 0.0176;
                double semiAnuallyTimeToMaturity = 12.82;
                double semiAnnuallyExpectedZeroCouponBondPrice = Math.Pow(1 + semiAnnuallyZeroRate / 2.0, -semiAnuallyTimeToMaturity * 2.0);
                yield return new TestCaseData(semiAnnuallyZeroRate, Compounding.ZeroRate.SemiAnnually, semiAnuallyTimeToMaturity, semiAnnuallyExpectedZeroCouponBondPrice);

                double quarterlyZeroRate = 0.064;
                double quarterlyTimeToMaturity = 1.42;
                double quarterlyExpectedZeroCouponBondPrice = Math.Pow(1 + quarterlyZeroRate / 4.0, -quarterlyTimeToMaturity * 4.0);
                yield return new TestCaseData(quarterlyZeroRate, Compounding.ZeroRate.Quarterly, quarterlyTimeToMaturity, quarterlyExpectedZeroCouponBondPrice);

                double biMonthlyZeroRate = 0.16;
                double biMonthlyTimeToMaturity = 4.13;
                double biMonthlyExpectedZeroCouponBondPrice = Math.Pow(1 + biMonthlyZeroRate / 6.0, -biMonthlyTimeToMaturity * 6.0);
                yield return new TestCaseData(biMonthlyZeroRate, Compounding.ZeroRate.BiMonthly, biMonthlyTimeToMaturity, biMonthlyExpectedZeroCouponBondPrice);

                double monthlyZeroRate = 0.075;
                double monthlyTimeToMaturity = 2.78;
                double monthlyExpectedZeroCouponBondPrice = Math.Pow(1 + monthlyZeroRate / 12.0, -monthlyTimeToMaturity * 12.0);
                yield return new TestCaseData(monthlyZeroRate, Compounding.ZeroRate.Monthly, monthlyTimeToMaturity, monthlyExpectedZeroCouponBondPrice);

                double dailyZeroRate = 0.027;
                double dailyTimeToMaturity = 20.246;
                double dailyExpectedZeroCouponBondPrice = Math.Pow(1 + dailyZeroRate / 365.0, -dailyTimeToMaturity * 365.0);
                yield return new TestCaseData(dailyZeroRate, Compounding.ZeroRate.Daily, dailyTimeToMaturity, dailyExpectedZeroCouponBondPrice);
            }
        }
    }
}