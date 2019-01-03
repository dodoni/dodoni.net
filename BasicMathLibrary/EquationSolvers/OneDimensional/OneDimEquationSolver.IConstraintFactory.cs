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

namespace Dodoni.MathLibrary.EquationSolvers
{
    public partial class OneDimEquationSolver
    {
        /// <summary>Serves as interface for a factory of constraints for a specific 1-dimensional root finder.
        /// </summary>
        public interface IConstraintFactory : IInfoOutputQueriable
        {
            /// <summary>Creates a new <see cref="OneDimEquationSolver.IConstraint"/> object.
            /// </summary>
            /// <param name="constraint">The specific constraint, i.e. the argument of the objective function are constrainted to lie in a specified interval.</param>
            /// <returns>A specific <see cref="OneDimEquationSolver.IConstraint"/> object with respect to the specified root finder algorithm.</returns>
            /// <exception cref="InvalidOperationException">Thrown, if the root finder algorithm does not support this kind of constraints.</exception>
            IConstraint Create(Interval.Bounded constraint);

            /// <summary>Creates a new <see cref="OneDimEquationSolver.IConstraint"/> object.
            /// </summary>
            /// <param name="constraint">The specific constraint, i.e. the argument of the objective function are constrainted to lie in a specified interval.</param>
            /// <returns>A specific <see cref="OneDimEquationSolver.IConstraint"/> object with respect to the specified root finder algorithm.</returns>
            /// <exception cref="InvalidOperationException">Thrown, if the root finder algorithm does not support this kind of constraints.</exception>
            IConstraint Create(Interval.LeftBounded constraint);

            /// <summary>Creates a new <see cref="OneDimEquationSolver.IConstraint"/> object.
            /// </summary>
            /// <param name="constraint">The specific constraint, i.e. the argument of the objective function are constrainted to lie in a specified interval.</param>
            /// <returns>A specific <see cref="OneDimEquationSolver.IConstraint"/> object with respect to the specified root finder algorithm.</returns>
            /// <exception cref="InvalidOperationException">Thrown, if the root finder algorithm does not support this kind of constraints.</exception>
            IConstraint Create(Interval.RightBounded constraint);

            /// <summary>Creates a new <see cref="OneDimEquationSolver.IConstraint"/> object.
            /// </summary>
            /// <param name="constraint">The specific constraint, i.e. the argument of the objective function are constrainted to lie in a specified interval.</param>
            /// <returns>A specific <see cref="OneDimEquationSolver.IConstraint"/> object with respect to the specified root finder algorithm.</returns>
            /// <exception cref="InvalidOperationException">Thrown, if the root finder algorithm does not support this kind of constraints.</exception>
            IConstraint Create(RealAxis constraint);
        }
    }
}