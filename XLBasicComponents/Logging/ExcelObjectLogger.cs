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
using System.ComponentModel;
using System.Collections.Generic;

using Dodoni.BasicComponents.Logging;
using Dodoni.BasicComponents.Utilities;
using System.IO;

namespace Dodoni.XLBasicComponents.Logging
{
    /// <summary>Serves as logger for a specific object.
    /// </summary>
    public class ExcelObjectLogger //: ILoggerStream
    {
        #region private static members

        /// <summary>A value indicating whether to write a file.
        /// </summary>
        private static ExcelLogger.OutputUsage sm_OutputUsage;

        /// <summary>The XML tag in the configuration file for the 'output usage'.
        /// </summary>
        private const string m_OutputUsageConfigKey = "LoggingOutputUsage";

        /// <summary>The (optional) output folder.
        /// </summary>
        private static string sm_OutputFolder;

        /// <summary>The XML tag in the configuration file for the 'output folder'.
        /// </summary>
        private const string m_OutputFolderConfigKey = "LoggingOutputFolder";
        #endregion

        #region private members

        /// <summary>The name of the object type.
        /// </summary>
        private string m_ObjectTypeName;

        /// <summary>The object type.
        /// </summary>
        private Type m_ObjectType;

        /// <summary>The name of the object to log.
        /// </summary>
        private string m_ObjectName;

        /// <summary>The optional channel.
        /// </summary>
        private string m_Channel;

        /// <summary>The list of messages.
        /// </summary>
        private List<LogFileRow> m_ListOfMessages;

        /// <summary>A reference to the Excel logger (for global messages, for example fatal messages).
        /// </summary>
        private ExcelLogger m_ExcelLogger;

        /// <summary>A stream writer.
        /// </summary>
        private TextWriter m_StreamWriter = null;
        #endregion

        #region static constructor

        /// <summary>Initializes the <see cref="ExcelObjectLogger" /> class.
        /// </summary>
        static ExcelObjectLogger()
        {
            sm_OutputUsage = GetOutputUsage();
            sm_OutputFolder = GetOutputFolder();
        }
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="ExcelObjectLogger" /> class.
        /// </summary>
        /// <param name="senderObjectTypeName">The object type name of the source object that adds the message.</param>
        /// <param name="senderObjectType">The type of the source object that adds the message.</param>
        /// <param name="senderObjectName">The name of the source object that adds the message.</param>
        /// <param name="channel">A specific channel, i.e. a name or category, for example <c>YieldCurveConstruction</c> etc.</param>
        /// <param name="excelLogger">The reference to the Excel logger (for fatal error messages etc.).</param>
        public ExcelObjectLogger(string senderObjectTypeName, Type senderObjectType, string senderObjectName, string channel, ExcelLogger excelLogger)
        {
            m_ObjectTypeName = senderObjectTypeName;
            m_ObjectType = senderObjectType;
            m_ObjectName = senderObjectName;
            m_Channel = channel;
            m_ExcelLogger = excelLogger;

            m_ListOfMessages = new List<LogFileRow>();

            if (sm_OutputUsage == ExcelLogger.OutputUsage.New)
            {
                string fileName = null;
                if ((senderObjectTypeName != null) && (senderObjectTypeName != ""))
                {
                    fileName = senderObjectTypeName;
                }

                if ((senderObjectName != null) && (senderObjectName != ""))
                {
                    if (fileName != null)
                    {
                        fileName += "." + senderObjectName;
                    }
                    else
                    {
                        fileName = senderObjectName;
                    }
                }

                if (fileName == null)
                {
                    fileName = "UnknownObject";
                }

                //fileName = Path.GetRandomFileName();
                fileName += Guid.NewGuid().ToString() + ".log";

                // alternativ:
                DirectoryInfo info = new DirectoryInfo(sm_OutputFolder);
                long uniqueKey = info.LastWriteTime.Ticks + 1L;
                string f = String.Format("fsssdf{0}.text", uniqueKey);

                m_StreamWriter = new StreamWriter(Path.Combine(sm_OutputFolder, fileName));

                //m_StreamWriter.Close();
            }
        }
        #endregion

        #region public methods

        #region ILoggerStream Members

        ///// <summary>Adds a specific message.
        ///// </summary>
        ///// <param name="messageType">The type of the message.</param>
        ///// <param name="message">The message.</param>
        ///// <param name="exception">The exception raised by the source object that adds the message.</param>
        //public void Add(Logger.MessageType messageType, string message = null, Exception exception = null)
        //{
        //    m_ListOfMessages.Add(new LogFileRow(messageType, message));
        //    if (exception != null)
        //    {
        //        m_ListOfMessages.Add(new LogFileRow(messageType, exception.Message));
        //    }

        //    if (m_StreamWriter != null)
        //    {
        //        m_StreamWriter.WriteLine(DateTime.Now.ToString("T") + ":  <" + messageType.LongName.String + "> [" + ((m_ObjectTypeName == null) ? "unknown" : m_ObjectTypeName) + "; " + ((m_ObjectName == null) ? "unknown" : m_ObjectName) + "]" + message);
        //    }
        //}
        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {

        }

        #endregion

