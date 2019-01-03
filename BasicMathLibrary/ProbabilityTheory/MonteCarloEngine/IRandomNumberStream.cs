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

using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.ProbabilityTheory.MonteCarloEngine
{
    /// <summary>Serves as interface for a random stream.
    /// </summary>
    /// <remarks>Each (external) random number library has is own stream representation, perhaps a pointer of an internal data structure.</remarks>
    public interface IRandomNumberStream : IOperable, IDisposable
    {
        #region properties

        /// <summary>Gets the Random Number Generator related to the random stream.
        /// </summary>
        /// <value>The Random Number Generator.</value>
        IRandomNumberGenerator Generator
        {
            get;
        }

        /// <summary>Gets a (the next) sequence of random numbers with respect to a specified distribution.
        /// </summary>
        /// <value>Random numbers with respect to a specified distribution.</value>
        /// <remarks>In general random numbers will not be cached, i.e. each access to this property yields other random numbers.</remarks>
        RandomNumberSequence.IDistribution NextNumberSequence
        {
            get;
        }

        /// <summary>Gets a value indicating whether splitting of a random number sequence into non-overlapping subsequences is supported.
        /// </summary>
        /// <value>The splitting approach.</value>
        RandomNumberSequence.SplittingApproach SplittingApproach
        {
            get;
        }

        /// <summary>Gets the dimension of the sequences, i.e. the dimension of the Quasi-Random Number Generator or <c>1</c> if the
        /// current instance represents a Pseudo-Random Number Generator.
        /// </summary>
        /// <value>The dimension of a Quasi-Random Number Generator or <c>1</c>.</value>
        long Dimension
        {
            get;
        }
        #endregion

        #region methods

        /// <summary>Creates a (deep) copy of the current random stream object.
        /// </summary>
        /// <returns>A (deep) copy of the current instance.</returns>
        IRandomNumberStream CopyStream();

        /// <summary>Overrides the stream state data of a specific stream object with the data of the current object.
        /// </summary>
        /// <param name="destinationStream">The destination random number stream.</param>
        /// <exception cref="ArgumentException">Thrown, if the stream of the current instance and the stream represented by <paramref name="destinationStream"/> are not compatible.</exception>
        /// <remarks>Stream state data are for example the dimension etc.</remarks>
        void CopyStreamStateTo(IRandomNumberStream destinationStream);

        /// <summary>Creates a copy of the current <see cref="IRandomNumberStream"/> object and apply Leapfrog approach to it.
        /// </summary>
        /// <param name="streamIndex">The null-based index of the computational node (stream index), 0 &lt;= <paramref name="streamIndex"/> &lt; <paramref name="numberOfStreams"/>.</param>
        /// <param name="numberOfStreams">The number of streams, i.e. largest number of computational nodes, or stride. In the case of a Quasi-Random Number Generator this parameter should be equal to the dimension.
        /// </param>
        /// <returns>A (deep) copy of the current instance which applies the Leapfrog approach.</returns>
        /// <exception cref="ArgumentException">Thrown, if the current object does not support the Leapfrog method or the arguments are invalid.</exception>
        /// <remarks>This feature is particularly useful in distributing random numbers from the original stream across the <paramref name="numberOfStreams"/> buffers. One of the important applications 
        /// of the Leapfrog method is splitting the original sequence into non-overlapping subsequences across <paramref name="numberOfStreams"/> computational nodes.
        /// For Quasi-Random Number Generators, the Leapfrog method allows generating individual components of Quasi-Random Vectors instead of whole 
        /// Quasi-Random Vectors. In this case <paramref name="numberOfStreams"/> should be equal to the dimension of the Quasi-Random Vector.
        /// </remarks>
        IRandomNumberStream CreateLeapfrogStream(int streamIndex, int numberOfStreams);

        /// <summary>Creates a copy of the current <see cref="IRandomNumberStream"/> object and apply Block-splitting approach to it.
        /// </summary>
        /// <param name="numberOfSkippedElements">The number of skipped elements.</param>
        /// <returns>A (deep) copy of the current instance which applies the Block-splitting approach.</returns>
        /// <exception cref="ArgumentException">Thrown, if the current object does not support the Block-split approach or the arguments are invalid.</exception>
        /// <remarks>This method skips a specified number of elements in a random stream. This feature is particularly useful in distributing random numbers from 
        /// original random stream across different computational nodes. 
        /// If the largest number of random numbers used by a computational node is <paramref name="numberOfSkippedElements"/>, then the original random sequence 
        /// may be split into non-overlapping blocks of <paramref name="numberOfSkippedElements"/> size so that each block corresponds to the respective computational node. 
        /// The number of computational nodes is unlimited.
        /// <para>
        /// Note that for Quasi-Random Number Generators the Skip-ahead method works with components of Quasi-Random Vectors rather than with whole Quasi-Random Vectors. 
        /// Therefore, to skip <c>n</c> Quasi-Random Vectors, set <paramref name="numberOfSkippedElements"/> to <c>n</c> * <see cref="IRandomNumberStream.Dimension"/>. 
        /// If this operation results in exceeding the period of the Quasi-Random Number Generator, an exception will be thrown.
        /// </para>
        /// </remarks>
        IRandomNumberStream CreateSkipAheadStream(int numberOfSkippedElements);
        #endregion
    }
}