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
    /// <summary>Represents the state of the result object of a Root-Bracketing algorithm, <see cref="IRootBracketing"/>.
    /// </summary>
    public enum RootBracketingResultState
    {
        /// <summary>The algorithm has not found a Bracketing tuple.
        /// </summary>
        NoBracketingTupleFound,

        /// <summary>The Bracketing algorithm ended normally, i.e. a root bracketing tuple has been found.
        /// </summary>
        ProperResult,

        /// <summary>The Bracketing algorithm found a root, no further algorithm should be applied.
        /// </summary>
        FoundRoot,

        /// <summary>Found a bracketing tuple, where at least one element of the tuple is less than a specific lower bound or is greater than a upper bound.
        /// </summary>
        OutOfBounds,

        /// <summary>Both points of the Bracketing tuple are (up to some tolerance) equal, i.e. a = b.
        /// </summary>
        FlatBracketingTuple,
    }
}