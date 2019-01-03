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
    /// <summary>Serves as class for unit tests with respect to <see cref="ActualActualISMA"/>.
    /// </summary>
    [TestFixture]
    public class ActualActualISMA_Tests
    {
        /// <summary>Serves as unit test for <see cref="IDayCountConvention.GetYearFraction(DateTime, DateTime, DateTime?, DateTime?)"/>.
        /// </summary>
        /// <param name="startDate">The start date of the period.</param>
        /// <param name="endDate">The end date of the period.</param>
        /// <param name="referenceStartDate">The optional reference start date.</param>
        /// <param name="referenceEndDate">The optional reference end date.</param>
        /// <param name="expectedYearFraction">The expected year fraction.</param>
        [TestCaseSource("GetYearFractionTestCaseData")]
        public void GetYearFraction_TestCase(DateTime startDate, DateTime endDate, DateTime? referenceStartDate, DateTime? referenceEndDate, double expectedYearFraction)
        {
            double actualYearFraction = DayCountConvention.ActualActual.ISMA.GetYearFraction(startDate, endDate, referenceStartDate, referenceEndDate);

            Assert.That(actualYearFraction, Is.EqualTo(expectedYearFraction).Within(1E-6));
        }

        /// <summary>Gets the test case data for <see cref="GetYearFraction_TestCase(DateTime, DateTime, DateTime?, DateTime?, double)"/>.
        /// </summary>
        /// <value>The test case data for <see cref="GetYearFraction_TestCase(DateTime, DateTime, DateTime?, DateTime?, double)"/>.</value>
        public static IEnumerable<TestCaseData> GetYearFractionTestCaseData
        {
            get
            {
                /* The following unit tests are inspired by the unit tests of the QuantLib project; 
                 *  Copyright (C) 2003 RiskMap srl
                 *  Copyright (C) 2006 Piter Dias
                 */
                yield return new TestCaseData(new DateTime(2003, 11, 1), new DateTime(2004, 5, 1), null, null, 0.5);
                yield return new TestCaseData(new DateTime(1999, 2, 1), new DateTime(1999, 7, 1), new DateTime(1998, 7, 1), new DateTime(1999, 7, 1), 0.410958904110);
                yield return new TestCaseData(new DateTime(1999, 7, 1), new DateTime(2000, 7, 1), new DateTime(1999, 7, 1), new DateTime(2000, 7, 1), 1.0);
                yield return new TestCaseData(new DateTime(2002, 8, 15), new DateTime(2003, 7, 15), new DateTime(2003, 1, 15), new DateTime(2003, 7, 15), 0.915760869565); //?
                yield return new TestCaseData(new DateTime(2003, 7, 15), new DateTime(2004, 1, 15), new DateTime(2003, 7, 15), new DateTime(2004, 1, 15), 0.5);
                yield return new TestCaseData(new DateTime(1999, 7, 30), new DateTime(2000, 1, 30), new DateTime(1999, 7, 30), new DateTime(2000, 1, 30), 0.5);
                yield return new TestCaseData(new DateTime(2000, 1, 30), new DateTime(2000, 6, 30), new DateTime(2000, 1, 30), new DateTime(2000, 7, 30), 0.417582417582);


                // use QuantLib 1.2.0 as benchmark:
                yield return new TestCaseData(new DateTime(2003, 11, 1), new DateTime(2004, 5, 1), null, null, 0.5);
                yield return new TestCaseData(new DateTime(2001, 10, 1), new DateTime(2004, 5, 1), null, null, 2.583333333333330);
                yield return new TestCaseData(new DateTime(2000, 2, 26), new DateTime(2002, 8, 15), null, null, 2.5);
                yield return new TestCaseData(new DateTime(2000, 2, 29), new DateTime(2003, 2, 18), null, null, 3.0);
                yield return new TestCaseData(new DateTime(2003, 5, 18), new DateTime(2012, 6, 3), null, null, 9.083333333333330);


                // benchmark: "EMU and market conventions: Recent developments", ISDA Internationa Swaps and Derivatives Association, Inc., Euro Swap memo ISDA 1998.
                yield return new TestCaseData(new DateTime(1999, 2, 1), new DateTime(2000, 7, 1), new DateTime(1999, 7, 1), new DateTime(2000, 7, 1), 1.410958904109590);
                yield return new TestCaseData(new DateTime(2002, 8, 15), new DateTime(2004, 1, 15), new DateTime(2003, 7, 15), new DateTime(2004, 1, 15), 181.0 / (2 * 181) + 153.0 / (2 * 184) + 184.0 / (2 * 184.0));
                yield return new TestCaseData(new DateTime(1999, 7, 30), new DateTime(2000, 6, 30), new DateTime(1999, 7, 30), new DateTime(2000, 1, 30), 184.0 / (184 * 2) + 152.0 / (2 * 182));
            }
        }
    }
}