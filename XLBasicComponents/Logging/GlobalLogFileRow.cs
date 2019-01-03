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

using Dodoni.BasicComponents.Logging;

namespace Dodoni.XLBasicComponents.Logging
{
    /// <summary>Represents a row of the global log file.
    /// </summary>
    public class GlobalLogFileRow : LogFileRow
    {
        #region private members

        /// <summary>The object type.
        /// </summary>
        private Type m_ObjectType;

        /// <summary>The name of the object type, for example Holiday calendar pool etc.
        /// </summary>
        private string m_ObjectTypeName;

        /// <summary>The name of the object.
        /// </summary>
        private string m_ObjectName;
        #endregion

        #region public constructors

        ///// <summary>Initializes a new instance of the <see cref="GlobalLogFileRow"/> class.
        ///// </summary>
        ///// <param name="messageType">The message type.</param>
        ///// <param name="message">The message.</param>
        ///// <param name="objectType">The object type.</param>
        ///// <param name="objectTypeName">The name of the object type.</param>
        ///// <param name="objectName">The name of the specific object.</param>
        ///// <exception cref="ArgumentNullException">Thrown, if the <paramref name="messageType"/> is <c>null</c>.</exception>
        //public GlobalLogFileRow(Logger.MessageType messageType, string message = null, Type objectType = null, string objectTypeName = "", string objectName = "")
        //    : base(messageType, message)
        //{
        //    m_ObjectType = objectType;
        //    m_ObjectTypeName = (objectTypeName != null) ? objectTypeName : String.Empty;
        //    m_ObjectName = (objectName != null) ? objectName : String.Empty;
        //}
        #endregion

        #region public properties

        /// <summary>Gets the object type, perhaps <c>null</c>.
        /// </summary>
        /// <value>The object type.</value>
        public Type ObjectType
        {
            get { return m_ObjectType; }
        }

        /// <summary>Gets the name of the object type, for example Holiday Calendar Pool etc.
        /// </summary>
        /// <value>The name of the object type.</value>
        public string ObjectTypeName
        {
            get { return m_ObjectTypeName; }
        }

        /// <summary>Gets the name of the object.
        /// </summary>
        /// <value>The name of the object.</value>
        public string ObjectName
        {
            get { return m_ObjectName; }
        }
        #endregion
    }
}