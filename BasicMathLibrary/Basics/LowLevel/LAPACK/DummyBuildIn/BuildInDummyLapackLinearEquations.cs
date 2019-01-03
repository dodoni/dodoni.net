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

using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.Basics.LowLevel
{
    /// <summary>A dummy implementation for LAPACK routines.
    /// </summary>
    /// <seealso cref="Dodoni.MathLibrary.Basics.LowLevel.LapackLinearEquations.IMatrixConditionalNumbers" />
    /// <seealso cref="Dodoni.MathLibrary.Basics.LowLevel.LapackLinearEquations.IMatrixEquilibration" />
    /// <seealso cref="Dodoni.MathLibrary.Basics.LowLevel.LapackLinearEquations.IMatrixFactorization" />
    /// <seealso cref="Dodoni.MathLibrary.Basics.LowLevel.LapackLinearEquations.IMatrixInversion" />
    /// <seealso cref="Dodoni.MathLibrary.Basics.LowLevel.LapackLinearEquations.ISolver" />
    /// <seealso cref="Dodoni.MathLibrary.Basics.LowLevel.LapackLinearEquations.IErrorEstimationSolver" />
    internal class BuildInDummyLapackLinearEquations :
        LapackLinearEquations.IMatrixConditionalNumbers,
        LapackLinearEquations.IMatrixEquilibration,
        LapackLinearEquations.IMatrixFactorization,
        LapackLinearEquations.IMatrixInversion,
        LapackLinearEquations.ISolver,
        LapackLinearEquations.IErrorEstimationSolver
    {
        public double dgbcon(int n, int kl, int ku, double[] a, double normOfOriginalMatrix, double[] work, LapackLinearEquations.MatrixConditionNormType matrixNormType = LapackLinearEquations.MatrixConditionNormType.Infinity)
        {
            throw new NotImplementedException();
        }

        public void dgbtrf(int m, int n, int kl, int ku, double[] a, int[] iPivot)
        {
            throw new NotImplementedException();
        }

        public void dgbtrs(int n, int kl, int ku, double[] ab, int[] ipiv, double[] b, int nrhs = 1, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }

        public double dgecon(int n, double[] a, double normOfOriginalMatrix, double[] work, LapackLinearEquations.MatrixConditionNormType matrixNormType = LapackLinearEquations.MatrixConditionNormType.Infinity)
        {
            throw new NotImplementedException();
        }

        public void dgetrf(int m, int n, double[] a, int[] iPivot)
        {
            throw new NotImplementedException();
        }

        public void dgetrs(int n, double[] a, int[] ipiv, double[] b, int nrhs = 1, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }

        public double dgtcon(int n, double[] dl, double[] d, double[] du, double[] du2, int[] iPivot, double normOfOriginalMatrix, double[] work, LapackLinearEquations.MatrixConditionNormType matrixNormType = LapackLinearEquations.MatrixConditionNormType.Infinity)
        {
            throw new NotImplementedException();
        }

        public void dgttrf(int n, double[] dl, double[] d, double[] du, double[] du2, int[] iPivot)
        {
            throw new NotImplementedException();
        }

        public void dgttrs(int n, double[] dl, double[] d, double[] du, double[] du2, int[] ipiv, double[] b, int nrhs = 1, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }

        public void dpbtrf(int n, int kd, double[] a, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void dpbtrs(int n, int kd, double[] ab, double[] b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void dpftrf(int n, double[] a, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }

        public void dpftrs(int n, double[] a, double[] b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }

        public void dpotrf(int n, double[] a, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void dpotrs(int n, double[] a, double[] b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void dpptrf(int n, double[] aPacked, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void dpptrs(int n, double[] ap, double[] b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void dpttrf(int n, double[] diagonalElements, double[] e)
        {
            throw new NotImplementedException();
        }

        public void dpttrs(int n, double[] d, double[] e, double[] b, int nrhs = 1)
        {
            throw new NotImplementedException();
        }

        public void driver_dgbsv(int n, int kl, int ku, double[] ab, int[] ipiv, double[] b, int nrhs = 1)
        {
            throw new NotImplementedException();
        }

        public void driver_dgesv(int n, double[] a, int[] ipiv, double[] b, int nrhs = 1)
        {
            throw new NotImplementedException();
        }

        public void driver_dgtsv(int n, double[] dl, double[] d, double[] du, double[] b, int nrhs = 1)
        {
            throw new NotImplementedException();
        }

        public void driver_dpbsv(int n, int kd, double[] ab, double[] b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void driver_dposv(int n, double[] a, double[] b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void driver_dppsv(int n, double[] ap, double[] b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void driver_dptsv(int n, double[] d, double[] e, double[] b, int nrhs = 1)
        {
            throw new NotImplementedException();
        }

        public void driver_dspsv(int n, double[] ap, int[] ipiv, double[] b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void driver_dsysv(int n, double[] a, int[] ipiv, double[] b, double[] work, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public int driver_dsysvQuery(int n, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void driver_zgbsv(int n, int kl, int ku, Complex[] ab, int[] ipiv, Complex[] b, int nrhs = 1)
        {
            throw new NotImplementedException();
        }

        public void driver_zgesv(int n, Complex[] a, int[] ipiv, Complex[] b, int nrhs = 1)
        {
            throw new NotImplementedException();
        }

        public void driver_zgtsv(int n, Complex[] dl, Complex[] d, Complex[] du, Complex[] b, int nrhs = 1)
        {
            throw new NotImplementedException();
        }

        public void driver_zhesv(int n, Complex[] a, int[] ipiv, Complex[] b, Complex[] work, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public int driver_zhesvQuery(int n, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void driver_zhpsv(int n, Complex[] ap, int[] ipiv, Complex[] b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void driver_zpbsv(int n, int kd, Complex[] ab, Complex[] b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void driver_zposv(int n, Complex[] a, Complex[] b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void driver_zppsv(int n, Complex[] ap, Complex[] b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void driver_zptsv(int n, double[] d, Complex[] e, Complex[] b, int nrhs = 1)
        {
            throw new NotImplementedException();
        }

        public void driver_zspsv(int n, Complex[] ap, int[] ipiv, Complex[] b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void driver_zsysv(int n, Complex[] a, int[] ipiv, Complex[] b, Complex[] work, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public int driver_zsysvQuery(int n, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void dsptrf(int n, double[] aPacked, int[] iPivot, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void dsptrs(int n, double[] ap, int[] ipiv, double[] b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void dsytrf(int n, double[] a, int[] iPivot, double[] work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public int dsytrfQuery(int n, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void dsytrs(int n, double[] a, int[] ipiv, double[] b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void dsytrs2(int n, double[] a, int[] ipiv, double[] b, double[] work, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void dtbtrs(int n, int kd, double[] ab, double[] b, int nrhs = 1, bool isUnitTriangularMatrix = true, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }

        public void dtptrs(int n, double[] ap, double[] b, int nrhs = 1, bool isUnitTriangularMatrix = true, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }

        public void dtrtrs(int n, double[] a, double[] b, int nrhs = 1, bool isUnitTriangularMatrix = true, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }

        public void zgbtrf(int m, int n, int kl, int ku, Complex[] a, int[] iPivot)
        {
            throw new NotImplementedException();
        }

        public void zgbtrs(int n, int kl, int ku, Complex[] ab, int[] ipiv, Complex[] b, int nrhs = 1, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }

        public void zgetrf(int m, int n, Complex[] a, int[] iPivot)
        {
            throw new NotImplementedException();
        }

        public void zgetrs(int n, Complex[] a, int[] ipiv, Complex[] b, int nrhs = 1, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }

        public void zgttrf(int n, Complex[] dl, Complex[] d, Complex[] du, Complex[] du2, int[] iPivot)
        {
            throw new NotImplementedException();
        }

        public void zgttrs(int n, Complex[] dl, Complex[] d, Complex[] du, Complex[] du2, int[] ipiv, Complex[] b, int nrhs = 1, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }

        public void zhetrf(int n, Complex[] a, int[] iPivot, Complex[] work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public int zhetrfQuery(int n, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void zhetrs(int n, Complex[] a, int[] ipiv, Complex[] b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void zhetrs2(int n, Complex[] a, int[] ipiv, Complex[] b, Complex[] work, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void zhptrf(int n, Complex[] aPacked, int[] iPivot, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void zhptrs(int n, Complex[] ap, int[] ipiv, Complex[] b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void zpbtrf(int n, int kd, Complex[] a, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void zpbtrs(int n, int kd, Complex[] ab, Complex[] b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void zpftrf(int n, Complex[] a, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }

        public void zpftrs(int n, Complex[] a, Complex[] b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }

        public void zpotrf(int n, Complex[] a, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void zpotrs(int n, Complex[] a, Complex[] b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void zpptrf(int n, Complex[] aPacked, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void zpptrs(int n, Complex[] ap, Complex[] b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void zpttrf(int n, Complex[] diagonalElements, Complex[] e)
        {
            throw new NotImplementedException();
        }

        public void zpttrs(int n, double[] d, Complex[] e, Complex[] b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void zsptrf(int n, Complex[] aPacked, int[] iPivot, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void zsptrs(int n, Complex[] ap, int[] ipiv, Complex[] b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void zsytrf(int n, Complex[] a, int[] iPivot, Complex[] work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public int zsytrfQuery(int n, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void zsytrs(int n, Complex[] a, int[] ipiv, Complex[] b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void zsytrs2(int n, Complex[] a, int[] ipiv, Complex[] b, Complex[] work, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void ztbtrs(int n, int kd, Complex[] ab, Complex[] b, int nrhs = 1, bool isUnitTriangularMatrix = true, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }

        public void ztptrs(int n, Complex[] ap, Complex[] b, int nrhs = 1, bool isUnitTriangularMatrix = true, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }

        public void ztrtrs(int n, Complex[] a, Complex[] b, int nrhs = 1, bool isUnitTriangularMatrix = true, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }
    }
}