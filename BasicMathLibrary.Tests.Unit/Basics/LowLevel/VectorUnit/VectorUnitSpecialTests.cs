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
using NUnit.Framework;

using Dodoni.MathLibrary.Basics.LowLevel.BuildIn;

namespace Dodoni.MathLibrary.Basics.LowLevel
{
    /// <summary>Serves as abstract unit test class for mathematical vector operations, i.e. a specified <see cref="IVectorUnitSpecial"/>.
    /// </summary>
    /// <remarks>The <see cref="BuildInVectorUnitSpecial"/> class will be use as a benchmark object.</remarks>
    public abstract class VectorUnitSpecialTests
    {
        #region private members

        /// <summary>The Benchmark implementation.
        /// </summary>
        private BuildInVectorUnitSpecial m_Benchmark;

        /// <summary>The <see cref="IVectorUnitSpecial"/> object to test.
        /// </summary>
        private IVectorUnitSpecial m_MathVectorOperations;
        #endregion

        #region protected constructors

        /// <summary>Initializes a new instance of the <see cref="VectorUnitSpecialTests"/> class.
        /// </summary>
        protected VectorUnitSpecialTests()
        {
        }
        #endregion

        #region public methods

        /// <summary>Prepares the unit tests.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            m_Benchmark = new BuildInVectorUnitSpecial();
            m_MathVectorOperations = CreateTestObject();
        }

        /// <summary>A test function for <see cref="IVectorUnitSpecial.CdfNorm(int, double[], double[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        [TestCase(3, -1.0, 2.0, 3.0)]
        [TestCase(3, 4.0, 17.635, -14.464)]
        public void CdfNorm_TestCaseData_BenchmarkResult(int n, params double[] a)
        {
            var actual = new double[n];
            m_MathVectorOperations.CdfNorm(n, a, actual);

            var expected = new double[n];
            m_Benchmark.CdfNorm(n, a, expected);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-8));
        }

        /// <summary>A test function for <see cref="IVectorUnitSpecial.CdfNorm(int, double[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        [TestCase(3, -1.0, 2.0, 3.0)]
        [TestCase(3, 4.0, 17.635, -14.464)]
        public void CdfNormBuildIn_TestCaseData_BenchmarkResult(int n, params double[] a)
        {
            var actual = a.ToArray(); // copy of the argument
            m_MathVectorOperations.CdfNorm(n, actual);

            var expected = a.ToArray();  // copy of the argument
            m_Benchmark.CdfNorm(n, expected);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-8));
        }

        /// <summary>A test function for <see cref="IVectorUnitSpecial.CdfNorm(int, double[], double[],int,int)"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of the output vector.</param>
        /// <param name="a">The input vector a.</param>
        [TestCase(3, 0, 0, 1.0, -2.0, 4.0)]
        [TestCase(2, 1, 0, 1.0, 2.5, 4.0)]
        [TestCase(2, 1, 2, -21.0, -24.5, 4.56)]
        public void CdfNorm_ExtendedDoubleTestCaseData_BenchmarkResult(int n, int startIndexA, int startIndexY, params double[] a)
        {
            var actual = new double[startIndexY + n];
            m_MathVectorOperations.CdfNorm(n, a.AsSpan().Slice(startIndexA), actual.AsSpan().Slice(startIndexY));

            var expected = new double[startIndexY + n];
            m_Benchmark.CdfNorm(n, a.AsSpan().Slice(startIndexA), expected.AsSpan().Slice(startIndexY));

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-8));
        }

        /// <summary>A test function for <see cref="IVectorUnitSpecial.CdfNormInv(int, double[], double[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        [TestCase(11, 0.00001, 0.014, 0.124, 0.261, 0.379, 0.5, 0.651, 0.7, 0.854, 0.9, 0.9991)]
        [TestCase(3, 0.0054, 0.8431, 0.975)]
        public void CdfNormInv_TestCaseData_BenchmarkResult(int n, params double[] a)
        {
            var actual = new double[n];
            m_MathVectorOperations.CdfNormInv(n, a, actual);

            var expected = new double[n];
            m_Benchmark.CdfNormInv(n, a, expected);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-8));
        }

        /// <summary>A test function for <see cref="IVectorUnitSpecial.CdfNormInv(int, double[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        [TestCase(11, 0.00001, 0.014, 0.124, 0.261, 0.379, 0.5, 0.651, 0.7, 0.854, 0.9, 0.9991)]
        [TestCase(3, 0.0054, 0.8431, 0.975)]
        public void CdfNormInvBuildIn_TestCaseData_BenchmarkResult(int n, params double[] a)
        {
            var actual = a.ToArray(); // copy of the argument
            m_MathVectorOperations.CdfNormInv(n, actual);

            var expected = a.ToArray(); // copy of the argument
            m_Benchmark.CdfNormInv(n, expected);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-8));
        }

        /// <summary>A test function for <see cref="IVectorUnitSpecial.CdfNormInv(int, double[], double[],int,int)"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of the output vector.</param>
        /// <param name="a">The input vector a.</param>
        [TestCase(3, 0, 0, 0.0001, 0.5, 0.99991)]
        [TestCase(2, 1, 0, 0.12, 0.4281, 0.8761)]
        [TestCase(2, 1, 2, 0.61, 0.271, 0.9412)]
        public void CdfNormInv_ExtendedDoubleTestCaseData_BenchmarkResult(int n, int startIndexA, int startIndexY, params double[] a)
        {
            var actual = new double[startIndexY + n];
            m_MathVectorOperations.CdfNormInv(n, a.AsSpan().Slice(startIndexA), actual.AsSpan().Slice(startIndexY));

            var expected = new double[startIndexY + n];
            m_Benchmark.CdfNormInv(n, a.AsSpan().Slice(startIndexA), expected.AsSpan().Slice(startIndexY));

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-8));
        }
        #endregion

        #region protected methods

        /// <summary>Creates the test object, i.e. the <see cref="IVectorUnitSpecial"/> object under test.
        /// </summary>
        /// <returns>The <see cref="IVectorUnitSpecial"/> object to test.</returns>
        protected abstract IVectorUnitSpecial CreateTestObject();
        #endregion
    }
}