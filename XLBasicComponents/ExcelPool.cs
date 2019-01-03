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

using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Utilities;
using Dodoni.BasicComponents.Containers;

using Dodoni.XLBasicComponents.IO;
using Dodoni.XLBasicComponents.Utilities;
using Dodoni.BasicComponents.Logging;

namespace Dodoni.XLBasicComponents
{
    /// <summary>Represents the object pool in Excel.
    /// </summary>
    public static class ExcelPool
    {
        #region nested stuff

        /// <summary>Handles the event of <see cref="ExcelPool"/> which will be raised after a new <see cref="ExcelPoolItem"/> object has been added to the pool.
        /// </summary>
        public class ItemAddedEventArgs : EventArgs
        {
            #region public (readonly) members

            /// <summary>The <see cref="ExcelPoolItem"/> which has been added to the <see cref="ExcelPool"/>.
            /// </summary>
            public readonly ExcelPoolItem Item;

            /// <summary>A value indicating whether <see cref="Item"/> replaced an object in the <see cref="ExcelPool"/> with the same name.
            /// </summary>
            public readonly ItemAddedState State;
            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="ItemAddedEventArgs"/> class.
            /// </summary>
            /// <param name="excelPoolItem">The <see cref="ExcelPoolItem"/> object which has been added.</param>
            /// <param name="state">A value indicating whether <paramref name="excelPoolItem"/> replaced an other item in the <see cref="ExcelPool"/> with the same name.</param>
            internal ItemAddedEventArgs(ExcelPoolItem excelPoolItem, ItemAddedState state)
            {
                Item = excelPoolItem;
                State = state;
            }
            #endregion
        }
        #endregion

        #region public static members

        /// <summary>Occurs after all elements of the <see cref="ExcelPool"/> has been removed.
        /// </summary>
        /// <remarks>Use this event for example to load a standard set of <see cref="ExcelPoolItem"/> objects from a file.</remarks>
        public static event Action<EventArgs> AfterClear;

        /// <summary>Occurs after a new <see cref="ExcelPoolItem"/> object has been added to the pool.
        /// </summary>
        public static event Action<ItemAddedEventArgs> ItemAdded;
        #endregion

        #region private (static) members

        /// <summary>The pool of elements, where the key is the identifier string representation.
        /// </summary>
        private static IdentifierNameableDictionary<ExcelPoolItem> sm_Pool = new IdentifierNameableDictionary<ExcelPoolItem>(capacity: 100, isReadOnlyExceptAdding: false);

        /// <summary>The logging level for the <see cref="ExcelPool"/>.
        /// </summary>
        private static ExcelPoolLoggingLevel sm_LoggingLevel = ExcelPoolLoggingLevel.TrackPoolChanges;

        /// <summary>The XML tag in the configuration file for the 'logging level' combobox.
        /// </summary>
        private const string m_LoggingLevelConfigKey = "GlobalLogfileLoggingLevel";
        #endregion

        #region static constructor

        /// <summary>Initializes the <see cref="ExcelPool"/> class.
        /// </summary>
        static ExcelPool()
        {
            GetLoggingLevelFromConfigFile();  // read from config file
        }
        #endregion

        #region public (static) properties

        /// <summary>Gets the number of pool elements.
        /// </summary>
        /// <value>The number of pool elements.</value>
        public static int Count
        {
            get { return sm_Pool.Count; }
        }

        /// <summary>Gets the <see cref="ExcelPoolItem"/> objects.
        /// </summary>
        /// <value>The <see cref="ExcelPoolItem"/> objects of the <see cref="ExcelPool"/>.</value>
        public static IEnumerable<ExcelPoolItem> Items
        {
            get { return sm_Pool.Values; }
        }

        /// <summary>Gets the logging level of the <see cref="ExcelPool"/>.
        /// </summary>
        /// <value>The logging level.</value>
        public static ExcelPoolLoggingLevel LoggingLevel
        {
            get { return sm_LoggingLevel; }
        }
        #endregion

        #region public (static) methods

        #region file operations

        /// <summary>Gets the object names with respect to a specific <see cref="IObjectStreamReader"/> instance.
        /// </summary>
        /// <param name="objectStreamReader">The object stream reader.</param>
        /// <returns>A collection where the first component represents the <see cref="ExcelPoolItemType"/> and the second component represents the object name.</returns>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="objectStreamReader"/> is <c>null</c>.</exception>
        public static IEnumerable<Tuple<ExcelPoolItemType, string>> GetObjectNames(IObjectStreamReader objectStreamReader)
        {
            if (objectStreamReader == null)
            {
                throw new ArgumentNullException("objectStreamReader");
            }
            return objectStreamReader.GetObjectNames();
        }

