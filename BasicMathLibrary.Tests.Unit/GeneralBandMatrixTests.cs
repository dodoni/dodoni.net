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

namespace Dodoni.MathLibrary
{
    /// <summary>Serves as unit test class for <see cref="GeneralBandMatrix"/>.
    /// </summary>
    [TestFixture]
    public class GeneralBandMatrixTests
    {
        /// <summary>A test function for the indexer of a specific <see cref="GeneralBandMatrix"/> object.
        /// </summary>
        [Test]
        public void Indexer_TestCaseData1_ExpectedMatrixEntries()
        {
            var matrixEntries = new double[] { -999, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, -42.0, 11, 12, 13, 14, -88, 15, 16, -123, -456 };

            int subDiagonalCount = 2;
            int superDiagonalCount = 1;
            int rowCount = 4;
            int columnCount = 3;

            var actual = new GeneralBandMatrix(rowCount, columnCount, subDiagonalCount, superDiagonalCount, matrixEntries);

            var expected = new DenseMatrix(rowCount, columnCount, new double[] { 1, 2, 3, 0, 4, 5, 6, 7, 0, 8, 9, 10 });

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    Assert.That(actual[i, j], Is.EqualTo(expected[i, j]).Within(1E-8), String.Format("Error in position (rowIndex={0}; columnIndex={1}).", i, j));
                }
            }
        }

        /// <summary>A test function for the indexer of the transposed of a specific <see cref="GeneralBandMatrix"/> object.
        /// </summary>
        [Test]
        public void Indexer_TestCaseData1Transposed_ExpectedMatrixEntries()
        {
            var matrixEntries = new double[] { -999, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, -42.0, 11, 12, 13, 14, -88, 15, 16, -123, -456 };

            int subDiagonalCount = 2;
            int superDiagonalCount = 1;
            int rowCount = 4;
            int columnCount = 3;

            var actual = new GeneralBandMatrix(rowCount, columnCount, subDiagonalCount, superDiagonalCount, matrixEntries).T;

            var expected = new DenseMatrix(columnCount, rowCount, new double[] { 1, 4, 0, 2, 5, 8, 3, 6, 9, 0, 7, 10 });

            for (int i = 0; i < columnCount; i++)
            {
                for (int j = 0; j < rowCount; j++)
                {
                    Assert.That(actual[i, j], Is.EqualTo(expected[i, j]).Within(1E-8), String.Format("Error in position (rowIndex={0}; columnIndex={1}).", i, j));
                }
            }
        }

        /// <summary>A test function for <c>ToDenseMatrix</c>.
        /// </summary>
        /// <param name="rowCount">The number of rows.</param>
        /// <param name="columnCount">The number of columns.</param>
        /// <param name="subDiagonalCount">The number of sub-diagonals.</param>
        /// <param name="superDiagonalCount">The number of super-diagonals.</param>
        /// <param name="matrixEntries">The matrix entries.</param>
        [TestCase(4, 3, 2, 1, new double[] { -999, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, -42.0, 11, 12, 13, 14, -88, 15, 16, -123, -456 })]
        public void ToDenseMatrix_TestCaseData_IdenticalEntries(int rowCount, int columnCount, int subDiagonalCount, int superDiagonalCount, double[] matrixEntries)
        {
            var expected = new GeneralBandMatrix(rowCount, columnCount, subDiagonalCount, superDiagonalCount, matrixEntries);
            var actual = expected.ToDenseMatrix();

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    Assert.That(actual[i, j], Is.EqualTo(expected[i, j]).Within(1E-8), String.Format("Error in position (rowIndex={0}; columnIndex={1}).", i, j));
                }
            }
        }

        /// <summary>A test function for <c>ToDenseMatrix</c> applied to the result of the transposed of a specific <see cref="GeneralBandMatrix"/> object.
        /// </summary>
        /// <param name="rowCount">The number of rows.</param>
        /// <param name="columnCount">The number of columns.</param>
        /// <param name="subDiagonalCount">The number of sub-diagonals.</param>
        /// <param name="superDiagonalCount">The number of super-diagonals.</param>
        /// <param name="matrixEntries">The matrix entries.</param>
        [TestCase(4, 3, 2, 1, new double[] { -999, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, -42.0, 11, 12, 13, 14, -88, 15, 16, -123, -456 })]
        public void ToDenseMatrixOfTransposeMatrix_TestCaseData_IdenticalEntries(int rowCount, int columnCount, int subDiagonalCount, int superDiagonalCount, double[] matrixEntries)
        {
            var originalMatrix = new GeneralBandMatrix(rowCount, columnCount, subDiagonalCount, superDiagonalCount, matrixEntries);
            var expected = originalMatrix.T;

            var actual = originalMatrix.T.ToDenseMatrix();

            for (int i = 0; i < expected.RowCount; i++)
            {
                for (int j = 0; j < expected.ColumnCount; j++)
                {
                    Assert.That(actual[i, j], Is.EqualTo(expected[i, j]).Within(1E-8), String.Format("Error in position (rowIndex={0}; columnIndex={1}).", i, j));
                }
            }
        }

        /// <summary>A test function for <c>GetColumn</c>.
        /// </summary>
        /// <param name="columnIndex">The null-based column index.</param>
        [Test]
        public void GetColumn_TestCaseData1_CompareWithGetColumnOfDenseMatrix(
            [Values(0, 1, 2)] int columnIndex)
        {
            var matrixEntries = new double[] { -999, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, -42.0, 11, 12, 13, 14, -88, 15, 16, -123, -456 };

            int subDiagonalCount = 2;
            int superDiagonalCount = 1;
            int rowCount = 4;
            int columnCount = 3;

            var generalBandMatrix = new GeneralBandMatrix(rowCount, columnCount, subDiagonalCount, superDiagonalCount, matrixEntries);
            var actual = generalBandMatrix.GetColumn(columnIndex);

            var denseMatrix = new DenseMatrix(rowCount, columnCount, new double[] { 1, 2, 3, 0, 4, 5, 6, 7, 0, 8, 9, 10 });
            var expected = denseMatrix.GetColumn(columnIndex);

            Assert.That(actual.ColumnCount, Is.EqualTo(1));
            Assert.That(actual.RowCount, Is.EqualTo(rowCount));

            for (int i = 0; i < rowCount; i++)
            {
                Assert.That(actual[i, 0], Is.EqualTo(expected[i, 0]).Within(1E-8), String.Format("Error in position rowIndex={0}.", i));
            }
        }

        /// <summary>A test function for <c>GetColumn</c> applied to the transposed of a specific <see cref="GeneralBandMatrix"/> object.
        /// </summary>
        /// <param name="columnIndex">The null-based column index.</param>
        [Test]
        public void GetColumn_TestCaseData1Transposed_CompareWithGetColumnOfDenseMatrix(
            [Values(0, 1, 2, 3)] int columnIndex)
        {
            var matrixEntries = new double[] { -999, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, -42.0, 11, 12, 13, 14, -88, 15, 16, -123, -456 };

            int subDiagonalCount = 2;
            int superDiagonalCount = 1;
            int rowCount = 4;
            int columnCount = 3;

            var generalBandMatrix = new GeneralBandMatrix(rowCount, columnCount, subDiagonalCount, superDiagonalCount, matrixEntries).T;
            var actual = generalBandMatrix.GetColumn(columnIndex);

            var denseMatrix = new DenseMatrix(columnCount, rowCount, new double[] { 1, 4, 0, 2, 5, 8, 3, 6, 9, 0, 7, 10 });
            var expected = denseMatrix.GetColumn(columnIndex);

            Assert.That(actual.ColumnCount, Is.EqualTo(1));
            Assert.That(actual.RowCount, Is.EqualTo(columnCount));

            for (int i = 0; i < columnCount; i++)
            {
                Assert.That(actual[i, 0], Is.EqualTo(expected[i, 0]).Within(1E-8), String.Format("Error in position rowIndex={0}.", i));
            }
        }

        /// <summary>A test function for <c>GetTransposedRow</c>.
        /// </summary>
        /// <param name="rowIndex">The null-based row index.</param>
        [Test]
        public void GetTransposedRow_TestCaseData1_CompareWithGetTransposedRowOfDenseMatrix(
            [Values(0, 1, 2, 3)] int rowIndex)
        {
            var matrixEntries = new double[] { -999, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, -42.0, 11, 12, 13, 14, -88, 15, 16, -123, -456 };

            int subDiagonalCount = 2;
            int superDiagonalCount = 1;
            int rowCount = 4;
            int columnCount = 3;

            var generalBandMatrix = new GeneralBandMatrix(rowCount, columnCount, subDiagonalCount, superDiagonalCount, matrixEntries);
            var actual = generalBandMatrix.GetTransposedRow(rowIndex);

            var denseMatrix = new DenseMatrix(rowCount, columnCount, new double[] { 1, 2, 3, 0, 4, 5, 6, 7, 0, 8, 9, 10 });
            var expected = denseMatrix.GetTransposedRow(rowIndex);

            Assert.That(actual.ColumnCount, Is.EqualTo(1));
            Assert.That(actual.RowCount, Is.EqualTo(columnCount));

            for (int i = 0; i < columnCount; i++)
            {
                Assert.That(actual[i, 0], Is.EqualTo(expected[i, 0]).Within(1E-8), String.Format("Error in position rowIndex={0}.", i));
            }
        }

        /// <summary>A test function for <c>GetTransposedRow</c> applied to the transposed of a specific <see cref="GeneralBandMatrix"/> object.
        /// </summary>
        /// <param name="rowIndex">The null-based row index.</param>
        [Test]
        public void GetTransposedRow_TestCaseData1Transposed_CompareWithGetTransposedRowOfDenseMatrix(
            [Values(0, 1, 2)] int rowIndex)
        {
            var matrixEntries = new double[] { -999, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, -42.0, 11, 12, 13, 14, -88, 15, 16, -123, -456 };

            int subDiagonalCount = 2;
            int superDiagonalCount = 1;
            int rowCount = 4;
            int columnCount = 3;

            var generalBandMatrix = new GeneralBandMatrix(rowCount, columnCount, subDiagonalCount, superDiagonalCount, matrixEntries).T;
            var actual = generalBandMatrix.GetTransposedRow(rowIndex);

            var denseMatrix = new DenseMatrix(columnCount, rowCount, new double[] { 1, 4, 0, 2, 5, 8, 3, 6, 9, 0, 7, 10 });
            var expected = denseMatrix.GetTransposedRow(rowIndex);

            Assert.That(actual.ColumnCount, Is.EqualTo(1));
            Assert.That(actual.RowCount, Is.EqualTo(rowCount));

            for (int i = 0; i < rowCount; i++)
            {
                Assert.That(actual[i, 0], Is.EqualTo(expected[i, 0]).Within(1E-8), String.Format("Error in position rowIndex={0}.", i));
            }
        }
    }
}