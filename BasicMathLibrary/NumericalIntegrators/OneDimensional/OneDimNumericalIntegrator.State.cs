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
using System.Text;

using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.NumericalIntegrators
{
    public partial class OneDimNumericalIntegrator
    {
        /// <summary>Represents the state of a specific one-dimensional numerical integrator calculation.
        /// </summary>
        public class State : IInfoOutputQueriable
        {
            #region private members

            /// <summary>A delegate used to fill a specific <see cref="InfoOutputPackage"/> in <see cref="FillInfoOutput(InfoOutput, string)"/>.
            /// </summary>
            private Action<InfoOutputPackage> m_InfoOutputPackageAction;
            #endregion

            #region protected constructors

            /// <summary>Initializes a new instance of the <see cref="State"/> class.
            /// </summary>
            /// <param name="classification">The classification of the result.</param>
            /// <param name="infoOutputDetailLevel">The info output detail level.</param>
            protected internal State(NumericalIntegratorErrorClassification classification, InfoOutputDetailLevel infoOutputDetailLevel = InfoOutputDetailLevel.Full)
            {
                Classification = classification;
                InfoOutputDetailLevel = infoOutputDetailLevel;
                m_InfoOutputPackageAction = null;
            }

            /// <summary>Initializes a new instance of the <see cref="State"/> class.
            /// </summary>
            /// <param name="classification">The classification of the result.</param>
            /// <param name="infoOutputPackageAction">A method applied to fill a specific <see cref="InfoOutputPackage"/>.</param>
            /// <param name="infoOutputDetailLevel">The info output detail level.</param>
            protected internal State(NumericalIntegratorErrorClassification classification, Action<InfoOutputPackage> infoOutputPackageAction, InfoOutputDetailLevel infoOutputDetailLevel = InfoOutputDetailLevel.Full)
            {
                Classification = classification;
                InfoOutputDetailLevel = infoOutputDetailLevel;
                if (infoOutputPackageAction == null)
                {
                    throw new ArgumentNullException(nameof(infoOutputPackageAction));
                }
                m_InfoOutputPackageAction = infoOutputPackageAction;
            }
            #endregion

            #region public properties

            #region IInfoOutputQueriable Member

            /// <summary>Gets the info-output level of detail.
            /// </summary>
            /// <value>The info-output level of detail.</value>
            public InfoOutputDetailLevel InfoOutputDetailLevel
            {
                get;
                private set;
            }
            #endregion

            /// <summary>Gets the classification of the state, i.e. a value indicating whether the calculation returns a proper result.
            /// </summary>
            /// <value>The error classification.</value>
            public NumericalIntegratorErrorClassification Classification
            {
                get;
                private set;
            }
            #endregion

            #region public methods

            #region IInfoOutputQueriable Member

            /// <summary>Gets informations of the current object as a specific <see cref="T:Dodoni.BasicComponents.Containers.InfoOutput"/> instance.
            /// </summary>
            /// <param name="infoOutput">The <see cref="T:Dodoni.BasicComponents.Containers.InfoOutput"/> object which is to be filled with informations concering the current instance.</param>
            /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
            public virtual void FillInfoOutput(InfoOutput infoOutput, string categoryName = "General")
            {
                var infoOutputPackage = infoOutput.AcquirePackage(categoryName);
                infoOutputPackage.Add("Classification", Classification);

                if (m_InfoOutputPackageAction != null)
                {
                    m_InfoOutputPackageAction(infoOutputPackage);
                }
            }

            /// <summary>Sets the <see cref="P:Dodoni.BasicComponents.Containers.IInfoOutputQueriable.InfoOutputDetailLevel"/> property.
            /// </summary>
            /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
            /// <returns>A value indicating whether the <see cref="P:Dodoni.BasicComponents.Containers.IInfoOutputQueriable.InfoOutputDetailLevel"/> has been set to <paramref name="infoOutputDetailLevel"/>.
            /// </returns>
            public bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
            {
                return (infoOutputDetailLevel == InfoOutputDetailLevel);
            }
            #endregion

            /// <summary>Returns a <see cref="System.String" /> that represents this instance.
            /// </summary>
            /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
            public override string ToString()
            {
                if (m_InfoOutputPackageAction != null)
                {
                    var infoOutput = new InfoOutput();
                    var infoOutputPackage = infoOutput.AcquirePackage("General");
                    m_InfoOutputPackageAction(infoOutputPackage);

                    StringBuilder strBuilder = new StringBuilder();
                    strBuilder.AppendFormat("{0}, ", Classification);

                    foreach (var propertyCollection in infoOutputPackage.Properties)
                    {
                        foreach (var property in propertyCollection.Item2)
                        {
                            strBuilder.AppendFormat(";{0}: {1}", property.Name.String, property.Value);
                        }
                    }
                    return strBuilder.ToString();
                }
                return String.Format("{0}", Classification);
            }
            #endregion

            #region public static methods

            /// <summary>Creates a new <see cref="State"/> object.
            /// </summary>
            /// <param name="classification">The classification of the result.</param>
            /// <returns>A <see cref="State"/> object that represents the state of a specific calculation.</returns>
            public static State Create(NumericalIntegratorErrorClassification classification)
            {
                return new State(classification);
            }

            /// <summary>Creates a new <see cref="State"/> object.
            /// </summary>
            /// <param name="classification">The classification of the result.</param>
            /// <param name="previousEstimatedValue">A estimation of the numerical integration in a previous iteration step.</param>
            /// <param name="finalEstimatedValue">A estimation of the numerical integraton in the final iteration step.</param>
            /// <param name="iterationsNeeded">The number of iterations needed by the algorithm to reach the desired accuracy.</param>
            /// <returns>A <see cref="State"/> object that represents the state of a specific calculation.</returns>
            public static State Create(NumericalIntegratorErrorClassification classification, double previousEstimatedValue = Double.NaN, double finalEstimatedValue = Double.NaN, int iterationsNeeded = Int32.MaxValue)
            {
                double estimatedAbsoluteError = Math.Abs(previousEstimatedValue - finalEstimatedValue);

                double estimatedRelativeError = estimatedAbsoluteError;
                if (Math.Abs(previousEstimatedValue) > 0.0)
                {
                    estimatedRelativeError /= Math.Abs(previousEstimatedValue);
                }

                return new State(classification, (infoOutputPackage) =>
                {
                    infoOutputPackage.Add("Estimated absolute error", estimatedAbsoluteError);
                    infoOutputPackage.Add("Estimated relative error", estimatedRelativeError);
                    infoOutputPackage.Add("Iterations needed", iterationsNeeded);
                });
            }

            /// <summary>Creates a new <see cref="State"/> object.
            /// </summary>
            /// <param name="classification">The classification of the result.</param>
            /// <param name="previousEstimatedValue">A estimation of the numerical integration in a previous iteration step.</param>
            /// <param name="finalEstimatedValue">A estimation of the numerical integraton in the final iteration step.</param>
            /// <param name="iterationsNeeded">The number of iterations needed by the algorithm to reach the desired accuracy.</param>
            /// <param name="evaluationsNeeded">The number of function evaluations needed by the algorithm to reach the desired accuracy.</param>
            /// <returns>A <see cref="State"/> object that represents the state of a specific calculation.</returns>
            public static State Create(NumericalIntegratorErrorClassification classification, double previousEstimatedValue = Double.NaN, double finalEstimatedValue = Double.NaN, int iterationsNeeded = Int32.MaxValue, long evaluationsNeeded = Int64.MaxValue)
            {
                double estimatedAbsoluteError = Math.Abs(previousEstimatedValue - finalEstimatedValue);

                double estimatedRelativeError = estimatedAbsoluteError;
                if (Math.Abs(previousEstimatedValue) > 0.0)
                {
                    estimatedRelativeError /= Math.Abs(previousEstimatedValue);
                }

                return new State(classification, (infoOutputPackage) =>
                {
                    infoOutputPackage.Add("Estimated absolute error", estimatedAbsoluteError);
                    infoOutputPackage.Add("Estimated relative error", estimatedRelativeError);
                    infoOutputPackage.Add("Iterations needed", iterationsNeeded);
                    infoOutputPackage.Add("Evaluations needed", evaluationsNeeded);
                });
            }

            /// <summary>Performs an implicit conversion from <see cref="Dodoni.MathLibrary.NumericalIntegrators.OneDimNumericalIntegrator.State"/> to <see cref="Dodoni.MathLibrary.NumericalIntegrators.NumericalIntegratorErrorClassification"/>.
            /// </summary>
            /// <param name="state">The state.</param>
            /// <returns>The result of the conversion.</returns>
            public static implicit operator NumericalIntegratorErrorClassification(State state)
            {
                if (state == null)
                {
                    throw new ArgumentNullException(nameof(state));
                }
                return state.Classification;
            }
            #endregion
        }
    }
}