        /// <summary>Load objects from a specific stream and add the objects to the <see cref="ExcelPool"/>.
        /// </summary>
        /// <param name="objectStreamReader">The object stream reader.</param>
        /// <param name="infoMessage">A <see cref="System.String"/> which represent a summary of the file operation, perhaps a error message (output).</param>
        /// <param name="excelPoolItems">The collection of added <see cref="ExcelPoolItem"/> objects (output).</param>
        /// <returns>A value indicating whether the file operation was succeeded.</returns>
        public static bool TryLoadObjects(IObjectStreamReader objectStreamReader, out string infoMessage, out IEnumerable<ExcelPoolItem> excelPoolItems)
        {
            if (objectStreamReader == null)
            {
                throw new ArgumentNullException("objectStreamReader");
            }
            return TryAddObjects(objectStreamReader.GetObjects(), out infoMessage, out excelPoolItems);
        }

        /// <summary>Load objects with respect to a specific object name from a specific stream and add the objects into the pool.
        /// </summary>
        /// <param name="objectStreamReader">The object stream reader.</param>
        /// <param name="objectNames">The object names; if <c>null</c> all elements in the <paramref name="objectStreamReader"/> will be loaded.</param>
        /// <param name="infoMessage">A <see cref="System.String"/> which represent a summary of the file operation, perhaps a error message (output).</param>
        /// <param name="excelPoolItems">The collection of added <see cref="ExcelPoolItem"/> objects (output).</param>
        /// <returns>A value indicating whether the file operation was succeeded.</returns>
        public static bool TryLoadObjectsByName(IObjectStreamReader objectStreamReader, IEnumerable<string> objectNames, out string infoMessage, out IEnumerable<ExcelPoolItem> excelPoolItems)
        {
            if (objectStreamReader == null)
            {
                throw new ArgumentNullException("objectStreamReader");
            }
            return TryAddObjects(objectStreamReader.GetObjectsByName(objectNames), out infoMessage, out excelPoolItems);
        }

        /// <summary>Loads objects with respect to a specific type name from a specific stream and add the objects into the pool.
        /// </summary>
        /// <param name="objectStreamReader">The object stream reader.</param>
        /// <param name="excelPoolItemTypes">The object types to take into acount; if <c>null</c> all elements in the <paramref name="objectStreamReader"/> will be loaded.</param>
        /// <param name="infoMessage">A <see cref="System.String"/> which represent a summary of the file operation, perhaps a error message (output).</param>
        /// <param name="excelPoolItems">The collection of added <see cref="ExcelPoolItem"/> objects (output).</param>
        /// <returns>A value indicating whether the file operation was succeeded.</returns>
        public static bool TryLoadObjectsByTypeNames(IObjectStreamReader objectStreamReader, IEnumerable<ExcelPoolItemType> excelPoolItemTypes, out string infoMessage, out IEnumerable<ExcelPoolItem> excelPoolItems)
        {
            if (objectStreamReader == null)
            {
                throw new ArgumentNullException("objectStreamReader");
            }
            return TryAddObjects(objectStreamReader.GetObjectsByTypeName(excelPoolItemTypes), out infoMessage, out excelPoolItems);
        }

        /// <summary>Writes all pool elements into a specific stream.
        /// </summary>
        /// <param name="objectStreamWriter">The object stream writer.</param>
        /// <param name="infoMessage">A <see cref="System.String"/> which represent a summary of the file operation, perhaps a error message (output).</param>
        /// <returns>A value indicating whether the file operation was succeeded.</returns>
        public static bool TryWriteObjects(IObjectStreamWriter objectStreamWriter, out string infoMessage)
        {
            return TryWriteObjects(objectStreamWriter, null, out infoMessage);
        }

        /// <summary>Writes pool elements into a specific stream with respect to specific object names.
        /// </summary>
        /// <param name="objectStreamWriter">The object stream writer.</param>
        /// <param name="objectNames">The object names to take into account; if <c>null</c> all pool elements will be stored into the stream.</param>
        /// <param name="infoMessage">A <see cref="System.String"/> which represent a summary of the file operation, perhaps a error message (output).</param>
        /// <returns>A value indicating whether the file operation was succeeded.</returns>
        public static bool TryWriteObjectsByName(IObjectStreamWriter objectStreamWriter, IEnumerable<string> objectNames, out string infoMessage)
        {
            if ((objectNames == null) || (objectNames.Count() == 0))
            {
                return TryWriteObjects(objectStreamWriter, null, out infoMessage);
            }
            return TryWriteObjects(objectStreamWriter, (from objName in objectNames select (IdentifierString)objName.ToIdentifierString()).ToArray(), out infoMessage);
        }

        /// <summary>Writes pool elements into a specific stream with respect to specific object type names.
        /// </summary>
        /// <param name="objectStreamWriter">The object stream writer.</param>
        /// <param name="excelPoolItemTypes">The object type names to take into account; if <c>null</c> all pool elements will be stored into the stream.</param>
        /// <param name="infoMessage">A <see cref="System.String"/> which represent a summary of the file operation, perhaps a error message (output).</param>
        /// <returns>A value indicating whether the file operation was succeeded.</returns>
        public static bool WriteObjectsByTypeNames(IObjectStreamWriter objectStreamWriter, IEnumerable<ExcelPoolItemType> excelPoolItemTypes, out string infoMessage)
        {
            if ((excelPoolItemTypes == null) || (excelPoolItemTypes.Count() == 0))
            {
                return TryWriteObjects(objectStreamWriter, null, out infoMessage);
            }
            IEnumerable<Guid> itemTypeIdentifiers = (from objTypeName in excelPoolItemTypes select objTypeName.Identifier).ToArray();

            return TryWriteObjects(objectStreamWriter,
                (from poolItem in sm_Pool
                 where itemTypeIdentifiers.Contains(poolItem.ObjectType.Identifier)
                 select poolItem.ObjectName).ToArray(), out infoMessage);
        }
        #endregion

