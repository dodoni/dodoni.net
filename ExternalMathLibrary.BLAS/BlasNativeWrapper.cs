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
    /// <summary>Represents a wrapper for BLAS operations with respect to the Fortran interface. See http://www.netlib.org/blas for further information.
    /// </summary>
    [Export(typeof(BLAS.ILibrary))]
    public partial class BlasNativeWrapper : BLAS.ILibrary
    {
        #region public const members

        /// <summary>The name of the dll needed for the Basic Linear Algebra Subprograms (BLAS), Fortran interface.
        /// </summary>
        public const string dllName = "libBLAS.dll";

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

        /// <summary>Initializes a new instance of the <see cref="BlasNativeWrapper" /> class.
        /// </summary>
        public BlasNativeWrapper()
        {
            m_Name = new IdentifierString("BLAS");
            m_Level1 = new Level1BLAS();
            m_Level2 = new Level2BLAS();
            m_Level3 = new Level3BLAS();
        }
        #endregion

        #region protected constructors

        /// <summary>Initializes a new instance of the <see cref="BlasNativeWrapper" /> class.
        /// </summary>
        /// <param name="name">The name of the Library.</param>
        /// <param name="level1">The implementation of level 1 BLAS functions.</param>
        /// <param name="level2">The implementation of level 2 BLAS functions.</param>
        /// <param name="level3">The implementation of level 3 BLAS functions.</param>
        protected BlasNativeWrapper(IdentifierString name, ILevel1BLAS level1, ILevel2BLAS level2, ILevel3BLAS level3)
        {
            m_Name = name ?? throw new ArgumentNullException(nameof(name));
            m_Level1 = level1 ?? throw new ArgumentNullException(nameof(level1));
            m_Level2 = level2 ?? throw new ArgumentNullException(nameof(level2));
            m_Level3 = level3 ?? throw new ArgumentNullException(nameof(level3));
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
        public virtual string Annotation =>
#if LOWER_CASE_UNDERSCORE
        String.Format(ResourceFile.Description, dllName, ResourceFile.LowerCaseUnderscoreEntryPoints);
#else
        String.Format(ResourceFile.Description,  dllName, ResourceFile.UpperCaseEntryPoints);
#endif
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