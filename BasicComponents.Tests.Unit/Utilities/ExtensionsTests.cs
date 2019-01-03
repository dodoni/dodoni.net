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

namespace Dodoni.BasicComponents.Utilities
{
    /// <summary>A unit test class for <see cref="Extensions"/>.
    /// </summary>
    [TestFixture]
    public class ExtensionsTests
    {
        /// <summary>A test function that compares the result of <see cref="Extensions.GetRawDistanceTo(DateTime, DateTime)"/>.
        /// </summary>        
        [Test]
        public void GetRawDistanceTo__From_01_01_2010_To_01_01_2020__10()
        {
            DateTime startDate = new DateTime(2010, 1, 1);
            DateTime endDate = new DateTime(2020, 1, 1);

            Assert.AreEqual(10, Extensions.GetRawDistanceTo(startDate, endDate), 1E-10);
        }

        /// <summary>A test function that compares the result of <see cref="Extensions.GetRawDistanceTo(DateTime, DateTime)"/>.
        /// </summary>        
        [Test]
        public void GetRawDistanceTo__From_01_01_2010_To_30_06_2020__10_5()
        {
            DateTime startDate = new DateTime(2010, 1, 1);
            DateTime endDate = new DateTime(2020, 6, 30);

            Assert.AreEqual(10.5, Extensions.GetRawDistanceTo(startDate, endDate), 1E-2);
        }

        /// <summary>A test function that compares the result of <see cref="Extensions.GetRawDistanceTo(DateTime, DateTime)"/>.
        /// </summary>        
        [Test]
        public void GetRawDistanceTo__From_07_05_2009_To_01_04_2015__5_897()
        {
            DateTime startDate = new DateTime(2009, 5, 7);
            DateTime endDate = new DateTime(2015, 4, 1);

            // 5Y 10M 23d
            Assert.AreEqual(5.0 + 10.0 / 12.0 + (23.0 / 30.0) / 12.0, Extensions.GetRawDistanceTo(startDate, endDate), 1E-2);
        }

        /// <summary>A test function that compares the result of <see cref="Extensions.GetRawDistanceTo(DateTime, DateTime)"/>.
        /// </summary>        
        [Test]
        public void GetRawDistanceTo__From_01_04_2015_To_07_05_2009__5_897()
        {
            DateTime startDate = new DateTime(2015, 4, 1);
            DateTime endDate = new DateTime(2009, 5, 7);

            // absolute( -[5Y 10M 23d] ) 
            Assert.AreEqual(5.0 + 10.0 / 12.0 + (23.0 / 30.0) / 12.0, Extensions.GetRawDistanceTo(startDate, endDate), 1E-2);
        }

        /// <summary>A test function that compares the result of <see cref="Extensions.GetRawDistanceTo(DateTime, DateTime)"/>.
        /// </summary>        
        [Test]
        public void GetRawDistanceTo__From_09_10_2003_To_23_08_2014__10_872()
        {
            DateTime startDate = new DateTime(2003, 10, 9);
            DateTime endDate = new DateTime(2014, 8, 23);

            // 10Y 9M 44d
            Assert.AreEqual(10.0 + 9.0 / 12.0 + (44.0 / 30.0) / 12.0, Extensions.GetRawDistanceTo(startDate, endDate), 1E-2);

            // 10Y 10M 14d
            Assert.AreEqual(10.0 + 10.0 / 12.0 + (14.0 / 30.0) / 12.0, Extensions.GetRawDistanceTo(startDate, endDate), 1E-2);
        }

        /// <summary>A test function that compares the result of <see cref="Extensions.BinarySearch&lt;T&gt;(IList&lt;T&gt;,T)"/>.
        /// </summary>        
        [Test]
        public void BinarySearch__0To99And11__11()
        {
            IList<double> sortedListOfNumbers = Enumerable.Range(0, 100).Select(x => (double)x).ToList();
            Assert.AreEqual(11, Extensions.BinarySearch<double>(sortedListOfNumbers, 11.0));
        }

        /// <summary>A test function that compares the result of <see cref="Extensions.BinarySearch&lt;T&gt;(IList&lt;T&gt;,T)"/>.
        /// </summary>        
        [Test]
        public void BinarySearch__0To99And99__99()
        {
            IList<double> sortedListOfNumbers = Enumerable.Range(0, 100).Select(x => (double)x).ToList();
            Assert.AreEqual(99, Extensions.BinarySearch<double>(sortedListOfNumbers, 99.0));
        }

        /// <summary>A test function that compares the result of <see cref="Extensions.BinarySearch&lt;T&gt;(IList&lt;T&gt;,T)"/>.
        /// </summary>        
        [Test]
        public void BinarySearch__0To99And100__BitwiseComplementOf100()
        {
            IList<double> sortedListOfNumbers = Enumerable.Range(0, 100).Select(x => (double)x).ToList();
            Assert.AreEqual(~100, Extensions.BinarySearch<double>(sortedListOfNumbers, 100.0));
        }


        /// <summary>A test function that compares the result of <see cref="Extensions.BinarySearch&lt;T&gt;(IList&lt;T&gt;,T)"/>.
        /// </summary>        
        [Test]
        public void BinarySearch__0To99And0__0()
        {
            IList<double> sortedListOfNumbers = Enumerable.Range(0, 100).Select(x => (double)x).ToList();
            Assert.AreEqual(0, Extensions.BinarySearch<double>(sortedListOfNumbers, 0.0));
        }

        /// <summary>A test function that compares the result of <see cref="Extensions.BinarySearch&lt;T&gt;(IList&lt;T&gt;,T)"/>.
        /// </summary>        
        [Test]
        public void BinarySearch__0To99And0_5__BitwiseComplementOf1()
        {
            IList<double> sortedListOfNumbers = Enumerable.Range(0, 100).Select(x => (double)x).ToList();
            Assert.AreEqual(~1, Extensions.BinarySearch<double>(sortedListOfNumbers, 0.5));
        }

        /// <summary>A test function that compares the result of <see cref="Extensions.BinarySearch&lt;T&gt;(IList&lt;T&gt;,T)"/>.
        /// </summary>        
        [Test]
        public void BinarySearch__0To99AndMinus2_5__Minus1()
        {
            List<double> sortedListOfNumbers = Enumerable.Range(0, 100).Select(x => (double)x).ToList();
            
            Assert.AreEqual(sortedListOfNumbers.BinarySearch(-2.5), Extensions.BinarySearch<double>(sortedListOfNumbers, -2.5));
            Assert.AreEqual(-1, Extensions.BinarySearch<double>(sortedListOfNumbers, -2.5));
        }
    }
}