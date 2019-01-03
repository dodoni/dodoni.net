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
    /// <summary>Serves as interface for one-dimensional, real-valued, basic functions for a one-dimensional regression.
    /// </summary>
    ///<remarks>A basis function is a real-valued function \phi_j and for some given coefficients which are in general computed with respect to some regression, one compute
    ///<para>
    ///   \sum_{j=0}^d a_j * \phi_j(x),
    ///</para>
    /// which respresents the corresponding value and d is the order.
    /// <para>It is assumed that the basis functions are differentiable functions.</para>
    /// Instances of this type are assumed to be thread safe; any method is guaranted to be thread safe, too. 
    /// <example>\phi_j(x) = x^j, j = 0, 1,... represents the case of polynomials.</example>
    /// </remarks>
    public interface ILeastSquaresRegressionBasisFunctions : IIdentifierNameable
    {
        /// <summary>Gets the maximal order allowed for the current instance.
        /// </summary>
        /// <value>The maximal order.</value>
        int MaximalOrder
        {
            get;
        }

        /// <summary>Gets the value of a specific base function.
        /// </summary>
        /// <param name="order">The null-based index of the base function, i.e. the order.</param>
        /// <param name="x">The point to evaluate the specific base function.</param>
        /// <returns>The value of the base function given with respect to <paramref name="order"/> evaluated at <paramref name="x"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="order"/> is greater than <see cref="MaximalOrder"/> or less than <c>0</c>.</exception>
        double GetBaseFunctionValue(int order, double x);

        /// <summary>Gets the weighted sum, i.e. '\sum_{j=0}^d a_j * \phi_j(<paramref name="x"/>)', where (a_j) are
        /// the <paramref name="weights"/> and 'd' represents the <paramref name="order"/>.
        /// </summary>
        /// <param name="x">The point to evaluate the weighted sum of basis functions.</param>
        /// <param name="weights">The weights, i.e. an array with at least <paramref name="order"/>+1 elements.</param>
        /// <param name="order">The order.</param>
        /// <returns>The value of the weighted sum of the basis functions up to the given <paramref name="order"/>, evaluated at <paramref name="x"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="order"/> is greater than <see cref="MaximalOrder"/> or less than <c>0</c>.</exception>
        double GetValue(double x, double[] weights, int order);

        /// <summary>Evaluate the derivative of the weighted sum, i.e. '\sum_{j=0}^d a_j * \phi_j(<paramref name="x"/>)', where (a_j) are
        /// the <paramref name="weights"/> and 'd' represents the <paramref name="order"/>, at a specific point.
        /// </summary>
        /// <param name="x">The point to evaluate the derivative of the weighted sum of basis functions.</param>
        /// <param name="weights">The weights, i.e. an array with at least <paramref name="order"/>+1 elements.</param>
        /// <param name="order">The order.</param>
        /// <returns>The value of the derivative at the <paramref name="x"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="order"/> is greater than <see cref="MaximalOrder"/> or less than <c>0</c>.</exception>
        double GetDerivative(double x, double[] weights, int order);

        /// <summary>Gets the value of the integral \int_a^b f(x) dx, where f is the weighted sum '\sum_{j=0}^d a_j * \phi_j', 
        /// where (a_j) are the <paramref name="weights"/> and 'd' represents the <paramref name="order"/>.
        /// </summary>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="upperBound">The upper bound which is greater than <paramref name="lowerBound"/>.</param>
        /// <param name="weights">The weights, i.e. an array with at least <paramref name="order"/>+1 elements.</param>
        /// <param name="order">The order.</param>
        /// <returns>The value of \int_a^b f(x) dx, taking into account the explicit representation of the basis functions.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="order"/> is greater than <see cref="MaximalOrder"/> or less than <c>0</c>.</exception>
        double GetIntegral(double lowerBound, double upperBound, double[] weights, int order);

        /// <summary>Gets the design matrix of the linear least square problem, i.e.
        /// <para>
        ///   A =(a_{i,j}), where a_{i,j} = \phi_j(x_i)
        /// </para>
        /// with i=0,1,...,<paramref name="gridPointCount"/> -1 and j=0,...,<paramref name="order"/>.
        /// </summary>
        /// <param name="gridPointDoubleLabels">The labels of the grid points given in its <see cref="System.Double"/> representation.</param>
        /// <param name="gridPointCount">The number of grid points taken into account, moreover the number of rows of the output matrix.</param>
        /// <param name="order">The order, i.e. the number of columns of the output matrix.</param>
        /// <returns>The design matrix with respect to the specified arguments.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="order"/> is greater than <see cref="MaximalOrder"/> or less than <c>0</c>.</exception>
        DenseMatrix GetDesignMatrix(double[] gridPointDoubleLabels, int gridPointCount, int order);
    }
}