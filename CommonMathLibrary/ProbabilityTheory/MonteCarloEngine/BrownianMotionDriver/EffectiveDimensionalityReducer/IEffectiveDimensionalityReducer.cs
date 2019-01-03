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
    /// <summary>Serves as an interface for the conversion of Standard-Normal distributed numbers to Standard-Normal distributed numbers
    /// as used for low discrepancy sequences in the Brownian Bridge approach.
    /// </summary>
    [CLSCompliant(false)]
    public interface IEffectiveDimensionalityReducer
    {
        #region properties

        /// <summary>Gets a value indicating whether this instance is operable.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is operable; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// <c>false</c> will be returned if the current instance is not initialized completly.</remarks>
        bool IsOperable
        {
            get;
        }

        /// <summary>Gets the number of discretisation steps.
        /// </summary>
        /// <value>The number of discretisation steps.</value>
        int NumberOfDiscretisationSteps
        {
            get;
        }

        #endregion

        #region methods

        /// <summary>Reinitializes the current instance with a given number of discretisation steps.
        /// </summary>
        /// <param name="numberOfDiscretisationSteps">The number of discretisation steps.</param>
        /// <returns>A value indicating whether the current instance has been reinitialized.</returns>
        bool Reinitialize(int numberOfDiscretisationSteps);

        /// <summary>Gets a sequence of Standard-Normal distributed numbers which are the result of some conversion of Standard-Normal distributed numbers.
        /// </summary>
        /// <param name="normalDistributedSample">The normal distributed sample, i.e. a one-dimensional array which contains
        /// normal distributed random number.</param>
        /// <param name="convertedNormalDistributedSample">Normal distributed numbers which are the output of the conversion of <paramref name="normalDistributedSample"/>.
        /// Thus a one-dimensional array where the lenght is given by the number of elements in <paramref name="normalDistributedSample"/>.</param>
        /// <returns>A value indicating whether <paramref name="convertedNormalDistributedSample"/> contains valid data after the function call.</returns>
     //   bool GetConvertedNormalDistributedSequence(ILArray<double> normalDistributedSample, ILArray<double> convertedNormalDistributedSample);

        /// <summary>Gets a matrix of Standard-Normal distributed numbers which are the result of some conversion of Standard-Normal distributed numbers.
        /// </summary>
        /// <param name="numberOfRealisations">The number of realisations.</param>
        /// <param name="normalDistributedSampleMatrix">The normal distributed samples, i.e. a two-dimensional array where the first 
        /// null-based index corresponds to the <see cref="IEffectiveDimensionalityReducer.NumberOfDiscretisationSteps"/> and the second index corresponds to the 
        /// <paramref name="numberOfRealisations"/> of the normal distributed entries.</param>
        /// <param name="convertedNormalDistributedMatrix">Normal distributed numberw which are the output of the conversion of <paramref name="normalDistributedSampleMatrix"/>.
        /// Thus a two-dimensional array where the first null-based index corresponds in general to the number of time steps of some time discretisation and the second 
        /// index corresponds to the realisation.</param>
        /// <returns>A value indicating whether <paramref name="convertedNormalDistributedMatrix"/> contains valid data after the function call.</returns>
        /// <remarks>The number of rows and columns of the output matrix <paramref name="convertedNormalDistributedMatrix"/> will be the same as
        /// of <paramref name="normalDistributedSampleMatrix"/>.</remarks>
   //     bool GetConvertedNormalDistributedSequences(int numberOfRealisations, ILArray<double> normalDistributedSampleMatrix, ILArray<double> convertedNormalDistributedMatrix);

        /// <summary>Gets a matrix of Standard-Normal distributed numbers which are the result of some conversion of Standard-Normal distributed numbers.
        /// </summary>
        /// <param name="dimension">The dimension (of some Brownian Motion).</param>
        /// <param name="numberOfRealisations">The number of realisations.</param>
        /// <param name="normalDistributedSampleMatrix">The normal distributed samples, i.e. a two-dimensional array where the first 
        /// null-based index corresponds to the sequence lenght which is given by <see cref="IEffectiveDimensionalityReducer.NumberOfDiscretisationSteps"/> * <paramref name="dimension"/> 
        /// and the second index corresponds to the <paramref name="numberOfRealisations"/> of the normal distributed entries.</param>
        /// <param name="convertedNormalDistributedMatrix">Normal distributed numberw which are the output of the conversion of <paramref name="normalDistributedSampleMatrix"/>.
        /// Thus a three-dimensional array where the first null-based index corresponds to the <see cref="IEffectiveDimensionalityReducer.NumberOfDiscretisationSteps"/>, the second 
        /// index corresponds to the <paramref name="dimension"/> and the third index corresponds to the realisation.</param>
        /// <returns>A value indicating whether <paramref name="convertedNormalDistributedMatrix"/> contains valid data after the function call.</returns>
        /// <remarks>The number of rows and columns of the output matrix <paramref name="convertedNormalDistributedMatrix"/> will be the same as
        /// of <paramref name="normalDistributedSampleMatrix"/>.</remarks>
  //      bool GetConvertedNormalDistributedSequences(int dimension, int numberOfRealisations, ILArray<double> normalDistributedSampleMatrix, ILArray<double> convertedNormalDistributedMatrix);

        #endregion
    }
}