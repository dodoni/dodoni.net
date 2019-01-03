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

namespace Dodoni.MathLibrary.Optimizer.OneDimensional
{
    public partial class GoldenSectionSearchOptimizer
    {
        /// <summary>Represents the implementation of the algorithm.
        /// </summary>
        private class Algorithm : IOneDimOptimizerAlgorithm
        {
            #region private members

            /// <summary>The <see cref="GoldenSectionSearchOptimizer"/> object that serves as factory of the current object.
            /// </summary>
            private GoldenSectionSearchOptimizer m_Optimizer;

            /// <summary>The objective function in its <see cref="OneDimOptimizerFunction"/> representation.
            /// </summary>
            private OneDimOptimizerFunction m_ObjectiveFunction;

            /// <summary>The constraint in its <see cref="OneDimOptimizerConstraint"/> representation.
            /// </summary>
            private OneDimOptimizerConstraint m_Constraint;
            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="Algorithm"/> class.
            /// </summary>
            /// <param name="optimizer">The <see cref="GoldenSectionSearchOptimizer"/> object that serves as factory of the current object.</param>
            /// <param name="constraint">The constraint in its <see cref="OneDimOptimizerConstraint"/> representation.</param>
            internal Algorithm(GoldenSectionSearchOptimizer optimizer, OneDimOptimizerConstraint constraint)
            {
                m_Optimizer = optimizer;
                m_Constraint = constraint;
            }
            #endregion

            #region public properties

            #region IOneDimOptimizerAlgorithm Members

            /// <summary>Gets the factory for further <see cref="IOneDimOptimizerAlgorithm" /> objects of the same type, i.e. with the same stopping condition etc.
            /// </summary>
            /// <value>The factory for further <see cref="IOneDimOptimizerAlgorithm" /> objects of the same type.</value>
            public OneDimOptimizer Factory
            {
                get { return m_Optimizer; }
            }

