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
using System.Security;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

using Dodoni.BasicComponents;
using Dodoni.MathLibrary.Basics;
using Dodoni.BasicComponents.Utilities;
using Dodoni.BasicComponents.Containers;
using Dodoni.MathLibrary.GridPointCurves;
using Dodoni.MathLibrary.Basics.LowLevel.Native;

namespace Dodoni.MathLibrary.Native.GridPointCurves
{
    /// <summary>Serves as wrapper for the Data fitting of Intel's Math Kernel Library (MKL).
    /// </summary>
    internal class MklDataFitting : MklGridPointCurve.Interpolator
    {
        #region nested enumerations

        /// <summary>The parameter type of a pointer going to change.
        /// </summary>
        internal enum PtrParameterChangeType
        {
            /// <summary>Partition x of the interpolation interval
            /// </summary>
            DF_X = 1,

            /// <summary>Vector-valued function y
            /// </summary>
            DF_Y = 2,

            /// <summary>Internal conditions for spline construction. For details, see table "Internal Conditions Supported by Data Fitting Functions". 
            /// </summary>
            DF_IC = 3,

            /// <summary>Boundary conditions for spline construction. For details, see table "Boundary Conditions Supported by Data Fitting Functions". 
            /// </summary>
            DF_BC = 4,

            /// <summary>Spline coefficients
            /// </summary>
            DF_PP_SCOEFF = 5
        }

        /// <summary>The parameter type of an integer going to change.
        /// </summary>
        internal enum IntParameterChangeType
        {
            /// <summary>Number of breakpoints (grid points).
            /// </summary>
            DF_NX = 14,

            /// <summary>A flag describing the structure of partition. See table "Hint Values for Partition x" for the list of available values.
            /// </summary>
            DF_XHINT = 15,

            /// <summary>Dimension of the vector-valued function
            /// </summary>
            DF_NY = 16,

            /// <summary>A flag describing the structure of the vector-valued function. See table "Hint Values for Vector Function y" for the list of available values.
            /// </summary>
            DF_YHINT = 17,

            /// <summary>Spline order. See table "Spline Orders Supported by Data Fitting Functions" for the list of available values.
            /// </summary>
            DF_SPLINE_ORDER = 18,

            /// <summary>Spline type. See table "Spline Types Supported by Data Fitting Functions" for the list of available values.
            /// </summary>
            DF_SPLINE_TYPE = 19,

            /// <summary>Type of boundary conditions used in spline construction. See table "Boundary Conditions Supported by Data Fitting Functions" for the list of available values.
            /// </summary>
            DF_BC_TYPE = 21,

            /// <summary>Type of internal conditions used in spline construction. See table "Internal Conditions Supported by Data Fitting Functions" for the list of available values.
            /// </summary>
            DF_IC_TYPE = 20,

            /// <summary>A flag describing the structure of spline coefficients. See table "Hint Values for Spline Coefficients" for the list of available values.
            /// </summary>
            DF_PP_COEFF_HINT = 22,

            /// <summary>A flag which controls checking of Data Fitting parameters. See table "Possible Values for the DF_CHECK_FLAG Parameter" for the list of available values.
            /// </summary>
            DF_CHECK_FLAG = 23,
        }
        #endregion

        #region nested classes

        /// <summary>Serves as implementation of the interpolation approach.
        /// </summary>
        internal class Interpolator : IMklCurveDataFitting
        {
            #region protected function import

            [UnmanagedFunctionPointer(MklNativeWrapper.callingConvention)]
            protected delegate int dfdIntegralCallBack(ref Int64 n, Int64[] lcell, double[] llim, Int64[] rcell, double[] rlim, [In, Out] double[] r, IntPtr param);


