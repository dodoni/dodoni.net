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

using NSubstitute;
using NUnit.Framework;

using Dodoni.MathLibrary.Basics;
using Dodoni.MathLibrary.Miscellaneous;
using Dodoni.MathLibrary.ProbabilityTheory.MonteCarloEngine;

namespace Dodoni.MathLibrary.Optimizer.MultiDimensional
{
    /// <summary>Serves as unit test class for <see cref="PraxisOptimizer"/> objects.
    /// </summary>
    [TestFixture]
    public class PraxisOptimizerTests
    {
        #region public methods

        /// <summary>Serves as unit test for <see cref="PraxisOptimizer"/> with respect to a quadratic test function with several constraints.
        /// </summary>
        /// <remarks>The example is taken from 'A numerically stable dual method for solving strictly convex quadratic programs', D. Goldfarb, A. Idnani, Mathematical Programming 27 (1983) pp. 1-33.</remarks>
        [Test]
        public void FindMinimum_QuadraticTestFunctionWithConstraints_AnalyticResult()
        {
            var optimizer = new PraxisOptimizer(CreateStubRandomNumberStream(), PraxisOptimizer.StandardAbortCondition, MultiDimOptimizerConstraintProvider.QuadraticPenalty);

            /* Constraints: 
            * x_0 + x_1 = 3, 
            * x_0 >= 0, 
            * x_1 >= 0, 
            * x_0 + x_2 >= 2 */

            var boxConstraint = optimizer.Constraint.Create(MultiDimRegion.Interval.Create(2, new[] { 0.0, 0.0 }, new[] { Double.NaN, Double.NaN }));
            var equalityConstraint = optimizer.Constraint.Create(new MultiDimRegion.LinearEquality(new DenseMatrix(2, 1, new[] { 1.0, 1.0 }), new[] { 3.0 }));
            var inequalityConstraint = optimizer.Constraint.Create(new MultiDimRegion.LinearInequality(new DenseMatrix(2, 1, new[] { 1.0, 1.0 }), new[] { 2.0 }));

            var algorithm = optimizer.Create(boxConstraint, equalityConstraint, inequalityConstraint);


            var A = new DenseMatrix(2, 2, new[] { 4.0, -2.0, -2.0, 4 - 0 });  // = (4 & -2 \\ -2 & 4)
            var b = new[] { 6.0, 0.0 };

            algorithm.Function = optimizer.Function.Create(2, x => 0.5 * DenseMatrix.GetBilinearForm(A, x) + BLAS.Level1.ddot(2, x, b));

            var actualArgMin = new[] { 1.5, 1.5 }; // is a feasible point
            double actualMinimum;
            var state = algorithm.FindMinimum(actualArgMin, out actualMinimum);

            var expectedArgMin = new[] { 1.0, 2.0 };
            var expectedMinimum = 12.0;

            Assert.That(actualMinimum, Is.EqualTo(expectedMinimum).Within(1E-7), String.Format("State: {0}; actual Minimum: {1}; expected Minimum: {2}; actual argMin: [{3}; {4}]; expected argMin: [{5}; {6}].", state, actualMinimum, expectedMinimum, actualArgMin[0], actualArgMin[1], expectedArgMin[0], expectedArgMin[1]));
            Assert.That(actualArgMin, Is.EqualTo(expectedArgMin).AsCollection.Within(1E-7), String.Format("State: {0}; actual Minimum: {1}; expected Minimum: {2}; actual argMin: [{3}; {4}]; expected argMin: [{5}; {6}].", state, actualMinimum, expectedMinimum, actualArgMin[0], actualArgMin[1], expectedArgMin[0], expectedArgMin[1]));
        }

