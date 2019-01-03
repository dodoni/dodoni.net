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
    /// <summary>Serves as factory for <see cref="MultiDimOptimizer.IFunction"/> objects that represents a . 
    /// </summary>
    public class MultivariateFunctionFactory : MultivariateOptimizer.IFunctionFactory
    {
        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="MultivariateFunctionFactory"/> class.
        /// </summary>
        public MultivariateFunctionFactory()
        {
        }
        #endregion

        #region public properties

        #region IInfoOutputQueriable Members

        /// <summary>Gets the info-output level of detail.
        /// </summary>
        /// <value>The info-output level of detail.</value>
        public InfoOutputDetailLevel InfoOutputDetailLevel
        {
            get { return InfoOutputDetailLevel.Full; }
        }
        #endregion

        #endregion

        #region public methods

        #region MultivariateOptimizer.IFunctionFactory Members

        /// <summary>Creates a specific <see cref="MultiDimOptimizer.IFunction"/> object.
        /// </summary>
        /// <param name="dimension">The dimension of the feasible region.</param>
        /// <param name="codomainDimension">The dimension of the codomain, i.e. the objective function is taking values in a subset of R^k where k is the dimension of the codomain.</param>
        /// <param name="objectiveFunction">The objective function, where the first argument is the point where to evaluate and the second argument is the value of the function.</param>
        /// <returns>A specific <see cref="MultiDimOptimizer.IFunction"/> object with respect to the specified optimization algorithm.</returns>
        public MultiDimOptimizer.IFunction Create(int dimension, int codomainDimension, Func<double[], double[]> objectiveFunction)
        {
            throw new InvalidOperationException();  // todo: add method without analytic derivative
        }

        /// <summary>Creates a specific <see cref="MultiDimOptimizer.IFunction"/> object.
        /// </summary>
        /// <param name="dimension">The dimension of the feasible region.</param>
        /// <param name="codomainDimension">The dimension of the codomain, i.e. the objective function is taking values in a subset of R^k where k is the dimension of the codomain.</param>
        /// <param name="objectiveFunction">The objective function, where the first argument is the point where to evalute, the second argument contains the Jacobian matrix and 
        /// the last argument is the value of the function at the first argument.</param>
        /// <returns>A specific <see cref="MultiDimOptimizer.IFunction"/> object with respect to the specified optimization algorithm.</returns>
        /// <exception cref="InvalidOperationException">Thrown because the matrix representation of the objective function is required.</exception>
        public MultiDimOptimizer.IFunction Create(int dimension, int codomainDimension, Action<double[], double[], double[]> objectiveFunction)
        {
            return new MultivariateFunction(this, dimension, codomainDimension, objectiveFunction);
        }
        #endregion

        #region IInfoOutputQueriable Members

        /// <summary>Sets the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel"/> property.
        /// </summary>
        /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
        /// <returns>A value indicating whether the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel"/> has been set to <paramref name="infoOutputDetailLevel"/>.</returns>
        public bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
        {
            return (infoOutputDetailLevel == InfoOutputDetailLevel.Full);
        }

        /// <summary>Gets informations of the current object as a specific <see cref="InfoOutput"/> instance.
        /// </summary>
        /// <param name="infoOutput">The <see cref="InfoOutput"/> object which is to be filled with informations concering the current instance.</param>
        /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
        public void FillInfoOutput(InfoOutput infoOutput, string categoryName = InfoOutput.GeneralCategoryName)
        {
        }
        #endregion

        #endregion
    }
}