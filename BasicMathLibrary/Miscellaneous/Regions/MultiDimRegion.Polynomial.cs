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
using System.Globalization;
using System.Collections.Generic;

using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.Miscellaneous
{
    public partial class MultiDimRegion
    {
        /// <summary>Serves as factory for n-dimensional constraint, where the boundary is represented by a specific polynomial (in n variables).
        /// </summary>
        public abstract partial class Polynomial : IMultiDimRegion
        {
            #region protected constructors

            /// <summary>Initializes a new instance of the <see cref="MultiDimRegion.Polynomial" /> class.
            /// </summary>
            /// <param name="dimension">The dimension of the feasible region.</param>
            /// <param name="lowerBound">The lower bound, perhaps <see cref="System.Double.NegativeInfinity"/>.</param>
            /// <param name="upperBound">The upper bound, perhaps <see cref="System.Double.PositiveInfinity"/>.</param>
            protected Polynomial(int dimension, double lowerBound, double upperBound)
            {
                if (dimension < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(dimension), String.Format(CultureInfo.InvariantCulture, ExceptionMessages.ArgumentOutOfRangeGreaterEqual, "Dimension", 1));
                }
                Dimension = dimension;
                if ((Double.IsNaN(lowerBound) == true) || (Double.IsNegativeInfinity(lowerBound) == true))
                {
                    LowerBound = Double.NegativeInfinity;
                }
                else
                {
                    LowerBound = lowerBound;
                }
                if ((Double.IsNaN(upperBound) == true) || (Double.IsPositiveInfinity(upperBound) == true))
                {
                    UpperBound = Double.PositiveInfinity;
                }
                else
                {
                    UpperBound = upperBound;
                }
                if (UpperBound < LowerBound)
                {
                    throw new ArgumentOutOfRangeException("upper-/lower bound", String.Format(CultureInfo.InvariantCulture, ExceptionMessages.ArgumentCombinationInvalid, "Lower/upper bound"));
                }
            }
            #endregion

            #region public properties

            #region IMultiDimRegion Members

            /// <summary>Gets the dimension of the region.
            /// </summary>
            /// <value>The dimension.</value>
            public int Dimension
            {
                get;
                private set;
            }
            #endregion

            /// <summary>Gets the lower bound, perhaps <see cref="System.Double.NegativeInfinity"/>.
            /// </summary>
            /// <value>The lower bound.</value>
            public double LowerBound
            {
                get;
                private set;
            }

            /// <summary>Gets the upper bound, perhaps <see cref="System.Double.PositiveInfinity"/>.
            /// </summary>
            /// <value>The upper bound.</value>
            public double UpperBound
            {
                get;
                private set;
            }

            /// <summary>Gets the collection of terms of the polynomial that represents the constraint, where the polynomial is the sum of the terms. The first component represents
            /// the coefficient and the second component represents the monomial to multiply with these coefficient.
            /// </summary>
            /// <value>The collection of terms of the polynomial that represents the constraint, where the polynomial is the sum of the terms.</value>
            public abstract IEnumerable<Tuple<double, IEnumerable<Monomial>>> Terms
            {
                get;
            }
            #endregion

            #region public methods

            #region IMultiDimRegion Members

            /// <summary>Gets a value indicating whether a specific point is inside the region.
            /// </summary>
            /// <param name="x">The argument, i.e. a point of dimension <see cref="IMultiDimRegion.Dimension" />.</param>
            /// <param name="tolerance">This parameter will be ignored in this implementation.</param>
            /// <returns>A value indicating whether <paramref name="x" /> is inside the represented region.</returns>
            public abstract PointRegionRelation GetPointPosition(double[] x, double tolerance = MachineConsts.Epsilon);

            /// <summary>Gets a value indicating whether a specific point is inside the region.
            /// </summary>
            /// <param name="x">The argument, i.e. a point of dimension <see cref="IMultiDimRegion.Dimension" />.</param>
            /// <param name="distance">The distance to the region represented by the current instance (output).</param>
            /// <param name="tolerance">This parameter will be ignored in this implementation.</param>
            /// <returns>A value indicating whether <paramref name="x" /> is inside the region and has some strictly positive distance to the region represented by the current instance.</returns>
            public abstract PointRegionRelation GetDistance(double[] x, out double distance, double tolerance = MachineConsts.Epsilon);
            #endregion

            #endregion

            #region public static methods

            /// <summary>Create a new <see cref="MultiDimRegion.Polynomial" /> instance, where the constraint is represented by 
            /// <para>a \leq c * x_{j_1}^{k_1} * ... * x_{j_m}^{k_m} \leq b,</para> where c is a specific coefficient.
            /// </summary>
            /// <param name="dimension">The dimension of the feasible region.</param>
            /// <param name="lowerBound">The lower bound, perhaps <see cref="System.Double.NegativeInfinity"/>.</param>
            /// <param name="upperBound">The upper bound, perhaps <see cref="System.Double.PositiveInfinity"/>.</param>
            /// <param name="coefficient">The (single) coefficient.</param>
            /// <param name="monomials">A collection of monomials, i.e. powers and corresponding argument indices.</param>
            /// <returns>The specified <see cref="MultiDimRegion.Polynomial"/> object.</returns>
            /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="dimension"/> is less than <c>1</c> or if some monomial is specified with respect to some higher dimension than <paramref name="dimension"/>.</exception>
            public static MultiDimRegion.Polynomial Create(int dimension, double lowerBound, double upperBound, double coefficient, params Monomial[] monomials)
            {
                return new OneCoefficientPolynomialRegion(dimension, lowerBound, upperBound, coefficient, monomials);
            }

            /// <summary>Create a new <see cref="MultiDimRegion.Polynomial" /> instance, where the constraint is represented by
            /// <para>a \leq \sum_{j=0}^m c_j * x_{j,1}^{k_{j,1}} * ... * x_{j,n_j}^{k_{j,n_j}} \leq b.</para>
            /// </summary>
            /// <param name="dimension">The dimension of the feasible region.</param>
            /// <param name="lowerBound">The lower bound, perhaps <see cref="System.Double.NegativeInfinity"/>.</param>
            /// <param name="upperBound">The upper bound, perhaps <see cref="System.Double.PositiveInfinity"/>.</param>
            /// <param name="coefficients">The coefficients.</param>
            /// <param name="monomials">A collection of monomials, i.e. powers and corresponding argument indices, where the first corresponds to the index of the sum of the polynomial, i.e. with respect to <paramref name="coefficients"/>.
            /// The second null-based index corresponds to the monomial in the product of the polynomial representation of the constraint.</param>
            /// <returns>The specified <see cref="MultiDimRegion.Polynomial"/> object.</returns>
            /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="dimension"/> is less than <c>1</c> or if some monomial is specified with respect to some higher dimension than <paramref name="dimension"/>.</exception>
            public static MultiDimRegion.Polynomial Create(int dimension, double lowerBound, double upperBound, double[] coefficients, params IList<Monomial>[] monomials)
            {
                return new GeneralPolynomialRegion(dimension, lowerBound, upperBound, coefficients, monomials);
            }

            /// <summary>Create a new <see cref="MultiDimRegion.Polynomial"/> instance  where the constraint is represented by
            /// <para>a \leq c_1 * x_{j_1}^{k_1} + c_2 * x_{j_2}^{k_2} + ... + c_m * x_{j_m}^{k_m} \leq b.</para>
            /// </summary>
            /// <param name="dimension">The dimension of the feasible region.</param>
            /// <param name="lowerBound">The lower bound, perhaps <see cref="System.Double.NegativeInfinity"/>.</param>
            /// <param name="upperBound">The upper bound, perhaps <see cref="System.Double.PositiveInfinity"/>.</param>
            /// <param name="coefficients">The coefficients (c_j).</param>
            /// <param name="monomials">A collection of monomials, i.e. powers and corresponding argument indices, for each coefficient c_j.</param>
            /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="dimension"/> is less than <c>1</c> or if some monomial is specified with respect to some higher dimension than <paramref name="dimension"/>.</exception>
            public static MultiDimRegion.Polynomial Create(int dimension, double lowerBound, double upperBound, double[] coefficients, params Monomial[] monomials)
            {
                return new MonomialSumPolynomialRegion(dimension, lowerBound, upperBound, coefficients, monomials);
            }
            #endregion
        }
    }
}