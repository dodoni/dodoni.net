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
        /// <summary>Represents a multi-dimensional region which is specified with respect to the following condition:
        /// <para>
        ///   C^t * x >= d,
        /// </para>
        /// i.e. an element x \in \R^n is inside the region if and only if x fullfills the above condition.
        /// </summary>
        public class LinearInequality : IMultiDimRegion
        {
            #region private members

            /// <summary>The matrix C for the condition C^t * x >= d.
            /// </summary>
            private DenseMatrix m_InequalityMatrix;

            /// <summary>The vector d for the condition C^t * x >= d.
            /// </summary>
            private double[] m_InequalityVector;
            #endregion

            #region public constructors

            /// <summary>Initializes a new instance of the <see cref="LinearInequality" /> class.
            /// </summary>
            /// <param name="inequalityMatrix">The inequality matrix C, i.e. a n x k matrix, where n is the dimension of the feasible region.</param>
            /// <param name="inequalityVector">The inequality vector d, i.e. a k-dimensional vector.</param>
            /// <exception cref="ArgumentNullException">Thrown if one of its arguments is <c>null</c>.</exception>
            /// <exception cref="ArgumentException">Thrown if the length of <paramref name="inequalityVector"/> does not agree with the number of columns of <paramref name="inequalityMatrix"/>.</exception>
            /// <remarks>A shallow copy of the arguments will be stored only.</remarks>
            public LinearInequality(DenseMatrix inequalityMatrix, double[] inequalityVector)
            {
                m_InequalityMatrix = inequalityMatrix ?? throw new ArgumentNullException(nameof(inequalityMatrix), String.Format(CultureInfo.InvariantCulture, ExceptionMessages.ArgumentNull, "Inequality matrix"));
                m_InequalityVector = inequalityVector ?? throw new ArgumentNullException(nameof(inequalityVector), String.Format(CultureInfo.InvariantCulture, ExceptionMessages.ArgumentNull, "Inequality vector"));

                int inequalityConstraintCount = inequalityMatrix.ColumnCount;
                if (inequalityConstraintCount > inequalityVector.Length)
                {
                    throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, ExceptionMessages.ArgumentHasWrongDimension, "Inequality matrix/vector"), nameof(inequalityVector));
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
                get { return m_InequalityMatrix.RowCount; }
            }
            #endregion

            /// <summary>Gets the number of inequalities.
            /// </summary>
            /// <value>The number of inequalities.</value>
            public int InequalityCount
            {
                get { return m_InequalityMatrix.ColumnCount; }
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
                /* Because of some early exit condition, we do the matrix operation 'by hand': */
                for (int i = 0; i < m_InequalityMatrix.ColumnCount; i++)
                {
                    double product = 0.0;
                    for (int k = 0; k < Dimension; k++)
                    {
                        product += m_InequalityMatrix[k, i] * x[k];
                    }
                    if (product < m_InequalityVector[i])
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
                PointRegionRelation state = PointRegionRelation.InsideOrBoundaryPoint;

                /* We do the matrix operation 'by hand'. This needs less memory - perhaps this method will be called often: */
                for (int i = 0; i < m_InequalityMatrix.ColumnCount; i++)
                {
                    double product = 0.0;
                    for (int k = 0; k < Dimension; k++)
                    {
                        product += m_InequalityMatrix[k, i] * x[k];
                    }
                    if (product < m_InequalityVector[i])
                    {
                        distance += m_InequalityVector[i] - product;
                        state = PointRegionRelation.Outside;
                    }
                }
                return state;
            }
            #endregion

            /// <summary>Gets the constraints in its matrix representation, i.e.
            /// <para>C^t * x >= d,</para>
            /// i.e. an element x \in \R^n is inside the region if and only if x fullfills the above condition.
            /// </summary>
            /// <param name="inequalityMatrix">The inequality matrix C provided column-by-column, where
            /// <list type="bullet">
            /// <item><description>the number of rows will be equal to <see cref="LinearInequality.Dimension"/> and</description></item>
            /// <item><description>the number of columns corresponds to the number of constraints, i.e. <see cref="MultiDimRegion.LinearInequality.InequalityCount"/>.</description></item>
            /// </list>Additional columns will be added.</param>
            /// <param name="inequalityVector">The inequality vector d. On exit the number of elements is at least equal to the number of columns of <paramref name="inequalityMatrix"/>, i.e. will be filled with respect to the constraints represented by the current instance (output).</param>
            /// <returns>The nummber of contraints added to the arguments, i.e. identical to <see cref="MultiDimRegion.LinearInequality.InequalityCount"/>.</returns>
            public int GetRegionConstraints(IList<double> inequalityMatrix, IList<double> inequalityVector)
            {
                for (int j = 0; j < InequalityCount; j++)
                {
                    inequalityVector.Add(m_InequalityVector[j]);
                }
                for (int j = 0; j < Dimension * InequalityCount; j++)
                {
                    inequalityMatrix.Add(m_InequalityMatrix.Data[j]);
                }
                return InequalityCount;
            }

            /// <summary>Gets the constraints in its matrix representation, i.e.
            /// <para>C^t * x >= d,</para>
            /// i.e. an element x \in \R^n is inside the region if and only if x fullfills the above condition.
            /// </summary>
            /// <param name="inequalityMatrix">The inequality matrix C provided column-by-column, where
            /// <list type="bullet">
            /// <item><description>the number of rows will be equal to <see cref="LinearInequality.Dimension"/> and</description></item>
            /// <item><description>the number of columns corresponds to the number of constraints, i.e. <see cref="MultiDimRegion.LinearInequality.InequalityCount"/>.</description></item>
            /// </list>Additional columns will be added.</param>
            /// <param name="inequalityVector">The inequality vector d. On exit the number of elements is at least equal to the number of columns of <paramref name="inequalityMatrix"/>, i.e. will be filled with respect to the constraints represented by the current instance (output).</param>
            /// <param name="offset">The arguments will be filled under the assumption that a specific number of constraints are already contained.</param>
            /// <returns>The nummber of contraints added to the arguments, i.e. identical to <see cref="MultiDimRegion.LinearInequality.InequalityCount"/>.</returns>
            public int GetRegionConstraints(Span<double> inequalityMatrix, Span<double> inequalityVector, int offset = 0)
            {
                BLAS.Level1.dcopy(InequalityCount, m_InequalityVector, inequalityVector.Slice(offset));
                BLAS.Level1.dcopy(Dimension * InequalityCount, m_InequalityMatrix.Data, inequalityMatrix.Slice(offset * Dimension));

                return InequalityCount;
            }

            /// <summary>Returns a <see cref="System.String"/> that represents this instance.
            /// </summary>
            /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
            public override string ToString()
            {
                var strBuilder = new StringBuilder();
                strBuilder.AppendLine("C^t * x >= d, ");
                strBuilder.Append("d = {");
                for (int j = 0; j < m_InequalityMatrix.ColumnCount; j++)
                {
                    strBuilder.Append(" " + m_InequalityVector[j]);
                }
                strBuilder.AppendLine("}, ");
                strBuilder.Append("C = ");
                strBuilder.Append(m_InequalityMatrix.ToString());
                return strBuilder.ToString();
            }
            #endregion

            #region public static methods

            ///<summary>Creates a specific <see cref="MultiDimRegion.LinearInequality"/> object.</summary>
            /// <param name="inequalityMatrix">The inequality matrix 'A', i.e. a 'n x k' matrix, where n is the dimension of the feasible region.</param>
            /// <param name="inequalityVector">The inequality vector 'b', i.e. a k-dimensional vector.</param>
            /// <returns>The specified <see cref="MultiDimRegion.LinearInequality"/> object.</returns>
            /// <exception cref="ArgumentNullException">Thrown if one of its arguments is <c>null</c>.</exception>
            /// <exception cref="ArgumentException">Thrown if the length of <paramref name="inequalityVector"/> does not agree with the number of columns of <paramref name="inequalityMatrix"/>.</exception>
            /// <remarks>A shallow copy of the arguments will be stored only. This allowes to change constraints of an optimization approach in an effective way.</remarks>
            public static LinearInequality Create(DenseMatrix inequalityMatrix, double[] inequalityVector)
            {
                return new LinearInequality(inequalityMatrix, inequalityVector);
            }
            #endregion
        }
    }
}