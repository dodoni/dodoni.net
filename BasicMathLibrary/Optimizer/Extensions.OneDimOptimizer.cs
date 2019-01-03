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

using Dodoni.MathLibrary.Miscellaneous;
using Dodoni.MathLibrary.Optimizer.OneDimensional;

namespace Dodoni.MathLibrary.Optimizer
{
    public static partial class Extensions
    {
        /// <summary>Creates a new <see cref="IOneDimOptimizerAlgorithm"/> object.
        /// </summary>
        /// <param name="oneDimOptimizer">The <see cref="OneDimOptimizer"/> object.</param>
        /// <param name="interval">The constraint in its <see cref="IOneDimRegion"/> representation.</param>
        /// <returns>A new <see cref="IOneDimOptimizerAlgorithm"/> object.</returns>
        public static IOneDimOptimizerAlgorithm Create(this OneDimOptimizer oneDimOptimizer, IOneDimRegion interval)
        {
            if (interval == null)
            {
                return oneDimOptimizer.Create(oneDimOptimizer.Constraint.Create(Interval.RealAxis));
            }
            var constraint = oneDimOptimizer.Constraint.Create(interval);
            return oneDimOptimizer.Create(constraint);
        }

        /// <summary>Creates a new <see cref="OneDimOptimizer.IConstraint"/> object.
        /// </summary>
        /// <param name="constraintDescriptor">The specific <see cref="OneDimOptimizer.IConstraintFactory"/> object.</param>
        /// <param name="interval">The specific constraint in its <see cref="IOneDimRegion"/> representation, i.e. the argument of the objective function are constrainted to lie in the specified interval.</param>
        /// <returns>A specific <see cref="OneDimOptimizer.IConstraint"/> object with respect to the specified optimization algorithm.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if the optimization algorithm does not support this kind of constraints.</exception>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="interval"/> can not be cast to one of the following one-dimensional interval representation:
        /// <see cref="RealAxis"/>, <see cref="Interval.Bounded"/>, <see cref="Interval.LeftBounded"/>, <see cref="Interval.RightBounded"/>.</exception>
        public static OneDimOptimizer.IConstraint Create(this OneDimOptimizer.IConstraintFactory constraintDescriptor, IOneDimRegion interval)
        {
            if (interval == null)
            {
                return constraintDescriptor.Create(Interval.RealAxis);
            }
            if (interval is RealAxis)
            {
                return constraintDescriptor.Create((RealAxis)interval);
            }
            else if (interval is Interval.Bounded)
            {
                return constraintDescriptor.Create((Interval.Bounded)interval);
            }
            else if (interval is Interval.LeftBounded)
            {
                return constraintDescriptor.Create((Interval.LeftBounded)interval);
            }
            else if (interval is Interval.RightBounded)
            {
                return constraintDescriptor.Create((Interval.RightBounded)interval);
            }
            throw new ArgumentException("Do not support the specific 1-dimensional interval constraint.", nameof(interval));
        }

        /// <summary>Sets the objective function.
        /// </summary>
        /// <param name="optimizerAlgorithm">The <see cref="IOneDimOptimizerAlgorithm"/> object.</param>
        /// <param name="objectiveFunction">The objective function.</param>
        /// <exception cref="InvalidOperationException">Thrown, if <paramref name="objectiveFunction"/> is invalid.</exception>
        public static void SetFunction(this IOneDimOptimizerAlgorithm optimizerAlgorithm, Func<double, double> objectiveFunction)
        {
            var objFunction = optimizerAlgorithm.Factory.Function.Create(objectiveFunction);
            optimizerAlgorithm.Function = objFunction;
        }

        /// <summary>Sets the objective function.
        /// </summary>
        /// <param name="optimizerAlgorithm">The <see cref="IOneDimOptimizerAlgorithm"/> object.</param>
        /// <param name="objectiveFunction">The differentiable objective function.</param>
        /// <exception cref="InvalidOperationException">Thrown, if <paramref name="objectiveFunction"/> is invalid.</exception>
        public static void SetFunction(this IOneDimOptimizerAlgorithm optimizerAlgorithm, OneDimOptimizer.DifferentiableObjectiveFunction objectiveFunction)
        {
            var objFunction = optimizerAlgorithm.Factory.Function.Create(objectiveFunction);
            optimizerAlgorithm.Function = objFunction;
        }
    }
}