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
using System.Text;
using System.Collections.Generic;

using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Utilities;
using Dodoni.BasicComponents.Containers;

namespace Dodoni.XLBasicComponents
{
    /// <summary>Extension methods for <see cref="IExcelDataQuery"/> objects.
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static partial class ExcelDataQueryExtensions
    {
        #region public (static) methods

        #region general

        /// <summary>Gets the name of a specific <see cref="IExcelDataQuery"/> object in a specific <see cref="System.String"/> representation.
        /// </summary>
        /// <param name="dataQuery">The <see cref="IExcelDataQuery"/> object.</param>
        /// <returns>The name of <paramref name="dataQuery"/> in a specific <see cref="System.String"/> representation, i.e. add '[' and ']'; <see cref="System.String.Empty"/> if no name available.</returns>
        public static string GetFormatedDataQueryName(this IExcelDataQuery dataQuery)
        {
            if ((dataQuery == null) | (dataQuery.Name == null) || (dataQuery.Name.String.Length == 0))
            {
                return String.Empty;
            }
            return " [" + dataQuery.Name.String + "]";
        }

        /// <summary>Gets the value of a specific required property.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="dataQuery">The <see cref="IExcelDataQuery"/> object.</param>
        /// <param name="propertyName">The name of the property to search.</param>
        /// <param name="dataAdvice">A data advice for a the property value, i.e. possible outcome to improve the useability.</param>
        /// <param name="propertyValueColumnIndex">The null-based index of the column which contains the value.</param>        
        /// <returns>The value of the property.</returns>
        /// <exception cref="ArgumentException">Thrown, if no property or valid value is given by the user or if
        /// <typeparamref name="T"/> represents the type of a specific enumeration.</exception>
        public static T GetRequiredPropertyValue<T>(this IExcelDataQuery dataQuery, string propertyName, IExcelDataAdvice dataAdvice = null, int propertyValueColumnIndex = 1)
        {
            if (dataQuery == null)
            {
                throw new ArgumentNullException("dataQuery");
            }
            T value;
            int rowIndex;
            if (dataQuery.TryGetRowIndexOfPropertyName(propertyName, out rowIndex) == false)
            {
                throw new ArgumentException("No property with name '" + propertyName + "' found" + GetFormatedDataQueryName(dataQuery) + ".");
            }
            if (dataQuery.TryGetValue<T>(out value, rowIndex, propertyValueColumnIndex, dataAdvice) == ExcelCellValueState.ProperValue)
            {
                return value;
            }
            throw new ArgumentException("No valid input for property '" + propertyName + "' found" + GetFormatedDataQueryName(dataQuery) + ".");
        }

        /// <summary>Gets the value of a specific property.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="dataQuery">The <see cref="IExcelDataQuery"/> object.</param>
        /// <param name="propertyName">The name of the property to search.</param>
        /// <param name="value">The value of the property (output).</param>
        /// <param name="dataAdvice">A data advice for a the property value, i.e. possible outcome to improve the useability.</param>
        /// <param name="propertyValueColumnIndex">The null-based index of the column which contains the value.</param>        
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        /// <exception cref="ArgumentException">Thrown, if <typeparamref name="T"/> represents the type of a specific enumeration.</exception>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="dataQuery"/> is <c>null</c>.</exception>
        public static ExcelPropertyValueState TryGetPropertyValue<T>(this IExcelDataQuery dataQuery, string propertyName, out T value, IExcelDataAdvice dataAdvice = null, int propertyValueColumnIndex = 1)
        {
            if (dataQuery == null)
            {
                throw new ArgumentNullException("dataQuery");
            }
            int rowIndex;
            if (dataQuery.TryGetRowIndexOfPropertyName(propertyName, out rowIndex) == false)
            {
                value = default(T);
                return ExcelPropertyValueState.NoPropertyFound;
            }
            ExcelCellValueState state = dataQuery.TryGetValue<T>(out value, rowIndex, propertyValueColumnIndex, dataAdvice);
            if (state == ExcelCellValueState.ProperValue)
            {
                return ExcelPropertyValueState.ProperProperty;
            }
            else if (state == ExcelCellValueState.EmptyOrMissingExcelCell)
            {
                return ExcelPropertyValueState.ValueIsEmptyExcelCell;
            }
            else
            {
                return ExcelPropertyValueState.NoValidValue;
            }
        }

        /// <summary>Gets the value of a specific [optional] property.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="dataQuery">The <see cref="IExcelDataQuery"/> object.</param>
        /// <param name="propertyName">The name of the property to search.</param>
        /// <param name="value">On input this is a standard value of the property; on exit this argument will be changed if and only if a property with 
        /// name <paramref name="propertyName"/> exists and contains valid data.</param>
        /// <param name="dataAdvice">A data advice for a the property value, i.e. possible outcome to improve the useability.</param>
        /// <param name="propertyValueColumnIndex">The null-based index of the column which contains the value.</param>        
        /// <returns>A value indicating whether <paramref name="value"/> has been changed with user input.</returns>
        /// <exception cref="ArgumentException">Thrown, if <typeparamref name="T"/> represents the type of a specific enumeration or no <b>valid</b> user input is given, i.e.
        /// a property of name <paramref name="propertyName"/> exists but the value is non-empty and can not convert to <typeparamref name="T"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="dataQuery"/> is <c>null</c>.</exception>
        /// <remarks>Use this method if some standard values are available and the user has the option to change the standard setting.</remarks>
        public static bool TryGetOptionalPropertyValue<T>(this IExcelDataQuery dataQuery, string propertyName, ref T value, IExcelDataAdvice dataAdvice = null, int propertyValueColumnIndex = 1)
        {
            T tempValue;
            ExcelPropertyValueState state = TryGetPropertyValue<T>(dataQuery, propertyName, out tempValue, dataAdvice, propertyValueColumnIndex);
            if (state == ExcelPropertyValueState.NoValidValue)
            {
                throw new ArgumentException("No valid input for property '" + propertyName + "' found" + GetFormatedDataQueryName(dataQuery) + ".");
            }
            else if (state == ExcelPropertyValueState.ProperProperty)
            {
                value = tempValue;
                return true;
            }
            return false;
        }

        /// <summary>Gets the [optional] value of a specific Excel cell.
        /// </summary>
        /// <typeparam name="T">The type of the output.</typeparam>
        /// <param name="dataQuery">The <see cref="IExcelDataQuery"/> object.</param>
        /// <param name="value">On input this is a standard value; on exist this argument will be changed if and only if <paramref name="dataQuery"/> contains valid data at the desired position.</param>
        /// <param name="rowIndex">The null-based index of the row.</param>
        /// <param name="columnIndex">The null-based index of the column.</param>
        /// <param name="dataAdvice">Data advice, i.e. a list of possible outcome to improve the useability, perhaps <c>null</c>.</param>
        /// <returns>A value indicating whether <paramref name="value"/> has been changed and set to some user input.</returns>
        /// <exception cref="ArgumentException">Thrown, if <typeparamref name="T"/> represents an enumeration or if the user input at the desired position can not converted to <typeparamref name="T"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="dataQuery"/> is <c>null</c>.</exception>
        public static bool TryGetOptionalValue<T>(this IExcelDataQuery dataQuery, ref T value, int rowIndex = 0, int columnIndex = 0, IExcelDataAdvice dataAdvice = null)
        {
            if (dataQuery == null)
            {
                throw new ArgumentNullException("dataQuery");
            }

            T tempValue;
            ExcelCellValueState state = dataQuery.TryGetValue<T>(out tempValue, rowIndex, columnIndex, dataAdvice);
            if (state == ExcelCellValueState.NoValidValue)
            {
                throw new ArgumentException("No valid input '" + dataQuery.ToString(rowIndex, columnIndex) + "'" + GetFormatedDataQueryName(dataQuery) + ".");
            }
            else if (state == ExcelCellValueState.ProperValue)
            {
                value = tempValue;
                return true;
            }
            return false;
        }

        /// <summary>Sets an optional property value, i.e. if a specific property value is available the value will be used
        /// to change the state of a specific object, otherwise the standard value will be use.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="dataQuery">The property stream.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="trySetPropertyValue">A delegate which is used to set the value of the property for a specific object, if the property value is available.</param>
        /// <param name="dataAdvice">A data advice for a the property value, i.e. possible outcome to improve the useability.</param>
        /// <param name="propertyValueColumnIndex">The null-based index of the column which contains the value.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="dataQuery"/> is <c>null</c>.</exception>
        /// <remarks>Use this method for properties which have some standard value if the user does not
        /// enter a specific value, for example the standard tolerance used for a optimizer algorithm is 1e-7 if the
        /// user does not enter a different value.</remarks>
        public static void SetOptionalPropertyValue<T>(this IExcelDataQuery dataQuery, string propertyName, Func<T, bool> trySetPropertyValue, IExcelDataAdvice dataAdvice = null, int propertyValueColumnIndex = 1)
        {
            if (dataQuery == null)
            {
                throw new ArgumentNullException("dataQuery");
            }
            T propertyValue;
            ExcelPropertyValueState state = dataQuery.TryGetPropertyValue<T>(propertyName, out propertyValue, dataAdvice, propertyValueColumnIndex);

            if (state == ExcelPropertyValueState.ProperProperty)
            {
                if (trySetPropertyValue(propertyValue) == false)
                {
                    throw new ArgumentException("Invalid data for property '" + propertyName + "'" + GetFormatedDataQueryName(dataQuery) + ".");
                }
            }
            else if (state == ExcelPropertyValueState.NoValidValue)
            {
                throw new ArgumentException("Invalid data for property '" + propertyName + "'" + GetFormatedDataQueryName(dataQuery) + ".");
            }
        }

        /// <summary>Gets the value of a specific required property.
        /// </summary>
        /// <typeparam name="TEnum">The type of the value which is assumed to be a enumeration.</typeparam>
        /// <param name="dataQuery">The <see cref="IExcelDataQuery"/> object.</param>
        /// <param name="propertyName">The name of the property to search.</param>
        /// <param name="enumStringRepresentationUsage">The method how to compute the <see cref="System.String"/> representation of the enumeration <typeparamref name="TEnum"/>.</param>
        /// <param name="propertyValueColumnIndex">The null-based index of the column which contains the value, the second column is standard.</param>
        /// <returns>The value of the property.</returns>
        /// <exception cref="ArgumentException">Thrown, if no property or valid value is given by the user or if
        /// <typeparamref name="TEnum"/> does not represents a specific enumeration.</exception>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="dataQuery"/> is <c>null</c>.</exception>
        public static TEnum GetRequiredPropertyValue<TEnum>(this IExcelDataQuery dataQuery, string propertyName, EnumStringRepresentationUsage enumStringRepresentationUsage, int propertyValueColumnIndex = 1)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            if (dataQuery == null)
            {
                throw new ArgumentNullException("dataQuery");
            }
            TEnum value;
            int rowIndex;
            if (dataQuery.TryGetRowIndexOfPropertyName(propertyName, out rowIndex) == false)
            {
                throw new ArgumentException("No property with name '" + propertyName + "' found" + GetFormatedDataQueryName(dataQuery) + ".");
            }
            if (dataQuery.TryGetValue<TEnum>(enumStringRepresentationUsage, out value, rowIndex, propertyValueColumnIndex) == ExcelCellValueState.ProperValue)
            {
                return value;
            }
            throw new ArgumentException("No valid input for property '" + propertyName + "' found" + GetFormatedDataQueryName(dataQuery) + ".");
        }

