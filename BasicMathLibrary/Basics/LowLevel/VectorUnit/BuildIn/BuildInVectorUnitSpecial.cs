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

using Dodoni.MathLibrary.ProbabilityTheory.Distributions;

namespace Dodoni.MathLibrary.Basics.LowLevel.BuildIn
{
    /// <summary>Provides a managed .net implementation of vector operations with respect to special mathematical functions.
    /// </summary>    
    internal class BuildInVectorUnitSpecial : IVectorUnitSpecial
    {
        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="BuildInVectorUnitSpecial"/> class.
        /// </summary>
        internal BuildInVectorUnitSpecial()
        {
        }
        #endregion

        #region IVectorUnitSpecial Members

        /// <summary>Computes the cumulative normal distribution function values of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector N(a[j]), j=0,...,<paramref name="n"/>-1, where N(x) =\int_{-\infty}^x  1/\sqrt(2*PI) * exp(-1/2 *t^2) dt.</param>
        public void CdfNorm(int n, ReadOnlySpan<double> a, Span<double> y)
        {
            for (int j = 0; j < n; j++)
            {
                y[j] = StandardNormalDistribution.GetCdfValue(a[j]);
            }
        }

        /// <summary>Computes the cumulative normal distribution function values of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. N(a[j]), j=0,...,<paramref name="n" />-1, where N(x) =\int_{-\infty}^x  1/\sqrt(2*PI) * exp(-1/2 *t^2) dt.</param>
        public void CdfNorm(int n, Span<double> a)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] = StandardNormalDistribution.GetCdfValue(a[j]);
            }
        }

        /// <summary>Computes the inverse cumulative normal distribution function values of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector N^{-1}(a[j]), j=0,...,<paramref name="n"/>-1, where N(x) =\int_{-\infty}^x  1/\sqrt(2*PI) * exp(-1/2 *t^2) dt.</param>
        public void CdfNormInv(int n, ReadOnlySpan<double> a, Span<double> y)
        {
            for (int j = 0; j < n; j++)
            {
                y[j] = StandardNormalDistribution.GetInverseCdfValue(a[j]);
            }
        }

        /// <summary>Computes the inverse cumulative normal distribution function values of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. N^{-1}(a[j]), j=0,...,<paramref name="n" />-1, where N(x) =\int_{-\infty}^x  1/\sqrt(2*PI) * exp(-1/2 *t^2) dt.</param>
        public void CdfNormInv(int n, Span<double> a)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] = StandardNormalDistribution.GetInverseCdfValue(a[j]);
            }
        }

        /// <summary>Computes the gamma function of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \Gamma(a[j]), j=0,...,<paramref name="n" />-1.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void Gamma(int n, ReadOnlySpan<double> a, Span<double> y)
        {
            for (int j = 0; j < n; j++)
            {
                y[j] = SpecialFunction.Gamma.GetValue(a[j]);
            }
        }

        public void GammaLn(int n, ReadOnlySpan<double> a, Span<double> y)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}