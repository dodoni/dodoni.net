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
//using System;
//using System.Text;
//using System.Collections.Generic;

//using Dodoni.BasicComponents;
//using Dodoni.MathLibrary.Basics;
//using Dodoni.MathLibrary.Basics.LowLevel;

//namespace Dodoni.MathLibrary.Curves
//{
//    /// <summary>Represents a one-dimensional constraint least squares regression, for example used in the <see cref="ShiftableGridPointDifferentiableCurve&lt;TLabel, TCurveFitting, TLeftCurveBuilder, TRightCurveBuilder&gt;"/> class.
//    /// </summary>
//    /// <remarks>The (constraint) least squares regression is given by 
//    /// <para>
//    ///   min{ (y-A*\beta)^t*(y-A*\beta): \beta },
//    /// </para>
//    /// where A is the design matrix, y are the observed values and \beta represents the coefficients,
//    /// which are somehow restricted. We reformulate the above problem as
//    /// <para>
//    ///   argMin_{\beta} (y-A*\beta)^t*(y-A*\beta) = argMin_{\beta} 0.5*\beta^t*Q*\beta -(y^t*A)*\beta,
//    /// </para>
//    /// where Q=A^t*Q and this implementation use a quadratic program to solve this problem.
//    /// </remarks>
//    public class ConstraintLeastSquaresRegression
//    {
//        #region public static (readonly) members

//        /// <summary>The language independent name of the curve parametrization.
//        /// </summary>
//        public static readonly IdentifierString Name = new IdentifierString("ConstraintLeastSquareRegression");
//        #endregion

//        #region private members

//        /// <summary>The design matrix.
//        /// </summary>
//        private DenseMatrix m_DesignMatrix;

//        /// <summary>The matrix Q=A^t*A, where A is the design matrix.
//        /// </summary>
//        private DenseMatrix m_QMatrix;

//        /// <summary>The vector input for the optimizer.
//        /// </summary>
//        private double[] m_TempOptimizerVectorInput;
//        #endregion

//        #region public constructors

//        /// <summary>Initializes a new instance of the <see cref="ConstraintLeastSquaresRegression"/> class.
//        /// </summary>
//        public ConstraintLeastSquaresRegression()
//        {
//        }
//        #endregion

//        #region public properties


//        #endregion

//        #region public methods



//        #region ICurveFitting Members



//        /// <summary>Gets some value for a specified point.
//        /// </summary>
//        /// <param name="pointToEvaluate">The point to evaluate.</param>
//        /// <param name="nonLastLeftGridIndex">The null-based index of the left neighbor grid point or the null-based index of the
//        /// last but one grid point if <paramref name="pointToEvaluate"/> is equal to the last grid point.</param>
//        /// <returns>The value of the curve at <paramref name="pointToEvaluate"/>, perhaps interpolated or taken into account some parametrization.</returns>
//        double ICurveFitting.GetValue(double pointToEvaluate, int nonLastLeftGridIndex)
//        {
//            return m_CurveFittingSetting.BasisFunctions.GetValue(pointToEvaluate, m_Coefficients, m_CurveFittingSetting.Order);  // the same as GetValue(double);
//        }







//        #endregion

//        /// <summary>Configurate the current least squares regression object.
//        /// </summary>
//        /// <param name="basisFunctions">The basis functions.</param>
//        /// <param name="order">The order with respect to the basis functions.</param>
//        /// <param name="optimizer">The quadratic program solver and the constrants.</param>
//        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="basisFunctions"/> or <paramref name="optimizer"/> is <c>null</c>.</exception>
//        /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="order"/> is negative
//        /// or greater than <see cref="ILeastSquaresRegressionBasisFunctions.MaximalOrder"/>.</exception>
//        public void Setup(ILeastSquaresRegressionBasisFunctions basisFunctions, int order, IQuadraticProgram optimizer)
//        {
//            if (IsSharedObject)  // the current instance is some shared object, no setup is allowed
//            {
//                throw new InvalidOperationException(String.Format(ExceptionMessages.ObjectIsInvalid, "Least squares regression", "Setup is not allowed for shared objects"));
//            }
//            lock (m_CurveFittingSetting)
//            {
//                SetCurveFittingSettingChangedFlag();

