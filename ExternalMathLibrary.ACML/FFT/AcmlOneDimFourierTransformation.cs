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
using System.Runtime.InteropServices;

namespace Dodoni.MathLibrary.Basics.LowLevel.Native
{
    /// <summary>Represents the wrapper for AMD's ACML one-dimensional discrete Fourier transformation implementation.
    /// </summary>
    internal class AcmlOneDimFourierTransformation : FFT.IOneDimensional
    {
        #region private const members

        /// <summary>Initialize the transformation.
        /// </summary>
        private const int ModeInit = 0;

        /// <summary>Do some forward transformation.
        /// </summary>
        private const int ModeForwardTransform = -1;

        /// <summary>Do some backward transformation.
        /// </summary>
        private const int ModeBackwardTransform = 1;
        #endregion

        #region private function import

        /// <summary>Complex one-dimensional Fast-Fourier Transformation (Z-FFT-1D).
        /// </summary>
        /// <param name="mode">The mode, i.e. a value which determines the operation performed.</param>
        /// <param name="n">The length of the complex sequence <paramref name="x"/>.</param>
        /// <param name="x">The complex array (in-place) with at least <paramref name="n"/> elements.</param>
        /// <param name="communicationArray">A specific communication array with at least <c>3*<paramref name="n"/> + 100</c> elements.</param>
        /// <param name="info">The info, <c>0</c> represents no error (output).</param>
        [DllImport(AcmlNativeWrapper.dllName, CallingConvention = AcmlNativeWrapper.callingConvention, EntryPoint = "ZFFT1D", ExactSpelling = true)]
        private static extern void acml_ZFFT1D(ref int mode, ref int n, IntPtr x, IntPtr communicationArray, ref int info);

        /// <summary>Complex one-dimensional Fast-Fourier Transformation (Z-FFT-1D), extended functionality.
        /// </summary>
        /// <param name="mode">The mode, i.e. a value which determines the operation performed.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="inPlace">A value indicating whether some in-place or some out-of-place calculation will be take place.</param>
        /// <param name="n">The length of the complex sequence <paramref name="x"/>.</param>
        /// <param name="x">The complex array (in-place) with at least <paramref name="n"/> elements.</param>
        /// <param name="incX">The increment with respect to <paramref name="x"/>, in general set to <c>1</c>.</param>
        /// <param name="y">If some out-of-place calculation takes place a complex array which represents the output; otherwise <see cref="IntPtr.Zero"/>.</param>
        /// <param name="incY">The increment with respect to <paramref name="y"/>.</param>
        /// <param name="communicationArray">A specific communication array with at least <c>3*<paramref name="n"/> + 100</c> elements.</param>
        /// <param name="info">The info, <c>0</c> represents no error (output).</param>
        [DllImport(AcmlNativeWrapper.dllName, CallingConvention = AcmlNativeWrapper.callingConvention, EntryPoint = "ZFFT1DX", ExactSpelling = true)]
        private static extern void acm_ZFFT1DX(ref int mode, ref double scale, ref bool inPlace, ref int n, IntPtr x, ref int incX, IntPtr y, ref int incY, IntPtr communicationArray, ref int info);
        #endregion

        #region private members

        /// <summary>The length, i.e. the number of Fourier coefficients.
        /// </summary>
        private int m_Length;

        /// <summary>A working array needed for the calculation.
        /// </summary>
        private Complex[] m_CommunicationArray;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="AcmlOneDimFourierTransformation"/> class.
        /// </summary>
        /// <param name="length">The length, i.e. the number of Fourier coefficients.</param>
        internal AcmlOneDimFourierTransformation(int length)
        {
            m_Length = length;
            m_CommunicationArray = new Complex[3 * length + 100];

            // do some initialization:
            unsafe
            {
                fixed (Complex* workingArrayPtr = m_CommunicationArray)
                {
                    int mode = ModeInit;
                    double scale = 0;  // dummy
                    bool inPlace = false; // dummy?
                    int incX = 1;
                    int incY = 1;
                    int info = 0;
                    acm_ZFFT1DX(ref mode, ref scale, ref inPlace, ref length, IntPtr.Zero, ref incX, IntPtr.Zero, ref incY, (IntPtr)workingArrayPtr, ref info);
                    CheckInfoResult(info, "Init FFT");
                }
            }
        }
        #endregion

