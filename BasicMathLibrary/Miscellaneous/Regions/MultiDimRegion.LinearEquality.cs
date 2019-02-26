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
using Dodoni.MathLibrary.Basics;

namespace Dodoni.MathLibrary.Miscellaneous
{
    public partial class MultiDimRegion
    {
        /// <summary>Represents a multi-dimensional region which is specified with respect to the following condition
        /// <para>
        ///    C^t * x = d,
        /// </para>
        /// i.e. an element x \in \R^n is inside the region if and only if x fullfills the above condition.
        /// </summary>
        public class LinearEquality : IMultiDimRegion
        {
            #region private members

            /// <summary>The matrix C for the condition C^t * x = d.
            /// </summary>
            private DenseMatrix m_EqualityMatrix;

            /// <summary>The vector d [with at least C.ColumnCount elements] for the condition C^t * x = d.
            /// </summary>
            private double[] m_EqualityVector;
            #endregion

            #region public constructors

            /// <summary>Initializes a new instance of the <see cref="LinearEquality" /> class.
            /// </summary>
            /// <param name="equalityMatrix">The equality matrix C, i.e. a n x m matrix, where n is the dimension of the feasible region.</param>
            /// <param name="equalityVector">The equality vector d, i.e. a m-dimensional vector.</param>
            /// <exception cref="ArgumentNullException">Thrown if one of the arguments is <c>null</c>.</exception>
            /// <exception cref="ArgumentException">Thrown if the length of <paramref name="equalityVector"/> does not agree with the number of columns of <paramref name="equalityMatrix"/>.</exception>
            /// <remarks>A shallow copy of the arguments will be stored only.</remarks>
            public LinearEquality(DenseMatrix equalityMatrix, double[] equalityVector)
            {
                if (equalityMatrix == null)
                {
                    throw new ArgumentNullException(nameof(equalityMatrix), String.Format(CultureInfo.InvariantCulture, ExceptionMessages.ArgumentNull, "Equality matrix"));
                }
                m_EqualityMatrix = equalityMatrix;

                if (equalityVector == null)
                {
                    throw new ArgumentNullException(nameof(equalityVector), String.Format(CultureInfo.InvariantCulture, ExceptionMessages.ArgumentNull, "Equality vector"));
                }
                m_EqualityVector = equalityVector;

                int equalityConstraintCount = equalityMatrix.ColumnCount;
                if (equalityConstraintCount > equalityVector.Length)
                {
                    throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, ExceptionMessages.ArgumentHasWrongDimension, "Equality matrix/vector"), nameof(equalityVector));
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
                get { return m_EqualityMatrix.RowCount; }
            }
            #endregion

            /// <summary>Gets the number of equalities.
            /// </summary>
            /// <value>The number of equalities.</value>
            public int EqualityCount
            {
                get { return m_EqualityMatrix.ColumnCount; }
            }
            #endregion

            #region public methods

            #region IMultiDimRegion Members

