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
    /// <summary>Serves as unit test class for <see cref="DiagonalMatrix"/>.
    /// </summary>
    [TestFixture]
    public class DiagonalMatrixTests
    {
        /// <summary>A test function for <c>operator *</c>.
        /// </summary>       
        [TestCase]
        public void OperatorLeftMultiplication_TestCaseData1_BenchmarkResults()
        {
            var a = new DenseMatrix(3, 2, new double[] { 1, 2, 3, 4, 5, 6 });
            var d = new DiagonalMatrix(3, new double[] { 1, 2, 3 });

            var actual = d * a;

            var denseA = a.ToDenseMatrix();
            var denseD = d.ToDenseMatrix();

            var expected = denseD * denseA;

            Assert.That(actual.RowCount, Is.EqualTo(expected.RowCount));
            Assert.That(actual.ColumnCount, Is.EqualTo(expected.ColumnCount));

            for (int i = 0; i < expected.RowCount; i++)
            {
                for (int j = 0; j < expected.ColumnCount; j++)
                {
                    Assert.That(actual[i, j], Is.EqualTo(expected[i, j]).Within(1E-9));
                }
            }
        }

        /// <summary>A test function for <c>operator *</c>.
        /// </summary>       
        [TestCase]
        public void OperatorLeftMultiplication_TestCaseData2_BenchmarkResults()
        {
            var a = new DenseMatrix(3, 2, new double[] { 1, 2, 3, 4, 5, 6 });
            var d = new DiagonalMatrix(2, new double[] { 1, 2 });

            var actual = d * a.T;

            var denseA = a.T.ToDenseMatrix();
            var denseD = d.ToDenseMatrix();

            var expected = denseD * denseA;

            Assert.That(actual.RowCount, Is.EqualTo(expected.RowCount));
            Assert.That(actual.ColumnCount, Is.EqualTo(expected.ColumnCount));

            for (int i = 0; i < expected.RowCount; i++)
            {
                for (int j = 0; j < expected.ColumnCount; j++)
                {
                    Assert.That(actual[i, j], Is.EqualTo(expected[i, j]).Within(1E-9));
                }
            }
        }

        /// <summary>A test function for <c>operator *</c>.
        /// </summary>       
        [TestCase]
        public void OperatorRightMultiplication_TestCaseData1_BenchmarkResults()
        {
            var a = new DenseMatrix(3, 2, new double[] { 1, 2, 3, 4, 5, 6 });
            var d = new DiagonalMatrix(2, new double[] { 1.2, 3 });

            var actual =  a * d;

            var denseA = a.ToDenseMatrix();
            var denseD = d.ToDenseMatrix();

            var expected = denseA * denseD;

            Assert.That(actual.RowCount, Is.EqualTo(expected.RowCount));
            Assert.That(actual.ColumnCount, Is.EqualTo(expected.ColumnCount));

            for (int i = 0; i < expected.RowCount; i++)
            {
                for (int j = 0; j < expected.ColumnCount; j++)
                {
                    Assert.That(actual[i, j], Is.EqualTo(expected[i, j]).Within(1E-9));
                }
            }
        }

        /// <summary>A test function for <c>operator *</c>.
        /// </summary>       
        [TestCase]
        public void OperatorRightMultiplication_TestCaseData2_BenchmarkResults()
        {
            var a = new DenseMatrix(3, 2, new double[] { 1, 2, 3, 4, 5, 6 });
            var d = new DiagonalMatrix(3, new double[] { 1.1, 2, 3 });

            var actual = a.T * d;

            var denseA = a.T.ToDenseMatrix();
            var denseD = d.ToDenseMatrix();

            var expected = denseA * denseD;

            Assert.That(actual.RowCount, Is.EqualTo(expected.RowCount));
            Assert.That(actual.ColumnCount, Is.EqualTo(expected.ColumnCount));

            for (int i = 0; i < expected.RowCount; i++)
            {
                for (int j = 0; j < expected.ColumnCount; j++)
                {
                    Assert.That(actual[i, j], Is.EqualTo(expected[i, j]).Within(1E-9));
                }
            }
        }
    }
}