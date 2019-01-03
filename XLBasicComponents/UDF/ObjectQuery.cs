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
using System.Data;
using System.Collections.Generic;

using ExcelDna.Integration;
using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Utilities;
using Dodoni.BasicComponents.Containers;

namespace Dodoni.XLBasicComponents.UDF
{
    /// <summary>Excel functions to list input and output properties/tables (=<see cref="IExcelDataQuery"/>) of specific <see cref="ExcelPool"/> items.
    /// </summary>
    public static class ObjectQuery
    {
        #region nested enumerations

        /// <summary>The type of the data Query, i.e. a property or a table.
        /// </summary>
        private enum eQueryType
        {
            /// <summary>The properties are requested.
            /// </summary>
            [String("Properties")]
            PropertyNames,

            /// <summary>The tables are requested.
            /// </summary>
            [String("Tables")]
            TableNames
        }
        #endregion

        #region private consts

        /// <summary>The Excel category for each exported function in this class.
        /// </summary>
        private const string ExcelCategory = ExcelAddIn.GeneralCategoryName;

        /// <summary>The way of interpreting whether a specific Excel cell is empty.
        /// </summary>
        private const GuidedExcelDataQuery.eEmptyExcelCellMode m_EmptyExcelCellMode = GuidedExcelDataQuery.eEmptyExcelCellMode.Standard | GuidedExcelDataQuery.eEmptyExcelCellMode.HasSpecificTypeWithoutAdvice;
        #endregion

        #region public (static) methods

        #region methods concerning the (User) input

        /// <summary>Gets the name of the input tables or (general) properties.
        /// </summary>
        /// <param name="xlObjectName">The name of the (<see cref="ExcelPool"/>) object.</param>
        /// <param name="xlInputType">The type of the input, i.e. a table or (general) properties.</param>
        /// <param name="xlPropertyTableName">If <paramref name="xlInputType"/> reprsents <see cref="eQueryType.PropertyNames"/> this [optional] 
        /// argument represents the name of the table which contains the properties.</param>
        /// <returns>The names of the input tables or (general) properties.</returns>
        [ExcelFunction(Name = "doGetInputNames", Description = "List the names of the (user input) tables or (general) properties with respect to a specific element of the pool", Category = ExcelCategory)]
        public static object GetInputNames(
           [ExcelArgument(Name = "objectName", Description = "The name of the object", AllowReference = true)]
            object xlObjectName,
            [ExcelArgument(Name = "inputType", Description = "Contains the type of the input, i.e. 'tables' or 'properties'", AllowReference = true)]
            object xlInputType,
            [ExcelArgument(Name = "propertyTableName", Description = "[optional] If 'inputType = \"properties\" this argument represents the name of the table which represents the properties", AllowReference = true)]
            object xlPropertyTableName)
        {
            try
            {
                if (ExcelDnaUtil.IsInFunctionWizard() == true)
                {
                    return String.Empty;
                }

                IExcelDataQuery objectNameQuery = ExcelDataQuery.Create(xlObjectName);
                string objectName;
                ExcelPoolItem poolItem;
                if ((objectNameQuery.TryGetValue<string>(out objectName, dataAdvice: ExcelDataAdvice.Create(ExcelPool.GetObjectNames())) != ExcelCellValueState.ProperValue) || (ExcelPool.TryGetItem(objectName, out poolItem) == false))
                {
                    throw new ArgumentException("No object with name '" + objectNameQuery.ToString(0, 0).GetRelevantSubstring() + "' available.");
                }
                IExcelDataQuery inputTypeQuery = ExcelDataQuery.Create(xlInputType);
                eQueryType inputType;
                if (inputTypeQuery.TryGetValue<eQueryType>(out inputType, EnumStringRepresentationUsage.StringAttribute) != ExcelCellValueState.ProperValue)
                {
                    throw new ArgumentException("Need valid input type; 'tables' and 'properties' are allowed.");
                }

                IExcelDataQuery propertyTableQuery = ExcelDataQuery.Create(xlPropertyTableName);
                if (inputType == eQueryType.PropertyNames)
                {
                    string propertyTableName = "General properties";
                    if (propertyTableQuery.IsEmpty == false)
                    {
                        propertyTableName = propertyTableQuery.GetValue<string>(dataAdvice: ExcelDataAdvice.Create(poolItem.GetDataQueryNames()));
                    }
                    GuidedExcelDataQuery propertyDataQuery;
                    if (poolItem.TryGetDataQuery(propertyTableName, out propertyDataQuery) == false)
                    {
                        throw new ArgumentException("No property table with name' " + propertyTableQuery.ToString(0, 0) + "' available.");
                    }
                    object[,] outputRange = new object[propertyDataQuery.GetNonEmptyRowCount(m_EmptyExcelCellMode) + 1, 1];
                    outputRange[0, 0] = "Property names (input)";
                    int k = 1;
                    for (int j = 0; j < propertyDataQuery.RowCount; j++)
                    {
                        if (propertyDataQuery.IsEmptyExcelCell(j, 0, m_EmptyExcelCellMode) == false)
                        {
                            outputRange[k, 0] = propertyDataQuery.GetExcelData(j, 0);
                            k++;
                        }
                    }
                    return ExcelDataConverter.GetExcelRangeOutput(outputRange);
                }
                else
                {
                    if (propertyTableQuery.IsEmpty == false)
                    {
                        throw new ArgumentException("The property table with name' " + propertyTableQuery.ToString(0, 0) + "' is invalid.");
                    }

                    List<object> outputRange = new List<object>();
                    outputRange.Add("Table names");
                    foreach (string tableName in poolItem.GetDataQueryNames())
                    {
                        outputRange.Add(tableName);
                    }
                    return ExcelDataConverter.GetExcelRangeOutput(outputRange);
                }
            }
            catch (Exception e)
            {
                return ExcelDataConverter.GetExcelRangeErrorMessage(e.Message);
            }
        }

