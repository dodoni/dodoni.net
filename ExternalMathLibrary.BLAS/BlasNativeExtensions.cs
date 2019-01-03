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

namespace Dodoni.MathLibrary.Basics.LowLevel.Native
{
    /// <summary>Provides extension methods for BLAS. See http://www.netlib.org/blas for further information.
    /// </summary>
    public static class BlasNativeExtensions
    {
        /// <summary>Gets the <see cref="System.Char"/> representation of a specified <see cref="BLAS.MatrixTransposeState"/>.
        /// </summary>
        /// <param name="matrixTransposeState">The <see cref="BLAS.MatrixTransposeState"/> object.</param>
        /// <returns>The <see cref="System.Char"/> representation of <paramref name="matrixTransposeState"/>.</returns>
        /// <exception cref="System.NotImplementedException">Thrown, if the transformation is not defined.</exception>
        public static char AsBlasTransposeChar(this BLAS.MatrixTransposeState matrixTransposeState)
        {
            switch (matrixTransposeState)
            {
                case BLAS.MatrixTransposeState.NoTranspose: return 'n';
                case BLAS.MatrixTransposeState.Transpose: return 't';
                case BLAS.MatrixTransposeState.Hermite: return 'c';
                default:
                    throw new NotImplementedException(matrixTransposeState.ToString());
            }
        }
    }
}