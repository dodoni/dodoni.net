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

using Dodoni.MathLibrary.Basics.LowLevel;

namespace Dodoni.MathLibrary.Basics
{
    public static partial class Extensions
    {
#pragma warning disable IDE1006 // Naming Styles

        #region double precision methods

        /// <summary>Computes a matrix-vector product using a general band matrix, i.e. y := \alpha * op(A) * x + \beta * y, where op(A) = A or op(A) = A^t.
        /// </summary>
        /// <param name="level2">The BLAS level 2 implementation.</param>
        /// <param name="m">The number of rows of matrix A.</param>
        /// <param name="n">The number of columns of matrix A.</param>
        /// <param name="kl">The number of sub-diagonals of matrix A.</param>
        /// <param name="ku">The number of super-diagonals of matrix A.</param>
        /// <param name="alpha">The scalar factor \alpha.</param>
        /// <param name="a">The general band matrix A of dimension (<paramref name="kl"/> + <paramref name="ku"/> + 1, <paramref name="n"/>). The leading (<paramref name="kl"/> + <paramref name="ku"/> + 1) by <paramref name="n"/> part 
        /// must contain the matrix of coefficients (supplied column-by-column) with the leading diagonal of the matrix 
        /// in row (<paramref name="ku"/> + 1) of the array, the first super-diagonal starting at position 2 in row ku, 
        /// the first sub-diagonal starting at position 1 in row (ku + 2), etc. 
        /// The following program segment transfers a band matrix from conventional full matrix storage to band storage:
        /// <code>
        /// for j = 0 to n-1
        ///   k = ku - j
        ///   for i = max(0, j-ku) to min(m-1, j+kl-1) 
        ///     a(k+i, j) = matrix(i,j)
        ///   end
        /// end
        /// </code>
        /// </param>
        /// <param name="x">The vector x with at least 1+(<paramref name="n"/>-1) * |<paramref name="incX"/>| elements if 'op(A)=A'; 1+(<paramref name="m"/>-1) *|<paramref name="incX"/>| otherwise.</param>
        /// <param name="beta">The scalar factor \beta.</param>
        /// <param name="y">The vector y with at least 1+(<paramref name="m"/>-1) *|<paramref name="incY"/>| elements if 'op(A)=A'; 1+(<paramref name="n"/>-1) * |<paramref name="incY"/>| otherwise (input/output).</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
        public static void dgbmv(this ILevel2BLAS level2, int m, int n, int kl, int ku, double alpha, ReadOnlySpan<double> a, ReadOnlySpan<double> x, double beta, Span<double> y, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1, int incY = 1)
        {
            level2.dgbmv(m, n, kl, ku, alpha, a, x, beta, y, kl + ku + 1, transpose, incX, incY);
        }

        /// <summary>Computes a matrix-vector product for a general matrix, i.e. y = \alpha * op(A)*x + \beta*y, where \op(A) = A or \op(A)=A^t.
        /// </summary>
        /// <param name="level2">The BLAS level 2 implementation.</param>
        /// <param name="m">The number of rows of matrix A.</param>
        /// <param name="n">The number of columns of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The matrix A of dimension (<paramref name="m"/>, <paramref name="n"/>) supplied column-by-column.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incX"/>| elements if 'op(A)=A'; 1 + (<paramref name="m"/>-1) * |<paramref name="incY"/>| elements otherwise.</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="y">The vector y with at least 1 + (<paramref name="m"/>-1) * |<paramref name="incY"/>| elements if 'op(A)=A'; 1 + (<paramref name="n"/>-1) * | <paramref name="incX"/>| otherwise (input/output).</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
        public static void dgemv(this ILevel2BLAS level2, int m, int n, double alpha, ReadOnlySpan<double> a, ReadOnlySpan<double> x, double beta, Span<double> y, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1, int incY = 1)
        {
            level2.dgemv(m, n, alpha, a, x, beta, y, m, transpose, incX, incY);
        }

        /// <summary>Performs a rank-1 update of a general matrix, i.e. A := \alpha * x * y^t + A.
        /// </summary>
        /// <param name="level2">The BLAS level 2 implementation.</param>
        /// <param name="m">The number of rows of matrix A.</param>
        /// <param name="n">The number of columns of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="m"/>-1) * |<paramref name="incX"/>| elements.</param>
        /// <param name="y">The vector y with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incY"/>| elements.</param>
        /// <param name="a">The matrix A of dimension (<paramref name="m"/>, <paramref name="n"/>) supplied column-by-column (input/output).</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
        public static void dger(this ILevel2BLAS level2, int m, int n, double alpha, ReadOnlySpan<double> x, ReadOnlySpan<double> y, Span<double> a, int incX = 1, int incY = 1)
        {
            level2.dger(m, n, alpha, x, y, a, m, incX, incY);
        }

