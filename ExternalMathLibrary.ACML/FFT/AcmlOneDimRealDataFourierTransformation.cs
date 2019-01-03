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
    /// <summary>Represents the wrapper for discrete Fourier transformations where the input are real (rather than complex) numbers with respect to AMD's ACML library.
    /// </summary>
    internal class AcmlOneDimRealDataFourierTransformation : FFT.IOneDimensionalRealData
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

        [DllImport(AcmlNativeWrapper.dllName, CallingConvention = AcmlNativeWrapper.callingConvention, EntryPoint = "DZFFT", ExactSpelling = true)]
        private static extern void acml_DZFFT(ref int mode, ref int n, IntPtr x, IntPtr communicationArray, ref int info);

        [DllImport(AcmlNativeWrapper.dllName, CallingConvention = AcmlNativeWrapper.callingConvention, EntryPoint = "DZFFT", ExactSpelling = true)]
        private static extern void acml_DZFFT(ref int mode, ref int n, [In, Out] double[] x, [In, Out] double[] communicationArray, out int info);

        [DllImport(AcmlNativeWrapper.dllName, CallingConvention = AcmlNativeWrapper.callingConvention, EntryPoint = "ZDFFT", ExactSpelling = true)]
        private static extern void acml_ZDFFT(ref int mode, ref int n, [In, Out] double[] x, [In, Out] double[] communicationArray, out int info);
        #endregion

        #region private members

        /// <summary>The length, i.e. the number of Fourier coefficients.
        /// </summary>
        private int m_Length;

        /// <summary>The square-root of the length.
        /// </summary>
        /// <remarks>This member is used for performance reason only.</remarks>
        private double m_SqrtOfLength;

        /// <summary>A working array needed for the calculation.
        /// </summary>
        private double[] m_CommunicationArray;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="AcmlOneDimRealDataFourierTransformation"/> class.
        /// </summary>
        /// <param name="length">The length, i.e. the number of Fourier coefficients.</param>
        internal AcmlOneDimRealDataFourierTransformation(int length)
        {
            m_Length = length;
            m_SqrtOfLength = Math.Sqrt(length);
            m_CommunicationArray = new double[3 * length + 100];
            int mode = 100;

            int info;
            acml_DZFFT(ref mode, ref length, null, m_CommunicationArray, out info);
            CheckInfoResult(info, "Initialization fails.");
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

        #region FFT.IOneDimensionalRealData Members

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
            int mode = 1;
            int info;

            acml_DZFFT(ref mode, ref m_Length, fourierCoefficients, m_CommunicationArray, out info);
            CheckInfoResult(info, "Forward transformation (in-place, scaling factor)");

            HermitianToCcsRepresentation(fourierCoefficients, m_SqrtOfLength * scalingFactor);
        }

        /// <summary>Computes the forward Fourier transformation.
        /// </summary>
        /// <param name="fourierCoefficients">The input as well as the output [in place] with at least 2* (<see cref="FFT.IOneDimensionalRealData.Length"/>/2 + 1) elements.</param>
        /// <remarks>The output are complex numbers with Hermitian symmetry, stored in the CCE format.
        /// <para>The scaling factor is assumed to be <c>1.0</c>.</para></remarks>
        public void ForwardTransformation(double[] fourierCoefficients)
        {
            int mode = 1;
            int info;

            acml_DZFFT(ref mode, ref m_Length, fourierCoefficients, m_CommunicationArray, out info);
            CheckInfoResult(info, "Forward transformation (in-place)");

            HermitianToCcsRepresentation(fourierCoefficients, m_SqrtOfLength);
        }

        /// <summary>Computes the forward Fourier transformation.
        /// </summary>
        /// <param name="inputFourierCoefficients">The input Fourier coefficients with at least <see cref="FFT.IOneDimensionalRealData.Length"/> elements.</param>
        /// <param name="outputFourierCoefficients">The output Fourier coefficients, i.e. out-of-place calculation with at least 2* (<see cref="FFT.IOneDimensionalRealData.Length"/> / 2 + 1) elements (output).</param>
        /// <param name="scalingFactor">The scaling factor with respect to the input domain.</param>
        /// <remarks>The output are complex numbers with Hermitian symmetry, stored in the CCE format.</remarks>
        public void ForwardTransformation(double[] inputFourierCoefficients, double[] outputFourierCoefficients, double scalingFactor)
        {
            BLAS.Level1.dcopy(m_Length, inputFourierCoefficients, outputFourierCoefficients);

            int mode = 1;
            int info;

            acml_DZFFT(ref mode, ref m_Length, outputFourierCoefficients, m_CommunicationArray, out info);
            CheckInfoResult(info, "Forward transformation (scaling factor)");

            HermitianToCcsRepresentation(outputFourierCoefficients, m_SqrtOfLength * scalingFactor);
        }

        /// <summary>Computes the forward Fourier transformation.
        /// </summary>
        /// <param name="inputFourierCoefficients">The input Fourier coefficients with at least <see cref="FFT.IOneDimensionalRealData.Length"/> elements.</param>
        /// <param name="outputFourierCoefficients">The output Fourier coefficients, i.e. out-of-place calculation with at least 2* (<see cref="FFT.IOneDimensionalRealData.Length"/> / 2 + 1) elements (output).</param>
        /// <remarks>The output are complex numbers with Hermitian symmetry, stored in the CCE format.
        /// <para>The scaling factor is assumed to be <c>1.0</c>.</para></remarks>
        public void ForwardTransformation(double[] inputFourierCoefficients, double[] outputFourierCoefficients)
        {
            BLAS.Level1.dcopy(m_Length, inputFourierCoefficients, outputFourierCoefficients);

            int mode = 1;
            int info;

            acml_DZFFT(ref mode, ref m_Length, outputFourierCoefficients, m_CommunicationArray, out info);
            CheckInfoResult(info, "Forward transformation");

            HermitianToCcsRepresentation(outputFourierCoefficients, m_SqrtOfLength);
        }

        /// <summary>Computes the backward Fourier transformation.
        /// </summary>
        /// <param name="fourierCoefficients">The input as well as the output [in place] with at least 2* (<see cref="FFT.IOneDimensionalRealData.Length" /> / 2 + 1) elements.</param>
        /// <param name="scalingFactor">The scaling factor with respect to the input domain.</param>
        /// <remarks>The input are complex numbers with Hermitian symmetry, stored in the CCE format, i.e. 2* (<see cref="FFT.IOneDimensionalRealData.Length" /> / 2 + 1) real numbers, the output
        /// are <see cref="FFT.IOneDimensionalRealData.Length" /> real numbers.
        /// </remarks>
        public void BackwardTransformation(double[] fourierCoefficients, double scalingFactor)
        {
            CcsToHermitianRepresentation(fourierCoefficients, scalingFactor);

            int mode = 1;
            int info;

            acml_ZDFFT(ref mode, ref m_Length, fourierCoefficients, m_CommunicationArray, out info);
            CheckInfoResult(info, "Backward transformation (in-place, scaling factor)");
        }

        /// <summary>Computes the backward Fourier transformation.
        /// </summary>
        /// <param name="fourierCoefficients">The input as well as the output [in place] with at least 2* (<see cref="FFT.IOneDimensionalRealData.Length" /> / 2 + 1) elements.</param>
        /// <remarks>The input are complex numbers with Hermitian symmetry, stored in the CCE format, i.e. 2* (<see cref="FFT.IOneDimensionalRealData.Length" /> / 2 + 1) real numbers, the output
        /// are <see cref="FFT.IOneDimensionalRealData.Length" /> real numbers.
        /// <para>The scaling factor is assumed to be <c>1.0</c>.</para>
        /// </remarks>
        public void BackwardTransformation(double[] fourierCoefficients)
        {
            CcsToHermitianRepresentation(fourierCoefficients);
            int mode = 1;
            int info;

            acml_ZDFFT(ref mode, ref m_Length, fourierCoefficients, m_CommunicationArray, out info);
            CheckInfoResult(info, "Backward transformation (in-place)");
        }

        /// <summary>Computes the backward Fourier transformation.
        /// </summary>
        /// <param name="inputFourierCoefficients">The input Fourier coefficients with at least 2* (<see cref="FFT.IOneDimensionalRealData.Length" /> / 2 + 1) elements.</param>
        /// <param name="outputFourierCoefficients">The output Fourier coefficients, i.e. out-of-place calculation with at least <see cref="FFT.IOneDimensionalRealData.Length" /> elements (output).</param>
        /// <param name="scalingFactor">The scaling factor with respect to the input domain.</param>
        /// <remarks>
        /// The input are complex numbers with Hermitian symmetry, stored in the CCE format, i.e. 2* (<see cref="FFT.IOneDimensionalRealData.Length" /> / 2 + 1) real numbers, the output
        /// are <see cref="FFT.IOneDimensionalRealData.Length" /> real numbers.
        /// </remarks>
        public void BackwardTransformation(double[] inputFourierCoefficients, double[] outputFourierCoefficients, double scalingFactor)
        {
            CcsToHermitianRepresentation(inputFourierCoefficients, outputFourierCoefficients, scalingFactor);
            int mode = 1;
            int info;

            acml_ZDFFT(ref mode, ref m_Length, outputFourierCoefficients, m_CommunicationArray, out info);
            CheckInfoResult(info, "Backward transformation (scaling factor)");
        }

        /// <summary>Computes the backward Fourier transformation.
        /// </summary>
        /// <param name="inputFourierCoefficients">The input Fourier coefficients with at least 2* (<see cref="FFT.IOneDimensionalRealData.Length" /> / 2 + 1) elements.</param>
        /// <param name="outputFourierCoefficients">The output Fourier coefficients, i.e. out-of-place calculation with at
        /// least <see cref="FFT.IOneDimensionalRealData.Length" /> elements (output).</param>
        /// <remarks>The input are complex numbers with Hermitian symmetry, stored in the CCE format, i.e. 2* (<see cref="FFT.IOneDimensionalRealData.Length"/> / 2 + 1) real numbers, the output
        /// are <see cref="FFT.IOneDimensionalRealData.Length" /> real numbers.
        /// <para>The scaling factor is assumed to be <c>1.0</c>.</para>
        /// </remarks>
        public void BackwardTransformation(double[] inputFourierCoefficients, double[] outputFourierCoefficients)
        {
            CcsToHermitianRepresentation(inputFourierCoefficients, outputFourierCoefficients);
            int mode = 1;
            int info;

            acml_ZDFFT(ref mode, ref m_Length, outputFourierCoefficients, m_CommunicationArray, out info);
            CheckInfoResult(info, "Backward transformation");
        }
        #endregion

        #endregion

        #region protected methods

        /// <summary>Checks the 'info' result of a specific ACML function.
        /// </summary>
        /// <param name="info">The 'info' result; <c>0</c> represents no error.</param>
        /// <param name="description">The description.</param>
        /// <exception cref="InvalidOperationException">Thrown, if <paramref name="info"/> != 0.</exception>
        protected void CheckInfoResult(int info, string description)
        {
            if (info != 0)
            {
                throw new InvalidOperationException(description + " info: " + info);
            }
        }

        /// <summary>Converts Fourier coefficients from the Hermitian representation into CCS format. 
        /// </summary>
        /// <param name="x">An array of at least 2 * (<see cref="Length"/> / 2 + 1) elements representing Fourier coefficients. 
        /// Hermitian representation on input, i.e. Re z_0, Re z_1, ..., -Im z_k, -Im z_{k-1},..., -Im z_1, where k=n/2 if <see cref="Length"/> is odd, otherwise k= n/2-1, thus <see cref="Length"/> relevant elements.
        /// on exit CCS representation, i.e. Re z_0, Im z_0 (=.0.0), Re z_1, Im z_1, etc., thus 2 * (<see cref="Length"/> / 2 + 1) elements.</param>
        /// <param name="scalingFactor">The scaling factor.</param>
        protected void HermitianToCcsRepresentation(double[] x, double scalingFactor = 1.0)
        {
            /* Re-ordering is a O(n^2) operation and time consuming. One may use a temporary array instead (perhaps the communication array?)
             * and apply it to HermitianToCcsRepresentation(double[], double[], double)!
             */
            int n = 2 * (m_Length / 2 + 1);
            BLAS.Level1.dscal(n, scalingFactor, x);

            int lastImaginaryPartPosition = n - 2;
            if (m_Length % 2 == 1)
            {
                lastImaginaryPartPosition = n - 1;
            }

            for (int j = 1; j < n; j += 2)
            {
                // iterative swaping of the last imaginary part (i.e. at position n-2 or n-1) until it is at position j
                for (int k = lastImaginaryPartPosition; k > j; k--)
                {
                    double temp = x[k];
                    x[k] = x[k - 1];
                    x[k - 1] = temp;
                }
            }

            x[1] = 0.0;
            if (m_Length % 2 == 0)
            {
                x[n - 1] = 0.0;  // be sure that the imaginary part of the last element is 0.0
            }
        }

        /// <summary>Converts Fourier coefficients from the Hermitian representation into CCS format. 
        /// </summary>
        /// <param name="x">The Hermitian representation, i.e. Re z_0, Re z_1, ..., -Im z_k, -Im z_{k-1},..., -Im z_1, where k=n/2 if <see cref="Length"/> is odd, otherwise k= n/2-1, thus <see cref="Length"/> 
        /// relevant elements. This implementation assumes a length of at least 2 * (<see cref="Length"/> / 2 + 1).</param>
        /// <param name="y">The CCS representation, i.e. Re z_0, Im z_0 (=.0.0), Re z_1, Im z_1, etc., thus 2 * (<see cref="Length"/> / 2 + 1) elements (output).</param>
        /// <param name="scalingFactor">The scaling factor.</param>
        /// <remarks>Use this method and a working array if the performance of <see cref="HermitianToCcsRepresentation(double[], double)"/> is to worse.</remarks>
        protected void HermitianToCcsRepresentation(double[] x, double[] y, double scalingFactor = 1.0)
        {
            int n = 2 * (m_Length / 2 + 1);

            y[0] = x[0];
            y[1] = 0.0;
            int inputImaginaryIndex = n - 2;  // ignore last element of the input!

            if (m_Length % 2 == 0)
            {
                inputImaginaryIndex = n - 3;
            }

            int inputRealIndex = 1;
            for (int j = 2; j < n; j += 2)
            {
                y[j] = scalingFactor * x[inputRealIndex++];
                y[j + 1] = scalingFactor * x[inputImaginaryIndex--];
            }
            if (m_Length % 2 == 0)
            {
                y[n - 1] = 0.0;  // be sure that the imaginary part of the last element is 0.0
            }
        }

        /// <summary>Converts Fourier coefficients from the CCS format into the Hermitian representation.
        /// </summary>
        /// <param name="x">An array of at least 2 * (<see cref="Length"/> / 2 + 1) elements representing Fourier coefficients. 
        /// CCS representation on input, i.e. Re z_0, Im z_0 (=.0.0), Re z_1, Im z_1, etc., thus 2 * (<see cref="Length"/> / 2 + 1) elements;
        /// on exit Hermitian representation, i.e. Re z_0, Re z_1, ..., -Im z_k, -Im z_{k-1},..., -Im z_1, where k=n/2 if <see cref="Length"/> is odd, otherwise k= n/2-1, thus <see cref="Length"/> relevant elements.</param>
        /// <param name="scalingFactor">The scaling factor.</param>
        protected void CcsToHermitianRepresentation(double[] x, double scalingFactor = 1.0)
        {
            int n = 2 * (m_Length / 2 + 1);

            BLAS.Level1.dscal(n / 2, -m_Length * scalingFactor / m_SqrtOfLength, x, increment: 2, startIndex: 1);  // scale the imaginary part

            int startImBlock = 1;
            int endImBlock = 1;

            x[0] *= m_Length * scalingFactor / m_SqrtOfLength;
            for (int j = 1; j < n / 2; j++)
            {
                /* move the first real-part after the block of imaginary parts at the current position:
                 */
                double temp1 = x[j];

                x[j] = x[endImBlock + 1] * m_Length * scalingFactor / m_SqrtOfLength;
                x[endImBlock + 1] = temp1;


                /* move the block of imaginary parts one step to the right, moverover one additional imaginary part is in the block: */
                startImBlock++;
                endImBlock += 2;

                /* move the imaginary part of the block of imaginary parts with the highest index at the first position inside the block: */
                for (int k = endImBlock; k > startImBlock; k--)
                {
                    double temp = x[k];
                    x[k] = x[k - 1];
                    x[k - 1] = temp;
                }

                /* if there is at least one additional real part move the imaginary part with index '0' (i.e. Im z_0) at the first position inside the block: */
                if (j < n / 2 - 1)
                {
                    for (int k = endImBlock; k > startImBlock; k--)
                    {
                        double temp = x[k];
                        x[k] = x[k - 1];
                        x[k - 1] = temp;
                    }
                }

            }
            /* If the length is even the Imaginary part Im z_k with the greates index k is zero, therefore we move it at the and: */
            if (m_Length % 2 == 0)
            {
                for (int k = startImBlock; k < endImBlock; k++)
                {
                    double temp = x[k];
                    x[k] = x[k + 1];
                    x[k + 1] = temp;
                }
            }
        }

        /// <summary>Converts Fourier coefficients from the CCS format into the Hermitian representation.
        /// </summary>
        /// <param name="x">The 2 * (<see cref="Length"/> /2 + 1) coefficients in CCS format, i.e. Re z_0, Im z_0 (=0.0), Re z_1, Im z_1,....</param>
        /// <param name="y">The <see cref="Length"/> coefficients in Hermitian format, i.e. Re z_0, Re z_1, ..., -Im z_k, -Im z_{k-1},.... (output).</param>
        /// <param name="scalingFactor">The scaling factor.</param>
        protected void CcsToHermitianRepresentation(double[] x, double[] y, double scalingFactor = 1.0)
        {
            int n = 2 * (m_Length / 2 + 1);

            BLAS.Level1.dscal(m_Length, 0.0, y);

            if (m_Length % 2 == 1)
            {
                BLAS.Level1.daxpy(n / 2, m_Length * scalingFactor / m_SqrtOfLength, x, y, 2, 1);  // copy real part
                BLAS.Level1.daxpy(m_Length - n / 2, -m_Length * scalingFactor / m_SqrtOfLength, x, y, -2, 1, n - 1, n / 2);  // use the complex conjugated

                /* the above code is equivalent to:
                 * 
                for (int j = 0; j < n / 2; j++)
                {
                    y[j] = m_Length * scalingFactor * x[2 * j] / m_SqrtOfLength; // copy real part
                }

                int inputImaginaryIndex = n - 1;  // ignore last element of the input!
                for (int j = n / 2; j < m_Length; j++)
                {
                    y[j] = -x[inputImaginaryIndex] * Length * scalingFactor / m_SqrtOfLength;  // complex conjugated ?
                    inputImaginaryIndex -= 2;
                }
                 * */
            }
            else
            {
                BLAS.Level1.daxpy(n / 2, m_Length * scalingFactor / m_SqrtOfLength, x, y, 2, 1);
                BLAS.Level1.daxpy(m_Length - n / 2, -m_Length * scalingFactor / m_SqrtOfLength, x, y, -2, 1, n - 3, n / 2);

                /* the above code is equivalent to:
               * 
              for (int j = 0; j < n / 2; j++)
              {
                  y[j] = m_Length * scalingFactor * x[2 * j] / m_SqrtOfLength; // copy real part
              }

              int inputImaginaryIndex = n - 3;  // last imaginary part is always 0.0, ignore it

              for (int j = n / 2; j < m_Length; j++)
              {
                  y[j] = -x[inputImaginaryIndex] * Length * scalingFactor / m_SqrtOfLength;  // complex conjugated ?
                  inputImaginaryIndex -= 2;
              }
              */
            }
        }
        #endregion
    }
}