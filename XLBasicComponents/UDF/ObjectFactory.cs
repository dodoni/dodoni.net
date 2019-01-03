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
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;

using ExcelDna.Integration;
using Dodoni.BasicComponents;
using Dodoni.XLBasicComponents.IO;
using Dodoni.XLBasicComponents.Utilities;

namespace Dodoni.XLBasicComponents.UDF
{
    /// <summary>Excel functions for loading objects from a specific file and saving objects from the <see cref="ExcelPool"/> into a file with a specific format.
    /// </summary>
    public static class ObjectFactory
    {
        #region private consts

        /// <summary>The Excel category for each exported function in this class.
        /// </summary>
        private const string ExcelCategory = ExcelAddIn.GeneralCategoryName;
        #endregion

        #region public static methods

        /// <summary>Loads objects from a specific file.
        /// </summary>
        /// <param name="xlPath">The path.</param>
        /// <param name="xlFileName">The name of the file.</param>
        /// <param name="xlObjectNames">The (optional) object names to load.</param>
        /// <param name="xlFileFormat">The (optional) file format; the file extension will be used by default.</param>
        /// <returns></returns>
        [ExcelFunction(Name = "doLoadObjects", Description = "Load objects form a specific file", Category = ExcelCategory)]
        public static object LoadObjectsFromFile(
            [ExcelArgument(Name = "path", Description = "The path", AllowReference = true)]
            object xlPath,
            [ExcelArgument(Name = "fileName", Description = "The file name", AllowReference = true)]
            object xlFileName,
            [ExcelArgument(Name = "objectNames", Description = "[Optional] A list of object names to load; all objects will be loaded by default", AllowReference = true)]
            object xlObjectNames,
            [ExcelArgument(Name = "fileFormat", Description = "[Optional] The file format; the file extension will be used by default", AllowReference = true)]
            object xlFileFormat)
        {
            try
            {
                IExcelDataQuery fileFormatDataQuery = ExcelDataQuery.Create(xlFileFormat, "File format");

                string fileName;
                IObjectStreamer objectStreamer;
                if (fileFormatDataQuery.IsEmpty == true)
                {
                    fileName = GetFileName(xlPath, xlFileName, ObjectStreamer.GetFileExtensions());

                    if (ObjectStreamer.TryGetObjectStreamerByFileExtension(ExtendedPath.GetExtension(fileName), out objectStreamer) == false)
                    {
                        throw new ArgumentException("Invalid file extension '" + Path.GetExtension(fileName) + "', used default file extensions or specify the file format.");
                    }
                }
                else
                {
                    if (fileFormatDataQuery.TryGetPoolValue<IObjectStreamer>(ObjectStreamer.TryGetObjectStreamer, out objectStreamer, dataAdvice: ExcelDataAdvice.Create(ObjectStreamer.GetNames())) == false)
                    {
                        throw new ArgumentException("Invalid file format " + fileFormatDataQuery.ToString(0, 0) + ".");
                    }
                    fileName = GetFileName(xlPath, xlFileName, objectStreamer.FileExtension);
                    fileFormatDataQuery.QueryCompleted();
                }

                IExcelDataQuery objectNamesDataQuery = ExcelDataQuery.Create(xlObjectNames, "Object names");
                IEnumerable<string> objectNames = objectNamesDataQuery.GetColumnVector<string>();
                objectNamesDataQuery.QueryCompleted();

                StreamReader streamReader = new StreamReader(fileName);
                IObjectStreamReader objectStreamReader = objectStreamer.GetStreamReader(streamReader);
                string infoMessage;
                IEnumerable<ExcelPoolItem> excelPoolItems;
                ExcelPool.TryLoadObjectsByName(objectStreamReader, objectNames, out infoMessage, out excelPoolItems);
                objectStreamReader.Close();

                return infoMessage.ToTimeStampString();
            }
            catch (Exception e)
            {
                return ExcelDataConverter.GetExcelRangeErrorMessage(e);
            }
        }

