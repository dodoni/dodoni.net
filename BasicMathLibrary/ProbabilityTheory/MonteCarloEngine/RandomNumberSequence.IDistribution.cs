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
using System.Collections.Generic;

using Dodoni.MathLibrary.Basics;

namespace Dodoni.MathLibrary.ProbabilityTheory.MonteCarloEngine
{
    /// <summary>Provides interfaces and structures etc. for handling Random Number sequences.
    /// </summary>
    public partial class RandomNumberSequence
    {
        /// <summary>Provides methods to produce random numbers following a specified distribution.
        /// </summary>
        public interface IDistribution
        {
            /// <summary>Generate random numbers following a uniform distribution on a closed interval [a,b].
            /// </summary>
            /// <param name="n">The number of random values to generate.</param>
            /// <param name="data">The <paramref name="n"/> random numbers uniformly distributed over the interval [<paramref name="a"/>,<paramref name="b"/>] (output).</param>
            /// <param name="a">The left bound 'a'.</param>
            /// <param name="b">The right bound 'b'.</param>
            /// <param name="generationMode">The optional generation mode.</param>
            void Uniform(int n, double[] data, double a = 0.0, double b = 1.0, UniformGenerationMode generationMode = null);

            /// <summary>Gets the [optional] generator modes for the uniform distribution.
            /// </summary>
            /// <value>The uniform distribution generator modes.</value>
            IEnumerable<UniformGenerationMode> UniformModes
            {
                get;
            }

            /// <summary>Generate random numbers following a normal distribution N(a,\sigma^2).
            /// </summary>
            /// <param name="n">The number of random values to generate.</param>
            /// <param name="data">The <paramref name="n"/> random numbers following a normal distribution (output).</param>
            /// <param name="a">The mean.</param>
            /// <param name="sigma">The standard deviation.</param>
            /// <param name="generationMethod">The optional generation mode.</param>
            void Gaussian(int n, double[] data, double a = 0.0, double sigma = 1.0, GaussianGenerationMode generationMethod = null);

            /// <summary>Gets the [optional] Gaussian distribution generator modes.
            /// </summary>
            /// <value>The Gaussian distribution generator modes.</value>
            IEnumerable<GaussianGenerationMode> GaussianModes
            {
                get;
            }

            /// <summary>Gets a value indicating the matrix contain random vectors following a multivariate normal distribution, i.e. the representation of the outcome of
            /// <see cref="RandomNumberSequence.IDistribution.GaussianMultivariate(int, double[], int, RandomNumberSequence.MultivariateMatrixStorageType, double[], double[], double[], RandomNumberSequence.GaussianGenerationMode)"/>.
            /// </summary>
            /// <value>The representation of the outcome of a multivariate normal distribution random number generation.</value>
            /// <remarks>The return value of <see cref="RandomNumberSequence.IDistribution.GaussianMultivariate(int, double[], int, RandomNumberSequence.MultivariateMatrixStorageType, double[], double[], double[], RandomNumberSequence.GaussianGenerationMode)"/> should
            /// be identical to this property.</remarks>
            BLAS.MatrixTransposeState GaussianMultivariateDataRepresentation
            {
                get;
            }

            /// <summary>Gets the length of the workspace array required for optimal performance of <see cref="GaussianMultivariate(int, double[], int, RandomNumberSequence.MultivariateMatrixStorageType, double[], double[], double[], RandomNumberSequence.GaussianGenerationMode)"/>.
            /// </summary>
            /// <param name="n">The number of random values to generate.</param>
            /// <param name="dimension">The dimension of the output vectors.</param>
            /// <param name="matrixStorage">The matrix storage schema.</param>
            /// <param name="generationMethod">The optional generation mode.</param>
            /// <returns>The length of the workspace array required for the generation of random numbers following a multivariate normal distribution, perhaps <c>0</c>.</returns>
            int GaussianMultivariateWorkspaceQuery(int n, int dimension, RandomNumberSequence.MultivariateMatrixStorageType matrixStorage, RandomNumberSequence.GaussianGenerationMode generationMethod = null);

            /// <summary>Generates random numbers following a multivariate normal distribution N_d(\mu, \Sigma), i.e. d-dimensional vectors, where \Sigma = Q * Q' represents the variance-covariance matrix.
            /// </summary>
            /// <param name="n">The number of random values to generate.</param>
            /// <param name="data">The <paramref name="n"/> random numbers following a multivariate normal distribution, i.e. random vectors of size at least <paramref name="dimension"/> * <paramref name="n"/>. The return value
            /// as well as <see cref="GaussianMultivariateDataRepresentation"/> indicates the representation of the data (output).</param>
            /// <param name="dimension">The dimension of the output vectors.</param>
            /// <param name="matrixStorage">The matrix storage schema of <paramref name="pseudoRootOfVarianceCovarianceMatrix"/>.</param>
            /// <param name="mu">The mean vector of dimension <paramref name="dimension"/>.</param>
            /// <param name="pseudoRootOfVarianceCovarianceMatrix">The elements of the upper triangular matrix Q' passed according to the matrix storage scheme <paramref name="matrixStorage"/>, where \Sigma = Q * Q' represents the variance-covariance matrix.</param>
            /// <param name="workspace">A workspace array, perhaps <c>null</c>.</param>
            /// <param name="generationMethod">The optional generation mode.</param>
            /// <returns>The representation of the <paramref name="data"/>; the random vectors (of dimension <paramref name="dimension"/>) are stored column-by-column if <see cref="BLAS.MatrixTransposeState.NoTranspose"/>; otherwise each column contains all realisation of the j'th component (i.e. transposed matrix).</returns>
            BLAS.MatrixTransposeState GaussianMultivariate(int n, double[] data, int dimension, RandomNumberSequence.MultivariateMatrixStorageType matrixStorage, double[] mu, double[] pseudoRootOfVarianceCovarianceMatrix, double[] workspace = null, RandomNumberSequence.GaussianGenerationMode generationMethod = null);
        }
    }
}