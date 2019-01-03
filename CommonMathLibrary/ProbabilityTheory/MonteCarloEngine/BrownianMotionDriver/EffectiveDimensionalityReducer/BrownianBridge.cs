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
    /// <summary>Represents the implementation of the Brownian Motion path construction via the Brownian Bridge approch used
    /// for low discrepancy sequences.
    /// </summary>
    /// <remarks>
    /// The implementation is based on
    /// <para>Peter Jäckel, Monte Carlo methods in finance, p. 124ff.</para>
    /// </remarks>    
    public sealed class BrownianBridgeConversion : IEffectiveDimensionalityReducer
    {
        #region private members

        /// <summary>The number of discretisation steps.
        /// </summary>
        private int m_NumberOfDiscretisationSteps;

        /// <summary>The Brownian bridge index.
        /// </summary>
        private int[] m_BridgeIndex;

        /// <summary>The left index.
        /// </summary>
        private int[] m_LeftIndex;

        /// <summary>The right index.
        /// </summary>
        private int[] m_RightIndex;

        /// <summary>Weights used for <see cref="m_LeftIndex"/>.
        /// </summary>
        private double[] m_LeftWeights;

        /// <summary>Weights used for <see cref="m_RightIndex"/>.
        /// </summary>
        private double[] m_RightWeights;

        /// <summary>The standard deviations.
        /// </summary>
        private double[] m_StandardDeviations;

        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="BrownianBridgeConversion"/> class.
        /// </summary>
        public BrownianBridgeConversion()
        {
            m_NumberOfDiscretisationSteps = 0;
        }

        #endregion

        #region public properties

        #region IEffectiveDimensionalityReducer Members

        /// <summary>Gets a value indicating whether this instance is operable.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is operable; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// 	<c>false</c> will be returned if the current instance is not initialized completly.</remarks>
        public bool IsOperable
        {
            get { return (m_NumberOfDiscretisationSteps >= 1); }
        }

        #endregion

        /// <summary>Gets the number of discretisation steps.
        /// </summary>
        /// <value>The number of discretisation steps.</value>
        public int NumberOfDiscretisationSteps
        {
            get { return m_NumberOfDiscretisationSteps; }
        }

        #endregion

        #region public methods

        #region IEffectiveDimensionalityReducer Members

        /// <summary>Reinitializes the current instance with a given number of discretisation steps.
        /// </summary>
        /// <param name="numberOfDiscretisationSteps">The number of discretisation steps.</param>
        /// <returns>
        /// A value indicating whether the current instance has been reinitialized.
        /// </returns>
        public bool Reinitialize(int numberOfDiscretisationSteps)
        {
            if (numberOfDiscretisationSteps == 0)
            {
                return false;
            }
            if (m_NumberOfDiscretisationSteps == numberOfDiscretisationSteps)
            {
                return true;
            }
            m_NumberOfDiscretisationSteps = numberOfDiscretisationSteps;
            m_BridgeIndex = new int[numberOfDiscretisationSteps];
            m_LeftIndex = new int[numberOfDiscretisationSteps];
            m_RightIndex = new int[numberOfDiscretisationSteps];
            m_LeftWeights = new double[numberOfDiscretisationSteps];
            m_RightWeights = new double[numberOfDiscretisationSteps];
            m_StandardDeviations = new double[numberOfDiscretisationSteps];

            // map is used to indicate which points are already constructed. If map[i] is zero, path point i is yet unconstructed.
            // map[i]-1 is the index of the variate that constructs the path point # i.
            int[] map = new int[m_NumberOfDiscretisationSteps];

            int i, j, k, mapIndex;
            map[m_NumberOfDiscretisationSteps - 1] = 1;               //  The first point in the construction is the global step.            
            m_BridgeIndex[0] = m_NumberOfDiscretisationSteps - 1;     //  The global step is constructed from the first variate.
            m_StandardDeviations[0] = Math.Sqrt(m_NumberOfDiscretisationSteps);  //  The variance of the global step
            m_LeftWeights[0] = m_RightWeights[0] = 0.0;               //  The global step to the last point in time is special.

            j = 0;
            for (i = 1; i < m_NumberOfDiscretisationSteps; ++i)
            {
                while (map[j] != 0)   // Find the next unpopulated entry in the map.
                {
                    ++j;
                }
                k = j;
                while (map[k] == 0)   // Find the next populated entry in the map from there.
                {
                    ++k;
                }
                mapIndex = j + ((k - 1 - j) >> 1);   // 'mapIndex' is now the index of the point to be constructed next.
                map[mapIndex] = i;

                m_BridgeIndex[i] = mapIndex;         // The i-th Gaussian variate will be used to set point 'mapIndex'.
                m_LeftIndex[i] = j;           // Point j-l is the left strut of the bridge for point 'mapIndex'.
                m_RightIndex[i] = k;          // Point  k is the right strut of the bridge for point 'mapIndex'.
                m_LeftWeights[i] = (k - mapIndex) / (k + 1.0 - j);

                m_RightWeights[i] = (mapIndex + 1.0 - j) / (k + 1.0 - j);
                m_StandardDeviations[i] = Math.Sqrt((mapIndex + 1.0 - j) * (k - mapIndex) / (k + 1.0 - j));

                j = k + 1;
                if (j >= m_NumberOfDiscretisationSteps)
                {
                    j = 0;    //  wrap around
                }
            }
            return true;
        }

        /// <summary>Gets a sequence of Standard-Normal distributed numbers which are the result of some conversion of Standard-Normal distributed numbers.
        /// </summary>
        /// <param name="normalDistributedSample">The normal distributed sample, i.e. a one-dimensional array which contains
        /// normal distributed random number.</param>
        /// <param name="convertedNormalDistributedSample">Normal distributed numbers which are the output of the conversion of <paramref name="normalDistributedSample"/>.
        /// Thus a one-dimensional array where the lenght is given by the number of elements in <paramref name="normalDistributedSample"/>.</param>
        /// <returns>
        /// A value indicating whether <paramref name="convertedNormalDistributedSample"/> contains valid data after the function call.
        /// </returns>
        //public bool GetConvertedNormalDistributedSequence(ILArray<double> normalDistributedSample, ILArray<double> convertedNormalDistributedSample)
        //{
        //    if (m_NumberOfDiscretisationSteps == 0)
        //    {
        //        return false;
        //    }
        //    convertedNormalDistributedSample.SetValue(m_StandardDeviations[0] * normalDistributedSample.GetValue(0), m_NumberOfDiscretisationSteps - 1);
        //    int j, k, l;
        //    for (int i = 1; i < m_NumberOfDiscretisationSteps; i++)
        //    {
        //        j = m_LeftIndex[i];
        //        k = m_RightIndex[i];
        //        l = m_BridgeIndex[i];
        //        if (j != 0)
        //        {
        //            convertedNormalDistributedSample.SetValue(m_LeftWeights[i] * convertedNormalDistributedSample.GetValue(j - 1) + m_RightWeights[i] * convertedNormalDistributedSample.GetValue(k) + m_StandardDeviations[i] * normalDistributedSample.GetValue(i), l);
        //        }
        //        else
        //        {
        //            convertedNormalDistributedSample.SetValue(m_RightWeights[i] * convertedNormalDistributedSample.GetValue(k) + m_StandardDeviations[i] * normalDistributedSample.GetValue(i), l);
        //        }
        //    }
        //    /* we return the differences (increments) and not the values itself - see remarks in Peter Jäckel, 'Monte Carlo methods in finance', p. 124ff. */
        //    for (int i = 1; i < m_NumberOfDiscretisationSteps; i++)
        //    {
        //        convertedNormalDistributedSample.SetValue(convertedNormalDistributedSample.GetValue(i) - convertedNormalDistributedSample.GetValue(i - 1), i);
        //    }
        //    return true;
        //}

        /// <summary>Gets a matrix of Standard-Normal distributed numbers which are the result of some conversion of Standard-Normal distributed numbers.
        /// </summary>
        /// <param name="numberOfRealisations">The number of realisations.</param>
        /// <param name="normalDistributedSampleMatrix">The normal distributed samples, i.e. a two-dimensional array where the first
        /// null-based index corresponds to the <see cref="IEffectiveDimensionalityReducer.NumberOfDiscretisationSteps"/> and the second index corresponds to the
        /// <paramref name="numberOfRealisations"/> of the normal distributed entries.</param>
        /// <param name="convertedNormalDistributedMatrix">Normal distributed numberw which are the output of the conversion of <paramref name="normalDistributedSampleMatrix"/>.
        /// Thus a two-dimensional array where the first null-based index corresponds in general to the number of time steps of some time discretisation and the second
        /// index corresponds to the realisation.</param>
        /// <returns>
        /// A value indicating whether <paramref name="convertedNormalDistributedMatrix"/> contains valid data after the function call.
        /// </returns>
        /// <remarks>The number of rows and columns of the output matrix <paramref name="convertedNormalDistributedMatrix"/> will be the same as
        /// of <paramref name="normalDistributedSampleMatrix"/>.</remarks>
        //public bool GetConvertedNormalDistributedSequences(int numberOfRealisations, ILArray<double> normalDistributedSampleMatrix, ILArray<double> convertedNormalDistributedMatrix)
        //{
        //    if (m_NumberOfDiscretisationSteps == 0)
        //    {
        //        return false;
        //    }
        //    int j, k, l;

        //    for (int realisationIndex = 0; realisationIndex < numberOfRealisations; realisationIndex++)  // multi-thread?
        //    {
        //        convertedNormalDistributedMatrix.SetValue(m_StandardDeviations[0] * normalDistributedSampleMatrix.GetValue(0, realisationIndex), m_NumberOfDiscretisationSteps - 1, realisationIndex);

        //        for (int i = 1; i < m_NumberOfDiscretisationSteps; i++)
        //        {
        //            j = m_LeftIndex[i];
        //            k = m_RightIndex[i];
        //            l = m_BridgeIndex[i];
        //            if (j != 0)
        //            {
        //                convertedNormalDistributedMatrix.SetValue(m_LeftWeights[i] * convertedNormalDistributedMatrix.GetValue(j - 1, realisationIndex) + m_RightWeights[i] * convertedNormalDistributedMatrix.GetValue(k, realisationIndex) + m_StandardDeviations[i] * normalDistributedSampleMatrix.GetValue(i, realisationIndex), l, realisationIndex);
        //            }
        //            else
        //            {
        //                convertedNormalDistributedMatrix.SetValue(m_RightWeights[i] * convertedNormalDistributedMatrix.GetValue(k, realisationIndex) + m_StandardDeviations[i] * normalDistributedSampleMatrix.GetValue(i, realisationIndex), l, realisationIndex);
        //            }
        //        }

        //        /* we return the differences (increments) and not the values itself - see remarks in Peter Jäckel, 'Monte Carlo methods in finance', p. 124ff. */
        //        for (int i = 1; i < m_NumberOfDiscretisationSteps; i++)
        //        {
        //            convertedNormalDistributedMatrix.SetValue(convertedNormalDistributedMatrix.GetValue(i, realisationIndex) - convertedNormalDistributedMatrix.GetValue(i - 1, realisationIndex), i, realisationIndex);
        //        }
        //    }
        //    return true;
        //}

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
        /// <returns>
        /// A value indicating whether <paramref name="convertedNormalDistributedMatrix"/> contains valid data after the function call.
        /// </returns>
        /// <remarks>The number of rows and columns of the output matrix <paramref name="convertedNormalDistributedMatrix"/> will be the same as
        /// of <paramref name="normalDistributedSampleMatrix"/>.</remarks>
        //public bool GetConvertedNormalDistributedSequences(int dimension, int numberOfRealisations, ILArray<double> normalDistributedSampleMatrix, ILArray<double> convertedNormalDistributedMatrix)
        //{
        //    if (m_NumberOfDiscretisationSteps == 0)
        //    {
        //        return false;
        //    }
        //    int j, k, l;

        //    for (int realisationIndex = 0; realisationIndex < numberOfRealisations; realisationIndex++)  // multi-thread?
        //    {
        //        for (int iDimension = 0; iDimension < dimension; iDimension++)
        //        {
        //            convertedNormalDistributedMatrix.SetValue(m_StandardDeviations[0] * normalDistributedSampleMatrix.GetValue(iDimension, realisationIndex), m_NumberOfDiscretisationSteps - 1, iDimension, realisationIndex);

        //            for (int i = 1; i < m_NumberOfDiscretisationSteps; i++)
        //            {
        //                j = m_LeftIndex[i];
        //                k = m_RightIndex[i];
        //                l = m_BridgeIndex[i];
        //                if (j != 0)
        //                {
        //                    convertedNormalDistributedMatrix.SetValue(m_LeftWeights[i] * convertedNormalDistributedMatrix.GetValue(j - 1, iDimension, realisationIndex) + m_RightWeights[i] * convertedNormalDistributedMatrix.GetValue(k, iDimension, realisationIndex) + m_StandardDeviations[i] * normalDistributedSampleMatrix.GetValue(i, realisationIndex), l, iDimension, realisationIndex);
        //                }
        //                else
        //                {
        //                    convertedNormalDistributedMatrix.SetValue(m_RightWeights[i] * convertedNormalDistributedMatrix.GetValue(k, iDimension, realisationIndex) + m_StandardDeviations[i] * normalDistributedSampleMatrix.GetValue(iDimension + i, realisationIndex), l, iDimension, realisationIndex);
        //                }
        //            }
        //            /* we return the differences (increments) and not the values itself - see remarks in Peter Jäckel, 'Monte Carlo methods in finance', p. 124ff. */
        //            for (int i = 1; i < m_NumberOfDiscretisationSteps; i++)
        //            {
        //                convertedNormalDistributedMatrix.SetValue(convertedNormalDistributedMatrix.GetValue(i, iDimension, realisationIndex) - convertedNormalDistributedMatrix.GetValue(i - 1, iDimension, realisationIndex), i, iDimension, realisationIndex);
        //            }
        //        }
        //    }
        //    return true;
        //}

        #endregion

        #endregion
    }
}