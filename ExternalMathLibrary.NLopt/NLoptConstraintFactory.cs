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
using System.Collections.Generic;

using Dodoni.MathLibrary.Basics;
using Dodoni.MathLibrary.Optimizer;
using Dodoni.MathLibrary.Miscellaneous;
using Dodoni.BasicComponents.Containers;
using Dodoni.MathLibrary.Optimizer.MultiDimensional;

namespace Dodoni.MathLibrary.Native.NLopt
{
    /// <summary>Serves as factory for <see cref="NLoptConstraint"/> objects with respect to a specific NLopt optimization algorithm.
    /// </summary>
    public class NLoptConstraintFactory : MultiDimOptimizer.IConstraintFactory
    {
        #region internal (readonly) members

        /// <summary>The <see cref="NLoptMultiDimOptimizer"/> object that represents the NLopt optimization algorithm.
        /// </summary>
        internal readonly NLoptMultiDimOptimizer nloptMultiDimOptimizer;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="NLoptConstraintFactory" /> class.
        /// </summary>
        /// <param name="nloptMultiDimOptimizer">The <see cref="NLoptMultiDimOptimizer"/> object that represents the NLopt optimization algorithm.</param>
        internal NLoptConstraintFactory(NLoptMultiDimOptimizer nloptMultiDimOptimizer)
        {
            this.nloptMultiDimOptimizer = nloptMultiDimOptimizer;
        }
        #endregion

        #region public properties

        #region IConstraintFactory Members

        /// <summary>Gets the minimum dimension supported by the optimizer algorithm.
        /// </summary>
        /// <value>The minimum dimension.</value>
        /// <remarks>For example some algorithms are not able to do a one-dimensional optimization.</remarks>
        public int MinimumDimension
        {
            get { return 2; }  // here, we assume that one-dimensional problems are not covered
        }

        /// <summary>Gets the maximum dimension supported by the optimizer algorithm.
        /// </summary>
        /// <value>The maximum dimension.</value>
        /// <remarks>Perhaps a algorithm for a fixed dimension (for example two) is represented by the current instance.</remarks>
        public int MaximumDimension
        {
            get { return Int32.MaxValue; }
        }
        #endregion

        #region IInfoOutputQueriable Members

        /// <summary>Gets the info-output level of detail.
        /// </summary>
        /// <value>The info-output level of detail.</value>
        public InfoOutputDetailLevel InfoOutputDetailLevel
        {
            get { return InfoOutputDetailLevel.Full; }
        }
        #endregion

        #endregion

        #region public methods

        #region MultiDimOptimizer.IConstraintFactory Members

        /// <summary>Creates a new <see cref="MultiDimOptimizer.IConstraint"/> object.
        /// </summary>
        /// <param name="boxConstraint">The specific box constraint, i.e. the argument of the objective function are constrainted to lie in a specified hyperrectangle.</param>
        /// <returns>A specific <see cref="MultiDimOptimizer.IConstraint"/> object with respect to the specified optimization algorithm.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if the optimization algorithm does not support this kind of constraints.</exception>
        MultiDimOptimizer.IConstraint MultiDimOptimizer.IConstraintFactory.Create(MultiDimRegion.Interval boxConstraint)
        {
            return this.Create(boxConstraint);
        }

        /// <summary>Creates a new <see cref="MultiDimOptimizer.IConstraint"/> object.
        /// </summary>
        /// <param name="linearInequalityConstraint">The specific constraints in its <see cref="MultiDimRegion.LinearInequality"/> representation.</param>
        /// <returns>A specific <see cref="MultiDimOptimizer.IConstraint"/> object with respect to the specified optimization algorithm.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if the optimization algorithm does not support this kind of constraints.</exception>
        MultiDimOptimizer.IConstraint MultiDimOptimizer.IConstraintFactory.Create(MultiDimRegion.LinearInequality linearInequalityConstraint)
        {
            return this.Create(linearInequalityConstraint);
        }

        /// <summary>Creates a new <see cref="MultiDimOptimizer.IConstraint"/> object.
        /// </summary>
        /// <param name="linearEqualityConstraint">The specific constraints in its <see cref="MultiDimRegion.LinearEquality"/> representation.</param>
        /// <returns>A specific <see cref="MultiDimOptimizer.IConstraint"/> object with respect to the specified optimization algorithm.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if the optimization algorithm does not support this kind of constraints.</exception>
        MultiDimOptimizer.IConstraint MultiDimOptimizer.IConstraintFactory.Create(MultiDimRegion.LinearEquality linearEqualityConstraint)
        {
            return this.Create(linearEqualityConstraint);
        }

