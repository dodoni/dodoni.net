/*
   Copyright (C) 2011-2014 Markus Wendt
   All rights reserved.
 
   Redistribution and use in source and binary forms, including commercial applications, with or without modification, 
   are permitted provided that the following conditions are met: 
 
   1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer. 
      Altered source versions must be plainly marked as such, and must not be misrepresented as being the original software.

   2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following 
      disclaimer in the documentation and/or other materials provided with the distribution. 

   3. If you use this software in a product, an acknowledgment (see the following) in the product documentation is required. 
      The end-user documentation included with the redistribution, if any, must include the following acknowledgment: 

      "Dodoni.net (http://www.dodoni-project.net/) Copyright (C) 2011-2012 Markus Wendt" 

      Alternately, this acknowledgment may appear in the software itself, if and wherever such third-party acknowledgments normally appear. 

   4. Neither the name 'Dodoni.net' nor the names of its contributors may be used to endorse or promote products 
      derived from this software without specific prior written permission. For written permission, please contact info<at>dodoni-project.net. 

   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, 
   BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
   SHALL THE COPYRIGHT HOLDERS OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
   DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
   INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE 
   OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 
   For more information, please see http://www.dodoni-project.net/.
 */
using System;
using System.Collections.Generic;

using ExcelDna.Integration;
using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Utilities;

namespace Dodoni.XLBasicComponents
{
    /// <summary>Represents a Excel Range specified by a header only, i.e. assuming that every row below a specified range ('header') can contain valid data. Therefore
    /// this <see cref="IExcelDataQuery"/> implementation represents almost all rows starting from a specific row index.
    /// </summary>
    internal class ExcelTableQueryHeader : IExcelDataQuery
    {
        #region private members

        /// <summary>The Excel Range which represents the 'header'.
        /// </summary>
        private ExcelReference m_HeaderRange;

        /// <summary>The data of the Excel Range which represents the 'header'.
        /// </summary>
        private object[,] m_HeaderData;

        /// <summary>The Excel Range below the 'header', perhaps up to the last available row on the specific Excel sheet.
        /// </summary>
        private ExcelReference m_BelowHeaderRange;

        /// <summary>The data of the Excel range below the 'header'.
        /// </summary>
        private object[,] m_BelowHeaderData;

