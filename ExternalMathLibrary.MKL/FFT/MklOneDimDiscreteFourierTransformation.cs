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
using System.Runtime.InteropServices;

using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.Basics.LowLevel.Native
{
    /// <summary>Abstract class for one-dimensional Fourier Transformation methods with respect to Intel's MKL Library.
    /// </summary>
    internal abstract class MklOneDimDiscreteFourierTransformation : IDisposable, IOperable
    {
        #region private const members

        /// <summary>The precision is always equal to 'DFTI_DOUBLE', we do not use a single precision.
        /// </summary>
        private const int DFTI_DoublePrecision = 36;

        /// <summary>The parameter name for the scaling factor used for the forward transformation.
        /// </summary>
        private const int DFTI_FORWARD_SCALE = 4;

        /// <summary>The The parameter name for the scaling factor used for the backward transformation.
        /// </summary>
        private const int DFTI_BACKWARD_SCALE = 5;

        /// <summary>The parameter name for the property which indicates a in-place or out-of-place calculation. 
        /// </summary>
        private const int DFTI_PLACEMENT = 11;

        /// <summary>The magic number which represents some in-place calculation.
        /// </summary>
        private const int DFTI_INPLACE = 43;

        /// <summary>The magic number which represents some out-of-place calculation.
        /// </summary>
        private const int DFTI_NOT_INPLACE = 44;

        /// <summary>The parameter name for the commit status of the descriptor.
        /// </summary>
        private const int DFTI_COMMIT_STATUS = 22;

        /// <summary>The magic number which represents the status 'uncommitted'.
        /// </summary>
        private const int DFTI_UNCOMMITTED = 31;

        /// <summary>The error classe used for generating error messages.
        /// </summary>
        private const int DFTI_NO_ERROR = 0;
        #endregion

        #region private function import

        /// <summary>Create some MKL discrete Fourier descriptor.
        /// </summary>
        /// <param name="handle">The handle of the Fourier descriptor (output).</param>
        /// <param name="precision">The precision.</param>
        /// <param name="forwardDomain">The type of the forward domain.</param>
        /// <param name="dimension">The dimension.</param>
        /// <param name="lenght">The lenght.</param>
        /// <returns>An error code, <c>0</c> if no error occurs.</returns>
        [DllImport(MklNativeWrapper.dllName, CallingConvention = MklNativeWrapper.callingConvention, EntryPoint = "DftiCreateDescriptor", ExactSpelling = true)]
        private static extern int DftiCreateDescriptor(ref IntPtr handle, int precision, int forwardDomain, int dimension, int lenght);

        /// <summary>Frees memory allocated for a Fourier descriptor.
        /// </summary>
        /// <param name="handle">The handle of the Fourier descriptor.</param>
        /// <returns>An error code, <c>0</c> if no error occurs.</returns>
        [DllImport(MklNativeWrapper.dllName, CallingConvention = MklNativeWrapper.callingConvention, EntryPoint = "DftiFreeDescriptor", ExactSpelling = true)]
        private static extern int DftiFreeDescriptor(ref IntPtr handle);

        /// <summary>Sets one particular configuration parameter with the specified configuration value.
        /// </summary>
        /// <param name="handle">The handle of the Fourier descriptor.</param>
        /// <param name="configParam">The config parameter name.</param>
        /// <param name="configValue">The config value.</param>
        /// <returns>An error code, <c>0</c> if no error occurs.</returns>
        [DllImport(MklNativeWrapper.dllName, CallingConvention = MklNativeWrapper.callingConvention, EntryPoint = "DftiSetValue", ExactSpelling = true)]
        private static extern int DftiSetValue(IntPtr handle, int configParam, int configValue);

        /// <summary>Sets one particular configuration parameter with the specified configuration value.
        /// </summary>
        /// <param name="handle">The handle of the Fourier descriptor.</param>
        /// <param name="configParam">The config parameter name.</param>
        /// <param name="configValue">The config value.</param>
        /// <returns>An error code, <c>0</c> if no error occurs.</returns>
        [DllImport(MklNativeWrapper.dllName, CallingConvention = MklNativeWrapper.callingConvention, EntryPoint = "DftiSetValue", ExactSpelling = true)]
        private static extern int DftiSetValue(IntPtr handle, int configParam, double configValue);

        /// <summary>Gets the configuration value of one particular configuration parameter.
        /// </summary>
        /// <param name="handle">The handle of the Fourier descriptor.</param>
        /// <param name="configParam">The config parameter name.</param>
        /// <param name="configValue">The config value (output).</param>
        /// <returns>An error code, <c>0</c> if no error occurs.</returns>
        [DllImport(MklNativeWrapper.dllName, CallingConvention = MklNativeWrapper.callingConvention, EntryPoint = "DftiGetValue", ExactSpelling = true)]
        private static extern int DftiGetValue(IntPtr handle, int configParam, ref int configValue);

        /// <summary>Commits some MKL discrete Fourier descriptor.
        /// </summary>
        /// <param name="handle">The handle of the Fourier descriptor.</param>
        /// <returns>An error code, <c>0</c> if no error occurs.</returns>
        [DllImport(MklNativeWrapper.dllName, CallingConvention = MklNativeWrapper.callingConvention, EntryPoint = "DftiCommitDescriptor", ExactSpelling = true)]
        private static extern int DftiCommitDescriptor(IntPtr handle);

        /// <summary>Generates an error message.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns>The error message.</returns>
        [DllImport(MklNativeWrapper.dllName, CallingConvention = MklNativeWrapper.callingConvention, EntryPoint = "DftiErrorMessage", ExactSpelling = true)]
        private static extern string DftiErrorMessage(int status);

        /// <summary>Checks if the status reflects an error of a predefined class.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="errorClass">The error class.</param>
        /// <returns><c>false</c> if some <paramref name="status"/> represents some error state.</returns>
        [DllImport(MklNativeWrapper.dllName, CallingConvention = MklNativeWrapper.callingConvention, EntryPoint = "DftiErrorClass", ExactSpelling = true)]
        private static extern bool DftiErrorClass(int status, int errorClass);
        #endregion

        #region protected function import

        /// <summary>Computes the forward FFT.
        /// </summary>
        /// <param name="handle">The handle of the Fourier descriptor.</param>
        /// <param name="inputOutputdata">The input/output data.</param>
        /// <returns>An error code, <c>0</c> if no error occurs.</returns>
        [DllImport(MklNativeWrapper.dllName, CallingConvention = MklNativeWrapper.callingConvention, EntryPoint = "DftiComputeForward", ExactSpelling = true)]
        protected static extern int DftiComputeForward(IntPtr handle, IntPtr inputOutputdata);

        /// <summary>Computes the forward FFT.
        /// </summary>
        /// <param name="handle">The handle of the Fourier descriptor.</param>
        /// <param name="inputData">The input data.</param>
        /// <param name="ouputData">The output data.</param>
        /// <returns>An error code, <c>0</c> if no error occurs.</returns>
        [DllImport(MklNativeWrapper.dllName, CallingConvention = MklNativeWrapper.callingConvention, EntryPoint = "DftiComputeForward", ExactSpelling = true)]
        protected static extern int DftiComputeForward(IntPtr handle, IntPtr inputData, IntPtr ouputData);

        /// <summary>Computes the backward FFT.
        /// </summary>
        /// <param name="handle">The handle of the Fourier descriptor.</param>
        /// <param name="inputOutputData">The input/output data.</param>
        /// <returns>An error code, <c>0</c> if no error occurs.</returns>
        [DllImport(MklNativeWrapper.dllName, CallingConvention = MklNativeWrapper.callingConvention, EntryPoint = "DftiComputeBackward", ExactSpelling = true)]
        protected static extern int DftiComputeBackward(IntPtr handle, IntPtr inputOutputData);

        /// <summary>Computes the backward FFT.
        /// </summary>
        /// <param name="handle">The handle of the Fourier descriptor.</param>
        /// <param name="inputData">The input data.</param>
        /// <param name="outputData">The output data.</param>
        /// <returns>An error code, <c>0</c> if no error occurs.</returns>
        [DllImport(MklNativeWrapper.dllName, CallingConvention = MklNativeWrapper.callingConvention, EntryPoint = "DftiComputeBackward", ExactSpelling = true)]
        protected static extern int DftiComputeBackward(IntPtr handle, IntPtr inputData, IntPtr outputData);
        #endregion

        #region private members

        /// <summary>The length, i.e. the number of (complex or real) Fourier coefficients.
        /// </summary>
        private int m_Length;

        /// <summary>The handle, i.e. the pointer with respect to Intel's discrete Fourier descriptor structure.
        /// </summary>
        private IntPtr m_Handle = IntPtr.Zero;
        #endregion

        #region protected constructors

        /// <summary>Initializes a new instance of the <see cref="MklOneDimDiscreteFourierTransformation"/> class.
        /// </summary>
        /// <param name="domainType">Type of the (forward) domain, i.e. DFTI_COMPLEX = 32 or DFTI_REAL = 33.</param>
        /// <param name="length">The length, i.e. the number of Fourier coefficients.</param>
        /// <param name="dimension">The dimension.</param>
        protected MklOneDimDiscreteFourierTransformation(int domainType, int length, int dimension)
        {
            CheckErrorCode(DftiCreateDescriptor(ref m_Handle, DFTI_DoublePrecision, domainType, dimension, length), "DftiCreateDescriptor");
            m_Length = length;
        }
        #endregion

        #region destructor

        /// <summary>Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="MklOneDimDiscreteFourierTransformation"/> is reclaimed by garbage collection.
        /// </summary>
        ~MklOneDimDiscreteFourierTransformation()
        {
            Dispose();
        }
        #endregion

        #region protected properties

        /// <summary>Gets the handle, i.e. the pointer with respect to Intel's discrete Fourier descriptor structure.
        /// </summary>
        /// <value>The handle of the descriptor.</value>
        protected IntPtr Handle
        {
            get { return m_Handle; }
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
        /// 	<c>false</c> will be returned if the current instance is already disposed.
        /// </remarks>
        public bool IsOperable
        {
            get { return (m_Handle != IntPtr.Zero); }
        }
        #endregion

        /// <summary>Gets the length, i.e. the number of (complex or real) Fourier coefficients.
        /// </summary>
        /// <value>The length, i.e. the number of Fourier coefficients.</value>
        public int Length
        {
            get { return m_Length; }
        }
        #endregion

        #region public methods

        #region IDisposable Members

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (m_Handle != IntPtr.Zero)
            {
                DftiFreeDescriptor(ref m_Handle);
                m_Handle = IntPtr.Zero;
            }
        }
        #endregion

        #endregion

        #region protected methods

        /// <summary>Commits the discrete Fourier descriptor represented by the current instance.
        /// </summary>
        /// <remarks>Call this method before starting some forward or backward transformation.</remarks>
        protected void CommitDescriptor()
        {
            // do a commit if and only if the current instance is uncommitted:
            int commitedState = 0;
            CheckErrorCode(DftiGetValue(m_Handle, DFTI_COMMIT_STATUS, ref commitedState), "DftiGetValue");
            if (commitedState == DFTI_UNCOMMITTED)
            {
                CheckErrorCode(DftiCommitDescriptor(m_Handle), "DftiCommitDescriptor");
            }
        }

        /// <summary>Sets the scaling factor used for some forward transformation.
        /// </summary>
        /// <param name="scalingFactor">The scaling factor.</param>
        protected void SetForwardScalingFactor(double scalingFactor)
        {
            CheckErrorCode(DftiSetValue(m_Handle, DFTI_FORWARD_SCALE, scalingFactor), "DftiSetValue");
        }

        /// <summary>Sets the scaling factor used for some backward transformation.
        /// </summary>
        /// <param name="scalingFactor">The scaling factor.</param>
        protected void SetBackwardScalingFactor(double scalingFactor)
        {
            CheckErrorCode(DftiSetValue(m_Handle, DFTI_BACKWARD_SCALE, scalingFactor), "DftiSetValue");
        }

        /// <summary>Prepares the current instance for some in-place calculation.
        /// </summary>
        protected void PrepareInPlaceCalculation()
        {
            CheckErrorCode(DftiSetValue(m_Handle, DFTI_PLACEMENT, DFTI_INPLACE), "DftiSetValue");
        }

        /// <summary>Prepares the current instance for some out-of-place calculation.
        /// </summary>
        protected void PrepareOutOfPlaceCalculation()
        {
            CheckErrorCode(DftiSetValue(m_Handle, DFTI_PLACEMENT, DFTI_NOT_INPLACE), "DftiSetValue");
        }

        /// <summary>Checks some MKL error code.
        /// </summary>
        /// <param name="errorCode">The error code, <c>0</c> represents 'no error'.</param>
        /// <param name="functionName">The name of the MKL function, needed perhaps for the exception message.</param>
        /// <exception cref="InvalidOperationException">Thrown, if <paramref name="errorCode"/> is != 0.</exception>
        protected void CheckErrorCode(int errorCode, string functionName)
        {
            if (errorCode != 0)  // execution is not successful
            {
                if (!DftiErrorClass(errorCode, DFTI_NO_ERROR))
                {
                    throw new InvalidOperationException("MKL: Return value '" + errorCode + "' in " + functionName + ": {" + DftiErrorMessage(errorCode) + "}.");
                }
                else
                {
                    throw new InvalidOperationException("MKL: Return value '" + errorCode + "' in " + functionName + ".");
                }
            }
        }
        #endregion
    }
}