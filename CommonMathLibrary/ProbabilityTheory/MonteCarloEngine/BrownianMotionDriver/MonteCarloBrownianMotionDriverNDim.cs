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
//using System;
//using System.Text;
//using System.Collections.Generic;

//using Dodoni.MathLibrary.Basics;
//using Dodoni.MathLibrary.Statistics.MontoCarloEngine;

//namespace Dodoni.MathLibrary.ProbabilityTheory.MonteCarlo.BrownianMotionDriver
//{
//    /// <summary>Represents the implementation of the multivariate Brownian motion generator for (pseudo) random number generators
//    /// as Mersenne Twister etc.
//    /// </summary>
//    public sealed class MonteCarloBrownianMotionDriverNDim : IBrownianMotionDriverNDim
//    {
//        #region private members

//        /// <summary>The sequence generator.
//        /// </summary>
//        private IRandomNumberGenerator m_SequenceGenerator;

//        /// <summary>The factor loading, i.e. the correlation and volatilities if desired.
//        /// </summary>
//        private IFactorLoading m_FactorLoading;

//        /// <summary>The number of realisations.
//        /// </summary>
//        private int m_NumberOfRealisations;

//        /// <summary>The construction type.
//        /// </summary>
//        private eBrownianMotionConstructionType m_ConstructionType;

//        /// <summary>This array contains the normal distributed random numbers which are the output of the sequence generator.
//        /// </summary>
//        /// <remarks>This member is (re-)used for performance reason.</remarks>
//        private ILArray<double> m_TempNormalDistributedRandomNumbers;

//        /// <summary>This array contains the antithetic normal distributed random numbers which are the output of the sequence generator.
//        /// </summary>
//        /// <remarks>This member is (re-)used for performance reason.</remarks>
//        private ILArray<double> m_TempAntitheticNormalDistributedRandomNumbers;

//        /// <summary>A value indicating whether some antithetic path/increment is available for some already constructed path/increment.
//        /// </summary>
//        private bool m_AntiteticValueAvailable = false;

//        #endregion

//        #region public constructors

//        /// <summary>Initializes a new instance of the <see cref="MonteCarloBrownianMotionDriverNDim"/> class.
//        /// </summary>
//        /// <param name="factorLoading">The factor loading.</param>
//        /// <param name="sequenceGenerator">The sequence generator.</param>
//        /// <param name="numberOfRealisations">The number of realisations.</param>
//        /// <param name="constructionType">The construction type of the Brownian motion paths/increments.</param>
//        /// <exception cref="ArgumentNullException">Thrown if one of the argument is <c>null</c>.</exception>
//        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="numberOfRealisations"/> is less or equal <c>0</c>.</exception>
//        public MonteCarloBrownianMotionDriverNDim(IFactorLoading factorLoading, IRandomNumberGenerator sequenceGenerator, int numberOfRealisations, eBrownianMotionConstructionType constructionType)
//        {
//            if (numberOfRealisations <= 0)
//            {
//                throw new ArgumentOutOfRangeException("number of realisations", string.Format(null, ExceptionMessages.ArgumentOutOfRange, numberOfRealisations));
//            }
//            if (factorLoading == null)
//            {
//                throw new ArgumentNullException("factor loading", string.Format(null, ExceptionMessages.ArgumentNull, "factorLoading"));
//            }
//            if (sequenceGenerator == null)
//            {
//                throw new ArgumentNullException("sequence generator", string.Format(null, ExceptionMessages.ArgumentNull, "sequenceGenerator"));
//            }
//            m_FactorLoading = factorLoading;
//            m_SequenceGenerator = sequenceGenerator;
//            m_NumberOfRealisations = numberOfRealisations;
//            Reinitialize(constructionType, numberOfRealisations);
//        }

//        #endregion

//        #region public properties

//        #region IBrownianMotionDriverNDim Members

//        /// <summary>Gets the dimension of the Brownian motion.
//        /// </summary>
//        /// <value>The dimension.</value>
//        public int Dimension
//        {
//            get { return m_FactorLoading.NumberOfFactors; }
//        }

//        /// <summary>Gets the number of time steps, which corresponds to the discretization of the time horizont including the initial point in time <c>0.0</c>.
//        /// </summary>
//        /// <value>The number of time steps.</value>
//        public int NumberOfTimeSteps
//        {
//            get { return m_FactorLoading.NumberOfTimeSteps; }
//        }

