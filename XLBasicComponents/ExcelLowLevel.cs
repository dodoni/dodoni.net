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
using System.Globalization;
using System.Collections.Generic;

using Dodoni.BasicComponents.Utilities;

using ExcelDna.Integration;
using Dodoni.BasicComponents.Logging;
using Dodoni.XLBasicComponents.Logging;

namespace Dodoni.XLBasicComponents
{
    /// <summary>Contains low-level Excel methods to enhance the original ExcelDNA interface.
    /// </summary>
    public static class ExcelLowLevel
    {
        #region nested enumerations

        /// <summary>Represents whether a drop-down list for a specific Excel cell will be created.
        /// </summary>
        internal enum DropDownListCreationType
        {
            /// <summary>No drop-down list will be create.
            /// </summary>
            [String("None")]
            None,

            /// <summary>A drop-down list will be create for empty cells only.
            /// </summary>
            [String("For empty Excel cells only")]
            EmptyExcelCells,

            /// <summary>A drop-down list will be create for Excel cells only which do not contained a formula.
            /// </summary>
            [String("For Excel cells without formula")]
            ExcelCellsWithoutFormula,

            /// <summary>A drop-down list will be create in any case.
            /// </summary>
            [String("Always")]
            Always
        }

        /// <summary>The orientation of a specific Excel Range output.
        /// </summary>
        internal enum OutputRangeOrientation
        {
            /// <summary>The output will be take place without modifications, i.e. a one-dimensional array will be shown row-wise, the representation of a two-dimensional array will not be
            /// changed, i.e. the first index represents the row, the second the column.
            /// </summary>
            Regular,

            /// <summary>The output will be transposed, i.e. a one-dimensional array will be shown column-wise and the first index of a two-dimensional array will
            /// be interpreted as column index, the second index is the row index.
            /// </summary>
            Transposed
        }

        /// <summary>Specifies the type of validation test to be performed in conjunction with values.
        /// </summary>
        /// <remarks>This enumeration is part of Excel Object model (C) Microsoft.</remarks>
        private enum XlDVType
        {
            /// <summary>Only validate when user changes the value.
            /// </summary>
            xlValidateInputOnly = 0,

            /// <summary>Whole numeric values.
            /// </summary>
            xlValidateWholeNumber = 1,

            /// <summary>Numeric values.
            /// </summary>
            xlValidateDecimal = 2,

            /// <summary>Value must be present in a specified list.
            /// </summary>
            xlValidateList = 3,

            /// <summary>Date values.
            /// </summary>
            xlValidateDate = 4,

            /// <summary>Time values.
            /// </summary>
            xlValidateTime = 5,

            /// <summary>Length of text.
            /// </summary>
            xlValidateTextLength = 6,

            /// <summary>Data is validated using an arbitrary formula.
            /// </summary>
            xlValidateCustom = 7
        }

        /// <summary>Specifies the icon used in message boxes displayed during validation.
        /// </summary>
        /// <remarks>This enumeration is part of Excel Object model (C) Microsoft.</remarks>
        private enum XlDVAlertStyle
        {
            /// <summary>Stop icon.
            /// </summary>
            xlValidAlertStop = 1,

            /// <summary>Warning icon.
            /// </summary>
            xlValidAlertWarning = 2,

            /// <summary>Information icon.
            /// </summary>
            xlValidAlertInformation = 3
        }

        /// <summary>Specifies the operator to use to compare a formula against the value in a
        /// cell or, for xlBetween and xlNotBetween, to compare two formulas.
        /// </summary>
        /// <remarks>This enumeration is part of Excel Object model (C) Microsoft.</remarks>
        private enum XlFormatConditionOperator
        {
            /// <summary>Between. Can only be used if two formulas are provided.
            /// </summary>
            xlBetween = 1,

            /// <summary>Not between. Can only be used if two formulas are provided.
            /// </summary>
            xlNotBetween = 2,

            /// <summary>Equal.
            /// </summary>
            xlEqual = 3,

            /// <summary>Not equal.
            /// </summary>
            xlNotEqual = 4,

            /// <summary>Greater than.
            /// </summary>
            xlGreater = 5,

            /// <summary>Less than.
            /// </summary>
            xlLess = 6,

            /// <summary>Greater than or equal to.
            /// </summary>
            xlGreaterEqual = 7,

            /// <summary>Less than or equal to.
            /// </summary>
            xlLessEqual = 8,
        }

