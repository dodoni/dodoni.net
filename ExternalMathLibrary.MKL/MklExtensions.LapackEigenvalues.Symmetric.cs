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
        #region private function import

        [DllImport(sm_DllName, EntryPoint = "DSYRDB", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _dsyrdb(ref char jobz, ref char uplo, ref int n, ref int kd, [In, Out] double[] a, ref int lda, [In, Out] double[] d, [In, Out] double[] e, [In, Out] double[] tau, [In, Out] double[] z, ref int ldz, [In, Out] double[] work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DSYRDB", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern unsafe void _dsyrdb(ref char jobz, ref char uplo, ref int n, ref int kd, [In, Out] double[] a, ref int lda, [In, Out] double[] d, [In, Out] double[] e, [In, Out] double[] tau, [In, Out] double[] z, ref int ldz, [In, Out] double* work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZHERDB", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _zherdb(ref char jobz, ref char uplo, ref int n, ref int kd, [In, Out] Complex[] a, ref int lda, [In, Out] Complex[] d, [In, Out] Complex[] e, [In, Out] Complex[] tau, [In, Out] Complex[] z, ref int ldz, [In, Out] Complex[] work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZHERDB", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern unsafe void _zherdb(ref char jobz, ref char uplo, ref int n, ref int kd, [In, Out] Complex[] a, ref int lda, [In, Out] Complex[] d, [In, Out] Complex[] e, [In, Out] Complex[] tau, [In, Out] Complex[] z, ref int ldz, Complex* work, ref int lwork, out int info);
        #endregion

        #region public (static) methods

        /// <summary>Gets a optimal workspace array length for the <c>dsyrdb</c> function.
        /// </summary>
        /// <param name="symmetricEigenvalueProblems">The <see cref="LapackEigenvalues.ISymmetricEigenvalueProblems"/> object.</param>
        /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>
        /// <param name="n">The order of the matrix A.</param>
        /// <param name="kd">The bandwidth of the banded matrix B.</param>
        /// <returns>The optimal workspace array length.</returns>
        /// <remarks>The parameter <paramref name="triangularMatrixType"/> should not have an impact of the calculation of the optimal length of the workspace array.</remarks>
        public static int dsyrdbQuery(this LapackEigenvalues.ISymmetricEigenvalueProblems symmetricEigenvalueProblems, MklLapackEigenvalues.DsyrdbJob job, int n, int kd, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            var lwork = -1;
            var jobz = GetJob(job);
            var uplo = GetUplo(triangularMatrixType);

            unsafe
            {
                double* work = stackalloc double[1];
                int info;

                _dsyrdb(ref jobz, ref uplo, ref n, ref kd, null, ref n, null, null, null, null, ref n, work, ref lwork, out info);
                CheckForError(info, "dsyrdb");

                return ((int)work[0]) + 1;
            }
        }

        /// <summary>Reduces a real symmetric matrix to tridiagonal form with Successive Bandwidth Reduction approach, i.e. A = Q * T * Q' and optionally multiplies matrix Z by Q, or simply forms Q.
        /// </summary>
        /// <param name="symmetricEigenvalueProblems">The <see cref="LapackEigenvalues.ISymmetricEigenvalueProblems"/> object.</param>
        /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="n">The order of the matrix A.</param>
        /// <param name="kd">The bandwidth of the banded matrix B, for example 40 or 64.</param>
        /// <param name="a">The symmetric matrix provided column-by-column; depending on <paramref name="job"/> it will be overwritten by the banded matrix B and details of the orthogonal matrix Q to reduce A to B a specified by <paramref name="triangularMatrixType"/>.</param>
        /// <param name="d">The diagonal elements of the tridiagonal matrix; the array should have at least <paramref name="n"/> elements (output).</param>
        /// <param name="e">The off-diagonal elements of the tridiagonal matrix; the array should have at least <paramref name="n"/> - 1 elements (output).</param>
        /// <param name="tau">Further details of the orthogonal matrix; the array should have at least <paramref name="n"/> - <paramref name="kd"/> - 1  elements (output).</param>
        /// <param name="z">The matrix Z if <paramref name="job"/> indicates to take into account this parameter, if referenced it should have at least <paramref name="n"/> * <paramref name="n"/> elements and will be overwritten on exit by Z * Q.</param>
        /// <param name="work">A workspace array with at least (2 * <paramref name="kd"/> + 1) * <paramref name="n"/> + <paramref name="kd"/> elements.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>
        public static void dsyrdb(this LapackEigenvalues.ISymmetricEigenvalueProblems symmetricEigenvalueProblems, MklLapackEigenvalues.DsyrdbJob job, int n, int kd, double[] a, double[] d, double[] e, double[] tau, double[] z, double[] work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var jobz = GetJob(job);
            int lwork = work.Length;
            var uplo = GetUplo(triangularMatrixType);

            _dsyrdb(ref jobz, ref uplo, ref n, ref kd, a, ref n, d, e, tau, z, ref n, work, ref lwork, out info);
            CheckForError(info, "dsyrdb");
        }

        /// <summary>Gets a optimal workspace array length for the <c>zherdb</c> function.
        /// </summary>
        /// <param name="symmetricEigenvalueProblems">The <see cref="LapackEigenvalues.ISymmetricEigenvalueProblems"/> object.</param>
        /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="n">The order of the matrix A.</param>
        /// <param name="kd">The bandwidth of the banded matrix B.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>
        /// <returns>The optimal workspace array length.</returns>
        /// <remarks>The parameter <paramref name="triangularMatrixType"/> should not have an impact of the calculation of the optimal length of the workspace array.</remarks>
        public static int zherdbQuery(this LapackEigenvalues.ISymmetricEigenvalueProblems symmetricEigenvalueProblems, MklLapackEigenvalues.ZherdbJob job, int n, int kd, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            var lwork = -1;
            var jobz = GetJob(job);
            var uplo = GetUplo(triangularMatrixType);

            unsafe
            {
                Complex* work = stackalloc Complex[1];
                int info;

                _zherdb(ref jobz, ref uplo, ref n, ref kd, null, ref n, null, null, null, null, ref n, work, ref lwork, out info);
                CheckForError(info, "zherdb");

                return ((int)work[0].Real) + 1;
            }
        }

        /// <summary>Reduces a complex Hermitian matrix to tridiagonal form with Successive Bandwidth Reduction approach, i.e. A = Q * T * Q' and optionally multiplies matrix Z by Q, or simply forms Q.
        /// </summary>
        /// <param name="symmetricEigenvalueProblems">The <see cref="LapackEigenvalues.ISymmetricEigenvalueProblems"/> object.</param>
        /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="n">The order of the matrix A.</param>
        /// <param name="kd">The bandwidth of the banded matrix B, for example 40 or 64.</param>
        /// <param name="a">The symmetric matrix provided column-by-column; depending on <paramref name="job"/> it will be overwritten by the banded matrix B and details of the orthogonal matrix Q to reduce A to B a specified by <paramref name="triangularMatrixType"/>.</param>
        /// <param name="d">The diagonal elements of the tridiagonal matrix; the array should have at least <paramref name="n"/> elements (output).</param>
        /// <param name="e">The off-diagonal elements of the tridiagonal matrix; the array should have at least <paramref name="n"/> - 1 elements (output).</param>
        /// <param name="tau">Further details of the orthogonal matrix; the array should have at least <paramref name="n"/> - <paramref name="kd"/> - 1  elements (output).</param>
        /// <param name="z">The matrix Z if <paramref name="job"/> indicates to take into account this parameter, if referenced it should have at least <paramref name="n"/> * <paramref name="n"/> elements and will be overwritten on exit by Z * Q.</param>
        /// <param name="work">A workspace array with at least (2 * <paramref name="kd"/> + 1) * <paramref name="n"/> + <paramref name="kd"/> elements.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>
        public static void zherdb(this LapackEigenvalues.ISymmetricEigenvalueProblems symmetricEigenvalueProblems, MklLapackEigenvalues.ZherdbJob job, int n, int kd, Complex[] a, Complex[] d, Complex[] e, Complex[] tau, Complex[] z, Complex[] work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var jobz = GetJob(job);
            int lwork = work.Length;
            var uplo = GetUplo(triangularMatrixType);

            _zherdb(ref jobz, ref uplo, ref n, ref kd, a, ref n, d, e, tau, z, ref n, work, ref lwork, out info);
            CheckForError(info, "zherdb");
        }
        #endregion

        #region private (static) methods

        /// <summary>Gets the <see cref="System.Char"/> representation of a specific LAPACK job parameter.
        /// </summary>
        /// <param name="job">The specified LAPACK job parameter.</param>
        /// <returns>The <see cref="System.Char"/> representation of the specified LAPACK job parameter.</returns>
        private static char GetJob(MklLapackEigenvalues.DsyrdbJob job)
        {
            switch (job)
            {
                case MklLapackEigenvalues.DsyrdbJob.None:
                    return 'N';
                case MklLapackEigenvalues.DsyrdbJob.U:
                    return 'U';
                case MklLapackEigenvalues.DsyrdbJob.V:
                    return 'V';
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>Gets the <see cref="System.Char"/> representation of a specific LAPACK job parameter.
        /// </summary>
        /// <param name="job">The specified LAPACK job parameter.</param>
        /// <returns>The <see cref="System.Char"/> representation of the specified LAPACK job parameter.</returns>
        private static char GetJob(MklLapackEigenvalues.ZherdbJob job)
        {
            switch (job)
            {
                case MklLapackEigenvalues.ZherdbJob.None:
                    return 'N';
                case MklLapackEigenvalues.ZherdbJob.U:
                    return 'U';
                case MklLapackEigenvalues.ZherdbJob.V:
                    return 'V';
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion
    }
}