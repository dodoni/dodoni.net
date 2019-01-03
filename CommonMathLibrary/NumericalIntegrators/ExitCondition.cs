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
    /// <summary>Provides exit condition for many one-dimensional numerical integration algorithm.
    /// </summary>
    public class ExitCondition : IInfoOutputQueriable
    {
        #region nested enumerations

        /// <summary>Represents a value indicating how to interpret the tolerance as exit condition for a specified numerical integrator object.
        /// </summary>
        public enum ToleranceType
        {
            /// <summary>Do not take into account any tolerance as exit condition for the algorithm.
            /// </summary>
            None,

            /// <summary>Terminate the algorithm if a specific absolute tolerance has been fulfilled.
            /// </summary>
            WithinAbsoluteTolerance,

            /// <summary>Terminate the algorithm if a specific relative tolerance has been fulfilled.
            /// </summary>
            WithinRelativeTolerance
        }
        #endregion

        #region private members

        /// <summary>The absolute tolerance taken into account as exit condition; or <see cref="System.Double.NaN"/>.
        /// </summary>
        private double m_AbsoluteTolerance;

        /// <summary>The relative tolerance taken into account as exit condition; or <see cref="System.Double.NaN"/>.
        /// </summary>
        private double m_RelativeTolerance;
        #endregion

        #region protected constructors

        /// <summary>Initializes a new instance of the <see cref="ExitCondition"/> class.
        /// </summary>
        /// <param name="maxIterations">The maximal number of iterations.</param>
        /// <param name="maxEvaluations">The maximal number of function evaluations.</param>
        /// <param name="tolerance">The tolerance to take into account as exit condition; or <see cref="System.Double.NaN"/>.</param>
        /// <param name="toleranceType">A value indicating how to interprete <paramref name="tolerance"/>.</param>
        protected ExitCondition(int maxIterations, int maxEvaluations = Int32.MaxValue, double tolerance = Double.NaN, ToleranceType toleranceType = ToleranceType.None)
        {
            MaxIterations = maxIterations;
            MaxEvaluations = maxEvaluations;
            switch (toleranceType)
            {
                case ToleranceType.WithinAbsoluteTolerance:
                    m_AbsoluteTolerance = tolerance;
                    m_RelativeTolerance = Double.NaN;
                    break;
                case ToleranceType.WithinRelativeTolerance:
                    m_RelativeTolerance = tolerance;
                    m_AbsoluteTolerance = Double.NaN;
                    break;

                default:
                    m_AbsoluteTolerance = m_RelativeTolerance = Double.NaN;
                    break;
            }
        }

        /// <summary>Initializes a new instance of the <see cref="ExitCondition"/> class.
        /// </summary>
        /// <param name="maxIterations">The maximal number of iterations.</param>
        /// <param name="absoluteTolerance">A absolute tolerance.</param>
        /// <param name="relativeTolerance">A relative tolerance.</param>
        /// <param name="maxEvaluations">The maximal number of function evaluations.</param>
        protected ExitCondition(int maxIterations, double absoluteTolerance, double relativeTolerance, int maxEvaluations)
        {
            MaxIterations = maxIterations;
            MaxEvaluations = maxEvaluations;
            m_AbsoluteTolerance = absoluteTolerance;
            m_RelativeTolerance = relativeTolerance;
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

        /// <summary>Gets the maximal number of function evaluations.
        /// </summary>
        /// <value>The maximal number of function evaluations.</value>
        public int MaxEvaluations
        {
            get;
            private set;
        }

        /// <summary>Gets the maximal number of iterations.
        /// </summary>
        /// <value>The maximal number of iterations.</value>
        public int MaxIterations
        {
            get;
            private set;
        }
        #endregion

        #region protected properties

        /// <summary>Gets the absolute tolerance taken into account as exit condition; or <see cref="System.Double.NaN"/>.
        /// </summary>
        /// <value>The absolute tolerance; or <see cref="System.Double.NaN"/>.</value>
        protected double AbsoluteTolerance
        {
            get { return m_AbsoluteTolerance; }
        }

        /// <summary>Gets the relative tolerance taken into account as exit condition; or <see cref="System.Double.NaN"/>.
        /// </summary>
        /// <value>The relative tolerance; or <see cref="System.Double.NaN"/>.</value>
        protected double RelativeTolerance
        {
            get { return m_RelativeTolerance; }
        }
        #endregion

        #region public methods

        #region IInfoOutputQueriable Members

        /// <summary>Gets informations of the current object as a specific <see cref="T:Dodoni.BasicComponents.Containers.InfoOutput"/> instance.
        /// </summary>
        /// <param name="infoOutput">The <see cref="T:Dodoni.BasicComponents.Containers.InfoOutput"/> object which is to be filled with informations concering the current instance.</param>
        /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
        public virtual void FillInfoOutput(InfoOutput infoOutput, string categoryName = "General")
        {
            var infoOutputPackage = infoOutput.AcquirePackage(categoryName);

            infoOutputPackage.Add("Maximal number of iterations", MaxIterations);
            infoOutputPackage.Add("Maximal function evaluations", MaxEvaluations);
            infoOutputPackage.Add("Absolute tolerance", m_AbsoluteTolerance);
            infoOutputPackage.Add("Relative tolerance", m_RelativeTolerance);
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

        /// <summary>Gets a value indicating whether the specified exit condition is met.
        /// </summary>
        /// <param name="previousEstimatedValue">The estimated value of the specified integral in a previous iteration step.</param>
        /// <param name="estimatedValue">The estimated value of the specified integral in the current iteration step.</param>
        /// <returns><c>true</c> if the specified exit condition is fulfilled; <c>false</c> otherwise.</returns>
        public virtual bool IsFulfilled(double previousEstimatedValue, double estimatedValue)
        {
            if (Double.IsNaN(m_AbsoluteTolerance) == false)
            {
                if (Math.Abs(previousEstimatedValue - estimatedValue) < m_AbsoluteTolerance)
                {
                    return true;
                }
            }

            if (Double.IsNaN(m_RelativeTolerance) == false)
            {
                double estimatedRelativeError = Math.Abs(previousEstimatedValue - estimatedValue);
                if (Math.Abs(previousEstimatedValue) > 0.0)
                {
                    estimatedRelativeError /= Math.Abs(previousEstimatedValue);
                }
                if (estimatedRelativeError < m_RelativeTolerance)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return String.Format("max. iterations: {0}; max. evaluations: {1}; abs. tolerance: {2}, rel. tolerance: {3}", MaxIterations, MaxEvaluations, m_AbsoluteTolerance, m_RelativeTolerance);
        }
        #endregion

        #region public static methods

        /// <summary>Creates a new <see cref="ExitCondition"/> object.
        /// </summary>
        /// <param name="maxIterations">The maximal number of iterations.</param>
        /// <param name="maxEvaluations">The maximal number of function evaluations.</param>
        /// <param name="tolerance">The tolerance to take into account as exit condition; or <see cref="System.Double.NaN"/>.</param>
        /// <param name="toleranceType">A value indicating how to interprete <paramref name="tolerance"/>.</param>
        /// <returns>A specific <see cref="ExitCondition"/> object.</returns>
        public static ExitCondition Create(int maxIterations, int maxEvaluations = Int32.MaxValue, double tolerance = Double.NaN, ToleranceType toleranceType = ToleranceType.None)
        {
            return new ExitCondition(maxIterations, maxEvaluations, tolerance, toleranceType);
        }

        /// <summary>Creates a new <see cref="ExitCondition"/> object.
        /// </summary>
        /// <param name="maxIterations">The maximal number of iterations.</param>
        /// <param name="absoluteTolerance">A absolute tolerance.</param>
        /// <param name="relativeTolerance">A relative tolerance.</param>
        /// <param name="maxEvaluations">The maximal number of function evaluations.</param>
        /// <returns>A specific <see cref="ExitCondition"/> object.</returns>
        public static ExitCondition Create(int maxIterations, double absoluteTolerance, double relativeTolerance, int maxEvaluations = Int32.MaxValue)
        {
            return new ExitCondition(maxIterations, absoluteTolerance, relativeTolerance, maxEvaluations);
        }
        #endregion
    }
}