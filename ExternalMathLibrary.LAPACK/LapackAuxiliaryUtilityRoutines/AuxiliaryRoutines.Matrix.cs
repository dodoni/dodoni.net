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
using System.Security;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Dodoni.MathLibrary.Basics.LowLevel.Native
{
    /// <summary>The native wrapper for auxiliary utility routines with respect to Lapack, see http://www.netlib.org/lapack/index.html.
    /// </summary>
    internal partial class AuxiliaryUtilityRoutines : LapackAuxiliaryUtilityRoutines.IMatrix
    {
        #region private function import

#if LOWER_CASE_UNDERSCORE
        [DllImport(LapackNativeWrapper.dllName, EntryPoint = "dlange_", CallingConvention = LapackNativeWrapper.CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern double _dlange(ref char norm, ref int m, ref int n, [In, Out] double[] a, ref int lda, [In, Out] double[] work);

        [DllImport(LapackNativeWrapper.dllName, EntryPoint = "zlange_", CallingConvention = LapackNativeWrapper.CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern double _zlange(ref char norm, ref int m, ref int n, [In, Out] Complex[] a, ref int lda, [In, Out] Complex[] work);

        [DllImport(LapackNativeWrapper.dllName, EntryPoint = "dlansp_", CallingConvention = LapackNativeWrapper.CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern double _dlansp(ref char norm, ref char uplo, ref int n, [In, Out] double[] ap, [In, Out] double[] work);

        [DllImport(LapackNativeWrapper.dllName, EntryPoint = "zlansp_", CallingConvention = LapackNativeWrapper.CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern double _zlansp(ref char norm, ref char uplo, ref int n, [In, Out] Complex[] ap, [In, Out] Complex[] work);

        [DllImport(LapackNativeWrapper.dllName, EntryPoint = "dlangb_", CallingConvention = LapackNativeWrapper.CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern double _dlangb(ref char norm, ref int n, ref int kl, ref int ku, [In, Out] double[] ab, ref int ldab, [In, Out] double[] work);

        [DllImport(LapackNativeWrapper.dllName, EntryPoint = "zlangb_", CallingConvention = LapackNativeWrapper.CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern double _zlangb(ref char norm, ref int n, ref int kl, ref int ku, [In, Out] Complex[] ab, ref int ldab, [In, Out] Complex[] work);
#else
        [DllImport(LapackNativeWrapper.sm_DllName, EntryPoint = "DLANGE", CallingConvention = LapackNativeWrapper.sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern double _dlange(ref char norm, ref int m, ref int n, [In, Out] double[] a, ref int lda, [In, Out] double[] work);

        [DllImport(LapackNativeWrapper.sm_DllName, EntryPoint = "ZLANGE", CallingConvention = LapackNativeWrapper.sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern double _zlange(ref char norm, ref int m, ref int n, [In, Out] Complex[] a, ref int lda, [In, Out] Complex[] work);

        [DllImport(LapackNativeWrapper.sm_DllName, EntryPoint = "DLANSP", CallingConvention = LapackNativeWrapper.sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern double _dlansp(ref char norm, ref char uplo, ref int n, [In, Out] double[] ap, [In, Out] double[] work);

        [DllImport(LapackNativeWrapper.sm_DllName, EntryPoint = "ZLANSP", CallingConvention = LapackNativeWrapper.sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern double _zlansp(ref char norm, ref char uplo, ref int n, [In, Out] Complex[] ap, [In, Out] Complex[] work);

        [DllImport(LapackNativeWrapper.sm_DllName, EntryPoint = "DLANGB", CallingConvention = LapackNativeWrapper.sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern double _dlangb(ref char norm, ref int n, ref int kl, ref int ku, [In, Out] double[] ab, ref int ldab, [In, Out] double[] work);

        [DllImport(LapackNativeWrapper.sm_DllName, EntryPoint = "ZLANGB", CallingConvention = LapackNativeWrapper.sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern double _zlangb(ref char norm, ref int n, ref int kl, ref int ku, [In, Out] Complex[] ab, ref int ldab, [In, Out] Complex[] work);
#endif
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
            var norm = LapackNativeWrapper.GetMatrixNormType(matrixNormType);
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
            var norm = LapackNativeWrapper.GetMatrixNormType(matrixNormType);
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
            var norm = LapackNativeWrapper.GetMatrixNormType(matrixNormType);
            var uplo = LapackNativeWrapper.GetUplo(triangularMatrixType);

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
            var norm = LapackNativeWrapper.GetMatrixNormType(matrixNormType);
            var uplo = LapackNativeWrapper.GetUplo(triangularMatrixType);

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
            var norm = LapackNativeWrapper.GetMatrixNormType(matrixNormType);
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
            var norm = LapackNativeWrapper.GetMatrixNormType(matrixNormType);
            int ldab = kl + ku + 1;

            return _zlangb(ref norm, ref n, ref kl, ref ku, a, ref ldab, work);
        }
        #endregion

        #region internal static methods

        /// <summary>Creates a new <see cref="AuxiliaryUtilityRoutines"/> instance.
        /// </summary>
        /// <returns>A new <see cref="AuxiliaryUtilityRoutines"/> instance.</returns>
        internal static AuxiliaryUtilityRoutines Create()
        {
            return new AuxiliaryUtilityRoutines();
        }
        #endregion
    }
}