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

using Dodoni.MathLibrary.Basics.LowLevel;

namespace Dodoni.MathLibrary.Basics
{
    public static partial class Extensions
    {
        #region double precision

        /// <summary>Computes a matrix-matrix product with a general matrix, i.e. C := \alpha * op(A)*op(B) + \beta * C, where op(.) is the identity or the transpose operation.
        /// </summary>
        /// <param name="level3">The BLAS level 3 implementation.</param>
        /// <param name="m">The number of rows of the matrix op(A) and of the matrix C.</param>
        /// <param name="n">The number of columns of the matrix op(B) and of the matrix C.</param>
        /// <param name="k">The number of columns of the matrix op(A) and the number of rows of the matrix op(B).</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The matrix A supplied column-by-column of dimension (s, ka), where s must be at least max(1,<paramref name="m"/>) and ka is <paramref name="k"/> if op(A) = A; s at least max(1, <paramref name="k"/>) and ka is <paramref name="m"/> otherwise.</param>
        /// <param name="b">The matrix B supplied column-by-column of dimension (s, kb), where s must be at least max(1, <paramref name="k"/>) and kb is <paramref name="n"/> if op(B) = B; s at least max(1, <paramref name="n"/>) and ka is <paramref name="k"/> otherwise.</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="c">The matrix C supplied column-by-column of dimension (<paramref name="m"/>, <paramref name="n"/>).</param>
        /// <param name="transposeA">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        /// <param name="transposeB">A value indicating whether 'op(B)=B' or 'op(B)=B^t'.</param>
        public static void dgemm(this ILevel3BLAS level3, int m, int n, int k, double alpha, double[] a, double[] b, double beta, double[] c, BLAS.MatrixTransposeState transposeA = BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState transposeB = BLAS.MatrixTransposeState.NoTranspose)
        {
            level3.dgemm(m, n, k, alpha, a, b, beta, c, (transposeA == BLAS.MatrixTransposeState.NoTranspose) ? m : k, (transposeB == BLAS.MatrixTransposeState.NoTranspose) ? k : n, m, transposeA, transposeB);
        }

        /// <summary>Computes a matrix-matrix product with a general matrix, i.e. C := \alpha * op(A)*op(B) + \beta * C, where op(.) is the identity or the transpose operation.
        /// </summary>
        /// <param name="level3">The BLAS level 3 implementation.</param>
        /// <param name="m">The number of rows of the matrix op(A) and of the matrix C.</param>
        /// <param name="n">The number of columns of the matrix op(B) and of the matrix C.</param>
        /// <param name="k">The number of columns of the matrix op(A) and the number of rows of the matrix op(B).</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The matrix A supplied column-by-column of dimension (s, ka), where s must be at least max(1,<paramref name="m"/>) and ka is <paramref name="k"/> if op(A) = A; s at least max(1, <paramref name="k"/>) and ka is <paramref name="m"/> otherwise.</param>
        /// <param name="b">The matrix B supplied column-by-column of dimension (s, kb), where s must be at least max(1, <paramref name="k"/>) and kb is <paramref name="n"/> if op(B) = B; s at least max(1, <paramref name="n"/>) and ka is <paramref name="k"/> otherwise.</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="c">The matrix C supplied column-by-column of dimension (<paramref name="m"/>, <paramref name="n"/>).</param>
        /// <param name="startIndexA">The null-based start index for <paramref name="a"/></param>
        /// <param name="startIndexB">The null-based start index for <paramref name="b"/></param>
        /// <param name="transposeA">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        /// <param name="transposeB">A value indicating whether 'op(B)=B' or 'op(B)=B^t'.</param>
        /// <param name="startIndexC">The null-based start index for <paramref name="c"/></param>
        public static void dgemm(this ILevel3BLAS level3, int m, int n, int k, double alpha, double[] a, double[] b, double beta, double[] c, int startIndexA, int startIndexB, BLAS.MatrixTransposeState transposeA = BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState transposeB = BLAS.MatrixTransposeState.NoTranspose, int startIndexC = 0)
        {
            level3.dgemm(m, n, k, alpha, a, b, beta, c, (transposeA == BLAS.MatrixTransposeState.NoTranspose) ? m : k, (transposeB == BLAS.MatrixTransposeState.NoTranspose) ? k : n, m, startIndexA, startIndexB, transposeA, transposeB, startIndexC);
        }

