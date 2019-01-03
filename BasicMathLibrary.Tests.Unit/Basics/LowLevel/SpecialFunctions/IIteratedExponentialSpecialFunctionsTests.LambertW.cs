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
using System.Numerics;

using NUnit.Framework;

namespace Dodoni.MathLibrary.Basics.LowLevel
{
    public abstract partial class IIteratedExponentialSpecialFunctionsTests
    {
        /// <summary>A test function for real arguments on the Principal branch.
        /// </summary>
        /// <param name="x">The argument x of W(x).</param>
        [Test]
        public void LambertW_RealArgument_FunctionalEquation(
            [Values(1.0, 1.63, 2.831231313, -0.23441312, 7.9817231123, -0.1234567891011, 24.6152441235, 65.9823477234, 126.716265123, 1023.928242474247, -0.36787944117144)] double x)
        {
            var specialFunctions = GetTestObject();
            Assume.That(x, Is.GreaterThanOrEqualTo(-1 / Math.E));

            var w = specialFunctions.LambertW(x);

            var actual = w * Math.Exp(w);
            var expected = x;

            Assert.That(actual, Is.EqualTo(expected).Within(1E-12).Percent, String.Format("x={0}; w ={1}; w * exp(w) = {2} is not equal to x!", x, w, w * Math.Exp(w)));

            if (x < 0) // check whether in Principal branch
            {
                Assert.That(w, Is.GreaterThanOrEqualTo(-1.0), "Not the Principal branch!");
            }
        }

        /// <summary>A test function for real arguments on the Principal branch applied to the complex function implementation.
        /// </summary>
        /// <param name="x">The argument x of W(x).</param>
        [Test]
        public void LambertW_ComplexPrincipalBranch_RealArgument_FunctionalEquation(
            [Values(1.0, 1.63, 2.831231313, -0.23441312, 7.9817231123, -0.1234567891011, 24.6152441235, 65.9823477234, 126.716265123, 1023.928242474247, -0.36787944117144)] double x)
        {
            var specialFunctions = GetTestObject();

            var w = specialFunctions.LambertW(x + 0 * Complex.ImaginaryOne, branch: 0);

            var actual = w * Complex.Exp(w);
            var expected = x;

            Assert.That(actual.Real, Is.EqualTo(expected).Within(1E-12).Percent, String.Format("x={0}; w ={1}; w * exp(w) = {2} is not equal to x!", x, w, w * Complex.Exp(w)));
            Assert.That(actual.Imaginary, Is.EqualTo(0.0).Within(1E-12));

            if (x < 0) // check whether in Principal branch, i.e. >= -1.0
            {
                Assert.That(w.Real, Is.GreaterThanOrEqualTo(-1.0), "Not in Principal branch!");
            }
        }

        /// <summary>A test function for complex arguments checking whether the functional equation is fullfilled.
        /// </summary>
        /// <param name="x">The real component of the argument z of W(z).</param>
        /// <param name="y">The imaginary component of the argument z of W(z).</param>
        /// <param name="branch">The index of the branch.</param>
        [Test]
        public void LambertW_ComplexArgument_FunctionalEquation(
            [Values(1.0, 1.63, 2.831231313, -0.23441312, 7.9817231123, -0.1234567891011, 24.6152441235, 65.9823477234, 126.716265123, 1023.928242474247, -0.36787944117144)] 
            double x,
            [Values(1.0, -0.52, 2.541, 5.126, -7.89)]
            double y,
            [Values(1, 2, 3, 5, 10, 0, -1, -2, -3, -5, -10)]
            int branch)
        {
            var specialFunctions = GetTestObject();

            var w = specialFunctions.LambertW(x + y * Complex.ImaginaryOne, branch);

            var actual = w * Complex.Exp(w);
            var expected = x + y * Complex.ImaginaryOne;

            Assert.That(actual.Real, Is.EqualTo(expected.Real).Within(1E-10).Percent, String.Format("x={0}; w ={1}; w * exp(w) = {2} is not equal to x!", x, w, w * Complex.Exp(w)));
            Assert.That(actual.Imaginary, Is.EqualTo(expected.Imaginary).Within(1E-9).Percent, String.Format("x={0}; w ={1}; w * exp(w) = {2} is not equal to x!", x, w, w * Complex.Exp(w)));
        }

