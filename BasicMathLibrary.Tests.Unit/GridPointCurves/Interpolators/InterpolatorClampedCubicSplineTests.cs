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

namespace Dodoni.MathLibrary.GridPointCurves
{
    /// <summary>Serves as unit test class for the clamped cubic spline curve interpolator.
    /// </summary>
    /// <remarks>The Benchmar example has been taken from http://banach.millersville.edu/~BobBuchanan/math375/CubicSpline/main.pdf. </remarks>
    [TestFixture]
    public class InterpolatorClampedCubicSplineTests
    {
        /// <summary>A test function for the GetIntegral method of a clamped cubic spline interpolator object.
        /// </summary>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="upperBound">The upper bound.</param>
        [TestCase(-0.6, 0.2)]
        [TestCase(0.05, 0.4)]
        public void GetIntegral_TestCase_BenchmarkNumericalIntegrator(double lowerBound, double upperBound)
        {
            var interpolator = GetTestCaseInterpolatorObject();

            double actual = interpolator.GetIntegral(lowerBound, upperBound);

            BenchmarkIntegrator benchmarkIntegrator = new BenchmarkIntegrator();
            benchmarkIntegrator.FunctionToIntegrate = interpolator.GetValue;

            double expected = benchmarkIntegrator.GetValue(lowerBound, upperBound);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-6));
        }

        /// <summary>A test function for the GetValue method of a clamped cubic spline interpolator object.
        /// </summary>
        /// <param name="x">The point where to evaluate.</param>
        [TestCase(-1.0)]
        [TestCase(-0.76)]
        [TestCase(0.12)]
        [TestCase(0.5)]
        public void GetValue_TestCaseData_AnalyticValue(double x)
        {
            var piecewiseConstantInterpolator = GetTestCaseInterpolatorObject();
            int gridPointCount = piecewiseConstantInterpolator.GridPointCount;

            double actual = piecewiseConstantInterpolator.GetValue(x);

            double expected = GetBenchmarkValueFunction()(x);
            Assert.That(actual, Is.EqualTo(expected).Within(1E-5));
        }

        #region protected methods

        /// <summary>Gets the interpolator object (without any grid points).
        /// </summary>
        /// <param name="derivativeAtFirstPoint">The derivative at first point.</param>
        /// <param name="derivativeAtLastPoint">The derivative at last point.</param>
        /// <returns>The <see cref="ICurveDataFitting"/> object under test.</returns>
        protected virtual ICurveDataFitting GetEmptyTestCaseInterpolatorObject(double derivativeAtFirstPoint, double derivativeAtLastPoint)
        {
            return GridPointCurve.Interpolator.Splines.Create(derivativeAtFirstPoint, derivativeAtLastPoint).Create();
        }
        #endregion

        #region private methods

        /// <summary>Gets the test case interpolator object.
        /// </summary>
        /// <returns>The <see cref="ICurveDataFitting"/> object under test.</returns>
        private ICurveDataFitting GetTestCaseInterpolatorObject()
        {
            var interpolator = GetEmptyTestCaseInterpolatorObject(0.155362, 0.451863);

            /* example taken from
             * http://banach.millersville.edu/~BobBuchanan/math375/CubicSpline/main.pdf
             */
            interpolator.Update(4, new[] { -1.0, -0.5, 0.0, 0.5 },
                                   new[] { 0.86199480, 0.95802009, 1.0986123, 1.2943767 });
            return interpolator;
        }

        /// <summary>Gets the benchmark value function.
        /// </summary>
        /// <returns>A function that returns the benchmark value of the interpolator object with respect to <see cref="GetTestCaseInterpolatorObject"/>.</returns>
        private Func<double, double> GetBenchmarkValueFunction()
        {
            /* example taken from
             * http://banach.millersville.edu/~BobBuchanan/math375/CubicSpline/main.pdf
             */
            return x =>
            {
                if (x <= -0.5)
                {
                    return 0.861995 + 0.155362 * (x + 1) + 0.0653748 * Math.Pow(x + 1, 2) + 0.0160031 * Math.Pow(x + 1, 3);
                }
                else if (x <= 0)
                {
                    return 0.95802 + 0.23274 * (x + 0.5) + 0.0893795 * Math.Pow(x + 0.5, 2) + 0.0150207 * Math.Pow(x + 0.5, 3);
                }
                else
                {
                    return 1.09861 + 0.333384 * x + 0.11191 * x * x + 0.00875717 * x * x * x;
                }
            };
        }
        #endregion
    }
}