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
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Dodoni.MathLibrary.Basics;
using Dodoni.MathLibrary.Basics.LowLevel.Native;

namespace Dodoni.MathLibrary.ProbabilityTheory.MonteCarloEngine
{
    /// <summary>Provides settings for Random Number Sequences with respect to Intel's MKL Library.
    /// </summary>
    public partial class MklRandomNumberSequence
    {
        /// <summary>Provides methods to produce random numbers following a specified distribution.
        /// </summary>
        /// <remarks>For multivariate distributions or more general functions involving matrix functionality, we use the Fortran interface of the MKL Library.
        /// </remarks>
        internal class Distribution : RandomNumberSequence.IDistribution
        {
            #region  private function import

            [DllImport(MklNativeWrapper.dllName, CallingConvention = MklNativeWrapper.callingConvention, EntryPoint = "vdRngUniform", ExactSpelling = true, SetLastError = true)]
            private static extern int vdRngUniform(int method, IntPtr stream, int n, double[] r, double a, double b);

            [DllImport(MklNativeWrapper.dllName, CallingConvention = MklNativeWrapper.callingConvention, EntryPoint = "vdRngGaussian", ExactSpelling = true)]
            private static extern int vdRngGaussian(int method, IntPtr stream, int n, double[] r, double a, double sigma);

            [DllImport(MklNativeWrapper.dllName, CallingConvention = MklNativeWrapper.callingConvention, EntryPoint = "vdrnggaussianmv", ExactSpelling = true)]
            private static extern int vdRngGaussianMV(ref int method, ref IntPtr stream, ref int n, IntPtr r, ref int dimension, ref int mstorage, IntPtr a, IntPtr t);
            #endregion

            #region private members

            /// <summary>The random number stream.
            /// </summary>
            private MklRandomNumberStream m_Stream;
            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="Distribution" /> class.
            /// </summary>
            /// <param name="randomNumberStream">The random number stream.</param>
            internal Distribution(MklRandomNumberStream randomNumberStream)
            {
                m_Stream = randomNumberStream;
            }
            #endregion

            #region IDistribution Members

            /// <summary>Generate random numbers following a uniform distribution on a closed interval [a,b].
            /// </summary>
            /// <param name="n">The number of random values to generate.</param>
            /// <param name="data">The <paramref name="n" /> random numbers uniformly distributed over the interval [<paramref name="a" />,<paramref name="b" />] (output).</param>
            /// <param name="a">The left bound 'a'.</param>
            /// <param name="b">The right bound 'b'.</param>
            /// <param name="generationMode">The optional generation mode.</param>
            public void Uniform(int n, double[] data, double a, double b, RandomNumberSequence.UniformGenerationMode generationMode = null)
            {
                int errorCode;
                if (generationMode != null)
                {
                    if (generationMode is MklRandomNumberSequence.UniformGenerationMode)
                    {
                        var vslGenerationMode = (MklRandomNumberSequence.UniformGenerationMode)generationMode;
                        errorCode = vdRngUniform(vslGenerationMode.MagicNumber, m_Stream.m_Handle, n, data, a, b);
                    }
                    else
                    {
                        throw new ArgumentException("Generation mode is not a valid argument", "generationMode");
                    }
                }
                else  // use the standard method
                {
                    errorCode = vdRngUniform(UniformGenerationMode.Standard.MagicNumber, m_Stream.m_Handle, n, data, a, b);
                }

                if (errorCode != 0) // execution is not successful
                {
                    throw new InvalidOperationException("MKL: Return value " + errorCode + " in vdRngUniform.");
                }
            }

            /// <summary>Gets the [optional] generator modes for the uniform distribution.
            /// </summary>
            /// <value>The uniform distribution generator modes.
            /// </value>
            public IEnumerable<RandomNumberSequence.UniformGenerationMode> UniformModes
            {
                get
                {
                    yield return MklRandomNumberSequence.UniformGenerationMode.Standard;
                    yield return MklRandomNumberSequence.UniformGenerationMode.Accurate;
                }
            }

            /// <summary>Generate random numbers following a normal distribution N(a,\sigma^2).
            /// </summary>
            /// <param name="n">The number of random values to generate.</param>
            /// <param name="data">The <paramref name="n" /> random numbers following a normal distribution (output).</param>
            /// <param name="a">The mean.</param>
            /// <param name="sigma">The standard deviation.</param>
            /// <param name="generationMethod">The optional generation mode.</param>
            public void Gaussian(int n, double[] data, double a, double sigma, RandomNumberSequence.GaussianGenerationMode generationMethod = null)
            {
                int errorCode;
                if (generationMethod != null)
                {
                    if (generationMethod is MklRandomNumberSequence.GaussianGenerationMode)
                    {
                        var vslGaussianGenerationMode = (MklRandomNumberSequence.GaussianGenerationMode)generationMethod;

                        errorCode = vdRngGaussian(vslGaussianGenerationMode.MagicNumber, m_Stream.m_Handle, n, data, a, sigma);
                    }
                    else
                    {
                        throw new ArgumentException("Generation mode is not a valid argument", "generationMode");
                    }
                }
                else
                {
                    errorCode = vdRngGaussian(MklRandomNumberSequence.GaussianGenerationMode.InverseCumulativeDistributionFunction.MagicNumber, m_Stream.m_Handle, n, data, a, sigma);
                }
                if (errorCode != 0) // execution is not successful
                {
                    throw new InvalidOperationException("MKL: Return value " + errorCode + " in vdRngGaussian.");
                }
            }

