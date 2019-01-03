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

namespace Dodoni.XLBasicComponents
{
    public static partial class ExcelDataQueryExtensions
    {
        #region public static methods

        /// <summary>Gets the value of a specific pool with respect to a specific [optional] property name.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="dataQuery">The data query, i.e. property name/value pairs.</param>
        /// <param name="propertyName">The name of the property to search.</param>
        /// <param name="tryGetPoolElement">A method to pick a specific object.</param>
        /// <param name="value">On input this is a standard value of the property; on exit this argument will be changed if and only if a property with 
        /// name <paramref name="propertyName"/> exists and contains valid data.</param>
        /// <param name="dataAdvice">Data advice, i.e. a list of possible outcome to improve the useability, perhaps <c>null</c>.</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        /// <exception cref="ArgumentException">Thrown, if the user input is invalid, i.e. the name of the pool item is not given in its <see cref="System.String"/> representation.</exception>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="dataQuery"/> is <c>null</c>.</exception>
        public static bool TryGetOptionalPropertyPoolValue<T>(this IExcelDataQuery dataQuery, string propertyName, ExcelDataQuery.tTryGetPoolElement<T> tryGetPoolElement, ref T value, IExcelDataAdvice dataAdvice = null)
        {
            if (dataQuery == null)
            {
                throw new ArgumentNullException("dataQuery");
            }
            string objectName;
            ExcelPropertyValueState state = dataQuery.TryGetPropertyValue<String>(propertyName, out objectName, dataAdvice);

            if (state == ExcelPropertyValueState.ProperProperty)
            {
                T tempValue;
                if (tryGetPoolElement(objectName, out tempValue) == true)
                {
                    value = tempValue;
                    return true;
                }
                throw new ArgumentException("No valid input for property '" + propertyName + "' found; '" + objectName + "' is invalid input" + GetFormatedDataQueryName(dataQuery) + ".");
            }
            else if ((state == ExcelPropertyValueState.NoPropertyFound) || (state == ExcelPropertyValueState.ValueIsEmptyExcelCell))
            {
                return false;
            }
            throw new ArgumentException("No valid input for property '" + propertyName + "' found" + GetFormatedDataQueryName(dataQuery) + ".");
        }

        /// <summary>Gets the value of a specific pool with respect to a specific property name.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="dataQuery">The data query, i.e. property name/value pairs.</param>
        /// <param name="propertyName">The name of the property to search.</param>
        /// <param name="tryGetPoolElement">A method to pick a specific object.</param>
        /// <param name="value">The value of the property (output).</param>
        /// <param name="dataAdvice">Data advice, i.e. a list of possible outcome to improve the useability, perhaps <c>null</c>.</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        /// <exception cref="ArgumentException">Thrown, if the user input is invalid, i.e. the name of the pool item is not given in its <see cref="System.String"/> representation.</exception>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="dataQuery"/> is <c>null</c>.</exception>
        public static bool TryGetPropertyPoolValue<T>(this IExcelDataQuery dataQuery, string propertyName, ExcelDataQuery.tTryGetPoolElement<T> tryGetPoolElement, out T value, IExcelDataAdvice dataAdvice = null)
        {
            if (dataQuery == null)
            {
                throw new ArgumentNullException("dataQuery");
            }
            string objectName;
            ExcelPropertyValueState state = dataQuery.TryGetPropertyValue<String>(propertyName, out objectName, dataAdvice);

            if (state == ExcelPropertyValueState.ProperProperty)
            {
                return tryGetPoolElement(objectName, out value);
            }
            else if ((state == ExcelPropertyValueState.NoPropertyFound) || (state == ExcelPropertyValueState.ValueIsEmptyExcelCell))
            {
                value = default(T);
                return false;
            }
            throw new ArgumentException("No valid input for property '" + propertyName + "' found" + GetFormatedDataQueryName(dataQuery) + ".");
        }

