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

namespace Dodoni.MathLibrary.Optimizer.MultiDimensional
{
    /// <summary>Serves as abort (stopping) condition for the Levenberg-Marquardt algorithm.
    /// </summary>
    public class LevenbergMarquardtAbortCondition : IInfoOutputQueriable
    {
        #region private members

        /// <summary>A delegate used to fill a specific <see cref="InfoOutputPackage"/> object.
        /// </summary>
        private Action<InfoOutputPackage> m_InfoOutputAction;

        /// <summary>The <see cref="System.String"/> representation of the current object.
        /// </summary>
        private string m_StringRepresentation;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="LevenbergMarquardtAbortCondition" /> class.
        /// </summary>
        /// <param name="maxIterations">The number of repetitions of the algorithm, perhaps <see cref="System.Int32.MaxValue"/>.</param>
        /// <param name="maxEvaluations">The maximal number of function evaluations, perhaps <see cref="System.Int32.MaxValue"/>.</param>
        public LevenbergMarquardtAbortCondition(int maxIterations = 1000, int maxEvaluations = Int32.MaxValue)
        {
            MaxIterations = maxIterations;
            MaxEvaluations = maxEvaluations;
            Tolerance1 = Tolerance2 = Tolerance3 = 1E-6;
            Tau = 1E-3;

            m_InfoOutputAction = infoOutputPackage =>
            {
                infoOutputPackage.Add("Max evaluations", maxEvaluations);
                infoOutputPackage.Add("Max Iterations", maxIterations);
            };
            m_StringRepresentation = String.Format("Max. Iterations: {0}; Max. Evaluations: {1}", maxIterations, maxEvaluations);
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

        /// <summary>Gets the maximal number of function evaluations, perhaps <see cref="System.Int32.MaxValue"/>.
        /// </summary>
        /// <value>The maximal number of function evaluations.</value>
        public int MaxEvaluations
        {
            get;
            private set;
        }

        /// <summary>Gets the number of repetitions of the algorithm, perhaps <see cref="System.Int32.MaxValue"/>.
        /// </summary>
        /// <value>The number of repetitions of the algorithm.</value>
        public int MaxIterations
        {
            get;
            private set;
        }

        public double Tolerance1
        {
            get;
            private set;
        }

        public double Tolerance2
        {
            get;
            private set;
        }


        public double Tolerance3 // ist streng genommen ein Teil der Konfiguration
        {
            get;
            private set;
        }

        public double Tau
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
            m_InfoOutputAction(infoOutputPackage);
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

        /// <summary>Creates a new <see cref="LevenbergMarquardtAbortCondition"/> object.
        /// </summary>          
        /// <param name="maxIterations">The number of repetitions of the algorithm, perhaps <see cref="System.Int32.MaxValue"/>.</param>
        /// <param name="maxEvaluations">The maximal number of function evaluations, perhaps <see cref="System.Int32.MaxValue"/>.</param>
        /// <returns>A specific <see cref="GoldfarbIdanaQuadraticProgramAbortCondition"/> object.</returns>
        public static LevenbergMarquardtAbortCondition Create(int maxIterations = 1000, int maxEvaluations = Int32.MaxValue)
        {
            return new LevenbergMarquardtAbortCondition(maxIterations, maxEvaluations);
        }
        #endregion
    }
}