        /// <summary>VBA Extensibility Library: The kind of procedure that the specified line is contained in.
        /// </summary>
        /// <remarks>This enumeration is part of Excel Object model (C) Microsoft; VBA Extensibility Library.</remarks>
        private enum vbext_ProcKind
        {
            /// <summary>Regular Procedure (Sub or Function).
            /// </summary>
            vbext_pk_Proc = 0,

            /// <summary>Property Let Procedure.
            /// </summary>
            vbext_pk_Let = 1,

            /// <summary>Property Set Procedure.
            /// </summary>
            vbext_pk_Set = 2,

            /// <summary>Property Get Procedure.
            /// </summary>
            vbext_pk_Get = 3,
        }
        #endregion

        #region private (static) members

        /// <summary>A value indicating whether a drop down list with data advice will be created.
        /// </summary>
        private static DropDownListCreationType sm_UseDataAdvice = DropDownListCreationType.EmptyExcelCells;

        /// <summary>The XML tag in the configuration file for the 'use data advice' checkbox.
        /// </summary>
        private const string m_UseDataAdviceConfigKey = "UseDataAdvice";

        /// <summary>A value indicating whether a list with the names of Excel.VBA functions will be created for user data advice.
        /// </summary>
        private static bool sm_UseVBADataAdvice = true;

        /// <summary>The XML tag in the configuration file for the 'use VBA method list data advice' checkbox.
        /// </summary>
        private const string m_UseVBADataAdviceConfigKey = "UseVBADataAdvice";
        #endregion

        #region static constructor

        /// <summary>Initializes the <see cref="ExcelLowLevel"/> class.
        /// </summary>
        static ExcelLowLevel()
        {
            GetUseDataAdviceFromConfigFile();
            GetUseVBADataAdviceFromConfigFile();
        }
        #endregion

        #region public (static) properties

        /// <summary>Gets a value indicating whether <see cref="CreateDropdownList(ExcelDna.Integration.ExcelReference, int, int, string)"/> creates a drop down list with user data advice (if applictable).
        /// </summary>
        /// <value>A value indicating whether <see cref="CreateDropdownList(ExcelDna.Integration.ExcelReference, int, int, string)"/> creates a drop-down list.</value>
        internal static DropDownListCreationType UseDataAdvice
        {
            get { return sm_UseDataAdvice; }
        }

        /// <summary>Gets a value indicating whether <see cref="GetVBAFunctionNames()"/> returns <c>null</c>.
        /// </summary>
        /// <value>
        /// <c>false</c> if <see cref="GetVBAFunctionNames()"/> returns <c>null</c>; otherwise, <c>false</c>.
        /// </value>
        public static bool UseVBADataAdvice
        {
            get { return sm_UseVBADataAdvice; }
        }
        #endregion

        #region public (static) methods

        /// <summary>Creates a dropdown list at a specific Excel cell.
        /// </summary>
        /// <param name="excelRange">The Excel range given as a single row or single column containing values of some properties.</param>
        /// <param name="rowIndex">The null-based row index of the <paramref name="excelRange"/> where to add a dropdown list.</param>
        /// <param name="columnIndex">The null-based column index of the <paramref name="excelRange"/> where to add a dropdown list.</param>
        /// <param name="dropDownListAsString">The semicolon separated <see cref="System.String"/> representation of the dropdown list to add.</param>
        /// <remarks>This method adds a specific (Excel range) data validation with respect to a specific Excel position of <paramref name="excelRange"/> 
        /// and the dropdown list will contains the elements of the <paramref name="dropDownListAsString"/>.</remarks>
        public static void CreateDropdownList(this ExcelDna.Integration.ExcelReference excelRange, int rowIndex, int columnIndex, string dropDownListAsString)
        {
            if ((sm_UseDataAdvice != DropDownListCreationType.None) && (dropDownListAsString != null) && (dropDownListAsString.Length > 0))
            {
                try
                {
                    if (ExcelDna.Integration.ExcelDnaUtil.IsInFunctionWizard() == false)
                    {
                        dynamic sheet = ExcelAddIn.ExcelApplication.ActiveSheet;
                        dynamic cell = sheet.Cells[excelRange.RowFirst + rowIndex + 1, excelRange.ColumnFirst + columnIndex + 1];  // in Excel rows/columns are one-based

                        if (((sm_UseDataAdvice == DropDownListCreationType.EmptyExcelCells) && (ExcelDataConverter.IsEmptyCell(cell.Value) == true))
                            || ((sm_UseDataAdvice == DropDownListCreationType.ExcelCellsWithoutFormula) && (cell.HasFormula == false))
                            || (sm_UseDataAdvice == DropDownListCreationType.Always))
                        {
                            cell.Validation.Delete();
                            cell.Validation.Add(XlDVType.xlValidateList, XlDVAlertStyle.xlValidAlertInformation, XlFormatConditionOperator.xlBetween, dropDownListAsString, Type.Missing);  // todo: Excel has a restriction
                            cell.Validation.ShowError = false;
                        }
                    }
                }
                catch (Exception e)
                {
                  //  Logger.Stream.Add_Info_ExcelCellDropdownListFails(exception: e);
                }
            }
        }