        /// <summary>Creates a new <see cref="MultiDimOptimizer.IConstraint"/> object.
        /// </summary>
        /// <param name="polynomialConstraint">The specific constraints in its <see cref="MultiDimRegion.Polynomial"/> representation.</param>
        /// <returns>A specific <see cref="MultiDimOptimizer.IConstraint"/> object with respect to the specified optimization algorithm.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if the optimization algorithm does not support this kind of constraints.</exception>
        MultiDimOptimizer.IConstraint MultiDimOptimizer.IConstraintFactory.Create(MultiDimRegion.Polynomial polynomialConstraint)
        {
            return this.Create(polynomialConstraint);
        }

        /// <summary>Creates a new <see cref="MultiDimOptimizer.IConstraint"/> object.
        /// </summary>
        /// <param name="inequality">The specific constraints in its <see cref="MultiDimRegion.Inequality"/> representation.</param>
        /// <returns>A specific <see cref="MultiDimOptimizer.IConstraint"/> object with respect to the specified optimization algorithm.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if the optimization algorithm does not support this kind of constraints.</exception>
        MultiDimOptimizer.IConstraint MultiDimOptimizer.IConstraintFactory.Create(MultiDimRegion.Inequality inequality)
        {
            return this.Create(inequality.Dimension, inequality.GetValue, inequality.Tolerance);
        }
        #endregion

        #region IInfoOutputQueriable Members

        /// <summary>Sets the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> property.
        /// </summary>
        /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
        /// <returns>A value indicating whether the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> has been set to <paramref name="infoOutputDetailLevel" />.</returns>
        public bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
        {
            return (infoOutputDetailLevel == InfoOutputDetailLevel.Full);
        }

        /// <summary>Gets informations of the current object as a specific <see cref="InfoOutput" /> instance.
        /// </summary>
        /// <param name="infoOutput">The <see cref="InfoOutput" /> object which is to be filled with informations concering the current instance.</param>
        /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
        public void FillInfoOutput(InfoOutput infoOutput, string categoryName = InfoOutput.GeneralCategoryName)
        {
            var infoOutputPackage = infoOutput.AcquirePackage(categoryName);
            infoOutputPackage.Add("Minimal dimension", MinimumDimension);
            infoOutputPackage.Add("Maximal dimension", MaximumDimension);
        }
        #endregion

        /// <summary>Creates a new <see cref="NLoptConstraint"/> object.
        /// </summary>
        /// <param name="boxConstraint">The specific box constraint, i.e. the argument of the objective function are constrainted to lie in a specified hyperrectangle.</param>
        /// <returns>A specific <see cref="NLoptConstraint"/> object with respect to the specified optimization algorithm.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if the optimization algorithm does not support this kind of constraints.</exception>
        public NLoptConstraint Create(MultiDimRegion.Interval boxConstraint)
        {
            if (boxConstraint == null)
            {
                throw new ArgumentNullException(nameof(boxConstraint));
            }
            if (nloptMultiDimOptimizer.Configuration.BoxConstraintRequirement == NLoptBoundConstraintRequirement.None)
            {
                throw new InvalidOperationException("The NLopt algorithm does not support box constraints");
            }
            return new NLoptConstraint(this,
                                        boxConstraint.Dimension,
                                        "Box constraint",
                                        nloptPtr =>
                                        {
                                            nloptPtr.SetLowerBounds(boxConstraint.LowerBounds.ToArray());
                                            nloptPtr.SetUpperBounds(boxConstraint.UpperBounds.ToArray());
                                        },
                                        infoOutputPackage => { });
        }

        /// <summary>Creates a new <see cref="NLoptConstraint"/> object.
        /// </summary>
        /// <param name="linearInequalityConstraint">The specific constraints in its <see cref="MultiDimRegion.LinearInequality"/> representation.</param>
        /// <returns>A specific <see cref="NLoptConstraint"/> object with respect to the specified optimization algorithm.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if the optimization algorithm does not support this kind of constraints.</exception>
        public NLoptConstraint Create(MultiDimRegion.LinearInequality linearInequalityConstraint)
        {
            if (linearInequalityConstraint == null)
            {
                throw new ArgumentNullException(nameof(linearInequalityConstraint));
            }
            if (nloptMultiDimOptimizer.Configuration.NonlinearConstraintRequirement.HasFlag(NLoptNonlinearConstraintRequirement.SupportsInequalityConstraints) == false)
            {
                throw new InvalidOperationException("The NLopt algorithm does not support linear inequality constraints");
            }

            var entriesOfA = new List<double>();
            var entriesOfb = new List<double>();
            int r = linearInequalityConstraint.GetRegionConstraints(entriesOfA, entriesOfb);  // condition A^t * x >= b should be written as c(x) = b - A^t * x 

            var A = entriesOfA.ToArray(); // matrix A is of type d[dimension] x r, provided column-by-column
            var b = entriesOfb.ToArray();

            NLoptVectorInequalityConstraintFunction c = (m, result, n, x, grad, data) =>  // todo: bug in nlopt(?) - argument x has wrong dimension
            {
                BLAS.Level2.dgemv(n, m, -1.0, A, x, 0.0, result, BLAS.MatrixTransposeState.Transpose);
                VectorUnit.Basics.Add(r, result, b);

                if (grad != null)
                {
                    /* it holds \partial c_i / \partial x_j = A[j,i] which should be stored in grad[i * n + j], i = 0,...,m-1, j = 0,...,n-1 */
                    for (int i = 0; i < m; i++)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            grad[i * n + j] = A[j + i * n]; // can be improved
                        }
                    }
                }
            };

