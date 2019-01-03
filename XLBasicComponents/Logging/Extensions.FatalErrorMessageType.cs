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
//        #region Excel Range Size Problem

//        /// <summary>Represents the message type which indicates that the size of the Excel range output can not be calculated, i.e. a <see cref="ExcelDna.Integration.XlCallException"/> has been thrown.
//        /// </summary>
//        [Export(typeof(Logger.MessageType))]
//        public class ExcelRangeSizeProblemFatalErrorMessageType : Logger.MessageType.FatalError
//        {
//            /// <summary>A static instance of the Message Type.
//            /// </summary>
//            internal static Logger.MessageType.FatalError Value = new ExcelRangeSizeProblemFatalErrorMessageType();

//            /// <summary>Initializes a new instance of the <see cref="ExcelRangeSizeProblemFatalErrorMessageType" /> class.
//            /// </summary>
//            public ExcelRangeSizeProblemFatalErrorMessageType()
//                : base("Excel range size problem", new Guid(0xbcaf5857, 0xcb2c, 0x4e99, 0xb4, 0xd2, 0x25, 0xa2, 0x94, 0xb6, 0x7e, 0xc0))
//            {
//            }
//        }

//        /// <summary>Adds a specific Info message indicating that the size of the Excel range output can not be calculated, i.e. a <see cref="ExcelDna.Integration.XlCallException"/> has been thrown.
//        /// </summary>
//        /// <param name="loggingStream">The <see cref="ILoggerStream"/> object.</param>
//        /// <param name="message">The message.</param>
//        /// <param name="exception">The exception raised by the source object that adds the message.</param>
//        public static void Add_FatalError_ExcelRangeSizeProblem(this ILoggerStream loggingStream, string message = null, Exception exception = null)
//        {
//            loggingStream.Add(ExcelRangeSizeProblemFatalErrorMessageType.Value, message, exception);
//        }

//        /// <summary>Adds a specific Info message indicating that the size of the Excel range output can not be calculated, i.e. a <see cref="ExcelDna.Integration.XlCallException"/> has been thrown.
//        /// </summary>
//        /// <param name="logger">The <see cref="ILogger"/> object.</param>
//        /// <param name="message">The message.</param>
//        /// <param name="senderObjectTypeName">The object type name of the source object that adds the message.</param>
//        /// <param name="senderObjectType">The type of the source object that adds the message.</param>
//        /// <param name="senderObjectName">The name of the source object that adds the message.</param>
//        /// <param name="exception">The exception raised by the source object that adds the message.</param>
//        public static void Add_FatalError_ExcelRangeSizeProblem(this ILogger logger, string message = "", string senderObjectTypeName = "", Type senderObjectType = null, string senderObjectName = "", Exception exception = null)
//        {
//            logger.Add(ExcelRangeSizeProblemFatalErrorMessageType.Value, message, senderObjectTypeName, senderObjectType, senderObjectName, exception);
//        }
//        #endregion
//    }
//}