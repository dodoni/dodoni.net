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

using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Containers;

namespace Dodoni.XLBasicComponents.IO
{
    /// <summary>Represents the set of available <see cref="IObjectStreamer"/> instances used for serialization and deserialization of <see cref="ExcelPool"/> objects.
    /// </summary>
    public static class ObjectStreamer
    {
        #region private static members

        /// <summary>The internal pool of <see cref="IObjectStreamer"/> used for serialization/deserialisation.
        /// </summary>
        private static IdentifierNameableDictionary<IObjectStreamer> sm_Pool;
        #endregion

        #region static constructor

        /// <summary>Initializes the <see cref="ObjectStreamer"/> class.
        /// </summary>
        static ObjectStreamer()
        {
            sm_Pool = new IdentifierNameableDictionary<IObjectStreamer>();

            /* do not allow replacing pool elements: */
            sm_Pool.Adding += (sender, eventArgs) => { eventArgs.Cancel = ((eventArgs.OldItem != null) && (eventArgs.NewItem.Name.IDString == eventArgs.OldItem.Name.IDString)); };
        }
        #endregion

        #region public static properties

        /// <summary>Gets the number of <see cref="IObjectStreamer"/>.
        /// </summary>
        /// <value>The number of <see cref="IObjectStreamer"/>.</value>
        public static int Count
        {
            get { return sm_Pool.Count; }
        }

        /// <summary>Gets the <see cref="IObjectStreamer"/> objects.
        /// </summary>
        /// <value>The <see cref="IObjectStreamer"/> objects.</value>
        public static IEnumerable<IObjectStreamer> Values
        {
            get { return sm_Pool.Values; }
        }
        #endregion

        #region public static methods

        /// <summary>Gets the names of the <see cref="IObjectStreamer"/> objects in the <see cref="ObjectStreamer"/>.
        /// </summary>
        /// <value>The names of the <see cref="IObjectStreamer"/> objects.</value>
        public static IEnumerable<IdentifierString> GetNames()
        {
            return sm_Pool.GetNamesAsIdentifierStrings();
        }

        /// <summary>Gets the file extensions of the <see cref="IObjectStreamer"/> objects in the <see cref="ObjectStreamer"/> class, in its <see cref="IdentifierString"/> representation.
        /// </summary>
        /// <returns>The file extensions of the <see cref="IObjectStreamer"/> objects.</returns>
        public static IEnumerable<IdentifierString> GetFileExtensions()
        {
            foreach (var item in sm_Pool.Values)
            {
                yield return item.FileExtension;
            }
        }

        /// <summary>Gets the <see cref="IObjectStreamer"/> object with repsect to a specific name.
        /// </summary>
        /// <param name="objectStreamerName">The (unique) name of the <see cref="IObjectStreamer"/>.</param>
        /// <param name="value">An instance of <see cref="IObjectStreamer"/>, where <see cref="IIdentifierNameable.Name"/> equals to
        /// <paramref name="objectStreamerName"/> (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public static bool TryGetObjectStreamer(IdentifierString objectStreamerName, out IObjectStreamer value)
        {
            return sm_Pool.TryGetValue(objectStreamerName, out value);
        }

        /// <summary>Gets a <see cref="IObjectStreamer"/> object with repsect to a specific name.
        /// </summary>
        /// <param name="objectStreamerName">The (unique) name of the <see cref="IObjectStreamer"/> to search.</param>
        /// <param name="value">An instance of <see cref="IObjectStreamer"/>, where <see cref="IIdentifierNameable.Name"/> equals to
        /// <paramref name="objectStreamerName"/> (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public static bool TryGetObjectStreamer(string objectStreamerName, out IObjectStreamer value)
        {
            return sm_Pool.TryGetValue(objectStreamerName, out value);
        }

        /// <summary>Gets a <see cref="IObjectStreamer"/> object with repsect to a specific file extension.
        /// </summary>
        /// <param name="fileExtension">The (unique) file extension of the <see cref="IObjectStreamer"/> to search.</param>
        /// <param name="value">An instance of <see cref="IObjectStreamer"/>, where <see cref="IObjectStreamer.FileExtension"/> equals to
        /// <paramref name="fileExtension"/> (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public static bool TryGetObjectStreamerByFileExtension(string fileExtension, out IObjectStreamer value)
        {
            string idFileExtension = fileExtension.ToIDString();
            foreach (var objectStreamer in sm_Pool.Values)
            {
                if (objectStreamer.FileExtension.IDString == idFileExtension)
                {
                    value = objectStreamer;
                    return true;
                }
            }
            value = null;
            return false;
        }

        /// <summary>Adds a specific <see cref="IObjectStreamer"/>.
        /// </summary>
        /// <param name="objectStreamer">The <see cref="IObjectStreamer"/> to add.</param>
        /// <returns>A value indicating whether <paramref name="objectStreamer"/> has been added.</returns>
        public static ItemAddedState Add(IObjectStreamer objectStreamer)
        {
            return sm_Pool.Add(objectStreamer);
        }
        #endregion
    }
}