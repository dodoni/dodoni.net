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

using Dodoni.MathLibrary;
using Dodoni.MathLibrary.Basics;

namespace Dodoni.MathLibrary.ProbabilityTheory
{
    /// <summary>Serves as unit test class for <see cref="EziMatrixDecomposer"/>.
    /// </summary>
    public class EziMatrixDecomposerTests
    {
        /// <summary>A test function for the calculation of a specific pseudo-sqrt matrix.
        /// </summary>
        /// <remarks>This is the first example taken from "An EZI method to reduce the rank of a correlation matrix", §5, M. Morini, N. Webber, April 2004.</remarks>
        [Test]
        public void Create_Example1_BenchmarkResult()
        {
            var rawCorrelationMatrix = new DenseMatrix(3, 3, new[] { 1.0, 0.9, 0.7, 0.9, 1.0, 0.3, 0.7, 0.3, 1.0 });

            var matrixDecomposer = new EziMatrixDecomposer(2);

            PseudoSqrtMatrixDecomposer.State state;
            var b = matrixDecomposer.Create(rawCorrelationMatrix, out state);

            var actualCorrelationMatrix = b * b.T;
            var actual = Math.Pow((actualCorrelationMatrix - rawCorrelationMatrix).GetNorm(), 2);

            var expected = 0.946E-4;
            Assert.That(actual, Is.EqualTo(expected).Within(1E-7));
        }

        /// <summary>A test function for the calculation of a specific pseudo-sqrt matrix.
        /// </summary>
        /// <remarks>This is the second example taken from "An EZI method to reduce the rank of a correlation matrix", §5, M. Morini, N. Webber, April 2004.</remarks>
        [TestCase(2, 0.0765, 1E-4)]
        [TestCase(4, 0.0070, 1E-4)]
        [TestCase(7, 0.918E-3, 1E-5)]
        public void Create_Example2_BenchmarkResult(int maximalRank, double expected, double tolerance)
        {
            int n = 10;
            var rawCorrelationMatrix = new DenseMatrix(n, n);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    rawCorrelationMatrix[i, j] = 0.5 + 0.5 * Math.Exp(-0.05 * Math.Abs(i - j));
                }
            }

            var matrixDecomposer = new EziMatrixDecomposer(maximalRank);

            PseudoSqrtMatrixDecomposer.State state;
            var b = matrixDecomposer.Create(rawCorrelationMatrix, out state);

            var actualCorrelationMatrix = b * b.T;
            var actual = Math.Pow((actualCorrelationMatrix - rawCorrelationMatrix).GetNorm(), 2);

            Assert.That(actual, Is.EqualTo(expected).Within(tolerance));
        }

        /// <summary>A test function for the calculation of a specific pseudo-sqrt matrix.
        /// </summary>
        /// <remarks>This is the 3th example taken from "An EZI method to reduce the rank of a correlation matrix", §5, M. Morini, N. Webber, April 2004.</remarks>
        [TestCase(4, 5.96, 1E-2)]
        [TestCase(7, 1.13, 1E-2)]
        public void Create_Example3_BenchmarkResult(int maximalRank, double expected, double tolerance)
        {
            int n = 10;
            var rawCorrelationMatrix = new DenseMatrix(n, n);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    rawCorrelationMatrix[i, j] = Math.Exp(-Math.Abs(i - j));
                }
            }

            var matrixDecomposer = new EziMatrixDecomposer(maximalRank);

            PseudoSqrtMatrixDecomposer.State state;
            var b = matrixDecomposer.Create(rawCorrelationMatrix, out state);

            var actualCorrelationMatrix = b * b.T;
            var actual = Math.Pow((actualCorrelationMatrix - rawCorrelationMatrix).GetNorm(), 2);

            Assert.That(actual, Is.EqualTo(expected).Within(tolerance));
        }
    }
}