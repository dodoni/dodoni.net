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
using System.Runtime.InteropServices;

using Dodoni.BasicComponents;
using Dodoni.MathLibrary.Basics.LowLevel.Native;

namespace Dodoni.MathLibrary.ProbabilityTheory.MonteCarloEngine
{
    /// <summary>Represents the .net wrapper for Random Number Generators with respect to Intel's MKL Library (Copyright 1994-2013, Intel Corporation. All rights reserved, http://www.intel.com).
    /// </summary>    
    [RandomNumberLibrary]
    public class MklRandomNumberLibrary : IRandomNumberLibrary
    {
        #region internal function import

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vslNewStream", ExactSpelling = true, CallingConvention = MklNativeWrapper.callingConvention)]
        internal static extern int vslNewStream(ref IntPtr stream, int brng, uint seed);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vslNewStreamEx", ExactSpelling = true, CallingConvention = MklNativeWrapper.callingConvention)]
        internal static extern int vslNewStreamEx(ref IntPtr stream, int brng, int n, uint[] additionalParams);

        [DllImport(MklNativeWrapper.dllName, EntryPoint = "vslNewStreamEx", ExactSpelling = true, CallingConvention = MklNativeWrapper.callingConvention)]
        internal static extern int vslNewStreamEx(ref IntPtr stream, int brng, int n, int[] additionalParams);
        #endregion

        #region private/public const (readonly) members

        /// <summary>The name of the Random Number Library.
        /// </summary>
        private static readonly IdentifierString sm_Name = new IdentifierString("MKL");

        /// <summary>A 31-bit multiplicative congruential generator.
        /// </summary>
        public readonly IPseudoRandomNumberGenerator MCG31;

        /// <summary>A generalized feedback shift register generator.
        /// </summary>
        public readonly IPseudoRandomNumberGenerator R250;

        /// <summary>A combined multiple recursive generator with two components of order 3.
        /// </summary>
        public readonly IPseudoRandomNumberGenerator MRG32K3A;

        /// <summary>A 59-bit multiplicative congruential generator.
        /// </summary>
        public readonly IPseudoRandomNumberGenerator MCG59;

        /// <summary>A set of 273 Wichmann-Hill combined multiplicative congruential generators.
        /// </summary>
        public readonly IPseudoRandomNumberGenerator WichmannHill;

        /// <summary>A Mersenne Twister pseudorandom number generator.
        /// </summary>
        public readonly IPseudoRandomNumberGenerator MT19937;

        /// <summary>A set of 1024 Mersenne Twister pseudorandom number generators.
        /// </summary>
        public readonly IPseudoRandomNumberGenerator MT2203;

        /// <summary>A 32-bit Gray code-based generator producing low-discrepancy sequences.
        /// </summary>
        public readonly IQuasiRandomNumberGenerator Sobol;

