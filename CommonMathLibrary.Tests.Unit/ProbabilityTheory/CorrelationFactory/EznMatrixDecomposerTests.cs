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
using System.Collections.Generic;

using NUnit.Framework;

using Dodoni.MathLibrary;
using Dodoni.MathLibrary.Basics;

namespace Dodoni.MathLibrary.ProbabilityTheory
{
    /// <summary>Serves as unit test class for <see cref="EznMatrixDecomposer"/>.
    /// </summary>
    public class EznMatrixDecomposerTests
    {
        /// <summary>A test function for the calculation of a specific pseudo-sqrt matrix.
        /// </summary>
        /// <remarks>This is the first example taken from "The most general methodology to create a valid correlation matrix for risk management and option pricing purposes", §4, R. Rebonato, P. Jäckel, Oct. 1999.</remarks>
        [Test]
        public void Create_RebonatoJaeckelExample1_BenchmarkResult()
        {
            int n = 3;
            var rawCorrelationMatrix = new DenseMatrix(n, n, new[] { 
                1.0, 0.9, 0.7, 
                0.9, 1.0, 0.4, 
                0.7, 0.4, 1.0 });

            var matrixDecomposer = new EznMatrixDecomposer();

            PseudoSqrtMatrixDecomposer.State state;
            var actual = matrixDecomposer.Create(rawCorrelationMatrix, out state);
            Assert.That(state.Rank, Is.EqualTo(n), String.Format("Rank should be {0}, but was {1}.", n, state.Rank));

            var expected = new DenseMatrix(n, n, new[]{  // the values taken from the reference are re-ordered with a negative sign
                0.13192, -0.10021, -0.05389,
                -0.08718, -0.45536, 0.63329,
                -0.98742, -0.88465, -0.77203                
                });

            Assert.That(actual.Data, Is.EqualTo(expected.Data).AsCollection.Within(1E-4));
        }

        /// <summary>A test function for the calculation of a specific pseudo-sqrt matrix.
        /// </summary>
        /// <remarks>This is the second example taken from "The most general methodology to create a valid correlation matrix for risk management and option pricing purposes", §4, R. Rebonato, P. Jäckel, Oct. 1999.</remarks>
        [Test]
        public void Create_RebonatoJaeckelExample2_BenchmarkResult()
        {
            int n = 3;
            var rawCorrelationMatrix = new DenseMatrix(n, n, new[] { 1.0, 0.9, 0.7, 0.9, 1.0, 0.3, 0.7, 0.3, 1.0 });

            var matrixDecomposer = new EznMatrixDecomposer(BasicComponents.Containers.InfoOutputDetailLevel.Full);
            PseudoSqrtMatrixDecomposer.State state;
            var actual = matrixDecomposer.Create(rawCorrelationMatrix, out state);

            int expectedRank = 2;
            Assert.That(state.Rank, Is.EqualTo(expectedRank), String.Format("Rank should be {0}, but was {1}.", expectedRank, state.Rank));

            var expected = new DenseMatrix(n, state.Rank, new[]{ // the values taken from the reference are re-ordered and with a negative sign
                -0.06238, -0.50292, 0.67290, 
                -0.99805, -0.86434, -0.73974
            });

            Assert.That(actual.Data.Take(actual.RowCount * actual.ColumnCount).ToArray(), Is.EqualTo(expected.Data).AsCollection.Within(1E-4));
        }

        /// <summary>A test function for the calculation of a specific pseudo-sqrt matrix (for <see cref="SymmetricMatrix"/> objects).
        /// </summary>
        /// <remarks>This is the second example taken from "The most general methodology to create a valid correlation matrix for risk management and option pricing purposes", §4, R. Rebonato, P. Jäckel, Oct. 1999.</remarks>
        [Test]
        public void CreateSymmetric_RebonatoJaeckelExample2_BenchmarkResult()
        {
            int n = 3;
            var rawCorrelationMatrix = new SymmetricMatrix(n, new[] { 1.0, 0.9, 0.7, 1.0, 0.3, 1.0 });

            var matrixDecomposer = new EznMatrixDecomposer();
            int rank;

            var actual = matrixDecomposer.Create(rawCorrelationMatrix, out rank);

            int expectedRank = 2;
            Assert.That(rank, Is.EqualTo(expectedRank), String.Format("Rank should be {0}, but was {1}.", expectedRank, rank));

            var expected = new DenseMatrix(n, rank, new[]{ // the values taken from the reference are re-ordered and same(!) columns are with a negative sign
                -0.06238, -0.50292, 0.67290, 
                0.99805, 0.86434, 0.73974
            });

            Assert.That(actual.Data.Take(actual.RowCount * actual.ColumnCount).ToArray(), Is.EqualTo(expected.Data).AsCollection.Within(1E-4));
        }
    }
}