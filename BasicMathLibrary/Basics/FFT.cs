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
using Microsoft.Extensions.Logging;

using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Logging;
using Dodoni.MathLibrary.Basics.LowLevel;

namespace Dodoni.MathLibrary.Basics
{
    /// <summary>Serves as factory for Fast-Fourier transformations (for mixed radices (not limited to sizes that are power of 2), i.e.
    /// <para>
    /// H_n = a * \sum_{k=0}^{N-1} h_k * exp( -/+ 2 \pi * i * k * n * \alpha),
    /// </para>
    /// where \alpha = 1/N for an ordinary (Fast) Fourier transformation and \alpha arbritrary for a Fractional (Fast) Fourier transformation.
    /// </summary>
    public static class FFT
    {
        #region nested enumerations/interfaces

        /// <summary>Serves as interface for a one-dimensional (Fast) Fourier transformation, i.e.
        /// <para>
        /// H_n = a * \sum_{k=0}^{N-1} h_k * exp( -2 \pi * i * k * n * \alpha),  (forward transformation, FFT: \alpha=1/N; Fractional FFT: \alpha arbritrary)
        /// </para>
        /// where N is the number of the given complex numbers h_0,..., h_N and a is some real scaling factor, or
        /// <para>
        ///  h_n = b * \sum_{k=0}^{N-1} H_k * exp(2 \pi * i * k * n * \alpha), (backward transformation without factor 1/N etc., FFT: \alpha=1/N; Fractional FFT: \alpha arbritrary)
        /// </para>
        /// where N is the number of the given complex numbers H_0,..., H_N and b is some real scaling factor.
        /// </summary>
        /// <remarks>In the case of a backward Fast Fourier Transformation one has to set b = \alpha to get the correct inverse Fourier coefficients (with a = 1).</remarks>
        public interface IOneDimensional : IOperable, IDisposable
        {
            /// <summary>Gets the length, i.e. the number of (complex) Fourier coefficients.
            /// </summary>
            /// <value>The length, i.e. the number of Fourier coefficients.</value>
            int Length
            {
                get;
            }

            /// <summary>Gets the scaling factor \alpha, i.e. 1/<see cref="Length"/> in the case of the FFT and an arbritrary value in the case of a Fractional FFT.
            /// </summary>
            /// <value>The scaling factor \alpha.</value>
            Complex Alpha
            {
                get;
            }

            /// <summary>Gets a value indicating the restriction of the parameter \alpha in the Fourier transformation.
            /// </summary>
            /// <value>The restriction of the parameter \alpha in the Fourier transformation.</value>
            FFT.FourierExponentialFactorRestriction FourierExponentialFactorRestriction
            {
                get;
            }

            /// <summary>Sets the scaling factor \alpha in the Fourier Transformation.
            /// </summary>
            /// <param name="alpha">The scaling factor \alpha.</param>
            /// <exception cref="InvalidOperationException">Thrown if <see cref="FFT.IOneDimensional.FourierExponentialFactorRestriction"/> indicates that the parameter \alpha can not be changed in this way or <paramref name="alpha"/> contains an invalid value.</exception>
            void SetParameterAlpha(Complex alpha);

            /// <summary>Sets the scaling factor \alpha in the Fourier Transformation.
            /// </summary>
            /// <param name="alpha">The scaling factor \alpha.</param>
            /// <exception cref="InvalidOperationException">Thrown if <see cref="FFT.IOneDimensional.FourierExponentialFactorRestriction"/> indicates that the parameter \alpha can not be changed in this way or <paramref name="alpha"/> contains an invalid value.</exception>
            void SetParameterAlpha(double alpha);

            /// <summary>Computes the forward Fourier transformation.
            /// </summary>
            /// <param name="fourierCoefficients">The input as well as the output [in place] with at least <see cref="FFT.IOneDimensional.Length"/> elements.</param>
            /// <param name="scalingFactor">The scaling factor with respect to the input domain.</param>
            void ForwardTransformation(Complex[] fourierCoefficients, double scalingFactor);

