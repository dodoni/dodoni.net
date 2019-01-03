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
using System.Runtime.CompilerServices;

namespace Dodoni.MathLibrary.Basics.LowLevel.Native
{
    public partial class BlasNativeWrapper
    {
        /// <summary>The wrapper for the BLAS level 1 methods (Fortran interface). See http://www.netlib.org/blas for further information.
        /// </summary>
        protected internal class Level1BLAS : ILevel1BLAS
        {
            #region private function import

#if LOWER_CASE_UNDERSCORE

            #region double precision methods

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "dasum_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern double _dasum(ref int n, IntPtr x, ref int incX);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "daxpy_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _daxpy(ref int n, ref double alpha, IntPtr x, ref int incX, IntPtr y, ref int incY);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "dcopy_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dcopy(ref int n, IntPtr x, ref int incX, IntPtr y, ref int incY);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "ddot_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern double _ddot(ref int n, IntPtr x, ref int incX, IntPtr y, ref int incY);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "dnrm2_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern double _dnrm2(ref int n, IntPtr x, ref int incX);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "drot_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern double _drot(ref int n, IntPtr x, ref int incX, IntPtr y, ref int incY, ref double c, ref double s);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "drotg_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern double _drotg(ref double a, ref double b, out double c, out double s);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "drotm_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern double _drotm(ref int n, IntPtr x, ref int incX, IntPtr y, ref int incY, IntPtr param);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "drotmg_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern double _drotmg(ref double d1, ref double d2, ref double x1, ref double y1, IntPtr param);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "dscal_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dscal(ref int n, ref double alpha, IntPtr x, ref int incX);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "dswap_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dswap(ref int n, IntPtr x, ref int incX, IntPtr y, ref int incY);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "idamax_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern int _idamax(ref int n, IntPtr x, ref int incX);
            #endregion

            #region complex methods

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "zasum_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern double _zasum(ref int n, IntPtr x, ref int incX);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "zaxpy_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zaxpy(ref int n, ref Complex alpha, IntPtr x, ref int incX, IntPtr y, ref int incY);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "zcopy_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zcopy(ref int n, IntPtr x, ref int incX, IntPtr y, ref int incY);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "zdotc_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern Complex _zdotc(ref int n, IntPtr x, ref int incX, IntPtr y, ref int incY);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "zdotu_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern Complex _zdotu(ref int n, IntPtr x, ref int incX, IntPtr y, ref int incY);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "znrm2_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern double _znrm2(ref int n, IntPtr x, ref int incX);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "zdrot_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern double _zdrot(ref int n, IntPtr x, ref int incX, IntPtr y, ref int incY, ref double c, ref double s);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "zrotg_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern double _zrotg(ref Complex a, ref Complex b, out double c, out Complex s);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "zscal_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zscal(ref int n, ref Complex alpha, IntPtr x, ref int incX);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "zdscal_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zdscal(ref int n, ref double alpha, IntPtr x, ref int incX);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "zswap_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zswap(ref int n, IntPtr x, ref int incX, IntPtr y, ref int incY);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "izamax_", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern int _izamax(ref int n, IntPtr x, ref int incX);
            #endregion
#else
            #region double precision methods

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "DASUM", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern double _dasum(ref int n, IntPtr x, ref int incX);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "DAXPY", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _daxpy(ref int n, ref double alpha, IntPtr x, ref int incX, IntPtr y, ref int incY);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "DCOPY", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dcopy(ref int n, IntPtr x, ref int incX, IntPtr y, ref int incY);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "DDOT", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern double _ddot(ref int n, IntPtr x, ref int incX, IntPtr y, ref int incY);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "DNRM2", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern double _dnrm2(ref int n, IntPtr x, ref int incX);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "DROT", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern double _drot(ref int n, IntPtr x, ref int incX, IntPtr y, ref int incY, ref double c, ref double s);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "DROTG", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern double _drotg(ref double a, ref double b, out double c, out double s);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "DROTM", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern double _drotm(ref int n, IntPtr x, ref int incX, IntPtr y, ref int incY, IntPtr param);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "DROTMG", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern double _drotmg(ref double d1, ref double d2, ref double x1, ref double y1, IntPtr param);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "DSCAL", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dscal(ref int n, ref double alpha, IntPtr x, ref int incX);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "DSWAP", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dswap(ref int n, IntPtr x, ref int incX, IntPtr y, ref int incY);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "IDAMAX", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern int _idamax(ref int n, IntPtr x, ref int incX);
            #endregion

            #region complex methods

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "DZASUM", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern double _zasum(ref int n, IntPtr x, ref int incX);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "ZAXPY", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zaxpy(ref int n, ref Complex alpha, IntPtr x, ref int incX, IntPtr y, ref int incY);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "ZCOPY", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zcopy(ref int n, IntPtr x, ref int incX, IntPtr y, ref int incY);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "ZDOTC", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern Complex _zdotc(ref int n, IntPtr x, ref int incX, IntPtr y, ref int incY);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "ZDOTU", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern Complex _zdotu(ref int n, IntPtr x, ref int incX, IntPtr y, ref int incY);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "DZNRM2", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern double _znrm2(ref int n, IntPtr x, ref int incX);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "ZDROT", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern double _zdrot(ref int n, IntPtr x, ref int incX, IntPtr y, ref int incY, ref double c, ref double s);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "ZROTG", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern double _zrotg(ref Complex a, ref Complex b, out double c, out Complex s);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "ZSCAL", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zscal(ref int n, ref Complex alpha, IntPtr x, ref int incX);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "ZDSCAL", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zdscal(ref int n, ref double alpha, IntPtr x, ref int incX);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "ZSWAP", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zswap(ref int n, IntPtr x, ref int incX, IntPtr y, ref int incY);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "IZAMAX", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern int _izamax(ref int n, IntPtr x, ref int incX);
            #endregion
#endif
            #endregion

