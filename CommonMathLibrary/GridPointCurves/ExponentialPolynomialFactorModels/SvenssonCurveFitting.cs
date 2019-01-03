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

//namespace Dodoni.MathLibrary.Curves
//{
//    public class SvenssonCurveFitting
//    {
//    }

//        /// <summary>Represents the Svensson cap volatility parametrisiation.
//    /// </summary>
//    /// <remarks>Based on 'The LIBOR market model', Nevena Selic, School of Computational 
//    /// and spplied mathematics, University of the Witwatersrand, May 2006.
//    /// <para>
//    /// The interpolation (parametrization) takes place for fixed strikes only. Thus the vertical 
//    /// interpolation is the only interpolation which can be applied to this class.
//    /// </para>
//    /// The parametrisation is given by 
//    ///  <para>
//    ///    \sigma(T) = \beta_0 + (\beta_1 + \beta_2 * T) * e^{-\alpha_1 * T} + \beta_3 * T * e^{-\alpha_2 * T}
//    /// </para>
//    /// </remarks>    
//    //public class SvenssonCapInterpolationRule : IVicinityInterpolationRule, ICollectableLogFileMessages
//    //{
//    //    #region private static (readonly) members

//    //    /// <summary>The position of the parameter '\beta_0' in the array <see cref="m_ParameterSet"/>.
//    //    /// </summary>
//    //    private static readonly int sm_Beta0_Position = 0;

//    //    /// <summary>The position of the parameter '\beta_1' in the array <see cref="m_ParameterSet"/>.
//    //    /// </summary>
//    //    private static readonly int sm_Beta1_Position = 1;

//    //    /// <summary>The position of the parameter '\beta_2' in the array <see cref="m_ParameterSet"/>.
//    //    /// </summary>
//    //    private static readonly int sm_Beta2_Position = 2;

//    //    /// <summary>The position of the parameter '\beta_3' in the array <see cref="m_ParameterSet"/>.
//    //    /// </summary>
//    //    private static readonly int sm_Beta3_Position = 3;

//    //    /// <summary>The position of the parameter '\alpha_0' in the array <see cref="m_ParameterSet"/>.
//    //    /// </summary>
//    //    private static readonly int sm_Alpha1_Position = 4;

//    //    /// <summary>The position of the parameter '\alpha_1' in the array <see cref="m_ParameterSet"/>.
//    //    /// </summary>
//    //    private static readonly int sm_Alpha2_Position = 5;

//    //    #endregion

//    //    #region private members

//    //    #region setup members

//    //    /// <summary>The optimizer used for the parameter \alpha_1 and \alpha_2 only.
//    //    /// </summary>
//    //    private IMultiDimOptimizerAlgorithm m_Optimizer;

//    //    #endregion

//    //    #region members needed for the deviation function

//    //    /// <summary>The null-based index of the strike needed for the deviation function.
//    //    /// </summary>
//    //    private int m_CurrentStrikeIndex;

//    //    /// <summary>The vertical labels in some <see cref="System.Double"/> representation.
//    //    /// </summary>
//    //    private double[] m_VerticalLabelsAsDouble;

//    //    /// <summary>The design matrix for the linear regression.
//    //    /// </summary>
//    //    private ILArray<double> m_DesignMatrix;

//    //    /// <summary>Matrices, needed for the singular value decomposition, i.e. for the solving the linear regression problem.
//    //    /// </summary>
//    //    private ILArray<double> m_U, m_V, m_SingularValues;

//    //    /// <summary>The one-step parameter estimation, i.e. the parameter \beta_0, \beta_1, \beta_2, \beta3.
//    //    /// </summary>
//    //    private ILArray<double> m_ParameterEstimator;

//    //    #endregion

//    //    /// <summary>The set of Svensson parameter, i.e. the first index corresponds to the
//    //    /// strike and the second to the six paramter which are given in the order which are indicated
//    //    /// by the above static members.
//    //    /// </summary>
//    //    private double[][] m_ParameterSet;

//    //    /// <summary>The given cap volatility matrix.
//    //    /// </summary>
//    //    private ILArray<double> m_CapVolaMatrix;

//    //    /// <summary>A list of log-file messages.
//    //    /// </summary>
//    //    private List<string> m_LogFileStrings = new List<string>();

//    //    #endregion

//    //    #region public constructors

