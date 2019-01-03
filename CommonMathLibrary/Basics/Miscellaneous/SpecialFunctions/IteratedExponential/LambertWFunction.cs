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
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

namespace Dodoni.MathLibrary.Miscellaneous
{
    /// <summary>Represents the implementation of Lambert's W function, i.e. it holds W(z) * exp(W(z)) = z. There are two real branches and countable many complex branches of the W function.
    /// </summary>
    /// <remarks>The implementation is base on "On the Lambert W Function", R. M. Corless, G. H. Gonnet, D. E. G. Hare, D. J. Jeffrey, D. E. Knuth, 1996 and
    /// "The SIAM 100-Digit Challenge - A study in High-Accuracy Numerical Computing", F. Bornemann, D. Laurie, S. Wagon, J. Waldvogel, 2004, §1.5.</remarks>
    internal class LambertWFunction
    {
        #region private members

        /// <summary>The maximal number of iterations for real and complex calculation.
        /// </summary>
        private const int MaxIteration = 30;

        /// <summary>The maximal number of Taylor coefficients to take into account for the real calculation.
        /// </summary>
        private const int MaxTaylor = 10;

        /// <summary>The real polynomials p_n for calculating the n-th derivative of W(x) with p[0] = 1.
        /// </summary>
        private IRealPolynomial[] p;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="LambertWFunction" /> class.
        /// </summary>
        internal LambertWFunction()
        {
            p = GetPolynomialCoefficient(MaxTaylor).Select(coefficients => Polynomial.Real.Create(coefficients)).ToArray();
        }
        #endregion

        #region public methods

        /// <summary>Gets the value W_k(z) of the k-th branch of Lambert's W function.
        /// </summary>
        /// <param name="z">The argument.</param>
        /// <param name="branch">A value that indicates the branch of Lambert's W function; 0 is the Principal Branch; see reference for further information.</param>
        /// <returns>The value W_k(z) of the k-th branch of Lambert's W function.</returns>
        public Complex GetValue(Complex z, int branch = 0)
        {
            if (z == 0)
            {
                return 0.0;
            }

            /* calculate accurate initial guess for Halley's method, see §5 in "On the Lambert W Function", R. M. Corless, G. H. Gonnet, D. E. G. Hare, D. J. Jeffrey, D. E. Knuth, i.e.
             * - use in general w_0 = log (z) - log log (z), i.e. asymptotic exapnsion at 0
             * - if branch = 0, -1 or 1, one check whether z is near the branch point -1/e and series expansion around the Branch point (4.22), i.e. for example use w_0 = -1 + p -1/3 *p^2 + 11/72*p^3, where p = +/- sqrt( 2 * e * z + 2 ),
             * 
             * For branch = 0 and z near 0.0 the article advice to use a Pade approximation. If one declares '0.0' as near by the branch point -1/e and applies the same
             * approach as describes above, it seems to give an acceptable initial guess as well.
             */

            Complex w = 0;

            switch (branch)
            {
                case -1:
                case 0:
                case 1:
                    var p = Complex.Sqrt(2 * MathConsts.E * z + 2.0) * (1 - 2 * Math.Abs(branch));

                    w = -1.0 + p * (1.0 - 1.0 / 3.0 * p + 11.0 / 72.0 * p * p);  // with respect to (4.22)

                    /* if *not* near branch point -1/e use the same initial value as for all other points */
                    if ((Complex.Abs(z + MathConsts.OneOverE) > 1.45 - 1.1 * Math.Abs(branch)) || (branch * z.Imaginary > 0.0) || ((z.Imaginary != 0.0) && (branch == 1)))
                    {
                        goto default;
                    }
                    break;

                default:
                    var logZ = Complex.Log(z) + 2 * Math.PI * branch * Complex.ImaginaryOne;  // use the corresponding branch of log
                    if (logZ == 0.0)
                    {
                        w = 0.0;
                    }
                    else
                    {
                        w = logZ - Complex.Log(logZ);
                    }
                    break;
            }

            /* apply Halley's method as described in the reference */
            for (int k = 1; k <= MaxIteration; k++)
            {
                var expOfW = Complex.Exp(w);
                var wTimesExpOfWminusZ = w * expOfW - z;

                if (wTimesExpOfWminusZ == Complex.Zero) // otherwise the expression in Halley's method returns NaN
                {
                    break;
                }
                var diff = wTimesExpOfWminusZ / (expOfW * (w + 1) - (w + 2) * wTimesExpOfWminusZ / (2 * w + 2));

                if ((Math.Abs(diff.Real) < MachineConsts.ExtremeTinyEpsilon * (2 + Math.Abs(w.Real))) && (Math.Abs(diff.Imaginary) < MachineConsts.ExtremeTinyEpsilon * (2 + Math.Abs(w.Imaginary))))
                {
                    break;
                }
                w = w - diff;
            }
            return w;
        }