            #region public constructors

            /// <summary>Initializes a new instance of the <see cref="Level1BLAS"/> class.
            /// </summary>
            public Level1BLAS()
            {
            }
            #endregion

            #region public methods

            #region ILevel1BLAS Members

            #region double precision methods

            /// <summary>Compute the sum of the magnitudes of elements of some vector, i.e.
            /// <para>|x(0)|+ |x(incX)| + |x(2 * incX)| +... + |x(k * incX)|.</para>
            /// </summary>
            /// <param name="n">The number of elements in <paramref name="x"/> with at least
            /// <c>(1 + (<paramref name="n"/>-1)*abs(<paramref name="incX"/>))</c> elements.</param>
            /// <param name="x">The vector.</param>
            /// <param name="incX">The increment for indexing vector <paramref name="x"/>.</param>
            /// <returns>The sum of magnitudes of elements of the vector <paramref name="x"/>.
            /// </returns>
            public double dasum(int n, double[] x, int incX)
            {
                unsafe
                {
                    fixed (double* ptrX = x)
                    {
                        return _dasum(ref n, (IntPtr)ptrX, ref incX);
                    }
                }
            }

            /// <summary>Compute the sum of the magnitudes of elements of some vector, i.e.
            /// <para>|x(s)|+ |x(s + incX)| + |x(s + 2 * incX)| +... + |x(s+ k * incX)|, where s represents the null-based start index <paramref name="startIndexX"/>.</para>
            /// </summary>
            /// <param name="n">The number of elements in <paramref name="x"/> with at least
            /// <c><paramref name="startIndexX"/> + (1 + (<paramref name="n"/>-1)*abs(<paramref name="incX"/>))</c> elements.</param>
            /// <param name="x">The vector.</param>
            /// <param name="incX">The increment for indexing vector <paramref name="x"/>.</param>
            /// <param name="startIndexX">The null-based start index for <paramref name="x"/>.</param>
            /// <returns>The sum of magnitudes of elements of the vector <paramref name="x"/>.</returns>
            public double dasum(int n, double[] x, int incX, int startIndexX = 0)
            {
                unsafe
                {
                    fixed (double* ptrX = &x[startIndexX])
                    {
                        return _dasum(ref n, (IntPtr)ptrX, ref incX);
                    }
                }
            }

            /// <summary>Perform a vector-vector operation defined as y := a*x + y, i.e. scalar constant times vector plus a vector.
            /// </summary>
            /// <param name="n">The number of elements of <paramref name="x"/> and <paramref name="y"/>.</param>
            /// <param name="a">The scalar factor 'a'.</param>
            /// <param name="x">The vector 'x' with at least <paramref name="n"/> elements.</param>
            /// <param name="y">The vector 'y' with at least <paramref name="n"/> elements; contains the updated vector on exit.</param>
            public void daxpy(int n, double a, double[] x, double[] y)
            {
                unsafe
                {
                    fixed (double* ptrX = x, ptrY = y)
                    {
                        int inc = 1;
                        _daxpy(ref n, ref a, (IntPtr)ptrX, ref inc, (IntPtr)ptrY, ref inc);
                    }
                }
            }

            /// <summary>Perform a vector-vector operation defined as
            /// <para>
            /// y[<paramref name="startIndexY"/> + k * <paramref name="incY"/>] += <paramref name="a"/> * x[<paramref name="startIndexX"/> + k * <paramref name="incX"/>]
            /// </para>
            /// for k=0,..,<paramref name="n"/>-1. Thus some partial scalar times vector plus vector operation.
            /// </summary>
            /// <param name="n">The number of elements to add.</param>
            /// <param name="a">The scalar factor 'a'.</param>
            /// <param name="x">The vector 'x' with at least 1 + <paramref name="startIndexX"/> + ( <paramref name="n"/> - 1) * <paramref name="incX"/> elements.</param>
            /// <param name="y">The vector 'y' with at least 1 + <paramref name="startIndexY"/> + ( <paramref name="n"/> - 1) * <paramref name="incY"/> elements; contains the updated vector on exit.</param>
            /// <param name="incX">The increment for <paramref name="x"/>.</param>
            /// <param name="incY">The increment for <paramref name="y"/>.</param>
            /// <param name="startIndexX">The null-based start index for <paramref name="x"/>.</param>
            /// <param name="startIndexY">The null-based start index for <paramref name="y"/>.</param>
            public void daxpy(int n, double a, double[] x, double[] y, int incX, int incY, int startIndexX, int startIndexY)
            {
                unsafe
                {
                    fixed (double* ptrX = &x[startIndexX], ptrY = &y[startIndexY])
                    {
                        _daxpy(ref n, ref a, (IntPtr)ptrX, ref incX, (IntPtr)ptrY, ref incY);
                    }
                }
            }

            /// <summary>Copies a vector to another vector, i.e. y = x.
            /// </summary>
            /// <param name="n">The number of elements of <paramref name="x"/> and <paramref name="y"/>.</param>
            /// <param name="x">The vector 'x' with at least <paramref name="n"/> elements.</param>
            /// <param name="y">The vector 'y' with at least <paramref name="n"/> elements;
            /// contains a copy of the vector <paramref name="x"/> after function evaluation, if <paramref name="n"/> is positive.</param>
            public void dcopy(int n, double[] x, double[] y)
            {
                unsafe
                {
                    fixed (double* ptrX = x, ptrY = y)
                    {
                        int inc = 1;
                        _dcopy(ref n, (IntPtr)ptrX, ref inc, (IntPtr)ptrY, ref inc);
                    }
                }
            }

