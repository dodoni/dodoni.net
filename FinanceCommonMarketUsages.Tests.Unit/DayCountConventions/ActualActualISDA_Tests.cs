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
    /// <summary>Serves as class for unit tests with respect to <see cref="ActualActualISDA"/>.
    /// </summary>
    [TestFixture]
    public class ActualActualISDA_Tests
    {
        /// <summary>Serves as unit test for <see cref="IDayCountConvention.GetYearFraction(DateTime, DateTime, DateTime?, DateTime?)"/>.
        /// </summary>
        /// <param name="startDate">The start date of the period.</param>
        /// <param name="endDate">The end date of the period.</param>
        /// <param name="expectedYearFraction">The expected year fraction.</param>
        [TestCaseSource("GetYearFractionTestCaseData")]
        public void GetYearFraction_TestCase(DateTime startDate, DateTime endDate, double expectedYearFraction)
        {
            double actualYearFraction = DayCountConvention.ActualActual.ISDA.GetYearFraction(startDate, endDate);

            Assert.That(actualYearFraction, Is.EqualTo(expectedYearFraction).Within(1E-6));
        }

        /// <summary>Gets the test case data for <see cref="GetYearFraction_TestCase(DateTime, DateTime, double)"/>.
        /// </summary>
        /// <value>The test case data for <see cref="GetYearFraction_TestCase(DateTime, DateTime, double)"/>.</value>
        public static IEnumerable<TestCaseData> GetYearFractionTestCaseData
        {
            get
            {
                /* The following unit tests are inspired by the unit tests of the QuantLib project; 
                *  Copyright (C) 2003 RiskMap srl
                *  Copyright (C) 2006 Piter Dias
                */
                yield return new TestCaseData(new DateTime(2003, 11, 1), new DateTime(2004, 5, 1), 0.497724380567);
                yield return new TestCaseData(new DateTime(1999, 2, 1), new DateTime(1999, 7, 1), 0.410958904110);
                yield return new TestCaseData(new DateTime(1999, 7, 1), new DateTime(2000, 7, 1), 1.001377348600);
                yield return new TestCaseData(new DateTime(2002, 8, 15), new DateTime(2003, 7, 15), 0.915068493151);
                yield return new TestCaseData(new DateTime(2003, 7, 15), new DateTime(2004, 1, 15), 0.504004790778);
                yield return new TestCaseData(new DateTime(1999, 7, 30), new DateTime(2000, 1, 30), 0.503892506924);
                yield return new TestCaseData(new DateTime(2000, 1, 30), new DateTime(2000, 6, 30), 0.415300546448);


                // use QuantLib 1.2.0 as a benchmark:
                yield return new TestCaseData(new DateTime(2003, 11, 1), new DateTime(2004, 5, 1), 0.497724380567408);
                yield return new TestCaseData(new DateTime(2001, 10, 1), new DateTime(2004, 5, 1), 2.582655887416720);
                yield return new TestCaseData(new DateTime(2000, 2, 26), new DateTime(2002, 8, 15), 2.466172617710910);
                yield return new TestCaseData(new DateTime(2000, 2, 29), new DateTime(2003, 2, 18), 2.970304663522720);
                yield return new TestCaseData(new DateTime(2003, 5, 18), new DateTime(2012, 6, 3), 9.045422561568980);

                // benchmark: "Interest RateInstruments and Market Conventions Guide", OpenGamma Quantitative Research, Version 1.0, 4. april 2012
                yield return new TestCaseData(new DateTime(2010, 12, 30), new DateTime(2011, 1, 2), 3.0 / 365.0);
                yield return new TestCaseData(new DateTime(2011, 12, 30), new DateTime(2012, 1, 2), 2.0 / 365.0 + 1.0 / 366);
                yield return new TestCaseData(new DateTime(2010, 12, 30), new DateTime(2013, 1, 2), 367.0 / 365.0 + 366.0 / 366.0 + 1.0 / 365.0);

                // benchmark: "EMU and market conventions: Recent developments", ISDA Internationa Swaps and Derivatives Association, Inc., Euro Swap memo ISDA 1998.
                yield return new TestCaseData(new DateTime(1999, 2, 1), new DateTime(2000, 7, 1), 150.0 / 365 + 184.0 / 365 + 182.0 / 366);
                yield return new TestCaseData(new DateTime(2002, 8, 15), new DateTime(2003, 7, 15), 334.0 / 365);
                // yield return new TestCaseData(new DateTime(2003, 7, 15), new DateTime(2004, 1, 15), 184.0 / 365);
                yield return new TestCaseData(new DateTime(1999, 7, 30), new DateTime(2000, 6, 30), 155.0 / 365 + 29.0 / 366 + 152.0 / 366);
            }
        }
    }
}