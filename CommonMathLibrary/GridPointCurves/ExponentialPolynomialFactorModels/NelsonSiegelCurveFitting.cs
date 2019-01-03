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

//namespace Dodoni.MathLibrary.Curves
//{
//    /// <summary>Represents the curve fitting via the Nelson-Siegel parametrization.
//    /// The Nelson-Siegel parametrisation is given by 
//    ///  <para>
//    ///    \sigma(t) = \beta_0 + (\beta_1 + \beta_2 * t \alpha) * e^{-\alpha * t}
//    /// </para>
//    /// where \alpha &gt; 0.
//    /// Based on 
//    /// <para>
//    /// 'Estimating and interpreting forward interest rates: Sweden 1992-1994, Lars E. O. Svensson, Working Paper No. 4871'
//    /// </para>
//    /// and 
//    /// <para>
//    /// 'The LIBOR market model', Nevena Selic, School of Computational and spplied mathematics, University of the Witwatersrand, May 2006.
//    /// </para>
//    /// For fixed \alpha, a linear regression problem is given. Therefore a one-dimensional optimization problem (with respect to \alpha) is needed only.
//    /// </summary>
//    public class NelsonSiegelCurveFitting 
//    {
//        #region public static (readonly) members

//        /// <summary>The language independent name of the curve parametrization.
//        /// </summary>
//        public static readonly IdentifierString Name = new IdentifierString("Nelson-Siegel");
//        #endregion

//        #region private members

//        /// <summary>The calibrated parameter '\beta_0'.
//        /// </summary>
//        private double m_Beta0;

//        /// <summary>The calibrated parameter '\beta_1'.
//        /// </summary>
//        private double m_Beta1;

//        /// <summary>The calibrated parameter '\beta_2'.
//        /// </summary>
//        private double m_Beta2;

//        /// <summary>The calibrated parameter '\alpha'.
//        /// </summary>
//        private double m_Alpha;
//        #endregion

//        #region public constructors

//        /// <summary>Initializes a new instance of the <see cref="NelsonSiegelCurveFitting"/> class.
//        /// </summary>
//        public NelsonSiegelCurveFitting()
//        {
//        }
//        #endregion

//        #region protected constructors

//        /// <summary>Initializes a new instance of the <see cref="NelsonSiegelCurveFitting"/> class.
//        /// </summary>
//        /// <param name="nelsonSiegelCurveFitting">The Nelson-Siegel curve fitting.</param>
//        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="nelsonSiegelCurveFitting"/> is <c>null</c>.</exception>
//        /// <exception cref="ArgumentException">Thrown, if <paramref name="nelsonSiegelCurveFitting"/> is some shared object and <paramref name="copyType"/>
//        /// is equal to <see cref="eSetupParameterCopyType.SharedCopy"/>.</exception>
//        /// <remarks><see cref="ParametricCurve&lt;TCurveFittingSetting&gt;.IsSharedObject"/> will be set to <c>false</c> in the
//        /// case of some deep copy and to <c>true</c> in the case of some shallow copy.</remarks>
//        protected NelsonSiegelCurveFitting(NelsonSiegelCurveFitting nelsonSiegelCurveFitting, eSetupParameterCopyType copyType)
//            : base(nelsonSiegelCurveFitting, copyType)
//        {
//        }
//        #endregion

//        #region public properties

//        #region IOperable Members

//        /// <summary>Gets a value indicating whether this instance is operable.
//        /// </summary>
//        /// <value>
//        /// 	<c>true</c> if this instance is operable; otherwise, <c>false</c>.
//        /// </value>
//        /// <remarks>
//        /// 	<c>false</c> will be returned if the current instance represents some data, model, interpolation procedure,
//        /// integration approach, optimization procedure etc. and no valid parameters are given.
//        /// </remarks>
//        public bool IsOperable
//        {
//            get { return ((!m_GridPointChanged) && (!CurveFittingSettingChanged) && (!Double.IsNaN(m_Alpha)) && (!Double.IsNaN(m_Beta0)) && (!Double.IsNaN(m_Beta1)) && (!Double.IsNaN(m_Beta2))); }
//        }
//        #endregion

//        #region IPreparableForOperation Members

