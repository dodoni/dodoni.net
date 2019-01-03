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
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using NUnit.Framework;

namespace Dodoni.MathLibrary.ProbabilityTheory.MonteCarloEngine
{
    /// <summary>Serves as abstract unit test class for <see cref="IRandomNumberStream"/> objects.
    /// </summary>
    /// <remarks>In this implementation we calculate the estimated mean and compare it to the expectation only.</remarks>
    public abstract class RandomNumberStreamTests
    {
        #region Uniform distribution

        /// <summary>A test function for random numbers following a uniform distribution.
        /// </summary>
        /// <param name="sampleSize">The size of the sample.</param>
        /// <param name="a">The left bound of the distribution.</param>
        /// <param name="b">The right bound of the distribution.</param>
        /// <param name="generatorMode">The generator mode.</param>
        [TestCaseSource(nameof(NextNumberSequenceUniformTestCaseData))]
        public void NextNumberSequenceUniform_EstimatedMean_ConvidenceInterval(int sampleSize, double a, double b, RandomNumberSequence.UniformGenerationMode generatorMode)
        {
            double expectedMean = (a + b) / 2.0;
            double expectedStandardDeviation = (b - a) / (2.0 * Math.Sqrt(3.0));

            var sample = new double[sampleSize];
            IRandomNumberStream randomStream = GetRandomStream();
            randomStream.NextNumberSequence.Uniform(sampleSize, sample, a, b, generatorMode);

            double estimatedMean = sample.Average();
            double estimatedStandardDeviation = GetEstimatedStandardDeviation(sample, sampleSize, estimatedMean);
            double epsilon = 2.0 * estimatedStandardDeviation / Math.Sqrt(sampleSize); // size of the confidence interval w.r.t. normal distribution N{-1}(0.975) \approx 1.96 \approx 2.0, i.e. \alpha/2 quantile with \alpha = 5% 

            /* test whether E(X) \in [ estimatedMean - \epsilon, estimatedMean + \epsilon], where \epsilon = \sigma/\sqrt(n} */
            Assert.That((expectedMean >= estimatedMean - epsilon) && (expectedMean <= estimatedMean + epsilon), String.Format("a: {0}, b: {1}, Expectation: {2}, Standard deviation: {3}, estimated mean: {4}, estimated Standard Deviation: {5}, epsilon: {6}", a, b, expectedMean, expectedStandardDeviation, estimatedMean, estimatedStandardDeviation, epsilon));
        }

        /// <summary>A test function for random numbers following a uniform distribution.
        /// </summary>
        /// <param name="sampleSize">The size of the sample.</param>
        /// <param name="a">The left bound of the distribution.</param>
        /// <param name="b">The right bound of the distribution.</param>
        /// <param name="generatorMode">The generator mode.</param>
        [TestCaseSource(nameof(NextNumberSequenceUniformTestCaseData))]
        public void NextNumberSequenceUniform_LeapfrogEstimatedMean_ConvidenceInterval(int sampleSize, double a, double b, RandomNumberSequence.UniformGenerationMode generatorMode)
        {
            double expectedMean = (a + b) / 2.0;
            double expectedStandardDeviation = (b - a) / (2.0 * Math.Sqrt(3.0));

            IRandomNumberStream randomStream = GetRandomStream();
            Assume.That(randomStream.SplittingApproach.HasFlag(RandomNumberSequence.SplittingApproach.LeapFrog), String.Format("Random Number Generator {0} does not support Leapfrog approach.", randomStream.ToString()));

            int numberOfStreams = 5;  // use five sub-streams for Pseudo-Random-Number Generators
            if (randomStream.Generator is IQuasiRandomNumberGenerator)
            {
                numberOfStreams = (int)randomStream.Dimension;
            }
            var sample = new List<double>(sampleSize);
            for (int j = 0; j < numberOfStreams; j++)
            {
                var stream = randomStream.CreateLeapfrogStream(j, numberOfStreams);

                int subSampleSize = sampleSize / numberOfStreams;
                var subSample = new double[subSampleSize];
                stream.NextNumberSequence.Uniform(subSampleSize, subSample, a, b, generatorMode);

                sample.AddRange(subSample);
            }
            /* compute estimated mean and standard deviation: */
            double estimatedMean = sample.Average();
            double estimatedStandardDeviation = GetEstimatedStandardDeviation(sample.ToArray(), sampleSize, estimatedMean);
            double epsilon = 2.0 * estimatedStandardDeviation / Math.Sqrt(sampleSize); // size of the confidence interval w.r.t. normal distribution N{-1}(0.975) \approx 1.96 \approx 2.0, i.e. \alpha/2 quantile with \alpha = 5% 

            /* test whether E(X) \in [ estimatedMean - \epsilon, estimatedMean + \epsilon], where \epsilon = \sigma/\sqrt(n} */
            Assert.That((expectedMean >= estimatedMean - epsilon) && (expectedMean <= estimatedMean + epsilon), String.Format("a: {0}, b: {1}, Expectation: {2}, Standard deviation: {3}, estimated mean: {4}, estimated Standard Deviation: {5}, epsilon: {6}", a, b, expectedMean, expectedStandardDeviation, estimatedMean, estimatedStandardDeviation, epsilon));
        }

