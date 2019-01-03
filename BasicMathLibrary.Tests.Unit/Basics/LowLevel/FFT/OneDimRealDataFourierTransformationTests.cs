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
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

using NUnit.Framework;
using Dodoni.MathLibrary.Basics.LowLevel.BuildIn;

namespace Dodoni.MathLibrary.Basics.LowLevel
{
    /// <summary>Serves as abstract unit test class for one-dimensional Fourier Transformations, i.e. a specified <see cref="FFT.IOneDimensionalRealData"/> object.
    /// </summary>
    /// <remarks>The <see cref="BuildInNaiveOneDimRealDataFourierTransformation"/> class will be use as a benchmark object.</remarks>
    public abstract class OneDimRealDataFourierTransformationTests
    {
        #region protected constructors

        /// <summary>Initializes a new instance of the <see cref="OneDimRealDataFourierTransformationTests"/> class.
        /// </summary>
        protected OneDimRealDataFourierTransformationTests()
        {
        }
        #endregion

        #region public methods

        /// <summary>A test function for <see cref="FFT.IOneDimensionalRealData.ForwardTransformation(double[], double[])"/>.
        /// </summary>
        /// <param name="length">The number of Fourier coefficients.</param>
        /// <param name="inputFourierCoefficients">The input Fourier coefficients.</param>
        [TestCase(4, 2.45, -1.4, 12.53, -0.0987)]
        [TestCase(5, 1.0, 2.45, -1.4, 12.53, -0.0987)]
        [TestCase(7, -2.3, 43.45, -1.4, 12.53, -0.0987, 9.912, -12.5412)]
        public void ForwardTransformation_RealTestCaseData_BenchmarkResult(int length, params double[] inputFourierCoefficients)
        {
            int n = 2 * (length / 2 + 1);

            var actual = new double[n];
            var testObject = CreateTestObject(length);
            testObject.ForwardTransformation(inputFourierCoefficients, actual);

            var expected = new double[n];
            var benchmarkObject = CreateBenchmarkObject(length);
            benchmarkObject.ForwardTransformation(inputFourierCoefficients, expected);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-9));
        }

        /// <summary>A test function for <see cref="FFT.IOneDimensionalRealData.ForwardTransformation(double[], double[], double)"/>.
        /// </summary>
        /// <param name="length">The number of Fourier coefficients.</param>
        /// <param name="scalingFactor">The scaling factor.</param>
        /// <param name="inputFourierCoefficients">The input Fourier coefficients.</param>
        [TestCase(4, -1.542, 2.45, -1.4, 12.53, -0.0987)]
        [TestCase(5, 1.0, 1.0, 2.45, -1.4, 12.53, -0.0987)]
        [TestCase(5, -3.651, 1.0, 2.45, -1.4, 12.53, -0.0987)]
        [TestCase(7, 5.7871, -2.3, 43.45, -1.4, 12.53, -0.0987, 9.912, -12.5412)]
        [TestCase(7, -43.7871, -2.3, 43.45, -1.4, 12.53, -0.0987, 9.912, -12.5412)]
        public void ForwardTransformation_RealTestCaseData_BenchmarkResult(int length, double scalingFactor, params double[] inputFourierCoefficients)
        {
            int n = 2 * (length / 2 + 1);

            var actual = new double[n];
            var testObject = CreateTestObject(length);
            testObject.ForwardTransformation(inputFourierCoefficients, actual, scalingFactor);

            var expected = new double[n];
            var benchmarkObject = CreateBenchmarkObject(length);
            benchmarkObject.ForwardTransformation(inputFourierCoefficients, expected, scalingFactor);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-9));
        }

        /// <summary>A test function for <see cref="FFT.IOneDimensionalRealData.ForwardTransformation(double[])"/>.
        /// </summary>
        /// <param name="length">The number of Fourier coefficients.</param>
        /// <param name="inputFourierCoefficients">The input Fourier coefficients.</param>
        [TestCase(4, 2.45, -1.4, 12.53, -0.0987)]
        [TestCase(5, 1.0, 2.45, -1.4, 12.53, -0.0987)]
        [TestCase(7, -2.3, 43.45, -1.4, 12.53, -0.0987, 9.912, -12.5412)]
        public void ForwardTransformationInPlace_RealTestCaseData_BenchmarkResult(int length, params double[] inputFourierCoefficients)
        {
            int n = 2 * (length / 2 + 1);

            var testObject = CreateTestObject(length);
            var actual = new double[n];
            inputFourierCoefficients.CopyTo(actual, 0);
            testObject.ForwardTransformation(actual);

            var benchmarkObject = CreateBenchmarkObject(length);
            var expected = new double[n];
            inputFourierCoefficients.CopyTo(expected, 0);
            benchmarkObject.ForwardTransformation(expected);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-9));
        }

        /// <summary>A test function for <see cref="FFT.IOneDimensionalRealData.ForwardTransformation(double[], double)"/>.
        /// </summary>
        /// <param name="length">The number of Fourier coefficients.</param>
        /// <param name="scalingFactor">The scaling factor.</param>
        /// <param name="inputFourierCoefficients">The input Fourier coefficients.</param>
        [TestCase(4, -0.8541, 2.45, -1.4, 12.53, -0.0987)]
        [TestCase(5, 1.0, 1.0, 2.45, -1.4, 12.53, -0.0987)]
        [TestCase(5, -2.431, 1.0, 2.45, -1.4, 12.53, -0.0987)]
        [TestCase(7, 3.874, -2.3, 43.45, -1.4, 12.53, -0.0987, 9.912, -12.5412)]
        [TestCase(7, -7.921, -2.3, 43.45, -1.4, 12.53, -0.0987, 9.912, -12.5412)]
        public void ForwardTransformationInPlace_RealTestCaseData_BenchmarkResult(int length, double scalingFactor, params double[] inputFourierCoefficients)
        {
            int n = 2 * (length / 2 + 1);

            var testObject = CreateTestObject(length);
            var actual = new double[n];
            inputFourierCoefficients.CopyTo(actual, 0);
            testObject.ForwardTransformation(actual, scalingFactor);

            var benchmarkObject = CreateBenchmarkObject(length);
            var expected = new double[n];
            inputFourierCoefficients.CopyTo(expected, 0);
            benchmarkObject.ForwardTransformation(expected, scalingFactor);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-9));
        }

        /// <summary>A test function for <see cref="FFT.IOneDimensionalRealData.BackwardTransformation(double[], double[])"/>.
        /// </summary>
        /// <param name="length">The number of Fourier coefficients.</param>
        /// <param name="inputFourierCoefficients">The input Fourier coefficients.</param>
        [TestCase(4, 1.0, 0.0, -1.4, 12.53, -0.0987, 0.0)]
        [TestCase(4, 12.1591, 0.0, 0.012, 15.261, -8.183, 0.0)]
        [TestCase(5, 1.0, 0.0, -1.4, 12.53, -0.0987, 1.51)]
        [TestCase(7, -2.3, 0.0, -1.4, 12.53, -0.0987, 9.912, -12.5412, -54.1761)]
        public void BackwardTransformation_RealTestCaseData_BenchmarkResult(int length, params double[] inputFourierCoefficients)
        {
            int n = 2 * (length / 2 + 1);

            Assume.That(inputFourierCoefficients.Length >= n, String.Format("Number of input coefficients should be at least {0}.", n));
            Assume.That(inputFourierCoefficients[1], Is.EqualTo(0.0).Within(1E-11), String.Format("The imaginary part of the first element should be 0, but is {0}.", inputFourierCoefficients[1]));

            if (length % 2 == 0)
            {
                Assume.That(inputFourierCoefficients[n - 1], Is.EqualTo(0.0).Within(1E-11), String.Format("Last element of complex Hermitian input should be 0, but is {0}.", inputFourierCoefficients[n - 1]));
            }

            var actual = new double[length];
            var testObject = CreateTestObject(length);
            testObject.BackwardTransformation(inputFourierCoefficients, actual);

            var expected = new double[length];
            var benchmarkObject = CreateBenchmarkObject(length);
            benchmarkObject.BackwardTransformation(inputFourierCoefficients, expected);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-9));
        }

        /// <summary>A test function that applied <see cref="FFT.IOneDimensionalRealData.ForwardTransformation(double[], double[])"/> and 
        /// afterwards <see cref="FFT.IOneDimensionalRealData.BackwardTransformation(double[], double[], double)"/>.
        /// </summary>
        /// <param name="length">The number of Fourier coefficients.</param>
        /// <param name="inputFourierCoefficients">The input Fourier coefficients.</param>
        [TestCase(4, 2.45, -1.4, 12.53, -0.0987)]
        [TestCase(5, 1.0, 2.45, -1.4, 12.53, -0.0987)]
        [TestCase(7, -2.3, 43.45, -1.4, 12.53, -0.0987, 9.912, -12.5412)]
        public void ForwardBackwardTransformation_RealTestCaseData_BenchmarkResult(int length, params double[] inputFourierCoefficients)
        {
            int n = 2 * (length / 2 + 1);

            var testObject = CreateTestObject(length);
            var testForwardCoefficientOutput = new double[n];
            var actual = new double[length];

            testObject.ForwardTransformation(inputFourierCoefficients, testForwardCoefficientOutput);
            testObject.BackwardTransformation(testForwardCoefficientOutput, actual, 1.0 / length); // scaling factor required!

            Assert.That(actual, Is.EqualTo(inputFourierCoefficients).AsCollection.Within(1E-9));
        }

        /// <summary>A test function for <see cref="FFT.IOneDimensionalRealData.BackwardTransformation(double[], double[], double)"/>.
        /// </summary>
        /// <param name="length">The number of Fourier coefficients.</param>
        /// <param name="scalingFactor">The scaling factor.</param>
        /// <param name="inputFourierCoefficients">The input Fourier coefficients.</param>
        [TestCase(4, -0.871, 1.0, 0.0, -1.4, 12.53, -0.0987, 0.0)]
        [TestCase(5, 1.0, 1.0, 0.0, -1.4, 12.53, -0.0987, 4.615)]
        [TestCase(5, -3.651, 1.0, 0.0, -1.4, 12.53, -0.0987, 0.9987)]
        [TestCase(7, 5.7871, -2.3, 0.0, -1.4, 12.53, -0.0987, 9.912, -12.5412, 0.7615)]
        [TestCase(7, -43.7871, -2.3, 0.0, -1.4, 12.53, -0.0987, 9.912, -12.5412, -51.816)]
        public void BackwardTransformation_RealTestCaseData_BenchmarkResult(int length, double scalingFactor, params double[] inputFourierCoefficients)
        {
            int n = 2 * (length / 2 + 1);
            Assume.That(inputFourierCoefficients.Length >= n, String.Format("Number of input coefficients should be at least {0}.", n));
            Assume.That(inputFourierCoefficients[1], Is.EqualTo(0.0).Within(1E-11), String.Format("The imaginary part of the first element should be 0, but is {0}.", inputFourierCoefficients[1]));

            if (length % 2 == 0)
            {
                Assume.That(inputFourierCoefficients[n - 1], Is.EqualTo(0.0).Within(1E-11), String.Format("Last element of complex Hermitian input should be 0, but is {0}.", inputFourierCoefficients[n - 1]));
            }

            var actual = new double[length];
            var testObject = CreateTestObject(length);
            testObject.BackwardTransformation(inputFourierCoefficients, actual, scalingFactor);

            var expected = new double[length];
            var benchmarkObject = CreateBenchmarkObject(length);
            benchmarkObject.BackwardTransformation(inputFourierCoefficients, expected, scalingFactor);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-9));
        }

        /// <summary>A test function for <see cref="FFT.IOneDimensionalRealData.BackwardTransformation(double[])"/>.
        /// </summary>
        /// <param name="length">The number of Fourier coefficients.</param>
        /// <param name="inputFourierCoefficients">The input Fourier coefficients.</param>
        [TestCase(4, 1.0, 0.0, -1.4, 12.53, -0.0987, 0.0)]
        [TestCase(5, 1.0, 0.0, -1.4, 12.53, -0.0987, 0.9871)]
        [TestCase(7, -2.3, 0.0, -1.4, 12.53, -0.0987, 9.912, -12.5412, -8.413)]
        public void BackwardTransformationInPlace_RealTestCaseData_BenchmarkResult(int length, params double[] inputFourierCoefficients)
        {
            int n = 2 * (length / 2 + 1);
            Assume.That(inputFourierCoefficients.Length >= n, String.Format("Number of input coefficients should be at least {0}.", n));
            Assume.That(inputFourierCoefficients[1], Is.EqualTo(0.0).Within(1E-11), String.Format("The imaginary part of the first element should be 0, but is {0}.", inputFourierCoefficients[1]));

            if (length % 2 == 0)
            {
                Assume.That(inputFourierCoefficients[n - 1], Is.EqualTo(0.0).Within(1E-11), String.Format("Last element of complex Hermitian input should be 0, but is {0}.", inputFourierCoefficients[n - 1]));
            }

            var testObject = CreateTestObject(length);
            var actual = new double[n];
            inputFourierCoefficients.CopyTo(actual, 0);
            testObject.BackwardTransformation(actual);

            var benchmarkObject = CreateBenchmarkObject(length);
            var expected = new double[n];
            inputFourierCoefficients.CopyTo(expected, 0);
            benchmarkObject.BackwardTransformation(expected);

            // only 'length' elements of the return values are required!
            Assert.That(actual.Take(length), Is.EqualTo(expected.Take(length)).AsCollection.Within(1E-9));
        }

        /// <summary>A test function for <see cref="FFT.IOneDimensionalRealData.BackwardTransformation(double[], double)"/>.
        /// </summary>
        /// <param name="length">The number of Fourier coefficients.</param>
        /// <param name="scalingFactor">The scaling factor.</param>
        /// <param name="inputFourierCoefficients">The input Fourier coefficients.</param>
        [TestCase(4, -0.8715, 1.0, 0.0, -1.4, 12.53, -0.0987, 0.0)]
        [TestCase(5, 1.0, 1.0, 0.0, -1.4, 12.53, -0.0987, 1.541)]
        [TestCase(5, -2.431, 1.0, 0.0, -1.4, 12.53, -0.0987, -5.6515)]
        [TestCase(7, 3.874, -2.3, 0.0, -1.4, 12.53, -0.0987, 9.912, -12.5412, 0.011661)]
        [TestCase(7, -7.921, -2.3, 0.0, -1.4, 12.53, -0.0987, 9.912, -12.5412, -0.871)]
        public void BackwardTransformationInPlace_RealTestCaseData_BenchmarkResult(int length, double scalingFactor, params double[] inputFourierCoefficients)
        {
            int n = 2 * (length / 2 + 1);
            Assume.That(inputFourierCoefficients.Length >= n, String.Format("Number of input coefficients should be at least {0}.", n));
            Assume.That(inputFourierCoefficients[1], Is.EqualTo(0.0).Within(1E-11), String.Format("The imaginary part of the first element should be 0, but is {0}.", inputFourierCoefficients[1]));

            if (length % 2 == 0)
            {
                Assume.That(inputFourierCoefficients[n - 1], Is.EqualTo(0.0).Within(1E-11), String.Format("Last element of complex Hermitian input should be 0, but is {0}.", inputFourierCoefficients[n - 1]));
            }

            var testObject = CreateTestObject(length);
            var actual = new double[n];
            inputFourierCoefficients.CopyTo(actual, 0);
            testObject.BackwardTransformation(actual, scalingFactor);

            var benchmarkObject = CreateBenchmarkObject(length);
            var expected = new double[n];
            inputFourierCoefficients.CopyTo(expected, 0);
            benchmarkObject.BackwardTransformation(expected, scalingFactor);

            // only 'length' elements of the return values are required!
            Assert.That(actual.Take(length), Is.EqualTo(expected.Take(length)).AsCollection.Within(1E-9));
        }
        #endregion

        #region protected/internal methods

        /// <summary>Creates the test object, i.e. the <see cref="FFT.IOneDimensionalRealData"/> object under test.
        /// </summary>
        /// <param name="length">The number of Fourier coefficients.</param>
        /// <returns>The <see cref="FFT.IOneDimensionalRealData"/> object to test.</returns>
        protected abstract FFT.IOneDimensionalRealData CreateTestObject(int length);

        /// <summary>Creates the benchmark object.
        /// </summary>
        /// <param name="length">The number of Fourier coefficients.</param>
        /// <returns>The benchmark object, i.e. a simple managed code build-In implementation.</returns>
        internal BuildInNaiveOneDimRealDataFourierTransformation CreateBenchmarkObject(int length)
        {
            return new BuildInNaiveOneDimRealDataFourierTransformation(length);
        }
        #endregion
    }
}