            /// <summary>Computes the forward Fourier transformation.
            /// </summary>
            /// <param name="fourierCoefficients">The input as well as the output [in place] with at least <see cref="FFT.IOneDimensional.Length"/> elements.</param>
            /// <remarks>The scaling factor is assumed to be <c>1.0</c>.</remarks>
            void ForwardTransformation(Complex[] fourierCoefficients);

            /// <summary>Computes the forward Fourier transformation.
            /// </summary>
            /// <param name="inputFourierCoefficients">The input Fourier coefficients with at least <see cref="FFT.IOneDimensional.Length"/> elements.</param>
            /// <param name="outputFourierCoefficients">The output Fourier coefficients, i.e. out-of-place calculation with at least <see cref="FFT.IOneDimensional.Length"/> elements (output).</param>
            /// <param name="scalingFactor">The scaling factor with respect to the input domain.</param>
            void ForwardTransformation(Complex[] inputFourierCoefficients, Complex[] outputFourierCoefficients, double scalingFactor);

            /// <summary>Computes the forward Fourier transformation.
            /// </summary>
            /// <param name="inputFourierCoefficients">The input Fourier coefficients with at least <see cref="FFT.IOneDimensional.Length"/> elements.</param>
            /// <param name="outputFourierCoefficients">The output Fourier coefficients, i.e. out-of-place calculation with at 
            /// least <see cref="FFT.IOneDimensional.Length"/> elements (output).</param>
            /// <remarks>The scaling factor is assumed to be <c>1.0</c>.</remarks>
            void ForwardTransformation(Complex[] inputFourierCoefficients, Complex[] outputFourierCoefficients);

            /// <summary>Computes the backward Fourier transformation.
            /// </summary>
            /// <param name="fourierCoefficients">The input as well as the output [in place] with at least <see cref="FFT.IOneDimensional.Length"/> elements.</param>
            /// <param name="scalingFactor">The scaling factor with respect to the input domain.</param>
            void BackwardTransformation(Complex[] fourierCoefficients, double scalingFactor);

            /// <summary>Computes the backward Fourier transformation.
            /// </summary>
            /// <param name="fourierCoefficients">The input as well as the output [in place] with at least <see cref="FFT.IOneDimensional.Length"/> elements.</param>
            /// <remarks>The scaling factor is assumed to be <c>1.0</c>.</remarks>
            void BackwardTransformation(Complex[] fourierCoefficients);

            /// <summary>Computes the backward Fourier transformation.
            /// </summary>
            /// <param name="inputFourierCoefficients">The input Fourier coefficients with at least <see cref="FFT.IOneDimensional.Length"/> elements.</param>
            /// <param name="outputFourierCoefficients">The output Fourier coefficients, i.e. out-of-place calculation with at 
            /// least <see cref="FFT.IOneDimensional.Length"/> elements (output).</param>
            /// <param name="scalingFactor">The scaling factor with respect to the input domain.</param>
            void BackwardTransformation(Complex[] inputFourierCoefficients, Complex[] outputFourierCoefficients, double scalingFactor);

            /// <summary>Computes the backward Fourier transformation.
            /// </summary>
            /// <param name="inputFourierCoefficients">The input Fourier coefficients with at least <see cref="FFT.IOneDimensional.Length"/> elements.</param>
            /// <param name="outputFourierCoefficients">The output Fourier coefficients, i.e. out-of-place calculation with at 
            /// least <see cref="FFT.IOneDimensional.Length"/> elements (output).</param>
            /// <remarks>The scaling factor is assumed to be <c>1.0</c>.</remarks>
            void BackwardTransformation(Complex[] inputFourierCoefficients, Complex[] outputFourierCoefficients);
        }

