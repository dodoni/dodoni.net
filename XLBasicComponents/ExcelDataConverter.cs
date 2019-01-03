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
using System.Linq;
using System.Globalization;
using System.Collections.Generic;

using ExcelDna.Integration;

using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Utilities;

namespace Dodoni.XLBasicComponents
{
    /// <summary>Represents some special functions needed to convert some Excel input into its regular data representation.
    /// </summary>
    public static class ExcelDataConverter
    {
        #region nested declarations

        /// <summary>A function to convert an Excel cell into a specific type.
        /// </summary>
        /// <param name="excelCell">The excel cell value to convert.</param>
        /// <param name="value">The converted <paramref name="excelCell"/> in its <see cref="System.Object"/> representation (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public delegate bool TryGetExcelCellValue(object excelCell, out object value);

        /// <summary>Represents the type of the output Range.
        /// </summary>
        public enum RangeOutputType
        {
            /// <summary>If the output can not be represented by a single Excel cell, a matrix function is assumed. Therefore it will be checked whether the selected Range is suitable for the output.
            /// </summary>
            Standard,

            /// <summary>The output should be written below the cell where the specific UDF has been called ('similar to Bloomberg BDH function').
            /// </summary>
            BelowCallerCell
        }
        #endregion

        #region private static members

        /// <summary>For non-standard types a function used to convert an Excel cell into this type; the type is the key of the dictionary.
        /// </summary>
        private static Dictionary<Type, TryGetExcelCellValue> sm_ExcelCellConverter = new Dictionary<Type, TryGetExcelCellValue>();
        #endregion

        #region public static methods

        #region cell value converter methods

        /// <summary>Registers a function to convert an Excel cell value into a specific <see cref="System.Type"/>.
        /// </summary>
        /// <param name="type">The type of the Excel cell to convert to.</param>
        /// <param name="tryGetExcelCellValue">The function which converts an Excel cell value into its <paramref name="type"/> representation.</param>
        public static void RegisterTryGetExcelCellValueFunction(Type type, TryGetExcelCellValue tryGetExcelCellValue)
        {
            sm_ExcelCellConverter.Add(type, tryGetExcelCellValue);
        }

        /// <summary>Gets the value of a specific Excel cell.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enumeration.</typeparam>
        /// <param name="excelCell">The Excel cell.</param>
        /// <param name="enumStringRepresentationUsage">The method how to compute the <see cref="System.String"/> representation of the enumeration <typeparamref name="TEnum"/>.</param>
        /// <param name="value">The value of the <paramref name="excelCell"/> (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        /// <exception cref="ArgumentException">Thrown, if <typeparamref name="TEnum"/> does not represent a enumeration.</exception>
        public static bool TryGetCellValue<TEnum>(object excelCell, EnumStringRepresentationUsage enumStringRepresentationUsage, out TEnum value)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            if (typeof(TEnum).IsEnum)
            {
                string tableValue = ((string)excelCell);

                return EnumString<TEnum>.TryParse(tableValue, out value, enumStringRepresentationUsage);
            }
            throw new ArgumentException(String.Format(ExceptionMessages.ArgumentIsInvalid, "EnumType: " + typeof(TEnum).ToString()), "TEnum");
        }

        /// <summary>Gets the value of a specific Excel cell.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="excelCell">The Excel cell.</param>
        /// <returns>The value of the <paramref name="excelCell"/> in its <typeparamref name="T"/> representation.</returns>
        /// <exception cref="ArgumentException">Thrown, <paramref name="excelCell"/> can not converted to an object of type <typeparamref name="T"/>.</exception>
        public static T GetCellValue<T>(object excelCell)
        {
            T value;
            if (TryGetCellValue<T>(excelCell, out value) == true)
            {
                return value;
            }
            throw new ArgumentException("Can not cast " + GetExcelCellRepresentation(excelCell) + " to " + typeof(T).Name + ".");
        }

