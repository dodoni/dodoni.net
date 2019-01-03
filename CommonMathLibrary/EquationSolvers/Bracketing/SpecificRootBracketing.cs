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
    public class SpecificRootBracketing : IRootBracketing
    {
        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="SpecificRootBracketing" /> class.
        /// </summary>
        /// <param name="first">The first component of the bracketing tuple.</param>
        /// <param name="second">The second component of the bracketing tuple.</param>
        /// <param name="tolerance">A value indicating whether a floating point number should be interpreted as <c>0</c>.</param>
        public SpecificRootBracketing(double first, double second, double tolerance = MachineConsts.Epsilon)
        {
            if (first < second)
            {
                First = first;
                Second = second;
            }
            else
            {
                First = second;
                Second = first;
            }
            Tolerance = tolerance;
        }
        #endregion

        #region public properties

        /// <summary>Gets the first component of the bracketing tuple.
        /// </summary>
        /// <value>The first component of the bracketing tuple.</value>
        public double First
        {
            get;
            private set;
        }

        /// <summary>Gets the second component of the bracketing tuple.
        /// </summary>
        /// <value>The second component of the bracketing tuple.</value>
        public double Second
        {
            get;
            private set;
        }

        /// <summary>Gets the tolerance, i.e. a value indicating whether a floating point number should be interpreted as <c>0</c>.
        /// </summary>
        /// <value>The tolerance.</value>
        public double Tolerance
        {
            get;
            private set;
        }
        #endregion

        #region public methods

        #region IRootBracketing Members

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
        public RootBracketingResultState TryGetBracketingTuple(Func<double, double> functionToBracketing, double lowerBound, double upperBound, double initialGuess, out double a, out double fa, out double b, out double fb, out int evaluationsNeeded)
        {
            a = First;
            fa = functionToBracketing(a);

            b = Second;
            fb = functionToBracketing(b);

            evaluationsNeeded = 2;

            if ((a < lowerBound) || (b > upperBound))
            {
                return RootBracketingResultState.OutOfBounds;
            }
            if ((Math.Abs(fa) < Tolerance) || (Math.Abs(fb) < Tolerance))
            {
                if (Math.Abs(fa) <= Math.Abs(fb))
                {
                    b = a;
                    fb = a;
                }
                else
                {
                    a = b;
                    fa = fb;
                }
                return RootBracketingResultState.FoundRoot;
            }            

            if (Math.Sign(fa) == Math.Sign(fb))
            {
                if (Math.Abs(a - b) < Tolerance)
                {
                    return RootBracketingResultState.FlatBracketingTuple;
                }
                return RootBracketingResultState.NoBracketingTupleFound;
            }
            return RootBracketingResultState.ProperResult;
        }
        #endregion

        #endregion

        #region public static methods

        public static IRootBracketing Create(double first, double second)
        {
            return new SpecificRootBracketing(first, second);
        }
        #endregion
    }
}