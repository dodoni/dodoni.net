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

namespace Dodoni.MathLibrary.GridPointCurves
{
    /// <summary>Serves as unit test class for the log-linear curve interpolator.
    /// </summary>
    [TestFixture]
    public class InterpolatorLogLinearTests
    {
        /// <summary>A test function for the GetIntegral method of a log-linear interpolator object.
        /// </summary>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="upperBound">The upper bound.</param>
        [TestCase(0.0, 1.2)]
        [TestCase(-0.05, 2.75)]
        [TestCase(1.8, 4.75)]
        public void GetIntegral_TestCase_BenchmarkNumericalIntegrator(double lowerBound, double upperBound)
        {
            var logLinearInterpolator = GetTestCaseInterpolatorObject();

            double actual = logLinearInterpolator.GetIntegral(lowerBound, upperBound);

            BenchmarkIntegrator benchmarkIntegrator = new BenchmarkIntegrator();
            benchmarkIntegrator.FunctionToIntegrate = logLinearInterpolator.GetValue;

            double expected = benchmarkIntegrator.GetValue(lowerBound, upperBound);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-5));
        }

        /// <summary>A test function for the GetIntegral method of a log-linear interpolator object.
        /// </summary>
        [Test]
        public void GetIntegral_TestCaseFromGridPointToGridPoint_AnalyticResult()
        {
            var logLinearInterpolator = GetTestCaseInterpolatorObject();

            for (int j = 0; j < logLinearInterpolator.GridPointCount - 1; j++)
            {
                double lowerIntegralBound = logLinearInterpolator.GridPointArguments[j];
                double upperIntegralBound = logLinearInterpolator.GridPointArguments[j + 1];

                double actual = logLinearInterpolator.GetIntegral(lowerIntegralBound, upperIntegralBound);

                double m = (Math.Log(logLinearInterpolator.GridPointValues[j + 1]) - Math.Log(logLinearInterpolator.GridPointValues[j])) / (logLinearInterpolator.GridPointArguments[j + 1] - logLinearInterpolator.GridPointArguments[j]);
                double b = Math.Log(logLinearInterpolator.GridPointValues[j]) - m * logLinearInterpolator.GridPointArguments[j];

                double expected = Math.Exp(m * upperIntegralBound + b) / m - Math.Exp(m * lowerIntegralBound + b) / m;

                Assert.That(actual, Is.EqualTo(expected).Within(1E-6), String.Format("Integral from gridpoints: {0} to {1}; null-based index of the left grid point {2}.", lowerIntegralBound, upperIntegralBound, j));
            }
        }

        /// <summary>A test function for the GetIntegral method of a log-linear interpolator object.
        /// </summary>
        [Test]
        public void GetIntegral_TestCaseInsideTwoGridPoints_AnalyticResult()
        {
            var logLinearInterpolator = GetTestCaseInterpolatorObject();

            for (int j = 0; j < logLinearInterpolator.GridPointCount - 1; j++)
            {
                double width = logLinearInterpolator.GridPointArguments[j + 1] - logLinearInterpolator.GridPointArguments[j];

                double lowerIntegralBound = logLinearInterpolator.GridPointArguments[j] + width / 4.0;
                double upperIntegralBound = logLinearInterpolator.GridPointArguments[j + 1] - width / 4.0;

                double actual = logLinearInterpolator.GetIntegral(lowerIntegralBound, upperIntegralBound);

                double m = (Math.Log(logLinearInterpolator.GridPointValues[j + 1]) - Math.Log(logLinearInterpolator.GridPointValues[j])) / (logLinearInterpolator.GridPointArguments[j + 1] - logLinearInterpolator.GridPointArguments[j]);
                double b = Math.Log(logLinearInterpolator.GridPointValues[j]) - m * logLinearInterpolator.GridPointArguments[j];

                double expected = Math.Exp(m * upperIntegralBound + b) / m - Math.Exp(m * lowerIntegralBound + b) / m;

                Assert.That(actual, Is.EqualTo(expected).Within(1E-6), String.Format("Integral from gridpoints: {0} to {1}; null-based index of the left grid point {2}.", lowerIntegralBound, upperIntegralBound, j));
            }
        }

        /// <summary>A test function for the GetIntegral method of a log-linear interpolator object.
        /// </summary>
        [Test]
        public void GetIntegral_TestCase_AnalyticResult()
        {
            var logLinearInterpolator = GetTestCaseInterpolatorObject();

            for (int j = 0; j < logLinearInterpolator.GridPointCount - 2; j++)
            {
                var lowerGridPointArgument = logLinearInterpolator.GridPointArguments[j];
                var lowerGridPointValue = logLinearInterpolator.GridPointValues[j];

                var midGridPointArgument = logLinearInterpolator.GridPointArguments[j + 1];
                var midGridPointValue = logLinearInterpolator.GridPointValues[j + 1];

                var higherGridPointArgument = logLinearInterpolator.GridPointArguments[j + 2];
                var higherGridPointValue = logLinearInterpolator.GridPointValues[j + 2];

                var width = Math.Min(midGridPointArgument - lowerGridPointArgument, higherGridPointArgument - midGridPointArgument);

                var lowerIntegralBound = lowerGridPointArgument + width / 4.0;
                var upperIntegralBound = higherGridPointArgument - width / 4.0;

                var actual = logLinearInterpolator.GetIntegral(lowerIntegralBound, upperIntegralBound);


                var m1 = (Math.Log(midGridPointValue) - Math.Log(lowerGridPointValue)) / (midGridPointArgument - lowerGridPointArgument);
                var b1 = Math.Log(lowerGridPointValue) - m1 * lowerGridPointArgument;

                var integralValueFromlowerBoundToMidGridPoint = Math.Exp(m1 * midGridPointArgument + b1) / m1 - Math.Exp(m1 * lowerIntegralBound + b1) / m1;

                var m2 = (Math.Log(higherGridPointValue) - Math.Log(midGridPointValue)) / (higherGridPointArgument - midGridPointArgument);
                var b2 = Math.Log(midGridPointValue) - m2 * midGridPointArgument;

                double integralValueFromMidGridPointToUpperBound = Math.Exp(m2 * upperIntegralBound + b2) / m2 - Math.Exp(m2 * midGridPointArgument + b2) / m2;

                double expected = integralValueFromlowerBoundToMidGridPoint + integralValueFromMidGridPointToUpperBound;
                Assert.That(actual, Is.EqualTo(expected).Within(1E-6), String.Format("Integral from gridpoints: {0} to {1}; null-based index of the left grid point {2}.", lowerIntegralBound, upperIntegralBound, j));
            }
        }

        /// <summary>A test function for the GetValue method of a log-linear interpolator object.
        /// </summary>
        /// <param name="x">The point where to evaluate.</param>
        /// <param name="leftGridPointIndex">The null-based index of the left grid point with respect to <paramref name="x"/>.</param>
        [TestCase(1.0, 2)]
        [TestCase(1.5, 2)]
        [TestCase(-0.5, 0)]
        [TestCase(-0.2, 0)]
        [TestCase(5.0, 3)]
        public void GetValue_TestCaseData_AnalyticValue(double x, int leftGridPointIndex)
        {
            var logLinearInterpolator = GetTestCaseInterpolatorObject();

            double actual = logLinearInterpolator.GetValue(x);

            double m = (Math.Log(logLinearInterpolator.GridPointValues[leftGridPointIndex]) - Math.Log(logLinearInterpolator.GridPointValues[leftGridPointIndex + 1])) / (logLinearInterpolator.GridPointArguments[leftGridPointIndex] - logLinearInterpolator.GridPointArguments[leftGridPointIndex + 1]);
            double b = Math.Log(logLinearInterpolator.GridPointValues[leftGridPointIndex]) - m * logLinearInterpolator.GridPointArguments[leftGridPointIndex];
            double expected = Math.Exp(m * x + b);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-6));
        }

        #region protected methods

        /// <summary>Gets the interpolator object (without any grid points).
        /// </summary>
        /// <returns>The <see cref="ICurveDataFitting"/> object under test.</returns>
        protected virtual ICurveDataFitting GetEmptyTestCaseInterpolatorObject()
        {
            return GridPointCurve.Interpolator.LogLinear.Create();
        }
        #endregion

        #region private methods

        /// <summary>Gets the test case interpolator object.
        /// </summary>
        /// <returns>The <see cref="ICurveDataFitting"/> object under test.</returns>
        private ICurveDataFitting GetTestCaseInterpolatorObject()
        {
            var interpolator = GetEmptyTestCaseInterpolatorObject();

            interpolator.Update(5, new[] { -0.5, 0.0, 0.1, 2.75, 5.0 },
                                         new[] { 1.0, 2.5, 3.0, 1.25, 1.75 });
            return interpolator;
        }
        #endregion
    }
}