//        /// <summary>Gets the number of realisations.
//        /// </summary>
//        /// <value>The number of realisations.</value>
//        public int NumberOfRealisations
//        {
//            get { return m_NumberOfRealisations; }
//        }

//        /// <summary>Gets the null-based index of the current time step, which corresponds to the discretization of the time horizont.
//        /// </summary>
//        /// <value>The index of the current time.</value>
//        /// <remarks>The value <c>0</c> corresponds to the point in time <c>0.0</c> where the value of the (correlated) Brownian motions are <c>0.0</c> as well.</remarks>
//        public int CurrentTimeIndex
//        {
//            get { return m_FactorLoading.CurrentTimeIndex; }
//        }

//        /// <summary>Gets the current time, i.e. the <see cref="System.Double"/> representation of the current point in time.
//        /// </summary>
//        /// <value>The current time.</value>
//        public double CurrentTime
//        {
//            get { return m_FactorLoading.CurrentTime; }
//        }

//        /// <summary>Gets the current time difference between the current point in time and the previous point in time.
//        /// </summary>
//        /// <value>The current time difference.</value>
//        /// <remarks>If <see cref="CurrentTimeIndex"/> is equal to <c>0</c> this property will return <c>0.0</c> as well.</remarks>
//        public double CurrentTimeDifference
//        {
//            get { return m_FactorLoading.CurrentTimeDifference; }
//        }

//        /// <summary>Gets a value indicating whether the current instances make available antithetic paths.
//        /// </summary>
//        /// <value>
//        /// 	<c>true</c> if [make available antithetic paths]; otherwise, <c>false</c>.
//        /// </value>
//        /// <remarks>This implementation returns <c>true</c> thus antithetic paths will be generated as well.</remarks>
//        public bool MakeAvailableAntitheticPaths
//        {
//            get { return true; }
//        }

//        /// <summary>Gets the construction type of the Brownian motion paths and Brownian motion increment generation.
//        /// </summary>
//        /// <value>The type of the construction.</value>
//        public eBrownianMotionConstructionType ConstructionType
//        {
//            get { return m_ConstructionType; }
//        }

//        #endregion

//        #endregion

//        #region public methods

//        #region IBrownianMotionDriverNDim Members

//        /// <summary>Reinitializes the current instance with some specified generation type and a number of realisations.
//        /// </summary>
//        /// <param name="constructionType">The Type of the construction.</param>
//        /// <param name="numberOfRealisations">The number of realisations.</param>
//        /// <remarks>It is not allowed to mix different path construction modes, i.e. either one wants to create for each point in time 
//        /// one-step realisations or all path/increment realisations will be generated.</remarks>
//        /// <returns>A value indicating whether the current instance has changed the construction type and the number of realisations.</returns>
//        public bool Reinitialize(eBrownianMotionConstructionType constructionType, int numberOfRealisations)
//        {
//            if (numberOfRealisations <= 0)
//            {
//                return false;
//            }
//            m_NumberOfRealisations = numberOfRealisations;

//            switch (constructionType)
//            {
//                case eBrownianMotionConstructionType.StepByStep:
//                    if (m_SequenceGenerator.SetSequenceLength(m_FactorLoading.NumberOfFactors) == false)
//                    {
//                        return false;
//                    }
//                    m_TempNormalDistributedRandomNumbers = new ILArray<double>(m_FactorLoading.NumberOfFactors, m_NumberOfRealisations);
//                    m_TempNormalDistributedRandomNumbers = new ILArray<double>(m_FactorLoading.NumberOfFactors, m_NumberOfRealisations);
//                    break;

//                case eBrownianMotionConstructionType.CompletePaths:
//                    if (m_SequenceGenerator.SetSequenceLength(m_FactorLoading.NumberOfTimeSteps) == false)
//                    {
//                        return false;
//                    }
//                    m_TempNormalDistributedRandomNumbers = new ILArray<double>(m_FactorLoading.NumberOfTimeSteps, m_FactorLoading.NumberOfFactors, m_NumberOfRealisations);
//                    m_TempAntitheticNormalDistributedRandomNumbers = new ILArray<double>(m_FactorLoading.NumberOfTimeSteps, m_FactorLoading.NumberOfFactors, m_NumberOfRealisations);
//                    break;

