﻿/* MIT License
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

namespace Dodoni.MathLibrary.Optimizer.MultiDimensional
{
    public abstract partial class MultiDimOptimizer
    {
        /// <summary>Serves as interface for the objetive function, returning a double-precision floating-point number that has a collection of double-precision floating-point numbers as argument.
        /// </summary>
        /// <remarks>The internal representation of the objective function may depend on the API of the (external) library. Therefore one has to apply <see cref="MultiDimOptimizer.Function"/> as a factory for 
        /// a specific <see cref="IFunction"/> object and the corresponding <see cref="IMultiDimOptimizerAlgorithm"/> object will apply a specific cast.</remarks>
        public interface IFunction : IInfoOutputQueriable
        {
            /// <summary>Gets the <see cref="MultiDimOptimizer.IFunctionFactory"/> instance that has been used as factory for the current object.
            /// </summary>
            /// <value>The <see cref="MultiDimOptimizer.IFunctionFactory"/> instance that has been used as factory for the current object.</value>
            IFunctionFactory Factory
            {
                get;
            }

            /// <summary>Gets the value of the objective function at a specific point.
            /// </summary>
            /// <param name="x">The argument of the function of dimension <see cref="IMultiDimOptimizerAlgorithm.Dimension"/> which should be inside the feasible region.</param>
            /// <returns>The value of the objective function at <paramref name="x"/>.</returns>
            double GetValue(double[] x);
        }
    }
}