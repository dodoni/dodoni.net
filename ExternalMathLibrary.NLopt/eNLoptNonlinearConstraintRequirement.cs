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

namespace Dodoni.MathLibrary.Native.NLopt
{
    /// <summary>Represents the requirements of nonlinear constraints for a specific optimization algorithm with respect to the NLopt library 2.4.x, see http://ab-initio.mit.edu/wiki/index.php/NLopt.
    /// </summary>
    [Flags]
    public enum NLoptNonlinearConstraintRequirement
    {
        /// <summary>The optimization algorithm does not support nonlinear constraints.
        /// </summary>
        None = 0,

        /// <summary>The optimization algorithm supports nonlinear inequality constraints.
        /// </summary>
        SupportsInequalityConstraints = 0x01,

        /// <summary>The optimization algorithm requires nonlinear inequality constraints.
        /// </summary>
        RequiredInequalityConstraints = 0x03,

        /// <summary>The optimization algorithm supports nonlinear equality constraints.
        /// </summary>
        SupportsEqualityConstraints = 0x04,

        /// <summary>The optimization algorithm requires nonlinear equality constraints.
        /// </summary>
        RequiredEqualityConstraints = 0x0c,

        /// <summary>The optimization algorithm supports nonlinear inequality and nonlinear equality constraints.
        /// </summary>
        SupportsInequalityAndEqualityConstraints = SupportsInequalityConstraints | SupportsEqualityConstraints,

        /// <summary>The optimization algorithm requires nonlinear inequality and nonlinear equality constraints.
        /// </summary>
        RequiredInequalityAndEqualityConstraints = RequiredInequalityConstraints | RequiredEqualityConstraints
    }
}