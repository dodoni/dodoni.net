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

namespace Dodoni.MathLibrary.Basics
{
    /// <summary>Serves as abstract unit test class for complex numbers.
    /// </summary>
    /// <remarks>In a previous release of dodoni.net a own implementation of complex numbers was used. Since .NET 4.0 the .net framework provides its own implementation.
    /// This test class compares the result of the .net implementation and my own implementation (<see cref="ComplexNumber"/>).
    /// </remarks>
    [TestFixture]
    public class ComplexTests
    {
        #region public methods

        /// <summary>A test function for the sum of two complex numbers.
        /// </summary>
        /// <param name="a">The first input number.</param>
        /// <param name="b">The second input number.</param>
        [TestCaseSource(nameof(TwoComplexObjectsTestCaseData))]
        public void OperatorPlus_TestCaseData_ResultOfBenchmarkImplementation(Complex a, Complex b)
        {
            Complex actual = a + b;

            ComplexNumber expected = ComplexNumber.Create(a.Real, a.Imaginary) + ComplexNumber.Create(b.Real, b.Imaginary);

            Assert.That(actual.Real, Is.EqualTo(expected.RealPart).Within(1E-9));
            Assert.That(actual.Imaginary, Is.EqualTo(expected.ImaginaryPart).Within(1E-9));
        }

        /// <summary>A test function for the product of two complex numbers.
        /// </summary>
        /// <param name="a">The first input number.</param>
        /// <param name="b">The second input number.</param>
        [TestCaseSource(nameof(TwoComplexObjectsTestCaseData))]
        public void OperatorMultiplication_TestCaseData_ResultOfBenchmarkImplementation(Complex a, Complex b)
        {
            Complex actual = a * b;

            ComplexNumber expected = ComplexNumber.Create(a.Real, a.Imaginary) * ComplexNumber.Create(b.Real, b.Imaginary);

            Assert.That(actual.Real, Is.EqualTo(expected.RealPart).Within(1E-9));
            Assert.That(actual.Imaginary, Is.EqualTo(expected.ImaginaryPart).Within(1E-9));
        }

        /// <summary>A test function for the evaluation of the exponential function.
        /// </summary>
        /// <param name="z">The input number.</param>
        [TestCaseSource(nameof(OneComplexObjectTestCaseData))]
        public void Exp_TestCaseData_ResultOfBenchmarkImplementation(Complex z)
        {
            Complex actual = Complex.Exp(z);

            ComplexNumber expected = ComplexNumber.Exp(new ComplexNumber(z.Real, z.Imaginary));

            Assert.That(actual.Real, Is.EqualTo(expected.RealPart).Within(1E-9));
            Assert.That(actual.Imaginary, Is.EqualTo(expected.ImaginaryPart).Within(1E-9));
        }

        /// <summary>A test function for the evaluation of the phase (argument) of a specified complex number.
        /// </summary>
        /// <param name="z">The input number.</param>
        [TestCaseSource(nameof(OneComplexObjectTestCaseData))]
        public void Phase_TestCaseData_ResultOfBenchmarkImplementation(Complex z)
        {
            double actual = z.Phase;

            double expected = ComplexNumber.Create(z.Real, z.Imaginary).Argument;

            Assert.That(actual, Is.EqualTo(expected).Within(1E-9));
        }

        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects that encapsulate two <see cref="Complex"/> objects.
        /// </summary>
        /// <value>The two-complex-objects test case data.</value>
        public static IEnumerable<TestCaseData> TwoComplexObjectsTestCaseData
        {
            get
            {
                yield return new TestCaseData(new Complex(1, 1), new Complex(2, 4.5));
                yield return new TestCaseData(new Complex(-0.7615, 141.12), new Complex(-2.65, 2.55));
                yield return new TestCaseData(new Complex(1, -14.12), new Complex(3.12, -7.54));
                yield return new TestCaseData(new Complex(0.7615, -11.12), new Complex(-2.65, -2.55));
            }
        }


        /// <summary>Gets a collection of <see cref="TestCaseData"/> objects that encapsulate one <see cref="Complex"/> objects.
        /// </summary>
        /// <value>The complex-object test case data.</value>
        public static IEnumerable<TestCaseData> OneComplexObjectTestCaseData
        {
            get
            {
                yield return new TestCaseData(new Complex(1.0, 0));
                yield return new TestCaseData(new Complex(2.3, -61));
                yield return new TestCaseData(new Complex(1, 0));
                yield return new TestCaseData(new Complex(2.5, 3.5));
                yield return new TestCaseData(new Complex(2.5, 3.5));
            }
        }
        #endregion
    }
}