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
using System.Text;
using System.Collections.Generic;

using ExcelDna.Integration;

using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Utilities;

namespace Dodoni.XLBasicComponents
{
    /// <summary>Serves as <see cref="IExcelDataQuery"/> implementation which behaves like an Excel Range but the entries are guided, i.e. remotely controlled, and not directly user input.
    /// </summary>
    /// <remarks>One may use this class to generate a specific object with respect to the Excel interface of Dodoni.NET in a automatically way. The object type of the elements must 
    /// be specified in the correct way, thus the input must be type safe, i.e. do not enter the <see cref="System.String"/> representation of a specific enumeration element, use the enumeration element itself.</remarks>
    public class GuidedExcelDataQuery : IExcelDataQuery
    {
        #region nested enumerations

        /// <summary>Represents the method how to interpret whether a specific Excel cell is empty.
        /// </summary>
        [Flags]
        public enum eEmptyExcelCellMode
        {
            /// <summary>The specific cell is <c>null</c>.
            /// </summary>
            IsNull = 0x01,

            /// <summary>The specific cell contains <see cref="System.Type.Missing"/> or <see cref="System.Reflection.Missing"/>.
            /// </summary>
            HasMissingType = 0x02,

            /// <summary>The specific cell contains <see cref="ExcelEmpty"/>.
            /// </summary>
            IsEmptyExcelCell = 0x04,

            /// <summary>The specific cell contains the empty string <see cref="System.String.Empty"/>.
            /// </summary>
            ContainsEmptyString = 0x08,

            /// <summary>The specific cell contains the type of the input, for example <code>typeof(string), typeof(bool)</code> etc. but no user informations
            /// as a <see cref="IExcelDataAdvice"/> object is given.
            /// </summary>
            HasSpecificTypeWithoutAdvice = 0x10,

            /// <summary>The specific cell contains the type of the input, for example <code>typeof(string), typeof(bool)</code> etc. and further user informations
            /// as a <see cref="IExcelDataAdvice"/> object is given.
            /// </summary>
            HasSpecificTypeWithAdvice = 0x20,

            /// <summary>The standard way of interpreting whether a specific Excel cell is empty, i.e. empty, contains <see cref="System.Type.Missing"/>, <see cref="System.String.Empty"/> or represents <see cref="ExcelEmpty"/>.
            /// </summary>
            Standard = IsNull | HasMissingType | IsEmptyExcelCell | ContainsEmptyString
        }
        #endregion

        #region private members

        /// <summary>The name of the <see cref="IExcelDataQuery"/>.
        /// </summary>
        private IdentifierString m_Name;

        /// <summary>The representation of the data (i.e. value or if no value is available the <see cref="System.Type"/> representation,
        /// perhaps <see cref="Type.Missing"/>), where the first null-based index represents the row and
        /// the second null-based index represents the column.
        /// </summary>
        private List<object[]> m_Data = new List<object[]>();

        /// <summary>Data advice, where the key represents the index of the (Excel) cell, i.e. the (i,j)-th element is <c>ColumnCount * i + j</c>.
        /// </summary>
        private Dictionary<int, IExcelDataAdvice> m_DataAdvice = new Dictionary<int, IExcelDataAdvice>();

        /// <summar>The collection of optional property names which are queried by the GUI, but not given by the user.
        /// </summar> 
        private SortedSet<string> m_UnusedOptionalPropertyNames = new SortedSet<string>();

        /// <summary>The number of columns.
        /// </summary>
        private int m_ColumnCount;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="GuidedExcelDataQuery"/> class.
        /// </summary>
        /// <param name="excelDataQueryName">The name of the Excel data query.</param>
        /// <param name="columnCount">The number of columns.</param>
        /// <param name="rowCount">The number of rows.</param>
        /// <remarks>The elements of the <see cref="GuidedExcelDataQuery"/> are filled with <see cref="Type.Missing"/>.</remarks>
        public GuidedExcelDataQuery(string excelDataQueryName, int columnCount = 2, int rowCount = 0)
        {
            if (excelDataQueryName == null)
            {
                throw new ArgumentNullException("excelDataQueryName");
            }
            m_Name = new IdentifierString(excelDataQueryName);

            if (columnCount < 0)
            {
                throw new ArgumentException(String.Format(ExceptionMessages.ArgumentOutOfRangeGreater, "column count", 1), "columnCount");
            }
            m_ColumnCount = columnCount;
            for (int j = 0; j < rowCount; j++)
            {
                AddRow();  // add empty rows
            }
        }
        #endregion

