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
using System.Numerics;

namespace Dodoni.MathLibrary.Basics.LowLevel
{
    public partial class LapackAuxiliaryUtilityRoutines
    {
        /// <summary>Provides auxiliary and utility routines related to matrices, for example matrix norm functions.
        /// </summary>
        public interface IMatrix
        {
            /// <summary>Returns the value of the 1-norm, Frobenius norm, infinity-norm, or the largest absolute value of any element of general rectangular matrix.
            /// </summary>
            /// <param name="matrixNormType">The type of the matrix norm.</param>
            /// <param name="m">Th enumber of rows.</param>
            /// <param name="n">The number of columns.</param>
            /// <param name="a">The <paramref name="m"/>-by-<paramref name="n"/> dense matrix provided column-by-column.</param>
            /// <param name="work">A workspace array which is referenced in the case of infinity norm only. In this case the length must be at least <paramref name="m"/>.</param>
            /// <returns>The value of the specific matrix norm.</returns>
            double dlange(MatrixNormType matrixNormType, int m, int n, ReadOnlySpan<double> a, Span<double> work);

            /// <summary>Returns the value of the 1-norm, Frobenius norm, infinity-norm, or the largest absolute value of any element of general rectangular matrix.
            /// </summary>
            /// <param name="matrixNormType">The type of the matrix norm.</param>
            /// <param name="m">Th enumber of rows.</param>
            /// <param name="n">The number of columns.</param>
            /// <param name="a">The <paramref name="m"/>-by-<paramref name="n"/> dense matrix provided column-by-column.</param>
            /// <param name="work">A workspace array which is referenced in the case of infinity norm only. In this case the length must be at least <paramref name="m"/>.</param>
            /// <returns>The value of the specific matrix norm.</returns>
            double zlange(MatrixNormType matrixNormType, int m, int n, ReadOnlySpan<Complex> a, Span<Complex> work);

            /// <summary>Returns the value of the 1-norm, Frobenius norm, infinity-norm, or the largest absolute value of any element of symmetric matrix supplied in packed form.
            /// </summary>
            /// <param name="matrixNormType">The type of the matrix norm.</param>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="ap">The specified symmetric matrix in packed form, i.e. either upper or lower triangle as specified in <paramref name="triangularMatrixType"/> with at least <paramref name="n"/> * (<paramref name="n"/> + 1) / 2 elements.</param>
            /// <param name="work">A workspace array which is referenced in the case of 1- or infinity-norm only. In this case the length must be at least <paramref name="n"/>.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>
            /// <returns>The value of the specific matrix norm.</returns>
            double dlansp(MatrixNormType matrixNormType, int n, ReadOnlySpan<double> ap, Span<double> work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Returns the value of the 1-norm, Frobenius norm, infinity-norm, or the largest absolute value of any element of symmetric matrix supplied in packed form.
            /// </summary>
            /// <param name="matrixNormType">The type of the matrix norm.</param>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="ap">The specified symmetric matrix in packed form, i.e. either upper or lower triangle as specified in <paramref name="triangularMatrixType"/> with at least <paramref name="n"/> * (<paramref name="n"/> + 1) / 2 elements.</param>
            /// <param name="work">A workspace array which is referenced in the case of 1- or infinity-norm only. In this case the length must be at least <paramref name="n"/>.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>
            /// <returns>The value of the specific matrix norm.</returns>
            double zlansp(MatrixNormType matrixNormType, int n, ReadOnlySpan<Complex> ap, Span<Complex> work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Returns the value of the 1-norm, Frobenius norm, infinity-norm, or the largest absolute value of any element of general band matrix.
            /// </summary>
            /// <param name="matrixNormType">The type of the matrix norm.</param>
            /// <param name="n">The order of the quadratic general band matrix.</param>
            /// <param name="kl">The number of sub-diagonals of the specific general band matrix.</param>
            /// <param name="ku">The number of super-diagonals of the specific general band matrix.</param>
            /// <param name="a">The general band matrix stored in general band matrix storage, i.e. column-by-column, where each column contains exactly <paramref name="kl" /> + <paramref name="ku" /> + 1 elements.</param>
            /// <param name="work">A workspace array which is referenced in the case of infinity norm only. In this case the length must be at least <paramref name="n" />.</param>
            /// <returns>The value of the specific matrix norm.</returns>
            double dlangb(MatrixNormType matrixNormType, int n, int kl, int ku, ReadOnlySpan<double> a, Span<double> work);

            /// <summary>Returns the value of the 1-norm, Frobenius norm, infinity-norm, or the largest absolute value of any element of general band matrix.
            /// </summary>
            /// <param name="matrixNormType">The type of the matrix norm.</param>
            /// <param name="n">The order of the quadratic general band matrix.</param>
            /// <param name="kl">The number of sub-diagonals of the specific general band matrix.</param>
            /// <param name="ku">The number of super-diagonals of the specific general band matrix.</param>
            /// <param name="a">The general band matrix stored in general band matrix storage, i.e. column-by-column, where each column contains exactly <paramref name="kl" /> + <paramref name="ku" /> + 1 elements.</param>
            /// <param name="work">A workspace array which is referenced in the case of infinity norm only. In this case the length must be at least <paramref name="n" />.</param>
            /// <returns>The value of the specific matrix norm.</returns>
            double zlangb(MatrixNormType matrixNormType, int n, int kl, int ku, ReadOnlySpan<Complex> a, Span<Complex> work);
        }
    }
}