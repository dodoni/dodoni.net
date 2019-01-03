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

namespace Dodoni.MathLibrary.Native.GridPointCurves
{
    /// <summary>Represents curve interpolation approaches based on splines with respect to Intel's MKL Library.
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public class MklCurveInterpolationSpline
    {
        #region nested enumerations

        /// <summary>The order of the spline.
        /// </summary>
        public enum SplineOrder
        {
            /// <summary>Artificial value. Use this value for look-up and step-wise constant interpolants only.
            /// </summary>
            DF_PP_STD = 0,

            /// <summary>A linear spline, i.e. linear interpolation.
            /// </summary>
            DF_PP_LINEAR = 2,

            /// <summary>Quadatic splines.
            /// </summary>
            DF_PP_QUADRATIC = 3,

            /// <summary>Cubic splines.
            /// </summary>
            DF_PP_CUBIC = 4
        }

        /// <summary>´The type of the spline data fitting.
        /// </summary>
        public enum SplineType
        {
            /// <summary>The default spline type. You can use this type with linear, quadratic, or user-defined splines.
            /// </summary>
            DF_PP_DEFAULT = 0,

            /// <summary>Quadratic splines based on Subbotin algorithm, [StechSub76].
            /// </summary>
            DF_PP_SUBBOTIN = 1,

            /// <summary>Natural cubic spline.
            /// </summary>
            DF_PP_NATURAL = 2,

            /// <summary>Hermite cubic spline.
            /// </summary>
            DF_PP_HERMITE = 3,

            /// <summary>Bessel cubic spline.
            /// </summary>
            DF_PP_BESSEL = 4,

            /// <summary>Akima cubic spline.
            /// </summary>
            DF_PP_AKIMA = 5,

            /// <summary>Look-up interpolant.
            /// </summary>
            DF_LOOKUP_INTERPOLANT = 6,

            /// <summary>Continuous right step-wise constant interpolant.
            /// </summary>
            DF_CR_STEPWISE_CONST_INTERPOLANT = 7,

            /// <summary>Continuous left step-wise constant interpolant. 
            /// </summary>
            DF_CL_STEPWISE_CONST_INTERPOLANT = 8
        }

        /// <summary>The type of the boundary condition.
        /// </summary>
        [Flags]
        public enum SplineBoundaryCondition
        {
            /// <summary>No boundary conditions provided. Can be applied to all type of splines.
            /// </summary>
            DF_NO_BC = 0,

            /// <summary>Not-a-knot boundary condition. Can be applied to: Akima, Bessel, Hermite, Natural cubic.
            /// </summary>
            DF_BC_NOT_A_KNOT = 1,

            /// <summary>Free-end boundary conditions. Can be applied to: Akim, Bessel, Hermite, Natural Cubic, quadratic Subbotin.
            /// </summary>
            DF_BC_FREE_END = 2,

            /// <summary>The first derivative at the left endpoint. Can be applied to: Akim, Bessel, Hermite, Natural Cubic, quadratic Subbotin.
            /// </summary>
            DF_BC_1ST_LEFT_DER = 4,

            /// <summary>The first derivative at the right endpoint. Can be applied to: Akim, Bessel, Hermite, Natural Cubic, quadratic Subbotin.
            /// </summary>
            DF_BC_1ST_RIGHT_DER = 8,

            /// <summary>The second derivative at the left endpoint. Can be applied to: Akim, Bessel, Hermite, Natural Cubic, quadratic Subbotin.
            /// </summary>
            DF_BC_2ND_LEFT_DER = 16,

            /// <summary>The second derivative at the right endpoint. Can be applied to: Akim, Bessel, Hermite, Natural Cubic, quadratic Subbotin.
            /// </summary>
            DF_BC_2ND_RIGHT_DER = 32,

            /// <summary>Periodic boundary conditions. Can be applied to: Linear, all cubic splines.
            /// </summary>
            DF_BC_PERIODIC = 64,

            /// <summary>Function value at point 1/2 * (x_0 + x_1). Can be applied to: Default quadratic.
            /// </summary>
            DF_BC_Q_VAL = 128
        }

        /// <summary>Internal conditions supported in the Data Fitting domain that you can use for the <c>ic_type</c> parameter.
        /// </summary>
        public enum SplineInternalConditionType
        {
            /// <summary>No internal conditions provided.
            /// </summary>
            DF_NO_IC = 0,

            /// <summary>Array of first derivatives of size <c>n</c>-2, where <c>n</c> is the number of breakpoints (grid points). Deratives are applicable to each coordinate of the vector-valued function.
            /// Can be applied to: Hermite cubic.
            /// </summary>
            DF_IC_1ST_DER = 1,

            /// <summary>Array of second derivatives of size <c>n</c>-2, where <c>n</c> is the number of breakpoints (grid points). Deratives are applicable to each coordinate of the vector-valued function.
            /// Can be applied to: Default cubic.
            /// </summary>
            DF_IC_2ND_DER = 2,

            /// <summary>Knot array of size <c>n</c>+1, where <c>n</c> is the number of breakpoints (grid points). Can be applied to: Subbotin quadratic.
            /// </summary>
            DF_IC_Q_KNOT = 8
        }

        /// <summary>Describes the spline coefficients.
        /// </summary>
        public enum SplineCoefficientStorageFormat
        {
            /// <summary>No hint is provided. By default, all sets of spline coefficients are stored in row-major format.
            /// </summary>
            DF_NO_HINT = 0x00,

            /// <summary>The first coordinate of vector-valued data is provided.
            /// </summary>
            DF_1ST_COORDINATE = 0x00000080
        }

        /// <summary>Describes the format of the splines.
        /// </summary>
        public enum SplineFormat
        {
            /// <summary>The standard format of the splines.
            /// </summary>
            DF_PP_SPLINE = 0x00
        }

        /// <summary>Describes the Spline construction method.
        /// </summary>
        public enum SplineConstructionMethod
        {
            /// <summary>The standard construction method.
            /// </summary>
            DF_METHOD_STD = 0
        }
        #endregion

        #region public (readonly) members

        /// <summary>Provides curve interpolation approaches based on natural cubic splines.
        /// </summary>
        public readonly MklGridPointCurve.Interpolator NaturalCubicSpline;

        /// <summary>Provides the Bessel (Hermite) cubic spline curve interpolation.
        /// </summary>
        /// <remarks>This interpolation is also named 'Hermite interpolation'. This approach contains already spline boundary conditions.</remarks>
        public readonly MklGridPointCurve.Interpolator BesselCubicSpline;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="MklCurveInterpolationSpline" /> class.
        /// </summary>
        internal MklCurveInterpolationSpline()
        {
            NaturalCubicSpline = new MklDataFitting(SplineOrder.DF_PP_CUBIC, SplineType.DF_PP_NATURAL, SplineBoundaryCondition.DF_BC_FREE_END);
            BesselCubicSpline = new MklDataFitting(MklCurveInterpolationSpline.SplineOrder.DF_PP_CUBIC, MklCurveInterpolationSpline.SplineType.DF_PP_BESSEL, MklCurveInterpolationSpline.SplineBoundaryCondition.DF_BC_NOT_A_KNOT, null);
        }
        #endregion

        #region public methods

        /// <summary>Creates a <see cref="MklGridPointCurve.Interpolator"/> object that represents the implementation of a specified interpolation approach.
        /// </summary>
        /// <param name="splineOrder">The spline order.</param>
        /// <param name="splineType">The type of the spline.</param>
        /// <param name="boundaryConditionType">The type of the boundary condition.</param>
        /// <param name="boundaryCondition">The boundary condition.</param>
        /// <param name="splineCoefficientHint">The spline coefficient hint.</param>
        /// <param name="internalConditionType">The internal boundary condition type.</param>
        /// <param name="internalConditions">The internal boundary condition.</param>
        /// <returns>A <see cref="MklGridPointCurve.Interpolator"/> object that represents the implementation of the specified interpolation approach.</returns>
        public MklGridPointCurve.Interpolator Create(SplineOrder splineOrder, SplineType splineType, SplineBoundaryCondition boundaryConditionType, double[] boundaryCondition = null, SplineCoefficientStorageFormat splineCoefficientHint = SplineCoefficientStorageFormat.DF_NO_HINT, SplineInternalConditionType internalConditionType = SplineInternalConditionType.DF_NO_IC, double[] internalConditions = null)
        {
            return new MklDataFitting(splineOrder, splineType, boundaryConditionType, boundaryCondition, splineCoefficientHint, internalConditionType, internalConditions);
        }
        #endregion
    }
}