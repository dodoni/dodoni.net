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

namespace Dodoni.MathLibrary.Basics.LowLevel
{
    /// <summary>Provides methods for calculation of Gamma and related functions.
    /// </summary>
    public abstract class GammaSpecialFunctions
    {
        #region protected constructors

        /// <summary>Initializes a new instance of the <see cref="GammaSpecialFunctions" /> class.
        /// </summary>
        protected GammaSpecialFunctions()
        {
            Factorial = new Factorial();
            DoubleFactorial = new DoubleFactorial();
        }
        #endregion

        #region public properties

        /// <summary>Gets the factorial n! = n * (n-1) * ... * 2 * 1 with 0! = 1, i.e. the product of all positive integers less than or equal to n.
        /// </summary>
        /// <value>The factorial n! = n * (n-1) * ... * 2 * 1 with 0! = 1, i.e. the product of all positive integers less than or equal to n.</value>
        public Factorial Factorial
        {
            get;
            private set;
        }

        /// <summary>Gets the double factorial n!! = n!! = \prod_{j=}^k (n - 2 * j), where k = [n/2] -1.
        /// </summary>
        /// <value>The double factorial n!! = n!! = \prod_{j=}^k (n - 2 * j), where k = [n/2] -1.</value>
        public DoubleFactorial DoubleFactorial
        {
            get;
            private set;
        }
        #endregion

        #region public methods

        /// <summary>Returns the value of the gamma function at a specific point.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <returns>The value of the Gamma function at <paramref name="x"/>.</returns>
        public abstract double GetValue(double x);

        /// <summary>Returns the value of the gamma function at a specific point.
        /// </summary>
        /// <param name="z">The argument.</param>
        /// <returns>The value of the Gamma function at <paramref name="z"/>.</returns>
        public abstract Complex GetValue(Complex z);

        /// <summary>Returns the logarithm of \Gamma(x), where \Gamma is the gamma function.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <returns>The logarithm of the Gamma function at <paramref name="x"/>.</returns>
        public abstract double GetLogValue(double x);

        /// <summary>Returns the logarithm of \Gamma(x), where \Gamma is the gamma function.
        /// </summary>
        /// <param name="z">The argument.</param>
        /// <returns>The logarithm of the Gamma function at <paramref name="z"/>.</returns>
        public abstract Complex GetLogValue(Complex z);

        /// <summary>Gets the value of the normalized incompleted gamma function.
        /// </summary>
        /// <param name="a">The parameter a.</param>
        /// <param name="x">The parameter x.</param>
        /// <param name="normalizedLowerValue">The normalized lower value.</param>
        /// <param name="normalizedUpperValue">The normalized upper value.</param>
        public abstract void GetNormalizedIncompletedValue(double a, double x, out double normalizedLowerValue, out double normalizedUpperValue);
        #endregion
    }
}