        /// <summary>Gets the logfile in some <see cref="System.String"/> representation.
        /// </summary>
        /// <returns>The logfile in its <see cref="System.String"/> representation.</returns>
        public string GetAsString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < m_ListOfMessages.Count; i++)
            {
                LogFileRow row = m_ListOfMessages[i];

//                stringBuilder.AppendLine(row.TimeStamp + ": <" + row.Classification.ToFormatString() + ": " + row.MessageTypeName + "> [" + m_ObjectName + "; " + m_ObjectTypeName + "] " + row.Message);
            }
            return stringBuilder.ToString();
        }

        /////<summary>Gets the logfile in some <see cref="System.String"/> representation.
        /////</summary>
        /////<param name="messageTypes">The message types to take into account.</param>
        /////<returns>The <see cref="System.String"/> representation of the global logfile, where the first index corresponds to the null-based
        /////row index and the second index corresponds to the type, i.e. date, message type, object name, object type and logfile message.</returns>
        /////<remarks>The output contains some extra row, which represents the header.</remarks>
        //public string[][] GetAsStringArray(params Logger.Classification[] messageTypes)
        //{
        //    int filterFlags = 0x00;

        //    if ((messageTypes != null) && (messageTypes.Length > 0))
        //    {
        //        foreach (var logFileMessageType in messageTypes)
        //        {
        //            filterFlags |= (int)logFileMessageType;
        //        }
        //    }
        //    else
        //    {
        //        foreach (Logger.Classification logFileMessageType in Enum.GetValues(typeof(Logger.Classification)))
        //        {
        //            filterFlags |= (int)logFileMessageType;
        //        }
        //    }
        //    return GetAsStringArray(filterFlags);
        //}
        #endregion

        #region internal methods

        /// <summary>Gets the current instance as binding list.
        /// </summary>
        /// <returns>The BindingList representation.</returns>
        internal BindingList<LogFileRow> GetAsBindingList()
        {
            return new BindingList<LogFileRow>(m_ListOfMessages);
        }
        #endregion

        #region private methods

        /// <summary>Gets the logfile in some <see cref="System.String"/> representation.
        /// </summary>
        /// <param name="filterFlags">The message types to take into account.</param>
        /// <returns>The <see cref="System.String"/> representation of the global logfile, where the first index corresponds to the null-based
        /// row index and the second index corresponds to the type, i.e. date, message type, object name, object type and logfile message.</returns>
        /// <remarks>The output contains some extra row, which represents the header.</remarks>
        private string[][] GetAsStringArray(int filterFlags)
        {
            List<string[]> messageList = new List<string[]>();

            string[] header = { XLResources.LogFileTimeHeader, XLResources.LogFileClassificationHeader, XLResources.LogFileMessageTypeHeader, XLResources.LogFileObjectNameHeader, XLResources.LogFileObjectTypeHeader, XLResources.LogFileMessageHeader };
            messageList.Add(header);

            for (int i = 0; i < m_ListOfMessages.Count; i++)
            {
                LogFileRow row = m_ListOfMessages[i];
                //if (((int)row.Classification & filterFlags) != 0)
                //{
                //    messageList.Add(new string[] { row.TimeStamp, row.Classification.ToFormatString(), row.MessageTypeName, m_ObjectName, m_ObjectTypeName, row.Message });
                //}
            }
            return messageList.ToArray();
        }
        #endregion

        /// <summary>Gets a flag from the configuration file indicating which kind of output to apply.
        /// </summary>
        /// <returns>The value of the flag in the configuration file or a spcific standard value.</returns>
        internal static ExcelLogger.OutputUsage GetOutputUsage()
        {
            ExcelLogger.OutputUsage result = ExcelLogger.OutputUsage.None;

            string stringRepresentation;

            if (ExcelAddIn.Configuration.GeneralSettings.TryGetValue(m_OutputUsageConfigKey, out stringRepresentation, defaultValue: "") == true)
            {
                if (Enum.TryParse<ExcelLogger.OutputUsage>(stringRepresentation, out result) == false)
                {
                    return ExcelLogger.OutputUsage.None;
                }
            }
            return result;
        }

        /// <summary>Stores a flag in the config file indicating which kind of output to apply.
        /// </summary>
        /// <param name="outputUsage">The output usage.</param>
        internal static void SetOutputUsage(ExcelLogger.OutputUsage outputUsage)
        {
            ExcelAddIn.Configuration.GeneralSettings.SetValue(m_OutputUsageConfigKey, outputUsage);
        }

        /// <summary>Gets the output folder.
        /// </summary>
        /// <returns>The output folder.</returns>
        internal static string GetOutputFolder()
        {
            string outputFolder;
            ExcelAddIn.Configuration.GeneralSettings.TryGetValue(m_OutputFolderConfigKey, out outputFolder, defaultValue: "");

            return outputFolder;
        }

        /// <summary>Stores the output folder in the configuration file.
        /// </summary>
        /// <param name="outputFolder">The output folder.</param>
        internal static void SetOutputFolder(string outputFolder)
        {
            ExcelAddIn.Configuration.GeneralSettings.SetValue(m_OutputFolderConfigKey, outputFolder);
        }
    }
}