            /// <summary>Gets a value indicating whether a specific point is inside the region.
            /// </summary>
            /// <param name="x">The argument, i.e. a point of dimension <see cref="IMultiDimRegion.Dimension" />.</param>
            /// <param name="tolerance">Some tolerance to take into account.</param>
            /// <returns>A value indicating whether <paramref name="x" /> is inside the represented region.</returns>
            public PointRegionRelation GetPointPosition(double[] x, double tolerance = MachineConsts.Epsilon)
            {
                /* Because of some early exit condition, we do the matrix operation 'by hand': */
                for (int i = 0; i < m_EqualityMatrix.ColumnCount; i++)
                {
                    double product = 0.0;
                    for (int k = 0; k < Dimension; k++)
                    {
                        product += m_EqualityMatrix[k, i] * x[k];
                    }
                    if (Math.Abs(product - m_EqualityVector[i]) > tolerance)  // 'if (product != m_EqualityVector[j])' + rounding errors
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
            /// <param name="tolerance">Some tolerance to take into account.</param>
            /// <returns>A value indicating whether <paramref name="x" /> is inside the region and has some strictly positive distance to the region represented by the current instance.</returns>
            public PointRegionRelation GetDistance(double[] x, out double distance, double tolerance = MachineConsts.Epsilon)
            {
                distance = 0.0;
                PointRegionRelation state = PointRegionRelation.InsideOrBoundaryPoint;

                /* We do the matrix operation 'by hand'. This needs less memory: */
                for (int i = 0; i < m_EqualityMatrix.ColumnCount; i++)
                {
                    double product = 0.0;
                    for (int k = 0; k < Dimension; k++)
                    {
                        product += m_EqualityMatrix[k, i] * x[k];
                    }
                    if (Math.Abs(product - m_EqualityVector[i]) > tolerance)  // 'if (product != m_EqualityVector[j])' + rounding errors
                    {
                        distance += Math.Abs(product - m_EqualityVector[i]);
                        state = PointRegionRelation.Outside;
                    }
                }
                return state;
            }
            #endregion

            /// <summary>Gets the constraints in its matrix representation, i.e.
            /// <para>C^t * x = d,</para>
            /// i.e. an element x \in \R^n is inside the region if and only if x fullfills the above condition.
            /// </summary>
            /// <param name="equalityMatrix">The equality matrix C provided column-by-column, where
            /// <list type="bullet">
            /// <item><description>the number of rows will be equal to <see cref="LinearEquality.Dimension"/> and</description></item>
            /// <item><description>the number of columns corresponds to the number of constraints, i.e. <see cref="MultiDimRegion.LinearEquality.EqualityCount"/>.</description></item>
            /// </list>Additional columns will be added.</param>
            /// <param name="equalityVector">The equality vector 'd'. On exit the number of elements is at least equal to the number of columns of <paramref name="equalityMatrix"/>, i.e. will be filled with respect to the constraints represented by the current instance (output).</param>
            /// <returns>The nummber of contraints added to the arguments, i.e. identical to <see cref="MultiDimRegion.LinearEquality.EqualityCount"/>.</returns>
            public int GetRegionConstraints(IList<double> equalityMatrix, IList<double> equalityVector)
            {
                for (int j = 0; j < EqualityCount; j++)
                {
                    equalityVector.Add(m_EqualityVector[j]);
                }
                for (int j = 0; j < Dimension * EqualityCount; j++)
                {
                    equalityMatrix.Add(m_EqualityMatrix.Data[j]);
                }
                return EqualityCount;
            }

            /// <summary>Gets the constraints in its matrix representation, i.e.
            /// <para>C^t * x = d,</para>
            /// i.e. an element x \in \R^n is inside the region if and only if x fullfills the above condition.
            /// </summary>
            /// <param name="equalityMatrix">The equality matrix C provided column-by-column, where
            /// <list type="bullet">
            /// <item><description>the number of rows will be equal to <see cref="LinearEquality.Dimension"/> and</description></item>
            /// <item><description>the number of columns corresponds to the number of constraints, i.e. <see cref="MultiDimRegion.LinearEquality.EqualityCount"/>.</description></item>
            /// </list>Additional columns will be added.</param>
            /// <param name="equalityVector">The equality vector 'd'. On exit the number of elements is at least equal to the number of columns of <paramref name="equalityMatrix"/>, i.e. will be filled with respect to the constraints represented by the current instance (output).</param>
            /// <param name="offset">The arguments will be filled under the assumption that a specific number of constraints are already contained.</param>
            /// <returns>The nummber of contraints added to the arguments, i.e. identical to <see cref="MultiDimRegion.LinearEquality.EqualityCount"/>.</returns>
            public int GetRegionConstraints(Span<double> equalityMatrix, Span<double> equalityVector, int offset = 0)
            {
                BLAS.Level1.dcopy(EqualityCount, m_EqualityVector, equalityVector.Slice(offset));
                BLAS.Level1.dcopy(Dimension * EqualityCount, m_EqualityMatrix.Data, equalityMatrix.Slice(offset * Dimension));

                return EqualityCount;
            }

            /// <summary>Returns a <see cref="System.String"/> that represents this instance.
            /// </summary>
            /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
            public override string ToString()
            {
                var strBuilder = new StringBuilder();
                strBuilder.AppendLine("C^t * x = d, ");
                strBuilder.Append("d = {");
                for (int j = 0; j < m_EqualityMatrix.ColumnCount; j++)
                {
                    strBuilder.Append(" " + m_EqualityVector[j]);
                }
                strBuilder.AppendLine("}, ");
                strBuilder.Append("C = ");
                strBuilder.Append(m_EqualityMatrix.ToString());
                return strBuilder.ToString();
            }
            #endregion

            #region public static methods

            /// <summary>Creates a specific <see cref="MultiDimRegion.LinearEquality"/> object.
            /// </summary>
            /// <param name="equalityMatrix">The equality matrix C, i.e. a n x m matrix, where n is the dimension of the feasible region.</param>
            /// <param name="equalityVector">The equality vector d, i.e. a m-dimensional vector.</param>
            /// <returns>The specified <see cref="MultiDimRegion.LinearEquality"/> object.</returns>
            /// <exception cref="ArgumentNullException">Thrown if one of the arguments is <c>null</c>.</exception>
            /// <exception cref="ArgumentException">Thrown if the length of <paramref name="equalityVector"/> does not agree with the number of columns of <paramref name="equalityMatrix"/>.</exception>
            /// <remarks>A shallow copy of the arguments will be stored only. This allowes to change constraints of an optimization approach in an effective way.</remarks>
            public static LinearEquality Create(DenseMatrix equalityMatrix, double[] equalityVector)
            {
                return new LinearEquality(equalityMatrix, equalityVector);
            }
            #endregion
        }
    }
}