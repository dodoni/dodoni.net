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
using System.Linq;

using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.Optimizer.OneDimensional
{
    /// <summary>Serves as abort (stopping) condition for the Simulated Annealing optimizer represented by <see cref="OneDimSAOptimizer"/>.
    /// </summary>
    public class OneDimSAOptimizerAbortCondition : IInfoOutputQueriable
    {
        #region nested enumerations

        /// <summary>Represents the pure abort condition of the algorithm.
        /// </summary>
        public enum Term
        {
            /// <summary>Compare the average of the <see cref="OneDimSAOptimizerAbortCondition.RequiredNumberOfAcceptedPoints"/> with the estimated minimum and last accepted point.
            /// </summary>
            /// <remarks>This condition is based on "Use of a simulated annealing algorithm to fit compartmental models with an application to fractal pharmacokinetics"; Rebeccah E. Marsh, Terence A. Riauka, Steve A. McQuarrie, 14.06.2007.</remarks>
            Average,

            /// <summary>Compare each of the last <see cref="OneDimSAOptimizerAbortCondition.RequiredNumberOfAcceptedPoints"/> accepted points with the estimated minimum and last accepted point.
            /// </summary>
            TestEachAcceptedPoint
        }
        #endregion

        #region private members

        /// <summary>A delegate used to fill a specific <see cref="InfoOutputPackage"/> object.
        /// </summary>
        private Action<InfoOutputPackage> m_InfoOutputAction;

        /// <summary>The <see cref="System.String"/> representation of the current object.
        /// </summary>
        private string m_StringRepresentation;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="OneDimSAOptimizerAbortCondition" /> class.
        /// </summary>
        /// <param name="tolerance">The tolerance.</param>
        /// <param name="maxEvaluations">The maximal number of function evaluations.</param>
        /// <param name="maxIterations">The number of repetitions of the algorithm, perhaps <see cref="System.Int32.MaxValue"/>.</param>
        /// <param name="numberOfIterationsBeforeTemperatureReduction">The number of iterations before temperature reduction. After <paramref name="numberOfIterationsBeforeTemperatureReduction"/> * <paramref name="numberOfCycles"/> function evaluations, the temperature is changed.</param>
        /// <param name="numberOfCycles">The number of cycles. After <paramref name="numberOfCycles"/> function evaluations, the step length is adjusted so that approximately half of all function evaluations are accepted.</param>        
        /// <param name="requiredNumberOfAcceptedPoints">The number of accepted points required before applying exit condition of the algorithm.</param>       
        /// <param name="conditionType">The abort condition type in its <see cref="OneDimSAOptimizerAbortCondition.Term"/> representation.</param>
        public OneDimSAOptimizerAbortCondition(double tolerance = 1E-2, int maxEvaluations = 15000, int maxIterations = 15, int numberOfIterationsBeforeTemperatureReduction = 50, int numberOfCycles = 20, int requiredNumberOfAcceptedPoints = 4, Term conditionType = Term.Average)
        {
            if (Double.IsNaN(tolerance) && (maxEvaluations == Int32.MaxValue) && (maxIterations == Int32.MaxValue) && (numberOfIterationsBeforeTemperatureReduction == Int32.MaxValue) && (numberOfCycles == Int32.MaxValue) && (requiredNumberOfAcceptedPoints == Int32.MaxValue))
            {
                throw new ArgumentException("No valid abort condition for Simulated Annealing optimizer algorithm provided.");
            }
            Tolerance = tolerance;
            MaxEvaluations = maxEvaluations;
            MaxIterations = maxIterations;
            NumberOfIterationsBeforeTemperatureReduction = numberOfIterationsBeforeTemperatureReduction;
            NumberOfCyles = numberOfCycles;
            RequiredNumberOfAcceptedPoints = requiredNumberOfAcceptedPoints;
            ConditionType = conditionType;

            m_InfoOutputAction = infoOutputPackage =>
            {
                infoOutputPackage.Add("Tolerance", tolerance);
                infoOutputPackage.Add("Required number of accepted points", requiredNumberOfAcceptedPoints);
                infoOutputPackage.Add("Max evaluations", maxEvaluations);
                infoOutputPackage.Add("Max Iterations", maxIterations);
            };
            m_StringRepresentation = String.Format("Tolerance: {0}; Max. Evaluations: {1}; Max. Iterations: {2}; NT: {3}; NS: {4}, NEPS: {5}", tolerance, maxEvaluations, maxIterations, numberOfIterationsBeforeTemperatureReduction, numberOfCycles, requiredNumberOfAcceptedPoints);
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

        /// <summary>Gets the number of iterations before temperature reduction.
        /// </summary>
        /// <value>The number of iterations before temperature reduction.</value>
        public int NumberOfIterationsBeforeTemperatureReduction
        {
            get;
            private set;
        }

        /// <summary>Gets the number of cycles. After <see cref="NumberOfCyles"/> function evaluations, the step length is adjusted so that approximately half of all function evaluations are accepted.
        /// </summary>
        /// <value>The number of cycles. After <see cref="NumberOfCyles"/> function evaluations, the step length is adjusted so that approximately half of all function evaluations are accepted.</value>
        public int NumberOfCyles
        {
            get;
            private set;
        }

        /// <summary>Gets the number of final function values used to decide upon termination.
        /// </summary>
        /// <value>The number of final function values used to decide upon termination.</value>
        public int RequiredNumberOfAcceptedPoints
        {
            get;
            private set;
        }

        /// <summary>Gets the abort condition type in its <see cref="OneDimSAOptimizerAbortCondition.Term"/> representation.
        /// </summary>
        /// <value>The abort condition type in its <see cref="OneDimSAOptimizerAbortCondition.Term"/> representation.</value>
        public Term ConditionType
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

        /// <summary>Gets a value indicating whether the abort (exit) condition is satisfied.
        /// </summary>
        /// <param name="estimatedMinimum">The estimated minimum of the specific objective function.</param>
        /// <param name="y">The current function value of the Simulated Annealing algorithm.</param>
        /// <param name="yLastAccepted">The last 'accepted' function value of the Simulated Annealing algorithm.</param>
        /// <param name="yStar">A array with <see cref="RequiredNumberOfAcceptedPoints"/> relevant entries that contains 'accepted' function values taken into account for the exit condition.</param>
        /// <param name="yStarIndex">The null-based index of <paramref name="yStar"/> that represents the index of the currently added 'accepted' function value.</param>
        /// <returns><c>true</c> if the abort (exit) condition is satisfied; <c>false</c> otherwise.</returns>
        public virtual bool IsSatisfied(double estimatedMinimum, double y, double yLastAccepted, double[] yStar, int yStarIndex)
        {
            switch (ConditionType)
            {
                case Term.Average:
                    double fprevious = yStar.Average();
                    if (Math.Abs(fprevious - yLastAccepted) > Tolerance * Math.Abs(fprevious))
                    {
                        return false;
                    }
                    if (Math.Abs(fprevious - estimatedMinimum) > Tolerance * Math.Abs(fprevious))
                    {
                        return false;
                    }
                    return true;

                case Term.TestEachAcceptedPoint:
                    if (Math.Abs(estimatedMinimum - yStar[yStarIndex]) > Tolerance)
                    {
                        return false;
                    }
                    for (int i = 0; i < RequiredNumberOfAcceptedPoints; i++)
                    {
                        if (Math.Abs(yLastAccepted - yStar[i]) >= Tolerance)
                        {
                            return false;
                        }
                    }
                    return true;

                default: throw new NotImplementedException();
            }
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

        /// <summary>Creates a new <see cref="OneDimSAOptimizerAbortCondition"/> object.
        /// </summary>          
        /// <param name="tolerance">The tolerance.</param>
        /// <param name="maxEvaluations">The maximal number of function evaluations.</param>
        /// <param name="maxIterations">The number of repetitions of the algorithm, perhaps <see cref="System.Int32.MaxValue"/>.</param>
        /// <param name="numberOfIterationsBeforeTemperatureReduction">The number of iterations before temperature reduction. After <paramref name="numberOfIterationsBeforeTemperatureReduction"/> * <paramref name="numberOfCycles"/> function evaluations, the temperature is changed.</param>
        /// <param name="numberOfCycles">The number of cycles. After <paramref name="numberOfCycles"/> function evaluations, the step length is adjusted so that approximately half of all function evaluations are accepted.</param>        
        /// <param name="requiredNumberOfAcceptedPoints">The number of accepted points required before applying exit condition of the algorithm.</param>       
        /// <param name="conditionType">The abort condition type in its <see cref="OneDimSAOptimizerAbortCondition.Term"/> representation.</param>
        /// <returns>A specific <see cref="OneDimSAOptimizerAbortCondition"/> object.</returns>
        public static OneDimSAOptimizerAbortCondition Create(double tolerance = 1E-2, int maxEvaluations = 15000, int maxIterations = 15, int numberOfIterationsBeforeTemperatureReduction = 50, int numberOfCycles = 20, int requiredNumberOfAcceptedPoints = 4, Term conditionType = Term.Average)
        {
            return new OneDimSAOptimizerAbortCondition(tolerance, maxEvaluations, maxIterations, numberOfIterationsBeforeTemperatureReduction, numberOfCycles, requiredNumberOfAcceptedPoints, conditionType);
        }
        #endregion
    }
}