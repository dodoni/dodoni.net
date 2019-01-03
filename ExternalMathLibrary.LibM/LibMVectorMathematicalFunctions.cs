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

using Dodoni.MathLibrary.Basics.LowLevel.BuildIn;

namespace Dodoni.MathLibrary.Basics.LowLevel.Native
{
    /// <summary>
    /// Provides mathematical functions with respect to AMD's LibM.
    /// </summary>
    /// <remarks>
    /// Tested with .
    /// </remarks>
    internal class LibMVectorMathematicalFunctions : IVectorUnitBasics, IVectorUnitSpecial
    {
        #region private function import

        #region Arithmetic functions

        /* not suppored by AMD's LibM */
        #endregion

        #region Power and Root functions

        [DllImport(LibMNativeWrapper.dllName, EntryPoint = "amd_vrda_cbrt", CallingConvention = LibMNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern int _vdSqrt(int n, double[] a, double[] y);

        [DllImport(LibMNativeWrapper.dllName, EntryPoint = "amd_vrda_cbrt", CallingConvention = LibMNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern int _vdSqrt(int n, double* a, double* y);
        #endregion

        #region Exponential and Logarithmic Functions

        [DllImport(LibMNativeWrapper.dllName, EntryPoint = "amd_vrda_exp", CallingConvention = LibMNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern int _vdExp(int n, double[] a, double[] y);

        [DllImport(LibMNativeWrapper.dllName, EntryPoint = "amd_vrda_exp", CallingConvention = LibMNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern int _vdExp(int n, double* a, double* y);

        [DllImport(LibMNativeWrapper.dllName, EntryPoint = "amd_vrda_log", CallingConvention = LibMNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern int _vdLn(int n, double[] a, double[] y);

        [DllImport(LibMNativeWrapper.dllName, EntryPoint = "amd_vrda_log", CallingConvention = LibMNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern int _vdLn(int n, double* a, double* y);

        [DllImport(LibMNativeWrapper.dllName, EntryPoint = "amd_vrda_log10", CallingConvention = LibMNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern int _vdLog10(int n, double[] a, double[] y);

        [DllImport(LibMNativeWrapper.dllName, EntryPoint = "amd_vrda_log10", CallingConvention = LibMNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern int _vdLog10(int n, double* a, double* y);
        #endregion

        #region Trigonometric Functions

        [DllImport(LibMNativeWrapper.dllName, EntryPoint = "amd_vrda_cos", CallingConvention = LibMNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern int _vdCos(int n, double[] a, double[] y);

        [DllImport(LibMNativeWrapper.dllName, EntryPoint = "amd_vrda_cos", CallingConvention = LibMNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern int _vdCos(int n, double* a, double* y);

        [DllImport(LibMNativeWrapper.dllName, EntryPoint = "amd_vrda_sin", CallingConvention = LibMNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern int _vdSin(int n, double[] a, double[] y);

        [DllImport(LibMNativeWrapper.dllName, EntryPoint = "amd_vrda_sin", CallingConvention = LibMNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern int _vdSin(int n, double* a, double* y);

        [DllImport(LibMNativeWrapper.dllName, EntryPoint = "amd_vrda_sincos", CallingConvention = LibMNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern int _vdSinCos(int n, double[] a, double[] y, double[] z);

        [DllImport(LibMNativeWrapper.dllName, EntryPoint = "amd_vrda_sincos", CallingConvention = LibMNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
        private unsafe static extern int _vdSinCos(int n, double* a, double* y, double* z);
        #endregion

        #region special functions

        /* not suppored by AMD's LibM */
        #endregion

        #endregion

        #region private members

        /// <summary>The managed implementation of vector functions not suppored by AMD's LibM (as a fall-back solution).
        /// </summary>
        BuildInVectorUnit m_BuildInVectorUnit;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="LibMVectorMathematicalFunctions"/> class.
        /// </summary>
        internal LibMVectorMathematicalFunctions()
        {
            m_BuildInVectorUnit = new BuildInVectorUnit();
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
            m_BuildInVectorUnit.Basics.Add(n, a, b, y);
        }

        /// <summary>Performs element by element addition of vector <paramref name="a" /> and vector <paramref name="b" />, i.e. a = a + b [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a + b.</param>
        /// <param name="b">The input vector b.</param>
        public void Add(int n, double[] a, double[] b)
        {
            m_BuildInVectorUnit.Basics.Add(n, a, b);
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
            m_BuildInVectorUnit.Basics.Add(n, a, b, y, startIndexA, startIndexB, startIndexY);
        }

        /// <summary>Performs addition of vector <paramref name="a" /> and double precision number <paramref name="b" />.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input double precision number b.</param>
        /// <param name="y">The output vector a+b.</param>
        public void Add(int n, double[] a, double b, double[] y)
        {
            m_BuildInVectorUnit.Basics.Add(n, a, b, y);
        }

        /// <summary>Performs addition of vector <paramref name="a" /> and double precision number <paramref name="b" />, i.e. a_j = a_j + b [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a + b.</param>
        /// <param name="b">The input double precision number b.</param>
        public void Add(int n, double[] a, double b)
        {
            m_BuildInVectorUnit.Basics.Add(n, a, b);
        }

        /// <summary>Performs element by element addition of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector a+b.</param>
        public void Add(int n, Complex[] a, Complex[] b, Complex[] y)
        {
            m_BuildInVectorUnit.Basics.Add(n, a, b, y);
        }

        /// <summary>Performs element by element addition of vector <paramref name="a" /> and vector <paramref name="b" />, i.e. a = a + b [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a + b.</param>
        /// <param name="b">The input vector b.</param>
        public void Add(int n, Complex[] a, Complex[] b)
        {
            m_BuildInVectorUnit.Basics.Add(n, a, b);
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
            m_BuildInVectorUnit.Basics.Add(n, a, b, y, startIndexA, startIndexB, startIndexY);
        }

        /// <summary>Performs element by element subtraction of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector a-b.</param>
        public void Sub(int n, double[] a, double[] b, double[] y)
        {
            m_BuildInVectorUnit.Basics.Sub(n, a, b, y);
        }

        /// <summary>Performs element by element subtraction of vector <paramref name="a" /> and vector <paramref name="b" />, i.e. a = a - b [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a - b.</param>
        /// <param name="b">The input vector b.</param>
        public void Sub(int n, double[] a, double[] b)
        {
            m_BuildInVectorUnit.Basics.Sub(n, a, b);
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
            m_BuildInVectorUnit.Basics.Sub(n, a, b, y, startIndexA, startIndexB, startIndexY);
        }

        /// <summary>Performs element by element subtraction of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector a-b.</param>
        public void Sub(int n, Complex[] a, Complex[] b, Complex[] y)
        {
            m_BuildInVectorUnit.Basics.Sub(n, a, b, y);
        }

        /// <summary>Performs element by element subtraction of vector <paramref name="a" /> and vector <paramref name="b" />, i.e. a = a - b [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a - b.</param>
        /// <param name="b">The input vector b.</param>
        public void Sub(int n, Complex[] a, Complex[] b)
        {
            m_BuildInVectorUnit.Basics.Sub(n, a, b);
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
            m_BuildInVectorUnit.Basics.Sub(n, a, b, y, startIndexA, startIndexB, startIndexY);
        }

        /// <summary>Performs element by element squaring of the vector.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector a[j]^2, j=0,...,<paramref name="n"/>-1.</param>
        public void Sqr(int n, double[] a, double[] y)
        {
            m_BuildInVectorUnit.Basics.Sqr(n, a, y);
        }

        /// <summary>Performs element by element squaring of the vector [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a[j]^2, j=0,...,<paramref name="n" />-1.</param>
        public void Sqr(int n, double[] a)
        {
            m_BuildInVectorUnit.Basics.Sqr(n, a, a);
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
            m_BuildInVectorUnit.Basics.Sqr(n, a, y, startIndexA, startIndexY);
        }

        /// <summary>Performs element by element multiplication of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector a[j]*b[j], j=0,...,<paramref name="n"/>-1.</param>
        public void Mul(int n, double[] a, double[] b, double[] y)
        {
            m_BuildInVectorUnit.Basics.Mul(n, a, b, y);
        }

        /// <summary>Performs element by element multiplication of vector <paramref name="a" /> and vector <paramref name="b" /> [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a[j]*b[j], j=0,...,<paramref name="n" />-1.</param>
        /// <param name="b">The input vector b.</param>
        public void Mul(int n, double[] a, double[] b)
        {
            m_BuildInVectorUnit.Basics.Mul(n, a, b);
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
            m_BuildInVectorUnit.Basics.Mul(n, a, b, y, startIndexA, startIndexB, startIndexY);
        }

        /// <summary>Performs element by element multiplication of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector a[j]*b[j], j=0,...,<paramref name="n"/>-1.</param>
        public void Mul(int n, Complex[] a, Complex[] b, Complex[] y)
        {
            m_BuildInVectorUnit.Basics.Mul(n, a, b, y);
        }

        /// <summary>Performs element by element multiplication of vector <paramref name="a" /> and vector <paramref name="b" /> [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a[j]*b[j], j=0,...,<paramref name="n" />-1.</param>
        /// <param name="b">The input vector b.</param>
        public void Mul(int n, Complex[] a, Complex[] b)
        {
            m_BuildInVectorUnit.Basics.Mul(n, a, b);
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
            m_BuildInVectorUnit.Basics.Mul(n, a, b, y, startIndexA, startIndexB, startIndexY);
        }

        /// <summary>Performs element by element conjugation of vector <paramref name="a"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \conj(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Conjugate(int n, Complex[] a, Complex[] y)
        {
            m_BuildInVectorUnit.Basics.Conjugate(n, a, y);
        }

        /// <summary>Performs element by element conjugation of vector <paramref name="a" /> [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result ot the operation, i.e. \conj(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Conjugate(int n, Complex[] a)
        {
            m_BuildInVectorUnit.Basics.Conjugate(n, a);
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
            m_BuildInVectorUnit.Basics.Conjugate(n, a, y, startIndexA, startIndexY);
        }

        /// <summary>Computes absolute value of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector |a[j]|, j=0,...,<paramref name="n"/>-1.</param>
        public void Abs(int n, double[] a, double[] y)
        {
            m_BuildInVectorUnit.Basics.Abs(n, a, y);
        }

        /// <summary>Computes absolute value of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the resulf ot the operation, i.e. |a[j]|, j=0,...,<paramref name="n" />-1.</param>
        public void Abs(int n, double[] a)
        {
            m_BuildInVectorUnit.Basics.Abs(n, a);
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
            m_BuildInVectorUnit.Basics.Abs(n, a, y, startIndexA, startIndexY);
        }

        /// <summary>Computes absolute value of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector |a[j]|, j=0,...,<paramref name="n"/>-1.</param>
        public void Abs(int n, Complex[] a, double[] y)
        {
            m_BuildInVectorUnit.Basics.Abs(n, a, y);
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
            m_BuildInVectorUnit.Basics.Abs(n, a, y, startIndexA, startIndexY);
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
            m_BuildInVectorUnit.Basics.LinearFraction(n, a, b, y, scaleFactorA, scaleFactorB, shiftA, shiftB);
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
            m_BuildInVectorUnit.Basics.LinearFraction(n, a, b, y, startIndexA, startIndexB, startIndexY, scaleFactorA, scaleFactorB, shiftA, shiftB);
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
            m_BuildInVectorUnit.Basics.Inv(n, a, y);
        }

        /// <summary>Performs element by element inversion of the vector [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector; overwritten by the result of the operation.</param>
        public void Inv(int n, double[] a)
        {
            m_BuildInVectorUnit.Basics.Inv(n, a);
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
            m_BuildInVectorUnit.Basics.Inv(n, a, y, startIndexA, startIndexY);
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
            m_BuildInVectorUnit.Basics.Pow(n, a, b, y);
        }

        /// <summary>Computes a to the power b for elements of two vectors [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a[j]^b[j], j=0,...,<paramref name="n" />-1.</param>
        /// <param name="b">The input vector b.</param>
        public void Pow(int n, double[] a, double[] b)
        {
            m_BuildInVectorUnit.Basics.Pow(n, a, b);
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
            m_BuildInVectorUnit.Basics.Pow(n, a, b, y, startIndexA, startIndexB, startIndexY);
        }

        /// <summary>Computes a to the power b for elements of two vectors.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector a[j]^b[j], j=0,...,<paramref name="n"/>-1.</param>
        public void Pow(int n, Complex[] a, Complex[] b, Complex[] y)
        {
            m_BuildInVectorUnit.Basics.Pow(n, a, b, y);
        }

        /// <summary>Computes a to the power b for elements of two vectors [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a[j]^b[j], j=0,...,<paramref name="n" />-1.</param>
        /// <param name="b">The input vector b.</param>
        public void Pow(int n, Complex[] a, Complex[] b)
        {
            m_BuildInVectorUnit.Basics.Pow(n, a, b, a);
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
            m_BuildInVectorUnit.Basics.Pow(n, a, b, y, startIndexA, startIndexB, startIndexY);
        }

        /// <summary>Raises each element of a vector to the constant power.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The constant power.</param>
        /// <param name="y">The output vector a[j]^b, j=0,...,<paramref name="n"/>-1.</param>
        public void Pow(int n, double[] a, double b, double[] y)
        {
            m_BuildInVectorUnit.Basics.Pow(n, a, b, y);
        }

        /// <summary>Raises each element of a vector to the constant power [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a[j]^b, j=0,...,<paramref name="n" />-1.</param>
        /// <param name="b">The constant power.</param>
        public void Pow(int n, double[] a, double b)
        {
            m_BuildInVectorUnit.Basics.Pow(n, a, b, a);
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
            m_BuildInVectorUnit.Basics.Pow(n, a, b, y, startIndexA, startIndexY);
        }

        /// <summary>Raises each element of a vector to the constant power.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The constant power.</param>
        /// <param name="y">The output vector a[j]^b, j=0,...,<paramref name="n"/>-1.</param>
        public void Pow(int n, Complex[] a, Complex b, Complex[] y)
        {
            m_BuildInVectorUnit.Basics.Pow(n, a, b, y);
        }

        /// <summary>Raises each element of a vector to the constant power [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a[j]^b, j=0,...,<paramref name="n" />-1.</param>
        /// <param name="b">The constant power.</param>
        public void Pow(int n, Complex[] a, Complex b)
        {
            m_BuildInVectorUnit.Basics.Pow(n, a, b, a);
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
            m_BuildInVectorUnit.Basics.Pow(n, a, b, y, startIndexA, startIndexY);
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
            m_BuildInVectorUnit.Basics.Exp(n, a, y);
        }

        /// <summary>Computes an exponential of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \exp(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Exp(int n, Complex[] a)
        {
            m_BuildInVectorUnit.Basics.Exp(n, a, a);
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
            m_BuildInVectorUnit.Basics.Exp(n, a, y, startIndexA, startIndexY);
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
            m_BuildInVectorUnit.Basics.Log(n, a, y);
        }

        /// <summary>Computes natural logarithm of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \log(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Log(int n, Complex[] a)
        {
            m_BuildInVectorUnit.Basics.Log(n, a, a);
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
            m_BuildInVectorUnit.Basics.Log(n, a, y, startIndexA, startIndexY);
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
            m_BuildInVectorUnit.Basics.Log10(n, a, y);
        }

        /// <summary>Computes denary logarithm of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \log_10(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Log10(int n, Complex[] a)
        {
            m_BuildInVectorUnit.Basics.Log10(n, a, a);
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
            m_BuildInVectorUnit.Basics.Log10(n, a, y, startIndexA, startIndexY);
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
            m_BuildInVectorUnit.Basics.Cos(n, a, y);
        }

        /// <summary>Computes cosine of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \cos(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Cos(int n, Complex[] a)
        {
            m_BuildInVectorUnit.Basics.Cos(n, a, a);
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
            m_BuildInVectorUnit.Basics.Cos(n, a, y, startIndexA, startIndexY);
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
            m_BuildInVectorUnit.Basics.Sin(n, a, y);
        }

        /// <summary>Computes sine of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \sin(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Sin(int n, Complex[] a)
        {
            m_BuildInVectorUnit.Basics.Sin(n, a, a);
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
            m_BuildInVectorUnit.Basics.Sin(n, a, y, startIndexA, startIndexY);
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
            m_BuildInVectorUnit.Special.CdfNorm(n, a, y);
        }

        /// <summary>Computes the cumulative normal distribution function values of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. N(a[j]), j=0,...,<paramref name="n" />-1, where N(x) =\int_{-\infty}^x  1/\sqrt(2*PI) * exp(-1/2 *t^2) dt.</param>
        public void CdfNorm(int n, double[] a)
        {
            m_BuildInVectorUnit.Special.CdfNorm(n, a, a);
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
            m_BuildInVectorUnit.Special.CdfNorm(n, a, y, startIndexA, startIndexY);
        }

        /// <summary>Computes the inverse cumulative normal distribution function values of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector N^{-1}(a[j]), j=0,...,<paramref name="n"/>-1, where N(x) =\int_{-\infty}^x  1/\sqrt(2*PI) * exp(-1/2 *t^2) dt.</param>
        public void CdfNormInv(int n, double[] a, double[] y)
        {
            m_BuildInVectorUnit.Special.CdfNormInv(n, a, y);
        }

        /// <summary>Computes the inverse cumulative normal distribution function values of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. N^{-1}(a[j]), j=0,...,<paramref name="n" />-1, where N(x) =\int_{-\infty}^x  1/\sqrt(2*PI) * exp(-1/2 *t^2) dt.</param>
        public void CdfNormInv(int n, double[] a)
        {
            m_BuildInVectorUnit.Special.CdfNormInv(n, a, a);
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
            m_BuildInVectorUnit.Special.CdfNormInv(n, a, y, startIndexA, startIndexY);
        }

        public void Gamma(int n, double[] a, double[] y)
        {
            throw new System.NotImplementedException();
        }

        public void Gamma(int n, double[] a, double[] y, int startIndexA, int startIndexY)
        {
            throw new System.NotImplementedException();
        }

        public void GammaLn(int n, double[] a, double[] y)
        {
            throw new System.NotImplementedException();
        }

        public void GammaLn(int n, double[] a, double[] y, int startIndexA, int startIndexY)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #endregion
    }
}