        /// <summary>Gets the value (user input) of a specific property.
        /// </summary>
        /// <param name="xlObjectName">The name of the (<see cref="ExcelPool"/>) object.</param>
        /// <param name="xlPropertyName">The name of the property.</param>
        /// <param name="xlPropertyValueColumnIndex">The null-based index of the column which contains the value, '1' is standard.</param>
        /// <param name="xlPropertyTableName">The name of the table which represents the properties ('general properties' is standard).</param>
        /// <returns>The value of a specific property.</returns>
        [ExcelFunction(Name = "doGetInputPropertyValue", Description = "Gets the value of a (User) input property", Category = ExcelCategory)]
        public static object GetInputPropertyValue(
            [ExcelArgument(Name = "objectName", Description = "The name of the object", AllowReference = true)]
            object xlObjectName,
            [ExcelArgument(Name = "propertyName", Description = "The name of the property", AllowReference = true)]
            object xlPropertyName,
            [ExcelArgument(Name = "propertyValueColumnIndex", Description = "[optional] The null-based column index of the column which contains the value of the property, '1' is standard", AllowReference = true)]
            object xlPropertyValueColumnIndex,
            [ExcelArgument(Name = "propertyTableName", Description = "[optional] The name of the table which represents the properties ('general properties' is standard)", AllowReference = true)]
            object xlPropertyTableName = null)
        {
            try
            {
                if (ExcelDnaUtil.IsInFunctionWizard() == true)
                {
                    return String.Empty;
                }

                IExcelDataQuery objectNameQuery = ExcelDataQuery.Create(xlObjectName);
                string objectName;
                ExcelPoolItem poolItem;
                if ((objectNameQuery.TryGetValue<string>(out objectName, dataAdvice: ExcelDataAdvice.Create(ExcelPool.GetObjectNames())) != ExcelCellValueState.ProperValue) || (ExcelPool.TryGetItem(objectName, out poolItem) == false))
                {
                    throw new ArgumentException("No object with name '" + objectNameQuery.ToString(0, 0).GetRelevantSubstring() + "' available.");
                }

                string propertyTableName = "General properties";
                IExcelDataQuery propertyTableQuery = ExcelDataQuery.Create(xlPropertyTableName);
                if (propertyTableQuery.IsEmpty == false)
                {
                    propertyTableName = propertyTableQuery.GetValue<string>(dataAdvice: ExcelDataAdvice.Create(poolItem.GetDataQueryNames()));
                }
                GuidedExcelDataQuery propertyDataQuery;
                if (poolItem.TryGetDataQuery(propertyTableName, out propertyDataQuery) == false)
                {
                    throw new ArgumentException("The property table name' " + propertyTableName + "' is invalid.");
                }

                IExcelDataQuery propertyNameQuery = ExcelDataQuery.Create(xlPropertyName);
                string propertyName;
                if (propertyNameQuery.TryGetValue<string>(out propertyName, dataAdvice: ExcelDataAdvice.Create(propertyNameQuery.GetColumnVector<string>())) != ExcelCellValueState.ProperValue)
                {
                    throw new ArgumentException("The property name '" + propertyNameQuery.ToString(0, 0) + "' is invalid.");
                }

                /* now get the property value, but use a Excel conform output: */
                int propertyRowIndex;
                if (propertyDataQuery.TryGetRowIndexOfPropertyName(propertyName, out propertyRowIndex) == false)
                {
                    throw new ArgumentException("No property with name '" + propertyName + "' found.");
                }
                int propertyValueColumnIndex;
                ExcelCellValueState propertyValueColumnIndexState = ExcelDataQuery.Create(xlPropertyValueColumnIndex).TryGetValue<int>(out propertyValueColumnIndex);

                if (propertyValueColumnIndexState == ExcelCellValueState.EmptyOrMissingExcelCell)
                {
                    propertyValueColumnIndex = 1;
                }
                else if (propertyValueColumnIndexState == ExcelCellValueState.NoValidValue)
                {
                    throw new ArgumentException("Invalid 'Property value column index', a positive integer expected.");
                }

                return propertyDataQuery.GetExcelData(propertyRowIndex, propertyValueColumnIndex);
            }
            catch (Exception e)
            {
                return ExcelDataConverter.GetExcelRangeErrorMessage(e.Message);
            }
        }

