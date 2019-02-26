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

namespace Dodoni.MathLibrary.Basics.LowLevel
{
    /// <summary>Provides special vector operations.
    /// </summary>
    public interface IVectorUnitSpecial
    {
        /// <summary>Computes the cumulative normal distribution function values of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector N(a[j]), j=0,...,<paramref name="n"/>-1, where N(x) =\int_{-\infty}^x  1/\sqrt(2*PI) * exp(-1/2 *t^2) dt.</param>
        void CdfNorm(int n, ReadOnlySpan<double> a, Span<double> y);

        /// <summary>Computes the cumulative normal distribution function values of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. N(a[j]), j=0,...,<paramref name="n"/>-1, where N(x) =\int_{-\infty}^x  1/\sqrt(2*PI) * exp(-1/2 *t^2) dt.</param>
        void CdfNorm(int n, Span<double> a);

        /// <summary>Computes the inverse cumulative normal distribution function values of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector N^{-1}(a[j]), j=0,...,<paramref name="n"/>-1, where N(x) =\int_{-\infty}^x  1/\sqrt(2*PI) * exp(-1/2 *t^2) dt.</param>
        void CdfNormInv(int n, ReadOnlySpan<double> a, Span<double> y);

        /// <summary>Computes the inverse cumulative normal distribution function values of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. N^{-1}(a[j]), j=0,...,<paramref name="n"/>-1, where N(x) =\int_{-\infty}^x  1/\sqrt(2*PI) * exp(-1/2 *t^2) dt.</param>
        void CdfNormInv(int n, Span<double> a);

        /// <summary>Computes the gamma function of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \Gamma(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        void Gamma(int n, ReadOnlySpan<double> a, Span<double> y);

        /// <summary>Computes the natural logarithm of the absolute value of gamma function of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \log|\Gamma(a[j])|, j=0,...,<paramref name="n"/>-1.</param>
        void GammaLn(int n, ReadOnlySpan<double> a, Span<double> y);
    }
}