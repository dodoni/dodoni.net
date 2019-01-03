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
    /// <summary>Serves as factory for <see cref="IGridPointCurve{TLabel}"/> objects. Moreover it provides some basic interpolation and extrapolation approaches, as
    /// (log-)linear interpolation, constant interpolation, constant extrapolation etc.
    /// </summary>
    public static partial class GridPointCurve
    {
        /// <summary>Represents the state of a curve object, i.e. indicates whether grid points have been changed, added etc. since the last call of <see cref="IGridPointCurve.Update()"/>.
        /// </summary>
        [Flags]
        public enum State
        {
            /// <summary>No grid point value or grid point arguments has been changed, i.e. the curve object should be operable.
            /// </summary>
            NoChangeSinceLastUpdate = 0,

            /// <summary>At least one values of a specific grid point has been changed. The grid point arguments (i.e. labels of the x-axis) have not been changed.
            /// </summary>
            GridPointValueChanged = 1,

            /// <summary>At least one grid point argument (i.e. label on the x-axis) has been changed. No grid point value has been changed.
            /// </summary>
            GridPointArgumentChanged = 2,

            /// <summary>At least one grid point has been changed, new grid points have been added, or grid points have been removed since the last update.
            /// </summary>
            GridPointChanged = GridPointValueChanged | GridPointArgumentChanged
        }

        #region generic grid point curve factory methods

        /// <summary>Creates a new <see cref="IGridPointCurve&lt;TLabel&gt;"/> object with respect to specified interpolation and extrapolation approaches.
        /// </summary>
        /// <typeparam name="TLabel">The type of the label.</typeparam>
        /// <param name="curveInterpolator">The curve interpolator.</param>
        /// <param name="leftExtrapolator">The extrapolator on the left side, i.e. from the first grid point to -\infinity.</param>
        /// <param name="rightExtrapolator">The extrapolator on the right side, i.e. from the last grid point to \infinity.</param>
        /// <param name="capacity">The number of elements that the new grid point curve can initially store.</param>
        /// <returns>A <see cref="IGridPointCurve{TLabel}"/> object with respect to the desired interpolation and extrapolation approaches.</returns>
        public static IGridPointCurve<TLabel> Create<TLabel>(Interpolator curveInterpolator, Extrapolator leftExtrapolator, Extrapolator rightExtrapolator, int capacity = 20)
            where TLabel : IEquatable<TLabel>
        {
            if (curveInterpolator == null)
            {
                throw new ArgumentException(nameof(curveInterpolator));
            }
            var interpolator = curveInterpolator.Create();

            if (leftExtrapolator == null)
            {
                throw new ArgumentNullException(nameof(leftExtrapolator));
            }
            if (leftExtrapolator.ExtrapolationBuildingDirection != Extrapolator.BuildingDirection.FromFirstGridPoint)
            {
                throw new ArgumentException("Wrong building direction of curve extrapolation", "leftExtrapolator");
            }
            var left = leftExtrapolator.Create(interpolator);

            if (rightExtrapolator == null)
            {
                throw new ArgumentNullException(nameof(rightExtrapolator));
            }
            var right = rightExtrapolator.Create(interpolator);
            if (rightExtrapolator.ExtrapolationBuildingDirection != Extrapolator.BuildingDirection.FromLastGridPoint)
            {
                throw new ArgumentException("Wrong building direction of curve extrapolation", "rightExtrapolation");
            }

            if ((interpolator is IDifferentiableRealValuedCurve) && (left is IDifferentiableRealValuedCurve) && (right is IDifferentiableRealValuedCurve))
            {
                return new StandardGridPointCurve<TLabel>.Differentiable(curveInterpolator, interpolator, leftExtrapolator, left, rightExtrapolator, right, capacity);
            }
            return new StandardGridPointCurve<TLabel>(curveInterpolator, interpolator, leftExtrapolator, left, rightExtrapolator, right, capacity);
        }

        /// <summary>Creates a new <see cref="IGridPointCurve&lt;TLabel&gt;"/> object with respect to a specified curve parametrization.
        /// </summary>
        /// <typeparam name="TLabel">The type of the label.</typeparam>
        /// <param name="curveParametrization">The curve parametrization.</param>
        /// <param name="capacity">The number of elements that the new grid point curve can initially store.</param>
        /// <returns>A <see cref="IGridPointCurve{TLabel}"/> object with respect to a specified curve parametrization.</returns>
        public static IGridPointCurve<TLabel> Create<TLabel>(Parametrization curveParametrization, int capacity = 20)
                 where TLabel : IEquatable<TLabel>
        {
            if (curveParametrization == null)
            {
                throw new ArgumentException(nameof(curveParametrization));
            }
            var parametrization = curveParametrization.Create();

            if (parametrization is IDifferentiableRealValuedCurve)
            {
                return new StandardGridPointCurve<TLabel>.Differentiable(curveParametrization, parametrization, capacity);
            }
            return new StandardGridPointCurve<TLabel>(curveParametrization, parametrization, capacity);
        }
        #endregion

        #region grid point curve (without labels) factory methods

        /// <summary>Creates a new <see cref="IGridPointCurve&lt;TLabel&gt;"/> object with respect to specified interpolation and extrapolation approaches.
        /// </summary>
        /// <param name="curveInterpolator">The curve interpolator.</param>
        /// <param name="leftExtrapolator">The extrapolator on the left side, i.e. from the first grid point to -\infinity.</param>
        /// <param name="rightExtrapolator">The extrapolator on the right side, i.e. from the last grid point to \infinity.</param>
        /// <param name="capacity">The number of elements that the new grid point curve can initially store.</param>
        /// <returns>A <see cref="IGridPointCurve{TLabel}"/> object with respect to the desired interpolation and extrapolation approaches.</returns>
        public static IGridPointCurve<double> Create(Interpolator curveInterpolator, Extrapolator leftExtrapolator, Extrapolator rightExtrapolator, int capacity = 20)
        {
            if (curveInterpolator == null)
            {
                throw new ArgumentException(nameof(curveInterpolator));
            }
            var interpolator = curveInterpolator.Create();

            if (leftExtrapolator == null)
            {
                throw new ArgumentNullException(nameof(leftExtrapolator));
            }
            if (leftExtrapolator.ExtrapolationBuildingDirection != Extrapolator.BuildingDirection.FromFirstGridPoint)
            {
                throw new ArgumentException("Wrong building direction of curve extrapolation", "leftExtrapolator");
            }
            var left = leftExtrapolator.Create(interpolator);

            if (rightExtrapolator == null)
            {
                throw new ArgumentNullException(nameof(rightExtrapolator));
            }
            var right = rightExtrapolator.Create(interpolator);
            if (rightExtrapolator.ExtrapolationBuildingDirection != Extrapolator.BuildingDirection.FromLastGridPoint)
            {
                throw new ArgumentException("Wrong building direction of curve extrapolation", "rightExtrapolation");
            }

            if ((interpolator is IDifferentiableRealValuedCurve) && (left is IDifferentiableRealValuedCurve) && (right is IDifferentiableRealValuedCurve))
            {
                return new StandardGridPointCurveNoLabels.Differentiable(curveInterpolator, interpolator, leftExtrapolator, left, rightExtrapolator, right, capacity);
            }
            return new StandardGridPointCurveNoLabels(curveInterpolator, interpolator, leftExtrapolator, left, rightExtrapolator, right, capacity);
        }

        /// <summary>Creates a new <see cref="IGridPointCurve&lt;TLabel&gt;"/> object with respect to a specified curve parametrization.
        /// </summary>
        /// <param name="curveParametrization">The curve parametrization.</param>
        /// <param name="capacity">The number of elements that the new grid point curve can initially store.</param>
        /// <returns>A <see cref="IGridPointCurve{TLabel}"/> object with respect to a specified curve parametrization.</returns>
        public static IGridPointCurve<double> Create(Parametrization curveParametrization, int capacity = 20)
        {
            if (curveParametrization == null)
            {
                throw new ArgumentException(nameof(curveParametrization));
            }
            var parametrization = curveParametrization.Create();

            if (parametrization is IDifferentiableRealValuedCurve)
            {
                return new StandardGridPointCurveNoLabels.Differentiable(curveParametrization, parametrization, capacity);
            }
            return new StandardGridPointCurveNoLabels(curveParametrization, parametrization, capacity);
        }
        #endregion

        #region read-only grid point curve factory methods

        /// <summary>Creates a new read-only <see cref="IGridPointCurve&lt;TLabel&gt;"/> object with respect to specified interpolation and extrapolation approaches.
        /// </summary>
        /// <typeparam name="TLabel">The type of the label.</typeparam>
        /// <param name="curveInterpolator">The curve interpolator.</param>
        /// <param name="leftExtrapolator">The extrapolator on the left side, i.e. from the first grid point to -\infinity.</param>
        /// <param name="rightExtrapolator">The extrapolator on the right side, i.e. from the last grid point to \infinity.</param>
        /// <param name="gridPointCount">The number of grid points, i.e. the number of relevant elements of <paramref name="gridPointLabels"/>, <paramref name="gridPointArguments"/> and <paramref name="gridPointValues"/> to take into account.</param>
        /// <param name="gridPointLabels">The labels of the grid points (the reference will be stored only).</param>
        /// <param name="gridPointArguments">The arguments of the grid points, thus labels of the curve in its <see cref="System.Double"/> representation in ascending order.</param>
        /// <param name="gridPointValues">The values of the grid points corresponding to <paramref name="gridPointArguments"/>.</param>
        /// <param name="gridPointArgumentStartIndex">The null-based start index of <paramref name="gridPointArguments"/> and <paramref name="gridPointLabels"/> to take into account.</param>
        /// <param name="gridPointValueStartIndex">The null-based start index of <paramref name="gridPointValues"/> to take into account.</param>
        /// <param name="gridPointArgumentIncrement">The increment for <paramref name="gridPointArguments"/> and <paramref name="gridPointLabels"/>.</param>
        /// <param name="gridPointValueIncrement">The increment for <paramref name="gridPointValues"/>.</param>
        /// <returns>A read-only <see cref="IGridPointCurve&lt;TLabel&gt;"/> object with respect to the desired interpolation and extrapolation approaches.</returns>
        /// <remarks>This method is mainly used for two-dimensional surface interpolation. Assuming a large grid point matrix, for example 1.000 x 1.000 and one may apply for example a linear interpolation 
        /// along horizontal direction and afterwards a linear interpolation along vertical direction. Then we create 1.000 curves in horizontal direction, where the (double) labels and values are constant. The
        /// standard implementation create a copy of 3 * 1.000 x 1.000 = 3.000.000 values and calles the update method of the underlying <see cref="ICurveDataFitting"/> object which again copy 2 * 1.000 * 1.000 = 2.000.000 values, i.e. in total 5 Mio values.
        /// This implementation takes into account references, the underlying <see cref="ICurveDataFitting"/> implementation creates deep copies of double values and grid point values only, i.e. 2 Mio double values.</remarks>
        public static IGridPointCurve<TLabel> Create<TLabel>(Interpolator curveInterpolator, Extrapolator leftExtrapolator, Extrapolator rightExtrapolator, int gridPointCount, IList<TLabel> gridPointLabels, IList<double> gridPointArguments, IList<double> gridPointValues, int gridPointArgumentStartIndex = 0, int gridPointValueStartIndex = 0, int gridPointArgumentIncrement = 1, int gridPointValueIncrement = 1)
            where TLabel : IEquatable<TLabel>
        {
            if (curveInterpolator == null)
            {
                throw new ArgumentException(nameof(curveInterpolator));
            }
            var interpolator = curveInterpolator.Create();

            if (leftExtrapolator == null)
            {
                throw new ArgumentNullException(nameof(leftExtrapolator));
            }
            if (leftExtrapolator.ExtrapolationBuildingDirection != Extrapolator.BuildingDirection.FromFirstGridPoint)
            {
                throw new ArgumentException("Wrong building direction of curve extrapolation", "leftExtrapolator");
            }
            var left = leftExtrapolator.Create(interpolator);

            if (rightExtrapolator == null)
            {
                throw new ArgumentNullException(nameof(rightExtrapolator));
            }
            var right = rightExtrapolator.Create(interpolator);
            if (rightExtrapolator.ExtrapolationBuildingDirection != Extrapolator.BuildingDirection.FromLastGridPoint)
            {
                throw new ArgumentException("Wrong building direction of curve extrapolation", "rightExtrapolation");
            }

            if ((interpolator is IDifferentiableRealValuedCurve) && (left is IDifferentiableRealValuedCurve) && (right is IDifferentiableRealValuedCurve))
            {
                return new SmartReadOnlyGridPointCurve<TLabel>.Differentiable(interpolator, left, right, gridPointCount, gridPointLabels, gridPointArguments, gridPointValues, gridPointArgumentStartIndex, gridPointValueStartIndex, gridPointArgumentIncrement, gridPointValueIncrement);
            }
            return new SmartReadOnlyGridPointCurve<TLabel>(interpolator, left, right, gridPointCount, gridPointLabels, gridPointArguments, gridPointValues, gridPointArgumentStartIndex, gridPointValueStartIndex, gridPointArgumentIncrement, gridPointValueIncrement);
        }

        /// <summary>Creates a new read-only <see cref="IGridPointCurve&lt;TLabel&gt;"/> object with respect to a specified curve parametrization.
        /// </summary>
        /// <typeparam name="TLabel">The type of the label.</typeparam>
        /// <param name="curveParametrization">The curve parametrization.</param>
        /// <param name="gridPointCount">The number of grid points, i.e. the number of relevant elements of <paramref name="gridPointLabels"/>, <paramref name="gridPointArguments"/> and <paramref name="gridPointValues"/> to take into account.</param>
        /// <param name="gridPointLabels">The labels of the grid points (the reference will be stored only).</param>
        /// <param name="gridPointArguments">The arguments of the grid points, thus labels of the curve in its <see cref="System.Double"/> representation in ascending order.</param>
        /// <param name="gridPointValues">The values of the grid points corresponding to <paramref name="gridPointArguments"/>.</param>
        /// <param name="gridPointArgumentStartIndex">The null-based start index of <paramref name="gridPointArguments"/> and <paramref name="gridPointLabels"/> to take into account.</param>
        /// <param name="gridPointValueStartIndex">The null-based start index of <paramref name="gridPointValues"/> to take into account.</param>
        /// <param name="gridPointArgumentIncrement">The increment for <paramref name="gridPointArguments"/> and <paramref name="gridPointLabels"/>.</param>
        /// <param name="gridPointValueIncrement">The increment for <paramref name="gridPointValues"/>.</param>
        /// <returns>A read-only <see cref="IGridPointCurve&lt;TLabel&gt;"/> object with respect to the desired interpolation and extrapolation approaches.</returns>
        /// <remarks>This method is mainly used for two-dimensional surface interpolation. Assuming a large grid point matrix, for example 1.000 x 1.000 and one may apply for example a linear interpolation 
        /// along horizontal direction and afterwards a linear interpolation along vertical direction. Then we create 1.000 curves in horizontal direction, where the (double) labels and values are constant. The
        /// standard implementation create a copy of 3 * 1.000 x 1.000 = 3.000.000 values and calles the update method of the underlying <see cref="ICurveDataFitting"/> object which again copy 2 * 1.000 * 1.000 = 2.000.000 values, i.e. in total 5 Mio values.
        /// This implementation takes into account references, the underlying <see cref="ICurveDataFitting"/> implementation should create deep copies of grid point arguments/values only, i.e. 2 Mio double values.</remarks>
        public static IGridPointCurve<TLabel> Create<TLabel>(Parametrization curveParametrization, int gridPointCount, IList<TLabel> gridPointLabels, IList<double> gridPointArguments, IList<double> gridPointValues, int gridPointArgumentStartIndex = 0, int gridPointValueStartIndex = 0, int gridPointArgumentIncrement = 1, int gridPointValueIncrement = 1)
                 where TLabel : IEquatable<TLabel>
        {
            if (curveParametrization == null)
            {
                throw new ArgumentException(nameof(curveParametrization));
            }
            var parametrization = curveParametrization.Create();

            if (parametrization is IDifferentiableRealValuedCurve)
            {
                return new SmartReadOnlyGridPointCurve<TLabel>.Differentiable(parametrization, gridPointCount, gridPointLabels, gridPointArguments, gridPointValues, gridPointArgumentStartIndex, gridPointValueStartIndex, gridPointArgumentIncrement, gridPointValueIncrement);
            }
            return new SmartReadOnlyGridPointCurve<TLabel>(parametrization, gridPointCount, gridPointLabels, gridPointArguments, gridPointValues, gridPointArgumentStartIndex, gridPointValueStartIndex, gridPointArgumentIncrement, gridPointValueIncrement);
        }
        #endregion
    }
}