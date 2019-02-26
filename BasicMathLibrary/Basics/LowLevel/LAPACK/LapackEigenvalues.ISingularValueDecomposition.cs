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
        #region nested enumerations etc.

        /// <summary>Represents the calculation type of left singular vectors within a singular value decomposition, i.e. 'U' with respect to the decomposition A=U * \Sigma * V^t.
        /// </summary>
        public enum SVDleftSingularVectorsJob
        {
            /// <summary>All m columns of U are returned in the array u. 
            /// </summary>
            All,

            /// <summary>The first min(m, n) columns of U (the left singular vectors) are returned in the array u.
            /// </summary>
            OutOfPlace,

            /// <summary>The first min(m, n) columns of U (the left singular vectors) are overwritten on the array a; the parameter u is not referenced.
            /// </summary>
            InPlace,

            /// <summary>No columns of U (no left singular vectors) are compute, the parameter u is not referenced.
            /// </summary>
            NoLeftSingularVectors,
        }

        /// <summary>Represents the calculation type of right singular vectors within a singular 
        /// value decomposition, i.e. 'V^t' with respect to the decomposition A=U * \Sigma * V^t.
        /// </summary>
        public enum SVDrightSingularVectorsJob
        {
            /// <summary>All n rows of V^t are returned in the array vt. 
            /// </summary>
            All,

            /// <summary>The first min(m, n) rows of V^t (the right singular vectors) are returned in the array vt.
            /// </summary>
            OutOfPlace,

            /// <summary>The first min(m, n) rows of V^t (the right singular vectors) are overwritten on the array a; the parameter vt is not referenced.
            /// </summary>
            InPlace,

            /// <summary>No rows of V^t (no right singular vectors) are compute, the
            /// parameter vt is not referenced.
            /// </summary>
            NoLeftSingularVectors,
        }

        /// <summary>A value indicating what kind of job to do by the LAPACK function 'dgbbrd' and 'zgbbrd'.
        /// </summary>
        [Flags]
        public enum SVDxgbbrdJob
        {
            /// <summary>Neither matrix Q nor P^H is generated.
            /// </summary>
            None = 0,

            /// <summary>Compute matrix Q.
            /// </summary>
            MatrixQ = 0x02,

            /// <summary>Compute matrix P^H.
            /// </summary>
            MatrixP = 0x04,

            /// <summary>Compute matrix Q and matrix P^H.
            /// </summary>
            Both = MatrixQ | MatrixP
        }

        /// <summary>A value indicating which matrix to compute for one of the following LAPACK functions: 'dorgbr', 'zungbr'.
        /// </summary>
        public enum SVDxorgbrJob
        {
            /// <summary>Compute matrix Q.
            /// </summary>
            MatrixQ,

            /// <summary>Compute matrix P'.
            /// </summary>
            MatrixP
        }

        /// <summary>A value indicating which matrix to take into account for the multiplication for the LAPACK functions 'dormbr' and 'zunmbr'.
        /// </summary>
        public enum SVDxormbrJob
        {
            /// <summary>Apply matrix Q in the multiplication.
            /// </summary>
            ApplyQ,

            /// <summary>Apply matrix P in the multiplication.
            /// </summary>
            ApplyP
        }

        /// <summary>Specifies the job for LAPACK function 'ddbsdc'.
        /// </summary>
        public enum SVDdbdsdcJob
        {
            /// <summary>Calculate singular values only.
            /// </summary>
            SingularValuesOnly,

            /// <summary>Calculate singular values and singular vectors.
            /// </summary>
            SingularValuesAndSingularVectors,

            /// <summary>Calculate singular values and singular vectors in compact form.
            /// </summary>
            SingularValuesAndSingularVectorsInCompactForm
        }

        /// <summary>Specifies the job for LAPACK routine 'dgesdd' and 'zgesdd'.
        /// </summary>
        public enum SVDxgesddJob
        {
            /// <summary>All columns of U and all rows of V'/conjg(V') are computed.
            /// </summary>
            All,

            /// <summary>The first min(m,n) columns of U and the first min(m,n) rows of V'/conjg(V') are computed.
            /// </summary>
            Selected,

            /// <summary>The first n columns of U are overwritten in the array a and all rows of V'/conjg(V') are returned in array vt.
            /// </summary>
            Overwritten,

            /// <summary>No columns of U or rows of V'/conjg(V') are computed.
            /// </summary>
            None
        }

        /// <summary>Specifies the level of accuracy.
        /// </summary>
        public enum SVDdgejsvJobA
        {
            /// <summary>High relative accuracy.
            /// </summary>
            C,

            /// <summary>High relative accuracy with an additional estimate of the condition number of B. It provides a realistic error bound.
            /// </summary>
            E,

            /// <summary>Higher accuracy than <see cref="SVDdgejsvJobA.C"/> is achieved. This option is advisable, if the structure of the input matrix it not known and relative accuracy is desirable.
            /// </summary>
            F,

            /// <summary>Similar to <see cref="SVDdgejsvJobA.F"/> with an additional estimate of the condition number of B. If A has heavily weighted rows, using this condition number gives too pessimistic error bound.
            /// </summary>
            G,

            /// <summary>Small singular values are the noise and the matrix is treated as numerically rank defficient. This enables the procedure to set all singular values below n * \epsilon ||A|| to zero.
            /// </summary>
            A,

            /// <summary>Similar to <see cref="SVDdgejsvJobA.A"/>. The SVD is computed with absolute error bounds, but more accurately than <see cref="SVDdgejsvJobA.A"/>.
            /// </summary>
            R
        }

        /// <summary>Specifies whether to compute the columns of the matrix U.
        /// </summary>
        public enum SVDdgejsvJobU
        {
            /// <summary>n columns of matrix U are returned in parameter u.
            /// </summary>
            U,

            /// <summary>A full set of m left singular vectors is returned in the array u.
            /// </summary>
            F,

            /// <summary>The parameter u may be used as workspace of length m * n.
            /// </summary>
            W,

            /// <summary>The matrix U is not computed.
            /// </summary>
            None
        }

        /// <summary>Specifies whether to compute the matrix V.
        /// </summary>
        public enum SVDdgejsvJobV
        {
            /// <summary>n columns of V are returnd in array v; Jacobi rotations are not explicitly accumulated.
            /// </summary>
            V,

            /// <summary>n colums of V are returned in array v but the are computed as the product of Jacobi rotations.
            /// </summary>
            J,

            /// <summary>The parameter v may be used as workspace of length n * n.
            /// </summary>
            W,

            /// <summary>The matrix V is not computed.
            /// </summary>
            None
        }

        /// <summary>Specifies the range for the singular values. If small positive singular values are outside the specified rante, they may be set to zero.
        /// </summary>
        public enum SVDdgejsvJobR
        {
            /// <summary>Does not remove small columns of the scaled matrix.
            /// </summary>
            None,

            /// <summary>Restricted range for singular values of the scaled matrix A.
            /// </summary>
            Restricted
        }

        /// <summary>If the matrix is square, the procedure may determine to use the transposed A if A' seems to be better with respect to convergence.
        /// </summary>
        public enum SVDdgejsvJobT
        {
            /// <summary>Performs transposition if the entropy test indicates possibly faster convergence of the Jacobi process.
            /// </summary>
            T,

            /// <summary>Attemps no speculations.
            /// </summary>
            None
        }

        /// <summary>Enables structured perturbations of denormalized numbers.
        /// </summary>
        public enum SVDdgejsvJobP
        {
            /// <summary>Introduces pertubations.
            /// </summary>
            Perturbation,

            /// <summary>No pertubations.
            /// </summary>
            None
        }

        /// <summary>Specifies the type of the matrix A for LAPACK routine 'dgesvj'.
        /// </summary>
        public enum SVDdgesvjJobA
        {
            /// <summary>The input matrix A is lower triangular.
            /// </summary>
            LowerTriangular,

            /// <summary>The input matrix A ist upper triangular.
            /// </summary>
            UpperTriangular,

            /// <summary>The input matrix A is a general matrix.
            /// </summary>
            GeneralMatrix
        }

        /// <summary>Specifies the job for parameter 'u' of LAPACK routine 'dgesvj'.
        /// </summary>
        public enum SVDdgesvjJobU
        {
            /// <summary>Compute left singular vectors.
            /// </summary>
            LeftSingularVectors,

            /// <summary>Compute left singular vectors. The level of numerical orthogonality of the computed left singular vectors can be controlled.
            /// </summary>
            ControlLeftSingularVectors,

            /// <summary>The specified matrix is not computed.
            /// </summary>
            None
        }

        /// <summary>Specifies the job for parameter 'v' of LAPACK routine 'dgesvj'.
        /// </summary>
        public enum SVDdgesvjJobV
        {
            /// <summary>Compute the specific matrix.
            /// </summary>
            ComputeMatrix,

            /// <summary>Apply Jacobi rotation, i.e. the right singular vector is not computed explicitly.
            /// </summary>
            ApplyJacobiRotation,

            /// <summary>The specified matrix is not computed.
            /// </summary>
            None
        }

        /// <summary>Represents the job of the LAPACK routine 'dggsvd' and 'zggsvd'.
        /// </summary>
        public enum SVDxggsvdJob
        {
            /// <summary>Compute the specified orthogonal/unitary matrix.
            /// </summary>
            ComputeMatrix,

            /// <summary>Do not compute the specified orthogonal/unitary matrix.
            /// </summary>
            None
        }
        #endregion

        /// <summary>Provides methods for computing the singular value decomposition (SVD) of a general m-by-n matrix. See the documentation of the Linear Algebra PACKage http://www.netlib.org/lapack/index.html.
        /// </summary>
        public interface ISingularValueDecomposition
        {
            /// <summary>Gets a optimal workspace array length for the <c>dgebrd</c> function.
            /// </summary>
            /// <param name="m">The number of rows in the matrix.</param>
            /// <param name="n">The number of columns in the matrix.</param>
            /// <returns>The optimal workspace array length.</returns>
            int dgebrdQuery(int m, int n);

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
            void dgebrd(int m, int n, Span<double> a, Span<double> d, Span<double> e, Span<double> tauq, Span<double> taup, Span<double> work);

            /// <summary>Gets a optimal workspace array length for the <c>zgebrd</c> function.
            /// </summary>
            /// <param name="m">The number of rows in the matrix.</param>
            /// <param name="n">The number of columns in the matrix.</param>
            /// <returns>The optimal workspace array length.</returns>
            int zgebrdQuery(int m, int n);

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
            void zgebrd(int m, int n, Span<Complex> a, Span<double> d, Span<double> e, Span<Complex> tauq, Span<Complex> taup, Span<Complex> work);

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
            void dgbbrd(SVDxgbbrdJob job, int m, int n, int ncc, int kl, int ku, Span<double> ab, Span<double> d, Span<double> e, Span<double> q, Span<double> pt, Span<double> c, Span<double> work);

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
            void zgbbrd(SVDxgbbrdJob job, int m, int n, int ncc, int kl, int ku, Span<Complex> ab, Span<double> d, Span<double> e, Span<Complex> q, Span<Complex> pt, Span<Complex> c, Span<Complex> work, Span<double> rwork);

            /// <summary>Gets a optimal workspace array length for the <c>dorgbr</c> function.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="m">The number of rows in the matrix Q or P' to be returned.</param>
            /// <param name="n">The number of columns in the matrix Q or P' to be returned.</param>
            /// <param name="k">The number of columns/rows in the original <paramref name="m"/>-by-<paramref name="k"/> or <paramref name="k"/>-by-<paramref name="n"/> matrix reduced by <c>dgebrd</c>.</param>
            /// <returns>The optimal workspace array length.</returns>
            int dorgbrQuery(SVDxorgbrJob job, int m, int n, int k);

            /// <summary>Generates the real orthogonal matrix Q or P' determined by <c>dgebrd</c>.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="m">The number of rows in the matrix Q or P' to be returned.</param>
            /// <param name="n">The number of columns in the matrix Q or P' to be returned.</param>
            /// <param name="k">The number of columns/rows in the original <paramref name="m"/>-by-<paramref name="k"/> or <paramref name="k"/>-by-<paramref name="n"/> matrix reduced by <c>dgebrd</c>.</param>
            /// <param name="a">The vectors which define the elementary reflectors, as returned by <c>dgebrd</c>; overwritten by the orthogonal matrix Q or P'.</param>
            /// <param name="tau">Scalar factor of the elementary reflector H(i) or G(i) which determines Q and P' as returned by <c>dgebrd</c> in the array <c>tauq</c> or <c>taup</c>.</param>
            /// <param name="work">A workspace array.</param>
            void dorgbr(SVDxorgbrJob job, int m, int n, int k, Span<double> a, Span<double> tau, Span<double> work);

            /// <summary>Gets a optimal workspace array length for the <c>dormbr</c> function.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="m">The number of rows in the matrix C.</param>
            /// <param name="n">The number of columns in the matrix C.</param>        
            /// <param name="k">One of the dimension of matrix A in <c>dgebrd</c>. If <paramref name="job"/> indicates matrix Q it is the number of columns in A; otherwise the number of rows.</param>
            /// <param name="side">A value indicating whether multipliers are applied to matrix C from the left or from the right.</param>
            /// <param name="transposeState">A value indicating whether the routine multiplies C by X or X', where X is matrix P or matrix Q with respect to <paramref name="job"/>.</param>
            /// <returns>The optimal workspace array length.</returns>
            int dormbrQuery(SVDxormbrJob job, int m, int n, int k, LAPACK.Side side = LAPACK.Side.Left, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose);

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
            void dormbr(SVDxormbrJob job, int m, int n, int k, Span<double> a, Span<double> tau, Span<double> c, Span<double> work, LAPACK.Side side = LAPACK.Side.Left, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose);

            /// <summary>Gets a optimal workspace array length for the <c>zungbr</c> function.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="m">The number of rows in the matrix Q or P' to be returned.</param>
            /// <param name="n">The number of columns in the matrix Q or P' to be returned.</param>
            /// <param name="k">The number of columns/rows in the original <paramref name="m"/>-by-<paramref name="k"/> or <paramref name="k"/>-by-<paramref name="n"/> matrix reduced by <c>dgebrd</c>.</param>
            /// <returns>The optimal workspace array length.</returns>
            int zungbrQuery(SVDxorgbrJob job, int m, int n, int k);

            /// <summary>Generates the complex unitary matrix Q or P^H determined by <c>zgebrd</c>.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="m">The number of rows in the matrix Q or P' to be returned.</param>
            /// <param name="n">The number of columns in the matrix Q or P' to be returned.</param>
            /// <param name="k">The number of columns/rows in the original <paramref name="m"/>-by-<paramref name="k"/> or <paramref name="k"/>-by-<paramref name="n"/> matrix reduced by <c>dgebrd</c>.</param>
            /// <param name="a">The vectors which define the elementary reflectors, as returned by <c>dgebrd</c>; overwritten by the orthogonal matrix Q or P'.</param>
            /// <param name="tau">Scalar factor of the elementary reflector H(i) or G(i) which determines Q and P' as returned by <c>dgebrd</c> in the array <c>tauq</c> or <c>taup</c>.</param>
            /// <param name="work">A workspace array.</param>
            void zungbr(SVDxorgbrJob job, int m, int n, int k, Span<Complex> a, Span<Complex> tau, Span<Complex> work);

            /// <summary>Gets a optimal workspace array length for the <c>zunmbr</c> function.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="m">The number of rows in the matrix C.</param>
            /// <param name="n">The number of columns in the matrix C.</param>        
            /// <param name="k">One of the dimension of matrix A in <c>dgebrd</c>. If <paramref name="job"/> indicates matrix Q it is the number of columns in A; otherwise the number of rows.</param>
            /// <param name="side">A value indicating whether multipliers are applied to matrix C from the left or from the right.</param>
            /// <param name="transposeState">A value indicating whether the routine multiplies C by X or X', where X is matrix P or matrix Q with respect to <paramref name="job"/>.</param>
            /// <returns>The optimal workspace array length.</returns>
            int zunmbrQuery(SVDxormbrJob job, int m, int n, int k, LAPACK.Side side = LAPACK.Side.Left, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose);

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
            void zunmbr(SVDxormbrJob job, int m, int n, int k, Span<Complex> a, Span<Complex> tau, Span<Complex> c, Span<Complex> work, LAPACK.Side side = LAPACK.Side.Left, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose);

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
            void dbsdsqr(int n, int ncvt, int nru, int ncc, Span<double> d, Span<double> e, Span<double> vt, Span<double> u, Span<double> c, Span<double> work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

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
            void zbsdsqr(int n, int ncvt, int nru, int ncc, Span<double> d, Span<double> e, Span<Complex> vt, Span<Complex> u, Span<Complex> c, Span<double> work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

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
            void dbdsdc(SVDdbdsdcJob job, int n, Span<double> d, Span<double> e, Span<double> u, Span<double> vt, Span<double> q, int[] iq, Span<double> work, int[] iwork, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Gets a optimal workspace array length for the <c>dgesvd</c> function.
            /// </summary>
            /// <param name="m">The number of rows of matrix A.</param>
            /// <param name="n">The number of columns of matrix A.</param>
            /// <param name="uJob">A value indicating what kind of job to do by the LAPACK function (for matrix U).</param>
            /// <param name="vtJob">A value indicating what kind of job to do by the LAPACK function (for matrix V').</param>
            int driver_dgesvdQuery(int m, int n, LapackEigenvalues.SVDleftSingularVectorsJob uJob = SVDleftSingularVectorsJob.All, LapackEigenvalues.SVDrightSingularVectorsJob vtJob = SVDrightSingularVectorsJob.All);

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
            /// <param name="uJob">A value indicating what kind of job to do by the LAPACK function (for matrix U).</param>
            /// <param name="vtJob">A value indicating what kind of job to do by the LAPACK function (for matrix V').</param>
            /// <remarks>The singular values are the roots of the non-negative eigenvalues of A^t*A.</remarks>        
            void driver_dgesvd(int m, int n, Span<double> a, Span<double> s, Span<double> u, Span<double> vt, Span<double> work, LapackEigenvalues.SVDleftSingularVectorsJob uJob = SVDleftSingularVectorsJob.All, LapackEigenvalues.SVDrightSingularVectorsJob vtJob = SVDrightSingularVectorsJob.All);

            /// <summary>Gets a optimal workspace array length for the <c>zgesvd</c> function.
            /// </summary>
            /// <param name="m">The number of rows of matrix A.</param>
            /// <param name="n">The number of columns of matrix A.</param>
            /// <param name="uJob">A value indicating what kind of job to do by the LAPACK function (for matrix U).</param>
            /// <param name="vtJob">A value indicating what kind of job to do by the LAPACK function (for matrix V').</param>
            int driver_zgesvdQuery(int m, int n, LapackEigenvalues.SVDleftSingularVectorsJob uJob = LapackEigenvalues.SVDleftSingularVectorsJob.All, LapackEigenvalues.SVDrightSingularVectorsJob vtJob = LapackEigenvalues.SVDrightSingularVectorsJob.All);

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
            void driver_zgesvd(int m, int n, Span<Complex> a, Span<double> s, Span<Complex> u, Span<Complex> vt, Span<Complex> work, Span<double> rwork, LapackEigenvalues.SVDleftSingularVectorsJob uJob = LapackEigenvalues.SVDleftSingularVectorsJob.All, LapackEigenvalues.SVDrightSingularVectorsJob vtJob = LapackEigenvalues.SVDrightSingularVectorsJob.All);

            /// <summary>Gets a optimal workspace array length for the <c>driver_dgesdd</c> function.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="m">The number of rows of matrix A.</param>
            /// <param name="n">The number of columns of matrix A.</param>
            /// <returns>The optimal workspace array length.</returns>
            int driver_dgesddQuery(LapackEigenvalues.SVDxgesddJob job, int m, int n);

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
            void driver_dgesdd(LapackEigenvalues.SVDxgesddJob job, int m, int n, Span<double> a, Span<double> s, Span<double> u, Span<double> vt, Span<double> work, int[] iwork);

            /// <summary>Gets a optimal workspace array length for the <c>driver_zgesdd</c> function.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="m">The number of rows of matrix A.</param>
            /// <param name="n">The number of columns of matrix A.</param>
            /// <returns>The optimal workspace array length.</returns>
            int driver_zgesddQuery(LapackEigenvalues.SVDxgesddJob job, int m, int n);

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
            /// <param name="rwork">A workspace array with dimension at least 5 * min(<paramref name="m"/>, <paramref name="n"/>) if <paramref name="job"/> equals <see cref="SVDxgesddJob.None"/>; min(m,n) * max(5*min(m,n) + 7, 2 * max(m,n) + 2 * min(m,n) +1) otherwise.</param>
            /// <param name="iwork">A workspace array with dimension at least 8 * min(<paramref name="m"/>, <paramref name="n"/>).</param>
            void driver_zgesdd(LapackEigenvalues.SVDxgesddJob job, int m, int n, Span<Complex> a, Span<double> s, Span<Complex> u, Span<Complex> vt, Span<Complex> work, Span<double> rwork, int[] iwork);

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
            void driver_dgejsv(LapackEigenvalues.SVDdgejsvJobA joba, LapackEigenvalues.SVDdgejsvJobU jobu, LapackEigenvalues.SVDdgejsvJobV jobv, LapackEigenvalues.SVDdgejsvJobR jobr, LapackEigenvalues.SVDdgejsvJobT jobt, LapackEigenvalues.SVDdgejsvJobP jobp, int m, int n, Span<double> a, Span<double> sva, Span<double> u, Span<double> v, Span<double> work, int[] iwork);

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
            void driver_dgesvj(LapackEigenvalues.SVDdgesvjJobA joba, LapackEigenvalues.SVDdgesvjJobU jobu, LapackEigenvalues.SVDdgesvjJobV jobv, int m, int n, Span<double> a, Span<double> sva, int mv, Span<double> v, Span<double> work);

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
            void driver_dggsvd(LapackEigenvalues.SVDxggsvdJob jobu, LapackEigenvalues.SVDxggsvdJob jobv, LapackEigenvalues.SVDxggsvdJob jobq, int m, int n, int p, out int k, out int l, Span<double> a, Span<double> b, Span<double> alpha, Span<double> beta, Span<double> u, Span<double> v, Span<double> q, Span<double> work, int[] iwork);

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
            void driver_zggsvd(LapackEigenvalues.SVDxggsvdJob jobu, LapackEigenvalues.SVDxggsvdJob jobv, LapackEigenvalues.SVDxggsvdJob jobq, int m, int n, int p, out int k, out int l, Span<Complex> a, Span<Complex> b, Span<double> alpha, Span<double> beta, Span<Complex> u, Span<Complex> v, Span<Complex> q, Span<Complex> work, Span<double> rwork, int[] iwork);
        }
    }
}