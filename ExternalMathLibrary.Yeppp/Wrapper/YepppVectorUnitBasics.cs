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

using Dodoni.MathLibrary.Basics.LowLevel.BuildIn;

namespace Dodoni.MathLibrary.Basics.LowLevel.Native.Yeppp
{
    /// <summary>Provides mathematical vector functions with respect to Yeppp! library.
    /// </summary>
    internal class YepppVectorUnitBasics : IVectorUnitBasics
    {
        #region private function import

        #region Arithmetic functions

        [DllImport(YepppNativeWrapper.dllName, EntryPoint = "yepCore_Add_V64fV64f_V64f", ExactSpelling = true, CallingConvention = YepppNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern Status _vdAdd(double[] a, double[] b, double[] y, int n);

        [DllImport(YepppNativeWrapper.dllName, EntryPoint = "yepCore_Add_V64fV64f_V64f", ExactSpelling = true, CallingConvention = YepppNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern Status _vdAdd(double* a, double* b, double* y, int n);

        [DllImport(YepppNativeWrapper.dllName, EntryPoint = "yepCore_Add_V64fS64f_V64f", ExactSpelling = true, CallingConvention = YepppNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern Status _vdAdd(double[] a, double b, double[] y, int n);

        [DllImport(YepppNativeWrapper.dllName, EntryPoint = "yepCore_Add_IV64fS64f_IV64f", ExactSpelling = true, CallingConvention = YepppNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern Status _vdAdd(double[] a, double b, int n);

        [DllImport(YepppNativeWrapper.dllName, EntryPoint = "yepCore_Add_IV64fV64f_IV64f", ExactSpelling = true, CallingConvention = YepppNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static unsafe extern Status _vdAdd([In, Out] double[] x, double[] y, int n);

        [DllImport(YepppNativeWrapper.dllName, EntryPoint = "yepCore_Add_IV64fV64f_IV64f", ExactSpelling = true, CallingConvention = YepppNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static unsafe extern Status _vdAdd(double* x, double* y, int n);

        [DllImport(YepppNativeWrapper.dllName, EntryPoint = "yepCore_Add_V64fV64f_V64f", ExactSpelling = true, CallingConvention = YepppNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern Status _vzAdd(Complex[] a, Complex[] b, [Out] Complex[] y, int n);

        [DllImport(YepppNativeWrapper.dllName, EntryPoint = "yepCore_Add_V64fV64f_V64f", ExactSpelling = true, CallingConvention = YepppNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern Status _vzAdd(Complex* a, Complex* b, Complex* y, int n);

        [DllImport(YepppNativeWrapper.dllName, EntryPoint = "yepCore_Add_IV64fV64f_IV64f", ExactSpelling = true, CallingConvention = YepppNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static unsafe extern Status _vzAdd([In, Out] Complex[] x, Complex[] y, int n);

        [DllImport(YepppNativeWrapper.dllName, EntryPoint = "yepCore_Subtract_V64fV64f_V64f", CallingConvention = YepppNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern Status _vdSub(double[] a, double[] b, double[] y, int n);

        [DllImport(YepppNativeWrapper.dllName, EntryPoint = "yepCore_Subtract_V64fV64f_V64f", CallingConvention = YepppNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern Status _vdSub(double* a, double* b, double* y, int n);

        [DllImport(YepppNativeWrapper.dllName, EntryPoint = "yepCore_Subtract_IV64fV64f_IV64f", ExactSpelling = true, CallingConvention = YepppNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static unsafe extern Status _vdSub([In, Out] double[] x, double[] y, int n);

        [DllImport(YepppNativeWrapper.dllName, EntryPoint = "yepCore_Subtract_IV64fV64f_IV64f", ExactSpelling = true, CallingConvention = YepppNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static unsafe extern Status _vdSub(double* x, double* y, int n);

        [DllImport(YepppNativeWrapper.dllName, EntryPoint = "yepCore_Subtract_V64fV64f_V64f", CallingConvention = YepppNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern Status _vzSub(Complex[] a, Complex[] b, [Out] Complex[] y, int n);

        [DllImport(YepppNativeWrapper.dllName, EntryPoint = "yepCore_Subtract_V64fV64f_V64f", CallingConvention = YepppNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern Status _vzSub(Complex* a, Complex* b, Complex* y, int n);

        [DllImport(YepppNativeWrapper.dllName, EntryPoint = "yepCore_Subtract_IV64fV64f_IV64f", ExactSpelling = true, CallingConvention = YepppNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static unsafe extern Status _vzSub([In, Out] Complex[] x, Complex[] y, int n);

        [DllImport(YepppNativeWrapper.dllName, EntryPoint = "yepCore_Multiply_V64fV64f_V64f", CallingConvention = YepppNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern Status _vdMul(double[] a, double[] b, double[] y, int n);

        [DllImport(YepppNativeWrapper.dllName, EntryPoint = "yepCore_Multiply_V64fV64f_V64f", CallingConvention = YepppNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern Status _vdMul(double* a, double* b, double* y, int n);

        [DllImport(YepppNativeWrapper.dllName, EntryPoint = "yepCore_Multiply_IV64fV64f_IV64f", CallingConvention = YepppNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern Status _vdMul(double[] a, double[] b, int n);

        [DllImport(YepppNativeWrapper.dllName, EntryPoint = "yepCore_Multiply_IV64fV64f_IV64f", CallingConvention = YepppNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern Status _vdMul(double* a, double* b, int n);

        // the multiplication of complex numbers is not supported in Yeppp! yet, because (a+ib) * (c+id) = a*c-b*d + i*(a*d + b*c) requires some increment argument in yepCore_Multiply_V64fV64f_V64f
        //[DllImport(YepppNativeWrapper.dllName, EntryPoint = "yepCore_Multiply_V64fV64f_V64f", CallingConvention = YepppNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        //private static extern Status _vzMul(Complex[] a, Complex[] b, [In, Out] Complex[] y, int n);  

        //[DllImport(YepppNativeWrapper.dllName, EntryPoint = "yepCore_Multiply_V64fV64f_V64f", CallingConvention = YepppNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        //private unsafe static extern Status _vzMul(Complex* a, Complex* b, Complex* y, int n);

        //[DllImport(YepppNativeWrapper.dllName, EntryPoint = "yepCore_Multiply_IV64fV64f_IV64f", CallingConvention = YepppNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        //private static extern Status _vzMul([In, Out] Complex[] a, Complex[] b, int n);

        //[DllImport(YepppNativeWrapper.dllName, EntryPoint = "yepCore_Multiply_IV64fV64f_IV64f", CallingConvention = YepppNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        //private unsafe static extern Status _vzMul(Complex* a, Complex* b, int n);

        [DllImport(YepppNativeWrapper.dllName, EntryPoint = "yepCore_Multiply_IV64fS64f_IV64f", CallingConvention = YepppNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern Status _vdMul(double[] a, double b, int n);

        [DllImport(YepppNativeWrapper.dllName, EntryPoint = "yepCore_Multiply_IV64fS64f_IV64f", CallingConvention = YepppNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern Status _vdMul(double* a, double b, int n);

        [DllImport(YepppNativeWrapper.dllName, EntryPoint = "yepCore_Multiply_IV64fS64f_IV64f", CallingConvention = YepppNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern Status _vzMul([In, Out] Complex[] a, double b, int n);

        [DllImport(YepppNativeWrapper.dllName, EntryPoint = "yepCore_Multiply_IV64fS64f_IV64f", CallingConvention = YepppNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern Status _vzMul(Complex* a, double b, int n);
        #endregion

        #region Power and Root functions

        // not directly supported by Yeppp!
        #endregion

        #region Exponential and Logarithmic Functions

        [DllImport(YepppNativeWrapper.dllName, EntryPoint = "yepMath_Exp_V64f_V64f", CallingConvention = YepppNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern Status _vdExp(double[] a, double[] y, int n);

        [DllImport(YepppNativeWrapper.dllName, EntryPoint = "yepMath_Exp_V64f_V64f", CallingConvention = YepppNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern Status _vdExp(double* a, double* y, int n);

        [DllImport(YepppNativeWrapper.dllName, EntryPoint = "yepMath_Log_V64f_V64f", CallingConvention = YepppNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern Status _vdLn(double[] a, double[] y, int n);

        [DllImport(YepppNativeWrapper.dllName, EntryPoint = "yepMath_Log_V64f_V64f", CallingConvention = YepppNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern Status _vdLn(double* a, double* y, int n);
        #endregion

        #region Trigonometric Functions

        [DllImport(YepppNativeWrapper.dllName, EntryPoint = "yepMath_Cos_V64f_V64f", CallingConvention = YepppNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern Status _vdCos(double[] a, double[] y, int n);

        [DllImport(YepppNativeWrapper.dllName, EntryPoint = "yepMath_Cos_V64f_V64f", CallingConvention = YepppNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern Status _vdCos(double* a, double* y, int n);

        [DllImport(YepppNativeWrapper.dllName, EntryPoint = "yepMath_Sin_V64f_V64f", CallingConvention = YepppNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern Status _vdSin(double[] a, double[] y, int n);

        [DllImport(YepppNativeWrapper.dllName, EntryPoint = "yepMath_Sin_V64f_V64f", CallingConvention = YepppNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern Status _vdSin(double* a, double* y, int n);
        #endregion

        #endregion

        #region private members

        /// <summary>The managed implementation of vector functions not suppored by Yeppp! (as a fall-back solution).
        /// </summary>
        private BuildInVectorUnitBasics m_BuildInBasics;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="YepppVectorUnitBasics" /> class.
        /// </summary>
        internal YepppVectorUnitBasics()
        {
            m_BuildInBasics = new BuildInVectorUnitBasics();
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
            CheckReturnValue(_vdAdd(a, b, y, n));
        }

        /// <summary>Performs element by element addition of vector <paramref name="a" /> and vector <paramref name="b" />, i.e. a = a + b [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a + b.</param>
        /// <param name="b">The input vector b.</param>
        public void Add(int n, double[] a, double[] b)
        {
            CheckReturnValue(_vdAdd(a, b, n));
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
                    CheckReturnValue(_vdAdd(ptrA, ptrB, ptrY, n));
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
            CheckReturnValue(_vdAdd(a, b, y, n));
        }

        /// <summary>Performs addition of vector <paramref name="a" /> and double precision number <paramref name="b" />, i.e. a_j = a_j + b [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a + b.</param>
        /// <param name="b">The input double precision number b.</param>
        public void Add(int n, double[] a, double b)
        {
           CheckReturnValue( _vdAdd(a, b, n));
        }

        /// <summary>Performs element by element addition of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector a+b.</param>
        public void Add(int n, Complex[] a, Complex[] b, Complex[] y)
        {
            CheckReturnValue(_vzAdd(a, b, y, 2 * n));
        }

        /// <summary>Performs element by element addition of vector <paramref name="a" /> and vector <paramref name="b" />, i.e. a = a + b [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a + b.</param>
        /// <param name="b">The input vector b.</param>
        public void Add(int n, Complex[] a, Complex[] b)
        {
            CheckReturnValue(_vzAdd(a, b, 2 * n));
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
                    CheckReturnValue(_vzAdd(ptrA, ptrB, ptrY, 2 * n));
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
            CheckReturnValue(_vdSub(a, b, y, n));
        }

        /// <summary>Performs element by element subtraction of vector <paramref name="a" /> and vector <paramref name="b" />, i.e. a = a - b [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a - b.</param>
        /// <param name="b">The input vector b.</param>
        public void Sub(int n, double[] a, double[] b)
        {
            CheckReturnValue(_vdSub(a, b, n));
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
                    CheckReturnValue(_vdSub(ptrA, ptrB, ptrY, n));
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
            CheckReturnValue(_vzSub(a, b, y, 2 * n));
        }

        /// <summary>Performs element by element subtraction of vector <paramref name="a" /> and vector <paramref name="b" />, i.e. a = a - b [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a - b.</param>
        /// <param name="b">The input vector b.</param>
        public void Sub(int n, Complex[] a, Complex[] b)
        {
            CheckReturnValue(_vzSub(a, b, 2 * n));
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
                    CheckReturnValue(_vzSub(ptrA, ptrB, ptrY, 2 * n));
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
            m_BuildInBasics.Sqr(n, a, y);
        }

        /// <summary>Performs element by element squaring of the vector [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a[j]^2, j=0,...,<paramref name="n" />-1.</param>
        public void Sqr(int n, double[] a)
        {
            m_BuildInBasics.Sqr(n, a);
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
            m_BuildInBasics.Sqr(n, a, y, startIndexA, startIndexY);
        }

        /// <summary>Performs element by element multiplication of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector a[j]*b[j], j=0,...,<paramref name="n"/>-1.</param>
        public void Mul(int n, double[] a, double[] b, double[] y)
        {
            CheckReturnValue(_vdMul(a, b, y, n));
        }

        /// <summary>Performs element by element multiplication of vector <paramref name="a" /> and vector <paramref name="b" /> [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a[j]*b[j], j=0,...,<paramref name="n" />-1.</param>
        /// <param name="b">The input vector b.</param>
        public void Mul(int n, double[] a, double[] b)
        {
            CheckReturnValue(_vdMul(a, b, n));
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
                    CheckReturnValue(_vdMul(ptrA, ptrB, ptrY, n));
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
            m_BuildInBasics.Mul(n, a, b, y);
        }

        /// <summary>Performs element by element multiplication of vector <paramref name="a" /> and vector <paramref name="b" /> [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a[j]*b[j], j=0,...,<paramref name="n" />-1.</param>
        /// <param name="b">The input vector b.</param>
        public void Mul(int n, Complex[] a, Complex[] b)
        {
            m_BuildInBasics.Mul(n, a, b);
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
            m_BuildInBasics.Mul(n, a, b, y, startIndexA, startIndexB, startIndexY);
        }

        /// <summary>Performs element by element conjugation of vector <paramref name="a"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \conj(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Conjugate(int n, Complex[] a, Complex[] y)
        {
            m_BuildInBasics.Conjugate(n, a, y);
        }

        /// <summary>Performs element by element conjugation of vector <paramref name="a" /> [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result ot the operation, i.e. \conj(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Conjugate(int n, Complex[] a)
        {
            m_BuildInBasics.Conjugate(n, a);
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
            m_BuildInBasics.Conjugate(n, a, y, startIndexA, startIndexY);
        }

        /// <summary>Computes absolute value of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector |a[j]|, j=0,...,<paramref name="n"/>-1.</param>
        public void Abs(int n, double[] a, double[] y)
        {
            m_BuildInBasics.Abs(n, a, y);
        }

        /// <summary>Computes absolute value of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the resulf ot the operation, i.e. |a[j]|, j=0,...,<paramref name="n" />-1.</param>
        public void Abs(int n, double[] a)
        {
            m_BuildInBasics.Abs(n, a);
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
            m_BuildInBasics.Abs(n, a, y, startIndexA, startIndexY);
        }

        /// <summary>Computes absolute value of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector |a[j]|, j=0,...,<paramref name="n"/>-1.</param>
        public void Abs(int n, Complex[] a, double[] y)
        {
            m_BuildInBasics.Abs(n, a, y);
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
            m_BuildInBasics.Abs(n, a, y, startIndexA, startIndexY);
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
            m_BuildInBasics.LinearFraction(n, a, b, y, scaleFactorA, scaleFactorB, shiftA, shiftB);
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
            m_BuildInBasics.LinearFraction(n, a, b, y, startIndexA, startIndexB, startIndexY, scaleFactorA, scaleFactorB, shiftA, shiftB);
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
            m_BuildInBasics.Inv(n, a, y);
        }

        /// <summary>Performs element by element inversion of the vector [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector; overwritten by the result of the operation.</param>
        public void Inv(int n, double[] a)
        {
            m_BuildInBasics.Inv(n, a);
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
            m_BuildInBasics.Inv(n, a, y, startIndexA, startIndexY);
        }

        /// <summary>Performs element by element square root calculation of the vector.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \sqrt(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Sqrt(int n, double[] a, double[] y)
        {
            m_BuildInBasics.Sqrt(n, a, y);
        }

        /// <summary>Performs element by element square root calculation of the vector [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \sqrt(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Sqrt(int n, double[] a)
        {
            m_BuildInBasics.Sqrt(n, a);
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
            m_BuildInBasics.Sqrt(n, a, y, startIndexA, startIndexY);
        }

        /// <summary>Computes a to the power b for elements of two vectors.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector a[j]^b[j], j=0,...,<paramref name="n"/>-1.</param>
        public void Pow(int n, double[] a, double[] b, double[] y)
        {
            Log(n, a, y);  // y_j = ln(a_j)
            Mul(n, y, b);  // y_j = b_j * ln(a_j)
            Exp(n, y);     // y_j = exp(b_j * ln(a_j)) = a_j^b_j
        }

        /// <summary>Computes a to the power b for elements of two vectors [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a[j]^b[j], j=0,...,<paramref name="n" />-1.</param>
        /// <param name="b">The input vector b.</param>
        public void Pow(int n, double[] a, double[] b)
        {
            Log(n, a); // a_j = log(a_j)
            Mul(n, a, b); // a_j = b_j * log(a_j)
            Exp(n, a); // a_j = exp(b_j * log(a_j)) = a_j^b_j            
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
                    CheckReturnValue(_vdLn(ptrA, ptrY, n));
                    CheckReturnValue(_vdMul(ptrY, ptrB, n));
                    CheckReturnValue(_vdExp(ptrY, ptrY, n));
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
            m_BuildInBasics.Pow(n, a, b, y);
        }

        /// <summary>Computes a to the power b for elements of two vectors [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a[j]^b[j], j=0,...,<paramref name="n" />-1.</param>
        /// <param name="b">The input vector b.</param>
        public void Pow(int n, Complex[] a, Complex[] b)
        {
            m_BuildInBasics.Pow(n, a, b);
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
            m_BuildInBasics.Pow(n, a, b, y, startIndexA, startIndexB, startIndexY);
        }

        /// <summary>Raises each element of a vector to the constant power.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The constant power.</param>
        /// <param name="y">The output vector a[j]^b, j=0,...,<paramref name="n"/>-1.</param>
        public void Pow(int n, double[] a, double b, double[] y)
        {
            Log(n, a, y);  // y_j = ln(a_j)
            CheckReturnValue(_vdMul(y, b, n));  // y_j = b * ln(a_j)
            Exp(n, y);     // y_j = exp(b * ln(a_j)) = a_j^b_j
        }

        /// <summary>Raises each element of a vector to the constant power [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a[j]^b, j=0,...,<paramref name="n" />-1.</param>
        /// <param name="b">The constant power.</param>
        public void Pow(int n, double[] a, double b)
        {
            Log(n, a); // a_j = log(a_j)
            CheckReturnValue(_vdMul(a, b, n)); // a_j = b * log(a_j)
            Exp(n, a); // a_j = exp(b * log(a_j)) = a_j^b            
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
                    CheckReturnValue(_vdLn(ptrA, ptrY, n));
                    CheckReturnValue(_vdMul(ptrY, b, n));
                    CheckReturnValue(_vdExp(ptrY, ptrY, n));
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
            m_BuildInBasics.Pow(n, a, b, y);
        }

        /// <summary>Raises each element of a vector to the constant power [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a[j]^b, j=0,...,<paramref name="n" />-1.</param>
        /// <param name="b">The constant power.</param>
        public void Pow(int n, Complex[] a, Complex b)
        {
            m_BuildInBasics.Pow(n, a, b);
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
            m_BuildInBasics.Pow(n, a, b, y, startIndexA, startIndexY);
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
            CheckReturnValue(_vdExp(a, y, n));
        }

        /// <summary>Computes an exponential of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \exp(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Exp(int n, double[] a)
        {
            CheckReturnValue(_vdExp(a, a, n));
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
                    CheckReturnValue(_vdExp(ptrA, ptrY, n));
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
            m_BuildInBasics.Exp(n, a, y);
        }

        /// <summary>Computes an exponential of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \exp(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Exp(int n, Complex[] a)
        {
            m_BuildInBasics.Exp(n, a);
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
            m_BuildInBasics.Exp(n, a, y, startIndexA, startIndexY);
        }

        /// <summary>Computes natural logarithm of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \log(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Log(int n, double[] a, double[] y)
        {
            CheckReturnValue(_vdLn(a, y, n));
        }

        /// <summary>Computes natural logarithm of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \log(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Log(int n, double[] a)
        {
            CheckReturnValue(_vdLn(a, a, n));
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
                    CheckReturnValue(_vdLn(ptrA, ptrY, n));
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
            m_BuildInBasics.Log(n, a, y);
        }

        /// <summary>Computes natural logarithm of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \log(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Log(int n, Complex[] a)
        {
            m_BuildInBasics.Log(n, a);
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
            m_BuildInBasics.Log(n, a, y, startIndexA, startIndexY);
        }

        /// <summary>Computes denary logarithm of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \log_10(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Log10(int n, double[] a, double[] y)
        {
            // log_10(x) = ln(x) / ln(10)
            Log(n, a, y);
            CheckReturnValue(_vdMul(y, MathConsts.OneOverLn10, n));
        }

        /// <summary>Computes denary logarithm of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \log_10(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Log10(int n, double[] a)
        {
            // log_10(x) = ln(x) / ln(10)
            Log(n, a);
            CheckReturnValue(_vdMul(a, MathConsts.OneOverLn10, n));
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
            // log_10(x) = ln(x) / ln(10)
            Log(n, a, y, startIndexA, startIndexY);
            unsafe
            {
                fixed (double* yPtr = &y[startIndexY])
                {
                    CheckReturnValue(_vdMul(yPtr, MathConsts.OneOverLn10, n));
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
            m_BuildInBasics.Log10(n, a, y);
        }

        /// <summary>Computes denary logarithm of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \log_10(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Log10(int n, Complex[] a)
        {
            m_BuildInBasics.Log10(n, a);
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
            m_BuildInBasics.Log10(n, a, y, startIndexA, startIndexY);
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
            CheckReturnValue(_vdCos(a, y, n));
        }

        /// <summary>Computes cosine of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \cos(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Cos(int n, double[] a)
        {
            CheckReturnValue(_vdCos(a, a, n));
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
                    CheckReturnValue(_vdCos(ptrA, ptrY, n));
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
            m_BuildInBasics.Cos(n, a, y);
        }

        /// <summary>Computes cosine of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \cos(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Cos(int n, Complex[] a)
        {
            m_BuildInBasics.Cos(n, a);
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
            m_BuildInBasics.Cos(n, a, y, startIndexA, startIndexY);
        }

        /// <summary>Computes sine of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \sin(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Sin(int n, double[] a, double[] y)
        {
            CheckReturnValue(_vdSin(a, y, n));
        }

        /// <summary>Computes sine of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \sin(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Sin(int n, double[] a)
        {
            CheckReturnValue(_vdSin(a, a, n));
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
                    CheckReturnValue(_vdSin(ptrA, ptrY, n));
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
            m_BuildInBasics.Sin(n, a, y);
        }

        /// <summary>Computes sine of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \sin(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Sin(int n, Complex[] a)
        {
            m_BuildInBasics.Sin(n, a);
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
            m_BuildInBasics.Sin(n, a, y, startIndexA, startIndexY);
        }

        /// <summary>Computes sine and cosine of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \sin(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        /// <param name="z">The output vector \cos(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void SinCos(int n, double[] a, double[] y, double[] z)
        {
            CheckReturnValue(_vdSin(a, y, n));
            CheckReturnValue(_vdCos(a, z, n));
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
                    CheckReturnValue(_vdSin(ptrA, ptrY, n));
                    CheckReturnValue(_vdCos(ptrA, ptrZ, n));
                }
            }
        }
        #endregion

        #endregion

        #endregion

        #region private methods

        /// <summary>Checks the return value of a specific Yeppp! function.
        /// </summary>
        /// <param name="errorCode">The error code, i.e. return value of the specified Yeppp! function.</param>
        private void CheckReturnValue(Status errorCode)
        {
            if (errorCode != Status.Ok)
            {
                throw YepppNativeWrapper.GetException(errorCode);
            }
        }
        #endregion
    }
}