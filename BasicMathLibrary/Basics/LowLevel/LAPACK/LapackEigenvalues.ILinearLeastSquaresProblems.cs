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

namespace Dodoni.MathLibrary.Basics.LowLevel
{
    public partial class LapackEigenvalues
    {
        /// <summary>Provides methods for Linear Least Squares Problems. See the documentation of the Linear Algebra PACKage http://www.netlib.org/lapack/index.html.
        /// </summary>
        public interface ILinearLeastSquaresProblems
        {
            /// <summary>Gets a optimal workspace array length for the <c>dgels</c> function.
            /// </summary>
            /// <param name="m">The number of rows of the matrix A.</param>
            /// <param name="n">The number of columns of the matrix A.</param>
            /// <param name="nrhs">The number of right-hand sides; the number of columns in b.</param>
            /// <param name="transposeState">A value indicating whether to take into account A or A'.</param>
            /// <returns>The optimal workspace array length.</returns>
            int driver_dgelsQuery(int m, int n, int nrhs, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose);

            /// <summary>Uses QR or LQ factorization to solve a overdetermined or underdetermined linear system with full rank matrix, i.e. minimize ||b - op(A) * x||.
            /// </summary>
            /// <param name="m">The number of rows of the matrix A.</param>
            /// <param name="n">The number of columns of the matrix A.</param>
            /// <param name="nrhs">The number of right-hand sides; the number of columns in b.</param>
            /// <param name="a">The <paramref name="m"/>-by-<paramref name="n"/> matrix provided column-by-column.</param>
            /// <param name="b">The <paramref name="m"/>-by-<paramref name="nrhs"/> matrix B of right hand side vectors, stored columnwise.</param>
            /// <param name="work">A workspace array.</param>
            /// <param name="transposeState">A value indicating whether to take into account A or A'.</param>
            void driver_dgels(int m, int n, int nrhs, Span<double> a, Span<double> b, Span<double> work, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose);

            /// <summary>Gets a optimal workspace array length for the <c>zgels</c> function.
            /// </summary>
            /// <param name="m">The number of rows of the matrix A.</param>
            /// <param name="n">The number of columns of the matrix A.</param>
            /// <param name="nrhs">The number of right-hand sides; the number of columns in b.</param>
            /// <param name="transposeState">A value indicating whether to take into account A or A'.</param>
            /// <returns>The optimal workspace array length.</returns>
            int driver_zgelsQuery(int m, int n, int nrhs, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose);

            /// <summary>Uses QR or LQ factorization to solve a overdetermined or underdetermined linear system with full rank matrix, i.e. minimize ||b - op(A) * x||.
            /// </summary>
            /// <param name="m">The number of rows of the matrix A.</param>
            /// <param name="n">The number of columns of the matrix A.</param>
            /// <param name="nrhs">The number of right-hand sides; the number of columns in b.</param>
            /// <param name="a">The <paramref name="m"/>-by-<paramref name="n"/> matrix provided column-by-column.</param>
            /// <param name="b">The <paramref name="m"/>-by-<paramref name="nrhs"/> matrix B of right hand side vectors, stored columnwise.</param>
            /// <param name="work">A workspace array.</param>
            /// <param name="transposeState">A value indicating whether to take into account A or A'.</param>
            void driver_zgels(int m, int n, int nrhs, Span<Complex> a, Span<Complex> b, Span<Complex> work, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose);

            /// <summary>Gets a optimal workspace array length for the <c>dgelsy</c> function.
            /// </summary>
            /// <param name="m">The number of rows of the matrix A.</param>
            /// <param name="n">The number of columns of the matrix A.</param>
            /// <param name="nrhs">The number of right-hand sides; the number of columns in b.</param>
            /// <param name="rcond">Is used to determine the effective rank of A, which is defined as the order of the largest leading triangular submatrix R_11 in the 
            /// QR factorization with pivoting of A, whose estimated condition number &lt; 1 / rcond.</param>
            /// <returns>The optimal workspace array length.</returns>
            int driver_dgelsyQuery(int m, int n, int nrhs, double rcond);

