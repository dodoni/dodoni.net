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
using System.Collections.ObjectModel;

using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.GridPointCurves
{
    /// <summary>Serves as standard implementation of a grid point curve without labels, i.e. the labels are identical to the arguments of the grid points.
    /// </summary>
    internal class StandardGridPointCurveNoLabels : IGridPointCurve<double>
    {
        #region nested classes

        /// <summary>Serves as standard implementation of a differentiable grid point curve.
        /// </summary>
        internal class Differentiable : StandardGridPointCurveNoLabels, IDifferentiableRealValuedCurve
        {
            #region internal/protected constructors

            /// <summary>Initializes a new instance of the <see cref="Differentiable"/> class.
            /// </summary>
            /// <param name="curveInterpolatorFactory">The curve interpolator factory.</param>
            /// <param name="curveInterpolator">The curve interpolator.</param>
            /// <param name="leftExtrapolatorFactory">The left extrapolator factory.</param>
            /// <param name="leftExtrapolator">The left extrapolator.</param>
            /// <param name="rightExtrapolatorFactory">The right extrapolator factory.</param>
            /// <param name="rightExtrapolator">The right extrapolator.</param>
            /// <param name="capacity">The number of elements that the new grid point curve can initially store.</param>
            internal Differentiable(GridPointCurve.Interpolator curveInterpolatorFactory, ICurveDataFitting curveInterpolator, GridPointCurve.Extrapolator leftExtrapolatorFactory, ICurveExtrapolator leftExtrapolator, GridPointCurve.Extrapolator rightExtrapolatorFactory, ICurveExtrapolator rightExtrapolator, int capacity)
                : base(curveInterpolatorFactory, curveInterpolator, leftExtrapolatorFactory, leftExtrapolator, rightExtrapolatorFactory, rightExtrapolator, capacity)
            {
            }

            /// <summary>Initializes a new instance of the <see cref="Differentiable"/> class.
            /// </summary>
            /// <param name="curveParametrizationFactory">The curve parametrization factory.</param>
            /// <param name="curveParametrization">The curve parametrization.</param>
            /// <param name="capacity">The number of elements that the new grid point curve can initially store.</param>
            internal Differentiable(GridPointCurve.Parametrization curveParametrizationFactory, ICurveDataFitting curveParametrization, int capacity)
                : base(curveParametrizationFactory, curveParametrization, capacity)
            {
            }
            #endregion

            #region public methods

            /// <summary>Returns a read-only <see cref="IGridPointCurve{TLabel}"/> wrapper for the current instance.
            /// </summary>
            /// <returns>A <see cref="IGridPointCurve{TLabel}"/> object that acts as a read-only wrapper around the current instance.</returns>
            public override IGridPointCurve<double> AsReadOnly()
            {
                return new SmartReadOnlyGridPointCurve<double>.Differentiable(m_CurveBuilder, m_LeftExtrapolator, m_RightExtrapolator, GridPointCount, m_GridPointArguments.ToArray(), m_GridPointArguments, m_GridPointValues);
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
                if (pointToEvaluate < m_CurveBuilder.LowerBound)
                {
                    return ((IDifferentiableRealValuedCurve)m_LeftExtrapolator).GetDerivative(pointToEvaluate);
                }
                else if (pointToEvaluate > m_CurveBuilder.UpperBound)
                {
                    return ((IDifferentiableRealValuedCurve)m_RightExtrapolator).GetDerivative(pointToEvaluate);
                }
                return ((IDifferentiableRealValuedCurve)m_CurveBuilder).GetDerivative(pointToEvaluate);
            }
            #endregion
        }
        #endregion

        #region private/protected members

        /// <summary>The state of the grid point curve.
        /// </summary>
        private GridPointCurve.State m_State;

        /// <summary>The arguments of the grid points (and the labels), i.e. the x-components of the grid points. 
        /// </summary>
        private List<double> m_GridPointArguments;

        /// <summary>The values of the grid points, i.e. the y-components of the grid points.
        /// </summary>
        private List<double> m_GridPointValues;

        /// <summary>The curve builder approach (interpolator/parametrization).
        /// </summary>
        protected readonly ICurveDataFitting m_CurveBuilder;

        /// <summary>The 'left' curve extrapolation (truncation) approach.
        /// </summary>
        protected readonly ICurveExtrapolator m_LeftExtrapolator;

        /// <summary>The 'right' curve extrapolation (truncation) approach.
        /// </summary>
        protected readonly ICurveExtrapolator m_RightExtrapolator;

        /// <summary>The read-only wrapper of the grid point arguments/labels.
        /// </summary>
        /// <remarks>This member is used for performance reason only.</remarks>
        private ReadOnlyCollection<double> m_ReadOnlyGridPointArguments;

        /// <summary>The read-only wrapper of the grid point values.
        /// </summary>
        /// <remarks>This member is used for performance reason only.</remarks>
        private ReadOnlyCollection<double> m_ReadOnlyGridPointValues;
        #endregion

        #region internal/protected constructors

        /// <summary>Initializes a new instance of the <see cref="StandardGridPointCurveNoLabels"/> class.
        /// </summary>
        /// <param name="curveInterpolatorFactory">The curve interpolator factory.</param>
        /// <param name="curveInterpolator">The curve interpolator.</param>
        /// <param name="leftExtrapolatorFactory">The left extrapolator factory.</param>
        /// <param name="leftExtrapolator">The left extrapolator.</param>
        /// <param name="rightExtrapolatorFactory">The right extrapolator factory.</param>
        /// <param name="rightExtrapolator">The right extrapolator.</param>
        /// <param name="capacity">The number of elements that the new grid point curve can initially store.</param>
        internal StandardGridPointCurveNoLabels(GridPointCurve.Interpolator curveInterpolatorFactory, ICurveDataFitting curveInterpolator, GridPointCurve.Extrapolator leftExtrapolatorFactory, ICurveExtrapolator leftExtrapolator, GridPointCurve.Extrapolator rightExtrapolatorFactory, ICurveExtrapolator rightExtrapolator, int capacity = 20)
        {
            m_GridPointArguments = new List<double>(capacity);
            m_GridPointValues = new List<double>(capacity);

            m_CurveBuilder = curveInterpolator;
            m_LeftExtrapolator = leftExtrapolator;
            m_RightExtrapolator = rightExtrapolator;

            m_State = GridPointCurve.State.GridPointChanged;

            m_ReadOnlyGridPointValues = new ReadOnlyCollection<double>(m_GridPointValues);
            m_ReadOnlyGridPointArguments = new ReadOnlyCollection<double>(m_GridPointArguments);
        }

        /// <summary>Initializes a new instance of the <see cref="StandardGridPointCurveNoLabels"/> class.
        /// </summary>
        /// <param name="curveParametrizationFactory">The curve parametrization factory.</param>
        /// <param name="curveParametrization">The curve parametrization.</param>
        /// <param name="capacity">The number of elements that the new grid point curve can initially store.</param>
        internal StandardGridPointCurveNoLabels(GridPointCurve.Parametrization curveParametrizationFactory, ICurveDataFitting curveParametrization, int capacity = 20)
        {
            m_GridPointArguments = new List<double>(capacity);
            m_GridPointValues = new List<double>(capacity);

            m_CurveBuilder = curveParametrization;

            // do not apply any truncation (extrapolation) approach:
            m_LeftExtrapolator = GridPointCurve.Extrapolator.None.First.Create(curveParametrization);
            m_RightExtrapolator = GridPointCurve.Extrapolator.None.Last.Create(curveParametrization);

            m_ReadOnlyGridPointValues = new ReadOnlyCollection<double>(m_GridPointValues);
            m_ReadOnlyGridPointArguments = new ReadOnlyCollection<double>(m_GridPointArguments);

            m_State = GridPointCurve.State.GridPointChanged;
        }
        #endregion

        #region public properties

        #region IOperable Members

        /// <summary>Gets a value indicating whether this instance is operable.
        /// </summary>
        /// <value><c>true</c> if this instance is operable; otherwise, <c>false</c>.</value>
        public bool IsOperable
        {
            get { return ((m_State == GridPointCurve.State.NoChangeSinceLastUpdate) && (m_GridPointArguments.Count >= 2) && m_CurveBuilder.IsOperable && m_LeftExtrapolator.IsOperable && m_RightExtrapolator.IsOperable); }
        }
        #endregion

        #region IGridPointCurve Members

        /// <summary>Gets the number of grid points.
        /// </summary>
        /// <value>The number of grid points.</value>
        public int GridPointCount
        {
            get { return m_GridPointArguments.Count; }
        }

        /// <summary>Gets the grid point arguments, i.e. the labels (on the x-axis) of the curve in its <see cref="System.Double"/> representation.
        /// </summary>
        /// <value>The grid point arguments.</value>
        public IList<double> GridPointArguments
        {
            get { return m_ReadOnlyGridPointArguments; }
        }

        /// <summary>Gets the grid point values with respect to <see cref="IGridPointCurve.GridPointArguments"/>.
        /// </summary>
        /// <value>The grid point values.</value>
        public IList<double> GridPointValues
        {
            get { return m_ReadOnlyGridPointValues; }
        }

        /// <summary>Gets a value indicating whether this instance is read-only.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is read-only; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>Gets a value indicating the fitting quality, i.e. whether grid points are meet exactly. 
        /// </summary>
        /// <value>A value indicating the fitting quality, i.e. whether grid points are meet exactly.</value>
        public FittingQuality FittingQuality
        {
            get { return m_CurveBuilder.Factory.FittingQuality; }
        }
        #endregion

        #region IGridPointCurve<TLabel> Members

        /// <summary>Gets the labels of the grid points
        /// </summary>
        /// <value>The grid point labels.</value>
        public IList<double> GridPointLabels
        {
            get { return m_ReadOnlyGridPointArguments; }
        }
        #endregion

        #region IRealValuedCurve Members

        /// <summary>Gets the lower bound of the domain of definition.
        /// </summary>
        /// <value>The lower bound of the domain of definition, perhaps <see cref="System.Double.NegativeInfinity"/> or <see cref="System.Double.NaN"/>.</value>
        public double LowerBound
        {
            get { return m_LeftExtrapolator.LowerBound; }
        }

        /// <summary>Gets the upper bound of the domain of definition.
        /// </summary>
        /// <value>The upper bound of the domain of definition, perhaps <see cref="System.Double.PositiveInfinity"/> or <see cref="System.Double.NaN"/>.</value>
        public double UpperBound
        {
            get { return m_RightExtrapolator.UpperBound; }
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
        public int Add(double label, double argument, double value)
        {
            int index = m_GridPointArguments.BinarySearch(argument);
            if (index >= 0)
            {
                throw new ArgumentException();
            }
            index = (~index);

            m_GridPointArguments.Insert(index, argument);
            m_GridPointValues.Insert(index, value);
            m_State = GridPointCurve.State.GridPointChanged;
            return index;
        }

        /// <summary>Adds a collection of grid points.
        /// </summary>
        /// <param name="values">A collection of grid points, where the first component is the label, the second component is the argument, i.e. the <see cref="System.Double"/> representation of the label,
        /// and the third component of the triple is the value.</param>
        /// <param name="isSorted">A value indicating whether the <see cref="System.Double"/> representation of the labels in <paramref name="values"/> are sorted in ascending order.</param>
        /// <exception cref="InvalidOperationException">Thrown, if <see cref="IGridPointCurve.IsReadOnly"/> is <c>true</c>.</exception>
        public void AddRange(IEnumerable<Tuple<double, double, double>> values, bool isSorted = false)
        {
            if (values == null)
            {
                throw new ArgumentNullException();
            }
            if ((isSorted == true) && (m_GridPointArguments.Count == 0))
            {
                foreach (var gridPoint in values)
                {
                    m_GridPointArguments.Add(gridPoint.Item2);
                    m_GridPointValues.Add(gridPoint.Item3);
                }
            }
            else
            {
                foreach (var gridPoint in values)
                {
                    Add(gridPoint.Item1, gridPoint.Item2, gridPoint.Item3);
                }
            }
        }

        /// <summary>Removes a specific grid point.
        /// </summary>
        /// <param name="label">The label of the grid point to remove.</param>
        /// <returns>A value indicating whether the grid point with respect to <paramref name="label"/> has been removed.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if <see cref="IGridPointCurve.IsReadOnly"/> is <c>true</c>.</exception>
        public bool TryRemove(double label)
        {
            int labelIndex = m_GridPointArguments.FindIndex(match => match.Equals(label));
            if (labelIndex >= 0)
            {
                m_GridPointArguments.RemoveAt(labelIndex);
                m_GridPointValues.RemoveAt(labelIndex);
                m_State = GridPointCurve.State.GridPointChanged;
                return true;
            }
            return false;
        }

        /// <summary>Changes the argument of a specific grid point.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="argument">The argument, i.e. the <see cref="System.Double"/> representation of <paramref name="label"/>.</param>
        /// <returns>A value indicating whether the argument of the specific grid point has been changed to <paramref name="argument"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if <see cref="IGridPointCurve.IsReadOnly"/> is <c>true</c>.</exception>
        public bool TrySetGridPointArgument(double label, double argument)
        {
            int labelIndex = m_GridPointArguments.FindIndex(match => match.Equals(label));
            if (labelIndex >= 0)
            {
                SetGridPointArgument(labelIndex, argument);
                return true;
            }
            return false;
        }

        /// <summary>Changes the value component of a specific grid point.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="value">The value.</param>
        /// <returns>A value indicating whether the <paramref name="label"/> exists in the curve and the value component has been changed to <paramref name="value"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if <see cref="IGridPointCurve.IsReadOnly"/> is <c>true</c>.</exception>
        public bool TrySetValue(double label, double value)
        {
            int labelIndex = m_GridPointArguments.FindIndex(match => match.Equals(label));
            if (labelIndex >= 0)
            {
                m_GridPointValues[labelIndex] = value;
                m_State |= GridPointCurve.State.GridPointValueChanged;
                return true;
            }
            return false;
        }

        /// <summary>Returns a read-only <see cref="IGridPointCurve{TLabel}"/> wrapper for the current instance.
        /// </summary>
        /// <returns>A <see cref="IGridPointCurve{TLabel}"/> object that acts as a read-only wrapper around the current instance.</returns>
        public virtual IGridPointCurve<double> AsReadOnly()
        {
            return new SmartReadOnlyGridPointCurve<double>(m_CurveBuilder, m_LeftExtrapolator, m_RightExtrapolator, GridPointCount, m_GridPointArguments.ToArray(), m_GridPointArguments, m_GridPointValues);
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
            return m_CurveBuilder.Factory.GetLeftLocalnessLevel(gridPointIndex, GridPointCount);
        }

        /// <summary>Gets the right localness level for a specific grid point, i.e.
        /// changing grid point (t_j,x_j) implies changes on the interval ]t_{j-leftLocalnessLevel}, t_{j+rightLocalnessLevel}[.
        /// </summary>
        /// <param name="gridPointIndex">The null-based index of the grid point (t_j,x_j).</param>
        /// <returns>The right localness level with respect to the grid point (t_j,x_j), where j is represented by <paramref name="gridPointIndex"/>
        /// i.e. changing grid point (t_j,x_j) implies changes on the interval ]t_{j-leftLocalnessLevel}, t_{j+rightLocalnessLevel}[.</returns>
        public int GetRightLocalnessLevel(int gridPointIndex)
        {
            return m_CurveBuilder.Factory.GetRightLocalnessLevel(gridPointIndex, GridPointCount);
        }

        /// <summary>Removes all elements from the <see cref="IGridPointCurve"/> object.
        /// </summary>
        /// <remarks>This method set the <see cref="IOperable.IsOperable"/> flag to <c>false</c>.</remarks>
        /// <exception cref="InvalidOperationException">Thrown, if <see cref="IGridPointCurve.IsReadOnly"/> is <c>true</c>.</exception>
        public void Clear()
        {
            m_GridPointArguments.Clear();
            m_GridPointValues.Clear();
            m_State = GridPointCurve.State.GridPointChanged;
        }

        /// <summary>Removes a specific grid point.
        /// </summary>
        /// <param name="gridPointIndex">The null-based index of the grid point.</param>
        /// <remarks>This method set the <see cref="IOperable.IsOperable"/> flag to <c>false</c>.</remarks>
        /// <exception cref="InvalidOperationException">Thrown, if <see cref="IGridPointCurve.IsReadOnly"/> is <c>true</c>.</exception>
        public void RemoveAt(int gridPointIndex)
        {
            m_GridPointArguments.RemoveAt(gridPointIndex);
            m_GridPointValues.RemoveAt(gridPointIndex);

            m_State = GridPointCurve.State.GridPointChanged;
        }

        /// <summary>Changes a specific grid point argument.
        /// </summary>
        /// <param name="gridPointIndex">The null-based index of the grid point.</param>
        /// <param name="argument">The argument of the specified grid point, i.e. the <see cref="System.Double"/> representation of the label of the x-axis.</param>
        /// <remarks>This method set the <see cref="IOperable.IsOperable"/> flag to <c>false</c>.</remarks>
        /// <exception cref="InvalidOperationException">Thrown, if <see cref="IGridPointCurve.IsReadOnly"/> is <c>true</c>.</exception>
        public void SetGridPointArgument(int gridPointIndex, double argument)
        {
            /* If a re-ordering of the grid points is necessary we first remove the grid point and re-insert the 'new' a the specific position: */
            if (GridPointCount == 0)
            {
                throw new InvalidOperationException();
            }
            if ((gridPointIndex < 0) || (gridPointIndex > GridPointCount - 1))
            {
                throw new IndexOutOfRangeException();
            }
            int previousGridPointIndex = Math.Max(0, gridPointIndex - 1);
            int nextGridPointIndex = Math.Min(gridPointIndex + 1, GridPointCount - 1);
            if ((m_GridPointArguments[previousGridPointIndex] <= argument) && (argument <= m_GridPointArguments[nextGridPointIndex]))
            {
                m_GridPointArguments[gridPointIndex] = argument;
                m_State |= GridPointCurve.State.GridPointArgumentChanged;
            }
            else
            {
                var value = m_GridPointValues[gridPointIndex];

                RemoveAt(gridPointIndex);
                Add(argument, argument, value);
            }
        }

        /// <summary>Changes the value component of a specific grid point.
        /// </summary>
        /// <param name="gridPointIndex">The null-based index of the grid point.</param>
        /// <param name="value">The value.</param>
        /// <remarks>This method set the <see cref="IOperable.IsOperable"/> flag to <c>false</c>.</remarks>
        /// <exception cref="InvalidOperationException">Thrown, if <see cref="IGridPointCurve.IsReadOnly"/> is <c>true</c>.</exception>
        public void SetValue(int gridPointIndex, double value)
        {
            m_GridPointValues[gridPointIndex] = value;
            m_State |= GridPointCurve.State.GridPointValueChanged;
        }

        /// <summary>Updates the current instance. This method may change <see cref="IOperable.IsOperable"/>.
        /// </summary>
        /// <remarks>Call this method after grid points have been removed, modified or added and before trying to compute a value at a specified argument.
        /// <para>In general this method sets the <see cref="IOperable.IsOperable"/> flag to <c>true</c>.</para></remarks>
        public void Update()
        {
            m_CurveBuilder.Update(m_GridPointArguments.Count, m_GridPointArguments, m_GridPointValues, m_State);
            m_LeftExtrapolator.Update();
            m_RightExtrapolator.Update();

            m_State = GridPointCurve.State.NoChangeSinceLastUpdate;
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
            if (pointToEvaluate < m_CurveBuilder.LowerBound)
            {
                return m_LeftExtrapolator.GetValue(pointToEvaluate);
            }
            else if (pointToEvaluate > m_CurveBuilder.UpperBound)
            {
                return m_RightExtrapolator.GetValue(pointToEvaluate);
            }
            return m_CurveBuilder.GetValue(pointToEvaluate);
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

            var curveBuilderLowerBound = m_CurveBuilder.LowerBound;
            var curveBuilderUpperBound = m_CurveBuilder.UpperBound;

            /* we split the integral in parts which are outside and which are inside the range: */
            if (lowerBound < curveBuilderLowerBound)
            {
                if (upperBound < curveBuilderLowerBound)
                {
                    return sign * m_LeftExtrapolator.GetIntegral(lowerBound, upperBound);
                }
                double integralValue = m_LeftExtrapolator.GetIntegral(lowerBound, curveBuilderLowerBound);
                if (upperBound < curveBuilderUpperBound)
                {
                    return sign * (integralValue + m_CurveBuilder.GetIntegral(curveBuilderLowerBound, upperBound));
                }
                return sign * (integralValue + m_CurveBuilder.GetIntegral(curveBuilderLowerBound, curveBuilderUpperBound) + m_RightExtrapolator.GetIntegral(curveBuilderUpperBound, upperBound));
            }
            else if (lowerBound > curveBuilderUpperBound)
            {
                return sign * m_RightExtrapolator.GetIntegral(lowerBound, upperBound);  /* here we assume lowerBound < upperBound */
            }

            if (upperBound <= curveBuilderUpperBound)
            {
                return sign * m_CurveBuilder.GetIntegral(lowerBound, upperBound);
            }
            return sign * (m_CurveBuilder.GetIntegral(lowerBound, curveBuilderUpperBound) + m_RightExtrapolator.GetIntegral(curveBuilderUpperBound, upperBound));
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

            if (m_CurveBuilder.Factory is GridPointCurve.Interpolator)
            {
                m_CurveBuilder.FillInfoOutput(infoOutput, categoryName + ".Interpolator");
                m_LeftExtrapolator.FillInfoOutput(infoOutput, categoryName + ".Extrapolator.Left");
                m_RightExtrapolator.FillInfoOutput(infoOutput, categoryName + ".Extrapolator.Right");
            }
            else
            {
                m_CurveBuilder.FillInfoOutput(infoOutput, categoryName + ".Parametrization");
                m_LeftExtrapolator.FillInfoOutput(infoOutput, categoryName + ".Truncation.Left");
                m_RightExtrapolator.FillInfoOutput(infoOutput, categoryName + ".Truncation.Right");
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

        /// <summary>Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return String.Format("{0};{1};{2}", m_CurveBuilder.Factory.Name.String, m_LeftExtrapolator.Factory.Name.String, m_RightExtrapolator.Factory.Name.String);
        }
        #endregion
    }
}