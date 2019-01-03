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
using System.Globalization;
using System.Collections.Generic;

using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.Miscellaneous
{
    /// <summary>Represents a n-dimensional boundary, where the boundary is represented by a polynomial (in n variables), a point x is feasible if it fullfill the following condition
    /// <para>
    ///     a \leq \sum_{j=0}^m c_j * x_{j,1}^{k_{j,1}} * ... * x_{j,n_j}^{k_{j,n_j}} \leq b,
    /// </para>
    /// where a and b are lower bound, upper bound respectively. 
    /// </summary>
    internal class GeneralPolynomialRegion : MultiDimRegion.Polynomial
    {
        #region private members

        /// <summary>The collection of coefficients (c_j) in the polynomial representation.
        /// </summary>
        private double[] m_Coefficients;

        /// <summary>Represents the monomials of the polynomial representation, i.e. powers and the corresponding argument index. The first null-based index corresponds to 
        /// the index of the constraint ('j'), the second null-based index corresponds to the monomial in the product.
        /// </summary>
        private Monomial[][] m_Monomials;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="GeneralPolynomialRegion" /> class, where the constraint is represented by
        /// <para>a \leq \sum_{j=0}^m c_j * x_{j,1}^{k_{j,1}} * ... * x_{j,n_j}^{k_{j,n_j}} \leq b.</para>
        /// </summary>
        /// <param name="dimension">The dimension of the feasible region.</param>
        /// <param name="lowerBound">The lower bound, perhaps <see cref="System.Double.NegativeInfinity"/>.</param>
        /// <param name="upperBound">The upper bound, perhaps <see cref="System.Double.PositiveInfinity"/>.</param>
        /// <param name="coefficients">The coefficients (c_j).</param>
        /// <param name="monomials">A collection of monomials, i.e. powers and corresponding argument indices, where the first corresponds to the index of the sum ('j') of the polynomial, i.e. with respect to <paramref name="coefficients"/>.
        /// The second null-based index corresponds to the monomial in the product of the polynomial representation of the constraint.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="dimension"/> is less than <c>1</c> or if some monomial is specified with respect to some higher dimension than <paramref name="dimension"/>.</exception>
        internal GeneralPolynomialRegion(int dimension, double lowerBound, double upperBound, double[] coefficients, params IList<Monomial>[] monomials)
            : base(dimension, lowerBound, upperBound)
        {
            m_Coefficients = coefficients ?? throw new ArgumentNullException(nameof(coefficients));
            if ((monomials == null) || (monomials.Length == 0))
            {
                throw new ArgumentException(nameof(monomials));
            }
            m_Monomials = new Monomial[monomials.Length][];
            for (int k = 0; k < monomials.Length; k++)
            {
                m_Monomials[k] = new Monomial[monomials[k].Count];

                int s = 0;
                foreach (var monomial in monomials[k])
                {
                    if (monomial.ArgumentIndex >= dimension)
                    {
                        throw new ArgumentOutOfRangeException(nameof(monomials), String.Format(CultureInfo.InvariantCulture, ExceptionMessages.ArgumentCombinationInvalid, "monomial and dimension"));
                    }
                    m_Monomials[k][s] = monomial;
                    s++;
                }
            }
        }
        #endregion

        #region public properties

        /// <summary>Gets the collection of terms of the polynomial that represents the constraint, where the polynomial is the sum of the terms. The first component represents
        /// the coefficient and the second component represents the monomial to multiply with these coefficient.
        /// </summary>
        /// <value>The collection of terms of the polynomial that represents the constraint, where the polynomial is the sum of the terms.</value>
        public override IEnumerable<Tuple<double, IEnumerable<Monomial>>> Terms
        {
            get
            {
                for (int j = 0; j < m_Coefficients.Length; j++)
                {
                    yield return Tuple.Create(m_Coefficients[j], (IEnumerable<Monomial>)m_Monomials[j]);
                }
            }
        }
        #endregion

        #region public methods

        #region IMultiDimRegion Members

        /// <summary>Gets a value indicating whether a specific point is inside the region.
        /// </summary>
        /// <param name="x">The argument, i.e. a point of dimension <see cref="IMultiDimRegion.Dimension" />.</param>
        /// <param name="tolerance">This parameter will be ignored in this implementation.</param>
        /// <returns>A value indicating whether <paramref name="x" /> is inside the represented region.</returns>
        public override PointRegionRelation GetPointPosition(double[] x, double tolerance = MachineConsts.Epsilon)
        {
            double value = 0.0;

            for (int j = 0; j < m_Monomials.Length; j++)
            {
                double temp = m_Coefficients[j];
                foreach (var monomial in m_Monomials[j])
                {
                    temp *= DoMath.Pow(x[monomial.ArgumentIndex], monomial.Power);
                }
                value += temp;
            }
            if ((value < LowerBound) || (value > UpperBound))
            {
                return PointRegionRelation.Outside;
            }
            return PointRegionRelation.InsideOrBoundaryPoint;
        }

        /// <summary>Gets a value indicating whether a specific point is inside the region.
        /// </summary>
        /// <param name="x">The argument, i.e. a point of dimension <see cref="IMultiDimRegion.Dimension" />.</param>
        /// <param name="distance">The distance to the region represented by the current instance (output).</param>
        /// <param name="tolerance">This parameter will be ignored in this implementation.</param>
        /// <returns>A value indicating whether <paramref name="x" /> is inside the region and has some strictly positive distance to the region represented by the current instance.</returns>
        public override PointRegionRelation GetDistance(double[] x, out double distance, double tolerance = MachineConsts.Epsilon)
        {
            double value = 0.0;

            for (int j = 0; j < m_Monomials.Length; j++)
            {
                double temp = m_Coefficients[j];
                foreach (var monomial in m_Monomials[j])
                {
                    temp *= DoMath.Pow(x[monomial.ArgumentIndex], monomial.Power);
                }
                value += temp;
            }
            if (value < LowerBound)
            {
                distance = LowerBound - value;
                return PointRegionRelation.Outside;
            }
            if (value > UpperBound)
            {
                distance = value - UpperBound;
                return PointRegionRelation.Outside;
            }
            distance = 0.0;
            return PointRegionRelation.InsideOrBoundaryPoint;
        }
        #endregion

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            if (Double.IsNegativeInfinity(LowerBound) == false)
            {
                str.AppendFormat("{0} <=", LowerBound);
            }

            for (int j = 0; j < m_Monomials.Length; j++)
            {
                var coefficient = m_Coefficients[j];

                if ((coefficient == 1.0) || (coefficient == -1.0))
                {
                    string sign = (coefficient >= 0.0) ? "+" : "-";

                    if (j == 0)
                    {
                        str.AppendFormat(" x_{0}^{1}", m_Monomials[j][0].ArgumentIndex, m_Monomials[j][0].Power);
                    }
                    else
                    {
                        str.AppendFormat(" {0} x_{1}^{2}", sign, m_Monomials[j][0].ArgumentIndex, m_Monomials[j][0].Power);
                    }

                    for (int k = 1; k < m_Monomials[j].Length; k++)
                    {
                        var monomial = m_Monomials[j][k];
                        str.AppendFormat(" * x_{0}^{1}", monomial.ArgumentIndex, monomial.Power);
                    }
                }
                else if (coefficient != 0.0)
                {
                    str.AppendFormat(" {0}", coefficient.ToString("+#.######;-#.######;#.#"));  // always show the sign of the coefficient
                    foreach (var monomial in m_Monomials[j])
                    {
                        str.AppendFormat(" * x_{0}^{1}", monomial.ArgumentIndex, monomial.Power);
                    }
                }
            }
            if (Double.IsPositiveInfinity(UpperBound) == false)
            {
                str.AppendFormat("<= {0}", UpperBound);
            }
            return str.ToString();
        }
        #endregion
    }
}