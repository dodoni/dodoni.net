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
using Dodoni.MathLibrary.Miscellaneous;
using Dodoni.MathLibrary.Optimizer.MultiDimensional;

namespace Dodoni.MathLibrary.Optimizer
{
    public static partial class Extensions
    {
        /// <summary>Creates a new <see cref="IMultiDimOptimizerAlgorithm"/> object.
        /// </summary>
        /// <param name="multiDimOptimizer">The <see cref="MultiDimOptimizer"/> object.</param>
        /// <param name="constraints">A collection of contraints for the optimization algorithm.</param>
        /// <returns>A new <see cref="IMultiDimOptimizerAlgorithm"/> object.</returns>
        public static IMultiDimOptimizerAlgorithm Create(this MultiDimOptimizer multiDimOptimizer, IEnumerable<IMultiDimRegion> constraints)
        {
            return multiDimOptimizer.Create(GetConvertedConstraints(multiDimOptimizer, constraints).ToArray());
        }

        /// <summary>Creates a new <see cref="IMultiDimOptimizerAlgorithm"/> object.
        /// </summary>
        /// <param name="multiDimOptimizer">The <see cref="MultiDimOptimizer"/> object.</param>
        /// <param name="constraints">A collection of contraints for the optimization algorithm.</param>
        /// <returns>A new <see cref="IMultiDimOptimizerAlgorithm"/> object.</returns>
        public static IMultiDimOptimizerAlgorithm Create(this MultiDimOptimizer multiDimOptimizer, params IMultiDimRegion[] constraints)
        {
            return multiDimOptimizer.Create(GetConvertedConstraints(multiDimOptimizer, constraints).ToArray());
        }

        /// <summary>Converts a collection of <see cref="IMultiDimRegion"/> objects to a specific <see cref="MultiDimOptimizer.IConstraint"/> representation.
        /// </summary>
        /// <param name="multiDimOptimizer">The <see cref="MultiDimOptimizer"/> object.</param>
        /// <param name="constraints">The constraints in its generic <see cref="IMultiDimRegion"/> representation.</param>
        /// <returns>The collection of <see cref="MultiDimOptimizer.IConstraint"/> that contains the algorithm specific representation of the <paramref name="constraints"/>.</returns>
        public static IEnumerable<MultiDimOptimizer.IConstraint> GetConvertedConstraints(this MultiDimOptimizer multiDimOptimizer, IEnumerable<IMultiDimRegion> constraints)
        {
            if (constraints == null)
            {
                throw new ArgumentNullException(nameof(constraints));
            }
            int dimension = -1;

            foreach (var constraint in constraints)
            {
                if (dimension == -1)
                {
                    dimension = constraint.Dimension;
                }
                else if (dimension != constraint.Dimension)
                {
                    throw new ArgumentException(nameof(constraints));
                }
                if (constraint is MultiDimRegion.Interval)
                {
                    yield return multiDimOptimizer.Constraint.Create((MultiDimRegion.Interval)constraint);
                }
                else if (constraint is MultiDimRegion.LinearInequality)
                {
                    yield return multiDimOptimizer.Constraint.Create((MultiDimRegion.LinearInequality)constraint);
                }
                else if (constraint is MultiDimRegion.LinearEquality)
                {
                    yield return multiDimOptimizer.Constraint.Create((MultiDimRegion.LinearEquality)constraint);
                }
                else if (constraint is MultiDimRegion.Polynomial)
                {
                    yield return multiDimOptimizer.Constraint.Create((MultiDimRegion.Polynomial)constraint);
                }
                else if (constraint is MultiDimRegion.Inequality)
                {
                    yield return multiDimOptimizer.Constraint.Create((MultiDimRegion.Inequality)constraint);
                }
                else
                {
                    throw new ArgumentException(nameof(constraints));
                }
            }
        }

        /// <summary>Sets the objective function.
        /// </summary>
        /// <param name="optimizerAlgorithm">The <see cref="IMultiDimOptimizerAlgorithm"/> object.</param>
        /// <param name="objectiveFunction">The objective function.</param>
        /// <exception cref="InvalidOperationException">Thrown, if <see cref="OrdinaryMultiDimOptimizer.IFunctionFactory.GradientRequirement"/> indicates that a gradient is required.</exception>
        public static void SetFunction(this IMultiDimOptimizerAlgorithm optimizerAlgorithm, Func<double[], double> objectiveFunction)
        {
            var ordinaryFunctionDescriptor = optimizerAlgorithm.Factory.Function as OrdinaryMultiDimOptimizer.IFunctionFactory;

            if (ordinaryFunctionDescriptor != null)
            {
                optimizerAlgorithm.Function = ordinaryFunctionDescriptor.Create(optimizerAlgorithm.Dimension, objectiveFunction);
                return;
            }
            throw new InvalidOperationException();
        }

