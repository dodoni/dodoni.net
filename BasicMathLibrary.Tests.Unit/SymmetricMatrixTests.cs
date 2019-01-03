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

namespace Dodoni.MathLibrary
{
    /// <summary>Serves as unit test class for <see cref="SymmetricMatrix"/>.
    /// </summary>
    public class SymmetricMatrixTests
    {
        /// <summary>A test function for <c>Indexed properties</c>.
        /// </summary>       
        [TestCase]
        public void IndexedProperties_TestCaseData_InputData()
        {
            int n = 3;
            var data = new double[] { 1, 2, 3, 4, 5, 6 };

            var symmetricMatrix = new SymmetricMatrix(n, data);

            Assert.That(symmetricMatrix[0, 0], Is.EqualTo(1.0).Within(1E-9));
            Assert.That(symmetricMatrix[1, 0], Is.EqualTo(2.0).Within(1E-9));
            Assert.That(symmetricMatrix[2, 0], Is.EqualTo(3.0).Within(1E-9));

            Assert.That(symmetricMatrix[0, 1], Is.EqualTo(2.0).Within(1E-9));
            Assert.That(symmetricMatrix[1, 1], Is.EqualTo(4.0).Within(1E-9));
            Assert.That(symmetricMatrix[2, 1], Is.EqualTo(5.0).Within(1E-9));


            Assert.That(symmetricMatrix[0, 2], Is.EqualTo(3.0).Within(1E-9));
            Assert.That(symmetricMatrix[1, 2], Is.EqualTo(5.0).Within(1E-9));
            Assert.That(symmetricMatrix[2, 2], Is.EqualTo(6.0).Within(1E-9));
        }

        /// <summary>A test function for <c>ToDenseMatrix</c>.
        /// </summary>       
        [TestCase]
        public void ToDenseMatrix_TestCaseData_InputData()
        {
            int n = 3;
            var data = new double[] { 1, 2, 3, 4, 5, 6 };

            var expected = new SymmetricMatrix(n, data);

            var actual = expected.ToDenseMatrix();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Assert.That(expected[i, j], Is.EqualTo(actual[i, j]).Within(1E-9));
                }
            }
        }
    }
}