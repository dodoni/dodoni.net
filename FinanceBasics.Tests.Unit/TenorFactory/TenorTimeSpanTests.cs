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

namespace Dodoni.Finance.TenorFactory
{
    /// <summary>Serves as class for unit tests with respect to <see cref="TenorTimeSpan"/>.
    /// </summary>
    [TestFixture]
    public class TenorTimeSpanTests
    {
        /// <summary>Serves as unit test for <see cref="TenorTimeSpan.Parse(string)"/>. 
        /// </summary>
        /// <param name="tenorString">The tenor string.</param>
        /// <param name="expectedYears">The expected years.</param>
        /// <param name="expectedMonths">The expected months.</param>
        /// <param name="expectedDays">The expected days.</param>
        /// <param name="expectedIsPositive">The expected value of the <c>IsPositive</c> flag.</param>
        /// <param name="expectedTenorType">The expected type of the tenor.</param>
        [TestCase("1Y5M1W", 1, 5, 7, true, TenorType.RegularTenor)]
        [TestCase("5Y 2m 10W", 5, 2, 70, true, TenorType.RegularTenor)]
        [TestCase("- 8m  2w 4d", 0, -8, -18, false, TenorType.RegularTenor)]
        [TestCase("  0 ", 0, 0, 0, true, TenorType.RegularTenor)]
        [TestCase(" oN ", 0, 0, 1, true, TenorType.Overnight)]
        [TestCase(" T n  ", 0, 0, 1, true, TenorType.TomorrowNext)]
        [TestCase(" 4y ", 4, 0, 0, true, TenorType.RegularTenor)]
        [TestCase(" 15m ", 1, 3, 0, true, TenorType.RegularTenor)]
        [TestCase(" 5d 4M 5Y 2w ", 5, 4, 19, true, TenorType.RegularTenor)]
        [TestCase("4y 25M 53w 6d", 6, 1, 53 * 7 + 6, true, TenorType.RegularTenor)]
        public void Parse_TestCase(string tenorString, int expectedYears, int expectedMonths, int expectedDays, bool expectedIsPositive, TenorType expectedTenorType)
        {
            TenorTimeSpan actualTenor = TenorTimeSpan.Parse(tenorString);

            Assert.That(actualTenor.Years, Is.EqualTo(expectedYears), "Years component");
            Assert.That(actualTenor.Months, Is.EqualTo(expectedMonths), "Months component");
            Assert.That(actualTenor.Days, Is.EqualTo(expectedDays), "Days component");

            Assert.That(actualTenor.TenorType, Is.EqualTo(expectedTenorType), "Tenor type component");
            Assert.That(actualTenor.IsPositive, Is.EqualTo(expectedIsPositive), "IsPositive component");
        }

        /// <summary>Serves as unit test for <see cref="TenorTimeSpan.Parse(string)"/>. 
        /// </summary>
        /// <param name="tenorString">The tenor string.</param>
        /// <param name="expectedYears">The expected years.</param>
        /// <param name="expectedMonths">The expected months.</param>
        /// <param name="expectedDays">The expected days.</param>
        /// <param name="expectedIsPositive">The expected value of the <c>IsPositive</c> flag.</param>
        /// <param name="expectedTenorType">The expected type of the tenor.</param>
        /// <param name="expectedException">The expected exception.</param>
        [TestCase("-2Y 8m  -1w 4d", -2, 8, 3, false, TenorType.RegularTenor, typeof(ArgumentException))]
        [TestCase("12Y 8m  1E 4d", 12, 8, 0, false, TenorType.RegularTenor, typeof(ArgumentException))]
        public void Parse_TestCaseException(string tenorString, int expectedYears, int expectedMonths, int expectedDays, bool expectedIsPositive, TenorType expectedTenorType, Type expectedException)
        {
            Assert.Throws(expectedException, () =>
            {
                TenorTimeSpan actualTenor = TenorTimeSpan.Parse(tenorString);

                Assert.That(actualTenor.Years, Is.EqualTo(expectedYears), "Years component");
                Assert.That(actualTenor.Months, Is.EqualTo(expectedMonths), "Months component");
                Assert.That(actualTenor.Days, Is.EqualTo(expectedDays), "Days component");

                Assert.That(actualTenor.TenorType, Is.EqualTo(expectedTenorType), "Tenor type component");
                Assert.That(actualTenor.IsPositive, Is.EqualTo(expectedIsPositive), "IsPositive component");
            });
        }

