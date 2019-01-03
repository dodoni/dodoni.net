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

using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.Optimizer.MultiDimensional
{
    public abstract partial class OrdinaryMultiDimOptimizer
    {
        /// <summary>Represents the requirements of a gradient for a specific multi-dimensional optimization algorithm.
        /// </summary>
        public enum GradientMethod
        {
            /// <summary>No gradient is used for the optimization algorithm.
            /// </summary>
            None,

            /// <summary>The gradient is required for the optimization algorithm.
            /// </summary>
            Required,
        }

        /// <summary>Serves as interface for a factory of objective functions in its <see cref="MultiDimOptimizer.IFunction"/> representation.
        /// </summary>
        /// <remarks>The dimension of the argument is provided by the corresponding <see cref="IMultiDimOptimizerAlgorithm"/> object, i.e. via <see cref="IMultiDimOptimizerAlgorithm.Dimension"/>.</remarks>
        public new interface IFunctionFactory : MultiDimOptimizer.IFunctionFactory
        {
            /// <summary>Gets a value indicating whether the gradient is required in the optimization algorithm.
            /// </summary>
            /// <value>The gradient requirement.</value>
            GradientMethod GradientRequirement
            {
                get;
            }

            /// <summary>Gets a lower bound of all function values if the optimization algorithm assumed function values greater than a specific number (for example positive values only); otherwise <see cref="System.Double.NaN"/>.
            /// </summary>
            /// <value>The lower bound of all function values if the optimization algorithm assumed function values greater than a specific number (for example positive values only); otherwise <see cref="System.Double.NaN"/>.</value>
            double LowerBound
            {
                get;
            }

            /// <summary>Creates a specific <see cref="MultiDimOptimizer.IFunction"/> object.
            /// </summary>
            /// <param name="dimension">The dimension of the feasible region.</param>
            /// <param name="objectiveFunction">The objective function.</param>
            /// <returns>A specific <see cref="MultiDimOptimizer.IFunction"/> object with respect to the specified optimization algorithm.</returns>
            /// <exception cref="InvalidOperationException">Thrown, if <see cref="OrdinaryMultiDimOptimizer.IFunctionFactory.GradientRequirement"/> indicates that a gradient is required.</exception>
            IFunction Create(int dimension, Func<double[], double> objectiveFunction);

            /// <summary>Creates a specific <see cref="MultiDimOptimizer.IFunction"/> object.
            /// </summary>
            /// <param name="dimension">The dimension of the feasible region.</param>
            /// <param name="objectiveFunction">The objective function, where the first argument is the point where to evalute and the second argument contains the gradient, 
            /// i.e. the partial derivatives of the function at the first argument; the return value is the value of the function at the first argument.</param>
            /// <returns>A specific <see cref="MultiDimOptimizer.IFunction"/> object with respect to the specified optimization algorithm.</returns>
            IFunction Create(int dimension, Func<double[], double[], double> objectiveFunction);
        }
    }
}