        /// <summary>Serves as unit test for <see cref="PraxisOptimizer"/> with respect to a quadratic test function with several constraints.
        /// </summary>
        public void FindMinimum_QuadraticTestFunction2WithConstraints_AnalyticResult()
        {
            /* Constraints: 
             * x_0 + x_1 + x_2 >= 6, 
             * -x_0 - x_1 + 2* x_2 >= 2, 
             * x_0, x_1, x_2  >= 0, 
             * */

            var optimizer = new PraxisOptimizer(CreateStubRandomNumberStream(), PraxisOptimizerAbortCondition.Create(requiredNumberOfAcceptedPoints: 100), MultiDimOptimizerConstraintProvider.QuadraticPenalty);

            var boxConstraint = optimizer.Constraint.Create(MultiDimRegion.Interval.Create(3, new[] { 0.0, 0.0, 0.0 }, new[] { Double.NaN, Double.NaN, Double.NaN }));
            var inequalityConstraint = optimizer.Constraint.Create(new MultiDimRegion.LinearInequality(new DenseMatrix(3, 2, new[] { 1.0, 1.0, 1.0, -1.0, -1.0, 2.0 }), new[] { 6.0, 2.0 }));

            var algorithm = optimizer.Create(boxConstraint, inequalityConstraint);

            var A = new DenseMatrix(3, 3, new[] { 2.0, 1.0, 0.0, 1.0, 4.0, 2.0, 0.0, 2.0, 4.0 });
            var b = new[] { 4.0, 6.0, 12.0 };

            algorithm.Function = optimizer.Function.Create(3, x => 0.5 * DenseMatrix.GetBilinearForm(A, x) + BLAS.Level1.ddot(3, x, b));

            var actualArgMin = new[] { 3.3333, 0.0001, 2.66 };
            double actualMinimum;

            var state = algorithm.FindMinimum(actualArgMin, out actualMinimum);

            var expectedArgMin = new[] { 3 + 1.0 / 3.0, 0.0, 2 + 2.0 / 3.0 };
            var expectedMinimum = 70.666666666666666;
            Assert.That(actualMinimum, Is.EqualTo(expectedMinimum).Within(1E-7), String.Format("State: {0}; actual Minimum: {1}; expected Minimum: {2}; actual argMin: [{3}; {4}]; expected argMin: [{5}; {6}].", state, actualMinimum, expectedMinimum, actualArgMin[0], actualArgMin[1], expectedArgMin[0], expectedArgMin[1]));
            Assert.That(actualArgMin, Is.EqualTo(expectedArgMin).AsCollection.Within(1E-7), String.Format("State: {0}; actual Minimum: {1}; expected Minimum: {2}; actual argMin: [{3}; {4}]; expected argMin: [{5}; {6}].", state, actualMinimum, expectedMinimum, actualArgMin[0], actualArgMin[1], expectedArgMin[0], expectedArgMin[1]));
        }

        /// <summary>Serves as unit test for <see cref="PraxisOptimizer"/> with respect to the De Jong's test function, see http://www.geatbx.com/docu/fcnindex-01.html.
        /// </summary>
        /// <param name="d">The dimension of the feasible region.</param>
        [Test]
        public void FindMinimum_DeJongTestFunction_AnalyticResult(
            [Values(2, 3, 5, 7)]
            int d)
        {
            var randomNumberStream = CreateStubRandomNumberStream();

            var optimizer = new PraxisOptimizer(randomNumberStream);
            var optimizerAlgorithm = optimizer.Create(d);
            optimizerAlgorithm.Function = optimizer.Function.Create(d, x =>
            {
                double y = 0.0;
                for (int k = 0; k < d; k++)
                {
                    y += x[k] * x[k];
                }
                return y;
            });

            /* take an initial guess which is not extremly fare away from the argMin: */
            var actualArgMin = new double[d];
            for (int k = 0; k < d; k++)
            {
                actualArgMin[k] = Math.Min(k, 5.9) * Math.Pow(-1, k);
            }
            double actualMinimum;
            var state = optimizerAlgorithm.FindMinimum(actualArgMin, out actualMinimum);

            var expectedMinimum = 0.0;
            Assert.That(actualMinimum, Is.EqualTo(expectedMinimum).Within(1E-5), "Minimum");

            var expectedArgMin = Enumerable.Repeat(0.0, d).ToArray();
            Assert.That(actualArgMin, Is.EqualTo(expectedArgMin).AsCollection.Within(1E-5));
        }

        /// <summary>Serves as unit test for <see cref="PraxisOptimizer"/> with respect to the Rosenbrock test function, see http://en.wikipedia.org/wiki/Test_functions_for_optimization.
        /// </summary>
        /// <param name="d">The dimension of the feasible region.</param>
        [Test]
        public void FindMinimum_RosenbrockTestFunction_AnalyticResult(
            [Values(2, 4, 5, 7)]
            int d)
        {
            var randomNumberStream = CreateStubRandomNumberStream();

            var optimizer = new PraxisOptimizer(randomNumberStream);
            var optimizerAlgorithm = optimizer.Create(d);
            optimizerAlgorithm.Function = optimizer.Function.Create(d, x =>
            {
                double y = 0.0;
                for (int k = 0; k < d - 1; k++)
                {
                    y += 100 * Math.Pow(x[k + 1] - x[k] * x[k], 2) + Math.Pow(x[k] - 1.0, 2);
                }
                return y;
            });

            /* take an initial guess which is not extremly fare away from the argMin: */
            var actualArgMin = new double[d];
            for (int k = 0; k < d; k++)
            {
                actualArgMin[k] = Math.Min(k, 2.75) * Math.Pow(-1, k);
            }
            double actualMinimum;
            var state = optimizerAlgorithm.FindMinimum(actualArgMin, out actualMinimum);

            var expectedMinimum = 0.0;
            Assert.That(actualMinimum, Is.EqualTo(expectedMinimum).Within(1E-2), "Minimum");

            var expectedArgMin = Enumerable.Repeat(1.0, d).ToArray();
            Assert.That(actualArgMin, Is.EqualTo(expectedArgMin).AsCollection.Within(1E-1));
        }

