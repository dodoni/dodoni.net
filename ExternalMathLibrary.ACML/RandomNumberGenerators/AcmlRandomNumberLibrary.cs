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
using Dodoni.MathLibrary.Basics.LowLevel.Native;

namespace Dodoni.MathLibrary.ProbabilityTheory.MonteCarloEngine
{
    /// <summary>Represents the .net wrapper for Random Number Generators of AMD's ACML library.
    /// </summary>    
    [RandomNumberLibrary]
    public class AcmlRandomNumberLibrary : IRandomNumberLibrary
    {
        #region private members

        /// <summary>The name of the library.
        /// </summary>
        private static readonly IdentifierString sm_Name = new IdentifierString("ACML");
        #endregion

        #region public (readonly) members

        /// <summary>The NAG basic generator is a linear congruential generator.
        /// </summary>
        public readonly AcmlPseudoRandomNumberGenerator NAG;

        /// <summary>The Wichmann-Hill base generator uses a combination of four linear congruential generators.
        /// </summary>
        public readonly AcmlPseudoRandomNumberGenerator WichmannHill;

        /// <summary>The Mersenne Twister is a twisted generalized feedback shift register generator
        /// </summary>
        public readonly AcmlPseudoRandomNumberGenerator MT19937;

        /// <summary>The L'Ecuyer's combined recursive generator combines two multiple recursive generators.
        /// </summary>
        public readonly AcmlPseudoRandomNumberGenerator Ecuyer;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="AcmlRandomNumberLibrary"/> class.
        /// </summary>
        public AcmlRandomNumberLibrary()
        {
            NAG = new AcmlPseudoRandomNumberGenerator(1, 16, "NAG_acml", new AcmlRandomNumberInitialConditions.Descriptor(), AcmlResources.NAG, RandomNumberSequence.SplittingApproach.LeapFrog | RandomNumberSequence.SplittingApproach.SkipAhead, 1, this);
            WichmannHill = new AcmlPseudoRandomNumberGenerator(2, 20, "WichmannHill_acml", new AcmlRandomNumberInitialConditions.Descriptor(), AcmlResources.WichmannHill, RandomNumberSequence.SplittingApproach.LeapFrog | RandomNumberSequence.SplittingApproach.SkipAhead, 273, this);
            MT19937 = new AcmlPseudoRandomNumberGenerator(3, 633, "MersenneTwister_acml", new AcmlRandomNumberInitialConditions.Descriptor(), AcmlResources.MersenneTwister, RandomNumberSequence.SplittingApproach.None, 1, this);
            Ecuyer = new AcmlPseudoRandomNumberGenerator(4, 61, "Ecuyer_acml", new AcmlRandomNumberInitialConditions.Descriptor(), AcmlResources.Ecuyer, RandomNumberSequence.SplittingApproach.LeapFrog | RandomNumberSequence.SplittingApproach.SkipAhead, 1, this);
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the long name of the Random Number Library.
        /// </summary>
        /// <value>The (perhaps) language dependent long name of the Random Number library.
        /// </value>
        public IdentifierString LongName
        {
            get { return sm_Name; }
        }

        /// <summary>Gets the name of the Random Number Library.
        /// </summary>
        /// <value>The language independent name of the Random Number library.</value>
        public IdentifierString Name
        {
            get { return sm_Name; }
        }
        #endregion

        #region IAnnotatable Members

        /// <summary>Gets the annotation of the current instance.
        /// </summary>
        /// <value>The annotation of the current instance.</value>
        public string Description
        {
            get { return String.Format(AcmlResources.LibraryDescription, AcmlNativeWrapper.dllName); }
        }
        #endregion

        #region IRandomNumberLibrary Members

        /// <summary>Gets the collection of Pseudo-Random Number Generators provided by the library.
        /// </summary>
        /// <value>The collection of Pseudo-Random Number Generator.</value>
        public IEnumerable<IPseudoRandomNumberGenerator> PseudoRandomNumberGenerators
        {
            get
            {
                yield return NAG;
                yield return WichmannHill;
                yield return MT19937;
                yield return Ecuyer;
            }
        }

        /// <summary>Gets the collection of Quasi-Random Number Generators provided by the library.
        /// </summary>
        /// <value>The collection of Quasi-Random Number Generator.</value>
        public IEnumerable<IQuasiRandomNumberGenerator> QuasiRandomNumberGenerator
        {
            get { return Enumerable.Empty<IQuasiRandomNumberGenerator>(); }
        }
        #endregion

        #endregion
    }
}