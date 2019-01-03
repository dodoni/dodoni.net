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
using System.Collections.Generic;

using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.ProbabilityTheory.MonteCarloEngine
{
    /// <summary>Serves as interface for a specified Random Number Generator Library, for example Intel's Math Kernel Library (MKL), AMD's Core Math Library (ACML) etc.
    /// </summary>
    public interface IRandomNumberLibrary
    {
        /// <summary>Gets the collection of Pseudo-Random Number Generators provided by the library.
        /// </summary>
        /// <value>The collection of Pseudo-Random Number Generator.</value>
        IEnumerable<IPseudoRandomNumberGenerator> PseudoRandomNumberGenerators
        {
            get;
        }

        /// <summary>Gets the collection of Quasi-Random Number Generators provided by the library.
        /// </summary>
        /// <value>The collection of Quasi-Random Number Generator.</value>
        IEnumerable<IQuasiRandomNumberGenerator> QuasiRandomNumberGenerator
        {
            get;
        }

        /// <summary>Gets the name of the Random Number Generator Library.
        /// </summary>
        /// <value>The name of the Random Number Generator Library.</value>
        IdentifierString Name { get; }

        /// <summary>Gets a description of the Random Number Generator Library.
        /// </summary>
        /// <value>The description of the Random Number Generator Library.</value>
        string Description { get; }
    }
}