        #region protected constructors

        /// <summary>Initializes a new instance of the <see cref="GuidedExcelDataQuery"/> class.
        /// </summary>
        /// <param name="guidedExcelDataQuery">The guided excel data query.</param>
        /// <remarks>This copy constructor creates a deep copy of its argument.</remarks>
        protected GuidedExcelDataQuery(GuidedExcelDataQuery guidedExcelDataQuery)
        {
            if (guidedExcelDataQuery == null)
            {
                throw new ArgumentNullException("guidedExcelDataQuery");
            }
            m_Name = guidedExcelDataQuery.m_Name;
            m_ColumnCount = guidedExcelDataQuery.m_ColumnCount;
            m_Data = new List<object[]>();
            m_DataAdvice = new Dictionary<int, IExcelDataAdvice>();
            for (int j = 0; j < guidedExcelDataQuery.m_Data.Count; j++)
            {
                object[] newRow = new object[m_ColumnCount];
                object[] row = guidedExcelDataQuery.m_Data[j];
                for (int k = 0; k < m_ColumnCount; k++)
                {
                    newRow[k] = row[k];
                }
                m_Data.Add(newRow);
            }

            // we assume that the values of the data advice are value types:
            foreach (var keyValuePair in guidedExcelDataQuery.m_DataAdvice)
            {
                m_DataAdvice.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the current instance.
        /// </summary>
        /// <value>The language independent name of the current instance.</value>
        public IdentifierString Name
        {
            get { return m_Name; }
        }

        /// <summary>Gets the long name of the current instance.
        /// </summary>
        /// <value>The language dependent long name of the current instance.</value>
        public IdentifierString LongName
        {
            get { return m_Name; }
        }
        #endregion

        #region IExcelDataQuery Member

        /// <summary>Gets the number of rows.
        /// </summary>
        /// <value>The number of rows.</value>
        public int RowCount
        {
            get { return m_Data.Count; }
        }

        /// <summary>Gets the number of columns.
        /// </summary>
        /// <value>The number columns.</value>
        public int ColumnCount
        {
            get { return m_ColumnCount; }
        }

        /// <summary>Gets a value indicating whether the Excel Range is just a single Excel cell.
        /// </summary>
        /// <value><c>true</c> if this instance is single cell; otherwise, <c>false</c>.
        /// </value>
        public bool IsSingleCell
        {
            get { return ((m_Data.Count == 1) && (m_ColumnCount == 1)); }
        }

        /// <summary>Gets a value indicating whether the current instance is empty, i.e. <see cref="IExcelDataQuery.RowCount"/> = <see cref="IExcelDataQuery.ColumnCount"/> = 0.
        /// </summary>
        /// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
        public bool IsEmpty
        {
            get { return ((m_ColumnCount == 0) || (m_Data.Count == 0)); }
        }
        #endregion

        #endregion

        #region public methods

        #region IExcelDataQuery Member

        /// <summary>Determines whether a specific Excel cell is empty.
        /// </summary>
        /// <param name="rowIndex">The null-based index of the row.</param>
        /// <param name="columnIndex">The null-based index of the column.</param>
        /// <returns><c>true</c> if the Excel cell at the specific position is empty; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentException">Thrown, if the row/column position is not valid.</exception>
        public bool IsEmptyExcelCell(int rowIndex, int columnIndex)
        {
            if ((rowIndex < m_Data.Count) && (rowIndex >= 0) && (columnIndex < m_ColumnCount) && (columnIndex >= 0))
            {
                object data = m_Data[rowIndex][columnIndex];
                return ((data == null) || (data == Type.Missing) || (data is ExcelEmpty) || (data is ExcelMissing) || ((data is String) && (((string)data).Length == 0)));
            }
            throw new ArgumentException("Invalid cell position, row = " + rowIndex + " column = " + columnIndex + ".");
        }

        /// <summary>Gets customize data, i.e. a representation of the current <see cref="IExcelDataQuery"/> object as a specific <see cref="GuidedExcelDataQuery"/> instance.
        /// </summary>
        /// <returns>A representation of the data which contains the type of each property etc.</returns>
        /// <remarks>The return value will be used perhaps for file operations, i.e. for writing a representation of the data into a specific file etc.</remarks>
        GuidedExcelDataQuery IExcelDataQuery.AsCustomizeData()
        {
            return GetDeepCopy();
        }

        /// <summary>Returns a <see cref="System.String"/> representation of a specific entry.
        /// </summary>
        /// <param name="rowIndex">The null-based index of the row.</param>
        /// <param name="columnIndex">The null-based index of the column.</param>
        /// <returns>A <see cref="System.String"/> that represents the entry with the desired position.</returns>
        /// <remarks>This method is used for error reports etc. only, thus do not use this method to get the value of a specific Excel cell.</remarks>
        public string ToString(int rowIndex, int columnIndex)
        {
            if ((rowIndex < m_Data.Count) && (rowIndex >= 0) && (columnIndex < m_ColumnCount) && (columnIndex >= 0))
            {
                object data = m_Data[rowIndex][columnIndex];

                if ((data == null) || (data == Type.Missing) || (data is ExcelEmpty) || (data is ExcelMissing) || ((data is String) && (((string)data).Length == 0)))
                {
                    return "<empty>";
                }
                return data.ToString();
            }
            throw new ArgumentException("Invalid cell position, row = " + rowIndex + " column = " + columnIndex + ".");
        }

        /// <summary>Gets the null-based row index of a specific property.
        /// </summary>
        /// <param name="propertyName">The name of the property to search (in the first column).</param>
        /// <param name="rowIndex">The null-based index of the row which contains the property (output).</param>
        /// <param name="dataAdvice">Data advice, i.e. a list of possible outcome, perhaps <c>null</c> (This parameter will be ignored in this implementation).</param>
        /// <returns>A value indicating whether <paramref name="rowIndex"/> contains valid data.</returns>
        public bool TryGetRowIndexOfPropertyName(string propertyName, out int rowIndex, IExcelDataAdvice dataAdvice = null)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException("propertyName");
            }
            string idPropertyName = propertyName.ToIDString();
            for (int j = 0; j < m_Data.Count; j++)
            {
                object data = m_Data[j][0];
                if (data is String)
                {
                    if (((string)data).ToIDString() == idPropertyName)
                    {
                        rowIndex = j;
                        return true;
                    }
                }
            }
            rowIndex = -1;
            return false;
        }

