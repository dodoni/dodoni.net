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

namespace Dodoni.MathLibrary.Native.NLopt
{
    /// <summary>Serves as abort (stopping) condition for a specific NLopt optimization algorithm.
    /// </summary>
    public class NLoptAbortCondition : IInfoOutputQueriable
    {
        #region private members

        /// <summary>A delegate to set the abort (stopping) condition in a specific NLopt algorithm represented by a specific <see cref="NLoptPtr"/> object.
        /// </summary>
        private Action<NLoptPtr> m_ApplyToNLoptPtr;

        /// <summary>A delegate used to fill a specific <see cref="InfoOutputPackage"/> object.
        /// </summary>
        private Action<InfoOutputPackage> m_InfoOutputAction;

        /// <summary>The <see cref="System.String"/> representation of the current object.
        /// </summary>
        private string m_StringRepresentation;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="NLoptAbortCondition" /> class.
        /// </summary>
        /// <param name="absoluteXTolerance">The absolute x-tolerance.</param>
        /// <param name="relativeXTolerance">The relative x-tolerance.</param>
        /// <param name="absoluteFTolerance">The absolute f-tolerance.</param>
        /// <param name="relativeFTolerance">The relative f-tolerance.</param>
        /// <param name="maxEvaluations">The maximal number of function evaluations.</param>
        /// <param name="maxEvaluationTime">The maximal evaluation time in seconds.</param>
        public NLoptAbortCondition(double absoluteXTolerance = MachineConsts.Epsilon, double relativeXTolerance = MachineConsts.Epsilon, double absoluteFTolerance = MachineConsts.Epsilon, double relativeFTolerance = MachineConsts.Epsilon, int maxEvaluations = Int32.MaxValue, int maxEvaluationTime = Int32.MaxValue)
        {
            if (Double.IsNaN(absoluteXTolerance) && Double.IsNaN(relativeXTolerance) && Double.IsNaN(absoluteFTolerance) && Double.IsNaN(relativeFTolerance) && (maxEvaluations == Int32.MaxValue) && (maxEvaluationTime == Int32.MaxValue))
            {
                throw new ArgumentException("No valid abort condition for NLopt optimization algorithm provided.");
            }

            m_ApplyToNLoptPtr = nloptPtr =>
            {
                if (Double.IsNaN(absoluteXTolerance) == false)
                {
                    nloptPtr.TrySetAbsoluteXTolerance(absoluteXTolerance);
                }
                if (Double.IsNaN(relativeXTolerance) == false)
                {
                    nloptPtr.TrySetRelativeXTolerance(relativeXTolerance);
                }
                if (Double.IsNaN(absoluteFTolerance) == false)
                {
                    nloptPtr.TrySetAbsoluteFValueTolerance(absoluteFTolerance);
                }
                if (Double.IsNaN(relativeFTolerance) == false)
                {
                    nloptPtr.TrySetRelativeFValueTolerance(relativeFTolerance);
                }
                if (maxEvaluations < Int32.MaxValue)
                {
                    nloptPtr.TrySetMaxEvaluations(maxEvaluations);
                }
                if (maxEvaluationTime < Int32.MaxValue)
                {
                    nloptPtr.TrySetMaxEvaluationTime(maxEvaluationTime);
                }
            };

            m_InfoOutputAction = infoOutputPackage =>
                {
                    infoOutputPackage.Add("Absolute x-tolerance", absoluteXTolerance);
                    infoOutputPackage.Add("Relative x-tolerance", relativeXTolerance);
                    infoOutputPackage.Add("Absolute f-tolerance", absoluteFTolerance);
                    infoOutputPackage.Add("Relative f-tolerance", relativeFTolerance);
                    infoOutputPackage.Add("Max evaluations", maxEvaluations);
                    infoOutputPackage.Add("Max evaluation time", maxEvaluationTime);
                };

            m_StringRepresentation = String.Format("Absolute x-tolerance: {0}; Relative x-tolerance: {1}; Absolute f-tolerance: {2}; Relative f-tolerance: {3}, Max Evaluations: {4}; Max Evaluation time: {5}", absoluteXTolerance, relativeXTolerance, absoluteFTolerance, relativeFTolerance, maxEvaluations, maxEvaluationTime);
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
            m_InfoOutputAction(infoOutputPackage);
        }
        #endregion

        /// <summary>Applies the abort (stopping) condition represented by the current instance to a specific NLopt algorithm in its <see cref="NLoptPtr"/> representation.
        /// </summary>
        /// <param name="nloptPtr">The NLopt algorithm in its <see cref="NLoptPtr"/> representation to apply the abort (stopping) condition.</param>
        public void ApplyTo(NLoptPtr nloptPtr)
        {
            m_ApplyToNLoptPtr(nloptPtr);
        }

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return m_StringRepresentation;
        }
        #endregion

        #region public static methods

        /// <summary>Creates a new <see cref="NLoptAbortCondition"/> object.
        /// </summary>
        /// <param name="absoluteXTolerance">The absolute x-tolerance.</param>
        /// <param name="relativeXTolerance">The relative x-tolerance.</param>
        /// <param name="absoluteFTolerance">The absolute f-tolerance.</param>
        /// <param name="relativeFTolerance">The relative f-tolerance.</param>
        /// <param name="maxEvaluations">The maximal number of function evaluations.</param>
        /// <param name="maxEvaluationTime">The maximal evaluation time in seconds.</param>
        /// <returns>A specific <see cref="NLoptAbortCondition"/> object.</returns>
        public static NLoptAbortCondition Create(double absoluteXTolerance = MachineConsts.Epsilon, double relativeXTolerance = MachineConsts.Epsilon, double absoluteFTolerance = MachineConsts.Epsilon, double relativeFTolerance = MachineConsts.Epsilon, int maxEvaluations = Int32.MaxValue, int maxEvaluationTime = Int32.MaxValue)
        {
            return new NLoptAbortCondition(absoluteXTolerance, relativeXTolerance, absoluteFTolerance, relativeFTolerance, maxEvaluations, maxEvaluationTime);
        }
        #endregion
    }
}