using System;

using NUnit.Framework;
using Dodoni.MathLibrary.Miscellaneous;

namespace Dodoni.MathLibrary.Optimizer.MultiDimensional
{
    [TestFixture]
    public class LevenbergMarquardtOptimizerTests
    {
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>This example has been taken from the homepage of the Institute für Geometrie und praktische Mathematik, http://www.igpm.rwth-aachen.de/Numa/NumaMB/SS10/LevMarq.pdf </remarks>
        [Test]
        public void FindMinimum_SimpleTestFunction_AnalyticResult()
        {
            var optimizer = new LevenbergMarquardtOptimizer();

            var algorithm = optimizer.Create(2);

            algorithm.Function = optimizer.Function.Create(2, 3,
                (x, J, y) =>
                {
                    if (J != null)
                    {
                        J[0] = -4 + 2 * x[0];
                        J[1] = -6 + 2 * x[0];
                        J[2] = -8 + 2 * x[0];
                        J[3] = 4 * Math.Exp(4 * x[1]);
                        J[4] = 13 * Math.Exp(13 * x[1]);
                        J[5] = 16 * Math.Exp(16 * x[1]);
                    }

                    y[0] = Math.Pow(2.0 - x[0], 2) + Math.Exp(x[1] * 4) - 5.0;
                    y[1] = Math.Pow(3.0 - x[0], 2) + Math.Exp(x[1] * 13) - 5.0;
                    y[2] = Math.Pow(4.0 - x[0], 2) + Math.Exp(x[1] * 16) - 5.0;
                });

            var actualArgMin = new[] { 4.0, 0.0 };
            var state = algorithm.FindMinimum(actualArgMin, out double actualMinimum);

            var expectedArgMin = new[] { 3.915042527, 0.1029172979 };
            var expectedMinimum = 0.19361117440123918;

            Assert.That(actualMinimum, Is.EqualTo(expectedMinimum).Within(1E-5), String.Format("State: {0}; actual Minimum: {1}; expected Minimum: {2}; actual argMin: [{3}; {4}]; expected argMin: [{5}; {6}].", state, actualMinimum, expectedMinimum, actualArgMin[0], actualArgMin[1], expectedArgMin[0], expectedArgMin[1]));
            Assert.That(actualArgMin, Is.EqualTo(expectedArgMin).AsCollection.Within(1E-5), String.Format("State: {0}; actual Minimum: {1}; expected Minimum: {2}; actual argMin: [{3}; {4}]; expected argMin: [{5}; {6}].", state, actualMinimum, expectedMinimum, actualArgMin[0], actualArgMin[1], expectedArgMin[0], expectedArgMin[1]));
        }

        /// <summary>
        /// Diese Testfunktion ist zwar korrekt, entspricht jedoch nicht der Referenz!
        /// </summary>
        /// <remarks>This example has been taken from W. Hock and K. Schittkowski, Test Examples for Nonlinear Programming Codes, Springer Series Lectures Notes in Economics Mathematical Systems, 1981; p.34 (Problem 11, Classification QQR-T1-2).</remarks>
        [Test]
        public void FindMinimum_SimpBiggsTestFunction_AnalyticResult()
        {
            var optimizer = new LevenbergMarquardtOptimizer();

            var algorithm = optimizer.Create(optimizer.Constraint.Create(MultiDimRegion.Interval.Create(2, new[] { 1.0, 0.0 }, new[] { Double.PositiveInfinity, Double.PositiveInfinity })));

            algorithm.Function = optimizer.Function.Create(2, 2,
                (x, J, y) =>
                {
                    if (J != null)
                    {
                        J[0] = 1.0;
                        J[1] = 0.0;
                        J[2] = 0.0;
                        J[3] = 1.0;
                    }
                    y[0] = x[0] - 5.0;
                    y[1] = x[1];
                });

            var actualArgMin = new[] { 3.1, 1.2 }; // not feasible
            var state = algorithm.FindMinimum(actualArgMin, out double actualMinimum);

            var expectedArgMin = new[] { 5.0, 0.0 };
            var expectedMinimum = 0.0;

            Assert.That(actualMinimum, Is.EqualTo(expectedMinimum).Within(1E-5), String.Format("State: {0}; actual Minimum: {1}; expected Minimum: {2}; actual argMin: [{3}; {4}]; expected argMin: [{5}; {6}].", state, actualMinimum, expectedMinimum, actualArgMin[0], actualArgMin[1], expectedArgMin[0], expectedArgMin[1]));
            Assert.That(actualArgMin, Is.EqualTo(expectedArgMin).AsCollection.Within(1E-5), String.Format("State: {0}; actual Minimum: {1}; expected Minimum: {2}; actual argMin: [{3}; {4}]; expected argMin: [{5}; {6}].", state, actualMinimum, expectedMinimum, actualArgMin[0], actualArgMin[1], expectedArgMin[0], expectedArgMin[1]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>This example has been taken from W. Hock and K. Schittkowski, Test Examples for Nonlinear Programming Codes, Springer Series Lectures Notes in Economics Mathematical Systems, 1981; p.44 (Problem 21, Classification QLR-T1-1).</remarks>
        [Test]
        public void FindMinimum_BettsTestFunction_AnalyticResult()
        {
            /* f(x) = 0.01 * x_0^2 + x_1^2 w.r.t 10*x_0 - x_1 >= 10, 2 <= x_0 <= 50, -50 <= x_1 <= 50 */
            var optimizer = new LevenbergMarquardtOptimizer();

            var box = MultiDimRegion.Interval.Create(2, new[] { 2.0, -50.0 }, new[] { 50.0, 50.0 });
            var linConstraint = new MultiDimRegion.LinearInequality(new DenseMatrix(2, 1, new[] { 10.0, -1.0 }), new[] { 10.0 });

            var algorithm = optimizer.Create(optimizer.Constraint.Create(box), optimizer.Constraint.Create(linConstraint));

            algorithm.Function = optimizer.Function.Create(2, 2,
                (x, J, y) =>
                {
                    if (J != null)
                    {
                        J[0] = 0.01;
                        J[1] = 0.0;
                        J[2] = 0.0;
                        J[3] = 1.0;
                    }
                    y[0] = 0.1 * x[0];
                    y[1] = x[1];
                });

            var actualArgMin = new[] { 4.0, 5.0 }; // use feasible initial points
            var state = algorithm.FindMinimum(actualArgMin, out double actualMinimum);

            var expectedArgMin = new[] { 2.0, 0.0 };
            var expectedMinimum = 0.04;

            Assert.That(actualMinimum, Is.EqualTo(expectedMinimum).Within(1E-5), String.Format("State: {0}; actual Minimum: {1}; expected Minimum: {2}; actual argMin: [{3}; {4}]; expected argMin: [{5}; {6}].", state, actualMinimum, expectedMinimum, actualArgMin[0], actualArgMin[1], expectedArgMin[0], expectedArgMin[1]));
            Assert.That(actualArgMin, Is.EqualTo(expectedArgMin).AsCollection.Within(1E-5), String.Format("State: {0}; actual Minimum: {1}; expected Minimum: {2}; actual argMin: [{3}; {4}]; expected argMin: [{5}; {6}].", state, actualMinimum, expectedMinimum, actualArgMin[0], actualArgMin[1], expectedArgMin[0], expectedArgMin[1]));
        }
    }
}