//    //    /// <summary>Initializes a new instance of the <see cref="SvenssonCapInterpolationRule"/> class.
//    //    /// </summary>
//    //    /// <param name="optimizer">The optimizer taken into account for the parameter \alpha_1 and \alpha_2 only.</param>
//    //    /// <remarks>The <paramref name="optimizer"/> has to be initialized before, i.e. set some initial guess etc.</remarks>
//    //    public SvenssonCapInterpolationRule(IMultiDimOptimizerAlgorithm optimizer)
//    //    {
//    //        if (optimizer == null)
//    //        {
//    //            throw new ArgumentNullException("Initialization of 'SvenssonCapInterpolationRule' fails: The given multi-dimensional optimizer instance is 'null'.");
//    //        }
//    //        m_Optimizer = optimizer;
//    //        m_Optimizer.SetupOptimizer(2, DeviationFunction, null);
//    //        m_U = new ILArray<double>();
//    //        m_V = new ILArray<double>();
//    //        m_ParameterEstimator = new ILArray<double>(4);
//    //    }

//    //    #endregion

//    //    #region public methods

//    //    #region public properties

//    //    #region IVicinityInterpolationRule Members

//    //    /// <summary>Gets some info string which contains a string representation of the interpolation rule.
//    //    /// </summary>
//    //    /// <value>The info string.</value>
//    //    public string InfoString
//    //    {
//    //        get { return "Svensson cap volatility interpolation rule"; }
//    //    }

//    //    #endregion

//    //    #endregion

//    //    private double DeviationFunction(double[] alpha)
//    //    {
//    //        /* build the design matrix, i.e. 
//    //         *     [ 1  exp(-T1 * \alpha_1)  T_1 * exp(-T1 * \alpha_1   T1 * exp(-\alpha_2 * T1)) ]
//    //         * A = | ...
//    //         *     [ 1  exp(-Tn * \alpha_1)  T_n * exp(-Tn * \alpha_1)  Tn * exp(-\alpha_2 * Tn)) ]
//    //         * */
//    //        double alpha1 = alpha[0];
//    //        double alpha2 = alpha[1];
//    //        for (int maturityIndex = 0; maturityIndex < m_VerticalLabelsAsDouble.Length; maturityIndex++)
//    //        {
//    //            double expOfMinusMaturityTimesAlpha1 = Math.Exp(-m_VerticalLabelsAsDouble[maturityIndex] * alpha1);
//    //            m_DesignMatrix[maturityIndex, 1] = expOfMinusMaturityTimesAlpha1;
//    //            m_DesignMatrix[maturityIndex, 2] = m_VerticalLabelsAsDouble[maturityIndex] * expOfMinusMaturityTimesAlpha1;
//    //            m_DesignMatrix[maturityIndex, 3] = m_VerticalLabelsAsDouble[maturityIndex] * Math.Exp(-alpha2 * m_VerticalLabelsAsDouble[maturityIndex]);
//    //        }
//    //        m_SingularValues = ILAlgorithm.svd(m_DesignMatrix, ref m_U, ref m_V); // A = U * \Sigma * V, where \Sigma = \diag(singularValues)

//    //        /* the estimation of the parameter \beta, i.e. 'y = A \beta + \epsilon' is given by
//    //         * 
//    //         * \beta = \sum_{i=1}^3 \frac{u_i^T y}{\sigma_i} * v_i,
//    //         * 
//    //         * where U=(u_1,...,u_n), V = (v_1,..,v3) */

//    //        m_ParameterEstimator.SetValue(0.0, 0, 0);
//    //        m_ParameterEstimator.SetValue(0.0, 1, 0);
//    //        m_ParameterEstimator.SetValue(0.0, 2, 0);
//    //        m_ParameterEstimator.SetValue(0.0, 3, 0);

//    //        for (int i = 0; i < 4; i++)
//    //        {
//    //            double singularValue = m_SingularValues.GetValue(i, i);
//    //            if (singularValue > MachineConsts.TinyEpsilon)
//    //            {
//    //                m_ParameterEstimator += (ILAlgorithm.multiply(m_U[null, i].T, m_CapVolaMatrix[null, m_CurrentStrikeIndex]) / singularValue) * m_V[null, i];
//    //            }
//    //        }
//    //        return  ILAlgorithm.norm(ILAlgorithm.multiply(m_DesignMatrix, m_ParameterEstimator) - m_CapVolaMatrix[null, m_CurrentStrikeIndex], 2).GetValue(0);
//    //    }

