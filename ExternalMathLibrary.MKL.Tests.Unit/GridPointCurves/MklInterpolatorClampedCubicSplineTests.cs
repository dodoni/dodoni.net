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
using NUnit.Framework;

using Dodoni.MathLibrary.GridPointCurves;

namespace Dodoni.MathLibrary.Native.GridPointCurves
{
    /// <summary>Serves as unit test class for the clamped cubic spline curve interpolator with respect to Intel's MKL Library.
    /// </summary>
    [TestFixture]
    public class MklInterpolatorClampedCubicSplineTests : InterpolatorClampedCubicSplineTests
    {
        /// <summary>Initializes a new instance of the <see cref="MklInterpolatorClampedCubicSplineTests" /> class.
        /// </summary>
        public MklInterpolatorClampedCubicSplineTests()
        {
        }

        /// <summary>Gets the interpolator object (without any grid points).
        /// </summary>
        /// <param name="derivativeAtFirstPoint">The derivative at first point.</param>
        /// <param name="derivativeAtLastPoint">The derivative at last point.</param>
        /// <returns>The <see cref="ICurveDataFitting"/> object under test.</returns>
        protected override ICurveDataFitting GetEmptyTestCaseInterpolatorObject(double derivativeAtFirstPoint, double derivativeAtLastPoint)
        {
            return MklGridPointCurve.Interpolator.Splines.Create(MklCurveInterpolationSpline.SplineOrder.DF_PP_CUBIC, MklCurveInterpolationSpline.SplineType.DF_PP_NATURAL, MklCurveInterpolationSpline.SplineBoundaryCondition.DF_BC_1ST_LEFT_DER | MklCurveInterpolationSpline.SplineBoundaryCondition.DF_BC_1ST_RIGHT_DER, new[] { derivativeAtFirstPoint, derivativeAtLastPoint }).Create();
        }
    }
}