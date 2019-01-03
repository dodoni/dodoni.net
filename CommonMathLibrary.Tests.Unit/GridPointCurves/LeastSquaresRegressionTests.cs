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
using System.Text;

using NUnit.Framework;

namespace Dodoni.MathLibrary.GridPointCurves
{
    /// <summary>Serves as unit test class for <see cref="LeastSquaresRegression"/>.
    /// </summary>
    public class LeastSquaresRegressionTests
    {
        /// <summary>A test function for a specific <see cref="LeastSquaresRegression"/> object.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <remarks>This example has been taken from http://people.stern.nyu.edu/wgreene/Statistics/SimpleLinearRegressionCollection.pdf. </remarks>
        [Test]
        public void GetValue_TestCaseDataOrder1_BenchmarkResult1([Values(0.0, 1.2, 4.64, 8.98)] double x)
        {
            int gridPointCount = 10;
            var gridPointArguments = new[] { 1.0, 2.0, 3.0, 4.0, 4.0, 5.0, 5.0, 6.0, 6.0, 7.0 };
            var gridPointValues = new[] { 7.0, 8.0, 9.0, 8.0, 9.0, 11.0, 10.0, 13.0, 14.0, 13.0 };

            var leastSquaresRegression = new LeastSquaresRegression(1, BasisFunctions.Monomial);
            var curve = leastSquaresRegression.Create();
            curve.Update(gridPointCount, gridPointArguments, gridPointValues);

            double actual = curve.GetValue(x);

            double xSumDivN = 4.3; // = 1/n * \sum_j x_j
            double ySumDivN = 10.2;
            double Sxy = 37.4;  // = \sum x_j * y_j - 1/n * \sum x_j * \sum y_j
            double Sxx = 32.1; // = \sum x_j^2 - 1/n * (\sum x_j)^2

            // y = m * x + b, where 
            var m = Sxy / Sxx;  // = 1.1651090342679127	
            var b = ySumDivN - m * xSumDivN;  //  = 5.19...

            double expected = m * x + b;

            Assert.That(actual, Is.EqualTo(expected).Within(1E-7));
        }

        /// <summary>A test function for a specific <see cref="LeastSquaresRegression"/> object.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <remarks>This example has been taken from http://wwww.cba.edu.kw/zainal/docs/220Docs/Regression%20Analysis.pdf. </remarks>
        [Test]
        public void GetValue_TestCaseDataOrder1_BenchmarkResult2([Values(1.2, 11.421)]double x)
        {
            int gridPointCount = 8;
            var gridPointArguments = new[] { 5.0, 2.0, 12.0, 9.0, 15.0, 6.0, 25.0, 16.0 };
            var gridPointValues = new[] { 64.0, 87.0, 50.0, 71.0, 44.0, 56.0, 42.0, 60.0 };

            var leastSquaresRegression = new LeastSquaresRegression(1, BasisFunctions.Monomial);
            var curve = leastSquaresRegression.Create();
            curve.Update(gridPointCount, gridPointArguments, gridPointValues);

            double actual = curve.GetValue(x);

            double xSumDivN = 11.25;
            double ySumDivN = 59.25;
            double Sxx = 383.5;
            double Sxy = -593.5;

            // y = m * x +b, where             
            var m = Sxy / Sxx; // = 1.5476...
            var b = ySumDivN - m * xSumDivN; // =  76.6605...
            double expected = m * x + b;

            Assert.That(actual, Is.EqualTo(expected).Within(1E-7));
        }

        /// <summary>A test function for a specific <see cref="LeastSquaresRegression"/> object.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <remarks>This example has been taken from http://www.math.washington.edu/~conroy/m112-general/leastSquaresExample.pdf, but the paper contains a typo and thus wrong slope and constant argument of the fitted line.</remarks>
        [Test]
        public void GetValue_TestCaseDataOrder1_BenchmarkResult3([Values(2.67, 7.85)] double x)
        {
            int gridPointCount = 6;
            var gridPointArguments = new[] { 1.2, 2.3, 3.0, 3.8, 4.7, 5.9 };
            var gridPointValues = new[] { 1.1, 2.1, 3.1, 4.0, 4.9, 5.9 };

            var leastSquaresRegression = new LeastSquaresRegression(1, BasisFunctions.Monomial);
            var curve = leastSquaresRegression.Create();
            curve.Update(gridPointCount, gridPointArguments, gridPointValues);

            double actual = curve.GetValue(x);

            var m = 1.0506950122649; // y = m * x +b, calculated by Microsoft Excel and with 'paper and pencil'
            var b = -0.1432542927228;

            double expected = m * x + b;
            Assert.That(actual, Is.EqualTo(expected).Within(1E-7));
        }
    }
}