        /// <summary>A test function for random numbers following a uniform distribution.
        /// </summary>
        /// <param name="sampleSize">The size of the sample.</param>
        /// <param name="a">The left bound of the distribution.</param>
        /// <param name="b">The right bound of the distribution.</param>
        /// <param name="generatorMode">The generator mode.</param>
        [TestCaseSource(nameof(NextNumberSequenceUniformTestCaseData))]
        public void NextNumberSequenceUniform_SkipAheadEstimatedMean_ConvidenceInterval(int sampleSize, double a, double b, RandomNumberSequence.UniformGenerationMode generatorMode)
        {
            double expectedMean = (a + b) / 2.0;
            double expectedStandardDeviation = (b - a) / (2.0 * Math.Sqrt(3.0));

            IRandomNumberStream randomStream = GetRandomStream();
            Assume.That(randomStream.SplittingApproach.HasFlag(RandomNumberSequence.SplittingApproach.SkipAhead), String.Format("Random Number Generator {0} does not support Leapfrog approach.", randomStream.ToString()));

            var sample = new List<double>();
            int numberOfStreams = 2;
            int nskip = sampleSize * numberOfStreams;

            for (int j = 0; j < numberOfStreams; j++)
            {
                var stream = randomStream.CreateSkipAheadStream(j * nskip);

                int subSampleSize = sampleSize / numberOfStreams;
                var subSample = new double[subSampleSize];
                stream.NextNumberSequence.Uniform(subSampleSize, subSample, a, b, generatorMode);

                sample.AddRange(subSample);
            }
            sampleSize = sample.Count;

            /* alternativly, use: */
            /*
            var stream1 = randomStream.CreateSkipAheadStream(5);
            var stream2 = stream1.CreateSkipAheadStream(5);
            var stream3 = stream2.CreateSkipAheadStream(5);

            int subSampleSize = sampleSize / 3;
            var sample1 = new double[subSampleSize];
            stream1.NextNumberSequence.Uniform(subSampleSize, sample1, a, b, generatorMode);

            var sample2 = new double[subSampleSize];
            stream2.NextNumberSequence.Uniform(subSampleSize, sample2, a, b, generatorMode);

            var sample3 = new double[subSampleSize];
            stream3.NextNumberSequence.Uniform(subSampleSize, sample3, a, b, generatorMode);

            var sample = new List<double>();
            sample.AddRange(sample1);
            sample.AddRange(sample2);
            sample.AddRange(sample3);
            */

            /* compute estimated mean and standard deviation: */
            double estimatedMean = sample.Average();
            double estimatedStandardDeviation = GetEstimatedStandardDeviation(sample.ToArray(), sampleSize, estimatedMean);
            double epsilon = 2.0 * estimatedStandardDeviation / Math.Sqrt(sampleSize); // size of the confidence interval w.r.t. normal distribution N{-1}(0.975) \approx 1.96 \approx 2.0, i.e. \alpha/2 quantile with \alpha = 5% 

            /* 
             * Even for a fixed seed the results of the random sampling are not identically (for Intel's MKL Library)! 
             * 
             * Therefore we enlarge the convidence interval - this unit test is not part of a test suite for Random Number Generators. 
             * We focus on the integration with the native Dll.
             */
            epsilon *= 2.0;  // special adjustment, in most cases not necessary

            /* test whether E(X) \in [ estimatedMean - \epsilon, estimatedMean + \epsilon], where \epsilon = \sigma/\sqrt(n} */
            Assert.That(estimatedMean - epsilon, Is.LessThanOrEqualTo(expectedMean), String.Format("a: {0}, b: {1}, Expectation: {2}, Standard deviation: {3}, estimated mean: {4}, estimated Standard Deviation: {5}, epsilon: {6}", a, b, expectedMean, expectedStandardDeviation, estimatedMean, estimatedStandardDeviation, epsilon));
            Assert.That(estimatedMean + epsilon, Is.GreaterThanOrEqualTo(expectedMean), String.Format("a: {0}, b: {1}, Expectation: {2}, Standard deviation: {3}, estimated mean: {4}, estimated Standard Deviation: {5}, epsilon: {6}", a, b, expectedMean, expectedStandardDeviation, estimatedMean, estimatedStandardDeviation, epsilon));
        }

