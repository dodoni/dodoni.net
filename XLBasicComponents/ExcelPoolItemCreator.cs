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
using System.Collections.Generic;

using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Containers;
using Dodoni.XLBasicComponents.Utilities;

namespace Dodoni.XLBasicComponents
{
    /// <summary>This delegate represents a function used to create a specific <see cref="ExcelPoolItem"/> object via a collection of <see cref="IExcelDataQuery"/> instances.
    /// </summary>
    /// <param name="excelDataQueries">The data needed to construct a new object.</param>
    /// <param name="value">The output object (output).</param>
    /// <param name="errorMessage">The error message or undefined if no error occurs (output).</param>
    /// <returns>A value indicating whether <paramref name="value"/> contains valid data; otherwise <paramref name="errorMessage"/> contains an error message.</returns>
    public delegate bool TryCreateExcelPoolItem(IIdentifierStringDictionary<IExcelDataQuery> excelDataQueries, out ExcelPoolItem value, out string errorMessage);

    /// <summary>This delegate represents a function used to create a collection of <see cref="ExcelPoolItem"/> object via a collection of <see cref="IExcelDataQuery"/> instances.
    /// </summary>
    /// <param name="excelDataQueries">The data needed to construct a new object.</param>
    /// <param name="values">The output objects (output).</param>
    /// <param name="errorMessage">The error message or undefined if no error occurs (output).</param>
    /// <returns>A value indicating whether <paramref name="values"/> contains valid data; otherwise <paramref name="errorMessage"/> contains an error message.</returns>
    public delegate bool TryCreateExcelPoolItems(IIdentifierStringDictionary<IExcelDataQuery> excelDataQueries, out IEnumerable<ExcelPoolItem> values, out string errorMessage);

    /// <summary>Represents the data needed to create a specific object via a collection of <see cref="IExcelDataQuery"/> instances.
    /// </summary>
    public struct ExcelPoolItemCreator : IIdentifierNameable
    {
        #region nested stuff

        /// <summary>Handles the initialization event of <see cref="ExcelPoolItemCreator"/> which will be raised before starting to query a specific pool item.
        /// </summary>
        public class InitializeEventArgs : EventArgs
        {
            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="InitializeEventArgs"/> class.
            /// </summary>
            internal InitializeEventArgs()
            {
            }
            #endregion

            #region public methods

            /// <summary>Registers a specific <see cref="ExcelPoolItemCreator"/> object, i.e. stores a mapping from a collection of <see cref="IExcelDataQuery"/> to <see cref="ExcelPoolItem"/>.
            /// </summary>
            /// <param name="value">The <see cref="ExcelPoolItemCreator"/> to register, i.e. used to create objects of a specific type via a collection of <see cref="IExcelDataQuery"/> instances.</param>
            /// <returns>A value indicating whether <paramref name="value"/> has been inserted.</returns>
            /// <exception cref="ArgumentException">Thrown, if a <see cref="ExcelPoolItemCreator"/> with the same (identifier) string or <see cref="System.Guid"/> representation has already been added.</exception>
            public void Add(ExcelPoolItemCreator value)
            {
                ExcelPoolItemCreator.Add(value);
            }
            #endregion
        }
        #endregion

        #region private static members

        /// <summary>Occurs before quering <see cref="ExcelPoolItemCreator"/> from the pool.
        /// </summary>
        private static event Action<InitializeEventArgs> sm_Initialize;

        /// <summary>A value indicating whether the user has added or removed at least one event to the <see cref="ExcelPoolItemCreator.Initialize"/> event handler
        /// since the last call of the event-handler.
        /// </summary>
        /// <remarks>This member is used for performance reason only.</remarks>
        private static bool sm_InitializeChanged = true;

        /// <summary>The collection of <see cref="ExcelPoolItemCreator"/> instances, where the key is the unique <see cref="System.Guid"/>.
        /// </summary>
        /// <remarks>Perhaps the <see cref="ExcelPoolItemCreator"/> object for a specific <see cref="System.Guid"/> or (identifier) string representation is
        /// searched, therefore we use two dictionaries for performance reason.
        /// </remarks>
        private static Dictionary<Guid, ExcelPoolItemCreator> sm_ObjectCreatorsByGuid = new Dictionary<Guid, ExcelPoolItemCreator>(100);

