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
using NUnit.Framework;

namespace Dodoni.MathLibrary.Basics.LowLevel.Native
{
    public partial class LapackNativeWrapperTests
    {
        /// <summary>A test function that compares the result of 'DGTTRF' to the benchmark implementation.
        /// </summary>
        /// <remarks>The example has been taken from http://www.nag.co.uk/numeric/fl/manual/pdf/F07/f07cdf.pdf. </remarks>
        [Test]
        public void dgttrf_TestCaseData_BenchmarkResult()
        {
            int n = 5;
            var diagonalElements = new[] { 3.0, 2.3, -5, -0.9, 7.1 };
            var subDiagonalElements = new[] { 3.4, 3.6, 7.0, -6.0 };
            var superDiagonalElements = new[] { 2.1, -1.0, 1.9, 8.0 };
            var secondSuperDiagonalElements = new double[n];
            var pivotIndices = new int[n];

            var native = new LapackNativeWrapper();
            native.LinearEquations.MatrixFactorization.dgttrf(n, subDiagonalElements, diagonalElements, superDiagonalElements, secondSuperDiagonalElements, pivotIndices);

            var expectedPivotIndices = new[] { 2, 3, 4, 5, 5 };
            var expectedDiagonalElementsOfU = new[] { 3.4000, 3.6000, 7.0000, -6.0000, -1.0154 };
            var expectedMultipliers = new[] { 0.8824, 0.0196, 0.1401, -0.0148 };

            Assert.That(pivotIndices, Is.EqualTo(expectedPivotIndices).AsCollection);
            Assert.That(diagonalElements, Is.EqualTo(expectedDiagonalElementsOfU).AsCollection.Within(1E-3));
            Assert.That(subDiagonalElements, Is.EqualTo(expectedMultipliers).AsCollection.Within(1E-3));
        }
    }
}