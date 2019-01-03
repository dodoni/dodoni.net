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
using System.Xml;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Dodoni.BasicComponents.Containers;

namespace Dodoni.BasicComponents.Utilities
{
    public partial class ConfigurationFile : IDisposable
    {
        /// <summary>Represents a collection of homogenious entries in a configuration file.
        /// </summary>
        public class Table : IInfoOutputQueriable, IIdentifierNameable
        {
            #region nested classes

            /// <summary>The collection of each data entry in the table.
            /// </summary>
            public class DataCollection : IEnumerable<IList<string>>
            {
                #region private nested classes

                /// <summary>The Enumerator implementation of the <see cref="DataCollection"/> class.
                /// </summary>
                private class Enumerator : IEnumerator<IList<string>>
                {
                    #region private members

                    private DataCollection m_DataCollection;
                    private int m_CurrentIndex = -1;
                    #endregion

                    #region internal constructors

                    /// <summary>Initializes a new instance of the <see cref="Enumerator" /> class.
                    /// </summary>
                    /// <param name="dataCollection">The data collection.</param>
                    internal Enumerator(DataCollection dataCollection)
                    {
                        m_DataCollection = dataCollection;
                    }
                    #endregion

                    #region IEnumerator<IList<string>> Members

                    /// <summary>Gets the element in the collection at the current position of the enumerator.
                    /// </summary>
                    /// <value>The element in the collection at the current position of the enumerator.</value>
                    public IList<string> Current
                    {
                        get { return m_DataCollection[m_CurrentIndex]; }
                    }

                    #endregion

                    #region IDisposable Members

                    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
                    /// </summary>
                    public void Dispose()
                    {
                    }
                    #endregion

                    #region IEnumerator Members

                    /// <summary> Gets the element in the collection at the current position of the enumerator.
                    /// </summary>
                    /// <value>The element in the collection at the current position of the enumerator.</value>
                    object IEnumerator.Current
                    {
                        get { return m_DataCollection[m_CurrentIndex]; }
                    }

                    /// <summary>Advances the enumerator to the next element of the collection.
                    /// </summary>
                    /// <returns><c>true</c> if the enumerator was successfully advanced to the next element; <c>false</c> if the enumerator has passed the end of the collection.</returns>
                    public bool MoveNext()
                    {
                        m_CurrentIndex++;
                        return m_CurrentIndex < m_DataCollection.Count;
                    }

                    /// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.
                    /// </summary>
                    public void Reset()
                    {
                        m_CurrentIndex = -1;
                    }
                    #endregion
                }
                #endregion

                #region private members

                /// <summary>The reference to the <see cref="Table"/> object.
                /// </summary>
                private Table m_Table;

                /// <summary>The name of the entries in the data table, for example 'Plain Vanilla option'.
                /// </summary>
                private IdentifierString m_TableEntryName;

                /// <summary>The parent node of the section in the XML file that represents the data.
                /// </summary>
                private XmlNode m_TableParentNode;
                #endregion

                #region public constructors

                /// <summary>Initializes a new instance of the <see cref="DataCollection" /> class.
                /// </summary>
                /// <param name="table">The reference to the <see cref="Table"/> object.</param>
                /// <param name="tableParentNode">The parent node of the section in the XML file that represents the data.</param>
                /// <param name="tableEntryName">The name of each data table entry.</param>
                public DataCollection(Table table, XmlNode tableParentNode, IdentifierString tableEntryName)
                {
                    m_Table = table;

                    m_TableEntryName = tableEntryName;
                    m_TableParentNode = tableParentNode;
                }
                #endregion

                #region public properties

                /// <summary>Gets the number of entries.
                /// </summary>
                /// <value>The number of entries.
                /// </value>
                public int Count
                {
                    get { return m_TableParentNode.ChildNodes.Count; }
                }

                /// <summary>Gets the data at a specified null-based index.
                /// </summary>
                /// <param name="index">The null-based index.</param>
                /// <returns>For each field the value in its <see cref="System.String"/> representation.</returns>
                public string[] this[int index]
                {
                    get
                    {
                        var entryNode = m_TableParentNode.ChildNodes[index];

                        var value = new string[entryNode.ChildNodes.Count];
                        for (int j = 0; j < entryNode.ChildNodes.Count; j++)
                        {
                            value[j] = entryNode.ChildNodes[j].InnerText;
                        }
                        return value;
                    }
                }
                #endregion