        /// <summary>Serves as interface for a one-dimensional (Fast) Fourier transformation, i.e.
        /// <para>
        ///   H_n = a * \sum_{k=0}^{N-1} h_k * exp( -2 \pi * i * k * n * \alpha),  (forward transformation, FFT: \alpha=1/N; Fractional FFT: \alpha arbritrary)
        /// </para>
        /// where N is the number of the specified real numbers h_0,..., h_N and a is some real scaling factor, or
        /// <para>
        ///   h_n = b * \sum_{k=0}^{N-1} H_k * exp(2 \pi * i * k * n * \alpha), (backward transformation without factor 1/N etc., FFT: \alpha=1/N; Fractional FFT: \alpha arbritrary)
        /// </para>
        /// where N is the number of the specified complex numbers H_0,..., H_N with Hermitian symmetry and b is some real scaling factor.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The result of the (real) fourier forward transformation are complex numbers z_j with the property z_j = z_{n-j}*, where z* 
        /// denotes the complex conjugate of z, the so called Hermitian symmetry. Moreover Im(z_0) = 0.
        /// </para>
        /// The CCE format stores the values of the first half of the output complex conjugate-even signal
        /// resulted from the forward FFT, i.e. 
        /// <para>
        ///  Re z_0, Im z_0, Re z_1, Im z_1, ..., Re z_m, Im z_m,
        /// </para>
        /// where m = n/2+1. Note that the one-dimensional signal stored in CCE format is one complex element longer than the input of the forward transformation.
        /// The input of the backward transformation is assumed to store in the CCE format.
        /// </remarks>
        public interface IOneDimensionalRealData : IOperable, IDisposable
        {
            /// <summary>Gets the length, i.e. the number of (real) Fourier coefficients.
            /// </summary>
            /// <value>The length, i.e. the number of Fourier coefficients.</value>
            int Length
            {
                get;
            }

            /// <summary>Gets the scaling factor \alpha, i.e. 1/<see cref="Length"/> in the case of the FFT and an arbritrary value in the case of a Fractional FFT.
            /// </summary>
            /// <value>The scaling factor \alpha.</value>
            Complex Alpha
            {
                get;
            }

            /// <summary>Gets a value indicating the restriction of the parameter \alpha in the Fourier transformation.
            /// </summary>
            /// <value>The restriction of the parameter \alpha in the Fourier transformation.</value>
            FFT.FourierExponentialFactorRestriction FourierExponentialFactorRestriction
            {
                get;
            }

            /// <summary>Sets the scaling factor \alpha in the Fourier Transformation.
            /// </summary>
            /// <param name="alpha">The scaling factor \alpha.</param>
            /// <exception cref="InvalidOperationException">Thrown if <see cref="FFT.IOneDimensional.FourierExponentialFactorRestriction"/> indicates that the parameter \alpha can not be changed in this way or <paramref name="alpha"/> contains an invalid value.</exception>
            void SetParameterAlpha(Complex alpha);

            /// <summary>Sets the scaling factor \alpha in the Fourier Transformation.
            /// </summary>
            /// <param name="alpha">The scaling factor \alpha.</param>
            /// <exception cref="InvalidOperationException">Thrown if <see cref="FFT.IOneDimensional.FourierExponentialFactorRestriction"/> indicates that the parameter \alpha can not be changed in this way or <paramref name="alpha"/> contains an invalid value.</exception>
            void SetParameterAlpha(double alpha);

            /// <summary>Computes the forward Fourier transformation.
            /// </summary>
            /// <param name="fourierCoefficients">The input as well as the output [in place] with at least 2* (<see cref="FFT.IOneDimensionalRealData.Length"/>/2 + 1) elements.</param>
            /// <param name="scalingFactor">The scaling factor with respect to the input domain.</param>
            /// <remarks>The output are complex numbers with Hermitian symmetry, stored in the CCE format.</remarks>
            void ForwardTransformation(double[] fourierCoefficients, double scalingFactor);

            /// <summary>Computes the forward Fourier transformation.
            /// </summary>
            /// <param name="fourierCoefficients">The input as well as the output [in place] with at least 2* (<see cref="FFT.IOneDimensionalRealData.Length"/>/2 + 1) elements.</param>
            /// <remarks>The output are complex numbers with Hermitian symmetry, stored in the CCE format.
            /// <para>The scaling factor is assumed to be <c>1.0</c>.</para></remarks>
            void ForwardTransformation(double[] fourierCoefficients);