        /// <summary>Computes a matrix-matrix product where one input matrix is symmetric, i.e. C := \alpha*A*B + \beta*C or C := \alpha*B*A +\beta*C.
        /// </summary>
        /// <param name="level3">The BLAS level 3 implementation.</param>
        /// <param name="m">The number of rows of the matrix C.</param>
        /// <param name="n">The number of columns of the matrix C.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The symmetric matrix A supplied column-by-column of dimension (s, ka), where s must be at least max(1,<paramref name="m"/>) and ka is <paramref name="m"/> if to calculate C := \alpha * A*B + \beta*C; s at least max(1,<paramref name="n"/>) and ka is <paramref name="n"/> otherwise.</param>
        /// <param name="b">The matrix B supplied column-by-column of dimension (<paramref name="m"/>,<paramref name="n"/>).</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="c">The matrix C supplied column-by-column of dimension (<paramref name="m"/>,<paramref name="n"/>); input/output.</param>
        /// <param name="side">A value indicating whether to calculate C := \alpha * A*B + \beta*C or C := \alpha * B*A +\beta*C.</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        public static void dsymm(this ILevel3BLAS level3, int m, int n, double alpha, double[] a, double[] b, double beta, double[] c, BLAS.Side side = BLAS.Side.Left, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix)
        {
            level3.dsymm(m, n, alpha, a, b, beta, c, (side == BLAS.Side.Left) ? m : n, m, m, side, triangularMatrixType);
        }

        /// <summary>Performs a symmetric rank-k update, i.e. C:= \alpha*A*A^t + \beta *C or C:= \alpha*A^t*A + \beta*C.
        /// </summary>
        /// <param name="level3">The BLAS level 3 implementation.</param>
        /// <param name="n">The order of matrix C.</param>
        /// <param name="k">The number of columns of matrix A if to calculate C:= \alpha*A*A^t + \beta *C; otherwise the number of rows of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The matrix A supplied column-by-column of dimension (s, ka), where s must be at least max(1,<paramref name="n"/>) and ka is <paramref name="k"/> if to calculate C:= \alpha*A*A^t + \beta *C; s at least max(1,<paramref name="k"/>) and ka is <paramref name="n"/> otherwise.</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="c">The symmetric matrix C supplied column-by-column of dimension (<paramref name="n"/>, <paramref name="n"/>).</param>
        /// <param name="triangularMatrixType">A value whether matrix C is in its upper or lower triangular representation.</param>
        /// <param name="operation">A value indicating whether to calculate C:= \alpha*A*A^t + \beta *C or C:= \alpha*A^t*A + \beta*C.</param>
        public static void dsyrk(this ILevel3BLAS level3, int n, int k, double alpha, double[] a, double beta, double[] c, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.XsyrkOperation operation = BLAS.XsyrkOperation.ATimesATranspose)
        {
            level3.dsyrk(n, k, alpha, a, beta, c, operation == BLAS.XsyrkOperation.ATimesATranspose ? n : k, n, triangularMatrixType, operation);
        }

        /// <summary>Performs a symmetric rank-2k update, i.e. C := alpha*A*B^t + alpha*B*A^t + beta*C or C := alpha*A^t*B + alpha*B^t*A + beta*C with a symmetric matrix C.
        /// </summary>
        /// <param name="level3">The BLAS level 3 implementation.</param>
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
        public static void dsyr2k(this ILevel3BLAS level3, int n, int k, double alpha, double[] a, double[] b, double beta, double[] c, int lda, int ldb, int ldc, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.Xsyr2kOperation operation = BLAS.Xsyr2kOperation.ATimesBTransPlusBTimesATrans)
        {
            level3.dsyr2k(n, k, alpha, a, b, beta, c, operation == BLAS.Xsyr2kOperation.ATimesBTransPlusBTimesATrans ? n : k, operation == BLAS.Xsyr2kOperation.ATimesBTransPlusBTimesATrans ? n : k, n, triangularMatrixType, operation);
        }