        /// <summary>The collection of <see cref=" ExcelPoolItemCreator"/> instances, where the key is the unique (identifier) string representation.
        /// </summary>
        /// <remarks>Perhaps the <see cref="ExcelPoolItemCreator"/> object for a specific <see cref="System.Guid"/> or (identifier) string representation is
        /// searched, therefore we use two dictionaries for performance reason.
        /// </remarks>
        private static IdentifierNameableDictionary<ExcelPoolItemCreator> sm_ObjectCreatorsByName = null; //new IdentifierNameableDictionary<ExcelPoolItemCreator>(100, isReadOnlyExceptAdding: true);
        #endregion

        #region private/public (readonly) members

        /// <summary>The type of the object to create in its <see cref="ExcelPoolItemType"/> representation.
        /// </summary>
        public readonly ExcelPoolItemType ObjectType;

        /// <summary>A function which creates a collection of <see cref="ExcelPoolItem"/> objects via a collection of <see cref="GuidedExcelDataQuery"/> instances.
        /// </summary>
        public readonly TryCreateExcelPoolItems CreatingFunction;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="ExcelPoolItemCreator"/> struct.
        /// </summary>
        /// <param name="excelPoolItemType">The type of the object to create in its <see cref="ExcelPoolItemType"/> representation.</param>
        /// <param name="excelPoolItemCreatingFunction">The creating function.</param>
        /// <exception cref="ArgumentNullException">Thrown, if one of the arguments is <c>null</c>.</exception>
        /// <remarks>Use the <see cref="ExcelPoolItemCreator.Initialize"/> event to store the <see cref="ExcelPoolItemCreator"/> instance.</remarks>
        public ExcelPoolItemCreator(ExcelPoolItemType excelPoolItemType, TryCreateExcelPoolItem excelPoolItemCreatingFunction)
        {
            if (excelPoolItemType == null)
            {
                throw new ArgumentNullException("excelPoolItemType");
            }
            ObjectType = excelPoolItemType;

            if (excelPoolItemCreatingFunction == null)
            {
                throw new ArgumentNullException("excelPoolItemCreatingFunction");
            }
            CreatingFunction = (IIdentifierStringDictionary<IExcelDataQuery> excelDataQueries, out IEnumerable<ExcelPoolItem> values, out string errorMessage) =>
            {
                ExcelPoolItem value;
                if (excelPoolItemCreatingFunction(excelDataQueries, out value, out errorMessage) == false)
                {
                    values = null;
                    return false;
                }
                values = GetAsEnumerable(value);
                return true;
            };
        }

