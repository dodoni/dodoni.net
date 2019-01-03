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

using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.EquationSolvers
{
    public abstract partial class OneDimEquationSolver
    {
        /// <summary>Represents a differential objective function.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <param name="derivative">The derivative of the objective function at <paramref name="x"/>.</param>
        /// <returns>The value of the objective function at <paramref name="x"/>.</returns>
        public delegate double DifferentiableObjectiveFunction(double x, out double derivative);

        /// <summary>Represents the requirements of the derivative of the objective function for a specific 1-dimensional root finder algorithm.
        /// </summary>
        public enum ObjectiveFunctionDerivativeRequirement
        {
            /// <summary>A derivative-free algorithm, i.e. the derivative of the objective function is not required and will be ignored.
            /// </summary>
            None,

            /// <summary>The optimization algorithm requires the (first) derivative of the objective function.
            /// </summary>
            Required,
        }

        /// <summary>Serves as interface for a factory of objective functions in its <see cref="IFunction"/> representation.
        /// </summary>
        public interface IFunctionFactory : IInfoOutputQueriable
        {
            /// <summary>Gets a value indicating whether the derivative of the objective function is required in the root finder algorithm.
            /// </summary>
            /// <value>The derivative requirement.</value>
            ObjectiveFunctionDerivativeRequirement DerivativeRequirement
            {
                get;
            }

            /// <summary>Creates a specific <see cref="OneDimEquationSolver.IFunction"/> object.
            /// </summary>
            /// <param name="objectiveFunction">The objective function.</param>
            /// <returns>A specific <see cref="OneDimEquationSolver.IFunction"/> object with respect to the specified root finder algorithm.</returns>
            /// <exception cref="InvalidOperationException">Thrown, if <see cref="OneDimEquationSolver.IFunctionFactory.DerivativeRequirement"/> indicates that the derivative is required.</exception>
            IFunction Create(Func<double, double> objectiveFunction);

            /// <summary>Creates a specific <see cref="OneDimEquationSolver.IFunction"/> object.
            /// </summary>
            /// <param name="objectiveFunction">The differentiable objective function.</param>
            /// <returns>A specific <see cref="OneDimEquationSolver.IFunction"/> object with respect to the specified optimization algorithm.</returns>
            IFunction Create(DifferentiableObjectiveFunction objectiveFunction);
        }
    }
}