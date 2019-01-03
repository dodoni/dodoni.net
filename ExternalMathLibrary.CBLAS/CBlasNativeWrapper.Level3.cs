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
    public partial class CBlasNativeWrapper
    {
        /// <summary>The wrapper for BLAS level 3 methods (C interface). See http://www.netlib.org/blas for further information.
        /// </summary>
        protected internal class Level3CBLAS : ILevel3BLAS
        {
            #region private function import

            #region double precision methods

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_dgemm", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dgemm(CBLAS.Order order, CBLAS.Transpose transA, CBLAS.Transpose transB, int m, int n, int k, double alpha, double[] a, int lda, double[] b, int ldb, double beta, double[] c, int ldc);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_dgemm", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static unsafe extern void _dgemm(CBLAS.Order order, CBLAS.Transpose transA, CBLAS.Transpose transB, int m, int n, int k, double alpha, double* a, int lda, double* b, int ldb, double beta, double* c, int ldc);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_dsymm", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dsymm(CBLAS.Order order, CBLAS.Side side, CBLAS.UpLo uplo, int m, int n, double alpha, double[] a, int lda, double[] b, int ldb, double beta, double[] c, int ldc);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_dsyrk", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dsyrk(CBLAS.Order order, CBLAS.UpLo uplo, CBLAS.Transpose trans, int n, int k, double alpha, double[] a, int lda, double beta, double[] c, int ldc);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_dsyr2k", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dsyr2k(CBLAS.Order order, CBLAS.UpLo uplo, CBLAS.Transpose trans, int n, int k, double alpha, double[] a, int lda, double[] b, int ldb, double beta, double[] c, int ldc);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_dtrmm", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dtrmm(CBLAS.Order order, CBLAS.Side side, CBLAS.UpLo uplo, CBLAS.Transpose trans, CBLAS.Diag diag, int m, int n, double alpha, double[] a, int lda, double[] b, int ldb);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_dtrsm", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dtrsm(CBLAS.Order order, CBLAS.Side side, CBLAS.UpLo uplo, CBLAS.Transpose trans, CBLAS.Diag diag, int m, int n, double alpha, double[] a, int lda, double[] b, int ldb);
            #endregion

            #region (double precision) complex methods

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_zgemm", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zgemm(CBLAS.Order order, CBLAS.Transpose transA, CBLAS.Transpose transB, int m, int n, int k, ref Complex alpha, [In, Out] Complex[] a, int lda, [In, Out] Complex[] b, int ldb, ref Complex beta, [In, Out] Complex[] c, int ldc);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_zhemm", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zhemm(CBLAS.Order order, CBLAS.Side side, CBLAS.UpLo uplo, int m, int n, ref Complex alpha, [In, Out] Complex[] a, int lda, [In, Out] Complex[] b, int ldb, ref Complex beta, [In, Out] Complex[] c, int ldc);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_zherk", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zherk(CBLAS.Order order, CBLAS.UpLo uplo, CBLAS.Transpose trans, int n, int k, double alpha, [In, Out] Complex[] a, int lda, double beta, [In, Out] Complex[] c, int ldc);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_zher2k", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zher2k(CBLAS.Order order, CBLAS.UpLo uplo, CBLAS.Transpose trans, int n, int k, ref Complex alpha, [In, Out] Complex[] a, int lda, [In, Out] Complex[] b, int ldb, double beta, [In, Out] Complex[] c, int ldc);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_zsymm", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zsymm(CBLAS.Order order, CBLAS.Side side, CBLAS.UpLo uplo, int m, int n, ref Complex alpha, [In, Out] Complex[] a, int lda, [In, Out] Complex[] b, int ldb, ref Complex beta, [In, Out] Complex[] c, int ldc);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_zsyrk", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zsyrk(CBLAS.Order order, CBLAS.UpLo uplo, CBLAS.Transpose trans, int n, int k, ref Complex alpha, [In, Out] Complex[] a, int lda, ref Complex beta, [In, Out] Complex[] c, int ldc);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_zsyr2k", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zsyr2k(CBLAS.Order order, CBLAS.UpLo uplo, CBLAS.Transpose trans, int n, int k, ref Complex alpha, [In, Out] Complex[] a, int lda, [In, Out] Complex[] b, int ldb, ref Complex beta, [In, Out] Complex[] c, int ldc);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_ztrmm", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _ztrmm(CBLAS.Order order, CBLAS.Side side, CBLAS.UpLo uplo, CBLAS.Transpose transa, CBLAS.Diag diag, int m, int n, ref Complex alpha, [In, Out] Complex[] a, int lda, [In, Out] Complex[] b, int ldb);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_ztrsm", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _ztrsm(CBLAS.Order order, CBLAS.Side side, CBLAS.UpLo uplo, CBLAS.Transpose transa, CBLAS.Diag diag, int m, int n, ref Complex alpha, [In, Out] Complex[] a, int lda, [In, Out] Complex[] b, int ldb);
            #endregion

            #endregion

            #region private members

            /// <summary>The managed BLAS level 3 implementation. Needed for some none-standard BLAS routines only.
            /// </summary>
            private BuildIn.BuildInLevel3BLAS m_BuildInLevel3Blas;
            #endregion

            #region public constructors

            /// <summary>Initializes a new instance of the <see cref="Level3CBLAS"/> class.
            /// </summary>
            public Level3CBLAS()
            {
                m_BuildInLevel3Blas = new BuildIn.BuildInLevel3BLAS();
            }
            #endregion

            #region ILevel3BLAS Members

            #region double precision methods

            /// <summary>Computes a matrix-matrix product with a general matrix, i.e. C := \alpha * op(A)*op(B) + \beta * C, where op(.) is the identity or the transpose operation.
            /// </summary>
            /// <param name="m">The number of rows of the matrix op(A) and of the matrix C.</param>
            /// <param name="n">The number of columns of the matrix op(B) and of the matrix C.</param>
            /// <param name="k">The number of columns of the matrix op(A) and the number of rows of the matrix op(B).</param>
            /// <param name="alpha">The scalar \alpha.</param>
            /// <param name="a">The matrix A supplied column-by-column of dimension (<paramref name="lda"/>, ka), where ka is <paramref name="k"/> if op(A) = A; <paramref name="m"/> otherwise.</param>
            /// <param name="b">The matrix B supplied column-by-column of dimension (<paramref name="ldb"/>, kb), where kb is <paramref name="n"/> if op(B) = B; <paramref name="k"/> otherwise.</param>
            /// <param name="beta">The scalar \beta.</param>
            /// <param name="c">The matrix C supplied column-by-column of dimension (<paramref name="ldc"/>, <paramref name="n"/>).</param>
            /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="m"/>) if op(A) = A; max(1, <paramref name="k"/>) otherwise.</param>
            /// <param name="ldb">The leading dimension of <paramref name="b"/>, must be at least max(1,<paramref name="k"/>) if op(B) = B; max(1, <paramref name="n"/>) otherwise.</param>
            /// <param name="ldc">The leading dimension of <paramref name="c"/>, must be at least max(1, <paramref name="m"/>).</param>
            /// <param name="transposeA">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
            /// <param name="transposeB">A value indicating whether 'op(B)=B' or 'op(B)=B^t'.</param>
            public void dgemm(int m, int n, int k, double alpha, double[] a, double[] b, double beta, double[] c, int lda, int ldb, int ldc, BLAS.MatrixTransposeState transposeA = BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState transposeB = BLAS.MatrixTransposeState.NoTranspose)
            {
                if (a.Length < lda * ((transposeA == BLAS.MatrixTransposeState.NoTranspose) ? k : m))
                {
                    throw new ArgumentException("a");
                }
                if (b.Length < ldb * ((transposeB == BLAS.MatrixTransposeState.NoTranspose) ? n : k))
                {
                    throw new ArgumentException("b");
                }
                if (c.Length < ldc * n)
                {
                    throw new ArgumentException("c");
                }
                _dgemm(CBLAS.Order.ColumnMajor, (transposeA == BLAS.MatrixTransposeState.NoTranspose) ? CBLAS.Transpose.NoTranspose : CBLAS.Transpose.Transpose, (transposeB == BLAS.MatrixTransposeState.NoTranspose) ? CBLAS.Transpose.NoTranspose : CBLAS.Transpose.Transpose, m, n, k, alpha, a, lda, b, ldb, beta, c, ldc);
            }

            /// <summary>Computes a matrix-matrix product with a general matrix, i.e. C := \alpha * op(A)*op(B) + \beta * C, where op(.) is the identity or the transpose operation.
            /// </summary>
            /// <param name="m">The number of rows of the matrix op(A) and of the matrix C.</param>
            /// <param name="n">The number of columns of the matrix op(B) and of the matrix C.</param>
            /// <param name="k">The number of columns of the matrix op(A) and the number of rows of the matrix op(B).</param>
            /// <param name="alpha">The scalar \alpha.</param>
            /// <param name="a">The matrix A supplied column-by-column of dimension (<paramref name="lda"/>, ka), where ka is <paramref name="k"/> if op(A) = A; <paramref name="m"/> otherwise.</param>
            /// <param name="b">The matrix B supplied column-by-column of dimension (<paramref name="ldb"/>, kb), where kb is <paramref name="n"/> if op(B) = B; <paramref name="k"/> otherwise.</param>
            /// <param name="beta">The scalar \beta.</param>
            /// <param name="c">The matrix C supplied column-by-column of dimension (<paramref name="ldc"/>, <paramref name="n"/>).</param>
            /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="m"/>) if op(A) = A; max(1, <paramref name="k"/>) otherwise.</param>
            /// <param name="ldb">The leading dimension of <paramref name="b"/>, must be at least max(1,<paramref name="k"/>) if op(B) = B; max(1, <paramref name="n"/>) otherwise.</param>
            /// <param name="ldc">The leading dimension of <paramref name="c"/>, must be at least max(1, <paramref name="m"/>).</param>
            /// <param name="transposeA">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
            /// <param name="transposeB">A value indicating whether 'op(B)=B' or 'op(B)=B^t'.</param>
            /// <param name="startIndexA">The null-based start index for <paramref name="a"/></param>
            /// <param name="startIndexB">The null-based start index for <paramref name="b"/></param>
            /// <param name="startIndexC">The null-based start index for <paramref name="c"/></param>
            public void dgemm(int m, int n, int k, double alpha, double[] a, double[] b, double beta, double[] c, int lda, int ldb, int ldc, int startIndexA, int startIndexB, BLAS.MatrixTransposeState transposeA = BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState transposeB = BLAS.MatrixTransposeState.NoTranspose, int startIndexC = 0)
            {
                if (a.Length < lda * ((transposeA == BLAS.MatrixTransposeState.NoTranspose) ? k : m))
                {
                    throw new ArgumentException("a");
                }
                if (b.Length < ldb * ((transposeB == BLAS.MatrixTransposeState.NoTranspose) ? n : k))
                {
                    throw new ArgumentException("b");
                }
                if (c.Length < ldc * n)
                {
                    throw new ArgumentException("c");
                }
                unsafe
                {
                    fixed (double* ptrC = &c[startIndexC], ptrA = &a[startIndexA], ptrB = &b[startIndexB])
                    {
                        _dgemm(CBLAS.Order.ColumnMajor, (transposeA == BLAS.MatrixTransposeState.NoTranspose) ? CBLAS.Transpose.NoTranspose : CBLAS.Transpose.Transpose, (transposeB == BLAS.MatrixTransposeState.NoTranspose) ? CBLAS.Transpose.NoTranspose : CBLAS.Transpose.Transpose, m, n, k, alpha, ptrA, lda, ptrB, ldb, beta, ptrC, ldc);
                    }
                }
            }

            /// <summary>Computes a matrix-matrix product where one input matrix is symmetric, i.e. C := \alpha*A*B + \beta*C or C := \alpha*B*A +\beta*C.
            /// </summary>
            /// <param name="m">The number of rows of the matrix C.</param>
            /// <param name="n">The number of columns of the matrix C.</param>
            /// <param name="alpha">The scalar \alpha.</param>
            /// <param name="a">The symmetric matrix A supplied column-by-column of dimension (<paramref name="lda"/>, ka), where ka is <paramref name="m"/> if to calculate C := \alpha * A*B + \beta*C; otherwise <paramref name="n"/>.</param>
            /// <param name="b">The matrix B supplied column-by-column of dimension (<paramref name="ldb"/>,<paramref name="n"/>).</param>
            /// <param name="beta">The scalar \beta.</param>
            /// <param name="c">The matrix C supplied column-by-column of dimension (<paramref name="ldc"/>,<paramref name="n"/>); input/output.</param>
            /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="m"/>) if <paramref name="side"/>=left; max(1,n) otherwise.</param>
            /// <param name="ldb">The leading dimension of <paramref name="b"/>, must be at least max(1,<paramref name="m"/>).</param>
            /// <param name="ldc">The leading dimension of <paramref name="c"/>, must be at least max(1,<paramref name="m"/>).</param>
            /// <param name="side">A value indicating whether to calculate C := \alpha * A*B + \beta*C or C := \alpha * B*A +\beta*C.</param>
            /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
            public void dsymm(int m, int n, double alpha, double[] a, double[] b, double beta, double[] c, int lda, int ldb, int ldc, BLAS.Side side = BLAS.Side.Left, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix)
            {
                CBLAS.UpLo uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? CBLAS.UpLo.Upper : CBLAS.UpLo.Lower;

                _dsymm(CBLAS.Order.ColumnMajor, (side == BLAS.Side.Left) ? CBLAS.Side.Left : CBLAS.Side.Right, uplo, m, n, alpha, a, lda, b, ldb, beta, c, ldc);
            }

            /// <summary>Performs a symmetric rank-k update, i.e. C:= \alpha*A*A^t + \beta *C or C:= \alpha*A^t*A + \beta*C.
            /// </summary>
            /// <param name="n">The order of matrix C.</param>
            /// <param name="k">The number of columns of matrix A if to calculate C:= \alpha*A*A^t + \beta *C; otherwise the number of rows of matrix A.</param>
            /// <param name="alpha">The scalar \alpha.</param>
            /// <param name="a">The matrix A supplied column-by-column of dimension (<paramref name="lda"/>, ka), where ka is <paramref name="k"/> if to calculate C:= \alpha*A*A^t + \beta *C; otherwise <paramref name="n"/>.</param>
            /// <param name="beta">The scalar \beta.</param>
            /// <param name="c">The symmetric matrix C supplied column-by-column of dimension (<paramref name="ldc"/>, <paramref name="n"/>).</param>
            /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="n"/>) if to calculate C:= \alpha*A*A^t + \beta *C; max(1,<paramref name="k"/>) otherwise.</param>
            /// <param name="ldc">The leading dimension of <paramref name="c"/>, must be at least max(1,<paramref name="n"/>).</param>
            /// <param name="triangularMatrixType">A value whether matrix C is in its upper or lower triangular representation.</param>
            /// <param name="operation">A value indicating whether to calculate C:= \alpha*A*A^t + \beta *C or C:= \alpha*A^t*A + \beta*C.</param>
            public void dsyrk(int n, int k, double alpha, double[] a, double beta, double[] c, int lda, int ldc, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.XsyrkOperation operation = BLAS.XsyrkOperation.ATimesATranspose)
            {
                CBLAS.UpLo uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? CBLAS.UpLo.Upper : CBLAS.UpLo.Lower;

                _dsyrk(CBLAS.Order.ColumnMajor, uplo, (operation == BLAS.XsyrkOperation.ATimesATranspose) ? CBLAS.Transpose.NoTranspose : CBLAS.Transpose.Transpose, n, k, alpha, a, lda, beta, c, ldc);
            }

            /// <summary>Performs a symmetric rank-2k update, i.e. C := alpha*A*B^t + alpha*B*A^t + beta*C or C := alpha*A^t*B + alpha*B^t*A + beta*C with a symmetric matrix C.
            /// </summary>
            /// <param name="n">The order of matrix C.</param>
            /// <param name="k">The The number of columns of matrices A and B or the number .</param>
            /// <param name="alpha">The scalar \alpha.</param>
            /// <param name="a">The matrix A supplied column-by-column of dimension (<paramref name="lda"/>, ka), where ka is <paramref name="k"/> if to calculate C := alpha*A*B^t + alpha*B*A^t + beta*C; otherwise <paramref name="n"/>.</param>
            /// <param name="b">The matrix B supplied column-by-column of dimension (<paramref name="ldb"/>, kb), where ka is at least max(1,<paramref name="n"/>) if to calculate C := alpha*A*B^t + alpha*B*A^t + beta*C; otherwise at least max(1,<paramref name="k"/>).</param>
            /// <param name="beta">The scalar \beta.</param>
            /// <param name="c">The symmetric matrix C supplied column-by-column of dimension (<paramref name="ldc"/>, <paramref name="n"/>).</param>
            /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="n"/>) if to calculate C:= alpha*A*B^t+alpha*B*A^t+beta*C; max(1,<paramref name="k"/>) otherwise.</param>
            /// <param name="ldb">The leading dimension of <paramref name="b"/>, must be at least max(1,<paramref name="n"/>) if to calculate C:= alpha*A*B^t+alpha*B*A^t+beta*C; max(1,<paramref name="k"/>) otherwise.</param>
            /// <param name="ldc">The leading dimension of <paramref name="c"/>, must be at least max(1,<paramref name="n"/>).</param>
            /// <param name="triangularMatrixType">A value whether matrix C is in its upper or lower triangular representation.</param>
            /// <param name="operation">A value indicating whether to calculate C := alpha*A*B^t + alpha*B*A^t + beta*C or C := alpha*A^t*B + alpha*B^t*A + beta*C.</param>
            public void dsyr2k(int n, int k, double alpha, double[] a, double[] b, double beta, double[] c, int lda, int ldb, int ldc, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.Xsyr2kOperation operation = BLAS.Xsyr2kOperation.ATimesBTransPlusBTimesATrans)
            {
                CBLAS.UpLo uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? CBLAS.UpLo.Upper : CBLAS.UpLo.Lower;

                _dsyr2k(CBLAS.Order.ColumnMajor, uplo, (operation == BLAS.Xsyr2kOperation.ATimesBTransPlusBTimesATrans) ? CBLAS.Transpose.NoTranspose : CBLAS.Transpose.Transpose, n, k, alpha, a, lda, b, ldb, beta, c, ldc);
            }

            /// <summary>Computes a matrix-matrix product where one input matrix is triangular, i.e. B := \alpha * op(A)*B or B:= \alpha *B * op(A), where A is a unit or non-unit upper or lower triangular matrix.
            /// </summary>
            /// <param name="m">The number of rows of matrix B.</param>
            /// <param name="n">The number of columns of matrix B.</param>
            /// <param name="alpha">The scalar \alpha.</param>
            /// <param name="a">The triangular matrix A supplied column-by-column of dimension (<paramref name="lda"/>, k), where k is <paramref name="m"/> if to calculate B := \alpha * op(A)*B; <paramref name="n"/> otherwise.</param>
            /// <param name="b">The matrix B supplied column-by-column of dimension (<paramref name="ldb"/>, <paramref name="n"/>).</param>
            /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="m"/>) if to calculate B := \alpha * op(A)*B; max(1,<paramref name="n"/>) otherwise.</param>
            /// <param name="ldb">The leading dimension of <paramref name="b"/>, must be at least max(1,<paramref name="m"/>).</param>
            /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
            /// <param name="side">A value indicating whether to calculate B := \alpha * op(A)*B or B:= \alpha *B * op(A).</param>
            /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
            /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
            public void dtrmm(int m, int n, double alpha, double[] a, double[] b, int lda, int ldb, bool isUnitTriangular = true, BLAS.Side side = BLAS.Side.Left, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose)
            {
                CBLAS.UpLo uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? CBLAS.UpLo.Upper : CBLAS.UpLo.Lower;
                CBLAS.Diag diag = (isUnitTriangular == true) ? CBLAS.Diag.Unit : CBLAS.Diag.NonUnit;
                CBLAS.Transpose transA = (transpose == BLAS.MatrixTransposeState.NoTranspose) ? CBLAS.Transpose.NoTranspose : CBLAS.Transpose.Transpose;

                _dtrmm(CBLAS.Order.ColumnMajor, (side == BLAS.Side.Left) ? CBLAS.Side.Left : CBLAS.Side.Right, uplo, transA, diag, m, n, alpha, a, lda, b, ldb);
            }

            /// <summary>Solves a triangular matrix equation, i.e. op(A) * X = \alpha * B or X * op(A) = \alpha *B, where A is a unit or non-unit upper or lower triangular matrix.
            /// </summary>
            /// <param name="m">The number of rows of matrix B.</param>
            /// <param name="n">The number of column of matrix B.</param>
            /// <param name="alpha">The scalar \alpha.</param>
            /// <param name="a">The triangular matrix A supplied column-by-column of dimension (<paramref name="lda"/>, k), where k is <paramref name="m"/> if to calculate op(A) * X = \alpha * B; <paramref name="n"/> otherwise.</param>
            /// <param name="b">The matrix B supplied column-by-column of dimension (<paramref name="ldb"/>, <paramref name="n"/>).</param>
            /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="m"/>) if to calculate op(A) * X = \alpha * B; max(1,<paramref name="n"/>) otherwise.</param>
            /// <param name="ldb">The leading dimension of <paramref name="b"/>, must be at least max(1,<paramref name="m"/>).</param>
            /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
            /// <param name="side">A value indicating whether to calculate op(A) * X = \alpha * B or X * op(A) = \alpha *B.</param>
            /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
            /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
            public void dtrsm(int m, int n, double alpha, double[] a, double[] b, int lda, int ldb, bool isUnitTriangular = true, BLAS.Side side = BLAS.Side.Left, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose)
            {
                CBLAS.UpLo uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? CBLAS.UpLo.Upper : CBLAS.UpLo.Lower;
                CBLAS.Diag diag = (isUnitTriangular == true) ? CBLAS.Diag.Unit : CBLAS.Diag.NonUnit;
                CBLAS.Transpose transA = (transpose == BLAS.MatrixTransposeState.NoTranspose) ? CBLAS.Transpose.NoTranspose : CBLAS.Transpose.Transpose;

                _dtrsm(CBLAS.Order.ColumnMajor, (side == BLAS.Side.Left) ? CBLAS.Side.Left : CBLAS.Side.Right, uplo, transA, diag, m, n, alpha, a, lda, b, ldb);
            }

            /// <summary>Gets a optimal workspace array length for the <c>aux_dgetrans</c> function.
            /// </summary>
            /// <param name="rowCount">The number of rows.</param>
            /// <param name="columnCount">The number of columns.</param>
            /// <returns>The optimal workspace array length.</returns>
            public virtual int aux_dgetransQuery(int rowCount, int columnCount)
            {
                return m_BuildInLevel3Blas.aux_dgetransQuery(rowCount, columnCount);
            }

            /// <summary>Performs in-place transposition of a specific matrix.
            /// </summary>
            /// <param name="rowCount">The number of rows.</param>
            /// <param name="columnCount">The number of columns.</param>
            /// <param name="a">The matrix provided column-by-column (column-major ordering).</param>
            /// <param name="work">A workspace array.</param>
            public virtual void aux_dgetrans(int rowCount, int columnCount, double[] a, double[] work = null)
            {
                m_BuildInLevel3Blas.aux_dgetrans(rowCount, columnCount, a, work);
            }
            #endregion

            #region (double precision) complex methods

            /// <summary>Computes a matrix-matrix product with a general matrix, i.e. C := \alpha * op(A)*op(B) + \beta * C, where where op(.) is the identity or the transpose operation.
            /// </summary>
            /// <param name="m">The number of rows of the matrix op(A) and of the matrix C.</param>
            /// <param name="n">The number of columns of the matrix op(B) and of the matrix C.</param>
            /// <param name="k">The number of columns of the matrix op(A) and the number of rows of the matrix op(B).</param>
            /// <param name="alpha">The scalar \alpha.</param>
            /// <param name="a">The matrix A supplied column-by-column of dimension (<paramref name="lda"/>, ka), where ka is <paramref name="k"/> if op(A) = A; <paramref name="m"/> otherwise.</param>
            /// <param name="b">The matrix B supplied column-by-column of dimension (<paramref name="ldb"/>, kb), where kb is <paramref name="n"/> if op(B) = B; <paramref name="k"/> otherwise.</param>
            /// <param name="beta">The scalar \beta.</param>
            /// <param name="c">The matrix C supplied column-by-column of dimension (<paramref name="ldc"/>, <paramref name="n"/>).</param>
            /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="m"/>) if op(A) = A; max(1, <paramref name="k"/>) otherwise.</param>
            /// <param name="ldb">The leading dimension of <paramref name="b"/>, must be at least max(1,<paramref name="k"/>) if op(B) = B; max(1, <paramref name="n"/>) otherwise.</param>
            /// <param name="ldc">The leading dimension of <paramref name="c"/>, must be at least max(1, <paramref name="m"/>).</param>
            /// <param name="transposeA">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
            /// <param name="transposeB">A value indicating whether 'op(B)=B' or 'op(B)=B^t'.</param>
            /// <remarks>This function is not part of the BLAS level 3 standard, but supported by the Intel MKL library for example.</remarks>
            public virtual void dzgemm(int m, int n, int k, Complex alpha, double[] a, Complex[] b, Complex beta, Complex[] c, int lda, int ldb, int ldc, BLAS.MatrixTransposeState transposeA = BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState transposeB = BLAS.MatrixTransposeState.NoTranspose)
            {
                Complex[] tempA = new Complex[a.Length];
                for (int j = 0; j < a.Length; j++)
                {
                    tempA[j] = new Complex(a[j], 0.0);
                }
                zgemm(m, n, k, alpha, tempA, b, beta, c, lda, ldb, ldc, transposeA, transposeB);
            }

            /// <summary>Computes a matrix-matrix product with a general matrix, i.e. C := \alpha * op(A)*op(B) + \beta * C,
            /// where where op(.) is the identity or the transpose operation.
            /// </summary>
            /// <param name="m">The number of rows of the matrix op(A) and of the matrix C.</param>
            /// <param name="n">The number of columns of the matrix op(B) and of the matrix C.</param>
            /// <param name="k">The number of columns of the matrix op(A) and the number of rows of the matrix op(B).</param>
            /// <param name="alpha">The scalar \alpha.</param>
            /// <param name="a">The matrix A supplied column-by-column of dimension (<paramref name="lda"/>, ka), where ka is <paramref name="k"/> if op(A) = A; <paramref name="m"/> otherwise.</param>
            /// <param name="b">The matrix B supplied column-by-column of dimension (<paramref name="ldb"/>, kb), where kb is <paramref name="n"/> if op(B) = B; <paramref name="k"/> otherwise.</param>
            /// <param name="beta">The scalar \beta.</param>
            /// <param name="c">The matrix C supplied column-by-column of dimension (<paramref name="ldc"/>, <paramref name="n"/>).</param>
            /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="m"/>) if op(A) = A; max(1, <paramref name="k"/>) otherwise.</param>
            /// <param name="ldb">The leading dimension of <paramref name="b"/>, must be at least max(1,<paramref name="k"/>) if op(B) = B; max(1, <paramref name="n"/>) otherwise.</param>
            /// <param name="ldc">The leading dimension of <paramref name="c"/>, must be at least max(1, <paramref name="m"/>).</param>
            /// <param name="transposeA">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
            /// <param name="transposeB">A value indicating whether 'op(B)=B' or 'op(B)=B^t'.</param>
            public void zgemm(int m, int n, int k, Complex alpha, Complex[] a, Complex[] b, Complex beta, Complex[] c, int lda, int ldb, int ldc, BLAS.MatrixTransposeState transposeA = BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState transposeB = BLAS.MatrixTransposeState.NoTranspose)
            {
                if (a.Length < lda * ((transposeA == BLAS.MatrixTransposeState.NoTranspose) ? k : m))
                {
                    throw new ArgumentException("a");
                }
                if (b.Length < ldb * ((transposeB == BLAS.MatrixTransposeState.NoTranspose) ? n : k))
                {
                    throw new ArgumentException("b");
                }
                if (c.Length < ldc * n)
                {
                    throw new ArgumentException("c");
                }
                _zgemm(CBLAS.Order.ColumnMajor, transposeA.AsCblasTranspose(), transposeB.AsCblasTranspose(), m, n, k, ref alpha, a, lda, b, ldb, ref beta, c, ldc);
            }

            /// <summary>Computes a matrix-matrix product where one input matrix is Hermitian, i.e. C := \alpha*A*B + \beta*C or C := \alpha*B*A + \beta*C, where A is a Hermitian matrix.
            /// </summary>
            /// <param name="m">The number of rows of matrix C.</param>
            /// <param name="n">The number of columns of matrix C.</param>
            /// <param name="alpha">The scalar \alpha.</param>
            /// <param name="a">The Hermitian matrix A supplied column-by-column of dimension (<paramref name="ldc"/>, ka), where ka is <paramref name="m"/> if to calculate C := \alpha*A*B + \beta*C; <paramref name="n"/> otherwise.</param>
            /// <param name="b">The matrix B supplied column-by-column of dimension (<paramref name="ldb"/>, <paramref name="n"/>).</param>
            /// <param name="beta">The scalar \beta.</param>
            /// <param name="c">The matrix C supplied column-by-column of dimension (<paramref name="ldc"/>, <paramref name="n"/>).</param>
            /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="m"/>) if to calculate C := \alpha*A*B + \beta*C; max(1, <paramref name="n"/>) otherwise.</param>
            /// <param name="ldb">The leading dimension of <paramref name="b"/>, must be at least max(1,<paramref name="m"/>).</param>
            /// <param name="ldc">The leading dimension of <paramref name="c"/>, must be at least max(1, <paramref name="m"/>).</param>
            /// <param name="side">A value indicating whether to calculate C := \alpha*A*B + \beta*C or C := \alpha*B*A + \beta*C.</param>
            /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
            public void zhemm(int m, int n, Complex alpha, Complex[] a, Complex[] b, Complex beta, Complex[] c, int lda, int ldb, int ldc, BLAS.Side side = BLAS.Side.Left, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix)
            {
                CBLAS.UpLo uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? CBLAS.UpLo.Upper : CBLAS.UpLo.Lower;

                _zhemm(CBLAS.Order.ColumnMajor, (side == BLAS.Side.Left) ? CBLAS.Side.Left : CBLAS.Side.Right, uplo, m, n, ref alpha, a, lda, b, ldb, ref beta, c, ldc);
            }

            /// <summary>Performs a Hermitian rank-k update, i.e. C := \alpha * A * A^h + \beta*C or C := alpha*A^h * A + beta*C, where C is a Hermitian matrix.
            /// </summary>
            /// <param name="n">The order of matrix C.</param>
            /// <param name="k">The number of columns of matrix A if to calculate C := \alpha * A * A^h + \beta*C; otherwise the number of rows of matrix A.</param>
            /// <param name="alpha">The scalar \alpha.</param>
            /// <param name="a">The matrix A supplied column-by-column of dimension (<paramref name="lda"/>, ka), where ka equals to <paramref name="k"/> if to calculate C := \alpha * A * A^h + \beta*C; <paramref name="n"/> otherwise.</param>
            /// <param name="beta">The scalar \beta.</param>
            /// <param name="c">The Hermitian matrix C supplied column-by-column of dimension (<paramref name="ldc"/>, <paramref name="n"/>).</param>
            /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="n"/>) if to calculate  C := \alpha * A * A^h + \beta*C ; max(1, <paramref name="k"/>) otherwise.</param>
            /// <param name="ldc">The leading dimension of <paramref name="c"/>, must be at least max(1, <paramref name="n"/>).</param>
            /// <param name="triangularMatrixType">A value whether matrix C is in its upper or lower triangular representation.</param>
            /// <param name="operation">A value indicating whether to calculate C := \alpha * A * A^h + \beta*C or C := alpha*A^h * A + beta*C.</param>
            public void zherk(int n, int k, double alpha, Complex[] a, double beta, Complex[] c, int lda, int ldc, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.ZherkOperation operation = BLAS.ZherkOperation.ATimesAHermite)
            {
                CBLAS.Transpose trans = (operation == BLAS.ZherkOperation.ATimesAHermite) ? CBLAS.Transpose.NoTranspose : CBLAS.Transpose.ConjugateTranspose;
                CBLAS.UpLo uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? CBLAS.UpLo.Upper : CBLAS.UpLo.Lower;

                _zherk(CBLAS.Order.ColumnMajor, uplo, trans, n, k, alpha, a, lda, beta, c, ldc);
            }

            /// <summary>Performs a Hermitian rank-2 update, i.e. C := \alpha*A*B^h + conjg(\alpha)*B*A^h + \beta * C or C := \alpha*B^h*A + conjg(\alpha)*A^h*B + beta*C, where C is an Hermitian matrix.
            /// </summary>
            /// <param name="n">The order of matrix C.</param>
            /// <param name="k">The number of columns of matrix A if to calculate C := \alpha*A*B^h + conjg(\alpha)*B*A^h + \beta * C; the number of rows of the matrix A otherwise.</param>
            /// <param name="alpha">The scalar \alpha.</param>
            /// <param name="a">The matrix A supplied column-by-column of dimension (<paramref name="lda"/>, ka), where ka equals to <paramref name="k"/> if to calculate C := \alpha*A*B^h + conjg(\alpha)*B*A^h + \beta * C; <paramref name="n"/> otherwise.</param>
            /// <param name="b">The matrix B supplied column-by-column of dimension (<paramref name="ldb"/>, kb), where kb equals to <paramref name="k"/> if to calculate C := \alpha*A*B^h + conjg(\alpha)*B*A^h + \beta * C; <paramref name="n"/> otherwise.</param>
            /// <param name="beta">The scalar \beta.</param>
            /// <param name="c">The Hermitian matrix C supplied column-by-column of dimension (<paramref name="ldc"/>, <paramref name="n"/>).</param>
            /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="n"/>) if to calculate C := \alpha*A*B^h + conjg(\alpha)*B*A^h + \beta * C; max(1, <paramref name="k"/>) otherwise.</param>
            /// <param name="ldb">The leading dimension of <paramref name="b"/>, must be at least max(1,<paramref name="n"/>) if to calculate C := \alpha*A*B^h + conjg(\alpha)*B*A^h + \beta * C; max(1, <paramref name="k"/>) otherwise.</param>
            /// <param name="ldc">The leading dimension of <paramref name="c"/>, must be at least max(1, <paramref name="n"/>).</param>
            /// <param name="triangularMatrixType">A value whether matrix C is in its upper or lower triangular representation.</param>
            /// <param name="operation">A value indicating whether to calculate C := \alpha*A*B^h + conjg(\alpha)*B*A^h + \beta * C or C := \alpha*B^h*A + conjg(\alpha)*A^h*B + beta*C.</param>
            public void zher2k(int n, int k, Complex alpha, Complex[] a, Complex[] b, double beta, Complex[] c, int lda, int ldb, int ldc, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.Zher2kOperation operation = BLAS.Zher2kOperation.ATimesBHermitePlusBTimesAHermite)
            {
                CBLAS.Transpose trans = (operation == BLAS.Zher2kOperation.ATimesBHermitePlusBTimesAHermite) ? CBLAS.Transpose.NoTranspose : CBLAS.Transpose.ConjugateTranspose;
                CBLAS.UpLo uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? CBLAS.UpLo.Upper : CBLAS.UpLo.Lower;

                _zher2k(CBLAS.Order.ColumnMajor, uplo, trans, n, k, ref alpha, a, lda, b, ldb, beta, c, ldc);
            }

            /// <summary>Computes a matrix-matrix product where one input matrix is symmetric, i.e. C := \alpha*A*B + \beta*C or C := \alpha*B*A +\beta*C.
            /// </summary>
            /// <param name="m">The number of rows of the matrix C.</param>
            /// <param name="n">The number of columns of the matrix C.</param>
            /// <param name="alpha">The scalar \alpha.</param>
            /// <param name="a">The symmetric matrix A supplied column-by-column of dimension (<paramref name="lda"/>, ka), where ka is <paramref name="m"/> if to calculate C := \alpha * A*B + \beta*C; otherwise <paramref name="n"/>.</param>
            /// <param name="b">The matrix B supplied column-by-column of dimension (<paramref name="ldb"/>,<paramref name="n"/>).</param>
            /// <param name="beta">The scalar \beta.</param>
            /// <param name="c">The matrix C supplied column-by-column of dimension (<paramref name="ldc"/>,<paramref name="n"/>); input/output.</param>
            /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="m"/>) if <paramref name="side"/>=left; max(1,n) otherwise.</param>
            /// <param name="ldb">The leading dimension of <paramref name="b"/>, must be at least max(1,<paramref name="m"/>).</param>
            /// <param name="ldc">The leading dimension of <paramref name="c"/>, must be at least max(1,<paramref name="m"/>).</param>
            /// <param name="side">A value indicating whether to calculate C := \alpha * A*B + \beta*C or C := \alpha * B*A +\beta*C.</param>
            /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
            public void zsymm(int m, int n, Complex alpha, Complex[] a, Complex[] b, Complex beta, Complex[] c, int lda, int ldb, int ldc, BLAS.Side side = BLAS.Side.Left, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix)
            {
                CBLAS.UpLo uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? CBLAS.UpLo.Upper : CBLAS.UpLo.Lower;

                _zsymm(CBLAS.Order.ColumnMajor, (side == BLAS.Side.Left) ? CBLAS.Side.Left : CBLAS.Side.Right, uplo, m, n, ref alpha, a, lda, b, ldb, ref beta, c, ldc);
            }

            /// <summary>Performs a symmetric rank-k update, i.e. C:= \alpha*A*A^t + \beta *C or C:= \alpha*A^t*A + \beta*C.
            /// </summary>
            /// <param name="n">The order of matrix C.</param>
            /// <param name="k">The number of columns of matrix A if to calculate C:= \alpha*A*A^t + \beta *C; otherwise the number of rows of matrix A.</param>
            /// <param name="alpha">The scalar \alpha.</param>
            /// <param name="a">The matrix A supplied column-by-column of dimension (<paramref name="lda"/>, ka), where ka is <paramref name="k"/> if to calculate C:= \alpha*A*A^t + \beta *C; otherwise <paramref name="n"/>.</param>
            /// <param name="beta">The scalar \beta.</param>
            /// <param name="c">The symmetric matrix C supplied column-by-column of dimension (<paramref name="ldc"/>, <paramref name="n"/>).</param>
            /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="n"/>) if to calculate C:= \alpha*A*A^t + \beta *C; max(1,<paramref name="k"/>) otherwise.</param>
            /// <param name="ldc">The leading dimension of <paramref name="c"/>, must be at least max(1,<paramref name="n"/>).</param>
            /// <param name="triangularMatrixType">A value whether matrix C is in its upper or lower triangular representation.</param>
            /// <param name="transpose">A value indicating whether to calculate C:= \alpha*A*A^t + \beta *C or C:= \alpha*A^t*A + \beta*C.</param>
            public void zsyrk(int n, int k, Complex alpha, Complex[] a, Complex beta, Complex[] c, int lda, int ldc, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.XsyrkOperation transpose = BLAS.XsyrkOperation.ATimesATranspose)
            {
                CBLAS.UpLo uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? CBLAS.UpLo.Upper : CBLAS.UpLo.Lower;

                _zsyrk(CBLAS.Order.ColumnMajor, uplo, (transpose == BLAS.XsyrkOperation.ATimesATranspose) ? CBLAS.Transpose.NoTranspose : CBLAS.Transpose.Transpose, n, k, ref alpha, a, lda, ref beta, c, ldc);
            }

            /// <summary>Performs a symmetric rank-2k update, i.e. C := alpha*A*B^t + alpha*B*A^t + beta*C or C := alpha*A^t*B + alpha*B^t*A + beta*C with a symmetric matrix C.
            /// </summary>
            /// <param name="n">The order of matrix C.</param>
            /// <param name="k">The The number of columns of matrices A and B or the number .</param>
            /// <param name="alpha">The scalar \alpha.</param>
            /// <param name="a">The matrix A supplied column-by-column of dimension (<paramref name="lda"/>, ka), where ka is <paramref name="k"/> if to calculate C := alpha*A*B^t + alpha*B*A^t + beta*C; otherwise <paramref name="n"/>.</param>
            /// <param name="b">The matrix B supplied column-by-column of dimension (<paramref name="ldb"/>, kb), where ka is at least max(1,<paramref name="n"/>) if to calculate C := alpha*A*B^t + alpha*B*A^t + beta*C; otherwise at least max(1,<paramref name="k"/>).</param>
            /// <param name="beta">The scalar \beta.</param>
            /// <param name="c">The symmetric matrix C supplied column-by-column of dimension (<paramref name="ldc"/>, <paramref name="n"/>).</param>
            /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="n"/>) if to calculate C:= alpha*A*B^t+alpha*B*A^t+beta*C; max(1,<paramref name="k"/>) otherwise.</param>
            /// <param name="ldb">The leading dimension of <paramref name="b"/>, must be at least max(1,<paramref name="n"/>) if to calculate C:= alpha*A*B^t+alpha*B*A^t+beta*C; max(1,<paramref name="k"/>) otherwise.</param>
            /// <param name="ldc">The leading dimension of <paramref name="c"/>, must be at least max(1,<paramref name="n"/>).</param>
            /// <param name="triangularMatrixType">A value whether matrix C is in its upper or lower triangular representation.</param>
            /// <param name="transpose">A value indicating whether to calculate C := alpha*A*B^t + alpha*B*A^t + beta*C or C := alpha*A^t*B + alpha*B^t*A + beta*C.</param>
            public void zsyr2k(int n, int k, Complex alpha, Complex[] a, Complex[] b, Complex beta, Complex[] c, int lda, int ldb, int ldc, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.Xsyr2kOperation transpose = BLAS.Xsyr2kOperation.ATimesBTransPlusBTimesATrans)
            {
                CBLAS.UpLo uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? CBLAS.UpLo.Upper : CBLAS.UpLo.Lower;

                _zsyr2k(CBLAS.Order.ColumnMajor, uplo, (transpose == BLAS.Xsyr2kOperation.ATimesBTransPlusBTimesATrans) ? CBLAS.Transpose.NoTranspose : CBLAS.Transpose.Transpose, n, k, ref alpha, a, lda, b, ldb, ref beta, c, ldc);
            }

            /// <summary>Computes a matrix-matrix product where one input matrix is triangular, i.e. B := \alpha * op(A)*B or B:= \alpha *B * op(A), where A is a unit or non-unit upper or lower triangular matrix.
            /// </summary>
            /// <param name="m">The number of rows of matrix B.</param>
            /// <param name="n">The number of columns of matrix B.</param>
            /// <param name="alpha">The scalar \alpha.</param>
            /// <param name="a">The triangular matrix A supplied column-by-column of dimension (<paramref name="lda"/>, k), where k is <paramref name="m"/> if to calculate B := \alpha * op(A)*B; <paramref name="n"/> otherwise.</param>
            /// <param name="b">The matrix B supplied column-by-column of dimension (<paramref name="ldb"/>, <paramref name="n"/>).</param>
            /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="m"/>) if to calculate B := \alpha * op(A)*B; max(1,<paramref name="n"/>) otherwise.</param>
            /// <param name="ldb">The leading dimension of <paramref name="b"/>, must be at least max(1,<paramref name="m"/>).</param>
            /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
            /// <param name="side">A value indicating whether to calculate B := \alpha * op(A)*B or B:= \alpha *B * op(A).</param>
            /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
            /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
            public void ztrmm(int m, int n, Complex alpha, Complex[] a, Complex[] b, int lda, int ldb, bool isUnitTriangular = true, BLAS.Side side = BLAS.Side.Left, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose)
            {
                CBLAS.UpLo uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? CBLAS.UpLo.Upper : CBLAS.UpLo.Lower;
                CBLAS.Diag diag = (isUnitTriangular == true) ? CBLAS.Diag.Unit : CBLAS.Diag.NonUnit;
                CBLAS.Transpose transA = transpose.AsCblasTranspose();

                _ztrmm(CBLAS.Order.ColumnMajor, (side == BLAS.Side.Left) ? CBLAS.Side.Left : CBLAS.Side.Right, uplo, transA, diag, m, n, ref alpha, a, lda, b, ldb);
            }

            /// <summary>Solves a triangular matrix equation, i.e. op(A) * X = \alpha * B or X * op(A) = \alpha *B, where A is a unit or non-unit upper or lower triangular matrix.
            /// </summary>
            /// <param name="m">The number of rows of matrix B.</param>
            /// <param name="n">The number of column of matrix B.</param>
            /// <param name="alpha">The scalar \alpha.</param>
            /// <param name="a">The triangular matrix A supplied column-by-column of dimension (<paramref name="lda"/>, k), where k is <paramref name="m"/> if to calculate op(A) * X = \alpha * B; <paramref name="n"/> otherwise.</param>
            /// <param name="b">The matrix B supplied column-by-column of dimension (<paramref name="ldb"/>, <paramref name="n"/>).</param>
            /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="m"/>) if to calculate op(A) * X = \alpha * B; max(1,<paramref name="n"/>) otherwise.</param>
            /// <param name="ldb">The leading dimension of <paramref name="b"/>, must be at least max(1,<paramref name="m"/>).</param>
            /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
            /// <param name="side">A value indicating whether to calculate op(A) * X = \alpha * B or X * op(A) = \alpha *B.</param>
            /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
            /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
            public void ztrsm(int m, int n, Complex alpha, Complex[] a, Complex[] b, int lda, int ldb, bool isUnitTriangular = true, BLAS.Side side = BLAS.Side.Left, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose)
            {
                CBLAS.UpLo uplo = (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix) ? CBLAS.UpLo.Upper : CBLAS.UpLo.Lower;
                CBLAS.Diag diag = (isUnitTriangular == true) ? CBLAS.Diag.Unit : CBLAS.Diag.NonUnit;
                CBLAS.Transpose transA = transpose.AsCblasTranspose();

                _ztrsm(CBLAS.Order.ColumnMajor, (side == BLAS.Side.Left) ? CBLAS.Side.Left : CBLAS.Side.Right, uplo, transA, diag, m, n, ref alpha, a, lda, b, ldb);
            }

            /// <summary>Gets a optimal workspace array length for the <c>aux_zgetrans</c> function.
            /// </summary>
            /// <param name="rowCount">The number of rows.</param>
            /// <param name="columnCount">The number of columns.</param>
            /// <returns>The optimal workspace array length.</returns>
            public virtual int aux_zgetransQuery(int rowCount, int columnCount)
            {
                return m_BuildInLevel3Blas.aux_zgetransQuery(rowCount, columnCount);
            }

            /// <summary>Performs in-place transposition of a specific matrix.
            /// </summary>
            /// <param name="rowCount">The number of rows.</param>
            /// <param name="columnCount">The number of columns.</param>
            /// <param name="a">The matrix provided column-by-column (column-major ordering).</param>
            /// <param name="work">A workspace array.</param>
            public virtual void aux_zgetrans(int rowCount, int columnCount, Complex[] a, Complex[] work = null)
            {
                m_BuildInLevel3Blas.aux_zgetrans(rowCount, columnCount, a, work);
            }
            #endregion

            #endregion
        }
    }
}