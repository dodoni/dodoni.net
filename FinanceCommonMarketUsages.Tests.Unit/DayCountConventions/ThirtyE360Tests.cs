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

namespace Dodoni.Finance.CommonMarketUsages.DayCountConventions
{
    /// <summary>Serves as class for unit tests with respect to <see cref="ThirtyE360"/>.
    /// </summary>
    [TestFixture]
    public class ThirtyE360Tests
    {
        /// <summary>Serves as unit test for <see cref="IDayCountConvention.GetYearFraction(DateTime, DateTime, DateTime?, DateTime?)"/>.
        /// </summary>
        /// <param name="startDate">The start date of the period.</param>
        /// <param name="endDate">The end date of the period.</param>
        /// <param name="expectedYearFraction">The expected year fraction.</param>
        [TestCaseSource("GetYearFractionTestCaseData")]
        public void GetYearFraction_TestCase(DateTime startDate, DateTime endDate, double expectedYearFraction)
        {
            double actualYearFraction = DayCountConvention.ThirtyE360.GetYearFraction(startDate, endDate);

            Assert.That(actualYearFraction, Is.EqualTo(expectedYearFraction).Within(1E-6));
        }

        /// <summary>Gets the test case data for <see cref="GetYearFraction_TestCase(DateTime, DateTime, double)"/>.
        /// </summary>
        /// <value>The test case data for <see cref="GetYearFraction_TestCase(DateTime, DateTime, double)"/>.</value>
        public static IEnumerable<TestCaseData> GetYearFractionTestCaseData
        {
            get
            {
                // use QuantLib 1.2.0 as a benchmark:
                yield return new TestCaseData(new DateTime(2003, 11, 1), new DateTime(2004, 5, 1), 0.5);
                yield return new TestCaseData(new DateTime(2001, 10, 1), new DateTime(2004, 5, 1), 2.583333333333330);
                yield return new TestCaseData(new DateTime(2000, 2, 26), new DateTime(2002, 8, 15), 2.469444444444440);
                yield return new TestCaseData(new DateTime(2000, 2, 29), new DateTime(2003, 2, 18), 2.969444444444440);
                yield return new TestCaseData(new DateTime(2003, 5, 18), new DateTime(2012, 6, 3), 9.041666666666670);
            }
        }
    }
}