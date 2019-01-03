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
    /// <summary>Represents the event data for the event that will be raised after adding a new item into a specific <see cref="IdentifierNameableDictionary&lt;T&gt;"/> instance.
    /// </summary>
    public class ItemAddedEventArgs : EventArgs
    {
        #region public (readonly) members

        /// <summary>The new object to add into the <see cref="IdentifierNameableDictionary&lt;T&gt;"/> object.
        /// </summary>
        public readonly IIdentifierNameable NewItem;

        /// <summary>The state.
        /// </summary>
        public readonly ItemAddedState State;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="ItemAddedEventArgs"/> class.
        /// </summary>
        /// <param name="newItem">The new element that has been added into some <see cref="IdentifierNameableDictionary&lt;T&gt;"/> instance.</param>
        /// <param name="state">The state.</param>
        internal ItemAddedEventArgs(IIdentifierNameable newItem, ItemAddedState state)
        {
            NewItem = newItem;
            State = state;
        }
        #endregion
    }
}