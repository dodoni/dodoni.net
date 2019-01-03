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
using System.Linq;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.Miscellaneous
{
    public partial class MultiDimRegion
    {
        /// <summary>Represents a multi-dimensional interval, i.e. an element x =(x_0,...,x_{n-1}) \in \R^n is inside the region if and only if x fullfills the following conditions:
        /// <para>
        ///    a_j &lt; x_j &lt; b_j for j = 0,...,n-1.
        /// </para>
        /// The lower/upper bounds can be 'not a number' or +/-infinity.
        /// </summary>
        public class Interval : IMultiDimRegion
        {
            #region private members

            /// <summary>The dimension.
            /// </summary>
            private int m_Dimension;

            /// <summary>The lower bounds.
            /// </summary>
            /// <remarks><see cref="System.Double.NaN"/> and <see cref="System.Double.NegativeInfinity"/> is allowed.</remarks>
            private double[] m_LowerBounds;

            /// <summary>The upper bounds.
            /// </summary>
            /// <remarks><see cref="System.Double.NaN"/> and <see cref="System.Double.PositiveInfinity"/> is allowed.</remarks>
            private double[] m_UpperBounds;

            /// <summary>Represents for each coordinate the type of the interval.
            /// </summary>
            /// <remarks>This member is used to avoid to test the state of each lower/upper bound each time.</remarks>
            private Miscellaneous.Interval.BoundaryType[] m_IntervalBoundType;
            #endregion

            #region public constructors

            /// <summary>Initializes a new instance of the <see cref="Interval"/> class.
            /// </summary>
            /// <param name="dimension">The dimension.</param>
            /// <param name="lowerBounds">The lower bounds, <see cref="System.Double.NaN"/> and <see cref="System.Double.NegativeInfinity"/> are allowed.</param>
            /// <param name="upperBounds">The upper bounds, <see cref="System.Double.NaN"/> and <see cref="System.Double.PositiveInfinity"/> are allowed.</param>
            /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="dimension"/> is less or equal than <c>2</c>.</exception>
            /// <exception cref="ArgumentNullException">Thrown, if <paramref name="lowerBounds"/> or <paramref name="upperBounds"/> is <c>null</c>.</exception>
            /// <exception cref="ArgumentException">Thrown, if the number of elements of <paramref name="lowerBounds"/> or <paramref name="upperBounds"/> is less 
            /// than <paramref name="dimension"/> or one element of <paramref name="lowerBounds"/> is positive infinity or one element of <paramref name="upperBounds"/> is minus infinity.</exception>
            public Interval(int dimension, double[] lowerBounds, double[] upperBounds)
            {
                if (dimension < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(dimension), String.Format(CultureInfo.InvariantCulture, ExceptionMessages.ArgumentOutOfRangeGreaterEqual, "Dimension", 1));
                }
                m_Dimension = dimension;

                if (lowerBounds == null)
                {
                    throw new ArgumentNullException(nameof(lowerBounds), String.Format(ExceptionMessages.ArgumentNull, "Lower bounds"));
                }
                else if (dimension > lowerBounds.Length)
                {
                    throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, ExceptionMessages.ArgumentHasWrongDimension, "Lower bounds"), nameof(lowerBounds));
                }

                if (upperBounds == null)
                {
                    throw new ArgumentNullException(nameof(upperBounds), String.Format(ExceptionMessages.ArgumentNull, "Upper bounds"));
                }
                else if (dimension > upperBounds.Length)
                {
                    throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, ExceptionMessages.ArgumentHasWrongDimension, "Upper bounds"), nameof(upperBounds));
                }

                m_LowerBounds = new double[dimension];
                m_UpperBounds = new double[dimension];
                m_IntervalBoundType = new Miscellaneous.Interval.BoundaryType[dimension];

                LowerBounds = new ReadOnlyCollection<double>(m_LowerBounds);
                UpperBounds = new ReadOnlyCollection<double>(m_UpperBounds);
                IntervalBoundTypes = new ReadOnlyCollection<Miscellaneous.Interval.BoundaryType>(m_IntervalBoundType);

                for (int j = 0; j < dimension; j++)
                {
                    double lowerBound = lowerBounds[j];
                    bool lowerBounded = false;
                    if (Double.IsPositiveInfinity(lowerBound))
                    {
                        throw new ArgumentException(nameof(lowerBounds), String.Format(CultureInfo.InvariantCulture, String.Format(ExceptionMessages.ArgumentIsPositiveInfinity, "lowerBounds[" + j + "]")));
                    }
                    else if ((Double.IsNegativeInfinity(lowerBound) == false) && (Double.IsNaN(lowerBound) == false))
                    {
                        lowerBounded = true;
                        m_LowerBounds[j] = lowerBound;
                    }
                    else
                    {
                        m_LowerBounds[j] = Double.NegativeInfinity;
                    }

                    double upperBound = upperBounds[j];
                    if (Double.IsNegativeInfinity(upperBound))
                    {
                        throw new ArgumentException(nameof(upperBounds), string.Format(CultureInfo.InvariantCulture, string.Format(ExceptionMessages.ArgumentIsMinusInfinity, "upperBounds[" + j + "]")));
                    }
                    else if ((Double.IsPositiveInfinity(upperBound) == false) && (Double.IsNaN(upperBound) == false))
                    {
                        m_UpperBounds[j] = upperBound;
                        if (lowerBounded)
                        {
                            m_IntervalBoundType[j] = Miscellaneous.Interval.BoundaryType.Bounded;
                        }
                        else
                        {
                            m_IntervalBoundType[j] = Miscellaneous.Interval.BoundaryType.UpperBounded;
                        }
                    }
                    else
                    {
                        m_UpperBounds[j] = Double.PositiveInfinity;
                        if (lowerBounded)
                        {
                            m_IntervalBoundType[j] = Miscellaneous.Interval.BoundaryType.LowerBounded;
                        }
                        else
                        {
                            m_IntervalBoundType[j] = Miscellaneous.Interval.BoundaryType.Unbounded;
                        }
                    }
                }
            }

            /// <summary>Initializes a new instance of the <see cref="Interval"/> class.
            /// </summary>
            /// <param name="dimension">The dimension.</param>
            /// <param name="lowerBound">The lower bound for each coordinate, <see cref="System.Double.NaN"/> and <see cref="System.Double.NegativeInfinity"/> are allowed.</param>
            /// <param name="upperBound">The upper bound for each coordinate, <see cref="System.Double.NaN"/> and <see cref="System.Double.PositiveInfinity"/> are allowed.</param>
            /// <returns>The specified <see cref="Interval"/> object.</returns>
            /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="dimension"/> is less or equal than <c>2</c>.</exception>
            /// <exception cref="ArgumentException">Thrown, if <paramref name="lowerBound"/> is positive infinity or <paramref name="upperBound"/> is minus infinity.</exception>
            public Interval(int dimension, double lowerBound, double upperBound)
            {
                if (dimension < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(dimension), String.Format(CultureInfo.InvariantCulture, ExceptionMessages.ArgumentOutOfRangeGreaterEqual, "Dimension", 1));
                }
                m_Dimension = dimension;

                Miscellaneous.Interval.BoundaryType boundaryType;
                bool isLowerBounded = false;

                if (Double.IsPositiveInfinity(lowerBound))
                {
                    throw new ArgumentException(nameof(lowerBound), String.Format(CultureInfo.InvariantCulture, String.Format(ExceptionMessages.ArgumentIsPositiveInfinity, "lowerBound")));
                }
                else if ((Double.IsNegativeInfinity(lowerBound) == false) && (Double.IsNaN(lowerBound) == false))
                {
                    isLowerBounded = true;
                }
                else
                {
                    lowerBound = Double.NegativeInfinity;
                }

                if (Double.IsNegativeInfinity(upperBound))
                {
                    throw new ArgumentException(nameof(upperBound), string.Format(CultureInfo.InvariantCulture, string.Format(ExceptionMessages.ArgumentIsMinusInfinity, "upperBound")));
                }
                else if ((Double.IsPositiveInfinity(upperBound) == false) && (Double.IsNaN(upperBound) == false))
                {
                    if (isLowerBounded)
                    {
                        boundaryType = Miscellaneous.Interval.BoundaryType.Bounded;
                    }
                    else
                    {
                        boundaryType = Miscellaneous.Interval.BoundaryType.UpperBounded;
                    }
                }
                else
                {
                    upperBound = Double.PositiveInfinity;
                    if (isLowerBounded)
                    {
                        boundaryType = Miscellaneous.Interval.BoundaryType.LowerBounded;
                    }
                    else
                    {
                        boundaryType = Miscellaneous.Interval.BoundaryType.Unbounded;
                    }
                }
                m_LowerBounds = Enumerable.Repeat(lowerBound, dimension).ToArray();
                m_UpperBounds = Enumerable.Repeat(upperBound, dimension).ToArray();
                m_IntervalBoundType = Enumerable.Repeat(boundaryType, dimension).ToArray();

                LowerBounds = new ReadOnlyCollection<double>(m_LowerBounds);
                UpperBounds = new ReadOnlyCollection<double>(m_UpperBounds);
                IntervalBoundTypes = new ReadOnlyCollection<Miscellaneous.Interval.BoundaryType>(m_IntervalBoundType);
            }
            #endregion

            #region public properties

            #region IMultiDimRegion Members

            /// <summary>Gets the dimension of the region.
            /// </summary>
            /// <value>The dimension.</value>
            public int Dimension
            {
                get { return m_Dimension; }
            }
            #endregion

            /// <summary>Gets the lower bounds; <see cref="System.Double.NaN"/> and <see cref="System.Double.NegativeInfinity"/> are allowed.
            /// </summary>
            /// <value>The lower bounds; <see cref="System.Double.NaN"/> and <see cref="System.Double.NegativeInfinity"/> are allowed.</value>
            public ReadOnlyCollection<double> LowerBounds
            {
                get;
                private set;
            }

            /// <summary>Gets the upper bounds; <see cref="System.Double.NaN"/> and <see cref="System.Double.PositiveInfinity"/> are allowed.
            /// </summary>
            ///<value>The upper bounds; <see cref="System.Double.NaN"/> and <see cref="System.Double.PositiveInfinity"/> are allowed.</value>
            public ReadOnlyCollection<double> UpperBounds
            {
                get;
                private set;
            }

            /// <summary>Gets for each coordinate the type of the corresponding interval in its <see cref="Miscellaneous.Interval.BoundaryType"/> representation.
            /// </summary>
            /// <value>For each coordinate the type of the corresponding interval in its <see cref="Miscellaneous.Interval.BoundaryType"/> representation.</value>
            public ReadOnlyCollection<Miscellaneous.Interval.BoundaryType> IntervalBoundTypes
            {
                get;
                private set;
            }

            /// <summary>Gets a value indicating whether the multi-dimensional interval is finite, i.e. it has a finite diameter.
            /// </summary>
            /// <value><c>true</c> if this instance represents a finite multi-dimensional interval; otherwise, <c>false</c>.</value>
            public bool IsFinite
            {
                get
                {
                    for (int j = 0; j < m_Dimension; j++)
                    {
                        if (m_IntervalBoundType[j] != Miscellaneous.Interval.BoundaryType.Bounded)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }

            /// <summary>Gets a value indicating whether this instance is unconstraint region, i.e. \R^n.
            /// </summary>
            /// <value><c>true</c> if this instance represents the unconstraint region \R^n; otherwise, <c>false</c>.</value>
            public bool IsUnconstraintRegion
            {
                get
                {
                    for (int j = 0; j < m_Dimension; j++)
                    {
                        if (m_IntervalBoundType[j] != Miscellaneous.Interval.BoundaryType.Unbounded)
                        {
                            return false;
                        }
                    }
                    return true;
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
            public PointRegionRelation GetPointPosition(double[] x, double tolerance = MachineConsts.Epsilon)
            {
                for (int j = 0; j < m_Dimension; j++)
                {
                    if ((m_LowerBounds[j] > x[j]) || (m_UpperBounds[j] < x[j]))  // works for 'NaN'
                    {
                        return PointRegionRelation.Outside;
                    }
                }
                return PointRegionRelation.InsideOrBoundaryPoint;
            }

            /// <summary>Gets a value indicating whether a specific point is inside the region.
            /// </summary>
            /// <param name="x">The argument, i.e. a point of dimension <see cref="IMultiDimRegion.Dimension" />.</param>
            /// <param name="distance">The distance to the region represented by the current instance (output).</param>
            /// <param name="tolerance">This parameter will be ignored in this implementation.</param>
            /// <returns>A value indicating whether <paramref name="x" /> is inside the region and has some strictly positive distance to the region represented by the current instance.</returns>
            public PointRegionRelation GetDistance(double[] x, out double distance, double tolerance = MachineConsts.Epsilon)
            {
                distance = 0.0;
                PointRegionRelation pointRelation = PointRegionRelation.InsideOrBoundaryPoint;

                for (int j = 0; j < m_Dimension; j++)
                {
                    double pointComponent = x[j];
                    if (pointComponent < m_LowerBounds[j])
                    {
                        distance += m_LowerBounds[j] - pointComponent;
                        pointRelation = PointRegionRelation.Outside;
                    }
                    if (m_UpperBounds[j] < pointComponent)
                    {
                        distance += pointComponent - m_UpperBounds[j];
                        pointRelation = PointRegionRelation.Outside;
                    }
                }
                return pointRelation;
            }
            #endregion

            /// <summary>Gets the box constraints represented as a linear constraint, i.e.
            /// <para>C^t * x >= d,</para>
            /// i.e. an element x \in \R^n is inside the region if and only if x fullfills the above condition.
            /// </summary>
            /// <param name="inequalityMatrix">The inequality matrix 'C' provided column-by-column, where
            /// <list type="bullet">
            /// <item><description>the number of rows will be equal to <see cref="Interval.Dimension"/> and</description></item>
            /// <item><description>the number of columns corresponds to the number of constraints</description></item>
            /// </list>Additional columns will be added.</param>
            /// <param name="inequalityVector">The inequality vector 'd'. On exit the number of elements is at least equal to the number of columns of <paramref name="inequalityMatrix"/>, i.e. will be filled with respect to the constraints represented by the current instance (output).</param>
            /// <returns>The nummber of contraints added to the arguments.</returns>
            public int GetRegionConstraints(List<double> inequalityMatrix, IList<double> inequalityVector)
            {
                /* If all constraints are needed, i.e. no boundary is -\infinity or \infinity, the matrix is specified by
                 *     |  1 -1  0  0  0  0
                 *     |  0  0  1 -1  0  0
                 *  A= |  0  0  0  0  1 -1
                 *     |   ....
                 *  and the vector is (a_0,-b_0,a_1,-b_1,...); otherwise some columns are removed. Thus it holds (A^t *x)_j = \sum_{k=1}^n a_{kj}*x_k = +/- x_j
                 *  for j=1,...,p, where p <=2*n is the number of constraints.
                 */

                int constraintCount = 0;

                for (int j = 0; j < m_Dimension; j++)
                {
                    var boundaryType = m_IntervalBoundType[j];

                    if (boundaryType.HasFlag(Miscellaneous.Interval.BoundaryType.LowerBounded) == true)
                    {
                        constraintCount++;
                        inequalityVector.Add(m_LowerBounds[j]);

                        /* add d (=dimension) elements where each element is 0.0, except the value with index 'j' */
                        inequalityMatrix.AddRange(Enumerable.Repeat(0.0, m_Dimension).Select((x, index) => (index == j) ? 1.0 : x));
                    }

                    if (boundaryType.HasFlag(Miscellaneous.Interval.BoundaryType.UpperBounded) == true)
                    {
                        constraintCount++;
                        inequalityVector.Add(-m_UpperBounds[j]);

                        /* add d (=dimension) elements where each element is 0.0, except the value with index 'j' */
                        inequalityMatrix.AddRange(Enumerable.Repeat(0.0, m_Dimension).Select((x, index) => (index == j) ? -1.0 : x));
                    }
                }
                return constraintCount;
            }

            /// <summary>Gets the lower and upper bounds for a specific coordinate.
            /// </summary>
            /// <param name="index">The null-based index of the coordinate.</param>
            /// <param name="lowerBound">The lower bound, perhaps <see cref="Double.NegativeInfinity"/> (output).</param>
            /// <param name="upperBound">The upper bound, perhaps <see cref="Double.PositiveInfinity"/> (output).</param>
            /// <param name="intervalBoundType">The type of the (one-dimensional) interval (output).</param>
            /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="index"/> is not valid.</exception>
            public void GetBounds(int index, out double lowerBound, out double upperBound, out Miscellaneous.Interval.BoundaryType intervalBoundType)
            {
                if ((index >= 0) && (index < m_Dimension))
                {
                    lowerBound = m_LowerBounds[index];
                    upperBound = m_UpperBounds[index];
                }
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            /// <summary>Returns a <see cref="System.String"/> that represents this instance.
            /// </summary>
            /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
            public override string ToString()
            {
                StringBuilder strBuilder = new StringBuilder();
                for (int j = 0; j < m_Dimension; j++)
                {
                    if (j > 0)
                    {
                        strBuilder.Append(" x ");
                    }
                    switch (m_IntervalBoundType[j])
                    {
                        case Miscellaneous.Interval.BoundaryType.Bounded:
                            strBuilder.AppendFormat("[{0}; {1}]", m_LowerBounds[j], m_UpperBounds[j]);
                            break;

                        case Miscellaneous.Interval.BoundaryType.LowerBounded:
                            strBuilder.AppendFormat("[ {0}; {1}[", m_LowerBounds[j], @"\infty");
                            break;

                        case Miscellaneous.Interval.BoundaryType.UpperBounded:
                            strBuilder.AppendFormat("]{0}; {1}]", @"-\infty", m_UpperBounds[j]);
                            break;

                        case Miscellaneous.Interval.BoundaryType.Unbounded:
                            strBuilder.Append(@"]-\infty; \infty[");
                            break;

                        default:
                            throw new NotImplementedException();
                    }
                }
                return strBuilder.ToString();
            }
            #endregion

            #region public static methods

            /// <summary>Creates a specified <see cref="Interval"/> object.
            /// </summary>
            /// <param name="dimension">The dimension.</param>
            /// <param name="lowerBounds">The lower bounds, <see cref="System.Double.NaN"/> and <see cref="System.Double.NegativeInfinity"/> are allowed.</param>
            /// <param name="upperBounds">The upper bounds, <see cref="System.Double.NaN"/> and <see cref="System.Double.PositiveInfinity"/> are allowed.</param>
            /// <returns>The specified <see cref="Interval"/> object.</returns>
            /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="dimension"/> is less or equal than <c>2</c>.</exception>
            /// <exception cref="ArgumentNullException">Thrown, if <paramref name="lowerBounds"/> or <paramref name="upperBounds"/> is <c>null</c>.</exception>
            /// <exception cref="ArgumentException">Thrown, if the number of elements of <paramref name="lowerBounds"/> or <paramref name="upperBounds"/> is less 
            /// than <paramref name="dimension"/> or one element of <paramref name="lowerBounds"/> is positive infinity or one element of <paramref name="upperBounds"/> is minus infinity.</exception>
            public static Interval Create(int dimension, double[] lowerBounds, double[] upperBounds)
            {
                return new Interval(dimension, lowerBounds, upperBounds);
            }

            /// <summary>Creates a specified <see cref="Interval"/> object.
            /// </summary>
            /// <param name="dimension">The dimension.</param>
            /// <param name="lowerBound">The lower bound for each coordinate, <see cref="System.Double.NaN"/> and <see cref="System.Double.NegativeInfinity"/> are allowed.</param>
            /// <param name="upperBound">The upper bound for each coordinate, <see cref="System.Double.NaN"/> and <see cref="System.Double.PositiveInfinity"/> are allowed.</param>
            /// <returns>The specified <see cref="Interval"/> object.</returns>
            /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="dimension"/> is less or equal than <c>2</c>.</exception>
            /// <exception cref="ArgumentException">Thrown, if <paramref name="lowerBound"/> is positive infinity or <paramref name="upperBound"/> is minus infinity.</exception>
            public static Interval Create(int dimension, double lowerBound, double upperBound)
            {
                return new Interval(dimension, lowerBound, upperBound);
            }
            #endregion
        }
    }
}