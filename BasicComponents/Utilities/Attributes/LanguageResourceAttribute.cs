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

namespace Dodoni.BasicComponents.Utilities
{
    /// <summary>Represents an attribute class that contains the name of the resource (files) which contains language dependend 
    /// <see cref="System.String"/> representations of the elements of the underlying enumeration.
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum, Inherited = false, AllowMultiple = false)]
    public sealed class LanguageResourceAttribute : System.Attribute
    {
        #region public (readonly) members

        /// <summary>The resource name (no language dependend suffix) and the corresponding namespace.
        /// </summary>
        public readonly string FullResourceName;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="LanguageResourceAttribute"/> class.
        /// </summary>
        /// <param name="fullResourceName">The name of the resource (including the namespace).</param>
        public LanguageResourceAttribute(string fullResourceName)
        {
            FullResourceName = fullResourceName;
        }
        #endregion
    }
}