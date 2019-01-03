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
    /// <summary>Serves as class for unit tests with respect to <see cref="TenorTimeSpanExtensions"/>.
    /// </summary>
    [TestFixture]
    public class TenorTimeSpanExtensionsTests
    {
        /// <summary>Serves as unit test for <see cref="TenorTimeSpanExtensions.AddTimeSpan(TenorTimeSpan,TenorTimeSpan,int)"/>. 
        /// </summary>
        /// <param name="tenorTimeSpan">The tenor time span.</param>
        /// <param name="tenorTimeSpanToAdd">The <see cref="TenorTimeSpan"/> to add.</param>
        /// <param name="tenorTimeSpanFactor">A (optional) factor to take into account.</param>
        /// <returns>A <see cref="TenorTimeSpan"/> object which is the result of <paramref name="tenorTimeSpan"/> plus <paramref name="tenorTimeSpanFactor"/> * <paramref name="tenorTimeSpanToAdd"/>.</returns>
        [TestCaseSource(nameof(TestCaseData_AddTimeSpan))]
        public TenorTimeSpan AddTimeSpan_TestCase(TenorTimeSpan tenorTimeSpan, TenorTimeSpan tenorTimeSpanToAdd, int tenorTimeSpanFactor)
        {
            return TenorTimeSpanExtensions.AddTimeSpan(tenorTimeSpan, tenorTimeSpanToAdd, tenorTimeSpanFactor);
        }

        /// <summary>Gets the test case data for <see cref="AddTimeSpan_TestCase(TenorTimeSpan, TenorTimeSpan, int)"/>.
        /// </summary>
        /// <value>The test case data for <see cref="AddTimeSpan_TestCase(TenorTimeSpan, TenorTimeSpan, int)"/>.</value>
        public static IEnumerable<TestCaseData> TestCaseData_AddTimeSpan
        {
            get
            {
                yield return new TestCaseData(new TenorTimeSpan(1, 6, 0), new TenorTimeSpan(0, 6, 0), 2).Returns(new TenorTimeSpan(2, 6, 0));
                yield return new TestCaseData(new TenorTimeSpan(2, 3, 10), new TenorTimeSpan(0, 3, 0), 3).Returns(new TenorTimeSpan(3, 0, 10));
                yield return new TestCaseData(new TenorTimeSpan(0, 2, 4), new TenorTimeSpan(1, 4, 0), 2).Returns(new TenorTimeSpan(2, 10, 4));
            }
        }

        /// <summary>Serves as unit test for <see cref="TenorTimeSpanExtensions.AddTenorTimeSpan(DateTime,TenorTimeSpan)"/>. 
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="tenorTimeSpan">The tenor time span.</param>
        /// <returns>The <see cref="DateTime"/> object that represents <paramref name="startDate"/> plus <paramref name="tenorTimeSpan"/>.</returns>
        [TestCaseSource(nameof(TestCaseData_AddTenorTimeSpan))]
        public DateTime AddTenorTimeSpan_TestCase(DateTime startDate, TenorTimeSpan tenorTimeSpan)
        {
            return TenorTimeSpanExtensions.AddTenorTimeSpan(startDate, tenorTimeSpan);
        }

        /// <summary>Gets the test case data for <see cref="AddTenorTimeSpan_TestCase(DateTime, TenorTimeSpan)"/>.
        /// </summary>
        /// <value>The test case data for <see cref="AddTenorTimeSpan_TestCase(DateTime, TenorTimeSpan)"/>.</value>
        public static IEnumerable<TestCaseData> TestCaseData_AddTenorTimeSpan
        {
            get
            {
                yield return new TestCaseData(new DateTime(2010, 2, 18), new TenorTimeSpan(2, 3, 2)).Returns(new DateTime(2012, 5, 20));
                yield return new TestCaseData(new DateTime(2003, 1, 31), new TenorTimeSpan(5, 2, 10)).Returns(new DateTime(2008, 4, 10));
            }
        }

        /// <summary>Serves as unit test for <see cref="TenorTimeSpanExtensions.AddTenorTimeSpan(DateTime,TenorTimeSpan,int)"/>. 
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="tenorTimeSpan">The tenor time span.</param>
        /// <param name="tenorTimeSpanFactor">A factor to take into account.</param>
        /// <returns>The <see cref="DateTime"/> object that represents <paramref name="startDate"/> plus <paramref name="tenorTimeSpanFactor"/> * <paramref name="tenorTimeSpan"/>.</returns>
        [TestCaseSource(nameof(TestCaseData_AddTenorTimeSpanWithTenorTimeSpanFactorArgument))]
        public DateTime AddTenorTimeSpan_TestCase(DateTime startDate, TenorTimeSpan tenorTimeSpan, int tenorTimeSpanFactor)
        {
            return TenorTimeSpanExtensions.AddTenorTimeSpan(startDate, tenorTimeSpan, tenorTimeSpanFactor);
        }

        /// <summary>Gets the test case data for <see cref="AddTenorTimeSpan_TestCase(DateTime, TenorTimeSpan,int)"/>.
        /// </summary>
        /// <value>The test case data for <see cref="AddTenorTimeSpan_TestCase(DateTime, TenorTimeSpan,int)"/>.</value>
        public static IEnumerable<TestCaseData> TestCaseData_AddTenorTimeSpanWithTenorTimeSpanFactorArgument
        {
            get
            {
                yield return new TestCaseData(new DateTime(2010, 2, 18), new TenorTimeSpan(2, 3, 2), 1).Returns(new DateTime(2012, 5, 20));
                yield return new TestCaseData(new DateTime(2010, 2, 18), new TenorTimeSpan(2, 3, 2), 3).Returns(new DateTime(2016, 11, 24));

                yield return new TestCaseData(new DateTime(2003, 1, 31), new TenorTimeSpan(5, 2, 10), 1).Returns(new DateTime(2008, 4, 10));
                yield return new TestCaseData(new DateTime(2003, 1, 31), new TenorTimeSpan(5, 2, 10), 2).Returns(new DateTime(2013, 6, 20));
            }
        }

        /// <summary>Serves as unit test for <see cref="TenorTimeSpanExtensions.AddTenorTimeSpan(DateTime,string,int)"/>. 
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="tenorTimeSpan">The tenor time span.</param>
        /// <param name="tenorTimeSpanFactor">A factor to take into account.</param>
        /// <returns>The <see cref="DateTime"/> object that represents <paramref name="startDate"/> plus <paramref name="tenorTimeSpanFactor"/> * <paramref name="tenorTimeSpan"/>.</returns>
        [TestCaseSource(nameof(TestCaseData_AddTenorTimeSpanWithStringArgument))]
        public DateTime AddTenorTimeSpan_TestCase(DateTime startDate, string tenorTimeSpan, int tenorTimeSpanFactor)
        {
            return TenorTimeSpanExtensions.AddTenorTimeSpan(startDate, tenorTimeSpan, tenorTimeSpanFactor);
        }

        /// <summary>Gets the test case data for <see cref="AddTenorTimeSpan_TestCase(DateTime, string,int)"/>.
        /// </summary>
        /// <value>The test case data for <see cref="AddTenorTimeSpan_TestCase(DateTime, string,int)"/>.</value>
        public static IEnumerable<TestCaseData> TestCaseData_AddTenorTimeSpanWithStringArgument
        {
            get
            {
                yield return new TestCaseData(new DateTime(2010, 2, 18), "2Y 3M 2d", 1).Returns(new DateTime(2012, 5, 20));
                yield return new TestCaseData(new DateTime(2010, 2, 18), "2y 3m 2d", 3).Returns(new DateTime(2016, 11, 24));

                yield return new TestCaseData(new DateTime(2003, 1, 31), " 5Y  2m 10D", 1).Returns(new DateTime(2008, 4, 10));
                yield return new TestCaseData(new DateTime(2003, 1, 31), "5Y   2M 10d", 2).Returns(new DateTime(2013, 6, 20));
            }
        }
    }
}