        /// <summary>Gets the input range, i.e. a specific <see cref="IExcelDataQuery"/> or a subset.
        /// </summary>
        /// <param name="xlObjectName">The name of the (<see cref="ExcelPool"/>) object.</param>
        /// <param name="xlTableName">The name of the table, i.e. <see cref="IExcelDataQuery"/>.</param>
        /// <param name="xlRowIndex">The [optional] null-based index of the row.</param>
        /// <param name="xlColumnIndex">The [optional] null-based index of the column.</param>
        /// <returns>The specified input range.</returns>
        [ExcelFunction(Name = "doGetInputRange", Description = "Gets a specific user input (sub-) Range", Category = ExcelCategory)]
        public static object GetInputRange(
            [ExcelArgument(Name = "objectName", Description = "The name of the object", AllowReference = true)]
            object xlObjectName,
            [ExcelArgument(Name = "tableName", Description = "The name of the table", AllowReference = true)]
            object xlTableName,
            [ExcelArgument(Name = "rowIndex", Description = "The [optional] null-based row index of the table")]
            object xlRowIndex,
            [ExcelArgument(Name = "columnIndex", Description = "The [optional] null-based column index of the table")]
            object xlColumnIndex)
        {
            try
            {
                if (ExcelDnaUtil.IsInFunctionWizard() == true)
                {
                    return String.Empty;
                }

                IExcelDataQuery objectNameQuery = ExcelDataQuery.Create(xlObjectName);
                string objectName;
                ExcelPoolItem poolItem;
                if ((objectNameQuery.TryGetValue<string>(out objectName, dataAdvice: ExcelDataAdvice.Create(ExcelPool.GetObjectNames())) != ExcelCellValueState.ProperValue) || (ExcelPool.TryGetItem(objectName, out poolItem) == false))
                {
                    throw new ArgumentException("No object with name '" + objectNameQuery.ToString(0, 0).GetRelevantSubstring() + "' available.");
                }

                IExcelDataQuery tableNameDataQuery = ExcelDataQuery.Create(xlTableName);
                GuidedExcelDataQuery tableDataQuery;
                string tableName;
                if ((tableNameDataQuery.TryGetValue<string>(out tableName, dataAdvice: ExcelDataAdvice.Create(poolItem.GetDataQueryNames())) != ExcelCellValueState.ProperValue) || (poolItem.TryGetDataQuery(tableName, out tableDataQuery) == false))
                {
                    throw new ArgumentException("Table name '" + tableNameDataQuery.ToString(0, 0) + "' is invalid.");
                }

                IExcelDataQuery rowDataQuery = ExcelDataQuery.Create(xlRowIndex);
                IExcelDataQuery columnDataQuery = ExcelDataQuery.Create(xlColumnIndex);

                object[,] outputRange = null;
                if (!rowDataQuery.IsEmpty)
                {
                    int rowIndex;
                    if ((rowDataQuery.TryGetValue<int>(out rowIndex) != ExcelCellValueState.ProperValue) || (rowIndex < 0) || (rowIndex > tableDataQuery.RowCount))
                    {
                        throw new ArgumentException("Invalid row index '" + rowDataQuery.ToString(0, 0) + "'.");
                    }
                    if (!columnDataQuery.IsEmpty)
                    {
                        int columnIndex;
                        if ((columnDataQuery.TryGetValue<int>(out columnIndex) != ExcelCellValueState.ProperValue) || (columnIndex < 0) || (columnIndex > tableDataQuery.ColumnCount))
                        {
                            throw new ArgumentException("Invalid column index '" + columnDataQuery.ToString(0, 0) + "'.");
                        }
                        outputRange = new object[1, 1];
                        outputRange[0, 0] = tableDataQuery.GetExcelData(rowIndex, columnIndex);
                    }
                    else
                    {
                        outputRange = new object[1, tableDataQuery.ColumnCount];
                        for (int j = 0; j < tableDataQuery.ColumnCount; j++)
                        {
                            outputRange[0, j] = tableDataQuery.GetExcelData(rowIndex, j);
                        }
                    }
                }
                else
                {
                    if (!columnDataQuery.IsEmpty)
                    {
                        int columnIndex;
                        if ((columnDataQuery.TryGetValue<int>(out columnIndex) != ExcelCellValueState.ProperValue) || (columnIndex < 0) || (columnIndex > tableDataQuery.ColumnCount))
                        {
                            throw new ArgumentException("Invalid column index '" + columnDataQuery.ToString(0, 0) + "'.");
                        }
                        outputRange = new object[tableDataQuery.GetNonEmptyRowCount(m_EmptyExcelCellMode), 1];
                        int k = 0;
                        for (int j = 0; j < tableDataQuery.RowCount; j++)
                        {
                            if (tableDataQuery.IsEmptyExcelCell(j, columnIndex, m_EmptyExcelCellMode) == false)
                            {
                                outputRange[k, 0] = tableDataQuery.GetExcelData(j, columnIndex);
                                k++;
                            }
                        }
                    }
                    else
                    {
                        outputRange = new object[tableDataQuery.GetNonEmptyRowCount(m_EmptyExcelCellMode), tableDataQuery.ColumnCount];
                        int rowIndex = 0;
                        for (int j = 0; j < tableDataQuery.RowCount; j++)
                        {
                            if (tableDataQuery.IsEmptyRow(j, m_EmptyExcelCellMode) == false)
                            {
                                for (int k = 0; k < tableDataQuery.ColumnCount; k++)
                                {
                                    outputRange[rowIndex, k] = tableDataQuery.GetExcelData(j, k);
                                }
                                rowIndex++;
                            }
                        }
                    }
                }
                return ExcelDataConverter.GetExcelRangeOutput(outputRange);
            }
            catch (Exception e)
            {
                return ExcelDataConverter.GetExcelRangeErrorMessage(e.Message);
            }
        }
        #endregion

