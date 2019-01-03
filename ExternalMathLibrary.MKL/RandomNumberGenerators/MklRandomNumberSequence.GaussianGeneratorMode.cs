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
using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.ProbabilityTheory.MonteCarloEngine
{
    /// <summary>Provides settings for Random Number Sequences with respect to Intel's MKL Library.
    /// </summary>
    public partial class MklRandomNumberSequence
    {
        /// <summary>Represents the generation mode provided for Random Number generation following a Gaussian (Normal) distribution.
        /// </summary>
        public class GaussianGenerationMode : RandomNumberSequence.GaussianGenerationMode
        {
            /// <summary>Generates normally distributed random number x thru the pair of uniformly distributed numbers u1 and u2 according to the formula
            /// <para>x=sqrt(-ln(u1))*sin(2*Pi*u2).</para>
            /// </summary>
            public static readonly GaussianGenerationMode BoxMuller = new GaussianGenerationMode("BoxMuller", 0, MklResources.Gaussian_BoxMuller);

            /// <summary>Generates pair of normally distributed random numbers x1 and x2 thru the pair of uniformly dustributed numbers u1 and u2 according to the formula
            /// <para>x1=sqrt(-ln(u1))*sin(2*Pi*u2), x2=sqrt(-ln(u1))*cos(2*Pi*u2).</para>
            /// </summary>
            /// <remarks>Implementation correctly works with odd vector lengths.</remarks>
            public static readonly GaussianGenerationMode BoxMuller2 = new GaussianGenerationMode("BoxMuller2", 1, MklResources.Gaussian_BoxMuller2);

            /// <summary>The inverse cumulative distribution approach.
            /// </summary>
            public static readonly GaussianGenerationMode InverseCumulativeDistributionFunction = new GaussianGenerationMode("InverseCumulativeDistributionFunction", 2, MklResources.GaussianInverseCumulativeDistributionFunction);

            /// <summary>The magic number, i.e. ID needed by Intel's MKL library (VSL).
            /// </summary>
            internal readonly int MagicNumber;

            /// <summary>Initializes a new instance of the <see cref="GaussianGenerationMode" /> class.
            /// </summary>
            /// <param name="name">The name of the generation mode.</param>
            /// <param name="magicNumber">The magic number, i.e. ID needed by Intel's MKL library.</param>
            /// <param name="description">The description of the generation mode.</param>
            private GaussianGenerationMode(string name, int magicNumber, string description)
                : base(IdentifierString.Create(name), description)
            {
                MagicNumber = magicNumber;
            }
        }
    }
}