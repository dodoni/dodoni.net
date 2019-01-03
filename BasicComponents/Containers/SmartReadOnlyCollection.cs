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
using System.Collections.ObjectModel;

namespace Dodoni.BasicComponents.Containers
{
    /// <summary>Serves as wrapper for a generic read-only collection; similar to <see cref="ReadOnlyCollection&lt;T&gt;"/>. Additional features are an increment and 
    /// start offset for the encapsulated collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    public class SmartReadOnlyCollection<T> : IList<T>, ICollection<T>, IEnumerable<T>, IList, ICollection, IEnumerable
    {
        #region nested classes

        /// <summary>The implementation of the Enumerator.
        /// </summary>
        private class Enumerator : IEnumerator<T>, IEnumerator
        {
            #region private members

            private SmartReadOnlyCollection<T> m_ReadOnlyCollection;

            /// <summary>The current position in the list to wrap.
            /// </summary>
            private int m_CurrentIndex;

            /// <summary>The current number of the list, i.e. from 0 up to count -1. Initially set to -1.
            /// </summary>
            private int m_CurrentNumber;
            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="SmartReadOnlyCollection&lt;T&gt;.Enumerator"/> class.
            /// </summary>
            /// <param name="smartReadOnlyCollection">The smart read only collection.</param>
            internal Enumerator(SmartReadOnlyCollection<T> smartReadOnlyCollection)
            {
                m_ReadOnlyCollection = smartReadOnlyCollection;
                Reset();
            }
            #endregion

            #region IEnumerator<T> Members

            /// <summary>Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            /// <value>The element in the collection at the current position of the enumerator.</value>
            public T Current
            {
                get
                {
                    if (m_CurrentNumber < 0)
                    {
                        throw new InvalidOperationException();
                    }
                    return m_ReadOnlyCollection.m_List[m_CurrentIndex];
                }
            }
            #endregion

            #region IDisposable Members

            /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                // nothing to do here
            }
            #endregion

            #region IEnumerator Members

            /// <summary>Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            /// <value>The element in the collection at the current position of the enumerator.</value>
            object IEnumerator.Current
            {
                get
                {
                    if (m_CurrentNumber < 0)
                    {
                        throw new InvalidOperationException();
                    }
                    return m_ReadOnlyCollection.m_List[m_CurrentIndex];
                }
            }

            /// <summary>Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns><c>true</c> if the enumerator was successfully advanced to the next element; <c>false</c> if the enumerator has passed the end of the collection.
            /// </returns>
            /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public bool MoveNext()
            {
                m_CurrentIndex += m_ReadOnlyCollection.m_Increment;
                m_CurrentNumber++;
                return (m_CurrentNumber < m_ReadOnlyCollection.m_Count);
            }

            /// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
            public void Reset()
            {
                m_CurrentIndex = m_ReadOnlyCollection.m_StartIndex - m_ReadOnlyCollection.m_Increment;
                m_CurrentNumber = -1;
            }
            #endregion
        }
        #endregion

        #region private members
        
        /// <summary>The number of elements in the list.
        /// </summary>
        private int m_Count;

        /// <summary>The list to wrap.
        /// </summary>
        private IList<T> m_List;

        /// <summary>The null-based start index of the list to take into account.
        /// </summary>
        private int m_StartIndex;

        /// <summary>The increment of the list to take into account.
        /// </summary>
        private int m_Increment;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="SmartReadOnlyCollection&lt;T&gt;"/> class that is a read-only wrapper around the specified list.
        /// </summary>
        /// <param name="count">The number of elements of <paramref name="list"/> to take into account.</param>
        /// <param name="list">The list to wrap.</param>
        /// <param name="startIndex">The null-based start index of the <paramref name="list"/> to take into account.</param>
        /// <param name="increment">The increment of <paramref name="list"/> to take into account.</param>
        public SmartReadOnlyCollection(int count, IList<T> list, int startIndex = 0, int increment = 1)
        {
            m_List = list ?? throw new ArgumentNullException(nameof(list));
            m_Count = count;
            m_StartIndex = startIndex;
            m_Increment = increment;
        }
        #endregion

        #region IList<T> Members

