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
        /// <summary>Represents matrix balance approaches needed for example for the Lapack function 'dgebal' to improve the accuracy of computed eigenvalues and eigenvectors. 
        /// </summary>
        public enum NonSymmetricMatrixBalancesType
        {
            /// <summary>Neither permuted nor scale the matrix.
            /// </summary>
            NeitherPermutedNorScaled,

            /// <summary>Permute the matrix.
            /// </summary>
            Permuted,

            /// <summary>Scale the matrix.
            /// </summary>
            Scaled,

            /// <summary>Permuted and scale the matrix.
            /// </summary>
            PermutedAndScaled
        }

        /// <summary>The job for <c>?hseqr</c> function calls.
        /// </summary>
        public enum NonSymmetricXhseqrJob
        {
            /// <summary>Eigenvalues are required only.
            /// </summary>
            EigenvaluesOnly,

            /// <summary>Calculate Eigenvalues and Schur Factorization.
            /// </summary>
            EigenvaluesAndSchurForm
        }

        /// <summary>Represents the kind of operation for the Lapack method <c>?hseqr</c>.
        /// </summary>
        public enum NonSymmetricXhseqrOperation
        {
            /// <summary>Do not compute Schur vectors and input argument z of <c>?hseqr</c> is not referenced, the leading dimension of z can be set to <c>1</c>.
            /// </summary>
            NoSchurVectors,

            /// <summary>Compute the Schur vectors of H. The second dimension of argument z of <c>?hseqr</c> should be at least n and z need not be set.
            /// </summary>
            SchurVectorOfH,

            /// <summary>Compute the Schur vectors of the input matrix A. The second dimension of argument z of <c>?hseqr</c> should be at least n and z must contain the 
            /// matrix Q from the reduction to Hessenberg form on input.
            /// </summary>
            SchurVectorOfA
        }

        /// <summary>Specifies whether or not to order the eigenvalues on the diagonal of the Schur form.
        /// </summary>
        public enum NonSymmetricEigenvalueOrdering
        {
            /// <summary>The eigenvalues are not ordered.
            /// </summary>
            None,

            /// <summary>Eigenvalues are ordered.
            /// </summary>
            Sorted
        }

        /// <summary>Determines whether Schur vectors should be computed.
        /// </summary>
        public enum NonSymmetricSchurVectorsJob
        {
            /// <summary>Schur vectors are not computed.
            /// </summary>
            None,

            /// <summary>Schur vectors are computed.
            /// </summary>
            Compute
        }

        /// <summary>Determines whether to calculate left/right eigenvectors.
        /// </summary>
        public enum NonSymmetricEigenValueVectorsJob
        {
            /// <summary>Right eigenvectors are computed.
            /// </summary>
            Right = 0x01,

            /// <summary>Left eigenvectors are computed.
            /// </summary>
            Left = 0x02,

            /// <summary>All eigenvectors are computed.
            /// </summary>
            All = Right | Left
        }

        /// <summary>Determines whehter eigenvalues are found using <c>?hseqr</c>.
        /// </summary>
        public enum NonSymmetricEigenvalueSource
        {
            /// <summary>The eigenvalues of H were found using <c>?hseqr</c>; thus if H has any zero sub-diagonal elements, then the j-th eigenvalue can be assumed to be an eigenvalue of the block containing the j-th row/column.
            /// </summary>
            Xhseqr,

            /// <summary>The routine performs inverse itation using the whole matrix.
            /// </summary>
            Unknown
        }

        /// <summary>Determines whether initial estimates for specific eigenvectors are supplied.
        /// </summary>
        public enum NonSymmetricXhseinInitV
        {
            /// <summary>No initial estimates for the selected eigenvectors are supplied.
            /// </summary>
            None,

            /// <summary>Initial estimates for slected eigenvectors are supplied in parameter vl and/or vr.
            /// </summary>
            InitialEstimates
        }

        /// <summary>Determines which reciprocal condition number are computed.
        /// </summary>
        public enum NonSymmetricSense
        {
            /// <summary>None reciprocal condition number are computed.
            /// </summary>
            None,

            /// <summary>Compute reciprocal condition number for average of selected eigenvalues only.
            /// </summary>
            AverageOfSelectedEigenvalues,

            /// <summary>Compute reciprocal condition number for selected right invariant subspace only.
            /// </summary>
            SelectedRightInvariantSubspace,

            /// <summary>Compute reciprocal condition number for average of selected eigenvalues and for selected right invariant subspace.
            /// </summary>
            All = AverageOfSelectedEigenvalues | SelectedRightInvariantSubspace
        }

        /// <summary>Determines which reciprocal condition number are computed.
        /// </summary>
        public enum NonSymmetricXgeevxSense
        {
            /// <summary>None reciprocal condition number are computed.
            /// </summary>
            None,

            /// <summary>Compute reciprocal condition number for eigenvalues only.
            /// </summary>
            Eigenvalues,

            /// <summary>Compute reciprocal condition number for right eigenvectors only.
            /// </summary>
            RightEigenvectors,

            /// <summary>Compute reciprocal condition number for eigenvalues and right eigenvectors.
            /// </summary>
            EigenvaluesAndrightEigenvectors
        }

        /// <summary>Determines which eigenvectors to compute by function 'dtrevc' and 'ztrevc'.
        /// </summary>
        public enum NonSymmetricXtrevcOperation
        {
            /// <summary>Compute all specified eigenvectors.
            /// </summary>
            All,

            /// <summary>Compute all specified eigenvectors and backtransformed by the matrices supplied in parameter vl and vr.
            /// </summary>
            AllBacktransformed,

            /// <summary>Compute selected eigenvectors.
            /// </summary>
            SelectedEigenvectors
        }

        /// <summary>Provides methods of non-symmetric Eigenvalue problems. For more information see the documentation of the Linear Algebra PACKage http://www.netlib.org/lapack/index.html.
        /// </summary>
        public interface INonSymmetricEigenvalueProblems
        {
#pragma warning disable IDE1006 // Naming Styles

            /// <summary>Gets a optimal workspace array length for the <c>dgehrd</c> function.
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="ilo">If the input matrix is an output by <c>?gebal</c> this parameter must contain the value returned by that routine; otherwise <c>1</c>.</param>
            /// <param name="ihi">If the input matrix is an output by <c>?gebal</c> this parameter must contain the value returned by that routine; otherwise <paramref name="n" />.</param>
            /// <returns>The optimal workspace array length.</returns>
            int dgehrdQuery(int n, int ilo, int ihi);

            /// <summary>Reduces a general matrix to upper Hessenberg form H by an orthogonal or unitary similarity transformation A = Q * H * Q', where H has real subdiagonal elements.
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="ilo">If the input matrix is an output by <c>dgebal</c> this parameter must contain the value returned by that routine; otherwise <c>1</c>.</param>
            /// <param name="ihi">If the input matrix is an output by <c>dgebal</c> this parameter must contain the value returned by that routine; otherwise <paramref name="n" />.</param>
            /// <param name="a">The <paramref name="n"/>-by-<paramref name="n"/> matrix A supplied column-by-column; overwritten by the upper Hessenberg matrix H and details of the matrix Q.</param>
            /// <param name="tau">A array of dimension at least max(1, n-1) containing additional informations on the matrix Q (output).</param>
            /// <param name="work">A workspace array.</param>
            void dgehrd(int n, int ilo, int ihi, Span<double> a, Span<double> tau, Span<double> work);

            /// <summary>Gets a optimal workspace array length for the <c>zgehrd</c> function.
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="ilo">If the input matrix is an output by <c>?gebal</c> this parameter must contain the value returned by that routine; otherwise <c>1</c>.</param>
            /// <param name="ihi">If the input matrix is an output by <c>?gebal</c> this parameter must contain the value returned by that routine; otherwise <paramref name="n" />.</param>
            /// <returns>The optimal workspace array length.</returns>
            int zgehrdQuery(int n, int ilo, int ihi);

            /// <summary>Reduces a general matrix to upper Hessenberg form H by an orthogonal or unitary similarity transformation A = Q * H * Q', where H has real subdiagonal elements.
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="ilo">If the input matrix is an output by <c>dgebal</c> this parameter must contain the value returned by that routine; otherwise <c>1</c>.</param>
            /// <param name="ihi">If the input matrix is an output by <c>dgebal</c> this parameter must contain the value returned by that routine; otherwise <paramref name="n" />.</param>
            /// <param name="a">The <paramref name="n"/>-by-<paramref name="n"/> matrix A supplied column-by-column; overwritten by the upper Hessenberg matrix H and details of the matrix Q.</param>
            /// <param name="tau">A array of dimension at least max(1, n-1) containing additional informations on the matrix Q (output).</param>
            /// <param name="work">A workspace array.</param>
            void zgehrd(int n, int ilo, int ihi, Span<Complex> a, Span<Complex> tau, Span<Complex> work);

            /// <summary>Gets a optimal workspace array length for the <c>dorghr</c> function.
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="ilo">If the input matrix is an output by <c>dgebal</c> this parameter must contain the value returned by that routine; otherwise <c>1</c>.</param>
            /// <param name="ihi">If the input matrix is an output by <c>dgebal</c> this parameter must contain the value returned by that routine; otherwise <paramref name="n" />.</param>
            /// <returns>The optimal workspace array length.</returns>
            int dorghrQuery(int n, int ilo, int ihi);

            /// <summary>Generates the real orthogonal matrix Q determined by <c>dgehrd</c>, i.e. explicitly generates the orthogonal matrix Q with A = Q * H * Q'.
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="ilo">If the input matrix is an output by <c>dgebal</c> this parameter must contain the value returned by that routine; otherwise <c>1</c>.</param>
            /// <param name="ihi">If the input matrix is an output by <c>dgebal</c> this parameter must contain the value returned by that routine; otherwise <paramref name="n" />.</param>
            /// <param name="a">Contains on input details of the vectors which define the elementary reflectors, as returned by <c>dgehrd</c>; overwritten by the n-by-n orthogonal matrix Q.</param>
            /// <param name="tau">A array of dimension at least max(1, n-1) containing additional informations on the matrix Q as returned by <c>dgehrd</c>.</param>
            /// <param name="work">A workspace array.</param>
            void dorghr(int n, int ilo, int ihi, Span<double> a, Span<double> tau, Span<double> work);

            /// <summary>Gets a optimal workspace array length for the <c>dormhr</c> function.
            /// </summary>
            /// <param name="side">A value indicating whether to calculate \op(Q) * C [left] or C * \op(Q) [right].</param>
            /// <param name="transposeState">A value indicating whether \op(Q) = Q or \op(Q) = Q'.</param>
            /// <param name="m">The number of rows in matrix C.</param>
            /// <param name="n">The number of columns in matrix C.</param>
            /// <param name="ilo">The same parameter value as supplied to <c>dgehrd</c>.</param>
            /// <param name="ihi">The same parameter value as supplied to <c>dgehrd</c>.</param>
            /// <returns>The optimal workspace array length.</returns>
            int dormhrQuery(LAPACK.Side side, BLAS.MatrixTransposeState transposeState, int m, int n, int ilo, int ihi);

            /// <summary>Multiplies an arbitrary real matrix C by the real orthogonal matrix Q determined by <c>dgehrd</c>, i.e. \op(Q) * C or C * \op(Q), where \op(Q) = Q or \op(Q) = Q'.
            /// </summary>
            /// <param name="side">A value indicating whether to calculate \op(Q) * C [left] or C * \op(Q) [right].</param>
            /// <param name="transposeState">A value indicating whether \op(Q) = Q or \op(Q) = Q'.</param>
            /// <param name="m">The number of rows in matrix C.</param>
            /// <param name="n">The number of columns in matrix C.</param>
            /// <param name="ilo">The same parameter value as supplied to <c>dgehrd</c>.</param>
            /// <param name="ihi">The same parameter value as supplied to <c>dgehrd</c>.</param>
            /// <param name="a">Contains details of the vectors which define the elementary reflectors, as returned by <c>dgehrd</c>.</param>
            /// <param name="tau">Contains further details of the elementary reflectors as returned by <c>dgehrd</c>.</param>
            /// <param name="c">The m-by-n matrix C supplied column-by-column.</param>
            /// <param name="work">A workspace array.</param>
            void dormhr(LAPACK.Side side, BLAS.MatrixTransposeState transposeState, int m, int n, int ilo, int ihi, Span<double> a, Span<double> tau, Span<double> c, Span<double> work);

            /// <summary>Gets a optimal workspace array length for the <c>zunghr</c> function.
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="ilo">If the input matrix is an output by <c>zgehrd</c> this parameter must contain the value returned by that routine; otherwise <c>1</c>.</param>
            /// <param name="ihi">If the input matrix is an output by <c>zgehrd</c> this parameter must contain the value returned by that routine; otherwise <paramref name="n" />.</param>
            /// <returns>The optimal workspace array length.</returns>
            int zunghrQuery(int n, int ilo, int ihi);

            /// <summary>Generates the complex unitary matrix Q determined by <c>zgehrd</c>.
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="ilo">If the input matrix is an output by <c>zgehrd</c> this parameter must contain the value returned by that routine; otherwise <c>1</c>.</param>
            /// <param name="ihi">If the input matrix is an output by <c>zgehrd</c> this parameter must contain the value returned by that routine; otherwise <paramref name="n" />.</param>
            /// <param name="a">Contains on input details of the vectors which define the elementary reflectors, as returned by <c>zgehrd</c>; overwritten by the n-by-n unitary matrix Q.</param>
            /// <param name="tau">A array of dimension at least max(1, n-1) containing additional informations on the matrix Q as returned by <c>zgehrd</c>.</param>
            /// <param name="work">A workspace array.</param>
            void zunghr(int n, int ilo, int ihi, Span<Complex> a, Span<Complex> tau, Span<Complex> work);

            /// <summary>Gets a optimal workspace array length for the <c>zunmhr</c> function.
            /// </summary>
            /// <param name="side">A value indicating whether to calculate \op(Q) * C [left] or C * \op(Q) [right].</param>
            /// <param name="transposeState">A value indicating whether \op(Q) = Q or \op(Q) = Q'.</param>
            /// <param name="m">The number of rows in matrix C.</param>
            /// <param name="n">The number of columns in matrix C.</param>
            /// <param name="ilo">The same parameter value as supplied to <c>dgehrd</c>.</param>
            /// <param name="ihi">The same parameter value as supplied to <c>dgehrd</c>.</param>
            /// <returns>The optimal workspace array length.</returns>
            int zunmhrQuery(LAPACK.Side side, BLAS.MatrixTransposeState transposeState, int m, int n, int ilo, int ihi);

            /// <summary>Multiplies an arbitrary complex matrix C by the complex unitary matrix Q determined by <c>zgehrd</c>, i.e. \op(Q) * C or C * \op(Q), where \op(Q) = Q or \op(Q) = Q'.
            /// </summary>
            /// <param name="side">A value indicating whether to calculate \op(Q); * C [left] or C * \op(Q); [right].</param>
            /// <param name="transposeState">A value indicating whether \op(Q) = Q or \op(Q) = Q'.</param>
            /// <param name="m">The number of rows in matrix C.</param>
            /// <param name="n">The number of columns in matrix C.</param>
            /// <param name="ilo">The same parameter value as supplied to <c>zgehrd</c>.</param>
            /// <param name="ihi">The same parameter value as supplied to <c>zgehrd</c>.</param>
            /// <param name="a">Contains details of the vectors which define the elementary reflectors, as returned by <c>zgehrd</c>.</param>
            /// <param name="tau">Contains further details of the elementary reflectors as returned by <c>zgehrd</c>.</param>
            /// <param name="c">The m-by-n matrix C supplied column-by-column.</param>
            /// <param name="work">A workspace array.</param>
            void zunmhr(LAPACK.Side side, BLAS.MatrixTransposeState transposeState, int m, int n, int ilo, int ihi, Span<Complex> a, Span<Complex> tau, Span<Complex> c, Span<Complex> work);

            /// <summary>Balances a general matrix to improve the accuracy of computed eigenvalues and eigenvectors.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="a">The matrix A supplied column-by-column of dimension; overwritten by the balanced matrix.</param>
            /// <param name="ilo">A null-based index such that a[i,j] is zero for i &gt; j and 1 &lt; j &lt;= <paramref name="ilo" /> or <paramref name="ihi" /> &lt; i &lt;= n.</param>
            /// <param name="ihi">A null-based index such that a[i,j] is zero for i &gt; j and 1 &lt; j &lt;= <paramref name="ilo" /> or <paramref name="ihi" /> &lt; i &lt;= n.</param>
            /// <param name="scale">Contains details of the permutations and scaling factors; at least <paramref name="n" /> elements (output).</param>
            void dgebal(LapackEigenvalues.NonSymmetricMatrixBalancesType job, int n, Span<double> a, out int ilo, out int ihi, Span<double> scale);

            /// <summary>Balances a general matrix to improve the accuracy of computed eigenvalues and eigenvectors.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="a">The matrix A supplied column-by-column of dimension; overwritten by the balanced matrix.</param>
            /// <param name="ilo">A null-based index such that a[i,j] is zero for i &gt; j and 1 &lt; j &lt;= <paramref name="ilo" /> or <paramref name="ihi" /> &lt; i &lt;= n.</param>
            /// <param name="ihi">A null-based index such that a[i,j] is zero for i &gt; j and 1 &lt; j &lt;= <paramref name="ilo" /> or <paramref name="ihi" /> &lt; i &lt;= n.</param>
            /// <param name="scale">Contains details of the permutations and scaling factors; at least <paramref name="n" /> elements (output).</param>
            void zgebal(LapackEigenvalues.NonSymmetricMatrixBalancesType job, int n, Span<Complex> a, out int ilo, out int ihi, Span<double> scale);

            /// <summary>Transforms eigenvectors of a balanced matrix to those of the original nonsymmetric matrix. Is intended to be used after a matrix has been balanced by a call to <c>dgebal</c>.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="side">A value indicating whether to transform left eigenvectors, right eigenvectors respectively.</param>
            /// <param name="n">The number of rows of the matrix of eigenvectors.</param>
            /// <param name="ilo">The returned value of <c>dgebal</c>.</param>
            /// <param name="ihi">The returned value of <c>dgebal</c>.</param>
            /// <param name="scale">The returned value of <c>dgebal</c>.</param>
            /// <param name="m">The number of columns of the matrix of eigenvectors.</param>
            /// <param name="v">The matrix of left or right eigenvectors to be transformed; overwritten by the transformed eigenvectors</param>
            void dgebak(LapackEigenvalues.NonSymmetricMatrixBalancesType job, LAPACK.Side side, int n, int ilo, int ihi, Span<double> scale, int m, Span<double> v);

            /// <summary>Transforms eigenvectors of a balanced matrix to those of the original nonsymmetric matrix. Is intended to be used after a matrix has been balanced by a call to <c>zgebal</c>.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="side">A value indicating whether to transform left eigenvectors, right eigenvectors respectively.</param>
            /// <param name="n">The number of rows of the matrix of eigenvectors.</param>
            /// <param name="ilo">The returned value of <c>zgebal</c>.</param>
            /// <param name="ihi">The returned value of <c>zgebal</c>.</param>
            /// <param name="scale">The returned value of <c>zgebal</c>.</param>
            /// <param name="m">The number of columns of the matrix of eigenvectors.</param>
            /// <param name="v">The matrix of left or right eigenvectors to be transformed; overwritten by the transformed eigenvectors</param>
            void zgebak(LapackEigenvalues.NonSymmetricMatrixBalancesType job, LAPACK.Side side, int n, int ilo, int ihi, Span<double> scale, int m, Span<Complex> v);

            /// <summary>Gets a optimal workspace array length for the <c>dhseqr</c> function.
            /// </summary>
            /// <param name="job">A value indicating whether eigenvalues or Schur form is required.</param>
            /// <param name="operation">A value indicating</param>
            /// <param name="n">The order of the Hessenberg matrix H.</param>
            /// <param name="ilo">If the input matrix is an output by <c>dgebal</c> this parameter must contain the value returned by that routine; otherwise <c>1</c>.</param>
            /// <param name="ihi">If the input matrix is an output by <c>dgebal</c> this parameter must contain the value returned by that routine; otherwise <paramref name="n" />.</param>
            /// <returns>The optimal workspace array length.</returns>
            int dhseqrQuery(LapackEigenvalues.NonSymmetricXhseqrJob job, LapackEigenvalues.NonSymmetricXhseqrOperation operation, int n, int ilo, int ihi);

            /// <summary>Computes all eigenvalues and (optionally); the Schur factorization of a matrix reduced to Hessenberg form, i.e. A = Q * H * Q' where Q is orthogonal and H = Z * T * Z'.
            /// </summary>
            /// <param name="job">A value indicating whether eigenvalues or Schur form is required.</param>
            /// <param name="operation">A value indicating which matrix should be applied by the Schur vector calculation.</param>
            /// <param name="n">The order of the Hessenberg matrix H.</param>
            /// <param name="ilo">If the input matrix is an output by <c>dgebal</c> this parameter must contain the value returned by that routine; otherwise <c>1</c>.</param>
            /// <param name="ihi">If the input matrix is an output by <c>dgebal</c> this parameter must contain the value returned by that routine; otherwise <paramref name="n" />.</param>
            /// <param name="h">On input the Hessenberg matrix H; overwritten by the upper triangular matrix T from the Schur decomposition if <paramref name="job" /> indicates
            /// to calculate the Schur decomposition, otherwise the content is unspecified on exit.</param>
            /// <param name="wr">The real part of the eigenvalues, i.e. an array with at least <paramref name="n" /> elements (output).</param>
            /// <param name="wi">The imaginary part of the eigenvalues, i.e. an array with at least <paramref name="n" /> elements (output).</param>
            /// <param name="z">Contains the matrix Q reduced to Hessenberg form; perhaps this parameter will not referenced.</param>
            /// <param name="work">A workspace array.</param>
            void dhseqr(LapackEigenvalues.NonSymmetricXhseqrJob job, LapackEigenvalues.NonSymmetricXhseqrOperation operation, int n, int ilo, int ihi, Span<double> h, Span<double> wr, Span<double> wi, Span<double> z, Span<double> work);

            /// <summary>Gets a optimal workspace array length for the <c>zhseqr</c> function.
            /// </summary>
            /// <param name="job">A value indicating whether eigenvalues or Schur form is required.</param>
            /// <param name="operation">A value indicating</param>
            /// <param name="n">The order of the Hessenberg matrix H.</param>
            /// <param name="ilo">If the input matrix is an output by <c>zgebal</c> this parameter must contain the value returned by that routine; otherwise <c>1</c>.</param>
            /// <param name="ihi">If the input matrix is an output by <c>zgebal</c> this parameter must contain the value returned by that routine; otherwise <paramref name="n" />.</param>
            /// <returns>The optimal workspace array length.</returns>
            int zhseqrQuery(LapackEigenvalues.NonSymmetricXhseqrJob job, LapackEigenvalues.NonSymmetricXhseqrOperation operation, int n, int ilo, int ihi);

            /// <summary>Computes all eigenvalues and (optionally) the Schur factorization of a matrix reduced to Hessenberg form, i.e. A = Q * H * Q' where Q is orthogonal and H = Z * T * Z'.
            /// </summary>
            /// <param name="job">A value indicating whether eigenvalues or Schur form is required.</param>
            /// <param name="operation">A value indicating which matrix should be applied by the Schur vector calculation.</param>
            /// <param name="n">The order of the Hessenberg matrix H.</param>
            /// <param name="ilo">If the input matrix is an output by <c>zgebal</c> this parameter must contain the value returned by that routine; otherwise <c>1</c>.</param>
            /// <param name="ihi">If the input matrix is an output by <c>zgebal</c> this parameter must contain the value returned by that routine; otherwise <paramref name="n" />.</param>
            /// <param name="h">On input the Hessenberg matrix H; overwritten by the upper triangular matrix T from the Schur decomposition if <paramref name="job" /> indicates
            /// to calculate the Schur decomposition, otherwise the content is unspecified on exit.</param>
            /// <param name="w">The eigenvalues, i.e. an array with at least <paramref name="n" /> elements (output).</param>
            /// <param name="z">Contains the matrix Q reduced to Hessenberg form; perhaps this parameter will not referenced.</param>
            /// <param name="work">A workspace array.</param>
            void zhseqr(LapackEigenvalues.NonSymmetricXhseqrJob job, LapackEigenvalues.NonSymmetricXhseqrOperation operation, int n, int ilo, int ihi, Span<Complex> h, Span<Complex> w, Span<Complex> z, Span<Complex> work);

            /// <summary>Computes selected eigenvectors of an upper Hessenberg matrix that correspond to specified eigenvalues.
            /// </summary>
            /// <param name="job">A value indicating whether eigenvalues or Schur form is required.</param>
            /// <param name="eigenvalueSource">A value indicating whether the eigenvalues of H were found using <c>dhseqr</c>.</param>
            /// <param name="initv">A value indicating whether initial estimates are supplied.</param>
            /// <param name="select">Specifies which eigenvectors are to be computed.</param>
            /// <param name="n">The order of the matrix H.</param>
            /// <param name="h"></param>
            /// <param name="wr">Contains the real parts of the eigenvalues of the matrix H.</param>
            /// <param name="wi">Contains the imaginary parts of the eigenvalues of the matrix H.</param>
            /// <param name="vl">Contains computed left eigenvectors if specified by <paramref name="select"/> (output).</param>
            /// <param name="vr">Contains computed right eigenvectors if specified by <paramref name="select"/> (output).</param>
            /// <param name="mm">The number of columns in <paramref name="vl"/> and/or <paramref name="vr"/>.</param>
            /// <param name="m">The number of columns of <paramref name="vl"/> and/or <paramref name="vr"/> required to store the selected eigenvectors (output).</param>
            /// <param name="work">A workspace array with dimension at least <paramref name="n"/> * (2 + <paramref name="n"/>).</param>
            /// <param name="ifaill">Indicates whether the calculation of a specific eigenvector fails; dimension at least <paramref name="mm"/>.</param>
            /// <param name="ifailr">Indicates whether the calculation of a specific eigenvector fails; dimension at least <paramref name="mm"/>.</param>
            void dhsein(NonSymmetricEigenValueVectorsJob job, NonSymmetricEigenvalueSource eigenvalueSource, NonSymmetricXhseinInitV initv, bool[] select, int n, Span<double> h, Span<double> wr, Span<double> wi, Span<double> vl, Span<double> vr, int mm, out int m, Span<double> work, int[] ifaill, int[] ifailr);

            /// <summary>Computes selected eigenvectors of an upper Hessenberg matrix that correspond to specified eigenvalues.
            /// </summary>
            /// <param name="job">A value indicating whether eigenvalues or Schur form is required.</param>
            /// <param name="eigenvalueSource">A value indicating whether the eigenvalues of H were found using <c>zhseqr</c>.</param>
            /// <param name="initv">A value indicating whether initial estimates are supplied.</param>
            /// <param name="select">Specifies which eigenvectors are to be computed.</param>
            /// <param name="n">The order of the matrix H.</param>
            /// <param name="h"></param>
            /// <param name="w">Contains the eigenvalues of the matrix H.</param>
            /// <param name="vl">Contains computed left eigenvectors if specified by <paramref name="select"/> (output).</param>
            /// <param name="vr">Contains computed right eigenvectors if specified by <paramref name="select"/> (output).</param>
            /// <param name="mm">The number of columns in <paramref name="vl"/> and/or <paramref name="vr"/>.</param>
            /// <param name="m">The number of selected eigenvectors (output).</param>
            /// <param name="work">A workspace array with dimension at least <paramref name="n"/> * <paramref name="n"/>.</param>
            /// <param name="rwork">A workspace array with dimension at least <paramref name="n"/>.</param>
            /// <param name="ifaill">Indicates whether the calculation of a specific eigenvector fails; dimension at least <paramref name="mm"/>.</param>
            /// <param name="ifailr">Indicates whether the calculation of a specific eigenvector fails; dimension at least <paramref name="mm"/>.</param>
            void zhsein(NonSymmetricEigenValueVectorsJob job, NonSymmetricEigenvalueSource eigenvalueSource, NonSymmetricXhseinInitV initv, bool[] select, int n, Span<Complex> h, Span<Complex> w, Span<Complex> vl, Span<Complex> vr, int mm, out int m, Span<Complex> work, Span<double> rwork, int[] ifaill, int[] ifailr);

            /// <summary>Computes selected eigenvectors of an upper (quasi-) triangular matrix computed by <c>dhseqr</c>, i.e. A = Q * T * Q^H.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="howmny">A value indicating whether to take into acccount a subset of eigenvectors only.</param>
            /// <param name="select">Specifies which eigenvectors are to be computed, if specified by <paramref name="howmny"/>; overwritten on exit.</param>
            /// <param name="n">The order of the matrix T.</param>
            /// <param name="t">The <paramref name="n"/>-by-<paramref name="n"/> matrix T in Schur canonical form.</param>
            /// <param name="vl">Contain an n-by-n matrix Q (usually the matrix of Schur vectors returned by dhseqr), if specified by <paramref name="job"/>; overwritten by the computed left eigenvectors.</param>
            /// <param name="vr">Contain an n-by-n matrix Q (usually the matrix of Schur vectors returned by dhseqr), if specified by <paramref name="job"/>; overwritten by the computed right eigenvectors.</param>
            /// <param name="mm">The number of columns in <paramref name="vl"/> and/or <paramref name="vr"/>. Must be at least <paramref name="m"/>.</param>
            /// <param name="m">The number of columsn of <paramref name="vl"/> and/or <paramref name="vr"/> actually used to store the selected eigenvectors (output).</param>
            /// <param name="work">A workspace array with dimension at least 3 * <paramref name="n"/>.</param>
            void dtrevc(LapackEigenvalues.NonSymmetricEigenValueVectorsJob job, LapackEigenvalues.NonSymmetricXtrevcOperation howmny, bool[] select, int n, Span<double> t, Span<double> vl, Span<double> vr, int mm, out int m, Span<double> work);

            /// <summary>Computes selected eigenvectors of an upper (quasi-) triangular matrix computed by <c>zhseqr</c>, i.e. A = Q * T * Q^H.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="howmny">A value indicating whether to take into acccount a subset of eigenvectors only.</param>
            /// <param name="select">Specifies which eigenvectors are to be computed, if specified by <paramref name="howmny"/>; overwritten on exit.</param>
            /// <param name="n">The order of the matrix T.</param>
            /// <param name="t">The <paramref name="n"/>-by-<paramref name="n"/> matrix T in Schur canonical form.</param>
            /// <param name="vl">Contain an n-by-n matrix Q (usually the matrix of Schur vectors returned by zhseqr), if specified by <paramref name="job"/>; overwritten by the computed left eigenvectors.</param>
            /// <param name="vr">Contain an n-by-n matrix Q (usually the matrix of Schur vectors returned by zhseqr), if specified by <paramref name="job"/>; overwritten by the computed right eigenvectors.</param>
            /// <param name="mm">The number of columns in <paramref name="vl"/> and/or <paramref name="vr"/>. Must be at least <paramref name="m"/>.</param>
            /// <param name="m">The number of columsn of <paramref name="vl"/> and/or <paramref name="vr"/> actually used to store the selected eigenvectors (output).</param>
            /// <param name="work">A workspace array with dimension at least 2 * <paramref name="n"/>.</param>
            /// <param name="rwork">A workspace array with dimension at least <paramref name="n"/>.</param>
            void ztrevc(LapackEigenvalues.NonSymmetricEigenValueVectorsJob job, LapackEigenvalues.NonSymmetricXtrevcOperation howmny, bool[] select, int n, Span<Complex> t, Span<Complex> vl, Span<Complex> vr, int mm, out int m, Span<Complex> work, Span<double> rwork);

            /// <summary>Reorders the Schur factorization of a general matrix.
            /// </summary>
            /// <param name="updatedSchurVectors">A value indicating whether the Schur vectors are updated.</param>
            /// <param name="n">The order of the matrix T.</param>
            /// <param name="t">The <paramref name="n"/>-by-<paramref name="n"/> matrix T.</param>
            /// <param name="q">The matrix Q; will not referenced if <paramref name="updatedSchurVectors"/> is <c>false</c>.</param>
            /// <param name="ifst">Specify the reordering of the diagonal elements of matrix T. The element with row index <c>ifst</c> is moved to row <c>ilst</c> by a sequence of exchanges between adjacent elements.</param>
            /// <param name="ilst">Specify the reordering of the diagonal elements of matrix T. The element with row index <c>ifst</c> is moved to row <c>ilst</c> by a sequence of exchanges between adjacent elements.</param>
            /// <param name="work">A workspace array with dimension at least <paramref name="n"/>.</param>
            void dtrexc(bool updatedSchurVectors, int n, Span<double> t, Span<double> q, int ifst, int ilst, Span<double> work);

            /// <summary>Reorders the Schur factorization of a general matrix.
            /// </summary>
            /// <param name="updatedSchurVectors">A value indicating whether the Schur vectors are updated.</param>
            /// <param name="n">The order of the matrix T.</param>
            /// <param name="t">The <paramref name="n"/>-by-<paramref name="n"/> matrix T.</param>
            /// <param name="q">The matrix Q; will not referenced if <paramref name="updatedSchurVectors"/> is <c>false</c>.</param>
            /// <param name="ifst">Specify the reordering of the diagonal elements of matrix T. The element with row index <c>ifst</c> is moved to row <c>ilst</c> by a sequence of exchanges between adjacent elements.</param>
            /// <param name="ilst">Specify the reordering of the diagonal elements of matrix T. The element with row index <c>ifst</c> is moved to row <c>ilst</c> by a sequence of exchanges between adjacent elements.</param>
            void ztrexc(bool updatedSchurVectors, int n, Span<Complex> t, Span<Complex> q, int ifst, int ilst);

            /// <summary>Solves Sylvester equation for real quasi-triangular or complex triangular matrices.
            /// </summary>
            /// <param name="transposeStateA">A value indicating whether \op(A) = A or \op(A) = A'.</param>
            /// <param name="transposeStateB">A value indicating whether \op(B) = B or \op(B) = B'.</param>
            /// <param name="sign">Indicates the form of the Sylvester equation.</param>
            /// <param name="m">The order of matrix A, and the number of rows in X and C.</param>
            /// <param name="n">The order of matrix B, and the number of columns in X and C.</param>
            /// <param name="a">The matrix A.</param>
            /// <param name="b">The matrix B.</param>
            /// <param name="c">The matrix C.</param>
            /// <param name="scale">The scale factor (output).</param>
            void dtrsyl(BLAS.MatrixTransposeState transposeStateA, BLAS.MatrixTransposeState transposeStateB, int sign, int m, int n, Span<double> a, Span<double> b, Span<double> c, out double scale);

            /// <summary>Solves Sylvester equation for real quasi-triangular or complex triangular matrices.
            /// </summary>
            /// <param name="transposeStateA">A value indicating whether \op(A) = A, A' or A^H.</param>
            /// <param name="transposeStateB">A value indicating whether \op(B) = B, B' or B^H.</param>
            /// <param name="sign">Indicates the form of the Sylvester equation.</param>
            /// <param name="m">The order of matrix A, and the number of rows in X and C.</param>
            /// <param name="n">The order of matrix B, and the number of columns in X and C.</param>
            /// <param name="a">The matrix A.</param>
            /// <param name="b">The matrix B.</param>
            /// <param name="c">The matrix C.</param>
            /// <param name="scale">The scale factor (output).</param>
            void ztrsyl(BLAS.MatrixTransposeState transposeStateA, BLAS.MatrixTransposeState transposeStateB, int sign, int m, int n, Span<Complex> a, Span<Complex> b, Span<Complex> c, out double scale);

            /// <summary>Gets a optimal workspace array length for the <c>driver_dgees</c> function.
            /// </summary>
            /// <param name="job">A value indicating whether eigenvalues or Schur form is required.</param>
            /// <param name="sort">Specifies whether or not to order the eigenvalues on the diagonal of the Schur form.</param>        
            /// <param name="n">The order of the matrix.</param>
            /// <returns>The optimal workspace array length.</returns>
            int driver_dgeesQuery(NonSymmetricSchurVectorsJob job, NonSymmetricEigenvalueOrdering sort, int n);

            /// <summary>Computes the eigenvalues and Schur factorization of a general matrix, and orders the factorization so that selected eigenvalues are at the top left of the Schur form.
            /// </summary>
            /// <param name="job">A value indicating whether eigenvalues or Schur form is required.</param>
            /// <param name="sort">Specifies whether or not to order the eigenvalues on the diagonal of the Schur form.</param>        
            /// <param name="select">A function that is used to select eigenvalues to sort to the top left of the Schur form.</param>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="a">Contains the <paramref name="n"/>-by-<paramref name="n"/> matrix A; overwritten by the real-Schur form T.</param>
            /// <param name="sdim">The number of eigenvalues (after sorting) for which <paramref name="select"/> is <c>true</c>; otherwise <c>0</c> (output).</param>
            /// <param name="wr">Contains the real parts of the eigenvalues of the matrix H (output).</param>
            /// <param name="wi">Contains the imaginary parts of the eigenvalues of the matrix H (output).</param>
            /// <param name="vs">Contains the ortogonal matrix Z of Schur vectors as specified by <paramref name="job"/> (output).</param>
            /// <param name="work">A workspace array.</param>
            /// <param name="bwork">A workspace array of length at least <paramref name="n"/>.</param>
            void driver_dgees(NonSymmetricSchurVectorsJob job, NonSymmetricEigenvalueOrdering sort, Func<double, double, bool> select, int n, Span<double> a, out int sdim, Span<double> wr, Span<double> wi, Span<double> vs, Span<double> work, bool[] bwork);

            /// <summary>Gets a optimal workspace array length for the <c>driver_zgees</c> function.
            /// </summary>
            /// <param name="job">A value indicating whether eigenvalues or Schur form is required.</param>
            /// <param name="sort">Specifies whether or not to order the eigenvalues on the diagonal of the Schur form.</param>        
            /// <param name="n">The order of the matrix.</param>
            /// <returns>The optimal workspace array length.</returns>
            int driver_zgeesQuery(NonSymmetricSchurVectorsJob job, NonSymmetricEigenvalueOrdering sort, int n);

            /// <summary>Computes the eigenvalues and Schur factorization of a general matrix, and orders the factorization so that selected eigenvalues are at the top left of the Schur form.
            /// </summary>
            /// <param name="job">A value indicating whether eigenvalues or Schur form is required.</param>
            /// <param name="sort">Specifies whether or not to order the eigenvalues on the diagonal of the Schur form.</param>        
            /// <param name="select">A function that is used to select eigenvalues to sort to the top left of the Schur form.</param>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="a">Contains the <paramref name="n"/>-by-<paramref name="n"/> matrix A; overwritten by the real-Schur form T.</param>
            /// <param name="sdim">The number of eigenvalues (after sorting) for which <paramref name="select"/> is <c>true</c>; otherwise <c>0</c> (output).</param>
            /// <param name="w">Contains the eigenvalues of the matrix H (output).</param>
            /// <param name="vs">Contains the ortogonal matrix Z of Schur vectors as specified by <paramref name="job"/> (output).</param>
            /// <param name="work">A workspace array.</param>
            /// <param name="rwork">A workspace array of length at least <paramref name="n"/>.</param>
            /// <param name="bwork">A workspace array of length at least <paramref name="n"/>.</param>
            void driver_zgees(NonSymmetricSchurVectorsJob job, NonSymmetricEigenvalueOrdering sort, Func<Complex, bool> select, int n, Span<Complex> a, out int sdim, Span<Complex> w, Span<Complex> vs, Span<Complex> work, Span<double> rwork, bool[] bwork);

            /// <summary>Gets a optimal workspace array length for the <c>driver_dgeesx</c> function.
            /// </summary>
            /// <param name="job">A value indicating whether eigenvalues or Schur form is required.</param>
            /// <param name="sort">Specifies whether or not to order the eigenvalues on the diagonal of the Schur form.</param>        
            /// <param name="n">The order of the matrix.</param>
            /// <param name="sense">Determines which reciprocal condition number are computed.</param>
            /// <returns>The optimal workspace array length.</returns>
            int driver_dgeesxQuery(NonSymmetricSchurVectorsJob job, NonSymmetricEigenvalueOrdering sort, NonSymmetricSense sense, int n);

            /// <summary>Computes the eigenvalues and Schur factorization of a general matrix, orders the factorization and computes reciprocal condition numbers.
            /// </summary>
            /// <param name="job">A value indicating whether eigenvalues or Schur form is required.</param>
            /// <param name="sort">Specifies whether or not to order the eigenvalues on the diagonal of the Schur form.</param>        
            /// <param name="select">A function that is used to select eigenvalues to sort to the top left of the Schur form.</param>
            /// <param name="sense">Determines which reciprocal condition number are computed.</param>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="a">Contains the <paramref name="n"/>-by-<paramref name="n"/> matrix A; overwritten by the real-Schur form T.</param>
            /// <param name="sdim">The number of eigenvalues (after sorting); for which <paramref name="select"/> is <c>true</c>; otherwise <c>0</c> (output).</param>
            /// <param name="wr">Contains the real parts of the eigenvalues of the matrix H (output).</param>
            /// <param name="wi">Contains the imaginary parts of the eigenvalues of the matrix H (output).</param>
            /// <param name="vs">Contains the ortogonal matrix Z of Schur vectors as specified by <paramref name="job"/> (output).</param>
            /// <param name="rconde">The reciprocal condition number for the average of the selected eigenvalues as specified by <paramref name="sense"/> (output).</param>
            /// <param name="rcondv">The reciprocal condition number for th selected right invariant subspace as specified by <paramref name="sense"/> (output).</param>
            /// <param name="work">A workspace array.</param>
            /// <param name="iwork">A workspace array with dimension at least <paramref name="sdim"/> * ( <paramref name="n"/> - <paramref name="sdim"/>).</param>
            /// <param name="bwork">A workspace array of length at least <paramref name="n"/>.</param>
            void driver_dgeesx(NonSymmetricSchurVectorsJob job, NonSymmetricEigenvalueOrdering sort, Func<double, double, bool> select, NonSymmetricSense sense, int n, Span<double> a, out int sdim, Span<double> wr, Span<double> wi, Span<double> vs, out double rconde, out double rcondv, Span<double> work, int[] iwork, bool[] bwork);

            /// <summary>Gets a optimal workspace array length for the <c>driver_zgeesx</c> function.
            /// </summary>
            /// <param name="job">A value indicating whether eigenvalues or Schur form is required.</param>
            /// <param name="sort">Specifies whether or not to order the eigenvalues on the diagonal of the Schur form.</param>        
            /// <param name="n">The order of the matrix.</param>
            /// <param name="sense">Determines which reciprocal condition number are computed.</param>
            /// <returns>The optimal workspace array length.</returns>
            int driver_zgeesxQuery(NonSymmetricSchurVectorsJob job, NonSymmetricEigenvalueOrdering sort, NonSymmetricSense sense, int n);

            /// <summary>Computes the eigenvalues and Schur factorization of a general matrix, orders the factorization and computes reciprocal condition numbers.
            /// </summary>
            /// <param name="job">A value indicating whether eigenvalues or Schur form is required.</param>
            /// <param name="sort">Specifies whether or not to order the eigenvalues on the diagonal of the Schur form.</param>        
            /// <param name="select">A function that is used to select eigenvalues to sort to the top left of the Schur form.</param>
            /// <param name="sense">Determines which reciprocal condition number are computed.</param>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="a">Contains the <paramref name="n"/>-by-<paramref name="n"/> matrix A; overwritten by the real-Schur form T.</param>
            /// <param name="sdim">The number of eigenvalues (after sorting) for which <paramref name="select"/> is <c>true</c>; otherwise <c>0</c> (output).</param>
            /// <param name="w">Contains the eigenvalues of the matrix H (output).</param>
            /// <param name="vs">Contains the unitary matrix Z of Schur vectors as specified by <paramref name="job"/> (output).</param>
            /// <param name="rconde">The reciprocal condition number for the average of the selected eigenvalues as specified by <paramref name="sense"/> (output).</param>
            /// <param name="rcondv">The reciprocal condition number for th selected right invariant subspace as specified by <paramref name="sense"/> (output).</param>
            /// <param name="work">A workspace array.</param>
            /// <param name="rwork">A workspace array with dimension at least <paramref name="n"/>.</param>
            /// <param name="bwork">A workspace array of length at least <paramref name="n"/>.</param>
            void driver_zgeesx(NonSymmetricSchurVectorsJob job, NonSymmetricEigenvalueOrdering sort, Func<Complex, bool> select, NonSymmetricSense sense, int n, Span<Complex> a, out int sdim, Span<Complex> w, Span<Complex> vs, out double rconde, out double rcondv, Span<Complex> work, Span<double> rwork, bool[] bwork);

            /// <summary>Gets a optimal workspace array length for the <c>driver_dgeev</c> function.
            /// </summary>
            /// <param name="computeLeftEigenvectors">A value indicating whether to calculate left eigenvectors.</param>
            /// <param name="computeRightEigenvectors">A value indicating whether to calculate right eigenvectors.</param>
            /// <param name="n">The order of the matrix.</param>
            /// <returns>The optimal workspace array length.</returns>
            int driver_dgeevQuery(bool computeLeftEigenvectors, bool computeRightEigenvectors, int n);

            /// <summary>Computes the eigenvalues and left and right eigenvectors of a general matrix.
            /// </summary>
            /// <param name="computeLeftEigenvectors">A value indicating whether to calculate left eigenvectors.</param>
            /// <param name="computeRightEigenvectors">A value indicating whether to calculate right eigenvectors.</param>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="a">Contains the <paramref name="n"/>-by-<paramref name="n"/> matrix A; overwritten by intermediate results.</param>
            /// <param name="wr">Contains the real parts of the eigenvalues (output).</param>
            /// <param name="wi">Contains the imaginary parts of the eigenvalues (output).</param>
            /// <param name="vl">Contains computed left eigenvectors if specified by <paramref name="computeLeftEigenvectors"/> (output).</param>
            /// <param name="vr">Contains computed right eigenvectors if specified by <paramref name="computeRightEigenvectors"/> (output).</param>
            /// <param name="work">A workspace array.</param>
            void driver_dgeev(bool computeLeftEigenvectors, bool computeRightEigenvectors, int n, Span<double> a, Span<double> wr, Span<double> wi, Span<double> vl, Span<double> vr, Span<double> work);

            /// <summary>Gets a optimal workspace array length for the <c>driver_zgeev</c> function.
            /// </summary>
            /// <param name="computeLeftEigenvectors">A value indicating whether to calculate left eigenvectors.</param>
            /// <param name="computeRightEigenvectors">A value indicating whether to calculate right eigenvectors.</param>
            /// <param name="n">The order of the matrix.</param>
            /// <returns>The optimal workspace array length.</returns>
            int driver_zgeevQuery(bool computeLeftEigenvectors, bool computeRightEigenvectors, int n);

            /// <summary>Computes the eigenvalues and left and right eigenvectors of a general matrix.
            /// </summary>
            /// <param name="computeLeftEigenvectors">A value indicating whether to calculate left eigenvectors.</param>
            /// <param name="computeRightEigenvectors">A value indicating whether to calculate right eigenvectors.</param>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="a">Contains the <paramref name="n"/>-by-<paramref name="n"/> matrix A; overwritten by intermediate results.</param>
            /// <param name="w">Contains the eigenvalues (output).</param>
            /// <param name="vl">Contains computed left eigenvectors if specified by <paramref name="computeLeftEigenvectors"/> (output).</param>
            /// <param name="vr">Contains computed right eigenvectors if specified by <paramref name="computeRightEigenvectors"/> (output).</param>
            /// <param name="work">A workspace array.</param>
            /// <param name="rwork">A workspace array length at least 2 * <paramref name="n"/>.</param>
            void driver_zgeev(bool computeLeftEigenvectors, bool computeRightEigenvectors, int n, Span<Complex> a, Span<Complex> w, Span<Complex> vl, Span<Complex> vr, Span<Complex> work, Span<double> rwork);

            /// <summary>Gets a optimal workspace array length for the <c>driver_dgeevx</c> function.
            /// </summary>
            /// <param name="balanceType">Indicates how the input matrix should be diagonally scaled and/or permuted to improve the conditioning of its eigenvalues.</param>
            /// <param name="computeLeftEigenvectors">A value indicating whether to calculate left eigenvectors.</param>
            /// <param name="computeRightEigenvectors">A value indicating whether to calculate right eigenvectors.</param>
            /// <param name="sense">Determines which reciprocal condition number are computed.</param>
            /// <param name="n">The order of the matrix.</param>
            /// <returns>The optimal workspace array length.</returns>
            int driver_dgeevxQuery(LapackEigenvalues.NonSymmetricMatrixBalancesType balanceType, bool computeLeftEigenvectors, bool computeRightEigenvectors, NonSymmetricXgeevxSense sense, int n);

            /// <summary>Computes the eigenvalues and left and right eigenvectors of a general matrix, with preliminary matrix balancing, and computes reciprocal condition numbers for the eigenvalues and right eigenvectors.
            /// </summary>
            /// <param name="balanceType">Indicates how the input matrix should be diagonally scaled and/or permuted to improve the conditioning of its eigenvalues.</param>
            /// <param name="computeLeftEigenvectors">A value indicating whether to calculate left eigenvectors.</param>
            /// <param name="computeRightEigenvectors">A value indicating whether to calculate right eigenvectors.</param>
            /// <param name="sense">Determines which reciprocal condition number are computed.</param>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="a">Contains the <paramref name="n"/>-by-<paramref name="n"/> matrix A; overwritten by intermediate results.</param>
            /// <param name="wr">Contains the real parts of the eigenvalues (output).</param>
            /// <param name="wi">Contains the imaginary parts of the eigenvalues (output).</param>
            /// <param name="vl">Contains computed left eigenvectors if specified by <paramref name="computeLeftEigenvectors"/> (output).</param>
            /// <param name="vr">Contains computed right eigenvectors if specified by <paramref name="computeRightEigenvectors"/> (output).</param>
            /// <param name="ilo">Determined when A was balanced (output).</param>
            /// <param name="ihi">Determined when A was balanced (output).</param>
            /// <param name="scale">Details of the permutations and scaling factors applied when balancing A (output).</param>
            /// <param name="abnrm">The one-norm of the balanced matrix (the maximum of the sum of absolute values of elements of any column); (output).</param>
            /// <param name="rconde">For each eigenvalue the corresponding reciprocal condition number (output).</param>
            /// <param name="rcondv">For each right eigenvector the corresponding reciprocal condition number (output).</param>
            /// <param name="work">A workspace array.</param>
            /// <param name="iwork">A workspace array with dimension at least 2 * <paramref name="n"/> - 2.</param>
            void driver_dgeevx(LapackEigenvalues.NonSymmetricMatrixBalancesType balanceType, bool computeLeftEigenvectors, bool computeRightEigenvectors, NonSymmetricXgeevxSense sense, int n, Span<double> a, Span<double> wr, Span<double> wi, Span<double> vl, Span<double> vr, out int ilo, out int ihi, Span<double> scale, out double abnrm, Span<double> rconde, Span<double> rcondv, Span<double> work, int[] iwork);

            /// <summary>Gets a optimal workspace array length for the <c>driver_zgeevx</c> function.
            /// </summary>
            /// <param name="balanceType">Indicates how the input matrix should be diagonally scaled and/or permuted to improve the conditioning of its eigenvalues.</param>
            /// <param name="computeLeftEigenvectors">A value indicating whether to calculate left eigenvectors.</param>
            /// <param name="computeRightEigenvectors">A value indicating whether to calculate right eigenvectors.</param>
            /// <param name="sense">Determines which reciprocal condition number are computed.</param>
            /// <param name="n">The order of the matrix.</param>
            /// <returns>The optimal workspace array length.</returns>
            int driver_zgeevxQuery(LapackEigenvalues.NonSymmetricMatrixBalancesType balanceType, bool computeLeftEigenvectors, bool computeRightEigenvectors, NonSymmetricXgeevxSense sense, int n);

            /// <summary>Computes the eigenvalues and left and right eigenvectors of a general matrix, with preliminary matrix balancing, and computes reciprocal condition numbers for the eigenvalues and right eigenvectors.
            /// </summary>
            /// <param name="balanceType">Indicates how the input matrix should be diagonally scaled and/or permuted to improve the conditioning of its eigenvalues.</param>
            /// <param name="computeLeftEigenvectors">A value indicating whether to calculate left eigenvectors.</param>
            /// <param name="computeRightEigenvectors">A value indicating whether to calculate right eigenvectors.</param>
            /// <param name="sense">Determines which reciprocal condition number are computed.</param>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="a">Contains the <paramref name="n"/>-by-<paramref name="n"/> matrix A; overwritten by intermediate results.</param>
            /// <param name="w">Contains the eigenvalues (output).</param>
            /// <param name="vl">Contains computed left eigenvectors if specified by <paramref name="computeLeftEigenvectors"/> (output).</param>
            /// <param name="vr">Contains computed right eigenvectors if specified by <paramref name="computeRightEigenvectors"/> (output).</param>
            /// <param name="ilo">Determined when A was balanced (output).</param>
            /// <param name="ihi">Determined when A was balanced (output).</param>
            /// <param name="scale">Details of the permutations and scaling factors applied when balancing A (output).</param>
            /// <param name="abnrm">The one-norm of the balanced matrix (the maximum of the sum of absolute values of elements of any column); (output).</param>
            /// <param name="rconde">For each eigenvalue the corresponding reciprocal condition number (output).</param>
            /// <param name="rcondv">For each right eigenvector the corresponding reciprocal condition number (output).</param>
            /// <param name="work">A workspace array.</param>
            /// <param name="rwork">A workspace array with dimension at least 2 * <paramref name="n"/>.</param>
            void driver_zgeevx(LapackEigenvalues.NonSymmetricMatrixBalancesType balanceType, bool computeLeftEigenvectors, bool computeRightEigenvectors, NonSymmetricXgeevxSense sense, int n, Span<Complex> a, Span<Complex> w, Span<Complex> vl, Span<Complex> vr, out int ilo, out int ihi, Span<double> scale, out double abnrm, Span<double> rconde, Span<double> rcondv, Span<Complex> work, Span<double> rwork);

#pragma warning restore IDE1006 // Naming Styles
        }
    }
}