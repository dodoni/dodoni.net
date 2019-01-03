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
namespace Dodoni.MathLibrary.NumericalIntegrators
{
    /// <summary>Represents the classification of a numerical integrator calculation, i.e. a value indicating whether the calculation returns a proper result etc.
    /// </summary>
    public enum NumericalIntegratorErrorClassification
    {
        /// <summary>The algorithm has not been exceeded.
        /// </summary>
        NoResult,

        /// <summary>The algorithm ended normally, i.e. some proper result of the algorithm.
        /// </summary>
        ProperResult,

        /// <summary>The maximum number of interations was exceeded.
        /// </summary>
        IterationLimitExceeded,

        /// <summary>The maximum number of function evaluations was exceeded.
        /// </summary>
        EvaluationLimitExceeded,

        /// <summary>Round-off prevented the algorithm from achieving a result within the desired tolerance.
        /// </summary>
        RoundOffError,

        /// <summary>The input was not applictable to the algorithmus, the calculation was aborted.
        /// </summary>
        InputError,

        /// <summary>The algorithm diverges, i.e. the algorithm fails.
        /// </summary>
        Divergent,

        /// <summary>A unknown state.
        /// </summary>
        Unknown
    }
}