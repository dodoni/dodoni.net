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

using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.EquationSolvers.OneDimensional
{
    public partial class BrentRootFinder
    {
        /// <summary>The implementation of Brent's root finding algorithm.
        /// </summary>
        private class Algorithm : IOneDimEquationSolverAlgorithm
        {
            #region private members

            /// <summary>The <see cref="BrentRootFinder"/> object that serves as factory of the current object.
            /// </summary>
            private BrentRootFinder m_RootFinder;

            /// <summary>The objective function in its <see cref="OneDimRootFinderFunction"/> representation.
            /// </summary>
            private OneDimRootFinderFunction m_ObjectiveFunction;

            /// <summary>The constraint in its <see cref="OneDimRootFinderConstraint"/> representation.
            /// </summary>
            private OneDimRootFinderConstraint m_Constraint;
            #endregion

            #region public constructors

            /// <summary>Initializes a new instance of the <see cref="Algorithm" /> class.
            /// </summary>
            /// <param name="rootFinder">The root finder.</param>
            /// <param name="constraint">The constraint.</param>
            public Algorithm(BrentRootFinder rootFinder, OneDimRootFinderConstraint constraint)
            {
                m_RootFinder = rootFinder;
                m_Constraint = constraint;
            }
            #endregion

            #region public properties

            #region IOneDimEquationSolverAlgorithm Members

            /// <summary>Gets the factory for further <see cref="IOneDimEquationSolverAlgorithm" /> objects of the same type, i.e. with the same stopping condition etc.
            /// </summary>
            /// <value>The factory for further <see cref="IOneDimEquationSolverAlgorithm" /> objects of the same type.</value>
            public OneDimEquationSolver Factory
            {
                get { return m_RootFinder; }
            }

            /// <summary>Gets or sets the objective function in its <see cref="OneDimEquationSolver.IFunction" /> representation.
            /// </summary>
            /// <value>The objective function.</value>
            public IFunction Function
            {
                get { return m_ObjectiveFunction; }
                set
                {
                    if (value is OneDimRootFinderFunction)
                    {
                        m_ObjectiveFunction = (OneDimRootFinderFunction)value;
                    }
                    else
                    {
                        throw new InvalidCastException("objectiveFunction");
                    }
                }
            }
            #endregion

            #endregion

            #region private properties

            /// <summary>Gets the abort (stopping) condition of the algorithm.
            /// </summary>
            /// <value>The abort (stopping) condition of the algorithm.</value>
            private BrentRootFinderAbortCondition AbortCondition
            {
                get { return m_RootFinder.AbortCondition; }
            }

            /// <summary>Gets the machine floating-point precision for the specified algorithm.
            /// </summary>
            /// <value>The machine floating-point precision for the specified algorithm.</value>
            private double Epsilon
            {
                get { return 3.0e-8; }
            }
            #endregion

            #region public methods

            #region IOneDimEquationSolverAlgorithm Members

            /// <summary>Finds a specific root of the specified <see cref="IOneDimEquationSolverAlgorithm.Function" />.
            /// </summary>
            /// <param name="x">An initial guess of the algorithm (if applicable).</param>
            /// <param name="root">The estimated root of the objective function (output).</param>
            /// <returns>The state of the algorithm, i.e. an indicating whether <paramref name="root" /> contains valid data.</returns>
            public State FindRoot(double x, out double root)
            {
                double a, fa, b, fb;
                int evaluationsNeeded;
                var bracketingResultState = m_RootFinder.Bracketing.TryGetBracketingTuple(m_ObjectiveFunction.GetValue, m_Constraint.IntervalRepresentation.Infimum, m_Constraint.IntervalRepresentation.Supremum, x, out a, out fa, out b, out fb, out evaluationsNeeded);

                switch (bracketingResultState)
                {
                    case RootBracketingResultState.FoundRoot:
                        root = a;
                        return State.Create(EquationSolverErrorClassification.ProperResult, root, fa, evaluationsNeeded);

                    case RootBracketingResultState.ProperResult:
                        return zbrent(m_ObjectiveFunction.GetValue, a, fa, b, fb, evaluationsNeeded, out root);

                    case RootBracketingResultState.NoBracketingTupleFound:
                        root = Double.NaN;
                        return State.Create(EquationSolverErrorClassification.NoResult, Double.NaN, Double.NaN, evaluationsNeeded);

                    case RootBracketingResultState.OutOfBounds:
                    case RootBracketingResultState.FlatBracketingTuple:
                        root = a;
                        return State.Create(EquationSolverErrorClassification.Unknown, root, fa, evaluationsNeeded);

                    default:
                        throw new NotImplementedException();
                }
            }
            #endregion

            #region IDisposable Members

            /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                // nothing to do here
            }
            #endregion

            /// <summary>Returns a <see cref="System.String" /> that represents this instance.
            /// </summary>
            /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
            public override string ToString()
            {
                return String.Format("{0}; constraint = {1}", Factory.Name.String, m_Constraint.IntervalRepresentation.ToString());
            }
            #endregion

            #region private methods

            /// <summary>Using Brent's method, find the root of a specific function known to lie between <paramref name="a"/> and <paramref name="b"/>.
            /// </summary>
            /// <param name="f">The objective function.</param>
            /// <param name="a">The first component of the bracketing tuple.</param>
            /// <param name="fa">The function value at <paramref name="a"/>.</param>
            /// <param name="b">The second component of the bracketing tuple.</param>
            /// <param name="fb">The function value at <paramref name="b"/>.</param>
            /// <param name="evaluationsNeeded">The number of function evaluations needed by the bracketing algorithm applied before.</param>
            /// <param name="root">The estimated root of the objective function (output).</param>
            /// <returns>The state of the algorithm, i.e. an indicating whether <paramref name="root" /> contains valid data.</returns>
            private State zbrent(Func<double, double> f, double a, double fa, double b, double fb, int evaluationsNeeded, out double root)
            {
                double c = b;
                var fc = fb;

                var e = 0.0;
                var d = 0.0;
                for (int n = 1; n <= AbortCondition.MaxIterations; n++)
                {
                    if ((fb > 0.0 && fc > 0.0) || (fb < 0.0 && fc < 0.0))
                    {
                        c = a; // rename a, b, c and adjust bounding interval d.
                        fc = fa;
                        e = d = b - a;
                    }

                    if (Math.Abs(fc) < Math.Abs(fb))
                    {
                        a = b;
                        b = c;
                        c = a;
                        fa = fb;
                        fb = fc;
                        fc = fa;
                    }
                    var tol1 = 2.0 * Epsilon * Math.Abs(b) + 0.5 * AbortCondition.Tolerance;
                    var xm = 0.5 * (c - b);
                    if (Math.Abs(xm) <= tol1 || fb == 0.0)
                    {
                        root = b;
                        return State.Create(EquationSolverErrorClassification.ProperResult, root, fb, evaluationsNeeded);
                    }

                    if (Math.Abs(e) >= tol1 && Math.Abs(fa) > Math.Abs(fb))
                    {
                        double p, q;
                        var s = fb / fa; // attempt inverse quadratic interpolation
                        if (a == c)
                        {
                            p = 2.0 * xm * s;
                            q = 1.0 - s;
                        }
                        else
                        {
                            q = fa / fc;
                            var r = fb / fc;
                            p = s * (2.0 * xm * q * (q - r) - (b - a) * (r - 1.0));
                            q = (q - 1.0) * (r - 1.0) * (s - 1.0);
                        }
                        if (p > 0.0) // check whether in bounds
                        {
                            q = -q;
                        }
                        p = Math.Abs(p);
                        var min1 = 3.0 * xm * q - Math.Abs(tol1 * q);
                        var min2 = Math.Abs(e * q);
                        if (2.0 * p < (min1 < min2 ? min1 : min2))
                        {
                            e = d;   // accept interpolation
                            d = p / q;
                        }
                        else
                        {
                            d = xm; // interpolation failed, use bisection
                            e = d;
                        }
                    }
                    else // bounds decreasing too slowly, use bisection
                    {
                        d = xm;
                        e = d;
                    }
                    a = b; // move last best guess to a
                    fa = fb;
                    if (Math.Abs(d) > tol1) // evaluate new trial root
                    {
                        b += d;
                    }
                    else
                    {
                        b += Sign(tol1, xm);
                    }
                    fb = f(b);
                }
                root = b;
                return State.Create(EquationSolverErrorClassification.IterationLimitExceeded, root, fb, evaluationsNeeded);
            }

            /// <summary>Gets the magnitude of <paramref name="a"/> times sign of <paramref name="b"/>.
            /// </summary>
            /// <param name="a">The parameter a.</param>
            /// <param name="b">The parameter b.</param>
            /// <returns>the magnitude of <paramref name="a"/> times sign of <paramref name="b"/>.</returns>
            private double Sign(double a, double b)
            {
                if (b >= 0)
                {
                    return Math.Abs(b);
                }
                return -Math.Abs(b);
            }
            #endregion
        }
    }
}