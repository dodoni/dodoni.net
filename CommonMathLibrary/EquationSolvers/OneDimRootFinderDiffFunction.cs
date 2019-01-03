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

namespace Dodoni.MathLibrary.EquationSolvers
{
    /// <summary>Represents a simple encapsulated differentiable objective function for 1-dimensional root finder algorithms.
    /// </summary>
    public class OneDimRootFinderDifferentiableFunction : OneDimRootFinderFunction
    {
        #region private members

        /// <summary>The objective function.
        /// </summary>
        private OneDimEquationSolver.DifferentiableObjectiveFunction m_DifferentiableObjectiveFunction;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="OneDimRootFinderDifferentiableFunction"/> class.
        /// </summary>
        /// <param name="functionFactory">The objective function descriptor, i.e. the <see cref="OneDimRootFinderFunctionFactory"/> object that served as factory for the current object.</param>
        /// <param name="objectiveFunction">The objective function.</param>
        internal OneDimRootFinderDifferentiableFunction(OneDimRootFinderFunctionFactory functionFactory, OneDimEquationSolver.DifferentiableObjectiveFunction objectiveFunction)
            : base(functionFactory, x => { double dummy; return objectiveFunction(x, out dummy); })
        {
            m_DifferentiableObjectiveFunction = objectiveFunction ?? throw new ArgumentNullException(nameof(objectiveFunction));
        }
        #endregion

        #region public methods

        /// <summary>Gets the value of the objective function at a specific point.
        /// </summary>
        /// <param name="x">The argument of the objective function which should be inside the feasible region.</param>
        /// <param name="derivative">The derivative of the objective function at <paramref name="x"/>.</param>
        /// <returns>The value of the objective function at <paramref name="x"/>.</returns>
        public double GetValue(double x, out double derivative)
        {
            return m_DifferentiableObjectiveFunction(x, out derivative);
        }
        #endregion
    }
}