            /// <summary>Copies a vector to another vector, i.e.
            /// <para>
            /// y[<paramref name="startIndexY"/> + k * <paramref name="incY"/>] = x[<paramref name="startIndexX"/> + k * <paramref name="incX"/>]
            /// </para> for k=0,..,<paramref name="n"/>-1.
            /// </summary>
            /// <param name="n">The number of elements to copy.</param>
            /// <param name="x">The vector 'x' with at least 1 + <paramref name="startIndexX"/> + ( <paramref name="n"/> - 1) * <paramref name="incX"/> elements.</param>
            /// <param name="y">The vector 'y' with at least 1 + <paramref name="startIndexY"/> + ( <paramref name="n"/> - 1) * <paramref name="incY"/> elements; contains the updated vector on exit.</param>
            /// <param name="incX">The increment for <paramref name="x"/>.</param>
            /// <param name="incY">The increment for <paramref name="y"/>.</param>
            /// <param name="startIndexX">The null-based start index for <paramref name="x"/>.</param>
            /// <param name="startIndexY">The null-based start index for <paramref name="y"/>.</param>
            public void dcopy(int n, double[] x, double[] y, int incX, int incY, int startIndexX, int startIndexY)
            {
                unsafe
                {
                    fixed (double* ptrX = &x[startIndexX], ptrY = &y[startIndexY])
                    {
                        _dcopy(ref n, (IntPtr)ptrX, ref incX, (IntPtr)ptrY, ref incY);
                    }
                }
            }

            /// <summary>Computes a vector-vector dot product, i.e. \sum_j x_j*y_j.
            /// </summary>
            /// <param name="n">The number of elements of <paramref name="x"/> and <paramref name="y"/>.</param>
            /// <param name="x">The vector 'x' with at least <paramref name="n"/> elements.</param>
            /// <param name="y">The vector 'y' with at least <paramref name="n"/> elements.</param>
            /// <returns>The dot product of <paramref name="x"/> and <paramref name="y"/>, i.e. \sum_j x_j * y_j.
            /// </returns>
            public double ddot(int n, double[] x, double[] y)
            {
                int inc = 1;
                unsafe
                {
                    fixed (double* ptrX = x, ptrY = y)
                    {
                        return _ddot(ref n, (IntPtr)ptrX, ref inc, (IntPtr)ptrY, ref inc);
                    }
                }
            }

            /// <summary>Computes a vector-vector dot product, i.e. \sum_j x_{<paramref name="startIndexX"/> + j * <paramref name="incX"/>} * y_{<paramref name="startIndexY"/> + j * <paramref name="incY"/>).
            /// </summary>
            /// <param name="n">The number of elements of <paramref name="x"/> and <paramref name="y"/>.</param>
            /// <param name="x">The vector 'x' with at least <paramref name="n"/> elements.</param>
            /// <param name="y">The vector 'y' with at least <paramref name="n"/> elements.</param>
            /// <param name="incX">The increment for <paramref name="x"/>.</param>
            /// <param name="incY">The increment for <paramref name="y"/>.</param>
            /// <param name="startIndexX">The null-based start index for <paramref name="x"/>.</param>
            /// <param name="startIndexY">The null-based start index for <paramref name="y"/>.</param>
            /// <returns>The dot product of <paramref name="x"/> and <paramref name="y"/>, i.e. \sum_j x_{<paramref name="startIndexX"/> + j * <paramref name="incX"/>} * y_{<paramref name="startIndexY"/> + j * <paramref name="incY"/>).
            /// </returns>
            public double ddot(int n, double[] x, double[] y, int incX, int incY, int startIndexX, int startIndexY)
            {
                unsafe
                {
                    fixed (double* ptrX = &x[startIndexX], ptrY = &y[startIndexY])
                    {
                        return _ddot(ref n, (IntPtr)ptrX, ref incX, (IntPtr)ptrY, ref incY);
                    }
                }
            }

            /// <summary>Computes the Euclidean norm of a vector, i.e. ||x||.
            /// </summary>
            /// <param name="n">The number of elements of <paramref name="x"/>.</param>
            /// <param name="x">The vector 'x' with at least <paramref name="n"/> elements.</param>
            /// <param name="incX">The increment for <paramref name="x"/>.</param>
            /// <returns>The euclidian norm of <paramref name="x"/>, i.e. \sqrt(x_0^2 + ... + x_n^2).
            /// </returns>
            public double dnrm2(int n, double[] x, int incX = 1)
            {
                unsafe
                {
                    fixed (double* ptrX = x)
                    {
                        return _dnrm2(ref n, (IntPtr)ptrX, ref incX);
                    }
                }
            }

            /// <summary>Performs rotation of points in the plane, i.e. x(i) = c * x(i) + s * y(i) and y(i) = c * y(i) - s * x(i).
            /// </summary>
            /// <param name="n">The number of elements of <paramref name="x"/> and <paramref name="y"/>.</param>
            /// <param name="x">The vector 'x' with at least <paramref name="n"/> elements; 'x(i) = c * x(i) + s * y(i)' after function evaluation.</param>
            /// <param name="y">The vector 'y' with at least <paramref name="n"/> elements; 'y(i) = c * y(i) - s * x(i)' after function evaluation.</param>
            /// <param name="c">The scalar 'c'.</param>
            /// <param name="s">The scalar 's'.</param>
            /// <param name="incX">The increment for <paramref name="x"/>.</param>
            /// <param name="incY">The increment for <paramref name="y"/>.</param>
            public void drot(int n, double[] x, double[] y, double c, double s, int incX = 1, int incY = 1)
            {
                unsafe
                {
                    fixed (double* ptrX = x, ptrY = y)
                    {
                        _drot(ref n, (IntPtr)ptrX, ref incX, (IntPtr)ptrY, ref incY, ref c, ref s);
                    }
                }
            }