        /// <summary>Gets the value W_0(x) of the Principal Branch of Lambert's W function.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <returns>The value W_0(x) of the Principal Branch of Lambert's W function.</returns>
        /// <remarks>This implementation is based on "The SIAM 100-Digit Challenge - A study in High-Accuracy Numerical Computing", F. Bornemann, D. Laurie, S. Wagon, J. Waldvogel, 2004, §1.5
        /// Tests have shown that the implementation is correct for arguments ]-1/e,0] as well.</remarks>
        public double GetValue(double x)
        {
            if (x < -MathConsts.OneOverE)
            {
                throw new ArgumentException();
            }
            else if (x == -MathConsts.OneOverE)  // without tolerance!
            {
                return -1.0;
            }

            /* This implementation is based on "The SIAM 100-Digit Challenge - A study in High-Accuracy Numerical Computing", F. Bornemann, D. Laurie, S. Wagon, J. Waldvogel, 2004, §1.5
             * Tests shown that the implementation is correct for arguments ]-1/e,0] as well.
             * */

            var wk = Math.Log(1 + x);  // initial value
            var expOfwk = Math.Exp(wk);

            int k = 1;
            while ((k <= MaxIteration) && (Math.Abs(x - wk * expOfwk) > MachineConsts.ExtremeTinyEpsilon * (2.0 + Math.Abs(x))))
            {
                var xk = wk * expOfwk;

                var power = (x - xk); // = (x - x_k)^n
                var q = expOfwk * (1 + wk); // = exp( w * n) * (1+ w)^{2n -1}

                var wkPlus1 = wk + power / q; // = W(x_k) + 1/1! * (x - x_k) * W'(x_k) + 1/2! * (x - x_k)^2 * W''(x_k) +...

                var faculty = 1; // = n!
                for (int n = 2; n < ((k < MaxIteration) ? 5 : MaxTaylor); n++)  // As described in the reference a higher number of coefficients of the Taylor series will used at most in the last iteration step
                {
                    power *= (x - xk);
                    faculty *= n;
                    q *= expOfwk * (1 + wk) * (1 + wk);

                    wkPlus1 += power / faculty * p[n - 1].GetValue(wk) / q;
                }

                wk = wkPlus1;
                expOfwk = Math.Exp(wk);
                k++;
            }
            return wk;
        }
        #endregion

        #region private methods

        /// <summary>Gets the coefficients of the polynomial p_n that describes the n-th derivative of W, see for example formula (1.6) in  "The SIAM 100-Digit Challenge - A study in High-Accuracy Numerical Computing", F. Bornemann, D. Laurie, S. Wagon, J. Waldvogel, 2004.
        /// </summary>
        /// <param name="N">The number of polynomials, i.e. W^(n) for n = 1,..., N are taken calculated.</param>
        /// <returns>For each polynomial p_n the coefficients that describes the n-th derivative of W.</returns>
        private IEnumerable<double[]> GetPolynomialCoefficient(int N)
        {
            var a = new[] { 1.0 };
            yield return a;

            for (int n = 1; n < N; n++)
            {
                var b = new double[n + 1];

                b[0] = (1 - 3 * n) * a[0];
                b[n] = -n * a[n - 1];
                if (n > 1)
                {
                    b[0] += a[1];
                    b[n - 1] = -2 * n * a[n - 1] - n * a[n - 2];

                    for (int k = 1; k < n - 1; k++)
                    {
                        b[k] = (k + 1 - 3 * n) * a[k] - n * a[k - 1] + (1 + k) * a[k + 1];
                    }
                }
                a = b;
                yield return b;
            }
        }
        #endregion
    }
}