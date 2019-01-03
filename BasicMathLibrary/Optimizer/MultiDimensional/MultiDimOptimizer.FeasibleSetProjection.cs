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
using System.Collections.Generic;

using Dodoni.MathLibrary.Basics;
using Dodoni.MathLibrary.Miscellaneous;

namespace Dodoni.MathLibrary.Optimizer.MultiDimensional
{
    public abstract partial class MultiDimOptimizer
    {
        /// <summary>Represents the projection onto a specific feasible set, i.e. P_X(p) = argMin_{x \in X} ||x - p||^2, where X represents the (convex) feasbible set.
        /// </summary>
        public class FeasibleSetProjection
        {
            #region private members

            /// <summary>A projection onto the feasible set.
            /// </summary>
            private Action<double[]> m_ProjectionOntoFeasibleSet;
            #endregion

            #region public properties

            /// <summary>Gets the dimension of the feasible region.
            /// </summary>
            /// <value>The dimension.</value>
            public int Dimension
            {
                get;
                private set;
            }
            #endregion

            #region protected methods

            /// <summary>Initializes a new instance of the <see cref="FeasibleSetProjection" /> class.
            /// </summary>
            /// <param name="dimension">The dimension of the feasible region.</param>
            /// <param name="projectionMapping">The projection mapping, i.e. maps a arbitrary number onto the specific feasible set.</param>
            protected FeasibleSetProjection(int dimension, Action<double[]> projectionMapping)
            {
                Dimension = dimension;
                m_ProjectionOntoFeasibleSet = projectionMapping;
            }
            #endregion

            #region public methods

            /// <summary>Maps a specific point onto the specified feasible set.
            /// </summary>
            /// <param name="x">The point to map onto the specific feasible set; on exit the projected point.</param>
            public void GetValue(double[] x)
            {
                m_ProjectionOntoFeasibleSet(x);
            }
            #endregion

            #region public static methods

            /// <summary>Creates a new <see cref="FeasibleSetProjection"/> object.
            /// </summary>
            /// <param name="dimension">The dimension of the feasible region.</param>
            public static FeasibleSetProjection Create(int dimension)
            {
                return new FeasibleSetProjection(dimension, p => { }); // a projection is not necessary
            }

            /// <summary>Creates a new <see cref="FeasibleSetProjection"/> object.
            /// </summary>
            /// <param name="boxConstraints">The box constraints.</param>
            public static FeasibleSetProjection Create(MultiDimRegion.Interval boxConstraints)
            {
                /* Projection onto box constraints: P_X(p) is defined via [P_X(p)]_j = p_j if lowerBound_j < p_j < upperBound_j; [P_X(p)]_j = upperBound_j, if p_j >= upperBound_j etc. */
                return new FeasibleSetProjection(boxConstraints.Dimension, p =>
                {
                    for (int j = 0; j < boxConstraints.Dimension; j++)
                    {
                        var p_j = p[j];
                        if (p_j <= boxConstraints.LowerBounds[j])
                        {
                            p[j] = boxConstraints.LowerBounds[j];
                        }
                        else if (p_j >= boxConstraints.UpperBounds[j])
                        {
                            p[j] = boxConstraints.UpperBounds[j];
                        }
                    }
                });
            }

            /// <summary>Creates a new <see cref="FeasibleSetProjection"/> object.
            /// </summary>
            /// <param name="constraints">A collection of contraints for the optimization algorithm that represents the feasible set.</param>
            /// <param name="quadraticProgram">A <see cref="QuadraticProgram"/> object applied to the optimization problem.</param>
            public static FeasibleSetProjection Create(IEnumerable<IMultiDimRegion> constraints, QuadraticProgram quadraticProgram)
            {
                var projectionAlgorithm = quadraticProgram.Create(constraints);

                int n = projectionAlgorithm.Dimension;
                var b = new double[n];
                var unityMatrix = DenseMatrix.Unity.Create(n);

                projectionAlgorithm.SetFunction(unityMatrix, b);

                /* the projection of a point p onto the feasible set is equivalent to calculate 
                 * 
                 *    argMin_x ||x - p||^2 = argMin_x [x^t * I * x - 2 * p^t * x + p^t * p], I: unit matrix
                 *    
                 * unter the specified constraints. The term p^t * p can be dropped in the optimization. */
                return new FeasibleSetProjection(n, p =>
                 {
                     BLAS.Level1.dcopy(n, p, b);
                     BLAS.Level1.dscal(n, -2.0, b);  // b = - 2 * p

                     double minimum;
                     var state = projectionAlgorithm.FindMinimum(p, out minimum);
                 });
            }
            #endregion
        }
    }
}