        /// <summary>Computes a matrix-vector product using a symmetric band matrix, i.e. y:= \alpha * A * x + \beta * y.
        /// </summary>
        /// <param name="level2">The BLAS level 2 implementation.</param>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="k">The number of super-diagonals of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The matrix A of dimension (1+<paramref name="k"/>, <paramref name="n"/>) supplied column-by-column.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="y">The vector y with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incY"/> | elements (input/output).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
        public static void dsbmv(this ILevel2BLAS level2, int n, int k, double alpha, ReadOnlySpan<double> a, ReadOnlySpan<double> x, double beta, Span<double> y, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1, int incY = 1)
        {
            level2.dsbmv(n, k, alpha, a, x, beta, y, 1 + k, triangularMatrixType, incX, incY);
        }

        /// <summary>Computes a matrix-vector product for a symmetric matrix, i.e. y := \alpha * A * x + \beta * y.
        /// </summary>
        /// <param name="level2">The BLAS level 2 implementation.</param>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The symmetric matrix A of dimension (<paramref name="n"/>, <paramref name="n"/>) supplied column-by-column.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="y">The vector y with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incY"/> | elements (input/output).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
        public static void dsymv(this ILevel2BLAS level2, int n, double alpha, ReadOnlySpan<double> a, ReadOnlySpan<double> x, double beta, Span<double> y, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1, int incY = 1)
        {
            level2.dsymv(n, alpha, a, x, beta, y, n, triangularMatrixType, incX, incY);
        }

        /// <summary>Performs a rank-1 update of a symmetric matrix, i.e. A := \alpha * x * x^t + A.
        /// </summary>
        /// <param name="level2">The BLAS level 2 implementation.</param>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
        /// <param name="a">The symmetric matrix A of dimension (<paramref name="n"/>, <paramref name="n"/>) supplied column-by-column (input/output).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        public static void dsyr(this ILevel2BLAS level2, int n, double alpha, ReadOnlySpan<double> x, Span<double> a, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1)
        {
            level2.dsyr(n, alpha, x, a, n, triangularMatrixType, incX);
        }

        /// <summary>Performs a rank-2 update of a symmetric matrix, i.e. A := \alpha * x * y^t + \alpha * y * x^t + A.
        /// </summary>
        /// <param name="level2">The BLAS level 2 implementation.</param>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
        /// <param name="y">The vector y with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incY"/> | elements.</param>
        /// <param name="a">The symmetric matrix A of dimension (<paramref name="n"/>, <paramref name="n"/>) supplied column-by-column (input/output).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
        public static void dsyr2(this ILevel2BLAS level2, int n, double alpha, ReadOnlySpan<double> x, ReadOnlySpan<double> y, Span<double> a, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1, int incY = 1)
        {
            level2.dsyr2(n, alpha, x, y, a, n, triangularMatrixType, incX, incY);
        }

        /// <summary>Computes a matrix-vector product using a triangular band matrix, i.e. x := op(A) * x, where op(A) = A or op(A) = A^t.
        /// </summary>
        /// <param name="level2">The BLAS level 2 implementation.</param>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="k">The number of super-diagonales of A if the matrix A is provided in its upper triangular representation; the number of sub-diagonals otherwise.</param>
        /// <param name="a">The triangular band matrix with dimension ( 1 + <paramref name="k"/>, <paramref name="n"/>).</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        public static void dtbmv(this ILevel2BLAS level2, int n, int k, ReadOnlySpan<double> a, Span<double> x, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1)
        {
            level2.dtbmv(n, k, a, x, 1 + k, triangularMatrixType, isUnitTriangular, transpose, incX);
        }

        /// <summary>Solves a system of linear equations whose coefficients are in a triangular band matrix, i.e. solves op(A) * x = b, where op(A) = A or op(A) = A^t.
        /// </summary>
        /// <param name="level2">The BLAS level 2 implementation.</param>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="k">The number of super-diagonales of A if the matrix A is provided in its upper triangular representation; the number of sub-diagonals otherwise.</param>
        /// <param name="a">The triangular band matrix with dimension ( 1+ <paramref name="k"/>, <paramref name="n"/>).</param>
        /// <param name="x">The vector b (input), x (output) with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements (input/output).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        public static void dtbsv(this ILevel2BLAS level2, int n, int k, ReadOnlySpan<double> a, Span<double> x, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1)
        {
            level2.dtbsv(n, k, a, x, 1 + k, triangularMatrixType, isUnitTriangular, transpose, incX);
        }

