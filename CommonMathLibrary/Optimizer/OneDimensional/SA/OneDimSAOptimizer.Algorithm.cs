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
using System.Linq;

using Dodoni.MathLibrary.Miscellaneous;
using Dodoni.MathLibrary.ProbabilityTheory.MonteCarloEngine;

namespace Dodoni.MathLibrary.Optimizer.OneDimensional
{
    public partial class OneDimSAOptimizer
    {
        /// <summary>Represents the implementation of the algorithm.
        /// </summary>
        private class Algorithm : IOneDimOptimizerAlgorithm
        {
            #region private members

            /// <summary>The <see cref="OneDimSAOptimizer"/> object that serves as factory of the current object.
            /// </summary>
            private OneDimSAOptimizer m_Optimizer;

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
            /// <param name="optimizer">The <see cref="OneDimSAOptimizer"/> object that serves as factory of the current object.</param>
            /// <param name="constraint">The constraint in its <see cref="OneDimOptimizerConstraint"/> representation.</param>
            internal Algorithm(OneDimSAOptimizer optimizer, OneDimOptimizerConstraint constraint)
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
            private OneDimSAOptimizerAbortCondition AbortCondition
            {
                get { return m_Optimizer.AbortCondition; }
            }

            /// <summary>Gets the configuration of the Simulated Annealing algorithm in its <see cref="OneDimSAOptimizerConfiguration"/> representation.
            /// </summary>
            /// <value>The configuration of the Simulated Annealing algorithm in its <see cref="OneDimSAOptimizerConfiguration"/> representation.</value>
            private OneDimSAOptimizerConfiguration Configuration
            {
                get { return m_Optimizer.Configuration; }
            }

            /// <summary>Gets the (single) random number stream.
            /// </summary>
            /// <value>The (single) random number stream.</value>
            private SingleRandomNumberStream SingleRandomNumberStream
            {
                get { return m_Optimizer.m_SingleRandomNumberStream; }
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
                var lowerBound = m_Constraint.IntervalRepresentation.Infimum;
                var upperBound = m_Constraint.IntervalRepresentation.Supremum;
                var constraintDiameter = upperBound - lowerBound;

                if (m_Constraint.IntervalRepresentation.GetPointPosition(x) != PointRegionRelation.InsideOrBoundaryPoint)
                {
                    x = lowerBound + constraintDiameter * SingleRandomNumberStream.GetNextDouble();
                }
                var y = m_ObjectiveFunction.GetValue(x);
                int evaluationsNeeded = 1;

                minimum = Double.MaxValue;
                argMin = x;
                var xLastAccepted = x;
                var yLastAccepted = y;

                var yStar = Enumerable.Repeat(21.0, AbortCondition.RequiredNumberOfAcceptedPoints).ToArray(); // todo: initialized with 21.0 ??   
                yStar[0] = yLastAccepted;
                int yStarIndex = 0;

                int numberOfAcceptedPoint = 0;
                var stepLength = Configuration.InitialStepLength;
                var temperature = Configuration.InitialTemperature;

                for (int iteration = 1; iteration <= AbortCondition.MaxIterations; iteration++)
                {
                    for (int k = 1; k <= AbortCondition.NumberOfIterationsBeforeTemperatureReduction; k++)
                    {
                        for (int j = 1; j <= AbortCondition.NumberOfCyles; j++)
                        {
                            /* 1.) generate a new point and ensure that the point is inside the constraint: */
                            if (Configuration.PointGenerationRule == OneDimSAOptimizerConfiguration.GenerationRule.UseWholeDomain)
                            {
                                x = xLastAccepted + stepLength * (2.0 * SingleRandomNumberStream.GetNextDouble() - 1.0);
                            }
                            else
                            {
                                var sign = SingleRandomNumberStream.GetNextDouble() >= 0.5 ? 1 : -1;
                                if (sign < 0)
                                {
                                    var leftPoint = DoMath.Max(lowerBound, xLastAccepted - stepLength);
                                    x = leftPoint + (xLastAccepted - leftPoint) * SingleRandomNumberStream.GetNextDouble();
                                }
                                else
                                {
                                    var rightPoint = DoMath.Min(upperBound, xLastAccepted + stepLength);
                                    x = xLastAccepted + (rightPoint - xLastAccepted) * SingleRandomNumberStream.GetNextDouble();
                                }
                            }
                            if ((x < lowerBound) || (x > upperBound))
                            {
                                x = lowerBound + constraintDiameter * SingleRandomNumberStream.GetNextDouble();
                            }
                            y = m_ObjectiveFunction.GetValue(x);
                            evaluationsNeeded++;

                            if (evaluationsNeeded > AbortCondition.MaxEvaluations)
                            {
                                return State.Create(OptimizerErrorClassification.EvaluationLimitExceeded, argMin, minimum, evaluationsNeeded, 1);
                            }

                            /* 2.) check if this point is 'better' than the point before (accept point) */
                            if (y < yLastAccepted)
                            {
                                xLastAccepted = x;
                                yLastAccepted = y;
                                numberOfAcceptedPoint++;

                                yStarIndex++;
                                if (yStarIndex >= AbortCondition.RequiredNumberOfAcceptedPoints)
                                {
                                    yStarIndex = 0;
                                }
                                yStar[yStarIndex] = y;

                                if (y < minimum)  /* maybe it is also better than the minimum founded before */
                                {
                                    argMin = x;
                                    minimum = y;
                                }
                            }
                            else  /* use metropolis criterion */
                            {
                                var p = Math.Exp(-(y - yLastAccepted) / temperature);
                                if (p > SingleRandomNumberStream.GetNextDouble()) // accept this value as new point
                                {
                                    xLastAccepted = x;
                                    yLastAccepted = y;
                                    numberOfAcceptedPoint++;

                                    yStarIndex++;
                                    if (yStarIndex >= AbortCondition.RequiredNumberOfAcceptedPoints)
                                    {
                                        yStarIndex = 0;
                                    }
                                    yStar[yStarIndex] = y;
                                }
                            }
                        }

                        /* 3.) adjust step length so that approximately half of all evaluations are accepted */
                        var acceptanceRate = numberOfAcceptedPoint / ((double)AbortCondition.NumberOfCyles);
                        if (acceptanceRate > 0.6)
                        {
                            stepLength = stepLength * (1.0 + Configuration.StepLengthControlFactor * (acceptanceRate - 0.6) / 0.4);
                        }
                        else if (acceptanceRate < 0.4)
                        {
                            stepLength = stepLength / (1.0 + Configuration.StepLengthControlFactor * ((0.4 - acceptanceRate) / 0.4));
                        }
                        if (stepLength > constraintDiameter)
                        {
                            stepLength = constraintDiameter;
                        }
                        numberOfAcceptedPoint = 0;
                    }
                    if (AbortCondition.IsSatisfied(minimum, y, yLastAccepted, yStar, yStarIndex) == true)
                    {
                        return State.Create(OptimizerErrorClassification.ProperResult, argMin, minimum, evaluationsNeeded, iteration);
                    }

                    /* prepare for another loop */
                    temperature *= Configuration.CoolDownFactor;
                    yLastAccepted = minimum;
                    xLastAccepted = argMin;
                }
                return State.Create(OptimizerErrorClassification.IterationLimitExceeded, argMin, minimum, evaluationsNeeded, AbortCondition.MaxIterations);
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
        }
    }
}