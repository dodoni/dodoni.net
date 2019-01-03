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

using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.GridPointCurves
{
    /// <summary>Represents the Monotonic preserving cubic spline curve interpolation.
    /// </summary>
    /// <remarks>The implentation is based on 
    /// <para>Patrick S. Hagan, Graeme West, Interpolation methods for curve construction, Applied Mathematical Finance, Vol. 13, No. 2, p.89-129, June 2006.</para>
    /// and
    /// <para>
    /// J. M. Hyman, Accurate monotonicity preserving cubic interpolation, SIAM Journal on Scientific and Statistical Computing, 4(4), pp. 645-654, 1983.
    /// </para>
    /// </remarks>
    public static class CurveInterpolationMonotonicPreservingCubicSpline
    {
        #region nested classes

        /// <summary>Serves as abstract basis class for a factory for <see cref="IMonotonicPreservingCubicSplineBoundaryCondition"/> objects.
        /// </summary>
        public abstract class BoundaryCondition : IIdentifierNameable, IAnnotatable
        {
            #region public static (readonly) members

            /// <summary>Represents the boundary condition for <see cref="CurveInterpolationMonotonicPreservingCubicSpline"/> instances, where 
            /// the coefficient 'b' at the boundary will be set with respect to
            /// <para>
            ///    Patrick S. Hagan, Graeme West: Interpolation methods for curve construction, 
            ///          Applied Mathematical Finance, Vol. 13, No. 2, 89-129, June 2006,
            /// </para>
            /// i.e. the coefficient 'b' at the boundary will be set to 0.0, i.e. b_0 = b_n = 0.0.
            /// </summary>
            public static readonly BoundaryCondition HaganWest = new HaganWestBoundaryConditions();

            /// <summary>Represents the boundary condition for <see cref="CurveInterpolationMonotonicPreservingCubicSpline"/> instances, where 
            /// the coefficient 'b' at the boundary will be set with respect to
            /// <para>
            ///   J. M. Hyman: Accurate monotonicity preserving cubic interpolation, SIAM Journal on Scientific and Statistical Computing, 4(4), pp. 645-654,
            /// </para>
            /// i.e. b_0 = [(2*h_1 + h_2) * m_1 - h_1 * m_2]/(h_1 + h_2) and b_n = [(2*h_{n-1} + h_{n-2}) * m_{n-1} - h_{n-1} * m_{n-2}]/(h_{n-1} + h_{n-2}).
            /// </summary>
            public static readonly BoundaryCondition Hyman = new HymanBoundaryConditions();
            #endregion

            #region private members

            /// <summary>A short description of the boundary condition.
            /// </summary>
            private string m_Annotation;
            #endregion

            #region protected constructors

            /// <summary>Initializes a new instance of the <see cref="BoundaryCondition"/> class.
            /// </summary>
            protected BoundaryCondition()
            {
                m_Annotation = String.Empty;
            }

            /// <summary>Initializes a new instance of the <see cref="BoundaryCondition"/> class.
            /// </summary>
            /// <param name="annotation">The annotation, i.e. short description, of the boundary condition.</param>
            protected BoundaryCondition(string annotation)
            {
                m_Annotation = annotation ?? String.Empty;
            }
            #endregion

            #region static constructor

            static BoundaryCondition()
            {
            }
            #endregion

            #region public properties

            #region IIdentifierNameable Members

            /// <summary>Gets the name of the boundary condition.
            /// </summary>
            /// <value>The language independent name of the boundary condition.</value>
            public IdentifierString Name
            {
                get { return GetName(); }
            }

            /// <summary>Gets the long name of the boundary condition.
            /// </summary>
            /// <value>The (perhaps) language dependent long name of the boundary condition.</value>
            public IdentifierString LongName
            {
                get { return GetLongName(); }
            }
            #endregion

            #region IAnnotatable Members

            /// <summary>Gets a value indicating whether the annotation is read-only.
            /// </summary>
            /// <value><c>true</c> if the annotation of this instance is readonly; otherwise, <c>false</c>.</value>
            public bool HasReadOnlyAnnotation
            {
                get { return false; }
            }

            /// <summary>Gets the annotation of the current instance.
            /// </summary>
            /// <value>The annotation of the current instance.</value>
            public string Annotation
            {
                get { return m_Annotation; }
            }
            #endregion

            #endregion

            #region public methods

            #region IAnnotatable Members

            /// <summary>Sets the annotation of the current instance.
            /// </summary>
            /// <param name="annotation">The annotation.</param>
            /// <returns>A value indicating whether the <see cref="Annotation"/> has been changed.</returns>
            public bool TrySetAnnotation(string annotation)
            {
                m_Annotation = annotation ?? String.Empty;
                return true;
            }
            #endregion

            /// <summary>Creates a <see cref="IMonotonicPreservingCubicSplineBoundaryCondition"/> object that represents the implementation of the boundary condition.
            /// </summary>
            /// <returns>A <see cref="IMonotonicPreservingCubicSplineBoundaryCondition"/> object that represents the implementation of the boundary condition.</returns>
            public abstract IMonotonicPreservingCubicSplineBoundaryCondition Create();

            /// <summary>Returns a <see cref="System.String"/> that represents this instance.
            /// </summary>
            /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
            public override string ToString()
            {
                return Name.String;
            }
            #endregion

            #region protected methods

            /// <summary>Gets the name of the boundary condition.
            /// </summary>
            /// <returns>The name of the boundary condition.</returns>
            protected abstract IdentifierString GetName();

            /// <summary>Gets the long name of the boundary condition.
            /// </summary>
            /// <returns>The (perhaps) language dependent long name of the boundary condition.</returns>
            protected abstract IdentifierString GetLongName();
            #endregion
        }
        #endregion

        #region public static methods

        /// <summary>Creates a new monotonic preserving cubic spline curve interpolator.
        /// </summary>
        /// <param name="curveInterpolationSpline">The <see cref="CurveInterpolationSpline"/> object.</param>
        /// <param name="boundaryCondition">The boundary condition.</param>
        /// <returns>A <see cref="GridPointCurve.Interpolator"/> object that represents a monotonic preserving cubic spline curve interpolator.</returns>
        /// <remarks>The implentation is based on 
        /// <para>Patrick S. Hagan, Graeme West, Interpolation methods for curve construction, Applied Mathematical Finance, Vol. 13, No. 2, p.89-129, June 2006.</para>
        /// and
        /// <para>
        /// J. M. Hyman, Accurate monotonicity preserving cubic interpolation, SIAM Journal on Scientific and Statistical Computing, 4(4), pp. 645-654, 1983.
        /// </para>
        /// </remarks>
        public static GridPointCurve.Interpolator Create(this CurveInterpolationSpline curveInterpolationSpline, BoundaryCondition boundaryCondition)
        {
            return new MonotonicPreservingCubicSpline(boundaryCondition);
        }
        #endregion
    }
}