//                default: throw new NotImplementedException();
//            }
//            m_ConstructionType = constructionType;
//            return true;
//        }

//        /// <summary>Reset the time discretisation to the first point in time, i.e. <see cref="IBrownianMotionDriverNDim.CurrentTimeIndex"/> will be set to <c>0</c> thus
//        /// <see cref="IBrownianMotionDriverNDim.CurrentTime"/> and <see cref="IBrownianMotionDriverNDim.CurrentTimeDifference"/> to <c>0.0</c>.
//        /// </summary>
//        public void ResetCurrentTimeIndex()
//        {
//            m_FactorLoading.Reset();
//        }

//        /// <summary>Resets the sequence generator, i.e. if the sequence generator allows resetting it will produces the same sequences again.
//        /// </summary>
//        /// <returns>
//        /// 	<c>true</c>, if the sequence generator was reset; otherwise, <c>false</c>.
//        /// </returns>
//        public bool ResetSequenceGenerator()
//        {
//            return m_SequenceGenerator.Reset();
//        }

//        /// <summary>Gets Wiener increments for the next point in time, i.e. for the next time
//        /// </summary>
//        /// <param name="wienerIncrements">The Wiener increments for the point in time which is represented by the value of <see cref="IBrownianMotionDriverNDim.CurrentTimeIndex"/>
//        /// 	<c>after</c> the function call, where the first null-based index corresponds to the <see cref="IBrownianMotionDriverNDim.Dimension"/> and the second index corresponds
//        /// to the <see cref="IBrownianMotionDriverNDim.NumberOfRealisations"/>.</param>
//        /// <returns>
//        /// A value indicating whether <paramref name="wienerIncrements"/> contains valid data.
//        /// </returns>
//        /// <remarks>The properties <see cref="IBrownianMotionDriverNDim.CurrentTimeIndex"/>, <see cref="IBrownianMotionDriverNDim.CurrentTime"/> and <see cref="IBrownianMotionDriverNDim.CurrentTimeDifference"/>
//        /// might be changed. If there are no further points in time available <c>false</c> will returned.
//        /// <para>Before calling this method be sure that <see cref="IBrownianMotionDriverNDim.ConstructionType"/> is set to <see cref="eBrownianMotionConstructionType.StepByStep"/>. </para>
//        /// </remarks>
//        public bool GetNextOneStepWienerIncrements(ref ILArray<double> wienerIncrements)
//        {
//            if (m_FactorLoading.NextPointInTime())   // the first time is 0.0 which will not take into account.
//            {
//                m_SequenceGenerator.GetNextNormalDistributedSequences(m_NumberOfRealisations, m_TempNormalDistributedRandomNumbers, m_TempAntitheticNormalDistributedRandomNumbers);
//                wienerIncrements = m_FactorLoading.GetOneStepBrownianMotionIncrements(m_NumberOfRealisations, m_TempNormalDistributedRandomNumbers);
//                m_AntiteticValueAvailable = true;
//                return true;
//            }
//            m_AntiteticValueAvailable = false;
//            return false;
//        }

//        /// <summary>Gets the antithetic Wiener increments with respect to the last call of <see cref="GetNextOneStepWienerIncrements(ref ILArray&lt;double&gt;)"/>.
//        /// </summary>
//        /// <param name="antitheticWienerIncrements">The antithetic Wiener increments with respect to the previous call of <see cref="GetNextOneStepWienerIncrements(ref ILArray&lt;double&gt;)"/>,
//        /// i.e. the first null-based index corresponds to the <see cref="IBrownianMotionDriverNDim.Dimension"/> and the second index corresponds
//        /// to the <see cref="IBrownianMotionDriverNDim.NumberOfRealisations"/>.</param>
//        /// <returns>
//        /// A value indicating whether <paramref name="antitheticWienerIncrements"/> contains valid data.
//        /// </returns>
//        /// <remarks>The properties <see cref="IBrownianMotionDriverNDim.CurrentTimeIndex"/>, <see cref="IBrownianMotionDriverNDim.CurrentTime"/> and <see cref="IBrownianMotionDriverNDim.CurrentTimeDifference"/>
//        /// belief unchanged. If there are no further points in time available <c>false</c> will returned.
//        /// <para>Before calling this method be sure that <see cref="IBrownianMotionDriverNDim.ConstructionType"/> is set to <see cref="eBrownianMotionConstructionType.StepByStep"/>. </para>
//        /// </remarks>
//        public bool GetOneStepAntitheticWienerIncrements(ref ILArray<double> antitheticWienerIncrements)
//        {
//            if (m_AntiteticValueAvailable == true)
//            {
//                antitheticWienerIncrements = m_FactorLoading.GetOneStepBrownianMotionIncrements(m_NumberOfRealisations, m_TempAntitheticNormalDistributedRandomNumbers);
//                return true;
//            }
//            return false;
//        }