        /// <summary>Gets the test case data for generation of random numbers following a uniform distribution.
        /// </summary>
        /// <value>The test case data for generation of random numbers following a uniform distribution.</value>
        public IEnumerable<TestCaseData> NextNumberSequenceUniformTestCaseData
        {
            get
            {
                foreach (var generatorMode in GetRandomStream().NextNumberSequence.UniformModes)
                {
                    yield return new TestCaseData(5000, 0, 1, generatorMode);
                    yield return new TestCaseData(12000, 2, 6, generatorMode);
                }
            }
        }
        #endregion

        #region Gaussian (normal) distribution

        /// <summary>A test function for random numbers following a normal distribution.
        /// </summary>
        /// <param name="sampleSize">The size of the sample.</param>
        /// <param name="a">The mean of the normal distribution.</param>
        /// <param name="sigma">The Standard deviation of the normal distribution.</param>
        /// <param name="generatorMode">The generator mode.</param>
        [TestCaseSource(nameof(NextNumberSequenceGaussianMomentTestCaseData))]
        public void NextNumberSequenceGaussian_EstimatedMean_ConvidenceInterval(int sampleSize, double a, double sigma, RandomNumberSequence.GaussianGenerationMode generatorMode)
        {
            double expectedMean = a;

            var sample = new double[sampleSize];
            IRandomNumberStream randomStream = GetRandomStream();

            randomStream.NextNumberSequence.Gaussian(sampleSize, sample, a, sigma, generatorMode);

            double estimatedMean = sample.Average();
            double estimatedStandardDeviation = GetEstimatedStandardDeviation(sample, sampleSize, estimatedMean);
            double epsilon = 2.0 * estimatedStandardDeviation / Math.Sqrt(sampleSize); // size of the confidence interval w.r.t. normal distribution N{-1}(0.975) \approx 1.96 \approx 2.0

            /* test whether E(X) \in [ estimatedMean - \epsilon, estimatedMean + \epsilon], where \epsilon = \sigma/\sqrt(n} */
            Assert.That(estimatedMean - epsilon, Is.LessThanOrEqualTo(expectedMean), String.Format("a: {0}, sigma: {1}, estimated mean: {2}, estimated Standard Deviation: {3}, epsilon: {4}", a, sigma, estimatedMean, estimatedStandardDeviation, epsilon));
            Assert.That(estimatedMean + epsilon, Is.GreaterThanOrEqualTo(expectedMean), String.Format("a: {0}, sigma: {1}, estimated mean: {2}, estimated Standard Deviation: {3}, epsilon: {4}", a, sigma, estimatedMean, estimatedStandardDeviation, epsilon));
        }

