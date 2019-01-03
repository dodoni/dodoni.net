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
using System.Collections.Generic;

using NUnit.Framework;

namespace Dodoni.BasicComponents.Containers
{
    /// <summary>A unit test class for <see cref="InfoOutputProperty"/>.
    /// </summary>
    [TestFixture]
    public class InfoOutputPropertyTests
    {
        /// <summary>A test function that compares the <see cref="InfoOutputProperty.Name"/> component of a specific <see cref="InfoOutputProperty"/> object.
        /// </summary>
        [Test]
        public void CreateAndGetPropertyName_ExampleStringProperty_ExampleStringPropertyName()
        {
            string propertyName = "Example property name";
            string propertyValue = "Example proprty value";
            var infoOutputProperty = new InfoOutputProperty(propertyName, propertyValue);

            Assert.That(infoOutputProperty.Name, Is.EqualTo(propertyName));
        }

        /// <summary>A test function that compares the <see cref="InfoOutputProperty.Value"/> component of a specific <see cref="InfoOutputProperty"/> object.
        /// </summary>
        [Test]
        public void CreateAndGetPropertyValue_ExampleStringProperty_ExampleStringPropertyValue()
        {
            string propertyName = "Example property name";
            string propertyValue = "Example proprty value";
            var infoOutputProperty = new InfoOutputProperty(propertyName, propertyValue);

            Assert.That(infoOutputProperty.Value, Is.EqualTo(propertyValue));
        }

        /// <summary>A test function that compares the <see cref="InfoOutputProperty.Value"/> component of a specific <see cref="InfoOutputProperty"/> object.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <param name="propertyValue">The property value.</param>
        [TestCaseSource(nameof(TestPropertyNamesAndValues))]
        public void CreateAndGetPropertyValue_ObjectProperty_PropertyValue(string propertyName, object propertyValue)
        {
            var infoOutputProperty = new InfoOutputProperty(propertyName, propertyValue);

            Assert.That(infoOutputProperty.Value, Is.SameAs(propertyValue));
        }

        /// <summary>Gets a collection of property names and values for test purpose.
        /// </summary>
        /// <value>The test property names and values.</value>
        public static IEnumerable<object[]> TestPropertyNamesAndValues
        {
            get
            {
                yield return new object[] { "Example property name", "Example property value" };
                yield return new object[] { "Cut-off date", new DateTime(2010, 10, 12) };
                yield return new object[] { "Factor", 42.42 };
                yield return new object[] { "Name", IdentifierString.Create("My name") };
                yield return new object[] { "Day of week", DayOfWeek.Friday };
                yield return new object[] { "List of double", new List<double>() { 1.0, 2.0, 3.0 } };
            }
        }
    }
}