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
using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.ProbabilityTheory.Distributions.Empirical
{
    /// <summary>Serves as density estimator algorithm.
    /// </summary>
    public interface IDensityEstimatorAlgorithm : IInfoOutputQueriable
    {
        /// <summary>Gets the <see cref="DensityEstimator"/> object that serves as factory of the current object.
        /// </summary>
        /// <value>The <see cref="DensityEstimator"/> object that serves as factory of the current object.</value>
        DensityEstimator Factory
        {
            get;
        }

        /// <summary>Gets the estimated density at a specific point.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <returns>The estimated density at <paramref name="x"/>.</returns>
        double GetValue(double x);
    }
}