        /// <summary>Gets the value of a specific Excel cell with respect to specific row and column index.
        /// </summary>
        /// <typeparam name="T">The type of the output.</typeparam>
        /// <param name="rowIndex">The null-based index of the row.</param>
        /// <param name="columnIndex">The null-based index of the column.</param>
        /// <param name="value">The value (output).</param>
        /// <param name="dataAdvice">Data advice, i.e. a list of possible outcome, perhaps <c>null</c> (This parameter will be ignored in this implementation).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        /// <exception cref="ArgumentException">Thrown, if <typeparamref name="T"/> represents a enumeration.</exception>
        public ExcelCellValueState TryGetValue<T>(out T value, int rowIndex, int columnIndex, IExcelDataAdvice dataAdvice = null)
        {
            if ((rowIndex < m_Data.Count) && (rowIndex >= 0) && (columnIndex < m_ColumnCount) && (columnIndex >= 0))
            {
                object data = m_Data[rowIndex][columnIndex];
                if ((data == null) || (data == Type.Missing) || (data is ExcelEmpty) || (data is ExcelMissing) || ((data is String) && (((string)data).Length == 0)))
                {
                    value = default(T);
                    return ExcelCellValueState.EmptyOrMissingExcelCell;
                }
                else if (data is T)
                {
                    value = (T)data;
                    return ExcelCellValueState.ProperValue;
                }
            }
            value = default(T);
            return ExcelCellValueState.NoValidValue;
        }

