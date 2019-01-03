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
using Dodoni.MathLibrary.Optimizer;
using Dodoni.MathLibrary.Optimizer.MultiDimensional;

namespace Dodoni.MathLibrary.ProbabilityTheory
{
    /// <summary>Serves as unit test class for <see cref="SapMatrixDecomposer"/>.
    /// </summary>
    public class SapMatrixDecomposerTests
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

            OrdinaryMultiDimOptimizer multiDimOptimizer = new PowellOptimizer();

            var matrixDecomposer = new SapMatrixDecomposer(multiDimOptimizer);

            PseudoSqrtMatrixDecomposer.State state;
            var actual = matrixDecomposer.Create(rawCorrelationMatrix, out state);
            Assert.That(state.Rank, Is.EqualTo(n), String.Format("Rank should be {0}, but was {1}.", n, state.Rank));

            var expected = new DenseMatrix(n, n, new[]{  // the values taken from the reference are re-ordered and some columns are multiplied with -1.0
                0.13192,  -0.10021, -0.05389,
                -0.08718, -0.45536,  0.63329,
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

            OrdinaryMultiDimOptimizer multiDimOptimizer = new PowellOptimizer();
            var matrixDecomposer = new SapMatrixDecomposer(multiDimOptimizer);

            PseudoSqrtMatrixDecomposer.State state;
            var b = matrixDecomposer.Create(rawCorrelationMatrix, out state);

            int expectedRank = 2;
            Assert.That(state.Rank, Is.EqualTo(expectedRank), String.Format("Rank should be {0}, but was {1}.", expectedRank, state.Rank));

            var actual = b * b.T;

            var expected = new DenseMatrix(n, n, new[]{ 
                1.0, 0.89458, 0.69662,
                0.89458, 1.0, 0.30254,
                0.69662, 0.30254, 1.0
            });

            Assert.That(actual.Data, Is.EqualTo(expected.Data).AsCollection.Within(1E-4));
        }

        /// <summary>A test function for the calculation of a parametric Matrix B(\theta).
        /// </summary>
        /// <remarks>This is the first example taken from "A Note on Correlation and Rank Reduction", D. Brigo, May 2002.</remarks>
        [Test]
        public void GetParametricMatrix_BrigoExample1Rank2Opt_BenchmarkResult()
        {
            var theta = new[] { 1.2367, 1.2812, 1.3319, 1.3961, 1.4947, 1.6469, 1.7455, 1.8097, 1.8604, 1.9049 };

            int n = 10;
            int rank = 2;

            var b = new DenseMatrix(n, rank);
            SapMatrixDecomposer.GetParametricMatrix(theta, b);

            var actual = b * b.T;

            var expected = new DenseMatrix(n, n, new[]{
                1.0, 0.9990, 0.9955, 0.9873, 0.9669, 0.9170, 0.8733, 0.8403, 0.8117, 0.7849,
                0.9990, 1.0, 0.9987, 0.9934, 0.9773, 0.9339, 0.8941, 0.8636, 0.8369, 0.8117,
                0.9955, 0.9987, 1.0, 0.9979, 0.9868, 0.9508, 0.9157, 0.8880, 0.8636, 0.8403,
                0.9873, 0.9934, 0.9979, 1.0, 0.9951, 0.9687, 0.9396, 0.9157, 0.8941, 0.8733,
                0.9669, 0.9773, 0.9868, 0.9951, 1.0, 0.9885, 0.9687, 0.9508, 0.9339, 0.9170,
                0.9170, 0.9339, 0.9508, 0.9687, 0.9885, 1.0, 0.9951, 0.9868, 0.9773, 0.9669,
                0.8733, 0.8941, 0.9157, 0.9396, 0.9687, 0.9951, 1.0, 0.9979, 0.9934, 0.9873,
                0.8403, 0.8636, 0.8880, 0.9157, 0.9508, 0.9868, 0.9979, 1.0, 0.9987, 0.9955,
                0.8117, 0.8369, 0.8636, 0.8941, 0.9339, 0.9773, 0.9934, 0.9987, 1.0, 0.9990,
                0.7849, 0.8117, 0.8403, 0.8733, 0.9170, 0.9669, 0.9873, 0.9955, 0.9990, 1.0
            });
            Assert.That(actual.Data, Is.EqualTo(expected.Data).AsCollection.Within(1E-4));
        }

        /// <summary>A test function for the calculation of a parametric Matrix B(\theta*).
        /// </summary>
        /// <remarks>This is the first example taken from "A Note on Correlation and Rank Reduction", D. Brigo, May 2002.</remarks>
        [Test]
        public void GetParametricMatrix_BrigoExample1Rank4Opt_BenchmarkResult()
        {
            var theta = new[] {                                
                1.6844, 1.6088, 1.4688, 1.4435, 1.5051, 1.6365, 1.6981, 1.6728, 1.5328, 1.4571, 
                1.7328, 1.6828, 1.5810, 1.4708, 1.3957, 1.3957, 1.4708, 1.5810, 1.6828, 1.7328,
                1.2775, 1.2965, 1.3444, 1.4267, 1.5203, 1.6213, 1.7149, 1.7972, 1.8451, 1.8640};

            int n = 10;
            int rank = 4;

            var b = new DenseMatrix(n, rank);
            SapMatrixDecomposer.GetParametricMatrix(theta, b);

            var actual = b * b.T;

            var expected = new DenseMatrix(n, n, new[]{
                1.0, 0.9957, 0.9633, 0.9267, 0.8999, 0.8867, 0.8752, 0.8598, 0.8346, 0.8136,
                0.9957, 1.0, 0.9839, 0.9559, 0.9295, 0.9078, 0.8893, 0.8715, 0.8524, 0.8346,
                0.9633, 0.9839, 1.0, 0.9904, 0.9673, 0.9317, 0.9012, 0.8796, 0.8715, 0.8598,
                0.9267, 0.9559, 0.9904, 1.0, 0.9911, 0.9603, 0.9276, 0.9012, 0.8893, 0.8752,
                0.8999, 0.9295, 0.9673, 0.9911, 1.0, 0.9865, 0.9603, 0.9317, 0.9078, 0.8867,
                0.8867, 0.9078, 0.9317, 0.9603, 0.9865, 1.0, 0.9911, 0.9673, 0.9295, 0.8999,
                0.8752, 0.8893, 0.9012, 0.9276, 0.9603, 0.9911, 1.0, 0.9904, 0.9559, 0.9267,
                0.8598, 0.8715, 0.8796, 0.9012, 0.9317, 0.9673, 0.9904, 1.0, 0.9839, 0.9633, 
                0.8346, 0.8524, 0.8715, 0.8893, 0.9078, 0.9295, 0.9559, 0.9839, 1.0, 0.9957,
                0.8136, 0.8346, 0.8598, 0.8752, 0.8867, 0.8999, 0.9267, 0.9633, 0.9957, 1.0
            });
            Assert.That(actual.Data, Is.EqualTo(expected.Data).AsCollection.Within(1E-4));
        }

        /// <summary>A test function for the calculation of the angle parameter \theta; check whether <see cref="SapMatrixDecomposer.GetParametricMatrix(double[], DenseMatrix)"/> 
        /// and <see cref="SapMatrixDecomposer.GetAngleParameter(DenseMatrix, double[])"/> are consistent.
        /// </summary>
        /// <remarks>This test is based on the first example taken from "A Note on Correlation and Rank Reduction", D. Brigo, May 2002.</remarks>
        [Test]
        public void GetAngleParameter_ParametricMatrix_BenchmarkParameter()
        {
            var expected = new[] { // original parameter \theta
                1.6844, 1.6088, 1.4688, 1.4435, 1.5051, 1.6365, 1.6981, 1.6728, 1.5328, 1.4571, 
                1.7328, 1.6828, 1.5810, 1.4708, 1.3957, 1.3957, 1.4708, 1.5810, 1.6828, 1.7328,
                1.2775, 1.2965, 1.3444, 1.4267, 1.5203, 1.6213, 1.7149, 1.7972, 1.8451, 1.8640};

            int n = 10;
            int rank = 4;

            var b = new DenseMatrix(n, rank);
            SapMatrixDecomposer.GetParametricMatrix(expected, b);  // b = B(\theta)

            var actual = new double[n * (rank - 1)];
            SapMatrixDecomposer.GetAngleParameter(b, actual); // determine \theta* such that B(\theta*)  = b

            Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-4));
        }

        /// <summary>A test function for the calculation of the angle parameter \theta; check whether <see cref="SapMatrixDecomposer.GetParametricMatrix(double[], DenseMatrix)"/> 
        /// and <see cref="SapMatrixDecomposer.GetAngleParameter(DenseMatrix, double[])"/> are consistent.
        /// </summary>
        /// <remarks>This test is based on the 2th example taken from "The most general methodology to create a valid correlation matrix for risk management and option pricing purposes", R. Rebonato, P. Jäckel, Oct. 1999.</remarks>
        [Test]
        public void GetParametricMatrix_Test_BenchmarkMatrix()
        {
            int n = 3;
            int rank = 2;

            var expected = new DenseMatrix(n, rank, new[] {  // matrix B taken from the reference
                0.99805, 0.86434, 0.73974,
                0.06238, 0.50292, -0.67290});

            var theta = new double[n * (rank - 1)];
            SapMatrixDecomposer.GetAngleParameter(expected, theta);

            var actual = new DenseMatrix(n, rank);
            SapMatrixDecomposer.GetParametricMatrix(theta, actual);

            Assert.That(actual.Data, Is.EqualTo(expected.Data).AsCollection.Within(1E-4));
        }

        #region obsolete code

        /* the following examples shows different results than the reference. Perhaps a typo in the references [other examples in the same reference are fine] */

        ///// <summary>A test function for the calculation of a parametric Matrix B(\theta).
        ///// </summary>
        ///// <remarks>This is the first example taken from "A Note on Correlation and Rank Reduction", D. Brigo, May 2002.</remarks>
        //[Test]
        //public void GetParametricMatrix_BrigoExample1Rank2_BenchmarkResult()
        //{
        //    var theta = new[] { 1.2886, 1.3081, 1.3586, 1.4333, 1.5233, 1.6183, 1.7083, 1.7830, 1.8335, 1.8530 };

        //    int n = 10;
        //    int rank = 2;

        //    var b = new DenseMatrix(n, rank);
        //    SapMatrixDecomposer.GetParametricMatrix(theta, b);

        //    var actual = b * b.T;

        //    //Assert.That(actual.Data, Is.EqualTo(expected.Data).AsCollection.Within(1E-4));
        //}

        ///// <summary>A test function for the calculation of a parametric Matrix B(\theta).
        ///// </summary>
        ///// <remarks>This is the first example taken from "A Note on Correlation and Rank Reduction", D. Brigo, May 2002.</remarks>
        //[Test]
        //public void GetParametricMatrix_BrigoExample1Rank4_BenchmarkResult()
        //{
        //    var theta = new[] {                                      
        //        1.6695, 1.5914, 1.496,  1.4634, 1.5211, 1.6205, 1.6782, 1.6456, 1.5502, 1.4721,
        //        1.7239, 1.6672, 1.5737, 1.4784, 1.4194, 1.4194, 1.4784, 1.5737, 1.6672, 1.7239,
        //        1.2837, 1.3068, 1.358,  1.4319, 1.5226, 1.6189, 1.7097, 1.7836, 1.8348, 1.8579};

        //    int n = 10;
        //    int rank = 4;

        //    var b = new DenseMatrix(n, rank);
        //    SapMatrixDecomposer.GetParametricMatrix(theta, b);

        //    var actual = b * b.T;
           
        //    //Assert.That(actual.Data, Is.EqualTo(expected.Data).AsCollection.Within(1E-4));
        //}
        #endregion
    }
}