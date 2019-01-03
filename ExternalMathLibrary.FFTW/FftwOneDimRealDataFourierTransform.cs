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

namespace Dodoni.MathLibrary.Basics.LowLevel.Native
{
    /// <summary>Represents the wrapper for discrete Fourier transformations where the input of the forward transformation are real (rather than complex) numbers with respect to the FFTW library.
    /// </summary>
    internal class FftwOneDimRealDataFourierTransform : FftwOneDimDiscreteFourierTransformation, FFT.IOneDimensionalRealData
    {
        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="FftwOneDimRealDataFourierTransform"/> class.
        /// </summary>
        /// <param name="length">The length.</param>
        internal FftwOneDimRealDataFourierTransform(int length)
            : base(length)
        {
        }
        #endregion

        #region public properties

        #region FFT.IOneDimensionalRealData Members

        /// <summary>Gets the scaling factor \alpha, i.e. 1/<see cref="FFT.IOneDimensionalRealData.Length"/> in the case of the FFT and an arbritrary value in the case of a Fractional FFT.
        /// </summary>
        /// <value>The scaling factor \alpha.</value>
        public Complex Alpha
        {
            get { return 1.0 / Length; }
        }

        /// <summary>Gets a value indicating the restriction of the parameter \alpha in the Fourier transformation.
        /// </summary>
        /// <value>The restriction of the parameter \alpha in the Fourier transformation.</value>
        public FFT.FourierExponentialFactorRestriction FourierExponentialFactorRestriction
        {
            get { return FFT.FourierExponentialFactorRestriction.OneOverLength; }
        }
        #endregion

        #endregion

        #region public methods

        #region FFT.IOneDimensionalRealData Members

        /// <summary>Sets the scaling factor \alpha in the Fourier Transformation.
        /// </summary>
        /// <param name="alpha">The scaling factor \alpha.</param>
        /// <exception cref="InvalidOperationException">Thrown if <see cref="FFT.IOneDimensional.FourierExponentialFactorRestriction"/> indicates that the parameter \alpha can not be changed in this way or <paramref name="alpha"/> contains an invalid value.</exception>
        public void SetParameterAlpha(Complex alpha)
        {
            throw new InvalidOperationException();
        }

        /// <summary>Sets the scaling factor \alpha in the Fourier Transformation.
        /// </summary>
        /// <param name="alpha">The scaling factor \alpha.</param>
        /// <exception cref="InvalidOperationException">Thrown if <see cref="FFT.IOneDimensional.FourierExponentialFactorRestriction"/> indicates that the parameter \alpha can not be changed in this way or <paramref name="alpha"/> contains an invalid value.</exception>
        public void SetParameterAlpha(double alpha)
        {
            throw new InvalidOperationException();
        }

        /// <summary>Compute the forward Fourier transformation.
        /// </summary>
        /// <param name="fourierCoefficients">The input as well as the output [in place] with at least 2* (<see cref="FFT.IOneDimensionalRealData.Length"/>/2 + 2) elements.</param>
        /// <param name="scalingFactor">The scaling factor with respect to the input domain.</param>
        /// <remarks>The output are complex numbers with Hermitian symmetry, stored in the CCE format.</remarks>
        public void ForwardTransformation(double[] fourierCoefficients, double scalingFactor)
        {
            CreateOneDimRealDataForwardPlan(fourierCoefficients, fourierCoefficients);
            ExecudePlan();
            BLAS.Level1.dscal(2 * (Length / 2 + 1), scalingFactor, fourierCoefficients);
        }

        /// <summary>Compute the forward Fourier transformation.
        /// </summary>
        /// <param name="fourierCoefficients">The input as well as the output [in place] with at least 2* (<see cref="FFT.IOneDimensionalRealData.Length"/>/2 + 2) elements.</param>
        /// <remarks>The output are complex numbers with Hermitian symmetry, stored in the CCE format.
        /// <para>The scaling factor is assumed to be <c>1.0</c>.</para></remarks>
        public void ForwardTransformation(double[] fourierCoefficients)
        {
            CreateOneDimRealDataForwardPlan(fourierCoefficients, fourierCoefficients);
            ExecudePlan();
        }

        /// <summary>Compute the forward Fourier transformation.
        /// </summary>
        /// <param name="inputFourierCoefficients">The input Fourier coefficients with at least <see cref="FFT.IOneDimensionalRealData.Length"/> elements.</param>
        /// <param name="outputFourierCoefficients">The output Fourier coefficients, i.e. out-of-place calculation with at
        /// least 2* (<see cref="FFT.IOneDimensionalRealData.Length"/> / 2 + 2) elements (output).</param>
        /// <param name="scalingFactor">The scaling factor with respect to the input domain.</param>
        /// <remarks>The output are complex numbers with Hermitian symmetry, stored in the CCE format.</remarks>
        public void ForwardTransformation(double[] inputFourierCoefficients, double[] outputFourierCoefficients, double scalingFactor)
        {
            CreateOneDimRealDataForwardPlan(inputFourierCoefficients, outputFourierCoefficients);
            ExecudePlan();
            BLAS.Level1.dscal(2 * (Length / 2 + 1), scalingFactor, outputFourierCoefficients);
        }

