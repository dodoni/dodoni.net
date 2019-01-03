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
using System.Linq;
using System.Collections.Generic;

using NUnit.Framework;

namespace Dodoni.MathLibrary.ProbabilityTheory.Distributions
{
    class NormalDistributionTests
    {
        /// <summary>A test function for the cummulative distribution function, i.e. apply the inverse cummulative distribution function first and then the cummulative distribution function.
        /// </summary>
        /// <param name="probability">The probability where to evaluate the inverse cummulative distribution function.</param>
        /// <param name="mean">The mean.</param>
        /// <param name="sigma">The standard deviation.</param>
        [Test]
        public void GetCdfValue_ResultOfGetInverseCdfValue_InputValue(
            [Values(0.0, 0.005, 0.25, 0.5, 0.75, 0.995, 1.0)]
            double probability,
            [Values(0.0, -1.25, 2.75)]
            double mean,
            [Values(1.0, 0.25, 1.75)]
            double sigma)
        {            
            Assume.That(probability, Is.LessThanOrEqualTo(1.0));
            var normalDistribution = NormalDistribution.Create(mean, sigma);

            double expected = probability;

            double x = normalDistribution.GetInverseCdfValue(probability);
            double actual = normalDistribution.GetCdfValue(x);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-6));
        }
    }
}