        /// <summary>Gets the value of a specific Excel cell.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="excelCell">The Excel cell.</param>
        /// <returns>The value of the <paramref name="excelCell"/> in its <typeparamref name="T"/> representation; <c>null</c> if <paramref name="excelCell"/> can not be cast to <typeparamref name="T"/>.</returns>
        public static T? GetCellValueAsNullable<T>(object excelCell)
            where T : struct
        {
            T value;
            if (TryGetCellValue<T>(excelCell, out value) == true)
            {
                return value;
            }
            return null;
        }

        /// <summary>Gets the value of a specific Excel cell.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="excelCell">The Excel cell.</param>
        /// <param name="value">The value of the <paramref name="excelCell"/> (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        /// <exception cref="ArgumentException">Thrown, if <typeparamref name="T"/> represents a enumeration or <paramref name="excelCell"/> can not converted to an object of type <typeparamref name="T"/>.</exception>
        public static bool TryGetCellValue<T>(object excelCell, out T value)
        {
            if (typeof(T).IsEnum) // which enumStringRepresentationUsage? --> call an other method
            {
                throw new ArgumentException("Do not use these method [TryGetCellValue] for enumerations.");
            }

            if ((excelCell == null) || (excelCell is ExcelEmpty) || (excelCell is ExcelMissing))
            {
                value = default(T);
                return false;
            }
            Type typeofT = typeof(T);

            if (typeofT == typeof(string))
            {
                if (excelCell is String)
                {
                    value = (T)(object)(string)excelCell;
                    return true;
                }
                else
                {
                    value = (T)(object)excelCell.ToString();
                    return true;
                }
            }
            else if (typeofT == typeof(Double))
            {
                double doubleValue;
                if (ExcelDataConverter.TryGetDouble(excelCell, out doubleValue) == true)
                {
                    value = (T)(object)doubleValue;
                    return true;
                }
            }
            else if (typeofT == typeof(int))
            {
                int intValue;
                if (TryGetInteger(excelCell, out intValue) == true)
                {
                    value = (T)(object)(intValue);
                    return true;
                }
            }
            else if (typeofT == typeof(DateTime))
            {
                DateTime dateTime;
                if (TryGetDateTime(excelCell, out dateTime) == true)
                {
                    value = (T)((object)dateTime);
                    return true;
                }
            }
            else if (typeofT == typeof(bool))
            {
                if (excelCell is bool)
                {
                    value = (T)(object)(bool)excelCell;
                    return true;
                }
                if (excelCell is string)  // a fallback solution
                {
                    IdentifierString stringRepresentation = new IdentifierString((string)excelCell);
                    if (IsTrueExcelCell(stringRepresentation))
                    {
                        value = (T)(object)(true);
                        return true;
                    }
                    else if (IsFalseExcelCell(stringRepresentation))
                    {
                        value = (T)(object)(false);
                        return true;
                    }
                }
                value = default(T);
                return false;
            }
            else if (typeofT == typeof(IdentifierString))
            {
                if (excelCell is String)
                {
                    value = (T)(object)new IdentifierString((string)excelCell);
                    return true;
                }
                else
                {
                    value = (T)(object)new IdentifierString(excelCell.ToString());
                    return true;
                }
            }
            else
            {
                TryGetExcelCellValue tryGetExcelCellValue;
                if (sm_ExcelCellConverter.TryGetValue(typeofT, out tryGetExcelCellValue) == true)
                {
                    object tempValue;
                    if (tryGetExcelCellValue(excelCell, out tempValue) == true)
                    {
                        value = (T)tempValue;
                        return true;
                    }
                }
            }
            if (excelCell is T) // a fallback solution
            {
                value = (T)excelCell;
                return true;
            }
            value = default(T);
            return false;
        }
        #endregion

        #region Excel range methods

