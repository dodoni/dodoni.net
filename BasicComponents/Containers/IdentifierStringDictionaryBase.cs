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
using System.Collections;
using System.Collections.Generic;

namespace Dodoni.BasicComponents.Containers
{
    /// <summary>Serves as base class for a collection of keys and values, where the key is a <see cref="IdentifierString"/>.
    /// </summary>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    public class IdentifierStringDictionaryBase<TValue> : IIdentifierStringDictionary<TValue>
    {
        #region private members

        /// <summary>The encapsulated dictionary, where the key is the identifier string representation and the value contains the objects.
        /// </summary>
        private Dictionary<IdentifierString, TValue> m_Dictionary = new Dictionary<IdentifierString, TValue>();

        /// <summary>A value that determines if the dictionary is readonly (except adding new items); if <c>true</c> new items can be added, overwriting or removing is not allowed; otherwise there are not restrictions.
        /// </summary>
        protected readonly bool IsReadOnlyExceptAdding;
        #endregion

        #region private constructors

        /// <summary>Initializes a new instance of the <see cref="IdentifierStringDictionaryBase&lt;TValue&gt;"/> class.
        /// </summary>
        /// <param name="identifierStringDictionaryBase">The identifier string dictionary base.</param>
        /// <remarks>This copy constructor creats a deep copy of the argument.</remarks>
        private IdentifierStringDictionaryBase(IdentifierStringDictionaryBase<TValue> identifierStringDictionaryBase)
        {
            if (identifierStringDictionaryBase == null)
            {
                throw new ArgumentNullException(nameof(identifierStringDictionaryBase));
            }
            IsReadOnlyExceptAdding = identifierStringDictionaryBase.IsReadOnlyExceptAdding;
            m_Dictionary = new Dictionary<IdentifierString, TValue>(identifierStringDictionaryBase.m_Dictionary);
        }
        #endregion

        #region protected constructors

        /// <summary>Initializes a new instance of the <see cref="IdentifierStringDictionaryBase&lt;TValue&gt;"/> class.
        /// </summary>
        /// <param name="isReadOnlyExceptAdding">A value that determines if the dictionary is readonly (except adding new items); if <c>true</c> new items can be added, overwriting or removing is not allowed; otherwise there are not restrictions.</param>
        protected IdentifierStringDictionaryBase(bool isReadOnlyExceptAdding = false)
        {
            IsReadOnlyExceptAdding = isReadOnlyExceptAdding;
        }

        /// <summary>Initializes a new instance of the <see cref="IdentifierStringDictionaryBase&lt;TValue&gt;"/> class.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="IdentifierStringDictionaryBase&lt;TValue&gt;"/> can contain.</param>
        /// <param name="isReadOnlyExceptAdding">A value that determines if the dictionary is readonly (except adding new items); if <c>true</c> new items can be added, overwriting or removing is not allowed; otherwise there are not restrictions.</param>
        protected IdentifierStringDictionaryBase(int capacity, bool isReadOnlyExceptAdding = false)
        {
            IsReadOnlyExceptAdding = isReadOnlyExceptAdding;
        }
        #endregion

        #region public properties

        #region IIdentifierStringDictionary<TValue> Members

        /// <summary>Gets the number of key/value pairs contained in the <see cref="IIdentifierStringDictionary&lt;TValue&gt;"/>.
        /// </summary>
        /// <value>The number of key/value pairs.</value>
        public int Count
        {
            get { return m_Dictionary.Count; }
        }

        /// <summary>Gets the (non-normalized) names of the objects in the <see cref="IIdentifierStringDictionary&lt;TValue&gt;"/> instance in its <see cref="System.String"/> representation.
        /// </summary>
        /// <value>The names of the items in its <see cref="System.String"/> representation.</value>
        public IEnumerable<string> Names
        {
            get
            {
                foreach (var keyValuePair in m_Dictionary)
                {
                    yield return keyValuePair.Key.String;
                }
            }
        }

        /// <summary>Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <value>The value associated with the specified key. If the specified key is not found, a get operation
        /// throws a <see cref="KeyNotFoundException"/>, and a set operation creates a new element with the specified key.</value>
        public TValue this[string key]
        {
            get
            {
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }
                return m_Dictionary[key.ToIdentifierString()];
            }
            set
            {
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }
                if (IsReadOnlyExceptAdding == false)
                {
                    IdentifierString idKey = key.ToIdentifierString();
                    m_Dictionary[idKey] = value;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        /// <summary>Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <value>The value associated with the specified key. If the specified key is not found, a get operation
        /// throws a <see cref="KeyNotFoundException"/>, and a set operation creates a new element with the specified key.</value>
        public TValue this[IdentifierString key]
        {
            get
            {
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }
                return m_Dictionary[key];
            }
            set
            {
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }
                if (IsReadOnlyExceptAdding == false)
                {
                    m_Dictionary[key] = value;
                }
            }
        }
        #endregion

        /// <summary>Gets a collection of the values and the corresponding name in its <see cref="IdentifierString"/> representation.
        /// </summary>
        /// <value>The item values and names.</value>
        public IEnumerable<Tuple<IdentifierString, TValue>> NamedValues
        {
            get
            {
                foreach (var keyValuePair in m_Dictionary)
                {
                    yield return Tuple.Create(keyValuePair.Key, keyValuePair.Value);
                }
            }
        }

        /// <summary>Gets a collection containing the values in the <see cref="IdentifierStringDictionaryBase&lt;TValue&gt;"/>.
        /// </summary>
        /// <value>A collection containing the values.</value>
        public IEnumerable<TValue> Values
        {
            get { return m_Dictionary.Values; }
        }
        #endregion

