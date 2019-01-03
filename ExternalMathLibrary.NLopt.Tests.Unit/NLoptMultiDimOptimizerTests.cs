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

using Dodoni.MathLibrary.Basics;
using Dodoni.MathLibrary.Optimizer;
using Dodoni.MathLibrary.Miscellaneous;

namespace Dodoni.MathLibrary.Native.NLopt
{
    /// <summary>Serves as class for unit tests for <see cref="NLoptMultiDimOptimizer"/>.
    /// </summary>
    [TestFixture]
    public class NLoptMultiDimOptimizerTests
    {
        /// <summary>Serves as unit test for <see cref="NLoptPtr"/> with a simple example and COBYLA algorithm.
        /// </summary>
        [Test]
        public void NLoptCobyla_TestCase_AnalyticMinimum()
        {
            var cobyla = new NLoptMultiDimOptimizer(NLoptAlgorithm.LN_COBYLA);
            var nloptBoxConstraint = cobyla.Constraint.Create(MultiDimRegion.Interval.Create(dimension: 2, lowerBounds: new[] { 1.0, 5.0 }, upperBounds: new[] { 12.4, 34.2 }));

            var algorithm = cobyla.Create(nloptBoxConstraint);
            algorithm.Function = cobyla.Function.Create(2, x => x[0] * x[0] + x[1] * x[1] + 1.123);

            double[] argMin = new double[2] { 1.4, 5.8 };
            double actual;
            var code = algorithm.FindMinimum(argMin, out actual);

            double expected = 1.0 * 1.0 + 5.0 * 5.0 + 1.123;

            Assert.That(actual, Is.EqualTo(expected).Within(1E-7));
        }

        /// <summary>Serves as unit test for <see cref="NLoptMultiDimOptimizer"/>; see Tutorial of the NLopt documentation.
        /// </summary>
        /// <param name="nloptAlgorithm">The NLopt algorithm, i.e. COBYLA or MMA.</param>
        [TestCase(NLoptAlgorithm.LN_COBYLA)]
        [TestCase(NLoptAlgorithm.LD_MMA)]
        public void NLoptTutorialExample_TestCase_AnalyticSolution(NLoptAlgorithm nloptAlgorithm)
        {
            var multiDimOptimizer = new NLoptMultiDimOptimizer(nloptAlgorithm, NLoptAbortCondition.Create(relativeXTolerance: 1E-4));

            var nloptBoxConstraint = multiDimOptimizer.Constraint.Create(MultiDimRegion.Interval.Create(dimension: 2, lowerBounds: new[] { Double.NegativeInfinity, 0.0 }, upperBounds: new[] { Double.PositiveInfinity, Double.PositiveInfinity }));

            double a1 = 2.0;
            double b1 = 0.0;

            /* This code uses generic constraints, i.e. polynomial constraints: */
            /* The constraints in the Tutorial of the NLopt documentation can be re-written as polynomial in the following form:
             * 
             * x_2 - a^3 * x_1^3 - 3*a^2*b*x_1^2 - 3*a*b^2 * x_1 >= b^3
             */

            var polynomialConstraint1 = MultiDimRegion.Polynomial.Create(2, b1 * b1 * b1, Double.PositiveInfinity, new[]{
                    1.0, - a1*a1*a1, - 3.0*a1*a1*b1, - 3.0 * a1*b1*b1 },
                  MultiDimRegion.Polynomial.Monomial.Create(1, 1),
                  MultiDimRegion.Polynomial.Monomial.Create(0, 3),
                  MultiDimRegion.Polynomial.Monomial.Create(0, 2),
                  MultiDimRegion.Polynomial.Monomial.Create(0, 1));

            double a2 = -1;
            double b2 = 1.0;

            var polynomialConstraint2 = MultiDimRegion.Polynomial.Create(2, b2 * b2 * b2, Double.PositiveInfinity, new[]{
                    1.0, - a2*a2*a2, - 3.0*a2*a2*b2, - 3.0 * a2*b2*b2 },
                     MultiDimRegion.Polynomial.Monomial.Create(1, 1),
                     MultiDimRegion.Polynomial.Monomial.Create(0, 3),
                     MultiDimRegion.Polynomial.Monomial.Create(0, 2),
                     MultiDimRegion.Polynomial.Monomial.Create(0, 1));

            var optimizer = multiDimOptimizer.Create(nloptBoxConstraint,
                                  multiDimOptimizer.Constraint.Create(polynomialConstraint1),
                                  multiDimOptimizer.Constraint.Create(polynomialConstraint2));

            optimizer.Function = multiDimOptimizer.Function.Create(2, (x, grad) =>
            {
                if (grad != null)
                {
                    grad[0] = 0.0;
                    grad[1] = 0.5 / Math.Sqrt(x[1]);
                }
                return Math.Sqrt(x[1]);
            });

            var actualArgMin = new[] { 1.234, 5.678 };
            double actualMinimum;

            var state = optimizer.FindMinimum(actualArgMin, out actualMinimum);

            double expectedMinimum = Math.Sqrt(8.0 / 27.0);
            double expectedArgMin0 = 1.0 / 3.0;
            double expectedArgMin1 = 8.0 / 27.0;

            Assert.That(actualMinimum, Is.EqualTo(expectedMinimum).Within(1E-3), String.Format("<Minimum> State: {0}; Actual minimum: {1}; Expected minimum: {2}; Actual argMin: ({3}; {4}); Expected argMin: ({5}; {6})", state, actualMinimum, expectedMinimum, actualArgMin[0], actualArgMin[1], expectedArgMin0, expectedArgMin1));
            Assert.That(actualArgMin[0], Is.EqualTo(expectedArgMin0).Within(1E-3), String.Format("<argMin[0]> State: {0}; Actual minimum: {1}; Expected minimum: {2}; Actual argMin: ({3}; {4}); Expected argMin: ({5}; {6})", state, actualMinimum, expectedMinimum, actualArgMin[0], actualArgMin[1], expectedArgMin0, expectedArgMin1));
            Assert.That(actualArgMin[1], Is.EqualTo(expectedArgMin1).Within(1E-3), String.Format("<argMin[1]> State: {0}; Actual minimum: {1}; Expected minimum: {2}; Actual argMin: ({3}; {4}); Expected argMin: ({5}; {6})", state, actualMinimum, expectedMinimum, actualArgMin[0], actualArgMin[1], expectedArgMin0, expectedArgMin1));
        }