//        /// <summary>Gets a value indicating whether this instance is <c>prepared for operation</c>,
//        /// i.e. some set-up method has been called but perhaps the instance is not yet ready to use.
//        /// </summary>
//        /// <value>
//        /// 	<c>true</c> if this instance is prepared for operation; otherwise, <c>false</c>.
//        /// </value>
//        /// <remarks>Use <see cref="IOperable.IsOperable"/> to indicate whether the current instance
//        /// is ready to use. If <see cref="IPreparableForOperation.IsPreparedForOperation"/> is <c>false</c>,
//        /// then the current instance is not ready to use and <see cref="IOperable.IsOperable"/> is also <c>false</c>.
//        /// </remarks>
//        public bool IsPreparedForOperation
//        {
//            get { return ((!m_GridPointChanged) && (!CurveFittingSettingChanged) && (m_CurveFittingSetting.Optimizer.IsOperable) ); }
//        }
//        #endregion

//        #region IIdentifierNameable Members

//        /// <summary>Gets the name of the curve parametrization.
//        /// </summary>
//        /// <value>The language independent name of the curve parametrization.</value>
//        IdentifierString IIdentifierNameable.Name
//        {
//            get { return NelsonSiegelCurveFitting.Name; }
//        }

//        /// <summary>Gets the long name of the curve parametrization.
//        /// </summary>
//        /// <value>The language dependent long name of the curve parametrization.</value>
//        IdentifierString IIdentifierNameable.LongName
//        {
//            get { return ToString().ToIdentifierString(); }
//        }
//        #endregion


//        #endregion

//        #region public methods

//        #region ICurveFitting Members

//        /// <summary>Gets some value for a specified point.
//        /// </summary>
//        /// <param name="pointToEvaluate">The point to evaluate.</param>
//        /// <param name="nonLastLeftGridIndex">The null-based index of the left neighbor grid point or the null-based index of the
//        /// last but one grid point if <paramref name="pointToEvaluate"/> is equal to the last grid point.</param>
//        /// <returns>
//        /// The value of the curve at <paramref name="pointToEvaluate"/>, perhaps interpolated or taken into account some parametrization.
//        /// </returns>
//        double ICurveFitting.GetValue(double pointToEvaluate, int nonLastLeftGridIndex)
//        {
//            return m_Beta0 + (m_Beta1 + m_Beta2 * pointToEvaluate) * Math.Exp(-m_Alpha * pointToEvaluate);
//        }


//        /// <summary>Reinitialize the current instance.
//        /// </summary>
//        /// <param name="doubleLabels">The labels in some <see cref="System.Double"/> representation.</param>
//        /// <param name="values">The values with respect to the <paramref name="doubleLabels"/>.</param>
//        /// <param name="gridPointCount">The number of grid points, i.e. the number of relevant elements of <paramref name="doubleLabels"/> and <paramref name="values"/>.</param>
//        /// <param name="state">The state of the grid points, i.e. <paramref name="doubleLabels"/> and <paramref name="values"/>, with respect to the previous function call.</param>
//        /// <remarks>This method will be called before starting some interpolation/extrapolation and will be called again if the underlying grid points
//        /// changed or some grid point has been added.</remarks>
//        void ICurveFitting.ReInitialize(double[] doubleLabels, double[] values, int gridPointCount, ShiftableGridPointCurveState state)
//        {
//            ReInitialize(doubleLabels, values, gridPointCount, state);
//        }


//        #endregion

//        #region IRealValuedCurve Members

//        /// <summary>Gets some value for a specified point.
//        /// </summary>
//        /// <param name="pointToEvaluate">The point to evaluate.</param>
//        /// <returns>
//        /// The value of the curve at <paramref name="pointToEvaluate"/>, perhaps interpolated/extrapolated or taken into account some parametrization etc.
//        /// </returns>
//        /// <remarks>The argument must be an element of the domain of definition,
//        /// given by <see cref="IRealValuedCurve.LowerBound"/> and <see cref="IRealValuedCurve.UpperBound"/>.</remarks>
//        public double GetValue(double pointToEvaluate)
//        {
//            return m_Beta0 + (m_Beta1 + m_Beta2 * pointToEvaluate) * Math.Exp(-m_Alpha * pointToEvaluate);
//        }

//        public double GetIntegral(double lowerBound, double upperBound)
//        {
//            throw new NotImplementedException();
//        }
//        #endregion

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
//            return new NelsonSiegelCurveFitting(this, eSetupParameterCopyType.DeepCopy);
//        }
//        #endregion

//        #region IDifferentiableRealValuedCurve Members

//        public double GetDerivative(double pointToEvaluate)
//        {
//            throw new NotImplementedException();
//        }
//        #endregion

//        public void Setup(IOneDimOptimizerAlgorithm optimizerForParameterAlpha)
//        {
//        }

