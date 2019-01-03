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
using System.Numerics;
using System.Security;
using System.Runtime.InteropServices;

namespace Dodoni.MathLibrary.Basics.LowLevel.Native
{
    internal partial class LapackNativeWrapper : LapackLinearEquations.ISolver
    {
        #region private function import

        [DllImport(sm_DllName, EntryPoint = "DGETRS", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dgetrs(ref char trans, ref int n, ref int nrhs, [In, Out] double[] a, ref int lda, [In, Out] int[] ipiv, [In, Out] double[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZGETRS", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zgetrs(ref char trans, ref int n, ref int nrhs, [In, Out] Complex[] a, ref int lda, [In, Out] int[] ipiv, [In, Out] Complex[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "DGBTRS", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dgbtrs(ref char trans, ref int n, ref int kl, ref int ku, ref int nrhs, [In, Out] double[] ab, ref int ldab, [In, Out] int[] ipiv, [In, Out] double[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZGBTRS", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zgbtrs(ref char trans, ref int n, ref int kl, ref int ku, ref int nrhs, [In, Out] Complex[] ab, ref int ldab, [In, Out] int[] ipiv, [In, Out] Complex[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "DGTTRS", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dgttrs(ref char trans, ref int n, ref int nrhs, [In, Out] double[] dl, [In, Out] double[] d, [In, Out] double[] du, [In, Out] double[] du2, [In, Out] int[] ipiv, [In, Out] double[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZGTTRS", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zgttrs(ref char trans, ref int n, ref int nrhs, [In, Out] Complex[] dl, [In, Out] Complex[] d, [In, Out] Complex[] du, [In, Out] Complex[] du2, [In, Out] int[] ipiv, [In, Out] Complex[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "DPOTRS", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dpotrs(ref char uplo, ref int n, ref int nrhs, [In, Out] double[] a, ref int lda, [In, Out] double[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZPOTRS", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zpotrs(ref char uplo, ref int n, ref int nrhs, [In, Out] Complex[] a, ref int lda, [In, Out] Complex[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "DPFTRS", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dpftrs(ref char transr, ref char uplo, ref int n, ref int nrhs, [In, Out] double[] a, [In, Out] double[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZPFTRS", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zpftrs(ref char transr, ref char uplo, ref int n, ref int nrhs, [In, Out] Complex[] a, [In, Out] Complex[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "DPPTRS", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dpptrs(ref char uplo, ref int n, ref int nrhs, [In, Out] double[] ap, [In, Out] double[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZPPTRS", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zpptrs(ref char uplo, ref int n, ref int nrhs, [In, Out] Complex[] ap, [In, Out] Complex[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "DPBTRS", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dpbtrs(ref char uplo, ref int n, ref int kd, ref int nrhs, [In, Out] double[] ab, ref int ldab, [In, Out] double[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZPBTRS", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zpbtrs(ref char uplo, ref int n, ref int kd, ref int nrhs, [In, Out] Complex[] ab, ref int ldab, [In, Out] Complex[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "DPTTRS", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dpttrs(ref int n, ref int nrhs, [In, Out] double[] d, [In, Out] double[] e, [In, Out] double[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZPTTRS", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zpttrs(ref char uplo, ref int n, ref int nrhs, [In, Out] double[] d, [In, Out] Complex[] e, [In, Out] Complex[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "DSYTRS", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dsytrs(ref char uplo, ref int n, ref int nrhs, [In, Out] double[] a, ref int lda, [In, Out] int[] ipiv, [In, Out] double[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZSYTRS", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zsytrs(ref char uplo, ref int n, ref int nrhs, [In, Out] Complex[] a, ref int lda, [In, Out] int[] ipiv, [In, Out] Complex[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZHETRS", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zhetrs(ref char uplo, ref int n, ref int nrhs, [In, Out] Complex[] a, ref int lda, [In, Out] int[] ipiv, [In, Out] Complex[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "DSYTRS2", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dsytrs2(ref char uplo, ref int n, ref int nrhs, [In, Out] double[] a, ref int lda, [In, Out] int[] ipiv, [In, Out] double[] b, ref int ldb, [In, Out] double[] work, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZSYTRS2", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zsytrs2(ref char uplo, ref int n, ref int nrhs, [In, Out] Complex[] a, ref int lda, [In, Out] int[] ipiv, [In, Out] Complex[] b, ref int ldb, [In, Out] Complex[] work, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZHETRS2", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zhetrs2(ref char uplo, ref int n, ref int nrhs, [In, Out] Complex[] a, ref int lda, [In, Out] int[] ipiv, [In, Out] Complex[] b, ref int ldb, [In, Out] Complex[] work, out int info);

        [DllImport(sm_DllName, EntryPoint = "DSPTRS", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dsptrs(ref char uplo, ref int n, ref int nrhs, [In, Out] double[] ap, [In, Out] int[] ipiv, [In, Out] double[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZSPTRS", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zsptrs(ref char uplo, ref int n, ref int nrhs, [In, Out] Complex[] ap, [In, Out] int[] ipiv, [In, Out] Complex[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZHPTRS", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zhptrs(ref char uplo, ref int n, ref int nrhs, [In, Out] Complex[] ap, [In, Out] int[] ipiv, [In, Out] Complex[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "DTRTRS", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dtrtrs(ref char uplo, ref char trans, ref char diag, ref int n, ref int nrhs, [In, Out] double[] a, ref int lda, [In, Out] double[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZTRTRS", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _ztrtrs(ref char uplo, ref char trans, ref char diag, ref int n, ref int nrhs, [In, Out] Complex[] a, ref int lda, [In, Out] Complex[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "DTPTRS", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dtptrs(ref char uplo, ref char trans, ref char diag, ref int n, ref int nrhs, [In, Out] double[] ap, [In, Out] double[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZTPTRS", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _ztptrs(ref char uplo, ref char trans, ref char diag, ref int n, ref int nrhs, [In, Out] Complex[] ap, [In, Out] Complex[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "DTBTRS", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dtbtrs(ref char uplo, ref char trans, ref char diag, ref int n, ref int kd, ref int nrhs, [In, Out] double[] ab, ref int ldab, [In, Out] double[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZTBTRS", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _ztbtrs(ref char uplo, ref char trans, ref char diag, ref int n, ref int kd, ref int nrhs, [In, Out] Complex[] ab, ref int ldab, [In, Out] Complex[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "DGESV", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _driver_dgesv(ref int n, ref int nrhs, [In, Out] double[] a, ref int lda, [In, Out] int[] ipiv, [In, Out] double[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZGESV", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _driver_zgesv(ref int n, ref int nrhs, [In, Out] Complex[] a, ref int lda, [In, Out] int[] ipiv, [In, Out] Complex[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "DGBSV", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _driver_dgbsv(ref int n, ref int kl, ref int ku, ref int nrhs, [In, Out] double[] ab, ref int ldab, [In, Out] int[] ipiv, [In, Out] double[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZGBSV", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _driver_zgbsv(ref int n, ref int kl, ref int ku, ref int nrhs, [In, Out] Complex[] ab, ref int ldab, [In, Out] int[] ipiv, [In, Out] Complex[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "DGTSV", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _driver_dgtsv(ref int n, ref int nrhs, [In, Out] double[] dl, [In, Out] double[] d, [In, Out] double[] du, [In, Out] double[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZGTSV", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _driver_zgtsv(ref int n, ref int nrhs, [In, Out] Complex[] dl, [In, Out] Complex[] d, [In, Out] Complex[] du, [In, Out] Complex[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "DPOSV", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _driver_dposv(ref char uplo, ref int n, ref int nrhs, [In, Out] double[] a, ref int lda, [In, Out] double[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZPOSV", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _driver_zposv(ref char uplo, ref int n, ref int nrhs, [In, Out] Complex[] a, ref int lda, [In, Out] Complex[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "DPPSV", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _driver_dppsv(ref char uplo, ref int n, ref int nrhs, [In, Out] double[] ap, [In, Out] double[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZPPSV", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _driver_zppsv(ref char uplo, ref int n, ref int nrhs, [In, Out] Complex[] ap, [In, Out] Complex[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "DPBSV", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _driver_dpbsv(ref char uplo, ref int n, ref int kd, ref int nrhs, [In, Out] double[] ab, ref int ldab, [In, Out] double[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZPBSV", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _driver_zpbsv(ref char uplo, ref int n, ref int kd, ref int nrhs, [In, Out] Complex[] ab, ref int ldab, [In, Out] Complex[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "DPTSV", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _driver_dptsv(ref int n, ref int nrhs, [In, Out] double[] d, [In, Out] double[] e, [In, Out] double[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZPTSV", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _driver_zptsv(ref int n, ref int nrhs, [In, Out] double[] d, [In, Out] Complex[] e, [In, Out] Complex[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "DSYSV", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _driver_dsysv(ref char uplo, ref int n, ref int nrhs, [In, Out] double[] a, ref int lda, [In, Out] int[] ipiv, [In, Out] double[] b, ref int ldb, [In, Out] double[] work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DSYSV", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void _driver_dsysv(ref char uplo, ref int n, ref int nrhs, [In, Out] double[] a, ref int lda, [In, Out] int[] ipiv, [In, Out] double[] b, ref int ldb, double* work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZSYSV", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _driver_zsysv(ref char uplo, ref int n, ref int nrhs, [In, Out] Complex[] a, ref int lda, [In, Out] int[] ipiv, [In, Out] Complex[] b, ref int ldb, [In, Out] Complex[] work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZSYSV", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void _driver_zsysv(ref char uplo, ref int n, ref int nrhs, [In, Out] Complex[] a, ref int lda, [In, Out] int[] ipiv, [In, Out] Complex[] b, ref int ldb, Complex* work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZHESV", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _driver_zhesv(ref char uplo, ref int n, ref int nrhs, [In, Out] Complex[] a, ref int lda, [In, Out] int[] ipiv, [In, Out] Complex[] b, ref int ldb, [In, Out] Complex[] work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZHESV", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void _driver_zhesv(ref char uplo, ref int n, ref int nrhs, [In, Out] Complex[] a, ref int lda, [In, Out] int[] ipiv, [In, Out] Complex[] b, ref int ldb, Complex* work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DSPSV", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _driver_dspsv(ref char uplo, ref int n, ref int nrhs, [In, Out] double[] ap, [In, Out] int[] ipiv, [In, Out] double[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZSPSV", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _driver_zspsv(ref char uplo, ref int n, ref int nrhs, [In, Out] Complex[] ap, [In, Out] int[] ipiv, [In, Out] Complex[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZHPSV", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _driver_zhpsv(ref char uplo, ref int n, ref int nrhs, [In, Out] Complex[] ap, [In, Out] int[] ipiv, [In, Out] Complex[] b, ref int ldb, out int info);
        #endregion

        #region ISolver Members

        /// <summary>Solves a system of linear equations with a LU-factored square matrix, with multiple right-hand sides, i.e. op(A) * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A, i.e. the number of rows in B.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="a">The LU factorization of matrix A resulting from the call of <c>dgetrf</c> </param>
        /// <param name="ipiv">The argument corresponds to the output of the LU factorization, i.e. <c>dgetrf</c>.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="transposeState">A value indicating whether op(A) is matrix A or its transposed.</param>
        public void dgetrs(int n, double[] a, int[] ipiv, double[] b, int nrhs, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            int info;
            var trans = LAPACK.GetTrans(transposeState);

            _dgetrs(ref trans, ref n, ref nrhs, a, ref n, ipiv, b, ref n, out info);
            CheckForError(info, "dgetrs");
        }

        /// <summary>Solves a system of linear equations with a LU-factored square matrix, with multiple right-hand sides, i.e. op(A) * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A, i.e. the number of rows in B.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="a">The LU factorization of matrix A resulting from the call of <c>zgetrf</c> </param>
        /// <param name="ipiv">The argument corresponds to the output of the LU factorization, i.e. <c>zgetrf</c>.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="transposeState">A value indicating whether op(A) is matrix A, its transposed or its Hermitian.</param>
        public void zgetrs(int n, Complex[] a, int[] ipiv, Complex[] b, int nrhs, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            int info;
            var trans = LAPACK.GetTrans(transposeState);

            _zgetrs(ref trans, ref n, ref nrhs, a, ref n, ipiv, b, ref n, out info);
            CheckForError(info, "zgetrs");
        }

        /// <summary>Solves a system of linear equations with a LU-factored band matrix, with multiple right-hand sides, i.e. op(A) * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A, i.e. the number of rows in B.</param>
        /// <param name="kl">The number of subdiagonals within the band of A.</param>
        /// <param name="ku">The number of superdiagonals within the band of A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="ab">The matrix A in band storage.</param>
        /// <param name="ipiv">The argument corresponds to the output of the LU factorization, i.e. <c>dgbtrf</c>.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="transposeState">A value indicating whether op(A) is matrix A or its transposed.</param>
        public void dgbtrs(int n, int kl, int ku, double[] ab, int[] ipiv, double[] b, int nrhs, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            int info;
            int ldab = 2 * kl + ku + 1;
            var trans = LAPACK.GetTrans(transposeState);

            _dgbtrs(ref trans, ref n, ref kl, ref ku, ref nrhs, ab, ref ldab, ipiv, b, ref n, out info);
            CheckForError(info, "dgbtrs");
        }

        /// <summary>Solves a system of linear equations with a LU-factored band matrix, with multiple right-hand sides, i.e. op(A) * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A, i.e. the number of rows in B.</param>
        /// <param name="kl">The number of subdiagonals within the band of A.</param>
        /// <param name="ku">The number of superdiagonals within the band of A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="ab">The matrix A in band storage.</param>
        /// <param name="ipiv">The argument corresponds to the output of the LU factorization, i.e. <c>dgbtrf</c>.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="transposeState">A value indicating whether op(A) is matrix A, its transposed or its Hermitian.</param>
        public void zgbtrs(int n, int kl, int ku, Complex[] ab, int[] ipiv, Complex[] b, int nrhs, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            int info;
            int ldab = 2 * kl + ku + 1;
            var trans = LAPACK.GetTrans(transposeState);

            _zgbtrs(ref trans, ref n, ref kl, ref ku, ref nrhs, ab, ref ldab, ipiv, b, ref n, out info);
            CheckForError(info, "zgbtrs");
        }

        /// <summary>Solves a system of linear equations with a tridiagonal matrix using the LU factorization computed by <c>dgttrf</c>, i.e. op(A) * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A, i.e. the number of columns in B.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="dl">The multipliers that define the matrix L from the LU factorization of A.</param>
        /// <param name="d">The diagonal elements of the upper triangular matrix U from the LU factorization of A.</param>
        /// <param name="du">The <paramref name="n"/> - 1 elements of the first superdiagonal of U.</param>
        /// <param name="du2">The <paramref name="n"/> - 2 elements of the second superdiagonal of U.</param>
        /// <param name="ipiv">The <c>ipiv</c> array, as returned by <c>dgttrf</c>.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="transposeState">A value indicating whether op(A) is matrix A or its transposed.</param>
        public void dgttrs(int n, double[] dl, double[] d, double[] du, double[] du2, int[] ipiv, double[] b, int nrhs, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            int info;
            var trans = LAPACK.GetTrans(transposeState);

            _dgttrs(ref trans, ref n, ref nrhs, dl, d, du, du2, ipiv, b, ref n, out info);
            CheckForError(info, "dgttrs");
        }

        /// <summary>Solves a system of linear equations with a tridiagonal matrix using the LU factorization computed by <c>dgttrf</c>, i.e. op(A) * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A, i.e. the number of columns in B.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="dl">The multipliers that define the matrix L from the LU factorization of A.</param>
        /// <param name="d">The diagonal elements of the upper triangular matrix U from the LU factorization of A.</param>
        /// <param name="du">The <paramref name="n"/> - 1 elements of the first superdiagonal of U.</param>
        /// <param name="du2">The <paramref name="n"/> - 2 elements of the second superdiagonal of U.</param>
        /// <param name="ipiv">The <c>ipiv</c> array, as returned by <c>dgttrf</c>.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="transposeState">A value indicating whether op(A) is matrix A, its transposed or its Hermitian.</param>
        public void zgttrs(int n, Complex[] dl, Complex[] d, Complex[] du, Complex[] du2, int[] ipiv, Complex[] b, int nrhs, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            int info;
            var trans = LAPACK.GetTrans(transposeState);

            _zgttrs(ref trans, ref n, ref nrhs, dl, d, du, du2, ipiv, b, ref n, out info);
            CheckForError(info, "zgttrs");
        }

        /// <summary>Solves a system of linear equations with a Cholesky-factored symmetric (Hermitian) positive-definite matrix computed by <c>dpotrf</c>, i.e. A * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="a">Contains the factor U or L with respect to <paramref name="triangularMatrixType"/>.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
        public void dpotrs(int n, double[] a, double[] b, int nrhs, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _dpotrs(ref uplo, ref n, ref nrhs, a, ref n, b, ref n, out info);
            CheckForError(info, "dpotrs");
        }

        /// <summary>Solves a system of linear equations with a Cholesky-factored symmetric (Hermitian) positive-definite matrix computed by <c>zpotrf</c>, i.e. A * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="a">Contains the factor U or L with respect to <paramref name="triangularMatrixType"/>.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
        public void zpotrs(int n, Complex[] a, Complex[] b, int nrhs, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _zpotrs(ref uplo, ref n, ref nrhs, a, ref n, b, ref n, out info);
            CheckForError(info, "zpotrs");
        }

        /// <summary>Solves a system of linear equations with a Cholesky-factored symmetric (Hermitian) positive-definite matrix using the Rectangular Full Packed (RFP) format computed by <c>dpftrf</c>, i.e. A * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="a">Contains the upper or lower triangular part of matrix op(A) with respect to <paramref name="triangularMatrixType"/> in the RFP format.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
        /// <param name="transposeState">A value indicating whether matrix A or A' is stored  in RFP format.</param>
        public void dpftrs(int n, double[] a, double[] b, int nrhs, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            int info;
            var trans = LAPACK.GetTrans(transposeState);
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _dpftrs(ref trans, ref uplo, ref n, ref nrhs, a, b, ref n, out info);
            CheckForError(info, "dpftrs");
        }

        /// <summary>Solves a system of linear equations with a Cholesky-factored symmetric (Hermitian) positive-definite matrix using the Rectangular Full Packed (RFP) format computed by <c>zpftrf</c>, i.e. A * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="a">Contains the upper or lower triangular part of matrix op(A) with respect to <paramref name="triangularMatrixType"/> in the RFP format.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
        /// <param name="transposeState">A value indicating whether matrix A, A' or A^H is stored  in RFP format.</param>
        public void zpftrs(int n, Complex[] a, Complex[] b, int nrhs, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            int info;
            var trans = LAPACK.GetTrans(transposeState);
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _zpftrs(ref trans, ref uplo, ref n, ref nrhs, a, b, ref n, out info);
            CheckForError(info, "zpftrs");
        }

        /// <summary>Solves a system of linear equations with a packed Cholesky-factored symmetric (Hermitian) positive-definite matrix computed by <c>dpptrf</c>, i.e. A * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="ap">Contains the factor U or L with respect to <paramref name="triangularMatrixType"/> in packed storage.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
        public void dpptrs(int n, double[] ap, double[] b, int nrhs, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _dpptrs(ref uplo, ref n, ref nrhs, ap, b, ref n, out info);
            CheckForError(info, "dpptrs");
        }

        /// <summary>Solves a system of linear equations with a packed Cholesky-factored symmetric (Hermitian) positive-definite matrix computed by <c>zpptrf</c>, i.e. A * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="ap">Contains the factor U or L with respect to <paramref name="triangularMatrixType"/> in packed storage.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
        public void zpptrs(int n, Complex[] ap, Complex[] b, int nrhs, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _zpptrs(ref uplo, ref n, ref nrhs, ap, b, ref n, out info);
            CheckForError(info, "zpptrs");
        }

        /// <summary>Solves a system of linear equations with a Cholesky-factored symmetric (Hermitian) positive-definite band matrix computed by <c>dpbtrf</c>, i.e. A * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="kd">The number of superdiagonals or subdiagonals in the matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="ab">The Cholesky factor as returned by the factorization routine, in band storage form.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
        public void dpbtrs(int n, int kd, double[] ab, double[] b, int nrhs, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            int ldab = kd + 1;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _dpbtrs(ref uplo, ref n, ref kd, ref nrhs, ab, ref ldab, b, ref n, out info);
            CheckForError(info, "dpbtrs");
        }

        /// <summary>Solves a system of linear equations with a Cholesky-factored symmetric (Hermitian) positive-definite band matrix computed by <c>zpbtrf</c>, i.e. A * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="kd">The number of superdiagonals or subdiagonals in the matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="ab">The Cholesky factor as returned by the factorization routine, in band storage form.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
        public void zpbtrs(int n, int kd, Complex[] ab, Complex[] b, int nrhs, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            int ldab = kd + 1;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _zpbtrs(ref uplo, ref n, ref kd, ref nrhs, ab, ref ldab, b, ref n, out info);
            CheckForError(info, "zpbtrs");
        }

        /// <summary>Solves a system of linear equations with a symmetric (Hermitian) positive-definite tridiagonal matrix using the factorization computed by <c>dpttrf</c>, i.e. A * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="d">The diagonal elements of the diagonal matrix D from the factorization computed by <c>dpttrf</c>.</param>
        /// <param name="e">The <paramref name="n"/> - 1 off-diagonal elements of the unit bidiagonal factor U or L from the factorization computed by <c>dpttrf</c>.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        public void dpttrs(int n, double[] d, double[] e, double[] b, int nrhs)
        {
            int info;

            _dpttrs(ref n, ref nrhs, d, e, b, ref n, out info);
            CheckForError(info, "dpttrs");
        }

        /// <summary>Solves a system of linear equations with a symmetric (Hermitian) positive-definite tridiagonal matrix using the factorization computed by <c>zpttrf</c>, i.e. A * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="d">The diagonal elements of the diagonal matrix D from the factorization computed by <c>zpttrf</c>.</param>
        /// <param name="e">The <paramref name="n"/> - 1 off-diagonal elements of the unit bidiagonal factor U or L from the factorization computed by <c>zpttrf</c>.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
        public void zpttrs(int n, double[] d, Complex[] e, Complex[] b, int nrhs, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _zpttrs(ref uplo, ref n, ref nrhs, d, e, b, ref n, out info);
            CheckForError(info, "zpttrs");
        }

        /// <summary>Solves a system of linear equations with a UDU- or LDL-factored symmetric matrix, computed by <c>dsytrf</c>, i.e. A * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="a">Contains the upper or lower triangular part of matrix A with respect to <paramref name="triangularMatrixType"/>.</param>
        /// <param name="ipiv">The ipiv array, as returned by <c>dsytrf</c>.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
        public void dsytrs(int n, double[] a, int[] ipiv, double[] b, int nrhs, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _dsytrs(ref uplo, ref n, ref nrhs, a, ref n, ipiv, b, ref n, out info);
            CheckForError(info, "dsytrs");
        }

        /// <summary>Solves a system of linear equations with a UDU- or LDL-factored symmetric matrix, computed by <c>zsytrf</c>, i.e. A * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="a">Contains the upper or lower triangular part of matrix A with respect to <paramref name="triangularMatrixType"/>.</param>
        /// <param name="ipiv">The ipiv array, as returned by <c>zsytrf</c>.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
        public void zsytrs(int n, Complex[] a, int[] ipiv, Complex[] b, int nrhs, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _zsytrs(ref uplo, ref n, ref nrhs, a, ref n, ipiv, b, ref n, out info);
            CheckForError(info, "zsytrs");
        }

        /// <summary>Solves a system of linear equations with a UDU- or LDL-factored Hermitian matrix, as computed by <c>zhetrf</c>, i.e. A * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="a">Contains the upper or lower triangular part of matrix A with respect to <paramref name="triangularMatrixType"/>.</param>
        /// <param name="ipiv">The ipiv array, as returned by <c>zhetrf</c>.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
        public void zhetrs(int n, Complex[] a, int[] ipiv, Complex[] b, int nrhs, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _zhetrs(ref uplo, ref n, ref nrhs, a, ref n, ipiv, b, ref n, out info);
            CheckForError(info, "zhetrs");
        }

        /// <summary>Solves a system of linear equations with a UDU- or LDL-factored symmetric matrix computed by <c>dsytrf</c> and converted by <c>dsyconv</c>, i.e. A * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="a">Contains the block diagonal matrix D and the multiplies used to obain the factor U or L as compured by <c>dsytrf</c> with respect to <paramref name="triangularMatrixType"/>.</param>
        /// <param name="ipiv">The ipiv array, as returned by <c>dsytrf</c>.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="work">A workspace array of dimension at least <paramref name="n"/>.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
        public void dsytrs2(int n, double[] a, int[] ipiv, double[] b, double[] work, int nrhs, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _dsytrs2(ref uplo, ref n, ref nrhs, a, ref n, ipiv, b, ref n, work, out info);
            CheckForError(info, "dsytrs2");
        }

        /// <summary>Solves a system of linear equations with a UDU- or LDL-factored symmetric matrix computed by <c>zsytrf</c> and converted by <c>zsyconv</c>, i.e. A * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="a">Contains the block diagonal matrix D and the multiplies used to obain the factor U or L as compured by <c>zsytrf</c> with respect to <paramref name="triangularMatrixType"/>.</param>
        /// <param name="ipiv">The ipiv array, as returned by <c>zsytrf</c>.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="work">A workspace array of dimension at least <paramref name="n"/>.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
        public void zsytrs2(int n, Complex[] a, int[] ipiv, Complex[] b, Complex[] work, int nrhs, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _zsytrs2(ref uplo, ref n, ref nrhs, a, ref n, ipiv, b, ref n, work, out info);
            CheckForError(info, "zsytrs2");
        }

        /// <summary>Solves a system of linear equations with a UDU- or LDL-factored Hermitian matrix computed by <c>zhetrf</c> and converted by <c>zsyconv</c>, i.e. A * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="a">Contains the block diagonal matrix D and the multiplies used to obain the factor U or L as compured by <c>zhetrf</c> with respect to <paramref name="triangularMatrixType"/>.</param>
        /// <param name="ipiv">The ipiv array, as returned by <c>zhetrf</c>.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="work">A workspace array of dimension at least <paramref name="n"/>.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
        public void zhetrs2(int n, Complex[] a, int[] ipiv, Complex[] b, Complex[] work, int nrhs, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _zhetrs2(ref uplo, ref n, ref nrhs, a, ref n, ipiv, b, ref n, work, out info);
            CheckForError(info, "zhetrs2");
        }

        /// <summary>Solves a system of linear equations with a UDU- or LDL-factored symmetric matrix using packed storage, as computed by <c>dsptrf</c>, i.e. A * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="ap">Contains the factor U or L, as specified by <paramref name="triangularMatrixType"/>, in packed storage.</param>
        /// <param name="ipiv">The ipiv array, as returned by <c>dsptrf</c>.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
        public void dsptrs(int n, double[] ap, int[] ipiv, double[] b, int nrhs, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _dsptrs(ref uplo, ref n, ref nrhs, ap, ipiv, b, ref n, out info);
            CheckForError(info, "dsptrs");
        }

        /// <summary>Solves a system of linear equations with a UDU- or LDL-factored symmetric matrix using packed storage, as computed by <c>zsptrf</c>, i.e. A * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="ap">Contains the factor U or L, as specified by <paramref name="triangularMatrixType"/>, in packed storage.</param>
        /// <param name="ipiv">The ipiv array, as returned by <c>zsptrf</c>.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
        public void zsptrs(int n, Complex[] ap, int[] ipiv, Complex[] b, int nrhs, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _zsptrs(ref uplo, ref n, ref nrhs, ap, ipiv, b, ref n, out info);
            CheckForError(info, "zsptrs");
        }

        /// <summary>Solves a system of linear equations with a UDU- or LDL-factored Hermitian matrix using packed storage, computed by <c>dhptrf</c>, i.e. A * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="ap">Contains the factor U or L, as specified by <paramref name="triangularMatrixType"/>, in packed storage.</param>
        /// <param name="ipiv">The ipiv array, as returned by <c>dhptrf</c>.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>
        public void zhptrs(int n, Complex[] ap, int[] ipiv, Complex[] b, int nrhs, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _zhptrs(ref uplo, ref n, ref nrhs, ap, ipiv, b, ref n, out info);
            CheckForError(info, "zhptrs");
        }

        /// <summary>Solves a system of linear equations with a triangular matrix, with multiple right-hand sides, i.e. op(A) * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="a">Contains the matrix A.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="isUnitTriangularMatrix">A value indicating whether matrix A is unit triangular, i.e. diagonal elements of A are assumed to be 1 and not referenced in the array <paramref name="a"/>.</param>
        /// <param name="triangularMatrixType">A value indicating whether matrix A is upper or lower triangular.</param>
        /// <param name="transposeState">A value indicating whether op(A) is matrix A or its transposed.</param>
        public void dtrtrs(int n, double[] a, double[] b, int nrhs, bool isUnitTriangularMatrix = true, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            int info;
            var trans = LAPACK.GetTrans(transposeState);
            var uplo = LAPACK.GetUplo(triangularMatrixType);
            var diag = GetDiag(isUnitTriangularMatrix);

            _dtrtrs(ref uplo, ref trans, ref diag, ref n, ref nrhs, a, ref n, b, ref n, out info);
            CheckForError(info, "dtrtrs");
        }

        /// <summary>Solves a system of linear equations with a triangular matrix, with multiple right-hand sides, i.e. op(A) * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="a">Contains the matrix A.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="isUnitTriangularMatrix">A value indicating whether matrix A is unit triangular, i.e. diagonal elements of A are assumed to be 1 and not referenced in the array <paramref name="a"/>.</param>
        /// <param name="triangularMatrixType">A value indicating whether matrix A is upper or lower triangular.</param>
        /// <param name="transposeState">A value indicating whether op(A) is matrix A, its transposed or its Hermitian.</param>
        public void ztrtrs(int n, Complex[] a, Complex[] b, int nrhs, bool isUnitTriangularMatrix = true, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            int info;
            var trans = LAPACK.GetTrans(transposeState);
            var uplo = LAPACK.GetUplo(triangularMatrixType);
            var diag = GetDiag(isUnitTriangularMatrix);

            _ztrtrs(ref uplo, ref trans, ref diag, ref n, ref nrhs, a, ref n, b, ref n, out info);
            CheckForError(info, "ztrtrs");
        }

        /// <summary>Solves a system of linear equations with a packed triangular matrix, with multiple right-hand sides, i.e. op(A) * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="ap">Contains the matrix A in packed storage.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="isUnitTriangularMatrix">A value indicating whether matrix A is unit triangular, i.e. diagonal elements of A are assumed to be 1 and not referenced in the array <paramref name="ap"/>.</param>
        /// <param name="triangularMatrixType">A value indicating whether matrix A is upper or lower triangular.</param>
        /// <param name="transposeState">A value indicating whether op(A) is matrix A or its transposed.</param>
        public void dtptrs(int n, double[] ap, double[] b, int nrhs, bool isUnitTriangularMatrix = true, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            int info;
            var trans = LAPACK.GetTrans(transposeState);
            var uplo = LAPACK.GetUplo(triangularMatrixType);
            var diag = GetDiag(isUnitTriangularMatrix);

            _dtptrs(ref uplo, ref trans, ref diag, ref n, ref nrhs, ap, b, ref n, out info);
            CheckForError(info, "dtptrs");
        }

        /// <summary>Solves a system of linear equations with a packed triangular matrix, with multiple right-hand sides, i.e. op(A) * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="ap">Contains the matrix A in packed storage.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="isUnitTriangularMatrix">A value indicating whether matrix A is unit triangular, i.e. diagonal elements of A are assumed to be 1 and not referenced in the array <paramref name="ap"/>.</param>
        /// <param name="triangularMatrixType">A value indicating whether matrix A is upper or lower triangular.</param>
        /// <param name="transposeState">A value indicating whether op(A) is matrix A, its transposed or its Hermitian.</param>
        public void ztptrs(int n, Complex[] ap, Complex[] b, int nrhs, bool isUnitTriangularMatrix = true, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            int info;
            var trans = LAPACK.GetTrans(transposeState);
            var uplo = LAPACK.GetUplo(triangularMatrixType);
            var diag = GetDiag(isUnitTriangularMatrix);

            _ztptrs(ref uplo, ref trans, ref diag, ref n, ref nrhs, ap, b, ref n, out info);
            CheckForError(info, "ztptrs");
        }

        /// <summary>Solves a system of linear equations with a band triangular matrix, with multiple right-hand sides, i.e. op(A) * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="kd">The number of superdiagonals or subdiagonals in the matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="ab">The matrix A in band storage form.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="isUnitTriangularMatrix">A value indicating whether matrix A is unit triangular, i.e. diagonal elements of A are assumed to be 1 and not referenced in the array <paramref name="ab"/>.</param>
        /// <param name="triangularMatrixType">A value indicating whether matrix A is upper or lower triangular.</param>
        /// <param name="transposeState">A value indicating whether op(A) is matrix A or its transposed.</param>
        public void dtbtrs(int n, int kd, double[] ab, double[] b, int nrhs, bool isUnitTriangularMatrix = true, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            int info;
            int ldab = kd + 1;
            var trans = LAPACK.GetTrans(transposeState);
            var uplo = LAPACK.GetUplo(triangularMatrixType);
            var diag = GetDiag(isUnitTriangularMatrix);

            _dtbtrs(ref uplo, ref trans, ref diag, ref n, ref kd, ref nrhs, ab, ref ldab, b, ref n, out info);
            CheckForError(info, "dtbtrs");
        }

        /// <summary>Solves a system of linear equations with a band triangular matrix, with multiple right-hand sides, i.e. op(A) * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="kd">The number of superdiagonals or subdiagonals in the matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="ab">The matrix A in band storage form.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="isUnitTriangularMatrix">A value indicating whether matrix A is unit triangular, i.e. diagonal elements of A are assumed to be 1 and not referenced in the array <paramref name="ab"/>.</param>
        /// <param name="triangularMatrixType">A value indicating whether matrix A is upper or lower triangular.</param>
        /// <param name="transposeState">A value indicating whether op(A) is matrix A, its transposed or its Hermitian.</param>
        public void ztbtrs(int n, int kd, Complex[] ab, Complex[] b, int nrhs, bool isUnitTriangularMatrix = true, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            int info;
            int ldab = kd + 1;
            var trans = LAPACK.GetTrans(transposeState);
            var uplo = LAPACK.GetUplo(triangularMatrixType);
            var diag = GetDiag(isUnitTriangularMatrix);

            _ztbtrs(ref uplo, ref trans, ref diag, ref n, ref kd, ref nrhs, ab, ref ldab, b, ref n, out info);
            CheckForError(info, "ztbtrs");
        }

        /// <summary>Computes the solution to the system of linear equations with a square matrix A and multiple right-hand sides, i.e. A * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="a">The matrix A provided column-by-column; overwritten by the factors L and U from the factorization of A = P * L * U, the unit diagonal elements of L are not stored.</param>
        /// <param name="ipiv">Array of dimension at least <paramref name="n"/>, where the pivot indices define the permutation matrix P.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        public void driver_dgesv(int n, double[] a, int[] ipiv, double[] b, int nrhs)
        {
            int info;
            _driver_dgesv(ref n, ref nrhs, a, ref n, ipiv, b, ref n, out info);
            CheckForError(info, "driver_dgesv");
        }

        /// <summary>Computes the solution to the system of linear equations with a square matrix A and multiple right-hand sides, i.e. A * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="a">The matrix A provided column-by-column; overwritten by the factors L and U from the factorization of A = P * L * U, the unit diagonal elements of L are not stored.</param>
        /// <param name="ipiv">Array of dimension at least <paramref name="n"/>, where the pivot indices define the permutation matrix P.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        public void driver_zgesv(int n, Complex[] a, int[] ipiv, Complex[] b, int nrhs)
        {
            int info;
            _driver_zgesv(ref n, ref nrhs, a, ref n, ipiv, b, ref n, out info);
            CheckForError(info, "driver_zgesv");
        }

        /// <summary>Computes the solution to the system of linear equations with a band matrix A and multiple right-hand sides, i.e. A * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="kl">The number of subdiagonals within the band of A.</param>
        /// <param name="ku">The number of superdiagonals within the band of A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="ab">The matrix A in band storage; overwritten by L and U.</param>
        /// <param name="ipiv">Array of dimension at least <paramref name="n"/>, where the pivot indices define the permutation matrix P.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        public void driver_dgbsv(int n, int kl, int ku, double[] ab, int[] ipiv, double[] b, int nrhs)
        {
            int info;
            int ldab = 2 * kl + ku + 1;

            _driver_dgbsv(ref n, ref kl, ref ku, ref nrhs, ab, ref ldab, ipiv, b, ref n, out info);
            CheckForError(info, "driver_dgbsv");
        }

        /// <summary>Computes the solution to the system of linear equations with a band matrix A and multiple right-hand sides, i.e. A * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="kl">The number of subdiagonals within the band of A.</param>
        /// <param name="ku">The number of superdiagonals within the band of A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="ab">The matrix A in band storage; overwritten by L and U.</param>
        /// <param name="ipiv">Array of dimension at least <paramref name="n"/>, where the pivot indices define the permutation matrix P.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        public void driver_zgbsv(int n, int kl, int ku, Complex[] ab, int[] ipiv, Complex[] b, int nrhs)
        {
            int info;
            int ldab = 2 * kl + ku + 1;

            _driver_zgbsv(ref n, ref kl, ref ku, ref nrhs, ab, ref ldab, ipiv, b, ref n, out info);
            CheckForError(info, "driver_zgbsv");
        }

        /// <summary>Computes the solution to the system of linear equations with a tridiagonal matrix A and multiple right-hand sides, i.e. A * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="dl">The <paramref name="n"/> - 1 subdiagonal elements of A; overwritten by the <paramref name="n"/> - 2 elements of the second superdiagonal of the upper triangular matrix U.</param>
        /// <param name="d">The <paramref name="n"/> diagonal elements of A; overwritten by the <paramref name="n"/> diagonal elements of U.</param>
        /// <param name="du">The <paramref name="n"/> - 1 superdiagonal elements of A; overwritten by the <paramref name="n"/> - 1 elements of the first superdiagonal of U.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        public void driver_dgtsv(int n, double[] dl, double[] d, double[] du, double[] b, int nrhs)
        {
            int info;

            _driver_dgtsv(ref n, ref nrhs, dl, d, du, b, ref n, out info);
            CheckForError(info, "driver_dgtsv");
        }

        /// <summary>Computes the solution to the system of linear equations with a tridiagonal matrix A and multiple right-hand sides, i.e. A * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="dl">The <paramref name="n"/> - 1 subdiagonal elements of A; overwritten by the <paramref name="n"/> - 2 elements of the second superdiagonal of the upper triangular matrix U.</param>
        /// <param name="d">The <paramref name="n"/> diagonal elements of A; overwritten by the <paramref name="n"/> diagonal elements of U.</param>
        /// <param name="du">The <paramref name="n"/> - 1 superdiagonal elements of A; overwritten by the <paramref name="n"/> - 1 elements of the first superdiagonal of U.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        public void driver_zgtsv(int n, Complex[] dl, Complex[] d, Complex[] du, Complex[] b, int nrhs)
        {
            int info;

            _driver_zgtsv(ref n, ref nrhs, dl, d, du, b, ref n, out info);
            CheckForError(info, "driver_zgtsv");
        }

        /// <summary>Computes the solution to the system of linear equations with a symmetric positive-definite matrix A and multiple right-hand sides, i.e. A * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="a">Contains the factor U or L, as specified by <paramref name="triangularMatrixType"/>.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>        
        public void driver_dposv(int n, double[] a, double[] b, int nrhs, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _driver_dposv(ref uplo, ref n, ref nrhs, a, ref n, b, ref n, out info);
            CheckForError(info, "driver_dposv");
        }

        /// <summary>Computes the solution to the system of linear equations with a Hermitian positive-definite matrix A and multiple right-hand sides, i.e. A * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="a">Contains the factor U or L, as specified by <paramref name="triangularMatrixType"/>.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>        
        public void driver_zposv(int n, Complex[] a, Complex[] b, int nrhs, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _driver_zposv(ref uplo, ref n, ref nrhs, a, ref n, b, ref n, out info);
            CheckForError(info, "driver_zposv");
        }

        /// <summary>Computes the solution to the system of linear equations with a symmetric positive definite packed matrix A and multiple right-hand sides, i.e. A * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="ap">Contains the factor U or L, as specified by <paramref name="triangularMatrixType"/> in packed storage.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>        
        public void driver_dppsv(int n, double[] ap, double[] b, int nrhs, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _driver_dppsv(ref uplo, ref n, ref nrhs, ap, b, ref n, out info);
            CheckForError(info, "driver_dppsv");
        }

        /// <summary>Computes the solution to the system of linear equations with a Hermitian positive definite packed matrix A and multiple right-hand sides, i.e. A * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="ap">Contains the factor U or L, as specified by <paramref name="triangularMatrixType"/> in packed storage.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>        
        public void driver_zppsv(int n, Complex[] ap, Complex[] b, int nrhs, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _driver_zppsv(ref uplo, ref n, ref nrhs, ap, b, ref n, out info);
            CheckForError(info, "driver_zppsv");
        }

        /// <summary>Computes the solution to the system of linear equations with a symmetric positive-definite band matrix A and multiple right-hand sides, i.e. A * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="kd">The number of superdiagonals or subdiagonals as specified by <paramref name="triangularMatrixType"/>.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="ab">Contains the upper or lower triangular part of matrix A, as specified by <paramref name="triangularMatrixType"/> in packed storage in band storage.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>        
        public void driver_dpbsv(int n, int kd, double[] ab, double[] b, int nrhs, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            int ldab = kd + 1;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _driver_dpbsv(ref uplo, ref n, ref kd, ref nrhs, ab, ref ldab, b, ref n, out info);
            CheckForError(info, "driver_dpbsv");
        }

        /// <summary>Computes the solution to the system of linear equations with a Hermitian positive-definite band matrix A and multiple right-hand sides, i.e. A * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="kd">The number of superdiagonals or subdiagonals as specified by <paramref name="triangularMatrixType"/>.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="ab">Contains the upper or lower triangular part of matrix A, as specified by <paramref name="triangularMatrixType"/> in packed storage in band storage.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>        
        public void driver_zpbsv(int n, int kd, Complex[] ab, Complex[] b, int nrhs, BLAS.TriangularMatrixType triangularMatrixType)
        {
            int info;
            int ldab = kd + 1;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _driver_zpbsv(ref uplo, ref n, ref kd, ref nrhs, ab, ref ldab, b, ref n, out info);
            CheckForError(info, "driver_zpbsv");
        }

        /// <summary>Computes the solution to the system of linear equations with a symmetric positive definite tridiagonal matrix A and multiple right-hand sides, i.e. A * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="d">Contains the <paramref name="n"/> diagonal elements of the tridiagonal matrix A; overwritten by the <paramref name="n"/> diagonal elements of the diagonal matrix D from L * D * L' factorization of A.</param>
        /// <param name="e">Contains the <paramref name="n"/> - 1 subdiagonal elements of matrix A; overwritten by the <paramref name="n"/> - 1 subdiagonal elements of the unit bidiagonal factor L from the factorization of A.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        public void driver_dptsv(int n, double[] d, double[] e, double[] b, int nrhs)
        {
            int info;

            _driver_dptsv(ref n, ref nrhs, d, e, b, ref n, out info);
            CheckForError(info, "driver_dptsv");
        }

        /// <summary>Computes the solution to the system of linear equations with a Hermitian positive definite tridiagonal matrix A and multiple right-hand sides, i.e. A * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="d">Contains the <paramref name="n"/> diagonal elements of the tridiagonal matrix A; overwritten by the <paramref name="n"/> diagonal elements of the diagonal matrix D from L * D * L' factorization of A.</param>
        /// <param name="e">Contains the <paramref name="n"/> - 1 subdiagonal elements of matrix A; overwritten by the <paramref name="n"/> - 1 subdiagonal elements of the unit bidiagonal factor L from the factorization of A.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        public void driver_zptsv(int n, double[] d, Complex[] e, Complex[] b, int nrhs)
        {
            int info;

            _driver_zptsv(ref n, ref nrhs, d, e, b, ref n, out info);
            CheckForError(info, "driver_zptsv");
        }

        /// <summary>Gets a optimal workspace array length for the <c>driver_dsysv</c> function.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>        
        /// <returns>The optimal workspace array length.</returns>
        /// <remarks>The parameter <paramref name="triangularMatrixType"/> should not have an impact of the calculation of the optimal length of the workspace array.</remarks>
        public int driver_dsysvQuery(int n, int nrhs, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            var lwork = -1;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            unsafe
            {
                double* work = stackalloc double[1];

                int info;
                _driver_dsysv(ref uplo, ref n, ref nrhs, null, ref n, null, null, ref n, work, ref lwork, out info);
                CheckForError(info, "_driver_dsysv");

                return ((int)work[0]) + 1;
            }
        }

        /// <summary>Computes the solution to the system of linear equations with a real symmetric matrix A and multiple right-hand sides, i.e. A * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="a">Contains the upper or lower triangular part of matrix A, as specified by <paramref name="triangularMatrixType"/>; overwritten by the block-diagonal matrix D and the multipliers used to obtain the factor U (or L) from the factorization of A as computed by <c>dsytrf</c>.</param>
        /// <param name="ipiv">A array with dimension at least <paramref name="n"/>; contains details of the interchanges and the block structure of D, as determined by <c>dsytrf</c> (output). </param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="work">A workspace array.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>        
        public void driver_dsysv(int n, double[] a, int[] ipiv, double[] b, double[] work, int nrhs, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            int lwork = work.Length;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _driver_dsysv(ref uplo, ref n, ref nrhs, a, ref n, ipiv, b, ref n, work, ref lwork, out info);
            CheckForError(info, "driver_dsysv");
        }

        /// <summary>Gets a optimal workspace array length for the <c>driver_zsysv</c> function.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>        
        /// <returns>The optimal workspace array length.</returns>
        /// <remarks>The parameter <paramref name="triangularMatrixType"/> should not have an impact of the calculation of the optimal length of the workspace array.</remarks>
        public int driver_zsysvQuery(int n, int nrhs, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            var lwork = -1;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            unsafe
            {
                Complex* work = stackalloc Complex[1];

                int info;
                _driver_zsysv(ref uplo, ref n, ref nrhs, null, ref n, null, null, ref n, work, ref lwork, out info);
                CheckForError(info, "driver_zsysv");

                return ((int)work[0].Real) + 1;
            }
        }

        /// <summary>Computes the solution to the system of linear equations with a complex symmetric matrix A and multiple right-hand sides, i.e. A * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="a">Contains the upper or lower triangular part of matrix A, as specified by <paramref name="triangularMatrixType"/>; overwritten by the block-diagonal matrix D and the multipliers used to obtain the factor U (or L) from the factorization of A as computed by <c>zsytrf</c>.</param>
        /// <param name="ipiv">A array with dimension at least <paramref name="n"/>; contains details of the interchanges and the block structure of D, as determined by <c>zsytrf</c> (output). </param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="work">A workspace array.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>        
        public void driver_zsysv(int n, Complex[] a, int[] ipiv, Complex[] b, Complex[] work, int nrhs, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            int lwork = work.Length;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _driver_zsysv(ref uplo, ref n, ref nrhs, a, ref n, ipiv, b, ref n, work, ref lwork, out info);
            CheckForError(info, "driver_zsysv");
        }

        /// <summary>Gets a optimal workspace array length for the <c>driver_zhesv</c> function.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>        
        /// <returns>The optimal workspace array length.</returns>
        /// <remarks>The parameter <paramref name="triangularMatrixType"/> should not have an impact of the calculation of the optimal length of the workspace array.</remarks>
        public int driver_zhesvQuery(int n, int nrhs, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            var lwork = -1;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            unsafe
            {
                Complex* work = stackalloc Complex[1];

                int info;
                _driver_zhesv(ref uplo, ref n, ref nrhs, null, ref n, null, null, ref n, work, ref lwork, out info);
                CheckForError(info, "driver_zhesv");

                return ((int)work[0].Real) + 1;
            }
        }

        /// <summary>Computes the solution to the system of linear equations with a Hermitian matrix A and multiple right-hand sides, i.e. A * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="a">Contains the upper or lower triangular part of matrix A, as specified by <paramref name="triangularMatrixType"/>; overwritten by the block-diagonal matrix D and the multipliers used to obtain the factor U (or L) from the factorization of A as computed by <c>zhetrf</c>.</param>
        /// <param name="ipiv">A array with dimension at least <paramref name="n"/>; contains details of the interchanges and the block structure of D, as determined by <c>zhetrf</c> (output).</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="work">A workspace array.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>        
        public void driver_zhesv(int n, Complex[] a, int[] ipiv, Complex[] b, Complex[] work, int nrhs, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            int lwork = work.Length;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _driver_zhesv(ref uplo, ref n, ref nrhs, a, ref n, ipiv, b, ref n, work, ref lwork, out info);
            CheckForError(info, "driver_zhesv");
        }

        /// <summary>Computes the solution to the system of linear equations with a real symmetric matrix A stored in packed format, and multiple right-hand sides, i.e. A * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="ap">Contains the upper or lower triangular part of matrix A, as specified by <paramref name="triangularMatrixType"/> in packed format; overwritten by the block-diagonal matrix D and the multipliers used to obtain the factor U (or L) from the factorization of A as computed by <c>dsptrf</c>.</param>
        /// <param name="ipiv">A array with dimension at least <paramref name="n"/>; contains details of the interchanges and the block structure of D, as determined by <c>dsptrf</c> (output).</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>        
        public void driver_dspsv(int n, double[] ap, int[] ipiv, double[] b, int nrhs, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _driver_dspsv(ref uplo, ref n, ref nrhs, ap, ipiv, b, ref n, out info);
            CheckForError(info, "driver_dspsv");
        }

        /// <summary>Computes the solution to the system of linear equations with a complex symmetric matrix A stored in packed format, and multiple right-hand sides, i.e. A * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="ap">Contains the upper or lower triangular part of matrix A, as specified by <paramref name="triangularMatrixType"/> in packed format; overwritten by the block-diagonal matrix D and the multipliers used to obtain the factor U (or L) from the factorization of A as computed by <c>zsptrf</c>.</param>
        /// <param name="ipiv">A array with dimension at least <paramref name="n"/>; contains details of the interchanges and the block structure of D, as determined by <c>zsptrf</c> (output).</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>        
        public void driver_zspsv(int n, Complex[] ap, int[] ipiv, Complex[] b, int nrhs, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _driver_zspsv(ref uplo, ref n, ref nrhs, ap, ipiv, b, ref n, out info);
            CheckForError(info, "driver_zspsv");
        }

        /// <summary>Computes the solution to the system of linear equations with a Hermitian matrix A stored in packed format, and multiple right-hand sides, i.e. A * X = B.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="ap">Contains the upper or lower triangular part of matrix A, as specified by <paramref name="triangularMatrixType"/> in packed format; overwritten by the block-diagonal matrix D and the multipliers used to obtain the factor U (or L) from the factorization of A as computed by <c>zhptrf</c>.</param>
        /// <param name="ipiv">A array with dimension at least <paramref name="n"/>; contains details of the interchanges and the block structure of D, as determined by <c>zhptrf</c> (output).</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of matrix A is stored and how matrix A is factored.</param>        
        public void driver_zhpsv(int n, Complex[] ap, int[] ipiv, Complex[] b, int nrhs, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _driver_zhpsv(ref uplo, ref n, ref nrhs, ap, ipiv, b, ref n, out info);
            CheckForError(info, "driver_zhpsv");
        }
        #endregion

        #region private methods

        /// <summary>Gets the <see cref="System.Char"/> representation of a value indicating whether a specified matrix is a unit triangular matrix.
        /// </summary>
        /// <param name="isUnitTriangularMatrix"> A value indicating whether the specified matrix is a unit triangular matrix.</param>
        /// <returns>The <see cref="System.Char"/> representation of <paramref name="isUnitTriangularMatrix"/>.</returns>
        private char GetDiag(bool isUnitTriangularMatrix)
        {
            return (isUnitTriangularMatrix == true) ? 'U' : 'N';
        }
        #endregion
    }
}