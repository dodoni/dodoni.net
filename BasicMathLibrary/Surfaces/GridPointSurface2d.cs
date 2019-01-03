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

using Dodoni.MathLibrary.GridPointCurves;

namespace Dodoni.MathLibrary.Surfaces
{
    /// <summary>Provides methods to create objects that represents a two-dimensional surface, i.e. grid points and a specified interpolation/extrapolation or parametrization which
    /// will applied for each row and each column, respectively.
    /// </summary>
    public partial class GridPointSurface2d
    {
        /// <summary>Indicating the order of the interpolation, extrapolation etc. to construct a two-dimensional surface.
        /// </summary>
        public enum ConstructionOrder
        {
            /// <summary>Initially apply in horizontal direction for each row a specified interpolation/extrapolation/parametrization. 
            /// A interpolation/extrapolation/parametrization in vertical direction will be applied afterwards.
            /// </summary>
            HorizontalVertical,

            /// <summary>Initially apply in vertical direction for each row a specified interpolation/extrapolation/parametrization. 
            /// A interpolation/extrapolation/parametrization in horizontal direction will be applied afterwards.
            /// </summary>
            VerticalHorizontal
        }

        /// <summary>Creates a specified two-dimensional surface.
        /// </summary>
        /// <typeparam name="THorizontalLabel">The type of the horizontal label.</typeparam>
        /// <typeparam name="TVerticalLabel">The type of the vertical label.</typeparam>
        /// <param name="gridPoints">The grid points in its <see cref="LabelMatrix&lt;THorizontalLabel,TVerticalLabel&gt;"/> representation.</param>
        /// <param name="horizontalInterpolator">The interpolation approach along horizontal direction.</param>
        /// <param name="horizontalLeftExtrapolator">The extrapolation approach in horizontal direction on the left.</param>
        /// <param name="horizontalRightExtrapolator">The extrapolation approach in horizontal direction on the right.</param>
        /// <param name="verticalInterpolator">The interpolation approach along vertical direction.</param>
        /// <param name="verticalAboveExtrapolator">The extrapolation approach in vertical direction above the grid points.</param>
        /// <param name="verticalBelowExtrapolator">The extrapolation approach in vertical direction below the grid points.</param>
        /// <param name="constructionOrder">A value indicating the order of the vertical and horizontal interpolation, extrapolation etc.</param>
        /// <returns>An object that repesents the two-dimensional surface with respect to the specified grid points and interpolation/extrapolation.</returns>
        public static IGridPointSurface2d<THorizontalLabel, TVerticalLabel> Create<THorizontalLabel, TVerticalLabel>(LabelMatrix<THorizontalLabel, TVerticalLabel> gridPoints, GridPointCurve.Interpolator horizontalInterpolator, GridPointCurve.Extrapolator horizontalLeftExtrapolator, GridPointCurve.Extrapolator horizontalRightExtrapolator, GridPointCurve.Interpolator verticalInterpolator, GridPointCurve.Extrapolator verticalAboveExtrapolator, GridPointCurve.Extrapolator verticalBelowExtrapolator, ConstructionOrder constructionOrder = ConstructionOrder.HorizontalVertical)
            where THorizontalLabel : IComparable<THorizontalLabel>, IEquatable<THorizontalLabel>
            where TVerticalLabel : IComparable<TVerticalLabel>, IEquatable<TVerticalLabel>
        {
            if (gridPoints == null)
            {
                throw new ArgumentNullException(nameof(gridPoints));
            }
            if (horizontalInterpolator == null)
            {
                throw new ArgumentNullException(nameof(horizontalInterpolator));
            }
            if (horizontalLeftExtrapolator == null)
            {
                throw new ArgumentNullException(nameof(horizontalLeftExtrapolator));
            }
            if (horizontalRightExtrapolator == null)
            {
                throw new ArgumentNullException(nameof(horizontalRightExtrapolator));
            }
            switch (constructionOrder)
            {
                case ConstructionOrder.HorizontalVertical:
                    var horizontalCurveFactory = GridPointCurve.Factory.Create<THorizontalLabel>(horizontalInterpolator, horizontalLeftExtrapolator, horizontalRightExtrapolator);
                    return new HorizontalVerticalWiseSurface2d<THorizontalLabel, TVerticalLabel>.VInterpolation(gridPoints, verticalLabel => horizontalCurveFactory, verticalInterpolator, verticalAboveExtrapolator, verticalBelowExtrapolator);

                case ConstructionOrder.VerticalHorizontal:
                    var verticalCurveFactory = GridPointCurve.Factory.Create<TVerticalLabel>(verticalInterpolator, verticalAboveExtrapolator, verticalBelowExtrapolator);
                    return new VerticalHorizontalWiseSurface2d<THorizontalLabel, TVerticalLabel>.HInterpolation(gridPoints, horizontalLabel => verticalCurveFactory, horizontalInterpolator, horizontalLeftExtrapolator, horizontalRightExtrapolator);

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>Creates a specified two-dimensional surface.
        /// </summary>
        /// <typeparam name="THorizontalLabel">The type of the horizontal label.</typeparam>
        /// <typeparam name="TVerticalLabel">The type of the vertical label.</typeparam>
        /// <param name="gridPoints">The grid points in its <see cref="LabelMatrix&lt;THorizontalLabel,TVerticalLabel&gt;"/> representation.</param>
        /// <param name="horizontalInterpolator">The interpolation approach along horizontal direction.</param>
        /// <param name="horizontalLeftExtrapolator">The extrapolation approach in horizontal direction on the left.</param>
        /// <param name="horizontalRightExtrapolator">The extrapolation approach in horizontal direction on the right.</param>
        /// <param name="verticalParametrization">The (curve) parametrization in vertical direction.</param>
        /// <param name="constructionOrder">A value indicating the order of the vertical and horizontal interpolation, extrapolation etc.</param>
        /// <returns>An object that repesents the two-dimensional surface with respect to the specified grid points and interpolation/extrapolation.</returns>
        public static IGridPointSurface2d<THorizontalLabel, TVerticalLabel> Create<THorizontalLabel, TVerticalLabel>(LabelMatrix<THorizontalLabel, TVerticalLabel> gridPoints, GridPointCurve.Interpolator horizontalInterpolator, GridPointCurve.Extrapolator horizontalLeftExtrapolator, GridPointCurve.Extrapolator horizontalRightExtrapolator, GridPointCurve.Parametrization verticalParametrization, ConstructionOrder constructionOrder = ConstructionOrder.HorizontalVertical)
            where THorizontalLabel : IComparable<THorizontalLabel>, IEquatable<THorizontalLabel>
            where TVerticalLabel : IComparable<TVerticalLabel>, IEquatable<TVerticalLabel>
        {
            if (gridPoints == null)
            {
                throw new ArgumentNullException(nameof(gridPoints));
            }
            if (horizontalInterpolator == null)
            {
                throw new ArgumentNullException(nameof(horizontalInterpolator));
            }
            if (horizontalLeftExtrapolator == null)
            {
                throw new ArgumentNullException(nameof(horizontalLeftExtrapolator));
            }
            if (horizontalRightExtrapolator == null)
            {
                throw new ArgumentNullException(nameof(horizontalRightExtrapolator));
            }
            switch (constructionOrder)
            {
                case ConstructionOrder.HorizontalVertical:
                    var horizontalCurveFactory = GridPointCurve.Factory.Create<THorizontalLabel>(horizontalInterpolator, horizontalLeftExtrapolator, horizontalRightExtrapolator);
                    return new HorizontalVerticalWiseSurface2d<THorizontalLabel, TVerticalLabel>.VParametrization(gridPoints, verticalLabel => horizontalCurveFactory, verticalParametrization);

                case ConstructionOrder.VerticalHorizontal:
                    var verticalCurveFactory = GridPointCurve.Factory.Create<TVerticalLabel>(verticalParametrization);
                    return new VerticalHorizontalWiseSurface2d<THorizontalLabel, TVerticalLabel>.HInterpolation(gridPoints, horizontalLabel => verticalCurveFactory, horizontalInterpolator, horizontalLeftExtrapolator, horizontalRightExtrapolator);

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>Creates a specified two-dimensional surface.
        /// </summary>
        /// <typeparam name="THorizontalLabel">The type of the horizontal label.</typeparam>
        /// <typeparam name="TVerticalLabel">The type of the vertical label.</typeparam>
        /// <param name="gridPoints">The grid points in its <see cref="LabelMatrix&lt;THorizontalLabel,TVerticalLabel&gt;"/> representation.</param>
        /// <param name="horizontalParametrization">The (curve) parametrization in horizontal direction.</param>
        /// <param name="verticalInterpolator">The interpolation approach along vertical direction.</param>
        /// <param name="verticalAboveExtrapolator">The extrapolation approach in vertical direction above the grid points.</param>
        /// <param name="verticalBelowExtrapolator">The extrapolation approach in vertical direction below the grid points.</param>
        /// <param name="constructionOrder">A value indicating the order of the vertical and horizontal interpolation, extrapolation etc.</param>
        /// <returns>An object that repesents the two-dimensional surface with respect to the specified grid points and interpolation/extrapolation.</returns>
        public static IGridPointSurface2d<THorizontalLabel, TVerticalLabel> Create<THorizontalLabel, TVerticalLabel>(LabelMatrix<THorizontalLabel, TVerticalLabel> gridPoints, GridPointCurve.Parametrization horizontalParametrization, GridPointCurve.Interpolator verticalInterpolator, GridPointCurve.Extrapolator verticalAboveExtrapolator, GridPointCurve.Extrapolator verticalBelowExtrapolator, ConstructionOrder constructionOrder = ConstructionOrder.HorizontalVertical)
            where THorizontalLabel : IComparable<THorizontalLabel>, IEquatable<THorizontalLabel>
            where TVerticalLabel : IComparable<TVerticalLabel>, IEquatable<TVerticalLabel>
        {
            if (gridPoints == null)
            {
                throw new ArgumentNullException(nameof(gridPoints));
            }
            switch (constructionOrder)
            {
                case ConstructionOrder.HorizontalVertical:
                    var horizontalCurveFactory = GridPointCurve.Factory.Create<THorizontalLabel>(horizontalParametrization);
                    return new HorizontalVerticalWiseSurface2d<THorizontalLabel, TVerticalLabel>.VInterpolation(gridPoints, verticalLabel => horizontalCurveFactory, verticalInterpolator, verticalAboveExtrapolator, verticalBelowExtrapolator);

                case ConstructionOrder.VerticalHorizontal:
                    var verticalCurveFactory = GridPointCurve.Factory.Create<TVerticalLabel>(verticalInterpolator, verticalAboveExtrapolator, verticalBelowExtrapolator);
                    return new VerticalHorizontalWiseSurface2d<THorizontalLabel, TVerticalLabel>.HParametrization(gridPoints, horizontalLabel => verticalCurveFactory, horizontalParametrization);

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>Creates a specified two-dimensional surface.
        /// </summary>
        /// <typeparam name="THorizontalLabel">The type of the horizontal label.</typeparam>
        /// <typeparam name="TVerticalLabel">The type of the vertical label.</typeparam>
        /// <param name="gridPoints">The grid points in its <see cref="LabelMatrix&lt;THorizontalLabel,TVerticalLabel&gt;"/> representation.</param>
        /// <param name="horizontalParametrization">The (curve) parametrization in horizontal direction.</param>
        /// <param name="verticalParametrization">The (curve) parametrization in vertical direction.</param>
        /// <param name="constructionOrder">A value indicating the order of the vertical and horizontal interpolation, extrapolation etc.</param>
        /// <returns>An object that repesents the two-dimensional surface with respect to the specified grid points and interpolation/extrapolation.</returns>
        public static IGridPointSurface2d<THorizontalLabel, TVerticalLabel> Create<THorizontalLabel, TVerticalLabel>(LabelMatrix<THorizontalLabel, TVerticalLabel> gridPoints, GridPointCurve.Parametrization horizontalParametrization, GridPointCurve.Parametrization verticalParametrization, ConstructionOrder constructionOrder = ConstructionOrder.HorizontalVertical)
            where THorizontalLabel : IComparable<THorizontalLabel>, IEquatable<THorizontalLabel>
            where TVerticalLabel : IComparable<TVerticalLabel>, IEquatable<TVerticalLabel>
        {
            if (gridPoints == null)
            {
                throw new ArgumentNullException(nameof(gridPoints));
            }
            switch (constructionOrder)
            {
                case ConstructionOrder.HorizontalVertical:
                    var horizontalCurveFactory = GridPointCurve.Factory.Create<THorizontalLabel>(horizontalParametrization);
                    return new HorizontalVerticalWiseSurface2d<THorizontalLabel, TVerticalLabel>.VParametrization(gridPoints, verticalLabel => horizontalCurveFactory, verticalParametrization);

                case ConstructionOrder.VerticalHorizontal:
                    var verticalCurveFactory = GridPointCurve.Factory.Create<TVerticalLabel>(verticalParametrization);
                    return new VerticalHorizontalWiseSurface2d<THorizontalLabel, TVerticalLabel>.HParametrization(gridPoints, horizontalLabel => verticalCurveFactory, horizontalParametrization);

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>Creates a specified two-dimensional surface, where the interpolation, parametrization etc. takes place first in horizontal direction and then in vertical direction.
        /// </summary>
        /// <typeparam name="THorizontalLabel">The type of the horizontal label.</typeparam>
        /// <typeparam name="TVerticalLabel">The type of the vertical label.</typeparam>
        /// <param name="gridPoints">The grid points in its <see cref="LabelMatrix&lt;THorizontalLabel,TVerticalLabel&gt;"/> representation.</param>
        /// <param name="horizontalCurveFactory">A factory for horizontal grid point curves.</param>
        /// <param name="verticalInterpolator">The interpolation approach along vertical direction.</param>
        /// <param name="verticalAboveExtrapolator">The extrapolation approach in vertical direction above the grid points.</param>
        /// <param name="verticalBelowExtrapolator">The extrapolation approach in vertical direction below the grid points.</param>
        /// <returns>An object that repesents the two-dimensional surface with respect to the specified grid points and interpolation/extrapolation.</returns>
        public static IGridPointSurface2d<THorizontalLabel, TVerticalLabel> Create<THorizontalLabel, TVerticalLabel>(LabelMatrix<THorizontalLabel, TVerticalLabel> gridPoints, Func<TVerticalLabel, IGridPointCurveFactory<THorizontalLabel>> horizontalCurveFactory, GridPointCurve.Interpolator verticalInterpolator, GridPointCurve.Extrapolator verticalAboveExtrapolator, GridPointCurve.Extrapolator verticalBelowExtrapolator)
            where THorizontalLabel : IComparable<THorizontalLabel>, IEquatable<THorizontalLabel>
            where TVerticalLabel : IComparable<TVerticalLabel>, IEquatable<TVerticalLabel>
        {
            if (gridPoints == null)
            {
                throw new ArgumentNullException(nameof(gridPoints));
            }
            if (horizontalCurveFactory == null)
            {
                throw new ArgumentNullException(nameof(horizontalCurveFactory));
            }
            return new HorizontalVerticalWiseSurface2d<THorizontalLabel, TVerticalLabel>.VInterpolation(gridPoints, horizontalCurveFactory, verticalInterpolator, verticalAboveExtrapolator, verticalBelowExtrapolator);
        }

        /// <summary>Creates a specified two-dimensional surface, where the interpolation, parametrization etc. takes place first in horizontal direction and then in vertical direction.
        /// </summary>
        /// <typeparam name="THorizontalLabel">The type of the horizontal label.</typeparam>
        /// <typeparam name="TVerticalLabel">The type of the vertical label.</typeparam>
        /// <param name="gridPoints">The grid points in its <see cref="LabelMatrix&lt;THorizontalLabel,TVerticalLabel&gt;"/> representation.</param>
        /// <param name="horizontalCurveFactory">A factory for horizontal grid point curves.</param>
        /// <param name="verticalParametrization">The (curve) parametrization in vertical direction.</param>
        /// <returns>An object that repesents the two-dimensional surface with respect to the specified grid points and interpolation/extrapolation.</returns>
        public static IGridPointSurface2d<THorizontalLabel, TVerticalLabel> Create<THorizontalLabel, TVerticalLabel>(LabelMatrix<THorizontalLabel, TVerticalLabel> gridPoints, Func<TVerticalLabel, IGridPointCurveFactory<THorizontalLabel>> horizontalCurveFactory, GridPointCurve.Parametrization verticalParametrization)
            where THorizontalLabel : IComparable<THorizontalLabel>, IEquatable<THorizontalLabel>
            where TVerticalLabel : IComparable<TVerticalLabel>, IEquatable<TVerticalLabel>
        {
            if (gridPoints == null)
            {
                throw new ArgumentNullException(nameof(gridPoints));
            }
            if (horizontalCurveFactory == null)
            {
                throw new ArgumentNullException(nameof(horizontalCurveFactory));
            }
            return new HorizontalVerticalWiseSurface2d<THorizontalLabel, TVerticalLabel>.VParametrization(gridPoints, horizontalCurveFactory, verticalParametrization);
        }

        /// <summary>Creates a specified two-dimensional surface, where the interpolation, parametrization etc. takes place first in vertical direction and then in horizontal direction.
        /// </summary>
        /// <typeparam name="THorizontalLabel">The type of the horizontal label.</typeparam>
        /// <typeparam name="TVerticalLabel">The type of the vertical label.</typeparam>
        /// <param name="gridPoints">The grid points in its <see cref="LabelMatrix&lt;THorizontalLabel,TVerticalLabel&gt;"/> representation.</param>
        /// <param name="verticalCurveFactory">A factory for horizontal grid point curves.</param>
        /// <param name="horizontalInterpolator">The interpolation approach along horizontal direction.</param>
        /// <param name="horizontalLeftExtrapolator">The extrapolation approach in horizontal direction on the left.</param>
        /// <param name="horizontalRightExtrapolator">The extrapolation approach in horizontal direction on the right.</param>
        /// <returns>An object that repesents the two-dimensional surface with respect to the specified grid points and interpolation/extrapolation.</returns>
        public static IGridPointSurface2d<THorizontalLabel, TVerticalLabel> Create<THorizontalLabel, TVerticalLabel>(LabelMatrix<THorizontalLabel, TVerticalLabel> gridPoints, Func<THorizontalLabel, IGridPointCurveFactory<TVerticalLabel>> verticalCurveFactory, GridPointCurve.Interpolator horizontalInterpolator, GridPointCurve.Extrapolator horizontalLeftExtrapolator, GridPointCurve.Extrapolator horizontalRightExtrapolator)
            where THorizontalLabel : IComparable<THorizontalLabel>, IEquatable<THorizontalLabel>
            where TVerticalLabel : IComparable<TVerticalLabel>, IEquatable<TVerticalLabel>
        {
            if (gridPoints == null)
            {
                throw new ArgumentNullException(nameof(gridPoints));
            }
            if (verticalCurveFactory == null)
            {
                throw new ArgumentNullException(nameof(verticalCurveFactory));
            }
            return new VerticalHorizontalWiseSurface2d<THorizontalLabel, TVerticalLabel>.HInterpolation(gridPoints, verticalCurveFactory, horizontalInterpolator, horizontalLeftExtrapolator, horizontalRightExtrapolator);
        }

        /// <summary>Creates a specified two-dimensional surface, where the interpolation, parametrization etc. takes place first in vertical direction and then in horizontal direction.
        /// </summary>
        /// <typeparam name="THorizontalLabel">The type of the horizontal label.</typeparam>
        /// <typeparam name="TVerticalLabel">The type of the vertical label.</typeparam>
        /// <param name="gridPoints">The grid points in its <see cref="LabelMatrix&lt;THorizontalLabel,TVerticalLabel&gt;"/> representation.</param>
        /// <param name="verticalCurveFactory">A factory for horizontal grid point curves.</param>
        /// <param name="horizontalParametrization">The (curve) parametrization in horizontal direction.</param>
        /// <returns>An object that repesents the two-dimensional surface with respect to the specified grid points and interpolation/extrapolation.</returns>
        public static IGridPointSurface2d<THorizontalLabel, TVerticalLabel> Create<THorizontalLabel, TVerticalLabel>(LabelMatrix<THorizontalLabel, TVerticalLabel> gridPoints, Func<THorizontalLabel, IGridPointCurveFactory<TVerticalLabel>> verticalCurveFactory, GridPointCurve.Parametrization horizontalParametrization)
            where THorizontalLabel : IComparable<THorizontalLabel>, IEquatable<THorizontalLabel>
            where TVerticalLabel : IComparable<TVerticalLabel>, IEquatable<TVerticalLabel>
        {
            if (gridPoints == null)
            {
                throw new ArgumentNullException(nameof(gridPoints));
            }
            if (verticalCurveFactory == null)
            {
                throw new ArgumentNullException(nameof(verticalCurveFactory));
            }
            return new VerticalHorizontalWiseSurface2d<THorizontalLabel, TVerticalLabel>.HParametrization(gridPoints, verticalCurveFactory, horizontalParametrization);
        }
    }
}