//    //    #region IVicinityInterpolationRule Members

//    //    /// <summary>Initializes the current instance.
//    //    /// </summary>
//    //    /// <param name="matrix">The underlying matrix, the first index is the row, the second the column.</param>
//    //    /// <param name="horizontalLabelsAsDouble">The horizontal labels in some <see cref="System.Double"/> representation.</param>
//    //    /// <param name="verticalLabelsAsDouble">The vertical labels in some <see cref="System.Double"/> representation.</param>
//    //    /// <remarks>This method will be used for example to compute spline coefficients.</remarks>
//    //    public void Initialize(ILArray<double> matrix, double[] horizontalLabelsAsDouble, double[] verticalLabelsAsDouble)
//    //    {
//    //        int strikeCount = horizontalLabelsAsDouble.Length;
//    //        int maturityCount = verticalLabelsAsDouble.Length;
//    //        m_VerticalLabelsAsDouble = verticalLabelsAsDouble;


//    //        m_CapVolaMatrix = matrix;
//    //        m_ParameterSet = new double[strikeCount][];

//    //        m_DesignMatrix = new ILArray<double>(maturityCount, 4);   // the matrix 'A' used for the regression Y = A \beta + \epsilon
//    //        m_DesignMatrix[null, 0] = 1;


//    //        /* For each strike a separate calibration takes place. We fixed the parameter \alpha
//    //         * and do some regression. Changing the parameter \alpha with respect to some discrete 
//    //         * points in time and choose the \alpha_1, \alpha_2 (as well as \beta_1,..\beta_3) which fits in the best way.
//    //         * Keep in mind that this is a symmetric problem, i.e. lets assume \alpha_1 \geq \alpha_2.
//    //         * */
//    //        for (m_CurrentStrikeIndex = 0; m_CurrentStrikeIndex < strikeCount; m_CurrentStrikeIndex++)
//    //        {
//    //            m_ParameterSet[m_CurrentStrikeIndex] = new double[6];

//    //            m_Optimizer.FindMinimum();

//    //            m_ParameterSet[m_CurrentStrikeIndex][sm_Alpha1_Position] = m_Optimizer.Result.ArgMin[0];
//    //            m_ParameterSet[m_CurrentStrikeIndex][sm_Alpha2_Position] = m_Optimizer.Result.ArgMin[1];
//    //            m_ParameterSet[m_CurrentStrikeIndex][sm_Beta0_Position] = m_ParameterEstimator.GetValue(0);
//    //            m_ParameterSet[m_CurrentStrikeIndex][sm_Beta1_Position] = m_ParameterEstimator.GetValue(1);
//    //            m_ParameterSet[m_CurrentStrikeIndex][sm_Beta2_Position] = m_ParameterEstimator.GetValue(2);
//    //            m_ParameterSet[m_CurrentStrikeIndex][sm_Beta3_Position] = m_ParameterEstimator.GetValue(3);

//    //            m_LogFileStrings.Add("Svenson estimation error of " + m_Optimizer.Result.Minimum + " for strike " + horizontalLabelsAsDouble[m_CurrentStrikeIndex].ToString("p"));
//    //        }
//    //    }

//    //    /// <summary>Gets an interpolated value of some <see cref="VicinityMatrix&lt;HorizontalLabelType, VerticalLabelType&gt;"/> instance
//    //    /// doing some interpolation on the vertical direction only.
//    //    /// </summary>
//    //    /// <param name="y">The y coordinate of point to interpolate (corresponds to an expiry).</param>
//    //    /// <param name="rowIndex">The null-based row index of the upper grid point, i.e. the null-based index of the expiry, (add <c>1</c> for the lower grid point).</param>
//    //    /// <param name="columnIndex">The null-based column index, i.e. the null-based index of the strike.</param>
//    //    /// <returns>
//    //    /// A <see cref="System.Double"/> which reflects to the interpolated value.
//    //    /// </returns>
//    //    /// <remarks>
//    //    /// Two grid points (x,y1,z1), (x,y2,z2) are given and it is assumed that these points have the same y-coordinate.
//    //    /// This method does a interpolation of some point (x,y) where y is between y1 and y2.
//    //    /// </remarks>
//    //    public double GetVerticalValue(double y, int rowIndex, int columnIndex)
//    //    {
//    //        double[] svenssonParameter = m_ParameterSet[columnIndex];

