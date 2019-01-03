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
    /// <summary>Represents the implementation of the confluent hypergeometric function of the first kind (Kummer's function) 1_F_1(a,b,x) = M(a,b,x) = \sum_{k=0}^\infty (a)_k / (b)_k * x^k /k! via a rational apprximation.
    /// </summary>
    ///<remarks>The implementation is based on 'Algorithms for the Computation of Mathematical Functions", Luke, Y.L (1977), Academic Press and 
    ///"Computing the confluent hypergeometric function, M(a,b,x)", K. E. Muller, Numer. Math. 90, p.179-196 (2001). The later reference has several typos.</remarks>
    internal class Hypergeometric1F1RationalApproximation
    {
        #region private members

        /// <summary>The maximal number of iteration, in general about 15 iterations are needed.
        /// </summary>
        private int m_MaxIteration = 75;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="Hypergeometric1F1RationalApproximation" /> class.
        /// </summary>
        internal Hypergeometric1F1RationalApproximation()
        {
        }
        #endregion

        #region public methods

        /// <summary>Gets the value of the confluent hypergeometric function of the first kind (Kummer's function) 1_F_1(a,b,x) = M(a,b,x) = \sum_{k=0}^\infty (a)_k / (b)_k * x^k /k!.
        /// </summary>
        /// <param name="a">The first argument.</param>
        /// <param name="b">The second argument.</param>
        /// <param name="x">The third argument.</param>
        /// <returns>The value of the confluent hypergeometric function 1_F_1(a,b,x) = M(a,b,x) = \sum_{k=0}^\infty (a)_k / (b)_k * x^k /k!.</returns>
        ///<remarks>The implementation is based on 'Algorithms for the Computation of Mathematical Functions" p.182-191, Luke, Y.L (1977), Academic Press and 
        ///"Computing the confluent hypergeometric function, M(a,b,x)", K. E. Muller, Numer. Math. 90, p.179-196 (2001). The later reference has several typos.</remarks>
        public double GetValue(double a, double b, double x)
        {
            if (a == b)  // without tolerance
            {
                return Math.Exp(x);
            }
            else if ((a == 0) || (x == 0)) // without tolerance
            {
                return 1.0;
            }         
            else if ((b == 0) || ((b < 0.0) && (b == Math.Round(b))))  // if b is a non-positive integer or 0, the function is not defined
            {
                throw new ArgumentException(String.Format(ExceptionMessages.ArgumentIsInvalidForObject, x, "1F1"),nameof(x));
            }

            /* We replace 'x' by '-x' and '\delta' by 'a' in "Computing the confluent hypergeometric function M(a,b,x)", K. E. Muller */

            var QnMinus3 = 1.0;
            var QnMinus2 = 1 - x * (a + 1) / (2.0 * b);
            var QnMinus1 = 1 - x * (a + 2) / (2 * (b + 1)) + x * x * (a + 1) * (a + 2) / (12 * b * (b + 1));

            var PnMinus3 = 1.0;
            var PnMinus2 = QnMinus2 + x * a / b;

            var PnMinus1 = QnMinus1 + a * x / b * (1 - x * (a + 2) / (2 * (b + 1))) + x * x * a * (a + 1) / (2 * b * (b + 1));  // the first '(b+1)' was '(b+2) in the paper of K. E. Muller

            double Qn, Pn;
            Qn = Pn = 0.0;

            var previousValue = 0.0;
            for (int n = 3; n < m_MaxIteration; n++)
            {
                var fi1 = (n - a - 2) / (2 * (2 * n - 3) * (n + b - 1));
                var fi2 = (n + a) * (n + a - 1) / (4 * (2 * n - 1) * (2 * n - 3) * (n + b - 2) * (n + b - 1));  // additional term in paper of K. E. Muller
                var fi3 = -(n + a - 2) * (n + a - 1) * (n - a - 2) / (8 * (2 * n - 3) * (2 * n - 3) * (2 * n - 5) * (n + b - 3) * (n + b - 2) * (n + b - 1)); // different sign in the 3. product in the paper of K. E. Muller
                var fi4 = -(n + a - 1) * (n - b - 1) / (2 * (2 * n - 3) * (n + b - 2) * (n + b - 1));

                Pn = (1 - fi1 * x) * PnMinus1 - (fi4 - fi2 * x) * x * PnMinus2 - fi3 * x * x * x * PnMinus3;  // an additional '-x' was missing in paper of K. e. Muller
                Qn = (1 - fi1 * x) * QnMinus1 - (fi4 - fi2 * x) * x * QnMinus2 - fi3 * x * x * x * QnMinus3;

                if ((n >= 4) && (Math.Abs(Pn / Qn - previousValue) < MachineConsts.ExtremeTinyEpsilon * (2.0 + Math.Abs(previousValue))))  // abort condition
                {
                    return Pn / Qn;
                }
                previousValue = Pn / Qn;

                PnMinus3 = PnMinus2;
                PnMinus2 = PnMinus1;
                PnMinus1 = Pn;

                QnMinus3 = QnMinus2;
                QnMinus2 = QnMinus1;
                QnMinus1 = Qn;
            }
            return Pn / Qn;
        }

        /// <summary>Gets the value of the confluent hypergeometric function of the first kind (Kummer's function) 1_F_1(a,b,z) = M(a,b,z) = \sum_{k=0}^\infty (a)_k / (b)_k * z^k /k!.
        /// </summary>
        /// <param name="a">The first argument.</param>
        /// <param name="b">The second argument.</param>
        /// <param name="z">The third argument.</param>
        /// <returns>The value of the confluent hypergeometric function 1_F_1(a,b,z) = M(a,b,z) = \sum_{k=0}^\infty (a)_k / (b)_k * z^k /k!.</returns>
        ///<remarks>The implementation is based on 'Algorithms for the Computation of Mathematical Functions" p.182-191, Luke, Y.L (1977), Academic Press and 
        ///"Computing the confluent hypergeometric function, M(a,b,x)", K. E. Muller, Numer. Math. 90, p.179-196 (2001). The later reference has several typos.</remarks>
        public Complex GetValue(Complex a, Complex b, Complex z)
        {
            /* remark: This implementation is almost identical to the real-argument implementation above. */

            if (a.Equals(b))  // without tolerance
            {
                return Complex.Exp(z);
            }
            else if (a.Equals(0.0) || z.Equals(0.0)) // without tolerance
            {
                return 1.0;
            }
            else if ((b == 0) || ((b.Imaginary == 0.0) && (b.Real < 0.0) && (b.Real == Math.Round(b.Real))))  // if b is a non-positive integer or 0, the function is not defined
            {
                throw new ArgumentException(String.Format(ExceptionMessages.ArgumentIsInvalidForObject, z, "1F1"), nameof(z));
            }
            /* We replace 'x' by '-x' and '\delta' by 'a' in "Computing the confluent hypergeometric function M(a,b,x)", K. E. Muller */

            Complex QnMinus3 = 1.0;
            var QnMinus2 = 1 - z * (a + 1) / (2.0 * b);
            var QnMinus1 = 1 - z * (a + 2) / (2 * (b + 1)) + z * z * (a + 1) * (a + 2) / (12 * b * (b + 1));

            Complex PnMinus3 = 1.0;
            var PnMinus2 = QnMinus2 + z * a / b;

            var PnMinus1 = QnMinus1 + a * z / b * (1 - z * (a + 2) / (2 * (b + 1))) + z * z * a * (a + 1) / (2 * b * (b + 1));  // the first '(b+1)' was '(b+2) in the paper of K. E. Muller

            Complex Qn, Pn;
            Qn = Pn = 0.0;

            Complex previousValue = 0.0;
            for (int n = 3; n < m_MaxIteration; n++)
            {
                var fi1 = (n - a - 2) / (2 * (2 * n - 3) * (n + b - 1));
                var fi2 = (n + a) * (n + a - 1) / (4 * (2 * n - 1) * (2 * n - 3) * (n + b - 2) * (n + b - 1));  // additional term in paper of K. E. Muller
                var fi3 = -(n + a - 2) * (n + a - 1) * (n - a - 2) / (8 * (2 * n - 3) * (2 * n - 3) * (2 * n - 5) * (n + b - 3) * (n + b - 2) * (n + b - 1)); // different sign in the 3. product in the paper of K. E. Muller
                var fi4 = -(n + a - 1) * (n - b - 1) / (2 * (2 * n - 3) * (n + b - 2) * (n + b - 1));

                Pn = (1 - fi1 * z) * PnMinus1 - (fi4 - fi2 * z) * z * PnMinus2 - fi3 * z * z * z * PnMinus3;  // an additional '-x' was missing in paper of K. e. Muller
                Qn = (1 - fi1 * z) * QnMinus1 - (fi4 - fi2 * z) * z * QnMinus2 - fi3 * z * z * z * QnMinus3;

                if ((n >= 4) && (Complex.Abs(Pn / Qn - previousValue) < MachineConsts.ExtremeTinyEpsilon * (2.0 + Complex.Abs(previousValue))))  // abort condition
                {
                    return Pn / Qn;
                }
                previousValue = Pn / Qn;

                PnMinus3 = PnMinus2;
                PnMinus2 = PnMinus1;
                PnMinus1 = Pn;

                QnMinus3 = QnMinus2;
                QnMinus2 = QnMinus1;
                QnMinus1 = Qn;
            }
            return Pn / Qn;
        }
        #endregion
    }
}