            [DllImport(MklNativeWrapper.dllName, EntryPoint = "dfdNewTask1D", ExactSpelling = true, CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            protected extern static int _dfdNewTask1D(out IntPtr task, int nx, double[] x, MklGridPointCurve.xHintValue xhint, int ny, double[] y, MklGridPointCurve.yHintValue yhint);

            [DllImport(MklNativeWrapper.dllName, EntryPoint = "dfdEditPPSpline1D", ExactSpelling = true, CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            protected extern static int _dfdEditPPSpline1D(IntPtr task, MklCurveInterpolationSpline.SplineOrder splineOrder, MklCurveInterpolationSpline.SplineType splineType, MklCurveInterpolationSpline.SplineBoundaryCondition boundaryConditionType, double[] bc, MklCurveInterpolationSpline.SplineInternalConditionType ic_type, double[] ic, double[] splineCoefficients, MklCurveInterpolationSpline.SplineCoefficientStorageFormat scoeffhint);

            [DllImport(MklNativeWrapper.dllName, EntryPoint = "dfdConstruct1D", ExactSpelling = true, CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            protected extern static int _dfdConstruct1D(IntPtr task, MklCurveInterpolationSpline.SplineFormat splineFormat, MklCurveInterpolationSpline.SplineConstructionMethod constructionMethod);

            [DllImport(MklNativeWrapper.dllName, EntryPoint = "dfdInterpolate1D", ExactSpelling = true, CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            protected extern static int _dfdInterpolate1D(IntPtr task, MklGridPointCurve.EstimationType type, MklGridPointCurve.ComputationMethod computationMethod, int nsite, double[] site, MklGridPointCurve.SiteHint sitehint, int ndorder, int[] dorder, double[] datahint, [In, Out] double[] r, MklGridPointCurve.ResultHint rhint, [In, Out] int[] cell);

            [DllImport(MklNativeWrapper.dllName, EntryPoint = "dfdInterpolate1D", ExactSpelling = true, CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            protected unsafe extern static int _dfdInterpolate1D(IntPtr task, MklGridPointCurve.EstimationType type, MklGridPointCurve.ComputationMethod computationMethod, int nsite, double* site, MklGridPointCurve.SiteHint sitehint, int ndorder, int* dorder, double[] datahint, [In, Out] double* r, MklGridPointCurve.ResultHint rhint, [In, Out] int[] cell);

            [DllImport(MklNativeWrapper.dllName, EntryPoint = "dfdIntegrate1D", ExactSpelling = true, CallingConvention = MklNativeWrapper.callingConvention)]
            protected extern static int _dfdIntegrate1D(IntPtr task, MklGridPointCurve.ComputationMethod method, int nlim, double[] llim, MklGridPointCurve.IntegrationLimitHint llimhint, double[] rlim, MklGridPointCurve.IntegrationLimitHint rlimhint, double[] ldatahint, double[] rdatahint, double[] r, MklGridPointCurve.IntegralResultHint rhint);

            [DllImport(MklNativeWrapper.dllName, EntryPoint = "dfdIntegrate1D", ExactSpelling = true, CallingConvention = MklNativeWrapper.callingConvention)]
            protected unsafe extern static int _dfdIntegrate1D(IntPtr task, MklGridPointCurve.ComputationMethod method, int nlim, double* llim, MklGridPointCurve.IntegrationLimitHint llimhint, double* rlim, MklGridPointCurve.IntegrationLimitHint rlimhint, double[] ldatahint, double[] rdatahint, double* r, MklGridPointCurve.IntegralResultHint rhint);

            [DllImport(MklNativeWrapper.dllName, EntryPoint = "dfdIntegrateEx1D", ExactSpelling = true, CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            protected extern static int _dfdIntegrateEx1D(IntPtr task, MklGridPointCurve.ComputationMethod method, int nlim, double[] llim, MklGridPointCurve.IntegrationLimitHint llimhint, double[] rlim, MklGridPointCurve.IntegrationLimitHint rlimhint, double[] ldatahint, double[] rdatahint, double[] r, MklGridPointCurve.IntegralResultHint rhint, dfdIntegralCallBack le_cb, IntPtr le_params, dfdIntegralCallBack re_cb, IntPtr re_params, dfdIntegralCallBack i_cb, IntPtr i_params, dfdIntegralCallBack search_cb, IntPtr search_params);

            [DllImport(MklNativeWrapper.dllName, EntryPoint = "dfdIntegrateEx1D", ExactSpelling = true, CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            protected unsafe extern static int _dfdIntegrateEx1D(IntPtr task, MklGridPointCurve.ComputationMethod method, int nlim, double* llim, MklGridPointCurve.IntegrationLimitHint llimhint, double* rlim, MklGridPointCurve.IntegrationLimitHint rlimhint, double[] ldatahint, double[] rdatahint, double* r, MklGridPointCurve.IntegralResultHint rhint, dfdIntegralCallBack le_cb, IntPtr le_params, dfdIntegralCallBack re_cb, IntPtr re_params, dfdIntegralCallBack i_cb, IntPtr i_params, dfdIntegralCallBack search_cb, IntPtr search_params);

            [DllImport(MklNativeWrapper.dllName, EntryPoint = "dfdEditPtr", ExactSpelling = true, CallingConvention = MklNativeWrapper.callingConvention)]
            protected extern static int _dfdEditPtr(IntPtr task, PtrParameterChangeType type, double[] newPointer);

            [DllImport(MklNativeWrapper.dllName, EntryPoint = "dfiEditVal", ExactSpelling = true, CallingConvention = MklNativeWrapper.callingConvention)]
            protected extern static int _dfiEditVal(IntPtr task, IntParameterChangeType type, int value);

            [DllImport(MklNativeWrapper.dllName, EntryPoint = "dfDeleteTask", ExactSpelling = true, CallingConvention = MklNativeWrapper.callingConvention), SuppressUnmanagedCodeSecurity]
            protected extern static int _dfDeleteTask(ref IntPtr task);
            #endregion

            #region private members

            /// <summary>The MKL data fitting task.
            /// </summary>
            private IntPtr m_Task;

            /// <summary>The factory of <see cref="IMklCurveDataFitting" /> objects of the same type and configuration.
            /// </summary>
            private MklDataFitting m_MklDataFitting;

            /// <summary>The number of grid points.
            /// </summary>
            private int m_GridPointCount;

            /// <summary>The arguments of the grid points, i.e. the labels on the x-axis.
            /// </summary>
            private double[] m_GridPointArguments;

            /// <summary>The read-only wrapper of the grid points, i.e. the labels of the x-axis.
            /// </summary>
            /// <remarks>This member is used for performance reason only.</remarks>
            private ReadOnlyCollection<double> m_ReadOnlyGridPointArguments;

            /// <summary>The values of the grid points.
            /// </summary>
            private double[] m_GridPointValues;

            /// <summary>The read-only wrapper of grid point values.
            /// </summary>
            /// <remarks>This member is used for performance reason only.</remarks>
            private ReadOnlyCollection<double> m_ReadOnlyGridPointValues;

            /// <summary>The spline coefficients.
            /// </summary>
            private double[] m_SplineCoefficients = null;
            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="Interpolator"/> class.
            /// </summary>
            /// <param name="interpolationFactory">The <see cref="MklDataFitting"/> object that serves as factory for the current object.</param>
            internal Interpolator(MklDataFitting interpolationFactory)
            {
                m_Task = IntPtr.Zero;
                m_GridPointCount = 0;
                m_MklDataFitting = interpolationFactory;
            }

            /// <summary>Finalizes an instance of the <see cref="Interpolator" /> class.
            /// </summary>
            ~Interpolator()
            {
                _dfDeleteTask(ref m_Task);
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

            #region ICurveDataFitting Members

            /// <summary>Gets the number of grid points.
            /// </summary>
            /// <value>The number of grid points.</value>
            public int GridPointCount
            {
                get { return m_GridPointCount; }
            }

            /// <summary>Gets the grid point arguments, i.e. the labels (on the x-axis) of the curve in its <see cref="System.Double"/> representation.
            /// </summary>
            /// <value>The grid point arguments.</value>
            public IList<double> GridPointArguments
            {
                get { return m_ReadOnlyGridPointArguments; }
            }

            /// <summary>Gets the grid point values with respect to <see cref="ICurveDataFitting.GridPointArguments"/>.
            /// </summary>
            /// <value>The grid point values.</value>
            public IList<double> GridPointValues
            {
                get { return m_ReadOnlyGridPointValues; }
            }

            /// <summary>Gets the factory of <see cref="ICurveDataFitting" /> objects of the same type and configuration.
            /// </summary>
            /// <value>The factory of <see cref="ICurveDataFitting" /> objects of the same type and configuration.</value>
            public ICurveDataFittingFactory Factory
            {
                get { return m_MklDataFitting; }
            }
            #endregion

            #region IRealValuedCurve Members

            /// <summary>Gets the lower bound of the domain of definition.
            /// </summary>
            /// <value>The lower bound of the domain of definition, perhaps <see cref="System.Double.NegativeInfinity"/> or <see cref="System.Double.NaN"/>.</value>
            public double LowerBound
            {
                get { return m_GridPointArguments[0]; }
            }

            /// <summary>Gets the upper bound of the domain of definition.
            /// </summary>
            /// <value>The upper bound of the domain of definition, perhaps <see cref="System.Double.PositiveInfinity"/> or <see cref="System.Double.NaN"/>.</value>
            public double UpperBound
            {
                get { return m_GridPointArguments[m_GridPointCount - 1]; }
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

            #region protected properties

            /// <summary>Gets the MKL data fitting task in its <see cref="System.IntPtr"/> representation.
            /// </summary>
            /// <value>The MKL data fitting task.</value>
            protected IntPtr Task
            {
                get { return m_Task; }
            }
            #endregion

            #region public methods

            #region ICurveDataFitting Members

            /// <summary>Updates the current curve interpolator.
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
            /// </remarks>
            public void Update(int gridPointCount, IList<double> gridPointArguments, IList<double> gridPointValues, GridPointCurve.State state, int gridPointArgumentStartIndex = 0, int gridPointValueStartIndex = 0, int gridPointArgumentIncrement = 1, int gridPointValueIncrement = 1)
            {
                Update(gridPointCount, gridPointArguments, gridPointValues, MklGridPointCurve.xHintValue.DF_NO_HINT, MklGridPointCurve.yHintValue.DF_NO_HINT, state, gridPointArgumentStartIndex, gridPointValueStartIndex, gridPointArgumentIncrement, gridPointValueIncrement);
            }
            #endregion

            #region IRealValuedCurve Members

            /// <summary>Gets the value at a specific argument.
            /// </summary>
            /// <param name="pointToEvaluate">The point to evaluate.</param>
            /// <returns>The value of the curve at <paramref name="pointToEvaluate"/></returns>
            /// <remarks>The argument must be an element of the domain of definition, represented by <see cref="IRealValuedCurve.LowerBound"/> and <see cref="IRealValuedCurve.UpperBound"/>.</remarks>
            public double GetValue(double pointToEvaluate)
            {
                unsafe
                {
                    var site = stackalloc double[1];
                    site[0] = pointToEvaluate;

                    var dorder = stackalloc int[1];
                    dorder[0] = 1;

                    var result = stackalloc double[1];
                    CheckErrorCode(_dfdInterpolate1D(m_Task, MklGridPointCurve.EstimationType.DF_INTERP, MklGridPointCurve.ComputationMethod.DF_METHOD_PP, 1, site, MklGridPointCurve.SiteHint.DF_NO_HINT, 1, dorder, null, result, MklGridPointCurve.ResultHint.DF_NO_HINT, null), "dfdInterpolate1D");
                    return result[0];
                }
            }

            /// <summary>Gets the value of the integral \int_a^b f(x) dx.
            /// </summary>
            /// <param name="lowerBound">The lower bound.</param>
            /// <param name="upperBound">The upper bound.</param>
            /// <returns>The value of \int_a^b f(x) dx.</returns>
            /// <remarks>The arguments must be elements of the domain of definition, represented by <see cref="IRealValuedCurve.LowerBound"/> and <see cref="IRealValuedCurve.UpperBound"/>.</remarks>
            public double GetIntegral(double lowerBound, double upperBound)
            {
                unsafe
                {
                    double* lowerLimits = stackalloc double[1];
                    lowerLimits[0] = lowerBound;

                    double* upperLimits = stackalloc double[1];
                    upperLimits[0] = upperBound;

                    double* result = stackalloc double[1];
                    CheckErrorCode(_dfdIntegrate1D(m_Task, MklGridPointCurve.ComputationMethod.DF_METHOD_PP, 1, lowerLimits, MklGridPointCurve.IntegrationLimitHint.DF_NO_HINT, upperLimits, MklGridPointCurve.IntegrationLimitHint.DF_NO_HINT, null, null, result, MklGridPointCurve.IntegralResultHint.DF_NO_HINT), "dfdInterpolate1D");
                    return result[0];
                }
            }
            #endregion

            #region ICurveDataFitting Members

            /// <summary>Gets the value of the integral \int_a^b f(x) dx inside two specific grid points.
            /// </summary>
            /// <param name="lowerBound">The lower bound; between the grid point arguments specified by <paramref name="leftGridPointIndex" /> and <paramref name="leftGridPointIndex" /> + 1.</param>
            /// <param name="upperBound">The upper bound; between the grid point arguments specified by <paramref name="leftGridPointIndex" /> and <paramref name="leftGridPointIndex" /> + 1.</param>
            /// <param name="leftGridPointIndex">The null-based index of the left grid point index.</param>
            /// <returns>The value of \int_a^b f(x) dx.</returns>
            public double GetIntegral(double lowerBound, double upperBound, int leftGridPointIndex)
            {
                return GetIntegral(lowerBound, upperBound);
            }
            #endregion

            #region IMklCurveDataFitting Members

            /// <summary>Updates the current curve fitting object.
            /// </summary>
            /// <param name="gridPointCount">The number of grid points, i.e. the number of relevant elements of <paramref name="gridPointArguments" /> and <paramref name="gridPointValues" /> to take into account.</param>
            /// <param name="gridPointArguments">The arguments of the grid points, thus labels of the curve in its <see cref="System.Double" /> representation in ascending order.</param>
            /// <param name="gridPointValues">The values of the grid points corresponding to <paramref name="gridPointArguments" />.</param>
            /// <param name="gridPointArgumentHint">Describes the structure of the grid point arguments.</param>
            /// <param name="gridPointValueHint">Describes the structure of the grid point values.</param>
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
            public void Update(int gridPointCount, IList<double> gridPointArguments, IList<double> gridPointValues, MklGridPointCurve.xHintValue gridPointArgumentHint, MklGridPointCurve.yHintValue gridPointValueHint, GridPointCurve.State state = GridPointCurve.State.GridPointChanged, int gridPointArgumentStartIndex = 0, int gridPointValueStartIndex = 0, int gridPointArgumentIncrement = 1, int gridPointValueIncrement = 1)
            {
                Update(gridPointCount, gridPointArguments, gridPointValues, gridPointArgumentHint, gridPointValueHint, (n, v) => { }, state, gridPointArgumentStartIndex, gridPointValueStartIndex, gridPointArgumentIncrement, gridPointValueIncrement);
            }

            /// <summary>Runs data fitting computations.
            /// </summary>
            /// <param name="n">The Number of interpolation sites.</param>
            /// <param name="site">Array of interpolation sites of size <paramref name="n" />. The structure of the array is defined by the sitehint parameter:
            /// <list type=" ´bullet">
            /// <item><description>If sites form a non-uniform partition, the array should contain nsite values.</description></item>
            /// <item><description>If sites form a uniform partition, the array should contain two entries that represent the left and the right interpolation sites. The first entry
            /// of the array contains the left-most interpolation point. The second entry of the array contains the right-most interpolation point.</description></item>
            /// </list></param>
            /// <param name="result">Array that contains results of computations at the interpolation sites (output). If you do not need spline-based interpolation or integration, set this pointer to NULL.</param>
            /// <param name="ndorder">Maximal derivative order increased by one to be computed at interpolation sites.</param>
            /// <param name="dorder">Array of size <paramref name="ndorder" /> that defines the order of the derivatives to be computed at the interpolation sites. If all the elements in are zero, the library computes the spline values only. If you do not need interpolation computations, set <paramref name="ndorder" /> to zero and pass a <c>null</c> pointer.</param>
            /// <param name="siteHint">A flag describing the structure of the interpolation sites.</param>
            /// <param name="resultHint">A flag describing the structure of the results.</param>
            /// <param name="estimationType">Type of spline-based computations.</param>
            /// <param name="cell">Array of cell indices in partition x that contain the interpolation sites. If you do not need cell indices, set this parameter to <c>null</c>.</param>           
            /// <param name="dataHint">Array that contains additional information about the structure of partition x and interpolation sites. This data helps to speed up the computation. If you provide a <c>null</c> pointer, the routine uses the default settings for computations. For details on the datahint array, see table "Structure of the datahint Array" in Intel Math Kernel Library Reference Manual.</param>
            /// <remarks>The argument must be an element of the domain of definition, represented by <see cref="IRealValuedCurve.LowerBound"/> and <see cref="IRealValuedCurve.UpperBound"/>.</remarks>
            public void GetValues(int n, double[] site, double[] result, int ndorder, int[] dorder, MklGridPointCurve.SiteHint siteHint = MklGridPointCurve.SiteHint.DF_NO_HINT, MklGridPointCurve.ResultHint resultHint = MklGridPointCurve.ResultHint.DF_NO_HINT, MklGridPointCurve.EstimationType estimationType = MklGridPointCurve.EstimationType.DF_INTERP, int[] cell = null, double[] dataHint = null)
            {
                CheckErrorCode(_dfdInterpolate1D(m_Task, estimationType, MklGridPointCurve.ComputationMethod.DF_METHOD_PP, n, site, siteHint, ndorder, dorder, dataHint, result, resultHint, cell), "dfdInterpolate1D");
            }

            /// <summary>Gets the value of the integral \int_a^b f(x) dx.
            /// </summary>
            /// <param name="n">Number of pairs of integraion limits.</param>
            /// <param name="lowerBounds">The lower bounds.</param>
            /// <param name="upperBounds">The upper bounds.</param>
            /// <param name="result">Array of integration results (output). The size of the array should be sufficient to hold <paramref name="n"/>* ny values, where ny is the dimension of the vector-valued function. The integration results are packed according to the settings in <paramref name="resultHint"/>.</param>
            /// <param name="lowerBoundHint">A flag describing the structure of the left-side integration limits.</param>
            /// <param name="upperBoundHint">A flag describing the structure of the right-side integration limits.</param>
            /// <param name="resultHint">A flag describing the structure of the results.</param>
            /// <param name="ldataHint">Array that contains additional information about the structure of partition x and left-side integration limits. For details see table "Structure of the datahint Array" in the description of the df?Intepolate1D function in Intel Math Kernel Library Reference Manual.</param>
            /// <param name="rdataHint">Array that contains additional information about the structure of partition x and right-side integration limits. For details see table "Structure of the datahint Array" in the description of the df?Intepolate1D function in Intel Math Kernel Library Reference Manual.</param>
            /// <remarks>The arguments must be elements of the domain of definition, represented by <see cref="IRealValuedCurve.LowerBound"/> and <see cref="IRealValuedCurve.UpperBound"/>.</remarks>
            public void GetIntegral(int n, double[] lowerBounds, double[] upperBounds, double[] result, MklGridPointCurve.IntegrationLimitHint lowerBoundHint = MklGridPointCurve.IntegrationLimitHint.DF_NO_HINT, MklGridPointCurve.IntegrationLimitHint upperBoundHint = MklGridPointCurve.IntegrationLimitHint.DF_NO_HINT, MklGridPointCurve.IntegralResultHint resultHint = MklGridPointCurve.IntegralResultHint.DF_NO_HINT, double[] ldataHint = null, double[] rdataHint = null)
            {
                CheckErrorCode(_dfdIntegrate1D(m_Task, MklGridPointCurve.ComputationMethod.DF_METHOD_PP, n, lowerBounds, lowerBoundHint, upperBounds, upperBoundHint, ldataHint, rdataHint, result, resultHint), "dfdInterpolate1D");
            }
            #endregion

            #region IInfoOutputQueriable Members

            /// <summary>Gets informations of the current object as a specific <see cref="T:Dodoni.BasicComponents.Containers.InfoOutput"/> instance.
            /// </summary>
            /// <param name="infoOutput">The <see cref="T:Dodoni.BasicComponents.Containers.InfoOutput"/> object which is to be filled with informations concering the current instance.</param>
            /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
            public void FillInfoOutput(InfoOutput infoOutput, string categoryName = "General")
            {
                var infoOutputPackage = infoOutput.AcquirePackage(categoryName);

                infoOutputPackage.Add("Name", Factory.Name.String);
                infoOutputPackage.Add("Long Name", Factory.LongName.String);
                infoOutputPackage.Add("Fitting Quality", Factory.FittingQuality);
                infoOutputPackage.Add("Is Local approach", Factory.IsLocalApproach);
                infoOutputPackage.Add("Minimal required number of grid points", Factory.MinimalRequiredNumberOfGridPoints);
                infoOutputPackage.Add("Spline type", m_MklDataFitting.m_SplineType);
                infoOutputPackage.Add("Spline order", m_MklDataFitting.m_SplineOrder);
                infoOutputPackage.Add("Boundary condition type", m_MklDataFitting.m_BoundaryConditionType);
                infoOutputPackage.Add("Internal condition type", m_MklDataFitting.m_InternalConditionTypes);
                infoOutputPackage.Add("Spline coefficient hint", m_MklDataFitting.m_SplineCoefficientHint);

                infoOutputPackage.Add("Count", m_GridPointCount);

                DataTable gridPointTable = new DataTable("Grid points");
                gridPointTable.Columns.Add("Argument", typeof(double));
                gridPointTable.Columns.Add("Value", typeof(double));

                for (int j = 0; j < m_GridPointCount; j++)
                {
                    var row = gridPointTable.NewRow();
                    row[0] = m_GridPointArguments[j];
                    row[1] = m_GridPointValues[j];  // perhaps transformed in the case of log-linear interpolation
                    gridPointTable.Rows.Add(row);
                }
                infoOutputPackage.Add(gridPointTable);
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

            /// <summary>Updates the current curve fitting object.
            /// </summary>
            /// <param name="gridPointCount">The number of grid points, i.e. the number of relevant elements of <paramref name="gridPointArguments" /> and <paramref name="gridPointValues" /> to take into account.</param>
            /// <param name="gridPointArguments">The arguments of the grid points, thus labels of the curve in its <see cref="System.Double" /> representation in ascending order.</param>
            /// <param name="gridPointValues">The values of the grid points corresponding to <paramref name="gridPointArguments" />.</param>
            /// <param name="gridPointArgumentHint">Describes the structure of the grid point arguments.</param>
            /// <param name="gridPointValueHint">Describes the structure of the grid point values.</param>
            /// <param name="gridPointValueTransformation">A transformation to apply to the grid point values, i.e. number of grid points and grid point values (mainly used for log-linear interpolation). 
            /// Caution: The application of a transformation has an impact on <see cref="Interpolator.GridPointValues"/>.</param>
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
            public void Update(int gridPointCount, IList<double> gridPointArguments, IList<double> gridPointValues, MklGridPointCurve.xHintValue gridPointArgumentHint, MklGridPointCurve.yHintValue gridPointValueHint, Action<int, double[]> gridPointValueTransformation, GridPointCurve.State state = GridPointCurve.State.GridPointChanged, int gridPointArgumentStartIndex = 0, int gridPointValueStartIndex = 0, int gridPointArgumentIncrement = 1, int gridPointValueIncrement = 1)
            {
                if (gridPointCount <= 0)
                {
                    m_GridPointCount = 0;  // i.e. current instance is not operable
                }
                else
                {
                    m_GridPointCount = gridPointCount;
                    bool isInitializedBefore = m_Task != IntPtr.Zero;

                    if (state.HasFlag(GridPointCurve.State.GridPointArgumentChanged))
                    {
                        if (ArrayMemory.Reallocate(ref m_GridPointArguments, gridPointCount, Math.Max(10, gridPointCount / 5)) == true)
                        {
                            m_ReadOnlyGridPointArguments = new ReadOnlyCollection<double>(m_GridPointArguments);
                            if (isInitializedBefore == true)
                            {
                                CheckErrorCode(_dfdEditPtr(m_Task, PtrParameterChangeType.DF_X, m_GridPointArguments), "dfdEditPtr");
                            }
                        }
                        gridPointArguments.CopyTo(m_GridPointArguments, gridPointCount, gridPointArgumentStartIndex, sourceIncrement: gridPointArgumentIncrement);

                        if ((ArrayMemory.Reallocate(ref m_SplineCoefficients, (int)m_MklDataFitting.m_SplineOrder * gridPointCount, Math.Max(10, gridPointCount / 5)) == true) && (isInitializedBefore == true))
                        {
                            CheckErrorCode(_dfdEditPtr(m_Task, PtrParameterChangeType.DF_PP_SCOEFF, m_SplineCoefficients), "dfdEditPtr");
                        }
                        if (isInitializedBefore == true)
                        {
                            CheckErrorCode(_dfiEditVal(m_Task, IntParameterChangeType.DF_XHINT, (int)gridPointArgumentHint), "dfiEditVal");
                        }
                    }

                    if (state.HasFlag(GridPointCurve.State.GridPointValueChanged))
                    {
                        if (ArrayMemory.Reallocate(ref m_GridPointValues, gridPointCount, Math.Max(10, gridPointCount / 5)) == true)
                        {
                            m_ReadOnlyGridPointValues = new ReadOnlyCollection<double>(m_GridPointValues);
                            if (isInitializedBefore == true)
                            {
                                CheckErrorCode(_dfdEditPtr(m_Task, PtrParameterChangeType.DF_Y, m_GridPointValues), "dfdEditPtr");
                            }
                        }
                        gridPointValues.CopyTo(m_GridPointValues, gridPointCount, gridPointValueStartIndex, sourceIncrement: gridPointValueIncrement);

                        gridPointValueTransformation(gridPointCount, m_GridPointValues);  // mainly used for log-linear transformation, i.e. apply logarithm to each grid point value. Caution: The public properties shows transformed values as well!

                        if (isInitializedBefore == true)
                        {
                            CheckErrorCode(_dfiEditVal(m_Task, IntParameterChangeType.DF_NX, gridPointCount), "dfiEditVal");
                            CheckErrorCode(_dfiEditVal(m_Task, IntParameterChangeType.DF_YHINT, (int)gridPointValueHint), "dfiEditVal");
                        }
                        else
                        {
                            CheckErrorCode(_dfdNewTask1D(out m_Task, gridPointCount, m_GridPointArguments, gridPointArgumentHint, 1, m_GridPointValues, gridPointValueHint), "dfdNewTask1D");
                        }
                    }

                    if (state != GridPointCurve.State.NoChangeSinceLastUpdate)
                    {
                        CheckErrorCode(_dfdEditPPSpline1D(m_Task, m_MklDataFitting.m_SplineOrder, m_MklDataFitting.m_SplineType, m_MklDataFitting.m_BoundaryConditionType, m_MklDataFitting.m_BoundaryCondition, m_MklDataFitting.m_InternalConditionTypes, m_MklDataFitting.m_InternalConditions, m_SplineCoefficients, m_MklDataFitting.m_SplineCoefficientHint), "dfdEditPPSpline1D");

                        if (m_MklDataFitting.m_SplineOrder != MklCurveInterpolationSpline.SplineOrder.DF_PP_STD)  // for a 'real' spline interpolation, one has to calculate the spline coefficients
                        {
                            CheckErrorCode(_dfdConstruct1D(m_Task, MklCurveInterpolationSpline.SplineFormat.DF_PP_SPLINE, MklCurveInterpolationSpline.SplineConstructionMethod.DF_METHOD_STD), "dfdConstruct1D");
                        }
                    }
                }
            }
            #endregion

            #region protected methods

            /// <summary>Checks some MKL error code.
            /// </summary>
            /// <param name="status">The status, <c>0</c> represents 'no error'.</param>
            /// <param name="functionName">The name of the MKL function, needed perhaps for the exception message.</param>
            /// <exception cref="InvalidOperationException">Thrown, if <paramref name="status"/> is != 0.</exception>
            protected void CheckErrorCode(int status, string functionName)
            {
                if (status != 0)  // execution is not successful
                {
                    throw new InvalidOperationException(String.Format("MKL: Return value {0} in {1}.", status, functionName));
                }
            }
            #endregion
        }

        #endregion

        #region private members

        /// <summary>The spline order.
        /// </summary>
        private MklCurveInterpolationSpline.SplineOrder m_SplineOrder;

        /// <summary>The spline type.
        /// </summary>
        private MklCurveInterpolationSpline.SplineType m_SplineType;

        /// <summary>The type of the boundary condition.
        /// </summary>
        private MklCurveInterpolationSpline.SplineBoundaryCondition m_BoundaryConditionType;

        /// <summary>The boundary condition.
        /// </summary>
        private double[] m_BoundaryCondition;

        /// <summary>The usage of the spline coefficients.
        /// </summary>
        private MklCurveInterpolationSpline.SplineCoefficientStorageFormat m_SplineCoefficientHint;

        /// <summary>The internal Spline condition type.
        /// </summary>
        private MklCurveInterpolationSpline.SplineInternalConditionType m_InternalConditionTypes;

        /// <summary>The internal Spline condition; often <c>null</c>.
        /// </summary>
        private double[] m_InternalConditions;

        /// <summary>The name of the curve interpolator.
        /// </summary>
        private IdentifierString m_Name;

        /// <summary>The long name of the curve interpolator.
        /// </summary>
        private IdentifierString m_LongName;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="MklDataFitting" /> class.
        /// </summary>
        /// <param name="splineOrder">The spline order.</param>
        /// <param name="splineType">The type of the spline.</param>
        /// <param name="boundaryConditionType">The type of the boundary condition.</param>
        /// <param name="boundaryCondition">The boundary condition.</param>
        /// <param name="splineCoefficientHint">The spline coefficient hint.</param>
        /// <param name="internalConditionType">The internal boundary condition type.</param>
        /// <param name="internalConditions">The internal boundary condition.</param>
        public MklDataFitting(MklCurveInterpolationSpline.SplineOrder splineOrder, MklCurveInterpolationSpline.SplineType splineType, MklCurveInterpolationSpline.SplineBoundaryCondition boundaryConditionType, double[] boundaryCondition = null, MklCurveInterpolationSpline.SplineCoefficientStorageFormat splineCoefficientHint = MklCurveInterpolationSpline.SplineCoefficientStorageFormat.DF_NO_HINT, MklCurveInterpolationSpline.SplineInternalConditionType internalConditionType = MklCurveInterpolationSpline.SplineInternalConditionType.DF_NO_IC, double[] internalConditions = null)
            : base(MklCurveResource.AnnotationMklGeneral)
        {
            m_SplineOrder = splineOrder;
            m_SplineType = splineType;
            m_BoundaryConditionType = boundaryConditionType;
            m_BoundaryCondition = boundaryCondition;
            m_SplineCoefficientHint = splineCoefficientHint;
            m_InternalConditionTypes = internalConditionType;
            m_InternalConditions = internalConditions;

            m_Name = m_LongName = new IdentifierString(String.Format("MKL data fitting ({0};{1};{2};{3})", splineOrder.ToFormatString(EnumStringRepresentationUsage.StringAttribute), splineType.ToFormatString(EnumStringRepresentationUsage.StringAttribute), boundaryConditionType.ToFormatString(EnumStringRepresentationUsage.StringAttribute), internalConditionType.ToFormatString(EnumStringRepresentationUsage.StringAttribute)));
        }
        #endregion

        #region public properties

        /// <summary>Gets a value indicating whether this instance represents a local approach.
        /// </summary>
        /// <value><c>true</c> if this instance is local approach; otherwise, <c>false</c>.</value>
        /// <remarks>In the case of a local approach call <see cref="GetLeftLocalnessLevel(int, int)" /> and <see cref="GetRightLocalnessLevel(int, int)" />
        /// for the left and right localness level.
        /// <para>In the case of a global approach all grid points are required for the curve interpolation.</para>
        /// </remarks>
        public override bool IsLocalApproach
        {
            get { return true; }
        }
        #endregion

        #region public methods

        /// <summary>Creates a <see cref="ICurveDataFitting" /> object that represents the implementation of the interpolation approach.
        /// </summary>
        /// <returns>A <see cref="ICurveDataFitting" /> object that represents the implementation of the interpolation approach.</returns>
        public override ICurveDataFitting Create()
        {
            return new Interpolator(this);
        }

        /// <summary>Creates a <see cref="IMklCurveDataFitting" /> object that represents the implementation of the interpolation approach.
        /// </summary>
        /// <param name="computationMethod">The computation method with respect to the MKL data fitting routines.</param>
        /// <returns>A <see cref="IMklCurveDataFitting" /> object that represents the implementation of the interpolation approach.</returns>
        /// <remarks>The current release of Intel's Math Kernel Library (11.0, Update 5) does support exactly one computation method.</remarks>
        public override IMklCurveDataFitting Create(MklGridPointCurve.ComputationMethod computationMethod)
        {
            return new Interpolator(this);
        }

        public override int GetLeftLocalnessLevel(int gridPointIndex, int gridPointCount)
        {
            return gridPointIndex;  // todo: 

        }

        public override int GetRightLocalnessLevel(int gridPointIndex, int gridPointCount)
        {
            return gridPointCount - gridPointIndex - 1;  // todo: 
        }
        #endregion

        #region protected methods

        /// <summary>Gets the name of the curve interpolator.
        /// </summary>
        /// <returns>The name of the curve interpolator.</returns>
        protected override IdentifierString GetName()
        {
            return m_Name;
        }

        /// <summary>Gets the long name of the curve interpolator.
        /// </summary>
        /// <returns>The (perhaps) language dependent long name of the curve interpolator.</returns>
        protected override IdentifierString GetLongName()
        {
            return m_LongName;
        }
        #endregion
    }
}