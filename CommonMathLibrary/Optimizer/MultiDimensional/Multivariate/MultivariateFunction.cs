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
using Dodoni.MathLibrary.Basics;

namespace Dodoni.MathLibrary.Optimizer.MultiDimensional
{
    /// <summary>Represents the objective function with respect to vector-valued functions f_1(x)^2 + ... + f_m(x)^2.
    /// </summary>
    public class MultivariateFunction : MultiDimOptimizer.IFunction
    {
        #region private members

        /// <summary>The <see cref="MultivariateFunctionFactory"/> object that served as factory for the current object.
        /// </summary>
        private MultivariateFunctionFactory m_Factory;

        /// <summary>The objective function, where the first argument is the point where to evalute, the second argument contains the Jacobian matrix and 
        /// the last argument is the value of the function at the first argument.
        /// </summary>
        private Action<double[], double[], double[]> m_ObjectiveFunction;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="MultivariateFunction"/> class.
        /// </summary>
        /// <param name="functionDescriptor">The function descriptor, i.e. the <see cref="QuadraticProgramFunctionFactory"/> object that served as factory for the current object.</param>
        /// <param name="dimension">The dimension of the feasible region.</param>
        /// <param name="codomainDimension">The dimension of the codomain, i.e. the objective function is taking values in a subset of R^k where k is the dimension of the codomain.</param>
        /// <param name="objectiveFunction">The objective function, where the first argument is the point where to evalute, the second argument contains the Jacobian matrix and 
        /// the last argument is the value of the function at the first argument.</param>
        public MultivariateFunction(MultivariateFunctionFactory functionDescriptor, int dimension, int codomainDimension, Action<double[], double[], double[]> objectiveFunction)
        {
            m_Factory = functionDescriptor;
            Dimension = dimension;
            CodomainDimension = codomainDimension;
            m_ObjectiveFunction = objectiveFunction;
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

        #region IFunction Members

        /// <summary>Gets the <see cref="MultiDimOptimizer.IFunctionFactory"/> instance that has been used as factory for the current object.
        /// </summary>
        /// <value>The <see cref="MultiDimOptimizer.IFunctionFactory"/> instance that has been used as factory for the current object.</value>
        public MultiDimOptimizer.IFunctionFactory Factory
        {
            get { return m_Factory; }
        }
        #endregion

        /// <summary>Gets the dimension of the feasible region.
        /// </summary>
        /// <value>The dimension of the feasible region.</value>
        public int Dimension
        {
            get;
            private set;
        }

        /// <summary>Gets the codomain dimension, i.e. the objective function is taking values in a subset of R^k where k is the dimension of the codomain.
        /// </summary>
        /// <value>The codomain dimension.</value>
        public int CodomainDimension
        {
            get;
            private set;
        }
        #endregion

        #region public methods

        #region IFunction Members

        /// <summary>Gets the value of the objective function at a specific point.
        /// </summary>
        /// <param name="x">The argument of the function of dimension <see cref="IMultiDimOptimizerAlgorithm.Dimension"/> which should be inside the feasible region.</param>
        /// <returns>The value of the objective function at x.</returns>
        public double GetValue(double[] x)
        {
            var y = new double[CodomainDimension]; // todo: re-use array!

            m_ObjectiveFunction(x, null, y);

            return BLAS.Level1.dnrm2sq(CodomainDimension, y);
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

        /// <summary>Gets the value of the objective function at a specific point.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <param name="jacobiMatrix">The Jacobi matrix at <paramref name="x"/>.</param>
        /// <param name="value">The value of the objective function at <paramref name="x"/>.</param>
        public void GetValue(double[] x, double[] jacobiMatrix, double[] value)
        {
            m_ObjectiveFunction(x, jacobiMatrix, value);
        }
        #endregion
    }
}