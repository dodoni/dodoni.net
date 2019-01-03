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
using System.Runtime.InteropServices;

using Dodoni.BasicComponents;
using Dodoni.MathLibrary.Basics.LowLevel.Native;

namespace Dodoni.MathLibrary.ProbabilityTheory.MonteCarloEngine
{
    /// <summary>Represents Pseudo-Random Number Generator of AMD's ACML Library.
    /// </summary>
    public class AcmlPseudoRandomNumberGenerator : IPseudoRandomNumberGenerator
    {
        #region private function import

        [DllImport(AcmlNativeWrapper.dllName, EntryPoint = "DRANDINITIALIZE", ExactSpelling = true, CallingConvention = AcmlNativeWrapper.callingConvention)]
        private static extern void _acml_DRandomInitialize(ref int generatorID, ref int subGeneratorID, int[] seed, ref int seedLength, int[] state, ref int stateLength, ref int info);

        [DllImport(AcmlNativeWrapper.dllName, EntryPoint = "DRANDINITIALIZE", ExactSpelling = true, CallingConvention = AcmlNativeWrapper.callingConvention)]
        private static extern void _acml_DRandomInitialize(ref int generatorID, ref int subGeneratorID, ref int seed, ref int seedLength, int[] state, ref int stateLength, ref int info);
        #endregion

        #region public static (readonly) members

        /// <summary>The NAG basic generator is a linear congruential generator.
        /// </summary>
        public static readonly AcmlPseudoRandomNumberGenerator NAG;

        /// <summary>The Wichmann-Hill base generator uses a combination of four linear congruential generators.
        /// </summary>
        public static readonly AcmlPseudoRandomNumberGenerator WichmannHill;

        /// <summary>The Mersenne Twister is a twisted generalized feedback shift register generator
        /// </summary>
        public static readonly AcmlPseudoRandomNumberGenerator MT19937;

        /// <summary>The L'Ecuyer's combined recursive generator combines two multiple recursive generators.
        /// </summary>
        public static readonly AcmlPseudoRandomNumberGenerator Ecuyer;
        #endregion

        #region private members

        /// <summary>The Random Number library related to the Pseudo-Random Number Generator.
        /// </summary>
        private readonly IRandomNumberLibrary m_Library;

        /// <summary>The name of the Random Number Generator.
        /// </summary>
        private readonly IdentifierString m_Name;

        /// <summary>The internal Generator ID.
        /// </summary>
        private readonly int m_GeneratorID;

        /// <summary>The splitting approach supported by the Random Number Generator.
        /// </summary>
        private RandomNumberSequence.SplittingApproach m_SplittingApproach;

        /// <summary>The sub-generator ID's (relevant for Wichmann-Hill Random Number Generator only).
        /// </summary>
        private IEnumerable<int> m_SubGeneratorIDs;

        /// <summary>The minimum length of the state vector (for ACML internal use).
        /// </summary>
        private readonly int m_MinimumStateVectorLength;

        /// <summary>A short description of the Pseudo-Random Number Generator.
        /// </summary>
        private string m_Description;

