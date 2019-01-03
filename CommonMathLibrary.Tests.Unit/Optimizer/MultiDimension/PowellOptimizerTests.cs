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

using Dodoni.BasicComponents;
using Dodoni.MathLibrary.Miscellaneous;

namespace Dodoni.MathLibrary.Optimizer.MultiDimensional
{
    /// <summary>Serves as unit test class for <see cref="PowellOptimizer"/> objects.
    /// </summary>
    [TestFixture]
    public class PowellOptimizerTests
    {
        /// <summary>Serves as unit test for <see cref="PowellOptimizer"/> with respect to the De Jong's test function, see http://www.geatbx.com/docu/fcnindex-01.html.
        /// </summary>
        /// <param name="d">The dimension of the feasible region.</param>
        [Test]
        public void FindMinimum_DeJongTestFunction_AnalyticResult(
            [Values(2, 3, 5, 7)]
            int d)
        {
            var optimizer = new PowellOptimizer();
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

        /// <summary>Serves as unit test for <see cref="PowellOptimizer"/> with respect to the Rosenbrock test function, see http://en.wikipedia.org/wiki/Test_functions_for_optimization.
        /// </summary>
        /// <param name="d">The dimension of the feasible region.</param>
        [Test]
        public void FindMinimum_RosenbrockTestFunction_AnalyticResult(
            [Values(2, 4, 5, 7)]
            int d)
        {
            var optimizer = new PowellOptimizer();
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
                actualArgMin[k] = Math.Min(k, 2.1);
            }
            double actualMinimum;
            var state = optimizerAlgorithm.FindMinimum(actualArgMin, out actualMinimum);

            var expectedMinimum = 0.0;
            Assert.That(actualMinimum, Is.EqualTo(expectedMinimum).Within(1E-2), "Minimum");

            var expectedArgMin = Enumerable.Repeat(1.0, d).ToArray();
            Assert.That(actualArgMin, Is.EqualTo(expectedArgMin).AsCollection.Within(1E-1));
        }

        /// <summary>Serves as unit test for <see cref="PowellOptimizer"/> with respect to the Goldstein Price function, see http://en.wikipedia.org/wiki/Test_functions_for_optimization.
        /// </summary>
        [TestCaseSource(nameof(ConstraintTransformation))]
        public void FindMinimumGoldsteinPriceFunction_AnalyticResult(MultiDimOptimizerConstraintProvider constraintTransformation)
        {
            var optimizer = new PowellOptimizer(PowellOptimizer.StandardAbortCondition, PowellOptimizer.StandardLineSearchOptimizer, constraintTransformation);
            var constraint = optimizer.Constraint.Create(MultiDimRegion.Interval.Create(2, new[] { -2.0, -2.0 }, new[] { 2.0, 2.0 }));
            var optimizerAlgorithm = optimizer.Create(constraint);
            optimizerAlgorithm.Function = optimizer.Function.Create(2, z =>
            {
                var x = z[0];
                var y = z[1];
                return (1.0 + Math.Pow(x + y + 1.0, 2) * (19.0 - 14.0 * x + 3 * x * x - 14.0 * y + 6.0 * x * y + 3.0 * y * y)) * (30.0 + Math.Pow(2.0 * x - 3.0 * y, 2) * (18.0 - 32.0 * x + 12.0 * x * x + 48.0 * y - 36 * x * y + 27 * y * y));
            });

            /* take an initial guess which is not extremly fare away from the expected argMin: */
            var actualArgMin = new double[2];
            actualArgMin[0] = -0.1;
            actualArgMin[1] = -0.75;
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
    }
}