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
using Dodoni.BasicComponents.Utilities;

namespace Dodoni.MathLibrary.GridPointCurves
{
    /// <summary>Serves as cache for the calculation of the integral with respect to a specific <see cref="ICurveDataFitting"/> object.
    /// </summary>
    public class CurveInterpolatorIntegralCache
    {
        #region private members

        /// <summary>The internal cache, i.e. \int_{t_0}^{t_j} f(x) dx for j=0,1,...n.
        /// </summary>
        private double[] m_Cache;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="CurveInterpolatorIntegralCache" /> class.
        /// </summary>
        /// <param name="curveDataFitting">The curve data fitting.</param>
        public CurveInterpolatorIntegralCache(ICurveDataFitting curveDataFitting)
        {
            CurveDataFitting = curveDataFitting;
        }
        #endregion

        #region protected properties

        /// <summary>Gets the encapsulated <see cref="ICurveDataFitting"/> object.
        /// </summary>
        /// <value>The encapsulated <see cref="ICurveDataFitting"/> object.</value>
        protected ICurveDataFitting CurveDataFitting
        {
            get;
            private set;
        }

        /// <summary>Gets a value indicating whether an update of the cache is required.
        /// </summary>
        /// <value><c>true</c> if an update of the cache is requested; otherwise, <c>false</c>.</value>
        protected bool UpdateRequested
        {
            get;
            private set;
        }
        #endregion

        #region public properties

        /// <summary>Gets the number of grid points.
        /// </summary>
        /// <value>The number of grid points.</value>
        public int GridPointCount
        {
            get { return CurveDataFitting.GridPointCount; }
        }

        /// <summary>Gets the grid point arguments, i.e. the labels (on the x-axis) of the curve in its <see cref="System.Double"/> representation.
        /// </summary>
        /// <value>The grid point arguments.</value>
        public IList<double> GridPointArguments
        {
            get { return CurveDataFitting.GridPointArguments; }
        }

        /// <summary>Gets the grid point values with respect to <see cref="ICurveDataFitting.GridPointArguments"/>.
        /// </summary>
        /// <value>The grid point values.</value>
        public IList<double> GridPointValues
        {
            get { return CurveDataFitting.GridPointValues; }
        }
        #endregion

        #region public methods

        /// <summary>Earmarks an update request of the current <see cref="CurveInterpolatorIntegralCache"/> object for the next time when a specific integral should be calculated.
        /// </summary>
        public void EarmarkUpdateRequest()
        {
            UpdateRequested = true;
        }

        /// <summary>Gets the value of the integral \int_a^b f(x) dx.
        /// </summary>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="upperBound">The upper bound.</param>
        /// <returns>The value of \int_a^b f(x) dx.</returns>
        /// <remarks>The arguments must be elements of the domain of definition, represented by <see cref="IRealValuedCurve.LowerBound"/> and <see cref="IRealValuedCurve.UpperBound"/> of the encapsulated <see cref="ICurveDataFitting"/> object.</remarks>
        public double GetIntegral(double lowerBound, double upperBound)
        {
            if (UpdateRequested == true)
            {
                Update();
            }
            int leftGridIndex = GridPointArguments.BinarySearch(0, GridPointCount, lowerBound);
            int rightGridIndex = GridPointArguments.BinarySearch(0, GridPointCount, upperBound);

            if ((leftGridIndex >= 0) && (rightGridIndex >= 0))
            {
                return m_Cache[rightGridIndex] - m_Cache[leftGridIndex];  // \int_{t_j}^{t_k} f(x) dx = \int_{t_0}^{t_k} f(x) dx - \int_{t_0}^{t_j} f(x) dx
            }
            else if (leftGridIndex < 0)
            {
                int previousLeftGridIndex = (~leftGridIndex) - 1; // previous grid point, i.e. smaller than the lower bound

                var value = CurveDataFitting.GetIntegral(lowerBound, Math.Min(upperBound, GridPointArguments[previousLeftGridIndex + 1]), previousLeftGridIndex);

                if (leftGridIndex != rightGridIndex)  // lower and upper bound are not in the same interval [t_j, t_{j+1}]
                {
                    if (rightGridIndex >= 0)
                    {
                        value += (m_Cache[rightGridIndex] - m_Cache[previousLeftGridIndex + 1]);
                    }
                    else
                    {
                        rightGridIndex = (~rightGridIndex) - 1; // previous grid point, i.e. smaller than the upper bound
                        var stepValue = CurveDataFitting.GetIntegral(Math.Max(lowerBound, GridPointArguments[rightGridIndex]), upperBound, rightGridIndex);

                        value += stepValue + (m_Cache[rightGridIndex] - m_Cache[previousLeftGridIndex + 1]);
                    }
                }
                return value;
            }
            else  // lower bound is a grid point and the upper bound is not a grid point
            {
                rightGridIndex = (~rightGridIndex) - 1; // previous grid point, i.e. smaller than the upper bound
                var stepValue = CurveDataFitting.GetIntegral(Math.Max(lowerBound, GridPointArguments[rightGridIndex]), upperBound, rightGridIndex);

                return stepValue + (m_Cache[rightGridIndex] - m_Cache[leftGridIndex]);
            }
        }
        #endregion

        #region private methods

        /// <summary>Updates the internal cache.
        /// </summary>
        private void Update()
        {
            ArrayMemory.Reallocate(ref m_Cache, GridPointCount, pufferSize: 5);

            var cumIntegralValue = m_Cache[0] = 0.0;
            var lowerBound = GridPointArguments[0];

            for (int k = 1; k < GridPointCount; k++)
            {
                var upperBound = GridPointArguments[k];
                var upperGridPointValue = GridPointValues[k];

                cumIntegralValue += CurveDataFitting.GetIntegral(lowerBound, upperBound, k - 1);
                m_Cache[k] = cumIntegralValue;

                /* prepare for next loop: */
                lowerBound = upperBound;
            }
            UpdateRequested = false;
        }
        #endregion
    }
}