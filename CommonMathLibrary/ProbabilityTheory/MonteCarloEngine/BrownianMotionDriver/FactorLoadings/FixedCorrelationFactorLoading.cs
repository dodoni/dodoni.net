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

using Dodoni.MathLibrary.Basics;

namespace Dodoni.MathLibrary.ProbabilityTheory.MonteCarlo.BrownianMotionDriver
{
    /// <summary>Represents a simple implementation of some Factor loading which is given via some time-independent correlation matrix 
    /// (a common choice) but without taken into account any kind of volatility.
    /// </summary>
    public sealed class FixedCorrelationFactorLoading : IFactorLoading
    {
        #region private members

        /// <summary>The discretization of the time horizont excluding <c>0.0</c>.
        /// </summary>
        private double[] m_Timediscretization;

        /// <summary>The current point in time as some null-based index.
        /// </summary>
        /// <remarks>Keep in mind that <c>0.0</c> is not included in <see cref="m_Timediscretization"/>.</remarks>
        private int m_CurrentTimeIndex;

        /// <summary>The number of factors.
        /// </summary>
        private int m_NumberOfFactors;

        /// <summary>The pseudo root of the correlation matrix which is assumed to be time-independent.
        /// </summary>
 //       private ILArray<double> m_PseudoRootOfCorrelationMatrix;

        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="FixedCorrelationFactorLoading"/> class.
        /// </summary>
        /// <param name="timediscretization">The time discretization (without <c>0.0</c>).</param>
        /// <param name="pseudoRootOfCorrelationMatrix">The pseudo root of correlation matrix.</param>
        //public FixedCorrelationFactorLoading(double[] timediscretization, ILArray<double> pseudoRootOfCorrelationMatrix)
        //{
        //    if (timediscretization == null)
        //    {
        //        throw new ArgumentNullException("time discretization", string.Format(null, ExceptionMessages.ArgumentNull, "timediscretization"));
        //    }
        //    if (pseudoRootOfCorrelationMatrix == null)
        //    {
        //        throw new ArgumentNullException("pseudo root of correlation matrix", string.Format(null, ExceptionMessages.ArgumentNull, "pseudoRootOfCorrelationMatrix"));
        //    }
        //    if (pseudoRootOfCorrelationMatrix.Dimensions[0] != pseudoRootOfCorrelationMatrix.Dimensions[1])
        //    {
        //        throw new ArgumentException("pseudo root of correlation matrix", string.Format(null, ExceptionMessages.ArgumentIsNoSquaredMatrix, "pseudoRootOfCorrelationMatrix"));
        //    }
        //    m_NumberOfFactors = pseudoRootOfCorrelationMatrix.Dimensions[0];

        //    m_PseudoRootOfCorrelationMatrix = pseudoRootOfCorrelationMatrix;
        //    m_Timediscretization = timediscretization;
        //    m_CurrentTimeIndex = -1;
        //}

        #endregion

        #region public properties

        #region IFactorLoading Members

        /// <summary>Gets the number of factors, i.e. the number of independent Brownian motions taken into account.
        /// </summary>
        /// <value>The number of factors.</value>
        public int NumberOfFactors
        {
            get { return m_NumberOfFactors; }
        }

        /// <summary>Gets the number of time steps, which corresponds to the discretization of the time horizont including the initial point in time <c>0.0</c>.
        /// </summary>
        /// <value>The number of time steps.</value>
        public int NumberOfTimeSteps
        {
            get { return m_Timediscretization.Length + 1; }
        }

        /// <summary>Gets the null-based index of the current time step, which corresponds to the discretization of the time horizont.
        /// </summary>
        /// <value>The index of the current time.</value>
        /// <remarks>This value will never be equal to <c>0</c> because the value of the (correlated) Brownian motion at time <c>0.0</c> is
        /// deterministic and equal to <c>0.0</c> as well.</remarks>
        public int CurrentTimeIndex
        {
            get { return m_CurrentTimeIndex + 1; }
        }

        /// <summary>Gets the current time, i.e. the <see cref="System.Double"/> representation of the current point in time.
        /// </summary>
        /// <value>The current time.</value>
        public double CurrentTime
        {
            get { return (m_CurrentTimeIndex < 0) ? 0.0 : m_Timediscretization[m_CurrentTimeIndex]; }
        }

        /// <summary>Gets the current time difference between the current point in time and the previous point in time.
        /// </summary>
        /// <value>The current time difference.</value>
        public double CurrentTimeDifference
        {
            get { return (m_CurrentTimeIndex < 0) ? 0.0 : (m_Timediscretization[m_CurrentTimeIndex] - ((m_CurrentTimeIndex == 0) ? 0 : m_Timediscretization[m_CurrentTimeIndex - 1])); }
        }

        #endregion

        #endregion

        #region IFactorLoading Members

        /// <summary>Resets the current instance, i.e. <see cref="CurrentTimeIndex"/> will be set to <c>0</c> thus to time <c>0.0</c>, i.e.
        /// the first point of the given time discretization.
        /// </summary>
        public void Reset()
        {
            m_CurrentTimeIndex = -1;
        }

        /// <summary>Points to the next point in time, i.e. to the next discretization step.
        /// </summary>
        /// <returns>
        /// A value indicating whether for the current instance there is some additional point in time available.
        /// </returns>
        public bool NextPointInTime()
        {
            if (m_CurrentTimeIndex < m_Timediscretization.Length - 1)
            {
                m_CurrentTimeIndex++;
                return true;
            }
            return false;
        }

