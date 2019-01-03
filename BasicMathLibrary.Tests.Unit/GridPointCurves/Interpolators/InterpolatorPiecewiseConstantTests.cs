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
    /// <summary>Serves as unit test class for the piecewise constant curve interpolator.
    /// </summary>
    [TestFixture]
    public class InterpolatorPiecewiseConstantTests
    {
        /// <summary>A test function for the GetIntegral method of a piecewise constant interpolator object.
        /// </summary>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="upperBound">The upper bound.</param>
        [TestCase(0.0, 1.2)]
        [TestCase(-0.05, 2.75)]
        [TestCase(1.8, 4.75)]
        public void GetIntegral_TestCase_BenchmarkNumericalIntegrator(double lowerBound, double upperBound)
        {
            var piecewiseConstantInterpolator = GetTestCaseInterpolatorObject();

            double actual = piecewiseConstantInterpolator.GetIntegral(lowerBound, upperBound);

            BenchmarkIntegrator benchmarkIntegrator = new BenchmarkIntegrator();
            benchmarkIntegrator.FunctionToIntegrate = piecewiseConstantInterpolator.GetValue;

            double expected = benchmarkIntegrator.GetValue(lowerBound, upperBound);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-2));  // the Benchmark is not accurate for such a discontinous function
        }

        /// <summary>A test function for the GetValue method of a piecewise constant interpolator object.
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
            var piecewiseConstantInterpolator = GetTestCaseInterpolatorObject();
            int gridPointCount = piecewiseConstantInterpolator.GridPointCount;

            double actual = piecewiseConstantInterpolator.GetValue(x);
            double expected;
            if (x < piecewiseConstantInterpolator.GridPointArguments[gridPointCount - 1])
            {
                expected = piecewiseConstantInterpolator.GridPointValues[leftGridPointIndex];
            }
            else
            {
                expected = piecewiseConstantInterpolator.GridPointValues[gridPointCount - 1];
            }
            Assert.That(actual, Is.EqualTo(expected).Within(1E-6));
        }

        #region protected methods

        /// <summary>Gets the interpolator object (without any grid points).
        /// </summary>
        /// <returns>The <see cref="ICurveDataFitting"/> object under test.</returns>
        protected virtual ICurveDataFitting GetEmptyTestCaseInterpolatorObject()
        {
            return GridPointCurve.Interpolator.PiecewiseConstant.Create();
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
                                         new[] { 1.0, -2.5, 3.0, -1.25, 1.75 });
            return interpolator;
        }
        #endregion
    }
}