            /// <summary>Computes the parameters for a Givens rotation; given the Cartesian coordinates (a, b) of a point p, these routines
            /// return the parameters a, b, c, and s associated with the Givens rotation that zeros the y-coordinate of the point.
            /// </summary>
            /// <param name="a">Provides the x-coordinate of the point p; contains the parameter r associated with the Givens rotation after function evaluation.</param>
            /// <param name="b">Provides the y-coordinate of the point p; contains the parameter z associated with the Givens rotation after function evaluation.</param>
            /// <param name="c">Contains the parameter c associated with the Givens rotation (output).</param>
            /// <param name="s">Contains the parameter s associated with the Givens rotation (output).</param>
            public void drotg(ref double a, ref double b, out double c, out double s)
            {
                unsafe
                {
                    _drotg(ref a, ref b, out c, out s);
                }
            }

            /// <summary>Performs rotation of points in the modified plane; x(i) = H*x(i) + H*y(i), y(i) = H*y(i) - H*x(i), where H is a modified Givens transformation matrix
            /// whose values are stored in the <paramref name="param"/>.
            /// </summary>
            /// <param name="n">The number of elements of <paramref name="x"/> and <paramref name="y"/>.</param>
            /// <param name="x">The vector 'x' with at least <paramref name="n"/> elements.</param>
            /// <param name="y">The vector 'y' with at least <paramref name="n"/> elements.</param>
            /// <param name="param">The elements of the param array are: param(0) contains a switch, flag. param(1-4) contain h11, h21, h12, and h22, respectively,
            /// the components of the array H.</param>
            /// <param name="incX">The increment for <paramref name="x"/>.</param>
            /// <param name="incY">The increment for <paramref name="y"/>.</param>
            public void drotm(int n, double[] x, double[] y, double[] param, int incX = 1, int incY = 1)
            {
                unsafe
                {
                    fixed (double* ptrX = x, ptrY = y, ptrParam = param)
                    {
                        _drotm(ref n, (IntPtr)ptrX, ref incX, (IntPtr)ptrY, ref incY, (IntPtr)ptrParam);
                    }
                }
            }

            /// <summary>Construct the modified Givens transformation matrix H which zeros the second component of a 2-dimensional vector, i.e.
            /// given (x_1,y_1) compute matrix H such that <para>(x_1,0)^t = H * (x_1*\sqrt(d1), y_1 * \qrt(d2))^t.</para>
            /// </summary>
            /// <param name="d1">Provides the scaling factor for the x-coordinate of the input vector; the first diagonal element of the updated matrix on exit.</param>
            /// <param name="d2">Provides the scaling factor for the y-coordinate of the input vector; the second diagonal element of the updated matrix on exit.</param>
            /// <param name="x1">Provides the x-coordinate of the input vector; the x-coordinate of the rotated vector before scaling on exit.</param>
            /// <param name="y1">Provides the y-coordinate of the input vector.</param>
            /// <param name="param">The elements of the param array are: param(0) contains a switch, flag. param(1-4) contain h11, h21, h12, and h22, respectively,
            /// the components of the array H.</param>
            public void drotmg(ref double d1, ref double d2, ref double x1, double y1, double[] param)
            {
                unsafe
                {
                    fixed (double* ptrParam = param)
                    {
                        _drotmg(ref d1, ref d2, ref x1, ref y1, (IntPtr)ptrParam);
                    }
                }
            }

            /// <summary>Computes the product of a vector by a scalar, i.e. x = a * x.
            /// </summary>
            /// <param name="n">The number of elements of <paramref name="x"/>.</param>
            /// <param name="a">The scalar factor 'a'.</param>
            /// <param name="x">The vector 'x' with at least <paramref name="n"/> elements, contains the updated vector after function call.</param>
            public void dscal(int n, double a, double[] x)
            {
                unsafe
                {
                    fixed (double* ptrX = x)
                    {
                        int inc = 1;
                        _dscal(ref n, ref a, (IntPtr)ptrX, ref inc);
                    }
                }
            }

            /// <summary>Computes the product of a vector by a scalar, i.e. x = a * x.
            /// </summary>
            /// <param name="a">The scalar factor 'a'.</param>
            /// <param name="x">The vector 'x', contains the updated vector after function call.</param>
            public void dscal(double a, Span<double> x)
            {
                unsafe
                {
                    fixed (double* ptrX = &MemoryMarshal.GetReference(x))
                    {
                        _dscal(ref Unsafe.AsRef<int>(x.Length), ref Unsafe.AsRef<double>(a), (IntPtr)ptrX, ref Unsafe.AsRef<int>(1));
                    }
                }
            }

            /// <summary>Computes the product of a vector by a scalar, i.e. x[iStart + j * increment] = a * x[iStart + j * increment], for j = 0,..., n-1.
            /// </summary>
            /// <param name="n">The number of elements of <paramref name="x"/>.</param>
            /// <param name="a">The scalar factor 'a'.</param>
            /// <param name="x">The vector 'x' with at least <paramref name="startIndex"/> + 1 + (<paramref name="n"/> -1) * <paramref name="increment"/> elements.</param>
            /// <param name="startIndex">The null-based start index.</param>
            /// <param name="increment">The increment.</param>
            public void dscal(int n, double a, double[] x, int increment, int startIndex)
            {
                unsafe
                {
                    fixed (double* ptrX = &x[startIndex])
                    {
                        _dscal(ref n, ref a, (IntPtr)ptrX, ref increment);
                    }
                }
            }

            /// <summary>Swaps a vector with another vector.
            /// </summary>
            /// <param name="n">The number of elements of <paramref name="x"/> and <paramref name="y"/>.</param>
            /// <param name="x">The vector 'x' with at least <paramref name="n"/> elements.</param>
            /// <param name="y">The vector 'y' with at least <paramref name="n"/> elements.</param>
            public void dswap(int n, double[] x, double[] y)
            {
                unsafe
                {
                    fixed (double* ptrX = x, ptrY = y)
                    {
                        int inc = 1;
                        _dswap(ref n, (IntPtr)ptrX, ref inc, (IntPtr)ptrY, ref inc);
                    }
                }
            }