        /// <summary>A test function for random numbers following a normal distribution.
        /// </summary>
        /// <param name="sampleSize">The size of the sample.</param>
        /// <param name="a">The mean of the normal distribution.</param>
        /// <param name="sigma">The Standard deviation of the normal distribution.</param>
        /// <param name="generatorMode">The generator mode.</param>
        [TestCaseSource(nameof(NextNumberSequenceGaussianMomentTestCaseData))]
        public void NextNumberSequenceGaussian_EstimatedSecondMoment_ConvidenceInterval(int sampleSize, double a, double sigma, RandomNumberSequence.GaussianGenerationMode generatorMode)
        {
            double expectedSecondMoment = sigma * sigma + a * a;

            var sample = new double[sampleSize];
            IRandomNumberStream randomStream = GetRandomStream();

            randomStream.NextNumberSequence.Gaussian(sampleSize, sample, a, sigma, generatorMode);
            sample = sample.Select(x => x * x).ToArray();

            double estimatedSecondMoment = sample.Average();
            double estimatedStandardDeviation = GetEstimatedStandardDeviation(sample, sampleSize, estimatedSecondMoment);
            double epsilon = 2.0 * estimatedStandardDeviation / Math.Sqrt(sampleSize); // size of the confidence interval w.r.t. normal distribution N{-1}(0.975) \approx 1.96 \approx 2.00           

            /* test whether E(X^2) \in [ estimatedMoment - \epsilon, estimatedMoment + \epsilon], where \epsilon = \sigma/\sqrt(n} */
            Assert.That(estimatedSecondMoment - epsilon, Is.LessThanOrEqualTo(expectedSecondMoment), String.Format("a: {0}; sigma: {1}; 2nd Moment: {2}; estimated 2nd Moment: {3}; estimated Standard Deviation: {4}; epsilon: {5}", a, sigma, sigma * sigma + a * a, estimatedSecondMoment, estimatedStandardDeviation, epsilon));
            Assert.That(estimatedSecondMoment + epsilon, Is.GreaterThanOrEqualTo(expectedSecondMoment), String.Format("a: {0}; sigma: {1}; 2nd Moment: {2}; estimated 2nd Moment: {3}; estimated Standard Deviation: {4}; epsilon: {5}", a, sigma, sigma * sigma + a * a, estimatedSecondMoment, estimatedStandardDeviation, epsilon));
        }

        /// <summary>Gets the test case data for generation of random numbers following a normal distribution.
        /// </summary>
        /// <value>The test case data for generation of random numbers following a normal distribution.</value>
        public IEnumerable<TestCaseData> NextNumberSequenceGaussianMomentTestCaseData
        {
            get
            {
                foreach (var generatorMode in GetRandomStream().NextNumberSequence.GaussianModes)
                {
                    yield return new TestCaseData(5000, 0, 1, generatorMode);
                    yield return new TestCaseData(12000, 1.4, 0.65, generatorMode);
                }
            }
        }

        /// <summary>A test function for random numbers following a normal distribution.
        /// </summary>
        /// <param name="sampleSize">The size of the sample.</param>
        /// <param name="a">The mean of the normal distribution.</param>
        /// <param name="sigma">The Standard deviation of the normal distribution.</param>
        /// <param name="lowerCriticalValueOfChiSquaredDistribution">\chi^2_{\alpha/2, n-1}, i.e. the quantile of the Chi-Squared distribution with degree of freedom = <paramref name="sampleSize"/> -1 and \alpha = 0.05.</param>
        /// <param name="upperCriticalValueOfChiSquaredDistribution">\chi^2_{1.0 - \alpha/2, n-1}, i.e. the quantile of the Chi-Squared distribution with degree of freedom = <paramref name="sampleSize"/> -1 and \alpha = 0.05.</param>
        /// <param name="generatorMode">The generator mode.</param>
        /// <remarks>The quantiles of the Chi-Squared distribution can be calculated with R, MathLab, AlgLib etc.</remarks>
        [TestCaseSource(nameof(NextNumberSequenceGaussianVarianceTestCaseData))]
        public void NextNumberSequenceGaussian_EstimateVariance_ConvidenceInterval(int sampleSize, double a, double sigma, double lowerCriticalValueOfChiSquaredDistribution, double upperCriticalValueOfChiSquaredDistribution, RandomNumberSequence.GaussianGenerationMode generatorMode)
        {
            var sample = new double[sampleSize];
            IRandomNumberStream randomStream = GetRandomStream();
            randomStream.NextNumberSequence.Gaussian(sampleSize, sample, a, sigma, generatorMode);

            double estimatedMean = sample.Average();
            double estimatedVariance = GetEstimatedVariance(sample, sampleSize, estimatedMean);

            /* check whether the (theoretical) variance is inside 
             * 
             * [ (n-1) * S^2 / \chi^2_{\alpha/2, n-1}, (n-1)*S^2 / \chi^2_{1.0 - \alpha/2, n-1} ], 
             * 
             * where \chi^2_{\alpha, n} is the quantile of the chi-square distribution with degree n and parameter \alpha. 
             */
            double expectedVariance = sigma * sigma;

            Assert.That((sampleSize - 1) * estimatedVariance / upperCriticalValueOfChiSquaredDistribution, Is.GreaterThanOrEqualTo(expectedVariance), String.Format("a: {0}, sigma: {1}, sampleSize: {2}, theor. Variance: {3}, estimated mean: {4}, estimated Variance: {5}, upper-Chi Squared quantile: {6}", a, sigma, sampleSize, expectedVariance, estimatedMean, estimatedVariance, upperCriticalValueOfChiSquaredDistribution));
            Assert.That((sampleSize - 1) * estimatedVariance / lowerCriticalValueOfChiSquaredDistribution, Is.LessThanOrEqualTo(expectedVariance), String.Format("a: {0}, sigma: {1}, sampleSize: {2}, theor. Variance: {3}, estimated mean: {4}, estimated Variance: {5}, lower-Chi Squared quantile: {6}", a, sigma, sampleSize, expectedVariance, estimatedMean, estimatedVariance, lowerCriticalValueOfChiSquaredDistribution));
        }

