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

namespace Dodoni.MathLibrary.Basics.LowLevel.Native
{
    public partial class MklBlasNativeWrapper
    {
        /// <summary>Represents the level 1 BLAS methods and some routines which are not BLAS standard but implemented in Intels MKL library.
        /// </summary>
        protected class MklLevel1BLAS : Level1BLAS
        {
            #region private function import

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "IDAMIN", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern int _idamin(ref int n, IntPtr x, ref int incX);

            [DllImport(BlasNativeWrapper.dllName, EntryPoint = "IZAMIN", CallingConvention = BlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern int _izamin(ref int n, IntPtr x, ref int incX);
            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="MklLevel1BLAS"/> class.
            /// </summary>
            internal MklLevel1BLAS()
            {
            }
            #endregion

            #region public methods

            /// <summary>Finds the index of the element with smallest absolute value.
            /// </summary>
            /// <param name="n">The number of elements in vector <paramref name="x"/>.</param>
            /// <param name="x">The vector with at least <paramref name="n"/> elements.</param>
            /// <returns>The position of vector element <paramref name="x"/> that has the smallest absolute value.</returns>
            public override int idamin(int n, double[] x)
            {
                unsafe
                {
                    fixed (double* ptrX = x)
                    {
                        int inc = 1;
                        return _idamin(ref n, (IntPtr)ptrX, ref inc) - 1;  // the result of _idamin is one-based
                    }
                }
            }

            /// <summary>Finds the index of the element with smallest absolute value.
            /// </summary>
            /// <param name="n">The number of elements in vector <paramref name="x"/>.</param>
            /// <param name="x">The vector with at least <paramref name="n"/> elements.</param>
            /// <param name="incX">The increment for <paramref name="x"/>.</param>
            /// <returns>The position of vector element <paramref name="x"/> that has the smallest absolute value.</returns>
            /// <remarks>This method is not part of the BLAS standard.</remarks>
            public override int idamin(int n, double[] x, int incX)
            {
                unsafe
                {
                    fixed (double* ptrX = x)
                    {
                        return _idamin(ref n, (IntPtr)ptrX, ref incX) - 1; // the result of _idamin is one-based
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
            public override int izamin(int n, Complex[] x)
            {
                unsafe
                {
                    fixed (Complex* ptrX = x)
                    {
                        int inc = 1;
                        return _izamin(ref n, (IntPtr)ptrX, ref inc) - 1; // the result of _idamin is one-based
                    }
                }
            }

            /// <summary>Finds the index of the element with smallest absolute value, i.e. |Re(x(i))| + |Im(x(i))|.
            /// </summary>
            /// <param name="n">The number of elements in vector <paramref name="x"/>.</param>
            /// <param name="x">The vector with at least <paramref name="n"/> elements.</param>
            /// <param name="incX">The increment for <paramref name="x"/>.</param>
            /// <returns>The position of vector element <paramref name="x"/> that has the smallest absolute value.
            /// </returns>
            /// <remarks>This method is not part of the BLAS standard.</remarks>
            public override int izamin(int n, Complex[] x, int incX)
            {
                unsafe
                {
                    fixed (Complex* ptrX = x)
                    {
                        return _izamin(ref n, (IntPtr)ptrX, ref incX) - 1; // the result of _idamin is one-based
                    }
                }
            }
            #endregion
        }
    }
}