//                if (basisFunctions == null)
//                {
//                    throw new ArgumentNullException("basisFunctions");
//                }
//                m_CurveFittingSetting.BasisFunctions = basisFunctions;
//                if (order < 0)
//                {
//                    throw new ArgumentOutOfRangeException("order");
//                }
//                else if (order > basisFunctions.MaximalOrder)
//                {
//                    throw new ArgumentOutOfRangeException("order");
//                }
//                m_CurveFittingSetting.Order = order;
//                if (optimizer == null)
//                {
//                    throw new ArgumentNullException("optimizer");
//                }
//                m_CurveFittingSetting.Optimizer = optimizer;
//            }
//        }



//        #endregion

//        #region protected methods

//        /// <summary>Reinitialize the current instance.
//        /// </summary>
//        /// <param name="doubleLabels">The labels in some <see cref="System.Double"/> representation.</param>
//        /// <param name="values">The values with respect to the <paramref name="doubleLabels"/>.</param>
//        /// <param name="gridPointCount">The number of grid points, i.e. the number of relevant elements of <paramref name="doubleLabels"/> and <paramref name="values"/>.</param>
//        /// <param name="state">The state of the grid points, i.e. <paramref name="doubleLabels"/> and <paramref name="values"/>, with respect to the previous function call.</param>
//        /// <remarks>This method will be called before starting some interpolation/extrapolation and will be called again if the underlying grid points
//        /// changed or some grid point has been added.</remarks>
//        /// <exception cref="InvalidOperationException">Thrown, if the order is greater than the
//        /// number of grid points, i.e. less grid points than coefficients are given.</exception>
//        protected void ReInitialize(double[] doubleLabels, double[] values, int gridPointCount, ShiftableGridPointCurveState state)
//        {
//            m_CurveFittingSetting.ReInitialize(doubleLabels, values, gridPointCount, state);
//            int order = m_CurveFittingSetting.Order;

//            // compute the singular value decomposition if and only if the labels have been changed or the user has change the order/basis functions:
//            if ((CurveFittingSettingChanged) || ((state & ShiftableGridPointCurveState.LabelsChanged) == ShiftableGridPointCurveState.LabelsChanged))
//            {
//                ResetCurveFittingSettingChangedFlag();
//                if ((m_DesignMatrix != null) && (m_QMatrix != null))
//                {
//                    m_DesignMatrix.Dispose();
//                    m_QMatrix.Dispose();
//                }
//                m_DesignMatrix = m_CurveFittingSetting.BasisFunctions.GetDesignMatrix(doubleLabels, gridPointCount, order);
//                m_QMatrix = DenseMatrix.Multiply(m_DesignMatrix, BLAS.MatrixTransposeState.Transpose, m_DesignMatrix, BLAS.MatrixTransposeState.NoTranspose);
//            }

//            // the vector for the optimization is equal to -y^t*A = -[A^t*y]^t, where A is the design matrix
//            ArrayMemoryPoolOld.Realloc<double>(ref m_TempOptimizerVectorInput, gridPointCount, 4);  // will be the BLAS output
//            BLAS.Level2.dgemv(gridPointCount, order + 1, -1.0, m_DesignMatrix.Data, values, 0.0, m_TempOptimizerVectorInput, BLAS.MatrixTransposeState.Transpose);
//            ArrayMemoryPoolOld.Realloc<double>(ref m_Coefficients, order + 1, 4);  // small buffer

//            double minimum;
//            if (m_CurveFittingSetting.Optimizer.FindMinimum(m_QMatrix, m_TempOptimizerVectorInput, m_Coefficients, out minimum) != OptimizerState.ProperResult)
//            {
//                throw new ArithmeticException();
//            }
//            m_GridPointChanged = false;
//        }
//        #endregion
//    }
//}