        #region public methods

        #region IEnumerable<TValue> Member

        /// <summary>Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<TValue> GetEnumerator()
        {
            foreach (var value in m_Dictionary.Values)
            {
                yield return value;
            }
        }
        #endregion

        #region IEnumerable Member

        /// <summary>Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_Dictionary.Values.GetEnumerator();
        }
        #endregion

        #region IIdentifierStringDictionary<TValue> Members

        /// <summary>Gets the names of the objects in the <see cref="IIdentifierStringDictionary&lt;TValue&gt;"/> instance in its <see cref="IdentifierString"/> representation.
        /// </summary>
        /// <returns>The names of the items in its <see cref="IdentifierString"/> representation.</returns>
        public IEnumerable<IdentifierString> GetNamesAsIdentifierStrings()
        {
            return m_Dictionary.Keys;
        }

        /// <summary>Determines whether the <see cref="IIdentifierStringDictionary&lt;TValue&gt;"/> contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="IIdentifierStringDictionary&lt;TValue&gt;"/>.</param>
        /// <returns><c>true</c> if the <see cref="IIdentifierStringDictionary&lt;TValue&gt;"/> contains an element with the specified key; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsKey(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return m_Dictionary.ContainsKey(key.ToIdentifierString());
        }

        /// <summary>Determines whether the <see cref="IIdentifierStringDictionary&lt;TValue&gt;"/> contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="IIdentifierStringDictionary&lt;TValue&gt;"/>.</param>
        /// <returns><c>true</c> if the <see cref="IIdentifierStringDictionary&lt;TValue&gt;"/> contains an element with the specified key; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsKey(IdentifierString key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return m_Dictionary.ContainsKey(key);
        }
        #endregion

        /// <summary>Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">When this method return, contains the value associated with the specified key, if the
        /// key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
        /// <returns><c>true</c> if the <see cref="IdentifierStringDictionaryBase&lt;TValue&gt;"/> contains an element with the specified key; otherwise <c>false</c>.</returns>
        public bool TryGetValue(string key, out TValue value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return m_Dictionary.TryGetValue(key.ToIdentifierString(), out value);
        }

        /// <summary>Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">When this method return, contains the value associated with the specified key, if the
        /// key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
        /// <returns><c>true</c> if the <see cref="IdentifierStringDictionaryBase&lt;TValue&gt;"/> contains an element with the specified key; otherwise <c>false</c>.</returns>
        public bool TryGetValue(IdentifierString key, out TValue value)
        {
            return m_Dictionary.TryGetValue(key, out value);
        }
        #endregion

        #region protected methods

        /// <summary>Adds the specified key and value to the dictionary.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can be null for reference types.</param>
        /// <returns>A value indicating whether <paramref name="value"/> has been inserted.</returns>
        protected ItemAddedState Add(string key, TValue value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            IdentifierString idKey = key.ToIdentifierString();

            if (m_Dictionary.ContainsKey(idKey) == false)
            {
                m_Dictionary.Add(idKey, value);
                return ItemAddedState.Added;
            }
            else if (IsReadOnlyExceptAdding == true)
            {
                return ItemAddedState.Rejected;
            }
            m_Dictionary[idKey] = value;
            return ItemAddedState.Replaced;
        }

        /// <summary>Adds the specified key and value to the dictionary.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can be null for reference types.</param>
        /// <returns>A value indicating whether <paramref name="value"/> has been inserted.</returns>
        protected ItemAddedState Add(IdentifierString key, TValue value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (m_Dictionary.ContainsKey(key) == false)
            {
                m_Dictionary.Add(key, value);
                return ItemAddedState.Added;
            }
            else if (IsReadOnlyExceptAdding == true)
            {
                return ItemAddedState.Rejected;
            }
            m_Dictionary[key] = value;
            return ItemAddedState.Replaced;
        }

        /// <summary>Removes all keys and values from the <see cref="IdentifierStringDictionaryBase&lt;TValue&gt;"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown, if this operation is not allowed.</exception>
        protected void Clear()
        {
            if (IsReadOnlyExceptAdding == true)
            {
                throw new InvalidOperationException();
            }
            m_Dictionary.Clear();
        }

        /// <summary>Removes the value with the specified key from the <see cref="IdentifierStringDictionaryBase&lt;TValue&gt;"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns><c>true</c> if the element is sucessfully found and removed; otherwise, <c>false</c>. This method
        /// returns <c>false</c> if <paramref name="key"/> is not found in the <see cref="IdentifierStringDictionaryBase&lt;TValue&gt;"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if this operation is not allowed.</exception>
        protected bool Remove(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (IsReadOnlyExceptAdding == true)
            {
                throw new InvalidOperationException();
            }
            return m_Dictionary.Remove(key.ToIdentifierString());
        }

        /// <summary>Removes the value with the specified key from the <see cref="IdentifierStringDictionaryBase&lt;TValue&gt;"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns><c>true</c> if the element is sucessfully found and removed; otherwise, <c>false</c>. This method
        /// returns <c>false</c> if <paramref name="key"/> is not found in the <see cref="IdentifierStringDictionaryBase&lt;TValue&gt;"/>.</returns>
        protected bool Remove(IdentifierString key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (IsReadOnlyExceptAdding == true)
            {
                throw new InvalidOperationException();
            }
            return m_Dictionary.Remove(key);
        }

        /// <summary>Gets a deep copy of the current instance.
        /// </summary>
        /// <returns>A deep copy of the current instance.</returns>
        virtual protected IdentifierStringDictionaryBase<TValue> GetDeepCopy()
        {
            return new IdentifierStringDictionaryBase<TValue>(this);
        }
        #endregion
    }
}