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

namespace Dodoni.MathLibrary.Basics.LowLevel
{
    /// <summary>Serves as factory for 1-dimensional Fractional (Fast) Fourier transformations.
    /// </summary>
    public interface IOneDimFractionalFourierTransformationFactory : IOneDimFourierTransformationFactory
    {
        /// <summary>Gets a value indicating the restriction of the parameter \alpha in the Fourier transformation.
        /// </summary>
        /// <value>The restriction of the parameter \alpha in the Fourier transformation.</value>
        FFT.FourierExponentialFactorRestriction FourierExponentialFactorRestriction
        {
            get;
        }

        /// <summary>Creates a specific one-dimensional Fractional Fourier transformation.
        /// </summary>
        /// <param name="length">The length, i.e. the number of complex coefficients.</param>
        /// <param name="alpha">The parameter of the fractional Fast Fourier transformation.</param>
        /// <returns>A specific one-dimensional Fourier transformation.</returns>
        FFT.IOneDimensional Create(int length, double alpha);

        /// <summary>Creates a specific one-dimensional Fractional Fourier transformation.
        /// </summary>
        /// <param name="length">The length, i.e. the number of complex coefficients.</param>
        /// <param name="alpha">The parameter of the fractional Fast Fourier transformation.</param>
        /// <returns>A specific one-dimensional Fourier transformation.</returns>
        FFT.IOneDimensional Create(int length, Complex alpha);

        /// <summary>Creates a specific one-dimensional real Fourier transformation.
        /// </summary>
        /// <param name="length">The length, i.e. the number of real (input) coefficients.</param>
        /// <param name="alpha">The parameter of the fractional Fast Fourier transformation.</param>
        /// <returns>A specific one-dimensional real Fourier transformation.</returns>
        FFT.IOneDimensionalRealData CreateRealTransformation(int length, double alpha);

        /// <summary>Creates a specific one-dimensional real Fourier transformation.
        /// </summary>
        /// <param name="length">The length, i.e. the number of real (input) coefficients.</param>
        /// <param name="alpha">The parameter of the fractional Fast Fourier transformation.</param>
        /// <returns>A specific one-dimensional real Fourier transformation.</returns>
        FFT.IOneDimensionalRealData CreateRealTransformation(int length, Complex alpha);
    }
}
