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
using System.Numerics;
using System.Collections.Generic;

using NUnit.Framework;
using Dodoni.MathLibrary.Basics.LowLevel.BuildIn;

namespace Dodoni.MathLibrary.Basics.LowLevel
{
    /// <summary>Serves as abstract unit test class for mathematical vector operations, i.e. a specified <see cref="IVectorUnitBasics"/>.
    /// </summary>
    /// <remarks>The <see cref="BuildInVectorUnitBasics"/> class will be use as a benchmark object.</remarks>
    public abstract class VectorUnitBasicsTests
    {
        #region private members

        /// <summary>The Benchmark implementation.
        /// </summary>
        private BuildInVectorUnitBasics m_Benchmark;

        /// <summary>The <see cref="IVectorUnitBasics"/> object to test.
        /// </summary>
        private IVectorUnitBasics m_MathVectorOperations;
        #endregion

        #region protected constructors

        /// <summary>Initializes a new instance of the <see cref="VectorUnitBasicsTests"/> class.
        /// </summary>
        protected VectorUnitBasicsTests()
        {
        }
        #endregion

        #region public methods

        /// <summary>Prepares the unit tests.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            m_Benchmark = new BuildInVectorUnitBasics();
            m_MathVectorOperations = CreateTestObject();
        }

        #region Arithmetic functions

