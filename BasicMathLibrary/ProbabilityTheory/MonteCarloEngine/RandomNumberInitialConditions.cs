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
using System.Collections.Generic;

using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.ProbabilityTheory.MonteCarloEngine
{
    /// <summary>Represents initial conditions of a specified Random Number Generator, <seealso cref="IRandomNumberGenerator"/>.
    /// </summary>
    /// <remarks>This class is used to indicate whether additional initial conditions are required for the creation of <see cref="IRandomNumberStream"/> objects. 
    /// Moreover it contains a short description and perhaps a set of standard initial conditions etc.</remarks>
    public abstract class RandomNumberInitialConditions : IIdentifierNameable
    {
        #region nested classes, enumerations etc.

        /// <summary>The usage of initial condition parameters used to generate <see cref="IRandomNumberStream"/> instances.
        /// </summary>
        [Flags]
        public enum ParametersUsage
        {
            /// <summary>Additional parameters are optional while creating a new <see cref="IRandomNumberStream"/> instance.
            /// </summary>
            Optional = 0,

            /// <summary>Additional parameters are required while creating a new <see cref="IRandomNumberStream"/> instance.
            /// </summary>
            Required
        }

        /// <summary>Serves as abstract descriptor and factory for initial conditions of a specified Random Number Generator.
        /// </summary>
        public abstract class Descriptor
        {
            #region public (readonly) members

            /// <summary>A value indicating whether initial conditions are required while creating a new <see cref="IRandomNumberStream"/> instance
            /// </summary>
            public readonly ParametersUsage Usage;

            /// <summary>A description of [optional/additional] initial conditions.
            /// </summary>
            public string Description;
            #endregion

            #region protected constructors

            /// <summary>Initializes a new instance of the <see cref="Descriptor" /> class.
            /// </summary>
            /// <param name="usage">The usage of initial condition parameters used to generate <see cref="IRandomNumberStream"/> instances..</param>
            /// <param name="description">A description of [optional/additional] initial conditions.</param>
            protected Descriptor(ParametersUsage usage, string description)
            {
                Usage = usage;
                Description = description;
            }
            #endregion

            #region public properties

            /// <summary>Gets the number of retrievable initial conditions for the specified Random Number Generator.
            /// </summary>
            /// <value>The number of retrievable initial conditions for a specified Random Number Generator.
            /// </value>
            public abstract int Count
            {
                get;
            }

            /// <summary>Gets a specified retrievable <see cref="RandomNumberInitialConditions"/> object.
            /// </summary>
            /// <value>The retrievable initial conditions in its <see cref="RandomNumberInitialConditions"/> representation.
            /// </value>
            /// <param name="index">The null-based index.</param>
            public abstract RandomNumberInitialConditions this[int index]
            {
                get;
            }

            /// <summary>Gets a specified retrievable <see cref="RandomNumberInitialConditions"/> object.
            /// </summary>
            /// <value>The retrievable initial conditions in its <see cref="RandomNumberInitialConditions"/> representation.
            /// </value>
            /// <param name="name">The name of the initial condition parameters.</param>
            public abstract RandomNumberInitialConditions this[string name]
            {
                get;
            }

            /// <summary>Gets the collection of retrievable initial conditions in its <see cref="RandomNumberInitialConditions"/> representation.
            /// </summary>
            /// <value>The collection of retrievable initial conditions in its <see cref="RandomNumberInitialConditions"/> representation.
            /// </value>
            public abstract IEnumerable<RandomNumberInitialConditions> Values
            {
                get;
            }
            #endregion

            #region public methods

            /// <summary>Creates specified initial conditions for the Random Number Generator.
            /// </summary>
            /// <typeparam name="T">The type of initial conditions.</typeparam>
            /// <param name="initialConditions">The initial conditions.</param>
            /// <returns>The initial conditions for the Random Number Generator in its <see cref="RandomNumberInitialConditions"/> representation.</returns>
            /// <exception cref="InvalidOperationException">Thrown, if the arguments for the initial conditions are not suitable for the specified Random Number Generator; perhaps individual initial conditions are not allowed at all.</exception>
            public abstract RandomNumberInitialConditions Create<T>(T initialConditions);

            /// <summary>Creates specified initial conditions for the Random Number Generator.
            /// </summary>
            /// <typeparam name="T1">The type of first argument of the initial conditions.</typeparam>
            /// <typeparam name="T2">The type of second argument of the initial conditions.</typeparam>
            /// <param name="initialCondition1">The first initial condition.</param>
            /// <param name="initialCondition2">The second initial condition.</param>
            /// <returns>The initial conditions for the Random Number Generator in its <see cref="RandomNumberInitialConditions"/> representation.</returns>
            /// <exception cref="InvalidOperationException">Thrown, if the arguments for the initial conditions are not suitable for the specified Random Number Generator; perhaps individual initial conditions are not allowed at all.</exception>
            public abstract RandomNumberInitialConditions Create<T1, T2>(T1 initialCondition1, T2 initialCondition2);

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
            public abstract RandomNumberInitialConditions Create<T1, T2, T3>(T1 initialCondition1, T2 initialCondition2, T3 initialCondition3);
            #endregion
        }
        #endregion

        #region private members

        /// <summary>The name of the initial conditions.
        /// </summary>
        private IdentifierString m_Name;
        #endregion

        #region protected constructors

        /// <summary>Initializes a new instance of the <see cref="RandomNumberInitialConditions" /> class.
        /// </summary>
        /// <param name="nameOfInitialConditions">The name of initial conditions.</param>
        protected RandomNumberInitialConditions(string nameOfInitialConditions = "<unknown>")
        {
            m_Name = new IdentifierString(nameOfInitialConditions);
        }
        #endregion

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the initial condition parameters.
        /// </summary>
        /// <value>The language independent name of the initial condition parameters.
        /// </value>
        public IdentifierString Name
        {
            get { return m_Name; }
        }

        /// <summary>Gets the long name of the initial condition parameters.
        /// </summary>
        /// <value>The (perhaps) language dependent long name of the initial condition parameters.
        /// </value>
        public IdentifierString LongName
        {
            get { return m_Name; }
        }
        #endregion
    }
}