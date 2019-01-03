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

using NUnit.Framework;

namespace Dodoni.BasicComponents.Containers
{
    /// <summary>Serves as unit test class for <see cref="IdentifierStringDictionary&lt;TValue&gt;"/>.
    /// </summary>
    [TestFixture]
    public class IdentifierStringDictionaryTests
    {
        #region private members

        /// <summary>The object to test.
        /// </summary>
        private IdentifierStringDictionary<object> m_IdentifierStringDictionary;
        #endregion

        /// <summary>Setup the unit tests.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            m_IdentifierStringDictionary = IdentifierStringDictionary.Create<object>("Name of first item", 42.12, "Name of second item", true);
        }

        /// <summary>A test function that first adds an additional item and calls <see cref="IdentifierStringDictionaryBase&lt;TValue&gt;.TryGetValue(string, out TValue)"/>.
        /// </summary>
        [Test]
        public void AddItem_TryGetValue_TestCase()
        {
            m_IdentifierStringDictionary.Add("TestItem", 1234);

            bool state = m_IdentifierStringDictionary.TryGetValue("TestItem", out object testOutput);

            Assert.That(state, NUnit.Framework.Is.EqualTo(true));
            Assert.That(testOutput, NUnit.Framework.Is.EqualTo(1234));
        }

        /// <summary>A test function for <see cref="IdentifierStringDictionaryBase&lt;TValue&gt;.TryGetValue(string, out TValue)"/> that queries the first item that has been added.
        /// </summary>
        [Test]
        public void TryGetValue_FirstItem_True()
        {
            bool state = m_IdentifierStringDictionary.TryGetValue("Name of first item", out object testOutput);

            Assert.That(state, NUnit.Framework.Is.EqualTo(true));
        }

        /// <summary>A test function for <see cref="IdentifierStringDictionary&lt;TValue&gt;.Remove(string)"/>.
        /// </summary>
        [Test]
        public void Remove_FirstItem_InvalidOperationException()
        {
            void Remove()
            {
                // by default it is allowed to add items, but removing a item throws an exception:
                m_IdentifierStringDictionary.Remove("Name of first item");
            }
            Assert.Throws<InvalidOperationException>(Remove);
        }
    }
}