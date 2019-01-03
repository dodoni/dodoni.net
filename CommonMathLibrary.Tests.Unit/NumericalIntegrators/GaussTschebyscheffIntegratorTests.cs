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

namespace Dodoni.MathLibrary.NumericalIntegrators
{
    /// <summary>Serves as unit test class for <see cref="GaussTschebyscheffIntegrator"/>.
    /// </summary>
    public class GaussTschebyscheffIntegratorTests
    {
        /// <summary>A test function for the calculation of the estimated integration value w.r.t. a specific test case.
        /// </summary>
        /// <param name="initialOrder">The initial order of the Gauss-Tschebyscheff approach, i.e. the order in the first iteration step.</param>
        /// <param name="orderStepSize">The step size of the order, i.e. in each iteration step the order will be increased by the specified number.</param>
        [Test, Combinatorial]
        public void GetValueWithState_OneOverWeightfunction_BenchmarkResult(
            [Values(10, 25, 50, 100)]
            int initialOrder,
            [Values(5, 10, 25)]
            int orderStepSize)
        {
            var gaussTschebyscheffIntegrator = new GaussTschebyscheffIntegrator(initialOrder, orderStepSize);
            var numericalIntegrator = gaussTschebyscheffIntegrator.Create();

            double lowerBound = -1.0;
            double upperBound = 1.0;

            Assert.That(numericalIntegrator.TrySetBounds(lowerBound, upperBound) == true, String.Format("1-dimensional Integrator {0} does not support individual lower/upper bounds", numericalIntegrator.Factory.Name.String));

            numericalIntegrator.FunctionToIntegrate = x => Math.Sqrt(1.0 - x * x);  // for Tschebyscheff Integrator, the integrator is 1 in this case!

            double expected = upperBound - lowerBound;
            double actual;
            OneDimNumericalIntegrator.State state = numericalIntegrator.GetValue(out actual);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-4), String.Format("1-dimensional integrator {0}; state: {1}.", numericalIntegrator.Factory.Name, state.ToString()));
        }

        /// <summary>A test function for the calculation of the estimated integration value w.r.t. a specific test case.
        /// </summary>
        /// <param name="initialOrder">The initial order of the Gauss-Tschebyscheff approach, i.e. the order in the first iteration step.</param>
        /// <param name="orderStepSize">The step size of the order, i.e. in each iteration step the order will be increased by the specified number.</param>
        /// <param name="lowerBound">The lower integration bound.</param>
        /// <param name="upperBound">The upper integration bound.</param>
        [Test, Combinatorial]
        public void GetValueWithState_OneOverWeightfunction_BenchmarkResult(
            [Values(10, 25, 50, 100)]
            int initialOrder,
            [Values(5, 10, 25)]
            int orderStepSize,
            [Values(-1.0, 2.5)]
            double lowerBound,
            [Values(1.0, 6.4)]
            double upperBound)
        {
            var gaussTschebyscheffIntegrator = new GaussTschebyscheffIntegrator(initialOrder, orderStepSize);
            var numericalIntegrator = gaussTschebyscheffIntegrator.Create();

            Assert.That(numericalIntegrator.TrySetBounds(lowerBound, upperBound) == true, String.Format("1-dimensional Integrator {0} does not support individual lower/upper bounds", numericalIntegrator.Factory.Name.String));

            numericalIntegrator.FunctionToIntegrate = x => 1.0 / numericalIntegrator.WeightFunction.GetValue(x); 

            double expected = upperBound - lowerBound;
            double actual;
            OneDimNumericalIntegrator.State state = numericalIntegrator.GetValue(out actual);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-4), String.Format("1-dimensional integrator {0}; state: {1}.", numericalIntegrator.Factory.Name, state.ToString()));
        }

        /// <summary>A test function for the calculation of the estimated integration value w.r.t. a specific test case.
        /// </summary>
        /// <param name="initialOrder">The initial order of the Gauss-Tschebyscheff approach, i.e. the order in the first iteration step.</param>
        /// <param name="orderStepSize">The step size of the order, i.e. in each iteration step the order will be increased by the specified number.</param>
        [Test, Combinatorial]
        public void GetValue_OneOverWeightfunction_BenchmarkResult(
            [Values(10, 25, 50, 100)]
            int initialOrder,
            [Values(5, 10, 25)]
            int orderStepSize)
        {
            var gaussTschebyscheffIntegrator = new GaussTschebyscheffIntegrator(initialOrder, orderStepSize);
            var numericalIntegrator = gaussTschebyscheffIntegrator.Create();

            double lowerBound = -1.0;
            double upperBound = 1.0;

            Assert.That(numericalIntegrator.TrySetBounds(lowerBound, upperBound) == true, String.Format("1-dimensional Integrator {0} does not support individual lower/upper bounds", numericalIntegrator.Factory.Name.String));

            numericalIntegrator.FunctionToIntegrate = x => Math.Sqrt(1.0 - x * x);  // for Tschebyscheff Integrator, the integrator is 1 in this case!

            double expected = upperBound - lowerBound;
            double actual = numericalIntegrator.GetValue();

            Assert.That(actual, Is.EqualTo(expected).Within(1E-4), String.Format("1-dimensional integrator {0}.", numericalIntegrator.Factory.Name));
        }

        /// <summary>A test function for the calculation of the estimated integration value w.r.t. a specific test case.
        /// </summary>
        /// <param name="initialOrder">The initial order of the Gauss-Tschebyscheff approach, i.e. the order in the first iteration step.</param>
        /// <param name="orderStepSize">The step size of the order, i.e. in each iteration step the order will be increased by the specified number.</param>
        /// <param name="lowerBound">The lower integration bound.</param>
        /// <param name="upperBound">The upper integration bound.</param>
        [Test, Combinatorial]
        public void GetValue_OneOverWeightfunction_BenchmarkResult(
            [Values(10, 25, 50, 100)]
            int initialOrder,
            [Values(5, 10, 25)]
            int orderStepSize,
            [Values(-1.0, 2.5)]
            double lowerBound,
            [Values(1.0, 6.4)]
            double upperBound)
        {
            var gaussTschebyscheffIntegrator = new GaussTschebyscheffIntegrator(initialOrder, orderStepSize);
            var numericalIntegrator = gaussTschebyscheffIntegrator.Create();

            Assert.That(numericalIntegrator.TrySetBounds(lowerBound, upperBound) == true, String.Format("1-dimensional Integrator {0} does not support individual lower/upper bounds", numericalIntegrator.Factory.Name.String));

            numericalIntegrator.FunctionToIntegrate = x => 1.0/numericalIntegrator.WeightFunction.GetValue(x); 

            double expected = upperBound - lowerBound;
            double actual = numericalIntegrator.GetValue();

            Assert.That(actual, Is.EqualTo(expected).Within(1E-4), String.Format("1-dimensional integrator {0}.", numericalIntegrator.Factory.Name));
        }

        /// <summary>A test function for the calculation of the estimated integration value w.r.t. a specific test case.
        /// </summary>
        /// <param name="initialOrder">The initial order of the Gauss-Tschebyscheff approach, i.e. the order in the first iteration step.</param>
        /// <param name="orderStepSize">The step size of the order, i.e. in each iteration step the order will be increased by the specified number.</param>
        [Test, Combinatorial]
        public void GetValueWithState_ThreeTimesXToThePowerOf3_BenchmarkResult(
            [Values(10, 25, 50, 100)]
            int initialOrder,
            [Values(5, 10, 25)]
            int orderStepSize)
        {
            var gaussTschebyscheffIntegrator = new GaussTschebyscheffIntegrator(initialOrder, orderStepSize);
            var numericalIntegrator = gaussTschebyscheffIntegrator.Create();

            double lowerBound = -1.0;
            double upperBound = 1.0;

            Assert.That(numericalIntegrator.TrySetBounds(lowerBound, upperBound) == true, String.Format("1-dimensional Integrator {0} does not support individual lower/upper bounds", numericalIntegrator.Factory.Name.String));

            numericalIntegrator.FunctionToIntegrate = x => 3.0 * x * x * x;

            double a = 1.0;  // see §21.5.25 in "Taschenbuch der Mathematik", Bronstein, Semendjajew, Musiol, Mühlig, 1995
            double expected = 3.0 / 5.0 * Math.Sqrt(Math.Pow(a * a - upperBound * upperBound, 5)) - a * a * Math.Sqrt(Math.Pow(a * a - upperBound * upperBound, 3)) - 3.0 / 5.0 * Math.Sqrt(Math.Pow(a * a - lowerBound * lowerBound, 5)) - a * a * Math.Sqrt(Math.Pow(a * a - lowerBound * lowerBound, 3));

            double actual;
            OneDimNumericalIntegrator.State state = numericalIntegrator.GetValue(out actual);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-6), String.Format("1-dimensional integrator {0}; state: {1}.", numericalIntegrator.Factory.Name, state.ToString()));
        }

        /// <summary>A test function for the calculation of the estimated integration value w.r.t. a specific test case.
        /// </summary>
        /// <param name="initialOrder">The initial order of the Gauss-Tschebyscheff approach, i.e. the order in the first iteration step.</param>
        /// <param name="orderStepSize">The step size of the order, i.e. in each iteration step the order will be increased by the specified number.</param>
        [Test, Combinatorial]
        public void GetValue_ThreeTimesXToThePowerOf3_BenchmarkResult(
            [Values(10, 25, 50, 100)]
            int initialOrder,
            [Values(5, 10, 25)]
            int orderStepSize)
        {
            var gaussTschebyscheffIntegrator = new GaussTschebyscheffIntegrator(initialOrder, orderStepSize);
            var numericalIntegrator = gaussTschebyscheffIntegrator.Create();

            double lowerBound = -1.0;
            double upperBound = 1.0;

            Assert.That(numericalIntegrator.TrySetBounds(lowerBound, upperBound) == true, String.Format("1-dimensional Integrator {0} does not support individual lower/upper bounds", numericalIntegrator.Factory.Name.String));

            numericalIntegrator.FunctionToIntegrate = x => 3.0 * x * x * x;

            double a = 1.0;  // see §21.5.25 in "Taschenbuch der Mathematik", Bronstein, Semendjajew, Musiol, Mühlig, 1995
            double expected = 3.0 / 5.0 * Math.Sqrt(Math.Pow(a * a - upperBound * upperBound, 5)) - a * a * Math.Sqrt(Math.Pow(a * a - upperBound * upperBound, 3)) - 3.0 / 5.0 * Math.Sqrt(Math.Pow(a * a - lowerBound * lowerBound, 5)) - a * a * Math.Sqrt(Math.Pow(a * a - lowerBound * lowerBound, 3));

            double actual = numericalIntegrator.GetValue();

            Assert.That(actual, Is.EqualTo(expected).Within(1E-6), String.Format("1-dimensional integrator {0}.", numericalIntegrator.Factory.Name));
        }
    }
}