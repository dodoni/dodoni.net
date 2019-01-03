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
    /// <summary>Serves as interface for 1-dimensional root finder algorithms.
    /// </summary>
    public interface IOneDimEquationSolverAlgorithm : IDisposable
    {
        /// <summary>Gets the factory for further <see cref="IOneDimEquationSolverAlgorithm"/> objects of the same type, i.e. with the same stopping condition etc.
        /// </summary>
        /// <value>The factory for further <see cref="IOneDimEquationSolverAlgorithm"/> objects of the same type.</value>
        OneDimEquationSolver Factory
        {
            get;
        }

        /// <summary>Gets or sets the objective function in its <see cref="OneDimEquationSolver.IFunction"/> representation.
        /// </summary>
        /// <value>The objective function.</value>
        OneDimEquationSolver.IFunction Function
        {
            get;
            set;
        }

        /// <summary>Finds a specific root of the specified <see cref="IOneDimEquationSolverAlgorithm.Function"/>.
        /// </summary>
        /// <param name="x">An initial guess of the algorithm (if applicable).</param>
        /// <param name="root">The estimated root of the objective function (output).</param>
        /// <returns>The state of the algorithm, i.e. an indicating whether <paramref name="root"/> contains valid data.</returns>
        OneDimEquationSolver.State FindRoot(double x, out double root);
    }
}