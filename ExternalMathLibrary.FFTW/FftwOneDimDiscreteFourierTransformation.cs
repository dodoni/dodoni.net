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

using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.Basics.LowLevel.Native
{
    /// <summary>Serves as abstract wrapper for one-dimensional Fourier methods defined in FFTW 3.3.x library.
    /// </summary>
    /// <remarks>The FFTW library does not supports output scaling, here we use BLAS for this part.
    /// <para>
    /// With respect to §4.6 New-array Execute Functions in the FFTW 3.3.x library documentation,
    /// we do not use 'fftw_malloc' and thus for each calculation we have to create a new plan.
    /// </para></remarks>
    internal abstract class FftwOneDimDiscreteFourierTransformation : IDisposable, IOperable
    {
        #region private/protected const members

        /// <summary>The sign of the exponent in the formula that defines the Fourier transform, i.e. <c>-1</c>.
        /// </summary>
        protected const int FFTW_FORWARD = -1;

        /// <summary>The sign of the exponent in the formula that defines the Fourier transform, i.e. <c>1</c>.
        /// </summary>
        protected const int FFTW_BACKWARD = 1;

        /// <summary>The Planning-rigor flag which specifies that, instead of actual measurements of different 
        /// algorithms, a simple heuristic is used to pick a (probably sub-optimal) plan quickly. With this 
        /// flag, the input/output arrays are not overwritten during planning.
        /// </summary>
        private const uint FFTW_ESTIMATE = 1u << 6;
        #endregion

        #region private function import

        /// <summary>Executes the plan, to compute the corresponding transform on the arrays for which it
        /// was planned (which must still exist). The <paramref name="plan"/> is not modified, and this method can be called 
        /// as many times as desired.
        /// </summary>
        /// <param name="plan">The plan.</param>
        [DllImport(FftwNativeWrapper.dllName, CallingConvention = FftwNativeWrapper.callingConvention, EntryPoint = "fftw_execute", ExactSpelling = true)]
        private static extern void fftw_execute(IntPtr plan);

        /// <summary>Deallocates a specific <paramref name="plan"/> and all its associated data.
        /// </summary>
        /// <param name="plan">The plan.</param>
        [DllImport(FftwNativeWrapper.dllName, CallingConvention = FftwNativeWrapper.callingConvention, EntryPoint = "fftw_destroy_plan", ExactSpelling = true)]
        private static extern void fftw_destroy_plan(IntPtr plan);

        /// <summary>Creates a plan which represents a one-dimensional Fourier transformation.
        /// </summary>
        /// <param name="n">The number of Fourier coefficients.</param>
        /// <param name="input">The pointer to the input array of the transform, which may be the
        /// same as the <paramref name="output"/> (yielding an in-place transforminput.</param>
        /// <param name="output">The pointer to the output array of the transform.</param>
        /// <param name="sign">The sign, i.e. <c>-1</c> for a forward transform and <c>1</c> for a backward transform.</param>
        /// <param name="flags">The flags, here we use <see cref="FFTW_ESTIMATE"/> only.</param>
        /// <returns>The plain, i.e. a pointer with respect to the Fourier transform.</returns>
        [DllImport(FftwNativeWrapper.dllName, CallingConvention = FftwNativeWrapper.callingConvention, EntryPoint = "fftw_plan_dft_1d", ExactSpelling = true)]
        private static extern IntPtr fftw_plan_dft_1d(int n, IntPtr input, IntPtr output, int sign, uint flags);

        /// <summary>Creates a plan which represents a one-dimensional real data forward Fourier transformation.
        /// </summary>
        /// <param name="n">The "logical" number of Fourier coefficients.</param>
        /// <param name="input">The pointer to the real input array.</param>
        /// <param name="output">The pointer to the complex output, i.e. the complex-Hermitian output.</param>
        /// <param name="flags">The flags, here we use <see cref="FFTW_ESTIMATE"/> only.</param>
        /// <returns>The plain, i.e. a pointer with respect to the Fourier transform.</returns>
        [DllImport(FftwNativeWrapper.dllName, CallingConvention = FftwNativeWrapper.callingConvention, EntryPoint = "fftw_plan_dft_r2c_1d", ExactSpelling = true)]
        private static extern IntPtr fftw_plan_dft_r2c_1d(int n, IntPtr input, IntPtr output, uint flags);

        /// <summary>Creates a plan which represents a one-dimensional real data backward Fourier transformation.
        /// </summary>
        /// <param name="n">The "logical" number of Fourier coefficients.</param>
        /// <param name="input">The pointer to the complex input, i.e. complex-Hermitian input.</param>
        /// <param name="output">The pointer to the real output array.</param>
        /// <param name="flags">The flags, here we use <see cref="FFTW_ESTIMATE"/> only.</param>
        /// <returns>The plain, i.e. a pointer with respect to the Fourier transform.</returns>
        [DllImport(FftwNativeWrapper.dllName, CallingConvention = FftwNativeWrapper.callingConvention, EntryPoint = "fftw_plan_dft_c2r_1d", ExactSpelling = true)]
        private static extern IntPtr fftw_plan_dft_c2r_1d(int n, IntPtr input, IntPtr output, uint flags);
        #endregion

        #region private/protected members

        /// <summary>The length, i.e. the number of (complex or real) Fourier coefficients.
        /// </summary>
        private int m_Length;

        /// <summary>The handle, i.e. the pointer with respect to the FFTW plan structure.
        /// </summary>
        private IntPtr m_Handle = IntPtr.Zero;
        #endregion

        #region protected constructors

        /// <summary>Initializes a new instance of the <see cref="FftwOneDimDiscreteFourierTransformation"/> class.
        /// </summary>
        /// <param name="length">The length, i.e. the number of Fourier coefficients.</param>
        protected FftwOneDimDiscreteFourierTransformation(int length)
        {
            m_Length = length;
        }
        #endregion

        #region destructor

        /// <summary>Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="FftwOneDimDiscreteFourierTransformation"/> is reclaimed by garbage collection.
        /// </summary>
        ~FftwOneDimDiscreteFourierTransformation()
        {
            Dispose();
        }
        #endregion

        #region protected properties

        /// <summary>Gets the handle, i.e. the pointer with respect to the plan, i.e. FFTW's discrete Fourier descriptor structure.
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
        /// 	<c>true</c> will be returned in any case, even if the current instance is already disposed.
        /// </remarks>
        public bool IsOperable
        {
            get { return true; }
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
                fftw_destroy_plan(m_Handle);
                m_Handle = IntPtr.Zero;
            }
        }
        #endregion

        #endregion

        #region protected methods

        /// <summary>Execudes the internal FFTW's plan structure which represents the current Fourier transformation.
        /// </summary>
        protected void ExecudePlan()
        {
            fftw_execute(m_Handle);
        }

        /// <summary>Creates a specific one-dimensional discrete Fourier FFTW plan. 
        /// </summary>
        /// <param name="input">The input array of the transform, which may be the
        /// same as the <paramref name="output"/> (yielding an in-place transforminput.</param>
        /// <param name="output">The output array of the transform.</param>
        /// <param name="sign">The sign, i.e. <c>-1</c> for a forward transform and <c>1</c> for a backward transform.</param>
        /// <remarks>The property <see cref="Handle"/> will be set to the new plain, i.e. a pointer with respect to the 
        /// Fourier transform.</remarks>
        protected void CreateOneDimPlan(Complex[] input, Complex[] output, int sign)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            if (input.Length < m_Length)
            {
                throw new ArgumentException("input");
            }
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }
            if (output.Length < m_Length)
            {
                throw new ArgumentException("output");
            }
            // first store the old plan, create a new one and destory the old one later
            // here we use order because of performance reason, see §4.6, FFTW documentation:
            // "If possible, it is probably better for you to simply create multiple plans (creating a new
            // plan is quick once one exists for a given size), or better yet re-use the same array for your transforms."

            IntPtr oldPlan = m_Handle;

            unsafe
            {
                fixed (Complex* inputPtr = input, outputPtr = output)
                {
                    m_Handle = fftw_plan_dft_1d(m_Length, (IntPtr)inputPtr, (IntPtr)outputPtr, sign, FFTW_ESTIMATE);
                    if (m_Handle == IntPtr.Zero)
                    {
                        throw new InvalidOperationException("FFTW: Can not create plan.");
                    }
                }
            }
            if (oldPlan != IntPtr.Zero)
            {
                fftw_destroy_plan(oldPlan);
            }
        }

        /// <summary>Creates a specific FFTW plan for a real-data forward transformation, i.e. the 
        /// input are real numbers and the output are complex numbers with Hermitian symmetry.
        /// </summary>
        /// <param name="input">The input, i.e. real numbers.</param>
        /// <param name="output">The output, i.e. complex numbers with Hermitian symmetry stored in the CCE format.</param>
        protected void CreateOneDimRealDataForwardPlan(double[] input, double[] output)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            if (input.Length < m_Length)
            {
                throw new ArgumentException("input");
            }
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }
            if (output.Length < m_Length)
            {
                throw new ArgumentException("output");
            }

            unsafe
            {
                fixed (double* inputPtr = input, outputPtr = output)
                {
                    IntPtr oldPlan = m_Handle;

                    m_Handle = fftw_plan_dft_r2c_1d(m_Length, (IntPtr)inputPtr, (IntPtr)outputPtr, FFTW_ESTIMATE);
                    if (m_Handle == IntPtr.Zero)
                    {
                        throw new InvalidOperationException("FFTW: Can not create plan.");
                    }
                    if (oldPlan != IntPtr.Zero)
                    {
                        fftw_destroy_plan(oldPlan);
                    }
                }
            }
        }

        /// <summary>Creates a specific FFTW plan for a real-data backward transformation, i.e. the 
        /// input are complex numbers with Hermitian symmetry and the output are real numbers.
        /// </summary>
        /// <param name="input">The input, i.e. complex numbers with Hermitian symmetry stored in the CCE format.</param>
        /// <param name="output">The output, i.e. real numbers.</param>
        protected void CreateOneDimRealDataBackwardPlan(double[] input, double[] output)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            if (input.Length < m_Length)
            {
                throw new ArgumentException("input");
            }
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }
            if (output.Length < m_Length)
            {
                throw new ArgumentException("output");
            }

            unsafe
            {
                fixed (double* inputPtr = input, outputPtr = output)
                {
                    IntPtr oldPlan = m_Handle;

                    m_Handle = fftw_plan_dft_c2r_1d(m_Length, (IntPtr)inputPtr, (IntPtr)outputPtr, FFTW_ESTIMATE);
                    if (m_Handle == IntPtr.Zero)
                    {
                        throw new InvalidOperationException("FFTW: Can not create plan.");
                    }
                    if (oldPlan != IntPtr.Zero)
                    {
                        fftw_destroy_plan(oldPlan);
                    }
                }
            }
        }
        #endregion
    }
}