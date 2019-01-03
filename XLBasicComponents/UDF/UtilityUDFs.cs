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
using System.Globalization;
using System.Collections.Generic;

using ExcelDna.Integration;
using Dodoni.BasicComponents;

namespace Dodoni.XLBasicComponents.UDF
{
    /// <summary>Contains user defined functions (UDF) for Excel which are useful for daily business.
    /// </summary>
    public static class UtilityUDFs
    {
        #region private consts

        /// <summary>The Excel category for each exported function in this class.
        /// </summary>
        private const string ExcelCategory = ExcelAddIn.GeneralCategoryName;
        #endregion

        #region public (static) methods

        /// <summary>Gets the relevant substring, i.e. returns a copy of the argument, where each character followed by <see cref="IdentifierString.IgnoringStartCharacter"/> will be suppress.
        /// </summary>
        /// <param name="xlString">The string.</param>
        /// <returns>A copy of <paramref name="xlString"/> where each character followed by <see cref="IdentifierString.IgnoringStartCharacter"/> will be removed.</returns>
        [ExcelFunction(Name = "do.GetRelevantSubString", Description = "Gets the relevant substring, i.e. returns a copy of the argument, where each character followed by '@' will be suppress", Category = ExcelCategory)]
        public static string GetRelevantSubString(
            [ExcelArgument(Name = "string", Description = "The string representation to convert")]
            string xlString)
        {
            if (xlString == null)
            {
                return null;
            }
            return xlString.GetRelevantSubstring();
        }

        /// <summary>Determines whether the result of a specific Dodoni.net UDF is meaningful.
        /// </summary>
        /// <param name="xlDodoniUDFResult">The result of a specific Dodoni.net UDF.</param>
        /// <returns><c>False</c> if <paramref name="xlDodoniUDFResult"/> does not contain a meaningful result of a Dodoni.net UDF; otherwise <c>true</c>.</returns>
        [ExcelFunction(Name = "doIsError", Description = "Checks whether the result of a Dodoni.net user defined function is meaningful", Category = ExcelCategory)]
        public static object IsError(
            [ExcelArgument(Name = "dodoni UDF result", Description = "The Excel cell to check whether the result of a Dodoni.net UDF is meaningful")]
            object xlDodoniUDFResult)
        {
            IExcelDataQuery excelDataQuery = ExcelDataQuery.Create(xlDodoniUDFResult);
            string value;
            if (excelDataQuery.TryGetValue<string>(out value) == ExcelCellValueState.ProperValue)
            {
                return value.StartsWith("Error!", true, CultureInfo.InvariantCulture);  // here, we just check whether some Error message starts with "Error!"
            }
            return false;
        }

        /// <summary>Determines whether the result of a specific Dodoni.net UDF is meaningful and return <see cref="System.String.Empty"/> if not; otherwise return <paramref name="xlDodoniUDFResult"/>.
        /// </summary>
        /// <param name="xlDodoniUDFResult">The result of a specific Dodoni.net UDF.</param>
        /// <returns><see cref="String.Empty"/> if <paramref name="xlDodoniUDFResult"/> represents an error message; <paramref name="xlDodoniUDFResult"/> otherwise.</returns>
        [ExcelFunction(Name = "doTryEvaluateUDF", Description = "Returns '' if the input (i.e. the result of a Dodoni.net user defined function) is not meaningful; otherwise return the input of this UDF", Category = ExcelCategory)]
        public static object TryEvaluateUDF(
            [ExcelArgument(Name = "dodoni UDF result", Description = "The Excel cell to check whether the result of a Dodoni.net UDF is meaningful")]
            object xlDodoniUDFResult)
        {
            IExcelDataQuery excelDataQuery = ExcelDataQuery.Create(xlDodoniUDFResult);
            string value;

            if (excelDataQuery.TryGetValue<string>(out value) == ExcelCellValueState.ProperValue)
            {
                if (value.StartsWith("Error!", true, CultureInfo.InvariantCulture) == true)  // here, we just check whether some Error message starts with "Error!"
                {
                    if (excelDataQuery.IsSingleCell == true)
                    {
                        return String.Empty;
                    }
                    object[,] emptyOutput = new object[excelDataQuery.RowCount, excelDataQuery.ColumnCount];
                    for (int j = 0; j < excelDataQuery.RowCount; j++)
                    {
                        for (int k = 0; k < excelDataQuery.ColumnCount; k++)
                        {
                            emptyOutput[j, k] = String.Empty;
                        }
                    }
                    return emptyOutput;
                }
            }
            return xlDodoniUDFResult;
        }
        #endregion
    }
}