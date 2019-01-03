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
using System.ComponentModel;
using System.ComponentModel.Composition;
using Dodoni.BasicComponents.Logging;
using Microsoft.Extensions.Logging;

namespace Dodoni.XLBasicComponents.Logging
{
    /// <summary>Represents the logging for the Excel Add-In.
    /// </summary>
    [Export(typeof(ILogger))]
    public partial class ExcelLogger : ILogger
    {
        #region private static members

        /// <summary>A value indicating whether a specific warning will be added if the creation of a drop down list with data advice fails.
        /// </summary>
        private static bool sm_AddOnDataAdviceFails = true;

        /// <summary>The XML tag in the configuration file for the 'add global (info) log file message if excel data advice fails' checkbox.
        /// </summary>
        private const string m_AddOnDataAdviceFailsConfigKey = "LogOnDataAdviceFails";

        /// <summary>A value indicating whether the global logging message form will be pop-up if an (Fatal) Error occur.
        /// </summary>
        private static bool sm_PopupGlobalLogfileBoxOnError = false;

        /// <summary>The XML tag in the configuration file for the 'popup global logging Windows form on error' checkbox.
        /// </summary>
        private const string m_PopupFormOnErrorConfigKey = "PopupGlobalLoggingOnError";
        #endregion

        #region private members

        /// <summary>The 'global' logging as as a list of <see cref="GlobalLogFileRow"/> objects.
        /// </summary>
        /// <remarks>Here, we use a Windows Form, therfore we use a BindingList instead of a List.</remarks>
        private BindingList<GlobalLogFileRow> m_GlobalLogFile;
        #endregion

        #region static constructor

        /// <summary>Initializes the <see cref="ExcelLogger" /> class.
        /// </summary>
        static ExcelLogger()
        {
            sm_AddOnDataAdviceFails = GetAddOnDataAdviceFails();
            sm_PopupGlobalLogfileBoxOnError = GetPopupGlobalLogfileBoxOnError();
        }
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="ExcelLogger" /> class.
        /// </summary>
        public ExcelLogger()
        {
            m_GlobalLogFile = new BindingList<GlobalLogFileRow>();
        }
        #endregion

        #region public properties

        /// <summary>Gets the source of data for the Windows Form.
        /// </summary>
        /// <value>The data source.</value>
        public BindingList<GlobalLogFileRow> DataSource
        {
            get { return m_GlobalLogFile; }
        }
        #endregion

        #region ILogger Members

        ///// <summary>Adds a specific message.
        ///// </summary>
        ///// <param name="messageType">The type of the message.</param>
        ///// <param name="message">The message.</param>
        ///// <param name="senderObjectTypeName">The object type name of the source object that adds the message.</param>
        ///// <param name="senderObjectType">The type of the source object that adds the message.</param>
        ///// <param name="senderObjectName">The name of the source object that adds the message.</param>
        ///// <param name="exception">The exception raised by the source object that adds the message.</param>
        //public virtual void Add(Logger.MessageType messageType, string message = "", string senderObjectTypeName = "", Type senderObjectType = null, string senderObjectName = "", Exception exception = null)
        //{
        //    m_GlobalLogFile.Add(new GlobalLogFileRow(messageType, message, senderObjectType, senderObjectTypeName, senderObjectName));

        //    if (exception != null)
        //    {
        //        m_GlobalLogFile.Add(new GlobalLogFileRow(messageType, exception.Message, senderObjectType, senderObjectTypeName, senderObjectName));
        //    }
        //    if ((messageType is Logger.MessageType.FatalError) && (sm_PopupGlobalLogfileBoxOnError == true))
        //    {
        //        ExcelAddIn.ShowGlobalLogFile();
        //    }
        //}

