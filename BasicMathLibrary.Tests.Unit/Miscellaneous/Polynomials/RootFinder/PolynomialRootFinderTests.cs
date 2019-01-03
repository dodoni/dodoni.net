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

namespace Dodoni.MathLibrary.Miscellaneous
{
    /// <summary>Serves as abstract unit test class for <see cref="IPolynomialRootFinder"/> objects.
    /// </summary>
    [TestFixture]
    public abstract class PolynomialRootFinderTests
    {
        /// <summary>A test function for <see cref="IPolynomialRootFinder.GetRoots(int,IList{Complex}, IList{Complex})"/>.
        /// </summary>
        /// <param name="degree">The degree.</param>
        /// <param name="coefficientsAsObjects">The coefficients of the polynomial.</param>
        [TestCaseSource(nameof(RootComplexTestCaseData))]
        public void GetRoots_ComplexTestCase_ZeroFunctionValue(int degree, object[] coefficientsAsObjects)
        {
            var coefficients = coefficientsAsObjects[0] as Complex[];

            var polynomialRootFinder = GetPolynomialRootFinder();

            var roots = new List<Complex>();
            polynomialRootFinder.GetRoots(degree, coefficients, roots);

            Assert.That(roots.Count, Is.EqualTo(degree), String.Format("Actual number of roots {0}; expected {1}.", roots.Count, degree));
            var polynomial = Polynomial.Complex.Create(degree, coefficients);

            for (int j = 0; j < degree; j++)
            {
                var actualFunctionValue = polynomial.GetValue(roots[j]);

                Assert.That(Complex.Abs(actualFunctionValue), Is.EqualTo(0.0).Within(1E-8), String.Format("Function value of {0}-th actual root {1} is {2} != 0.0.", j, roots[j], actualFunctionValue));
            }
        }

        /// <summary>Gets the test case data for polynomials with complex coefficients.
        /// </summary>
        /// <value>The test data for polynomials with complex coefficients.</value>        
        public static IEnumerable<TestCaseData> RootComplexTestCaseData
        {
            get
            {
                yield return new TestCaseData(5, new object[] { new Complex[] { 1.25, 2, 3, 4, 5, 1.0 } });
                yield return new TestCaseData(6, new object[] { new Complex[] { 1.25 + 0.5 * Complex.ImaginaryOne, 2, 3, 4 * Complex.ImaginaryOne, 5.25, 1.0, -0.75 } });
                yield return new TestCaseData(7, new object[] { new Complex[] { 4.25 + 1.25 * Complex.ImaginaryOne, 2, 3 * Complex.ImaginaryOne, 1.65 * Complex.ImaginaryOne, 5.25, 1.0, -0.75, 3.62 * Complex.ImaginaryOne } });
            }
        }

        /// <summary>A test function for <see cref="IPolynomialRootFinder.GetRealRoots(int,IList{Complex}, IList{double},double)"/>.
        /// </summary>
        /// <param name="degree">The degree.</param>
        /// <param name="coefficientsAsObjects">The coefficients of the polynomial.</param>        
        [TestCaseSource(nameof(RootComplexTestCaseDataWithRealRoots))]
        public void GetRealRoots_ComplexTestCase_ZeroFunctionValue(int degree, object[] coefficientsAsObjects)
        {
            var coefficients = coefficientsAsObjects[0] as Complex[];

            var polynomialRootFinder = GetPolynomialRootFinder();

            var roots = new List<double>();
            polynomialRootFinder.GetRealRoots(degree, coefficients, roots);
            Assume.That(roots.Count > 0, "No real root.");

            var polynomial = Polynomial.Complex.Create(degree, coefficients);

            for (int j = 0; j < roots.Count; j++)
            {
                var actualFunctionValue = polynomial.GetValue(roots[j]);

                Assert.That(Complex.Abs(actualFunctionValue), Is.EqualTo(0.0).Within(1E-8), String.Format("Function value of {0}-th actual root {1} is {2} != 0.0.", j, roots[j], actualFunctionValue));
            }
        }

        /// <summary>Gets the test case data for polynomials with complex coefficients.
        /// </summary>
        /// <value>The test data for polynomials with complex coefficients.</value>        
        public static IEnumerable<TestCaseData> RootComplexTestCaseDataWithRealRoots
        {
            get
            {
                yield return new TestCaseData(5, new object[] { new Complex[] { 1.25, 2, 3, 4, 5, 1.0 } });
                yield return new TestCaseData(3, new object[] { new Complex[] { -4.0, 6.0, -3.0, 1.0 } });
                yield return new TestCaseData(3, new object[] { new Complex[] { 6 - 10 * Complex.ImaginaryOne, 13 * Complex.ImaginaryOne + 7.0, -7 - 4 * Complex.ImaginaryOne, 1.0 } });
            }
        }

