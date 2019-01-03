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

namespace Dodoni.MathLibrary.Optimizer.MultiDimensional
{
    /// <summary>Serves as abstract basis for multi-dimensional optimization problems with respect to quadratic functions, i.e. 1/2 * x^t * A * x + b^t * x. 
    /// </summary>
    public abstract partial class QuadraticProgram : MultiDimOptimizer
    {
        /// <summary>Initializes a new instance of the <see cref="QuadraticProgram"/> class.
        /// </summary>
        /// <param name="infoOutputDetailLevel">The info-output level of detail in its <see cref="Dodoni.BasicComponents.Containers.InfoOutputDetailLevel"/> representation.</param>
        protected QuadraticProgram(InfoOutputDetailLevel infoOutputDetailLevel = InfoOutputDetailLevel.Full)
            : base(infoOutputDetailLevel)
        {
        }

        /// <summary>Gets a factory for <see cref="MultiDimOptimizer.IFunction"/> objects that encapsulate the objective function.
        /// </summary>
        /// <value>A factory for <see cref="MultiDimOptimizer.IFunction"/> objects that encapsulate the objective function.</value>
        public new IFunctionFactory Function
        {
            get { return GetQuadraticFunctionDescriptor(); }
        }

        /// <summary>Gets a factory for <see cref="MultiDimOptimizer.IFunction"/> objects that encapsulate the function to optimize.
        /// </summary>
        /// <returns>A factory for <see cref="MultiDimOptimizer.IFunction"/> objects that encapsulate the function to optimize.</returns>
        protected abstract QuadraticProgram.IFunctionFactory GetQuadraticFunctionDescriptor();
    }
}