//    //        return svenssonParameter[sm_Beta0_Position] + (svenssonParameter[sm_Beta1_Position] + svenssonParameter[sm_Beta2_Position] * y) * Math.Exp(-svenssonParameter[sm_Alpha1_Position] * y)
//    //            + svenssonParameter[sm_Beta3_Position] * y * Math.Exp(-svenssonParameter[sm_Alpha2_Position] * y);
//    //    }

//    //    /// <summary>Gets an interpolated value of some <see cref="TransformableVicinityMatrix&lt;HorizontalLabelType, VerticalLabelType&gt;"/> instance.
//    //    /// </summary>
//    //    /// <param name="x">The x coordinate of point to interpolate.</param>
//    //    /// <param name="y">The y coordinate of point to interpolate.</param>
//    //    /// <param name="rowIndex">The null-based row index of the upper corner point, i.e. <c>y1</c> (add <c>1</c> for the lower grid point y2).</param>
//    //    /// <param name="columnIndex">The null-based column index of the left corner point, i.e. <c>x1</c> (add <c>1</c> for the right corner point x2).</param>
//    //    /// <param name="z1">The upper left corner value.</param>
//    //    /// <param name="z2">The upper right corner value.</param>
//    //    /// <param name="z3">The lower right corner value.</param>
//    //    /// <param name="z4">The lower left corner value.</param>
//    //    /// <returns>
//    //    /// A <see cref="System.Double"/> which reflects to the interpolated value.
//    //    /// </returns>
//    //    /// <remarks>The points are given clockwise starting in the left upper corner. The points (x_i, y_i) and values z_i are
//    //    /// connected as follows:
//    //    /// <para>
//    //    /// (x1,y1,z1), (x2,y1,z2), (x2,y2,z3), (x1,y2,z4).
//    //    /// </para>
//    //    /// Only the null-based indices of x1 and y1 are given. One plus these values corresponds to x2
//    //    /// respectively y2. Furthermore z1,z2,z3,z4 are given here to allow some transformation before applying the interpolation rule.
//    //    /// </remarks>
//    //    /// <exception cref="InvalidOperationException">Will be thrown because this type of interpolation is not allowed for cap volatiliites.</exception>
//    //    public double GetValue(double x, double y, int rowIndex, int columnIndex, double z1, double z2, double z3, double z4)
//    //    {
//    //        throw new InvalidOperationException("This type of interpolation is not allowed for cap volatilites.");
//    //    }

//    //    /// <summary>Gets an interpolated value of some <see cref="VicinityMatrix&lt;HorizontalLabelType, VerticalLabelType&gt;"/> instance.
//    //    /// </summary>
//    //    /// <param name="x">The x coordinate of point to interpolate.</param>
//    //    /// <param name="y">The y coordinate of point to interpolate.</param>
//    //    /// <param name="rowIndex">The null-based row index of the upper corner point, i.e. <c>y1</c> (add <c>1</c> for the lower grid point y2).</param>
//    //    /// <param name="columnIndex">The null-based column index of the left corner point, i.e. <c>x1</c> (add <c>1</c> for the right corner point x2).</param>
//    //    /// <returns>
//    //    /// A <see cref="System.Double"/> which reflects to the interpolated value.
//    //    /// </returns>
//    //    /// <remarks>The points are given clockwise starting in the left upper corner. The points (x_i, y_i) and values z_i are
//    //    /// connected as follows:
//    //    /// <para>
//    //    /// (x1,y1,z1), (x2,y1,z2), (x2,y2,z3), (x1,y2,z4).
//    //    /// </para>
//    //    /// Only the null-based indices of x1 and y1 are given. One plus these values corresponds to x2
//    //    /// respectively y2.
//    //    /// </remarks>
//    //    /// <exception cref="InvalidOperationException">Will be thrown because this type of interpolation is not allowed for cap volatiliites.</exception>
//    //    public double GetValue(double x, double y, int rowIndex, int columnIndex)
//    //    {
//    //        throw new InvalidOperationException("This type of interpolation is not allowed for cap volatilites.");
//    //    }