//        /// <summary>Gets the svensson parameter.
//        /// </summary>
//        /// <param name="tau1">The parameter tau_1 (output).</param>
//        /// <param name="tau2">The parameter tau_2 (output).</param>
//        /// <param name="beta0">The parameter beta_0 (output).</param>
//        /// <param name="beta1">The parameter beta_1 (output).</param>
//        /// <param name="beta2">The parameter beta_2 (output).</param>
//        /// <param name="beta3">The parameter beta_3 (output).</param>
//        /// <returns>A value indicating whether the given parameter contains some values; otherwise the parameter remain unchanged (for example if
//        /// no optimization has been yet execute).</returns>
//        public bool TryGetSvenssonParameters(out double alpha, out double beta0, out double beta1, out double beta2)
//        {
//            throw new NotImplementedException();
//            //if ((m_Optimizer == null) || (m_Optimizer.Status == eResultStatus.NoResult))
//            //{
//            //    return false;
//            //}
//            //tau1 = m_Tau1;
//            //tau2 = 0.0;
//            //beta0 = m_ParameterEstimator.GetValue(0);
//            //beta1 = m_ParameterEstimator.GetValue(1);
//            //beta2 = m_ParameterEstimator.GetValue(2);
//            //beta3 = 0.0;
//            //return true;
//        }
//        #endregion

//        #region private methods

//        private void ReInitialize(double[] doubleLabels, double[] values, int gridPointCount, ShiftableGridPointCurveState state)
//        {
//        }


//        //    /// <summary>The deviation function for the calibration to a interest curve where the discount factors for a given
//        //    /// set of dates are used.
//        //    /// </summary>
//        //    /// <param name="tau1">The parameter tau_1.</param>
//        //    /// <returns>The euclidian norm of the error squares, where the differences of the given spot rates <see cref="m_BenchmarkSpotRates"/>
//        //    /// and the spot rates coming from the Svensson parametrization.
//        //    /// </returns>
//        //    private double DeviationFunction(double tau1)
//        //    {
//        //        /* build the design matrix:
//        //         *     [ 1   (1-exp(-T_1/tau[0]))*tau[0]/T_1  ...
//        //         * A = | 1    ...
//        //         *     [ 1   (1-exp(-T_n/tau[0]))*tau[0]/T_n  ...
//        //         *     
//        //         *  etc., vgl. formula (3.3) 'Estimating and interpreting forward rates', L. E. Svensson * but without beta_3 and tau_2
//        //         */

//        //        double expTerm1;
//        //        for (int j = 0; j < m_BenchmarkGridPointTimeComponents.Count; j++)
//        //        {
//        //            expTerm1 = Math.Exp(-m_BenchmarkGridPointTimeComponents[j] / tau1);

//        //            m_DesignMatrix[j, 1] = (1.0 - expTerm1) * tau1 / m_BenchmarkGridPointTimeComponents[j];
//        //            m_DesignMatrix[j, 2] = m_DesignMatrix.GetValue(j, 1) - expTerm1;
//        //        }

//        //        m_SingularValues = ILAlgorithm.svd(m_DesignMatrix, ref m_U, ref m_V); // A = U * \Sigma * V, where \Sigma = \diag(singularValues)

//        //        /* the estimation of the parameter \beta, i.e. 'y = A \beta + \epsilon' is given by
//        //         * 
//        //         * \beta = \sum_{i=1}^3 \frac{u_i^T y}{\sigma_i} * v_i,
//        //         * 
//        //         * where U=(u_1,...,u_n), V = (v_1,..,v3) */

//        //        m_ParameterEstimator.SetValue(0.0, 0, 0);
//        //        m_ParameterEstimator.SetValue(0.0, 1, 0);
//        //        m_ParameterEstimator.SetValue(0.0, 2, 0);

//        //        for (int i = 0; i < 3; i++)
//        //        {
//        //            double singularValue = m_SingularValues.GetValue(i, i);
//        //            if (singularValue > MachineConsts.TinyEpsilon)
//        //            {
//        //                m_ParameterEstimator += (ILAlgorithm.multiply(m_U[null, i].T, m_BenchmarkSpotRates) / singularValue) * m_V[null, i];
//        //            }
//        //        }
//        //        return ILAlgorithm.norm(ILAlgorithm.multiply(m_DesignMatrix, m_ParameterEstimator) - m_BenchmarkSpotRates, 2).GetValue(0);
//        //    }
//        #endregion
//    }
//}