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
using System.Text;
using System.Numerics;

namespace Dodoni.MathLibrary.Basics.LowLevel
{
    /// <summary>Serves as interface for level 2 Basic Linear Algebra Subprograms (BLAS), i.e. <para>matrix-vector operations.</para>
    /// </summary>
    /// <remarks>The function names are almost identically to the BLAS naming convention, see http://www.netlib.org/blas for further information. 
    /// Here we restrict ourself to double precision and complex number arguments, the order of the arguments are other than the standard BLAS.</remarks>
    public interface ILevel2BLAS
    {
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
        /// <param name="lda">The leading dimension, must be at least <paramref name="kl"/> + <paramref name="ku"/> +1.</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
        void dgbmv(int m, int n, int kl, int ku, double alpha, double[] a, double[] x, double beta, double[] y, int lda, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1, int incY = 1);

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
        void dgemv(int m, int n, double alpha, double[] a, double[] x, double beta, double[] y, int lda, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1, int incY = 1);

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
        void dger(int m, int n, double alpha, double[] x, double[] y, double[] a, int lda, int incX = 1, int incY = 1);

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
        void dsbmv(int n, int k, double alpha, double[] a, double[] x, double beta, double[] y, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1, int incY = 1);

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
        void dspmv(int n, double alpha, double[] aPacked, double[] x, double beta, double[] y, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1, int incY = 1);

        /// <summary>Performs a rank-1 update of a symmetric packed matrix, i.e. A := \alpha * x * x^t + A.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
        /// <param name="aPacked">The symmetric packed matrix A with dimension at least (<paramref name="n"/> * (<paramref name="n"/> + 1) ) / 2.</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        void dspr(int n, double alpha, double[] x, double[] aPacked, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1);

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
        void dspr2(int n, double alpha, double[] x, double[] y, double[] aPacked, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1, int incY = 1);

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
        void dsymv(int n, double alpha, double[] a, double[] x, double beta, double[] y, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1, int incY = 1);

        /// <summary>Performs a rank-1 update of a symmetric matrix, i.e. A := \alpha * x * x^t + A.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
        /// <param name="a">The symmetric matrix A of dimension (<paramref name="lda"/>, <paramref name="n"/>) supplied column-by-column (input/output).</param>
        /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1, <paramref name="n"/>).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        void dsyr(int n, double alpha, double[] x, double[] a, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1);

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
        void dsyr2(int n, double alpha, double[] x, double[] y, double[] a, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1, int incY = 1);

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
        void dtbmv(int n, int k, double[] a, double[] x, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1);

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
        void dtbsv(int n, int k, double[] a, double[] x, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1);

        /// <summary>Computes a matrix-vector product using a triangular packed matrix, i.e. x := op(A) * x, where op(A) = A or op(A) = A^t.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="aPacked">The triangular packed matrix A with dimension at least (<paramref name="n"/> * (<paramref name="n"/> + 1) ) / 2.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        void dtpmv(int n, double[] aPacked, double[] x, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1);

        /// <summary>Solves a system of linear equations whose coefficients are in a triangular packed matrix, i.e. op(A) * x = b, where op(A) = A or op(A) = A^t.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="aPacked">The triangular packed matrix A with dimension at least (<paramref name="n"/> * (<paramref name="n"/> + 1) ) / 2.</param>
        /// <param name="x">The vector b (input), x (output) with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements (input/output).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        void dtpsv(int n, double[] aPacked, double[] x, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1);

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
        void dtrmv(int n, double[] a, double[] x, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1);

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
        void dtrsv(int n, double[] a, double[] x, int lda, BLAS.TriangularMatrixType triangularMatrixType, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1);
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
        /// <param name="lda">The leading dimension, must be at least <paramref name="kl"/> + <paramref name="ku"/> +1.</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
        void zgbmv(int m, int n, int kl, int ku, Complex alpha, Complex[] a, Complex[] x, Complex beta, Complex[] y, int lda, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1, int incY = 1);

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
        void zgemv(int m, int n, Complex alpha, Complex[] a, Complex[] x, Complex beta, Complex[] y, int lda, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1, int incY = 1);

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
        void zgerc(int m, int n, Complex alpha, Complex[] x, Complex[] y, Complex[] a, int lda, int incX = 1, int incY = 1);

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
        void zgeru(int m, int n, Complex alpha, Complex[] x, Complex[] y, Complex[] a, int lda, int incX = 1, int incY = 1);

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
        void zhbmv(int n, int k, Complex alpha, Complex[] a, Complex[] x, Complex beta, Complex[] y, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1, int incY = 1);

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
        void zhemv(int n, Complex alpha, Complex[] x, Complex beta, Complex[] y, Complex[] a, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1, int incY = 1);

        /// <summary>Performs a rank-1 update of a Hermitian matrix, i.e. A := \alpha * x * conj(x^t) + A.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incX"/>| elements.</param>
        /// <param name="a">The Hermitian matrix A with dimension (<paramref name="lda"/>, <paramref name="n"/>).</param>
        /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1, <paramref name="n"/>).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        void zher(int n, double alpha, Complex[] x, Complex[] a, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1);

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
        void zher2(int n, Complex alpha, Complex[] x, Complex[] y, Complex[] a, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1, int incY = 1);

        /// <summary>Computes a matrix-vector product using a Hermitian packed matrix, i.e. y := \alpha * A * x + \beta * y.
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
        void zhpmv(int n, Complex alpha, Complex[] aPacked, Complex[] x, Complex beta, Complex[] y, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1, int incY = 1);

        /// <summary>Performs a rank-1 update of a Hermitian packed matrix, i.e. A := \alpha * x * conjg(x^t) + A.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incX"/>| elements.</param>
        /// <param name="aPacked">The Hermitian packed matrix A with dimension at least (<paramref name="n"/> * (<paramref name="n"/> + 1) ) / 2.</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        void zhpr(int n, double alpha, Complex[] x, Complex[] aPacked, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1);

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
        void zhpr2(int n, Complex alpha, Complex[] x, Complex[] y, Complex[] aPacked, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1, int incY = 1);

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
        void ztbmv(int n, int k, Complex[] a, Complex[] x, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1);

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
        void ztbsv(int n, int k, Complex[] a, Complex[] x, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1);

        /// <summary>Computes a matrix-vector product using a triangular packed matrix, i.e. x := op(A) * x, where op(A) = A, op(A) = A ^t or op(A) = A^h.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="aPacked">The triangular packed matrix A with dimension at least (<paramref name="n"/> * (<paramref name="n"/> + 1) ) / 2.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A', 'op(A)=A^t' or 'op(A)=A^h'.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        void ztpmv(int n, Complex[] aPacked, Complex[] x, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1);

        /// <summary>Solves a system of linear equations whose coefficients are in a triangular packed matrix, i.e. op(A) * x = b, where op(A) = A, op(A) = A ^t or op(A) = A^h.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="aPacked">The triangular packed matrix A with dimension at least (<paramref name="n"/> * (<paramref name="n"/> + 1) ) / 2.</param>
        /// <param name="x">The vector b (input), x (output) with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements (input/output).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A', 'op(A)=A^t' or 'op(A)=A^h'.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        void ztpsv(int n, Complex[] aPacked, Complex[] x, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1);

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
        void ztrmv(int n, Complex[] a, Complex[] x, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1);

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
        void ztrsv(int n, Complex[] a, Complex[] x, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1);
        #endregion
    }
}