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
    /// <summary>Serves as abstract unit test class for Level 2 BLAS methods.
    /// </summary>
    public abstract class Level2BLASTests
    {
        #region private members

        /// <summary>The Level 2 BLAS implementation to test.
        /// </summary>
        private ILevel2BLAS m_TestObject;

        /// <summary>A benchmark Level 2 BLAS implementation.
        /// </summary>
        private BuildInLevel2BLAS m_BenchmarkObject;
        #endregion

        #region protected constructors

        /// <summary>Initializes a new instance of the <see cref="Level2BLASTests"/> class.
        /// </summary>
        protected Level2BLASTests()
        {
        }
        #endregion

        #region public methods

        /// <summary>The SetUp method for the unit tests.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            m_BenchmarkObject = new BuildInLevel2BLAS();
            m_TestObject = GetLevel2BLAS();
        }

        #region double precision methods

        /// <summary>A test function that compares the result of 'DGBMV' to the benchmark implementation.
        /// </summary>
        /// <param name="m">The number of rows of matrix A.</param>
        /// <param name="n">The number of columns of matrix A.</param>
        /// <param name="kl">The number of sub-diagonals of matrix A.</param>
        /// <param name="ku">The number of super-diagonals of matrix A.</param>
        /// <param name="alpha">The scalar factor \alpha.</param>
        /// <param name="a">The general band matrix A of dimension (<paramref name="lda"/>, <paramref name="n"/>). The leading (<paramref name="kl"/> + <paramref name="ku"/> + 1) by <paramref name="n"/> part 
        /// must contain the matrix of coefficients (supplied column-by-column) with the leading diagonal of the matrix 
        /// in row (<paramref name="ku"/> + 1) of the array, the first super-diagonal starting at position 2 in row ku, 
        /// the first sub-diagonal starting at position 1 in row (ku + 2), etc. 
        /// The following program segment transfers a band matrix from conventional full matrix storage to band storage:
        /// <code>
        /// for j = 0 to n-1
        ///   k = ku - j
        ///   for i = max(0, j-ku) to min(m-1, j+kl-1) 
        ///     a(k+i, j) = matrix(i,j)
        ///   end
        /// end
        /// </code>
        /// </param>
        /// <param name="x">The vector x with at least 1+(<paramref name="n"/>-1) * |<paramref name="incX"/>| elements if 'op(A)=A'; 1+(<paramref name="m"/>-1) *|<paramref name="incX"/>| otherwise.</param>
        /// <param name="beta">The scalar factor \beta.</param>
        /// <param name="y">The vector y with at least 1+(<paramref name="m"/>-1) *|<paramref name="incY"/>| elements if 'op(A)=A'; 1+(<paramref name="n"/>-1) * |<paramref name="incY"/>| otherwise (input/output).</param>
        /// <param name="lda">The leading dimension, must be at least <paramref name="kl"/> + <paramref name="ku"/> +1.</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
        [TestCaseSource(nameof(TestCaseData_dgbmv))]
        public void dgbmv_TestCaseData_ResultOfBenchmarkImplementation(int m, int n, int kl, int ku, double alpha, double[] a, double[] x, double beta, double[] y, int lda, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1, int incY = 1)
        {
            var actual = y.ToArray();
            m_TestObject.dgbmv(m, n, kl, ku, alpha, a, x, beta, actual, lda, transpose, incX, incY);

            var expected = y.ToArray();
            m_BenchmarkObject.dgbmv(m, n, kl, ku, alpha, a, x, beta, expected, lda, transpose, incX, incY);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'DGBMV'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_dgbmv
        {
            get
            {
                // squared input matrix
                yield return new TestCaseData(4, 4, 2, 2, -1.25, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new double[] { 11, 12, 13, 14 }, -1.5, new double[] { -16, -15, -14, -13 }, 5, BLAS.MatrixTransposeState.NoTranspose, 1, 1);
                yield return new TestCaseData(4, 4, 2, 2, 3.7, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new double[] { 11, 12, 13, 14 }, 2.4, new double[] { -16, -15, -14, -13 }, 5, BLAS.MatrixTransposeState.Transpose, 1, 1);

                // non-squared input matrices
                yield return new TestCaseData(3, 4, 2, 2, -1.25, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new double[] { 11, 12, 13, 14 }, -5.76, new double[] { -16, -15, -14, -13 }, 5, BLAS.MatrixTransposeState.NoTranspose, 1, 1);
                yield return new TestCaseData(4, 3, 2, 2, 3.7, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new double[] { 11, 12, 13, 14 }, 3.54, new double[] { -16, -15, -14, -13 }, 5, BLAS.MatrixTransposeState.Transpose, 1, 1);
                yield return new TestCaseData(3, 4, 1, 2, -1.25, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new double[] { 11, 12, 13, 14 }, -2.5, new double[] { -16, -15, -14, -13 }, 4, BLAS.MatrixTransposeState.NoTranspose, 1, 1);
                yield return new TestCaseData(4, 3, 2, 1, 3.7, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new double[] { 11, 12, 13, 14 }, 1.5, new double[] { -16, -15, -14, -13 }, 4, BLAS.MatrixTransposeState.Transpose, 1, 1);


                yield return new TestCaseData(3, 4, 1, 2, -1.25, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new double[] { 11, 12, 13, 14, 5, 7, 8 }, -1.87, new double[] { -16, -15, -14, -13 }, 4, BLAS.MatrixTransposeState.NoTranspose, 2, 1);
                yield return new TestCaseData(4, 3, 2, 1, 0.7, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new double[] { 11, 12, 13, 14, 9.5 }, 1.25, new double[] { -16, -15, -14, -13, 1.0, 4.5, 7.1 }, 4, BLAS.MatrixTransposeState.NoTranspose, 2, 2);
                yield return new TestCaseData(4, 3, 2, 1, -1.2, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new double[] { 11, 12, 13, 14, 9.5, 6.4, 1.2 }, 1.25, new double[] { -16, -15, -14, -13, 1.0, 4.5, 7.1 }, 4, BLAS.MatrixTransposeState.Transpose, 2, 2);
            }
        }

        /// <summary>A test function that compares the result of 'DGEMV' to the benchmark implementation.
        /// </summary>
        /// <param name="m">The number of rows of matrix A.</param>
        /// <param name="n">The number of columns of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The matrix A of dimension (<paramref name="lda"/>, <paramref name="n"/>) supplied column-by-column.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incX"/>| elements if 'op(A)=A'; 1 + (<paramref name="m"/>-1) * |<paramref name="incY"/>| elements otherwise.</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="y">The vector y with at least 1 + (<paramref name="m"/>-1) * |<paramref name="incY"/>| elements if 'op(A)=A'; 1 + (<paramref name="n"/>-1) * | <paramref name="incX"/>| otherwise (input/output).</param>
        /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="m"/>).</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
        [TestCaseSource(nameof(TestCaseData_dgemv))]
        public void dgemv_TestCaseData_ResultOfBenchmarkImplementation(int m, int n, double alpha, double[] a, double[] x, double beta, double[] y, int lda, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1, int incY = 1)
        {
            var actual = y.ToArray();
            m_TestObject.dgemv(m, n, alpha, a, x, beta, actual, lda, transpose, incX, incY);

            var expected = y.ToArray();
            m_BenchmarkObject.dgemv(m, n, alpha, a, x, beta, expected, lda, transpose, incX, incY);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'DGEMV'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_dgemv
        {
            get
            {
                // squared input matrix
                yield return new TestCaseData(4, 4, -1.25, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new double[] { 11, 12, 13, 14 }, -1.5, new double[] { -16, -15, -14, -13 }, 4, BLAS.MatrixTransposeState.NoTranspose, 1, 1);
                yield return new TestCaseData(4, 4, 3.7, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new double[] { 11, 12, 13, 14 }, 2.4, new double[] { -16, -15, -14, -13 }, 4, BLAS.MatrixTransposeState.Transpose, 1, 1);

                // non-squared input matrices
                yield return new TestCaseData(3, 4, -1.25, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new double[] { 11, 12, 13, 14 }, -5.76, new double[] { -16, -15, -14, -13 }, 3, BLAS.MatrixTransposeState.NoTranspose, 1, 1);
                yield return new TestCaseData(4, 3, 3.7, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new double[] { 11, 12, 13, 14 }, 3.54, new double[] { -16, -15, -14, -13 }, 4, BLAS.MatrixTransposeState.Transpose, 1, 1);

                yield return new TestCaseData(3, 4, -1.25, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, -5.76, new double[] { -16, -15, -14, -13 }, 3, BLAS.MatrixTransposeState.NoTranspose, 2, 1);
                yield return new TestCaseData(4, 3, 2.7, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new double[] { 11, 12, 13, 14, 15 }, 3.54, new double[] { -16, -15, -14, -13, -11, -10, -9 }, 4, BLAS.MatrixTransposeState.Transpose, 1, 2);
            }
        }

        /// <summary>A test function that compares the result of 'DGER' to the benchmark implementation.
        /// </summary>
        /// <param name="m">The number of rows of matrix A.</param>
        /// <param name="n">The number of columns of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="m"/>-1) * |<paramref name="incX"/>| elements.</param>
        /// <param name="y">The vector y with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incY"/>| elements.</param>
        /// <param name="a">The matrix A of dimension (<paramref name="lda"/>, <paramref name="n"/>) supplied column-by-column (input/output).</param>
        /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="m"/>).</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
        [TestCaseSource(nameof(TestCaseData_dger))]
        public void dger_TestCaseData_ResultOfBenchmarkImplementation(int m, int n, double alpha, double[] x, double[] y, double[] a, int lda, int incX = 1, int incY = 1)
        {
            var actual = a.ToArray();
            m_TestObject.dger(m, n, alpha, x, y, actual, lda, incX, incY);

            var expected = a.ToArray();
            m_BenchmarkObject.dger(m, n, alpha, x, y, expected, lda, incX, incY);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'DGER'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_dger
        {
            get
            {
                // squared input matrix
                yield return new TestCaseData(4, 4, -1.25, new double[] { 11, 12, 13, 14 }, new double[] { -16, -15, -14, -13 }, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, 4, 1, 1);

                // non-squared input matrices
                yield return new TestCaseData(3, 4, -1.25, new double[] { 11, 12, 13, 14 }, new double[] { -16, -15, -14, -13 }, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, 3, 1, 1);
                yield return new TestCaseData(4, 3, 3.7, new double[] { 11, 12, 13, 14 }, new double[] { -16, -15, -14, -13 }, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, 4, 1, 1);

                yield return new TestCaseData(3, 4, -1.25, new double[] { 11, 12, 13, 14, 15, 16, 17 }, new double[] { -16, -15, -14, -13 }, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, 3, 2, 1);
                yield return new TestCaseData(4, 3, 2.7, new double[] { 11, 12, 13, 14, 15 }, new double[] { -16, -15, -14, -13, -11, -10, -9 }, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, 4, 1, 2);
            }
        }

        /// <summary>A test function that compares the result of 'DSBMV' to the benchmark implementation.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="k">The number of super-diagonals of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The matrix A of dimension (<paramref name="lda"/>, <paramref name="n"/>) supplied column-by-column.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="y">The vector y with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incY"/> | elements (input/output).</param>
        /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least (1 + <paramref name="k"/>).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
        [TestCaseSource(nameof(TestCaseData_dsbmv))]
        public void dsbmv_TestCaseData_ResultOfBenchmarkImplementation(int n, int k, double alpha, double[] a, double[] x, double beta, double[] y, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1, int incY = 1)
        {
            var actual = y.ToArray();
            m_TestObject.dsbmv(n, k, alpha, a, x, beta, actual, lda, triangularMatrixType, incX, incY);

            var expected = y.ToArray();
            m_BenchmarkObject.dsbmv(n, k, alpha, a, x, beta, expected, lda, triangularMatrixType, incX, incY);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'DSBMV'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_dsbmv
        {
            get
            {
                yield return new TestCaseData(4, 2, -1.25, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new double[] { 11, 12, 13, 14 }, -1.5, new double[] { -16, -15, -14, -13 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, 1, 1);
                yield return new TestCaseData(4, 2, -1.25, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new double[] { 11, 12, 13, 14 }, -1.5, new double[] { -16, -15, -14, -13 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, 1, 1);

                yield return new TestCaseData(4, 2, -1.25, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new double[] { 11, 12, 13, 14 }, -1.5, new double[] { -16, -15, -14, -13, -12, -11, -10, -9 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, 1, 2);
                yield return new TestCaseData(4, 2, -1.25, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, -1.5, new double[] { -16, -15, -14, -13 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, 2, 1);
            }
        }

        /// <summary>A test function that compares the result of 'DSPMV' to the benchmark implementation.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="aPacked">The symmetric packed matrix A with dimension at least (<paramref name="n"/> * (<paramref name="n"/> + 1) ) / 2, i.e. in the upper triangular representation: aPacked[0] = a[0,0], aPacked[1] = a[0,1], aPacked[2] = a[1,1]; otherwise aPacked[0] = a[0,0], aPacked[1] = a[1,0], aPacked[2] = a[2,0] etc.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="y">The vector y with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incY"/> | elements (input/output).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
        [TestCaseSource(nameof(TestCaseData_dspmv))]
        public void dspmv_TestCaseData_ResultOfBenchmarkImplementation(int n, double alpha, double[] aPacked, double[] x, double beta, double[] y, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1, int incY = 1)
        {
            var actual = y.ToArray();
            m_TestObject.dspmv(n, alpha, aPacked, x, beta, actual, triangularMatrixType, incX, incY);

            var expected = y.ToArray();
            m_BenchmarkObject.dspmv(n, alpha, aPacked, x, beta, expected, triangularMatrixType, incX, incY);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'DSPMV'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_dspmv
        {
            get
            {
                yield return new TestCaseData(4, -1.25, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new double[] { 11, 12, 13, 14 }, -1.5, new double[] { -16, -15, -14, -13 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, 1, 1);
                yield return new TestCaseData(4, -1.25, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new double[] { 11, 12, 13, 14 }, -1.5, new double[] { -16, -15, -14, -13 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, 1, 1);

                yield return new TestCaseData(4, -1.25, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, -1.5, new double[] { -16, -15, -14, -13 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, 2, 1);
                yield return new TestCaseData(4, -1.25, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new double[] { 11, 12, 13, 14 }, -1.5, new double[] { -16, -15, -14, -13, -12, -11, -10 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, 1, 2);
            }
        }

        /// <summary>A test function that compares the result of 'DSPR' to the benchmark implementation.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
        /// <param name="aPacked">The symmetric packed matrix A with dimension at least (<paramref name="n"/> * (<paramref name="n"/> + 1) ) / 2.</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        [TestCaseSource(nameof(TestCaseData_dspr))]
        public void dspr_TestCaseData_ResultOfBenchmarkImplementation(int n, double alpha, double[] x, double[] aPacked, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1)
        {
            var actual = aPacked.ToArray();
            m_TestObject.dspr(n, alpha, x, actual, triangularMatrixType, incX);

            var expected = aPacked.ToArray();
            m_BenchmarkObject.dspr(n, alpha, x, expected, triangularMatrixType, incX);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'DSPR'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_dspr
        {
            get
            {
                yield return new TestCaseData(4, -1.25, new double[] { 11, 12, 13, 14 }, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, 1);
                yield return new TestCaseData(4, -1.25, new double[] { 11, 12, 13, 14 }, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, 1);

                yield return new TestCaseData(4, -1.25, new double[] { 11, 12, 13, 14, 15, 16, 17 }, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, 2);
            }
        }

        /// <summary>A test function that compares the result of 'DSPR2' to the benchmark implementation.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
        /// <param name="y">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incY"/> | elements.</param>
        /// <param name="aPacked">The symmetric packed matrix A with dimension at least (<paramref name="n"/> * (<paramref name="n"/> + 1) ) / 2.</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
        [TestCaseSource(nameof(TestCaseData_dspr2))]
        public void dspr2_TestCaseData_ResultOfBenchmarkImplementation(int n, double alpha, double[] x, double[] y, double[] aPacked, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1, int incY = 1)
        {
            var actual = aPacked.ToArray();
            m_TestObject.dspr2(n, alpha, x, y, actual, triangularMatrixType, incX, incY);

            var expected = aPacked.ToArray();
            m_BenchmarkObject.dspr2(n, alpha, x, y, expected, triangularMatrixType, incX, incY);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'DSPR2'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_dspr2
        {
            get
            {
                yield return new TestCaseData(4, -1.25, new double[] { 11, 12, 13, 14 }, new double[] { -16, -15, -14, -13 }, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, 1, 1);
                yield return new TestCaseData(4, -1.25, new double[] { 11, 12, 13, 14 }, new double[] { -16, -15, -14, -13 }, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, 1, 1);

                yield return new TestCaseData(4, -1.25, new double[] { 11, 12, 13, 14, 15, 16, 17 }, new double[] { -16, -15, -14, -13 }, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, 2, 1);
                yield return new TestCaseData(4, -1.25, new double[] { 11, 12, 13, 14 }, new double[] { -16, -15, -14, -13, -12, -11, -10 }, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, 1, 2);
            }
        }

        /// <summary>A test function that compares the result of 'DSYMV' to the benchmark implementation.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The symmetric matrix A of dimension (<paramref name="lda"/>, <paramref name="n"/>) supplied column-by-column.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="y">The vector y with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incY"/> | elements (input/output).</param>
        /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1, <paramref name="n"/>).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
        [TestCaseSource(nameof(TestCaseData_dsymv))]
        public void dsymv_TestCaseData_ResultOfBenchmarkImplementation(int n, double alpha, double[] a, double[] x, double beta, double[] y, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1, int incY = 1)
        {
            var actual = y.ToArray();
            m_TestObject.dsymv(n, alpha, a, x, beta, actual, lda, triangularMatrixType, incX, incY);

            var expected = y.ToArray();
            m_BenchmarkObject.dsymv(n, alpha, a, x, beta, expected, lda, triangularMatrixType, incX, incY);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'DSYMV'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_dsymv
        {
            get
            {
                yield return new TestCaseData(4, -1.25, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14 }, -1.5, new double[] { -16, -15, -14, -13 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, 1, 1);
                yield return new TestCaseData(4, -1.25, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14 }, -1.5, new double[] { -16, -15, -14, -13 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, 1, 1);

                yield return new TestCaseData(4, -1.25, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14 }, -1.5, new double[] { -16, -15, -14, -13, -12, -11, -10 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, 1, 2);
                yield return new TestCaseData(4, -1.25, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, -1.5, new double[] { -16, -15, -14, -13 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, 2, 1);
                yield return new TestCaseData(4, -1.25, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14 }, -1.5, new double[] { -16, -15, -14, -13, -12, -11, -10 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, 1, 2);
                yield return new TestCaseData(4, -1.25, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, -1.5, new double[] { -16, -15, -14, -13 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, 2, 1);
            }
        }

        /// <summary>A test function that compares the result of 'DSYR' to the benchmark implementation.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
        /// <param name="a">The symmetric matrix A of dimension (<paramref name="lda"/>, <paramref name="n"/>) supplied column-by-column (input/output).</param>
        /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1, <paramref name="n"/>).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        [TestCaseSource(nameof(TestCaseData_dsyr))]
        public void dsyr_TestCaseData_ResultOfBenchmarkImplementation(int n, double alpha, double[] x, double[] a, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1)
        {
            var actual = a.ToArray();
            m_TestObject.dsyr(n, alpha, x, actual, triangularMatrixType, incX);

            var expected = a.ToArray();
            m_BenchmarkObject.dsyr(n, alpha, x, expected, triangularMatrixType, incX);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'DSYR'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_dsyr
        {
            get
            {
                yield return new TestCaseData(4, -1.25, new double[] { 11, 12, 13, 14 }, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, 1);
                yield return new TestCaseData(4, -1.25, new double[] { 11, 12, 13, 14 }, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, 1);

                yield return new TestCaseData(4, -1.25, new double[] { 11, 12, 13, 14, 15, 16, 17 }, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, 2);
                yield return new TestCaseData(4, -1.25, new double[] { 11, 12, 13, 14, 15, 16, 17 }, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, 2);
            }
        }

        /// <summary>A test function that compares the result of 'DSYR2' to the benchmark implementation.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
        /// <param name="y">The vector y with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incY"/> | elements.</param>
        /// <param name="a">The symmetric matrix A of dimension (<paramref name="lda"/>, <paramref name="n"/>) supplied column-by-column (input/output).</param>
        /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1, <paramref name="n"/>).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
        [TestCaseSource(nameof(TestCaseData_dsyr2))]
        public void dsyr2_TestCaseData_ResultOfBenchmarkImplementation(int n, double alpha, double[] x, double[] y, double[] a, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1, int incY = 1)
        {
            var actual = a.ToArray();
            m_TestObject.dsyr2(n, alpha, x, y, actual, lda, triangularMatrixType, incX, incY);

            var expected = a.ToArray();
            m_BenchmarkObject.dsyr2(n, alpha, x, y, expected, lda, triangularMatrixType, incX, incY);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'DSYR2'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_dsyr2
        {
            get
            {
                yield return new TestCaseData(4, -1.25, new double[] { 11, 12, 13, 14 }, new double[] { -16, -15, -14, -13 }, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, 1, 1);
                yield return new TestCaseData(4, -1.25, new double[] { 11, 12, 13, 14 }, new double[] { -16, -15, -14, -13 }, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, 1, 1);

                yield return new TestCaseData(4, -1.25, new double[] { 11, 12, 13, 14, 15, 16, 17 }, new double[] { -16, -15, -14, -13 }, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, 2, 1);
                yield return new TestCaseData(4, -1.25, new double[] { 11, 12, 13, 14 }, new double[] { -16, -15, -14, -13, -12, -11, -10 }, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, 1, 2);
            }
        }

        /// <summary>A test function that compares the result of 'DTBMV' to the benchmark implementation.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="k">The number of super-diagonales of A if the matrix A is provided in its upper triangular representation; the number of sub-diagonals otherwise.</param>
        /// <param name="a">The triangular band matrix with dimension (<paramref name="lda"/>, <paramref name="n"/>).</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
        /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least (1  + <paramref name="k"/>).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        [TestCaseSource(nameof(TestCaseData_dtbmv))]
        public void dtbmv_TestCaseData_ResultOfBenchmarkImplementation(int n, int k, double[] a, double[] x, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1)
        {
            var actual = x.ToArray();
            m_TestObject.dtbmv(n, k, a, actual, lda, triangularMatrixType, isUnitTriangular, transpose, incX);

            var expected = x.ToArray();
            m_BenchmarkObject.dtbmv(n, k, a, expected, lda, triangularMatrixType, isUnitTriangular, transpose, incX);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'DTBMV'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_dtbmv
        {
            get
            {
                yield return new TestCaseData(4, 2, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new double[] { 11, 12, 13, 14 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, 2, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new double[] { 11, 12, 13, 14 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, 2, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new double[] { 11, 12, 13, 14 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, 2, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new double[] { 11, 12, 13, 14 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, 2, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new double[] { 11, 12, 13, 14 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, 2, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new double[] { 11, 12, 13, 14 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, 2, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new double[] { 11, 12, 13, 14 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, 2, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new double[] { 11, 12, 13, 14 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 1);


                yield return new TestCaseData(4, 2, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, 2, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, 2, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, 2, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, 2, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, 2, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, 2, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, 2, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 2);
            }
        }

        /// <summary>A test function that compares the result of 'DTBSV' to the benchmark implementation.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="k">The number of super-diagonales of A if the matrix A is provided in its upper triangular representation; the number of sub-diagonals otherwise.</param>
        /// <param name="a">The triangular band matrix with dimension (<paramref name="lda"/>, <paramref name="n"/>).</param>
        /// <param name="x">The vector b (input), x (output) with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements (input/output).</param>
        /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least (1  + <paramref name="k"/>).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        [TestCaseSource(nameof(TestCaseData_dtbsv))]
        public void dtbsv_TestCaseData_ResultOfBenchmarkImplementation(int n, int k, double[] a, double[] x, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1)
        {
            var actual = x.ToArray();
            m_TestObject.dtbsv(n, k, a, actual, lda, triangularMatrixType, isUnitTriangular, transpose, incX);

            var expected = x.ToArray();
            m_BenchmarkObject.dtbsv(n, k, a, expected, lda, triangularMatrixType, isUnitTriangular, transpose, incX);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'DTBSV'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_dtbsv
        {
            get
            {
                yield return new TestCaseData(4, 2, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new double[] { 11, 12, 13, 14 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, 2, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new double[] { 11, 12, 13, 14 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, 2, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new double[] { 11, 12, 13, 14 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, 2, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new double[] { 11, 12, 13, 14 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, 2, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new double[] { 11, 12, 13, 14 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, 2, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new double[] { 11, 12, 13, 14 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, 2, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new double[] { 11, 12, 13, 14 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, 2, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new double[] { 11, 12, 13, 14 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 1);


                yield return new TestCaseData(4, 2, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, 2, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, 2, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, 2, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, 2, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, 2, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, 2, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, 2, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 2);
            }
        }

        /// <summary>A test function that compares the result of 'DTPMV' to the benchmark implementation.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="aPacked">The triangular packed matrix A with dimension at least (<paramref name="n"/> * (<paramref name="n"/> + 1) ) / 2.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        [TestCaseSource(nameof(TestCaseData_dtpmv))]
        public void dtpmv_TestCaseData_ResultOfBenchmarkImplementation(int n, double[] aPacked, double[] x, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1)
        {
            var actual = x.ToArray();
            m_TestObject.dtpmv(n, aPacked, actual, triangularMatrixType, isUnitTriangular, transpose, incX);

            var expected = x.ToArray();
            m_BenchmarkObject.dtpmv(n, aPacked, expected, triangularMatrixType, isUnitTriangular, transpose, incX);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'DTPMV'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_dtpmv
        {
            get
            {
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new double[] { 11, 12, 13, 14 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new double[] { 11, 12, 13, 14 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new double[] { 11, 12, 13, 14 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new double[] { 11, 12, 13, 14 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new double[] { 11, 12, 13, 14 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new double[] { 11, 12, 13, 14 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new double[] { 11, 12, 13, 14 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new double[] { 11, 12, 13, 14 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 1);


                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 2);
            }
        }

        /// <summary>A test function that compares the result of 'DTPSV' to the benchmark implementation.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="aPacked">The triangular packed matrix A with dimension at least (<paramref name="n"/> * (<paramref name="n"/> + 1) ) / 2.</param>
        /// <param name="x">The vector b (input), x (output) with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements (input/output).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        [TestCaseSource(nameof(TestCaseData_dtpsv))]
        public void dtpsv_TestCaseData_ResultOfBenchmarkImplementation(int n, double[] aPacked, double[] x, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1)
        {
            var actual = x.ToArray();
            m_TestObject.dtpsv(n, aPacked, actual, triangularMatrixType, isUnitTriangular, transpose, incX);

            var expected = x.ToArray();
            m_BenchmarkObject.dtpsv(n, aPacked, expected, triangularMatrixType, isUnitTriangular, transpose, incX);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'DTPSV'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_dtpsv
        {
            get
            {
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new double[] { 11, 12, 13, 14 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new double[] { 11, 12, 13, 14 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new double[] { 11, 12, 13, 14 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new double[] { 11, 12, 13, 14 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new double[] { 11, 12, 13, 14 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new double[] { 11, 12, 13, 14 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new double[] { 11, 12, 13, 14 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new double[] { 11, 12, 13, 14 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 1);


                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 2);
            }
        }

        /// <summary>A test function that compares the result of 'DTRMV' to the benchmark implementation.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="a">The triangular matrix A with dimension (<paramref name="lda"/>, <paramref name="n"/>).</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
        /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1, <paramref name="n"/>).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        [TestCaseSource(nameof(TestCaseData_dtrmv))]
        public void dtrmv_TestCaseData_ResultOfBenchmarkImplementation(int n, double[] a, double[] x, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1)
        {
            var actual = x.ToArray();
            m_TestObject.dtrmv(n, a, actual, lda, triangularMatrixType, isUnitTriangular, transpose, incX);

            var expected = x.ToArray();
            m_BenchmarkObject.dtrmv(n, a, expected, lda, triangularMatrixType, isUnitTriangular, transpose, incX);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'DTRMV'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_dtrmv
        {
            get
            {
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 1);


                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 2);
            }
        }

        /// <summary>A test function that compares the result of 'DTRSV' to the benchmark implementation.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="a">The triangular matrix A with dimension (<paramref name="lda"/>, <paramref name="n"/>).</param>
        /// <param name="x">The vector b (input), x (output) with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements (input/output).</param>
        /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1, <paramref name="n"/>).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
        /// <param name="transpose">A value indicating whether op(A) = A or op(A) = A^t.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        [TestCaseSource(nameof(TestCaseData_dtrsv))]
        public void dtrsv_TestCaseData_ResultOfBenchmarkImplementation(int n, double[] a, double[] x, int lda, BLAS.TriangularMatrixType triangularMatrixType, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1)
        {
            var actual = x.ToArray();
            m_TestObject.dtrsv(n, a, actual, lda, triangularMatrixType, isUnitTriangular, transpose, incX);

            var expected = x.ToArray();
            m_BenchmarkObject.dtrsv(n, a, expected, lda, triangularMatrixType, isUnitTriangular, transpose, incX);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'DTRSV'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_dtrsv
        {
            get
            {
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 1);


                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 2);
            }
        }
        #endregion

        #region complex methods

        /// <summary>A test function that compares the result of 'ZGBMV' to the benchmark implementation.
        /// </summary>
        /// <param name="m">The number of rows of matrix A.</param>
        /// <param name="n">The number of columns of matrix A.</param>
        /// <param name="kl">The number of sub-diagonals of matrix A.</param>
        /// <param name="ku">The number of super-diagonals of matrix A.</param>
        /// <param name="alpha">The scalar factor \alpha.</param>
        /// <param name="a">The general band matrix A of dimension (<paramref name="lda"/>, <paramref name="n"/>). The leading (<paramref name="kl"/> + <paramref name="ku"/> + 1) by <paramref name="n"/> part 
        /// must contain the matrix of coefficients (supplied column-by-column) with the leading diagonal of the matrix 
        /// in row (<paramref name="ku"/> + 1) of the array, the first super-diagonal starting at position 2 in row ku, 
        /// the first sub-diagonal starting at position 1 in row (ku + 2), etc. 
        /// The following program segment transfers a band matrix from conventional full matrix storage to band storage:
        /// <code>
        /// for j = 0 to n-1
        ///   k = ku - j
        ///   for i = max(0, j-ku) to min(m-1, j+kl-1) 
        ///     a(k+i, j) = matrix(i,j)
        ///   end
        /// end
        /// </code>
        /// </param>
        /// <param name="x">The vector x with at least 1+(<paramref name="n"/>-1) * |<paramref name="incX"/>| elements if 'op(A)=A'; 1+(<paramref name="m"/>-1) *|<paramref name="incX"/>| otherwise.</param>
        /// <param name="beta">The scalar factor \beta.</param>
        /// <param name="y">The vector y with at least 1+(<paramref name="m"/>-1) *|<paramref name="incY"/>| elements if 'op(A)=A'; 1+(<paramref name="n"/>-1) * |<paramref name="incY"/>| otherwise (input/output).</param>
        /// <param name="lda">The leading dimension, must be at least <paramref name="kl"/> + <paramref name="ku"/> +1.</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
        [TestCaseSource(nameof(TestCaseData_zgbmv))]
        public void zgbmv_TestCaseData_ResultOfBenchmarkImplementation(int m, int n, int kl, int ku, Complex alpha, Complex[] a, Complex[] x, Complex beta, Complex[] y, int lda, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1, int incY = 1)
        {
            var actual = y.ToArray();
            m_TestObject.zgbmv(m, n, kl, ku, alpha, a, x, beta, actual, lda, transpose, incX, incY);

            var expected = y.ToArray();
            m_BenchmarkObject.zgbmv(m, n, kl, ku, alpha, a, x, beta, expected, lda, transpose, incX, incY);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'ZGBMV'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_zgbmv
        {
            get
            {
                // squared input matrix
                yield return new TestCaseData(4, 4, 2, 2, -1.25 + 0.5 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6 + 0.5 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new Complex[] { 11, 12, 13, 14 }, -1.5 + 0.5 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13 }, 5, BLAS.MatrixTransposeState.NoTranspose, 1, 1);
                yield return new TestCaseData(4, 4, 2, 2, 3.7 + 0.5 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 + 0.5 * Complex.ImaginaryOne, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new Complex[] { 11, 12, 13, 14 }, 2.4 + 0.5 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13 }, 5, BLAS.MatrixTransposeState.Transpose, 1, 1);
                yield return new TestCaseData(4, 4, 2, 2, 3.7 + 0.5 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 + 0.5 * Complex.ImaginaryOne, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new Complex[] { 11, 12, 13, 14 }, 2.4 + 0.5 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13 }, 5, BLAS.MatrixTransposeState.Hermite, 1, 1);

                // non-squared input matrices
                yield return new TestCaseData(3, 4, 2, 2, -1.25 + 0.5 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 0.5 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new Complex[] { 11, 12, 13, 14 }, -5.76 + 0.5 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13 }, 5, BLAS.MatrixTransposeState.NoTranspose, 1, 1);
                yield return new TestCaseData(4, 3, 2, 2, 3.7 + 0.5 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 + 0.5 * Complex.ImaginaryOne, 10 + 0.5 * Complex.ImaginaryOne, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new Complex[] { 11, 12, 13, 14 }, 3.54 + 0.5 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13 }, 5, BLAS.MatrixTransposeState.Transpose, 1, 1);
                yield return new TestCaseData(4, 3, 2, 2, 3.7 + 0.5 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 + 0.5 * Complex.ImaginaryOne, 10 + 0.5 * Complex.ImaginaryOne, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new Complex[] { 11, 12, 13, 14 }, 3.54 + 0.5 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13 }, 5, BLAS.MatrixTransposeState.Hermite, 1, 1);
                yield return new TestCaseData(3, 4, 1, 2, -1.25 + 0.5 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 0.5 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new Complex[] { 11, 12, 13, 14 }, -2.5 + 0.5 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13 }, 4, BLAS.MatrixTransposeState.NoTranspose, 1, 1);
                yield return new TestCaseData(4, 3, 2, 1, 3.7 + 0.5 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 0.5 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new Complex[] { 11, 12, 13, 14 }, 1.5 + 0.5 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13 }, 4, BLAS.MatrixTransposeState.Transpose, 1, 1);
                yield return new TestCaseData(4, 3, 2, 1, 3.7 + 0.5 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 0.5 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new Complex[] { 11, 12, 13, 14 }, 1.5 + 0.5 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13 }, 4, BLAS.MatrixTransposeState.Hermite, 1, 1);

                yield return new TestCaseData(3, 4, 1, 2, -1.25 + 0.5 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 0.5 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new Complex[] { 11, 12, 13, 14, 5, 7, 8 }, -1.87 + 0.5 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13 }, 4, BLAS.MatrixTransposeState.NoTranspose, 2, 1);
                yield return new TestCaseData(4, 3, 2, 1, 0.7 + 0.5 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8 + 0.5 * Complex.ImaginaryOne, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new Complex[] { 11, 12, 13, 14, 9.5 }, 1.25 + 0.5 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, 1.0, 4.5, 7.1 }, 4, BLAS.MatrixTransposeState.NoTranspose, 2, 2);
                yield return new TestCaseData(4, 3, 2, 1, -1.2 + 0.5 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6 + 0.5 * Complex.ImaginaryOne, 7, 8 + 0.5 * Complex.ImaginaryOne, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new Complex[] { 11, 12, 13, 14, 9.5, 6.4, 1.2 }, 1.25 + 0.5 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, 1.0, 4.5, 7.1 }, 4, BLAS.MatrixTransposeState.Transpose, 2, 2);
                yield return new TestCaseData(4, 3, 2, 1, -1.2 + 0.5 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6 + 0.5 * Complex.ImaginaryOne, 7, 8 + 0.5 * Complex.ImaginaryOne, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new Complex[] { 11, 12, 13, 14, 9.5, 6.4, 1.2 }, 1.25 + 0.5 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, 1.0, 4.5, 7.1 }, 4, BLAS.MatrixTransposeState.Hermite, 2, 2);
            }
        }

        /// <summary>A test function that compares the result of 'ZGEMV' to the benchmark implementation.
        /// </summary>
        /// <param name="m">The number of rows of matrix A.</param>
        /// <param name="n">The number of columns of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The matrix A of dimension (<paramref name="lda"/>, <paramref name="n"/>) supplied column-by-column.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incX"/>| elements if 'op(A)=A'; 1 + (<paramref name="m"/>-1) * |<paramref name="incY"/>| elements otherwise.</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="y">The vector y with at least 1 + (<paramref name="m"/>-1) * |<paramref name="incY"/>| elements if 'op(A)=A'; 1 + (<paramref name="n"/>-1) * | <paramref name="incX"/>| otherwise (input/output).</param>
        /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="m"/>).</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
        [TestCaseSource(nameof(TestCaseData_zgemv))]
        public void zgemv_TestCaseData_ResultOfBenchmarkImplementation(int m, int n, Complex alpha, Complex[] a, Complex[] x, Complex beta, Complex[] y, int lda, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1, int incY = 1)
        {
            var actual = y.ToArray();
            m_TestObject.zgemv(m, n, alpha, a, x, beta, actual, lda, transpose, incX, incY);

            var expected = y.ToArray();
            m_BenchmarkObject.zgemv(m, n, alpha, a, x, beta, expected, lda, transpose, incX, incY);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'ZGEMV'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_zgemv
        {
            get
            {
                // squared input matrix
                yield return new TestCaseData(4, 4, -1.25 - 0.76 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4 - 0.76 * Complex.ImaginaryOne, 5 - 0.76 * Complex.ImaginaryOne, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new Complex[] { 11, 12 - 0.76 * Complex.ImaginaryOne, 13, 14 }, -1.5 - 0.76 * Complex.ImaginaryOne, new Complex[] { -16, -15 - 0.76 * Complex.ImaginaryOne, -14, -13 }, 4, BLAS.MatrixTransposeState.NoTranspose, 1, 1);
                yield return new TestCaseData(4, 4, 3.7 - 0.76 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6 - 0.76 * Complex.ImaginaryOne, 7, 8 - 0.76 * Complex.ImaginaryOne, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new Complex[] { 11, 12 - 0.76 * Complex.ImaginaryOne, 13, 14 }, 2.4 - 0.76 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14 - 0.76 * Complex.ImaginaryOne, -13 }, 4, BLAS.MatrixTransposeState.Transpose, 1, 1);
                yield return new TestCaseData(4, 4, 3.7 - 0.76 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6 - 0.76 * Complex.ImaginaryOne, 7, 8 - 0.76 * Complex.ImaginaryOne, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new Complex[] { 11, 12 - 0.76 * Complex.ImaginaryOne, 13, 14 }, 2.4 - 0.76 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14 - 0.76 * Complex.ImaginaryOne, -13 }, 4, BLAS.MatrixTransposeState.Hermite, 1, 1);


                // non-squared input matrices
                yield return new TestCaseData(3, 4, -1.25 - 0.76 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5 - 0.76 * Complex.ImaginaryOne, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new Complex[] { 11, 12, 13 - 0.76 * Complex.ImaginaryOne, 14 }, -5.76 - 0.76 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14 - 0.76 * Complex.ImaginaryOne, -13 }, 3, BLAS.MatrixTransposeState.NoTranspose, 1, 1);
                yield return new TestCaseData(4, 3, 3.7 - 0.76 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 - 0.76 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new Complex[] { 11, 12, 13 - 0.76 * Complex.ImaginaryOne, 14 }, 3.54 - 0.76 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13 - 0.76 * Complex.ImaginaryOne }, 4, BLAS.MatrixTransposeState.Transpose, 1, 1);
                yield return new TestCaseData(4, 3, 3.7 - 0.76 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 - 0.76 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new Complex[] { 11, 12, 13 - 0.76 * Complex.ImaginaryOne, 14 }, 3.54 - 0.76 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13 - 0.76 * Complex.ImaginaryOne }, 4, BLAS.MatrixTransposeState.Hermite, 1, 1);


                yield return new TestCaseData(3, 4, -1.25 - 0.76 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 - 0.76 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new Complex[] { 11, 12, 13 - 0.76 * Complex.ImaginaryOne, 14, 15, 16, 17 }, -5.76 - 0.76 * Complex.ImaginaryOne, new Complex[] { -16, -15 - 0.76 * Complex.ImaginaryOne, -14, -13 }, 3, BLAS.MatrixTransposeState.NoTranspose, 2, 1);
                yield return new TestCaseData(4, 3, 2.7 - 0.76 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, -0.76 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new Complex[] { 11, 12, 13, 14 - 0.76 * Complex.ImaginaryOne, 15 }, 3.54 - 0.76 * Complex.ImaginaryOne, new Complex[] { -16, -15 - 0.76 * Complex.ImaginaryOne, -14, -13 - 0.76 * Complex.ImaginaryOne, -11, -10, -9 }, 4, BLAS.MatrixTransposeState.Transpose, 1, 2);
                yield return new TestCaseData(4, 3, 2.7 - 0.76 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, -0.76 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new Complex[] { 11, 12, 13, 14 - 0.76 * Complex.ImaginaryOne, 15 }, 3.54 - 0.76 * Complex.ImaginaryOne, new Complex[] { -16, -15 - 0.76 * Complex.ImaginaryOne, -14, -13 - 0.76 * Complex.ImaginaryOne, -11, -10, -9 }, 4, BLAS.MatrixTransposeState.Hermite, 1, 2);
            }
        }

        /// <summary>A test function that compares the result of 'ZGERC' to the benchmark implementation.
        /// </summary>
        /// <param name="m">The number of rows of matrix A.</param>
        /// <param name="n">The number of columns of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="m"/>-1) * |<paramref name="incX"/>| elements.</param>
        /// <param name="y">The vector y with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incY"/>| elements.</param>
        /// <param name="a">The matrix A of dimension (<paramref name="lda"/>, <paramref name="n"/>) supplied column-by-column.</param>
        /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="m"/>).</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
        [TestCaseSource(nameof(TestCaseData_zger))]
        public void zgerc_TestCaseData_ResultOfBenchmarkImplementation(int m, int n, Complex alpha, Complex[] x, Complex[] y, Complex[] a, int lda, int incX = 1, int incY = 1)
        {
            var actual = a.ToArray();
            m_TestObject.zgerc(m, n, alpha, x, y, actual, lda, incX, incY);

            var expected = a.ToArray();
            m_BenchmarkObject.zgerc(m, n, alpha, x, y, expected, lda, incX, incY);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>A test function that compares the result of 'ZGERU' to the benchmark implementation.
        /// </summary>
        /// <param name="m">The number of rows of matrix A.</param>
        /// <param name="n">The number of columns of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="m"/>-1) * |<paramref name="incX"/>| elements.</param>
        /// <param name="y">The vector y with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incY"/>| elements.</param>
        /// <param name="a">The matrix A of dimension (<paramref name="lda"/>, <paramref name="n"/>) supplied column-by-column.</param>
        /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="m"/>).</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
        [TestCaseSource(nameof(TestCaseData_zger))]
        public void zgeru_TestCaseData_ResultOfBenchmarkImplementation(int m, int n, Complex alpha, Complex[] x, Complex[] y, Complex[] a, int lda, int incX = 1, int incY = 1)
        {
            var actual = a.ToArray();
            m_TestObject.zgeru(m, n, alpha, x, y, actual, lda, incX, incY);

            var expected = a.ToArray();
            m_BenchmarkObject.zgeru(m, n, alpha, x, y, expected, lda, incX, incY);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'ZGERC' and 'ZGERU'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_zger
        {
            get
            {
                // squared input matrix
                yield return new TestCaseData(4, 4, -1.25 + 0.8 * Complex.ImaginaryOne, new Complex[] { 11, 12 + 0.8 * Complex.ImaginaryOne, 13, 14 }, new Complex[] { -16 + 0.8 * Complex.ImaginaryOne, -15, -14, -13 }, new Complex[] { 1, 2, 3 + 0.8 * Complex.ImaginaryOne, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, 4, 1, 1);

                // non-squared input matrices
                yield return new TestCaseData(3, 4, -1.25 + 0.8 * Complex.ImaginaryOne, new Complex[] { 11, 12, 13 + 0.8 * Complex.ImaginaryOne, 14 }, new Complex[] { -16, -15 + 0.8 * Complex.ImaginaryOne, -14, -13 }, new Complex[] { 1, 2, 3, 4, 5 + 0.8 * Complex.ImaginaryOne, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, 3, 1, 1);
                yield return new TestCaseData(4, 3, 3.7 + 0.8 * Complex.ImaginaryOne, new Complex[] { 11 + 0.8 * Complex.ImaginaryOne, 12, 13 + 0.8 * Complex.ImaginaryOne, 14 }, new Complex[] { -16, -15, -14 + 0.8 * Complex.ImaginaryOne, -13 }, new Complex[] { 1, 2, 3, 4 + 0.8 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, 4, 1, 1);

                yield return new TestCaseData(3, 4, -1.25 + 0.8 * Complex.ImaginaryOne, new Complex[] { 11, 12 + 0.8 * Complex.ImaginaryOne, 13, 14, 15, 16, 17 }, new Complex[] { -16, -15, -14 + 0.8 * Complex.ImaginaryOne, -13 }, new Complex[] { 1, 2 + 0.8 * Complex.ImaginaryOne, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, 3, 2, 1);
                yield return new TestCaseData(4, 3, 2.7 + 0.8 * Complex.ImaginaryOne, new Complex[] { 11, 12, 13 + 0.8 * Complex.ImaginaryOne, 14, 15 }, new Complex[] { -16, -15 + 0.8 * Complex.ImaginaryOne, -14, -13, -11, -10, -9 }, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8 + 0.8 * Complex.ImaginaryOne, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, 4, 1, 2);
            }
        }

        /// <summary>A test function that compares the result of 'ZHBMV' to the benchmark implementation.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="k">The number of super-diagonals.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The matrix Hermitian band matrix A with dimension (<paramref name="lda"/>, <paramref name="n"/>).</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incX"/>| elements.</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="y">The vector y with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incY"/>| elements (input/output).</param>
        /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least (1 + <paramref name="k"/>).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
        [TestCaseSource(nameof(TestCaseData_zhbmv))]
        public void zhbmv_TestCaseData_ResultOfBenchmarkImplementation(int n, int k, Complex alpha, Complex[] a, Complex[] x, Complex beta, Complex[] y, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1, int incY = 1)
        {
            var actual = y.ToArray();
            m_TestObject.zhbmv(n, k, alpha, a, x, beta, actual, lda, triangularMatrixType, incX, incY);

            var expected = y.ToArray();
            m_BenchmarkObject.zhbmv(n, k, alpha, a, x, beta, expected, lda, triangularMatrixType, incX, incY);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'ZHBMV'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_zhbmv
        {
            get
            {
                yield return new TestCaseData(4, 2, -1.25 - 2.31 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3 - 2.31 * Complex.ImaginaryOne, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new Complex[] { 11, 12 - 2.31 * Complex.ImaginaryOne, 13, 14 }, -1.5 + 2.31 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14 - 2.31 * Complex.ImaginaryOne, -13 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, 1, 1);
                yield return new TestCaseData(4, 2, -1.25 - 2.31 * Complex.ImaginaryOne, new Complex[] { 1, 2 - 2.31 * Complex.ImaginaryOne, 3, 4 - 2.31 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new Complex[] { 11 - 2.31 * Complex.ImaginaryOne, 12, 13, 14 }, -1.5 + 2.31 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14 - 2.31 * Complex.ImaginaryOne, -13 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, 1, 1);

                yield return new TestCaseData(4, 2, -1.25 - 2.31 * Complex.ImaginaryOne, new Complex[] { 1, 2 - 2.31 * Complex.ImaginaryOne, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new Complex[] { 11, 12, 13 - 2.31 * Complex.ImaginaryOne, 14 }, -1.5 + 2.31 * Complex.ImaginaryOne, new Complex[] { -16 - 2.31 * Complex.ImaginaryOne, -15, -14 - 2.31 * Complex.ImaginaryOne, -13 - 2.31 * Complex.ImaginaryOne, -12, -11, -10, -9 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, 1, 2);
                yield return new TestCaseData(4, 2, -1.25 - 2.31 * Complex.ImaginaryOne, new Complex[] { 1, 2 - 2.31 * Complex.ImaginaryOne, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new Complex[] { 11, 12, 13 - 2.31 * Complex.ImaginaryOne, 14 }, -1.5 + 2.31 * Complex.ImaginaryOne, new Complex[] { -16 - 2.31 * Complex.ImaginaryOne, -15, -14 - 2.31 * Complex.ImaginaryOne, -13 - 2.31 * Complex.ImaginaryOne, -12, -11, -10, -9 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, 1, 2);

                yield return new TestCaseData(4, 2, -1.25 - 2.31 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3 - 2.31 * Complex.ImaginaryOne, 4, 5 - 2.31 * Complex.ImaginaryOne, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new Complex[] { 11 - 2.31 * Complex.ImaginaryOne, 12 - 2.31 * Complex.ImaginaryOne, 13, 14, 15, 16, 17 }, -1.5 + 2.31 * Complex.ImaginaryOne, new Complex[] { -16, -15 - 2.31 * Complex.ImaginaryOne, -14, -13 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, 2, 1);
                yield return new TestCaseData(4, 2, -1.25 - 2.31 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3 - 2.31 * Complex.ImaginaryOne, 4, 5 - 2.31 * Complex.ImaginaryOne, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new Complex[] { 11 - 2.31 * Complex.ImaginaryOne, 12 - 2.31 * Complex.ImaginaryOne, 13, 14, 15, 16, 17 }, -1.5 + 2.31 * Complex.ImaginaryOne, new Complex[] { -16, -15 - 2.31 * Complex.ImaginaryOne, -14, -13 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, 2, 1);
            }
        }

        /// <summary>A test function that compares the result of 'ZHEMV' to the benchmark implementation.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incX"/>| elements.</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="y">The vector y with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incY"/>| elements (input/output).</param>
        /// <param name="a">The Hermitian matrix A with dimension (<paramref name="lda"/>, <paramref name="n"/>).</param>
        /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1, <paramref name="n"/>).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
        public void zhemv_TestCaseData_ResultOfBenchmarkImplementation(int n, Complex alpha, Complex[] x, Complex beta, Complex[] y, Complex[] a, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1, int incY = 1)
        {
            var actual = y.ToArray();
            m_TestObject.zhemv(n, alpha, x, beta, actual, a, lda, triangularMatrixType, incX, incY);

            var expected = y.ToArray();
            m_BenchmarkObject.zhemv(n, alpha, x, beta, expected, a, lda, triangularMatrixType, incX, incY);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>A test function that compares the result of 'ZHER' to the benchmark implementation.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incX"/>| elements.</param>
        /// <param name="a">The Hermitian matrix A with dimension (<paramref name="lda"/>, <paramref name="n"/>).</param>
        /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1, <paramref name="n"/>).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        [TestCaseSource(nameof(TestCaseData_zher))]
        public void zher_TestCaseData_ResultOfBenchmarkImplementation(int n, double alpha, Complex[] x, Complex[] a, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1)
        {
            var actual = a.ToArray();
            m_TestObject.zher(n, alpha, x, actual, triangularMatrixType, incX);

            var expected = a.ToArray();
            m_BenchmarkObject.zher(n, alpha, x, expected, triangularMatrixType, incX);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'ZHER'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_zher
        {
            get
            {
                yield return new TestCaseData(4, -1.25, new Complex[] { 11, 12, 13 + 2.34 * Complex.ImaginaryOne, 14 }, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 + 2.34 * Complex.ImaginaryOne, 11, 12, 13, 14, 15, 16 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, 1);
                yield return new TestCaseData(4, -1.25, new Complex[] { 11, 12 + 2.34 * Complex.ImaginaryOne, 13, 14 }, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 + 2.34 * Complex.ImaginaryOne, 10, 11, 12, 13, 14, 15, 16 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, 1);

                yield return new TestCaseData(4, -1.25, new Complex[] { 11, 12, 13 + 2.34 * Complex.ImaginaryOne, 14, 15, 16, 17 }, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.34 * Complex.ImaginaryOne, 8 + 2.34 * Complex.ImaginaryOne, 9, 10, 11 + 2.34 * Complex.ImaginaryOne, 12, 13, 14, 15, 16 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, 2);
                yield return new TestCaseData(4, -1.25, new Complex[] { 11, 12, 13, 14 + 2.34 * Complex.ImaginaryOne, 15, 16, 17 }, new Complex[] { 1, 2, 3 + 2.34 * Complex.ImaginaryOne, 4 + 2.34 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, 2);
            }
        }

        /// <summary>A test function that compares the result of 'ZHYR2' to the benchmark implementation.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incX"/>| elements.</param>
        /// <param name="y">The vector y with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incY"/>| elements.</param>
        /// <param name="a">The Hermitian matrix A with dimension (<paramref name="lda"/>, <paramref name="n"/>).</param>
        /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1, <paramref name="n"/>).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
        [TestCaseSource(nameof(TestCaseData_zher2))]
        public void zher2_TestCaseData_ResultOfBenchmarkImplementation(int n, Complex alpha, Complex[] x, Complex[] y, Complex[] a, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1, int incY = 1)
        {
            var actual = a.ToArray();
            m_TestObject.zher2(n, alpha, x, y, actual, lda, triangularMatrixType, incX, incY);

            var expected = a.ToArray();
            m_BenchmarkObject.zher2(n, alpha, x, y, expected, lda, triangularMatrixType, incX, incY);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'ZHER2'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_zher2
        {
            get
            {
                yield return new TestCaseData(4, -1.25 + 7.61 + Complex.ImaginaryOne, new Complex[] { 11, 12 + 7.61 + Complex.ImaginaryOne, 13, 14 }, new Complex[] { -16, -15 + 7.61 + Complex.ImaginaryOne, -14, -13 }, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8 + 7.61 + Complex.ImaginaryOne, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, 1, 1);
                yield return new TestCaseData(4, -1.25 + 7.61 + Complex.ImaginaryOne, new Complex[] { 11, 12, 13 + 7.61 + Complex.ImaginaryOne, 14 }, new Complex[] { -16, -15, -14 + 7.61 + Complex.ImaginaryOne, -13 }, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 + 7.61 + Complex.ImaginaryOne, 10, 11, 12, 13, 14, 15, 16 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, 1, 1);

                yield return new TestCaseData(4, -1.25 + 7.61 + Complex.ImaginaryOne, new Complex[] { 11, 12 + 7.61 + Complex.ImaginaryOne, 13, 14, 15, 16, 17 }, new Complex[] { -16, -15 + 7.61 + Complex.ImaginaryOne, -14, -13 }, new Complex[] { 1, 2, 3, 4, 5, 6 + 7.61 + Complex.ImaginaryOne, 7, 8 + 7.61 + Complex.ImaginaryOne, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, 2, 1);
                yield return new TestCaseData(4, -1.25 + 7.61 + Complex.ImaginaryOne, new Complex[] { 11, 12, 13 + 7.61 + Complex.ImaginaryOne, 14 }, new Complex[] { -16, -15, -14 + 7.61 + Complex.ImaginaryOne, -13 + 7.61 + Complex.ImaginaryOne, -12, -11, -10 }, new Complex[] { 1, 2, 3, 4 + 7.61 + Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, 1, 2);
            }
        }

        /// <summary>A test function that compares the result of 'ZHPMV' to the benchmark implementation.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="aPacked">The Hermitian packed matrix A with dimension at least (<paramref name="n"/> * (<paramref name="n"/> + 1) ) / 2.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incX"/>| elements.</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="y">The vector y with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incY"/>| elements.</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
        [TestCaseSource(nameof(TestCaseData_zhpmv))]
        public void zhpmv_TestCaseData_ResultOfBenchmarkImplementation(int n, Complex alpha, Complex[] aPacked, Complex[] x, Complex beta, Complex[] y, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1, int incY = 1)
        {
            var actual = y.ToArray();
            m_TestObject.zhpmv(n, alpha, aPacked, x, beta, actual, triangularMatrixType, incX, incY);

            var expected = y.ToArray();
            m_BenchmarkObject.zhpmv(n, alpha, aPacked, x, beta, expected, triangularMatrixType, incX, incY);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'ZHPMV'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_zhpmv
        {
            get
            {
                yield return new TestCaseData(4, -1.25 + 4.1 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3 + 4.1 * Complex.ImaginaryOne, 4, 5, 6, 7, 8, 9, 10 }, new Complex[] { 11, 12 + 4.1 * Complex.ImaginaryOne, 13, 14 }, -1.5 + 4.1 * Complex.ImaginaryOne, new Complex[] { -16, -15 + 4.1 * Complex.ImaginaryOne, -14, -13 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, 1, 1);
                yield return new TestCaseData(4, -1.25 + 4.1 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4 + 4.1 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10 }, new Complex[] { 11, 12, 13 + 4.1 * Complex.ImaginaryOne, 14 }, -1.5 + 4.1 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14 + 4.1 * Complex.ImaginaryOne, -13 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, 1, 1);

                yield return new TestCaseData(4, -1.25 + 4.1 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4 + 4.1 * Complex.ImaginaryOne, 5, 6 + 4.1 * Complex.ImaginaryOne, 7, 8, 9, 10 }, new Complex[] { 11, 12, 13 + 4.1 * Complex.ImaginaryOne, 14, 15, 16, 17 }, -1.5 + 4.1 * Complex.ImaginaryOne, new Complex[] { -16, -15 + 4.1 * Complex.ImaginaryOne, -14, -13 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, 2, 1);
                yield return new TestCaseData(4, -1.25 + 4.1 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4 + 4.1 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10 }, new Complex[] { 11, 12 + 4.1 * Complex.ImaginaryOne, 13, 14 }, -1.5 + 4.1 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14 + 4.1 * Complex.ImaginaryOne, -13 + 4.1 * Complex.ImaginaryOne, -12, -11, -10 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, 1, 2);
            }
        }

        /// <summary>A test function that compares the result of 'ZHPR' to the benchmark implementation.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incX"/>| elements.</param>
        /// <param name="aPacked">The Hermitian packed matrix A with dimension at least (<paramref name="n"/> * (<paramref name="n"/> + 1) ) / 2.</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        [TestCaseSource(nameof(TestCaseData_zhpr))]
        public void zhpr_TestCaseData_ResultOfBenchmarkImplementation(int n, double alpha, Complex[] x, Complex[] aPacked, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1)
        {
            var actual = aPacked.ToArray();
            m_TestObject.zhpr(n, alpha, x, actual, triangularMatrixType, incX);

            var expected = aPacked.ToArray();
            m_BenchmarkObject.zhpr(n, alpha, x, expected, triangularMatrixType, incX);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'ZHPR'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_zhpr
        {
            get
            {
                yield return new TestCaseData(4, -1.25, new Complex[] { 11, 12, 13 - 2.15 * Complex.ImaginaryOne, 14 }, new Complex[] { 1, 2, 3 - 2.15 * Complex.ImaginaryOne, 4, 5, 6, 7, 8, 9, 10 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, 1);
                yield return new TestCaseData(4, -1.25, new Complex[] { 11, 12 - 2.15 * Complex.ImaginaryOne, 13, 14 }, new Complex[] { 1, 2, 3, 4, 5 - 2.15 * Complex.ImaginaryOne, 6, 7, 8, 9, 10 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, 1);

                yield return new TestCaseData(4, -1.25, new Complex[] { 11, 12, 13 - 2.15 * Complex.ImaginaryOne, 14, 15, 16, 17 }, new Complex[] { 1, 2, 3, 4 - 2.15 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, 2);
            }
        }

        /// <summary>A test function that compares the result of 'ZHPR2' to the benchmark implementation.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incX"/>| elements.</param>
        /// <param name="y">The vector y with at least 1 + (<paramref name="n"/>-1) * |<paramref name="incY"/>| elements.</param>
        /// <param name="aPacked">The Hermitian packed matrix A with dimension at least (<paramref name="n"/> * (<paramref name="n"/> + 1) ) / 2.</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        /// <param name="incY">The increment for the elements of <paramref name="y"/>.</param>
        [TestCaseSource(nameof(TestCaseData_zhpr2))]
        public void zhpr2_TestCaseData_ResultOfBenchmarkImplementation(int n, Complex alpha, Complex[] x, Complex[] y, Complex[] aPacked, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, int incX = 1, int incY = 1)
        {
            var actual = aPacked.ToArray();
            m_TestObject.zhpr2(n, alpha, x, y, actual, triangularMatrixType, incX, incY);

            var expected = aPacked.ToArray();
            m_BenchmarkObject.zhpr2(n, alpha, x, y, expected, triangularMatrixType, incX, incY);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'DSPR2'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_zhpr2
        {
            get
            {
                yield return new TestCaseData(4, -1.25 + 3.51 * Complex.ImaginaryOne, new Complex[] { 11, 12, 13 + 3.51 * Complex.ImaginaryOne, 14 }, new Complex[] { -16, -15, -14 + 3.51 * Complex.ImaginaryOne, -13 }, new Complex[] { 1, 2, 3, 4 + 3.51 * Complex.ImaginaryOne, 5, 6 + 3.51 * Complex.ImaginaryOne, 7, 8, 9, 10 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, 1, 1);
                yield return new TestCaseData(4, -1.25 + 3.51 * Complex.ImaginaryOne, new Complex[] { 11, 12, 13 + 3.51 * Complex.ImaginaryOne, 14 }, new Complex[] { -16, -15 + 3.51 * Complex.ImaginaryOne, -14, -13 }, new Complex[] { 1, 2, 3, 4 + 3.51 * Complex.ImaginaryOne, 5, 6, 7 + 3.51 * Complex.ImaginaryOne, 8, 9, 10 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, 1, 1);

                yield return new TestCaseData(4, -1.25 + 3.51 * Complex.ImaginaryOne, new Complex[] { 11, 12 + 3.51 * Complex.ImaginaryOne, 13, 14, 15, 16, 17 }, new Complex[] { -16, -15 + 3.51 * Complex.ImaginaryOne, -14 + 3.51 * Complex.ImaginaryOne, -13 }, new Complex[] { 1, 2, 3, 4, 5 + 3.51 * Complex.ImaginaryOne, 6 + 3.51 * Complex.ImaginaryOne, 7, 8, 9, 10 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, 2, 1);
                yield return new TestCaseData(4, -1.25 + 3.51 * Complex.ImaginaryOne, new Complex[] { 11 + 3.51 * Complex.ImaginaryOne, 12, 13, 14 }, new Complex[] { -16, -15 + 3.51 * Complex.ImaginaryOne, -14 + 3.51 * Complex.ImaginaryOne, -13 + 3.51 * Complex.ImaginaryOne, -12, -11, -10 }, new Complex[] { 1, 2, 3, 4, 5 + 3.51 * Complex.ImaginaryOne, 6 + 3.51 * Complex.ImaginaryOne, 7, 8, 9, 10 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, 1, 2);
            }
        }

        /// <summary>A test function that compares the result of 'ZTBMV' to the benchmark implementation.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="k">The number of super-diagonals of A if the matrix A is provided in its upper triangular representation; the number of sub-diagonals otherwise.</param>
        /// <param name="a">The triangular band matrix with dimension (<paramref name="lda"/>, <paramref name="n"/>).</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
        /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least (1  + <paramref name="k"/>).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A', 'op(A)=A^t' or 'op(A)=A^h'.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        [TestCaseSource(nameof(TestCaseData_ztbmv))]
        public void ztbmv_TestCaseData_ResultOfBenchmarkImplementation(int n, int k, Complex[] a, Complex[] x, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1)
        {
            var actual = x.ToArray();
            m_TestObject.ztbmv(n, k, a, actual, lda, triangularMatrixType, isUnitTriangular, transpose, incX);

            var expected = x.ToArray();
            m_BenchmarkObject.ztbmv(n, k, a, expected, lda, triangularMatrixType, isUnitTriangular, transpose, incX);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'ZTBMV'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_ztbmv
        {
            get
            {
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3, 4, 5 - 0.65 * Complex.ImaginaryOne, 6, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11, 12 - 0.65 * Complex.ImaginaryOne, 13, 14 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3, 4, 5, 6 - 0.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11, 12 - 0.65 * Complex.ImaginaryOne, 13, 14 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3, 4, 5, 6 - 0.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11, 12 - 0.65 * Complex.ImaginaryOne, 13, 14 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.Hermite, 1);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3, 4, 5, 6 - 0.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11, 12 - 0.65 * Complex.ImaginaryOne, 13, 14 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11 - 0.65 * Complex.ImaginaryOne, 12, 13, 14 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11 - 0.65 * Complex.ImaginaryOne, 12, 13, 14 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.Hermite, 1);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3, 4 - 0.65 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11, 12 - 0.65 * Complex.ImaginaryOne, 13, 14 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3, 4, 5 - 0.65 * Complex.ImaginaryOne, 6, 7 - 0.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12 }, new Complex[] { 11, 12 - 0.65 * Complex.ImaginaryOne, 13 - 0.65 * Complex.ImaginaryOne, 14 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3, 4, 5 - 0.65 * Complex.ImaginaryOne, 6, 7 - 0.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12 }, new Complex[] { 11, 12 - 0.65 * Complex.ImaginaryOne, 13 - 0.65 * Complex.ImaginaryOne, 14 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.Hermite, 1);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8 - 0.65 * Complex.ImaginaryOne, 9, 10, 11, 12 }, new Complex[] { 11, 12 - 0.65 * Complex.ImaginaryOne, 13, 14 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3, 4, 5, 6 - 0.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11, 12 - 0.65 * Complex.ImaginaryOne, 13, 14 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3, 4, 5, 6 - 0.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11, 12 - 0.65 * Complex.ImaginaryOne, 13, 14 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.Hermite, 1);

                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3 - 0.65 * Complex.ImaginaryOne, 4, 5, 6, 7 - 0.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12 }, new Complex[] { 11, 12 - 0.65 * Complex.ImaginaryOne, 13, 14 - 0.65 * Complex.ImaginaryOne, 15 - 0.65 * Complex.ImaginaryOne, 16, 17 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3 - 0.65 * Complex.ImaginaryOne, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11, 12 - 0.65 * Complex.ImaginaryOne, 13 - 0.65 * Complex.ImaginaryOne, 14, 15, 16, 17 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3 - 0.65 * Complex.ImaginaryOne, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11, 12 - 0.65 * Complex.ImaginaryOne, 13 - 0.65 * Complex.ImaginaryOne, 14, 15, 16, 17 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.Hermite, 2);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3, 4 - 0.65 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11, 12 - 0.65 * Complex.ImaginaryOne, 13 - 0.65 * Complex.ImaginaryOne, 14, 15, 16, 17 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3 - 0.65 * Complex.ImaginaryOne, 4, 5 - 0.65 * Complex.ImaginaryOne, 6, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11 - 0.65 * Complex.ImaginaryOne, 12 - 0.65 * Complex.ImaginaryOne, 13, 14 - 0.65 * Complex.ImaginaryOne, 15, 16, 17 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3 - 0.65 * Complex.ImaginaryOne, 4, 5 - 0.65 * Complex.ImaginaryOne, 6, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11 - 0.65 * Complex.ImaginaryOne, 12 - 0.65 * Complex.ImaginaryOne, 13, 14 - 0.65 * Complex.ImaginaryOne, 15, 16, 17 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.Hermite, 2);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2 - 0.65 * Complex.ImaginaryOne, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11, 12 - 0.65 * Complex.ImaginaryOne, 13 - 0.65 * Complex.ImaginaryOne, 14, 15, 16, 17 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3, 4, 5, 6, 7 - 0.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12 }, new Complex[] { 11, 12 - 0.65 * Complex.ImaginaryOne, 13 - 0.65 * Complex.ImaginaryOne, 14, 15, 16, 17 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3, 4, 5, 6, 7 - 0.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12 }, new Complex[] { 11, 12 - 0.65 * Complex.ImaginaryOne, 13 - 0.65 * Complex.ImaginaryOne, 14, 15, 16, 17 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.Hermite, 2);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3, 4, 5 - 0.65 * Complex.ImaginaryOne, 6 - 0.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11 - 0.65 * Complex.ImaginaryOne, 12 - 0.65 * Complex.ImaginaryOne, 13 - 0.65 * Complex.ImaginaryOne, 14, 15, 16, 17 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3, 4, 5, 6, 7 - 0.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12 }, new Complex[] { 11, 12 - 0.65 * Complex.ImaginaryOne, 13 - 0.65 * Complex.ImaginaryOne, 14 - 0.65 * Complex.ImaginaryOne, 15, 16, 17 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3, 4, 5, 6, 7 - 0.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12 }, new Complex[] { 11, 12 - 0.65 * Complex.ImaginaryOne, 13 - 0.65 * Complex.ImaginaryOne, 14 - 0.65 * Complex.ImaginaryOne, 15, 16, 17 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.Hermite, 2);
            }
        }

        /// <summary>A test function that compares the result of 'ZTBSV' to the benchmark implementation.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="k">The number of super-diagonals of A if the matrix A is provided in its upper triangular representation; the number of sub-diagonals otherwise.</param>
        /// <param name="a">The triangular band matrix with dimension (<paramref name="lda"/>, <paramref name="n"/>).</param>
        /// <param name="x">The vector b (input), x (output) with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements (input/output).</param>
        /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least (1  + <paramref name="k"/>).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A', 'op(A)=A^t' or 'op(A)=A^h'.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        [TestCaseSource(nameof(TestCaseData_ztbsv))]
        public void ztbsv_TestCaseData_ResultOfBenchmarkImplementation(int n, int k, Complex[] a, Complex[] x, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1)
        {
            var actual = x.ToArray();
            m_TestObject.ztbsv(n, k, a, actual, lda, triangularMatrixType, isUnitTriangular, transpose, incX);

            var expected = x.ToArray();
            m_BenchmarkObject.ztbsv(n, k, a, expected, lda, triangularMatrixType, isUnitTriangular, transpose, incX);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'ZTBSV'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_ztbsv
        {
            get
            {
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3 + 0.24 * Complex.ImaginaryOne, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11, 12 + 0.24 * Complex.ImaginaryOne, 13, 14 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3 + 0.24 * Complex.ImaginaryOne, 4 + 0.24 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11, 12 + 0.24 * Complex.ImaginaryOne, 13, 14 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3 + 0.24 * Complex.ImaginaryOne, 4 + 0.24 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11, 12 + 0.24 * Complex.ImaginaryOne, 13, 14 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.Hermite, 1);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3 + 0.24 * Complex.ImaginaryOne, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11, 12 + 0.24 * Complex.ImaginaryOne, 13 + 0.24 * Complex.ImaginaryOne, 14 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3, 4, 5, 6 + 0.24 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11, 12 + 0.24 * Complex.ImaginaryOne, 13, 14 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3, 4, 5, 6 + 0.24 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11, 12 + 0.24 * Complex.ImaginaryOne, 13, 14 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.Hermite, 1);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3, 4, 5 + 0.24 * Complex.ImaginaryOne, 6, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11, 12 + 0.24 * Complex.ImaginaryOne, 13, 14 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3, 4, 5 + 0.24 * Complex.ImaginaryOne, 6, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11 + 0.24 * Complex.ImaginaryOne, 12, 13, 14 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3, 4, 5 + 0.24 * Complex.ImaginaryOne, 6, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11 + 0.24 * Complex.ImaginaryOne, 12, 13, 14 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.Hermite, 1);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3 + 0.24 * Complex.ImaginaryOne, 4 + 0.24 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11 + 0.24 * Complex.ImaginaryOne, 12, 13, 14 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3 + 0.24 * Complex.ImaginaryOne, 4, 5 + 0.24 * Complex.ImaginaryOne, 6, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11 + 0.24 * Complex.ImaginaryOne, 12 + 0.24 * Complex.ImaginaryOne, 13, 14 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3 + 0.24 * Complex.ImaginaryOne, 4, 5 + 0.24 * Complex.ImaginaryOne, 6, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11 + 0.24 * Complex.ImaginaryOne, 12 + 0.24 * Complex.ImaginaryOne, 13, 14 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.Hermite, 1);

                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3 + 0.24 * Complex.ImaginaryOne, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11 + 0.24 * Complex.ImaginaryOne, 12, 13, 14, 15, 16, 17 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3 + 0.24 * Complex.ImaginaryOne, 4 + 0.24 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11 + 0.24 * Complex.ImaginaryOne, 12, 13 + 0.24 * Complex.ImaginaryOne, 14, 15, 16, 17 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3 + 0.24 * Complex.ImaginaryOne, 4 + 0.24 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11 + 0.24 * Complex.ImaginaryOne, 12, 13 + 0.24 * Complex.ImaginaryOne, 14, 15, 16, 17 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.Hermite, 2);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3, 4, 5 + 0.24 * Complex.ImaginaryOne, 6 + 0.24 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11, 12 + 0.24 * Complex.ImaginaryOne, 13, 14, 15, 16, 17 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3, 4, 5, 6 + 0.24 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11, 12 + 0.24 * Complex.ImaginaryOne, 13, 14, 15, 16, 17 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3, 4, 5, 6 + 0.24 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11, 12 + 0.24 * Complex.ImaginaryOne, 13, 14, 15, 16, 17 }, 3, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.Hermite, 2);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3, 4, 5 + 0.24 * Complex.ImaginaryOne, 6, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11, 12, 13 + 0.24 * Complex.ImaginaryOne, 14, 15, 16, 17 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3, 4 + 0.24 * Complex.ImaginaryOne, 5 + 0.24 * Complex.ImaginaryOne, 6, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11, 12 + 0.24 * Complex.ImaginaryOne, 13, 14, 15, 16, 17 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3, 4 + 0.24 * Complex.ImaginaryOne, 5 + 0.24 * Complex.ImaginaryOne, 6, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11, 12 + 0.24 * Complex.ImaginaryOne, 13, 14, 15, 16, 17 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.Hermite, 2);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2 + 0.24 * Complex.ImaginaryOne, 3 + 0.24 * Complex.ImaginaryOne, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11, 12 + 0.24 * Complex.ImaginaryOne, 13, 14 + 0.24 * Complex.ImaginaryOne, 15, 16, 17 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3 + 0.24 * Complex.ImaginaryOne, 4, 5 + 0.24 * Complex.ImaginaryOne, 6, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11, 12 + 0.24 * Complex.ImaginaryOne, 13 + 0.24 * Complex.ImaginaryOne, 14, 15, 16, 17 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, 2, new Complex[] { 1, 2, 3 + 0.24 * Complex.ImaginaryOne, 4, 5 + 0.24 * Complex.ImaginaryOne, 6, 7, 8, 9, 10, 11, 12 }, new Complex[] { 11, 12 + 0.24 * Complex.ImaginaryOne, 13 + 0.24 * Complex.ImaginaryOne, 14, 15, 16, 17 }, 3, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.Hermite, 2);
            }
        }

        /// <summary>A test function that compares the result of 'ZTPMV' to the benchmark implementation.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="aPacked">The triangular packed matrix A with dimension at least (<paramref name="n"/> * (<paramref name="n"/> + 1) ) / 2.</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A', 'op(A)=A^t' or 'op(A)=A^h'.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        [TestCaseSource(nameof(TestCaseData_ztpmv))]
        public void ztpmv_TestCaseData_ResultOfBenchmarkImplementation(int n, Complex[] aPacked, Complex[] x, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1)
        {
            var actual = x.ToArray();
            m_TestObject.ztpmv(n, aPacked, actual, triangularMatrixType, isUnitTriangular, transpose, incX);

            var expected = x.ToArray();
            m_BenchmarkObject.ztpmv(n, aPacked, expected, triangularMatrixType, isUnitTriangular, transpose, incX);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'ZTPMV'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_ztpmv
        {
            get
            {
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4 - 2.3 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10 }, new Complex[] { 11, 12, 13, 14 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5 - 2.3 * Complex.ImaginaryOne, 6, 7, 8, 9, 10 }, new Complex[] { 11, 12, 13, 14 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5 - 2.3 * Complex.ImaginaryOne, 6, 7, 8, 9, 10 }, new Complex[] { 11, 12, 13, 14 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.Hermite, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new Complex[] { 11, 12, 13, 14 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5 - 2.3 * Complex.ImaginaryOne, 6, 7, 8, 9 - 2.3 * Complex.ImaginaryOne, 10 }, new Complex[] { 11, 12, 13, 14 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5 - 2.3 * Complex.ImaginaryOne, 6, 7, 8, 9 - 2.3 * Complex.ImaginaryOne, 10 }, new Complex[] { 11, 12, 13, 14 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.Hermite, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4 - 2.3 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10 }, new Complex[] { 11, 12, 13, 14 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3 - 2.3 * Complex.ImaginaryOne, 4 - 2.3 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10 }, new Complex[] { 11, 12, 13, 14 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3 - 2.3 * Complex.ImaginaryOne, 4 - 2.3 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10 }, new Complex[] { 11, 12, 13, 14 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.Hermite, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3 - 2.3 * Complex.ImaginaryOne, 4, 5, 6, 7, 8, 9, 10 }, new Complex[] { 11, 12, 13, 14 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5 - 2.3 * Complex.ImaginaryOne, 6, 7, 8, 9, 10 }, new Complex[] { 11, 12, 13, 14 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5 - 2.3 * Complex.ImaginaryOne, 6, 7, 8, 9, 10 }, new Complex[] { 11, 12, 13, 14 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.Hermite, 1);


                yield return new TestCaseData(4, new Complex[] { 1, 2, 3 - 2.3 * Complex.ImaginaryOne, 4, 5, 6, 7, 8, 9, 10 }, new Complex[] { 11, 12 - 2.3 * Complex.ImaginaryOne, 13, 14, 15, 16, 17 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4 - 2.3 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10 }, new Complex[] { 11, 12, 13, 14 - 2.3 * Complex.ImaginaryOne, 15, 16, 17 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4 - 2.3 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10 }, new Complex[] { 11, 12, 13, 14 - 2.3 * Complex.ImaginaryOne, 15, 16, 17 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.Hermite, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4 - 2.3 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10 }, new Complex[] { 11, 12, 13 - 2.3 * Complex.ImaginaryOne, 14, 15, 16, 17 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3 - 2.3 * Complex.ImaginaryOne, 4, 5, 6, 7, 8, 9, 10 }, new Complex[] { 11, 12, 13 - 2.3 * Complex.ImaginaryOne, 14 - 2.3 * Complex.ImaginaryOne, 15, 16, 17 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3 - 2.3 * Complex.ImaginaryOne, 4, 5, 6, 7, 8, 9, 10 }, new Complex[] { 11, 12, 13 - 2.3 * Complex.ImaginaryOne, 14 - 2.3 * Complex.ImaginaryOne, 15, 16, 17 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.Hermite, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5, 6 - 2.3 * Complex.ImaginaryOne, 7, 8, 9, 10 }, new Complex[] { 11, 12 - 2.3 * Complex.ImaginaryOne, 13, 14, 15, 16, 17 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5 - 2.3 * Complex.ImaginaryOne, 6, 7, 8, 9, 10 }, new Complex[] { 11, 12, 13, 14, 15 - 2.3 * Complex.ImaginaryOne, 16 - 2.3 * Complex.ImaginaryOne, 17 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5 - 2.3 * Complex.ImaginaryOne, 6, 7, 8, 9, 10 }, new Complex[] { 11, 12, 13, 14, 15 - 2.3 * Complex.ImaginaryOne, 16 - 2.3 * Complex.ImaginaryOne, 17 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.Hermite, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3 - 2.3 * Complex.ImaginaryOne, 4, 5, 6, 7, 8, 9, 10 }, new Complex[] { 11, 12 - 2.3 * Complex.ImaginaryOne, 13 - 2.3 * Complex.ImaginaryOne, 14, 15, 16, 17 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4 - 2.3 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10 }, new Complex[] { 11, 12 - 2.3 * Complex.ImaginaryOne, 13 - 2.3 * Complex.ImaginaryOne, 14, 15, 16, 17 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4 - 2.3 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10 }, new Complex[] { 11, 12 - 2.3 * Complex.ImaginaryOne, 13 - 2.3 * Complex.ImaginaryOne, 14, 15, 16, 17 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.Hermite, 2);
            }
        }


        /// <summary>A test function that compares the result of 'ZTPSV' to the benchmark implementation.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="aPacked">The triangular packed matrix A with dimension at least (<paramref name="n"/> * (<paramref name="n"/> + 1) ) / 2.</param>
        /// <param name="x">The vector b (input), x (output) with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements (input/output).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A', 'op(A)=A^t' or 'op(A)=A^h'.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        [TestCaseSource(nameof(TestCaseData_ztpsv))]
        public void ztpsv_TestCaseData_ResultOfBenchmarkImplementation(int n, Complex[] aPacked, Complex[] x, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1)
        {
            var actual = x.ToArray();
            m_TestObject.ztpsv(n, aPacked, actual, triangularMatrixType, isUnitTriangular, transpose, incX);

            var expected = x.ToArray();
            m_BenchmarkObject.ztpsv(n, aPacked, expected, triangularMatrixType, isUnitTriangular, transpose, incX);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'ZTPSV'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_ztpsv
        {
            get
            {
                yield return new TestCaseData(4, new Complex[] { 1 - 0.75 * Complex.ImaginaryOne, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new Complex[] { 11, 12 - 0.75 * Complex.ImaginaryOne, 13, 14 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4 - 0.75 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10 }, new Complex[] { 11, 12 - 0.75 * Complex.ImaginaryOne, 13, 14 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4 - 0.75 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10 }, new Complex[] { 11, 12 - 0.75 * Complex.ImaginaryOne, 13, 14 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.Hermite, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3 - 0.75 * Complex.ImaginaryOne, 4 - 0.75 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10 }, new Complex[] { 11, 12, 13, 14 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4 - 0.75 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10 }, new Complex[] { 11 - 0.75 * Complex.ImaginaryOne, 12, 13, 14 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4 - 0.75 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10 }, new Complex[] { 11 - 0.75 * Complex.ImaginaryOne, 12, 13, 14 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.Hermite, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5 - 0.75 * Complex.ImaginaryOne, 6, 7, 8, 9, 10 }, new Complex[] { 11, 12, 13 - 0.75 * Complex.ImaginaryOne, 14 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5, 6 - 0.75 * Complex.ImaginaryOne, 7, 8, 9, 10 }, new Complex[] { 11, 12, 13 - 0.75 * Complex.ImaginaryOne, 14 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5, 6 - 0.75 * Complex.ImaginaryOne, 7, 8, 9, 10 }, new Complex[] { 11, 12, 13 - 0.75 * Complex.ImaginaryOne, 14 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.Hermite, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4 - 0.75 * Complex.ImaginaryOne, 5, 6 - 0.75 * Complex.ImaginaryOne, 7, 8, 9, 10 }, new Complex[] { 11, 12 - 0.75 * Complex.ImaginaryOne, 13, 14 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2 - 0.75 * Complex.ImaginaryOne, 3, 4 - 0.75 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10 }, new Complex[] { 11, 12 - 0.75 * Complex.ImaginaryOne, 13, 14 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2 - 0.75 * Complex.ImaginaryOne, 3, 4 - 0.75 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10 }, new Complex[] { 11, 12 - 0.75 * Complex.ImaginaryOne, 13, 14 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.Hermite, 1);


                yield return new TestCaseData(4, new Complex[] { 1, 2 - 0.75 * Complex.ImaginaryOne, 3, 4, 5, 6, 7, 8, 9, 10 }, new Complex[] { 11, 12 - 0.75 * Complex.ImaginaryOne, 13 - 0.75 * Complex.ImaginaryOne, 14, 15, 16, 17 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2 - 0.75 * Complex.ImaginaryOne, 3 - 0.75 * Complex.ImaginaryOne, 4, 5, 6, 7, 8, 9, 10 }, new Complex[] { 11, 12 - 0.75 * Complex.ImaginaryOne, 13 - 0.75 * Complex.ImaginaryOne, 14, 15, 16, 17 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2 - 0.75 * Complex.ImaginaryOne, 3 - 0.75 * Complex.ImaginaryOne, 4, 5, 6, 7, 8, 9, 10 }, new Complex[] { 11, 12 - 0.75 * Complex.ImaginaryOne, 13 - 0.75 * Complex.ImaginaryOne, 14, 15, 16, 17 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.Hermite, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4 - 0.75 * Complex.ImaginaryOne, 5, 6, 7, 8 - 0.75 * Complex.ImaginaryOne, 9, 10 }, new Complex[] { 11, 12, 13 - 0.75 * Complex.ImaginaryOne, 14 - 0.75 * Complex.ImaginaryOne, 15, 16, 17 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3 - 0.75 * Complex.ImaginaryOne, 4, 5, 6, 7, 8, 9, 10 }, new Complex[] { 11, 12 - 0.75 * Complex.ImaginaryOne, 13 - 0.75 * Complex.ImaginaryOne, 14, 15, 16, 17 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3 - 0.75 * Complex.ImaginaryOne, 4, 5, 6, 7, 8, 9, 10 }, new Complex[] { 11, 12 - 0.75 * Complex.ImaginaryOne, 13 - 0.75 * Complex.ImaginaryOne, 14, 15, 16, 17 }, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.Hermite, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5, 6, 7 - 0.75 * Complex.ImaginaryOne, 8, 9, 10 }, new Complex[] { 11, 12 - 0.75 * Complex.ImaginaryOne, 13 - 0.75 * Complex.ImaginaryOne, 14, 15, 16, 17 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5, 6 - 0.75 * Complex.ImaginaryOne, 7, 8, 9, 10 }, new Complex[] { 11, 12, 13 - 0.75 * Complex.ImaginaryOne, 14 - 0.75 * Complex.ImaginaryOne, 15 - 0.75 * Complex.ImaginaryOne, 16, 17 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5, 6 - 0.75 * Complex.ImaginaryOne, 7, 8, 9, 10 }, new Complex[] { 11, 12, 13 - 0.75 * Complex.ImaginaryOne, 14 - 0.75 * Complex.ImaginaryOne, 15 - 0.75 * Complex.ImaginaryOne, 16, 17 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.Hermite, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5 - 0.75 * Complex.ImaginaryOne, 6, 7, 8, 9, 10 }, new Complex[] { 11, 12 - 0.75 * Complex.ImaginaryOne, 13 - 0.75 * Complex.ImaginaryOne, 14, 15, 16, 17 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3 - 0.75 * Complex.ImaginaryOne, 4 - 0.75 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10 }, new Complex[] { 11 - 0.75 * Complex.ImaginaryOne, 12 - 0.75 * Complex.ImaginaryOne, 13 - 0.75 * Complex.ImaginaryOne, 14, 15, 16, 17 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3 - 0.75 * Complex.ImaginaryOne, 4 - 0.75 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10 }, new Complex[] { 11 - 0.75 * Complex.ImaginaryOne, 12 - 0.75 * Complex.ImaginaryOne, 13 - 0.75 * Complex.ImaginaryOne, 14, 15, 16, 17 }, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.Hermite, 2);
            }
        }

        /// <summary>A test function that compares the result of 'ZTRMV' to the benchmark implementation.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="a">The triangular matrix A with dimension at least (<paramref name="lda"/>, <paramref name="n"/>).</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
        /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1, <paramref name="n"/>).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A', 'op(A)=A^t' or 'op(A)=A^h'.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        [TestCaseSource(nameof(TestCaseData_ztrmv))]
        public void ztrmv_TestCaseData_ResultOfBenchmarkImplementation(int n, Complex[] a, Complex[] x, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1)
        {
            var actual = x.ToArray();
            m_TestObject.ztrmv(n, a, actual, lda, triangularMatrixType, isUnitTriangular, transpose, incX);

            var expected = x.ToArray();
            m_BenchmarkObject.ztrmv(n, a, expected, lda, triangularMatrixType, isUnitTriangular, transpose, incX);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'ZTRMV'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_ztrmv
        {
            get
            {
                yield return new TestCaseData(4, new Complex[] { 1, 2 - 2.34 * Complex.ImaginaryOne, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12 - 2.34 * Complex.ImaginaryOne, 13, 14 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5, 6, 7 - 2.34 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13 - 2.34 * Complex.ImaginaryOne, 14 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5, 6, 7 - 2.34 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13 - 2.34 * Complex.ImaginaryOne, 14 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.Hermite, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5, 6 - 2.34 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4 - 2.34 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12 - 2.34 * Complex.ImaginaryOne, 13, 14 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4 - 2.34 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12 - 2.34 * Complex.ImaginaryOne, 13, 14 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.Hermite, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3 - 2.34 * Complex.ImaginaryOne, 4, 5 - 2.34 * Complex.ImaginaryOne, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12 - 2.34 * Complex.ImaginaryOne, 13, 14 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4 - 2.34 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4 - 2.34 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.Hermite, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3 - 2.34 * Complex.ImaginaryOne, 4, 5 - 2.34 * Complex.ImaginaryOne, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11 - 2.34 * Complex.ImaginaryOne, 12 - 2.34 * Complex.ImaginaryOne, 13, 14 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4 - 2.34 * Complex.ImaginaryOne, 5, 6 - 2.34 * Complex.ImaginaryOne, 7 - 2.34 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11 - 2.34 * Complex.ImaginaryOne, 12 - 2.34 * Complex.ImaginaryOne, 13, 14 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4 - 2.34 * Complex.ImaginaryOne, 5, 6 - 2.34 * Complex.ImaginaryOne, 7 - 2.34 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11 - 2.34 * Complex.ImaginaryOne, 12 - 2.34 * Complex.ImaginaryOne, 13, 14 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.Hermite, 1);


                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5 - 2.34 * Complex.ImaginaryOne, 6 - 2.34 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13 - 2.34 * Complex.ImaginaryOne, 14 - 2.34 * Complex.ImaginaryOne, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5 - 2.34 * Complex.ImaginaryOne, 6 - 2.34 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13 - 2.34 * Complex.ImaginaryOne, 14 - 2.34 * Complex.ImaginaryOne, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5 - 2.34 * Complex.ImaginaryOne, 6 - 2.34 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13 - 2.34 * Complex.ImaginaryOne, 14 - 2.34 * Complex.ImaginaryOne, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.Hermite, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5 - 2.34 * Complex.ImaginaryOne, 6 - 2.34 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13 - 2.34 * Complex.ImaginaryOne, 14 - 2.34 * Complex.ImaginaryOne, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5 - 2.34 * Complex.ImaginaryOne, 6 - 2.34 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13 - 2.34 * Complex.ImaginaryOne, 14 - 2.34 * Complex.ImaginaryOne, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5 - 2.34 * Complex.ImaginaryOne, 6 - 2.34 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13 - 2.34 * Complex.ImaginaryOne, 14 - 2.34 * Complex.ImaginaryOne, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.Hermite, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5 - 2.34 * Complex.ImaginaryOne, 6 - 2.34 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13 - 2.34 * Complex.ImaginaryOne, 14 - 2.34 * Complex.ImaginaryOne, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5 - 2.34 * Complex.ImaginaryOne, 6 - 2.34 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13 - 2.34 * Complex.ImaginaryOne, 14 - 2.34 * Complex.ImaginaryOne, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5 - 2.34 * Complex.ImaginaryOne, 6 - 2.34 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13 - 2.34 * Complex.ImaginaryOne, 14 - 2.34 * Complex.ImaginaryOne, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.Hermite, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5 - 2.34 * Complex.ImaginaryOne, 6 - 2.34 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13 - 2.34 * Complex.ImaginaryOne, 14 - 2.34 * Complex.ImaginaryOne, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5 - 2.34 * Complex.ImaginaryOne, 6 - 2.34 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13 - 2.34 * Complex.ImaginaryOne, 14 - 2.34 * Complex.ImaginaryOne, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5 - 2.34 * Complex.ImaginaryOne, 6 - 2.34 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13 - 2.34 * Complex.ImaginaryOne, 14 - 2.34 * Complex.ImaginaryOne, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.Hermite, 2);
            }
        }

        /// <summary>A test function that compares the result of 'DTRSV' to the benchmark implementation.
        /// </summary>
        /// <param name="n">The order of matrix A.</param>
        /// <param name="a">The triangular matrix A with dimension (<paramref name="lda"/>, <paramref name="n"/>).</param>
        /// <param name="x">The vector x with at least 1 + (<paramref name="n"/> - 1) * | <paramref name="incX"/> | elements.</param>
        /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1, <paramref name="n"/>).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A', 'op(A)=A^t' or 'op(A)=A^h'.</param>
        /// <param name="incX">The increment for the elements of <paramref name="x"/>.</param>
        [TestCaseSource(nameof(TestCaseData_ztrsv))]
        public void ztrsv_TestCaseData_ResultOfBenchmarkImplementation(int n, Complex[] a, Complex[] x, int lda, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, bool isUnitTriangular = true, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose, int incX = 1)
        {
            var actual = x.ToArray();
            m_TestObject.ztrsv(n, a, actual, lda, triangularMatrixType, isUnitTriangular, transpose, incX);

            var expected = x.ToArray();
            m_BenchmarkObject.ztrsv(n, a, expected, lda, triangularMatrixType, isUnitTriangular, transpose, incX);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'ZTRSV'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_ztrsv
        {
            get
            {
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3 - 0.25 * Complex.ImaginaryOne, 4 - 0.25 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12 - 0.25 * Complex.ImaginaryOne, 13, 14 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3 - 0.25 * Complex.ImaginaryOne, 4 - 0.25 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12 - 0.25 * Complex.ImaginaryOne, 13, 14 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3 - 0.25 * Complex.ImaginaryOne, 4 - 0.25 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12 - 0.25 * Complex.ImaginaryOne, 13, 14 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.Hermite, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3 - 0.25 * Complex.ImaginaryOne, 4 - 0.25 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12 - 0.25 * Complex.ImaginaryOne, 13, 14 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3 - 0.25 * Complex.ImaginaryOne, 4 - 0.25 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12 - 0.25 * Complex.ImaginaryOne, 13, 14 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3 - 0.25 * Complex.ImaginaryOne, 4 - 0.25 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12 - 0.25 * Complex.ImaginaryOne, 13, 14 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.Hermite, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3 - 0.25 * Complex.ImaginaryOne, 4 - 0.25 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12 - 0.25 * Complex.ImaginaryOne, 13, 14 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3 - 0.25 * Complex.ImaginaryOne, 4 - 0.25 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12 - 0.25 * Complex.ImaginaryOne, 13, 14 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3 - 0.25 * Complex.ImaginaryOne, 4 - 0.25 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12 - 0.25 * Complex.ImaginaryOne, 13, 14 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.Hermite, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3 - 0.25 * Complex.ImaginaryOne, 4 - 0.25 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12 - 0.25 * Complex.ImaginaryOne, 13, 14 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3 - 0.25 * Complex.ImaginaryOne, 4 - 0.25 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12 - 0.25 * Complex.ImaginaryOne, 13, 14 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 1);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3 - 0.25 * Complex.ImaginaryOne, 4 - 0.25 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12 - 0.25 * Complex.ImaginaryOne, 13, 14 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.Hermite, 1);

                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5 - 0.25 * Complex.ImaginaryOne, 6 - 0.25 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12 - 0.25 * Complex.ImaginaryOne, 13 - 0.25 * Complex.ImaginaryOne, 14, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5 - 0.25 * Complex.ImaginaryOne, 6 - 0.25 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12 - 0.25 * Complex.ImaginaryOne, 13 - 0.25 * Complex.ImaginaryOne, 14, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5 - 0.25 * Complex.ImaginaryOne, 6 - 0.25 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12 - 0.25 * Complex.ImaginaryOne, 13 - 0.25 * Complex.ImaginaryOne, 14, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, true, BLAS.MatrixTransposeState.Hermite, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5 - 0.25 * Complex.ImaginaryOne, 6 - 0.25 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12 - 0.25 * Complex.ImaginaryOne, 13 - 0.25 * Complex.ImaginaryOne, 14, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5 - 0.25 * Complex.ImaginaryOne, 6 - 0.25 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12 - 0.25 * Complex.ImaginaryOne, 13 - 0.25 * Complex.ImaginaryOne, 14, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5 - 0.25 * Complex.ImaginaryOne, 6 - 0.25 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12 - 0.25 * Complex.ImaginaryOne, 13 - 0.25 * Complex.ImaginaryOne, 14, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, false, BLAS.MatrixTransposeState.Hermite, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5 - 0.25 * Complex.ImaginaryOne, 6 - 0.25 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12 - 0.25 * Complex.ImaginaryOne, 13 - 0.25 * Complex.ImaginaryOne, 14, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5 - 0.25 * Complex.ImaginaryOne, 6 - 0.25 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12 - 0.25 * Complex.ImaginaryOne, 13 - 0.25 * Complex.ImaginaryOne, 14, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5 - 0.25 * Complex.ImaginaryOne, 6 - 0.25 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12 - 0.25 * Complex.ImaginaryOne, 13 - 0.25 * Complex.ImaginaryOne, 14, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, true, BLAS.MatrixTransposeState.Hermite, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5 - 0.25 * Complex.ImaginaryOne, 6 - 0.25 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12 - 0.25 * Complex.ImaginaryOne, 13 - 0.25 * Complex.ImaginaryOne, 14, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.NoTranspose, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5 - 0.25 * Complex.ImaginaryOne, 6 - 0.25 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12 - 0.25 * Complex.ImaginaryOne, 13 - 0.25 * Complex.ImaginaryOne, 14, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.Transpose, 2);
                yield return new TestCaseData(4, new Complex[] { 1, 2, 3, 4, 5 - 0.25 * Complex.ImaginaryOne, 6 - 0.25 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12 - 0.25 * Complex.ImaginaryOne, 13 - 0.25 * Complex.ImaginaryOne, 14, 15, 16, 17 }, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, false, BLAS.MatrixTransposeState.Hermite, 2);
            }
        }
        #endregion

        #endregion

        #region protected methods

        /// <summary>Gets the level 2 BLAS implementation.
        /// </summary>
        /// <returns>A <see cref="ILevel2BLAS"/> object that encapuslate the level 2 BLAS functions.</returns>
        protected abstract ILevel2BLAS GetLevel2BLAS();
        #endregion
    }
}