        /// <summary>Computes a matrix-vector product using a triangular matrix, i.e. x := op(A) * x, where op(A) = A or op(A) = A^t.
        /// </summary>
        /// <param name="level2">The BLAS level 2 implementation.</param>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="a">The triangular matrix A with dimension (<paramref name="n"/>, <paramref name="n"/>).</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        public static void dtrmv(this ILevel2BLAS level2, int n, ReadOnlySpan<double> a, Span<double> x, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1)
        {
            level2.dtrmv(n, a, x, n, triangularMatrixType, isUnitTriangular, transpose, incX);
        }

        /// <summary>Solves a system of linear equations whose coefficients are in a triangular matrix, i.e. op(A) * x = b, where op(A) = A or op(A) = A^t.
        /// </summary>
        /// <param name="level2">The BLAS level 2 implementation.</param>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="a">The triangular matrix A with dimension (<paramref name="n"/>, <paramref name="n"/>).</param>
        /// <param name="x">The vector b (input), x (output) with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements (input/output).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
        /// <param name="transpose">A value indicating whether op(A) = A or op(A) = A^t.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        public static void dtrsv(this ILevel2BLAS level2, int n, ReadOnlySpan<double> a, Span<double> x, BLAS.TriangularMatrixType triangularMatrixType, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1)
        {
            level2.dtrsv(n, a, x, n, triangularMatrixType, isUnitTriangular, transpose, incX);
        }
        #endregion

        #region (double precision) complex methods

        /// <summary>Computes a matrix-vector product using a general band matrix, i.e. y := \alpha * op(A) * x + \beta * y, where op(A) = A, op(A) = A^t or op(A) = A^h.
        /// </summary>
        /// <param name="level2">The BLAS level 2 implementation.</param>
        /// <param name="m">The number of rows of matrix A.</param>
        /// <param name="n">The number of columns of matrix A.</param>
        /// <param name="kl">The number of sub-diagonals of matrix A.</param>
        /// <param name="ku">The number of super-diagonals of matrix A.</param>
        /// <param name="alpha">The scalar factor \alpha.</param>
        /// <param name="a">The general band matrix A of dimension (<paramref name="kl"/> + <paramref name="ku"/> + 1, <paramref name="n"/>). The leading (<paramref name="kl"/> + <paramref name="ku"/> + 1) by <paramref name="n"/> part 
        /// must contain the matrix of coefficients (supplied column-by-column) with the leading diagonal of the matrix 
        /// in row (<paramref name="ku"/> + 1) of the array, the first super-diagonal starting at position 2 in row ku, 
        /// the first sub-diagonal starting at position 1 in row (ku + 2), etc. 
        /// The following program segment transfers a band matrix from conventional full matrix storage to band storage:
        /// <code>
        /// for j = 0 to n-1
        ///   k = ku - j
        ///   for i = max(0, j-ku) to min(m-1, j+kl-1) 
        ///     a(k+i, j) = matrix(i,j)
        ///   end
        /// end
        /// </code>
        /// </param>
        /// <param name="x">The vector x with at least 1+(<paramref name="n"/>-1) * |<paramref name="incX"/>| elements if 'op(A)=A'; 1+(<paramref name="m"/>-1) *|<paramref name="incX"/>| otherwise.</param>
        /// <param name="beta">The scalar factor \beta.</param>
        /// <param name="y">The vector y with at least 1+(<paramref name="m"/>-1) *|<paramref name="incY"/>| elements if 'op(A)=A'; 1+(<paramref name="n"/>-1) * |<paramref name="incY"/>| otherwise (input/output).</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
        public static void zgbmv(this ILevel2BLAS level2, int m, int n, int kl, int ku, Complex alpha, ReadOnlySpan<Complex> a, ReadOnlySpan<Complex> x, Complex beta, Span<Complex> y, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1, int incY = 1)
        {
            level2.zgbmv(m, n, kl, ku, alpha, a, x, beta, y, kl + ku + 1, transpose, incX, incY);
        }

