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
using System.Collections.ObjectModel;

using Dodoni.BasicComponents;
using Dodoni.MathLibrary.Basics;
using Dodoni.BasicComponents.Containers;
using Dodoni.MathLibrary.GridPointCurves;

namespace Dodoni.MathLibrary.Native.GridPointCurves
{
    /// <summary>Represents the log-linear curve interpolator with respect to Intel's MKL Library.
    /// </summary>
    internal class MklCurveInterpolationLogLinear : MklGridPointCurve.Interpolator
    {
        #region nested classes

        /// <summary>The modification of the MKL data fitting for Log-linear interpolation.
        /// </summary>
        private class LogMklDataFitting : MklDataFitting.Interpolator
        {
            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="LogMklDataFitting"/> class.
            /// </summary>
            /// <param name="interpolationFactory">The <see cref="MklDataFitting"/> object that serves as factory for the current object.</param>
            internal LogMklDataFitting(MklDataFitting interpolationFactory)
                : base(interpolationFactory)
            {
            }
            #endregion

            #region public methods

            /// <summary>Gets the value at a specific argument.
            /// </summary>
            /// <param name="pointToEvaluate">The point to evaluate.</param>
            /// <returns>The value of the curve at <paramref name="pointToEvaluate"/></returns>
            /// <remarks>The argument must be an element of the domain of definition, represented by <see cref="IRealValuedCurve.LowerBound"/> and <see cref="IRealValuedCurve.UpperBound"/>.</remarks>
            public new double GetValue(double pointToEvaluate)
            {
                return Math.Exp(base.GetValue(pointToEvaluate));
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
            public new void GetValues(int n, double[] site, double[] result, int ndorder, int[] dorder, MklGridPointCurve.SiteHint siteHint = MklGridPointCurve.SiteHint.DF_NO_HINT, MklGridPointCurve.ResultHint resultHint = MklGridPointCurve.ResultHint.DF_NO_HINT, MklGridPointCurve.EstimationType estimationType = MklGridPointCurve.EstimationType.DF_INTERP, int[] cell = null, double[] dataHint = null)
            {
                base.GetValues(n, site, result, ndorder, dorder, siteHint, resultHint, estimationType, cell, dataHint);
                VectorUnit.Basics.Exp(n, result);
            }

            /// <summary>Gets the value of the integral \int_a^b f(x) dx.
            /// </summary>
            /// <param name="lowerBound">The lower bound.</param>
            /// <param name="upperBound">The upper bound.</param>
            /// <returns>The value of \int_a^b f(x) dx.</returns>
            /// <remarks>The arguments must be elements of the domain of definition, represented by <see cref="IRealValuedCurve.LowerBound"/> and <see cref="IRealValuedCurve.UpperBound"/>.</remarks>
            public new double GetIntegral(double lowerBound, double upperBound)
            {
                unsafe
                {
                    double* lowerLimits = stackalloc double[1];
                    lowerLimits[0] = lowerBound;

                    double* upperLimits = stackalloc double[1];
                    upperLimits[0] = upperBound;

                    double* result = stackalloc double[1];

                    CheckErrorCode(_dfdIntegrateEx1D(Task, MklGridPointCurve.ComputationMethod.DF_METHOD_PP, 1, lowerLimits, MklGridPointCurve.IntegrationLimitHint.DF_NO_HINT, upperLimits, MklGridPointCurve.IntegrationLimitHint.DF_NO_HINT, null, null, result, MklGridPointCurve.IntegralResultHint.DF_NO_HINT, null, IntPtr.Zero, null, IntPtr.Zero, IntegrationMklCallBackFunction, IntPtr.Zero, null, IntPtr.Zero), "dfdInterpolate1D");
                    return result[0];
                }
            }

            /// <summary>Gets the value of the integral \int_a^b f(x) dx.
            /// </summary>
            /// <param name="n">Number of pairs of integraion limits.</param>
            /// <param name="lowerBounds">The lower bounds.</param>
            /// <param name="upperBounds">The upper bounds.</param>
            /// <param name="result">Array of integration results (output). The size of the array should be sufficient to hold <paramref name="n" />* ny values, where ny is the dimension of the vector-valued function. The integration results are packed according to the settings in <paramref name="resultHint" />.</param>
            /// <param name="lowerBoundHint">A flag describing the structure of the left-side integration limits.</param>
            /// <param name="upperBoundHint">A flag describing the structure of the right-side integration limits.</param>
            /// <param name="resultHint">A flag describing the structure of the results.</param>
            /// <param name="ldataHint">Array that contains additional information about the structure of partition x and left-side integration limits. For details see table "Structure of the datahint Array" in the description of the df?Intepolate1D function in Intel Math Kernel Library Reference Manual.</param>
            /// <param name="rdataHint">Array that contains additional information about the structure of partition x and right-side integration limits. For details see table "Structure of the datahint Array" in the description of the df?Intepolate1D function in Intel Math Kernel Library Reference Manual.</param>
            /// <remarks>
            /// The arguments must be elements of the domain of definition, represented by <see cref="IRealValuedCurve.LowerBound" /> and <see cref="IRealValuedCurve.UpperBound" />.
            /// </remarks>
            public new void GetIntegral(int n, double[] lowerBounds, double[] upperBounds, double[] result, MklGridPointCurve.IntegrationLimitHint lowerBoundHint = MklGridPointCurve.IntegrationLimitHint.DF_NO_HINT, MklGridPointCurve.IntegrationLimitHint upperBoundHint = MklGridPointCurve.IntegrationLimitHint.DF_NO_HINT, MklGridPointCurve.IntegralResultHint resultHint = MklGridPointCurve.IntegralResultHint.DF_NO_HINT, double[] ldataHint = null, double[] rdataHint = null)
            {
                CheckErrorCode(_dfdIntegrateEx1D(Task, MklGridPointCurve.ComputationMethod.DF_METHOD_PP, n, lowerBounds, lowerBoundHint, upperBounds, upperBoundHint, ldataHint, rdataHint, result, resultHint, null, IntPtr.Zero, null, IntPtr.Zero, IntegrationMklCallBackFunction, IntPtr.Zero, null, IntPtr.Zero), "dfdInterpolate1D");
            }
            #endregion

            #region private methods

            /// <summary>The call-back method for the integration. 
            /// </summary>
            /// <param name="n">Number of pairs of integration limits.</param>
            /// <param name="lcell">Array of size n with indices of the cells that contain the left-side integration limits in array llim.</param>
            /// <param name="llim">Array of size n that holds the left-side integration limits.</param>
            /// <param name="rcell">Array of size n with indices of the cells that contain the right-side integration limits in array rlim.</param>
            /// <param name="rlim">Array of size n that holds the right-side integration limits.</param>
            /// <param name="r">Array of integration results. For packing the results in row-major format, follow the instructions described in df?interpolate1d/df?interpolateex1d.</param>
            /// <param name="param">Pointer to user-defined parameters of the callback function.</param>
            /// <returns>The status returned by the callback function: Zero indicates successful completion of the callback operation.A negative value indicates an error. A positive value indicates a warning.</returns>
            private int IntegrationMklCallBackFunction(ref Int64 n, Int64[] lcell, double[] llim, Int64[] rcell, double[] rlim, double[] r, IntPtr param)
            {
                /* If this approach is slow, it is possible to apply the MKL functionality to each relevant sub-interval first and apply the exponential 
                 * function afterwards. This requires to store the results of the sub-intervals temporary.
                 */
                for (var k = 0; k < n; k++)
                {
                    double value = 0.0;
                    var leftGridIndex = (int)lcell[k] - 1;  // is one-based ! Why?
                    var lowerBound = llim[k];
                    var upperBound = rlim[k];

                    while ((leftGridIndex < GridPointCount - 1) && (GridPointArguments[leftGridIndex] <= upperBound))
                    {
                        // the property 'GridPointValues' contains the logarithm of the specified grid point values!
                        double m = (GridPointValues[leftGridIndex + 1] - GridPointValues[leftGridIndex]) / (GridPointArguments[leftGridIndex + 1] - GridPointArguments[leftGridIndex]);
                        double b = GridPointValues[leftGridIndex] - m * GridPointArguments[leftGridIndex];

                        double stepUpperBound = Math.Min(upperBound, GridPointArguments[leftGridIndex + 1]);
                        value += (Math.Exp(stepUpperBound * m + b) - Math.Exp(lowerBound * m + b)) / m;

                        lowerBound = GridPointArguments[leftGridIndex + 1];
                        leftGridIndex++;
                    }
                    r[k] = value;
                }
                return 0;
            }
            #endregion
        }

