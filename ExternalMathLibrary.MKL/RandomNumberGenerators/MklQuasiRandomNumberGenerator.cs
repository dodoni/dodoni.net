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

using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.ProbabilityTheory.MonteCarloEngine
{
    /// <summary>Represents Quasi-Random Number Generator of Intel's MKL Library.
    /// </summary>
    public class MklQuasiRandomNumberGenerator : IQuasiRandomNumberGenerator
    {
        #region nested enumerations

        /// <summary>Represents the Generator ID's (magic numbers) of Quasi-Random Number Generators.
        /// </summary>
        internal enum MagicNumber
        {
            /// <summary>A 32-bit Gray code-based generator producing low-discrepancy sequences.
            /// </summary>
            Sobol = 6291456,

            /// <summary>A 32-bit Gray code-based generator producing low-discrepancy sequences.
            /// </summary>
            Niederreiter = 7340032
        }
        #endregion

        #region public static (readonly) members

        /// <summary>A 32-bit Gray code-based generator producing low-discrepancy sequences.
        /// </summary>
        public static readonly IQuasiRandomNumberGenerator Sobol;

        /// <summary>A 32-bit Gray code-based generator producing low-discrepancy sequences.
        /// </summary>
        public static readonly IQuasiRandomNumberGenerator Niederreiter;
        #endregion

        #region private members

        /// <summary>The name of the Quasi-Random Number Generator.
        /// </summary>
        private IdentifierString m_GeneratorName;

        /// <summary>The internal Generator ID.
        /// </summary>
        private int m_GeneratorID;

        /// <summary>The Random Number Library related to the Quasi-Random Number Generator.
        /// </summary>
        private IRandomNumberLibrary m_Library;

        /// <summary>The splitting approach supported by the current Random Number Generator.
        /// </summary>
        private RandomNumberSequence.SplittingApproach m_SplittingApproach;

        /// <summary>The description of initial conditions.
        /// </summary>
        private RandomNumberInitialConditions.Descriptor m_InitialConditions;

        /// <summary>A description of the Quasi-Random Number Generator.
        /// </summary>
        private string m_Description;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="MklQuasiRandomNumberGenerator" /> class.
        /// </summary>
        /// <param name="generatorName">The name of the Random Number Generator.</param>
        /// <param name="generatorID">The (internal) Generator ID.</param>
        /// <param name="initialConditions">The description of initial conditions.</param>
        /// <param name="description">The description of the Quasi-Random Number Generator.</param>
        /// <param name="splittingApproach">The splitting approach supported by the Random Number Generator.</param>
        /// <param name="randomNumberLibrary">The random number library.</param>
        internal MklQuasiRandomNumberGenerator(string generatorName, int generatorID, RandomNumberInitialConditions.Descriptor initialConditions, string description, RandomNumberSequence.SplittingApproach splittingApproach = RandomNumberSequence.SplittingApproach.None, IRandomNumberLibrary randomNumberLibrary = null)
        {
            m_Library = randomNumberLibrary;
            m_GeneratorName = IdentifierString.Create(generatorName);
            m_GeneratorID = generatorID;
            m_InitialConditions = initialConditions;
            m_Description = description;
            m_SplittingApproach = splittingApproach;
        }
        #endregion

        #region static constructor

        /// <summary>Initializes the <see cref="MklQuasiRandomNumberGenerator" /> class.
        /// </summary>
        static MklQuasiRandomNumberGenerator()
        {
            Sobol = new MklQuasiRandomNumberGenerator("Sobol_mkl", (int)MklQuasiRandomNumberGenerator.MagicNumber.Sobol, new MklRandomNumberInitialConditions.Descriptor(), MklResources.Sobol, RandomNumberSequence.SplittingApproach.LeapFrog | RandomNumberSequence.SplittingApproach.SkipAhead);
            Niederreiter = new MklQuasiRandomNumberGenerator("Niederreiter_mkl", (int)MklQuasiRandomNumberGenerator.MagicNumber.Niederreiter, new MklRandomNumberInitialConditions.Descriptor(), MklResources.Niederreiter, RandomNumberSequence.SplittingApproach.LeapFrog | RandomNumberSequence.SplittingApproach.SkipAhead);
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the long name of the Quasi-Random Number Generator.
        /// </summary>
        /// <value>The (perhaps) language dependent long name of the Quasi-Random Number Generator.
        /// </value>
        public IdentifierString LongName
        {
            get { return m_GeneratorName; }
        }

        /// <summary>Gets the name of the Quasi-Random Number Generator.
        /// </summary>
        /// <value>The language independent name of the Quasi-Random Number Generator.</value>
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

        #endregion

        #region public methods

        #region IQuasiRandomNumberGenerator Members

        /// <summary>Creates a new random number stream.
        /// </summary>
        /// <param name="dimension">The dimension.</param>
        /// <returns>The random number stream in its <see cref="IRandomNumberStream"/> representation.
        /// </returns>
        /// <exception cref="InvalidOperationException">Thrown, if additional parameters are required.</exception>
        public IRandomNumberStream Create(long dimension)
        {
            if (dimension > UInt32.MaxValue)
            {
                throw new ArgumentException("dimension");
            }
            IntPtr streamHandle = IntPtr.Zero;
            int errorCode = MklRandomNumberLibrary.vslNewStream(ref streamHandle, m_GeneratorID, (uint)dimension);

            if (errorCode != 0) // execution is not successful
            {
                throw new InvalidOperationException("MKL: Return value " + errorCode + " in vslNewStream.");
            }
            return new MklRandomNumberStream(streamHandle, this, (uint)dimension);
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
        public IRandomNumberStream Create(int n, int[] initialConditions)
        {
            IntPtr streamHandle = IntPtr.Zero;
            int errorCode = MklRandomNumberLibrary.vslNewStreamEx(ref streamHandle, m_GeneratorID, n, initialConditions);
            if (errorCode != 0) // execution is not successful
            {
                throw new InvalidOperationException("MKL: Return value " + errorCode + " in vslNewStreamEx.");
            }
            int dimension = initialConditions[0];

            return new MklRandomNumberStream(streamHandle, this, dimension);
        }

        /// <summary>Creates a new random number stream.
        /// </summary>
        /// <param name="n">The number of initial conditions contained in <paramref name="initialConditions"/>.</param>
        /// <param name="initialConditions">Initial conditions. <see cref="IRandomNumberGenerator.InitialConditions"/> contains a description of the usage of additional parameters.</param>
        /// <returns>The random number stream in its <see cref="IRandomNumberStream"/> representation.</returns>
        public IRandomNumberStream Create(int n, uint[] initialConditions)
        {
            IntPtr streamHandle = IntPtr.Zero;
            int errorCode = MklRandomNumberLibrary.vslNewStreamEx(ref streamHandle, m_GeneratorID, n, initialConditions);
            if (errorCode != 0) // execution is not successful
            {
                throw new InvalidOperationException("MKL: Return value " + errorCode + " in vslNewStreamEx.");
            }
            var dimension = initialConditions[0];

            return new MklRandomNumberStream(streamHandle, this, dimension);
        }
        #endregion
    }
}