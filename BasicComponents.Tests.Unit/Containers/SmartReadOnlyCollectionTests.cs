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
using NUnit.Framework;

namespace Dodoni.BasicComponents.Containers
{
    /// <summary>A unit test class for <see cref="SmartReadOnlyCollection&lt;T&gt;"/>.
    /// </summary>
    public class SmartReadOnlyCollectionTests
    {
        /// <summary>A test function that creates a specific <see cref="SmartReadOnlyCollection&lt;T&gt;"/> object and compares the results.
        /// </summary>
        [Test]
        public void GetterProperty_IntTestCaseData_BenchmarkResults()
        {
            var benchmarkList = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var smartReadOnlyCollection = new SmartReadOnlyCollection<int>(5, benchmarkList, 1, 2);

            Assert.That(smartReadOnlyCollection[0], Is.EqualTo(2));
            Assert.That(smartReadOnlyCollection[1], Is.EqualTo(4));
            Assert.That(smartReadOnlyCollection[2], Is.EqualTo(6));
            Assert.That(smartReadOnlyCollection[3], Is.EqualTo(8));
            Assert.That(smartReadOnlyCollection[4], Is.EqualTo(10));
        }
    }
}