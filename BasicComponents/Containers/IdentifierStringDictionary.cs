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
using System.Collections.Generic;

namespace Dodoni.BasicComponents.Containers
{
    /// <summary>Represents a collection of keys and values, where the key is a <see cref="IdentifierString"/>.
    /// </summary>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    public class IdentifierStringDictionary<TValue> : IdentifierStringDictionaryBase<TValue>
    {
        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="IdentifierStringDictionary&lt;TValue&gt;"/> class.
        /// </summary>
        /// <param name="isReadOnlyExceptAdding">A value that determines if the dictionary is readonly (except adding new items); if <c>true</c> new items can be added, overwriting or removing is not allowed; otherwise there are not restrictions.</param>
        public IdentifierStringDictionary(bool isReadOnlyExceptAdding = true)
            : base(isReadOnlyExceptAdding)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="IdentifierStringDictionary&lt;TValue&gt;"/> class.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="IdentifierStringDictionary&lt;TValue&gt;"/> can contain.</param>
        /// <param name="isReadOnlyExceptAdding">A value that determines if the dictionary is readonly (except adding new items); if <c>true</c> new items can be added, overwriting or removing is not allowed; otherwise there are not restrictions.</param>
        public IdentifierStringDictionary(int capacity, bool isReadOnlyExceptAdding = true)
            : base(capacity, isReadOnlyExceptAdding)
        {
        }
        #endregion

        #region public methods

        /// <summary>Returns a read-only <see cref="IdentifierStringDictionaryBase&lt;TValue&gt;"/> wrapper for the current dictionary.
        /// </summary>
        /// <returns></returns>
        public IdentifierStringDictionaryBase<TValue> AsReadOnly()
        {
            return base.GetDeepCopy();
        }

        /// <summary>Adds the specified key and value to the dictionary.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can be null for reference types.</param>
        /// <returns>A value indicating whether <paramref name="value"/> has been inserted.</returns>
        public new ItemAddedState Add(string key, TValue value)
        {
            return base.Add(key, value);
        }

        /// <summary>Adds the specified key and value to the dictionary.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can be null for reference types.</param>
        /// <returns>A value indicating whether <paramref name="value"/> has been inserted.</returns>
        public new ItemAddedState Add(IdentifierString key, TValue value)
        {
            return base.Add(key, value);
        }

        /// <summary>Removes all keys and values from the <see cref="IdentifierStringDictionary&lt;TValue&gt;"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown, if this operation is not allowed.</exception>
        public new void Clear()
        {
            base.Clear();
        }

        /// <summary>Removes the value with the specified key from the <see cref="IdentifierStringDictionary&lt;TValue&gt;"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns><c>true</c> if the element is sucessfully found and removed; otherwise, <c>false</c>. This method
        /// returns <c>false</c> if <paramref name="key"/> is not found in the <see cref="IdentifierStringDictionary&lt;TValue&gt;"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if this operation is not allowed.</exception>
        public new bool Remove(string key)
        {
            return base.Remove(key);
        }

        /// <summary>Removes the value with the specified key from the <see cref="IdentifierStringDictionary&lt;TValue&gt;"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns><c>true</c> if the element is sucessfully found and removed; otherwise, <c>false</c>. This method
        /// returns <c>false</c> if <paramref name="key"/> is not found in the <see cref="IdentifierStringDictionary&lt;TValue&gt;"/>.</returns>
        public new bool Remove(IdentifierString key)
        {
            return base.Remove(key);
        }
        #endregion
    }

    /// <summary>Provides static methods for creating <see cref="IdentifierStringDictionary&lt;TValue&gt;"/> objects.
    /// </summary>
    public static class IdentifierStringDictionary
    {
        #region factory methods

        /// <summary>Creates a new <see cref="IdentifierStringDictionary&lt;TValue&gt;"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="name">The name of the <paramref name="value"/>.</param>
        /// <param name="value">The value.</param>
        /// <returns>A <see cref="IdentifierStringDictionary&lt;TValue&gt;"/> that contains <paramref name="value"/>.</returns>
        public static IdentifierStringDictionary<TValue> Create<TValue>(IdentifierString name, TValue value)
        {
            var dictionary = new IdentifierStringDictionary<TValue>
            {
                { name, value }
            };
            return dictionary;
        }