        #region public properties

        #region IOperable Members

        ///<summary>Gets a value indicating whether this instance is operable.
        ///</summary>
        ///<value>
        ///   <c>true</c> if this instance is operable; otherwise, <c>false</c>.
        ///</value>
        ///<remarks>
        ///   <c>false</c> will be returned if the current instance is already disposed.
        ///</remarks>
        public bool IsOperable
        {
            get { return (m_CommunicationArray != null); }
        }
        #endregion

        #region FFT.IOneDimensional Members

        /// <summary>Gets the length, i.e. the number of (complex or real) Fourier coefficients.
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
            get { return 1.0 / m_Length; }
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

        #region IDisposable Members

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // nothing do here
        }
        #endregion

        #region FFT.IOneDimensional Members

        /// <summary>Sets the scaling factor \alpha in the Fourier Transformation.
        /// </summary>
        /// <param name="alpha">The scaling factor \alpha.</param>
        /// <exception cref="InvalidOperationException">Thrown if <see cref="FFT.IOneDimensional.FourierExponentialFactorRestriction"/> indicates that the parameter \alpha can not be changed in this way or <paramref name="alpha"/> contains an invalid value.</exception>
        public void SetParameterAlpha(Complex alpha)
        {
            throw new InvalidOperationException();  // an arbitrary parameter \alpha is not allowed
        }

        /// <summary>Sets the scaling factor \alpha in the Fourier Transformation.
        /// </summary>
        /// <param name="alpha">The scaling factor \alpha.</param>
        /// <exception cref="InvalidOperationException">Thrown if <see cref="FFT.IOneDimensional.FourierExponentialFactorRestriction"/> indicates that the parameter \alpha can not be changed in this way or <paramref name="alpha"/> contains an invalid value.</exception>
        public void SetParameterAlpha(double alpha)
        {
            throw new InvalidOperationException();  // an arbitrary parameter \alpha is not allowed
        }

        /// <summary>Compute the forward Fourier transformation.
        /// </summary>
        /// <param name="fourierCoefficients">The input as well as the output [in place] with at least <see cref="FFT.IOneDimensional.Length"/> elements.</param>
        /// <param name="scalingFactor">The scaling factor with respect to the input domain.</param>
        public void ForwardTransformation(Complex[] fourierCoefficients, double scalingFactor)
        {
            unsafe
            {
                fixed (Complex* inputOutput = fourierCoefficients, workingArrayPtr = m_CommunicationArray)
                {
                    int mode = ModeForwardTransform;
                    bool inPlace = true;
                    int incX = 1;
                    int incY = 1;
                    int info = 0;
                    acm_ZFFT1DX(ref mode, ref scalingFactor, ref inPlace, ref m_Length, (IntPtr)inputOutput, ref incX, IntPtr.Zero, ref incY, (IntPtr)workingArrayPtr, ref info);
                    CheckInfoResult(info, "Forward transformation (in-place)");
                }
            }
        }

        /// <summary>Compute the forward Fourier transformation.
        /// </summary>
        /// <param name="fourierCoefficients">The input as well as the output [in place] with at least <see cref="FFT.IOneDimensional.Length"/> elements.</param>
        /// <remarks>The scaling factor is assumed to be <c>1.0</c>.</remarks>
        public void ForwardTransformation(Complex[] fourierCoefficients)
        {
            unsafe
            {
                fixed (Complex* inputOutput = fourierCoefficients, workingArrayPtr = m_CommunicationArray)
                {
                    int mode = ModeForwardTransform;
                    bool inPlace = true;
                    int incX = 1;
                    int incY = 1;
                    int info = 0;
                    double scalingFactor = 1;
                    acm_ZFFT1DX(ref mode, ref scalingFactor, ref inPlace, ref m_Length, (IntPtr)inputOutput, ref incX, IntPtr.Zero, ref incY, (IntPtr)workingArrayPtr, ref info);
                    CheckInfoResult(info, "Forward transformation (in-place)");
                }
            }
        }

