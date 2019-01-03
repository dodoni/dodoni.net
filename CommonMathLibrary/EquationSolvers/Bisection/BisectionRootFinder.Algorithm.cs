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
    public partial class BisectionRootFinder
    {
        /// <summary>The implementation of Bisection root finding algorithm.
        /// </summary>
        private class Algorithm : IOneDimEquationSolverAlgorithm
        {
            #region private members

            /// <summary>The <see cref="BisectionRootFinder"/> object that serves as factory of the current object.
            /// </summary>
            private BisectionRootFinder m_RootFinder;

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
            public Algorithm(BisectionRootFinder rootFinder, OneDimRootFinderConstraint constraint)
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
            private BisectionRootFinderAbortCondition AbortCondition
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
                        throw new NotImplementedException();
                        //return zbrent(m_ObjectiveFunction.GetValue, a, fa, b, fb, evaluationsNeeded, out root);

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


            public double rtbis(Func<double, double> func, double x1, double x2, double xacc, int evaluationsNeeded)
            {
                double dx, f, fmid, xmid, rtb;
                f = func(x1);
                fmid = func(x2);
              
                if (f < 0.0)  // Orient the search so that f > 0
                {
                    dx = x2 - x1;
                    rtb = x1;
                }
                else
                {
                    dx = x1 - x2;
                    rtb = x2;
                }

                for (int j = 0; j < AbortCondition.MaxIterations; j++) // lies at x+dx.
                {
                    fmid = func(xmid = rtb + (dx *= 0.5));  // Bisection loop
                    if (fmid <= 0.0) rtb = xmid;
                    if (Math.Abs(dx) < xacc || fmid == 0.0)
                    {
                        return rtb;
                    }
                }
                throw new Exception("Too many bisections in rtbis");
            }
            #endregion
        }
    }
}