        #region insert methods

        /// <summary>Adds a specific <see cref="ExcelPoolItem"/> object into the <see cref="ExcelPool"/>.
        /// </summary>
        /// <param name="excelPoolItem">The object to insert into the pool.</param>
        /// <returns>A value indicating whether <paramref name="excelPoolItem"/> has been inserted to the pool.</returns>
        public static ItemAddedState InsertObject(ExcelPoolItem excelPoolItem)
        {
            lock (sm_Pool)
            {
                if (excelPoolItem == null)
                {
                    if (ItemAdded != null)
                    {
                        ItemAdded(new ItemAddedEventArgs(null, ItemAddedState.Rejected));
                    }
                    return ItemAddedState.Rejected;
                }
                ExcelPoolItem oldItem;

                if (sm_Pool.TryGetValue(excelPoolItem.ObjectName, out oldItem) == true)
                {
                    if (oldItem.Value is IDisposable)
                    {
                        ((IDisposable)oldItem.Value).Dispose();
                    }
                    sm_Pool[excelPoolItem.ObjectName] = excelPoolItem;
                    if (sm_LoggingLevel == ExcelPoolLoggingLevel.TrackPoolChanges)
                    {
                        // Logger.Stream.Add_Info_PoolItemReplaced(senderObjectTypeName: "ExcelPool", senderObjectType: typeof(ExcelPool), senderObjectName: excelPoolItem.ObjectName.String);
                    }
                    if (ItemAdded != null)
                    {
                        ItemAdded(new ItemAddedEventArgs(excelPoolItem, ItemAddedState.Replaced));
                    }
                    return ItemAddedState.Replaced;
                }
                sm_Pool.Add(excelPoolItem);
                if (sm_LoggingLevel == ExcelPoolLoggingLevel.TrackPoolChanges)
                {
                  //  Logger.Stream.Add_Info_PoolItemAdded(senderObjectTypeName: "ExcelPool", senderObjectType: typeof(ExcelPool), senderObjectName: excelPoolItem.ObjectName.String);
                }
                if (ItemAdded != null)
                {
                    ItemAdded(new ItemAddedEventArgs(excelPoolItem, ItemAddedState.Added));
                }
                return ItemAddedState.Added;
            }
        }

        /// <summary>Adds a specific <see cref="ExcelPoolItem"/> object into the <see cref="ExcelPool"/>.
        /// </summary>
        /// <param name="tryCreateExcelPoolItem">A delegate for creating a <see cref="ExcelPoolItem"/> object via <paramref name="inputExcelDataQueries"/>.</param>
        /// <param name="inputExcelDataQueries">A collection of <see cref="IExcelDataQuery"/> objects used as input to construct a <see cref="ExcelPoolItem"/> object.</param>
        /// <returns>A <see cref="System.String"/> representation which contains a error message or the name of the object to insert together with some time stamp.</returns>
        public static string InsertObject(TryCreateExcelPoolItem tryCreateExcelPoolItem, IIdentifierStringDictionary<IExcelDataQuery> inputExcelDataQueries)
        {
            ExcelPoolItem value;
            string errorMessage;
            if (tryCreateExcelPoolItem(inputExcelDataQueries, out value, out errorMessage) == false)
            {
                return errorMessage;
            }
            if (InsertObject(value) == ItemAddedState.Rejected)
            {
                return "Error! Object rejected, i.e. not added to the [Excel] pool.";
            }
            return value.GetObjectNameWithTimeStamp();
        }

        /// <summary>Adds a collection of <see cref="ExcelPoolItem"/> object into the <see cref="ExcelPool"/>.
        /// </summary>
        /// <param name="tryCreateExcelPoolItem">A delegate for creating a collection of <see cref="ExcelPoolItem"/> objects via <paramref name="inputExcelDataQueries"/>.</param>
        /// <param name="inputExcelDataQueries">A collection of <see cref="IExcelDataQuery"/> objects used as input to construct a collection of <see cref="ExcelPoolItem"/> objects.</param>
        /// <returns>A <see cref="System.String"/> representation which contains a error message or the name of the object to insert together with some time stamp.</returns>
        public static IEnumerable<string> InsertObject(TryCreateExcelPoolItems tryCreateExcelPoolItem, IIdentifierStringDictionary<IExcelDataQuery> inputExcelDataQueries)
        {
            IEnumerable<ExcelPoolItem> values;
            string errorMessage;
            List<string> output = new List<string>();

            if (tryCreateExcelPoolItem(inputExcelDataQueries, out values, out errorMessage) == false)
            {
                output.Add(errorMessage);
            }
            else
            {
                Add(values, output);
            }
            return output;
        }
        #endregion

