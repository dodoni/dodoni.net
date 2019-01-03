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

namespace Dodoni.MathLibrary.EquationSolvers
{
    /// <summary>Represents the interface for classes that implements a root bracket algorithm, i.e. for a real valued function f generate 
    /// points a,b such that f(a), f(b) have different sign; or one of the argument is <c>0.0</c>. Furthermore a and b are inside the domain of f.
    /// </summary>
    public interface IRootBracketing
    {
        /// <summary>Gets a bracketing tuple (a,b), starting from an interval with specified bounds, where a,b are inside the specific interval.
        /// </summary>
        /// <param name="functionToBracketing">The real-valued function to bracketing.</param>
        /// <param name="lowerBound">The lower bound of the function constraint (=interval), perhaps <see cref="System.Double.NegativeInfinity"/>.</param>
        /// <param name="upperBound">The upper bound of the function constraint (=interval), perhaps <see cref="System.Double.PositiveInfinity"/>.</param>
        /// <param name="initialGuess">An initial guess of the algorithm (if applicable).</param>
        /// <param name="a">The first component of the bracketing tuple (output).</param>
        /// <param name="b">The second component of the bracketing tuple (output).</param>
        /// <param name="fa">The function value at the bracketing point <paramref name="a"/> (output).</param>
        /// <param name="fb">The function value at the bracketing point <paramref name="b"/> (output). </param>
        /// <param name="evaluationsNeeded">The number of function evaluations needed by the algorithm to reach the desired accuracy.</param>
        /// <returns>A value indicating whether <paramref name="a"/> and <paramref name="b"/> contain valid data.</returns>
        RootBracketingResultState TryGetBracketingTuple(Func<double, double> functionToBracketing, double lowerBound, double upperBound, double initialGuess, out double a, out double fa, out double b, out double fb, out int evaluationsNeeded);
    }
}