        /// <summary>Compute the forward Fourier transformation.
        /// </summary>
        /// <param name="inputFourierCoefficients">The input Fourier coefficients with at least <see cref="FFT.IOneDimensional.Length"/> elements.</param>
        /// <param name="outputFourierCoefficients">The output Fourier coefficients, i.e. out-of-place calculation with at
        /// least <see cref="FFT.IOneDimensional.Length"/> elements (output).</param>
        /// <param name="scalingFactor">The scaling factor with respect to the input domain.</param>
        public void ForwardTransformation(Complex[] inputFourierCoefficients, Complex[] outputFourierCoefficients, double scalingFactor)
        {
            unsafe
            {
                fixed (Complex* input = inputFourierCoefficients, output = outputFourierCoefficients, workingArrayPtr = m_CommunicationArray)
                {
                    int mode = ModeForwardTransform;
                    bool inPlace = false;
                    int incX = 1;
                    int incY = 1;
                    int info = 0;
                    acm_ZFFT1DX(ref mode, ref scalingFactor, ref inPlace, ref m_Length, (IntPtr)input, ref incX, (IntPtr)output, ref incY, (IntPtr)workingArrayPtr, ref info);
                    CheckInfoResult(info, "Forward transformation (out-of-place)");
                }
            }
        }

        /// <summary>Compute the forward Fourier transformation.
        /// </summary>
        /// <param name="inputFourierCoefficients">The input Fourier coefficients with at least <see cref="FFT.IOneDimensional.Length"/> elements.</param>
        /// <param name="outputFourierCoefficients">The output Fourier coefficients, i.e. out-of-place calculation with at
        /// least <see cref="FFT.IOneDimensional.Length"/> elements (output).</param>
        /// <remarks>The scaling factor is assumed to be <c>1.0</c>.</remarks>
        public void ForwardTransformation(Complex[] inputFourierCoefficients, Complex[] outputFourierCoefficients)
        {
            unsafe
            {
                fixed (Complex* input = inputFourierCoefficients, output = outputFourierCoefficients, workingArrayPtr = m_CommunicationArray)
                {
                    int mode = ModeForwardTransform;
                    bool inPlace = false;
                    int incX = 1;
                    int incY = 1;
                    int info = 0;
                    double scalingFactor = 1.0;
                    acm_ZFFT1DX(ref mode, ref scalingFactor, ref inPlace, ref m_Length, (IntPtr)input, ref incX, (IntPtr)output, ref incY, (IntPtr)workingArrayPtr, ref info);
                    CheckInfoResult(info, "Forward transformation (out-of-place)");
                }
            }
        }

        /// <summary>Compute the backward Fourier transformation.
        /// </summary>
        /// <param name="fourierCoefficients">The input as well as the output [in place] with at least <see cref="FFT.IOneDimensional.Length"/> elements.</param>
        /// <param name="scalingFactor">The scaling factor with respect to the input domain.</param>
        public void BackwardTransformation(Complex[] fourierCoefficients, double scalingFactor)
        {
            unsafe
            {
                fixed (Complex* inputOutput = fourierCoefficients, workingArrayPtr = m_CommunicationArray)
                {
                    int mode = ModeBackwardTransform;
                    bool inPlace = true;
                    int incX = 1;
                    int incY = 1;
                    int info = 0;
                    acm_ZFFT1DX(ref mode, ref scalingFactor, ref inPlace, ref m_Length, (IntPtr)inputOutput, ref incX, IntPtr.Zero, ref incY, (IntPtr)workingArrayPtr, ref info);
                    CheckInfoResult(info, "Backward transformation (in-place)");
                }
            }
        }