            /// <summary>Swaps a vector with another vector.
            /// </summary>
            /// <param name="n">The number of elements of <paramref name="x"/> and <paramref name="y"/>.</param>
            /// <param name="x">The vector 'x' with at least <paramref name="n"/> elements.</param>
            /// <param name="incX">The increment for <paramref name="x"/>.</param>
            /// <param name="y">The vector 'y' with at least <paramref name="n"/> elements.</param>
            /// <param name="incY">The increment for <paramref name="y"/>.</param>
            /// <param name="startIndexX">The null-based start index for <paramref name="x"/>.</param>
            /// <param name="startIndexY">The null-based start index for <paramref name="y"/>.</param>
            public void dswap(int n, double[] x, int incX, double[] y, int incY, int startIndexX, int startIndexY)
            {
                unsafe
                {
                    fixed (double* ptrX = &x[startIndexX], ptrY = &y[startIndexY])
                    {
                        _dswap(ref n, (IntPtr)ptrX, ref incX, (IntPtr)ptrY, ref incY);
                    }
                }
            }

            /// <summary>Finds the index of the element with maximum absolute value.
            /// </summary>
            /// <param name="n">The number of elements in vector <paramref name="x"/>.</param>
            /// <param name="x">The vector with at least <paramref name="n"/> elements.</param>
            /// <returns>The position of vector element <paramref name="x"/> that has the largest absolute value.
            /// </returns>
            public int idamax(int n, double[] x)
            {
                unsafe
                {
                    fixed (double* ptrX = x)
                    {
                        int inc = 1;
                        return _idamax(ref n, (IntPtr)ptrX, ref inc) - 1;  // is one-based
                    }
                }
            }

            /// <summary>Finds the index of the element with maximum absolute value.
            /// </summary>
            /// <param name="n">The number of elements in vector <paramref name="x"/>.</param>
            /// <param name="x">The vector with at least <paramref name="n"/> elements.</param>
            /// <param name="incX">The increment for <paramref name="x"/>.</param>
            /// <returns>The position of vector element <paramref name="x"/> that has the largest absolute value.
            /// </returns>
            public int idamax(int n, double[] x, int incX)
            {
                unsafe
                {
                    fixed (double* ptrX = x)
                    {
                        return _idamax(ref n, (IntPtr)ptrX, ref incX) - 1;  // is one-based
                    }
                }
            }

            /// <summary>Finds the index of the element with smallest absolute value.
            /// </summary>
            /// <param name="n">The number of elements in vector <paramref name="x"/>.</param>
            /// <param name="x">The vector with at least <paramref name="n"/> elements.</param>
            /// <returns>The position of vector element <paramref name="x"/> that has the smallest absolute value.
            /// </returns>
            /// <remarks>This method is not part of the BLAS standard.</remarks>
            public virtual int idamin(int n, double[] x)
            {
                int index = 0;
                double value = Math.Abs(x[0]);
                for (int j = 1; j < n; j++)
                {
                    if (Math.Abs(x[j]) < value)
                    {
                        value = Math.Abs(x[j]);
                        index = j;
                    }
                }
                return index;
            }

            /// <summary>Finds the index of the element with smallest absolute value.
            /// </summary>
            /// <param name="n">The number of elements in vector <paramref name="x"/>.</param>
            /// <param name="x">The vector with at least <paramref name="n"/> elements.</param>
            /// <param name="incX">The increment for <paramref name="x"/>.</param>
            /// <returns>
            /// The position of vector element <paramref name="x"/> that has the smallest absolute value.
            /// </returns>
            /// <remarks>This method is not part of the BLAS standard.</remarks>
            public virtual int idamin(int n, double[] x, int incX)
            {
                int index = 0;
                double value = Math.Abs(x[0]);
                for (int j = 1; j < n; j++)
                {
                    if (Math.Abs(x[j * incX]) < value)
                    {
                        value = Math.Abs(x[j * incX]);
                        index = j * incX;
                    }
                }
                return index;
            }
            #endregion

            #region complex methods

            /// <summary>Compute the sum of the magnitudes of elements of some vector, i.e.
            /// <para>|Re x(0)|+ |Im x(0)| + |Re x(incX)| + |Im x(incX)| + |Re x(2 * incX)| + |Im x(2 * incX)| + ...  + |Re x(k * incX)| + |x(k * incX)|.</para>
            /// </summary>
            /// <param name="n">The number of elements in <paramref name="x"/> with at least
            /// <c>(1 + (<paramref name="n"/>-1)*abs(<paramref name="incX"/>))</c> elements.</param>
            /// <param name="x">The vector.</param>
            /// <param name="incX">The increment for indexing vector <paramref name="x"/>.</param>
            /// <returns>The sum of magnitudes of elements of the vector <paramref name="x"/>.
            /// </returns>
            public double zasum(int n, Complex[] x, int incX)
            {
                unsafe
                {
                    fixed (Complex* ptrX = x)
                    {
                        return _zasum(ref n, (IntPtr)ptrX, ref incX);
                    }
                }
            }

            /// <summary>Perform a vector-vector operation defined as y := a*x + y, i.e. scalar constant times vector plus a vector.
            /// </summary>
            /// <param name="n">The number of elements of <paramref name="x"/> and <paramref name="y"/>.</param>
            /// <param name="a">The scalar factor 'a'.</param>
            /// <param name="x">The vector 'x' with at least <paramref name="n"/> elements.</param>
            /// <param name="y">The vector 'y' with at least <paramref name="n"/> elements.</param>
            public void zaxpy(int n, Complex a, Complex[] x, Complex[] y)
            {
                unsafe
                {
                    fixed (Complex* ptrX = x, ptrY = y)
                    {
                        int inc = 1;
                        _zaxpy(ref n, ref a, (IntPtr)ptrX, ref inc, (IntPtr)ptrY, ref inc);
                    }
                }
            }

