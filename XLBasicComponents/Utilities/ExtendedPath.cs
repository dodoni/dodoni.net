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
using System.IO;
using System.Text;

namespace Dodoni.XLBasicComponents.Utilities
{
    /// <summary>Represents an extension of the <see cref="System.IO.Path"/> class.
    /// </summary>
    /// <remarks>File extensions contains '.' in the .NET framework.</remarks>
    internal static class ExtendedPath
    {
        /// <summary>Gets the extension of the specific path string.
        /// </summary>
        /// <param name="path">The path string from which to get the extension.</param>
        /// <returns>The file extension of <paramref name="path"/>.</returns>
        public static string GetExtension(string path)
        {
            string fileExtension = Path.GetExtension(path);
            if (fileExtension == null)
            {
                return String.Empty;
            }

            if (fileExtension.StartsWith(".") == true)
            {
                return fileExtension.Substring(1);  // remove '.' of the file extension
            }
            return fileExtension;
        }
    }
}