        /// <summary>Computes a matrix-matrix product where one input matrix is triangular, i.e. B := \alpha * op(A)*B or B:= \alpha *B * op(A), where A is a unit or non-unit upper or lower triangular matrix.
        /// </summary>
        /// <param name="level3">The BLAS level 3 implementation.</param>
        /// <param name="m">The number of rows of matrix B.</param>
        /// <param name="n">The number of columns of matrix B.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The triangular matrix A supplied column-by-column of dimension (s, k), where s must be at least max(1,<paramref name="m"/>) and k is <paramref name="m"/> if to calculate B := \alpha * op(A)*B; s at least max(1,<paramref name="n"/>) and k is <paramref name="n"/> otherwise.</param>
        /// <param name="b">The matrix B supplied column-by-column of dimension (<paramref name="m"/>, <paramref name="n"/>).</param>
        /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
        /// <param name="side">A value indicating whether to calculate B := \alpha * op(A)*B or B:= \alpha *B * op(A).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        public static void dtrmm(this ILevel3BLAS level3, int m, int n, double alpha, double[] a, double[] b, bool isUnitTriangular = true, BLAS.Side side = BLAS.Side.Left, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose)
        {
            level3.dtrmm(m, n, alpha, a, b, side == BLAS.Side.Left ? m : n, m, isUnitTriangular, side, triangularMatrixType, transpose);
        }

        /// <summary>Solves a triangular matrix equation, i.e. op(A) * X = \alpha * B or X * op(A) = \alpha *B, where A is a unit or non-unit upper or lower triangular matrix.
        /// </summary>
        /// <param name="level3">The BLAS level 3 implementation.</param>
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
        public static void dtrsm(this ILevel3BLAS level3, int m, int n, double alpha, double[] a, double[] b, int lda, int ldb, bool isUnitTriangular = true, BLAS.Side side = BLAS.Side.Left, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose)
        {
            level3.dtrsm(m, n, alpha, a, b, side == BLAS.Side.Left ? m : n, m, isUnitTriangular, side, triangularMatrixType, transpose);
        }
        #endregion

        #region (double precision) complex methods

        /// <summary>Computes a matrix-matrix product with a general matrix, i.e. C := \alpha * op(A)*op(B) + \beta * C, 
        /// where where op(.) is the identity or the transpose operation.
        /// </summary>
        /// <param name="level3">The BLAS level 3 implementation.</param>
        /// <param name="m">The number of rows of the matrix op(A) and of the matrix C.</param>
        /// <param name="n">The number of columns of the matrix op(B) and of the matrix C.</param>
        /// <param name="k">The number of columns of the matrix op(A) and the number of rows of the matrix op(B).</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The matrix A supplied column-by-column of dimension (s, ka), where s must be at least max(1,<paramref name="m"/>) and ka is <paramref name="k"/> if op(A) = A; s at least max(1, <paramref name="k"/>) and ka is <paramref name="m"/> otherwise.</param>
        /// <param name="b">The matrix B supplied column-by-column of dimension (s, kb), where s must be at least max(1,<paramref name="k"/>) and kb is <paramref name="n"/> if op(B) = B; s at least max(1, <paramref name="n"/>) and kb is <paramref name="k"/> otherwise.</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="c">The matrix C supplied column-by-column of dimension (<paramref name="m"/>, <paramref name="n"/>).</param>
        /// <param name="transposeA">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        /// <param name="transposeB">A value indicating whether 'op(B)=B' or 'op(B)=B^t'.</param>
        public static void dzgemm(this ILevel3BLAS level3, int m, int n, int k, Complex alpha, double[] a, Complex[] b, Complex beta, Complex[] c, BLAS.MatrixTransposeState transposeA = BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState transposeB = BLAS.MatrixTransposeState.NoTranspose)
        {
            level3.dzgemm(m, n, k, alpha, a, b, beta, c, transposeA == BLAS.MatrixTransposeState.NoTranspose ? m : k, transposeB == BLAS.MatrixTransposeState.NoTranspose ? k : n, m, transposeA, transposeB);
        }