        /// <summary>Gets the excel range output, i.e. converts the output such that no '#NV' values are
        /// shown and print an error message, if the given range is to small for the output.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="rangeOutputType">A value indicating how the values should be presented in Excel.</param>
        /// <returns>The range which contains <paramref name="values"/> or some error message.</returns>
        public static object GetExcelRangeOutput(object[,] values, RangeOutputType rangeOutputType = RangeOutputType.Standard)
        {
            return GetExcelRangeOutput<object>(values, rangeOutputType);
        }

        /// <summary>Gets the excel range output, i.e. converts the output such that no '#NV' values are
        /// shown and print an error message, if the given range is to small for the output.
        /// </summary>
        /// <typeparam name="T">The type of the values.</typeparam>
        /// <param name="values">The values.</param>
        /// <param name="rangeOutputType">A value indicating how the values should be presented in Excel.</param>
        /// <returns>The range which contains <paramref name="values"/> or some error message.</returns>
        public static object GetExcelRangeOutput<T>(T[,] values, RangeOutputType rangeOutputType = RangeOutputType.Standard)
        {
            int valueRowCount = values.GetLength(0);
            int valueColumnCount = values.GetLength(1);

            if (rangeOutputType == RangeOutputType.Standard)
            {
                int excelRangeRowCount, excelRangeColumnCount;
                ExcelLowLevel.OutputRangeOrientation rangeOrientation;
                ExcelLowLevel.GetRangeSize(out excelRangeRowCount, out excelRangeColumnCount, out rangeOrientation);

                if (rangeOrientation == ExcelLowLevel.OutputRangeOrientation.Transposed)  // swap the Excel Range size given by the user
                {
                    int temp = excelRangeRowCount;
                    excelRangeRowCount = excelRangeColumnCount;
                    excelRangeColumnCount = temp;
                }

                object[,] returnValue = new object[excelRangeRowCount, excelRangeColumnCount];
                for (int i = 0; i < excelRangeRowCount; i++)
                {
                    for (int j = 0; j < excelRangeColumnCount; j++)
                    {
                        returnValue[i, j] = String.Empty;
                    }
                }

                if ((valueRowCount > excelRangeRowCount) || (valueColumnCount > excelRangeColumnCount))
                {
                    if (rangeOrientation == ExcelLowLevel.OutputRangeOrientation.Regular)
                    {
                        returnValue[0, 0] = "ERROR: At least " + valueRowCount + " x " + valueColumnCount + " Range needed.";
                    }
                    else
                    {
                        returnValue[0, 0] = "ERROR: At least " + valueColumnCount + " x " + valueRowCount + " Range needed.";
                    }
                    return returnValue;
                }

                for (int i = 0; i < valueRowCount; i++)
                {
                    for (int j = 0; j < valueColumnCount; j++)
                    {
                        returnValue[i, j] = values[i, j];
                    }
                }
                return returnValue;
            }
            else  // write the result below the cell where the user has called the specified UDF
            {
                int firstRowIndex, firstColumnIndex;
                var returnValue = ExcelLowLevel.GetCurrentRangePosition(out firstRowIndex, out firstColumnIndex);

                ExcelAsyncUtil.QueueAsMacro(() =>
                {
                    for (int i = 0; i < valueRowCount; i++)
                    {
                        for (int j = 0; j < valueColumnCount; j++)
                        {
                            var cellReference = new ExcelReference(firstRowIndex + i + 1, firstColumnIndex + j);
                            cellReference.SetValue(values[i, j]);
                        }
                    }
                });
                return String.Format("<Below result of> {0}", returnValue).ToTimeStampString();
            }
        }

        /// <summary>Gets the excel range output, i.e. converts the output such that no '#NV' values are
        /// shown and print an error message, if the given range is to small for the output.
        /// </summary>
        /// <typeparam name="T">The type of the values.</typeparam>
        /// <param name="values">The values.</param>
        /// <param name="rangeOutputType">A value indicating how the values should be presented in Excel.</param>
        /// <returns>The range which contains <paramref name="values"/> or some error message.</returns>
        public static object GetExcelRangeOutput<T>(T[][] values, RangeOutputType rangeOutputType = RangeOutputType.Standard)
        {
            int valueRowCount = values.GetLength(0);
            int valueColumnCount = values[0].GetLength(0);

