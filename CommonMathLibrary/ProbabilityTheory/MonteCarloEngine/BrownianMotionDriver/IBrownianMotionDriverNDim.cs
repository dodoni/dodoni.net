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
    /// <summary>Serves as interface for the generation of Brownian motion realisations, i.e. for
    /// a given dimension of the Brownian motion, some discretisation ot the time horizont and some number of realisations to
    /// create, Wiener increments will be generated.
    /// </summary>
    public interface IBrownianMotionDriverNDim
    {
        #region properties

        /// <summary>Gets the dimension of the Brownian motion.
        /// </summary>
        /// <value>The dimension.</value>
        int Dimension
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

        /// <summary>Gets the number of realisations.
        /// </summary>
        /// <value>The number of realisations.</value>
        int NumberOfRealisations
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

        /// <summary>Gets a value indicating whether the current instances make available antithetic paths.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [make available antithetic paths]; otherwise, <c>false</c>.
        /// </value>
        bool MakeAvailableAntitheticPaths
        {
            get;
        }

        /// <summary>Gets the construction type of the Brownian motion paths and Brownian motion increment generation.
        /// </summary>
        /// <value>The type of the construction.</value>
        eBrownianMotionConstructionType ConstructionType
        {
            get;
        }

        #endregion

        #region methods

        /// <summary>Reinitializes the current instance with some specified generation type and a number of realisations.
        /// </summary>
        /// <param name="constructionType">The Type of the construction.</param>
        /// <param name="numberOfRealisations">The number of realisations.</param>
        /// <remarks>It is not allowed to mix different path construction modes, i.e. either one wants to create for each point in time 
        /// one-step realisations or all path/increment realisations will be generated.</remarks>
        /// <returns>A value indicating whether the current instance has changed the construction type and the number of realisations.</returns>
        bool Reinitialize(eBrownianMotionConstructionType constructionType, int numberOfRealisations);

        /// <summary>Reset the time discretisation to the first point in time, i.e. <see cref="IBrownianMotionDriverNDim.CurrentTimeIndex"/> will be set to <c>0</c> thus 
        /// <see cref="IBrownianMotionDriverNDim.CurrentTime"/> and <see cref="IBrownianMotionDriverNDim.CurrentTimeDifference"/> to <c>0.0</c>.
        /// </summary>
        void ResetCurrentTimeIndex();

        /// <summary>Resets the sequence generator, i.e. if the sequence generator allows resetting it will produces the same sequences again.
        /// </summary>
        /// <returns><c>true</c>, if the sequence generator was reset; otherwise, <c>false</c>.</returns>
        bool ResetSequenceGenerator();

        /// <summary>Gets Wiener increments for the next point in time, i.e. for the next time 
        /// </summary>
        /// <param name="wienerIncrements">The Wiener increments for the point in time which is represented by the value of <see cref="IBrownianMotionDriverNDim.CurrentTimeIndex"/>
        /// <c>after</c> the function call, where the first null-based index corresponds to the <see cref="IBrownianMotionDriverNDim.Dimension"/> and the second index corresponds 
        /// to the <see cref="IBrownianMotionDriverNDim.NumberOfRealisations"/>. </param>
        /// <returns>A value indicating whether <paramref name="wienerIncrements"/> contains valid data.
        /// </returns>
        /// <remarks>The properties <see cref="IBrownianMotionDriverNDim.CurrentTimeIndex"/>, <see cref="IBrownianMotionDriverNDim.CurrentTime"/> and <see cref="IBrownianMotionDriverNDim.CurrentTimeDifference"/> 
        /// might be changed. If there are no further points in time available <c>false</c> will returned.
        /// <para>Before calling this method be sure that <see cref="IBrownianMotionDriverNDim.ConstructionType"/> is set to <see cref="eBrownianMotionConstructionType.StepByStep"/>. </para>
        /// </remarks>
//        bool GetNextOneStepWienerIncrements(ref ILArray<double> wienerIncrements);

        /// <summary>Gets the antithetic Wiener increments with respect to the last call of <see cref="GetNextOneStepWienerIncrements(ref ILArray&lt;double&gt;)"/>.
        /// </summary>
        /// <param name="antitheticWienerIncrements">The antithetic Wiener increments with respect to the previous call of <see cref="GetNextOneStepWienerIncrements(ref ILArray&lt;double&gt;)"/>,
        /// i.e. the first null-based index corresponds to the <see cref="IBrownianMotionDriverNDim.Dimension"/> and the second index corresponds 
        /// to the <see cref="IBrownianMotionDriverNDim.NumberOfRealisations"/>.</param>
        /// <returns>A value indicating whether <paramref name="antitheticWienerIncrements"/> contains valid data.
        /// </returns>
        /// <remarks>The properties <see cref="IBrownianMotionDriverNDim.CurrentTimeIndex"/>, <see cref="IBrownianMotionDriverNDim.CurrentTime"/> and <see cref="IBrownianMotionDriverNDim.CurrentTimeDifference"/> 
        /// belief unchanged. If there are no further points in time available <c>false</c> will returned.
        /// <para>Before calling this method be sure that <see cref="IBrownianMotionDriverNDim.ConstructionType"/> is set to <see cref="eBrownianMotionConstructionType.StepByStep"/>. </para>
        /// </remarks>
  //      bool GetOneStepAntitheticWienerIncrements(ref ILArray<double> antitheticWienerIncrements);

        /// <summary>Gets the Wiener increments for each point in time, i.e. simultaneously for each discretisation step and each realisation,
        /// where the number of realisations is represented by <see cref="IBrownianMotionDriverNDim.NumberOfRealisations"/>.
        /// </summary>
        /// <param name="wienerIncrements">The Wiener increments for each discretisation step, i.e. the first null-based index corresponds to the time discretisation, 
        /// the second to the <see cref="IBrownianMotionDriverNDim.Dimension"/> and the last for the realisation (see <see cref="IBrownianMotionDriverNDim.NumberOfRealisations"/>).</param>
        /// <returns>A value indicating whether <paramref name="wienerIncrements"/> contains valid data.
        /// </returns>
        /// <remarks>The properties <see cref="IBrownianMotionDriverNDim.CurrentTimeIndex"/>, <see cref="IBrownianMotionDriverNDim.CurrentTime"/> and 
        /// <see cref="IBrownianMotionDriverNDim.CurrentTimeDifference"/> beleave unchanged.
        /// <para>Before calling this method be sure that <see cref="IBrownianMotionDriverNDim.ConstructionType"/> is set to <see cref="eBrownianMotionConstructionType.CompletePaths"/>. </para>
        /// </remarks>
 //       bool GetWienerIncrements(ref ILArray<double> wienerIncrements);

        /// <summary>Gets the antithetic wiener increments with respect to the last call of <see cref="GetWienerIncrements(ref ILArray&lt;double&gt;)"/>.
        /// </summary>
        /// <param name="antitheticWienerIncrements">The antithetic Wiener increments with respect to the previous call of <see cref="GetWienerIncrements(ref ILArray&lt;double&gt;)"/>,
        /// i.e. the first null-based index corresponds to the time discretisation, the second to the <see cref="IBrownianMotionDriverNDim.Dimension"/> and the 
        /// last for the realisation (see <see cref="IBrownianMotionDriverNDim.NumberOfRealisations"/>).</param>
        /// <returns>A value indicating whether <paramref name="antitheticWienerIncrements"/> contains valid data.
        /// </returns>
        /// <remarks>The properties <see cref="IBrownianMotionDriverNDim.CurrentTimeIndex"/>, <see cref="IBrownianMotionDriverNDim.CurrentTime"/> and 
        /// <see cref="IBrownianMotionDriverNDim.CurrentTimeDifference"/> beleave unchanged.
        /// <para>Before calling this method be sure that <see cref="IBrownianMotionDriverNDim.ConstructionType"/> is set to <see cref="eBrownianMotionConstructionType.CompletePaths"/>. </para>
        /// </remarks>
 //       bool GetAntitheticWienerIncrements(ref ILArray<double> antitheticWienerIncrements);

        #endregion
    }
}