        #region methods concerning the output

        /// <summary>Gets the name of the output tables or properties.
        /// </summary>
        /// <param name="xlObjectName">The name of the (<see cref="ExcelPool"/>) object.</param>
        /// <param name="xlOutputType">The type of the output, i.e. a table or (general) properties.</param>
        /// <param name="xlPropertyTableName">If <paramref name="xlOutputType"/> reprsents <see cref="eQueryType.PropertyNames"/> this [optional] 
        /// argument represents the name of the table which contains the properties.</param>
        /// <param name="xlCategoryName">The name of the category.</param>
        /// <returns>The list of names of the output tables or properties.</returns>
        [ExcelFunction(Name = "doGetOutputNames", Description = "List the names of the output tables or (general) properties with respect to a specific element of the pool", Category = ExcelCategory)]
        public static object GetOutputNames(
           [ExcelArgument(Name = "objectName", Description = "The name of the object", AllowReference = true)]
            object xlObjectName,
            [ExcelArgument(Name = "outputType", Description = "Contains the type of the output, i.e. 'tables' or 'properties'", AllowReference = true)]
            object xlOutputType,
            [ExcelArgument(Name = "propertyGroupName", Description = "[optional] If 'inputType = \"properties\" this argument represents the name of the table which represents the properties", AllowReference = true)]
            object xlPropertyTableName,
            [ExcelArgument(Name = "category", Description = "[optional] The category of the output, i.e. 'general' etc.", AllowReference = true)]
            object xlCategoryName)
        {
            try
            {
                if (ExcelDnaUtil.IsInFunctionWizard() == true)
                {
                    return String.Empty;
                }

                IExcelDataQuery objectNameQuery = ExcelDataQuery.Create(xlObjectName);
                string objectName;
                ExcelPoolItem poolItem;
                if ((objectNameQuery.TryGetValue<string>(out objectName, dataAdvice: ExcelDataAdvice.Create(ExcelPool.GetObjectNames())) != ExcelCellValueState.ProperValue) || (ExcelPool.TryGetItem(objectName, out poolItem) == false))
                {
                    throw new ArgumentException("No object with name '" + objectNameQuery.ToString(0, 0).GetRelevantSubstring() + "' available.");
                }
                IExcelDataQuery outputTypeQuery = ExcelDataQuery.Create(xlOutputType);
                eQueryType outputType;
                if (outputTypeQuery.TryGetValue<eQueryType>(out outputType, EnumStringRepresentationUsage.StringAttribute) != ExcelCellValueState.ProperValue)
                {
                    throw new ArgumentException("Invalid output type '" + outputTypeQuery.ToString(0, 0) + "'.");
                }

                /* get the output of the pool object: */
                InfoOutput itemOutput = new InfoOutput();
                poolItem.Value.FillInfoOutput(itemOutput);

                InfoOutputPackage itemOutputCollection = null;
                IExcelDataQuery categoryNameQuery = ExcelDataQuery.Create(xlCategoryName);
                if (categoryNameQuery.IsEmpty == true)
                {
                    itemOutputCollection = itemOutput.GetGeneralPackage();
                }
                else
                {
                    string categoryName = categoryNameQuery.GetValue<string>(0, 0, ExcelDataAdvice.Create(itemOutput.CategoryNames));
                    itemOutputCollection = itemOutput.GetPackage(categoryName);
                }

                IExcelDataQuery propertyTableQuery = ExcelDataQuery.Create(xlPropertyTableName);
                if (outputType == eQueryType.PropertyNames)
                {
                    string propertyTableName = InfoOutputPackage.GeneralPropertyGroupName.String;
                    if (propertyTableQuery.IsEmpty == false)
                    {
                        propertyTableName = propertyTableQuery.GetValue<string>(0, 0, ExcelDataAdvice.Create(itemOutputCollection.PropertyGroupNames));
                    }

                    IIdentifierStringDictionary<InfoOutputProperty> propertyCollection;
                    if (itemOutputCollection.TryGetProperties(propertyTableName, out  propertyCollection) == false)
                    {
                        throw new ArgumentException("The property table with name' " + propertyTableName + "' is invalid.");
                    }

                    object[,] outputRange = new object[propertyCollection.Count() + 1, 1];
                    outputRange[0, 0] = "Property names (output)";
                    int j = 1;
                    foreach (var property in propertyCollection)
                    {
                        outputRange[j, 0] = (string)property.Name;
                        j++;
                    }
                    return ExcelDataConverter.GetExcelRangeOutput(outputRange);
                }
                else
                {
                    if (propertyTableQuery.IsEmpty == false)
                    {
                        throw new ArgumentException("The property table with name' " + propertyTableQuery.ToString(0, 0) + "' is invalid.");
                    }

                    List<object> outputRange = new List<object>();
                    outputRange.Add("Table names");
                    foreach (var table in itemOutputCollection.GetDataTables(InfoOutputPackage.DataTableType.Single | InfoOutputPackage.DataTableType.Parent))
                    {
                        outputRange.Add(table.Item1.String);
                    }
                    return ExcelDataConverter.GetExcelRangeOutput(outputRange);
                }
            }
            catch (Exception e)
            {
                return ExcelDataConverter.GetExcelRangeErrorMessage(e.Message);
            }
        }