            if (rangeOutputType == RangeOutputType.Standard)
            {
                ExcelLowLevel.OutputRangeOrientation rangeOrientation;
                int excelRangeRowCount, excelRangeColumnCount;
                ExcelLowLevel.GetRangeSize(out excelRangeRowCount, out excelRangeColumnCount, out rangeOrientation);

                if (rangeOrientation == ExcelLowLevel.OutputRangeOrientation.Transposed)  // swap the Excel Range size given by the user
                {
                    int temp = excelRangeRowCount;
                    excelRangeRowCount = excelRangeColumnCount;
                    excelRangeColumnCount = temp;
                }

                object[,] returnValue = new object[excelRangeRowCount, excelRangeColumnCount];
                for (int i = 0; i < excelRangeRowCount; i++)
                {
                    for (int j = 0; j < excelRangeColumnCount; j++)
                    {
                        returnValue[i, j] = String.Empty;
                    }
                }
                if ((valueRowCount > excelRangeRowCount) || (valueColumnCount > excelRangeColumnCount))
                {
                    if (rangeOrientation == ExcelLowLevel.OutputRangeOrientation.Regular)
                    {
                        returnValue[0, 0] = "ERROR: At least " + valueRowCount + " x " + valueColumnCount + " Range needed.";
                    }
                    else
                    {
                        returnValue[0, 0] = "ERROR: At least " + valueColumnCount + " x " + valueRowCount + " Range needed.";
                    }
                    return returnValue;
                }

                for (int i = 0; i < valueRowCount; i++)
                {
                    for (int j = 0; j < valueColumnCount; j++)
                    {
                        returnValue[i, j] = values[i][j];
                    }
                }
                return returnValue;
            }
            else  // write the result below the cell where the user has called the specified UDF
            {
                int firstRowIndex, firstColumnIndex;
                var returnValue = ExcelLowLevel.GetCurrentRangePosition(out firstRowIndex, out firstColumnIndex);

                ExcelAsyncUtil.QueueAsMacro(() =>
                {
                    for (int i = 0; i < valueRowCount; i++)
                    {
                        for (int j = 0; j < valueColumnCount; j++)
                        {
                            var cellReference = new ExcelReference(firstRowIndex + i + 1, firstColumnIndex + j);
                            cellReference.SetValue(values[i][j]);
                        }
                    }
                });
                return String.Format("<Below result of> {0}", returnValue).ToTimeStampString();
            }
        }

