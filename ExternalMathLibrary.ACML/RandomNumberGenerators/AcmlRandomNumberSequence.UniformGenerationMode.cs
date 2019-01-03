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
using Dodoni.MathLibrary.Basics.LowLevel.Native;

namespace Dodoni.MathLibrary.ProbabilityTheory.MonteCarloEngine
{
    /// <summary>Provides settings for Random Number Sequences with respect to AMD's ACML Library.
    /// </summary>
    public partial class AcmlRandomNumberSequence
    {
        /// <summary>Represents the generation mode provided for random numbers uniformly distributed over a specified interval.
        /// </summary>
        public class UniformGenerationMode : RandomNumberSequence.UniformGenerationMode
        {
            /// <summary>Standard approach for generation random numbers uniformly distributed over a specified interval.
            /// </summary>
            public static readonly RandomNumberSequence.UniformGenerationMode Standard = new UniformGenerationMode("StandardApproach", AcmlResources.Generation_StandardApproach);

            /// <summary>Initializes a new instance of the <see cref="UniformGenerationMode"/> class.
            /// </summary>
            /// <param name="name">The name of the generation method for uniform distributed random numbers.</param>
            /// <param name="annotation">The annotation (description) of the generation method.</param>
            private UniformGenerationMode(string name, string annotation)
                : base(IdentifierString.Create(name), annotation)
            {
            }

            /// <summary>Initializes the <see cref="UniformGenerationMode" /> class.
            /// </summary>
            static UniformGenerationMode()
            {
            }
        }
    }
}