            var tolerance = Enumerable.Repeat(MachineConsts.Epsilon, r).ToArray();

            return new NLoptConstraint(this,
                           linearInequalityConstraint.Dimension,
                           "Linear Inequality constraint",
                            nloptPtr =>
                            {
                                var code = nloptPtr.AddInequalityConstraint(c, r, tolerance);
                                if (code != NLoptResultCode.Success)
                                {
                                    throw new InvalidOperationException(String.Format("Constraint can not be set to NLopt algorithm {0}; error code: {1}.", nloptPtr.ToString(), code));
                                }
                            },
                            infoOutputPackage => { }
                            );
        }

        /// <summary>Creates a new <see cref="NLoptConstraint"/> object.
        /// </summary>
        /// <param name="linearEqualityConstraint">The specific constraints in its <see cref="MultiDimRegion.LinearEquality"/> representation.</param>
        /// <returns>A specific <see cref="NLoptConstraint"/> object with respect to the specified optimization algorithm.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if the optimization algorithm does not support this kind of constraints.</exception>
        public NLoptConstraint Create(MultiDimRegion.LinearEquality linearEqualityConstraint)
        {
            if (linearEqualityConstraint == null)
            {
                throw new ArgumentNullException(nameof(linearEqualityConstraint));
            }
            if (nloptMultiDimOptimizer.Configuration.NonlinearConstraintRequirement.HasFlag(NLoptNonlinearConstraintRequirement.SupportsEqualityConstraints) == false)
            {
                throw new InvalidOperationException("The NLopt algorithm does not support linear equality constraints");
            }

            throw new NotImplementedException();
        }

        /// <summary>Creates a new <see cref="NLoptConstraint"/> object.
        /// </summary>
        /// <param name="polynomialConstraint">The specific constraints in its <see cref="MultiDimRegion.Polynomial"/> representation.</param>
        /// <returns>A specific <see cref="NLoptConstraint"/> object with respect to the specified optimization algorithm.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if the optimization algorithm does not support this kind of constraints.</exception>
        public NLoptConstraint Create(MultiDimRegion.Polynomial polynomialConstraint)
        {
            if (polynomialConstraint == null)
            {
                throw new ArgumentNullException(nameof(polynomialConstraint));
            }
            if (nloptMultiDimOptimizer.Configuration.NonlinearConstraintRequirement.HasFlag(NLoptNonlinearConstraintRequirement.SupportsInequalityConstraints) == false)
            {
                throw new InvalidOperationException("The NLopt algorithm does not support linear inequality for polynomial constraints");
            }

            /* we split the constraint to at most two NLopt constraints c_1(x) <= \epsilon and c_2(x) <= \epsilon, where 
             *    c_1(x) = P(x) - upper bound, 
             *    c_2(x) = lower bound - P(x),
             *    where P(x) is the polynomial representation of the constraint.
             *    
             * We use the NLopt vector constraint representation to combine both constraint, because the evaluation is almost identically.
             */

            if ((Double.IsNegativeInfinity(polynomialConstraint.LowerBound) == false) && (Double.IsPositiveInfinity(polynomialConstraint.UpperBound) == false))
            {
                NLoptVectorInequalityConstraintFunction c = (m, result, n, x, grad, data) =>
                {
                    double polynomialValue = 0.0;
                    foreach (var term in polynomialConstraint.Terms)
                    {
                        double temp = 0.0;
                        foreach (var monomial in term.Item2)
                        {
                            temp += DoMath.Pow(x[monomial.ArgumentIndex], monomial.Power);
                        }
                        polynomialValue += term.Item1 * temp;
                    }

                    if (grad != null)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            grad[j] = 0.0;
                        }
                        foreach (var term in polynomialConstraint.Terms)
                        {
                            foreach (var monomial in term.Item2)
                            {
                                grad[monomial.ArgumentIndex] -= term.Item1 * monomial.Power * DoMath.Pow(x[monomial.ArgumentIndex], monomial.Power - 1);
                            }
                        }
                    }
                    result[0] = polynomialValue - polynomialConstraint.UpperBound;
                    result[1] = polynomialConstraint.LowerBound - polynomialValue;
                };

                var tolerance = new[] { MachineConsts.Epsilon, MachineConsts.Epsilon };

                return new NLoptConstraint(this,
                               polynomialConstraint.Dimension,
                               "Polynomial constraint",
                                nloptPtr =>
                                {
                                    var code = nloptPtr.AddInequalityConstraint(c, 2, tolerance);
                                    if (code != NLoptResultCode.Success)
                                    {
                                        throw new InvalidOperationException(String.Format("Constraint can not be set to NLopt algorithm {0}; error code: {1}.", nloptPtr.ToString(), code));
                                    }
                                }
                                );
            }
            else if (Double.IsNegativeInfinity(polynomialConstraint.LowerBound) == false)
            {
                return CreateSingle(polynomialConstraint, polynomialConstraint.LowerBound, sign: 1);
            }
            else
            {
                return CreateSingle(polynomialConstraint, polynomialConstraint.UpperBound, sign: -1);
            }
        }

        /// <summary>Creates a new <see cref="NLoptConstraint"/> object.
        /// </summary>
        /// <param name="dimension">The dimension of the constrain region.</param>
        /// <param name="inequalityConstraintFunction">The inequality constraint function c(x), i.e. a argument x will be accepted iff c(x) &lt; <paramref name="tolerance"/>,
        /// where the first argument is the point where to evalute and the second argument contains the gradient (perhaps <c>null</c>), i.e. the partial derivatives of the function at the first argument; 
        /// the return value is the value of the function at the first argument.</param>
        /// <param name="tolerance">The tolerance to take into account.</param>
        /// <returns>A specific <see cref="NLoptConstraint"/> object with respect to the specified optimization algorithm.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if the optimization algorithm does not support this kind of constraints.</exception>
        public NLoptConstraint Create(int dimension, Func<double[], double[], double> inequalityConstraintFunction, double tolerance = MachineConsts.Epsilon)
        {
            if (inequalityConstraintFunction == null)
            {
                throw new ArgumentNullException(nameof(inequalityConstraintFunction));
            }
            if (nloptMultiDimOptimizer.Configuration.NonlinearConstraintRequirement.HasFlag(NLoptNonlinearConstraintRequirement.SupportsInequalityConstraints) == false)
            {
                throw new InvalidOperationException("The NLopt algorithm does not support inequality constraints.");
            }

            NLoptInequalityConstraintFunction c = (n, x, grad, data) => inequalityConstraintFunction(x, grad);

            return new NLoptConstraint(this,
                         dimension,
                         "Inequality constraint",
                          nloptPtr =>
                          {
                              var code = nloptPtr.AddInequalityConstraint(c, tolerance);
                              if (code != NLoptResultCode.Success)
                              {
                                  throw new InvalidOperationException(String.Format("Constraint can not be set to NLopt algorithm {0}; error code: {1}.", nloptPtr.ToString(), code));
                              }
                          }
                          );
        }
        #endregion

        #region private methods

        /// <summary>Creates a specific (one-side) NLopt constraint which is specified in its <see cref="MultiDimRegion.Polynomial"/> representation.
        /// </summary>
        /// <param name="polynomialConstraint">The polynomial constraint.</param>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="sign">The sign.</param>
        /// <returns>The specific <see cref="NLoptConstraint"/> object.</returns>
        private NLoptConstraint CreateSingle(MultiDimRegion.Polynomial polynomialConstraint, double lowerBound, int sign = 1)
        {
            NLoptInequalityConstraintFunction c = (n, x, grad, data) =>
            {
                double polynomialValue = 0.0;
                foreach (var term in polynomialConstraint.Terms)
                {
                    double temp = 0.0;
                    foreach (var monomial in term.Item2)
                    {
                        temp += DoMath.Pow(x[monomial.ArgumentIndex], monomial.Power);
                    }
                    polynomialValue += term.Item1 * temp;
                }

                if (grad != null)
                {
                    for (int j = 0; j < n; j++)
                    {
                        grad[j] = 0.0;
                    }

                    foreach (var term in polynomialConstraint.Terms)
                    {
                        foreach (var monomial in term.Item2)
                        {
                            grad[monomial.ArgumentIndex] -= term.Item1 * monomial.Power * DoMath.Pow(x[monomial.ArgumentIndex], monomial.Power - 1);
                        }
                    }
                }
                return sign * (lowerBound - polynomialValue);
            };

            return new NLoptConstraint(this,
                           polynomialConstraint.Dimension,
                           "Polynomial constraint",
                            nloptPtr =>
                            {
                                var code = nloptPtr.AddInequalityConstraint(c, MachineConsts.Epsilon);
                                if (code != NLoptResultCode.Success)
                                {
                                    throw new InvalidOperationException(String.Format("Constraint can not be set to NLopt algorithm {0}; error code: {1}.", nloptPtr.ToString(), code));
                                }
                            }
                            );
        }
        #endregion
    }
}