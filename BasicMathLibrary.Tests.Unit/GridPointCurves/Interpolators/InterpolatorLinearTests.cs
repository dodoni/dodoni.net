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
    /// <summary>Serves as unit test class for the linear curve interpolator.
    /// </summary>
    [TestFixture]
    public class InterpolatorLinearTests
    {
        /// <summary>A test function for the GetIntegral method of a linear interpolator object.
        /// </summary>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="upperBound">The upper bound.</param>
        [TestCase(0.0, 1.2)]
        [TestCase(-0.05, 2.75)]
        [TestCase(1.8, 4.75)]
        public void GetIntegral_TestCase_BenchmarkNumericalIntegrator(double lowerBound, double upperBound)
        {
            var linearInterpolator = GetTestCaseInterpolatorObject();

            double actual = linearInterpolator.GetIntegral(lowerBound, upperBound);

            BenchmarkIntegrator benchmarkIntegrator = new BenchmarkIntegrator();
            benchmarkIntegrator.FunctionToIntegrate = linearInterpolator.GetValue;

            double expected = benchmarkIntegrator.GetValue(lowerBound, upperBound);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-5));
        }

        /// <summary>A test function for the GetIntegral method of a linear interpolator object.
        /// </summary>
        [Test]
        public void GetIntegral_TestCaseFromGridPointToGridPoint_AnalyticResult()
        {
            var linearInterpolator = GetTestCaseInterpolatorObject();

            for (int j = 0; j < linearInterpolator.GridPointCount - 1; j++)
            {
                double lowerIntegralBound = linearInterpolator.GridPointArguments[j];
                double upperIntegralBound = linearInterpolator.GridPointArguments[j + 1];

                double actual = linearInterpolator.GetIntegral(lowerIntegralBound, upperIntegralBound);

                double m = (linearInterpolator.GridPointValues[j + 1] - linearInterpolator.GridPointValues[j]) / (linearInterpolator.GridPointArguments[j + 1] - linearInterpolator.GridPointArguments[j]);
                double b = linearInterpolator.GridPointValues[j] - m * linearInterpolator.GridPointArguments[j];

                double expected = 0.5 * m * upperIntegralBound * upperIntegralBound + upperIntegralBound * b - (0.5 * m * lowerIntegralBound * lowerIntegralBound + b * lowerIntegralBound);

                Assert.That(actual, Is.EqualTo(expected).Within(1E-6), String.Format("Integral from gridpoints: {0} to {1}; null-based index of the left grid point {2}.", lowerIntegralBound, upperIntegralBound, j));
            }
        }

        /// <summary>A test function for the GetIntegral method of a linear interpolator object.
        /// </summary>
        [Test]
        public void GetIntegral_TestCaseInsideTwoGridPoints_AnalyticResult()
        {
            var linearInterpolator = GetTestCaseInterpolatorObject();

            for (int j = 0; j < linearInterpolator.GridPointCount - 1; j++)
            {
                double width = linearInterpolator.GridPointArguments[j + 1] - linearInterpolator.GridPointArguments[j];

                double lowerIntegralBound = linearInterpolator.GridPointArguments[j] + width / 4.0;
                double upperIntegralBound = linearInterpolator.GridPointArguments[j + 1] - width / 4.0;

                double actual = linearInterpolator.GetIntegral(lowerIntegralBound, upperIntegralBound);

                double m = (linearInterpolator.GridPointValues[j + 1] - linearInterpolator.GridPointValues[j]) / (linearInterpolator.GridPointArguments[j + 1] - linearInterpolator.GridPointArguments[j]);
                double b = linearInterpolator.GridPointValues[j] - m * linearInterpolator.GridPointArguments[j];

                double expected = 0.5 * m * upperIntegralBound * upperIntegralBound + upperIntegralBound * b - (0.5 * m * lowerIntegralBound * lowerIntegralBound + b * lowerIntegralBound);

                Assert.That(actual, Is.EqualTo(expected).Within(1E-6), String.Format("Integral from gridpoints: {0} to {1}; null-based index of the left grid point {2}.", lowerIntegralBound, upperIntegralBound, j));
            }
        }

        /// <summary>A test function for the GetIntegral method of a linear interpolator object.
        /// </summary>
        [Test]
        public void GetIntegral_TestCase_AnalyticResult()
        {
            var linearInterpolator = GetTestCaseInterpolatorObject();

            for (int j = 0; j < linearInterpolator.GridPointCount - 2; j++)
            {
                var lowerGridPointArgument = linearInterpolator.GridPointArguments[j];
                var lowerGridPointValue = linearInterpolator.GridPointValues[j];

                var midGridPointArgument = linearInterpolator.GridPointArguments[j + 1];
                var midGridPointValue = linearInterpolator.GridPointValues[j + 1];

                var higherGridPointArgument = linearInterpolator.GridPointArguments[j + 2];
                var higherGridPointValue = linearInterpolator.GridPointValues[j + 2];

                var width = Math.Min(midGridPointArgument - lowerGridPointArgument, higherGridPointArgument - midGridPointArgument);

                var lowerIntegralBound = lowerGridPointArgument + width / 4.0;
                var upperIntegralBound = higherGridPointArgument - width / 4.0;

                var actual = linearInterpolator.GetIntegral(lowerIntegralBound, upperIntegralBound);


                var m1 = (midGridPointValue - lowerGridPointValue) / (midGridPointArgument - lowerGridPointArgument);
                var b1 = lowerGridPointValue - m1 * lowerGridPointArgument;

                var integralValueFromlowerBoundToMidGridPoint = 0.5 * m1 * midGridPointArgument * midGridPointArgument + midGridPointArgument * b1 - (0.5 * m1 * lowerIntegralBound * lowerIntegralBound + b1 * lowerIntegralBound);

                var m2 = (higherGridPointValue - midGridPointValue) / (higherGridPointArgument - midGridPointArgument);
                var b2 = midGridPointValue - m2 * midGridPointArgument;

                double integralValueFromMidGridPointToUpperBound = 0.5 * m2 * upperIntegralBound * upperIntegralBound + upperIntegralBound * b2 - (0.5 * m2 * midGridPointArgument * midGridPointArgument + b2 * midGridPointArgument);

                double expected = integralValueFromlowerBoundToMidGridPoint + integralValueFromMidGridPointToUpperBound;
                Assert.That(actual, Is.EqualTo(expected).Within(1E-6), String.Format("Integral from gridpoints: {0} to {1}; null-based index of the left grid point {2}.", lowerIntegralBound, upperIntegralBound, j));
            }
        }

        /// <summary>A test function for the GetValue method of a linear interpolator object.
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
            var linearInterpolator = GetTestCaseInterpolatorObject();

            double actual = linearInterpolator.GetValue(x);

            double m = (linearInterpolator.GridPointValues[leftGridPointIndex] - linearInterpolator.GridPointValues[leftGridPointIndex + 1]) / (linearInterpolator.GridPointArguments[leftGridPointIndex] - linearInterpolator.GridPointArguments[leftGridPointIndex + 1]);
            double b = linearInterpolator.GridPointValues[leftGridPointIndex] - m * linearInterpolator.GridPointArguments[leftGridPointIndex];
            double expected = m * x + b;

            Assert.That(actual, Is.EqualTo(expected).Within(1E-6));
        }

        #region protected methods

        /// <summary>Gets the interpolator object (without any grid points).
        /// </summary>
        /// <returns>The <see cref="ICurveDataFitting"/> object under test.</returns>
        protected virtual ICurveDataFitting GetEmptyTestCaseInterpolatorObject()
        {
           return GridPointCurve.Interpolator.Linear.Create();
        }       
        #endregion

        #region private methods

        /// <summary>Gets the test case interpolator object.
        /// </summary>
        /// <returns>The <see cref="ICurveDataFitting"/> object under test.</returns>
        private ICurveDataFitting GetTestCaseInterpolatorObject()
        {
            var linearInterpolator = GetEmptyTestCaseInterpolatorObject();

            linearInterpolator.Update(5, new[] { -0.5, 0.0, 0.1, 2.75, 5.0 },
                                         new[] { 1.0, -2.5, 3.0, -1.25, 1.75 });
            return linearInterpolator;
        }
        #endregion
    }
}