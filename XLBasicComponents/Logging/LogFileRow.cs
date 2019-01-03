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
using Dodoni.BasicComponents.Utilities;

namespace Dodoni.XLBasicComponents.Logging
{
    /// <summary>Represents a row of the logfile, i.e. for a specific object type/object name.
    /// </summary>
    public class LogFileRow
    {
        #region private members

        /// <summary>The time stamp.
        /// </summary>
        private DateTime m_Time;

        /// <summary>The type of the message.
        /// </summary>
   //     private Logger.MessageType m_MessageType;

        /// <summary>The message in its <see cref="System.String"/> representation.
        /// </summary>
        private string m_Message;
        #endregion

        #region public constructors

        ///// <summary>Initializes a new instance of the <see cref="LogFileRow"/> class.
        ///// </summary>
        ///// <param name="messageType">The message type.</param>
        ///// <param name="message">The message.</param>
        ///// <exception cref="ArgumentNullException">Thrown, if the <paramref name="messageType"/> is <c>null</c>.</exception>
        //public LogFileRow(Logger.MessageType messageType, string message)
        //{
        //    if (messageType == null)
        //    {
        //        throw new ArgumentNullException("messageType");
        //    }
        //    m_Time = DateTime.Now;
        //    m_MessageType = messageType;
        //    m_Message = (message != null) ? message : String.Empty;
        //}
        #endregion

        #region public properties

        /// <summary>Gets the timestamp of the logfile message.
        /// </summary>
        /// <value>The timestamp.</value>
        public DateTime Time
        {
            get { return m_Time; }
        }

        /// <summary>Gets the <see cref="Time"/> in its <see cref="System.String"/> representation.
        /// </summary>
        /// <value>The time stamp.</value>
        public string TimeStamp
        {
            get { return m_Time.ToString("T"); }
        }

        ///// <summary>Gets the classification of the message type in its <see cref="Logger.Classification"/> representation.
        ///// </summary>
        ///// <value>The classification of the message type.</value>
        //public Logger.Classification Classification
        //{
        //    get { return m_MessageType.Classification; }
        //}

        ///// <summary>Gets the message type classification in its <see cref="System.String"/> reprentation.
        ///// </summary>
        ///// <value>The message type classification in its <see cref="System.String"/> representation.</value>
        //public string MessageTypeClassification
        //{
        //    get { return m_MessageType.Classification.ToFormatString(); }
        //}

        ///// <summary>Gets the name of the message type.
        ///// </summary>
        ///// <value>The name of the message type.</value>
        //public string MessageTypeName
        //{
        //    get { return m_MessageType.LongName.String; }
        //}

        /// <summary>Gets the logfile message.
        /// </summary>
        /// <value>The message of the logfile.</value>
        public string Message
        {
            get { return m_Message; }
        }
        #endregion
    }
}