        /// <summary>Gets the test case data for generation of random numbers following a normal distribution.
        /// </summary>
        /// <value>The test case data for generation of random numbers following a normal distribution.</value>
        public IEnumerable<TestCaseData> NextNumberSequenceGaussianVarianceTestCaseData
        {
            get
            {
                foreach (var generatorMode in GetRandomStream().NextNumberSequence.GaussianModes)
                {
                    yield return new TestCaseData(5000, 0, 1, 5196.86, 4804.92, generatorMode);
                    yield return new TestCaseData(12000, 1.4, 0.85, 12304.51, 11697.275, generatorMode);
                    yield return new TestCaseData(25000, 0, 1, 25439.143, 24562.64, generatorMode);
                }
            }
        }
        #endregion

        #region Multivariate Gaussian (Normal) distribution

        /// <summary>A test function that generates random numbers following a multivariate normal distribution.
        /// </summary>
        /// <param name="dimension">The dimension.</param>
        /// <param name="sampleSize">The size of the sample.</param>
        /// <param name="mu">The mean, i.e. of dimension <paramref name="dimension"/>.</param>
        /// <param name="pseudoRootOfVarianceCovarianceMatrix">The pseudo-root of variance covariance matrix.</param>
        /// <param name="matrixStorageType">The type of the matrix storage of <paramref name="pseudoRootOfVarianceCovarianceMatrix"/>.</param>
        /// <param name="generatorMode">The generator mode.</param>
        [TestCaseSource(nameof(NextNumberSequenceGaussianMultivariateMeanTestCaseData))]
        public void NextNumberSequenceGaussianMultivariate_EstimatedMean_ConvidenceInterval(int dimension, int sampleSize, double[] mu, double[] pseudoRootOfVarianceCovarianceMatrix, RandomNumberSequence.MultivariateMatrixStorageType matrixStorageType, RandomNumberSequence.GaussianGenerationMode generatorMode)
        {
            IRandomNumberStream randomStream = GetRandomStream();

            /* do not apply BoxMuller to Pseudo-Random Number Generator: */
            Assume.That(randomStream.Generator is IPseudoRandomNumberGenerator || (string)generatorMode.Name != "BoxMuller", "Box-Muller approach is for Pseudo-Random Number Generators not adequate.");

            var sample = new double[sampleSize * dimension];
            int workspaceLength = randomStream.NextNumberSequence.GaussianMultivariateWorkspaceQuery(sampleSize, dimension, matrixStorageType, generatorMode);

            double[] workspace = null;
            if (workspaceLength > 0)
            {
                workspace = new double[workspaceLength];
            }
            var outputRepresentation = randomStream.NextNumberSequence.GaussianMultivariate(sampleSize, sample, dimension, matrixStorageType, mu, pseudoRootOfVarianceCovarianceMatrix, workspace, generatorMode);

            for (int j = 0; j < dimension; j++)
            {
                // check the mean of the j'te component etc.:
                double[] projectedSample = null;

                switch (outputRepresentation)
                {
                    case Basics.BLAS.MatrixTransposeState.NoTranspose:
                        projectedSample = sample.Skip(j).Where((x, k) => k % dimension == 0).ToArray();  // the relevant sample, i.e. realisations of a normal-distributed random number
                        break;

                    case Basics.BLAS.MatrixTransposeState.Transpose:
                    case Basics.BLAS.MatrixTransposeState.Hermite:
                        projectedSample = sample.Skip(j * sampleSize).Where((x, k) => k < sampleSize).ToArray();
                        break;

                    default:
                        throw new NotImplementedException();
                }
                Assert.That(projectedSample.Length >= sampleSize, String.Format("Length : {0}", projectedSample.Length));

                double expectedMean = mu[j];
                double estimatedMean = projectedSample.Average();

                double estimatedStandardDeviation = GetEstimatedStandardDeviation(projectedSample, sampleSize, estimatedMean);
                double epsilon = 2.0 * estimatedStandardDeviation / Math.Sqrt(sampleSize); // size of the confidence interval w.r.t. normal distribution N{-1}(0.975) \approx 1.96 \approx 2.0

                /* test whether E(X) \in [ estimatedMean - \epsilon, estimatedMean + \epsilon], where \epsilon = \sigma/\sqrt(n} */
                Assert.That(estimatedMean + epsilon, Is.GreaterThanOrEqualTo(expectedMean), String.Format("Component: {0}; expected Mean: {1}; estimated Mean: {2}; lower Confidence Interval bound: {3}; upper Confidence Interval bound: {4}.", j, expectedMean, estimatedMean, estimatedMean - epsilon, estimatedMean + epsilon));
                Assert.That(estimatedMean - epsilon, Is.LessThanOrEqualTo(expectedMean), String.Format("Component: {0}; expected Mean: {1}; estimated Mean: {2}; lower Confidence Interval bound: {3}; upper Confidence Interval bound: {4}.", j, expectedMean, estimatedMean, estimatedMean - epsilon, estimatedMean + epsilon));
            }
        }