        /// <summary>Serves as unit test for <see cref="TenorTimeSpan.GetTimeSpanInBetween(DateTime,DateTime,TenorTimeSpan.RoundingRule)"/>.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="roundingRule">The rounding rule.</param>
        /// <returns>A <see cref="TenorTimeSpan"/> representation of the time span between <paramref name="startDate"/> and <paramref name="endDate"/> where
        /// years and months are taken into account only.</returns>
        [TestCaseSource(nameof(TestCaseDataForGetTimeSpanInBetween))]
        public TenorTimeSpan GetTimeSpanInBetween_TestCase(DateTime startDate, DateTime endDate, TenorTimeSpan.RoundingRule roundingRule)
        {
            return TenorTimeSpan.GetTimeSpanInBetween(startDate, endDate, roundingRule);
        }

        /// <summary>Gets the test case data for <see cref="GetTimeSpanInBetween_TestCase(DateTime, DateTime,TenorTimeSpan.RoundingRule)"/>.
        /// </summary>
        /// <value>The test case data for <see cref="GetTimeSpanInBetween_TestCase(DateTime, DateTime,TenorTimeSpan.RoundingRule)"/>.</value>
        public static IEnumerable<TestCaseData> TestCaseDataForGetTimeSpanInBetween
        {
            get
            {
                yield return new TestCaseData(new DateTime(2000, 1, 1), new DateTime(2010, 6, 1), TenorTimeSpan.RoundingRule.Exact).Returns(new TenorTimeSpan(10, 5, 0));

                yield return new TestCaseData(new DateTime(2004, 4, 6), new DateTime(2006, 5, 6), TenorTimeSpan.RoundingRule.Exact).Returns(new TenorTimeSpan(2, 1, 0));

                yield return new TestCaseData(new DateTime(2004, 4, 6), new DateTime(2006, 3, 6), TenorTimeSpan.RoundingRule.Exact).Returns(new TenorTimeSpan(1, 11, 0));

                yield return new TestCaseData(new DateTime(2004, 4, 6), new DateTime(2006, 3, 21), TenorTimeSpan.RoundingRule.Exact).Returns(new TenorTimeSpan(1, 11, 15));

                yield return new TestCaseData(new DateTime(2004, 4, 6), new DateTime(2006, 3, 2), TenorTimeSpan.RoundingRule.Exact).Returns(new TenorTimeSpan(1, 10, 24));
                yield return new TestCaseData(new DateTime(2004, 4, 6), new DateTime(2006, 3, 2), TenorTimeSpan.RoundingRule.NearestMonth).Returns(new TenorTimeSpan(1, 11, 0));

                yield return new TestCaseData(new DateTime(2006, 4, 6), new DateTime(2004, 3, 2), TenorTimeSpan.RoundingRule.Exact).Returns(new TenorTimeSpan(-2, -1, -4));
                yield return new TestCaseData(new DateTime(2006, 4, 6), new DateTime(2004, 3, 2), TenorTimeSpan.RoundingRule.NearestMonth).Returns(new TenorTimeSpan(-2, -1, 0));

                yield return new TestCaseData(new DateTime(2006, 3, 2), new DateTime(2004, 4, 6), TenorTimeSpan.RoundingRule.Exact).Returns(new TenorTimeSpan(-1, -10, -26));
                yield return new TestCaseData(new DateTime(2006, 3, 2), new DateTime(2004, 4, 6), TenorTimeSpan.RoundingRule.NearestMonth).Returns(new TenorTimeSpan(-1, -11, 0));


                /* Adding a TenorTimeSpan to a specific start date and computing the end date should be conform to the calculation of the TenorTimeSpan with 
                 * respect to the start date and the end date:
                 */

                DateTime startDate1 = new DateTime(2001, 5, 19);
                TenorTimeSpan timeSpanSpan1 = new TenorTimeSpan(3, 6, 4);
                DateTime endDate1 = startDate1.AddTenorTimeSpan(timeSpanSpan1);
                yield return new TestCaseData(startDate1, endDate1, TenorTimeSpan.RoundingRule.Exact).Returns(timeSpanSpan1);
                yield return new TestCaseData(startDate1, endDate1, TenorTimeSpan.RoundingRule.NearestMonth).Returns(new TenorTimeSpan(3, 6, 0));

                DateTime startDate2 = new DateTime(2006, 12, 4);
                TenorTimeSpan timeSpanSpan2 = new TenorTimeSpan(-3, -6, -18);
                DateTime endDate2 = startDate2.AddTenorTimeSpan(timeSpanSpan2);
                yield return new TestCaseData(startDate2, endDate2, TenorTimeSpan.RoundingRule.Exact).Returns(timeSpanSpan2);
                yield return new TestCaseData(startDate2, endDate2, TenorTimeSpan.RoundingRule.NearestMonth).Returns(new TenorTimeSpan(-3, -7, 0));

                DateTime startDate3 = new DateTime(2000, 5, 18);
                TenorTimeSpan timeSpanSpan3 = new TenorTimeSpan(-5, -2, -5);
                DateTime endDate3 = startDate3.AddTenorTimeSpan(timeSpanSpan3);
                yield return new TestCaseData(startDate3, endDate3, TenorTimeSpan.RoundingRule.Exact).Returns(timeSpanSpan3);
                yield return new TestCaseData(startDate3, endDate3, TenorTimeSpan.RoundingRule.NearestMonth).Returns(new TenorTimeSpan(-5, -2, 0));
            }
        }

