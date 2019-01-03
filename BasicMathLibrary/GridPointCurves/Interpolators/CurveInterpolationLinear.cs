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

namespace Dodoni.MathLibrary.GridPointCurves
{
    /// <summary>Represents the linear curve interpolator.
    /// </summary>
    internal class CurveInterpolationLinear : GridPointCurve.Interpolator
    {
        #region nested classes

        /// <summary>Serves as implementation of the interpolation approach.
        /// </summary>
        private class Interpolator : ICurveDataFitting
        {
            #region private members

            /// <summary>The number of grid points.
            /// </summary>
            private int m_GridPointCount;

            /// <summary>The arguments of the grid points, i.e. the labels on the x-axis.
            /// </summary>
            private double[] m_GridPointArguments;

            /// <summary>The read-only wrapper of the grid points, i.e. the labels of the x-axis.
            /// </summary>
            private ReadOnlyCollection<double> m_ReadOnlyGridPointArguments;

            /// <summary>The values of the grid points.
            /// </summary>
            private double[] m_GridPointValues;

            /// <summary>The read-only wrapper of grid point values.
            /// </summary>
            private ReadOnlyCollection<double> m_ReadOnlyGridPointValues;

            /// <summary>Represents a cache for the calculation of a specific integral.
            /// </summary>
            private CurveInterpolatorIntegralCache m_IntegralCache;
            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="Interpolator"/> class.
            /// </summary>
            /// <param name="interpolationFactory">The <see cref="CurveInterpolationLinear"/> object that serves as factory for the current object.</param>
            internal Interpolator(CurveInterpolationLinear interpolationFactory)
            {
                m_GridPointCount = 0;
                Factory = interpolationFactory;
                m_IntegralCache = new CurveInterpolatorIntegralCache(this);
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
                get;
                private set;
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
                if (gridPointCount <= 0)
                {
                    m_GridPointCount = 0;  // current instance is not operable
                }
                else
                {
                    m_GridPointCount = gridPointCount;

                    if (state.HasFlag(GridPointCurve.State.GridPointArgumentChanged))
                    {
                        if (ArrayMemory.Reallocate(ref m_GridPointArguments, gridPointCount, Math.Max(10, gridPointCount / 5)) == true)
                        {
                            m_ReadOnlyGridPointArguments = new ReadOnlyCollection<double>(m_GridPointArguments);
                        }
                        gridPointArguments.CopyTo(m_GridPointArguments, gridPointCount, gridPointArgumentStartIndex, sourceIncrement: gridPointArgumentIncrement);
                    }
                    if (state.HasFlag(GridPointCurve.State.GridPointValueChanged))
                    {
                        if (ArrayMemory.Reallocate(ref m_GridPointValues, gridPointCount, Math.Max(10, gridPointCount / 5)) == true)
                        {
                            m_ReadOnlyGridPointValues = new ReadOnlyCollection<double>(m_GridPointValues);
                        }
                        gridPointValues.CopyTo(m_GridPointValues, gridPointCount, gridPointValueStartIndex, sourceIncrement: gridPointValueIncrement);
                    }
                }
                m_IntegralCache.EarmarkUpdateRequest();
            }

            /// <summary>Gets the value of the integral \int_a^b f(x) dx inside two specific grid points.
            /// </summary>
            /// <param name="lowerBound">The lower bound; between the grid point arguments specified by <paramref name="leftGridPointIndex" /> and <paramref name="leftGridPointIndex" /> + 1.</param>
            /// <param name="upperBound">The upper bound; between the grid point arguments specified by <paramref name="leftGridPointIndex" /> and <paramref name="leftGridPointIndex" /> + 1.</param>
            /// <param name="leftGridPointIndex">The null-based index of the left grid point index.</param>
            /// <returns>The value of \int_a^b f(x) dx.</returns>
            public double GetIntegral(double lowerBound, double upperBound, int leftGridPointIndex)
            {
                var x0 = m_GridPointArguments[leftGridPointIndex];
                var x1 = m_GridPointArguments[leftGridPointIndex + 1];
                var y0 = m_GridPointValues[leftGridPointIndex];
                var y1 = m_GridPointValues[leftGridPointIndex + 1];

                var m = (y1 - y0) / (x1 - x0);
                var b = y0 - m * x0;

                return upperBound * (0.5 * upperBound * m + b) - lowerBound * (0.5 * lowerBound * m + b);
            }
            #endregion

            #region IRealValuedCurve Members