        /// <summary>Gets the test case data for generation of random numbers following a multivariate normal distribution.
        /// </summary>
        /// <value>The test case data for generation of random numbers following a multivariate normal distribution</value>
        public IEnumerable<TestCaseData> NextNumberSequenceGaussianMultivariateMeanTestCaseData
        {
            get
            {
                foreach (var generatorMode in GetRandomStream().NextNumberSequence.GaussianModes)
                {

                    /*      (1 2)
                     * Q' = (   )
                     *      (0 1)
                     */
                    yield return new TestCaseData(2, 5000, new double[] { 0, 1.5 }, new double[] { 1, 0, 2, 1 }, RandomNumberSequence.MultivariateMatrixStorageType.Full, generatorMode);
                    yield return new TestCaseData(2, 5000, new double[] { 0, 1.5 }, new double[] { 1, 2, 1 }, RandomNumberSequence.MultivariateMatrixStorageType.TriangularPackaged, generatorMode);

                    yield return new TestCaseData(2, 5000, new double[] { 0, 1.5 }, new double[] { 1, 0, 0, 1 }, RandomNumberSequence.MultivariateMatrixStorageType.Full, generatorMode);
                    yield return new TestCaseData(2, 5000, new double[] { 0, 1.5 }, new double[] { 1.5, 2.7 }, RandomNumberSequence.MultivariateMatrixStorageType.Diagonal, generatorMode);
                }
            }
        }