            /// <summary>Computes the minimum-norm solution to a linear least squares problem using a complete orthogonal factorization of A, i.e. minimize ||b - A * x||.
            /// </summary>
            /// <param name="m">The number of rows of the matrix A.</param>
            /// <param name="n">The number of columns of the matrix A.</param>
            /// <param name="nrhs">The number of right-hand sides; the number of columns in b.</param>
            /// <param name="a">The <paramref name="m"/>-by-<paramref name="n"/> matrix provided column-by-column.</param>
            /// <param name="b">The <paramref name="m"/>-by-<paramref name="nrhs"/> matrix B of right hand side vectors, stored columnwise.</param>
            /// <param name="jpvt">On entry, if jpvt[j] != 0, the i-th column of A is permuted to the front of AP, otherwise i-th column of A is a free column.</param>
            /// <param name="rcond">Is used to determine the effective rank of A, which is defined as the order of the largest leading triangular submatrix R_11 in the 
            /// QR factorization with pivoting of A, whose estimated condition number &lt; 1 / rcond.</param>
            /// <param name="work">A workspace array.</param>
            /// <param name="rank">The effective rank of A, which is defined as the order of the largest leading triangular submatrix R_11 in the 
            /// QR factorization with pivoting of A, whose estimated condition number &lt; 1 / rcond.</param>
            void driver_dgelsy(int m, int n, int nrhs, Span<double> a, Span<double> b, int[] jpvt, double rcond, Span<double> work, out int rank);

            /// <summary>Gets a optimal workspace array length for the <c>zgelsy</c> function.
            /// </summary>
            /// <param name="m">The number of rows of the matrix A.</param>
            /// <param name="n">The number of columns of the matrix A.</param>
            /// <param name="nrhs">The number of right-hand sides; the number of columns in b.</param>
            /// <param name="rcond">Is used to determine the effective rank of A, which is defined as the order of the largest leading triangular submatrix R_11 in the 
            /// QR factorization with pivoting of A, whose estimated condition number &lt; 1 / rcond.</param>
            /// <returns>The optimal workspace array length.</returns>
            int driver_zgelsyQuery(int m, int n, int nrhs, double rcond);

            /// <summary>Computes the minimum-norm solution to a linear least squares problem using a complete orthogonal factorization of A, i.e. minimize ||b - A * x||.
            /// </summary>
            /// <param name="m">The number of rows of the matrix A.</param>
            /// <param name="n">The number of columns of the matrix A.</param>
            /// <param name="nrhs">The number of right-hand sides; the number of columns in b.</param>
            /// <param name="a">The <paramref name="m"/>-by-<paramref name="n"/> matrix provided column-by-column.</param>
            /// <param name="b">The <paramref name="m"/>-by-<paramref name="nrhs"/> matrix B of right hand side vectors, stored columnwise.</param>
            /// <param name="jpvt">On entry, if jpvt[j] != 0, the i-th column of A is permuted to the front of AP, otherwise i-th column of A is a free column.</param>
            /// <param name="rcond">Is used to determine the effective rank of A, which is defined as the order of the largest leading triangular submatrix R_11 in the 
            /// QR factorization with pivoting of A, whose estimated condition number &lt; 1 / rcond.</param>
            /// <param name="work">A workspace array.</param>
            /// <param name="rwork">A workspace array with dimension at least 2 * <paramref name="n"/>.</param>
            /// <param name="rank">The effective rank of A, which is defined as the order of the largest leading triangular submatrix R_11 in the 
            /// QR factorization with pivoting of A, whose estimated condition number &lt; 1 / rcond.</param>
            void driver_zgelsy(int m, int n, int nrhs, Span<Complex> a, Span<Complex> b, int[] jpvt, double rcond, Span<Complex> work, Span<double> rwork, out int rank);

            /// <summary>Gets a optimal workspace array length for the <c>dgelss</c> function.
            /// </summary>
            /// <param name="m">The number of rows of the matrix A.</param>
            /// <param name="n">The number of columns of the matrix A.</param>
            /// <param name="nrhs">The number of right-hand sides; the number of columns in b.</param>
            /// <param name="rcond">Is used to determine the effective rank of A. Singular values s_i &lt;= rcond * s_1 are treated as zero.</param>
            /// <returns>The optimal workspace array length.</returns>
            int driver_dgelssQuery(int m, int n, int nrhs, double rcond);

            /// <summary>Computes the minimum-norm solution to a linear least squares problem using the singular value decomposition of A,  i.e. minimize ||b - A * x||.
            /// </summary>
            /// <param name="m">The number of rows of the matrix A.</param>
            /// <param name="n">The number of columns of the matrix A.</param>
            /// <param name="nrhs">The number of right-hand sides; the number of columns in b.</param>
            /// <param name="a">The <paramref name="m"/>-by-<paramref name="n"/> matrix provided column-by-column.</param>
            /// <param name="b">The <paramref name="m"/>-by-<paramref name="nrhs"/> matrix B of right hand side vectors, stored columnwise.</param>
            /// <param name="s">Contains the singular values of matrix A in decreasing order on exit; dimension at least min(<paramref name="m"/>, <paramref name="n"/>).</param>
            /// <param name="rcond">Is used to determine the effective rank of A. Singular values s_i &lt;= rcond * s_1 are treated as zero.</param>
            /// <param name="work">A workspace array.</param>
            /// <param name="rank">The effective rank of matrix A, that is, the number of singular values which are reater than <paramref name="rcond"/> * s_1 (output).</param>
            void driver_dgelss(int m, int n, int nrhs, Span<double> a, Span<double> b, Span<double> s, double rcond, Span<double> work, out int rank);

