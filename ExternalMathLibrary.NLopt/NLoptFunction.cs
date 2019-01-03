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
using Dodoni.MathLibrary.Optimizer.MultiDimensional;

namespace Dodoni.MathLibrary.Native.NLopt
{
    /// <summary>Represents the objective function of a specific NLopt algorithm.
    /// </summary>
    public class NLoptFunction : MultiDimOptimizer.IFunction
    {
        #region private members

        /// <summary>Represents the factory of the current object in its <see cref="NLoptFunctionFactory"/> representation.
        /// </summary>
        private NLoptFunctionFactory m_NLoptFunctionDescriptor;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="NLoptFunction" /> class.
        /// </summary>
        /// <param name="nloptFunctionDescriptor">The <see cref="NLoptFunctionFactory"/> object that has been used as factory of the current instance.</param>
        /// <param name="dimension">The dimension of the feasible region.</param>
        /// <param name="nloptObjectiveFunction">The objective function in its <see cref="NLoptObjectiveFunction"/> representation.</param>
        internal NLoptFunction(NLoptFunctionFactory nloptFunctionDescriptor, int dimension, NLoptObjectiveFunction nloptObjectiveFunction)
        {
            m_NLoptFunctionDescriptor = nloptFunctionDescriptor;
            Dimension = dimension;
            NLoptObjectiveFunction = nloptObjectiveFunction;
        }
        #endregion

        #region public properties

        #region IFunction Members

        /// <summary>Gets the <see cref="MultiDimOptimizer.IFunctionFactory" /> instance that has been used as factory for the current object.
        /// </summary>
        /// <value>The <see cref="MultiDimOptimizer.IFunctionFactory" /> instance that has been used as factory for the current object.</value>
        MultiDimOptimizer.IFunctionFactory MultiDimOptimizer.IFunction.Factory
        {
            get { return m_NLoptFunctionDescriptor; }
        }
        #endregion

        #region IInfoOutputQueriable Members

        /// <summary>Gets the info-output level of detail.
        /// </summary>
        /// <value>The info-output level of detail.</value>
        public InfoOutputDetailLevel InfoOutputDetailLevel
        {
            get { return InfoOutputDetailLevel.Full; }
        }
        #endregion

        /// <summary>Gets the <see cref="NLoptFunctionFactory" /> instance that has been used as factory for the current object.
        /// </summary>
        /// <value>The <see cref="NLoptFunctionFactory" /> instance that has been used as factory for the current object.</value>
        public NLoptFunctionFactory Factory
        {
            get { return m_NLoptFunctionDescriptor; }
        }

        /// <summary>Gets the dimension of the feasible region.
        /// </summary>
        /// <value>The dimension of the feasible region.</value>
        public int Dimension
        {
            get;
            private set;
        }
        #endregion

        #region public methods

        #region IFunction Members

        /// <summary>Gets the value of the function to optmize at a specific point.
        /// </summary>
        /// <param name="x">The argument of the function of dimension <see cref="IMultiDimOptimizerAlgorithm.Dimension" /> which should be inside the feasible region.</param>
        /// <returns>The value of the function to optimize at <paramref name="x" />.</returns>
        public double GetValue(double[] x)
        {
            return NLoptObjectiveFunction(Dimension, x, grad: null, data: IntPtr.Zero);
        }
        #endregion

        #region IInfoOutputQueriable Members

        /// <summary>Sets the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> property.
        /// </summary>
        /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
        /// <returns>A value indicating whether the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> has been set to <paramref name="infoOutputDetailLevel" />.</returns>
        public bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
        {
            return infoOutputDetailLevel == InfoOutputDetailLevel.Full;
        }

        /// <summary>Gets informations of the current object as a specific <see cref="InfoOutput" /> instance.
        /// </summary>
        /// <param name="infoOutput">The <see cref="InfoOutput" /> object which is to be filled with informations concering the current instance.</param>
        /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
        public void FillInfoOutput(InfoOutput infoOutput, string categoryName = InfoOutput.GeneralCategoryName)
        {
            var infoOutputPackage = infoOutput.AcquirePackage(categoryName);
            infoOutputPackage.Add("Dimension", Dimension);
        }
        #endregion

        /// <summary>Gets the objective function in its <see cref="Dodoni.MathLibrary.Native.NLopt.NLoptObjectiveFunction"/> representation.
        /// </summary>
        /// <value>The objective function in its <see cref="Dodoni.MathLibrary.Native.NLopt.NLoptObjectiveFunction"/> representation.</value>
        public NLoptObjectiveFunction NLoptObjectiveFunction
        {
            get;
            private set;
        }
        #endregion
    }
}