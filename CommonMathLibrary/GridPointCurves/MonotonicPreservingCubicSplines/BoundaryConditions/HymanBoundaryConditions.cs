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

namespace Dodoni.MathLibrary.GridPointCurves
{
    /// <summary>Represents the boundary condition for <see cref="CurveInterpolationMonotonicPreservingCubicSpline"/> instances, where 
    /// the coefficient 'b' at the boundary will be set with respect to
    /// <para>
    ///   J. M. Hyman: Accurate monotonicity preserving cubic interpolation, SIAM Journal on Scientific and Statistical Computing, 4(4), pp. 645-654,
    /// </para>
    /// i.e. b_0 = [(2*h_1 + h_2) * m_1 - h_1 * m_2]/(h_1 + h_2) and b_n = [(2*h_{n-1} + h_{n-2}) * m_{n-1} - h_{n-1} * m_{n-2}]/(h_{n-1} + h_{n-2}).
    /// </summary>
    internal class HymanBoundaryConditions : CurveInterpolationMonotonicPreservingCubicSpline.BoundaryCondition
    {
        #region nested classes

        /// <summary>Serves as <see cref="IMonotonicPreservingCubicSplineBoundaryCondition"/> implementation of boundary condition.
        /// </summary>
        private class BoundaryCondition : IMonotonicPreservingCubicSplineBoundaryCondition
        {
            #region private members

            /// <summary>The first boundary coefficient.
            /// </summary>
            private double m_FirstBoundaryCoefficient;

            /// <summary>The last boundary coefficient.
            /// </summary>
            private double m_LastBoundaryCoefficient;
            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="BoundaryCondition"/> class.
            /// </summary>
            internal BoundaryCondition()
            {
            }
            #endregion

            #region IMonotonicPreservingCubicSplineBoundaryCondition Members

            /// <summary>Updates the current boundary condition.
            /// </summary>
            /// <param name="gridPointCount">The number of grid points, i.e. the number of relevant elements of <paramref name="gridPointArguments"/> and <paramref name="gridPointValues"/> to take into account.</param>
            /// <param name="gridPointArguments">The arguments of the grid points, thus labels of the curve in its <see cref="System.Double"/> representation.</param>
            /// <param name="gridPointValues">The values of the grid points corresponding to <paramref name="gridPointArguments"/>.</param>
            /// <param name="state">The state of the grid points, i.e. <paramref name="gridPointArguments"/> and <paramref name="gridPointValues"/>, with respect to the previous function call.</param>
            /// <param name="gridPointArgumentsStartIndex">The null-based start index of <paramref name="gridPointArguments"/> to take into account.</param>
            /// <param name="gridPointValuesStartIndex">The null-based start index of <paramref name="gridPointValues"/> to take into account.</param>
            /// <param name="gridPointArgumentIncrement">The increment for <paramref name="gridPointArguments"/>.</param>
            /// <param name="gridPointValueIncrement">The increment for <paramref name="gridPointValues"/>.</param>
            /// <remarks>This method should be called if grid points have been changed, added, removed etc. and before evaluating the grid point curve at a specified point.
            /// </remarks>
            public void Update(int gridPointCount, IList<double> gridPointArguments, IList<double> gridPointValues, GridPointCurve.State state, int gridPointArgumentsStartIndex = 0, int gridPointValuesStartIndex = 0, int gridPointArgumentIncrement = 1, int gridPointValueIncrement = 1)
            {
                if (gridPointCount < 2)
                {
                    m_FirstBoundaryCoefficient = m_LastBoundaryCoefficient = Double.NaN;
                }
                else
                {
                    double deltaT = gridPointArguments[gridPointArgumentsStartIndex + gridPointArgumentIncrement] - gridPointArguments[gridPointArgumentsStartIndex];  // = t_{j+1} - t_j
                    double nextDeltaT = gridPointArguments[gridPointArgumentsStartIndex + 2 * gridPointArgumentIncrement] - gridPointArguments[gridPointArgumentsStartIndex + gridPointArgumentIncrement]; // = t_{j+2} - t_{j+1}

                    double valueAt1 = gridPointValues[gridPointValuesStartIndex + gridPointValueIncrement];
                    double m = (valueAt1 - gridPointValues[gridPointValuesStartIndex]) / deltaT;

                    m_FirstBoundaryCoefficient = ((deltaT + deltaT + nextDeltaT) * m - deltaT * (gridPointValues[gridPointValuesStartIndex + 2 * gridPointValueIncrement] - valueAt1) / nextDeltaT) / (deltaT + nextDeltaT);


                    int lastGridPointArgumentIndex = gridPointArgumentsStartIndex + gridPointArgumentIncrement * (gridPointCount - 1);
                    int lastGridPointValueIndex = gridPointValuesStartIndex + gridPointValueIncrement * (gridPointCount - 1);

                    double lastButOneDeltaT = gridPointArguments[lastGridPointArgumentIndex - 2 * gridPointArgumentIncrement] - gridPointArguments[lastGridPointArgumentIndex - gridPointArgumentIncrement];
                    double lastButOne_m = (gridPointValues[lastGridPointValueIndex - 2 * gridPointValueIncrement] - gridPointValues[lastGridPointValueIndex - gridPointValueIncrement]) / lastButOneDeltaT;

                    double lastDeltaT = gridPointArguments[lastGridPointArgumentIndex - gridPointArgumentIncrement] - gridPointArguments[lastGridPointArgumentIndex];
                    double last_m = (gridPointValues[lastGridPointValueIndex - gridPointValueIncrement] - gridPointValues[lastGridPointValueIndex]) / lastDeltaT;

                    m_LastBoundaryCoefficient = ((2.0 * lastDeltaT + lastButOneDeltaT) * last_m - lastDeltaT * last_m) / (lastButOneDeltaT + lastDeltaT);
                }
            }

