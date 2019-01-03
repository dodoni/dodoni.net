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
using System.Collections.Generic;

using NUnit.Framework;

using Dodoni.MathLibrary.GridPointCurves;

namespace Dodoni.MathLibrary.Surfaces
{
    public partial class LabelMatrixTests
    {
        /*
         *  Example input matrix:  
         *  
         *     1  4  7
         *     2 [x] 8
         *     3  6  9
         */

        /// <summary>A test function for the Indexer property of a benchmark Label Matrix. This method checks the result at non-replenished positions in the matrix only.
        /// </summary>
        /// <param name="missingValueReplenishment">The missing Value replenishment.</param>
        /// <param name="orderOfInput">A value indicating whether the labels are assumed to be in assending order etc.</param>
        [TestCaseSource("TestCase1Configuration")]
        public void IndexerProperty_AtNoneReplenishedGridPoint_BenchmarkResult1(LabelMatrix.MissingValueReplenishment missingValueReplenishment, LabelMatrix.OrderOfInput orderOfInput)
        {
            /* some sample data, matrix is provided column-by-column: */
            var matrixEntries = new double[] { 1, 2, 3, 4, Double.NaN, 6, 7, 8, 9 };
            var xLabels = new double[] { 1.1, 2.12, 3.4 };
            var yLabels = new double[] { 1.05, 2.07, 3.65 };

            int rowCount = 3;
            int columnCount = 3;
            var labelMatrix = LabelMatrix.Create(rowCount, columnCount, matrixEntries, xLabels, yLabels, missingValueReplenishment, orderOfInput);

            var indexCollection = new Tuple<int, int, int>[]{  // (rowIndex, columnIndex, Index of 'matrixEntries')
                Tuple.Create(0,0,0),
                Tuple.Create(1,0,1),
                Tuple.Create(2,0,2),
                Tuple.Create(0,1,3),
                   // skip the 'NaN' value in this test
                Tuple.Create(2,1,5),
                Tuple.Create(0,2,6),
                Tuple.Create(1,2,7),
                Tuple.Create(2,2,8)
            };

            foreach (var indices in indexCollection)
            {
                int rowIndex = indices.Item1;
                int columnIndex = indices.Item2;
                int expectedMatrixEntryIndex = indices.Item3;

                double actual = labelMatrix[rowIndex, columnIndex];
                double expected = matrixEntries[expectedMatrixEntryIndex];

                Assert.That(actual, Is.EqualTo(expected).Within(1E-7), String.Format("Row: {0}, Column: {1}, expected: {2}, actual: {3}", rowIndex, columnIndex, expected, actual));
            }
        }

        /// <summary>Gets a collection of <see cref="LabelMatrix.MissingValueReplenishment"/> and <see cref="LabelMatrix.OrderOfInput"/> objects used as input for the first test case object.
        /// </summary>
        /// <value>The configuration input for the first test case object.</value>
        public static IEnumerable<TestCaseData> TestCase1Configuration
        {
            get
            {
                foreach (LabelMatrix.OrderOfInput orderOfInput in Enum.GetValues(typeof(LabelMatrix.OrderOfInput)))
                {
                    yield return new TestCaseData(LabelMatrix.MissingValueReplenishment.None, orderOfInput);
                    yield return new TestCaseData(LabelMatrix.MissingValueReplenishment.WeightedNearestGridPoints.xAxis.Linear, orderOfInput);
                    yield return new TestCaseData(LabelMatrix.MissingValueReplenishment.WeightedNearestGridPoints.xAxis.LogLinear, orderOfInput);

                    yield return new TestCaseData(LabelMatrix.MissingValueReplenishment.WeightedNearestGridPoints.yAxis.Linear, orderOfInput);
                    yield return new TestCaseData(LabelMatrix.MissingValueReplenishment.WeightedNearestGridPoints.yAxis.LogLinear, orderOfInput);

                    yield return new TestCaseData(LabelMatrix.MissingValueReplenishment.WeightedNearestGridPoints.Create(GridPointCurve.Interpolator.Linear, GridPointCurve.Interpolator.LogLinear, 0.25), orderOfInput);
                }
            }
        }

        /// <summary>A test function for the Indexer property of a benchmark Label Matrix.
        /// </summary>
        /// <param name="orderOfInput">A value indicating whether the labels are assumed to be in assending order etc.</param>        
        [Test, Combinatorial]
        public void IndexerProperty_xLinear_BenchmarkResult1(
            [Values(LabelMatrix.OrderOfInput.AscendingOrder, LabelMatrix.OrderOfInput.Disordered, LabelMatrix.OrderOfInput.DisorderedHorizontalLabels, LabelMatrix.OrderOfInput.DisorderedVerticalLabels)]
            LabelMatrix.OrderOfInput orderOfInput)
        {
            var matrixEntries = new double[] { 1, 2, 3, 4, Double.NaN, 6, 7, 8, 9 };
            var xLabels = new double[] { 1.1, 2.12, 3.4 };
            var yLabels = new double[] { 1.05, 2.07, 3.65 };

            int rowCount = 3;
            int columnCount = 3;

            var missingValueReplenishment = LabelMatrix.MissingValueReplenishment.WeightedNearestGridPoints.xAxis.Linear;

            var labelMatrix = LabelMatrix.Create(rowCount, columnCount, matrixEntries, xLabels, yLabels, missingValueReplenishment, orderOfInput);

            double actual = labelMatrix[1, 1];
            double expected = (xLabels[1] - xLabels[0]) * (8.0 - 2.0) / (xLabels[2] - xLabels[0]) + 2.0;  // a linear interpolation

            Assert.That(actual, Is.EqualTo(expected).Within(1E-7), String.Format("Row: {0}, Column: {1}, expected: {2}, actual: {3}", 1, 1, expected, actual));
        }

        /// <summary>A test function for the Indexer property of a benchmark Label Matrix.
        /// </summary>
        /// <param name="orderOfInput">A value indicating whether the labels are assumed to be in assending order etc.</param>        
        [Test, Combinatorial]
        public void IndexerProperty_yLinear_BenchmarkResult1(
            [Values(LabelMatrix.OrderOfInput.AscendingOrder, LabelMatrix.OrderOfInput.Disordered, LabelMatrix.OrderOfInput.DisorderedHorizontalLabels, LabelMatrix.OrderOfInput.DisorderedVerticalLabels)]
            LabelMatrix.OrderOfInput orderOfInput)
        {
            var matrixEntries = new double[] { 1, 2, 3, 4, Double.NaN, 6, 7, 8, 9 };
            var xLabels = new double[] { 1.1, 2.12, 3.4 };
            var yLabels = new double[] { 1.05, 2.07, 3.65 };

            int rowCount = 3;
            int columnCount = 3;

            var missingValueReplenishment = LabelMatrix.MissingValueReplenishment.WeightedNearestGridPoints.yAxis.Linear;

            var labelMatrix = LabelMatrix.Create(rowCount, columnCount, matrixEntries, xLabels, yLabels, missingValueReplenishment, orderOfInput);

            double actual = labelMatrix[1, 1];
            double expected = (yLabels[1] - yLabels[0]) * (6.0 - 4.0) / (yLabels[2] - yLabels[0]) + 4.0;  // a linear interpolation

            Assert.That(actual, Is.EqualTo(expected).Within(1E-7), String.Format("Row: {0}, Column: {1}, expected: {2}, actual: {3}", 1, 1, expected, actual));
        }
    }
}