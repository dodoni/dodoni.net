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

namespace Dodoni.MathLibrary.GridPointCurves
{
    /// <summary>Represents a family of real-valued monomial polynomial functions which represents basis functions for a least squares regression.
    /// </summary>
    internal class MonomialBasisFunctions : ILeastSquaresRegressionBasisFunctions
    {
        #region public static (readonly) members

        /// <summary>The language independent name of the basis functions.
        /// </summary>
        public static readonly IdentifierString Name = new IdentifierString("Polynomial Basis functions");
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="MonomialBasisFunctions"/> class.
        /// </summary>
        internal MonomialBasisFunctions()
        {
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the basis functions.
        /// </summary>
        /// <value>The language independent name of the basis functions.</value>
        IdentifierString IIdentifierNameable.Name
        {
            get { return Name; }
        }

        /// <summary>Gets the long name of the basis functions.
        /// </summary>
        /// <value>The language dependent long name of the current instance.</value>
        IdentifierString IIdentifierNameable.LongName
        {
            get { return Name; }
        }
        #endregion

        #region ILeastSquaresRegressionBasisFunctions

        /// <summary>Gets the maximal order allowed for the current instance.
        /// </summary>
        /// <value>The maximal order, i.e. <see cref="Int32.MaxValue"/>.</value>
        public int MaximalOrder
        {
            get { return Int32.MaxValue; }
        }
        #endregion

        #endregion

        #region public methods

        #region ILeastSquaresRegressionBasisFunctions Members

        /// <summary>Gets the value of a specific base function, i.e. 'x^n'.
        /// </summary>
        /// <param name="order">The null-based index of the base function, i.e. the order.</param>
        /// <param name="x">The point to evaluate the specific base function.</param>
        /// <returns>The value of the base function given with respect to <paramref name="order"/> evaluated at <paramref name="x"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="order"/> is greater than <see cref="MaximalOrder"/> or less than <c>0</c>.</exception>
        public double GetBaseFunctionValue(int order, double x)
        {
            if (order < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(order), String.Format(ExceptionMessages.ArgumentOutOfRangeGreaterEqual, "Order", 0));
            }
            return DoMath.Pow(x, order);
        }

        /// <summary>Gets the weighted sum, i.e. '\sum_{j=0}^d a_j * \phi_j(<paramref name="x"/>)', where (a_j) are
        /// the <paramref name="weights"/> and 'd' represents the <paramref name="order"/>.
        /// </summary>
        /// <param name="x">The point to evaluate the weighted sum of basis functions.</param>
        /// <param name="weights">The weights, i.e. an array with at least <paramref name="order"/>+1 elements.</param>
        /// <param name="order">The order.</param>
        /// <returns>The value of the weighted sum of the basis functions up to the given <paramref name="order"/>, evaluated at <paramref name="x"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="order"/> is greater than <see cref="MaximalOrder"/> or less than <c>0</c>.</exception>
        public double GetValue(double x, double[] weights, int order)
        {
            // Here we use the horner scheme for evaluating the polynomial: 
            double value = weights[order];

            for (int j = order - 1; j >= 0; j--)
            {
                value = weights[j] + x * value;
            }
            return value;
        }

        /// <summary>Evaluate the derivative of the weighted sum, i.e. '\sum_{j=0}^d a_j * \phi_j(<paramref name="x"/>)', where (a_j) are
        /// the <paramref name="weights"/> and 'd' represents the <paramref name="order"/>, at a specific point.
        /// </summary>
        /// <param name="x">The point to evaluate the derivative of the weighted sum of basis functions.</param>
        /// <param name="weights">The weights, i.e. an array with at least <paramref name="order"/>+1 elements.</param>
        /// <param name="order">The order.</param>
        /// <returns>The value of the derivative at the <paramref name="x"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="order"/> is greater than <see cref="MaximalOrder"/> or less than <c>0</c>.</exception>
        public double GetDerivative(double x, double[] weights, int order)
        {
            double value = weights[order] * order;
            for (int j = order - 1; j >= 1; j--)
            {
                value = weights[j] * j + value * x;
            }
            return value;
        }

        /// <summary>Gets the value of the integral \int_a^b f(x) dx, where f is the weighted sum '\sum_{j=0}^d a_j * \phi_j',
        /// where (a_j) are the <paramref name="weights"/> and 'd' represents the <paramref name="order"/>.
        /// </summary>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="upperBound">The upper bound which is greater than <paramref name="lowerBound"/>.</param>
        /// <param name="weights">The weights, i.e. an array with at least <paramref name="order"/>+1 elements.</param>
        /// <param name="order">The order.</param>
        /// <returns>
        /// The value of \int_a^b f(x) dx, taking into account the explicit representation of the
        /// basis functions.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="order"/> is greater than
        /// <see cref="MaximalOrder"/> or less than <c>0</c>.</exception>
        public double GetIntegral(double lowerBound, double upperBound, double[] weights, int order)
        {
            double upperValue = weights[order] / (order + 1.0);
            double lowerValue = upperValue;

            for (int j = order - 1; j >= 0; j--)
            {
                double tempCoefficient = weights[j] / (j + 1.0);

                upperValue = tempCoefficient + upperBound * upperValue;
                lowerValue = tempCoefficient + lowerBound * lowerValue;
            }
            return upperBound * upperValue - lowerBound * lowerValue;
        }

        /// <summary>Gets the design matrix of the linear least square problem, i.e.
        /// <para>
        /// A =(a_{i,j}), where a_{i,j} = \phi_j(x_i)
        /// </para>
        /// with i=0,1,...,<paramref name="gridPointCount"/>-1 and j=0,...,<paramref name="order"/>.
        /// </summary>
        /// <param name="gridPointDoubleLabels">The labels of the grid points given in its <see cref="System.Double"/> representation.</param>
        /// <param name="gridPointCount">The number of grid points taken into account, moreover the number of rows of the output matrix.</param>
        /// <param name="order">The order, i.e. the number of columns of the output matrix.</param>
        /// <returns>The design matrix with respect to the specified arguments.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="order"/> is greater than
        /// <see cref="MaximalOrder"/> or less than <c>0</c>.</exception>
        public DenseMatrix GetDesignMatrix(double[] gridPointDoubleLabels, int gridPointCount, int order)
        {
            int length = gridPointCount * (order + 1);
            double[] matrix = new double[length];

            int rowIndex = 0;

            for (int j = 0; j < gridPointCount; j++)
            {
                double abscissa = gridPointDoubleLabels[j];

                // insert '1' in the first column 
                matrix[rowIndex] = 1.0;

                double x = 1.0;
                for (int columnIndex = 1; columnIndex <= order; columnIndex++)
                {
                    x *= abscissa;
                    matrix[rowIndex + gridPointCount * columnIndex] = x;
                }
                rowIndex++;
            }
            return new DenseMatrix(gridPointCount, order + 1, matrix);
        }
        #endregion

        /// <summary>Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.</returns>
        public override string ToString()
        {
            return MonomialBasisFunctions.Name.String;
        }
        #endregion
    }
}