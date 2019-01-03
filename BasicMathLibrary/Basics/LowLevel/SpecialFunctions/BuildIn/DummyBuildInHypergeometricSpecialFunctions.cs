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
    /// <summary>This implementation of <see cref="IHypergeometricSpecialFunctions"/> does not implement any method correctly, a <see cref="NotImplementedException"/> will be thrown instead.
    /// </summary>
    internal class DummyBuildInHypergeometricSpecialFunctions : IHypergeometricSpecialFunctions
    {
        #region IHypergeometricSpecialFunction Members

        /// <summary>Gets the value of the confluent hypergeometric function of the first kind (Kummer's function) 1_F_1(a,b,x) = M(a,b,x) = \sum_{k=0}^\infty (a)_k / (b)_k * x^k /k!.
        /// </summary>
        /// <param name="a">The first argument.</param>
        /// <param name="b">The second argument.</param>
        /// <param name="x">The third argument.</param>
        /// <returns>The value of the confluent hypergeometric function 1_F_1(a,b,x) = M(a,b,x) = \sum_{k=0}^\infty (a)_k / (b)_k * x^k /k!.</returns>
        public double One_F_One(double a, double b, double x)
        {
            throw new NotImplementedException();
        }

        /// <summary>Gets the value of the confluent hypergeometric function of the first kind (Kummer's function) 1_F_1(a,b,z) = M(a,b,z) = \sum_{k=0}^\infty (a)_k / (b)_k * z^k /k!.
        /// </summary>
        /// <param name="a">The first argument.</param>
        /// <param name="b">The second argument.</param>
        /// <param name="z">The third argument.</param>
        /// <returns>The value of the confluent hypergeometric function 1_F_1(a,b,z) = M(a,b,z) = \sum_{k=0}^\infty (a)_k / (b)_k * z^k /k!.</returns>
        public Complex One_F_One(Complex a, Complex b, Complex z)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}