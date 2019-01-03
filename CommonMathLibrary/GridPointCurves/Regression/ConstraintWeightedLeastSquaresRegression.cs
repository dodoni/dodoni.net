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
//    /// <summary>Represents a constraint weighted regression of a set of grid points, i.e.
//    /// <para>
//    ///   \sum_{j=1}^m   w_j * (y_j - \sum_{k=0}^N \beta_k * \phi_k(x_j))^2, 
//    /// </para>
//    /// where the grid point parameters are the weights w_j, N represents the <see cref="RegressionBasedCurveParametrization&lt;TRegressionBasedCurveFittingSetting&gt;.Order"/>,
//    /// \phi_k is given with respect to the <see cref="RegressionBasedCurveParametrization&lt;TRegressionBasedCurveFittingSetting&gt;.BasisFunctions"/> and \beta=(\beta_0,...,\beta_N)
//    /// are the unknown parameters taken into account some constraints.
//    /// </summary>
//    /// <remarks>See for example 
//    /// <para>
//    ///   Mathematische Statistik, G. Alsmeyer, Skripten zur Mathematischen Statistik Nr. 36.
//    /// </para>
//    /// We do some constraint least squares regression, where the design matrix is 
//    /// <para>
//    ///   B=W^{-1/2} * A, with W=diag(w_1,...,w_m) and A is the design matrix of the 'regular' least squares regression,
//    /// </para>
//    /// and the observed data 'y' is transformed by b=W^{-1/2}*y. This implementation use a quadratic program to solve this problem.
//    /// </remarks>
//    public class ConstraintWeightedLeastSquaresRegression 
//    {
//        #region public static (readonly) members

//        /// <summary>The language independent name of the curve parametrization.
//        /// </summary>
//        public static readonly IdentifierString Name = new IdentifierString("ConstraintWeightedLeastSquareRegression");
//        #endregion

//        #region private members

//        #region input members
//        /// <summary>The grid point parameters, i.e. the (positive) weights with respect to the weighted least square regression.
//        /// </summary>
//        private CurveDoubleGridPointParameters m_GridPointParameters;
//        #endregion

//        #region calculated vectors and intermediate matrices

//        /// <summary>The design matrix.
//        /// </summary>
//        private DenseMatrix m_DesignMatrix;

//        /// <summary>The matrix Q=A^t*A, where A is the design matrix.
//        /// </summary>
//        private DenseMatrix m_QMatrix;

//        /// <summary>The diagonal matrix W^{-1/2}, where W=diag(w_1,...,w_m), where w_j are the given weights.
//        /// </summary>
//        private double[] m_WPowMinusOneOverTwo;

//        /// <summary>Represents -W^{-1/2}*b, where 'b' represents the values of the grid points, needed for the BLAS operation.
//        /// </summary>
//        private double[] m_TempAdjustedInputValues;

//        /// <summary>The vector input for the optimizer.
//        /// </summary>
//        private double[] m_TempOptimizerVectorInput;
//        #endregion

//        #endregion

//        #region public constructors

//        /// <summary>Initializes a new instance of the <see cref="ConstraintWeightedLeastSquaresRegression"/> class.
//        /// </summary>
//        public ConstraintWeightedLeastSquaresRegression()
//        {
//            m_GridPointParameters = new CurveDoubleGridPointParameters(1.0, 0.0, Double.PositiveInfinity);
//        }
//        #endregion                

//        #region public methods

//        #region IDifferentiableCurveFitting Members

//        /// <summary>Gets a deep copy of the differentiable core curve builder represented by the current instance.
//        /// </summary>
//        /// <returns>
//        /// A deep copy of the current instance. One has to call <see cref="ICurveFitting.ReInitialize(double[],double[],int,ShiftableGridPointCurveState)"/>
//        /// before using the new instance.
//        /// </returns>
//        /// <remarks>The internal list of grid points will <c>not</c> be copied but the perhaps given grid point parameters
//        /// as well as class specific parameters.
//        /// <para>
//        /// One has to call <see cref="ICurveFitting.ReInitialize(double[],double[],int,ShiftableGridPointCurveState)"/> before using the copy.
//        /// </para>
//        /// </remarks>
//        IDifferentiableCurveFitting IDifferentiableCurveFitting.GetDeepWrapperCopy()
//        {
//            return new ConstraintWeightedLeastSquaresRegression(this, eSetupParameterCopyType.DeepCopy);
//        }
//        #endregion

//        #region ICurveFitting Members

