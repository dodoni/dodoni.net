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

using Dodoni.MathLibrary.Basics;
using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.ProbabilityTheory
{
    /// <summary>Serves as abort (stopping) condition for the EZI algorithm represented by <see cref="EziMatrixDecomposer"/>.
    /// </summary>
    public class EziMatrixDecomposerAbortCondition : IInfoOutputQueriable
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

        /// <summary>Initializes a new instance of the <see cref="EziMatrixDecomposerAbortCondition"/> class.
        /// </summary>
        /// <param name="tolerance1">The tolerance for stopping condition || \tilde{p} - normalised( \tilde{p}) ||.</param>
        /// <param name="tolerance2">The tolerance for stopping condition |a_i - a_{i-1}|.</param>
        /// <param name="maxIterations">The number of repetitions of the algorithm, perhaps <see cref="System.Int32.MaxValue"/>.</param>
        public EziMatrixDecomposerAbortCondition(double tolerance1 = MachineConsts.Epsilon, double tolerance2 = MachineConsts.Epsilon, int maxIterations = 500)
        {
            if (Double.IsNaN(tolerance1) && (Double.IsNaN(tolerance2)) && (maxIterations == Int32.MaxValue))
            {
                throw new ArgumentException("No valid abort condition for EZI algorithm provided.");
            }
            Tolerance1 = tolerance1;
            Tolerance2 = tolerance2;
            MaxIterations = maxIterations;
            m_InfoOutputAction = infoOutputPackage =>
            {
                infoOutputPackage.Add("Tolerance 1", tolerance1);
                infoOutputPackage.Add("Tolerance 2", tolerance2);
                infoOutputPackage.Add("Max Iterations", maxIterations);
            };
            m_StringRepresentation = String.Format("Tolerance 1: {0}; Tolerance 2: {1}; Max. Iterations: {2}", tolerance1, tolerance2, maxIterations);
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

        /// <summary>Gets the tolerance for stopping condition || \tilde{p} - normalised( \tilde{p}) ||.
        /// </summary>
        /// <value>The tolerance for stopping condition || \tilde{p} - normalised( \tilde{p}) ||.</value>
        public double Tolerance1
        {
            get;
            private set;
        }

        /// <summary>Gets the tolerance for stopping condition |a_i - a_{i-1}|.
        /// </summary>
        /// <value>The tolerance for stopping condition |a_i - a_{i-1}|.</value>
        public double Tolerance2
        {
            get;
            private set;
        }

        /// <summary>Gets the number of repetitions of the algorithm, perhaps <see cref="System.Int32.MaxValue"/>.
        /// </summary>
        /// <value>The the number of repetitions of the algorithm, perhaps <see cref="System.Int32.MaxValue"/>.</value>
        public int MaxIterations
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
        /// <param name="n">The dimension of the correlation matrix.</param>
        /// <param name="p">The matrix p, i.e. the matrix calculated in the previous iteration step; on exit perhaps overwritten.</param>
        /// <param name="q">The matrix q, i.e. the matrix calculated in the current iteration step.</param>
        /// <param name="qNormalized">The normalized matrix q; on exit; on exit perhaps overwritten.</param>
        /// <param name="a">The value of ||p - q|| with respect to the previous iteration step; will be updated with respect to <paramref name="p"/> and <paramref name="q"/>.</param>
        /// <returns><c>true</c> if the abort (exit) condition is satisfied; <c>false</c> otherwise.</returns>
        public virtual bool IsSatisfied(int n, double[] p, double[] q, double[] qNormalized, ref double a)
        {
            VectorUnit.Basics.Sub(n * n, p, q); // set p = p - q, i.e. overwrite p for the calculation of the norm
            var aCurrent = BLAS.Level1.dnrm2(n * n, p);   // = ||p - q||

            VectorUnit.Basics.Sub(n * n, qNormalized, p); // set <q> := <q> - p, i.e. overwrite <q> for the calculation of the norm
            var norm = BLAS.Level1.dnrm2(n * n, qNormalized); // = ||p - <q>||            
            
            if ((norm < Tolerance1) || (Math.Abs(a - aCurrent) < Tolerance2))
            {
                a = aCurrent;
                return true;
            }
            a = aCurrent;
            return false;
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

        /// <summary>Creates a new <see cref="EziMatrixDecomposerAbortCondition"/> object.
        /// </summary>          
        /// <param name="tolerance1">The tolerance for stopping condition || \tilde{p} - normalised( \tilde{p}) ||.</param>
        /// <param name="tolerance2">The tolerance for stopping condition |a_i - a_{i-1}|.</param>
        /// <param name="maxIterations">The number of repetitions of the algorithm, perhaps <see cref="System.Int32.MaxValue"/>.</param>
        /// <returns>A specific <see cref="EziMatrixDecomposerAbortCondition"/> object.</returns>
        public static EziMatrixDecomposerAbortCondition Create(double tolerance1 = MachineConsts.Epsilon, double tolerance2 = MachineConsts.Epsilon, int maxIterations = 500)
        {
            return new EziMatrixDecomposerAbortCondition(tolerance1, tolerance2, maxIterations);
        }
        #endregion
    }
}