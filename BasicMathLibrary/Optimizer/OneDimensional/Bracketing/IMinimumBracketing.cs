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

namespace Dodoni.MathLibrary.Optimizer.OneDimensional
{
    /// <summary>Represents the interface for classes which implements a minimum bracket algorithm,
    /// i.e. for a given real valued function generate points a,b,c with
    /// <para>
    ///     a  &lt; b &lt; c (or or c &lt; b &lt; a) and f(b) &lt; f(a), f(b) &lt; f(c).
    /// </para>
    /// where a and b are inside the domain of f.
    /// </summary>
    public interface IMinimumBracketing
    {
        /// <summary>Gets a bracketing triplet (a,b,c), starting from an interval with specified bounds, where a,b,c are inside the specific interval.
        /// </summary>
        /// <param name="functionToBracketing">The real-valued function to bracketing.</param>
        /// <param name="lowerBound">The lower bound of the function constraint (=interval), perhaps <see cref="System.Double.NegativeInfinity"/>.</param>
        /// <param name="upperBound">The upper bound of the function constraint (=interval), perhaps <see cref="System.Double.PositiveInfinity"/>.</param>
        /// <param name="interiorPoint">A interior point, i.e. a point which is strictly greater than the <paramref name="lowerBound"/> and strictly less than the <paramref name="upperBound"/>.</param>
        /// <param name="bracketingTriple">The bracketing triple (output).</param>
        /// <param name="fa">The function value at the bracketing point 'A'. </param>
        /// <param name="fb">The function value at the bracketing point 'B'. </param>
        /// <param name="fc">The function value at the bracketing point 'C'. </param>
        /// <param name="functionCallsNeeded">The number of function calls needed (output).</param>
        /// <returns>A value indicating whether <paramref name="bracketingTriple"/> contains valid data.</returns>
        MinimumBracketingResultState TryGetBracketingTriple(Func<double, double> functionToBracketing, double lowerBound, double upperBound, double interiorPoint, out BracketingTriple bracketingTriple, out double fa, out double fb, out double fc, out int functionCallsNeeded);
    }
}