        /// <summary>Compute the backward Fourier transformation.
        /// </summary>
        /// <param name="fourierCoefficients">The input as well as the output [in place] with at least <see cref="FFT.IOneDimensional.Length"/> elements.</param>
        /// <remarks>The scaling factor is assumed to be <c>1.0</c>.</remarks>
        public void BackwardTransformation(Complex[] fourierCoefficients)
        {
            unsafe
            {
                fixed (Complex* inputOutput = fourierCoefficients, workingArrayPtr = m_CommunicationArray)
                {
                    int mode = ModeBackwardTransform;
                    bool inPlace = true;
                    int incX = 1;
                    int incY = 1;
                    int info = 0;
                    double scalingFactor = 1.0;
                    acm_ZFFT1DX(ref mode, ref scalingFactor, ref inPlace, ref m_Length, (IntPtr)inputOutput, ref incX, IntPtr.Zero, ref incY, (IntPtr)workingArrayPtr, ref info);
                    CheckInfoResult(info, "Backward transformation (in-place)");
                }
            }
        }

        /// <summary>Compute the backward Fourier transformation.
        /// </summary>
        /// <param name="inputFourierCoefficients">The input Fourier coefficients with at least <see cref="FFT.IOneDimensional.Length"/> elements.</param>
        /// <param name="outputFourierCoefficients">The output Fourier coefficients, i.e. out-of-place calculation with at
        /// least <see cref="FFT.IOneDimensional.Length"/> elements (output).</param>
        /// <param name="scalingFactor">The scaling factor with respect to the input domain.</param>
        public void BackwardTransformation(Complex[] inputFourierCoefficients, Complex[] outputFourierCoefficients, double scalingFactor)
        {
            unsafe
            {
                fixed (Complex* input = inputFourierCoefficients, output = outputFourierCoefficients, workingArrayPtr = m_CommunicationArray)
                {
                    int mode = ModeBackwardTransform;
                    bool inPlace = false;
                    int incX = 1;
                    int incY = 1;
                    int info = 0;
                    acm_ZFFT1DX(ref mode, ref scalingFactor, ref inPlace, ref m_Length, (IntPtr)input, ref incX, (IntPtr)output, ref incY, (IntPtr)workingArrayPtr, ref info);
                    CheckInfoResult(info, "Backward transformation (out-of-place)");
                }
            }
        }

        /// <summary>Compute the backward Fourier transformation.
        /// </summary>
        /// <param name="inputFourierCoefficients">The input Fourier coefficients with at least <see cref="FFT.IOneDimensional.Length"/> elements.</param>
        /// <param name="outputFourierCoefficients">The output Fourier coefficients, i.e. out-of-place calculation with at
        /// least <see cref="FFT.IOneDimensional.Length"/> elements (output).</param>
        /// <remarks>The scaling factor is assumed to be <c>1.0</c>.</remarks>
        public void BackwardTransformation(Complex[] inputFourierCoefficients, Complex[] outputFourierCoefficients)
        {
            unsafe
            {
                fixed (Complex* input = inputFourierCoefficients, output = outputFourierCoefficients, workingArrayPtr = m_CommunicationArray)
                {
                    int mode = ModeBackwardTransform;
                    bool inPlace = false;
                    int incX = 1;
                    int incY = 1;
                    int info = 0;
                    double scalingFactor = 1.0;
                    acm_ZFFT1DX(ref mode, ref scalingFactor, ref inPlace, ref m_Length, (IntPtr)input, ref incX, (IntPtr)output, ref incY, (IntPtr)workingArrayPtr, ref info);
                    CheckInfoResult(info, "Backward transformation (out-of-place)");
                }
            }
        }
        #endregion

        #endregion

        #region private methods

        /// <summary>Checks the 'info' result of a specific ACML function.
        /// </summary>
        /// <param name="info">The 'info' result; <c>0</c> represents no error.</param>
        /// <param name="description">The description.</param>
        /// <exception cref="InvalidOperationException">Thrown, if <paramref name="info"/> != 0.</exception>
        private void CheckInfoResult(int info, string description)
        {
            if (info != 0)
            {
                throw new InvalidOperationException(description + " info: " + info);
            }
        }
        #endregion
    }
}