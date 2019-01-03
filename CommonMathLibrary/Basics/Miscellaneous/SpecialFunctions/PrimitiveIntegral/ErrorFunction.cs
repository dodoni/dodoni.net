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
using System.Numerics;

using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.Miscellaneous
{
    /// <summary>Serves as abstract base class and as factory for classes that implements an algorithm for the calculation of the (complementary) error function erf(x); erfc(x) respectively.
    /// </summary>
    public abstract class ErrorFunction
    {
        #region public static (readonly) members

        /// <summary>Apply a coarse algorithm based on "Algorithms with Guaranteed Error Bounds for the Error Function and the Complementary Error Function" Walter Krämer and Frithjof Blomquist Preprint 2000/2 Wissenschaftliches Rechnen/Softwaretechnologie Bergische Universität GH Wuppertal.
        /// </summary>
        public static readonly ErrorFunction CoarseAlgorithm;

        /// <summary>Apply Cody's algorithm based on "Rational Chebyshev approximations for the error function", W. J. Cody, Mathematics of Computation 23; p.631-637.
        /// </summary>
        public static readonly ErrorFunction CodyAlgorithm;
        #endregion

        #region static constructor

        /// <summary>Initializes the <see cref="ErrorFunction" /> class.
        /// </summary>
        static ErrorFunction()
        {
            CoarseAlgorithm = new ErrorFunctionCoarseAlgorithm();
            CodyAlgorithm = new ErrorFunctionCodyAlgorithm();
        }
        #endregion

        #region protected constructors

        /// <summary>Initializes a new instance of the <see cref="ErrorFunction" /> class.
        /// </summary>
        protected ErrorFunction()
        {
        }
        #endregion

        #region public (abstract) methods

        /// <summary>Gets a specific value of the error function erf(x) = 2/\sqrt{PI} * \int_0^x e^{-t^2} dt.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <returns>The specified value of the error function erf(x) = 2/\sqrt{PI} * \int_0^x e^{-t^2} dt.</returns>
        public abstract double GetValue(double x);

        /// <summary>Gets a specific value of the complementary error function erfc(x) = 1- erf(x) = 2/\sqrt{PI} * \int_x^\infty e^{-t^2} dt.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <returns>The specified value of the complementary error function erfc(x) = 1- erf(x) = 2/\sqrt{PI} * \int_x^\infty e^{-t^2} dt.</returns>
        public abstract double GetCValue(double x);
        #endregion
    }
}