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
using System.Collections.Generic;

using NUnit.Framework;

namespace Dodoni.MathLibrary
{
    /// <summary>Serves as unit test class for <see cref="RoundingRule"/>.
    /// </summary>
    public class RoundingRuleTests
    {
        /// <summary>A test function for a specific rounding rule.
        /// </summary>
        /// <param name="numberOfDigits">The number of digits for the rounding rule.</param>
        /// <param name="rawValue">The raw (input) value.</param>
        /// <param name="expected">The expected value.</param>
        [TestCaseSource("BankersRoundingTestCaseData")]
        public void BankersRoundingGetValue_TestCaseData_BenchmarkResult(int numberOfDigits, double rawValue, double expected)
        {
            var roundingRule = RoundingRule.BankersRounding.Create(numberOfDigits);

            double actual = roundingRule.GetValue(rawValue);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-6));
        }

        /// <summary>Gets the test case data for the Bankers rounding rule.
        /// </summary>
        /// <value>The test case data for Bankers rounding rule.</value>
        public static IEnumerable<TestCaseData> BankersRoundingTestCaseData
        {
            get
            {
                yield return new TestCaseData(2, 1.12345, 1.12);
                yield return new TestCaseData(2, 1.1251, 1.13);
                yield return new TestCaseData(3, -1.12345, -1.123);
                yield return new TestCaseData(3, -1.12355, -1.124);
            }
        }

        /// <summary>A test function for a specific rounding rule.
        /// </summary>
        /// <param name="unit">The unit to take into account.</param>
        /// <param name="rawValue">The raw (input) value.</param>
        /// <param name="expected">The expected value.</param>
        [TestCaseSource("CeilingRoundingTestCaseData")]
        public void CeilingRoundingGetValue_TestCaseData_BenchmarkResult(double unit, double rawValue, double expected)
        {
            var roundingRule = RoundingRule.Ceiling.Create(unit);

            double actual = roundingRule.GetValue(rawValue);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-6));
        }

        /// <summary>Gets the test case data for the Ceiling rounding rule.
        /// </summary>
        /// <value>The test case data for Ceiling rounding rule.</value>
        public static IEnumerable<TestCaseData> CeilingRoundingTestCaseData
        {
            get
            {
                yield return new TestCaseData(0.1, 1.12345, 1.2);
                yield return new TestCaseData(0.5, 1.1251, 1.5);
                yield return new TestCaseData(10.1, 26.17, 30.3);
                yield return new TestCaseData(0.1, -1.12345, -1.2);
                yield return new TestCaseData(0.25, -1.12355, -1.25);
            }
        }

        /// <summary>A test function for a specific rounding rule.
        /// </summary>
        /// <param name="unit">The unit to take into account.</param>
        /// <param name="rawValue">The raw (input) value.</param>
        /// <param name="expected">The expected value.</param>
        [TestCaseSource("FloorRoundingTestCaseData")]
        public void FloorRoundingGetValue_TestCaseData_BenchmarkResult(double unit, double rawValue, double expected)
        {
            var roundingRule = RoundingRule.Floor.Create(unit);

            double actual = roundingRule.GetValue(rawValue);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-6));
        }

        /// <summary>Gets the test case data for the Floor rounding rule.
        /// </summary>
        /// <value>The test case data for Floor rounding rule.</value>
        public static IEnumerable<TestCaseData> FloorRoundingTestCaseData
        {
            get
            {
                yield return new TestCaseData(0.1, 1.12345, 1.1);
                yield return new TestCaseData(0.5, 1.1251, 1.0);
                yield return new TestCaseData(10.1, 26.17, 20.2);
                yield return new TestCaseData(0.1, -1.12345, -1.1);
                yield return new TestCaseData(0.25, -1.12355, -1.0);
            }
        }

        /// <summary>A test function for a specific rounding rule.
        /// </summary>
        /// <param name="rawValue">The raw (input) value.</param>
        /// <param name="expected">The expected value.</param>
        [TestCaseSource("NoRoundingTestCaseData")]
        public void NoRoundingGetValue_TestCaseData_BenchmarkResult(double rawValue, double expected)
        {
            var roundingRule = RoundingRule.NoRounding;

            double actual = roundingRule.GetValue(rawValue);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-6));
        }

        /// <summary>Gets the test case data for the 'No rounding' rounding rule.
        /// </summary>
        /// <value>The test case data for 'No rounding' rounding rule.</value>
        public static IEnumerable<TestCaseData> NoRoundingTestCaseData
        {
            get
            {
                yield return new TestCaseData(1.12345, 1.12345);
                yield return new TestCaseData(1.1251, 1.1251);
                yield return new TestCaseData(26.17, 26.17);
                yield return new TestCaseData(-1.12345, -1.12345);
                yield return new TestCaseData(-1.12355, -1.12355);
            }
        }

        /// <summary>A test function for a specific rounding rule.
        /// </summary>
        /// <param name="numberOfDigits">The number of digits for the rounding rule.</param>
        /// <param name="rawValue">The raw (input) value.</param>
        /// <param name="expected">The expected value.</param>
        [TestCaseSource("TruncateRoundingTestCaseData")]
        public void TruncateRoundingGetValue_TestCaseData_BenchmarkResult(int numberOfDigits, double rawValue, double expected)
        {
            var roundingRule = RoundingRule.Truncate.Create(numberOfDigits);

            double actual = roundingRule.GetValue(rawValue);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-6));
        }

        /// <summary>Gets the test case data for the Truncating rounding rule.
        /// </summary>
        /// <value>The test case data for Truncating rounding rule.</value>
        public static IEnumerable<TestCaseData> TruncateRoundingTestCaseData
        {
            get
            {
                yield return new TestCaseData(3, 1.12345, 1.123);
                yield return new TestCaseData(1, 1.1251, 1.1);
                yield return new TestCaseData(0, 26.17, 26.0);
                yield return new TestCaseData(2, -1.12345, -1.12);
                yield return new TestCaseData(1, -1.12355, -1.1);
            }
        }
    }
}