        /// <summary>List objects in a specific file.
        /// </summary>
        /// <param name="xlPath">The path.</param>
        /// <param name="xlFileName">The name of the file.</param>
        /// <param name="xlFileFormat">The (optional) file format; the file extension will be used by default.</param>
        /// <returns></returns>
        [ExcelFunction(Name = "doListObjects", Description = "List objects in a specific file", Category = ExcelCategory)]
        public static object ListObjectsFromFile(
            [ExcelArgument(Name = "path", Description = "The path", AllowReference = true)]
            object xlPath,
            [ExcelArgument(Name = "fileName", Description = "The file name", AllowReference = true)]
            object xlFileName,
            [ExcelArgument(Name = "fileFormat", Description = "[Optional] The file format; the file extension will be used by default", AllowReference = true)]
            object xlFileFormat)
        {
            try
            {
                IExcelDataQuery fileFormatDataQuery = ExcelDataQuery.Create(xlFileFormat, "File format");

                string fileName;
                IObjectStreamer objectStreamer;

                if (fileFormatDataQuery.IsEmpty == true)
                {
                    fileName = GetFileName(xlPath, xlFileName, ObjectStreamer.GetFileExtensions());

                    if (ObjectStreamer.TryGetObjectStreamerByFileExtension(ExtendedPath.GetExtension(fileName), out objectStreamer) == false)
                    {
                        throw new ArgumentException("Invalid file extension '" + ExtendedPath.GetExtension(fileName) + "', used default file extensions or specify the file format.");
                    }
                }
                else
                {
                    if (fileFormatDataQuery.TryGetPoolValue<IObjectStreamer>(ObjectStreamer.TryGetObjectStreamer, out objectStreamer, dataAdvice: ExcelDataAdvice.Create(ObjectStreamer.GetNames())) == false)
                    {
                        throw new ArgumentException("Invalid file format " + fileFormatDataQuery.ToString(0, 0) + ".");
                    }
                    fileName = GetFileName(xlPath, xlFileName, objectStreamer.FileExtension);
                    fileFormatDataQuery.QueryCompleted();
                }

                StreamReader streamReader = new StreamReader(fileName);
                IObjectStreamReader objectStreamReader = objectStreamer.GetStreamReader(streamReader);

                var objectNames = ExcelPool.GetObjectNames(objectStreamReader);
                int n = objectNames.Count();
                object[,] output = new object[n + 1, 2];
                output[0, 0] = "Name";
                output[0, 1] = "Type";
                int j = 1;
                foreach (var obj in objectNames)
                {
                    output[j, 0] = obj.Item2;
                    output[j, 1] = obj.Item1.Name.String;
                    j++;
                }
                objectStreamReader.Close();
                return ExcelDataConverter.GetExcelRangeOutput(output);
            }
            catch (Exception e)
            {
                return ExcelDataConverter.GetExcelRangeErrorMessage(e);
            }
        }

        /// <summary>Write objects into a specific file.
        /// </summary>
        /// <param name="xlPath">The path.</param>
        /// <param name="xlFileName">The name of the file.</param>
        /// <param name="xlObjectNames">The (optional) object names to load.</param>
        /// <param name="xlFileFormat">The (optional) file format; the file extension will be used by default.</param>
        /// <returns></returns>
        [ExcelFunction(Name = "doWriteObjects", Description = "Write objects into a specific file", Category = ExcelCategory)]
        public static object WriteObjectsIntoFile(
            [ExcelArgument(Name = "path", Description = "The path", AllowReference = true)]
            object xlPath,
            [ExcelArgument(Name = "fileName", Description = "The file name", AllowReference = true)]
            object xlFileName,
            [ExcelArgument(Name = "objectNames", Description = "A [optional] list of object names to write.", AllowReference = true)]
            object xlObjectNames,
            [ExcelArgument(Name = "fileFormat", Description = "[Optional] The file format; the file extension will be used by default", AllowReference = true)]
            object xlFileFormat)
        {
            try
            {
                IExcelDataQuery fileFormatDataQuery = ExcelDataQuery.Create(xlFileFormat, "File format");

                string fileName;
                IObjectStreamer objectStreamer;

                if (fileFormatDataQuery.IsEmpty == true)
                {
                    fileName = GetFileName(xlPath, xlFileName, ObjectStreamer.GetFileExtensions());

                    if (ObjectStreamer.TryGetObjectStreamerByFileExtension(ExtendedPath.GetExtension(fileName), out objectStreamer) == false)
                    {
                        throw new ArgumentException("Invalid file extension '" + ExtendedPath.GetExtension(fileName) + "', used default file extensions or specify the file format.");
                    }
                }
                else
                {
                    if (fileFormatDataQuery.TryGetPoolValue<IObjectStreamer>(ObjectStreamer.TryGetObjectStreamer, out objectStreamer, dataAdvice: ExcelDataAdvice.Create(ObjectStreamer.GetNames())) == false)
                    {
                        throw new ArgumentException("Invalid file format " + fileFormatDataQuery.ToString(0, 0) + ".");
                    }
                    fileName = GetFileName(xlPath, xlFileName, objectStreamer.FileExtension);
                    fileFormatDataQuery.QueryCompleted();
                }

                IExcelDataQuery objectNamesDataQuery = ExcelDataQuery.Create(xlObjectNames);
                IEnumerable<string> objectNames = objectNamesDataQuery.GetColumnVector<string>();
                objectNamesDataQuery.QueryCompleted();

                StreamWriter streamWriter = new StreamWriter(fileName, false);
                IObjectStreamWriter objectStreamWriter = objectStreamer.GetStreamWriter(streamWriter);
                string infoMessage;
                ExcelPool.TryWriteObjectsByName(objectStreamWriter, objectNames, out infoMessage);
                objectStreamWriter.Close();

                return infoMessage.ToTimeStampString();
            }
            catch (Exception e)
            {
                return ExcelDataConverter.GetExcelRangeErrorMessage(e.Message);
            }
        }
        #endregion

