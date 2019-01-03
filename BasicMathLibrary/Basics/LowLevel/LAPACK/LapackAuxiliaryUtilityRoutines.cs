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
using System.Numerics;

namespace Dodoni.MathLibrary.Basics.LowLevel
{
    /// <summary>Provides auxiliary and utility routines of the Linear Algebra PACKage, see http://www.netlib.org/lapack/index.html.
    /// </summary>
    public partial class LapackAuxiliaryUtilityRoutines
    {
        #region public (readonly) members

        /// <summary>Provides auxiliary and utility routines related to matrices.
        /// </summary>
        public readonly IMatrix Matrix;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="LapackAuxiliaryUtilityRoutines" /> class.
        /// </summary>
        /// <param name="matrixImplementation">The (native wrapper) implementation of <see cref="IMatrix"/>.</param>
        public LapackAuxiliaryUtilityRoutines(IMatrix matrixImplementation)
        {
            Matrix = matrixImplementation;
        }
        #endregion

        #region public static methods

        /// <summary>Creates a new <see cref="LapackAuxiliaryUtilityRoutines"/> instance.
        /// </summary>
        /// <param name="matrixImplementation">The (native wrapper) implementation of <see cref="IMatrix"/>.</param>
        /// <returns>A new <see cref="LapackAuxiliaryUtilityRoutines"/> object.</returns>
        public static LapackAuxiliaryUtilityRoutines Create(IMatrix matrixImplementation)
        {
            return new LapackAuxiliaryUtilityRoutines(matrixImplementation);
        }
        #endregion
    }
}