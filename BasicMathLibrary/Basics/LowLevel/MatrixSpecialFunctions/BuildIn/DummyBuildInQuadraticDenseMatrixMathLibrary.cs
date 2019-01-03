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

namespace Dodoni.MathLibrary.Basics.LowLevel
{
    /// <summary>Serves as dummy implementation for <see cref="IQuadraticDenseMatrixSpecialFunctionLibrary"/>, i.e. in any case a <see cref="NotImplementedException"/> will be thrown.
    /// </summary>
    internal class DummyBuildInQuadraticDenseMatrixMathLibrary : IQuadraticDenseMatrixSpecialFunctionLibrary
    {
        #region IQuadraticDenseMatrixMathLibrary Members

        /// <summary>Gets a <see cref="IQuadraticDenseMatrixSpecialFunctionOperation" /> object that servers as calculator for the exponential of specific matrices.
        /// </summary>
        /// <value>A <see cref="IQuadraticDenseMatrixSpecialFunctionOperation" /> object that servers as calculator for the exponential of specific matrices.</value>
        public IQuadraticDenseMatrixSpecialFunctionOperation Exp
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>Gets a <see cref="IQuadraticDenseMatrixSpecialFunctionOperation" /> object that servers as calculator for the logarithm of specific matrices.
        /// </summary>
        /// <value>A <see cref="IQuadraticDenseMatrixSpecialFunctionOperation" /> object that servers as calculator for the logarithm of specific matrices.</value>
        public IQuadraticDenseMatrixSpecialFunctionOperation Log
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>Gets a <see cref="IQuadraticDenseMatrixSpecialFunctionOperation" /> object that servers as calculator for the sinus of specific matrices.
        /// </summary>
        /// <value>A <see cref="IQuadraticDenseMatrixSpecialFunctionOperation" /> object that servers as calculator for the sinus of specific matrices.</value>
        public IQuadraticDenseMatrixSpecialFunctionOperation Sin
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>Gets a <see cref="IQuadraticDenseMatrixSpecialFunctionOperation" /> object that servers as calculator for the cosinus of specific matrices.
        /// </summary>
        /// <value>A <see cref="IQuadraticDenseMatrixSpecialFunctionOperation" /> object that servers as calculator for the cosinus of specific matrices.</value>
        public IQuadraticDenseMatrixSpecialFunctionOperation Cos
        {
            get { throw new NotImplementedException(); }
        }
        #endregion
    }
}