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
    /// <summary>Serves as abstract unit test class for one-dimensional Fourier Transformations, i.e. a specified <see cref="FFT.IOneDimensional"/> object.
    /// </summary>
    /// <remarks>The <see cref="BuildInNaiveOneDimFourierTransformation"/> class will be use as a benchmark object.</remarks>
    public abstract class OneDimFourierTransformationTests
    {
        #region protected constructors

        /// <summary>Initializes a new instance of the <see cref="OneDimFourierTransformationTests"/> class.
        /// </summary>
        protected OneDimFourierTransformationTests()
        {
        }
        #endregion

        #region public methods

        /// <summary>A test function for <see cref="FFT.IOneDimensional.ForwardTransformation(Complex[], Complex[])"/>.
        /// </summary>
        /// <param name="length">The number of Fourier coefficients.</param>
        /// <param name="inputFourierCoefficients">The input Fourier coefficients.</param>
        [TestCase(5, 1.0, 2.45, -1.4, 12.53, -0.0987)]
        [TestCase(7, -2.3, 43.45, -1.4, 12.53, -0.0987, 9.912, -12.5412)]
        public void ForwardTransformation_RealTestCaseData_BenchmarkResult(int length, params double[] inputFourierCoefficients)
        {
            var complexInputCoefficients = inputFourierCoefficients.Select(x => new Complex(x, 0)).ToArray();

            var actual = new Complex[length];
            var testObject = CreateTestObject(length);
            testObject.ForwardTransformation(complexInputCoefficients, actual);

            var expected = new Complex[length];
            var benchmarkObject = CreateBenchmarkObject(length);
            benchmarkObject.ForwardTransformation(complexInputCoefficients, expected);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>A test function for <see cref="FFT.IOneDimensional.ForwardTransformation(Complex[], Complex[])"/>.
        /// </summary>
        /// <param name="length">The number of Fourier coefficients.</param>
        /// <param name="inputFourierCoefficientsAsObject">The complex input Fourier coefficients.</param>
        [TestCaseSource(nameof(ComplexArrayTestCaseData))]
        public void ForwardTransformation_ComplexTestCaseData_BenchmarkResult(int length, object[] inputFourierCoefficientsAsObject)
        {
            var inputFourierCoefficients = inputFourierCoefficientsAsObject[0] as Complex[];

            var actual = new Complex[length];
            var testObject = CreateTestObject(length);
            testObject.ForwardTransformation(inputFourierCoefficients, actual);

            var expected = new Complex[length];
            var benchmarkObject = CreateBenchmarkObject(length);
            benchmarkObject.ForwardTransformation(inputFourierCoefficients, expected);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>A test function for <see cref="FFT.IOneDimensional.ForwardTransformation(Complex[], Complex[], double)"/>.
        /// </summary>
        /// <param name="length">The number of Fourier coefficients.</param>
        /// <param name="scalingFactor">The scaling factor.</param>
        /// <param name="inputFourierCoefficients">The input Fourier coefficients.</param>
        [TestCase(5, 1.0, 1.0, 2.45, -1.4, 12.53, -0.0987)]
        [TestCase(5, -3.651, 1.0, 2.45, -1.4, 12.53, -0.0987)]
        [TestCase(7, 5.7871, -2.3, 43.45, -1.4, 12.53, -0.0987, 9.912, -12.5412)]
        [TestCase(7, -43.7871, -2.3, 43.45, -1.4, 12.53, -0.0987, 9.912, -12.5412)]
        public void ForwardTransformation_RealTestCaseData_BenchmarkResult(int length, double scalingFactor, params double[] inputFourierCoefficients)
        {
            var complexInputCoefficients = inputFourierCoefficients.Select(x => new Complex(x, 0)).ToArray();

            var actual = new Complex[length];
            var testObject = CreateTestObject(length);
            testObject.ForwardTransformation(complexInputCoefficients, actual, scalingFactor);

            var expected = new Complex[length];
            var benchmarkObject = CreateBenchmarkObject(length);
            benchmarkObject.ForwardTransformation(complexInputCoefficients, expected, scalingFactor);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>A test function for <see cref="FFT.IOneDimensional.ForwardTransformation(Complex[], Complex[], double)"/>.
        /// </summary>
        /// <param name="length">The number of Fourier coefficients.</param>
        /// <param name="scalingFactor">The scaling factor.</param>
        /// <param name="inputFourierCoefficientsAsObject">The complex input Fourier coefficients.</param>
        [TestCaseSource(nameof(ComplexArrayWithRealScalingFactorTestCaseData))]
        public void ForwardTransformationScalingFactor_ComplexTestCaseData_BenchmarkResult(int length, double scalingFactor, object[] inputFourierCoefficientsAsObject)
        {
            Complex[] inputFourierCoefficients = inputFourierCoefficientsAsObject[0] as Complex[];

            var actual = new Complex[length];
            var testObject = CreateTestObject(length);
            testObject.ForwardTransformation(inputFourierCoefficients, actual, scalingFactor);

            var expected = new Complex[length];
            var benchmarkObject = CreateBenchmarkObject(length);
            benchmarkObject.ForwardTransformation(inputFourierCoefficients, expected, scalingFactor);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>A test function for <see cref="FFT.IOneDimensional.ForwardTransformation(Complex[])"/>.
        /// </summary>
        /// <param name="length">The number of Fourier coefficients.</param>
        /// <param name="inputFourierCoefficients">The input Fourier coefficients.</param>
        [TestCase(5, 1.0, 2.45, -1.4, 12.53, -0.0987)]
        [TestCase(7, -2.3, 43.45, -1.4, 12.53, -0.0987, 9.912, -12.5412)]
        public void ForwardTransformationInPlace_RealTestCaseData_BenchmarkResult(int length, params double[] inputFourierCoefficients)
        {
            var testObject = CreateTestObject(length);
            var actual = inputFourierCoefficients.Select(x => new Complex(x, 0)).ToArray();
            testObject.ForwardTransformation(actual);

            var benchmarkObject = CreateBenchmarkObject(length);
            var expected = inputFourierCoefficients.Select(x => new Complex(x, 0)).ToArray();
            benchmarkObject.ForwardTransformation(expected);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>A test function for <see cref="FFT.IOneDimensional.ForwardTransformation(Complex[])"/>.
        /// </summary>
        /// <param name="length">The number of Fourier coefficients.</param>
        /// <param name="inputFourierCoefficientsAsObject">The complex input Fourier coefficients.</param>
        [TestCaseSource(nameof(ComplexArrayTestCaseData))]
        public void ForwardTransformationInPlace_ComplexTestCaseData_BenchmarkResult(int length, object[] inputFourierCoefficientsAsObject)
        {
            var inputFourierCoefficients = inputFourierCoefficientsAsObject[0] as Complex[];

            var testObject = CreateTestObject(length);
            var actual = inputFourierCoefficients.ToArray();  // create a deep copy of the argument
            testObject.ForwardTransformation(actual);

            var benchmarkObject = CreateBenchmarkObject(length);
            var expected = inputFourierCoefficients.ToArray();
            benchmarkObject.ForwardTransformation(expected);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>A test function for <see cref="FFT.IOneDimensional.ForwardTransformation(Complex[], double)"/>.
        /// </summary>
        /// <param name="length">The number of Fourier coefficients.</param>
        /// <param name="scalingFactor">The scaling factor.</param>
        /// <param name="inputFourierCoefficients">The input Fourier coefficients.</param>
        [TestCase(5, 1.0, 1.0, 2.45, -1.4, 12.53, -0.0987)]
        [TestCase(5, -2.431, 1.0, 2.45, -1.4, 12.53, -0.0987)]
        [TestCase(7, 3.874, -2.3, 43.45, -1.4, 12.53, -0.0987, 9.912, -12.5412)]
        [TestCase(7, -7.921, -2.3, 43.45, -1.4, 12.53, -0.0987, 9.912, -12.5412)]
        public void ForwardTransformationInPlace_RealTestCaseData_BenchmarkResult(int length, double scalingFactor, params double[] inputFourierCoefficients)
        {
            var testObject = CreateTestObject(length);
            var actual = inputFourierCoefficients.Select(x => new Complex(x, 0)).ToArray();
            testObject.ForwardTransformation(actual, scalingFactor);

            var benchmarkObject = CreateBenchmarkObject(length);
            var expected = inputFourierCoefficients.Select(x => new Complex(x, 0)).ToArray();
            benchmarkObject.ForwardTransformation(expected, scalingFactor);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>A test function for <see cref="FFT.IOneDimensional.ForwardTransformation(Complex[], double)"/>.
        /// </summary>
        /// <param name="length">The number of Fourier coefficients.</param>
        /// <param name="scalingFactor">The scaling factor.</param>
        /// <param name="inputFourierCoefficientsAsObject">The complex input Fourier coefficients.</param>
        [TestCaseSource(nameof(ComplexArrayWithRealScalingFactorTestCaseData))]
        public void ForwardTransformationInPlaceScalingFactor_ComplexTestCaseData_BenchmarkResult(int length, double scalingFactor, object[] inputFourierCoefficientsAsObject)
        {
            var inputFourierCoefficients = inputFourierCoefficientsAsObject[0] as Complex[];

            var testObject = CreateTestObject(length);
            var actual = inputFourierCoefficients.ToArray();  // create a deep copy of the argument
            testObject.ForwardTransformation(actual, scalingFactor);

            var benchmarkObject = CreateBenchmarkObject(length);
            var expected = inputFourierCoefficients.ToArray();
            benchmarkObject.ForwardTransformation(expected, scalingFactor);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>A test function for <see cref="FFT.IOneDimensional.BackwardTransformation(Complex[], Complex[])"/>.
        /// </summary>
        /// <param name="length">The number of Fourier coefficients.</param>
        /// <param name="inputFourierCoefficients">The input Fourier coefficients.</param>
        [TestCase(5, 1.0, 2.45, -1.4, 12.53, -0.0987)]
        [TestCase(7, -2.3, 43.45, -1.4, 12.53, -0.0987, 9.912, -12.5412)]
        public void BackwardTransformation_RealTestCaseData_BenchmarkResult(int length, params double[] inputFourierCoefficients)
        {
            var complexInputCoefficients = inputFourierCoefficients.Select(x => new Complex(x, 0)).ToArray();

            var actual = new Complex[length];
            var testObject = CreateTestObject(length);
            testObject.BackwardTransformation(complexInputCoefficients, actual);

            var expected = new Complex[length];
            var benchmarkObject = CreateBenchmarkObject(length);
            benchmarkObject.BackwardTransformation(complexInputCoefficients, expected);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>A test function for <see cref="FFT.IOneDimensional.BackwardTransformation(Complex[], Complex[])"/>.
        /// </summary>
        /// <param name="length">The number of Fourier coefficients.</param>
        /// <param name="inputFourierCoefficientsAsObject">The complex input Fourier coefficients.</param>
        [TestCaseSource(nameof(ComplexArrayTestCaseData))]
        public void BackwardTransformation_ComplexTestCaseData_BenchmarkResult(int length, object[] inputFourierCoefficientsAsObject)
        {
            var inputFourierCoefficients = inputFourierCoefficientsAsObject[0] as Complex[];

            var actual = new Complex[length];
            var testObject = CreateTestObject(length);
            testObject.BackwardTransformation(inputFourierCoefficients, actual);

            var expected = new Complex[length];
            var benchmarkObject = CreateBenchmarkObject(length);
            benchmarkObject.BackwardTransformation(inputFourierCoefficients, expected);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>A test function for <see cref="FFT.IOneDimensional.BackwardTransformation(Complex[], Complex[], double)"/>.
        /// </summary>
        /// <param name="length">The number of Fourier coefficients.</param>
        /// <param name="scalingFactor">The scaling factor.</param>
        /// <param name="inputFourierCoefficients">The input Fourier coefficients.</param>
        [TestCase(5, 1.0, 1.0, 2.45, -1.4, 12.53, -0.0987)]
        [TestCase(5, -3.651, 1.0, 2.45, -1.4, 12.53, -0.0987)]
        [TestCase(7, 5.7871, -2.3, 43.45, -1.4, 12.53, -0.0987, 9.912, -12.5412)]
        [TestCase(7, -43.7871, -2.3, 43.45, -1.4, 12.53, -0.0987, 9.912, -12.5412)]
        public void BackwardTransformation_RealTestCaseData_BenchmarkResult(int length, double scalingFactor, params double[] inputFourierCoefficients)
        {
            var complexInputCoefficients = inputFourierCoefficients.Select(x => new Complex(x, 0)).ToArray();

            var actual = new Complex[length];
            var testObject = CreateTestObject(length);
            testObject.BackwardTransformation(complexInputCoefficients, actual, scalingFactor);

            var expected = new Complex[length];
            var benchmarkObject = CreateBenchmarkObject(length);
            benchmarkObject.BackwardTransformation(complexInputCoefficients, expected, scalingFactor);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>A test function for <see cref="FFT.IOneDimensional.BackwardTransformation(Complex[], Complex[], double)"/>.
        /// </summary>
        /// <param name="length">The number of Fourier coefficients.</param>
        /// <param name="scalingFactor">The scaling factor.</param>
        /// <param name="inputFourierCoefficientsAsObject">The complex input Fourier coefficients.</param>
        [TestCaseSource(nameof(ComplexArrayWithRealScalingFactorTestCaseData))]
        public void BackwardTransformationScalingFactor_ComplexTestCaseData_BenchmarkResult(int length, double scalingFactor, object[] inputFourierCoefficientsAsObject)
        {
            var inputFourierCoefficients = inputFourierCoefficientsAsObject[0] as Complex[];

            var actual = new Complex[length];
            var testObject = CreateTestObject(length);
            testObject.BackwardTransformation(inputFourierCoefficients, actual, scalingFactor);

            var expected = new Complex[length];
            var benchmarkObject = CreateBenchmarkObject(length);
            benchmarkObject.BackwardTransformation(inputFourierCoefficients, expected, scalingFactor);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>A test function for <see cref="FFT.IOneDimensional.BackwardTransformation(Complex[])"/>.
        /// </summary>
        /// <param name="length">The number of Fourier coefficients.</param>
        /// <param name="inputFourierCoefficients">The input Fourier coefficients.</param>
        [TestCase(5, 1.0, 2.45, -1.4, 12.53, -0.0987)]
        [TestCase(7, -2.3, 43.45, -1.4, 12.53, -0.0987, 9.912, -12.5412)]
        public void BackwardTransformationInPlace_RealTestCaseData_BenchmarkResult(int length, params double[] inputFourierCoefficients)
        {
            var testObject = CreateTestObject(length);
            var actual = inputFourierCoefficients.Select(x => new Complex(x, 0)).ToArray();
            testObject.BackwardTransformation(actual);

            var benchmarkObject = CreateBenchmarkObject(length);
            var expected = inputFourierCoefficients.Select(x => new Complex(x, 0)).ToArray();
            benchmarkObject.BackwardTransformation(expected);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>A test function for <see cref="FFT.IOneDimensional.BackwardTransformation(Complex[])"/>.
        /// </summary>
        /// <param name="length">The number of Fourier coefficients.</param>
        /// <param name="inputFourierCoefficientsAsObject">The complex input Fourier coefficients.</param>
        [TestCaseSource(nameof(ComplexArrayTestCaseData))]
        public void BackwardTransformationInPlace_ComplexTestCaseData_BenchmarkResult(int length, object[] inputFourierCoefficientsAsObject)
        {
            var inputFourierCoefficients = inputFourierCoefficientsAsObject[0] as Complex[];

            var testObject = CreateTestObject(length);
            var actual = inputFourierCoefficients.ToArray();  // create a deep copy of the argument
            testObject.BackwardTransformation(actual);

            var benchmarkObject = CreateBenchmarkObject(length);
            var expected = inputFourierCoefficients.ToArray();
            benchmarkObject.BackwardTransformation(expected);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>A test function for <see cref="FFT.IOneDimensional.BackwardTransformation(Complex[], double)"/>.
        /// </summary>
        /// <param name="length">The number of Fourier coefficients.</param>
        /// <param name="scalingFactor">The scaling factor.</param>
        /// <param name="inputFourierCoefficients">The input Fourier coefficients.</param>
        [TestCase(5, 1.0, 1.0, 2.45, -1.4, 12.53, -0.0987)]
        [TestCase(5, -2.431, 1.0, 2.45, -1.4, 12.53, -0.0987)]
        [TestCase(7, 3.874, -2.3, 43.45, -1.4, 12.53, -0.0987, 9.912, -12.5412)]
        [TestCase(7, -7.921, -2.3, 43.45, -1.4, 12.53, -0.0987, 9.912, -12.5412)]
        public void BackwardTransformationInPlace_RealTestCaseData_BenchmarkResult(int length, double scalingFactor, params double[] inputFourierCoefficients)
        {
            var testObject = CreateTestObject(length);
            var actual = inputFourierCoefficients.Select(x => new Complex(x, 0)).ToArray();
            testObject.BackwardTransformation(actual, scalingFactor);

            var benchmarkObject = CreateBenchmarkObject(length);
            var expected = inputFourierCoefficients.Select(x => new Complex(x, 0)).ToArray();
            benchmarkObject.BackwardTransformation(expected, scalingFactor);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>A test function for <see cref="FFT.IOneDimensional.BackwardTransformation(Complex[], double)"/>.
        /// </summary>
        /// <param name="length">The number of Fourier coefficients.</param>
        /// <param name="scalingFactor">The scaling factor.</param>
        /// <param name="inputFourierCoefficientsAsObject">The complex input Fourier coefficients.</param>
        [TestCaseSource(nameof(ComplexArrayWithRealScalingFactorTestCaseData))]
        public void BackwardTransformationInPlaceScalingFactor_ComplexTestCaseData_BenchmarkResult(int length, double scalingFactor, object[] inputFourierCoefficientsAsObject)
        {
            var inputFourierCoefficients = inputFourierCoefficientsAsObject[0] as Complex[];

            var testObject = CreateTestObject(length);
            var actual = inputFourierCoefficients.ToArray();  // create a deep copy of the argument
            testObject.BackwardTransformation(actual, scalingFactor);

            var benchmarkObject = CreateBenchmarkObject(length);
            var expected = inputFourierCoefficients.ToArray();
            benchmarkObject.BackwardTransformation(expected, scalingFactor);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }
        #region testcase data

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects where the first item is the number of elements and the second is an arrays of complex numbers.
        /// </summary>
        /// <value>The complex array test case data.</value>
        public static IEnumerable<TestCaseData> ComplexArrayTestCaseData
        {
            get
            {
                yield return new TestCaseData(3, new object[] { new Complex[] { 1.2, 3.5, -5.12 } });
                yield return new TestCaseData(3, new object[] { new Complex[] { new Complex(1.2, -0.9), 3.5, new Complex(-5.12, 1.23) } });
                yield return new TestCaseData(3, new object[] { new Complex[] { new Complex(-0.3, 1.0), new Complex(10.4, -1.0), 64.1 } });
            }
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects where the first item is the number of elements, the second is a real scaling factor and the third is an arrays of complex numbers.
        /// </summary>
        /// <value>The complex array test case data with an additional real scaling factor.</value>
        public static IEnumerable<TestCaseData> ComplexArrayWithRealScalingFactorTestCaseData
        {
            get
            {
                yield return new TestCaseData(3, 1.0, new object[] { new Complex[] { 1.2, 3.5, -5.12 } });
                yield return new TestCaseData(3, -1.431, new object[] { new Complex[] { 1.2, 3.5, -5.12 } });
                yield return new TestCaseData(3, 3.162, new object[] { new Complex[] { new Complex(1.2, -0.9), 3.5, new Complex(-5.12, 1.23) } });
                yield return new TestCaseData(3, -4.87151, new object[] { new Complex[] { new Complex(-0.3, 1.0), new Complex(10.4, -1.0), 64.1 } });
            }
        }
        #endregion

        #endregion

        #region protected/internal methods

        /// <summary>Creates the test object, i.e. the <see cref="FFT.IOneDimensional"/> object under test.
        /// </summary>
        /// <param name="length">The number of Fourier coefficients.</param>
        /// <returns>The <see cref="FFT.IOneDimensional"/> object to test.</returns>
        protected abstract FFT.IOneDimensional CreateTestObject(int length);

        /// <summary>Creates the benchmark object.
        /// </summary>
        /// <param name="length">The number of Fourier coefficients.</param>
        /// <returns>The benchmark object, i.e. a simple managed code build-In implementation.</returns>
        internal virtual BuildInNaiveOneDimFourierTransformation CreateBenchmarkObject(int length)
        {
            return new BuildInNaiveOneDimFourierTransformation(length);
        }
        #endregion
    }
}