        /// <summary>Computes a matrix-matrix product with a general matrix, i.e. C := \alpha * op(A)*op(B) + \beta * C, 
        /// where where op(.) is the identity or the transpose operation.
        /// </summary>
        /// <param name="level3">The BLAS level 3 implementation.</param>
        /// <param name="m">The number of rows of the matrix op(A) and of the matrix C.</param>
        /// <param name="n">The number of columns of the matrix op(B) and of the matrix C.</param>
        /// <param name="k">The number of columns of the matrix op(A) and the number of rows of the matrix op(B).</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The matrix A supplied column-by-column of dimension (s, ka), where s must be at least max(1,<paramref name="m"/>) and ka is <paramref name="k"/> if op(A) = A; s at least max(1, <paramref name="k"/>) and ka is <paramref name="m"/> otherwise.</param>
        /// <param name="b">The matrix B supplied column-by-column of dimension (s, kb), where s must be at least max(1,<paramref name="k"/>) and kb is <paramref name="n"/> if op(B) = B; s at least max(1, <paramref name="n"/>) and kb is <paramref name="k"/> otherwise.</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="c">The matrix C supplied column-by-column of dimension (<paramref name="m"/>, <paramref name="n"/>).</param>
        /// <param name="transposeA">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        /// <param name="transposeB">A value indicating whether 'op(B)=B' or 'op(B)=B^t'.</param>
        public static void zgemm(this ILevel3BLAS level3, int m, int n, int k, Complex alpha, Complex[] a, Complex[] b, Complex beta, Complex[] c, BLAS.MatrixTransposeState transposeA = BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState transposeB = BLAS.MatrixTransposeState.NoTranspose)
        {
            level3.zgemm(m, n, k, alpha, a, b, beta, c, transposeA == BLAS.MatrixTransposeState.NoTranspose ? m : k, transposeB == BLAS.MatrixTransposeState.NoTranspose ? k : n, m, transposeA, transposeB);
        }

        /// <summary>Computes a matrix-matrix product where one input matrix is Hermitian, i.e. C := \alpha*A*B + \beta*C or C := \alpha*B*A + \beta*C, where A is a Hermitian matrix.
        /// </summary>
        /// <param name="level3">The BLAS level 3 implementation.</param>
        /// <param name="m">The number of rows of matrix C.</param>
        /// <param name="n">The number of columns of matrix C.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The Hermitian matrix A supplied column-by-column of dimension (<paramref name="m"/>, ka), where ka is <paramref name="m"/> if to calculate C := \alpha*A*B + \beta*C; <paramref name="n"/> otherwise.</param>
        /// <param name="b">The matrix B supplied column-by-column of dimension (<paramref name="m"/>, <paramref name="n"/>).</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="c">The matrix C supplied column-by-column of dimension (<paramref name="m"/>, <paramref name="n"/>).</param>
        /// <param name="side">A value indicating whether to calculate C := \alpha*A*B + \beta*C or C := \alpha*B*A + \beta*C.</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        public static void zhemm(this ILevel3BLAS level3, int m, int n, Complex alpha, Complex[] a, Complex[] b, Complex beta, Complex[] c, BLAS.Side side = BLAS.Side.Left, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix)
        {
            level3.zhemm(m, n, alpha, a, b, beta, c, side == BLAS.Side.Left ? m : n, m, m, side, triangularMatrixType);
        }

