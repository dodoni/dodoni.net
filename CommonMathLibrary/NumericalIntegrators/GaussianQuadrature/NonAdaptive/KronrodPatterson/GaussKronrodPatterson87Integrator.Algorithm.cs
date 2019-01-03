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
    public partial class GaussKronrodPatterson87Integrator
    {
        /// <summary>Represents the implementation of the algorithm.
        /// </summary>
        private class Algorithm : GaussKronrodPatterson87Quadrature, IOneDimNumericalIntegratorAlgorithm
        {
            #region private members

            /// <summary>The <see cref="GaussKronrodPatterson87Integrator"/> object which serves as factory for the current object.
            /// </summary>
            private GaussKronrodPatterson87Integrator m_IntegratorFactory;

            /// <summary>A list of values which are sums of function values.
            /// </summary>
            /// <remarks>For each i the following statement w_i * [f(x_i) + f(-x_i)] has to be computed, where the sum of these both function values is stored in the specified list. 
            /// This member is initialized at this point for performance reason only.</remarks>
            private double[] m_FunctionSumValueList = new double[43];
            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="Algorithm"/> class.
            /// </summary>
            /// <param name="gaussKronrodPattersonIntegrator">The <see cref="GaussKronrodPatterson87Integrator"/> object which serves as factory for the current object.</param>
            internal Algorithm(GaussKronrodPatterson87Integrator gaussKronrodPattersonIntegrator)
            {
                m_IntegratorFactory = gaussKronrodPattersonIntegrator;
            }
            #endregion

            #region public properties

            #region IOperable Members

            /// <summary>Gets a value indicating whether this instance is operable.
            /// </summary>
            /// <value>
            /// 	<c>true</c> if this instance is operable; otherwise, <c>false</c>.
            /// </value>
            /// <remarks><c>false</c> will be returned if no function to integrate is given or the upper resp. lower integration bound is not valid.
            /// </remarks>
            public override bool IsOperable
            {
                get { return ((FunctionToIntegrate != null) && (Double.IsNaN(m_LowerBound) == false) && (Double.IsNaN(m_UpperBound) == false)); }
            }
            #endregion

            #region IOneDimIntegrator Members

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

            /// <summary>Gets the factory for further <see cref="IOneDimNumericalIntegratorAlgorithm"/> objects of the same type and with the same configuration.
            /// </summary>
            /// <value>The factory for further <see cref="IOneDimNumericalIntegratorAlgorithm"/> objects of the same type and with the same configuration.
            /// </value>
            public OneDimNumericalIntegrator Factory
            {
                get { return m_IntegratorFactory; }
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
                /* compute the values which are used for the transformation from [-1,1]
                 * to the specified interval only once. The transformation is given by:
                 * 
                 *  \int_a^b f(x) dx  \approx  \frac{b-a}{2} * \sum_{j=0}^n w_j * f( x_j * (b-a)/2  + (b+a)/2),
                 * 
                 * where w_j, x_j are the weights resp. evaluation points for the case of a=-1, b=1.
                 */
                double upperBoundMinusLowerBoundDivTwo = (m_UpperBound - m_LowerBound) / 2.0;
                double upperBoundPlusLowerBoundDivTwo = (m_UpperBound + m_LowerBound) / 2.0;

                double functionValueInCenteredPoint = FunctionToIntegrate(upperBoundPlusLowerBoundDivTwo);  // = f( (b+a)/2 )

                /* 1.) Compute the integral using the 10- and 21-point rules: */
                double previousValue = 0.0;
                double value = sm_WeightsKronrod21b[5] * functionValueInCenteredPoint;  // = w * f( midpoint )
                for (int i = 0; i < 5; i++)
                {
                    m_FunctionSumValueList[i] = FunctionToIntegrate(upperBoundMinusLowerBoundDivTwo * sm_CommonEvaluationPoints1[i] + upperBoundPlusLowerBoundDivTwo) + FunctionToIntegrate(-upperBoundMinusLowerBoundDivTwo * sm_CommonEvaluationPoints1[i] + upperBoundPlusLowerBoundDivTwo);

                    previousValue += sm_WeightsKronrod10[i] * m_FunctionSumValueList[i];
                    value += sm_WeightsKronrod21a[i] * m_FunctionSumValueList[i];

                    m_FunctionSumValueList[5 + i] = FunctionToIntegrate(upperBoundMinusLowerBoundDivTwo * sm_CommonEvaluationPoints2[i] + upperBoundPlusLowerBoundDivTwo) + FunctionToIntegrate(-upperBoundMinusLowerBoundDivTwo * sm_CommonEvaluationPoints2[i] + upperBoundPlusLowerBoundDivTwo);
                    value += sm_WeightsKronrod21b[i] * m_FunctionSumValueList[5 + i];
                }
                if ((m_IntegratorFactory.ExitCondition.MaxIterations < 2) || (m_IntegratorFactory.ExitCondition.IsFulfilled(upperBoundMinusLowerBoundDivTwo * value, upperBoundMinusLowerBoundDivTwo * previousValue) == true))
                {
                    return value * upperBoundMinusLowerBoundDivTwo;
                }
                else /* 2.) Compute the integral using the 43-point rule: */
                {
                    previousValue = value;
                    value = sm_WeightsKronrod43b[11] * functionValueInCenteredPoint;
                    for (int i = 0; i < 10; i++)
                    {
                        value += sm_WeightsKronrod43a[i] * m_FunctionSumValueList[i];

                        m_FunctionSumValueList[10 + i] = FunctionToIntegrate(upperBoundMinusLowerBoundDivTwo * sm_CommonEvaluationPoints3[i] + upperBoundPlusLowerBoundDivTwo) + FunctionToIntegrate(-upperBoundMinusLowerBoundDivTwo * sm_CommonEvaluationPoints3[i] + upperBoundPlusLowerBoundDivTwo);
                        value += sm_WeightsKronrod43b[i] * m_FunctionSumValueList[10 + i];
                    }
                    m_FunctionSumValueList[20] = FunctionToIntegrate(upperBoundMinusLowerBoundDivTwo * sm_CommonEvaluationPoints3[10] + upperBoundPlusLowerBoundDivTwo) + FunctionToIntegrate(-upperBoundMinusLowerBoundDivTwo * sm_CommonEvaluationPoints3[10] + upperBoundPlusLowerBoundDivTwo);
                    value += sm_WeightsKronrod43b[10] * m_FunctionSumValueList[20];

                    if ((m_IntegratorFactory.ExitCondition.MaxIterations < 3) || (m_IntegratorFactory.ExitCondition.IsFulfilled(upperBoundMinusLowerBoundDivTwo * value, upperBoundMinusLowerBoundDivTwo * previousValue) == true))
                    {
                        return value * upperBoundMinusLowerBoundDivTwo;
                    }
                    else /* 3.) Compute the integral using the 87-point rule: */
                    {
                        previousValue = value;
                        value = sm_WeightsKronrod87b[22] * functionValueInCenteredPoint;

                        for (int i = 0; i < 21; i++)
                        {
                            value += sm_WeightsKronrod87a[i] * m_FunctionSumValueList[i];

                            m_FunctionSumValueList[21 + i] = FunctionToIntegrate(upperBoundMinusLowerBoundDivTwo * sm_CommonEvaluationPoints4[i] + upperBoundPlusLowerBoundDivTwo) + FunctionToIntegrate(-upperBoundMinusLowerBoundDivTwo * sm_CommonEvaluationPoints4[i] + upperBoundPlusLowerBoundDivTwo);
                            value += sm_WeightsKronrod87b[i] * m_FunctionSumValueList[21 + i];
                        }
                        m_FunctionSumValueList[42] = FunctionToIntegrate(upperBoundMinusLowerBoundDivTwo * sm_CommonEvaluationPoints4[21] + upperBoundPlusLowerBoundDivTwo) + FunctionToIntegrate(-upperBoundMinusLowerBoundDivTwo * sm_CommonEvaluationPoints4[21] + upperBoundPlusLowerBoundDivTwo);
                        value += sm_WeightsKronrod87b[21] * m_FunctionSumValueList[42];

                        return value * upperBoundMinusLowerBoundDivTwo; // we do not check the exit condition any more
                    }
                }
            }

            /// <summary>Gets the value of the integral \int_a^b w(x) * f(x) dx.
            /// </summary>
            /// <param name="value">An approximation of the specific integral.</param>
            /// <returns>The state of the algorithm, i.e. an indicating whether <paramref name="value"/> contains valid data.</returns>
            public State GetValue(out double value)
            {
                /* compute the values which are used for the transformation from [-1,1]
                 * to the given interval only once. The transformation is given by:
                 * 
                 *  \int_a^b f(x) dx  \approx  \frac{b-a}{2} * \sum_{j=0}^n w_j * f( x_j * (b-a)/2  + (b+a)/2),
                 * 
                 * where w_j, x_j are the weights resp. evaluation points for the case of a=-1, b=1.
                 */
                double upperBoundMinusLowerBoundDivTwo = (m_UpperBound - m_LowerBound) / 2.0;
                double upperBoundPlusLowerBoundDivTwo = (m_UpperBound + m_LowerBound) / 2.0;

                double functionValueInCenteredPoint = FunctionToIntegrate(upperBoundPlusLowerBoundDivTwo);  // = f( (b+a)/2 )

                /* 1.) Compute the integral using the 10- and 21-point rules: */
                double previousValue = 0.0;
                value = sm_WeightsKronrod21b[5] * functionValueInCenteredPoint;  // = w * f( midpoint )
                for (int i = 0; i < 5; i++)
                {
                    m_FunctionSumValueList[i] = FunctionToIntegrate(upperBoundMinusLowerBoundDivTwo * sm_CommonEvaluationPoints1[i] + upperBoundPlusLowerBoundDivTwo) + FunctionToIntegrate(-upperBoundMinusLowerBoundDivTwo * sm_CommonEvaluationPoints1[i] + upperBoundPlusLowerBoundDivTwo);

                    previousValue += sm_WeightsKronrod10[i] * m_FunctionSumValueList[i];
                    value += sm_WeightsKronrod21a[i] * m_FunctionSumValueList[i];

                    m_FunctionSumValueList[5 + i] = FunctionToIntegrate(upperBoundMinusLowerBoundDivTwo * sm_CommonEvaluationPoints2[i] + upperBoundPlusLowerBoundDivTwo) + FunctionToIntegrate(-upperBoundMinusLowerBoundDivTwo * sm_CommonEvaluationPoints2[i] + upperBoundPlusLowerBoundDivTwo);
                    value += sm_WeightsKronrod21b[i] * m_FunctionSumValueList[5 + i];
                }
                if (m_IntegratorFactory.ExitCondition.IsFulfilled(upperBoundMinusLowerBoundDivTwo * value, upperBoundMinusLowerBoundDivTwo * previousValue) == true)
                {
                    value *= upperBoundMinusLowerBoundDivTwo;
                    previousValue *= upperBoundMinusLowerBoundDivTwo;
                    return OneDimNumericalIntegrator.State.Create(NumericalIntegratorErrorClassification.ProperResult, previousValue, value, 1, evaluationsNeeded: 21);
                }
                else if (m_IntegratorFactory.ExitCondition.MaxIterations < 2)
                {
                    value *= upperBoundMinusLowerBoundDivTwo;
                    previousValue *= upperBoundMinusLowerBoundDivTwo;
                    return OneDimNumericalIntegrator.State.Create(NumericalIntegratorErrorClassification.IterationLimitExceeded, previousValue, value, 1, evaluationsNeeded: 21);
                }
                else /* 2.) Compute the integral using the 43-point rule: */
                {
                    previousValue = value;
                    value = sm_WeightsKronrod43b[11] * functionValueInCenteredPoint;
                    for (int i = 0; i < 10; i++)
                    {
                        value += sm_WeightsKronrod43a[i] * m_FunctionSumValueList[i];

                        m_FunctionSumValueList[10 + i] = FunctionToIntegrate(upperBoundMinusLowerBoundDivTwo * sm_CommonEvaluationPoints3[i] + upperBoundPlusLowerBoundDivTwo) + FunctionToIntegrate(-upperBoundMinusLowerBoundDivTwo * sm_CommonEvaluationPoints3[i] + upperBoundPlusLowerBoundDivTwo);
                        value += sm_WeightsKronrod43b[i] * m_FunctionSumValueList[10 + i];
                    }
                    m_FunctionSumValueList[20] = FunctionToIntegrate(upperBoundMinusLowerBoundDivTwo * sm_CommonEvaluationPoints3[10] + upperBoundPlusLowerBoundDivTwo) + FunctionToIntegrate(-upperBoundMinusLowerBoundDivTwo * sm_CommonEvaluationPoints3[10] + upperBoundPlusLowerBoundDivTwo);
                    value += sm_WeightsKronrod43b[10] * m_FunctionSumValueList[20];

                    if (m_IntegratorFactory.ExitCondition.IsFulfilled(upperBoundMinusLowerBoundDivTwo * value, upperBoundMinusLowerBoundDivTwo * previousValue) == true)
                    {
                        value *= upperBoundMinusLowerBoundDivTwo;
                        previousValue *= upperBoundMinusLowerBoundDivTwo;
                        return OneDimNumericalIntegrator.State.Create(NumericalIntegratorErrorClassification.ProperResult, previousValue, value, 2, evaluationsNeeded: 43);
                    }
                    else if (m_IntegratorFactory.ExitCondition.MaxIterations < 3)
                    {
                        value *= upperBoundMinusLowerBoundDivTwo;
                        previousValue *= upperBoundMinusLowerBoundDivTwo;
                        return OneDimNumericalIntegrator.State.Create(NumericalIntegratorErrorClassification.IterationLimitExceeded, previousValue, value, 2, evaluationsNeeded: 43);
                    }
                    else /* 3.) Compute the integral using the 87-point rule: */
                    {
                        previousValue = value;
                        value = sm_WeightsKronrod87b[22] * functionValueInCenteredPoint;

                        for (int i = 0; i < 21; i++)
                        {
                            value += sm_WeightsKronrod87a[i] * m_FunctionSumValueList[i];

                            m_FunctionSumValueList[21 + i] = FunctionToIntegrate(upperBoundMinusLowerBoundDivTwo * sm_CommonEvaluationPoints4[i] + upperBoundPlusLowerBoundDivTwo) + FunctionToIntegrate(-upperBoundMinusLowerBoundDivTwo * sm_CommonEvaluationPoints4[i] + upperBoundPlusLowerBoundDivTwo);
                            value += sm_WeightsKronrod87b[i] * m_FunctionSumValueList[21 + i];
                        }
                        m_FunctionSumValueList[42] = FunctionToIntegrate(upperBoundMinusLowerBoundDivTwo * sm_CommonEvaluationPoints4[21] + upperBoundPlusLowerBoundDivTwo) + FunctionToIntegrate(-upperBoundMinusLowerBoundDivTwo * sm_CommonEvaluationPoints4[21] + upperBoundPlusLowerBoundDivTwo);
                        value += sm_WeightsKronrod87b[21] * m_FunctionSumValueList[42];

                        value *= upperBoundMinusLowerBoundDivTwo;
                        previousValue *= upperBoundMinusLowerBoundDivTwo;

                        var errorClassification = (m_IntegratorFactory.ExitCondition.IsFulfilled(upperBoundMinusLowerBoundDivTwo * value, upperBoundMinusLowerBoundDivTwo * previousValue) == true) ? NumericalIntegratorErrorClassification.ProperResult : NumericalIntegratorErrorClassification.Divergent;
                        return OneDimNumericalIntegrator.State.Create(errorClassification, previousValue, value, 3, evaluationsNeeded: 87);
                    }
                }
            }
            #endregion

            /// <summary>Gets informations of the current object as a specific <see cref="T:Dodoni.BasicComponents.Containers.InfoOutput"/> instance.
            /// </summary>
            /// <param name="infoOutput">The <see cref="T:Dodoni.BasicComponents.Containers.InfoOutput"/> object which is to be filled with informations concering the current instance.</param>
            /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
            public override void FillInfoOutput(InfoOutput infoOutput, string categoryName = "General")
            {
                m_IntegratorFactory.FillInfoOutput(infoOutput, categoryName);

                var infoOutputPackage = infoOutput.AcquirePackage(categoryName);
                infoOutputPackage.Add("Lower bound", LowerBound);
                infoOutputPackage.Add("Upper bound", UpperBound);
                infoOutputPackage.Add("Is operable", IsOperable);
            }

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