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
using System.Numerics;

using NUnit.Framework;

namespace Dodoni.MathLibrary.Basics.LowLevel
{
    public abstract partial class IHypergeometricSpecialFunctionsTests
    {
        /// <summary>A test function for '1_F_1'.
        /// </summary>
        /// <param name="a">The first argument.</param>
        /// <param name="b">The second argument.</param>
        /// <param name="x">The third argument.</param>
        /// <param name="expected">The expected result.</param>
        [TestCase(2 / 3.0, 4 / 3.0, -0.75, 0.708168653667129617617947, TestName = "Hypergeometric_1F1_LukeExample_ReferenceResults", Description = "Taken from 'Algorithms for the computation of mathematical functions', Y. L. Luke, 1977")]
        [TestCase(2.0, 3.0, 0.5, 1.405114917199487412605, Description = "Taken from 'www.klisan.casio.com/exec/system/1349143651'")]
        [TestCase(1.25, 2.7, 1.2, 1.829670146765415376952, Description = "Taken from 'www.klisan.casio.com/exec/system/1349143651'")]
        [TestCase(-1.67, 2.89, 1.96, 0.0680823479035134571217, Description = "Taken from 'www.klisan.casio.com/exec/system/1349143651'")]
        [TestCase(-4.65, 24.4, -2.6, 1.596426604090693156138, Description = "Taken from 'www.klisan.casio.com/exec/system/1349143651'")]
        [TestCase(-3.0, 5.7, -7.8, 11.49830138439722100366, Description = "Taken from 'www.klisan.casio.com/exec/system/1349143651'")]
        [TestCase(0.1, 0.2, 0.5, 1.317627178278510, Description = "Taken from 'Computation of Hypergeometric Functions' by John Pearson, University of Oxford, September 2009")]
        [TestCase(-0.1, 0.2, 0.5, 0.695536565102261, Description = "Taken from 'Computation of Hypergeometric Functions' by John Pearson, University of Oxford, September 2009")]
        [TestCase(1E-8, 1E-8, 1E-10, 1.000000000100000, Description = "Taken from 'Computation of Hypergeometric Functions' by John Pearson, University of Oxford, September 2009")]
        [TestCase(1.0, 3.0, 10.0, 4.403093158961343E2, Description = "Taken from 'Computation of Hypergeometric Functions' by John Pearson, University of Oxford, September 2009")]
        [TestCase(500.0, 511.0, 10.0, 1.779668553337393251718E+4, Description = "(typo in the reference) Taken from 'Computation of Hypergeometric Functions' by John Pearson, University of Oxford, September 2009")]
        //        [TestCase(8.1, 10.1, 100.0, 1.724131075992688E41)]  // for the rational approximation the result of this test case is not accurate
        public void One_F_One_TestParameters_BenchmarkResults(double a, double b, double x, double expected)
        {
            var specialFunctions = GetTestObject();
            var actual = specialFunctions.One_F_One(a, b, x);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-9).Percent);
        }

        /// <summary>A test function for '1_F_1' with complex arguments.
        /// </summary>
        /// <param name="aReal">The real component of the first argument.</param>
        /// <param name="aImaginary">The imaginary component of the first argument.</param>
        /// <param name="bReal">The real component of the second argument.</param>
        /// <param name="bImaginary">The imaginary component of the second argument.</param>
        /// <param name="zReal">The real component of the third argument.</param>
        /// <param name="zImaginary">The imaginary component of the third argument.</param>
        /// <param name="expectedReal">The real component of the expected result.</param>
        /// <param name="expectedImaginary">The imaginary component of the expected result.</param>
        [TestCase(2 / 3.0, 0.0, 4 / 3.0, 0.0, -0.75, 0.0, 0.708168653667129617617947, 0.0, TestName = "Hypergeometric_1F1_LukeExample_ReferenceResults", Description = "Taken from 'Algorithms for the computation of mathematical functions', Y. L. Luke, 1977")]
        [TestCase(2.0, 0.0, 3.0, 0.0, 0.5, 0.0, 1.405114917199487412605, 0.0, Description = "Taken from 'www.klisan.casio.com/exec/system/1349143651'")]
        [TestCase(1.25, 0.0, 2.7, 0.0, 1.2, 0.0, 1.829670146765415376952, 0.0, Description = "Taken from 'www.klisan.casio.com/exec/system/1349143651'")]
        [TestCase(-1.67, 0.0, 2.89, 0.0, 1.96, 0.0, 0.0680823479035134571217, 0.0, Description = "Taken from 'www.klisan.casio.com/exec/system/1349143651'")]
        [TestCase(-4.65, 0.0, 24.4, 0.0, -2.6, 0.0, 1.596426604090693156138, 0.0, Description = "Taken from 'www.klisan.casio.com/exec/system/1349143651'")]
        [TestCase(-3.0, 0.0, 5.7, 0.0, -7.8, 0.0, 11.49830138439722100366, 0.0, Description = "Taken from 'www.klisan.casio.com/exec/system/1349143651'")]
        [TestCase(-1.52, 0.0, 3.26, 0.0, -1.27, 5.81, 0.80656423556974320483, -3.2228061050305956394, Description = "Taken from 'www.klisan.casio.com/exec/system/1349143651'")]
        [TestCase(0.1, 0.0, 0.2, 0.0, 0.5, 0.0, 1.317627178278510, 0.0, Description = "Taken from 'Computation of Hypergeometric Functions' by John Pearson, University of Oxford, September 2009")]
        [TestCase(-0.1, 0.0, 0.2, 0.0, 0.5, 0.0, 0.695536565102261, 0.0, Description = "Taken from 'Computation of Hypergeometric Functions' by John Pearson, University of Oxford, September 2009")]
        [TestCase(0.1, 0.0, 0.2, 0.0, -0.5, 1.0, 0.667236640109150, 0.274769720129335, Description = "Taken from 'Computation of Hypergeometric Functions' by John Pearson, University of Oxford, September 2009")]
        [TestCase(1.0, 1.0, 1.0, 1.0, 1.0, -1.0, 1.468693939915885, -2.287355287178842, Description = "Taken from 'Computation of Hypergeometric Functions' by John Pearson, University of Oxford, September 2009")]
        [TestCase(1E-8, 0.0, 1E-8, 0.0, 1E-10, 0.0, 1.000000000100000, 0.0, Description = "Taken from 'Computation of Hypergeometric Functions' by John Pearson, University of Oxford, September 2009")]
        [TestCase(1E-8, 0.0, 1E-12, 0.0, -1E-10, 1E-12, 0.999999000000000, 0.000000010000000, Description = "Taken from 'Computation of Hypergeometric Functions' by John Pearson, University of Oxford, September 2009")]
        [TestCase(1.0, 0.0, 3.0, 0.0, 10.0, 0.0, 4.403093158961343E2, 0.0, Description = "Taken from 'Computation of Hypergeometric Functions' by John Pearson, University of Oxford, September 2009")]
        [TestCase(500.0, 0.0, 511.0, 0.0, 10.0, 0.0, 1.779668553337393E+4, 0.0, Description = "(typo in the reference) Taken from 'Computation of Hypergeometric Functions' by John Pearson, University of Oxford, September 2009")]

        /* the following test cases are not suitable to the implementation via rational approximation */
        //        [TestCase(1.0, 0.0, 1.0, 0.0, 10.0, 1E-9, 2.202646579480672E+4, 2.02646579480672E-5, Description = "(typo in the reference) Taken from 'Computation of Hypergeometric Functions' by John Pearson, University of Oxford, September 2009")]
        //        [TestCase(8.1, 0.0, 10.1, 0.0, 100.0, 0.0, 1.724131075992688E41, 0.0)]
        //        [TestCase(1.0, 0.0, 2.0, 0.0, 600.0, 0.0, 6.288367168216566E257, 0.0)]
        //        [TestCase(2.0, 8.0, -150.0, 1.0, 150.0, 0.0, -9.853780031496243E135, 3.29388896210013E136)]
        [TestCase(20.0, 0.0, 10.0, 1000.0, -5.0, 0.0, 0.993763703678828, 0.099687801957356)]
        public void One_F_One_TestParameters_BenchmarkResults(double aReal, double aImaginary, double bReal, double bImaginary, double zReal, double zImaginary, double expectedReal, double expectedImaginary)
        {
            var specialFunctions = GetTestObject();
            var actual = specialFunctions.One_F_One(aReal + Complex.ImaginaryOne * aImaginary, bReal + Complex.ImaginaryOne * bImaginary, zReal + Complex.ImaginaryOne * zImaginary);

            if (expectedReal == 0.0)
            {
                Assert.That(actual.Real, Is.EqualTo(expectedReal).Within(1E-8));
            }
            else
            {
                Assert.That(actual.Real, Is.EqualTo(expectedReal).Within(1E-8).Percent);
            }

            if (expectedImaginary == 0.0)
            {
                Assert.That(actual.Imaginary, Is.EqualTo(expectedImaginary).Within(1E-8));
            }
            else
            {
                Assert.That(actual.Imaginary, Is.EqualTo(expectedImaginary).Within(1E-8).Percent);
            }
        }
    }
}