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
using System.Text;
using System.Collections.Generic;

using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Containers;
using Dodoni.MathLibrary.Basics;

namespace Dodoni.MathLibrary.GridPointCurves
{
    /// <summary>Represents a weighted regression of a set of grid points, i.e.
    /// <para>
    ///   \sum_{j=1}^m   w_j * (y_j - \sum_{k=0}^N \beta_k * \phi_k(x_j))^2, 
    /// </para>
    /// where the grid point parameters are the weights w_j, N represents the order, \phi_k is the base function and \beta=(\beta_0,...,\beta_N) are the unknown parameters.
    /// </summary>
    /// <remarks>See for example 
    /// <para>
    ///   Mathematische Statistik, G. Alsmeyer, Skripten zur Mathematischen Statistik Nr. 36.
    /// </para>
    /// We do some least squares regression, where the design matrix is 
    /// <para>
    ///   B=W^{-1/2} * A, with W=diag(w_1,...,w_m) and A is the design matrix of the 'regular' least squares regression,
    /// </para>
    /// and the observed data 'y' is transformed by b=W^{-1/2}*y. See also
    /// <para>
    ///  Numerical Recipes in C, W. H. Press, §15.4,
    /// </para>
    /// apply to \sigma_j = 1/\sqrt(w_j).</remarks>
    [Obsolete("Zusätzliche Parameter der Update Funktion noch nicht korrekt")]
    public class WeightedLeastSquaresRegression : GridPointCurve.Parametrization
    {
        #region nested classes

        private class Parametrization : ICurveDataFitting, IDifferentiableRealValuedCurve
        {
            #region private members

            /// <summary>The number of grid points.
            /// </summary>
            private int m_GridPointCount;

            /// <summary>The arguments of the grid points, i.e. the labels on the x-axis.
            /// </summary>
            private double[] m_GridPointArguments;

            /// <summary>The values of the grid points.
            /// </summary>
            private double[] m_GridPointValues;

            /// <summary>The 
            /// </summary>
            private WeightedLeastSquaresRegression m_LeastSquaresRegression;

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

            /// <summary>The diagonal matrix W^{-1/2}, where W=diag(w_1,...,w_m), where w_j are the weights.
            /// </summary>
            private double[] m_WPowMinusOneOverTwo;

            /// <summary>A working array with at least 'order'+1 elements.
            /// </summary>
            private double[] m_WorkingArray;
            #endregion

            #region internal constructors

