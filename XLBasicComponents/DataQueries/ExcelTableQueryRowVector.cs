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
using System.Linq;
using System.Collections.Generic;

using ExcelDna.Integration;
using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Utilities;

namespace Dodoni.XLBasicComponents
{
    /// <summary>Represents a Excel Range which contains exactly one row, i.e. a row vector, which is transposed by this implementation.
    /// </summary>
    internal class ExcelTableQueryRowVector : IExcelDataQuery
    {
        #region private members

        /// <summary>The name of the <see cref="IExcelDataQuery"/>.
        /// </summary>
        private IdentifierString m_Name;

        /// <summary>The Excel Range.
        /// </summary>
        private ExcelReference m_Range;

        /// <summary>The data of the Excel Range.
        /// </summary>
        private object[,] m_Data;

        /// <summary>Internally, this is the number of columns, externally, it is the number of rows.
        /// </summary>
        private int m_Count;

        /// <summary>The set of null-based indices which represents the used rows.
        /// </summary>
        private HashSet<int> m_SetOfUsedPropertyIndices = new HashSet<int>();

        /// <summary>The set of optional property names which are not available by the current instance.
        /// </summary>
        private SortedSet<string> m_UnusedOptionalPropertyNames = new SortedSet<string>();

        /// <summary>The customize data representation.
        /// </summary>
        private GuidedExcelDataQuery m_GuidedExcelDataQuery;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="ExcelTableQueryRowVector"/> class.
        /// </summary>
        /// <param name="excelReference">The Excel Range.</param>
        /// <param name="excelDataQueryName">The name of the Excel data query.</param>
        /// <exception cref="ArgumentException">Thrown, if the value of <paramref name="excelReference"/> is not
        /// represented by a row-vector.</exception>
        public ExcelTableQueryRowVector(ExcelReference excelReference, string excelDataQueryName)
        {
            if (excelReference == null)
            {
                throw new ArgumentNullException("excelRangeValue");
            }
            m_Range = excelReference;
            object value = m_Range.GetValue();
            if (value is object[,])
            {
                m_Data = (object[,])value;

                if (m_Data.GetLength(0) != 1)
                {
                    throw new ArgumentException("A row vector required", "excelRangeValue");
                }
                m_Count = m_Data.GetLength(1);
            }
            else
            {
                throw new ArgumentException("excelRangeValue");
            }

            if (excelDataQueryName == null)
            {
                throw new ArgumentNullException("excelDataQueryName");
            }
            m_Name = new IdentifierString(excelDataQueryName);
            m_GuidedExcelDataQuery = new GuidedExcelDataQuery(excelDataQueryName, rowCount: m_Count, columnCount: 1);
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
            get { return m_Count; }
        }

        /// <summary>Gets the number of columns.
        /// </summary>
        /// <value>The number columns.</value>
        public int ColumnCount
        {
            get { return 1; }
        }

        /// <summary>Gets a value indicating whether the <see cref="IExcelDataQuery"/> object is just a single Excel cell.
        /// </summary>
        /// <value><c>true</c> if this instance is single cell; otherwise, <c>false</c>.
        /// </value>
        public bool IsSingleCell
        {
            get { return (m_Count == 1); }
        }

        /// <summary>Gets a value indicating whether the current instance is empty, i.e. <see cref="IExcelDataQuery.RowCount"/> = <see cref="IExcelDataQuery.ColumnCount"/> = 0.
        /// </summary>
        /// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
        public bool IsEmpty
        {
            get { return false; }
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
            if ((rowIndex < m_Count) && (rowIndex >= 0) && (columnIndex == 0))
            {
                return ExcelDataConverter.IsEmptyCell(m_Data[0, rowIndex]);
            }
            throw new ArgumentException("Invalid Excel cell position, row = " + rowIndex + " column = " + columnIndex + ".");
        }

        /// <summary>Gets customize data, i.e. a representation of the current <see cref="IExcelDataQuery"/> object as a specific <see cref="GuidedExcelDataQuery"/> instance.
        /// </summary>
        /// <returns>A representation of the data which contains the type of each property etc.</returns>
        /// <remarks>The return value will be used perhaps for file operations, i.e. for writing a representation of the data into a specific file etc.</remarks>
        public GuidedExcelDataQuery AsCustomizeData()
        {
            return m_GuidedExcelDataQuery.GetDeepCopy();
        }

        /// <summary>Returns a <see cref="System.String"/> representation of a specific entry.
        /// </summary>
        /// <param name="rowIndex">The null-based index of the row.</param>
        /// <param name="columnIndex">The null-based index of the column.</param>
        /// <returns>A <see cref="System.String"/> that represents the entry with the desired position.</returns>
        /// <remarks>This method is used for error reports etc. only, thus do not use this method to get the value of a specific Excel cell.</remarks>
        public string ToString(int rowIndex, int columnIndex)
        {
            if ((rowIndex < m_Count) && (rowIndex >= 0) && (columnIndex == 0))
            {
                return ExcelDataConverter.IsEmptyCell(m_Data[0, rowIndex]) ? "<empty>" : m_Data[0, rowIndex].ToString();
            }
            throw new ArgumentException("Invalid Excel cell position, row = " + rowIndex + " column = " + columnIndex + ".");
        }

