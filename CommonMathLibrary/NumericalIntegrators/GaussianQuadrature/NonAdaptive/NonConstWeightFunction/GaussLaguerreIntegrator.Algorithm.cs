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

using Dodoni.MathLibrary.Basics;
using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.NumericalIntegrators
{
    public partial class GaussLaguerreIntegrator
    {
        /// <summary>Represents the implementation of the algorithm.
        /// </summary>
        private class Algorithm : IOneDimNumericalIntegratorAlgorithm
        {
            #region private members

            /// <summary>The <see cref="GaussLaguerreIntegrator"/> object which serves as factory for the current object.
            /// </summary>
            private GaussLaguerreIntegrator m_IntegratorFactory;
            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="Algorithm"/> class.
            /// </summary>
            /// <param name="gaussLaguerreIntegrator">The <see cref="GaussLaguerreIntegrator"/> object which serves as factory for the current object.</param>
            internal Algorithm(GaussLaguerreIntegrator gaussLaguerreIntegrator)
            {
                m_IntegratorFactory = gaussLaguerreIntegrator;
                if (gaussLaguerreIntegrator.m_AlphaIsZero == true)
                {
                    WeightFunction = OneDimNumericalIntegrator.WeightFunction.Create(x => Math.Exp(-x));
                }
                else
                {
                    WeightFunction = OneDimNumericalIntegrator.WeightFunction.Create(x => Math.Pow(x, m_IntegratorFactory.Alpha) * Math.Exp(-x));
                }
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
                get { return (FunctionToIntegrate != null); }
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
                get;
                private set;
            }

            /// <summary>Gets the lower integration bound.
            /// </summary>
            /// <value>The lower integration bound.</value>
            public double LowerBound
            {
                get { return 0.0; }
            }

            /// <summary>Gets the upper integration bound.
            /// </summary>
            /// <value>The upper integration bound.</value>
            public double UpperBound
            {
                get { return Double.PositiveInfinity; }
            }
            #endregion

            #endregion

            #region public methods

            #region IOneDimNumericalIntegratorAlgorithm Members

            /// <summary>Gets the value of the integral \int_a^b w(x) * f(x) dx.
            /// </summary>
            /// <param name="value">An approximation of the specific integral.</param>
            /// <returns>The state of the algorithm, i.e. an indicating whether <paramref name="value"/> contains valid data.</returns>
            public State GetValue(out double value)
            {
                double previousValue = Double.NaN;
                value = Double.NaN;

                int n = m_IntegratorFactory.InitialOrder;

                for (int k = 0; k < m_IntegratorFactory.ExitCondition.MaxIterations; k++)
                {
                    previousValue = value;
                    m_IntegratorFactory.GetWeightsAndAbscissas(k, out double[] weights, out double[] abscissas);

                    value = 0.0;
                    for (int i = 0; i < n; i++)
                    {
                        value += weights[i] * FunctionToIntegrate(abscissas[i]);
                    }
                    if (m_IntegratorFactory.ExitCondition.IsFulfilled(previousValue, value) == true)
                    {
                        return State.Create(NumericalIntegratorErrorClassification.ProperResult, previousValue, value, iterationsNeeded: k + 1, evaluationsNeeded: (k + 1) * m_IntegratorFactory.InitialOrder + m_IntegratorFactory.OrderStepSize * k * (k + 1) / 2);
                    }
                    n += m_IntegratorFactory.OrderStepSize;
                }
                return State.Create(NumericalIntegratorErrorClassification.IterationLimitExceeded, previousValue, value, iterationsNeeded: m_IntegratorFactory.ExitCondition.MaxIterations, evaluationsNeeded: m_IntegratorFactory.ExitCondition.MaxIterations * m_IntegratorFactory.InitialOrder + m_IntegratorFactory.OrderStepSize * m_IntegratorFactory.ExitCondition.MaxIterations * (m_IntegratorFactory.ExitCondition.MaxIterations + 1) / 2);
            }

            /// <summary>Gets the value of the integral \int_a^b w(x) * f(x) dx.
            /// </summary>
            /// <returns>An approximation of the specific integral.</returns>
            public double GetValue()
            {
                double previousValue;
                double value = Double.NaN;

                int n = m_IntegratorFactory.InitialOrder;

                for (int k = 0; k < m_IntegratorFactory.ExitCondition.MaxIterations; k++)
                {
                    previousValue = value;
                    m_IntegratorFactory.GetWeightsAndAbscissas(k, out double[] weights, out double[] abscissas);

                    value = 0.0;
                    for (int i = 0; i < n; i++)
                    {
                        value += weights[i] * FunctionToIntegrate(abscissas[i]);
                    }
                    if (m_IntegratorFactory.ExitCondition.IsFulfilled(previousValue, value) == true)
                    {
                        return value;
                    }
                    n += m_IntegratorFactory.OrderStepSize;
                }
                return value;
            }

            /// <summary>Sets the lower integration bound.
            /// </summary>
            /// <param name="lowerBound">The lower integration bound.</param>
            /// <returns>A value indicating whether <see cref="IOneDimNumericalIntegratorAlgorithm.LowerBound" /> has been set to <paramref name="lowerBound" />.</returns>
            public bool TrySetLowerBound(double lowerBound)
            {
                return lowerBound.IsZero();
            }

            /// <summary>Sets the upper integration bound.
            /// </summary>
            /// <param name="upperBound">The upper integration bound.</param>
            /// <returns>A value indicating whether <see cref="IOneDimNumericalIntegratorAlgorithm.UpperBound" /> has been set to <paramref name="upperBound" />.</returns>
            public bool TrySetUpperBound(double upperBound)
            {
                return Double.IsPositiveInfinity(upperBound);
            }

            /// <summary>Sets the lower and upper integration bounds.
            /// </summary>
            /// <param name="lowerBound">The lower integration bound.</param>
            /// <param name="upperBound">The upper integration bound.</param>
            /// <returns>A value indicating whether <see cref="IOneDimNumericalIntegratorAlgorithm.LowerBound" /> and <see cref="IOneDimNumericalIntegratorAlgorithm.UpperBound" /> have been changed.</returns>
            public bool TrySetBounds(double lowerBound, double upperBound)
            {
                return lowerBound.IsZero() && Double.IsPositiveInfinity(upperBound);
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
                return String.Format("{0}; lower bound: {1}; upper bound: {2}", m_IntegratorFactory, LowerBound, UpperBound);
            }
            #endregion
        }
    }
}