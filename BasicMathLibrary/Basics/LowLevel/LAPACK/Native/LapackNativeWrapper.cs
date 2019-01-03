/*
   Copyright (c) 2011-2018 Markus Wendt

 This software is provided 'as-is', without any express or implied warranty. In no event will the authors be held liable for 
 any damages arising from the use of this software. 

 Permission is granted to anyone to use this software for any purpose, including commercial applications, and to alter it and 
 redistribute it freely, subject to the following restrictions: 
   1.The origin of this software must not be misrepresented; you must not claim that you wrote the original software. If you 
     use this software in a product, an acknowledgment in the product documentation would be appreciated but is not required.
   2.Altered source versions must be plainly marked as such, and must not be misrepresented as being the original software.
   3.This notice may not be removed or altered from any source distribution.

 Please see http://www.dodoni-project.net/ for more information concerning the Dodoni.net project.
 */
using System;
using System.Runtime.InteropServices;

using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.Basics.LowLevel.Native
{
    /// <summary>Serves as native wrapper for the Lapack functions, see http://www.netlib.org/lapack/index.html.
    /// </summary>
    internal partial class LapackNativeWrapper : LAPACK.ILibrary
    {
        #region private (const) members

        /// <summary>The name of the external Lapack dll.
        /// </summary>
        private const string sm_DllName = "libLAPACK.dll";

        /// <summary>The calling convention of the external Lapack dll.
        /// </summary>
        private const CallingConvention sm_CallingConvention = CallingConvention.Cdecl;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="LapackNativeWrapper" /> class.
        /// </summary>
        internal LapackNativeWrapper()
        {
            Name =  LongName = new IdentifierString("LAPACK");
            LinearEquations = new LapackLinearEquations(this);
            EigenValues = new LapackEigenvalues(this);
            AuxiliaryRoutines = new LapackAuxiliaryUtilityRoutines(this);
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the current instance.
        /// </summary>
        /// <value>The language independent name of the current instance.</value>
        public IdentifierString Name
        {
            get;
            private set;
        }

        /// <summary>Gets the long name of the current instance.
        /// </summary>
        /// <value>The (perhaps) language dependent long name of the current instance.</value>
        public IdentifierString LongName
        {
            get;
            private set;
        }
        #endregion

        #region ILibrary Members

        /// <summary>Provides routines that are related to Least Square and Eigenvalue calculations.
        /// </summary>
        /// <value>Provides routines that are related to Least Square and Eigenvalue calculations.</value>
        public LapackEigenvalues EigenValues
        {
            get;
            private set;
        }

        /// <summary>Provides routines that are related to Linear Equations.
        /// </summary>
        /// <value>Provides routines that are related to Linear Equations.</value>
        public LapackLinearEquations LinearEquations
        {
            get;
            private set;
        }

        /// <summary>Provides auxiliary and utility routines of the LAPACK library.
        /// </summary>
        /// <value>Provides auxiliary and utility routines.</value>
        public LapackAuxiliaryUtilityRoutines AuxiliaryRoutines
        {
            get;
            private set;
        }
        #endregion

        #region IAnnotatable Members

        /// <summary>Gets a value indicating whether the annotation is read-only.
        /// </summary>
        /// <value><c>true</c> if the annotation of this instance is readonly; otherwise, <c>false</c>.</value>
        public bool HasReadOnlyAnnotation
        {
            get { return true; }
        }

        /// <summary>Gets a description of the LAPACK Library.
        /// </summary>
        /// <value>The description of the LAPACK Library.</value>
        public string Annotation
        {
            get { return String.Format(LowLevelMathConfigurationResources.DescriptionLapack, sm_DllName); }
        }
        #endregion

        #endregion

        #region public methods

        #region IAnnotatable Members

        /// <summary>Sets the annotation of the current instance.
        /// </summary>
        /// <param name="annotation">The annotation.</param>
        /// <returns>A value indicating whether the <see cref="Annotation" /> has been changed.</returns>
        public bool TrySetAnnotation(string annotation)
        {
            return false;
        }
        #endregion

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return Name.String;
        }
        #endregion

        #region private methods

        /// <summary>Checks for error of a specified LAPACK function.
        /// </summary>
        /// <param name="info">The return value of a specified LAPACK function.</param>
        /// <param name="functionName">The name of the LAPACK function.</param>
        /// <exception cref="Exception">Thrown, if <paramref name="info"/> != <c>0</c>.</exception>
        private void CheckForError(int info, string functionName)
        {
            if (info != 0)
            {
                throw new Exception(String.Format("Return value {0} in LAPACK function {1}.", info, functionName));
            }
        }
        #endregion
    }
}