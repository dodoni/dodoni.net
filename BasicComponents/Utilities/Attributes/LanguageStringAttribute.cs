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
    /// <summary>Represents an attribute class which contains the property name of some resource 
    /// which contains some language depending <see cref="System.String"/> representation.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public sealed class LanguageStringAttribute : System.Attribute
    {
        #region public (readonly) members

        /// <summary>The property name with respect to a specific resource file that contains a language depending <see cref="System.String"/> representation.
        /// </summary>
        public readonly string ResourcePropertyName;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="LanguageStringAttribute"/> class.
        /// </summary>
        /// <param name="resourcePropertyName">The property name of the language depending <see cref="System.String"/> representation with respect 
        /// to a resource file that is represented by an instance of <see cref="LanguageResourceAttribute"/>.</param>
        public LanguageStringAttribute(string resourcePropertyName)
        {
            ResourcePropertyName = resourcePropertyName;
        }
        #endregion
    }
}