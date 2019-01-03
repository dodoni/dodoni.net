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
using System.Security;
using System.Runtime.InteropServices;

namespace Dodoni.MathLibrary.Basics.LowLevel.Native
{
    public static partial class MklExtensions
    {
        #region private

        [DllImport(sm_DllName, EntryPoint = "DDTTRFB", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        private static extern void _ddttrfb(ref int n, [In, Out] double[] dl, [In, Out] double[] d, [In, Out] double[] du, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZDTTRFB", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        private static extern void _zdttrfb(ref int n, [In, Out] Complex[] dl, [In, Out] Complex[] d, [In, Out] Complex[] du, out int info);

        [DllImport(sm_DllName, EntryPoint = "DPSTRF", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        private static extern void _dpstrf(ref char uplo, ref int n, [In, Out] double[] a, ref int lda, [In, Out] int[] piv, out int rank, ref double tol, [In, Out] double[] work, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZPSTRF", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        private static extern void _zpstrf(ref char uplo, ref int n, [In, Out] Complex[] a, ref int lda, [In, Out] int[] piv, out int rank, ref double tol, [In, Out] Complex[] work, out int info);
        #endregion

        #region public (static) methods

        /// <summary>Computes the factorization of a diagonally dominant triangular matrix, i.e. A = L_1 * U * L_, 
        /// <list type="bullet">
        ///  <item>
        ///  <description>where L_1 and L_2 are unit lower bidiagonal with k and n - k - 1 subdiagonal elements, respectively, where k = n/2,</description>
        ///  </item>
        ///  <item>
        ///   <description>U is an upper bidiagonal matrix with nonzeroes in only the main diagonal and first superdiagonal.</description>
        ///  </item>
        /// </list>
        /// </summary>
        /// <param name="matrixFactorization">The <see cref="LapackLinearEquations.IMatrixFactorization"/> object.</param>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="dl">The (<paramref name="n"/> - 1) subdiagonal elements of the input matrix; overwritten by the (<paramref name="n"/> - 1) multipliers that defines the matrix L from the LU factorization.</param>
        /// <param name="d">The <paramref name="n"/> diagonal elements of the input matrix; overwritten by the <paramref name="n"/> diagonal element reciprocals of the upper triangular matrix U from the LU factorization.</param>
        /// <param name="du">The (<paramref name="n"/> - 1) superdiagonal elements of the input matrix; overwritten by the (<paramref name="n"/> - 1) elements of the superdiagonal of U.</param>
        public static void ddttrfb(this LapackLinearEquations.IMatrixFactorization matrixFactorization, int n, double[] dl, double[] d, double[] du)
        {
            int info;
            _ddttrfb(ref n, dl, d, du, out info);
            CheckForError(info, "ddttrfb");
        }

        /// <summary>Computes the factorization of a diagonally dominant triangular matrix, i.e. A = L_1 * U * L_, 
        /// <list type="bullet">
        ///  <item>
        ///  <description>where L_1 and L_2 are unit lower bidiagonal with k and n - k - 1 subdiagonal elements, respectively, where k = n/2,</description>
        ///  </item>
        ///  <item>
        ///   <description>U is an upper bidiagonal matrix with nonzeroes in only the main diagonal and first superdiagonal.</description>
        ///  </item>
        /// </list>
        /// </summary>
        /// <param name="matrixFactorization">The <see cref="LapackLinearEquations.IMatrixFactorization"/> object.</param>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="dl">The (<paramref name="n"/> - 1) subdiagonal elements of the input matrix; overwritten by the (<paramref name="n"/> - 1) multipliers that defines the matrix L from the LU factorization.</param>
        /// <param name="d">The <paramref name="n"/> diagonal elements of the input matrix; overwritten by the <paramref name="n"/> diagonal element reciprocals of the upper triangular matrix U from the LU factorization.</param>
        /// <param name="du">The (<paramref name="n"/> - 1) superdiagonal elements of the input matrix; overwritten by the (<paramref name="n"/> - 1) elements of the superdiagonal of U.</param>
        public static void zdttrfb(this LapackLinearEquations.IMatrixFactorization matrixFactorization, int n, Complex[] dl, Complex[] d, Complex[] du)
        {
            int info;
            _zdttrfb(ref n, dl, d, du, out info);
            CheckForError(info, "zdttrfb");
        }

        /// <summary>Computes the Cholesky decomposition with complete pivoting of a real symmetric positive-definite matrix, i.e. P' * A * P = U' * U or P' * A * P = L * L', where L is a lower triangular matrix and U is upper triangular.
        /// </summary>
        /// <param name="matrixFactorization">The <see cref="LapackLinearEquations.IMatrixFactorization"/> object.</param>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="a">The matrix A supplied column-by-column of dimension (<paramref name="n"/>; <paramref name="n"/>); overwritten by the upper or lower triangular matrix U, L respectively.</param>
        /// <param name="pivot">The nonzero entries of permutation matrix P, i.e. P( pivot(k) , k ) = 1 for k = 1,.., <paramref name="n"/> - 1 (output).</param>
        /// <param name="work">A workspace array of length at least 2 * <paramref name="n"/>.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
        /// <param name="tolerance">The tolerance for the termination condition of the algorithm, i.e. if pivot elements &lt; <paramref name="tolerance"/>.</param>
        /// <returns>The rank of <paramref name="a"/> given by the number of steps the algorithm completed.</returns>
        public static int dpstrf(this LapackLinearEquations.IMatrixFactorization matrixFactorization, int n, double[] a, int[] pivot, double[] work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, double tolerance = MachineConsts.Epsilon)
        {
            int info;
            int lda = Math.Max(1, n);
            var uplo = GetUplo(triangularMatrixType);

            int rank;
            _dpstrf(ref uplo, ref n, a, ref lda, pivot, out rank, ref tolerance, work, out info);
            CheckForError(info, "dpstrf");
            return rank;
        }

        /// <summary>Computes the Cholesky decomposition with complete pivoting of a complex Hermitian positive-definite matrix, i.e. P' * A * P = U' * U or P' * A * P = L * L', where L is a lower triangular matrix and U is upper triangular.
        /// </summary>
        /// <param name="matrixFactorization">The <see cref="LapackLinearEquations.IMatrixFactorization"/> object.</param>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="a">The matrix A supplied column-by-column of dimension (<paramref name="n"/>; <paramref name="n"/>); overwritten by the upper or lower triangular matrix U, L respectively.</param>
        /// <param name="pivot">The nonzero entries of permutation matrix P, i.e. P( pivot(k) , k ) = 1 for k = 1,.., <paramref name="n"/> - 1 (output).</param>
        /// <param name="work">A workspace array of length at least 2 * <paramref name="n"/>.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
        /// <param name="tolerance">The tolerance for the termination condition of the algorithm, i.e. if pivot elements &lt; <paramref name="tolerance"/>.</param>
        /// <returns>The rank of <paramref name="a"/> given by the number of steps the algorithm completed.</returns>
        public static int zpstrf(this LapackLinearEquations.IMatrixFactorization matrixFactorization, int n, Complex[] a, int[] pivot, Complex[] work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, double tolerance = MachineConsts.Epsilon)
        {
            int info;
            int lda = Math.Max(1, n);
            var uplo = GetUplo(triangularMatrixType);

            int rank;
            _zpstrf(ref uplo, ref n, a, ref lda, pivot, out rank, ref tolerance, work, out info);
            CheckForError(info, "zpstrf");
            return rank;
        }
        #endregion
    }
}