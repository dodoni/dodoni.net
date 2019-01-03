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
using System.Numerics;
using System.Security;
using System.Runtime.InteropServices;

namespace Dodoni.MathLibrary.Basics.LowLevel.Native
{
    internal partial class LapackNativeWrapper : LapackEigenvalues.ISymmetricEigenvalueProblems
    {
        #region private function import

        [DllImport(sm_DllName, EntryPoint = "DSYTRD", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _dsytrd(ref char uplo, ref int n, [In, Out] double[] a, ref int lda, [In, Out] double[] d, [In, Out] double[] e, [In, Out] double[] tau, [In, Out] double[] work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DSYTRD", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern unsafe void _dsytrd(ref char uplo, ref int n, [In, Out] double[] a, ref int lda, [In, Out] double[] d, [In, Out] double[] e, [In, Out] double[] tau, double* work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DORGTR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _dorgtr(ref char uplo, ref int n, [In, Out] double[] a, ref int lda, [In, Out] double[] tau, [In, Out] double[] work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DORGTR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern unsafe void _dorgtr(ref char uplo, ref int n, [In, Out] double[] a, ref int lda, [In, Out] double[] tau, double* work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DORMTR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _dormtr(ref char side, ref char uplo, ref char trans, ref int m, ref int n, [In, Out] double[] a, ref int lda, [In, Out] double[] tau, [In, Out] double[] c, ref int ldc, [In, Out] double[] work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DORMTR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern unsafe void _dormtr(ref char side, ref char uplo, ref char trans, ref int m, ref int n, [In, Out] double[] a, ref int lda, [In, Out] double[] tau, [In, Out] double[] c, ref int ldc, double* work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZHETRD", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _zhetrd(ref char uplo, ref int n, [In, Out] Complex[] a, ref int lda, [In, Out] double[] d, [In, Out] double[] e, [In, Out] Complex[] tau, [In, Out] Complex[] work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZHETRD", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern unsafe void _zhetrd(ref char uplo, ref int n, [In, Out] Complex[] a, ref int lda, [In, Out] double[] d, [In, Out] double[] e, [In, Out] Complex[] tau, Complex* work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZUNGTR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _zungtr(ref char uplo, ref int n, [In, Out] Complex[] a, ref int lda, [In, Out] Complex[] tau, [In, Out] Complex[] work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZUNGTR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern unsafe void _zungtr(ref char uplo, ref int n, [In, Out] Complex[] a, ref int lda, [In, Out] Complex[] tau, Complex* work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZUNMTR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _zunmtr(ref char side, ref char uplo, ref char trans, ref int m, ref int n, [In, Out] Complex[] a, ref int lda, [In, Out] Complex[] tau, [In, Out] Complex[] c, ref int ldc, [In, Out] Complex[] work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZUNMTR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern unsafe void _zunmtr(ref char side, ref char uplo, ref char trans, ref int m, ref int n, [In, Out] Complex[] a, ref int lda, [In, Out] Complex[] tau, [In, Out] Complex[] c, ref int ldc, Complex* work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DSPTRD", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _dsptrd(ref char uplo, ref int n, [In, Out] double[] ap, [In, Out] double[] d, [In, Out] double[] e, [In, Out] double[] tau, out int info);

        [DllImport(sm_DllName, EntryPoint = "DOPGTR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _dopgtr(ref char uplo, ref int n, [In, Out] double[] ap, [In, Out] double[] tau, [In, Out] double[] q, ref int ldq, [In, Out] double[] work, out int info);

        [DllImport(sm_DllName, EntryPoint = "DOPMTR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _dopmtr(ref char side, ref char uplo, ref char trans, ref int m, ref int n, [In, Out] double[] ap, [In, Out] double[] tau, [In, Out] double[] c, ref int ldc, [In, Out] double[] work, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZHPTRD", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _zhptrd(ref char uplo, ref int n, [In, Out] Complex[] ap, [In, Out] double[] d, [In, Out] double[] e, [In, Out] Complex[] tau, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZUPGTR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _zupgtr(ref char uplo, ref int n, [In, Out] Complex[] ap, [In, Out] Complex[] tau, [In, Out] Complex[] q, ref int ldq, [In, Out] Complex[] work, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZUPMTR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _zupmtr(ref char side, ref char uplo, ref char trans, ref int m, ref int n, [In, Out] Complex[] ap, [In, Out] Complex[] tau, [In, Out] Complex[] c, ref int ldc, [In, Out] Complex[] work, out int info);

        [DllImport(sm_DllName, EntryPoint = "DSBTRD", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _dsbtrd(ref char vect, ref char uplo, ref int n, ref int kd, [In, Out] double[] ab, ref int ldab, [In, Out] double[] d, [In, Out] double[] e, [In, Out] double[] q, ref int ldq, [In, Out] double[] work, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZHBTRD", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _zhbtrd(ref char vect, ref char uplo, ref int n, ref int kd, [In, Out] Complex[] ab, ref int ldab, [In, Out] double[] d, [In, Out] double[] e, [In, Out] Complex[] q, ref int ldq, [In, Out] Complex[] work, out int info);

        [DllImport(sm_DllName, EntryPoint = "DSTERF", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _dsterf(ref int n, [In, Out] double[] d, [In, Out] double[] e, out int info);

        [DllImport(sm_DllName, EntryPoint = "DSTEQR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _dsteqr(ref char compz, ref int n, [In, Out] double[] d, [In, Out] double[] e, [In, Out] double[] z, ref int ldz, [In, Out] double[] work, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZSTEQR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _zsteqr(ref char compz, ref int n, [In, Out] double[] d, [In, Out] double[] e, [In, Out] Complex[] z, ref int ldz, [In, Out] double[] work, out int info);

        [DllImport(sm_DllName, EntryPoint = "DSTEMR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _dstemr(ref char jobz, ref char range, ref int n, [In, Out] double[] d, [In, Out] double[] e, ref double vl, ref double vu, ref int il, ref int iu, out int m, [In, Out] double[] w, [In, Out] double[] z, ref int ldz, ref int nzc, [In, Out] int[] isuppz, ref bool tryrac, [In, Out] double[] work, ref int lwork, [In, Out] int[] iwork, ref int liwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DSTEMR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern unsafe void _dstemr(ref char jobz, ref char range, ref int n, [In, Out] double[] d, [In, Out] double[] e, ref double vl, ref double vu, ref int il, ref int iu, out int m, [In, Out] double[] w, [In, Out] double[] z, ref int ldz, ref int nzc, [In, Out] int[] isuppz, ref bool tryrac, double* work, ref int lwork, int* iwork, ref int liwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZSTEMR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _zstemr(ref char jobz, ref char range, ref int n, [In, Out] double[] d, [In, Out] double[] e, ref double vl, ref double vu, ref int il, ref int iu, out int m, [In, Out] double[] w, [In, Out] Complex[] z, ref int ldz, ref int nzc, [In, Out] int[] isuppz, ref bool tryrac, [In, Out] double[] work, ref int lwork, [In, Out] int[] iwork, ref int liwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZSTEMR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern unsafe void _zstemr(ref char jobz, ref char range, ref int n, [In, Out] double[] d, [In, Out] double[] e, ref double vl, ref double vu, ref int il, ref int iu, out int m, [In, Out] double[] w, [In, Out] Complex[] z, ref int ldz, ref int nzc, [In, Out] int[] isuppz, ref bool tryrac, double* work, ref int lwork, int* iwork, ref int liwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DSTEDC", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _dstedc(ref char compz, ref int n, [In, Out] double[] d, [In, Out] double[] e, [In, Out] double[] z, ref int ldz, [In, Out] double[] work, ref int lwork, [In, Out] int[] iwork, ref int liwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DSTEDC", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern unsafe void _dstedc(ref char compz, ref int n, [In, Out] double[] d, [In, Out] double[] e, [In, Out] double[] z, ref int ldz, double* work, ref int lwork, int* iwork, ref int liwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZSTEDC", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _zstedc(ref char compz, ref int n, [In, Out] double[] d, [In, Out] double[] e, [In, Out] Complex[] z, ref int ldz, [In, Out] Complex[] work, ref int lwork, [In, Out] double[] rwork, ref int lrwork, [In, Out] int[] iwork, ref int liwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZSTEDC", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern unsafe void _zstedc(ref char compz, ref int n, [In, Out] double[] d, [In, Out] double[] e, [In, Out] Complex[] z, ref int ldz, Complex* work, ref int lwork, double* rwork, ref int lrwork, int* iwork, ref int liwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DSTEGR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _dstegr(ref char jobz, ref char range, ref int n, [In, Out] double[] d, [In, Out] double[] e, ref double vl, ref double vu, ref int il, ref int iu, ref double abstol, out int m, [In, Out] double[] w, [In, Out] double[] z, ref int ldz, [In, Out] int[] isuppz, [In, Out] double[] work, ref int lwork, [In, Out] int[] iwork, ref int liwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZSTEGR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _zstegr(ref char jobz, ref char range, ref int n, [In, Out] double[] d, [In, Out] double[] e, ref double vl, ref double vu, ref int il, ref int iu, ref double abstol, out int m, [In, Out] double[] w, [In, Out] Complex[] z, ref int ldz, [In, Out] int[] isuppz, [In, Out] double[] work, ref int lwork, [In, Out] int[] iwork, ref int liwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DPTEQR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _dpteqr(ref char compz, ref int n, [In, Out] double[] d, [In, Out] double[] e, [In, Out] double[] z, ref int ldz, [In, Out] double[] work, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZPTEQR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _zpteqr(ref char compz, ref int n, [In, Out] double[] d, [In, Out] double[] e, [In, Out] Complex[] z, ref int ldz, [In, Out] double[] work, out int info);

        [DllImport(sm_DllName, EntryPoint = "DSTEBZ", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _dstebz(ref char range, ref char order, ref int n, ref double vl, ref double vu, ref int il, ref int iu, ref double abstol, [In, Out] double[] d, [In, Out] double[] e, out int m, out int nsplit, [In, Out] double[] w, [In, Out] int[] iblock, [In, Out] int[] isplit, [In, Out] double[] work, ref int iwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DSTEIN", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _dstein(ref int n, [In, Out] double[] d, [In, Out] double[] e, ref int m, [In, Out] double[] w, [In, Out] int[] iblock, [In, Out] int[] isplit, [In, Out] double[] z, ref int ldz, [In, Out] double[] work, [In, Out] int[] iwork, [In, Out] int[] ifailv, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZSTEIN", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _zstein(ref int n, [In, Out] double[] d, [In, Out] double[] e, ref int m, [In, Out] double[] w, [In, Out] int[] iblock, [In, Out] int[] isplit, [In, Out] Complex[] z, ref int ldz, [In, Out] double[] work, [In, Out] int[] iwork, [In, Out] int[] ifailv, out int info);

        [DllImport(sm_DllName, EntryPoint = "DDISNA", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _ddisna(ref char job, ref int m, ref int n, [In, Out] double[] d, [In, Out] double[] sep, out int info);

        // driver routines

        [DllImport(sm_DllName, EntryPoint = "DSYEV", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _driver_dsyev(ref char jobz, ref char uplo, ref int n, [In, Out] double[] a, ref int lda, [In, Out] double[] w, [In, Out] double[] work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DSYEV", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern unsafe void _driver_dsyev(ref char jobz, ref char uplo, ref int n, [In, Out] double[] a, ref int lda, [In, Out] double[] w, double* work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZHEEV", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _driver_zheev(ref char jobz, ref char uplo, ref int n, [In, Out] Complex[] a, ref int lda, [In, Out] double[] w, [In, Out] Complex[] work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZHEEV", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern unsafe void _driver_zheev(ref char jobz, ref char uplo, ref int n, [In, Out] Complex[] a, ref int lda, [In, Out] double[] w, Complex* work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DSYEVD", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _driver_dsyevd(ref char jobz, ref char uplo, ref int n, [In, Out] double[] a, ref int lda, [In, Out] double[] w, [In, Out] double[] work, ref int lwork, [In, Out] int[] iwork, ref int liwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DSYEVD", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern unsafe void _driver_dsyevd(ref char jobz, ref char uplo, ref int n, [In, Out] double[] a, ref int lda, [In, Out] double[] w, double* work, ref int lwork, int* iwork, ref int liwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZHEEVD", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _driver_zheevd(ref char jobz, ref char uplo, ref int n, [In, Out] Complex[] a, ref int lda, [In, Out] double[] w, [In, Out] Complex[] work, ref int lwork, [In, Out] double[] rwork, ref int lrwork, [In, Out] int[] iwork, ref int liwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZHEEVD", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern unsafe void _driver_zheevd(ref char jobz, ref char uplo, ref int n, [In, Out] Complex[] a, ref int lda, [In, Out] double[] w, Complex* work, ref int lwork, double* rwork, ref int lrwork, int* iwork, ref int liwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DSYEVX", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _driver_dsyevx(ref char jobz, ref char range, ref char uplo, ref int n, [In, Out] double[] a, ref int lda, ref double vl, ref double vu, ref int il, ref int iu, ref double abstol, out int m, [In, Out] double[] w, [In, Out] double[] z, ref int ldz, [In, Out] double[] work, ref int lwork, [In, Out] int[] iwork, [In, Out] int[] ifail, out int info);

        [DllImport(sm_DllName, EntryPoint = "DSYEVX", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern unsafe void _driver_dsyevx(ref char jobz, ref char range, ref char uplo, ref int n, [In, Out] double[] a, ref int lda, ref double vl, ref double vu, ref int il, ref int iu, ref double abstol, out int m, [In, Out] double[] w, [In, Out] double[] z, ref int ldz, double* work, ref int lwork, [In, Out] int[] iwork, [In, Out] int[] ifail, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZHEEVX", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _driver_zheevx(ref char jobz, ref char range, ref char uplo, ref int n, [In, Out] Complex[] a, ref int lda, ref double vl, ref double vu, ref int il, ref int iu, ref double abstol, out int m, [In, Out] double[] w, [In, Out] Complex[] z, ref int ldz, [In, Out] Complex[] work, ref int lwork, [In, Out] double[] rwork, [In, Out] int[] iwork, [In, Out] int[] ifail, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZHEEVX", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern unsafe void _driver_zheevx(ref char jobz, ref char range, ref char uplo, ref int n, [In, Out] Complex[] a, ref int lda, ref double vl, ref double vu, ref int il, ref int iu, ref double abstol, out int m, [In, Out] double[] w, [In, Out] Complex[] z, ref int ldz, Complex* work, ref int lwork, [In, Out] double[] rwork, [In, Out] int[] iwork, [In, Out] int[] ifail, out int info);

        [DllImport(sm_DllName, EntryPoint = "DSYEVR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _driver_dsyevr(ref char jobz, ref char range, ref char uplo, ref int n, [In, Out] double[] a, ref int lda, ref double vl, ref double vu, ref int il, ref int iu, ref double abstol, out int m, [In, Out] double[] w, [In, Out] double[] z, ref int ldz, [In, Out] int[] isuppz, [In, Out] double[] work, ref int lwork, [In, Out] int[] iwork, ref int liwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DSYEVR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern unsafe void _driver_dsyevr(ref char jobz, ref char range, ref char uplo, ref int n, [In, Out] double[] a, ref int lda, ref double vl, ref double vu, ref int il, ref int iu, ref double abstol, out int m, [In, Out] double[] w, [In, Out] double[] z, ref int ldz, [In, Out] int[] isuppz, double* work, ref int lwork, int* iwork, ref int liwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZHEEVR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _driver_zheevr(ref char jobz, ref char range, ref char uplo, ref int n, [In, Out] Complex[] a, ref int lda, ref double vl, ref double vu, ref int il, ref int iu, ref double abstol, out int m, [In, Out] double[] w, [In, Out] Complex[] z, ref int ldz, [In, Out] int[] isuppz, [In, Out] Complex[] work, ref int lwork, [In, Out] double[] rwork, ref int lrwork, [In, Out] int[] iwork, ref int liwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZHEEVR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern unsafe void _driver_zheevr(ref char jobz, ref char range, ref char uplo, ref int n, [In, Out] Complex[] a, ref int lda, ref double vl, ref double vu, ref int il, ref int iu, ref double abstol, out int m, [In, Out] double[] w, [In, Out] Complex[] z, ref int ldz, [In, Out] int[] isuppz, Complex* work, ref int lwork, double* rwork, ref int lrwork, int* iwork, ref int liwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DSPEV", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _driver_dspev(ref char jobz, ref char uplo, ref int n, [In, Out] double[] ap, [In, Out] double[] w, [In, Out] double[] z, ref int ldz, [In, Out] double[] work, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZHPEV", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurityAttribute]
        private static extern void _driver_zhpev(ref char jobz, ref char uplo, ref int n, [In, Out] Complex[] ap, [In, Out] double[] w, [In, Out] Complex[] z, ref int ldz, [In, Out] Complex[] work, [In, Out] double[] rwork, out int info);

        #endregion

        #region public methods

        /// <summary>Gets a optimal workspace array length for the <c>dsytrd</c> function.
        /// </summary>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>
        /// <returns>The optimal workspace array length.</returns>
        /// <remarks>The parameter <paramref name="triangularMatrixType"/> should not have an impact of the calculation of the optimal length of the workspace array.</remarks>
        public int dsytrdQuery(int n, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            var lwork = -1;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            unsafe
            {
                int info;
                double* work = stackalloc double[1];

                _dsytrd(ref uplo, ref n, null, ref n, null, null, null, work, ref lwork, out info);
                CheckForError(info, "dsytrd");

                return ((int)work[0]) + 1;
            }
        }

        /// <summary>Reduces a real symmetric matrix to tridiagonal form, i.e. A = Q * T * Q'.
        /// </summary>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="a">The symmetric matrix provided column-by-column, i.e. the length should be at least <paramref name="n"/>^2; on exit it will be overwritten by the tridiagonal form and details of the orthogonal matrix.</param>
        /// <param name="d">The diagonal elements of the tridiagonal matrix; the array should have at least <paramref name="n"/> elements (output).</param>
        /// <param name="e">The off-diagonal elements of the tridiagonal matrix; the array should have at least <paramref name="n"/> - 1 elements (output).</param>
        /// <param name="tau">Further details of the orthogonal matrix in the first <paramref name="n"/> - 1 elements, tau[n] is used as workspace; the array should have at least <paramref name="n"/> elements (output).</param>
        /// <param name="work">A workspace array with at least <paramref name="n"/> elements.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>
        public void dsytrd(int n, double[] a, double[] d, double[] e, double[] tau, double[] work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            var uplo = LAPACK.GetUplo(triangularMatrixType);
            int lwork = work.Length;
            int info;

            _dsytrd(ref uplo, ref n, a, ref n, d, e, tau, work, ref lwork, out info);
            CheckForError(info, "dsytrd");
        }

        /// <summary>Gets a optimal workspace array length for the <c>dorgtr</c> function.
        /// </summary>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>
        /// <returns>The optimal workspace array length.</returns>
        /// <remarks>The parameter <paramref name="triangularMatrixType"/> should not have an impact of the calculation of the optimal length of the workspace array.</remarks>
        public int dorgtrQuery(int n, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            var lwork = -1;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            unsafe
            {
                int info;
                double* work = stackalloc double[1];

                _dorgtr(ref uplo, ref n, null, ref n, null, work, ref lwork, out info);
                CheckForError(info, "dorgtr");

                return ((int)work[0]) + 1;
            }
        }

        /// <summary>Generates the real orthogonal matrix Q determined by <c>dsytrd</c>.
        /// </summary>
        /// <param name="n">The order of matrix Q.</param>
        /// <param name="a">This parameter should be the output of <c>dsytrd</c>; will be overwritten by the orthogonal matrix Q.</param>
        /// <param name="tau">This parameter should be the output of <c>dsytrd</c>. </param>
        /// <param name="work">A workspace array.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>
        /// <remarks>The routine explicitly generates the n-by-n orthogonal matrix Q formed by dsytrd when reducing a real symmetric matrix A to tridiagonal form A = Q * T * Q'. Use this routine after a call to <c>dsytrd</c>.</remarks>
        public void dorgtr(int n, double[] a, double[] tau, double[] work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            int lwork = work.Length;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _dorgtr(ref uplo, ref n, a, ref n, tau, work, ref lwork, out info);
            CheckForError(info, "dorgtr");
        }

        /// <summary>Gets a optimal workspace array length for the <c>dormtr</c> function.
        /// </summary>
        /// <param name="m">The number of rows in matrix C.</param>
        /// <param name="n">The number of columns in matrix C.</param>
        /// <param name="side">A value indicating whether op(Q) is applied to matrix C from the left or from the right.</param>
        /// <param name="transposeState">A value indicating whether the routine multiplies C by Q or Q'.</param>
        /// <param name="triangularMatrixType">Use the same parameter as supplied to <c>dsytrd</c>.</param>
        /// <returns>The optimal workspace array length.</returns>
        public int dormtrQuery(int m, int n, LAPACK.Side side = LAPACK.Side.Left, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            var lwork = -1;
            var sidez = LAPACK.GetSide(side);
            var trans = LAPACK.GetTrans(transposeState);
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            unsafe
            {
                int info;
                double* work = stackalloc double[1];

                _dormtr(ref sidez, ref uplo, ref trans, ref m, ref n, null, ref n, null, null, ref n, work, ref lwork, out info);
                CheckForError(info, "dormtr");

                return ((int)work[0]) + 1;
            }
        }

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
        public void dormtr(int m, int n, double[] a, double[] tau, double[] c, double[] work, LAPACK.Side side, BLAS.MatrixTransposeState transposeState, BLAS.TriangularMatrixType triangularMatrixType)
        {
            int info;
            var lwork = work.Length;
            var sidez = LAPACK.GetSide(side);
            var trans = LAPACK.GetTrans(transposeState);
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _dormtr(ref sidez, ref uplo, ref trans, ref m, ref n, a, ref n, tau, c, ref n, work, ref lwork, out info);
            CheckForError(info, "dormtr");
        }

        /// <summary>Gets a optimal workspace array length for the <c>zhetrd</c> function.
        /// </summary>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>
        /// <returns>The optimal workspace array length.</returns>
        /// <remarks>The parameter <paramref name="triangularMatrixType"/> should not have an impact of the calculation of the optimal length of the workspace array.</remarks>
        public int zhetrdQuery(int n, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            var lwork = -1;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            unsafe
            {
                int info;
                Complex* work = stackalloc Complex[1];

                _zhetrd(ref uplo, ref n, null, ref n, null, null, null, work, ref lwork, out info);
                CheckForError(info, "zhetrd");

                return ((int)work[0].Real) + 1;
            }
        }

        /// <summary>Reduces a complex Hermitian matrix to tridiagonal form, i.e. A = Q * T * Q^H.       
        /// </summary>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="a">The Hermitian matrix provided column-by-column; on exit it will be overwritten by the tridiagonal form and details of the orthogonal matrix.</param>
        /// <param name="d">The diagonal elements of the tridiagonal matrix; the array should have at least <paramref name="n"/> elements (output).</param>
        /// <param name="e">The off-diagonal elements of the tridiagonal matrix; the array should have at least <paramref name="n"/> - 1 elements (output).</param>
        /// <param name="tau">Further details of the orthogonal matrix in the first <paramref name="n"/> - 1 elements; the array should have at least <paramref name="n"/> - 1 elements (output).</param>
        /// <param name="work">A workspace array.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the Hermitian input matrix is stored.</param>              
        public void zhetrd(int n, Complex[] a, double[] d, double[] e, Complex[] tau, Complex[] work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var uplo = LAPACK.GetUplo(triangularMatrixType);
            int lwork = work.Length;

            _zhetrd(ref uplo, ref n, a, ref n, d, e, tau, work, ref lwork, out info);
            CheckForError(info, "zhetrd");
        }

        /// <summary>Gets a optimal workspace array length for the <c>zungtr</c> function.
        /// </summary>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="triangularMatrixType">Should be the same as supplied to <c>zhetrd</c>.</param>
        /// <returns>The optimal workspace array length.</returns>
        /// <remarks>The parameter <paramref name="triangularMatrixType"/> should not have an impact of the calculation of the optimal length of the workspace array.</remarks>
        public int zungtrQuery(int n, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            var lwork = -1;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            unsafe
            {
                int info;
                Complex* work = stackalloc Complex[1];

                _zungtr(ref uplo, ref n, null, ref n, null, work, ref lwork, out info);
                CheckForError(info, "zungtr");

                return ((int)work[0].Real) + 1;
            }
        }

        /// <summary>Generates the complex unitary matrix Q determined by <c>zhetrd</c>.
        /// </summary>
        /// <param name="n">The order of the matrix Q.</param>
        /// <param name="a">This parameter should be the array returned by <c>zhetrd</c>.</param>
        /// <param name="tau">This parameter should be the array returned by <c>zhetrd</c>.</param>
        /// <param name="work">A workspace array.</param>
        /// <param name="triangularMatrixType">Should be the same as supplied to <c>zhetrd</c>.</param>
        /// <remarks>The routine explicitly generates the n-by-n unitary matrix Q formed by <c>zhetrd</c> when reducing a complex Hermitian matrix A to tridiagonal form A = Q * T * Q^H. Use this routine after a call to <c>zhetrd</c>.</remarks>
        public void zungtr(int n, Complex[] a, Complex[] tau, Complex[] work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            int lwork = work.Length;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _zungtr(ref uplo, ref n, a, ref n, tau, work, ref lwork, out info);
            CheckForError(info, "zungtr");
        }

        /// <summary>Gets a optimal workspace array length for the <c>zunmtr</c> function.
        /// </summary>
        /// <param name="m">The number of rows in matrix C.</param>
        /// <param name="n">The number of columns in matrix C.</param>
        /// <param name="side">A value indicating whether op(Q) is applied to matrix C from the left or from the right.</param>
        /// <param name="transposeState">A value indicating whether the routine multiplies C by Q or Q'.</param>
        /// <param name="triangularMatrixType">Use the same parameter as supplied to <c>zhetrd</c>.</param>
        /// <returns>The optimal workspace array length.</returns>
        public int zunmtrQuery(int m, int n, LAPACK.Side side = LAPACK.Side.Left, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            var lwork = -1;
            var sidez = LAPACK.GetSide(side);
            var trans = LAPACK.GetTrans(transposeState);
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            unsafe
            {
                int info;
                Complex* work = stackalloc Complex[1];

                _zunmtr(ref sidez, ref uplo, ref trans, ref m, ref n, null, ref n, null, null, ref n, work, ref lwork, out info);
                CheckForError(info, "zunmtr");

                return ((int)work[0].Real) + 1;
            }
        }

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
        public void zunmtr(int m, int n, Complex[] a, Complex[] tau, Complex[] c, Complex[] work, LAPACK.Side side, BLAS.MatrixTransposeState transposeState, BLAS.TriangularMatrixType triangularMatrixType)
        {
            int info;
            var lwork = work.Length;
            var sidez = LAPACK.GetSide(side);
            var trans = LAPACK.GetTrans(transposeState);
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            int r = (side == LAPACK.Side.Left) ? m : n;

            _zunmtr(ref sidez, ref uplo, ref trans, ref m, ref n, a, ref r, tau, c, ref n, work, ref lwork, out info);
            CheckForError(info, "zunmtr");
        }

        /// <summary>Reduces a real symmetric matrix to tridiagonal form using packed storage, i.e. a packed real symmetric matrix A is transformed to symmetric tridiagonal form T by an orthogonal similarity transformation A = Q * T * Q'.
        /// </summary>
        /// <param name="n">The order of the specified symmetric matrix.</param>
        /// <param name="ap">The specified symmetric matrix in packed form, i.e. either upper or lower triangle as specified in <paramref name="triangularMatrixType"/> with at least <paramref name="n"/> * (<paramref name="n"/> + 1) / 2 elements.</param>
        /// <param name="d">The diagonal elements of the tridiagonal matrix; the array should have at least <paramref name="n"/> elements (output).</param>
        /// <param name="e">The off-diagonal elements of the tridiagonal matrix; the array should have at least <paramref name="n"/> - 1 elements (output).</param>
        /// <param name="tau">Further details of the orthogonal matrix in the first <paramref name="n"/> - 1 elements; the array should have at least <paramref name="n"/> - 1 elements (output).</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>
        public void dsptrd(int n, double[] ap, double[] d, double[] e, double[] tau, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _dsptrd(ref uplo, ref n, ap, d, e, tau, out info);
            CheckForError(info, "dsptrd");
        }

        /// <summary>Generates the real orthogonal matrix Q determined by <c>dsptrd</c>.
        /// </summary>
        /// <param name="n">The order of matrix Q.</param>
        /// <param name="ap">This parameter should be the output of <c>dsptrd</c>.</param>
        /// <param name="tau">This parameter should be the output of <c>dsptrd</c>.</param>
        /// <param name="q">Contains the computed matrix Q of dimension <paramref name="n"/> x <paramref name="n"/>, provided column-by-column (output).</param>
        /// <param name="work">A workspace array with at least <paramref name="n"/> - 1 elements.</param>
        /// <param name="triangularMatrixType">Use the same parameter as supplied to <c>dsptrd</c>.</param>
        public void dopgtr(int n, double[] ap, double[] tau, double[] q, double[] work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _dopgtr(ref uplo, ref n, ap, tau, q, ref n, work, out info);
            CheckForError(info, "dopgtr");
        }

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
        public void dopmtr(int m, int n, double[] ap, double[] tau, double[] c, double[] work, LAPACK.Side side = LAPACK.Side.Left, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var lwork = work.Length;
            var sidez = LAPACK.GetSide(side);
            var trans = LAPACK.GetTrans(transposeState);
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _dopmtr(ref sidez, ref uplo, ref trans, ref m, ref n, ap, tau, c, ref n, work, out info);
            CheckForError(info, "dopmtr");
        }

        /// <summary>Reduces a complex Hermitian matrix to tridiagonal form T by a unitary similarity transformation A = Q * T *Q^H using packed storage.
        /// </summary>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="ap">The Hermitian matrix, i.e. either upper or lower triangle in packed form; on exit it will be overwritten by the tridiagonal matrix T and details of the orthogonal matrix Q. This parameter should have a length of at least <paramref name="n"/> * ( <paramref name="n"/> + 1) / 2.</param>
        /// <param name="d">The diagonal elements of the tridiagonal matrix; the array should have at least <paramref name="n"/> elements (output).</param>
        /// <param name="e">The off-diagonal elements of the tridiagonal matrix; the array should have at least <paramref name="n"/> - 1 elements (output).</param>
        /// <param name="tau">Further details of the orthogonal matrix in the first <paramref name="n"/> - 1 elements; the array should have at least <paramref name="n"/> - 1 elements (output).</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the Hermitian input matrix is stored.</param>        
        public void zhptrd(int n, Complex[] ap, double[] d, double[] e, Complex[] tau, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _zhptrd(ref uplo, ref n, ap, d, e, tau, out info);
            CheckForError(info, "zhptrd");
        }

        /// <summary>Generates the complex unitary matrix Q determined by <c>zhptrd</c>.
        /// </summary>
        /// <param name="n">The order of the matrix Q.</param>
        /// <param name="ap">This parameter should be the array returned by <c>zhptrd</c>.</param>
        /// <param name="tau">This parameter should be the array returned by <c>zhptrd</c>.</param>
        /// <param name="q">The computed matrix Q provided column-by-column of dimension n x n (output).</param>
        /// <param name="work">A workspace array with at least <paramref name="n"/> -1 elements.</param>
        /// <param name="triangularMatrixType">Should be the same as supplied to <c>zhptrd</c>.</param>
        public void zupgtr(int n, Complex[] ap, Complex[] tau, Complex[] q, Complex[] work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _zupgtr(ref uplo, ref n, ap, tau, q, ref n, work, out info);
            CheckForError(info, "zupgtr");
        }

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
        public void zupmtr(int m, int n, Complex[] ap, Complex[] tau, Complex[] c, Complex[] work, LAPACK.Side side = LAPACK.Side.Left, BLAS.MatrixTransposeState transposeState = BLAS.MatrixTransposeState.NoTranspose, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var sidez = LAPACK.GetSide(side);
            var trans = LAPACK.GetTrans(transposeState);
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _zupmtr(ref sidez, ref uplo, ref trans, ref m, ref n, ap, tau, c, ref n, work, out info);

            CheckForError(info, "zupmtr");
        }

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
        public void dsbtrd(LapackEigenvalues.SymmetricXxbtrdJob job, int n, int kd, double[] ab, double[] d, double[] e, double[] q, double[] work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var jobz = GetJob(job);
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            int ldab = kd + 1;
            int ldq = (job == LapackEigenvalues.SymmetricXxbtrdJob.CalculateMatrixQ) ? n : 1;

            _dsbtrd(ref jobz, ref uplo, ref n, ref kd, ab, ref ldab, d, e, q, ref ldq, work, out info);
            CheckForError(info, "dsbtrd");
        }

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
        public void zhbtrd(LapackEigenvalues.SymmetricXxbtrdJob job, int n, int kd, Complex[] ab, double[] d, double[] e, Complex[] q, Complex[] work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var uplo = LAPACK.GetUplo(triangularMatrixType);
            var jobz = GetJob(job);

            int ldab = kd + 1;
            int ldq = (job == LapackEigenvalues.SymmetricXxbtrdJob.CalculateMatrixQ) ? n : 1;

            _zhbtrd(ref jobz, ref uplo, ref n, ref kd, ab, ref ldab, d, e, q, ref ldq, work, out info);
            CheckForError(info, "zhbtrd");
        }

        /// <summary>Computes all eigenvalues of a real symmetric tridiagonal matrix T (which can be obtained by reducing a symmetric or Hermitian matrix to tridiagonal form) using QR algorithm.
        /// </summary>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="d">Contails the diagonal element of the specified tridiagonal matrix T, i.e. the array should have at least <paramref name="n"/> elements; on exit overwritten by the <paramref name="n"/> eigenvalues in ascending order.</param>
        /// <param name="e">Contains the off-diagonal elements of the specified tridiagonal matrix T, i.e. the array should have at least <paramref name="n"/> -1 elements; will be overwritten on exit.</param>
        public void dsterf(int n, double[] d, double[] e)
        {
            int info;
            _dsterf(ref n, d, e, out info);
            CheckForError(info, "dsterf");
        }

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
        public void dsteqr(LapackEigenvalues.SymmetricJob job, int n, double[] d, double[] e, double[] z, double[] work)
        {
            int info;
            var compz = GetJob(job);
            _dsteqr(ref compz, ref n, d, e, z, ref n, work, out info);
            CheckForError(info, "dsteqr");
        }

        /// <summary>Computes all eigenvalues and eigenvectors of a Hermitian matrix reduced to tridiagonal form (QR algorithm)
        /// </summary>
        /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="n">The order of the tridiagonal matrix T.</param>
        /// <param name="d">The diagonal elements of the tridiagonal matrix, i.e. at least <paramref name="n"/> elements; overwritten by the <paramref name="n"/> eigenvalues in ascending order.</param>
        /// <param name="e">The off-diagonal elements of the tridiagonal matrix T, i.e. at least <paramref name="n"/> -1 elements; overwritten on exit.</param>
        /// <param name="z">If <paramref name="job"/> indicates to take into account this parameter the n-by-n matrix Q on exit (output).</param>
        /// <param name="work">A workspace array with at least 2 * <paramref name="n"/> -2 elements if <paramref name="job"/> indicates to calculate eigenvalues and eigenvectors; otherwise at least 1 element.</param>
        /// <remarks>Before calling <c>zsteqr</c>, you must reduce A to tridiagonal form and generate the explicit matrix Q by calling on specific LAPACK routines.</remarks>
        public void zsteqr(LapackEigenvalues.SymmetricJob job, int n, double[] d, double[] e, Complex[] z, double[] work)
        {
            int info;
            var compz = GetJob(job);

            _zsteqr(ref compz, ref n, d, e, z, ref n, work, out info);
            CheckForError(info, "zsteqr");
        }

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
        public void dstemrQuery(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, double vl, double vu, int il, int iu, out int lwork, out int liwork, out int nzc)
        {
            int info;
            lwork = liwork = nzc = -1;
            var jobz = GetJob(job);
            var rangez = GetRange(range);
            int ldz = (job == LapackEigenvalues.SymmetricGeneralJob.All) ? n : 1;

            unsafe
            {
                double* work = stackalloc double[1];
                int* iwork = stackalloc int[1];

                int m;
                bool tryrac = true;
                _dstemr(ref jobz, ref rangez, ref n, null, null, ref vl, ref vu, ref il, ref iu, out m, null, null, ref  ldz, ref nzc, null, ref tryrac, work, ref lwork, iwork, ref liwork, out info);
                CheckForError(info, "dstemr");

                lwork = ((int)work[0]) + 1;
                liwork = iwork[0];
            }
        }

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
        public void dstemr(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, double[] d, double[] e, double vl, double vu, int il, int iu, out int m, double[] w, double[] z, int nzc, int[] isuppz, double[] work, int[] iwork, bool tryrac = true)
        {
            int info;
            var jobz = GetJob(job);
            var rangez = GetRange(range);

            int lwork = work.Length;
            int liwork = iwork.Length;
            int ldz = (job == LapackEigenvalues.SymmetricGeneralJob.All) ? n : 1;
            _dstemr(ref jobz, ref rangez, ref n, d, e, ref vl, ref vu, ref il, ref iu, out m, w, z, ref ldz, ref nzc, isuppz, ref tryrac, work, ref lwork, iwork, ref liwork, out info);
            CheckForError(info, "dstemr");
        }

        /// <summary>Gets a optimal workspace array length for the <c>zstemr</c> function.
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
        public void zstemrQuery(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, double vl, double vu, int il, int iu, out int lwork, out int liwork, out int nzc)
        {
            int info;
            lwork = liwork = nzc = -1;
            var jobz = GetJob(job);
            var rangez = GetRange(range);
            int ldz = (job == LapackEigenvalues.SymmetricGeneralJob.All) ? n : 1;

            unsafe
            {
                double* work = stackalloc double[1];
                int* iwork = stackalloc int[1];

                int m;
                bool tryrac = true;
                _zstemr(ref jobz, ref rangez, ref n, null, null, ref vl, ref vu, ref il, ref iu, out m, null, null, ref  ldz, ref nzc, null, ref tryrac, work, ref lwork, iwork, ref liwork, out info);
                CheckForError(info, "zstemr");

                lwork = ((int)work[0]) + 1;
                liwork = iwork[0];
            }
        }

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
        public void zstemr(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, double[] d, double[] e, double vl, double vu, int il, int iu, out int m, double[] w, Complex[] z, int nzc, int[] isuppz, double[] work, int[] iwork, bool tryrac = true)
        {
            int info;
            var jobz = GetJob(job);
            var rangez = GetRange(range);

            int lwork = work.Length;
            int liwork = iwork.Length;
            int ldz = (job == LapackEigenvalues.SymmetricGeneralJob.All) ? n : 1;
            _zstemr(ref jobz, ref rangez, ref n, d, e, ref vl, ref vu, ref il, ref iu, out m, w, z, ref ldz, ref nzc, isuppz, ref tryrac, work, ref lwork, iwork, ref liwork, out info);
            CheckForError(info, "zstemr");
        }

        /// <summary>Gets a optimal workspace array length for the <c>dstedc</c> function.
        /// </summary>
        /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="n">The order of the symmetric tridiagonal matrix.</param>
        /// <param name="liwork">The optimal workspace array length for parameter 'work'.</param>
        /// <param name="lwork">The optimal workspace array length for parameter 'iwork'.</param>
        public void dstedcQuery(LapackEigenvalues.SymmetricJob job, int n, out int lwork, out int liwork)
        {
            lwork = -1;
            var jobz = GetJob(job);
            int ldz = (job == LapackEigenvalues.SymmetricJob.EigenValuesOnly) ? 1 : n;

            unsafe
            {
                double* work = stackalloc double[1];
                int* iwork = stackalloc int[1];

                int info;
                _dstedc(ref jobz, ref n, null, null, null, ref ldz, work, ref lwork, iwork, ref lwork, out info);
                CheckForError(info, "dstedc");

                lwork = ((int)work[0]) + 1;
                liwork = iwork[0];
            }
        }

        /// <summary>Computes all eigenvalues and eigenvectors of a symmetric tridiagonal matrix using the divide and conquer method.
        /// </summary>
        /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="n">The order of the symmetric tridiagonal matrix.</param>
        /// <param name="d">The diagonal elements of the tridiagonal matrix, i.e. at least <paramref name="n"/> elements; will be overwritten by the <paramref name="n"/> eigenvalues in ascending order (output).</param>
        /// <param name="e">The off-diagonal elements of the tridiagonal matrix in the first <paramref name="n"/> - 1 elements; will be overwritten.</param>
        /// <param name="z">The orthogonal matrix used to reduce the original matrix to tridiagonal form; on exist contains the orthonormal eigenvectors (output).</param>
        /// <param name="work">A workspace array.</param>
        /// <param name="iwork">A workspace array.</param>
        public void dstedc(LapackEigenvalues.SymmetricJob job, int n, double[] d, double[] e, double[] z, double[] work, int[] iwork)
        {
            int info;
            var jobz = GetJob(job);

            int lwork = work.Length;
            int liwork = iwork.Length;
            int ldz = (job == LapackEigenvalues.SymmetricJob.EigenValuesOnly) ? 1 : n;

            _dstedc(ref jobz, ref n, d, e, z, ref ldz, work, ref lwork, iwork, ref liwork, out info);
            CheckForError(info, "dstedc");
        }

        /// <summary>Gets a optimal workspace array length for the <c>zstedc</c> function.
        /// </summary>
        /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="n">The order of the symmetric tridiagonal matrix.</param>
        /// <param name="liwork">The optimal workspace array length for parameter 'work'.</param>
        /// <param name="lwork">The optimal workspace array length for parameter 'iwork'.</param>
        /// <param name="lrwork">The optimal workspace array length for parameter 'rwork'.</param>
        public void zstedcQuery(LapackEigenvalues.SymmetricJob job, int n, out int lwork, out int liwork, out int lrwork)
        {
            lwork = -1;
            var jobz = GetJob(job);
            int ldz = (job == LapackEigenvalues.SymmetricJob.EigenValuesOnly) ? 1 : n;

            unsafe
            {
                Complex* work = stackalloc Complex[1];
                double* rwork = stackalloc double[1];
                int* iwork = stackalloc int[1];

                int info;
                _zstedc(ref jobz, ref n, null, null, null, ref ldz, work, ref lwork, rwork, ref lwork, iwork, ref lwork, out info);
                CheckForError(info, "zstedc");

                lwork = ((int)work[0].Real) + 1;
                liwork = iwork[0];
                lrwork = ((int)rwork[0]) + 1;
            }
        }

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
        public void zstedc(LapackEigenvalues.SymmetricJob job, int n, double[] d, double[] e, Complex[] z, Complex[] work, int[] iwork, double[] rwork)
        {
            int info;
            var jobz = GetJob(job);

            int lwork = work.Length;
            int liwork = iwork.Length;
            int lrwork = rwork.Length;
            int ldz = (job == LapackEigenvalues.SymmetricJob.EigenValuesOnly) ? 1 : n;

            _zstedc(ref jobz, ref n, d, e, z, ref ldz, work, ref lwork, rwork, ref lrwork, iwork, ref liwork, out info);
            CheckForError(info, "zstedc");
        }

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
        public void dstegr(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, double[] d, double[] e, double vl, double vu, int il, int iu, out int m, double[] w, double[] z, int[] isuppz, double[] work, int[] iwork)
        {
            int info;
            char jobz = GetJob(job);
            char rangez = GetRange(range);

            int lwork = work.Length;
            int liwork = iwork.Length;
            double abstol = 1E-6; // this parameter is not longer needed, we use some dummy value
            int ldz = (job == LapackEigenvalues.SymmetricGeneralJob.EigenValuesOnly) ? 1 : n;

            _dstegr(ref jobz, ref rangez, ref n, d, e, ref vl, ref vu, ref il, ref iu, ref abstol, out m, w, z, ref ldz, isuppz, work, ref lwork, iwork, ref liwork, out info);
            CheckForError(info, "dstegr");
        }

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
        public void zstegr(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, double[] d, double[] e, double vl, double vu, int il, int iu, out int m, double[] w, Complex[] z, int[] isuppz, double[] work, int[] iwork)
        {
            int info;
            char jobz = GetJob(job);
            char rangez = GetRange(range);

            int lwork = work.Length;
            int liwork = iwork.Length;
            double abstol = 1E-6; // this parameter is not longer needed, we use some dummy value
            int ldz = (job == LapackEigenvalues.SymmetricGeneralJob.EigenValuesOnly) ? 1 : n;

            _zstegr(ref jobz, ref rangez, ref n, d, e, ref vl, ref vu, ref il, ref iu, ref abstol, out m, w, z, ref ldz, isuppz, work, ref lwork, iwork, ref liwork, out info);
            CheckForError(info, "zstegr");
        }

        /// <summary>Computes all eigenvalues and (optionally) all eigenvectors of a real symmetric positive-definite tridiagonal matrix.
        /// </summary>
        /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="n">The order of the tridiagonal matrix T.</param>
        /// <param name="d">The diagonal elements of the tridiagonal matrix, i.e. at least <paramref name="n"/> elements; on exit the <paramref name="n"/> eigenvalues in descending order.</param>
        /// <param name="e">The off-diagonal elements of the tridiagonal matrix T, i.e. at least <paramref name="n"/> -1 elements.</param>
        /// <param name="z">If <paramref name="job"/> indicates to take into account this parameter the n-by-n matrix Q on exit.</param>
        /// <param name="work">A workspace array with at least 4 * <paramref name="n"/> -4 elements.</param>
        public void dpteqr(LapackEigenvalues.SymmetricJob job, int n, double[] d, double[] e, double[] z, double[] work)
        {
            int info;
            var jobz = GetJob(job);
            _dpteqr(ref jobz, ref n, d, e, z, ref n, work, out info);
            CheckForError(info, "dpteqr");
        }

        /// <summary>Computes all eigenvalues and (optionally) all eigenvectors of a real symmetric positive-definite tridiagonal matrix.
        /// </summary>
        /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="n">The order of the tridiagonal matrix T.</param>
        /// <param name="d">The diagonal elements of the tridiagonal matrix, i.e. at least <paramref name="n"/> elements; on exit the <paramref name="n"/> eigenvalues in descending order.</param>
        /// <param name="e">The off-diagonal elements of the tridiagonal matrix T, i.e. at least <paramref name="n"/> -1 elements.</param>
        /// <param name="z">If <paramref name="job"/> indicates to take into account this parameter the n-by-n matrix Q on exit.</param>
        /// <param name="work">A workspace array with at least 4 * <paramref name="n"/> -4 elements.</param>
        public void zpteqr(LapackEigenvalues.SymmetricJob job, int n, double[] d, double[] e, Complex[] z, double[] work)
        {
            int info;
            var jobz = GetJob(job);
            _zpteqr(ref jobz, ref n, d, e, z, ref n, work, out info);
            CheckForError(info, "zpteqr");
        }

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
        public void dstebz(LapackEigenvalues.SymmetricEigenvaluesRange range, LapackEigenvalues.SymmetricDstebzOrder order, int n, double[] d, double[] e, double vl, double vu, int il, int iu, out int m, double[] w, out int nsplit, int[] iblock, int[] isplit, double[] work, double abstol = MachineConsts.Epsilon)
        {
            int info;
            char rangez = GetRange(range);
            char orderz = GetOrder(order);

            int lwork = work.Length;

            _dstebz(ref rangez, ref orderz, ref n, ref vl, ref vu, ref il, ref iu, ref abstol, d, e, out m, out nsplit, w, iblock, isplit, work, ref lwork, out info);
            CheckForError(info, "dstebz");
        }

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
        public void dstein(int n, double[] d, double[] e, int m, double[] w, int[] iblock, int[] isplit, double[] z, int[] ifailv, double[] work, int[] iwork)
        {
            int info;
            _dstein(ref n, d, e, ref m, w, iblock, isplit, z, ref n, work, iwork, ifailv, out info);
            CheckForError(info, "dstein");
        }

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
        public void zstein(int n, double[] d, double[] e, int m, double[] w, int[] iblock, int[] isplit, Complex[] z, int[] ifailv, double[] work, int[] iwork)
        {
            int info;
            _zstein(ref n, d, e, ref m, w, iblock, isplit, z, ref n, work, iwork, ifailv, out info);
            CheckForError(info, "zstein");
        }

        /// <summary>Computes the reciprocal condition numbers for the eigenvectors of a symmetric/ Hermitian matrix or for the left or right singular vectors of a general matrix.
        /// </summary>
        /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="m">The number of rows of the matrix.</param>
        /// <param name="n">The number of columns of the matrix.</param>
        /// <param name="d">Contains the eigenvalues or singular values of the matrix, in either increasing or decreasing order.</param>
        /// <param name="sep">The reciprocal condition numbers of the vectors (output).</param>
        public void ddisna(LapackEigenvalues.SymmetricDdisnaJob job, int m, int n, double[] d, double[] sep)
        {
            int info;
            char jobz = GetJob(job);

            _ddisna(ref jobz, ref m, ref n, d, sep, out info);
            CheckForError(info, "ddisna");
        }

        /// <summary>Gets a optimal workspace array length for the <c>driver_dsyev</c> function.
        /// </summary>
        /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>
        /// <returns>The optimal workspace array length.</returns>
        /// <remarks>The parameter <paramref name="triangularMatrixType"/> should not have an impact of the calculation of the optimal length of the workspace array.</remarks>
        public int driver_dsyevQuery(LapackEigenvalues.SymmetricGeneralJob job, int n, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            var lwork = -1;
            var jobz = GetJob(job);
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            unsafe
            {
                double* work = stackalloc double[1];

                int info;
                _driver_dsyev(ref jobz, ref uplo, ref n, null, ref n, null, work, ref lwork, out info);
                CheckForError(info, "driver_dsyev");

                return ((int)work[0]) + 1;
            }
        }

        /// <summary>Computes all eigenvalues and, optionally, eigenvectors of a real symmetric matrix.
        /// </summary>
        /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="a">The symmetric matrix, i.e. containing either upper or lower triangular part of the symmetric matrix, as specified by <paramref name="triangularMatrixType"/>. The length should be at least <paramref name="n"/>^2; overwritten on exit, perhaps by the orthonormal eigenvectors.</param>
        /// <param name="w">The eigenvalues of <paramref name="a"/> in ascending order, i.e. the array should have at least <paramref name="n"/> elements (output).</param>
        /// <param name="work">A workspace array with at least 3 * <paramref name="n"/> - 1 elements.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>
        public void driver_dsyev(LapackEigenvalues.SymmetricGeneralJob job, int n, double[] a, double[] w, double[] work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var jobz = GetJob(job);
            int lwork = work.Length;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _driver_dsyev(ref jobz, ref uplo, ref n, a, ref n, w, work, ref lwork, out info);

            CheckForError(info, "driver_dsyev");
        }

        /// <summary>Gets a optimal workspace array length for the <c>driver_zheev</c> function.
        /// </summary>
        /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the Hermitian matrix is stored.</param>
        /// <returns>The optimal workspace array length.</returns>
        /// <remarks>The parameter <paramref name="triangularMatrixType"/> should not have an impact of the calculation of the optimal length of the workspace array.</remarks>
        public int driver_zheevQuery(LapackEigenvalues.SymmetricGeneralJob job, int n, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            var lwork = -1;
            var jobz = GetJob(job);
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            unsafe
            {
                Complex* work = stackalloc Complex[1];

                int info;
                _driver_zheev(ref jobz, ref uplo, ref n, null, ref n, null, work, ref lwork, out info);
                CheckForError(info, "driver_zheev");

                return ((int)work[0].Real) + 1;
            }
        }

        /// <summary>Computes all eigenvalues and, optionally, eigenvectors of a Hermitian matrix.
        /// </summary>
        /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="a">The Hermitian matrix, i.e. containing either upper or lower triangular part of the Hermitian matrix, as specified by <paramref name="triangularMatrixType"/>. The length should be at least <paramref name="n"/>^2; overwritten on exit, perhaps by the orthonormal eigenvectors.</param>
        /// <param name="w">The eigenvalues of <paramref name="a"/> in ascending order, i.e. the array should have at least <paramref name="n"/> elements (output).</param>
        /// <param name="work">A workspace array with at least 3 * <paramref name="n"/> - 1 elements.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>
        public void driver_zheev(LapackEigenvalues.SymmetricGeneralJob job, int n, Complex[] a, double[] w, Complex[] work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var jobz = GetJob(job);
            int lwork = work.Length;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _driver_zheev(ref jobz, ref uplo, ref n, a, ref n, w, work, ref lwork, out info);
            CheckForError(info, "driver_zheev");
        }

        /// <summary>Gets a optimal workspace array length for the <c>driver_dsyevd</c> function.
        /// </summary>
        /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="n">The order of the symmetric tridiagonal matrix.</param>
        /// <param name="liwork">The optimal workspace array length for parameter 'work'.</param>
        /// <param name="lwork">The optimal workspace array length for parameter 'iwork'.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>        
        /// <remarks>The parameter <paramref name="triangularMatrixType"/> should not have an impact of the calculation of the optimal length of the workspace array.</remarks>
        public void driver_dsyevdQuery(LapackEigenvalues.SymmetricGeneralJob job, int n, out int lwork, out int liwork, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            lwork = -1;
            liwork = -1;
            var jobz = GetJob(job);
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            unsafe
            {
                double* work = stackalloc double[1];
                int* iwork = stackalloc int[1];

                int info;
                _driver_dsyevd(ref jobz, ref uplo, ref n, null, ref n, null, work, ref lwork, iwork, ref liwork, out info);
                CheckForError(info, "driver_dsyevd");

                lwork = ((int)work[0]) + 1;
                liwork = iwork[0];
            }
        }

        /// <summary>Computes all eigenvalues and (optionally) all eigenvectors of a real symmetric matrix using divide and conquer algorithm.
        /// </summary>
        /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="a">The symmetric matrix, i.e. containing either upper or lower triangular part of the symmetric matrix, as specified by <paramref name="triangularMatrixType"/>. The length should be at least <paramref name="n"/>^2; overwritten on exit, perhaps by the orthonormal eigenvectors.</param>
        /// <param name="w">The eigenvalues of <paramref name="a"/> in ascending order, i.e. the array should have at least <paramref name="n"/> elements (output).</param>
        /// <param name="work">A workspace array.</param>
        /// <param name="iwork">A workspace array.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>
        public void driver_dsyevd(LapackEigenvalues.SymmetricGeneralJob job, int n, double[] a, double[] w, double[] work, int[] iwork, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var jobz = GetJob(job);
            int lwork = work.Length;
            int liwork = iwork.Length;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _driver_dsyevd(ref jobz, ref uplo, ref n, a, ref n, w, work, ref lwork, iwork, ref liwork, out info);
            CheckForError(info, "driver_dsyevd");
        }

        /// <summary>Gets a optimal workspace array length for the <c>driver_zheevd</c> function.
        /// </summary>
        /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="n">The order of the Hermitian matrix.</param>
        /// <param name="liwork">The optimal workspace array length for parameter 'work'.</param>
        /// <param name="lwork">The optimal workspace array length for parameter 'iwork'.</param>
        /// <param name="lrwork">The optimal workspace array length for parameter 'lrwork'.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the Hermitian input matrix is stored.</param>        
        /// <remarks>The parameter <paramref name="triangularMatrixType"/> should not have an impact of the calculation of the optimal length of the workspace array.</remarks>
        public void driver_zheevdQuery(LapackEigenvalues.SymmetricGeneralJob job, int n, out int lwork, out int liwork, out int lrwork, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            lwork = -1;
            liwork = -1;
            lrwork = -1;
            var jobz = GetJob(job);
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            unsafe
            {
                Complex* work = stackalloc Complex[1];
                double* rwork = stackalloc double[1];
                int* iwork = stackalloc int[1];


                int info;
                _driver_zheevd(ref jobz, ref uplo, ref n, null, ref n, null, work, ref lwork, rwork, ref lrwork, iwork, ref liwork, out info);
                CheckForError(info, "driver_zheevd");

                lwork = ((int)work[0].Real) + 1;
                lrwork = ((int)rwork[0]) + 1;
                liwork = iwork[0];
            }
        }

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
        public void driver_zheevd(LapackEigenvalues.SymmetricGeneralJob job, int n, Complex[] a, double[] w, Complex[] work, double[] rwork, int[] iwork, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var jobz = GetJob(job);
            int lwork = work.Length;
            int lrwork = rwork.Length;
            int liwork = iwork.Length;
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            _driver_zheevd(ref jobz, ref uplo, ref n, a, ref n, w, work, ref lwork, rwork, ref lrwork, iwork, ref liwork, out info);
            CheckForError(info, "driver_zheevd");
        }

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
        public int driver_dsyevxQuery(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, double vl, double vu, int il, int iu, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, double abstol = MachineConsts.Epsilon)
        {
            var lwork = -1;
            var jobz = GetJob(job);
            var rangez = GetRange(range);
            var uplo = LAPACK.GetUplo(triangularMatrixType);
            int ldz = (job == LapackEigenvalues.SymmetricGeneralJob.All) ? n : 1;

            int m;
            unsafe
            {
                double* work = stackalloc double[1];

                int info;
                _driver_dsyevx(ref jobz, ref rangez, ref uplo, ref n, null, ref n, ref vl, ref vu, ref il, ref iu, ref abstol, out m, null, null, ref ldz, work, ref lwork, null, null, out info);
                CheckForError(info, "driver_dsyevx");

                return ((int)work[0]) + 1;
            }
        }

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
        public void driver_dsyevx(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, double[] a, double vl, double vu, int il, int iu, out int m, double[] w, double[] z, int[] ifail, double[] work, int[] iwork, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, double abstol = MachineConsts.Epsilon)
        {
            int info;
            var jobz = GetJob(job);
            int lwork = work.Length;
            int liwork = iwork.Length;
            var rangez = GetRange(range);
            var uplo = LAPACK.GetUplo(triangularMatrixType);
            int ldz = (job == LapackEigenvalues.SymmetricGeneralJob.All) ? n : 1;

            _driver_dsyevx(ref jobz, ref rangez, ref uplo, ref n, a, ref n, ref vl, ref vu, ref il, ref iu, ref abstol, out m, w, z, ref ldz, work, ref lwork, iwork, ifail, out info);
            CheckForError(info, "driver_dsyevx");
        }

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
        public int driver_zheevxQuery(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, double vl, double vu, int il, int iu, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, double abstol = MachineConsts.Epsilon)
        {
            var lwork = -1;
            var jobz = GetJob(job);
            var rangez = GetRange(range);
            var uplo = LAPACK.GetUplo(triangularMatrixType);
            int ldz = (job == LapackEigenvalues.SymmetricGeneralJob.All) ? n : 1;

            int m;
            unsafe
            {
                Complex* work = stackalloc Complex[1];

                int info;
                _driver_zheevx(ref jobz, ref rangez, ref uplo, ref n, null, ref n, ref vl, ref vu, ref il, ref iu, ref abstol, out m, null, null, ref ldz, work, ref lwork, null, null, null, out info);
                CheckForError(info, "driver_zheevx");

                return ((int)work[0].Real) + 1;
            }
        }

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
        public void driver_zheevx(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, Complex[] a, double vl, double vu, int il, int iu, out int m, double[] w, Complex[] z, int[] ifail, Complex[] work, double[] rwork, int[] iwork, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, double abstol = MachineConsts.Epsilon)
        {
            int info;
            var jobz = GetJob(job);
            int lwork = work.Length;
            int liwork = iwork.Length;
            var rangez = GetRange(range);
            var uplo = LAPACK.GetUplo(triangularMatrixType);
            int ldz = (job == LapackEigenvalues.SymmetricGeneralJob.All) ? n : 1;

            _driver_zheevx(ref jobz, ref rangez, ref uplo, ref n, a, ref n, ref vl, ref vu, ref il, ref iu, ref abstol, out m, w, z, ref ldz, work, ref lwork, rwork, iwork, ifail, out info);
            CheckForError(info, "driver_zheevx");
        }

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
        public void driver_dsyevrQuery(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, double vl, double vu, int il, int iu, out int lwork, out int liwork, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, double abstol = MachineConsts.Epsilon)
        {
            lwork = -1;
            liwork = -1;
            var jobz = GetJob(job);
            var rangez = GetRange(range);
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            unsafe
            {
                double* work = stackalloc double[1];
                int* iwork = stackalloc int[1];

                int m;
                int ldz = (job == LapackEigenvalues.SymmetricGeneralJob.All) ? n : 1;

                int info;
                _driver_dsyevr(ref jobz, ref rangez, ref uplo, ref n, null, ref n, ref vl, ref vu, ref il, ref iu, ref abstol, out m, null, null, ref ldz, null, work, ref lwork, iwork, ref liwork, out info);
                CheckForError(info, "driver_dsyevr");

                lwork = ((int)work[0]) + 1;
                liwork = iwork[0];
            }
        }

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
        public void driver_dsyevr(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, double[] a, double vl, double vu, int il, int iu, out int m, double[] w, double[] z, int[] isuppz, double[] work, int[] iwork, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, double abstol = MachineConsts.Epsilon)
        {
            int info;
            var jobz = GetJob(job);
            int lwork = work.Length;
            int liwork = iwork.Length;
            var rangez = GetRange(range);
            var uplo = LAPACK.GetUplo(triangularMatrixType);
            int ldz = (job == LapackEigenvalues.SymmetricGeneralJob.All) ? n : 1;

            _driver_dsyevr(ref jobz, ref rangez, ref uplo, ref n, a, ref n, ref vl, ref vu, ref il, ref iu, ref abstol, out m, w, z, ref ldz, isuppz, work, ref lwork, iwork, ref liwork, out info);
            CheckForError(info, "driver_dsyevr");
        }

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
        public void driver_zheevrQuery(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, double vl, double vu, int il, int iu, out int lwork, out int lrwork, out int liwork, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, double abstol = MachineConsts.Epsilon)
        {
            lwork = -1;
            liwork = -1;
            lrwork = -1;
            var jobz = GetJob(job);
            var rangez = GetRange(range);
            var uplo = LAPACK.GetUplo(triangularMatrixType);

            unsafe
            {
                Complex* work = stackalloc Complex[1];
                double* rwork = stackalloc double[1];
                int* iwork = stackalloc int[1];

                int m;
                int ldz = (job == LapackEigenvalues.SymmetricGeneralJob.All) ? n : 1;

                int info;
                _driver_zheevr(ref jobz, ref rangez, ref uplo, ref n, null, ref n, ref vl, ref vu, ref il, ref iu, ref abstol, out m, null, null, ref ldz, null, work, ref lwork, rwork, ref lrwork, iwork, ref liwork, out info);
                CheckForError(info, "driver_zheevr");

                lwork = ((int)work[0].Real) + 1;
                lrwork = ((int)rwork[0]) + 1;
                liwork = iwork[0];
            }
        }

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
        public void driver_zheevr(LapackEigenvalues.SymmetricGeneralJob job, LapackEigenvalues.SymmetricEigenvaluesRange range, int n, Complex[] a, double vl, double vu, int il, int iu, out int m, double[] w, Complex[] z, int[] isuppz, Complex[] work, double[] rwork, int[] iwork, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix, double abstol = MachineConsts.Epsilon)
        {
            int info;
            var jobz = GetJob(job);
            int lwork = work.Length;
            int liwork = iwork.Length;
            int lrwork = rwork.Length;
            var rangez = GetRange(range);
            var uplo = LAPACK.GetUplo(triangularMatrixType);
            int ldz = (job == LapackEigenvalues.SymmetricGeneralJob.All) ? n : 1;

            _driver_zheevr(ref jobz, ref rangez, ref uplo, ref n, a, ref n, ref vl, ref vu, ref il, ref iu, ref abstol, out m, w, z, ref ldz, isuppz, work, ref lwork, rwork, ref lrwork, iwork, ref liwork, out info);
            CheckForError(info, "driver_zheevr");
        }

        /// <summary>Computes all eigenvalues and, optionally, eigenvectors of a real symmetric matrix in packed storage.
        /// </summary>
        /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="n">The order of the specified symmetric matrix.</param>
        /// <param name="ap">The specified symmetric matrix in packed form, i.e. either upper or lower triangle as specified in <paramref name="triangularMatrixType"/> with at least <paramref name="n"/> * (<paramref name="n"/> + 1) / 2 elements.</param>
        /// <param name="w">The eigenvalues of <paramref name="ap"/> in ascending order, i.e. the array should have at least <paramref name="n"/> elements (output).</param>
        /// <param name="z">If <paramref name="job"/> indicates to compute eigenvectors, this parameter contains the orthonormal eigenvectors (output).</param>
        /// <param name="work">A workspace array with at least 3 * <paramref name="n"/> elements.</param>
        /// <param name="triangularMatrixType">A value indicating whether the upper or lower triangular part of the symmetric input matrix is stored.</param>
        public void driver_dspev(LapackEigenvalues.SymmetricGeneralJob job, int n, double[] ap, double[] w, double[] z, double[] work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var jobz = GetJob(job);
            var uplo = LAPACK.GetUplo(triangularMatrixType);
            int ldz = (job == LapackEigenvalues.SymmetricGeneralJob.All) ? n : 1;

            _driver_dspev(ref jobz, ref uplo, ref n, ap, w, z, ref ldz, work, out info);
            CheckForError(info, "driver_dspev");
        }

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
        public void driver_zhpev(LapackEigenvalues.SymmetricGeneralJob job, int n, Complex[] ap, double[] w, Complex[] z, Complex[] work, double[] rwork, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            int info;
            var jobz = GetJob(job);
            int lwork = work.Length;
            var uplo = LAPACK.GetUplo(triangularMatrixType);
            int ldz = (job == LapackEigenvalues.SymmetricGeneralJob.All) ? n : 1;

            _driver_zhpev(ref jobz, ref uplo, ref n, ap, w, z, ref ldz, work, rwork, out info);
            CheckForError(info, "driver_zhpev");
        }
        #endregion

        #region private methods

        /// <summary>Gets the <see cref="System.Char"/> representation of a specific LAPACK job parameter.
        /// </summary>
        /// <param name="job">The specified LAPACK job parameter.</param>
        /// <returns>The <see cref="System.Char"/> representation of the specified LAPACK job parameter.</returns>
        private char GetJob(LapackEigenvalues.SymmetricXxbtrdJob job)
        {
            switch (job)
            {
                case LapackEigenvalues.SymmetricXxbtrdJob.CalculateMatrixQ:
                    return 'V';
                case LapackEigenvalues.SymmetricXxbtrdJob.None:
                    return 'N';
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>Gets the <see cref="System.Char"/> representation of a specific LAPACK job parameter.
        /// </summary>
        /// <param name="job">The specified LAPACK job parameter.</param>
        /// <returns>The <see cref="System.Char"/> representation of the specified LAPACK job parameter.</returns>
        private char GetJob(LapackEigenvalues.SymmetricJob job)
        {
            switch (job)
            {
                case LapackEigenvalues.SymmetricJob.EigenValuesOnly:
                    return 'N';
                case LapackEigenvalues.SymmetricJob.ForTridiagonalMatrix:
                    return 'I';
                case LapackEigenvalues.SymmetricJob.ForOriginalMatrix:
                    return 'V';
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>Gets the <see cref="System.Char"/> representation of a specific LAPACK job parameter.
        /// </summary>
        /// <param name="job">The specified LAPACK job parameter.</param>
        /// <returns>The <see cref="System.Char"/> representation of the specified LAPACK job parameter.</returns>
        private char GetJob(LapackEigenvalues.SymmetricGeneralJob job)
        {
            switch (job)
            {
                case LapackEigenvalues.SymmetricGeneralJob.EigenValuesOnly:
                    return 'N';
                case LapackEigenvalues.SymmetricGeneralJob.All:
                    return 'V';
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>Gets the <see cref="System.Char"/> representation of the range.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <returns>The <see cref="System.Char"/> representation of the range.</returns>
        private char GetRange(LapackEigenvalues.SymmetricEigenvaluesRange range)
        {
            switch (range)
            {
                case LapackEigenvalues.SymmetricEigenvaluesRange.All:
                    return 'A';
                case LapackEigenvalues.SymmetricEigenvaluesRange.Boundaries:
                    return 'V';
                case LapackEigenvalues.SymmetricEigenvaluesRange.Indices:
                    return 'I';
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>Gets the <see cref="System.Char"/> representation of the way how to order the eigenvalues.
        /// </summary>
        /// <param name="order">A value indicating the way how to order the eigenvalues.</param>
        /// <returns>The <see cref="System.Char"/> representation of the order of the eigenvalues.</returns>
        private char GetOrder(LapackEigenvalues.SymmetricDstebzOrder order)
        {
            switch (order)
            {
                case LapackEigenvalues.SymmetricDstebzOrder.Blockwise:
                    return 'B';
                case LapackEigenvalues.SymmetricDstebzOrder.Complete:
                    return 'E';
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>Gets the <see cref="System.Char"/> representation of a specific LAPACK job parameter.
        /// </summary>
        /// <param name="job">The specified LAPACK job parameter.</param>
        /// <returns>The <see cref="System.Char"/> representation of the specified LAPACK job parameter.</returns>
        private char GetJob(LapackEigenvalues.SymmetricDdisnaJob job)
        {
            switch (job)
            {
                case LapackEigenvalues.SymmetricDdisnaJob.Eigenvectors:
                    return 'E';
                case LapackEigenvalues.SymmetricDdisnaJob.LeftSingularVectors:
                    return 'L';
                case LapackEigenvalues.SymmetricDdisnaJob.RightSingularVectors:
                    return 'R';
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion
    }
}