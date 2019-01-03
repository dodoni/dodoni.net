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
using System.Reflection;

using NUnit.Framework;

namespace Dodoni.BasicComponents
{
    /// <summary>A unit test class for <see cref="IdentifierString"/>.
    /// </summary>
    [TestFixture]
    public class IdentifierStringTests
    {
        /// <summary>Data points for the unit tests.
        /// </summary>
        [Datapoints]
        public string[] TestInputStrings = new string[] { "Hello", " StranGe ExAmple   ", " Contains a @ ignoring Part" };

        /// <summary>A test function that compares the <see cref="IdentifierString.String"/> component of the result of <see cref="IdentifierString.Create(String)"/>.
        /// </summary>
        /// <param name="rawString">The (raw) string.</param>
        [Theory]
        public void CreateAndGetString_RawString_RawString(string rawString)
        {
            Assume.That(rawString != null);

            IdentifierString identifierString = IdentifierString.Create(rawString);

            Assert.That(identifierString != null);
            Assert.That(identifierString.String, Is.EqualTo(rawString));
        }

        /// <summary>A test function that compares the <see cref="IdentifierString.IDString"/> component of the result of <see cref="IdentifierString.Create(String)"/>.
        /// </summary>
        /// <param name="rawString">The (raw) string.</param>
        [Theory]
        public void CreateAndGetIDString_RawString_IDStringOfRawString(string rawString)
        {
            Assume.That(rawString != null);

            IdentifierString identifierString = IdentifierString.Create(rawString);

            Assert.That(identifierString != null);
            Assert.That(rawString.ToIDString(), Is.EqualTo(identifierString.IDString));
        }

        /// <summary>A test function that compares the <see cref="IdentifierString.IDString"/> component of the result of <see cref="IdentifierString.Create(String)"/>.
        /// </summary>
        /// <param name="rawString">The (raw) string.</param>
        [Theory]
        public void CreateAndGetIDString_TrimLowerRawString_IDStringOfRawString(string rawString)
        {
            Assume.That(rawString != null);

            IdentifierString identifierString = IdentifierString.Create(rawString);

            string modifiedRawString = rawString.Trim().ToLower();
            IdentifierString modifiedIdentifierString = IdentifierString.Create(modifiedRawString);

            Assert.That(identifierString.IDString, Is.EqualTo(modifiedIdentifierString.IDString));
        }

        /// <summary>A test function that compares the <see cref="IdentifierString.IDString"/> component of the result of <see cref="IdentifierString.Create(String)"/>.
        /// </summary>
        /// <param name="rawString">The (raw) string.</param>
        [Theory]
        public void CreateAndGetIDString_TrimUpperRawString_IDStringOfRawString(string rawString)
        {
            Assume.That(rawString != null);

            IdentifierString identifierString = IdentifierString.Create(rawString);

            string modifiedRawString = rawString.Trim().ToUpper();
            IdentifierString modifiedIdentifierString = IdentifierString.Create(modifiedRawString);

            Assert.That(identifierString.IDString, Is.EqualTo(modifiedIdentifierString.IDString));
        }

        /// <summary>A test function that compares the <see cref="IdentifierString.IDString"/> component of the result of <see cref="IdentifierString.Create(String)"/>.
        /// </summary>
        /// <param name="rawString">The (raw) string.</param>
        [Theory]
        public void CreateAndGetIDString_NoWhiteSpacesLowerRawString_IDStringOfRawString(string rawString)
        {
            Assume.That(rawString != null);

            IdentifierString identifierString = IdentifierString.Create(rawString);

            string modifiedRawString = rawString.Replace(" ", String.Empty).ToLower();
            IdentifierString modifiedIdentifierString = IdentifierString.Create(modifiedRawString);

            Assert.That(identifierString.IDString, Is.EqualTo(modifiedIdentifierString.IDString));
        }

        /// <summary>A test function that compares the <see cref="IdentifierString.IDString"/> component of the result of <see cref="IdentifierString.Create(String)"/>.
        /// </summary>
        /// <param name="rawString">The (raw) string.</param>
        [Theory]
        public void CreateAndGetIDString_NoWhiteSpacesUpperRawString_IDStringOfRawString(string rawString)
        {
            Assume.That(rawString != null);

            IdentifierString identifierString = IdentifierString.Create(rawString);

            string modifiedRawString = rawString.Replace(" ", String.Empty).ToUpper();
            IdentifierString modifiedIdentifierString = IdentifierString.Create(modifiedRawString);

            Assert.That(identifierString.IDString, Is.EqualTo(modifiedIdentifierString.IDString));
        }

        /// <summary>A test function for <see cref="IdentifierString.Create(String,String,Assembly)"/>.
        /// </summary>
        /// <param name="fullResourceName">The resource name (no language dependend suffix) and the corresponding namespace.</param>
        /// <param name="resourcePropertyName">The property name with respect to a given resource which contains some language dependend <see cref="System.String"/> representation.</param>
        [TestCase("Dodoni.BasicComponents.IdentifierStringTestsResources", "ExampleTwoResourcePropertyName", ExpectedResult = "A    simple EXample")]
        [TestCase("Dodoni.BasicComponents.IdentifierStringTestsResources", "ExampleResourcePropertyName", ExpectedResult = "  a sTranGe NAme@ should be ignored")]
        public string CreateAndGetString_ResourceNameAndResourcePropertyName_PropertyValue(string fullResourceName, string resourcePropertyName)
        {
            IdentifierString identifierString = IdentifierString.Create(fullResourceName, resourcePropertyName, Assembly.GetAssembly(typeof(IdentifierStringTests)));

            return identifierString.String;
        }

        /// <summary>A test function for <see cref="IdentifierString.Create(String,String,Assembly)"/>.
        /// </summary>
        /// <param name="fullResourceName">The resource name (no language dependend suffix) and the corresponding namespace.</param>
        /// <param name="resourcePropertyName">The property name with respect to a given resource which contains some language dependend <see cref="System.String"/> representation.</param>
        /// <param name="expectedException">The expected exception.</param>
        [TestCase("Dodoni.BasicComponents.WrongResourceName", "ExampleResourcePropertyName", typeof(ArgumentException))]
        [TestCase("Dodoni.BasicComponents.IdentifierStringTestsResources", "InvalidResourcePropertyName", typeof(ArgumentException))]
        public void CreateAndGetString_ResourceNameAndResourcePropertyName_PropertyValueException(string fullResourceName, string resourcePropertyName, Type expectedException)
        {
            Assert.Throws(expectedException, () =>
            {
                IdentifierString identifierString = IdentifierString.Create(fullResourceName, resourcePropertyName, Assembly.GetAssembly(typeof(IdentifierStringTests)));

                var value = identifierString.String;
            });
        }

    }
}