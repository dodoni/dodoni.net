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

using Dodoni.BasicComponents;
using Dodoni.MathLibrary.Miscellaneous;

namespace Dodoni.MathLibrary.Optimizer.OneDimensional
{
    /// <summary>Serves as unit test class for <see cref="BrentOptimizer"/> objects.
    /// </summary>
    [TestFixture]
    public class BrentOptimizerTests
    {
        /// <summary>Serves as unit test for <see cref="BrentOptimizer"/> objects.
        /// </summary>
        /// <param name="lowerBound">The lower bound of the interval constraint; should be smaller than 1.0.</param>
        /// <param name="upperBound">The upper bound of the interval constraint; should be larger than 1.0.</param>
        /// <param name="initialGuess">The initial guess; should be a number inside the constraint specified by <paramref name="lowerBound"/> and <paramref name="upperBound"/>.</param>
        [TestCase(0.5, 2.5, 1.5)]
        [TestCase(-1.25, 5.5, 4.75)]
        public void FindMinimum_XMinus1SquareTestFunction_AnalyticResult(double lowerBound, double upperBound, double initialGuess)
        {
            Assume.That((lowerBound < upperBound) && (lowerBound < 1.0) && (1.0 < upperBound), "Wrong lower bounds");
            Assume.That((initialGuess < upperBound) && (lowerBound < initialGuess), "Invalid initial guess");

            var optimizer = new BrentOptimizer();

            var constraint = optimizer.Constraint.Create(Interval.Create(lowerBound, upperBound));
            var optimizerAlgorithm = optimizer.Create(constraint);

            optimizerAlgorithm.Function = optimizer.Function.Create(x => (x - 1.0) * (x - 1.0));

            var state = optimizerAlgorithm.FindMinimum(initialGuess, out double actualArgMin, out double actualMinimum);

            var expectedArgMin = 1.0;
            var expectedMinimum = 0.0;
            Assert.That(actualArgMin, Is.EqualTo(expectedArgMin).Within(1E-5));
            Assert.That(actualMinimum, Is.EqualTo(expectedMinimum).Within(1E-5));
        }

        /// <summary>Serves as unit test for <see cref="BrentOptimizer"/> objects; with respect to f(x) =  2 * (x - 5) * (x - 5) * (x - 5) * (x - 4) + 4.0.
        /// </summary>
        /// <param name="lowerBound">The lower bound of the interval constraint.</param>
        /// <param name="upperBound">The upper bound of the interval constraint.</param>
        /// <param name="initialGuess">The initial guess; should be a number inside the constraint specified by <paramref name="lowerBound"/> and <paramref name="upperBound"/>.</param>
        [TestCase(0.5, 4.5, 1.5)]
        [TestCase(-1.25, 5.5, -0.15)]
        public void FindMinimum_CubicTestfunction_AnalyticResult(double lowerBound, double upperBound, double initialGuess)
        {
            var optimizer = new BrentOptimizer();

            var constraint = optimizer.Constraint.Create(Interval.Create(lowerBound, upperBound));
            var optimizerAlgorithm = optimizer.Create(constraint);

            optimizerAlgorithm.Function = optimizer.Function.Create(x => 2 * (x - 5) * (x - 5) * (x - 5) * (x - 4) + 4.0);

            var state = optimizerAlgorithm.FindMinimum(initialGuess, out double actualArgMin, out double actualMinimum);

            var expectedArgMin = 4.25;
            var expectedMinimum = 4.0 - 27.0 / 128.0;
            Assert.That(actualMinimum, Is.EqualTo(expectedMinimum).Within(1E-5), String.Format("Expected argMin: {0}, Actual argMin: {1}", expectedArgMin, actualArgMin));
            Assert.That(actualArgMin, Is.EqualTo(expectedArgMin).Within(1E-5));
        }
    }
}