        /// <summary>Gets a specific element of the <see cref="ExcelPool"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="name">The name of the object in its <see cref="IdentifierString"/> representation.</param>
        /// <param name="value">The requested object (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public static bool TryGetObject<T>(IdentifierString name, out T value)
        {
            if ((name != null) && (name.IDString.Length > 0))
            {
                ExcelPoolItem item;
                if (sm_Pool.TryGetValue(name, out item) == true)
                {
                    if (item.Value is T)
                    {
                        value = (T)item.Value;
                        return true;
                    }
                    else if (item.Value is InfoOutput<T>)
                    {
                        value = ((InfoOutput<T>)item.Value).Value;
                        return true;
                    }
                }
            }
            value = default(T);
            return false;
        }

        /// <summary>Gets a specific element of the <see cref="ExcelPool"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="name">The name of the object in its <see cref="IdentifierString"/> representation.</param>
        /// <returns>The requested object.</returns>
        /// <exception cref="KeyNotFoundException">Thrown, if no item with name <paramref name="name"/> available in <see cref="ExcelPool"/>.</exception>
        public static T GetObject<T>(IdentifierString name)
        {
            T value;
            if (TryGetObject(name, out value) == true)
            {
                return value;
            }
            throw new KeyNotFoundException(String.Format(XLResources.NotFoundExcelItemException, name != null ? name.String : "Unknown"));
        }

        /// <summary>Gets a specific element of the <see cref="ExcelPool"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="name">The name of the object in its <see cref="System.String"/> representation.</param>
        /// <param name="value">The requested object (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public static bool TryGetObject<T>(string name, out T value)
        {
            if ((name != null) && (name.Length > 0))
            {
                return TryGetObject<T>(new IdentifierString(name), out value);
            }
            value = default(T);
            return false;
        }

        /// <summary>Gets a specific element of the <see cref="ExcelPool"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="name">The name of the object in its <see cref="System.String"/> representation.</param>
        /// <returns>The requested object.</returns>
        /// <exception cref="KeyNotFoundException">Thrown, if no item with name <paramref name="name"/> available in <see cref="ExcelPool"/>.</exception>
        public static T GetObject<T>(string name)
        {
            if ((name != null) && (name.Length > 0))
            {
                return GetObject<T>(new IdentifierString(name));
            }
            throw new KeyNotFoundException(String.Format(XLResources.NotFoundExcelItemException, name));
        }

        /// <summary>Gets a specific <see cref="InfoOutput&lt;T&gt;"/> object of the <see cref="ExcelPool"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="name">The name of the <see cref="InfoOutput&lt;T&gt;"/> object.</param>
        /// <returns>The requested object.</returns>
        /// <exception cref="KeyNotFoundException">Thrown, if no item with name <paramref name="name"/> available in <see cref="ExcelPool"/>.</exception>
        public static T GetInfoOutputObject<T>(string name)
        {
            return GetObject<InfoOutput<T>>(name).Value;
        }

        /// <summary>Gets a specific <see cref="InfoOutput&lt;T&gt;"/> object of the <see cref="ExcelPool"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="name">The name of the <see cref="InfoOutput&lt;T&gt;"/> object.</param>
        /// <returns>The requested object.</returns>
        /// <exception cref="KeyNotFoundException">Thrown, if no item with name <paramref name="name"/> available in <see cref="ExcelPool"/>.</exception>
        public static T GetInfoOutputObject<T>(IdentifierString name)
        {
            return GetObject<InfoOutput<T>>(name).Value;
        }

        /// <summary>Gets a specific element of the <see cref="ExcelPool"/>.
        /// </summary>
        /// <param name="name">The name of the object in its <see cref="System.String"/> representation.</param>
        /// <param name="value">The requested object (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public static bool TryGetItem(string name, out ExcelPoolItem value)
        {
            return sm_Pool.TryGetValue(name, out value);
        }

        /// <summary>Gets a specific element of the <see cref="ExcelPool"/>.
        /// </summary>
        /// <param name="name">The name of the object in its <see cref="System.String"/> representation.</param>
        /// <returns>The requested object.</returns>
        public static ExcelPoolItem GetItem(string name)
        {
            return sm_Pool[name];
        }

        /// <summary>Gets a specific element of the <see cref="ExcelPool"/>.
        /// </summary>
        /// <param name="name">The name of the object in its <see cref="IdentifierString"/> representation.</param>
        /// <param name="value">The requested object (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public static bool TryGetItem(IdentifierString name, out ExcelPoolItem value)
        {
            return sm_Pool.TryGetValue(name, out value);
        }

        /// <summary>Gets a specific element of the <see cref="ExcelPool"/>.
        /// </summary>
        /// <param name="name">The name of the object in its <see cref="IdentifierString"/> representation.</param>
        /// <returns>The requested object.</returns>
        public static ExcelPoolItem GetItem(IdentifierString name)
        {
            return sm_Pool[name];
        }