        /// <summary>A test function for real arguments on the Principal branch.
        /// </summary>
        /// <param name="x">The argument x of W(x).</param>
        /// <param name="expected">The expected value of W(x).</param>
        /// <remarks>The benchmark values are taken from http://functions.wolfram.com/webMathematica ProductLog[z].</remarks>
        [TestCase(-1.0 / Math.E, -1.0)]
        [TestCase(0.0, 0.0)]
        [TestCase(Math.E, 1.0)]
        [TestCase(-0.35, -0.716638816456074)]
        [TestCase(-0.21, -0.277034493696194)]
        [TestCase(0.123, 0.110168980643260)]
        [TestCase(0.891, 0.526359668322806)]
        [TestCase(2.0 * Math.E * Math.E, 2.0)]
        [TestCase(2.25, 0.907734052843247)]
        [TestCase(13.67, 1.94826461527652)]
        public void LambertW_RealArgument_BenchmarkResult(double x, double expected)
        {
            var specialFunctions = GetTestObject();
            var actual = specialFunctions.LambertW(x);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-12).Percent, String.Format("x={0}; w ={1}; w * exp(w) = {2} is not equal to x!", x, expected, actual * Math.Exp(actual)));
        }

        /// <summary>A test function for the GetValue method.
        /// </summary>
        /// <param name="x">The real component of the argument z of W(z).</param>
        /// <param name="y">The imaginary component of the argument z of W(z).</param>
        /// <param name="branch">The index of the branch.</param>
        /// <param name="expectedReal">The expected real part of W(z).</param>
        /// <param name="expectedImaginary">The expected imaginary part of W(z).</param>
        /// <remarks>Some benchmark values are analytic results, other are taken from http://functions.wolfram.com/webMathematica ProductLog[k,z], from http://www.getreuer.info/home/lambertw with a cross-check using Octave and package "specfun".</remarks>
        [TestCase(-Math.PI / 2, 0, -1, 0.0, -Math.PI / 2.0)]
        [TestCase(-1.0 / Math.E, 0.0, -1, -1.0, 0.0)]
        [TestCase(-1.5, 0.0, 0, -0.0327837359155725, 1.5496438233501593)]
        [TestCase(1.0, 0.0, -4, -3.16295273880408, -23.42774750375521)]
        [TestCase(1.0, 0.0, -3, -2.85358175540904, -17.11353553941215)]
        [TestCase(1.0, 0.0, -2, -2.40158510486800, -10.77629951611507)]
        [TestCase(1.0, 0.0, -1, -1.53391331979357, -4.37518515306190)]
        [TestCase(1.0, 0.0, 0, 0.567143290409784, 0.0)]
        [TestCase(1.0, 0.0, 1, -1.53391331979357, 4.37518515306190)]
        [TestCase(1.0, 0.0, 2, -2.40158510486800, 10.77629951611507)]
        [TestCase(1.0, 0.0, 3, -2.85358175540904, 17.11353553941215)]
        [TestCase(1.0, 0.0, 4, -3.16295273880408, 23.42774750375521)]
        [TestCase(1.5, 3.25, 4, -1.93149911985448, 24.62204785722477)]
        [TestCase(-2.7, 5.91, -2, -0.322026389140745, -8.960316169616036)]
        public void LambertW_ComplexArgument_BenchmarkResult(double x, double y, int branch, double expectedReal, double expectedImaginary)
        {
            var specialFunctions = GetTestObject();
            var w = specialFunctions.LambertW(x + Complex.ImaginaryOne * y, branch);

            var actual = w;

            if (expectedReal == 0.0)  // do not check relative errors
            {
                Assert.That(actual.Real, Is.EqualTo(expectedReal).Within(1E-12), String.Format("x={0}; w ={1}; w * exp(w) = {2}", x, w, w * Complex.Exp(w)));
            }
            else
            {
                Assert.That(actual.Real, Is.EqualTo(expectedReal).Within(1E-12).Percent, String.Format("x={0}; w ={1}; w * exp(w) = {2}", x, w, w * Complex.Exp(w)));
            }

            if (expectedImaginary == 0.0)  // do not check relative errors
            {
                Assert.That(actual.Imaginary, Is.EqualTo(expectedImaginary).Within(1E-12), String.Format("x={0}; w ={1}; w * exp(w) = {2}", x, w, w * Complex.Exp(w)));
            }
            else
            {
                Assert.That(actual.Imaginary, Is.EqualTo(expectedImaginary).Within(1E-12).Percent, String.Format("x={0}; w ={1}; w * exp(w) = {2}", x, w, w * Complex.Exp(w)));
            }
        }
    }
}