            /// <summary>Computes the forward Fourier transformation.
            /// </summary>
            /// <param name="inputFourierCoefficients">The input Fourier coefficients with at least <see cref="FFT.IOneDimensionalRealData.Length"/> elements.</param>
            /// <param name="outputFourierCoefficients">The output Fourier coefficients, i.e. out-of-place calculation with at least 2* (<see cref="FFT.IOneDimensionalRealData.Length"/> / 2 + 1) elements (output).</param>
            /// <param name="scalingFactor">The scaling factor with respect to the input domain.</param>
            /// <remarks>The output are complex numbers with Hermitian symmetry, stored in the CCE format.</remarks>
            void ForwardTransformation(double[] inputFourierCoefficients, double[] outputFourierCoefficients, double scalingFactor);

            /// <summary>Computes the forward Fourier transformation.
            /// </summary>
            /// <param name="inputFourierCoefficients">The input Fourier coefficients with at least <see cref="FFT.IOneDimensionalRealData.Length"/> elements.</param>
            /// <param name="outputFourierCoefficients">The output Fourier coefficients, i.e. out-of-place calculation with at least 2* (<see cref="FFT.IOneDimensionalRealData.Length"/> / 2 + 1) elements (output).</param>
            /// <remarks>The output are complex numbers with Hermitian symmetry, stored in the CCE format.
            /// <para>The scaling factor is assumed to be <c>1.0</c>.</para></remarks>
            void ForwardTransformation(double[] inputFourierCoefficients, double[] outputFourierCoefficients);

            /// <summary>Computes the backward Fourier transformation.
            /// </summary>
            /// <param name="fourierCoefficients">The input as well as the output [in place] with at least 2* (<see cref="FFT.IOneDimensionalRealData.Length"/> / 2 + 1) elements.</param>
            /// <param name="scalingFactor">The scaling factor with respect to the input domain.</param>
            /// <remarks>The input are complex numbers with Hermitian symmetry, stored in the CCE format, i.e. 2* (<see cref="FFT.IOneDimensionalRealData.Length"/> / 2 + 1) real numbers, the output 
            /// are <see cref="IOneDimensionalRealData.Length"/> real numbers.</remarks>
            void BackwardTransformation(double[] fourierCoefficients, double scalingFactor);

            /// <summary>Computes the backward Fourier transformation.
            /// </summary>
            /// <param name="fourierCoefficients">The input as well as the output [in place] with at least 2* (<see cref="FFT.IOneDimensionalRealData.Length"/> / 2 + 1) elements.</param>
            /// <remarks>The input are complex numbers with Hermitian symmetry, stored in the CCE format, i.e. 2* (<see cref="FFT.IOneDimensionalRealData.Length"/> / 2 + 1) real numbers, the output 
            /// are <see cref="IOneDimensionalRealData.Length"/> real numbers.
            /// <para>The scaling factor is assumed to be <c>1.0</c>.</para></remarks>
            void BackwardTransformation(double[] fourierCoefficients);

            /// <summary>Computes the backward Fourier transformation.
            /// </summary>
            /// <param name="inputFourierCoefficients">The input Fourier coefficients with at least 2* (<see cref="FFT.IOneDimensionalRealData.Length"/> / 2 + 1) elements.</param>
            /// <param name="outputFourierCoefficients">The output Fourier coefficients, i.e. out-of-place calculation with at 
            /// least <see cref="FFT.IOneDimensionalRealData.Length"/> elements (output).</param>
            /// <param name="scalingFactor">The scaling factor with respect to the input domain.</param>
            /// <remarks>The input are complex numbers with Hermitian symmetry, stored in the CCE format, i.e. 2* (<see cref="FFT.IOneDimensionalRealData.Length"/> / 2 + 1) real numbers, the output 
            /// are <see cref="FFT.IOneDimensionalRealData.Length"/> real numbers.</remarks>
            void BackwardTransformation(double[] inputFourierCoefficients, double[] outputFourierCoefficients, double scalingFactor);