        /// <summary>Gets the names of (visible) VBA functions.
        /// </summary>
        /// <returns>A collection of VBA function names, perhaps <c>null</c>.</returns>
        public static IEnumerable<String> GetVBAFunctionNames()
        {
            if (sm_UseVBADataAdvice == false)
            {
                return null;
            }
            List<string> functionNameList = new List<string>();
            try
            {
                foreach (var vbComponent in ExcelAddIn.ExcelApplication.ActiveWorkbook.VBProject.VBComponents)
                {
                    int startLine = 1 + vbComponent.CodeModule.CountOfDeclarationLines;
                    while (startLine <= vbComponent.CodeModule.CountOfLines)
                    {
                        dynamic procKind;
                        string codeName = vbComponent.CodeModule.get_ProcOfLine(startLine, out procKind);
                        if (procKind == vbext_ProcKind.vbext_pk_Proc)
                        {
                            /* it could be a function or a sub-routine. Here, we would like to use functions only, 
                             * i.e. we test whether the keyword 'Function' is contained in the body of the procedure: */

                            int rowIndex = vbComponent.CodeModule.get_ProcBodyLine(codeName, procKind);
                            string bodyAsString = vbComponent.CodeModule.get_Lines(rowIndex, 1);

                            if (bodyAsString.ToLower().Contains("function ") == true)
                            {
                                functionNameList.Add(codeName);
                            }
                        }
                        startLine += vbComponent.CodeModule.get_ProcCountLines(codeName, vbext_ProcKind.vbext_pk_Proc);
                    }
                }
                return functionNameList;
            }
            catch
            {
                return functionNameList;
            }
        }
        #endregion

        #region internal (static) methods

        #region configuration methods

        /// <summary>Sets the <see cref="UseDataAdvice"/> flag and write the flag into the config file.
        /// </summary>
        /// <param name="state">The <see cref="DropDownListCreationType"/> object to set the <see cref="UseDataAdvice"/> flag.</param>
        internal static void StoreUseDataAdvice(DropDownListCreationType state)
        {
            ExcelAddIn.Configuration.GeneralSettings.SetValue(m_UseDataAdviceConfigKey, state);
            sm_UseDataAdvice = state;
        }

        /// <summary>Gets the <see cref="UseDataAdvice"/> flag from the config file.
        /// </summary>
        /// <returns>The <see cref="UseDataAdvice"/> flag with respect to the config file or some standard value.</returns>
        internal static DropDownListCreationType GetUseDataAdviceFromConfigFile()
        {
            if (ExcelAddIn.Configuration.GeneralSettings.TryGetEnumValue(m_UseDataAdviceConfigKey, out sm_UseDataAdvice) == false)
            {
                sm_UseDataAdvice = DropDownListCreationType.EmptyExcelCells;
            }
            return sm_UseDataAdvice;
        }

        /// <summary>Sets the <see cref="UseVBADataAdvice"/> flag and write the flag into the config file.
        /// </summary>
        /// <param name="state">The <see cref="System.Boolean"/> to set the <see cref="UseVBADataAdvice"/> flag.</param>
        internal static void StoreUseVBADataAdvice(bool state)
        {
            ExcelAddIn.Configuration.GeneralSettings.SetValue(m_UseVBADataAdviceConfigKey, state);
            sm_UseVBADataAdvice = state;
        }

        /// <summary>Gets the <see cref="UseVBADataAdvice"/> flag via the config file.
        /// </summary>
        /// <returns>The loaded <see cref="UseVBADataAdvice"/> flag.</returns>
        internal static bool GetUseVBADataAdviceFromConfigFile()
        {
            ExcelAddIn.Configuration.GeneralSettings.TryGetValue(m_UseVBADataAdviceConfigKey, out sm_UseVBADataAdvice, defaultValue: true);
            return sm_UseVBADataAdvice;
        }
        #endregion