        /// <summary>A test function that generates random numbers following a multivariate normal distribution.
        /// </summary>
        /// <param name="dimension">The dimension.</param>
        /// <param name="sampleSize">The size of the sample.</param>
        /// <param name="mu">The mean, i.e. of dimension <paramref name="dimension"/>.</param>
        /// <param name="pseudoRootOfVarianceCovarianceMatrix">The pseudo-root of variance covariance matrix.</param>
        /// <param name="matrixStorageType">The type of the matrix storage of <paramref name="pseudoRootOfVarianceCovarianceMatrix"/>.</param>
        /// <param name="lowerCriticalValueOfChiSquaredDistribution">\chi^2_{\alpha/2, n-1}, i.e. the quantile of the Chi-Squared distribution with degree of freedom = <paramref name="sampleSize"/> -1 and \alpha = 0.05.</param>
        /// <param name="upperCriticalValueOfChiSquaredDistribution">\chi^2_{1.0 - \alpha/2, n-1}, i.e. the quantile of the Chi-Squared distribution with degree of freedom = <paramref name="sampleSize"/> -1 and \alpha = 0.05.</param>
        /// <param name="generatorMode">The generator mode.</param>
        /// <remarks>The quantiles of the Chi-Squared distribution can be calculated with R, MathLab, AlgLib etc.</remarks>
        [TestCaseSource(nameof(NextNumberSequenceGaussianMultivariateVarianceTestCaseData))]
        public void NextNumberSequenceGaussianMultivariate_EstimatedVariance_ConvidenceInterval(int dimension, int sampleSize, double[] mu, double[] pseudoRootOfVarianceCovarianceMatrix, RandomNumberSequence.MultivariateMatrixStorageType matrixStorageType, double lowerCriticalValueOfChiSquaredDistribution, double upperCriticalValueOfChiSquaredDistribution, RandomNumberSequence.GaussianGenerationMode generatorMode)
        {
            IRandomNumberStream randomStream = GetRandomStream();

            /* do not apply BoxMuller to Pseudo-Random Number Generator: */
            Assume.That(randomStream.Generator is IPseudoRandomNumberGenerator || (string)generatorMode.Name != "BoxMuller", "Box-Muller approach is for Pseudo-Random Number Generators not adequate.");


            var sample = new double[sampleSize * dimension];
            int workspaceLength = randomStream.NextNumberSequence.GaussianMultivariateWorkspaceQuery(sampleSize, dimension, matrixStorageType, generatorMode);

            double[] workspace = null;
            if (workspaceLength > 0)
            {
                workspace = new double[workspaceLength];
            }
            var outputRepresentation = randomStream.NextNumberSequence.GaussianMultivariate(sampleSize, sample, dimension, matrixStorageType, mu, pseudoRootOfVarianceCovarianceMatrix, workspace, generatorMode);

            int triangularPackagedMatrixStartIndex = 0;

            for (int j = 0; j < dimension; j++)
            {
                // check the variance of the j'te component
                double[] projectedSample = null;

                switch (outputRepresentation)
                {
                    case Basics.BLAS.MatrixTransposeState.NoTranspose:
                        projectedSample = sample.Skip(j).Where((x, k) => k % dimension == 0).ToArray();  // the relevant sample, i.e. realisations of a normal-distributed random number
                        break;

                    case Basics.BLAS.MatrixTransposeState.Transpose:
                    case Basics.BLAS.MatrixTransposeState.Hermite:
                        projectedSample = sample.Skip(j * sampleSize).Where((x, k) => k < sampleSize).ToArray();
                        break;

                    default:
                        throw new NotImplementedException();
                }
                Assert.That(projectedSample.Length >= sampleSize, String.Format("Length : {0}", projectedSample.Length));

                double expectedMean = mu[j];
                double estimatedMean = projectedSample.Average();

                double estimatedVariance = GetEstimatedVariance(projectedSample, sampleSize, estimatedMean);

                double expectedVariance = 0.0;
                switch (matrixStorageType)
                {
                    case RandomNumberSequence.MultivariateMatrixStorageType.Diagonal:
                        expectedVariance = pseudoRootOfVarianceCovarianceMatrix[j] * pseudoRootOfVarianceCovarianceMatrix[j];
                        break;

                    case RandomNumberSequence.MultivariateMatrixStorageType.Full:

                        // compute [Q * Q']_{j,j], where Q' is one of the arguments
                        for (int k = 0; k < dimension; k++)
                        {
                            double temp = pseudoRootOfVarianceCovarianceMatrix[k + dimension * j];
                            expectedVariance += temp * temp;
                        }
                        break;

                    case RandomNumberSequence.MultivariateMatrixStorageType.TriangularPackaged:

                        for (int k = 0; k <= j; k++)
                        {
                            double temp = pseudoRootOfVarianceCovarianceMatrix[triangularPackagedMatrixStartIndex + k];
                            expectedVariance += temp * temp;
                        }
                        triangularPackagedMatrixStartIndex += j + 1;
                        break;

                    default:
                        throw new NotImplementedException();
                }

                /* check whether the (theoretical) variance is inside 
                 * 
                 * [ (n-1) * S^2 / \chi^2_{\alpha/2, n-1}, (n-1)*S^2 / \chi^2_{1.0 - \alpha/2, n-1} ], 
                 * 
                 * where \chi^2_{\alpha, n} is the quantile of the chi-square distribution with degree n and parameter \alpha. 
                 */
                Assert.That(estimatedVariance * (sampleSize - 1) / upperCriticalValueOfChiSquaredDistribution, Is.GreaterThanOrEqualTo(expectedVariance), String.Format("Component: {0}; sampleSize: {1}, theor. Variance: {2}, estimated mean: {3}, estimated Variance: {4}, upper-Chi Squared quantile: {5}", j, sampleSize, expectedVariance, estimatedMean, estimatedVariance, upperCriticalValueOfChiSquaredDistribution));
                Assert.That(estimatedVariance * (sampleSize - 1) / lowerCriticalValueOfChiSquaredDistribution, Is.LessThanOrEqualTo(expectedVariance), String.Format("Component: {0}; sampleSize: {1}, theor. Variance: {2}, estimated mean: {3}, estimated Variance: {4}, upper-Chi Squared quantile: {5}", j, sampleSize, expectedVariance, estimatedMean, estimatedVariance, upperCriticalValueOfChiSquaredDistribution));
            }
        }

