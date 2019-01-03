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

namespace Dodoni.BasicComponents
{
    /// <summary>Represents extensions for <see cref="IdentifierString"/> as well as for <see cref="System.String"/>.
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static class IdentifierStringExtensions
    {
        #region public (static) methods

        /// <summary>Gets the <see cref="IdentifierString"/> representation.
        /// </summary>
        /// <param name="value">The (raw) string.</param>
        /// <returns>The <see cref="IdentifierString"/> representation of <paramref name="value"/>.</returns>
        public static IdentifierString ToIdentifierString(this string value)
        {
            return new IdentifierString(value);
        }

        /// <summary>Gets the <see cref="IdentifierString.IDString"/> component of the <see cref="IdentifierString"/> representation.
        /// </summary>
        /// <param name="value">The (raw) string.</param>
        /// <returns>The <see cref="IdentifierString.IDString"/> component of the <see cref="IdentifierString"/> representation of <paramref name="value"/>.</returns>
        public static string ToIDString(this string value)
        {
            return IdentifierString.GetIDString(value);
        }

        /// <summary>Gets the relevant substring, i.e. returns a copy of the argument, where each character followed by <see cref="IdentifierString.IgnoringStartCharacter"/> will be suppress.
        /// </summary>
        /// <param name="value">The string.</param>
        /// <returns>A copy of <paramref name="value"/> where each character followed by <see cref="IdentifierString.IgnoringStartCharacter"/> will be removed.</returns>
        public static string GetRelevantSubstring(this string value)
        {
            int index = value.IndexOf(IdentifierString.IgnoringStartCharacter);  // perhaps use span<char> in the future?
            if (index >= 0)
            {
                return value.Remove(index);
            }
            return value;
        }

        /// <summary>Gets a <see cref="System.String"/> representation that contains an additional time stamp, where <see cref="IdentifierString.IgnoringStartCharacter"/> 
        /// is used as splitting character.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><paramref name="value"/> plus <see cref="IdentifierString.IgnoringStartCharacter"/> plus the current time.</returns>
        public static string ToTimeStampString(this string value)
        {
            return String.Format("{0}{1}{2}", value, IdentifierString.IgnoringStartCharacter, DateTime.Now.ToString("HH:mm:ss.ff"));
        }

        /// <summary>Gets a <see cref="System.String"/> representation that contains an additional time stamp, where <see cref="IdentifierString.IgnoringStartCharacter"/> 
        /// is used as splitting character.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="dateTime">The <see cref="System.DateTime"/> object.</param>
        /// <returns><paramref name="value"/> plus <see cref="IdentifierString.IgnoringStartCharacter"/> plus <paramref name="dateTime"/>.</returns>
        public static string ToTimeStampString(this string value, DateTime dateTime)
        {
            return String.Format("{0}{1}{2}", value, IdentifierString.IgnoringStartCharacter, dateTime.ToString("HH:mm:ss.ff"));
        }
        #endregion
    }
}