            /// <summary>Gets a optimal workspace array length for the <c>zgelss</c> function.
            /// </summary>
            /// <param name="m">The number of rows of the matrix A.</param>
            /// <param name="n">The number of columns of the matrix A.</param>
            /// <param name="nrhs">The number of right-hand sides; the number of columns in b.</param>
            /// <param name="rcond">Is used to determine the effective rank of A. Singular values s_i &lt;= rcond * s_1 are treated as zero.</param>
            /// <returns>The optimal workspace array length.</returns>
            int driver_zgelssQuery(int m, int n, int nrhs, double rcond);

            /// <summary>Computes the minimum-norm solution to a linear least squares problem using the singular value decomposition of A,  i.e. minimize ||b - A * x||.
            /// </summary>
            /// <param name="m">The number of rows of the matrix A.</param>
            /// <param name="n">The number of columns of the matrix A.</param>
            /// <param name="nrhs">The number of right-hand sides; the number of columns in b.</param>
            /// <param name="a">The <paramref name="m"/>-by-<paramref name="n"/> matrix provided column-by-column.</param>
            /// <param name="b">The <paramref name="m"/>-by-<paramref name="nrhs"/> matrix B of right hand side vectors, stored columnwise.</param>
            /// <param name="s">Contains the singular values of matrix A in decreasing order on exit; dimension at least min(<paramref name="m"/>, <paramref name="n"/>).</param>
            /// <param name="rcond">Is used to determine the effective rank of A. Singular values s_i &lt;= rcond * s_1 are treated as zero.</param>
            /// <param name="work">A workspace array.</param>
            /// <param name="rwork">A workspace array with dimension at least 5 * min(<paramref name="m"/>, <paramref name="n"/>).</param>
            /// <param name="rank">The effective rank of matrix A, that is, the number of singular values which are reater than <paramref name="rcond"/> * s_1 (output).</param>
            void driver_zgelss(int m, int n, int nrhs, Span<Complex> a, Span<Complex> b, Span<double> s, double rcond, Span<Complex> work, Span<double> rwork, out int rank);

            /// <summary>Gets a optimal workspace array length for the <c>dgelsd</c> function.
            /// </summary>
            /// <param name="m">The number of rows of the matrix A.</param>
            /// <param name="n">The number of columns of the matrix A.</param>
            /// <param name="nrhs">The number of right-hand sides; the number of columns in b.</param>
            /// <param name="rcond">Is used to determine the effective rank of A. Singular values s_i &lt;= rcond * s_1 are treated as zero.</param>
            /// <param name="ilwork">The optimal workspace array length for parameter 'iwork'.</param>
            /// <returns>The optimal workspace array length.</returns>
            int driver_dgelsdQuery(int m, int n, int nrhs, double rcond, out int ilwork);

            ///<summary>Computes the minimum-norm solution to a linear least squares problem using the singular value decomposition of A and a divide and conquer method, i.e. minimize ||b - A * x||.</summary>
            /// <param name="m">The number of rows of the matrix A.</param>
            /// <param name="n">The number of columns of the matrix A.</param>
            /// <param name="nrhs">The number of right-hand sides; the number of columns in b.</param>
            /// <param name="a">The <paramref name="m"/>-by-<paramref name="n"/> matrix provided column-by-column.</param>
            /// <param name="b">The <paramref name="m"/>-by-<paramref name="nrhs"/> matrix B of right hand side vectors, stored columnwise.</param>
            /// <param name="s">Contains the singular values of matrix A in decreasing order on exit; dimension at least min(<paramref name="m"/>, <paramref name="n"/>).</param>
            /// <param name="rcond">Is used to determine the effective rank of A. Singular values s_i &lt;= rcond * s_1 are treated as zero.</param>
            /// <param name="work">A workspace array.</param>
            /// <param name="iwork">A workspace array.</param>
            /// <param name="rank">The effective rank of matrix A, that is, the number of singular values which are reater than <paramref name="rcond"/> * s_1 (output).</param>
            void driver_dgelsd(int m, int n, int nrhs, Span<double> a, Span<double> b, Span<double> s, double rcond, Span<double> work, int[] iwork, out int rank);