        /// <summary>Gets the <see cref="ExcelPoolItem"/> objects with respect to a specific <see cref="ExcelPoolItemType"/>.
        /// </summary>
        /// <param name="excelPoolItemType">The type of the excel pool item.</param>
        /// <returns>A collection of the <see cref="ExcelPoolItem"/> objects in the <see cref="ExcelPool"/> of with respect to <paramref name="excelPoolItemType"/>.</returns>
        public static IEnumerable<ExcelPoolItem> GetItems(ExcelPoolItemType excelPoolItemType)
        {
            return (from item in sm_Pool
                    where item.ObjectType.Identifier == excelPoolItemType.Identifier
                    select item).ToArray();
        }

        /// <summary>Gets a specific element of the <see cref="ExcelPool"/>.
        /// </summary>
        /// <param name="name">The name of the object.</param>
        /// <param name="value">The requested object (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public static bool TryGetObject(string name, out IInfoOutputQueriable value)
        {
            ExcelPoolItem item;
            if (sm_Pool.TryGetValue(name, out item) == true)
            {
                value = item.Value;
                return true;
            }
            value = null;
            return false;
        }

        /// <summary>Removes all elements of the <see cref="ExcelPool"/>.
        /// </summary>
        /// <remarks>The <see cref="AfterClear"/> event occurs after the elements of the <see cref="ExcelPool"/> has been removed.</remarks>
        public static void Clear()
        {
            sm_Pool.Clear();
            if (AfterClear != null)
            {
                AfterClear(new EventArgs());
            }
        }

        /// <summary>Removes a specific <see cref="ExcelPoolItem"/> object.
        /// </summary>
        /// <param name="name">The name of the <see cref="ExcelPoolItem"/> object to remove.</param>
        /// <returns><c>true</c> if the element is sucessfully found and removed; otherwise, <c>false</c>.</returns>
        /// <remarks>If the value of the <see cref="ExcelPoolItem"/> object to remove implements the <see cref="IDisposable"/> interface, a 
        /// freeing, releasing, or resetting of unmanaged resources will be applied.</remarks>
        public static bool RemoveItem(string name)
        {
            ExcelPoolItem itemToRemove;
            if (sm_Pool.TryGetValue(name, out itemToRemove) == true)
            {
                if (itemToRemove.Value is IDisposable)
                {
                    ((IDisposable)itemToRemove.Value).Dispose();
                }
                return sm_Pool.Remove(name);
            }
            return false;
        }

        /// <summary>Removes a specific <see cref="ExcelPoolItem"/> object.
        /// </summary>
        /// <param name="name">The name of the <see cref="ExcelPoolItem"/> object to remove.</param>
        /// <returns><c>true</c> if the element is sucessfully found and removed; otherwise, <c>false</c>.</returns>
        /// <remarks>If the value of the <see cref="ExcelPoolItem"/> object to remove implements the <see cref="IDisposable"/> interface, a 
        /// freeing, releasing, or resetting of unmanaged resources will be applied.</remarks>
        public static bool RemoveItem(IdentifierString name)
        {
            ExcelPoolItem itemToRemove;
            if (sm_Pool.TryGetValue(name, out itemToRemove) == true)
            {
                if (itemToRemove.Value is IDisposable)
                {
                    ((IDisposable)itemToRemove.Value).Dispose();
                }
                return sm_Pool.Remove(name);
            }
            return false;
        }

        /// <summary>Gets a collection of the names of the <see cref="ExcelPool"/> items.
        /// </summary>
        /// <returns>A collection of the names of the <see cref="ExcelPool"/> items.</returns>
        public static IEnumerable<IdentifierString> GetObjectNames()
        {
            return (from item in sm_Pool
                    select item.ObjectName).ToArray();
        }

        /// <summary>Gets the object names with respect to a specific object type.
        /// </summary>
        /// <param name="excelPoolItemType">The type of the pool element in its <see cref="ExcelPoolItemType"/> representation.</param>
        /// <returns>The names of the objects in the <see cref="ExcelPool"/> where the object type is equal to <paramref name="excelPoolItemType"/>;
        /// the name of the return value is equal to the name of <paramref name="excelPoolItemType"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="excelPoolItemType"/> is <c>null</c>.</exception>
        public static IExcelDataAdvice GetObjectNames(ExcelPoolItemType excelPoolItemType)
        {
            if (excelPoolItemType == null)
            {
                throw new ArgumentNullException("excelPoolItemType");
            }
            return ExcelDataAdvice.Create((from item in sm_Pool
                                           where item.ObjectType.Identifier == excelPoolItemType.Identifier
                                           select item.ObjectName),
                                           excelPoolItemType.Name.String);
        }

        /// <summary>Gets the object names with respect to a specific object type.
        /// </summary>
        /// <typeparam name="T">The type of the objects.</typeparam>
        /// <returns>The names of the objects in the <see cref="ExcelPool"/> of type <typeparamref name="T"/> in
        /// its <see cref="IdentifierString"/> representation.</returns>
        public static IEnumerable<IdentifierString> GetObjectNames<T>()
        {
            return (from item in sm_Pool
                    where item.Value is T
                    select item.ObjectName).ToArray();
        }

