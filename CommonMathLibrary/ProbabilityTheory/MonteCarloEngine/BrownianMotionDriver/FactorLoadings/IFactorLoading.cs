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

namespace Dodoni.MathLibrary.ProbabilityTheory.MonteCarlo.BrownianMotionDriver
{
    /// <summary>Serves as interface for factor loadings which are neede for the generation of correlated Brownian motions.
    /// </summary>
    /// <remarks>
    /// Let 0 = t_0 t_1 ... t_n = T be some given discretization of the time horizont [0,T]. For the construction of correlated Brownian motions
    /// assume that a vector of independent Brownian motions (Z^1,...,Z^d) is given and consider the point in time t_i. The correlated Brownian
    /// motions are given via
    /// <para>
    ///    W^k = \sum_{j} a_{k,j} dZ^j,
    /// </para>
    /// where a_{k,j} represents the correlation as well as the square root of time and perhaps volatilites, i.e. these matrix depends on the time
    /// t_i as well.
    /// <para>At time 0 the Brownian motions are deterministic and zero therefore these values will not taken into account.</para>
    /// </remarks>
    public interface IFactorLoading
    {
        #region properties

        /// <summary>Gets the number of factors, i.e. the number of independent Brownian motions taken into account.
        /// </summary>
        /// <value>The number of factors.</value>
        int NumberOfFactors
        {
            get;
        }

        /// <summary>Gets the number of time steps, which corresponds to the discretization of the time horizont including the initial point in time <c>0.0</c>.
        /// </summary>
        /// <value>The number of time steps.</value>
        int NumberOfTimeSteps
        {
            get;
        }

        /// <summary>Gets the null-based index of the current time step, which corresponds to the discretization of the time horizont.
        /// </summary>
        /// <value>The index of the current time.</value>
        /// <remarks>The value <c>0</c> corresponds to the point in time <c>0.0</c> where the value of the (correlated) Brownian motions are <c>0.0</c> as well.</remarks>
        int CurrentTimeIndex
        {
            get;
        }

        /// <summary>Gets the current time, i.e. the <see cref="System.Double"/> representation of the current point in time.
        /// </summary>
        /// <value>The current time.</value>
        double CurrentTime
        {
            get;
        }

        /// <summary>Gets the current time difference between the current point in time and the previous point in time.
        /// </summary>
        /// <value>The current time difference.</value>
        /// <remarks>If <see cref="CurrentTimeIndex"/> is equal to <c>0</c> this property will return <c>0.0</c> as well.</remarks>
        double CurrentTimeDifference
        {
            get;
        }

        #endregion

        #region methods

        /// <summary>Resets the current instance, i.e. <see cref="CurrentTimeIndex"/> will be set to <c>0</c> thus to time <c>0.0</c>, i.e.
        /// the first point of the given time discretization.
        /// </summary>
        void Reset();

        /// <summary>Points to the next point in time, i.e. to the next discretization step.
        /// </summary>
        /// <returns>A value indicating whether for the current instance there is some additional point in time available.</returns>
        bool NextPointInTime();

        /// <summary>Gets realisations of the Brownian motion increments for the <see cref="CurrentTimeDifference"/>.
        /// </summary>
        /// <param name="numberOfRealisations">The number of realisations.</param>
        /// <param name="normalDistributedNumbers">The normal distributed numbers, where the first null-based index corresponds to the 
        /// <see cref="IFactorLoading.NumberOfFactors"/> and the second index corresponds to the <paramref name="numberOfRealisations"/>.</param>
        /// <returns>The Brownian motion increments for the <see cref="CurrentTimeDifference"/>, where the first null-based index corresponds to 
        /// the <see cref="IFactorLoading.NumberOfFactors"/> and the second index corresponds to the <paramref name="numberOfRealisations"/>.</returns>
   //     ILArray<double> GetOneStepBrownianMotionIncrements(int numberOfRealisations, ILArray<double> normalDistributedNumbers);

        /// <summary>Gets a realisation of the Brownian motion increments for the <see cref="CurrentTimeDifference"/>.
        /// </summary>
        /// <param name="normalDistributedNumbers">The normal distributed numbers, i.e. a list of <see cref="System.Double"/> instances, where the 
        /// null-based index corresponds to the <see cref="IFactorLoading.NumberOfFactors"/>.</param>
        /// <returns>The Brownian motion increments for the <see cref="CurrentTimeDifference"/>, where the null-based index corresponds to 
        /// the <see cref="IFactorLoading.NumberOfFactors"/>.</returns>
 //       ILArray<double> GetOneStepBrownianMotionIncrements(ILArray<double> normalDistributedNumbers);

        /// <summary>Gets realisations of the Brownian motion increments from the first point in time (not <c>0.0</c>) up to the 
        /// last point in time of the discretization of the time horizont, i.e. <see cref="CurrentTimeIndex"/> will not be taken into account,
        /// the whole time horizont will be used instead.
        /// </summary>
        /// <param name="numberOfRealisations">The number of realisations.</param>
        /// <param name="normalDistributedNumbers">The normal distributed numbers, where the first null-based index corresponds to
        /// the time discretization (the first element does not belong to time <c>0.0</c> but to the first point in time which is destinct from <c>0.0</c>),
        /// the second index corresponds to the <see cref="IFactorLoading.NumberOfFactors"/> and the third null-based index corresponds to the
        /// <paramref name="numberOfRealisations"/>.
        /// </param>
        /// <returns>The Brownian motion increments for the whole time horizont, thus the first null-based index corresponds to
        /// the time discretization (the first element does not belong to time <c>0.0</c> but to the first point in time which is destinct from <c>0.0</c>),
        /// the second index corresponds to the <see cref="IFactorLoading.NumberOfFactors"/> and the third null-based index corresponds to the
        /// <paramref name="numberOfRealisations"/>.</returns>
        /// <remarks><see cref="CurrentTimeIndex"/> will not taken into account and will not be changed. Remember that the Brownian
        /// increments are not defined at time <c>0.0</c>.</remarks>
 //       ILArray<double> GetBrownianMotionIncrement(int numberOfRealisations, ILArray<double> normalDistributedNumbers);

        #endregion
    }
}