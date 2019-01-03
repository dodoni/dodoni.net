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

namespace Dodoni.MathLibrary.ProbabilityTheory.MonteCarloEngine
{
    /// <summary>Provides interfaces and structures etc. for handling Random Number sequences.
    /// </summary>
    public partial class RandomNumberSequence
    {
        /// <summary>Represents a value indicating whether a specified <see cref="IRandomNumberStream"/> object allowes a non-overlapping sequence generation.
        /// </summary>
        [Flags]
        public enum SplittingApproach
        {
            /// <summary>Splitting of original random sequence into non-overlapping subsequences is not supported.
            /// </summary>
            None = 0,

            /// <summary>A Block-splitting, i.e. skipping elements of the random number sequence by a specified number.
            /// </summary>
            SkipAhead = 0x01,

            /// <summary>The leapfrog approach, sequences that can be guaranteed not to overlap for a certain period.
            /// </summary>
            LeapFrog = 0x02
        }
    }
}
