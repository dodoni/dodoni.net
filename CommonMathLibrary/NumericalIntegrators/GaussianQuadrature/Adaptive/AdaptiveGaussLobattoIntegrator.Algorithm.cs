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
    public partial class AdaptiveGaussLobattoIntegrator
    {
        /// <summary>Represents the implementation of the algorithm.
        /// </summary>
        private class Algorithm : IOneDimNumericalIntegratorAlgorithm
        {
            #region private static members/consts

            /// <summary>Some evaluation point.
            /// </summary>
            private const double x1 = 0.942882415695480;

            /// <summary>Some evaluation point.
            /// </summary>
            private const double x2 = 0.641853342345781;

            /// <summary>Some evaluation point.
            /// </summary>
            private const double x3 = 0.236383199662150;
            #endregion

            #region private members

            /// <summary>The <see cref="AdaptiveGaussLobattoIntegrator"/> object which serves as factory for the current object.
            /// </summary>
            private AdaptiveGaussLobattoIntegrator m_IntegratorFactory;

            /// <summary>The lower integration bound.
            /// </summary>
            private double m_LowerBound = Double.NaN;

            /// <summary>The upper integration bound.
            /// </summary>
            private double m_UpperBound = Double.NaN;

            /// <summary>The number of function evaluations performed by the algorithm.
            /// </summary>
            private int m_FunctionEvaluationsNeeded = 0;

            /// <summary>The number of iterations needed.
            /// </summary>
            private int m_IterationsNeeded = 0;

            /// <summary>A estimation of the integral which will be used for setting the estimated absolute and the estimated relative error.
            /// </summary>
            private double m_BenchmarkEstimation;
            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="Algorithm"/> class.
            /// </summary>
            /// <param name="adaptiveGaussLobattoIntegrator">The <see cref="AdaptiveGaussLobattoIntegrator"/> object which serves as factory for the current object.</param>
            internal Algorithm(AdaptiveGaussLobattoIntegrator adaptiveGaussLobattoIntegrator)
            {
                m_IntegratorFactory = adaptiveGaussLobattoIntegrator;
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
            /// <returns>The state of the algorithm, i.e. an indicating whether <paramref name="value"/> contains valid data.</returns>
            public State GetValue(out double value)
            {
                /* calculate the integral in the first step at 13 points: */
                double h = (m_UpperBound - m_LowerBound) / 2.0;
                double m = (m_LowerBound + m_UpperBound) / 2.0;

                double f1 = FunctionToIntegrate(m_LowerBound);
                double f2 = FunctionToIntegrate(m - x1 * h);
                double f3 = FunctionToIntegrate(m - MathConsts.SqrtTwoOverThree * h);
                double f4 = FunctionToIntegrate(m - x2 * h);
                double f5 = FunctionToIntegrate(m - MathConsts.OneOverSqrt5 * h);
                double f6 = FunctionToIntegrate(m - x3 * h);
                double f7 = FunctionToIntegrate(m);
                double f8 = FunctionToIntegrate(m + x3 * h);
                double f9 = FunctionToIntegrate(m + MathConsts.OneOverSqrt5 * h);
                double f10 = FunctionToIntegrate(m + x2 * h);
                double f11 = FunctionToIntegrate(m + MathConsts.SqrtTwoOverThree * h);
                double f12 = FunctionToIntegrate(m + x1 * h);
                double f13 = FunctionToIntegrate(m_UpperBound);

                m_FunctionEvaluationsNeeded = 13;

                // calculate two approximations for the integral:
                double value1 = (h / 1470.0) * (77.0 * (f1 + f13) + 432.0 * (f3 + f11) + 625.0 * (f5 + f9) + 672.0 * f7);
                double value2 = (h / 6.0) * (f1 + f13 + 5.0 * (f5 + f9));

                // the estimation of 'Math.Abs(\int_a^b f(x) \; dx )'
                double modulusOfTheIntegral = h * (0.0158271919734802 * (f1 + f13) + 0.0942738402188500 * (f2 + f12) + 0.155071987336585 * (f3 + f11) + 0.188821573960182 * (f4 + f10) + 0.199773405226859 * (f5 + f9) + 0.224926465333340 * (f6 + f8) + 0.242611071901408 * f7);

                double absoluteDifference = Math.Abs(value2 - modulusOfTheIntegral);
                double magnitude = (absoluteDifference != 0) ? Math.Abs(value1 - modulusOfTheIntegral) / absoluteDifference : 0.0;

                double tolerance = m_IntegratorFactory.ExitCondition.Tolerance;
                if ((magnitude > 0) && (magnitude < 1))
                {
                    tolerance = tolerance / magnitude;
                }
                modulusOfTheIntegral = modulusOfTheIntegral * tolerance / MachineConsts.SuperTinyEpsilon;

                if (modulusOfTheIntegral == 0)
                {
                    modulusOfTheIntegral = m_UpperBound - m_LowerBound;
                }

                m_IterationsNeeded = 1;
                m_BenchmarkEstimation = 0.0;

                var state = NumericalIntegratorErrorClassification.NoResult;
                value = OneStepIntegration(m_LowerBound, m_UpperBound, f1, f13, modulusOfTheIntegral, ref state);

                return State.Create(state, m_BenchmarkEstimation, value, m_IterationsNeeded, m_FunctionEvaluationsNeeded);
            }

            /// <summary>Gets the value of the integral \int_a^b w(x) * f(x) dx.
            /// </summary>
            /// <returns>An approximation of the specific integral.
            /// </returns>
            public double GetValue()
            {
                double value;
                GetValue(out value);

                return value;
            }

            /// <summary>Sets the lower integration bound.
            /// </summary>
            /// <param name="lowerBound">The lower integration bound.</param>
            /// <returns>A value indicating whether <see cref="IOneDimNumericalIntegratorAlgorithm.LowerBound" /> has been set to <paramref name="lowerBound" />.
            /// </returns>
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
            /// <returns>A value indicating whether <see cref="IOneDimNumericalIntegratorAlgorithm.UpperBound" /> has been set to <paramref name="upperBound" />.
            /// </returns>
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

            #region private methods

            /// <summary>Numerically integrates the given function of one variable.
            /// </summary>
            /// <param name="lowerBound">The lower bound.</param>
            /// <param name="upperBound">The upper bound.</param>
            /// <param name="valueAtLowerBound">The value of <see cref="IOneDimNumericalIntegratorAlgorithm.FunctionToIntegrate"/> at <paramref name="lowerBound"/>.</param>
            /// <param name="valueAtUpperBound">The value of <see cref="IOneDimNumericalIntegratorAlgorithm.FunctionToIntegrate"/> at <paramref name="upperBound"/>.</param>
            /// <param name="modulusOfTheIntegral">The modulus of the integral, perhaps magnify with respect to the absolute error tolerance. </param>
            /// <param name="state">The state of the algorithm (input is the state in a previous iteration step).</param>
            /// <returns>An approximation of the specified integral from <paramref name="lowerBound"/> to <paramref name="upperBound"/>.
            /// </returns>
            private double OneStepIntegration(double lowerBound, double upperBound, double valueAtLowerBound, double valueAtUpperBound, double modulusOfTheIntegral, ref NumericalIntegratorErrorClassification state)
            {
                double h = (upperBound - lowerBound) / 2.0;
                double m = (lowerBound + upperBound) / 2.0;

                double mll = m - MathConsts.SqrtTwoOverThree * h;
                double fmll = FunctionToIntegrate(mll);
                double ml = m - MathConsts.OneOverSqrt5 * h;
                double fml = FunctionToIntegrate(ml);
                double fm = FunctionToIntegrate(m);
                double mr = m + MathConsts.OneOverSqrt5 * h;
                double fmr = FunctionToIntegrate(mr);
                double mrr = m + MathConsts.SqrtTwoOverThree * h;
                double fmrr = FunctionToIntegrate(mrr);

                m_FunctionEvaluationsNeeded += 5;

                // calculate two approximations for the sub-integral:
                double value1 = (h / 1470.0) * (77.0 * (valueAtLowerBound + valueAtUpperBound) + 432.0 * (fmll + fmrr) + 625.0 * (fml + fmr) + 672.0 * fm);
                double value2 = (h / 6.0) * (valueAtLowerBound + valueAtUpperBound + 5.0 * (fml + fmr));

                if ((state != NumericalIntegratorErrorClassification.ProperResult) && (state != NumericalIntegratorErrorClassification.NoResult))
                {
                    m_BenchmarkEstimation += value2;
                    return value1;  // the algorithm already fails
                }
                else
                {
                    if ((m <= lowerBound) || (upperBound <= m))
                    {
                        state = NumericalIntegratorErrorClassification.Divergent;
                        m_BenchmarkEstimation += value2;
                        return value1;
                    }
                    if (m_IterationsNeeded >= m_IntegratorFactory.ExitCondition.MaxIterations)
                    {
                        state = NumericalIntegratorErrorClassification.IterationLimitExceeded;
                        m_BenchmarkEstimation += value2;
                        return value1;
                    }
                    if (m_FunctionEvaluationsNeeded >= m_IntegratorFactory.ExitCondition.MaxEvaluations)
                    {
                        state = NumericalIntegratorErrorClassification.EvaluationLimitExceeded;
                        m_BenchmarkEstimation += value2;
                        return value1;
                    }

                    /* check whether the tolerance is reached: */
                    if (Double.IsNaN(m_IntegratorFactory.ExitCondition.Tolerance) == false)
                    {
                        // Attention! Here we compare two double values, see paper!
                        if ((modulusOfTheIntegral + (value1 - value2) == modulusOfTheIntegral) || (mll <= lowerBound) || (upperBound <= mrr))
                        {
                            if ((m <= lowerBound) || (upperBound <= m))  // Interval contains no more machine number. Required tolerance may not be met.
                            {
                                state = NumericalIntegratorErrorClassification.RoundOffError;
                            }
                            else
                            {
                                state = NumericalIntegratorErrorClassification.ProperResult;
                            }
                            m_BenchmarkEstimation += value2;
                            return value1;
                        }
                    }
                    m_IterationsNeeded++;
                    return OneStepIntegration(lowerBound, mll, valueAtLowerBound, fmll, modulusOfTheIntegral, ref state) +
                           OneStepIntegration(mll, ml, fmll, fml, modulusOfTheIntegral, ref state) +
                           OneStepIntegration(ml, m, fml, fm, modulusOfTheIntegral, ref state) +
                           OneStepIntegration(m, mr, fm, fmr, modulusOfTheIntegral, ref state) +
                           OneStepIntegration(mr, mrr, fmr, fmrr, modulusOfTheIntegral, ref state) +
                           OneStepIntegration(mrr, upperBound, fmrr, valueAtUpperBound, modulusOfTheIntegral, ref state);
                }
            }
            #endregion
        }
    }
}