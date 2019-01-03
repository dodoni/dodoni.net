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
using System.Text;
using System.Collections.Generic;

using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.GridPointCurves
{
    public static partial class GridPointCurve
    {
        /// <summary>Serves as abstract basis class for curve interpolators, i.e. as factory for <see cref="ICurveDataFitting"/> objects. Moverover it provides some basic curve interpolation approaches.
        /// </summary>
        public abstract class Interpolator : ICurveDataFittingFactory
        {
            #region public static (readonly) members

            /// <summary>The curve interpolation 'none'.
            /// </summary>
            public static readonly Interpolator None = new CurveInterpolationNone();

            /// <summary>The piecewise constant curve interpolation where the value of the nearest grid point to the left will be returned.
            /// </summary>
            public static readonly Interpolator PiecewiseConstant = new CurveInterpolationPiecewiseConstant();

            /// <summary>The linear curve interpolation.
            /// </summary>
            public static readonly Interpolator Linear = new CurveInterpolationLinear();

            /// <summary>The log-linear curve interpolation.
            /// </summary>
            public static readonly Interpolator LogLinear = new CurveInterpolationLogLinear();

            /// <summary>Provides curve interpolation approaches based on (Cubic) Splines.
            /// </summary>
            public static readonly CurveInterpolationSpline Splines = new CurveInterpolationSpline();
            #endregion

            #region private members

            /// <summary>A short description of the curve interpolator.
            /// </summary>
            private string m_Annotation;
            #endregion

            #region static constructor

            /// <summary>Initializes the <see cref="Interpolator"/> class.
            /// </summary>
            static Interpolator()
            {
            }
            #endregion

            #region protected constructors

            /// <summary>Initializes a new instance of the <see cref="Interpolator"/> class.
            /// </summary>
            /// <param name="minimalRequiredNumberOfGridPoints">The minimal number of grid points required for a successfully application of the interpolation approach.</param>
            /// <param name="fittingQuality">A value indicating the fitting quality, i.e. whether grid points are meet exactly.</param>
            protected Interpolator(int minimalRequiredNumberOfGridPoints = 2, FittingQuality fittingQuality = GridPointCurves.FittingQuality.Exact)
            {
                m_Annotation = String.Empty;
                FittingQuality = fittingQuality;
                MinimalRequiredNumberOfGridPoints = minimalRequiredNumberOfGridPoints;
            }

            /// <summary>Initializes a new instance of the <see cref="Interpolator"/> class.
            /// </summary>
            /// <param name="annotation">The annotation, i.e. short description, of the curve interpolator.</param>
            /// <param name="minimalRequiredNumberOfGridPoints">The minimal number of grid points required for a successfully application of the interpolation approach.</param>
            /// <param name="fittingQuality">A value indicating the fitting quality, i.e. whether grid points are meet exactly.</param>
            protected Interpolator(string annotation, int minimalRequiredNumberOfGridPoints = 2, FittingQuality fittingQuality = GridPointCurves.FittingQuality.Exact)
            {
                m_Annotation = annotation ?? String.Empty;
                FittingQuality = fittingQuality;
                MinimalRequiredNumberOfGridPoints = minimalRequiredNumberOfGridPoints;
            }
            #endregion

            #region public properties

            #region IIdentifierNameable Members

            /// <summary>Gets the name of the curve interpolator.
            /// </summary>
            /// <value>The language independent name of the curve interpolator.</value>
            public IdentifierString Name
            {
                get { return GetName(); }
            }

            /// <summary>Gets the long name of the curve interpolator.
            /// </summary>
            /// <value>The (perhaps) language dependent long name of the curve interpolator.</value>
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

            #region ICurveDataFittingFactory Members

            /// <summary>Gets a value indicating the fitting quality, i.e. whether grid points are meet exactly. 
            /// </summary>
            /// <value>A value indicating the fitting quality, i.e. whether grid points are meet exactly.</value>
            public FittingQuality FittingQuality
            {
                get;
                private set;
            }

            /// <summary>Gets the minimal number of grid points required for a successfully application of the interpolation approach.
            /// </summary>
            /// <value>The minimal number of grid points required for a successfully application of the interpolation approach.</value>
            /// <remarks>In general two grid points are required.</remarks>
            public int MinimalRequiredNumberOfGridPoints
            {
                get;
                private set;
            }

            /// <summary>Gets a value indicating whether this instance represents a local approach.
            /// </summary>
            /// <value>
            /// 	<c>true</c> if this instance is local approach; otherwise, <c>false</c>.
            /// </value>
            /// <remarks>In the case of a local approach call <see cref="GetLeftLocalnessLevel(int, int)"/> and <see cref="GetRightLocalnessLevel(int, int)"/> 
            /// for the left and right localness level.
            /// <para>In the case of a global approach all grid points are required for the curve interpolation.</para></remarks>
            public abstract bool IsLocalApproach
            {
                get;
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

            #region ICurveDataFittingFactory Members

            /// <summary>Creates a <see cref="ICurveDataFitting"/> object that represents the implementation of the interpolation approach.
            /// </summary>
            /// <returns>A <see cref="ICurveDataFitting"/> object that represents the implementation of the interpolation approach.</returns>
            public abstract ICurveDataFitting Create();

            /// <summary>Gets the left localness level for a specific grid point, i.e.
            /// changing grid point (t_j,x_j) implies changes on the interval [t_{j-leftLocalnessLevel}, t_{j+rightLocalnessLevel}].
            /// </summary>
            /// <param name="gridPointIndex">The null-based index of the grid point (t_j,x_j).</param>
            /// <param name="gridPointCount">The number of grid points.</param>
            /// <returns>The left localness level with respect to the grid point (t_j,x_j), where j is represented by <paramref name="gridPointIndex"/>
            /// i.e. changing grid point (t_j,x_j) implies changes on the interval [t_{j-leftLocalnessLevel}, t_{j+rightLocalnessLevel}].</returns>
            /// <remarks>The localness level does not depend on the value of the grid point itself.</remarks>
            public abstract int GetLeftLocalnessLevel(int gridPointIndex, int gridPointCount);

            /// <summary>Gets the right localness level for a specific grid point, i.e.
            /// changing grid point (t_j,x_j) implies changes on the interval [t_{j-leftLocalnessLevel}, t_{j+rightLocalnessLevel}].
            /// </summary>
            /// <param name="gridPointIndex">The null-based index of the grid point (t_j,x_j).</param>
            /// <param name="gridPointCount">The number of grid points.</param>
            /// <returns>The right localness level with respect to the grid point (t_j,x_j), where j is represented by <paramref name="gridPointIndex"/>
            /// i.e. changing grid point (t_j,x_j) implies changes on the interval [t_{j-leftLocalnessLevel}, t_{j+rightLocalnessLevel}].</returns>
            /// <remarks>The localness level does not depend on the value of the grid point itself.</remarks>
            public abstract int GetRightLocalnessLevel(int gridPointIndex, int gridPointCount);
            #endregion

            /// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
            /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
            public override string ToString()
            {
                return Name.String;
            }
            #endregion

            #region protected methods

            /// <summary>Gets the name of the curve interpolator.
            /// </summary>
            /// <returns>The name of the curve interpolator.</returns>
            protected abstract IdentifierString GetName();

            /// <summary>Gets the long name of the curve interpolator.
            /// </summary>
            /// <returns>The (perhaps) language dependent long name of the curve interpolator.</returns>
            protected abstract IdentifierString GetLongName();
            #endregion
        }
    }
}