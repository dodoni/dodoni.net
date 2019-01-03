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

namespace Dodoni.MathLibrary.NumericalIntegrators
{
    public partial class RombergIntegrator
    {
        /// <summary>Represents the implementation of the algorithm.
        /// </summary>
        private class Algorithm : IOneDimNumericalIntegratorAlgorithm
        {
            #region private static members

            /// <summary>Represents the maximal number of iteration steps, i.e. the integer 'N' such that 2^N is strictly less than <see cref="Int32.MaxValue"/>, i.e. log(<see cref="Int32.MaxValue"/>) / log (2) -1.
            /// </summary>
            private static readonly int MaxIterations = ((int)(Math.Log(Int32.MaxValue) / MathConsts.Log2) - 1);
            #endregion

            #region private members

            /// <summary>The <see cref="RombergIntegrator"/> object which serves as factory for the current object.
            /// </summary>
            private RombergIntegrator m_IntegratorFactory;

            /// <summary>The lower integration bound.
            /// </summary>
            private double m_LowerBound = Double.NaN;

            /// <summary>The upper integration bound.
            /// </summary>
            private double m_UpperBound = Double.NaN;

            /// <summary>A workspace array for the calculation.
            /// </summary>
            private double[] m_CurrentTempValue;

            /// <summary>A workspace array for the calculation.
            /// </summary>
            private double[] m_NextTempValue;
            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="Algorithm"/> class.
            /// </summary>
            /// <param name="rombergIntegrator">The <see cref="RombergIntegrator"/> object which serves as factory for the current object.</param>
            internal Algorithm(RombergIntegrator rombergIntegrator)
            {
                m_IntegratorFactory = rombergIntegrator;

                m_CurrentTempValue = new double[m_IntegratorFactory.ExitCondition.MaxIterations];
                m_NextTempValue = new double[m_IntegratorFactory.ExitCondition.MaxIterations];
            }
            #endregion

            #region static constructors

            /// <summary>Initializes the <see cref="Algorithm"/> class.
            /// </summary>
            static Algorithm()
            {
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
            /// <param name="value">An approximation of the specific integral.</param>
            /// <returns>The state of the algorithm, i.e. an indicating whether <paramref name="value"/> contains valid data.
            /// </returns>
            public State GetValue(out double value)
            {
                var intervalLength = m_UpperBound - m_LowerBound;

                long M = 1;

                int j = 0;
                while (j < Math.Min(m_IntegratorFactory.ExitCondition.MaxIterations, MaxIterations))
                {
                    /* set 'nextT[0] = T_{j,0}' to the result of the composite trapezoidal rule of degree M: */
                    if (j == 0)
                    {
                        m_NextTempValue[0] = intervalLength / 2.0 * (FunctionToIntegrate(m_LowerBound) + FunctionToIntegrate(m_UpperBound));
                    }
                    else
                    {
                        m_CurrentTempValue[j] = m_NextTempValue[j - 1];

                        M <<= 1; // M = 2^{j}

                        double h = intervalLength / M;
                        double x = m_LowerBound + h;

                        double subTrapezValueRefinement = 0.0;
                        for (int i = 1; i <= M / 2; i++)
                        {
                            subTrapezValueRefinement += FunctionToIntegrate(x);
                            x += 2.0 * h;
                        }
                        m_NextTempValue[0] = 0.5 * m_NextTempValue[0] + h * subTrapezValueRefinement;

                        /* now compute T_{j,k}, k =1,...j */
                        long denominator = 1;
                        for (int k = 1; k <= j; k++)
                        {
                            denominator <<= 2;  // = 4^k

                            m_NextTempValue[k] = m_NextTempValue[k - 1] + (m_NextTempValue[k - 1] - m_CurrentTempValue[k - 1]) / (denominator - 1);
                            m_CurrentTempValue[k - 1] = m_NextTempValue[k - 1];
                        }

                        if (m_IntegratorFactory.ExitCondition.IsFulfilled(m_CurrentTempValue[j - 1], m_NextTempValue[j]) == true)
                        {
                            value = m_NextTempValue[j];
                            return State.Create(NumericalIntegratorErrorClassification.ProperResult, m_CurrentTempValue[j - 1], m_NextTempValue[j], j + 1, (int)DoMath.Pow(2, j) + 1);
                        }
                        if (M + 1 > m_IntegratorFactory.ExitCondition.MaxEvaluations)
                        {
                            value = m_NextTempValue[j];
                            return State.Create(NumericalIntegratorErrorClassification.EvaluationLimitExceeded, m_CurrentTempValue[j - 1], m_NextTempValue[j], j + 1, (int)DoMath.Pow(2, j) + 1);
                        }
                    }
                    j++;
                }
                value = m_NextTempValue[j - 1];
                return State.Create(NumericalIntegratorErrorClassification.IterationLimitExceeded, m_CurrentTempValue[j - 1], m_NextTempValue[j - 1], m_IntegratorFactory.ExitCondition.MaxIterations, (int)DoMath.Pow(2, j) + 1);
            }

            /// <summary>Gets the value of the integral \int_a^b w(x) * f(x) dx.
            /// </summary>
            /// <returns>An approximation of the specific integral.</returns>
            public double GetValue()
            {
                var intervalLength = m_UpperBound - m_LowerBound;

                long M = 1;
                for (int j = 0; j < Math.Min(m_IntegratorFactory.ExitCondition.MaxIterations, MaxIterations); j++)
                {
                    /* set 'nextT[0] = T_{j,0}' to the result of the composite trapezoidal rule of degree M: */
                    if (j == 0)
                    {
                        m_NextTempValue[0] = intervalLength / 2.0 * (FunctionToIntegrate(m_LowerBound) + FunctionToIntegrate(m_UpperBound));
                    }
                    else
                    {
                        m_CurrentTempValue[j] = m_NextTempValue[j - 1];

                        M <<= 1; // M = 2^{j}

                        double h = intervalLength / M;
                        double x = m_LowerBound + h;

                        double subTrapezValueRefinement = 0.0;
                        for (int i = 1; i <= M / 2; i++)
                        {
                            subTrapezValueRefinement += FunctionToIntegrate(x);
                            x += 2.0 * h;
                        }
                        m_NextTempValue[0] = 0.5 * m_NextTempValue[0] + h * subTrapezValueRefinement;

                        /* now compute T_{j,k}, k =1,...j */
                        long denominator = 1;
                        for (int k = 1; k <= j; k++)
                        {
                            denominator <<= 2;  // = 4^k

                            m_NextTempValue[k] = m_NextTempValue[k - 1] + (m_NextTempValue[k - 1] - m_CurrentTempValue[k - 1]) / (denominator - 1);
                            m_CurrentTempValue[k - 1] = m_NextTempValue[k - 1];
                        }

                        if (m_IntegratorFactory.ExitCondition.IsFulfilled(m_CurrentTempValue[j - 1], m_NextTempValue[j]) == true)
                        {
                            return m_NextTempValue[j];
                        }
                        if (M + 1 > m_IntegratorFactory.ExitCondition.MaxEvaluations)
                        {
                            return m_NextTempValue[j];
                        }
                    }
                }
                return m_NextTempValue[m_IntegratorFactory.ExitCondition.MaxIterations - 1];
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
                return String.Format("{0}; lower bound: {1}; upper bound: {2}", m_IntegratorFactory, LowerBound, UpperBound);
            }
            #endregion
        }
    }
}