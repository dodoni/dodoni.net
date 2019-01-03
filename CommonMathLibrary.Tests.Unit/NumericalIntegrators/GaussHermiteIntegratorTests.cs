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
    /// <summary>Serves as unit test class for <see cref="GaussHermiteIntegrator"/>.
    /// </summary>
    public class GaussHermiteIntegratorTests
    {
        /// <summary>A test function for the calculation of the estimated integration value w.r.t. a specific test case.
        /// </summary>
        /// <param name="initialOrder">The initial order of the Gauss-Hermite approach, i.e. the order in the first iteration step.</param>
        /// <param name="orderStepSize">The step size of the order, i.e. in each iteration step the order will be increased by the specified number.</param>
        [Test, Combinatorial]
        public void GetValue_One_BenchmarkResult(
            [Values(100, 125)]
            int initialOrder,
            [Values(5, 10, 25)]
            int orderStepSize)
        {
            var gaussHermiteIntegrator = new GaussHermiteIntegrator(initialOrder, orderStepSize);
            var numericalIntegrator = gaussHermiteIntegrator.Create();

            numericalIntegrator.FunctionToIntegrate = x => 1.0;

            double expected = Math.Sqrt(Math.PI);
            double actual = numericalIntegrator.GetValue();

            Assert.That(actual, Is.EqualTo(expected).Within(1E-6), String.Format("1-dimensional integrator {0}.", numericalIntegrator.Factory.Name));
        }

        /// <summary>A test function for the calculation of the estimated integration value w.r.t. a specific test case.
        /// </summary>
        /// <param name="initialOrder">The initial order of the Gauss-Hermite approach, i.e. the order in the first iteration step.</param>
        /// <param name="orderStepSize">The step size of the order, i.e. in each iteration step the order will be increased by the specified number.</param>
        [Test, Combinatorial]
        public void GetValueWithState_One_BenchmarkResult(
            [Values(100, 125)]
            int initialOrder,
            [Values(5, 10, 25)]
            int orderStepSize)
        {
            var gaussHermiteIntegrator = new GaussHermiteIntegrator(initialOrder, orderStepSize);
            var numericalIntegrator = gaussHermiteIntegrator.Create();

            numericalIntegrator.FunctionToIntegrate = x => 1.0;

            double expected = Math.Sqrt(Math.PI);
            double actual;
            OneDimNumericalIntegrator.State state = numericalIntegrator.GetValue(out actual);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-6), String.Format("1-dimensional integrator {0}; state: {1}.", numericalIntegrator.Factory.Name, state.ToString()));
        }
    }
}