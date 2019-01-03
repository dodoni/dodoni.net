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
using System.Collections.Generic;

using NUnit.Framework;

using Dodoni.MathLibrary.Basics;

namespace Dodoni.MathLibrary.ProbabilityTheory.Distributions
{
    /// <summary>Serves as unit test class for <see cref="StandardNormalDistribution"/>.
    /// </summary>
    /// <remarks>The implementation of the normal (inverse) cummulative distribution function depends on <see cref="SpecialFunction.PrimitiveIntegral"/>, i.e.
    /// rather a integration test than a unit test.</remarks>
    public partial class StandardNormalDistributionTests
    {
        /// <summary>A test function for the standard cummulative distribution function.
        /// </summary>
        /// <param name="x">The value where to evaluate.</param>
        [Test]
        public void GetCdfValue_TestCase_GenzBenchmarkResult([Range(-5.0, 5.0, 0.1)] double x)
        {
            double actual = StandardNormalDistribution.GetCdfValue(x);
            double expected = Benchmark.GetStandardCdfValue(x);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-14));
        }

        /// <summary>A test function for the standard cummulative distribution function, i.e. apply the inverse cummulative distribution function first and then the cummulative distribution function.
        /// </summary>
        /// <param name="probability">The probability where to evaluate the standard inverse cummulative distribution function.</param>
        [Test]
        public void GetCdfValue_ResultOfInverseCdfValue_InputValue([Range(0.0, 1.0, 0.01)] double probability)
        {
            Assume.That(probability, Is.LessThanOrEqualTo(1.0));
            double expected = probability;

            double x = StandardNormalDistribution.GetInverseCdfValue(probability);
            double actual = StandardNormalDistribution.GetCdfValue(x);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-9));
        }

        /// <summary>A test function for the standard inverse cummulative distribution function, i.e. apply the cummulative distribution function first and then the inverse function.
        /// </summary>
        /// <param name="x">The value where to evaluate the cummulative distribution function.</param>
        [Test]
        public void GetInverseCdfValue_ResultOfCdfValue_InputValue([Range(-5.0, 5.0, 0.1)] double x)
        {
            double expected = x;

            double probability = StandardNormalDistribution.GetCdfValue(x);
            double actual = StandardNormalDistribution.GetInverseCdfValue(probability);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-8));
        }

        /// <summary>A test function for the standard cummulative distribution function.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <param name="expected">The expected value.</param>
        /// <remarks>The benchmark values are taken from http://keisan.casio.com/calculator. </remarks>
        [TestCase(-15.5, 1.734460791793870051340447592663711906486504785289E-54)]
        [TestCase(-12.0, 1.776482112077678997696171001845557092392666434179E-33)]
        [TestCase(-10.0, 7.619853024160526065973343251599308363504033277957E-24)]
        [TestCase(-5.5, 1.8989562465887719383851274033580186316357489119297E-8)]
        [TestCase(-1.75, 0.04005915686381709041875734988564191093727336722944)]
        [TestCase(-0.5, 0.30853753872598689636229538939166226011639782444542)]
        [TestCase(0.0, 0.5)]
        [TestCase(0.77, 0.77935005365735038875603935732888368726352806688898)]
        [TestCase(1.42, 0.92219615947345360638960043253444727846811578859063)]
        [TestCase(2.25, 0.98777552734495529684737606870025850747583521678246)]
        [TestCase(6.25, 0.99999999979477365747810611183772364042083616085379)]
        [TestCase(10.10, 9.9999999999999999999999723789052823549321707976675E-1)]
        public void GetCdfValue_TestCase_BenchmarkResult(double x, double expected)
        {
            double actual = StandardNormalDistribution.GetCdfValue(x);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-14));
        }

        /// <summary>A test function for the standard inverse cummulative distribution function.
        /// </summary>
        /// <param name="probability">The argument.</param>
        /// <param name="expected">The expected value.</param>
        /// <remarks>The benchmark values are taken from http://keisan.casio.com/calculator. </remarks>
        [TestCase(0.0000001, -5.1993375821928169315873472669623368665097371602387E+0)]
        [TestCase(0.00001, -4.2648907939228246284985246989063446293560532226955E+0)]
        [TestCase(0.099, -1.287270563107941499589784183413351575459538527257E+0)]
        [TestCase(0.1234, -1.1581569325527092227331463447127110632388477509581E+0)]
        [TestCase(0.3781, -3.1047469477587946060951972232528362979390622143463E-1)]
        [TestCase(0.5, 0.0)]
        [TestCase(0.50000001, 2.50662827463100076490926438018476538318647277E-8)]
        [TestCase(0.79181, 8.1271757916548852862138186644825108261214081662408E-1)]
        [TestCase(0.9, 1.2815515655446004669651033294487428186199078243526E+0)]
        [TestCase(0.999, 3.0902323061678135415403998301073792054910084918658E+0)]
        [TestCase(0.9999999999, 6.3613409024040562046953758282652216792039373509158E+0)]
//        [TestCase(0.99999999999999999, 8.4937932241095980744447188132289548161213991737094E+0)]
        public void GetInverseCdfValue_TestCase_BenchmarkResult(double probability, double expected)
        {
            double actual = StandardNormalDistribution.GetInverseCdfValue(probability);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-7));  // low tolerance only!
        }
    }
}