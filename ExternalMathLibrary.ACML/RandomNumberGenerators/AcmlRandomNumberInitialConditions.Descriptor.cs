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
using System.Linq;
using System.Collections.Generic;

namespace Dodoni.MathLibrary.ProbabilityTheory.MonteCarloEngine
{
    /// <summary>Represents initial conditions of a specified Random Number Generator of the ACML Library, <seealso cref="IRandomNumberGenerator"/>.
    /// </summary>
    /// <remarks>This class is used to indicate whether additional initial conditions are required for the creation of <see cref="IRandomNumberStream"/> objects. 
    /// Moreover it contains a short description and perhaps a set of standard initial conditions etc.</remarks>
    internal partial class AcmlRandomNumberInitialConditions
    {
        /// <summary>Serves as descriptor and factory for initial conditions of a specified Random Number Generator of the ACML Library.
        /// </summary>
        internal class Descriptor : RandomNumberInitialConditions.Descriptor
        {
            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="Descriptor" /> class.
            /// </summary>
            /// <param name="usage">The usage of initial condition parameters used to generate <see cref="IRandomNumberStream"/> instances..</param>
            /// <param name="description">A description of [optional/additional] initial conditions.</param>
            internal Descriptor(RandomNumberInitialConditions.ParametersUsage usage = RandomNumberInitialConditions.ParametersUsage.Optional, string description = "")
                : base(usage, description)
            {
            }
            #endregion

            #region public properties

            /// <summary>Gets the number of retrievable initial conditions for the specified Random Number Generator.
            /// </summary>
            /// <value>The number of retrievable initial conditions for a specified Random Number Generator.
            /// </value>
            public override int Count
            {
                get { return 0; }
            }

            /// <summary>Gets a specified retrievable <see cref="RandomNumberInitialConditions" /> object.
            /// </summary>
            /// <value>The retrievable initial conditions in its <see cref="RandomNumberInitialConditions" /> representation.
            /// </value>
            /// <param name="index">The null-based index.</param>
            public override RandomNumberInitialConditions this[int index]
            {
                get { throw new IndexOutOfRangeException(); }
            }

            /// <summary>Gets a specified retrievable <see cref="RandomNumberInitialConditions"/> object.
            /// </summary>
            /// <value>The retrievable initial conditions in its <see cref="RandomNumberInitialConditions"/> representation.
            /// </value>
            /// <param name="name">The name of the initial condition parameters.</param>
            public override RandomNumberInitialConditions this[string name]
            {
                get { throw new ArgumentException(); }
            }

            /// <summary>Gets the collection of retrievable initial conditions in its <see cref="RandomNumberInitialConditions"/> representation.
            /// </summary>
            /// <value>The collection of retrievable initial conditions in its <see cref="RandomNumberInitialConditions"/> representation.
            /// </value>
            public override IEnumerable<RandomNumberInitialConditions> Values
            {
                get { return Enumerable.Empty<RandomNumberInitialConditions>(); }
            }
            #endregion

            #region public methods

            /// <summary>Creates specified initial conditions for the Random Number Generator.
            /// </summary>
            /// <typeparam name="T">The type of initial conditions.</typeparam>
            /// <param name="initialConditions">The initial conditions.</param>
            /// <returns>The initial conditions for the Random Number Generator in its <see cref="RandomNumberInitialConditions"/> representation.</returns>
            /// <exception cref="InvalidOperationException">Thrown, if the arguments for the initial conditions are not suitable for the specified Random Number Generator; perhaps individual initial conditions are not allowed at all.</exception>
            public override RandomNumberInitialConditions Create<T>(T initialConditions)
            {
                if (initialConditions is int)
                {
                    int seed = (int)(object)initialConditions;
                    return new SingleSeed(seed);
                }
                else if (initialConditions is int[])
                {
                    int[] seedArray = (int[])(object)initialConditions;
                    int n = seedArray.Length;
                    return new MultiSeed(n, seedArray);
                }
                throw new InvalidOperationException();
            }

            /// <summary>Creates specified initial conditions for the Random Number Generator.
            /// </summary>
            /// <typeparam name="T1">The type of first argument of the initial conditions.</typeparam>
            /// <typeparam name="T2">The type of second argument of the initial conditions.</typeparam>
            /// <param name="initialCondition1">The first initial condition.</param>
            /// <param name="initialCondition2">The second initial condition.</param>
            /// <returns>The initial conditions for the Random Number Generator in its <see cref="RandomNumberInitialConditions"/> representation.</returns>
            /// <exception cref="InvalidOperationException">Thrown, if the arguments for the initial conditions are not suitable for the specified Random Number Generator; perhaps individual initial conditions are not allowed at all.</exception>
            public override RandomNumberInitialConditions Create<T1, T2>(T1 initialCondition1, T2 initialCondition2)
            {
                if ((initialCondition1 is int) && (initialCondition2 is int[]))
                {
                    int n = (int)(object)initialCondition1;

                    int[] seedArray = (int[])(object)initialCondition2;
                    return new MultiSeed(n, seedArray);
                }
                throw new InvalidOperationException();
            }

            /// <summary>Creates specified initial conditions for the Random Number Generator.
            /// </summary>
            /// <typeparam name="T1">The type of first argument of the initial conditions.</typeparam>
            /// <typeparam name="T2">The type of second argument of the initial conditions.</typeparam>
            /// <typeparam name="T3">The type of third argument of the initial conditions.</typeparam>
            /// <param name="initialCondition1">The first initial condition.</param>
            /// <param name="initialCondition2">The second initial condition.</param>
            /// <param name="initialCondition3">The third initial condition.</param>
            /// <returns>The initial conditions for the Random Number Generator in its <see cref="RandomNumberInitialConditions"/> representation.</returns>
            /// <exception cref="InvalidOperationException">Thrown, if the arguments for the initial conditions are not suitable for the specified Random Number Generator; perhaps individual initial conditions are not allowed at all.</exception>
            public override RandomNumberInitialConditions Create<T1, T2, T3>(T1 initialCondition1, T2 initialCondition2, T3 initialCondition3)
            {
                throw new InvalidOperationException();
            }
            #endregion
        }
    }
}