        /// <summary>Serves as implementation of the log-linear interpolation approach.
        /// </summary>
        protected class LogInterpolator : IMklCurveDataFitting
        {
            #region private members

            /// <summary>The log-linear curve interpolator factory.
            /// </summary>
            private MklCurveInterpolationLogLinear m_Factory;

            /// <summary>The log-linear MKL data fitting approach.
            /// </summary>
            private LogMklDataFitting m_DataFitting;

            /// <summary>The original grid point values.
            /// </summary>
            private double[] m_OriginalGridPointValues;

            /// <summary>A read-only wrapper for the original grid point values.
            /// </summary>
            private ReadOnlyCollection<double> m_OriginalGridPointValuesReadOnlyCollection;
            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="LogInterpolator"/> class.
            /// </summary>
            /// <param name="interpolationFactory">The <see cref="MklCurveInterpolationLogLinear"/> object that serves as factory for the current object.</param>
            internal LogInterpolator(MklCurveInterpolationLogLinear interpolationFactory)
            {
                m_Factory = interpolationFactory;
                m_DataFitting = new LogMklDataFitting(interpolationFactory.m_DataFitting);
            }
            #endregion

            #region public properties

            #region IOperable Members

            /// <summary>Gets a value indicating whether this instance is operable.
            /// </summary>
            /// <value><c>true</c> if this instance is operable; otherwise, <c>false</c>.</value>
            /// <remarks>
            ///   <c>false</c> will be returned if the current instance represents some data, model, interpolation procedure,
            /// integration approach, optimization procedure etc. and no valid parameters are available.
            /// </remarks>
            public bool IsOperable
            {
                get { return m_DataFitting.IsOperable; }
            }
            #endregion