            /// <summary>Perform a vector-vector operation defined as
            /// <para>
            /// y[<paramref name="startIndexY"/> + k * <paramref name="incY"/>] += <paramref name="a"/> * x[<paramref name="startIndexX"/> + k * <paramref name="incX"/>]
            /// </para>
            /// for k=0,..,<paramref name="n"/>-1. Thus some partial scalar times vector plus vector operation.
            /// </summary>
            /// <param name="n">The number of elements to add.</param>
            /// <param name="a">The scalar factor 'a'.</param>
            /// <param name="x">The vector 'x' with at least 1 + <paramref name="startIndexX"/> + ( <paramref name="n"/> - 1) * <paramref name="incX"/> elements.</param>
            /// <param name="y">The vector 'y' with at least 1 + <paramref name="startIndexY"/> + ( <paramref name="n"/> - 1) * <paramref name="incY"/> elements (output).</param>
            /// <param name="incX">The increment for <paramref name="x"/>.</param>
            /// <param name="incY">The increment for <paramref name="y"/>.</param>
            /// <param name="startIndexX">The null-based start index for <paramref name="x"/>.</param>
            /// <param name="startIndexY">The null-based start index for <paramref name="y"/>.</param>
            public void zaxpy(int n, Complex a, Complex[] x, Complex[] y, int incX, int incY, int startIndexX, int startIndexY)
            {
                unsafe
                {
                    fixed (Complex* ptrX = &x[startIndexX], ptrY = &y[startIndexY])
                    {
                        _zaxpy(ref n, ref a, (IntPtr)ptrX, ref incX, (IntPtr)ptrY, ref incY);
                    }
                }
            }

            /// <summary>Copies a vector to another vector, i.e. y = x.
            /// </summary>
            /// <param name="n">The number of elements of <paramref name="x"/> and <paramref name="y"/>.</param>
            /// <param name="x">The vector 'x' with at least <paramref name="n"/> elements.</param>
            /// <param name="y">The vector 'y' with at least <paramref name="n"/> elements;
            /// contains a copy of the vector <paramref name="x"/> after function evaluation, if <paramref name="n"/> is positive.</param>
            public void zcopy(int n, Complex[] x, Complex[] y)
            {
                unsafe
                {
                    fixed (Complex* ptrX = x, ptrY = y)
                    {
                        int inc = 1;
                        _zcopy(ref n, (IntPtr)ptrX, ref inc, (IntPtr)ptrY, ref inc);
                    }
                }
            }

            /// <summary>Copies a vector to another vector, i.e.
            /// <para>
            /// y[<paramref name="startIndexY"/> + k * <paramref name="incY"/>] = x[<paramref name="startIndexX"/> + k * <paramref name="incX"/>]
            /// </para> for k=0,..,<paramref name="n"/>-1.
            /// </summary>
            /// <param name="n">The number of elements to copy.</param>
            /// <param name="x">The vector 'x' with at least 1 + <paramref name="startIndexX"/> + ( <paramref name="n"/> - 1) * <paramref name="incX"/> elements.</param>
            /// <param name="y">The vector 'y' with at least 1 + <paramref name="startIndexY"/> + ( <paramref name="n"/> - 1) * <paramref name="incY"/> elements; contains the updated vector on exit.</param>
            /// <param name="incX">The increment for <paramref name="x"/>.</param>
            /// <param name="incY">The increment for <paramref name="y"/>.</param>
            /// <param name="startIndexX">The null-based start index for <paramref name="x"/>.</param>
            /// <param name="startIndexY">The null-based start index for <paramref name="y"/>.</param>
            public void zcopy(int n, Complex[] x, Complex[] y, int incX, int incY, int startIndexX, int startIndexY)
            {
                unsafe
                {
                    fixed (Complex* ptrX = &x[startIndexX], ptrY = &y[startIndexY])
                    {
                        _zcopy(ref n, (IntPtr)ptrX, ref incX, (IntPtr)ptrY, ref incY);
                    }
                }
            }

            /// <summary>Computes a dot product of a conjugated vector with another vector, i.e. \sum conjugate(x) * y.
            /// </summary>
            /// <param name="n">The number of elements of <paramref name="x"/> and <paramref name="y"/>.</param>
            /// <param name="x">The vector 'x' with at least <paramref name="n"/> elements.</param>
            /// <param name="y">The vector 'y' with at least <paramref name="n"/> elements.</param>
            /// <param name="incX">The increment for <paramref name="x"/>.</param>
            /// <param name="incY">The increment for <paramref name="y"/>.</param>
            /// <returns>The dot product of <paramref name="x"/> and <paramref name="y"/>, i.e. \sum conjugate(x) * y.</returns>
            public Complex zdotc(int n, Complex[] x, Complex[] y, int incX = 1, int incY = 1)
            {
                unsafe
                {
                    fixed (Complex* ptrX = x, ptrY = y)
                    {
                        return _zdotc(ref n, (IntPtr)ptrX, ref incX, (IntPtr)ptrY, ref incY);
                    }
                }
            }