        /// <summary>Serves as unit test for <see cref="NLoptMultiDimOptimizer"/>; see Tutorial of the NLopt documentation.
        /// </summary>
        /// <param name="nloptAlgorithm">The NLopt algorithm, i.e. COBYLA or MMA.</param>
        /// <remarks>Here we use a second (more complex) way how to call the polynomial constraint.</remarks>
        [TestCase(NLoptAlgorithm.LN_COBYLA)]
        [TestCase(NLoptAlgorithm.LD_MMA)]
        public void NLoptTutorialExample_AlternativeTestCase_AnalyticSolution(NLoptAlgorithm nloptAlgorithm)
        {
            var multiDimOptimizer = new NLoptMultiDimOptimizer(nloptAlgorithm, NLoptAbortCondition.Create(relativeXTolerance: 1E-4));

            var nloptBoxConstraint = multiDimOptimizer.Constraint.Create(MultiDimRegion.Interval.Create(dimension: 2, lowerBounds: new[] { Double.NegativeInfinity, 0.0 }, upperBounds: new[] { Double.PositiveInfinity, Double.PositiveInfinity }));

            double a1 = 2.0;
            double b1 = 0.0;

            /* This code uses generic constraints, i.e. polynomial constraints: */
            /* The constraints in the Tutorial of the NLopt documentation can be re-written as polynomial in the following form:
             * 
             * x_2 - a^3 * x_1^3 - 3*a^2*b*x_1^2 - 3*a*b^2 * x_1 >= b^3
             */

            var polynomialConstraint1 = MultiDimRegion.Polynomial.Create(2, b1 * b1 * b1, Double.PositiveInfinity, new[]{
                    1.0, - a1*a1*a1, - 3.0*a1*a1*b1, - 3.0 * a1*b1*b1 },
                 new[] { MultiDimRegion.Polynomial.Monomial.Create(1, 1) },
                 new[] { MultiDimRegion.Polynomial.Monomial.Create(0, 3) },
                 new[] { MultiDimRegion.Polynomial.Monomial.Create(0, 2) },
                 new[] { MultiDimRegion.Polynomial.Monomial.Create(0, 1) });


            double a2 = -1;
            double b2 = 1.0;

            var polynomialConstraint2 = MultiDimRegion.Polynomial.Create(2, b2 * b2 * b2, Double.PositiveInfinity, new[]{
                    1.0, - a2*a2*a2, - 3.0*a2*a2*b2, - 3.0 * a2*b2*b2 },
                    new[] { MultiDimRegion.Polynomial.Monomial.Create(1, 1) },
                    new[] { MultiDimRegion.Polynomial.Monomial.Create(0, 3) },
                    new[] { MultiDimRegion.Polynomial.Monomial.Create(0, 2) },
                    new[] { MultiDimRegion.Polynomial.Monomial.Create(0, 1) });


            var optimizer = multiDimOptimizer.Create(nloptBoxConstraint,
                                  multiDimOptimizer.Constraint.Create(polynomialConstraint1),
                                  multiDimOptimizer.Constraint.Create(polynomialConstraint2));

            optimizer.Function = multiDimOptimizer.Function.Create(2, (x, grad) =>
                                  {
                                      if (grad != null)
                                      {
                                          grad[0] = 0.0;
                                          grad[1] = 0.5 / Math.Sqrt(x[1]);
                                      }
                                      return Math.Sqrt(x[1]);
                                  });

            var actualArgMin = new[] { 1.234, 5.678 };
            double actualMinimum;
            optimizer.FindMinimum(actualArgMin, out actualMinimum);

            double expectedMinimum = Math.Sqrt(8.0 / 27.0);
            double expectedArgMin0 = 1.0 / 3.0;
            double expectedArgMin1 = 8.0 / 27.0;

            Assert.That(actualMinimum, Is.EqualTo(expectedMinimum).Within(1E-3));
            Assert.That(actualArgMin[0], Is.EqualTo(expectedArgMin0).Within(1E-3));
            Assert.That(actualArgMin[1], Is.EqualTo(expectedArgMin1).Within(1E-3));
        }