        /// <summary>A specification of initial conditions.
        /// </summary>
        private RandomNumberInitialConditions.Descriptor m_InitialConditions;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="AcmlPseudoRandomNumberGenerator" /> class.
        /// </summary>
        /// <param name="generatorID">The (ACML internal) Generator ID.</param>
        /// <param name="minimumStateVectorLength">The (ACML internal) minimum length of the STATE vector.</param>
        /// <param name="generatorName">The name of the Pseudo-Random Number Generator.</param>
        /// <param name="initialContitionDescription">The initial contition description.</param>
        /// <param name="description">The description of the Pseudo-Random Number Generator.</param>
        /// <param name="splittingApproach">The splitting approach w.r.t. Random Number Generator.</param>
        /// <param name="numberOfSubGenerators">The number of sub-generator's (relevant for Wichmann-Hill only).</param>
        /// <param name="randomNumberLibrary">The related Random Number Library.</param>
        internal AcmlPseudoRandomNumberGenerator(int generatorID, int minimumStateVectorLength, string generatorName, RandomNumberInitialConditions.Descriptor initialContitionDescription, string description, RandomNumberSequence.SplittingApproach splittingApproach = RandomNumberSequence.SplittingApproach.None, int numberOfSubGenerators = 1, IRandomNumberLibrary randomNumberLibrary = null)
        {
            m_GeneratorID = generatorID;
            m_Description = description;
            m_Library = randomNumberLibrary;
            m_MinimumStateVectorLength = minimumStateVectorLength;
            m_Name = IdentifierString.Create(generatorName);
            m_InitialConditions = InitialConditions;
            m_SplittingApproach = splittingApproach;
            m_SubGeneratorIDs = Enumerable.Range(0, numberOfSubGenerators);
        }
        #endregion

        #region static constructor

