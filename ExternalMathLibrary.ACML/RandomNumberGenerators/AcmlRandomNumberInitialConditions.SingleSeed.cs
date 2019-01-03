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

namespace Dodoni.MathLibrary.ProbabilityTheory.MonteCarloEngine
{
    /// <summary>Represents initial conditions of a specified Random Number Generator of the ACML Library, <seealso cref="IRandomNumberGenerator"/>.
    /// </summary>
    /// <remarks>This class is used to indicate whether additional initial conditions are required for the creation of <see cref="IRandomNumberStream"/> objects. 
    /// Moreover it contains a short description and perhaps a set of standard initial conditions etc.</remarks>
    internal partial class AcmlRandomNumberInitialConditions
    {
        /// <summary>Represents a seed in its <see cref="System.Int32"/> representation.
        /// </summary>
        internal class SingleSeed : RandomNumberInitialConditions
        {
            /// <summary>The seed of the Random Number Generator in its <see cref="System.Int32"/> representation.
            /// </summary>
            internal readonly int Seed;

            /// <summary>Initializes a new instance of the <see cref="SingleSeed" /> class.
            /// </summary>
            /// <param name="seed">The seed.</param>
            public SingleSeed(int seed)
            {
                Seed = seed;
            }
        }
    }
}