        /// <summary>Serves as unit test for <see cref="NLoptMultiDimOptimizer"/>; see Tutorial of the NLopt documentation.
        /// </summary>
        /// <param name="nloptAlgorithm">The NLopt algorithm, i.e. COBYLA or MMA.</param>
        [TestCase(NLoptAlgorithm.LN_COBYLA)]
        [TestCase(NLoptAlgorithm.LD_MMA)]
        public void NLoptTutorialExample_TestCaseNLoptConstraints_AnalyticSolution(NLoptAlgorithm nloptAlgorithm)
        {
            var multiDimOptimizer = new NLoptMultiDimOptimizer(NLoptAlgorithm.LN_COBYLA, NLoptAbortCondition.Create(relativeXTolerance: 1E-4));

            var nloptBoxConstraint = multiDimOptimizer.Constraint.Create(MultiDimRegion.Interval.Create(dimension: 2, lowerBounds: new[] { Double.NegativeInfinity, 0.0 }, upperBounds: new[] { Double.PositiveInfinity, Double.PositiveInfinity }));

            double a1 = 2.0;
            double b1 = 0.0;
            double a2 = -1;
            double b2 = 1.0;

            /* this code uses NLopt specific constraints: */
            var optimizer = multiDimOptimizer.Create(nloptBoxConstraint,
                                  multiDimOptimizer.Constraint.Create(2,
                                     (x, grad) =>
                                     {
                                         if (grad != null)
                                         {
                                             grad[0] = 3.0 * a1 * (a1 * x[0] + b1) * (a1 * x[0] + b1);
                                             grad[1] = -1.0;
                                         }
                                         return (a1 * x[0] + b1) * (a1 * x[0] + b1) * (a1 * x[0] + b1) - x[1];
                                     }),
                                  multiDimOptimizer.Constraint.Create(2,
                                     (x, grad) =>
                                     {
                                         if (grad != null)
                                         {
                                             grad[0] = 3.0 * a2 * (a2 * x[0] + b2) * (a2 * x[0] + b2);
                                             grad[1] = -1.0;
                                         }
                                         return (a2 * x[0] + b2) * (a2 * x[0] + b2) * (a2 * x[0] + b2) - x[1];
                                     }
                                  ));

            optimizer.Function = multiDimOptimizer.Function.Create(2, (x, grad) =>
            {
                if (grad != null)
                {
                    grad[0] = 0.0;
                    grad[1] = 0.5 / Math.Sqrt(x[1]);
                }
                return Math.Sqrt(x[1]);
            });

            var actualArgMin = new[] { 1.234, 5.678 };
            double actualMinimum;
            optimizer.FindMinimum(actualArgMin, out actualMinimum);

            double expectedMinimum = Math.Sqrt(8.0 / 27.0);
            double expectedArgMin0 = 1.0 / 3.0;
            double expectedArgMin1 = 8.0 / 27.0;

            Assert.That(actualMinimum, Is.EqualTo(expectedMinimum).Within(1E-3));
            Assert.That(actualArgMin[0], Is.EqualTo(expectedArgMin0).Within(1E-3));
            Assert.That(actualArgMin[1], Is.EqualTo(expectedArgMin1).Within(1E-3));
        }