                #region public methods

                #region IEnumerable<IList<string>> Members

                /// <summary>Returns an enumerator that iterates through a collection.
                /// </summary>
                /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
                public IEnumerator<IList<string>> GetEnumerator()
                {
                    return new Enumerator(this);
                }
                #endregion

                #region IEnumerable Members

                /// <summary>Returns an enumerator that iterates through a collection.
                /// </summary>
                /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
                IEnumerator IEnumerable.GetEnumerator()
                {
                    return new Enumerator(this);
                }
                #endregion

                /// <summary>Append a new data entry into the configuration file.
                /// </summary>
                /// <param name="values">The values.</param>
                /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="values"/> is <c>null</c>.</exception>
                /// <exception cref="System.ArgumentException">Thrown if the length of <paramref name="values"/> is different from the number of fields.</exception>
                public void Append(params string[] values)
                {
                    if (values == null)
                    {
                        throw new ArgumentNullException(nameof(values));
                    }
                    if (values.Length != m_Table.m_FieldNames.Length)
                    {
                        throw new ArgumentException(String.Format(ExceptionMessages.ArgumentHasWrongDimension, nameof(values)));
                    }
                    var dataRow = m_Table.m_ConfigurationFile.m_XmlFileRepresenation.CreateElement(m_TableEntryName.IDString);
                    for (int j = 0; j < m_Table.m_FieldNames.Length; j++)
                    {
                        var dataEntry = m_Table.m_ConfigurationFile.m_XmlFileRepresenation.CreateElement(m_Table.m_FieldNames[j]);
                        dataEntry.AppendChild(m_Table.m_ConfigurationFile.m_XmlFileRepresenation.CreateTextNode(values[j]));

                        dataRow.AppendChild(dataEntry);
                    }
                    m_TableParentNode.AppendChild(dataRow);
                }

                /// <summary>Removes all data.
                /// </summary>
                public void Clear()
                {
                    m_TableParentNode.RemoveAll();
                }

                /// <summary>Removes an item at the specified null-based index.
                /// </summary>
                /// <param name="index">The null-based index of the item to remove.</param>
                public void RemoveAt(int index)
                {
                    m_TableParentNode.RemoveChild(m_TableParentNode.ChildNodes[index]);
                }

                /// <summary>Changes the value at a specified null-based index and with respect to a specified field.
                /// </summary>
                /// <param name="index">The null-based index of the data entry.</param>
                /// <param name="fieldIndex">The null-based index of the field.</param>
                /// <param name="value">The value to set in its <see cref="System.String"/> representation.</param>
                public void ChangeAt(int index, int fieldIndex, string value)
                {
                    var entryNode = m_TableParentNode.ChildNodes[index];

                    entryNode.ChildNodes[fieldIndex].InnerText = value;
                }
                #endregion
            }
            #endregion

            #region private members

            /// <summary>The configuration file in its <see cref="ConfigurationFile"/> representation.
            /// </summary>
            private ConfigurationFile m_ConfigurationFile;

            /// <summary>The name of the data table in the configuration file.
            /// </summary>
            private IdentifierString m_TableName;

            /// <summary>The name of the fields of each entry in the data table.
            /// </summary>
            private string[] m_FieldNames;

            /// <summary>The data collection.
            /// </summary>
            private DataCollection m_Items;

            /// <summary>A value indicating whether the configuration file already contains settings with respect to the current <see cref="ConfigurationFile.Table"/> object.
            /// </summary>
            private readonly bool m_ContainsSettings;
            #endregion

            #region internal protected constructors

            /// <summary>Initializes a new instance of the <see cref="Table" /> class.
            /// </summary>
            /// <param name="configurationFile">The configuration file.</param>
            /// <param name="tableName">The name of the table, i.e. of the section in the XML representation of the configuration file.</param>
            /// <param name="tableEntryName">The name of each data table entry.</param>
            /// <param name="fieldNames">The name of each field of the data table entries.</param>
            internal protected Table(ConfigurationFile configurationFile, IdentifierString tableName, IdentifierString tableEntryName, params string[] fieldNames)
            {
                m_ConfigurationFile = configurationFile;
                m_TableName = tableName;
                m_FieldNames = fieldNames;

                m_Items = new DataCollection(this, configurationFile.GetSectionNode(tableName.IDString, out m_ContainsSettings), tableEntryName);
            }

