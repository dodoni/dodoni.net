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
using System.Security;
using System.Runtime.InteropServices;

namespace Dodoni.MathLibrary.Basics.LowLevel.Native
{
    public static partial class MklExtensions
    {
        #region private function import

        [DllImport(sm_DllName, EntryPoint = "DDTTRSB", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _ddttrsb(ref char trans, ref int n, ref int nrhs, [In, Out] double[] dl, [In, Out] double[] d, [In, Out] double[] du, [In, Out] double[] b, ref int ldb, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZDTTRSB", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zdttrsb(ref char trans, ref int n, ref int nrhs, [In, Out] Complex[] dl, [In, Out] Complex[] d, [In, Out] Complex[] du, [In, Out] Complex[] b, ref int ldb, out int info);
        #endregion

        #region public (static) methods

        /// <summary>Solves a system of linear equations with a diagonally dominant tridiagonal matrix using the LU factorization computed by <c>ddttrfb</c>, i.e. op(A) * X = B.
        /// </summary>
        /// <param name="solver">The <see cref="LapackLinearEquations.ISolver"/> object.</param>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="dl">The <paramref name="n"/> -1 multipliers that define the matrices L_1, L_2 from the factorization of A.</param>
        /// <param name="d">The <paramref name="n"/> diagonal elements of the upper triangular matrix U from the factorization of A.</param>
        /// <param name="du">The <paramref name="n"/> - 1 elements of the superdiagonal of U.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="transposeState">A value indicating whether op(A) is matrix A or its transposed.</param>
        public static void ddttrsb(this LapackLinearEquations.ISolver solver, int n, double[] dl, double[] d, double[] du, double[] b, int nrhs, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            int info;
            var trans = GetTrans(transposeState);

            _ddttrsb(ref trans, ref n, ref nrhs, dl, d, du, b, ref n, out info);
            CheckForError(info, "ddttrsb");
        }

        /// <summary>Solves a system of linear equations with a diagonally dominant tridiagonal matrix using the LU factorization computed by <c>zdttrfb</c>, i.e. op(A) * X = B.
        /// </summary>
        /// <param name="solver">The <see cref="LapackLinearEquations.ISolver"/> object.</param>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="nrhs">The number of right-hand sides.</param>
        /// <param name="dl">The <paramref name="n"/> -1 multipliers that define the matrices L_1, L_2 from the factorization of A.</param>
        /// <param name="d">The <paramref name="n"/> diagonal elements of the upper triangular matrix U from the factorization of A.</param>
        /// <param name="du">The <paramref name="n"/> - 1 elements of the superdiagonal of U.</param>
        /// <param name="b">Contains matrix B whose columns are the right-hand side for the systems of equations; overwritten by the solution matrix X on exit (output).</param>
        /// <param name="transposeState">A value indicating whether op(A) is matrix A, its transposed or its Hermitian.</param>
        public static void zdttrsb(this LapackLinearEquations.ISolver solver, int n, Complex[] dl, Complex[] d, Complex[] du, Complex[] b, int nrhs, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            int info;
            var trans = GetTrans(transposeState);

            _zdttrsb(ref trans, ref n, ref nrhs, dl, d, du, b, ref n, out info);
            CheckForError(info, "zdttrsb");
        }
        #endregion
    }
}