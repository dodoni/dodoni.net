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
    /// <summary>Represents a wrapper for BLAS operation with respect to the C-interface (CBLAS). See http://www.netlib.org/blas for further information.
    /// </summary>
    [Export(typeof(BLAS.ILibrary))]
    public partial class CBlasNativeWrapper : BLAS.ILibrary
    {
        #region public const members

        /// <summary>The name of the dll needed for the Basic Linear Algebra Subprograms (BLAS), Fortran interface.
        /// </summary>
        public const string dllName = "libCBLAS.dll";

        /// <summary>The calling convention of the external BLAS dll.
        /// </summary>
        public const CallingConvention callingConvention = CallingConvention.Cdecl;
        #endregion

        #region private members

        /// <summary>The name of the Library.
        /// </summary>
        private IdentifierString m_Name;

        /// <summary>Level 1 operations.
        /// </summary>
        private ILevel1BLAS m_Level1;

        /// <summary>Level 2 operations.
        /// </summary>
        private ILevel2BLAS m_Level2;

        /// <summary>Level 3 operations.
        /// </summary>
        private ILevel3BLAS m_Level3;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="CBlasNativeWrapper" /> class.
        /// </summary>
        public CBlasNativeWrapper()
        {
            m_Name = new IdentifierString("CBLAS");
            m_Level1 = new Level1CBLAS();
            m_Level2 = new Level2CBLAS();
            m_Level3 = new Level3CBLAS();
        }
        #endregion

        #region protected constructors

        /// <summary>Initializes a new instance of the <see cref="CBlasNativeWrapper" /> class.
        /// </summary>
        /// <param name="name">The name of the Library.</param>
        /// <param name="level1">The implementation of level 1 BLAS functions.</param>
        /// <param name="level2">The implementation of level 2 BLAS functions.</param>
        /// <param name="level3">The implementation of level 3 BLAS functions.</param>
        protected CBlasNativeWrapper(IdentifierString name, ILevel1BLAS level1, ILevel2BLAS level2, ILevel3BLAS level3)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            m_Name = name;
            if (level1 == null)
            {
                throw new ArgumentNullException("level1");
            }
            m_Level1 = level1;
            if (level2 == null)
            {
                throw new ArgumentNullException("level2");
            }
            m_Level2 = level2;
            if (level3 == null)
            {
                throw new ArgumentNullException("level3");
            }
            m_Level3 = level3;
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the BLAS Library.
        /// </summary>
        /// <value>The name of the BLAS Library.</value>
        public IdentifierString Name
        {
            get { return m_Name; }
        }

        /// <summary>Gets the long name of the BLAS Library.
        /// </summary>
        /// <value>The long name of the BLAS Library.</value>
        public IdentifierString LongName
        {
            get { return m_Name; }
        }
        #endregion

        #region IAnnotatable Members

        /// <summary>Gets a value indicating whether the annotation is read-only.
        /// </summary>
        /// <value>
        /// <c>true</c> if the annotation of this instance is readonly; otherwise, <c>false</c>.</value>
        bool IAnnotatable.HasReadOnlyAnnotation
        {
            get { return true; }
        }

        /// <summary>Gets a description of the BLAS Library.
        /// </summary>
        /// <value>The description of the BLAS Library.</value>
        public virtual string Annotation
        {
            get { return String.Format(ResourceFile.Description, dllName); }
        }
        #endregion

        #region ILibrary Members

        /// <summary>Gets Level 1 operations, i.e. vector operations.
        /// </summary>
        /// <value>Level 1 operations.</value>
        public ILevel1BLAS Level1
        {
            get { return m_Level1; }
        }

        /// <summary>Gets Level 2 operations, i.e. matrix-vector operations.
        /// </summary>
        /// <value>Level 2 operations.</value>
        public ILevel2BLAS Level2
        {
            get { return m_Level2; }
        }

        /// <summary>Gets Level 3 operations, i.e. matrix-matrix operations.
        /// </summary>
        /// <value>Level 3 operations.</value>
        public ILevel3BLAS Level3
        {
            get { return m_Level3; }
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