        /// <summary>Compute the forward Fourier transformation.
        /// </summary>
        /// <param name="inputFourierCoefficients">The input Fourier coefficients with at least <see cref="FFT.IOneDimensionalRealData.Length"/> elements.</param>
        /// <param name="outputFourierCoefficients">The output Fourier coefficients, i.e. out-of-place calculation with at
        /// least 2* (<see cref="FFT.IOneDimensionalRealData.Length"/> / 2 + 2) elements (output).</param>
        /// <remarks>The output are complex numbers with Hermitian symmetry, stored in the CCE format.
        /// <para>The scaling factor is assumed to be <c>1.0</c>.</para></remarks>
        public void ForwardTransformation(double[] inputFourierCoefficients, double[] outputFourierCoefficients)
        {
            CreateOneDimRealDataForwardPlan(inputFourierCoefficients, outputFourierCoefficients);
            ExecudePlan();
        }

        /// <summary>Compute the backward Fourier transformation.
        /// </summary>
        /// <param name="fourierCoefficients">The input as well as the output [in place] with at least 2* (<see cref="FFT.IOneDimensionalRealData.Length"/> / 2 + 2) elements.</param>
        /// <param name="scalingFactor">The scaling factor with respect to the input domain.</param>
        /// <remarks>The input are complex numbers with Hermitian symmetry, stored in the CCE format, i.e.
        /// 2* (<see cref="FFT.IOneDimensionalRealData.Length"/> / 2 + 2) real numbers, the output
        /// are <see cref="FFT.IOneDimensionalRealData.Length"/> real numbers.</remarks>
        public void BackwardTransformation(double[] fourierCoefficients, double scalingFactor)
        {
            CreateOneDimRealDataBackwardPlan(fourierCoefficients, fourierCoefficients);
            ExecudePlan();
            BLAS.Level1.dscal(Length, scalingFactor, fourierCoefficients);
        }

        /// <summary>Compute the backward Fourier transformation.
        /// </summary>
        /// <param name="fourierCoefficients">The input as well as the output [in place] with at least 2* (<see cref="FFT.IOneDimensionalRealData.Length"/> / 2 + 2) elements.</param>
        /// <remarks>The input are complex numbers with Hermitian symmetry, stored in the CCE format, i.e.
        /// 2* (<see cref="FFT.IOneDimensionalRealData.Length"/> / 2 + 2) real numbers, the output
        /// are <see cref="FFT.IOneDimensionalRealData.Length"/> real numbers.
        /// <para>The scaling factor is assumed to be <c>1.0</c>.</para></remarks>
        public void BackwardTransformation(double[] fourierCoefficients)
        {
            CreateOneDimRealDataBackwardPlan(fourierCoefficients, fourierCoefficients);
            ExecudePlan();
        }

        /// <summary>Compute the backward Fourier transformation.
        /// </summary>
        /// <param name="inputFourierCoefficients">The input Fourier coefficients with at least 2* (<see cref="FFT.IOneDimensionalRealData.Length"/> / 2 + 2) elements.</param>
        /// <param name="outputFourierCoefficients">The output Fourier coefficients, i.e. out-of-place calculation with at
        /// least <see cref="FFT.IOneDimensionalRealData.Length"/> elements (output).</param>
        /// <param name="scalingFactor">The scaling factor with respect to the input domain.</param>
        /// <remarks>The input are complex numbers with Hermitian symmetry, stored in the CCE format, i.e.
        /// 2* (<see cref="FFT.IOneDimensionalRealData.Length"/> / 2 + 2) real numbers, the output
        /// are <see cref="FFT.IOneDimensionalRealData.Length"/> real numbers.</remarks>
        public void BackwardTransformation(double[] inputFourierCoefficients, double[] outputFourierCoefficients, double scalingFactor)
        {
            CreateOneDimRealDataBackwardPlan(inputFourierCoefficients, outputFourierCoefficients);
            ExecudePlan();
            BLAS.Level1.dscal(Length, scalingFactor, outputFourierCoefficients);
        }

        /// <summary>Compute the backward Fourier transformation.
        /// </summary>
        /// <param name="inputFourierCoefficients">The input Fourier coefficients with at least 2* (<see cref="FFT.IOneDimensionalRealData.Length"/> / 2 + 2) elements.</param>
        /// <param name="outputFourierCoefficients">The output Fourier coefficients, i.e. out-of-place calculation with at
        /// least <see cref="FFT.IOneDimensionalRealData.Length"/> elements (output).</param>
        /// <remarks>The input are complex numbers with Hermitian symmetry, stored in the CCE format, i.e.
        /// 2* (<see cref="FFT.IOneDimensionalRealData.Length"/> / 2 + 2) real numbers, the output
        /// are <see cref="FFT.IOneDimensionalRealData.Length"/> real numbers.
        /// <para>The scaling factor is assumed to be <c>1.0</c>.</para></remarks>
        public void BackwardTransformation(double[] inputFourierCoefficients, double[] outputFourierCoefficients)
        {
            CreateOneDimRealDataBackwardPlan(inputFourierCoefficients, outputFourierCoefficients);
            ExecudePlan();
        }
        #endregion

        #endregion
    }
}