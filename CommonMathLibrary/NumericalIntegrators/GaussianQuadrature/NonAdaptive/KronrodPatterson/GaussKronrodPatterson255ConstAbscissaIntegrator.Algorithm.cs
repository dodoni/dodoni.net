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
    public partial class GaussKronrodPatterson255ConstAbscissaIntegrator
    {
        /// <summary>Represents the implementation of the algorithm.
        /// </summary>
        private class Algorithm : GaussKronrodPatterson255Quadrature, IOneDimNumericalConstAbscissaIntegratorAlgorithm
        {
            #region private members

            /// <summary>The <see cref="GaussKronrodPatterson255ConstAbscissaIntegrator"/> object which serves as factory for the current object.
            /// </summary>
            private GaussKronrodPatterson255ConstAbscissaIntegrator m_IntegratorFactory;

            /// <summary>The function to integrate.
            /// </summary>
            private OneDimNumericalConstAbscissaIntegrator.FunctionToIntegrate m_FunctionToIntegrate;

            /// <summary>A list of values which are sums of function values.
            /// </summary>
            /// <remarks>This array stores sums of function values and corresponds the the array WORK in 'algorithm 699'.</remarks>
            private double[] m_FunctionSumValueList = new double[17];
            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="Algorithm"/> class.
            /// </summary>
            /// <param name="gaussKronrodPatterson255ConstantAbscissasIntegrator">The <see cref="GaussKronrodPatterson255ConstAbscissaIntegrator"/> object which serves as factory for the current object.</param>
            internal Algorithm(GaussKronrodPatterson255ConstAbscissaIntegrator gaussKronrodPatterson255ConstantAbscissasIntegrator)
            {
                m_IntegratorFactory = gaussKronrodPatterson255ConstantAbscissasIntegrator;
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
                get { return ((m_FunctionToIntegrate != null) && (Double.IsNaN(m_LowerBound) == false) && (Double.IsNaN(m_UpperBound) == false)); }
            }
            #endregion

            #region IOneDimIntegrator Members

            /// <summary>Gets the factory for further <see cref="IOneDimNumericalIntegratorAlgorithm"/> objects of the same type and with the same configuration.
            /// </summary>
            /// <value>The factory for further <see cref="IOneDimNumericalIntegratorAlgorithm"/> objects of the same type and with the same configuration.
            /// </value>
            OneDimNumericalIntegrator IOneDimNumericalIntegratorAlgorithm.Factory
            {
                get { return m_IntegratorFactory; }
            }

            /// <summary>Gets or sets the function to integrate.
            /// </summary>
            /// <value>The function to integrate.</value>
            /// <remarks>A wrapper delegate is used to convert a function f:\R\times \N \to \R to a function f:\R \to \R and vice a versa.</remarks>
            Func<double, double> IOneDimNumericalIntegratorAlgorithm.FunctionToIntegrate
            {
                get { return x => m_FunctionToIntegrate(x, -1); }
                set { m_FunctionToIntegrate = (x, k) => value(x); }
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
            #endregion

            #region IOneDimNumericalConstAbscissaIntegratorAlgorithm Members

            /// <summary>Gets the factory for further <see cref="IOneDimNumericalConstAbscissaIntegratorAlgorithm"/> objects of the same type and with the same configuration.
            /// </summary>
            /// <value>The factory for further <see cref="IOneDimNumericalConstAbscissaIntegratorAlgorithm"/> objects of the same type and with the same configuration.</value>
            public OneDimNumericalConstAbscissaIntegrator Factory
            {
                get { return m_IntegratorFactory; }
            }

            /// <summary>The abscissas, i.e. evaluation points of the numerical integration approach.
            /// </summary>
            /// <value>The abscissas, i.e. evaluation points of the numerical integration approach.</value>
            public IEnumerable<double> Abscissas
            {
                get
                {
                    double upperBoundMinusLowerBoundDivTwo = (m_UpperBound - m_LowerBound) / 2.0;

                    yield return m_LowerBound + upperBoundMinusLowerBoundDivTwo;

                    int ip = 0;
                    int jh = 0;

                    for (int k = 1; k <= Math.Min(m_IntegratorFactory.ExitCondition.MaxIterations, 7); k++)
                    {
                        for (int i = sm_KX[k - 1]; i <= sm_KX[k] - 1; i++)
                        {
                            for (int j = sm_KL[i]; j <= sm_KH[i]; j++)
                            {
                                ip++;
                            }
                        }

                        int jl = jh;
                        jh = jl + jl;

                        for (int j = jl; j <= jh; j++)
                        {
                            double x = sm_Coefficients[ip] * upperBoundMinusLowerBoundDivTwo;
                            yield return m_LowerBound + x;
                            yield return m_UpperBound - x;

                            ip += 2;
                        }
                        jh++;
                    }
                }
            }

            /// <summary>Gets or sets the function to integrate.
            /// </summary>
            /// <value>The function to integrate.</value>
            public OneDimNumericalConstAbscissaIntegrator.FunctionToIntegrate FunctionToIntegrate
            {
                get { return m_FunctionToIntegrate; }
                set { m_FunctionToIntegrate = value; }
            }
            #endregion

            #endregion

            #region public methods

            #region IOneDimNumericalIntegratorAlgorithm Member

            /// <summary>Numerically integrates the specific real-valued function of one variable.
            /// </summary>
            /// <returns> An approximation of the specific integral.</returns>
            public double GetValue()
            {
                double upperBoundMinusLowerBoundDivTwo = (m_UpperBound - m_LowerBound) / 2.0;
                int abscissaIndex = 0;

                /* apply the 1-point Gauss rule (which is the midpoint rule) and store the function value: */
                double functionValue = m_FunctionToIntegrate(m_LowerBound + upperBoundMinusLowerBoundDivTwo, abscissaIndex++);
                m_FunctionSumValueList[0] = functionValue;

                double valueForRefinement = functionValue * (m_UpperBound - m_LowerBound);
                double value = valueForRefinement;

                int ip = 0;
                int jh = 0;

                for (int k = 1; k <= Math.Min(m_IntegratorFactory.ExitCondition.MaxIterations, GaussKronrodPatterson255Quadrature.MaxIterations); k++)
                {
                    value = valueForRefinement;

                    double priviousAccumulationValue = valueForRefinement;
                    valueForRefinement = 0.0;
                    for (int i = sm_KX[k - 1]; i <= sm_KX[k] - 1; i++)
                    {
                        for (int j = sm_KL[i]; j <= sm_KH[i]; j++)
                        {
                            valueForRefinement += sm_Coefficients[ip] * m_FunctionSumValueList[j];
                            ip++;
                        }
                    }

                    // compute contribution from new function values:
                    int jl = jh;
                    jh = jl + jl;
                    int j1 = sm_FL[k - 1];
                    int j2 = sm_FH[k - 1];

                    for (int j = jl; j <= jh; j++)
                    {
                        double x = sm_Coefficients[ip] * upperBoundMinusLowerBoundDivTwo;
                        functionValue = m_FunctionToIntegrate(m_LowerBound + x, abscissaIndex++) + m_FunctionToIntegrate(m_UpperBound - x, abscissaIndex++);

                        valueForRefinement += sm_Coefficients[ip + 1] * functionValue;
                        if (j1 <= j2)
                        {
                            m_FunctionSumValueList[j1] = functionValue;
                            j1++;
                        }
                        ip += 2;
                    }
                    valueForRefinement = upperBoundMinusLowerBoundDivTwo * valueForRefinement + 0.5 * priviousAccumulationValue;
                    jh++;

                    if (m_IntegratorFactory.ExitCondition.IsFulfilled(valueForRefinement, value) == true)
                    {
                        return valueForRefinement;
                    }
                }
                return valueForRefinement;
            }

            /// <summary>Gets the value of the integral \int_a^b w(x) * f(x) dx.
            /// </summary>
            /// <param name="value">An approximation of the specific integral.</param>
            /// <returns>The state of the algorithm, i.e. an indicating whether <paramref name="value" /> contains valid data.</returns>
            public OneDimNumericalIntegrator.State GetValue(out double value)
            {
                double upperBoundMinusLowerBoundDivTwo = (m_UpperBound - m_LowerBound) / 2.0;
                int abscissaIndex = 0;

                /* apply the 1-point Gauss rule (which is the midpoint rule) and store the function value: */
                double functionValue = m_FunctionToIntegrate(m_LowerBound + upperBoundMinusLowerBoundDivTwo, abscissaIndex++);
                m_FunctionSumValueList[0] = functionValue;

                value = functionValue * (m_UpperBound - m_LowerBound);
                double valueBenchmark = value;

                int ip = 0;
                int jh = 0;

                for (int k = 1; k <= Math.Min(m_IntegratorFactory.ExitCondition.MaxIterations, GaussKronrodPatterson255Quadrature.MaxIterations); k++)
                {
                    valueBenchmark = value;

                    double priviousAccumulationValue = value;
                    value = 0.0;
                    for (int i = sm_KX[k - 1]; i <= sm_KX[k] - 1; i++)
                    {
                        for (int j = sm_KL[i]; j <= sm_KH[i]; j++)
                        {
                            value += sm_Coefficients[ip] * m_FunctionSumValueList[j];
                            ip++;
                        }
                    }

                    // compute contribution from new function values:
                    int jl = jh;
                    jh = jl + jl;
                    int j1 = sm_FL[k - 1];
                    int j2 = sm_FH[k - 1];

                    for (int j = jl; j <= jh; j++)
                    {
                        double x = sm_Coefficients[ip] * upperBoundMinusLowerBoundDivTwo;
                        functionValue = m_FunctionToIntegrate(m_LowerBound + x, abscissaIndex++) + m_FunctionToIntegrate(m_UpperBound - x, abscissaIndex++);

                        value += sm_Coefficients[ip + 1] * functionValue;
                        if (j1 <= j2)
                        {
                            m_FunctionSumValueList[j1] = functionValue;
                            j1++;
                        }
                        ip += 2;
                    }
                    value = upperBoundMinusLowerBoundDivTwo * value + 0.5 * priviousAccumulationValue;
                    jh++;

                    if (m_IntegratorFactory.ExitCondition.IsFulfilled(value, valueBenchmark) == true)
                    {
                        return OneDimNumericalIntegrator.State.Create(NumericalIntegratorErrorClassification.ProperResult, valueBenchmark, value, iterationsNeeded: k);
                    }
                }
                return OneDimNumericalIntegrator.State.Create(NumericalIntegratorErrorClassification.IterationLimitExceeded, valueBenchmark, value, Math.Min(m_IntegratorFactory.ExitCondition.MaxIterations, GaussKronrodPatterson255Quadrature.MaxIterations), evaluationsNeeded: MaxEvaluations);
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