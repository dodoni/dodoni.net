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

namespace Dodoni.BasicComponents.Containers
{
    /// <summary>Represents the event data for the event that will be raised before adding a new item into a specific <see cref="IdentifierNameableDictionary&lt;T&gt;"/> object.
    /// </summary>
    public class ItemAddingEventArgs : EventArgs
    {
        #region public (readonly) members

        /// <summary>The new object to add into the <see cref="IdentifierNameableDictionary&lt;T&gt;"/> instance.
        /// </summary>
        public readonly IIdentifierNameable NewItem;

        /// <summary>The item of the <see cref="IdentifierNameableDictionary&lt;T&gt;"/> object that will be replaced by <see cref="NewItem"/>; or <c>null</c> if no such element exists.
        /// </summary>
        public readonly IIdentifierNameable OldItem;

        /// <summary>A value indicating whether the adding of <see cref="NewItem"/> will be canceled.
        /// </summary>
        public bool Cancel;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="ItemAddingEventArgs"/> class.
        /// </summary>
        /// <param name="newItem">The new element to add into a specific <see cref="IdentifierNameableDictionary&lt;T&gt;"/> instance.</param>
        /// <param name="oldItem">The element of a specific <see cref="IdentifierNameableDictionary&lt;T&gt;"/> object which will
        /// be replaced by <paramref name="newItem"/>; or <c>null</c> if no such element exists.</param>
        internal ItemAddingEventArgs(IIdentifierNameable newItem, IIdentifierNameable oldItem)
        {
            NewItem = newItem;
            OldItem = oldItem;

            Cancel = false;
        }
        #endregion
    }
}