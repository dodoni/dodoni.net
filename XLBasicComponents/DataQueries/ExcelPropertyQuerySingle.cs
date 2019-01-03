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
    /// <summary>Represents a Excel property, i.e. property names/values, which contains a single property only.
    /// </summary>
    internal class ExcelPropertyQuerySingle : IExcelDataQuery
    {
        #region private members

        /// <summary>The name of the <see cref="IExcelDataQuery"/>.
        /// </summary>
        private IdentifierString m_Name;

        /// <summary>The name of the property, given in its <see cref="System.String"/> or <see cref="ExcelReference"/> representation.
        /// </summary>
        private object m_PropertyName;

        /// <summary>The value of the property.
        /// </summary>
        private object m_PropertyValue;

        /// <summary>The customize data representation.
        /// </summary>
        private GuidedExcelDataQuery m_GuidedExcelDataQuery;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="ExcelPropertyQuerySingle"/> class.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="propertyValue">The value of the property.</param>
        /// <param name="excelDataQueryName">The name of the Excel data query.</param> 
        public ExcelPropertyQuerySingle(object propertyName, object propertyValue, string excelDataQueryName)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException("propertyName");
            }
            if (propertyValue == null)
            {
                throw new ArgumentNullException("propertyValue");
            }
            if (((propertyName is String) == false) && ((propertyName is ExcelReference) == false))
            {
                throw new ArgumentException("Invalid Property name input.");
            }
            m_PropertyName = propertyName;
            m_PropertyValue = propertyValue;

            if (excelDataQueryName == null)
            {
                throw new ArgumentNullException("excelDataQueryName");
            }
            m_Name = new IdentifierString(excelDataQueryName);
            m_GuidedExcelDataQuery = new GuidedExcelDataQuery(excelDataQueryName, rowCount: 1);
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

        #region IExcelDataQuery Members

        /// <summary>Gets the number of rows.
        /// </summary>
        /// <value>The number of rows.</value>
        public int RowCount
        {
            get { return 1; }
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
            get { return false; }
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

        #region IExcelDataQuery Members

        /// <summary>Determines whether a specific Excel cell is empty.
        /// </summary>
        /// <param name="rowIndex">The null-based index of the row.</param>
        /// <param name="columnIndex">The null-based index of the column.</param>
        /// <returns><c>true</c> if the Excel cell at the specific position is empty; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentException">Thrown, if the row/column position is not valid.</exception>
        public bool IsEmptyExcelCell(int rowIndex, int columnIndex)
        {
            if (rowIndex == 0)
            {
                if (columnIndex == 0)
                {
                    return ExcelDataConverter.IsEmptyCell(m_PropertyName);
                }
                else if (columnIndex == 1)
                {
                    return ExcelDataConverter.IsEmptyCell(m_PropertyValue);
                }
            }
            throw new ArgumentException("Invalid Excel cell position, row = " + rowIndex + " column = " + columnIndex + ".");
        }

        /// <summary>Gets customize data, i.e. a representation of the current <see cref="IExcelDataQuery"/>
        /// object as a specific <see cref="GuidedExcelDataQuery"/> instance.
        /// </summary>
        /// <returns>A representation of the data which contains the type of each property etc.</returns>
        /// <remarks>The return value will be used perhaps for file operations, i.e. for writing a representation of the
        /// data into a specific file etc.</remarks>
        public GuidedExcelDataQuery AsCustomizeData()
        {
            return m_GuidedExcelDataQuery.GetDeepCopy();
        }

        /// <summary>Returns a <see cref="System.String"/> representation of a specific entry.
        /// </summary>
        /// <param name="rowIndex">The null-based index of the row.</param>
        /// <param name="columnIndex">The null-based index of the column.</param>
        /// <returns>A <see cref="System.String"/> that represents the entry with the desired position.</returns>
        /// <remarks>This method is used for error reports etc. only, thus do not use this method to
        /// get the value of a specific Excel cell.</remarks>
        public string ToString(int rowIndex, int columnIndex)
        {
            if (rowIndex == 0)
            {
                if (columnIndex == 0)
                {
                    return ExcelDataConverter.IsEmptyCell(m_PropertyName) ? "<empty>" : m_PropertyName.ToString();
                }
                else if (columnIndex == 1)
                {
                    return ExcelDataConverter.IsEmptyCell(m_PropertyValue) ? "<empty>" : m_PropertyValue.ToString();
                }
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
            string thisPropertyName;
            if (TryGetValue<string>(m_PropertyName, out thisPropertyName, (dataAdvice != null) ? dataAdvice.AsExcelDropDownListString() : null) == ExcelCellValueState.ProperValue)
            {
                if (propertyName.ToIDString() == thisPropertyName.ToIDString())
                {
                    m_GuidedExcelDataQuery.SetData(0, 0, propertyName);
                    m_GuidedExcelDataQuery.SetDataAdvice(0, 0, dataAdvice);
                    rowIndex = 0;
                    return true;
                }
            }
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
            if (typeof(T).IsEnum)
            {
                throw new ArgumentException("The type " + typeof(T).ToString() + " represents a enumeration which is not allowed.", "T");
            }

            if (rowIndex == 0)
            {
                ExcelCellValueState state = ExcelCellValueState.NoValidValue;

                if (columnIndex == 0)
                {
                    state = TryGetValue<T>(m_PropertyName, out value, (dataAdvice != null) ? dataAdvice.AsExcelDropDownListString() : null);
                    m_GuidedExcelDataQuery.SetDataAdvice(rowIndex, columnIndex, dataAdvice);
                }
                else if (columnIndex == 1)
                {
                    state = TryGetValue<T>(m_PropertyValue, out value, (dataAdvice != null) ? dataAdvice.AsExcelDropDownListString() : null);
                    m_GuidedExcelDataQuery.SetDataAdvice(rowIndex, columnIndex, dataAdvice);
                }
                else
                {
                    value = default(T);
                    state = ExcelCellValueState.NoValidValue;
                }

                if (state == ExcelCellValueState.ProperValue)
                {
                    m_GuidedExcelDataQuery.SetData(rowIndex, columnIndex, value);
                }
                else if (state == ExcelCellValueState.EmptyOrMissingExcelCell)
                {
                    m_GuidedExcelDataQuery.SetData(rowIndex, columnIndex, typeof(T));
                }
                return state;
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
            if (typeof(TEnum).IsEnum == false)
            {
                throw new ArgumentException("The type " + typeof(TEnum).ToString() + " does not represents a enumeration.", "TEnum");
            }

            if (rowIndex == 0)
            {
                ExcelCellValueState state = ExcelCellValueState.NoValidValue;
                if (columnIndex == 0)
                {
                    state = TryGetValue<TEnum>(m_PropertyName, out value, enumStringRepresentationUsage);
                }
                else if (columnIndex == 1)
                {
                    state = TryGetValue<TEnum>(m_PropertyValue, out value, enumStringRepresentationUsage);
                }
                else
                {
                    value = default(TEnum);
                    state = ExcelCellValueState.NoValidValue;
                }
                if (state == ExcelCellValueState.ProperValue)
                {
                    m_GuidedExcelDataQuery.SetData(rowIndex, columnIndex, value);
                }
                else if (state == ExcelCellValueState.EmptyOrMissingExcelCell)
                {
                    m_GuidedExcelDataQuery.SetData(rowIndex, columnIndex, typeof(TEnum));
                }
                return state;
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

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return String.Format("{0} : {1}", ExcelDataConverter.IsEmptyCell(m_PropertyName) ? "<empty>" : m_PropertyName.ToString(), ExcelDataConverter.IsEmptyCell(m_PropertyValue) ? "<empty>" : m_PropertyValue.ToString());
        }
        #endregion

        #region private methods

        /// <summary>Gets the value of a specific Excel cell.
        /// </summary>
        /// <typeparam name="T">The type of the output.</typeparam>
        /// <param name="inputCell">The input Excel cell.</param>
        /// <param name="value">The value of the Excel cell (output).</param>
        /// <param name="valueDropDownListAsString">The optional semicolon separated <see cref="System.String"/> representation of the
        /// possible outcomes with respect to <paramref name="value"/>, i.e. if not <c>null</c> a drop down list will be added to the corresponding Excel cell.</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        private ExcelCellValueState TryGetValue<T>(object inputCell, out T value, string valueDropDownListAsString)
        {
            object excelCellValue;

            if (inputCell == null)
            {
                value = default(T);
                return ExcelCellValueState.NoValidValue;
            }
            else if (inputCell is ExcelReference)
            {
                ExcelReference excelRange = (ExcelReference)inputCell;
                excelRange.CreateDropdownList(0, 0, valueDropDownListAsString);

                excelCellValue = excelRange.GetValue();
            }
            else
            {
                excelCellValue = inputCell;
            }

            if (ExcelDataConverter.IsEmptyCell(excelCellValue) == true)
            {
                value = default(T);
                return ExcelCellValueState.EmptyOrMissingExcelCell;
            }
            return (ExcelDataConverter.TryGetCellValue<T>(excelCellValue, out value) == true) ? ExcelCellValueState.ProperValue : ExcelCellValueState.NoValidValue;
        }

        /// <summary>Gets the value of a specific Excel cell.
        /// </summary>
        /// <typeparam name="TEnum">The type of the output which is assumed to be a enumeration.</typeparam>
        /// <param name="inputCell">The input Excel cell.</param>
        /// <param name="value">The value of the Excel cell (output).</param>
        /// <param name="enumStringRepresentationUsage">The method how to compute the <see cref="System.String"/> representation.</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        /// <exception cref="ArgumentException">Thrown, if <typeparamref name="TEnum"/> does not represents a enumeration.</exception>
        private ExcelCellValueState TryGetValue<TEnum>(object inputCell, out TEnum value, EnumStringRepresentationUsage enumStringRepresentationUsage)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            object excelCellValue;

            if (inputCell == null)
            {
                value = default(TEnum);
                return ExcelCellValueState.NoValidValue;
            }
            else if (inputCell is ExcelReference)
            {
                ExcelReference excelRange = (ExcelReference)inputCell;
                string valueDropDownListAsString = EnumString<TEnum>.GetValues(enumStringRepresentationUsage).AsExcelDropDownListString();

                excelRange.CreateDropdownList(0, 0, valueDropDownListAsString);

                excelCellValue = excelRange.GetValue();
            }
            else
            {
                excelCellValue = inputCell;
            }

            if (ExcelDataConverter.IsEmptyCell(excelCellValue) == true)
            {
                value = default(TEnum);
                return ExcelCellValueState.EmptyOrMissingExcelCell;
            }
            return (ExcelDataConverter.TryGetCellValue<TEnum>(excelCellValue, enumStringRepresentationUsage, out value) == true) ? ExcelCellValueState.ProperValue : ExcelCellValueState.NoValidValue;
        }
        #endregion
    }
}