        /// <summary>A test function for <see cref="IPolynomialRootFinder.GetRealRoots(int,IList{Complex}, IList{double},double)"/>.
        /// </summary>
        /// <param name="degree">The degree.</param>
        /// <param name="coefficientsAsObjects">The coefficients of the polynomial.</param>        
        [TestCaseSource(nameof(RootComplexTestCaseDataNoRealRoots))]
        public void GetRealRoots_ComplexTestCase_NoRealRootThrown(int degree, object[] coefficientsAsObjects)
        {
            var coefficients = coefficientsAsObjects[0] as Complex[];

            var polynomialRootFinder = GetPolynomialRootFinder();

            var roots = new List<double>();
            polynomialRootFinder.GetRealRoots(degree, coefficients, roots);
            Assert.That(roots.Count == 0, "No real root.");
        }

        /// <summary>Gets the test case data for polynomials with complex coefficients.
        /// </summary>
        /// <value>The test data for polynomials with complex coefficients.</value>        
        public static IEnumerable<TestCaseData> RootComplexTestCaseDataNoRealRoots
        {
            get
            {
                yield return new TestCaseData(6, new object[] { new Complex[] { 1.25 + 0.5 * Complex.ImaginaryOne, 2, 3, 4 * Complex.ImaginaryOne, 5.25, 1.0, -0.75 } }); // has no real roots
                yield return new TestCaseData(7, new object[] { new Complex[] { 4.25 + 1.25 * Complex.ImaginaryOne, 2, 3 * Complex.ImaginaryOne, 1.65 * Complex.ImaginaryOne, 5.25, 1.0, -0.75, 3.62 * Complex.ImaginaryOne } });
            }
        }

        /// <summary>A test function for <see cref="IPolynomialRootFinder.GetRoots(int,IList{double}, IList{Complex})"/>.
        /// </summary>
        /// <param name="degree">The degree.</param>
        /// <param name="coefficients">The coefficients of the polynomial.</param>
        [TestCaseSource(nameof(GetRootRealTestCaseData))]
        public void GetRoots_RealTestCase_ZeroFunctionValue(int degree, double[] coefficients)
        {
            var polynomialRootFinder = GetPolynomialRootFinder();

            var roots = new List<Complex>();
            polynomialRootFinder.GetRoots(degree, coefficients, roots);

            Assert.That(roots.Count, Is.EqualTo(degree), String.Format("Actual number of roots {0}; expected {1}.", roots.Count, degree));
            var polynomial = Polynomial.Real.Create(degree, coefficients);

            for (int j = 0; j < degree; j++)
            {
                var actualFunctionValue = polynomial.GetValue(roots[j]);

                Assert.That(Complex.Abs(actualFunctionValue), Is.EqualTo(0.0).Within(1E-8), String.Format("Function value of {0}-th actual root {1} is {2} != 0.0.", j, roots[j], actualFunctionValue));
            }
        }

        /// <summary>A test function for <see cref="IPolynomialRootFinder.GetRealRoots(int,IList{double}, IList{double},double)"/>.
        /// </summary>
        /// <param name="degree">The degree.</param>
        /// <param name="coefficients">The coefficients of the polynomial.</param>
        [TestCaseSource(nameof(GetRootRealTestCaseData))]
        public void GetRealRoots_RealTestCase_ZeroFunctionValue(int degree, double[] coefficients)
        {
            var polynomialRootFinder = GetPolynomialRootFinder();

            var roots = new List<double>();
            polynomialRootFinder.GetRealRoots(degree, coefficients, roots);
            Assume.That(roots.Count > 0, "No real root.");

            var polynomial = Polynomial.Real.Create(degree, coefficients);

            for (int j = 0; j < roots.Count; j++)
            {
                var actualFunctionValue = polynomial.GetValue(roots[j]);

                Assert.That(Math.Abs(actualFunctionValue), Is.EqualTo(0.0).Within(1E-8), String.Format("Function value of {0}-th actual root {1} is {2} != 0.0.", j, roots[j], actualFunctionValue));
            }
        }

        /// <summary>Gets the test case data for polynomials with real coefficients.
        /// </summary>
        /// <value>The test data for polynomials with real coefficients.</value>
        public static IEnumerable<TestCaseData> GetRootRealTestCaseData
        {
            get
            {
                yield return new TestCaseData(5, new double[] { 1.25, 2, 3, 4, 5, 1.0 });
                yield return new TestCaseData(6, new double[] { 1.25, 2, 3, 4, 5.25, 1.0, -0.75 });
                yield return new TestCaseData(7, new double[] { 4.25, 2, 3, 1.65, 5.25, 1.0, -0.75, 3.62 });
            }
        }

        /// <summary>Gets the <see cref="IPolynomialRootFinder"/> object under test.
        /// </summary>
        /// <returns>The <see cref="IPolynomialRootFinder"/> under test.</returns>
        protected abstract IPolynomialRootFinder GetPolynomialRootFinder();
    }
}