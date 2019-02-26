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

        /// <summary>A value indicating what kind of job to do by the LAPACK function 'dstemr' and 'zstemr', 'dsyev' and 'dsyevd', 'zheev' and 'zheevd', 'dstegr' and 'zstegr'.
        /// </summary>
        public enum SymmetricGeneralJob
        {
            /// <summary>Eigenvalues are computed only.
            /// </summary>
            EigenValuesOnly,

            /// <summary>Eigenvalues and eigenvectors are computed.
            /// </summary>
            All
        }

        /// <summary>A value indicating what kind of range to use for the LAPACK function 'dstemr' and 'zstemr', 'dstegr' and 'zstegr', 'dstebz'.
        /// </summary>
        public enum SymmetricEigenvaluesRange
        {
            /// <summary>The routine computes all eigenvalues.
            /// </summary>
            All,

            /// <summary>The routine computes all eigenvalues in the half-open interval: (vl, vu].
            /// </summary>
            Boundaries,

            /// <summary>The routine computes eigenvalues with indices il to iu.
            /// </summary>
            Indices
        }

        /// <summary>A value indicating what kind of job to do by the LAPACK function 'dsteqr' and 'zsteqr', 'dstedc' and 'zstedc', 'dpteqr' and 'zpteqr'.
        /// </summary>
        public enum SymmetricJob
        {
            /// <summary>The routine computes eigenvalues only.
            /// </summary>
            EigenValuesOnly,

            /// <summary>The routine computes the eigenvalues and eigenvectors of the tridiagonal matrix T.
            /// </summary>
            ForTridiagonalMatrix,

            /// <summary>The routine computes the eigenvalues and eigenvectors of original A (and the array z must contain the matrix Q on entry).
            /// </summary>
            ForOriginalMatrix
        }

        /// <summary>A value indicating the way how to order the eigenvalues for LAPACK function 'dstebz'.
        /// </summary>
        public enum SymmetricDstebzOrder
        {
            /// <summary>Ehe eigenvalues are to be ordered from smallest to largest within each split-off block.
            /// </summary>
            Blockwise,

            /// <summary>The eigenvalues for the entire matrix are to be ordered from smallest to largest.
            /// </summary>
            Complete
        }

        /// <summary>A value indicating what kind of job to do by the LAPACK function 'dsbtrd' and 'zhbtrd'.
        /// </summary>
        public enum SymmetricXxbtrdJob
        {
            /// <summary>The routine returns the explicit matrix Q.
            /// </summary>
            CalculateMatrixQ,

            /// <summary>The routine does not return matrix Q.
            /// </summary>
            None
        }

        /// <summary>Specifies for which problem the reciprocal condition numbers should be compute by LAPACK function 'ddisna'.
        /// </summary>
        public enum SymmetricDdisnaJob
        {
            /// <summary>Ehe eigenvectors of a symmetric/Hermitian matrix.
            /// </summary>
            Eigenvectors,

            /// <summary>The left singular vectors of a general matrix.
            /// </summary>
            LeftSingularVectors,

            /// <summary>The right singular vectors of a general matrix.
            /// </summary>
            RightSingularVectors
        }
        #endregion

        /// <summary>Provides methods for Eigenvalue problems of symmetric matrices. See the documentation of the Linear Algebra PACKage http://www.netlib.org/lapack/index.html.
        /// </summary>
        public interface ISymmetricEigenvalueProblems
        {
#pragma warning disable IDE1006 // Naming Styles

            /// <summary>Gets a optimal workspace array length for the <c>dsytrd</c> function.
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>
            /// <returns>The optimal workspace array length.</returns>
            /// <remarks>The parameter <paramref name="triangularMatrixType"/> should not have an impact of the calculation of the optimal length of the workspace array.</remarks>
            int dsytrdQuery(int n, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Reduces a real symmetric matrix to tridiagonal form, i.e. A = Q * T * Q'.
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="a">The symmetric matrix provided column-by-column, i.e. the length should be at least <paramref name="n"/>^2; on exit it will be overwritten by the tridiagonal form and details of the orthogonal matrix.</param>
            /// <param name="d">The diagonal elements of the tridiagonal matrix; the array should have at least <paramref name="n"/> elements (output).</param>
            /// <param name="e">The off-diagonal elements of the tridiagonal matrix; the array should have at least <paramref name="n"/> - 1 elements (output).</param>
            /// <param name="tau">Further details of the orthogonal matrix in the first <paramref name="n"/> - 1 elements, tau[n] is used as workspace; the array should have at least <paramref name="n"/> elements (output).</param>
            /// <param name="work">A workspace array with at least <paramref name="n"/> elements.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>
            void dsytrd(int n, Span<double> a, Span<double> d, Span<double> e, Span<double> tau, Span<double> work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Gets a optimal workspace array length for the <c>dorgtr</c> function.
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>
            /// <returns>The optimal workspace array length.</returns>
            /// <remarks>The parameter <paramref name="triangularMatrixType"/> should not have an impact of the calculation of the optimal length of the workspace array.</remarks>
            int dorgtrQuery(int n, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Generates the real orthogonal matrix Q determined by <c>dsytrd</c>.
            /// </summary>
            /// <param name="n">The order of matrix Q.</param>
            /// <param name="a">This parameter should be the output of <c>dsytrd</c>; will be overwritten by the orthogonal matrix Q.</param>
            /// <param name="tau">This parameter should be the output of <c>dsytrd</c>. </param>
            /// <param name="work">A workspace array.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>
            /// <remarks>The routine explicitly generates the n-by-n orthogonal matrix Q formed by dsytrd when reducing a real symmetric matrix A to tridiagonal form A = Q * T * Q'. Use this routine after a call to <c>dsytrd</c>.</remarks>
            void dorgtr(int n, Span<double> a, Span<double> tau, Span<double> work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Gets a optimal workspace array length for the <c>dormtr</c> function.
            /// </summary>
            /// <param name="m">The number of rows in matrix C.</param>
            /// <param name="n">The number of columns in matrix C.</param>
            /// <param name="side">A value indicating whether op(Q) is applied to matrix C from the left or from the right.</param>
            /// <param name="transposeState">A value indicating whether the routine multiplies C by Q or Q'.</param>
            /// <param name="triangularMatrixType">Use the same parameter as supplied to <c>dsytrd</c>.</param>
            /// <returns>The optimal workspace array length.</returns>
            int dormtrQuery(int m, int n, LAPACK.Side side = LAPACK.Side.Left, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Multiplies a real matrix C by the real orthogonal matrix Q determined by <c>dsytrd</c>.
            /// </summary>
            /// <param name="m">The number of rows in matrix C.</param>
            /// <param name="n">The number of columns in matrix C.</param>
            /// <param name="a">This parameter should be the array returned by <c>dsytrd</c>.</param>
            /// <param name="tau">This parameter should be the array returned by <c>dsytrd</c>.</param>
            /// <param name="c">The matrix C provided column-by-column; overwritten by the product op(Q)*C or C*op(Q).</param>
            /// <param name="work">A workspace array.</param>
            /// <param name="side">A value indicating whether op(Q) is applied to matrix C from the left or from the right.</param>
            /// <param name="transposeState">A value indicating whether the routine multiplies C by Q or Q'.</param>
            /// <param name="triangularMatrixType">Use the same parameter as supplied to <c>dsytrd</c>.</param>
            /// <remarks>The routine multiplies a real matrix C by Q or Q', where Q is the orthogonal matrix Q formed by <c>dsytrd</c> when reducing a real symmetric matrix A to tridiagonal form A = Q * T * Q'. Use this routine after a call to <c>dsytrd</c>.</remarks>
            void dormtr(int m, int n, Span<double> a, Span<double> tau, Span<double> c, Span<double> work, LAPACK.Side side, BLAS.MatrixTransposeState transposeState, BLAS.TriangularMatrixType triangularMatrixType);

            /// <summary>Gets a optimal workspace array length for the <c>zhetrd</c> function.
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>
            /// <returns>The optimal workspace array length.</returns>
            /// <remarks>The parameter <paramref name="triangularMatrixType"/> should not have an impact of the calculation of the optimal length of the workspace array.</remarks>
            int zhetrdQuery(int n, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Reduces a complex Hermitian matrix to tridiagonal form, i.e. A = Q * T * Q^H.       
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="a">The Hermitian matrix provided column-by-column; on exit it will be overwritten by the tridiagonal form and details of the orthogonal matrix.</param>
            /// <param name="d">The diagonal elements of the tridiagonal matrix; the array should have at least <paramref name="n"/> elements (output).</param>
            /// <param name="e">The off-diagonal elements of the tridiagonal matrix; the array should have at least <paramref name="n"/> - 1 elements (output).</param>
            /// <param name="tau">Further details of the orthogonal matrix in the first <paramref name="n"/> - 1 elements; the array should have at least <paramref name="n"/> - 1 elements (output).</param>
            /// <param name="work">A workspace array.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the Hermitian input matrix is stored.</param>              
            void zhetrd(int n, Span<Complex> a, Span<double> d, Span<double> e, Span<Complex> tau, Span<Complex> work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Gets a optimal workspace array length for the <c>zungtr</c> function.
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="triangularMatrixType">Should be the same as supplied to <c>zhetrd</c>.</param>
            /// <returns>The optimal workspace array length.</returns>
            /// <remarks>The parameter <paramref name="triangularMatrixType"/> should not have an impact of the calculation of the optimal length of the workspace array.</remarks>
            int zungtrQuery(int n, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Generates the complex unitary matrix Q determined by <c>zhetrd</c>.
            /// </summary>
            /// <param name="n">The order of the matrix Q.</param>
            /// <param name="a">This parameter should be the array returned by <c>zhetrd</c>.</param>
            /// <param name="tau">This parameter should be the array returned by <c>zhetrd</c>.</param>
            /// <param name="work">A workspace array.</param>
            /// <param name="triangularMatrixType">Should be the same as supplied to <c>zhetrd</c>.</param>
            /// <remarks>The routine explicitly generates the n-by-n unitary matrix Q formed by <c>zhetrd</c> when reducing a complex Hermitian matrix A to tridiagonal form A = Q * T * Q^H. Use this routine after a call to <c>zhetrd</c>.</remarks>
            void zungtr(int n, Span<Complex> a, Span<Complex> tau, Span<Complex> work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Gets a optimal workspace array length for the <c>zunmtr</c> function.
            /// </summary>
            /// <param name="m">The number of rows in matrix C.</param>
            /// <param name="n">The number of columns in matrix C.</param>
            /// <param name="side">A value indicating whether op(Q) is applied to matrix C from the left or from the right.</param>
            /// <param name="transposeState">A value indicating whether the routine multiplies C by Q or Q'.</param>
            /// <param name="triangularMatrixType">Use the same parameter as supplied to <c>zhetrd</c>.</param>
            /// <returns>The optimal workspace array length.</returns>
            int zunmtrQuery(int m, int n, LAPACK.Side side = LAPACK.Side.Left, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Multiplies a complex matrix by the complex unitary matrix Q determined by <c>zhetrd</c>.
            /// </summary>
            /// <param name="m">The number of rows in matrix C.</param>
            /// <param name="n">The number of columns in matrix C.</param>
            /// <param name="a">This parameter should be the array returned by <c>zhetrd</c>.</param>
            /// <param name="tau">This parameter should be the array returned by <c>zhetrd</c>.</param>
            /// <param name="c">The matrix C provided column-by-column; overwritten by the product op(Q)*C or C*op(Q).</param>
            /// <param name="work">A workspace array.</param>
            /// <param name="side">A value indicating whether op(Q) is applied to matrix C from the left or from the right.</param>
            /// <param name="transposeState">A value indicating whether the routine multiplies C by Q or Q'.</param>
            /// <param name="triangularMatrixType">Use the same parameter as supplied to <c>zhetrd</c>.</param>
            void zunmtr(int m, int n, Span<Complex> a, Span<Complex> tau, Span<Complex> c, Span<Complex> work, LAPACK.Side side, BLAS.MatrixTransposeState transposeState, BLAS.TriangularMatrixType triangularMatrixType);

            /// <summary>Reduces a real symmetric matrix to tridiagonal form using packed storage, i.e. a packed real symmetric matrix A is transformed to symmetric tridiagonal form T by an orthogonal similarity transformation A = Q * T * Q'.
            /// </summary>
            /// <param name="n">The order of the specified symmetric matrix.</param>
            /// <param name="ap">The specified symmetric matrix in packed form, i.e. either upper or lower triangle as specified in <paramref name="triangularMatrixType"/> with at least <paramref name="n"/> * (<paramref name="n"/> + 1) / 2 elements.</param>
            /// <param name="d">The diagonal elements of the tridiagonal matrix; the array should have at least <paramref name="n"/> elements (output).</param>
            /// <param name="e">The off-diagonal elements of the tridiagonal matrix; the array should have at least <paramref name="n"/> - 1 elements (output).</param>
            /// <param name="tau">Further details of the orthogonal matrix in the first <paramref name="n"/> - 1 elements; the array should have at least <paramref name="n"/> - 1 elements (output).</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>
            void dsptrd(int n, Span<double> ap, Span<double> d, Span<double> e, Span<double> tau, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Generates the real orthogonal matrix Q determined by <c>dsptrd</c>.
            /// </summary>
            /// <param name="n">The order of matrix Q.</param>
            /// <param name="ap">This parameter should be the output of <c>dsptrd</c>.</param>
            /// <param name="tau">This parameter should be the output of <c>dsptrd</c>.</param>
            /// <param name="q">Contains the computed matrix Q of dimension <paramref name="n"/> x <paramref name="n"/>, provided column-by-column (output).</param>
            /// <param name="work">A workspace array with at least <paramref name="n"/> - 1 elements.</param>
            /// <param name="triangularMatrixType">Use the same parameter as supplied to <c>dsptrd</c>.</param>
            void dopgtr(int n, Span<double> ap, Span<double> tau, Span<double> q, Span<double> work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Multiplies a real matrix by the real orthogonal matrix Q determined by <c>dsptrd</c>.
            /// </summary>
            /// <param name="m">The number of rows in matrix C.</param>
            /// <param name="n">The number of columns in matrix C.</param>
            /// <param name="ap">This parameter should be the array returned by <c>dsptrd</c>.</param>
            /// <param name="tau">This parameter should be the array returned by <c>dsptrd</c>.</param>
            /// <param name="c">The matrix C provided column-by-column; overwritten by the product op(Q) * C or C * op(Q) as specified by <paramref name="side"/> and <paramref name="transposeState"/>.</param>
            /// <param name="work">A workspace array. The dimension must be at least <paramref name="n"/> if <paramref name="side"/> indicates a left multiplication; otherwise at least <paramref name="m"/>.</param>
            /// <param name="side">A value indicating whether op(Q) is applied to matrix C from the left or from the right.</param>
            /// <param name="transposeState">A value indicating whether the routine multiplies C by Q or Q'.</param>
            /// <param name="triangularMatrixType">Use the same parameter as supplied to <c>zhetrd</c>.</param>
            /// <remarks>The routine multiplies a real matrix C by Q or Q', where Q is the orthogonal matrix Q formed by <c>dsptrd</c> when reducing a packed real symmetric matrix A to tridiagonal form A = Q * T * Q'. Use this routine after a call to <c>dsptrd</c>.</remarks>
            void dopmtr(int m, int n, Span<double> ap, Span<double> tau, Span<double> c, Span<double> work, LAPACK.Side side = LAPACK.Side.Left, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Reduces a complex Hermitian matrix to tridiagonal form T by a unitary similarity transformation A = Q * T *Q^H using packed storage.
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="ap">The Hermitian matrix, i.e. either upper or lower triangle in packed form; on exit it will be overwritten by the tridiagonal matrix T and details of the orthogonal matrix Q. This parameter should have a length of at least <paramref name="n"/> * ( <paramref name="n"/> + 1) / 2.</param>
            /// <param name="d">The diagonal elements of the tridiagonal matrix; the array should have at least <paramref name="n"/> elements (output).</param>
            /// <param name="e">The off-diagonal elements of the tridiagonal matrix; the array should have at least <paramref name="n"/> - 1 elements (output).</param>
            /// <param name="tau">Further details of the orthogonal matrix in the first <paramref name="n"/> - 1 elements; the array should have at least <paramref name="n"/> - 1 elements (output).</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the Hermitian input matrix is stored.</param>        
            void zhptrd(int n, Span<Complex> ap, Span<double> d, Span<double> e, Span<Complex> tau, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Generates the complex unitary matrix Q determined by <c>zhptrd</c>.
            /// </summary>
            /// <param name="n">The order of the matrix Q.</param>
            /// <param name="ap">This parameter should be the array returned by <c>zhptrd</c>.</param>
            /// <param name="tau">This parameter should be the array returned by <c>zhptrd</c>.</param>
            /// <param name="q">The computed matrix Q provided column-by-column of dimension n x n (output).</param>
            /// <param name="work">A workspace array with at least <paramref name="n"/> -1 elements.</param>
            /// <param name="triangularMatrixType">Should be the same as supplied to <c>zhptrd</c>.</param>
            void zupgtr(int n, Span<Complex> ap, Span<Complex> tau, Span<Complex> q, Span<Complex> work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Multiplies a complex matrix by the unitary matrix Q determined by <c>zhptrd</c>.
            /// </summary>
            /// <param name="m">The number of rows in matrix C.</param>
            /// <param name="n">The number of columns in matrix C.</param>
            /// <param name="ap">This parameter should be the array returned by <c>zhptrd</c>; the length should be at least r * (r + 1) / 2, where r = <paramref name="m"/> if <paramref name="side"/> indicates a left multiplication; <paramref name="n"/> otherwise.</param>
            /// <param name="tau">This parameter should be the array returned by <c>zhptrd</c>.</param>
            /// <param name="c">The matrix C provided column-by-column; overwritten by the product op(Q)*C or C * op(Q).</param>
            /// <param name="work">A workspace array. The dimension must be at least <paramref name="n"/> if <paramref name="side"/> indicates a left multiplication; otherwise at least <paramref name="m"/>.</param>
            /// <param name="side">A value indicating whether op(Q) is applied to matrix C from the left or from the right.</param>
            /// <param name="transposeState">A value indicating whether the routine multiplies C by Q or Q^H.</param>
            /// <param name="triangularMatrixType">Use the same parameter as supplied to <c>zhptrd</c>.</param>
            void zupmtr(int m, int n, Span<Complex> ap, Span<Complex> tau, Span<Complex> c, Span<Complex> work, LAPACK.Side side = LAPACK.Side.Left, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Reduces a real symmetric band matrix to tridiagonal form T by an orthogonal similarity transformation A = Q * T * Q'.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="n">The order of the matrix A.</param>
            /// <param name="kd">The number of super- or sub-diagonals in matrix A.</param>
            /// <param name="ab">The lower or upper triangular part of the specified input matrix A in band storage format; will be overwritten on exit. This parameter should have a length of at least (<paramref name="kd"/> + 1) * <paramref name="n"/>.</param>
            /// <param name="d">The diagonal elements of the tridiagonal matrix T; the array should have at least <paramref name="n"/> elements (output).</param>
            /// <param name="e">The off-diagonal elements of the tridiagonal matrix T; the array should have at least <paramref name="n"/> - 1 elements (output).</param>
            /// <param name="q">The matrix Q if <paramref name="job"/> indicates to take into account this parameter, if referenced it should have at least <paramref name="n"/> * <paramref name="n"/> elements and will be overwritten on exit.</param>
            /// <param name="work">A workspace array with at least <paramref name="n"/> elements.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>        
            /// <remarks>Note that diagonal (d) and off-diagonal (e) elements of the matrix T are omitted because they are kept in the matrix A on exit.</remarks>
            void dsbtrd(LapackEigenvalues.SymmetricXxbtrdJob job, int n, int kd, Span<double> ab, Span<double> d, Span<double> e, Span<double> q, Span<double> work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Reduces a complex Hermitian band matrix to tridiagonal form T by an unitary similarity transformation A = Q * T * Q^H.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="n">The order of the matrix A.</param>
            /// <param name="kd">The number of super- or sub-diagonals in matrix A.</param>
            /// <param name="ab">The lower or upper triangular part of the specified input matrix A in band storage format; will be overwritten on exit. This parameter should have a length of at least (<paramref name="kd"/> + 1) * <paramref name="n"/>.</param>
            /// <param name="d">The diagonal elements of the tridiagonal matrix T; the array should have at least <paramref name="n"/> elements (output).</param>
            /// <param name="e">The off-diagonal elements of the tridiagonal matrix T; the array should have at least <paramref name="n"/> - 1 elements (output).</param>
            /// <param name="q">The matrix Q if <paramref name="job"/> indicates to take into account this parameter, if referenced it should have at least <paramref name="n"/> * <paramref name="n"/> elements and will be overwritten on exit.</param>
            /// <param name="work">A workspace array with at least <paramref name="n"/> elements.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>        
            /// <remarks>Note that diagonal (d) and off-diagonal (e) elements of the matrix T are omitted because they are kept in the matrix A on exit.</remarks>
            void zhbtrd(LapackEigenvalues.SymmetricXxbtrdJob job, int n, int kd, Span<Complex> ab, Span<double> d, Span<double> e, Span<Complex> q, Span<Complex> work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Computes all eigenvalues of a real symmetric tridiagonal matrix T (which can be obtained by reducing a symmetric or Hermitian matrix to tridiagonal form) using QR algorithm.
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="d">Contails the diagonal element of the specified tridiagonal matrix T, i.e. the array should have at least <paramref name="n"/> elements; on exit overwritten by the <paramref name="n"/> eigenvalues in ascending order.</param>
            /// <param name="e">Contains the off-diagonal elements of the specified tridiagonal matrix T, i.e. the array should have at least <paramref name="n"/> -1 elements; will be overwritten on exit.</param>
            void dsterf(int n, Span<double> d, Span<double> e);

            /// <summary>Computes all eigenvalues and eigenvectors of a symmetric matrix reduced to tridiagonal form (QR algorithm), i.e. T = Z * D * Z', where
            /// D is the diagonal matrix whose diagonal elements are the eigenvalues, Z is an orthogonal matrix whose columns are eigenvectors.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="n">The order of the tridiagonal matrix T.</param>
            /// <param name="d">The diagonal elements of the tridiagonal matrix, i.e. at least <paramref name="n"/> elements; overwritten by the <paramref name="n"/> eigenvalues in ascending order.</param>
            /// <param name="e">The off-diagonal elements of the tridiagonal matrix T, i.e. at least <paramref name="n"/> -1 elements; overwritten on exit.</param>
            /// <param name="z">If <paramref name="job"/> indicates to take into account this parameter the n-by-n matrix Q on exit (output).</param>
            /// <param name="work">A workspace array with at least 2 * <paramref name="n"/> -2 elements if <paramref name="job"/> indicates to calculate eigenvalues and eigenvectors; otherwise at least 1 element.</param>
            /// <remarks>Before calling <c>dsteqr</c>, you must reduce A to tridiagonal form and generate the explicit matrix Q by calling on specific LAPACK routines.</remarks>
            void dsteqr(LapackEigenvalues.SymmetricJob job, int n, Span<double> d, Span<double> e, Span<double> z, Span<double> work);

            /// <summary>Computes all eigenvalues and eigenvectors of a Hermitian matrix reduced to tridiagonal form (QR algorithm)
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="n">The order of the tridiagonal matrix T.</param>
            /// <param name="d">The diagonal elements of the tridiagonal matrix, i.e. at least <paramref name="n"/> elements; overwritten by the <paramref name="n"/> eigenvalues in ascending order.</param>
            /// <param name="e">The off-diagonal elements of the tridiagonal matrix T, i.e. at least <paramref name="n"/> -1 elements; overwritten on exit.</param>
            /// <param name="z">If <paramref name="job"/> indicates to take into account this parameter the n-by-n matrix Q on exit (output).</param>
            /// <param name="work">A workspace array with at least 2 * <paramref name="n"/> -2 elements if <paramref name="job"/> indicates to calculate eigenvalues and eigenvectors; otherwise at least 1 element.</param>
            /// <remarks>Before calling <c>zsteqr</c>, you must reduce A to tridiagonal form and generate the explicit matrix Q by calling on specific LAPACK routines.</remarks>
            void zsteqr(LapackEigenvalues.SymmetricJob job, int n, Span<double> d, Span<double> e, Span<Complex> z, Span<double> work);

            /// <summary>Gets a optimal workspace array length for the <c>dstemr</c> function.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="range">A value indicating which eigenvalues to compute.</param>
            /// <param name="n">The order of the tridiagonal matrix.</param>
            /// <param name="vl">The lower bound of the interval to be searched for eigenvalues.</param>
            /// <param name="vu">The upper bound of the interval to be searched for eigenvalues.</param>
            /// <param name="il">The lower index of the smallest and largest eigenvalues to be returned.</param>
            /// <param name="iu">The upper index of the smallest and largest eigenvalues to be returned.</param>
            /// <param name="liwork">The optimal workspace array length for parameter 'work'.</param>
            /// <param name="lwork">The optimal workspace array length for parameter 'iwork'.</param>
            /// <param name="nzc">The optimal value of parameter 'nzc'.</param>
            void dstemrQuery(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, double vl, double vu, int il, int iu, out int lwork, out int liwork, out int nzc);

            /// <summary>Computes selected eigenvalues and eigenvectors of a real symmetric tridiagonal matrix. The spectrum may be computed either completely or partially by specifying either an interval (<paramref name="vl"/>,<paramref name="vu"/>] or a range of indices <paramref name="il"/>:<paramref name="iu"/> for the desired eigenvalues.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="range">A value indicating which eigenvalues to compute.</param>
            /// <param name="n">The order of the tridiagonal matrix.</param>
            /// <param name="d">The diagonal elements of the tridiagonal matrix, i.e. at least <paramref name="n"/> elements.</param>
            /// <param name="e">The off-diagonal elements of the tridiagonal matrix in the first <paramref name="n"/> - 1 elements, e[n] is used as workspace i.e. at least <paramref name="n"/> elements.</param>
            /// <param name="vl">The lower bound of the interval to be searched for eigenvalues.</param>
            /// <param name="vu">The upper bound of the interval to be searched for eigenvalues.</param>
            /// <param name="il">The lower index of the smallest and largest eigenvalues to be returned.</param>
            /// <param name="iu">The upper index of the smallest and largest eigenvalues to be returned.</param>
            /// <param name="m">The total number of eigenvalues found (output).</param>
            /// <param name="w">The first <paramref name="m"/> values contain the selected eigenvalues in ascending order; this array should have a length of at least <paramref name="n"/> (output).</param>
            /// <param name="z">If <paramref name="job"/> indicates to compute eigenvectors, the first <paramref name="m"/> columns of matrix Z contain the orthonormal eigenvectors.</param>
            /// <param name="nzc">The number of eigenvectors to be held in the array z.</param>
            /// <param name="isuppz">The support of the eigenvectors in matrix Z, that is the indices indicating the nonzero elements in z (output).</param>
            /// <param name="work">A workspace array with at least 18 * <paramref name="n"/> elements.</param>
            /// <param name="iwork">A workspace array with at least 10 * <paramref name="n"/> elements; at least 8 * <paramref name="n"/> elements if only eigenvalues to be computed.</param>
            /// <param name="tryrac"><c>true</c> indicates that the code should check whether the tridiagonal matrix defines its eigenvalues to high relative accuracy. If so, the code uses relative-accuracy preserving algorithms 
            /// that might be (a bit) slower depending on the matrix. If the matrix does not define its eigenvalues to high relative accuracy, the code can uses possibly faster algorithms;
            /// otherwise the code is not required to guarantee relatively accurate eigenvalues and can use the fastest possible techniques.</param>
            void dstemr(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, Span<double> d, Span<double> e, double vl, double vu, int il, int iu, out int m, Span<double> w, Span<double> z, int nzc, int[] isuppz, Span<double> work, int[] iwork, bool tryrac = true);

            /// <summary>Computes selected eigenvalues and eigenvectors of a real symmetric tridiagonal matrix.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="range">A value indicating which eigenvalues to compute.</param>
            /// <param name="n">The order of the tridiagonal matrix.</param>
            /// <param name="d">The diagonal elements of the tridiagonal matrix, i.e. at least <paramref name="n"/> elements.</param>
            /// <param name="e">The off-diagonal elements of the tridiagonal matrix in the first <paramref name="n"/> - 1 elements, e[n] is used as workspace i.e. at least <paramref name="n"/> elements.</param>
            /// <param name="vl">The lower bound of the interval to be searched for eigenvalues.</param>
            /// <param name="vu">The upper bound of the interval to be searched for eigenvalues.</param>
            /// <param name="il">The lower index of the smallest and largest eigenvalues to be returned.</param>
            /// <param name="iu">The upper index of the smallest and largest eigenvalues to be returned.</param>
            /// <param name="m">The total number of eigenvalues found (output).</param>
            /// <param name="w">The first <paramref name="m"/> values contain the selected eigenvalues in ascending order; this array should have a length of at least <paramref name="n"/> (output).</param>
            /// <param name="z">If <paramref name="job"/> indicates to compute eigenvectors, the first <paramref name="m"/> columns of matrix Z contain the orthonormal eigenvectors.</param>
            /// <param name="nzc">The number of eigenvectors to be held in the array z.</param>
            /// <param name="isuppz">The support of the eigenvectors in matrix Z, that is the indices indicating the nonzero elements in z (output).</param>
            /// <param name="work">A workspace array with at least 18 * <paramref name="n"/> elements.</param>
            /// <param name="iwork">A workspace array with at least 10 * <paramref name="n"/> elements; at least 8 * <paramref name="n"/> elements if only eigenvalues to be computed.</param>
            /// <param name="tryrac"><c>true</c> indicates that the code should check whether the tridiagonal matrix defines its eigenvalues to high relative accuracy. If so, the code uses relative-accuracy preserving algorithms 
            /// that might be (a bit) slower depending on the matrix. If the matrix does not define its eigenvalues to high relative accuracy, the code can uses possibly faster algorithms;
            /// otherwise the code is not required to guarantee relatively accurate eigenvalues and can use the fastest possible techniques.</param>
            void zstemr(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, Span<double> d, Span<double> e, double vl, double vu, int il, int iu, out int m, Span<double> w, Span<Complex> z, int nzc, int[] isuppz, Span<double> work, int[] iwork, bool tryrac = true);

            /// <summary>Gets a optimal workspace array length for the <c>dstedc</c> function.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="n">The order of the symmetric tridiagonal matrix.</param>
            /// <param name="liwork">The optimal workspace array length for parameter 'work'.</param>
            /// <param name="lwork">The optimal workspace array length for parameter 'iwork'.</param>
            void dstedcQuery(LapackEigenvalues.SymmetricJob job, int n, out int lwork, out int liwork);

            /// <summary>Computes all eigenvalues and eigenvectors of a symmetric tridiagonal matrix using the divide and conquer method.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="n">The order of the symmetric tridiagonal matrix.</param>
            /// <param name="d">The diagonal elements of the tridiagonal matrix, i.e. at least <paramref name="n"/> elements; will be overwritten by the <paramref name="n"/> eigenvalues in ascending order (output).</param>
            /// <param name="e">The off-diagonal elements of the tridiagonal matrix in the first <paramref name="n"/> - 1 elements; will be overwritten.</param>
            /// <param name="z">The orthogonal matrix used to reduce the original matrix to tridiagonal form; on exist contains the orthonormal eigenvectors (output).</param>
            /// <param name="work">A workspace array.</param>
            /// <param name="iwork">A workspace array.</param>
            void dstedc(LapackEigenvalues.SymmetricJob job, int n, Span<double> d, Span<double> e, Span<double> z, Span<double> work, int[] iwork);

            /// <summary>Gets a optimal workspace array length for the <c>zstedc</c> function.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="n">The order of the symmetric tridiagonal matrix.</param>
            /// <param name="liwork">The optimal workspace array length for parameter 'work'.</param>
            /// <param name="lwork">The optimal workspace array length for parameter 'iwork'.</param>
            /// <param name="lrwork">The optimal workspace array length for parameter 'rwork'.</param>
            void zstedcQuery(LapackEigenvalues.SymmetricJob job, int n, out int lwork, out int liwork, out int lrwork);

            /// <summary>Computes all eigenvalues and eigenvectors of a symmetric tridiagonal matrix using the divide and conquer method.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="n">The order of the symmetric tridiagonal matrix.</param>
            /// <param name="d">The diagonal elements of the tridiagonal matrix, i.e. at least <paramref name="n"/> elements; will be overwritten by the <paramref name="n"/> eigenvalues in ascending order (output).</param>
            /// <param name="e">The off-diagonal elements of the tridiagonal matrix in the first <paramref name="n"/> - 1 elements; will be overwritten.</param>
            /// <param name="z">The orthogonal matrix used to reduce the original matrix to tridiagonal form; on exist contains the orthonormal eigenvectors (output).</param>
            /// <param name="work">A workspace array.</param>
            /// <param name="iwork">A workspace array.</param>
            /// <param name="rwork">A workspace array.</param>
            void zstedc(LapackEigenvalues.SymmetricJob job, int n, Span<double> d, Span<double> e, Span<Complex> z, Span<Complex> work, int[] iwork, Span<double> rwork);

            /// <summary>Computes selected eigenvalues and eigenvectors of a real symmetric tridiagonal matrix
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="range">A value indicating which eigenvalues to compute.</param>
            /// <param name="n">The order of the tridiagonal matrix.</param>
            /// <param name="d">The diagonal elements of the tridiagonal matrix, i.e. at least <paramref name="n"/> elements.</param>
            /// <param name="e">The off-diagonal elements of the tridiagonal matrix in the first <paramref name="n"/> - 1 elements, e[n] is used as workspace i.e. at least <paramref name="n"/> elements.</param>
            /// <param name="vl">The lower bound of the interval to be searched for eigenvalues.</param>
            /// <param name="vu">The upper bound of the interval to be searched for eigenvalues.</param>
            /// <param name="il">The lower index of the smallest and largest eigenvalues to be returned.</param>
            /// <param name="iu">The upper index of the smallest and largest eigenvalues to be returned.</param>
            /// <param name="m">The total number of eigenvalues found (output).</param>
            /// <param name="w">The first <paramref name="m"/> values contain the selected eigenvalues in ascending order; this array should have a length of at least <paramref name="n"/> (output).</param>
            /// <param name="z">If <paramref name="job"/> indicates to compute eigenvectors, the first <paramref name="m"/> columns of matrix Z contain the orthonormal eigenvectors.</param>
            /// <param name="isuppz">The support of the eigenvectors in matrix Z, that is the indices indicating the nonzero elements in z (output).</param>
            /// <param name="work">A workspace array with at least 18 * <paramref name="n"/> elements.</param>
            /// <param name="iwork">A workspace array with at least 10 * <paramref name="n"/> elements; at least 8 * <paramref name="n"/> elements if only eigenvalues to be computed.</param>
            void dstegr(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, Span<double> d, Span<double> e, double vl, double vu, int il, int iu, out int m, Span<double> w, Span<double> z, int[] isuppz, Span<double> work, int[] iwork);

            /// <summary>Computes selected eigenvalues and eigenvectors of a real symmetric tridiagonal matrix
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="range">A value indicating which eigenvalues to compute.</param>
            /// <param name="n">The order of the tridiagonal matrix.</param>
            /// <param name="d">The diagonal elements of the tridiagonal matrix, i.e. at least <paramref name="n"/> elements.</param>
            /// <param name="e">The off-diagonal elements of the tridiagonal matrix in the first <paramref name="n"/> - 1 elements, e[n] is used as workspace i.e. at least <paramref name="n"/> elements.</param>
            /// <param name="vl">The lower bound of the interval to be searched for eigenvalues.</param>
            /// <param name="vu">The upper bound of the interval to be searched for eigenvalues.</param>
            /// <param name="il">The lower index of the smallest and largest eigenvalues to be returned.</param>
            /// <param name="iu">The upper index of the smallest and largest eigenvalues to be returned.</param>
            /// <param name="m">The total number of eigenvalues found (output).</param>
            /// <param name="w">The first <paramref name="m"/> values contain the selected eigenvalues in ascending order; this array should have a length of at least <paramref name="n"/> (output).</param>
            /// <param name="z">If <paramref name="job"/> indicates to compute eigenvectors, the first <paramref name="m"/> columns of matrix Z contain the orthonormal eigenvectors.</param>
            /// <param name="isuppz">The support of the eigenvectors in matrix Z, that is the indices indicating the nonzero elements in z (output).</param>
            /// <param name="work">A workspace array with at least 18 * <paramref name="n"/> elements.</param>
            /// <param name="iwork">A workspace array with at least 10 * <paramref name="n"/> elements; at least 8 * <paramref name="n"/> elements if only eigenvalues to be computed.</param>
            void zstegr(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, Span<double> d, Span<double> e, double vl, double vu, int il, int iu, out int m, Span<double> w, Span<Complex> z, int[] isuppz, Span<double> work, int[] iwork);

            /// <summary>Computes all eigenvalues and (optionally) all eigenvectors of a real symmetric positive-definite tridiagonal matrix.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="n">The order of the tridiagonal matrix T.</param>
            /// <param name="d">The diagonal elements of the tridiagonal matrix, i.e. at least <paramref name="n"/> elements; on exit the <paramref name="n"/> eigenvalues in descending order.</param>
            /// <param name="e">The off-diagonal elements of the tridiagonal matrix T, i.e. at least <paramref name="n"/> -1 elements.</param>
            /// <param name="z">If <paramref name="job"/> indicates to take into account this parameter the n-by-n matrix Q on exit.</param>
            /// <param name="work">A workspace array with at least 4 * <paramref name="n"/> -4 elements.</param>
            void dpteqr(LapackEigenvalues.SymmetricJob job, int n, Span<double> d, Span<double> e, Span<double> z, Span<double> work);

            /// <summary>Computes all eigenvalues and (optionally) all eigenvectors of a real symmetric positive-definite tridiagonal matrix.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="n">The order of the tridiagonal matrix T.</param>
            /// <param name="d">The diagonal elements of the tridiagonal matrix, i.e. at least <paramref name="n"/> elements; on exit the <paramref name="n"/> eigenvalues in descending order.</param>
            /// <param name="e">The off-diagonal elements of the tridiagonal matrix T, i.e. at least <paramref name="n"/> -1 elements.</param>
            /// <param name="z">If <paramref name="job"/> indicates to take into account this parameter the n-by-n matrix Q on exit.</param>
            /// <param name="work">A workspace array with at least 4 * <paramref name="n"/> -4 elements.</param>
            void zpteqr(LapackEigenvalues.SymmetricJob job, int n, Span<double> d, Span<double> e, Span<Complex> z, Span<double> work);

            /// <summary>Computes selected eigenvalues of a real symmetric tridiagonal matrix by bisection.
            /// </summary>
            /// <param name="range">A value indicating which eigenvalues to compute.</param>
            /// <param name="order">A value indicating the way how to order the eigenvalues.</param>
            /// <param name="n">The order of the tridiagonal matrix.</param>
            /// <param name="d">The diagonal elements of the tridiagonal matrix, i.e. at least <paramref name="n"/> elements.</param>
            /// <param name="e">The off-diagonal elements of the tridiagonal matrix in the first <paramref name="n"/> - 1 elements, i.e. at least <paramref name="n"/> - 1 elements.</param>
            /// <param name="vl">The lower bound of the interval to be searched for eigenvalues.</param>
            /// <param name="vu">The upper bound of the interval to be searched for eigenvalues.</param>
            /// <param name="il">The lower index of the smallest and largest eigenvalues to be returned.</param>
            /// <param name="iu">The upper index of the smallest and largest eigenvalues to be returned.</param>
            /// <param name="m">The actual number of eigenvalues found (output).</param>
            /// <param name="w">The first <paramref name="m"/> values contain the selected eigenvalues in ascending order; this array should have a length of at least <paramref name="n"/> (output).</param>
            /// <param name="nsplit">The number of diagonal blocks detected (output).</param>
            /// <param name="isplit">The leading nsplit elements of isplit contain points at which T splits into blocks.</param>
            /// <param name="work">A workspace array with at least 3 * <paramref name="n"/> elements.</param>
            /// <param name="iblock">A positive value iblock(i) is the block number of the eigenvalue stored in w(i) (see Lapack documentation for further information).</param>
            /// <param name="abstol">The absolute tolerance to which each eigenvalue is required. An eigenvalue (or cluster) is considered to have converged if it lies in an interval of width abstol.</param>
            void dstebz(LapackEigenvalues.SymmetricEigenvaluesRange range, LapackEigenvalues.SymmetricDstebzOrder order, int n, Span<double> d, Span<double> e, double vl, double vu, int il, int iu, out int m, Span<double> w, out int nsplit, int[] iblock, int[] isplit, Span<double> work, double abstol = MachineConsts.Epsilon);

            /// <summary>Computes the eigenvectors corresponding to specified eigenvalues of a real symmetric tridiagonal matrix.
            /// </summary>
            /// <param name="n">The order of the tridiagonal matrix.</param>
            /// <param name="d">The diagonal elements of the tridiagonal matrix, i.e. at least <paramref name="n"/> elements.</param>
            /// <param name="e">The off-diagonal elements of the tridiagonal matrix in the first <paramref name="n"/> - 1 elements, i.e. at least <paramref name="n"/> -1 elements.</param>
            /// <param name="m">The number of eigenvectors to be returned.</param>
            /// <param name="w">Contains the eigenvalues stored in w[0] to w[m-1] (as returned by <c>dstebz</c>) in non-decresing order.</param>
            /// <param name="iblock">As returned by <c>dstebz</c>.</param>
            /// <param name="isplit">As returned by <c>dstebz</c>.</param>
            /// <param name="z">Contains the <paramref name="m"/> orthonomal eigenvectors, stored by columns (output).</param>
            /// <param name="ifailv">Contains the indices of any eigenvectors that failed to converge; i.e. at least <paramref name="m"/> elements.</param>
            /// <param name="work">A workspace array with at least 5 * <paramref name="n"/> elements.</param>
            /// <param name="iwork">A workspace array with at least <paramref name="n"/> elements.</param>
            void dstein(int n, Span<double> d, Span<double> e, int m, Span<double> w, int[] iblock, int[] isplit, Span<double> z, int[] ifailv, Span<double> work, int[] iwork);

            /// <summary>Computes the eigenvectors corresponding to specified eigenvalues of a real symmetric tridiagonal matrix.
            /// </summary>
            /// <param name="n">The order of the tridiagonal matrix.</param>
            /// <param name="d">The diagonal elements of the tridiagonal matrix, i.e. at least <paramref name="n"/> elements.</param>
            /// <param name="e">The off-diagonal elements of the tridiagonal matrix in the first <paramref name="n"/> - 1 elements, i.e. at least <paramref name="n"/> -1 elements.</param>
            /// <param name="m">The number of eigenvectors to be returned.</param>
            /// <param name="w">Contains the eigenvalues stored in w[0] to w[m-1] (as returned by <c>zstebz</c>) in non-decresing order.</param>
            /// <param name="iblock">As returned by <c>zstebz</c>.</param>
            /// <param name="isplit">As returned by <c>zstebz</c>.</param>
            /// <param name="z">Contains the <paramref name="m"/> orthonomal eigenvectors, stored by columns (output).</param>
            /// <param name="ifailv">Contains the indices of any eigenvectors that failed to converge; i.e. at least <paramref name="m"/> elements.</param>
            /// <param name="work">A workspace array with at least 5 * <paramref name="n"/> elements.</param>
            /// <param name="iwork">A workspace array with at least <paramref name="n"/> elements.</param>
            void zstein(int n, Span<double> d, Span<double> e, int m, Span<double> w, int[] iblock, int[] isplit, Span<Complex> z, int[] ifailv, Span<double> work, int[] iwork);

            /// <summary>Computes the reciprocal condition numbers for the eigenvectors of a symmetric/ Hermitian matrix or for the left or right singular vectors of a general matrix.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="m">The number of rows of the matrix.</param>
            /// <param name="n">The number of columns of the matrix.</param>
            /// <param name="d">Contains the eigenvalues or singular values of the matrix, in either increasing or decreasing order.</param>
            /// <param name="sep">The reciprocal condition numbers of the vectors (output).</param>
            void ddisna(LapackEigenvalues.SymmetricDdisnaJob job, int m, int n, Span<double> d, Span<double> sep);

            /// <summary>Gets a optimal workspace array length for the <c>driver_dsyev</c> function.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>
            /// <returns>The optimal workspace array length.</returns>
            /// <remarks>The parameter <paramref name="triangularMatrixType"/> should not have an impact of the calculation of the optimal length of the workspace array.</remarks>
            int driver_dsyevQuery(LapackEigenvalues.SymmetricGeneralJob job, int n, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Computes all eigenvalues and, optionally, eigenvectors of a real symmetric matrix.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="a">The symmetric matrix, i.e. containing either upper or lower triangular part of the symmetric matrix, as specified by <paramref name="triangularMatrixType"/>. The length should be at least <paramref name="n"/>^2; overwritten on exit, perhaps by the orthonormal eigenvectors.</param>
            /// <param name="w">The eigenvalues of <paramref name="a"/> in ascending order, i.e. the array should have at least <paramref name="n"/> elements (output).</param>
            /// <param name="work">A workspace array with at least 3 * <paramref name="n"/> - 1 elements.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>
            void driver_dsyev(LapackEigenvalues.SymmetricGeneralJob job, int n, Span<double> a, Span<double> w, Span<double> work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Gets a optimal workspace array length for the <c>driver_zheev</c> function.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the Hermitian matrix is stored.</param>
            /// <returns>The optimal workspace array length.</returns>
            /// <remarks>The parameter <paramref name="triangularMatrixType"/> should not have an impact of the calculation of the optimal length of the workspace array.</remarks>
            int driver_zheevQuery(LapackEigenvalues.SymmetricGeneralJob job, int n, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Computes all eigenvalues and, optionally, eigenvectors of a Hermitian matrix.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="a">The Hermitian matrix, i.e. containing either upper or lower triangular part of the Hermitian matrix, as specified by <paramref name="triangularMatrixType"/>. The length should be at least <paramref name="n"/>^2; overwritten on exit, perhaps by the orthonormal eigenvectors.</param>
            /// <param name="w">The eigenvalues of <paramref name="a"/> in ascending order, i.e. the array should have at least <paramref name="n"/> elements (output).</param>
            /// <param name="work">A workspace array with at least 3 * <paramref name="n"/> - 1 elements.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>
            void driver_zheev(LapackEigenvalues.SymmetricGeneralJob job, int n, Span<Complex> a, Span<double> w, Span<Complex> work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Gets a optimal workspace array length for the <c>driver_dsyevd</c> function.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="n">The order of the symmetric tridiagonal matrix.</param>
            /// <param name="liwork">The optimal workspace array length for parameter 'work'.</param>
            /// <param name="lwork">The optimal workspace array length for parameter 'iwork'.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>        
            /// <remarks>The parameter <paramref name="triangularMatrixType"/> should not have an impact of the calculation of the optimal length of the workspace array.</remarks>
            void driver_dsyevdQuery(LapackEigenvalues.SymmetricGeneralJob job, int n, out int lwork, out int liwork, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Computes all eigenvalues and (optionally) all eigenvectors of a real symmetric matrix using divide and conquer algorithm.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="a">The symmetric matrix, i.e. containing either upper or lower triangular part of the symmetric matrix, as specified by <paramref name="triangularMatrixType"/>. The length should be at least <paramref name="n"/>^2; overwritten on exit, perhaps by the orthonormal eigenvectors.</param>
            /// <param name="w">The eigenvalues of <paramref name="a"/> in ascending order, i.e. the array should have at least <paramref name="n"/> elements (output).</param>
            /// <param name="work">A workspace array.</param>
            /// <param name="iwork">A workspace array.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>
            void driver_dsyevd(LapackEigenvalues.SymmetricGeneralJob job, int n, Span<double> a, Span<double> w, Span<double> work, int[] iwork, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Gets a optimal workspace array length for the <c>driver_zheevd</c> function.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="n">The order of the Hermitian matrix.</param>
            /// <param name="liwork">The optimal workspace array length for parameter 'work'.</param>
            /// <param name="lwork">The optimal workspace array length for parameter 'iwork'.</param>
            /// <param name="lrwork">The optimal workspace array length for parameter 'lrwork'.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the Hermitian input matrix is stored.</param>        
            /// <remarks>The parameter <paramref name="triangularMatrixType"/> should not have an impact of the calculation of the optimal length of the workspace array.</remarks>
            void driver_zheevdQuery(LapackEigenvalues.SymmetricGeneralJob job, int n, out int lwork, out int liwork, out int lrwork, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Computes all eigenvalues and (optionally) all eigenvectors of a Hermitian matrix using divide and conquer algorithm.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="a">The Hermitian matrix, i.e. containing either upper or lower triangular part of the Hermitian matrix, as specified by <paramref name="triangularMatrixType"/>. The length should be at least <paramref name="n"/>^2; overwritten on exit, perhaps by the orthonormal eigenvectors.</param>
            /// <param name="w">The eigenvalues of <paramref name="a"/> in ascending order, i.e. the array should have at least <paramref name="n"/> elements (output).</param>
            /// <param name="work">A workspace array.</param>
            /// <param name="rwork">A workspace array.</param>
            /// <param name="iwork">A workspace array.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>
            void driver_zheevd(LapackEigenvalues.SymmetricGeneralJob job, int n, Span<Complex> a, Span<double> w, Span<Complex> work, Span<double> rwork, int[] iwork, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Gets a optimal workspace array length for the <c>driver_dsyevx</c> function.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="range">A value indicating which eigenvalues to compute.</param>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="vl">The lower bound of the interval to be searched for eigenvalues.</param>
            /// <param name="vu">The upper bound of the interval to be searched for eigenvalues.</param>
            /// <param name="il">The lower index of the smallest and largest eigenvalues to be returned.</param>
            /// <param name="iu">The upper index of the smallest and largest eigenvalues to be returned.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>
            /// <param name="abstol">The absolute error tolerance for the eigenvalues.</param>
            /// <returns>The optimal workspace array length.</returns>
            /// <remarks>The parameter <paramref name="triangularMatrixType"/> should not have an impact of the calculation of the optimal length of the workspace array.</remarks>
            int driver_dsyevxQuery(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, double vl, double vu, int il, int iu, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, double abstol = MachineConsts.Epsilon);

            /// <summary>Computes selected eigenvalues and eigenvectors of a real symmetric tridiagonal matrix. The spectrum may be computed either completely or partially by specifying either an interval (<paramref name="vl"/>,<paramref name="vu"/>] or a range of indices <paramref name="il"/>:<paramref name="iu"/> for the desired eigenvalues.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="range">A value indicating which eigenvalues to compute.</param>
            /// <param name="n">The order of the tridiagonal matrix.</param>
            /// <param name="a">The symmetric matrix, i.e. containing either upper or lower triangular part of the symmetric matrix, as specified by <paramref name="triangularMatrixType"/>. The length should be at least <paramref name="n"/>^2; overwritten on exit.</param>
            /// <param name="vl">The lower bound of the interval to be searched for eigenvalues.</param>
            /// <param name="vu">The upper bound of the interval to be searched for eigenvalues.</param>
            /// <param name="il">The lower index of the smallest and largest eigenvalues to be returned.</param>
            /// <param name="iu">The upper index of the smallest and largest eigenvalues to be returned.</param>
            /// <param name="m">The total number of eigenvalues found (output).</param>
            /// <param name="w">The first <paramref name="m"/> values contain the selected eigenvalues in ascending order; this array should have a length of at least <paramref name="n"/> (output).</param>
            /// <param name="z">If <paramref name="job"/> indicates to compute eigenvectors, the first <paramref name="m"/> columns of matrix Z contain the orthonormal eigenvectors (output).</param>
            /// <param name="ifail">Contains the indices of the eigenvectors that failed to converge (if applictable); array should have at least <paramref name="n"/> elements (output).</param>
            /// <param name="work">A workspace array with at least 8 * <paramref name="n"/> elements.</param>
            /// <param name="iwork">A workspace array with at least 5 * <paramref name="n"/> elements.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>
            /// <param name="abstol">The absolute error tolerance for the eigenvalues.</param>
            void driver_dsyevx(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, Span<double> a, double vl, double vu, int il, int iu, out int m, Span<double> w, Span<double> z, int[] ifail, Span<double> work, int[] iwork, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, double abstol = MachineConsts.Epsilon);

            /// <summary>Gets a optimal workspace array length for the <c>driver_zheevx</c> function.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="range">A value indicating which eigenvalues to compute.</param>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="vl">The lower bound of the interval to be searched for eigenvalues.</param>
            /// <param name="vu">The upper bound of the interval to be searched for eigenvalues.</param>
            /// <param name="il">The lower index of the smallest and largest eigenvalues to be returned.</param>
            /// <param name="iu">The upper index of the smallest and largest eigenvalues to be returned.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>
            /// <param name="abstol">The absolute error tolerance for the eigenvalues.</param>
            /// <returns>The optimal workspace array length.</returns>
            /// <remarks>The parameter <paramref name="triangularMatrixType"/> should not have an impact of the calculation of the optimal length of the workspace array.</remarks>
            int driver_zheevxQuery(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, double vl, double vu, int il, int iu, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, double abstol = MachineConsts.Epsilon);

            /// <summary>Computes selected eigenvalues and eigenvectors of a Hermitian matrix. The spectrum may be computed either completely or partially by specifying either an interval (<paramref name="vl"/>,<paramref name="vu"/>] or a range of indices <paramref name="il"/>:<paramref name="iu"/> for the desired eigenvalues.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="range">A value indicating which eigenvalues to compute.</param>
            /// <param name="n">The order of the tridiagonal matrix.</param>
            /// <param name="a">The symmetric matrix, i.e. containing either upper or lower triangular part of the symmetric matrix, as specified by <paramref name="triangularMatrixType"/>. The length should be at least <paramref name="n"/>^2; overwritten on exit.</param>
            /// <param name="vl">The lower bound of the interval to be searched for eigenvalues.</param>
            /// <param name="vu">The upper bound of the interval to be searched for eigenvalues.</param>
            /// <param name="il">The lower index of the smallest and largest eigenvalues to be returned.</param>
            /// <param name="iu">The upper index of the smallest and largest eigenvalues to be returned.</param>
            /// <param name="m">The total number of eigenvalues found (output).</param>
            /// <param name="w">The first <paramref name="m"/> values contain the selected eigenvalues in ascending order; this array should have a length of at least <paramref name="n"/> (output).</param>
            /// <param name="z">If <paramref name="job"/> indicates to compute eigenvectors, the first <paramref name="m"/> columns of matrix Z contain the orthonormal eigenvectors (output).</param>
            /// <param name="ifail">Contains the indices of the eigenvectors that failed to converge (if applictable); array should have at least <paramref name="n"/> elements (output).</param>
            /// <param name="work">A workspace array with at least 2 * <paramref name="n"/> elements.</param>
            /// <param name="rwork">A workspace array with at least 7 * <paramref name="n"/> elements.</param>
            /// <param name="iwork">A workspace array with at least 5 * <paramref name="n"/> elements.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>
            /// <param name="abstol">The absolute error tolerance for the eigenvalues.</param>
            void driver_zheevx(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, Span<Complex> a, double vl, double vu, int il, int iu, out int m, Span<double> w, Span<Complex> z, int[] ifail, Span<Complex> work, Span<double> rwork, int[] iwork, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, double abstol = MachineConsts.Epsilon);

            /// <summary>Gets a optimal workspace array length for the <c>driver_dsyevr</c> function.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="range">A value indicating which eigenvalues to compute.</param>
            /// <param name="n">The order of the tridiagonal matrix.</param>
            /// <param name="vl">The lower bound of the interval to be searched for eigenvalues.</param>
            /// <param name="vu">The upper bound of the interval to be searched for eigenvalues.</param>
            /// <param name="il">The lower index of the smallest and largest eigenvalues to be returned.</param>
            /// <param name="iu">The upper index of the smallest and largest eigenvalues to be returned.</param>
            /// <param name="liwork">The optimal workspace array length for parameter 'work'.</param>
            /// <param name="lwork">The optimal workspace array length for parameter 'iwork'.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the Hermitian input matrix is stored.</param>        
            /// <param name="abstol">The absolute error tolerance for the eigenvalues.</param>
            /// <remarks>The parameter <paramref name="triangularMatrixType"/> should not have an impact of the calculation of the optimal length of the workspace array.</remarks>
            void driver_dsyevrQuery(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, double vl, double vu, int il, int iu, out int lwork, out int liwork, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, double abstol = MachineConsts.Epsilon);

            /// <summary>Computes selected eigenvalues and, optionally, eigenvectors of a real symmetric matrix using the Relatively Robust Representations. The spectrum may be computed either completely or partially by specifying either an interval (<paramref name="vl"/>,<paramref name="vu"/>] or a range of indices <paramref name="il"/>:<paramref name="iu"/> for the desired eigenvalues.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="range">A value indicating which eigenvalues to compute.</param>
            /// <param name="n">The order of the tridiagonal matrix.</param>
            /// <param name="a">The symmetric matrix, i.e. containing either upper or lower triangular part of the symmetric matrix, as specified by <paramref name="triangularMatrixType"/>. The length should be at least <paramref name="n"/>^2; overwritten on exit.</param>
            /// <param name="vl">The lower bound of the interval to be searched for eigenvalues.</param>
            /// <param name="vu">The upper bound of the interval to be searched for eigenvalues.</param>
            /// <param name="il">The lower index of the smallest and largest eigenvalues to be returned.</param>
            /// <param name="iu">The upper index of the smallest and largest eigenvalues to be returned.</param>
            /// <param name="m">The total number of eigenvalues found (output).</param>
            /// <param name="w">The first <paramref name="m"/> values contain the selected eigenvalues in ascending order; this array should have a length of at least <paramref name="n"/> (output).</param>
            /// <param name="z">If <paramref name="job"/> indicates to compute eigenvectors, the first <paramref name="m"/> columns of matrix Z contain the orthonormal eigenvectors.</param>
            /// <param name="isuppz">The support of the eigenvectors in matrix Z, that is the indices indicating the nonzero elements in z (output).</param>
            /// <param name="work">A workspace array with at least 26 * <paramref name="n"/> elements.</param>
            /// <param name="iwork">A workspace array with at least 10 * <paramref name="n"/> elements.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>
            /// <param name="abstol">The absolute error tolerance for the eigenvalues.</param>
            void driver_dsyevr(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, Span<double> a, double vl, double vu, int il, int iu, out int m, Span<double> w, Span<double> z, int[] isuppz, Span<double> work, int[] iwork, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, double abstol = MachineConsts.Epsilon);

            /// <summary>Gets a optimal workspace array length for the <c>driver_zheevr</c> function.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="range">A value indicating which eigenvalues to compute.</param>
            /// <param name="n">The order of the tridiagonal matrix.</param>
            /// <param name="vl">The lower bound of the interval to be searched for eigenvalues.</param>
            /// <param name="vu">The upper bound of the interval to be searched for eigenvalues.</param>
            /// <param name="il">The lower index of the smallest and largest eigenvalues to be returned.</param>
            /// <param name="iu">The upper index of the smallest and largest eigenvalues to be returned.</param>
            /// <param name="liwork">The optimal workspace array length for parameter 'work'.</param>
            /// <param name="lrwork">The optimal workspace array length for parameter 'rwork'.</param>
            /// <param name="lwork">The optimal workspace array length for parameter 'iwork'.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the Hermitian input matrix is stored.</param>        
            /// <param name="abstol">The absolute error tolerance for the eigenvalues.</param>
            /// <remarks>The parameter <paramref name="triangularMatrixType"/> should not have an impact of the calculation of the optimal length of the workspace array.</remarks>
            void driver_zheevrQuery(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, double vl, double vu, int il, int iu, out int lwork, out int lrwork, out int liwork, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, double abstol = MachineConsts.Epsilon);

            /// <summary>Computes selected eigenvalues and, optionally, eigenvectors of a Hermitian matrix using the Relatively Robust Representations. The spectrum may be computed either completely or partially by specifying either an interval (<paramref name="vl"/>,<paramref name="vu"/>] or a range of indices <paramref name="il"/>:<paramref name="iu"/> for the desired eigenvalues.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="range">A value indicating which eigenvalues to compute.</param>
            /// <param name="n">The order of the Hermitian matrix.</param>
            /// <param name="a">The symmetric matrix, i.e. containing either upper or lower triangular part of the symmetric matrix, as specified by <paramref name="triangularMatrixType"/>. The length should be at least <paramref name="n"/>^2; overwritten on exit.</param>
            /// <param name="vl">The lower bound of the interval to be searched for eigenvalues.</param>
            /// <param name="vu">The upper bound of the interval to be searched for eigenvalues.</param>
            /// <param name="il">The lower index of the smallest and largest eigenvalues to be returned.</param>
            /// <param name="iu">The upper index of the smallest and largest eigenvalues to be returned.</param>
            /// <param name="m">The total number of eigenvalues found (output).</param>
            /// <param name="w">The first <paramref name="m"/> values contain the selected eigenvalues in ascending order; this array should have a length of at least <paramref name="n"/> (output).</param>
            /// <param name="z">If <paramref name="job"/> indicates to compute eigenvectors, the first <paramref name="m"/> columns of matrix Z contain the orthonormal eigenvectors.</param>
            /// <param name="isuppz">The support of the eigenvectors in matrix Z, that is the indices indicating the nonzero elements in z (output).</param>
            /// <param name="work">A workspace array with at least 2 * <paramref name="n"/> elements.</param>
            /// <param name="rwork">A workspace array with at least 24 * <paramref name="n"/> elements.</param>
            /// <param name="iwork">A workspace array with at least 10 * <paramref name="n"/> elements.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>
            /// <param name="abstol">The absolute error tolerance for the eigenvalues.</param>
            void driver_zheevr(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, Span<Complex> a, double vl, double vu, int il, int iu, out int m, Span<double> w, Span<Complex> z, int[] isuppz, Span<Complex> work, Span<double> rwork, int[] iwork, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, double abstol = MachineConsts.Epsilon);

            /// <summary>Computes all eigenvalues and, optionally, eigenvectors of a real symmetric matrix in packed storage.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="n">The order of the specified symmetric matrix.</param>
            /// <param name="ap">The specified symmetric matrix in packed form, i.e. either upper or lower triangle as specified in <paramref name="triangularMatrixType"/> with at least <paramref name="n"/> * (<paramref name="n"/> + 1) / 2 elements.</param>
            /// <param name="w">The eigenvalues of <paramref name="ap"/> in ascending order, i.e. the array should have at least <paramref name="n"/> elements (output).</param>
            /// <param name="z">If <paramref name="job"/> indicates to compute eigenvectors, this parameter contains the orthonormal eigenvectors (output).</param>
            /// <param name="work">A workspace array with at least 3 * <paramref name="n"/> elements.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>
            void driver_dspev(LapackEigenvalues.SymmetricGeneralJob job, int n, Span<double> ap, Span<double> w, Span<double> z, Span<double> work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

            /// <summary>Computes all eigenvalues and, optionally, eigenvectors of a Hermitian matrix in packed storage.
            /// </summary>
            /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
            /// <param name="n">The order of the specified Hermitian matrix.</param>
            /// <param name="ap">The specified symmetric matrix in packed form, i.e. either upper or lower triangle as specified in <paramref name="triangularMatrixType"/> with at least <paramref name="n"/> * (<paramref name="n"/> + 1) / 2 elements.</param>
            /// <param name="w">The eigenvalues of <paramref name="ap"/> in ascending order, i.e. the array should have at least <paramref name="n"/> elements (output).</param>
            /// <param name="z">If <paramref name="job"/> indicates to compute eigenvectors, this parameter contains the orthonormal eigenvectors (output).</param>
            /// <param name="work">A workspace array with at least 2 * <paramref name="n"/> - 1 elements.</param>
            /// <param name="rwork">A workspace array with at least 3 * <paramref name="n"/> - 2 elements.</param>
            /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the Hermitian input matrix is stored.</param>
            void driver_zhpev(LapackEigenvalues.SymmetricGeneralJob job, int n, Span<Complex> ap, Span<double> w, Span<Complex> z, Span<Complex> work, Span<double> rwork, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);
#pragma warning restore IDE1006 // Naming Styles
        }
    }
}