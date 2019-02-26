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
    /// <summary>Serves as naive managed implementation of a one-dimensional real Fourier transformation, i.e.
    /// <para>
    ///   H_n = a * \sum_{k=0}^{N-1} h_k * exp( -2 \pi * i * k * n * \alpha),  (forward transformation, FFT: \alpha=1/N; Fractional FFT: \alpha arbritrary)
    /// </para>
    /// where N is the number of the given real numbers h_0,..., h_N and a is some real scaling factor, or
    /// <para>
    ///   h_n = b * \sum_{k=0}^{N-1} H_k * exp(2 \pi * i * k * n * \alpha), (backward transformation without factor 1/N etc., FFT: \alpha=1/N; Fractional FFT: \alpha arbritrary)
    /// </para>
    /// where N is the number of the given complex numbers H_0,..., H_N with Hermitian symmetry 
    /// and b is some real scaling factor.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The result of the (real) fourier forward transformation are complex numbers z_j with the property
    /// z_j = z_{n-j}*, where z* denotes the complex conjugate of z, the so called Hermitian symmetry. 
    /// Moreover Im(z_0) = 0.
    /// </para>
    /// The CCE format stores the values of the first half of the output complex conjugate-even signal
    /// resulted from the forward FFT, i.e. 
    /// <para>
    ///  Re z_0, Im z_0, Re z_1, Im z_1, ..., Re z_m, Im z_m,
    /// </para>
    /// where m = n/2+1. Note that the one-dimensional signal stored in CCE format is one complex element longer than the input of the forward transformation.
    /// The input of the backward transformation is assumed to given in CCE format.
    /// </remarks>
    internal class BuildInNaiveOneDimRealDataFourierTransformation : FFT.IOneDimensionalRealData
    {
        #region private members

        /// <summary>The number of (complex) Fourier coefficients.
        /// </summary>
        private int m_Length;

        /// <summary>The parameter \alpha of the Fourier Transformation.
        /// </summary>
        private Complex m_Alpha;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="BuildInNaiveOneDimRealDataFourierTransformation"/> class.
        /// </summary>
        /// <param name="length">The length, i.e. the number of (complex) Fourier coefficients.</param>
        internal BuildInNaiveOneDimRealDataFourierTransformation(int length)
        {
            if (length <= 0)
            {
                throw new ArgumentException(nameof(length));
            }
            m_Length = length;
            m_Alpha = 1.0 / length;
        }

        /// <summary>Initializes a new instance of the <see cref="BuildInNaiveOneDimRealDataFourierTransformation"/> class.
        /// </summary>
        /// <param name="length">The length, i.e. the number of (complex) Fourier coefficients.</param>
        /// <param name="alpha">The parameter \alpha of the Fractional Fourier transformation.</param>
        internal BuildInNaiveOneDimRealDataFourierTransformation(int length, Complex alpha)
        {
            if (length <= 0)
            {
                throw new ArgumentException(nameof(length));
            }
            m_Length = length;
            m_Alpha = alpha;
        }
        #endregion

        #region public properties

        #region IOperable Members

        /// <summary>Gets a value indicating whether this instance is operable.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is operable; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// 	<c>false</c> will be returned if the current instance represents some data, model, interpolation procedure,
        /// integration approach, optimization procedure etc. and no valid parameters are available.
        /// </remarks>
        public bool IsOperable
        {
            get { return true; }
        }
        #endregion

        #region FFT.IOneDimensionalRealData Members

        /// <summary>Gets the length, i.e. the number of (complex) Fourier coefficients.
        /// </summary>
        /// <value>The length, i.e. the number of Fourier coefficients.</value>
        public int Length
        {
            get { return m_Length; }
        }

        /// <summary>Gets the scaling factor \alpha, i.e. 1/<see cref="Length"/> in the case of the FFT and an arbritrary value in the case of a Fractional FFT.
        /// </summary>
        /// <value>The scaling factor \alpha.</value>
        public Complex Alpha
        {
            get { return m_Alpha; }
        }

        /// <summary>Gets a value indicating the restriction of the parameter \alpha in the Fourier transformation.
        /// </summary>
        /// <value>The restriction of the parameter \alpha in the Fourier transformation.</value>
        public FFT.FourierExponentialFactorRestriction FourierExponentialFactorRestriction
        {
            get { return FFT.FourierExponentialFactorRestriction.Arbitrary; }
        }
        #endregion

        #endregion

        #region public methods

        #region IDisposable Members

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // nothing to do here
        }
        #endregion

        #region FFT.IOneDimensionalRealData Members

        /// <summary>Sets the scaling factor \alpha in the Fourier Transformation.
        /// </summary>
        /// <param name="alpha">The scaling factor \alpha.</param>
        /// <exception cref="InvalidOperationException">Thrown if <see cref="FFT.IOneDimensional.FourierExponentialFactorRestriction"/> indicates that the parameter \alpha can not be changed in this way or <paramref name="alpha"/> contains an invalid value.</exception>
        public void SetParameterAlpha(Complex alpha)
        {
            if (alpha.IsZero(MachineConsts.Epsilon) == true)
            {
                throw new InvalidOperationException();
            }
            m_Alpha = alpha;
        }

        /// <summary>Sets the scaling factor \alpha in the Fourier Transformation.
        /// </summary>
        /// <param name="alpha">The scaling factor \alpha.</param>
        /// <exception cref="InvalidOperationException">Thrown if <see cref="FFT.IOneDimensional.FourierExponentialFactorRestriction"/> indicates that the parameter \alpha can not be changed in this way or <paramref name="alpha"/> contains an invalid value.</exception>
        public void SetParameterAlpha(double alpha)
        {
            if (Math.Abs(alpha) < MachineConsts.Epsilon)
            {
                throw new InvalidOperationException();
            }
            m_Alpha = alpha;
        }

        /// <summary>Computes the forward Fourier transformation.
        /// </summary>
        /// <param name="fourierCoefficients">The input as well as the output [in place] with at least 2* (<see cref="FFT.IOneDimensionalRealData.Length"/>/2 + 1) elements.</param>
        /// <param name="scalingFactor">The scaling factor with respect to the input domain.</param>
        /// <remarks>The output are complex numbers with Hermitian symmetry, stored in the CCE format.</remarks>
        public void ForwardTransformation(Span<double> fourierCoefficients, double scalingFactor)
        {
            var tempOutput = new double[2 * (m_Length / 2 + 1)];

            ForwardTransformation(fourierCoefficients, tempOutput, scalingFactor);

            for (int j = 0; j < tempOutput.Length; j++)
            {
                fourierCoefficients[j] = tempOutput[j];
            }
        }

        /// <summary>Computes the forward Fourier transformation.
        /// </summary>
        /// <param name="fourierCoefficients">The input as well as the output [in place] with at least 2* (<see cref="FFT.IOneDimensionalRealData.Length"/>/2 + 1) elements.</param>
        /// <remarks>The output are complex numbers with Hermitian symmetry, stored in the CCE format.
        /// <para>The scaling factor is assumed to be <c>1.0</c>.</para></remarks>
        public void ForwardTransformation(Span<double> fourierCoefficients)
        {
            ForwardTransformation(fourierCoefficients, 1.0);
        }

        /// <summary>Computes the forward Fourier transformation.
        /// </summary>
        /// <param name="inputFourierCoefficients">The input Fourier coefficients with at least <see cref="FFT.IOneDimensionalRealData.Length"/> elements.</param>
        /// <param name="outputFourierCoefficients">The output Fourier coefficients, i.e. out-of-place calculation with at least 2* (<see cref="FFT.IOneDimensionalRealData.Length"/> / 2 + 1) elements (output).</param>
        /// <param name="scalingFactor">The scaling factor with respect to the input domain.</param>
        /// <remarks>The output are complex numbers with Hermitian symmetry, stored in the CCE format.</remarks>
        public void ForwardTransformation(ReadOnlySpan<double> inputFourierCoefficients, Span<double> outputFourierCoefficients, double scalingFactor)
        {
            var temp = new Complex[m_Length];

            // \sum_{k=0}^{N-1} h_k * exp( -2 \pi * i * k * n * \alpha)
            for (int j = 0; j < m_Length; j++)
            {
                for (int k = 0; k < m_Length; k++)
                {
                    temp[j] += scalingFactor * inputFourierCoefficients[k] * Complex.Exp(-2 * Math.PI * Complex.ImaginaryOne * k * j * m_Alpha);
                }
            }

            int n = m_Length / 2 + 1;
            int i = 0;
            for (int j = 0; j < n; j++)
            {
                outputFourierCoefficients[i++] = temp[j].Real;
                outputFourierCoefficients[i++] = temp[j].Imaginary;
            }
        }

        /// <summary>Computes the forward Fourier transformation.
        /// </summary>
        /// <param name="inputFourierCoefficients">The input Fourier coefficients with at least <see cref="FFT.IOneDimensionalRealData.Length"/> elements.</param>
        /// <param name="outputFourierCoefficients">The output Fourier coefficients, i.e. out-of-place calculation with at least 2* (<see cref="FFT.IOneDimensionalRealData.Length"/> / 2 + 1) elements (output).</param>
        /// <remarks>The output are complex numbers with Hermitian symmetry, stored in the CCE format.
        /// <para>The scaling factor is assumed to be <c>1.0</c>.</para></remarks>
        public void ForwardTransformation(ReadOnlySpan<double> inputFourierCoefficients, Span<double> outputFourierCoefficients)
        {
            ForwardTransformation(inputFourierCoefficients, outputFourierCoefficients, 1.0);
        }

        /// <summary>Computes the backward Fourier transformation.
        /// </summary>
        /// <param name="fourierCoefficients">The input as well as the output [in place] with at least 2* (<see cref="FFT.IOneDimensionalRealData.Length"/> / 2 + 1) elements.</param>
        /// <param name="scalingFactor">The scaling factor with respect to the input domain.</param>
        /// <remarks>The input are complex numbers with Hermitian symmetry, stored in the CCE format, i.e.
        /// 2* (<see cref="FFT.IOneDimensionalRealData.Length"/> / 2 + 2) real numbers, the output
        /// are <see cref="FFT.IOneDimensionalRealData.Length"/> real numbers.</remarks>
        public void BackwardTransformation(Span<double> fourierCoefficients, double scalingFactor)
        {
            var tempOutput = new double[2 * (m_Length / 2 + 1)];

            BackwardTransformation(fourierCoefficients, tempOutput, scalingFactor);

            for (int j = 0; j < tempOutput.Length; j++)
            {
                fourierCoefficients[j] = tempOutput[j];
            }
        }

        /// <summary>Computes the backward Fourier transformation.
        /// </summary>
        /// <param name="fourierCoefficients">The input as well as the output [in place] with at least 2* (<see cref="FFT.IOneDimensionalRealData.Length"/> / 2 + 1) elements.</param>
        /// <remarks>The input are complex numbers with Hermitian symmetry, stored in the CCE format, i.e. 2* (<see cref="FFT.IOneDimensionalRealData.Length"/> / 2 + 1) real numbers, the output
        /// are <see cref="FFT.IOneDimensionalRealData.Length"/> real numbers. <para>The scaling factor is assumed to be <c>1.0</c>.</para></remarks>
        public void BackwardTransformation(Span<double> fourierCoefficients)
        {
            BackwardTransformation(fourierCoefficients, 1.0);
        }

        /// <summary>Computes the backward Fourier transformation.
        /// </summary>
        /// <param name="inputFourierCoefficients">The input Fourier coefficients with at least 2* (<see cref="FFT.IOneDimensionalRealData.Length"/> / 2 + 1) elements.</param>
        /// <param name="outputFourierCoefficients">The output Fourier coefficients, i.e. out-of-place calculation with at least <see cref="FFT.IOneDimensionalRealData.Length"/> elements (output).</param>
        /// <param name="scalingFactor">The scaling factor with respect to the input domain.</param>
        /// <remarks>The input are complex numbers with Hermitian symmetry, stored in the CCE format, i.e. 2* (<see cref="FFT.IOneDimensionalRealData.Length"/> / 2 + 1) real numbers, the output
        /// are <see cref="FFT.IOneDimensionalRealData.Length"/> real numbers.</remarks>
        public void BackwardTransformation(ReadOnlySpan<double> inputFourierCoefficients, Span<double> outputFourierCoefficients, double scalingFactor)
        {
            /* The input is in CCE format, store the first half and use 
             * 
             * n: odd
             *    z(n/2 + i) = z(n/2 - i)^* for 1<=i<=n/2 
             *    
             * n: even
             *    z(m + i) = z(m - i +1)^* for 1<= i <= m, m=floor(n/2)
             *    
             * for the second half.
             */

            var temp = new Complex[m_Length];

            int i = 0;
            for (int j = 0; j < 2 * (m_Length / 2 + 1); j += 2)
            {
                temp[i++] = new Complex(inputFourierCoefficients[j], inputFourierCoefficients[j + 1]);
            }

            if ((m_Length % 2) == 0)
            {
                for (int j = 1; j <= m_Length / 2 - 1; j++)
                {
                    temp[m_Length / 2 + j] = Complex.Conjugate(temp[m_Length / 2 - j]);
                }
            }
            else
            {
                int m = m_Length / 2;
                for (int j = 1; j <= m; j++)
                {
                    temp[m + j] = Complex.Conjugate(temp[m - j + 1]);
                }
            }

            /* now compute
             * 
             *   \sum_{k=0}^{N-1} h_k * exp( 2 \pi * i * k * n * \alpha)
             *   
             *  as simple as possible - extrem low performance! Use this implementation as a benchmark implementation only, prefere a native implementation!
             */
            var tempOutput = new Complex[m_Length];

            for (int j = 0; j < m_Length; j++)
            {
                for (int k = 0; k < m_Length; k++)
                {
                    tempOutput[j] += scalingFactor * temp[k] * Complex.Exp(2 * Math.PI * Complex.ImaginaryOne * k * j * m_Alpha);
                }
            }

            /* store the result:
             */
            for (int j = 0; j < m_Length; j++)
            {
                if (tempOutput[j].IsReal(1E-8) == false)
                {
                    throw new InvalidCastException(String.Format("The result of the FFT calculation is not a real number; {0}.", tempOutput[j]));
                }
                outputFourierCoefficients[j] = tempOutput[j].Real;
            }
        }

        /// <summary>Computes the backward Fourier transformation.
        /// </summary>
        /// <param name="inputFourierCoefficients">The input Fourier coefficients with at least 2* (<see cref="FFT.IOneDimensionalRealData.Length"/> / 2 + 1) elements.</param>
        /// <param name="outputFourierCoefficients">The output Fourier coefficients, i.e. out-of-place calculation with at
        /// least <see cref="FFT.IOneDimensionalRealData.Length"/> elements (output).</param>
        /// <remarks>The input are complex numbers with Hermitian symmetry, stored in the CCE format, i.e. 2* (<see cref="FFT.IOneDimensionalRealData.Length"/> / 2 + 1) real numbers, the output
        /// are <see cref="FFT.IOneDimensionalRealData.Length"/> real numbers.
        /// <para>The scaling factor is assumed to be <c>1.0</c>.</para></remarks>
        public void BackwardTransformation(ReadOnlySpan<double> inputFourierCoefficients, Span<double> outputFourierCoefficients)
        {
            BackwardTransformation(inputFourierCoefficients, outputFourierCoefficients, 1.0);
        }
        #endregion

        #endregion
    }
}