        /// <summary>The customize data representation.
        /// </summary>
        private GuidedExcelDataQuery m_GuidedExcelDataQuery;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="ExcelTableQueryHeader"/> class.
        /// </summary>
        /// <param name="excelReference">The Excel Range ('header').</param>
        /// <param name="excelDataQueryName">The name of the Excel data query.</param>
        /// <exception cref="ArgumentException">Thrown, if the value of <paramref name="excelReference"/> is not represented by an two-dimensional array (for example a single row header) or a <see cref="System.String"/>.</exception>
        public ExcelTableQueryHeader(ExcelReference excelReference, string excelDataQueryName)
            : this(excelReference, excelDataQueryName, ExcelDnaUtil.ExcelLimits.MaxRows)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ExcelTableQueryHeader"/> class.
        /// </summary>
        /// <param name="excelReference">The Excel Range ('header').</param>
        /// <param name="excelDataQueryName">The name of the Excel data query.</param>
        /// <param name="maxRowCount">The maximal number of rows to take into account.</param>
        /// <exception cref="ArgumentException">Thrown, if the value of <paramref name="excelReference"/> is not represented by an two-dimensional array (for example a single row header) or a <see cref="System.String"/>.</exception>
        public ExcelTableQueryHeader(ExcelReference excelReference, string excelDataQueryName, int maxRowCount)
        {
            if (excelReference == null)
            {
                throw new ArgumentNullException("excelRangeValue");
            }

            m_HeaderRange = excelReference;
            m_BelowHeaderRange = new ExcelReference(m_HeaderRange.RowFirst + 1, maxRowCount - m_HeaderRange.RowLast - 1, m_HeaderRange.ColumnFirst, m_HeaderRange.ColumnLast, m_HeaderRange.SheetId);

            var value = m_HeaderRange.GetValue();
            if (value is object[,])
            {
                m_HeaderData = (object[,])value;
                RowCount = m_HeaderData.GetLength(0);
                ColumnCount = m_HeaderData.GetLength(1);
            }
            else if (value is String)
            {
                m_HeaderData = new object[1, 1];
                m_HeaderData[0, 0] = value as String;
                ColumnCount = 1;
            }
            else
            {
                throw new ArgumentException("excelRangeValue");
            }
            RowCount = (m_HeaderRange.RowLast - m_HeaderRange.RowFirst + 1) + (m_BelowHeaderRange.RowLast - m_BelowHeaderRange.RowFirst + 1);
            m_BelowHeaderData = m_BelowHeaderRange.GetValue() as object[,];

            if (excelDataQueryName == null)
            {
                throw new ArgumentNullException("excelDataQueryName");
            }
            Name = LongName = new IdentifierString(excelDataQueryName);
            m_GuidedExcelDataQuery = new GuidedExcelDataQuery(excelDataQueryName, rowCount: RowCount, columnCount: ColumnCount);
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the current instance.
        /// </summary>
        /// <value>The language independent name of the current instance.</value>
        public IdentifierString Name
        {
            get;
            private set;
        }

        /// <summary>Gets the long name of the current instance.
        /// </summary>
        /// <value>The language dependent long name of the current instance.</value>
        public IdentifierString LongName
        {
            get;
            private set;
        }
        #endregion

        #region IExcelDataQuery Member

        /// <summary>Gets the number of rows.
        /// </summary>
        /// <value>The number of rows.</value>
        public int RowCount
        {
            get;
            private set;
        }

        /// <summary>Gets the number of columns.
        /// </summary>
        /// <value>The number columns.</value>
        public int ColumnCount
        {
            get;
            private set;
        }

        /// <summary>Gets a value indicating whether the <see cref="IExcelDataQuery"/> object is just a single Excel cell.
        /// </summary>
        /// <value><c>true</c> if this instance is single cell; otherwise, <c>false</c>.
        /// </value>
        public bool IsSingleCell
        {
            get { return (RowCount == 1) && (ColumnCount == 1); }
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
            if ((rowIndex < RowCount) && (rowIndex >= 0) && (columnIndex < ColumnCount) && (columnIndex >= 0))
            {
                if (rowIndex < m_HeaderRange.RowLast - m_HeaderRange.RowFirst + 1)  // query the header
                {
                    return ExcelDataConverter.IsEmptyCell(m_HeaderData[rowIndex, columnIndex]);
                }
                else
                {
                    return ExcelDataConverter.IsEmptyCell(m_BelowHeaderData[rowIndex - (m_HeaderRange.RowLast - m_HeaderRange.RowFirst + 1), columnIndex]);
                }
            }
            throw new ArgumentException("Invalid Excel cell position, row = " + rowIndex + " column = " + columnIndex + ".");
        }

        /// <summary>Gets customize data, i.e. a representation of the current <see cref="IExcelDataQuery"/> object as a specific <see cref="GuidedExcelDataQuery"/> instance.
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
        /// <remarks>This method is used for error reports etc. only, thus do not use this method to get the value of a specific Excel cell.</remarks>
        public string ToString(int rowIndex, int columnIndex)
        {
            if ((rowIndex < RowCount) && (rowIndex >= 0) && (columnIndex < ColumnCount) && (columnIndex >= 0))
            {
                if (rowIndex < m_HeaderRange.RowLast - m_HeaderRange.RowFirst + 1)  // query the header
                {
                    return ExcelDataConverter.IsEmptyCell(m_HeaderData[rowIndex, columnIndex]) ? "<empty>" : m_HeaderData[rowIndex, columnIndex].ToString();
                }
                else
                {
                    int adjRowIndex = rowIndex - (m_HeaderRange.RowLast - m_HeaderRange.RowFirst + 1);
                    return ExcelDataConverter.IsEmptyCell(m_BelowHeaderData[adjRowIndex, columnIndex]) ? "<empty>" : m_BelowHeaderData[adjRowIndex, columnIndex].ToString();
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
            throw new InvalidOperationException("Property name/value collection not supported for this kind of Excel Range.");
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
            if ((rowIndex < RowCount) && (rowIndex >= 0) && (columnIndex < ColumnCount) && (columnIndex >= 0))
            {
                if (dataAdvice != null)
                {
                    m_HeaderRange.CreateDropdownList(rowIndex, columnIndex, dataAdvice.AsExcelDropDownListString());
                }
                m_GuidedExcelDataQuery.SetDataAdvice(rowIndex, columnIndex, dataAdvice);

                if (rowIndex < m_HeaderRange.RowLast - m_HeaderRange.RowFirst + 1)  // query the header
                {
                    if (ExcelDataConverter.IsEmptyCell(m_HeaderData[rowIndex, columnIndex]) == true)
                    {
                        value = default(T);
                        m_GuidedExcelDataQuery.SetData(rowIndex, columnIndex, typeof(T));
                        return ExcelCellValueState.EmptyOrMissingExcelCell;
                    }
                    else if (ExcelDataConverter.TryGetCellValue<T>(m_HeaderData[rowIndex, columnIndex], out value) == true)
                    {
                        m_GuidedExcelDataQuery.SetData(rowIndex, columnIndex, value);
                        return ExcelCellValueState.ProperValue;
                    }
                }
                else  // query below the header
                {
                    int adjRowIndex = rowIndex - (m_HeaderRange.RowLast - m_HeaderRange.RowFirst + 1);
                    if (ExcelDataConverter.IsEmptyCell(m_BelowHeaderData[adjRowIndex, columnIndex]) == true)
                    {
                        value = default(T);
                        m_GuidedExcelDataQuery.SetData(rowIndex, columnIndex, typeof(T));
                        return ExcelCellValueState.EmptyOrMissingExcelCell;
                    }
                    else if (ExcelDataConverter.TryGetCellValue<T>(m_BelowHeaderData[adjRowIndex, columnIndex], out value) == true)
                    {
                        m_GuidedExcelDataQuery.SetData(rowIndex, columnIndex, value);
                        return ExcelCellValueState.ProperValue;
                    }
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
            if ((rowIndex < RowCount) && (rowIndex >= 0) && (columnIndex < ColumnCount) && (columnIndex >= 0))
            {
                string valueDropDownListAsString = EnumString<TEnum>.GetValues(enumStringRepresentationUsage).AsExcelDropDownListString();

                if (rowIndex < m_HeaderRange.RowLast - m_HeaderRange.RowFirst + 1)  // query the header
                {
                    m_HeaderRange.CreateDropdownList(rowIndex, columnIndex, valueDropDownListAsString);
                    if (ExcelDataConverter.IsEmptyCell(m_HeaderData[rowIndex, columnIndex]) == true)
                    {
                        value = default(TEnum);
                        m_GuidedExcelDataQuery.SetData(rowIndex, columnIndex, typeof(TEnum));
                        return ExcelCellValueState.EmptyOrMissingExcelCell;
                    }
                    else if (ExcelDataConverter.TryGetCellValue<TEnum>(m_HeaderData[rowIndex, columnIndex], enumStringRepresentationUsage, out value) == true)
                    {
                        m_GuidedExcelDataQuery.SetData(rowIndex, columnIndex, value);
                        return ExcelCellValueState.ProperValue;
                    }
                }
                else  // query below the header
                {
                    int adjRowIndex = rowIndex - (m_HeaderRange.RowLast - m_HeaderRange.RowFirst + 1);

                    m_BelowHeaderRange.CreateDropdownList(adjRowIndex, columnIndex, valueDropDownListAsString);
                    if (ExcelDataConverter.IsEmptyCell(m_BelowHeaderData[adjRowIndex, columnIndex]) == true)
                    {
                        value = default(TEnum);
                        m_GuidedExcelDataQuery.SetData(rowIndex, columnIndex, typeof(TEnum));
                        return ExcelCellValueState.EmptyOrMissingExcelCell;
                    }
                    else if (ExcelDataConverter.TryGetCellValue<TEnum>(m_BelowHeaderData[adjRowIndex, columnIndex], enumStringRepresentationUsage, out value) == true)
                    {
                        m_GuidedExcelDataQuery.SetData(rowIndex, columnIndex, value);
                        return ExcelCellValueState.ProperValue;
                    }
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
            // do not do show 'unused optional property names'; in general this object represents some kind of table and not a property name/value collection
        }
        #endregion

        #endregion
    }
}