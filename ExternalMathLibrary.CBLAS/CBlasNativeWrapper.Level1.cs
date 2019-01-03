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
using System.Numerics;
using System.Security;
using System.Runtime.InteropServices;

namespace Dodoni.MathLibrary.Basics.LowLevel.Native
{
    public partial class CBlasNativeWrapper
    {
        /// <summary>The wrapper for C-BLAS level 1 methods, i.e. the C-interface for BLAS. See http://www.netlib.org/blas for further information.
        /// </summary>
        protected internal class Level1CBLAS : ILevel1BLAS
        {
            #region private function import

            #region double precision methods

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_dasum", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern double _dasum(int n, double[] x, int incX);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_dasum", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private unsafe static extern double _dasum(int n, double* x, int incX);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_daxpy", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _daxpy(int n, double alpha, double[] x, int incX, double[] y, int incY);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_daxpy", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private unsafe static extern void _daxpy(int n, double alpha, double* x, int incX, double* y, int incY);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_dcopy", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dcopy(int n, double[] x, int incX, double[] y, int incY);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_dcopy", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private unsafe static extern void _dcopy(int n, double* x, int incX, double* y, int incY);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_ddot", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern double _ddot(int n, double[] x, int incX, double[] y, int incY);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_ddot", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private unsafe static extern double _ddot(int n, double* x, int incX, double* y, int incY);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_dnrm2", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern double _dnrm2(int n, double[] x, int incX);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_dnrm2", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private unsafe static extern double _dnrm2(int n, double* x, int incX);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_drot", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern double _drot(int n, double[] x, int incX, double[] y, int incY, double c, double s);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_drotg", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern double _drotg(ref double a, ref double b, out double c, out double s);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_drotm", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern double _drotm(int n, double[] x, int incX, double[] y, int incY, double[] param);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_drotmg", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern double _drotmg(ref double d1, ref double d2, ref double x1, double y1, double[] param);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_dscal", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dscal(int n, double alpha, double[] x, int incX);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_dscal", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private unsafe static extern void _dscal(int n, double alpha, double* x, int incX);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_dswap", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _dswap(int n, double[] x, int incX, double[] y, int incY);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_dswap", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private unsafe static extern void _dswap(int n, double* x, int incX, double* y, int incY);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_idamax", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern int _idamax(int n, double[] x, int incX);
            #endregion

            #region complex methods

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_dzasum", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern double _zasum(int n, [In, Out] Complex[] x, int incX);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_dzasum", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private unsafe static extern double _zasum(int n, Complex* x, int incX);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_zaxpy", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zaxpy(int n, ref Complex alpha, [In, Out] Complex[] x, int incX, [In, Out] Complex[] y, int incY);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_zaxpy", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private unsafe static extern void _zaxpy(int n, ref Complex alpha, Complex* x, int incX, Complex* y, int incY);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_zcopy", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zcopy(int n, [In, Out] Complex[] x, int incX, [In, Out] Complex[] y, int incY);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_zcopy", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private unsafe static extern void _zcopy(int n, Complex* x, int incX, Complex* y, int incY);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_zdotc_sub", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zdotc(int n, [In, Out] Complex[] x, int incX, [In, Out] Complex[] y, int incY, out Complex result);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_zdotc_sub", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private unsafe static extern void _zdotc(int n, Complex* x, int incX, Complex* y, int incY, out Complex result);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_zdotu_sub", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zdotu(int n, [In, Out] Complex[] x, int incX, [In, Out] Complex[] y, int incY, out Complex result);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_zdotu_sub", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private unsafe static extern void _zdotu(int n, Complex* x, int incX, Complex* y, int incY, out Complex result);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_dznrm2", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern double _znrm2(int n, [In, Out] Complex[] x, int incX);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_dznrm2", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private unsafe static extern double _znrm2(int n, Complex* x, int incX);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_zdrot", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern double _zdrot(int n, [In, Out] Complex[] x, int incX, [In, Out] Complex[] y, int incY, double c, double s);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_zdrot", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private unsafe static extern double _zdrot(int n, Complex* x, int incX, Complex* y, int incY, double c, double s);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_zrotg", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern double _zrotg(ref Complex a, ref Complex b, out double c, out Complex s);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_zscal", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zscal(int n, ref Complex alpha, [In, Out] Complex[] x, int incX);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_zscal", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private unsafe static extern void _zscal(int n, ref Complex alpha, Complex* x, int incX);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_zdscal", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zdscal(int n, double alpha, [In, Out] Complex[] x, int incX);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_zdscal", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private unsafe static extern void _zdscal(int n, double alpha, Complex* x, int incX);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_zswap", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern void _zswap(int n, [In, Out] Complex[] x, int incX, [In, Out] Complex[] y, int incY);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_zswap", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private unsafe static extern void _zswap(int n, Complex* x, int incX, Complex* y, int incY);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_izamax", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern int _izamax(int n, [In, Out] Complex[] x, int incX);

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_izamax", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private unsafe static extern int _izamax(int n, Complex* x, int incX);
            #endregion