        ///// <summary>Creates a new <see cref="ILoggerStream" /> object.
        ///// </summary>
        ///// <param name="senderObjectTypeName">The object type name of the source object that adds the message.</param>
        ///// <param name="senderObjectType">The type of the source object that adds the message.</param>
        ///// <param name="senderObjectName">The name of the source object that adds the message.</param>
        ///// <param name="channel">A specific channel, i.e. a name or category, for example <c>YieldCurveConstruction</c> etc.</param>
        ///// <returns>A new <see cref="ILoggerStream" /> instance.</returns>
        //public virtual ILoggerStream Create(string senderObjectTypeName, Type senderObjectType, string senderObjectName = "", string channel = "")
        //{
        //    return new ExcelObjectLogger(senderObjectTypeName, senderObjectType, senderObjectName, channel, this);
        //}
        #endregion

        #region ILoggerStream Members

        ///// <summary>Adds a specific message.
        ///// </summary>
        ///// <param name="messageType">The type of the message.</param>
        ///// <param name="message">The message.</param>
        ///// <param name="exception">The exception raised by the source object that adds the message.</param>
        //public virtual void Add(Logger.MessageType messageType, string message = null, Exception exception = null)
        //{
        //    m_GlobalLogFile.Add(new GlobalLogFileRow(messageType, message));

        //    if (exception != null)
        //    {
        //        m_GlobalLogFile.Add(new GlobalLogFileRow(messageType, exception.Message));
        //    }
        //    if ((messageType.Classification == Logger.Classification.FatalErrorMessage) && (sm_PopupGlobalLogfileBoxOnError == true))
        //    {
        //        ExcelAddIn.ShowGlobalLogFile();
        //    }
        //}
        #endregion

        #region internal static methods

        /// <summary>Gets a flag from the configuration file indicating whether the global logfile Windows form will be pop-up if an (Fatal) Error occurs.
        /// </summary>
        /// <returns>The value of the flag in the configuration file or some standard value.</returns>
        internal static bool GetPopupGlobalLogfileBoxOnError()
        {
            bool popupGlobalLogfileBoxOnError;

            ExcelAddIn.Configuration.GeneralSettings.TryGetValue(m_PopupFormOnErrorConfigKey, out popupGlobalLogfileBoxOnError, defaultValue: true);
            return popupGlobalLogfileBoxOnError;
        }

        /// <summary>Stores a flag in the config file indicating whether the global logfile Windows form will be pop-up if an (fatal) error occurs.
        /// </summary>
        /// <param name="state">If set to <c>true</c> the global logfile Windows form will be pop-up if an (fatal) error occurs.</param>
        internal static void SetPopupGlobalLogfileBoxOnError(bool state)
        {
            ExcelAddIn.Configuration.GeneralSettings.SetValue(m_PopupFormOnErrorConfigKey, state);
        }

        /// <summary>Gets a value indicating whether a specific message will be added if the creation of a dropdown list with user data advice fails.
        /// </summary>
        /// <value><c>true</c> if a message should be be added if the creation of a dropdown list with user data advice fails; otherwise, <c>false</c>.</value>
        public static bool AddOnDataAdviceFails
        {
            get { return sm_AddOnDataAdviceFails; }
        }

        /// <summary>Loads the <see cref="AddOnDataAdviceFails"/> flag from the config file.
        /// </summary>
        /// <returns>The loaded <see cref="AddOnDataAdviceFails"/> flag.</returns>
        internal static bool GetAddOnDataAdviceFails()
        {
            ExcelAddIn.Configuration.GeneralSettings.TryGetValue(m_AddOnDataAdviceFailsConfigKey, out sm_AddOnDataAdviceFails, defaultValue: true);
            return sm_AddOnDataAdviceFails;
        }

        /// <summary>Sets the <see cref="AddOnDataAdviceFails"/> flag and write the flag into the config file.
        /// </summary>
        /// <param name="state">The <see cref="System.Boolean"/> to set the <see cref="AddOnDataAdviceFails"/> flag.</param>
        internal static void SetAddOnDataAdviceFails(bool state)
        {
            sm_AddOnDataAdviceFails = state;
            ExcelAddIn.Configuration.GeneralSettings.SetValue(m_AddOnDataAdviceFailsConfigKey, state);
        }
        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}