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
namespace Dodoni.MathLibrary.Native.NLopt
{
    /// <summary>Represents the result code for the NLopt library, see http://ab-initio.mit.edu/wiki/index.php/NLopt.
    /// </summary>
    public enum NLoptResultCode
    {
        /// <summary>A generic failure.
        /// </summary>
        Failure = -1,

        /// <summary>Invalid arguments.
        /// </summary>
        InvalidArguments = -2,

        /// <summary>Out of memory.
        /// </summary>
        OutOfMemory = -3,

        /// <summary>A round off limit.
        /// </summary>
        RoundoffLimited = -4,

        /// <summary>A stop forced.
        /// </summary>
        ForcedStop = -5,

        /// <summary>The calculation was successful.
        /// </summary>
        Success = 1,

        /// <summary>
        /// </summary>
        StopValReached = 2,

        /// <summary>The tolerance of the function values reached.
        /// </summary>
        FToleranceReached = 3,

        /// <summary>The tolerance of the function argument reached.
        /// </summary>
        XToleranceReached = 4,

        /// <summary>The maximual number of function evaluations reached.
        /// </summary>
        MaximalNumberOfFunctionEvaluationsReached = 5,

        /// <summary>The maximal evaulation time reached.
        /// </summary>
        MaximalTimeReached = 6
    }
}