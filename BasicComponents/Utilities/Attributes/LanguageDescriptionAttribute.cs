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
    /// <summary>Represents an attribute class that contains a language depending description, more precisely it contains 
    /// the property name of a resource file which contains a specific language depending <see cref="System.String"/> representation.
    /// </summary>
    /// <example>
    /// <code>
    ///   [LanguageResource("YourNamespace.ResourceFileName")]
    ///   public enum Example {
    ///    [String("Number 1")]   // or use LanguageStringAttribute
    ///    [LanguageDescriptionAttribute("Example_1", "Test")]
    ///     No_1,
    ///     
    ///     // ...
    ///   }
    ///   // Moreover a resource file is given with respect to the 'LanguageResource' attribute
    ///   // which contains some element with name 'Example_1' (no code generation is necessary),
    ///   // for example: "Example_1": "This is my {0}."
    ///   ...
    ///   Example example = Example.No_1;
    ///   string name = example.ToFormatString();
    ///   string description = example.GetDescription();  // = "This is my Test."
    /// </code></example>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public sealed class LanguageDescriptionAttribute : System.Attribute
    {
        #region private/public (readonly) members

        /// <summary>The property name with respect to a given resource which contains some language dependend <see cref="System.String"/> representation.
        /// </summary>
        public readonly string ResourcePropertyName;

        /// <summary>The first argument used in the <see cref="System.String.Format(string,object)"/> method needed to generate the <see cref="System.String"/> representation.
        /// </summary>
        private object m_Arg0;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="LanguageDescriptionAttribute"/> class.
        /// </summary>
        /// <param name="resourcePropertyName">The property name of the language dependend <see cref="System.String"/> representation with respect 
        /// to a resource file which is represented by an instance of <see cref="LanguageResourceAttribute"/>. The resource string represents some composite format string.</param>
        public LanguageDescriptionAttribute(string resourcePropertyName)
        {
            ResourcePropertyName = resourcePropertyName;
        }
        #endregion

        #region public properties

        /// <summary>Gets or sets the first argument used in the <see cref="System.String.Format(string,object)"/> method 
        /// needed to generate the <see cref="System.String"/> representation of the current instance.
        /// </summary>
        /// <value>The first argument needed for the <see cref="System.String"/> representation.</value>
        public object Arg0
        {
            get { return m_Arg0; }
            set { m_Arg0 = value; }
        }
        #endregion
    }
}