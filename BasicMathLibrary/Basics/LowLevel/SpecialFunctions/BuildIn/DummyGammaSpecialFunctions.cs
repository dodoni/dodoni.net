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

namespace Dodoni.MathLibrary.Basics.LowLevel
{
    /// <summary>This implementation of <see cref="GammaSpecialFunctions"/> does not implement any method correctly, a <see cref="NotImplementedException"/> will be thrown instead.
    /// </summary>
    internal class DummyGammaSpecialFunctions : GammaSpecialFunctions
    {
        /// <summary>Returns the value of the gamma function at a specific point.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <returns>The value of the Gamma function at <paramref name="x" />.</returns>
        public override double GetValue(double x)
        {
            throw new NotImplementedException();
        }

        /// <summary>Returns the value of the gamma function at a specific point.
        /// </summary>
        /// <param name="z">The argument.</param>
        /// <returns>The value of the Gamma function at <paramref name="z" />.</returns>
        public override Complex GetValue(Complex z)
        {
            throw new NotImplementedException();
        }

        /// <summary>Returns the logarithm of \Gamma(x), where \Gamma is the gamma function.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <returns>The logarithm of the Gamma function at <paramref name="x" />.</returns>
        public override double GetLogValue(double x)
        {
            throw new NotImplementedException();
        }

        /// <summary>Returns the logarithm of \Gamma(x), where \Gamma is the gamma function.
        /// </summary>
        /// <param name="z">The argument.</param>
        /// <returns>The logarithm of the Gamma function at <paramref name="z" />.</returns>
        public override Complex GetLogValue(Complex z)
        {
            throw new NotImplementedException();
        }

        /// <summary>Gets the value of the normalized incompleted gamma function.
        /// </summary>
        /// <param name="a">The parameter a.</param>
        /// <param name="x">The parameter x.</param>
        /// <param name="normalizedLowerValue">The normalized lower value.</param>
        /// <param name="normalizedUpperValue">The normalized upper value.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void GetNormalizedIncompletedValue(double a, double x, out double normalizedLowerValue, out double normalizedUpperValue)
        {
            throw new NotImplementedException();
        }
    }
}