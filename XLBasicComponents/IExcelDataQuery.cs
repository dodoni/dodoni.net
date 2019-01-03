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

using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Utilities;

namespace Dodoni.XLBasicComponents
{
    /// <summary>Serves as main Excel interface, i.e. represents a Excel Range or a list of property names/values etc.
    /// </summary>
    public interface IExcelDataQuery : IIdentifierNameable
    {
        #region properties

        /// <summary>Gets the number of rows.
        /// </summary>
        /// <value>The number of rows.</value>
        int RowCount { get; }

        /// <summary>Gets the number of columns.
        /// </summary>
        /// <value>The number columns.</value>
        int ColumnCount { get; }

        /// <summary>Gets a value indicating whether the <see cref="IExcelDataQuery"/> object is just a single Excel cell.
        /// </summary>
        /// <value><c>true</c> if this instance is single cell; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>If <see cref="IExcelDataQuery.IsEmpty"/> is <c>true</c>, the return value of this property is undefined.</remarks>
        bool IsSingleCell { get; }

        /// <summary>Gets a value indicating whether the current instance is empty, i.e. <see cref="IExcelDataQuery.RowCount"/> = <see cref="IExcelDataQuery.ColumnCount"/> = 0.
        /// </summary>
        /// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
        bool IsEmpty { get; }
        #endregion

        #region methods

        /// <summary>Determines whether a specific Excel cell is empty.
        /// </summary>
        /// <param name="rowIndex">The null-based index of the row.</param>
        /// <param name="columnIndex">The null-based index of the column.</param>
        /// <returns><c>true</c> if the Excel cell at the specific position is empty; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown, if the row/column position is not valid.</exception>
        bool IsEmptyExcelCell(int rowIndex, int columnIndex);

        /// <summary>Gets customize data, i.e. a representation of the current <see cref="IExcelDataQuery"/> 
        /// object as a specific <see cref="GuidedExcelDataQuery"/> instance.
        /// </summary>
        /// <returns>A representation of the data which contains the type of each property etc.</returns>
        /// <remarks>The return value will be used perhaps for file operations, i.e. for writing a representation of the 
        /// data into a specific file etc.</remarks>
        GuidedExcelDataQuery AsCustomizeData();

        /// <summary>Gets the null-based row index of a specific property.
        /// </summary>
        /// <param name="propertyName">The name of the property to search (in the first column).</param>
        /// <param name="rowIndex">The null-based index of the row which contains the property (output).</param>
        /// <param name="dataAdvice">Data advice, i.e. a list of possible outcome, perhaps <c>null</c>.</param>
        /// <returns>A value indicating whether <paramref name="rowIndex"/> contains valid data.</returns>
        bool TryGetRowIndexOfPropertyName(string propertyName, out int rowIndex, IExcelDataAdvice dataAdvice = null);

        /// <summary>Gets the value of a specific Excel cell.
        /// </summary>
        /// <typeparam name="T">The type of the output.</typeparam>
        /// <param name="value">The value (output).</param>
        /// <param name="rowIndex">The null-based index of the row.</param>
        /// <param name="columnIndex">The null-based index of the column.</param>
        /// <param name="dataAdvice">Data advice, i.e. a list of possible outcome, perhaps <c>null</c>.</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        /// <exception cref="ArgumentException">Thrown, if <typeparamref name="T"/> represents a enumeration.</exception>
        ExcelCellValueState TryGetValue<T>(out T value, int rowIndex = 0, int columnIndex = 0, IExcelDataAdvice dataAdvice = null);

        /// <summary>Gets the value of a specific Excel cell.
        /// </summary>
        /// <typeparam name="TEnum">The type of the output which is assumed to be a enumeration.</typeparam>
        /// <param name="enumStringRepresentationUsage">The method how to compute the <see cref="System.String"/> representation.</param>
        /// <param name="value">The value (output).</param>
        /// <param name="rowIndex">The null-based index of the row.</param>
        /// <param name="columnIndex">The null-based index of the column.</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        /// <exception cref="ArgumentException">Thrown, if <typeparamref name="TEnum"/> does not represents a enumeration.</exception>
        ExcelCellValueState TryGetValue<TEnum>(EnumStringRepresentationUsage enumStringRepresentationUsage, out TEnum value, int rowIndex = 0, int columnIndex = 0)
            where TEnum : struct, IComparable, IConvertible, IFormattable;

        /// <summary>Returns a <see cref="System.String"/> representation of a specific entry.
        /// </summary>
        /// <param name="rowIndex">The null-based index of the row.</param>
        /// <param name="columnIndex">The null-based index of the column.</param>
        /// <returns>A <see cref="System.String"/> that represents the entry with the desired position.
        /// </returns>
        /// <remarks>This method is used for error reports etc. only, thus do not use this method to
        /// get the value of a specific Excel cell.</remarks>
        string ToString(int rowIndex, int columnIndex);

        /// <summary>Finalize the current <see cref="IExcelDataQuery"/> instance.
        /// </summary>
        /// <param name="throwExceptionIfDataIsNotUsed">A value indicating whether an exception will be thrown, if some of the data are are not queried by the user.</param>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="throwExceptionIfDataIsNotUsed"/> is <c>true</c> and a property or table entry has been detected which
        /// is not queried, i.e. not used.</exception>
        /// <remarks>Call this method before calling <see cref="IExcelDataQuery.AsCustomizeData()"/> to check the user input; perhaps the user has enter properties or values
        /// which are not used by the program and an exception will be shown to indicate wrong user input.</remarks>
        void QueryCompleted(bool throwExceptionIfDataIsNotUsed = true);
        #endregion
    }
}