        /// <summary>Gets the value of a specific required property.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="dataQuery">The data query, i.e. property name/value pairs.</param>
        /// <param name="propertyName">The name of the property to search.</param>
        /// <param name="tryGetPoolElement">A method to pick a specific object.</param>    
        /// <param name="dataAdvice">Data advice, i.e. a list of possible outcome to improve the useability, perhaps <c>null</c>.</param>
        /// <returns>The value of the property.</returns>
        /// <exception cref="ArgumentException">Thrown, if no property or valid value is given by the user.</exception>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="dataQuery"/> is <c>null</c>.</exception>
        public static T GetRequiredPropertyPoolValue<T>(this IExcelDataQuery dataQuery, string propertyName, ExcelDataQuery.tTryGetPoolElement<T> tryGetPoolElement, IExcelDataAdvice dataAdvice = null)
        {
            if (dataQuery == null)
            {
                throw new ArgumentNullException("dataQuery");
            }

            string objectName;
            ExcelPropertyValueState state = dataQuery.TryGetPropertyValue<String>(propertyName, out objectName, dataAdvice);
            if (state == ExcelPropertyValueState.NoPropertyFound)
            {
                throw new ArgumentException("No property with name '" + propertyName + " ' found" + GetFormatedDataQueryName(dataQuery) + ".");
            }
            else if (state == ExcelPropertyValueState.NoValidValue)
            {
                throw new ArgumentException("No valid input for property '" + propertyName + "' found" + GetFormatedDataQueryName(dataQuery) + ".");
            }
            else if (state == ExcelPropertyValueState.ValueIsEmptyExcelCell)
            {
                throw new ArgumentException("Input required for property '" + propertyName + "'" + GetFormatedDataQueryName(dataQuery) + ".");
            }

            T value;
            if (tryGetPoolElement(objectName, out value) == true)
            {
                return value;
            }
            throw new ArgumentException("No valid input for property '" + propertyName + "' found" + GetFormatedDataQueryName(dataQuery) + ".");
        }

        /// <summary>Gets the [optional] value of a specific pool with respect to a specific position of a <see cref="IExcelDataQuery"/> object.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="dataQuery">The data query, i.e. property name/value pairs.</param>
        /// <param name="tryGetPoolElement">A method to pick a specific object.</param>
        /// <param name="value">On input this is a standard value; on exist this argument will be changed if and only if <paramref name="dataQuery"/> contains valid data at the desired position.</param>
        /// <param name="rowIndex">The null-based index of the row.</param>
        /// <param name="columnIndex">The null-based index of the column.</param>
        /// <param name="dataAdvice">Data advice, i.e. a list of possible outcome to improve the useability, perhaps <c>null</c>.</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data; or the Excel cell is empty.</returns>
        /// <exception cref="ArgumentException">Thrown, if the user input is invalid, i.e. the name of the pool item is not given its <see cref="System.String"/> representation.</exception>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="dataQuery"/> is <c>null</c>.</exception>
        public static bool TryGetOptionalPoolValue<T>(this IExcelDataQuery dataQuery, ExcelDataQuery.tTryGetPoolElement<T> tryGetPoolElement, ref T value, int rowIndex = 0, int columnIndex = 0, IExcelDataAdvice dataAdvice = null)
        {
            if (dataQuery == null)
            {
                throw new ArgumentNullException("dataQuery");
            }

            string objectName;
            ExcelCellValueState state = dataQuery.TryGetValue<String>(out objectName, rowIndex, columnIndex, dataAdvice);

            if (state == ExcelCellValueState.ProperValue)
            {
                T tempValue;
                if (tryGetPoolElement(objectName, out tempValue) == true)
                {
                    value = tempValue;
                    return true;
                }
            }
            else if (state == ExcelCellValueState.EmptyOrMissingExcelCell)
            {
                return false;
            }
            throw new ArgumentException(dataQuery.ToString(rowIndex, columnIndex) + " is no valid input" + GetFormatedDataQueryName(dataQuery) + ".");
        }

