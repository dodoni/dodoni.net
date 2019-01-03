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

using NSubstitute;
using NUnit.Framework;

namespace Dodoni.BasicComponents.Containers
{
    /// <summary>Serves as unit test class for <see cref="IdentifierNameableDictionary&lt;TValue&gt;"/>.
    /// </summary>
    [TestFixture]
    public class IdentifierNameableDictionaryTests
    {
        #region private members

        /// <summary>The object to test.
        /// </summary>
        private IdentifierNameableDictionary<IIdentifierNameable> m_IdentifierNameableDictionary;
        #endregion

        /// <summary>Setup the unit tests.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            var firstItem = Substitute.For<IIdentifierNameable>();
            firstItem.Name.Returns(new IdentifierString("Name of first item"));

            var secondItem = Substitute.For<IIdentifierNameable>();
            secondItem.Name.Returns(new IdentifierString("Name of second item"));

            m_IdentifierNameableDictionary = IdentifierNameableDictionary.Create(firstItem, secondItem);
        }

        /// <summary>A test function that first adds an additional item and calls <see cref="IdentifierStringDictionaryBase&lt;TValue&gt;.TryGetValue(string, out TValue)"/>.
        /// </summary>
        [Test]
        public void AddItem_TryGetValue_TestCase()
        {
            var newItem = Substitute.For<IIdentifierNameable>();
            newItem.Name.Returns(new IdentifierString("TestItem"));

            m_IdentifierNameableDictionary.Add(newItem);

            bool state = m_IdentifierNameableDictionary.TryGetValue("TestItem", out IIdentifierNameable testOutput);

            Assert.That(state, NUnit.Framework.Is.EqualTo(true));
            Assert.That(testOutput, NUnit.Framework.Is.EqualTo(newItem));
        }

        /// <summary>A test function for <see cref="IdentifierStringDictionaryBase&lt;TValue&gt;.TryGetValue(string, out TValue)"/> that queries the first item that has been added.
        /// </summary>
        [Test]
        public void TryGetValue_FirstItem_True()
        {
            bool state = m_IdentifierNameableDictionary.TryGetValue("Name of first item", out IIdentifierNameable testOutput);

            Assert.That(state, NUnit.Framework.Is.EqualTo(true));
        }

        /// <summary>A test function for <see cref="IdentifierNameableDictionary&lt;TValue&gt;.Remove(string)"/>.
        /// </summary>
        [Test]
        [TestCase(typeof(InvalidOperationException))]
        public void Remove_FirstItem_InvalidOperationException(Type expectedException)
        {
            // by default it is allowed to add items, but removing a item throws an exception:
            Assert.Throws(expectedException, () => m_IdentifierNameableDictionary.Remove("Name of first item"));
        }

        /// <summary>A test function for <see cref="IdentifierNameableDictionary&lt;TValue&gt;.Add(TValue)"/> using the delegate infastructure to cancel the adding.
        /// </summary>
        [Test]
        public void Add_CanceledNewItem_ItemAddedStateRejected()
        {
            var newItem = Substitute.For<IIdentifierNameable>();
            newItem.Name.Returns(new IdentifierString("TestItem"));

            m_IdentifierNameableDictionary.Adding += (sender, eventArgs) => { eventArgs.Cancel = true; };
            m_IdentifierNameableDictionary.Added += (sender, eventArgs) => { Assert.That(eventArgs.State, NUnit.Framework.Is.EqualTo(ItemAddedState.Rejected)); };

            m_IdentifierNameableDictionary.Add(newItem);
        }

        /// <summary>A test function for <see cref="IdentifierNameableDictionary&lt;TValue&gt;.Add(TValue)"/> and checking the delegate infastructure.
        /// </summary>
        [Test]
        public void Add_NewItem_ItemAddedStateAdded()
        {
            var newItem = Substitute.For<IIdentifierNameable>();
            newItem.Name.Returns(new IdentifierString("TestItem"));

            m_IdentifierNameableDictionary.Adding += (sender, eventArgs) => { Assert.That(eventArgs.OldItem, NUnit.Framework.Is.Null); };
            m_IdentifierNameableDictionary.Added += (sender, eventArgs) => { Assert.That(eventArgs.State, NUnit.Framework.Is.EqualTo(ItemAddedState.Added)); };

            m_IdentifierNameableDictionary.Add(newItem);
        }
    }
}