        /// <summary>Computes a matrix-vector product for a general matrix, i.e. y = \alpha * op(A)*x + \beta*y, where op(A) = A, op(A) = A^t or op(A) = A^h.
        /// </summary>
        /// <param name="level2">The BLAS level 2 implementation.</param>
        /// <param name="m">The number of rows of matrix A.</param>
        /// <param name="n">The number of columns of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The matrix A of dimension (<paramref name="m"/>, <paramref name="n"/>) supplied column-by-column.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incX"/>| elements if 'op(A)=A'; 1 + (<paramref name="m"/>-1) * |<paramref name="incY"/>| elements otherwise.</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="y">The vector y with at least 1 + (<paramref name="m"/>-1) * |<paramref name="incY"/>| elements if 'op(A)=A'; 1 + (<paramref name="n"/>-1) * | <paramref name="incX"/>| otherwise (input/output).</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
        public static void zgemv(this ILevel2BLAS level2, int m, int n, Complex alpha, ReadOnlySpan<Complex> a, ReadOnlySpan<Complex> x, Complex beta, Span<Complex> y, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1, int incY = 1)
        {
            level2.zgemv(m, n, alpha, a, x, beta, y, m, transpose, incX, incY);
        }

        /// <summary>Performs a rank-1 update (conjuaged) of a general matrix, i.e. A := \alpha * x * conj(y^t) + A.
        /// </summary>
        /// <param name="level2">The BLAS level 2 implementation.</param>
        /// <param name="m">The number of rows of matrix A.</param>
        /// <param name="n">The number of columns of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="m"/>-1) * |<paramref name="incX"/>| elements.</param>
        /// <param name="y">The vector y with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incY"/>| elements.</param>
        /// <param name="a">The matrix A of dimension (<paramref name="m"/>, <paramref name="n"/>) supplied column-by-column.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
        public static void zgerc(this ILevel2BLAS level2, int m, int n, Complex alpha, ReadOnlySpan<Complex> x, ReadOnlySpan<Complex> y, Span<Complex> a, int incX = 1, int incY = 1)
        {
            level2.zgerc(m, n, alpha, x, y, a, m, incX, incY);
        }

        /// <summary>Performs a rank-1 update (unconjugated) of a general matrix, i.e. A := \alpha * x * y^t + A.
        /// </summary>
        /// <param name="level2">The BLAS level 2 implementation.</param>
        /// <param name="m">The number of rows of matrix A.</param>
        /// <param name="n">The number of columns of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="m"/>-1) * |<paramref name="incX"/>| elements.</param>
        /// <param name="y">The vector y with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incY"/>| elements.</param>
        /// <param name="a">The matrix A with dimension (<paramref name="m"/>, <paramref name="n"/>) supplied column-by-column.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
        public static void zgeru(this ILevel2BLAS level2, int m, int n, Complex alpha, ReadOnlySpan<Complex> x, ReadOnlySpan<Complex> y, Span<Complex> a, int incX = 1, int incY = 1)
        {
            level2.zgeru(m, n, alpha, x, y, a, m, incX, incY);
        }

        /// <summary>Computes a matrix-vector product using a Hermitian band matrix, i.e. y := \alpha * A * x + \beta * y.
        /// </summary>
        /// <param name="level2">The BLAS level 2 implementation.</param>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="k">The number of super-diagonals.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The matrix Hermitian band matrix A with dimension (1 + <paramref name="k"/>, <paramref name="n"/>).</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incX"/>| elements.</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="y">The vector y with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incY"/>| elements (input/output).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
        public static void zhbmv(this ILevel2BLAS level2, int n, int k, Complex alpha, ReadOnlySpan<Complex> a, ReadOnlySpan<Complex> x, Complex beta, Span<Complex> y, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1, int incY = 1)
        {
            level2.zhbmv(n, k, alpha, a, x, beta, y, 1 + k, triangularMatrixType, incX, incY);
        }

        /// <summary>Computes a matrix-vector product using a Hermitian matrix, i.e. y := \alpha * A * x + \beta * y.
        /// </summary>
        /// <param name="level2">The BLAS level 2 implementation.</param>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incX"/>| elements.</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="y">The vector y with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incY"/>| elements (input/output).</param>
        /// <param name="a">The Hermitian matrix A with dimension (<paramref name="n"/>, <paramref name="n"/>).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
        public static void zhemv(this ILevel2BLAS level2, int n, Complex alpha, ReadOnlySpan<Complex> x, Complex beta, Span<Complex> y, ReadOnlySpan<Complex> a, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1, int incY = 1)
        {
            level2.zhemv(n, alpha, x, beta, y, a, n, triangularMatrixType, incX, incY);
        }

