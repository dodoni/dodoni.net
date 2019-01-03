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
    /// <summary>Serves as unit test class for <see cref="GaussLaguerreConstAbscissaIntegrator"/>.
    /// </summary>
    public class GaussLaguerreConstAbscissaIntegratorTests
    {
        /// <summary>A test function for the calculation of the estimated integration value w.r.t. a specific test case.
        /// </summary>
        /// <param name="alpha">The parameter \alpha of the Guass-Laguerre integrator.</param>
        /// <param name="order">The order of the Gauss-Laguerre integration.</param>
        [Test, Combinatorial]
        public void GetValue_One_BenchmarkResult(
            [Values(5, 10, 22)]
            int alpha,
            [Values(100, 125)]
            int order)
        {
            var gaussLaguerreConstantAbscissasIntegrator = new GaussLaguerreConstAbscissaIntegrator(alpha, order);
            var numericalIntegrator = gaussLaguerreConstantAbscissasIntegrator.Create();

            numericalIntegrator.FunctionToIntegrate = (x, k) => 1.0;

            double expected = GetFaculty(alpha);  // see for example § 21.6.2 "Taschenbuch der Mathematik", Bronstein, Semendjajew, Musiol, Mühlig, 1995
            double actual = numericalIntegrator.GetValue();

            Assert.That(actual, Is.EqualTo(expected).Within(1E-7).Percent, String.Format("1-dimensional integrator {0}.", numericalIntegrator.Factory.Name));
        }

        /// <summary>A test function for the calculation of the estimated integration value w.r.t. a specific test case.
        /// </summary>
        /// <param name="order">The order of the Gauss-Laguerre integration.</param>
        [Test, Combinatorial]
        public void GetValue_ZeroAlphaExpOfMinusX_BenchmarkResult(
            [Values(100, 125)]
            int order)
        {
            var gaussLaguerreConstantAbscissasIntegrator = new GaussLaguerreConstAbscissaIntegrator(order);
            var numericalIntegrator = gaussLaguerreConstantAbscissasIntegrator.Create();

            numericalIntegrator.FunctionToIntegrate = (x, k) => Math.Exp(-x);  // i.e. \int_0^\infty e^{-2*x} = 1/2

            double expected = 0.5;
            double actual = numericalIntegrator.GetValue();

            Assert.That(actual, Is.EqualTo(expected).Within(1E-6), String.Format("1-dimensional integrator {0}.", numericalIntegrator.Factory.Name));
        }

        #region private methods

        /// <summary>Gets the faculty.
        /// </summary>
        /// <param name="n">The argument.</param>
        /// <returns>Returns n!.</returns>
        private static double GetFaculty(int n)
        {
            double value = 1.0;
            while (n > 0)
            {
                value *= n;
                n--;
            }
            return value;
        }
        #endregion
    }
}