        /// <summary>Gets the value of a specific (output) property.
        /// </summary>
        /// <param name="xlObjectName">The name of the (<see cref="ExcelPool"/>) object.</param>
        /// <param name="xlPropertyName">The name of the property.</param>
        /// <param name="xlPropertyGroupName">The name of the property group, for example 'general properties' etc.</param>
        /// <param name="xlCategoryName">The [optional] name of the category, as for example 'general' etc.</param>
        /// <returns>The value of the specified (output) property.</returns>
        [ExcelFunction(Name = "doGetOutputPropertyValue", Description = "Gets the value of a output property", Category = ExcelCategory)]
        public static object GetOutputPropertyValue(
            [ExcelArgument(Name = "objectName", Description = "The name of the object", AllowReference = true)]
            object xlObjectName,
            [ExcelArgument(Name = "propertyName", Description = "The name of the property", AllowReference = true)]
            object xlPropertyName,
            [ExcelArgument(Name = "propertyGroupName", Description = "[optional] The name of the property group ('general properties' is standard)", AllowReference = true)]
            object xlPropertyGroupName,
            [ExcelArgument(Name = "category", Description = "The category of the output, i.e. 'general' etc.", AllowReference = true)]
            object xlCategoryName)
        {
            try
            {
                if (ExcelDnaUtil.IsInFunctionWizard() == true)
                {
                    return String.Empty;
                }

                IExcelDataQuery objectNameQuery = ExcelDataQuery.Create(xlObjectName);
                string objectName;
                ExcelPoolItem poolItem;
                if ((objectNameQuery.TryGetValue<string>(out objectName, dataAdvice: ExcelDataAdvice.Create(ExcelPool.GetObjectNames())) != ExcelCellValueState.ProperValue) || (ExcelPool.TryGetItem(objectName, out poolItem) == false))
                {
                    throw new ArgumentException("No object with name '" + objectNameQuery.ToString(0, 0).GetRelevantSubstring() + "' available.");
                }
                /* get the output of the pool object: */
                InfoOutput itemOutput = new InfoOutput();
                poolItem.Value.FillInfoOutput(itemOutput);

                InfoOutputPackage itemOutputCollection = null;
                IExcelDataQuery categoryNameQuery = ExcelDataQuery.Create(xlCategoryName);
                if (categoryNameQuery.IsEmpty == true)
                {
                    itemOutputCollection = itemOutput.GetGeneralPackage();
                }
                else
                {
                    string categoryName = categoryNameQuery.GetValue<string>(dataAdvice: ExcelDataAdvice.Create(itemOutput.CategoryNames));
                    itemOutputCollection = itemOutput.GetPackage(categoryName);
                }

                string propertyGroupName = InfoOutputPackage.GeneralPropertyGroupName.String;
                IExcelDataQuery propertyGroupNameQuery = ExcelDataQuery.Create(xlPropertyGroupName);
                if (propertyGroupNameQuery.IsEmpty == false)
                {
                    propertyGroupName = propertyGroupNameQuery.GetValue<string>(dataAdvice: ExcelDataAdvice.Create(itemOutputCollection.PropertyGroupNames));
                }

                IIdentifierStringDictionary<InfoOutputProperty> properties;
                if (itemOutputCollection.TryGetProperties(propertyGroupName, out properties) == false)
                {
                    throw new ArgumentException("The property group name' " + propertyGroupName + "' is invalid.");
                }

                IExcelDataQuery propertyNameQuery = ExcelDataQuery.Create(xlPropertyName);
                string propertyName;
                if (propertyNameQuery.TryGetValue<string>(out propertyName, dataAdvice: ExcelDataAdvice.Create(properties)) != ExcelCellValueState.ProperValue)
                {
                    throw new ArgumentException("The property name '" + propertyNameQuery.ToString(0, 0) + "' is invalid.");
                }

                InfoOutputProperty property;
                if (properties.TryGetValue(propertyName, out property) == false)
                {
                    throw new ArgumentException("No property with name '" + propertyName + "' found.");
                }
                return ExcelDataConverter.GetExcelCellRepresentation(property.Value);
            }
            catch (Exception e)
            {
                return ExcelDataConverter.GetExcelRangeErrorMessage(e.Message);
            }
        }