        /// <summary>Performs a rank-1 update of a Hermitian matrix, i.e. A := \alpha * x * conj(x^t) + A.
        /// </summary>
        /// <param name="level2">The BLAS level 2 implementation.</param>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incX"/>| elements.</param>
        /// <param name="a">The Hermitian matrix A with dimension (<paramref name="n"/>, <paramref name="n"/>).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        public static void zher(this ILevel2BLAS level2, int n, double alpha, ReadOnlySpan<Complex> x, Span<Complex> a, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1)
        {
            level2.zher(n, alpha, x, a, n, triangularMatrixType, incX);
        }

        /// <summary>Performs a rank-2 update of a Hermitian matrix, i.e. A := \alpha * x * conjg(y^t) + conjg(\alpha) * y * conjg(x^t) + A.
        /// </summary>
        /// <param name="level2">The BLAS level 2 implementation.</param>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incX"/>| elements.</param>
        /// <param name="y">The vector y with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incY"/>| elements.</param>
        /// <param name="a">The Hermitian matrix A with dimension (<paramref name="n"/>, <paramref name="n"/>).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
        public static void zher2(this ILevel2BLAS level2, int n, Complex alpha, ReadOnlySpan<Complex> x, ReadOnlySpan<Complex> y, Span<Complex> a, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1, int incY = 1)
        {
            level2.zher2(n, alpha, x, y, a, n, triangularMatrixType, incX, incY);
        }

        /// <summary>Computes a matrix-vector product using a triangular band matrix, i.e. x := op(A) * x, where op(A) = A, op(A) = A^t or op(A) = A^h.
        /// </summary>
        /// <param name="level2">The BLAS level 2 implementation.</param>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="k">The number of super-diagonals of A if the matrix A is provided in its upper triangular representation; the number of sub-diagonals otherwise.</param>
        /// <param name="a">The triangular band matrix with dimension ( 1 +<paramref name="k"/>, <paramref name="n"/>).</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A', 'op(A)=A^t' or 'op(A)=A^h'.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        public static void ztbmv(this ILevel2BLAS level2, int n, int k, ReadOnlySpan<Complex> a, Span<Complex> x, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1)
        {
            level2.ztbmv(n, k, a, x, 1 + k, triangularMatrixType, isUnitTriangular, transpose, incX);
        }

        /// <summary>Solves a system of linear equations whose coefficients are in a triangular band matrix, i.e. op(A) * x = b, where op(A) = A, op(A) = A ^t or op(A) = A^h.
        /// </summary>
        /// <param name="level2">The BLAS level 2 implementation.</param>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="k">The number of super-diagonals of A if the matrix A is provided in its upper triangular representation; the number of sub-diagonals otherwise.</param>
        /// <param name="a">The triangular band matrix with dimension (1+<paramref name="k"/>, <paramref name="n"/>).</param>
        /// <param name="x">The vector b (input), x (output) with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements (input/output).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A', 'op(A)=A^t' or 'op(A)=A^h'.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        public static void ztbsv(this ILevel2BLAS level2, int n, int k, ReadOnlySpan<Complex> a, Span<Complex> x, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1)
        {
            level2.ztbsv(n, k, a, x, 1 + k, triangularMatrixType, isUnitTriangular, transpose, incX);
        }

        /// <summary>Computes a matrix-vector product using a triangular matrix, i.e. x := op(A) * x, where op(A) = A, op(A) = A ^t or op(A) = A^h.
        /// </summary>
        /// <param name="level2">The BLAS level 2 implementation.</param>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="a">The triangular matrix A with dimension at least (<paramref name="n"/>, <paramref name="n"/>).</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A', 'op(A)=A^t' or 'op(A)=A^h'.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        public static void ztrmv(this ILevel2BLAS level2, int n, ReadOnlySpan<Complex> a, Span<Complex> x, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1)
        {
            level2.ztrmv(n, a, x, n, triangularMatrixType, isUnitTriangular, transpose, incX);
        }

        /// <summary>Solves a system of linear equations whose coefficients are in a triangular matrix, i.e. op(A) * x = b, where op(A) = A, op(A) = A ^t or op(A) = A^h.
        /// </summary>
        /// <param name="level2">The BLAS level 2 implementation.</param>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="a">The triangular matrix A with dimension (<paramref name="n"/>, <paramref name="n"/>).</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A', 'op(A)=A^t' or 'op(A)=A^h'.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        public static void ztrsv(this ILevel2BLAS level2, int n, ReadOnlySpan<Complex> a, Span<Complex> x, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1)
        {
            level2.ztrsv(n, a, x, n, triangularMatrixType, isUnitTriangular, transpose, incX);
        }
        #endregion
#pragma warning restore IDE1006 // Naming Styles
    }
}