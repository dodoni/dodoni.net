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
    public partial class CBlasNativeWrapper
    {
        /// <summary>The wrapper for BLAS level 2 methods (C interface). See http://www.netlib.org/blas for further information.
        /// </summary>
        protected internal class Level2CBLAS : ILevel2BLAS
        {
            #region private function import

            #region double precision methods

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_dgemv", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dgemv(CBLAS.Order order, CBLAS.Transpose trans, int m, int n, double alpha, double[] a, int lda, double[] x, int incX, double beta, double[] y, int incY);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_dgbmv", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dgbmv(CBLAS.Order order, CBLAS.Transpose trans, int m, int n, int kl, int ku, double alpha, double[] a, int lda, double[] x, int incX, double beta, double[] y, int incY);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_dger", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dger(CBLAS.Order order, int m, int n, double alpha, double[] x, int incx, double[] y, int incy, double[] a, int lda);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_dsbmv", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dsbmv(CBLAS.Order order, CBLAS.UpLo uplo, int n, int k, double alpha, double[] a, int lda, double[] x, int incx, double beta, double[] y, int incy);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_dspmv", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dspmv(CBLAS.Order order, CBLAS.UpLo uplo, int n, double alpha, double[] ap, double[] x, int incx, double beta, double[] y, int incy);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_dspr", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dspr(CBLAS.Order order, CBLAS.UpLo uplo, int n, double alpha, double[] x, int incx, double[] ap);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_dspr2", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dspr2(CBLAS.Order order, CBLAS.UpLo uplo, int n, double alpha, double[] x, int incx, double[] y, int incy, double[] ap);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_dsymv", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dsymv(CBLAS.Order order, CBLAS.UpLo uplo, int n, double alpha, double[] a, int lda, double[] x, int incx, double beta, double[] y, int incy);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_dsyr", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dsyr(CBLAS.Order order, CBLAS.UpLo uplo, int n, double alpha, double[] x, int incx, double[] a, int lda);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_dsyr2", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dsyr2(CBLAS.Order order, CBLAS.UpLo uplo, int n, double alpha, double[] x, int incx, double[] y, int incy, double[] a, int lda);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_dtbmv", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dtbmv(CBLAS.Order order, CBLAS.UpLo uplo, CBLAS.Transpose trans, CBLAS.Diag diag, int n, int k, double[] a, int lda, double[] x, int incx);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_dtbsv", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dtbsv(CBLAS.Order order, CBLAS.UpLo uplo, CBLAS.Transpose trans, CBLAS.Diag diag, int n, int k, double[] a, int lda, double[] x, int incx);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_dtpmv", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dtpmv(CBLAS.Order order, CBLAS.UpLo uplo, CBLAS.Transpose trans, CBLAS.Diag diag, int n, double[] ap, double[] x, int incx);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_dtpsv", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dtpsv(CBLAS.Order order, CBLAS.UpLo uplo, CBLAS.Transpose trans, CBLAS.Diag diag, int n, double[] ap, double[] x, int incx);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_dtrmv", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dtrmv(CBLAS.Order order, CBLAS.UpLo uplo, CBLAS.Transpose trans, CBLAS.Diag diag, int n, double[] a, int lda, double[] x, int incx);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_dtrsv", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dtrsv(CBLAS.Order order, CBLAS.UpLo uplo, CBLAS.Transpose trans, CBLAS.Diag diag, int n, double[] a, int lda, double[] x, int incx);
            #endregion

            #region (double precision) complex methods

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_zgemv", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zgemv(CBLAS.Order order, CBLAS.Transpose trans, int m, int n, ref Complex alpha, [In, Out] Complex[] a, int lda, [In, Out] Complex[] x, int incX, ref Complex beta, [In, Out] Complex[] y, int incY);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_zgbmv", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zgbmv(CBLAS.Order order, CBLAS.Transpose trans, int m, int n, int kl, int ku, ref Complex alpha, [In, Out] Complex[] a, int lda, [In, Out] Complex[] x, int incX, ref Complex beta, [In, Out] Complex[] y, int incY);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_zgerc", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zgerc(CBLAS.Order order, int m, int n, ref Complex alpha, [In, Out] Complex[] x, int incx, [In, Out] Complex[] y, int incy, [In, Out] Complex[] a, int lda);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_zgeru", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zgeru(CBLAS.Order order, int m, int n, ref Complex alpha, [In, Out] Complex[] x, int incx, [In, Out] Complex[] y, int incy, [In, Out] Complex[] a, int lda);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_zhbmv", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zhbmv(CBLAS.Order order, CBLAS.UpLo uplo, int n, int k, ref Complex alpha, [In, Out] Complex[] a, int lda, [In, Out] Complex[] x, int incx, ref Complex beta, [In, Out] Complex[] y, int incy);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_zhemv", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zhemv(CBLAS.Order order, CBLAS.UpLo uplo, int n, ref Complex alpha, [In, Out] Complex[] a, int lda, [In, Out] Complex[] x, int incx, ref Complex beta, [In, Out] Complex[] y, int incy);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_zher", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zher(CBLAS.Order order, CBLAS.UpLo uplo, int n, double alpha, [In, Out] Complex[] x, int incx, [In, Out] Complex[] a, int lda);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_zher2", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zher2(CBLAS.Order order, CBLAS.UpLo uplo, int n, ref Complex alpha, [In, Out] Complex[] x, int incx, [In, Out] Complex[] y, int incy, [In, Out] Complex[] a, int lda);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_zhpmv", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zhpmv(CBLAS.Order order, CBLAS.UpLo uplo, int n, ref Complex alpha, [In, Out] Complex[] ap, [In, Out] Complex[] x, int incx, ref Complex beta, [In, Out] Complex[] y, int incy);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_zhpr", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zhpr(CBLAS.Order order, CBLAS.UpLo uplo, int n, double alpha, [In, Out] Complex[] x, int incx, [In, Out] Complex[] ap);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_zhpr2", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zhpr2(CBLAS.Order order, CBLAS.UpLo uplo, int n, ref Complex alpha, [In, Out] Complex[] x, int incx, [In, Out] Complex[] y, int incy, [In, Out] Complex[] ap);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_ztbmv", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _ztbmv(CBLAS.Order order, CBLAS.UpLo uplo, CBLAS.Transpose trans, CBLAS.Diag diag, int n, int k, [In, Out] Complex[] a, int lda, [In, Out] Complex[] x, int incx);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_ztbsv", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _ztbsv(CBLAS.Order order, CBLAS.UpLo uplo, CBLAS.Transpose trans, CBLAS.Diag diag, int n, int k, [In, Out] Complex[] a, int lda, [In, Out] Complex[] x, int incx);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_ztpmv", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _ztpmv(CBLAS.Order order, CBLAS.UpLo uplo, CBLAS.Transpose trans, CBLAS.Diag diag, int n, [In, Out] Complex[] ap, [In, Out] Complex[] x, int incx);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_ztpsv", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _ztpsv(CBLAS.Order order, CBLAS.UpLo uplo, CBLAS.Transpose trans, CBLAS.Diag diag, int n, [In, Out] Complex[] ap, [In, Out] Complex[] x, int incx);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_ztrmv", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _ztrmv(CBLAS.Order order, CBLAS.UpLo uplo, CBLAS.Transpose trans, CBLAS.Diag diag, int n, [In, Out] Complex[] a, int lda, [In, Out] Complex[] x, int incx);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_ztrsv", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _ztrsv(CBLAS.Order order, CBLAS.UpLo uplo, CBLAS.Transpose trans, CBLAS.Diag diag, int n, [In, Out] Complex[] a, int lda, [In, Out] Complex[] x, int incx);
            #endregion

            #endregion

            #region public constructors

            /// <summary>Initializes a new instance of the <see c="Level2CBLAS"/> class.
            /// </summary>
            public Level2CBLAS()
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
                _dgbmv(CBLAS.Order.ColumnMajor, (transpose == BLAS.MatrixTransposeState.NoTranspose) ? CBLAS.Transpose.NoTranspose : CBLAS.Transpose.Transpose, m, n, kl, ku, alpha, a, lda, x, incX, beta, y, incY);
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
                _dgemv(CBLAS.Order.ColumnMajor, (transpose == BLAS.MatrixTransposeState.NoTranspose) ? CBLAS.Transpose.NoTranspose : CBLAS.Transpose.Transpose, m, n, alpha, a, lda, x, incX, beta, y, incY);
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
                _dger(CBLAS.Order.ColumnMajor, m, n, alpha, x, incX, y, incY, a, lda);
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
                CBLAS.UpLo uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? CBLAS.UpLo.Upper : CBLAS.UpLo.Lower;
                _dsbmv(CBLAS.Order.ColumnMajor, uplo, n, k, alpha, a, lda, x, incX, beta, y, incY);
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
                CBLAS.UpLo uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? CBLAS.UpLo.Upper : CBLAS.UpLo.Lower;
                _dspmv(CBLAS.Order.ColumnMajor, uplo, n, alpha, aPacked, x, incX, beta, y, incY);
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
                CBLAS.UpLo uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? CBLAS.UpLo.Upper : CBLAS.UpLo.Lower;
                _dspr(CBLAS.Order.ColumnMajor, uplo, n, alpha, x, incX, aPacked);
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
                CBLAS.UpLo uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? CBLAS.UpLo.Upper : CBLAS.UpLo.Lower;
                _dspr2(CBLAS.Order.ColumnMajor, uplo, n, alpha, x, incX, y, incY, aPacked);
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
                CBLAS.UpLo uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? CBLAS.UpLo.Upper : CBLAS.UpLo.Lower;
                _dsymv(CBLAS.Order.ColumnMajor, uplo, n, alpha, a, lda, x, incX, beta, y, incY);
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
                CBLAS.UpLo uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? CBLAS.UpLo.Upper : CBLAS.UpLo.Lower;
                _dsyr(CBLAS.Order.ColumnMajor, uplo, n, alpha, x, incX, a, lda);
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
                CBLAS.UpLo uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? CBLAS.UpLo.Upper : CBLAS.UpLo.Lower;
                _dsyr2(CBLAS.Order.ColumnMajor, uplo, n, alpha, x, incX, y, incY, a, lda);
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
                CBLAS.UpLo uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? CBLAS.UpLo.Upper : CBLAS.UpLo.Lower;
                CBLAS.Transpose trans = (transpose == BLAS.MatrixTransposeState.NoTranspose) ? CBLAS.Transpose.NoTranspose : CBLAS.Transpose.Transpose;
                CBLAS.Diag diag = (isUnitTriangular == true) ? CBLAS.Diag.Unit : CBLAS.Diag.NonUnit;
                _dtbmv(CBLAS.Order.ColumnMajor, uplo, trans, diag, n, k, a, lda, x, incX);
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
                CBLAS.UpLo uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? CBLAS.UpLo.Upper : CBLAS.UpLo.Lower;
                CBLAS.Transpose trans = (transpose == BLAS.MatrixTransposeState.NoTranspose) ? CBLAS.Transpose.NoTranspose : CBLAS.Transpose.Transpose;
                CBLAS.Diag diag = (isUnitTriangular == true) ? CBLAS.Diag.Unit : CBLAS.Diag.NonUnit;
                _dtbsv(CBLAS.Order.ColumnMajor, uplo, trans, diag, n, k, a, lda, x, incX);
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
                CBLAS.UpLo uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? CBLAS.UpLo.Upper : CBLAS.UpLo.Lower;
                CBLAS.Transpose trans = (transpose == BLAS.MatrixTransposeState.NoTranspose) ? CBLAS.Transpose.NoTranspose : CBLAS.Transpose.Transpose;
                CBLAS.Diag diag = (isUnitTriangular == true) ? CBLAS.Diag.Unit : CBLAS.Diag.NonUnit;
                _dtpmv(CBLAS.Order.ColumnMajor, uplo, trans, diag, n, aPacked, x, incX);
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
                CBLAS.UpLo uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? CBLAS.UpLo.Upper : CBLAS.UpLo.Lower;
                CBLAS.Transpose trans = (transpose == BLAS.MatrixTransposeState.NoTranspose) ? CBLAS.Transpose.NoTranspose : CBLAS.Transpose.Transpose;
                CBLAS.Diag diag = (isUnitTriangular == true) ? CBLAS.Diag.Unit : CBLAS.Diag.NonUnit;
                _dtpsv(CBLAS.Order.ColumnMajor, uplo, trans, diag, n, aPacked, x, incX);
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
                CBLAS.UpLo uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? CBLAS.UpLo.Upper : CBLAS.UpLo.Lower;
                CBLAS.Transpose trans = (transpose == BLAS.MatrixTransposeState.NoTranspose) ? CBLAS.Transpose.NoTranspose : CBLAS.Transpose.Transpose;
                CBLAS.Diag diag = (isUnitTriangular == true) ? CBLAS.Diag.Unit : CBLAS.Diag.NonUnit;
                _dtrmv(CBLAS.Order.ColumnMajor, uplo, trans, diag, n, a, lda, x, incX);
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
                CBLAS.UpLo uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? CBLAS.UpLo.Upper : CBLAS.UpLo.Lower;
                CBLAS.Transpose trans = (transpose == BLAS.MatrixTransposeState.NoTranspose) ? CBLAS.Transpose.NoTranspose : CBLAS.Transpose.Transpose;
                CBLAS.Diag diag = (isUnitTriangular == true) ? CBLAS.Diag.Unit : CBLAS.Diag.NonUnit;
                _dtrsv(CBLAS.Order.ColumnMajor, uplo, trans, diag, n, a, lda, x, incX);
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
                _zgbmv(CBLAS.Order.ColumnMajor, transpose.AsCblasTranspose(), m, n, kl, ku, ref alpha, a, lda, x, incX, ref beta, y, incY);
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
                _zgemv(CBLAS.Order.ColumnMajor, transpose.AsCblasTranspose(), m, n, ref alpha, a, lda, x, incX, ref beta, y, incY);
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
                _zgerc(CBLAS.Order.ColumnMajor, m, n, ref alpha, x, incX, y, incY, a, lda);
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
                _zgeru(CBLAS.Order.ColumnMajor, m, n, ref alpha, x, incX, y, incY, a, lda);
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
                CBLAS.UpLo uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? CBLAS.UpLo.Upper : CBLAS.UpLo.Lower;
                _zhbmv(CBLAS.Order.ColumnMajor, uplo, n, k, ref alpha, a, lda, x, incX, ref beta, y, incY);
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
                CBLAS.UpLo uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? CBLAS.UpLo.Upper : CBLAS.UpLo.Lower;
                _zhemv(CBLAS.Order.ColumnMajor, uplo, n, ref alpha, a, lda, x, incX, ref beta, y, incY);
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
                CBLAS.UpLo uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? CBLAS.UpLo.Upper : CBLAS.UpLo.Lower;
                _zher(CBLAS.Order.ColumnMajor, uplo, n, alpha, x, incX, a, lda);
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
                CBLAS.UpLo uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? CBLAS.UpLo.Upper : CBLAS.UpLo.Lower;
                _zher2(CBLAS.Order.ColumnMajor, uplo, n, ref alpha, x, incX, y, incY, a, lda);
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
                CBLAS.UpLo uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? CBLAS.UpLo.Upper : CBLAS.UpLo.Lower;
                _zhpmv(CBLAS.Order.ColumnMajor, uplo, n, ref alpha, aPacked, x, incX, ref beta, y, incY);
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
                CBLAS.UpLo uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? CBLAS.UpLo.Upper : CBLAS.UpLo.Lower;
                _zhpr(CBLAS.Order.ColumnMajor, uplo, n, alpha, x, incX, aPacked);
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
                CBLAS.UpLo uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? CBLAS.UpLo.Upper : CBLAS.UpLo.Lower;
                _zhpr2(CBLAS.Order.ColumnMajor, uplo, n, ref alpha, x, incX, y, incY, aPacked);
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
                CBLAS.UpLo uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? CBLAS.UpLo.Upper : CBLAS.UpLo.Lower;
                CBLAS.Diag diag = (isUnitTriangular == true) ? CBLAS.Diag.Unit : CBLAS.Diag.NonUnit;
                CBLAS.Transpose trans = transpose.AsCblasTranspose();

                _ztbmv(CBLAS.Order.ColumnMajor, uplo, trans, diag, n, k, a, lda, x, incX);
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
                CBLAS.UpLo uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? CBLAS.UpLo.Upper : CBLAS.UpLo.Lower;
                CBLAS.Diag diag = (isUnitTriangular == true) ? CBLAS.Diag.Unit : CBLAS.Diag.NonUnit;
                CBLAS.Transpose trans = transpose.AsCblasTranspose();

                _ztbsv(CBLAS.Order.ColumnMajor, uplo, trans, diag, n, k, a, lda, x, incX);
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
                CBLAS.UpLo uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? CBLAS.UpLo.Upper : CBLAS.UpLo.Lower;
                CBLAS.Diag diag = (isUnitTriangular == true) ? CBLAS.Diag.Unit : CBLAS.Diag.NonUnit;
                CBLAS.Transpose trans = transpose.AsCblasTranspose();

                _ztpmv(CBLAS.Order.ColumnMajor, uplo, trans, diag, n, aPacked, x, incX);
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
                CBLAS.UpLo uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? CBLAS.UpLo.Upper : CBLAS.UpLo.Lower;
                CBLAS.Diag diag = (isUnitTriangular == true) ? CBLAS.Diag.Unit : CBLAS.Diag.NonUnit;
                CBLAS.Transpose trans = transpose.AsCblasTranspose();

                _ztpsv(CBLAS.Order.ColumnMajor, uplo, trans, diag, n, aPacked, x, incX);
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
                CBLAS.UpLo uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? CBLAS.UpLo.Upper : CBLAS.UpLo.Lower;
                CBLAS.Diag diag = (isUnitTriangular == true) ? CBLAS.Diag.Unit : CBLAS.Diag.NonUnit;
                CBLAS.Transpose trans = transpose.AsCblasTranspose();

                _ztrmv(CBLAS.Order.ColumnMajor, uplo, trans, diag, n, a, lda, x, incX);
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
                CBLAS.UpLo uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? CBLAS.UpLo.Upper : CBLAS.UpLo.Lower;
                CBLAS.Diag diag = (isUnitTriangular == true) ? CBLAS.Diag.Unit : CBLAS.Diag.NonUnit;
                CBLAS.Transpose trans = transpose.AsCblasTranspose();

                _ztrsv(CBLAS.Order.ColumnMajor, uplo, trans, diag, n, a, lda, x, incX);
            }
            #endregion

            #endregion
        }
    }
}