        /// <summary>Sets the objective function.
        /// </summary>
        /// <param name="optimizerAlgorithm">The <see cref="IMultiDimOptimizerAlgorithm"/> object.</param>
        /// <param name="objectiveFunction">The objective function, where the first argument is the point where to evalute and the second argument contains the gradient, 
        /// i.e. the partial derivatives of the function at the first argument; the return value is the value of the function at the first argument.</param>
        public static void SetFunction(this IMultiDimOptimizerAlgorithm optimizerAlgorithm, Func<double[], double[], double> objectiveFunction)
        {
            var ordinaryFunctionDescriptor = optimizerAlgorithm.Factory.Function as OrdinaryMultiDimOptimizer.IFunctionFactory;

            if (ordinaryFunctionDescriptor != null)
            {
                optimizerAlgorithm.Function = ordinaryFunctionDescriptor.Create(optimizerAlgorithm.Dimension, objectiveFunction);
                return;
            }
            throw new InvalidOperationException();
        }

        /// <summary>Sets a quadratic objective function 1/2 * x^t * A * x + b^t * x.
        /// </summary>
        /// <param name="optimizerAlgorithm">The <see cref="IMultiDimOptimizerAlgorithm"/> object.</param>
        /// <param name="A">The quadratic program matrix A.</param>
        /// <param name="b">The quadratic program vector b.</param>
        public static void SetFunction(this IMultiDimOptimizerAlgorithm optimizerAlgorithm, DenseMatrix A, double[] b)
        {
            var quadraticProgramFunctionDescriptor = optimizerAlgorithm.Factory.Function as QuadraticProgram.IFunctionFactory;
            if (quadraticProgramFunctionDescriptor != null)
            {
                optimizerAlgorithm.Function = quadraticProgramFunctionDescriptor.Create(A, b);
                return;
            }

            var ordinaryFunctionDescriptor = optimizerAlgorithm.Factory.Function as OrdinaryMultiDimOptimizer.IFunctionFactory;
            if (ordinaryFunctionDescriptor != null)
            {
                optimizerAlgorithm.Function = ordinaryFunctionDescriptor.Create(optimizerAlgorithm.Dimension, x => 0.5 * DenseMatrix.GetBilinearForm(A, x) + BLAS.Level1.ddot(optimizerAlgorithm.Dimension, x, b));
                return;
            }
            throw new InvalidOperationException();
        }

        /// <summary>Sets the objective function.
        /// </summary>
        /// <param name="optimizerAlgorithm">The <see cref="IMultiDimOptimizerAlgorithm"/> object.</param>
        /// <param name="codomainDimension">The dimension of the codomain, i.e. the objective function is taking values in a subset of R^k where k is the dimension of the codomain.</param>
        /// <param name="objectiveFunction">The objective function, where the first argument is the point where to evaluate and the second argument is the value of the function.</param>
        /// <returns>A specific <see cref="MultiDimOptimizer.IFunction"/> object with respect to the specified optimization algorithm.</returns>
        public static void SetFunction(this IMultiDimOptimizerAlgorithm optimizerAlgorithm, int codomainDimension, Func<double[], double[]> objectiveFunction)
        {
            var multivariateFunctionDescriptor = optimizerAlgorithm.Factory.Function as MultivariateOptimizer.IFunctionFactory;
            if (multivariateFunctionDescriptor != null)
            {
                optimizerAlgorithm.Function = multivariateFunctionDescriptor.Create(optimizerAlgorithm.Dimension, codomainDimension, objectiveFunction);
                return;
            }
            throw new InvalidOperationException();
        }

        /// <summary>Sets the objective function.
        /// </summary>
        /// <param name="optimizerAlgorithm">The <see cref="IMultiDimOptimizerAlgorithm"/> object.</param>
        /// <param name="codomainDimension">The dimension of the codomain, i.e. the objective function is taking values in a subset of R^k where k is the dimension of the codomain.</param>
        /// <param name="objectiveFunction">The objective function, where the first argument is the point where to evalute, the second argument contains the Jacobian matrix and 
        /// the last argument is the value of the function at the first argument.</param>
        /// <returns>A specific <see cref="MultiDimOptimizer.IFunction"/> object with respect to the specified optimization algorithm.</returns>
        public static void SetFunction(this IMultiDimOptimizerAlgorithm optimizerAlgorithm, int codomainDimension, Action<double[], double[], double[]> objectiveFunction)
        {
            var multivariateFunctionDescriptor = optimizerAlgorithm.Factory.Function as MultivariateOptimizer.IFunctionFactory;
            if (multivariateFunctionDescriptor != null)
            {
                optimizerAlgorithm.Function = multivariateFunctionDescriptor.Create(optimizerAlgorithm.Dimension, codomainDimension, objectiveFunction);
                return;
            }
            throw new InvalidOperationException();
        }
    }
}