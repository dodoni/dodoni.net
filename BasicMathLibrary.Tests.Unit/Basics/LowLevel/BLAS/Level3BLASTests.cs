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
    /// <summary>Serves as abstract unit test class for Level 3 BLAS methods.
    /// </summary>
    public abstract class Level3BLASTests
    {
        #region private members

        /// <summary>The Level 3 BLAS implementation to test.
        /// </summary>
        private ILevel3BLAS m_TestObject;

        /// <summary>A benchmark Level 3 BLAS implementation.
        /// </summary>
        private BuildInLevel3BLAS m_BenchmarkObject = new BuildInLevel3BLAS();
        #endregion

        #region protected constructors

        /// <summary>Initializes a new instance of the <see cref="Level3BLASTests"/> class.
        /// </summary>
        protected Level3BLASTests()
        {
            m_TestObject = GetLevel3BLAS();
        }
        #endregion

        #region public methods

        /// <summary>The SetUp method for the unit tests.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            m_BenchmarkObject = new BuildInLevel3BLAS();
            m_TestObject = GetLevel3BLAS();
        }

        #region double precision methods

        /// <summary>A test function that compares the result of 'DGEMM' to the benchmark implementation.
        /// </summary>
        /// <param name="m">The number of rows of the matrix op(A) and of the matrix C.</param>
        /// <param name="n">The number of columns of the matrix op(B) and of the matrix C.</param>
        /// <param name="k">The number of columns of the matrix op(A) and the number of rows of the matrix op(B).</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The matrix A supplied column-by-column of dimension (<paramref name="lda"/>, ka), where ka is <paramref name="k"/> if op(A) = A; <paramref name="m"/> otherwise.</param>
        /// <param name="b">The matrix B supplied column-by-column of dimension (<paramref name="ldb"/>, kb), where kb is <paramref name="n"/> if op(B) = B; <paramref name="k"/> otherwise.</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="c">The matrix C supplied column-by-column of dimension (<paramref name="ldc"/>, <paramref name="n"/>).</param>
        /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="m"/>) if op(A) = A; max(1, <paramref name="k"/>) otherwise.</param>
        /// <param name="ldb">The leading dimension of <paramref name="b"/>, must be at least max(1,<paramref name="k"/>) if op(B) = B; max(1, <paramref name="n"/>) otherwise.</param>
        /// <param name="ldc">The leading dimension of <paramref name="c"/>, must be at least max(1, <paramref name="m"/>).</param>
        /// <param name="transposeA">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        /// <param name="transposeB">A value indicating whether 'op(B)=B' or 'op(B)=B^t'.</param>
        [TestCaseSource(nameof(TestCaseData_dgemm))]
        public void dgemm_TestCaseData_ResultOfBenchmarkImplementation(int m, int n, int k, double alpha, double[] a, double[] b, double beta, double[] c, int lda, int ldb, int ldc, BLAS.MatrixTransposeState transposeA = BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState transposeB = BLAS.MatrixTransposeState.NoTranspose)
        {
            var actual = c.ToArray();
            m_TestObject.dgemm(m, n, k, alpha, a, b, beta, actual, lda, ldb, ldc, transposeA, transposeB);

            var expected = c.ToArray();
            m_BenchmarkObject.dgemm(m, n, k, alpha, a, b, beta, expected, lda, ldb, ldc, transposeA, transposeB);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'DGEMM'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_dgemm
        {
            get
            {
                // wrong input test case
//                yield return new TestCaseData(4, 4, 4, 1.0, new double[] { 1, 2, 3, 4 }, new double[] { 11, 12, 13, 14 }, 1, new double[] { -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.NoTranspose).Throws(typeof(ArgumentException)).SetDescription("The size of the array is smaller than expected");

                // squared input matrices
                yield return new TestCaseData(4, 4, 4, 1.0, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1, new double[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(4, 4, 4, -2.25, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1.65, new double[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(4, 4, 4, 3.5, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, -0.75, new double[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.NoTranspose);

                yield return new TestCaseData(4, 4, 4, 1.0, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1, new double[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(4, 4, 4, -2.25, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1.65, new double[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(4, 4, 4, 3.5, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, -0.75, new double[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.Transpose);

                yield return new TestCaseData(4, 4, 4, 1.0, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1, new double[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.Transpose, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(4, 4, 4, -2.25, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1.65, new double[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.Transpose, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(4, 4, 4, 3.5, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, -0.75, new double[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.Transpose, BLAS.MatrixTransposeState.NoTranspose);

                yield return new TestCaseData(4, 4, 4, 1.0, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1, new double[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.Transpose, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(4, 4, 4, -2.25, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1.65, new double[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.Transpose, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(4, 4, 4, 3.5, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, -0.75, new double[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.Transpose, BLAS.MatrixTransposeState.Transpose);


                // non-squared input matrices
                yield return new TestCaseData(2, 3, 4, 1.0, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1, new double[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 2, 4, 2, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 3, 4, -2.25, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1.65, new double[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 2, 4, 2, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 3, 4, 3.5, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, -0.75, new double[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 2, 4, 2, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.NoTranspose);

                yield return new TestCaseData(2, 3, 4, 1.0, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1, new double[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 2, 3, 2, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(2, 3, 4, -2.25, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1.65, new double[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 2, 3, 2, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(2, 3, 4, 3.5, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, -0.75, new double[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 2, 3, 2, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.Transpose);

                yield return new TestCaseData(2, 3, 4, 1.0, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1, new double[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 2, BLAS.MatrixTransposeState.Transpose, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 3, 4, -2.25, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1.65, new double[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 2, BLAS.MatrixTransposeState.Transpose, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 3, 4, 3.5, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, -0.75, new double[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 2, BLAS.MatrixTransposeState.Transpose, BLAS.MatrixTransposeState.NoTranspose);

                yield return new TestCaseData(2, 3, 4, 1.0, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1, new double[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 3, 2, BLAS.MatrixTransposeState.Transpose, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(2, 3, 4, -2.25, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1.65, new double[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 3, 2, BLAS.MatrixTransposeState.Transpose, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(2, 3, 4, 3.5, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new double[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, -0.75, new double[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 3, 2, BLAS.MatrixTransposeState.Transpose, BLAS.MatrixTransposeState.Transpose);
            }
        }

        /// <summary>A test function that compares the result of 'DSYMM' to the benchmark implementation.
        /// </summary>
        /// <param name="m">The number of rows of the matrix C.</param>
        /// <param name="n">The number of columns of the matrix C.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The symmetric matrix A supplied column-by-column of dimension (<paramref name="lda"/>, ka), where ka is <paramref name="m"/> if to calculate C := \alpha * A*B + \beta*C; otherwise <paramref name="n"/>.</param>
        /// <param name="b">The matrix B supplied column-by-column of dimension (<paramref name="ldb"/>,<paramref name="n"/>).</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="c">The matrix C supplied column-by-column of dimension (<paramref name="ldc"/>,<paramref name="n"/>); input/output.</param>
        /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="m"/>) if <paramref name="side"/>=left; max(1,n) otherwise.</param>
        /// <param name="ldb">The leading dimension of <paramref name="b"/>, must be at least max(1,<paramref name="m"/>).</param>
        /// <param name="ldc">The leading dimension of <paramref name="c"/>, must be at least max(1,<paramref name="m"/>).</param>
        /// <param name="side">A value indicating whether to calculate C := \alpha * A*B + \beta*C or C := \alpha * B*A +\beta*C.</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        [TestCaseSource(nameof(TestCaseData_dsymm))]
        public void dsymm_TestCaseData_ResultOfBenchmarkImplementation(int m, int n, double alpha, double[] a, double[] b, double beta, double[] c, int lda, int ldb, int ldc, BLAS.Side side = BLAS.Side.Left, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix)
        {
            var actual = c.ToArray();
            m_TestObject.dsymm(m, n, alpha, a, b, beta, actual, lda, ldb, ldc, side, triangularMatrixType);

            var expected = c.ToArray();
            m_BenchmarkObject.dsymm(m, n, alpha, a, b, beta, expected, lda, ldb, ldc, side, triangularMatrixType);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'DSYMM'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_dsymm
        {
            get
            {
                // simple quadratic matrices
                yield return new TestCaseData(2, 2, 1.0, new double[] { 1, 1, 1, 1 }, new double[] { -9.5, 4.5, 2.25, 6.75 }, 2.0, new double[] { 1.0, 0.0, 1.0, 0.0 }, 2, 2, 2, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix);
                yield return new TestCaseData(2, 2, 1.0, new double[] { 1, 1, 1, 1 }, new double[] { -9.5, 4.5, 2.25, 6.75 }, 2.0, new double[] { 1.0, 0.0, 1.0, 0.0 }, 2, 2, 2, BLAS.Side.Left, BLAS.TriangularMatrixType.UpperTriangularMatrix);
                yield return new TestCaseData(2, 2, 1.0, new double[] { 1, 1, 1, 1 }, new double[] { -9.5, 4.5, 2.25, 6.75 }, 2.0, new double[] { 1.0, 0.0, 1.0, 0.0 }, 2, 2, 2, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix);
                yield return new TestCaseData(2, 2, 1.0, new double[] { 1, 1, 1, 1 }, new double[] { -9.5, 4.5, 2.25, 6.75 }, 2.0, new double[] { 1.0, 0.0, 1.0, 0.0 }, 2, 2, 2, BLAS.Side.Right, BLAS.TriangularMatrixType.UpperTriangularMatrix);

                // Matrix a should be symmetric, but we refer to the lower/upper part only
                yield return new TestCaseData(2, 3, 1.25, new double[] { 0.25, 1.25, 2.25, -2.5, 3.1, -4.5 }, new double[] { -9.5, 4.5, 2.25, 6.75, -0.75, 1.89 }, 2.25, new double[] { 1.0, 2.0, 3.25, -5.41, 4.75, -9.76 }, 2, 2, 2, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix);
                yield return new TestCaseData(2, 3, -1.1, new double[] { 0.25, 1.25, 2.25, -2.5, 3.1, -4.5 }, new double[] { -9.5, 4.5, 2.25, 6.75, -0.75, 1.89 }, -2.8, new double[] { 1.0, 2.0, 3.25, -5.41, 4.75, -9.76 }, 2, 2, 2, BLAS.Side.Left, BLAS.TriangularMatrixType.UpperTriangularMatrix);
                yield return new TestCaseData(2, 3, 2.5, new double[] { 0.25, 1.25, 2.25, -2.5, 3.1, -4.5, -1.64, 2.47, -3.86 }, new double[] { -9.5, 4.5, 2.25, 6.75, -0.75, 1.89 }, 3.1, new double[] { 1.0, 2.0, 3.25, -5.41, 4.75, -9.76 }, 3, 2, 2, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix);
                yield return new TestCaseData(2, 3, -3.2, new double[] { 0.25, 1.25, 2.25, -2.5, 3.1, -4.5, -1.64, 2.47, -3.86 }, new double[] { -9.5, 4.5, 2.25, 6.75, -0.75, 1.89 }, -1.7, new double[] { 1.0, 2.0, 3.25, -5.41, 4.75, -9.76 }, 3, 2, 2, BLAS.Side.Right, BLAS.TriangularMatrixType.UpperTriangularMatrix);
            }
        }

        /// <summary>A test function that compares the result of 'DSYRK' to the benchmark implementation.
        /// </summary>
        /// <param name="n">The order of matrix C.</param>
        /// <param name="k">The number of columns of matrix A if to calculate C:= \alpha*A*A^t + \beta *C; otherwise the number of rows of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The matrix A supplied column-by-column of dimension (<paramref name="lda"/>, ka), where ka is <paramref name="k"/> if to calculate C:= \alpha*A*A^t + \beta *C; otherwise <paramref name="n"/>.</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="c">The symmetric matrix C supplied column-by-column of dimension (<paramref name="ldc"/>, <paramref name="n"/>).</param>
        /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="n"/>) if to calculate C:= \alpha*A*A^t + \beta *C; max(1,<paramref name="k"/>) otherwise.</param>
        /// <param name="ldc">The leading dimension of <paramref name="c"/>, must be at least max(1,<paramref name="n"/>).</param>
        /// <param name="triangularMatrixType">A value whether matrix C is in its upper or lower triangular representation.</param>
        /// <param name="transpose">A value indicating whether to calculate C:= \alpha*A*A^t + \beta *C or C:= \alpha*A^t*A + \beta*C.</param>
        [TestCaseSource(nameof(TestCaseData_dsyrk))]
        public void dsyrk_TestCaseData_ResultOfBenchmarkImplementation(int n, int k, double alpha, double[] a, double beta, double[] c, int lda, int ldc, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.XsyrkOperation transpose = BLAS.XsyrkOperation.ATimesATranspose)
        {
            var actual = c.ToArray();
            m_TestObject.dsyrk(n, k, alpha, a, beta, actual, lda, ldc, triangularMatrixType, transpose);

            var expected = c.ToArray();
            m_BenchmarkObject.dsyrk(n, k, alpha, a, beta, expected, lda, ldc, triangularMatrixType, transpose);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'DSYRK'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_dsyrk
        {
            get
            {
                // simple quadratic matrices
                yield return new TestCaseData(2, 2, 1.0, new double[] { 1, 1, 1, 1 }, 2.0, new double[] { -9.5, 4.5, 2.25, 6.75 }, 2, 2, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.XsyrkOperation.ATimesATranspose);
                yield return new TestCaseData(2, 2, 1.0, new double[] { 1, 1, 1, 1 }, 2.0, new double[] { -9.5, 4.5, 2.25, 6.75 }, 2, 2, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.XsyrkOperation.AtransposeTimesA);
                yield return new TestCaseData(2, 2, 1.0, new double[] { 1, 1, 1, 1 }, 2.0, new double[] { -9.5, 4.5, 2.25, 6.75 }, 2, 2, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.XsyrkOperation.ATimesATranspose);
                yield return new TestCaseData(2, 2, 1.0, new double[] { 1, 1, 1, 1 }, 2.0, new double[] { -9.5, 4.5, 2.25, 6.75 }, 2, 2, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.XsyrkOperation.AtransposeTimesA);

                yield return new TestCaseData(2, 3, 1.25, new double[] { 0.25, 1.25, 2.25, -2.5, 3.1, -4.5 }, -2.25, new double[] { -9.5, 4.5, 2.25, 6.75 }, 2, 2, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.XsyrkOperation.ATimesATranspose);
                yield return new TestCaseData(2, 3, -1.1, new double[] { 0.25, 1.25, 2.25, -2.5, 3.1, -4.5 }, 1.25, new double[] { -9.5, 4.5, 2.25, 6.75 }, 3, 2, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.XsyrkOperation.AtransposeTimesA);
                yield return new TestCaseData(2, 3, 2.5, new double[] { 0.25, 1.25, 2.25, -2.5, 3.1, -4.5 }, 3.4, new double[] { -9.5, 4.5, 2.25, 6.75 }, 2, 2, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.XsyrkOperation.ATimesATranspose);
                yield return new TestCaseData(2, 3, -3.2, new double[] { 0.25, 1.25, 2.25, -2.5, 3.1, -4.5 }, -7.75, new double[] { -9.5, 4.5, 2.25, 6.75 }, 3, 2, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.XsyrkOperation.AtransposeTimesA);
            }
        }

        /// <summary>A test function that compares the result of 'DSYR2K' to the benchmark implementation.
        /// </summary>
        /// <param name="n">The order of matrix C.</param>
        /// <param name="k">The The number of columns of matrices A and B or the number .</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The matrix A supplied column-by-column of dimension (<paramref name="lda"/>, ka), where ka is <paramref name="k"/> if to calculate C := alpha*A*B^t + alpha*B*A^t + beta*C; otherwise <paramref name="n"/>.</param>
        /// <param name="b">The matrix B supplied column-by-column of dimension (<paramref name="ldb"/>, kb), where ka is at least max(1,<paramref name="n"/>) if to calculate C := alpha*A*B^t + alpha*B*A^t + beta*C; otherwise at least max(1,<paramref name="k"/>).</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="c">The symmetric matrix C supplied column-by-column of dimension (<paramref name="ldc"/>, <paramref name="n"/>).</param>
        /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="n"/>) if to calculate C:= alpha*A*B^t+alpha*B*A^t+beta*C; max(1,<paramref name="k"/>) otherwise.</param>
        /// <param name="ldb">The leading dimension of <paramref name="b"/>, must be at least max(1,<paramref name="n"/>) if to calculate C:= alpha*A*B^t+alpha*B*A^t+beta*C; max(1,<paramref name="k"/>) otherwise.</param>        
        /// <param name="ldc">The leading dimension of <paramref name="c"/>, must be at least max(1,<paramref name="n"/>).</param>        
        /// <param name="triangularMatrixType">A value whether matrix C is in its upper or lower triangular representation.</param>
        /// <param name="transpose">A value indicating whether to calculate C := alpha*A*B^t + alpha*B*A^t + beta*C or C := alpha*A^t*B + alpha*B^t*A + beta*C.</param>
        [TestCaseSource(nameof(TestCaseData_dsyr2k))]
        public void dsyr2k_TestCaseData_ResultOfBenchmarkImplementation(int n, int k, double alpha, double[] a, double[] b, double beta, double[] c, int lda, int ldb, int ldc, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.Xsyr2kOperation transpose = BLAS.Xsyr2kOperation.ATimesBTransPlusBTimesATrans)
        {
            var actual = c.ToArray();
            m_TestObject.dsyr2k(n, k, alpha, a, b, beta, actual, lda, ldb, ldc, triangularMatrixType, transpose);

            var expected = c.ToArray();
            m_BenchmarkObject.dsyr2k(n, k, alpha, a, b, beta, expected, lda, ldb, ldc, triangularMatrixType, transpose);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'DSYR2K'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_dsyr2k
        {
            get
            {
                // simple quadratic matrices
                yield return new TestCaseData(2, 2, 1.0, new double[] { 1, 1, 1, 1 }, new double[] { 1.0, 0.0, 1.0, 0.0 }, 2.0, new double[] { -9.5, 4.5, 2.25, 6.75 }, 2, 2, 2, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.Xsyr2kOperation.ATimesBTransPlusBTimesATrans);
                yield return new TestCaseData(2, 2, 1.0, new double[] { 1, 1, 1, 1 }, new double[] { 1.0, 0.0, 1.0, 0.0 }, 2.0, new double[] { -9.5, 4.5, 2.25, 6.75 }, 2, 2, 2, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.Xsyr2kOperation.ATimesBTransPlusBTimesATrans);
                yield return new TestCaseData(2, 2, 1.0, new double[] { 1, 1, 1, 1 }, new double[] { 1.0, 0.0, 1.0, 0.0 }, 2.0, new double[] { -9.5, 4.5, 2.25, 6.75 }, 2, 2, 2, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.Xsyr2kOperation.ATransTimesBPlusBTransTimesA);
                yield return new TestCaseData(2, 2, 1.0, new double[] { 1, 1, 1, 1 }, new double[] { 1.0, 0.0, 1.0, 0.0 }, 2.0, new double[] { -9.5, 4.5, 2.25, 6.75 }, 2, 2, 2, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.Xsyr2kOperation.ATransTimesBPlusBTransTimesA);

                yield return new TestCaseData(2, 3, -1.25, new double[] { 0.25, 1.25, 2.25, -2.5, 3.1, -4.5 }, new double[] { 1.0, 2.0, 3.25, -5.41, 4.75, -9.76 }, -2.25, new double[] { -9.5, 4.5, 2.25, 6.75, 1.25, 2.76 }, 2, 2, 2, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.Xsyr2kOperation.ATimesBTransPlusBTimesATrans);
                yield return new TestCaseData(2, 3, 2.5, new double[] { 0.25, 1.25, 2.25, -2.5, 3.1, -4.5 }, new double[] { 1.0, 2.0, 3.25, -5.41, 4.75, -9.76 }, 1.25, new double[] { -9.5, 4.5, 2.25, 6.75, -2.54, 3.76 }, 2, 2, 2, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.Xsyr2kOperation.ATimesBTransPlusBTimesATrans);
                yield return new TestCaseData(2, 3, 3.75, new double[] { 0.25, 1.25, 2.25, -2.5, 3.1, -4.5 }, new double[] { 1.0, 2.0, 3.25, -5.41, 4.75, -9.76 }, 3.4, new double[] { -9.5, 4.5, 2.25, 6.75, 9.65, -0.76 }, 3, 3, 2, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.Xsyr2kOperation.ATransTimesBPlusBTransTimesA);
                yield return new TestCaseData(2, 3, -4.85, new double[] { 0.25, 1.25, 2.25, -2.5, 3.1, -4.5 }, new double[] { 1.0, 2.0, 3.25, -5.41, 4.75, -9.76 }, -7.75, new double[] { -9.5, 4.5, 2.25, 6.75, 5.84, -0.0416 }, 3, 3, 2, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.Xsyr2kOperation.ATransTimesBPlusBTransTimesA);
            }
        }

        /// <summary>A test function that compares the result of 'DTRMM' to the benchmark implementation.
        /// </summary>
        /// <param name="m">The number of rows of matrix B.</param>
        /// <param name="n">The number of columns of matrix B.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The triangular matrix A supplied column-by-column of dimension (<paramref name="lda"/>, k), where k is <paramref name="m"/> if to calculate B := \alpha * op(A)*B; <paramref name="n"/> otherwise.</param>
        /// <param name="b">The matrix B supplied column-by-column of dimension (<paramref name="ldb"/>, <paramref name="n"/>).</param>                
        /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="m"/>) if to calculate B := \alpha * op(A)*B; max(1,<paramref name="n"/>) otherwise.</param>
        /// <param name="ldb">The leading dimension of <paramref name="b"/>, must be at least max(1,<paramref name="m"/>).</param>
        /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
        /// <param name="side">A value indicating whether to calculate B := \alpha * op(A)*B or B:= \alpha *B * op(A).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        [TestCaseSource(nameof(TestCaseData_dtrmm))]
        public void dtrmm_TestCaseData_ResultOfBenchmarkImplementation(int m, int n, double alpha, double[] a, double[] b, int lda, int ldb, bool isUnitTriangular = true, BLAS.Side side = BLAS.Side.Left, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose)
        {
            var actual = b.ToArray();
            m_TestObject.dtrmm(m, n, alpha, a, actual, lda, ldb, isUnitTriangular, side, triangularMatrixType, transpose);

            var expected = b.ToArray();
            m_BenchmarkObject.dtrmm(m, n, alpha, a, expected, lda, ldb, isUnitTriangular, side, triangularMatrixType, transpose);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'DTRMM'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_dtrmm
        {
            get
            {
                // simple quadratic matrices
                yield return new TestCaseData(2, 2, 1.0, new double[] { 1, 1, 1, 1 }, new double[] { -9.5, 4.5, 2.25, 6.75 }, 2, 2, false, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 2, 1.0, new double[] { 1, 1, 1, 1 }, new double[] { -9.5, 4.5, 2.25, 6.75 }, 2, 2, false, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(2, 2, 1.0, new double[] { 1, 1, 1, 1 }, new double[] { -9.5, 4.5, 2.25, 6.75 }, 2, 2, false, BLAS.Side.Left, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 2, 1.0, new double[] { 1, 1, 1, 1 }, new double[] { -9.5, 4.5, 2.25, 6.75 }, 2, 2, false, BLAS.Side.Left, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Transpose);

                yield return new TestCaseData(2, 2, 1.0, new double[] { 1, 1, 1, 1 }, new double[] { -9.5, 4.5, 2.25, 6.75 }, 2, 2, false, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 2, 1.0, new double[] { 1, 1, 1, 1 }, new double[] { -9.5, 4.5, 2.25, 6.75 }, 2, 2, false, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(2, 2, 1.0, new double[] { 1, 1, 1, 1 }, new double[] { -9.5, 4.5, 2.25, 6.75 }, 2, 2, false, BLAS.Side.Right, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 2, 1.0, new double[] { 1, 1, 1, 1 }, new double[] { -9.5, 4.5, 2.25, 6.75 }, 2, 2, false, BLAS.Side.Right, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Transpose);


                yield return new TestCaseData(2, 3, -1.25, new double[] { 0.25, 1.25, 2.25, -2.5, 3.1, -4.5 }, new double[] { 1.0, 2.0, 3.25, -5.41, 4.75, -9.76 }, 2, 2, false, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 3, 2.25, new double[] { 0.25, 1.25, 2.25, -2.5, 3.1, -4.5 }, new double[] { 1.0, 2.0, 3.25, -5.41, 4.75, -9.76 }, 2, 2, false, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(2, 3, -3.61, new double[] { 0.25, 1.25, 2.25, -2.5, 3.1, -4.5 }, new double[] { 1.0, 2.0, 3.25, -5.41, 4.75, -9.76 }, 2, 2, false, BLAS.Side.Left, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 3, 4.6, new double[] { 0.25, 1.25, 2.25, -2.5, 3.1, -4.5 }, new double[] { 1.0, 2.0, 3.25, -5.41, 4.75, -9.76 }, 2, 2, false, BLAS.Side.Left, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Transpose);

                yield return new TestCaseData(2, 3, -2.4, new double[] { 0.25, 1.25, 2.25, -2.5, 3.1, -4.5, -1.23, 2.4, -7.23 }, new double[] { 1.0, 2.0, 3.25, -5.41, 4.75, -9.76 }, 3, 2, false, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 3, 1.65, new double[] { 0.25, 1.25, 2.25, -2.5, 3.1, -4.5, -1.23, 2.4, -7.23 }, new double[] { 1.0, 2.0, 3.25, -5.41, 4.75, -9.76 }, 3, 2, false, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(2, 3, -0.83, new double[] { 0.25, 1.25, 2.25, -2.5, 3.1, -4.5, -1.23, 2.4, -7.23 }, new double[] { 1.0, 2.0, 3.25, -5.41, 4.75, -9.76 }, 3, 2, false, BLAS.Side.Right, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 3, 11.22, new double[] { 0.25, 1.25, 2.25, -2.5, 3.1, -4.5, -1.23, 2.4, -7.23 }, new double[] { 1.0, 2.0, 3.25, -5.41, 4.75, -9.76 }, 3, 2, false, BLAS.Side.Right, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
            }
        }

        /// <summary>A test function that compares the result of 'DTRSM' to the benchmark implementation.
        /// </summary>
        /// <param name="m">The number of rows of matrix B.</param>
        /// <param name="n">The number of column of matrix B.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The triangular matrix A supplied column-by-column of dimension (<paramref name="lda"/>, k), where k is <paramref name="m"/> if to calculate op(A) * X = \alpha * B; <paramref name="n"/> otherwise.</param>
        /// <param name="b">The matrix B supplied column-by-column of dimension (<paramref name="ldb"/>, <paramref name="n"/>).</param>
        /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="m"/>) if to calculate op(A) * X = \alpha * B; max(1,<paramref name="n"/>) otherwise.</param>
        /// <param name="ldb">The leading dimension of <paramref name="b"/>, must be at least max(1,<paramref name="m"/>).</param>
        /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
        /// <param name="side">A value indicating whether to calculate op(A) * X = \alpha * B or X * op(A) = \alpha *B.</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        [TestCaseSource(nameof(TestCaseData_dtrsm))]
        public void dtrsm_TestCaseData_ResultOfBenchmarkImplementation(int m, int n, double alpha, double[] a, double[] b, int lda, int ldb, bool isUnitTriangular = true, BLAS.Side side = BLAS.Side.Left, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose)
        {
            var actual = b.ToArray();
            m_TestObject.dtrsm(m, n, alpha, a, actual, lda, ldb, isUnitTriangular, side, triangularMatrixType, transpose);

            var expected = b.ToArray();
            m_BenchmarkObject.dtrsm(m, n, alpha, a, expected, lda, ldb, isUnitTriangular, side, triangularMatrixType, transpose);

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'DTRSM'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_dtrsm
        {
            get
            {
                // simple quadratic matrices
                yield return new TestCaseData(2, 2, 1.0, new double[] { 1, 1, 1, 1 }, new double[] { -9.5, 4.5, 2.25, 6.75 }, 2, 2, false, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 2, 1.0, new double[] { 1, 1, 1, 1 }, new double[] { -9.5, 4.5, 2.25, 6.75 }, 2, 2, false, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(2, 2, 1.0, new double[] { 1, 1, 1, 1 }, new double[] { -9.5, 4.5, 2.25, 6.75 }, 2, 2, false, BLAS.Side.Left, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 2, 1.0, new double[] { 1, 1, 1, 1 }, new double[] { -9.5, 4.5, 2.25, 6.75 }, 2, 2, false, BLAS.Side.Left, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(2, 2, 1.0, new double[] { 1, 1, 1, 1 }, new double[] { -9.5, 4.5, 2.25, 6.75 }, 2, 2, false, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 2, 1.0, new double[] { 1, 1, 1, 1 }, new double[] { -9.5, 4.5, 2.25, 6.75 }, 2, 2, false, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(2, 2, 1.0, new double[] { 1, 1, 1, 1 }, new double[] { -9.5, 4.5, 2.25, 6.75 }, 2, 2, false, BLAS.Side.Right, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 2, 1.0, new double[] { 1, 1, 1, 1 }, new double[] { -9.5, 4.5, 2.25, 6.75 }, 2, 2, false, BLAS.Side.Right, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Transpose);

                yield return new TestCaseData(2, 3, -1.25, new double[] { 0.25, 1.25, 2.25, -2.5, 3.1, -4.5 }, new double[] { 1.0, 2.0, 3.25, -5.41, 4.75, -9.76 }, 2, 2, false, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 3, 3.1, new double[] { 0.25, 1.25, 2.25, -2.5, 3.1, -4.5 }, new double[] { 1.0, 2.0, 3.25, -5.41, 4.75, -9.76 }, 2, 2, false, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(2, 3, 2.25, new double[] { 0.25, 1.25, 2.25, -2.5, 3.1, -4.5 }, new double[] { 1.0, 2.0, 3.25, -5.41, 4.75, -9.76 }, 2, 2, false, BLAS.Side.Left, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 3, -0.25, new double[] { 0.25, 1.25, 2.25, -2.5, 3.1, -4.5 }, new double[] { 1.0, 2.0, 3.25, -5.41, 4.75, -9.76 }, 2, 2, false, BLAS.Side.Left, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(2, 3, 4.62, new double[] { 0.25, 1.25, 2.25, -2.5, 3.1, -4.5, -1.95, 7.84, -6.75 }, new double[] { 1.0, 2.0, 3.25, -5.41, 4.75, -9.76 }, 3, 2, false, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 3, -2.1, new double[] { 0.25, 1.25, 2.25, -2.5, 3.1, -4.5, -1.95, 7.84, -6.75 }, new double[] { 1.0, 2.0, 3.25, -5.41, 4.75, -9.76 }, 3, 2, false, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(2, 3, -1.3, new double[] { 0.25, 1.25, 2.25, -2.5, 3.1, -4.5, -1.95, 7.84, -6.75 }, new double[] { 1.0, 2.0, 3.25, -5.41, 4.75, -9.76 }, 3, 2, false, BLAS.Side.Right, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 3, 12.3, new double[] { 0.25, 1.25, 2.25, -2.5, 3.1, -4.5, -1.95, 7.84, -6.75 }, new double[] { 1.0, 2.0, 3.25, -5.41, 4.75, -9.76 }, 3, 2, false, BLAS.Side.Right, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
            }
        }

        /// <summary>A test function for <c>aux_dgetrans</c>.
        /// </summary>
        /// <param name="rowCount">The number of rows.</param>
        /// <param name="columnCount">The number of columns.</param>
        /// <param name="a">The matrix to transpose (column-major ordering).</param>
        /// <param name="expected">The expected outcome.</param>
        [TestCase(2, 2, new double[] { 1, 2, 3, 4 }, new double[] { 1, 3, 2, 4 })]
        [TestCase(3, 3, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, new double[] { 1, 4, 7, 2, 5, 8, 3, 6, 9 })]
        [TestCase(2, 3, new double[] { 1, 2, 3, 4, 5, 6 }, new double[] { 1, 3, 5, 2, 4, 6 })]
        [TestCase(3, 2, new double[] { 1, 2, 3, 4, 5, 6 }, new double[] { 1, 4, 2, 5, 3, 6 })]
        public void aux_dgeTrans(int rowCount, int columnCount, double[] a, double[] expected)
        {
            var actual = a.ToArray();

            m_TestObject.aux_dgetrans(rowCount, columnCount, actual);
            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-9));
        }
        #endregion

        #region (double precision) complex methods

        /// <summary>A test function that compares the result of 'DZGEMM' to the benchmark implementation.
        /// </summary>
        /// <param name="m">The number of rows of the matrix op(A) and of the matrix C.</param>
        /// <param name="n">The number of columns of the matrix op(B) and of the matrix C.</param>
        /// <param name="k">The number of columns of the matrix op(A) and the number of rows of the matrix op(B).</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The matrix A supplied column-by-column of dimension (<paramref name="lda"/>, ka), where ka is <paramref name="k"/> if op(A) = A; <paramref name="m"/> otherwise.</param>
        /// <param name="b">The matrix B supplied column-by-column of dimension (<paramref name="ldb"/>, kb), where kb is <paramref name="n"/> if op(B) = B; <paramref name="k"/> otherwise.</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="c">The matrix C supplied column-by-column of dimension (<paramref name="ldc"/>, <paramref name="n"/>).</param>
        /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="m"/>) if op(A) = A; max(1, <paramref name="k"/>) otherwise.</param>
        /// <param name="ldb">The leading dimension of <paramref name="b"/>, must be at least max(1,<paramref name="k"/>) if op(B) = B; max(1, <paramref name="n"/>) otherwise.</param>
        /// <param name="ldc">The leading dimension of <paramref name="c"/>, must be at least max(1, <paramref name="m"/>).</param>
        /// <param name="transposeA">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        /// <param name="transposeB">A value indicating whether 'op(B)=B' or 'op(B)=B^t'.</param>
        [TestCaseSource(nameof(TestCaseData_dzgemm))]
        public void dzgemm_TestCaseData_ResultOfBenchmarkImplementation(int m, int n, int k, Complex alpha, double[] a, Complex[] b, Complex beta, Complex[] c, int lda, int ldb, int ldc, BLAS.MatrixTransposeState transposeA = BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState transposeB = BLAS.MatrixTransposeState.NoTranspose)
        {
            var actual = c.ToArray();
            m_TestObject.dzgemm(m, n, k, alpha, a, b, beta, actual, lda, ldb, ldc, transposeA, transposeB);

            var expected = c.ToArray();
            m_BenchmarkObject.dzgemm(m, n, k, alpha, a, b, beta, expected, lda, ldb, ldc, transposeA, transposeB);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'DZGEMM'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_dzgemm
        {
            get
            {
                // wrong input test case
//                yield return new TestCaseData(4, 4, 4, new Complex(1.0, 0.0), new double[] { 1, 2, 3, 4 }, new Complex[] { 11, 12, 13, 14 }, 1, new Complex[] { -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.NoTranspose).Throws(typeof(ArgumentException)).SetDescription("The size of the array is smaller than expected");

                // squared input matrices
                yield return new TestCaseData(4, 4, 4, 1.0 + 0.5 * Complex.ImaginaryOne, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22 + 2.0 * Complex.ImaginaryOne, 23, 24, 25, 26 }, 1 + Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11 + 2.1 * Complex.ImaginaryOne, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(4, 4, 4, -2.25 + 1.2 * Complex.ImaginaryOne, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16 + 2.0 * Complex.ImaginaryOne, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1.65 - 1.34 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10 - 2.1 * Complex.ImaginaryOne, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(4, 4, 4, 3.5 - 0.4 * Complex.ImaginaryOne, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12 + 2.0 * Complex.ImaginaryOne, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, -0.75 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1 + 2.1 * Complex.ImaginaryOne, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.NoTranspose);

                yield return new TestCaseData(4, 4, 4, 1.0 + 3.6 * Complex.ImaginaryOne, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15 - 2.0 * Complex.ImaginaryOne, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1.0 - 9.2 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9, -8 + 2.1 * Complex.ImaginaryOne, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(4, 4, 4, -2.25 * Complex.ImaginaryOne, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21 + 2.0 * Complex.ImaginaryOne, 22, 23, 24, 25, 26 }, 1.65 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12 + 2.1 * Complex.ImaginaryOne, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(4, 4, 4, 3.5 - 0.76 * Complex.ImaginaryOne, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16, 17 + 2.0 * Complex.ImaginaryOne, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, -0.75 + 0.2 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3 - 2.1 * Complex.ImaginaryOne, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.Transpose);

                yield return new TestCaseData(4, 4, 4, 1.0 + 3.6 * Complex.ImaginaryOne, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15 - 2.0 * Complex.ImaginaryOne, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1.0 - 9.2 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6 + 2.1 * Complex.ImaginaryOne, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.Hermite);
                yield return new TestCaseData(4, 4, 4, -2.25 * Complex.ImaginaryOne, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 + 2.0 * Complex.ImaginaryOne, 26 }, 1.65 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9 + 2.1 * Complex.ImaginaryOne, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.Hermite);
                yield return new TestCaseData(4, 4, 4, 3.5 - 0.76 * Complex.ImaginaryOne, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11 - 2.0 * Complex.ImaginaryOne, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, -0.75 + 0.2 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10 - 2.1 * Complex.ImaginaryOne, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.Hermite);

                yield return new TestCaseData(4, 4, 4, 1.0 - 2.3 * Complex.ImaginaryOne, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14 + 2.0 * Complex.ImaginaryOne, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1.0 + 2.4 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10 - 2.1 * Complex.ImaginaryOne, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.Transpose, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(4, 4, 4, -2.25 * Complex.ImaginaryOne, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 - 2.1 * Complex.ImaginaryOne, 24, 25, 26 }, 1.65 - 0.4 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3 + 2.1 * Complex.ImaginaryOne, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.Transpose, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(4, 4, 4, 3.5 + 4.6 * Complex.ImaginaryOne, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13 - 2.1 * Complex.ImaginaryOne, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, -0.75 + 2.56 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13 + 2.1 * Complex.ImaginaryOne, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.Transpose, BLAS.MatrixTransposeState.NoTranspose);

                yield return new TestCaseData(4, 4, 4, 1.0 - 6.45 * Complex.ImaginaryOne, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22 + 2.1 * Complex.ImaginaryOne, 23, 24, 25, 26 }, 1.2 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10 + 2.1 * Complex.ImaginaryOne, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.Transpose, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(4, 4, 4, -2.25 + 7.4 * Complex.ImaginaryOne, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15 + 2.1 * Complex.ImaginaryOne, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1.65 + 2.56 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12 - 2.1 * Complex.ImaginaryOne, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.Transpose, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(4, 4, 4, 3.5 * Complex.ImaginaryOne, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16, 17, 18 - 2.1 * Complex.ImaginaryOne, 19, 20, 21, 22, 23, 24, 25, 26 }, -0.75 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11 - 2.1 * Complex.ImaginaryOne, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.Transpose, BLAS.MatrixTransposeState.Transpose);

                yield return new TestCaseData(4, 4, 4, 1.0 - 6.45 * Complex.ImaginaryOne, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16, 17 - 2.1 * Complex.ImaginaryOne, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1.2 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11 + 2.1 * Complex.ImaginaryOne, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.Transpose, BLAS.MatrixTransposeState.Hermite);
                yield return new TestCaseData(4, 4, 4, -2.25 + 7.4 * Complex.ImaginaryOne, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13 - 2.1 * Complex.ImaginaryOne, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1.65 + 2.56 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6 + 2.1 * Complex.ImaginaryOne, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.Transpose, BLAS.MatrixTransposeState.Hermite);
                yield return new TestCaseData(4, 4, 4, 3.5 * Complex.ImaginaryOne, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12 + 2.1 * Complex.ImaginaryOne, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, -0.75 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7 + 2.1 * Complex.ImaginaryOne, -6, -5 + 2.1 * Complex.ImaginaryOne, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.Transpose, BLAS.MatrixTransposeState.Hermite);


                // non-squared input matrices
                yield return new TestCaseData(2, 3, 4, 1.0 + 0.9 * Complex.ImaginaryOne, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13 + 2.1 * Complex.ImaginaryOne, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1.0 + 2.4 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7 - 3.45 * Complex.ImaginaryOne, -6, -5, -1, -2, -3, -4 }, 2, 4, 2, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 3, 4, -2.25 * Complex.ImaginaryOne, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21 - 2.1 * Complex.ImaginaryOne, 22, 23, 24, 25, 26 }, 1.65 - 4.65 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3 + 3.45 * Complex.ImaginaryOne, -4 }, 2, 4, 2, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 3, 4, 3.5 - 2.45 * Complex.ImaginaryOne, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13 - 2.1 * Complex.ImaginaryOne, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, -0.75 + 2.4 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13 + 3.45 * Complex.ImaginaryOne, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 2, 4, 2, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.NoTranspose);

                yield return new TestCaseData(2, 3, 4, 1.23 * Complex.ImaginaryOne, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 - 2.1 * Complex.ImaginaryOne, 24 - 3.45 * Complex.ImaginaryOne, 25, 26 }, 1.0 - 1.2 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14 + 3.45 * Complex.ImaginaryOne, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 2, 3, 2, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(2, 3, 4, -2.25 + 5.6 * Complex.ImaginaryOne, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14 + 2.1 * Complex.ImaginaryOne, 15, 16, 17, 18, 19, 20, 21, 22, 23 - 3.45 * Complex.ImaginaryOne, 24, 25, 26 }, 1.65 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10 + 3.45 * Complex.ImaginaryOne, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 2, 3, 2, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(2, 3, 4, 3.5 - 2.5 * Complex.ImaginaryOne, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21 - 3.45 * Complex.ImaginaryOne, 22, 23, 24, 25, 26 }, -0.75 + 3.56 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7 + 3.45 * Complex.ImaginaryOne, -6, -5, -1, -2, -3, -4 }, 2, 3, 2, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.Hermite);

                yield return new TestCaseData(2, 3, 4, 1.0 * Complex.ImaginaryOne, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 + 3.45 * Complex.ImaginaryOne, 21, 22, 23, 24, 25, 26 }, 1.45 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9 - 3.45 * Complex.ImaginaryOne, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 2, BLAS.MatrixTransposeState.Transpose, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 3, 4, -2.25 - 0.4 * Complex.ImaginaryOne, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 - 3.45 * Complex.ImaginaryOne, 26 }, 1.65 + 2.4 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6 - 3.45 * Complex.ImaginaryOne, -5, -1, -2, -3, -4 }, 4, 4, 2, BLAS.MatrixTransposeState.Transpose, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 3, 4, 3.5 + 0.23 * Complex.ImaginaryOne, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16, 17, 18 + 3.45 * Complex.ImaginaryOne, 19, 20, 21, 22, 23, 24, 25, 26 }, -0.75 + 3.3 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9 - 3.45 * Complex.ImaginaryOne, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 2, BLAS.MatrixTransposeState.Transpose, BLAS.MatrixTransposeState.NoTranspose);

                yield return new TestCaseData(2, 3, 4, 1.4 * Complex.ImaginaryOne, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16, 17, 18, 19 - 3.45 * Complex.ImaginaryOne, 20, 21, 22, 23, 24, 25, 26 }, -1.43 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6 - 3.45 * Complex.ImaginaryOne, -5, -1, -2, -3, -4 }, 4, 3, 2, BLAS.MatrixTransposeState.Transpose, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(2, 3, 4, -2.25 * Complex.ImaginaryOne, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13 - 3.45 * Complex.ImaginaryOne, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1.65 + 3.1 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10 + 3.45 * Complex.ImaginaryOne, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 3, 2, BLAS.MatrixTransposeState.Transpose, BLAS.MatrixTransposeState.Hermite);
                yield return new TestCaseData(2, 3, 4, 3.5 - 3.5 * Complex.ImaginaryOne, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14 + 3.45 * Complex.ImaginaryOne, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, -0.75 + 6.87 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1 + 3.45 * Complex.ImaginaryOne, -2, -3 + 3.45 * Complex.ImaginaryOne, -4 }, 4, 3, 2, BLAS.MatrixTransposeState.Transpose, BLAS.MatrixTransposeState.Transpose);
            }
        }

        /// <summary>A test function that compares the result of 'ZGEMM' to the benchmark implementation.
        /// </summary>
        /// <param name="m">The number of rows of the matrix op(A) and of the matrix C.</param>
        /// <param name="n">The number of columns of the matrix op(B) and of the matrix C.</param>
        /// <param name="k">The number of columns of the matrix op(A) and the number of rows of the matrix op(B).</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The matrix A supplied column-by-column of dimension (<paramref name="lda"/>, ka), where ka is <paramref name="k"/> if op(A) = A; <paramref name="m"/> otherwise.</param>
        /// <param name="b">The matrix B supplied column-by-column of dimension (<paramref name="ldb"/>, kb), where kb is <paramref name="n"/> if op(B) = B; <paramref name="k"/> otherwise.</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="c">The matrix C supplied column-by-column of dimension (<paramref name="ldc"/>, <paramref name="n"/>).</param>
        /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="m"/>) if op(A) = A; max(1, <paramref name="k"/>) otherwise.</param>
        /// <param name="ldb">The leading dimension of <paramref name="b"/>, must be at least max(1,<paramref name="k"/>) if op(B) = B; max(1, <paramref name="n"/>) otherwise.</param>
        /// <param name="ldc">The leading dimension of <paramref name="c"/>, must be at least max(1, <paramref name="m"/>).</param>
        /// <param name="transposeA">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        /// <param name="transposeB">A value indicating whether 'op(B)=B' or 'op(B)=B^t'.</param>
        [TestCaseSource(nameof(TestCaseData_zgemm))]
        public void zgemm_TestCaseData_ResultOfBenchmarkImplementation(int m, int n, int k, Complex alpha, Complex[] a, Complex[] b, Complex beta, Complex[] c, int lda, int ldb, int ldc, BLAS.MatrixTransposeState transposeA = BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState transposeB = BLAS.MatrixTransposeState.NoTranspose)
        {
            var actual = c.ToArray();
            m_TestObject.zgemm(m, n, k, alpha, a, b, beta, actual, lda, ldb, ldc, transposeA, transposeB);

            var expected = c.ToArray();
            m_BenchmarkObject.zgemm(m, n, k, alpha, a, b, beta, expected, lda, ldb, ldc, transposeA, transposeB);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'ZGEMM'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_zgemm
        {
            get
            {
                // wrong input test case
//                yield return new TestCaseData(4, 4, 4, new Complex(1.0, 0.0), new Complex[] { 1, 2, 3, 4 }, new Complex[] { 11, 12, 13, 14 }, 1, new Complex[] { -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.NoTranspose).Throws(typeof(ArgumentException)).SetDescription("The size of the array is smaller than expected");

                // squared input matrices
                yield return new TestCaseData(4, 4, 4, 1.0 + 0.5 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4 + 3.45 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12 + 3.45 * Complex.ImaginaryOne, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1 + Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12 + 1.94 * Complex.ImaginaryOne, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3 - 3.45 * Complex.ImaginaryOne, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(4, 4, 4, new Complex(-2.25, 0), new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 - 3.45 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 - 3.45 * Complex.ImaginaryOne, 24, 25, 26 }, new Complex(1.65, 0.75), new Complex[] { -16, -15, -14 + 1.94 * Complex.ImaginaryOne, -13, -12, -11, -10, -9, -8, -7, -6 + 3.45 * Complex.ImaginaryOne, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(4, 4, 4, new Complex(3.5, 0), new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 - 3.45 * Complex.ImaginaryOne, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11 - 3.45 * Complex.ImaginaryOne, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, new Complex(-0.75, 2.65), new Complex[] { -16, -15, -14, -13, -12, -11, -10 - 3.45 * Complex.ImaginaryOne, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.NoTranspose);

                yield return new TestCaseData(4, 4, 4, new Complex(1.0, 0.5), new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 + 3.45 * Complex.ImaginaryOne, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 + 3.45 * Complex.ImaginaryOne, 24, 25, 26 }, 1 + 0.25 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12 + 3.45 * Complex.ImaginaryOne, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(4, 4, 4, new Complex(-2.25, -0.75), new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 + 3.45 * Complex.ImaginaryOne, 16 }, new Complex[] { 11, 12, 13, 14 - 3.45 * Complex.ImaginaryOne, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1.65 - 1.25 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11 + 3.45 * Complex.ImaginaryOne, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(4, 4, 4, new Complex(3.5, 1.5), new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13 + 3.45 * Complex.ImaginaryOne, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, -0.75 - 0.15 * Complex.ImaginaryOne, new Complex[] { -16, -15 + 3.45 * Complex.ImaginaryOne, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.Transpose);

                yield return new TestCaseData(4, 4, 4, new Complex(1.0, -1.0), new Complex[] { 1, 2, 3 - 3.45 * Complex.ImaginaryOne, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16, 17, 18 - 3.45 * Complex.ImaginaryOne, 19, 20, 21, 22, 23, 24, 25, 26 }, 1 + 3.65 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12 - 3.45 * Complex.ImaginaryOne, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.Transpose, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(4, 4, 4, new Complex(-2.25, 0.5), new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 + 3.45 * Complex.ImaginaryOne, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 + 3.45 * Complex.ImaginaryOne, 26 }, 1.65 + Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9, -8 + 3.45 * Complex.ImaginaryOne, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.Transpose, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(4, 4, 4, new Complex(3.5, -0.5), new Complex[] { 1 - 3.45 * Complex.ImaginaryOne, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16, 17 + 3.45 * Complex.ImaginaryOne, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, -0.75 + 2.54 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.Transpose, BLAS.MatrixTransposeState.NoTranspose);

                yield return new TestCaseData(4, 4, 4, new Complex(1.0, 1.25), new Complex[] { 1, 2 + 3.45 * Complex.ImaginaryOne, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16 + 3.45 * Complex.ImaginaryOne, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1 + 0 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12 - 3.45 * Complex.ImaginaryOne, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.Transpose, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(4, 4, 4, new Complex(-2.25, 1.75), new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 - 3.45 * Complex.ImaginaryOne, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 - 3.45 * Complex.ImaginaryOne, 24, 25, 26 }, 1.65 - 0.65 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12 + 3.45 * Complex.ImaginaryOne, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.Transpose, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(4, 4, 4, new Complex(3.5, -1.75), new Complex[] { 1, 2, 3, 4 + 3.45 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 - 3.45 * Complex.ImaginaryOne, 25, 26 }, -0.75 + 6.54 * Complex.ImaginaryOne, new Complex[] { -16, -15 + 3.45 * Complex.ImaginaryOne, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.Transpose, BLAS.MatrixTransposeState.Transpose);

                yield return new TestCaseData(4, 4, 4, new Complex(1.0, 1.25), new Complex[] { 1, 2 + 3.45 * Complex.ImaginaryOne, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16 + 3.45 * Complex.ImaginaryOne, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1 + 0 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12 - 3.45 * Complex.ImaginaryOne, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.Transpose, BLAS.MatrixTransposeState.Hermite);
                yield return new TestCaseData(4, 4, 4, new Complex(-2.25, 1.75), new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 - 3.45 * Complex.ImaginaryOne, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 - 3.45 * Complex.ImaginaryOne, 24, 25, 26 }, 1.65 - 0.65 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12 + 3.45 * Complex.ImaginaryOne, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.Hermite);
                yield return new TestCaseData(4, 4, 4, new Complex(3.5, -1.75), new Complex[] { 1, 2, 3, 4 + 3.45 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 - 3.45 * Complex.ImaginaryOne, 25, 26 }, -0.75 + 6.54 * Complex.ImaginaryOne, new Complex[] { -16, -15 + 3.45 * Complex.ImaginaryOne, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.Hermite, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(4, 4, 4, new Complex(3.5, -13.75), new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13 - 3.45 * Complex.ImaginaryOne, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, -0.75 + 6.54 * Complex.ImaginaryOne, new Complex[] { -16, -15 + 3.45 * Complex.ImaginaryOne, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.Hermite, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(4, 4, 4, new Complex(3.5, -1.75), new Complex[] { 1, 2, 3, 4 + 3.45 * Complex.ImaginaryOne, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 - 3.45 * Complex.ImaginaryOne, 25, 26 }, -0.75 + 6.54 * Complex.ImaginaryOne, new Complex[] { -16, -15 + 3.45 * Complex.ImaginaryOne, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.MatrixTransposeState.Hermite, BLAS.MatrixTransposeState.Hermite);

                // non-squared input matrices
                yield return new TestCaseData(2, 3, 4, new Complex(1.0, -0.5), new Complex[] { 1, 2, 3, 4 + 3.45, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 - 2.35, 21, 22, 23, 24, 25, 26 }, 1 + 3.87 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9, -8 + 1.94 * Complex.ImaginaryOne, -7, -6, -5, -1, -2, -3, -4 }, 2, 4, 2, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 3, 4, new Complex(-2.25, 0.75), new Complex[] { 1, 2 - 3.45 * Complex.ImaginaryOne, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16, 17 + 2.35 * Complex.ImaginaryOne, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1.65 - 2.65 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13 - 1.94 * Complex.ImaginaryOne, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 2, 4, 2, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 3, 4, new Complex(3.5, 1.25), new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 - 3.45 * Complex.ImaginaryOne, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14 - 2.35 * Complex.ImaginaryOne, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, -0.75 + 0.0541 * Complex.ImaginaryOne, new Complex[] { -16 + 1.94 * Complex.ImaginaryOne, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 2, 4, 2, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.NoTranspose);

                yield return new TestCaseData(2, 3, 4, new Complex(1.0, -1.25), new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1 + 1 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 2, 3, 2, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(2, 3, 4, new Complex(-2.25, 5.25), new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 - 3.45 * Complex.ImaginaryOne, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 + 2.35 * Complex.ImaginaryOne, 21, 22, 23, 24, 25, 26 }, 1.65 - 0.43 * Complex.ImaginaryOne, new Complex[] { -16, -15 + 1.94 * Complex.ImaginaryOne, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 2, 3, 2, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(2, 3, 4, new Complex(3.5, -4.25), new Complex[] { 1, 2, 3, 4, 5 + 3.45 * Complex.ImaginaryOne, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16, 17 - 2.35 * Complex.ImaginaryOne, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, -0.75 + 0.87 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10 - 1.94 * Complex.ImaginaryOne, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 2, 3, 2, BLAS.MatrixTransposeState.NoTranspose, BLAS.MatrixTransposeState.Hermite);

                yield return new TestCaseData(2, 3, 4, new Complex(1.0, 0.75), new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1 - 2.65 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 2, BLAS.MatrixTransposeState.Transpose, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 3, 4, new Complex(-2.25, 1.25), new Complex[] { 1, 2, 3 - 3.45 * Complex.ImaginaryOne, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16, 17, 18 - 2.35 * Complex.ImaginaryOne, 19, 20, 21, 22, 23, 24, 25, 26 }, 1.65 - 0.5 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14 + 1.94 * Complex.ImaginaryOne, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 2, BLAS.MatrixTransposeState.Transpose, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 3, 4, new Complex(3.5, -7.52), new Complex[] { 1 + 3.45 * Complex.ImaginaryOne, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12 - 2.35 * Complex.ImaginaryOne, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, -0.75 + 2.65 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9 - 1.94 * Complex.ImaginaryOne, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 2, BLAS.MatrixTransposeState.Hermite, BLAS.MatrixTransposeState.NoTranspose);

                yield return new TestCaseData(2, 3, 4, new Complex(1.0, 2.79), new Complex[] { 1, 2 - 3.45 * Complex.ImaginaryOne, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12 - 2.35 * Complex.ImaginaryOne, 13 + 2.35 * Complex.ImaginaryOne, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1 + 6.75 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14 + 1.94 * Complex.ImaginaryOne, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 3, 2, BLAS.MatrixTransposeState.Transpose, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(2, 3, 4, new Complex(1.0, 2.79), new Complex[] { 1, 2 - 1.45 * Complex.ImaginaryOne, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12 - 2.35 * Complex.ImaginaryOne, 13 + 2.35 * Complex.ImaginaryOne, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1 + 6.75 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14 + 1.94 * Complex.ImaginaryOne, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 3, 2, BLAS.MatrixTransposeState.Transpose, BLAS.MatrixTransposeState.Hermite);
                yield return new TestCaseData(2, 3, 4, new Complex(-2.25, 3.76), new Complex[] { 1, 2, 3, 4, 5, 6 + 3.45 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13 - 2.35 * Complex.ImaginaryOne, 14, 15, 16 + 2.35 * Complex.ImaginaryOne, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1.65 - 3.05 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11 - 1.94 * Complex.ImaginaryOne, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 3, 2, BLAS.MatrixTransposeState.Hermite, BLAS.MatrixTransposeState.Hermite);
                yield return new TestCaseData(2, 3, 4, new Complex(-2.25, 5.76), new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1.65 - 3.05 * Complex.ImaginaryOne, new Complex[] { -16, -15 + 0.5 * Complex.ImaginaryOne, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 3, 2, BLAS.MatrixTransposeState.Hermite, BLAS.MatrixTransposeState.Hermite);
                yield return new TestCaseData(2, 3, 4, new Complex(3.5, 0.87), new Complex[] { 1, 2, 3 - 3.45 * Complex.ImaginaryOne, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13 + 2.35 * Complex.ImaginaryOne, 14, 15, 16, 17 - 2.35 * Complex.ImaginaryOne, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, -0.75 - 0.75 * Complex.ImaginaryOne, new Complex[] { -16 - 1.94 * Complex.ImaginaryOne, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 3, 2, BLAS.MatrixTransposeState.Hermite, BLAS.MatrixTransposeState.Transpose);
            }
        }

        /// <summary>A test function that compares the result of 'ZHEMM' to the benchmark implementation.
        /// </summary>
        /// <param name="m">The number of rows of matrix C.</param>
        /// <param name="n">The number of columns of matrix C.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The Hermitian matrix A supplied column-by-column of dimension (<paramref name="ldc"/>, ka), where ka is <paramref name="m"/> if to calculate C := \alpha*A*B + \beta*C; <paramref name="n"/> otherwise.</param>
        /// <param name="b">The matrix B supplied column-by-column of dimension (<paramref name="ldb"/>, <paramref name="n"/>).</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="c">The matrix C supplied column-by-column of dimension (<paramref name="ldc"/>, <paramref name="n"/>).</param>
        /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="m"/>) if to calculate C := \alpha*A*B + \beta*C; max(1, <paramref name="n"/>) otherwise.</param>
        /// <param name="ldb">The leading dimension of <paramref name="b"/>, must be at least max(1,<paramref name="m"/>).</param>
        /// <param name="ldc">The leading dimension of <paramref name="c"/>, must be at least max(1, <paramref name="m"/>).</param>
        /// <param name="side">A value indicating whether to calculate C := \alpha*A*B + \beta*C or C := \alpha*B*A + \beta*C.</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        [TestCaseSource(nameof(TestCaseData_zhemm))]
        public void zhemm_TestCaseData_ResultOfBenchmarkImplementation(int m, int n, Complex alpha, Complex[] a, Complex[] b, Complex beta, Complex[] c, int lda, int ldb, int ldc, BLAS.Side side = BLAS.Side.Left, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix)
        {
            var actual = c.ToArray();
            m_TestObject.zhemm(m, n, alpha, a, b, beta, actual, lda, ldb, ldc, side, triangularMatrixType);

            var expected = c.ToArray();
            m_BenchmarkObject.zhemm(m, n, alpha, a, b, beta, expected, lda, ldb, ldc, side, triangularMatrixType);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'ZHEMM'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_zhemm
        {
            get
            {
                // squared input matrices
                yield return new TestCaseData(4, 4, 1.0 + 0.5 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8 - 2.65 * Complex.ImaginaryOne, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21 + 2.65 * Complex.ImaginaryOne, 22, 23, 24, 25, 26 }, 1 + Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9 + 2.65 * Complex.ImaginaryOne, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix);
                yield return new TestCaseData(4, 4, new Complex(-2.25, 0), new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16, 17 - 2.65 * Complex.ImaginaryOne, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, new Complex(1.65, 0.75), new Complex[] { -16, -15, -14, -13, -12 - 2.65 * Complex.ImaginaryOne, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix);
                yield return new TestCaseData(4, 4, new Complex(3.5, 0), new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13 - 2.65 * Complex.ImaginaryOne, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, new Complex(-0.75, 2.65), new Complex[] { -16, -15, -14, -13, -12, -11, -10 + 2.65 * Complex.ImaginaryOne, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.Side.Right, BLAS.TriangularMatrixType.UpperTriangularMatrix);

                yield return new TestCaseData(4, 4, new Complex(1.0, 0.5), new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 - 2.65 * Complex.ImaginaryOne, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12 - 2.65 * Complex.ImaginaryOne, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1 + 0.25 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13 + 2.65 * Complex.ImaginaryOne, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix);
                yield return new TestCaseData(4, 4, new Complex(-2.25, -0.75), new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8 + 2.65 * Complex.ImaginaryOne, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16, 17, 18 + 2.65 * Complex.ImaginaryOne, 19, 20, 21, 22, 23, 24, 25, 26 }, 1.65 - 1.25 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9, -8 + 2.65 * Complex.ImaginaryOne, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.Side.Left, BLAS.TriangularMatrixType.UpperTriangularMatrix);
                yield return new TestCaseData(4, 4, new Complex(3.5, 1.5), new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 - 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 11 - 2.65 * Complex.ImaginaryOne, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, -0.75 - 0.15 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9 - 2.65 * Complex.ImaginaryOne, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix);

                yield return new TestCaseData(4, 4, new Complex(1.0, -1.0), new Complex[] { 1, 2, 3, 4, 5.63 + 2.65 * Complex.ImaginaryOne, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12 + 2.65 * Complex.ImaginaryOne, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1 + 3.65 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7 + 2.65 * Complex.ImaginaryOne, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.Side.Right, BLAS.TriangularMatrixType.UpperTriangularMatrix);
                yield return new TestCaseData(4, 4, new Complex(-2.25, 0.5), new Complex[] { 1, 2, 3, 4, 5, 6, 7.76 + 51 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13 - 2.65 * Complex.ImaginaryOne, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1.65 + Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6 - 2.65 * Complex.ImaginaryOne, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix);
                yield return new TestCaseData(4, 4, new Complex(3.5, -0.5), new Complex[] { 1, 2 + 2.6 * Complex.ImaginaryOne, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15 + 2.65 * Complex.ImaginaryOne, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, -0.75 + 2.54 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10 + 2.65 * Complex.ImaginaryOne, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix);

                // non-squared input matrices       
                yield return new TestCaseData(2, 3, new Complex(1.0, -0.5), new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8 + 2.65 * Complex.ImaginaryOne, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12 - 2.65 * Complex.ImaginaryOne, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1 + 3.87 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13 - 2.65 * Complex.ImaginaryOne, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 2, 2, 2, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix);
                yield return new TestCaseData(2, 3, new Complex(-2.25, 0.75), new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.65 * Complex.ImaginaryOne, 16 }, new Complex[] { 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 1.65 - 2.65 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9, -8 + 2.65 * Complex.ImaginaryOne, -7, -6, -5, -1, -2, -3, -4 }, 2, 2, 2, BLAS.Side.Left, BLAS.TriangularMatrixType.UpperTriangularMatrix);
                yield return new TestCaseData(2, 3, new Complex(3.5, 1.25), new Complex[] { 1, 2 + 2.65 * Complex.ImaginaryOne, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11, 12, 13, 14, 15, 16, 17 - 2.65 * Complex.ImaginaryOne, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, -0.75 + 0.0541 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1 + 2.65 * Complex.ImaginaryOne, -2, -3, -4 }, 3, 2, 2, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix);
                yield return new TestCaseData(2, 3, new Complex(3.5, 1.25), new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 11 + 2.65 * Complex.ImaginaryOne, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, -0.75 + 0.0541 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11 + 2.65 * Complex.ImaginaryOne, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 3, 2, 2, BLAS.Side.Right, BLAS.TriangularMatrixType.UpperTriangularMatrix);
            }
        }

        /// <summary>A test function that compares the result of 'ZHERK' to the benchmark implementation.
        /// </summary>
        /// <param name="n">The order of matrix C.</param>
        /// <param name="k">The number of columns of matrix A if to calculate C := \alpha * A * A^h + \beta*C; otherwise the number of rows of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The matrix A supplied column-by-column of dimension (<paramref name="lda"/>, ka), where ka equals to <paramref name="k"/> if to calculate C := \alpha * A * A^h + \beta*C; <paramref name="n"/> otherwise.</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="c">The Hermitian matrix C supplied column-by-column of dimension (<paramref name="ldc"/>, <paramref name="n"/>).</param>
        /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="n"/>) if to calculate  C := \alpha * A * A^h + \beta*C ; max(1, <paramref name="k"/>) otherwise.</param>
        /// <param name="ldc">The leading dimension of <paramref name="c"/>, must be at least max(1, <paramref name="n"/>).</param>
        /// <param name="triangularMatrixType">A value whether matrix C is in its upper or lower triangular representation.</param>
        /// <param name="operation">A value indicating whether to calculate C := \alpha * A * A^h + \beta*C or C := alpha*A^h * A + beta*C.</param>        
        [TestCaseSource(nameof(TestCaseData_zherk))]
        public void zherk_TestCaseData_ResultOfBenchmarkImplementation(int n, int k, double alpha, Complex[] a, double beta, Complex[] c, int lda, int ldc, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.ZherkOperation operation = BLAS.ZherkOperation.ATimesAHermite)
        {
            var actual = c.ToArray();
            m_TestObject.zherk(n, k, alpha, a, beta, actual, lda, ldc, triangularMatrixType, operation);

            var expected = c.ToArray();
            m_BenchmarkObject.zherk(n, k, alpha, a, beta, expected, lda, ldc, triangularMatrixType, operation);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'ZHERK'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_zherk
        {
            get
            {
                // squared input matrices
                yield return new TestCaseData(4, 4, 0.5, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8 - 2.65 * Complex.ImaginaryOne, 9, 10, 11, 12, 13, 14, 15, 16 }, 1.64, new Complex[] { 11, 12, 13 - 2.65 * Complex.ImaginaryOne, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 4, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.ZherkOperation.AHermiteTimesA);
                yield return new TestCaseData(4, 4, -2.25, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 - 2.65 * Complex.ImaginaryOne, 15, 16 }, -0.75, new Complex[] { -16, -15, -14, -13 + 0.43 * Complex.ImaginaryOne, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.ZherkOperation.ATimesAHermite);
                yield return new TestCaseData(4, 4, 3.5, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, -0.83, new Complex[] { 11, 12, 13 - 2.65 * Complex.ImaginaryOne, 14, 15, 16, 17, 18, 19, 20, 21 - 0.43 * Complex.ImaginaryOne, 22, 23, 24, 25, 26 }, 4, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.ZherkOperation.ATimesAHermite);
                yield return new TestCaseData(4, 4, 3.5, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 - 2.65 * Complex.ImaginaryOne, 15, 16 }, -0.72, new Complex[] { -16, -15, -14, -13 + 0.43 * Complex.ImaginaryOne, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.ZherkOperation.AHermiteTimesA);

                // non-squared input matrices       
                yield return new TestCaseData(2, 3, -0.5, new Complex[] { 1, 2, 3, 4, 5 + 0.43 * Complex.ImaginaryOne, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 3.87, new Complex[] { -16, -15, -14, -13, -12 + 0.43 * Complex.ImaginaryOne, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 3, 2, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.ZherkOperation.AHermiteTimesA);
                yield return new TestCaseData(2, 3, -2.25, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8 - 2.65 * Complex.ImaginaryOne, 9, 10, 11, 12, 13, 14, 15, 16 }, 1.65, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9 - 0.43 * Complex.ImaginaryOne, -8, -7, -6, -5, -1, -2, -3, -4 }, 2, 2, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.ZherkOperation.ATimesAHermite);
                yield return new TestCaseData(2, 3, 3.5, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, -0.75, new Complex[] { 11 + 2.65 * Complex.ImaginaryOne, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 2, 2, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.ZherkOperation.ATimesAHermite);
                yield return new TestCaseData(2, 3, 1.25, new Complex[] { 1, 2 + 2.65 * Complex.ImaginaryOne, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, -0.0541, new Complex[] { -16, -15, -14, -13, -12, -11 + 0.43 * Complex.ImaginaryOne, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 3, 2, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.ZherkOperation.AHermiteTimesA);
            }
        }

        /// <summary>A test function that compares the result of 'ZHER2K' to the benchmark implementation.
        /// </summary>
        /// <param name="n">The order of matrix C.</param>
        /// <param name="k">The number of columns of matrix A if to calculate C := \alpha*A*B^h + conjg(\alpha)*B*A^h + \beta * C; the number of rows of the matrix A otherwise.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The matrix A supplied column-by-column of dimension (<paramref name="lda"/>, ka), where ka equals to <paramref name="k"/> if to calculate C := \alpha*A*B^h + conjg(\alpha)*B*A^h + \beta * C; <paramref name="n"/> otherwise.</param>
        /// <param name="b">The matrix B supplied column-by-column of dimension (<paramref name="ldb"/>, kb), where kb equals to <paramref name="k"/> if to calculate C := \alpha*A*B^h + conjg(\alpha)*B*A^h + \beta * C; <paramref name="n"/> otherwise.</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="c">The Hermitian matrix C supplied column-by-column of dimension (<paramref name="ldc"/>, <paramref name="n"/>).</param>
        /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="n"/>) if to calculate C := \alpha*A*B^h + conjg(\alpha)*B*A^h + \beta * C; max(1, <paramref name="k"/>) otherwise.</param>
        /// <param name="ldb">The leading dimension of <paramref name="b"/>, must be at least max(1,<paramref name="n"/>) if to calculate C := \alpha*A*B^h + conjg(\alpha)*B*A^h + \beta * C; max(1, <paramref name="k"/>) otherwise.</param>
        /// <param name="ldc">The leading dimension of <paramref name="c"/>, must be at least max(1, <paramref name="n"/>).</param>
        /// <param name="triangularMatrixType">A value whether matrix C is in its upper or lower triangular representation.</param>
        /// <param name="operation">A value indicating whether to calculate C := \alpha*A*B^h + conjg(\alpha)*B*A^h + \beta * C or C := \alpha*B^h*A + conjg(\alpha)*A^h*B + beta*C.</param>                
        [TestCaseSource(nameof(TestCaseData_zher2k))]
        public void zher2k_TestCaseData_ResultOfBenchmarkImplementation(int n, int k, Complex alpha, Complex[] a, Complex[] b, double beta, Complex[] c, int lda, int ldb, int ldc, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.Zher2kOperation operation = BLAS.Zher2kOperation.ATimesBHermitePlusBTimesAHermite)
        {
            var actual = c.ToArray();
            m_TestObject.zher2k(n, k, alpha, a, b, beta, actual, lda, ldb, ldc, triangularMatrixType, operation);

            var expected = c.ToArray();
            m_BenchmarkObject.zher2k(n, k, alpha, a, b, beta, expected, lda, ldb, ldc, triangularMatrixType, operation);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'ZHER2K'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_zher2k
        {
            get
            {
                // squared input matrices
                yield return new TestCaseData(4, 4, 0.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8 - 2.65 * Complex.ImaginaryOne, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 1.64, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9, -8 + 0.43 * Complex.ImaginaryOne, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.Zher2kOperation.ATimesBHermitePlusBTimesAHermite);
                yield return new TestCaseData(4, 4, -2.25 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, -0.75, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7 - 0.43 * Complex.ImaginaryOne, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.Zher2kOperation.BHermiteTimesAPlusAHermiteTimesB);
                yield return new TestCaseData(4, 4, 3.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, -0.83, new Complex[] { -16, -15, -14 + 0.43 * Complex.ImaginaryOne, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.Zher2kOperation.ATimesBHermitePlusBTimesAHermite);
                yield return new TestCaseData(4, 4, 3.5 - 1.2 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9 - 0.3 * Complex.ImaginaryOne, -8, -7, -6, -5, -1, -2, -3, -4 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 2.3, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, 4, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.Zher2kOperation.BHermiteTimesAPlusAHermiteTimesB);

                // non-squared input matrices       
                yield return new TestCaseData(2, 3, -0.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8 - 2.65 * Complex.ImaginaryOne, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 1.64, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9, -8 + 0.43 * Complex.ImaginaryOne, -7, -6, -5, -1, -2, -3, -4 }, 2, 2, 2, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.Zher2kOperation.ATimesBHermitePlusBTimesAHermite);
                yield return new TestCaseData(2, 3, -2.25 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, -0.75, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7 - 0.43 * Complex.ImaginaryOne, -6, -5, -1, -2, -3, -4 }, 3, 3, 2, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.Zher2kOperation.BHermiteTimesAPlusAHermiteTimesB);
                yield return new TestCaseData(2, 3, 3.5 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, -0.83, new Complex[] { -16, -15, -14 + 0.43 * Complex.ImaginaryOne, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 2, 2, 2, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.Zher2kOperation.ATimesBHermitePlusBTimesAHermite);
                yield return new TestCaseData(2, 3, 1.25 + 1.2 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9 - 0.3 * Complex.ImaginaryOne, -8, -7, -6, -5, -1, -2, -3, -4 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 2.3, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 3, 3, 2, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.Zher2kOperation.BHermiteTimesAPlusAHermiteTimesB);
            }
        }

        /// <summary>A test function that compares the result of 'ZSYMM' to the benchmark implementation.
        /// </summary>
        /// <param name="m">The number of rows of the matrix C.</param>
        /// <param name="n">The number of columns of the matrix C.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The symmetric matrix A supplied column-by-column of dimension (<paramref name="lda" />, ka), where ka is <paramref name="m" /> if to calculate C := \alpha * A*B + \beta*C; otherwise <paramref name="n" />.</param>
        /// <param name="b">The matrix B supplied column-by-column of dimension (<paramref name="ldb" />,<paramref name="n" />).</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="c">The matrix C supplied column-by-column of dimension (<paramref name="ldc" />,<paramref name="n" />); input/output.</param>
        /// <param name="lda">The leading dimension of <paramref name="a" />, must be at least max(1,<paramref name="m" />) if <paramref name="side" />=left; max(1,n) otherwise.</param>
        /// <param name="ldb">The leading dimension of <paramref name="b" />, must be at least max(1,<paramref name="m" />).</param>
        /// <param name="ldc">The leading dimension of <paramref name="c" />, must be at least max(1,<paramref name="m" />).</param>
        /// <param name="side">A value indicating whether to calculate C := \alpha * A*B + \beta*C or C := \alpha * B*A +\beta*C.</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        [TestCaseSource(nameof(TestCaseData_zsymm))]
        public void zsymm_TestCaseData_ResultOfBenchmarkImplementation(int m, int n, Complex alpha, Complex[] a, Complex[] b, Complex beta, Complex[] c, int lda, int ldb, int ldc, BLAS.Side side = BLAS.Side.Left, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix)
        {
            var actual = c.ToArray();
            m_TestObject.zsymm(m, n, alpha, a, b, beta, actual, lda, ldb, ldc, side, triangularMatrixType);

            var expected = c.ToArray();
            m_BenchmarkObject.zsymm(m, n, alpha, a, b, beta, expected, lda, ldb, ldc, side, triangularMatrixType);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'ZSYMM'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_zsymm
        {
            get
            {
                // squared input matrices
                yield return new TestCaseData(4, 4, 0.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8 - 2.65 * Complex.ImaginaryOne, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 1.64 + 2.2 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9, -8 + 0.43 * Complex.ImaginaryOne, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix);
                yield return new TestCaseData(4, 4, -2.25 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, -0.75 - 2.2 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7 - 0.43 * Complex.ImaginaryOne, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix);
                yield return new TestCaseData(4, 4, 3.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, -0.83 + 2.2 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14 + 0.43 * Complex.ImaginaryOne, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.Side.Left, BLAS.TriangularMatrixType.UpperTriangularMatrix);
                yield return new TestCaseData(4, 4, 3.5 - 1.2 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9 - 0.3 * Complex.ImaginaryOne, -8, -7, -6, -5, -1, -2, -3, -4 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 2.3 - 1.3 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, 4, 4, BLAS.Side.Right, BLAS.TriangularMatrixType.UpperTriangularMatrix);

                // non-squared input matrices       
                yield return new TestCaseData(2, 3, -0.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8 - 2.65 * Complex.ImaginaryOne, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 1.64 + 1.3 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9, -8 + 0.43 * Complex.ImaginaryOne, -7, -6, -5, -1, -2, -3, -4 }, 2, 2, 2, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix);
                yield return new TestCaseData(2, 3, -2.25 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, -0.75 - 1.3 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7 - 0.43 * Complex.ImaginaryOne, -6, -5, -1, -2, -3, -4 }, 3, 2, 2, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix);
                yield return new TestCaseData(2, 3, 3.5 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, -0.83 + 1.3 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14 + 0.43 * Complex.ImaginaryOne, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 2, 2, 2, BLAS.Side.Left, BLAS.TriangularMatrixType.UpperTriangularMatrix);
                yield return new TestCaseData(2, 3, 1.25 + 1.2 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9 - 0.3 * Complex.ImaginaryOne, -8, -7, -6, -5, -1, -2, -3, -4 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 2.3 - 1.3 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 3, 2, 2, BLAS.Side.Right, BLAS.TriangularMatrixType.UpperTriangularMatrix);
            }
        }

        /// <summary>A test function that compares the result of 'ZSYRK' to the benchmark implementation.
        /// </summary>
        /// <param name="n">The order of matrix C.</param>
        /// <param name="k">The number of columns of matrix A if to calculate C:= \alpha*A*A^t + \beta *C; otherwise the number of rows of matrix A.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The matrix A supplied column-by-column of dimension (<paramref name="lda" />, ka), where ka is <paramref name="k" /> if to calculate C:= \alpha*A*A^t + \beta *C; otherwise <paramref name="n" />.</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="c">The symmetric matrix C supplied column-by-column of dimension (<paramref name="ldc" />, <paramref name="n" />).</param>
        /// <param name="lda">The leading dimension of <paramref name="a" />, must be at least max(1,<paramref name="n" />) if to calculate C:= \alpha*A*A^t + \beta *C; max(1,<paramref name="k" />) otherwise.</param>
        /// <param name="ldc">The leading dimension of <paramref name="c" />, must be at least max(1,<paramref name="n" />).</param>
        /// <param name="triangularMatrixType">A value whether matrix C is in its upper or lower triangular representation.</param>
        /// <param name="operation">A value indicating whether to calculate C:= \alpha*A*A^t + \beta *C or C:= \alpha*A^t*A + \beta*C.</param>
        [TestCaseSource(nameof(TestCaseData_zsyrk))]
        public void zsyrk_TestCaseData_ResultOfBenchmarkImplementation(int n, int k, Complex alpha, Complex[] a, Complex beta, Complex[] c, int lda, int ldc, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.XsyrkOperation operation = BLAS.XsyrkOperation.ATimesATranspose)
        {
            var actual = c.ToArray();
            m_TestObject.zsyrk(n, k, alpha, a, beta, actual, lda, ldc, triangularMatrixType, operation);

            var expected = c.ToArray();
            m_BenchmarkObject.zsyrk(n, k, alpha, a, beta, expected, lda, ldc, triangularMatrixType, operation);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'ZSYRK'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_zsyrk
        {
            get
            {
                // squared input matrices
                yield return new TestCaseData(4, 4, 0.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 1.64 + 2.2 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9, -8 + 0.43 * Complex.ImaginaryOne, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.XsyrkOperation.ATimesATranspose);
                yield return new TestCaseData(4, 4, -2.25 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, -0.75 - 2.2 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7 - 0.43 * Complex.ImaginaryOne, -6, -5, -1, -2, -3, -4 }, 4, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.XsyrkOperation.AtransposeTimesA);
                yield return new TestCaseData(4, 4, 3.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, -0.83 + 2.2 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14 + 0.43 * Complex.ImaginaryOne, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.XsyrkOperation.ATimesATranspose);
                yield return new TestCaseData(4, 4, 3.5 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 2.3 - 1.3 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.XsyrkOperation.AtransposeTimesA);

                // non-squared input matrices       
                yield return new TestCaseData(2, 3, -0.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 1.64 + 1.3 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9, -8 + 0.43 * Complex.ImaginaryOne, -7, -6, -5, -1, -2, -3, -4 }, 2, 2, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.XsyrkOperation.ATimesATranspose);
                yield return new TestCaseData(2, 3, -2.25 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, -0.75 - 1.3 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7 - 0.43 * Complex.ImaginaryOne, -6, -5, -1, -2, -3, -4 }, 3, 2, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.XsyrkOperation.AtransposeTimesA);
                yield return new TestCaseData(2, 3, 3.5 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, -0.83 + 1.3 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14 + 0.43 * Complex.ImaginaryOne, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 2, 2, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.XsyrkOperation.ATimesATranspose);
                yield return new TestCaseData(2, 3, 1.25 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 2.3 - 1.3 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 3, 2, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.XsyrkOperation.AtransposeTimesA);
            }
        }

        /// <summary>A test function that compares the result of 'ZSYR2K' to the benchmark implementation.
        /// </summary>
        /// <param name="n">The order of matrix C.</param>
        /// <param name="k">The The number of columns of matrices A and B or the number .</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The matrix A supplied column-by-column of dimension (<paramref name="lda" />, ka), where ka is <paramref name="k" /> if to calculate C := alpha*A*B^t + alpha*B*A^t + beta*C; otherwise <paramref name="n" />.</param>
        /// <param name="b">The matrix B supplied column-by-column of dimension (<paramref name="ldb" />, kb), where ka is at least max(1,<paramref name="n" />) if to calculate C := alpha*A*B^t + alpha*B*A^t + beta*C; otherwise at least max(1,<paramref name="k" />).</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="c">The symmetric matrix C supplied column-by-column of dimension (<paramref name="ldc" />, <paramref name="n" />).</param>
        /// <param name="lda">The leading dimension of <paramref name="a" />, must be at least max(1,<paramref name="n" />) if to calculate C:= alpha*A*B^t+alpha*B*A^t+beta*C; max(1,<paramref name="k" />) otherwise.</param>
        /// <param name="ldb">The leading dimension of <paramref name="b" />, must be at least max(1,<paramref name="n" />) if to calculate C:= alpha*A*B^t+alpha*B*A^t+beta*C; max(1,<paramref name="k" />) otherwise.</param>
        /// <param name="ldc">The leading dimension of <paramref name="c" />, must be at least max(1,<paramref name="n" />).</param>
        /// <param name="triangularMatrixType">A value whether matrix C is in its upper or lower triangular representation.</param>
        /// <param name="operation">A value indicating whether to calculate C := alpha*A*B^t + alpha*B*A^t + beta*C or C := alpha*A^t*B + alpha*B^t*A + beta*C.</param>
        [TestCaseSource(nameof(TestCaseData_zsyr2k))]
        public void zsyr2k_TestCaseData_ResultOfBenchmarkImplementation(int n, int k, Complex alpha, Complex[] a, Complex[] b, Complex beta, Complex[] c, int lda, int ldb, int ldc, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.Xsyr2kOperation operation = BLAS.Xsyr2kOperation.ATimesBTransPlusBTimesATrans)
        {
            var actual = c.ToArray();
            m_TestObject.zsyr2k(n, k, alpha, a, b, beta, actual, lda, ldb, ldc, triangularMatrixType, operation);

            var expected = c.ToArray();
            m_BenchmarkObject.zsyr2k(n, k, alpha, a, b, beta, expected, lda, ldb, ldc, triangularMatrixType, operation);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'ZSYR2K'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_zsyr2k
        {
            get
            {
                // squared input matrices
                yield return new TestCaseData(4, 4, 0.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8 - 2.65 * Complex.ImaginaryOne, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 1.64 + 2.2 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9, -8 + 0.43 * Complex.ImaginaryOne, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.Xsyr2kOperation.ATimesBTransPlusBTimesATrans);
                yield return new TestCaseData(4, 4, -2.25 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, -0.75 - 2.2 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7 - 0.43 * Complex.ImaginaryOne, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.Xsyr2kOperation.ATransTimesBPlusBTransTimesA);
                yield return new TestCaseData(4, 4, 3.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, -0.83 + 2.2 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14 + 0.43 * Complex.ImaginaryOne, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 4, 4, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.Xsyr2kOperation.ATimesBTransPlusBTimesATrans);
                yield return new TestCaseData(4, 4, 3.5 - 1.2 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9 - 0.3 * Complex.ImaginaryOne, -8, -7, -6, -5, -1, -2, -3, -4 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 2.3 - 1.3 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, 4, 4, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.Xsyr2kOperation.ATransTimesBPlusBTransTimesA);

                // non-squared input matrices       
                yield return new TestCaseData(2, 3, -0.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8 - 2.65 * Complex.ImaginaryOne, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 1.64 + 1.3 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9, -8 + 0.43 * Complex.ImaginaryOne, -7, -6, -5, -1, -2, -3, -4 }, 2, 2, 2, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.Xsyr2kOperation.ATimesBTransPlusBTimesATrans);
                yield return new TestCaseData(2, 3, -2.25 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, -0.75 - 1.3 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9, -8, -7 - 0.43 * Complex.ImaginaryOne, -6, -5, -1, -2, -3, -4 }, 3, 3, 2, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.Xsyr2kOperation.ATransTimesBPlusBTransTimesA);
                yield return new TestCaseData(2, 3, 3.5 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, -0.83 + 1.3 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14 + 0.43 * Complex.ImaginaryOne, -13, -12, -11, -10, -9, -8, -7, -6, -5, -1, -2, -3, -4 }, 2, 2, 2, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.Xsyr2kOperation.ATimesBTransPlusBTimesATrans);
                yield return new TestCaseData(2, 3, 1.25 + 1.2 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9 - 0.3 * Complex.ImaginaryOne, -8, -7, -6, -5, -1, -2, -3, -4 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 2.3 - 1.3 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 3, 3, 2, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.Xsyr2kOperation.ATransTimesBPlusBTransTimesA);
            }
        }

        /// <summary>A test function that compares the result of 'ZTRMM' to the benchmark implementation.
        /// </summary>
        /// <param name="m">The number of rows of matrix B.</param>
        /// <param name="n">The number of columns of matrix B.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The triangular matrix A supplied column-by-column of dimension (<paramref name="lda" />, k), where k is <paramref name="m" /> if to calculate B := \alpha * op(A)*B; <paramref name="n" /> otherwise.</param>
        /// <param name="b">The matrix B supplied column-by-column of dimension (<paramref name="ldb" />, <paramref name="n" />).</param>
        /// <param name="lda">The leading dimension of <paramref name="a" />, must be at least max(1,<paramref name="m" />) if to calculate B := \alpha * op(A)*B; max(1,<paramref name="n" />) otherwise.</param>
        /// <param name="ldb">The leading dimension of <paramref name="b" />, must be at least max(1,<paramref name="m" />).</param>
        /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
        /// <param name="side">A value indicating whether to calculate B := \alpha * op(A)*B or B:= \alpha *B * op(A).</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        [TestCaseSource(nameof(TestCaseData_ztrmm))]
        public void ztrmm_TestCaseData_ResultOfBenchmarkImplementation(int m, int n, Complex alpha, Complex[] a, Complex[] b, int lda, int ldb, bool isUnitTriangular = true, BLAS.Side side = BLAS.Side.Left, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose)
        {
            var actual = b.ToArray();
            m_TestObject.ztrmm(m, n, alpha, a, actual, lda, ldb, isUnitTriangular, side, triangularMatrixType, transpose);

            var expected = b.ToArray();
            m_BenchmarkObject.ztrmm(m, n, alpha, a, expected, lda, ldb, isUnitTriangular, side, triangularMatrixType, transpose);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'ZTRMM'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_ztrmm
        {
            get
            {
                // squared input matrices
                yield return new TestCaseData(4, 4, 0.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8 - 2.65 * Complex.ImaginaryOne, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, 4, true, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(4, 4, -2.25 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, 4, true, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(4, 4, 3.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, 4, 4, true, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Hermite);
                yield return new TestCaseData(4, 4, 3.5 - 1.2 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9 - 0.3 * Complex.ImaginaryOne, -8, -7, -6, -5, -1, -2, -3, -4 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, 4, true, BLAS.Side.Left, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(4, 4, -2.25 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, 4, true, BLAS.Side.Left, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(4, 4, 3.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, 4, 4, true, BLAS.Side.Left, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Hermite);

                yield return new TestCaseData(4, 4, 0.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8 - 2.65 * Complex.ImaginaryOne, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, 4, true, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(4, 4, -2.25 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, 4, true, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(4, 4, 3.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, 4, 4, true, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Hermite);
                yield return new TestCaseData(4, 4, 3.5 - 1.2 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9 - 0.3 * Complex.ImaginaryOne, -8, -7, -6, -5, -1, -2, -3, -4 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, 4, true, BLAS.Side.Right, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(4, 4, -2.25 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, 4, true, BLAS.Side.Right, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(4, 4, 3.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, 4, 4, true, BLAS.Side.Right, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Hermite);


                yield return new TestCaseData(4, 4, 0.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8 - 2.65 * Complex.ImaginaryOne, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, 4, false, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(4, 4, -2.25 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, 4, false, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(4, 4, 3.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, 4, 4, false, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Hermite);
                yield return new TestCaseData(4, 4, 3.5 - 1.2 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9 - 0.3 * Complex.ImaginaryOne, -8, -7, -6, -5, -1, -2, -3, -4 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, 4, false, BLAS.Side.Left, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(4, 4, -2.25 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, 4, false, BLAS.Side.Left, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(4, 4, 3.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, 4, 4, false, BLAS.Side.Left, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Hermite);

                yield return new TestCaseData(4, 4, 0.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8 - 2.65 * Complex.ImaginaryOne, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, 4, false, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(4, 4, -2.25 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, 4, false, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(4, 4, 3.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, 4, 4, false, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Hermite);
                yield return new TestCaseData(4, 4, 3.5 - 1.2 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9 - 0.3 * Complex.ImaginaryOne, -8, -7, -6, -5, -1, -2, -3, -4 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, 4, false, BLAS.Side.Right, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(4, 4, -2.25 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, 4, false, BLAS.Side.Right, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(4, 4, 3.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, 4, 4, false, BLAS.Side.Right, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Hermite);


                // non-squared input matrices       
                yield return new TestCaseData(2, 3, 0.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8 - 2.65 * Complex.ImaginaryOne, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 2, 2, true, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 3, -2.25 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 2, 2, true, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(2, 3, 3.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, 4, 4, true, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Hermite);
                yield return new TestCaseData(2, 3, 3.5 - 1.2 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9 - 0.3 * Complex.ImaginaryOne, -8, -7, -6, -5, -1, -2, -3, -4 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 2, 2, true, BLAS.Side.Left, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 3, -2.25 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 2, 2, true, BLAS.Side.Left, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(2, 3, 3.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, 2, 2, true, BLAS.Side.Left, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Hermite);

                yield return new TestCaseData(2, 3, 0.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8 - 2.65 * Complex.ImaginaryOne, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 3, 2, true, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 3, -2.25 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 3, 2, true, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(2, 3, 3.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, 3, 2, true, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Hermite);
                yield return new TestCaseData(2, 3, 3.5 - 1.2 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9 - 0.3 * Complex.ImaginaryOne, -8, -7, -6, -5, -1, -2, -3, -4 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 3, 2, true, BLAS.Side.Right, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 3, -2.25 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 3, 2, true, BLAS.Side.Right, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(2, 3, 3.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, 3, 2, true, BLAS.Side.Right, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Hermite);

                yield return new TestCaseData(2, 3, 0.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8 - 2.65 * Complex.ImaginaryOne, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 2, 2, false, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 3, -2.25 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 2, 2, false, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(2, 3, 3.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, 4, 4, false, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Hermite);
                yield return new TestCaseData(2, 3, 3.5 - 1.2 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9 - 0.3 * Complex.ImaginaryOne, -8, -7, -6, -5, -1, -2, -3, -4 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 2, 2, false, BLAS.Side.Left, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 3, -2.25 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 2, 2, false, BLAS.Side.Left, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(2, 3, 3.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, 2, 2, false, BLAS.Side.Left, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Hermite);

                yield return new TestCaseData(2, 3, 0.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8 - 2.65 * Complex.ImaginaryOne, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 3, 2, false, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 3, -2.25 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 3, 2, false, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(2, 3, 3.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, 3, 2, false, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Hermite);
                yield return new TestCaseData(2, 3, 3.5 - 1.2 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9 - 0.3 * Complex.ImaginaryOne, -8, -7, -6, -5, -1, -2, -3, -4 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 3, 2, false, BLAS.Side.Right, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 3, -2.25 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 3, 2, false, BLAS.Side.Right, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(2, 3, 3.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, 3, 2, false, BLAS.Side.Right, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Hermite);
            }
        }

        /// <summary>A test function that compares the result of 'ZTRSM' to the benchmark implementation.
        /// </summary>
        /// <param name="m">The number of rows of matrix B.</param>
        /// <param name="n">The number of column of matrix B.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The triangular matrix A supplied column-by-column of dimension (<paramref name="lda" />, k), where k is <paramref name="m" /> if to calculate op(A) * X = \alpha * B; <paramref name="n" /> otherwise.</param>
        /// <param name="b">The matrix B supplied column-by-column of dimension (<paramref name="ldb" />, <paramref name="n" />).</param>
        /// <param name="lda">The leading dimension of <paramref name="a" />, must be at least max(1,<paramref name="m" />) if to calculate op(A) * X = \alpha * B; max(1,<paramref name="n" />) otherwise.</param>
        /// <param name="ldb">The leading dimension of <paramref name="b" />, must be at least max(1,<paramref name="m" />).</param>
        /// <param name="isUnitTriangular">A value indicating whether the matrix A is unit triangular.</param>
        /// <param name="side">A value indicating whether to calculate op(A) * X = \alpha * B or X * op(A) = \alpha *B.</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        /// <param name="transpose">A value indicating whether 'op(A)=A' or 'op(A)=A^t'.</param>
        [TestCaseSource(nameof(TestCaseData_ztrsm))]
        public void ztrsm_TestCaseData_ResultOfBenchmarkImplementation(int m, int n, Complex alpha, Complex[] a, Complex[] b, int lda, int ldb, bool isUnitTriangular = true, BLAS.Side side = BLAS.Side.Left, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState transpose = BLAS.MatrixTransposeState.NoTranspose)
        {
            var actual = b.ToArray();
            m_TestObject.ztrsm(m, n, alpha, a, actual, lda, ldb, isUnitTriangular, side, triangularMatrixType, transpose);

            var expected = b.ToArray();
            m_BenchmarkObject.ztrsm(m, n, alpha, a, expected, lda, ldb, isUnitTriangular, side, triangularMatrixType, transpose);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects for the test of the BLAS method 'ZTRSM'.
        /// </summary>
        /// <value>The test case data.</value>
        public static IEnumerable<TestCaseData> TestCaseData_ztrsm
        {
            get
            {
                // squared input matrices
                yield return new TestCaseData(4, 4, 0.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8 - 2.65 * Complex.ImaginaryOne, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, 4, true, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(4, 4, -2.25 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, 4, true, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(4, 4, 3.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, 4, 4, true, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Hermite);
                yield return new TestCaseData(4, 4, 3.5 - 1.2 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9 - 0.3 * Complex.ImaginaryOne, -8, -7, -6, -5, -1, -2, -3, -4 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, 4, true, BLAS.Side.Left, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(4, 4, -2.25 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, 4, true, BLAS.Side.Left, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(4, 4, 3.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, 4, 4, true, BLAS.Side.Left, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Hermite);

                yield return new TestCaseData(4, 4, 0.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8 - 2.65 * Complex.ImaginaryOne, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, 4, true, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(4, 4, -2.25 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, 4, true, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(4, 4, 3.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, 4, 4, true, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Hermite);
                yield return new TestCaseData(4, 4, 3.5 - 1.2 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9 - 0.3 * Complex.ImaginaryOne, -8, -7, -6, -5, -1, -2, -3, -4 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, 4, true, BLAS.Side.Right, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(4, 4, -2.25 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, 4, true, BLAS.Side.Right, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(4, 4, 3.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, 4, 4, true, BLAS.Side.Right, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Hermite);


                yield return new TestCaseData(4, 4, 0.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8 - 2.65 * Complex.ImaginaryOne, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, 4, false, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(4, 4, -2.25 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, 4, false, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(4, 4, 3.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, 4, 4, false, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Hermite);
                yield return new TestCaseData(4, 4, 3.5 - 1.2 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9 - 0.3 * Complex.ImaginaryOne, -8, -7, -6, -5, -1, -2, -3, -4 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, 4, false, BLAS.Side.Left, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(4, 4, -2.25 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, 4, false, BLAS.Side.Left, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(4, 4, 3.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, 4, 4, false, BLAS.Side.Left, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Hermite);

                yield return new TestCaseData(4, 4, 0.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8 - 2.65 * Complex.ImaginaryOne, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, 4, false, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(4, 4, -2.25 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, 4, false, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(4, 4, 3.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, 4, 4, false, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Hermite);
                yield return new TestCaseData(4, 4, 3.5 - 1.2 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9 - 0.3 * Complex.ImaginaryOne, -8, -7, -6, -5, -1, -2, -3, -4 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, 4, false, BLAS.Side.Right, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(4, 4, -2.25 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, 4, false, BLAS.Side.Right, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(4, 4, 3.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, 4, 4, false, BLAS.Side.Right, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Hermite);


                // non-squared input matrices       
                yield return new TestCaseData(2, 3, 0.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8 - 2.65 * Complex.ImaginaryOne, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 2, 2, true, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 3, -2.25 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 2, 2, true, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(2, 3, 3.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, 4, 4, true, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Hermite);
                yield return new TestCaseData(2, 3, 3.5 - 1.2 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9 - 0.3 * Complex.ImaginaryOne, -8, -7, -6, -5, -1, -2, -3, -4 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 2, 2, true, BLAS.Side.Left, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 3, -2.25 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 2, 2, true, BLAS.Side.Left, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(2, 3, 3.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, 2, 2, true, BLAS.Side.Left, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Hermite);

                yield return new TestCaseData(2, 3, 0.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8 - 2.65 * Complex.ImaginaryOne, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 3, 2, true, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 3, -2.25 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 3, 2, true, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(2, 3, 3.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, 3, 2, true, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Hermite);
                yield return new TestCaseData(2, 3, 3.5 - 1.2 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9 - 0.3 * Complex.ImaginaryOne, -8, -7, -6, -5, -1, -2, -3, -4 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 3, 2, true, BLAS.Side.Right, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 3, -2.25 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 3, 2, true, BLAS.Side.Right, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(2, 3, 3.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, 3, 2, true, BLAS.Side.Right, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Hermite);

                yield return new TestCaseData(2, 3, 0.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8 - 2.65 * Complex.ImaginaryOne, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 2, 2, false, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 3, -2.25 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 2, 2, false, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(2, 3, 3.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, 4, 4, false, BLAS.Side.Left, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Hermite);
                yield return new TestCaseData(2, 3, 3.5 - 1.2 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9 - 0.3 * Complex.ImaginaryOne, -8, -7, -6, -5, -1, -2, -3, -4 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 2, 2, false, BLAS.Side.Left, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 3, -2.25 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 2, 2, false, BLAS.Side.Left, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(2, 3, 3.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, 2, 2, false, BLAS.Side.Left, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Hermite);

                yield return new TestCaseData(2, 3, 0.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8 - 2.65 * Complex.ImaginaryOne, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 3, 2, false, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 3, -2.25 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 3, 2, false, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(2, 3, 3.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, 3, 2, false, BLAS.Side.Right, BLAS.TriangularMatrixType.LowerTriangularMatrix, BLAS.MatrixTransposeState.Hermite);
                yield return new TestCaseData(2, 3, 3.5 - 1.2 * Complex.ImaginaryOne, new Complex[] { -16, -15, -14, -13, -12, -11, -10, -9 - 0.3 * Complex.ImaginaryOne, -8, -7, -6, -5, -1, -2, -3, -4 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 3, 2, false, BLAS.Side.Right, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.NoTranspose);
                yield return new TestCaseData(2, 3, -2.25 - 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 + 2.65 * Complex.ImaginaryOne, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 3, 2, false, BLAS.Side.Right, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Transpose);
                yield return new TestCaseData(2, 3, 3.5 + 1.2 * Complex.ImaginaryOne, new Complex[] { 1, 2, 3, 4, 5, 6, 7 + 2.65 * Complex.ImaginaryOne, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, new Complex[] { 1, 2, 3, 4 - 2.65 * Complex.ImaginaryOne, 5, 6 + 2.65 * Complex.ImaginaryOne, 7, 8, 9, 10, 11, 12, 13, 14, 15 - 2.4 * Complex.ImaginaryOne, 16 }, 3, 2, false, BLAS.Side.Right, BLAS.TriangularMatrixType.UpperTriangularMatrix, BLAS.MatrixTransposeState.Hermite);
            }
        }

        /// <summary>A test function for <c>aux_zgetrans</c>.
        /// </summary>
        /// <param name="rowCount">The number of rows.</param>
        /// <param name="columnCount">The number of columns.</param>
        /// <param name="a">The matrix to transpose (column-major ordering).</param>
        /// <param name="expected">The expected outcome.</param>
        [TestCase(2, 2, new double[] { 1, 2, 3, 4 }, new double[] { 1, 3, 2, 4 })]
        [TestCase(3, 3, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, new double[] { 1, 4, 7, 2, 5, 8, 3, 6, 9 })]
        [TestCase(2, 3, new double[] { 1, 2, 3, 4, 5, 6 }, new double[] { 1, 3, 5, 2, 4, 6 })]
        [TestCase(3, 2, new double[] { 1, 2, 3, 4, 5, 6 }, new double[] { 1, 4, 2, 5, 3, 6 })]
        public void aux_zgeTrans(int rowCount, int columnCount, double[] a, double[] expected)
        {
            var actual =  a.Select(x => new Complex(x,0)).ToArray();
            
            m_TestObject.aux_zgetrans(rowCount, columnCount, actual);
            Assert.That(actual, new ComplexArrayNUnitConstraint(expected.Select(x => new Complex(x, 0)).ToArray(), tolerance: 1E-9));
        }
        #endregion

        #endregion

        #region protected methods

        /// <summary>Gets the level 3 BLAS implementation.
        /// </summary>
        /// <returns>A <see cref="ILevel3BLAS"/> object that encapuslate the level 3 BLAS functions.</returns>
        protected abstract ILevel3BLAS GetLevel3BLAS();
        #endregion
    }
}