        /// <summary>A test function for <see cref="IVectorUnitBasics.Add(int, double[], double[], double[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        [TestCaseSource(nameof(TwoDoubleArrayTestCaseData))]
        public void Add_DoubleTestCaseData_BenchmarkResult(int n, double[] a, double[] b)
        {
            var actual = new double[n];
            m_MathVectorOperations.Add(n, a, b, actual);

            var expected = new double[n];
            m_Benchmark.Add(n, a, b, expected);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Add(int, double[], double[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        [TestCaseSource(nameof(TwoDoubleArrayTestCaseData))]
        public void AddBuildIn_DoubleTestCaseData_BenchmarkResult(int n, double[] a, double[] b)
        {
            var actual = a.ToArray(); // a copy of the input
            m_MathVectorOperations.Add(n, actual, b);

            var expected = a.ToArray(); // a copy of the input
            m_Benchmark.Add(n, expected, b);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Add(int, double[], double[], double[],int,int,int)"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexB">The null-based start index of <paramref name="b"/>.</param>
        /// <param name="startIndexY">The null-based start index of the output vector.</param>
        [TestCaseSource(nameof(ExtendedTwoDoubleArrayTestCaseData))]
        public void Add_ExtendedDoubleTestCaseData_BenchmarkResult(int n, double[] a, double[] b, int startIndexA, int startIndexB, int startIndexY)
        {
            var actual = new double[startIndexY + n];
            m_MathVectorOperations.Add(n, a, b, actual, startIndexA, startIndexB, startIndexY);

            var expected = new double[startIndexY + n];
            m_Benchmark.Add(n, a, b, expected, startIndexA, startIndexB, startIndexY);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Add(int, double[], double, double[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input double precision number b.</param>
        [TestCaseSource(nameof(DoubleArrayDoubleValueTestCaseData))]
        public void Add_DoubleTestCaseData_BenchmarkResult(int n, double[] a, double b)
        {
            var actual = new double[n];
            m_MathVectorOperations.Add(n, a, b, actual);

            var expected = new double[n];
            m_Benchmark.Add(n, a, b, expected);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Add(int, double[], double)"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input double precision number b.</param>
        [TestCaseSource(nameof(DoubleArrayDoubleValueTestCaseData))]
        public void AddBuildIn_DoubleTestCaseData_BenchmarkResult(int n, double[] a, double b)
        {
            var actual = a.ToArray(); // a copy of the input
            m_MathVectorOperations.Add(n, actual, b);

            var expected = a.ToArray(); // a copy of the input
            m_Benchmark.Add(n, expected, b);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Add(int, Complex[], Complex[], Complex[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        [TestCaseSource(nameof(TwoComplexArrayTestCaseData))]
        public void Add_ComplexTestCaseData_BenchmarkResult(int n, Complex[] a, Complex[] b)
        {
            var actual = new Complex[n];
            m_MathVectorOperations.Add(n, a, b, actual);

            var expected = new Complex[n];
            m_Benchmark.Add(n, a, b, expected);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Add(int, Complex[], Complex[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        [TestCaseSource(nameof(TwoComplexArrayTestCaseData))]
        public void AddBuildIn_ComplexTestCaseData_BenchmarkResult(int n, Complex[] a, Complex[] b)
        {
            var actual = a.ToArray();
            m_MathVectorOperations.Add(n, actual, b);

            var expected = a.ToArray();
            m_Benchmark.Add(n, expected, b);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Add(int, Complex[], Complex[], Complex[],int,int,int)"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexB">The null-based start index of <paramref name="b"/>.</param>
        /// <param name="startIndexY">The null-based start index of the output vector.</param>
        [TestCaseSource(nameof(ExtendedTwoComplexArrayTestCaseData))]
        public void Add_ExtendedComplexTestCaseData_BenchmarkResult(int n, Complex[] a, Complex[] b, int startIndexA, int startIndexB, int startIndexY)
        {
            var actual = new Complex[startIndexY + n];
            m_MathVectorOperations.Add(n, a, b, actual, startIndexA, startIndexB, startIndexY);

            var expected = new Complex[startIndexY + n];
            m_Benchmark.Add(n, a, b, expected, startIndexA, startIndexB, startIndexY);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Sub(int, double[], double[], double[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        [TestCaseSource(nameof(TwoDoubleArrayTestCaseData))]
        public void Sub_DoubleTestCaseData_BenchmarkResult(int n, double[] a, double[] b)
        {
            var actual = new double[n];
            m_MathVectorOperations.Sub(n, a, b, actual);

            var expected = new double[n];
            m_Benchmark.Sub(n, a, b, expected);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Sub(int, double[], double[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        [TestCaseSource(nameof(TwoDoubleArrayTestCaseData))]
        public void SubBuildIn_DoubleTestCaseData_BenchmarkResult(int n, double[] a, double[] b)
        {
            var actual = a.ToArray();
            m_MathVectorOperations.Sub(n, actual, b);

            var expected = a.ToArray();
            m_Benchmark.Sub(n, expected, b);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Sub(int, double[], double[], double[],int,int,int)"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexB">The null-based start index of <paramref name="b"/>.</param>
        /// <param name="startIndexY">The null-based start index of the output vector.</param>
        [TestCaseSource(nameof(ExtendedTwoDoubleArrayTestCaseData))]
        public void Sub_ExtendedDoubleTestCaseData_BenchmarkResult(int n, double[] a, double[] b, int startIndexA, int startIndexB, int startIndexY)
        {
            var actual = new double[startIndexY + n];
            m_MathVectorOperations.Sub(n, a, b, actual, startIndexA, startIndexB, startIndexY);

            var expected = new double[startIndexY + n];
            m_Benchmark.Sub(n, a, b, expected, startIndexA, startIndexB, startIndexY);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Sub(int, Complex[], Complex[], Complex[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        [TestCaseSource(nameof(TwoComplexArrayTestCaseData))]
        public void Sub_ComplexTestCaseData_BenchmarkResult(int n, Complex[] a, Complex[] b)
        {
            var actual = new Complex[n];
            m_MathVectorOperations.Sub(n, a, b, actual);

            var expected = new Complex[n];
            m_Benchmark.Sub(n, a, b, expected);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Sub(int, Complex[], Complex[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        [TestCaseSource(nameof(TwoComplexArrayTestCaseData))]
        public void SubBuildIn_ComplexTestCaseData_BenchmarkResult(int n, Complex[] a, Complex[] b)
        {
            var actual = a.ToArray();
            m_MathVectorOperations.Sub(n, actual, b);

            var expected = a.ToArray();
            m_Benchmark.Sub(n, expected, b);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Sub(int, Complex[], Complex[], Complex[],int,int,int)"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexB">The null-based start index of <paramref name="b"/>.</param>
        /// <param name="startIndexY">The null-based start index of the output vector.</param>
        [TestCaseSource(nameof(ExtendedTwoComplexArrayTestCaseData))]
        public void Sub_ExtendedComplexTestCaseData_BenchmarkResult(int n, Complex[] a, Complex[] b, int startIndexA, int startIndexB, int startIndexY)
        {
            var actual = new Complex[startIndexY + n];
            m_MathVectorOperations.Sub(n, a, b, actual, startIndexA, startIndexB, startIndexY);

            var expected = new Complex[startIndexY + n];
            m_Benchmark.Sub(n, a, b, expected, startIndexA, startIndexB, startIndexY);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Sqr(int, double[], double[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        [TestCase(3, 1.0, 2.0, 3.0)]
        [TestCase(3, 4.0, 17.635, 124.464)]
        public void Sqr_DoubleTestCaseData_BenchmarkResult(int n, params double[] a)
        {
            var actual = new double[n];
            m_MathVectorOperations.Sqr(n, a, actual);

            var expected = new double[n];
            m_Benchmark.Sqr(n, a, expected);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Sqr(int, double[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        [TestCase(3, 1.0, 2.0, 3.0)]
        [TestCase(3, 4.0, 17.635, 124.464)]
        public void SqrBuildIn_DoubleTestCaseData_BenchmarkResult(int n, params double[] a)
        {
            var actual = a.ToArray();
            m_MathVectorOperations.Sqr(n, actual);

            var expected = a.ToArray();
            m_Benchmark.Sqr(n, expected);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Sqr(int, double[], double[],int,int)"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of the output vector.</param>
        /// <param name="a">The input vector a.</param>
        [TestCase(3, 0, 0, 1.0, 2.0, 4.0)]
        [TestCase(2, 1, 0, 1.0, 2.5, 4.0)]
        [TestCase(2, 1, 2, 21.0, 24.5, 4.56)]
        public void Sqr_ExtendedDoubleTestCaseData_BenchmarkResult(int n, int startIndexA, int startIndexY, params double[] a)
        {
            var actual = new double[startIndexY + n];
            m_MathVectorOperations.Sqr(n, a, actual, startIndexA, startIndexY);

            var expected = new double[startIndexY + n];
            m_Benchmark.Sqr(n, a, expected, startIndexA, startIndexY);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Mul(int, double[], double[], double[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        [TestCaseSource(nameof(TwoDoubleArrayTestCaseData))]
        public void Mul_DoubleTestCaseData_BenchmarkResult(int n, double[] a, double[] b)
        {
            var actual = new double[n];
            m_MathVectorOperations.Mul(n, a, b, actual);

            var expected = new double[n];
            m_Benchmark.Mul(n, a, b, expected);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Mul(int, double[], double[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        [TestCaseSource(nameof(TwoDoubleArrayTestCaseData))]
        public void MulBuildIn_DoubleTestCaseData_BenchmarkResult(int n, double[] a, double[] b)
        {
            var actual = a.ToArray();
            m_MathVectorOperations.Mul(n, actual, b);

            var expected = a.ToArray();
            m_Benchmark.Mul(n, expected, b);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Mul(int, double[], double[], double[],int,int,int)"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexB">The null-based start index of <paramref name="b"/>.</param>
        /// <param name="startIndexY">The null-based start index of the output vector.</param>
        [TestCaseSource(nameof(ExtendedTwoDoubleArrayTestCaseData))]
        public void Mul_ExtendedDoubleTestCaseData_BenchmarkResult(int n, double[] a, double[] b, int startIndexA, int startIndexB, int startIndexY)
        {
            var actual = new double[startIndexY + n];
            m_MathVectorOperations.Mul(n, a, b, actual, startIndexA, startIndexB, startIndexY);

            var expected = new double[startIndexY + n];
            m_Benchmark.Mul(n, a, b, expected, startIndexA, startIndexB, startIndexY);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Mul(int, Complex[], Complex[], Complex[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        [TestCaseSource(nameof(TwoComplexArrayTestCaseData))]
        public void Mul_ComplexTestCaseData_BenchmarkResult(int n, Complex[] a, Complex[] b)
        {
            var actual = new Complex[n];
            m_MathVectorOperations.Mul(n, a, b, actual);

            var expected = new Complex[n];
            m_Benchmark.Mul(n, a, b, expected);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Mul(int, Complex[], Complex[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        [TestCaseSource(nameof(TwoComplexArrayTestCaseData))]
        public void MulBuildIn_ComplexTestCaseData_BenchmarkResult(int n, Complex[] a, Complex[] b)
        {
            var actual = a.ToArray();
            m_MathVectorOperations.Mul(n, actual, b);

            var expected = a.ToArray();
            m_Benchmark.Mul(n, expected, b);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Mul(int, Complex[], Complex[], Complex[],int,int,int)"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexB">The null-based start index of <paramref name="b"/>.</param>
        /// <param name="startIndexY">The null-based start index of the output vector.</param>
        [TestCaseSource(nameof(ExtendedTwoComplexArrayTestCaseData))]
        public void Mul_ExtendedComplexTestCaseData_BenchmarkResult(int n, Complex[] a, Complex[] b, int startIndexA, int startIndexB, int startIndexY)
        {
            var actual = new Complex[startIndexY + n];
            m_MathVectorOperations.Mul(n, a, b, actual, startIndexA, startIndexB, startIndexY);

            var expected = new Complex[startIndexY + n];
            m_Benchmark.Mul(n, a, b, expected, startIndexA, startIndexB, startIndexY);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Conjugate(int, Complex[], Complex[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        [TestCaseSource(nameof(ComplexArrayTestCaseData))]
        public void Conjugate_TestCaseData_BenchmarkResult(int n, Complex[] a)
        {
            var actual = new Complex[n];
            m_MathVectorOperations.Conjugate(n, a, actual);

            var expected = new Complex[n];
            m_Benchmark.Conjugate(n, a, expected);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Conjugate(int, Complex[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        [TestCaseSource(nameof(ComplexArrayTestCaseData))]
        public void ConjugateBuildIn_TestCaseData_BenchmarkResult(int n, Complex[] a)
        {
            var actual = a.ToArray();
            m_MathVectorOperations.Conjugate(n, actual);

            var expected = a.ToArray();
            m_Benchmark.Conjugate(n, expected);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Conjugate(int, Complex[], Complex[],int,int)"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of the output vector.</param>
        [TestCaseSource(nameof(ExtendedComplexArrayTestCaseData))]
        public void Conjugate_ExtendedTestCaseData_BenchmarkResult(int n, Complex[] a, int startIndexA, int startIndexY)
        {
            var actual = new Complex[startIndexY + n];
            m_MathVectorOperations.Conjugate(n, a, actual, startIndexA, startIndexY);

            var expected = new Complex[startIndexY + n];
            m_Benchmark.Conjugate(n, a, expected, startIndexA, startIndexY);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Abs(int, double[], double[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        [TestCase(3, 1.0, -2.0, 4.0)]
        [TestCase(4, -21.0, 24.5, 4.56, -88.762)]
        public void Abs_DoubleTestCaseData_BenchmarkResult(int n, params double[] a)
        {
            var actual = new double[n];
            m_MathVectorOperations.Abs(n, a, actual);

            var expected = new double[n];
            m_Benchmark.Abs(n, a, expected);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Abs(int, double[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        [TestCase(3, 1.0, -2.0, 4.0)]
        [TestCase(4, -21.0, 24.5, 4.56, -88.762)]
        public void AbsBuildIn_DoubleTestCaseData_BenchmarkResult(int n, params double[] a)
        {
            var actual = a.ToArray(); ;
            m_MathVectorOperations.Abs(n, actual);

            var expected = a.ToArray();
            m_Benchmark.Abs(n, expected);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Abs(int, double[], double[],int,int)"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of the output vector.</param>
        /// <param name="a">The input vector a.</param>
        [TestCase(3, 0, 0, 1.0, -2.0, 4.0)]
        [TestCase(2, 1, 0, 1.0, 2.5, -4.23)]
        [TestCase(2, 1, 2, -21.0, 24.5, 4.56)]
        public void Abs_ExtendedDoubleTestCaseData_BenchmarkResult(int n, int startIndexA, int startIndexY, params double[] a)
        {
            var actual = new double[startIndexY + n];
            m_MathVectorOperations.Abs(n, a, actual, startIndexA, startIndexY);

            var expected = new double[startIndexY + n];
            m_Benchmark.Abs(n, a, expected, startIndexA, startIndexY);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Abs(int, Complex[], double[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        [TestCaseSource(nameof(ComplexArrayTestCaseData))]
        public void Abs_TestCaseData_BenchmarkResult(int n, Complex[] a)
        {
            var actual = new double[n];
            m_MathVectorOperations.Abs(n, a, actual);

            var expected = new double[n];
            m_Benchmark.Abs(n, a, expected);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Abs(int, Complex[], double[],int,int)"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of the output vector.</param>
        [TestCaseSource(nameof(ExtendedComplexArrayTestCaseData))]
        public void Abs_ExtendedTestCaseData_BenchmarkResult(int n, Complex[] a, int startIndexA, int startIndexY)
        {
            var actual = new double[startIndexY + n];
            m_MathVectorOperations.Abs(n, a, actual, startIndexA, startIndexY);

            var expected = new double[startIndexY + n];
            m_Benchmark.Abs(n, a, expected, startIndexA, startIndexY);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.LinearFraction(int, double[], double[],double[],double,double,double,double)"/>.</summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="scaleFactorA">The scaling factor for vector a.</param>
        /// <param name="scaleFactorB">The scaling factor for vector b.</param>
        /// <param name="shiftA">Constant value for shifting addends of vector a.</param>
        /// <param name="shiftB">Constant value for shifting addends of vector b.</param>
        [TestCaseSource(nameof(LinearFractionTestCaseData))]
        public void LinearFraction_TestCaseData_BenchmarkResult(int n, double[] a, double[] b, double scaleFactorA, double scaleFactorB, double shiftA = 0.0, double shiftB = 0.0)
        {
            var actual = new double[n];
            m_MathVectorOperations.LinearFraction(n, a, b, actual, scaleFactorA, scaleFactorB, shiftA, shiftB);

            var expected = new double[n];
            m_Benchmark.LinearFraction(n, a, b, expected, scaleFactorA, scaleFactorB, shiftA, shiftB);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.LinearFraction(int,double[],double[],double[],int,int,int,double,double,double,double)"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexB">The null-based start index of <paramref name="b"/>.</param>
        /// <param name="startIndexY">The null-based start index of the output vector.</param>
        /// <param name="scaleFactorA">The scaling factor for vector a.</param>
        /// <param name="scaleFactorB">The scaling factor for vector b.</param>
        /// <param name="shiftA">Constant value for shifting addends of vector a.</param>
        /// <param name="shiftB">Constant value for shifting addends of vector b.</param>
        [TestCaseSource(nameof(ExtendedLinearFractionTestCaseData))]
        public void LinearFraction_ExtendedTestCaseData_BenchmarkResult(int n, double[] a, double[] b, int startIndexA, int startIndexB, int startIndexY, double scaleFactorA, double scaleFactorB, double shiftA = 0.0, double shiftB = 0.0)
        {
            var actual = new double[startIndexY + n];
            m_MathVectorOperations.LinearFraction(n, a, b, actual, startIndexA, startIndexB, startIndexY, scaleFactorA, scaleFactorB, shiftA, shiftB);

            var expected = new double[startIndexY + n];
            m_Benchmark.LinearFraction(n, a, b, expected, startIndexA, startIndexB, startIndexY, scaleFactorA, scaleFactorB, shiftA, shiftB);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }
        #endregion

        #region Power and root functions

        /// <summary>A test function for <see cref="IVectorUnitBasics.Inv(int, double[], double[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector.</param>
        [TestCase(3, 1.0, -2.0, 4.0)]
        [TestCase(4, -21.0, 24.5, 4.56, -88.762)]
        public void Inv_TestCaseData_BenchmarkResult(int n, params double[] a)
        {
            var actual = new double[n];
            m_MathVectorOperations.Inv(n, a, actual);

            var expected = new double[n];
            m_Benchmark.Inv(n, a, expected);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Inv(int, double[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector.</param>
        [TestCase(3, 1.0, -2.0, 4.0)]
        [TestCase(4, -21.0, 24.5, 4.56, -88.762)]
        public void InvBuildIn_TestCaseData_BenchmarkResult(int n, params double[] a)
        {
            var actual = a.ToArray();
            m_MathVectorOperations.Inv(n, actual);

            var expected = a.ToArray();
            m_Benchmark.Inv(n, expected);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Inv(int, double[], double[],int,int)"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of the output vector.</param>
        [TestCase(3, 0, 0, 1.0, -2.0, 4.0)]
        [TestCase(2, 1, 0, 1.0, 2.5, -4.23)]
        [TestCase(2, 1, 2, -21.0, 24.5, 4.56)]
        public void Inv_ExtendedTestCaseData_BenchmarkResult(int n, int startIndexA, int startIndexY, params double[] a)
        {
            var actual = new double[startIndexY + n];
            m_MathVectorOperations.Inv(n, a, actual, startIndexA, startIndexY);

            var expected = new double[startIndexY + n];
            m_Benchmark.Inv(n, a, expected, startIndexA, startIndexY);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Sqrt(int, double[], double[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector.</param>
        [TestCase(3, 1.0, 2.0, 4.0)]
        [TestCase(4, 21.0, 24.5, 4.56, 88.762)]
        public void Sqrt_TestCaseData_BenchmarkResult(int n, params double[] a)
        {
            var actual = new double[n];
            m_MathVectorOperations.Sqrt(n, a, actual);

            var expected = new double[n];
            m_Benchmark.Sqrt(n, a, expected);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Sqrt(int, double[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector.</param>
        [TestCase(3, 1.0, 2.0, 4.0)]
        [TestCase(4, 21.0, 24.5, 4.56, 88.762)]
        public void SqrtBuildIn_TestCaseData_BenchmarkResult(int n, params double[] a)
        {
            var actual = a.ToArray();
            m_MathVectorOperations.Sqrt(n, actual);

            var expected = a.ToArray();
            m_Benchmark.Sqrt(n, expected);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Sqrt(int, double[], double[],int,int)"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of the output vector.</param>
        [TestCase(3, 0, 0, 1.0, 2.0, 4.0)]
        [TestCase(2, 1, 0, 1.0, 2.5, 4.23)]
        [TestCase(2, 1, 2, 21.0, 24.5, 4.56)]
        public void Sqrt_ExtendedTestCaseData_BenchmarkResult(int n, int startIndexA, int startIndexY, params double[] a)
        {
            var actual = new double[startIndexY + n];
            m_MathVectorOperations.Sqrt(n, a, actual, startIndexA, startIndexY);

            var expected = new double[startIndexY + n];
            m_Benchmark.Sqrt(n, a, expected, startIndexA, startIndexY);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Pow(int, double[], double[], double[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        [TestCaseSource(nameof(TwoDoubleArrayTestCaseDataNonNegativeFirstArguments))]
        public void Pow_DoubleTestCaseData_BenchmarkResult(int n, double[] a, double[] b)
        {
            var actual = new double[n];
            m_MathVectorOperations.Pow(n, a, b, actual);

            var expected = new double[n];
            m_Benchmark.Pow(n, a, b, expected);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7).Percent);  // perhaps one number is large
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Pow(int, double[], double[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        [TestCaseSource(nameof(TwoDoubleArrayTestCaseDataNonNegativeFirstArguments))]
        public void PowBuildIn_DoubleTestCaseData_BenchmarkResult(int n, double[] a, double[] b)
        {
            var actual = a.ToArray();
            m_MathVectorOperations.Pow(n, actual, b);

            var expected = a.ToArray();
            m_Benchmark.Pow(n, expected, b);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7).Percent);
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Pow(int, double[], double[], double[],int,int,int)"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexB">The null-based start index of <paramref name="b"/>.</param>
        /// <param name="startIndexY">The null-based start index of the output vector.</param>
        [TestCaseSource(nameof(ExtendedTwoDoubleArrayTestCaseData))]
        public void Pow_ExtendedDoubleTestCaseData_BenchmarkResult(int n, double[] a, double[] b, int startIndexA, int startIndexB, int startIndexY)
        {
            var actual = new double[startIndexY + n];
            m_MathVectorOperations.Pow(n, a, b, actual, startIndexA, startIndexB, startIndexY);

            var expected = new double[startIndexY + n];
            m_Benchmark.Pow(n, a, b, expected, startIndexA, startIndexB, startIndexY);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7).Percent);  // take relative deviation only: perhaps some value is large
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Pow(int, Complex[], Complex[], Complex[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        [TestCaseSource(nameof(TwoComplexArrayTestCaseData))]
        public void Pow_ComplexTestCaseData_BenchmarkResult(int n, Complex[] a, Complex[] b)
        {
            var actual = new Complex[n];
            m_MathVectorOperations.Pow(n, a, b, actual);

            var expected = new Complex[n];
            m_Benchmark.Pow(n, a, b, expected);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Pow(int, Complex[], Complex[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        [TestCaseSource(nameof(TwoComplexArrayTestCaseData))]
        public void PowBuildIn_ComplexTestCaseData_BenchmarkResult(int n, Complex[] a, Complex[] b)
        {
            var actual = a.ToArray();
            m_MathVectorOperations.Pow(n, actual, b);

            var expected = a.ToArray();
            m_Benchmark.Pow(n, expected, b);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Pow(int, Complex[], Complex[], Complex[],int,int,int)"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexB">The null-based start index of <paramref name="b"/>.</param>
        /// <param name="startIndexY">The null-based start index of the output vector.</param>
        [TestCaseSource(nameof(ExtendedTwoComplexArrayTestCaseData))]
        public void Pow_ExtendedComplexTestCaseData_BenchmarkResult(int n, Complex[] a, Complex[] b, int startIndexA, int startIndexB, int startIndexY)
        {
            var actual = new Complex[startIndexY + n];
            m_MathVectorOperations.Pow(n, a, b, actual, startIndexA, startIndexB, startIndexY);

            var expected = new Complex[startIndexY + n];
            m_Benchmark.Pow(n, a, b, expected, startIndexA, startIndexB, startIndexY);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Pow(int, double[], double, double[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector.</param>
        /// <param name="b">The constant power.</param>
        [TestCase(3, 1.24, 1.0, 2.0, 4.0)]
        [TestCase(3, -0.24, 1.0, 2.0, 4.0)]
        [TestCase(4, -5.653, 21.0, 24.5, 4.56, 88.762)]
        public void Pow_TestCaseData_BenchmarkResult(int n, double b, params double[] a)
        {
            var actual = new double[n];
            m_MathVectorOperations.Pow(n, a, b, actual);

            var expected = new double[n];
            m_Benchmark.Pow(n, a, b, expected);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Pow(int, double[], double)"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector.</param>
        /// <param name="b">The constant power.</param>
        [TestCase(3, 1.24, 1.0, 2.0, 4.0)]
        [TestCase(3, -0.24, 1.0, 2.0, 4.0)]
        [TestCase(4, -5.653, 21.0, 24.5, 4.56, 88.762)]
        public void PowBuildIn_TestCaseData_BenchmarkResult(int n, double b, params double[] a)
        {
            var actual = a.ToArray();
            m_MathVectorOperations.Pow(n, actual, b);

            var expected = a.ToArray();
            m_Benchmark.Pow(n, expected, b);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Pow(int, double[], double, double[], int, int)"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector.</param>
        /// <param name="b">The constant power.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of the output vector.</param>
        [TestCase(3, 1.2, 0, 0, 1.0, 2.0, 4.0)]
        [TestCase(3, 0.0, 0, 0, 1.0, 2.0, 4.0)]
        [TestCase(2, -0.26, 1, 0, 1.0, 2.5, 4.23)]
        [TestCase(2, 3.86, 1, 0, 1.0, 2.5, 4.23)]
        [TestCase(2, 1.52, 1, 2, 21.0, 24.5, 4.56)]
        public void Pow_ExtendedTestCaseData_BenchmarkResult(int n, double b, int startIndexA, int startIndexY, params double[] a)
        {
            var actual = new double[startIndexY + n];
            m_MathVectorOperations.Pow(n, a, b, actual, startIndexA, startIndexY);

            var expected = new double[startIndexY + n];
            m_Benchmark.Pow(n, a, b, expected, startIndexA, startIndexY);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Pow(int, Complex[], Complex, Complex[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector.</param>
        [TestCaseSource(nameof(ComplexArrayTestCaseData))]
        public void Pow_TestCaseData_BenchmarkResult(int n, Complex[] a)
        {
            // assume a specified constant power
            Complex b = new Complex(-0.81, 1.23);

            var actual = new Complex[n];
            m_MathVectorOperations.Pow(n, a, b, actual);

            var expected = new Complex[n];
            m_Benchmark.Pow(n, a, b, expected);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-7), String.Format("constant power: {0}", b));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Pow(int, Complex[], Complex)"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector.</param>
        [TestCaseSource(nameof(ComplexArrayTestCaseData))]
        public void PowBuildIn_TestCaseData_BenchmarkResult(int n, Complex[] a)
        {
            // assume a specified constant power
            Complex b = new Complex(-0.81, 1.23);

            var actual = a.ToArray();
            m_MathVectorOperations.Pow(n, actual, b);

            var expected = a.ToArray();
            m_Benchmark.Pow(n, expected, b);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-7), String.Format("constant power: {0}", b));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Pow(int, Complex[], Complex, Complex[], int, int)"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of the output vector.</param>
        [TestCaseSource(nameof(ExtendedComplexArrayTestCaseData))]
        public void Pow_ExtendedTestCaseData_BenchmarkResult(int n, Complex[] a, int startIndexA, int startIndexY)
        {
            // assume a specified constant power
            Complex b = new Complex(0.51, 1.23);

            var actual = new Complex[startIndexY + n];
            m_MathVectorOperations.Pow(n, a, b, actual, startIndexA, startIndexY);

            var expected = new Complex[startIndexY + n];
            m_Benchmark.Pow(n, a, b, expected, startIndexA, startIndexY);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-7), String.Format("constant power: {0}", b));
        }
        #endregion

        #region Exponential and logarithmic functions

        /// <summary>A test function for <see cref="IVectorUnitBasics.Exp(int, double[], double[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        [TestCase(3, -1.0, 2.0, 3.0)]
        [TestCase(3, 4.0, 17.635, -24.464)]
        public void Exp_DoubleTestCaseData_BenchmarkResult(int n, params double[] a)
        {
            var actual = new double[n];
            m_MathVectorOperations.Exp(n, a, actual);

            var expected = new double[n];
            m_Benchmark.Exp(n, a, expected);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Exp(int, double[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        [TestCase(3, -1.0, 2.0, 3.0)]
        [TestCase(3, 4.0, 17.635, -24.464)]
        public void ExpBuildIn_DoubleTestCaseData_BenchmarkResult(int n, params double[] a)
        {
            var actual = a.ToArray();
            m_MathVectorOperations.Exp(n, actual);

            var expected = a.ToArray();
            m_Benchmark.Exp(n, expected);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Exp(int, double[], double[],int,int)"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of the output vector.</param>
        /// <param name="a">The input vector a.</param>
        [TestCase(3, 0, 0, 1.0, 2.0, 4.0)]
        [TestCase(2, 1, 0, 1.0, 2.5, 4.0)]
        [TestCase(2, 1, 2, -21.0, -24.5, 4.56)]
        public void Exp_ExtendedDoubleTestCaseData_BenchmarkResult(int n, int startIndexA, int startIndexY, params double[] a)
        {
            var actual = new double[startIndexY + n];
            m_MathVectorOperations.Exp(n, a, actual, startIndexA, startIndexY);

            var expected = new double[startIndexY + n];
            m_Benchmark.Exp(n, a, expected, startIndexA, startIndexY);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Exp(int, Complex[], Complex[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        [TestCaseSource(nameof(ComplexArrayTestCaseData))]
        public void Exp_TestCaseData_BenchmarkResult(int n, Complex[] a)
        {
            var actual = new Complex[n];
            m_MathVectorOperations.Exp(n, a, actual);

            var expected = new Complex[n];
            m_Benchmark.Exp(n, a, expected);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Exp(int, Complex[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        [TestCaseSource(nameof(ComplexArrayTestCaseData))]
        public void ExpBuildIn_TestCaseData_BenchmarkResult(int n, Complex[] a)
        {
            var actual = a.ToArray();
            m_MathVectorOperations.Exp(n, actual);

            var expected = a.ToArray();
            m_Benchmark.Exp(n, expected);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Exp(int, Complex[], Complex[],int,int)"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of the output vector.</param>
        [TestCaseSource(nameof(ExtendedComplexArrayTestCaseData))]
        public void Exp_ExtendedTestCaseData_BenchmarkResult(int n, Complex[] a, int startIndexA, int startIndexY)
        {
            var actual = new Complex[startIndexY + n];
            m_MathVectorOperations.Exp(n, a, actual, startIndexA, startIndexY);

            var expected = new Complex[startIndexY + n];
            m_Benchmark.Exp(n, a, expected, startIndexA, startIndexY);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Log(int, double[], double[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        [TestCase(3, 1.0, 2.0, 3.0)]
        [TestCase(3, 4.0, 17.635, 24.464)]
        public void Log_DoubleTestCaseData_BenchmarkResult(int n, params double[] a)
        {
            var actual = new double[n];
            m_MathVectorOperations.Log(n, a, actual);

            var expected = new double[n];
            m_Benchmark.Log(n, a, expected);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Log(int, double[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        [TestCase(3, 1.0, 2.0, 3.0)]
        [TestCase(3, 4.0, 17.635, 24.464)]
        public void LogBuildIn_DoubleTestCaseData_BenchmarkResult(int n, params double[] a)
        {
            var actual = a.ToArray();
            m_MathVectorOperations.Log(n, actual);

            var expected = a.ToArray();
            m_Benchmark.Log(n, expected);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Log(int, double[], double[],int,int)"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of the output vector.</param>
        /// <param name="a">The input vector a.</param>
        [TestCase(3, 0, 0, 1.0, 2.0, 4.0)]
        [TestCase(2, 1, 0, 1.0, 2.5, 4.0)]
        [TestCase(2, 1, 2, 21.0, 24.5, 4.56)]
        public void Log_ExtendedDoubleTestCaseData_BenchmarkResult(int n, int startIndexA, int startIndexY, params double[] a)
        {
            var actual = new double[startIndexY + n];
            m_MathVectorOperations.Log(n, a, actual, startIndexA, startIndexY);

            var expected = new double[startIndexY + n];
            m_Benchmark.Log(n, a, expected, startIndexA, startIndexY);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Log(int, Complex[], Complex[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        [TestCaseSource(nameof(ComplexArrayTestCaseData))]
        public void Log_TestCaseData_BenchmarkResult(int n, Complex[] a)
        {
            var actual = new Complex[n];
            m_MathVectorOperations.Log(n, a, actual);

            var expected = new Complex[n];
            m_Benchmark.Log(n, a, expected);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Log(int, Complex[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        [TestCaseSource(nameof(ComplexArrayTestCaseData))]
        public void LogBuildIn_TestCaseData_BenchmarkResult(int n, Complex[] a)
        {
            var actual = a.ToArray();
            m_MathVectorOperations.Log(n, actual);

            var expected = a.ToArray();
            m_Benchmark.Log(n, expected);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Log(int, Complex[], Complex[],int,int)"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of the output vector.</param>
        [TestCaseSource(nameof(ExtendedComplexArrayTestCaseData))]
        public void Log_ExtendedTestCaseData_BenchmarkResult(int n, Complex[] a, int startIndexA, int startIndexY)
        {
            var actual = new Complex[startIndexY + n];
            m_MathVectorOperations.Log(n, a, actual, startIndexA, startIndexY);

            var expected = new Complex[startIndexY + n];
            m_Benchmark.Log(n, a, expected, startIndexA, startIndexY);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Log10(int, double[], double[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        [TestCase(3, 1.0, 2.0, 3.0)]
        [TestCase(3, 4.0, 17.635, 24.464)]
        public void Log10_DoubleTestCaseData_BenchmarkResult(int n, params double[] a)
        {
            var actual = new double[n];
            m_MathVectorOperations.Log10(n, a, actual);

            var expected = new double[n];
            m_Benchmark.Log10(n, a, expected);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Log10(int, double[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        [TestCase(3, 1.0, 2.0, 3.0)]
        [TestCase(3, 4.0, 17.635, 24.464)]
        public void Log10BuildIn_DoubleTestCaseData_BenchmarkResult(int n, params double[] a)
        {
            var actual = a.ToArray();
            m_MathVectorOperations.Log10(n, actual);

            var expected = a.ToArray();
            m_Benchmark.Log10(n, expected);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Log10(int, double[], double[],int,int)"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of the output vector.</param>
        /// <param name="a">The input vector a.</param>
        [TestCase(3, 0, 0, 1.0, 2.0, 4.0)]
        [TestCase(2, 1, 0, 1.0, 2.5, 4.0)]
        [TestCase(2, 1, 2, 21.0, 24.5, 4.56)]
        public void Log10_ExtendedDoubleTestCaseData_BenchmarkResult(int n, int startIndexA, int startIndexY, params double[] a)
        {
            var actual = new double[startIndexY + n];
            m_MathVectorOperations.Log10(n, a, actual, startIndexA, startIndexY);

            var expected = new double[startIndexY + n];
            m_Benchmark.Log10(n, a, expected, startIndexA, startIndexY);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Log10(int, Complex[], Complex[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        [TestCaseSource(nameof(ComplexArrayTestCaseData))]
        public void Log10_TestCaseData_BenchmarkResult(int n, Complex[] a)
        {
            var actual = new Complex[n];
            m_MathVectorOperations.Log10(n, a, actual);

            var expected = new Complex[n];
            m_Benchmark.Log10(n, a, expected);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Log10(int, Complex[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        [TestCaseSource(nameof(ComplexArrayTestCaseData))]
        public void Log10BuildIn_TestCaseData_BenchmarkResult(int n, Complex[] a)
        {
            var actual = a.ToArray();
            m_MathVectorOperations.Log10(n, actual);

            var expected = a.ToArray();
            m_Benchmark.Log10(n, expected);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Log10(int, Complex[], Complex[],int,int)"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of the output vector.</param>
        [TestCaseSource(nameof(ExtendedComplexArrayTestCaseData))]
        public void Log10_ExtendedTestCaseData_BenchmarkResult(int n, Complex[] a, int startIndexA, int startIndexY)
        {
            var actual = new Complex[startIndexY + n];
            m_MathVectorOperations.Log10(n, a, actual, startIndexA, startIndexY);

            var expected = new Complex[startIndexY + n];
            m_Benchmark.Log10(n, a, expected, startIndexA, startIndexY);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-7));
        }
        #endregion

        #region Trigonometric functions

        /// <summary>A test function for <see cref="IVectorUnitBasics.Cos(int, double[], double[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        [TestCase(3, -1.0, 2.0, 3.0)]
        [TestCase(3, 4.0, 17.635, -24.464)]
        public void Cos_DoubleTestCaseData_BenchmarkResult(int n, params double[] a)
        {
            var actual = new double[n];
            m_MathVectorOperations.Cos(n, a, actual);

            var expected = new double[n];
            m_Benchmark.Cos(n, a, expected);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Cos(int, double[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        [TestCase(3, -1.0, 2.0, 3.0)]
        [TestCase(3, 4.0, 17.635, -24.464)]
        public void CosBuildIn_DoubleTestCaseData_BenchmarkResult(int n, params double[] a)
        {
            var actual = a.ToArray();
            m_MathVectorOperations.Cos(n, actual);

            var expected = a.ToArray();
            m_Benchmark.Cos(n, expected);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Cos(int, double[], double[],int,int)"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of the output vector.</param>
        /// <param name="a">The input vector a.</param>
        [TestCase(3, 0, 0, 1.0, 2.0, 4.0)]
        [TestCase(2, 1, 0, 1.0, 2.5, 4.0)]
        [TestCase(2, 1, 2, -21.0, -24.5, 4.56)]
        public void Cos_ExtendedDoubleTestCaseData_BenchmarkResult(int n, int startIndexA, int startIndexY, params double[] a)
        {
            var actual = new double[startIndexY + n];
            m_MathVectorOperations.Cos(n, a, actual, startIndexA, startIndexY);

            var expected = new double[startIndexY + n];
            m_Benchmark.Cos(n, a, expected, startIndexA, startIndexY);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Cos(int, Complex[], Complex[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        [TestCaseSource(nameof(ComplexArrayTestCaseData))]
        public void Cos_TestCaseData_BenchmarkResult(int n, Complex[] a)
        {
            var actual = new Complex[n];
            m_MathVectorOperations.Cos(n, a, actual);

            var expected = new Complex[n];
            m_Benchmark.Cos(n, a, expected);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Cos(int, Complex[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        [TestCaseSource(nameof(ComplexArrayTestCaseData))]
        public void CosBuildIn_TestCaseData_BenchmarkResult(int n, Complex[] a)
        {
            var actual = a.ToArray();
            m_MathVectorOperations.Cos(n, actual);

            var expected = a.ToArray();
            m_Benchmark.Cos(n, expected);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Cos(int, Complex[], Complex[],int,int)"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of the output vector.</param>
        [TestCaseSource(nameof(ExtendedComplexArrayTestCaseData))]
        public void Cos_ExtendedTestCaseData_BenchmarkResult(int n, Complex[] a, int startIndexA, int startIndexY)
        {
            var actual = new Complex[startIndexY + n];
            m_MathVectorOperations.Cos(n, a, actual, startIndexA, startIndexY);

            var expected = new Complex[startIndexY + n];
            m_Benchmark.Cos(n, a, expected, startIndexA, startIndexY);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Sin(int, double[], double[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        [TestCase(3, -1.0, 2.0, 3.0)]
        [TestCase(3, 4.0, 17.635, -24.464)]
        public void Sin_DoubleTestCaseData_BenchmarkResult(int n, params double[] a)
        {
            var actual = new double[n];
            m_MathVectorOperations.Sin(n, a, actual);

            var expected = new double[n];
            m_Benchmark.Sin(n, a, expected);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Sin(int, double[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        [TestCase(3, -1.0, 2.0, 3.0)]
        [TestCase(3, 4.0, 17.635, -24.464)]
        public void SinBuildIn_DoubleTestCaseData_BenchmarkResult(int n, params double[] a)
        {
            var actual = a.ToArray();
            m_MathVectorOperations.Sin(n, actual);

            var expected = a.ToArray();
            m_Benchmark.Sin(n, expected);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Sin(int, double[], double[],int,int)"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of the output vector.</param>
        /// <param name="a">The input vector a.</param>
        [TestCase(3, 0, 0, 1.0, 2.0, 4.0)]
        [TestCase(2, 1, 0, 1.0, 2.5, 4.0)]
        [TestCase(2, 1, 2, -21.0, -24.5, 4.56)]
        public void Sin_ExtendedDoubleTestCaseData_BenchmarkResult(int n, int startIndexA, int startIndexY, params double[] a)
        {
            var actual = new double[startIndexY + n];
            m_MathVectorOperations.Sin(n, a, actual, startIndexA, startIndexY);

            var expected = new double[startIndexY + n];
            m_Benchmark.Sin(n, a, expected, startIndexA, startIndexY);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Sin(int, Complex[], Complex[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        [TestCaseSource(nameof(ComplexArrayTestCaseData))]
        public void Sin_TestCaseData_BenchmarkResult(int n, Complex[] a)
        {
            var actual = new Complex[n];
            m_MathVectorOperations.Sin(n, a, actual);

            var expected = new Complex[n];
            m_Benchmark.Sin(n, a, expected);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Sin(int, Complex[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        [TestCaseSource(nameof(ComplexArrayTestCaseData))]
        public void SinBuildIn_TestCaseData_BenchmarkResult(int n, Complex[] a)
        {
            var actual = a.ToArray();
            m_MathVectorOperations.Sin(n, actual);

            var expected = a.ToArray();
            m_Benchmark.Sin(n, expected);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.Sin(int, Complex[], Complex[],int,int)"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of the output vector.</param>
        [TestCaseSource(nameof(ExtendedComplexArrayTestCaseData))]
        public void Sin_ExtendedTestCaseData_BenchmarkResult(int n, Complex[] a, int startIndexA, int startIndexY)
        {
            var actual = new Complex[startIndexY + n];
            m_MathVectorOperations.Sin(n, a, actual, startIndexA, startIndexY);

            var expected = new Complex[startIndexY + n];
            m_Benchmark.Sin(n, a, expected, startIndexA, startIndexY);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-7));
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.SinCos(int, double[], double[], double[])"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        [TestCase(3, -1.0, 2.0, 3.0)]
        [TestCase(3, 4.0, 17.635, -24.464)]
        public void SinCos_DoubleTestCaseData_BenchmarkResult(int n, params double[] a)
        {
            var actualSin = new double[n];
            var actualCos = new double[n];
            m_MathVectorOperations.SinCos(n, a, actualSin, actualCos);

            var expectedSin = new double[n];
            var expectedCos = new double[n];
            m_Benchmark.SinCos(n, a, expectedSin, expectedCos);

            Assert.That(actualSin, Is.EqualTo(expectedSin).AsCollection.Within(1E-7), "Sin");
            Assert.That(actualCos, Is.EqualTo(expectedCos).AsCollection.Within(1E-7), "Cos");
        }

        /// <summary>A test function for <see cref="IVectorUnitBasics.SinCos(int, double[], double[], double[],int,int,int)"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of the sinus output vector.</param>
        /// <param name="startIndexZ">The null-based start index of the cosinus output vector.</param>
        /// <param name="a">The input vector a.</param>
        [TestCase(3, 0, 0, 0, 1.0, 2.0, 4.0)]
        [TestCase(2, 1, 0, 1, 1.0, 2.5, 4.0)]
        [TestCase(2, 1, 2, 3, -21.0, -24.5, 4.56)]
        public void SinCos_ExtendedDoubleTestCaseData_BenchmarkResult(int n, int startIndexA, int startIndexY, int startIndexZ, params double[] a)
        {
            var actualSin = new double[startIndexY + n];
            var actualCos = new double[startIndexZ + n];

            m_MathVectorOperations.SinCos(n, a, actualSin, actualCos, startIndexA, startIndexY, startIndexZ);

            var expectedSin = new double[startIndexY + n];
            var expectedCos = new double[startIndexZ + n];
            m_Benchmark.SinCos(n, a, expectedSin, expectedCos, startIndexA, startIndexY, startIndexZ);

            Assert.That(actualSin, Is.EqualTo(expectedSin).AsCollection.Within(1E-7), "Sin");
            Assert.That(actualCos, Is.EqualTo(expectedCos).AsCollection.Within(1E-7), "Cos");
        }
        #endregion

        #region testcase data

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects where the first item is the number of elements and the second is an arrays of complex numbers.
        /// </summary>
        /// <value>The complex array test case data.</value>
        public static IEnumerable<TestCaseData> ComplexArrayTestCaseData
        {
            get
            {
                yield return new TestCaseData(3, new Complex[] { 1.2, 3.5, -5.12 });
                yield return new TestCaseData(3, new Complex[] { new Complex(1.2, -0.9), 3.5, new Complex(-5.12, 1.23) });
                yield return new TestCaseData(3, new Complex[] { new Complex(-0.3, 1.0), new Complex(10.4, -1.0), 64.1 });
            }
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects where the first item is the number of elements, the second 
        /// is an of complex numbers, it follows the null-based startindex of the arrray and the null-based startindex of the output vector.
        /// </summary>
        /// <value>The extended complex array test case data.</value>
        public static IEnumerable<TestCaseData> ExtendedComplexArrayTestCaseData
        {
            get
            {
                yield return new TestCaseData(3, new Complex[] { 1.2, 3.5, -5.12 }, 0, 0);
                yield return new TestCaseData(3, new Complex[] { 1.2, 3.5, -5.12 }, 0, 2);
                yield return new TestCaseData(2, new Complex[] { 1.2, 3.5, -5.12 }, 1, 0);

                yield return new TestCaseData(3, new Complex[] { new Complex(1.2, -0.9), 3.5, new Complex(-5.12, 1.23) }, 0, 0);
                yield return new TestCaseData(3, new Complex[] { new Complex(1.2, -0.9), 3.5, new Complex(-5.12, 1.23) }, 0, 5);
                yield return new TestCaseData(2, new Complex[] { new Complex(1.2, -0.9), 3.5, new Complex(-5.12, 1.23) }, 1, 3);

                yield return new TestCaseData(3, new Complex[] { new Complex(-0.3, 1.0), new Complex(10.4, -1.0), 64.1 }, 0, 3);
                yield return new TestCaseData(2, new Complex[] { new Complex(-0.3, 1.0), new Complex(10.4, -1.0), 64.1 }, 1, 2);
            }
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects where the first item is the number of elements, the second and third 
        /// are arrays of floating point numbers.
        /// </summary>
        /// <value>The two double array test case data.</value>
        public static IEnumerable<TestCaseData> TwoDoubleArrayTestCaseData
        {
            get
            {
                yield return new TestCaseData(3, new double[] { 1.2, 3.5, -5.12 }, new double[] { -0.3, 10.4, 64.1 });
                yield return new TestCaseData(4, new double[] { -2.4, 25, 1.25, 6.12 }, new double[] { -0.3, 12.4, 42.1, -0.09 });
            }
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects where the first item is the number of elements, the second 
        /// is an array of floating point numbers and the third is a floating point number.
        /// </summary>
        /// <value>The two double array test case data.</value>
        public static IEnumerable<TestCaseData> DoubleArrayDoubleValueTestCaseData
        {
            get
            {
                yield return new TestCaseData(3, new double[] { 1.2, 3.5, -5.12 }, -0.3);
                yield return new TestCaseData(3, new double[] { 1.2, 3.5, -5.12 }, 10.4);

                yield return new TestCaseData(4, new double[] { -2.4, 25, 1.25, 6.12 }, -0.3);
                yield return new TestCaseData(4, new double[] { -2.4, 25, 1.25, 6.12 }, 12.4);
                yield return new TestCaseData(4, new double[] { -2.4, 25, 1.25, 6.12 }, 42.1);
            }
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects where the first item is the number of elements, the second (positive numbers only) and third 
        /// are arrays of floating point numbers.
        /// </summary>
        /// <value>The two double array test case data.</value>
        public static IEnumerable<TestCaseData> TwoDoubleArrayTestCaseDataNonNegativeFirstArguments
        {
            get
            {
                yield return new TestCaseData(3, new double[] { 1.2, 3.5, 5.12 }, new double[] { -0.3, 10.4, 64.1 });
                yield return new TestCaseData(4, new double[] { 2.4, 25, 1.25, 6.12 }, new double[] { -0.3, 12.4, 42.1, -0.09 });
            }
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects where the first item is the number of elements, the second and 
        /// third are arrays of floating point numbers, it follows the null-based startindex of the first arrray and the 
        /// null-based startindex of the second array to take into account and finally the null-based startindex of the output vector.
        /// </summary>
        /// <value>The extended two double array test case data.</value>
        public static IEnumerable<TestCaseData> ExtendedTwoDoubleArrayTestCaseData
        {
            get
            {
                yield return new TestCaseData(3, new double[] { 1.2, 3.5, -5.12 }, new double[] { -0.3, 10.4, 64.1 }, 0, 0, 0);
                yield return new TestCaseData(3, new double[] { 1.2, 3.5, -5.12 }, new double[] { -0.3, 10.4, 64.1 }, 0, 0, 2);
                yield return new TestCaseData(2, new double[] { 1.2, 3.5, -5.12 }, new double[] { -0.3, 10.4 }, 1, 0, 1);

                yield return new TestCaseData(4, new double[] { -2.4, 25, 1.25, 6.12 }, new double[] { -0.3, 12.4, 42.1, -0.09 }, 0, 0, 0);
                yield return new TestCaseData(4, new double[] { -2.4, 25, 1.25, 6.12 }, new double[] { -0.3, 12.4, 42.1, -0.09 }, 0, 0, 3);
                yield return new TestCaseData(2, new double[] { -2.4, 25, 1.25, 6.12 }, new double[] { -0.3, 12.4, 42.1, -0.09 }, 1, 2, 1);
            }
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects where the first item is the number of elements, the second and third 
        /// are arrays of complex numbers.
        /// </summary>
        /// <value>The two complex array test case data.</value>
        public static IEnumerable<TestCaseData> TwoComplexArrayTestCaseData
        {
            get
            {
                yield return new TestCaseData(3, new Complex[] { 1.2, 3.5, -5.12 }, new Complex[] { -0.3, 10.4, 64.1 });
                yield return new TestCaseData(3, new Complex[] { new Complex(1.2, -0.9), 3.5, new Complex(-5.12, 1.23) }, new Complex[] { new Complex(-0.3, 1.0), new Complex(10.4, -1.0), 64.1 });
            }
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects where the first item is the number of elements, the second and 
        /// third are arrays of complex numbers, it follows the null-based startindex of the first arrray and the 
        /// null-based startindex of the second array to take into account and finally the null-based startindex of the output vector.
        /// </summary>
        /// <value>The extended two complex array test case data.</value>
        public static IEnumerable<TestCaseData> ExtendedTwoComplexArrayTestCaseData
        {
            get
            {
                yield return new TestCaseData(3, new Complex[] { 1.2, 3.5, -5.12 }, new Complex[] { -0.3, 10.4, 64.1 }, 0, 0, 0);
                yield return new TestCaseData(3, new Complex[] { 1.2, 3.5, -5.12 }, new Complex[] { -0.3, 10.4, 64.1 }, 0, 0, 2);
                yield return new TestCaseData(2, new Complex[] { 1.2, 3.5, -5.12 }, new Complex[] { -0.3, 10.4, 64.1 }, 1, 0, 3);

                yield return new TestCaseData(3, new Complex[] { new Complex(1.2, -0.9), 3.5, new Complex(-5.12, 1.23) }, new Complex[] { new Complex(-0.3, 1.0), new Complex(10.4, -1.0), 64.1 }, 0, 0, 0);
                yield return new TestCaseData(3, new Complex[] { new Complex(1.2, -0.9), 3.5, new Complex(-5.12, 1.23) }, new Complex[] { new Complex(-0.3, 1.0), new Complex(10.4, -1.0), 64.1 }, 0, 0, 2);
                yield return new TestCaseData(2, new Complex[] { new Complex(1.2, -0.9), 3.5, new Complex(-5.12, 1.23) }, new Complex[] { new Complex(-0.3, 1.0), new Complex(10.4, -1.0), 64.1 }, 1, 0, 0);
                yield return new TestCaseData(2, new Complex[] { new Complex(1.2, -0.9), 3.5, new Complex(-5.12, 1.23) }, new Complex[] { new Complex(-0.3, 1.0), new Complex(10.4, -1.0), 64.1 }, 1, 1, 0);
            }
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for <see cref="LinearFraction_TestCaseData_BenchmarkResult(int, double[], double[], double, double, double, double)"/>.
        /// </summary>
        /// <value>The linear fraction test case data.</value>
        public static IEnumerable<TestCaseData> LinearFractionTestCaseData
        {
            get
            {
                // Input values: n, a, b, scaleFactorA, scaleFactorB, shiftA, shiftB
                yield return new TestCaseData(3, new double[] { 1.7, 23, 51.34 }, new double[] { -1.23, 43.92, 7.9 }, 1.0, 1.0, 0.0, 0.0);
                yield return new TestCaseData(3, new double[] { 1.7, 23, 51.34 }, new double[] { -1.23, 43.92, 7.9 }, -1.52, 12.0, 0.0, 0.0);
                yield return new TestCaseData(3, new double[] { 1.7, 23, 51.34 }, new double[] { -1.23, 43.92, 7.9 }, -1.52, 12.0, 0.5, -1.26);

            }
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for <see cref="LinearFraction_ExtendedTestCaseData_BenchmarkResult(int, double[], double[], int, int, int, double, double, double, double)"/>.
        /// </summary>
        /// <value>The extended linear fraction test case data.</value>
        public static IEnumerable<TestCaseData> ExtendedLinearFractionTestCaseData
        {
            get
            {
                // Input values:  n, a, b, startIndexA, startIndexB, startIndexY, scaleFactorA, scaleFactorB, shiftA, shiftB
                yield return new TestCaseData(3, new double[] { 1.7, 23, 51.34 }, new double[] { -1.23, 43.92, 7.9 }, 0, 0, 0, 1.0, 1.0, 0.0, 0.0);
                yield return new TestCaseData(3, new double[] { 1.7, 23, 51.34 }, new double[] { -1.23, 43.92, 7.9 }, 0, 0, 2, 1.0, 1.0, 0.0, 0.0);
                yield return new TestCaseData(2, new double[] { 1.7, 23, 51.34 }, new double[] { -1.23, 43.92, 7.9 }, 1, 0, 1, 1.25, -0.85, 2.5, 0.0);

                yield return new TestCaseData(3, new double[] { 1.7, 23, 31.34 }, new double[] { -1.23, 13.92, 7.9 }, 0, 0, 2, -1.52, 12.0, 0.0, 0.0);
                yield return new TestCaseData(2, new double[] { 1.7, 23, 21.34 }, new double[] { -1.23, 13.92, 7.9 }, 1, 1, 2, -1.52, 1.0, -0.124, 1.2);
                yield return new TestCaseData(3, new double[] { 1.7, 23, 11.34 }, new double[] { -1.23, 23.92, 7.9 }, 0, 0, 1, -1.52, 12.0, 0.5, -1.26);
            }
        }
        #endregion

        #endregion

        #region protected methods

        /// <summary>Creates the test object, i.e. the <see cref="IVectorUnitBasics"/> object under test.
        /// </summary>
        /// <returns>The <see cref="IVectorUnitBasics"/> object to test.</returns>
        protected abstract IVectorUnitBasics CreateTestObject();
        #endregion
    }
}