        /// <summary>Serves as unit test for <see cref="NLoptMultiDimOptimizer"/>.
        /// </summary>
        [Test]
        public void FindMinimum_TestFunctionWithLinearConstraints_AnalyticResult()
        {
            /* Constraints: 
             * x_0 + x_1 + x_2 >= 6, 
             * -x_0 - x_1 + 2* x_2 >= 2, 
             * x_0, x_1, x_2  >= 0, 
             * */

            // todo: this unit test failed; see 
            /* https://nlopt-discuss.ab-initio.mit.narkive.com/05OUy9nv/net-wrapper-vector-valued-constraints
             * 
             * see also comments in: NLoptConstraintFactory.Create(MultiDimRegion.LinearInequality linearInequalityConstraint)
             */
            var optimizer = new NLoptMultiDimOptimizer(NLoptAlgorithm.LN_COBYLA);
            //var optimizer = new NLoptMultiDimOptimizer(NLoptAlgorithm.LD_MMA);

            var boxConstraint = optimizer.Constraint.Create(MultiDimRegion.Interval.Create(3, new[] { 0.0, 0.0, 0.0 }, new[] { Double.NaN, Double.NaN, Double.NaN }));
            var inequalityConstraint = optimizer.Constraint.Create(MultiDimRegion.LinearInequality.Create(new DenseMatrix(3, 2, new[] { 1.0, 1.0, 1.0, -1.0, -1.0, 2.0 }), new[] { 6.0, 2.0 }));

            var algorithm = optimizer.Create(boxConstraint, inequalityConstraint);

            var A = new DenseMatrix(3, 3, new[] { 2.0, 1.0, 0.0, 1.0, 4.0, 2.0, 0.0, 2.0, 4.0 });
            var b = new[] { 4.0, 6.0, 12.0 };

            algorithm.SetFunction(  // a simple quadratic program
                (x, grad) =>
                {
                    if (grad != null)
                    {
                    }
                    return 0.5 * DenseMatrix.GetBilinearForm(A, x) + BLAS.Level1.ddot(3, x, b);
                });

            var actualArgMin = new[] { 3.3333, 0.0001, 2.66 };
            double actualMinimum;

            var state = algorithm.FindMinimum(actualArgMin, out actualMinimum);

            var expectedArgMin = new[] { 3 + 1.0 / 3.0, 0.0, 2 + 2.0 / 3.0 };
            var expectedMinimum = 70.666666666666666;
            Assert.That(actualMinimum, Is.EqualTo(expectedMinimum).Within(1E-7), String.Format("State: {0}; actual Minimum: {1}; expected Minimum: {2}; actual argMin: [{3}; {4}]; expected argMin: [{5}; {6}].", state, actualMinimum, expectedMinimum, actualArgMin[0], actualArgMin[1], expectedArgMin[0], expectedArgMin[1]));
            Assert.That(actualArgMin, Is.EqualTo(expectedArgMin).AsCollection.Within(1E-7), String.Format("State: {0}; actual Minimum: {1}; expected Minimum: {2}; actual argMin: [{3}; {4}]; expected argMin: [{5}; {6}].", state, actualMinimum, expectedMinimum, actualArgMin[0], actualArgMin[1], expectedArgMin[0], expectedArgMin[1]));
        }
    }
}