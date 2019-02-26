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
        public double dgbcon(int n, int kl, int ku, Span<double> a, double normOfOriginalMatrix, Span<double> work, LapackLinearEquations.MatrixConditionNormType matrixNormType = LapackLinearEquations.MatrixConditionNormType.Infinity)
        {
            throw new NotImplementedException();
        }

        public void dgbtrf(int m, int n, int kl, int ku, Span<double> a, int[] iPivot)
        {
            throw new NotImplementedException();
        }

        public void dgbtrs(int n, int kl, int ku, Span<double> ab, int[] ipiv, Span<double> b, int nrhs = 1, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }

        public double dgecon(int n, Span<double> a, double normOfOriginalMatrix, Span<double> work, LapackLinearEquations.MatrixConditionNormType matrixNormType = LapackLinearEquations.MatrixConditionNormType.Infinity)
        {
            throw new NotImplementedException();
        }

        public void dgetrf(int m, int n, Span<double> a, int[] iPivot)
        {
            throw new NotImplementedException();
        }

        public void dgetrs(int n, Span<double> a, int[] ipiv, Span<double> b, int nrhs = 1, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }

        public double dgtcon(int n, Span<double> dl, Span<double> d, Span<double> du, Span<double> du2, int[] iPivot, double normOfOriginalMatrix, Span<double> work, LapackLinearEquations.MatrixConditionNormType matrixNormType = LapackLinearEquations.MatrixConditionNormType.Infinity)
        {
            throw new NotImplementedException();
        }

        public void dgttrf(int n, Span<double> dl, Span<double> d, Span<double> du, Span<double> du2, int[] iPivot)
        {
            throw new NotImplementedException();
        }

        public void dgttrs(int n, Span<double> dl, Span<double> d, Span<double> du, Span<double> du2, int[] ipiv, Span<double> b, int nrhs = 1, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }

        public void dpbtrf(int n, int kd, Span<double> a, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void dpbtrs(int n, int kd, Span<double> ab, Span<double> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void dpftrf(int n, Span<double> a, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }

        public void dpftrs(int n, Span<double> a, Span<double> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }

        public void dpotrf(int n, Span<double> a, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void dpotrs(int n, Span<double> a, Span<double> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void dpptrf(int n, Span<double> aPacked, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void dpptrs(int n, Span<double> ap, Span<double> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void dpttrf(int n, Span<double> diagonalElements, Span<double> e)
        {
            throw new NotImplementedException();
        }

        public void dpttrs(int n, Span<double> d, Span<double> e, Span<double> b, int nrhs = 1)
        {
            throw new NotImplementedException();
        }

        public void driver_dgbsv(int n, int kl, int ku, Span<double> ab, int[] ipiv, Span<double> b, int nrhs = 1)
        {
            throw new NotImplementedException();
        }

        public void driver_dgesv(int n, Span<double> a, int[] ipiv, Span<double> b, int nrhs = 1)
        {
            throw new NotImplementedException();
        }

        public void driver_dgtsv(int n, Span<double> dl, Span<double> d, Span<double> du, Span<double> b, int nrhs = 1)
        {
            throw new NotImplementedException();
        }

        public void driver_dpbsv(int n, int kd, Span<double> ab, Span<double> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void driver_dposv(int n, Span<double> a, Span<double> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void driver_dppsv(int n, Span<double> ap, Span<double> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void driver_dptsv(int n, Span<double> d, Span<double> e, Span<double> b, int nrhs = 1)
        {
            throw new NotImplementedException();
        }

        public void driver_dspsv(int n, Span<double> ap, int[] ipiv, Span<double> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void driver_dsysv(int n, Span<double> a, int[] ipiv, Span<double> b, Span<double> work, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public int driver_dsysvQuery(int n, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void driver_zgbsv(int n, int kl, int ku, Span<Complex> ab, int[] ipiv, Span<Complex> b, int nrhs = 1)
        {
            throw new NotImplementedException();
        }

        public void driver_zgesv(int n, Span<Complex> a, int[] ipiv, Span<Complex> b, int nrhs = 1)
        {
            throw new NotImplementedException();
        }

        public void driver_zgtsv(int n, Span<Complex> dl, Span<Complex> d, Span<Complex> du, Span<Complex> b, int nrhs = 1)
        {
            throw new NotImplementedException();
        }

        public void driver_zhesv(int n, Span<Complex> a, int[] ipiv, Span<Complex> b, Span<Complex> work, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public int driver_zhesvQuery(int n, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void driver_zhpsv(int n, Span<Complex> ap, int[] ipiv, Span<Complex> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void driver_zpbsv(int n, int kd, Span<Complex> ab, Span<Complex> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void driver_zposv(int n, Span<Complex> a, Span<Complex> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void driver_zppsv(int n, Span<Complex> ap, Span<Complex> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void driver_zptsv(int n, Span<double> d, Span<Complex> e, Span<Complex> b, int nrhs = 1)
        {
            throw new NotImplementedException();
        }

        public void driver_zspsv(int n, Span<Complex> ap, int[] ipiv, Span<Complex> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void driver_zsysv(int n, Span<Complex> a, int[] ipiv, Span<Complex> b, Span<Complex> work, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public int driver_zsysvQuery(int n, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void dsptrf(int n, Span<double> aPacked, int[] iPivot, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void dsptrs(int n, Span<double> ap, int[] ipiv, Span<double> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void dsytrf(int n, Span<double> a, int[] iPivot, Span<double> work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public int dsytrfQuery(int n, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void dsytrs(int n, Span<double> a, int[] ipiv, Span<double> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void dsytrs2(int n, Span<double> a, int[] ipiv, Span<double> b, Span<double> work, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void dtbtrs(int n, int kd, Span<double> ab, Span<double> b, int nrhs = 1, bool isUnitTriangularMatrix = true, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }

        public void dtptrs(int n, Span<double> ap, Span<double> b, int nrhs = 1, bool isUnitTriangularMatrix = true, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }

        public void dtrtrs(int n, Span<double> a, Span<double> b, int nrhs = 1, bool isUnitTriangularMatrix = true, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }

        public void zgbtrf(int m, int n, int kl, int ku, Span<Complex> a, int[] iPivot)
        {
            throw new NotImplementedException();
        }

        public void zgbtrs(int n, int kl, int ku, Span<Complex> ab, int[] ipiv, Span<Complex> b, int nrhs = 1, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }

        public void zgetrf(int m, int n, Span<Complex> a, int[] iPivot)
        {
            throw new NotImplementedException();
        }

        public void zgetrs(int n, Span<Complex> a, int[] ipiv, Span<Complex> b, int nrhs = 1, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }

        public void zgttrf(int n, Span<Complex> dl, Span<Complex> d, Span<Complex> du, Span<Complex> du2, int[] iPivot)
        {
            throw new NotImplementedException();
        }

        public void zgttrs(int n, Span<Complex> dl, Span<Complex> d, Span<Complex> du, Span<Complex> du2, int[] ipiv, Span<Complex> b, int nrhs = 1, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }

        public void zhetrf(int n, Span<Complex> a, int[] iPivot, Span<Complex> work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public int zhetrfQuery(int n, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void zhetrs(int n, Span<Complex> a, int[] ipiv, Span<Complex> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void zhetrs2(int n, Span<Complex> a, int[] ipiv, Span<Complex> b, Span<Complex> work, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void zhptrf(int n, Span<Complex> aPacked, int[] iPivot, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void zhptrs(int n, Span<Complex> ap, int[] ipiv, Span<Complex> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void zpbtrf(int n, int kd, Span<Complex> a, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void zpbtrs(int n, int kd, Span<Complex> ab, Span<Complex> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void zpftrf(int n, Span<Complex> a, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }

        public void zpftrs(int n, Span<Complex> a, Span<Complex> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }

        public void zpotrf(int n, Span<Complex> a, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void zpotrs(int n, Span<Complex> a, Span<Complex> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void zpptrf(int n, Span<Complex> aPacked, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void zpptrs(int n, Span<Complex> ap, Span<Complex> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void zpttrf(int n, Span<Complex> diagonalElements, Span<Complex> e)
        {
            throw new NotImplementedException();
        }

        public void zpttrs(int n, Span<double> d, Span<Complex> e, Span<Complex> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void zsptrf(int n, Span<Complex> aPacked, int[] iPivot, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void zsptrs(int n, Span<Complex> ap, int[] ipiv, Span<Complex> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void zsytrf(int n, Span<Complex> a, int[] iPivot, Span<Complex> work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public int zsytrfQuery(int n, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void zsytrs(int n, Span<Complex> a, int[] ipiv, Span<Complex> b, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void zsytrs2(int n, Span<Complex> a, int[] ipiv, Span<Complex> b, Span<Complex> work, int nrhs = 1, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void ztbtrs(int n, int kd, Span<Complex> ab, Span<Complex> b, int nrhs = 1, bool isUnitTriangularMatrix = true, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }

        public void ztptrs(int n, Span<Complex> ap, Span<Complex> b, int nrhs = 1, bool isUnitTriangularMatrix = true, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }

        public void ztrtrs(int n, Span<Complex> a, Span<Complex> b, int nrhs = 1, bool isUnitTriangularMatrix = true, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }
    }
}