        /// <summary>Gets the output range, or a subset.
        /// </summary>
        /// <param name="xlObjectName">The name of the (<see cref="ExcelPool"/>) object.</param>
        /// <param name="xlTableName">The name of the table, i.e. <see cref="IExcelDataQuery"/>.</param>
        /// <param name="xlRowIndices">An [optional] Excel Range vector that contains null-based indices (excluding header) of the row.</param>
        /// <param name="xlColumnIndicesOrNames">An [optional] Excel Range vector that contains column names or null-based column indices of the output.</param>
        /// <param name="xlCategoryName">The [optional] category name, for example 'general' etc.</param>
        /// <returns>The specified output range.</returns>
        /// <remarks>The header will be shown if the output contains more than two columns.</remarks>
        [ExcelFunction(Name = "doGetOutputRange", Description = "Gets a specific output (sub-) Range. The header will be shown if the output contains more than two columns", Category = ExcelCategory)]
        public static object GetOutputRange(
            [ExcelArgument(Name = "objectName", Description = "The name of the object", AllowReference = true)]
            object xlObjectName,
            [ExcelArgument(Name = "tableName", Description = "The name of the table", AllowReference = true)]
            object xlTableName,
            [ExcelArgument(Name = "rowIndices", Description = "An [optional] Excel Range vector that contains null-based (excluding header) row indices of the table", AllowReference = true)]
            object xlRowIndices,
            [ExcelArgument(Name = "columnIndicesOrNames", Description = "An [optional] Excel Range vector which contains column names or null-based column indices of the table", AllowReference = true)]
            object xlColumnIndicesOrNames,
            [ExcelArgument(Name = "category", Description = "The category of the output, i.e. 'general' etc.", AllowReference = true)]
            object xlCategoryName)
        {
            try
            {
                if (ExcelDnaUtil.IsInFunctionWizard() == true)
                {
                    return String.Empty;
                }

                string objectName;
                ExcelPoolItem poolItem;
                IExcelDataQuery objectNameQuery = ExcelDataQuery.Create(xlObjectName);

                if ((objectNameQuery.TryGetValue<string>(out objectName, dataAdvice: ExcelDataAdvice.Create(ExcelPool.GetObjectNames())) != ExcelCellValueState.ProperValue) || (ExcelPool.TryGetItem(objectName, out poolItem) == false))
                {
                    throw new ArgumentException("No object with name '" + objectNameQuery.ToString(0, 0).GetRelevantSubstring() + "' available.");
                }

                InfoOutput infoOutput = new InfoOutput();
                poolItem.Value.FillInfoOutput(infoOutput);

                InfoOutputPackage itemOutputCollection = null;
                IExcelDataQuery categoryNameQuery = ExcelDataQuery.Create(xlCategoryName);
                if (categoryNameQuery.IsEmpty == true)
                {
                    itemOutputCollection = infoOutput.GetGeneralPackage();
                }
                else
                {
                    string categoryName = categoryNameQuery.GetValue<string>(dataAdvice: ExcelDataAdvice.Create(infoOutput.CategoryNames));
                    itemOutputCollection = infoOutput.GetPackage(categoryName);
                }

                IExcelDataQuery tableNameDataQuery = ExcelDataQuery.Create(xlTableName);

                DataTable tableOutput;
                string tableName;
                if ((tableNameDataQuery.TryGetValue<string>(out tableName, dataAdvice: ExcelDataAdvice.Create(itemOutputCollection.GetDataTableNames())) != ExcelCellValueState.ProperValue) || (itemOutputCollection.TryGetDataTable(tableName, out tableOutput) == false))
                {
                    throw new ArgumentException("Table with name '" + tableNameDataQuery.ToString(0, 0) + "' is invalid.");
                }

                int rowCount, columnCount;
                IEnumerable<int> rowIndices = GetRowIndices(ExcelDataQuery.Create(xlRowIndices), tableOutput, out rowCount);
                IEnumerable<int> columnIndices = GetColumnIndices(ExcelDataQuery.Create(xlColumnIndicesOrNames), tableOutput, out columnCount);

                return ExcelDataConverter.GetExcelRangeOutput(GetSubTable(rowIndices, rowCount, columnIndices, columnCount, tableOutput));
            }
            catch (Exception e)
            {
                return ExcelDataConverter.GetExcelRangeErrorMessage(e.Message);
            }
        }
        #endregion

