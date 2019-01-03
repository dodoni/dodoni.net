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

namespace Dodoni.BasicComponents.Utilities
{
    /// <summary>Represents a two sided list, i.e. two instances of <see cref="List&lt;T&gt;"/> and some center element where
    /// the index can be negative as well.
    /// </summary>
    /// <typeparam name="T">The type of the items.</typeparam>
    public class TwoSideList<T>
    {
        #region public members

        /// <summary>The center element.
        /// </summary>
        public T CenterElement;

        /// <summary>The left list.
        /// </summary>
        public readonly List<T> LeftList;

        /// <summary>The right list.
        /// </summary>
        public readonly List<T> RightList;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="TwoSideList&lt;T&gt;"/> class.
        /// </summary>
        public TwoSideList()
        {
            LeftList = new List<T>();
            RightList = new List<T>();
            CenterElement = default;
        }
        /// <summary>Initializes a new instance of the <see cref="TwoSideList&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="capacity">The number of elements that the new list can initialize store for each direction.</param>
        public TwoSideList(int capacity)
        {
            LeftList = new List<T>(capacity);
            RightList = new List<T>(capacity);
            CenterElement = default;
        }
        #endregion

        #region public properties

        /// <summary>Gets the number of elements.
        /// </summary>
        /// <value>The number of elements.</value>
        public int Count
        {
            get { return LeftList.Count + RightList.Count + 1; }
        }

        /// <summary>Gets the number of elements with negative index.
        /// </summary>
        /// <value>The number of elements with negative index.</value>
        public int LeftCount
        {
            get { return LeftList.Count; }
        }

        /// <summary>Gets the number of elements with strict positive index.
        /// </summary>
        /// <value>The number of elements with strict positive index.</value>
        public int RightCount
        {
            get { return RightList.Count; }
        }

        /// <summary>Gets or sets the <typeparamref name="T"/> at the specified index.
        /// </summary>
        /// <value>The element with the given index.</value>
        public T this[int index]
        {
            get
            {
                if (index == 0)
                {
                    return CenterElement;
                }
                else if (index > 0)
                {
                    return RightList[index - 1];
                }
                else
                {
                    return LeftList[-index - 1];
                }
            }
            set
            {
                if (index == 0)
                {
                    CenterElement = value;
                }
                else if (index > 0)
                {
                    RightList[index - 1] = value;
                }
                else
                {
                    LeftList[-index - 1] = value;
                }
            }
        }
        #endregion

        #region public methods

        /// <summary>Adds an element on the right side.
        /// </summary>
        /// <param name="value">The value.</param>
        public void AddRight(T value)
        {
            RightList.Add(value);
        }

        /// <summary>Adds an element on the left side.
        /// </summary>
        /// <param name="value">The value.</param>
        public void AddLeft(T value)
        {
            LeftList.Add(value);
        }

        /// <summary>Adds an element on the left and a second element on the right side.
        /// </summary>
        /// <param name="leftValue">The left value.</param>
        /// <param name="rightValue">The right value.</param>
        public void Add(T leftValue, T rightValue)
        {
            LeftList.Add(leftValue);
            RightList.Add(rightValue);
        }

        /// <summary>Removes all elements from the list, the <see cref="CenterElement"/> will be set to the default value.
        /// </summary>
        public void Clear()
        {
            LeftList.Clear();
            RightList.Clear();
            CenterElement = default;
        }

        /// <summary>Removes a sub-list.
        /// </summary>
        /// <param name="index">The position where to start removing elements.</param>
        /// <remarks>The element at position <paramref name="index"/> will be removed as well but the center element will not be removed.</remarks>
        public void RemoveSubList(int index)
        {
            if (index >= 1)
            {
                RightList.RemoveRange(index - 1, RightList.Count - index + 1);
            }
            else if (index <= -1)
            {
                LeftList.RemoveRange(-index - 1, LeftList.Count + index + 1);
            }
        }

        /// <summary>Remove two symmetric sub-lists from the internal list.
        /// </summary>
        /// <param name="index">The position where to start removing elements in both directions, i.e. also with the negative index.</param>
        /// <remarks>The center element will not be removed.</remarks>
        public void SymmetricClear(int index)
        {
            RemoveSubList(index);
            RemoveSubList(-index);
        }
        #endregion
    }
}