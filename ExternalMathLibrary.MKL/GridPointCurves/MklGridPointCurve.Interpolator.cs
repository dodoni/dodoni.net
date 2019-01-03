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
using Dodoni.MathLibrary.GridPointCurves;

namespace Dodoni.MathLibrary.Native.GridPointCurves
{
    public static partial class MklGridPointCurve
    {
        /// <summary>Serves as abstract basis class for curve interpolators w.r.t. Intel MKL Library, i.e. as factory for <see cref="IMklCurveDataFitting"/> objects.
        /// </summary>
        public abstract class Interpolator : GridPointCurve.Interpolator
        {
            #region public static (readonly) members

            /// <summary>The linear curve interpolation.
            /// </summary>
            public new static readonly Interpolator Linear;

            /// <summary>The log-linear curve interpolation.
            /// </summary>
            public new static readonly Interpolator LogLinear;

            /// <summary>The piecewise constant curve interpolation where the value of the nearest grid point to the left will be returned.
            /// </summary>
            public new static readonly Interpolator PiecewiseConstant;

            /// <summary>Provides curve interpolation approaches based on Splines.
            /// </summary>
            public new static readonly MklCurveInterpolationSpline Splines;
            #endregion

            #region static constructor

            /// <summary>Initializes the <see cref="Interpolator"/> class.
            /// </summary>
            static Interpolator()
            {
                Splines = new MklCurveInterpolationSpline();
                Linear = Splines.Create(MklCurveInterpolationSpline.SplineOrder.DF_PP_LINEAR, MklCurveInterpolationSpline.SplineType.DF_PP_DEFAULT, MklCurveInterpolationSpline.SplineBoundaryCondition.DF_NO_BC);
                PiecewiseConstant = Splines.Create(MklCurveInterpolationSpline.SplineOrder.DF_PP_STD, MklCurveInterpolationSpline.SplineType.DF_CR_STEPWISE_CONST_INTERPOLANT, MklCurveInterpolationSpline.SplineBoundaryCondition.DF_NO_BC);

                LogLinear = new MklCurveInterpolationLogLinear();
            }
            #endregion

            #region protected constructors

            /// <summary>Initializes a new instance of the <see cref="Interpolator"/> class.
            /// </summary>
            /// <param name="minimalRequiredNumberOfGridPoints">The minimal number of grid points required for a successfully application of the interpolation approach.</param>
            /// <param name="fittingQuality">A value indicating the fitting quality, i.e. whether grid points are meet exactly.</param>
            protected Interpolator(int minimalRequiredNumberOfGridPoints = 2, FittingQuality fittingQuality = FittingQuality.Exact)
                : base(minimalRequiredNumberOfGridPoints, fittingQuality)
            {
            }

            /// <summary>Initializes a new instance of the <see cref="Interpolator"/> class.
            /// </summary>
            /// <param name="annotation">The annotation, i.e. short description, of the curve interpolator.</param>
            /// <param name="minimalRequiredNumberOfGridPoints">The minimal number of grid points required for a successfully application of the interpolation approach.</param>
            /// <param name="fittingQuality">A value indicating the fitting quality, i.e. whether grid points are meet exactly.</param>
            protected Interpolator(string annotation, int minimalRequiredNumberOfGridPoints = 2, FittingQuality fittingQuality = FittingQuality.Exact)
                : base(annotation, minimalRequiredNumberOfGridPoints, fittingQuality)
            {
            }
            #endregion

            #region public methods

            /// <summary>Creates a <see cref="IMklCurveDataFitting"/> object that represents the implementation of the interpolation approach.
            /// </summary>
            /// <param name="computationMethod">The computation method with respect to the MKL data fitting routines.</param>
            /// <returns>A <see cref="IMklCurveDataFitting"/> object that represents the implementation of the interpolation approach.</returns>
            /// <remarks>The current release of Intel's Math Kernel Library (11.0, Update 5) does support exactly one computation method.</remarks>
            public abstract IMklCurveDataFitting Create(ComputationMethod computationMethod);
            #endregion
        }
    }
}