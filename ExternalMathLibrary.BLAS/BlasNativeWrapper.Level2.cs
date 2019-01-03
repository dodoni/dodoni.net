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
using System.Security;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Dodoni.MathLibrary.Basics.LowLevel.Native
{
    public partial class BlasNativeWrapper
    {
        /// <summary>The wrapper for BLAS level 2 methods (Fortran interface). See http://www.netlib.org/blas for further information.
        /// </summary>
        protected internal class Level2BLAS : ILevel2BLAS
        {
            #region private function import

#if LOWER_CASE_UNDERSCORE

            #region double precision methods

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "dgemv_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dgemv(ref char transA, ref int m, ref int n, ref double alpha, IntPtr a, ref int lda, IntPtr x, ref int incX, ref double beta, IntPtr y, ref int incY);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "dgbmv_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dgbmv(ref char transA, ref int m, ref int n, ref int kl, ref int ku, ref double alpha, IntPtr a, ref int lda, IntPtr x, ref int incX, ref double beta, IntPtr y, ref int incY);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "dger_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dger(ref int m, ref int n, ref double alpha, IntPtr x, ref int incx, IntPtr y, ref int incy, IntPtr a, ref int lda);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "dsbmv_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dsbmv(ref char uplo, ref int n, ref int k, ref double alpha, IntPtr a, ref int lda, IntPtr x, ref int incx, ref double beta, IntPtr y, ref int incy);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "dspmv_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dspmv(ref char uplo, ref int n, ref double alpha, IntPtr ap, IntPtr x, ref int incx, ref double beta, IntPtr y, ref int incy);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "dspr_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dspr(ref char uplo, ref int n, ref double alpha, IntPtr x, ref int incx, IntPtr ap);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "dspr2_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dspr2(ref char uplo, ref int n, ref double alpha, IntPtr x, ref int incx, IntPtr y, ref int incy, IntPtr ap);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "dsymv_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dsymv(ref char uplo, ref int n, ref double alpha, IntPtr a, ref int lda, IntPtr x, ref int incx, ref double beta, IntPtr y, ref int incy);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "dsyr_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dsyr(ref char uplo, ref int n, ref double alpha, IntPtr x, ref int incx, IntPtr a, ref int lda);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "dsyr2_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dsyr2(ref char uplo, ref int n, ref double alpha, IntPtr x, ref int incx, IntPtr y, ref int incy, IntPtr a, ref int lda);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "dtbmv_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dtbmv(ref char uplo, ref char trans, ref char diag, ref int n, ref int k, IntPtr a, ref int lda, IntPtr x, ref int incx);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "dtbsv_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dtbsv(ref char uplo, ref char trans, ref char diag, ref int n, ref int k, IntPtr a, ref int lda, IntPtr x, ref int incx);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "dtpmv_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dtpmv(ref char uplo, ref char trans, ref char diag, ref int n, IntPtr ap, IntPtr x, ref int incx);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "dtpsv_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dtpsv(ref char uplo, ref char trans, ref char diag, ref int n, IntPtr ap, IntPtr x, ref int incx);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "dtrmv_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dtrmv(ref char uplo, ref char trans, ref char diag, ref int n, IntPtr a, ref int lda, IntPtr x, ref int incx);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "dtrsv_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dtrsv(ref char uplo, ref char trans, ref char diag, ref int n, IntPtr a, ref int lda, IntPtr x, ref int incx);
            #endregion

            #region (double precision) complex methods

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "zgemv_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zgemv(ref char transA, ref int m, ref int n, ref Complex alpha, IntPtr a, ref int lda, IntPtr x, ref int incX, ref Complex beta, IntPtr y, ref int incY);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "zgbmv_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zgbmv(ref char transA, ref int m, ref int n, ref int kl, ref int ku, ref Complex alpha, IntPtr a, ref int lda, IntPtr x, ref int incX, ref Complex beta, IntPtr y, ref int incY);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "zgerc_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zgerc(ref int m, ref int n, ref Complex alpha, IntPtr x, ref int incx, IntPtr y, ref int incy, IntPtr a, ref int lda);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "zgeru_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zgeru(ref int m, ref int n, ref Complex alpha, IntPtr x, ref int incx, IntPtr y, ref int incy, IntPtr a, ref int lda);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "zhbmv_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zhbmv(ref char uplo, ref int n, ref int k, ref Complex alpha, IntPtr a, ref int lda, IntPtr x, ref int incx, ref Complex beta, IntPtr y, ref int incy);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "zhemv_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zhemv(ref char uplo, ref int n, ref Complex alpha, IntPtr a, ref int lda, IntPtr x, ref int incx, ref Complex beta, IntPtr y, ref int incy);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "zher_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zher(ref char uplo, ref int n, ref double alpha, IntPtr x, ref int incx, IntPtr a, ref int lda);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "zher2_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zher2(ref char uplo, ref int n, ref Complex alpha, IntPtr x, ref int incx, IntPtr y, ref int incy, IntPtr a, ref int lda);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "zhpmv_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zhpmv(ref char uplo, ref int n, ref Complex alpha, IntPtr ap, IntPtr x, ref int incx, ref Complex beta, IntPtr y, ref int incy);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "zhpr_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zhpr(ref char uplo, ref int n, ref double alpha, IntPtr x, ref int incx, IntPtr ap);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "zhpr2_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zhpr2(ref char uplo, ref int n, ref Complex alpha, IntPtr x, ref int incx, IntPtr y, ref int incy, IntPtr ap);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "ztbmv_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _ztbmv(ref char uplo, ref char trans, ref char diag, ref int n, ref int k, IntPtr a, ref int lda, IntPtr x, ref int incx);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "ztbsv_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _ztbsv(ref char uplo, ref char trans, ref char diag, ref int n, ref int k, IntPtr a, ref int lda, IntPtr x, ref int incx);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "ztpmv_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _ztpmv(ref char uplo, ref char trans, ref char diag, ref int n, IntPtr ap, IntPtr x, ref int incx);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "ztpsv_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _ztpsv(ref char uplo, ref char trans, ref char diag, ref int n, IntPtr ap, IntPtr x, ref int incx);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "ztrmv_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _ztrmv(ref char uplo, ref char trans, ref char diag, ref int n, IntPtr a, ref int lda, IntPtr x, ref int incx);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "ztrsv_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _ztrsv(ref char uplo, ref char trans, ref char diag, ref int n, IntPtr a, ref int lda, IntPtr x, ref int incx);
            #endregion
#else
            #region double precision methods

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "DGEMV", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dgemv(ref char transA, ref int m, ref int n, ref double alpha, IntPtr a, ref int lda, IntPtr x, ref int incX, ref double beta, IntPtr y, ref int incY);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "DGBMV", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dgbmv(ref char transA, ref int m, ref int n, ref int kl, ref int ku, ref double alpha, IntPtr a, ref int lda, IntPtr x, ref int incX, ref double beta, IntPtr y, ref int incY);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "DGER", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dger(ref int m, ref int n, ref double alpha, IntPtr x, ref int incx, IntPtr y, ref int incy, IntPtr a, ref int lda);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "DSBMV", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dsbmv(ref char uplo, ref int n, ref int k, ref double alpha, IntPtr a, ref int lda, IntPtr x, ref int incx, ref double beta, IntPtr y, ref int incy);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "DSPMV", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dspmv(ref char uplo, ref int n, ref double alpha, IntPtr ap, IntPtr x, ref int incx, ref double beta, IntPtr y, ref int incy);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "DSPR", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dspr(ref char uplo, ref int n, ref double alpha, IntPtr x, ref int incx, IntPtr ap);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "DSPR2", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dspr2(ref char uplo, ref int n, ref double alpha, IntPtr x, ref int incx, IntPtr y, ref int incy, IntPtr ap);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "DSYMV", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dsymv(ref char uplo, ref int n, ref double alpha, IntPtr a, ref int lda, IntPtr x, ref int incx, ref double beta, IntPtr y, ref int incy);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "DSYR", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dsyr(ref char uplo, ref int n, ref double alpha, IntPtr x, ref int incx, IntPtr a, ref int lda);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "DSYR2", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dsyr2(ref char uplo, ref int n, ref double alpha, IntPtr x, ref int incx, IntPtr y, ref int incy, IntPtr a, ref int lda);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "DTBMV", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dtbmv(ref char uplo, ref char trans, ref char diag, ref int n, ref int k, IntPtr a, ref int lda, IntPtr x, ref int incx);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "DTBSV", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dtbsv(ref char uplo, ref char trans, ref char diag, ref int n, ref int k, IntPtr a, ref int lda, IntPtr x, ref int incx);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "DTPMV", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dtpmv(ref char uplo, ref char trans, ref char diag, ref int n, IntPtr ap, IntPtr x, ref int incx);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "DTPSV", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dtpsv(ref char uplo, ref char trans, ref char diag, ref int n, IntPtr ap, IntPtr x, ref int incx);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "DTRMV", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dtrmv(ref char uplo, ref char trans, ref char diag, ref int n, IntPtr a, ref int lda, IntPtr x, ref int incx);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "DTRSV", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dtrsv(ref char uplo, ref char trans, ref char diag, ref int n, IntPtr a, ref int lda, IntPtr x, ref int incx);
            #endregion

            #region (double precision) complex methods

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "ZGEMV", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zgemv(ref char transA, ref int m, ref int n, ref Complex alpha, IntPtr a, ref int lda, IntPtr x, ref int incX, ref Complex beta, IntPtr y, ref int incY);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "ZGBMV", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zgbmv(ref char transA, ref int m, ref int n, ref int kl, ref int ku, ref Complex alpha, IntPtr a, ref int lda, IntPtr x, ref int incX, ref Complex beta, IntPtr y, ref int incY);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "ZGERC", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zgerc(ref int m, ref int n, ref Complex alpha, IntPtr x, ref int incx, IntPtr y, ref int incy, IntPtr a, ref int lda);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "ZGERU", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zgeru(ref int m, ref int n, ref Complex alpha, IntPtr x, ref int incx, IntPtr y, ref int incy, IntPtr a, ref int lda);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "ZHBMV", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zhbmv(ref char uplo, ref int n, ref int k, ref Complex alpha, IntPtr a, ref int lda, IntPtr x, ref int incx, ref Complex beta, IntPtr y, ref int incy);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "ZHEMV", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zhemv(ref char uplo, ref int n, ref Complex alpha, IntPtr a, ref int lda, IntPtr x, ref int incx, ref Complex beta, IntPtr y, ref int incy);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "ZHER", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zher(ref char uplo, ref int n, ref double alpha, IntPtr x, ref int incx, IntPtr a, ref int lda);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "ZHER2", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zher2(ref char uplo, ref int n, ref Complex alpha, IntPtr x, ref int incx, IntPtr y, ref int incy, IntPtr a, ref int lda);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "ZHPMV", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zhpmv(ref char uplo, ref int n, ref Complex alpha, IntPtr ap, IntPtr x, ref int incx, ref Complex beta, IntPtr y, ref int incy);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "ZHPR", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zhpr(ref char uplo, ref int n, ref double alpha, IntPtr x, ref int incx, IntPtr ap);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "ZHPR2", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zhpr2(ref char uplo, ref int n, ref Complex alpha, IntPtr x, ref int incx, IntPtr y, ref int incy, IntPtr ap);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "ZTBMV", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _ztbmv(ref char uplo, ref char trans, ref char diag, ref int n, ref int k, IntPtr a, ref int lda, IntPtr x, ref int incx);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "ZTBSV", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _ztbsv(ref char uplo, ref char trans, ref char diag, ref int n, ref int k, IntPtr a, ref int lda, IntPtr x, ref int incx);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "ZTPMV", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _ztpmv(ref char uplo, ref char trans, ref char diag, ref int n, IntPtr ap, IntPtr x, ref int incx);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "ZTPSV", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _ztpsv(ref char uplo, ref char trans, ref char diag, ref int n, IntPtr ap, IntPtr x, ref int incx);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "ZTRMV", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _ztrmv(ref char uplo, ref char trans, ref char diag, ref int n, IntPtr a, ref int lda, IntPtr x, ref int incx);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "ZTRSV", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _ztrsv(ref char uplo, ref char trans, ref char diag, ref int n, IntPtr a, ref int lda, IntPtr x, ref int incx);
            #endregion

#endif
            #endregion

            #region public constructors

            /// <summary>Initializes a new instance of the <see cref="Level2BLAS"/> class.
            /// </summary>
            public Level2BLAS()
            {
            }
            #endregion

            #region ILevel2BLAS Members

            #region double precision methods

            /// <summary>Computes a matrix-vector product using a general band matrix, i.e. y := \alpha * op(A) * x + \beta * y, where op(A) = A or op(A) = A^t.
            /// </summary>
            /// <param name="m">The number of rows of matrix A.</param>
            /// <param name="n">The number of columns of matrix A.</param>
            /// <param name="kl">The number of sub-diagonals of matrix A.</param>
            /// <param name="ku">The number of super-diagonals of matrix A.</param>
            /// <param name="alpha">The scalar factor \alpha.</param>
            /// <param name="a">The general band matrix A of dimension (<paramref name="lda"/>, <paramref name="n"/>). The leading (<paramref name="kl"/> + <paramref name="ku"/> + 1) by <paramref name="n"/> part
            /// must contain the matrix of coefficients (supplied column-by-column) with the leading diagonal of the matrix
            /// in row (<paramref name="ku"/> + 1) of the array, the first super-diagonal starting at position 2 in row ku,
            /// the first sub-diagonal starting at position 1 in row (ku + 2), etc.
            /// The following program segment transfers a band matrix from conventional full matrix storage to band storage:
            /// <code>
            /// for j = 0 to n-1
            /// k = ku - j
            /// for i = max(0, j-ku) to min(m-1, j+kl-1)
            /// a(k+i, j) = matrix(i,j)
            /// end
            /// end
            /// </code></param>
            /// <param name="x">The vector x with at least 1+(<paramref name="n"/>-1) * |<paramref name="incX"/>| elements if 'op(A)=A'; 1+(<paramref name="m"/>-1) *|<paramref name="incX"/>| otherwise.</param>
            /// <param name="beta">The scalar factor \beta.</param>
            /// <param name="y">The vector y with at least 1+(<paramref name="m"/>-1) *|<paramref name="incY"/>| elements if 'op(A)=A'; 1+(<paramref name="n"/>-1) * |<paramref name="incY"/>| otherwise (input/output).</param>
            /// <param name="lda">The leading dimension, must be at least <paramref name="kl"/> + <paramref name="ku"/> +1.</param>
            /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
            /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
            /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
            public void dgbmv(int m, int n, int kl, int ku, double alpha, double[] a, double[] x, double beta, double[] y, int lda, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1, int incY = 1)
            {
                unsafe
                {
                    fixed (double* ptrA = a, ptrX = x, ptrY = y)
                    {
                        char trans = (transpose == BLAS.MatrixTransposeState.NoTranspose) ? 'n' : 't';
                        _dgbmv(ref trans, ref m, ref n, ref kl, ref ku, ref alpha, (IntPtr)ptrA, ref lda, (IntPtr)ptrX, ref incX, ref beta, (IntPtr)ptrY, ref incY);
                    }
                }
            }

            /// <summary>Computes a matrix-vector product for a general matrix, i.e. y = \alpha * op(A)*x + \beta*y, where \op(A) = A or \op(A)=A^t.
            /// </summary>
            /// <param name="m">The number of rows of matrix A.</param>
            /// <param name="n">The number of columns of matrix A.</param>
            /// <param name="alpha">The scalar \alpha.</param>
            /// <param name="a">The matrix A of dimension (<paramref name="lda"/>, <paramref name="n"/>) supplied column-by-column.</param>
            /// <param name="x">The vector x with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incX"/>| elements if 'op(A)=A'; 1 + (<paramref name="m"/>-1) * |<paramref name="incY"/>| elements otherwise.</param>
            /// <param name="beta">The scalar \beta.</param>
            /// <param name="y">The vector y with at least 1 + (<paramref name="m"/>-1) * |<paramref name="incY"/>| elements if 'op(A)=A'; 1 + (<paramref name="n"/>-1) * | <paramref name="incX"/>| otherwise (input/output).</param>
            /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="m"/>).</param>
            /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
            /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
            /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
            public void dgemv(int m, int n, double alpha, double[] a, double[] x, double beta, double[] y, int lda, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1, int incY = 1)
            {
                unsafe
                {
                    fixed (double* ptrA = a, ptrX = x, ptrY = y)
                    {
                        char trans = (transpose == BLAS.MatrixTransposeState.NoTranspose) ? 'n' : 't';
                        _dgemv(ref trans, ref m, ref n, ref alpha, (IntPtr)ptrA, ref lda, (IntPtr)ptrX, ref incX, ref beta, (IntPtr)ptrY, ref incY);
                    }
                }
            }

            /// <summary>Performs a rank-1 update of a general matrix, i.e. A := \alpha * x * y^t + A.
            /// </summary>
            /// <param name="m">The number of rows of matrix A.</param>
            /// <param name="n">The number of columns of matrix A.</param>
            /// <param name="alpha">The scalar \alpha.</param>
            /// <param name="x">The vector x with at least 1 + (<paramref name="m"/>-1) * |<paramref name="incX"/>| elements.</param>
            /// <param name="y">The vector y with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incY"/>| elements.</param>
            /// <param name="a">The matrix A of dimension (<paramref name="lda"/>, <paramref name="n"/>) supplied column-by-column (input/output).</param>
            /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="m"/>).</param>
            /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
            /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
            public void dger(int m, int n, double alpha, double[] x, double[] y, double[] a, int lda, int incX = 1, int incY = 1)
            {
                unsafe
                {
                    fixed (double* ptrA = a, ptrX = x, ptrY = y)
                    {
                        _dger(ref m, ref n, ref alpha, (IntPtr)ptrX, ref incX, (IntPtr)ptrY, ref incY, (IntPtr)ptrA, ref lda);
                    }
                }
            }

            /// <summary>Computes a matrix-vector product using a symmetric band matrix, i.e. y:= \alpha * A * x + \beta * y.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="k">The number of super-diagonals of matrix A.</param>
            /// <param name="alpha">The scalar \alpha.</param>
            /// <param name="a">The matrix A of dimension (<paramref name="lda"/>, <paramref name="n"/>) supplied column-by-column.</param>
            /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
            /// <param name="beta">The scalar \beta.</param>
            /// <param name="y">The vector y with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incY"/> | elements (input/output).</param>
            /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least (1 + <paramref name="k"/>).</param>
            /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
            /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
            /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
            public void dsbmv(int n, int k, double alpha, double[] a, double[] x, double beta, double[] y, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1, int incY = 1)
            {
                unsafe
                {
                    fixed (double* ptrA = a, ptrX = x, ptrY = y)
                    {
                        char uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? 'u' : 'l';
                        _dsbmv(ref uplo, ref n, ref k, ref alpha, (IntPtr)ptrA, ref lda, (IntPtr)ptrX, ref incX, ref beta, (IntPtr)ptrY, ref incY);
                    }
                }
            }

            /// <summary>Computes a matrix-vector product using a symmetric packed matrix, i.e. y := \alpha * A * x + \beta * y.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="alpha">The scalar \alpha.</param>
            /// <param name="aPacked">The symmetric packed matrix A with dimension at least (<paramref name="n"/> * (<paramref name="n"/> + 1) ) / 2, i.e. in the upper triangular representation: aPacked[0] = a[0,0], aPacked[1] = a[0,1], aPacked[2] = a[1,1]; otherwise aPacked[0] = a[0,0], aPacked[1] = a[1,0], aPacked[2] = a[2,0] etc.</param>
            /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
            /// <param name="beta">The scalar \beta.</param>
            /// <param name="y">The vector y with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incY"/> | elements (input/output).</param>
            /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
            /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
            /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
            public void dspmv(int n, double alpha, double[] aPacked, double[] x, double beta, double[] y, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1, int incY = 1)
            {
                unsafe
                {
                    fixed (double* ptrA = aPacked, ptrX = x, ptrY = y)
                    {
                        char uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? 'u' : 'l';
                        _dspmv(ref uplo, ref n, ref alpha, (IntPtr)ptrA, (IntPtr)ptrX, ref incX, ref beta, (IntPtr)ptrY, ref incY);
                    }
                }
            }

            /// <summary>Performs a rank-1 update of a symmetric packed matrix, i.e. A := \alpha * x * x^t + A.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="alpha">The scalar \alpha.</param>
            /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
            /// <param name="aPacked">The symmetric packed matrix A with dimension at least (<paramref name="n"/> * (<paramref name="n"/> + 1) ) / 2.</param>
            /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
            /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
            public void dspr(int n, double alpha, double[] x, double[] aPacked, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1)
            {
                unsafe
                {
                    fixed (double* ptrA = aPacked, ptrX = x)
                    {
                        char uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? 'u' : 'l';
                        _dspr(ref uplo, ref n, ref alpha, (IntPtr)ptrX, ref incX, (IntPtr)ptrA);
                    }
                }
            }

            /// <summary>Performs a rank-2 update of a symmetric packed matrix, i.e. A := \alpha * x * y^t + \alpha * y * x^t + A.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="alpha">The scalar \alpha.</param>
            /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
            /// <param name="y">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incY"/> | elements.</param>
            /// <param name="aPacked">The symmetric packed matrix A with dimension at least (<paramref name="n"/> * (<paramref name="n"/> + 1) ) / 2.</param>
            /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
            /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
            /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
            public void dspr2(int n, double alpha, double[] x, double[] y, double[] aPacked, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1, int incY = 1)
            {
                unsafe
                {
                    fixed (double* ptrA = aPacked, ptrX = x, ptrY = y)
                    {
                        char uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? 'u' : 'l';
                        _dspr2(ref uplo, ref n, ref alpha, (IntPtr)ptrX, ref incX, (IntPtr)ptrY, ref incY, (IntPtr)ptrA);
                    }
                }
            }

            /// <summary>Computes a matrix-vector product for a symmetric matrix, i.e. y := \alpha * A * x + \beta * y.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="alpha">The scalar \alpha.</param>
            /// <param name="a">The symmetric matrix A of dimension (<paramref name="lda"/>, <paramref name="n"/>) supplied column-by-column.</param>
            /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
            /// <param name="beta">The scalar \beta.</param>
            /// <param name="y">The vector y with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incY"/> | elements (input/output).</param>
            /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1, <paramref name="n"/>).</param>
            /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
            /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
            /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
            public void dsymv(int n, double alpha, double[] a, double[] x, double beta, double[] y, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1, int incY = 1)
            {
                unsafe
                {
                    fixed (double* ptrA = a, ptrX = x, ptrY = y)
                    {
                        char uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? 'u' : 'l';
                        _dsymv(ref uplo, ref n, ref alpha, (IntPtr)ptrA, ref lda, (IntPtr)ptrX, ref incX, ref beta, (IntPtr)ptrY, ref incY);
                    }
                }
            }

            /// <summary>Performs a rank-1 update of a symmetric matrix, i.e. A := \alpha * x * x^t + A.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="alpha">The scalar \alpha.</param>
            /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
            /// <param name="a">The symmetric matrix A of dimension (<paramref name="lda"/>, <paramref name="n"/>) supplied column-by-column (input/output).</param>
            /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1, <paramref name="n"/>).</param>
            /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
            /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
            public void dsyr(int n, double alpha, double[] x, double[] a, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1)
            {
                unsafe
                {
                    fixed (double* ptrA = a, ptrX = x)
                    {
                        char uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? 'u' : 'l';
                        _dsyr(ref uplo, ref n, ref alpha, (IntPtr)ptrX, ref incX, (IntPtr)ptrA, ref lda);
                    }
                }
            }

            /// <summary>Performs a rank-2 update of a symmetric matrix, i.e. A := \alpha * x * y^t + \alpha * y * x^t + A.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="alpha">The scalar \alpha.</param>
            /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
            /// <param name="y">The vector y with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incY"/> | elements.</param>
            /// <param name="a">The symmetric matrix A of dimension (<paramref name="lda"/>, <paramref name="n"/>) supplied column-by-column (input/output).</param>
            /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1, <paramref name="n"/>).</param>
            /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
            /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
            /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
            public void dsyr2(int n, double alpha, double[] x, double[] y, double[] a, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1, int incY = 1)
            {
                unsafe
                {
                    fixed (double* ptrA = a, ptrX = x, ptrY = y)
                    {
                        char uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? 'u' : 'l';
                        _dsyr2(ref uplo, ref n, ref alpha, (IntPtr)ptrX, ref incX, (IntPtr)ptrY, ref incY, (IntPtr)ptrA, ref lda);
                    }
                }
            }

            /// <summary>Computes a matrix-vector product using a triangular band matrix, i.e. x := op(A) * x, where op(A) = A or op(A) = A^t.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="k">The number of super-diagonales of A if the matrix A is provided in its upper triangular representation; the number of sub-diagonals otherwise.</param>
            /// <param name="a">The triangular band matrix with dimension (<paramref name="lda"/>, <paramref name="n"/>).</param>
            /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
            /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least (1  + <paramref name="k"/>).</param>
            /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
            /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
            /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
            /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
            public void dtbmv(int n, int k, double[] a, double[] x, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1)
            {
                unsafe
                {
                    fixed (double* ptrA = a, ptrX = x)
                    {
                        char uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? 'u' : 'l';
                        char trans = (transpose == BLAS.MatrixTransposeState.NoTranspose) ? 'n' : 't';
                        char diag = (isUnitTriangular == true) ? 'u' : 'n';
                        _dtbmv(ref uplo, ref trans, ref diag, ref n, ref k, (IntPtr)ptrA, ref lda, (IntPtr)ptrX, ref incX);
                    }
                }
            }

            /// <summary>Solves a system of linear equations whose coefficients are in a triangular band matrix, i.e. solves op(A) * x = b, where op(A) = A or op(A) = A^t.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="k">The number of super-diagonales of A if the matrix A is provided in its upper triangular representation; the number of sub-diagonals otherwise.</param>
            /// <param name="a">The triangular band matrix with dimension (<paramref name="lda"/>, <paramref name="n"/>).</param>
            /// <param name="x">The vector b (input), x (output) with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements (input/output).</param>
            /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least (1  + <paramref name="k"/>).</param>
            /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
            /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
            /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
            /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
            public void dtbsv(int n, int k, double[] a, double[] x, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1)
            {
                unsafe
                {
                    fixed (double* ptrA = a, ptrX = x)
                    {
                        char uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? 'u' : 'l';
                        char trans = (transpose == BLAS.MatrixTransposeState.NoTranspose) ? 'n' : 't';
                        char diag = (isUnitTriangular == true) ? 'u' : 'n';
                        _dtbsv(ref uplo, ref trans, ref diag, ref n, ref k, (IntPtr)ptrA, ref lda, (IntPtr)ptrX, ref incX);
                    }
                }
            }

            /// <summary>Computes a matrix-vector product using a triangular packed matrix, i.e. x := op(A) * x, where op(A) = A or op(A) = A^t.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="aPacked">The triangular packed matrix A with dimension at least (<paramref name="n"/> * (<paramref name="n"/> + 1) ) / 2.</param>
            /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
            /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
            /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
            /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
            /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
            public void dtpmv(int n, double[] aPacked, double[] x, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1)
            {
                unsafe
                {
                    fixed (double* ptrA = aPacked, ptrX = x)
                    {
                        char uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? 'u' : 'l';
                        char trans = (transpose == BLAS.MatrixTransposeState.NoTranspose) ? 'n' : 't';
                        char diag = (isUnitTriangular == true) ? 'u' : 'n';
                        _dtpmv(ref uplo, ref trans, ref diag, ref n, (IntPtr)ptrA, (IntPtr)ptrX, ref incX);
                    }
                }
            }

            /// <summary>Solves a system of linear equations whose coefficients are in a triangular packed matrix, i.e. op(A) * x = b, where op(A) = A or op(A) = A^t.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="aPacked">The triangular packed matrix A with dimension at least (<paramref name="n"/> * (<paramref name="n"/> + 1) ) / 2.</param>
            /// <param name="x">The vector b (input), x (output) with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements (input/output).</param>
            /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
            /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
            /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
            /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
            public void dtpsv(int n, double[] aPacked, double[] x, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1)
            {
                unsafe
                {
                    fixed (double* ptrA = aPacked, ptrX = x)
                    {
                        char uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? 'u' : 'l';
                        char trans = (transpose == BLAS.MatrixTransposeState.NoTranspose) ? 'n' : 't';
                        char diag = (isUnitTriangular == true) ? 'u' : 'n';
                        _dtpsv(ref uplo, ref trans, ref diag, ref n, (IntPtr)ptrA, (IntPtr)ptrX, ref incX);
                    }
                }
            }

            /// <summary>Computes a matrix-vector product using a triangular matrix, i.e. x := op(A) * x, where op(A) = A or op(A) = A^t.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="a">The triangular matrix A with dimension (<paramref name="lda"/>, <paramref name="n"/>).</param>
            /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
            /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1, <paramref name="n"/>).</param>
            /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
            /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
            /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
            /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
            public void dtrmv(int n, double[] a, double[] x, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1)
            {
                unsafe
                {
                    fixed (double* ptrA = a, ptrX = x)
                    {
                        char uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? 'u' : 'l';
                        char trans = (transpose == BLAS.MatrixTransposeState.NoTranspose) ? 'n' : 't';
                        char diag = (isUnitTriangular == true) ? 'u' : 'n';
                        _dtrmv(ref uplo, ref trans, ref diag, ref n, (IntPtr)ptrA, ref lda, (IntPtr)ptrX, ref incX);
                    }
                }
            }

            /// <summary>Solves a system of linear equations whose coefficients are in a triangular matrix, i.e. op(A) * x = b, where op(A) = A or op(A) = A^t.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="a">The triangular matrix A with dimension (<paramref name="lda"/>, <paramref name="n"/>).</param>
            /// <param name="x">The vector b (input), x (output) with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements (input/output).</param>
            /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1, <paramref name="n"/>).</param>
            /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
            /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
            /// <param name="transpose">A value indicating whether op(A) = A or op(A) = A^t.</param>
            /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
            public void dtrsv(int n, double[] a, double[] x, int lda, BLAS.TriangularMatrixType triangularMatrixType, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1)
            {
                unsafe
                {
                    fixed (double* ptrA = a, ptrX = x)
                    {
                        char uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? 'u' : 'l';
                        char trans = (transpose == BLAS.MatrixTransposeState.NoTranspose) ? 'n' : 't';
                        char diag = (isUnitTriangular == true) ? 'u' : 'n';
                        _dtrsv(ref uplo, ref trans, ref diag, ref n, (IntPtr)ptrA, ref lda, (IntPtr)ptrX, ref incX);
                    }
                }
            }
            #endregion

            #region (double precision) complex methods

            /// <summary>Computes a matrix-vector product using a general band matrix, i.e. y := \alpha * op(A) * x + \beta * y, where op(A) = A, op(A) = A^t or op(A) = A^h.
            /// </summary>
            /// <param name="m">The number of rows of matrix A.</param>
            /// <param name="n">The number of columns of matrix A.</param>
            /// <param name="kl">The number of sub-diagonals of matrix A.</param>
            /// <param name="ku">The number of super-diagonals of matrix A.</param>
            /// <param name="alpha">The scalar factor \alpha.</param>
            /// <param name="a">The general band matrix A of dimension (<paramref name="lda"/>, <paramref name="n"/>). The leading (<paramref name="kl"/> + <paramref name="ku"/> + 1) by <paramref name="n"/> part
            /// must contain the matrix of coefficients (supplied column-by-column) with the leading diagonal of the matrix
            /// in row (<paramref name="ku"/> + 1) of the array, the first super-diagonal starting at position 2 in row ku,
            /// the first sub-diagonal starting at position 1 in row (ku + 2), etc.
            /// The following program segment transfers a band matrix from conventional full matrix storage to band storage:
            /// <code>
            /// for j = 0 to n-1
            /// k = ku - j
            /// for i = max(0, j-ku) to min(m-1, j+kl-1)
            /// a(k+i, j) = matrix(i,j)
            /// end
            /// end
            /// </code></param>
            /// <param name="x">The vector x with at least 1+(<paramref name="n"/>-1) * |<paramref name="incX"/>| elements if 'op(A)=A'; 1+(<paramref name="m"/>-1) *|<paramref name="incX"/>| otherwise.</param>
            /// <param name="beta">The scalar factor \beta.</param>
            /// <param name="y">The vector y with at least 1+(<paramref name="m"/>-1) *|<paramref name="incY"/>| elements if 'op(A)=A'; 1+(<paramref name="n"/>-1) * |<paramref name="incY"/>| otherwise (input/output).</param>
            /// <param name="lda">The leading dimension, must be at least <paramref name="kl"/> + <paramref name="ku"/> +1.</param>
            /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
            /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
            /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
            public void zgbmv(int m, int n, int kl, int ku, Complex alpha, Complex[] a, Complex[] x, Complex beta, Complex[] y, int lda, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1, int incY = 1)
            {
                unsafe
                {
                    fixed (Complex* ptrA = a, ptrX = x, ptrY = y)
                    {
                        char trans = transpose.AsBlasTransposeChar();
                        _zgbmv(ref trans, ref m, ref n, ref kl, ref ku, ref alpha, (IntPtr)ptrA, ref lda, (IntPtr)ptrX, ref incX, ref beta, (IntPtr)ptrY, ref incY);
                    }
                }
            }

            /// <summary>Computes a matrix-vector product for a general matrix, i.e. y = \alpha * op(A)*x + \beta*y, where op(A) = A, op(A) = A^t or op(A) = A^h.
            /// </summary>
            /// <param name="m">The number of rows of matrix A.</param>
            /// <param name="n">The number of columns of matrix A.</param>
            /// <param name="alpha">The scalar \alpha.</param>
            /// <param name="a">The matrix A of dimension (<paramref name="lda"/>, <paramref name="n"/>) supplied column-by-column.</param>
            /// <param name="x">The vector x with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incX"/>| elements if 'op(A)=A'; 1 + (<paramref name="m"/>-1) * |<paramref name="incY"/>| elements otherwise.</param>
            /// <param name="beta">The scalar \beta.</param>
            /// <param name="y">The vector y with at least 1 + (<paramref name="m"/>-1) * |<paramref name="incY"/>| elements if 'op(A)=A'; 1 + (<paramref name="n"/>-1) * | <paramref name="incX"/>| otherwise (input/output).</param>
            /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="m"/>).</param>
            /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
            /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
            /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
            public void zgemv(int m, int n, Complex alpha, Complex[] a, Complex[] x, Complex beta, Complex[] y, int lda, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1, int incY = 1)
            {
                unsafe
                {
                    fixed (Complex* ptrA = a, ptrX = x, ptrY = y)
                    {
                        char trans = transpose.AsBlasTransposeChar();
                        _zgemv(ref trans, ref m, ref n, ref alpha, (IntPtr)ptrA, ref lda, (IntPtr)ptrX, ref incX, ref beta, (IntPtr)ptrY, ref incY);
                    }
                }
            }

            /// <summary>Performs a rank-1 update (conjuaged) of a general matrix, i.e. A := \alpha * x * conj(y^t) + A.
            /// </summary>
            /// <param name="m">The number of rows of matrix A.</param>
            /// <param name="n">The number of columns of matrix A.</param>
            /// <param name="alpha">The scalar \alpha.</param>
            /// <param name="x">The vector x with at least 1 + (<paramref name="m"/>-1) * |<paramref name="incX"/>| elements.</param>
            /// <param name="y">The vector y with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incY"/>| elements.</param>
            /// <param name="a">The matrix A of dimension (<paramref name="lda"/>, <paramref name="n"/>) supplied column-by-column.</param>
            /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="m"/>).</param>
            /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
            /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
            public void zgerc(int m, int n, Complex alpha, Complex[] x, Complex[] y, Complex[] a, int lda, int incX = 1, int incY = 1)
            {
                unsafe
                {
                    fixed (Complex* ptrA = a, ptrX = x, ptrY = y)
                    {
                        _zgerc(ref m, ref n, ref alpha, (IntPtr)ptrX, ref incX, (IntPtr)ptrY, ref incY, (IntPtr)ptrA, ref lda);
                    }
                }
            }

            /// <summary>Performs a rank-1 update (unconjugated) of a general matrix., i.e. A := \alpha * x * y^t + A.
            /// </summary>
            /// <param name="m">The number of rows of matrix A.</param>
            /// <param name="n">The number of columns of matrix A.</param>
            /// <param name="alpha">The scalar \alpha.</param>
            /// <param name="x">The vector x with at least 1 + (<paramref name="m"/>-1) * |<paramref name="incX"/>| elements.</param>
            /// <param name="y">The vector y with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incY"/>| elements.</param>
            /// <param name="a">The matrix A with dimension (<paramref name="lda"/>, <paramref name="n"/>) supplied column-by-column.</param>
            /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="m"/>).</param>
            /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
            /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
            public void zgeru(int m, int n, Complex alpha, Complex[] x, Complex[] y, Complex[] a, int lda, int incX = 1, int incY = 1)
            {
                unsafe
                {
                    fixed (Complex* ptrA = a, ptrX = x, ptrY = y)
                    {
                        _zgeru(ref m, ref n, ref alpha, (IntPtr)ptrX, ref incX, (IntPtr)ptrY, ref incY, (IntPtr)ptrA, ref lda);
                    }
                }
            }

            /// <summary>Computes a matrix-vector product using a Hermitian band matrix, i.e. y := \alpha * A * x + \beta * y.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="k">The number of super-diagonals.</param>
            /// <param name="alpha">The scalar \alpha.</param>
            /// <param name="a">The matrix Hermitian band matrix A with dimension (<paramref name="lda"/>, <paramref name="n"/>).</param>
            /// <param name="x">The vector x with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incX"/>| elements.</param>
            /// <param name="beta">The scalar \beta.</param>
            /// <param name="y">The vector y with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incY"/>| elements (input/output).</param>
            /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least (1 + <paramref name="k"/>).</param>
            /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
            /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
            /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
            public void zhbmv(int n, int k, Complex alpha, Complex[] a, Complex[] x, Complex beta, Complex[] y, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1, int incY = 1)
            {
                unsafe
                {
                    fixed (Complex* ptrA = a, ptrX = x, ptrY = y)
                    {
                        char uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? 'u' : 'l';
                        _zhbmv(ref uplo, ref n, ref k, ref alpha, (IntPtr)ptrA, ref lda, (IntPtr)ptrX, ref incX, ref beta, (IntPtr)ptrY, ref incY);
                    }
                }
            }

            /// <summary>Computes a matrix-vector product using a Hermitian matrix, i.e. y := \alpha * A * x + \beta * y.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="alpha">The scalar \alpha.</param>
            /// <param name="x">The vector x with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incX"/>| elements.</param>
            /// <param name="beta">The scalar \beta.</param>
            /// <param name="y">The vector y with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incY"/>| elements (input/output).</param>
            /// <param name="a">The Hermitian matrix A with dimension (<paramref name="lda"/>, <paramref name="n"/>).</param>
            /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1, <paramref name="n"/>).</param>
            /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
            /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
            /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
            public void zhemv(int n, Complex alpha, Complex[] x, Complex beta, Complex[] y, Complex[] a, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1, int incY = 1)
            {
                unsafe
                {
                    fixed (Complex* ptrA = a, ptrX = x, ptrY = y)
                    {
                        char uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? 'u' : 'l';
                        _zhemv(ref uplo, ref n, ref alpha, (IntPtr)ptrA, ref lda, (IntPtr)ptrX, ref incX, ref beta, (IntPtr)ptrY, ref incY);
                    }
                }
            }

            /// <summary>Performs a rank-1 update of a Hermitian matrix, i.e. A := \alpha * x * conj(x^t) + A.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="alpha">The scalar \alpha.</param>
            /// <param name="x">The vector x with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incX"/>| elements.</param>
            /// <param name="a">The Hermitian matrix A with dimension (<paramref name="lda"/>, <paramref name="n"/>).</param>
            /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1, <paramref name="n"/>).</param>
            /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
            /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
            public void zher(int n, double alpha, Complex[] x, Complex[] a, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1)
            {
                unsafe
                {
                    fixed (Complex* ptrA = a, ptrX = x)
                    {
                        char uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? 'u' : 'l';
                        _zher(ref uplo, ref n, ref alpha, (IntPtr)ptrX, ref incX, (IntPtr)ptrA, ref lda);
                    }
                }
            }

            /// <summary>Performs a rank-2 update of a Hermitian matrix, i.e. A := \alpha * x * conjg(y^t) + conjg(\alpha) * y * conjg(x^t) + A.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="alpha">The scalar \alpha.</param>
            /// <param name="x">The vector x with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incX"/>| elements.</param>
            /// <param name="y">The vector y with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incY"/>| elements.</param>
            /// <param name="a">The Hermitian matrix A with dimension (<paramref name="lda"/>, <paramref name="n"/>).</param>
            /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1, <paramref name="n"/>).</param>
            /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
            /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
            /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
            public void zher2(int n, Complex alpha, Complex[] x, Complex[] y, Complex[] a, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1, int incY = 1)
            {
                unsafe
                {
                    fixed (Complex* ptrA = a, ptrX = x, ptrY = y)
                    {
                        char uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? 'u' : 'l';
                        _zher2(ref uplo, ref n, ref alpha, (IntPtr)ptrX, ref incX, (IntPtr)ptrY, ref incY, (IntPtr)ptrA, ref lda);
                    }
                }
            }

            /// <summary>Computes a matrix-vector product using a Hermitian packed matrix, i.e. \alpha * A * x + \beta * y.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="alpha">The scalar \alpha.</param>
            /// <param name="aPacked">The Hermitian packed matrix A with dimension at least (<paramref name="n"/> * (<paramref name="n"/> + 1) ) / 2.</param>
            /// <param name="x">The vector x with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incX"/>| elements.</param>
            /// <param name="beta">The scalar \beta.</param>
            /// <param name="y">The vector y with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incY"/>| elements.</param>
            /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
            /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
            /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
            public void zhpmv(int n, Complex alpha, Complex[] aPacked, Complex[] x, Complex beta, Complex[] y, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1, int incY = 1)
            {
                unsafe
                {
                    fixed (Complex* ptrA = aPacked, ptrX = x, ptrY = y)
                    {
                        char uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? 'u' : 'l';
                        _zhpmv(ref uplo, ref n, ref alpha, (IntPtr)ptrA, (IntPtr)ptrX, ref incX, ref beta, (IntPtr)ptrY, ref incY);
                    }
                }
            }

            /// <summary>Performs a rank-1 update of a Hermitian packed matrix, i.e. A := \alpha * x * conjg(x^t) + A.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="alpha">The scalar \alpha.</param>
            /// <param name="x">The vector x with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incX"/>| elements.</param>
            /// <param name="aPacked">The Hermitian packed matrix A with dimension at least (<paramref name="n"/> * (<paramref name="n"/> + 1) ) / 2.</param>
            /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
            /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
            public void zhpr(int n, double alpha, Complex[] x, Complex[] aPacked, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1)
            {
                unsafe
                {
                    fixed (Complex* ptrA = aPacked, ptrX = x)
                    {
                        char uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? 'u' : 'l';
                        _zhpr(ref uplo, ref n, ref alpha, (IntPtr)ptrX, ref incX, (IntPtr)ptrA);
                    }
                }
            }

            /// <summary>Performs a rank-2 update of a Hermitian packed matrix, i.e. A := \alpha * x * conjg(y^t) + conjg(\alpha) * y * conjg(x^t) + A.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="alpha">The scalar \alpha.</param>
            /// <param name="x">The vector x with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incX"/>| elements.</param>
            /// <param name="y">The vector y with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incY"/>| elements.</param>
            /// <param name="aPacked">The Hermitian packed matrix A with dimension at least (<paramref name="n"/> * (<paramref name="n"/> + 1) ) / 2.</param>
            /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
            /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
            /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
            public void zhpr2(int n, Complex alpha, Complex[] x, Complex[] y, Complex[] aPacked, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1, int incY = 1)
            {
                unsafe
                {
                    fixed (Complex* ptrA = aPacked, ptrX = x, ptrY = y)
                    {
                        char uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? 'u' : 'l';
                        _zhpr2(ref uplo, ref n, ref alpha, (IntPtr)ptrX, ref incX, (IntPtr)ptrY, ref incY, (IntPtr)ptrA);
                    }
                }
            }

            /// <summary>Computes a matrix-vector product using a triangular band matrix, i.e. x := op(A) * x, where op(A) = A, op(A) = A^t or op(A) = A^h.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="k">The number of super-diagonals of A if the matrix A is provided in its upper triangular representation; the number of sub-diagonals otherwise.</param>
            /// <param name="a">The triangular band matrix with dimension (<paramref name="lda"/>, <paramref name="n"/>).</param>
            /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
            /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least (1  + <paramref name="k"/>).</param>
            /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
            /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
            /// <param name="transpose">A value indicating whether 'op(A)=A', 'op(A)=A^t' or 'op(A)=A^h'.</param>
            /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
            public void ztbmv(int n, int k, Complex[] a, Complex[] x, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1)
            {
                unsafe
                {
                    fixed (Complex* ptrA = a, ptrX = x)
                    {
                        char uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? 'u' : 'l';
                        char trans = transpose.AsBlasTransposeChar();
                        char diag = (isUnitTriangular == true) ? 'u' : 'n';
                        _ztbmv(ref uplo, ref trans, ref diag, ref n, ref k, (IntPtr)ptrA, ref lda, (IntPtr)ptrX, ref incX);
                    }
                }
            }

            /// <summary>Solves a system of linear equations whose coefficients are in a triangular band matrix, i.e. op(A) * x = b, where op(A) = A, op(A) = A ^t or op(A) = A^h.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="k">The number of super-diagonals of A if the matrix A is provided in its upper triangular representation; the number of sub-diagonals otherwise.</param>
            /// <param name="a">The triangular band matrix with dimension (<paramref name="lda"/>, <paramref name="n"/>).</param>
            /// <param name="x">The vector b (input), x (output) with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements (input/output).</param>
            /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least (1  + <paramref name="k"/>).</param>
            /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
            /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
            /// <param name="transpose">A value indicating whether 'op(A)=A', 'op(A)=A^t' or 'op(A)=A^h'.</param>
            /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
            public void ztbsv(int n, int k, Complex[] a, Complex[] x, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1)
            {
                unsafe
                {
                    fixed (Complex* ptrA = a, ptrX = x)
                    {
                        char uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? 'u' : 'l';
                        char trans = transpose.AsBlasTransposeChar();
                        char diag = (isUnitTriangular == true) ? 'u' : 'n';
                        _ztbsv(ref uplo, ref trans, ref diag, ref n, ref k, (IntPtr)ptrA, ref lda, (IntPtr)ptrX, ref incX);
                    }
                }
            }

            /// <summary>Computes a matrix-vector product using a triangular packed matrix, i.e. x := op(A) * x, where op(A) = A, op(A) = A ^t or op(A) = A^h.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="aPacked">The triangular packed matrix A with dimension at least (<paramref name="n"/> * (<paramref name="n"/> + 1) ) / 2.</param>
            /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
            /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
            /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
            /// <param name="transpose">A value indicating whether 'op(A)=A', 'op(A)=A^t' or 'op(A)=A^h'.</param>
            /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
            public void ztpmv(int n, Complex[] aPacked, Complex[] x, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1)
            {
                unsafe
                {
                    fixed (Complex* ptrA = aPacked, ptrX = x)
                    {
                        char uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? 'u' : 'l';
                        char trans = transpose.AsBlasTransposeChar();
                        char diag = (isUnitTriangular == true) ? 'u' : 'n';
                        _ztpmv(ref uplo, ref trans, ref diag, ref n, (IntPtr)ptrA, (IntPtr)ptrX, ref incX);
                    }
                }
            }

            /// <summary>Solves a system of linear equations whose coefficients are in a triangular packed matrix, i.e. op(A) * x = b, where op(A) = A, op(A) = A ^t or op(A) = A^h.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="aPacked">The triangular packed matrix A with dimension at least (<paramref name="n"/> * (<paramref name="n"/> + 1) ) / 2.</param>
            /// <param name="x">The vector b (input), x (output) with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements (input/output).</param>
            /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
            /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
            /// <param name="transpose">A value indicating whether 'op(A)=A', 'op(A)=A^t' or 'op(A)=A^h'.</param>
            /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
            public void ztpsv(int n, Complex[] aPacked, Complex[] x, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1)
            {
                unsafe
                {
                    fixed (Complex* ptrA = aPacked, ptrX = x)
                    {
                        char uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? 'u' : 'l';
                        char trans = transpose.AsBlasTransposeChar();
                        char diag = (isUnitTriangular == true) ? 'u' : 'n';
                        _ztpsv(ref uplo, ref trans, ref diag, ref n, (IntPtr)ptrA, (IntPtr)ptrX, ref incX);
                    }
                }
            }

            /// <summary>Computes a matrix-vector product using a triangular matrix, i.e. x := op(A) * x, where op(A) = A, op(A) = A ^t or op(A) = A^h.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="a">The triangular matrix A with dimension at least (<paramref name="lda"/>, <paramref name="n"/>).</param>
            /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
            /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1, <paramref name="n"/>).</param>
            /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
            /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
            /// <param name="transpose">A value indicating whether 'op(A)=A', 'op(A)=A^t' or 'op(A)=A^h'.</param>
            /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
            public void ztrmv(int n, Complex[] a, Complex[] x, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1)
            {
                unsafe
                {
                    fixed (Complex* ptrA = a, ptrX = x)
                    {
                        char uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? 'u' : 'l';
                        char trans = transpose.AsBlasTransposeChar();
                        char diag = (isUnitTriangular == true) ? 'u' : 'n';
                        _ztrmv(ref uplo, ref trans, ref diag, ref n, (IntPtr)ptrA, ref lda, (IntPtr)ptrX, ref incX);
                    }
                }

            }

            /// <summary>Solves a system of linear equations whose coefficients are in a triangular matrix, i.e. op(A) * x = b, where op(A) = A, op(A) = A ^t or op(A) = A^h.
            /// </summary>
            /// <param name="n">The order of matrix A.</param>
            /// <param name="a">The triangular matrix A with dimension (<paramref name="lda"/>, <paramref name="n"/>).</param>
            /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
            /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1, <paramref name="n"/>).</param>
            /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
            /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
            /// <param name="transpose">A value indicating whether 'op(A)=A', 'op(A)=A^t' or 'op(A)=A^h'.</param>
            /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
            public void ztrsv(int n, Complex[] a, Complex[] x, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1)
            {
                unsafe
                {
                    fixed (Complex* ptrA = a, ptrX = x)
                    {
                        char uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? 'u' : 'l';
                        char trans = transpose.AsBlasTransposeChar();
                        char diag = (isUnitTriangular == true) ? 'u' : 'n';
                        _ztrsv(ref uplo, ref trans, ref diag, ref n, (IntPtr)ptrA, ref lda, (IntPtr)ptrX, ref incX);
                    }
                }
            }
            #endregion

            #endregion
        }
    }
}