        /// <summary>Initializes a new instance of the <see cref="ExcelPoolItemCreator"/> struct.
        /// </summary>
        /// <param name="excelPoolItemType">The type of the object to create in its <see cref="ExcelPoolItemType"/> representation.</param>
        /// <param name="excelPoolItemCreatingFunction">The creating function.</param>
        /// <exception cref="ArgumentNullException">Thrown, if one of the arguments is <c>null</c>.</exception>
        /// <remarks>Use the <see cref="ExcelPoolItemCreator.Initialize"/> event to store the <see cref="ExcelPoolItemCreator"/> instance.</remarks>
        public ExcelPoolItemCreator(ExcelPoolItemType excelPoolItemType, TryCreateExcelPoolItems excelPoolItemCreatingFunction)
        {
            if (excelPoolItemType == null)
            {
                throw new ArgumentNullException("excelPoolItemType");
            }
            ObjectType = excelPoolItemType;

            if (excelPoolItemCreatingFunction == null)
            {
                throw new ArgumentNullException("excelPoolItemCreatingFunction");
            }
            CreatingFunction = (IIdentifierStringDictionary<IExcelDataQuery> excelDataQueries, out IEnumerable<ExcelPoolItem> values, out string errorMessage) =>
            {
                return excelPoolItemCreatingFunction(excelDataQueries, out values, out errorMessage);
            };
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the current <see cref="ExcelPoolItemCreator"/>, which is equal to the name of the <see cref="ExcelPoolItemCreator.ObjectType"/>.
        /// </summary>
        /// <value>The language independent name of the current instance.</value>
        public IdentifierString Name
        {
            get { return ObjectType.Name; }
        }

        /// <summary>Gets the long name of the current instance, which is equal to the name of the <see cref="ExcelPoolItemCreator.ObjectType"/>.
        /// </summary>
        /// <value>The language dependent long name of the current instance.</value>
        public IdentifierString LongName
        {
            get { return ObjectType.LongName; }
        }
        #endregion

        #endregion

        #region public static properties

        /// <summary>Gets the number of available <see cref="ExcelPoolItemCreator"/> objects.
        /// </summary>
        /// <value>The number of available <see cref="ExcelPoolItemCreator"/> objects.</value>
        public static int Count
        {
            get
            {
                OnInitialize();
                return sm_ObjectCreatorsByGuid.Count;
            }
        }

        /// <summary>Gets a collection of <see cref="ExcelPoolItemCreator"/> objects.
        /// </summary>
        /// <value>The values.</value>
        public static IEnumerable<ExcelPoolItemCreator> Values
        {
            get
            {
                OnInitialize();
                return sm_ObjectCreatorsByName.Values;
            }
        }

        /// <summary>Occurs before quering a specific <see cref="ExcelPoolItemCreator"/> from the pool.
        /// </summary>
        /// <remarks>In the event-handler one stores the method how to create specific objects via a collection of <see cref="IExcelDataQuery"/> instances.</remarks>
        public static event Action<InitializeEventArgs> Initialize
        {
            add
            {
                sm_InitializeChanged = true;
                sm_Initialize += value;
            }
            remove
            {
                sm_InitializeChanged = true;
                sm_Initialize -= value;
            }
        }
        #endregion

        #region public static methods

        /// <summary>Gets the names of the available <see cref="ExcelPoolItemCreator"/> instances.
        /// </summary>
        /// <returns>A collection of the <see cref="System.String"/> representation of the available <see cref="ExcelPoolItemCreator"/> objects.</returns>
        public static IEnumerable<string> GetNames()
        {
            OnInitialize();
            return sm_ObjectCreatorsByName.Names;
        }

        /// <summary>Gets a specific <see cref="ExcelPoolItemCreator"/> object.
        /// </summary>
        /// <param name="key">The key, i.e. the unique <see cref="System.Guid"/> representation of the <see cref="ExcelPoolItemCreator"/> to search.</param>
        /// <param name="value">The value (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public static bool TryGetValue(Guid key, out ExcelPoolItemCreator value)
        {
            OnInitialize();
            return sm_ObjectCreatorsByGuid.TryGetValue(key, out value);
        }

        /// <summary>Gets a specific <see cref="ExcelPoolItemCreator"/> object.
        /// </summary>
        /// <param name="key">The key, i.e. the unique (identifier) string representation of the <see cref="ExcelPoolItemCreator"/> to search.</param>
        /// <param name="value">The value (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public static bool TryGetValue(string key, out ExcelPoolItemCreator value)
        {
            OnInitialize();
            return sm_ObjectCreatorsByName.TryGetValue(key, out value);
        }

        /// <summary>Gets a specific <see cref="ExcelPoolItemCreator"/> object.
        /// </summary>
        /// <param name="key">The key, i.e. the unique (identifier) string representation of the <see cref="ExcelPoolItemCreator"/> to search.</param>
        /// <param name="value">The value (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public static bool TryGetValue(IdentifierString key, out ExcelPoolItemCreator value)
        {
            OnInitialize();
            return sm_ObjectCreatorsByName.TryGetValue(key, out value);
        }

        /// <summary>Gets the unique <see cref="System.Guid"/> identifier of a specific <see cref="ExcelPoolItemCreator"/>.
        /// </summary>
        /// <param name="name">The name of the <see cref="ExcelPoolItemCreator"/>.</param>
        /// <param name="value">The (unique) <see cref="System.Guid"/> identifier of the <see cref="ExcelPoolItemCreator"/> with name <paramref name="name"/>.</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public static bool TryGetObjectTypeIdentifier(string name, out Guid value)
        {
            OnInitialize();
            ExcelPoolItemCreator excelPoolItemCreator;
            if (sm_ObjectCreatorsByName.TryGetValue(name, out  excelPoolItemCreator) == true)
            {
                value = excelPoolItemCreator.ObjectType.Identifier;
                return true;
            }
            value = Guid.Empty;
            return false;
        }

        /// <summary>Gets the unique <see cref="System.Guid"/> identifier of a specific <see cref="ExcelPoolItemCreator"/>.
        /// </summary>
        /// <param name="name">The name of the <see cref="ExcelPoolItemCreator"/>.</param>
        /// <param name="value">The (unique) <see cref="System.Guid"/> identifier of the <see cref="ExcelPoolItemCreator"/> with name <paramref name="name"/>.</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public static bool TryGetObjectTypeIdentifier(IdentifierString name, out Guid value)
        {
            OnInitialize();
            ExcelPoolItemCreator excelPoolItemCreator;
            if (sm_ObjectCreatorsByName.TryGetValue(name, out  excelPoolItemCreator) == true)
            {
                value = excelPoolItemCreator.ObjectType.Identifier;
                return true;
            }
            value = Guid.Empty;
            return false;
        }

        /// <summary>Gets the unique <see cref="IdentifierString"/> representation of a specific <see cref="ExcelPoolItemCreator"/>.
        /// </summary>
        /// <param name="identifier">The unique <see cref="System.Guid"/> identifier of the <see cref="ExcelPoolItemCreator"/>.</param>
        /// <param name="value">The (unique) <see cref="IdentifierString"/> representation of the <see cref="ExcelPoolItemCreator"/> with the unique <see cref="System.Guid"/> <paramref name="identifier"/>.</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public static bool TryGetObjectTypeName(Guid identifier, out IdentifierString value)
        {
            OnInitialize();
            ExcelPoolItemCreator excelPoolItemCreator;
            if (sm_ObjectCreatorsByGuid.TryGetValue(identifier, out  excelPoolItemCreator) == true)
            {
                value = excelPoolItemCreator.ObjectType.Name;
                return true;
            }
            value = null;
            return false;
        }
        #endregion

        #region private static methods

        /// <summary>Adds the specified <see cref="ExcelPoolItemCreator"/> object.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <exception cref="ArgumentException">Thrown, if a <see cref="ExcelPoolItemCreator"/> with the same (identifier) string or <see cref="System.Guid"/> representation
        /// has already been added.</exception>
        private static void Add(ExcelPoolItemCreator value)
        {
            Guid identifier = value.ObjectType.Identifier;
            if (sm_ObjectCreatorsByGuid.ContainsKey(identifier) == false)
            {
                if (sm_ObjectCreatorsByName.ContainsKey(value.Name) == false)
                {
                    sm_ObjectCreatorsByGuid.Add(identifier, value);
                    sm_ObjectCreatorsByName.Add(value);
                    return;
                }
            }
            throw new ArgumentException("Invalid 'ExcelPoolItemCreator' with name '" + value.Name.String + "' added. GUID or string representation is not unique.");
        }

        /// <summary>Raises the <see cref="Initialize"/> event.
        /// </summary>
        private static void OnInitialize()
        {
            if (sm_InitializeChanged == true)
            {
                /* first clear all ExcelPoolItemCreater objects */
                sm_ObjectCreatorsByGuid.Clear();
                sm_ObjectCreatorsByName = new IdentifierNameableDictionary<ExcelPoolItemCreator>(100, isReadOnlyExceptAdding: true);  // clear is not allowed, it is readonly

                /* add all ExcelPoolItemCreator objects */
                if (sm_Initialize != null)
                {
                    InitializeEventArgs eventArgs = new InitializeEventArgs();
                    sm_Initialize(eventArgs);
                }
                sm_InitializeChanged = false;
            }
        }

        /// <summary>Converts a single <see cref="ExcelPoolItem"/> object into a <see cref="IEnumerable&lt;ExcelPoolItem&gt;"/> object.
        /// </summary>
        /// <param name="excelPoolItem">The <see cref="ExcelPoolItem"/> object.</param>
        /// <returns>A <see cref="IEnumerable&lt;ExcelPoolItem&gt;"/> wrapper for <paramref name="excelPoolItem"/>.</returns>
        private static IEnumerable<ExcelPoolItem> GetAsEnumerable(ExcelPoolItem excelPoolItem)
        {
            yield return excelPoolItem;
        }
        #endregion
    }
}