//        /// <summary>Gets the Wiener increments for each point in time, i.e. simultaneously for each discretisation step and each realisation,
//        /// where the number of realisations is represented by <see cref="IBrownianMotionDriverNDim.NumberOfRealisations"/>.
//        /// </summary>
//        /// <param name="wienerIncrements">The Wiener increments for each discretisation step, i.e. the first null-based index corresponds to the time discretisation,
//        /// the second to the <see cref="IBrownianMotionDriverNDim.Dimension"/> and the last for the realisation (see <see cref="IBrownianMotionDriverNDim.NumberOfRealisations"/>).</param>
//        /// <returns>
//        /// A value indicating whether <paramref name="wienerIncrements"/> contains valid data.
//        /// </returns>
//        /// <remarks>The properties <see cref="IBrownianMotionDriverNDim.CurrentTimeIndex"/>, <see cref="IBrownianMotionDriverNDim.CurrentTime"/> and
//        /// <see cref="IBrownianMotionDriverNDim.CurrentTimeDifference"/> beleave unchanged.
//        /// <para>Before calling this method be sure that <see cref="IBrownianMotionDriverNDim.ConstructionType"/> is set to <see cref="eBrownianMotionConstructionType.CompletePaths"/>. </para>
//        /// </remarks>
//        public bool GetWienerIncrements(ref ILArray<double> wienerIncrements)
//        {
//            if (m_ConstructionType == eBrownianMotionConstructionType.CompletePaths)
//            {
//                m_SequenceGenerator.GetNextNormalDistributedSequences(m_NumberOfRealisations, m_FactorLoading.NumberOfFactors, m_TempNormalDistributedRandomNumbers, m_TempAntitheticNormalDistributedRandomNumbers);
//                wienerIncrements = m_FactorLoading.GetBrownianMotionIncrement(m_NumberOfRealisations, m_TempNormalDistributedRandomNumbers);
//                m_AntiteticValueAvailable = true;
//                return true;
//            }
//            return false;
//        }

//        /// <summary>Gets the antithetic wiener increments with respect to the last call of <see cref="GetWienerIncrements(ref ILArray&lt;double&gt;)"/>.
//        /// </summary>
//        /// <param name="antitheticWienerIncrements">The antithetic Wiener increments with respect to the previous call of <see cref="GetWienerIncrements(ref ILArray&lt;double&gt;)"/>,
//        /// i.e. the first null-based index corresponds to the time discretisation, the second to the <see cref="IBrownianMotionDriverNDim.Dimension"/> and the
//        /// last for the realisation (see <see cref="IBrownianMotionDriverNDim.NumberOfRealisations"/>).</param>
//        /// <returns>
//        /// A value indicating whether <paramref name="antitheticWienerIncrements"/> contains valid data.
//        /// </returns>
//        /// <remarks>The properties <see cref="IBrownianMotionDriverNDim.CurrentTimeIndex"/>, <see cref="IBrownianMotionDriverNDim.CurrentTime"/> and
//        /// <see cref="IBrownianMotionDriverNDim.CurrentTimeDifference"/> beleave unchanged.
//        /// <para>Before calling this method be sure that <see cref="IBrownianMotionDriverNDim.ConstructionType"/> is set to <see cref="eBrownianMotionConstructionType.CompletePaths"/>. </para>
//        /// </remarks>
//        public bool GetAntitheticWienerIncrements(ref ILArray<double> antitheticWienerIncrements)
//        {
//            if (m_AntiteticValueAvailable == true)
//            {
//                antitheticWienerIncrements = m_FactorLoading.GetBrownianMotionIncrement(m_NumberOfRealisations, m_TempAntitheticNormalDistributedRandomNumbers);
//                return true;
//            }
//            return false;
//        }

//        #endregion

//        #endregion
//    }
//}