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
using System.Numerics;
using System.Security;
using System.Runtime.InteropServices;

namespace Dodoni.MathLibrary.Basics.LowLevel.Native
{
    /// <summary>The native wrapper for matrix factorization.
    /// </summary>   
    internal class MatrixFactorization : LapackLinearEquations.IMatrixFactorization
    {
        #region private function import
#if LOWER_CASE_UNDERSCORE

        [DllImport(LapackNativeWrapper.dllName, EntryPoint = "dgetrf_", CallingConvention = LapackNativeWrapper.CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dgetrf(ref int m, ref int n, [In, Out] double[] a, ref int lda, [In, Out] int[] iPivot, out int info);

        [DllImport(LapackNativeWrapper.dllName, EntryPoint = "zgetrf_", CallingConvention = LapackNativeWrapper.CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zgetrf(ref int m, ref int n, [In, Out] Complex[] a, ref int lda, [In, Out] int[] iPivot, out int info);

        [DllImport(LapackNativeWrapper.dllName, EntryPoint = "dgbtrf_", CallingConvention = LapackNativeWrapper.CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dgbtrf(ref int m, ref int n, ref int kl, ref int ku, [In, Out] double[] ab, ref int ldab, [In, Out] int[] iPivot, out int info);

        [DllImport(LapackNativeWrapper.dllName, EntryPoint = "zgbtrf_", CallingConvention = LapackNativeWrapper.CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zgbtrf(ref int m, ref int n, ref int kl, ref int ku, [In, Out] Complex[] ab, ref int ldab, [In, Out] int[] iPivot, out int info);

        [DllImport(LapackNativeWrapper.dllName, EntryPoint = "dgttrf_", CallingConvention = LapackNativeWrapper.CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dgttrf(ref int n, [In, Out] double[] dl, [In, Out] double[] d, [In, Out] double[] du, [In, Out] double[] du2, [In, Out] int[] ipiv, out int info);

        [DllImport(LapackNativeWrapper.dllName, EntryPoint = "zgttrf_", CallingConvention = LapackNativeWrapper.CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zgttrf(ref int n, [In, Out] Complex[] dl, [In, Out] Complex[] d, [In, Out] Complex[] du, [In, Out] Complex[] du2, [In, Out] int[] ipiv, out int info);

        [DllImport(LapackNativeWrapper.dllName, EntryPoint = "dpotrf_", CallingConvention = LapackNativeWrapper.CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dpotrf(ref char uplo, ref int n, [In, Out] double[] a, ref int lda, out int info);

        [DllImport(LapackNativeWrapper.dllName, EntryPoint = "zpotrf_", CallingConvention = LapackNativeWrapper.CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zpotrf(ref char uplo, ref int n, [In, Out] Complex[] a, ref int lda, out int info);

        [DllImport(LapackNativeWrapper.dllName, EntryPoint = "dpftrf_", CallingConvention = LapackNativeWrapper.CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dpftrf(ref char transr, ref char uplo, ref int n, [In, Out] double[] a, out int info);

        [DllImport(LapackNativeWrapper.dllName, EntryPoint = "zpftrf", CallingConvention = LapackNativeWrapper.CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zpftrf(ref char transr, ref char uplo, ref int n, [In, Out] Complex[] a, out int info);

        [DllImport(LapackNativeWrapper.dllName, EntryPoint = "dpptrf", CallingConvention = LapackNativeWrapper.CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dpptrf(ref char uplo, ref int n, [In, Out] double[] ap, out int info);

        [DllImport(LapackNativeWrapper.dllName, EntryPoint = "zpptrf_", CallingConvention = LapackNativeWrapper.CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zpptrf(ref char uplo, ref int n, [In, Out] Complex[] ap, out int info);

        [DllImport(LapackNativeWrapper.dllName, EntryPoint = "dpbtrf_", CallingConvention = LapackNativeWrapper.CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dpbtrf(ref char uplo, ref int n, ref int kd, [In, Out] double[] ab, ref int ldab, out int info);

        [DllImport(LapackNativeWrapper.dllName, EntryPoint = "zpbtrf_", CallingConvention = LapackNativeWrapper.CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zpbtrf(ref char uplo, ref int n, ref int kd, [In, Out] Complex[] ab, ref int ldab, out int info);

        [DllImport(LapackNativeWrapper.dllName, EntryPoint = "dpttrf_", CallingConvention = LapackNativeWrapper.CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dpttrf(ref int n, [In, Out] double[] d, [In, Out] double[] e, out int info);

        [DllImport(LapackNativeWrapper.dllName, EntryPoint = "zpttrf_", CallingConvention = LapackNativeWrapper.CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zpttrf(ref int n, [In, Out] Complex[] d, [In, Out] Complex[] e, out int info);

        [DllImport(LapackNativeWrapper.dllName, EntryPoint = "dsytrf_", CallingConvention = LapackNativeWrapper.CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dsytrf(ref char uplo, ref int n, [In, Out] double[] a, ref int lda, [In, Out] int[] iPiv, [In, Out] double[] work, ref int lwork, out int info);

        [DllImport(LapackNativeWrapper.dllName, EntryPoint = "dsytrf_", CallingConvention = LapackNativeWrapper.CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void _dsytrf(ref char uplo, ref int n, [In, Out] double[] a, ref int lda, [In, Out] int[] iPiv, double* work, ref int lwork, out int info);

        [DllImport(LapackNativeWrapper.dllName, EntryPoint = "zsytrf_", CallingConvention = LapackNativeWrapper.CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zsytrf(ref char uplo, ref int n, [In, Out] Complex[] a, ref int lda, [In, Out] int[] iPiv, [In, Out] Complex[] work, ref int lwork, out int info);

        [DllImport(LapackNativeWrapper.dllName, EntryPoint = "zsytrf_", CallingConvention = LapackNativeWrapper.CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void _zsytrf(ref char uplo, ref int n, [In, Out] Complex[] a, ref int lda, [In, Out] int[] iPiv, Complex* work, ref int lwork, out int info);

        [DllImport(LapackNativeWrapper.dllName, EntryPoint = "zhetrf_", CallingConvention = LapackNativeWrapper.CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zhetrf(ref char uplo, ref int n, [In, Out] Complex[] a, ref int lda, [In, Out] int[] ipiv, [In, Out] Complex[] work, ref int lwork, out int info);

        [DllImport(LapackNativeWrapper.dllName, EntryPoint = "zhetrf_", CallingConvention = LapackNativeWrapper.CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void _zhetrf(ref char uplo, ref int n, [In, Out] Complex[] a, ref int lda, [In, Out] int[] ipiv, Complex* work, ref int lwork, out int info);

        [DllImport(LapackNativeWrapper.dllName, EntryPoint = "dsptrf_", CallingConvention = LapackNativeWrapper.CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dsptrf(ref char uplo, ref int n, [In, Out] double[] ap, [In, Out] int[] iPiv, out int info);

        [DllImport(LapackNativeWrapper.dllName, EntryPoint = "zsptrf_", CallingConvention = LapackNativeWrapper.CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zsptrf(ref char uplo, ref int n, [In, Out] Complex[] ap, [In, Out] int[] iPiv, out int info);

        [DllImport(LapackNativeWrapper.dllName, EntryPoint = "zhptrf_", CallingConvention = LapackNativeWrapper.CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zhptrf(ref char uplo, ref int n, [In, Out] Complex[] ap, [In, Out] int[] ipiv, out int info);
#else

        [DllImport(LapackNativeWrapper.sm_DllName, EntryPoint = "DGETRF", CallingConvention = LapackNativeWrapper.sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dgetrf(ref int m, ref int n, [In, Out] double[] a, ref int lda, [In, Out] int[] iPivot, out int info);

        [DllImport(LapackNativeWrapper.sm_DllName, EntryPoint = "ZGETRF", CallingConvention = LapackNativeWrapper.sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zgetrf(ref int m, ref int n, [In, Out] Complex[] a, ref int lda, [In, Out] int[] iPivot, out int info);

        [DllImport(LapackNativeWrapper.sm_DllName, EntryPoint = "DGBTRF", CallingConvention = LapackNativeWrapper.sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dgbtrf(ref int m, ref int n, ref int kl, ref int ku, [In, Out] double[] ab, ref int ldab, [In, Out] int[] iPivot, out int info);

        [DllImport(LapackNativeWrapper.sm_DllName, EntryPoint = "ZGBTRF", CallingConvention = LapackNativeWrapper.sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zgbtrf(ref int m, ref int n, ref int kl, ref int ku, [In, Out] Complex[] ab, ref int ldab, [In, Out] int[] iPivot, out int info);

        [DllImport(LapackNativeWrapper.sm_DllName, EntryPoint = "DGTTRF", CallingConvention = LapackNativeWrapper.sm_CallingConvention), SuppressUnmanagedCodeSecurity]        
        private static extern void _dgttrf(ref int n, [In, Out] double[] dl, [In, Out] double[] d, [In, Out] double[] du, [In, Out] double[] du2, [In, Out] int[] ipiv, out int info);

        [DllImport(LapackNativeWrapper.sm_DllName, EntryPoint = "ZGTTRF", CallingConvention = LapackNativeWrapper.sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zgttrf(ref int n, [In, Out] Complex[] dl, [In, Out] Complex[] d, [In, Out] Complex[] du, [In, Out] Complex[] du2, [In, Out] int[] ipiv, out int info);

        [DllImport(LapackNativeWrapper.sm_DllName, EntryPoint = "DPOTRF", CallingConvention = LapackNativeWrapper.sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dpotrf(ref char uplo, ref int n, [In, Out] double[] a, ref int lda, out int info);

        [DllImport(LapackNativeWrapper.sm_DllName, EntryPoint = "ZPOTRF", CallingConvention = LapackNativeWrapper.sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zpotrf(ref char uplo, ref int n, [In, Out] Complex[] a, ref int lda, out int info);

        [DllImport(LapackNativeWrapper.sm_DllName, EntryPoint = "DPFTRF", CallingConvention = LapackNativeWrapper.sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dpftrf(ref char transr, ref char uplo, ref int n, [In, Out] double[] a, out int info);

        [DllImport(LapackNativeWrapper.sm_DllName, EntryPoint = "ZPFTRF", CallingConvention = LapackNativeWrapper.sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zpftrf(ref char transr, ref char uplo, ref int n, [In, Out] Complex[] a, out int info);

        [DllImport(LapackNativeWrapper.sm_DllName, EntryPoint = "DPPTRF", CallingConvention = LapackNativeWrapper.sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dpptrf(ref char uplo, ref int n, [In, Out] double[] ap, out int info);

        [DllImport(LapackNativeWrapper.sm_DllName, EntryPoint = "ZPPTRF", CallingConvention = LapackNativeWrapper.sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zpptrf(ref char uplo, ref int n, [In, Out] Complex[] ap, out int info);

        [DllImport(LapackNativeWrapper.sm_DllName, EntryPoint = "DPBTRF", CallingConvention = LapackNativeWrapper.sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dpbtrf(ref char uplo, ref int n, ref int kd, [In, Out] double[] ab, ref int ldab, out int info);

        [DllImport(LapackNativeWrapper.sm_DllName, EntryPoint = "ZPBTRF", CallingConvention = LapackNativeWrapper.sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zpbtrf(ref char uplo, ref int n, ref int kd, [In, Out] Complex[] ab, ref int ldab, out int info);

        [DllImport(LapackNativeWrapper.sm_DllName, EntryPoint = "DPTTRF", CallingConvention = LapackNativeWrapper.sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dpttrf(ref int n, [In, Out] double[] d, [In, Out] double[] e, out int info);

        [DllImport(LapackNativeWrapper.sm_DllName, EntryPoint = "ZPTTRF", CallingConvention = LapackNativeWrapper.sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zpttrf(ref int n, [In, Out] Complex[] d, [In, Out] Complex[] e, out int info);

        [DllImport(LapackNativeWrapper.sm_DllName, EntryPoint = "DSYTRF", CallingConvention = LapackNativeWrapper.sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dsytrf(ref char uplo, ref int n, [In, Out] double[] a, ref int lda, [In, Out] int[] iPiv, [In, Out] double[] work, ref int lwork, out int info);

        [DllImport(LapackNativeWrapper.sm_DllName, EntryPoint = "DSYTRF", CallingConvention = LapackNativeWrapper.sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void _dsytrf(ref char uplo, ref int n, [In, Out] double[] a, ref int lda, [In, Out] int[] iPiv, double* work, ref int lwork, out int info);

        [DllImport(LapackNativeWrapper.sm_DllName, EntryPoint = "ZSYTRF", CallingConvention = LapackNativeWrapper.sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zsytrf(ref char uplo, ref int n, [In, Out] Complex[] a, ref int lda, [In, Out] int[] iPiv, [In, Out] Complex[] work, ref int lwork, out int info);

        [DllImport(LapackNativeWrapper.sm_DllName, EntryPoint = "ZSYTRF", CallingConvention = LapackNativeWrapper.sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void _zsytrf(ref char uplo, ref int n, [In, Out] Complex[] a, ref int lda, [In, Out] int[] iPiv, Complex* work, ref int lwork, out int info);

        [DllImport(LapackNativeWrapper.sm_DllName, EntryPoint = "ZHETRF", CallingConvention = LapackNativeWrapper.sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zhetrf(ref char uplo, ref int n, [In, Out] Complex[] a, ref int lda, [In, Out] int[] ipiv, [In, Out] Complex[] work, ref int lwork, out int info);

        [DllImport(LapackNativeWrapper.sm_DllName, EntryPoint = "ZHETRF", CallingConvention = LapackNativeWrapper.sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void _zhetrf(ref char uplo, ref int n, [In, Out] Complex[] a, ref int lda, [In, Out] int[] ipiv, Complex* work, ref int lwork, out int info);

        [DllImport(LapackNativeWrapper.sm_DllName, EntryPoint = "DSPTRF", CallingConvention = LapackNativeWrapper.sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dsptrf(ref char uplo, ref int n, [In, Out] double[] ap, [In, Out] int[] iPiv, out int info);

        [DllImport(LapackNativeWrapper.sm_DllName, EntryPoint = "ZSPTRF", CallingConvention = LapackNativeWrapper.sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zsptrf(ref char uplo, ref int n, [In, Out] Complex[] ap, [In, Out] int[] iPiv, out int info);

        [DllImport(LapackNativeWrapper.sm_DllName, EntryPoint = "ZHPTRF", CallingConvention = LapackNativeWrapper.sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zhptrf(ref char uplo, ref int n, [In, Out] Complex[] ap, [In, Out] int[] ipiv, out int info);
#endif
        #endregion

        #region IMatrixFactorization Members

        /// <summary>Computes the LU factorization of a general m-by-n matrix, i.e. A = P * L * U, where P is a permutation matrix, L is lower triangular with unit diagonal elements and U is upper triangular.
        /// </summary>
        /// <param name="m">The number of rows of the matrix.</param>
        /// <param name="n">The number of columns of the matrix.</param>
        /// <param name="a">The matrix A supplied column-by-column of dimension (<paramref name="m"/>; <paramref name="n"/>); overwritten by L and U, the unit diagonal elements of L are not stored; output.</param>
        /// <param name="iPivot">The min(<paramref name="m"/>,<paramref name="n"/>) pivot indices, i.e. row j was interchanged with row <paramref name="iPivot"/>[j] (output).</param>
        public void dgetrf(int m, int n, double[] a, int[] iPivot)
        {
            int info;
            int lda = m;

            _dgetrf(ref m, ref n, a, ref lda, iPivot, out info);
            LapackNativeWrapper.CheckForError(info, "dgetrf");
        }

        /// <summary>Computes the LU factorization of a general m-by-n matrix, i.e. A = P * L * U, where P is a permutation matrix, L is lower triangular with unit diagonal elements and U is upper triangular.
        /// </summary>
        /// <param name="m">The number of rows of the matrix.</param>
        /// <param name="n">The number of columns of the matrix.</param>
        /// <param name="a">The matrix A supplied column-by-column of dimension (<paramref name="m"/>; <paramref name="n"/>); overwritten by 'L' and 'U', the unit diagonal elements of 'L' are not stored; output.</param>
        /// <param name="iPivot">The min(<paramref name="m"/>,<paramref name="n"/>) pivot indices, i.e. row j was interchanged with row <paramref name="iPivot"/>[j] (output).</param>
        public void zgetrf(int m, int n, Complex[] a, int[] iPivot)
        {
            int info;
            int lda = m;

            _zgetrf(ref m, ref n, a, ref lda, iPivot, out info);
            LapackNativeWrapper.CheckForError(info, "zgetrf");
        }

        /// <summary>Computes the LU factorization of a general m-by-n band matrix, i.e. A = P * L * U, where P is a permutation matrix, L is lower triangular with unit diagonal elements and U is upper triangular.
        /// </summary>
        /// <param name="m">The number of rows of the matrix.</param>
        /// <param name="n">The number of columns of the matrix.</param>
        /// <param name="kl">The number of subdiagonals within the band of the input matrix.</param>
        /// <param name="ku">The number of superdiagonals within the band of the input matrix.</param>
        /// <param name="a">The input band matrix in band storage, i.e. of dimension (2 * <paramref name="kl"/> + <paramref name="ku"/> + 1; <paramref name="n"/>); overwritten by L and U. U is stored as an upper triangular band matrix with <paramref name="kl"/> + <paramref name="ku"/> superdiagonals.</param>
        /// <param name="iPivot">The pivot indices, i.e. row j was interchanged with row <paramref name="iPivot"/>[j]; must contain at least min(<paramref name="m"/>,<paramref name="n"/>) elements (output).</param>
        public void dgbtrf(int m, int n, int kl, int ku, double[] a, int[] iPivot)
        {
            int info;
            int lda = 2 * kl + ku + 1;

            _dgbtrf(ref m, ref n, ref kl, ref ku, a, ref lda, iPivot, out info);
            LapackNativeWrapper.CheckForError(info, "dgbtrf");
        }

        /// <summary>Computes the LU factorization of a general m-by-n band matrix, i.e. A = P * L * U, where P is a permutation matrix, L is lower triangular with unit diagonal elements and U is upper triangular.
        /// </summary>
        /// <param name="m">The number of rows of the matrix.</param>
        /// <param name="n">The number of columns of the matrix.</param>
        /// <param name="kl">The number of subdiagonals within the band of the input matrix.</param>
        /// <param name="ku">The number of superdiagonals within the band of the input matrix.</param>
        /// <param name="a">The input band matrix in band storage, i.e. of dimension (2 * <paramref name="kl"/> + <paramref name="ku"/> + 1; <paramref name="n"/>); overwritten by L and U. U is stored as an upper triangular band matrix with <paramref name="kl"/> + <paramref name="ku"/> superdiagonals.</param>
        /// <param name="iPivot">The pivot indices, i.e. row j was interchanged with row <paramref name="iPivot"/>[j]; must contain at least min(<paramref name="m"/>,<paramref name="n"/>) elements (output).</param>
        public void zgbtrf(int m, int n, int kl, int ku, Complex[] a, int[] iPivot)
        {
            int info;
            int lda = 2 * kl + ku + 1;

            _zgbtrf(ref m, ref n, ref kl, ref ku, a, ref lda, iPivot, out info);
            LapackNativeWrapper.CheckForError(info, "zgbtrf");
        }

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
        public void dgttrf(int n, double[] dl, double[] d, double[] du, double[] du2, int[] iPivot)
        {
            int info;

            _dgttrf(ref n, dl, d, du, du2, iPivot, out info);
            LapackNativeWrapper.CheckForError(info, "dgttrf");
        }

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
        public void zgttrf(int n, Complex[] dl, Complex[] d, Complex[] du, Complex[] du2, int[] iPivot)
        {
            int info;

            _zgttrf(ref n, dl, d, du, du2, iPivot, out info);
            LapackNativeWrapper.CheckForError(info, "zgttrf");
        }

        /// <summary>Computes the Cholesky decomposition of a symmetric positive-definite matrix, i.e. A = U' * U or A = L * L', where L is a lower triangular matrix and U is upper triangular.
        /// </summary>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="a">The matrix A supplied column-by-column of dimension (<paramref name="n" />; <paramref name="n" />); overwritten by the upper or lower triangular matrix U, L respectively.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
        public void dpotrf(int n, double[] a, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var uplo = LapackNativeWrapper.GetUplo(triangularMatrixType);

            _dpotrf(ref uplo, ref n, a, ref n, out info);
            LapackNativeWrapper.CheckForError(info, "dpotrf");
        }

        /// <summary>Computes the Cholesky decomposition of a Hermitian positive-definite matrix, i.e. A = conj(U') * U or A = L * conj(L'), where L is a lower triangular matrix and U is upper triangular.
        /// </summary>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="a">The matrix A supplied column-by-column of dimension (<paramref name="n"/>; <paramref name="n"/>); overwritten by the upper or lower triangular matrix U, L respectively.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
        public void zpotrf(int n, Complex[] a, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var uplo = LapackNativeWrapper.GetUplo(triangularMatrixType);

            _zpotrf(ref uplo, ref n, a, ref n, out info);
            LapackNativeWrapper.CheckForError(info, "zpotrf");
        }

        /// <summary>Computes the Cholesky factorization of a symmetric positive-definite matrix using the Rectangular Full Packed (RFP) format, i.e.
        /// A = U' * U or A = L * L', where L is a lower triangular matrix and U is upper triangular.
        /// </summary>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="a">The matrix A in the RFP format, i.e. an array with at least <paramref name="n"/> * (<paramref name="n"/> + 1) / 2 elements; overwritten by the upper or lower triangular matrix U, L respectively.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
        /// <param name="transposeState">A value indicating whether <paramref name="a"/> represents matrix A or its transposed.</param>
        public void dpftrf(int n, double[] a, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            int info;
            var trans = LapackNativeWrapper.GetTrans(transposeState);
            var uplo = LapackNativeWrapper.GetUplo(triangularMatrixType);

            _dpftrf(ref trans, ref uplo, ref n, a, out info);
            LapackNativeWrapper.CheckForError(info, "dpftrf");
        }

        /// <summary>Computes the Cholesky factorization of a Hermitian positive-definite matrix using the Rectangular Full Packed (RFP) format, i.e.
        /// A = conj(U') * U or A = L * conj(L'), where L is a lower triangular matrix and U is upper triangular.
        /// </summary>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="a">The matrix A in the RFP format, i.e. an array with at least <paramref name="n"/> * (<paramref name="n"/> + 1) / 2 elements; overwritten by the upper or lower triangular matrix U, L respectively.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
        /// <param name="transposeState">A value indicating whether <paramref name="a"/> represents matrix A or its transposed.</param>
        public void zpftrf(int n, Complex[] a, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            int info;
            var trans = LapackNativeWrapper.GetTrans(transposeState);
            var uplo = LapackNativeWrapper.GetUplo(triangularMatrixType);

            _zpftrf(ref trans, ref uplo, ref n, a, out info);
            LapackNativeWrapper.CheckForError(info, "zpftrf");
        }

        /// <summary>Computes the Cholesky factorization of a symmetric positive-definite matrix using packed storage, i.e.
        /// A = U' * U or A = L * L', where L is a lower triangular matrix and U is upper triangular.
        /// </summary>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="aPacked">Either the upper or lower triangular part of matrix A in packed storage, i.e. at least <paramref name="n"/> * (<paramref name="n"/> + 1)/2 elements; overwritten by the upper or lower triangular matrix U, L respectively in packed storage.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
        public void dpptrf(int n, double[] aPacked, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var uplo = LapackNativeWrapper.GetUplo(triangularMatrixType);

            _dpptrf(ref uplo, ref n, aPacked, out info);
            LapackNativeWrapper.CheckForError(info, "dpptrf");
        }

        /// <summary>Computes the Cholesky factorization of a Hermitian positive-definite matrix using packed storage, i.e.
        /// A = conj(U') * U or A = L * conj(L'), where L is a lower triangular matrix and U is upper triangular.
        /// </summary>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="aPacked">Either the upper or lower triangular part of matrix A in packed storage, i.e. at least <paramref name="n" /> * (<paramref name="n" /> + 1)/2 elements; overwritten by the upper or lower triangular matrix U, L respectively in packed storage.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
        public void zpptrf(int n, Complex[] aPacked, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var uplo = LapackNativeWrapper.GetUplo(triangularMatrixType);

            _zpptrf(ref uplo, ref n, aPacked, out info);
            LapackNativeWrapper.CheckForError(info, "zpptrf");
        }

        /// <summary>Computes the Cholesky factorization of a symmetric positive-definite band matrix, i.e. i.e. A = U' * U or A = L * L', where L is a lower triangular matrix and U is upper triangular.
        /// </summary>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="kd">The number of superdiagonals or subdiagonals in the input matrix.</param>
        /// <param name="a">Either the upper or lower triangular part of the input matrix in band storage of dimension (<paramref name="kd"/> + 1; <paramref name="n"/>); overwritten by the upper or lower triangular matrix U, L respectively.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
        public void dpbtrf(int n, int kd, double[] a, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            int lda = kd + 1;
            var uplo = LapackNativeWrapper.GetUplo(triangularMatrixType);

            _dpbtrf(ref uplo, ref n, ref kd, a, ref lda, out info);
            LapackNativeWrapper.CheckForError(info, "dpbtrf");
        }

        /// <summary>Computes the Cholesky factorization of a Hermitian positive-definite band matrix, i.e. i.e. A = conj(U') * U or A = L * conj(L'), where L is a lower triangular matrix and U is upper triangular.
        /// </summary>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="kd">The number of superdiagonals or subdiagonals in the input matrix.</param>
        /// <param name="a">Either the upper or lower triangular part of the input matrix in band storage of dimension (<paramref name="kd"/> + 1; <paramref name="n"/>); overwritten by the upper or lower triangular matrix U, L respectively.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
        public void zpbtrf(int n, int kd, Complex[] a, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            int lda = kd + 1;
            var uplo = LapackNativeWrapper.GetUplo(triangularMatrixType);

            _zpbtrf(ref uplo, ref n, ref kd, a, ref lda, out info);
            LapackNativeWrapper.CheckForError(info, "zpbtrf");
        }

        /// <summary>Computes the factorization of a symmetric positive-definite tridiagonal matrix, i.e. A = L * D * L' or A = U' * D * U, where D is diagonal, L is unit lower bidiagonal and U is unit upper bidiagonal.
        /// </summary>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="diagonalElements">The <paramref name="n"/> diagonal elements of the input matrix; overwritten by the <paramref name="n"/> diagonal elements of the diagonal matrix D.</param>
        /// <param name="e">The <paramref name="n"/> - 1 subdiagonal elements of the input matrix; overwritten by the (<paramref name="n"/> - 1) off-diagonal elements of the unit bidiagonal factor L or U from the factorization.</param>
        public void dpttrf(int n, double[] diagonalElements, double[] e)
        {
            int info;

            _dpttrf(ref n, diagonalElements, e, out info);
            LapackNativeWrapper.CheckForError(info, "dpttrf");
        }

        /// <summary>Computes the factorization of a Hermitian positive-definite tridiagonal matrix, i.e. A = L * D * conj(L') or A = conj(U') * D * U, where D is diagonal, L is unit lower bidiagonal and U is unit upper bidiagonal.
        /// </summary>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="diagonalElements">The <paramref name="n"/> diagonal elements of the input matrix; overwritten by the <paramref name="n"/> diagonal elements of the diagonal matrix D.</param>
        /// <param name="e">The <paramref name="n"/> - 1 subdiagonal elements of the input matrix; overwritten by the (<paramref name="n"/> - 1) off-diagonal elements of the unit bidiagonal factor L or U from the factorization.</param>
        public void zpttrf(int n, Complex[] diagonalElements, Complex[] e)
        {
            int info;

            _zpttrf(ref n, diagonalElements, e, out info);
            LapackNativeWrapper.CheckForError(info, "zpttrf");
        }

        /// <summary>Gets a optimal workspace array length for the <c>dsytrf</c> function.
        /// </summary>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
        /// <returns>The optimal workspace array length.</returns>
        /// <remarks>The parameter <paramref name="triangularMatrixType"/> should not have an impact of the calculation of the optimal length of the workspace array.</remarks>
        public int dsytrfQuery(int n, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            var lwork = -1;
            var uplo = LapackNativeWrapper.GetUplo(triangularMatrixType);

            unsafe
            {
                double* work = stackalloc double[1];

                int info;
                _dsytrf(ref uplo, ref n, null, ref n, null, work, ref lwork, out info);
                LapackNativeWrapper.CheckForError(info, "dsytrf");

                return ((int)work[0]) + 1;
            }
        }

        /// <summary>Computes the Bunch-Kaufman factorization of a symmetric matrix, i.e. A = P * U * D * U' * P' or A = P * L * D * L' * P', where P is a permutation matrix, U and L are upper and
        /// lower triangular matrices with unit diagonal and D is a symmetric block-diagonal matrix with 1-by-1 and 2-by-2 diagonal blocks. U and L have 2-by-2 unit diagonal blocks corresponding to the 2-by-2 blocks of D.
        /// </summary>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="a">The upper or the lower triangular part of the input matrix of dimension (<paramref name="n"/>; <paramref name="n"/>); overwritten by details of the block-diagonal matrix D and the multiplies used to obtain the factor U (or L).</param>
        /// <param name="iPivot">Contains details of the interchanges an the block structure of D, at least <paramref name="n"/> elements (output).</param>
        /// <param name="work">A workspace array of length at least <paramref name="n"/>.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
        public void dsytrf(int n, double[] a, int[] iPivot, double[] work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            int lwork = work.Length;
            var uplo = LapackNativeWrapper.GetUplo(triangularMatrixType);

            _dsytrf(ref uplo, ref n, a, ref n, iPivot, work, ref lwork, out info);
            LapackNativeWrapper.CheckForError(info, "dsytrf");
        }

        /// <summary>Gets a optimal workspace array length for the <c>zsytrf</c> function.
        /// </summary>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
        /// <returns>The optimal workspace array length.</returns>
        /// <remarks>The parameter <paramref name="triangularMatrixType"/> should not have an impact of the calculation of the optimal length of the workspace array.</remarks>
        public int zsytrfQuery(int n, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            var lwork = -1;
            var uplo = LapackNativeWrapper.GetUplo(triangularMatrixType);

            unsafe
            {
                Complex* work = stackalloc Complex[1];

                int info;
                _zsytrf(ref uplo, ref n, null, ref n, null, work, ref lwork, out info);
                LapackNativeWrapper.CheckForError(info, "zsytrf");

                return ((int)work[0].Real) + 1;
            }
        }

        /// <summary>Computes the Bunch-Kaufman factorization of a symmetric matrix, i.e. A = P * U * D * U' * P' or A = P * L * D * L' * P', where P is a permutation matrix, U and L are upper and
        /// lower triangular matrices with unit diagonal and D is a symmetric block-diagonal matrix with 1-by-1 and 2-by-2 diagonal blocks. U and L have 2-by-2 unit diagonal blocks corresponding to the 2-by-2 blocks of D.
        /// </summary>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="a">The upper or the lower triangular part of the input matrix of dimension (<paramref name="n"/>; <paramref name="n"/>); overwritten by details of the block-diagonal matrix D and the multiplies used to obtain the factor U (or L).</param>
        /// <param name="iPivot">Contains details of the interchanges an the block structure of D, at least <paramref name="n"/> elements (output).</param>
        /// <param name="work">A workspace array of length at least <paramref name="n"/>.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
        public void zsytrf(int n, Complex[] a, int[] iPivot, Complex[] work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            int lwork = work.Length;
            var uplo = LapackNativeWrapper.GetUplo(triangularMatrixType);

            _zsytrf(ref uplo, ref n, a, ref n, iPivot, work, ref lwork, out info);
            LapackNativeWrapper.CheckForError(info, "zsytrf");
        }

        /// <summary>Gets a optimal workspace array length for the <c>zhetrf</c> function.
        /// </summary>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
        /// <returns>The optimal workspace array length.</returns>
        /// <remarks>The parameter <paramref name="triangularMatrixType"/> should not have an impact of the calculation of the optimal length of the workspace array.</remarks>
        public int zhetrfQuery(int n, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            var lwork = -1;
            var uplo = LapackNativeWrapper.GetUplo(triangularMatrixType);

            unsafe
            {
                Complex* work = stackalloc Complex[1];

                int info;
                _zhetrf(ref uplo, ref n, null, ref n, null, work, ref lwork, out info);
                LapackNativeWrapper.CheckForError(info, "zhetrf");

                return ((int)work[0].Real) + 1;
            }
        }

        /// <summary>Computes the Bunch-Kaufman factorization of a complex Hermitian matrix, i.e. A = P * U * D * conj(U') * conj(P') or A = P * L * D * conj(L') * conj(P'), where P is a permutation matrix, U and L are upper and
        /// lower triangular matrices with unit diagonal and D is a symmetric block-diagonal matrix with 1-by-1 and 2-by-2 diagonal blocks. U and L have 2-by-2 unit diagonal blocks corresponding to the 2-by-2 blocks of D.
        /// </summary>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="a">The upper or the lower triangular part of the input matrix of dimension (<paramref name="n"/>; <paramref name="n"/>); overwritten by details of the block-diagonal matrix D and the multiplies used to obtain the factor U (or L).</param>
        /// <param name="iPivot">Contains details of the interchanges an the block structure of D, at least <paramref name="n"/> elements (output).</param>
        /// <param name="work">A workspace array of length at least <paramref name="n"/>.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
        public void zhetrf(int n, Complex[] a, int[] iPivot, Complex[] work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            int lwork = work.Length;
            var uplo = LapackNativeWrapper.GetUplo(triangularMatrixType);

            _zhetrf(ref uplo, ref n, a, ref n, iPivot, work, ref lwork, out info);
            LapackNativeWrapper.CheckForError(info, "zhetrf");
        }

        /// <summary>Computes the Bunch-Kaufman factorization of a symmetric matrix using packed storage, i.e. A = P * U * D * U' * P' or A = P * L * D * L' * P', where P is a permutation matrix, U and L are upper and
        /// lower triangular matrices with unit diagonal and D is a symmetric block-diagonal matrix with 1-by-1 and 2-by-2 diagonal blocks. U and L have 2-by-2 unit diagonal blocks corresponding to the 2-by-2 blocks of D.
        /// </summary>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="aPacked">Either the upper or lower triangular part of matrix A in packed storage, i.e. at least <paramref name="n"/> * (<paramref name="n"/> + 1)/2 elements; overwritten by details of the block-diagonal matrix D and the multiplies used to obtain the factor U (or L).</param>
        /// <param name="iPivot">Contains details of the interchanges an the block structure of D, at least <paramref name="n"/> elements (output).</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
        public void dsptrf(int n, double[] aPacked, int[] iPivot, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var uplo = LapackNativeWrapper.GetUplo(triangularMatrixType);

            _dsptrf(ref uplo, ref n, aPacked, iPivot, out info);
            LapackNativeWrapper.CheckForError(info, "dsptrf");
        }

        /// <summary>Computes the Bunch-Kaufman factorization of a symmetric matrix using packed storage, i.e. A = P * U * D * U' * P' or A = P * L * D * L' * P', where P is a permutation matrix, U and L are upper and
        /// lower triangular matrices with unit diagonal and D is a symmetric block-diagonal matrix with 1-by-1 and 2-by-2 diagonal blocks. U and L have 2-by-2 unit diagonal blocks corresponding to the 2-by-2 blocks of D.
        /// </summary>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="aPacked">Either the upper or lower triangular part of matrix A in packed storage, i.e. at least <paramref name="n"/> * (<paramref name="n"/> + 1)/2 elements; overwritten by details of the block-diagonal matrix D and the multiplies used to obtain the factor U (or L).</param>
        /// <param name="iPivot">Contains details of the interchanges an the block structure of D, at least <paramref name="n"/> elements (output).</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
        public void zsptrf(int n, Complex[] aPacked, int[] iPivot, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var uplo = LapackNativeWrapper.GetUplo(triangularMatrixType);

            _zsptrf(ref uplo, ref n, aPacked, iPivot, out info);
            LapackNativeWrapper.CheckForError(info, "zsptrf");
        }

        /// <summary>Computes the Bunch-Kaufman factorization of a Hermitian matrix using packed storage, i.e. A = P * U * D * conj(U') * conj(P') or A = P * L * D * conj(L') * conj(P'), where P is a permutation matrix, U and L are upper and
        /// lower triangular matrices with unit diagonal and D is a symmetric block-diagonal matrix with 1-by-1 and 2-by-2 diagonal blocks. U and L have 2-by-2 unit diagonal blocks corresponding to the 2-by-2 blocks of D.
        /// </summary>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="aPacked">Either the upper or lower triangular part of matrix A in packed storage, i.e. at least <paramref name="n"/> * (<paramref name="n"/> + 1)/2 elements; overwritten by details of the block-diagonal matrix D and the multiplies used to obtain the factor U (or L).</param>
        /// <param name="iPivot">Contains details of the interchanges an the block structure of D, at least <paramref name="n"/> elements (output).</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
        public void zhptrf(int n, Complex[] aPacked, int[] iPivot, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var uplo = LapackNativeWrapper.GetUplo(triangularMatrixType);

            _zhptrf(ref uplo, ref n, aPacked, iPivot, out info);
            LapackNativeWrapper.CheckForError(info, "zhptrf");
        }
        #endregion

        #region internal static methods

        /// <summary>Creates a new <see cref="MatrixFactorization"/> instance.
        /// </summary>
        /// <returns>A new <see cref="MatrixFactorization"/> instance.</returns>
        internal static MatrixFactorization Create() => new MatrixFactorization();
        #endregion
    }
}