        #region extension methods (for ExcelBackgroundWorker)

        /// <summary>Initializes the current <see cref="ExcelBackgroundWorker"/> object.
        /// </summary>
        /// <param name="excelBackgroundWorker">The <see cref="ExcelBackgroundWorker"/> object.</param>
        /// <param name="excelPoolItemType">The <see cref="ExcelPoolItemType"/> object of the <see cref="ExcelPoolItem"/> object(s) to create.</param>
        public static void Initialize(this ExcelBackgroundWorker excelBackgroundWorker, ExcelPoolItemType excelPoolItemType)
        {
            excelBackgroundWorker.Initialize(String.Format(XLResources.ExcelPoolItemBackgroundWorkerCreationTitle, excelPoolItemType.Name.String));
        }

        /// <summary>Initializes the current <see cref="ExcelBackgroundWorker"/> object.
        /// </summary>
        /// <param name="excelBackgroundWorker">The <see cref="ExcelBackgroundWorker"/> object.</param>
        /// <param name="excelPoolItemType">The <see cref="ExcelPoolItemType"/> object of the <see cref="ExcelPoolItem"/> object(s) to create.</param>
        /// <param name="maximalNumberOfCalculationSteps">The maximal number of calculation steps.</param>
        /// <param name="calculationStepSize">The step size of the calculation.</param>
        public static void Initialize(this ExcelBackgroundWorker excelBackgroundWorker, ExcelPoolItemType excelPoolItemType, int maximalNumberOfCalculationSteps, int calculationStepSize = 1)
        {
            excelBackgroundWorker.Initialize(String.Format(XLResources.ExcelPoolItemBackgroundWorkerCreationTitle, excelPoolItemType.Name.String), maximalNumberOfCalculationSteps, calculationStepSize);
        }
        #endregion

        #endregion

        #region internal (static) methods

        /// <summary>Sets the <see cref="LoggingLevel"/> flag and store it into the config file.
        /// </summary>
        /// <param name="loggingLevel">The logging level.</param>
        internal static void StoreLoggingLevel(ExcelPoolLoggingLevel loggingLevel)
        {
            sm_LoggingLevel = loggingLevel;
            ExcelAddIn.Configuration.GeneralSettings.SetValue(m_LoggingLevelConfigKey, loggingLevel.ToFormatString(EnumStringRepresentationUsage.StringAttribute));
        }

        /// <summary>Gets the <see cref="LoggingLevel"/> flag with respect to the config file.
        /// </summary>
        /// <returns>The loaded <see cref="ExcelPoolLoggingLevel"/> or a standard value.</returns>
        internal static ExcelPoolLoggingLevel GetLoggingLevelFromConfigFile()
        {
            string loggingLevelAsString;
            if (ExcelAddIn.Configuration.GeneralSettings.TryGetValue(m_LoggingLevelConfigKey, out loggingLevelAsString) == false)
            {
                sm_LoggingLevel = ExcelPoolLoggingLevel.None;
            }
            else
            {
                if (EnumString<ExcelPoolLoggingLevel>.TryParse(loggingLevelAsString, out sm_LoggingLevel, EnumStringRepresentationUsage.StringAttribute) == false)
                {
                    throw new ConfigurationFileErrorException("Configuration file corrupt:" + loggingLevelAsString + " is no valid input for the global logfile logging level.");
                }
            }
            return sm_LoggingLevel;
        }
        #endregion

        #region private (static) methods

