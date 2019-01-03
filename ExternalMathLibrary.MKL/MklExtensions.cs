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
using System.Runtime.InteropServices;

namespace Dodoni.MathLibrary.Basics.LowLevel.Native
{
    /// <summary>Provides extension methods for Intel's MKL Library.
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static partial class MklExtensions
    {
        #region private (const) members

        /// <summary>The name of the external Lapack dll.
        /// </summary>
        private const string sm_DllName = "libLAPACK.dll";

        /// <summary>The calling convention of the external Lapack dll.
        /// </summary>
        private const CallingConvention sm_CallingConvention = CallingConvention.Cdecl;
        #endregion

        #region private methods

        /// <summary>Checks for error of a specified LAPACK function.
        /// </summary>
        /// <param name="info">The return value of a specified LAPACK function.</param>
        /// <param name="functionName">The name of the LAPACK function.</param>
        /// <exception cref="Exception">Thrown, if <paramref name="info"/> != <c>0</c>.</exception>
        private static void CheckForError(int info, string functionName)
        {
            if (info != 0)
            {
                throw new Exception(String.Format("Return value {0} in LAPACK function {1}.", info, functionName));
            }
        }

        /// <summary>Gets the <see cref="System.Char"/> representation of a value indicating whether the lower or upper triangular part of a matrix should take into account.
        /// </summary>
        /// <param name="triangularMatrixType">The type of the triangular matrix.</param>
        /// <returns>The <see cref="System.Char"/> representation of <paramref name="triangularMatrixType"/>.</returns>
        private static char GetUplo(BLAS.TriangularMatrixType triangularMatrixType)
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

        /// <summary>Gets the <see cref="System.Char"/> representation of a value indicating whether to take into account the transpose or hermite matrix etc.
        /// </summary>
        /// <param name="transposeState">The state of the matrix.</param>
        /// <returns>The <see cref="System.Char"/> representation of <paramref name="transposeState"/>.</returns>
        private static char GetTrans(BLAS.MatrixTransposeState transposeState)
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
        #endregion
    }
}