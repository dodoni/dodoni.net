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

using NUnit.Framework;

namespace Dodoni.MathLibrary.NumericalIntegrators
{
    /// <summary>Serves as unit test class for <see cref="IOneDimNumericalConstAbscissaIntegratorAlgorithm"/>.
    /// </summary>
    public class OneDimNumericalConstAbscissaIntegratorTests
    {
        /// <summary>A test function for the calculation of the estimated integration value w.r.t. a specific test case.
        /// </summary>
        /// <param name="numericalIntegrator">The numerical integrator.</param>
        [TestCaseSource("TestCaseDataWithIndividualIntegrationBorders")]
        public void GetValueWithState_ThreeTimesXSquare_BenchmarkResult(IOneDimNumericalConstAbscissaIntegratorAlgorithm numericalIntegrator)
        {
            double lowerBound = 1.0;
            double upperBound = 10.0;

            Assume.That(numericalIntegrator.TrySetBounds(lowerBound, upperBound) == true, String.Format("1-dimensional Integrator {0} does not support individual lower/upper bounds", numericalIntegrator.Factory.Name.String));

            var xValues = numericalIntegrator.Abscissas.ToArray();
            var individualTestValues = new double[xValues.Length];

            numericalIntegrator.FunctionToIntegrate = (x_k, k) => 3.0 * x_k * x_k;

            double expected = upperBound * upperBound * upperBound - lowerBound * lowerBound * lowerBound;
            double actual;
            var state = numericalIntegrator.GetValue(out actual);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-5), String.Format("1-dimensional integrator {0}; state: {1}.", numericalIntegrator.Factory.Name.String, state.ToString()));
        }

        /// <summary>A test function for the calculation of the estimated integration value w.r.t. a specific test case.
        /// </summary>
        /// <param name="numericalIntegrator">The numerical integrator.</param>
        [TestCaseSource("TestCaseDataWithIndividualIntegrationBorders")]
        public void GetValue_ThreeTimesXSquare_BenchmarkResult(IOneDimNumericalConstAbscissaIntegratorAlgorithm numericalIntegrator)
        {
            double lowerBound = 1.0;
            double upperBound = 10.0;

            Assume.That(numericalIntegrator.TrySetBounds(lowerBound, upperBound) == true, String.Format("1-dimensional Integrator {0} does not support individual lower/upper bounds", numericalIntegrator.Factory.Name.String));

            var xValues = numericalIntegrator.Abscissas.ToArray();
            var individualTestValues = new double[xValues.Length];

            numericalIntegrator.FunctionToIntegrate = (x_k, k) => 3.0 * x_k * x_k;

            double expected = upperBound * upperBound * upperBound - lowerBound * lowerBound * lowerBound;
            double actual = numericalIntegrator.GetValue();

            Assert.That(actual, Is.EqualTo(expected).Within(1E-5), String.Format("1-dimensional integrator {0}.", numericalIntegrator.Factory.Name.String));
        }

        /// <summary>A test function for the calculation of the estimated integration value w.r.t. a specific test case.
        /// </summary>
        /// <param name="numericalIntegrator">The numerical integrator.</param>
        [TestCaseSource("TestCaseDataWithIndividualIntegrationBorders")]
        public void GetValueWithState_ExponentialFunction_BenchmarkResult(IOneDimNumericalConstAbscissaIntegratorAlgorithm numericalIntegrator)
        {
            double lowerBound = 1.0;
            double upperBound = 10.0;

            Assume.That(numericalIntegrator.TrySetBounds(lowerBound, upperBound) == true, String.Format("1-dimensional Integrator {0} does not support individual lower/upper bounds", numericalIntegrator.Factory.Name.String));

            numericalIntegrator.FunctionToIntegrate = (x_k, k) => Math.Exp(x_k);

            double expected = Math.Exp(upperBound) - Math.Exp(lowerBound);
            double actual;
            var state = numericalIntegrator.GetValue(out actual);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-5), String.Format("1-dimensional integrator {0}; state: {1}.", numericalIntegrator.Factory.Name.String, state.ToString()));
        }

        /// <summary>A test function for the calculation of the estimated integration value w.r.t. a specific test case.
        /// </summary>
        /// <param name="numericalIntegrator">The numerical integrator.</param>
        [TestCaseSource("TestCaseDataWithIndividualIntegrationBorders")]
        public void GetValue_ExponentialFunction_BenchmarkResult(IOneDimNumericalConstAbscissaIntegratorAlgorithm numericalIntegrator)
        {
            double lowerBound = 1.0;
            double upperBound = 10.0;

            Assume.That(numericalIntegrator.TrySetBounds(lowerBound, upperBound) == true, String.Format("1-dimensional Integrator {0} does not support individual lower/upper bounds", numericalIntegrator.Factory.Name.String));

            numericalIntegrator.FunctionToIntegrate = (x_k, k) => Math.Exp(x_k);

            double expected = Math.Exp(upperBound) - Math.Exp(lowerBound);
            double actual = numericalIntegrator.GetValue();

            Assert.That(actual, Is.EqualTo(expected).Within(1E-5), String.Format("1-dimensional integrator {0}.", numericalIntegrator.Factory.Name.String));
        }

        /// <summary>Gets the test case data with individual integration borders.
        /// </summary>
        /// <value>The test case data with individual integration borders.</value>
        public static IEnumerable<TestCaseData> TestCaseDataWithIndividualIntegrationBorders
        {
            get
            {
                yield return new TestCaseData(new GaussKronrodPatterson255ConstAbscissaIntegrator().Create());
                yield return new TestCaseData(new GaussKronrodPatterson87ConstAbscissaIntegrator().Create());
                yield return new TestCaseData(new GaussLegendreConstAbscissaIntegrator().Create());
            }
        }
    }
}