        /// <summary>Performs a Hermitian rank-k update, i.e. C := \alpha * A * A^h + \beta*C or C := alpha*A^h * A + beta*C, where C is a Hermitian matrix.
        /// </summary>
        /// <param name="level3">The BLAS level 3 implementation.</param>
        /// <param name="n">The order of matrix C.</param>
        /// <param name="k">The number of columns of matrix A if to calculate C := \alpha * A * A^h + \beta*C; otherwise the number of rows of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The matrix A supplied column-by-column of dimension (s, ka), where s must be at least max(1,<paramref name="n"/>) and ka equals to <paramref name="k"/> if to calculate C := \alpha * A * A^h + \beta*C; s = max(1, <paramref name="k"/>) and ka = <paramref name="n"/> otherwise.</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="c">The Hermitian matrix C supplied column-by-column of dimension (<paramref name="n"/>, <paramref name="n"/>).</param>
        /// <param name="triangularMatrixType">A value whether matrix C is in its upper or lower triangular representation.</param>
        /// <param name="operation">A value indicating whether to calculate C := \alpha * A * A^h + \beta*C or C := alpha*A^h * A + beta*C.</param>        
        public static void zherk(this ILevel3BLAS level3, int n, int k, double alpha, Complex[] a, double beta, Complex[] c, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.ZherkOperation operation = BLAS.ZherkOperation.AHermiteTimesA)
        {
            level3.zherk(n, k, alpha, a, beta, c, operation == BLAS.ZherkOperation.ATimesAHermite ? n : k, n, triangularMatrixType, operation);
        }

        /// <summary>Performs a Hermitian rank-2 update, i.e. C := \alpha*A*B^h + conjg(\alpha)*B*A^h + \beta * C or C := \alpha*B^h*A + conjg(\alpha)*A^h*B + beta*C, where C is an Hermitian matrix.
        /// </summary>
        /// <param name="level3">The BLAS level 3 implementation.</param>
        /// <param name="n">The order of matrix C.</param>
        /// <param name="k">The number of columns of matrix A if to calculate C := \alpha*A*B^h + conjg(\alpha)*B*A^h + \beta * C; the number of rows of the matrix A otherwise.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The matrix A supplied column-by-column of dimension (s, ka), where s must be at least max(1,<paramref name="n"/>) and ka equals to <paramref name="k"/> if to calculate C := \alpha*A*B^h + conjg(\alpha)*B*A^h + \beta * C; s = max(1, <paramref name="k"/>), ka = <paramref name="n"/> otherwise.</param>
        /// <param name="b">The matrix B supplied column-by-column of dimension (s, kb), where s must be at least max(1,<paramref name="n"/>) and kb equals to <paramref name="k"/> if to calculate C := \alpha*A*B^h + conjg(\alpha)*B*A^h + \beta * C; s = max(1, <paramref name="k"/>), ka = <paramref name="n"/> otherwise.</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="c">The Hermitian matrix C supplied column-by-column of dimension (<paramref name="n"/>, <paramref name="n"/>).</param>
        /// <param name="triangularMatrixType">A value whether matrix C is in its upper or lower triangular representation.</param>
        /// <param name="operation">A value indicating whether to calculate C := \alpha*A*B^h + conjg(\alpha)*B*A^h + \beta * C or C := \alpha*B^h*A + conjg(\alpha)*A^h*B + beta*C.</param>                
        public static void zher2k(this ILevel3BLAS level3, int n, int k, Complex alpha, Complex[] a, Complex[] b, double beta, Complex[] c, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.Zher2kOperation operation = BLAS.Zher2kOperation.ATimesBHermitePlusBTimesAHermite)
        {
            level3.zher2k(n, k, alpha, a, b, beta, c, operation == BLAS.Zher2kOperation.ATimesBHermitePlusBTimesAHermite ? n : k, operation == BLAS.Zher2kOperation.ATimesBHermitePlusBTimesAHermite ? n : k, n, triangularMatrixType, operation);
        }

