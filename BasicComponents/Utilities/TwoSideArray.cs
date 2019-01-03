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
    /// <summary>Represents a two sided array, i.e. two arrays and some center element where the index can be negative as well.
    /// </summary>
    /// <typeparam name="T">The type of the items.</typeparam>
    public class TwoSideArray<T>
    {
        #region public members

        /// <summary>The center element.
        /// </summary>
        public T CenterElement;

        /// <summary>The left array.
        /// </summary>
        public readonly T[] LeftArray;

        /// <summary>The right array.
        /// </summary>
        public readonly T[] RightArray;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="TwoSideArray&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="leftArrayLength">The length of the left array.</param>
        /// <param name="rightArrayLength">The lenght of the right array.</param>
        public TwoSideArray(int leftArrayLength, int rightArrayLength)
        {
            LeftCount = leftArrayLength;
            RightCount = rightArrayLength;

            LeftArray = new T[leftArrayLength];
            RightArray = new T[rightArrayLength];
        }

        /// <summary>Initializes a new instance of the <see cref="TwoSideArray&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="leftArray">The left array.</param>
        /// <param name="leftCount">The number of items in <paramref name="leftArray"/> to take into account.</param>
        /// <param name="rightArray">The right array.</param>
        /// <param name="rightCount">The number of item in <paramref name="rightArray"/> to take into account.</param>
        /// <param name="centerElement">The center element.</param>
        public TwoSideArray(T[] leftArray, int leftCount, T[] rightArray, int rightCount, T centerElement)
        {
            LeftCount = leftCount;
            LeftArray = leftArray;

            RightArray = rightArray;
            RightCount = rightCount;
            CenterElement = centerElement;
        }
        #endregion

        #region public properties

        /// <summary>Gets the number of elements.
        /// </summary>
        /// <value>The number of elements.</value>
        public int Count
        {
            get
            {
                return LeftCount + RightCount + 1;
            }
        }

        /// <summary>Gets the number of elements with negative index.
        /// </summary>
        /// <value>The number of elements with negative index.</value>
        public int LeftCount
        {
            get;
            private set;
        }

        /// <summary>Gets the number of elements with strict positive index.
        /// </summary>
        /// <value>The number of elements with strict positive index.</value>
        public int RightCount
        {
            get;
            private set;
        }

        /// <summary>Gets or sets the  <typeparamref name="T"/> at the specified index.
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
                    return RightArray[index - 1];
                }
                else
                {
                    return LeftArray[-index - 1];
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
                    RightArray[index - 1] = value;
                }
                else
                {
                    LeftArray[-index - 1] = value;
                }
            }
        }
        #endregion
    }
}