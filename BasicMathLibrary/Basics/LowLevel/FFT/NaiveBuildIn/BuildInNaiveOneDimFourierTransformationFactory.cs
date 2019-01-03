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
using System.Text;
using System.Numerics;
using System.Collections.Generic;

using Dodoni.MathLibrary.Basics;
using Dodoni.MathLibrary.Basics.LowLevel;

namespace Dodoni.MathLibrary.Basics.LowLevel.BuildIn
{
    /// <summary>Serves as factory for Fourier Transformations based on a naive managed code implementation.
    /// </summary>
    /// <remarks>It is recommended to use this implementation for test purporse only.</remarks>
    internal class BuildInNaiveOneDimFourierTransformationFactory : IOneDimFractionalFourierTransformationFactory
    {
        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="BuildInNaiveOneDimFourierTransformationFactory"/> class.
        /// </summary>
        internal BuildInNaiveOneDimFourierTransformationFactory()
        {
        }
        #endregion

        #region public properties

        #region IOneDimFractionalFourierTransformationFactory Members

        /// <summary>Gets a value indicating the restriction of the parameter \alpha in the Fourier transformation.
        /// </summary>
        /// <value>The restriction of the parameter \alpha in the Fourier transformation.
        /// </value>
        public FFT.FourierExponentialFactorRestriction FourierExponentialFactorRestriction
        {
            get { return FFT.FourierExponentialFactorRestriction.Arbitrary; }
        }
        #endregion

        #endregion

        #region public methods

        #region IOneDimFourierTransformationFactory Members

        /// <summary>Creates a specific one-dimensional Fourier transformation.
        /// </summary>
        /// <param name="length">The length, i.e. the number of complex coefficients.</param>
        /// <returns>A specific one-dimensional Fourier transformation.
        /// </returns>
        public FFT.IOneDimensional Create(int length)
        {
            return new BuildInNaiveOneDimFourierTransformation(length);
        }

        /// <summary>Creates a specific one-dimensional real Fourier transformation.
        /// </summary>
        /// <param name="length">The length, i.e. the number of real (input) coefficients.</param>
        /// <returns>A specific one-dimensional real Fourier transformation.
        /// </returns>
        public FFT.IOneDimensionalRealData CreateRealTransformation(int length)
        {
            return new BuildInNaiveOneDimRealDataFourierTransformation(length);
        }
        #endregion

        #region IOneDimFractionalFourierTransformationFactory Members

        /// <summary>Creates a specific one-dimensional Fractional Fourier transformation.
        /// </summary>
        /// <param name="length">The length, i.e. the number of complex coefficients.</param>
        /// <param name="alpha">The parameter of the fractional Fast Fourier transformation.</param>
        /// <returns>A specific one-dimensional Fourier transformation.
        /// </returns>
        public FFT.IOneDimensional Create(int length, double alpha)
        {
            return new BuildInNaiveOneDimFourierTransformation(length, alpha);
        }

        /// <summary>Creates a specific one-dimensional Fractional Fourier transformation.
        /// </summary>
        /// <param name="length">The length, i.e. the number of complex coefficients.</param>
        /// <param name="alpha">The parameter of the fractional Fast Fourier transformation.</param>
        /// <returns>A specific one-dimensional Fourier transformation.
        /// </returns>
        public FFT.IOneDimensional Create(int length, Complex alpha)
        {
            return new BuildInNaiveOneDimFourierTransformation(length, alpha);
        }

        /// <summary>Creates a specific one-dimensional real Fourier transformation.
        /// </summary>
        /// <param name="length">The length, i.e. the number of real (input) coefficients.</param>
        /// <param name="alpha">The parameter of the fractional Fast Fourier transformation.</param>
        /// <returns>A specific one-dimensional real Fourier transformation.
        /// </returns>
        public FFT.IOneDimensionalRealData CreateRealTransformation(int length, double alpha)
        {
            return new BuildInNaiveOneDimRealDataFourierTransformation(length, alpha);
        }

        /// <summary>Creates a specific one-dimensional real Fourier transformation.
        /// </summary>
        /// <param name="length">The length, i.e. the number of real (input) coefficients.</param>
        /// <param name="alpha">The parameter of the fractional Fast Fourier transformation.</param>
        /// <returns>A specific one-dimensional real Fourier transformation.
        /// </returns>
        public FFT.IOneDimensionalRealData CreateRealTransformation(int length, Complex alpha)
        {
            return new BuildInNaiveOneDimRealDataFourierTransformation(length, alpha);
        }
        #endregion

        /// <summary>Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "Build-in Fourier Transformation";
        }
        #endregion
    }
}