            /// <summary>Gets the first boundary coefficient, i.e. b_1.
            /// </summary>
            /// <returns>The value of 'b_1', i.e. the first boundary coefficient.</returns>
            public double GetFirstBoundaryCoefficient()
            {
                return m_FirstBoundaryCoefficient;
            }

            /// <summary>Gets the last boundary coefficient, i.e. b_n.
            /// </summary>
            /// <returns>The value of 'b_n', i.e. the last boundary coefficient.</returns>
            public double GetLastBoundaryCoefficient()
            {
                return m_LastBoundaryCoefficient;
            }
            #endregion
        }
        #endregion

        #region private members

        /// <summary>The name of the boundary condition.
        /// </summary>
        private IdentifierString m_Name;

        /// <summary>The long name of the boundary condition.
        /// </summary>
        private IdentifierString m_LongName;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="HymanBoundaryConditions"/> class.
        /// </summary>
        internal HymanBoundaryConditions()
            : base(CurveResource.AnnotationBoundaryCubicSplineHyman)
        {
            m_Name = new IdentifierString("Hyman boundary condition");
            m_LongName = new IdentifierString(CurveResource.LongNameBoundaryCubicSplineHyman);
        }
        #endregion

        #region public methods

        /// <summary>Creates a <see cref="IMonotonicPreservingCubicSplineBoundaryCondition"/> object that represents the implementation of the boundary condition.
        /// </summary>
        /// <returns>A <see cref="IMonotonicPreservingCubicSplineBoundaryCondition"/> object that represents the implementation of the boundary condition.</returns>
        public override IMonotonicPreservingCubicSplineBoundaryCondition Create()
        {
            return new BoundaryCondition();
        }
        #endregion

        #region protected methods

        /// <summary>Gets the name of the boundary condition.
        /// </summary>
        /// <returns>The name of the boundary condition.</returns>
        protected override IdentifierString GetName()
        {
            return m_Name;
        }

        /// <summary>Gets the long name of the boundary condition.
        /// </summary>
        /// <returns>The (perhaps) language dependent long name of the boundary condition.</returns>
        protected override IdentifierString GetLongName()
        {
            return m_LongName;
        }
        #endregion
    }
}