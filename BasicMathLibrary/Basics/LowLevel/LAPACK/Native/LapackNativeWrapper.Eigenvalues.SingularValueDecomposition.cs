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
using System;
using System.Security;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Dodoni.MathLibrary.Basics.LowLevel.Native
{
    internal partial class LapackNativeWrapper : LapackEigenvalues.ISingularValueDecomposition
    {
        #region private function import

        [DllImport(sm_DllName, EntryPoint = "DGEBRD", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _dgebrd(ref int m, ref int n, [In, Out] double[] a, ref int lda, [In, Out] double[] d, [In, Out] double[] e, [In, Out] double[] tauq, [In, Out] double[] taup, [In, Out] double[] work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DGEBRD", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static unsafe extern void _dgebrd(ref int m, ref int n, [In, Out] double[] a, ref int lda, [In, Out] double[] d, [In, Out] double[] e, [In, Out] double[] tauq, [In, Out] double[] taup, double* work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZGEBRD", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _zgebrd(ref int m, ref int n, [In, Out] Complex[] a, ref int lda, [In, Out] double[] d, [In, Out] double[] e, [In, Out] Complex[] tauq, [In, Out] Complex[] taup, [In, Out] Complex[] work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZGEBRD", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static unsafe extern void _zgebrd(ref int m, ref int n, [In, Out] Complex[] a, ref int lda, [In, Out] double[] d, [In, Out] double[] e, [In, Out] Complex[] tauq, [In, Out] Complex[] taup, Complex* work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DGBBRD", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _dgbbrd(ref char vect, ref int m, ref int n, ref int ncc, ref int kl, ref int ku, [In, Out] double[] ab, ref int ldab, [In, Out] double[] d, [In, Out] double[] e, [In, Out] double[] q, ref int ldq, [In, Out] double[] pt, ref int ldpt, [In, Out] double[] c, ref int ldc, [In, Out] double[] work, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZGBBRD", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _zgbbrd(ref char vect, ref int m, ref int n, ref int ncc, ref int kl, ref int ku, [In, Out] Complex[] ab, ref int ldab, [In, Out] double[] d, [In, Out] double[] e, [In, Out] Complex[] q, ref int ldq, [In, Out] Complex[] pt, ref int ldpt, [In, Out] Complex[] c, ref int ldc, [In, Out] Complex[] work, [In, Out] double[] rwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DORGBR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _dorgbr(ref char vect, ref int m, ref int n, ref int k, [In, Out] double[] a, ref int lda, [In, Out] double[] tau, [In, Out] double[] work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DORGBR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern unsafe void _dorgbr(ref char vect, ref int m, ref int n, ref int k, [In, Out] double[] a, ref int lda, [In, Out] double[] tau, double* work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DORMBR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _dormbr(ref char vect, ref char side, ref char trans, ref int m, ref int n, ref int k, [In, Out] double[] a, ref int lda, [In, Out] double[] tau, [In, Out] double[] c, ref int ldc, [In, Out] double[] work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DORMBR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern unsafe void _dormbr(ref char vect, ref char side, ref char trans, ref int m, ref int n, ref int k, [In, Out] double[] a, ref int lda, [In, Out] double[] tau, [In, Out] double[] c, ref int ldc, double* work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZUNGBR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _zungbr(ref char vect, ref int m, ref int n, ref int k, [In, Out] Complex[] a, ref int lda, [In, Out] Complex[] tau, [In, Out] Complex[] work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZUNGBR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern unsafe void _zungbr(ref char vect, ref int m, ref int n, ref int k, [In, Out] Complex[] a, ref int lda, [In, Out] Complex[] tau, Complex* work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZUNMBR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _zunmbr(ref char vect, ref char side, ref char trans, ref int m, ref int n, ref int k, [In, Out] Complex[] a, ref int lda, [In, Out] Complex[] tau, [In, Out] Complex[] c, ref int ldc, [In, Out] Complex[] work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZUNMBR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern unsafe void _zunmbr(ref char vect, ref char side, ref char trans, ref int m, ref int n, ref int k, [In, Out] Complex[] a, ref int lda, [In, Out] Complex[] tau, [In, Out] Complex[] c, ref int ldc, Complex* work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DBDSQR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _dbdsqr(ref char uplo, ref int n, ref int ncvt, ref int nru, ref int ncc, [In, Out] double[] d, [In, Out] double[] e, [In, Out] double[] vt, ref int ldvt, [In, Out] double[] u, ref int ldu, [In, Out] double[] c, ref int ldc, [In, Out] double[] work, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZBDSQR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _zbdsqr(ref char uplo, ref int n, ref int ncvt, ref int nru, ref int ncc, [In, Out] double[] d, [In, Out] double[] e, [In, Out] Complex[] vt, ref int ldvt, [In, Out] Complex[] u, ref int ldu, [In, Out] Complex[] c, ref int ldc, [In, Out] double[] rwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DBSDC", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _dbdsdc(ref char uplo, ref char compq, ref int n, [In, Out] double[] d, [In, Out] double[] e, [In, Out] double[] u, ref int ldu, [In, Out] double[] vt, ref int ldvt, [In, Out] double[] q, [In, Out] int[] iq, [In, Out] double[] work, [In, Out] int[] iwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DGESVD", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _driver_dgesvd(ref char jobu, ref char jobvt, ref int m, ref int n, [In, Out] double[] a, ref int lda, [In, Out] double[] s, [In, Out] double[] u, ref int ldu, [In, Out] double[] vt, ref int ldvt, [In, Out] double[] work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DGESVD", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static unsafe extern void _driver_dgesvd(ref char jobu, ref char jobvt, ref int m, ref int n, [In, Out] double[] a, ref int lda, [In, Out] double[] s, [In, Out] double[] u, ref int ldu, [In, Out] double[] vt, ref int ldvt, double* work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZGESVD", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _driver_zgesvd(ref char jobu, ref char jobvt, ref int m, ref int n, [In, Out] Complex[] a, ref int lda, [In, Out] double[] s, [In, Out] Complex[] u, ref int ldu, [In, Out] Complex[] vt, ref int ldvt, [In, Out] Complex[] work, ref int lwork, [In, Out] double[] rwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZGESVD", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static unsafe extern void _driver_zgesvd(ref char jobu, ref char jobvt, ref int m, ref int n, [In, Out] Complex[] a, ref int lda, [In, Out] double[] s, [In, Out] Complex[] u, ref int ldu, [In, Out] Complex[] vt, ref int ldvt, Complex* work, ref int lwork, [In, Out] double[] rwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DGESDD", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _driver_dgesdd(ref char jobz, ref int m, ref int n, [In, Out] double[] a, ref int lda, [In, Out] double[] s, [In, Out] double[] u, ref int ldu, [In, Out] double[] vt, ref int ldvt, [In, Out] double[] work, ref int lwork, [In, Out] int[] iwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DGESDD", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static unsafe extern void _driver_dgesdd(ref char jobz, ref int m, ref int n, [In, Out] double[] a, ref int lda, [In, Out] double[] s, [In, Out] double[] u, ref int ldu, [In, Out] double[] vt, ref int ldvt, double* work, ref int lwork, [In, Out] int[] iwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZGESDD", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _driver_zgesdd(ref char jobz, ref int m, ref int n, [In, Out] Complex[] a, ref int lda, [In, Out] double[] s, [In, Out] Complex[] u, ref int ldu, [In, Out] Complex[] vt, ref int ldvt, [In, Out] Complex[] work, ref int lwork, [In, Out] double[] rwork, [In, Out] int[] iwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZGESDD", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static unsafe extern void _driver_zgesdd(ref char jobz, ref int m, ref int n, [In, Out] Complex[] a, ref int lda, [In, Out] double[] s, [In, Out] Complex[] u, ref int ldu, [In, Out] Complex[] vt, ref int ldvt, Complex* work, ref int lwork, [In, Out] double[] rwork, [In, Out] int[] iwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DGEJSV", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _driver_dgejsv(ref char joba, ref char jobu, ref char jobv, ref char jobr, ref char jobt, ref char jobp, ref int m, ref int n, [In, Out] double[] a, ref int lda, [In, Out] double[] sva, [In, Out] double[] u, ref int ldu, [In, Out] double[] v, ref int ldv, [In, Out] double[] work, ref int lwork, [In, Out] int[] iwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DGESVJ", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _driver_dgesvj(ref char joba, ref char jobu, ref char jobv, ref int m, ref int n, [In, Out] double[] a, ref int lda, [In, Out] double[] sva, ref int mv, [In, Out] double[] v, ref int ldv, [In, Out] double[] work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DGGSVD", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _driver_dggsvd(ref char jobu, ref char jobv, ref char jobq, ref int m, ref int n, ref int p, out int k, out int l, [In, Out] double[] a, ref int lda, [In, Out] double[] b, ref int ldb, [In, Out] double[] alpha, [In, Out] double[] beta, [In, Out] double[] u, ref int ldu, [In, Out] double[] v, ref int ldv, [In, Out] double[] q, ref int ldq, [In, Out] double[] work, [In, Out] int[] iwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZGGSVD", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _driver_zggsvd(ref char jobu, ref char jobv, ref char jobq, ref int m, ref int n, ref int p, out int k, out int l, [In, Out] Complex[] a, ref int lda, [In, Out] Complex[] b, ref int ldb, [In, Out] double[] alpha, [In, Out] double[] beta, [In, Out] Complex[] u, ref int ldu, [In, Out] Complex[] v, ref int ldv, [In, Out] Complex[] q, ref int ldq, [In, Out] Complex[] work, [In, Out] double[] rwork, [In, Out] int[] iwork, out int info);
        #endregion

        #region public methods

        /// <summary>Gets a optimal workspace array length for the <c>dgebrd</c> function.
        /// </summary>
        /// <param name="m">The number of rows in the matrix.</param>
        /// <param name="n">The number of columns in the matrix.</param>
        /// <returns>The optimal workspace array length.</returns>
        public int dgebrdQuery(int m, int n)
        {
            int info;
            int lda = m;
            int lwork = -1;

            unsafe
            {
                double* work = stackalloc double[1];

                _dgebrd(ref m, ref n, null, ref lda, null, null, null, null, work, ref lwork, out info);

                CheckForError(info, "dgebrd");
                return ((int)work[0]) + 1;
            }
        }

        /// <summary>Reduces a general matrix to bidiagonal form, i.e. A = Q * B * P'.
        /// </summary>
        /// <param name="m">The number of rows in the matrix.</param>
        /// <param name="n">The number of columns in the matrix.</param>
        /// <param name="a">The matrix A provided column-by-column; overwritten by the upper/lower bidiagonal matrix B and details of matrix P.</param>
        /// <param name="d">The diagonal elements of B, the dimension must at least max(<paramref name="m"/>, <paramref name="n"/>) (output).</param>
        /// <param name="e">Contains off-diagonal elements of matrix B, the dimension must be at least max(<paramref name="m"/>, <paramref name="n"/>) - 1 (output).</param>
        /// <param name="tauq">Contains further details of the matrix Q, the dimension must be at least max(<paramref name="m"/>, <paramref name="n"/>) (output).</param>
        /// <param name="taup">Contains further details of the matrix P, the dimension must be at least max(<paramref name="m"/>, <paramref name="n"/>) (output).</param>
        /// <param name="work">A workspace array with dimension at least max(<paramref name="m"/>, <paramref name="n"/>).</param>
        public void dgebrd(int m, int n, double[] a, double[] d, double[] e, double[] tauq, double[] taup, double[] work)
        {
            int info;
            int lda = m;
            int lwork = work.Length;

            _dgebrd(ref m, ref n, a, ref lda, d, e, tauq, taup, work, ref lwork, out info);
            CheckForError(info, "dgebrd");
        }

        /// <summary>Gets a optimal workspace array length for the <c>zgebrd</c> function.
        /// </summary>
        /// <param name="m">The number of rows in the matrix.</param>
        /// <param name="n">The number of columns in the matrix.</param>
        /// <returns>The optimal workspace array length.</returns>
        public int zgebrdQuery(int m, int n)
        {
            int info;
            int lda = m;
            int lwork = -1;

            unsafe
            {
                Complex* work = stackalloc Complex[1];

                _zgebrd(ref m, ref n, null, ref lda, null, null, null, null, work, ref lwork, out info);

                CheckForError(info, "zgebrd");
                return ((int)work[0].Real) + 1;
            }
        }

        /// <summary>Reduces a general matrix to bidiagonal form, i.e. A = Q * B * P^H.
        /// </summary>
        /// <param name="m">The number of rows in the matrix.</param>
        /// <param name="n">The number of columns in the matrix.</param>
        /// <param name="a">The matrix A provided column-by-column; overwritten by the upper/lower bidiagonal matrix B and details of matrix P.</param>
        /// <param name="d">The diagonal elements of B, the dimension must at least max(<paramref name="m"/>, <paramref name="n"/>) (output).</param>
        /// <param name="e">Contains off-diagonal elements of matrix B, the dimension must be at least max(<paramref name="m"/>, <paramref name="n"/>) - 1 (output).</param>
        /// <param name="tauq">Contains further details of the matrix Q, the dimension must be at least max(<paramref name="m"/>, <paramref name="n"/>) (output).</param>
        /// <param name="taup">Contains further details of the matrix P, the dimension must be at least max(<paramref name="m"/>, <paramref name="n"/>) (output).</param>
        /// <param name="work">A workspace array with dimension at least max(<paramref name="m"/>, <paramref name="n"/>).</param>
        public void zgebrd(int m, int n, Complex[] a, double[] d, double[] e, Complex[] tauq, Complex[] taup, Complex[] work)
        {
            int info;
            int lda = m;
            int lwork = work.Length;

            _zgebrd(ref m, ref n, a, ref lda, d, e, tauq, taup, work, ref lwork, out info);
            CheckForError(info, "zgebrd");
        }

        /// <summary>Reduces a general band matrix to bidiagonal form, i.e. A = Q * B * P'. The routine can also update a matrix C as C = Q' * C.
        /// </summary>
        /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="m">The number of rows in the general band matrix A.</param>
        /// <param name="n">The number of columns in the general band matrix A.</param>
        /// <param name="ncc">The number of columns in C.</param>
        /// <param name="kl">The number of sub-diagonals within the band of A.</param>
        /// <param name="ku">The number of super-diagonals within the band of A.</param>
        /// <param name="ab">The general band matrix A in band storage; overwritten by values generated during the reduction.</param>
        /// <param name="d">Contains the diagonal elements of the matrix B; dimension at least min(<paramref name="m"/>, <paramref name="n"/>) (output).</param>
        /// <param name="e">Contains the off-diaognal elements of matrix B; dimension at least min(<paramref name="m"/>, <paramref name="n"/>) - 1 (output).</param>
        /// <param name="q">Contains the <paramref name="m"/>-by-<paramref name="m"/> matrix Q (output).</param>
        /// <param name="pt">Contains the <paramref name="n"/>-by-<paramref name="n"/> matrix P' (output).</param>
        /// <param name="c">The matrix <paramref name="m"/>-by-<paramref name="ncc"/> matrix C; overwritten by the product Q' * C; if <paramref name="ncc"/> == 0, the parameter is not referenced.</param>
        /// <param name="work">A workspace array with dimension at least 2 * max(<paramref name="m"/>, <paramref name="n"/>).</param>
        public void dgbbrd(LapackEigenvalues.SVDxgbbrdJob job, int m, int n, int ncc, int kl, int ku, double[] ab, double[] d, double[] e, double[] q, double[] pt, double[] c, double[] work)
        {
            int info;
            int ldab = kl + ku + 1;
            int ldq = job.HasFlag(LapackEigenvalues.SVDxgbbrdJob.MatrixQ) ? m : 1;
            int ldpt = job.HasFlag(LapackEigenvalues.SVDxgbbrdJob.MatrixP) ? n : 1;
            int ldc = (ncc > 0) ? m : 1;
            char vect = GetJob(job);

            _dgbbrd(ref vect, ref m, ref n, ref ncc, ref kl, ref ku, ab, ref ldab, d, e, q, ref ldq, pt, ref ldpt, c, ref ldc, work, out info);
            CheckForError(info, "dgbbrd");
        }

        /// <summary>Reduces a general band matrix to bidiagonal form, i.e. A = Q * B * P^H. The routine can also update a matrix C as C = Q^H * C.
        /// </summary>
        /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="m">The number of rows in the general band matrix A.</param>
        /// <param name="n">The number of columns in the general band matrix A.</param>
        /// <param name="ncc">The number of columns in C.</param>
        /// <param name="kl">The number of sub-diagonals within the band of A.</param>
        /// <param name="ku">The number of super-diagonals within the band of A.</param>
        /// <param name="ab">The general band matrix A in band storage; overwritten by values generated during the reduction.</param>
        /// <param name="d">Contains the diagonal elements of the matrix B; dimension at least min(<paramref name="m"/>, <paramref name="n"/>) (output).</param>
        /// <param name="e">Contains the off-diaognal elements of matrix B; dimension at least min(<paramref name="m"/>, <paramref name="n"/>) - 1 (output).</param>
        /// <param name="q">Contains the <paramref name="m"/>-by-<paramref name="m"/> matrix Q (output).</param>
        /// <param name="pt">Contains the <paramref name="n"/>-by-<paramref name="n"/> matrix P^H (output).</param>
        /// <param name="c">The matrix <paramref name="m"/>-by-<paramref name="ncc"/> matrix C; overwritten by the product Q^H * C; if <paramref name="ncc"/> == 0, the parameter is not referenced.</param>
        /// <param name="work">A workspace array with dimension at least max(<paramref name="m"/>, <paramref name="n"/>).</param>
        /// <param name="rwork">A workspace array with dimension at least max(<paramref name="m"/>, <paramref name="n"/>).</param>
        public void zgbbrd(LapackEigenvalues.SVDxgbbrdJob job, int m, int n, int ncc, int kl, int ku, Complex[] ab, double[] d, double[] e, Complex[] q, Complex[] pt, Complex[] c, Complex[] work, double[] rwork)
        {
            int info;
            int ldab = kl + ku + 1;
            int ldq = job.HasFlag(LapackEigenvalues.SVDxgbbrdJob.MatrixQ) ? m : 1;
            int ldpt = job.HasFlag(LapackEigenvalues.SVDxgbbrdJob.MatrixP) ? n : 1;
            int ldc = (ncc > 0) ? m : 1;
            char vect = GetJob(job);

            _zgbbrd(ref vect, ref m, ref n, ref ncc, ref kl, ref ku, ab, ref ldab, d, e, q, ref ldq, pt, ref ldpt, c, ref ldc, work, rwork, out info);
            CheckForError(info, "zgbbrd");
        }

        /// <summary>Gets a optimal workspace array length for the <c>dorgbr</c> function.
        /// </summary>
        /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="m">The number of rows in the matrix Q or P' to be returned.</param>
        /// <param name="n">The number of columns in the matrix Q or P' to be returned.</param>
        /// <param name="k">The number of columns/rows in the original <paramref name="m"/>-by-<paramref name="k"/> or <paramref name="k"/>-by-<paramref name="n"/> matrix reduced by <c>dgebrd</c>.</param>
        /// <returns>The optimal workspace array length.</returns>
        public int dorgbrQuery(LapackEigenvalues.SVDxorgbrJob job, int m, int n, int k)
        {
            int info;
            int lda = m;
            int lwork = -1;
            var vect = GetJob(job);
            unsafe
            {
                double* work = stackalloc double[1];

                _dorgbr(ref vect, ref m, ref n, ref k, null, ref lda, null, work, ref lwork, out info);

                CheckForError(info, "dgebrd");
                return ((int)work[0]) + 1;
            }
        }

        /// <summary>Generates the real orthogonal matrix Q or P' determined by <c>dgebrd</c>.
        /// </summary>
        /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="m">The number of rows in the matrix Q or P' to be returned.</param>
        /// <param name="n">The number of columns in the matrix Q or P' to be returned.</param>
        /// <param name="k">The number of columns/rows in the original <paramref name="m"/>-by-<paramref name="k"/> or <paramref name="k"/>-by-<paramref name="n"/> matrix reduced by <c>dgebrd</c>.</param>
        /// <param name="a">The vectors which define the elementary reflectors, as returned by <c>dgebrd</c>; overwritten by the orthogonal matrix Q or P'.</param>
        /// <param name="tau">Scalar factor of the elementary reflector H(i) or G(i) which determines Q and P' as returned by <c>dgebrd</c> in the array <c>tauq</c> or <c>taup</c>.</param>
        /// <param name="work">A workspace array.</param>
        public void dorgbr(LapackEigenvalues.SVDxorgbrJob job, int m, int n, int k, double[] a, double[] tau, double[] work)
        {
            int info;
            int lda = m;
            var vect = GetJob(job);
            int lwork = work.Length;

            _dorgbr(ref vect, ref m, ref n, ref k, a, ref lda, tau, work, ref lwork, out info);
            CheckForError(info, "dorgbr");
        }

        /// <summary>Gets a optimal workspace array length for the <c>dormbr</c> function.
        /// </summary>
        /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="m">The number of rows in the matrix C.</param>
        /// <param name="n">The number of columns in the matrix C.</param>        
        /// <param name="k">One of the dimension of matrix A in <c>dgebrd</c>. If <paramref name="job"/> indicates matrix Q it is the number of columns in A; otherwise the number of rows.</param>
        /// <param name="side">A value indicating whether multipliers are applied to matrix C from the left or from the right.</param>
        /// <param name="transposeState">A value indicating whether the routine multiplies C by X or X', where X is matrix P or matrix Q with respect to <paramref name="job"/>.</param>
        /// <returns>The optimal workspace array length.</returns>
        public int dormbrQuery(LapackEigenvalues.SVDxormbrJob job, int m, int n, int k, LAPACK.Side side = LAPACK.Side.Left, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            int info;
            int ldc = m;
            int lwork = -1;
            var vect = GetJob(job);
            var sidez = LAPACK.GetSide(side);
            var trans = LAPACK.GetTrans(transposeState);
            int r = (side == LAPACK.Side.Left) ? m : n;
            int lda = (job == LapackEigenvalues.SVDxormbrJob.ApplyQ) ? r : Math.Min(r, k);

            unsafe
            {
                double* work = stackalloc double[1];

                _dormbr(ref vect, ref sidez, ref trans, ref m, ref n, ref k, null, ref lda, null, null, ref ldc, work, ref lwork, out info);

                CheckForError(info, "dormbr");
                return ((int)work[0]) + 1;
            }
        }

        /// <summary>Multiplies an arbitrary real matrix by the real orthogonal matrix Q or P' determined by <c>dgebrd</c>, i.e. computes op(Q) * C, op(P) * C, C * op(Q) or C * op(P).
        /// </summary>
        /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="m">The number of rows in the matrix C.</param>
        /// <param name="n">The number of columns in the matrix C.</param>        
        /// <param name="k">One of the dimension of matrix A in <c>dgebrd</c>. If <paramref name="job"/> indicates matrix Q it is the number of columns in A; otherwise the number of rows.</param>
        /// <param name="a">The array returned by <c>dgebrd</c>; </param>
        /// <param name="tau">The parameter <c>tauq</c> or <c>taup</c> as returned by <c>dgebrd</c> depending on <paramref name="job"/>.</param>
        /// <param name="c">Overwritten by the product op(Q) * C, op(P) * C, C * op(Q) or C * op(P) as specified by <paramref name="job"/>, <paramref name="side"/> and <paramref name="transposeState"/>.</param>
        /// <param name="work">A workspace array with dimension at least <paramref name="n"/> if <paramref name="side"/> indicates a multiplication on the left side; at least <paramref name="m"/> otherwise.</param>
        /// <param name="side">A value indicating whether multipliers are applied to matrix C from the left or from the right.</param>
        /// <param name="transposeState">A value indicating whether the routine multiplies C by X or X', where X is matrix P or matrix Q with respect to <paramref name="job"/>.</param>
        public void dormbr(LapackEigenvalues.SVDxormbrJob job, int m, int n, int k, double[] a, double[] tau, double[] c, double[] work, LAPACK.Side side = LAPACK.Side.Left, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            int info;
            int ldc = m;
            int lwork = work.Length;
            var vect = GetJob(job);
            var sidez = LAPACK.GetSide(side);
            var trans = LAPACK.GetTrans(transposeState);
            int r = (side == LAPACK.Side.Left) ? m : n;
            int lda = (job == LapackEigenvalues.SVDxormbrJob.ApplyQ) ? r : Math.Min(r, k);

            _dormbr(ref vect, ref sidez, ref trans, ref m, ref n, ref k, a, ref lda, tau, c, ref ldc, work, ref lwork, out info);
            CheckForError(info, "dormbr");
        }

        /// <summary>Gets a optimal workspace array length for the <c>zungbr</c> function.
        /// </summary>
        /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="m">The number of rows in the matrix Q or P' to be returned.</param>
        /// <param name="n">The number of columns in the matrix Q or P' to be returned.</param>
        /// <param name="k">The number of columns/rows in the original <paramref name="m"/>-by-<paramref name="k"/> or <paramref name="k"/>-by-<paramref name="n"/> matrix reduced by <c>dgebrd</c>.</param>
        /// <returns>The optimal workspace array length.</returns>
        public int zungbrQuery(LapackEigenvalues.SVDxorgbrJob job, int m, int n, int k)
        {
            int info;
            int lda = m;
            int lwork = -1;
            var vect = GetJob(job);
            unsafe
            {
                Complex* work = stackalloc Complex[1];

                _zungbr(ref vect, ref m, ref n, ref k, null, ref lda, null, work, ref lwork, out info);

                CheckForError(info, "zungbr");
                return ((int)work[0].Real) + 1;
            }
        }

        /// <summary>Generates the complex unitary matrix Q or P^H determined by <c>zgebrd</c>.
        /// </summary>
        /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="m">The number of rows in the matrix Q or P' to be returned.</param>
        /// <param name="n">The number of columns in the matrix Q or P' to be returned.</param>
        /// <param name="k">The number of columns/rows in the original <paramref name="m"/>-by-<paramref name="k"/> or <paramref name="k"/>-by-<paramref name="n"/> matrix reduced by <c>dgebrd</c>.</param>
        /// <param name="a">The vectors which define the elementary reflectors, as returned by <c>dgebrd</c>; overwritten by the orthogonal matrix Q or P'.</param>
        /// <param name="tau">Scalar factor of the elementary reflector H(i) or G(i) which determines Q and P' as returned by <c>dgebrd</c> in the array <c>tauq</c> or <c>taup</c>.</param>
        /// <param name="work">A workspace array.</param>
        public void zungbr(LapackEigenvalues.SVDxorgbrJob job, int m, int n, int k, Complex[] a, Complex[] tau, Complex[] work)
        {
            int info;
            int lda = m;
            var vect = GetJob(job);
            int lwork = work.Length;
            _zungbr(ref vect, ref m, ref n, ref k, a, ref lda, tau, work, ref lwork, out info);
            CheckForError(info, "zungbr");
        }

        /// <summary>Gets a optimal workspace array length for the <c>zunmbr</c> function.
        /// </summary>
        /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="m">The number of rows in the matrix C.</param>
        /// <param name="n">The number of columns in the matrix C.</param>        
        /// <param name="k">One of the dimension of matrix A in <c>dgebrd</c>. If <paramref name="job"/> indicates matrix Q it is the number of columns in A; otherwise the number of rows.</param>
        /// <param name="side">A value indicating whether multipliers are applied to matrix C from the left or from the right.</param>
        /// <param name="transposeState">A value indicating whether the routine multiplies C by X or X', where X is matrix P or matrix Q with respect to <paramref name="job"/>.</param>
        /// <returns>The optimal workspace array length.</returns>
        public int zunmbrQuery(LapackEigenvalues.SVDxormbrJob job, int m, int n, int k, LAPACK.Side side = LAPACK.Side.Left, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            int info;
            int ldc = m;
            int lwork = -1;
            var vect = GetJob(job);
            var sidez = LAPACK.GetSide(side);
            var trans = LAPACK.GetTrans(transposeState);
            int r = (side == LAPACK.Side.Left) ? m : n;
            int lda = (job == LapackEigenvalues.SVDxormbrJob.ApplyQ) ? r : Math.Min(r, k);

            unsafe
            {
                Complex* work = stackalloc Complex[1];

                _zunmbr(ref vect, ref sidez, ref trans, ref m, ref n, ref k, null, ref lda, null, null, ref ldc, work, ref lwork, out info);

                CheckForError(info, "zunmbr");
                return ((int)work[0].Real) + 1;
            }
        }

        /// <summary>Multiplies an arbitrary real matrix by the real orthogonal matrix Q or P determined by <c>zgebrd</c>, i.e. computes op(Q) * C, op(P) * C, C * op(Q) or C * op(P).
        /// </summary>
        /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="m">The number of rows in the matrix C.</param>
        /// <param name="n">The number of columns in the matrix C.</param>        
        /// <param name="k">One of the dimension of matrix A in <c>zgebrd</c>. If <paramref name="job"/> indicates matrix Q it is the number of columns in A; otherwise the number of rows.</param>
        /// <param name="a">The array returned by <c>dgebrd</c>; </param>
        /// <param name="tau">The parameter <c>tauq</c> or <c>taup</c> as returned by <c>zgebrd</c> depending on <paramref name="job"/>.</param>
        /// <param name="c">Overwritten by the product op(Q) * C, op(P) * C, C * op(Q) or C * op(P) as specified by <paramref name="job"/>, <paramref name="side"/> and <paramref name="transposeState"/>.</param>
        /// <param name="work">A workspace array with dimension at least <paramref name="n"/> if <paramref name="side"/> indicates a multiplication on the left side; at least <paramref name="m"/> otherwise.</param>
        /// <param name="side">A value indicating whether multipliers are applied to matrix C from the left or from the right.</param>
        /// <param name="transposeState">A value indicating whether the routine multiplies C by X or X^H, where X is matrix P or matrix Q with respect to <paramref name="job"/>.</param>
        public void zunmbr(LapackEigenvalues.SVDxormbrJob job, int m, int n, int k, Complex[] a, Complex[] tau, Complex[] c, Complex[] work, LAPACK.Side side = LAPACK.Side.Left, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            int info;
            int ldc = m;
            int lwork = work.Length;
            var vect = GetJob(job);
            var sidez = LAPACK.GetSide(side);
            var trans = LAPACK.GetTrans(transposeState);
            int r = (side == LAPACK.Side.Left) ? m : n;
            int lda = (job == LapackEigenvalues.SVDxormbrJob.ApplyQ) ? r : Math.Min(r, k);

            _zunmbr(ref vect, ref sidez, ref trans, ref m, ref n, ref k, a, ref lda, tau, c, ref ldc, work, ref lwork, out info);
            CheckForError(info, "zunmbr");
        }

        /// <summary>Computes the singular value decomposition of a general matrix that has been reduced to bidiagonal form, i.e. B = Q * S * P^H. Optionally, the subroutine may compute also Q^H * C for a specific matrix C.
        /// </summary>
        /// <param name="n">The order of matrix B.</param>
        /// <param name="ncvt">The number of columns of the matrix VT, that is, the number of right singular vectors. Set to 0 if no right singular vectors are required.</param>
        /// <param name="nru">The number of rows in U, that is, the number of left singular vectors. Set to 0 if no left singular vectors are required.</param>
        /// <param name="ncc">The number of columns in the matrix C used for computing the product Q^H * C. Set to 0 if no matrix C is supplied.</param>
        /// <param name="d">The diagonal elements of B; the dimension must be at least <paramref name="n"/>.</param>
        /// <param name="e">The off-diagonal elements of B; the dimension must be at least <paramref name="n"/>.</param>
        /// <param name="vt">Contains the <paramref name="n"/>-by-<paramref name="ncvt"/> matrix. This parameter is not referenced if <paramref name="nru"/> = 0.</param>
        /// <param name="u">Contains the <paramref name="nru"/>-by-<paramref name="n"/> unit matrix U. This parameter is not referenced if <paramref name="nru"/> = 0.</param>
        /// <param name="c">The matrix C for computing the product Q^H * C. This parameter is not referenced if <paramref name="ncc"/> = 0.</param>
        /// <param name="work">A workspace array; the dimension must be at least 4 * <paramref name="n"/>.</param>
        /// <param name="triangularMatrixType">A value indicating whether B is upper or lower bidiagonal matrix.</param>
        public void dbsdsqr(int n, int ncvt, int nru, int ncc, double[] d, double[] e, double[] vt, double[] u, double[] c, double[] work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var uplo = LAPACK.GetUplo(triangularMatrixType);
            int ldvt = (ncvt > 0) ? n : 0;
            int ldu = nru;
            int ldc = (ncc > 0) ? n : 1;

            _dbdsqr(ref uplo, ref n, ref ncvt, ref nru, ref ncc, d, e, vt, ref ldvt, u, ref ldu, c, ref ldc, work, out info);
            CheckForError(info, "dbsdsqr");
        }

        /// <summary>Computes the singular value decomposition of a general matrix that has been reduced to bidiagonal form, i.e. B = Q * S * P^H. Optionally, the subroutine may compute also Q^H * C for a specific matrix C.
        /// </summary>
        /// <param name="n">The order of matrix B.</param>
        /// <param name="ncvt">The number of columns of the matrix VT, that is, the number of right singular vectors. Set to 0 if no right singular vectors are required.</param>
        /// <param name="nru">The number of rows in U, that is, the number of left singular vectors. Set to 0 if no left singular vectors are required.</param>
        /// <param name="ncc">The number of columns in the matrix C used for computing the product Q^H * C. Set to 0 if no matrix C is supplied.</param>
        /// <param name="d">The diagonal elements of B; the dimension must be at least <paramref name="n"/>.</param>
        /// <param name="e">The off-diagonal elements of B; the dimension must be at least <paramref name="n"/>.</param>
        /// <param name="vt">Contains the <paramref name="n"/>-by-<paramref name="ncvt"/> matrix. This parameter is not referenced if <paramref name="nru"/> = 0.</param>
        /// <param name="u">Contains the <paramref name="nru"/>-by-<paramref name="n"/> unit matrix U. This parameter is not referenced if <paramref name="nru"/> = 0.</param>
        /// <param name="c">The matrix C for computing the product Q^H * C. This parameter is not referenced if <paramref name="ncc"/> = 0.</param>
        /// <param name="work">A workspace array; the dimension must be at least 4 * <paramref name="n"/>.</param>
        /// <param name="triangularMatrixType">A value indicating whether B is upper or lower bidiagonal matrix.</param>
        public void zbsdsqr(int n, int ncvt, int nru, int ncc, double[] d, double[] e, Complex[] vt, Complex[] u, Complex[] c, double[] work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var uplo = LAPACK.GetUplo(triangularMatrixType);
            int ldvt = (ncvt > 0) ? n : 0;
            int ldu = nru;
            int ldc = (ncc > 0) ? n : 1;

            _zbdsqr(ref uplo, ref n, ref ncvt, ref nru, ref ncc, d, e, vt, ref ldvt, u, ref ldu, c, ref ldc, work, out info);
            CheckForError(info, "zbsdsqr");
        }

        /// <summary>Computes the singular value decomposition of a real bidiagonal matrix using a divide and conquer method.
        /// </summary>
        /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="n">The order of matrix B.</param>
        /// <param name="d">The <paramref name="n"/> diagonal elements of the bidiagonal matrix B; overwritten by the singular values of B.</param>
        /// <param name="e">The off-diagonal elements of the bidiagonal matrix B. The dimension must be at least <paramref name="n"/>. Overwritten on exit.</param>
        /// <param name="u">The left singular vectors if specified by <paramref name="job"/> (output).</param>
        /// <param name="vt">The right singular vectors if specified by <paramref name="job"/> (output).</param>
        /// <param name="q">If <paramref name="job"/> indicates the calculation of singular vectors in compact form, this parameter contains all real data for singular vectors; otherwise this parameter is not referenced.</param>
        /// <param name="iq">If <paramref name="job"/> indicates the calculation of singular vectors in compact form, this parameter contains all integer data for singular vectors; otherwise this parameter is not referenced.</param>
        /// <param name="work">A workspace array with dimension at least 4*<paramref name="n"/>, 6 * <paramref name="n"/> or 3*<paramref name="n"/>^2 + 4 * <paramref name="n"/> as specified by <paramref name="job"/>.</param>
        /// <param name="iwork">A workspace array with dimension at least 8 * <paramref name="n"/>.</param>
        /// <param name="triangularMatrixType">A value indicating whether matrix B is upper or lower diagonal matrix.</param>
        public void dbdsdc(LapackEigenvalues.SVDdbdsdcJob job, int n, double[] d, double[] e, double[] u, double[] vt, double[] q, int[] iq, double[] work, int[] iwork, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var jobz = GetJob(job);
            var uplo = LAPACK.GetUplo(triangularMatrixType);
            int ldu = (job == LapackEigenvalues.SVDdbdsdcJob.SingularValuesOnly) ? 1 : n;
            int ldvt = (job == LapackEigenvalues.SVDdbdsdcJob.SingularValuesOnly) ? 1 : n;

            _dbdsdc(ref uplo, ref jobz, ref n, d, e, u, ref ldu, vt, ref ldvt, q, iq, work, iwork, out info);
            CheckForError(info, "dbdsdc");
        }

        /// <summary>Gets a optimal workspace array length for the <c>dgesvd</c> function.
        /// </summary>
        /// <param name="uJob">A value indicating what kind of job to do by the LAPACK function (for matrix U).</param>
        /// <param name="vtJob">A value indicating what kind of job to do by the LAPACK function (for matrix V').</param>
        /// <param name="m">The number of rows of matrix A.</param>
        /// <param name="n">The number of columns of matrix A.</param>
        public int driver_dgesvdQuery(int m, int n, LapackEigenvalues.SVDleftSingularVectorsJob uJob = LapackEigenvalues.SVDleftSingularVectorsJob.All, LapackEigenvalues.SVDrightSingularVectorsJob vtJob = LapackEigenvalues.SVDrightSingularVectorsJob.All)
        {
            if ((uJob == LapackEigenvalues.SVDleftSingularVectorsJob.InPlace) && (vtJob == LapackEigenvalues.SVDrightSingularVectorsJob.InPlace))
            {
                throw new ArgumentException("Invalid argument combination!", "uJob/vtJob");
            }
            unsafe
            {
                int ldu = (uJob == LapackEigenvalues.SVDleftSingularVectorsJob.All || uJob == LapackEigenvalues.SVDleftSingularVectorsJob.OutOfPlace) ? m : 1;
                int ldv = 1;
                if (vtJob == LapackEigenvalues.SVDrightSingularVectorsJob.All)
                {
                    ldv = n;
                }
                else if (vtJob == LapackEigenvalues.SVDrightSingularVectorsJob.OutOfPlace)
                {
                    ldv = Math.Min(m, n);
                }

                int info;
                int lwork = -1;
                var jobu = GetJob(uJob);
                var jobvt = GetJob(vtJob);
                double* work = stackalloc double[1];

                _driver_dgesvd(ref jobu, ref jobvt, ref m, ref n, null, ref m, null, null, ref ldu, null, ref ldv, work, ref lwork, out info);
                CheckForError(info, "dgesvd");

                return ((int)work[0]) + 1;
            }
        }

        /// <summary>Computes the singular value decomposition of a general rectangular matrix, i.e. 'A = U * \Sigma * V^t', where the diagonal elements
        /// of '\Sigma' are the singular values in descending order.
        /// </summary>
        /// <param name="uJob">A value indicating what kind of job to do by the LAPACK function (for matrix U).</param>
        /// <param name="vtJob">A value indicating what kind of job to do by the LAPACK function (for matrix V').</param>
        /// <param name="m">The number of rows of matrix A.</param>
        /// <param name="n">The number of columns of matrix A.</param>
        /// <param name="a">The <paramref name="m"/>-by-<paramref name="n"/> matrix A provided column-by-column; will be perhaps overwritten on exit.</param>
        /// <param name="s">The singular values in descending order, must have at least min{<paramref name="m"/>, <paramref name="n"/>} elements (output).</param>
        /// <param name="u">The left singular vectors 'U', i.e. a 'm x m' unitary matrix (output, if <paramref name="uJob"/> indicate to calculate left singular vectors).</param>
        /// <param name="vt">The right singular vectors 'V^t', i.e. a 'n x n' unitary matrix (output, if <paramref name="vtJob"/> is indicate to calculate right singular vectors).</param>
        /// <param name="work">A workspace array.</param>
        /// <remarks>The singular values are the roots of the non-negative eigenvalues of A^t*A.</remarks>        
        public void driver_dgesvd(int m, int n, double[] a, double[] s, double[] u, double[] vt, double[] work, LapackEigenvalues.SVDleftSingularVectorsJob uJob = LapackEigenvalues.SVDleftSingularVectorsJob.All, LapackEigenvalues.SVDrightSingularVectorsJob vtJob = LapackEigenvalues.SVDrightSingularVectorsJob.All)
        {
            if ((uJob == LapackEigenvalues.SVDleftSingularVectorsJob.InPlace) && (vtJob == LapackEigenvalues.SVDrightSingularVectorsJob.InPlace))
            {
                throw new ArgumentException("Invalid argument combination!", "uJob/vtJob");
            }
            int ldu = (uJob == LapackEigenvalues.SVDleftSingularVectorsJob.All || uJob == LapackEigenvalues.SVDleftSingularVectorsJob.OutOfPlace) ? m : 1;
            int ldvt = 1;
            if (vtJob == LapackEigenvalues.SVDrightSingularVectorsJob.All)
            {
                ldvt = n;
            }
            else if (vtJob == LapackEigenvalues.SVDrightSingularVectorsJob.OutOfPlace)
            {
                ldvt = Math.Min(m, n);
            }

            int info;
            int lwork = work.Length;
            var jobu = GetJob(uJob);
            var jobvt = GetJob(vtJob);

            _driver_dgesvd(ref jobu, ref jobvt, ref m, ref n, a, ref m, s, u, ref ldu, vt, ref ldvt, work, ref lwork, out info);
            CheckForError(info, "dgesvd");
        }

        /// <summary>Gets a optimal workspace array length for the <c>zgesvd</c> function.
        /// </summary>
        /// <param name="m">The number of rows of matrix A.</param>
        /// <param name="n">The number of columns of matrix A.</param>
        /// <param name="uJob">A value indicating what kind of job to do by the LAPACK function (for matrix U).</param>
        /// <param name="vtJob">A value indicating what kind of job to do by the LAPACK function (for matrix V').</param>
        public int driver_zgesvdQuery(int m, int n, LapackEigenvalues.SVDleftSingularVectorsJob uJob = LapackEigenvalues.SVDleftSingularVectorsJob.All, LapackEigenvalues.SVDrightSingularVectorsJob vtJob = LapackEigenvalues.SVDrightSingularVectorsJob.All)
        {
            if ((uJob == LapackEigenvalues.SVDleftSingularVectorsJob.InPlace) && (vtJob == LapackEigenvalues.SVDrightSingularVectorsJob.InPlace))
            {
                throw new ArgumentException("Invalid argument combination!", "uJob/vtJob");
            }
            unsafe
            {
                int ldu = (uJob == LapackEigenvalues.SVDleftSingularVectorsJob.All || uJob == LapackEigenvalues.SVDleftSingularVectorsJob.OutOfPlace) ? m : 1;
                int ldv = 1;
                if (vtJob == LapackEigenvalues.SVDrightSingularVectorsJob.All)
                {
                    ldv = n;
                }
                else if (vtJob == LapackEigenvalues.SVDrightSingularVectorsJob.OutOfPlace)
                {
                    ldv = Math.Min(m, n);
                }

                int info;
                int lwork = -1;
                var jobu = GetJob(uJob);
                var jobvt = GetJob(vtJob);
                Complex* work = stackalloc Complex[1];

                _driver_zgesvd(ref jobu, ref jobvt, ref m, ref n, null, ref m, null, null, ref ldu, null, ref ldv, work, ref lwork, null, out info);
                CheckForError(info, "zgesvd");

                return ((int)work[0].Real) + 1;
            }
        }

        /// <summary>Computes the singular value decomposition of a general rectangular matrix, i.e. 'A = U * \Sigma * V^t', where the diagonal elements
        /// of '\Sigma' are the singular values in descending order.
        /// </summary>
        /// <param name="m">The number of rows of matrix A.</param>
        /// <param name="n">The number of columns of matrix A.</param>
        /// <param name="a">The <paramref name="m"/>-by-<paramref name="n"/> matrix A provided column-by-column; will be perhaps overwritten on exit.</param>
        /// <param name="s">The singular values in descending order, must have at least min{<paramref name="m"/>, <paramref name="n"/>} elements (output).</param>
        /// <param name="u">The left singular vectors 'U', i.e. a 'm x m' unitary matrix (output, if <paramref name="uJob"/> indicate to calculate left singular vectors).</param>
        /// <param name="vt">The right singular vectors 'V^t', i.e. a 'n x n' unitary matrix (output, if <paramref name="vtJob"/> is indicate to calculate right singular vectors).</param>
        /// <param name="work">A workspace array.</param>
        /// <param name="rwork">A workspace array with dimension at least 5 * min(<paramref name="m"/>, <paramref name="n"/>).</param>
        /// <param name="uJob">A value indicating what kind of job to do by the LAPACK function (for matrix U).</param>
        /// <param name="vtJob">A value indicating what kind of job to do by the LAPACK function (for matrix V').</param>
        /// <remarks>The singular values are the roots of the non-negative eigenvalues of A^t*A.</remarks>        
        public void driver_zgesvd(int m, int n, Complex[] a, double[] s, Complex[] u, Complex[] vt, Complex[] work, double[] rwork, LapackEigenvalues.SVDleftSingularVectorsJob uJob = LapackEigenvalues.SVDleftSingularVectorsJob.All, LapackEigenvalues.SVDrightSingularVectorsJob vtJob = LapackEigenvalues.SVDrightSingularVectorsJob.All)
        {
            if ((uJob == LapackEigenvalues.SVDleftSingularVectorsJob.InPlace) && (vtJob == LapackEigenvalues.SVDrightSingularVectorsJob.InPlace))
            {
                throw new ArgumentException("Invalid argument combination!", "uJob/vtJob");
            }
            int ldu = (uJob == LapackEigenvalues.SVDleftSingularVectorsJob.All || uJob == LapackEigenvalues.SVDleftSingularVectorsJob.OutOfPlace) ? m : 1;
            int ldvt = 1;
            if (vtJob == LapackEigenvalues.SVDrightSingularVectorsJob.All)
            {
                ldvt = n;
            }
            else if (vtJob == LapackEigenvalues.SVDrightSingularVectorsJob.OutOfPlace)
            {
                ldvt = Math.Min(m, n);
            }

            int info;
            int lwork = work.Length;
            var jobu = GetJob(uJob);
            var jobvt = GetJob(vtJob);

            _driver_zgesvd(ref jobu, ref jobvt, ref m, ref n, a, ref m, s, u, ref ldu, vt, ref ldvt, work, ref lwork, rwork, out info);
            CheckForError(info, "zgesvd");
        }

        /// <summary>Gets a optimal workspace array length for the <c>driver_dgesdd</c> function.
        /// </summary>
        /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="m">The number of rows of matrix A.</param>
        /// <param name="n">The number of columns of matrix A.</param>
        /// <returns>The optimal workspace array length.</returns>
        public int driver_dgesddQuery(LapackEigenvalues.SVDxgesddJob job, int m, int n)
        {
            unsafe
            {
                int info;

                int lwork = -1;
                int lda = m;
                var jobz = GetJob(job);
                int ldu = (((job == LapackEigenvalues.SVDxgesddJob.All) || (job == LapackEigenvalues.SVDxgesddJob.Selected) || (job == LapackEigenvalues.SVDxgesddJob.Overwritten)) && (m < n)) ? m : 1;
                int ldvt = (job == LapackEigenvalues.SVDxgesddJob.Selected) ? Math.Min(n, m) : ((((job == LapackEigenvalues.SVDxgesddJob.Selected) || (job == LapackEigenvalues.SVDxgesddJob.Overwritten)) && (m >= n)) ? n : 1);

                double* work = stackalloc double[1];

                _driver_dgesdd(ref jobz, ref m, ref n, null, ref lda, null, null, ref ldu, null, ref ldvt, work, ref lwork, null, out info);
                CheckForError(info, "dgesdd");

                return ((int)work[0]) + 1;
            }
        }

        /// <summary>Computes the singular value decomposition of a general rectangular matrix using a divide and conquer method.
        /// </summary>
        /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="m">The number of rows of matrix A.</param>
        /// <param name="n">The number of columns of matrix A.</param>
        /// <param name="a">The <paramref name="m"/>-by-<paramref name="n"/> matrix A provided column-by-column; will be perhaps overwritten on exit.</param>
        /// <param name="s">The singular values in descending order, must have at least min{<paramref name="m"/>, <paramref name="n"/>} elements (output).</param>
        /// <param name="u">The left singular vectors 'U', as specified by <paramref name="job"/>. See Lapack documentation for further details (http://www.netlib.org/lapack/index.html).</param>
        /// <param name="vt">The right singular vectors 'V^t' as specified by <paramref name="job"/>. See Lapack documentation for further details (http://www.netlib.org/lapack/index.html).</param>
        /// <param name="work">A workspace array.</param>
        /// <param name="iwork">A workspace array with dimension at least 8 * min(<paramref name="m"/>, <paramref name="n"/>).</param>
        public void driver_dgesdd(LapackEigenvalues.SVDxgesddJob job, int m, int n, double[] a, double[] s, double[] u, double[] vt, double[] work, int[] iwork)
        {
            int info;
            int lda = m;
            var jobz = GetJob(job);
            int lwork = work.Length;

            int ldu = (((job == LapackEigenvalues.SVDxgesddJob.All) || (job == LapackEigenvalues.SVDxgesddJob.Selected) || (job == LapackEigenvalues.SVDxgesddJob.Overwritten)) && (m < n)) ? m : 1;
            int ldvt = (job == LapackEigenvalues.SVDxgesddJob.Selected) ? Math.Min(n, m) : ((((job == LapackEigenvalues.SVDxgesddJob.Selected) || (job == LapackEigenvalues.SVDxgesddJob.Overwritten)) && (m >= n)) ? n : 1);

            _driver_dgesdd(ref jobz, ref m, ref n, a, ref lda, s, u, ref ldu, vt, ref ldvt, work, ref lwork, iwork, out info);
            CheckForError(info, "dgesdd");
        }

        /// <summary>Gets a optimal workspace array length for the <c>driver_zgesdd</c> function.
        /// </summary>
        /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="m">The number of rows of matrix A.</param>
        /// <param name="n">The number of columns of matrix A.</param>
        /// <returns>The optimal workspace array length.</returns>
        public int driver_zgesddQuery(LapackEigenvalues.SVDxgesddJob job, int m, int n)
        {
            unsafe
            {
                int info;

                int lwork = -1;
                int lda = m;
                var jobz = GetJob(job);
                int ldu = (((job == LapackEigenvalues.SVDxgesddJob.All) || (job == LapackEigenvalues.SVDxgesddJob.Selected) || (job == LapackEigenvalues.SVDxgesddJob.Overwritten)) && (m < n)) ? m : 1;
                int ldvt = (job == LapackEigenvalues.SVDxgesddJob.Selected) ? Math.Min(n, m) : ((((job == LapackEigenvalues.SVDxgesddJob.Selected) || (job == LapackEigenvalues.SVDxgesddJob.Overwritten)) && (m >= n)) ? n : 1);

                Complex* work = stackalloc Complex[1];

                _driver_zgesdd(ref jobz, ref m, ref n, null, ref lda, null, null, ref ldu, null, ref ldvt, work, ref lwork, null, null, out info);
                CheckForError(info, "zgesdd");

                return ((int)work[0].Real) + 1;
            }
        }

        /// <summary>Computes the singular value decomposition of a general rectangular matrix using a divide and conquer method.
        /// </summary>
        /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="m">The number of rows of matrix A.</param>
        /// <param name="n">The number of columns of matrix A.</param>
        /// <param name="a">The <paramref name="m"/>-by-<paramref name="n"/> matrix A provided column-by-column; will be perhaps overwritten on exit.</param>
        /// <param name="s">The singular values in descending order, must have at least min{<paramref name="m"/>, <paramref name="n"/>} elements (output).</param>
        /// <param name="u">The left singular vectors 'U', as specified by <paramref name="job"/>. See Lapack documentation for further details (http://www.netlib.org/lapack/index.html).</param>
        /// <param name="vt">The right singular vectors 'V^t' as specified by <paramref name="job"/>. See Lapack documentation for further details (http://www.netlib.org/lapack/index.html).</param>
        /// <param name="work">A workspace array.</param>
        /// <param name="rwork">A workspace array with dimension at least 5 * min(<paramref name="m"/>, <paramref name="n"/>) if <paramref name="job"/> equals <see cref="LapackEigenvalues.SVDxgesddJob.None"/>; min(m,n) * max(5*min(m,n) + 7, 2 * max(m,n) + 2 * min(m,n) +1) otherwise.</param>
        /// <param name="iwork">A workspace array with dimension at least 8 * min(<paramref name="m"/>, <paramref name="n"/>).</param>
        public void driver_zgesdd(LapackEigenvalues.SVDxgesddJob job, int m, int n, Complex[] a, double[] s, Complex[] u, Complex[] vt, Complex[] work, double[] rwork, int[] iwork)
        {
            int info;
            int lda = m;
            var jobz = GetJob(job);
            int lwork = work.Length;

            int ldu = (((job == LapackEigenvalues.SVDxgesddJob.All) || (job == LapackEigenvalues.SVDxgesddJob.Selected) || (job == LapackEigenvalues.SVDxgesddJob.Overwritten)) && (m < n)) ? m : 1;
            int ldvt = (job == LapackEigenvalues.SVDxgesddJob.Selected) ? Math.Min(n, m) : ((((job == LapackEigenvalues.SVDxgesddJob.Selected) || (job == LapackEigenvalues.SVDxgesddJob.Overwritten)) && (m >= n)) ? n : 1);

            _driver_zgesdd(ref jobz, ref m, ref n, a, ref lda, s, u, ref ldu, vt, ref ldvt, work, ref lwork, rwork, iwork, out info);
            CheckForError(info, "zgesdd");
        }

        /// <summary>Computes the singular value decomposition of a real matrix using a preconditioned Jacobi SVD method.
        /// </summary>
        /// <param name="joba">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="jobu">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="jobv">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="jobr">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="jobt">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="jobp">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="m">The number of rows of matrix A.</param>
        /// <param name="n">The number of columns of matrix A.</param>
        /// <param name="a">The <paramref name="m"/>-by-<paramref name="n"/> matrix A provided column-by-column; will be perhaps overwritten on exit.</param>
        /// <param name="sva">Contains singular values of A; dimension at least <paramref name="n"/> (output).</param>
        /// <param name="u">Contains the left singular vectors as specified by <paramref name="jobu"/> (output).</param>
        /// <param name="v">Contains the right singular vectors as specified by <paramref name="jobv"/> (output).</param>
        /// <param name="work">A workspace array which minimal length depends on the other parameters. See Lapack documentation for further details (http://www.netlib.org/lapack/index.html).</param>
        /// <param name="iwork">A workspace array with dimension at least max(3, <paramref name="m"/> + 3 * <paramref name="n"/>).</param>
        public void driver_dgejsv(LapackEigenvalues.SVDdgejsvJobA joba, LapackEigenvalues.SVDdgejsvJobU jobu, LapackEigenvalues.SVDdgejsvJobV jobv, LapackEigenvalues.SVDdgejsvJobR jobr, LapackEigenvalues.SVDdgejsvJobT jobt, LapackEigenvalues.SVDdgejsvJobP jobp, int m, int n, double[] a, double[] sva, double[] u, double[] v, double[] work, int[] iwork)
        {
            int info;
            int lda = m;
            int lwork = work.Length;
            int ldu = (jobu == LapackEigenvalues.SVDdgejsvJobU.None) ? 1 : m;
            int ldv = (jobv == LapackEigenvalues.SVDdgejsvJobV.None) ? 1 : n;

            var jobaZ = GetJob(joba);
            var jobuZ = GetJob(jobu);
            var jobvZ = GetJob(jobv);
            var jobrZ = GetJob(jobr);
            var jobtZ = GetJob(jobt);
            var jobpZ = GetJob(jobp);

            _driver_dgejsv(ref jobaZ, ref jobuZ, ref jobvZ, ref jobrZ, ref jobtZ, ref jobpZ, ref m, ref n, a, ref lda, sva, u, ref ldu, u, ref ldv, work, ref lwork, iwork, out info);
            CheckForError(info, "driver_dgejsv");
        }

        /// <summary>Computes the singular value decomposition of a real matrix using Jacobi plane rotations.
        /// </summary>
        /// <param name="joba">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="jobu">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="jobv">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="m">The number of rows of matrix A.</param>
        /// <param name="n">The number of columns of matrix A.</param>
        /// <param name="a">The <paramref name="m"/>-by-<paramref name="n"/> matrix A provided column-by-column; will be perhaps overwritten on exit.</param>
        /// <param name="sva">Contains singular values of A; dimension at least <paramref name="n"/> (output).</param>
        /// <param name="mv">The product of Jacobi rotations applied to the first mv rows of <paramref name="v"/>.</param>
        /// <param name="v">Contains the right singular vectors as specified by <paramref name="jobv"/> (output).</param>
        /// <param name="work">A workspace array with dimension at least max(4, <paramref name="m"/> + <paramref name="n"/>).</param>
        public void driver_dgesvj(LapackEigenvalues.SVDdgesvjJobA joba, LapackEigenvalues.SVDdgesvjJobU jobu, LapackEigenvalues.SVDdgesvjJobV jobv, int m, int n, double[] a, double[] sva, int mv, double[] v, double[] work)
        {
            int info;
            int lda = m;
            int ldv = 1;
            if (jobv == LapackEigenvalues.SVDdgesvjJobV.ComputeMatrix)
            {
                ldv = n;
            }
            else if (jobv == LapackEigenvalues.SVDdgesvjJobV.ApplyJacobiRotation)
            {
                ldv = Math.Max(1, mv);
            }
            int lwork = work.Length;
            var jobaZ = GetJob(joba);
            var jobuZ = GetJob(jobu);
            var jobvZ = GetJob(jobv);

            _driver_dgesvj(ref jobaZ, ref jobuZ, ref jobvZ, ref m, ref n, a, ref lda, sva, ref mv, v, ref ldv, work, ref lwork, out info);
            CheckForError(info, "driver_dgesvj");
        }

        /// <summary>Computes the generalized singular value decomposition of a pair of general rectangular matrices.
        /// </summary>
        /// <param name="jobu">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="jobv">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="jobq">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="m">The number of rows of matrix A.</param>
        /// <param name="n">The number of columns of matrix A.</param>
        /// <param name="p">The number of rows of matrix B.</param>
        /// <param name="k">Specify the dimension of the subblocks. The sum <paramref name="k"/> + <paramref name="l"/> is equal to the effective numerical rank of (A',B') (output).</param>
        /// <param name="l">Specify the dimension of the subblocks. The sum <paramref name="k"/> + <paramref name="l"/> is equal to the effective numerical rank of (A',B') (output).</param>
        /// <param name="a">The <paramref name="m"/>-by-<paramref name="n"/> matrix A provided column-by-column; will be perhaps overwritten on exit.</param>
        /// <param name="b">The <paramref name="p"/>-by-<paramref name="n"/> matrix B provided column-by-column; will be perhaps overwritten on exit.</param>
        /// <param name="alpha">Contain the generalized singular value paris of A and B (output).</param>
        /// <param name="beta">Contain the generalized singular value paris of A and B (output).</param>
        /// <param name="u">Contains the <paramref name="m"/>-by-<paramref name="m"/> orthogonal matrix U if specified by <paramref name="jobu"/> (output).</param>
        /// <param name="v">Contains the <paramref name="p"/>-by-<paramref name="p"/> orthogonal matrix V if specified by <paramref name="jobv"/> (output).</param>
        /// <param name="q">Contains the <paramref name="n"/>-by-<paramref name="n"/> orthogonal matrix Q if specified by <paramref name="jobq"/> (output).</param>
        /// <param name="work">A workspace array with dimension at least max(3 * n, m, p) + n.</param>
        /// <param name="iwork">A workspace array with dimension at least <paramref name="n"/>.</param>
        public void driver_dggsvd(LapackEigenvalues.SVDxggsvdJob jobu, LapackEigenvalues.SVDxggsvdJob jobv, LapackEigenvalues.SVDxggsvdJob jobq, int m, int n, int p, out int k, out int l, double[] a, double[] b, double[] alpha, double[] beta, double[] u, double[] v, double[] q, double[] work, int[] iwork)
        {
            int info;
            int lda = m;
            int ldb = p;
            int ldu = (jobu == LapackEigenvalues.SVDxggsvdJob.ComputeMatrix) ? m : 1;
            int ldv = (jobv == LapackEigenvalues.SVDxggsvdJob.ComputeMatrix) ? p : 1;
            int ldq = (jobq == LapackEigenvalues.SVDxggsvdJob.ComputeMatrix) ? n : 1;
            var jobuZ = GetJobu(jobu);
            var jobvZ = GetJobv(jobv);
            var jobqZ = GetJobq(jobq);

            _driver_dggsvd(ref jobuZ, ref jobvZ, ref jobqZ, ref m, ref n, ref p, out k, out l, a, ref lda, b, ref ldb, alpha, beta, u, ref ldu, v, ref ldv, q, ref ldq, work, iwork, out info);
            CheckForError(info, "driver_dggsvd");
        }

        /// <summary>Computes the generalized singular value decomposition of a pair of general rectangular matrices.
        /// </summary>
        /// <param name="jobu">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="jobv">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="jobq">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="m">The number of rows of matrix A.</param>
        /// <param name="n">The number of columns of matrix A.</param>
        /// <param name="p">The number of rows of matrix B.</param>
        /// <param name="k">Specify the dimension of the subblocks. The sum <paramref name="k"/> + <paramref name="l"/> is equal to the effective numerical rank of (A',B') (output).</param>
        /// <param name="l">Specify the dimension of the subblocks. The sum <paramref name="k"/> + <paramref name="l"/> is equal to the effective numerical rank of (A',B') (output).</param>
        /// <param name="a">The <paramref name="m"/>-by-<paramref name="n"/> matrix A provided column-by-column; will be perhaps overwritten on exit.</param>
        /// <param name="b">The <paramref name="p"/>-by-<paramref name="n"/> matrix B provided column-by-column; will be perhaps overwritten on exit.</param>
        /// <param name="alpha">Contain the generalized singular value paris of A and B (output).</param>
        /// <param name="beta">Contain the generalized singular value paris of A and B (output).</param>
        /// <param name="u">Contains the <paramref name="m"/>-by-<paramref name="m"/> orthogonal matrix U if specified by <paramref name="jobu"/> (output).</param>
        /// <param name="v">Contains the <paramref name="p"/>-by-<paramref name="p"/> orthogonal matrix V if specified by <paramref name="jobv"/> (output).</param>
        /// <param name="q">Contains the <paramref name="n"/>-by-<paramref name="n"/> orthogonal matrix Q if specified by <paramref name="jobq"/> (output).</param>
        /// <param name="work">A workspace array with dimension at least max(3 * n, m, p) + n.</param>
        /// <param name="rwork">A workspace array with dimension at least 2 * <paramref name="n"/>.</param>
        /// <param name="iwork">A workspace array with dimension at least <paramref name="n"/>.</param>
        public void driver_zggsvd(LapackEigenvalues.SVDxggsvdJob jobu, LapackEigenvalues.SVDxggsvdJob jobv, LapackEigenvalues.SVDxggsvdJob jobq, int m, int n, int p, out int k, out int l, Complex[] a, Complex[] b, double[] alpha, double[] beta, Complex[] u, Complex[] v, Complex[] q, Complex[] work, double[] rwork, int[] iwork)
        {
            int info;
            int lda = m;
            int ldb = p;
            int ldu = (jobu == LapackEigenvalues.SVDxggsvdJob.ComputeMatrix) ? m : 1;
            int ldv = (jobv == LapackEigenvalues.SVDxggsvdJob.ComputeMatrix) ? p : 1;
            int ldq = (jobq == LapackEigenvalues.SVDxggsvdJob.ComputeMatrix) ? n : 1;
            var jobuZ = GetJobu(jobu);
            var jobvZ = GetJobv(jobv);
            var jobqZ = GetJobq(jobq);

            _driver_zggsvd(ref jobuZ, ref jobvZ, ref jobqZ, ref m, ref n, ref p, out k, out l, a, ref lda, b, ref ldb, alpha, beta, u, ref ldu, v, ref ldv, q, ref ldq, work, rwork, iwork, out info);
            CheckForError(info, "driver_zggsvd");
        }
        #endregion

        #region private methods

        /// <summary>Gets the <see cref="System.Char"/> representation of a specific LAPACK job parameter.
        /// </summary>
        /// <param name="job">The specified LAPACK job parameter.</param>
        /// <returns>The <see cref="System.Char"/> representation of the specified LAPACK job parameter.</returns>
        private char GetJob(LapackEigenvalues.SVDrightSingularVectorsJob job)
        {

            switch (job)
            {
                case LapackEigenvalues.SVDrightSingularVectorsJob.All:
                    return 'A';
                case LapackEigenvalues.SVDrightSingularVectorsJob.InPlace:
                    return 'O';
                case LapackEigenvalues.SVDrightSingularVectorsJob.NoLeftSingularVectors:
                    return 'N';
                case LapackEigenvalues.SVDrightSingularVectorsJob.OutOfPlace:
                    return 'S';
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>Gets the <see cref="System.Char"/> representation of a specific LAPACK job parameter.
        /// </summary>
        /// <param name="job">The specified LAPACK job parameter.</param>
        /// <returns>The <see cref="System.Char"/> representation of the specified LAPACK job parameter.</returns>
        private char GetJob(LapackEigenvalues.SVDleftSingularVectorsJob job)
        {
            switch (job)
            {
                case LapackEigenvalues.SVDleftSingularVectorsJob.All:
                    return 'A';
                case LapackEigenvalues.SVDleftSingularVectorsJob.InPlace:
                    return 'O';
                case LapackEigenvalues.SVDleftSingularVectorsJob.NoLeftSingularVectors:
                    return 'N';
                case LapackEigenvalues.SVDleftSingularVectorsJob.OutOfPlace:
                    return 'S';
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>Gets the <see cref="System.Char"/> representation of a specific LAPACK job parameter.
        /// </summary>
        /// <param name="job">The specified LAPACK job parameter.</param>
        /// <returns>The <see cref="System.Char"/> representation of the specified LAPACK job parameter.</returns>
        private char GetJob(LapackEigenvalues.SVDxgbbrdJob job)
        {
            switch (job)
            {
                case LapackEigenvalues.SVDxgbbrdJob.None:
                    return 'N';
                case LapackEigenvalues.SVDxgbbrdJob.MatrixQ:
                    return 'Q';
                case LapackEigenvalues.SVDxgbbrdJob.MatrixP:
                    return 'P';
                case LapackEigenvalues.SVDxgbbrdJob.Both:
                    return 'B';
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>Gets the <see cref="System.Char"/> representation of a specific LAPACK job parameter.
        /// </summary>
        /// <param name="job">The specified LAPACK job parameter.</param>
        /// <returns>The <see cref="System.Char"/> representation of the specified LAPACK job parameter.</returns>
        private char GetJob(LapackEigenvalues.SVDxorgbrJob job)
        {
            switch (job)
            {
                case LapackEigenvalues.SVDxorgbrJob.MatrixQ:
                    return 'Q';
                case LapackEigenvalues.SVDxorgbrJob.MatrixP:
                    return 'P';
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>Gets the <see cref="System.Char"/> representation of a specific LAPACK job parameter.
        /// </summary>
        /// <param name="job">The specified LAPACK job parameter.</param>
        /// <returns>The <see cref="System.Char"/> representation of the specified LAPACK job parameter.</returns>
        private char GetJob(LapackEigenvalues.SVDxormbrJob job)
        {
            switch (job)
            {
                case LapackEigenvalues.SVDxormbrJob.ApplyP:
                    return 'P';

                case LapackEigenvalues.SVDxormbrJob.ApplyQ:
                    return 'Q';
                default: throw new NotImplementedException();

            }
        }

        /// <summary>Gets the <see cref="System.Char"/> representation of a specific LAPACK job parameter.
        /// </summary>
        /// <param name="job">The specified LAPACK job parameter.</param>
        /// <returns>The <see cref="System.Char"/> representation of the specified LAPACK job parameter.</returns>
        private char GetJob(LapackEigenvalues.SVDdbdsdcJob job)
        {
            switch (job)
            {
                case LapackEigenvalues.SVDdbdsdcJob.SingularValuesOnly:
                    return 'N';
                case LapackEigenvalues.SVDdbdsdcJob.SingularValuesAndSingularVectorsInCompactForm:
                    return 'P';
                case LapackEigenvalues.SVDdbdsdcJob.SingularValuesAndSingularVectors:
                    return 'I';

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>Gets the <see cref="System.Char"/> representation of a specific LAPACK job parameter.
        /// </summary>
        /// <param name="job">The specified LAPACK job parameter.</param>
        /// <returns>The <see cref="System.Char"/> representation of the specified LAPACK job parameter.</returns>
        private char GetJob(LapackEigenvalues.SVDxgesddJob job)
        {
            switch (job)
            {
                case LapackEigenvalues.SVDxgesddJob.All:
                    return 'A';
                case LapackEigenvalues.SVDxgesddJob.Selected:
                    return 'S';
                case LapackEigenvalues.SVDxgesddJob.Overwritten:
                    return 'O';
                case LapackEigenvalues.SVDxgesddJob.None:
                    return 'N';
                default:
                    throw new NotImplementedException();
            }
        }


        /// <summary>Gets the <see cref="System.Char"/> representation of a specific LAPACK job parameter.
        /// </summary>
        /// <param name="job">The specified LAPACK job parameter.</param>
        /// <returns>The <see cref="System.Char"/> representation of the specified LAPACK job parameter.</returns>
        private char GetJob(LapackEigenvalues.SVDdgejsvJobA job)
        {
            switch (job)
            {
                case LapackEigenvalues.SVDdgejsvJobA.C:
                    return 'C';
                case LapackEigenvalues.SVDdgejsvJobA.E:
                    return 'E';
                case LapackEigenvalues.SVDdgejsvJobA.F:
                    return 'F';
                case LapackEigenvalues.SVDdgejsvJobA.G:
                    return 'G';
                case LapackEigenvalues.SVDdgejsvJobA.A:
                    return 'A';
                case LapackEigenvalues.SVDdgejsvJobA.R:
                    return 'R';
                default: throw new NotImplementedException();
            }
        }

        /// <summary>Gets the <see cref="System.Char"/> representation of a specific LAPACK job parameter.
        /// </summary>
        /// <param name="job">The specified LAPACK job parameter.</param>
        /// <returns>The <see cref="System.Char"/> representation of the specified LAPACK job parameter.</returns>
        private char GetJob(LapackEigenvalues.SVDdgejsvJobU job)
        {
            switch (job)
            {
                case LapackEigenvalues.SVDdgejsvJobU.U:
                    return 'U';
                case LapackEigenvalues.SVDdgejsvJobU.F:
                    return 'F';
                case LapackEigenvalues.SVDdgejsvJobU.W:
                    return 'W';
                case LapackEigenvalues.SVDdgejsvJobU.None:
                    return 'N';
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>Gets the <see cref="System.Char"/> representation of a specific LAPACK job parameter.
        /// </summary>
        /// <param name="job">The specified LAPACK job parameter.</param>
        /// <returns>The <see cref="System.Char"/> representation of the specified LAPACK job parameter.</returns>
        private char GetJob(LapackEigenvalues.SVDdgejsvJobV job)
        {
            switch (job)
            {
                case LapackEigenvalues.SVDdgejsvJobV.V:
                    return 'V';
                case LapackEigenvalues.SVDdgejsvJobV.J:
                    return 'J';
                case LapackEigenvalues.SVDdgejsvJobV.W:
                    return 'W';
                case LapackEigenvalues.SVDdgejsvJobV.None:
                    return 'N';
                default: throw new NotImplementedException();
            }
        }

        /// <summary>Gets the <see cref="System.Char"/> representation of a specific LAPACK job parameter.
        /// </summary>
        /// <param name="job">The specified LAPACK job parameter.</param>
        /// <returns>The <see cref="System.Char"/> representation of the specified LAPACK job parameter.</returns>
        private char GetJob(LapackEigenvalues.SVDdgejsvJobR job)
        {
            switch (job)
            {
                case LapackEigenvalues.SVDdgejsvJobR.Restricted:
                    return 'R';
                case LapackEigenvalues.SVDdgejsvJobR.None:
                    return 'N';
                default: throw new NotImplementedException();
            }
        }

        /// <summary>Gets the <see cref="System.Char"/> representation of a specific LAPACK job parameter.
        /// </summary>
        /// <param name="job">The specified LAPACK job parameter.</param>
        /// <returns>The <see cref="System.Char"/> representation of the specified LAPACK job parameter.</returns>
        private char GetJob(LapackEigenvalues.SVDdgejsvJobT job)
        {
            switch (job)
            {
                case LapackEigenvalues.SVDdgejsvJobT.T:
                    return 'T';
                case LapackEigenvalues.SVDdgejsvJobT.None:
                    return 'N';
                default: throw new NotImplementedException();
            }
        }

        /// <summary>Gets the <see cref="System.Char"/> representation of a specific LAPACK job parameter.
        /// </summary>
        /// <param name="job">The specified LAPACK job parameter.</param>
        /// <returns>The <see cref="System.Char"/> representation of the specified LAPACK job parameter.</returns>
        private char GetJob(LapackEigenvalues.SVDdgejsvJobP job)
        {
            switch (job)
            {
                case LapackEigenvalues.SVDdgejsvJobP.Perturbation:
                    return 'P';
                case LapackEigenvalues.SVDdgejsvJobP.None:
                    return 'N';
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>Gets the <see cref="System.Char"/> representation of a specific LAPACK job parameter.
        /// </summary>
        /// <param name="job">The specified LAPACK job parameter.</param>
        /// <returns>The <see cref="System.Char"/> representation of the specified LAPACK job parameter.</returns>
        private char GetJob(LapackEigenvalues.SVDdgesvjJobA job)
        {
            switch (job)
            {
                case LapackEigenvalues.SVDdgesvjJobA.LowerTriangular:
                    return 'L';
                case LapackEigenvalues.SVDdgesvjJobA.UpperTriangular:
                    return 'U';
                case LapackEigenvalues.SVDdgesvjJobA.GeneralMatrix:
                    return 'G';
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>Gets the <see cref="System.Char"/> representation of a specific LAPACK job parameter.
        /// </summary>
        /// <param name="job">The specified LAPACK job parameter.</param>
        /// <returns>The <see cref="System.Char"/> representation of the specified LAPACK job parameter.</returns>
        private char GetJob(LapackEigenvalues.SVDdgesvjJobU job)
        {
            switch (job)
            {
                case LapackEigenvalues.SVDdgesvjJobU.LeftSingularVectors:
                    return 'U';
                case LapackEigenvalues.SVDdgesvjJobU.ControlLeftSingularVectors:
                    return 'C';
                case LapackEigenvalues.SVDdgesvjJobU.None:
                    return 'N';
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>Gets the <see cref="System.Char"/> representation of a specific LAPACK job parameter.
        /// </summary>
        /// <param name="job">The specified LAPACK job parameter.</param>
        /// <returns>The <see cref="System.Char"/> representation of the specified LAPACK job parameter.</returns>
        private char GetJob(LapackEigenvalues.SVDdgesvjJobV job)
        {
            switch (job)
            {
                case LapackEigenvalues.SVDdgesvjJobV.ComputeMatrix:
                    return 'V';
                case LapackEigenvalues.SVDdgesvjJobV.ApplyJacobiRotation:
                    return 'A';
                case LapackEigenvalues.SVDdgesvjJobV.None:
                    return 'N';
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>Gets the <see cref="System.Char"/> representation of a specific LAPACK job parameter.
        /// </summary>
        /// <param name="job">The specified LAPACK job parameter.</param>
        /// <returns>The <see cref="System.Char"/> representation of the specified LAPACK job parameter.</returns>
        private char GetJobu(LapackEigenvalues.SVDxggsvdJob job)
        {
            switch (job)
            {
                case LapackEigenvalues.SVDxggsvdJob.ComputeMatrix:
                    return 'U';
                case LapackEigenvalues.SVDxggsvdJob.None:
                    return 'N';

                default: throw new NotImplementedException();
            }
        }

        /// <summary>Gets the <see cref="System.Char"/> representation of a specific LAPACK job parameter.
        /// </summary>
        /// <param name="job">The specified LAPACK job parameter.</param>
        /// <returns>The <see cref="System.Char"/> representation of the specified LAPACK job parameter.</returns>
        private char GetJobv(LapackEigenvalues.SVDxggsvdJob job)
        {
            switch (job)
            {
                case LapackEigenvalues.SVDxggsvdJob.ComputeMatrix:
                    return 'V';
                case LapackEigenvalues.SVDxggsvdJob.None:
                    return 'N';

                default: throw new NotImplementedException();
            }
        }

        /// <summary>Gets the <see cref="System.Char"/> representation of a specific LAPACK job parameter.
        /// </summary>
        /// <param name="job">The specified LAPACK job parameter.</param>
        /// <returns>The <see cref="System.Char"/> representation of the specified LAPACK job parameter.</returns>
        private char GetJobq(LapackEigenvalues.SVDxggsvdJob job)
        {
            switch (job)
            {
                case LapackEigenvalues.SVDxggsvdJob.ComputeMatrix:
                    return 'Q';
                case LapackEigenvalues.SVDxggsvdJob.None:
                    return 'N';

                default: throw new NotImplementedException();
            }
        }
        #endregion
    }
}