        /// <summary>A 32-bit Gray code-based generator producing low-discrepancy sequences.
        /// </summary>
        public readonly IQuasiRandomNumberGenerator Niederreiter;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="MklRandomNumberLibrary" /> class.
        /// </summary>
        public MklRandomNumberLibrary()
        {
            MCG31 = new MklPseudoRandomNumberGenerator("MCG31_mkl", (int)MklPseudoRandomNumberGenerator.MagicNumber.MCG31, new MklRandomNumberInitialConditions.Descriptor(), MklResources.MCG31, RandomNumberSequence.SplittingApproach.LeapFrog | RandomNumberSequence.SplittingApproach.SkipAhead, 1, this);
            R250 = new MklPseudoRandomNumberGenerator("R250_mkl", (int)MklPseudoRandomNumberGenerator.MagicNumber.R250, new MklRandomNumberInitialConditions.Descriptor(), MklResources.R250, RandomNumberSequence.SplittingApproach.None, 1, this);
            MRG32K3A = new MklPseudoRandomNumberGenerator("MRG32K3A_mkl", (int)MklPseudoRandomNumberGenerator.MagicNumber.MRG32K3A, new MklRandomNumberInitialConditions.Descriptor(), MklResources.MRG32K3A, RandomNumberSequence.SplittingApproach.SkipAhead, 1, this);
            MCG59 = new MklPseudoRandomNumberGenerator("MCG59_mkl", (int)MklPseudoRandomNumberGenerator.MagicNumber.MCG59, new MklRandomNumberInitialConditions.Descriptor(), MklResources.MCG59, RandomNumberSequence.SplittingApproach.LeapFrog | RandomNumberSequence.SplittingApproach.SkipAhead, 1, this);
            WichmannHill = new MklPseudoRandomNumberGenerator("WichmannHill_mkl", (int)MklPseudoRandomNumberGenerator.MagicNumber.WH, new MklRandomNumberInitialConditions.Descriptor(), MklResources.WichmannHill, RandomNumberSequence.SplittingApproach.LeapFrog | RandomNumberSequence.SplittingApproach.SkipAhead, 273, this);
            MT19937 = new MklPseudoRandomNumberGenerator("MT19937_mkl", (int)MklPseudoRandomNumberGenerator.MagicNumber.MT19937, new MklRandomNumberInitialConditions.Descriptor(), MklResources.MT19937, RandomNumberSequence.SplittingApproach.None, 1, this);
            MT2203 = new MklPseudoRandomNumberGenerator("MT2203_mkl", (int)MklPseudoRandomNumberGenerator.MagicNumber.MT2203, new MklRandomNumberInitialConditions.Descriptor(), MklResources.MT2203, RandomNumberSequence.SplittingApproach.None, 6024, this);

            Sobol = new MklQuasiRandomNumberGenerator("Sobol_mkl", (int)MklQuasiRandomNumberGenerator.MagicNumber.Sobol, new MklRandomNumberInitialConditions.Descriptor(), MklResources.Sobol, RandomNumberSequence.SplittingApproach.LeapFrog | RandomNumberSequence.SplittingApproach.SkipAhead, this);
            Niederreiter = new MklQuasiRandomNumberGenerator("Niederreiter_mkl", (int)MklQuasiRandomNumberGenerator.MagicNumber.Niederreiter, new MklRandomNumberInitialConditions.Descriptor(), MklResources.Niederreiter, RandomNumberSequence.SplittingApproach.LeapFrog | RandomNumberSequence.SplittingApproach.SkipAhead, this);
        }
        #endregion

        #region public properties

        #region IRandomNumberLibrary Members

        /// <summary>Gets the collection of Pseudo-Random Number Generators provided by the library.
        /// </summary>
        /// <value>The collection of Pseudo-Random Number Generator.
        /// </value>
        public IEnumerable<IPseudoRandomNumberGenerator> PseudoRandomNumberGenerators
        {
            get
            {

                yield return MCG31;
                yield return R250;
                yield return MRG32K3A;
                yield return MCG59;
                yield return WichmannHill;
                yield return MT2203;
                yield return MT19937;
            }
        }

        /// <summary>Gets the collection of Quasi-Random Number Generators provided by the library.
        /// </summary>
        /// <value>The collection of Quasi-Random Number Generator.
        /// </value>
        public IEnumerable<IQuasiRandomNumberGenerator> QuasiRandomNumberGenerator
        {
            get
            {
                yield return Sobol;
                yield return Niederreiter;
            }
        }

        /// <summary>Gets the name of the Random Number Generator Library.
        /// </summary>
        /// <value>The name of the Random Number Generator Library.</value>
        public IdentifierString Name
        {
            get { return sm_Name; }
        }

        /// <summary>Gets a description of the Random Number Generator Library.
        /// </summary>
        /// <value>The description of the Random Number Generator Library.</value>
        public string Description
        {
            get { return String.Format(MklResources.LibraryDescription, MklNativeWrapper.dllName); }
        }
        #endregion

        #endregion

        #region public methods

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return sm_Name.String;
        }
        #endregion
    }
}