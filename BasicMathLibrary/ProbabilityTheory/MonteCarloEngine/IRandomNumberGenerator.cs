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
    /// <summary>Serves as common interface for Random Number Generators.
    /// </summary>
    public interface IRandomNumberGenerator : IIdentifierNameable, IAnnotatable
    {
        /// <summary>Gets the Random Number Library related to the current Random Number Generator; <c>null</c> if the Random Number Library is not available.
        /// </summary>
        /// <value>The Random Number Library; perhaps <c>null</c>.</value>
        IRandomNumberLibrary Library
        {
            get;
        }

        /// <summary>Gets a value indicating whether splitting of a random number sequence into non-overlapping subsequences is supported.
        /// </summary>
        /// <value>The splitting approach.</value>
        RandomNumberSequence.SplittingApproach SplittingApproach
        {
            get;
        }

        /// <summary>The usage of [additional] initial conditions used to generate <see cref="IRandomNumberStream"/> objects.
        /// </summary>
        /// <value>The usage of initial condition parameters.</value>
        /// <remarks>This property describes whether initial conditions for creation of <see cref="IRandomNumberStream"/> objects are required 
        /// and how these have to be applied. Moreover it may contains a set of standard parameters. 
        /// <example>For example this property may contains a collection of primitive polynomials and initial direction numbers in 
        /// the case of a Sobol Random Number Generator.</example></remarks>
        RandomNumberInitialConditions.Descriptor InitialConditions
        {
            get;
        }
    }
}