            internal Parametrization(WeightedLeastSquaresRegression leastSquaresRegression)
            {
                int order = leastSquaresRegression.m_Order;

                m_Coefficients = new double[order];
                m_WorkingArray = new double[order + 1];
                m_AdjustedReciprocalSingularValues = new double[order + 1];

                m_LeastSquaresRegression = leastSquaresRegression;
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
                get { return (m_GridPointCount >= 2); }
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

            #region ICurveBuilder Members

            /// <summary>Gets the number of grid points.
            /// </summary>
            /// <value>The number of grid points.</value>
            public int GridPointCount
            {
                get { return m_GridPointCount; }
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

            #region ICurveBuilder Members

            /// <summary>Updates the current curve interpolator.
            /// </summary>
            /// <param name="gridPointCount">The number of grid points, i.e. the number of relevant elements of <paramref name="gridPointArguments"/> and <paramref name="gridPointValues"/> to take into account.</param>
            /// <param name="gridPointArguments">The arguments of the grid points, thus labels of the curve in its <see cref="System.Double"/> representation.</param>
            /// <param name="gridPointValues">The values of the grid points corresponding to <paramref name="gridPointArguments"/>.</param>
            /// <param name="state">The state of the grid points, i.e. <paramref name="gridPointArguments"/> and <paramref name="gridPointValues"/>, with respect to the previous function call.</param>
            /// <param name="gridPointArgumentsStartIndex">The null-based start index of <paramref name="gridPointArguments"/> to take into account.</param>
            /// <param name="gridPointValuesStartIndex">The null-based start index of <paramref name="gridPointValues"/> to take into account.</param>
            /// <remarks>This method should be called if grid points have been changed, added, removed etc. and before evaluating the grid point curve at a specified point.
            /// <para>If no problem occurred, the flag <see cref="IOperable.IsOperable"/> will be set to <c>true</c>.</para>
            /// </remarks>
            public void Update(int gridPointCount, IList<double> gridPointArguments, IList<double> gridPointValues, GridPointCurve.State state, int gridPointArgumentsStartIndex = 0, int gridPointValuesStartIndex = 0)
            {
                if (gridPointCount <= 0)
                {
                    m_GridPointCount = 0;  // current instance is not operable
                }
                else
                {
                    m_GridPointCount = gridPointCount;
                    int order = m_LeastSquaresRegression.m_Order;

                    if (state.HasFlag(GridPointCurve.State.GridPointArgumentChanged))
                    {
                        ArrayMemory.Reallocate(ref m_GridPointArguments, gridPointCount, Math.Max(10, gridPointCount / 5));
                        gridPointArguments.CopyTo(m_GridPointArguments, gridPointCount, gridPointArgumentsStartIndex);

                        /* The optimal parameters are given by \beta = (B^t *B)^{-1} *B^t * y, where B=W^{-1/2}*A and
                         * 'A' is the design matrix and y=W^{-1/2}*b are the (transformed) observations (=values of the grid points). For this, we compute
                         * the SVD of the design matrix 'B', i.e. B = U*\Sigma*V^t, where U is a m-by-m, V is a n-by-n 
                         * and \Sigma is a m-by-n matrix, where n = order +1 and m = number of grid points.
                         * and it follows
                         * 
                         * \beta = (V * \Sigma^{-1} * U^t) * y. 
                         */

                        DenseMatrix designMatrix = m_LeastSquaresRegression.m_BasisFunction.GetDesignMatrix(m_GridPointArguments, gridPointCount, order);

                        List<double> weights = new List<double>(); // the weights! Input!!

                        m_WPowMinusOneOverTwo = new double[gridPointCount];
                        for (int j = 0; j < gridPointCount; j++)
                        {
                            m_WPowMinusOneOverTwo[j] = 1.0 / Math.Sqrt(weights[j]);
                        }
                        designMatrix.LeftMultiplyDiagonalMatrixAssignment(m_WPowMinusOneOverTwo);
                        m_AdjustedReciprocalSingularValues = designMatrix.GetSingularValueDecomposition(out m_U, out m_Vt);


                        // compute \Sigma^{-1} =\diag(s_1,...,s_n,0,....,0)^{-1}:

                        double relThreshold = m_LeastSquaresRegression.m_RelativeSingularValueThreshold * m_AdjustedReciprocalSingularValues[0]; // the singular values are given in decresing order
                        double absSingularValueThreshold = m_LeastSquaresRegression.m_AbsoluteSingularValueThreshold;
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
                        gridPointValues.CopyTo(m_GridPointValues, gridPointCount, gridPointValuesStartIndex);
                    }

                    /* one may compute b = U^t*b using BLAS, where 'b' are the grid point values, which gives a 
                     * vector of length #grid points. Afterwards, one divided the first (Order+1) elements with
                     * the adjusted singular values and truncate the vector (only the first (Order+1) elements are needed)
                     * for later use.
                     * 
                     * Disadvantages:
                     * 
                     * - b = W^{-1/2}*b with a diagonal matrix W
                     * - b = U^t*b does no work with 'dgemv', but c = U^t*b + 0.0*c, where 'c' is some working 
                     *   array of length #grid points. This is a #grid point x #grid point operation,
                     * - the working array has #grid points elements, but we need the first (Order+1) elements only.
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
                            value += m_U[k, j] * m_GridPointValues[k] * m_WPowMinusOneOverTwo[k];
                        }
                        m_WorkingArray[j] = value * m_AdjustedReciprocalSingularValues[j];
                    }
                    // the multiplication of the (Order+1,Order+1)-matrix V and the 'working array' gives the coefficients:
                    BLAS.Level2.dgemv(order + 1, order + 1, 1.0, m_Vt.Data, m_WorkingArray, 0.0, m_Coefficients, BLAS.MatrixTransposeState.Transpose);
                }
            }

            /// <summary>Gets the argument of a specified grid point.
            /// </summary>
            /// <param name="gridPointIndex">The null-based index of the grid point.</param>
            /// <returns>The argument of the specified grid point, thus the label (on the x-axis) in its <see cref="System.Double"/> representation.</returns>
            public double GetGridPointArgument(int gridPointIndex)
            {
                return m_GridPointArguments[gridPointIndex];
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
                return m_LeastSquaresRegression.m_BasisFunction.GetValue(pointToEvaluate, m_Coefficients, m_LeastSquaresRegression.m_Order);
            }

            /// <summary>Gets the value of the integral \int_a^b f(x) dx.
            /// </summary>
            /// <param name="lowerBound">The lower bound.</param>
            /// <param name="upperBound">The upper bound.</param>
            /// <returns>The value of \int_a^b f(x) dx.</returns>
            /// <remarks>The arguments must be elements of the domain of definition, represented by <see cref="IRealValuedCurve.LowerBound"/> and <see cref="IRealValuedCurve.UpperBound"/>.</remarks>
            public double GetIntegral(double lowerBound, double upperBound)
            {
                return m_LeastSquaresRegression.m_BasisFunction.GetIntegral(lowerBound, upperBound, m_Coefficients, m_LeastSquaresRegression.m_Order);
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
                return m_LeastSquaresRegression.m_BasisFunction.GetDerivative(pointToEvaluate, m_Coefficients, m_LeastSquaresRegression.m_Order);
            }
            #endregion

            #region IInfoOutputQueriable Members

            public void FillInfoOutput(InfoOutput infoOutput, string categoryName = "General")
            {
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

            #region ICurveBuilder Members


            public void Update(int gridPointCount, IList<double> gridPointArguments, IList<double> gridPointValues, GridPointCurve.State state, int gridPointArgumentStartIndex = 0, int gridPointValueStartIndex = 0, int gridPointArgumentIncrement = 1, int gridPointValueIncrement = 1)
            {
                throw new NotImplementedException();
            }

            #endregion

            #region ICurveBuilder Members


            public IList<double> GridPointArguments
            {
                get { throw new NotImplementedException(); }
            }

            public IList<double> GridPointValues
            {
                get { throw new NotImplementedException(); }
            }

            #endregion

            #region ICurveDataFitting Member

            public ICurveDataFittingFactory Factory
            {
                get { throw new NotImplementedException(); }
            }

            #endregion

            #region ICurveDataFitting Members


            public double GetIntegral(double lowerBound, double upperBound, int leftGridPointIndex)
            {
                throw new NotImplementedException();
            }

            #endregion
        }
        #endregion

        #region private members

        /// <summary>The order of the regression.
        /// </summary>
        private int m_Order;

        /// <summary>The bais function for the regression.
        /// </summary>
        private ILeastSquaresRegressionBasisFunctions m_BasisFunction;

        /// <summary>The absolute threshold for singular values, i.e. singular values less than the threshold are assumed to be <c>0.0</c>.
        /// </summary>
        private double m_AbsoluteSingularValueThreshold;

        /// <summary>The relative threshold for singular values, i.e. singular values less than the product of the relative threshold and the greatest singular value are assumed to be <c>0.0</c>.
        /// </summary>
        private double m_RelativeSingularValueThreshold;

        /// <summary>The name of the parametrization.
        /// </summary>
        private IdentifierString m_Name;

        /// <summary>The long name of the parametrization.
        /// </summary>
        private IdentifierString m_LongName;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="WeightedLeastSquaresRegression"/> class.
        /// </summary>
        /// <param name="order">The order of the regression.</param>
        /// <param name="basisFunction">The basis function to take into account for the regression.</param>
        /// <param name="absoluteSingularValueThreshold">The absolute threshold for singular values, i.e. singular values less than the threshold are assumed to be <c>0.0</c>.</param>
        /// <param name="relativeSingularValueThreshold">The relative threshold for singular values, i.e. singular values less than the product of the relative threshold and the greatest singular value are assumed to be <c>0.0</c>.</param>
        public WeightedLeastSquaresRegression(int order, ILeastSquaresRegressionBasisFunctions basisFunction, double absoluteSingularValueThreshold = MachineConsts.Epsilon, double relativeSingularValueThreshold = MachineConsts.Epsilon)
            : base(order + 1)
        {
            if (basisFunction == null)
            {
                throw new ArgumentNullException("baisFunction");
            }
            m_BasisFunction = basisFunction;

            if (order < 1)
            {
                throw new ArgumentOutOfRangeException("order");
            }
            m_Order = order;

            m_AbsoluteSingularValueThreshold = absoluteSingularValueThreshold;
            m_RelativeSingularValueThreshold = relativeSingularValueThreshold;

            m_Name = new IdentifierString("WeightedLeastSquareRegression");
        }
        #endregion

        #region public methods

        /// <summary>Creates a <see cref="ICurveBuilder"/> object that represents the implementation of the curve parametrization approach.
        /// </summary>
        /// <returns>A <see cref="ICurveBuilder"/> object that represents the implementation of the curve parametrization approach.</returns>
        public override ICurveDataFitting Create()
        {
            return new Parametrization(this);
        }
        #endregion

        #region protected methods

        /// <summary>Gets the name of the curve parametrization.
        /// </summary>
        /// <returns>The name of the curve parametrization.</returns>
        protected override IdentifierString GetName()
        {
            return m_Name;
        }

        /// <summary>Gets the long name of the curve parametrization.
        /// </summary>
        /// <returns>The (perhaps) language dependent long name of the curve parametrization.</returns>
        protected override IdentifierString GetLongName()
        {
            return m_LongName;
        }
        #endregion
    }
}