        /// <summary>Initializes the <see cref="AcmlPseudoRandomNumberGenerator" /> class.
        /// </summary>
        static AcmlPseudoRandomNumberGenerator()
        {
            NAG = new AcmlPseudoRandomNumberGenerator(1, 16, "NAG_acml", new AcmlRandomNumberInitialConditions.Descriptor(), AcmlResources.NAG, RandomNumberSequence.SplittingApproach.LeapFrog | RandomNumberSequence.SplittingApproach.SkipAhead);
            WichmannHill = new AcmlPseudoRandomNumberGenerator(2, 20, "WichmannHill_acml", new AcmlRandomNumberInitialConditions.Descriptor(), AcmlResources.WichmannHill, RandomNumberSequence.SplittingApproach.LeapFrog | RandomNumberSequence.SplittingApproach.SkipAhead, numberOfSubGenerators: 273);
            MT19937 = new AcmlPseudoRandomNumberGenerator(3, 633, "MersenneTwister_acml", new AcmlRandomNumberInitialConditions.Descriptor(), AcmlResources.MersenneTwister);
            Ecuyer = new AcmlPseudoRandomNumberGenerator(4, 61, "Ecuyer_acml", new AcmlRandomNumberInitialConditions.Descriptor(), AcmlResources.Ecuyer, RandomNumberSequence.SplittingApproach.LeapFrog | RandomNumberSequence.SplittingApproach.SkipAhead);
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the long name of the Pseudo-Random Number Generator.
        /// </summary>
        /// <value>The (perhaps) language dependent long name of the Pseudo-Random Number Generator.
        /// </value>
        public IdentifierString LongName
        {
            get { return m_Name; }
        }

        /// <summary>Gets the name of the Pseudo-Random Number Generator.
        /// </summary>
        /// <value>The language independent name of the Pseudo-Random Number Generator.</value>
        public IdentifierString Name
        {
            get { return m_Name; }
        }
        #endregion

        #region IAnnotatable Members

        /// <summary>Gets the annotation of the Pseudo-Random Number Generator.
        /// </summary>
        /// <value>The annotation of the Pseudo-Random Number Generator.</value>
        public string Annotation
        {
            get { return m_Description; }
        }

        /// <summary>Gets a value indicating whether the annotation is read-only.
        /// </summary>
        /// <value><c>true</c> if the annotation of this instance is readonly; otherwise, <c>false</c>.
        /// </value>
        public bool HasReadOnlyAnnotation
        {
            get { return true; }
        }
        #endregion

        #region IPseudoRandomNumberGenerator Members

        /// <summary>Gets the maximal value of the seed supported by the Random Number Generator.
        /// </summary>
        /// <value>The maximal value of the seed supported by the Random Number Generator.
        /// </value>
        public long MaxSeed
        {
            get { return Int32.MaxValue; }
        }

        /// <summary>Gets the collection of sub-generator ID's.
        /// </summary>
        /// <value>The collection of sub-generator ID's.</value>
        /// <remarks>This property represents the collection of possible arguments for <see cref="IPseudoRandomNumberGenerator.Create(long, int)" />.</remarks>
        public IEnumerable<int> SubGeneratorIDs
        {
            get { return m_SubGeneratorIDs; }
        }
        #endregion

        #region IRandomNumberGenerator Members

        /// <summary>Gets the Random Number Library related to the current Random Number Generator; <c>null</c> if the Random Number Library is not available.
        /// </summary>
        /// <value>The Random Number Library; perhaps <c>null</c>.
        /// </value>
        public IRandomNumberLibrary Library
        {
            get { return m_Library; }
        }

        /// <summary>Gets a value indicating whether splitting of a random number sequence into non-overlapping subsequences is supported.
        /// </summary>
        /// <value>The splitting approach.</value>
        public RandomNumberSequence.SplittingApproach SplittingApproach
        {
            get { return m_SplittingApproach; }
        }

        /// <summary>The usage of [additional] initial conditions used to generate <see cref="IRandomNumberStream" /> objects.
        /// </summary>
        /// <value>The usage of initial condition parameters.
        /// </value>
        /// <remarks>
        /// This property describes whether initial conditions for creation of <see cref="IRandomNumberStream" /> objects are required
        /// and how these have to be applied. Moreover it may contains a set of standard parameters.
        /// <example>For example this property may contains a collection of primitive polynomials and initial direction numbers in
        /// the case of a Sobol Random Number Generator.</example>
        /// </remarks>
        public RandomNumberInitialConditions.Descriptor InitialConditions
        {
            get { return m_InitialConditions; }
        }
        #endregion

        #endregion

        #region public methods

        #region IPseudoRandomNumberGenerator Members

        /// <summary>Creates a new random number stream.
        /// </summary>
        /// <param name="seed">The seed.</param>
        /// <returns>The random number stream in its <see cref="IRandomNumberStream"/> representation.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if additional parameters are required.</exception>
        IRandomNumberStream IPseudoRandomNumberGenerator.Create(long seed)
        {
            return Create(seed, 0);
        }

        /// <summary>Creates a new random number stream.
        /// </summary>
        /// <param name="seed">The seed.</param>
        /// <returns>The random number stream in its <see cref="IRandomNumberStream"/> representation.</returns>
        /// <param name="subGeneratorID">A sub-generator ID if a set of similar Random Number Generator is represented by the current instance.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="seed"/> or <paramref name="subGeneratorID"/> is invalid for the (set of) Random Number Generator(s) represented by the current instance.</exception>
        /// <exception cref="InvalidOperationException">Thrown, if additional parameters are required.</exception>
        IRandomNumberStream IPseudoRandomNumberGenerator.Create(long seed, int subGeneratorID)
        {
            return Create(seed, subGeneratorID);
        }

        /// <summary>Creates a new random number stream.
        /// </summary>
        /// <param name="initialConditions">Initial conditions for the Random Number Generator; <see cref="IRandomNumberGenerator.InitialConditions" /> serves as factory for initial conditions with respect to the Random Number Generator represented by the current instance.</param>
        /// <returns>The random number stream in its <see cref="IRandomNumberStream" /> representation.</returns>
        IRandomNumberStream IPseudoRandomNumberGenerator.Create(RandomNumberInitialConditions initialConditions)
        {
            return Create(initialConditions);
        }

        /// <summary>Creates a new random number stream.
        /// </summary>
        /// <param name="initialConditions">Initial conditions for the Random Number Generator; <see cref="IRandomNumberGenerator.InitialConditions"/> serves as factory for initial conditions with respect to the Random Number Generator represented by the current instance.</param>
        /// <param name="subGeneratorID">A sub-generator ID if a set of similar Random Number Generator is represented by the current instance.</param>
        /// <returns>The random number stream in its <see cref="IRandomNumberStream"/> representation.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="initialConditions"/> or <paramref name="subGeneratorID"/> is not suitable for the Random Number Generator represented by the current instance.</exception>
        IRandomNumberStream IPseudoRandomNumberGenerator.Create(RandomNumberInitialConditions initialConditions, int subGeneratorID)
        {
            return Create(initialConditions, subGeneratorID);
        }
        #endregion

        #region IAnnotatable Members

        /// <summary>Sets the annotation of the current instance.
        /// </summary>
        /// <param name="annotation">The annotation.</param>
        /// <returns>A value indicating whether the <see cref="P:Dodoni.BasicComponents.IAnnotatable.Annotation"/> has been changed.
        /// </returns>
        public bool TrySetAnnotation(string annotation)
        {
            return false;
        }
        #endregion

        /// <summary>Creates a new random number stream.
        /// </summary>
        /// <param name="seed">The seed.</param>
        /// <returns>The random number stream in its <see cref="AcmlRandomNumberStream"/> representation.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if additional parameters are required.</exception>
        public AcmlRandomNumberStream Create(long seed)
        {
            return Create(seed, 0);
        }

        /// <summary>Creates a new random number stream.
        /// </summary>
        /// <param name="seed">The seed.</param>
        /// <returns>The random number stream in its <see cref="AcmlRandomNumberStream"/> representation.</returns>
        /// <param name="subGeneratorID">A sub-generator ID if a set of similar Random Number Generator is represented by the current instance.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="seed"/> or <paramref name="subGeneratorID"/> is invalid for the (set of) Random Number Generator(s) represented by the current instance.</exception>
        /// <exception cref="InvalidOperationException">Thrown, if additional parameters are required.</exception>
        public AcmlRandomNumberStream Create(long seed, int subGeneratorID)
        {
            if (seed > MaxSeed)
            {
                throw new ArgumentException(nameof(seed));
            }
            int intSeed = (int)seed;

            if ((subGeneratorID < 0) || (subGeneratorID > m_SubGeneratorIDs.Last()))
            {
                throw new ArgumentException(nameof(subGeneratorID));
            }
            subGeneratorID += 1; // API of ACML is one-based

            int stateLength = m_MinimumStateVectorLength;
            int[] state = new int[stateLength];

            int seedLength, errorCode;
            seedLength = errorCode = 1;
            int generatorID = m_GeneratorID;

            _acml_DRandomInitialize(ref generatorID, ref subGeneratorID, ref intSeed, ref seedLength, state, ref stateLength, ref errorCode);
            if (errorCode != 0) // execution is not successful
            {
                throw new InvalidOperationException(String.Format("ACML: Return value {0} in DRANDINITIALIZE.", errorCode));
            }
            return new AcmlRandomNumberStream(this, state, subGeneratorID);
        }

        /// <summary>Creates a new random number stream.
        /// </summary>
        /// <param name="initialConditions">Initial conditions for the Random Number Generator; <see cref="IRandomNumberGenerator.InitialConditions" /> serves as factory for initial conditions with respect to the Random Number Generator represented by the current instance.</param>
        /// <returns>The random number stream in its <see cref="AcmlRandomNumberStream" /> representation.</returns>
        public AcmlRandomNumberStream Create(RandomNumberInitialConditions initialConditions)
        {
            if (initialConditions == null)
            {
                throw new ArgumentNullException(nameof(initialConditions));
            }
            if (initialConditions is AcmlRandomNumberInitialConditions.SingleSeed)
            {
                var acmlInitialConditions = initialConditions as AcmlRandomNumberInitialConditions.SingleSeed;
                return Create(acmlInitialConditions.Seed);
            }
            else if (initialConditions is AcmlRandomNumberInitialConditions.MultiSeed)
            {
                var acmlInitialConditions = initialConditions as AcmlRandomNumberInitialConditions.MultiSeed;
                return Create(acmlInitialConditions.n, acmlInitialConditions.Seed);
            }
            throw new ArgumentException(nameof(initialConditions));
        }

        /// <summary>Creates a new random number stream.
        /// </summary>
        /// <param name="initialConditions">Initial conditions for the Random Number Generator; <see cref="IRandomNumberGenerator.InitialConditions"/> serves as factory for initial conditions with respect to the Random Number Generator represented by the current instance.</param>
        /// <param name="subGeneratorID">A sub-generator ID if a set of similar Random Number Generator is represented by the current instance.</param>
        /// <returns>The random number stream in its <see cref="AcmlRandomNumberStream"/> representation.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="initialConditions"/> or <paramref name="subGeneratorID"/> is not suitable for the Random Number Generator represented by the current instance.</exception>
        public AcmlRandomNumberStream Create(RandomNumberInitialConditions initialConditions, int subGeneratorID)
        {
            if (initialConditions == null)
            {
                throw new ArgumentNullException(nameof(initialConditions));
            }
            if (initialConditions is AcmlRandomNumberInitialConditions.SingleSeed)
            {
                var acmlInitialConditions = initialConditions as AcmlRandomNumberInitialConditions.SingleSeed;
                return Create(acmlInitialConditions.Seed, subGeneratorID);
            }
            else if (initialConditions is AcmlRandomNumberInitialConditions.MultiSeed)
            {
                var acmlInitialConditions = initialConditions as AcmlRandomNumberInitialConditions.MultiSeed;
                return Create(acmlInitialConditions.n, acmlInitialConditions.Seed, subGeneratorID);
            }
            throw new ArgumentException(nameof(initialConditions));
        }

        /// <summary>Creates a new random number stream.
        /// </summary>
        /// <param name="n">The number of initial conditions contained in <paramref name="initialConditions" />.</param>
        /// <param name="initialConditions">Initial conditions. <see cref="IRandomNumberGenerator.InitialConditions" /> contains a description of the usage of additional parameters.</param>
        /// <returns>The random number stream in its <see cref="AcmlRandomNumberStream"/> representation.</returns>
        public AcmlRandomNumberStream Create(int n, int[] initialConditions)
        {
            int subID = 1;
            int stateLength = m_MinimumStateVectorLength;
            int[] state = new int[stateLength];

            int errorCode = 0;
            int id = m_GeneratorID;

            _acml_DRandomInitialize(ref id, ref subID, initialConditions, ref n, state, ref stateLength, ref errorCode);
            if (errorCode != 0) // execution is not successful
            {
                throw new InvalidOperationException("ACML: Return value " + errorCode + " in DRANDINITIALIZE.");
            }
            return new AcmlRandomNumberStream(this, state);
        }

        /// <summary>Creates a new random number stream.
        /// </summary>
        /// <param name="n">The number of initial conditions contained in <paramref name="initialConditions" />.</param>
        /// <param name="initialConditions">Initial conditions. <see cref="IRandomNumberGenerator.InitialConditions" /> contains a description of the usage of additional parameters.</param>
        /// <param name="subGeneratorID">A sub-generator ID if a set of similar Random Number Generator is represented by the current instance.</param>
        /// <returns>The random number stream in its <see cref="AcmlRandomNumberStream" /> representation.</returns>
        public AcmlRandomNumberStream Create(int n, int[] initialConditions, int subGeneratorID)
        {
            if ((subGeneratorID < 0) || (subGeneratorID > m_SubGeneratorIDs.Last()))
            {
                throw new ArgumentException(nameof(subGeneratorID));
            }
            subGeneratorID += 1; // API of ACML is one-based

            int stateLength = m_MinimumStateVectorLength;
            int[] state = new int[stateLength];

            int errorCode = 0;
            int id = m_GeneratorID;

            _acml_DRandomInitialize(ref id, ref subGeneratorID, initialConditions, ref n, state, ref stateLength, ref errorCode);
            if (errorCode != 0) // execution is not successful
            {
                throw new InvalidOperationException("ACML: Return value " + errorCode + " in DRANDINITIALIZE.");
            }
            return new AcmlRandomNumberStream(this, state, subGeneratorID);
        }
        #endregion
    }
}