            /// <summary>Gets a optimal workspace array length for the <c>zgelsd</c> function.
            /// </summary>
            /// <param name="m">The number of rows of the matrix A.</param>
            /// <param name="n">The number of columns of the matrix A.</param>
            /// <param name="nrhs">The number of right-hand sides; the number of columns in b.</param>
            /// <param name="rcond">Is used to determine the effective rank of A. Singular values s_i &lt;= rcond * s_1 are treated as zero.</param>
            /// <param name="ilwork">The optimal workspace array length for parameter 'iwork'.</param>
            /// <param name="rlwork">The optimal workspace array length for parameter 'rwork'.</param>
            /// <returns>The optimal workspace array length.</returns>
            int driver_zgelsdQuery(int m, int n, int nrhs, double rcond, out int ilwork, out int rlwork);

            ///<summary>Computes the minimum-norm solution to a linear least squares problem using the singular value decomposition of A and a divide and conquer method, i.e. minimize ||b - A * x||.</summary>
            /// <param name="m">The number of rows of the matrix A.</param>
            /// <param name="n">The number of columns of the matrix A.</param>
            /// <param name="nrhs">The number of right-hand sides; the number of columns in b.</param>
            /// <param name="a">The <paramref name="m"/>-by-<paramref name="n"/> matrix provided column-by-column.</param>
            /// <param name="b">The <paramref name="m"/>-by-<paramref name="nrhs"/> matrix B of right hand side vectors, stored columnwise.</param>
            /// <param name="s">Contains the singular values of matrix A in decreasing order on exit; dimension at least min(<paramref name="m"/>, <paramref name="n"/>).</param>
            /// <param name="rcond">Is used to determine the effective rank of A. Singular values s_i &lt;= rcond * s_1 are treated as zero.</param>
            /// <param name="work">A workspace array.</param>
            /// <param name="rwork">A workspace array.</param>
            /// <param name="iwork">A workspace array.</param>
            /// <param name="rank">The effective rank of matrix A, that is, the number of singular values which are reater than <paramref name="rcond"/> * s_1 (output).</param>
            void driver_zgelsd(int m, int n, int nrhs, Span<Complex> a, Span<Complex> b, Span<double> s, double rcond, Span<Complex> work, Span<double> rwork, int[] iwork, out int rank);

            /// <summary>Gets a optimal workspace array length for the <c>dgglse</c> function.
            /// </summary>
            /// <param name="m">The number of rows of the matrix A.</param>
            /// <param name="n">The number of columns of the matrices A and B.</param>
            /// <param name="p">The number of rows of the matrix B.</param>
            /// <returns>The optimal workspace array length.</returns>
            int driver_dgglseQuery(int m, int n, int p);

            /// <summary>Solves the linear equality-constrained least squares problem using a generalized RQ factorization, i.e. minimize ||c-A*x||^2 subject to B*x=d.
            /// </summary>
            /// <param name="m">The number of rows of the matrix A.</param>
            /// <param name="n">The number of columns of the matrices A and B.</param>
            /// <param name="p">The number of rows of the matrix B.</param>
            /// <param name="a">The <paramref name="m"/>-by-<paramref name="n"/> matrix provided column-by-column. On exist, the elements on and above the diagonal of the array contain the min(m,n)-by-n upper trapezoid matrix T.</param>
            /// <param name="b">The <paramref name="p"/>-by-<paramref name="n"/> matrix B of right hand side vectors, stored columnwise. On exit, the upper triangle of the subarray B</param>
            /// <param name="c">The right hand side vector for the least squares part of the LSE problem; dimension at least <paramref name="m"/>.</param>
            /// <param name="d">The right hand side vector for the constrained equation; dimension at least <paramref name="p"/>.</param>
            /// <param name="x">The solution of the LSE problem; dimension at least <paramref name="n"/>.</param>
            /// <param name="work">A workspace array.</param>
            void driver_dgglse(int m, int n, int p, Span<double> a, Span<double> b, Span<double> c, Span<double> d, Span<double> x, Span<double> work);

            /// <summary>Gets a optimal workspace array length for the <c>zgglse</c> function.
            /// </summary>
            /// <param name="m">The number of rows of the matrix A.</param>
            /// <param name="n">The number of columns of the matrices A and B.</param>
            /// <param name="p">The number of rows of the matrix B.</param>
            /// <returns>The optimal workspace array length.</returns>
            int driver_zgglseQuery(int m, int n, int p);