        /// <summary>Gets the Excel range output for a specific array.
        /// </summary>
        /// <typeparam name="T">The type of the values.</typeparam>
        /// <param name="values">The collection of values.</param>
        /// <param name="header">An optional header.</param>
        /// <param name="rangeOutputType">A value indicating how the values should be presented in Excel.</param>
        /// <returns>The Excel range which contains the <paramref name="values"/>.</returns>
        public static object GetExcelRangeOutput<T>(IEnumerable<T> values, string header = null, RangeOutputType rangeOutputType = RangeOutputType.Standard)
        {
            if (rangeOutputType == RangeOutputType.Standard)
            {
                ExcelLowLevel.OutputRangeOrientation rangeOrientation;
                int excelRangeRowCount, excelRangeColumnCount;
                ExcelLowLevel.GetRangeSize(out excelRangeRowCount, out excelRangeColumnCount, out rangeOrientation);

                if (rangeOrientation == ExcelLowLevel.OutputRangeOrientation.Transposed)  // swap the Excel Range size given by the user
                {
                    int temp = excelRangeRowCount;
                    excelRangeRowCount = excelRangeColumnCount;
                    excelRangeColumnCount = temp;
                }

                object[,] returnValue = new object[excelRangeRowCount, excelRangeColumnCount];
                for (int i = 0; i < excelRangeRowCount; i++)
                {
                    for (int j = 0; j < excelRangeColumnCount; j++)
                    {
                        returnValue[i, j] = String.Empty;
                    }
                }
                int valueRowCount = values.Count() + ((header != null) ? 1 : 0);
                if (valueRowCount > excelRangeRowCount)
                {
                    if (rangeOrientation == ExcelLowLevel.OutputRangeOrientation.Regular)
                    {
                        returnValue[0, 0] = "ERROR: At least " + valueRowCount + " x 1 Range needed.";
                    }
                    else
                    {
                        returnValue[0, 0] = "ERROR: At least 1 x " + valueRowCount + " Range needed.";
                    }
                    return returnValue;
                }

                int k = 0;
                if (header != null)
                {
                    returnValue[k++, 0] = header;
                }
                foreach (T value in values)
                {
                    returnValue[k++, 0] = value;
                }
                return returnValue;
            }
            else  // write the result below the cell where the user has called the specified UDF
            {
                int firstRowIndex, firstColumnIndex;
                ExcelLowLevel.GetCurrentRangePosition(out firstRowIndex, out firstColumnIndex);

                ExcelAsyncUtil.QueueAsMacro(() =>
                {
                    int k = 0;
                    foreach (T value in values)
                    {
                        var cellReference = new ExcelReference(firstRowIndex + k + 1, firstColumnIndex);
                        cellReference.SetValue(value);
                        k++;
                    }
                });
                return ((header != null) ? header : "<unknown>");
            }
        }

        /// <summary>Gets the Excel range output for a specific array.
        /// </summary>
        /// <typeparam name="T1">The type of the first item.</typeparam>
        /// <typeparam name="T2">The type of the second item.</typeparam>
        /// <param name="values">The collection of values.</param>
        /// <param name="header1">An optional header for the first item.</param>
        /// <param name="header2">An optional header for the second item.</param>
        /// <returns>The Excel range which contains the <paramref name="values"/>.</returns>
        public static object GetExcelRangeOutput<T1, T2>(IEnumerable<Tuple<T1, T2>> values, string header1 = null, string header2 = null, RangeOutputType rangeOutputType = RangeOutputType.Standard)
        {
            if (rangeOutputType == RangeOutputType.Standard)
            {
                ExcelLowLevel.OutputRangeOrientation rangeOrientation;
                int excelRangeRowCount, excelRangeColumnCount;
                ExcelLowLevel.GetRangeSize(out excelRangeRowCount, out excelRangeColumnCount, out rangeOrientation);

                if (rangeOrientation == ExcelLowLevel.OutputRangeOrientation.Transposed)  // swap the Excel Range size given by the user
                {
                    int temp = excelRangeRowCount;
                    excelRangeRowCount = excelRangeColumnCount;
                    excelRangeColumnCount = temp;
                }

                object[,] returnValue = new object[excelRangeRowCount, excelRangeColumnCount];
                for (int i = 0; i < excelRangeRowCount; i++)
                {
                    for (int j = 0; j < excelRangeColumnCount; j++)
                    {
                        returnValue[i, j] = String.Empty;
                    }
                }

                bool containsHeader = (header1 != null) || (header2 != null);
                int valueRowCount = values.Count() + (containsHeader == true ? 1 : 0);

                if (valueRowCount > excelRangeRowCount)
                {
                    if (rangeOrientation == ExcelLowLevel.OutputRangeOrientation.Regular)
                    {
                        returnValue[0, 0] = "ERROR: At least " + valueRowCount + " x 2 Range needed.";
                    }
                    else
                    {
                        returnValue[0, 0] = "ERROR: At least 2 x " + valueRowCount + " Range needed.";
                    }
                    return returnValue;
                }

                if (header1 != null)
                {
                    returnValue[0, 0] = header1;
                }
                if (header2 != null)
                {
                    returnValue[0, 1] = header2;
                }
                int k = 0 + (containsHeader == true ? 1 : 0);
                foreach (var value in values)
                {
                    returnValue[k, 0] = value.Item1;
                    returnValue[k++, 1] = value.Item2;
                }
                return returnValue;
            }
            else  // write the result below the cell where the user has called the specified UDF
            {
                int firstRowIndex, firstColumnIndex;
                ExcelLowLevel.GetCurrentRangePosition(out firstRowIndex, out firstColumnIndex);

                ExcelAsyncUtil.QueueAsMacro(() =>
                {
                    int k = 0;
                    foreach (var value in values)
                    {
                        var firstCellReference = new ExcelReference(firstRowIndex + k + 1, firstColumnIndex);
                        firstCellReference.SetValue(value.Item1);

                        var secondCellReference = new ExcelReference(firstRowIndex + k + 1, firstColumnIndex + 1);
                        secondCellReference.SetValue(value.Item2);
                        k++;
                    }
                });

                var returnValue = new object[1, 2];
                if (header1 != null)
                {
                    returnValue[0, 0] = header1;
                }
                if (header2 != null)
                {
                    returnValue[0, 1] = header2;
                }
                return returnValue;
            }
        }