            #region IRealValuedCurve Members

            /// <summary>Gets the lower bound of the domain of definition.
            /// </summary>
            /// <value>The lower bound of the domain of definition, perhaps <see cref="System.Double.NegativeInfinity" /> or <see cref="System.Double.NaN" />.</value>
            public double LowerBound
            {
                get { return m_DataFitting.LowerBound; }
            }

            /// <summary>Gets the upper bound of the domain of definition.
            /// </summary>
            /// <value>The upper bound of the domain of definition, perhaps <see cref="System.Double.PositiveInfinity" /> or <see cref="System.Double.NaN" />.</value>
            public double UpperBound
            {
                get { return m_DataFitting.UpperBound; }
            }
            #endregion

            #region ICurveDataFitting Members

            /// <summary>Gets the number of grid points.
            /// </summary>
            /// <value>The number of grid points.</value>
            public int GridPointCount
            {
                get { return m_DataFitting.GridPointCount; }
            }

            /// <summary>Gets the grid point arguments, i.e. the labels (on the x-axis) of the curve in its <see cref="System.Double" /> representation.
            /// </summary>
            /// <value>The grid point arguments.</value>
            public IList<double> GridPointArguments
            {
                get { return m_DataFitting.GridPointArguments; }
            }

            /// <summary>Gets the grid point values with respect to <see cref="ICurveDataFitting.GridPointArguments" />.
            /// </summary>
            /// <value>The grid point values.</value>
            public IList<double> GridPointValues
            {
                get { return m_OriginalGridPointValuesReadOnlyCollection; }
            }

            /// <summary>Gets the factory of <see cref="ICurveDataFitting" /> objects of the same type and configuration.
            /// </summary>
            /// <value>The factory of <see cref="ICurveDataFitting" /> objects of the same type and configuration.</value>
            public ICurveDataFittingFactory Factory
            {
                get { return m_Factory; }
            }
            #endregion

            #region IInfoOutputQueriable Members

