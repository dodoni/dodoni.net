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
using System.Numerics;

using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.ProbabilityTheory.Distributions
{
    /// <summary>Serves as interface for a (one-dimensional) probability distribution.
    /// </summary>
    public interface IProbabilityDistribution : IIdentifierNameable, IInfoOutputQueriable
    {
        /// <summary>Gets the infimum of the support of the distribution.
        /// </summary>
        /// <value>The infimum of the support of the distribution.</value>
        double Infimum
        {
            get;
        }

        /// <summary>Gets the supremum of the support of the distribution.
        /// </summary>
        /// <value>The supremum of the support of the distribution.</value>
        double Supremum
        {
            get;
        }

        /// <summary>Gets the (raw, central etc.) moments of the probability distribution.
        /// </summary>
        /// <value>The (raw, central etc.) moments of the probability distribution.</value>
        ProbabilityDistributionMoments Moment
        {
            get;
        }

        /// <summary>Gets the median.
        /// </summary>
        /// <value>The median.</value>
        double Median
        {
            get;
        }

        /// <summary>Gets a specific value of the cumulative distribution function.
        /// </summary>
        /// <param name="x">The value where to evaluate.</param>
        /// <returns>The specified value of the cumulative distribution function.</returns>
        double GetCdfValue(double x);

        /// <summary>Gets specific values of the cumulative distribution function.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The arguments where to evaluate the cumulative distribution function.</param>
        /// <param name="y">The specified values of the cumulative distribution function (output).</param>
        void GetCdfValue(int n, double[] a, double[] y);

        /// <summary>Gets a specific value of the inverse of the cumulative distribution function, i.e. quantile function.
        /// </summary>
        /// <param name="y">The probability where to evaluate.</param>
        /// <returns>The specified value of the inverse of the cumulative distribution function.</returns>
        double GetInverseCdfValue(double y);

        /// <summary>Gets specific values of the inverse of the cumulative distribution function, i.e. quantile function.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The probabilities where to evaluate the inverse of the cumulative distribution function.</param>
        /// <param name="y">The specified values of the inverse of the cumulative distribution function (output).</param>
        void GetInverseCdfValue(int n, double[] a, double[] y);

        /// <summary>Gets a specific value of the probability density function.
        /// </summary>
        /// <param name="x">The point where to evaluate.</param>
        /// <returns>The specified value of the probability density function.</returns>
        double GetPdfValue(double x);

        /// <summary>Gets specific values of the probability density function.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The arguments where to evaluate the probability density function.</param>
        /// <param name="y">The specified values of the probability density function (output).</param>
        void GetPdfValue(int n, double[] a, double[] y);

        /// <summary>Gets a specific value of the characteristic function E[exp(itX)].
        /// </summary>
        /// <param name="t">The point where to evaluate the characteristic function.</param>
        /// <returns>The specified value of the characteristic function E[exp(itX)].</returns>
        Complex GetChfValue(double t);

        /// <summary>Gets specific values of the characteristic function E[exp(itX)].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The arguments where to evaluate the characteristic function.</param>
        /// <param name="y">The specified values of the characteristic function E[exp(itX)] (output).</param>
        void GetChfValue(int n, double[] a, Complex[] y);
    }
}