//        /// <summary>Gets a deep copy of the curve builder represented by the current instance.
//        /// </summary>
//        /// <returns>
//        /// A deep copy of the current instance. One has to call <see cref="ICurveFitting.ReInitialize(double[],double[],int,ShiftableGridPointCurveState)"/>
//        /// before using the new instance.
//        /// </returns>
//        /// <remarks>The internal list of grid points will <c>not</c> be copied but the perhaps given grid point parameters
//        /// as well as class specific parameters.
//        /// <para>
//        /// One has to call <see cref="ICurveFitting.ReInitialize(double[],double[],int,ShiftableGridPointCurveState)"/> before using the copy.
//        /// </para>
//        /// </remarks>
//        ICurveFitting ICurveFitting.GetDeepWrapperCopy()
//        {
//            return new ConstraintWeightedLeastSquaresRegression(this, eSetupParameterCopyType.DeepCopy);
//        }

//        /// <summary>Gets a copy of the current instance where the (grid point) parameters as well as the <see cref="System.Double"/> representation of 
//        /// the labels are linked to the current instance.
//        /// </summary>
//        /// <returns>Some <see cref="ICurveFitting"/> instance with shared (grid point) parameters, i.e. the (grid point) 
//        /// parameters are always equal to the parameters of the current instance. Moreover the <see cref="System.Double"/> representation
//        /// of the labels are assumed to be equal as well. Therefore calling <see cref="ICurveFitting.ReInitialize(double[], double[], int, ShiftableGridPointCurveState)"/> 
//        /// of the result with <see cref="ShiftableGridPointCurveState.LabelsChanged"/> or <see cref="ShiftableGridPointCurveState.ValuesAndLabelsChanged"/> is not allowed and 
//        /// may thrown an exception; otherwise it will not have an impact on the current instance.</returns>
//        /// <remarks><para>This method will be used some multithreading environment.</para></remarks>
//        ICurveFitting ICurveFitting.GetSharedObject()
//        {
//            return new ConstraintWeightedLeastSquaresRegression(this, eSetupParameterCopyType.SharedCopy);
//        }

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

//        /// <summary>Sets a value indicating that at least one grid point has been changed but the current instance
//        /// has not been updated, i.e. the current instance is not operable any more. Moreover one has to
//        /// call <see cref="ICurveFitting.ReInitialize(double[],double[],int,ShiftableGridPointCurveState)"/>
//        /// before getting a value from the current instance.
//        /// </summary>
//        void ICurveFitting.MarkAsGridPointChanged()
//        {
//            m_GridPointChanged = true;
//        }

//        /// <summary>Reinitialize the current instance.
//        /// </summary>
//        /// <param name="doubleLabels">The labels in some <see cref="System.Double"/> representation.</param>
//        /// <param name="values">The values with respect to the <paramref name="doubleLabels"/>.</param>
//        /// <param name="gridPointCount">The number of grid points, i.e. the number of relevant elements of <paramref name="doubleLabels"/> and <paramref name="values"/>.</param>
//        /// <param name="state">The state of the grid points, i.e. <paramref name="doubleLabels"/> and <paramref name="values"/>, with respect to the previous function call.</param>
//        /// <remarks>This method will be called before starting some interpolation/extrapolation and will be called again if the underlying grid points
//        /// changed or some grid point has been added.</remarks>
//        /// <exception cref="InvalidOperationException">Thrown, if the <see cref="RegressionBasedCurveParametrization&lt;TRegressionBasedCurveFittingSetting&gt;.Order"/> is greater than the
//        /// number of grid points, i.e. less grid points than coefficients are given.</exception>
//        void ICurveFitting.ReInitialize(double[] doubleLabels, double[] values, int gridPointCount, ShiftableGridPointCurveState state)
//        {
//            this.ReInitialize(doubleLabels, values, gridPointCount, state);
//        }