        /// <summary>Gets the value of a specific property with respect to a null-based (row or column) index.
        /// </summary>
        /// <typeparam name="TEnum">The type of the output which is assumed to be a enumeration.</typeparam>
        /// <param name="rowIndex">The null-based index of the row.</param>
        /// <param name="columnIndex">The null-based index of the column.</param>
        /// <param name="enumStringRepresentationUsage">The method how to compute the <see cref="System.String"/> representation.</param>
        /// <param name="value">The value (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        /// <exception cref="ArgumentException">Thrown, if <typeparamref name="TEnum"/> does not represents a enumeration.</exception>
        public ExcelCellValueState TryGetValue<TEnum>(EnumStringRepresentationUsage enumStringRepresentationUsage, out TEnum value, int rowIndex, int columnIndex)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            if ((rowIndex < m_Data.Count) && (rowIndex >= 0) && (columnIndex < m_ColumnCount) && (columnIndex >= 0))
            {
                object data = m_Data[rowIndex][columnIndex];
                if ((data == null) || (data == Type.Missing) || (data is ExcelEmpty) || (data is ExcelMissing) || ((data is String) && (((string)data).Length == 0)))
                {
                    value = default(TEnum);
                    return ExcelCellValueState.EmptyOrMissingExcelCell;
                }
                else if (data is TEnum)
                {
                    value = (TEnum)data;
                    return ExcelCellValueState.ProperValue;
                }
            }
            value = default(TEnum);
            return ExcelCellValueState.NoValidValue;
        }

        /// <summary>Finalize the current <see cref="IExcelDataQuery"/> instance.
        /// </summary>
        /// <param name="throwExceptionIfDataIsNotUsed">A value indicating whether an exception will be thrown, if some of the data are are not queried by the user.</param>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="throwExceptionIfDataIsNotUsed"/> is <c>true</c> and a property or table entry has been detected which
        /// is not queried, i.e. not used.</exception>
        /// <remarks>Call this method before calling <see cref="IExcelDataQuery.AsCustomizeData()"/> to check the user input; perhaps the user has enter properties or values
        /// which are not used by the program and an exception will be shown to indicate wrong user input.</remarks>
        public void QueryCompleted(bool throwExceptionIfDataIsNotUsed = true)
        {
        }
        #endregion

        /// <summary>Determines whether a specific Excel cell is empty.
        /// </summary>
        /// <param name="rowIndex">The null-based index of the row.</param>
        /// <param name="columnIndex">The null-based index of the column.</param>
        /// <param name="emptyExcelCellMode">The method how to interpret whether a specific Excel cell is empty.</param>
        /// <returns><c>true</c> if the Excel cell at the specific position is empty; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentException">Thrown, if the row/column position is not valid.</exception>
        public bool IsEmptyExcelCell(int rowIndex, int columnIndex, eEmptyExcelCellMode emptyExcelCellMode = eEmptyExcelCellMode.Standard)
        {
            if ((rowIndex < m_Data.Count) && (rowIndex >= 0) && (columnIndex < m_ColumnCount) && (columnIndex >= 0))
            {
                return IsEmptyExcelCell(m_Data[rowIndex][columnIndex], rowIndex, columnIndex, emptyExcelCellMode);
            }
            throw new ArgumentException("Invalid cell position, row = " + rowIndex + " column = " + columnIndex + ".");
        }

