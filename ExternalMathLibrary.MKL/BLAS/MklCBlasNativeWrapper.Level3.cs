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
    public partial class MklCBlasNativeWrapper : CBlasNativeWrapper
    {
        /// <summary>Represents the level 3 C-BLAS methods and some routines which are not (C-)BLAS standard but implemented in Intels MKL library.
        /// </summary>
        /// <remarks>This implementation contains wrapper for methods which are not part of the (C-)BLAS standard, but defined in Intels MKL library.</remarks>
        protected class MklLevel3CBLAS : Level3CBLAS
        {
            #region private function import

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_dzgemm", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dzgemm(CBLAS.Order order, CBLAS.Transpose transA, CBLAS.Transpose transB, int m, int n, int k, ref Complex alpha, double[] a, int lda, [In, Out] Complex[] b, int ldb, ref Complex beta, [In, Out] Complex[] c, int ldc);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "MKL_DIMATCOPY", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _mkl_dimatcopy(ref char ordering, ref char trans, ref int rows, ref int cols, ref double alpha, [In, Out] double[] ab, ref int lda, ref int ldb);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "MKL_ZIMATCOPY", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _mkl_zimatcopy(ref char ordering, ref char trans, ref int rows, ref int cols, ref Complex alpha, [In, Out] Complex[] ab, ref int lda, ref int ldb);
            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="MklLevel3CBLAS"/> class.
            /// </summary>
            internal MklLevel3CBLAS()
            {
            }
            #endregion

            #region public methods

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
            public override void dzgemm(int m, int n, int k, Complex alpha, double[] a, Complex[] b, Complex beta, Complex[] c, int lda, int ldb, int ldc, BLAS.MatrixTransposeState transposeA = BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState transposeB = BLAS.MatrixTransposeState.NoTranspose)
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
                _dzgemm(CBLAS.Order.ColumnMajor, transposeA.AsCblasTranspose(), transposeB.AsCblasTranspose(), m, n, k, ref alpha, a, lda, b, ldb, ref beta, c, ldc);
            }

            /// <summary>Gets a optimal workspace array length for the <c>aux_dgetrans</c> function.
            /// </summary>
            /// <param name="rowCount">The number of rows.</param>
            /// <param name="columnCount">The number of columns.</param>
            /// <returns>The optimal workspace array length.</returns>
            public override int aux_dgetransQuery(int rowCount, int columnCount)
            {
                return 0;
            }

            /// <summary>Performs in-place transposition of a specific matrix.
            /// </summary>
            /// <param name="rowCount">The number of rows.</param>
            /// <param name="columnCount">The number of columns.</param>
            /// <param name="a">The matrix provided column-by-column (column-major ordering).</param>
            /// <param name="work">A workspace array.</param>
            public override void aux_dgetrans(int rowCount, int columnCount, double[] a, double[] work = null)
            {
                var trans = 't';
                var ordering = 'c';
                var alpha = 1.0;
                int lda = rowCount;
                int ldb = columnCount;

                _mkl_dimatcopy(ref ordering, ref trans, ref rowCount, ref columnCount, ref alpha, a, ref lda, ref ldb);
            }

            /// <summary>Gets a optimal workspace array length for the <c>aux_zgetrans</c> function.
            /// </summary>
            /// <param name="rowCount">The number of rows.</param>
            /// <param name="columnCount">The number of columns.</param>
            /// <returns>The optimal workspace array length.</returns>
            public override int aux_zgetransQuery(int rowCount, int columnCount)
            {
                return 0;
            }

            /// <summary>Performs in-place transposition of a specific matrix.
            /// </summary>
            /// <param name="rowCount">The number of rows.</param>
            /// <param name="columnCount">The number of columns.</param>
            /// <param name="a">The matrix provided column-by-column (column-major ordering).</param>
            /// <param name="work">A workspace array.</param>
            public override void aux_zgetrans(int rowCount, int columnCount, Complex[] a, Complex[] work = null)
            {
                var trans = 't';
                var ordering = 'c';
                var alpha = Complex.One;
                int lda = rowCount;
                int ldb = columnCount;

                _mkl_zimatcopy(ref ordering, ref trans, ref rowCount, ref columnCount, ref alpha, a, ref lda, ref ldb);
            }
            #endregion
        }
    }
}