            /// <summary>Computes the backward Fourier transformation.
            /// </summary>
            /// <param name="inputFourierCoefficients">The input Fourier coefficients with at least 2* (<see cref="FFT.IOneDimensionalRealData.Length"/> / 2 + 1) elements.</param>
            /// <param name="outputFourierCoefficients">The output Fourier coefficients, i.e. out-of-place calculation with at 
            /// least <see cref="FFT.IOneDimensionalRealData.Length"/> elements (output).</param>
            /// <remarks>The input are complex numbers with Hermitian symmetry, stored in the CCE format, i.e. 2* (<see cref="FFT.IOneDimensionalRealData.Length"/> / 2 + 1) real numbers, the output 
            /// are <see cref="FFT.IOneDimensionalRealData.Length"/> real numbers.
            /// <para>The scaling factor is assumed to be <c>1.0</c>.</para></remarks>
            void BackwardTransformation(double[] inputFourierCoefficients, double[] outputFourierCoefficients);
        }

        /// <summary>Represents the restriction of the parameter \alpha in the (Forward/Backward) transformation, i.e.
        /// <para>
        /// H_n = a * \sum_{k=0}^{N-1} h_k * exp( -/+ 2 \pi * i * k * n * \alpha),
        /// </para>
        /// where N is the number of the Fourier coefficents.
        /// </summary>
        [Flags]
        public enum FourierExponentialFactorRestriction
        {
            /// <summary>A ordinary (Forward/Backward) Fourier transformation is applied with \alpha=1/N, where N is the number of Fourier coefficients.
            /// </summary>
            OneOverLength = 0,

            /// <summary>A fractional (Forward/Backward) Fourier transformation is applied where \alpha can be an arbitrary real number.
            /// </summary>
            RealNumber = 0x1,

            /// <summary>A fractional (Forward/Backward) Fourier transformation is applied where \alpha can be an arbitrary real or complex number.
            /// </summary>
            Arbitrary = 0x3
        }

        /// <summary>Serves as interface for a Fast-Fourier Transformation Library.
        /// </summary>
        public interface ILibrary : IIdentifierNameable, IAnnotatable
        {
            /// <summary>Gets the factory for one dimensional Fast-Fourier Transformation.
            /// </summary>
            /// <value>The factory for one dimensional Fast-Fourier Transformation.</value>
            IOneDimFourierTransformationFactory OneDimensionalFactory
            {
                get;
            }
        }
        #endregion

        #region public static (readonly) members

        /// <summary>Represents the implementation of one-dimensional Fast-Fourier transformations.
        /// </summary>
        public static readonly OneDimFourierTransformationFactory OneDimensional;
        #endregion

        #region static constructor

        /// <summary>Initializes the <see cref="FFT" /> class.
        /// </summary>
        /// <remarks>This constructor takes into account the Managed Extensibility Framework (MEF) with respect to <see cref="LowLevelMathConfiguration"/>.</remarks>
        static FFT()
        {
            ILibrary fft = null;
            try
            {
                fft = LowLevelMathConfiguration.FFT.CreateFromConfigurationFile();
                if (fft == null)
                {
                    fft = LowLevelMathConfiguration.FFT.Libraries.NaiveBuildIn;
                    Logger.Stream.LogError(String.Format(LowLevelMathConfigurationResources.LogFileMessageConfigFileUseDefaultImplementation, "FFT"));
                }
            }
            catch (Exception e)
            {
                /* thrown of Exceptions in static constructors should be avoided: 
                 */
                Logger.Stream.LogError(e, LowLevelMathConfigurationResources.LogFileMessageCorruptConfigFile);

                fft = LowLevelMathConfiguration.FFT.Libraries.NaiveBuildIn;
                Logger.Stream.LogError(LowLevelMathConfigurationResources.LogFileMessageConfigFileUseDefaultImplementation, "FFT");
            }
            OneDimensional = new OneDimFourierTransformationFactory(fft.OneDimensionalFactory);
        }
        #endregion
    }
}