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

namespace Dodoni.MathLibrary.Optimizer.MultiDimensional
{
    /// <summary>Serves as interface for multi-dimensional optimization algorithms.
    /// </summary>
    public interface IMultiDimOptimizerAlgorithm : IDisposable
    {
        /// <summary>Gets the factory for further <see cref="IMultiDimOptimizerAlgorithm"/> objects of the same type, i.e. with the same stopping condition etc.
        /// </summary>
        /// <value>The factory for further <see cref="IMultiDimOptimizerAlgorithm"/> objects of the same type.</value>
        MultiDimOptimizer Factory
        {
            get;
        }

        /// <summary>Gets the dimension of the feasible region.
        /// </summary>
        /// <value>The dimension.</value>
        int Dimension
        {
            get;
        }

        /// <summary>Gets or sets the objective function in its <see cref="MultiDimOptimizer.IFunction"/> representation.
        /// </summary>
        /// <value>The objective function.</value>
        MultiDimOptimizer.IFunction Function
        {
            get;
            set;
        }

        /// <summary>Finds the minimum and argmin of <see cref="IMultiDimOptimizerAlgorithm.Function"/>.
        /// </summary>
        /// <param name="x">An array with at least <see cref="IMultiDimOptimizerAlgorithm.Dimension"/> elements which is perhaps taken into account as an initial guess of the algorithm; on exit this argument contains the argmin.</param>
        /// <param name="minimum">The minimum, i.e. the function value with respect to <paramref name="x"/> which represents the argmin (output).</param>
        /// <returns>The state of the algorithm, i.e. an indicating whether <paramref name="x"/> and <paramref name="minimum"/> contain valid data.</returns>
        MultiDimOptimizer.State FindMinimum(double[] x, out double minimum);
    }
}