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
using System.Data;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Dodoni.BasicComponents.Containers
{
    /// <summary>Represents a collection of <see cref="InfoOutputProperty"/> and <see cref="System.Data.DataTable"/> objects.
    /// </summary>    
    public class InfoOutputPackage : IIdentifierNameable
    {
        #region nested enumerations

        /// <summary>The type of a specific <see cref="DataTable"/> object that represents output of a specific <see cref="IInfoOutputQueriable"/> object.
        /// </summary>
        [Flags]
        public enum DataTableType
        {
            /// <summary>'None', i.e. ignoring all <see cref="DataTable"/> objects. 
            /// </summary>
            None = 0,

            /// <summary>A 'single' <see cref="DataTable"/> object, i.e. neither a parent nor a child table.
            /// </summary>
            Single = 1,

            /// <summary>A 'parent' <see cref="DataTable"/> object, i.e. linked to a specific child table.
            /// </summary>
            Parent = 2,

            /// <summary>A 'child' <see cref="DataTable"/> object, i.e. linked to a secific parent table.
            /// </summary>
            Child = 4,

            /// <summary>An arbitrary <see cref="DataTable"/> object.
            /// </summary>
            Arbitrary = Single | Parent | Child
        }
        #endregion

        #region public (readonly) members

        /// <summary>The (group) name of the collection of general properites.
        /// </summary>
        public static IdentifierString GeneralPropertyGroupName = new IdentifierString("General properties");

        /// <summary>The separator used to separate the parent and child <see cref="DataTable"/> objects, for example "parentTableName->childTableName".
        /// </summary>
        public const string ParentChildTableNameSeparator = "->";
        #endregion

        #region private members

        /// <summary>The name of the <see cref="InfoOutputPackage"/>.
        /// </summary>
        private readonly IdentifierString m_Name;

        /// <summary>A collection of <see cref="DataTable"/> objects, where the key is the name of the table.
        /// </summary>
        /// <remarks>The table name of an <see cref="DataTable"/> object can be changed.</remarks>
        private IdentifierStringDictionary<DataTable> m_Tables = new IdentifierStringDictionary<DataTable>(isReadOnlyExceptAdding: false);

        /// <summary>The collection of <see cref="InfoOutputParentChildDataTable"/> objects, where the key is the name of the parent table.
        /// </summary>
        private IdentifierStringDictionary<InfoOutputParentChildDataTable> m_ParentChildTables = new IdentifierStringDictionary<InfoOutputParentChildDataTable>(isReadOnlyExceptAdding: false);

        /// <summary>A collection of <see cref="InfoOutputProperty"/> objects, where the first key is the property group name ('general properties' etc.) and the second key is the 
        /// name of the property itself. 
        /// </summary>
        private IdentifierStringDictionary<IdentifierStringDictionary<InfoOutputProperty>> m_Properties = new IdentifierStringDictionary<IdentifierStringDictionary<InfoOutputProperty>>(isReadOnlyExceptAdding: false);

        /// <summary>A collection of 'general properties', i.e. the elements of <see cref="m_Properties"/>, where the key is equal to <see cref="GeneralPropertyGroupName"/>,
        /// </summary>
        private IdentifierStringDictionary<InfoOutputProperty> m_GeneralProperties = new IdentifierStringDictionary<InfoOutputProperty>(isReadOnlyExceptAdding: false);
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="InfoOutputPackage"/> class.
        /// </summary>
        /// <param name="categoryName">The name of the category, i.e. the name of the current instance.</param>        
        internal InfoOutputPackage(IdentifierString categoryName)
        {
            m_Name = categoryName;
            m_Properties.Add(GeneralPropertyGroupName, m_GeneralProperties);
        }

        /// <summary>Initializes a new instance of the <see cref="InfoOutputPackage"/> class.
        /// </summary>
        /// <param name="categoryName">The name of the category, i.e. the name of the current instance.</param>
        internal InfoOutputPackage(string categoryName)
            : this(new IdentifierString(categoryName))
        {
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the current instance.
        /// </summary>
        /// <value>The language independent name of the current instance.</value>
        IdentifierString IIdentifierNameable.Name
        {
            get { return m_Name; }
        }

        /// <summary>Gets the long name of the current instance.
        /// </summary>
        /// <value>The language dependent long name of the current instance.</value>
        IdentifierString IIdentifierNameable.LongName
        {
            get { return m_Name; }
        }
        #endregion

        /// <summary>Gets the name of the category.
        /// </summary>
        /// <value>The name of the category.</value>
        public IdentifierString CategoryName
        {
            get { return m_Name; }
        }

        /// <summary>Gets the collection of general properties, i.e. name/value pairs, where the key represents the name of the property.
        /// </summary>
        /// <value>The general properties.</value>
        public IIdentifierStringDictionary<InfoOutputProperty> GeneralProperties
        {
            get { return m_GeneralProperties; }
        }

        /// <summary>Gets the name of the property groups, i.e. 'general properties' etc.
        /// </summary>
        /// <value>The property group names.</value>
        public IEnumerable<string> PropertyGroupNames
        {
            get { return m_Properties.Names; }
        }

        /// <summary>Gets the collection of properties, where the first component is the property group name (i.e. 'General Properties' etc.)  and the second component is the collection of 
        /// the corresponding properties.
        /// </summary>
        /// <value>The collection of properties.</value>
        public IEnumerable<Tuple<IdentifierString, IIdentifierStringDictionary<InfoOutputProperty>>> Properties
        {
            get
            {
                foreach (var property in m_Properties.NamedValues)
                {
                    yield return Tuple.Create(property.Item1, (IIdentifierStringDictionary<InfoOutputProperty>)property.Item2);
                }
            }
        }

        /// <summary>Gets the collection of <see cref="InfoOutputParentChildDataTable"/> objects (as well as the unique identifier) that
        /// represents parent/child <see cref="DataTable"/> objects with a 1:n relation.
        /// </summary>
        /// <value>The parent/child data tables as well a unique identifier in its <see cref="IdentifierString"/> representation.</value>
        public IEnumerable<Tuple<IdentifierString, InfoOutputParentChildDataTable>> ParentChildDataTables
        {
            get { return m_ParentChildTables.NamedValues; }
        }
        #endregion

        #region public methods

        /// <summary>Gets the <see cref="System.Data.DataTable"/> objects with homogeneous informations, and the table name in its <see cref="IdentifierString"/> representation.
        /// </summary>
        /// <param name="dataTableType">The type of the tables to take into account.</param>
        /// <returns>The tables and table names.</returns>
        /// <remarks>The table name of an <see cref="DataTable"/> object can be changed.</remarks>
        public IEnumerable<Tuple<IdentifierString, DataTable>> GetDataTables(DataTableType dataTableType = DataTableType.Single)
        {
            if (dataTableType.HasFlag(DataTableType.Single) == true)
            {
                foreach (var namedTable in m_Tables.NamedValues)
                {
                    yield return namedTable;
                }
            }
            if (dataTableType.HasFlag(DataTableType.Parent) == true)
            {
                foreach (var namedParentChildTables in m_ParentChildTables.NamedValues)
                {
                    yield return Tuple.Create(namedParentChildTables.Item1, namedParentChildTables.Item2.ParentDataTable);
                }
            }
            if (dataTableType.HasFlag(DataTableType.Child) == true)
            {
                foreach (var namedParentChildTables in m_ParentChildTables.NamedValues)
                {
                    yield return Tuple.Create(IdentifierString.Create(namedParentChildTables.Item2.ParentDataTable.TableName + ParentChildTableNameSeparator + namedParentChildTables.Item2.ChildDataTable.TableName), namedParentChildTables.Item2.ChildDataTable);
                }
            }
        }

        /// <summary>Gets the table names.
        /// </summary>
        /// <param name="dataTableType">The type of the tables to take into account.</param>
        /// <returns>The table names.</returns>
        public IEnumerable<string> GetDataTableNames(DataTableType dataTableType = DataTableType.Single)
        {
            if (dataTableType.HasFlag(DataTableType.Single) == true)
            {
                foreach (var tableName in m_Tables.Names)
                {
                    yield return tableName;
                }
            }
            if (dataTableType.HasFlag(DataTableType.Parent) == true)
            {
                foreach (var namedParentChildTables in m_ParentChildTables.NamedValues)
                {
                    yield return (String)namedParentChildTables.Item1;
                }
            }
            if (dataTableType.HasFlag(DataTableType.Child) == true)
            {
                foreach (var namedParentChildTables in m_ParentChildTables.NamedValues)
                {
                    yield return namedParentChildTables.Item2.ParentDataTable.TableName + ParentChildTableNameSeparator + namedParentChildTables.Item2.ChildDataTable.TableName;
                }
            }
        }

        /// <summary>Gets a specific <see cref="DataTable"/> object.
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        /// <param name="value">The <see cref="DataTable"/>  (output).</param>
        /// <param name="dataTableType">The type of the tables to take into account.</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public bool TryGetDataTable(string tableName, out DataTable value, DataTableType dataTableType = DataTableType.Single)
        {
            return TryGetDataTable(IdentifierString.Create(tableName), out value, dataTableType);
        }

        /// <summary>Gets a specific <see cref="DataTable"/> object.
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        /// <param name="dataTableType">The type of the tables to take into account.</param>
        /// <returns>A <see cref="DataTable"/> with the desired name.</returns>
        /// <exception cref="ArgumentException">Thrown, if no table available with the desired name.</exception>
        public DataTable GetDataTable(string tableName, DataTableType dataTableType = DataTableType.Single)
        {
            if (TryGetDataTable(IdentifierString.Create(tableName), out DataTable value, dataTableType) == true)
            {
                return value;
            }
            throw new ArgumentException(String.Format(ExceptionMessages.TableNameUnknown, tableName), tableName);
        }

        /// <summary>Gets a specific <see cref="DataTable"/> object.
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        /// <param name="value">The <see cref="DataTable"/> and its name in its <see cref="IdentifierString"/> representation (output).</param>
        /// <param name="dataTableType">The type of the tables to take into account.</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public bool TryGetDataTable(IdentifierString tableName, out DataTable value, DataTableType dataTableType = DataTableType.Single)
        {
            if (dataTableType.HasFlag(DataTableType.Single) == true)
            {
                if (m_Tables.TryGetValue(tableName, out value) == true)
                {
                    return true;
                }
            }
            if (dataTableType.HasFlag(DataTableType.Parent) == true)
            {
                if (m_ParentChildTables.TryGetValue(tableName, out InfoOutputParentChildDataTable parentChildDataTable) == true)
                {
                    value = parentChildDataTable.ParentDataTable;
                    return true;
                }
            }
            if (dataTableType.HasFlag(DataTableType.Child) == true)
            {
                int index = tableName.IDString.IndexOf(ParentChildTableNameSeparator);
                if (index >= 0)
                {
                    string parentTableName = tableName.IDString.Substring(0, index);
                    string childTableName = tableName.IDString.Substring(index + ParentChildTableNameSeparator.Length, tableName.IDString.Length - index - ParentChildTableNameSeparator.Length);

                    if (m_ParentChildTables.TryGetValue(parentTableName, out InfoOutputParentChildDataTable parentChildDataTable) == true)
                    {
                        if (parentChildDataTable.ChildDataTable.TableName.ToIDString() == childTableName.ToIDString())
                        {
                            value = parentChildDataTable.ChildDataTable;
                            return true;
                        }
                    }
                }
            }
            value = null;
            return false;
        }

        /// <summary>Gets a specific <see cref="DataTable"/> object.
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        /// <param name="dataTableType">The type of the tables to take into account.</param>
        /// <returns>A <see cref="DataTable"/> with the desired name.</returns>
        /// <exception cref="ArgumentException">Thrown, if no table available with the desired name.</exception>
        public DataTable GetDataTable(IdentifierString tableName, DataTableType dataTableType = DataTableType.Single)
        {
            if (TryGetDataTable(tableName, out DataTable value, dataTableType) == true)
            {
                return value;
            }
            throw new ArgumentException(String.Format(ExceptionMessages.TableNameUnknown, (String)tableName), (String)tableName);
        }

        /// <summary>Gets a specific <see cref="InfoOutputParentChildDataTable"/> object, i.e. one parent and one child <see cref="DataTable"/> object, 
        /// connected with respect to a specified relation.
        /// </summary>
        /// <param name="parentDataTableName">The name of the parent data table.</param>
        /// <param name="value">The value.</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public bool TryGetParentChildDataTable(string parentDataTableName, out InfoOutputParentChildDataTable value)
        {
            return m_ParentChildTables.TryGetValue(parentDataTableName, out value);
        }

        /// <summary>Gets a specific <see cref="InfoOutputParentChildDataTable"/> object, i.e. one parent and one child <see cref="DataTable"/> object, 
        /// connected with respect to a specified relation.
        /// </summary>
        /// <param name="parentDataTableName">The name of the parent data table.</param>
        /// <returns>The requested <see cref="InfoOutputParentChildDataTable"/> object.</returns>
        /// <exception cref="ArgumentException">Thrown, if no data available with the desired name.</exception>
        public InfoOutputParentChildDataTable GetParentChildDataTable(string parentDataTableName)
        {
            if (m_ParentChildTables.TryGetValue(parentDataTableName, out InfoOutputParentChildDataTable value) == true)
            {
                return value;
            }
            throw new ArgumentException(String.Format(ExceptionMessages.TableNameUnknown, parentDataTableName), parentDataTableName);
        }

        /// <summary>Gets a specific <see cref="InfoOutputProperty"/> object with repsect to a specific property group name.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="value">The property (output).</param>
        /// <param name="propertyGroupName">The name of the property group (i.e. 'General Properties' etc.).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public bool TryGetProperty(string propertyName, out InfoOutputProperty value, string propertyGroupName = "General properties")
        {
            if (propertyGroupName != null)
            {
                if (m_Properties.TryGetValue(propertyGroupName, out IdentifierStringDictionaryBase<InfoOutputProperty> propertyCollection) == true)
                {
                    return propertyCollection.TryGetValue(propertyName, out value);
                }
            }
            value = default;
            return false;
        }

        /// <summary>Gets a specific <see cref="InfoOutputProperty"/> object with repsect to a specific property group name.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="propertyGroupName">The name of the property group (i.e. 'General Properties' etc.).</param>
        /// <returns>The property.</returns>
        /// <exception cref="ArgumentException">Thrown, if no property available with the desired name.</exception>
        public InfoOutputProperty GetProperty(string propertyName, string propertyGroupName = "General properties")
        {
            if (TryGetProperty(propertyName, out InfoOutputProperty value, propertyGroupName) == true)
            {
                return value;
            }
            throw new ArgumentException(String.Format(ExceptionMessages.PropertyNameUnknown, propertyGroupName + "." + propertyName), propertyName);
        }

        /// <summary>Gets a specific <see cref="InfoOutputProperty"/> object with repsect to a specific property group name.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="value">The property (output).</param>
        /// <param name="propertyGroupName">The name of the property group (i.e. 'General Properties' etc.).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public bool TryGetProperty(string propertyName, out InfoOutputProperty value, IdentifierString propertyGroupName)
        {
            if (propertyGroupName != null)
            {
                if (m_Properties.TryGetValue(propertyGroupName, out IdentifierStringDictionaryBase<InfoOutputProperty> propertyCollection) == true)
                {
                    return propertyCollection.TryGetValue(propertyName, out value);
                }
            }
            value = default;
            return false;
        }

        /// <summary>Gets a specific <see cref="InfoOutputProperty"/> object with repsect to a specific property group name.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="propertyGroupName">The name of the property group (i.e. 'General Properties' etc.).</param>
        /// <returns>The property.</returns>
        /// <exception cref="ArgumentException">Thrown, if no property available with the desired name.</exception>
        public InfoOutputProperty GetProperty(string propertyName, IdentifierString propertyGroupName)
        {
            if (TryGetProperty(propertyName, out InfoOutputProperty value, propertyGroupName) == true)
            {
                return value;
            }
            throw new ArgumentException(String.Format(ExceptionMessages.PropertyNameUnknown, propertyGroupName + "." + propertyName), propertyName);
        }

        /// <summary>Gets the collection of properties with respect to a specific property group name.
        /// </summary>
        /// <param name="propertyGroupName">The name of the property group (i.e. 'General Properties' etc.).</param>
        /// <param name="value">The property collection (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public bool TryGetProperties(string propertyGroupName, out IIdentifierStringDictionary<InfoOutputProperty> value)
        {
            if (propertyGroupName != null)
            {
                if (m_Properties.TryGetValue(propertyGroupName, out IdentifierStringDictionaryBase<InfoOutputProperty> propertyCollection) == true)
                {
                    value = propertyCollection;
                    return true;
                }
            }
            value = null;
            return false;
        }

        /// <summary>Gets the collection of properties with respect to a specific property group name.
        /// </summary>
        /// <param name="propertyGroupName">The name of the property group (i.e. 'General Properties' etc.).</param>
        /// <returns>The property collection.</returns>
        public IIdentifierStringDictionary<InfoOutputProperty> GetProperties(string propertyGroupName)
        {
            if (TryGetProperties(propertyGroupName, out IIdentifierStringDictionary<InfoOutputProperty> value) == true)
            {
                return value;
            }
            throw new ArgumentException(String.Format(ExceptionMessages.PropertyGroupNameUnknown, propertyGroupName), propertyGroupName);
        }

        /// <summary>Gets the collection of properties with respect to a specific property group name.
        /// </summary>
        /// <param name="propertyGroupName">The name of the property group (i.e. 'General Properties' etc.).</param>
        /// <param name="value">The property collection (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public bool TryGetProperties(IdentifierString propertyGroupName, out IIdentifierStringDictionary<InfoOutputProperty> value)
        {
            if (propertyGroupName != null)
            {
                if (m_Properties.TryGetValue(propertyGroupName, out IdentifierStringDictionaryBase<InfoOutputProperty> propertyCollection) == true)
                {
                    value = propertyCollection;
                    return true;
                }
            }
            value = null;
            return false;
        }

        /// <summary>Gets the collection of properties with respect to a specific property group name.
        /// </summary>
        /// <param name="propertyGroupName">The name of the property group (i.e. 'General Properties' etc.).</param>
        /// <returns>The property collection.</returns>
        public IIdentifierStringDictionary<InfoOutputProperty> GetProperties(IdentifierString propertyGroupName)
        {
            if (TryGetProperties(propertyGroupName, out IIdentifierStringDictionary<InfoOutputProperty> value) == true)
            {
                return value;
            }
            throw new ArgumentException(String.Format(ExceptionMessages.PropertyGroupNameUnknown, (String)propertyGroupName), (String)propertyGroupName);
        }

        /// <summary>Gets a specific <see cref="InfoOutputProperty"/> object which represents the value of a property with respect to <see cref="GeneralPropertyGroupName"/>.
        /// </summary>
        /// <param name="propertyName">The name of the general property.</param>
        /// <param name="value">The general property (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public bool TryGetGeneralProperty(string propertyName, out InfoOutputProperty value)
        {
            if (propertyName != null)
            {
                return m_GeneralProperties.TryGetValue(propertyName, out value);
            }
            value = default;
            return false;
        }

        /// <summary>Gets a specific <see cref="InfoOutputProperty"/> object which represents the value of a property with respect to <see cref="GeneralPropertyGroupName"/>.
        /// </summary>
        /// <param name="propertyName">The name of the general property.</param>
        /// <param name="value">The general property (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public bool TryGetGeneralProperty(IdentifierString propertyName, out InfoOutputProperty value)
        {
            if (propertyName != null)
            {
                return m_GeneralProperties.TryGetValue(propertyName, out value);
            }
            value = default;
            return false;
        }

        /// <summary>Adds a specified <see cref="DataTable"/> object, i.e. with respect to <see cref="DataTableType.Single"/>. 
        /// </summary>
        /// <param name="value">Homogeneous informations in some <see cref="System.Data.DataTable"/> representation.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown, if the table name of <paramref name="value"/> is empty.</exception>
        public void Add(DataTable value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            if ((value.TableName == null) || (value.TableName.Length == 0))
            {
                throw new ArgumentException(String.Format(ExceptionMessages.ArgumentIsNotWellDefined, "Table name"), "value");
            }
            m_Tables.Add(value.TableName, value);
        }

        /// <summary>Adds a specified <see cref="InfoOutputParentChildDataTable"/> object, i.e. a parent and child <see cref="DataTable"/> object with a 1:n relation. 
        /// </summary>
        /// <param name="value">Homogeneous informations in some <see cref="InfoOutputParentChildDataTable"/> representation.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown, if the table name(s) of <paramref name="value"/> is empty.</exception>
        public void Add(InfoOutputParentChildDataTable value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            if ((value.ParentDataTable == null) || (value.ParentDataTable.TableName == null) || (value.ParentDataTable.TableName.Length == 0))
            {
                throw new ArgumentException(String.Format(ExceptionMessages.ArgumentIsNotWellDefined, "Parent DataTable name"), "value");
            }
            m_ParentChildTables.Add(value.ParentDataTable.TableName, value);
        }

        /// <summary>Adds a specified <see cref="InfoOutputParentChildDataTable"/> object, i.e. a parent and child <see cref="DataTable"/> object with a 1:n relation. 
        /// </summary>
        /// <param name="parentDataTable">The parent data table.</param>
        /// <param name="childDataTable">The child data table.</param>
        /// <param name="parentDataColumn">The parent data column, i.e. the column with respect to the relation between parent und child data table.</param>
        /// <param name="childDataColumn">The child data column, i.e. the column with respect to the relation between parent und child data table.</param>
        public void Add(DataTable parentDataTable, DataTable childDataTable, DataColumn parentDataColumn, DataColumn childDataColumn)
        {
            Add(new InfoOutputParentChildDataTable(parentDataTable, childDataTable, parentDataColumn, childDataColumn));
        }

        /// <summary>Adds a specified collection of <see cref="InfoOutputProperty"/> objects to the current instance.
        /// </summary>
        /// <param name="propertyGroupName">The name of the property group (i.e. 'General Properties' etc.).</param>
        /// <param name="properties">The properties to add, if <c>null</c> an empty property group will be added.</param>
        public void Add(string propertyGroupName, params InfoOutputProperty[] properties)
        {
            if (propertyGroupName == null)
            {
                throw new ArgumentNullException(nameof(propertyGroupName));
            }

            if (properties != null)
            {
                if (m_Properties.TryGetValue(propertyGroupName, out IdentifierStringDictionary<InfoOutputProperty> propertyCollection) == false)
                {
                    propertyCollection = new IdentifierStringDictionary<InfoOutputProperty>(isReadOnlyExceptAdding: false);
                    m_Properties.Add(propertyGroupName, propertyCollection);
                }
                foreach (var property in properties)
                {
                    propertyCollection.Add(property.Name, property);
                }
            }
        }

        /// <summary>Adds a specified collection of <see cref="InfoOutputProperty"/> objects to the current instance with respect to <see cref="GeneralPropertyGroupName"/>.
        /// </summary>
        /// <param name="generalProperties">The general properties to add.</param>
        public void Add(params InfoOutputProperty[] generalProperties)
        {
            if (generalProperties != null)
            {
                foreach (var property in generalProperties)
                {
                    m_GeneralProperties.Add(property.Name, property);
                }
            }
        }

        /// <summary>Adds a specific property to the current instance with respect to <see cref="GeneralPropertyGroupName"/>.
        /// </summary>
        /// <param name="generalPropertyName">The name of the general property.</param>
        /// <param name="generalPropertyValue">The value of the general property.</param>
        public void Add(string generalPropertyName, object generalPropertyValue)
        {
            InfoOutputProperty property = new InfoOutputProperty(generalPropertyName, generalPropertyValue);
            m_GeneralProperties.Add(property.Name, property);
        }

        /// <summary>Adds a specific property to the current instance with respect to <see cref="GeneralPropertyGroupName"/>.
        /// </summary>
        /// <param name="generalPropertyName">The name of the general property.</param>
        /// <param name="generalPropertyValue">The value of the general property.</param>
        /// <param name="annotation">A description of the property.</param>
        public void Add(string generalPropertyName, object generalPropertyValue, string annotation)
        {
            InfoOutputProperty property = new InfoOutputProperty(generalPropertyName, generalPropertyValue, annotation);
            m_GeneralProperties.Add(property.Name, property);
        }
        #endregion
    }
}