        /// <summary>Gets the size of the Excel range which is selected by the user (matrix functions).
        /// </summary>
        /// <param name="rangeRowCount">The number of row of the Excel range.</param>
        /// <param name="rangeColumnCount">The number of columns of the Excel range.</param>
        internal static void GetRangeSize(out int rangeRowCount, out int rangeColumnCount)
        {
            try
            {
                ExcelReference reference = (ExcelReference)XlCall.Excel(XlCall.xlfCaller);
                rangeRowCount = 1 + reference.RowLast - reference.RowFirst;
                rangeColumnCount = 1 + reference.ColumnLast - reference.ColumnFirst;
            }
            catch (XlCallException e)  // sometimes this Exception occurs - why?
            {
                rangeRowCount = rangeColumnCount = 1;  // todo: just a workaround!
//                Logger.Stream.Add_FatalError_ExcelRangeSizeProblem(exception: e);
            }
        }

        /// <summary>Gets the size of the Excel range which is selected by the user (matrix functions).
        /// </summary>
        /// <param name="rangeRowCount">The number of row of the Excel range.</param>
        /// <param name="rangeColumnCount">The number of columns of the Excel range.</param>
        /// <param name="excelRangeOutput">The Excel Range output type, i.e. perhaps the user transposes the output.</param>
        internal static void GetRangeSize(out int rangeRowCount, out int rangeColumnCount, out OutputRangeOrientation excelRangeOutput)
        {
            ExcelReference reference;
            try
            {
                reference = (ExcelReference)XlCall.Excel(XlCall.xlfCaller);
                rangeRowCount = 1 + reference.RowLast - reference.RowFirst;
                rangeColumnCount = 1 + reference.ColumnLast - reference.ColumnFirst;

            }
            catch (XlCallException e)  // sometimes this Exception occurs - why?
            {
                rangeRowCount = rangeColumnCount = 1;  // just a workaround!
                excelRangeOutput = OutputRangeOrientation.Regular;
//                Logger.Stream.Add_FatalError_ExcelRangeSizeProblem(exception: e);
                return;
            }

            excelRangeOutput = OutputRangeOrientation.Regular;

            try
            {
                /* It is not clear how we can check whether the user wants a transposed representation.
                 * Here, we check whether the formula starts with '=Transpose(' (i.e. '=Mtrans(' in german). Thus a nested 
                 * or iterated Transpose command will be interpreted in a wrong way! */

                dynamic sheet = ExcelAddIn.ExcelApplication.ActiveSheet;
                dynamic cell = sheet.Cells[reference.RowFirst + 1, reference.ColumnFirst + 1];  // in Excel rows/columns are one-based

                if (cell.HasFormula == true)
                {
                    string formula = cell.Formula;
                    if ((formula != null) && (formula.Length > 0))
                    {
                        if (formula.StartsWith("=TRANSPOSE(", true, CultureInfo.InvariantCulture) == true)
                        {
                            excelRangeOutput = OutputRangeOrientation.Transposed;
                        }
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>Gets the position (and the formula) of the Excel range which is currently used by a specified UDF; perhaps a matrix function.
        /// </summary>
        /// <param name="firstRowIndex">The null-based index of the first row.</param>
        /// <param name="firstColumnIndex">The null-based index of the first column.</param>
        /// <returns>The formula of the current cell in its <see cref="System.String"/> representation.</returns>
        internal static string GetCurrentRangePosition(out int firstRowIndex, out int firstColumnIndex)
        {
            ExcelReference reference;
            try
            {
                reference = (ExcelReference)XlCall.Excel(XlCall.xlfCaller);
                firstRowIndex = reference.RowFirst;
                firstColumnIndex = reference.ColumnFirst;
            }
            catch (XlCallException e)  // sometimes this Exception occurs - why?
            {
                // Logger.Stream.Add_FatalError_ExcelRangeSizeProblem(exception: e);
                throw new Exception("", e);
            }

            try
            {
                dynamic sheet = ExcelAddIn.ExcelApplication.ActiveSheet;
                dynamic cell = sheet.Cells[reference.RowFirst + 1, reference.ColumnFirst + 1];  // in Excel rows/columns are one-based

                if (cell.HasFormula == true)
                {
                    string formula = cell.Formula;
                    if ((formula != null) && (formula.Length > 0))
                    {
                        return formula;
                    }
                }
            }
            catch
            {
            }
            return "<unknown>";
        }
        #endregion
    }
}