        /// <summary>Gets the value of a specific property.
        /// </summary>
        /// <typeparam name="TEnum">The type of the value which is assumed to be a enumeration.</typeparam>
        /// <param name="dataQuery">The <see cref="IExcelDataQuery"/> object.</param>
        /// <param name="propertyName">The name of the property to search.</param>
        /// <param name="value">The value of the property (output).</param>
        /// <param name="enumStringRepresentationUsage">The method how to compute the <see cref="System.String"/> representation of the enumeration <typeparamref name="TEnum"/>.</param>
        /// <param name="propertyValueColumnIndex">The null-based index of the column which contains the value, the second column is standard.</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        /// <exception cref="ArgumentException">Thrown, if <typeparamref name="TEnum"/> does not represents a specific enumeration.</exception>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="dataQuery"/> is <c>null</c>.</exception>
        public static ExcelPropertyValueState TryGetPropertyValue<TEnum>(this IExcelDataQuery dataQuery, string propertyName, out TEnum value, EnumStringRepresentationUsage enumStringRepresentationUsage, int propertyValueColumnIndex = 1)
          where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            if (dataQuery == null)
            {
                throw new ArgumentNullException("dataQuery");
            }
            int rowIndex;
            if (dataQuery.TryGetRowIndexOfPropertyName(propertyName, out rowIndex) == false)
            {
                value = default(TEnum);
                return ExcelPropertyValueState.NoPropertyFound;
            }
            ExcelCellValueState state = dataQuery.TryGetValue<TEnum>(enumStringRepresentationUsage, out value, rowIndex, propertyValueColumnIndex);
            if (state == ExcelCellValueState.ProperValue)
            {
                return ExcelPropertyValueState.ProperProperty;
            }
            else if (state == ExcelCellValueState.EmptyOrMissingExcelCell)
            {
                return ExcelPropertyValueState.ValueIsEmptyExcelCell;
            }
            else
            {
                return ExcelPropertyValueState.NoValidValue;
            }
        }

        /// <summary>Gets the value of a specific [optional] property.
        /// </summary>
        /// <typeparam name="TEnum">The type of the value which is assumed to be a enumeration.</typeparam>
        /// <param name="dataQuery">The <see cref="IExcelDataQuery"/> object.</param>
        /// <param name="propertyName">The name of the property to search.</param>
        /// <param name="value">On input this is a standard value of the property; on exit this argument will be changed if and only if a property with 
        /// name <paramref name="propertyName"/> exists and contains valid data.</param>
        /// <param name="enumStringRepresentationUsage">The method how to compute the <see cref="System.String"/> representation of the enumeration <typeparamref name="TEnum"/>.</param>
        /// <param name="propertyValueColumnIndex">The null-based index of the column which contains the value, the second column is standard.</param>
        /// <returns>A value indicating whether <paramref name="value"/> has been changed with user input.</returns>
        /// <exception cref="ArgumentException">Thrown, if <typeparamref name="TEnum"/> does not represents a specific enumeration or no <b>valid</b> user input is given, i.e.
        /// a property of name <paramref name="propertyName"/> exists but the value is non-empty and can not convert to <typeparamref name="TEnum"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="dataQuery"/> is <c>null</c>.</exception>
        /// <remarks>Use this method if some standard values are available and the user has the option to change the standard setting.</remarks>
        public static bool TryGetOptionalPropertyValue<TEnum>(this IExcelDataQuery dataQuery, string propertyName, ref TEnum value, EnumStringRepresentationUsage enumStringRepresentationUsage, int propertyValueColumnIndex = 1)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            TEnum tempValue;
            ExcelPropertyValueState state = TryGetPropertyValue<TEnum>(dataQuery, propertyName, out tempValue, enumStringRepresentationUsage, propertyValueColumnIndex);
            if (state == ExcelPropertyValueState.NoValidValue)
            {
                throw new ArgumentException("No valid input for property '" + propertyName + "' found" + GetFormatedDataQueryName(dataQuery) + ".");
            }
            else if (state == ExcelPropertyValueState.ProperProperty)
            {
                value = tempValue;
                return true;
            }
            return false;
        }

        /// <summary>Gets the value of a specific Excel cell.
        /// </summary>
        /// <typeparam name="TEnum">The type of the output which is assumed to be a enumeration.</typeparam>
        /// <param name="dataQuery">The <see cref="IExcelDataQuery"/> object.</param>
        /// <param name="value">The value (output).</param>
        /// <param name="enumStringRepresentationUsage">The method how to compute the <see cref="System.String"/> representation.</param>
        /// <param name="rowIndex">The null-based index of the row.</param>
        /// <param name="columnIndex">The null-based index of the column.</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        /// <exception cref="ArgumentException">Thrown, if <typeparamref name="TEnum"/> does not represents a enumeration.</exception>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="dataQuery"/> is <c>null</c>.</exception>
        public static ExcelCellValueState TryGetValue<TEnum>(this IExcelDataQuery dataQuery, out TEnum value, EnumStringRepresentationUsage enumStringRepresentationUsage, int rowIndex = 0, int columnIndex = 0)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            if (dataQuery == null)
            {
                throw new ArgumentNullException("dataQuery");
            }
            return dataQuery.TryGetValue<TEnum>(enumStringRepresentationUsage, out value, rowIndex, columnIndex);
        }

        /// <summary>Gets the [optional] value of a specific Excel cell.
        /// </summary>
        /// <typeparam name="TEnum">The type of the output which is assumed to be a enumeration.</typeparam>
        /// <param name="dataQuery">The <see cref="IExcelDataQuery"/> object.</param>
        /// <param name="value">On input this is a standard value; on exist this argument will be changed if and only if <paramref name="dataQuery"/> contains valid data at the desired position.</param>
        /// <param name="enumStringRepresentationUsage">The method how to compute the <see cref="System.String"/> representation.</param>
        /// <param name="rowIndex">The null-based index of the row.</param>
        /// <param name="columnIndex">The null-based index of the column.</param>
        /// <returns>A value indicating whether <paramref name="value"/> has been changed and set to some user input.</returns>
        /// <exception cref="ArgumentException">Thrown, if <typeparamref name="TEnum"/> does not represents an enumeration or if the user input at the desired position can not converted to <typeparamref name="TEnum"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="dataQuery"/> is <c>null</c>.</exception>
        public static bool TryGetOptionalValue<TEnum>(this IExcelDataQuery dataQuery, ref TEnum value, EnumStringRepresentationUsage enumStringRepresentationUsage, int rowIndex = 0, int columnIndex = 0)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            if (dataQuery == null)
            {
                throw new ArgumentNullException("dataQuery");
            }

            TEnum tempValue;
            ExcelCellValueState state = dataQuery.TryGetValue<TEnum>(enumStringRepresentationUsage, out tempValue, rowIndex, columnIndex);
            if (state == ExcelCellValueState.NoValidValue)
            {
                throw new ArgumentException("No valid input '" + dataQuery.ToString(rowIndex, columnIndex) + "'" + GetFormatedDataQueryName(dataQuery) + ".");
            }
            else if (state == ExcelCellValueState.ProperValue)
            {
                value = tempValue;
                return true;
            }
            return false;
        }

        /// <summary>Sets an optional property value, i.e. if a specific property value is available the value will be used
        /// to change the state of a specific object, otherwise the standard value will be use.
        /// </summary>
        /// <typeparam name="TEnum">The type of the value which is assumed to be a enumeration.</typeparam>
        /// <param name="dataQuery">The <see cref="IExcelDataQuery"/> object.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="trySetPropertyValue">A delegate which is used to set the value of the property with respect to a specific object, if the property value is available.</param>
        /// <param name="enumStringRepresentationUsage">The method how to compute the <see cref="System.String"/> representation of the enumeration <typeparamref name="TEnum"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="dataQuery"/> is <c>null</c>.</exception>
        /// <remarks>Use this method for properties which have some standard value if the user does not
        /// enter a specific value, for example the standard exit condition of an optimizer algorithm
        /// is the maximal number of iterations.</remarks>
        public static void SetOptionalPropertyValue<TEnum>(this IExcelDataQuery dataQuery, string propertyName, Func<TEnum, bool> trySetPropertyValue, EnumStringRepresentationUsage enumStringRepresentationUsage)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            if (dataQuery == null)
            {
                throw new ArgumentNullException("dataQuery");
            }
            TEnum propertyValue;
            ExcelPropertyValueState state = dataQuery.TryGetPropertyValue<TEnum>(propertyName, out propertyValue, enumStringRepresentationUsage);

            if (state == ExcelPropertyValueState.ProperProperty)
            {
                if (trySetPropertyValue(propertyValue) == false)
                {
                    throw new ArgumentException("Invalid data for property '" + propertyName + "'" + GetFormatedDataQueryName(dataQuery) + ".");
                }
            }
            else if (state == ExcelPropertyValueState.NoValidValue)
            {
                throw new ArgumentException("Invalid data for property '" + propertyName + "'" + GetFormatedDataQueryName(dataQuery) + ".");
            }
        }

        /// <summary>Gets the value of a specific cell.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="dataQuery">The <see cref="IExcelDataQuery"/> object.</param>
        /// <param name="rowIndex">The null-based index of the row.</param>
        /// <param name="columnIndex">The null-based index of the column.</param>
        /// <param name="dataAdvice">A data advice for a the property value, i.e. possible outcome to improve the useability.</param>
        /// <returns>The value of the cell at the desired position.</returns>
        /// <exception cref="ArgumentException">Thrown, if <typeparamref name="T"/> represents a enumeration or no valid input is given by the user.</exception>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="dataQuery"/> is <c>null</c>.</exception>
        public static T GetValue<T>(this IExcelDataQuery dataQuery, int rowIndex = 0, int columnIndex = 0, IExcelDataAdvice dataAdvice = null)
        {
            if (dataQuery == null)
            {
                throw new ArgumentNullException("dataQuery");
            }
            T value;
            if (dataQuery.TryGetValue<T>(out value, rowIndex, columnIndex, dataAdvice) == ExcelCellValueState.ProperValue)
            {
                return value;
            }
            throw new ArgumentException("No valid input '" + dataQuery.ToString(rowIndex, columnIndex) + "' at row: " + rowIndex + ", column: " + columnIndex + " found" + GetFormatedDataQueryName(dataQuery) + ".");
        }

        /// <summary>Gets the value of a specific cell.
        /// </summary>
        /// <typeparam name="TEnum">The type of the output which is assumed to be a enumeration.</typeparam>
        /// <param name="dataQuery">The <see cref="IExcelDataQuery"/> object.</param>
        /// <param name="enumStringRepresentationUsage">The method how to compute the <see cref="System.String"/> representation.</param>
        /// <param name="rowIndex">The null-based index of the row.</param>
        /// <param name="columnIndex">The null-based index of the column.</param>
        /// <exception cref="ArgumentException">Thrown, if <typeparamref name="TEnum"/> does not represents a enumeration or no valid input is given by the user.</exception>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="dataQuery"/> is <c>null</c>.</exception>
        public static TEnum GetValue<TEnum>(this IExcelDataQuery dataQuery, EnumStringRepresentationUsage enumStringRepresentationUsage, int rowIndex = 0, int columnIndex = 0)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            if (dataQuery == null)
            {
                throw new ArgumentNullException("dataQuery");
            }
            TEnum value;
            if (dataQuery.TryGetValue<TEnum>(enumStringRepresentationUsage, out value, rowIndex, columnIndex) == ExcelCellValueState.ProperValue)
            {
                return value;
            }
            throw new ArgumentException("No valid input '" + dataQuery.ToString(rowIndex, columnIndex) + "' at row: " + rowIndex + ", column: " + columnIndex + " found" + GetFormatedDataQueryName(dataQuery) + ".");
        }

        /// <summary>Gets values of a specific column with respect to a specific type, empty elements will be ignored.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="dataQuery">The <see cref="IExcelDataQuery"/> object.</param>
        /// <param name="columnIndex">The null-based index of the column.</param>
        /// <param name="startRowIndex">The first row to take into account in its null-based index representation.</param>
        /// <returns>The values of the <paramref name="dataQuery"/> in the specific column of the desired type, empty cells will be ignored.</returns>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="dataQuery"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="columnIndex"/> is invalid or the column contains
        /// a value which is non-empty but can not converted into <typeparamref name="T"/>.</exception>
        public static T[] GetColumnVector<T>(this IExcelDataQuery dataQuery, int columnIndex = 0, int startRowIndex = 0)
        {
            if (dataQuery == null)
            {
                throw new ArgumentNullException("dataQuery");
            }
            List<T> vector = new List<T>();
            for (int j = startRowIndex; j < dataQuery.RowCount; j++)
            {
                T value;
                ExcelCellValueState state = dataQuery.TryGetValue<T>(out value, j, columnIndex);
                if (state == ExcelCellValueState.ProperValue)
                {
                    vector.Add(value);
                }
                else if (state == ExcelCellValueState.NoValidValue)
                {
                    throw new ArgumentException("No valid input '" + dataQuery.ToString(j, columnIndex) + "' at row: " + j + ", column: " + columnIndex + " found" + GetFormatedDataQueryName(dataQuery) + ".");
                }
            }
            return vector.ToArray();
        }

        /// <summary>Gets values of a specific row with respect to a specific type, empty elements will be ignored.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="dataQuery">The <see cref="IExcelDataQuery"/> object.</param>
        /// <param name="rowIndex">The null-based index of the row.</param>
        /// <param name="startColumnIndex">The first column to take into account in its null-based index representation.</param>
        /// <returns>The values of the <paramref name="dataQuery"/> in the specific row of the desired type, empty cells will be ignored.</returns>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="dataQuery"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="rowIndex"/> is invalid or the column contains
        /// a value which is non-empty but can not converted into <typeparamref name="T"/>.</exception>
        public static T[] GetRowVector<T>(this IExcelDataQuery dataQuery, int rowIndex = 0, int startColumnIndex = 0)
        {
            if (dataQuery == null)
            {
                throw new ArgumentNullException("dataQuery");
            }
            List<T> vector = new List<T>();
            for (int j = startColumnIndex; j < dataQuery.ColumnCount; j++)
            {
                T value;
                ExcelCellValueState state = dataQuery.TryGetValue<T>(out value, rowIndex, j);
                if (state == ExcelCellValueState.ProperValue)
                {
                    vector.Add(value);
                }
                else if (state == ExcelCellValueState.NoValidValue)
                {
                    throw new ArgumentException("No valid input '" + dataQuery.ToString(rowIndex, j) + "' at row: " + rowIndex + ", column: " + j + " found" + GetFormatedDataQueryName(dataQuery) + ".");
                }
            }
            return vector.ToArray();
        }

        /// <summary>Gets the null-based column index of a specific header name.
        /// </summary>
        /// <param name="dataQuery">The <see cref="IExcelDataQuery"/> object.</param>
        /// <param name="headerName">The name of the header to search.</param>
        /// <param name="columnIndex">The null-based index of the column which contains in the first row <paramref name="headerName"/> (output).</param>
        /// <returns>A value indicating whether <paramref name="columnIndex"/> contains valid data.</returns>
        public static bool TryGetHeaderNameColumnIndex(this IExcelDataQuery dataQuery, string headerName, out int columnIndex)
        {
            if (dataQuery == null)
            {
                throw new ArgumentNullException("dataQuery");
            }

            if (headerName == null)
            {
                throw new ArgumentNullException("headerName");
            }
            if (headerName.Length == 0)
            {
                throw new ArgumentException(String.Format(ExceptionMessages.ArgumentIsInvalid, "<empty>"), "headerName");
            }

            string idHeaderName = headerName.ToIDString();
            for (int j = 0; j < dataQuery.ColumnCount; j++)
            {
                string value;
                switch (dataQuery.TryGetValue<string>(out value, rowIndex: 0, columnIndex: j))
                {
                    case ExcelCellValueState.EmptyOrMissingExcelCell:
                        break;

                    case ExcelCellValueState.NoValidValue:
                        throw new ArgumentException("Invalid header, ' " + dataQuery.ToString(0, j) + "' does not represent a string" + GetFormatedDataQueryName(dataQuery) + ".");

                    case ExcelCellValueState.ProperValue:
                        if (value != null)
                        {
                            if (idHeaderName == value.ToIDString())
                            {
                                columnIndex = j;
                                return true;
                            }
                        }
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
            columnIndex = -1;
            return false;
        }

        /// <summary>Gets the null-based column index of a specific header name.
        /// </summary>
        /// <param name="dataQuery">The <see cref="IExcelDataQuery"/> object.</param>
        /// <param name="headerName">The name of the header to search.</param>
        /// <returns>The null-based index of the column which contains in the first row <paramref name="headerName"/>.</returns>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="dataQuery"/> does not contain a header with name <paramref name="headerName"/>.</exception>
        public static int GetHeaderNameColumnIndex(this IExcelDataQuery dataQuery, string headerName)
        {
            int columnNameHeaderIndex;
            if (TryGetHeaderNameColumnIndex(dataQuery, headerName, out columnNameHeaderIndex) == true)
            {
                return columnNameHeaderIndex;
            }
            throw new ArgumentException("No column header found with name '" + headerName + "'" + GetFormatedDataQueryName(dataQuery) + ".");
        }

        /// <summary>Gets the null-based column index of a specific header name.
        /// </summary>
        /// <param name="dataQuery">The <see cref="IExcelDataQuery"/> object.</param>
        /// <param name="isHeaderName">A delegate that determines the name of the header to search; the argument is in the original representation, i.e. including whitespaces etc.</param>
        /// <param name="columnIndex">The null-based index of the column which contains in the first row the desired header name (output).</param>
        /// <returns>A value indicating whether <paramref name="columnIndex"/> contains valid data.</returns>
        public static bool TryGetHeaderNameColumnIndex(this IExcelDataQuery dataQuery, Func<string, bool> isHeaderName, out int columnIndex)
        {
            if (dataQuery == null)
            {
                throw new ArgumentNullException("dataQuery");
            }
            if (isHeaderName == null)
            {
                throw new ArgumentNullException("isHeaderName");
            }

            for (int j = 0; j < dataQuery.ColumnCount; j++)
            {
                string value;
                switch (dataQuery.TryGetValue<string>(out value, rowIndex: 0, columnIndex: j))
                {
                    case ExcelCellValueState.EmptyOrMissingExcelCell:
                        break;

                    case ExcelCellValueState.NoValidValue:
                        throw new ArgumentException("Invalid header, ' " + dataQuery.ToString(0, j) + "' does not represent a string" + GetFormatedDataQueryName(dataQuery) + ".");

                    case ExcelCellValueState.ProperValue:
                        if (value != null)
                        {
                            if (isHeaderName(value) == true)
                            {
                                columnIndex = j;
                                return true;
                            }
                        }
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
            columnIndex = -1;
            return false;
        }

        /// <summary>Gets the null-based column index of a specific header name.
        /// </summary>
        /// <param name="dataQuery">The <see cref="IExcelDataQuery"/> object.</param>
        /// <param name="isHeaderName">A delegate that determines the name of the header to search; the argument is in the original representation, i.e. including whitespaces etc.</param>
        /// <param name="rawHeaderName">A 'raw header name' which is used in the <see cref="System.String"/> representation of the <see cref="ArgumentException"/> thrown in the case that no specified header name is found.</param>
        /// <returns>The null-based index of the column which contains in the first row the desired header name.</returns>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="dataQuery"/> does not contain a header with respect to <paramref name="isHeaderName"/>.</exception>
        public static int GetHeaderNameColumnIndex(this IExcelDataQuery dataQuery, Func<string, bool> isHeaderName, string rawHeaderName = "")
        {
            int columnNameHeaderIndex;
            if (TryGetHeaderNameColumnIndex(dataQuery, isHeaderName, out columnNameHeaderIndex) == true)
            {
                return columnNameHeaderIndex;
            }
            throw new ArgumentException(String.Format("No column header found with name '<{0}>' {1}.", rawHeaderName, GetFormatedDataQueryName(dataQuery)));
        }
        #endregion

        #region methods for specific data queries

        /// <summary>Gets the <see cref="IExcelDataQuery"/> object which contains general properties, i.e. the table with name 'General Properties'.
        /// </summary>
        /// <param name="dataQueries">The data queries.</param>
        /// <returns>The <see cref="IExcelDataQuery"/> object which contains general properties.</returns>
        public static IExcelDataQuery GetGeneralProperties(this IIdentifierStringDictionary<IExcelDataQuery> dataQueries)
        {
            IExcelDataQuery value;
            if (dataQueries.TryGetValue(ExcelDataQuery.GeneralPropertyDataQueryName, out value) == false)
            {
                throw new ArgumentException("No data table found with name '" + ExcelDataQuery.GeneralPropertyDataQueryName + "'.");
            }
            return value;
        }

        /// <summary>Gets the level of details, i.e. the value of the optional property 'Level of Details'.
        /// </summary>
        /// <param name="generalPropertyExcelDataQuery">A <see cref="IExcelDataQuery"/> object that contains general properties.</param>
        /// <param name="propertyValueColumnIndex">The null-based index of the column which contains the value, the second column is standard.</param>
        /// <returns>A value indicating the level of detail for the output, i.e. for the Pool inspector etc.</returns>
        public static InfoOutputDetailLevel GetLevelOfDetails(this IExcelDataQuery generalPropertyExcelDataQuery, int propertyValueColumnIndex = 1)
        {
            if (generalPropertyExcelDataQuery == null)
            {
                throw new ArgumentNullException("generalPropertyExcelDataQuery");
            }
            InfoOutputDetailLevel levelOfDetails = InfoOutputDetailLevel.Full;
            generalPropertyExcelDataQuery.TryGetOptionalPropertyValue<InfoOutputDetailLevel>("Level of Details", ref levelOfDetails, EnumStringRepresentationUsage.StringAttribute, propertyValueColumnIndex);
            return levelOfDetails;
        }

        /// <summary>Gets the name of the object, i.e. the value of the property 'Object name'.
        /// </summary>
        /// <param name="dataQuery">The <see cref="IExcelDataQuery"/> object.</param>
        /// <returns>The name of the object.</returns>
        public static string GetObjectName(this IExcelDataQuery dataQuery)
        {
            return dataQuery.GetRequiredPropertyValue<string>(ExcelDataQuery.PropertyObjectName, propertyValueColumnIndex: 1);
        }
        #endregion

        #endregion
    }
}