        #endregion

        #region private static methods

        /// <summary>Gets a subset of a specified <see cref="DataTable"/> object in a two-dimensional <see cref="System.Object"/> array representation.
        /// </summary>
        /// <param name="rowIndices">The set of null-based (excluding header) row indices.</param>
        /// <param name="rowCount">The number of elements in <paramref name="rowIndices"/>.</param>
        /// <param name="columnIndices">The set of null-based column indices.</param>
        /// <param name="columnCount">The number of elements in <paramref name="columnIndices"/>.</param>
        /// <param name="dataTable">The data table.</param>
        /// <returns>A two-dimensional <see cref="System.Object"/> array representation of the desired subset of <paramref name="dataTable"/>; it contains
        /// the header if it contains more than one column.</returns>
        private static object[,] GetSubTable(IEnumerable<int> rowIndices, int rowCount, IEnumerable<int> columnIndices, int columnCount, DataTable dataTable)
        {
            if (columnCount == 1)  // without header
            {
                object[,] outputRange = new object[rowCount, 1];
                int columnIndex = columnIndices.First();

                int k = 0;
                foreach (int rowIndex in rowIndices)
                {
                    outputRange[k, 0] = ExcelDataConverter.GetExcelCellRepresentation(dataTable.Rows[rowIndex][columnIndex]);
                    k++;
                }
                return outputRange;
            }
            else if (columnCount > 1)  // with header
            {
                object[,] outputRange = new object[rowCount + 1, columnCount];

                int k = 0;
                foreach (int columnIndex in columnIndices)
                {
                    outputRange[0, k] = dataTable.Columns[columnIndex].ColumnName;
                    int j = 1;
                    foreach (int rowIndex in rowIndices)
                    {
                        outputRange[j, k] = ExcelDataConverter.GetExcelCellRepresentation(dataTable.Rows[rowIndex][columnIndex]);
                        j++;
                    }
                    k++;
                }
                return outputRange;
            }
            throw new ArgumentException("Empty data table.", "dataTable");
        }

