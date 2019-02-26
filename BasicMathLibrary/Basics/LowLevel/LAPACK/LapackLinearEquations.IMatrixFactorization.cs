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
    public partial class LapackLinearEquations
    {
        /// <summary>Provides methods for Matrix factorization. For more informations see the documentation of the Linear Algebra PACKage http://www.netlib.org/lapack/index.html.
        /// </summary>
        public interface IMatrixFactorization
        {
#pragma warning disable IDE1006 // Naming Styles

            /// <summary>Computes the LU factorization of a general m-by-n matrix, i.e. A = P * L * U, where P is a permutation matrix, L is lower triangular with unit diagonal elements and U is upper triangular.
            /// </summary>
            /// <param name="m">The number of rows of the matrix.</param>
            /// <param name="n">The number of columns of the matrix.</param>
            /// <param name="a">The matrix A supplied column-by-column of dimension (<paramref name="m"/>; <paramref name="n"/>); overwritten by L and U, the unit diagonal elements of L are not stored; output.</param>
            /// <param name="iPivot">The min(<paramref name="m"/>,<paramref name="n"/>) pivot indices, i.e. row j was interchanged with row <paramref name="iPivot"/>[j] (output).</param>
            void dgetrf(int m, int n, Span<double> a, int[] iPivot);

            /// <summary>Computes the LU factorization of a general m-by-n matrix, i.e. A = P * L * U, where P is a permutation matrix, L is lower triangular with unit diagonal elements and U is upper triangular.
            /// </summary>
            /// <param name="m">The number of rows of the matrix.</param>
            /// <param name="n">The number of columns of the matrix.</param>
            /// <param name="a">The matrix A supplied column-by-column of dimension (<paramref name="n"/>; <paramref name="n"/>); overwritten by 'L' and 'U', the unit diagonal elements of 'L' are not stored; output.</param>
            /// <param name="iPivot">The min(<paramref name="m"/>,<paramref name="n"/>) pivot indices, i.e. row j was interchanged with row <paramref name="iPivot"/>[j] (output).</param>
            void zgetrf(int m, int n, Span<Complex> a, int[] iPivot);

            /// <summary>Computes the LU factorization of a general m-by-n band matrix, i.e. A = P * L * U, where P is a permutation matrix, L is lower triangular with unit diagonal elements and U is upper triangular.
            /// </summary>
            /// <param name="m">The number of rows of the matrix.</param>
            /// <param name="n">The number of columns of the matrix.</param>
            /// <param name="kl">The number of subdiagonals within the band of the input matrix.</param>
            /// <param name="ku">The number of superdiagonals within the band of the input matrix.</param>
            /// <param name="a">The input band matrix in band storage, i.e. of dimension (2 * <paramref name="kl"/> + <paramref name="ku"/> + 1; <paramref name="n"/>); overwritten by L and U. U is stored as an upper triangular band matrix with <paramref name="kl"/> + <paramref name="ku"/> superdiagonals.</param>
            /// <param name="iPivot">The pivot indices, i.e. row j was interchanged with row <paramref name="iPivot"/>[j]; must contain at least min(<paramref name="m"/>,<paramref name="n"/>) elements (output).</param>
            void dgbtrf(int m, int n, int kl, int ku, Span<double> a, int[] iPivot);

            /// <summary>Computes the LU factorization of a general m-by-n band matrix, i.e. A = P * L * U, where P is a permutation matrix, L is lower triangular with unit diagonal elements and U is upper triangular.
            /// </summary>
            /// <param name="m">The number of rows of the matrix.</param>
            /// <param name="n">The number of columns of the matrix.</param>
            /// <param name="kl">The number of subdiagonals within the band of the input matrix.</param>
            /// <param name="ku">The number of superdiagonals within the band of the input matrix.</param>
            /// <param name="a">The input band matrix in band storage, i.e. of dimension (2 * <paramref name="kl"/> + <paramref name="ku"/> + 1; <paramref name="n"/>); overwritten by L and U. U is stored as an upper triangular band matrix with <paramref name="kl"/> + <paramref name="ku"/> superdiagonals.</param>
            /// <param name="iPivot">The pivot indices, i.e. row j was interchanged with row <paramref name="iPivot"/>[j]; must contain at least min(<paramref name="m"/>,<paramref name="n"/>) elements (output).</param>
            void zgbtrf(int m, int n, int kl, int ku, Span<Complex> a, int[] iPivot);

            /// <summary>Computes the LU factorization of a tridiagonal matrix, i.e. A = P * L * U, where P is a permutation matrix, L is lower bidiagonal with unit diagonal elements and U is upper triangular
            /// with nonzeroes in only the main diagonal and first two superdiagonals.
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="dl">The (<paramref name="n"/> - 1) subdiagonal elements of the input matrix; overwritten by the (<paramref name="n"/> - 1) multipliers that defines the matrix L from the LU factorization.</param>
            /// <param name="d">The <paramref name="n"/> diagonal elements of the input matrix; overwritten by the <paramref name="n"/> diagonal elements of the upper triangular matrix U from the LU factorization.</param>
            /// <param name="du">The (<paramref name="n"/> - 1) superdiagonal elements of the input matrix; overwritten by the (<paramref name="n"/> - 1) elements of the first superdiagonal of U.</param>
            /// <param name="du2">The (<paramref name="n"/> - 2) elements of the second superdiagonal of U (output).</param>
            /// <param name="iPivot">The <paramref name="n"/> pivot indices, i.e. row j was interchanged with row <paramref name="iPivot"/>[j].
            /// <paramref name="iPivot"/>[j] is always j or j+1; <paramref name="iPivot"/>[j] = j indicates a row interchange was not required (output).</param>
            /// <remarks>The matrix L has unit diagonal elements and the (<paramref name="n"/> - 1) elements of d1 from the subdiagonal. All other elements of L are zero.</remarks>
            void dgttrf(int n, Span<double> dl, Span<double> d, Span<double> du, Span<double> du2, int[] iPivot);

            /// <summary>Computes the LU factorization of a tridiagonal matrix, i.e. A = P * L * U, where P is a permutation matrix, L is lower bidiagonal with unit diagonal elements and U is upper triangular
            /// with nonzeroes in only the main diagonal and first two superdiagonals.
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="dl">The (<paramref name="n"/> - 1) subdiagonal elements of the input matrix; overwritten by the (<paramref name="n"/> - 1) multipliers that defines the matrix L from the LU factorization.</param>
            /// <param name="d">The <paramref name="n"/> diagonal elements of the input matrix; overwritten by the <paramref name="n"/> diagonal elements of the upper triangular matrix U from the LU factorization.</param>
            /// <param name="du">The (<paramref name="n"/> - 1) superdiagonal elements of the input matrix; overwritten by the (<paramref name="n"/> - 1) elements of the first superdiagonal of U.</param>
            /// <param name="du2">The (<paramref name="n"/> - 2) elements of the second superdiagonal of U (output).</param>
            /// <param name="iPivot">The <paramref name="n"/> pivot indices, i.e. row j was interchanged with row <paramref name="iPivot"/>[j].
            /// <paramref name="iPivot"/>[j] is always j or j+1; <paramref name="iPivot"/>[j] = j indicates a row interchange was not required (output).</param>
            /// <remarks>The matrix L has unit diagonal elements and the (<paramref name="n"/> - 1) elements of d1 from the subdiagonal. All other elements of L are zero.</remarks>
            void zgttrf(int n, Span<Complex> dl, Span<Complex> d, Span<Complex> du, Span<Complex> du2, int[] iPivot);

            /// <summary>Computes the Cholesky decomposition of a symmetric positive-definite matrix, i.e. A = U' * U or A = L * L', where L is a lower triangular matrix and U is upper triangular.
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="a">The matrix A supplied column-by-column of dimension (<paramref name="n" />; <paramref name="n" />); overwritten by the upper or lower triangular matrix U, L respectively.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
            void dpotrf(int n, Span<double> a, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Computes the Cholesky decomposition of a Hermitian positive-definite matrix, i.e. A = conj(U') * U or A = L * conj(L'), where L is a lower triangular matrix and U is upper triangular.
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="a">The matrix A supplied column-by-column of dimension (<paramref name="n"/>; <paramref name="n"/>); overwritten by the upper or lower triangular matrix U, L respectively.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
            void zpotrf(int n, Span<Complex> a, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Computes the Cholesky factorization of a symmetric positive-definite matrix using the Rectangular Full Packed (RFP) format, i.e.
            /// A = U' * U or A = L * L', where L is a lower triangular matrix and U is upper triangular.
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="a">The matrix A in the RFP format, i.e. an array with at least <paramref name="n"/> * (<paramref name="n"/> + 1) / 2 elements; overwritten by the upper or lower triangular matrix U, L respectively.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
            /// <param name="transposeState">A value indicating whether <paramref name="a"/> represents matrix A or its transposed.</param>
            void dpftrf(int n, Span<double> a, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose);

            /// <summary>Computes the Cholesky factorization of a Hermitian positive-definite matrix using the Rectangular Full Packed (RFP) format, i.e.
            /// A = conj(U') * U or A = L * conj(L'), where L is a lower triangular matrix and U is upper triangular.
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="a">The matrix A in the RFP format, i.e. an array with at least <paramref name="n"/> * (<paramref name="n"/> + 1) / 2 elements; overwritten by the upper or lower triangular matrix U, L respectively.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
            /// <param name="transposeState">A value indicating whether <paramref name="a"/> represents matrix A or its transposed.</param>
            void zpftrf(int n, Span<Complex> a, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose);

            /// <summary>Computes the Cholesky factorization of a symmetric positive-definite matrix using packed storage, i.e.
            /// A = U' * U or A = L * L', where L is a lower triangular matrix and U is upper triangular.
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="aPacked">Either the upper or lower triangular part of matrix A in packed storage, i.e. at least <paramref name="n"/> * (<paramref name="n"/> + 1)/2 elements; overwritten by the upper or lower triangular matrix U, L respectively in packed storage.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
            void dpptrf(int n, Span<double> aPacked, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Computes the Cholesky factorization of a Hermitian positive-definite matrix using packed storage, i.e.
            /// A = conj(U') * U or A = L * conj(L'), where L is a lower triangular matrix and U is upper triangular.
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="aPacked">Either the upper or lower triangular part of matrix A in packed storage, i.e. at least <paramref name="n" /> * (<paramref name="n" /> + 1)/2 elements; overwritten by the upper or lower triangular matrix U, L respectively in packed storage.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
            void zpptrf(int n, Span<Complex> aPacked, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Computes the Cholesky factorization of a symmetric positive-definite band matrix, i.e. i.e. A = U' * U or A = L * L', where L is a lower triangular matrix and U is upper triangular.
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="kd">The number of superdiagonals or subdiagonals in the input matrix.</param>
            /// <param name="a">Either the upper or lower triangular part of the input matrix in band storage of dimension (<paramref name="kd"/> + 1; <paramref name="n"/>); overwritten by the upper or lower triangular matrix U, L respectively.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
            void dpbtrf(int n, int kd, Span<double> a, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Computes the Cholesky factorization of a Hermitian positive-definite band matrix, i.e. i.e. A = conj(U') * U or A = L * conj(L'), where L is a lower triangular matrix and U is upper triangular.
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="kd">The number of superdiagonals or subdiagonals in the input matrix.</param>
            /// <param name="a">Either the upper or lower triangular part of the input matrix in band storage of dimension (<paramref name="kd"/> + 1; <paramref name="n"/>); overwritten by the upper or lower triangular matrix U, L respectively.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
            void zpbtrf(int n, int kd, Span<Complex> a, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Computes the factorization of a symmetric positive-definite tridiagonal matrix, i.e. A = L * D * L' or A = U' * D * U, where D is diagonal, L is unit lower bidiagonal and U is unit upper bidiagonal.
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="diagonalElements">The <paramref name="n"/> diagonal elements of the input matrix; overwritten by the <paramref name="n"/> diagonal elements of the diagonal matrix D.</param>
            /// <param name="e">The <paramref name="n"/> - 1 subdiagonal elements of the input matrix; overwritten by the (<paramref name="n"/> - 1) off-diagonal elements of the unit bidiagonal factor L or U from the factorization.</param>
            void dpttrf(int n, Span<double> diagonalElements, Span<double> e);

            /// <summary>Computes the factorization of a Hermitian positive-definite tridiagonal matrix, i.e. A = L * D * conj(L') or A = conj(U') * D * U, where D is diagonal, L is unit lower bidiagonal and U is unit upper bidiagonal.
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="diagonalElements">The <paramref name="n"/> diagonal elements of the input matrix; overwritten by the <paramref name="n"/> diagonal elements of the diagonal matrix D.</param>
            /// <param name="e">The <paramref name="n"/> - 1 subdiagonal elements of the input matrix; overwritten by the (<paramref name="n"/> - 1) off-diagonal elements of the unit bidiagonal factor L or U from the factorization.</param>
            void zpttrf(int n, Span<Complex> diagonalElements, Span<Complex> e);

            /// <summary>Gets a optimal workspace array length for the <c>dsytrf</c> function.
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
            /// <returns>The optimal workspace array length.</returns>
            /// <remarks>The parameter <paramref name="triangularMatrixType"/> should not have an impact of the calculation of the optimal length of the workspace array.</remarks>
            int dsytrfQuery(int n, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Computes the Bunch-Kaufman factorization of a symmetric matrix, i.e. A = P * U * D * U' * P' or A = P * L * D * L' * P', where P is a permutation matrix, U and L are upper and
            /// lower triangular matrices with unit diagonal and D is a symmetric block-diagonal matrix with 1-by-1 and 2-by-2 diagonal blocks. U and L have 2-by-2 unit diagonal blocks corresponding to the 2-by-2 blocks of D.
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="a">The upper or the lower triangular part of the input matrix of dimension (<paramref name="n"/>; <paramref name="n"/>); overwritten by details of the block-diagonal matrix D and the multiplies used to obtain the factor U (or L).</param>
            /// <param name="iPivot">Contains details of the interchanges an the block structure of D, at least <paramref name="n"/> elements (output).</param>
            /// <param name="work">A workspace array of length at least <paramref name="n"/>.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
            void dsytrf(int n, Span<double> a, int[] iPivot, Span<double> work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Gets a optimal workspace array length for the <c>zsytrf</c> function.
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
            /// <returns>The optimal workspace array length.</returns>
            /// <remarks>The parameter <paramref name="triangularMatrixType"/> should not have an impact of the calculation of the optimal length of the workspace array.</remarks>
            int zsytrfQuery(int n, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Computes the Bunch-Kaufman factorization of a symmetric matrix, i.e. A = P * U * D * U' * P' or A = P * L * D * L' * P', where P is a permutation matrix, U and L are upper and
            /// lower triangular matrices with unit diagonal and D is a symmetric block-diagonal matrix with 1-by-1 and 2-by-2 diagonal blocks. U and L have 2-by-2 unit diagonal blocks corresponding to the 2-by-2 blocks of D.
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="a">The upper or the lower triangular part of the input matrix of dimension (<paramref name="n"/>; <paramref name="n"/>); overwritten by details of the block-diagonal matrix D and the multiplies used to obtain the factor U (or L).</param>
            /// <param name="iPivot">Contains details of the interchanges an the block structure of D, at least <paramref name="n"/> elements (output).</param>
            /// <param name="work">A workspace array of length at least <paramref name="n"/>.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
            void zsytrf(int n, Span<Complex> a, int[] iPivot, Span<Complex> work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Gets a optimal workspace array length for the <c>zhetrf</c> function.
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
            /// <returns>The optimal workspace array length.</returns>
            /// <remarks>The parameter <paramref name="triangularMatrixType"/> should not have an impact of the calculation of the optimal length of the workspace array.</remarks>
            int zhetrfQuery(int n, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Computes the Bunch-Kaufman factorization of a complex Hermitian matrix, i.e. A = P * U * D * conj(U') * conj(P') or A = P * L * D * conj(L') * conj(P'), where P is a permutation matrix, U and L are upper and
            /// lower triangular matrices with unit diagonal and D is a symmetric block-diagonal matrix with 1-by-1 and 2-by-2 diagonal blocks. U and L have 2-by-2 unit diagonal blocks corresponding to the 2-by-2 blocks of D.
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="a">The upper or the lower triangular part of the input matrix of dimension (<paramref name="n"/>; <paramref name="n"/>); overwritten by details of the block-diagonal matrix D and the multiplies used to obtain the factor U (or L).</param>
            /// <param name="iPivot">Contains details of the interchanges an the block structure of D, at least <paramref name="n"/> elements (output).</param>
            /// <param name="work">A workspace array of length at least <paramref name="n"/>.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
            void zhetrf(int n, Span<Complex> a, int[] iPivot, Span<Complex> work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Computes the Bunch-Kaufman factorization of a symmetric matrix using packed storage, i.e. A = P * U * D * U' * P' or A = P * L * D * L' * P', where P is a permutation matrix, U and L are upper and
            /// lower triangular matrices with unit diagonal and D is a symmetric block-diagonal matrix with 1-by-1 and 2-by-2 diagonal blocks. U and L have 2-by-2 unit diagonal blocks corresponding to the 2-by-2 blocks of D.
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="aPacked">Either the upper or lower triangular part of matrix A in packed storage, i.e. at least <paramref name="n"/> * (<paramref name="n"/> + 1)/2 elements; overwritten by details of the block-diagonal matrix D and the multiplies used to obtain the factor U (or L).</param>
            /// <param name="iPivot">Contains details of the interchanges an the block structure of D, at least <paramref name="n"/> elements (output).</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
            void dsptrf(int n, Span<double> aPacked, int[] iPivot, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Computes the Bunch-Kaufman factorization of a symmetric matrix using packed storage, i.e. A = P * U * D * U' * P' or A = P * L * D * L' * P', where P is a permutation matrix, U and L are upper and
            /// lower triangular matrices with unit diagonal and D is a symmetric block-diagonal matrix with 1-by-1 and 2-by-2 diagonal blocks. U and L have 2-by-2 unit diagonal blocks corresponding to the 2-by-2 blocks of D.
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="aPacked">Either the upper or lower triangular part of matrix A in packed storage, i.e. at least <paramref name="n"/> * (<paramref name="n"/> + 1)/2 elements; overwritten by details of the block-diagonal matrix D and the multiplies used to obtain the factor U (or L).</param>
            /// <param name="iPivot">Contains details of the interchanges an the block structure of D, at least <paramref name="n"/> elements (output).</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
            void zsptrf(int n, Span<Complex> aPacked, int[] iPivot, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Computes the Bunch-Kaufman factorization of a Hermitian matrix using packed storage, i.e. A = P * U * D * conj(U') * conj(P') or A = P * L * D * conj(L') * conj(P'), where P is a permutation matrix, U and L are upper and
            /// lower triangular matrices with unit diagonal and D is a symmetric block-diagonal matrix with 1-by-1 and 2-by-2 diagonal blocks. U and L have 2-by-2 unit diagonal blocks corresponding to the 2-by-2 blocks of D.
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="aPacked">Either the upper or lower triangular part of matrix A in packed storage, i.e. at least <paramref name="n"/> * (<paramref name="n"/> + 1)/2 elements; overwritten by details of the block-diagonal matrix D and the multiplies used to obtain the factor U (or L).</param>
            /// <param name="iPivot">Contains details of the interchanges an the block structure of D, at least <paramref name="n"/> elements (output).</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
            void zhptrf(int n, Span<Complex> aPacked, int[] iPivot, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

#pragma warning restore IDE1006 // Naming Styles
        }
    }
}