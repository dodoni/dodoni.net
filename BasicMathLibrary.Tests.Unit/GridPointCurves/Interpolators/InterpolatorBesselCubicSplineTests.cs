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
using NUnit.Framework;

namespace Dodoni.MathLibrary.GridPointCurves
{
    /// <summary>Serves as unit test class for the Bessel cubic spline curve interpolator.
    /// </summary>   
    [TestFixture]
    public class InterpolatorBesselCubicSplineTests
    {
        /// <summary>A test function for the GetIntegral method of a Bessel cubic spline interpolator object.
        /// </summary>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="upperBound">The upper bound.</param>
        [TestCase(0.0, 1.2)]
        [TestCase(0.25, 1.75)]
        public void GetIntegral_TestCase_BenchmarkNumericalIntegrator(double lowerBound, double upperBound)
        {
            var interpolator = GetTestCaseInterpolatorObject();

            double actual = interpolator.GetIntegral(lowerBound, upperBound);

            BenchmarkIntegrator benchmarkIntegrator = new BenchmarkIntegrator();
            benchmarkIntegrator.FunctionToIntegrate = interpolator.GetValue;

            double expected = benchmarkIntegrator.GetValue(lowerBound, upperBound);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-6));
        }

        /// <summary>A test function for the GetValue method of a Bessel cubic spline interpolator object.
        /// </summary>
        /// <param name="x">The point where to evaluate.</param>
        /// <param name="expected">The expected value at <paramref name="x"/>.</param>
        /// <remarks>The expected values have been calculated with Intel MKL Library 11.0 Update 5.</remarks>
        [TestCase(0.25, 0.0625)]
        [TestCase(0.6, 0.6352)]
        [TestCase(1.15, 1.10785)]
        [TestCase(1.82, 1.43952)]
        [TestCase(2.0, 1.5)]
        public void GetValue_TestCaseData_BenchmarkResults(double x, double expected)
        {
            var interpolator = GetTestCaseInterpolatorObject();
            int gridPointCount = interpolator.GridPointCount;

            double actual = interpolator.GetValue(x);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-6));
        }

        #region protected methods

        /// <summary>Gets the interpolator object (without any grid points).
        /// </summary>
        /// <returns>The <see cref="ICurveDataFitting"/> object under test.</returns>
        protected virtual ICurveDataFitting GetEmptyTestCaseInterpolatorObject()
        {
            return GridPointCurve.Interpolator.Splines.BesselCubicSpline.Create();
        }
        #endregion

        #region private methods

        /// <summary>Gets the test case interpolator object.
        /// </summary>
        /// <returns>The <see cref="ICurveDataFitting"/> object under test.</returns>
        private ICurveDataFitting GetTestCaseInterpolatorObject()
        {
            var interpolator = GetEmptyTestCaseInterpolatorObject();

            interpolator.Update(5, new[] { 0.0, 0.5, 1.0, 1.5, 2.0 },
                                   new[] { -0.5, 0.5, 1.0, 1.3, 1.5 });
            return interpolator;
        }
        #endregion
    }
}