            /// <summary>Gets the info-output level of detail.
            /// </summary>
            /// <value>The info-output level of detail.</value>
            public InfoOutputDetailLevel InfoOutputDetailLevel
            {
                get { return m_DataFitting.InfoOutputDetailLevel; }
            }
            #endregion

            #endregion

            #region public methods

            #region IRealValuedCurve Members

            /// <summary>Gets the value at a specific argument.
            /// </summary>
            /// <param name="pointToEvaluate">The point to evaluate.</param>
            /// <returns>The value of the curve at <paramref name="pointToEvaluate"/></returns>
            /// <remarks>The argument must be an element of the domain of definition, represented by <see cref="IRealValuedCurve.LowerBound"/> and <see cref="IRealValuedCurve.UpperBound"/>.</remarks>
            public double GetValue(double pointToEvaluate)
            {
                return m_DataFitting.GetValue(pointToEvaluate);
            }

            /// <summary>Gets the value of the integral \int_a^b f(x) dx.
            /// </summary>
            /// <param name="lowerBound">The lower bound.</param>
            /// <param name="upperBound">The upper bound.</param>
            /// <returns>The value of \int_a^b f(x) dx.</returns>
            /// <remarks>The arguments must be elements of the domain of definition, represented by <see cref="IRealValuedCurve.LowerBound"/> and <see cref="IRealValuedCurve.UpperBound"/>.</remarks>
            public double GetIntegral(double lowerBound, double upperBound)
            {
                return m_DataFitting.GetIntegral(lowerBound, upperBound);
            }
            #endregion

            #region IMklCurveDataFitting Members

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
            /// <remarks>
            /// The argument must be an element of the domain of definition, represented by <see cref="IRealValuedCurve.LowerBound" /> and <see cref="IRealValuedCurve.UpperBound" />.
            /// </remarks>
            public void GetValues(int n, double[] site, double[] result, int ndorder, int[] dorder, MklGridPointCurve.SiteHint siteHint = MklGridPointCurve.SiteHint.DF_NO_HINT, MklGridPointCurve.ResultHint resultHint = MklGridPointCurve.ResultHint.DF_NO_HINT, MklGridPointCurve.EstimationType estimationType = MklGridPointCurve.EstimationType.DF_INTERP, int[] cell = null, double[] dataHint = null)
            {
                m_DataFitting.GetValues(n, site, result, ndorder, dorder, siteHint, resultHint, estimationType, cell, dataHint);
            }

