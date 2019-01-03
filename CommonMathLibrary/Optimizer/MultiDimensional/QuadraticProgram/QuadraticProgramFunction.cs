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
using Dodoni.MathLibrary.Basics;
using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.Optimizer.MultiDimensional
{
    /// <summary>Represents the objective function of a quadratic programm, i.e. f(x) = 1/2 * x^t * A * x + b^t * x, where A, b is a suitable matrix, vector respectively.
    /// </summary>
    public class QuadraticProgramFunction : MultiDimOptimizer.IFunction
    {
        #region private members

        /// <summary>The <see cref="QuadraticProgramFunctionFactory"/> object that served as factory for the current object.
        /// </summary>
        private QuadraticProgramFunctionFactory m_Factory;

        /// <summary>The quadratic program matrix A.
        /// </summary>
        public readonly DenseMatrix A;

        /// <summary>The quadratic program vector b.
        /// </summary>
        public readonly double[] b;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="QuadraticProgramFunction"/> class.
        /// </summary>
        /// <param name="functionDescriptor">The function descriptor, i.e. the <see cref="QuadraticProgramFunctionFactory"/> object that served as factory for the current object.</param>
        /// <param name="A">The quadratic program matrix A.</param>
        /// <param name="b">The quadratic program vector b.</param>
        public QuadraticProgramFunction(QuadraticProgramFunctionFactory functionDescriptor, DenseMatrix A, double[] b)
        {
            m_Factory = functionDescriptor;
            if (A == null)
            {
                throw new ArgumentNullException(nameof(A));
            }
            if (A.RowCount != A.ColumnCount)
            {
                throw new ArgumentException(String.Format(ExceptionMessages.ArgumentIsNoSquaredMatrix, "A"), nameof(A));
            }
            this.A = A;

            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }
            if (b.Length < A.RowCount)
            {
                throw new ArgumentException(String.Format(ExceptionMessages.ArgumentHasWrongDimension, "b"), nameof(b));
            }
            this.b = b;
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

        #endregion

        #region public methods

        #region IFunction Members

        /// <summary>Gets the value of the objective function at a specific point.
        /// </summary>
        /// <param name="x">The argument of the function of dimension <see cref="IMultiDimOptimizerAlgorithm.Dimension"/> which should be inside the feasible region.</param>
        /// <returns>The value of the objective function at x.</returns>
        public double GetValue(double[] x)
        {
            return 0.5 * DenseMatrix.GetBilinearForm(A, x) + BLAS.Level1.ddot(A.RowCount, x, b);
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