        /// <summary>Computes a matrix-matrix product where one input matrix is symmetric, i.e. C := \alpha*A*B + \beta*C or C := \alpha*B*A +\beta*C.
        /// </summary>
        /// <param name="level3">The BLAS level 3 implementation.</param>
        /// <param name="m">The number of rows of the matrix C.</param>
        /// <param name="n">The number of columns of the matrix C.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The symmetric matrix A supplied column-by-column of dimension (s, ka), where s must be at least max(1,<paramref name="m"/>) and ka is <paramref name="m"/> if to calculate C := \alpha * A*B + \beta*C; otherwise s at least max(1,<paramref name="n"/>) and ka = <paramref name="n"/>.</param>
        /// <param name="b">The matrix B supplied column-by-column of dimension (<paramref name="m"/>,<paramref name="n"/>).</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="c">The matrix C supplied column-by-column of dimension (<paramref name="m"/>,<paramref name="n"/>); input/output.</param>
        /// <param name="side">A value indicating whether to calculate C := \alpha * A*B + \beta*C or C := \alpha * B*A +\beta*C.</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        public static void zsymm(this ILevel3BLAS level3, int m, int n, Complex alpha, Complex[] a, Complex[] b, Complex beta, Complex[] c, BLAS.Side side = BLAS.Side.Left, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix)
        {
            level3.zsymm(m, n, alpha, a, b, beta, c, side == BLAS.Side.Left ? m : n, m, m, side, triangularMatrixType);
        }

        /// <summary>Performs a symmetric rank-k update, i.e. C:= \alpha*A*A^t + \beta *C or C:= \alpha*A^t*A + \beta*C.
        /// </summary>
        /// <param name="level3">The BLAS level 3 implementation.</param>
        /// <param name="n">The order of matrix C.</param>
        /// <param name="k">The number of columns of matrix A if to calculate C:= \alpha*A*A^t + \beta *C; otherwise the number of rows of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The matrix A supplied column-by-column of dimension (s, ka), where s must be at least max(1,<paramref name="n"/>) and ka is <paramref name="k"/> if to calculate C:= \alpha*A*A^t + \beta *C; s at least max(1,<paramref name="k"/>) and ka <paramref name="n"/> otherwise.</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="c">The symmetric matrix C supplied column-by-column of dimension (<paramref name="n"/>, <paramref name="n"/>).</param>
        /// <param name="triangularMatrixType">A value whether matrix C is in its upper or lower triangular representation.</param>
        /// <param name="operation">A value indicating whether to calculate C:= \alpha*A*A^t + \beta *C or C:= \alpha*A^t*A + \beta*C.</param>
        public static void zsyrk(this ILevel3BLAS level3, int n, int k, Complex alpha, Complex[] a, Complex beta, Complex[] c, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.XsyrkOperation operation = BLAS.XsyrkOperation.ATimesATranspose)
        {
            level3.zsyrk(n, k, alpha, a, beta, c, operation == BLAS.XsyrkOperation.ATimesATranspose ? n : k, n, triangularMatrixType, operation);
        }

