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
using Dodoni.BasicComponents.Utilities;
using Dodoni.MathLibrary.Basics.LowLevel.Native;

namespace Dodoni.MathLibrary.ProbabilityTheory.MonteCarloEngine
{
    /// <summary>Provides settings for Random Number Sequences with respect to AMD's ACML Library.
    /// </summary>
    public partial class AcmlRandomNumberSequence
    {
        /// <summary>Represents the Random Number sequence generation with respect to a specified distribution.
        /// </summary>
        public class Distribution : RandomNumberSequence.IDistribution
        {
            #region private function import

            [DllImport(AcmlNativeWrapper.dllName, EntryPoint = "DRANDUNIFORM", ExactSpelling = true, CallingConvention = AcmlNativeWrapper.callingConvention)]
            private static extern void _acml_UniformNumbers(ref int n, ref double minimum, ref double maximum, int[] state, double[] values, ref int info);

            [DllImport(AcmlNativeWrapper.dllName, EntryPoint = "DRANDGAUSSIAN", ExactSpelling = true, CallingConvention = AcmlNativeWrapper.callingConvention)]
            private static extern void _acml_GaussianNumbers(ref int n, ref double mean, ref double variance, int[] state, double[] values, ref int info);

            [DllImport(AcmlNativeWrapper.dllName, EntryPoint = "DRANDMULTINORMAL", ExactSpelling = true, CallingConvention = AcmlNativeWrapper.callingConvention)]
            private static extern void _acml_MultiNormalNumbers(ref int n, ref int dimension, double[] mean, double[] c, ref int ldc, int[] state, double[] x, ref int ldx, ref int info);
            #endregion

            #region private members

            /// <summary>The random number stream.
            /// </summary>
            private AcmlRandomNumberStream m_RandomNumberStream;
            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="Distribution"/> class.
            /// </summary>
            /// <param name="randomNumberStream">The random number stream.</param>
            internal Distribution(AcmlRandomNumberStream randomNumberStream)
            {
                m_RandomNumberStream = randomNumberStream;
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
                int errorCode = 0;
                _acml_UniformNumbers(ref n, ref a, ref b, m_RandomNumberStream.m_State, data, ref errorCode);
                if (errorCode != 0) // execution is not successful
                {
                    throw new InvalidOperationException("ACML: Return value " + errorCode + " in DRANDUNIFORM.");
                }
            }

            /// <summary>Gets the [optional] generator modes for the uniform distribution.
            /// </summary>
            /// <value>The uniform distribution generator modes.</value>
            public IEnumerable<RandomNumberSequence.UniformGenerationMode> UniformModes
            {
                get { yield return UniformGenerationMode.Standard; }
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
                int errorCode = 0;
                double variance = sigma * sigma;

                _acml_GaussianNumbers(ref n, ref a, ref variance, m_RandomNumberStream.m_State, data, ref errorCode);
                if (errorCode != 0) // execution is not successful
                {
                    throw new InvalidOperationException("ACML: Return value " + errorCode + " in DRANDGAUSSIAN.");
                }
            }

            /// <summary>Gets the [optional] Gaussian distribution generator modes.
            /// </summary>
            /// <value>The Gaussian distribution generator modes.</value>
            public IEnumerable<RandomNumberSequence.GaussianGenerationMode> GaussianModes
            {
                get { yield return GaussianGenerationMode.Standard; }
            }

            /// <summary>Gets the length of the workspace array required for optimal performance of <see cref="GaussianMultivariate(int, double[], int, RandomNumberSequence.MultivariateMatrixStorageType, double[], double[], double[], RandomNumberSequence.GaussianGenerationMode)"/>.
            /// </summary>
            /// <param name="n">The number of random values to generate.</param>
            /// <param name="dimension">The dimension of the output vectors.</param>
            /// <param name="matrixStorage">The matrix storage schema.</param>
            /// <param name="generationMethod">The optional generation mode.</param>
            /// <returns>The length of the workspace array required for the generation of random numbers following a multivariate normal distribution, perhaps <c>0</c>.</returns>
            public int GaussianMultivariateWorkspaceQuery(int n, int dimension, RandomNumberSequence.MultivariateMatrixStorageType matrixStorage, RandomNumberSequence.GaussianGenerationMode generationMethod = null)
            {
                return dimension * dimension;
            }