        /// <summary>Gets the test case data for generation of random numbers following a multivariate normal distribution.
        /// </summary>
        /// <value>The test case data for generation of random numbers following a multivariate normal distribution</value>
        public IEnumerable<TestCaseData> NextNumberSequenceGaussianMultivariateVarianceTestCaseData
        {
            get
            {
                /*      (1 2)
                 * Q' = (   )
                 *      (0 1)
                 */
                foreach (var generatorMode in GetRandomStream().NextNumberSequence.GaussianModes)
                {
                    yield return new TestCaseData(2, 5000, new double[] { 0, 1.5 }, new double[] { 1, 0, 2, 1 }, RandomNumberSequence.MultivariateMatrixStorageType.Full, 5196.86, 4804.92, generatorMode);
                    yield return new TestCaseData(2, 5000, new double[] { 0, 1.5 }, new double[] { 1, 0, 0, 1 }, RandomNumberSequence.MultivariateMatrixStorageType.Full, 5196.86, 4804.92, generatorMode);
                    yield return new TestCaseData(2, 5000, new double[] { 0, 1.5 }, new double[] { 1, 2, 1 }, RandomNumberSequence.MultivariateMatrixStorageType.TriangularPackaged, 5196.86, 4804.92, generatorMode);
                    yield return new TestCaseData(2, 5000, new double[] { 0, 1.5 }, new double[] { 1.5, 2.7 }, RandomNumberSequence.MultivariateMatrixStorageType.Diagonal, 5196.86, 4804.92, generatorMode);

                    //yield return new TestCaseData(2, 25000, new double[] { 0, 1.5 }, new double[] { 1, 0, 2, 1 }, RandomNumberSequence.MultivariateMatrixStorageType.Full, 25439.143, 24562.64, generatorMode);
                    yield return new TestCaseData(2, 25000, new double[] { 0, 1.5 }, new double[] { 1, 0, 0, 1 }, RandomNumberSequence.MultivariateMatrixStorageType.Full, 25439.143, 24562.64, generatorMode);
                    //yield return new TestCaseData(2, 25000, new double[] { 0, 1.5 }, new double[] { 1, 2, 1 }, RandomNumberSequence.MultivariateMatrixStorageType.TriangularPackaged, 25439.143, 24562.64, generatorMode);
                    yield return new TestCaseData(2, 25000, new double[] { 0, 1.5 }, new double[] { 1.5, 2.7 }, RandomNumberSequence.MultivariateMatrixStorageType.Diagonal, 25439.143, 24562.64, generatorMode);
                }
            }
        }
        #endregion

        #region protected methods

        /// <summary>Gets the <see cref="IRandomNumberStream"/> object under test.
        /// </summary>
        /// <returns>The <see cref="IRandomNumberStream"/> object to test.</returns>
        protected abstract IRandomNumberStream GetRandomStream();
        #endregion

        #region private members

        /// <summary>Gets the estimated Standard Deviation of a specified sample.
        /// </summary>
        /// <param name="sample">The sample.</param>
        /// <param name="sampleLength">The number of relevant elements of <paramref name="sample"/> to take into account.</param>
        /// <param name="estimatedMean">The estimated mean value of <paramref name="sample"/>.</param>
        /// <returns>The estimated Standard Deviation of the sample <paramref name="sample"/>.</returns>
        private static double GetEstimatedStandardDeviation(double[] sample, int sampleLength, double estimatedMean)
        {
            return Math.Sqrt(GetEstimatedVariance(sample, sampleLength, estimatedMean));
        }

        /// <summary>Gets the estimated Variance of a specified sample.
        /// </summary>
        /// <param name="sample">The sample.</param>
        /// <param name="sampleLength">The number of relevant elements of <paramref name="sample"/> to take into account.</param>
        /// <param name="estimatedMean">The estimated mean value of <paramref name="sample"/>.</param>
        /// <returns>The estimated Variance of the sample <paramref name="sample"/>.</returns>
        private static double GetEstimatedVariance(double[] sample, int sampleLength, double estimatedMean)
        {
            double value = 0.0;
            for (int i = 0; i < sampleLength; i++)
            {
                value += (sample[i] - estimatedMean) * (sample[i] - estimatedMean);
            }
            return value / (sampleLength - 1);
        }
        #endregion
    }
}