            /// <summary>Gets the [optional] Gaussian distribution generator modes.
            /// </summary>
            /// <value>The Gaussian distribution generator modes.
            /// </value>
            public IEnumerable<RandomNumberSequence.GaussianGenerationMode> GaussianModes
            {
                get
                {
                    yield return MklRandomNumberSequence.GaussianGenerationMode.BoxMuller;
                    yield return MklRandomNumberSequence.GaussianGenerationMode.BoxMuller2;
                    yield return MklRandomNumberSequence.GaussianGenerationMode.InverseCumulativeDistributionFunction;
                }
            }

            /// <summary>Gets the length of the workspace array required for optimal performance of <see cref="GaussianMultivariate(int, double[], int, RandomNumberSequence.MultivariateMatrixStorageType, double[], double[], double[], RandomNumberSequence.GaussianGenerationMode)" />.
            /// </summary>
            /// <param name="n">The number of random values to generate.</param>
            /// <param name="dimension">The dimension of the output vectors.</param>
            /// <param name="matrixStorage">The matrix storage schema.</param>
            /// <param name="generationMethod">The optional generation mode.</param>
            /// <returns>The length of the workspace array required for the generation of random numbers following a multivariate normal distribution, perhaps <c>0</c>.</returns>
            public int GaussianMultivariateWorkspaceQuery(int n, int dimension, RandomNumberSequence.MultivariateMatrixStorageType matrixStorage, RandomNumberSequence.GaussianGenerationMode generationMethod = null)
            {
                return 0;
            }

            /// <summary>Gets a value indicating the matrix contain random vectors following a multivariate normal distribution, i.e. the representation of the outcome of
            /// <see cref="RandomNumberSequence.IDistribution.GaussianMultivariate(int, double[], int, RandomNumberSequence.MultivariateMatrixStorageType, double[], double[], double[], RandomNumberSequence.GaussianGenerationMode)"/>.
            /// </summary>
            /// <value>The representation of the outcome of a multivariate normal distribution random number generation.</value>
            /// <remarks>The return value of <see cref="RandomNumberSequence.IDistribution.GaussianMultivariate(int, double[], int, RandomNumberSequence.MultivariateMatrixStorageType, double[], double[], double[], RandomNumberSequence.GaussianGenerationMode)"/> should
            /// be identical to this property.</remarks>
            public BLAS.MatrixTransposeState GaussianMultivariateDataRepresentation
            {
                get { return BLAS.MatrixTransposeState.NoTranspose; }
            }

            /// <summary>Generates random numbers following a multivariate normal distribution N_d(\mu, \Sigma), i.e. d-dimensional vectors, where \Sigma = Q * Q' represents the variance-covariance matrix.
            /// </summary>
            /// <param name="n">The number of random values to generate.</param>
            /// <param name="data">The <paramref name="n"/> random numbers following a multivariate normal distribution, i.e. random vectors of size at least <paramref name="dimension"/> * <paramref name="n"/>. The return value
            /// as well as <see cref="GaussianMultivariateDataRepresentation"/> indicates the representation of the data (output).</param>
            /// <param name="dimension">The dimension of the output vectors.</param>
            /// <param name="matrixStorage">The matrix storage schema of <paramref name="pseudoRootOfVarianceCovarianceMatrix" />.</param>
            /// <param name="mu">The mean vector of dimension <paramref name="dimension" />.</param>
            /// <param name="pseudoRootOfVarianceCovarianceMatrix">The elements of the upper triangular matrix Q' passed according to the matrix storage scheme <paramref name="matrixStorage" />, where \Sigma = Q * Q' represents the variance-covariance matrix.</param>
            /// <param name="workspace">A workspace array, perhaps <c>null</c>.</param>
            /// <param name="generationMethod">The optional generation mode.</param>
            /// <returns>The representation of the <paramref name="data"/>; the random vectors (of dimension <paramref name="dimension"/>) are stored column-by-column if <see cref="BLAS.MatrixTransposeState.NoTranspose"/>; otherwise each column contains all realisation of the j'th component (i.e. transposed matrix).</returns>
            public BLAS.MatrixTransposeState GaussianMultivariate(int n, double[] data, int dimension, RandomNumberSequence.MultivariateMatrixStorageType matrixStorage, double[] mu, double[] pseudoRootOfVarianceCovarianceMatrix, double[] workspace = null, RandomNumberSequence.GaussianGenerationMode generationMethod = null)
            {
                int method;

                if (generationMethod != null)
                {
                    if (generationMethod is MklRandomNumberSequence.GaussianGenerationMode)
                    {
                        var vslGaussianGenerationMode = (MklRandomNumberSequence.GaussianGenerationMode)generationMethod;
                        method = vslGaussianGenerationMode.MagicNumber;
                    }
                    else
                    {
                        throw new ArgumentException("Generation mode is not a valid argument", "generationMode");
                    }
                }
                else
                {
                    method = GaussianGenerationMode.InverseCumulativeDistributionFunction.MagicNumber;
                }

                int errorCode;
                int storage = (int)matrixStorage;
                unsafe
                {
                    fixed (double* dataPtr = data, muPtr = mu, pseudoRootPtr = pseudoRootOfVarianceCovarianceMatrix)
                    {
                        errorCode = vdRngGaussianMV(ref method, ref m_Stream.m_Handle, ref n, (IntPtr)dataPtr, ref dimension, ref storage, (IntPtr)muPtr, (IntPtr)pseudoRootPtr);
                    }
                }

                if (errorCode != 0) // execution is not successful
                {
                    throw new InvalidOperationException("MKL: Return value " + errorCode + " in vdRngGaussianmv.");
                }
                return BLAS.MatrixTransposeState.NoTranspose;
            }
            #endregion
        }
    }
}