            /// <summary>Computes a vector-vector product, i.e. \sum x * y.
            /// </summary>
            /// <param name="n">The number of elements of <paramref name="x"/> and <paramref name="y"/>.</param>
            /// <param name="x">The vector 'x' with at least <paramref name="n"/> elements.</param>
            /// <param name="y">The vector 'y' with at least <paramref name="n"/> elements.</param>
            /// <param name="incX">The increment for <paramref name="x"/>.</param>
            /// <param name="incY">The increment for <paramref name="y"/>.</param>
            /// <returns>The vector-vector product of <paramref name="x"/> and <paramref name="y"/>, i.e. \sum x * y.</returns>
            public Complex zdotu(int n, Complex[] x, Complex[] y, int incX = 1, int incY = 1)
            {
                unsafe
                {
                    fixed (Complex* ptrX = x, ptrY = y)
                    {
                        return _zdotu(ref n, (IntPtr)ptrX, ref incX, (IntPtr)ptrY, ref incY);
                    }
                }
            }

            /// <summary>Computes the Euclidean norm of a vector, i.e. ||x||.
            /// </summary>
            /// <param name="n">The number of elements of <paramref name="x"/>.</param>
            /// <param name="x">The vector 'x' with at least <paramref name="n"/> elements.</param>
            /// <param name="incX">The increment for <paramref name="x"/>.</param>
            /// <returns>The euclidian norm of <paramref name="x"/>, i.e. \sqrt(x_0^2 + ... + x_n^2).
            /// </returns>
            public double znrm2(int n, Complex[] x, int incX = 1)
            {
                unsafe
                {
                    fixed (Complex* ptrX = x)
                    {
                        return _znrm2(ref n, (IntPtr)ptrX, ref incX);
                    }
                }
            }

            /// <summary>Performs rotation of points in the plane, i.e. x(i) = c * x(i) + s * y(i) and y(i) = c * y(i) - s * x(i).
            /// </summary>
            /// <param name="n">The number of elements of <paramref name="x"/> and <paramref name="y"/>.</param>
            /// <param name="x">The vector 'x' with at least <paramref name="n"/> elements; 'x(i) = c * x(i) + s * y(i)' after function evaluation.</param>
            /// <param name="y">The vector 'y' with at least <paramref name="n"/> elements; 'y(i) = c * y(i) - s * x(i)' after function evaluation.</param>
            /// <param name="c">The scalar 'c'.</param>
            /// <param name="s">The scalar 's'.</param>
            /// <param name="incX">The increment for <paramref name="x"/>.</param>
            /// <param name="incY">The increment for <paramref name="y"/>.</param>
            public void zdrot(int n, Complex[] x, Complex[] y, double c, double s, int incX = 1, int incY = 1)
            {
                unsafe
                {
                    fixed (Complex* ptrX = x, ptrY = y)
                    {
                        _zdrot(ref n, (IntPtr)ptrX, ref incX, (IntPtr)ptrY, ref incY, ref c, ref s);
                    }
                }
            }

            /// <summary>Computes the parameters for a Givens rotation; given the Cartesian coordinates (a, b) of a point p, these routines
            /// return the parameters a, b, c, and s associated with the Givens rotation that zeros the y-coordinate of the point.
            /// </summary>
            /// <param name="a">Provides the x-coordinate of the point p; contains the parameter r associated with the Givens rotation after function evaluation.</param>
            /// <param name="b">Provides the y-coordinate of the point p; contains the parameter z associated with the Givens rotation after function evaluation.</param>
            /// <param name="c">Contains the parameter c associated with the Givens rotation (output).</param>
            /// <param name="s">Contains the parameter s associated with the Givens rotation (output).</param>
            public void zrotg(ref Complex a, ref Complex b, out double c, out Complex s)
            {
                unsafe
                {
                    _zrotg(ref a, ref b, out c, out s);
                }
            }

            /// <summary>Computes the product of a vector by a scalar, i.e. x = a * x.
            /// </summary>
            /// <param name="n">The number of elements of <paramref name="x"/>.</param>
            /// <param name="a">The scalar factor 'a'.</param>
            /// <param name="x">The vector 'x' with at least <paramref name="n"/> elements, contains the updated vector after function call.</param>
            /// <param name="incX">The increment for <paramref name="x"/>.</param>
            public void zscal(int n, Complex a, Complex[] x, int incX = 1)
            {
                unsafe
                {
                    fixed (Complex* ptrX = x)
                    {
                        _zscal(ref n, ref a, (IntPtr)ptrX, ref incX);
                    }
                }
            }

            /// <summary>Computes the product of a vector by a scalar, i.e. x = a * x.
            /// </summary>
            /// <param name="n">The number of elements of <paramref name="x"/>.</param>
            /// <param name="a">The scalar factor 'a'.</param>
            /// <param name="x">The vector 'x' with at least <paramref name="n"/> elements, contains the updated vector after function call.</param>
            /// <param name="incX">The increment for <paramref name="x"/>.</param>
            public void zdscal(int n, double a, Complex[] x, int incX = 1)
            {
                unsafe
                {
                    fixed (Complex* ptrX = x)
                    {
                        _zdscal(ref n, ref a, (IntPtr)ptrX, ref incX);
                    }
                }
            }

            /// <summary>Computes the product of a vector by a scalar, i.e. x[iStart + j * increment] = a * x[iStart + j * increment], for j = 0,..., n-1.
            /// </summary>
            /// <param name="n">The number of elements of <paramref name="x"/>.</param>
            /// <param name="a">The scalar factor 'a'.</param>
            /// <param name="x">The vector 'x' with at least <paramref name="startIndex"/> + 1 + (<paramref name="n"/> -1) * <paramref name="increment"/> elements.</param>
            /// <param name="startIndex">The null-based start index.</param>
            /// <param name="increment">The increment.</param>
            public void zscal(int n, Complex a, Complex[] x, int increment, int startIndex)
            {
                unsafe
                {
                    fixed (Complex* ptrX = &x[startIndex])
                    {
                        _zscal(ref n, ref a, (IntPtr)ptrX, ref increment);
                    }
                }
            }

