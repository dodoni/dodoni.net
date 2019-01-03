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
    /// <summary>Represents the boundary condition for the clamped cubic spline, i.e. f'(x_0) and f'(x_n) are user input.
    /// </summary>
    internal class ClampedCubicSplineBoundaryCondition : CurveInterpolationCubicSpline.BoundaryCondition
    {
        #region nested classes

        private class BoundaryCondition : ICubicSplineBoundaryCondition
        {
            #region private members

            /// <summary>The first derivative at the first point x_0.
            /// </summary>
            private double m_DerivativeAtFirstPoint;

            /// <summary>The first derivative at the last point x_n.
            /// </summary>
            private double m_DerivativeAtLastPoint;

            /// <summary>The first difference of the x-values, i.e. x_1 - x_0.
            /// </summary>
            private double m_FirstXDelta = Double.NaN;

            /// <summary>The last difference of the x-values, i.e. x_n - x_{n-1}.
            /// </summary>
            private double m_LastXDelta = Double.NaN;

            /// <summary>The first difference of the y-values, i.e. f(x_1) - f(x_0).
            /// </summary>
            private double m_FirstYDelta = Double.NaN;

            /// <summary>The last difference of the y-values, i.e. f(x_n) - f(x_{n-1}).
            /// </summary>
            private double m_LastYDelta = Double.NaN;
            #endregion

            #region public constructors

            /// <summary>Initializes a new instance of the <see cref="BoundaryCondition"/> class.
            /// </summary>
            /// <param name="derivativeAtFirstPoint">The derivative at the first point x_0.</param>
            /// <param name="derivativeAtLastPoint">The derivative at the last point x_n.</param>
            public BoundaryCondition(double derivativeAtFirstPoint, double derivativeAtLastPoint)
            {
                m_DerivativeAtFirstPoint = derivativeAtFirstPoint;
                m_DerivativeAtLastPoint = derivativeAtLastPoint;
            }
            #endregion

            #region ICubicSplineBoundaryCondition Members

            /// <summary>Updates the current boundary condition.
            /// </summary>
            /// <param name="gridPointCount">The number of grid points, i.e. the number of relevant elements of <paramref name="gridPointArguments" /> and <paramref name="gridPointValues" /> to take into account.</param>
            /// <param name="gridPointArguments">The arguments of the grid points, thus labels of the curve in its <see cref="System.Double" /> representation.</param>
            /// <param name="gridPointValues">The values of the grid points corresponding to <paramref name="gridPointArguments" />.</param>
            /// <param name="state">The state of the grid points, i.e. <paramref name="gridPointArguments" /> and <paramref name="gridPointValues" />, with respect to the previous function call.</param>
            /// <param name="gridPointArgumentStartIndex">The null-based start index of <paramref name="gridPointArguments" /> to take into account.</param>
            /// <param name="gridPointValueStartIndex">The null-based start index of <paramref name="gridPointValues" /> to take into account.</param>
            /// <param name="gridPointArgumentIncrement">The increment for <paramref name="gridPointArguments" />.</param>
            /// <param name="gridPointValueIncrement">The increment for <paramref name="gridPointValues" />.</param>
            /// <remarks>
            /// This method should be called if grid points have been changed, added, removed etc. and before evaluating the grid point curve at a specified point.
            /// </remarks>
            public void Update(int gridPointCount, IList<double> gridPointArguments, IList<double> gridPointValues, GridPointCurve.State state, int gridPointArgumentStartIndex = 0, int gridPointValueStartIndex = 0, int gridPointArgumentIncrement = 1, int gridPointValueIncrement = 1)
            {
                if (state.HasFlag(GridPointCurve.State.GridPointArgumentChanged))
                {
                    m_FirstXDelta = gridPointArguments[gridPointArgumentStartIndex + 1] - gridPointArguments[gridPointArgumentStartIndex];
                    m_LastXDelta = gridPointArguments[gridPointArgumentStartIndex + gridPointArgumentIncrement * (gridPointCount - 1)] - gridPointArguments[gridPointArgumentStartIndex + gridPointArgumentIncrement * (gridPointCount - 2)];
                }
                m_FirstYDelta = gridPointValues[gridPointValueStartIndex + 1] - gridPointValues[gridPointValueStartIndex];
                m_LastYDelta = gridPointValues[gridPointValueStartIndex + gridPointValueIncrement * (gridPointCount - 1)] - gridPointValues[gridPointValueStartIndex + gridPointValueIncrement * (gridPointCount - 2)];
            }

            /// <summary>Gets the remaining matrix elements of A = (a_{i,j}), where A*y=b represents the cubic spline coefficent equation.
            /// </summary>
            /// <param name="firstDiagonalElement">The first diagonal element, i.e. a_{1,1} (output).</param>
            /// <param name="firstSuperDiagonalElement">The first super diagonal element, i.e. a_{1,2} (output).</param>
            /// <param name="lastSubDiagonalElement">The last sub diagonal element, i.e. a_{n,n-1} (output).</param>
            /// <param name="lastDiagonalElement">The last diagonal element, i.e. a_{n,n} (output).</param>
            /// <remarks>Here, we assume that the remaining matrix elements do not depend on the grid points (output).</remarks>
            public void GetRemainingMatrixElements(out double firstDiagonalElement, out double firstSuperDiagonalElement, out double lastSubDiagonalElement, out double lastDiagonalElement)
            {
                firstDiagonalElement = 2 * m_FirstXDelta;
                firstSuperDiagonalElement = m_FirstXDelta;
                lastSubDiagonalElement = m_LastXDelta;
                lastDiagonalElement = 2 * m_LastXDelta;
            }

            /// <summary>Gets the remaining right hand side elements, i.e. the first and last element of 'b',
            /// where A*y=b represents the cubic spline coefficent equation.
            /// </summary>
            /// <param name="firstElement">The first element of b (output).</param>
            /// <param name="lastElement">The last element of b (output).</param>
            public void GetRemainingRightHandSideElements(out double firstElement, out double lastElement)
            {
                firstElement = 3 * (m_FirstYDelta / m_FirstXDelta - m_DerivativeAtFirstPoint);
                lastElement = 3 * (m_DerivativeAtLastPoint - m_LastYDelta / m_LastXDelta);
            }
            #endregion
        }
        #endregion

        #region private members

        /// <summary>The first derivative at the first point x_0.
        /// </summary>
        private double m_DerivativeAtFirstPoint;

        /// <summary>The first derivative at the last point x_n.
        /// </summary>
        private double m_DerivativeAtLastPoint;

        /// <summary>The name of the boundary condition.
        /// </summary>
        private IdentifierString m_Name;

        /// <summary>The long name of the boundary condition.
        /// </summary>
        private IdentifierString m_LongName;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="ClampedCubicSplineBoundaryCondition"/> class.
        /// </summary>
        /// <param name="derivativeAtFirstPoint">The derivative at first point.</param>
        /// <param name="derivativeAtLastPoint">The derivative at last point.</param>
        public ClampedCubicSplineBoundaryCondition(double derivativeAtFirstPoint, double derivativeAtLastPoint)
            : base(CurveResource.AnnotationBoundaryCubicSplineClamped)
        {
            m_DerivativeAtFirstPoint = derivativeAtFirstPoint;
            m_DerivativeAtLastPoint = derivativeAtLastPoint;
            m_Name = new IdentifierString(String.Format("Clamped (y_0'={0}; y_n'={1})", derivativeAtFirstPoint, derivativeAtLastPoint));
            m_LongName = new IdentifierString(String.Format(CurveResource.LongNameBoundaryCubicSplineClamped, derivativeAtFirstPoint, derivativeAtLastPoint));
        }
        #endregion

        #region public methods

        /// <summary>Creates a <see cref="ICubicSplineBoundaryCondition"/> object that represents the implementation of the boundary condition.
        /// </summary>
        /// <returns>A <see cref="ICubicSplineBoundaryCondition"/> object that represents the implementation of the boundary condition.</returns>
        public override ICubicSplineBoundaryCondition Create()
        {
            return new BoundaryCondition(m_DerivativeAtFirstPoint, m_DerivativeAtLastPoint);
        }
        #endregion

        #region public static methods

        /// <summary>Creates a specific <see cref="ClampedCubicSplineBoundaryCondition"/>.
        /// </summary>
        /// <param name="derivativeAtFirstPoint">The derivative at first point.</param>
        /// <param name="derivativeAtLastPoint">The derivative at last point.</param>
        /// <returns>The <see cref="ClampedCubicSplineBoundaryCondition"/> that represents the boundary condition for the cubic spline curve interpolation.</returns>
        public static ClampedCubicSplineBoundaryCondition Create(double derivativeAtFirstPoint, double derivativeAtLastPoint)
        {
            return new ClampedCubicSplineBoundaryCondition(derivativeAtFirstPoint, derivativeAtLastPoint);
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