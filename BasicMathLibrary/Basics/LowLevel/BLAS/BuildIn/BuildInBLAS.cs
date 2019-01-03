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

using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.Basics.LowLevel.BuildIn
{
    /// <summary>Serves as managed code implementation of the BLAS library, see http://www.netlib.org/blas.
    /// </summary>
    /// <remarks>It is recommended to use wrapper of a native code implementation.</remarks>
    internal class BuildInBLAS : BLAS.ILibrary
    {
        #region private members

        /// <summary>The name of the BLAS Library.
        /// </summary>
        private IdentifierString m_Name;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="BuildInBLAS" /> class.
        /// </summary>
        internal BuildInBLAS()
        {
            m_Name = new IdentifierString("Build-In BLAS");

            Level1 = new BuildInLevel1BLAS();
            Level2 = new BuildInLevel2BLAS();
            Level3 = new BuildInLevel3BLAS();
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
        /// <value><c>true</c> if the annotation of this instance is readonly; otherwise, <c>false</c>.</value>
        bool IAnnotatable.HasReadOnlyAnnotation
        {
            get { return true; }
        }

        /// <summary>Gets a description of the BLAS Library.
        /// </summary>
        /// <value>The description of the BLAS Library.</value>
        public string Annotation
        {
            get { return LowLevelMathConfigurationResources.DescriptionBlasBuildIn; }
        }
        #endregion

        #region ILibrary Members

        /// <summary>Gets Level 1 operations, i.e. vector operations.
        /// </summary>
        /// <value>level 1 operations.</value>
        public ILevel1BLAS Level1
        {
            get;
            private set;
        }

        /// <summary>Gets Level 2 operations, i.e. matrix-vector operations.
        /// </summary>
        /// <value>Level 2 operations.</value>
        public ILevel2BLAS Level2
        {
            get;
            private set;
        }

        /// <summary>Gets Level 3 operations, i.e. matrix-matrix operations.
        /// </summary>
        /// <value>Level 3 operations.</value>
        public ILevel3BLAS Level3
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