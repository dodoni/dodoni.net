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

using Dodoni.MathLibrary.Miscellaneous;
using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.Optimizer.MultiDimensional
{
    public abstract partial class MultiDimOptimizer
    {
        /// <summary>Serves as interface for a factory of constraints for a specific multi-dimensional optimization.
        /// </summary>
        public interface IConstraintFactory : IInfoOutputQueriable
        {
            /// <summary>Gets the minimum dimension supported by the optimizer algorithm.
            /// </summary>
            /// <value>The minimum dimension.</value>
            /// <remarks>For example some algorithms are not able to do a one-dimensional optimization.</remarks>
            int MinimumDimension
            {
                get;
            }

            /// <summary>Gets the maximum dimension supported by the optimizer algorithm.
            /// </summary>
            /// <value>The maximum dimension.</value>
            /// <remarks>Perhaps a algorithm for a fixed dimension (for example two) is represented by the current instance.</remarks>
            int MaximumDimension
            {
                get;
            }

            /// <summary>Creates a new <see cref="MultiDimOptimizer.IConstraint"/> object.
            /// </summary>
            /// <param name="boxConstraint">The specific box constraint, i.e. the argument of the objective function are constrainted to lie in a specified hyperrectangle.</param>
            /// <returns>A specific <see cref="MultiDimOptimizer.IConstraint"/> object with respect to the specified optimization algorithm.</returns>
            /// <exception cref="InvalidOperationException">Thrown, if the optimization algorithm does not support this kind of constraint.</exception>
            IConstraint Create(MultiDimRegion.Interval boxConstraint);

            /// <summary>Creates a new <see cref="MultiDimOptimizer.IConstraint"/> object.
            /// </summary>
            /// <param name="linearInequalityConstraint">The specific constraints in its <see cref="MultiDimRegion.LinearInequality"/> representation.</param>
            /// <returns>A specific <see cref="MultiDimOptimizer.IConstraint"/> object with respect to the specified optimization algorithm.</returns>
            /// <exception cref="InvalidOperationException">Thrown, if the optimization algorithm does not support this kind of constraint.</exception>
            IConstraint Create(MultiDimRegion.LinearInequality linearInequalityConstraint);

            /// <summary>Creates a new <see cref="MultiDimOptimizer.IConstraint"/> object.
            /// </summary>
            /// <param name="linearEqualityConstraint">The specific constraints in its <see cref="MultiDimRegion.LinearEquality"/> representation.</param>
            /// <returns>A specific <see cref="MultiDimOptimizer.IConstraint"/> object with respect to the specified optimization algorithm.</returns>
            /// <exception cref="InvalidOperationException">Thrown, if the optimization algorithm does not support this kind of constraint.</exception>
            IConstraint Create(MultiDimRegion.LinearEquality linearEqualityConstraint);

            /// <summary>Creates a new <see cref="MultiDimOptimizer.IConstraint"/> object.
            /// </summary>
            /// <param name="polynomialConstraint">The specific constraints in its <see cref="MultiDimRegion.Polynomial"/> representation.</param>
            /// <returns>A specific <see cref="MultiDimOptimizer.IConstraint"/> object with respect to the specified optimization algorithm.</returns>
            /// <exception cref="InvalidOperationException">Thrown, if the optimization algorithm does not support this kind of constraint.</exception>
            IConstraint Create(MultiDimRegion.Polynomial polynomialConstraint);

            /// <summary>Creates a new <see cref="MultiDimOptimizer.IConstraint"/> object.
            /// </summary>
            /// <param name="inequality">The specific constraints in its <see cref="MultiDimRegion.Inequality"/> representation.</param>
            /// <returns>A specific <see cref="MultiDimOptimizer.IConstraint"/> object with respect to the specified optimization algorithm.</returns>
            /// <exception cref="InvalidOperationException">Thrown, if the optimization algorithm does not support this kind of constraint.</exception>
            IConstraint Create(MultiDimRegion.Inequality inequality);
        }
    }
}