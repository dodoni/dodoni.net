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
using System.Collections;
using System.Collections.Generic;

namespace Dodoni.BasicComponents.Utilities
{
    /// <summary>Serves as straight forward implementation for <see cref="System.Linq.IGrouping{TKey,TElement}"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the key of the <see cref="System.Linq.IGrouping{TKey,TElement}"/>. This type parameter is covariant. That is, you can 
    /// use either the type you specified or any type that is more derived.</typeparam>
    /// <typeparam name="TElement">The type of the values in the <see cref="System.Linq.IGrouping{TKey,TElement}"/></typeparam>
    public class Grouping<TKey, TElement> : IGrouping<TKey, TElement>
    {
        #region private members

        /// <summary>The internal collection of values.
        /// </summary>
        private IEnumerable<TElement> m_Values;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="Grouping{TKey,TElement}" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="values">The values.</param>
        public Grouping(TKey key, IEnumerable<TElement> values)
        {
            Key = key;
            m_Values = values ?? throw new ArgumentNullException("values");
        }
        #endregion

        #region public properties

        /// <summary>Gets the key of the <see cref="System.Linq.IGrouping{TKey,TElement}"/>.
        /// </summary>
        /// <value>The key of the <see cref="System.Linq.IGrouping{TKey,TElement}"/>.</value>
        public TKey Key
        {
            get;
            private set;
        }
        #endregion

        #region public methods

        /// <summary>Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator{TElement}" /> object that can be used to iterate through the collection.</returns>
        public IEnumerator<TElement> GetEnumerator()
        {
            return m_Values.GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        #endregion
    }

    /// <summary>Serves as factory for <see cref="System.Linq.IGrouping{TKey,TElement}"/> objects.
    /// </summary>
    public static class Grouping
    {
        /// <summary>Creates a specific <see cref="System.Linq.IGrouping{TKey,TElement}"/> object.
        /// </summary>
        /// <typeparam name="TKey">The type of the key of the <see cref="System.Linq.IGrouping{TKey,TElement}"/>. This type parameter is covariant. That is, you can 
        /// use either the type you specified or any type that is more derived.</typeparam>
        /// <typeparam name="TElement">The type of the values in the <see cref="System.Linq.IGrouping{TKey,TElement}"/></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="values">The values.</param>
        /// <returns>The specified <see cref="System.Linq.IGrouping{TKey,TElement}"/> object.</returns>
        public static IGrouping<TKey, TElement> Create<TKey, TElement>(TKey key, IEnumerable<TElement> values)
        {
            return new Grouping<TKey, TElement>(key, values);
        }
    }
}