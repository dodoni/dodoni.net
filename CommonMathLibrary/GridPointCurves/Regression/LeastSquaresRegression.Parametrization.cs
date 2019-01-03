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
using System.Data;
using System.Collections.Generic;

using Dodoni.BasicComponents;
using Dodoni.MathLibrary.Basics;
using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.GridPointCurves
{
    public partial class LeastSquaresRegression
    {
        /// <summary>Represents the implementation of the algorithm.
        /// </summary>
        private class Parametrization : ICurveDataFitting, IDifferentiableRealValuedCurve
        {
            #region private members

            /// <summary>The <see cref="LeastSquaresRegression"/> object which serves as factory for the current object.
            /// </summary>
            private LeastSquaresRegression m_LeastSquaresRegressionFactory;

            /// <summary>The number of grid points.
            /// </summary>
            private int m_GridPointCount;

            /// <summary>The arguments of the grid points, i.e. the labels on the x-axis.
            /// </summary>
            private double[] m_GridPointArguments;

            /// <summary>The values of the grid points.
            /// </summary>
            private double[] m_GridPointValues;

            /// <summary>The weights with respect to the basis functions.
            /// </summary>
            private double[] m_Coefficients;

            /// <summary>The orthogonal matrix V^t computed from the singular value decomposition A = U*\Sigma*V^t, where A is the design matrix.
            /// A quadratic matrix with dimension 'order'+1.
            /// </summary>
            private DenseMatrix m_Vt;

            /// <summary>The orthogonal matrix U computed from the singular value decomposition A=U*\Sigma*V^t, where A is the design matrix. 
            /// A quadratic matrix where the dimension is equal to the number of grid points.
            /// </summary>
            private DenseMatrix m_U;

            /// <summary>The adjusted reciprocal singular values, i.e. 1/s_j (taken into account the threshold's), where s_j are the diagonal elements 
            /// of \Sigma, computed from the singular value decomposition A=U*\Sigma*V^t, where A is the design matrix. This array contains at least 'order'+1 elements.
            /// </summary>
            private double[] m_AdjustedReciprocalSingularValues;

            /// <summary>A working array with at least 'order'+1 elements.
            /// </summary>
            private double[] m_WorkingArray;
            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="Parametrization"/> class.
            /// </summary>
            /// <param name="leastSquaresRegression">The <see cref="LeastSquaresRegression"/> object which serves as factory for the current object.</param>
            internal Parametrization(LeastSquaresRegression leastSquaresRegression)
            {
                m_LeastSquaresRegressionFactory = leastSquaresRegression;

                int order = leastSquaresRegression.Order;
                m_Coefficients = new double[order + 1];  // only 'order' elements are relevant
                m_WorkingArray = new double[order + 1];
                m_AdjustedReciprocalSingularValues = new double[order + 1];
            }
            #endregion

            #region public properties

            #region IOperable Members

            /// <summary>Gets a value indicating whether this instance is operable.
            /// </summary>
            /// <value>
            /// 	<c>true</c> if this instance is operable; otherwise, <c>false</c>.
            /// </value>
            public bool IsOperable
            {
                get { return (m_GridPointCount > 0); }
            }
            #endregion

            #region IRealValuedCurve Members

            /// <summary>Gets the lower bound of the domain of definition.
            /// </summary>
            /// <value>The lower bound of the domain of definition, perhaps <see cref="System.Double.NegativeInfinity"/> or <see cref="System.Double.NaN"/>.</value>
            public double LowerBound
            {
                get { return Double.NegativeInfinity; }
            }

            /// <summary>Gets the upper bound of the domain of definition.
            /// </summary>
            /// <value>The upper bound of the domain of definition, perhaps <see cref="System.Double.PositiveInfinity"/> or <see cref="System.Double.NaN"/>.</value>
            public double UpperBound
            {
                get { return Double.PositiveInfinity; }
            }
            #endregion

            #region ICurveDataFitting Member

            /// <summary>Gets the number of grid points.
            /// </summary>
            /// <value>The number of grid points.</value>
            public int GridPointCount
            {
                get { return m_GridPointCount; }
            }

            /// <summary>Gets the factory of <see cref="ICurveDataFitting" /> objects of the same type and configuration.
            /// </summary>
            /// <value>The factory of <see cref="ICurveDataFitting" /> objects of the same type and configuration.
            /// </value>
            public ICurveDataFittingFactory Factory
            {
                get { return m_LeastSquaresRegressionFactory; }
            }

            /// <summary>Gets the grid point arguments, i.e. the labels (on the x-axis) of the curve in its <see cref="System.Double" /> representation.
            /// </summary>
            /// <value>The grid point arguments.</value>
            public IList<double> GridPointArguments
            {
                get
                {
                    if (m_GridPointArguments == null)
                    {
                        return null;
                    }
                    return new SmartReadOnlyCollection<double>(m_GridPointCount, m_GridPointArguments);
                }
            }

            /// <summary>Gets the grid point values with respect to <see cref="ICurveDataFitting.GridPointArguments" />.
            /// </summary>
            /// <value>The grid point values.</value>
            public IList<double> GridPointValues
            {
                get
                {
                    if (m_GridPointValues == null)
                    {
                        return null;
                    }
                    return new SmartReadOnlyCollection<double>(m_GridPointCount, m_GridPointValues);
                }
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

            #endregion

            #region public methods

            #region ICurveDataFitting Members

            /// <summary>Updates the current curve fitting object.
            /// </summary>
            /// <param name="gridPointCount">The number of grid points, i.e. the number of relevant elements of <paramref name="gridPointArguments" /> and <paramref name="gridPointValues" /> to take into account.</param>
            /// <param name="gridPointArguments">The arguments of the grid points, thus labels of the curve in its <see cref="System.Double" /> representation in ascending order.</param>
            /// <param name="gridPointValues">The values of the grid points corresponding to <paramref name="gridPointArguments" />.</param>
            /// <param name="state">The state of the grid points, i.e. <paramref name="gridPointArguments" /> and <paramref name="gridPointValues" />, with respect to the previous function call.</param>
            /// <param name="gridPointArgumentStartIndex">The null-based start index of <paramref name="gridPointArguments" /> to take into account.</param>
            /// <param name="gridPointValueStartIndex">The null-based start index of <paramref name="gridPointValues" /> to take into account.</param>
            /// <param name="gridPointArgumentIncrement">The increment for <paramref name="gridPointArguments" />.</param>
            /// <param name="gridPointValueIncrement">The increment for <paramref name="gridPointValues" />.</param>
            /// <remarks>
            /// This method should be called if grid points have been changed, added, removed etc. and before evaluating the grid point curve at a specified point.
            /// <para>If no problem occurred, the flag <see cref="IOperable.IsOperable" /> will be set to <c>true</c>.</para>
            /// <para>This method should always store all required data for later use, i.e. creates deep copies of the arguments.</para>
            /// </remarks>
            public void Update(int gridPointCount, IList<double> gridPointArguments, IList<double> gridPointValues, GridPointCurve.State state, int gridPointArgumentStartIndex = 0, int gridPointValueStartIndex = 0, int gridPointArgumentIncrement = 1, int gridPointValueIncrement = 1)
            {
                if (gridPointCount <= 0)
                {
                    m_GridPointCount = 0;  // current instance is not operable
                }
                else
                {
                    m_GridPointCount = gridPointCount;
                    int order = m_LeastSquaresRegressionFactory.Order;

                    if (state.HasFlag(GridPointCurve.State.GridPointArgumentChanged))
                    {
                        ArrayMemory.Reallocate(ref m_GridPointArguments, gridPointCount, Math.Max(10, gridPointCount / 5));
                        gridPointArguments.CopyTo(m_GridPointArguments, gridPointCount, gridPointArgumentStartIndex, sourceIncrement: gridPointArgumentIncrement);

                        /* The optimal parameters (weights) are given by \beta = (A^t *A)^{-1} *A^t * y, where A is the design matrix and 
                         * y are the observations (=values of the grid points). For this, we compute the SVD of the design matrix 'A', i.e. 
                         * A = U*\Sigma*V^t, where U is a m-by-m, V is a n-by-n and \Sigma is a m-by-n matrix, where n = order +1 and m = number of grid points. It follows
                         * \beta = (V * \Sigma^{-1} * U^t) * y. 
                         * */

                        DenseMatrix designMatrix = m_LeastSquaresRegressionFactory.BasisFunctions.GetDesignMatrix(m_GridPointArguments, gridPointCount, order);
                        m_AdjustedReciprocalSingularValues = designMatrix.GetSingularValueDecomposition(out m_U, out m_Vt);

                        // compute \Sigma^{-1} =\diag(s_1,...,s_n,0,....,0)^{-1}:

                        double relThreshold = m_LeastSquaresRegressionFactory.RelativeSingularValueThreshold * m_AdjustedReciprocalSingularValues[0]; // the singular values are given in decresing order
                        double absSingularValueThreshold = m_LeastSquaresRegressionFactory.AbsoluteSingularValueThreshold;
                        for (int j = 0; j <= order; j++)
                        {
                            double singularValue = m_AdjustedReciprocalSingularValues[j];
                            if ((singularValue < absSingularValueThreshold) || (singularValue < relThreshold))
                            {
                                m_AdjustedReciprocalSingularValues[j] = 0;
                            }
                            else
                            {
                                m_AdjustedReciprocalSingularValues[j] = 1.0 / singularValue;
                            }
                        }
                    }
                    if (state.HasFlag(GridPointCurve.State.GridPointValueChanged))
                    {
                        ArrayMemory.Reallocate(ref m_GridPointValues, gridPointCount, Math.Max(10, gridPointCount / 5));
                        gridPointValues.CopyTo(m_GridPointValues, gridPointCount, gridPointValueStartIndex, sourceIncrement: gridPointValueIncrement);
                    }

                    /* one may compute b = U^t*b using BLAS, where 'b' are the grid point values, which gives a 
                     * vector of length #grid points. Afterwards, one divided the first (Order+1) elements with
                     * the adjusted singular values and truncate the vector (only the first (Order+1) elements are needed)
                     * for later use.
                     * 
                     * Disadvantages:
                     * 
                     * - b = U^t*b does no work with 'dgemv', but c = U^t*b + 0.0*c, where 'c' is some working 
                     *   array of length #grid points. This is a #grid point x #grid point operation,
                     * - the working array 'c' has #grid points elements, but we need the first (Order+1) elements only.
                     * 
                     * example BLAS Code: 
                     *
                     * m_WorkingArray = new double[values.Count];
                     * BLAS.Level2.dgemv(values.Count, values.Count, 1.0, m_U.m_Data, BLAS.MatrixTransposeState.Transpose, values, 0.0, m_WorkingArray);
                     * for (int k = 0; k <= m_Order; k++){
                     *    m_WorkingArray[k] *= m_AdjustedReciprocalSingularValues[k];}
                     *    
                     */
                    for (int j = 0; j <= order; j++)  // we do not use BLAS (see above), its a (order+1) * #grid point operation only
                    {
                        double value = 0.0;
                        for (int k = 0; k < gridPointCount; k++)
                        {
                            value += m_U[k, j] * m_GridPointValues[k];
                        }
                        m_WorkingArray[j] = value * m_AdjustedReciprocalSingularValues[j];
                    }
                    // the multiplication of the (Order+1,Order+1)-matrix V and the 'working array' gives the coefficients:
                    BLAS.Level2.dgemv(order + 1, order + 1, 1.0, m_Vt.Data, m_WorkingArray, 0.0, m_Coefficients, BLAS.MatrixTransposeState.Transpose);
                }
            }

            /// <summary>Gets the value of the integral \int_a^b f(x) dx inside two specific grid points.
            /// </summary>
            /// <param name="lowerBound">The lower bound; between the grid point arguments specified by <paramref name="leftGridPointIndex" /> and <paramref name="leftGridPointIndex" /> + 1.</param>
            /// <param name="upperBound">The upper bound; between the grid point arguments specified by <paramref name="leftGridPointIndex" /> and <paramref name="leftGridPointIndex" /> + 1.</param>
            /// <param name="leftGridPointIndex">The null-based index of the left grid point index.</param>
            /// <returns>The value of \int_a^b f(x) dx.</returns>
            public double GetIntegral(double lowerBound, double upperBound, int leftGridPointIndex)
            {
                return m_LeastSquaresRegressionFactory.BasisFunctions.GetIntegral(lowerBound, upperBound, m_Coefficients, m_LeastSquaresRegressionFactory.Order);
            }
            #endregion

            #region IRealValuedCurve Members

            /// <summary>Gets the value at a specific argument.
            /// </summary>
            /// <param name="pointToEvaluate">The point to evaluate.</param>
            /// <returns>The value of the curve at <paramref name="pointToEvaluate"/>.</returns>
            /// <remarks>The argument must be an element of the domain of definition, represented by <see cref="IRealValuedCurve.LowerBound"/> and <see cref="IRealValuedCurve.UpperBound"/>.</remarks>
            public double GetValue(double pointToEvaluate)
            {
                return m_LeastSquaresRegressionFactory.BasisFunctions.GetValue(pointToEvaluate, m_Coefficients, m_LeastSquaresRegressionFactory.Order);
            }

            /// <summary>Gets the value of the integral \int_a^b f(x) dx.
            /// </summary>
            /// <param name="lowerBound">The lower bound.</param>
            /// <param name="upperBound">The upper bound.</param>
            /// <returns>The value of \int_a^b f(x) dx.</returns>
            /// <remarks>The arguments must be elements of the domain of definition, represented by <see cref="IRealValuedCurve.LowerBound"/> and <see cref="IRealValuedCurve.UpperBound"/>.</remarks>
            public double GetIntegral(double lowerBound, double upperBound)
            {
                return m_LeastSquaresRegressionFactory.BasisFunctions.GetIntegral(lowerBound, upperBound, m_Coefficients, m_LeastSquaresRegressionFactory.Order);
            }
            #endregion

            #region IDifferentiableRealValuedCurve Members

            /// <summary>Gets the derivative at a specific point.
            /// </summary>
            /// <param name="pointToEvaluate">The point to evaluate.</param>
            /// <returns>The value of the derivative at the <paramref name="pointToEvaluate"/>.</returns>
            /// <remarks>The argument must be an element of the domain of definition, represented by <see cref="IRealValuedCurve.LowerBound"/> and <see cref="IRealValuedCurve.UpperBound"/>.</remarks>
            public double GetDerivative(double pointToEvaluate)
            {
                return m_LeastSquaresRegressionFactory.BasisFunctions.GetDerivative(pointToEvaluate, m_Coefficients, m_LeastSquaresRegressionFactory.Order);
            }
            #endregion

            #region IInfoOutputQueriable Members

            /// <summary>Gets informations of the current object as a specific <see cref="T:Dodoni.BasicComponents.Containers.InfoOutput"/> instance.
            /// </summary>
            /// <param name="infoOutput">The <see cref="T:Dodoni.BasicComponents.Containers.InfoOutput"/> object which is to be filled with informations concering the current instance.</param>
            /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
            public void FillInfoOutput(InfoOutput infoOutput, string categoryName = "General")
            {
                if (m_GridPointCount > 0)
                {
                    var infoOutputPackage = infoOutput.AcquirePackage(categoryName);

                    var coefficientDataTable = new DataTable("Coefficients");
                    coefficientDataTable.Columns.Add("Order", typeof(int));
                    coefficientDataTable.Columns.Add("Value", typeof(double));
                    for (int j = 0; j <= m_LeastSquaresRegressionFactory.Order; j++)
                    {
                        var row = coefficientDataTable.NewRow();
                        row[0] = j;
                        row[1] = m_Coefficients[j];
                        coefficientDataTable.Rows.Add(row);
                    }
                    infoOutputPackage.Add(coefficientDataTable);
                }
            }

            /// <summary>Sets the <see cref="P:Dodoni.BasicComponents.Containers.IInfoOutputQueriable.InfoOutputDetailLevel"/> property.
            /// </summary>
            /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
            /// <returns>A value indicating whether the <see cref="P:Dodoni.BasicComponents.Containers.IInfoOutputQueriable.InfoOutputDetailLevel"/> has been set to <paramref name="infoOutputDetailLevel"/>.</returns>
            public bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
            {
                return (infoOutputDetailLevel == InfoOutputDetailLevel.Full);
            }
            #endregion

            #endregion
        }
    }
}