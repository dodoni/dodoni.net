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
using System.Collections.Generic;

using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.Optimizer.OneDimensional
{
    public partial class OneDimOptimizer
    {
        /// <summary>Represents the state of a specific 1-dimensional optimization (minimization) calculation.
        /// </summary>
        public class State : IInfoOutputQueriable
        {
            #region private members

            /// <summary>A delegate used to fill a specific <see cref="InfoOutputPackage"/> in <see cref="FillInfoOutput(InfoOutput, string)"/>.
            /// </summary>
            private Action<InfoOutputPackage> m_InfoOutputPackageAction = null;

            /// <summary>The <see cref="System.String"/> representation of the current instance.
            /// </summary>
            private string m_StringRepresentation;
            #endregion

            #region protected constructors

            /// <summary>Initializes a new instance of the <see cref="State"/> class.
            /// </summary>
            /// <param name="classification">The classification of the result.</param>
            /// <param name="evaluationsNeeded">The number of function evaluations needed by the algorithm to reach the desired accuracy.</param>
            /// <param name="iterationsNeeded">The number of iterations needed by the algorithm to reach the desired accuracy.</param>
            protected internal State(OptimizerErrorClassification classification, int evaluationsNeeded = Int32.MaxValue, int iterationsNeeded = Int32.MaxValue)
            {
                Classification = classification;
                EvaluationsNeeded = evaluationsNeeded;
                IterationsNeeded = iterationsNeeded;
                InfoOutputDetailLevel = InfoOutputDetailLevel.Full;

                var strBuilder = new StringBuilder();
                strBuilder.AppendFormat("{0}", Classification);

                if (iterationsNeeded < Int32.MaxValue)
                {
                    strBuilder.AppendFormat("; Iterations needed: {0}", iterationsNeeded);
                }
                if (evaluationsNeeded < Int32.MaxValue)
                {
                    strBuilder.AppendFormat("; Evaluations needed: {0}", evaluationsNeeded);
                }
                m_StringRepresentation = strBuilder.ToString();
                m_InfoOutputPackageAction = null;
            }

            /// <summary>Initializes a new instance of the <see cref="State"/> class.
            /// </summary>
            /// <param name="classification">The classification of the result.</param>
            /// <param name="details">Additional details in its <see cref="InfoOutputProperty"/> representation.</param>
            /// <param name="evaluationsNeeded">The number of function evaluations needed by the algorithm to reach the desired accuracy.</param>
            /// <param name="iterationsNeeded">The number of iterations needed by the algorithm to reach the desired accuracy.</param>
            protected internal State(OptimizerErrorClassification classification, IEnumerable<InfoOutputProperty> details, int evaluationsNeeded = Int32.MaxValue, int iterationsNeeded = Int32.MaxValue)
            {
                Classification = classification;
                EvaluationsNeeded = evaluationsNeeded;
                IterationsNeeded = iterationsNeeded;
                InfoOutputDetailLevel = InfoOutputDetailLevel.Full;

                var strBuilder = new StringBuilder();
                strBuilder.AppendFormat("{0}", Classification);

                if (details != null)
                {
                    foreach (var property in details)
                    {
                        strBuilder.AppendFormat("; {0}: {1}", property.Name.String, property.Value);
                    }

                    m_InfoOutputPackageAction = infoOutputPackage =>
                    {
                        foreach (var property in details)
                        {
                            infoOutputPackage.Add(property);
                        }
                    };
                }
                if (iterationsNeeded < Int32.MaxValue)
                {
                    strBuilder.AppendFormat("; Iterations needed: {0}", iterationsNeeded);
                }
                if (evaluationsNeeded < Int32.MaxValue)
                {
                    strBuilder.AppendFormat("; Evaluations needed: {0}", evaluationsNeeded);
                }
                m_StringRepresentation = strBuilder.ToString();
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
            public OptimizerErrorClassification Classification
            {
                get;
                private set;
            }

            /// <summary>Gets the number of function evaluations needed by the algorithm to reach the desired accuracy.
            /// </summary>
            /// <value>The number of function evaluations needed by the algorithm to reach the desired accuracy.</value>
            public int EvaluationsNeeded
            {
                get;
                private set;
            }

            /// <summary>Gets the number of iterations needed by the algorithm to reach the desired accuracy.
            /// </summary>
            /// <value>The number of iterations needed by the algorithm to reach the desired accuracy.</value>
            public int IterationsNeeded
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
                if (IterationsNeeded < Int32.MaxValue)
                {
                    infoOutputPackage.Add("Iterations needed", IterationsNeeded);
                }
                if (EvaluationsNeeded < Int32.MaxValue)
                {
                    infoOutputPackage.Add("Evaluations needed", EvaluationsNeeded);
                }
                if (m_InfoOutputPackageAction != null)
                {
                    m_InfoOutputPackageAction(infoOutputPackage);
                }
            }

            /// <summary>Sets the <see cref="P:Dodoni.BasicComponents.Containers.IInfoOutputQueriable.InfoOutputDetailLevel"/> property.
            /// </summary>
            /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
            /// <returns>A value indicating whether the <see cref="P:Dodoni.BasicComponents.Containers.IInfoOutputQueriable.InfoOutputDetailLevel"/> has been set to <paramref name="infoOutputDetailLevel"/>.</returns>
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
                return m_StringRepresentation;
            }
            #endregion

            #region public static methods

            /// <summary>Creates a new <see cref="State"/> object.
            /// </summary>
            /// <param name="classification">The classification of the result.</param>
            /// <returns>A <see cref="State"/> object that represents the state of a specific calculation.</returns>
            public static State Create(OptimizerErrorClassification classification)
            {
                return new State(classification);
            }

            /// <summary>Creates a new <see cref="State"/> object.
            /// </summary>
            /// <param name="classification">The classification of the result.</param>
            /// <param name="evaluationsNeeded">The number of function evaluations needed by the algorithm to reach the desired accuracy.</param>
            /// <param name="iterationsNeeded">The number of iterations needed by the algorithm to reach the desired accuracy.</param>
            /// <param name="details">Additional details in its <see cref="InfoOutputProperty"/> representation.</param>
            /// <returns>A <see cref="State"/> object that represents the state of a specific calculation.</returns>
            public static State Create(OptimizerErrorClassification classification, int evaluationsNeeded = Int32.MaxValue, int iterationsNeeded = Int32.MaxValue, params InfoOutputProperty[] details)
            {
                return new State(classification, details, evaluationsNeeded, iterationsNeeded);
            }

            /// <summary>Creates a new <see cref="State"/> object.
            /// </summary>
            /// <param name="classification">The classification of the result.</param>
            /// <param name="argMin">The estimated argument minium of the specific algorithm.</param>
            /// <param name="minimum">The estimated minimum of the specific algorithm.</param>
            /// <param name="evaluationsNeeded">The number of function evaluations needed by the algorithm to reach the desired accuracy.</param>
            /// <param name="iterationsNeeded">The number of iterations needed by the algorithm to reach the desired accuracy.</param>
            /// <param name="details">Additional details in its <see cref="InfoOutputProperty"/> representation.</param>
            /// <returns>A <see cref="State"/> object that represents the state of a specific calculation.</returns>
            public static State Create(OptimizerErrorClassification classification, double argMin, double minimum, int evaluationsNeeded = Int32.MaxValue, int iterationsNeeded = Int32.MaxValue, params InfoOutputProperty[] details)
            {
                var properties = new List<InfoOutputProperty>() {
                    InfoOutputProperty.Create("ArgMin", argMin),
                    InfoOutputProperty.Create("Minimum", minimum) };
                if (details != null)
                {
                    properties.AddRange(details);
                }
                return new State(classification, properties, evaluationsNeeded, iterationsNeeded);
            }

            /// <summary>Performs an implicit conversion from <see cref="Dodoni.MathLibrary.Optimizer.OneDimensional.OneDimOptimizer.State"/> to <see cref="Dodoni.MathLibrary.Optimizer.OptimizerErrorClassification"/>.
            /// </summary>
            /// <param name="state">The state.</param>
            /// <returns>The result of the conversion.</returns>
            public static implicit operator OptimizerErrorClassification(State state)
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