            #endregion

            #region public constructors

            /// <summary>Initializes a new instance of the <see cref="Level1CBLAS"/> class.
            /// </summary>
            public Level1CBLAS()
            {
            }
            #endregion

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
                return _dasum(n, x, incX);
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
                    fixed (double* xPtr = &x[startIndexX])
                    {
                        return _dasum(n, xPtr, incX);
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
                _daxpy(n, a, x, 1, y, 1);
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
                    fixed (double* xPtr = &x[startIndexX], yPtr = &y[startIndexY])
                    {
                        _daxpy(n, a, xPtr, incX, yPtr, incY);
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
                _dcopy(n, x, 1, y, 1);
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
                    fixed (double* xPtr = &x[startIndexX], yPtr = &y[startIndexY])
                    {
                        _dcopy(n, xPtr, incX, yPtr, incY);
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
                return _ddot(n, x, 1, y, 1);
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
                    fixed (double* xPtr = &x[startIndexX], yPtr = &y[startIndexY])
                    {
                        return _ddot(n, xPtr, incX, yPtr, incY);
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
                return _dnrm2(n, x, incX);
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
                _drot(n, x, incX, y, incY, c, s);
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
                _drotg(ref a, ref b, out c, out s);
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
                _drotm(n, x, incX, y, incY, param);
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
                _drotmg(ref d1, ref d2, ref x1, y1, param);
            }

            /// <summary>Computes the product of a vector by a scalar, i.e. x = a * x.
            /// </summary>
            /// <param name="n">The number of elements of <paramref name="x"/>.</param>
            /// <param name="a">The scalar factor 'a'.</param>
            /// <param name="x">The vector 'x' with at least <paramref name="n"/> elements, contains the updated vector after function call.</param>
            public void dscal(int n, double a, double[] x)
            {
                _dscal(n, a, x, 1);
            }


            public void dscal(double a, Span<double> x)
            {
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
                    fixed (double* xPtr = &x[startIndex])
                    {
                        _dscal(n, a, xPtr, increment);
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
                _dswap(n, x, 1, y, 1);
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
                    fixed (double* xPtr = &x[startIndexX], yPtr = &y[startIndexY])
                    {
                        _dswap(n, xPtr, incX, yPtr, incY);
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
                return _idamax(n, x, 1);
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
                return _idamax(n, x, incX);
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
                double compareValue = Math.Abs(x[0]);

                for (int j = 1; j < n; j++)
                {
                    if (Math.Abs(x[j]) < compareValue)
                    {
                        compareValue = Math.Abs(x[j]);
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
            /// <returns>The position of vector element <paramref name="x"/> that has the smallest absolute value.
            /// </returns>
            /// <remarks>This method is not part of the BLAS standard.</remarks>
            public virtual int idamin(int n, double[] x, int incX)
            {
                int index = 0;
                double compareValue = Math.Abs(x[0]);

                for (int j = 1; j < n; j++)
                {
                    if (Math.Abs(x[j * incX]) < compareValue)
                    {
                        compareValue = Math.Abs(x[j * incX]);
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
                return _zasum(n, x, incX);
            }

            /// <summary>Perform a vector-vector operation defined as y := a*x + y, i.e. scalar constant times vector plus a vector.
            /// </summary>
            /// <param name="n">The number of elements of <paramref name="x"/> and <paramref name="y"/>.</param>
            /// <param name="a">The scalar factor 'a'.</param>
            /// <param name="x">The vector 'x' with at least <paramref name="n"/> elements.</param>
            /// <param name="y">The vector 'y' with at least <paramref name="n"/> elements.</param>
            public void zaxpy(int n, Complex a, Complex[] x, Complex[] y)
            {
                _zaxpy(n, ref a, x, 1, y, 1);
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
                    fixed (Complex* xPtr = &x[startIndexX], yPtr = &y[startIndexY])
                    {
                        _zaxpy(n, ref a, xPtr, incX, yPtr, incY);
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
                _zcopy(n, x, 1, y, 1);
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
                    fixed (Complex* xPtr = &x[startIndexX], yPtr = &y[startIndexY])
                    {
                        _zcopy(n, xPtr, incX, yPtr, incY);
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
                Complex result;
                _zdotc(n, x, incX, y, incY, out result);
                return result;
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
                Complex result;
                _zdotu(n, x, incX, y, incY, out result);
                return result;
            }

            /// <summary>Computes the Euclidean norm of a vector, i.e. ||x||.
            /// </summary>
            /// <param name="n">The number of elements of <paramref name="x"/>.</param>
            /// <param name="x">The vector 'x' with at least <paramref name="n"/> elements.</param>
            /// <param name="incX">The increment for <paramref name="x"/>.</param>
            /// <returns>The euclidian norm of <paramref name="x"/>, i.e. \sqrt(x_0^2 + ... + x_n^2).</returns>
            public double znrm2(int n, Complex[] x, int incX = 1)
            {
                return _znrm2(n, x, incX);
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
                _zdrot(n, x, incX, y, incY, c, s);
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
                _zrotg(ref a, ref b, out c, out s);
            }

            /// <summary>Computes the product of a vector by a scalar, i.e. x = a * x.
            /// </summary>
            /// <param name="n">The number of elements of <paramref name="x"/>.</param>
            /// <param name="a">The scalar factor 'a'.</param>
            /// <param name="x">The vector 'x' with at least <paramref name="n"/> elements, contains the updated vector after function call.</param>
            /// <param name="incX">The increment for <paramref name="x"/>.</param>
            public void zscal(int n, Complex a, Complex[] x, int incX = 1)
            {
                _zscal(n, ref a, x, incX);
            }

            /// <summary>Computes the product of a vector by a scalar, i.e. x = a * x.
            /// </summary>
            /// <param name="n">The number of elements of <paramref name="x"/>.</param>
            /// <param name="a">The scalar factor 'a'.</param>
            /// <param name="x">The vector 'x' with at least <paramref name="n"/> elements, contains the updated vector after function call.</param>
            /// <param name="incX">The increment for <paramref name="x"/>.</param>
            public void zdscal(int n, double a, Complex[] x, int incX = 1)
            {
                _zdscal(n, a, x, incX);
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
                    fixed (Complex* xPtr = &x[startIndex])
                    {
                        _zscal(n, ref a, xPtr, increment);
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
                        _zdscal(n, a, xPtr, increment);
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
                _zswap(n, x, 1, y, 1);
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
                    fixed (Complex* xPtr = &x[startIndexX], yPtr = &y[startIndexY])
                    {
                        _zswap(n, xPtr, incX, yPtr, incY);
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
                return _izamax(n, x, 1);
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
                return _izamax(n, x, incX);
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
                double compareValue = Complex.Abs(x[0]);

                for (int j = 1; j < n; j++)
                {
                    if (Complex.Abs(x[j]) < compareValue)
                    {
                        compareValue = Complex.Abs(x[j]);
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
                double compareValue = Complex.Abs(x[0]);

                for (int j = 1; j < n; j++)
                {
                    if (Complex.Abs(x[j * incX]) < compareValue)
                    {
                        compareValue = Complex.Abs(x[j * incX]);
                        index = j * incX;
                    }
                }
                return index;
            }
            #endregion

            #endregion
        }
    }
}