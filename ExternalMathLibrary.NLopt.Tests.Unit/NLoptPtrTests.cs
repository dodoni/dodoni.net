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

namespace Dodoni.MathLibrary.Native.NLopt
{
    /// <summary>Serves as class for unit tests for <see cref="NLoptPtr"/>.
    /// </summary>
    public class NLoptPtrTests
    {
        /// <summary>Serves as unit test for <see cref="NLoptPtr"/> with a simple example and PRAXIS algorithm.
        /// </summary>
        [Test]
        public void NLoptPRAXIS_TestCase_AnalyticMinimum()
        {
            NLoptPtr ptr = new NLoptPtr(NLoptAlgorithm.LN_PRAXIS, 2);

            ptr.TrySetAbsoluteFValueTolerance(1E-6);
            ptr.TrySetRelativeFValueTolerance(1E-6);
            ptr.TrySetAbsoluteXTolerance(1E-6);
            ptr.TrySetRelativeXTolerance(1E-6);

            ptr.SetFunction((n, x, gradient, data) => { return x[0] * x[0] + x[1] * x[1] + 1.123; });

            double[] argMin = new double[2] { 1.0, 4.8 };
            double actual;
            var code = ptr.FindMinimum(argMin, out actual);

            double expected = 1.123;

            Assert.That(actual, Is.EqualTo(expected).Within(1E-6));
        }

        /// <summary>Serves as unit test for <see cref="NLoptPtr"/>; see Tutorial of the NLopt documentation.
        /// </summary>
        /// <param name="nloptAlgorithm">The NLopt algorithm, i.e. COBYLA or MMA.</param>
        [TestCase(NLoptAlgorithm.LN_COBYLA)]
        [TestCase(NLoptAlgorithm.LD_MMA)]
        public void NLoptTutorialExample_TestCase_AnalyticSolution(NLoptAlgorithm nloptAlgorithm)
        {
            NLoptPtr ptr = new NLoptPtr(nloptAlgorithm, 2);

            /* add and create boundary and nonlinear constraints: */
            ptr.SetLowerBounds(new[] { Double.NegativeInfinity, 0.0 });
            double a1 = 2.0;
            double b1 = 0.0;

            ptr.AddInequalityConstraint((n, x, grad, data) =>
            {
                if (grad != null)
                {
                    grad[0] = 3.0 * a1 * (a1 * x[0] + b1) * (a1 * x[0] + b1);
                    grad[1] = -1.0;
                }
                return (a1 * x[0] + b1) * (a1 * x[0] + b1) * (a1 * x[0] + b1) - x[1];
            }, 1E-8);


            double a2 = -1;
            double b2 = 1.0;
            var code = ptr.AddInequalityConstraint((n, x, grad, data) =>
            {
                if (grad != null)
                {
                    grad[0] = 3.0 * a2 * (a2 * x[0] + b2) * (a2 * x[0] + b2);
                    grad[1] = -1.0;
                }
                return (a2 * x[0] + b2) * (a2 * x[0] + b2) * (a2 * x[0] + b2) - x[1];
            }, 1E-8);


            ptr.SetFunction((n, x, grad, data) =>
            {
                if (grad != null)
                {
                    grad[0] = 0.0;
                    grad[1] = 0.5 / Math.Sqrt(x[1]);
                }
                return Math.Sqrt(x[1]);
            });

            ptr.TrySetRelativeXTolerance(1E-4);

            var actualArgMin = new[] { 1.234, 5.678 };
            double actualMinimum;
            ptr.FindMinimum(actualArgMin, out actualMinimum);

            double expectedMinimum = Math.Sqrt(8.0 / 27.0);
            double expectedArgMin0 = 1.0 / 3.0;
            double expectedArgMin1 = 8.0 / 27.0;

            Assert.That(actualMinimum, Is.EqualTo(expectedMinimum).Within(1E-3));
            Assert.That(actualArgMin[0], Is.EqualTo(expectedArgMin0).Within(1E-3));
            Assert.That(actualArgMin[1], Is.EqualTo(expectedArgMin1).Within(1E-3));
        }
    }
}