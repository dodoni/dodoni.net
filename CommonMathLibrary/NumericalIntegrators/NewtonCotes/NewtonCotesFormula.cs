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
    /// <summary>Serves as factory for a closed/open Newton-Cotes integrator.
    /// </summary>
    public partial class NewtonCotesFormula
    {
        /// <summary>Serves as factory for closed Newton-Cotes integrator.
        /// </summary>
        public static class Closed
        {
            /// <summary>Creates a new <see cref="ClosedNewtonCotesFormula"/> object.
            /// </summary>
            /// <param name="rule">The Newton-Cotes rule, i.e. the degree of the Newton-Cotes method.</param>
            /// <returns>A new <see cref="ClosedNewtonCotesFormula"/> object.</returns>
            public static ClosedNewtonCotesFormula Create(ClosedNewtonCotesFormula.Rule rule)
            {
                return new ClosedNewtonCotesFormula(rule);
            }

            /// <summary>Creates a new <see cref="ClosedNewtonCotesFormula"/> object.
            /// </summary>
            /// <param name="rule">The Newton-Cotes rule, i.e. the degree of the Newton-Cotes method.</param>
            /// <param name="exitCondition">The exit condition.</param>
            /// <returns>A new <see cref="ClosedNewtonCotesFormula"/> object.</returns>
            public static ClosedNewtonCotesFormula Create(ClosedNewtonCotesFormula.Rule rule, ExitCondition exitCondition)
            {
                return new ClosedNewtonCotesFormula(rule, exitCondition);
            }
        }
    }
}