        /// <summary>Gets realisations of the Brownian motion increments for the <see cref="CurrentTimeDifference"/>.
        /// </summary>
        /// <param name="numberOfRealisations">The number of realisations.</param>
        /// <param name="normalDistributedNumbers">The normal distributed numbers, where the first null-based index corresponds to the
        /// <see cref="IFactorLoading.NumberOfFactors"/> and the second index corresponds to the <paramref name="numberOfRealisations"/>.</param>
        /// <returns>
        /// The Brownian motion increments for the <see cref="CurrentTimeDifference"/>, where the first null-based index corresponds to
        /// the <see cref="IFactorLoading.NumberOfFactors"/> and the second index corresponds to the <paramref name="numberOfRealisations"/>.
        /// </returns>
        //public ILArray<double> GetOneStepBrownianMotionIncrements(int numberOfRealisations, ILArray<double> normalDistributedNumbers)
        //{
        //    double sqrtOfDeltaT = Math.Sqrt(m_Timediscretization[m_CurrentTimeIndex] - ((m_CurrentTimeIndex == 0) ? 0 : m_Timediscretization[m_CurrentTimeIndex - 1]));

        //    return sqrtOfDeltaT * ILAlgorithm.multiply(m_PseudoRootOfCorrelationMatrix, normalDistributedNumbers);
        //}

        /// <summary>Gets a realisation of the Brownian motion increments for the <see cref="CurrentTimeDifference"/>.
        /// </summary>
        /// <param name="normalDistributedNumbers">The normal distributed numbers, i.e. a list of <see cref="System.Double"/> instances, where the
        /// null-based index corresponds to the <see cref="IFactorLoading.NumberOfFactors"/>.</param>
        /// <returns>
        /// The Brownian motion increments for the <see cref="CurrentTimeDifference"/>, where the null-based index corresponds to
        /// the <see cref="IFactorLoading.NumberOfFactors"/>.
        /// </returns>
        //public ILArray<double> GetOneStepBrownianMotionIncrements(ILArray<double> normalDistributedNumbers)
        //{
        //    double sqrtOfDeltaT = Math.Sqrt(m_Timediscretization[m_CurrentTimeIndex] - ((m_CurrentTimeIndex == 0) ? 0 : m_Timediscretization[m_CurrentTimeIndex - 1]));

        //    return sqrtOfDeltaT * ILAlgorithm.multiply(m_PseudoRootOfCorrelationMatrix, normalDistributedNumbers);
        //}

        /// <summary>Gets realisations of the Brownian motion increments from the first point in time (not <c>0.0</c>) up to the
        /// last point in time of the discretization of the time horizont, i.e. <see cref="CurrentTimeIndex"/> will not be taken into account,
        /// the whole time horizont will be used instead.
        /// </summary>
        /// <param name="numberOfRealisations">The number of realisations.</param>
        /// <param name="normalDistributedNumbers">The normal distributed numbers, where the first null-based index corresponds to
        /// the time discretization (the first element does not belong to time <c>0.0</c> but to the first point in time which is destinct from <c>0.0</c>),
        /// the second index corresponds to the <see cref="IFactorLoading.NumberOfFactors"/> and the third null-based index corresponds to the
        /// <paramref name="numberOfRealisations"/>.</param>
        /// <returns>
        /// The Brownian motion increments for the whole time horizont, thus the first null-based index corresponds to
        /// the time discretization (the first element does not belong to time <c>0.0</c> but to the first point in time which is destinct from <c>0.0</c>),
        /// the second index corresponds to the <see cref="IFactorLoading.NumberOfFactors"/> and the third null-based index corresponds to the
        /// <paramref name="numberOfRealisations"/>.
        /// </returns>
        /// <remarks><see cref="CurrentTimeIndex"/> will not taken into account and will not be changed. Remember that the Brownian
        /// increments are not defined at time <c>0.0</c>.</remarks>
        //public ILArray<double> GetBrownianMotionIncrement(int numberOfRealisations, ILArray<double> normalDistributedNumbers)
        //{
        //    double sqrtOfDeltaT;
        //    double lastPointInTime = 0.0;
        //    ILArray<double> tempMatrix, returnMatrix;

        //    returnMatrix = new ILArray<double>(m_Timediscretization.Length, m_NumberOfFactors, numberOfRealisations);
        //    for (int i = 0; i < m_Timediscretization.Length; i++)
        //    {
        //        sqrtOfDeltaT = Math.Sqrt(m_Timediscretization[i] - lastPointInTime);
        //        lastPointInTime = m_Timediscretization[i];

        //        //tempMatrix = sqrtOfDeltaT * ILAlgorithm.multiply(m_PseudoRootOfCorrelationMatrix, normalDistributedNumbers[i,null,null]);
        //        // dummerweise gibt dies jedoch nicht eine zwei-dimensionale Matrix zurück, sondern eine dreidimensional [0,i,j]
        //        // dies funktioniert jedoch, wenn man die Eingabe ändert, d.h. [null,null, i] liefert tatsächlich eine zweidimensionale Matrix!
        //        // merkwüridigerweise funktioniert dies auch, wenn man die Matrix transponiert - warum?
        //        tempMatrix = sqrtOfDeltaT * ILAlgorithm.multiply(m_PseudoRootOfCorrelationMatrix, normalDistributedNumbers[i,null,null].T);

        //        returnMatrix[i,null,null] = tempMatrix;
        //    }
        //    return returnMatrix;
        //}

        #endregion
    }
}