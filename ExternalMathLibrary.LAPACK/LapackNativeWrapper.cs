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
    /// <summary>Serves as native wrapper for the Lapack functions, see http://www.netlib.org/lapack/index.html.
    /// </summary>
    [Export(typeof(LAPACK.ILibrary))]
    public class LapackNativeWrapper : LAPACK.ILibrary
    {
        #region internal (const) members

        /// <summary>The name of the external Lapack dll.
        /// </summary>
        internal const string dllName = "libLAPACK.dll";

        /// <summary>The calling convention of the external Lapack dll.
        /// </summary>
        internal const CallingConvention CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="LapackNativeWrapper" /> class.
        /// </summary>
        public LapackNativeWrapper()
        {
            Name = LongName = new IdentifierString("LAPACK");
            LinearEquations = new LapackLinearEquations(
                MatrixConditionalNumbers.Create(),
                MatrixEquilibration.Create(),
                MatrixFactorization.Create(),
                MatrixInversion.Create(),
                Solver.Create(),
                ErrorEstimationSolver.Create());

            EigenValues = new LapackEigenvalues(
                GeneralizedNonsymmetricEigenvalueProblems.Create(),
                GeneralizedSymmetricEigenvalueProblems.Create(),
                NonSymmetricEigenvalueProblems.Create(),
                SymmetricEigenvalueProblems.Create(),
                SingularValueDecomposition.Create(),
                GeneralizedSingularValueDecomposition.Create(),
                LinearLeastSquaresProblems.Create());

            AuxiliaryRoutines = new LapackAuxiliaryUtilityRoutines(AuxiliaryUtilityRoutines.Create());
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
        public string Annotation =>
#if LOWER_CASE_UNDERSCORE                
            String.Format(LapackResources.DescriptionLapack, LapackResources.LapackVersion, dllName, LapackResources.LowerCaseUnderscoreEntryPoints);
#else           
            String.Format(LapackResources.DescriptionLapack, LapackResources.LapackVersion, sm_DllName, LapackResources.UpperCaseEntryPoints);
#endif
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
        internal static void CheckForError(int info, string functionName)
        {
            if (info != 0)
            {
                throw new Exception(String.Format("Return value {0} in LAPACK function {1}.", info, functionName));
            }
        }
        #endregion

        #region internal (static) methods

        /// <summary>Gets the <see cref="System.Char"/> representation of a value indicating whether the lower or upper triangular part of a matrix should take into account.
        /// </summary>
        /// <param name="triangularMatrixType">The type of the triangular matrix.</param>
        /// <returns>The <see cref="System.Char"/> representation of <paramref name="triangularMatrixType"/>.</returns>
        internal static char GetUplo(BLAS.TriangularMatrixType triangularMatrixType)
        {
            switch (triangularMatrixType)
            {
                case BLAS.TriangularMatrixType.LowerTriangularMatrix:
                    return 'L';
                case BLAS.TriangularMatrixType.UpperTriangularMatrix:
                    return 'U';
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>Gets the <see cref="System.Char"/> representation of a value indicating whether a specific LAPACK function should be applied to a left or right side (matrix) operation.
        /// </summary>
        /// <param name="side">The side.</param>
        /// <returns>The <see cref="System.Char"/> representation of <paramref name="side"/>.</returns>
        internal static char GetSide(LAPACK.Side side)
        {
            switch (side)
            {
                case LAPACK.Side.Left:
                    return 'L';
                case LAPACK.Side.Right:
                    return 'R';
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>Gets the <see cref="System.Char"/> representation of a value indicating whether to take into account the transpose or hermite matrix etc.
        /// </summary>
        /// <param name="transposeState">The state of the matrix.</param>
        /// <returns>The <see cref="System.Char"/> representation of <paramref name="transposeState"/>.</returns>
        internal static char GetTrans(BLAS.MatrixTransposeState transposeState)
        {
            switch (transposeState)
            {
                case BLAS.MatrixTransposeState.NoTranspose:
                    return 'N';
                case BLAS.MatrixTransposeState.Transpose:
                    return 'T';
                case BLAS.MatrixTransposeState.Hermite:
                    return 'H';
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>Gets the <see cref="System.Char"/> representation of a value indicating the type of the matrix norm.
        /// </summary>
        /// <param name="matrixNormType">The matrix norm type.</param>
        /// <returns>The <see cref="System.Char"/> representation of <paramref name="matrixNormType"/>.</returns>
        internal static char GetMatrixNormType(MatrixNormType matrixNormType)
        {
            switch (matrixNormType)
            {
                case MatrixNormType.LargestAbsoluteValue:
                    return 'M';
                case MatrixNormType.OneNorm:
                    return '1';
                case MatrixNormType.Infinity:
                    return 'I';
                case MatrixNormType.Frobenius:
                    return 'F';
                default: throw new NotImplementedException();
            }
        }
        #endregion
    }
}