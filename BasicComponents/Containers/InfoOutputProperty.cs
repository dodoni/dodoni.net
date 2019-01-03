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
    /// <summary>Serves as property (i.e. name/value pair) representation of a specific information with respect to <see cref="InfoOutput"/>.
    /// </summary>
    public class InfoOutputProperty : IIdentifierNameable, IAnnotatable
    {
        #region private members

        /// <summary>The property name in its identifier string representation.
        /// </summary>
        private readonly IdentifierString m_IdPropertyName;

        /// <summary>The annotation of the property, perhaps <c>null</c>.
        /// </summary>
        private readonly string m_Annotation;
        #endregion

        #region public (readonly) members

        /// <summary>The value of the property in its <see cref="System.Object"/> representation.
        /// </summary>
        public readonly object Value;
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the property in its <see cref="IdentifierString"/> representation.
        /// </summary>
        /// <value>The language independent name of the current instance.</value>
        public IdentifierString Name
        {
            get { return m_IdPropertyName; }
        }

        /// <summary>Gets the long name of the current instance, i.e. <see cref="InfoOutputProperty.Name"/> in its <see cref="IdentifierString"/> representation.
        /// </summary>
        /// <value>The language dependent long name of the current instance.</value>
        IdentifierString IIdentifierNameable.LongName
        {
            get { return m_IdPropertyName; }
        }
        #endregion

        #region IAnnotatable Members

        /// <summary>Gets a value indicating whether the annotation is readonly.
        /// </summary>
        /// <value><c>true</c> if the annotation of this instance is readonly; otherwise, <c>false</c>.
        /// </value>
        bool IAnnotatable.HasReadOnlyAnnotation
        {
            get { return true; }
        }

        /// <summary>Gets the annotation of the current instance.
        /// </summary>
        /// <value>The annotation of the current instance.</value>
        public string Annotation
        {
            get { return m_Annotation; }
        }
        #endregion

        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="InfoOutputProperty"/> class.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="propertyValue">The value of the property.</param>
        /// <param name="annotation">A annotation of the property in its <see cref="System.String"/> representation.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="propertyName"/> or <paramref name="propertyValue"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="propertyName"/> represents the empty string.</exception>
        public InfoOutputProperty(string propertyName, object propertyValue, string annotation = null)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException("propertyName");
            }
            if (propertyName.Length == 0)
            {
                throw new ArgumentException(String.Format(ExceptionMessages.ArgumentIsInvalid, "Property name ''"), "propertyName");
            }

            Value = propertyValue ?? throw new ArgumentNullException("propertyValue");
            m_Annotation = annotation;
            m_IdPropertyName = new IdentifierString(propertyName);
        }
        #endregion

        #region public methods

        #region IAnnotatable Members

        /// <summary>Sets the annotation of the current instance.
        /// </summary>
        /// <param name="annotation">The annotation.</param>
        /// <returns>A value indicating whether the <see cref="Annotation"/> has been changed.
        /// </returns>
        public bool TrySetAnnotation(string annotation)
        {
            return false;
        }
        #endregion

        /// <summary>Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return String.Format("{0} = {1}", Name, Value);
        }
        #endregion

        #region public (static) methods

        /// <summary>Creates a new <see cref="InfoOutputProperty"/> object.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="propertyValue">The value of the property.</param>
        /// <param name="annotation">A annotation of the property in its <see cref="System.String"/> representation.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="propertyName"/> or <paramref name="propertyValue"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="propertyName"/> represents the empty string.</exception>
        public static InfoOutputProperty Create(string propertyName, object propertyValue, string annotation = null)
        {
            return new InfoOutputProperty(propertyName, propertyValue, annotation);
        }
        #endregion
    }
}