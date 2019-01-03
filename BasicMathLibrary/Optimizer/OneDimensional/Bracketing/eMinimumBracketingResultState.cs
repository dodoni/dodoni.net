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
namespace Dodoni.MathLibrary.Optimizer.OneDimensional
{
    /// <summary>Represents the state of the result object of a Minimum-Bracketing algorithm, <seealso cref="IMinimumBracketing"/>.
    /// </summary>
    public enum MinimumBracketingResultState
    {
        /// <summary>The Bracketing algorithm ended normally, i.e. some proper result of the algorithm is given.
        /// </summary>
        ProperResult,

        /// <summary>Found a bracketing triple where at least one element of the triple is less than a specific lower bound or is greater than a upper bound.
        /// </summary>
        OutOfBounds,

        /// <summary>All points of the Bracketing triple are (up to some tolerance) equal, i.e. a=b=c.
        /// </summary>
        /// <remarks>This case may occurs if the minimum is situated at the boundary of the constraint interval.</remarks>
        FlatBracketingTriple,

        /// <summary>The algorithm has not found a Bracketing triple.
        /// </summary>
        NoBracketingTripleFound
    }
}