            /// <summary>Gets a value indicating the matrix contain random vectors following a multivariate normal distribution, i.e. the representation of the outcome of
            /// <see cref="RandomNumberSequence.IDistribution.GaussianMultivariate(int, double[], int, RandomNumberSequence.MultivariateMatrixStorageType, double[], double[], double[], RandomNumberSequence.GaussianGenerationMode)"/>.
            /// </summary>
            /// <value>The representation of the outcome of a multivariate normal distribution random number generation.</value>
            /// <remarks>The return value of <see cref="RandomNumberSequence.IDistribution.GaussianMultivariate(int, double[], int, RandomNumberSequence.MultivariateMatrixStorageType, double[], double[], double[], RandomNumberSequence.GaussianGenerationMode)"/> should
            /// be identical to this property.</remarks>
            public BLAS.MatrixTransposeState GaussianMultivariateDataRepresentation
            {
                get { return BLAS.MatrixTransposeState.Transpose; }
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
                // for this implementation one has to calculate the covariance matrix:
                if ((workspace == null) || (workspace.Length < dimension * dimension))
                {
                    workspace = new double[dimension * dimension];
                }

                switch (matrixStorage)
                {
                    case RandomNumberSequence.MultivariateMatrixStorageType.Full:
                        BLAS.Level3.dgemm(dimension, dimension, dimension, 1.0, pseudoRootOfVarianceCovarianceMatrix, pseudoRootOfVarianceCovarianceMatrix, 0.0, workspace, BLAS.MatrixTransposeState.Transpose);
                        break;

                    case RandomNumberSequence.MultivariateMatrixStorageType.Diagonal:
                        BLAS.Level1.dscal(dimension * dimension, 0.0, workspace);  // set coVarianceMatrix = 0
                        for (int j = 0; j < dimension; j++)  // the input is just the diagonal matrix (column-by-column representation)
                        {
                            workspace[j * dimension + j] = pseudoRootOfVarianceCovarianceMatrix[j] * pseudoRootOfVarianceCovarianceMatrix[j];
                        }
                        break;

                    case RandomNumberSequence.MultivariateMatrixStorageType.TriangularPackaged:
                        int startIndexMatrixQ = 0; // the null-based index of the first index [in packaged representation] of matrix Q w.r.t. to the (i,j)-component of Q*Q'

                        for (int i = 0; i < dimension; i++)
                        {
                            startIndexMatrixQ += i;

                            for (int j = 0; j <= i; j++)
                            {
                                /* j is the start index of Matrix Q' and one has to calculate j + 1 multiplications: */
                                double value = 0.0;
                                
                                for (int k = 0; k < j+1; k++)
                                {
                                    value += pseudoRootOfVarianceCovarianceMatrix[startIndexMatrixQ + k] * pseudoRootOfVarianceCovarianceMatrix[j + k];
                                }
                                workspace[i + j * dimension] = workspace[j + i * dimension] = value;
                            }
                        }
                        break;

                    default:
                        throw new ArgumentException(matrixStorage.ToFormatString());
                }
                int errorCode = 0;

                _acml_MultiNormalNumbers(ref n, ref dimension, mu, workspace, ref dimension, m_RandomNumberStream.m_State, data, ref n, ref errorCode);
                if (errorCode != 0) // execution is not successful
                {
                    throw new InvalidOperationException("ACML: Return value " + errorCode + " in DRANDMULTINORMAL.");
                }
                return BLAS.MatrixTransposeState.Transpose;
            }
            #endregion

            /// <summary>Generates random numbers following a multivariate normal distribution N_d(\mu, \Sigma), i.e. d-dimensional vectors, where \Sigma = Q * Q' represents the variance-covariance matrix.
            /// </summary>
            /// <param name="n">The number of random values to generate.</param>
            /// <param name="data">The <paramref name="n"/> random numbers following a multivariate normal distribution, i.e. random vectors of size at least <paramref name="dimension"/> * <paramref name="n"/>,
            /// where the i-th block of <paramref name="n"/> elements contains the realisations of the i-th component of the random vector (output).</param>
            /// <param name="dimension">The dimension of the output vectors.</param>
            /// <param name="mu">The mean vector of dimension <paramref name="dimension" />.</param>
            /// <param name="varianceCovarianceMatrix">The variance covariance matrix supplied column-by-column.</param>
            /// <param name="generationMethod">The optional generation mode.</param>
            public void GaussianMultivariate(int n, double[] data, int dimension, double[] mu, double[] varianceCovarianceMatrix, RandomNumberSequence.GaussianGenerationMode generationMethod = null)
            {              
                int errorCode = 0;

                _acml_MultiNormalNumbers(ref n, ref dimension, mu, varianceCovarianceMatrix, ref dimension, m_RandomNumberStream.m_State, data, ref n, ref errorCode);
                if (errorCode != 0) // execution is not successful
                {
                    throw new InvalidOperationException("ACML: Return value " + errorCode + " in DRANDMULTINORMAL.");
                }
            }
        }
    }
}