        /// <summary>Adds a collection of items into the <see cref="ExcelPool"/> which are given in its <see cref="GuidedExcelDataQuery"/> representation.
        /// </summary>
        /// <param name="objects">The objects to add, i.e. a collection where the first component is the object type in its <see cref="ExcelPoolItemType"/> representation,
        /// the second component is the object name and the third component represents the data needed to construct the desired objector; or the first and third component 
        /// is <c>null</c> if no entry with the desired object name is available.</param>
        /// <param name="infoMessage">A <see cref="System.String"/> which represent a summary of the file operation, perhaps a error message (output).</param>
        /// <param name="excelPoolItems">The collection of added <see cref="ExcelPoolItem"/> objects (output).</param>
        /// <returns>A value indicating whether the operation was succeeded.</returns>
        /// <remarks>The <see cref="ExcelPoolItem"/> objects to insert must be given in a correct order taken into account dependencies of the input.</remarks>
        private static bool TryAddObjects(IEnumerable<Tuple<ExcelPoolItemType, string, IIdentifierStringDictionary<GuidedExcelDataQuery>>> objects, out string infoMessage, out IEnumerable<ExcelPoolItem> excelPoolItems)
        {
            if ((objects == null) || (objects.Count() == 0))
            {
                infoMessage = "No data to add.";
                excelPoolItems = null;
                return false;
            }
            List<ExcelPoolItem> addedItems = new List<ExcelPoolItem>();
            StringBuilder strBuilder = new StringBuilder();

            int objectCount = 0;
            int errorCount = 0;

            foreach (var obj in objects)
            {
                if (obj.Item1 == null)
                {
                    strBuilder.AppendLine("No data found for object with name '" + obj.Item2 + "'.");
                    errorCount++;
                }
                else if (obj.Item2 == null)
                {
                    strBuilder.AppendLine("No data found for object for type '" + obj.Item1.Name.String + "'.");
                }
                else
                {
                    ExcelPoolItemCreator guidedObjectCreator;
                    if (ExcelPoolItemCreator.TryGetValue(obj.Item1.Identifier, out guidedObjectCreator) == false)
                    {
                        strBuilder.AppendLine("No method found for generation objects of type '" + obj.Item1.Name.String + "'.");
                        errorCount++;
                    }
                    else
                    {
                        string errorMessage;
                        IEnumerable<ExcelPoolItem> excelPoolItemSet;
                        if (guidedObjectCreator.CreatingFunction(obj.Item3, out excelPoolItemSet, out errorMessage) == true)
                        {
                            foreach (ExcelPoolItem excelPoolItem in excelPoolItemSet)
                            {
                                if (InsertObject(excelPoolItem) == ItemAddedState.Rejected)
                                {
                                    strBuilder.AppendLine("Object '" + excelPoolItem.Name.String + "' of type '" + obj.Item1.Name.String + "' rejected, i.e. not added to the pool.");
                                    errorCount++;
                                }
                                else
                                {
                                    addedItems.Add(excelPoolItem);
                                    objectCount++;
                                }
                            }
                        }
                        else
                        {
                            strBuilder.AppendLine(errorMessage);
                            errorCount++;
                        }
                    }
                }
            }
            if (objectCount == 1)
            {
                strBuilder.AppendLine("Load 1 object.");
            }
            else if (objectCount > 1)
            {
                strBuilder.AppendLine("Load " + objectCount + " objects.");
            }
            else
            {
                strBuilder.AppendLine("No object loaded.");
            }
            if (errorCount > 0)
            {
                strBuilder.AppendLine(errorCount + " errors detected.");
            }
            infoMessage = strBuilder.ToString();
            excelPoolItems = addedItems;
            return (errorCount == 0);
        }

        /// <summary>Writes a subset of the <see cref="ExcelPoolItem"/> objects of the <see cref="ExcelPool"/> into a specific <see cref="IObjectStreamWriter"/> taken into account the 
        /// dependency structure, i.e. first 'independent' items are stored, afterwards objects which are needed such 'independent' input are stored etc.
        /// </summary>
        /// <param name="objectStreamWriter">The object stream writer.</param>
        /// <param name="objectNameFilter">The object name filter, i.e. at least the objects which satisfied this filter will be stored; <c>null</c> is allowed and
        /// in this case all items will be stored..</param>
        /// <param name="infoMessage">A <see cref="System.String"/> which represent a summary of the file operation, perhaps a error message (output).</param>
        /// <returns>A value indicating whether the operation was succeeded.</returns>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="objectStreamWriter"/> is <c>null</c>.</exception>
        private static bool TryWriteObjects(IObjectStreamWriter objectStreamWriter, IEnumerable<IdentifierString> objectNameFilter, out string infoMessage)
        {
            if (objectStreamWriter == null)
            {
                throw new ArgumentNullException("objectStreamWriter");
            }
            StringBuilder infoStrBuilder = new StringBuilder();
            int storedObjectCount = 0;
            int errorCount = 0;

            /* just a foreach-loop where we store the object into the stream is not o.k.,  because we have some dependency structure, because objects can be input
             * for other objects etc. For example swap rates, deposit rates etc. are input objects for a discount factor curve etc. We do a recursive approach: */

            var setOfInsertedItems = new IdentifierStringDictionary<ExcelPoolItem>(isReadOnlyExceptAdding: false);
            if ((objectNameFilter == null) || (objectNameFilter.Count() == 0))
            {
                foreach (var excelPoolItem in sm_Pool)
                {
                    WriteObject(objectStreamWriter, excelPoolItem, setOfInsertedItems, infoStrBuilder, ref storedObjectCount, ref errorCount);
                }
            }
            else
            {
                foreach (var idObjectName in objectNameFilter)
                {
                    ExcelPoolItem excelPoolItem;
                    if (sm_Pool.TryGetValue(idObjectName, out excelPoolItem) == true)
                    {
                        WriteObject(objectStreamWriter, excelPoolItem, setOfInsertedItems, infoStrBuilder, ref storedObjectCount, ref errorCount);
                    }
                    else
                    {
                        infoStrBuilder.AppendLine("No pool item found with name '" + idObjectName.String.GetRelevantSubstring() + "'.");
                    }
                }
            }
            if (storedObjectCount > 0)
            {
                infoStrBuilder.AppendLine("Wrote " + storedObjectCount + " objects.");
            }
            else
            {
                infoStrBuilder.AppendLine("No objects stored.");
            }
            if (errorCount > 0)
            {
                infoStrBuilder.AppendLine(errorCount + " errors detected!");
            }
            infoMessage = infoStrBuilder.ToString();
            return (errorCount == 0);
        }

