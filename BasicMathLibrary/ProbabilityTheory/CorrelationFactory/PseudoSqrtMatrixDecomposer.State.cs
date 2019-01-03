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
using System.Data;
using System.Collections.Generic;

using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.ProbabilityTheory
{
    public abstract partial class PseudoSqrtMatrixDecomposer
    {
        /// <summary>Represents the state of a specific correlation matrix calculation.
        /// </summary>
        public class State : IInfoOutputQueriable
        {
            #region private members

            /// <summary>A delegate for the <see cref="FillInfoOutput(InfoOutput, string)"/> method.
            /// </summary>
            private Action<InfoOutput, string> m_FillInfoOutput;

            /// <summary>The <see cref="System.String"/> representation of the current instance.
            /// </summary>
            private string m_StringRepresentation;
            #endregion

            #region protected internal constructors

            /// <summary>Initializes a new instance of the <see cref="State"/> class.
            /// </summary>
            /// <param name="rank">The rank of the decomposed correlation matrix, i.e. of matrix B where B * B^t represents the correlation matrix.</param>
            /// <param name="iterationsNeeded">The number of iterations needed by the algorithm to reach the desired accuracy.</param>
            /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
            protected internal State(int rank, int iterationsNeeded = Int32.MaxValue, InfoOutputDetailLevel infoOutputDetailLevel = InfoOutputDetailLevel.Low)
            {
                IterationsNeeded = iterationsNeeded;
                Rank = rank;
                InfoOutputDetailLevel = infoOutputDetailLevel;

                var strBuilder = new StringBuilder();
                strBuilder.AppendFormat("Rank: {0}", Rank);

                if (iterationsNeeded < Int32.MaxValue)
                {
                    strBuilder.AppendFormat("; Iterations needed: {0}", iterationsNeeded);
                }
                m_StringRepresentation = strBuilder.ToString();

                m_FillInfoOutput = (infoOutput, categoryName) =>
                {
                    var infoOutputPackage = infoOutput.AcquirePackage(categoryName);
                    infoOutputPackage.Add("Rank", Rank);

                    if (iterationsNeeded < Int32.MaxValue)
                    {
                        infoOutputPackage.Add("Iterations needed", IterationsNeeded);
                    }
                };
            }

            /// <summary>Initializes a new instance of the <see cref="State"/> class.
            /// </summary>
            /// <param name="rank">The rank of the decomposed correlation matrix, i.e. of matrix B where B * B^t represents the correlation matrix.</param>
            /// <param name="details">Additional details in its <see cref="InfoOutputProperty"/> representation.</param>
            /// <param name="iterationsNeeded">The number of iterations needed by the algorithm to reach the desired accuracy.</param>
            /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
            protected internal State(int rank, IEnumerable<InfoOutputProperty> details, int iterationsNeeded = Int32.MaxValue, InfoOutputDetailLevel infoOutputDetailLevel = InfoOutputDetailLevel.High)
            {
                IterationsNeeded = iterationsNeeded;
                Rank = rank;
                InfoOutputDetailLevel = infoOutputDetailLevel;

                var strBuilder = new StringBuilder();
                strBuilder.AppendFormat("Rank: {0}", Rank);

                if (iterationsNeeded < Int32.MaxValue)
                {
                    strBuilder.AppendFormat("; Iterations needed: {0}", iterationsNeeded);
                }
                if (details != null)
                {
                    foreach (var property in details)
                    {
                        strBuilder.AppendFormat("; {0}: {1}", property.Name.String, property.Value);
                    }
                }
                m_StringRepresentation = strBuilder.ToString();


                m_FillInfoOutput = (infoOutput, categoryName) =>
                {
                    var infoOutputPackage = infoOutput.AcquirePackage(categoryName);
                    infoOutputPackage.Add("Rank", Rank);

                    if (iterationsNeeded < Int32.MaxValue)
                    {
                        infoOutputPackage.Add("Iterations needed", IterationsNeeded);
                    }
                    if (details != null)
                    {
                        foreach (var property in details)
                        {
                            infoOutputPackage.Add(property);
                        }
                    }
                };
            }

            /// <summary>Initializes a new instance of the <see cref="State"/> class.
            /// </summary>
            /// <param name="rank">The rank of the decomposed correlation matrix, i.e. of matrix B where B * B^t represents the correlation matrix.</param>
            /// <param name="details">Additional details in its <see cref="IInfoOutputQueriable"/> representation together with a specific name.</param>
            /// <param name="iterationsNeeded">The number of iterations needed by the algorithm to reach the desired accuracy.</param>
            /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
            protected internal State(int rank, IEnumerable<Tuple<string, IInfoOutputQueriable>> details, int iterationsNeeded = Int32.MaxValue, InfoOutputDetailLevel infoOutputDetailLevel = InfoOutputDetailLevel.High)
            {
                IterationsNeeded = iterationsNeeded;
                Rank = rank;
                InfoOutputDetailLevel = infoOutputDetailLevel;

                var strBuilder = new StringBuilder();
                strBuilder.AppendFormat("Rank: {0}", Rank);

                if (iterationsNeeded < Int32.MaxValue)
                {
                    strBuilder.AppendFormat("; Iterations needed: {0}", iterationsNeeded);
                }

                if (details != null)
                {
                    foreach (var item in details)
                    {
                        if (item != null)
                        {
                            strBuilder.AppendFormat("; <{0}: {1}>", item.Item1, item.Item2.ToString());
                        }
                    }
                }
                m_StringRepresentation = strBuilder.ToString();

                m_FillInfoOutput = (infoOutput, categoryName) =>
                {
                    var infoOutputPackage = infoOutput.AcquirePackage(categoryName);
                    infoOutputPackage.Add("Rank", Rank);

                    if (iterationsNeeded < Int32.MaxValue)
                    {
                        infoOutputPackage.Add("Iterations needed", IterationsNeeded);
                    }
                    if (details != null)
                    {
                        foreach (var item in details)
                        {
                            if (item != null)
                            {
                                item.Item2.FillInfoOutput(infoOutput, item.Item1);
                            }
                        }
                    }
                };
            }

            /// <summary>Initializes a new instance of the <see cref="State"/> class.
            /// </summary>
            /// <param name="rank">The rank of the decomposed correlation matrix, i.e. of matrix B where B * B^t represents the correlation matrix.</param>
            /// <param name="detailProperties">Additional details in its <see cref="InfoOutputProperty"/> representation.</param>
            /// <param name="detailDataTables">Additional details in its <see cref="DataTable"/> representation; <c>null</c> entries will be ignored.</param>
            /// <param name="iterationsNeeded">The number of iterations needed by the algorithm to reach the desired accuracy.</param>
            /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
            protected internal State(int rank, IList<InfoOutputProperty> detailProperties, IList<DataTable> detailDataTables, int iterationsNeeded, InfoOutputDetailLevel infoOutputDetailLevel = InfoOutputDetailLevel.High)
            {
                IterationsNeeded = iterationsNeeded;
                Rank = rank;
                InfoOutputDetailLevel = infoOutputDetailLevel;

                var strBuilder = new StringBuilder();
                strBuilder.AppendFormat("Rank: {0}", Rank);

                if (iterationsNeeded < Int32.MaxValue)
                {
                    strBuilder.AppendFormat("; Iterations needed: {0}", iterationsNeeded);
                }

                if (detailProperties != null)
                {
                    foreach (var property in detailProperties)
                    {
                        strBuilder.AppendFormat("; {0}: {1}", property.Name.String, property.Value);
                    }
                }
                m_StringRepresentation = strBuilder.ToString();

                m_FillInfoOutput = (infoOutput, categoryName) =>
                {
                    var infoOutputPackage = infoOutput.AcquirePackage(categoryName);
                    infoOutputPackage.Add("Rank", Rank);

                    if (iterationsNeeded < Int32.MaxValue)
                    {
                        infoOutputPackage.Add("Iterations needed", IterationsNeeded);
                    }
                    if (detailProperties != null)
                    {
                        foreach (var property in detailProperties)
                        {
                            infoOutputPackage.Add(property);
                        }
                    }
                    if (detailDataTables != null)
                    {
                        foreach (var dataTable in detailDataTables)
                        {
                            if (dataTable != null)
                            {
                                infoOutputPackage.Add(dataTable);
                            }
                        }
                    }
                };
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

            /// <summary>Gets the rank of the decomposed correlation matrix, i.e. of matrix B where B * B^t represents the correlation matrix.
            /// </summary>
            /// <value>The rank of the decomposed correlation matrix, i.e. of matrix B where B * B^t represents the correlation matrix.</value>
            public int Rank
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
                m_FillInfoOutput(infoOutput, categoryName);
            }

            /// <summary>Sets the <see cref="P:Dodoni.BasicComponents.Containers.IInfoOutputQueriable.InfoOutputDetailLevel"/> property.
            /// </summary>
            /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
            /// <returns>A value indicating whether the <see cref="P:Dodoni.BasicComponents.Containers.IInfoOutputQueriable.InfoOutputDetailLevel"/> has been set to <paramref name="infoOutputDetailLevel"/>.</returns>
            public virtual bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
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
            /// <param name="rank">The rank of the decomposed correlation matrix, i.e. of matrix B where B * B^t represents the correlation matrix.</param>
            /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
            /// <returns>A <see cref="State"/> object that represents the state of a specific calculation.</returns>
            public static State Create(int rank, InfoOutputDetailLevel infoOutputDetailLevel = InfoOutputDetailLevel.Low)
            {
                return new State(rank, infoOutputDetailLevel: infoOutputDetailLevel);
            }

            /// <summary>Creates a new <see cref="State"/> object.
            /// </summary>
            /// <param name="rank">The rank of the decomposed correlation matrix, i.e. of matrix B where B * B^t represents the correlation matrix.</param>
            /// <param name="iterationsNeeded">The number of iterations needed by the algorithm to reach the desired accuracy.</param>
            /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
            /// <param name="details">Additional details in its <see cref="InfoOutputProperty"/> representation.</param>
            /// <returns>A <see cref="State"/> object that represents the state of a specific calculation.</returns>
            public static State Create(int rank, int iterationsNeeded = Int32.MaxValue, InfoOutputDetailLevel infoOutputDetailLevel = InfoOutputDetailLevel.High, params InfoOutputProperty[] details)
            {
                return new State(rank, details, iterationsNeeded, infoOutputDetailLevel);
            }

            /// <summary>Creates a new <see cref="State"/> object.
            /// </summary>
            /// <param name="rank">The rank of the decomposed correlation matrix, i.e. of matrix B where B * B^t represents the correlation matrix.</param>
            /// <param name="detailProperties">Additional details in its  <see cref="InfoOutputProperty"/> representation.</param>
            /// <param name="detailDataTables">Additional details in its <see cref="DataTable"/> representation; <c>null</c> entries will be ignored.</param>
            /// <param name="iterationsNeeded">The number of iterations needed by the algorithm to reach the desired accuracy.</param>
            /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
            /// <returns>A <see cref="State"/> object that represents the state of a specific calculation.</returns>
            public static State Create(int rank, IList<InfoOutputProperty> detailProperties, IList<DataTable> detailDataTables, int iterationsNeeded = Int32.MaxValue, InfoOutputDetailLevel infoOutputDetailLevel = InfoOutputDetailLevel.High)
            {
                return new State(rank, detailProperties, detailDataTables, iterationsNeeded, infoOutputDetailLevel);
            }

            /// <summary>Creates a new <see cref="State"/> object.
            /// </summary>
            /// <param name="rank">The rank of the decomposed correlation matrix, i.e. of matrix B where B * B^t represents the correlation matrix.</param>
            /// <param name="iterationsNeeded">The number of iterations needed by the algorithm to reach the desired accuracy.</param>
            /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
            /// <param name="details">Additional details in its <see cref="IInfoOutputQueriable"/> representation together with a specific name.</param>
            /// <returns>A <see cref="State"/> object that represents the state of a specific calculation.</returns>
            public static State Create(int rank, int iterationsNeeded = Int32.MaxValue, InfoOutputDetailLevel infoOutputDetailLevel = InfoOutputDetailLevel.High, params Tuple<string, IInfoOutputQueriable>[] details)
            {
                return new State(rank, details, iterationsNeeded, infoOutputDetailLevel);
            }
            #endregion
        }
    }
}