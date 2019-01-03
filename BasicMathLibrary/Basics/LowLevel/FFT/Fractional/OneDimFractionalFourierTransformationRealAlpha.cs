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
using System.Collections.Concurrent;

using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.Basics.LowLevel.Interdisciplinary
{
    ///<summary>Serves as a Fast Fractional Fourier Transformation, i.e. 
    ///<para>
    ///  H_n = a * \sum_{k=0}^{N-1} h_k * exp( -2 \pi * i * k * n * \alpha), (forward transformation)
    ///</para>
    /// where a is some scaling factor and \alpha is the real parameter of the Fractional Fourier Transformation; or
    /// <para>
    ///   h_n = b * \sum_{k=0}^{N-1} H_k * exp(2 \pi * i * k * n * \alpha), (backward transformation without factor 1/N etc.)
    /// </para>
    /// where b is some scaling factor.
    /// </summary>
    /// <remarks>In the case of a backward transformation one has to set b = 1/N to get the correct inverse Fourier coefficients (with a = 1). 
    /// <para>
    ///   This implementation is based on 'Option pricing using the Fractional FFT', Kyriakos Chourdakis, 2005.
    /// </para>
    /// </remarks>
    internal class OneDimFractionalFourierTransformationRealAlpha : FFT.IOneDimensional
    {
        #region private members

        /// <summary>The number of Fourier coefficients.
        /// </summary>
        private readonly int m_Length;

        /// <summary>The parameter \alpha.
        /// </summary>
        private double m_Alpha;

        /// <summary>The Fourier transformation with 2*n coefficients.
        /// </summary>
        private FFT.IOneDimensional m_FourierTransformation;

        /// <summary>Represents the factor needed in the forward transformation calculation that is independend on the input, i.e. e^{-i*pi*j^2*\alpha}, j=0,..,n-1.
        /// </summary>
        private readonly Complex[] m_ForwardPreFactor;

        /// <summary>Represents the factor needed in the backward transformation calculation that is independend on the input, i.e. e^{i*pi*j^2*\alpha}, j=0,..,n-1.
        /// </summary>
        private readonly Complex[] m_BackwardPreFactor;

        /// <summary>Represents the result of the forward Fourier transformation applied to the vector 'z' in §2.3 of
        /// 'Option pricing using the fractional FFT', K. Chourdakis, which is independend on the input coefficients of the forward transformation.
        /// </summary>
        private readonly Complex[] m_ForwardTransformedVectorZ;

        /// <summary>Represents the result of the backward Fourier transformation applied to the vector 'z' in §2.3 of
        /// 'Option pricing using the fractional FFT', K. Chourdakis, which is independend on the input coefficients of the backward transformation.
        /// </summary>
        private readonly Complex[] m_BackwardTransformedVectorZ;

        /// <summary>The working array 'y' with 2*m_Length elements will be reused in each call of a forward/backward transformation and stored in this thread-safe stack.
        /// </summary>
        /// <remarks>Multiple working arrays are required if and only if at least two forward/backward transformation methods are called concurrently.</remarks>
        private ConcurrentStack<Complex[]> m_WorkingArrays = new ConcurrentStack<Complex[]>();
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="OneDimFractionalFourierTransformationRealAlpha"/> class.
        /// </summary>
        /// <param name="alpha">The parameter \alpha of the Fractional Fast Fourier Transformation.</param>
        /// <param name="oneDimFourierTransformationFactory">The one-dimensional Fourier transformation factory.</param>
        /// <param name="length">The length, i.e. the number of (complex) Fourier coefficients.</param>
        internal OneDimFractionalFourierTransformationRealAlpha(int length, double alpha, IOneDimFourierTransformationFactory oneDimFourierTransformationFactory)
        {
            if (length <= 0)
            {
                throw new ArgumentException(String.Format(ExceptionMessages.ArgumentOutOfRangeGreater, "Length <" + length + ">", 0));
            }
            m_Length = length;
            m_ForwardPreFactor = new Complex[length];
            m_BackwardPreFactor = new Complex[length];

            int n = 2 * length;
            m_FourierTransformation = oneDimFourierTransformationFactory.Create(n);
            m_ForwardTransformedVectorZ = new Complex[n];
            m_BackwardTransformedVectorZ = new Complex[n];

            SetParameterAlpha(alpha);
        }
        #endregion

        #region public properties

        #region IOperable Members

        /// <summary>Gets a value indicating whether this instance is operable.
        /// </summary>
        /// <value><c>true</c> if this instance is operable; otherwise, <c>false</c>.
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

        #region IDisposable Members

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // nothing to do here
        }
        #endregion

        #region FFT.IOneDimensional Members

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
        /// <value>The restriction of the parameter \alpha in the Fourier transformation.
        /// </value>
        public FFT.FourierExponentialFactorRestriction FourierExponentialFactorRestriction
        {
            get { return FFT.FourierExponentialFactorRestriction.RealNumber; }
        }
        #endregion

        #endregion

        #region public methods

        #region FFT.IOneDimensional Members

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
            m_Alpha = alpha;

            /* calculate the pre-factor e^{+/- i*pi*j^2*\alpha}, j=0,..,n-1 and the Fourier transformed of z:
             */
            for (int j = 0; j < m_Length; j++)
            {
                double arg = j * j * Math.PI * alpha;
                double cos = Math.Cos(arg);
                double sin = Math.Sin(arg);

                m_ForwardPreFactor[j] = m_BackwardTransformedVectorZ[j] = new Complex(cos, -sin);
                m_BackwardPreFactor[j] = m_ForwardTransformedVectorZ[j] = new Complex(cos, sin);

                /* the second half of the vector z_forward, z_backward: 
                 */
                double arg2 = Math.PI * (m_Length - j) * (m_Length - j) * alpha;
                double cos2 = Math.Cos(arg2);
                double sin2 = Math.Sin(arg2);

                m_ForwardTransformedVectorZ[j + m_Length] = new Complex(cos2, sin2);
                m_BackwardTransformedVectorZ[j + m_Length] = new Complex(cos2, -sin2);
            }
            /* apply the Forward Fourier transformation to the vectors z_forward, z_backward: 
             */
            m_FourierTransformation.ForwardTransformation(m_ForwardTransformedVectorZ);
            m_FourierTransformation.ForwardTransformation(m_BackwardTransformedVectorZ);
        }

        /// <summary>Computes the forward Fourier transformation.
        /// </summary>
        /// <param name="fourierCoefficients">The input as well as the output [in place] with at least <see cref="FFT.IOneDimensional.Length"/> elements.</param>
        /// <param name="scalingFactor">The scaling factor with respect to the input domain.</param>
        public void ForwardTransformation(Complex[] fourierCoefficients, double scalingFactor)
        {
            ForwardTransformation(fourierCoefficients, fourierCoefficients, scalingFactor);
        }

        /// <summary>Computes the forward Fourier transformation.
        /// </summary>
        /// <param name="fourierCoefficients">The input as well as the output [in place] with at least <see cref="FFT.IOneDimensional.Length"/> elements.</param>
        /// <remarks>The scaling factor is assumed to be <c>1.0</c>.</remarks>
        public void ForwardTransformation(Complex[] fourierCoefficients)
        {
            ForwardTransformation(fourierCoefficients, fourierCoefficients, 1.0);
        }

        /// <summary>Computes the forward Fourier transformation.
        /// </summary>
        /// <param name="inputFourierCoefficients">The input Fourier coefficients with at least <see cref="FFT.IOneDimensional.Length"/> elements.</param>
        /// <param name="outputFourierCoefficients">The output Fourier coefficients, i.e. out-of-place calculation with at least <see cref="FFT.IOneDimensional.Length"/> elements (output).</param>
        /// <param name="scalingFactor">The scaling factor with respect to the input domain.</param>
        public void ForwardTransformation(Complex[] inputFourierCoefficients, Complex[] outputFourierCoefficients, double scalingFactor)
        {
            int n = 2 * m_Length;

            /* prepare working array 'y': */
            if (m_WorkingArrays.TryPop(out Complex[] y) == false)
            {
                y = new Complex[n];
            }
            BLAS.Level1.zcopy(m_Length, m_ForwardPreFactor, y);
            BLAS.Level1.zscal(m_Length, 0.0, y, 1, m_Length);

            VectorUnit.Basics.Mul(m_Length, y, inputFourierCoefficients, y);

            m_FourierTransformation.ForwardTransformation(y);

            VectorUnit.Basics.Mul(n, y, m_ForwardTransformedVectorZ, y, scalingFactor);  // y_j = scalingFactor * y_j *  m_ForwardTransformedVectorZ_j
            m_FourierTransformation.BackwardTransformation(y);

            VectorUnit.Basics.Mul(m_Length, y, m_ForwardPreFactor, y, 1.0 / n);
            BLAS.Level1.zcopy(m_Length, y, outputFourierCoefficients);

            m_WorkingArrays.Push(y);   // store for later re-use
        }

        /// <summary>Computes the forward Fourier transformation.
        /// </summary>
        /// <param name="inputFourierCoefficients">The input Fourier coefficients with at least <see cref="FFT.IOneDimensional.Length"/> elements.</param>
        /// <param name="outputFourierCoefficients">The output Fourier coefficients, i.e. out-of-place calculation with at
        /// least <see cref="FFT.IOneDimensional.Length"/> elements (output).</param>
        /// <remarks>The scaling factor is assumed to be <c>1.0</c>.</remarks>
        public void ForwardTransformation(Complex[] inputFourierCoefficients, Complex[] outputFourierCoefficients)
        {
            ForwardTransformation(inputFourierCoefficients, outputFourierCoefficients, 1.0);
        }

        /// <summary>Computes the backward Fourier transformation.
        /// </summary>
        /// <param name="fourierCoefficients">The input as well as the output [in place] with at least <see cref="FFT.IOneDimensional.Length"/> elements.</param>
        /// <param name="scalingFactor">The scaling factor with respect to the input domain.</param>
        public void BackwardTransformation(Complex[] fourierCoefficients, double scalingFactor)
        {
            BackwardTransformation(fourierCoefficients, fourierCoefficients, scalingFactor);
        }

        /// <summary>Computes the backward Fourier transformation.
        /// </summary>
        /// <param name="fourierCoefficients">The input as well as the output [in place] with at least <see cref="FFT.IOneDimensional.Length"/> elements.</param>
        /// <remarks>The scaling factor is assumed to be <c>1.0</c>.</remarks>
        public void BackwardTransformation(Complex[] fourierCoefficients)
        {
            BackwardTransformation(fourierCoefficients, fourierCoefficients, 1.0);
        }

        /// <summary>Computes the backward Fourier transformation.
        /// </summary>
        /// <param name="inputFourierCoefficients">The input Fourier coefficients with at least <see cref="FFT.IOneDimensional.Length"/> elements.</param>
        /// <param name="outputFourierCoefficients">The output Fourier coefficients, i.e. out-of-place calculation with at
        /// least <see cref="FFT.IOneDimensional.Length"/> elements (output).</param>
        /// <param name="scalingFactor">The scaling factor with respect to the input domain.</param>
        public void BackwardTransformation(Complex[] inputFourierCoefficients, Complex[] outputFourierCoefficients, double scalingFactor)
        {
            /* apply the approach with '-\alpha' instead of \alpha:
             */
            int n = 2 * m_Length;

            /* prepare working array 'y': */
            if (m_WorkingArrays.TryPop(out Complex[] y) == false)
            {
                y = new Complex[n];
            }
            BLAS.Level1.zcopy(m_Length, m_BackwardPreFactor, y);
            BLAS.Level1.zscal(m_Length, 0.0, y, 1, m_Length);

            VectorUnit.Basics.Mul(m_Length, y, inputFourierCoefficients, y);

            m_FourierTransformation.ForwardTransformation(y);

            VectorUnit.Basics.Mul(n, y, m_BackwardTransformedVectorZ, y, scalingFactor);  // y_j = scalingFactor * y_j *  m_ForwardTransformedVectorZ_j
            m_FourierTransformation.BackwardTransformation(y);

            VectorUnit.Basics.Mul(m_Length, y, m_BackwardPreFactor, y, 1.0 / n);
            BLAS.Level1.zcopy(m_Length, y, outputFourierCoefficients);

            m_WorkingArrays.Push(y);   // store for later re-use
        }

        /// <summary>Computes the backward Fourier transformation.
        /// </summary>
        /// <param name="inputFourierCoefficients">The input Fourier coefficients with at least <see cref="FFT.IOneDimensional.Length"/> elements.</param>
        /// <param name="outputFourierCoefficients">The output Fourier coefficients, i.e. out-of-place calculation with at
        /// least <see cref="FFT.IOneDimensional.Length"/> elements (output).</param>
        /// <remarks>The scaling factor is assumed to be <c>1.0</c>.</remarks>
        public void BackwardTransformation(Complex[] inputFourierCoefficients, Complex[] outputFourierCoefficients)
        {
            BackwardTransformation(inputFourierCoefficients, outputFourierCoefficients, 1.0);
        }
        #endregion

        /// <summary>Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return String.Format("1-dim Fractional Fourier Transformation, length: {0}; alpha: {1}.", m_Length, m_Alpha);
        }
        #endregion
    }
}