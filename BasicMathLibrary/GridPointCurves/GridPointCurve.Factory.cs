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
        /// <summary>Provide methods for a factory of specific grid point curves.
        /// </summary>
        public class Factory
        {
            #region private nested classes

            /// <summary>Serves a standard implementation for a grid point curve factory.
            /// </summary>
            /// <typeparam name="TLabel">The type of the label.</typeparam>
            private class CurveFactory<TLabel> : IGridPointCurveFactory<TLabel>
                where TLabel : IEquatable<TLabel>
            {
                #region private members

                /// <summary>The factory delegate.
                /// </summary>
                private Func<IGridPointCurve<TLabel>> m_FactoryDelegate;

                /// <summary>Represents the interpolation or parametrization approach in its <see cref="ICurveDataFittingFactory"/> representation.
                /// </summary>
                private ICurveDataFittingFactory m_CurveDataFittingFactory;

                /// <summary>The factory delegate for read-only grid point curves.
                /// </summary>
                private Func<int, IList<TLabel>, IList<double>, IList<double>, int, int, int, int, IGridPointCurve<TLabel>> m_ReadOnlyFactoryDelegate;
                #endregion

                #region internal constructors

                /// <summary>Initializes a new instance of the <see cref="CurveFactory&lt;TLabel&gt;"/> class.
                /// </summary>
                /// <param name="factoryDelegate">The factory delegate.</param>
                /// <param name="curveDataFittingFactory">Represents the interpolation or parametrization approach in its <see cref="ICurveDataFittingFactory"/> representation.</param>
                /// <param name="readyOnlyFactoryDelegate">A delegate for creating read-only grid point curves.</param>
                internal CurveFactory(Func<IGridPointCurve<TLabel>> factoryDelegate, ICurveDataFittingFactory curveDataFittingFactory, Func<int, IList<TLabel>, IList<double>, IList<double>, int, int, int, int, IGridPointCurve<TLabel>> readyOnlyFactoryDelegate)
                {
                    m_FactoryDelegate = factoryDelegate;
                    m_ReadOnlyFactoryDelegate = readyOnlyFactoryDelegate;
                    m_CurveDataFittingFactory = curveDataFittingFactory;
                }
                #endregion

                #region IGridPointCurveFactory<TLabel> Members

                /// <summary>Gets a value indicating the fitting quality, i.e. whether grid points are meet exactly. 
                /// </summary>
                /// <value>A value indicating the fitting quality, i.e. whether grid points are meet exactly.</value>
                public FittingQuality FittingQuality
                {
                    get { return m_CurveDataFittingFactory.FittingQuality; }
                }

                /// <summary>Gets the minimal number of grid points required for the grid point curve.
                /// </summary>
                /// <value>The minimal number of grid points.</value>
                /// <remarks>In general at least two grid points are required.</remarks>
                public int MinimalRequiredNumberOfGridPoints
                {
                    get { return m_CurveDataFittingFactory.MinimalRequiredNumberOfGridPoints; }
                }

                /// <summary>Gets the left localness level for a specific grid point, i.e.
                /// changing grid point (t_j,x_j) implies changes on the interval ]t_{j-leftLocalnessLevel}, t_{j+rightLocalnessLevel}[.
                /// </summary>
                /// <param name="gridPointIndex">The null-based index of the grid point (t_j,x_j).</param>
                /// <param name="gridPointCount">The number of grid points.</param>
                /// <returns>The left localness level with respect to the grid point (t_j,x_j), where j is represented by <paramref name="gridPointIndex"/>
                /// i.e. changing grid point (t_j,x_j) implies changes on the interval ]t_{j-leftLocalnessLevel}, t_{j+rightLocalnessLevel}[.</returns>
                public int GetLeftLocalnessLevel(int gridPointIndex, int gridPointCount)
                {
                    return m_CurveDataFittingFactory.GetLeftLocalnessLevel(gridPointIndex, gridPointCount);
                }

                /// <summary>Gets the right localness level for a specific grid point, i.e.
                /// changing grid point (t_j,x_j) implies changes on the interval ]t_{j-leftLocalnessLevel}, t_{j+rightLocalnessLevel}[.
                /// </summary>
                /// <param name="gridPointIndex">The null-based index of the grid point (t_j,x_j).</param>
                /// <param name="gridPointCount">The number of grid points.</param>
                /// <returns>The right localness level with respect to the grid point (t_j,x_j), where j is represented by <paramref name="gridPointIndex"/>
                /// i.e. changing grid point (t_j,x_j) implies changes on the interval ]t_{j-leftLocalnessLevel}, t_{j+rightLocalnessLevel}[.</returns>
                public int GetRightLocalnessLevel(int gridPointIndex, int gridPointCount)
                {
                    return m_CurveDataFittingFactory.GetRightLocalnessLevel(gridPointIndex, gridPointCount);
                }

                /// <summary>Creates a new <see cref="IGridPointCurve&lt;TLabel&gt;"/> objects.
                /// </summary>
                /// <returns>A <see cref="IGridPointCurve&lt;TLabel&gt;"/> instance.</returns>
                public IGridPointCurve<TLabel> Create()
                {
                    return m_FactoryDelegate();
                }

                /// <summary>Creates a new (read-only) <see cref="IGridPointCurve&lt;TLabel&gt;"/> object.
                /// </summary>
                /// <param name="gridPointCount">The number of grid points, i.e. the number of relevant elements of <paramref name="gridPointLabels"/>, <paramref name="gridPointArguments"/> and <paramref name="gridPointValues"/> to take into account.</param>
                /// <param name="gridPointLabels">The labels of the grid points.</param>
                /// <param name="gridPointArguments">The arguments of the grid points, thus labels of the curve in its <see cref="System.Double"/> representation.</param>
                /// <param name="gridPointValues">The values of the grid points corresponding to <paramref name="gridPointArguments"/>.</param>
                /// <param name="gridPointArgumentStartIndex">The null-based start index of <paramref name="gridPointArguments"/> and <paramref name="gridPointLabels"/> to take into account.</param>
                /// <param name="gridPointValueStartIndex">The null-based start index of <paramref name="gridPointValues"/> to take into account.</param>
                /// <param name="gridPointArgumentIncrement">The increment for <paramref name="gridPointArguments"/> and <paramref name="gridPointLabels"/>.</param>
                /// <param name="gridPointValueIncrement">The increment for <paramref name="gridPointValues"/>.</param>
                /// <returns>A read-only <see cref="IGridPointCurve&lt;TLabel&gt;"/> object with respect to the desired interpolation and extrapolation approaches.</returns>
                /// <remarks>This implementation takes into account references only; perhaps the underlying <see cref="ICurveDataFitting"/> implementation of the interpolation, parametrization etc. creates deep copies of <paramref name="gridPointArguments"/> and <paramref name="gridPointValues"/>.</remarks>
                public IGridPointCurve<TLabel> Create(int gridPointCount, IList<TLabel> gridPointLabels, IList<double> gridPointArguments, IList<double> gridPointValues, int gridPointArgumentStartIndex = 0, int gridPointValueStartIndex = 0, int gridPointArgumentIncrement = 1, int gridPointValueIncrement = 1)
                {
                    return m_ReadOnlyFactoryDelegate(gridPointCount, gridPointLabels, gridPointArguments, gridPointValues, gridPointArgumentStartIndex, gridPointValueStartIndex, gridPointArgumentIncrement, gridPointValueIncrement);
                }
                #endregion
            }
            #endregion

            #region public static methods

            /// <summary>Creates a specific grid point curve factory.
            /// </summary>
            /// <typeparam name="TLabel">The type of the label.</typeparam>
            /// <param name="curveInterpolator">The curve interpolator.</param>
            /// <param name="leftExtrapolator">The extrapolator on the left side, i.e. from the first grid point to -\infinity.</param>
            /// <param name="rightExtrapolator">The extrapolator on the right side, i.e. from the last grid point to \infinity.</param>
            /// <returns>A factory for <see cref="IGridPointCurve&lt;TLabel&gt;"/> objects with respect to the desired interpolation and extrapolation approaches.</returns>
            public static IGridPointCurveFactory<TLabel> Create<TLabel>(GridPointCurve.Interpolator curveInterpolator, GridPointCurve.Extrapolator leftExtrapolator, GridPointCurve.Extrapolator rightExtrapolator)
                where TLabel : IEquatable<TLabel>
            {
                return new CurveFactory<TLabel>(
                    () => GridPointCurve.Create<TLabel>(curveInterpolator, leftExtrapolator, rightExtrapolator),
                    curveInterpolator,
                    (gridPointCount, gridPointLabels, gridPointArguments, gridPointValues, gridPointArgumentStartIndex, gridPointValueStartIndex, gridPointArgumentIncrement, gridPointValueIncrement) => { return GridPointCurve.Create(curveInterpolator, leftExtrapolator, rightExtrapolator, gridPointCount, gridPointLabels, gridPointArguments, gridPointValues, gridPointArgumentStartIndex, gridPointValueStartIndex, gridPointArgumentIncrement, gridPointValueIncrement); });
            }

            /// <summary>Creates a specific grid point curve factory.
            /// </summary>
            /// <typeparam name="TLabel">The type of the label.</typeparam>
            /// <param name="curveParametrization">The curve parametrization.</param>
            /// <returns>A factory for <see cref="IGridPointCurve&lt;TLabel&gt;"/> objects with respect to the desired interpolation and extrapolation approaches.</returns>
            public static IGridPointCurveFactory<TLabel> Create<TLabel>(GridPointCurve.Parametrization curveParametrization)
                where TLabel : IEquatable<TLabel>
            {
                return new CurveFactory<TLabel>(
                    () => GridPointCurve.Create<TLabel>(curveParametrization),
                    curveParametrization,
                    (gridPointCount, gridPointLabels, gridPointArguments, gridPointValues, gridPointArgumentStartIndex, gridPointValueStartIndex, gridPointArgumentIncrement, gridPointValueIncrement) => { return GridPointCurve.Create(curveParametrization, gridPointCount, gridPointLabels, gridPointArguments, gridPointValues, gridPointArgumentStartIndex, gridPointValueStartIndex, gridPointArgumentIncrement, gridPointValueIncrement); });
            }
            #endregion
        }
    }
}