        /// <summary>Gets the null-based row index of a specific property.
        /// </summary>
        /// <param name="propertyName">The name of the property to search (in the first column).</param>
        /// <param name="rowIndex">The null-based index of the row which contains the property (output).</param>
        /// <param name="dataAdvice">Data advice, i.e. a list of possible outcome, perhaps <c>null</c>.</param>
        /// <returns>A value indicating whether <paramref name="rowIndex"/> contains valid data.</returns>
        public bool TryGetRowIndexOfPropertyName(string propertyName, out int rowIndex, IExcelDataAdvice dataAdvice = null)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException("propertyName");
            }
            string idPropertyName = propertyName.ToIDString();
            for (int j = 0; j < m_Count; j++)
            {
                if (m_Data[0, j] is String)
                {
                    string cellName = (string)m_Data[0, j];

                    if (cellName.ToIDString() == idPropertyName)
                    {
                        rowIndex = j;
                        if (dataAdvice != null)
                        {
                            ExcelLowLevel.CreateDropdownList(m_Range, 0, j, dataAdvice.AsExcelDropDownListString());
                        }
                        m_SetOfUsedPropertyIndices.Add(rowIndex);
                        m_GuidedExcelDataQuery.SetData(rowIndex, 0, cellName);
                        m_GuidedExcelDataQuery.SetDataAdvice(rowIndex, 0, dataAdvice);
                        return true;
                    }
                }
            }
            m_UnusedOptionalPropertyNames.Add(propertyName);
            m_GuidedExcelDataQuery.AddUnusedPropertyName(propertyName);
            rowIndex = -1;
            return false;
        }

        /// <summary>Gets the value of a specific Excel cell with respect to specific row and column index.
        /// </summary>
        /// <typeparam name="T">The type of the output.</typeparam>
        /// <param name="rowIndex">The null-based index of the row.</param>
        /// <param name="columnIndex">The null-based index of the column.</param>
        /// <param name="value">The value (output).</param>
        /// <param name="dataAdvice">Data advice, i.e. a list of possible outcome, perhaps <c>null</c>.</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        /// <exception cref="ArgumentException">Thrown, if <typeparamref name="T"/> represents a enumeration.</exception>
        public ExcelCellValueState TryGetValue<T>(out T value, int rowIndex, int columnIndex, IExcelDataAdvice dataAdvice = null)
        {
            if ((rowIndex < m_Count) && (rowIndex >= 0) && (columnIndex == 0))
            {
                m_SetOfUsedPropertyIndices.Add(rowIndex);

                if (dataAdvice != null)
                {
                    m_Range.CreateDropdownList(0, rowIndex, dataAdvice.AsExcelDropDownListString());
                }
                m_GuidedExcelDataQuery.SetDataAdvice(rowIndex, columnIndex, dataAdvice);

                if (ExcelDataConverter.IsEmptyCell(m_Data[0, rowIndex]) == true)
                {
                    value = default(T);
                    m_GuidedExcelDataQuery.SetData(rowIndex, columnIndex, typeof(T));
                    return ExcelCellValueState.EmptyOrMissingExcelCell;
                }
                else if (ExcelDataConverter.TryGetCellValue<T>(m_Data[0, rowIndex], out value) == true)
                {
                    m_GuidedExcelDataQuery.SetData(rowIndex, columnIndex, value);
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
            if ((rowIndex < m_Count) && (rowIndex >= 0) && (columnIndex == 0))
            {
                string valueDropDownListAsString = EnumString<TEnum>.GetValues(enumStringRepresentationUsage).AsExcelDropDownListString();
                m_SetOfUsedPropertyIndices.Add(rowIndex);

                m_Range.CreateDropdownList(0, rowIndex, valueDropDownListAsString);
                if (ExcelDataConverter.IsEmptyCell(m_Data[0, rowIndex]) == true)
                {
                    value = default(TEnum);
                    m_GuidedExcelDataQuery.SetData(rowIndex, columnIndex, typeof(TEnum));
                    return ExcelCellValueState.EmptyOrMissingExcelCell;
                }
                else if (ExcelDataConverter.TryGetCellValue<TEnum>(m_Data[0, rowIndex], enumStringRepresentationUsage, out value) == true)
                {
                    m_GuidedExcelDataQuery.SetData(rowIndex, columnIndex, value);
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
            if (throwExceptionIfDataIsNotUsed)
            {
                string unusedOptionalPropertyNameDropDownList = null;
                if (m_UnusedOptionalPropertyNames.Count > 0)
                {
                    unusedOptionalPropertyNameDropDownList = m_UnusedOptionalPropertyNames.AsExcelDropDownListString();
                }
                for (int j = 0; j < m_Count; j++)
                {
                    if (m_SetOfUsedPropertyIndices.Contains(j) == false)
                    {
                        if ((m_Data == null) && (ExcelDataConverter.IsEmptyCell(m_Data[0, j]) == false))
                        {
                            throw new ArgumentException(j + " th row not used.");
                        }
                        else
                        {
                            m_Range.CreateDropdownList(0, j, unusedOptionalPropertyNameDropDownList);
                        }
                    }
                }
            }
        }
        #endregion

        #endregion
    }
}