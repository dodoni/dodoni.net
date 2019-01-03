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
    public partial class BrentOptimizer
    {
        /// <summary>Represents the implementation of the algorithm.
        /// </summary>
        private class Algorithm : IOneDimOptimizerAlgorithm
        {
            #region private members

            /// <summary>The <see cref="BrentOptimizer"/> object that serves as factory of the current object.
            /// </summary>
            private BrentOptimizer m_Optimizer;

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
            /// <param name="optimizer">The <see cref="BrentOptimizer"/> object that serves as factory of the current object.</param>
            /// <param name="constraint">The constraint in its <see cref="OneDimOptimizerConstraint"/> representation.</param>
            internal Algorithm(BrentOptimizer optimizer, OneDimOptimizerConstraint constraint)
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
            private BrentOptimizerAbortCondition AbortCondition
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

                double fa, fb, fc;
                BracketingTriple triple;
                int evaluationsNeeded;
                var bracketingResultState = m_Optimizer.BracketingApproach.TryGetBracketingTriple(m_ObjectiveFunction.GetValue, m_Constraint.IntervalRepresentation.Infimum, m_Constraint.IntervalRepresentation.Supremum, x, out triple, out fa, out fb, out fc, out evaluationsNeeded);

                switch (bracketingResultState)
                {
                    case MinimumBracketingResultState.ProperResult:
                        if (m_ObjectiveFunction is OneDimOptimizerDifferentiableFunction)
                        {
                            return OptimizeWithDerivative(triple.A, triple.B, triple.C, evaluationsNeeded, out argMin, out minimum);
                        }
                        return OptimizeWithoutDerivative(triple.A, triple.B, triple.C, evaluationsNeeded, out argMin, out minimum);

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
                            InfoOutputProperty.Create("estimatedRelativeError", estimatedRelativeError)});
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
            /// <remarks>The implementation is based on Press, et al. (1992) "Numerical recipes in C", 2nd ed., p.402ff.</remarks>
            private State OptimizeWithoutDerivative(double a, double b, double c, int evaluationsNeeded, out double argMin, out double minimum)
            {
                double tolerance, twoTimesTolerance;
                double d = 0.0, e = 0.0;
                double xm = 0.0, fb, fu, fv, fw, fx, u, v, w, x;

                double aTemp = a < c ? a : c;
                double bTemp = a > c ? a : c;
                x = w = v = b;
                fw = fv = fx = fb = m_ObjectiveFunction.GetValue(b);

                var resultStatus = OptimizerErrorClassification.NoResult;
                for (int k = 1; k <= AbortCondition.MaxIterations; k++)
                {
                    xm = 0.5 * (aTemp + bTemp);

                    tolerance = AbortCondition.Tolerance * Math.Abs(x) + MachineConsts.ExtremeTinyEpsilon;
                    twoTimesTolerance = 2.0 * tolerance;

                    if (Math.Abs(x - xm) <= (twoTimesTolerance - 0.5 * (bTemp - aTemp)))
                    {
                        resultStatus = OptimizerErrorClassification.ProperResult;
                    }
                    if (evaluationsNeeded >= AbortCondition.MaxEvaluations)
                    {
                        resultStatus = OptimizerErrorClassification.EvaluationLimitExceeded;
                    }

                    if (resultStatus != OptimizerErrorClassification.NoResult)
                    {
                        argMin = x;
                        minimum = fx;
                        return State.Create(resultStatus, argMin, minimum, evaluationsNeeded, k,
                            InfoOutputProperty.Create("x", x), InfoOutputProperty.Create("fx", fx),
                            InfoOutputProperty.Create("w", w), InfoOutputProperty.Create("fw", fw),
                            InfoOutputProperty.Create("v", v), InfoOutputProperty.Create("fv", fv),
                            InfoOutputProperty.Create("b", b), InfoOutputProperty.Create("fb", fb));
                    }

                    /* do some iteration step */
                    if (Math.Abs(e) > tolerance)
                    {
                        double r = (x - w) * (fx - fv);
                        double q = (x - v) * (fx - fw);
                        double p = (x - v) * q - (x - w) * r;

                        q = 2.0 * (q - r);
                        if (q > 0.0)
                        {
                            p = -p;
                        }
                        q = Math.Abs(q);
                        double etemp = e;
                        e = d;
                        if (Math.Abs(p) >= Math.Abs(0.5 * q * etemp) || p <= q * (aTemp - x) || p >= q * (bTemp - x))
                        {
                            e = x >= xm ? aTemp - x : bTemp - x;
                            d = MathConsts.TwoMinusGoldenRatio * e;
                        }
                        else
                        {
                            d = p / q;
                            u = x + d;
                            if (u - aTemp < twoTimesTolerance || bTemp - u < twoTimesTolerance)
                            {
                                d = (xm - x >= 0) ? tolerance : -tolerance;
                            }
                        }
                    }
                    else
                    {
                        e = x >= xm ? aTemp - x : bTemp - x;
                        d = MathConsts.TwoMinusGoldenRatio * e;
                    }
                    u = Math.Abs(d) >= tolerance ? x + d : x + ((d >= 0) ? tolerance : -tolerance);
                    fu = m_ObjectiveFunction.GetValue(u);
                    evaluationsNeeded++;
                    if (fu <= fx)
                    {
                        if (u >= x)
                        {
                            aTemp = x;
                        }
                        else
                        {
                            bTemp = x;
                        }

                        v = w;
                        w = x;
                        x = u;

                        fv = fw;
                        fw = fx;
                        fx = fu;
                    }
                    else
                    {
                        if (u < x)
                        {
                            aTemp = u;
                        }
                        else
                        {
                            bTemp = u;
                        }
                        if (fu <= fw || w == x)
                        {
                            v = w;
                            w = u;
                            fv = fw;
                            fw = fu;
                        }
                        else if (fu <= fv || v == x || v == w)
                        {
                            v = u;
                            fv = fu;
                        }
                    }
                }
                argMin = x;
                minimum = fx;
                return State.Create(OptimizerErrorClassification.IterationLimitExceeded, argMin, minimum, evaluationsNeeded, AbortCondition.MaxIterations,
                    InfoOutputProperty.Create("x", x), InfoOutputProperty.Create("fx", fx),
                    InfoOutputProperty.Create("w", w), InfoOutputProperty.Create("fw", fw),
                    InfoOutputProperty.Create("v", v), InfoOutputProperty.Create("fv", fv),
                    InfoOutputProperty.Create("b", b), InfoOutputProperty.Create("fb", fb));
            }

            /// <summary>Finds the minimum of the objective function. This implementation does take into account the specified derivative.
            /// </summary>
            /// <param name="a">The first point of bracket triple.</param>
            /// <param name="b">The second point of bracket triple.</param>
            /// <param name="c">The third point of bracket triple.</param>
            /// <param name="evaluationsNeeded">The number of function evaluations which are already needed.</param>
            /// <param name="argMin">The estimated argmin of the objective function (output).</param>
            /// <param name="minimum">The minimum, i.e. the function value with respect to <paramref name="argMin"/> which represents the argmin (output).</param>
            /// <returns>The state of the algorithm.</returns>
            /// <remarks>The implementation is based on Press, et al. (1992) "Numerical recipes in C", 2nd ed., p.402ff.</remarks>
            private State OptimizeWithDerivative(double a, double b, double c, int evaluationsNeeded, out double argMin, out double minimum)
            {
                var objectiveFunction = (OneDimOptimizerDifferentiableFunction)m_ObjectiveFunction;
                double tolerance, twoTimesTolerance;
                double d = 0.0, e = 0.0;
                double xm = 0.0, fb, fu, fv, fw, fx, u, v, w, x;

                double dw, dv;

                double aTemp = a < c ? a : c;
                double bTemp = a > c ? a : c;
                x = w = v = b;
                fw = fv = fx = fb = objectiveFunction.GetValue(b, out double dx);
                dw = dv = dx;

                var resultStatus = OptimizerErrorClassification.NoResult;
                for (int k = 1; k <= AbortCondition.MaxIterations; k++)
                {
                    xm = 0.5 * (aTemp + bTemp);

                    tolerance = AbortCondition.Tolerance * Math.Abs(x) + MachineConsts.ExtremeTinyEpsilon;
                    twoTimesTolerance = 2.0 * tolerance;

                    if (Math.Abs(x - xm) <= (twoTimesTolerance - 0.5 * (bTemp - aTemp)))
                    {
                        resultStatus = OptimizerErrorClassification.ProperResult;
                    }
                    if (evaluationsNeeded >= AbortCondition.MaxEvaluations)
                    {
                        resultStatus = OptimizerErrorClassification.EvaluationLimitExceeded;
                    }

                    if (resultStatus != OptimizerErrorClassification.NoResult)
                    {
                        argMin = x;
                        minimum = fx;
                        return State.Create(resultStatus, argMin, minimum, evaluationsNeeded, k,
                            InfoOutputProperty.Create("x", x), InfoOutputProperty.Create("fx", fx),
                            InfoOutputProperty.Create("w", w), InfoOutputProperty.Create("fw", fw),
                            InfoOutputProperty.Create("v", v), InfoOutputProperty.Create("fv", fv),
                            InfoOutputProperty.Create("b", b), InfoOutputProperty.Create("fb", fb));
                    }

                    /* do some iteration step */
                    if (Math.Abs(e) > tolerance)
                    {
                        /* maybe there is no derivative given because of the given constraints: */
                        if (!Double.IsNaN(dw) && !Double.IsNaN(dv) && !Double.IsNaN(dx))
                        {
                            double d1 = 2.0 * (bTemp - aTemp);
                            double d2 = d1;

                            if (dw != dx)
                            {
                                d1 = (w - x) * dx / (dx - dw);
                            }
                            if (dv != dx)
                            {
                                d2 = (v - x) * dx / (dx - dv);
                            }
                            double u1 = x + d1;
                            double u2 = x + d2;

                            bool ok1 = (aTemp - u1) * (u1 - bTemp) > 0.0 && dx * d1 <= 0.0;
                            bool ok2 = (aTemp - u2) * (u2 - bTemp) > 0.0 && dx * d2 <= 0.0;

                            double olde = e;
                            e = d;
                            if (ok1 || ok2)
                            {
                                if (ok1 && ok2)
                                    d = (Math.Abs(d1) < Math.Abs(d2) ? d1 : d2);
                                else if (ok1)
                                    d = d1;
                                else
                                    d = d2;
                                if (Math.Abs(d) <= Math.Abs(0.5 * olde))
                                {
                                    u = x + d;
                                    if (u - aTemp < twoTimesTolerance || bTemp - u < twoTimesTolerance)
                                        d = (xm - x >= 0.0) ? tolerance : -tolerance;
                                }
                                else
                                {
                                    e = dx >= 0.0 ? aTemp - x : bTemp - x;
                                    d = 0.5 * e;
                                }
                            }
                            else
                            {
                                e = dx >= 0.0 ? aTemp - x : bTemp - x;
                                d = 0.5 * e;
                            }
                        }
                        else
                        {
                            double r = (x - w) * (fx - fv);
                            double q = (x - v) * (fx - fw);
                            double p = (x - v) * q - (x - w) * r;

                            q = 2.0 * (q - r);
                            if (q > 0.0)
                            {
                                p = -p;
                            }
                            q = Math.Abs(q);
                            double etemp = e;
                            e = d;
                            if (Math.Abs(p) >= Math.Abs(0.5 * q * etemp) || p <= q * (aTemp - x) || p >= q * (bTemp - x))
                            {
                                e = x >= xm ? aTemp - x : bTemp - x;
                                d = MathConsts.TwoMinusGoldenRatio * e;
                            }
                            else
                            {
                                d = p / q;
                                u = x + d;
                                if (u - aTemp < twoTimesTolerance || bTemp - u < twoTimesTolerance)
                                {
                                    d = (xm - x >= 0) ? tolerance : -tolerance;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Double.IsNaN(dx))
                        {
                            e = x >= xm ? aTemp - x : bTemp - x;
                            d = MathConsts.TwoMinusGoldenRatio * e;
                        }
                        else
                        {
                            e = dx >= 0.0 ? aTemp - x : bTemp - x;
                            d = 0.5 * e;
                        }
                    }

                    double du;

                    if (Math.Abs(d) >= tolerance)
                    {
                        u = x + d;
                        fu = objectiveFunction.GetValue(u, out du);
                        evaluationsNeeded++;
                    }
                    else
                    {
                        u = x + (d >= 0.0 ? tolerance : -tolerance);
                        fu = objectiveFunction.GetValue(u, out du);
                        evaluationsNeeded++;

                        if (fu > fx)
                        {
                            argMin = x;
                            minimum = fx;

                            return State.Create(OptimizerErrorClassification.ProperResult, argMin, minimum, evaluationsNeeded, k,
                                InfoOutputProperty.Create("x", x), InfoOutputProperty.Create("fx", fx),
                                InfoOutputProperty.Create("w", w), InfoOutputProperty.Create("fw", fw),
                                InfoOutputProperty.Create("v", v), InfoOutputProperty.Create("fv", fv),
                                InfoOutputProperty.Create("b", b), InfoOutputProperty.Create("fb", fb));
                        }
                    }

                    if (fu <= fx)
                    {
                        if (u >= x)
                        {
                            aTemp = x;
                        }
                        else
                        {
                            bTemp = x;
                        }
                        Copy(ref v, ref fv, ref dv, w, fw, dw);
                        Copy(ref w, ref fw, ref dw, x, fx, dx);
                        Copy(ref x, ref fx, ref dx, u, fu, du);
                    }
                    else
                    {
                        if (u < x)
                        {
                            aTemp = u;
                        }
                        else
                        {
                            bTemp = u;
                        }
                        if (fu <= fw || w == x)
                        {
                            Copy(ref v, ref fv, ref dv, w, fw, dw);
                            Copy(ref w, ref fw, ref dw, u, fu, du);
                        }
                        else if (fu < fv || v == x || v == w)
                        {
                            Copy(ref v, ref fv, ref dv, u, fu, du);
                        }
                    }
                }
                argMin = x;
                minimum = fx;

                return State.Create(OptimizerErrorClassification.IterationLimitExceeded, argMin, minimum, evaluationsNeeded, AbortCondition.MaxIterations,
                    InfoOutputProperty.Create("x", x), InfoOutputProperty.Create("fx", fx),
                    InfoOutputProperty.Create("w", w), InfoOutputProperty.Create("fw", fw),
                    InfoOutputProperty.Create("v", v), InfoOutputProperty.Create("fv", fv),
                    InfoOutputProperty.Create("b", b), InfoOutputProperty.Create("fb", fb));
            }

            /// <summary>Swaps <see cref="System.Double"/> instances.
            /// </summary>
            /// <param name="a">Will be set to <paramref name="d"/>.</param>
            /// <param name="b">Will be set to <paramref name="e"/>.</param>
            /// <param name="c">Will be set to <paramref name="f"/>.</param>
            /// <param name="d">The parameter d.</param>
            /// <param name="e">The parameter e.</param>
            /// <param name="f">The parameter f.</param>
            private void Copy(ref double a, ref double b, ref double c, double d, double e, double f)
            {
                a = d;
                b = e;
                c = f;
            }
            #endregion
        }
    }
}