            /// <summary>Gets the value of the integral \int_a^b f(x) dx.
            /// </summary>
            /// <param name="n">Number of pairs of integraion limits.</param>
            /// <param name="lowerBounds">The lower bounds.</param>
            /// <param name="upperBounds">The upper bounds.</param>
            /// <param name="result">Array of integration results (output). The size of the array should be sufficient to hold <paramref name="n" />* ny values, where ny is the dimension of the vector-valued function. The integration results are packed according to the settings in <paramref name="resultHint" />.</param>
            /// <param name="lowerBoundHint">A flag describing the structure of the left-side integration limits.</param>
            /// <param name="upperBoundHint">A flag describing the structure of the right-side integration limits.</param>
            /// <param name="resultHint">A flag describing the structure of the results.</param>
            /// <param name="ldataHint">Array that contains additional information about the structure of partition x and left-side integration limits. For details see table "Structure of the datahint Array" in the description of the df?Intepolate1D function in Intel Math Kernel Library Reference Manual.</param>
            /// <param name="rdataHint">Array that contains additional information about the structure of partition x and right-side integration limits. For details see table "Structure of the datahint Array" in the description of the df?Intepolate1D function in Intel Math Kernel Library Reference Manual.</param>
            /// <remarks>
            /// The arguments must be elements of the domain of definition, represented by <see cref="IRealValuedCurve.LowerBound" /> and <see cref="IRealValuedCurve.UpperBound" />.
            /// </remarks>
            public void GetIntegral(int n, double[] lowerBounds, double[] upperBounds, double[] result, MklGridPointCurve.IntegrationLimitHint lowerBoundHint = MklGridPointCurve.IntegrationLimitHint.DF_NO_HINT, MklGridPointCurve.IntegrationLimitHint upperBoundHint = MklGridPointCurve.IntegrationLimitHint.DF_NO_HINT, MklGridPointCurve.IntegralResultHint resultHint = MklGridPointCurve.IntegralResultHint.DF_NO_HINT, double[] ldataHint = null, double[] rdataHint = null)
            {
                m_DataFitting.GetIntegral(n, lowerBounds, upperBounds, result, lowerBoundHint, upperBoundHint, resultHint, ldataHint, rdataHint);
            }

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
                /* lets store a copy of the original grid point values: */
                if (state.HasFlag(GridPointCurve.State.GridPointValueChanged))
                {
                    if (ArrayMemory.Reallocate(ref m_OriginalGridPointValues, gridPointCount, Math.Max(10, gridPointCount / 5)) == true)
                    {
                        m_OriginalGridPointValuesReadOnlyCollection = new ReadOnlyCollection<double>(m_OriginalGridPointValues);
                    }
                    gridPointValues.CopyTo(m_OriginalGridPointValues, gridPointCount, gridPointValueStartIndex, sourceIncrement: gridPointValueIncrement);
                }
                m_DataFitting.Update(gridPointCount, gridPointArguments, gridPointValues, gridPointArgumentHint, gridPointValueHint, (n, x) => { VectorUnit.Basics.Log(n, x); }, state, gridPointArgumentStartIndex, gridPointValueStartIndex, gridPointArgumentIncrement, gridPointValueIncrement);
            }
            #endregion

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
            public void Update(int gridPointCount, IList<double> gridPointArguments, IList<double> gridPointValues, GridPointCurve.State state = GridPointCurve.State.GridPointChanged, int gridPointArgumentStartIndex = 0, int gridPointValueStartIndex = 0, int gridPointArgumentIncrement = 1, int gridPointValueIncrement = 1)
            {
                this.Update(gridPointCount, gridPointArguments, gridPointValues, MklGridPointCurve.xHintValue.DF_NO_HINT, MklGridPointCurve.yHintValue.DF_NO_HINT, state, gridPointArgumentStartIndex, gridPointValueStartIndex, gridPointArgumentIncrement, gridPointValueIncrement);
            }

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

            #region IInfoOutputQueriable Members

            /// <summary>Sets the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> property.
            /// </summary>
            /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
            /// <returns>A value indicating whether the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> has been set to <paramref name="infoOutputDetailLevel" />.</returns>
            public bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
            {
                return m_DataFitting.TrySetInfoOutputDetailLevel(infoOutputDetailLevel);
            }

            /// <summary>Gets informations of the current object as a specific <see cref="InfoOutput" /> instance.
            /// </summary>
            /// <param name="infoOutput">The <see cref="InfoOutput" /> object which is to be filled with informations concering the current instance.</param>
            /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
            public void FillInfoOutput(InfoOutput infoOutput, string categoryName = InfoOutput.GeneralCategoryName)
            {
                var infoOutputPackage = infoOutput.AcquirePackage(categoryName);

                infoOutputPackage.Add("Name", Factory.Name.String);
                infoOutputPackage.Add("Long Name", Factory.LongName.String);
                infoOutputPackage.Add("Fitting Quality", Factory.FittingQuality);
                infoOutputPackage.Add("Is Local approach", Factory.IsLocalApproach);
                infoOutputPackage.Add("Minimal required number of grid points", Factory.MinimalRequiredNumberOfGridPoints);

                infoOutputPackage.Add("Count", GridPointCount);

                DataTable gridPointTable = new DataTable("Grid points");
                gridPointTable.Columns.Add("Argument", typeof(double));
                gridPointTable.Columns.Add("Value", typeof(double));

                for (int j = 0; j < GridPointCount; j++)
                {
                    var row = gridPointTable.NewRow();
                    row[0] = GridPointArguments[j];
                    row[1] = GridPointValues[j];
                    gridPointTable.Rows.Add(row);
                }
                infoOutputPackage.Add(gridPointTable);
            }
            #endregion

            #endregion
        }
        #endregion

        #region private members

        /// <summary>The MKL data fitting algorithm.
        /// </summary>
        private MklDataFitting m_DataFitting;

        /// <summary>The name of the curve interpolation.
        /// </summary>
        private IdentifierString m_Name;

