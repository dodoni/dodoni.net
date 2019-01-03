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
using System.Collections.Generic;

using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.NumericalIntegrators
{
    public partial class ClosedNewtonCotesFormula
    {
        /// <summary>Represents the implementation of the algorithm.
        /// </summary>
        internal class Algorithm : IOneDimNumericalIntegratorAlgorithm
        {
            #region private static members

            /// <summary>The Newton-Cotes weights.
            /// </summary>
            private static Dictionary<Rule, double[]> sm_NewtonCotesWeights
                = new Dictionary<Rule, double[]>(){
                        {Rule.Trapezoid, new double[] { 1, 1 }},
                        {Rule.Simpson, new double[] { 1, 4, 1 }},
                        {Rule.SimpsonThreeEight, new double[] { 1, 3, 3, 1 }},
                        {Rule.MilneBools, new double[] { 7, 32, 12, 32, 7 }},
                        {Rule.DegreeFive, new double[] { 19, 75, 50, 50, 75, 19 }},
                        {Rule.Weddle, new double[] { 41, 216, 27, 272, 27, 216, 41 }}};
            #endregion

            #region private members

            /// <summary>The <see cref="ClosedNewtonCotesFormula"/> object which serves as factory for the current object.
            /// </summary>
            private ClosedNewtonCotesFormula m_IntegratorFactory;

            /// <summary>The lower integration bound.
            /// </summary>
            private double m_LowerBound = Double.NaN;

            /// <summary>The upper integration bound.
            /// </summary>
            private double m_UpperBound = Double.NaN;

            /// <summary>The initial number of equidistant subintervals of the interval [a,b], where a is the lower and b is the upper border of the integration.
            /// </summary>
            private const int m_InitialNumberOfSubIntervals = 1;
            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="Algorithm"/> class.
            /// </summary>
            /// <param name="newtonCotesIntegrator">The <see cref="ClosedNewtonCotesFormula"/> object which serves as factory for the current object.</param>
            internal Algorithm(ClosedNewtonCotesFormula newtonCotesIntegrator)
            {
                m_IntegratorFactory = newtonCotesIntegrator;
            }
            #endregion

            #region public properties

            #region IOperable Members

            /// <summary>Gets a value indicating whether this instance is operable.
            /// </summary>
            /// <value>
            /// 	<c>true</c> if this instance is operable; otherwise, <c>false</c>.
            /// </value>
            public bool IsOperable
            {
                get { return ((FunctionToIntegrate != null) && (Double.IsNaN(m_LowerBound) == false) && (Double.IsNaN(m_UpperBound) == false)); }
            }
            #endregion

            #region IInfoOutputQueriable Members

            /// <summary>Gets the info-output level of detail.
            /// </summary>
            /// <value>The info-output level of detail.</value>
            public InfoOutputDetailLevel InfoOutputDetailLevel
            {
                get { return InfoOutputDetailLevel.Full; }
            }
            #endregion

            #region IOneDimNumericalIntegratorAlgorithm Members

            /// <summary>Gets the factory for further <see cref="IOneDimNumericalIntegratorAlgorithm"/> objects of the same type and with the same configuration.
            /// </summary>
            /// <value>The factory for further <see cref="IOneDimNumericalIntegratorAlgorithm"/> objects of the same type and with the same configuration.
            /// </value>
            public OneDimNumericalIntegrator Factory
            {
                get { return m_IntegratorFactory; }
            }

            /// <summary>Gets or sets the function to integrate.
            /// </summary>
            /// <value>The function to integrate.</value>
            public Func<double, double> FunctionToIntegrate
            {
                get;
                set;
            }

            /// <summary>Gets the weight function w(x) of the numerical integration 
            ///  <para>
            /// \int_a^b w(x) * f(x) dx,
            /// </para>
            /// where f:I \to \R, I: Interval with a = inf I, b = sup I, is the function to integrate and w(x) is a specific strictly positive weight function.
            /// </summary>
            /// <value>The weight function.</value>
            public OneDimNumericalIntegrator.WeightFunction WeightFunction
            {
                get { return OneDimNumericalIntegrator.WeightFunction.One; }
            }

            /// <summary>Gets the lower integration bound.
            /// </summary>
            /// <value>The lower integration bound.</value>
            public double LowerBound
            {
                get { return m_LowerBound; }
            }

            /// <summary>Gets the upper integration bound.
            /// </summary>
            /// <value>The upper integration bound.</value>
            public double UpperBound
            {
                get { return m_UpperBound; }
            }
            #endregion

            #endregion

            #region public methods

            #region IOneDimNumericalIntegratorAlgorithm Members

            /// <summary>Gets the value of the integral \int_a^b w(x) * f(x) dx.
            /// </summary>
            /// <returns>An approximation of the specific integral.</returns>
            public double GetValue()
            {
                int n = (int)m_IntegratorFactory.NewtonCotesType;
                int ns = 0;
                switch (m_IntegratorFactory.NewtonCotesType)
                {
                    case Rule.Trapezoid:
                        ns = 2;
                        break;
                    case Rule.Simpson:
                        ns = 6;
                        break;
                    case Rule.SimpsonThreeEight:
                        ns = 8;
                        break;
                    case Rule.MilneBools:
                        ns = 90;
                        break;
                    case Rule.DegreeFive:
                        ns = 288;
                        break;
                    case Rule.Weddle:
                        ns = 840;
                        break;
                    default:
                        throw new NotImplementedException(String.Format("The Newton-Cotes integration method {0} is not supported yet.", m_IntegratorFactory.NewtonCotesType));
                }
                if (n == 0)
                {
                    throw new InvalidCastException(String.Format("Invalid Newton-Cotes type: {0} of order '0' which is not allowed! Did you change 'NewtonCotesType'?", m_IntegratorFactory.NewtonCotesType));
                }
                var newtonCotesWeights = sm_NewtonCotesWeights[m_IntegratorFactory.NewtonCotesType];


                /* split the interval in equidistant subintervals and increase the number of subintervals by factor two in each step: */
                var value = 0.0;
                var tempValue = 0.0;
                var numberOfSubIntervals = m_InitialNumberOfSubIntervals;

                for (int k = 1; k <= m_IntegratorFactory.ExitCondition.MaxIterations; k++)
                {
                    tempValue = value;

                    value = 0.0;
                    var subIntervalLenght = (m_UpperBound - m_LowerBound) / numberOfSubIntervals;
                    var normedSubIntervalLength = subIntervalLenght / n;

                    for (int j = 0; j <= numberOfSubIntervals - 1; j++)
                    {
                        for (int i = 0; i <= n; i++)
                        {
                            value += FunctionToIntegrate(m_LowerBound + subIntervalLenght * (j + i / (double)n)) * newtonCotesWeights[i];
                        }
                    }
                    value = value * subIntervalLenght / ns;
                    if (m_IntegratorFactory.ExitCondition.IsFulfilled(tempValue, value) == true)
                    {
                        return value;
                    }

                    /* calculate the number of function evaluations: */
                    if (m_IntegratorFactory.ExitCondition.MaxEvaluations != Int32.MaxValue)
                    {
                        long numberOfEvaluations = (long)(n * (k * (k + 1) / 2.0 - (m_InitialNumberOfSubIntervals - 1) * m_InitialNumberOfSubIntervals / 2.0));
                        if (numberOfEvaluations > m_IntegratorFactory.ExitCondition.MaxEvaluations)
                        {
                            return value;
                        }
                    }
                    numberOfSubIntervals++;
                }
                return value;
            }

            /// <summary>Gets the value of the integral \int_a^b w(x) * f(x) dx.
            /// </summary>
            /// <param name="value">An approximation of the specific integral.</param>
            /// <returns>The state of the algorithm, i.e. an indicating whether <paramref name="value"/> contains valid data.
            /// </returns>
            public State GetValue(out double value)
            {
                int n = (int)m_IntegratorFactory.NewtonCotesType;
                int ns = 0;
                switch (m_IntegratorFactory.NewtonCotesType)
                {
                    case Rule.Trapezoid:
                        ns = 2;
                        break;
                    case Rule.Simpson:
                        ns = 6;
                        break;
                    case Rule.SimpsonThreeEight:
                        ns = 8;
                        break;
                    case Rule.MilneBools:
                        ns = 90;
                        break;
                    case Rule.DegreeFive:
                        ns = 288;
                        break;
                    case Rule.Weddle:
                        ns = 840;
                        break;
                    default:
                        throw new NotImplementedException(String.Format("The Newton-Cotes integration method {0} is not supported yet.", m_IntegratorFactory.NewtonCotesType));
                }
                if (n == 0)
                {
                    throw new InvalidCastException(String.Format("Invalid Newton-Cotes type: {0} of order '0' which is not allowed! Did you change 'eNewtonCotesType'?", m_IntegratorFactory.NewtonCotesType));
                }
                var newtonCotesWeights = sm_NewtonCotesWeights[m_IntegratorFactory.NewtonCotesType];

                /* split the interval in equidistant subintervals and increase the number of subintervals by factor two in each step: */
                value = 0.0;
                var tempValue = 0.0;
                int numberOfSubIntervals = m_InitialNumberOfSubIntervals;

                for (int k = 1; k <= m_IntegratorFactory.ExitCondition.MaxIterations; k++)
                {
                    tempValue = value;

                    value = 0.0;
                    var subIntervalLenght = (m_UpperBound - m_LowerBound) / numberOfSubIntervals;
                    var normedSubIntervalLength = subIntervalLenght / n;

                    for (int j = 0; j <= numberOfSubIntervals - 1; j++)
                    {
                        for (int i = 0; i <= n; i++)
                        {
                            value += FunctionToIntegrate(m_LowerBound + subIntervalLenght * (j + i / (double)n)) * newtonCotesWeights[i];
                        }
                    }
                    value = value * subIntervalLenght / ns;

                    if (m_IntegratorFactory.ExitCondition.IsFulfilled(tempValue, value) == true)
                    {
                        return State.Create(NumericalIntegratorErrorClassification.ProperResult, tempValue, value, k);
                    }

                    /* calculate the number of function evaluations: */
                    if (m_IntegratorFactory.ExitCondition.MaxEvaluations != Int32.MaxValue)
                    {
                        long numberOfEvaluations = (long)(n * (k * (k + 1) / 2.0 - (m_InitialNumberOfSubIntervals - 1) * m_InitialNumberOfSubIntervals / 2.0));
                        if (numberOfEvaluations > m_IntegratorFactory.ExitCondition.MaxEvaluations)
                        {
                            return State.Create(NumericalIntegratorErrorClassification.EvaluationLimitExceeded, tempValue, value, k, numberOfEvaluations);
                        }
                    }
                    numberOfSubIntervals++;
                }
                return State.Create(NumericalIntegratorErrorClassification.IterationLimitExceeded, tempValue, value, m_IntegratorFactory.ExitCondition.MaxIterations);
            }

            /// <summary>Sets the lower integration bound.
            /// </summary>
            /// <param name="lowerBound">The lower integration bound.</param>
            /// <returns>A value indicating whether <see cref="IOneDimNumericalIntegratorAlgorithm.LowerBound" /> has been set to <paramref name="lowerBound" />.</returns>
            public bool TrySetLowerBound(double lowerBound)
            {
                if ((Double.IsNaN(lowerBound) == true) || (Double.IsInfinity(lowerBound) == true))
                {
                    return false;
                }
                m_LowerBound = lowerBound;
                return true;
            }

            /// <summary>Sets the upper integration bound.
            /// </summary>
            /// <param name="upperBound">The upper integration bound.</param>
            /// <returns>A value indicating whether <see cref="IOneDimNumericalIntegratorAlgorithm.UpperBound" /> has been set to <paramref name="upperBound" />.</returns>
            public bool TrySetUpperBound(double upperBound)
            {
                if ((Double.IsNaN(upperBound) == true) || (Double.IsInfinity(upperBound) == true))
                {
                    return false;
                }
                m_UpperBound = upperBound;
                return true;
            }

            /// <summary>Sets the lower and upper integration bounds.
            /// </summary>
            /// <param name="lowerBound">The lower integration bound.</param>
            /// <param name="upperBound">The upper integration bound.</param>
            /// <returns>A value indicating whether <see cref="IOneDimNumericalIntegratorAlgorithm.LowerBound" /> and <see cref="IOneDimNumericalIntegratorAlgorithm.UpperBound" /> have been changed.</returns>
            public bool TrySetBounds(double lowerBound, double upperBound)
            {
                if ((Double.IsNaN(lowerBound) == true) || (Double.IsInfinity(lowerBound) == true))
                {
                    return false;
                }
                if ((Double.IsNaN(upperBound) == true) || (Double.IsInfinity(upperBound) == true))
                {
                    return false;
                }
                m_LowerBound = lowerBound;
                m_UpperBound = upperBound;
                return true;
            }
            #endregion

            #region IInfoOutputQueriable Members

            /// <summary>Gets informations of the current object as a specific <see cref="T:Dodoni.BasicComponents.Containers.InfoOutput"/> instance.
            /// </summary>
            /// <param name="infoOutput">The <see cref="T:Dodoni.BasicComponents.Containers.InfoOutput"/> object which is to be filled with informations concering the current instance.</param>
            /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
            public void FillInfoOutput(InfoOutput infoOutput, string categoryName = "General")
            {
                m_IntegratorFactory.FillInfoOutput(infoOutput, categoryName);

                var infoOutputPackage = infoOutput.AcquirePackage(categoryName);
                infoOutputPackage.Add("Lower bound", LowerBound);
                infoOutputPackage.Add("Upper bound", UpperBound);
                infoOutputPackage.Add("Is operable", IsOperable);
            }

            /// <summary>Sets the <see cref="P:Dodoni.BasicComponents.Containers.IInfoOutputQueriable.InfoOutputDetailLevel"/> property.
            /// </summary>
            /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
            /// <returns>A value indicating whether the <see cref="P:Dodoni.BasicComponents.Containers.IInfoOutputQueriable.InfoOutputDetailLevel"/> has been set to <paramref name="infoOutputDetailLevel"/>.
            /// </returns>
            public bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
            {
                return (infoOutputDetailLevel == InfoOutputDetailLevel.Full);
            }
            #endregion

            /// <summary>Returns a <see cref="System.String" /> that represents this instance.
            /// </summary>
            /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
            public override string ToString()
            {
                return String.Format("{0}; lower bound: {1}; upper bound: {2}", m_IntegratorFactory.ToString(), LowerBound, UpperBound);
            }
            #endregion
        }
    }
}