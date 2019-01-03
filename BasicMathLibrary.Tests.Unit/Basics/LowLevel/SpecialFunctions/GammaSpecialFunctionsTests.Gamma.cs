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

using NUnit.Framework;
using System.Numerics;
using System.Collections.Generic;

namespace Dodoni.MathLibrary.Basics.LowLevel
{
    public abstract partial class GammaSpecialFunctionsTests
    {
        /// <summary>A test function for real arguments, where the Benchmark are some pre-calculated numbers.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <param name="expected">The expected value.</param>
        [TestCase(-1.5, 2.3632718012073547030642233111215269103967326081631802)]
        [TestCase(-0.5, -3.544907701811032054596334966682290365595098912244773)]
        [TestCase(0.1, 9.5135076986687312858079798958252325009137161063903012)]
        [TestCase(0.2, 4.59084371199880305320, Description = "Taken from http://de.wikipedia.org/wiki/Gammafunktion")]
        [TestCase(0.25, 3.62560990822190831193, Description = "Taken from http://de.wikipedia.org/wiki/Gammafunktion")]
        [TestCase(5.5, 52.342777784553520181149008492418193679490132376114268)]
        public void GetValue_RealArgument_Benchmark(double x, double expected)
        {
            var gammaFunction = GetTestObject();
            var actual = gammaFunction.GetValue(x);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-10));
        }

        [Test]
        public void Factorial_Argument_GammaValue( 
            [Range(1,25)] int n)
        {
            var gammaFunction = GetTestObject();

            var actual = gammaFunction.Factorial[n];
            var expected = gammaFunction.GetValue(n - 1);
            Assert.That(actual, Is.EqualTo(expected).Within(1E-10));
        }

        /// <summary>A test function for real arguments, where the Benchmark is the implementation in §6.1 in "Numerical Recipies" (NR).</summary>
        /// <param name="x">The argument.</param>
        [Test]
        public void GetLogValue_RealArgument_NRBenchmark(
            [Values(2.3, 4.0, 4.1, Math.PI, 5.0, 5.123)]
            double x)
        {
            var gammaFunction = GetTestObject();
            var actual = gammaFunction.GetLogValue(x);
            var expected = GetLogGammaValue_NR(x);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-6));
        }

        // Gamma(20+17i) als UT hinzufügen!

        public void GetValue(Complex z, Complex expected)
        {
        }


     // taken from http://functions.wolfram.com/webMathematica/FunctionEvaluation.jsp?name=Gamma

        public IEnumerable<TestCaseData> ComplexTestCases
        {
            get
            {
                yield return new TestCaseData(new Complex(0, 1.0), new Complex(-0.154949828301810685124955130484, -0.498015668118356042713691117462));
                yield return new TestCaseData(new Complex(2.5, -1.25), new Complex(0.538717320887414713868351497419, 0.751602908491934480515975711628));
                yield return new TestCaseData(new Complex(3.75, 0.75), new Complex(0.0364574874786038189549052955605, 0.0118511913275024248344079819951));
            }
        }

 

        public void IncompleteGamma(double a, double z, double expected)

        {

        }

 

        public IEnumerable<TestCaseData> IncGammaTestCases

        {

            get

            {

                yield return new TestCaseData(0.5, 0.0, 1.77245385090551602729816748334);

                yield return new TestCaseData(0.5, 0.5, 0.562418231594407124279494957302);

            }

        }


        public void GetLogValue_ComplexArgument_Benchmark(Complex z, Complex expected)
        {
        }

        public void GetNormalizedIncompletedValue_RealArgument_Benchmark(double x, double expected)
        {
        }


        #region private members

        /// <summary>Returns the logarithm of \Gamma(x), where \Gamma is the gamma function.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <returns>The logarithm of the Gamma function at <paramref name="x"/>.</returns>
        /// <remarks>Based on § 6.1 of 'Numerical Recipies'.</remarks>
        private double GetLogGammaValue_NR(double x)
        {
            double y = x;
            double temp = x + 5.5;
            temp -= (x + 0.5) * Math.Log(temp);
            double ser = 1.000000000190015;


            /* We do not use the following code to avoid arrays:
             
            double[] sm_CoefficientsForGammaLn = { 76.18009172947146, -86.50532032941677, 24.01409824083091, 
                                                     -1.231739572450155, 0.1208650973866179e-2, -0.5395239384953e-5 };

            for (int j = 0; j <= 5; j++)
            {
                ser += sm_CoefficientsForGammaLn[j] / ++y;
            }
            */

            // therefore we use the following code:
            ser += 76.18009172947146 / ++y;
            ser -= 86.50532032941677 / ++y;
            ser += 24.01409824083091 / ++y;
            ser -= 1.231739572450155 / ++y;
            ser += 0.1208650973866179e-2 / ++y;
            ser -= 0.5395239384953e-5 / ++y;

            return -temp + Math.Log(2.5066282746310005 * ser / x);
        }
        #endregion        
    }
}