//        /// <summary>Gets the grid point indices which are needed for some later (partially) reinitialization.
//        /// </summary>
//        /// <param name="labelCount">The number of labels. This argument has the same meaning as the corresponding parameter
//        /// in <see cref="ICurveFitting.ReInitializePartially(double[], double[], int, int, int, ShiftableGridPointCurveState)"/>.</param>
//        /// <param name="gridPointStartIndex">The null-based start index of the grid points taken into account, the last grid point is not allowed.</param>
//        /// <param name="gridPointCount">The number of grid points to taken into account, i.e. curve evaluation will be valid on
//        /// <para>
//        /// [t_j, t_{j+k}], where j is given by <paramref name="gridPointStartIndex"/> and  k is equal to <paramref name="gridPointCount"/>.
//        /// </para></param>
//        /// <param name="firstNeededGridPointIndex">The null-based index of the first grid point needed for some (partial) reinitialization of the current instance (output).</param>
//        /// <param name="lastNeededGridPointIndex">The null-based index of the last grid point needed for some (partial) reinitialization of the current instance (output).</param>
//        /// <remarks>
//        /// In the case of some partial reinitialization, <seealso cref="ICurveFitting.ReInitializePartially(double[], double[], int, int, int, ShiftableGridPointCurveState)"/>,
//        /// perhaps more grid points are needed than indicate by <paramref name="gridPointStartIndex"/> and <paramref name="gridPointCount"/>.
//        /// <para>
//        /// In the case of some two-dimensional surface labels are always given, but values are the result
//        /// of some horizontal or vertical interpolation. One would like to minimize the number of evaluations,
//        /// thus this method returns the indices of the grid points which are needed.
//        /// </para>
//        /// If the current instance is a global approach (i.e. <see cref="ICurveFitting.IsLocalApproach"/> is <c>false</c>),
//        /// <paramref name="firstNeededGridPointIndex"/> must be set to <c>0</c> and <paramref name="lastNeededGridPointIndex"/>
//        /// will return <paramref name="labelCount"/>-1.
//        /// </remarks>
//        void ICurveFitting.GetNeededGridPointIndicesForReInitialization(int labelCount, int gridPointStartIndex, int gridPointCount, out int firstNeededGridPointIndex, out int lastNeededGridPointIndex)
//        {
//            firstNeededGridPointIndex = 0;
//            lastNeededGridPointIndex = labelCount - 1;
//        }

//        /// <summary>Reinitialize the current instance partially.
//        /// </summary>
//        /// <param name="doubleLabels">The labels in some <see cref="System.Double"/> representation.</param>
//        /// <param name="values">The values with respect to the <paramref name="doubleLabels"/>.</param>
//        /// <param name="labelCount">The number of labels, i.e. the number of relevant elements of <paramref name="doubleLabels"/>.</param>
//        /// <param name="gridPointStartIndex">The null-based start index of the grid points taken into account, the last grid point is not allowed.</param>
//        /// <param name="gridPointCount">The number of grid points to taken into account, i.e. curve evaluation will be valid on
//        /// <para>
//        /// [t_j, t_{j+k}], where j is given by <paramref name="gridPointStartIndex"/> and  k is equal to <paramref name="gridPointCount"/>.
//        /// </para>
//        /// only.</param>
//        /// <param name="state">The state of the grid points, i.e. <paramref name="doubleLabels"/> and <paramref name="values"/>, with respect to the previous function call.</param>
//        /// <exception cref="ArgumentException">Thrown, if <paramref name="gridPointStartIndex"/> != 0 or <paramref name="gridPointCount"/> != <paramref name="labelCount"/>.</exception>
//        /// <remarks>
//        /// 	<para>
//        /// Call this method if some interpolation is needed on some sub-intervals only, as for example needed
//        /// in the case of two-dimensional surfaces to avoid some overhead. This method will be called before starting
//        /// some interpolation/extrapolation and will be called again if the underlying grid points changed
//        /// or some grid point has been added etc.
//        /// </para>
//        /// Perhaps more labels/values are needed than indicating by <paramref name="gridPointStartIndex"/> and <paramref name="gridPointCount"/>.
//        /// Call <see cref="ICurveFitting.GetNeededGridPointIndicesForReInitialization(int, int, int, out int, out int)"/> and fill
//        /// valid numbers into <paramref name="doubleLabels"/>, <paramref name="values"/> from the resulting
//        /// first needed grid point index up to the last needed grid point index before calling this method.
//        /// <para>
//        /// The instance will be indicate to be operable, but calculating values outside the range [t_j, t_{j+k}]
//        /// may gives wrong results or perhaps some exception will be thrown.
//        /// </para>
//        /// </remarks>
//        void ICurveFitting.ReInitializePartially(double[] doubleLabels, double[] values, int labelCount, int gridPointStartIndex, int gridPointCount, ShiftableGridPointCurveState state)
//        {
//            if (gridPointStartIndex != 0)
//            {
//                throw new ArgumentException("gridPointStartIndex");
//            }
//            if (gridPointCount != labelCount)
//            {
//                throw new ArgumentException("gridPointCount");
//            }
//            this.ReInitialize(doubleLabels, values, gridPointCount, state);
//        }
//        #endregion

//        /// <summary>Configurate the current least squares regression object.
//        /// </summary>
//        /// <param name="basisFunctions">The basis functions.</param>
//        /// <param name="order">The order with respect to the basis functions.</param>
//        /// <param name="optimizer">The quadratic program optimizer.</param>
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

//        /// <summary>Gets the coefficient for a specific null-based index.
//        /// </summary>
//        /// <param name="index">The index.</param>
//        /// <returns>The coefficient with respect to <paramref name="index"/>.</returns>
//        /// <exception cref="IndexOutOfRangeException">Thrown, if <paramref name="index"/> is negative
//        /// or <paramref name="index"/> is greater than the order or the 
//        /// current instance is not operable.</exception>
//        public double GetCoefficient(int index)
//        {
//            if (index <= m_CurveFittingSetting.Order)
//            {
//                return m_Coefficients[index];
//            }
//            throw new IndexOutOfRangeException(String.Format(ExceptionMessages.ArgumentOutOfRangeGreaterLessEqual, "index", 0, m_CurveFittingSetting.Order));
//        }

