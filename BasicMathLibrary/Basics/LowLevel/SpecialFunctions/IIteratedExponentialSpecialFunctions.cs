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
    /// <summary>Provides methods for calculation of Iterated Exponential and related functions.
    /// </summary>
    public interface IIteratedExponentialSpecialFunctions
    {
        /// <summary>Gets the value W_0(x) of the Principal Branch of Lambert's W function, i.e. W(x) * exp(W(x)) = x.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <returns>The value W_0(x) of the Principal Branch of Lambert's W function.</returns>
        double LambertW(double x);

        /// <summary>Gets the value W_k(z) of the k-th branch of Lambert's W function, i.e. W_k(z) * exp(W_k(z)) = z.
        /// </summary>
        /// <param name="z">The argument.</param>
        /// <param name="branch">A value that indicates the branch of Lambert's W function; 0 is the Principal Branch; see reference for further information.</param>
        /// <returns>The value W_k(z) of the k-th branch of Lambert's W function.</returns>
        Complex LambertW(Complex z, int branch = 0);
    }
}