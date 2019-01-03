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
    internal partial class LapackNativeWrapper : LapackEigenvalues.INonSymmetricEigenvalueProblems
    {
        #region private delegate for marshaling

        [UnmanagedFunctionPointer(sm_CallingConvention)]
        private delegate bool SelectEigenvaluesReal(ref double x, ref double y);

        [UnmanagedFunctionPointer(sm_CallingConvention)]
        private delegate bool SelectEigenvaluesComplex(ref Complex x);
        #endregion

        #region private function import

        [DllImport(sm_DllName, EntryPoint = "DGEHRD", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dgehrd(ref int n, ref int ilo, ref int ihi, [In, Out] double[] a, ref int lda, [In, Out] double[] tau, [In, Out] double[] work, ref int lWork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DGEHRD", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void _dgehrd(ref int n, ref int ilo, ref int ihi, [In, Out] double[] a, ref int lda, [In, Out] double[] tau, double* work, ref int lWork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZGEHRD", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zgehrd(ref int n, ref int ilo, ref int ihi, [In, Out] Complex[] a, ref int lda, [In, Out] Complex[] tau, [In, Out] Complex[] work, ref int lWork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZGEHRD", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void _zgehrd(ref int n, ref int ilo, ref int ihi, [In, Out] Complex[] a, ref int lda, [In, Out] Complex[] tau, Complex* work, ref int lWork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DORGHR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dorghr(ref int n, ref int ilo, ref int ihi, [In, Out] double[] a, ref int lda, [In, Out] double[] tau, [In, Out] double[] work, ref int lWork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DORGHR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void _dorghr(ref int n, ref int ilo, ref int ihi, [In, Out] double[] a, ref int lda, [In, Out] double[] tau, double* work, ref int lWork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DORMHR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dormhr(ref char side, ref char trans, ref int m, ref int n, ref int ilo, ref int ihi, [In, Out] double[] a, ref int lda, [In, Out] double[] tau, [In, Out] double[] c, ref int ldc, [In, Out] double[] work, ref int lWork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DORMHR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void _dormhr(ref char side, ref char trans, ref int m, ref int n, ref int ilo, ref int ihi, [In, Out] double[] a, ref int lda, [In, Out] double[] tau, [In, Out] double[] c, ref int ldc, double* work, ref int lWork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZUNGHR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zunghr(ref int n, ref int ilo, ref int ihi, [In, Out] Complex[] a, ref int lda, [In, Out] Complex[] tau, [In, Out] Complex[] work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZUNGHR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void _zunghr(ref int n, ref int ilo, ref int ihi, [In, Out] Complex[] a, ref int lda, [In, Out] Complex[] tau, Complex* work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZUNMHR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zunmhr(ref char side, ref char trans, ref int m, ref int n, ref int ilo, ref int ihi, [In, Out] Complex[] a, ref int lda, [In, Out] Complex[] tau, [In, Out] Complex[] c, ref int ldc, [In, Out] Complex[] work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZUNMHR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void _zunmhr(ref char side, ref char trans, ref int m, ref int n, ref int ilo, ref int ihi, [In, Out] Complex[] a, ref int lda, [In, Out] Complex[] tau, [In, Out] Complex[] c, ref int ldc, Complex* work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DGEBAL", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dgebal(ref char job, ref int n, [In, Out] double[] a, ref int lda, out int ilo, out int ihi, [In, Out] double[] scale, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZGEBAL", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zgebal(ref char job, ref int n, [In, Out] Complex[] a, ref int lda, out int ilo, out int ihi, [In, Out] double[] scale, out int info);

        [DllImport(sm_DllName, EntryPoint = "DGEBAK", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dgebak(ref char job, ref char side, ref int n, ref int ilo, ref int ihi, [In, Out] double[] scale, ref int m, [In, Out] double[] v, ref int ldv, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZGEBAK", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zgebak(ref char job, ref char side, ref int n, ref int ilo, ref int ihi, [In, Out] double[] scale, ref int m, [In, Out] Complex[] v, ref int ldv, out int info);

        [DllImport(sm_DllName, EntryPoint = "DHSEQR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dhseqr(ref char job, ref char compz, ref int n, ref int ilo, ref int ihi, [In, Out] double[] h, ref int ldh, [In, Out] double[] wr, [In, Out] double[] wi, [In, Out] double[] z, ref int ldz, [In, Out] double[] work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DHSEQR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void _dhseqr(ref char job, ref char compz, ref int n, ref int ilo, ref int ihi, [In, Out] double[] h, ref int ldh, [In, Out] double[] wr, [In, Out] double[] wi, [In, Out] double[] z, ref int ldz, double* work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZHSEQR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zhseqr(ref char job, ref char compz, ref int n, ref int ilo, ref int ihi, [In, Out] Complex[] h, ref int ldh, [In, Out] Complex[] w, [In, Out] Complex[] z, ref int ldz, [In, Out] Complex[] work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZHSEQR", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void _zhseqr(ref char job, ref char compz, ref int n, ref int ilo, ref int ihi, [In, Out] Complex[] h, ref int ldh, [In, Out] Complex[] w, [In, Out] Complex[] z, ref int ldz, Complex* work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DHSEIN", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dhsein(ref char job, ref char eigsrc, ref char initv, [In, Out] bool[] select, ref int n, [In, Out] double[] h, ref int ldh, [In, Out] double[] wr, [In, Out] double[] wi, [In, Out] double[] vl, ref int ldvl, [In, Out] double[] vr, ref int ldvr, ref int mm, out int m, [In, Out] double[] work, [In, Out] int[] ifaill, [In, Out] int[] ifailr, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZHSEIN", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _zhsein(ref char job, ref char eigsrc, ref char initv, [In, Out] bool[] select, ref int n, [In, Out] Complex[] h, ref int ldh, [In, Out] Complex[] w, [In, Out] Complex[] vl, ref int ldvl, [In, Out] Complex[] vr, ref int ldvr, ref int mm, out int m, [In, Out] Complex[] work, [In, Out] double[] rwork, [In, Out] int[] ifaill, [In, Out] int[] ifailr, out int info);

        [DllImport(sm_DllName, EntryPoint = "DTREVC", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dtrevc(ref char side, ref char howmny, [In, Out] bool[] select, ref int n, [In, Out] double[] t, ref int ldt, [In, Out] double[] vl, ref int ldvl, [In, Out] double[] vr, ref int ldvr, ref int mm, out int m, [In, Out] double[] work, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZTREVC", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _ztrevc(ref char side, ref char howmny, [In, Out] bool[] select, ref int n, [In, Out] Complex[] t, ref int ldt, [In, Out] Complex[] vl, ref int ldvl, [In, Out] Complex[] vr, ref int ldvr, ref int mm, out int m, [In, Out] Complex[] work, [In, Out] double[] rwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DTRSNA", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dtrsna(ref char job, ref char howmny, [In, Out] bool[] select, ref int n, [In, Out] double[] t, ref int ldt, [In, Out] double[] vl, ref int ldvl, [In, Out] double[] vr, ref int ldvr, [In, Out] double[] s, [In, Out] double[] sep, ref int mm, out int m, [In, Out] double[] work, ref int ldwork, [In, Out] int[] iwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZTRSNA", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _ztrsna(ref char job, ref char howmny, [In, Out] bool[] select, ref int n, [In, Out] Complex[] t, ref int ldt, [In, Out] Complex[] vl, ref int ldvl, [In, Out] Complex[] vr, ref int ldvr, [In, Out] double[] s, [In, Out] double[] sep, ref int mm, out int m, [In, Out] Complex[] work, ref int ldwork, [In, Out] double[] rwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DTREXC", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dtrexc(ref char compq, ref int n, [In, Out] double[] t, ref int ldt, [In, Out] double[] q, ref int ldq, ref int ifst, ref int ilst, [In, Out] double[] work, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZTREXC", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _ztrexc(ref char compq, ref int n, [In, Out] Complex[] t, ref int ldt, [In, Out] Complex[] q, ref int ldq, ref int ifst, ref int ilst, out int info);

        [DllImport(sm_DllName, EntryPoint = "DTRSEN", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dtrsen(ref char job, ref char compq, [In, Out] bool[] select, ref int n, [In, Out] double[] t, ref int ldt, [In, Out] double[] q, ref int ldq, [In, Out] double[] wr, [In, Out] double[] wi, out int m, [In, Out] double[] s, [In, Out] double[] sep, [In, Out] double[] work, ref int lwork, [In, Out] int[] iwork, ref int liwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DTRSEN", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void _dtrsen(ref char job, ref char compq, [In, Out] bool[] select, ref int n, [In, Out] double[] t, ref int ldt, [In, Out] double[] q, ref int ldq, [In, Out] double[] wr, [In, Out] double[] wi, out int m, [In, Out] double[] s, [In, Out] double[] sep, double* work, ref int lwork, int* iwork, ref int liwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZTRSEN", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _ztrsen(ref char job, ref char compq, [In, Out] bool[] select, ref int n, [In, Out] Complex[] t, ref int ldt, [In, Out] Complex[] q, ref int ldq, [In, Out] Complex[] w, out int m, [In, Out] double[] s, [In, Out] double[] sep, [In, Out] Complex[] work, ref int lwork, [In, Out] int[] iwork, ref int liwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZTRSEN", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void _ztrsen(ref char job, ref char compq, [In, Out] bool[] select, ref int n, [In, Out] Complex[] t, ref int ldt, [In, Out] Complex[] q, ref int ldq, [In, Out] Complex[] w, out int m, [In, Out] double[] s, [In, Out] double[] sep, Complex* work, ref int lwork, int* iwork, ref int liwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DTRSYL", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _dtrsyl(ref char trana, ref char tranb, ref int isgn, ref int m, ref int n, [In, Out] double[] a, ref int lda, [In, Out] double[] b, ref int ldb, [In, Out] double[] c, ref int ldc, out double scale, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZTRSYL", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _ztrsyl(ref char trana, ref char tranb, ref int isgn, ref int m, ref int n, [In, Out] Complex[] a, ref int lda, [In, Out] Complex[] b, ref int ldb, [In, Out] Complex[] c, ref int ldc, out double scale, out int info);

        [DllImport(sm_DllName, EntryPoint = "DGEES", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _driver_dgees(ref char jobvs, ref char sort, [MarshalAs(UnmanagedType.FunctionPtr)] SelectEigenvaluesReal select, ref int n, [In, Out] double[] a, ref int lda, out int sdim, [In, Out] double[] wr, [In, Out] double[] wi, [In, Out] double[] vs, ref int ldvs, [In, Out] double[] work, ref int lwork, [In, Out] bool[] bwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DGEES", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void _driver_dgees(ref char jobvs, ref char sort, [MarshalAs(UnmanagedType.FunctionPtr)] SelectEigenvaluesReal select, ref int n, [In, Out] double[] a, ref int lda, out int sdim, [In, Out] double[] wr, [In, Out] double[] wi, [In, Out] double[] vs, ref int ldvs, double* work, ref int lwork, [In, Out] bool[] bwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZGEES", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _driver_zgees(ref char jobvs, ref char sort, [MarshalAs(UnmanagedType.FunctionPtr)] SelectEigenvaluesComplex select, ref int n, [In, Out] Complex[] a, ref int lda, out int sdim, [In, Out] Complex[] w, [In, Out] Complex[] vs, ref int ldvs, [In, Out] Complex[] work, ref int lwork, [In, Out] double[] rwork, [In, Out] bool[] bwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZGEES", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void _driver_zgees(ref char jobvs, ref char sort, [MarshalAs(UnmanagedType.FunctionPtr)] SelectEigenvaluesComplex select, ref int n, [In, Out] Complex[] a, ref int lda, out int sdim, [In, Out] Complex[] w, [In, Out] Complex[] vs, ref int ldvs, Complex* work, ref int lwork, [In, Out] double[] rwork, [In, Out] bool[] bwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DGEESX", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _driver_dgeesx(ref char jobvs, ref char sort, [MarshalAs(UnmanagedType.FunctionPtr)] SelectEigenvaluesReal select, ref char sense, ref int n, [In, Out] double[] a, ref int lda, out int sdim, [In, Out] double[] wr, [In, Out] double[] wi, [In, Out] double[] vs, ref int ldvs, out double rconde, out double rcondv, [In, Out] double[] work, ref int lwork, [In, Out] int[] iwork, ref int liwork, [In, Out] bool[] bwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DGEESX", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void _driver_dgeesx(ref char jobvs, ref char sort, [MarshalAs(UnmanagedType.FunctionPtr)] SelectEigenvaluesReal select, ref char sense, ref int n, [In, Out] double[] a, ref int lda, out int sdim, [In, Out] double[] wr, [In, Out] double[] wi, [In, Out] double[] vs, ref int ldvs, out double rconde, out double rcondv, double* work, ref int lwork, [In, Out] int[] iwork, ref int liwork, [In, Out] bool[] bwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZGEESX", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _driver_zgeesx(ref char jobvs, ref char sort, [MarshalAs(UnmanagedType.FunctionPtr)] SelectEigenvaluesComplex select, ref char sense, ref int n, [In, Out] Complex[] a, ref int lda, out int sdim, [In, Out] Complex[] w, [In, Out] Complex[] vs, ref int ldvs, out double rconde, out double rcondv, [In, Out] Complex[] work, ref int lwork, [In, Out] double[] rwork, [In, Out] bool[] bwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZGEESX", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void _driver_zgeesx(ref char jobvs, ref char sort, [MarshalAs(UnmanagedType.FunctionPtr)] SelectEigenvaluesComplex select, ref char sense, ref int n, [In, Out] Complex[] a, ref int lda, out int sdim, [In, Out] Complex[] w, [In, Out] Complex[] vs, ref int ldvs, out double rconde, out double rcondv, Complex* work, ref int lwork, [In, Out] double[] rwork, [In, Out] bool[] bwork, out int info);



        [DllImport(sm_DllName, EntryPoint = "DGEEV", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _driver_dgeev(ref char jobvl, ref char jobvr, ref int n, [In, Out] double[] a, ref int lda, [In, Out] double[] wr, [In, Out] double[] wi, [In, Out] double[] vl, ref int ldvl, [In, Out] double[] vr, ref int ldvr, [In, Out] double[] work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DGEEV", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void _driver_dgeev(ref char jobvl, ref char jobvr, ref int n, [In, Out] double[] a, ref int lda, [In, Out] double[] wr, [In, Out] double[] wi, [In, Out] double[] vl, ref int ldvl, [In, Out] double[] vr, ref int ldvr, double* work, ref int lwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZGEEV", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _driver_zgeev(ref char jobvl, ref char jobvr, ref int n, [In, Out] Complex[] a, ref int lda, [In, Out] Complex[] w, [In, Out] Complex[] vl, ref int ldvl, [In, Out] Complex[] vr, ref int ldvr, [In, Out] Complex[] work, ref int lwork, [In, Out] double[] rwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZGEEV", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void _driver_zgeev(ref char jobvl, ref char jobvr, ref int n, [In, Out] Complex[] a, ref int lda, [In, Out] Complex[] w, [In, Out] Complex[] vl, ref int ldvl, [In, Out] Complex[] vr, ref int ldvr, Complex* work, ref int lwork, [In, Out] double[] rwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DGEEVX", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _driver_dgeevx(ref char balanc, ref char jobvl, ref char jobvr, ref char sense, ref int n, [In, Out] double[] a, ref int lda, [In, Out] double[] wr, [In, Out] double[] wi, [In, Out] double[] vl, ref int ldvl, [In, Out] double[] vr, ref int ldvr, out int ilo, out int ihi, [In, Out] double[] scale, out double abnrm, [In, Out] double[] rconde, [In, Out] double[] rcondv, [In, Out] double[] work, ref int lwork, [In, Out] int[] iwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "DGEEVX", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void _driver_dgeevx(ref char balanc, ref char jobvl, ref char jobvr, ref char sense, ref int n, [In, Out] double[] a, ref int lda, [In, Out] double[] wr, [In, Out] double[] wi, [In, Out] double[] vl, ref int ldvl, [In, Out] double[] vr, ref int ldvr, out int ilo, out int ihi, [In, Out] double[] scale, out double abnrm, [In, Out] double[] rconde, [In, Out] double[] rcondv, double* work, ref int lwork, [In, Out] int[] iwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZGEEVX", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _driver_zgeevx(ref char balanc, ref char jobvl, ref char jobvr, ref char sense, ref int n, [In, Out] Complex[] a, ref int lda, [In, Out] Complex[] w, [In, Out] Complex[] vl, ref int ldvl, [In, Out] Complex[] vr, ref int ldvr, out int ilo, out int ihi, [In, Out] double[] scale, out double abnrm, [In, Out] double[] rconde, [In, Out] double[] rcondv, [In, Out] Complex[] work, ref int lwork, [In, Out] double[] rwork, out int info);

        [DllImport(sm_DllName, EntryPoint = "ZGEEVX", CallingConvention = sm_CallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void _driver_zgeevx(ref char balanc, ref char jobvl, ref char jobvr, ref char sense, ref int n, [In, Out] Complex[] a, ref int lda, [In, Out] Complex[] w, [In, Out] Complex[] vl, ref int ldvl, [In, Out] Complex[] vr, ref int ldvr, out int ilo, out int ihi, [In, Out] double[] scale, out double abnrm, [In, Out] double[] rconde, [In, Out] double[] rcondv, Complex* work, ref int lwork, [In, Out] double[] rwork, out int info);
        #endregion

        #region public methods

        /// <summary>Gets a optimal workspace array length for the <c>dgehrd</c> function.
        /// </summary>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="ilo">If the input matrix is an output by <c>?gebal</c> this parameter must contain the value returned by that routine; otherwise <c>1</c>.</param>
        /// <param name="ihi">If the input matrix is an output by <c>?gebal</c> this parameter must contain the value returned by that routine; otherwise <paramref name="n" />.</param>
        /// <returns>The optimal workspace array length.</returns>
        public int dgehrdQuery(int n, int ilo, int ihi)
        {
            int info;
            int lda = n;
            int lwork = -1;

            unsafe
            {
                double* work = stackalloc double[1];
                _dgehrd(ref n, ref ilo, ref ihi, null, ref lda, null, work, ref lwork, out info);
                CheckForError(info, "dgehrd");
                return (int)work[0] + 1;
            }
        }

        /// <summary>Reduces a general matrix to upper Hessenberg form H by an orthogonal or unitary similarity transformation A = Q * H * Q', where H has real subdiagonal elements.
        /// </summary>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="ilo">If the input matrix is an output by <c>dgebal</c> this parameter must contain the value returned by that routine; otherwise <c>1</c>.</param>
        /// <param name="ihi">If the input matrix is an output by <c>dgebal</c> this parameter must contain the value returned by that routine; otherwise <paramref name="n" />.</param>
        /// <param name="a">The <paramref name="n"/>-by-<paramref name="n"/> matrix A supplied column-by-column; overwritten by the upper Hessenberg matrix H and details of the matrix Q.</param>
        /// <param name="tau">A array of dimension at least max(1, n-1) containing additional informations on the matrix Q (output).</param>
        /// <param name="work">A workspace array.</param>
        public void dgehrd(int n, int ilo, int ihi, double[] a, double[] tau, double[] work)
        {
            int info;
            int lda = n;
            int lwork = work.Length;

            _dgehrd(ref n, ref ilo, ref ihi, a, ref lda, tau, work, ref lwork, out info);
            CheckForError(info, "dgehrd");
        }

        /// <summary>Gets a optimal workspace array length for the <c>zgehrd</c> function.
        /// </summary>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="ilo">If the input matrix is an output by <c>?gebal</c> this parameter must contain the value returned by that routine; otherwise <c>1</c>.</param>
        /// <param name="ihi">If the input matrix is an output by <c>?gebal</c> this parameter must contain the value returned by that routine; otherwise <paramref name="n" />.</param>
        /// <returns>The optimal workspace array length.</returns>
        public int zgehrdQuery(int n, int ilo, int ihi)
        {
            int info;
            int lda = n;
            int lwork = -1;

            unsafe
            {
                Complex* work = stackalloc Complex[1];
                _zgehrd(ref n, ref ilo, ref ihi, null, ref lda, null, work, ref lwork, out info);
                CheckForError(info, "zgehrd");
                return (int)work[0].Real + 1;
            }
        }

        /// <summary>Reduces a general matrix to upper Hessenberg form H by an orthogonal or unitary similarity transformation A = Q * H * Q', where H has real subdiagonal elements.
        /// </summary>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="ilo">If the input matrix is an output by <c>dgebal</c> this parameter must contain the value returned by that routine; otherwise <c>1</c>.</param>
        /// <param name="ihi">If the input matrix is an output by <c>dgebal</c> this parameter must contain the value returned by that routine; otherwise <paramref name="n" />.</param>
        /// <param name="a">The <paramref name="n"/>-by-<paramref name="n"/> matrix A supplied column-by-column; overwritten by the upper Hessenberg matrix H and details of the matrix Q.</param>
        /// <param name="tau">A array of dimension at least max(1, n-1) containing additional informations on the matrix Q (output).</param>
        /// <param name="work">A workspace array.</param>
        public void zgehrd(int n, int ilo, int ihi, Complex[] a, Complex[] tau, Complex[] work)
        {
            int info;
            int lda = n;
            int lwork = work.Length;

            _zgehrd(ref n, ref ilo, ref ihi, a, ref lda, tau, work, ref lwork, out info);
            CheckForError(info, "zgehrd");
        }

        /// <summary>Gets a optimal workspace array length for the <c>dorghr</c> function.
        /// </summary>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="ilo">If the input matrix is an output by <c>dgebal</c> this parameter must contain the value returned by that routine; otherwise <c>1</c>.</param>
        /// <param name="ihi">If the input matrix is an output by <c>dgebal</c> this parameter must contain the value returned by that routine; otherwise <paramref name="n" />.</param>
        /// <returns>The optimal workspace array length.</returns>
        public int dorghrQuery(int n, int ilo, int ihi)
        {
            int info;
            int lda = n;
            int lwork = -1;

            unsafe
            {
                double* work = stackalloc double[1];
                _dorghr(ref n, ref ilo, ref ihi, null, ref lda, null, work, ref lwork, out info);
                CheckForError(info, "dorghr");
                return (int)work[0] + 1;
            }
        }

        /// <summary>Generates the real orthogonal matrix Q determined by <c>dgehrd</c>, i.e. explicitly generates the orthogonal matrix Q with A = Q * H * Q'.
        /// </summary>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="ilo">If the input matrix is an output by <c>dgebal</c> this parameter must contain the value returned by that routine; otherwise <c>1</c>.</param>
        /// <param name="ihi">If the input matrix is an output by <c>dgebal</c> this parameter must contain the value returned by that routine; otherwise <paramref name="n" />.</param>
        /// <param name="a">Contains on input details of the vectors which define the elementary reflectors, as returned by <c>dgehrd</c>; overwritten by the n-by-n orthogonal matrix Q.</param>
        /// <param name="tau">A array of dimension at least max(1, n-1) containing additional informations on the matrix Q as returned by <c>dgehrd</c>.</param>
        /// <param name="work">A workspace array.</param>
        public void dorghr(int n, int ilo, int ihi, double[] a, double[] tau, double[] work)
        {
            int info;
            int lda = n;
            int lwork = work.Length;

            _dorghr(ref n, ref ilo, ref ihi, a, ref lda, tau, work, ref lwork, out info);
            CheckForError(info, "dorghr");
        }

        /// <summary>Gets a optimal workspace array length for the <c>dormhr</c> function.
        /// </summary>
        /// <param name="side">A value indicating whether to calculate \op(Q) * C [left] or C * \op(Q) [right].</param>
        /// <param name="transposeState">A value indicating whether \op(Q) = Q or \op(Q) = Q'.</param>
        /// <param name="m">The number of rows in matrix C.</param>
        /// <param name="n">The number of columns in matrix C.</param>
        /// <param name="ilo">The same parameter value as supplied to <c>dgehrd</c>.</param>
        /// <param name="ihi">The same parameter value as supplied to <c>dgehrd</c>.</param>
        /// <returns>The optimal workspace array length.</returns>
        public int dormhrQuery(LAPACK.Side side, BLAS.MatrixTransposeState transposeState, int m, int n, int ilo, int ihi)
        {
            var cSide = LAPACK.GetSide(side);
            var trans = LAPACK.GetTrans(transposeState);

            int info;
            int lwork = -1;
            int ldc = Math.Max(1, m);
            int lda = (side == LAPACK.Side.Left) ? Math.Max(1, m) : Math.Max(1, n);

            unsafe
            {
                double* work = stackalloc double[1];
                _dormhr(ref cSide, ref trans, ref m, ref n, ref ilo, ref ihi, null, ref lda, null, null, ref ldc, work, ref lwork, out info);
                CheckForError(info, "dormhr");
                return (int)work[0] + 1;
            }
        }

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
        public void dormhr(LAPACK.Side side, BLAS.MatrixTransposeState transposeState, int m, int n, int ilo, int ihi, double[] a, double[] tau, double[] c, double[] work)
        {
            var cSide = LAPACK.GetSide(side);
            var trans = LAPACK.GetTrans(transposeState);

            int info;
            int lwork = work.Length;
            int ldc = Math.Max(1, m);
            int lda = (side == LAPACK.Side.Left) ? Math.Max(1, m) : Math.Max(1, n);

            _dormhr(ref cSide, ref trans, ref m, ref n, ref ilo, ref ihi, a, ref lda, tau, c, ref ldc, work, ref lwork, out info);
            CheckForError(info, "dormhr");
        }

        /// <summary>Gets a optimal workspace array length for the <c>zunghr</c> function.
        /// </summary>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="ilo">If the input matrix is an output by <c>zgehrd</c> this parameter must contain the value returned by that routine; otherwise <c>1</c>.</param>
        /// <param name="ihi">If the input matrix is an output by <c>zgehrd</c> this parameter must contain the value returned by that routine; otherwise <paramref name="n" />.</param>
        /// <returns>The optimal workspace array length.</returns>
        public int zunghrQuery(int n, int ilo, int ihi)
        {
            int info;
            int lda = n;
            int lwork = -1;

            unsafe
            {
                Complex* work = stackalloc Complex[1];
                _zunghr(ref n, ref ilo, ref ihi, null, ref lda, null, work, ref lwork, out info);
                CheckForError(info, "zunghr");
                return (int)work[0].Real + 1;
            }
        }

        /// <summary>Generates the complex unitary matrix Q determined by <c>zgehrd</c>.
        /// </summary>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="ilo">If the input matrix is an output by <c>zgehrd</c> this parameter must contain the value returned by that routine; otherwise <c>1</c>.</param>
        /// <param name="ihi">If the input matrix is an output by <c>zgehrd</c> this parameter must contain the value returned by that routine; otherwise <paramref name="n" />.</param>
        /// <param name="a">Contains on input details of the vectors which define the elementary reflectors, as returned by <c>zgehrd</c>; overwritten by the n-by-n unitary matrix Q.</param>
        /// <param name="tau">A array of dimension at least max(1, n-1) containing additional informations on the matrix Q as returned by <c>zgehrd</c>.</param>
        /// <param name="work">A workspace array.</param>
        public void zunghr(int n, int ilo, int ihi, Complex[] a, Complex[] tau, Complex[] work)
        {
            int info;
            int lda = n;
            int lwork = work.Length;

            _zunghr(ref n, ref ilo, ref ihi, a, ref lda, tau, work, ref lwork, out info);
            CheckForError(info, "zunghr");
        }

        /// <summary>Gets a optimal workspace array length for the <c>zunmhr</c> function.
        /// </summary>
        /// <param name="side">A value indicating whether to calculate \op(Q) * C [left] or C * \op(Q) [right].</param>
        /// <param name="transposeState">A value indicating whether \op(Q) = Q or \op(Q) = Q'.</param>
        /// <param name="m">The number of rows in matrix C.</param>
        /// <param name="n">The number of columns in matrix C.</param>
        /// <param name="ilo">The same parameter value as supplied to <c>dgehrd</c>.</param>
        /// <param name="ihi">The same parameter value as supplied to <c>dgehrd</c>.</param>
        /// <returns>The optimal workspace array length.</returns>
        public int zunmhrQuery(LAPACK.Side side, BLAS.MatrixTransposeState transposeState, int m, int n, int ilo, int ihi)
        {
            var cSide = LAPACK.GetSide(side);
            var trans = LAPACK.GetTrans(transposeState);

            int info;
            int lwork = -1;
            int ldc = Math.Max(1, m);
            int lda = (side == LAPACK.Side.Left) ? Math.Max(1, m) : Math.Max(1, n);

            unsafe
            {
                Complex* work = stackalloc Complex[1];
                _zunmhr(ref cSide, ref trans, ref m, ref n, ref ilo, ref ihi, null, ref lda, null, null, ref ldc, work, ref lwork, out info);
                CheckForError(info, "zunmhr");
                return (int)work[0].Real + 1;
            }
        }

        /// <summary>Multiplies an arbitrary complex matrix C by the complex unitary matrix Q determined by <c>zgehrd</c>, i.e. \op(Q) * C or C * \op(Q), where \op(Q) = Q or \op(Q) = Q'.
        /// </summary>
        /// <param name="side">A value indicating whether to calculate \op(Q) * C [left] or C * \op(Q) [right].</param>
        /// <param name="transposeState">A value indicating whether \op(Q) = Q or \op(Q) = Q'.</param>
        /// <param name="m">The number of rows in matrix C.</param>
        /// <param name="n">The number of columns in matrix C.</param>
        /// <param name="ilo">The same parameter value as supplied to <c>zgehrd</c>.</param>
        /// <param name="ihi">The same parameter value as supplied to <c>zgehrd</c>.</param>
        /// <param name="a">Contains details of the vectors which define the elementary reflectors, as returned by <c>zgehrd</c>.</param>
        /// <param name="tau">Contains further details of the elementary reflectors as returned by <c>zgehrd</c>.</param>
        /// <param name="c">The m-by-n matrix C supplied column-by-column.</param>
        /// <param name="work">A workspace array.</param>
        public void zunmhr(LAPACK.Side side, BLAS.MatrixTransposeState transposeState, int m, int n, int ilo, int ihi, Complex[] a, Complex[] tau, Complex[] c, Complex[] work)
        {
            var cSide = LAPACK.GetSide(side);
            var trans = LAPACK.GetTrans(transposeState);

            int info;
            int lwork = work.Length;
            int ldc = Math.Max(1, m);
            int lda = (side == LAPACK.Side.Left) ? Math.Max(1, m) : Math.Max(1, n);

            _zunmhr(ref cSide, ref trans, ref m, ref n, ref ilo, ref ihi, a, ref lda, tau, c, ref ldc, work, ref lwork, out info);
            CheckForError(info, "zunmhr");
        }

        /// <summary>Balances a general matrix to improve the accuracy of computed eigenvalues and eigenvectors.
        /// </summary>
        /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="a">The matrix A supplied column-by-column of dimension; overwritten by the balanced matrix.</param>
        /// <param name="ilo">A null-based index such that a[i,j] is zero for i &gt; j and 1 &lt; j &lt;= <paramref name="ilo" /> or <paramref name="ihi" /> &lt; i &lt;= n.</param>
        /// <param name="ihi">A null-based index such that a[i,j] is zero for i &gt; j and 1 &lt; j &lt;= <paramref name="ilo" /> or <paramref name="ihi" /> &lt; i &lt;= n.</param>
        /// <param name="scale">Contains details of the permutations and scaling factors; at least <paramref name="n" /> elements (output).</param>
        public void dgebal(LapackEigenvalues.NonSymmetricMatrixBalancesType job, int n, double[] a, out int ilo, out int ihi, double[] scale)
        {
            int info;
            int lda = n;
            var jobz = GetJob(job);

            _dgebal(ref jobz, ref n, a, ref lda, out ilo, out ihi, scale, out info);
            CheckForError(info, "dgebal");
        }

        /// <summary>Balances a general matrix to improve the accuracy of computed eigenvalues and eigenvectors.
        /// </summary>
        /// <param name="job">A value indicating what kind of job to do by the LAPACK function.</param>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="a">The matrix A supplied column-by-column of dimension; overwritten by the balanced matrix.</param>
        /// <param name="ilo">A null-based index such that a[i,j] is zero for i &gt; j and 1 &lt; j &lt;= <paramref name="ilo" /> or <paramref name="ihi" /> &lt; i &lt;= n.</param>
        /// <param name="ihi">A null-based index such that a[i,j] is zero for i &gt; j and 1 &lt; j &lt;= <paramref name="ilo" /> or <paramref name="ihi" /> &lt; i &lt;= n.</param>
        /// <param name="scale">Contains details of the permutations and scaling factors; at least <paramref name="n" /> elements (output).</param>
        public void zgebal(LapackEigenvalues.NonSymmetricMatrixBalancesType job, int n, Complex[] a, out int ilo, out int ihi, double[] scale)
        {
            int info;
            int lda = n;
            var jobz = GetJob(job);

            _zgebal(ref jobz, ref n, a, ref lda, out ilo, out ihi, scale, out info);
            CheckForError(info, "zgebal");
        }

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
        public void dgebak(LapackEigenvalues.NonSymmetricMatrixBalancesType job, LAPACK.Side side, int n, int ilo, int ihi, double[] scale, int m, double[] v)
        {
            int info;
            var jobz = GetJob(job);
            int ldv = Math.Max(1, n);
            var cSide = (side == LAPACK.Side.Left) ? 'L' : 'R';

            _dgebak(ref jobz, ref cSide, ref n, ref ilo, ref ihi, scale, ref m, v, ref ldv, out info);
            CheckForError(info, "dgebak");
        }

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
        public void zgebak(LapackEigenvalues.NonSymmetricMatrixBalancesType job, LAPACK.Side side, int n, int ilo, int ihi, double[] scale, int m, Complex[] v)
        {
            int info;
            var jobz = GetJob(job);
            int ldv = Math.Max(1, n);
            var cSide = (side == LAPACK.Side.Left) ? 'L' : 'R';

            _zgebak(ref jobz, ref cSide, ref n, ref ilo, ref ihi, scale, ref m, v, ref ldv, out info);
            CheckForError(info, "zgebak");
        }

        /// <summary>Gets a optimal workspace array length for the <c>dhseqr</c> function.
        /// </summary>
        /// <param name="job">A value indicating whether eigenvalues or Schur form is required.</param>
        /// <param name="operation">A value indicating</param>
        /// <param name="n">The order of the Hessenberg matrix H.</param>
        /// <param name="ilo">If the input matrix is an output by <c>dgebal</c> this parameter must contain the value returned by that routine; otherwise <c>1</c>.</param>
        /// <param name="ihi">If the input matrix is an output by <c>dgebal</c> this parameter must contain the value returned by that routine; otherwise <paramref name="n" />.</param>
        /// <returns>The optimal workspace array length.</returns>
        public int dhseqrQuery(LapackEigenvalues.NonSymmetricXhseqrJob job, LapackEigenvalues.NonSymmetricXhseqrOperation operation, int n, int ilo, int ihi)
        {
            unsafe
            {
                int info;
                int ldh = n;
                int lwork = -1;
                var jobz = GetJob(job);
                var compz = GetOperation(operation);
                int ldz = (operation == LapackEigenvalues.NonSymmetricXhseqrOperation.NoSchurVectors) ? 1 : Math.Max(1, n);

                double* work = stackalloc double[1];

                _dhseqr(ref jobz, ref compz, ref n, ref ilo, ref ihi, null, ref ldh, null, null, null, ref ldz, work, ref lwork, out info);
                CheckForError(info, "dhseqr");
                return (int)work[0] + 1;
            }
        }

        /// <summary>Computes all eigenvalues and (optionally) the Schur factorization of a matrix reduced to Hessenberg form, i.e. A = Q * H * Q' where Q is orthogonal and H = Z * T * Z'.
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
        public void dhseqr(LapackEigenvalues.NonSymmetricXhseqrJob job, LapackEigenvalues.NonSymmetricXhseqrOperation operation, int n, int ilo, int ihi, double[] h, double[] wr, double[] wi, double[] z, double[] work)
        {
            int ldh = n;
            int lwork = work.Length;
            var jobz = GetJob(job);
            var compz = GetOperation(operation);
            int ldz = (operation == LapackEigenvalues.NonSymmetricXhseqrOperation.NoSchurVectors) ? 1 : Math.Max(1, n);

            int info;
            _dhseqr(ref jobz, ref compz, ref n, ref ilo, ref ihi, h, ref ldh, wr, wi, z, ref ldz, work, ref lwork, out info);
            CheckForError(info, "dhseqr");
        }

        /// <summary>Gets a optimal workspace array length for the <c>zhseqr</c> function.
        /// </summary>
        /// <param name="job">A value indicating whether eigenvalues or Schur form is required.</param>
        /// <param name="operation">A value indicating</param>
        /// <param name="n">The order of the Hessenberg matrix H.</param>
        /// <param name="ilo">If the input matrix is an output by <c>zgebal</c> this parameter must contain the value returned by that routine; otherwise <c>1</c>.</param>
        /// <param name="ihi">If the input matrix is an output by <c>zgebal</c> this parameter must contain the value returned by that routine; otherwise <paramref name="n" />.</param>
        /// <returns>The optimal workspace array length.</returns>
        public int zhseqrQuery(LapackEigenvalues.NonSymmetricXhseqrJob job, LapackEigenvalues.NonSymmetricXhseqrOperation operation, int n, int ilo, int ihi)
        {
            unsafe
            {
                int info;
                int ldh = n;
                int lwork = -1;
                var jobz = GetJob(job);
                var compz = GetOperation(operation);
                int ldz = (operation == LapackEigenvalues.NonSymmetricXhseqrOperation.NoSchurVectors) ? 1 : Math.Max(1, n);

                Complex* work = stackalloc Complex[1];

                _zhseqr(ref jobz, ref compz, ref n, ref ilo, ref ihi, null, ref ldh, null, null, ref ldz, work, ref lwork, out info);
                CheckForError(info, "zhseqr");
                return (int)work[0].Real + 1;
            }
        }

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
        public void zhseqr(LapackEigenvalues.NonSymmetricXhseqrJob job, LapackEigenvalues.NonSymmetricXhseqrOperation operation, int n, int ilo, int ihi, Complex[] h, Complex[] w, Complex[] z, Complex[] work)
        {
            int ldh = n;
            int lwork = work.Length;
            var jobz = GetJob(job);
            var compz = GetOperation(operation);
            int ldz = (operation == LapackEigenvalues.NonSymmetricXhseqrOperation.NoSchurVectors) ? 1 : Math.Max(1, n);

            int info;
            _zhseqr(ref jobz, ref compz, ref n, ref ilo, ref ihi, h, ref ldh, w, z, ref ldz, work, ref lwork, out info);
            CheckForError(info, "zhseqr");
        }

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
        public void dhsein(LapackEigenvalues.NonSymmetricEigenValueVectorsJob job, LapackEigenvalues.NonSymmetricEigenvalueSource eigenvalueSource, LapackEigenvalues.NonSymmetricXhseinInitV initv, bool[] select, int n, double[] h, double[] wr, double[] wi, double[] vl, double[] vr, int mm, out int m, double[] work, int[] ifaill, int[] ifailr)
        {
            char jobz = GetJob(job);
            char eigsrc = GetSource(eigenvalueSource);
            char initvz = GetInit(initv);

            int ldh = Math.Max(1, n);
            int ldvl = (job == LapackEigenvalues.NonSymmetricEigenValueVectorsJob.Right) ? 1 : Math.Max(1, n);
            int ldvr = (job == LapackEigenvalues.NonSymmetricEigenValueVectorsJob.Left) ? 1 : Math.Max(1, n);

            int info;
            _dhsein(ref jobz, ref eigsrc, ref initvz, select, ref n, h, ref ldh, wr, wi, vl, ref ldvl, vr, ref ldvr, ref mm, out m, work, ifaill, ifailr, out info);
            CheckForError(info, "dhsein");
        }

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
        public void zhsein(LapackEigenvalues.NonSymmetricEigenValueVectorsJob job, LapackEigenvalues.NonSymmetricEigenvalueSource eigenvalueSource, LapackEigenvalues.NonSymmetricXhseinInitV initv, bool[] select, int n, Complex[] h, Complex[] w, Complex[] vl, Complex[] vr, int mm, out int m, Complex[] work, double[] rwork, int[] ifaill, int[] ifailr)
        {
            char jobz = GetJob(job);
            char eigsrc = GetSource(eigenvalueSource);
            char initvz = GetInit(initv);

            int ldh = Math.Max(1, n);
            int ldvl = (job == LapackEigenvalues.NonSymmetricEigenValueVectorsJob.Right) ? 1 : Math.Max(1, n);
            int ldvr = (job == LapackEigenvalues.NonSymmetricEigenValueVectorsJob.Left) ? 1 : Math.Max(1, n);

            int info;
            _zhsein(ref jobz, ref eigsrc, ref initvz, select, ref n, h, ref ldh, w, vl, ref ldvl, vr, ref ldvr, ref mm, out m, work, rwork, ifaill, ifailr, out info);
            CheckForError(info, "zhsein");
        }

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
        public void dtrevc(LapackEigenvalues.NonSymmetricEigenValueVectorsJob job, LapackEigenvalues.NonSymmetricXtrevcOperation howmny, bool[] select, int n, double[] t, double[] vl, double[] vr, int mm, out int m, double[] work)
        {
            int info;
            var side = GetJob(job);
            var howmnyz = GetOperation(howmny);
            int ldt = Math.Max(1, n);
            int ldvl = (job == LapackEigenvalues.NonSymmetricEigenValueVectorsJob.Right) ? 1 : n;
            int ldvr = (job == LapackEigenvalues.NonSymmetricEigenValueVectorsJob.Left) ? 1 : n;

            _dtrevc(ref side, ref howmnyz, select, ref n, t, ref ldt, vl, ref ldvl, vr, ref ldvr, ref mm, out m, work, out info);
            CheckForError(info, "dtrevc");
        }

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
        public void ztrevc(LapackEigenvalues.NonSymmetricEigenValueVectorsJob job, LapackEigenvalues.NonSymmetricXtrevcOperation howmny, bool[] select, int n, Complex[] t, Complex[] vl, Complex[] vr, int mm, out int m, Complex[] work, double[] rwork)
        {
            int info;
            var side = GetJob(job);
            var howmnyz = GetOperation(howmny);
            int ldt = Math.Max(1, n);
            int ldvl = (job == LapackEigenvalues.NonSymmetricEigenValueVectorsJob.Right) ? 1 : n;
            int ldvr = (job == LapackEigenvalues.NonSymmetricEigenValueVectorsJob.Left) ? 1 : n;

            _ztrevc(ref side, ref howmnyz, select, ref n, t, ref ldt, vl, ref ldvl, vr, ref ldvr, ref mm, out m, work, rwork, out info);
            CheckForError(info, "ztrevc");
        }

        /// <summary>Reorders the Schur factorization of a general matrix.
        /// </summary>
        /// <param name="updatedSchurVectors">A value indicating whether the Schur vectors are updated.</param>
        /// <param name="n">The order of the matrix T.</param>
        /// <param name="t">The <paramref name="n"/>-by-<paramref name="n"/> matrix T.</param>
        /// <param name="q">The matrix Q; will not referenced if <paramref name="updatedSchurVectors"/> is <c>false</c>.</param>
        /// <param name="ifst">Specify the reordering of the diagonal elements of matrix T. The element with row index <c>ifst</c> is moved to row <c>ilst</c> by a sequence of exchanges between adjacent elements.</param>
        /// <param name="ilst">Specify the reordering of the diagonal elements of matrix T. The element with row index <c>ifst</c> is moved to row <c>ilst</c> by a sequence of exchanges between adjacent elements.</param>
        /// <param name="work">A workspace array with dimension at least <paramref name="n"/>.</param>
        public void dtrexc(bool updatedSchurVectors, int n, double[] t, double[] q, int ifst, int ilst, double[] work)
        {
            int info;
            int ldt = n;
            var compq = (updatedSchurVectors == true) ? 'V' : 'N';
            int ldq = (updatedSchurVectors == true) ? Math.Max(1, n) : 1;

            _dtrexc(ref compq, ref n, t, ref ldt, q, ref ldq, ref ifst, ref ilst, work, out info);
            CheckForError(info, "dtrexc");
        }

        /// <summary>Reorders the Schur factorization of a general matrix.
        /// </summary>
        /// <param name="updatedSchurVectors">A value indicating whether the Schur vectors are updated.</param>
        /// <param name="n">The order of the matrix T.</param>
        /// <param name="t">The <paramref name="n"/>-by-<paramref name="n"/> matrix T.</param>
        /// <param name="q">The matrix Q; will not referenced if <paramref name="updatedSchurVectors"/> is <c>false</c>.</param>
        /// <param name="ifst">Specify the reordering of the diagonal elements of matrix T. The element with row index <c>ifst</c> is moved to row <c>ilst</c> by a sequence of exchanges between adjacent elements.</param>
        /// <param name="ilst">Specify the reordering of the diagonal elements of matrix T. The element with row index <c>ifst</c> is moved to row <c>ilst</c> by a sequence of exchanges between adjacent elements.</param>
        public void ztrexc(bool updatedSchurVectors, int n, Complex[] t, Complex[] q, int ifst, int ilst)
        {
            int info;
            int ldt = n;
            var compq = (updatedSchurVectors == true) ? 'V' : 'N';
            int ldq = (updatedSchurVectors == true) ? Math.Max(1, n) : 1;

            _ztrexc(ref compq, ref n, t, ref ldt, q, ref ldq, ref ifst, ref ilst, out info);
            CheckForError(info, "ztrexc");
        }

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
        public void dtrsyl(BLAS.MatrixTransposeState transposeStateA, BLAS.MatrixTransposeState transposeStateB, int sign, int m, int n, double[] a, double[] b, double[] c, out double scale)
        {
            int info;
            int lda = Math.Max(1, m);
            int ldb = Math.Max(1, n);
            int ldc = Math.Max(1, n);

            var trana = LAPACK.GetTrans(transposeStateA);
            var tranb = LAPACK.GetTrans(transposeStateB);

            _dtrsyl(ref trana, ref tranb, ref sign, ref m, ref n, a, ref lda, b, ref ldb, c, ref ldc, out scale, out info);
            CheckForError(info, "dtrsyl");
        }

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
        public void ztrsyl(BLAS.MatrixTransposeState transposeStateA, BLAS.MatrixTransposeState transposeStateB, int sign, int m, int n, Complex[] a, Complex[] b, Complex[] c, out double scale)
        {
            int info;
            int lda = Math.Max(1, m);
            int ldb = Math.Max(1, n);
            int ldc = Math.Max(1, n);

            var trana = LAPACK.GetTrans(transposeStateA);
            var tranb = LAPACK.GetTrans(transposeStateB);

            _ztrsyl(ref trana, ref tranb, ref sign, ref m, ref n, a, ref lda, b, ref ldb, c, ref ldc, out scale, out info);
            CheckForError(info, "ztrsyl");
        }

        /// <summary>Gets a optimal workspace array length for the <c>driver_dgees</c> function.
        /// </summary>
        /// <param name="job">A value indicating whether eigenvalues or Schur form is required.</param>
        /// <param name="sort">Specifies whether or not to order the eigenvalues on the diagonal of the Schur form.</param>        
        /// <param name="n">The order of the matrix.</param>
        /// <returns>The optimal workspace array length.</returns>
        public int driver_dgeesQuery(LapackEigenvalues.NonSymmetricSchurVectorsJob job, LapackEigenvalues.NonSymmetricEigenvalueOrdering sort, int n)
        {
            unsafe
            {
                int info;
                int sdim;  // dummy value
                int lwork = -1;
                char jobz = GetJob(job);

                int ldh = n;
                int lda = Math.Max(1, n);
                int ldvs = (job == LapackEigenvalues.NonSymmetricSchurVectorsJob.None) ? 1 : Math.Max(1, n);
                char sortz = (sort == LapackEigenvalues.NonSymmetricEigenvalueOrdering.None) ? 'N' : 'S';

                double* work = stackalloc double[1];

                _driver_dgees(ref jobz, ref sortz, null, ref n, null, ref lda, out sdim, null, null, null, ref ldvs, work, ref lwork, null, out info);
                CheckForError(info, "driver_dgees");
                return (int)work[0] + 1;
            }
        }

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
        public void driver_dgees(LapackEigenvalues.NonSymmetricSchurVectorsJob job, LapackEigenvalues.NonSymmetricEigenvalueOrdering sort, Func<double, double, bool> select, int n, double[] a, out int sdim, double[] wr, double[] wi, double[] vs, double[] work, bool[] bwork)
        {
            int info;
            int lwork = work.Length;
            char jobz = GetJob(job);

            int ldh = n;
            int lda = Math.Max(1, n);
            int ldvs = (job == LapackEigenvalues.NonSymmetricSchurVectorsJob.None) ? 1 : Math.Max(1, n);
            char sortz = (sort == LapackEigenvalues.NonSymmetricEigenvalueOrdering.None) ? 'N' : 'S';
            var refSelect = new SelectEigenvaluesReal((ref double x, ref double y) => select(x, y));

            _driver_dgees(ref jobz, ref sortz, refSelect, ref n, a, ref lda, out sdim, wr, wi, vs, ref ldvs, work, ref lwork, bwork, out info);
            CheckForError(info, "driver_dgees");
        }

        /// <summary>Gets a optimal workspace array length for the <c>driver_zgees</c> function.
        /// </summary>
        /// <param name="job">A value indicating whether eigenvalues or Schur form is required.</param>
        /// <param name="sort">Specifies whether or not to order the eigenvalues on the diagonal of the Schur form.</param>        
        /// <param name="n">The order of the matrix.</param>
        /// <returns>The optimal workspace array length.</returns>
        public int driver_zgeesQuery(LapackEigenvalues.NonSymmetricSchurVectorsJob job, LapackEigenvalues.NonSymmetricEigenvalueOrdering sort, int n)
        {
            unsafe
            {
                int info;
                int sdim;  // dummy value
                int lwork = -1;
                char jobz = GetJob(job);

                int ldh = n;
                int lda = Math.Max(1, n);
                int ldvs = (job == LapackEigenvalues.NonSymmetricSchurVectorsJob.None) ? 1 : Math.Max(1, n);
                char sortz = (sort == LapackEigenvalues.NonSymmetricEigenvalueOrdering.None) ? 'N' : 'S';

                Complex* work = stackalloc Complex[1];

                _driver_zgees(ref jobz, ref sortz, null, ref n, null, ref lda, out sdim, null, null, ref ldvs, work, ref lwork, null, null, out info);
                CheckForError(info, "driver_zgees");
                return (int)work[0].Real + 1;
            }
        }

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
        public void driver_zgees(LapackEigenvalues.NonSymmetricSchurVectorsJob job, LapackEigenvalues.NonSymmetricEigenvalueOrdering sort, Func<Complex, bool> select, int n, Complex[] a, out int sdim, Complex[] w, Complex[] vs, Complex[] work, double[] rwork, bool[] bwork)
        {
            int info;
            int lwork = work.Length;
            char jobz = GetJob(job);

            int ldh = n;
            int lda = Math.Max(1, n);
            int ldvs = (job == LapackEigenvalues.NonSymmetricSchurVectorsJob.None) ? 1 : Math.Max(1, n);
            char sortz = (sort == LapackEigenvalues.NonSymmetricEigenvalueOrdering.None) ? 'N' : 'S';
            var refSelect = new SelectEigenvaluesComplex((ref Complex x) => select(x));

            _driver_zgees(ref jobz, ref sortz, refSelect, ref n, a, ref lda, out sdim, w, vs, ref ldvs, work, ref lwork, rwork, bwork, out info);
            CheckForError(info, "driver_zgees");
        }

        /// <summary>Gets a optimal workspace array length for the <c>driver_dgeesx</c> function.
        /// </summary>
        /// <param name="job">A value indicating whether eigenvalues or Schur form is required.</param>
        /// <param name="sort">Specifies whether or not to order the eigenvalues on the diagonal of the Schur form.</param>        
        /// <param name="n">The order of the matrix.</param>
        /// <param name="sense">Determines which reciprocal condition number are computed.</param>
        /// <returns>The optimal workspace array length.</returns>
        public int driver_dgeesxQuery(LapackEigenvalues.NonSymmetricSchurVectorsJob job, LapackEigenvalues.NonSymmetricEigenvalueOrdering sort, LapackEigenvalues.NonSymmetricSense sense, int n)
        {
            unsafe
            {
                int info;
                int sdim;  // dummy value
                double dummy;
                int lwork = -1;
                int liwork = -1;
                var jobz = GetJob(job);
                var sensez = GetSense(sense);

                int ldh = n;
                int lda = Math.Max(1, n);
                int ldvs = (job == LapackEigenvalues.NonSymmetricSchurVectorsJob.None) ? 1 : Math.Max(1, n);
                char sortz = (sort == LapackEigenvalues.NonSymmetricEigenvalueOrdering.None) ? 'N' : 'S';

                double* work = stackalloc double[1];

                _driver_dgeesx(ref jobz, ref sortz, null, ref sensez, ref n, null, ref lda, out sdim, null, null, null, ref ldvs, out dummy, out dummy, work, ref lwork, null, ref liwork, null, out info);
                CheckForError(info, "driver_dgeesx");
                return (int)work[0] + 1;
            }
        }

        /// <summary>Computes the eigenvalues and Schur factorization of a general matrix, orders the factorization and computes reciprocal condition numbers.
        /// </summary>
        /// <param name="job">A value indicating whether eigenvalues or Schur form is required.</param>
        /// <param name="sort">Specifies whether or not to order the eigenvalues on the diagonal of the Schur form.</param>        
        /// <param name="select">A function that is used to select eigenvalues to sort to the top left of the Schur form.</param>
        /// <param name="sense">Determines which reciprocal condition number are computed.</param>
        /// <param name="n">The order of the matrix.</param>
        /// <param name="a">Contains the <paramref name="n"/>-by-<paramref name="n"/> matrix A; overwritten by the real-Schur form T.</param>
        /// <param name="sdim">The number of eigenvalues (after sorting) for which <paramref name="select"/> is <c>true</c>; otherwise <c>0</c> (output).</param>
        /// <param name="wr">Contains the real parts of the eigenvalues of the matrix H (output).</param>
        /// <param name="wi">Contains the imaginary parts of the eigenvalues of the matrix H (output).</param>
        /// <param name="vs">Contains the ortogonal matrix Z of Schur vectors as specified by <paramref name="job"/> (output).</param>
        /// <param name="rconde">The reciprocal condition number for the average of the selected eigenvalues as specified by <paramref name="sense"/> (output).</param>
        /// <param name="rcondv">The reciprocal condition number for th selected right invariant subspace as specified by <paramref name="sense"/> (output).</param>
        /// <param name="work">A workspace array.</param>
        /// <param name="iwork">A workspace array with dimension at least <paramref name="sdim"/> * ( <paramref name="n"/> - <paramref name="sdim"/>).</param>
        /// <param name="bwork">A workspace array of length at least <paramref name="n"/>.</param>
        public void driver_dgeesx(LapackEigenvalues.NonSymmetricSchurVectorsJob job, LapackEigenvalues.NonSymmetricEigenvalueOrdering sort, Func<double, double, bool> select, LapackEigenvalues.NonSymmetricSense sense, int n, double[] a, out int sdim, double[] wr, double[] wi, double[] vs, out double rconde, out double rcondv, double[] work, int[] iwork, bool[] bwork)
        {
            int info;
            int lwork = work.Length;
            int liwork = iwork.Length;
            char jobz = GetJob(job);

            int ldh = n;
            int lda = Math.Max(1, n);
            int ldvs = (job == LapackEigenvalues.NonSymmetricSchurVectorsJob.None) ? 1 : Math.Max(1, n);
            char sensez = GetSense(sense);
            char sortz = (sort == LapackEigenvalues.NonSymmetricEigenvalueOrdering.None) ? 'N' : 'S';
            var refSelect = new SelectEigenvaluesReal((ref double x, ref double y) => select(x, y));

            _driver_dgeesx(ref jobz, ref sortz, refSelect, ref sensez, ref n, a, ref lda, out sdim, wr, wi, vs, ref ldvs, out rconde, out rcondv, work, ref lwork, iwork, ref liwork, bwork, out info);
            CheckForError(info, "driver_dgeesx");
        }

        /// <summary>Gets a optimal workspace array length for the <c>driver_zgeesx</c> function.
        /// </summary>
        /// <param name="job">A value indicating whether eigenvalues or Schur form is required.</param>
        /// <param name="sort">Specifies whether or not to order the eigenvalues on the diagonal of the Schur form.</param>        
        /// <param name="n">The order of the matrix.</param>
        /// <param name="sense">Determines which reciprocal condition number are computed.</param>
        /// <returns>The optimal workspace array length.</returns>
        public int driver_zgeesxQuery(LapackEigenvalues.NonSymmetricSchurVectorsJob job, LapackEigenvalues.NonSymmetricEigenvalueOrdering sort, LapackEigenvalues.NonSymmetricSense sense, int n)
        {
            unsafe
            {
                int info;
                int sdim;  // dummy value
                double dummy;
                int lwork = -1;
                var jobz = GetJob(job);
                var sensez = GetSense(sense);

                int ldh = n;
                int lda = Math.Max(1, n);
                int ldvs = (job == LapackEigenvalues.NonSymmetricSchurVectorsJob.None) ? 1 : Math.Max(1, n);
                char sortz = (sort == LapackEigenvalues.NonSymmetricEigenvalueOrdering.None) ? 'N' : 'S';

                Complex* work = stackalloc Complex[1];

                _driver_zgeesx(ref jobz, ref sortz, null, ref sensez, ref n, null, ref lda, out sdim, null, null, ref ldvs, out dummy, out dummy, work, ref lwork, null, null, out info);
                CheckForError(info, "driver_zgeesx");
                return (int)work[0].Real + 1;
            }
        }

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
        public void driver_zgeesx(LapackEigenvalues.NonSymmetricSchurVectorsJob job, LapackEigenvalues.NonSymmetricEigenvalueOrdering sort, Func<Complex, bool> select, LapackEigenvalues.NonSymmetricSense sense, int n, Complex[] a, out int sdim, Complex[] w, Complex[] vs, out double rconde, out double rcondv, Complex[] work, double[] rwork, bool[] bwork)
        {
            int info;
            int lwork = work.Length;
            char jobz = GetJob(job);

            int ldh = n;
            int lda = Math.Max(1, n);
            int ldvs = (job == LapackEigenvalues.NonSymmetricSchurVectorsJob.None) ? 1 : Math.Max(1, n);
            char sensez = GetSense(sense);
            char sortz = (sort == LapackEigenvalues.NonSymmetricEigenvalueOrdering.None) ? 'N' : 'S';
            var refSelect = new SelectEigenvaluesComplex((ref Complex x) => select(x));

            _driver_zgeesx(ref jobz, ref sortz, refSelect, ref sensez, ref n, a, ref lda, out sdim, w, vs, ref ldvs, out rconde, out rcondv, work, ref lwork, rwork, bwork, out info);
            CheckForError(info, "driver_zgeesx");
        }

        /// <summary>Gets a optimal workspace array length for the <c>driver_dgeev</c> function.
        /// </summary>
        /// <param name="computeLeftEigenvectors">A value indicating whether to calculate left eigenvectors.</param>
        /// <param name="computeRightEigenvectors">A value indicating whether to calculate right eigenvectors.</param>
        /// <param name="n">The order of the matrix.</param>
        /// <returns>The optimal workspace array length.</returns>
        public int driver_dgeevQuery(bool computeLeftEigenvectors, bool computeRightEigenvectors, int n)
        {
            var jobvl = computeLeftEigenvectors ? 'V' : 'N';
            var jobvr = computeRightEigenvectors ? 'V' : 'N';

            int lda = Math.Max(1, n);
            int ldvl = computeLeftEigenvectors ? Math.Max(1, n) : 1;
            int ldvr = computeRightEigenvectors ? Math.Max(1, n) : 1;

            int lwork = -1;
            int info;

            unsafe
            {
                double* work = stackalloc double[1];

                _driver_dgeev(ref jobvl, ref jobvr, ref n, null, ref lda, null, null, null, ref ldvl, null, ref ldvr, work, ref lwork, out info);
                CheckForError(info, "driver_dgeev");
                return (int)work[0] + 1;
            }
        }

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
        public void driver_dgeev(bool computeLeftEigenvectors, bool computeRightEigenvectors, int n, double[] a, double[] wr, double[] wi, double[] vl, double[] vr, double[] work)
        {
            var jobvl = computeLeftEigenvectors ? 'V' : 'N';
            var jobvr = computeRightEigenvectors ? 'V' : 'N';

            int lda = Math.Max(1, n);
            int ldvl = computeLeftEigenvectors ? Math.Max(1, n) : 1;
            int ldvr = computeRightEigenvectors ? Math.Max(1, n) : 1;

            int lwork = work.Length;
            int info;

            _driver_dgeev(ref jobvl, ref jobvr, ref n, a, ref lda, wr, wi, vl, ref ldvl, vr, ref ldvr, work, ref lwork, out info);
            CheckForError(info, "driver_dgeev");
        }

        /// <summary>Gets a optimal workspace array length for the <c>driver_zgeev</c> function.
        /// </summary>
        /// <param name="computeLeftEigenvectors">A value indicating whether to calculate left eigenvectors.</param>
        /// <param name="computeRightEigenvectors">A value indicating whether to calculate right eigenvectors.</param>
        /// <param name="n">The order of the matrix.</param>
        /// <returns>The optimal workspace array length.</returns>
        public int driver_zgeevQuery(bool computeLeftEigenvectors, bool computeRightEigenvectors, int n)
        {
            var jobvl = computeLeftEigenvectors ? 'V' : 'N';
            var jobvr = computeRightEigenvectors ? 'V' : 'N';

            int lda = Math.Max(1, n);
            int ldvl = computeLeftEigenvectors ? Math.Max(1, n) : 1;
            int ldvr = computeRightEigenvectors ? Math.Max(1, n) : 1;

            int lwork = -1;
            int info;

            unsafe
            {
                Complex* work = stackalloc Complex[1];

                _driver_zgeev(ref jobvl, ref jobvr, ref n, null, ref lda, null, null, ref ldvl, null, ref ldvr, work, ref lwork, null, out info);
                CheckForError(info, "driver_zgeev");
                return (int)work[0].Real + 1;
            }
        }

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
        public void driver_zgeev(bool computeLeftEigenvectors, bool computeRightEigenvectors, int n, Complex[] a, Complex[] w, Complex[] vl, Complex[] vr, Complex[] work, double[] rwork)
        {
            var jobvl = computeLeftEigenvectors ? 'V' : 'N';
            var jobvr = computeRightEigenvectors ? 'V' : 'N';

            int lda = Math.Max(1, n);
            int ldvl = computeLeftEigenvectors ? Math.Max(1, n) : 1;
            int ldvr = computeRightEigenvectors ? Math.Max(1, n) : 1;

            int lwork = work.Length;
            int info;

            _driver_zgeev(ref jobvl, ref jobvr, ref n, a, ref lda, w, vl, ref ldvl, vr, ref ldvr, work, ref lwork, rwork, out info);
            CheckForError(info, "driver_zgeev");
        }

        /// <summary>Gets a optimal workspace array length for the <c>driver_dgeevx</c> function.
        /// </summary>
        /// <param name="balanceType">Indicates how the input matrix should be diagonally scaled and/or permuted to improve the conditioning of its eigenvalues.</param>
        /// <param name="computeLeftEigenvectors">A value indicating whether to calculate left eigenvectors.</param>
        /// <param name="computeRightEigenvectors">A value indicating whether to calculate right eigenvectors.</param>
        /// <param name="sense">Determines which reciprocal condition number are computed.</param>
        /// <param name="n">The order of the matrix.</param>
        /// <returns>The optimal workspace array length.</returns>
        public int driver_dgeevxQuery(LapackEigenvalues.NonSymmetricMatrixBalancesType balanceType, bool computeLeftEigenvectors, bool computeRightEigenvectors, LapackEigenvalues.NonSymmetricXgeevxSense sense, int n)
        {
            var jobvl = computeLeftEigenvectors ? 'V' : 'N';
            var jobvr = computeRightEigenvectors ? 'V' : 'N';
            var balanc = GetJob(balanceType);
            var sensez = GetSense(sense);

            int lda = Math.Max(1, n);
            int ldvl = computeLeftEigenvectors ? Math.Max(1, n) : 1;
            int ldvr = computeRightEigenvectors ? Math.Max(1, n) : 1;

            int lwork = -1;
            int info;

            double dummy;
            int idummy;

            unsafe
            {
                double* work = stackalloc double[1];

                _driver_dgeevx(ref balanc, ref jobvl, ref jobvr, ref sensez, ref n, null, ref lda, null, null, null, ref ldvl, null, ref ldvr, out idummy, out idummy, null, out dummy, null, null, work, ref lwork, null, out info);
                CheckForError(info, "driver_dgeevx");
                return (int)work[0] + 1;
            }
        }

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
        /// <param name="abnrm">The one-norm of the balanced matrix (the maximum of the sum of absolute values of elements of any column) (output).</param>
        /// <param name="rconde">For each eigenvalue the corresponding reciprocal condition number (output).</param>
        /// <param name="rcondv">For each right eigenvector the corresponding reciprocal condition number (output).</param>
        /// <param name="work">A workspace array.</param>
        /// <param name="iwork">A workspace array with dimension at least 2 * <paramref name="n"/> - 2.</param>
        public void driver_dgeevx(LapackEigenvalues.NonSymmetricMatrixBalancesType balanceType, bool computeLeftEigenvectors, bool computeRightEigenvectors, LapackEigenvalues.NonSymmetricXgeevxSense sense, int n, double[] a, double[] wr, double[] wi, double[] vl, double[] vr, out int ilo, out int ihi, double[] scale, out double abnrm, double[] rconde, double[] rcondv, double[] work, int[] iwork)
        {
            var jobvl = computeLeftEigenvectors ? 'V' : 'N';
            var jobvr = computeRightEigenvectors ? 'V' : 'N';
            var balanc = GetJob(balanceType);
            var sensez = GetSense(sense);

            int lda = Math.Max(1, n);
            int ldvl = computeLeftEigenvectors ? Math.Max(1, n) : 1;
            int ldvr = computeRightEigenvectors ? Math.Max(1, n) : 1;

            int lwork = work.Length;
            int info;

            _driver_dgeevx(ref balanc, ref jobvl, ref jobvr, ref sensez, ref n, a, ref lda, wr, wi, vl, ref ldvl, vr, ref ldvr, out ilo, out ihi, scale, out abnrm, rconde, rcondv, work, ref lwork, iwork, out info);
            CheckForError(info, "driver_dgeevx");
        }

        /// <summary>Gets a optimal workspace array length for the <c>driver_zgeevx</c> function.
        /// </summary>
        /// <param name="balanceType">Indicates how the input matrix should be diagonally scaled and/or permuted to improve the conditioning of its eigenvalues.</param>
        /// <param name="computeLeftEigenvectors">A value indicating whether to calculate left eigenvectors.</param>
        /// <param name="computeRightEigenvectors">A value indicating whether to calculate right eigenvectors.</param>
        /// <param name="sense">Determines which reciprocal condition number are computed.</param>
        /// <param name="n">The order of the matrix.</param>
        /// <returns>The optimal workspace array length.</returns>
        public int driver_zgeevxQuery(LapackEigenvalues.NonSymmetricMatrixBalancesType balanceType, bool computeLeftEigenvectors, bool computeRightEigenvectors, LapackEigenvalues.NonSymmetricXgeevxSense sense, int n)
        {
            var jobvl = computeLeftEigenvectors ? 'V' : 'N';
            var jobvr = computeRightEigenvectors ? 'V' : 'N';
            var balanc = GetJob(balanceType);
            var sensez = GetSense(sense);

            int lda = Math.Max(1, n);
            int ldvl = computeLeftEigenvectors ? Math.Max(1, n) : 1;
            int ldvr = computeRightEigenvectors ? Math.Max(1, n) : 1;

            int lwork = -1;
            int info;

            double dummy;
            int idummy;

            unsafe
            {
                Complex* work = stackalloc Complex[1];

                _driver_zgeevx(ref balanc, ref jobvl, ref jobvr, ref sensez, ref n, null, ref lda, null, null, ref ldvl, null, ref ldvr, out idummy, out idummy, null, out dummy, null, null, work, ref lwork, null, out info);
                CheckForError(info, "driver_zgeevx");
                return (int)work[0].Real + 1;
            }
        }

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
        /// <param name="abnrm">The one-norm of the balanced matrix (the maximum of the sum of absolute values of elements of any column) (output).</param>
        /// <param name="rconde">For each eigenvalue the corresponding reciprocal condition number (output).</param>
        /// <param name="rcondv">For each right eigenvector the corresponding reciprocal condition number (output).</param>
        /// <param name="work">A workspace array.</param>
        /// <param name="rwork">A workspace array with dimension at least 2 * <paramref name="n"/>.</param>
        public void driver_zgeevx(LapackEigenvalues.NonSymmetricMatrixBalancesType balanceType, bool computeLeftEigenvectors, bool computeRightEigenvectors, LapackEigenvalues.NonSymmetricXgeevxSense sense, int n, Complex[] a, Complex[] w, Complex[] vl, Complex[] vr, out int ilo, out int ihi, double[] scale, out double abnrm, double[] rconde, double[] rcondv, Complex[] work, double[] rwork)
        {
            var jobvl = computeLeftEigenvectors ? 'V' : 'N';
            var jobvr = computeRightEigenvectors ? 'V' : 'N';
            var balanc = GetJob(balanceType);
            var sensez = GetSense(sense);

            int lda = Math.Max(1, n);
            int ldvl = computeLeftEigenvectors ? Math.Max(1, n) : 1;
            int ldvr = computeRightEigenvectors ? Math.Max(1, n) : 1;

            int lwork = work.Length;
            int info;

            _driver_zgeevx(ref balanc, ref jobvl, ref jobvr, ref sensez, ref n, a, ref lda, w, vl, ref ldvl, vr, ref ldvr, out ilo, out ihi, scale, out abnrm, rconde, rcondv, work, ref lwork, rwork, out info);
            CheckForError(info, "driver_zgeevx");
        }
        #endregion

        #region private methods

        private char GetSense(LapackEigenvalues.NonSymmetricXgeevxSense sense)
        {
            switch (sense)
            {
                case LapackEigenvalues.NonSymmetricXgeevxSense.Eigenvalues:
                    return 'E';
                case LapackEigenvalues.NonSymmetricXgeevxSense.RightEigenvectors:
                    return 'V';
                case LapackEigenvalues.NonSymmetricXgeevxSense.EigenvaluesAndrightEigenvectors:
                    return 'B';

                case LapackEigenvalues.NonSymmetricXgeevxSense.None:
                    return 'N';
                default:
                    throw new NotImplementedException();
            }
        }

        private char GetSense(LapackEigenvalues.NonSymmetricSense sense)
        {
            switch (sense)
            {
                case LapackEigenvalues.NonSymmetricSense.AverageOfSelectedEigenvalues:
                    return 'E';
                case LapackEigenvalues.NonSymmetricSense.SelectedRightInvariantSubspace:
                    return 'V';
                case LapackEigenvalues.NonSymmetricSense.All:
                    return 'B';
                case LapackEigenvalues.NonSymmetricSense.None:
                    return 'N';
                default:
                    throw new NotImplementedException();
            }
        }

        private char GetJob(LapackEigenvalues.NonSymmetricSchurVectorsJob job)
        {
            switch (job)
            {
                case LapackEigenvalues.NonSymmetricSchurVectorsJob.None:
                    return 'N';
                case LapackEigenvalues.NonSymmetricSchurVectorsJob.Compute:
                    return 'V';
                default:
                    throw new NotImplementedException();
            }
        }

        private char GetInit(LapackEigenvalues.NonSymmetricXhseinInitV initv)
        {
            switch (initv)
            {
                case LapackEigenvalues.NonSymmetricXhseinInitV.InitialEstimates:
                    return 'U';
                case LapackEigenvalues.NonSymmetricXhseinInitV.None:
                    return 'N';
                default:
                    throw new NotImplementedException();
            }
        }

        private char GetSource(LapackEigenvalues.NonSymmetricEigenvalueSource eigenvalueSource)
        {
            switch (eigenvalueSource)
            {
                case LapackEigenvalues.NonSymmetricEigenvalueSource.Xhseqr:
                    return 'Q';
                case LapackEigenvalues.NonSymmetricEigenvalueSource.Unknown:
                    return 'N';
                default:
                    throw new NotImplementedException();
            }
        }

        private char GetJob(LapackEigenvalues.NonSymmetricEigenValueVectorsJob job)
        {
            switch (job)
            {
                case LapackEigenvalues.NonSymmetricEigenValueVectorsJob.Left:
                    return 'L';
                case LapackEigenvalues.NonSymmetricEigenValueVectorsJob.Right:
                    return 'R';
                case LapackEigenvalues.NonSymmetricEigenValueVectorsJob.All:
                    return 'B';
                default:
                    throw new NotImplementedException();
            }
        }

        private char GetJob(LapackEigenvalues.NonSymmetricMatrixBalancesType job)
        {
            switch (job)
            {
                case LapackEigenvalues.NonSymmetricMatrixBalancesType.NeitherPermutedNorScaled:
                    return 'N';
                case LapackEigenvalues.NonSymmetricMatrixBalancesType.Permuted:
                    return 'P';
                case LapackEigenvalues.NonSymmetricMatrixBalancesType.PermutedAndScaled:
                    return 'B';
                case LapackEigenvalues.NonSymmetricMatrixBalancesType.Scaled:
                    return 'S';
                default:
                    throw new NotImplementedException();
            }
        }

        private char GetJob(LapackEigenvalues.NonSymmetricXhseqrJob job)
        {
            switch (job)
            {
                case LapackEigenvalues.NonSymmetricXhseqrJob.EigenvaluesOnly:
                    return 'E';
                case LapackEigenvalues.NonSymmetricXhseqrJob.EigenvaluesAndSchurForm:
                    return 'S';
                default: throw new NotImplementedException();
            }
        }

        private char GetOperation(LapackEigenvalues.NonSymmetricXhseqrOperation operation)
        {
            switch (operation)
            {
                case LapackEigenvalues.NonSymmetricXhseqrOperation.NoSchurVectors:
                    return 'N';
                case LapackEigenvalues.NonSymmetricXhseqrOperation.SchurVectorOfH:
                    return 'I';
                case LapackEigenvalues.NonSymmetricXhseqrOperation.SchurVectorOfA:
                    return 'V';
                default:
                    throw new NotImplementedException();
            }
        }

        private char GetOperation(LapackEigenvalues.NonSymmetricXtrevcOperation howmny)
        {
            switch (howmny)
            {
                case LapackEigenvalues.NonSymmetricXtrevcOperation.All:
                    return 'A';
                case LapackEigenvalues.NonSymmetricXtrevcOperation.AllBacktransformed:
                    return 'B';
                case LapackEigenvalues.NonSymmetricXtrevcOperation.SelectedEigenvectors:
                    return 'S';
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion
    }
}