        /// <summary>Gets the excel range error message.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>The <paramref name="errorMessage"/> as some Excel range output.</returns>
        public static object GetExcelRangeErrorMessage(string errorMessage)
        {
            return GetExcelRangeMessage("Error! " + errorMessage);
        }

        /// <summary>Gets the excel range error message.
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> object.</param>
        /// <returns>The exception message of <paramref name="exception"/> as some Excel range output.</returns>
        public static object GetExcelRangeErrorMessage(Exception exception)
        {
            if (exception == null)
            {
                return GetExcelRangeMessage("Error! Unknown error, Exception object is null.");
            }
            return GetExcelRangeMessage("Error! " + exception.Message);
        }

        /// <summary>Gets a specific excel range message, i.e. convert a <see cref="System.String"/> into the Excel Range selected by the user as output of the user defined function. 
        /// </summary>
        /// <param name="message">The  message.</param>
        /// <returns>The <paramref name="message"/> as some Excel range output.</returns>
        public static object GetExcelRangeMessage(string message)
        {
            int excelRangeRowCount, excelRangeColumnCount;
            ExcelLowLevel.GetRangeSize(out excelRangeRowCount, out excelRangeColumnCount);

            if ((excelRangeRowCount == 1) && (excelRangeColumnCount == 1))
            {
                return message;
            }

            object[,] returnValue = new object[excelRangeRowCount, excelRangeColumnCount];
            for (int i = 0; i < excelRangeRowCount; i++)
            {
                for (int j = 0; j < excelRangeColumnCount; j++)
                {
                    returnValue[i, j] = String.Empty;
                }
            }
            returnValue[0, 0] = message;
            return returnValue;
        }
        #endregion

        /// <summary>Determines whether a specific Excel cell is empty.
        /// </summary>
        /// <param name="xlCell">The Excel cell.</param>
        /// <returns><c>true</c> if <paramref name="xlCell"/> represents an empty Excel cell; <c>false</c> otherwise.
        /// </returns>
        public static bool IsEmptyCell(object xlCell)
        {
            return ((xlCell == null) || (xlCell is ExcelEmpty) || (xlCell is ExcelMissing) || ((xlCell is String) && (((string)xlCell).Length == 0)));
        }

        /// <summary>Gets the Excel cell representation of a specific <see cref="System.Object"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns><paramref name="value"/> if <paramref name="value"/> is a standard type (i.e. boolean, integer, DateTime, double etc.); otherwise the <see cref="System.String"/> representation
        /// of <paramref name="value"/> using the <c>ToString()</c> method.</returns>
        public static object GetExcelCellRepresentation(object value)
        {
            if (IsEmptyCell(value) == true)
            {
                return "<empty>";
            }
            Type valueType = value.GetType();

