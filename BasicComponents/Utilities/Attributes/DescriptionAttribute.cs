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
    /// <summary>Represents an attribute class that contains a specific <see cref="System.String"/> object which
    /// represents a (language independent) description of enum member etc.
    /// </summary>
    /// <example>
    /// <code>
    ///   public enum Example {
    ///    [String("Number 1")]   // or use LanguageStringAttribute
    ///    [Description("This is a sample...")]
    ///     No_1,
    ///     
    ///     // ...
    ///   }
    ///   ...
    ///   Example example = Example.No_1;
    ///   string name = example.ToFormatString();
    ///   string description = example.GetDescription();
    /// </code></example>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public sealed class DescriptionAttribute : System.Attribute
    {
        #region private/public (readonly) members

        /// <summary>The <see cref="System.String"/> representation of the description.
        /// </summary>
        public readonly string Description;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="DescriptionAttribute"/> class.
        /// </summary>
        /// <param name="description">The description.</param>
        public DescriptionAttribute(string description)
        {
            Description = description;
        }
        #endregion

        #region public methods

        /// <summary>Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return Description;
        }
        #endregion
    }
}