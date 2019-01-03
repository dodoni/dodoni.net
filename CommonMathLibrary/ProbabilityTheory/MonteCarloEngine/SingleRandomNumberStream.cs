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

namespace Dodoni.MathLibrary.ProbabilityTheory.MonteCarloEngine
{
    /// <summary>Serves as factory of uniformly distributed random numbers in [0,1], i.e. encapsulates a specific <see cref="IRandomNumberStream"/> object.
    /// </summary>
    public class SingleRandomNumberStream
    {
        #region private members

        /// <summary>The length of random number sequences to pull from the <see cref="IRandomNumberStream"/> object.
        /// </summary>
        private int m_SequenceLength;

        /// <summary>The random number sequence pulled from the specific <see cref="IRandomNumberStream"/> object.
        /// </summary>
        private double[] m_Data;

        /// <summary>The current index in <see cref="m_Data"/>.
        /// </summary>
        private int m_CurrentIndex;

        /// <summary>The specific <see cref="IRandomNumberStream"/> object to encapsulate.
        /// </summary>
        private IRandomNumberStream m_RandomNumberStream;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="SingleRandomNumberStream"/> class.
        /// </summary>
        /// <param name="randomNumberStream">The <see cref="IRandomNumberStream"/> object to encapsulate.</param>
        /// <param name="sequenceLength">The length of random number sequences to pull from <paramref name="randomNumberStream"/>.</param>
        public SingleRandomNumberStream(IRandomNumberStream randomNumberStream, int sequenceLength = 100)
        {
            if (randomNumberStream == null)
            {
                throw new ArgumentNullException("randomNumberStream");
            }
            m_SequenceLength = sequenceLength;
            m_RandomNumberStream = randomNumberStream;

            m_Data = new double[sequenceLength];
            m_CurrentIndex = -1;
        }
        #endregion

        #region public methods

        /// <summary>Gets the next uniformly distributed random number in [0,1].
        /// </summary>
        /// <returns>The next uniformly distributed random number in [0,1].</returns>
        public double GetNextDouble()
        {
            if ((m_CurrentIndex < 0) || (m_CurrentIndex >= m_SequenceLength - 1))
            {
                m_RandomNumberStream.NextNumberSequence.Uniform(m_SequenceLength, m_Data);
                m_CurrentIndex = -1;
            }
            m_CurrentIndex++;
            return m_Data[m_CurrentIndex];
        }
        #endregion
    }
}