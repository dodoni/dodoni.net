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
using System.Diagnostics;

using System.Composition;
using Microsoft.Extensions.Logging;

namespace Dodoni.BasicComponents.Logging
{
    /// <summary>Represents a simple log file, a simple <see cref="System.StringBuilder"/> object is used for logging.
    /// </summary>
    [Export(typeof(IloggerStreamFactory))]
    public class TextLogFileGenerator : IloggerStreamFactory
    {
        #region nested classes

        /// <summary>A logger with respect to a specific object.
        /// </summary>
        private class LoggerStream : ILogger, IDisposable
        {
            #region private members

            private string m_SenderObjectTypeName;
            private Type m_SenderObjectType;
            private string m_SenderObjectName;
            private StringBuilder m_LoggingString = new StringBuilder();
            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="LoggerStream" /> class.
            /// </summary>
            /// <param name="senderObjectTypeName">Name of the sender object type.</param>
            /// <param name="senderObjectType">Type of the sender object.</param>
            /// <param name="senderObjectName">Name of the sender object.</param>
            internal LoggerStream(string senderObjectTypeName, Type senderObjectType, string senderObjectName = "")
            {
                m_SenderObjectTypeName = senderObjectTypeName;
                m_SenderObjectType = senderObjectType;
                m_SenderObjectName = senderObjectName;
            }
            #endregion

            #region public methods

            #region IDisposable Members

            /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                // nothing to do here
            }
            #endregion            

            #region ILogger Members

            /// <summary>Begins a logical operation scope.
            /// </summary>
            /// <typeparam name="TState"></typeparam>
            /// <param name="state">The identifier for the scope.</param>
            /// <returns>An IDisposable that ends the logical operation scope on dispose.</returns>
            public IDisposable BeginScope<TState>(TState state)
            {
                return this;
            }

            /// <summary>Checks if the given <paramref name="logLevel" /> is enabled.
            /// </summary>
            /// <param name="logLevel">level to be checked.</param>
            /// <returns><c>true</c> if enabled.</returns>            
            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            /// <summary>Writes a log entry.
            /// </summary>
            /// <typeparam name="TState"></typeparam>
            /// <param name="logLevel">Entry will be written on this level.</param>
            /// <param name="eventId">Id of the event.</param>
            /// <param name="state">The entry to be written. Can be also an object.</param>
            /// <param name="exception">The exception related to this entry.</param>
            /// <param name="formatter">Function to create a <c>string</c> message of the <paramref name="state" /> and <paramref name="exception" />.</param>
            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                var message = formatter(state, exception);

                if (exception == null)
                {
                    m_LoggingString.AppendLine(String.Format("{0}: <{1}>[{2}{3}] {4}", DateTime.Now.ToString("T"), logLevel.ToString(), String.IsNullOrWhiteSpace(m_SenderObjectTypeName) ? "<unknow>" : m_SenderObjectTypeName, String.IsNullOrWhiteSpace(m_SenderObjectName) ? String.Empty : "; " + m_SenderObjectName, message));
                }
                else
                {
                    m_LoggingString.AppendLine(String.Format("{0}: <{1}>[{2}{3}] {4}", DateTime.Now.ToString("T"), logLevel.ToString(), String.IsNullOrWhiteSpace(m_SenderObjectTypeName) ? "<unknow>" : m_SenderObjectTypeName, String.IsNullOrWhiteSpace(m_SenderObjectName) ? String.Empty : "; " + m_SenderObjectName, message));
                    m_LoggingString.AppendLine(exception.Message);
                }
            }
            #endregion

            #endregion
        }
        #endregion

        #region private members

        /// <summary>Represents the logging in its <see cref="System.StringBuilder"/> representation.
        /// </summary>
        private StringBuilder m_LoggingString = new StringBuilder();
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="TextLogFileGenerator"/> class.
        /// </summary>
        public TextLogFileGenerator()
        {
        }
        #endregion

        #region public methods

        /// <summary>Adds an <see cref="T:Microsoft.Extensions.Logging.ILoggerProvider" /> to the logging system.
        /// </summary>
        /// <param name="provider">The <see cref="T:Microsoft.Extensions.Logging.ILoggerProvider" />.</param>
        public void AddProvider(ILoggerProvider provider)
        {
            // nothing to do here
        }

        /// <summary>Creates a new <see cref="T:Microsoft.Extensions.Logging.ILogger" /> object.
        /// </summary>
        /// <param name="senderObjectTypeName">The object type name of the source object that adds the message.</param>
        /// <param name="senderObjectType">The type of the source object that adds the message.</param>
        /// <param name="senderObjectName">The name of the source object that adds the message.</param>
        /// <param name="channel">A specific channel, i.e. a name or category, for example <c>YieldCurveConstruction</c> etc.</param>
        /// <returns>A new <see cref="T:Microsoft.Extensions.Logging.ILogger" /> instance.</returns>
        public ILogger CreateLogger(string senderObjectTypeName, Type senderObjectType, string senderObjectName = "", string channel = "")
        {
            return new LoggerStream(senderObjectTypeName, senderObjectType, senderObjectName);
        }

        /// <summary>Creates a new <see cref="T:Microsoft.Extensions.Logging.ILogger" /> instance.
        /// </summary>
        /// <param name="categoryName">The category name for messages produced by the logger.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.Logging.ILogger" />.</returns>
        public ILogger CreateLogger(string categoryName)
        {
            return new LoggerStream(senderObjectTypeName: categoryName, senderObjectType: null);
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // nothing to do here
        }

        /// <summary>Writes a log entry.
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="logLevel">Entry will be written on this level.</param>
        /// <param name="eventId">Id of the event.</param>
        /// <param name="state">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">Function to create a <c>string</c> message of the <paramref name="state" /> and <paramref name="exception" />.</param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var message = formatter(state, exception);
            if (exception == null)
            {
                m_LoggingString.AppendLine(String.Format("{0}: <{1}> {2}", DateTime.Now.ToString("T"), logLevel.ToString(), message));
            }
            else
            {
                m_LoggingString.AppendLine(String.Format("{0}: <{1}> {2}", DateTime.Now.ToString("T"), logLevel.ToString(), message));
                m_LoggingString.AppendLine(exception.Message);
            }
        }

        /// <summary>Checks if the given <paramref name="logLevel" /> is enabled.
        /// </summary>
        /// <param name="logLevel">level to be checked.</param>
        /// <returns><c>true</c> if enabled.</returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        /// <summary>Begins a logical operation scope.
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="state">The identifier for the scope.</param>
        /// <returns>An IDisposable that ends the logical operation scope on dispose.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public IDisposable BeginScope<TState>(TState state)
        {
            return new LoggerStream(senderObjectTypeName: "<unknown>", senderObjectType: null);
        }
        #endregion

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return m_LoggingString.ToString();
        }
    }
}
