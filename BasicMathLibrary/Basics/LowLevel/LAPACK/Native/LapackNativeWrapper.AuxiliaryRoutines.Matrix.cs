/*
   Copyright (c) 2011-2018 Markus Wendt

 This software is provided 'as-is', without any express or implied warranty. In no event will the authors be held liable for 
 any damages arising from the use of this software. 

 Permission is granted to anyone to use this software for any purpose, including commercial applications, and to alter it and 
 redistribute it freely, subject to the following restrictions: 
   1.The origin of this software must not be misrepresented; you must not claim that you wrote the original software. If you 
     use this software in a product, an acknowledgment in the product documentation would be appreciated but is not required.
   2.Altered source versions must be plainly marked as such, and must not be misrepresented as being the original software.
   3.This notice may not be removed or altered from any source distribution.

 Please see http://www.dodoni-project.net/ for more information concerning the Dodoni.net project.
 */
using System;
using System.Security;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Dodoni.MathLibrary.Basics.LowLevel.Native
{
    internal partial class LapackNativeWrapper : LapackAuxiliaryUtilityRoutines.IMatrix
    {
        #region private function import

        [DllImport(sm_DllName, EntryPoint = "DLANGE", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern double _dlange(ref char norm, ref int m, ref int n, [In, Out] double[] a, ref int lda, [In, Out] double[] work);

        [DllImport(sm_DllName, EntryPoint = "ZLANGE", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern double _zlange(ref char norm, ref int m, ref int n, [In, Out] Complex[] a, ref int lda, [In, Out] Complex[] work);

        [DllImport(sm_DllName, EntryPoint = "DLANSP", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern double _dlansp(ref char norm, ref char uplo, ref int n, [In, Out] double[] ap, [In, Out] double[] work);

        [DllImport(sm_DllName, EntryPoint = "ZLANSP", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern double _zlansp(ref char norm, ref char uplo, ref int n, [In, Out] Complex[] ap, [In, Out] Complex[] work);

        [DllImport(sm_DllName, EntryPoint = "DLANGB", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern double _dlangb(ref char norm, ref int n, ref int kl, ref int ku, [In, Out] double[] ab, ref int ldab, [In, Out] double[] work);

        [DllImport(sm_DllName, EntryPoint = "ZLANGB", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern double _zlangb(ref char norm, ref int n, ref int kl, ref int ku, [In, Out] Complex[] ab, ref int ldab, [In, Out] Complex[] work);
        #endregion

        #region public methods

        /// <summary>Returns the value of the 1-norm, Frobenius norm, infinity-norm, or the largest absolute value of any element of general rectangular matrix.
        /// </summary>
        /// <param name="matrixNormType">The type of the matrix norm.</param>
        /// <param name="m">Th enumber of rows.</param>
        /// <param name="n">The number of columns.</param>
        /// <param name="a">The <paramref name="m"/>-by-<paramref name="n"/> dense matrix provided column-by-column.</param>
        /// <param name="work">A workspace array which is referenced in the case of infinity norm only. In this case the length must be at least <paramref name="m"/>.</param>
        /// <returns>The value of the specific matrix norm.</returns>
        public double dlange(MatrixNormType matrixNormType, int m, int n, double[] a, double[] work)
        {
            var norm = LAPACK.GetMatrixNormType(matrixNormType);
            int lda = Math.Max(1, m);

            return _dlange(ref norm, ref m, ref n, a, ref lda, work);
        }

        /// <summary>Returns the value of the 1-norm, Frobenius norm, infinity-norm, or the largest absolute value of any element of general rectangular matrix.
        /// </summary>
        /// <param name="matrixNormType">The type of the matrix norm.</param>
        /// <param name="m">Th enumber of rows.</param>
        /// <param name="n">The number of columns.</param>
        /// <param name="a">The <paramref name="m"/>-by-<paramref name="n"/> dense matrix provided column-by-column.</param>
        /// <param name="work">A workspace array which is referenced in the case of infinity norm only. In this case the length must be at least <paramref name="m"/>.</param>
        /// <returns>The value of the specific matrix norm.</returns>
        public double zlange(MatrixNormType matrixNormType, int m, int n, Complex[] a, Complex[] work)
        {
            var norm = LAPACK.GetMatrixNormType(matrixNormType);
            int lda = Math.Max(1, m);

            return _zlange(ref norm, ref m, ref n, a, ref lda, work);
        }

        /// <summary>Returns the value of the 1-norm, Frobenius norm, infinity-norm, or the largest absolute value of any element of symmetric matrix supplied in packed form.
        /// </summary>
        /// <param name="matrixNormType">The type of the matrix norm.</param>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="ap">The specified symmetric matrix in packed form, i.e. either upper or lower triangle as specified in <paramref name="triangularMatrixType"/> with at least <paramref name="n"/> * (<paramref name="n"/> + 1) / 2 elements.</param>
        /// <param name="work">A workspace array which is referenced in the case of 1- or infinity-norm only. In this case the length must be at least <paramref name="n"/>.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>
        /// <returns>The value of the specific matrix norm.</returns>
        public double dlansp(MatrixNormType matrixNormType, int n, double[] ap, double[] work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            var norm = LAPACK.GetMatrixNormType(matrixNormType);
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            return _dlansp(ref norm, ref uplo, ref n, ap, work);
        }

        /// <summary>Returns the value of the 1-norm, Frobenius norm, infinity-norm, or the largest absolute value of any element of symmetric matrix supplied in packed form.
        /// </summary>
        /// <param name="matrixNormType">The type of the matrix norm.</param>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="ap">The specified symmetric matrix in packed form, i.e. either upper or lower triangle as specified in <paramref name="triangularMatrixType"/> with at least <paramref name="n"/> * (<paramref name="n"/> + 1) / 2 elements.</param>
        /// <param name="work">A workspace array which is referenced in the case of 1- or infinity-norm only. In this case the length must be at least <paramref name="n"/>.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>
        /// <returns>The value of the specific matrix norm.</returns>
        public double zlansp(MatrixNormType matrixNormType, int n, Complex[] ap, Complex[] work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            var norm = LAPACK.GetMatrixNormType(matrixNormType);
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            return _zlansp(ref norm, ref uplo, ref n, ap, work);
        }

        /// <summary>Returns the value of the 1-norm, Frobenius norm, infinity-norm, or the largest absolute value of any element of general band matrix.
        /// </summary>
        /// <param name="matrixNormType">The type of the matrix norm.</param>
        /// <param name="n">The order of the quadratic general band matrix.</param>
        /// <param name="kl">The number of sub-diagonals of the specific general band matrix.</param>
        /// <param name="ku">The number of super-diagonals of the specific general band matrix.</param>
        /// <param name="a">The general band matrix stored in general band matrix storage, i.e. column-by-column, where each column contains exactly <paramref name="kl" /> + <paramref name="ku" /> + 1 elements.</param>
        /// <param name="work">A workspace array which is referenced in the case of infinity norm only. In this case the length must be at least <paramref name="n" />.</param>
        /// <returns>The value of the specific matrix norm.</returns>
        public double dlangb(MatrixNormType matrixNormType, int n, int kl, int ku, double[] a, double[] work)
        {
            var norm = LAPACK.GetMatrixNormType(matrixNormType);
            int ldab = kl + ku + 1;

            return _dlangb(ref norm, ref n, ref kl, ref ku, a, ref ldab, work);
        }

        /// <summary>Returns the value of the 1-norm, Frobenius norm, infinity-norm, or the largest absolute value of any element of general band matrix.
        /// </summary>
        /// <param name="matrixNormType">The type of the matrix norm.</param>
        /// <param name="n">The order of the quadratic general band matrix.</param>
        /// <param name="kl">The number of sub-diagonals of the specific general band matrix.</param>
        /// <param name="ku">The number of super-diagonals of the specific general band matrix.</param>
        /// <param name="a">The general band matrix stored in general band matrix storage, i.e. column-by-column, where each column contains exactly <paramref name="kl" /> + <paramref name="ku" /> + 1 elements.</param>
        /// <param name="work">A workspace array which is referenced in the case of infinity norm only. In this case the length must be at least <paramref name="n" />.</param>
        /// <returns>The value of the specific matrix norm.</returns>
        public double zlangb(MatrixNormType matrixNormType, int n, int kl, int ku, Complex[] a, Complex[] work)
        {
            var norm = LAPACK.GetMatrixNormType(matrixNormType);
            int ldab = kl + ku + 1;

            return _zlangb(ref norm, ref n, ref kl, ref ku, a, ref ldab, work);
        }
        #endregion
    }
}