//    //    /// <summary>Gets an interpolated value of some <see cref="TransformableVicinityMatrix&lt;HorizontalLabelType, VerticalLabelType&gt;"/> instance
//    //    /// doing some interpolation on the horizontal direction only.
//    //    /// </summary>
//    //    /// <param name="x">The x coordinate of point to interpolate.</param>
//    //    /// <param name="rowIndex">The null-based row index.</param>
//    //    /// <param name="columnIndex">The null-based column index of the left grid point (add <c>1</c> for the right grid point).</param>
//    //    /// <param name="z1">The value of the left grid point.</param>
//    //    /// <param name="z2">The value of the right grid point.</param>
//    //    /// <returns>
//    //    /// A <see cref="System.Double"/> which reflects to the interpolated value.
//    //    /// </returns>
//    //    /// <remarks>
//    //    /// Two grid points (x1,y,z1), (x2,y,z2) are given and it is assumed that these points have the same y-coordinate.
//    //    /// This method does a interpolation of some point (x,y) where x is between x1 and x2. Furthermore z1,z2 are given here to allow
//    //    /// some transformation before applying the interpolation rule.
//    //    /// </remarks>
//    //    /// <exception cref="InvalidOperationException">Will be thrown because this type of interpolation is not allowed for cap volatiliites.</exception>
//    //    public double GetHorizontalValue(double x, int rowIndex, int columnIndex, double z1, double z2)
//    //    {
//    //        throw new InvalidOperationException("This type of interpolation is not allowed for cap volatilites.");
//    //    }

//    //    /// <summary>Gets an interpolated value of some <see cref="VicinityMatrix&lt;HorizontalLabelType, VerticalLabelType&gt;"/> instance
//    //    /// doing some interpolation on the horizontal direction only.
//    //    /// </summary>
//    //    /// <param name="x">The x coordinate of point to interpolate.</param>
//    //    /// <param name="rowIndex">The null-based row index.</param>
//    //    /// <param name="columnIndex">The null-based column index of the left grid point (add <c>1</c> for the right grid point).</param>
//    //    /// <returns>
//    //    /// A <see cref="System.Double"/> which reflects to the interpolated value.
//    //    /// </returns>
//    //    /// <remarks>
//    //    /// Two grid points (x1,y,z1), (x2,y,z2) are given and it is assumed that these points have the same y-coordinate.
//    //    /// This method does a interpolation of some point (x,y) where x is between x1 and x2.
//    //    /// </remarks>
//    //    /// <exception cref="InvalidOperationException">Will be thrown because this type of interpolation is not allowed for cap volatiliites.</exception>
//    //    public double GetHorizontalValue(double x, int rowIndex, int columnIndex)
//    //    {
//    //        throw new InvalidOperationException("This type of interpolation is not allowed for cap volatilites.");
//    //    }

//    //    /// <summary>Gets an interpolated value of some <see cref="TransformableVicinityMatrix&lt;HorizontalLabelType, VerticalLabelType&gt;"/> instance
//    //    /// doing some interpolation on the vertical direction only.
//    //    /// </summary>
//    //    /// <param name="y">The y coordinate of point to interpolate.</param>
//    //    /// <param name="rowIndex">The null-based row index of the upper grid point (add <c>1</c> for the lower grid point).</param>
//    //    /// <param name="columnIndex">The null-based column index.</param>
//    //    /// <param name="z1">The value of the lower grid point.</param>
//    //    /// <param name="z2">The value of the upper grid point.</param>
//    //    /// <returns>
//    //    /// A <see cref="System.Double"/> which reflects to the interpolated value.
//    //    /// </returns>
//    //    /// <remarks>
//    //    /// Two grid points (x,y1,z1), (x,y2,z2) are given and it is assumed that these points have the same y-coordinate.
//    //    /// This method does a interpolation of some point (x,y) where y is between y1 and y2. Furthermore z1,z2 are given here to allow
//    //    /// some transformation before applying the interpolation rule.
//    //    /// </remarks>
//    //    /// <exception cref="InvalidOperationException">Will be thrown because this type of interpolation is not allowed for cap volatiliites.</exception>
//    //    public double GetVerticalValue(double y, int rowIndex, int columnIndex, double z1, double z2)
//    //    {
//    //        throw new InvalidOperationException("This type of interpolation is not allowed for cap volatilites.");
//    //    }
//}