        /// <summary>Creates a new <see cref="IdentifierStringDictionary&lt;TValue&gt;"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="name">The name of the <paramref name="value"/>.</param>
        /// <param name="value">The value.</param>
        /// <returns>A <see cref="IdentifierStringDictionary&lt;TValue&gt;"/> that contains <paramref name="value"/>.</returns>
        public static IdentifierStringDictionary<TValue> Create<TValue>(string name, TValue value)
        {
            var dictionary = new IdentifierStringDictionary<TValue>
            {
                { name, value }
            };
            return dictionary;
        }

        /// <summary>Creates a new <see cref="IdentifierStringDictionary&lt;TValue&gt;"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="name1">The name of <paramref name="value1"/>.</param>
        /// <param name="value1">The first value.</param>
        /// <param name="name2">The name of <paramref name="value2"/>.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>A <see cref="IdentifierStringDictionary&lt;TValue&gt;"/> that contains <paramref name="value1"/> and <paramref name="value2"/>.</returns>
        public static IdentifierStringDictionary<TValue> Create<TValue>(IdentifierString name1, TValue value1, IdentifierString name2, TValue value2)
        {
            var dictionary = new IdentifierStringDictionary<TValue>
            {
                { name1, value1 },
                { name2, value2 }
            };
            return dictionary;
        }

        /// <summary>Creates a new <see cref="IdentifierStringDictionary&lt;TValue&gt;"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="name1">The name of <paramref name="value1"/>.</param>
        /// <param name="value1">The first value.</param>
        /// <param name="name2">The name of <paramref name="value2"/>.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>A <see cref="IdentifierStringDictionary&lt;TValue&gt;"/> that contains <paramref name="value1"/> and <paramref name="value2"/>.</returns>
        public static IdentifierStringDictionary<TValue> Create<TValue>(string name1, TValue value1, string name2, TValue value2)
        {
            var dictionary = new IdentifierStringDictionary<TValue>
            {
                { name1, value1 },
                { name2, value2 }
            };
            return dictionary;
        }

        /// <summary>Creates a new <see cref="IdentifierStringDictionary&lt;TValue&gt;"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="name1">The name of <paramref name="value1"/>.</param>
        /// <param name="value1">The first value.</param>
        /// <param name="name2">The name of <paramref name="value2"/>.</param>
        /// <param name="value2">The second value.</param>
        /// <param name="name3">The name of <paramref name="value3"/>.</param>
        /// <param name="value3">The third value.</param>
        /// <returns>A <see cref="IdentifierStringDictionary&lt;TValue&gt;"/> that contains <paramref name="value1"/>, <paramref name="value2"/> and <paramref name="value3"/>.</returns>
        public static IdentifierStringDictionary<TValue> Create<TValue>(IdentifierString name1, TValue value1, IdentifierString name2, TValue value2, IdentifierString name3, TValue value3)
        {
            var dictionary = new IdentifierStringDictionary<TValue>
            {
                { name1, value1 },
                { name2, value2 },
                { name3, value2 }
            };
            return dictionary;
        }

        /// <summary>Creates a new <see cref="IdentifierStringDictionary&lt;TValue&gt;"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="name1">The name of <paramref name="value1"/>.</param>
        /// <param name="value1">The first value.</param>
        /// <param name="name2">The name of <paramref name="value2"/>.</param>
        /// <param name="value2">The second value.</param>
        /// <param name="name3">The name of <paramref name="value3"/>.</param>
        /// <param name="value3">The third value.</param>
        /// <returns>A <see cref="IdentifierStringDictionary&lt;TValue&gt;"/> that contains <paramref name="value1"/>, <paramref name="value2"/> and <paramref name="value3"/>.</returns>
        public static IdentifierStringDictionary<TValue> Create<TValue>(string name1, TValue value1, string name2, TValue value2, string name3, TValue value3)
        {
            var dictionary = new IdentifierStringDictionary<TValue>
            {
                { name1, value1 },
                { name2, value2 },
                { name3, value2 }
            };
            return dictionary;
        }

        /// <summary>Creates a new <see cref="IdentifierStringDictionary&lt;TValue&gt;"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="values">The values to add.</param>
        /// <returns>A <see cref="IdentifierStringDictionary&lt;TValue&gt;"/> that contains the elements of <paramref name="values"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="values"/> is <c>null</c>.</exception>
        public static IdentifierStringDictionary<TValue> Create<TValue>(params Tuple<string, TValue>[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }
            var dictionary = new IdentifierStringDictionary<TValue>();
            foreach (var data in values)
            {
                dictionary.Add(data.Item1, data.Item2);
            }
            return dictionary;
        }

