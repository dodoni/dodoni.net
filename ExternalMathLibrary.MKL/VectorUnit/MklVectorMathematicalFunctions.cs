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
    /// <summary>Provides mathematical functions with respect to Intel's MKL Vector Mathematical Functions Library (VML).
    /// </summary>
    /// <remarks>Tested with MKL Release 10.2.x.</remarks>
    internal class MklVectorMathematicalFunctions : IVectorUnitBasics, IVectorUnitSpecial
    {
        #region private const members

        /// <summary>Low accuracy VML functions are called.
        /// </summary>
        private const int VML_LA = 0x00000001;

        /// <summary>High accuracy VML functions are called.
        /// </summary>
        private const int VML_HA = 0x00000002;

        /// <summary>Use this value if the calls are typically to double precision VML functions.
        /// </summary>
        private const int VML_DOUBLE_CONSISTENT = 0x00000020;

        /// <summary>Ignore errors.
        /// </summary>
        private const int VML_ERRMODE_IGNORE = 0x00000100;

        /// <summary>Errno variable is set to error.
        /// </summary>
        private const int VML_ERRMODE_ERRNO = 0x00000200;

        /// <summary>Error description text is written to stderr on error.
        /// </summary>
        private const int VML_ERRMODE_STDERR = 0x00000400;

        /// <summary>Exception is raised on error.
        /// </summary>
        private const int VML_ERRMODE_EXCEPT = 0x00000800;

        /// <summary>User's error handler function is called on error.
        /// </summary>
        private const int VML_ERRMODE_CALLBACK = 0x00001000;

        /// <summary>Errno variable is set, exceptions are raided and user's error handler is called on error.
        /// </summary>
        private const int VML_ERRMODE_DEFAULT = VML_ERRMODE_ERRNO | VML_ERRMODE_CALLBACK | VML_ERRMODE_EXCEPT;

        /// <summary>Extract accuracy bits.
        /// </summary>
        private const int VML_ACCURACY_MASK = 0x0000000F;

        /// <summary>Exctract floating-point control bits.
        /// </summary>
        private const int VML_FPUMODE_MASK = 0x000000F0;

        /// <summary>Extract error handling control bits (including error callback bits).
        /// </summary>
        private const int VML_ERRMODE_MASK = 0x0000FF00;

        /// <summary>Extract error handling control bits (not including error callback bits).
        /// </summary>
        private const int VML_ERRMODE_STDHANDLER_MASK = 0x00000F00;

        /// <summary>Extract error callback bits.
        /// </summary>
        private const int VML_ERRMODE_CALLBACK_MASK = 0x0000F000;
        #endregion

        #region private function import

        #region Arithmetic functions

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vdAdd", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern int _vdAdd(int n, double[] a, double[] b, double[] y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vdAdd", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern int _vdAdd(int n, double* a, double* b, double* y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vzAdd", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern int _vzAdd(int n, [In, Out] Complex[] a, [In, Out] Complex[] b, [In, Out] Complex[] y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vzAdd", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern int _vzAdd(int n, Complex* a, Complex* b, Complex* y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vdSub", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern int _vdSub(int n, double[] a, double[] b, double[] y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vdSub", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern int _vdSub(int n, double* a, double* b, double* y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vzSub", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern int _vzSub(int n, [In, Out] Complex[] a, [In, Out] Complex[] b, [In, Out] Complex[] y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vzSub", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern int _vzSub(int n, Complex* a, Complex* b, Complex* y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vdSqr", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern int _vdSqr(int n, double[] a, double[] y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vdSqr", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern int _vdSqr(int n, double* a, double* y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vdMul", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern int _vdMul(int n, double[] a, double[] b, double[] y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vdMul", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern int _vdMul(int n, double* a, double* b, double* y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vzMul", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern int _vzMul(int n, [In, Out] Complex[] a, [In, Out] Complex[] b, [In, Out] Complex[] y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vzMul", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern int _vzMul(int n, Complex* a, Complex* b, Complex* y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vzConj", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern int _vzConj(int n, [In, Out] Complex[] a, [In, Out] Complex[] y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vzConj", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern int _vzConj(int n, Complex* a, Complex* y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vdAbs", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern int _vdAbs(int n, double[] a, double[] y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vdAbs", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern int _vdAbs(int n, double* a, double* y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vzAbs", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern int _vzAbs(int n, [In, Out] Complex[] a, double[] y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vzAbs", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern int _vzAbs(int n, Complex* a, double* y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vdLinearFrac", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _vdLinearFrac(int n, double[] a, double[] b, double scaleFactorA, double shiftA, double scaleFactorB, double shiftB, double[] y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vdLinearFrac", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void _vdLinearFrac(int n, double* a, double* b, double scaleFactorA, double shiftA, double scaleFactorB, double shiftB, double* y);
        #endregion

        #region Power and Root functions

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vdInv", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern int _vdInv(int n, double[] a, double[] y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vdInv", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern int _vdInv(int n, double* a, double* y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vdDiv", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern int _vdDiv(int n, double[] a, double[] b, double[] y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vdDiv", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern int _vdDiv(int n, double* a, double* b, double* y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vdSqrt", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern int _vdSqrt(int n, double[] a, double[] y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vdSqrt", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern int _vdSqrt(int n, double* a, double* y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vdPow", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern int _vdPow(int n, double[] a, double[] b, double[] y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vdPow", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern int _vdPow(int n, double* a, double* b, double* y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vzPow", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern int _vzPow(int n, [In, Out] Complex[] a, [In, Out] Complex[] b, [In, Out] Complex[] y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vzPow", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern int _vzPow(int n, Complex* a, Complex* b, Complex* y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vdPowx", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern int _vdPow(int n, double[] a, double b, double[] y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vdPowx", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern int _vdPow(int n, double* a, double b, double* y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vzPowx", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern int _vzPow(int n, [In, Out] Complex[] a, Complex b, [In, Out] Complex[] y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vzPowx", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern int _vzPow(int n, Complex* a, Complex b, Complex* y);
        #endregion

        #region Exponential and Logarithmic Functions

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vdExp", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern int _vdExp(int n, double[] a, double[] y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vdExp", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern int _vdExp(int n, double* a, double* y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vzExp", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern int _vzExp(int n, [In, Out] Complex[] a, [In, Out] Complex[] y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vzExp", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern int _vzExp(int n, Complex* a, Complex* y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vdLn", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern int _vdLn(int n, double[] a, double[] y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vdLn", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern int _vdLn(int n, double* a, double* y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vzLn", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern int _vzLn(int n, [In, Out] Complex[] a, [In, Out] Complex[] y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vzLn", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern int _vzLn(int n, Complex* a, Complex* y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vdLog10", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern int _vdLog10(int n, double[] a, double[] y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vdLog10", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern int _vdLog10(int n, double* a, double* y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vzLog10", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern int _vzLog10(int n, [In, Out] Complex[] a, [In, Out] Complex[] y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vzLog10", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern int _vzLog10(int n, Complex* a, Complex* y);
        #endregion

        #region Trigonometric Functions

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vdCos", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern int _vdCos(int n, double[] a, double[] y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vdCos", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern int _vdCos(int n, double* a, double* y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vzCos", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern int _vzCos(int n, [In, Out] Complex[] a, [In, Out] Complex[] y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vzCos", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern int _vzCos(int n, Complex* a, Complex* y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vdSin", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern int _vdSin(int n, double[] a, double[] y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vdSin", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern int _vdSin(int n, double* a, double* y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vzSin", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern int _vzSin(int n, [In, Out] Complex[] a, [In, Out] Complex[] y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vzSin", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern int _vzSin(int n, Complex* a, Complex* y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vdSinCos", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern int _vdSinCos(int n, double[] a, double[] y, double[] z);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vdSinCos", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern int _vdSinCos(int n, double* a, double* y, double* z);
        #endregion

        #region special functions

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vdCdfNorm", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _vdCdfNorm(int n, double[] a, double[] y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vdCdfNorm", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void _vdCdfNorm(int n, double* a, double* y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vdCdfNormInv", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void _vdCdfNormInv(int n, double[] a, double[] y);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vdCdfNormInv", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern void _vdCdfNormInv(int n, double* a, double* y);
        #endregion

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vmlSetMode", CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern int _vmlSetMode(int mode);
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="MklVectorMathematicalFunctions"/> class.
        /// </summary>
        internal MklVectorMathematicalFunctions()
        {
        }
        #endregion

        #region public methods

        #region IVectorUnitBasics Members

        #region Arithmetic functions

        /// <summary>Performs element by element addition of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector a+b.</param>
        public void Add(int n, double[] a, double[] b, double[] y)
        {
            _vdAdd(n, a, b, y);
        }

        /// <summary>Performs element by element addition of vector <paramref name="a" /> and vector <paramref name="b" />, i.e. a = a + b [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a + b.</param>
        /// <param name="b">The input vector b.</param>
        public void Add(int n, double[] a, double[] b)
        {
            _vdAdd(n, a, b, a);
        }

        /// <summary>Performs element by element addition of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] :=a[<paramref name="startIndexA"/> + j] + b[<paramref name="startIndexY"/> + j] for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexB">The null-based start index of <paramref name="b"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Add(int n, double[] a, double[] b, double[] y, int startIndexA, int startIndexB, int startIndexY)
        {
            unsafe
            {
                fixed (double* ptrA = &a[startIndexA], ptrB = &b[startIndexB], ptrY = &y[startIndexY])
                {
                    _vdAdd(n, ptrA, ptrB, ptrY);
                }
            }
        }

        /// <summary>Performs addition of vector <paramref name="a" /> and double precision number <paramref name="b" />.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input double precision number b.</param>
        /// <param name="y">The output vector a+b.</param>
        public void Add(int n, double[] a, double b, double[] y)
        {
            double[] test = null; //  new double[n];
           // _vdLinearFrac(n, a, test, 1.0, b, 0.0, 1.0, y);
            
            // alternativ:
            unsafe
            {
                fixed (double* ptrA = a, ptrY = y, ptrTemp= test)
                {
                   _vdLinearFrac(n, ptrA, ptrTemp, 1.0, b, 0.0, 1.0, ptrY);
                }
            }
        }

        /// <summary>Performs addition of vector <paramref name="a" /> and double precision number <paramref name="b" />, i.e. a_j = a_j + b [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a + b.</param>
        /// <param name="b">The input double precision number b.</param>
        public void Add(int n, double[] a, double b)
        {
            _vdLinearFrac(n, a, null, 1.0, b, 0.0, 1.0, a);
        }

        /// <summary>Performs element by element addition of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector a+b.</param>
        public void Add(int n, Complex[] a, Complex[] b, Complex[] y)
        {
            _vzAdd(n, a, b, y);
        }

        /// <summary>Performs element by element addition of vector <paramref name="a" /> and vector <paramref name="b" />, i.e. a = a + b [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a + b.</param>
        /// <param name="b">The input vector b.</param>
        public void Add(int n, Complex[] a, Complex[] b)
        {
            _vzAdd(n, a, b, a);
        }

        /// <summary>Performs element by element addition of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] :=a[<paramref name="startIndexA"/> + j] + b[<paramref name="startIndexY"/> + j] for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexB">The null-based start index of <paramref name="b"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Add(int n, Complex[] a, Complex[] b, Complex[] y, int startIndexA, int startIndexB, int startIndexY)
        {
            unsafe
            {
                fixed (Complex* ptrA = &a[startIndexA], ptrB = &b[startIndexB], ptrY = &y[startIndexY])
                {
                    _vzAdd(n, ptrA, ptrB, ptrY);
                }
            }
        }

        /// <summary>Performs element by element subtraction of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector a-b.</param>
        public void Sub(int n, double[] a, double[] b, double[] y)
        {
            _vdSub(n, a, b, y);
        }

        /// <summary>Performs element by element subtraction of vector <paramref name="a" /> and vector <paramref name="b" />, i.e. a = a - b [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a - b.</param>
        /// <param name="b">The input vector b.</param>
        public void Sub(int n, double[] a, double[] b)
        {
            _vdSub(n, a, b, a);
        }

        /// <summary>Performs element by element subtraction of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] :=a[<paramref name="startIndexA"/> + j] - b[<paramref name="startIndexY"/> + j] for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexB">The null-based start index of <paramref name="b"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Sub(int n, double[] a, double[] b, double[] y, int startIndexA, int startIndexB, int startIndexY)
        {
            unsafe
            {
                fixed (double* ptrA = &a[startIndexA], ptrB = &b[startIndexB], ptrY = &y[startIndexY])
                {
                    _vdSub(n, ptrA, ptrB, ptrY);
                }
            }
        }

        /// <summary>Performs element by element subtraction of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector a-b.</param>
        public void Sub(int n, Complex[] a, Complex[] b, Complex[] y)
        {
            _vzSub(n, a, b, y);
        }

        /// <summary>Performs element by element subtraction of vector <paramref name="a" /> and vector <paramref name="b" />, i.e. a = a - b [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a - b.</param>
        /// <param name="b">The input vector b.</param>
        public void Sub(int n, Complex[] a, Complex[] b)
        {
            _vzSub(n, a, b, a);
        }

        /// <summary>Performs element by element subtraction of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] :=a[<paramref name="startIndexA"/> + j] - b[<paramref name="startIndexY"/> + j] for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexB">The null-based start index of <paramref name="b"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Sub(int n, Complex[] a, Complex[] b, Complex[] y, int startIndexA, int startIndexB, int startIndexY)
        {
            unsafe
            {
                fixed (Complex* ptrA = &a[startIndexA], ptrB = &b[startIndexB], ptrY = &y[startIndexY])
                {
                    _vzSub(n, ptrA, ptrB, ptrY);
                }
            }
        }

        /// <summary>Performs element by element squaring of the vector.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector a[j]^2, j=0,...,<paramref name="n"/>-1.</param>
        public void Sqr(int n, double[] a, double[] y)
        {
            _vdSqr(n, a, y);
        }

        /// <summary>Performs element by element squaring of the vector [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a[j]^2, j=0,...,<paramref name="n" />-1.</param>
        public void Sqr(int n, double[] a)
        {
            _vdSqr(n, a, a);
        }

        /// <summary>Performs element by element squaring of the vector.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] :=a[<paramref name="startIndexA"/> + j]^2 for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Sqr(int n, double[] a, double[] y, int startIndexA, int startIndexY)
        {
            unsafe
            {
                fixed (double* ptrA = &a[startIndexA], ptrY = &y[startIndexY])
                {
                    _vdSqr(n, ptrA, ptrY);
                }
            }
        }

        /// <summary>Performs element by element multiplication of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector a[j]*b[j], j=0,...,<paramref name="n"/>-1.</param>
        public void Mul(int n, double[] a, double[] b, double[] y)
        {
            _vdMul(n, a, b, y);
        }

        /// <summary>Performs element by element multiplication of vector <paramref name="a" /> and vector <paramref name="b" /> [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a[j]*b[j], j=0,...,<paramref name="n" />-1.</param>
        /// <param name="b">The input vector b.</param>
        public void Mul(int n, double[] a, double[] b)
        {
            _vdMul(n, a, b, a);
        }

        /// <summary>Performs element by element multiplication of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] :=a[<paramref name="startIndexA"/> + j] * b[<paramref name="startIndexY"/> + j] for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexB">The null-based start index of <paramref name="b"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Mul(int n, double[] a, double[] b, double[] y, int startIndexA, int startIndexB, int startIndexY)
        {
            unsafe
            {
                fixed (double* ptrA = &a[startIndexA], ptrB = &b[startIndexB], ptrY = &y[startIndexY])
                {
                    _vdMul(n, ptrA, ptrB, ptrY);
                }
            }
        }

        /// <summary>Performs element by element multiplication of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector a[j]*b[j], j=0,...,<paramref name="n"/>-1.</param>
        public void Mul(int n, Complex[] a, Complex[] b, Complex[] y)
        {
            _vzMul(n, a, b, y);
        }

        /// <summary>Performs element by element multiplication of vector <paramref name="a" /> and vector <paramref name="b" /> [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a[j]*b[j], j=0,...,<paramref name="n" />-1.</param>
        /// <param name="b">The input vector b.</param>
        public void Mul(int n, Complex[] a, Complex[] b)
        {
            _vzMul(n, a, b, a);
        }

        /// <summary>Performs element by element multiplication of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] :=a[<paramref name="startIndexA"/> + j] * b[<paramref name="startIndexY"/> + j] for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexB">The null-based start index of <paramref name="b"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Mul(int n, Complex[] a, Complex[] b, Complex[] y, int startIndexA, int startIndexB, int startIndexY)
        {
            unsafe
            {
                fixed (Complex* ptrA = &a[startIndexA], ptrB = &b[startIndexB], ptrY = &y[startIndexY])
                {
                    _vzMul(n, ptrA, ptrB, ptrY);
                }
            }
        }

        /// <summary>Performs element by element conjugation of vector <paramref name="a"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \conj(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Conjugate(int n, Complex[] a, Complex[] y)
        {
            _vzConj(n, a, y);
        }

        /// <summary>Performs element by element conjugation of vector <paramref name="a" /> [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result ot the operation, i.e. \conj(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Conjugate(int n, Complex[] a)
        {
            _vzConj(n, a, a);
        }

        /// <summary>Performs element by element conjugation of vector <paramref name="a"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] :=\conj(a[<paramref name="startIndexA"/> + j]) for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Conjugate(int n, Complex[] a, Complex[] y, int startIndexA, int startIndexY)
        {
            unsafe
            {
                fixed (Complex* ptrA = &a[startIndexA], ptrY = &y[startIndexY])
                {
                    _vzConj(n, ptrA, ptrY);
                }
            }
        }

        /// <summary>Computes absolute value of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector |a[j]|, j=0,...,<paramref name="n"/>-1.</param>
        public void Abs(int n, double[] a, double[] y)
        {
            _vdAbs(n, a, y);
        }

        /// <summary>Computes absolute value of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the resulf ot the operation, i.e. |a[j]|, j=0,...,<paramref name="n" />-1.</param>
        public void Abs(int n, double[] a)
        {
            _vdAbs(n, a, a);
        }

        /// <summary>Computes absolute value of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] :=|a[<paramref name="startIndexA"/> + j]| for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Abs(int n, double[] a, double[] y, int startIndexA, int startIndexY)
        {
            unsafe
            {
                fixed (double* ptrA = &a[startIndexA], ptrY = &y[startIndexY])
                {
                    _vdAbs(n, ptrA, ptrY);
                }
            }
        }

        /// <summary>Computes absolute value of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector |a[j]|, j=0,...,<paramref name="n"/>-1.</param>
        public void Abs(int n, Complex[] a, double[] y)
        {
            _vzAbs(n, a, y);
        }

        /// <summary>Computes absolute value of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] :=|a[<paramref name="startIndexA"/> + j]| for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Abs(int n, Complex[] a, double[] y, int startIndexA, int startIndexY)
        {
            unsafe
            {
                fixed (Complex* ptrA = &a[startIndexA])
                {
                    fixed (double* ptrY = &y[startIndexY])
                    {
                        _vzAbs(n, ptrA, ptrY);
                    }
                }
            }
        }

        /// <summary>Performs linear fraction transformation of vector <paramref name="a"/> and vector <paramref name="b"/> with scalar parameters.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector y[j]= (<paramref name="scaleFactorA"/> * a[j] + <paramref name="shiftA"/>) / (<paramref name="scaleFactorB"/> * b[j] + <paramref name="shiftB"/>), j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="scaleFactorA">The scaling factor for vector a.</param>
        /// <param name="scaleFactorB">The scaling factor for vector b.</param>
        /// <param name="shiftA">Constant value for shifting addends of vector a.</param>
        /// <param name="shiftB">Constant value for shifting addends of vector b.</param>
        public void LinearFraction(int n, double[] a, double[] b, double[] y, double scaleFactorA, double scaleFactorB, double shiftA = 0.0, double shiftB = 0.0)
        {
            _vdLinearFrac(n, a, b, scaleFactorA, shiftA, scaleFactorB, shiftB, y);
        }

        /// <summary>Performs linear fraction transformation of vector <paramref name="a"/> and vector <paramref name="b"/> with scalar parameters.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] := (<paramref name="scaleFactorA"/> * a[<paramref name="startIndexA"/> + j] + <paramref name="shiftA"/>) / (<paramref name="scaleFactorB"/> * b[<paramref name="startIndexB"/> + j] + <paramref name="shiftB"/>) for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexB">The null-based start index of <paramref name="b"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        /// <param name="scaleFactorA">The scaling factor for vector a.</param>
        /// <param name="scaleFactorB">The scaling factor for vector b.</param>
        /// <param name="shiftA">Constant value for shifting addends of vector a.</param>
        /// <param name="shiftB">Constant value for shifting addends of vector b.</param>
        public void LinearFraction(int n, double[] a, double[] b, double[] y, int startIndexA, int startIndexB, int startIndexY, double scaleFactorA, double scaleFactorB, double shiftA = 0.0, double shiftB = 0.0)
        {
            unsafe
            {
                fixed (double* ptrA = &a[startIndexA], ptrB = &b[startIndexB], ptrY = &y[startIndexY])
                {
                    _vdLinearFrac(n, ptrA, ptrB, scaleFactorA, shiftA, scaleFactorB, shiftB, ptrY);
                }
            }
        }
        #endregion

        #region Power and Root functions

        /// <summary>Performs element by element inversion of the vector.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector.</param>
        /// <param name="y">The output vector.</param>
        public void Inv(int n, double[] a, double[] y)
        {
            _vdInv(n, a, y);
        }

        /// <summary>Performs element by element inversion of the vector [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector; overwritten by the result of the operation.</param>
        public void Inv(int n, double[] a)
        {
            _vdInv(n, a, a);
        }

        /// <summary>Performs element by element inversion of the vector.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] :=1.0 / a[<paramref name="startIndexA"/> + j] for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Inv(int n, double[] a, double[] y, int startIndexA, int startIndexY)
        {
            unsafe
            {
                fixed (double* ptrA = &a[startIndexA], ptrY = &y[startIndexY])
                {
                    _vdInv(n, ptrA, ptrY);
                }
            }
        }

        /// <summary>Performs element by element square root calculation of the vector.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \sqrt(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Sqrt(int n, double[] a, double[] y)
        {
            _vdSqrt(n, a, y);
        }

        /// <summary>Performs element by element square root calculation of the vector [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \sqrt(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Sqrt(int n, double[] a)
        {
            _vdSqrt(n, a, a);
        }

        /// <summary>Performs element by element square root calculation of the vector.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] :=\sqrt(a[<paramref name="startIndexA"/> + j]) for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Sqrt(int n, double[] a, double[] y, int startIndexA, int startIndexY)
        {
            unsafe
            {
                fixed (double* ptrA = &a[startIndexA], ptrY = &y[startIndexY])
                {
                    _vdSqrt(n, ptrA, ptrY);
                }
            }
        }

        /// <summary>Computes a to the power b for elements of two vectors.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector a[j]^b[j], j=0,...,<paramref name="n"/>-1.</param>
        public void Pow(int n, double[] a, double[] b, double[] y)
        {
            _vdPow(n, a, b, y);
        }

        /// <summary>Computes a to the power b for elements of two vectors [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a[j]^b[j], j=0,...,<paramref name="n" />-1.</param>
        /// <param name="b">The input vector b.</param>
        public void Pow(int n, double[] a, double[] b)
        {
            _vdPow(n, a, b, a);
        }

        /// <summary>Computes a to the power b for elements of two vectors.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] :=a[<paramref name="startIndexA"/> + j]^b[<paramref name="startIndexB"/> + j] for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexB">The null-based start index of <paramref name="b"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Pow(int n, double[] a, double[] b, double[] y, int startIndexA, int startIndexB, int startIndexY)
        {
            unsafe
            {
                fixed (double* ptrA = &a[startIndexA], ptrB = &b[startIndexB], ptrY = &y[startIndexY])
                {
                    _vdPow(n, ptrA, ptrB, ptrY);
                }
            }
        }

        /// <summary>Computes a to the power b for elements of two vectors.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector a[j]^b[j], j=0,...,<paramref name="n"/>-1.</param>
        public void Pow(int n, Complex[] a, Complex[] b, Complex[] y)
        {
            _vzPow(n, a, b, y);
        }

        /// <summary>Computes a to the power b for elements of two vectors [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a[j]^b[j], j=0,...,<paramref name="n" />-1.</param>
        /// <param name="b">The input vector b.</param>
        public void Pow(int n, Complex[] a, Complex[] b)
        {
            _vzPow(n, a, b, a);
        }

        /// <summary>Computes a to the power b for elements of two vectors.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] :=a[<paramref name="startIndexA"/> + j]^b[<paramref name="startIndexB"/> + j] for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexB">The null-based start index of <paramref name="b"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Pow(int n, Complex[] a, Complex[] b, Complex[] y, int startIndexA, int startIndexB, int startIndexY)
        {
            unsafe
            {
                fixed (Complex* ptrA = &a[startIndexA], ptrB = &b[startIndexB], ptrY = &y[startIndexY])
                {
                    _vzPow(n, ptrA, ptrB, ptrY);
                }
            }
        }

        /// <summary>Raises each element of a vector to the constant power.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The constant power.</param>
        /// <param name="y">The output vector a[j]^b, j=0,...,<paramref name="n"/>-1.</param>
        public void Pow(int n, double[] a, double b, double[] y)
        {
            _vdPow(n, a, b, y);
        }

        /// <summary>Raises each element of a vector to the constant power [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a[j]^b, j=0,...,<paramref name="n" />-1.</param>
        /// <param name="b">The constant power.</param>
        public void Pow(int n, double[] a, double b)
        {
            _vdPow(n, a, b, a);
        }

        /// <summary>Raises each element of a vector to the constant power.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The constant power.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] :=a[<paramref name="startIndexA"/> + j]^b for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Pow(int n, double[] a, double b, double[] y, int startIndexA, int startIndexY)
        {
            unsafe
            {
                fixed (double* ptrA = &a[startIndexA], ptrY = &y[startIndexY])
                {
                    _vdPow(n, ptrA, b, ptrY);
                }
            }
        }

        /// <summary>Raises each element of a vector to the constant power.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The constant power.</param>
        /// <param name="y">The output vector a[j]^b, j=0,...,<paramref name="n"/>-1.</param>
        public void Pow(int n, Complex[] a, Complex b, Complex[] y)
        {
            _vzPow(n, a, b, y);
        }

        /// <summary>Raises each element of a vector to the constant power [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a[j]^b, j=0,...,<paramref name="n" />-1.</param>
        /// <param name="b">The constant power.</param>
        public void Pow(int n, Complex[] a, Complex b)
        {
            _vzPow(n, a, b, a);
        }

        /// <summary>Raises each element of a vector to the constant power.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The constant power.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] :=a[<paramref name="startIndexA"/> + j]^b for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Pow(int n, Complex[] a, Complex b, Complex[] y, int startIndexA, int startIndexY)
        {
            unsafe
            {
                fixed (Complex* ptrA = &a[startIndexA], ptrY = &y[startIndexY])
                {
                    _vzPow(n, ptrA, b, ptrY);
                }
            }
        }
        #endregion

        #region Exponential and Logarithmic Functions

        /// <summary>Computes an exponential of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \exp(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Exp(int n, double[] a, double[] y)
        {
            _vdExp(n, a, y);
        }

        /// <summary>Computes an exponential of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \exp(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Exp(int n, double[] a)
        {
            _vdExp(n, a, a);
        }

        /// <summary>Computes an exponential of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] := \exp(a[<paramref name="startIndexA"/> + j]) for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Exp(int n, double[] a, double[] y, int startIndexA, int startIndexY)
        {
            unsafe
            {
                fixed (double* ptrA = &a[startIndexA], ptrY = &y[startIndexY])
                {
                    _vdExp(n, ptrA, ptrY);
                }
            }
        }

        /// <summary>Computes an exponential of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \exp(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Exp(int n, Complex[] a, Complex[] y)
        {
            _vzExp(n, a, y);
        }

        /// <summary>Computes an exponential of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \exp(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Exp(int n, Complex[] a)
        {
            _vzExp(n, a, a);
        }

        /// <summary>Computes an exponential of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] := \exp(a[<paramref name="startIndexA"/> + j]) for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Exp(int n, Complex[] a, Complex[] y, int startIndexA, int startIndexY)
        {
            unsafe
            {
                fixed (Complex* ptrA = &a[startIndexA], ptrY = &y[startIndexY])
                {
                    _vzExp(n, ptrA, ptrY);
                }
            }
        }

        /// <summary>Computes natural logarithm of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \log(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Log(int n, double[] a, double[] y)
        {
            _vdLn(n, a, y);
        }

        /// <summary>Computes natural logarithm of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \log(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Log(int n, double[] a)
        {
            _vdLn(n, a, a);
        }

        /// <summary>Computes natural logarithm of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] := \log(a[<paramref name="startIndexA"/> + j]) for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Log(int n, double[] a, double[] y, int startIndexA, int startIndexY)
        {
            unsafe
            {
                fixed (double* ptrA = &a[startIndexA], ptrY = &y[startIndexY])
                {
                    _vdLn(n, ptrA, ptrY);
                }
            }
        }

        /// <summary>Computes natural logarithm of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \log(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Log(int n, Complex[] a, Complex[] y)
        {
            _vzLn(n, a, y);
        }

        /// <summary>Computes natural logarithm of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \log(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Log(int n, Complex[] a)
        {
            _vzLn(n, a, a);
        }

        /// <summary>Computes natural logarithm of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] := \log(a[<paramref name="startIndexA"/> + j]) for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Log(int n, Complex[] a, Complex[] y, int startIndexA, int startIndexY)
        {
            unsafe
            {
                fixed (Complex* ptrA = &a[startIndexA], ptrY = &y[startIndexY])
                {
                    _vzLn(n, ptrA, ptrY);
                }
            }
        }

        /// <summary>Computes denary logarithm of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \log_10(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Log10(int n, double[] a, double[] y)
        {
            _vdLog10(n, a, y);
        }

        /// <summary>Computes denary logarithm of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \log_10(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Log10(int n, double[] a)
        {
            _vdLog10(n, a, a);
        }

        /// <summary>Computes denary logarithm of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] := \log_10(a[<paramref name="startIndexA"/> + j]) for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Log10(int n, double[] a, double[] y, int startIndexA, int startIndexY)
        {
            unsafe
            {
                fixed (double* ptrA = &a[startIndexA], ptrY = &y[startIndexY])
                {
                    _vdLog10(n, ptrA, ptrY);
                }
            }
        }

        /// <summary>Computes denary logarithm of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \log_10(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Log10(int n, Complex[] a, Complex[] y)
        {
            _vzLog10(n, a, y);
        }

        /// <summary>Computes denary logarithm of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \log_10(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Log10(int n, Complex[] a)
        {
            _vzLog10(n, a, a);
        }

        /// <summary>Computes denary logarithm of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] := \log_10(a[<paramref name="startIndexA"/> + j]) for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Log10(int n, Complex[] a, Complex[] y, int startIndexA, int startIndexY)
        {
            unsafe
            {
                fixed (Complex* ptrA = &a[startIndexA], ptrY = &y[startIndexY])
                {
                    _vzLog10(n, ptrA, ptrY);
                }
            }
        }
        #endregion

        #region Trigonometric Functions

        /// <summary>Computes cosine of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \cos(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Cos(int n, double[] a, double[] y)
        {
            _vdCos(n, a, y);
        }

        /// <summary>Computes cosine of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \cos(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Cos(int n, double[] a)
        {
            _vdCos(n, a, a);
        }

        /// <summary>Computes cosine of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] := \cos(a[<paramref name="startIndexA"/> + j]) for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Cos(int n, double[] a, double[] y, int startIndexA, int startIndexY)
        {
            unsafe
            {
                fixed (double* ptrA = &a[startIndexA], ptrY = &y[startIndexY])
                {
                    _vdCos(n, ptrA, ptrY);
                }
            }
        }

        /// <summary>Computes cosine of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \cos(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Cos(int n, Complex[] a, Complex[] y)
        {
            _vzCos(n, a, y);
        }

        /// <summary>Computes cosine of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \cos(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Cos(int n, Complex[] a)
        {
            _vzCos(n, a, a);
        }

        /// <summary>Computes cosine of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] := \cos(a[<paramref name="startIndexA"/> + j]) for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Cos(int n, Complex[] a, Complex[] y, int startIndexA, int startIndexY)
        {
            unsafe
            {
                fixed (Complex* ptrA = &a[startIndexA], ptrY = &y[startIndexY])
                {
                    _vzCos(n, ptrA, ptrY);
                }
            }
        }

        /// <summary>Computes sine of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \sin(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Sin(int n, double[] a, double[] y)
        {
            _vdSin(n, a, y);
        }

        /// <summary>Computes sine of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \sin(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Sin(int n, double[] a)
        {
            _vdSin(n, a, a);
        }

        /// <summary>Computes sine of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] := \sin(a[<paramref name="startIndexA"/> + j]) for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Sin(int n, double[] a, double[] y, int startIndexA, int startIndexY)
        {
            unsafe
            {
                fixed (double* ptrA = &a[startIndexA], ptrY = &y[startIndexY])
                {
                    _vdSin(n, ptrA, ptrY);
                }
            }
        }

        /// <summary>Computes sine of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \sin(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Sin(int n, Complex[] a, Complex[] y)
        {
            _vzSin(n, a, y);
        }

        /// <summary>Computes sine of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \sin(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Sin(int n, Complex[] a)
        {
            _vzSin(n, a, a);
        }

        /// <summary>Computes sine of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] := \sin(a[<paramref name="startIndexA"/> + j]) for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Sin(int n, Complex[] a, Complex[] y, int startIndexA, int startIndexY)
        {
            unsafe
            {
                fixed (Complex* ptrA = &a[startIndexA], ptrY = &y[startIndexY])
                {
                    _vzSin(n, ptrA, ptrY);
                }
            }
        }

        /// <summary>Computes sine and cosine of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \sin(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        /// <param name="z">The output vector \cos(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void SinCos(int n, double[] a, double[] y, double[] z)
        {
            _vdSinCos(n, a, y, z);
        }

        /// <summary>Computes sine and cosine of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] := \sin(a[<paramref name="startIndexA"/> + j]) for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="z">The output vector z with z[<paramref name="startIndexY"/> + j] := \cos(a[<paramref name="startIndexA"/> + j]) for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        /// <param name="startIndexZ">The null-based start index of <paramref name="z"/>.</param>
        public void SinCos(int n, double[] a, double[] y, double[] z, int startIndexA, int startIndexY, int startIndexZ)
        {
            unsafe
            {
                fixed (double* ptrA = &a[startIndexA], ptrY = &y[startIndexY], ptrZ = &z[startIndexZ])
                {
                    _vdSinCos(n, ptrA, ptrY, ptrZ);
                }
            }
        }
        #endregion

        #endregion

        #region IVectorUnitSpecial Members

        /// <summary>Computes the cumulative normal distribution function values of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector N(a[j]), j=0,...,<paramref name="n"/>-1, where N(x) =\int_{-\infty}^x  1/\sqrt(2*PI) * exp(-1/2 *t^2) dt.</param>
        public void CdfNorm(int n, double[] a, double[] y)
        {
            _vdCdfNorm(n, a, y);
        }

        /// <summary>Computes the cumulative normal distribution function values of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. N(a[j]), j=0,...,<paramref name="n" />-1, where N(x) =\int_{-\infty}^x  1/\sqrt(2*PI) * exp(-1/2 *t^2) dt.</param>
        public void CdfNorm(int n, double[] a)
        {
            _vdCdfNorm(n, a, a);
        }

        /// <summary>Computes the cumulative normal distribution function values of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] := N(a[<paramref name="startIndexA"/> + j]) for j = 0,...,<paramref name="n"/>-1, where N(x) =\int_{-\infty}^x  1/\sqrt(2*PI) * exp(-1/2 *t^2) dt.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void CdfNorm(int n, double[] a, double[] y, int startIndexA, int startIndexY)
        {
            unsafe
            {
                fixed (double* aPtr = &a[startIndexA], yPtr = &y[startIndexY])
                {
                    _vdCdfNorm(n, aPtr, yPtr);
                }
            }
        }

        /// <summary>Computes the inverse cumulative normal distribution function values of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector N^{-1}(a[j]), j=0,...,<paramref name="n"/>-1, where N(x) =\int_{-\infty}^x  1/\sqrt(2*PI) * exp(-1/2 *t^2) dt.</param>
        public void CdfNormInv(int n, double[] a, double[] y)
        {
            _vdCdfNormInv(n, a, y);
        }

        /// <summary>Computes the inverse cumulative normal distribution function values of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. N^{-1}(a[j]), j=0,...,<paramref name="n" />-1, where N(x) =\int_{-\infty}^x  1/\sqrt(2*PI) * exp(-1/2 *t^2) dt.</param>
        public void CdfNormInv(int n, double[] a)
        {
            _vdCdfNormInv(n, a, a);
        }

        /// <summary>Computes the inverse cumulative normal distribution function values of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] := N^{-1}(a[<paramref name="startIndexA"/> + j]) for j = 0,...,<paramref name="n"/>-1, where N(x) =\int_{-\infty}^x  1/\sqrt(2*PI) * exp(-1/2 *t^2) dt.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void CdfNormInv(int n, double[] a, double[] y, int startIndexA, int startIndexY)
        {
            unsafe
            {
                fixed (double* aPtr = &a[startIndexA], yPtr = &y[startIndexY])
                {
                    _vdCdfNormInv(n, aPtr, yPtr);
                }
            }
        }
        #endregion

        #endregion

        #region internal methods

        /// <summary>Initializes the Library.
        /// </summary>
        /// <remarks>Call this method before using the Library the first time.</remarks>
        internal void Initialize()
        {
            /* setting floating-point precision and rounding mode:  */

            // VML_FTZDAZ_ON/_OFF fehlt noch!
            _vmlSetMode(VML_HA | VML_DOUBLE_CONSISTENT | VML_ERRMODE_DEFAULT);
        }

        public void Gamma(int n, double[] a, double[] y)
        {
            throw new NotImplementedException();
        }

        public void Gamma(int n, double[] a, double[] y, int startIndexA, int startIndexY)
        {
            throw new NotImplementedException();
        }

        public void GammaLn(int n, double[] a, double[] y)
        {
            throw new NotImplementedException();
        }

        public void GammaLn(int n, double[] a, double[] y, int startIndexA, int startIndexY)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}