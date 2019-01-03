///*
//   Copyright (C) 2011-2013 Markus Wendt
//   All rights reserved.
 
//   Redistribution and use in source and binary forms, including commercial applications, with or without modification, 
//   are permitted provided that the following conditions are met: 
 
//   1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer. 
//      Altered source versions must be plainly marked as such, and must not be misrepresented as being the original software.

//   2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following 
//      disclaimer in the documentation and/or other materials provided with the distribution. 

//   3. If you use this software in a product, an acknowledgment (see the following) in the product documentation is required. 
//      The end-user documentation included with the redistribution, if any, must include the following acknowledgment: 

//      "Dodoni.net (http://www.dodoni-project.net/) Copyright (C) 2011-2012 Markus Wendt" 

//      Alternately, this acknowledgment may appear in the software itself, if and wherever such third-party acknowledgments normally appear. 

//   4. Neither the name 'Dodoni.net' nor the names of its contributors may be used to endorse or promote products 
//      derived from this software without specific prior written permission. For written permission, please contact info<at>dodoni-project.net. 

//   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, 
//   BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
//   SHALL THE COPYRIGHT HOLDERS OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
//   DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
//   INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE 
//   OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 
//   For more information, please see http://www.dodoni-project.net/.
// */
//using System;
//using System.ComponentModel.Composition;

//using Dodoni.BasicComponents.Logging;

//namespace Dodoni.XLBasicComponents.Logging
//{
//    public static partial class Extensions
//    {
//        #region Ignore empty Excel cell

//        /// <summary>Represents the message type which indicates that a specific Excel cell has been ignored because it is empty.
//        /// </summary>
//        [Export(typeof(Logger.MessageType))]
//        public class IgnoreEmptyExcelCellInfoMessageType : Logger.MessageType.Info
//        {
//            /// <summary>A static instance of the Message Type.
//            /// </summary>
//            internal static Logger.MessageType.Info Value = new IgnoreEmptyExcelCellInfoMessageType();

//            /// <summary>Initializes a new instance of the <see cref="IgnoreEmptyExcelCellInfoMessageType" /> class.
//            /// </summary>
//            public IgnoreEmptyExcelCellInfoMessageType()
//                : base("Ignored empty excel cell", new Guid(0x9f228aec, 0x94d7, 0x47a5, 0x91, 0xad, 0xd8, 0xf5, 0xbc, 0xab, 0xe2, 0x27))
//            {
//            }
//        }

//        /// <summary>Adds a specific Info message indicating that a specific Excel cell has been ignored because it is empty.
//        /// </summary>
//        /// <param name="loggingStream">The <see cref="ILoggerStream"/> object.</param>
//        /// <param name="message">The message.</param>
//        /// <param name="exception">The exception raised by the source object that adds the message.</param>
//        public static void Add_Info_IgnoreEmptyExcelCell(this ILoggerStream loggingStream, string message = null, Exception exception = null)
//        {
//            loggingStream.Add(IgnoreEmptyExcelCellInfoMessageType.Value, message, exception);
//        }

//        /// <summary>Adds a specific Info message indicating that a specific Excel cell has been ignored because it is empty.
//        /// </summary>
//        /// <param name="logger">The <see cref="ILogger"/> object.</param>
//        /// <param name="message">The message.</param>
//        /// <param name="senderObjectTypeName">The object type name of the source object that adds the message.</param>
//        /// <param name="senderObjectType">The type of the source object that adds the message.</param>
//        /// <param name="senderObjectName">The name of the source object that adds the message.</param>
//        /// <param name="exception">The exception raised by the source object that adds the message.</param>
//        public static void Add_Info_IgnoreEmptyExcelCell(this ILogger logger, string message = "", string senderObjectTypeName = "", Type senderObjectType = null, string senderObjectName = "", Exception exception = null)
//        {
//            logger.Add(IgnoreEmptyExcelCellInfoMessageType.Value, message, senderObjectTypeName, senderObjectType, senderObjectName, exception);
//        }
//        #endregion

//        #region Excel Cell Drop-down List creation fails

//        /// <summary>Represents the message type which indicates that the Excel cell data validation fails, i.e. it was not possible to create a specific drop-down list for a specific Excel cell.
//        /// </summary>
//        [Export(typeof(Logger.MessageType))]
//        public class ExcelCellDropdownListFailsInfoMessageType : Logger.MessageType.Info
//        {
//            /// <summary>A static instance of the Message Type.
//            /// </summary>
//            internal static Logger.MessageType.Info Value = new ExcelCellDropdownListFailsInfoMessageType();

//            /// <summary>Initializes a new instance of the <see cref="ExcelCellDropdownListFailsInfoMessageType" /> class.
//            /// </summary>
//            public ExcelCellDropdownListFailsInfoMessageType()
//                : base("Excel cell validation failure", new Guid(0x1bd41881, 0xb9eb, 0x49bc, 0x9b, 0xf0, 0x94, 0x4e, 0x7a, 0xcd, 0x36, 0x6c))
//            {
//            }
//        }

//        /// <summary>Adds a specific Info message indicating that the Excel cell data validation fails, i.e. it was not possible to create a specific drop-down list for a specific Excel cell.
//        /// </summary>
//        /// <param name="loggingStream">The <see cref="ILoggerStream"/> object.</param>
//        /// <param name="message">The message.</param>
//        /// <param name="exception">The exception raised by the source object that adds the message.</param>
//        public static void Add_Info_ExcelCellDropdownListFails(this ILoggerStream loggingStream, string message = null, Exception exception = null)
//        {
//            if (ExcelLogger.AddOnDataAdviceFails == true)
//            {
//                loggingStream.Add(ExcelCellDropdownListFailsInfoMessageType.Value, message, exception);
//            }
//        }

//        /// <summary>Adds a specific Info message indicating that the Excel cell data validation fails, i.e. it was not possible to create a specific drop-down list for a specific Excel cell.
//        /// </summary>
//        /// <param name="logger">The <see cref="ILogger"/> object.</param>
//        /// <param name="message">The message.</param>
//        /// <param name="senderObjectTypeName">The object type name of the source object that adds the message.</param>
//        /// <param name="senderObjectType">The type of the source object that adds the message.</param>
//        /// <param name="senderObjectName">The name of the source object that adds the message.</param>
//        /// <param name="exception">The exception raised by the source object that adds the message.</param>
//        public static void Add_Info_ExcelCellDropdownListFails(this ILogger logger, string message = "", string senderObjectTypeName = "", Type senderObjectType = null, string senderObjectName = "", Exception exception = null)
//        {
//            if (ExcelLogger.AddOnDataAdviceFails == true)
//            {
//                logger.Add(ExcelCellDropdownListFailsInfoMessageType.Value, message, senderObjectTypeName, senderObjectType, senderObjectName, exception);
//            }
//        }
//        #endregion
//    }
//}