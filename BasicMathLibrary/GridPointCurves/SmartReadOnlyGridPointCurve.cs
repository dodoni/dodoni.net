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
using System.Collections.Generic;

using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.GridPointCurves
{
    /// <summary>Serves as a read-only grid point curve.
    /// </summary>
    /// <typeparam name="TLabel">The type of the labels.</typeparam>
    /// <remarks>A reference of the labels will be stored only. The grid point arguments and grid point values should be stored by
    /// the underlying interpolation/parametrization. 
    /// <para>
    /// This class was mainly designed used for two-dimensional surface interpolation. Assuming a large grid point matrix, for example 1.000 x 1.000 and one may apply for example a linear interpolation 
    /// along horizontal direction and afterwards a linear interpolation along vertical direction. Then we create 1.000 curves in horizontal direction, where the (double) labels and values are constant. The
    /// standard implementation create a copy of 3 * 1.000 x 1.000 = 3.000.000 values and calles the update method of the underlying <see cref="ICurveDataFitting"/> object which again copy 2 * 1.000 * 1.000 = 2.000.000 values, i.e. in total 5 Mio values.
    /// This implementation takes into account references, the underlying <see cref="ICurveDataFitting"/> implementation should create deep copies of grid point arguments/values only, i.e. 2 Mio double values.
    /// </para></remarks>
    internal class SmartReadOnlyGridPointCurve<TLabel> : IGridPointCurve<TLabel>
        where TLabel : IEquatable<TLabel>
    {
        #region nested classes

        /// <summary>Serves as standard implementation of a differentiable grid point curve.
        /// </summary>
        internal class Differentiable : SmartReadOnlyGridPointCurve<TLabel>, IDifferentiableRealValuedCurve
        {
            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="Differentiable"/> class.
            /// </summary>
            /// <param name="curveInterpolator">The curve interpolator.</param>
            /// <param name="leftExtrapolator">The left extrapolator.</param>
            /// <param name="rightExtrapolator">The right extrapolator.</param>
            /// <param name="gridPointCount">The number of grid points, i.e. the number of relevant elements of <paramref name="gridPointLabels"/>, <paramref name="gridPointArguments"/> and <paramref name="gridPointValues"/> to take into account.</param>
            /// <param name="gridPointLabels">The labels of the grid points (the reference will be stored only).</param>
            /// <param name="gridPointArguments">The arguments of the grid points, thus labels of the curve in its <see cref="System.Double"/> representation.</param>
            /// <param name="gridPointValues">The values of the grid points corresponding to <paramref name="gridPointArguments"/>.</param>
            /// <param name="gridPointArgumentStartIndex">The null-based start index of <paramref name="gridPointArguments"/> and <paramref name="gridPointLabels"/> to take into account.</param>
            /// <param name="gridPointValueStartIndex">The null-based start index of <paramref name="gridPointValues"/> to take into account.</param>
            /// <param name="gridPointArgumentIncrement">The increment for <paramref name="gridPointArguments"/> and <paramref name="gridPointLabels"/>.</param>
            /// <param name="gridPointValueIncrement">The increment for <paramref name="gridPointValues"/>.</param>
            internal Differentiable(ICurveDataFitting curveInterpolator, ICurveExtrapolator leftExtrapolator, ICurveExtrapolator rightExtrapolator, int gridPointCount, IList<TLabel> gridPointLabels, IList<double> gridPointArguments, IList<double> gridPointValues, int gridPointArgumentStartIndex = 0, int gridPointValueStartIndex = 0, int gridPointArgumentIncrement = 1, int gridPointValueIncrement = 1)
                : base(curveInterpolator, leftExtrapolator, rightExtrapolator, gridPointCount, gridPointLabels, gridPointArguments, gridPointValues, gridPointArgumentStartIndex, gridPointValueStartIndex, gridPointArgumentIncrement, gridPointValueIncrement)
            {
            }

            /// <summary>Initializes a new instance of the <see cref="Differentiable"/> class.
            /// </summary>
            /// <param name="curveInterpolator">The curve interpolator, i.e. curve parametrization.</param>
            /// <param name="gridPointCount">The number of grid points, i.e. the number of relevant elements of <paramref name="gridPointLabels"/>, <paramref name="gridPointArguments"/> and <paramref name="gridPointValues"/> to take into account.</param>
            /// <param name="gridPointLabels">The labels of the grid points (the reference will be stored only).</param>
            /// <param name="gridPointArguments">The arguments of the grid points, thus labels of the curve in its <see cref="System.Double"/> representation.</param>
            /// <param name="gridPointValues">The values of the grid points corresponding to <paramref name="gridPointArguments"/>.</param>
            /// <param name="gridPointArgumentStartIndex">The null-based start index of <paramref name="gridPointArguments"/> and <paramref name="gridPointLabels"/> to take into account.</param>
            /// <param name="gridPointValueStartIndex">The null-based start index of <paramref name="gridPointValues"/> to take into account.</param>
            /// <param name="gridPointArgumentIncrement">The increment for <paramref name="gridPointArguments"/> and <paramref name="gridPointLabels"/>.</param>
            /// <param name="gridPointValueIncrement">The increment for <paramref name="gridPointValues"/>.</param>
            internal Differentiable(ICurveDataFitting curveInterpolator, int gridPointCount, IList<TLabel> gridPointLabels, IList<double> gridPointArguments, IList<double> gridPointValues, int gridPointArgumentStartIndex = 0, int gridPointValueStartIndex = 0, int gridPointArgumentIncrement = 1, int gridPointValueIncrement = 1)
                : base(curveInterpolator, gridPointCount, gridPointLabels, gridPointArguments, gridPointValues, gridPointArgumentStartIndex, gridPointValueStartIndex, gridPointArgumentIncrement, gridPointValueIncrement)
            {
            }
            #endregion

            #region public methods

            /// <summary>Returns a read-only <see cref="IGridPointCurve{TLabel}"/> wrapper for the current instance.
            /// </summary>
            /// <returns>A <see cref="IGridPointCurve{TLabel}"/> object that acts as a read-only wrapper around the current instance.</returns>
            public override IGridPointCurve<TLabel> AsReadOnly()
            {
                return this;
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
                if (pointToEvaluate < m_CurveInterpolator.LowerBound)
                {
                    return ((IDifferentiableRealValuedCurve)m_LeftExtrapolator).GetDerivative(pointToEvaluate);
                }
                else if (pointToEvaluate > m_CurveInterpolator.UpperBound)
                {
                    return ((IDifferentiableRealValuedCurve)m_RightExtrapolator).GetDerivative(pointToEvaluate);
                }
                return ((IDifferentiableRealValuedCurve)m_CurveInterpolator).GetDerivative(pointToEvaluate);
            }
            #endregion
        }
        #endregion

        #region protected members

        /// <summary>The labels of the grid points.
        /// </summary>
        protected readonly SmartReadOnlyCollection<TLabel> m_GridPointLabels;

        /// <summary>The curve interpolation approach.
        /// </summary>
        protected readonly ICurveDataFitting m_CurveInterpolator;

        /// <summary>The 'left' curve extrapolation approach.
        /// </summary>
        protected readonly ICurveExtrapolator m_LeftExtrapolator;

        /// <summary>The 'right' curve extrapolation approach.
        /// </summary>
        protected readonly ICurveExtrapolator m_RightExtrapolator;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="SmartReadOnlyGridPointCurve&lt;TLabel&gt;"/> class.
        /// </summary>
        /// <param name="curveInterpolator">The curve interpolator.</param>
        /// <param name="leftExtrapolator">The left extrapolator.</param>
        /// <param name="rightExtrapolator">The right extrapolator.</param>
        /// <param name="gridPointCount">The number of grid points, i.e. the number of relevant elements of <paramref name="gridPointLabels"/>, <paramref name="gridPointArguments"/> and <paramref name="gridPointValues"/> to take into account.</param>
        /// <param name="gridPointLabels">The labels of the grid points (the reference will be stored only).</param>
        /// <param name="gridPointArguments">The arguments of the grid points, thus labels of the curve in its <see cref="System.Double"/> representation.</param>
        /// <param name="gridPointValues">The values of the grid points corresponding to <paramref name="gridPointArguments"/>.</param>
        /// <param name="gridPointArgumentStartIndex">The null-based start index of <paramref name="gridPointArguments"/> and <paramref name="gridPointLabels"/> to take into account.</param>
        /// <param name="gridPointValueStartIndex">The null-based start index of <paramref name="gridPointValues"/> to take into account.</param>
        /// <param name="gridPointArgumentIncrement">The increment for <paramref name="gridPointArguments"/> and <paramref name="gridPointLabels"/>.</param>
        /// <param name="gridPointValueIncrement">The increment for <paramref name="gridPointValues"/>.</param>
        internal SmartReadOnlyGridPointCurve(ICurveDataFitting curveInterpolator, ICurveExtrapolator leftExtrapolator, ICurveExtrapolator rightExtrapolator, int gridPointCount, IList<TLabel> gridPointLabels, IList<double> gridPointArguments, IList<double> gridPointValues, int gridPointArgumentStartIndex = 0, int gridPointValueStartIndex = 0, int gridPointArgumentIncrement = 1, int gridPointValueIncrement = 1)
        {
            m_CurveInterpolator = curveInterpolator;
            m_LeftExtrapolator = leftExtrapolator;
            m_RightExtrapolator = rightExtrapolator;

            m_GridPointLabels = new SmartReadOnlyCollection<TLabel>(gridPointCount, gridPointLabels, gridPointArgumentStartIndex, gridPointArgumentIncrement);

            curveInterpolator.Update(gridPointCount, gridPointArguments, gridPointValues, GridPointCurve.State.GridPointChanged, gridPointArgumentStartIndex, gridPointValueStartIndex, gridPointArgumentIncrement = 1, gridPointValueIncrement);
            if (curveInterpolator.IsOperable == false)
            {
                throw new ArgumentException();
            }
            leftExtrapolator.Update();
            if (leftExtrapolator.IsOperable == false)
            {
                throw new ArgumentException();
            }
            rightExtrapolator.Update();
            if (rightExtrapolator.IsOperable == false)
            {
                throw new ArgumentException();
            }
        }

        /// <summary>Initializes a new instance of the <see cref="SmartReadOnlyGridPointCurve&lt;TLabel&gt;"/> class.
        /// </summary>
        /// <param name="curveInterpolator">The curve interpolator, i.e. curve parametrization.</param>
        /// <param name="gridPointCount">The number of grid points, i.e. the number of relevant elements of <paramref name="gridPointLabels"/>, <paramref name="gridPointArguments"/> and <paramref name="gridPointValues"/> to take into account.</param>
        /// <param name="gridPointLabels">The labels of the grid points (the reference will be stored only).</param>
        /// <param name="gridPointArguments">The arguments of the grid points, thus labels of the curve in its <see cref="System.Double"/> representation.</param>
        /// <param name="gridPointValues">The values of the grid points corresponding to <paramref name="gridPointArguments"/>.</param>
        /// <param name="gridPointArgumentStartIndex">The null-based start index of <paramref name="gridPointArguments"/> and <paramref name="gridPointLabels"/> to take into account.</param>
        /// <param name="gridPointValueStartIndex">The null-based start index of <paramref name="gridPointValues"/> to take into account.</param>
        /// <param name="gridPointArgumentIncrement">The increment for <paramref name="gridPointArguments"/> and <paramref name="gridPointLabels"/>.</param>
        /// <param name="gridPointValueIncrement">The increment for <paramref name="gridPointValues"/>.</param>
        internal SmartReadOnlyGridPointCurve(ICurveDataFitting curveInterpolator, int gridPointCount, IList<TLabel> gridPointLabels, IList<double> gridPointArguments, IList<double> gridPointValues, int gridPointArgumentStartIndex = 0, int gridPointValueStartIndex = 0, int gridPointArgumentIncrement = 1, int gridPointValueIncrement = 1)
            : this(curveInterpolator, GridPointCurve.Extrapolator.None.First.Create(curveInterpolator), GridPointCurve.Extrapolator.None.Last.Create(curveInterpolator), gridPointCount, gridPointLabels, gridPointArguments, gridPointValues, gridPointArgumentStartIndex, gridPointValueStartIndex, gridPointArgumentIncrement, gridPointValueIncrement)
        {
        }
        #endregion

        #region public properties

        #region IOperable Members

        /// <summary>Gets a value indicating whether this instance is operable.
        /// </summary>
        /// <value><c>true</c> if this instance is operable; otherwise, <c>false</c>.</value>
        public bool IsOperable
        {
            get { return true; }
        }
        #endregion

        #region IGridPointCurve Members

        /// <summary>Gets the number of grid points.
        /// </summary>
        /// <value>The number of grid points.</value>
        public int GridPointCount
        {
            get { return m_CurveInterpolator.GridPointCount; }
        }

        /// <summary>Gets the grid point arguments, i.e. the labels (on the x-axis) of the curve in its <see cref="System.Double"/> representation.
        /// </summary>
        /// <value>The grid point arguments.</value>
        public IList<double> GridPointArguments
        {
            get { return m_CurveInterpolator.GridPointArguments; }
        }

        /// <summary>Gets the grid point values with respect to <see cref="IGridPointCurve.GridPointArguments"/>.
        /// </summary>
        /// <value>The grid point values.</value>
        public IList<double> GridPointValues
        {
            get { return m_CurveInterpolator.GridPointValues; }
        }

        /// <summary>Gets a value indicating whether this instance is read-only.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is read-only; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly
        {
            get { return true; }
        }

        /// <summary>Gets a value indicating the fitting quality, i.e. whether grid points are meet exactly. 
        /// </summary>
        /// <value>A value indicating the fitting quality, i.e. whether grid points are meet exactly.</value>
        public FittingQuality FittingQuality
        {
            get { return m_CurveInterpolator.Factory.FittingQuality; }
        }
        #endregion

        #region IGridPointCurve<TLabel> Members

        /// <summary>Gets the labels of the grid points
        /// </summary>
        /// <value>The grid point labels.</value>
        public IList<TLabel> GridPointLabels
        {
            get { return m_GridPointLabels; }
        }
        #endregion

        #region IRealValuedCurve Members

        /// <summary>Gets the lower bound of the domain of definition.
        /// </summary>
        /// <value>The lower bound of the domain of definition, perhaps <see cref="System.Double.NegativeInfinity"/> or <see cref="System.Double.NaN"/>.</value>
        public double LowerBound
        {
            get { return (m_LeftExtrapolator != null) ? m_LeftExtrapolator.LowerBound : m_CurveInterpolator.LowerBound; }
        }

        /// <summary>Gets the upper bound of the domain of definition.
        /// </summary>
        /// <value>The upper bound of the domain of definition, perhaps <see cref="System.Double.PositiveInfinity"/> or <see cref="System.Double.NaN"/>.</value>
        public double UpperBound
        {
            get { return (m_RightExtrapolator != null) ? m_RightExtrapolator.UpperBound : m_CurveInterpolator.UpperBound; }
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

        #region IGridPointCurve<TLabel> Members

        /// <summary>Adds a specific grid point.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="argument">The argument, i.e. the <see cref="System.Double"/> representation of <paramref name="label"/>.</param>
        /// <param name="value">The value.</param>
        /// <returns>The null-based index of the grid point in the curve, the grid points should be ordered with respect to the argument.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if <see cref="IGridPointCurve.IsReadOnly"/> is <c>true</c>.</exception>
        public int Add(TLabel label, double argument, double value)
        {
            throw new InvalidOperationException();
        }

        /// <summary>Adds a collection of grid points.
        /// </summary>
        /// <param name="values">A collection of grid points, where the first component is the label, the second component is the argument, i.e. the <see cref="System.Double"/> representation of the label,
        /// and the third component of the triple is the value.</param>
        /// <param name="isSorted">A value indicating whether the <see cref="System.Double"/> representation of the labels in <paramref name="values"/> are sorted in ascending order.</param>
        /// <exception cref="InvalidOperationException">Thrown, if <see cref="IGridPointCurve.IsReadOnly"/> is <c>true</c>.</exception>
        public void AddRange(IEnumerable<Tuple<TLabel, double, double>> values, bool isSorted = false)
        {
            throw new InvalidOperationException();
        }

        /// <summary>Removes a specific grid point.
        /// </summary>
        /// <param name="label">The label of the grid point to remove.</param>
        /// <returns>A value indicating whether the grid point with respect to <paramref name="label"/> has been removed.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if <see cref="IGridPointCurve.IsReadOnly"/> is <c>true</c>.</exception>
        public bool TryRemove(TLabel label)
        {
            throw new InvalidOperationException();
        }

        /// <summary>Changes the argument of a specific grid point.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="argument">The argument, i.e. the <see cref="System.Double"/> representation of <paramref name="label"/>.</param>
        /// <returns>A value indicating whether the argument of the specific grid point has been changed to <paramref name="argument"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if <see cref="IGridPointCurve.IsReadOnly"/> is <c>true</c>.</exception>
        public bool TrySetGridPointArgument(TLabel label, double argument)
        {
            throw new InvalidOperationException();
        }

        /// <summary>Changes the value component of a specific grid point.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="value">The value.</param>
        /// <returns>A value indicating whether the <paramref name="label"/> exists in the curve and the value component has been changed to <paramref name="value"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if <see cref="IGridPointCurve.IsReadOnly"/> is <c>true</c>.</exception>
        public bool TrySetValue(TLabel label, double value)
        {
            throw new InvalidOperationException();
        }

        /// <summary>Returns a read-only <see cref="IGridPointCurve{TLabel}"/> wrapper for the current instance.
        /// </summary>
        /// <returns>A <see cref="IGridPointCurve{TLabel}"/> object that acts as a read-only wrapper around the current instance.</returns>
        public virtual IGridPointCurve<TLabel> AsReadOnly()
        {
            return this;
        }
        #endregion

        #region IGridPointCurve Members

        /// <summary>Gets the left localness level for a specific grid point, i.e.
        /// changing grid point (t_j,x_j) implies changes on the interval ]t_{j-leftLocalnessLevel}, t_{j+rightLocalnessLevel}[.
        /// </summary>
        /// <param name="gridPointIndex">The null-based index of the grid point (t_j,x_j).</param>
        /// <returns>The left localness level with respect to the grid point (t_j,x_j), where j is represented by <paramref name="gridPointIndex"/>
        /// i.e. changing grid point (t_j,x_j) implies changes on the interval ]t_{j-leftLocalnessLevel}, t_{j+rightLocalnessLevel}[.</returns>
        public int GetLeftLocalnessLevel(int gridPointIndex)
        {
            return m_CurveInterpolator.Factory.GetLeftLocalnessLevel(gridPointIndex, GridPointCount);
        }

        /// <summary>Gets the right localness level for a specific grid point, i.e.
        /// changing grid point (t_j,x_j) implies changes on the interval ]t_{j-leftLocalnessLevel}, t_{j+rightLocalnessLevel}[.
        /// </summary>
        /// <param name="gridPointIndex">The null-based index of the grid point (t_j,x_j).</param>
        /// <returns>The right localness level with respect to the grid point (t_j,x_j), where j is represented by <paramref name="gridPointIndex"/>
        /// i.e. changing grid point (t_j,x_j) implies changes on the interval ]t_{j-leftLocalnessLevel}, t_{j+rightLocalnessLevel}[.</returns>
        public int GetRightLocalnessLevel(int gridPointIndex)
        {
            return m_CurveInterpolator.Factory.GetRightLocalnessLevel(gridPointIndex, GridPointCount);
        }

        /// <summary>Removes all elements from the <see cref="IGridPointCurve"/> object.
        /// </summary>
        /// <remarks>This method set the <see cref="IOperable.IsOperable"/> flag to <c>false</c>.</remarks>
        /// <exception cref="InvalidOperationException">Thrown, if <see cref="IGridPointCurve.IsReadOnly"/> is <c>true</c>.</exception>
        public void Clear()
        {
            throw new InvalidOperationException();
        }

        /// <summary>Removes a specific grid point.
        /// </summary>
        /// <param name="gridPointIndex">The null-based index of the grid point.</param>
        /// <remarks>This method set the <see cref="IOperable.IsOperable"/> flag to <c>false</c>.</remarks>
        /// <exception cref="InvalidOperationException">Thrown, if <see cref="IGridPointCurve.IsReadOnly"/> is <c>true</c>.</exception>
        public void RemoveAt(int gridPointIndex)
        {
            throw new InvalidOperationException();
        }

        /// <summary>Changes a specific grid point argument.
        /// </summary>
        /// <param name="gridPointIndex">The null-based index of the grid point.</param>
        /// <param name="argument">The argument of the specified grid point, i.e. the <see cref="System.Double"/> representation of the label of the x-axis.</param>
        /// <remarks>This method set the <see cref="IOperable.IsOperable"/> flag to <c>false</c>.</remarks>
        /// <exception cref="InvalidOperationException">Thrown, if <see cref="IGridPointCurve.IsReadOnly"/> is <c>true</c>.</exception>
        public void SetGridPointArgument(int gridPointIndex, double argument)
        {
            throw new InvalidOperationException();
        }

        /// <summary>Changes the value component of a specific grid point.
        /// </summary>
        /// <param name="gridPointIndex">The null-based index of the grid point.</param>
        /// <param name="value">The value.</param>
        /// <remarks>This method set the <see cref="IOperable.IsOperable"/> flag to <c>false</c>.</remarks>
        /// <exception cref="InvalidOperationException">Thrown, if <see cref="IGridPointCurve.IsReadOnly"/> is <c>true</c>.</exception>
        public void SetValue(int gridPointIndex, double value)
        {
            throw new InvalidOperationException();
        }

        /// <summary>Updates the current instance. This method may change <see cref="IOperable.IsOperable"/>.
        /// </summary>
        /// <remarks>Call this method after grid points have been removed, modified or added and before trying to compute a value at a specified argument.
        /// <para>In general this method sets the <see cref="IOperable.IsOperable"/> flag to <c>true</c>.</para></remarks>
        public void Update()
        {
            // nothing to do
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
            if (pointToEvaluate < m_CurveInterpolator.LowerBound)
            {
                return m_LeftExtrapolator.GetValue(pointToEvaluate);
            }
            else if (pointToEvaluate > m_CurveInterpolator.UpperBound)
            {
                return m_RightExtrapolator.GetValue(pointToEvaluate);
            }
            return m_CurveInterpolator.GetValue(pointToEvaluate);
        }

        /// <summary>Gets the value of the integral \int_a^b f(x) dx.
        /// </summary>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="upperBound">The upper bound.</param>
        /// <returns>The value of \int_a^b f(x) dx.</returns>
        /// <remarks>The arguments must be elements of the domain of definition, represented by <see cref="IRealValuedCurve.LowerBound"/> and <see cref="IRealValuedCurve.UpperBound"/>.</remarks>
        public double GetIntegral(double lowerBound, double upperBound)
        {
            int sign = 1;
            if (upperBound > lowerBound)
            {
                sign = -1;
                var temp = upperBound;
                upperBound = lowerBound;
                lowerBound = temp;
            }

            var timeAtZero = m_CurveInterpolator.LowerBound;
            var timeAtLastPoint = m_CurveInterpolator.UpperBound;

            /* we split the integral in parts which are outside and which are inside the range: */
            if (lowerBound < timeAtZero)
            {
                if (upperBound < timeAtZero)
                {
                    return sign * m_LeftExtrapolator.GetIntegral(lowerBound, upperBound);
                }
                double integralValue = m_LeftExtrapolator.GetIntegral(lowerBound, timeAtZero);
                if (upperBound < timeAtLastPoint)
                {
                    return sign * (integralValue + m_CurveInterpolator.GetIntegral(timeAtZero, upperBound));
                }
                return sign * (integralValue + m_CurveInterpolator.GetIntegral(timeAtZero, timeAtLastPoint) + m_RightExtrapolator.GetIntegral(timeAtLastPoint, upperBound));
            }
            else if (lowerBound > timeAtLastPoint)
            {
                return sign * m_RightExtrapolator.GetIntegral(lowerBound, upperBound);  /* here we assume lowerBound < upperBound */
            }

            if (upperBound <= timeAtLastPoint)
            {
                return sign * m_CurveInterpolator.GetIntegral(lowerBound, upperBound);
            }
            return sign * (m_CurveInterpolator.GetIntegral(lowerBound, timeAtLastPoint) + m_RightExtrapolator.GetIntegral(timeAtLastPoint, upperBound));
        }
        #endregion

        #region IInfoOutputQueriable Members

        /// <summary>Gets informations of the current object as a specific <see cref="InfoOutput" /> instance.
        /// </summary>
        /// <param name="infoOutput">The <see cref="InfoOutput" /> object which is to be filled with informations concering the current instance.</param>
        /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
        public void FillInfoOutput(InfoOutput infoOutput, string categoryName = "General")
        {
            var infoOutputPackage = infoOutput.AcquirePackage(categoryName);
            infoOutputPackage.Add("Count", GridPointCount);
            infoOutputPackage.Add("Is read-only", IsReadOnly);

            m_CurveInterpolator.FillInfoOutput(infoOutput, categoryName + ".Interpolator");
            m_LeftExtrapolator.FillInfoOutput(infoOutput, categoryName + ".Extrapolator.Left");
            m_RightExtrapolator.FillInfoOutput(infoOutput, categoryName + ".Extrapolator.Right");
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