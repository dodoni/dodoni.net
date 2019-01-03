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
    /// <summary>Serves as class for unit tests with respect to <see cref="TenorTimeSpanSpread"/>.
    /// </summary>
    [TestFixture]
    public class TenorTimeSpanSpreadTests
    {
        /// <summary>Serves as unit test for <see cref="TenorTimeSpanSpread.Parse(string)"/>. 
        /// </summary>
        /// <param name="tenorTimeSpanSpreadString">The string representation of the <see cref="TenorTimeSpanSpread"/> object to create.</param>
        /// <param name="isTenorTimeSpan">The expected <see cref="TenorTimeSpanSpread.IsTenorTimeSpan"/> flag.</param>
        /// <param name="firstTenor">The expected first tenor.</param>
        /// <param name="firstTenorDescription">The excpected first tenor description.</param>
        /// <param name="secondTenor">The expected second tenor.</param>
        /// <param name="secondTenorDescription">The expected second tenor description.</param>
        [TestCaseSource(nameof(TestCaseDataForParse))]
        public void Parse_TestCase(string tenorTimeSpanSpreadString, bool isTenorTimeSpan, TenorTimeSpan firstTenor, string firstTenorDescription, TenorTimeSpan secondTenor, string secondTenorDescription)
        {
            TenorTimeSpanSpread tenorTimeSpanSpread = TenorTimeSpanSpread.Parse(tenorTimeSpanSpreadString);

            Assert.That(tenorTimeSpanSpread.IsTenorTimeSpan, Is.EqualTo(isTenorTimeSpan), "IsTenorTimeSpan");

            Assert.That(tenorTimeSpanSpread.FirstTenor, Is.EqualTo(firstTenor), "FirstTenor");
            Assert.That(tenorTimeSpanSpread.FirstTenorDescription, Is.EqualTo(firstTenorDescription), "FirstTenorDescription");

            Assert.That(tenorTimeSpanSpread.SecondTenor, Is.EqualTo(secondTenor), "SecondTenor");

            Assert.That(tenorTimeSpanSpread.SecondTenorDescription, Is.EqualTo(secondTenorDescription), "SecondTenorDescription");
        }

        /// <summary>Gets the test case data for <see cref="Parse_TestCase(string,bool,TenorTimeSpan,string,TenorTimeSpan,string)"/>.
        /// </summary>
        /// <value>The test case data for <see cref="Parse_TestCase(string,bool,TenorTimeSpan,string,TenorTimeSpan,string)"/>.</value>
        public static IEnumerable<TestCaseData> TestCaseDataForParse
        {
            get
            {
                yield return new TestCaseData("1Y 6M vs. 7Y 2w", false, new TenorTimeSpan(1, 6, 0), String.Empty, new TenorTimeSpan(7, 0, 14), String.Empty);
                yield return new TestCaseData("1Y 6M - 7Y 2w", false, new TenorTimeSpan(1, 6, 0), String.Empty, new TenorTimeSpan(7, 0, 14), String.Empty);

                yield return new TestCaseData("5Y 2W [EUR] - 2M 2w", false, new TenorTimeSpan(5, 0, 14), "EUR", new TenorTimeSpan(0, 2, 14), String.Empty);
                yield return new TestCaseData("5Y 2W [EUR] vs. 2M 2w", false, new TenorTimeSpan(5, 0, 14), "EUR", new TenorTimeSpan(0, 2, 14), String.Empty);
                yield return new TestCaseData("6M 2Y 1d [US] - 1Y[UK]", false, new TenorTimeSpan(2, 6, 1), "US", new TenorTimeSpan(1, 0, 0), "UK");

                yield return new TestCaseData("1W 1d 4M [EUR]", true, new TenorTimeSpan(0, 4, 8), "EUR", TenorTimeSpan.Null, String.Empty);


                // invalid input:
//                yield return new TestCaseData("-2M 6Y 2w  - 5M 3W [DE]", false, new TenorTimeSpan(-2, -6, -14), "Argument-EXCEPTION", new TenorTimeSpan(0, 5, 21), "DE").Throws(typeof(ArgumentException));
            }
        }
    }
}