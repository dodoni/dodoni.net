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

using Dodoni.BasicComponents.Logging;
using Dodoni.BasicComponents.Utilities;
using Dodoni.XLBasicComponents.Logging;

namespace Dodoni.XLBasicComponents.UDF
{
    /// <summary>Serves as Excel interface for logging functionality.
    /// </summary>
    public static class ExcelLogging
    {
        #region private consts

        /// <summary>The Excel category for each exported function in this class.
        /// </summary>
        private const string ExcelCategory = ExcelAddIn.GeneralCategoryName;
        #endregion

        #region public (static) methods

        /// <summary>Gets the message types for the logging.
        /// </summary>
        /// <returns>The message types for the logging.</returns>
        [ExcelFunction(Name = "doListLoggingMsgTypes", Description = "Gets the messeage types for the logging", Category = ExcelCategory)]
        public static object GetLogFileMessageTypes()
        {
            try
            {
                if (ExcelDnaUtil.IsInFunctionWizard())
                {
                    return String.Empty;
                }
                //                var msgTypes = Logger.GetMessageTypes();
                // int count = msgTypes.Count();
                int count = 1;

                object[,] excelOutput = new object[count + 1, 3];
                excelOutput[0, 0] = XLResources.LogFileClassificationHeader;
                excelOutput[0, 1] = XLResources.LogFileNameHeader;
                excelOutput[0, 2] = XLResources.LogFileDescriptionHeader;

                int i = 1;
                //foreach (var msgType in msgTypes)
                //{
                //    excelOutput[i, 0] = msgType.Classification.ToFormatString();
                //    excelOutput[i, 1] = msgType.Name.String;
                //    excelOutput[i, 2] = msgType.LongName.String;
                //    i++;
                //}
                return ExcelDataConverter.GetExcelRangeOutput(excelOutput);
            }
            catch (Exception e)
            {
                return ExcelDataConverter.GetExcelRangeErrorMessage(e.Message);
            }
        }

        /// <summary>Gets the logging of a specific Excel Pool Item.
        /// </summary>
        /// <param name="xlObjectName">The name of the object.</param>
        /// <returns>The logging of the specified object.</returns>
        [ExcelFunction(Name = "doGetLogging", Description = "Get the logging of a specific pool item.", Category = ExcelCategory)]
        public static object GetExcelPoolItemLogging(
            [ExcelArgument(Name = "objectName", Description = "The name of the object", AllowReference = true)]
             object xlObjectName)
        {
            try
            {
                IExcelDataQuery objectNameQuery = ExcelDataQuery.Create(xlObjectName);
                string objectName;
                if (objectNameQuery.TryGetValue<string>(out objectName, dataAdvice: ExcelDataAdvice.Create(ExcelPool.GetObjectNames())) != ExcelCellValueState.ProperValue)
                {
                    return ExcelDataConverter.GetExcelRangeErrorMessage("Invalid object name '" + objectNameQuery.ToString(0, 0) + "'.");
                }
                objectNameQuery.QueryCompleted();

                ILoggedObject loggingObject;
                if (ExcelPool.TryGetObject<ILoggedObject>(objectName, out loggingObject) == false)
                {
                    return ExcelDataConverter.GetExcelRangeErrorMessage("No object found of name '" + objectName + "' which supports logging functionality.");
                }
                var excelLogFile = loggingObject.Logging as ExcelObjectLogger;
                if (excelLogFile == null)
                {
                    return ExcelDataConverter.GetExcelRangeErrorMessage("No log file available.");
                }
                // return ExcelDataConverter.GetExcelRangeOutput<string>(excelLogFile.GetAsStringArray());
                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                return ExcelDataConverter.GetExcelRangeErrorMessage(e.Message);
            }
        }
        #endregion
    }
}