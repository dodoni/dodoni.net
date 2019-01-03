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
using System.Numerics;

using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.Basics.LowLevel
{
    /// <summary>Serves as dummy implementation for LAPACK routines.
    /// </summary>   
    /// <remarks>No managed implementation available yet.</remarks>
    internal class BuildInDummyLAPACK : LAPACK.ILibrary
    {
        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="BuildInDummyLAPACK"/> class.
        /// </summary>
        internal BuildInDummyLAPACK()
        {
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the current instance.
        /// </summary>
        /// <value>The language independent name of the current instance.</value>
        public IdentifierString Name => "Dummy LAPACK".ToIdentifierString();

        /// <summary>Gets the long name of the current instance.
        /// </summary>
        /// <value>The (perhaps) language dependent long name of the current instance.</value>
        public IdentifierString LongName => "Dummy LAPACK".ToIdentifierString();
        #endregion

        #region IAnnotatable Members

        /// <summary>Gets a value indicating whether the annotation is read-only.
        /// </summary>
        /// <value>
        /// <c>true</c> if the annotation of this instance is readonly; otherwise, <c>false</c>.
        /// </value>
        public bool HasReadOnlyAnnotation => true;

        /// <summary>Gets the annotation of the current instance.
        /// </summary>
        /// <value>The annotation of the current instance.</value>
        public string Annotation => "Dummy LAPACK";
        #endregion

        #region LAPACK.ILibrary Members

        /// <summary>Provides routines that are related to Least Square and Eigenvalue calculations.
        /// </summary>
        /// <value>Provides routines that are related to Least Square and Eigenvalue calculations.</value>
        public LapackEigenvalues EigenValues
        {
            get
            {
                var buildInDummy = new BuildInDummyLapackEigenvalues();
                return new LapackEigenvalues(buildInDummy, buildInDummy, buildInDummy, buildInDummy, buildInDummy, buildInDummy, buildInDummy);
            }
        }

        /// <summary>Provides routines that are related to Linear Equations.
        /// </summary>
        /// <value>Provides routines that are related to Linear Equations.</value>
        public LapackLinearEquations LinearEquations
        {
            get
            {
                var buildInDummy = new BuildInDummyLapackLinearEquations();
                return new LapackLinearEquations(buildInDummy, buildInDummy, buildInDummy, buildInDummy, buildInDummy, buildInDummy);
            }
        }

        /// <summary>Provides auxiliary and utility routines of the LAPACK library.
        /// </summary>
        /// <value>Provides auxiliary and utility routines.</value>
        public LapackAuxiliaryUtilityRoutines AuxiliaryRoutines => LapackAuxiliaryUtilityRoutines.Create(new BuildInDummyLapackAuxiliaryUtilityRoutines());
        #endregion

        #endregion

        #region public methods

        #region IAnnotatable Members

        /// <summary>Sets the annotation of the current instance.
        /// </summary>
        /// <param name="annotation">The annotation.</param>
        /// <returns>A value indicating whether the <see cref="P:Dodoni.BasicComponents.IAnnotatable.Annotation" /> has been changed.</returns>
        public bool TrySetAnnotation(string annotation)
        {
            return false;
        }
        #endregion

        #endregion
    }
}