            /// <summary>Computes the product of a vector by a scalar, i.e. x[iStart + j * increment] = a * x[iStart + j * increment],
            /// for j = 0,..., n-1.
            /// </summary>
            /// <param name="n">The number of elements of <paramref name="x"/>.</param>
            /// <param name="a">The scalar factor 'a'.</param>
            /// <param name="x">The vector 'x' with at least <paramref name="startIndex"/> + 1 + (<paramref name="n"/> -1) * <paramref name="increment"/> elements.</param>
            /// <param name="startIndex">The null-based start index.</param>
            /// <param name="increment">The increment.</param>
            public void zdscal(int n, double a, Complex[] x, int increment, int startIndex)
            {
                unsafe
                {
                    fixed (Complex* xPtr = &x[startIndex])
                    {
                        _zdscal(ref n, ref a, (IntPtr)xPtr, ref increment);
                    }
                }
            }

            /// <summary>Swaps a vector with another vector.
            /// </summary>
            /// <param name="n">The number of elements of <paramref name="x"/> and <paramref name="y"/>.</param>
            /// <param name="x">The vector 'x' with at least <paramref name="n"/> elements.</param>
            /// <param name="y">The vector 'y' with at least <paramref name="n"/> elements.</param>
            public void zswap(int n, Complex[] x, Complex[] y)
            {
                unsafe
                {
                    fixed (Complex* ptrX = x, ptrY = y)
                    {
                        int inc = 1;
                        _zswap(ref n, (IntPtr)ptrX, ref inc, (IntPtr)ptrY, ref inc);
                    }
                }
            }

            /// <summary>Swaps a vector with another vector.
            /// </summary>
            /// <param name="n">The number of elements of <paramref name="x"/> and <paramref name="y"/>.</param>
            /// <param name="x">The vector 'x' with at least <paramref name="n"/> elements.</param>
            /// <param name="incX">The increment for <paramref name="x"/>.</param>
            /// <param name="y">The vector 'y' with at least <paramref name="n"/> elements.</param>
            /// <param name="incY">The increment for <paramref name="y"/>.</param>
            /// <param name="startIndexX">The null-based start index for <paramref name="x"/>.</param>
            /// <param name="startIndexY">The null-based start index for <paramref name="y"/>.</param>
            public void zswap(int n, Complex[] x, int incX, Complex[] y, int incY, int startIndexX, int startIndexY)
            {
                unsafe
                {
                    fixed (Complex* ptrX = &x[startIndexX], ptrY = &y[startIndexY])
                    {
                        _zswap(ref n, (IntPtr)ptrX, ref incX, (IntPtr)ptrY, ref incY);
                    }
                }
            }

            /// <summary>Finds the index of the element with maximum absolute value, i.e. |Re(x(i))| + |Im(x(i))|.
            /// </summary>
            /// <param name="n">The number of elements in vector <paramref name="x"/>.</param>
            /// <param name="x">The vector with at least <paramref name="n"/> elements.</param>
            /// <returns>The position of vector element <paramref name="x"/> that has the largest absolute value.
            /// </returns>
            public int izamax(int n, Complex[] x)
            {
                unsafe
                {
                    fixed (Complex* ptrX = x)
                    {
                        int inc = 1;
                        return _izamax(ref n, (IntPtr)ptrX, ref inc) - 1;  // is one-based
                    }
                }
            }

            /// <summary>Finds the index of the element with maximum absolute value, i.e. |Re(x(i))| + |Im(x(i))|.
            /// </summary>
            /// <param name="n">The number of elements in vector <paramref name="x"/>.</param>
            /// <param name="x">The vector with at least <paramref name="n"/> elements.</param>
            /// <param name="incX">The increment for <paramref name="x"/>.</param>
            /// <returns>The position of vector element <paramref name="x"/> that has the largest absolute value.
            /// </returns>
            public int izamax(int n, Complex[] x, int incX)
            {
                unsafe
                {
                    fixed (Complex* ptrX = x)
                    {
                        return _izamax(ref n, (IntPtr)ptrX, ref incX) - 1;  // is one-based
                    }
                }
            }

            /// <summary>Finds the index of the element with smallest absolute value, i.e. |Re(x(i))| + |Im(x(i))|.
            /// </summary>
            /// <param name="n">The number of elements in vector <paramref name="x"/>.</param>
            /// <param name="x">The vector with at least <paramref name="n"/> elements.</param>
            /// <returns>The position of vector element <paramref name="x"/> that has the smallest absolute value.
            /// </returns>
            /// <remarks>This method is not part of the BLAS standard.</remarks>
            public virtual int izamin(int n, Complex[] x)
            {
                int index = 0;
                double value = Complex.Abs(x[0]);

                for (int j = 1; j < n; j++)
                {
                    if (Complex.Abs(x[j]) < value)
                    {
                        value = Complex.Abs(x[j]);
                        index = j;
                    }
                }
                return index;
            }

            /// <summary>Finds the index of the element with smallest absolute value, i.e. |Re(x(i))| + |Im(x(i))|.
            /// </summary>
            /// <param name="n">The number of elements in vector <paramref name="x"/>.</param>
            /// <param name="x">The vector with at least <paramref name="n"/> elements.</param>
            /// <param name="incX">The increment for <paramref name="x"/>.</param>
            /// <returns>The position of vector element <paramref name="x"/> that has the smallest absolute value.
            /// </returns>
            /// <remarks>This method is not part of the BLAS standard.</remarks>
            public virtual int izamin(int n, Complex[] x, int incX)
            {
                int index = 0;
                double value = Complex.Abs(x[0]);

                for (int j = 1; j < n; j++)
                {
                    if (Complex.Abs(x[j * incX]) < value)
                    {
                        value = Complex.Abs(x[j * incX]);
                        index = j * incX;
                    }
                }
                return index;
            }
            #endregion

            #endregion

            #endregion
        }
    }
}