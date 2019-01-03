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
using System.Security;
using System.Runtime.InteropServices;

namespace Dodoni.MathLibrary.Basics.LowLevel.Native
{
    public partial class MklCBlasNativeWrapper : CBlasNativeWrapper
    {
        /// <summary>Represents the level 1 C-BLAS methods and some routines which are not (C-)BLAS standard but implemented in Intels MKL library.
        /// </summary>
        /// <remarks>This implementation contains wrapper for methods which are not part of the (C-)BLAS standard, but defined in Intels MKL library.</remarks>
        protected class MklLevel1CBLAS : Level1CBLAS
        {
            #region private function import

            [DllImport(CBlasNativeWrapper.dllName, EntryPoint = "cblas_idamin", CallingConvention = CBlasNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            private static extern int _cBlas_idamin(int n, double[] x, int incX);
            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="MklLevel1CBLAS"/> class.
            /// </summary>
            internal MklLevel1CBLAS()
            {
            }
            #endregion

            #region public methods

            /// <summary>Finds the index of the element with smallest absolute value.
            /// </summary>
            /// <param name="n">The number of elements in vector <paramref name="x"/>.</param>
            /// <param name="x">The vector with at least <paramref name="n"/> elements.</param>
            /// <returns>The position of vector element <paramref name="x"/> that has the smallest absolute value.</returns>
            /// <remarks>This method is not part of the BLAS standard.</remarks>
            public override int idamin(int n, double[] x)
            {
                return _cBlas_idamin(n, x, 1);
            }
            #endregion
        }
    }
}