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
    /// <summary>Evaluates a cubic spline for specific coefficients, i.e. computes the integral, derivative etc. of a specific cubic spline.
    /// </summary>
    /// <remarks>Class does not contain the specific calculation of spline coefficients itself. This depends on boundary conditions etc.</remarks>
    public class CubicSplineEvaluator : IOperable, IInfoOutputQueriable
    {
        #region private members

        /// <summary>The number of grid points.
        /// </summary>
        private int m_GridPointCount;

        /// <summary>The grid point arguments, i.e. the x-component of the grid points.
        /// </summary>
        private double[] m_GridPointArguments;

        /// <summary>The values with respect to the <see cref="m_GridPointArguments"/>, moreover the 
        /// coefficients 'a' with respect to f(t) = a[j] + b[j]*(t[j] - t) + c[j] * (t[j] - t)^2 + d[j] * (t[j] - t)^3.
        /// </summary>
        private double[] m_GridPointValues;

        /// <summary>The coefficients 'b' with respect to f(t) = a[j] + b[j]*(t[j] - t) + c[j] * (t[j] - t)^2 + d[j] * (t[j] - t)^3.
        /// </summary>
        private double[] m_CoefficientsB;

        /// <summary>The coefficients 'c' with respect to f(t) = a[j] + b[j]*(t[j] - t) + c[j] * (t[j] - t)^2 + d[j] * (t[j] - t)^3.
        /// </summary>
        private double[] m_CoefficientsC;

        /// <summary>The coefficients 'd' with respect to f(t) = a[j] + b[j]*(t[j] - t) + c[j] * (t[j] - t)^2 + d[j] * (t[j] - t)^3.
        /// </summary>
        private double[] m_CoefficientsD;

        /// <summary>The internal cache, i.e. \int_{t_0}^{t_j} f(x) dx for j=0,1,...n.
        /// </summary>
        private double[] m_IntegralCache;

        /// <summary>The read-only wrapper of the grid points, i.e. the labels of the x-axis.
        /// </summary>
        private ReadOnlyCollection<double> m_ReadOnlyGridPointArguments;

        /// <summary>The read-only wrapper of grid point values.
        /// </summary>
        private ReadOnlyCollection<double> m_ReadOnlyGridPointValues;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="CubicSplineEvaluator"/> class.
        /// </summary>
        /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
        public CubicSplineEvaluator(InfoOutputDetailLevel infoOutputDetailLevel = InfoOutputDetailLevel.Full)
        {
            InfoOutputDetailLevel = infoOutputDetailLevel;
            IntegralCacheUpdateRequested = true;
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
            get { return ((m_GridPointCount >= 2) && (m_GridPointArguments != null) && (m_GridPointValues != null) && (m_CoefficientsB != null) && (m_CoefficientsB.Length >= m_GridPointCount - 1)); }
        }
        #endregion

        #region IInfoOutputQueriable Members

        /// <summary>Gets the info-output level of detail.
        /// </summary>
        /// <value>The info-output level of detail.</value>
        public InfoOutputDetailLevel InfoOutputDetailLevel
        {
            get;
            private set;
        }
        #endregion

        /// <summary>Gets the number of grid points.
        /// </summary>
        /// <value>The number of grid points.</value>
        public int Count
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
        #endregion

        #region protected properties

        /// <summary>Gets a value indicating whether an update of the cache for integration is required.
        /// </summary>
        /// <value><c>true</c> if an update of the cache for integration is requested; otherwise, <c>false</c>.</value>
        protected bool IntegralCacheUpdateRequested
        {
            get;
            private set;
        }
        #endregion

        #region public methods

        #region IInfoOutputQueriable Members

        /// <summary>Sets the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> property.
        /// </summary>
        /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
        /// <returns>A value indicating whether the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> has been set to <paramref name="infoOutputDetailLevel"/>.</returns>
        public bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
        {
            InfoOutputDetailLevel = infoOutputDetailLevel;
            return true;
        }

        /// <summary>Gets informations of the current object as a specific <see cref="InfoOutput" /> instance.
        /// </summary>
        /// <param name="infoOutput">The <see cref="InfoOutput" /> object which is to be filled with informations concering the current instance.</param>
        /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
        public void FillInfoOutput(InfoOutput infoOutput, string categoryName = InfoOutput.GeneralCategoryName)
        {
            var infoOutputPackage = infoOutput.AcquirePackage(categoryName);

            infoOutputPackage.Add("Count", m_GridPointCount);

            DataTable gridPointTable = new DataTable("Coefficients");
            gridPointTable.Columns.Add("Grid Point Argument", typeof(double));
            gridPointTable.Columns.Add("a", typeof(double));
            gridPointTable.Columns.Add("b", typeof(double));
            gridPointTable.Columns.Add("c", typeof(double));
            gridPointTable.Columns.Add("d", typeof(double));

            for (int j = 0; j < m_GridPointCount; j++)
            {
                var row = gridPointTable.NewRow();
                row[0] = m_GridPointArguments[j];
                row[1] = m_GridPointValues[j];
                row[2] = m_CoefficientsB[j];
                row[3] = m_CoefficientsC[j];
                row[4] = m_CoefficientsD[j];

                gridPointTable.Rows.Add(row);
            }
            infoOutputPackage.Add(gridPointTable);
        }
        #endregion

        /// <summary>Updates the current instance, i.e. stores the grid points and returns references to the coefficients of the splines.
        /// </summary>
        /// <param name="gridPointCount">The number of grid points, i.e. the number of relevant elements of <paramref name="gridPointArguments"/> and <paramref name="gridPointValues"/> to take into account.</param>
        /// <param name="gridPointArguments">The arguments of the grid points, thus labels of the curve in its <see cref="System.Double"/> representation.</param>
        /// <param name="gridPointValues">The values of the grid points corresponding to <paramref name="gridPointArguments"/>.</param>
        /// <param name="state">The state of the grid points, i.e. <paramref name="gridPointArguments"/> and <paramref name="gridPointValues"/>, with respect to the previous function call.</param>
        /// <param name="coefficientsB">A reference to the coefficients 'b' with respect to f(t) = a[j] + b[j]*(t[j] - t) + c[j] * (t[j] - t)^2 + d[j] * (t[j] - t)^3. The caller of this method has to set valid coefficients (output).</param>
        /// <param name="coefficientsC">A reference to the coefficients 'c' with respect to f(t) = a[j] + b[j]*(t[j] - t) + c[j] * (t[j] - t)^2 + d[j] * (t[j] - t)^3. The caller of this method has to set valid coefficients (output).</param>
        /// <param name="coefficientsD">A reference to the coefficients 'd' with respect to f(t) = a[j] + b[j]*(t[j] - t) + c[j] * (t[j] - t)^2 + d[j] * (t[j] - t)^3. The caller of this method has to set valid coefficients (output).</param>
        /// <param name="gridPointArgumentStartIndex">The null-based start index of <paramref name="gridPointArguments" /> to take into account.</param>
        /// <param name="gridPointValueStartIndex">The null-based start index of <paramref name="gridPointValues" /> to take into account.</param>
        /// <param name="gridPointArgumentIncrement">The increment for <paramref name="gridPointArguments" />.</param>
        /// <param name="gridPointValueIncrement">The increment for <paramref name="gridPointValues" />.</param>
        public void Update(int gridPointCount, IList<double> gridPointArguments, IList<double> gridPointValues, GridPointCurve.State state, out double[] coefficientsB, out double[] coefficientsC, out double[] coefficientsD, int gridPointArgumentStartIndex = 0, int gridPointValueStartIndex = 0, int gridPointArgumentIncrement = 1, int gridPointValueIncrement = 1)
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

            /* allocate memory for spline coefficients if necessary and add a small buffer to avoid reallocation of memory: */
            ArrayMemory.Reallocate(ref m_CoefficientsB, gridPointCount - 1, Math.Max(10, gridPointCount / 5));
            coefficientsB = m_CoefficientsB;

            ArrayMemory.Reallocate(ref m_CoefficientsC, gridPointCount - 1, Math.Max(10, gridPointCount / 5));
            coefficientsC = m_CoefficientsC;

            ArrayMemory.Reallocate(ref m_CoefficientsD, gridPointCount - 1, Math.Max(10, gridPointCount / 5));
            coefficientsD = m_CoefficientsD;

            IntegralCacheUpdateRequested = true;
        }

        /// <summary>Evaluate the cubic spline at a specific point.
        /// </summary>
        /// <param name="pointToEvaluate">The point to evaluate.</param>
        /// <returns>The value of the cubic spline at <paramref name="pointToEvaluate"/>.</returns>
        public double GetValue(double pointToEvaluate)
        {
            int leftGridIndex = GetLeftIndex(pointToEvaluate);

            double timeDifference = pointToEvaluate - m_GridPointArguments[leftGridIndex];
            return m_GridPointValues[leftGridIndex] + timeDifference * (m_CoefficientsB[leftGridIndex] + timeDifference * (m_CoefficientsC[leftGridIndex] + timeDifference * m_CoefficientsD[leftGridIndex]));
        }

        /// <summary>Evaluate the derivative at a specific point.
        /// </summary>
        /// <param name="pointToEvaluate">The point to evaluate.</param>
        /// <returns>The value of the derivative at the <paramref name="pointToEvaluate"/>.</returns>
        public double GetDerivative(double pointToEvaluate)
        {
            int nonLastLeftGridIndex = GetNonLastLeftIndex(pointToEvaluate);

            double xDifference = pointToEvaluate - m_GridPointArguments[nonLastLeftGridIndex];
            return m_CoefficientsB[nonLastLeftGridIndex] + xDifference * (2.0 * m_CoefficientsC[nonLastLeftGridIndex] + xDifference * 3.0 * m_CoefficientsD[nonLastLeftGridIndex]);
        }

        /// <summary>Gets the value of the integral \int_a^b f(x) dx where 'a' and 'b' are inside the given range of grid points.
        /// </summary>
        /// <param name="lowerBound">The lower border of the integral, i.e. a.</param>
        /// <param name="upperBound">The upper border of the integral, i.e. b.</param>
        /// <returns>The value of \int_a^b f(x) dx with respect to the represented cubic spline.</returns>
        /// <remarks>
        /// Here we assume that <paramref name="lowerBound"/> is lower than <paramref name="upperBound"/> thus
        /// if <paramref name="lowerBound"/> is equal to the last grid point also <paramref name="upperBound"/>.
        /// <para>Here we assume that the difference between the time components is strict greater than <c>0.0</c>. 
        /// This will no be checked here.</para></remarks>
        public double GetIntegral(double lowerBound, double upperBound)
        {
            if (IntegralCacheUpdateRequested == true)
            {
                UpdateIntegralCache();
            }
            int leftGridIndex = Array.BinarySearch<double>(m_GridPointArguments, 0, m_GridPointCount, lowerBound);
            int rightGridIndex = Array.BinarySearch<double>(m_GridPointArguments, 0, m_GridPointCount, upperBound); 

            if ((leftGridIndex >= 0) && (rightGridIndex >= 0))
            {
                return m_IntegralCache[rightGridIndex] - m_IntegralCache[leftGridIndex];  // \int_{t_j}^{t_k} f(x) dx = \int_{t_0}^{t_k} f(x) dx - \int_{t_0}^{t_j} f(x) dx
            }
            else if (leftGridIndex < 0)
            {
                int previousLeftGridIndex = (~leftGridIndex) - 1; // previous grid point, i.e. smaller than the lower bound

                var value = GetIntegral(lowerBound, Math.Min(upperBound, GridPointArguments[previousLeftGridIndex + 1]), previousLeftGridIndex);

                if (leftGridIndex != rightGridIndex)  // lower and upper bound are not in the same interval [t_j, t_{j+1}]
                {
                    if (rightGridIndex >= 0)
                    {
                        value += (m_IntegralCache[rightGridIndex] - m_IntegralCache[previousLeftGridIndex + 1]);
                    }
                    else
                    {
                        rightGridIndex = (~rightGridIndex) - 1; // previous grid point, i.e. smaller than the upper bound
                        var stepValue = GetIntegral(Math.Max(lowerBound, GridPointArguments[rightGridIndex]), upperBound, rightGridIndex);

                        value += stepValue + (m_IntegralCache[rightGridIndex] - m_IntegralCache[previousLeftGridIndex + 1]);
                    }
                }
                return value;
            }
            else  // lower bound is a grid point and the upper bound is not a grid point
            {
                rightGridIndex = (~rightGridIndex) - 1; // previous grid point, i.e. smaller than the upper bound
                var stepValue = GetIntegral(Math.Max(lowerBound, GridPointArguments[rightGridIndex]), upperBound, rightGridIndex);

                return stepValue + (m_IntegralCache[rightGridIndex] - m_IntegralCache[leftGridIndex]);
            }
        }

        /// <summary>Gets the value of the integral \int_a^b f(x) dx inside two specific grid points.
        /// </summary>
        /// <param name="lowerBound">The lower bound; between the grid point arguments specified by <paramref name="leftGridPointIndex" /> and <paramref name="leftGridPointIndex" /> + 1.</param>
        /// <param name="upperBound">The upper bound; between the grid point arguments specified by <paramref name="leftGridPointIndex" /> and <paramref name="leftGridPointIndex" /> + 1.</param>
        /// <param name="leftGridPointIndex">The null-based index of the left grid point index.</param>
        /// <returns>The value of \int_a^b f(x) dx.</returns>
        public double GetIntegral(double lowerBound, double upperBound, int leftGridPointIndex)
        {
            var b = m_CoefficientsB[leftGridPointIndex];
            var c = m_CoefficientsC[leftGridPointIndex];
            var d = m_CoefficientsD[leftGridPointIndex];
            var leftGridPointValue = m_GridPointValues[leftGridPointIndex];

            var h = upperBound - GridPointArguments[leftGridPointIndex];
            var value = h * (leftGridPointValue + h * (b / 2.0 + h * (c / 3.0 + h * d / 4.0)));

            h = lowerBound - GridPointArguments[leftGridPointIndex];
            return value - h * (leftGridPointValue + h * (b / 2.0 + h * (c / 3.0 + h * d / 4.0)));
        }
        #endregion

        #region private methods

        /// <summary>Updates the internal cache.
        /// </summary>
        private void UpdateIntegralCache()
        {
            ArrayMemory.Reallocate(ref m_IntegralCache, m_GridPointCount, pufferSize: 5);

            var cumIntegralValue = m_IntegralCache[0] = 0.0;
            var lowerBound = m_GridPointArguments[0];

            for (int k = 1; k < m_GridPointCount; k++)
            {
                var upperBound = m_GridPointArguments[k];
                var upperGridPointValue = m_GridPointValues[k];

                cumIntegralValue += GetIntegral(lowerBound, upperBound, k - 1);
                m_IntegralCache[k] = cumIntegralValue;

                /* prepare for next loop: */
                lowerBound = upperBound;
            }
            IntegralCacheUpdateRequested = false;
        }

        /// <summary>Gets the null-based index of the grid point which is next to the specific <see cref="System.Double"/> object.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The null-based index of the grid point which is next to the specific <see cref="System.Double"/> object.
        /// If <paramref name="value"/> represents the last grid point, the last but before index will be returned.</returns>
        private int GetNonLastLeftIndex(double value)
        {
            int index = Array.BinarySearch<double>(m_GridPointArguments, 0, m_GridPointCount, value);
            if (index < 0)
            {
                return (~index) - 1;  /* 'index' is now the left-neigbour gridpoint or the last but one grid point index */
            }
            else if (index == m_GridPointCount - 1)
            {
                return index - 1;
            }
            return index;
        }

        /// <summary>Gets the null-based index of the grid point which is next to the specific <see cref="System.Double"/> object.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The null-based index of the grid point which is next to the specific <see cref="System.Double"/> object.
        /// If <paramref name="value"/> represents the last grid point, the last index will be returned.</returns>
        private int GetLeftIndex(double value)
        {
            int index = Array.BinarySearch<double>(m_GridPointArguments, 0, m_GridPointCount, value);
            if (index < 0)
            {
                return (~index) - 1;  /* 'index' is now the left-neigbour gridpoint or the last but one grid point index */
            }
            return index;
        }
        #endregion
    }
}