        /// <summary>Creates a new <see cref="IdentifierStringDictionary&lt;TValue&gt;"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="values">The values to add.</param>
        /// <returns>A <see cref="IdentifierStringDictionary&lt;TValue&gt;"/> that contains the elements of <paramref name="values"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="values"/> is <c>null</c>.</exception>
        public static IdentifierStringDictionary<TValue> Create<TValue>(params Tuple<IdentifierString, TValue>[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }
            var dictionary = new IdentifierStringDictionary<TValue>();
            foreach (var data in values)
            {
                dictionary.Add(data.Item1, data.Item2);
            }
            return dictionary;
        }
        #endregion

        #region extension methods

        /// <summary>Creates a <see cref="IdentifierStringDictionary&lt;TValue&gt;"/> from a <see cref="IEnumerable&lt;TValue&gt;"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the elements of source.</typeparam>
        /// <param name="source">An <see cref="IEnumerable&lt;T&gt;"/> to create a <see cref="IdentifierStringDictionaryBase&lt;TValue&gt;"/> from.</param>
        /// <param name="isReadOnlyExceptAdding">A value that determines if the dictionary is readonly (except adding new items); if <c>true</c> new items can be added, overwriting or removing is not allowed; otherwise there are not restrictions.</param>
        /// <returns>A <see cref="IdentifierStringDictionary&lt;T&gt;"/> that contains the elements from the input sequence.</returns>
        public static IdentifierStringDictionary<TValue> ToIdStringDictionary<TValue>(this IEnumerable<Tuple<IdentifierString, TValue>> source, bool isReadOnlyExceptAdding = false)
        {
            var dictionary = new IdentifierStringDictionary<TValue>(isReadOnlyExceptAdding);
            foreach (var data in source)
            {
                dictionary.Add(data.Item1, data.Item2);
            }
            return dictionary;
        }

        /// <summary>Gets the value associated with the specified key.
        /// </summary>
        /// <param name="idStringDictionary">The <see cref="IIdentifierStringDictionary&lt;TValue&gt;"/>.</param>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">When this method return, contains the value associated with the specified key, if the
        /// key is found; otherwise, the default value for the type of the value parameter. This parameter 
        /// is passed uninitialized.</param>
        /// <returns><c>true</c> if the <see cref="IIdentifierStringDictionary&lt;TValue&gt;"/> contains an element with the specified key; otherwise <c>false</c>.</returns>
        /// <remarks>In .NET 4.0 out parameters are not allowed for covariant generic types, therefore it is not possible
        /// to add this method to the <see cref="IIdentifierStringDictionary&lt;TValue&gt;"/> class. This is a workaround.</remarks>
        public static bool TryGetValue<TValue>(this IIdentifierStringDictionary<TValue> idStringDictionary, string key, out TValue value)
        {
            if (idStringDictionary.ContainsKey(key) == false)
            {
                value = default;
                return false;
            }
            value = idStringDictionary[key];
            return true;
        }

        /// <summary>Gets the value associated with the specified key.
        /// </summary>
        /// <param name="idStringDictionary">The <see cref="IIdentifierStringDictionary&lt;TValue&gt;"/>.</param>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">When this method return, contains the value associated with the specified key, if the
        /// key is found; otherwise, the default value for the type of the value parameter. This parameter 
        /// is passed uninitialized.</param>
        /// <returns><c>true</c> if the <see cref="IIdentifierStringDictionary&lt;TValue&gt;"/> contains an element with the specified key; otherwise <c>false</c>.</returns>
        /// <remarks>In .NET 4.0 out parameters are not allowed for covariant generic types, therefore it is not possible
        /// to add this method to the <see cref="IIdentifierStringDictionary&lt;TValue&gt;"/> class. This is a workaround.</remarks>
        public static bool TryGetValue<TValue>(this IIdentifierStringDictionary<TValue> idStringDictionary, IdentifierString key, out TValue value)
        {
            if (idStringDictionary.ContainsKey(key) == false)
            {
                value = default;
                return false;
            }
            value = idStringDictionary[key];
            return true;
        }
        #endregion
    }
}