        /// <summary>Serves as unit test for <see cref="TenorTimeSpan.IsInsideTimeSpan(DateTime,DateTime,int)"/>.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="tenorTimeSpan">The tenor time span.</param>
        /// <param name="testDate">The test date.</param>
        /// <param name="deferredDays">The number of deferred days applyied to the end date of the time period only.</param>
        /// <returns>
        /// 	<c>true</c> if <paramref name="testDate"/> is inside the specific time period; otherwise, <c>false</c>.
        /// </returns>
        [TestCaseSource(nameof(TestCaseDataForIsInsideTimeSpan))]
        public bool IsInsideTimeSpan_TestCase(DateTime startDate, TenorTimeSpan tenorTimeSpan, DateTime testDate, int deferredDays)
        {
            return tenorTimeSpan.IsInsideTimeSpan(startDate, testDate, deferredDays);
        }

        /// <summary>Gets the test case data for <see cref="IsInsideTimeSpan_TestCase(DateTime, TenorTimeSpan, DateTime,int)"/>.
        /// </summary>
        /// <value>The test case data for <see cref="IsInsideTimeSpan_TestCase(DateTime, TenorTimeSpan, DateTime,int)"/>.</value>
        public static IEnumerable<TestCaseData> TestCaseDataForIsInsideTimeSpan
        {
            get
            {
                yield return new TestCaseData(new DateTime(2000, 1, 1), new TenorTimeSpan(10, 6, 0), new DateTime(2006, 5, 3), 0).Returns(true);

                yield return new TestCaseData(new DateTime(2000, 1, 1), new TenorTimeSpan(10, 0, 0), new DateTime(2010, 1, 5), 4).Returns(true);

                yield return new TestCaseData(new DateTime(2000, 1, 1), new TenorTimeSpan(10, 0, 0), new DateTime(2010, 1, 5), 3).Returns(false);
            }
        }

        /// <summary>Serves as unit test for <see cref="TenorTimeSpan.ToRawYearFraction"/>.
        /// </summary>
        [Test]
        public void ToRawYearFraction_1Y6M__1_5()
        {
            TenorTimeSpan tenorTimeSpan = new TenorTimeSpan(1, 6, 0);
            double actual = tenorTimeSpan.ToRawYearFraction();

            double expected = 1.5;

            Assert.That(actual, Is.EqualTo(expected).Within(1E-6));
        }

        /// <summary>Serves as unit test for <see cref="TenorTimeSpan.ToRawYearFraction"/>.
        /// </summary>
        [Test]
        public void ToRawYearFraction_7Y3M__1_5()
        {
            TenorTimeSpan tenorTimeSpan = new TenorTimeSpan(7, 3, 0);
            double actual = tenorTimeSpan.ToRawYearFraction();

            double expected = 7.25;

            Assert.That(actual, Is.EqualTo(expected).Within(1E-6));
        }
    }
}