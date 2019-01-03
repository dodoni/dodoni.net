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
using System.Data;
using System.Collections.Generic;

using NUnit.Framework;

namespace Dodoni.BasicComponents.Containers
{
    /// <summary>A unit test class for <see cref="InfoOutputPackage"/>.
    /// </summary>
    [TestFixture]
    public class InfoOutputPackageTests
    {
        #region private const members

        /// <summary>The category name of the <see cref="InfoOutputPackage"/> object to test.
        /// </summary>
        private const string m_CategoryName = "TestCategory";
        #endregion

        #region private members

        /// <summary>A <see cref="InfoOutputPackage"/> object to apply unit tests.
        /// </summary>
        private InfoOutputPackage m_InfoOutputPackage;
        #endregion

        #region public methods

        /// <summary>Setup the unit tests, i.e. create a <see cref="InfoOutputPackage"/> object for test purposes.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            m_InfoOutputPackage = new InfoOutputPackage(m_CategoryName);
        }

        /// <summary>A test function that adds a specific general property in a <see cref="InfoOutputPackage"/> object and compares the results again.
        /// </summary>
        [Test]
        public void GeneralProperties_DoublePropertyName_TheProperty()
        {
            string propertyName = "Time to expiry";
            object propertyValue = 42.0;

            m_InfoOutputPackage.Add(propertyName, propertyValue);
            m_InfoOutputPackage.Add("A second property name", "Second property value");  // add a second property 

            InfoOutputProperty expectedInfoOutputProperty = new InfoOutputProperty("time To    Expiry", 42.0);

            var actualInfoOutputProperty = m_InfoOutputPackage.GeneralProperties[propertyName];

            Assert.That(actualInfoOutputProperty, PropertyNameValuePair.Matches(expectedInfoOutputProperty));
        }

        /// <summary>A test function that adds a specific general property in a <see cref="InfoOutputPackage"/> object and compares the results again.
        /// </summary>
        [Test]
        public void GeneralProperties_IdentifierStringProperty_TheProperty()
        {
            string propertyName = "Day count convention";
            object propertyValue = IdentifierString.Create("30/360");

            m_InfoOutputPackage.Add(propertyName, propertyValue);
            m_InfoOutputPackage.Add("A second property name", "Second property value");  // add a second property 

            InfoOutputProperty expectedInfoOutputProperty = new InfoOutputProperty("day  Count convention", propertyValue);

            var actualInfoOutputProperty = m_InfoOutputPackage.GeneralProperties[propertyName];

            Assert.That(actualInfoOutputProperty, PropertyNameValuePair.Matches(expectedInfoOutputProperty));
        }

        /// <summary>A test function that tries to get a specific general property.
        /// </summary>
        [Test]
        public void GeneralProperties_UnknownProperty_ThrowKeyNotFoundException()
        {
            void CheckException()
            {
                var unknownProperty = m_InfoOutputPackage.GeneralProperties["Unknown property"];
            }
            Assert.Throws<KeyNotFoundException>(CheckException);
        }

        /// <summary>A test function that adds a specific property (in a new property group) in a <see cref="InfoOutputPackage"/> object and compares the results again.
        /// </summary>
        [Test]
        public void TestPropertyGroup_DoublePropertyName_TheProperty()
        {
            string propertyGroupName = "TestPropertyGroup";
            string propertyName = "Time to expiry";
            object propertyValue = 42.0;

            // here, we add a second property 
            m_InfoOutputPackage.Add(propertyGroupName,
                InfoOutputProperty.Create(propertyName, propertyValue),
                InfoOutputProperty.Create("A second property name", "Second property value"));


            InfoOutputProperty expectedInfoOutputProperty = new InfoOutputProperty("time To    Expiry", 42.0);

            var actualInfoOutputProperty = m_InfoOutputPackage.GetProperty(propertyName, propertyGroupName);

            Assert.That(actualInfoOutputProperty, PropertyNameValuePair.Matches(expectedInfoOutputProperty));
        }

        /// <summary>A test function that adds a specific <see cref="DataTable"/> object in a <see cref="InfoOutputPackage"/> object and compares the results again.
        /// </summary>
        [Test]
        public void GetDataTable_ExampleTableName_ExampleDataTable()
        {
            string tableName = "Date and Value Table";

            DataTable dataTable = new DataTable(tableName);
            dataTable.Columns.Add("Date", typeof(DateTime));
            dataTable.Columns.Add("Value", typeof(double));

            dataTable.Rows.Add(new DateTime(2011, 12, 1), 42.42);

            m_InfoOutputPackage.Add(dataTable);

            DataTable actualDataTable = m_InfoOutputPackage.GetDataTable("date And  value table");

            Assert.That(actualDataTable, Is.EqualTo(dataTable));
        }
        #endregion
    }
}