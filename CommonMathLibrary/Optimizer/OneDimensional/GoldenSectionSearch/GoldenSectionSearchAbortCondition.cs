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

namespace Dodoni.MathLibrary.Optimizer.OneDimensional
{
    /// <summary>Serves as abort (stopping) condition for the Golden Section Search optimizer represented by <see cref="GoldenSectionSearchOptimizer"/>.
    /// </summary>
    public class GoldenSectionSearchAbortCondition : IInfoOutputQueriable
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

        /// <summary>Initializes a new instance of the <see cref="GoldenSectionSearchAbortCondition" /> class.
        /// </summary>
        /// <param name="tolerance">The tolerance of the algorithm; a and b are 2 * x * <paramref name="tolerance"/> apart with x (the best abscissa) at the midpoint of a and b, and therefore fractionally accurate tolerance.</param>
        /// <param name="maxEvaluations">The maximal number of function evaluations.</param>
        /// <param name="maxIterations">The number of iterations.</param>
        public GoldenSectionSearchAbortCondition(double tolerance = MachineConsts.Epsilon, int maxEvaluations = Int32.MaxValue, int maxIterations = Int32.MaxValue)
        {
            if (Double.IsNaN(tolerance) && (maxEvaluations == Int32.MaxValue) && (maxIterations == Int32.MaxValue))
            {
                throw new ArgumentException("No valid abort condition for Golden Section Search optimizer algorithm provided.");
            }
            Tolerance = tolerance;
            MaxEvaluations = maxEvaluations;
            MaxIterations = maxIterations;

            m_InfoOutputAction = infoOutputPackage =>
            {
                infoOutputPackage.Add("Absolute x-tolerance", tolerance);
                infoOutputPackage.Add("Max evaluations", maxEvaluations);
                infoOutputPackage.Add("Max Iterations", maxIterations);
            };
            m_StringRepresentation = String.Format("Tolerance: {0}; Max. Evaluations: {1}; Max. Iterations: {2}", tolerance, maxEvaluations, maxIterations);
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

        /// <summary>Gets the tolerance of the algorithm.
        /// </summary>
        /// <value>The tolerance.</value>
        public double Tolerance
        {
            get;
            private set;
        }

        /// <summary>Gets the number of iterations, perhaps <see cref="System.Int32.MaxValue"/>.
        /// </summary>
        /// <value>The number of iterations.</value>
        public int MaxIterations
        {
            get;
            private set;
        }

        /// <summary>Gets the maximal number of function evaluations, perhaps <see cref="System.Int32.MaxValue"/>.
        /// </summary>
        /// <value>The maximal number of function evaluations.</value>
        public int MaxEvaluations
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

        /// <summary>Creates a new <see cref="GoldenSectionSearchAbortCondition"/> object.
        /// </summary>          
        /// <param name="tolerance">The tolerance of the algorithm; a and b are 2 * x * <paramref name="tolerance"/> apart with x (the best abscissa) at the midpoint of a and b, and therefore fractionally accurate tolerance.</param>
        /// <param name="maxEvaluations">The maximal number of function evaluations.</param>
        /// <param name="maxIterations">The number of iterations.</param>
        /// <returns>A specific <see cref="GoldenSectionSearchAbortCondition"/> object.</returns>
        public static GoldenSectionSearchAbortCondition Create(double tolerance = MachineConsts.Epsilon, int maxEvaluations = Int32.MaxValue, int maxIterations = Int32.MaxValue)
        {
            return new GoldenSectionSearchAbortCondition(tolerance, maxEvaluations, maxIterations);
        }
        #endregion
    }
}