        /// <summary>Serves as unit test for <see cref="PraxisOptimizer"/> with respect to the Goldstein Price function, see http://en.wikipedia.org/wiki/Test_functions_for_optimization.
        /// </summary>
        [TestCaseSource(nameof(ConstraintTransformation))]
        public void FindMinimumGoldsteinPriceFunction_AnalyticResult(MultiDimOptimizerConstraintProvider constraintTransformation)
        {
            var randomNumberStream = CreateStubRandomNumberStream();

            var optimizer = new PraxisOptimizer(randomNumberStream, PraxisOptimizer.StandardAbortCondition, constraintTransformation);
            var constraint = optimizer.Constraint.Create(MultiDimRegion.Interval.Create(2, new[] { -2.0, -2.0 }, new[] { 2.0, 2.0 }));
            var optimizerAlgorithm = optimizer.Create(constraint);
            optimizerAlgorithm.Function = optimizer.Function.Create(2, z =>
            {
                var x = z[0];
                var y = z[1];
                return (1.0 + Math.Pow(x + y + 1.0, 2) * (19.0 - 14.0 * x + 3 * x * x - 14.0 * y + 6.0 * x * y + 3.0 * y * y)) * (30.0 + Math.Pow(2.0 * x - 3.0 * y, 2) * (18.0 - 32.0 * x + 12.0 * x * x + 48.0 * y - 36 * x * y + 27 * y * y));
            });

            /* take an initial guess which is not extremly fare away from the argMin: */
            var actualArgMin = new double[2];

            actualArgMin[0] = 0.35;
            actualArgMin[1] = -0.45;  // Box-Constraint Transformation: sometimes one gets quite high arguments -/+ 3.13E19 arguments which yields to f(x) =\infinity and the algorithm fails
            
            double actualMinimum;
            var state = optimizerAlgorithm.FindMinimum(actualArgMin, out actualMinimum);

            var expectedMinimum = 3.0;
            Assert.That(actualMinimum, Is.EqualTo(expectedMinimum).Within(1E-2), "Minimum");

            var expectedArgMin = new[] { 0.0, -1.0 };
            Assert.That(actualArgMin, Is.EqualTo(expectedArgMin).AsCollection.Within(1E-1));
        }

        /// <summary>Gets the transformation for a specific constraint.
        /// </summary>
        /// <value>The constraint transformation.</value>
        public static IEnumerable<TestCaseData> ConstraintTransformation
        {
            get
            {
                yield return new TestCaseData(MultiDimOptimizerConstraintProvider.BoxTransformation);
                yield return new TestCaseData(MultiDimOptimizerConstraintProvider.QuadraticPenalty);
            }
        }
        #endregion

        #region private methods

        /// <summary>Creates a stub object that represents "random numbers" in its <see cref="IRandomNumberStream"/> representation.
        /// </summary>
        /// <returns>A stub object that represents "random numbers" in its <see cref="IRandomNumberStream"/> representation.</returns>
        private IRandomNumberStream CreateStubRandomNumberStream()
        {
            /* create stub object that represents "random numbers": */
            var random = new Random(12345);
            var nextNumberSequence = Substitute.For<RandomNumberSequence.IDistribution>();
            nextNumberSequence.When(x => x.Uniform(Arg.Any<int>(), Arg.Any<double[]>()))
                .Do(x =>
                {
                    int n = (int)x.Args()[0];
                    var data = (double[])x.Args()[1];
                    for (int k = 0; k < n; k++)
                    {
                        data[k] = random.NextDouble();
                    }
                });
            var randomNumberStream = Substitute.For<IRandomNumberStream>();
            randomNumberStream.NextNumberSequence.Returns(nextNumberSequence);

            return randomNumberStream;
        }
        #endregion
    }
}