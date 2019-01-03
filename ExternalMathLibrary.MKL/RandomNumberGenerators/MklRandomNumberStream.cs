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

using Dodoni.MathLibrary.Basics.LowLevel.Native;

namespace Dodoni.MathLibrary.ProbabilityTheory.MonteCarloEngine
{
    /// <summary>Represents Intel's VSL stream state pointer.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal class MklRandomNumberStream : IRandomNumberStream
    {
        #region private function import

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vslDeleteStream", ExactSpelling = true, CallingConvention = MklNativeWrapper.callingConvention)]
        internal static extern int vslDeleteStream(ref IntPtr stream);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vslCopyStream", ExactSpelling = true, CallingConvention = MklNativeWrapper.callingConvention)]
        private static extern int vslCopyStream(out IntPtr newStream, IntPtr sourceStream);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vslCopyStreamState", ExactSpelling = true, CallingConvention = MklNativeWrapper.callingConvention)]
        private static extern int vslCopyStreamState(IntPtr destinationStream, IntPtr sourceStream);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vslLeapfrogStream", ExactSpelling = true, CallingConvention = MklNativeWrapper.callingConvention)]
        private static extern int vslLeapfrogStream(IntPtr stream, int k, int nstreams);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vslSkipAheadStream", ExactSpelling = true, CallingConvention = MklNativeWrapper.callingConvention)]
        private static extern int vslSkipAheadStream(IntPtr stream, long nskip);   // Important! The argument 'nskip' is a long integer!!!
        #endregion

        #region private/internal members

        /// <summary>The handle, i.e. the adress of the VSL stream state structure.
        /// </summary>
        /// <remarks>This member is internal and not private because of the Fortran VSL interface which supports parameters by reference only.</remarks>
        internal IntPtr m_Handle = IntPtr.Zero;

        /// <summary>The Random Number Generator related to the random stream.
        /// </summary>
        private IRandomNumberGenerator m_Generator;

        /// <summary>The sub-generator ID (relevant for Wichmann-Hill and MT2203 Random Number Generators only).
        /// </summary>
        private int m_SubGeneratorID;

        /// <summary>The sequence generator with respect to a specified distribution.
        /// </summary>
        private RandomNumberSequence.IDistribution m_Distribution;

        /// <summary>The dimension of a specified Quasi-Random Number Generator, if <see cref="Generator"/> represents a Quasi-Random Number Generator; otherwise <c>1</c>.
        /// </summary>
        private long m_Dimension;

        /// <summary>The 'nstreams' argument for Quasi-Random-Number Generators applied for Leapfrog approach.
        /// </summary>
        private const int VSL_QRNG_LEAPFROG_COMPONENTS = 0x7FFFFFFF;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="MklRandomNumberStream"/> class.
        /// </summary>
        /// <param name="streamHandle">The stream handle.</param>
        /// <param name="randomNumberGenerator">The Random Number Generator related to the random stream.</param>
        /// <param name="dimension">The dimension.</param>
        /// <param name="subGeneratorID">A sub-generator ID if a set of similar Random Number Generator is represented by the calling Random Number Generator instance (relevant for Wichmann-Hill and MT2203 only).</param>
        internal MklRandomNumberStream(IntPtr streamHandle, IRandomNumberGenerator randomNumberGenerator, long dimension = 1, int subGeneratorID = 0)
        {
            m_Handle = streamHandle;
            m_Generator = randomNumberGenerator;
            m_Dimension = dimension;
            m_SubGeneratorID = subGeneratorID;
            m_Distribution = new MklRandomNumberSequence.Distribution(this);
        }
        #endregion

        #region destructor

        /// <summary>Releases unmanaged resources and performs other cleanup operations before the <see cref="MklRandomNumberStream"/> is reclaimed by garbage collection.
        /// </summary>
        ~MklRandomNumberStream()
        {
            if (m_Handle != IntPtr.Zero)
            {
                vslDeleteStream(ref m_Handle);
                m_Handle = IntPtr.Zero;
            }
        }
        #endregion

        #region public properties

        #region IOperable Members

        /// <summary>Gets a value indicating whether this instance is operable.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is operable; otherwise, <c>false</c>.
        /// </value>
        public bool IsOperable
        {
            get { return (m_Handle != IntPtr.Zero); }
        }
        #endregion

        #region IRandomNumberStream Members

        /// <summary>Gets the Random Number Generator related to the random stream.
        /// </summary>
        /// <value>The Random Number Generator.</value>
        public IRandomNumberGenerator Generator
        {
            get { return m_Generator; }
        }

        /// <summary>Gets a (the next) sequence of random numbers with respect to a specified distribution.
        /// </summary>
        /// <value>Random numbers with respect to a specified distribution.</value>
        /// <remarks>In general random numbers will not be cached, i.e. each access to this property yields other random numbers.</remarks>
        public RandomNumberSequence.IDistribution NextNumberSequence
        {
            get { return m_Distribution; }
        }

        /// <summary>Gets a value indicating whether splitting of a random number sequence into non-overlapping subsequences is supported.
        /// </summary>
        /// <value>The splitting approach.</value>
        public RandomNumberSequence.SplittingApproach SplittingApproach
        {
            get { return m_Generator.SplittingApproach; }
        }

        /// <summary>Gets the dimension of the sequences, i.e. the dimension of the Quasi-Random Number Generator or <c>1</c> if the
        /// current instance represents a Pseudo-Random Number Generator.
        /// </summary>
        /// <value>The dimension of a Quasi-Random Number Generator or <c>1</c>.</value>
        public long Dimension
        {
            get { return m_Dimension; }
        }
        #endregion

        #endregion

        #region public methods

        #region IRandomNumberStream Members

        /// <summary>Creates a (deep) copy of the current random stream object.
        /// </summary>
        /// <returns>A (deep) copy of the current instance.</returns>
        public IRandomNumberStream CopyStream()
        {
            IntPtr streamHandle;
            int errorCode = vslCopyStream(out streamHandle, m_Handle);
            if (errorCode != 0) // execution is not successful
            {
                throw new InvalidOperationException("MKL: Return value " + errorCode + " in vslCopyStream.");
            }
            return new MklRandomNumberStream(streamHandle, m_Generator, m_Dimension);
        }

        /// <summary>Overrides the stream state data of a specific stream object with the data of the current object.
        /// </summary>
        /// <param name="destinationStream">The destination random number stream.</param>
        /// <exception cref="ArgumentException">Thrown, if the stream of the current instance and the stream represented by <paramref name="destinationStream"/> are not compatible.</exception>
        /// <remarks>Stream state data are for example the dimension etc.</remarks>
        public void CopyStreamStateTo(IRandomNumberStream destinationStream)
        {
            if (destinationStream == null)
            {
                throw new ArgumentNullException("destinationStream");
            }

            MklRandomNumberStream destinationVslRandomStream = destinationStream as MklRandomNumberStream;
            if (destinationVslRandomStream == null)
            {
                throw new ArgumentException("destinationStream");
            }

            int errorCode = vslCopyStreamState(destinationVslRandomStream.m_Handle, m_Handle);
            if (errorCode != 0) // execution is not successful
            {
                throw new ArgumentException("MKL: Return value " + errorCode + " in vslCopyStreamState.", "destinationStream");
            }
            destinationVslRandomStream.m_Dimension = m_Dimension;
            destinationVslRandomStream.m_Generator = m_Generator;
        }

        /// <summary>Creates a copy of the current <see cref="IRandomNumberStream"/> object and apply Leapfrog approach to it.
        /// </summary>
        /// <param name="streamIndex">The null-based index of the computational node (stream index), 0 &lt;= <paramref name="streamIndex"/> &lt; <paramref name="numberOfStreams"/>.</param>
        /// <param name="numberOfStreams">The number of streams, i.e. largest number of computational nodes, or stride. In the case of a Quasi-Random Number Generator this parameter should be equal to the dimension.</param>
        /// <returns>A (deep) copy of the current instance which applies the Leapfrog approach.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown, if the current object does not support the Leapfrog method or the arguments are invalid.</exception>
        /// <remarks>This feature is particularly useful in distributing random numbers from the original stream across the <paramref name="numberOfStreams"/> buffers. One of the important applications
        /// of the Leapfrog method is splitting the original sequence into non-overlapping subsequences across <paramref name="numberOfStreams"/> computational nodes.
        /// For Quasi-Random Number Generators, the Leapfrog method allows generating individual components of Quasi-Random Vectors instead of whole
        /// Quasi-Random Vectors. In this case <paramref name="numberOfStreams"/> should be equal to the dimension of the Quasi-Random Vector.
        /// <para>The Leapfrog method is supported only when a Random Number Generator provides a more efficient implementation than generation of the full sequence to pick out a required subsequence.</para>
        /// </remarks>
        public IRandomNumberStream CreateLeapfrogStream(int streamIndex, int numberOfStreams)
        {
            IntPtr streamHandle;
            int errorCode = vslCopyStream(out streamHandle, m_Handle);
            if (errorCode != 0) // execution is not successful
            {
                throw new InvalidOperationException("MKL: Return value " + errorCode + " in vslCopyStream.");
            }

            /* Sobol and Niederreiter use a specified 'numberOfStreams' argument! (see Intel's VSL documentation) */
            if (m_Generator is IQuasiRandomNumberGenerator)
            {
                numberOfStreams = VSL_QRNG_LEAPFROG_COMPONENTS;
            }

            errorCode = vslLeapfrogStream(streamHandle, streamIndex, numberOfStreams);
            if (errorCode != 0) // execution is not successful
            {
                throw new InvalidOperationException("MKL: Return value " + errorCode + " in vslLeapfrogStream.");
            }
            return new MklRandomNumberStream(streamHandle, m_Generator, m_Dimension);
        }

        /// <summary>Creates a copy of the current <see cref="IRandomNumberStream"/> object and apply Block-splitting approach to it.
        /// </summary>
        /// <param name="numberOfSkippedElements">The number of skipped elements.</param>
        /// <returns>A (deep) copy of the current instance which applies the Block-splitting approach.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown, if the current object does not support the Block-split approach or the arguments are invalid.</exception>
        /// <remarks>This method skips a specified number of elements in a random stream. This feature is particularly useful in distributing random numbers from
        /// original random stream across different computational nodes.
        /// If the largest number of random numbers used by a computational node is <paramref name="numberOfSkippedElements"/>, then the original random sequence
        /// may be split into non-overlapping blocks of <paramref name="numberOfSkippedElements"/> size so that each block corresponds to the respective computational node.
        /// The number of computational nodes is unlimited.
        /// <para>
        /// Note that for Quasi-Random Number Generators the Skip-ahead method works with components of Quasi-Random Vectors rather than with whole Quasi-Random Vectors.
        /// Therefore, to skip <c>n</c> Quasi-Random Vectors, set <paramref name="numberOfSkippedElements"/> to <c>n</c> * <see cref="IRandomNumberStream.Dimension"/>.
        /// If this operation results in exceeding the period of the Quasi-Random Number Generator, an exception will be thrown.
        /// </para>
        /// </remarks>
        public IRandomNumberStream CreateSkipAheadStream(int numberOfSkippedElements)
        {
            IntPtr streamHandle;
            int errorCode = vslCopyStream(out streamHandle, m_Handle);
            if (errorCode != 0) // execution is not successful
            {
                throw new InvalidOperationException("MKL: Return value " + errorCode + " in vslCopyStream.");
            }

            errorCode = vslSkipAheadStream(streamHandle, numberOfSkippedElements);
            if (errorCode != 0) // execution is not successful
            {
                throw new InvalidOperationException("MKL: Return value " + errorCode + " in vslSkipAheadStream.");
            }
            return new MklRandomNumberStream(streamHandle, m_Generator, m_Dimension);
        }
        #endregion

        #region IDisposable Members

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (m_Handle != IntPtr.Zero)
            {
                vslDeleteStream(ref m_Handle);
                m_Handle = IntPtr.Zero;
            }
        }
        #endregion

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            if (m_SubGeneratorID == 0)
            {
                return String.Format("Random Number Stream: {0}", m_Generator.Name.String);
            }
            return String.Format("Random Number Stream: {0}_ {1}", m_Generator.Name.String, m_SubGeneratorID);
        }
        #endregion
    }
}