        #region private methods

        /// <summary>Gets the name of the file.
        /// </summary>
        /// <param name="xlPath">The path.</param>
        /// <param name="xlFileName">The name of the file (without path).</param>
        /// <param name="fileExtensionSearchPattern">The file extension to search.</param>
        /// <returns>The file name including path.</returns>
        private static string GetFileName(object xlPath, object xlFileName, IdentifierString fileExtensionSearchPattern)
        {
            IExcelDataQuery pathDataQuery = ExcelDataQuery.Create(xlPath);
            IExcelDataQuery fileNameDataQuery = ExcelDataQuery.Create(xlFileName);

            string path;
            if (pathDataQuery.TryGetValue<string>(out path) != ExcelCellValueState.ProperValue)
            {
                throw new ArgumentException("No valid path!");
            }
            string[] possibleFileNames = Directory.GetFiles(path, "*." + fileExtensionSearchPattern.String, SearchOption.TopDirectoryOnly);

            string fileName;
            if (fileNameDataQuery.TryGetValue<string>(out fileName, dataAdvice: ExcelDataAdvice.Create(possibleFileNames, possibleFileName => Path.GetFileName(possibleFileName))) != ExcelCellValueState.ProperValue)
            {
                throw new ArgumentException("No valid file name!");
            }
            return Path.Combine(path, fileName);
        }

        /// <summary>Gets the name of the file.
        /// </summary>
        /// <param name="xlPath">The path.</param>
        /// <param name="xlFileName">The name of the file (without path).</param>
        /// <param name="fileExtensionSearchPattern">A collection of possible file extensions.</param>
        /// <returns>The file name including path.</returns>
        private static string GetFileName(object xlPath, object xlFileName, IEnumerable<IdentifierString> fileExtensionSearchPattern)
        {
            IExcelDataQuery pathDataQuery = ExcelDataQuery.Create(xlPath);
            IExcelDataQuery fileNameDataQuery = ExcelDataQuery.Create(xlFileName);

            string path;
            if (pathDataQuery.TryGetValue<string>(out path) != ExcelCellValueState.ProperValue)
            {
                throw new ArgumentException("No valid path!");
            }

            List<string> possibleFileNameList = new List<string>();
            foreach (var fileExtension in fileExtensionSearchPattern)
            {
                string[] possibleFileNames = Directory.GetFiles(path, "*." + fileExtension.String, SearchOption.TopDirectoryOnly);
                if ((possibleFileNames != null) && (possibleFileNames.Length > 0))
                {
                    possibleFileNameList.AddRange(possibleFileNames);
                }
            }

            string fileName;
            if (fileNameDataQuery.TryGetValue<string>(out fileName, dataAdvice: ExcelDataAdvice.Create(possibleFileNameList, possibleFileName => Path.GetFileName(possibleFileName))) != ExcelCellValueState.ProperValue)
            {
                throw new ArgumentException("No valid file name!");
            }
            return Path.Combine(path, fileName);
        }
        #endregion
    }
}