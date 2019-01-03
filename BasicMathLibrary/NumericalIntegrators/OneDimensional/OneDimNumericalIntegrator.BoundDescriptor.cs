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
    public abstract partial class OneDimNumericalIntegrator
    {
        /// <summary>Serves as description of the lower integration bound, upper integration bound respectively of a specific numerical integration algorithm.
        /// </summary>
        public class BoundDescriptor : IInfoOutputQueriable
        {
            #region public (readonly) members

            /// <summary>The numerical integration algorithm may evaluate the function to integrate at the (lower, upper respectively) bound; moreover the bound can be changed.
            /// </summary>
            public static readonly BoundDescriptor Closed;

            /// <summary>The numerical integration algorithm does not evaluate the function to integrate at the (lower, upper respectively) bound; moreover the bound can be changed.
            /// </summary>
            public static readonly BoundDescriptor Open;
            #endregion

            #region private members

            /// <summary>Contains the value of the integration bound if the bound can not be changed by the user.
            /// </summary>
            private double m_ReadOnlyBoundValue;
            #endregion

            #region public constructors

            /// <summary>Initializes a new instance of the <see cref="BoundDescriptor" /> class.
            /// </summary>
            /// <param name="evaluationType">A value indicating whether the function to integrate will evaluate at the boundary of the integration region.</param>
            public BoundDescriptor(BoundEvaluationType evaluationType)
            {
                EvaluationType = evaluationType;
                IsConstant = false;
                m_ReadOnlyBoundValue = Double.NaN;
            }

            /// <summary>Initializes a new instance of the <see cref="BoundDescriptor" /> class.
            /// </summary>
            /// <param name="evaluationType">A value indicating whether the function to integrate will evaluate at the boundary of the integration region.</param>
            /// <param name="readOnlyBoundValue">The bound can not be changed; this argument contains the value of the lower bound, upper bound respectively.</param>
            public BoundDescriptor(BoundEvaluationType evaluationType, double readOnlyBoundValue)
            {
                EvaluationType = evaluationType;
                IsConstant = true;
                m_ReadOnlyBoundValue = readOnlyBoundValue;
            }
            #endregion

            #region static constructor

            /// <summary>Initializes the <see cref="BoundDescriptor" /> class.
            /// </summary>
            static BoundDescriptor()
            {
                Closed = new BoundDescriptor(BoundEvaluationType.Closed);
                Open = new BoundDescriptor(BoundEvaluationType.Open);
            }
            #endregion

            #region public properties

            #region IInfoOutputQueriable Members

            /// <summary>Gets the info-output level of detail.
            /// </summary>
            /// <value>The info-output level of detail.</value>
            public InfoOutputDetailLevel InfoOutputDetailLevel
            {
                get { return InfoOutputDetailLevel.Full; }
            }
            #endregion

            /// <summary>Gets a value indicating whether <see cref="IOneDimNumericalIntegratorAlgorithm.FunctionToIntegrate"/> will be evaluate at <see cref="IOneDimNumericalIntegratorAlgorithm.LowerBound"/>, <see cref="IOneDimNumericalIntegratorAlgorithm.UpperBound"/> repectively.
            /// </summary>
            /// <value>A value indicating whether <see cref="IOneDimNumericalIntegratorAlgorithm.FunctionToIntegrate"/> will be evaluate at <see cref="IOneDimNumericalIntegratorAlgorithm.LowerBound"/>, <see cref="IOneDimNumericalIntegratorAlgorithm.UpperBound"/> repectively.</value>
            public BoundEvaluationType EvaluationType
            {
                get;
                private set;
            }

            /// <summary>Gets a value indicating whether the specified integration bound is constant and can not be changed by the user.
            /// </summary>
            /// <value><c>true</c> if the specified integration bound is constant and can not be changed by the user; otherwise, <c>false</c>.</value>
            public bool IsConstant
            {
                get;
                private set;
            }
            #endregion

            #region public methods

            #region IInfoOutputQueriable Members

            /// <summary>Sets the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> property.
            /// </summary>
            /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
            /// <returns>A value indicating whether the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> has been set to <paramref name="infoOutputDetailLevel" />.</returns>
            public bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
            {
                return (infoOutputDetailLevel == InfoOutputDetailLevel.Full);
            }

            /// <summary>Gets informations of the current object as a specific <see cref="InfoOutput" /> instance.
            /// </summary>
            /// <param name="infoOutput">The <see cref="InfoOutput" /> object which is to be filled with informations concering the current instance.</param>
            /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
            public void FillInfoOutput(InfoOutput infoOutput, string categoryName = InfoOutput.GeneralCategoryName)
            {
                var infoOutputPackage = infoOutput.AcquirePackage(categoryName);

                infoOutputPackage.Add("Evaluation type", EvaluationType);
                infoOutputPackage.Add("Is constant", IsConstant);
                if (IsConstant == true)
                {
                    infoOutputPackage.Add("Value", m_ReadOnlyBoundValue);
                }
            }
            #endregion

            /// <summary>Returns a <see cref="System.String" /> that represents this instance.
            /// </summary>
            /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
            public override string ToString()
            {
                if (IsConstant == false)
                {
                    return String.Format("Evaluation type {0}", EvaluationType);
                }
                return String.Format("Evaluation type {0}; constant value: {1}", EvaluationType, m_ReadOnlyBoundValue);
            }
            #endregion

            #region public static methods

            /// <summary>Creates a specific <see cref="BoundDescriptor"/> object.
            /// </summary>
            /// <param name="evaluationType">A value indicating whether the function to integrate will evaluate at the boundary of the integration region.</param>
            /// <returns>A specific <see cref="BoundDescriptor"/> object.</returns>
            public static BoundDescriptor Create(BoundEvaluationType evaluationType)
            {
                return new BoundDescriptor(evaluationType);
            }

            /// <summary>Creates a specific <see cref="BoundDescriptor"/> object.
            /// </summary>
            /// <param name="evaluationType">A value indicating whether the function to integrate will evaluate at the boundary of the integration region.</param>
            /// <param name="readOnlyBoundValue">The bound can not be changed; this argument contains the value of the lower bound, upper bound respectively.</param>
            /// <returns>A specific <see cref="BoundDescriptor"/> object.</returns>
            public static BoundDescriptor Create(BoundEvaluationType evaluationType, double readOnlyBoundValue)
            {
                return new BoundDescriptor(evaluationType, readOnlyBoundValue);
            }
            #endregion
        }
    }
}