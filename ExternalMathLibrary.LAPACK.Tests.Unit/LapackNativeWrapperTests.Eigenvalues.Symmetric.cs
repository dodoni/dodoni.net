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

namespace Dodoni.MathLibrary.Basics.LowLevel.Native
{
    public partial class LapackNativeWrapperTests
    {
        /// <summary>A test function for <c>zhetrd</c>.
        /// </summary>
        /// <remarks>The test case data is taken from http://www.nag.co.uk/numeric/fl/manual/pdf/F08/f08fsf.pdf .</remarks>
        [TestCase]
        public void zhetrd_TestCaseData_Benchmark()
        {
            int n = 4;

            var d = new double[n];
            var e = new double[n - 1];
            var tau = new Complex[n - 1];

            var native = new LapackNativeWrapper();
            int lwork = native.EigenValues.Symmetric.zhetrdQuery(n);
            var a = new Complex[] {
                -2.28,                             1.78+2.03*Complex.ImaginaryOne,2.26-0.1*Complex.ImaginaryOne,-0.12-2.53*Complex.ImaginaryOne,
                 1.78 - 2.03*Complex.ImaginaryOne, -1.12, 0.01-0.43*Complex.ImaginaryOne,-1.07-0.86*Complex.ImaginaryOne,
                 2.26 + 0.1*Complex.ImaginaryOne,  0.01+0.43*Complex.ImaginaryOne,-0.37,2.31 + 0.92*Complex.ImaginaryOne,
                -0.12 + 2.53*Complex.ImaginaryOne, -1.07+0.86*Complex.ImaginaryOne, 2.31 - 0.92 * Complex.ImaginaryOne, -0.73};


            var work = new Complex[lwork];
            native.EigenValues.Symmetric.zhetrd(n, a, d, e, tau, work, BLAS.TriangularMatrixType.LowerTriangularMatrix);

            var expectedDiagonal = new[] { -2.28, -0.1285, -0.1666, -1.9249 };

            Assert.That(d, Is.EqualTo(expectedDiagonal).AsCollection.Within(1E-3));

            var expectedOffDiagonal = new[] { -4.3385, -2.0226, -1.8023 };
            Assert.That(e, Is.EqualTo(expectedOffDiagonal).AsCollection.Within(1E-3));
        }
    }
}