            /// <summary>Gets or sets the objective function in its <see cref="OneDimOptimizer.IFunction" /> representation.
            /// </summary>
            /// <value>The objective function.</value>
            public IFunction Function
            {
                get { return m_ObjectiveFunction; }
                set
                {
                    if (value is OneDimOptimizerFunction)
                    {
                        m_ObjectiveFunction = (OneDimOptimizerFunction)value;
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
            private GoldenSectionSearchAbortCondition AbortCondition
            {
                get { return m_Optimizer.AbortCondition; }
            }
            #endregion

            #region public methods

            #region IOneDimOptimizerAlgorithm Members

            /// <summary>Finds the argmin of <see cref="IOneDimOptimizerAlgorithm.Function"/>.
            /// </summary>
            /// <param name="x">An initial guess of the algorithm (if applicable); on exit this argument contains the argmin.</param>
            /// <param name="argMin">The estimated argmin of the objective function (output).</param>
            /// <param name="minimum">The minimum, i.e. the function value with respect to <paramref name="argMin"/> which represents the argmin (output).</param>
            /// <returns>The state of the algorithm, i.e. an indicating whether <paramref name="argMin"/> and <paramref name="minimum"/> contains valid data.</returns>
            public OneDimOptimizer.State FindMinimum(double x, out double argMin, out double minimum)
            {
                if (m_Constraint.IntervalRepresentation.GetPointPosition(x) != PointRegionRelation.InsideOrBoundaryPoint)
                {
                    throw new ArgumentException("Initial point is not a feasible point.");
                }
                var bracketingResultState = m_Optimizer.BracketingApproach.TryGetBracketingTriple(m_ObjectiveFunction.GetValue, m_Constraint.IntervalRepresentation.Infimum, m_Constraint.IntervalRepresentation.Supremum, x,
                    out BracketingTriple triple, out double fa, out double fb, out double fc, out int evaluationsNeeded);

                switch (bracketingResultState)
                {
                    case MinimumBracketingResultState.ProperResult:
                        return FindMinimum(triple.A, triple.B, triple.C, evaluationsNeeded, out argMin, out minimum);

                    case MinimumBracketingResultState.FlatBracketingTriple:
                        argMin = triple.A;
                        minimum = fa;
                        return State.Create(OptimizerErrorClassification.ProperResult, argMin, minimum, evaluationsNeeded: evaluationsNeeded, iterationsNeeded: 1, details: InfoOutputProperty.Create("Bracketing Result State", bracketingResultState));

                    default:  // return the smallest function values but indicating that no Bracketing triple has been found
                        if (fa < fb)
                        {
                            if (fa < fc)  // fa=f(a) is smallest value
                            {
                                argMin = triple.A;
                                minimum = fa;
                            }
                            else  // fc =f(c) is smallest value
                            {
                                argMin = triple.C;
                                minimum = fc;
                            }
                        }
                        else // fb <= fa
                        {
                            if (fc < fb)
                            {
                                argMin = triple.C;
                                minimum = fc;
                            }
                            else
                            {
                                argMin = triple.B;
                                minimum = fb;
                            }
                        }
                        double estimatedAbsoluteError = Math.Max(Math.Abs(triple.B - triple.A), Math.Max(Math.Abs(triple.C - triple.A), Math.Abs(triple.B - triple.C)));
                        double estimatedRelativeError = estimatedAbsoluteError / (MachineConsts.Epsilon + Math.Abs(triple.A) + Math.Abs(triple.B) + Math.Abs(triple.C));

                        return State.Create(OptimizerErrorClassification.Unknown, argMin, minimum,
                        evaluationsNeeded, iterationsNeeded: 1,
                        details: new[]{ InfoOutputProperty.Create("Bracketing Result State", bracketingResultState),
                            InfoOutputProperty.Create("estimatedAbsoluteError", estimatedAbsoluteError),
                            InfoOutputProperty.Create("estimatedRelativeError",estimatedRelativeError)});
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

            /// <summary>Finds the minimum of the objective function. This implementation does not take into account a probably given derivative.
            /// </summary>
            /// <param name="a">The first point of bracket triple.</param>
            /// <param name="b">The second point of bracket triple.</param>
            /// <param name="c">The third point of bracket triple.</param>
            /// <param name="evaluationsNeeded">The number of function evaluations which are already needed.</param>
            /// <param name="argMin">The estimated argmin of the objective function (output).</param>
            /// <param name="minimum">The minimum, i.e. the function value with respect to <paramref name="argMin"/> which represents the argmin (output).</param>
            /// <returns>The state of the algorithm.</returns>
            /// <remarks>The implementation is based on Press, et al. (1992) "Numerical recipes in C", 2nd ed., p.401f.</remarks>
            private State FindMinimum(double a, double b, double c, int evaluationsNeeded, out double argMin, out double minimum)
            {
                double x1, x2;

                double x0 = a;
                double x3 = c;
                if (Math.Abs(c - b) > Math.Abs(b - a))
                {
                    x1 = b;
                    x2 = b + MathConsts.TwoMinusGoldenRatio * (c - b);
                }
                else
                {
                    x2 = b;
                    x1 = b - MathConsts.TwoMinusGoldenRatio * (b - a);
                }
                double f1 = m_ObjectiveFunction.GetValue(x1);
                double f2 = m_ObjectiveFunction.GetValue(x2);
                evaluationsNeeded += 2;

                var resultStatus = OptimizerErrorClassification.NoResult;
                int k = 1;
                while (k <= AbortCondition.MaxIterations)
                {
                    if (Math.Abs(x3 - x0) <= AbortCondition.Tolerance * (Math.Abs(x1) + Math.Abs(x2)))
                    {
                        resultStatus = OptimizerErrorClassification.ProperResult;
                    }
                    if (evaluationsNeeded >= m_Optimizer.AbortCondition.MaxEvaluations)
                    {
                        resultStatus = OptimizerErrorClassification.EvaluationLimitExceeded;
                    }
                    if (resultStatus != OptimizerErrorClassification.NoResult)
                    {
                        break;  // exit loop
                    }

                    /* do some iteration step */
                    if (f2 < f1)
                    {
                        x0 = x1;
                        x1 = x2;
                        x2 = MathConsts.GoldenRatioMinusOne * x1 + MathConsts.TwoMinusGoldenRatio * x3;

                        f1 = f2;
                        f2 = m_ObjectiveFunction.GetValue(x2);
                    }
                    else
                    {
                        x3 = x2;
                        x2 = x1;
                        x1 = MathConsts.GoldenRatioMinusOne * x2 + MathConsts.TwoMinusGoldenRatio * x0;

                        f2 = f1;
                        f1 = m_ObjectiveFunction.GetValue(x1);
                    }
                    evaluationsNeeded++;
                    k++;
                }

                /* prepare output and state: */
                if (f1 < f2)
                {
                    argMin = x1;
                    minimum = f1;
                }
                else
                {
                    argMin = x2;
                    minimum = f2;
                }
                if (k == AbortCondition.MaxIterations)
                {
                    resultStatus = OptimizerErrorClassification.IterationLimitExceeded;
                }
                return State.Create(resultStatus, argMin, minimum, evaluationsNeeded, k, InfoOutputProperty.Create("x0", x0), InfoOutputProperty.Create("x1", x1), InfoOutputProperty.Create("x2", x2), InfoOutputProperty.Create("f1", f1), InfoOutputProperty.Create("f2", f2));
            }
            #endregion
        }
    }
}