        /// <summary>Gets the value of a specific pool with respect to a specific position of a <see cref="IExcelDataQuery"/> object.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="dataQuery">The data query, i.e. property name/value pairs.</param>
        /// <param name="tryGetPoolElement">A method to pick a specific object.</param>
        /// <param name="value">The value (output).</param>
        /// <param name="rowIndex">The null-based index of the row.</param>
        /// <param name="columnIndex">The null-based index of the column.</param>
        /// <param name="dataAdvice">Data advice, i.e. a list of possible outcome to improve the useability, perhaps <c>null</c>.</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data; or the Excel cell is empty.</returns>
        /// <exception cref="ArgumentException">Thrown, if the user input is invalid, i.e. the name of the pool item is not given its <see cref="System.String"/> representation.</exception>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="dataQuery"/> is <c>null</c>.</exception>
        public static bool TryGetPoolValue<T>(this IExcelDataQuery dataQuery, ExcelDataQuery.tTryGetPoolElement<T> tryGetPoolElement, out T value, int rowIndex = 0, int columnIndex = 0, IExcelDataAdvice dataAdvice = null)
        {
            if (dataQuery == null)
            {
                throw new ArgumentNullException("dataQuery");
            }

            string objectName;
            ExcelCellValueState state = dataQuery.TryGetValue<String>(out objectName, rowIndex, columnIndex, dataAdvice);

            if (state == ExcelCellValueState.ProperValue)
            {
                return tryGetPoolElement(objectName, out value);
            }
            else if (state == ExcelCellValueState.EmptyOrMissingExcelCell)
            {
                value = default(T);
                return false;
            }
            throw new ArgumentException(dataQuery.ToString(rowIndex, columnIndex) + " is no valid input" + GetFormatedDataQueryName(dataQuery) + ".");
        }

        /// <summary>Gets the value of a specific pool with respect to a specific position of a <see cref="IExcelDataQuery"/> object.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="dataQuery">The data query, i.e. property name/value pairs.</param>
        /// <param name="tryGetPoolElement">A method to pick a specific object.</param>
        /// <param name="rowIndex">The null-based index of the row.</param>
        /// <param name="columnIndex">The null-based index of the column.</param>
        /// <param name="dataAdvice">Data advice, i.e. a list of possible outcome to improve the useability, perhaps <c>null</c>.</param>
        /// <returns>The value.</returns>
        /// <exception cref="ArgumentException">Thrown, if the user input is invalid.</exception>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="dataQuery"/> is <c>null</c>.</exception>
        public static T GetRequiredPoolValue<T>(this IExcelDataQuery dataQuery, ExcelDataQuery.tTryGetPoolElement<T> tryGetPoolElement, int rowIndex = 0, int columnIndex = 0, IExcelDataAdvice dataAdvice = null)
        {
            if (dataQuery == null)
            {
                throw new ArgumentNullException("dataQuery");
            }

            string objectName;
            ExcelCellValueState state = dataQuery.TryGetValue<String>(out objectName, rowIndex, columnIndex, dataAdvice);

            T value;
            if (state == ExcelCellValueState.ProperValue)
            {
                if (tryGetPoolElement(objectName, out value) == true)
                {
                    return value;
                }
                else
                {
                    throw new ArgumentException(dataQuery.ToString(rowIndex, columnIndex) + " is no valid name" + GetFormatedDataQueryName(dataQuery) + ".");
                }
            }
            else if (state == ExcelCellValueState.EmptyOrMissingExcelCell)
            {
                throw new ArgumentException("Valid pool element name required" + GetFormatedDataQueryName(dataQuery) + ".");
            }
            throw new ArgumentException(dataQuery.ToString(rowIndex, columnIndex) + " is no valid input" + GetFormatedDataQueryName(dataQuery) + ".");
        }
        #endregion
    }
}