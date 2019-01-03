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

using NUnit.Framework;

namespace Dodoni.MathLibrary.Basics.LowLevel.Native
{
    public partial class LapackNativeWrapperTests
    {
        /// <summary>A test function for <c>driver_dgeesQuery</c>.
        /// </summary>
        /// <remarks>The test case data is taken from http://www.wrgrid.group.shef.ac.uk/icebergdocs/nagdocs/NAG23doc/pdf/F08/f08paf.pdf, the sign of the resulting matrices are different.</remarks>
        [TestCase]
        public void driver_dgees_TestCaseData_BenchmarkResult()
        {
            int n = 4;
            double[] a =
                new[]{0.35,  0.09, -0.44, 0.25,
                      0.45,  0.07, -0.33, -0.32,
                     -0.14, -0.54, -0.03, -0.13,
                     -0.17,  0.35,  0.17,  0.11};

            int lwork = LAPACK.EigenValues.NonSymmetric.driver_dgeesQuery(LapackEigenvalues.NonSymmetricSchurVectorsJob.Compute, LapackEigenvalues.NonSymmetricEigenvalueOrdering.Sorted, n);

            var wr = new double[n];
            var wi = new double[n];
            var vs = new double[n * n];
            var bwork = new bool[n];
            var work = new double[lwork];
            int sdim;

            LAPACK.EigenValues.NonSymmetric.driver_dgees(LapackEigenvalues.NonSymmetricSchurVectorsJob.Compute, LapackEigenvalues.NonSymmetricEigenvalueOrdering.Sorted, (x, y) => y == 0.00, n, a, out sdim, wr, wi, vs, work, bwork);

            Assert.That(sdim, Is.EqualTo(2));

            var exepectedMatrixT = new[] { 0.7995, 0, 0, 0, -0.0059, -0.1007, 0, 0, -0.0751, 0.3937, -0.0994, 0.3132, -0.0927, 0.3569, -0.5128, -0.0994 };
            Assert.That(a, Is.EqualTo(exepectedMatrixT).AsCollection.Within(1E-3));

            var exepectedMatrixZ = new[] { -0.6551, -0.5236, 0.5362, -0.0956, -0.1210, -0.3286, -0.5974, -0.7215, -0.5032, 0.7857, 0.0904, -0.3482, 0.5504, 0.0229, 0.5894, -0.5908 };
            Assert.That(vs, Is.EqualTo(exepectedMatrixZ).AsCollection.Within(1E-3));
        }
    }
}