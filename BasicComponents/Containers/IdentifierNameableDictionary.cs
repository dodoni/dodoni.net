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
    /// <summary>The delegate for the <see cref="IdentifierNameableDictionary&lt;TValue&gt;.Adding"/> event handling.
    /// </summary>
    /// <param name="sender">The sender, i.e. the <see cref="IdentifierNameableDictionary&lt;TValue&gt;"/> instance.</param>
    /// <param name="eventArgs">The event arguments.</param>
    public delegate void ItemAddingEventHandler(object sender, ItemAddingEventArgs eventArgs);

    /// <summary>The delegate for the <see cref="IdentifierNameableDictionary&lt;TValue&gt;.Added"/> event handling.
    /// </summary>
    /// <param name="sender">The sender, i.e. the <see cref="IdentifierNameableDictionary&lt;TValue&gt;"/> instance.</param>
    /// <param name="eventArgs">The event arguments.</param>
    public delegate void ItemAddedEventHandler(object sender, ItemAddedEventArgs eventArgs);

    /// <summary>Represent a dictionary for nameable items.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying elements.</typeparam>
    public class IdentifierNameableDictionary<TValue> : IdentifierStringDictionaryBase<TValue>
        where TValue : IIdentifierNameable
    {
        #region public members

        /// <summary>The event handler which will be raise before adding some new element into the pool.
        /// </summary>
        public event ItemAddingEventHandler Adding;

        /// <summary>The event handler which will be raise after adding some new element into the pool.
        /// </summary>
        public event ItemAddedEventHandler Added;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="IdentifierNameableDictionary&lt;TValue&gt;"/> class.
        /// </summary>
        /// <param name="isReadOnlyExceptAdding">A value that determines if the dictionary is readonly (except adding new items); if <c>true</c> new items can be added, overwriting or removing is not allowed; otherwise there are not restrictions.</param>
        public IdentifierNameableDictionary(bool isReadOnlyExceptAdding = true)
            : base(isReadOnlyExceptAdding)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="IdentifierNameableDictionary&lt;TValue&gt;"/> class.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="IdentifierNameableDictionary&lt;TValue&gt;"/> can contain.</param>
        /// <param name="isReadOnlyExceptAdding">A value that determines if the dictionary is readonly (except adding new items); if <c>true</c> new items can be added, overwriting or removing is not allowed; otherwise there are not restrictions.</param>
        public IdentifierNameableDictionary(int capacity, bool isReadOnlyExceptAdding = true)
            : base(capacity, isReadOnlyExceptAdding)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="IdentifierNameableDictionary&lt;TValue&gt;"/> class.
        /// </summary>
        /// <param name="value">The value to insert into the pool.</param>
        /// <param name="isReadOnlyExceptAdding">A value that determines if the dictionary is readonly (except adding new items); if <c>true</c> new items can be added, overwriting or removing is not allowed; otherwise there are not restrictions.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="value"/> is <c>null</c>.</exception>
        public IdentifierNameableDictionary(TValue value, bool isReadOnlyExceptAdding = true)
            : base(isReadOnlyExceptAdding)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            base.Add(value.Name, value);
        }

        /// <summary>Initializes a new instance of the <see cref="IdentifierNameableDictionary&lt;TValue&gt;"/> class.
        /// </summary>
        /// <param name="values">The values to insert into the pool.</param>
        /// <param name="isReadOnlyExceptAdding">A value that determines if the dictionary is readonly (except adding new items); if <c>true</c> new items can be added, overwriting or removing is not allowed; otherwise there are not restrictions.</param>
        /// <exception cref="NullReferenceException">Thrown, if one item of <paramref name="values"/> is <c>null</c>.</exception>
        public IdentifierNameableDictionary(IEnumerable<TValue> values, bool isReadOnlyExceptAdding = true)
            : base(isReadOnlyExceptAdding)
        {
            if (values != null)
            {
                foreach (TValue value in values)
                {
                    base.Add(value.Name, value);
                }
            }
        }

        /// <summary>Initializes a new instance of the <see cref="IdentifierNameableDictionary&lt;TValue&gt;"/> class.
        /// </summary>
        /// <param name="values">The values to insert into the pool.</param>
        /// <exception cref="NullReferenceException">Thrown, if one item of <paramref name="values"/> is <c>null</c>.</exception>
        public IdentifierNameableDictionary(params TValue[] values)
            : base(isReadOnlyExceptAdding: true)
        {
            if (values != null)
            {
                foreach (TValue value in values)
                {
                    base.Add(value.Name, value);
                }
            }
        }
        #endregion

        #region public methods

        /// <summary>Adds the specified value.
        /// </summary>
        /// <param name="value">The value to add into the current <see cref="IdentifierNameableDictionary&lt;T&gt;"/> instance.</param>
        /// <returns>A value indicating whether <paramref name="value"/> has been inserted.</returns>
        /// <remarks>If <paramref name="value"/> != <c>null</c> the <see cref="Adding"/> and <see cref="Added"/>
        /// event will be raise.</remarks>
        public ItemAddedState Add(TValue value)
        {
            if (value != null)
            {
                bool nameExists = base.TryGetValue(value.Name, out TValue oldItem);

                if ((nameExists == false) || (base.IsReadOnlyExceptAdding == false))
                {
                    var args = new ItemAddingEventArgs(value, (nameExists == true) ? oldItem : default);
                    OnAdding(args);

                    if (args.Cancel == false)
                    {
                        var addedState = base.Add(value.Name, value);
                        OnAdded(new ItemAddedEventArgs(value, addedState));
                        return addedState;
                    }
                }
            }
            return ItemAddedState.Rejected;
        }

        /// <summary>Removes all keys and values from the <see cref="IdentifierNameableDictionary&lt;TValue&gt;"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown, if this operation is not allowed.</exception>
        public new void Clear()
        {
            base.Clear();
        }

        /// <summary>Removes the value with the specified key from the <see cref="IdentifierNameableDictionary&lt;TValue&gt;"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns><c>true</c> if the element is sucessfully found and removed; otherwise, <c>false</c>. This method
        /// returns <c>false</c> if <paramref name="key"/> is not found in the <see cref="IdentifierStringDictionaryBase&lt;TValue&gt;"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if this operation is not allowed.</exception>
        public new bool Remove(string key)
        {
            return base.Remove(key);
        }

        /// <summary>Removes the value with the specified key from the <see cref="IdentifierNameableDictionary&lt;TValue&gt;"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns><c>true</c> if the element is sucessfully found and removed; otherwise, <c>false</c>. This method
        /// returns <c>false</c> if <paramref name="key"/> is not found in the <see cref="IdentifierStringDictionaryBase&lt;TValue&gt;"/>.</returns>
        public new bool Remove(IdentifierString key)
        {
            return base.Remove(key);
        }
        #endregion

        #region protected methods

        /// <summary>Raises the <see cref="E:Adding"/> event.
        /// </summary>
        /// <param name="args">The <see cref="Dodoni.BasicComponents.Containers.ItemAddingEventArgs"/> instance containing the event data.</param>
        protected virtual void OnAdding(ItemAddingEventArgs args)
        {
            Adding?.Invoke(this, args);
        }

        /// <summary>Raises the <see cref="E:Added"/> event.
        /// </summary>
        /// <param name="args">The <see cref="Dodoni.BasicComponents.Containers.ItemAddedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnAdded(ItemAddedEventArgs args)
        {
            Added?.Invoke(this, args);
        }
        #endregion
    }

    /// <summary>Provides static methods for creating <see cref="IdentifierNameableDictionary&lt;TValue&gt;"/> objects.
    /// </summary>
    public static class IdentifierNameableDictionary
    {
        #region factory methods

        /// <summary>Creates a new <see cref="IdentifierNameableDictionary&lt;TValue&gt;"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>A <see cref="IdentifierNameableDictionary&lt;TValue&gt;"/> that contains <paramref name="value"/>.</returns>
        public static IdentifierNameableDictionary<TValue> Create<TValue>(TValue value)
            where TValue : IIdentifierNameable
        {
            return new IdentifierNameableDictionary<TValue>(value);
        }

        /// <summary>Creates a new <see cref="IdentifierNameableDictionary&lt;TValue&gt;"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>A <see cref="IdentifierNameableDictionary&lt;TValue&gt;"/> that contains <paramref name="value1"/> and <paramref name="value2"/>.</returns>
        public static IdentifierNameableDictionary<TValue> Create<TValue>(TValue value1, TValue value2)
            where TValue : IIdentifierNameable
        {
            var dictionary = new IdentifierNameableDictionary<TValue>
            {
                value1,
                value2
            };
            return dictionary;
        }

        /// <summary>Creates a new <see cref="IdentifierNameableDictionary&lt;TValue&gt;"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <param name="value3">The third value.</param>
        /// <returns>A <see cref="IdentifierNameableDictionary&lt;TValue&gt;"/> that contains <paramref name="value1"/>, <paramref name="value2"/> and <paramref name="value3"/>.</returns>
        public static IdentifierNameableDictionary<TValue> Create<TValue>(TValue value1, TValue value2, TValue value3)
          where TValue : IIdentifierNameable
        {
            var dictionary = new IdentifierNameableDictionary<TValue>
            {
                value1,
                value2,
                value3
            };
            return dictionary;
        }

        /// <summary>Creates a new <see cref="IdentifierNameableDictionary&lt;TValue&gt;"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="values">The values to add.</param>
        /// <returns>A <see cref="IdentifierNameableDictionary&lt;TValue&gt;"/> that contains the elements of <paramref name="values"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="values"/> is <c>null</c>.</exception>
        public static IdentifierNameableDictionary<TValue> Create<TValue>(params TValue[] values)
            where TValue : IIdentifierNameable
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }
            var dictionary = new IdentifierNameableDictionary<TValue>();
            foreach (TValue value in values)
            {
                dictionary.Add(value);
            }
            return dictionary;
        }
        #endregion

        #region extension methods

        /// <summary>Creates a <see cref="IdentifierNameableDictionary&lt;TValue&gt;"/> from a <see cref="IEnumerable&lt;TValue&gt;"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the elements of source.</typeparam>
        /// <param name="source">An <see cref="IEnumerable&lt;T&gt;"/> to create a <see cref="IdentifierNameableDictionary&lt;TValue&gt;"/> from.</param>
        /// <param name="isReadOnlyExceptAdding">A value that determines if the dictionary is readonly (except adding new items); if <c>true</c> new items can be added, overwriting or removing is not allowed; otherwise there are not restrictions.</param>
        /// <returns>A <see cref="IdentifierNameableDictionary&lt;T&gt;"/> that contains the elements from the input sequence.</returns>
        public static IdentifierNameableDictionary<TValue> ToIdNameableDictionary<TValue>(this IEnumerable<TValue> source, bool isReadOnlyExceptAdding = false)
            where TValue : IIdentifierNameable
        {
            var dictionary = new IdentifierNameableDictionary<TValue>(isReadOnlyExceptAdding);
            foreach (TValue value in source)
            {
                dictionary.Add(value);
            }
            return dictionary;
        }
        #endregion
    }
}