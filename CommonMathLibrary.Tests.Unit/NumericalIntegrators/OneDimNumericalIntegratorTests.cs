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

namespace Dodoni.MathLibrary.NumericalIntegrators
{
    /// <summary>Serves as unit test class for <see cref="OneDimNumericalIntegrator"/>.
    /// </summary>
    public class OneDimNumericalIntegratorTests
    {
        /// <summary>A test function for the calculation of the estimated integration value w.r.t. a specific test case.
        /// </summary>
        /// <param name="numericalIntegrator">The numerical integrator.</param>
        [TestCaseSource("TestCaseDataWithIndividualIntegrationBorders")]
        public void GetValueWithState_ThreeTimesXSquare_BenchmarkResult(IOneDimNumericalIntegratorAlgorithm numericalIntegrator)
        {
            double lowerBound = 1.0;
            double upperBound = 10.0;

            Assume.That(numericalIntegrator.TrySetBounds(lowerBound, upperBound) == true, String.Format("1-dimensional Integrator {0} does not support individual lower/upper bounds", numericalIntegrator.Factory.Name.String));

            numericalIntegrator.FunctionToIntegrate = x => 3.0 * x * x;

            double expected = upperBound * upperBound * upperBound - lowerBound * lowerBound * lowerBound;
            double actual;
            OneDimNumericalIntegrator.State state = numericalIntegrator.GetValue(out actual);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-4), String.Format("1-dimensional integrator {0}; state: {1}.", numericalIntegrator.Factory.Name, state.ToString()));
        }

        /// <summary>A test function for the calculation of the estimated integration value w.r.t. a specific test case.
        /// </summary>
        /// <param name="numericalIntegrator">The numerical integrator.</param>
        [TestCaseSource("TestCaseDataWithIndividualIntegrationBorders")]
        public void GetValue_ThreeTimesXSquare_BenchmarkResult(IOneDimNumericalIntegratorAlgorithm numericalIntegrator)
        {
            double lowerBound = 1.0;
            double upperBound = 10.0;

            Assume.That(numericalIntegrator.TrySetBounds(lowerBound, upperBound) == true, String.Format("1-dimensional Integrator {0} does not support individual lower/upper bounds", numericalIntegrator.Factory.Name.String));

            numericalIntegrator.FunctionToIntegrate = x => 3.0 * x * x;

            double expected = upperBound * upperBound * upperBound - lowerBound * lowerBound * lowerBound;
            var actual = numericalIntegrator.GetValue();

            Assert.That(actual, Is.EqualTo(expected).Within(1E-4), String.Format("1-dimensional integrator {0}.", numericalIntegrator.Factory.Name));
        }

        /// <summary>A test function for the calculation of the estimated integration value w.r.t. a specific test case.
        /// </summary>
        /// <param name="numericalIntegrator">The numerical integrator.</param>
        [TestCaseSource("TestCaseDataWithIndividualIntegrationBorders")]
        public void GetValueWithState_ExponentialFunction_BenchmarkResult(IOneDimNumericalIntegratorAlgorithm numericalIntegrator)
        {
            double lowerBound = 1.0;
            double upperBound = 10.0;

            Assume.That(numericalIntegrator.TrySetBounds(lowerBound, upperBound) == true, String.Format("1-dimensional Integrator {0} does not support individual lower/upper bounds", numericalIntegrator.Factory.Name.String));

            numericalIntegrator.FunctionToIntegrate = x => Math.Exp(x);

            double expected = Math.Exp(upperBound) - Math.Exp(lowerBound);
            double actual;
            OneDimNumericalIntegrator.State state = numericalIntegrator.GetValue(out actual);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-3).Percent, String.Format("1-dimensional integrator {0}; state: {1}.", numericalIntegrator.Factory.Name, state.ToString()));
        }

        /// <summary>A test function for the calculation of the estimated integration value w.r.t. a specific test case.
        /// </summary>
        /// <param name="numericalIntegrator">The numerical integrator.</param>
        [TestCaseSource("TestCaseDataWithIndividualIntegrationBorders")]
        public void GetValue_ExponentialFunction_BenchmarkResult(IOneDimNumericalIntegratorAlgorithm numericalIntegrator)
        {
            double lowerBound = 1.0;
            double upperBound = 10.0;

            Assume.That(numericalIntegrator.TrySetBounds(lowerBound, upperBound) == true, String.Format("1-dimensional Integrator {0} does not support individual lower/upper bounds", numericalIntegrator.Factory.Name.String));

            numericalIntegrator.FunctionToIntegrate = x => Math.Exp(x);

            double expected = Math.Exp(upperBound) - Math.Exp(lowerBound);
            double actual = numericalIntegrator.GetValue();

            Assert.That(actual, Is.EqualTo(expected).Within(1E-1), String.Format("1-dimensional integrator {0}.", numericalIntegrator.Factory.Name));
        }

        /// <summary>Gets the test case data with individual integration borders.
        /// </summary>
        /// <value>The test case data with individual integration borders.</value>
        public static IEnumerable<TestCaseData> TestCaseDataWithIndividualIntegrationBorders
        {
            // because of some 'const abscissa Integrator' we return the IOneDimNumericalIntegratorAlgorithm object
            get
            {
                foreach (ClosedNewtonCotesFormula.Rule rule in Enum.GetValues(typeof(ClosedNewtonCotesFormula.Rule)))
                {
                    if (rule == ClosedNewtonCotesFormula.Rule.Trapezoid)  // the trapezoid rule performs poor, therefore we use a specific Exit condition to fullfill our tolerances in the unit tests
                    {
                        yield return new TestCaseData(NewtonCotesFormula.Closed.Create(rule, ExitCondition.Create(2000)).Create());
                    }
                    else
                    {
                        yield return new TestCaseData(NewtonCotesFormula.Closed.Create(rule).Create());
                    }
                }

                yield return new TestCaseData(new RombergIntegrator().Create());
                yield return new TestCaseData(new GaussKronrodPatterson87Integrator().Create());
                yield return new TestCaseData(new GaussKronrodPatterson255Integrator().Create());
                yield return new TestCaseData(new AdaptiveGaussLobattoIntegrator().Create());

                foreach (AdaptiveGaussKronrodIntegrator.Approach approach in Enum.GetValues(typeof(AdaptiveGaussKronrodIntegrator.Approach)))
                {
                    yield return new TestCaseData(new AdaptiveGaussKronrodIntegrator(approach).Create());
                }

                yield return new TestCaseData(new GaussKronrodPatterson255ConstAbscissaIntegrator().Create());
                yield return new TestCaseData(new GaussKronrodPatterson87ConstAbscissaIntegrator().Create());
                yield return new TestCaseData(new GaussLegendreIntegrator().Create());
            }
        }
    }
}