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
using Dodoni.MathLibrary.Basics;

namespace Dodoni.MathLibrary.Miscellaneous
{
    /// <summary>Serves as unit test class for analytical polynomial root finder approaches.
    /// </summary>
    [TestFixture]
    public class AnalyticalPolynomialRootFinderTests
    {
        /// <summary>A test function for <see cref="Polynomial.RootFinder.Analytical.GetRoots(double, double, double, double, double, out System.Numerics.Complex, out System.Numerics.Complex, out System.Numerics.Complex, out System.Numerics.Complex)"/>
        /// </summary>
        /// <param name="absoluteCoefficient">The absolute coefficient.</param>
        /// <param name="firstOrderCoefficient">The first order coefficient.</param>
        /// <param name="secondOrderCoefficient">The second order coefficient.</param>
        /// <param name="thirdOrderCoefficient">The third order coefficient.</param>
        /// <param name="fourthOrderCoefficient">The fourth order coefficient.</param>
        [TestCase(1, 2, 3, 4, 5)]
        [TestCase(1, 2, 3, 4, 0.0)]
        [TestCase(1, 2, 3, 0.0, 0.0)]
        public void GetRoots_TestCaseData_ZeroFunctionValue(double absoluteCoefficient, double firstOrderCoefficient, double secondOrderCoefficient, double thirdOrderCoefficient, double fourthOrderCoefficient)
        {
            Complex[] roots = new Complex[4];
            int rootCount = Polynomial.RootFinder.Analytical.GetRoots(absoluteCoefficient, firstOrderCoefficient, secondOrderCoefficient, thirdOrderCoefficient, fourthOrderCoefficient, out roots[0], out roots[1], out roots[2], out roots[3]);

            Assert.That(rootCount, Is.GreaterThan(0), "No root found for the specified polynomial!");

            var polynomial = Polynomial.Real.Create(absoluteCoefficient, firstOrderCoefficient, secondOrderCoefficient, thirdOrderCoefficient, fourthOrderCoefficient);
            var functionValues = roots.Select(z => polynomial.GetValue(z)).ToArray();

            for (int j = 0; j < rootCount; j++)
            {
                Assert.That(Complex.Abs(functionValues[j]), Is.EqualTo(0.0).Within(1E-8));
            }
        }

        /// <summary>A test function for <see cref="Polynomial.RootFinder.Analytical.GetRoots(Complex, Complex, Complex, Complex, out System.Numerics.Complex, out System.Numerics.Complex, out System.Numerics.Complex)"/>
        /// </summary>
        /// <param name="absoluteCoefficient">The absolute coefficient.</param>
        /// <param name="firstOrderCoefficient">The first order coefficient.</param>
        /// <param name="secondOrderCoefficient">The second order coefficient.</param>
        /// <param name="thirdOrderCoefficient">The third order coefficient.</param>
        [TestCaseSource(nameof(ComplexTestCaseData))]
        public void GetRoots_ComplexTestCaseData_ZeroFunctionValue(Complex absoluteCoefficient, Complex firstOrderCoefficient, Complex secondOrderCoefficient, Complex thirdOrderCoefficient)
        {
            Complex[] roots = new Complex[3];
            int rootCount = Polynomial.RootFinder.Analytical.GetRoots(absoluteCoefficient, firstOrderCoefficient, secondOrderCoefficient, thirdOrderCoefficient, out roots[0], out roots[1], out roots[2]);

            Assert.That(rootCount, Is.GreaterThan(0), "No root found for the specified polynomial!");

            var polynomial = Polynomial.Complex.Create(absoluteCoefficient, firstOrderCoefficient, secondOrderCoefficient, thirdOrderCoefficient);
            var functionValues = roots.Select(z => polynomial.GetValue(z)).ToArray();

            for (int j = 0; j < rootCount; j++)
            {
                Assert.That(Complex.Abs(functionValues[j]), Is.EqualTo(0.0).Within(1E-8));
            }
        }

        /// <summary>Gets the test case data for root calculation with respect to complex coefficients.
        /// </summary>
        /// <value>The test case data with complex coefficients.
        /// </value>
        public static IEnumerable<TestCaseData> ComplexTestCaseData
        {
            get
            {
                yield return new TestCaseData((Complex)1.0, (Complex)2.0, (Complex)3.0, (Complex)4.0);
                yield return new TestCaseData((Complex)1, (Complex)2, (Complex)3, (Complex)0.0);
                yield return new TestCaseData((Complex)1, (Complex)2, (Complex)0.0, (Complex)0.0);
            }
        }
    }
}