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
using System.Linq;
using System.Collections.Generic;

using NUnit.Framework;

namespace Dodoni.MathLibrary.ProbabilityTheory.Distributions
{
    [TestFixture]
    public class ExponentialDistributionTests
    {
        [TestCaseSource("CentralMomentTestCaseData")]
        public void CentralMoment_Order_BenchmarkResults(double lambda, int n, double expected)
        {
            var exponentialDistribution = ExponentialDistribution.Create(lambda);
            var actual = exponentialDistribution.Moment.GetCentralValue(n);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-5).Percent);
        }

        public static IEnumerable<TestCaseData> CentralMomentTestCaseData
        {
            get
            {
                foreach (var lambda in new[] { 0.1, 0.87, 1.9 })
                {
                    yield return new TestCaseData(lambda, 0, 1.0);
                    yield return new TestCaseData(lambda, 1, 0.0);
                    yield return new TestCaseData(lambda, 2, 1 / Math.Pow(lambda, 2));
                    yield return new TestCaseData(lambda, 3, 2 / Math.Pow(lambda, 3));
                    yield return new TestCaseData(lambda, 4, 9 / Math.Pow(lambda, 4));
                    yield return new TestCaseData(lambda, 5, 44 / Math.Pow(lambda, 5));
                    yield return new TestCaseData(lambda, 6, 265 / Math.Pow(lambda, 6));
                    yield return new TestCaseData(lambda, 7, 1854 / Math.Pow(lambda, 7));
                    yield return new TestCaseData(lambda, 8, 14833 / Math.Pow(lambda, 8));
                }
            }
        }
    }
}