        /// <summary>Determines whether a specific row is empty, i.e. contains <see cref="Type.Missing"/> only.
        /// </summary>
        /// <param name="rowIndex">The null-based index of the row.</param>
        /// <param name="emptyExcelCellMode">The method how to interpret whether a specific Excel cell is empty.</param>
        /// <returns><c>true</c> if the specified row is empty; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="rowIndex"/> does not represents a valid row index.</exception>
        public bool IsEmptyRow(int rowIndex, eEmptyExcelCellMode emptyExcelCellMode = eEmptyExcelCellMode.Standard)
        {
            if ((rowIndex >= 0) && (rowIndex < m_Data.Count))
            {
                object[] row = m_Data[rowIndex];

                int j = 0;
                while (j < m_ColumnCount)
                {
                    object data = row[j];

                    if (IsEmptyExcelCell(data, rowIndex, j, emptyExcelCellMode) == false)
                    {
                        return false;
                    }
                    j++;
                }
                return true;
            }
            throw new ArgumentException("Invalid cell position, row = " + rowIndex + ".");
        }

        /// <summary>Gets the number of non-empty rows.
        /// </summary>
        /// <param name="emptyExcelCellMode">The method how to interpret whether a specific Excel cell is empty.</param>
        /// <returns>The number of non-empty rows.</returns>
        public int GetNonEmptyRowCount(eEmptyExcelCellMode emptyExcelCellMode = eEmptyExcelCellMode.Standard)
        {
            int rowCount = 0;
            for (int j = 0; j < m_Data.Count; j++)
            {
                if (IsEmptyRow(j, emptyExcelCellMode) == false)
                {
                    rowCount++;
                }
            }
            return rowCount;
        }

        /// <summary>Adds an empty row into the Range, i.e. a new row where each element is equal to <see cref="Type.Missing"/>.
        /// </summary>
        public void AddRow()
        {
            object[] row = new object[m_ColumnCount];

            for (int j = 0; j < m_ColumnCount; j++)
            {
                row[j] = Type.Missing;
            }
            m_Data.Add(row);
        }