            /// <summary>Initializes a new instance of the <see cref="Table" /> class.
            /// </summary>
            /// <param name="configurationFile">The configuration file.</param>
            /// <param name="tableName">The name of the table, i.e. of the section in the XML representation of the configuration file.</param>
            /// <param name="tableEntryName">The name of each data table entry.</param>
            /// <param name="fieldNames">The name of each field of the data table entries.</param>
            internal protected Table(ConfigurationFile configurationFile, string tableName, string tableEntryName, params string[] fieldNames)
            {
                m_ConfigurationFile = configurationFile;
                m_TableName = new IdentifierString(tableName);
                m_FieldNames = fieldNames;

                m_Items = new DataCollection(this, configurationFile.GetSectionNode(m_TableName.IDString, out m_ContainsSettings), IdentifierString.Create(tableEntryName));
            }
            #endregion

            #region public properties

            #region IIdentifierNameable Members

            /// <summary>Gets the name of the current instance.
            /// </summary>
            /// <value>The language independent name of the current instance.
            /// </value>
            public IdentifierString Name
            {
                get { return m_TableName; }
            }

            /// <summary>Gets the long name of the current instance.
            /// </summary>
            /// <value>The (perhaps) language dependent long name of the current instance.
            /// </value>
            public IdentifierString LongName
            {
                get { return m_TableName; }
            }
            #endregion

            #region IInfoOutputQueriable Members

            /// <summary>Gets the info-output level of detail.
            /// </summary>
            /// <value>The info-output level of detail.
            /// </value>
            public InfoOutputDetailLevel InfoOutputDetailLevel
            {
                get { return InfoOutputDetailLevel.Full; }
            }
            #endregion

            /// <summary>Gets the name of the fields of each entry in the table.
            /// </summary>
            /// <value>The data field names.
            /// </value>
            public string[] FieldNames
            {
                get { return m_FieldNames; }
            }

            /// <summary>Gets an object representing the collection of the items contained in this <see cref="ConfigurationFile.Table"/> object.
            /// </summary>
            /// <value>The collection of data.
            /// </value>
            public DataCollection Items
            {
                get { return m_Items; }
            }

            /// <summary>Gets a value indicating whether the configuration file already contains settings with respect to the current <see cref="ConfigurationFile"/> object; will be set to <c>true</c>
            /// after calling <see cref="Save()"/>.
            /// </summary>
            /// <value><c>true</c> if the configuration file already contains some settings; otherwise, <c>false</c>.</value>
            public bool ContainsSettings
            {
                get { return m_ContainsSettings; }
            }
            #endregion

            #region public methods

            #region IInfoOutputQueriable Members

            /// <summary>Sets the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> property.
            /// </summary>
            /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
            /// <returns>A value indicating whether the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> has been set to <paramref name="infoOutputDetailLevel" />.
            /// </returns>
            public bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
            {
                return (infoOutputDetailLevel == Containers.InfoOutputDetailLevel.Full);
            }

            /// <summary>Gets informations of the current object as a specific <see cref="InfoOutput" /> instance.
            /// </summary>
            /// <param name="infoOutput">The <see cref="InfoOutput" /> object which is to be filled with informations concering the current instance.</param>
            /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
            public void FillInfoOutput(InfoOutput infoOutput, string categoryName = InfoOutput.GeneralCategoryName)
            {
                var infoPackage = infoOutput.AcquirePackage(categoryName);

                var dataTable = new DataTable(m_TableName.String);
                foreach (var fieldName in m_FieldNames)
                {
                    dataTable.Columns.Add(fieldName, typeof(String));
                }

                for (int j = 0; j < m_Items.Count; j++)
                {
                    var values = m_Items[j];
                    var rowData = dataTable.NewRow();

                    for (int k = 0; k < m_FieldNames.Length; k++)
                    {
                        rowData[k] = values[k];
                    }
                    dataTable.Rows.Add(rowData);
                }
            }
            #endregion

            #endregion
        }
    }
}
