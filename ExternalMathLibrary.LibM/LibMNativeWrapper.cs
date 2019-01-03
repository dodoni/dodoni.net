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
using System.Composition;
using System.Runtime.InteropServices;
using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.Basics.LowLevel.Native
{
    /// <summary>Provides mathematical functions with respect to AMD's LibM Library.
    /// </summary>
    [Export(typeof(VectorUnit.ILibrary))]
    public class LibMNativeWrapper : MarshalByRefObject, VectorUnit.ILibrary
    {
        #region public (const) members

        /// <summary>The name of the external dll.
        /// </summary>
        public const string dllName = "libAMDlibm.dll";

        /// <summary>The calling convention of the external dll.
        /// </summary>
        public const CallingConvention callingConvention = CallingConvention.Cdecl;
        #endregion

        #region private members

        /// <summary>The name of the Library.
        /// </summary>
        private IdentifierString m_Name;

        /// <summary>The vector mathematical functions.
        /// </summary>
        private LibMVectorMathematicalFunctions m_VectorMathematicalFunctions;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="LibMNativeWrapper" /> class.
        /// </summary>
        public LibMNativeWrapper()
        {
            m_Name = new IdentifierString("LibM");
            m_VectorMathematicalFunctions = new LibMVectorMathematicalFunctions();
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
            get { return String.Format(ResourceFile.DescriptionVectorUnit, LibMNativeWrapper.dllName); }
        }
        #endregion

        #region ILibrary Members

        /// <summary>Gets basic mathematical functions.
        /// </summary>
        /// <value>Provides basic mathematical functions.</value>
        public IVectorUnitBasics Basics
        {
            get { return m_VectorMathematicalFunctions; }
        }

        /// <summary>Gets special mathematical functions.
        /// </summary>
        /// <value>Provides special mathematical functions.</value>
        public IVectorUnitSpecial Special
        {
            get { return m_VectorMathematicalFunctions; }
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
            // nothing to do here
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
    }
}