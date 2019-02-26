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

using Dodoni.MathLibrary.Basics.LowLevel;

namespace Dodoni.MathLibrary.Basics
{
    public static partial class Extensions
    {
#pragma warning disable IDE1006 // Naming Styles

        /// <summary>Computes all eigenvalues and, optionally, eigenvectors of a real symmetric matrix using the Relatively Robust Representations.
        /// </summary>
        /// <param name="symmetricEigenvalueProblems">The <see cref="LapackEigenvalues.ISymmetricEigenvalueProblems"/> object.</param>
        /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="n">The order of the tridiagonal matrix.</param>
        /// <param name="a">The symmetric matrix, i.e. containing either upper or lower triangular part of the symmetric matrix, as specified by <paramref name="triangularMatrixType"/>. The length should be at least <paramref name="n"/>^2; overwritten on exit.</param>
        /// <param name="m">The total number of eigenvalues found (output).</param>
        /// <param name="w">The first <paramref name="m"/> values contain the selected eigenvalues in ascending order; this array should have a length of at least <paramref name="n"/> (output).</param>
        /// <param name="z">If <paramref name="job"/> indicates to compute eigenvectors, the first <paramref name="m"/> columns of matrix Z contain the orthonormal eigenvectors.</param>
        /// <param name="isuppz">The support of the eigenvectors in matrix Z, that is the indices indicating the nonzero elements in z (output).</param>
        /// <param name="work">A workspace array with at least 26 * <paramref name="n"/> elements.</param>
        /// <param name="iwork">A workspace array with at least 10 * <paramref name="n"/> elements.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>
        /// <param name="abstol">The absolute error tolerance for the eigenvalues.</param>
        public static void driver_dsyevr(this LapackEigenvalues.ISymmetricEigenvalueProblems symmetricEigenvalueProblems, LapackEigenvalues.SymmetricGeneralJob job, int n, double[] a, out int m, double[] w, double[] z, int[] isuppz, double[] work, int[] iwork, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, double abstol = MachineConsts.Epsilon)
        {
            symmetricEigenvalueProblems.driver_dsyevr(job, LapackEigenvalues.SymmetricEigenvaluesRange.All, n, a, 0, 0, 0, 0, out m, w, z, isuppz, work, iwork, triangularMatrixType, abstol);
        }

        /// <summary>Gets a optimal workspace array length for the <c>driver_dsyevr</c> function.
        /// </summary>
        /// <param name="symmetricEigenvalueProblems">The <see cref="LapackEigenvalues.ISymmetricEigenvalueProblems"/> object.</param>
        /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="n">The order of the tridiagonal matrix.</param>
        /// <param name="liwork">The optimal workspace array length for parameter 'work'.</param>
        /// <param name="lwork">The optimal workspace array length for parameter 'iwork'.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the Hermitian input matrix is stored.</param>        
        /// <param name="abstol">The absolute error tolerance for the eigenvalues.</param>
        /// <remarks>The parameter <paramref name="triangularMatrixType"/> should not have an impact of the calculation of the optimal length of the workspace array.</remarks>
        public static void driver_dsyevrQuery(this LapackEigenvalues.ISymmetricEigenvalueProblems symmetricEigenvalueProblems, LapackEigenvalues.SymmetricGeneralJob job, int n, out int lwork, out int liwork, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, double abstol = MachineConsts.Epsilon)
        {
            symmetricEigenvalueProblems.driver_dsyevrQuery(job, LapackEigenvalues.SymmetricEigenvaluesRange.All, n, 0, 0, 0, 0, out lwork, out liwork, triangularMatrixType, abstol);
        }

        /// <summary>Compute a singular value decomposition of a general band matrix, i.e. A = U * \Sigma *V', where the diagonal elements of \Sigma are the singular values in descending order.
        /// </summary>
        /// <param name="singularValueDecomposition">The <see cref="LapackEigenvalues.ISingularValueDecomposition"/> object.</param>
        /// <param name="computeSingularVectors">If set to <c>true</c> the left/right singular vectors will be computed as well.</param>
        /// <param name="m">The number of rows of the general band matrix.</param>
        /// <param name="n">The number of columns of the general band matrix.</param>
        /// <param name="kl">The number of sub-diagonals of the general band matrix.</param>
        /// <param name="ku">The number of super-diagonals of the general band matrix.</param>
        /// <param name="a">The general band matrix, matrix entries are provided columnwise.</param>
        /// <param name="s">The singular values in descending order; at least min(<paramref name="n"/>, <paramref name="m"/>) elements (output).</param>
        /// <param name="u">The left singular vectors U, i.e. a m x m unitary matrix (output, if <paramref name="computeSingularVectors"/> is <c>true</c>).</param>
        /// <param name="vt">The right singular vectors V', i.e. a n x n unitary matrix (output, if <paramref name="computeSingularVectors"/> is <c>true</c>).</param>
        /// <remarks>The singular values are the roots of the non-negative eigenvalues of A^t*A.</remarks>
        public static void dgbsvd(this LapackEigenvalues.ISingularValueDecomposition singularValueDecomposition, bool computeSingularVectors, int m, int n, int kl, int ku, double[] a, double[] s, double[] u, double[] vt)
        {
            // todo: Wozu nötig?
            throw new NotImplementedException();
            //int ncvt, nru;
            //char vect;

            //if (computeSingularVectors)
            //{
            //    ncvt = n;
            //    nru = m;
            //    vect = 'B';
            //}
            //else
            //{
            //    ncvt = nru = 0;
            //    vect = 'N';
            //}
            /* reduces the general band matrix to bidiagonal form, i.e.
             *           A = Q * B * P^H, where Q,P orthogonal: 
             * The value of 'vect' indicates whether Q and P^H will be compute. 
             */

            //int ncc = 0;   // do not generate C = Q^H*c
            //int ldc = 1;   // dummy needed for ncc = 0

            //int ldab = ku + kl + 1;

            //// working array and off-diagonal elements are used for both MKL functions,
            //// therefore we adjust the minimum size for both:
            //double[] work = new double[2 * Math.Max(n, m) + 2 * n];
            //double[] e = new double[Math.Max(n, m)]; // off-diagonal elements of B

            //singularValueDecomposition.dgbbrd(vect, m, n, ncc, kl, ku, a, ldab, s, e, u, m, vt, n, null, ldc, work);
            //singularValueDecomposition.dbdsqr(LAPACK.BidiagonalMatrixType.UpperBidiagonalMatrix, n, ncvt, nru, ncc, s, e, vt, n, u, m, null, ldc, work);
        }
#pragma warning restore IDE1006 // Naming Styles
    }
}