//        /// <summary>Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
//        /// </summary>
//        /// <returns>
//        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
//        /// </returns>
//        public override string ToString()
//        {
//            if (IsPreparedForOperation)
//            {
//                StringBuilder strBuilder = new StringBuilder();

//                strBuilder.Append(CurveTypeNames.ConstraintWeightedLeastSquaresRegression);
//                strBuilder.Append("(Basis functions:");
//                strBuilder.Append(m_CurveFittingSetting.BasisFunctions.Name.String);
//                strBuilder.Append(", Order: ");
//                strBuilder.Append(m_CurveFittingSetting.Order);

//                List<double> weights = m_GridPointParameters.m_Parameters;
//                if (weights.Count > 0)
//                {
//                    strBuilder.Append(", Weights: ");
//                    for (int j = 0; j < weights.Count; j++)
//                    {
//                        strBuilder.Append(weights[j]);
//                        if (j < weights.Count - 1)
//                        {
//                            strBuilder.Append(", ");
//                        }
//                    }
//                }
//                else
//                {
//                    strBuilder.Append(")");
//                }
//                return strBuilder.ToString();
//            }
//            return CurveTypeNames.ConstraintWeightedLeastSquaresRegression;
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
//        /// <exception cref="InvalidOperationException">Thrown, if the <see cref="RegressionBasedCurveParametrization&lt;TRegressionBasedCurveFittingSetting&gt;.Order"/> is greater than the
//        /// number of grid points, i.e. less grid points than coefficients are given.</exception>
//        protected void ReInitialize(double[] doubleLabels, double[] values, int gridPointCount, ShiftableGridPointCurveState state)
//        {
//            m_CurveFittingSetting.ReInitialize(doubleLabels, values, gridPointCount, state);
//            int order = m_CurveFittingSetting.Order;

//            // compute the singular value decomposition if and only if the labels have been changed or the user has change the order/basis functions:
//            if ((CurveFittingSettingChanged) || ((state & ShiftableGridPointCurveState.LabelsChanged) == ShiftableGridPointCurveState.LabelsChanged))
//            {
//                ResetCurveFittingSettingChangedFlag();
//                List<double> weights = m_GridPointParameters.m_Parameters;

//                ArrayMemoryPoolOld.Realloc<double>(ref m_WPowMinusOneOverTwo, gridPointCount, 5); // use a some small buffer
//                for (int j = 0; j < gridPointCount; j++)
//                {
//                    m_WPowMinusOneOverTwo[j] = 1.0 / Math.Sqrt(weights[j]);
//                }

//                if ((m_DesignMatrix != null) && (m_QMatrix != null))
//                {
//                    m_DesignMatrix.Dispose();
//                    m_QMatrix.Dispose();
//                }
//                m_DesignMatrix = m_CurveFittingSetting.BasisFunctions.GetDesignMatrix(doubleLabels, gridPointCount, order);
//                m_DesignMatrix.LeftMultiplyDiagonalMatrixAssignment(m_WPowMinusOneOverTwo);
//                m_QMatrix = DenseMatrix.Multiply(m_DesignMatrix, BLAS.MatrixTransposeState.Transpose, m_DesignMatrix, BLAS.MatrixTransposeState.NoTranspose);
//            }
//            // the vector for the optimization is equal to -z^t*A = -[A^t*z]^t, where A is the design matrix
//            // and z=y*W^{-1/2}
//            ArrayMemoryPoolOld.Realloc<double>(ref  m_TempAdjustedInputValues, gridPointCount, 4);
//            for (int j = 0; j < gridPointCount; j++)
//            {
//                m_TempAdjustedInputValues[j] = -values[j] * m_WPowMinusOneOverTwo[j];
//            }
//            ArrayMemoryPoolOld.Realloc<double>(ref m_TempOptimizerVectorInput, gridPointCount, 4);
//            BLAS.Level2.dgemv(gridPointCount, order + 1, 1.0, m_DesignMatrix.Data, m_TempAdjustedInputValues, 0.0, m_TempOptimizerVectorInput, BLAS.MatrixTransposeState.Transpose);

//            double minimum;
//            if (m_CurveFittingSetting.Optimizer.FindMinimum(m_QMatrix, m_TempOptimizerVectorInput, m_Coefficients, out minimum) != OptimizerState.ProperResult)
//            {
//                throw new ArithmeticException();
//            }
//        }
//        #endregion
//    }
//}