            /// <summary>Gets the value at a specific argument.
            /// </summary>
            /// <param name="pointToEvaluate">The point to evaluate.</param>
            /// <returns>The value of the curve at <paramref name="pointToEvaluate"/>, i.e. 
            /// <para>
            ///  m * (x - x1) + y1 where  m = (y2 - y1) / (x2 -x1).
            /// </para></returns>
            /// <remarks>The argument must be an element of the domain of definition, represented by <see cref="IRealValuedCurve.LowerBound"/> and <see cref="IRealValuedCurve.UpperBound"/>.</remarks>
            public double GetValue(double pointToEvaluate)
            {
                int nonLastLeftGridIndex = Array.BinarySearch(m_GridPointArguments, 0, m_GridPointCount, pointToEvaluate);
                if (nonLastLeftGridIndex < 0)
                {
                    nonLastLeftGridIndex = (~nonLastLeftGridIndex) - 1; // left-neigbour grid point or the last but one grid point index
                }
                else if (nonLastLeftGridIndex == m_GridPointCount - 1)
                {
                    return m_GridPointValues[nonLastLeftGridIndex];
                }

                double xDifference = m_GridPointArguments[nonLastLeftGridIndex + 1] - m_GridPointArguments[nonLastLeftGridIndex];

                if (xDifference > MachineConsts.Epsilon)
                {
                    double m = (m_GridPointValues[nonLastLeftGridIndex + 1] - m_GridPointValues[nonLastLeftGridIndex]) / xDifference;
                    return m * (pointToEvaluate - m_GridPointArguments[nonLastLeftGridIndex]) + m_GridPointValues[nonLastLeftGridIndex];
                }
                throw new ArithmeticException("Linear interpolation failed.");
            }            

            /// <summary>Gets the value of the integral \int_a^b f(x) dx.
            /// </summary>
            /// <param name="lowerBound">The lower bound.</param>
            /// <param name="upperBound">The upper bound.</param>
            /// <returns>The value of \int_a^b f(x) dx.</returns>
            /// <remarks>The arguments must be elements of the domain of definition, represented by <see cref="IRealValuedCurve.LowerBound"/> and <see cref="IRealValuedCurve.UpperBound"/>.</remarks>
            public double GetIntegral(double lowerBound, double upperBound)
            {
                return m_IntegralCache.GetIntegral(lowerBound, upperBound);
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

                infoOutputPackage.Add("Count", m_GridPointCount);

                DataTable gridPointTable = new DataTable("Grid points");
                gridPointTable.Columns.Add("Argument", typeof(double));
                gridPointTable.Columns.Add("Value", typeof(double));

                for (int j = 0; j < m_GridPointCount; j++)
                {
                    var row = gridPointTable.NewRow();
                    row[0] = m_GridPointArguments[j];
                    row[1] = m_GridPointValues[j];
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

            #endregion
        }
        #endregion

        #region private members

        /// <summary>The name of the curve interpolation.
        /// </summary>
        private IdentifierString m_Name;

        /// <summary>The long name of the curve interpolation.
        /// </summary>
        private IdentifierString m_LongName;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="CurveInterpolationLinear"/> class.
        /// </summary>
        internal CurveInterpolationLinear()
            : base(CurveResource.AnnotationInterpolationLinear)
        {
            m_Name = new IdentifierString("Linear");
            m_LongName = new IdentifierString(CurveResource.LongNameInterpolationLinear);
        }
        #endregion

        #region public properties

        /// <summary>Gets a value indicating whether this instance represents a local approach.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is local approach; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>In the case of a local approach call <see cref="GetLeftLocalnessLevel(int, int)"/> and <see cref="GetRightLocalnessLevel(int, int)"/>
        /// for the left and right localness level.
        /// <para>In the case of a global approach all grid points are required for the curve interpolation.</para></remarks>
        public override bool IsLocalApproach
        {
            get { return true; }
        }
        #endregion

        #region public methods

        /// <summary>Creates a <see cref="ICurveDataFitting"/> object that represents the implementation of the interpolation approach.
        /// </summary>
        /// <returns>A <see cref="ICurveDataFitting"/> object that represents the implementation of the interpolation approach.</returns>
        public override ICurveDataFitting Create()
        {
            return new Interpolator(this);
        }

        /// <summary>Gets the left localness level for a specific grid point, i.e.
        /// changing grid point (t_j,x_j) implies changes on the interval [t_{j-leftLocalnessLevel}, t_{j+rightLocalnessLevel}].
        /// </summary>
        /// <param name="gridPointIndex">The null-based index of the grid point (t_j,x_j).</param>
        /// <param name="gridPointCount">The number of grid points.</param>
        /// <returns>
        /// The left localness level with respect to the grid point (t_j,x_j), where j is represented by <paramref name="gridPointIndex"/>
        /// i.e. changing grid point (t_j,x_j) implies changes on the interval [t_{j-leftLocalnessLevel}, t_{j+rightLocalnessLevel}].
        /// </returns>
        /// <remarks>The localness level does not depend on the value of the grid point itself.</remarks>
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
        /// The right localness level with respect to the grid point (t_j,x_j), where j is represented by <paramref name="gridPointIndex"/>
        /// i.e. changing grid point (t_j,x_j) implies changes on the interval [t_{j-leftLocalnessLevel}, t_{j+rightLocalnessLevel}].
        /// </returns>
        /// <remarks>The localness level does not depend on the value of the grid point itself.</remarks>
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