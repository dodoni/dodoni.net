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
namespace Dodoni.MathLibrary.GridPointCurves
{
    /// <summary>Represents curve interpolation approaches based on (Cubic) splines.
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public class CurveInterpolationSpline
    {
        #region public (readonly) members

        /// <summary>Provides curve interpolation approaches based on natural cubic splines.
        /// </summary>
        public readonly GridPointCurve.Interpolator NaturalCubicSpline;

        /// <summary>Provides the Bessel (Hermite) cubic spline curve interpolation.
        /// </summary>
        /// <remarks>The implementation is based on 
        /// <para>P.S. Hagan, G. West, Interpolation methods for curve construction, Applied Mathematical Finance, Vol. 13, No. 2, p.89-129, June 2006.</para>
        /// This interpolation is also named 'Hermite interpolation'. This approach contains already spline boundary conditions.</remarks>
        public readonly GridPointCurve.Interpolator BesselCubicSpline;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="CurveInterpolationSpline"/> class.
        /// </summary>
        internal CurveInterpolationSpline()
        {
            var naturalCondition = new NaturalCubicSplineBoundaryCondition();
            NaturalCubicSpline = new CurveInterpolationCommonCubicSpline(naturalCondition, "Natural Cubic Spline", CurveResource.LongNameInterpolationNaturalCubicSpline, CurveResource.AnnotationInterpolationNaturalCubicSpline);

            BesselCubicSpline = new CurveInterpolationBesselCubicSpline();
        }
        #endregion

        #region public methods

        /// <summary>Creates a specific cubic spline curve interpolation.
        /// </summary>
        /// <param name="boundaryCondition">The boundary condition.</param>
        /// <returns>The cube spline curve interpolation with respect to the specified boundary conditions.</returns>
        public GridPointCurve.Interpolator Create(CurveInterpolationCubicSpline.BoundaryCondition boundaryCondition)
        {
            return new CurveInterpolationCommonCubicSpline(boundaryCondition);
        }

        /// <summary>Creates a specific cubic spline curve interpolation with respect to a clamped cubic spline, i.e. with specified f'(x_0) and f'(x_n).
        /// </summary>
        /// <param name="derivativeAtFirstPoint">The derivative at first point.</param>
        /// <param name="derivativeAtLastPoint">The derivative at last point.</param>
        /// <returns>The cube spline curve interpolation with respect to the specified clamped cubic spline boundary condition.</returns>
        public GridPointCurve.Interpolator Create(double derivativeAtFirstPoint, double derivativeAtLastPoint)
        {
            var boundaryCondition = ClampedCubicSplineBoundaryCondition.Create(derivativeAtFirstPoint, derivativeAtLastPoint);
            return new CurveInterpolationCommonCubicSpline(boundaryCondition);
        }
        #endregion
    }
}