        /// <summary>Performs a symmetric rank-2k update, i.e. C := alpha*A*B^t + alpha*B*A^t + beta*C or C := alpha*A^t*B + alpha*B^t*A + beta*C with a symmetric matrix C.
        /// </summary>
        /// <param name="level3">The BLAS level 3 implementation.</param>
        /// <param name="n">The order of matrix C.</param>
        /// <param name="k">The The number of columns of matrices A and B or the number .</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The matrix A supplied column-by-column of dimension (s, ka), where s = <paramref name="n"/>, ka = <paramref name="k"/> if to calculate C := alpha*A*B^t + alpha*B*A^t + beta*C; otherwise s=<paramref name="k"/>, ka = <paramref name="n"/>.</param>
        /// <param name="b">The matrix B supplied column-by-column of dimension (s, kb), where s = <paramref name="n"/>, ka is at least max(1,<paramref name="n"/>) if to calculate C := alpha*A*B^t + alpha*B*A^t + beta*C; otherwise s at least max(1,<paramref name="k"/> and ka at least max(1,<paramref name="k"/>).</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="c">The symmetric matrix C supplied column-by-column of dimension (<paramref name="n"/>, <paramref name="n"/>).</param>
        /// <param name="triangularMatrixType">A value whether matrix C is in its upper or lower triangular representation.</param>
        /// <param name="operation">A value indicating whether to calculate C := alpha*A*B^t + alpha*B*A^t + beta*C or C := alpha*A^t*B + alpha*B^t*A + beta*C.</param>
        public static void zsyr2k(this ILevel3BLAS level3, int n, int k, Complex alpha, Complex[] a, Complex[] b, Complex beta, Complex[] c, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.Xsyr2kOperation operation = BLAS.Xsyr2kOperation.ATimesBTransPlusBTimesATrans)
        {
            level3.zsyr2k(n, k, alpha, a, b, beta, c, operation == BLAS.Xsyr2kOperation.ATimesBTransPlusBTimesATrans ? n : k, operation == BLAS.Xsyr2kOperation.ATimesBTransPlusBTimesATrans ? n : k, n, triangularMatrixType, operation);
        }

        /// <summary>Computes a matrix-matrix product where one input matrix is triangular, i.e. B := \alpha * op(A)*B or B:= \alpha *B * op(A), where A is a unit or non-unit upper or lower triangular matrix.
        /// </summary>
        /// <param name="level3">The BLAS level 3 implementation.</param>
        /// <param name="m">The number of rows of matrix B.</param>
        /// <param name="n">The number of columns of matrix B.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The triangular matrix A supplied column-by-column of dimension (s, k), where s, k = <paramref name="m"/> if to calculate B := \alpha * op(A)*B; <paramref name="n"/> otherwise.</param>
        /// <param name="b">The matrix B supplied column-by-column of dimension (<paramref name="m"/>, <paramref name="n"/>).</param>                
        /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
        /// <param name="side">A value indicating whether to calculate B := \alpha * op(A)*B or B:= \alpha *B * op(A).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        public static void ztrmm(this ILevel3BLAS level3, int m, int n, Complex alpha, Complex[] a, Complex[] b, bool isUnitTriangular = true, BLAS.Side side = BLAS.Side.Left, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose)
        {
            level3.ztrmm(m, n, alpha, a, b, side == BLAS.Side.Left ? m : n, m, isUnitTriangular, side, triangularMatrixType, transpose);
        }

        /// <summary>Solves a triangular matrix equation, i.e. op(A) * X = \alpha * B or X * op(A) = \alpha *B, where A is a unit or non-unit upper or lower triangular matrix.
        /// </summary>
        /// <param name="level3">The BLAS level 3 implementation.</param>
        /// <param name="m">The number of rows of matrix B.</param>
        /// <param name="n">The number of column of matrix B.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The triangular matrix A supplied column-by-column of dimension (s, k), where s, k = <paramref name="m"/> if to calculate op(A) * X = \alpha * B; <paramref name="n"/> otherwise.</param>
        /// <param name="b">The matrix B supplied column-by-column of dimension (<paramref name="m"/>, <paramref name="n"/>).</param>
        /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
        /// <param name="side">A value indicating whether to calculate op(A) * X = \alpha * B or X * op(A) = \alpha *B.</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        public static void ztrsm(this ILevel3BLAS level3, int m, int n, Complex alpha, Complex[] a, Complex[] b, bool isUnitTriangular = true, BLAS.Side side = BLAS.Side.Left, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose)
        {
            level3.ztrsm(m, n, alpha, a, b, side == BLAS.Side.Left ? m : n, m, isUnitTriangular, side, triangularMatrixType, transpose);
        }
        #endregion
    }
}