            /// <summary>Solves the linear equality-constrained least squares problem using a generalized RQ factorization, i.e. minimize ||c-A*x||^2 subject to B*x=d.
            /// </summary>
            /// <param name="m">The number of rows of the matrix A.</param>
            /// <param name="n">The number of columns of the matrices A and B.</param>
            /// <param name="p">The number of rows of the matrix B.</param>
            /// <param name="a">The <paramref name="m"/>-by-<paramref name="n"/> matrix provided column-by-column. On exist, the elements on and above the diagonal of the array contain the min(m,n)-by-n upper trapezoid matrix T.</param>
            /// <param name="b">The <paramref name="p"/>-by-<paramref name="n"/> matrix B of right hand side vectors, stored columnwise. On exit, the upper triangle of the subarray B</param>
            /// <param name="c">The right hand side vector for the least squares part of the LSE problem; dimension at least <paramref name="m"/>.</param>
            /// <param name="d">The right hand side vector for the constrained equation; dimension at least <paramref name="p"/>.</param>
            /// <param name="x">The solution of the LSE problem; dimension at least <paramref name="n"/>.</param>
            /// <param name="work">A workspace array.</param>
            void driver_zgglse(int m, int n, int p, Span<Complex> a, Span<Complex> b, Span<Complex> c, Span<Complex> d, Span<Complex> x, Span<Complex> work);

            /// <summary>Gets a optimal workspace array length for the <c>dggglm</c> function.
            /// </summary>
            /// <param name="m">The number of rows in the matrices A and B.</param>
            /// <param name="n">The number of columns in matrix A.</param>
            /// <param name="p">The number of columns in matrix B.</param>
            /// <returns>The optimal workspace array length.</returns>
            int driver_dggglmQuery(int n, int m, int p);

            /// <summary>Solves a general Gauss-Markov linear model problem using a generalized QR factorization, i.e. minimize ||y|| subject to d = A * x + B * y.
            /// </summary>
            /// <param name="m">The number of rows in the matrices A and B.</param>
            /// <param name="n">The number of columns in matrix A.</param>
            /// <param name="p">The number of columns in matrix B.</param>
            /// <param name="a">The <paramref name="n"/>-by-<paramref name="m"/> matrix A provided column-by-column.</param>
            /// <param name="b">The <paramref name="n"/>-by-<paramref name="p"/> matrix B provided column-by-column.</param>
            /// <param name="d">The left hand side vector for the constrained equation; dimension at least <paramref name="n"/>.</param>
            /// <param name="x">The solution x of the LSE problem; dimension at least <paramref name="m"/>.</param>
            /// <param name="y">The solution y of the LSE problem; dimension at least <paramref name="p"/>.</param>
            /// <param name="work">A workspace array.</param>
            void driver_dggglm(int n, int m, int p, Span<double> a, Span<double> b, Span<double> d, Span<double> x, Span<double> y, Span<double> work);

            /// <summary>Gets a optimal workspace array length for the <c>zggglm</c> function.
            /// </summary>
            /// <param name="m">The number of rows in the matrices A and B.</param>
            /// <param name="n">The number of columns in matrix A.</param>
            /// <param name="p">The number of columns in matrix B.</param>
            /// <returns>The optimal workspace array length.</returns>
            int driver_zggglmQuery(int n, int m, int p);

            /// <summary>Solves a general Gauss-Markov linear model problem using a generalized QR factorization, i.e. minimize ||y|| subject to d = A * x + B * y.
            /// </summary>
            /// <param name="m">The number of rows in the matrices A and B.</param>
            /// <param name="n">The number of columns in matrix A.</param>
            /// <param name="p">The number of columns in matrix B.</param>
            /// <param name="a">The <paramref name="n"/>-by-<paramref name="m"/> matrix A provided column-by-column.</param>
            /// <param name="b">The <paramref name="n"/>-by-<paramref name="p"/> matrix B provided column-by-column.</param>
            /// <param name="d">The left hand side vector for the constrained equation; dimension at least <paramref name="n"/>.</param>
            /// <param name="x">The solution x of the LSE problem; dimension at least <paramref name="m"/>.</param>
            /// <param name="y">The solution y of the LSE problem; dimension at least <paramref name="p"/>.</param>
            /// <param name="work">A workspace array.</param>
            void driver_zggglm(int n, int m, int p, Span<Complex> a, Span<Complex> b, Span<Complex> d, Span<Complex> x, Span<Complex> y, Span<Complex> work);
        }
    }
}