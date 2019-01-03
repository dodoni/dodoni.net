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

namespace Dodoni.MathLibrary.Basics.LowLevel
{
    /// <summary>Provides methods and constants for the CBLAS interface.
    /// </summary>
    /// <remarks>The enumerations are taken from the C interface of (C-) BLAS, see http://www.netlib.org/blas. </remarks>
    public static class CBLAS
    {
        #region nested enumerations

        /// <summary>Provides the representation of the internal matrix data.
        /// </summary>
        public enum Order
        {
            /// <summary>Row-major, i.e. row-by-row.
            /// </summary>
            RowMajor = 101,

            /// <summary>Column-major, i.e. column-by-column.
            /// </summary>
            ColumnMajor = 102
        }

        /// <summary>Represents the states of the property 'transpose' for a specific matrix.
        /// </summary>
        public enum Transpose
        {
            /// <summary>No transpose.
            /// </summary>
            NoTranspose = 111,

            /// <summary>Transpose.
            /// </summary>
            Transpose = 112,

            /// <summary>Transpose and conjugate each element.
            /// </summary>
            ConjugateTranspose = 113
        }

        /// <summary>The representation of a triangular matrix.
        /// </summary>
        public enum UpLo
        {
            /// <summary>A upper triangular matrix.
            /// </summary>
            Upper = 121,

            /// <summary>A lower triangular matrix.
            /// </summary>
            Lower = 122
        }

        /// <summary>Represents the type of diagonal elements of a specified matrix.
        /// </summary>
        public enum Diag
        {
            /// <summary>A non-unit diagonal matrix.
            /// </summary>
            NonUnit = 131,

            /// <summary>A unit diagonal matrix.
            /// </summary>
            Unit = 132
        }

        /// <summary>Represents the side of a specific matrix or vector operation.
        /// </summary>
        public enum Side
        {
            /// <summary>On the left side.
            /// </summary>
            Left = 141,

            /// <summary>On the right side.
            /// </summary>
            Right = 142
        }
        #endregion

        #region public static methods

        /// <summary>Gets the <see cref="CBLAS.Transpose"/> representation of a specified <see cref="BLAS.MatrixTransposeState"/> object.
        /// </summary>
        /// <param name="matrixTransposeState">The <see cref="BLAS.MatrixTransposeState"/> object.</param>
        /// <returns>The <see cref="CBLAS.Transpose"/> representation of <paramref name="matrixTransposeState"/>.</returns>
        /// <exception cref="System.NotImplementedException">Thrown if the transformation is not defined.</exception>
        public static CBLAS.Transpose AsCblasTranspose(this BLAS.MatrixTransposeState matrixTransposeState)
        {
            switch (matrixTransposeState)
            {
                case BLAS.MatrixTransposeState.NoTranspose: return CBLAS.Transpose.NoTranspose;
                case BLAS.MatrixTransposeState.Transpose: return CBLAS.Transpose.Transpose;
                case BLAS.MatrixTransposeState.Hermite: return CBLAS.Transpose.ConjugateTranspose;
                default:
                    throw new NotImplementedException(matrixTransposeState.ToString());
            }
        }
        #endregion
    }
}