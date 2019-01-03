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
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

using Dodoni.MathLibrary.Basics;

using NUnit.Framework;

namespace Dodoni.MathLibrary.Miscellaneous
{
    /// <summary>Serves as abstract unit test class for <see cref="IPolynomial"/> objects.
    /// </summary>
    [TestFixture]
    public abstract class PolynomialTests
    {
        /// <summary>A test function for <see cref="ComplexPolynomialFactory.GetCoefficientsByRoots(int, IList{Complex}, IList{Complex}, Complex)"/>.
        /// </summary>
        /// <param name="degree">The degree.</param>
        /// <param name="coefficients">The coefficients of the polynomial.</param>
        [TestCaseSource(nameof(GetComplexCoefficientsByRootsTestCaseData))]
        public void GetCoefficientsByRoots_TestCasePolynomialCoefficients_Coefficients(int degree, Complex[] coefficients)
        {
            var expectedCoefficients = coefficients.ToArray();

            var polynomial = Polynomial.Complex.Create(degree, coefficients);
            var polynomialRootFinder = GetPolynomialRootFinder();

            var roots = new List<Complex>();
            polynomial.GetRoots(roots, polynomialRootFinder);

            var actualCoefficients = new Complex[degree + 1];
            Polynomial.Complex.GetCoefficientsByRoots(degree, roots, actualCoefficients, coefficients[degree]);

            Assert.That(actualCoefficients, new ComplexArrayNUnitConstraint(expectedCoefficients, tolerance: 1E-8));
        }

        /// <summary>Gets the test case data for polynomials with complex coefficients.
        /// </summary>
        /// <value>The test data for polynomials with complex coefficients.</value>
        public static IEnumerable<TestCaseData> GetComplexCoefficientsByRootsTestCaseData
        {
            get
            {
                yield return new TestCaseData(5, new Complex[] { 1.25, 2, 3, 4, 5, 1.0 });
                yield return new TestCaseData(6, new Complex[] { 1.25 + 0.5 * Complex.ImaginaryOne, 2, 3, 4 * Complex.ImaginaryOne, 5.25, 1.0, -0.75 });
                yield return new TestCaseData(7, new Complex[] { 4.25 + 1.25 * Complex.ImaginaryOne, 2, 3 * Complex.ImaginaryOne, 1.65 * Complex.ImaginaryOne, 5.25, 1.0, -0.75, 3.62 * Complex.ImaginaryOne });
            }
        }

        /// <summary>A test function for <see cref="RealPolynomialFactory.GetCoefficientsByRoots(int, IList{Complex}, IList{double}, double,double)"/>.
        /// </summary>
        /// <param name="degree">The degree.</param>
        /// <param name="coefficients">The coefficients of the polynomial.</param>
        [TestCaseSource(nameof(GetRealCoefficientsByRootsTestCaseData))]
        public void GetCoefficientsByRoots_TestCasePolynomialCoefficients_Coefficients(int degree, double[] coefficients)
        {
            var expectedCoefficients = coefficients.ToArray();

            var polynomial = Polynomial.Real.Create(degree, coefficients);
            var polynomialRootFinder = GetPolynomialRootFinder();

            var roots = new List<Complex>();
            polynomial.GetRoots(roots, polynomialRootFinder);

            var actualCoefficients = new double[degree + 1];
            Polynomial.Real.GetCoefficientsByRoots(degree, roots, actualCoefficients, coefficients[degree]);

            Assert.That(actualCoefficients, Is.EqualTo(expectedCoefficients).AsCollection.Within(1E-8));
        }


        /// <summary>Gets the test case data for polynomials with real coefficients.
        /// </summary>
        /// <value>The test data for polynomials with real coefficients.</value>
        public static IEnumerable<TestCaseData> GetRealCoefficientsByRootsTestCaseData
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