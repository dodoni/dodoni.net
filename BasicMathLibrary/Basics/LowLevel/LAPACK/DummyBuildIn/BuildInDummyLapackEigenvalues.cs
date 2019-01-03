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
    /// <summary>Serves as dummy implementation for LAPACK routines.
    /// </summary>
    /// <seealso cref="Dodoni.MathLibrary.Basics.LowLevel.LapackEigenvalues.IGeneralizedNonsymmetricEigenvalueProblems" />
    /// <seealso cref="Dodoni.MathLibrary.Basics.LowLevel.LapackEigenvalues.IGeneralizedSymmetricEigenvalueProblems" />
    /// <seealso cref="Dodoni.MathLibrary.Basics.LowLevel.LapackEigenvalues.INonSymmetricEigenvalueProblems" />
    /// <seealso cref="Dodoni.MathLibrary.Basics.LowLevel.LapackEigenvalues.ISymmetricEigenvalueProblems" />
    /// <seealso cref="Dodoni.MathLibrary.Basics.LowLevel.LapackEigenvalues.ISingularValueDecomposition" />
    /// <seealso cref="Dodoni.MathLibrary.Basics.LowLevel.LapackEigenvalues.IGeneralizedSingularValueDecomposition" />
    /// <seealso cref="Dodoni.MathLibrary.Basics.LowLevel.LapackEigenvalues.ILinearLeastSquaresProblems" />
    internal class BuildInDummyLapackEigenvalues :
        LapackEigenvalues.IGeneralizedNonsymmetricEigenvalueProblems,
        LapackEigenvalues.IGeneralizedSymmetricEigenvalueProblems,
        LapackEigenvalues.INonSymmetricEigenvalueProblems,
        LapackEigenvalues.ISymmetricEigenvalueProblems,
        LapackEigenvalues.ISingularValueDecomposition,
        LapackEigenvalues.IGeneralizedSingularValueDecomposition,
        LapackEigenvalues.ILinearLeastSquaresProblems
    {
        public void dbdsdc(LapackEigenvalues.SVDdbdsdcJob job, int n, double[] d, double[] e, double[] u, double[] vt, double[] q, int[] iq, double[] work, int[] iwork, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void dbsdsqr(int n, int ncvt, int nru, int ncc, double[] d, double[] e, double[] vt, double[] u, double[] c, double[] work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void ddisna(LapackEigenvalues.SymmetricDdisnaJob job, int m, int n, double[] d, double[] sep)
        {
            throw new NotImplementedException();
        }

        public void dgbbrd(LapackEigenvalues.SVDxgbbrdJob job, int m, int n, int ncc, int kl, int ku, double[] ab, double[] d, double[] e, double[] q, double[] pt, double[] c, double[] work)
        {
            throw new NotImplementedException();
        }

        public void dgebak(LapackEigenvalues.NonSymmetricMatrixBalancesType job, LAPACK.Side side, int n, int ilo, int ihi, double[] scale, int m, double[] v)
        {
            throw new NotImplementedException();
        }

        public void dgebal(LapackEigenvalues.NonSymmetricMatrixBalancesType job, int n, double[] a, out int ilo, out int ihi, double[] scale)
        {
            throw new NotImplementedException();
        }

        public void dgebrd(int m, int n, double[] a, double[] d, double[] e, double[] tauq, double[] taup, double[] work)
        {
            throw new NotImplementedException();
        }

        public int dgebrdQuery(int m, int n)
        {
            throw new NotImplementedException();
        }

        public void dgehrd(int n, int ilo, int ihi, double[] a, double[] tau, double[] work)
        {
            throw new NotImplementedException();
        }

        public int dgehrdQuery(int n, int ilo, int ihi)
        {
            throw new NotImplementedException();
        }

        public void dhsein(LapackEigenvalues.NonSymmetricEigenValueVectorsJob job, LapackEigenvalues.NonSymmetricEigenvalueSource eigenvalueSource, LapackEigenvalues.NonSymmetricXhseinInitV initv, bool[] select, int n, double[] h, double[] wr, double[] wi, double[] vl, double[] vr, int mm, out int m, double[] work, int[] ifaill, int[] ifailr)
        {
            throw new NotImplementedException();
        }

        public void dhseqr(LapackEigenvalues.NonSymmetricXhseqrJob job, LapackEigenvalues.NonSymmetricXhseqrOperation operation, int n, int ilo, int ihi, double[] h, double[] wr, double[] wi, double[] z, double[] work)
        {
            throw new NotImplementedException();
        }

        public int dhseqrQuery(LapackEigenvalues.NonSymmetricXhseqrJob job, LapackEigenvalues.NonSymmetricXhseqrOperation operation, int n, int ilo, int ihi)
        {
            throw new NotImplementedException();
        }

        public void dopgtr(int n, double[] ap, double[] tau, double[] q, double[] work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void dopmtr(int m, int n, double[] ap, double[] tau, double[] c, double[] work, LAPACK.Side side = LAPACK.Side.Left, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void dorgbr(LapackEigenvalues.SVDxorgbrJob job, int m, int n, int k, double[] a, double[] tau, double[] work)
        {
            throw new NotImplementedException();
        }

        public int dorgbrQuery(LapackEigenvalues.SVDxorgbrJob job, int m, int n, int k)
        {
            throw new NotImplementedException();
        }

        public void dorghr(int n, int ilo, int ihi, double[] a, double[] tau, double[] work)
        {
            throw new NotImplementedException();
        }

        public int dorghrQuery(int n, int ilo, int ihi)
        {
            throw new NotImplementedException();
        }

        public void dorgtr(int n, double[] a, double[] tau, double[] work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public int dorgtrQuery(int n, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void dormbr(LapackEigenvalues.SVDxormbrJob job, int m, int n, int k, double[] a, double[] tau, double[] c, double[] work, LAPACK.Side side = LAPACK.Side.Left, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }

        public int dormbrQuery(LapackEigenvalues.SVDxormbrJob job, int m, int n, int k, LAPACK.Side side = LAPACK.Side.Left, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }

        public void dormhr(LAPACK.Side side, BLAS.MatrixTransposeState transposeState, int m, int n, int ilo, int ihi, double[] a, double[] tau, double[] c, double[] work)
        {
            throw new NotImplementedException();
        }

        public int dormhrQuery(LAPACK.Side side, BLAS.MatrixTransposeState transposeState, int m, int n, int ilo, int ihi)
        {
            throw new NotImplementedException();
        }

        public void dormtr(int m, int n, double[] a, double[] tau, double[] c, double[] work, LAPACK.Side side, BLAS.MatrixTransposeState transposeState, BLAS.TriangularMatrixType triangularMatrixType)
        {
            throw new NotImplementedException();
        }

        public int dormtrQuery(int m, int n, LAPACK.Side side = LAPACK.Side.Left, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void dpteqr(LapackEigenvalues.SymmetricJob job, int n, double[] d, double[] e, double[] z, double[] work)
        {
            throw new NotImplementedException();
        }

        public void driver_dgees(LapackEigenvalues.NonSymmetricSchurVectorsJob job, LapackEigenvalues.NonSymmetricEigenvalueOrdering sort, Func<double, double, bool> select, int n, double[] a, out int sdim, double[] wr, double[] wi, double[] vs, double[] work, bool[] bwork)
        {
            throw new NotImplementedException();
        }

        public int driver_dgeesQuery(LapackEigenvalues.NonSymmetricSchurVectorsJob job, LapackEigenvalues.NonSymmetricEigenvalueOrdering sort, int n)
        {
            throw new NotImplementedException();
        }

        public void driver_dgeesx(LapackEigenvalues.NonSymmetricSchurVectorsJob job, LapackEigenvalues.NonSymmetricEigenvalueOrdering sort, Func<double, double, bool> select, LapackEigenvalues.NonSymmetricSense sense, int n, double[] a, out int sdim, double[] wr, double[] wi, double[] vs, out double rconde, out double rcondv, double[] work, int[] iwork, bool[] bwork)
        {
            throw new NotImplementedException();
        }

        public int driver_dgeesxQuery(LapackEigenvalues.NonSymmetricSchurVectorsJob job, LapackEigenvalues.NonSymmetricEigenvalueOrdering sort, LapackEigenvalues.NonSymmetricSense sense, int n)
        {
            throw new NotImplementedException();
        }

        public void driver_dgeev(bool computeLeftEigenvectors, bool computeRightEigenvectors, int n, double[] a, double[] wr, double[] wi, double[] vl, double[] vr, double[] work)
        {
            throw new NotImplementedException();
        }

        public int driver_dgeevQuery(bool computeLeftEigenvectors, bool computeRightEigenvectors, int n)
        {
            throw new NotImplementedException();
        }

        public void driver_dgeevx(LapackEigenvalues.NonSymmetricMatrixBalancesType balanceType, bool computeLeftEigenvectors, bool computeRightEigenvectors, LapackEigenvalues.NonSymmetricXgeevxSense sense, int n, double[] a, double[] wr, double[] wi, double[] vl, double[] vr, out int ilo, out int ihi, double[] scale, out double abnrm, double[] rconde, double[] rcondv, double[] work, int[] iwork)
        {
            throw new NotImplementedException();
        }

        public int driver_dgeevxQuery(LapackEigenvalues.NonSymmetricMatrixBalancesType balanceType, bool computeLeftEigenvectors, bool computeRightEigenvectors, LapackEigenvalues.NonSymmetricXgeevxSense sense, int n)
        {
            throw new NotImplementedException();
        }

        public void driver_dgejsv(LapackEigenvalues.SVDdgejsvJobA joba, LapackEigenvalues.SVDdgejsvJobU jobu, LapackEigenvalues.SVDdgejsvJobV jobv, LapackEigenvalues.SVDdgejsvJobR jobr, LapackEigenvalues.SVDdgejsvJobT jobt, LapackEigenvalues.SVDdgejsvJobP jobp, int m, int n, double[] a, double[] sva, double[] u, double[] v, double[] work, int[] iwork)
        {
            throw new NotImplementedException();
        }

        public void driver_dgels(int m, int n, int nrhs, double[] a, double[] b, double[] work, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }

        public void driver_dgelsd(int m, int n, int nrhs, double[] a, double[] b, double[] s, double rcond, double[] work, int[] iwork, out int rank)
        {
            throw new NotImplementedException();
        }

        public int driver_dgelsdQuery(int m, int n, int nrhs, double rcond, out int ilwork)
        {
            throw new NotImplementedException();
        }

        public int driver_dgelsQuery(int m, int n, int nrhs, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }

        public void driver_dgelss(int m, int n, int nrhs, double[] a, double[] b, double[] s, double rcond, double[] work, out int rank)
        {
            throw new NotImplementedException();
        }

        public int driver_dgelssQuery(int m, int n, int nrhs, double rcond)
        {
            throw new NotImplementedException();
        }

        public void driver_dgelsy(int m, int n, int nrhs, double[] a, double[] b, int[] jpvt, double rcond, double[] work, out int rank)
        {
            throw new NotImplementedException();
        }

        public int driver_dgelsyQuery(int m, int n, int nrhs, double rcond)
        {
            throw new NotImplementedException();
        }

        public void driver_dgesdd(LapackEigenvalues.SVDxgesddJob job, int m, int n, double[] a, double[] s, double[] u, double[] vt, double[] work, int[] iwork)
        {
            throw new NotImplementedException();
        }

        public int driver_dgesddQuery(LapackEigenvalues.SVDxgesddJob job, int m, int n)
        {
            throw new NotImplementedException();
        }

        public void driver_dgesvd(int m, int n, double[] a, double[] s, double[] u, double[] vt, double[] work, LapackEigenvalues.SVDleftSingularVectorsJob uJob = LapackEigenvalues.SVDleftSingularVectorsJob.All, LapackEigenvalues.SVDrightSingularVectorsJob vtJob = LapackEigenvalues.SVDrightSingularVectorsJob.All)
        {
            throw new NotImplementedException();
        }

        public int driver_dgesvdQuery(int m, int n, LapackEigenvalues.SVDleftSingularVectorsJob uJob = LapackEigenvalues.SVDleftSingularVectorsJob.All, LapackEigenvalues.SVDrightSingularVectorsJob vtJob = LapackEigenvalues.SVDrightSingularVectorsJob.All)
        {
            throw new NotImplementedException();
        }

        public void driver_dgesvj(LapackEigenvalues.SVDdgesvjJobA joba, LapackEigenvalues.SVDdgesvjJobU jobu, LapackEigenvalues.SVDdgesvjJobV jobv, int m, int n, double[] a, double[] sva, int mv, double[] v, double[] work)
        {
            throw new NotImplementedException();
        }

        public void driver_dggglm(int n, int m, int p, double[] a, double[] b, double[] d, double[] x, double[] y, double[] work)
        {
            throw new NotImplementedException();
        }

        public int driver_dggglmQuery(int n, int m, int p)
        {
            throw new NotImplementedException();
        }

        public void driver_dgglse(int m, int n, int p, double[] a, double[] b, double[] c, double[] d, double[] x, double[] work)
        {
            throw new NotImplementedException();
        }

        public int driver_dgglseQuery(int m, int n, int p)
        {
            throw new NotImplementedException();
        }

        public void driver_dggsvd(LapackEigenvalues.SVDxggsvdJob jobu, LapackEigenvalues.SVDxggsvdJob jobv, LapackEigenvalues.SVDxggsvdJob jobq, int m, int n, int p, out int k, out int l, double[] a, double[] b, double[] alpha, double[] beta, double[] u, double[] v, double[] q, double[] work, int[] iwork)
        {
            throw new NotImplementedException();
        }

        public void driver_dspev(LapackEigenvalues.SymmetricGeneralJob job, int n, double[] ap, double[] w, double[] z, double[] work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void driver_dsyev(LapackEigenvalues.SymmetricGeneralJob job, int n, double[] a, double[] w, double[] work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void driver_dsyevd(LapackEigenvalues.SymmetricGeneralJob job, int n, double[] a, double[] w, double[] work, int[] iwork, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void driver_dsyevdQuery(LapackEigenvalues.SymmetricGeneralJob job, int n, out int lwork, out int liwork, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public int driver_dsyevQuery(LapackEigenvalues.SymmetricGeneralJob job, int n, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void driver_dsyevr(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, double[] a, double vl, double vu, int il, int iu, out int m, double[] w, double[] z, int[] isuppz, double[] work, int[] iwork, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, double abstol = 1E-08)
        {
            throw new NotImplementedException();
        }

        public void driver_dsyevrQuery(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, double vl, double vu, int il, int iu, out int lwork, out int liwork, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, double abstol = 1E-08)
        {
            throw new NotImplementedException();
        }

        public void driver_dsyevx(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, double[] a, double vl, double vu, int il, int iu, out int m, double[] w, double[] z, int[] ifail, double[] work, int[] iwork, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, double abstol = 1E-08)
        {
            throw new NotImplementedException();
        }

        public int driver_dsyevxQuery(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, double vl, double vu, int il, int iu, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, double abstol = 1E-08)
        {
            throw new NotImplementedException();
        }

        public void driver_zgees(LapackEigenvalues.NonSymmetricSchurVectorsJob job, LapackEigenvalues.NonSymmetricEigenvalueOrdering sort, Func<Complex, bool> select, int n, Complex[] a, out int sdim, Complex[] w, Complex[] vs, Complex[] work, double[] rwork, bool[] bwork)
        {
            throw new NotImplementedException();
        }

        public int driver_zgeesQuery(LapackEigenvalues.NonSymmetricSchurVectorsJob job, LapackEigenvalues.NonSymmetricEigenvalueOrdering sort, int n)
        {
            throw new NotImplementedException();
        }

        public void driver_zgeesx(LapackEigenvalues.NonSymmetricSchurVectorsJob job, LapackEigenvalues.NonSymmetricEigenvalueOrdering sort, Func<Complex, bool> select, LapackEigenvalues.NonSymmetricSense sense, int n, Complex[] a, out int sdim, Complex[] w, Complex[] vs, out double rconde, out double rcondv, Complex[] work, double[] rwork, bool[] bwork)
        {
            throw new NotImplementedException();
        }

        public int driver_zgeesxQuery(LapackEigenvalues.NonSymmetricSchurVectorsJob job, LapackEigenvalues.NonSymmetricEigenvalueOrdering sort, LapackEigenvalues.NonSymmetricSense sense, int n)
        {
            throw new NotImplementedException();
        }

        public void driver_zgeev(bool computeLeftEigenvectors, bool computeRightEigenvectors, int n, Complex[] a, Complex[] w, Complex[] vl, Complex[] vr, Complex[] work, double[] rwork)
        {
            throw new NotImplementedException();
        }

        public int driver_zgeevQuery(bool computeLeftEigenvectors, bool computeRightEigenvectors, int n)
        {
            throw new NotImplementedException();
        }

        public void driver_zgeevx(LapackEigenvalues.NonSymmetricMatrixBalancesType balanceType, bool computeLeftEigenvectors, bool computeRightEigenvectors, LapackEigenvalues.NonSymmetricXgeevxSense sense, int n, Complex[] a, Complex[] w, Complex[] vl, Complex[] vr, out int ilo, out int ihi, double[] scale, out double abnrm, double[] rconde, double[] rcondv, Complex[] work, double[] rwork)
        {
            throw new NotImplementedException();
        }

        public int driver_zgeevxQuery(LapackEigenvalues.NonSymmetricMatrixBalancesType balanceType, bool computeLeftEigenvectors, bool computeRightEigenvectors, LapackEigenvalues.NonSymmetricXgeevxSense sense, int n)
        {
            throw new NotImplementedException();
        }

        public void driver_zgels(int m, int n, int nrhs, Complex[] a, Complex[] b, Complex[] work, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }

        public void driver_zgelsd(int m, int n, int nrhs, Complex[] a, Complex[] b, double[] s, double rcond, Complex[] work, double[] rwork, int[] iwork, out int rank)
        {
            throw new NotImplementedException();
        }

        public int driver_zgelsdQuery(int m, int n, int nrhs, double rcond, out int ilwork, out int rlwork)
        {
            throw new NotImplementedException();
        }

        public int driver_zgelsQuery(int m, int n, int nrhs, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }

        public void driver_zgelss(int m, int n, int nrhs, Complex[] a, Complex[] b, double[] s, double rcond, Complex[] work, double[] rwork, out int rank)
        {
            throw new NotImplementedException();
        }

        public int driver_zgelssQuery(int m, int n, int nrhs, double rcond)
        {
            throw new NotImplementedException();
        }

        public void driver_zgelsy(int m, int n, int nrhs, Complex[] a, Complex[] b, int[] jpvt, double rcond, Complex[] work, double[] rwork, out int rank)
        {
            throw new NotImplementedException();
        }

        public int driver_zgelsyQuery(int m, int n, int nrhs, double rcond)
        {
            throw new NotImplementedException();
        }

        public void driver_zgesdd(LapackEigenvalues.SVDxgesddJob job, int m, int n, Complex[] a, double[] s, Complex[] u, Complex[] vt, Complex[] work, double[] rwork, int[] iwork)
        {
            throw new NotImplementedException();
        }

        public int driver_zgesddQuery(LapackEigenvalues.SVDxgesddJob job, int m, int n)
        {
            throw new NotImplementedException();
        }

        public void driver_zgesvd(int m, int n, Complex[] a, double[] s, Complex[] u, Complex[] vt, Complex[] work, double[] rwork, LapackEigenvalues.SVDleftSingularVectorsJob uJob = LapackEigenvalues.SVDleftSingularVectorsJob.All, LapackEigenvalues.SVDrightSingularVectorsJob vtJob = LapackEigenvalues.SVDrightSingularVectorsJob.All)
        {
            throw new NotImplementedException();
        }

        public int driver_zgesvdQuery(int m, int n, LapackEigenvalues.SVDleftSingularVectorsJob uJob = LapackEigenvalues.SVDleftSingularVectorsJob.All, LapackEigenvalues.SVDrightSingularVectorsJob vtJob = LapackEigenvalues.SVDrightSingularVectorsJob.All)
        {
            throw new NotImplementedException();
        }

        public void driver_zggglm(int n, int m, int p, Complex[] a, Complex[] b, Complex[] d, Complex[] x, Complex[] y, Complex[] work)
        {
            throw new NotImplementedException();
        }

        public int driver_zggglmQuery(int n, int m, int p)
        {
            throw new NotImplementedException();
        }

        public void driver_zgglse(int m, int n, int p, Complex[] a, Complex[] b, Complex[] c, Complex[] d, Complex[] x, Complex[] work)
        {
            throw new NotImplementedException();
        }

        public int driver_zgglseQuery(int m, int n, int p)
        {
            throw new NotImplementedException();
        }

        public void driver_zggsvd(LapackEigenvalues.SVDxggsvdJob jobu, LapackEigenvalues.SVDxggsvdJob jobv, LapackEigenvalues.SVDxggsvdJob jobq, int m, int n, int p, out int k, out int l, Complex[] a, Complex[] b, double[] alpha, double[] beta, Complex[] u, Complex[] v, Complex[] q, Complex[] work, double[] rwork, int[] iwork)
        {
            throw new NotImplementedException();
        }

        public void driver_zheev(LapackEigenvalues.SymmetricGeneralJob job, int n, Complex[] a, double[] w, Complex[] work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void driver_zheevd(LapackEigenvalues.SymmetricGeneralJob job, int n, Complex[] a, double[] w, Complex[] work, double[] rwork, int[] iwork, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void driver_zheevdQuery(LapackEigenvalues.SymmetricGeneralJob job, int n, out int lwork, out int liwork, out int lrwork, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public int driver_zheevQuery(LapackEigenvalues.SymmetricGeneralJob job, int n, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void driver_zheevr(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, Complex[] a, double vl, double vu, int il, int iu, out int m, double[] w, Complex[] z, int[] isuppz, Complex[] work, double[] rwork, int[] iwork, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, double abstol = 1E-08)
        {
            throw new NotImplementedException();
        }

        public void driver_zheevrQuery(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, double vl, double vu, int il, int iu, out int lwork, out int lrwork, out int liwork, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, double abstol = 1E-08)
        {
            throw new NotImplementedException();
        }

        public void driver_zheevx(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, Complex[] a, double vl, double vu, int il, int iu, out int m, double[] w, Complex[] z, int[] ifail, Complex[] work, double[] rwork, int[] iwork, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, double abstol = 1E-08)
        {
            throw new NotImplementedException();
        }

        public int driver_zheevxQuery(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, double vl, double vu, int il, int iu, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, double abstol = 1E-08)
        {
            throw new NotImplementedException();
        }

        public void driver_zhpev(LapackEigenvalues.SymmetricGeneralJob job, int n, Complex[] ap, double[] w, Complex[] z, Complex[] work, double[] rwork, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void dsbtrd(LapackEigenvalues.SymmetricXxbtrdJob job, int n, int kd, double[] ab, double[] d, double[] e, double[] q, double[] work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void dsptrd(int n, double[] ap, double[] d, double[] e, double[] tau, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void dstebz(LapackEigenvalues.SymmetricEigenvaluesRange range, LapackEigenvalues.SymmetricDstebzOrder order, int n, double[] d, double[] e, double vl, double vu, int il, int iu, out int m, double[] w, out int nsplit, int[] iblock, int[] isplit, double[] work, double abstol = 1E-08)
        {
            throw new NotImplementedException();
        }

        public void dstedc(LapackEigenvalues.SymmetricJob job, int n, double[] d, double[] e, double[] z, double[] work, int[] iwork)
        {
            throw new NotImplementedException();
        }

        public void dstedcQuery(LapackEigenvalues.SymmetricJob job, int n, out int lwork, out int liwork)
        {
            throw new NotImplementedException();
        }

        public void dstegr(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, double[] d, double[] e, double vl, double vu, int il, int iu, out int m, double[] w, double[] z, int[] isuppz, double[] work, int[] iwork)
        {
            throw new NotImplementedException();
        }

        public void dstein(int n, double[] d, double[] e, int m, double[] w, int[] iblock, int[] isplit, double[] z, int[] ifailv, double[] work, int[] iwork)
        {
            throw new NotImplementedException();
        }

        public void dstemr(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, double[] d, double[] e, double vl, double vu, int il, int iu, out int m, double[] w, double[] z, int nzc, int[] isuppz, double[] work, int[] iwork, bool tryrac = true)
        {
            throw new NotImplementedException();
        }

        public void dstemrQuery(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, double vl, double vu, int il, int iu, out int lwork, out int liwork, out int nzc)
        {
            throw new NotImplementedException();
        }

        public void dsteqr(LapackEigenvalues.SymmetricJob job, int n, double[] d, double[] e, double[] z, double[] work)
        {
            throw new NotImplementedException();
        }

        public void dsterf(int n, double[] d, double[] e)
        {
            throw new NotImplementedException();
        }

        public void dsytrd(int n, double[] a, double[] d, double[] e, double[] tau, double[] work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public int dsytrdQuery(int n, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void dtrevc(LapackEigenvalues.NonSymmetricEigenValueVectorsJob job, LapackEigenvalues.NonSymmetricXtrevcOperation howmny, bool[] select, int n, double[] t, double[] vl, double[] vr, int mm, out int m, double[] work)
        {
            throw new NotImplementedException();
        }

        public void dtrexc(bool updatedSchurVectors, int n, double[] t, double[] q, int ifst, int ilst, double[] work)
        {
            throw new NotImplementedException();
        }

        public void dtrsyl(BLAS.MatrixTransposeState transposeStateA, BLAS.MatrixTransposeState transposeStateB, int sign, int m, int n, double[] a, double[] b, double[] c, out double scale)
        {
            throw new NotImplementedException();
        }

        public void zbsdsqr(int n, int ncvt, int nru, int ncc, double[] d, double[] e, Complex[] vt, Complex[] u, Complex[] c, double[] work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void zgbbrd(LapackEigenvalues.SVDxgbbrdJob job, int m, int n, int ncc, int kl, int ku, Complex[] ab, double[] d, double[] e, Complex[] q, Complex[] pt, Complex[] c, Complex[] work, double[] rwork)
        {
            throw new NotImplementedException();
        }

        public void zgebak(LapackEigenvalues.NonSymmetricMatrixBalancesType job, LAPACK.Side side, int n, int ilo, int ihi, double[] scale, int m, Complex[] v)
        {
            throw new NotImplementedException();
        }

        public void zgebal(LapackEigenvalues.NonSymmetricMatrixBalancesType job, int n, Complex[] a, out int ilo, out int ihi, double[] scale)
        {
            throw new NotImplementedException();
        }

        public void zgebrd(int m, int n, Complex[] a, double[] d, double[] e, Complex[] tauq, Complex[] taup, Complex[] work)
        {
            throw new NotImplementedException();
        }

        public int zgebrdQuery(int m, int n)
        {
            throw new NotImplementedException();
        }

        public void zgehrd(int n, int ilo, int ihi, Complex[] a, Complex[] tau, Complex[] work)
        {
            throw new NotImplementedException();
        }

        public int zgehrdQuery(int n, int ilo, int ihi)
        {
            throw new NotImplementedException();
        }

        public void zhbtrd(LapackEigenvalues.SymmetricXxbtrdJob job, int n, int kd, Complex[] ab, double[] d, double[] e, Complex[] q, Complex[] work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void zhetrd(int n, Complex[] a, double[] d, double[] e, Complex[] tau, Complex[] work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public int zhetrdQuery(int n, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void zhptrd(int n, Complex[] ap, double[] d, double[] e, Complex[] tau, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void zhsein(LapackEigenvalues.NonSymmetricEigenValueVectorsJob job, LapackEigenvalues.NonSymmetricEigenvalueSource eigenvalueSource, LapackEigenvalues.NonSymmetricXhseinInitV initv, bool[] select, int n, Complex[] h, Complex[] w, Complex[] vl, Complex[] vr, int mm, out int m, Complex[] work, double[] rwork, int[] ifaill, int[] ifailr)
        {
            throw new NotImplementedException();
        }

        public void zhseqr(LapackEigenvalues.NonSymmetricXhseqrJob job, LapackEigenvalues.NonSymmetricXhseqrOperation operation, int n, int ilo, int ihi, Complex[] h, Complex[] w, Complex[] z, Complex[] work)
        {
            throw new NotImplementedException();
        }

        public int zhseqrQuery(LapackEigenvalues.NonSymmetricXhseqrJob job, LapackEigenvalues.NonSymmetricXhseqrOperation operation, int n, int ilo, int ihi)
        {
            throw new NotImplementedException();
        }

        public void zpteqr(LapackEigenvalues.SymmetricJob job, int n, double[] d, double[] e, Complex[] z, double[] work)
        {
            throw new NotImplementedException();
        }

        public void zstedc(LapackEigenvalues.SymmetricJob job, int n, double[] d, double[] e, Complex[] z, Complex[] work, int[] iwork, double[] rwork)
        {
            throw new NotImplementedException();
        }

        public void zstedcQuery(LapackEigenvalues.SymmetricJob job, int n, out int lwork, out int liwork, out int lrwork)
        {
            throw new NotImplementedException();
        }

        public void zstegr(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, double[] d, double[] e, double vl, double vu, int il, int iu, out int m, double[] w, Complex[] z, int[] isuppz, double[] work, int[] iwork)
        {
            throw new NotImplementedException();
        }

        public void zstein(int n, double[] d, double[] e, int m, double[] w, int[] iblock, int[] isplit, Complex[] z, int[] ifailv, double[] work, int[] iwork)
        {
            throw new NotImplementedException();
        }

        public void zstemr(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, double[] d, double[] e, double vl, double vu, int il, int iu, out int m, double[] w, Complex[] z, int nzc, int[] isuppz, double[] work, int[] iwork, bool tryrac = true)
        {
            throw new NotImplementedException();
        }

        public void zsteqr(LapackEigenvalues.SymmetricJob job, int n, double[] d, double[] e, Complex[] z, double[] work)
        {
            throw new NotImplementedException();
        }

        public void ztrevc(LapackEigenvalues.NonSymmetricEigenValueVectorsJob job, LapackEigenvalues.NonSymmetricXtrevcOperation howmny, bool[] select, int n, Complex[] t, Complex[] vl, Complex[] vr, int mm, out int m, Complex[] work, double[] rwork)
        {
            throw new NotImplementedException();
        }

        public void ztrexc(bool updatedSchurVectors, int n, Complex[] t, Complex[] q, int ifst, int ilst)
        {
            throw new NotImplementedException();
        }

        public void ztrsyl(BLAS.MatrixTransposeState transposeStateA, BLAS.MatrixTransposeState transposeStateB, int sign, int m, int n, Complex[] a, Complex[] b, Complex[] c, out double scale)
        {
            throw new NotImplementedException();
        }

        public void zungbr(LapackEigenvalues.SVDxorgbrJob job, int m, int n, int k, Complex[] a, Complex[] tau, Complex[] work)
        {
            throw new NotImplementedException();
        }

        public int zungbrQuery(LapackEigenvalues.SVDxorgbrJob job, int m, int n, int k)
        {
            throw new NotImplementedException();
        }

        public void zunghr(int n, int ilo, int ihi, Complex[] a, Complex[] tau, Complex[] work)
        {
            throw new NotImplementedException();
        }

        public int zunghrQuery(int n, int ilo, int ihi)
        {
            throw new NotImplementedException();
        }

        public void zungtr(int n, Complex[] a, Complex[] tau, Complex[] work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public int zungtrQuery(int n, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void zunmbr(LapackEigenvalues.SVDxormbrJob job, int m, int n, int k, Complex[] a, Complex[] tau, Complex[] c, Complex[] work, LAPACK.Side side = LAPACK.Side.Left, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }

        public int zunmbrQuery(LapackEigenvalues.SVDxormbrJob job, int m, int n, int k, LAPACK.Side side = LAPACK.Side.Left, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose)
        {
            throw new NotImplementedException();
        }

        public void zunmhr(LAPACK.Side side, BLAS.MatrixTransposeState transposeState, int m, int n, int ilo, int ihi, Complex[] a, Complex[] tau, Complex[] c, Complex[] work)
        {
            throw new NotImplementedException();
        }

        public int zunmhrQuery(LAPACK.Side side, BLAS.MatrixTransposeState transposeState, int m, int n, int ilo, int ihi)
        {
            throw new NotImplementedException();
        }

        public void zunmtr(int m, int n, Complex[] a, Complex[] tau, Complex[] c, Complex[] work, LAPACK.Side side, BLAS.MatrixTransposeState transposeState, BLAS.TriangularMatrixType triangularMatrixType)
        {
            throw new NotImplementedException();
        }

        public int zunmtrQuery(int m, int n, LAPACK.Side side = LAPACK.Side.Left, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void zupgtr(int n, Complex[] ap, Complex[] tau, Complex[] q, Complex[] work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public void zupmtr(int m, int n, Complex[] ap, Complex[] tau, Complex[] c, Complex[] work, LAPACK.Side side = LAPACK.Side.Left, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }
    }
}