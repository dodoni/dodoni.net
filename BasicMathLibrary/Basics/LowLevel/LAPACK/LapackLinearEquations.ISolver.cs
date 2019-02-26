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
        /// <summary>Provides LAPACK routines for solving systems of linear equations. For more information see the documentation of the Linear Algebra PACKage http://www.netlib.org/lapack/index.html.
        /// </summary>
        /// <remarks>Before calling most of these routines, you need to factorize the matrix of your system of equations. However, the factorization is not necessary if your system of equations has a triangular matrix.</remarks>
        public interface ISolver
        {
            /// <summary>Solves a system of linear equations with a LU-factored square matrix, with multiple right-hand sides, i.e. op(A) * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A, i.e. the number of rows in B.</param>
            /// <param name="a">The LU factorization of matrix A resulting from the call of <c>dgetrf</c> </param>
            /// <param name="ipiv">The argument corresponds to the output of the LU factorization, i.e. <c>dgetrf</c>.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="transposeState">A value indicating whether op(A) is matrix A or its transposed.</param>
            void dgetrs(int n, Span<double> a, int[] ipiv, Span<double> b, int nrhs = 1, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose);

            /// <summary>Solves a system of linear equations with a LU-factored square matrix, with multiple right-hand sides, i.e. op(A) * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A, i.e. the number of rows in B.</param>
            /// <param name="a">The LU factorization of matrix A resulting from the call of <c>zgetrf</c> </param>
            /// <param name="ipiv">The argument corresponds to the output of the LU factorization, i.e. <c>zgetrf</c>.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="transposeState">A value indicating whether op(A) is matrix A, its transposed or its Hermitian.</param>
            void zgetrs(int n, Span<Complex> a, int[] ipiv, Span<Complex> b, int nrhs = 1, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose);

            /// <summary>Solves a system of linear equations with a LU-factored band matrix, with multiple right-hand sides, i.e. op(A) * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A, i.e. the number of rows in B.</param>
            /// <param name="kl">The number of subdiagonals within the band of A.</param>
            /// <param name="ku">The number of superdiagonals within the band of A.</param>
            /// <param name="ab">The matrix A in band storage.</param>
            /// <param name="ipiv">The argument corresponds to the output of the LU factorization, i.e. <c>dgbtrf</c>.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="transposeState">A value indicating whether op(A) is matrix A or its transposed.</param>
            void dgbtrs(int n, int kl, int ku, Span<double> ab, int[] ipiv, Span<double> b, int nrhs = 1, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose);

            /// <summary>Solves a system of linear equations with a LU-factored band matrix, with multiple right-hand sides, i.e. op(A) * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A, i.e. the number of rows in B.</param>
            /// <param name="kl">The number of subdiagonals within the band of A.</param>
            /// <param name="ku">The number of superdiagonals within the band of A.</param>
            /// <param name="ab">The matrix A in band storage.</param>
            /// <param name="ipiv">The argument corresponds to the output of the LU factorization, i.e. <c>dgbtrf</c>.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="transposeState">A value indicating whether op(A) is matrix A, its transposed or its Hermitian.</param>
            void zgbtrs(int n, int kl, int ku, Span<Complex> ab, int[] ipiv, Span<Complex> b, int nrhs = 1, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose);

            /// <summary>Solves a system of linear equations with a tridiagonal matrix using the LU factorization computed by <c>dgttrf</c>, i.e. op(A) * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A, i.e. the number of columns in B.</param>
            /// <param name="dl">The multipliers that define the matrix L from the LU factorization of A.</param>
            /// <param name="d">The diagonal elements of the upper triangular matrix U from the LU factorization of A.</param>
            /// <param name="du">The <paramref name="n"/> - 1 elements of the first superdiagonal of U.</param>
            /// <param name="du2">The <paramref name="n"/> - 2 elements of the second superdiagonal of U.</param>
            /// <param name="ipiv">The <c>ipiv</c> array, as returned by <c>dgttrf</c>.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="transposeState">A value indicating whether op(A) is matrix A or its transposed.</param>
            void dgttrs(int n, Span<double> dl, Span<double> d, Span<double> du, Span<double> du2, int[] ipiv, Span<double> b, int nrhs = 1, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose);

            /// <summary>Solves a system of linear equations with a tridiagonal matrix using the LU factorization computed by <c>dgttrf</c>, i.e. op(A) * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A, i.e. the number of columns in B.</param>
            /// <param name="dl">The multipliers that define the matrix L from the LU factorization of A.</param>
            /// <param name="d">The diagonal elements of the upper triangular matrix U from the LU factorization of A.</param>
            /// <param name="du">The <paramref name="n"/> - 1 elements of the first superdiagonal of U.</param>
            /// <param name="du2">The <paramref name="n"/> - 2 elements of the second superdiagonal of U.</param>
            /// <param name="ipiv">The <c>ipiv</c> array, as returned by <c>dgttrf</c>.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="transposeState">A value indicating whether op(A) is matrix A, its transposed or its Hermitian.</param>
            void zgttrs(int n, Span<Complex> dl, Span<Complex> d, Span<Complex> du, Span<Complex> du2, int[] ipiv, Span<Complex> b, int nrhs = 1, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose);

            /// <summary>Solves a system of linear equations with a Cholesky-factored symmetric (Hermitian) positive-definite matrix computed by <c>dpotrf</c>, i.e. A * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="a">Contains the factor U or L with respect to <paramref name="triangularMatrixType"/>.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
            void dpotrs(int n, Span<double> a, Span<double> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Solves a system of linear equations with a Cholesky-factored symmetric (Hermitian) positive-definite matrix computed by <c>zpotrf</c>, i.e. A * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="a">Contains the factor U or L with respect to <paramref name="triangularMatrixType"/>.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
            void zpotrs(int n, Span<Complex> a, Span<Complex> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Solves a system of linear equations with a Cholesky-factored symmetric (Hermitian) positive-definite matrix using the Rectangular Full Packed (RFP) format computed by <c>dpftrf</c>, i.e. A * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="a">Contains the upper or lower triangular part of matrix op(A) with respect to <paramref name="triangularMatrixType"/> in the RFP format.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
            /// <param name="transposeState">A value indicating whether matrix A or A' is stored  in RFP format.</param>
            void dpftrs(int n, Span<double> a, Span<double> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose);

            /// <summary>Solves a system of linear equations with a Cholesky-factored symmetric (Hermitian) positive-definite matrix using the Rectangular Full Packed (RFP) format computed by <c>zpftrf</c>, i.e. A * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="a">Contains the upper or lower triangular part of matrix op(A) with respect to <paramref name="triangularMatrixType"/> in the RFP format.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
            /// <param name="transposeState">A value indicating whether matrix A, A' or A^H is stored  in RFP format.</param>
            void zpftrs(int n, Span<Complex> a, Span<Complex> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose);

            /// <summary>Solves a system of linear equations with a packed Cholesky-factored symmetric (Hermitian) positive-definite matrix computed by <c>dpptrf</c>, i.e. A * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="ap">Contains the factor U or L with respect to <paramref name="triangularMatrixType"/> in packed storage.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
            void dpptrs(int n, Span<double> ap, Span<double> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Solves a system of linear equations with a packed Cholesky-factored symmetric (Hermitian) positive-definite matrix computed by <c>zpptrf</c>, i.e. A * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="ap">Contains the factor U or L with respect to <paramref name="triangularMatrixType"/> in packed storage.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
            void zpptrs(int n, Span<Complex> ap, Span<Complex> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Solves a system of linear equations with a Cholesky-factored symmetric (Hermitian) positive-definite band matrix computed by <c>dpbtrf</c>, i.e. A * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="kd">The number of superdiagonals or subdiagonals in the matrix A.</param>
            /// <param name="ab">The Cholesky factor as returned by the factorization routine, in band storage form.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
            void dpbtrs(int n, int kd, Span<double> ab, Span<double> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Solves a system of linear equations with a Cholesky-factored symmetric (Hermitian) positive-definite band matrix computed by <c>zpbtrf</c>, i.e. A * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="kd">The number of superdiagonals or subdiagonals in the matrix A.</param>
            /// <param name="ab">The Cholesky factor as returned by the factorization routine, in band storage form.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
            void zpbtrs(int n, int kd, Span<Complex> ab, Span<Complex> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Solves a system of linear equations with a symmetric (Hermitian) positive-definite tridiagonal matrix using the factorization computed by <c>dpttrf</c>, i.e. A * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="d">The diagonal elements of the diagonal matrix D from the factorization computed by <c>dpttrf</c>.</param>
            /// <param name="e">The <paramref name="n"/> - 1 off-diagonal elements of the unit bidiagonal factor U or L from the factorization computed by <c>dpttrf</c>.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            void dpttrs(int n, Span<double> d, Span<double> e, Span<double> b, int nrhs = 1);

            /// <summary>Solves a system of linear equations with a symmetric (Hermitian) positive-definite tridiagonal matrix using the factorization computed by <c>zpttrf</c>, i.e. A * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="d">The diagonal elements of the diagonal matrix D from the factorization computed by <c>zpttrf</c>.</param>
            /// <param name="e">The <paramref name="n"/> - 1 off-diagonal elements of the unit bidiagonal factor U or L from the factorization computed by <c>zpttrf</c>.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
            void zpttrs(int n, Span<double> d, Span<Complex> e, Span<Complex> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Solves a system of linear equations with a UDU- or LDL-factored symmetric matrix, computed by <c>dsytrf</c>, i.e. A * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="a">Contains the upper or lower triangular part of matrix A with respect to <paramref name="triangularMatrixType"/>.</param>
            /// <param name="ipiv">The ipiv array, as returned by <c>dsytrf</c>.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
            void dsytrs(int n, Span<double> a, int[] ipiv, Span<double> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Solves a system of linear equations with a UDU- or LDL-factored symmetric matrix, computed by <c>zsytrf</c>, i.e. A * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="a">Contains the upper or lower triangular part of matrix A with respect to <paramref name="triangularMatrixType"/>.</param>
            /// <param name="ipiv">The ipiv array, as returned by <c>zsytrf</c>.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
            void zsytrs(int n, Span<Complex> a, int[] ipiv, Span<Complex> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Solves a system of linear equations with a UDU- or LDL-factored Hermitian matrix, as computed by <c>zhetrf</c>, i.e. A * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="a">Contains the upper or lower triangular part of matrix A with respect to <paramref name="triangularMatrixType"/>.</param>
            /// <param name="ipiv">The ipiv array, as returned by <c>zhetrf</c>.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
            void zhetrs(int n, Span<Complex> a, int[] ipiv, Span<Complex> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Solves a system of linear equations with a UDU- or LDL-factored symmetric matrix computed by <c>dsytrf</c> and converted by <c>dsyconv</c>, i.e. A * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="a">Contains the block diagonal matrix D and the multiplies used to obain the factor U or L as compured by <c>dsytrf</c> with respect to <paramref name="triangularMatrixType"/>.</param>
            /// <param name="ipiv">The ipiv array, as returned by <c>dsytrf</c>.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="work">A workspace array of dimension at least <paramref name="n"/>.</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
            void dsytrs2(int n, Span<double> a, int[] ipiv, Span<double> b, Span<double> work, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Solves a system of linear equations with a UDU- or LDL-factored symmetric matrix computed by <c>zsytrf</c> and converted by <c>zsyconv</c>, i.e. A * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="a">Contains the block diagonal matrix D and the multiplies used to obain the factor U or L as compured by <c>zsytrf</c> with respect to <paramref name="triangularMatrixType"/>.</param>
            /// <param name="ipiv">The ipiv array, as returned by <c>zsytrf</c>.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="work">A workspace array of dimension at least <paramref name="n"/>.</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
            void zsytrs2(int n, Span<Complex> a, int[] ipiv, Span<Complex> b, Span<Complex> work, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Solves a system of linear equations with a UDU- or LDL-factored Hermitian matrix computed by <c>zhetrf</c> and converted by <c>zsyconv</c>, i.e. A * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="a">Contains the block diagonal matrix D and the multiplies used to obain the factor U or L as compured by <c>zhetrf</c> with respect to <paramref name="triangularMatrixType"/>.</param>
            /// <param name="ipiv">The ipiv array, as returned by <c>zhetrf</c>.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="work">A workspace array of dimension at least <paramref name="n"/>.</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
            void zhetrs2(int n, Span<Complex> a, int[] ipiv, Span<Complex> b, Span<Complex> work, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Solves a system of linear equations with a UDU- or LDL-factored symmetric matrix using packed storage, as computed by <c>dsptrf</c>, i.e. A * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="ap">Contains the factor U or L, as specified by <paramref name="triangularMatrixType"/>, in packed storage.</param>
            /// <param name="ipiv">The ipiv array, as returned by <c>dsptrf</c>.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
            void dsptrs(int n, Span<double> ap, int[] ipiv, Span<double> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Solves a system of linear equations with a UDU- or LDL-factored symmetric matrix using packed storage, as computed by <c>zsptrf</c>, i.e. A * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="ap">Contains the factor U or L, as specified by <paramref name="triangularMatrixType"/>, in packed storage.</param>
            /// <param name="ipiv">The ipiv array, as returned by <c>zsptrf</c>.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
            void zsptrs(int n, Span<Complex> ap, int[] ipiv, Span<Complex> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Solves a system of linear equations with a UDU- or LDL-factored Hermitian matrix using packed storage, computed by <c>dhptrf</c>, i.e. A * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="ap">Contains the factor U or L, as specified by <paramref name="triangularMatrixType"/>, in packed storage.</param>
            /// <param name="ipiv">The ipiv array, as returned by <c>dhptrf</c>.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
            void zhptrs(int n, Span<Complex> ap, int[] ipiv, Span<Complex> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Solves a system of linear equations with a triangular matrix, with multiple right-hand sides, i.e. op(A) * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="a">Contains the matrix A.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="isUnitTriangularMatrix">A value indicating whether matrix A is unit triangular, i.e. diagonal elements of A are assumed to be 1 and not referenced in the array <paramref name="a"/>.</param>
            /// <param name="triangularMatrixType">A value indicating whether matrix A is upper or lower triangular.</param>
            /// <param name="transposeState">A value indicating whether op(A) is matrix A or its transposed.</param>
            void dtrtrs(int n, Span<double> a, Span<double> b, int nrhs = 1, bool isUnitTriangularMatrix = true, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose);

            /// <summary>Solves a system of linear equations with a triangular matrix, with multiple right-hand sides, i.e. op(A) * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="a">Contains the matrix A.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="isUnitTriangularMatrix">A value indicating whether matrix A is unit triangular, i.e. diagonal elements of A are assumed to be 1 and not referenced in the array <paramref name="a"/>.</param>
            /// <param name="triangularMatrixType">A value indicating whether matrix A is upper or lower triangular.</param>
            /// <param name="transposeState">A value indicating whether op(A) is matrix A, its transposed or its Hermitian.</param>
            void ztrtrs(int n, Span<Complex> a, Span<Complex> b, int nrhs = 1, bool isUnitTriangularMatrix = true, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose);

            /// <summary>Solves a system of linear equations with a packed triangular matrix, with multiple right-hand sides, i.e. op(A) * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="ap">Contains the matrix A in packed storage.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="isUnitTriangularMatrix">A value indicating whether matrix A is unit triangular, i.e. diagonal elements of A are assumed to be 1 and not referenced in the array <paramref name="ap"/>.</param>
            /// <param name="triangularMatrixType">A value indicating whether matrix A is upper or lower triangular.</param>
            /// <param name="transposeState">A value indicating whether op(A) is matrix A or its transposed.</param>
            void dtptrs(int n, Span<double> ap, Span<double> b, int nrhs = 1, bool isUnitTriangularMatrix = true, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose);

            /// <summary>Solves a system of linear equations with a packed triangular matrix, with multiple right-hand sides, i.e. op(A) * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="ap">Contains the matrix A in packed storage.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="isUnitTriangularMatrix">A value indicating whether matrix A is unit triangular, i.e. diagonal elements of A are assumed to be 1 and not referenced in the array <paramref name="ap"/>.</param>
            /// <param name="triangularMatrixType">A value indicating whether matrix A is upper or lower triangular.</param>
            /// <param name="transposeState">A value indicating whether op(A) is matrix A, its transposed or its Hermitian.</param>
            void ztptrs(int n, Span<Complex> ap, Span<Complex> b, int nrhs = 1, bool isUnitTriangularMatrix = true, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose);

            /// <summary>Solves a system of linear equations with a band triangular matrix, with multiple right-hand sides, i.e. op(A) * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="kd">The number of superdiagonals or subdiagonals in the matrix A.</param>
            /// <param name="ab">The matrix A in band storage form.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="isUnitTriangularMatrix">A value indicating whether matrix A is unit triangular, i.e. diagonal elements of A are assumed to be 1 and not referenced in the array <paramref name="ab"/>.</param>
            /// <param name="triangularMatrixType">A value indicating whether matrix A is upper or lower triangular.</param>
            /// <param name="transposeState">A value indicating whether op(A) is matrix A or its transposed.</param>
            void dtbtrs(int n, int kd, Span<double> ab, Span<double> b, int nrhs = 1, bool isUnitTriangularMatrix = true, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose);

            /// <summary>Solves a system of linear equations with a band triangular matrix, with multiple right-hand sides, i.e. op(A) * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="kd">The number of superdiagonals or subdiagonals in the matrix A.</param>
            /// <param name="ab">The matrix A in band storage form.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="isUnitTriangularMatrix">A value indicating whether matrix A is unit triangular, i.e. diagonal elements of A are assumed to be 1 and not referenced in the array <paramref name="ab"/>.</param>
            /// <param name="triangularMatrixType">A value indicating whether matrix A is upper or lower triangular.</param>
            /// <param name="transposeState">A value indicating whether op(A) is matrix A, its transposed or its Hermitian.</param>
            void ztbtrs(int n, int kd, Span<Complex> ab, Span<Complex> b, int nrhs = 1, bool isUnitTriangularMatrix = true, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose);

            /// <summary>Computes the solution to the system of linear equations with a square matrix A and multiple right-hand sides, i.e. A * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="a">The matrix A provided column-by-column; overwritten by the factors L and U from the factorization of A = P * L * U, the unit diagonal elements of L are not stored.</param>
            /// <param name="ipiv">Array of dimension at least <paramref name="n"/>, where the pivot indices define the permutation matrix P.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            void driver_dgesv(int n, Span<double> a, int[] ipiv, Span<double> b, int nrhs = 1);

            /// <summary>Computes the solution to the system of linear equations with a square matrix A and multiple right-hand sides, i.e. A * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="a">The matrix A provided column-by-column; overwritten by the factors L and U from the factorization of A = P * L * U, the unit diagonal elements of L are not stored.</param>
            /// <param name="ipiv">Array of dimension at least <paramref name="n"/>, where the pivot indices define the permutation matrix P.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            void driver_zgesv(int n, Span<Complex> a, int[] ipiv, Span<Complex> b, int nrhs = 1);

            /// <summary>Computes the solution to the system of linear equations with a band matrix A and multiple right-hand sides, i.e. A * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="kl">The number of subdiagonals within the band of A.</param>
            /// <param name="ku">The number of superdiagonals within the band of A.</param>
            /// <param name="ab">The matrix A in band storage; overwritten by L and U.</param>
            /// <param name="ipiv">Array of dimension at least <paramref name="n"/>, where the pivot indices define the permutation matrix P.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            void driver_dgbsv(int n, int kl, int ku, Span<double> ab, int[] ipiv, Span<double> b, int nrhs = 1);

            /// <summary>Computes the solution to the system of linear equations with a band matrix A and multiple right-hand sides, i.e. A * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="kl">The number of subdiagonals within the band of A.</param>
            /// <param name="ku">The number of superdiagonals within the band of A.</param>
            /// <param name="ab">The matrix A in band storage; overwritten by L and U.</param>
            /// <param name="ipiv">Array of dimension at least <paramref name="n"/>, where the pivot indices define the permutation matrix P.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            void driver_zgbsv(int n, int kl, int ku, Span<Complex> ab, int[] ipiv, Span<Complex> b, int nrhs = 1);

            /// <summary>Computes the solution to the system of linear equations with a tridiagonal matrix A and multiple right-hand sides, i.e. A * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="dl">The <paramref name="n"/> - 1 subdiagonal elements of A; overwritten by the <paramref name="n"/> - 2 elements of the second superdiagonal of the upper triangular matrix U.</param>
            /// <param name="d">The <paramref name="n"/> diagonal elements of A; overwritten by the <paramref name="n"/> diagonal elements of U.</param>
            /// <param name="du">The <paramref name="n"/> - 1 superdiagonal elements of A; overwritten by the <paramref name="n"/> - 1 elements of the first superdiagonal of U.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            void driver_dgtsv(int n, Span<double> dl, Span<double> d, Span<double> du, Span<double> b, int nrhs = 1);

            /// <summary>Computes the solution to the system of linear equations with a tridiagonal matrix A and multiple right-hand sides, i.e. A * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="dl">The <paramref name="n"/> - 1 subdiagonal elements of A; overwritten by the <paramref name="n"/> - 2 elements of the second superdiagonal of the upper triangular matrix U.</param>
            /// <param name="d">The <paramref name="n"/> diagonal elements of A; overwritten by the <paramref name="n"/> diagonal elements of U.</param>
            /// <param name="du">The <paramref name="n"/> - 1 superdiagonal elements of A; overwritten by the <paramref name="n"/> - 1 elements of the first superdiagonal of U.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            void driver_zgtsv(int n, Span<Complex> dl, Span<Complex> d, Span<Complex> du, Span<Complex> b, int nrhs = 1);

            /// <summary>Computes the solution to the system of linear equations with a symmetric positive-definite matrix A and multiple right-hand sides, i.e. A * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="a">Contains the factor U or L, as specified by <paramref name="triangularMatrixType"/>.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>        
            void driver_dposv(int n, Span<double> a, Span<double> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Computes the solution to the system of linear equations with a Hermitian positive-definite matrix A and multiple right-hand sides, i.e. A * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="a">Contains the factor U or L, as specified by <paramref name="triangularMatrixType"/>.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>        
            void driver_zposv(int n, Span<Complex> a, Span<Complex> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Computes the solution to the system of linear equations with a symmetric positive definite packed matrix A and multiple right-hand sides, i.e. A * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="ap">Contains the factor U or L, as specified by <paramref name="triangularMatrixType"/> in packed storage.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>        
            void driver_dppsv(int n, Span<double> ap, Span<double> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Computes the solution to the system of linear equations with a Hermitian positive definite packed matrix A and multiple right-hand sides, i.e. A * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="ap">Contains the factor U or L, as specified by <paramref name="triangularMatrixType"/> in packed storage.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>        
            void driver_zppsv(int n, Span<Complex> ap, Span<Complex> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Computes the solution to the system of linear equations with a symmetric positive-definite band matrix A and multiple right-hand sides, i.e. A * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="kd">The number of superdiagonals or subdiagonals as specified by <paramref name="triangularMatrixType"/>.</param>
            /// <param name="ab">Contains the upper or lower triangular part of matrix A, as specified by <paramref name="triangularMatrixType"/> in packed storage in band storage.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>        
            void driver_dpbsv(int n, int kd, Span<double> ab, Span<double> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Computes the solution to the system of linear equations with a Hermitian positive-definite band matrix A and multiple right-hand sides, i.e. A * X = B.
            /// </summary>
            /// <param name="kd">The number of superdiagonals or subdiagonals as specified by <paramref name="triangularMatrixType"/>.</param>
            /// <param name="ab">Contains the upper or lower triangular part of matrix A, as specified by <paramref name="triangularMatrixType"/> in packed storage in band storage.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
            /// <param name="n">The order of matrix A.</param>        
            void driver_zpbsv(int n, int kd, Span<Complex> ab, Span<Complex> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Computes the solution to the system of linear equations with a symmetric positive definite tridiagonal matrix A and multiple right-hand sides, i.e. A * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="d">Contains the <paramref name="n"/> diagonal elements of the tridiagonal matrix A; overwritten by the <paramref name="n"/> diagonal elements of the diagonal matrix D from L * D * L' factorization of A.</param>
            /// <param name="e">Contains the <paramref name="n"/> - 1 subdiagonal elements of matrix A; overwritten by the <paramref name="n"/> - 1 subdiagonal elements of the unit bidiagonal factor L from the factorization of A.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            void driver_dptsv(int n, Span<double> d, Span<double> e, Span<double> b, int nrhs = 1);

            /// <summary>Computes the solution to the system of linear equations with a Hermitian positive definite tridiagonal matrix A and multiple right-hand sides, i.e. A * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="d">Contains the <paramref name="n"/> diagonal elements of the tridiagonal matrix A; overwritten by the <paramref name="n"/> diagonal elements of the diagonal matrix D from L * D * L' factorization of A.</param>
            /// <param name="e">Contains the <paramref name="n"/> - 1 subdiagonal elements of matrix A; overwritten by the <paramref name="n"/> - 1 subdiagonal elements of the unit bidiagonal factor L from the factorization of A.</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            void driver_zptsv(int n, Span<double> d, Span<Complex> e, Span<Complex> b, int nrhs = 1);

            /// <summary>Gets a optimal workspace array length for the <c>driver_dsysv</c> function.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>        
            /// <returns>The optimal workspace array length.</returns>
            /// <remarks>The parameter <paramref name="triangularMatrixType"/> should not have an impact of the calculation of the optimal length of the workspace array.</remarks>
            int driver_dsysvQuery(int n, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Computes the solution to the system of linear equations with a real symmetric matrix A and multiple right-hand sides, i.e. A * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="a">Contains the upper or lower triangular part of matrix A, as specified by <paramref name="triangularMatrixType"/>; overwritten by the block-diagonal matrix D and the multipliers used to obtain the factor U (or L) from the factorization of A as computed by <c>dsytrf</c>.</param>
            /// <param name="ipiv">A array with dimension at least <paramref name="n"/>; contains details of the interchanges and the block structure of D, as determined by <c>dsytrf</c> (output). </param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="work">A workspace array.</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>        
            void driver_dsysv(int n, Span<double> a, int[] ipiv, Span<double> b, Span<double> work, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Gets a optimal workspace array length for the <c>driver_zsysv</c> function.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>        
            /// <returns>The optimal workspace array length.</returns>
            /// <remarks>The parameter <paramref name="triangularMatrixType"/> should not have an impact of the calculation of the optimal length of the workspace array.</remarks>
            int driver_zsysvQuery(int n, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Computes the solution to the system of linear equations with a complex symmetric matrix A and multiple right-hand sides, i.e. A * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="a">Contains the upper or lower triangular part of matrix A, as specified by <paramref name="triangularMatrixType"/>; overwritten by the block-diagonal matrix D and the multipliers used to obtain the factor U (or L) from the factorization of A as computed by <c>zsytrf</c>.</param>
            /// <param name="ipiv">A array with dimension at least <paramref name="n"/>; contains details of the interchanges and the block structure of D, as determined by <c>zsytrf</c> (output). </param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="work">A workspace array.</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>        
            void driver_zsysv(int n, Span<Complex> a, int[] ipiv, Span<Complex> b, Span<Complex> work, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Gets a optimal workspace array length for the <c>driver_zhesv</c> function.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>        
            /// <returns>The optimal workspace array length.</returns>
            /// <remarks>The parameter <paramref name="triangularMatrixType"/> should not have an impact of the calculation of the optimal length of the workspace array.</remarks>
            int driver_zhesvQuery(int n, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Computes the solution to the system of linear equations with a Hermitian matrix A and multiple right-hand sides, i.e. A * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="a">Contains the upper or lower triangular part of matrix A, as specified by <paramref name="triangularMatrixType"/>; overwritten by the block-diagonal matrix D and the multipliers used to obtain the factor U (or L) from the factorization of A as computed by <c>zhetrf</c>.</param>
            /// <param name="ipiv">A array with dimension at least <paramref name="n"/>; contains details of the interchanges and the block structure of D, as determined by <c>zhetrf</c> (output).</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="work">A workspace array.</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>        
            void driver_zhesv(int n, Span<Complex> a, int[] ipiv, Span<Complex> b, Span<Complex> work, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Computes the solution to the system of linear equations with a real symmetric matrix A stored in packed format, and multiple right-hand sides, i.e. A * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="ap">Contains the upper or lower triangular part of matrix A, as specified by <paramref name="triangularMatrixType"/> in packed format; overwritten by the block-diagonal matrix D and the multipliers used to obtain the factor U (or L) from the factorization of A as computed by <c>dsptrf</c>.</param>
            /// <param name="ipiv">A array with dimension at least <paramref name="n"/>; contains details of the interchanges and the block structure of D, as determined by <c>dsptrf</c> (output).</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>        
            void driver_dspsv(int n, Span<double> ap, int[] ipiv, Span<double> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Computes the solution to the system of linear equations with a complex symmetric matrix A stored in packed format, and multiple right-hand sides, i.e. A * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="ap">Contains the upper or lower triangular part of matrix A, as specified by <paramref name="triangularMatrixType"/> in packed format; overwritten by the block-diagonal matrix D and the multipliers used to obtain the factor U (or L) from the factorization of A as computed by <c>zsptrf</c>.</param>
            /// <param name="ipiv">A array with dimension at least <paramref name="n"/>; contains details of the interchanges and the block structure of D, as determined by <c>zsptrf</c> (output).</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>        
            void driver_zspsv(int n, Span<Complex> ap, int[] ipiv, Span<Complex> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Computes the solution to the system of linear equations with a Hermitian matrix A stored in packed format, and multiple right-hand sides, i.e. A * X = B.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="ap">Contains the upper or lower triangular part of matrix A, as specified by <paramref name="triangularMatrixType"/> in packed format; overwritten by the block-diagonal matrix D and the multipliers used to obtain the factor U (or L) from the factorization of A as computed by <c>zhptrf</c>.</param>
            /// <param name="ipiv">A array with dimension at least <paramref name="n"/>; contains details of the interchanges and the block structure of D, as determined by <c>zhptrf</c> (output).</param>
            /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
            /// <param name="nrhs">The number of right-hand sides.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>        
            void driver_zhpsv(int n, Span<Complex> ap, int[] ipiv, Span<Complex> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);
        }
    }
}