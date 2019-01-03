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
    /// <summary>Serves as abstract unit test class for Level 1 BLAS methods.
    /// </summary>
    public abstract class Level1BLASTests
    {
        #region private members

        /// <summary>The Level 1 BLAS implementation to test.
        /// </summary>
        private ILevel1BLAS m_Level1BLAS;

        /// <summary>A benchmark Level 1 BLAS implementation.
        /// </summary>
        private ILevel1BLAS m_BenchmarkLevel1BLAS = new BuildInLevel1BLAS();
        #endregion

        #region protected constructors

        /// <summary>Initializes a new instance of the <see cref="Level1BLASTests"/> class.
        /// </summary>
        protected Level1BLASTests()
        {
            m_Level1BLAS = GetLevel1BLAS();
        }
        #endregion

        #region public methods

        #region double precision methods

        /// <summary>A test function that compares the result of 'DASUM' to the benchmark implementation.
        /// </summary>
        /// <param name="n">The number of elements in <paramref name="x"/>.</param>
        /// <param name="incX">The increment for <paramref name="x"/>.</param>
        /// <param name="x">The vector 'x'.</param>
        [TestCase(1, 1, 2.0)]
        [TestCase(2, 1, 1.0, 3.5)]
        public void dasum_TestCaseData_ResultOfBenchmarkImplementation(int n, int incX, params double[] x)
        {
            double actual = m_Level1BLAS.dasum(n, x, incX);
            double expected = m_BenchmarkLevel1BLAS.dasum(n, x, incX);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-9));
        }

        /// <summary>A test function for 'DAXPY' with respect to a specific test case data.
        /// </summary>
        [Test]
        public void daxpy_TestCaseData_ManuallyBenchmark()
        {
            int n = 3;
            double a = 2.5;
            double[] x = new double[] { 1.0, 2.0, 3.0 };
            double[] y = new double[] { 3.0, 2.0, 1.0 };

            m_Level1BLAS.daxpy(n, a, x, y);

            Assert.That(y[0], Is.EqualTo(a * x[0] + 3.0));
            Assert.That(y[1], Is.EqualTo(a * x[1] + 2.0));
            Assert.That(y[2], Is.EqualTo(a * x[2] + 1.0));
        }

        /// <summary>A test function for 'DAXPY' (with extended parameters) with respect to a specific test case data.
        /// </summary>
        [Test]
        public void daxpy_ExtendedTestCaseData_ResultOfManuallyBenchmark()
        {
            int n = 3;
            double a = -2.6;
            int incX = 2;
            int startIndexX = 1;

            int incY = 3;
            int startIndexY = 2;
            double[] x = new double[] { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0 };
            double[] y = new double[] { 9.0, 8.0, 7.0, 6.0, 5.0, 4.0, 3.0, 2.0, 1.0 };

            m_Level1BLAS.daxpy(n, a, x, y, incX, incY, startIndexX, startIndexY);

            Assert.That(y[0], Is.EqualTo(9.0));
            Assert.That(y[1], Is.EqualTo(8.0));
            Assert.That(y[2], Is.EqualTo(a * x[startIndexX] + 7.0));
            Assert.That(y[3], Is.EqualTo(6.0));
            Assert.That(y[4], Is.EqualTo(5.0));
            Assert.That(y[5], Is.EqualTo(a * x[startIndexX + 1 * incX] + 4.0));
            Assert.That(y[6], Is.EqualTo(3.0));
            Assert.That(y[7], Is.EqualTo(2.0));
            Assert.That(y[8], Is.EqualTo(a * x[startIndexX + 2 * incX] + 1.0));
        }

        /// <summary>A test function for 'DCOPY' with respect to a specific test case data.
        /// </summary>
        /// <param name="n">The number of elements in <paramref name="x"/>.</param>
        /// <param name="x">The vector x.</param>
        [TestCase(2, 1.0, 4.5)]
        [TestCase(5, 1.0, 2.0, 3.0, 4.0, 5.0)]
        public void dcopy_TestCaseData_SequenceEqualArray(int n, params double[] x)
        {
            double[] y = new double[n];
            m_Level1BLAS.dcopy(n, x, y);

            Assert.That(y.SequenceEqual(x));
        }

        /// <summary>A test function that compares the result of 'DCOPY' to the benchmark implementation.
        /// </summary>
        /// <param name="n">The number of elements in <paramref name="x"/>.</param>
        /// <param name="startIndexX">The null-based start index of <paramref name="x"/>.</param>
        /// <param name="startIndexY">The null-based start index of y.</param>
        /// <param name="incX">The increment for <paramref name="x"/>.</param>
        /// <param name="incY">The increment for y.</param>
        /// <param name="x">The vector x.</param>
        [TestCase(2, 0, 0, 1, 1, 24.0, 48.0)]
        [TestCase(2, 1, 0, 1, 2, -12.0, 8.0, 1.45)]
        public void dcopy_ExtendedTestCaseData_SequenceEqualArray(int n, int startIndexX, int startIndexY, int incX, int incY, params double[] x)
        {
            double[] y = new double[startIndexY + incY * n];

            m_Level1BLAS.dcopy(n, x, y, incX, incY, startIndexX, startIndexY);

            var sparseY = y.Where((z, i) => { return (i >= startIndexY) && (((i - startIndexY) % incY) == 0); });
            var sparseX = x.Where((z, i) => { return (i >= startIndexX) && (((i - startIndexX) % incX) == 0); });

            Assert.That(sparseY.SequenceEqual(sparseX));
        }

        /// <summary>A test function that compares the result of 'DDOT' to the benchmark implementation.
        /// </summary>
        [Test]
        public void ddot_TestCaseData_ResultOfBenchmarkImplementation()
        {
            int n = 4;
            double[] x = new double[] { 1, 2, 3, 4 };
            double[] y = new double[] { 1, -2, -3, 4 };
            double actual = m_Level1BLAS.ddot(n, x, y);

            double expected = m_BenchmarkLevel1BLAS.ddot(n, x, y);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-9));
        }

        /// <summary>A test function that compares the result of 'DCOPY' (with extended parameters) to the benchmark implementation.
        /// </summary>
        [Test]
        public void ddot_ExtendedTestCaseData_ResultOfBenchmarkImplementation()
        {
            int n = 3;
            int startIndexX = 1;
            int incX = 2;

            int startIndexY = 2;
            int incY = 3;

            double[] x = new double[] { 1, 2, 3, 4, 5, 6 };
            double[] y = new double[] { -2, -3, -5, -23, 4, 7, 8, 9, -42 };

            double actual = m_Level1BLAS.ddot(n, x, y, incX, incY, startIndexX, startIndexY);
            double expected = m_BenchmarkLevel1BLAS.ddot(n, x, y, incX, incY, startIndexX, startIndexY);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-9));
        }

        /// <summary>A test function that compares the result of 'DNRM2' to the benchmark implementation.
        /// </summary>
        /// <param name="n">The number of elements in <paramref name="x"/>.</param>
        /// <param name="incX">The increment for <paramref name="x"/>.</param>
        /// <param name="x">The vector x.</param>
        [TestCase(2, 1, 1.0, -9.2)]
        [TestCase(3, 2, -8.1, 5.6, -77.12, 942.42, 615.12, -0.77171)]
        public void dnrm2_TestCaseData_ResultOfBenchmarkImplementation(int n, int incX, params double[] x)
        {
            double actual = m_Level1BLAS.dnrm2(n, x, incX);
            double expected = m_BenchmarkLevel1BLAS.dnrm2(n, x, incX);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-9));
        }

        /// <summary>A test function for 'DROT' with respect to a specific test case data.
        /// </summary>
        [Test]
        public void drot_TestCaseData_ResultOfManuallyBenchmark()
        {
            int n = 4;
            int incX = 1;
            int incY = 2;
            double c = 2.5;
            double s = -1.25;
            double[] x = new double[] { 1, 2, 3, 4 };
            double[] y = new double[] { 9, 8, 7, 6, 5, 4, 3 };

            m_Level1BLAS.drot(n, x, y, c, s, incX, incY);

            Assert.That(x[0], Is.EqualTo(c * 1.0 + s * 9.0), String.Format("Index (x): {0}", 0));
            Assert.That(x[1], Is.EqualTo(c * 2.0 + s * 7.0), String.Format("Index (x): {0}", 1));
            Assert.That(x[2], Is.EqualTo(c * 3.0 + s * 5.0), String.Format("Index (x): {0}", 2));
            Assert.That(x[3], Is.EqualTo(c * 4.0 + s * 3.0), String.Format("Index (x): {0}", 3));

            Assert.That(y[0], Is.EqualTo(c * 9.0 - s * 1.0), String.Format("Index (y): {0}", 0));
            Assert.That(y[1], Is.EqualTo(8), String.Format("Index (y): {0}", 1));
            Assert.That(y[2], Is.EqualTo(c * 7.0 - s * 2.0), String.Format("Index (y): {0}", 2));
            Assert.That(y[3], Is.EqualTo(6), String.Format("Index (y): {0}", 3));
            Assert.That(y[4], Is.EqualTo(c * 5.0 - s * 3.0), String.Format("Index (y): {0}", 4));
            Assert.That(y[5], Is.EqualTo(4), String.Format("Index (y): {0}", 5));
            Assert.That(y[6], Is.EqualTo(c * 3.0 - s * 4.0), String.Format("Index (y): {0}", 6));
        }

        /// <summary>A test function for 'DROTG' with respect to a specific test case data.
        /// </summary>
        /// <param name="a">The parameter a.</param>
        /// <param name="b">The parameter b.</param>
        [TestCase(2.5, -3.125)]
        [TestCase(8.9, 2.16)]
        public void drotg_TestCaseData(double a, double b)
        {
            double r = a;
            double z = b;
            double c, s;
            m_Level1BLAS.drotg(ref r, ref z, out c, out s);

            double actualR = c * a + s * b;
            Assert.That(actualR, Is.EqualTo(r).Within(1E-9));

            double actualNull = -s * a + c * b;
            Assert.That(actualNull, Is.EqualTo(0).Within(1E-9));
        }
      
        /// <summary>A test function for 'DROTM' with respect to a specific test case data.
        /// </summary>
        [TestCase(-1, TestName = "drotm_TestCaseData_ResultOfManuallyBenchmark(Flag -1), ie H:(h11 & h12 \\h21 & h22)")]        // '.' and '=' in test name are not allowed (bug in NunitAdapter?)
        [TestCase(0, TestName = "drotm_TestCaseData_ResultOfManuallyBenchmark(Flag 0), ie H:(1 & h12 \\h21 & 1)")]
        [TestCase(1, TestName = "drotm_TestCaseData_ResultOfManuallyBenchmark(Flag 1), ie H:(h11 & 1 \\-1 & h22)")]
        [TestCase(-2, TestName = "drotm_TestCaseData_ResultOfManuallyBenchmark(Flag -2), ie H:(1 & 0 \\0 & 1)")]
        public void drotm_TestCaseData_ResultOfManuallyBenchmark(int flag)
        {
            int n = 4;
            double[] x = new double[] { 1.25, -2.1, 4.25, 7.1 };
            double[] y = new double[] { -0.95, 1.45, 9.54, 5.12 };

            double h11 = 1.0;
            double h21 = 9.75;
            double h12 = -2.5;
            double h22 = -3.1;
            double[] param = new double[] { flag, h11, h21, h12, h22 };

            /* store the original values for later comparision: */
            double[] xOriginal = new double[n];
            double[] yOriginal = new double[n];
            x.CopyTo(xOriginal, 0);
            y.CopyTo(yOriginal, 0);

            m_Level1BLAS.drotm(n, x, y, param);
            /* do the test and store the array elements that depends on the 'flag': */
            switch (flag)
            {
                case -1:
                    break;

                case 0:
                    h11 = 1;
                    h22 = 1;
                    break;

                case 1:
                    h12 = 1;
                    h21 = -1;
                    break;

                case -2:
                    h11 = h22 = 1;
                    h12 = h21 = 0;
                    break;

                default:
                    throw new NotImplementedException();
            }

            for (int j = 0; j < n; j++)
            {
                Assert.That(x[j], Is.EqualTo(h11 * xOriginal[j] + h12 * yOriginal[j]).Within(1E-9), String.Format("x[{0}]", j));
                Assert.That(y[j], Is.EqualTo(h21 * xOriginal[j] + h22 * yOriginal[j]).Within(1E-9), String.Format("y[{0}]", j));
            }
        }

        /// <summary>A test function for 'DROTMG' with respect to a specific test case data.
        /// </summary>
        [TestCase(-1, TestName = "drotmg_TestCaseData(Flag -1), ie H:(h11 & h12 \\h21 & h22)")]        
        [TestCase(0, TestName = "drotmg_TestCaseData(Flag 0), ie H:(1 & h12 \\h21 & 1)")]
        [TestCase(1, TestName = "drotmg_TestCaseData(Flag 1), ie H:(h11 & 1 \\-1 & h22)")]
        [TestCase(-2, TestName = "drotmg_TestCaseData(Flag -2), ie H:(1 & 0 \\0 & 1)")]
        public void drotmg_TestCaseData(int flag)
        {
            /* the initial parameter: */
            double d1_In = 3.0;
            double d2_In = 1.25;
            double x1_In = 0.76;

            double d1 = d1_In;
            double d2 = d2_In;
            double x1 = x1_In;
            double y1 = -24.5;

            double[] param = new double[5];
            param[0] = flag;

            m_Level1BLAS.drotmg(ref d1, ref d2, ref x1, y1, param);

            double h11, h21, h12, h22;

            int outputFlag = (int)param[0];  // the flag may changed after the function evaluation

            switch (outputFlag)
            {
                case -1:
                    h11 = param[1];
                    h21 = param[2];
                    h12 = param[3];
                    h22 = param[4];
                    break;

                case 0:
                    h11 = 1.0;
                    h21 = param[2];
                    h12 = param[3];
                    h22 = 1.0;
                    break;

                case 1:
                    h11 = param[1];
                    h21 = -1.0;
                    h12 = 1.0;
                    h22 = param[4];
                    break;

                case -2:
                    h11 = 1.0;
                    h21 = 0.0;
                    h12 = 0.0;
                    h22 = 1.0;
                    break;

                default:
                    throw new NotImplementedException(String.Format("Flag {0} is not allowed.", flag));
            }

            /* check whether the second component is 0: */

            Assert.That(h21 * x1_In + h22 * y1, Is.EqualTo(0.0).Within(1E-9));  // why is this independent on the scaling factors d1, d2?

            /* we have that 
             *            (d1_In 0 \\ 0 d2_In ) = H^t * (d1_out 0 \\ 0 d2_out) * H,
             * we check for d1_In only:
             */
            double actual_d1 = h11 * h11 * d1 + h21 * h21 * d2;
            Assert.That(actual_d1, Is.EqualTo(d1_In).Within(1E-9));
        }

        /// <summary>A test function for 'DSCAL' with respect to a specific test case data.
        /// </summary>
        /// <param name="n">The number of elements of <paramref name="x"/>.</param>
        /// <param name="a">The scaling factor.</param>
        /// <param name="x">The vector x.</param>
        [TestCase(3, 1, -42.1, 51.01, 2.5)]
        [TestCase(3, -15.34, -42.1, 51.01, 2.5)]
        public void dscal_TestCaseData_ManuallyScaledSequenceEqual(int n, double a, params double[] x)
        {
            var expected = x.Select(z => a * z).ToArray();
            m_Level1BLAS.dscal(n, a, x);

            Assert.That(x.SequenceEqual(expected));
        }

        /// <summary>A test function for 'DSWAP' with respect to a specific test case data.
        /// </summary>
        [Test]
        public void dswap_TestCaseData_SwapedArrays()
        {
            int n = 6;
            double[] x = new double[] { 1, 2, 3, 4, 5, 6 };
            double[] y = new double[] { 11, 22, 33, 44, 55, 66 };

            m_Level1BLAS.dswap(n, x, y);
            int k = 1;
            for (int j = 0; j < n; j++)
            {
                Assert.That(x[j], Is.EqualTo(11.0 * k).Within(1E-9));
                Assert.That(y[j], Is.EqualTo(k).Within(1E-9));
                k++;
            }
        }

        /// <summary>A test function for 'IDAMX' with respect to a specific test case data.
        /// </summary>
        /// <param name="n">The number of elements in array <paramref name="x"/>.</param>
        /// <param name="incX">The increment for <paramref name="x"/>.</param>
        /// <param name="expectedMaxIndex">The expected null-based index of the maximum.</param>
        /// <param name="x">The vector x.</param>
        [TestCase(3, 1, 1, 5.0, 7.5, -1.2)]
        [TestCase(5, 1, 2, 1, 63.52, 120.5, -10.04, 10.1, 42)]
        [TestCase(4, 1, 0, 100.0, 99, 5, -13)]
        [TestCase(2, 1, 1, -33.01, 77)]
        public void idamax_TestCaseData_ExpectedMinIndex(int n, int incX, int expectedMaxIndex, params double[] x)
        {
            int actual = m_Level1BLAS.idamax(n, x, incX);
            Assert.That(actual, Is.EqualTo(expectedMaxIndex));
        }

        /// <summary>A test function for 'IDAMIN' with respect to a specific test case data.
        /// </summary>
        /// <param name="n">The number of elements in array <paramref name="x"/>.</param>
        /// <param name="incX">The increment for <paramref name="x"/>.</param>
        /// <param name="expectedMinIndex">The expected null-based index of the minimum.</param>
        /// <param name="x">The vector x.</param>
        [TestCase(6, 1, 1, 9.0, 3, -4, 34, 81.76, 5)]
        public void idamin_TestCaseData(int n, int incX, int expectedMinIndex, params double[] x)
        {
            int actual = m_Level1BLAS.idamin(n, x, incX);
            Assert.That(actual, Is.EqualTo(expectedMinIndex));
        }
        #endregion

        #region (double precision) complex methods

        /// <summary>A test function that compares the result of 'ZASUM' to the benchmark implementation.
        /// </summary>
        /// <param name="n">The number of elements in <paramref name="x"/>.</param>
        /// <param name="incX">The increment for <paramref name="x"/>.</param>
        /// <param name="x">The vector 'x'.</param>
        [TestCaseSource(nameof(TestCaseData_zasum))]
        public void zasum_TestCaseData_ResultOfBenchmarkImplementation(int n, int incX, params Complex[] x)
        {
            double actual = m_Level1BLAS.zasum(n, x, incX);
            double expected = m_BenchmarkLevel1BLAS.zasum(n, x, incX);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-9));
        }

        /// <summary>Gets the test case data for 'ZASUM'.
        /// </summary>
        /// <value>The test case data for 'ZASUM'.</value>
        public static IEnumerable<TestCaseData> TestCaseData_zasum
        {
            get
            {
                yield return new TestCaseData(1, 1, new Complex[] { 34.12 + 2.0 * Complex.ImaginaryOne });
                yield return new TestCaseData(2, 1, new Complex[] { 1.0, 3.5 + Complex.ImaginaryOne });
            }
        }

        /// <summary>A test function for 'ZAXPY' with respect to a specific test case data.
        /// </summary>
        [Test]
        public void zaxpy_TestCaseData_ManuallyBenchmark()
        {
            int n = 3;
            double a = 2.5;
            Complex[] x = new Complex[] { 1.0 + 3.12 * Complex.ImaginaryOne, 2.0 - 2 * Complex.ImaginaryOne, 3.0 };
            Complex[] y = new Complex[] { 3.0, 2.0 + 3.15 * Complex.ImaginaryOne, 1.0 };

            m_Level1BLAS.zaxpy(n, a, x, y);

            Assert.That(y[0], Is.EqualTo(a * x[0] + 3.0));
            Assert.That(y[1], Is.EqualTo(a * x[1] + 2.0 + 3.15 * Complex.ImaginaryOne));
            Assert.That(y[2], Is.EqualTo(a * x[2] + 1.0));
        }

        /// <summary>A test function for 'ZAXPY' (with extended parameters) with respect to a specific test case data.
        /// </summary>
        [Test]
        public void zaxpy_ExtendedTestCaseData_ResultOfManuallyBenchmark()
        {
            int n = 3;
            Complex a = -2.6 + 2.0 * Complex.ImaginaryOne;
            int incX = 2;
            int startIndexX = 1;

            int incY = 3;
            int startIndexY = 2;
            Complex[] x = new Complex[] { 1.0 + Complex.ImaginaryOne, 2.0 + 23.1 * Complex.ImaginaryOne, 3.0 * Complex.ImaginaryOne, 4.0, 5.0, 6.0 };
            Complex[] y = new Complex[] { 9.0 * Complex.ImaginaryOne, 8.0, 7.0 + Complex.ImaginaryOne, 6.0, 5.0, 4.0 * Complex.ImaginaryOne, 3.0, 2.0, 1.0 };

            m_Level1BLAS.zaxpy(n, a, x, y, incX, incY, startIndexX, startIndexY);

            Assert.That(y[0], Is.EqualTo(9.0 * Complex.ImaginaryOne));
            Assert.That(y[1], Is.EqualTo(new Complex(8.0, 0.0)));
            Assert.That(y[2], Is.EqualTo(a * x[startIndexX] + 7.0 + Complex.ImaginaryOne));
            Assert.That(y[3], Is.EqualTo(new Complex(6.0, 0.0)));
            Assert.That(y[4], Is.EqualTo(new Complex(5.0, 0.0)));
            Assert.That(y[5], Is.EqualTo(a * x[startIndexX + 1 * incX] + 4.0 * Complex.ImaginaryOne));
            Assert.That(y[6], Is.EqualTo(new Complex(3.0, 0.0)));
            Assert.That(y[7], Is.EqualTo(new Complex(2.0, 0.0)));
            Assert.That(y[8], Is.EqualTo(a * x[startIndexX + 2 * incX] + 1.0));
        }

        /// <summary>A test function for 'ZCOPY' with respect to a specific test case data.
        /// </summary>
        /// <param name="n">The number of elements in <paramref name="x"/>.</param>
        /// <param name="x">The vector x.</param>
        [TestCaseSource(nameof(TestCaseData_zcopy))]
        public void zcopy_TestCaseData_SequenceEqualArray(int n, params Complex[] x)
        {
            Complex[] y = new Complex[n];
            m_Level1BLAS.zcopy(n, x, y);

            Assert.That(y.SequenceEqual(x));
        }

        /// <summary>Gets the test case data for 'ZCOPY'.
        /// </summary>
        /// <value>The test case data for 'ZCOPY'.</value>
        public static IEnumerable<TestCaseData> TestCaseData_zcopy
        {
            get
            {
                yield return new TestCaseData(2, new Complex[] { 1.0 * Complex.ImaginaryOne, 4.5 });
                yield return new TestCaseData(5, new Complex[] { 1.0, 2.0, 3.0, 4.0 * Complex.ImaginaryOne, 5.0 });
            }
        }

        /// <summary>A test function that compares the result of 'ZCOPY' to the benchmark implementation.
        /// </summary>
        /// <param name="n">The number of elements in <paramref name="x"/>.</param>
        /// <param name="startIndexX">The null-based start index of <paramref name="x"/>.</param>
        /// <param name="startIndexY">The null-based start index of y.</param>
        /// <param name="incX">The increment for <paramref name="x"/>.</param>
        /// <param name="incY">The increment for y.</param>
        /// <param name="x">The vector x.</param>
        [TestCaseSource(nameof(TestCaseDataExtended_zcopy))]
        public void zcopy_ExtendedTestCaseData_SequenceEqualArray(int n, int startIndexX, int startIndexY, int incX, int incY, params Complex[] x)
        {
            Complex[] y = new Complex[startIndexY + incY * n];

            m_Level1BLAS.zcopy(n, x, y, incX, incY, startIndexX, startIndexY);

            var sparseY = y.Where((z, i) => { return (i >= startIndexY) && (((i - startIndexY) % incY) == 0); });
            var sparseX = x.Where((z, i) => { return (i >= startIndexX) && (((i - startIndexX) % incX) == 0); });

            Assert.That(sparseY.SequenceEqual(sparseX));
        }

        /// <summary>Gets the extended test case data for 'ZCOPY'.
        /// </summary>
        /// <value>The extended test case data for 'ZCOPY'.</value>
        public static IEnumerable<TestCaseData> TestCaseDataExtended_zcopy
        {
            get
            {
                yield return new TestCaseData(2, 0, 0, 1, 1, new Complex[] { 24.0 - 2.9 * Complex.ImaginaryOne, 48.0 });
                yield return new TestCaseData(2, 1, 0, 1, 2, new Complex[] { -12.0 + 8.0 * Complex.ImaginaryOne, 8.0, 1.45 * Complex.ImaginaryOne });
            }
        }

        /// <summary>A test function that compares the result of 'ZDOTC' to the benchmark implementation.
        /// </summary>
        [Test]
        public void zdotc_TestCaseData_ResultOfBenchmarkImplementation()
        {
            int n = 4;
            Complex[] x = new Complex[] { 1 + Complex.ImaginaryOne, 2 + 12.4 * Complex.ImaginaryOne, 3 * Complex.ImaginaryOne, 4 };
            Complex[] y = new Complex[] { 1, -2 * Complex.ImaginaryOne, -3 + 1.2 * Complex.ImaginaryOne, 4 };
            Complex actual = m_Level1BLAS.zdotc(n, x, y);

            Complex expected = m_BenchmarkLevel1BLAS.zdotc(n, x, y);

            Assert.That(Complex.Abs(actual - expected), Is.EqualTo(0).Within(1E-9), String.Format("Actual: {0}, Expected: {1}", actual, expected));
        }

        /// <summary>A test function that compares the result of 'ZDOTC' (with extended parameters) to the benchmark implementation.
        /// </summary>
        [Test]
        public void zdotc_ExtendedTestCaseData_ResultOfBenchmarkImplementation()
        {
            int n = 3;
            int incX = 2;
            int incY = 3;

            Complex[] x = new Complex[] { 1, 2, 3, 4, 5, 6 + Complex.ImaginaryOne };
            Complex[] y = new Complex[] { -2, -3 + 2.25 * Complex.ImaginaryOne, -5, -23, 4 - 1.2 * Complex.ImaginaryOne, 7, 8, 9, -42 };

            Complex actual = m_Level1BLAS.zdotc(n, x, y, incX, incY);
            Complex expected = m_BenchmarkLevel1BLAS.zdotc(n, x, y, incX, incY);

            Assert.That(Complex.Abs(actual - expected), Is.EqualTo(0).Within(1E-9), String.Format("Actual: {0}, Expected: {1}", actual, expected));
        }

        /// <summary>A test function that compares the result of 'ZDOTU' to the benchmark implementation.
        /// </summary>
        [Test]
        public void zdotu_TestCaseData_ResultOfBenchmarkImplementation()
        {
            int n = 4;
            Complex[] x = new Complex[] { 1 + Complex.ImaginaryOne, 2 + 12.4 * Complex.ImaginaryOne, 3 * Complex.ImaginaryOne, 4 };
            Complex[] y = new Complex[] { 1, -2 * Complex.ImaginaryOne, -3 + 1.2 * Complex.ImaginaryOne, 4 };
            Complex actual = m_Level1BLAS.zdotu(n, x, y);

            Complex expected = m_BenchmarkLevel1BLAS.zdotu(n, x, y);

            Assert.That(Complex.Abs(actual - expected), Is.EqualTo(0).Within(1E-9), String.Format("Actual: {0}, Expected: {1}", actual, expected));
        }

        /// <summary>A test function that compares the result of 'ZDOTU' (with extended parameters) to the benchmark implementation.
        /// </summary>
        [Test]
        public void zdotu_ExtendedTestCaseData_ResultOfBenchmarkImplementation()
        {
            int n = 3;
            int incX = 2;
            int incY = 3;

            Complex[] x = new Complex[] { 1, 2, 3, 4 - 5.9 * Complex.ImaginaryOne, 5, 6 + Complex.ImaginaryOne };
            Complex[] y = new Complex[] { -2, -3 + 2.25 * Complex.ImaginaryOne, -5, -23, 4 - 1.2 * Complex.ImaginaryOne, 7, 8, 9, -42 };

            Complex actual = m_Level1BLAS.zdotu(n, x, y, incX, incY);
            Complex expected = m_BenchmarkLevel1BLAS.zdotu(n, x, y, incX, incY);

            Assert.That(Complex.Abs(actual - expected), Is.EqualTo(0).Within(1E-9), String.Format("Actual: {0}, Expected: {1}", actual, expected));
        }

        /// <summary>A test function that compares the result of 'ZNRM2' to the benchmark implementation.
        /// </summary>
        /// <param name="n">The number of elements in <paramref name="x"/>.</param>
        /// <param name="incX">The increment for <paramref name="x"/>.</param>
        /// <param name="x">The vector x.</param>
        [TestCaseSource(nameof(TestCaseDataExtended_znrm2))]
        public void znrm2_TestCaseData_ResultOfBenchmarkImplementation(int n, int incX, params Complex[] x)
        {
            double actual = m_Level1BLAS.znrm2(n, x, incX);
            double expected = m_BenchmarkLevel1BLAS.znrm2(n, x, incX);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-9));
        }

        /// <summary>Gets the extended test case data for 'ZNRM2'.
        /// </summary>
        /// <value>The extended test case data for 'ZNRM2'.</value>
        public static IEnumerable<TestCaseData> TestCaseDataExtended_znrm2
        {
            get
            {
                yield return new TestCaseData(2, 1, new Complex[] { 1.0 * Complex.ImaginaryOne, -9.2 });
                yield return new TestCaseData(3, 2, new Complex[] { -8.1, 5.6, -77.12, 942.42 + 32.1 * Complex.ImaginaryOne, 615.12 * Complex.ImaginaryOne, -0.77171 - 12.13 * Complex.ImaginaryOne });
            }
        }

        /// <summary>A test function for 'ZDROT' with respect to a specific test case data.
        /// </summary>
        [Test]
        public void zdrot_TestCaseData_ResultOfManuallyBenchmark()
        {
            int n = 4;
            int incX = 1;
            int incY = 2;
            double c = 2.5;
            double s = -1.25;
            Complex[] x = new Complex[] { 1 * Complex.ImaginaryOne, 2, 3, 4 };
            Complex[] y = new Complex[] { 9, 8 - 2.0 * Complex.ImaginaryOne, 7 + 3.12 * Complex.ImaginaryOne, 6, 5, 4, 3 };

            m_Level1BLAS.zdrot(n, x, y, c, s, incX, incY);

            Assert.That(x[0], Is.EqualTo((Complex)c * 1.0 * Complex.ImaginaryOne + s * 9.0), String.Format("Index (x): {0}", 0));
            Assert.That(x[1], Is.EqualTo((Complex)c * 2.0 + s * (7.0 + 3.12 * Complex.ImaginaryOne)), String.Format("Index (x): {0}", 1));
            Assert.That(x[2], Is.EqualTo((Complex)c * 3.0 + s * 5.0), String.Format("Index (x): {0}", 2));
            Assert.That(x[3], Is.EqualTo((Complex)c * 4.0 + s * 3.0), String.Format("Index (x): {0}", 3));

            Assert.That(y[0], Is.EqualTo((Complex)c * 9.0 - s * 1.0 * Complex.ImaginaryOne), String.Format("Index (y): {0}", 0));
            Assert.That(y[1], Is.EqualTo((Complex)8 - 2.0 * Complex.ImaginaryOne), String.Format("Index (y): {0}", 1));
            Assert.That(y[2], Is.EqualTo((Complex)c * (7.0 + 3.12 * Complex.ImaginaryOne) - s * 2.0), String.Format("Index (y): {0}", 2));
            Assert.That(y[3], Is.EqualTo((Complex)6), String.Format("Index (y): {0}", 3));
            Assert.That(y[4], Is.EqualTo((Complex)c * 5.0 - s * 3.0), String.Format("Index (y): {0}", 4));
            Assert.That(y[5], Is.EqualTo((Complex)4), String.Format("Index (y): {0}", 5));
            Assert.That(y[6], Is.EqualTo((Complex)c * 3.0 - s * 4.0), String.Format("Index (y): {0}", 6));
        }

        /// <summary>A test function for 'ZROTG' with respect to a specific test case data.
        /// </summary>
        /// <param name="a">The parameter a.</param>
        /// <param name="b">The parameter b.</param>
        [TestCaseSource(nameof(TestCaseData_zrotg))]
        public void zrotg_TestCaseData(Complex a, Complex b)
        {
            Complex r = a;
            Complex z = b;
            double c;
            Complex s;
            m_Level1BLAS.zrotg(ref r, ref z, out c, out s);

            Complex actualR = c * a + s * b;
            Assert.That(Complex.Abs(actualR - r) < 1E-9, String.Format("Check for parameter 'r', actual = {0}, expected = {1}", actualR, r));

            Complex actualNull = -Complex.Conjugate(s) * a + c * b;
            Assert.That(actualNull.IsZero(1E-9), String.Format("Check for 0.0, but was {0}", actualNull));
        }

        /// <summary>Gets the test case data for 'ZROTG'.
        /// </summary>
        /// <value>The test case data for 'ZROTG'.</value>
        public static IEnumerable<TestCaseData> TestCaseData_zrotg
        {
            get
            {
                yield return new TestCaseData(new Complex(2.5, 0.0), new Complex(-3.125, 0.0));
                yield return new TestCaseData(new Complex(8.9, 0.0), new Complex(2.16, 0.0));
                yield return new TestCaseData(-2.45 * Complex.ImaginaryOne, new Complex(2.16, 0.0));
                yield return new TestCaseData(-2.45 * Complex.ImaginaryOne, 2.16 + 4.12 * Complex.ImaginaryOne);
            }
        }

        /// <summary>A test function for 'ZSCAL' with respect to a specific test case data.
        /// </summary>
        /// <param name="n">The number of elements of <paramref name="x"/>.</param>
        /// <param name="a">The scaling factor.</param>
        /// <param name="x">The vector x.</param>
        [TestCaseSource(nameof(TestCaseData_zscal))]
        public void zscal_TestCaseData_ManuallyScaledSequenceEqual(int n, Complex a, params Complex[] x)
        {
            var expected = x.Select(z => a * z).ToArray();
            m_Level1BLAS.zscal(n, a, x);

            Assert.That(x.SequenceEqual(expected));
        }

        /// <summary>Gets the test case data for 'ZSCAL'.
        /// </summary>
        /// <value>The test case data for 'ZSCAL'.</value>
        public static IEnumerable<TestCaseData> TestCaseData_zscal
        {
            get
            {
                yield return new TestCaseData(3, (Complex)1, new Complex[] { -42.1, 51.01, 2.5 });
                yield return new TestCaseData(3, (Complex)(-15.34), new Complex[] { -42.1, 51.01, 2.5 });
                yield return new TestCaseData(3, -15.34 * Complex.ImaginaryOne, new Complex[] { -42.1 + 2 * Complex.ImaginaryOne, 51.01, 2.5 * Complex.ImaginaryOne });
                yield return new TestCaseData(3, 23.12 + 2.32 * Complex.ImaginaryOne, new Complex[] { -2.1 + 2.2 * Complex.ImaginaryOne, 51.01, 2.5 * Complex.ImaginaryOne });
            }
        }

        /// <summary>A test function for 'ZDSCAL' with respect to a specific test case data.
        /// </summary>
        /// <param name="n">The number of elements of <paramref name="x"/>.</param>
        /// <param name="a">The scaling factor.</param>
        /// <param name="x">The vector x.</param>
        [TestCaseSource(nameof(TestCaseData_zdscal))]
        public void zdscal_TestCaseData_ManuallyScaledSequenceEqual(int n, double a, params Complex[] x)
        {
            var expected = x.Select(z => a * z).ToArray();
            m_Level1BLAS.zdscal(n, a, x);

            Assert.That(x.SequenceEqual(expected));
        }

        /// <summary>Gets the test case data for 'ZDSCAL'.
        /// </summary>
        /// <value>The test case data for 'ZDSCAL'.</value>
        public static IEnumerable<TestCaseData> TestCaseData_zdscal
        {
            get
            {
                yield return new TestCaseData(3, 1, new Complex[] { -42.1, 51.01, 2.5 });
                yield return new TestCaseData(3, -15.34, new Complex[] { -42.1, 51.01, 2.5 });
                yield return new TestCaseData(3, -15.34, new Complex[] { -42.1 + 2 * Complex.ImaginaryOne, 51.01, 2.5 * Complex.ImaginaryOne });
            }
        }

        /// <summary>A test function for 'ZDSCAL' with respect to a specific test case data.
        /// </summary>
        /// <param name="a">The scaling factor.</param>
        [TestCase(1.0)]
        [TestCase(-1.871)]
        [TestCase(41.761)]
        public void zdscale_ExtendedTestCaseData(double a)
        {
            int n = 3;
            int startX = 1;
            int increment = 2;

            var x = new Complex[] { 1, 2, 3, 4 - 5.9 * Complex.ImaginaryOne, 5, 6 + Complex.ImaginaryOne };

            Complex[] actual = x.ToArray();
            m_Level1BLAS.zdscal(n, a, actual, increment, startX);

            Complex[] expected = x.ToArray();
            m_BenchmarkLevel1BLAS.zdscal(n, a, expected, increment, startX);

            Assert.That(actual, new ComplexArrayNUnitConstraint(expected, tolerance: 1E-9));
        }

        /// <summary>A test function for 'ZSWAP' with respect to a specific test case data.
        /// </summary>
        [Test]
        public void zswap_TestCaseData_SwapedArrays()
        {
            int n = 6;
            Complex[] x = new Complex[] { 1, 2, 3, 4, 5, 6 };
            Complex[] y = new Complex[] { 11, 22, 33, 44, 55, 66 };

            m_Level1BLAS.zswap(n, x, y);
            int k = 1;
            for (int j = 0; j < n; j++)
            {
                Assert.That(x[j], Is.EqualTo(new Complex(11.0 * k, 0.0)).Within(1E-9));
                Assert.That(y[j], Is.EqualTo(new Complex(k, 0.0)).Within(1E-9));
                k++;
            }
        }

        /// <summary>A test function for 'IZAMAX' with respect to a specific test case data.
        /// </summary>
        /// <param name="n">The number of elements in array <paramref name="x"/>.</param>
        /// <param name="incX">The increment for <paramref name="x"/>.</param>
        /// <param name="expectedMaxIndex">The expected null-based index of the maximum.</param>
        /// <param name="x">The vector x.</param>
        [TestCaseSource(nameof(TestCaseData_izamax))]
        public void izamax_TestCaseData_ExpectedMinIndex(int n, int incX, int expectedMaxIndex, params Complex[] x)
        {
            int actual = m_Level1BLAS.izamax(n, x, incX);
            Assert.That(actual, Is.EqualTo(expectedMaxIndex));
        }

        /// <summary>Gets the test case data for 'IZAMAX'.
        /// </summary>
        /// <value>The test case data for 'IZAMAX'.</value>
        public static IEnumerable<TestCaseData> TestCaseData_izamax
        {
            get
            {
                yield return new TestCaseData(3, 1, 1, new Complex[] { 5.0, 7.5, -1.2 });
                yield return new TestCaseData(5, 1, 2, new Complex[] { 1, 63.52, 120.5, -10.04, 10.1, 42 });
                yield return new TestCaseData(4, 1, 0, new Complex[] { 100.0, 99, 5, -13 });
                yield return new TestCaseData(2, 1, 1, new Complex[] { -33.01, 77 });
            }
        }

        /// <summary>A test function for 'IZAMIN' with respect to a specific test case data.
        /// </summary>
        /// <param name="n">The number of elements in array <paramref name="x"/>.</param>
        /// <param name="incX">The increment for <paramref name="x"/>.</param>
        /// <param name="expectedMinIndex">The expected null-based index of the minimum.</param>
        /// <param name="x">The vector x.</param>
        [TestCaseSource(nameof(TestCaseData_izamin))]
        public void izamin_TestCaseData(int n, int incX, int expectedMinIndex, params Complex[] x)
        {
            int actual = m_Level1BLAS.izamin(n, x, incX);
            Assert.That(actual, Is.EqualTo(expectedMinIndex));
        }

        /// <summary>Gets the test case data for 'IZAMIN'.
        /// </summary>
        /// <value>The test case data for 'IZAMIN'.</value>
        public static IEnumerable<TestCaseData> TestCaseData_izamin
        {
            get
            {
                yield return new TestCaseData(6, 1, 1, new Complex[] { 9.0, 3, -4, 34, 81.76, 5 });
                yield return new TestCaseData(6, 1, 2, new Complex[] { 9.0, 3, -Complex.ImaginaryOne, 34, 81.76, 5 });
            }
        }
        #endregion

        #endregion

        #region protected methods

        /// <summary>Gets the level 1 BLAS implementation.
        /// </summary>
        /// <returns>A <see cref="ILevel1BLAS"/> object that encapuslate the level 1 BLAS functions.</returns>
        protected abstract ILevel1BLAS GetLevel1BLAS();
        #endregion
    }
}