        /// <summary>Adds a specific row into the Range.
        /// </summary>
        /// <param name="data">The data, where the first component is a value or the <see cref="System.Type"/> representation, missing values are equal to <see cref="System.Type.Missing"/>;
        /// the second component represent a optional data advice.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="data"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown, if the length of <paramref name="data"/> is not equal to <see cref="ColumnCount"/>.</exception>
        public void AddRow(params Tuple<object, IExcelDataAdvice>[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (data.Length != m_ColumnCount)
            {
                throw new ArgumentException("data");
            }
            object[] row = new object[m_ColumnCount];

            int rowIndex = RowCount - 1;
            for (int j = 0; j < m_ColumnCount; j++)
            {
                var dataItem = data[j];

                row[j] = dataItem.Item1;
                if (dataItem.Item2 != null)
                {
                    SetDataAdvice(rowIndex, j, dataItem.Item2);
                }
            }
            m_Data.Add(row);
        }

        /// <summary>Gets the data at a specific position, i.e. the value or if no value is available 
        /// the <see cref="System.Type"/> representation.
        /// </summary>
        /// <param name="rowIndex">The null-based index of the row.</param>
        /// <param name="columnIndex">The null-based index of the column.</param>
        /// <returns>The value of the Excel cell at the desired position if available, otherwise the <see cref="System.Type"/> representation, perhaps <see cref="System.Type.Missing"/>.</returns>
        /// <exception cref="ArgumentException">Thrown, if the row/column position is not valid.</exception>
        public object GetData(int rowIndex, int columnIndex)
        {
            if ((rowIndex < m_Data.Count) && (rowIndex >= 0) && (columnIndex < m_ColumnCount) && (columnIndex >= 0))
            {
                return m_Data[rowIndex][columnIndex];
            }
            throw new ArgumentException("Invalid cell position, row = " + rowIndex + " column = " + columnIndex + ".");
        }

        /// <summary>Gets the data at a specific position in a Excel specific cell format.
        /// </summary>
        /// <param name="rowIndex">The null-based index of the row.</param>
        /// <param name="columnIndex">The null-based index of the column.</param>
        /// <returns>The value of the Excel cell at the desired position if available, perhaps an empty string.</returns>
        /// <exception cref="ArgumentException">Thrown, if the row/column position is not valid.</exception>
        public object GetExcelData(int rowIndex, int columnIndex)
        {
            if ((rowIndex < m_Data.Count) && (rowIndex >= 0) && (columnIndex < m_ColumnCount) && (columnIndex >= 0))
            {
                object data = m_Data[rowIndex][columnIndex];
                if (data == Type.Missing)
                {
                    return String.Empty;
                }
                else if (data is Enum)
                {
                    return ((Enum)data).ToFormatString(EnumStringRepresentationUsage.StringAttribute);
                }
                else if ((data.GetType().IsPrimitive) || (data is DateTime) || (data is string))
                {
                    return data;
                }
                return data.ToString();
            }
            throw new ArgumentException("Invalid cell position, row = " + rowIndex + " column = " + columnIndex + ".");
        }

        /// <summary>Gets the <see cref="IExcelDataAdvice"/> object with respect to a specific position.
        /// </summary>
        /// <param name="rowIndex">The null-based index of the row.</param>
        /// <param name="columnIndex">The null-based index of the column.</param>
        /// <param name="dataAdvice">The <see cref="IExcelDataAdvice"/> object which contains possible outcome which is used to improve the useability of the GUI (output).</param>
        /// <returns>A value indicating whether <paramref name="dataAdvice"/> contains </returns>
        public bool TryGetDataAdvice(int rowIndex, int columnIndex, out IExcelDataAdvice dataAdvice)
        {
            if ((rowIndex < m_Data.Count) && (rowIndex >= 0) && (columnIndex < m_ColumnCount) && (columnIndex >= 0))
            {
                return m_DataAdvice.TryGetValue(rowIndex * m_ColumnCount + columnIndex, out dataAdvice);
            }
            dataAdvice = null;
            return false;
        }

        /// <summary>Resize the <see cref="GuidedExcelDataQuery"/>, i.e. add additional columns.
        /// </summary>
        /// <param name="numberOfColumnsToAdd">The number of columns to add.</param>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="numberOfColumnsToAdd"/> is less or equal <c>0</c>.</exception>
        /// <remarks>The number of rows remains unchanged, use <see cref="AddRow()"/> to add additional rows.</remarks>
        public void AddColumn(int numberOfColumnsToAdd = 1)
        {
            if (numberOfColumnsToAdd <= 0)
            {
                throw new ArgumentException(String.Format(ExceptionMessages.ArgumentOutOfRangeGreater, "The number of columns to add", 1), "numberOfColumnsToAdd");
            }

            int newColumnCount = m_ColumnCount + numberOfColumnsToAdd;
            for (int k = 0; k < m_Data.Count; k++)
            {
                object[] newRow = new object[newColumnCount];
                m_Data[k].CopyTo(newRow, m_ColumnCount);

                for (int j = m_ColumnCount; j < newColumnCount; j++)
                {
                    newRow[j] = Type.Missing;
                }
                m_Data[k] = newRow;
            }
            m_ColumnCount = newColumnCount;
        }

        /// <summary>Stores a <see cref="IExcelDataAdvice"/> object at a specific position.
        /// </summary>
        /// <param name="rowIndex">The null-based index of the row.</param>
        /// <param name="columnIndex">The null-based index of the column.</param>
        /// <param name="dataAdvice">The data advice, i.e. a collection of possible outcome to improve the useability of the GUI.
        /// <c>null</c> is allowed.</param>
        public void SetDataAdvice(int rowIndex, int columnIndex, IExcelDataAdvice dataAdvice)
        {
            if (dataAdvice != null)
            {
                int key = rowIndex * m_ColumnCount + columnIndex;
                if (m_DataAdvice.ContainsKey(key))
                {
                    m_DataAdvice[key] = dataAdvice;
                }
                else
                {
                    m_DataAdvice.Add(key, dataAdvice);
                }
            }
        }

        /// <summary>Sets the data of a specific entry (i.e. imaginary Excel cell), thus the value of the entry or if no value available, the <see cref="System.Type"/> representation,
        /// perhaps <see cref="System.Type.Missing"/>.
        /// </summary>
        /// <param name="rowIndex">The null-based index of the row.</param>
        /// <param name="columnIndex">The null-based index of the column.</param>
        /// <param name="data">The data, i.e. the value of the entry or if no value available, the <see cref="System.Type"/> representation,
        /// perhaps <see cref="System.Type.Missing"/>.</param>
        public void SetData(int rowIndex, int columnIndex, object data)
        {
            if ((rowIndex < m_Data.Count) && (rowIndex >= 0) && (columnIndex < m_ColumnCount) && (columnIndex >= 0))
            {
                m_Data[rowIndex][columnIndex] = data;
            }
            else
            {
                throw new ArgumentException("Invalid Excel cell position, row = " + rowIndex + " column = " + columnIndex + ".");
            }
        }

        /// <summary>Adds the name of a specific property which is queried by the GUI, but not given by the user.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <remarks>Use this method to store optional property names to improve the useability.</remarks>
        public void AddUnusedPropertyName(string propertyName)
        {
            m_UnusedOptionalPropertyNames.Add(propertyName);
        }

        /// <summary>Gets the collection of property names which are queried by the GUI, but not given by the user.
        /// </summary>
        /// <returns>The collection of property names which are queried by the GUI, but not given by the user.</returns>
        /// <remarks>Use this method to store optional property names to improve the useability.</remarks>
        public IEnumerable<string> GetUnusedPropertyNames()
        {
            return m_UnusedOptionalPropertyNames;
        }

        /// <summary>Removes a specific property name of <see cref="GetUnusedPropertyNames()"/>.
        /// </summary>
        /// <param name="propertyName">The name of the property to remove.</param>
        /// <remarks>This method does change the result of <see cref="GetUnusedPropertyNames()"/> but
        /// it does not remove or change any cell.</remarks>
        public void RemoveUnusedPropertyName(string propertyName)
        {
            m_UnusedOptionalPropertyNames.Remove(propertyName);
        }

        /// <summary>Gets a deep copy of the current instance.
        /// </summary>
        /// <returns>A deep copy of the current instance.</returns>
        public GuidedExcelDataQuery GetDeepCopy()
        {
            return new GuidedExcelDataQuery(this);
        }
        #endregion

        #region private methods

        /// <summary>Determines whether a specific Excel cell is empty.
        /// </summary>
        /// <param name="excelCellData">The data of the Excel cell.</param>
        /// <param name="rowIndex">The null-based index of the row.</param>
        /// <param name="columnIndex">The null-based index of the column.</param>
        /// <param name="emptyExcelCellMode">The method how to interpret whether a specific Excel cell is empty.</param>
        /// <returns><c>true</c> if the Excel cell at the specific position is empty; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentException">Thrown, if the row/column position is not valid.</exception>
        public bool IsEmptyExcelCell(object excelCellData, int rowIndex, int columnIndex, eEmptyExcelCellMode emptyExcelCellMode = eEmptyExcelCellMode.IsNull | eEmptyExcelCellMode.HasMissingType | eEmptyExcelCellMode.IsEmptyExcelCell)
        {
            if (emptyExcelCellMode.HasFlag(eEmptyExcelCellMode.IsNull) && (excelCellData == null))
            {
                return true;
            }
            else if (emptyExcelCellMode.HasFlag(eEmptyExcelCellMode.HasMissingType) && ((excelCellData == Type.Missing) || (excelCellData is System.Reflection.Missing)))
            {
                return true;
            }
            else if (emptyExcelCellMode.HasFlag(eEmptyExcelCellMode.IsEmptyExcelCell) && (excelCellData is ExcelEmpty))
            {
                return true;
            }
            else if (emptyExcelCellMode.HasFlag(eEmptyExcelCellMode.ContainsEmptyString) && (excelCellData is string) && (((string)excelCellData).Length == 0))
            {
                return true;
            }

            bool isType = (excelCellData is Type);
            bool hasDataAdvice = m_DataAdvice.ContainsKey(m_ColumnCount * rowIndex + columnIndex);

            if (emptyExcelCellMode.HasFlag(eEmptyExcelCellMode.HasSpecificTypeWithoutAdvice) && isType && (hasDataAdvice == false))
            {
                return true;
            }
            else if (emptyExcelCellMode.HasFlag(eEmptyExcelCellMode.HasSpecificTypeWithAdvice) && isType && hasDataAdvice)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}