        /// <summary>The long name of the curve interpolation.
        /// </summary>
        private IdentifierString m_LongName;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="MklCurveInterpolationLogLinear" /> class.
        /// </summary>
        internal MklCurveInterpolationLogLinear()
            : base(MklCurveResource.AnnotationInterpolationLogLinear)
        {
            m_DataFitting = new MklDataFitting(MklCurveInterpolationSpline.SplineOrder.DF_PP_LINEAR, MklCurveInterpolationSpline.SplineType.DF_PP_DEFAULT, MklCurveInterpolationSpline.SplineBoundaryCondition.DF_NO_BC);
            m_Name = new IdentifierString("MKL LogLinear");
            m_LongName = new IdentifierString(MklCurveResource.LongNameInterpolationLogLinear);
        }
        #endregion

        #region public methods

        /// <summary>Creates a <see cref="ICurveDataFitting" /> object that represents the implementation of the interpolation approach.
        /// </summary>
        /// <returns>A <see cref="ICurveDataFitting" /> object that represents the implementation of the interpolation approach.</returns>
        public override ICurveDataFitting Create()
        {
            return new LogInterpolator(this);
        }

        /// <summary>Creates a <see cref="IMklCurveDataFitting" /> object that represents the implementation of the interpolation approach.
        /// </summary>
        /// <param name="computationMethod">The computation method with respect to the MKL data fitting routines.</param>
        /// <returns>A <see cref="IMklCurveDataFitting" /> object that represents the implementation of the interpolation approach.</returns>
        public override IMklCurveDataFitting Create(MklGridPointCurve.ComputationMethod computationMethod)
        {
            return new LogInterpolator(this);
        }

        /// <summary>Gets a value indicating whether this instance represents a local approach.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is local approach; otherwise, <c>false</c>.</value>
        /// <remarks>
        /// In the case of a local approach call <see cref="GetLeftLocalnessLevel(int, int)" /> and <see cref="GetRightLocalnessLevel(int, int)" />
        /// for the left and right localness level.
        /// <para>In the case of a global approach all grid points are required for the curve interpolation.</para>
        /// </remarks>
        public override bool IsLocalApproach
        {
            get { return true; }
        }

        /// <summary>Gets the left localness level for a specific grid point, i.e.
        /// changing grid point (t_j,x_j) implies changes on the interval [t_{j-leftLocalnessLevel}, t_{j+rightLocalnessLevel}].
        /// </summary>
        /// <param name="gridPointIndex">The null-based index of the grid point (t_j,x_j).</param>
        /// <param name="gridPointCount">The number of grid points.</param>
        /// <returns>
        /// The left localness level with respect to the grid point (t_j,x_j), where j is represented by <paramref name="gridPointIndex" />
        /// i.e. changing grid point (t_j,x_j) implies changes on the interval [t_{j-leftLocalnessLevel}, t_{j+rightLocalnessLevel}].
        /// </returns>
        /// <remarks>
        /// The localness level does not depend on the value of the grid point itself.
        /// </remarks>
        public override int GetLeftLocalnessLevel(int gridPointIndex, int gridPointCount)
        {
            return (gridPointIndex == 0) ? 0 : 1;
        }

        /// <summary>Gets the right localness level for a specific grid point, i.e.
        /// changing grid point (t_j,x_j) implies changes on the interval [t_{j-leftLocalnessLevel}, t_{j+rightLocalnessLevel}].
        /// </summary>
        /// <param name="gridPointIndex">The null-based index of the grid point (t_j,x_j).</param>
        /// <param name="gridPointCount">The number of grid points.</param>
        /// <returns>
        /// The right localness level with respect to the grid point (t_j,x_j), where j is represented by <paramref name="gridPointIndex" />
        /// i.e. changing grid point (t_j,x_j) implies changes on the interval [t_{j-leftLocalnessLevel}, t_{j+rightLocalnessLevel}].
        /// </returns>
        /// <remarks>
        /// The localness level does not depend on the value of the grid point itself.
        /// </remarks>
        public override int GetRightLocalnessLevel(int gridPointIndex, int gridPointCount)
        {
            return (gridPointIndex == gridPointCount - 1) ? 0 : 1;
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