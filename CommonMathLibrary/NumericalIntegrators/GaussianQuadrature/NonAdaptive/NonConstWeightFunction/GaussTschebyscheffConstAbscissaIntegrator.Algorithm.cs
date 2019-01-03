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
    public partial class GaussTschebyscheffConstAbscissaIntegrator : OneDimNumericalConstAbscissaIntegrator
    {
        /// <summary>Represents the implementation of the algorithm.
        /// </summary>
        private class Algorithm : IOneDimNumericalConstAbscissaIntegratorAlgorithm
        {
            #region private members

            /// <summary>The <see cref="GaussTschebyscheffConstAbscissaIntegrator"/> object which serves as factory for the current object.
            /// </summary>
            private GaussTschebyscheffConstAbscissaIntegrator m_IntegratorFactory;

            /// <summary>The function to integrate.
            /// </summary>
            private OneDimNumericalConstAbscissaIntegrator.FunctionToIntegrate m_FunctionToIntegrate;

            /// <summary>The lower integration bound.
            /// </summary>
            private double m_LowerBound = -1.0;

            /// <summary>The upper integration bound.
            /// </summary>
            private double m_UpperBound = 1.0;

            /// <summary>All weights in the Gauss-Tschebyscheff Quadrature are equal to PI/order.
            /// </summary>
            private double m_Weight;

            /// <summary>The abscissas, i.e. evaluation points of the Gauss-Tschebyscheff Quadrature on (-1,1).
            /// </summary>
            private double[] m_EvaluationPoints;
            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="Algorithm"/> class.
            /// </summary>
            /// <param name="gaussTschebyscheffConstantAbscissasIntegrator">The <see cref="GaussTschebyscheffConstAbscissaIntegrator"/> object which serves as factory for the current object.</param>
            internal Algorithm(GaussTschebyscheffConstAbscissaIntegrator gaussTschebyscheffConstantAbscissasIntegrator)
            {
                m_IntegratorFactory = gaussTschebyscheffConstantAbscissasIntegrator;
                WeightFunction = OneDimNumericalIntegrator.WeightFunction.Create(x => 1.0 / Math.Sqrt(1.0 - x * x));

                m_EvaluationPoints = GaussianQuadrature.Tschebyscheff.GetValue(m_IntegratorFactory.Order, out m_Weight);
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
                get { return (m_FunctionToIntegrate != null); }
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
                get;
                private set;
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
                    double upperBoundPlusLowerBoundDivTwo = (m_UpperBound + m_LowerBound) / 2.0;

                    for (int k = 0; k < m_IntegratorFactory.Order; k++)
                    {
                        yield return upperBoundMinusLowerBoundDivTwo * m_EvaluationPoints[k] + upperBoundPlusLowerBoundDivTwo;
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

            /// <summary>Gets the value of the integral \int_a^b w(x) * f(x) dx.
            /// </summary>
            /// <returns>An approximation of the specific integral.
            /// </returns>
            public double GetValue()
            {
                /* Compute the values which are used for the transformation from [-1,1] to the specified interval. The transformation is defined by:
                 * 
                 * \int_a^b f(x) dx  \approx  \frac{b-a}{2} * \sum_{j=0}^n w_j * f(x_j * (b-a)/2  + (b+a)/2),
                 * 
                 * where w_j, x_j are the weights resp. evaluation points for the case of a=-1, b=1. */

                var upperBoundMinusLowerBoundDivTwo = (m_UpperBound - m_LowerBound) / 2.0;
                var upperBoundPlusLowerBoundDivTwo = (m_UpperBound + m_LowerBound) / 2.0;

                var value = 0.0;
                for (int i = 0; i < m_IntegratorFactory.Order; i++)
                {
                    value += FunctionToIntegrate(upperBoundMinusLowerBoundDivTwo * m_EvaluationPoints[i] + upperBoundPlusLowerBoundDivTwo, i);
                }
                return value * upperBoundMinusLowerBoundDivTwo * m_Weight;
            }

            /// <summary>Gets the value of the integral \int_a^b w(x) * f(x) dx.
            /// </summary>
            /// <param name="value">An approximation of the specific integral.</param>
            /// <returns>The state of the algorithm, i.e. an indicating whether <paramref name="value"/> contains valid data.
            /// </returns>
            public OneDimNumericalIntegrator.State GetValue(out double value)
            {
                value = GetValue();
                return OneDimNumericalIntegrator.State.Create(NumericalIntegratorErrorClassification.Unknown);
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
                WeightFunction = OneDimNumericalIntegrator.WeightFunction.Create(x => 1.0 / Math.Sqrt(1.0 - Math.Pow((x - (m_UpperBound + m_LowerBound) / 2.0) * 2.0 / (m_UpperBound - m_LowerBound), 2)));
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
                WeightFunction = OneDimNumericalIntegrator.WeightFunction.Create(x => 1.0 / Math.Sqrt(1.0 - Math.Pow((x - (m_UpperBound + m_LowerBound) / 2.0) * 2.0 / (m_UpperBound - m_LowerBound), 2)));
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
                WeightFunction = OneDimNumericalIntegrator.WeightFunction.Create(x => 1.0 / Math.Sqrt(1.0 - Math.Pow((x - (m_UpperBound + m_LowerBound) / 2.0) * 2.0 / (m_UpperBound - m_LowerBound), 2)));
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