        /// <summary>Writes a specific object into a <see cref="IObjectStreamWriter"/> and all its dependency input objects.
        /// </summary>
        /// <param name="objectStreamWriter">The object stream writer.</param>
        /// <param name="value">The <c>root</c> item to add, i.e. all dependency input will be added in a recursive way.</param>
        /// <param name="setOfInsertedItems">A collection of the <see cref="ExcelPoolItem"/> objects which are already stored in <paramref name="objectStreamWriter"/>.</param>
        /// <param name="infoStrBuilder">A collector for info string, as for example error messages.</param>
        /// <param name="storedObjectCount">The number of objects added into the <paramref name="objectStreamWriter"/> (output).</param>
        /// <param name="errorCount">The number of error mesages in <paramref name="infoStrBuilder"/> (output).</param>
        /// <remarks><paramref name="setOfInsertedItems"/> is used to avoid multiply entries in <paramref name="objectStreamWriter"/> with the same name.</remarks>
        private static void WriteObject(IObjectStreamWriter objectStreamWriter, ExcelPoolItem value, IdentifierStringDictionary<ExcelPoolItem> setOfInsertedItems, StringBuilder infoStrBuilder, ref int storedObjectCount, ref int errorCount)
        {
            IEnumerable<ExcelPoolItem> inputItems = value.InputDependencyItems;

            if ((inputItems != null) && (inputItems.Count() > 0))  // there are dependencies, i.e. store the dependencies first
            {
                foreach (var dependentExcelPoolItem in inputItems)
                {
                    WriteObject(objectStreamWriter, dependentExcelPoolItem, setOfInsertedItems, infoStrBuilder, ref storedObjectCount, ref errorCount);
                }
            }

            /* if a item with the same name is already added to the stream check whether both objects are equal:  */
            ExcelPoolItem alreadyAddedItem;
            if (setOfInsertedItems.TryGetValue(value.ObjectName, out alreadyAddedItem) == true)
            {
                if (value.Equals(alreadyAddedItem) == false)  // assume that both objects share the same adress
                {
                    infoStrBuilder.AppendLine("Inconsistent data input: Object '" + value.ObjectName.String + "' with time stamp '" + value.TimeStamp.ToString("HH:mm:ss.ff") + " and " + alreadyAddedItem.TimeStamp.ToString("HH:mm:ss.ff") + " [is added]");
                }
            }
            else
            {
                /*  check whether the item is equal to an element of the Excel pool with the 
                 *  same name and add a warning message if it is different: (plausibility check) */

                ExcelPoolItem excelPoolItem;
                if (sm_Pool.TryGetValue(value.ObjectName, out excelPoolItem) == false)
                {
                    infoStrBuilder.AppendLine("Item to add is not in the Excel pool any more.");  // a internal error message
                    errorCount++;
                }
                else if (value.Equals(excelPoolItem) == false)
                {
                    infoStrBuilder.AppendLine("Inconsistent data input: Object '" + value.ObjectName.String + "' with time stamp '" + value.TimeStamp.ToString("HH:mm:ss.ff") + " [is added] and " + excelPoolItem.TimeStamp.ToString("HH:mm:ss.ff"));
                    errorCount++;
                }

                string errorMessage;
                if (TryWriteObject(objectStreamWriter, value, out errorMessage) == false)
                {
                    infoStrBuilder.AppendLine(errorMessage);
                    errorCount++;
                }
                else
                {
                    storedObjectCount++;
                }
                setOfInsertedItems.Add(value.Name, value);
            }
        }

        /// <summary>Writes a specific object in its <see cref="GuidedExcelDataQuery"/> representation into a specific stream.
        /// </summary>
        /// <param name="objectStreamWriter">The object stream writer.</param>
        /// <param name="value">The <see cref="ExcelPoolItem"/> object to store.</param>
        /// <param name="errorMessage">A <see cref="System.String"/> object which may contains a error message.</param>
        /// <returns>A value indicating whether the operation succeeded.</returns>
        private static bool TryWriteObject(IObjectStreamWriter objectStreamWriter, ExcelPoolItem value, out string errorMessage)
        {
            try
            {
                objectStreamWriter.WriteObject(value);
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return false;
            }
            errorMessage = null;
            return true;
        }

        /// <summary>Adds a collection of <see cref="ExcelPoolItem"/> objects.
        /// </summary>
        /// <param name="values">The <see cref="ExcelPoolItem"/> objects to add in the <see cref="ExcelPool"/>.</param>
        /// <param name="output">For each item of <paramref name="values"/> an error message or the object name.</param>
        private static void Add(IEnumerable<ExcelPoolItem> values, List<string> output)
        {
            foreach (var excelPoolItem in values)
            {
                if (InsertObject(excelPoolItem) == ItemAddedState.Rejected)
                {
                    output.Add("Error! Object " + excelPoolItem.Name.String + " rejected, i.e. not added to the [Excel] pool.");
                }
                else
                {
                    output.Add(excelPoolItem.GetObjectNameWithTimeStamp());
                }
            }
        }
        #endregion
    }
}