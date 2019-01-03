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
    /// <summary>Represents a Pseudo-Random Number Generator, i.e. serves as factory for <see cref="IRandomNumberStream"/> objects.
    /// </summary>
    public interface IPseudoRandomNumberGenerator : IRandomNumberGenerator
    {
        /// <summary>Gets the maximal value of the seed supported by the Random Number Generator.
        /// </summary>
        /// <value>The maximal value of the seed supported by the Random Number Generator.
        /// </value>
        long MaxSeed
        {
            get;
        }

        /// <summary>Gets the collection of sub-generator ID's.
        /// </summary>
        /// <value>The collection of sub-generator ID's.</value>
        /// <remarks>This property represents the collection of possible arguments for <see cref="IPseudoRandomNumberGenerator.Create(long, int)"/>.</remarks>
        IEnumerable<int> SubGeneratorIDs
        {
            get;
        }

        /// <summary>Creates a new random number stream.
        /// </summary>
        /// <param name="seed">The seed.</param>
        /// <returns>The random number stream in its <see cref="IRandomNumberStream"/> representation.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="seed"/> is invalid for the Random Number Generator represented by the current instance.</exception>
        /// <exception cref="InvalidOperationException">Thrown, if additional parameters are required.</exception>
        IRandomNumberStream Create(long seed);

        /// <summary>Creates a new random number stream.
        /// </summary>
        /// <param name="seed">The seed.</param>
        /// <returns>The random number stream in its <see cref="IRandomNumberStream"/> representation.</returns>
        /// <param name="subGeneratorID">A sub-generator ID if a set of similar Random Number Generator is represented by the current instance.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="seed"/> or <paramref name="subGeneratorID"/> is invalid for the (set of) Random Number Generator(s) represented by the current instance.</exception>
        /// <exception cref="InvalidOperationException">Thrown, if additional parameters are required.</exception>
        IRandomNumberStream Create(long seed, int subGeneratorID);

        /// <summary>Creates a new random number stream.
        /// </summary>
        /// <param name="initialConditions">Initial conditions for the Random Number Generator; <see cref="IRandomNumberGenerator.InitialConditions"/> serves as factory for initial conditions with respect to the Random Number Generator represented by the current instance.</param>
        /// <returns>The random number stream in its <see cref="IRandomNumberStream"/> representation.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="initialConditions"/> is not suitable for the Random Number Generator represented by the current instance.</exception>
        IRandomNumberStream Create(RandomNumberInitialConditions initialConditions);

        /// <summary>Creates a new random number stream.
        /// </summary>
        /// <param name="initialConditions">Initial conditions for the Random Number Generator; <see cref="IRandomNumberGenerator.InitialConditions"/> serves as factory for initial conditions with respect to the Random Number Generator represented by the current instance.</param>
        /// <param name="subGeneratorID">A sub-generator ID if a set of similar Random Number Generator is represented by the current instance.</param>
        /// <returns>The random number stream in its <see cref="IRandomNumberStream"/> representation.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="initialConditions"/> or <paramref name="subGeneratorID"/> is not suitable for the Random Number Generator represented by the current instance.</exception>
        IRandomNumberStream Create(RandomNumberInitialConditions initialConditions, int subGeneratorID);
    }
}