        /// <summary>Gets the set of null-based column indices of a specified <see cref="DataTable"/> object.
        /// </summary>
        /// <param name="columnDataQuery">The column data query, which contains the null-based column indices or column names.</param>
        /// <param name="dataTable">The data table.</param>
        /// <param name="columnCount">The number of column indices, i.e. of the return value (output).</param>
        /// <returns>The set of null-based column indices; <c>null</c> if the set of column indices is empty.</returns>
        private static IEnumerable<int> GetColumnIndices(IExcelDataQuery columnDataQuery, DataTable dataTable, out int columnCount)
        {
            if (columnDataQuery.IsEmpty == true)
            {
                columnCount = dataTable.Columns.Count;
                return GetNullbasedIndices(dataTable.Columns.Count);
            }
            if (columnDataQuery.ColumnCount > 1)
            {
                throw new ArgumentException("The Excel Range that contains the column names/indices must be a single row or column.");
            }

            HashSet<int> columnIndices = new HashSet<int>();
            for (int j = 0; j < columnDataQuery.RowCount; j++)
            {
                int columnIndex;

                if ((columnDataQuery.TryGetValue<int>(out columnIndex, j, dataAdvice: ExcelDataAdvice.Create(dataTable.Columns.Count)) == ExcelCellValueState.ProperValue) && (columnIndex < 0) && (columnIndex < dataTable.Columns.Count))
                {
                    columnIndices.Add(columnIndex);
                }
                else  // perhaps a column name is given by the user:
                {
                    string columnName;

                    switch (columnDataQuery.TryGetValue<string>(out columnName, j, dataAdvice: ExcelDataAdvice.Create(GetColumnNames(dataTable))))
                    {
                        case ExcelCellValueState.ProperValue:
                            string idColumnName = columnName.ToIDString();

                            bool foundColumn = false;
                            int k = 0;
                            while ((k < dataTable.Columns.Count) && (foundColumn == false))
                            {
                                if ((dataTable.Columns[k].ColumnName != null) && (dataTable.Columns[k].ColumnName.ToIDString() == idColumnName))
                                {
                                    columnIndices.Add(k);
                                    foundColumn = true;
                                }
                                k++;
                            }
                            if (foundColumn == false)
                            {
                                throw new ArgumentException("Invalid column name '" + columnDataQuery.ToString(j, 0) + "'.");
                            }
                            break;

                        case ExcelCellValueState.NoValidValue:
                            throw new ArgumentException("Invalid column index/name '" + columnDataQuery.ToString(j, 0) + "'.");

                        case ExcelCellValueState.EmptyOrMissingExcelCell:
                            break;

                        // default:  // todo: Funktioniert nicht - warum?
                        //    new NotImplementedException();
                    }
                }
            }
            if (columnIndices.Count == 0)
            {
                columnCount = dataTable.Columns.Count;
                return GetNullbasedIndices(dataTable.Columns.Count);
            }
            columnCount = columnIndices.Count;
            return columnIndices;
        }

        /// <summary>Gets the set of null-based (excluding header) row indices of a specified <see cref="DataTable"/> object.
        /// </summary>
        /// <param name="rowDataQuery">The row data query, which contains the null-based (excluding header) row indices.</param>
        /// <param name="dataTable">The data table.</param>
        /// <param name="rowCount">The number of row indices, i.e. of the return value (output).</param>
        /// <returns>The set of null-based row indices; <c>null</c> if the set of row indices is empty.</returns>
        private static IEnumerable<int> GetRowIndices(IExcelDataQuery rowDataQuery, DataTable dataTable, out int rowCount)
        {
            if (rowDataQuery.IsEmpty == true)
            {
                rowCount = dataTable.Rows.Count;
                return GetNullbasedIndices(rowCount);
            }
            if (rowDataQuery.ColumnCount > 1)
            {
                throw new ArgumentException("The Excel Range that contains the row indices must be a single row or column.");
            }
            SortedSet<int> rowIndices = new SortedSet<int>();
            for (int j = 0; j < rowDataQuery.RowCount; j++)
            {
                int rowIndex;
                switch (rowDataQuery.TryGetValue<int>(out rowIndex, j))
                {
                    case ExcelCellValueState.ProperValue:
                        if ((rowIndex < 0) || (rowIndex > dataTable.Rows.Count))
                        {
                            throw new ArgumentException("Invalid row index '" + rowDataQuery.ToString(j, 0) + "'.");
                        }
                        rowIndices.Add(rowIndex);
                        break;

                    case ExcelCellValueState.NoValidValue:
                        throw new ArgumentException("Invalid row index '" + rowDataQuery.ToString(j, 0) + "'.");

                    case ExcelCellValueState.EmptyOrMissingExcelCell:
                        break;  // nothing to do here

                    default:
                        throw new NotImplementedException();
                }
            }
            if (rowIndices.Count == 0)
            {
                rowCount = dataTable.Rows.Count;
                return GetNullbasedIndices(rowCount);
            }
            rowCount = rowIndices.Count;
            return rowIndices;
        }

        /// <summary>Gets the column names of a specific <see cref="DataTable"/> object.
        /// </summary>
        /// <param name="dataTable">The data table.</param>
        /// <returns>The collection of column names.</returns>
        /// <remarks>This is a helper class. <see cref="DataColumnCollection"/> does not implement a generic IEnumerable interface.</remarks>
        private static IEnumerable<string> GetColumnNames(DataTable dataTable)
        {
            for (int j = 0; j < dataTable.Columns.Count; j++)
            {
                yield return dataTable.Columns[j].ColumnName;
            }
        }

        /// <summary>Gets null-based indices in increasing order, i.e. 0,1,2,3,4,...
        /// </summary>
        /// <param name="count">The number of elements.</param>
        /// <returns>A collection of integer starting at 0, up to <paramref name="count"/>-1.</returns>
        private static IEnumerable<int> GetNullbasedIndices(int count)
        {
            for (int j = 0; j < count; j++)
            {
                yield return j;
            }
        }
        #endregion
    }
}