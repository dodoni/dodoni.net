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
using System.Collections.Generic;

namespace Dodoni.BasicComponents.Containers
{
    /// <summary>Serves as covariant (read-only) interface for a dictionary, where the key is a <see cref="IdentifierString"/>.
    /// </summary>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <remarks>In .NET 4.0 out parameters are not allowed for covariant generic types, thus <c>TryGetValue</c> methods are implemented
    /// as some extension methods. Moreover out parameters are allowed for interfaces only.</remarks>
    public interface IIdentifierStringDictionary<out TValue> : IEnumerable<TValue>
    {
        /// <summary>Gets the number of key/value pairs contained in the <see cref="IIdentifierStringDictionary&lt;TValue&gt;"/>.
        /// </summary>
        /// <value>The number of key/value pairs.</value>
        int Count
        {
            get;
        }

        /// <summary>Gets the (non-normalized) names of the objects in the <see cref="IIdentifierStringDictionary&lt;TValue&gt;"/> instance in its <see cref="System.String"/> representation.
        /// </summary>
        /// <value>The names of the items in its <see cref="System.String"/> representation.</value>
        IEnumerable<string> Names
        {
            get;
        }

        /// <summary>Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <value>The value associated with the specified key. If the specified key is not found, a get operation
        /// throws a <see cref="KeyNotFoundException"/>, and a set operation creates a new element with the specified key.</value>
        TValue this[string key]
        {
            get;
        }

        /// <summary>Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <value>The value associated with the specified key. If the specified key is not found, a get operation
        /// throws a <see cref="KeyNotFoundException"/>, and a set operation creates a new element with the specified key.</value>
        TValue this[IdentifierString key]
        {
            get;
        }

        /// <summary>Gets the names of the objects in the <see cref="IIdentifierStringDictionary&lt;TValue&gt;"/> instance in its <see cref="IdentifierString"/> representation.
        /// </summary>
        /// <returns>The names of the items in its <see cref="IdentifierString"/> representation.</returns>
        IEnumerable<IdentifierString> GetNamesAsIdentifierStrings();

        /// <summary>Determines whether the <see cref="IIdentifierStringDictionary&lt;TValue&gt;"/> contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="IIdentifierStringDictionary&lt;TValue&gt;"/>.</param>
        /// <returns><c>true</c> if the <see cref="IIdentifierStringDictionary&lt;TValue&gt;"/> contains an element with 
        /// the	specified key; otherwise, <c>false</c>.
        /// </returns>
        bool ContainsKey(string key);

        /// <summary>Determines whether the <see cref="IIdentifierStringDictionary&lt;TValue&gt;"/> contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="IIdentifierStringDictionary&lt;TValue&gt;"/>.</param>
        /// <returns><c>true</c> if the <see cref="IIdentifierStringDictionary&lt;TValue&gt;"/> contains an element with 
        /// the	specified key; otherwise, <c>false</c>.
        /// </returns>
        bool ContainsKey(IdentifierString key);
    }
}