            if ((valueType.IsPrimitive == true) || (value is DateTime) || (value is string))
            {
                return value;
            }
            else if (valueType.IsEnum == true)
            {
                return EnumString.Create((Enum)value, EnumStringRepresentationUsage.StringAttribute);
            }
            return value.ToString();
        }
        #endregion

        #region private methods

        /// <summary>Gets a specific integer, i.e. converts some Excel cell input into its <see cref="System.Int32"/> representation.
        /// </summary>
        /// <param name="excelCell">The excel cell to convert.</param>
        /// <param name="value">The <see cref="System.Int32"/> representation of <paramref name="excelCell"/> (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        private static bool TryGetInteger(object excelCell, out int value)
        {
            if (excelCell is double)
            {
                double dValue = (double)excelCell;
                value = (int)dValue;
                if (value == 0)
                {
                    if (Math.Abs(dValue - value) < 1E-11)
                    {
                        value = 0;
                        return true;
                    }
                }
                else
                {
                    if (Math.Abs((dValue - value) / value) < 1E-11)
                    {
                        return true;
                    }
                }
                value = value + Math.Sign(value);
                return true;
            }
            else if (excelCell is int)
            {
                value = (int)excelCell;
                return true;
            }
            else if (excelCell is string)
            {
                return Int32.TryParse(excelCell as string, out value);
            }

            value = 0;
            return false;
        }

        /// <summary>Gets a specific double-precision floating-point number, i.e. converts some Excel cell input into its <see cref="System.Double"/> representation.
        /// </summary>
        /// <param name="excelCell">The excel cell.</param>
        /// <param name="value">The <see cref="System.Double"/> representation of <paramref name="excelCell"/> (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        private static bool TryGetDouble(object excelCell, out double value)
        {
            if (excelCell is double)
            {
                value = (double)excelCell;
                return true;
            }
            else if (excelCell is int)
            {
                value = (int)excelCell;
                return true;
            }
            else if (excelCell is string)
            {
                return Double.TryParse(excelCell as string, out value);
            }
            value = Double.NaN;
            return false;
        }

        /// <summary>Gets a specific <see cref="System.DateTime"/> object, i.e. converts some Excel cell input into its <see cref="System.DateTime"/> representation.
        /// </summary>
        /// <param name="excelCell">The excel cell.</param>
        /// <param name="value">The <see cref="System.DateTime"/> representation of <paramref name="excelCell"/> (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        private static bool TryGetDateTime(object excelCell, out DateTime value)
        {
            if (excelCell is DateTime)
            {
                value = (DateTime)excelCell;
                return true;
            }
            else if (excelCell is double)
            {
                value = DateTime.FromOADate((double)excelCell);
                return true;
            }
            if (excelCell is string)
            {
                if (DateTime.TryParse(excelCell as string, out value) == true)
                {
                    return true;
                }
                return DateTime.TryParse(excelCell as string, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out  value);
            }
            value = DateTime.MinValue;
            return false;
        }

        /// <summary>Determines whether the value of a specific Excel cell represents 'true'.
        /// </summary>
        /// <param name="idCellValue">The cell value in its <see cref="Dodoni.BasicComponents.IdentifierString"/> representation.</param>
        /// <returns><c>true</c> if the specific Excel cell represents 'true'; otherwise, <c>false</c>.</returns>
        private static bool IsTrueExcelCell(IdentifierString idCellValue)
        {
            return ExcelDataAdvice.Pool.m_BooleanAdvice.TrueString.Equals(idCellValue);
        }

        /// <summary>Determines whether the value of a specific Excel cell represents 'false'.
        /// </summary>
        /// <param name="idCellValue">The cell value in its <see cref="Dodoni.BasicComponents.IdentifierString"/> representation.</param>
        /// <returns><c>true</c> if the specific Excel cell represents 'false'; otherwise, <c>false</c>.</returns>
        private static bool IsFalseExcelCell(IdentifierString idCellValue)
        {
            return ExcelDataAdvice.Pool.m_BooleanAdvice.FalseString.Equals(idCellValue);
        }
        #endregion
    }
}