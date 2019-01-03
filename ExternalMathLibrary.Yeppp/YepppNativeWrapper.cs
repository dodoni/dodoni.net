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
using System.IO;
using System.Composition;
using System.Runtime.InteropServices;

using Dodoni.BasicComponents;
using Dodoni.MathLibrary.Basics.LowLevel.BuildIn;

namespace Dodoni.MathLibrary.Basics.LowLevel.Native.Yeppp
{
    /// <summary>Provides mathematical functions with respect to Yeppp!.
    /// </summary>
    [Export(typeof(VectorUnit.ILibrary))]
    public class YepppNativeWrapper : VectorUnit.ILibrary
    {
        #region public (const) members

        /// <summary>The name of the external dll.
        /// </summary>
        public const string dllName = "yeppp.dll"; // we do not use "libYeppp.dll" etc., because the .net wrapper provided by Yeppp! assume a native code dll with name "yeppp.dll"

        /// <summary>The calling convention of the external dll.
        /// </summary>
        public const CallingConvention callingConvention = CallingConvention.Cdecl;
        #endregion

        #region private function import

        [DllImport(dllName, ExactSpelling = true, CallingConvention = callingConvention, EntryPoint = "yepLibrary_Init")]
        private static extern Status yepLibrary_Init();
        #endregion

        #region private members

        /// <summary>The name of the Library.
        /// </summary>
        private IdentifierString m_Name;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="YepppNativeWrapper" /> class.
        /// </summary>
        public YepppNativeWrapper()
        {
            m_Name = new IdentifierString("Yeppp!");
            Basics = new YepppVectorUnitBasics();
            Special = new BuildInVectorUnitSpecial();
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the Vector Unit Library.
        /// </summary>
        /// <value>The name of the Vector Unit Library.</value>
        public IdentifierString Name
        {
            get { return m_Name; }
        }
        /// <summary>Gets the long name of the Vector Unit Library.
        /// </summary>
        /// <value>The long name of the Vector Unit Library.</value>
        public IdentifierString LongName
        {
            get { return m_Name; }
        }
        #endregion

        #region IAnnotatable Members

        /// <summary>Gets a value indicating whether the annotation is read-only.
        /// </summary>
        /// <value><c>true</c> if the annotation of this instance is readonly; otherwise, <c>false</c>.</value>
        bool IAnnotatable.HasReadOnlyAnnotation
        {
            get { return true; }
        }

        /// <summary>Gets a description of the Vector Unit Library.
        /// </summary>
        /// <value>The description of the Vector Unit Library.</value>
        public string Annotation
        {
            get { return String.Format(ResourceFile.DescriptionVectorUnit, YepppNativeWrapper.dllName); }
        }
        #endregion

        #region ILibrary Members

        /// <summary>Gets basic mathematical functions.
        /// </summary>
        /// <value>Provides basic mathematical functions.</value>
        public IVectorUnitBasics Basics
        {
            get;
            private set;
        }

        /// <summary>Gets special mathematical functions.
        /// </summary>
        /// <value>Provides special mathematical functions.</value>
        public IVectorUnitSpecial Special
        {
            get;
            private set;
        }
        #endregion

        #endregion

        #region public methods

        #region IAnnotatable Members

        /// <summary>Sets the annotation of the current instance.
        /// </summary>
        /// <param name="annotation">The annotation.</param>
        /// <returns>A value indicating whether the <see cref="Annotation" /> has been changed.</returns>
        bool IAnnotatable.TrySetAnnotation(string annotation)
        {
            return false;
        }
        #endregion

        #region ILibrary Members

        /// <summary>Initializes the Library.
        /// </summary>
        /// <remarks>Call this method before using the Library the first time.</remarks>
        public void Initialize()
        {
            InitLibrary();
        }
        #endregion

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return m_Name.String;
        }
        #endregion

        #region internal static methods

        /// <summary>Initialize the Yeppp! Library.
        /// </summary>
        internal static void InitLibrary()
        {
            var status = yepLibrary_Init();
            if (status != Status.Ok)
            {
                throw new SystemException("Failed to initialize Yeppp! library");
            }
        }

        /// <summary>Gets a specific <see cref="Exception"/> object for a specified <see cref="Status"/>.
        /// </summary>
        /// <param name="status">The specified <see cref="Status"/> object.</param>
        /// <returns>A specific <see cref="Exception"/> object.</returns>
        internal static Exception GetException(Status status)
        {
            switch (status)
            {
                case Status.Ok:
                    return null;
                case Status.NullPointer:
                    return new NullReferenceException();

                case Status.MisalignedPointer:
                    return new DataMisalignedException();

                case Status.InvalidArgument:
                    return new ArgumentException();

                case Status.InvalidData:
                    return new InvalidDataException();

                case Status.InvalidState:
                    return new InvalidOperationException();

                case Status.UnsupportedHardware:
                case Status.UnsupportedSoftware:
                    return new PlatformNotSupportedException();

                case Status.InsufficientBuffer:
                    return new InternalBufferOverflowException();

                case Status.OutOfMemory:
                    return new OutOfMemoryException();

                case Status.SystemError:
                    return new SystemException();

                default:
                    return new Exception(String.Format("State: {0}.", unchecked((uint)status)));
            }
        }
        #endregion
    }
}