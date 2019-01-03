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
    /// <summary>Serves as unit test class for the Natural cubic spline curve interpolator.
    /// </summary>
    /// <remarks>The Benchmark example has been taken from http://www.maths.nuigalway.ie/~niall/teaching/Archive/0910/MA378/Lecture09.pdf. </remarks>
    [TestFixture]
    public class InterpolatorNaturalCubicSplineTests
    {
        /// <summary>A test function for the GetIntegral method of a cubic spline interpolator object.
        /// </summary>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="upperBound">The upper bound.</param>
        [TestCase(0.0, 1.2)]
        [TestCase(0.05, 1.75)]
        [TestCase(1.8, 2.95)]
        public void GetIntegral_TestCase_BenchmarkNumericalIntegrator(double lowerBound, double upperBound)
        {
            var interpolator = GetTestCaseInterpolatorObject();

            double actual = interpolator.GetIntegral(lowerBound, upperBound);

            BenchmarkIntegrator benchmarkIntegrator = new BenchmarkIntegrator();
            benchmarkIntegrator.FunctionToIntegrate = interpolator.GetValue;

            double expected = benchmarkIntegrator.GetValue(lowerBound, upperBound);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-6));
        }

        /// <summary>A test function for the GetValue method of a Natural cubic spline interpolator object.
        /// </summary>
        /// <param name="x">The point where to evaluate.</param>
        [TestCase(1.0)]
        [TestCase(1.27)]
        [TestCase(2.3)]
        [TestCase(2.999)]
        public void GetValue_TestCaseData_AnalyticValue(double x)
        {
            var piecewiseConstantInterpolator = GetTestCaseInterpolatorObject();
            int gridPointCount = piecewiseConstantInterpolator.GridPointCount;

            double actual = piecewiseConstantInterpolator.GetValue(x);

            double expected = GetBenchmarkValueFunction()(x);
            Assert.That(actual, Is.EqualTo(expected).Within(1E-6));
        }

        #region protected methods

        /// <summary>Gets the interpolator object (without any grid points).
        /// </summary>
        /// <returns>The <see cref="ICurveDataFitting"/> object under test.</returns>
        protected virtual ICurveDataFitting GetEmptyTestCaseInterpolatorObject()
        {
            return GridPointCurve.Interpolator.Splines.NaturalCubicSpline.Create();
        }
        #endregion

        #region private methods

        /// <summary>Gets the test case interpolator object.
        /// </summary>
        /// <returns>The <see cref="ICurveDataFitting"/> object under test.</returns>
        private ICurveDataFitting GetTestCaseInterpolatorObject()
        {
            var interpolator = GetEmptyTestCaseInterpolatorObject();

            /* example taken from
             * http://www.maths.nuigalway.ie/~niall/teaching/Archive/0910/MA378/Lecture09.pdf 
             */
            interpolator.Update(4, new[] { 0.0, 1.0, 2.0, 3.0 },
                                   new[] { 0.0, 2.0, 1.0, 0.0 });
            return interpolator;
        }

        /// <summary>Gets the benchmark value function.
        /// </summary>
        /// <returns>A function that returns the benchmark value of the interpolator object with respect to <see cref="GetTestCaseInterpolatorObject"/>.</returns>
        private Func<double, double> GetBenchmarkValueFunction()
        {
            /* example taken from
             * http://www.maths.nuigalway.ie/~niall/teaching/Archive/0910/MA378/Lecture09.pdf 
             */
            return x =>
            {
                if (x <= 1)
                {
                    return 14.0 / 5.0 * x - 4.0 / 5.0 * x * x * x;
                }
                else if (x <= 2)
                {
                    return 4.0 / 5.0 * (x - 1) + 14.0 / 5.0 * (2.0 - x) - 4.0 / 5.0 * Math.Pow(2 - x, 3) + Math.Pow(x - 1.0, 3) / 5.0;
                }
                else
                {
                    return 4.0 / 5.0 * (3.0 - x) + Math.Pow(3.0 - x, 3) / 5.0;
                }
            };
        }
        #endregion
    }
}