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
    /// <summary>Implements a 'simple downhill' bracket algorithm, i.e. for a given real 
    /// valued function generate points a,b,c with
    /// <para>
    ///  a  &lt; b &lt; c  or (c &lt; b &lt; a) and f(b) &lt; f(a), f(b) &lt; f(c).
    /// </para>
    /// where a and b are inside the domain of f. 
    /// </summary>
    /// <remarks>The implementation is based on
    /// <para>Ian T. Nabney, "NETLAB Algorithms for Pattern Recognition", Advanced in Pattern Recognition, §2.3.3</para>
    /// as well as on
    /// <para>Press, et al. (1992) "Numerical recipes in C", 2nd ed., p.400.</para></remarks>
    public class DownhillBracketing : IMinimumBracketing
    {
        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="DownhillBracketing"/> class.
        /// </summary>
        /// <param name="maxEvaluations">The maximal number of function evaluations during the calculation.</param>
        /// <param name="maxMagnifier">The maximum magnification allowed for a parabolic-fit step.</param>        
        /// <param name="magnifierRatio">The ratio by which successive intervals are magnified.</param>
        /// <param name="leftShift">A value taken into account to calculate the lower bound w.r.t. to an initial value if and only if the lower bound specified in <see cref="TryGetBracketingTriple(tRealFunction, double, double, double, out BracketingTriple, out double, out double, out double, out int)"/> is -\infty.</param>
        /// <param name="rightShift">A value taken into account to calculate the upper bound w.r.t. to an initial value if and only if the upper bound specified in <see cref="TryGetBracketingTriple(tRealFunction, double, double, double, out BracketingTriple, out double, out double, out double, out int)"/> is \infty.</param>
        public DownhillBracketing(int maxEvaluations = 100, double maxMagnifier = 10.0, double magnifierRatio = MathConsts.GoldenRatio, double leftShift = -100.0, double rightShift = 100.0)
        {
            if (maxEvaluations <= 1)
            {
                throw new ArgumentException(nameof(maxEvaluations));
            }
            MaxEvaluations = maxEvaluations;
            MaxMagnifier = maxMagnifier;
            MagnifierRatio = magnifierRatio;
            LeftShift = leftShift;
            RightShift = rightShift;
        }
        #endregion

        #region public properties

        #region IMinimumBracketing Members

        /// <summary>Gets the maximal number of function evaluations during the calculation.
        /// </summary>
        /// <value>The maximum number of function evaluations.</value>
        public int MaxEvaluations
        {
            get;
            private set;
        }
        #endregion

        /// <summary>Gets the magnifier ratio, i.e. the ratio by which successive intervals are magnified.
        /// </summary>
        /// <value>The magnifier ratio.</value>
        public double MagnifierRatio
        {
            get;
            private set;
        }

        /// <summary>Gets the maximum magnifier, i.e. the maximum magnification allowed for a parabolic-fit step.
        /// </summary>
        /// <value>The maximum magnifier.</value>
        public double MaxMagnifier
        {
            get;
            private set;
        }

        /// <summary>Gets a value taken into account to calculate the lower bound w.r.t. to an initial value if and only if the lower bound specified in <see cref="TryGetBracketingTriple(tRealFunction, double, double, double, out BracketingTriple, out double, out double, out double, out int)"/> is -\infty.
        /// </summary>
        /// <value>A value taken into account to calculate the lower bound w.r.t. to an initial value if and only if the lower bound specified in <see cref="TryGetBracketingTriple(tRealFunction, double, double, double, out BracketingTriple, out double, out double, out double, out int)"/> is -\infty.</value>
        public double LeftShift
        {
            get;
            private set;
        }

        /// <summary>Gets a value taken into account to calculate the upper bound w.r.t. to an initial value if and only if the upper bound specified in <see cref="TryGetBracketingTriple(tRealFunction, double, double, double, out BracketingTriple, out double, out double, out double, out int)"/> is \infty.
        /// </summary>
        /// <value>A value taken into account to calculate the upper bound w.r.t. to an initial value if and only if the upper bound specified in <see cref="TryGetBracketingTriple(tRealFunction, double, double, double, out BracketingTriple, out double, out double, out double, out int)"/> is \infty.</value>
        public double RightShift
        {
            get;
            private set;
        }
        #endregion

        #region public methods

        #region IMinimumBracketing Members

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
        public MinimumBracketingResultState TryGetBracketingTriple(Func<double, double> functionToBracketing, double lowerBound, double upperBound, double interiorPoint, out BracketingTriple bracketingTriple, out double fa, out double fb, out double fc, out int functionCallsNeeded)
        {
            bracketingTriple = null;

            /* In the unconstraint case we take the 'interiorPoint' +/- some fixed number (=100) which should not be small [1.0 gives wrong results even for the simplest functions]. 
             * Otherwise we use the lower/upper bound and hopefully the algorithm returns values between the given bounds too: */
            double a = Double.IsInfinity(lowerBound) ? interiorPoint + LeftShift : lowerBound;
            double b = Double.IsPositiveInfinity(upperBound) ? interiorPoint + RightShift : upperBound;
            double c;

            fa = functionToBracketing(a);
            fb = functionToBracketing(b);

            functionCallsNeeded = 2;

            if (fb > fa)
            {
                c = b;
                fc = fb;   // in this case, we use fc = f(c) to check the condition f(b) < f(a), f(b) < f(c) at the end of this method
                b = interiorPoint;
                fb = functionToBracketing(b);
                functionCallsNeeded++;
                while ((fb > fa) && (functionCallsNeeded <= MaxEvaluations))
                {
                    c = b;
                    fc = fb;
                    b = a + (c - a) / MagnifierRatio;
                    fb = functionToBracketing(b);
                    functionCallsNeeded++;
                }
            }
            else
            {
                c = interiorPoint;
                fc = functionToBracketing(c);
                functionCallsNeeded++;
                bool bracketFound = false;

                while ((fb > fc) && (functionCallsNeeded <= MaxEvaluations))
                {
                    double r = (b - a) * (fb - fc);
                    double q = (b - c) * (fb - fa);
                    double u = b - ((b - c) * q - (b - a) * r) / (2.0 * Math.Sign(q - r) * Math.Max(Math.Abs(q - r), MachineConsts.ExtremeTinyEpsilon));
                    double ulimit = b + MaxMagnifier * (c - b);
                    double fu;

                    if ((b - u) * (u - c) > 0.0)  // Parabolic u is between b and c
                    {
                        fu = functionToBracketing(u);
                        functionCallsNeeded++;
                        if (fu < fc)  // got a minimum between b and c
                        {
                            bracketingTriple = new BracketingTriple(b, u, c);
                            return MinimumBracketingResultState.ProperResult;
                        }
                        if (fu > fb)  // got a minimum between a and u
                        {
                            bracketingTriple = new BracketingTriple(a, c, u);
                            return MinimumBracketingResultState.ProperResult;
                        }
                        u = c + MagnifierRatio * (c - b);  // parabolic fit was not use. Use default magnification
                    }
                    else if ((c - u) * (u - ulimit) > 0.0)  // Parabolic fit is between c and its allowed limits
                    {
                        fu = functionToBracketing(u);
                        functionCallsNeeded++;
                        if (fu < fc)
                        {
                            b = c;
                            c = u;
                            u = c + MagnifierRatio * (c - b);
                        }
                        else
                        {
                            bracketFound = true;
                        }
                    }
                    else if ((u - ulimit) * (ulimit - c) >= 0.0)  // Limit parabolic u to maximum allowed value
                    {
                        u = ulimit;
                    }
                    else  // reject parabolic u, use default magnification
                    {
                        u = c + MagnifierRatio * (c - b);
                    }

                    a = b;
                    b = c;
                    c = u;
                    fa = fb;
                    fb = fc;
                    if (bracketFound)
                    {
                        fc = functionToBracketing(c);
                        functionCallsNeeded++;
                    }
                }
            }

            // check whether a = b = c up to some epsilon
            if ((Math.Abs(b - a) < MachineConsts.TinyEpsilon) && (Math.Abs(c - b) < MachineConsts.TinyEpsilon))
            {
                bracketingTriple = new BracketingTriple(a, b, c);
                return MinimumBracketingResultState.FlatBracketingTriple;
            }

            // perhaps, we have to sort a, b and c in increasing order
            if (a < c)
            {
                if (b < a)  // b < a < c
                {
                    bracketingTriple = new BracketingTriple(b, a, c);
                }
                else if (b > c)  // a < c < b
                {
                    bracketingTriple = new BracketingTriple(a, c, b);
                }
                else  // a < b < c
                {
                    bracketingTriple = new BracketingTriple(a, b, c);
                }
            }
            else  // c <= a
            {
                if (b > a)  // c < a < b
                {
                    bracketingTriple = new BracketingTriple(c, a, b);
                }
                else if (b < c)  // b < c < a
                {
                    bracketingTriple = new BracketingTriple(b, c, a);
                }
                else  // c < b < a
                {
                    bracketingTriple = new BracketingTriple(c, b, a);
                }
            }

            // lets check whether the three points are inside the given range:
            if ((bracketingTriple.A < lowerBound) || (bracketingTriple.C > upperBound))
            {
                return MinimumBracketingResultState.OutOfBounds;
            }

            if ((fa < fb) || (fc < fb))  // is it a Bracketing triple?
            {
                return MinimumBracketingResultState.NoBracketingTripleFound;
            }
            return MinimumBracketingResultState.ProperResult;
        }
        #endregion

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return String.Format("Downhill Bracketing: MaxEvaluations = {1}; MaxMagnifier = {2}; MagnifierRatio = {3}; Left-Shift = {4}; Right-Shift = {5}", MaxEvaluations, MaxMagnifier, MagnifierRatio, LeftShift, RightShift);
        }
        #endregion
    }
}