        /// <summary>Gets or sets the element at the specified index.
        /// </summary>
        /// <value>The element at the specified index.</value>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown, if <paramref name="index"/> is not a valid index in the <see cref="IList&lt;T&gt;"/>.</exception>
        /// <exception cref="System.NotSupportedException">Thrown, if The property is set.</exception>
        public T this[int index]
        {
            get { return m_List[m_StartIndex + index * m_Increment]; }
            set { throw new NotSupportedException(); }
        }

        /// <summary>Determines the index of a specific item in the <see cref="IList&lt;T&gt;"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="IList&lt;T&gt;"/>.</param>
        /// <returns>The index of item if found in the list; otherwise, -1.</returns>
        public int IndexOf(T item)
        {
            int index = m_List.IndexOf(item);
            if (index < 0)
            {
                return -1;
            }

            int adjIndex = (index - m_StartIndex) / m_Increment;

            if ((adjIndex < 0) || ((index - m_StartIndex) % m_Increment != 0))
            {
                return -1;
            }
            return adjIndex;
        }

        /// <summary>Inserts an item to the <see cref="IList&lt;T&gt;"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="IList&lt;T&gt;"/>.</param>
        /// <exception cref="System.NotSupportedException">Thrown, if the <see cref="IList&lt;T&gt;"/> is read-only.</exception>
        public void Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        /// <summary>Removes the <see cref="IList&lt;T&gt;"/> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="System.NotSupportedException">Thrown, if the <see cref="IList&lt;T&gt;"/> is read-only.</exception>
        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }
        #endregion

        #region ICollection<T> Members

        /// <summary>Adds an item to the <see cref="ICollection&lt;T&gt;"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="ICollection&lt;T&gt;"/>.</param>
        /// <exception cref="System.NotSupportedException">Thrown, if the <see cref="ICollection&lt;T&gt;"/> is read-only.</exception>
        public void Add(T item)
        {
            throw new NotSupportedException();
        }

        /// <summary>Removes all items from the <see cref="ICollection&lt;T&gt;"/>.
        /// </summary>
        /// <exception cref="System.NotSupportedException">Thrown, if the <see cref="ICollection&lt;T&gt;"/> is read-only.</exception>
        public void Clear()
        {
            throw new NotSupportedException();
        }

        /// <summary> Determines whether the <see cref="ICollection&lt;T&gt;"/> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="ICollection&lt;T&gt;"/>.</param>
        /// <returns><c>true</c> if <paramref name="item"/> is found in the <see cref="ICollection&lt;T&gt;"/>; otherwise, <c>false</c>.</returns>
        public bool Contains(T item)
        {
            return (IndexOf(item) >= 0);
        }

        /// <summary>Copies the elements of the <see cref="ICollection&lt;T&gt;"/> to an <see cref="System.Array"/>, starting at a particular <see cref="System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="System.Array"/> that is the destination of the elements copied from <see cref="ICollection&lt;T&gt;"/>. 
        /// The <see cref="System.Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            int index = m_StartIndex;
            for (int k = 0; k < m_Count; k++)
            {
                array[arrayIndex + k] = m_List[index];
                index += m_Increment;
            }
        }

        /// <summary>Gets the number of elements contained in the <see cref="ICollection&lt;T&gt;"/>.
        /// </summary>
        /// <value>The number of elements contained in the <see cref="ICollection&lt;T&gt;"/>.</value>
        public int Count
        {
            get { return m_Count; }
        }

        /// <summary>Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
        public bool IsReadOnly
        {
            get { return true; }
        }

        /// <summary>Removes the first occurrence of a specific object from the <see cref="ICollection&lt;T&gt;"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="ICollection&lt;T&gt;"/>.</param>
        /// <returns><c>true</c> if <paramref name="item"/> was successfully removed from the <see cref="ICollection&lt;T&gt;"/>;
        /// otherwise, <c>false</c>. This method also returns <c>false</c> if <paramref name="item"/> is not found in the original <see cref="ICollection&lt;T&gt;"/>.
        /// </returns>
        /// <exception cref="System.NotSupportedException">Thrown, if the <see cref="ICollection&lt;T&gt;"/> is read-only.</exception>
        public bool Remove(T item)
        {
            throw new NotSupportedException();
        }
        #endregion

        #region IEnumerable<T> Members

        /// <summary>Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="IEnumerator&lt;T&gt;"/> that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }
        #endregion

        #region IEnumerable Members

        /// <summary>Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }
        #endregion

        #region IList Members

        /// <summary>Adds an item to the <see cref="T:System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The object to add to the <see cref="T:System.Collections.IList"/>.</param>
        /// <returns>The position into which the new element was inserted, or -1 to indicate that the item was not inserted into the collection.</returns>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only -or- the <see cref="T:System.Collections.IList"/> has a fixed size.</exception>
        public int Add(object value)
        {
            throw new NotSupportedException();
        }

        /// <summary>Determines whether the <see cref="T:System.Collections.IList"/> contains a specific value.
        /// </summary>
        /// <param name="value">The object to locate in the <see cref="T:System.Collections.IList"/>.</param>
        /// <returns><c>true</c> if the <see cref="T:System.Object"/> is found in the <see cref="T:System.Collections.IList"/>; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(object value)
        {
            return (IndexOf(value) >= 0);
        }

        /// <summary>Determines the index of a specific item in the <see cref="T:System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The object to locate in the <see cref="T:System.Collections.IList"/>.</param>
        /// <returns>The index of <paramref name="value"/> if found in the list; otherwise, -1.</returns>
        public int IndexOf(object value)
        {
            if ((value is T) == false)
            {
                return -1;
            }
            int index = m_List.IndexOf((T)value);
            if (index < 0)
            {
                return -1;
            }

            int adjIndex = (index - m_StartIndex) / m_Increment;

            if ((adjIndex < 0) || ((index - m_StartIndex) % m_Increment != 0))
            {
                return -1;
            }
            return adjIndex;
        }

        /// <summary>Inserts an item to the <see cref="T:System.Collections.IList"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="value"/> should be inserted.</param>
        /// <param name="value">The object to insert into the <see cref="T:System.Collections.IList"/>.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.IList"/>.</exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only -or- the <see cref="T:System.Collections.IList"/> has a fixed size.</exception>
        /// <exception cref="T:System.NullReferenceException"><paramref name="value"/> is null reference in the <see cref="T:System.Collections.IList"/>.</exception>
        public void Insert(int index, object value)
        {
            throw new NotSupportedException();
        }

        /// <summary>Gets a value indicating whether the <see cref="T:System.Collections.IList"/> has a fixed size.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Collections.IList"/> has a fixed size; otherwise, false.</returns>
        public bool IsFixedSize
        {
            get { return true; }
        }

        /// <summary>Removes the first occurrence of a specific object from the <see cref="T:System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The object to remove from the <see cref="T:System.Collections.IList"/>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception>
        public void Remove(object value)
        {
            throw new NotSupportedException();
        }

        /// <summary>Gets or sets the element at the specified index.
        /// </summary>
        /// <value>The element at the specified index.</value>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown, if <paramref name="index"/> is not a valid index in the <see cref="IList&lt;T&gt;"/>.</exception>
        /// <exception cref="System.NotSupportedException">Thrown, if The property is set.</exception>
        object IList.this[int index]
        {
            get { return m_List[m_StartIndex + index * m_Increment]; }
            set { throw new NotSupportedException(); }
        }
        #endregion

        #region ICollection Members

        /// <summary>Copies the elements of the <see cref="T:System.Collections.ICollection"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than zero.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="array"/> is multidimensional -or- the number of elements in the source <see cref="T:System.Collections.ICollection"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.</exception>
        /// <exception cref="T:System.ArgumentException">The type of the source <see cref="T:System.Collections.ICollection"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.</exception>
        public void CopyTo(Array array, int arrayIndex)
        {
            if ((array is T[]) == false)
            {
                throw new ArgumentException("array");
            }
            int index = m_StartIndex;
            var castArray = (T[])array;

            for (int k = 0; k < m_Count; k++)
            {
                castArray[arrayIndex + k] = m_List[index];
                index += m_Increment;
            }
        }

        /// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe).
        /// </summary>
        /// <value></value>
        /// <returns><c>true</c> if access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe); otherwise, <c>false</c>.</returns>
        public bool IsSynchronized
        {
            get { return false; }
        }

        /// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.
        /// </summary>
        /// <value></value>
        /// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.</returns>
        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }
        #endregion
    }
}