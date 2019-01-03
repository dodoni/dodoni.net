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

using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.ProbabilityTheory.MonteCarloEngine
{
    /// <summary>Represents Pseudo-Random Number Generator of Intel's MKL Library.
    /// </summary>
    public class MklPseudoRandomNumberGenerator : IPseudoRandomNumberGenerator
    {
        #region nested enumerations

        /// <summary>Represents the Generator ID's of Pseudo-Random Number Generators.
        /// </summary>
        internal enum MagicNumber
        {
            /// <summary>A 31-bit multiplicative congruential generator.
            /// </summary>
            MCG31 = 1048576,

            /// <summary>A generalized feedback shift register generator.
            /// </summary>
            R250 = 2097152,

            /// <summary>A combined multiple recursive generator with two components of order 3.
            /// </summary>
            MRG32K3A = 3145728,

            /// <summary>A 59-bit multiplicative congruential generator.
            /// </summary>
            MCG59 = 4194304,

            /// <summary>A set of 273 Wichmann-Hill combined multiplicative congruential generators.
            /// </summary>
            WH = 5242880,

            /// <summary>A Mersenne Twister pseudorandom number generator.
            /// </summary>
            MT19937 = 8388608,

            /// <summary>A set of 1024 Mersenne Twister pseudorandom number generators.
            /// </summary>
            MT2203 = 9437184
        }
        #endregion

        #region public static (readonly) members

        /// <summary>A 31-bit multiplicative congruential generator.
        /// </summary>
        public static readonly IPseudoRandomNumberGenerator MCG31;

        /// <summary>A generalized feedback shift register generator.
        /// </summary>
        public static readonly IPseudoRandomNumberGenerator R250;

        /// <summary>A combined multiple recursive generator with two components of order 3.
        /// </summary>
        public static readonly IPseudoRandomNumberGenerator MRG32K3A;

        /// <summary>A 59-bit multiplicative congruential generator.
        /// </summary>
        public static readonly IPseudoRandomNumberGenerator MCG59;

        /// <summary>A set of 273 Wichmann-Hill combined multiplicative congruential generators.
        /// </summary>
        public static readonly IPseudoRandomNumberGenerator WichmannHill;

        /// <summary>A Mersenne Twister pseudorandom number generator.
        /// </summary>
        public static readonly IPseudoRandomNumberGenerator MT19937;

        /// <summary>A set of 1024 Mersenne Twister pseudorandom number generators.
        /// </summary>
        public static readonly IPseudoRandomNumberGenerator MT2203;
        #endregion

        #region private members

        /// <summary>The name of the Pseudo-Random Number Generator.
        /// </summary>
        private IdentifierString m_GeneratorName;

        /// <summary>The internal Generator ID.
        /// </summary>
        private int m_GeneratorID;

        /// <summary>The collection of sub-generator ID's (relevant for Wichmann-Hill and MT2203 only).
        /// </summary>
        private IEnumerable<int> m_SubGeneratorIDs;

        /// <summary>The splitting approach supported by the current Random Number Generator.
        /// </summary>
        private RandomNumberSequence.SplittingApproach m_SplittingApproach;

        /// <summary>The Random Number Library related to the Pseudo-Random Number Generator.
        /// </summary>
        private IRandomNumberLibrary m_Library;

        /// <summary>The description of initial conditions.
        /// </summary>
        private RandomNumberInitialConditions.Descriptor m_InitialConditions;

        /// <summary>A description of the Pseudo-Random Number Generator.
        /// </summary>
        private string m_Description;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="MklPseudoRandomNumberGenerator" /> class.
        /// </summary>
        /// <param name="generatorName">The name of the Random Number Generator.</param>
        /// <param name="generatorID">The (internal) Generator ID.</param>
        /// <param name="initialConditions">The description of initial conditions.</param>
        /// <param name="description">The description of the Pseudo-Random Number Generator.</param>
        /// <param name="splittingApproach">The splitting approach supported by the Random Number Generator.</param>
        /// <param name="numberOfSubGenerators">The number of sub-generator's (relevant for Wichmann-Hill and MT2203 only).</param>
        /// <param name="randomNumberLibrary">The random number library.</param>
        internal MklPseudoRandomNumberGenerator(string generatorName, int generatorID, RandomNumberInitialConditions.Descriptor initialConditions, string description, RandomNumberSequence.SplittingApproach splittingApproach = RandomNumberSequence.SplittingApproach.None, int numberOfSubGenerators = 1, IRandomNumberLibrary randomNumberLibrary = null)
        {
            m_Library = randomNumberLibrary;
            m_GeneratorName = IdentifierString.Create(generatorName);
            m_GeneratorID = generatorID;
            m_InitialConditions = initialConditions;
            m_Description = description;
            m_SplittingApproach = splittingApproach;
            m_SubGeneratorIDs = Enumerable.Range(0, numberOfSubGenerators);
        }
        #endregion

        #region static constructor

        /// <summary>Initializes the <see cref="MklPseudoRandomNumberGenerator" /> class.
        /// </summary>
        static MklPseudoRandomNumberGenerator()
        {
            MCG31 = new MklPseudoRandomNumberGenerator("MCG31_mkl", (int)MagicNumber.MCG31, new MklRandomNumberInitialConditions.Descriptor(), MklResources.MCG31, RandomNumberSequence.SplittingApproach.LeapFrog | RandomNumberSequence.SplittingApproach.SkipAhead);
            R250 = new MklPseudoRandomNumberGenerator("R250_mkl", (int)MagicNumber.R250, new MklRandomNumberInitialConditions.Descriptor(), MklResources.R250);
            MRG32K3A = new MklPseudoRandomNumberGenerator("MRG32K3A_mkl", (int)MagicNumber.MRG32K3A, new MklRandomNumberInitialConditions.Descriptor(), MklResources.MRG32K3A, RandomNumberSequence.SplittingApproach.SkipAhead);
            MCG59 = new MklPseudoRandomNumberGenerator("MCG59_mkl", (int)MagicNumber.MCG59, new MklRandomNumberInitialConditions.Descriptor(), MklResources.MCG59, RandomNumberSequence.SplittingApproach.LeapFrog | RandomNumberSequence.SplittingApproach.SkipAhead);
            MT2203 = new MklPseudoRandomNumberGenerator("MT2203_mkl", (int)MklPseudoRandomNumberGenerator.MagicNumber.MT2203, new MklRandomNumberInitialConditions.Descriptor(), MklResources.MT2203, numberOfSubGenerators: 6024);

            WichmannHill = new MklPseudoRandomNumberGenerator("WichmannHill_mkl", (int)MagicNumber.WH, new MklRandomNumberInitialConditions.Descriptor(), MklResources.WichmannHill, RandomNumberSequence.SplittingApproach.LeapFrog | RandomNumberSequence.SplittingApproach.SkipAhead, numberOfSubGenerators: 273);
            MT19937 = new MklPseudoRandomNumberGenerator("MT19937_mkl", (int)MagicNumber.MT19937, new MklRandomNumberInitialConditions.Descriptor(), MklResources.MT19937);
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
            get { return m_GeneratorName; }
        }

        /// <summary>Gets the name of the Pseudo-Random Number Generator.
        /// </summary>
        /// <value>The language independent name of the Pseudo-Random Number Generator.</value>
        public IdentifierString Name
        {
            get { return m_GeneratorName; }
        }
        #endregion

        #region IAnnotatable Members

        /// <summary>Gets the annotation of the current instance.
        /// </summary>
        /// <value>The annotation of the current instance.</value>
        public string Annotation
        {
            get { return m_Description; }
        }

        /// <summary>Gets a value indicating whether the annotation is read-only.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the annotation of this instance is readonly; otherwise, <c>false</c>.
        /// </value>
        bool IAnnotatable.HasReadOnlyAnnotation
        {
            get { return true; }
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

        /// <summary>The usage of [additional] initial conditions used to generate <see cref="IRandomNumberStream"/> objects.
        /// </summary>
        /// <value>The usage of initial condition parameters.</value>
        /// <remarks>This property describes whether initial conditions for creation of <see cref="IRandomNumberStream"/> objects are required
        /// and how these have to be applied. Moreover it may contains a set of standard parameters.
        /// <example>For example this property may contains a collection of primitive polynomials and initial direction numbers in
        /// the case of a Sobol Random Number Generator.</example></remarks>
        public RandomNumberInitialConditions.Descriptor InitialConditions
        {
            get { return m_InitialConditions; }
        }
        #endregion

        #region IPseudoRandomNumberGenerator Members

        /// <summary>Gets the maximal value of the seed supported by the Random Number Generator.
        /// </summary>
        /// <value>The maximal value of the seed supported by the Random Number Generator.
        /// </value>
        public long MaxSeed
        {
            get { return UInt32.MaxValue; }
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

        #endregion

        #region public methods

        #region IPseudoRandomNumberGenerator Members

        /// <summary>Creates a new random number stream.
        /// </summary>
        /// <param name="seed">The seed.</param>
        /// <returns>The random number stream in its <see cref="IRandomNumberStream"/> representation.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="seed"/> is invalid for the Random Number Generator represented by the current instance.</exception>
        /// <exception cref="InvalidOperationException">Thrown, if additional parameters are required.</exception>
        public IRandomNumberStream Create(long seed)
        {
            if ((seed < 0) || (seed > MaxSeed))
            {
                throw new ArgumentException("seed");
            }
            IntPtr streamHandle = IntPtr.Zero;
            int errorCode = MklRandomNumberLibrary.vslNewStream(ref streamHandle, m_GeneratorID, (uint)seed);

            if (errorCode != 0) // execution is not successful
            {
                throw new InvalidOperationException("MKL: Return value " + errorCode + " in vslNewStream.");
            }
            return new MklRandomNumberStream(streamHandle, this);
        }

        /// <summary>Creates a new random number stream.
        /// </summary>
        /// <param name="seed">The seed.</param>
        /// <param name="subGeneratorID">A sub-generator ID if a set of similar Random Number Generator is represented by the current instance.</param>
        /// <returns>The random number stream in its <see cref="IRandomNumberStream" /> representation.
        /// </returns>
        public IRandomNumberStream Create(long seed, int subGeneratorID)
        {
            if ((seed < 0) || (seed > MaxSeed))
            {
                throw new ArgumentException("seed");
            }
            if ((subGeneratorID < 0) || (subGeneratorID > m_SubGeneratorIDs.Last()))
            {
                throw new ArgumentException("subGeneratorID");
            }
            IntPtr streamHandle = IntPtr.Zero;
            int errorCode = MklRandomNumberLibrary.vslNewStream(ref streamHandle, m_GeneratorID + subGeneratorID, (uint)seed);

            if (errorCode != 0) // execution is not successful
            {
                throw new InvalidOperationException("MKL: Return value " + errorCode + " in vslNewStream.");
            }
            return new MklRandomNumberStream(streamHandle, this, subGeneratorID: subGeneratorID);
        }

        /// <summary>Creates a new random number stream.
        /// </summary>
        /// <param name="initialConditions">Initial conditions for the Random Number Generator; <see cref="IRandomNumberGenerator.InitialConditions"/> serves as factory for initial conditions with respect to the Random Number Generator represented by the current instance.</param>
        /// <returns>The random number stream in its <see cref="IRandomNumberStream"/> representation.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="initialConditions"/> is not suitable for the Random Number Generator represented by the current instance.</exception>
        public IRandomNumberStream Create(RandomNumberInitialConditions initialConditions)
        {
            if (initialConditions == null)
            {
                throw new ArgumentNullException("initialConditions");
            }
            if (initialConditions is MklRandomNumberInitialConditions.SingleSeed)
            {
                var mklInitialConditions = initialConditions as MklRandomNumberInitialConditions.SingleSeed;
                return Create(mklInitialConditions.Seed);
            }
            else if (initialConditions is MklRandomNumberInitialConditions.MultiSeed)
            {
                var mklInitialConditions = initialConditions as MklRandomNumberInitialConditions.MultiSeed;
                return Create(mklInitialConditions.n, mklInitialConditions.Seed);
            }
            throw new ArgumentException("initialConditions");
        }

        /// <summary>Creates a new random number stream.
        /// </summary>
        /// <param name="initialConditions">Initial conditions for the Random Number Generator; <see cref="IRandomNumberGenerator.InitialConditions"/> serves as factory for initial conditions with respect to the Random Number Generator represented by the current instance.</param>
        /// <param name="subGeneratorID">A sub-generator ID if a set of similar Random Number Generator is represented by the current instance.</param>
        /// <returns>The random number stream in its <see cref="IRandomNumberStream"/> representation.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="initialConditions"/> or <paramref name="subGeneratorID"/> is not suitable for the Random Number Generator represented by the current instance.</exception>
        public IRandomNumberStream Create(RandomNumberInitialConditions initialConditions, int subGeneratorID)
        {
            if (initialConditions == null)
            {
                throw new ArgumentNullException("initialConditions");
            }
            if (initialConditions is MklRandomNumberInitialConditions.SingleSeed)
            {
                var mklInitialConditions = initialConditions as MklRandomNumberInitialConditions.SingleSeed;
                return Create(mklInitialConditions.Seed, subGeneratorID);
            }
            else if (initialConditions is MklRandomNumberInitialConditions.MultiSeed)
            {
                var mklInitialConditions = initialConditions as MklRandomNumberInitialConditions.MultiSeed;
                return Create(mklInitialConditions.n, mklInitialConditions.Seed, subGeneratorID);
            }
            throw new ArgumentException("initialConditions");
        }
        #endregion

        #region IAnnotatable Members

        /// <summary>Sets the annotation of the current instance.
        /// </summary>
        /// <param name="annotation">The annotation.</param>
        /// <returns>A value indicating whether the <see cref="P:Dodoni.BasicComponents.IAnnotatable.Annotation"/> has been changed.
        /// </returns>
        bool IAnnotatable.TrySetAnnotation(string annotation)
        {
            return false;
        }
        #endregion

        /// <summary>Creates a new random number stream.
        /// </summary>
        /// <param name="n">The number of initial conditions contained in <paramref name="initialConditions"/>.</param>
        /// <param name="initialConditions">Initial conditions. <see cref="IRandomNumberGenerator.InitialConditions"/> contains a description of the usage of additional parameters.</param>
        /// <returns>The random number stream in its <see cref="IRandomNumberStream"/> representation.</returns>
        /// <exception cref="ArgumentException">Thrown if at least one parameter is not suitable for the Random Number Generator represented by the current instance.</exception>
        public IRandomNumberStream Create(int n, int[] initialConditions)
        {
            IntPtr streamHandle = IntPtr.Zero;
            int errorCode = MklRandomNumberLibrary.vslNewStreamEx(ref streamHandle, m_GeneratorID, n, initialConditions);
            if (errorCode != 0) // execution is not successful
            {
                throw new InvalidOperationException("MKL: Return value " + errorCode + " in vslNewStreamEx.");
            }
            return new MklRandomNumberStream(streamHandle, this);
        }

        /// <summary>Creates a new random number stream.
        /// </summary>
        /// <param name="n">The number of initial conditions contained in <paramref name="initialConditions"/>.</param>
        /// <param name="initialConditions">Initial conditions. <see cref="IRandomNumberGenerator.InitialConditions"/> contains a description of the usage of additional parameters.</param>
        /// <param name="subGeneratorID">A sub-generator ID if a set of similar Random Number Generator is represented by the current instance.</param>
        /// <returns>The random number stream in its <see cref="IRandomNumberStream"/> representation.</returns>
        /// <exception cref="ArgumentException">Thrown if at least one parameter is not suitable for the Random Number Generator represented by the current instance.</exception>
        public IRandomNumberStream Create(int n, int[] initialConditions, int subGeneratorID)
        {
            if ((subGeneratorID < 0) || (subGeneratorID > m_SubGeneratorIDs.Last()))
            {
                throw new ArgumentException("subGeneratorID");
            }
            IntPtr streamHandle = IntPtr.Zero;
            int errorCode = MklRandomNumberLibrary.vslNewStreamEx(ref streamHandle, m_GeneratorID + subGeneratorID, n, initialConditions);
            if (errorCode != 0) // execution is not successful
            {
                throw new InvalidOperationException("MKL: Return value " + errorCode + " in vslNewStreamEx.");
            }
            return new MklRandomNumberStream(streamHandle, this, subGeneratorID: subGeneratorID);
        }

        /// <summary>Creates a new random number stream.
        /// </summary>
        /// <param name="n">The number of initial conditions contained in <paramref name="initialConditions"/>.</param>
        /// <param name="initialConditions">Initial conditions. <see cref="IRandomNumberGenerator.InitialConditions"/> contains a description of the usage of additional parameters.</param>
        /// <returns>The random number stream in its <see cref="IRandomNumberStream"/> representation.</returns>
        /// <exception cref="ArgumentException">Thrown if at least one parameter is not suitable for the Random Number Generator represented by the current instance.</exception>
        public IRandomNumberStream Create(int n, uint[] initialConditions)
        {
            IntPtr streamHandle = IntPtr.Zero;
            int errorCode = MklRandomNumberLibrary.vslNewStreamEx(ref streamHandle, m_GeneratorID, n, initialConditions);
            if (errorCode != 0) // execution is not successful
            {
                throw new InvalidOperationException("MKL: Return value " + errorCode + " in vslNewStreamEx.");
            }
            return new MklRandomNumberStream(streamHandle, this);
        }

        /// <summary>Creates a new random number stream.
        /// </summary>
        /// <param name="n">The number of initial conditions contained in <paramref name="initialConditions"/>.</param>
        /// <param name="initialConditions">Initial conditions. <see cref="IRandomNumberGenerator.InitialConditions"/> contains a description of the usage of additional parameters.</param>
        /// <param name="subGeneratorID">A sub-generator ID if a set of similar Random Number Generator is represented by the current instance.</param>
        /// <returns>The random number stream in its <see cref="IRandomNumberStream"/> representation.</returns>
        /// <exception cref="ArgumentException">Thrown if at least one parameter is not suitable for the Random Number Generator represented by the current instance.</exception>
        public IRandomNumberStream Create(int n, uint[] initialConditions, int subGeneratorID)
        {
            if ((subGeneratorID < 0) || (subGeneratorID > m_SubGeneratorIDs.Last()))
            {
                throw new ArgumentException("subGeneratorID");
            }
            IntPtr streamHandle = IntPtr.Zero;
            int errorCode = MklRandomNumberLibrary.vslNewStreamEx(ref streamHandle, m_GeneratorID + subGeneratorID, n, initialConditions);
            if (errorCode != 0) // execution is not successful
            {
                throw new InvalidOperationException("MKL: Return value " + errorCode + " in vslNewStreamEx.");
            }
            return new MklRandomNumberStream(streamHandle, this, subGeneratorID: subGeneratorID);
        }
        #endregion
    }
}