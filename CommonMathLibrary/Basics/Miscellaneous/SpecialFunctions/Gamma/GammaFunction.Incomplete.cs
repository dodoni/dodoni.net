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
using Dodoni.MathLibrary.Basics;

namespace Dodoni.MathLibrary.Miscellaneous
{
    public partial class GammaFunction
    {
        #region private members

        /// <summary>The demanded accuracy of the calculation.
        /// </summary>
        private const double m_IncompleteEpsilon = 1E-13;

        /// <summary>The maximal iterations taken into account for the calculation.
        /// </summary>
        private const int m_IncompleteMaxIteration = 50;

        /// <summary>The pre-computed coefficients d_n with respect to (2.36) of the reference, used to compute S_a(\eta).
        /// </summary>
        private double[] m_CoefficientsDn = new double[]{
             1.0,
            -1.0 / 3.0,
             1.0 / 12.0,
            -2.0 / 135.0,
             1.0 / 864.0,
             1.0 / 2835.0,
            -139.0 / 777600.0,
             1.0 / 25515.0,
            -571.0 / 261273600.0,
            -281.0 / 151559100.0,
             8.29671134095308601e-7,
            -1.76659527368260793e-7,
             6.70785354340149857e-9,
             1.02618097842403080e-8,
            -4.38203601845335319e-9,
             9.14769958223679023e-10,
            -2.55141939949462497e-11,
            -5.83077213255042507e-11,
             2.43619480206674162e-11,
            -5.02766928011417559e-12,
             1.10043920319561347e-13,
             3.37176326240098538e-13,
            -1.39238872241816207e-13,
             2.85348938070474432e-14,
            -5.13911183424257258e-16,
            -1.97522882943494428e-15,
             8.09952115670456133e-16
           };

        /// <summary>Coefficients (Tschebycheff) for the calculation of g(a) satisfing 1 - 1/\gamma(a+1) = a * (1-a) * g(a).
        /// </summary>
        private double[] m_AuxGammaCoefficients = new double[]{
              -1.013609258009865776949,
               0.784903531024782283535e-1,
               0.67588668743258315530e-2,
              -0.12790434869623468120e-2,
               0.462939838642739585e-4,
               0.43381681744740352e-5,
              -0.5326872422618006e-6,
               0.172233457410539e-7,
               0.8300542107118e-9,
              -0.10553994239968e-9,
               0.39415842851e-11,
               0.362068537e-13,
              -0.107440229e-13,
               0.5000413e-15,
              -0.62452e-17,
              -0.5185e-18,
               0.347e-19,
              -0.9e-21 };


        /// <summary>Some coefficients for the Stirling formula.
        /// </summary>
        private double[] m_StirlingCoefficientsLow = new double[]{
                           1.996379051590076518221,
                          -0.17971032528832887213e-2,
                           0.131292857963846713e-4,
                          -0.2340875228178749e-6,
                           0.72291210671127e-8,
                          -0.3280997607821e-9,
                           0.198750709010e-10,
                          -0.15092141830e-11,
                           0.1375340084e-12,
                          -0.145728923e-13,
                           0.17532367e-14,
                          -0.2351465e-15,
                           0.346551e-16,
                          -0.55471e-17,
                           0.9548e-18,
                          -0.1748e-18,
                           0.332e-19,
                          -0.58e-20};
        /// <summary>Some coefficients for the Stirling formula.
        /// </summary>
        private double[] m_StirlingCoefficientsHigh = new double[]{ 
                         0.25721014990011306473e-1,
                         0.82475966166999631057e-1,
                        -0.25328157302663562668e-2,
                         0.60992926669463371e-3,
                        -0.33543297638406e-3,
                         0.250505279903e-3,
                         0.30865217988013567769};

        /// <summary>A large number used in the calculation.
        /// </summary>
        private const double m_Gigant = Double.MaxValue / 1000.0;
        #endregion

        #region public methods

        /// <summary>Gets the value of the normalized incompleted gamma function.
        /// </summary>
        /// <param name="a">The parameter a.</param>
        /// <param name="x">The parameter x.</param>
        /// <param name="normalizedLowerValue">The normalized lower value.</param>
        /// <param name="normalizedUpperValue">The normalized upper value.</param>
        /// <remarks>The implementation is based on "Efficient and Accurate Algorithms for the Computation and Inversion of the Incomplete Gamma Function Ratios", A. Gil, J. Segura, N. M. Temme, June 2013
        /// and the Fortran code referenced in the paper.</remarks>
        public void GetNormalizedIncompletedValue(double a, double x, out double normalizedLowerValue, out double normalizedUpperValue)
        {
            if (x < 0)
            {
                throw new ArgumentOutOfRangeException("x", String.Format(ExceptionMessages.ArgumentOutOfRangeGreaterEqual, "x", 0.0));
            }
            var dwarf = Double.Epsilon * 1000.0;
            var lnX = (x < dwarf) ? Math.Log(dwarf) : Math.Log(x);

            if (a > alpha(x))  // calculate lower value 'P(x)' first
            {
                var dp = GetDFactorPart(a, x);
                if (dp < 0.0)
                {
                    throw new OverflowException();  // over-/underflow problem
                }
                if ((x < 0.3 * a) || (a < 12.0))
                {
                    normalizedLowerValue = GetTaylorExpansion_P(a, x, dp);
                }
                else
                {
                    normalizedLowerValue = GetUniformAsymptoticExpansion(a, x, dp, sign: -1);
                }
                normalizedUpperValue = 1.0 - normalizedLowerValue;
            }
            else  // calculate upper value 'Q(x) first
            {
                if (a < -dwarf / lnX)
                {
                    normalizedUpperValue = 0.0;
                }
                else
                {
                    if (x < 1.0)
                    {
                        normalizedUpperValue = GetTaylorExpansion_Q(a, x);
                    }
                    else
                    {
                        double dp = GetDFactorPart(a, x);
                        if (dp < 0)
                        {
                            throw new OverflowException();  // over-/underflow problem
                        }
                        if ((x > 2.35 * a) || (a < 12.0))
                        {
                            normalizedUpperValue = GetContinuedFraction_Q(a, x, dp);
                        }
                        else
                        {
                            normalizedUpperValue = GetUniformAsymptoticExpansion(a, x, dp, sign: 1);
                        }
                    }
                }
                normalizedLowerValue = 1.0 - normalizedUpperValue;
            }
        }
        #endregion

        #region private methods

        /// <summary>Gets the taylor expansion for P(a,x) with respect to §2.2 of the reference.
        /// </summary>
        /// <param name="a">The parameter a.</param>
        /// <param name="x">The parameter x.</param>
        /// <param name="prefactor">The prefactor x^a * e^{-x} / \Gamma(a+1).</param>
        /// <returns>The approximation for P(a,x) with respect to the Taylor expansion.</returns>
        private double GetTaylorExpansion_P(double a, double x, double prefactor)
        {
            if (prefactor == 0.0)
            {
                return 0.0;
            }

            var sum = 1.0;
            var addend = 1.0; // = x^n / (a+1)_n
            var denominator = a; // = (a+1)_n

            int k = 1;
            while ((k <= m_IncompleteMaxIteration) && ((addend / sum) > m_IncompleteEpsilon))
            {
                denominator = denominator + 1;
                addend = x * addend / denominator;
                sum = sum + addend;
                k++;
            }
            return prefactor * sum;
        }

        /// <summary>Gets the continued fraction for Q(a,x) with respect to § 2.4 of the reference.
        /// </summary>
        /// <param name="a">The parameter a.</param>
        /// <param name="x">The parameter x.</param>
        /// <param name="prefactor">The prefactor x^a * exp({-x} / \Gamma(a+1).</param>
        /// <returns>The approximation for Q(x,a) with respect to § 2.4 of the reference.</returns>
        private double GetContinuedFraction_Q(double a, double x, double prefactor)
        {
            if (prefactor == 0.0)
            {
                return 0.0;
            }
            var p = 0.0;
            var q = (x - 1.0 - a) * (x + 1.0 - a);
            var r = 4.0 * (x + 1.0 - a);
            var s = 1.0 - a;

            var pk = 0.0;  // = p_k (2.28)
            var t = 1.0;  // = t_k =p_1...p_k (2.28)
            var Sax = 1.0; //  S(a,x) = 1 + \sum_{k=1}^\infty t_k (2.27)

            int k = 1;
            while ((k <= m_IncompleteMaxIteration) && (Math.Abs(t / Sax) >= m_IncompleteEpsilon))
            {
                p = p + s;
                q = q + r;
                r = r + 8.0;
                s = s + 2.0;
                var tau = p * (1.0 + pk);
                pk = tau / (q - tau);

                t *= pk;
                Sax += t;
                k++;
            }
            return a * prefactor / (x + 1.0 - a) * Sax;  // use \Gamma(a+1) = a * \Gamma(a) for the correction of the pre-factor to x^a * e^{-x} / \Gamma(a)
        }

        /// <summary>Gets the taylor expansion for Q(a,x) with respect to §2.3 of the reference.
        /// </summary>
        /// <param name="a">The parameter a.</param>
        /// <param name="x">The parameter x.</param>
        /// <returns>The approximation for Q(a,x) with respect to §2.3 of the reference.</returns>
        private double GetTaylorExpansion_Q(double a, double x)
        {
            /* 1.) calculate u = 1 - 1/\Gamma(1+a) + (1-x^a)/\Gamma(1+a): (2.22) */

            var oneMinusOneOverGamma = a * (1.0 - a) * AuxGamma(a); // = 1 - 1/\Gamma(1+a)

            var lnX = Math.Log(x);
            var w = a * lnX;
            var u = oneMinusOneOverGamma - (1 - oneMinusOneOverGamma) * w * GetExpArgumentMinus1DivArgument(w, m_IncompleteEpsilon);


            /* 2.) calculate  (2.24), i.e.
             *       v = x^a / \Gamma(1+a) * ( 1 - \Gamma(1+a) * x^{-a} * P(a,x) )
             *         = -x^a/\Gamma(a) * \sum_{n=1}^\infty (-1)^n * c^n /[ (a+n) * n! ], thus skipping the first term of the expansion
             *  */
            var p = a * x;
            var q = a + 1;
            var r = a + 3;

            var t = 1.0;
            var vPart = 1.0;  // = - (1+a) / x * \sum_{n=1}^\infty (-1)^n * x^n / [(a+n) * n!] = 1 - (1+a) * x / [ 2 * (2+a)] + ...

            int k = 1;
            while ((k <= m_IncompleteMaxIteration) && (Math.Abs(t / vPart) > m_IncompleteEpsilon))
            {
                p = p + x;
                q = q + r;
                r = r + 2;
                t = -p * t / q;
                vPart = vPart + t;

                k++;
            }
            /* we have a / \Gamma(1+a) * x^{a+1} * vPart / (1+a) = x^a / \Gamma(a) * x /(1+a) * vPart = v
             */
            return u + a * (1 - oneMinusOneOverGamma) * Math.Exp((a + 1.0) * lnX) * vPart / (a + 1.0);
        }

        /// <summary>Gets the uniform asymptotic expansion for P(a,x), Q(a,x) with respect to §2.5 of the reference.
        /// </summary>
        /// <param name="a">The parameter a.</param>
        /// <param name="x">The parameter x.</param>
        /// <param name="prefactor"></param>
        /// <param name="sign">A value indicating whether to calculate the uniform asymptotic expansion for Q(a,x) [i.e. 1] or P(a,x) [i.e. -1].</param>
        /// <returns>The uniform asymptotic expansion for P(a,x) or Q(a,x) indicating by <paramref name="sign"/> with respect to §2.5 of the reference.</returns>
        private double GetUniformAsymptoticExpansion(double a, double x, double prefactor, int sign)
        {
            if (prefactor == 0.0)
            {
                if (sign == -1)
                {
                    return 0.0;
                }
                return 1.0;
            }

            var mu = (x - a) / a;
            var y = -GetLogOfOnePlusWMinusW(mu);
            var eta = (y < 0.0) ? 0.0 : Math.Sqrt(2.0 * y);

            y = y * a; // = -0.5 * a * \eta^2
            var v = Math.Sqrt(Math.Abs(y));
            if (mu < 0.0)
            {
                eta = -eta;
                v = -v;
            }
            return 0.5 * SpecialFunction.PrimitiveIntegral.Erfc(sign * v) + sign * Math.Exp(-y) * GetSaAtEta(a, eta) / Math.Sqrt(2 * Math.PI * a);
        }

        /// <summary>Gets the value of S_a(\eta) with respect to (2.33) of the reference.
        /// </summary>
        /// <param name="a">The parameter a.</param>
        /// <param name="eta">The argument of S_a(\eta).</param>
        /// <returns>The approximation of S_a(\eta) with respect to (2.33) of the reference.</returns>
        private unsafe double GetSaAtEta(double a, double eta)
        {
            double* beta = stackalloc double[27]; // \beta = (\beta_0, ..., \beta_m) avoid to store all temporary coefficients on the heap

            beta[25] = m_CoefficientsDn[26];
            beta[24] = m_CoefficientsDn[25];
            for (int m = 24; m >= 1; m--)
            {
                beta[m - 1] = m_CoefficientsDn[m] + (m + 1) * beta[m + 1] / a;
            }

            int k = 1;
            var s = beta[0];
            var t = s;
            var y = eta;  // = \eta^k

            while ((k < 25) && (Math.Abs(t / s) > m_IncompleteEpsilon))
            {
                t = beta[k] * y;
                s += t;
                y *= eta;

                k++;
            }
            return s / (1.0 + beta[1] / a);  // todo: is this 1 over \Gamma^*(a) ?
        }

        /// <summary>The function \alpha(x) with respect to (2.1) of the reference.
        /// </summary>
        /// <param name="x">The argument x.</param>
        /// <returns>The value of \alpha(x).</returns>
        /// <remarks>The Fortran implementation of the reference takes into account an other implementation; see W. Gautschi: "A computational procedure for incomplete gamma functions", ACM Trans. Math. Software, 5 (4):466-481, 1979.</remarks>
        private double alpha(double x)
        {
            if (x >= 0.5)
            {
                return x;
            }
            return Math.Log(0.5) / Math.Log(0.5 * x);  // todo: Log(1/2)
        }

        /// <summary>Calculate function g with 1 - 1/\gamma(a+1) = a * (1-a) * g(a) for a \in [-1,1]; see (2.23).
        /// </summary>
        /// <param name="a">The argument a.</param>
        /// <returns>The value of g(a) that satisfied 1 - 1/\gamma(a+1) = a * (1-a) * g(a).</returns>
        private double AuxGamma(double a)
        {
            if (a < 0.0)
            {
                return -(1.0 + (1 + a) * (1 + a) * AuxGamma(1 + a)) / (1.0 - a);
            }
            else
            {
                var t = 2 * a - 1.0;
                return GetChebychevPolynomialSumValue(m_AuxGammaCoefficients.Length - 1, t, m_AuxGammaCoefficients);
            }
        }

        /// <summary>Gets the value of a specific weighted sum of Chebychev polynomials.
        /// </summary>
        /// <param name="n">The number of coefficients.</param>
        /// <param name="x">The argument to evaluate the polynomials.</param>
        /// <param name="a">The coefficients used for calculation of the Chebychev polynomials, i.e. 0.5 * a[0] + T_1(x) * a[1] + ... + T_n(x) * a[n], where T_1,...,T_n Chebychev polynomial.</param>
        /// <returns>The sum of the weighted Chebychev polynomials.</returns>
        private double GetChebychevPolynomialSumValue(int n, double x, double[] a)
        {
            if (n == 0)
            {
                return 0.5 * a[0];
            }
            else if (n == 1)
            {
                return 0.5 * a[0] + x * a[1];
            }
            var tx = x + x;
            var r = a[n];
            var h = a[n - 1] + r * tx;
            for (int k = n - 2; k >= 1; k--)
            {
                var s = r;
                r = h;
                h = a[k] + r * tx - s;
            }
            return 0.5 * a[0] + h * x;
        }

        /// <summary>Computes an approximation for D(a,x) = x^a * e^{-x}/\Gamma(a+1) with respect to (2.3) of the reference.
        /// </summary>
        /// <param name="a">The parameter a.</param>
        /// <param name="x">The parameter x.</param>
        /// <returns>The approximation for D(a,x).</returns>
        private double GetDFactorPart(double a, double x)
        {
            var lnX = Math.Log(x);

            if ((a < 3.0) || (x < 0.2))
            {
                return Math.Exp(a * lnX - x) / GetValue(a + 1.0);
            }
            double mu = (x - a) / a;
            double c = GetLogOfOnePlusWMinusW(mu);

            if ((a * c) > Math.Log(m_Gigant))
            {
                return -100;
            }
            return Math.Exp(a * c) / (Math.Sqrt(MathConsts.TwoPi * a) * GammaStar(a));
        }

        /// <summary>Gets an approximation for \Gamma*(x), i.e. exp(Stirling(x)) if x greater than 3.0; otherwise \Gamma(x) / [ exp(-x + (x-0.5) * ln(x)) / Sqrt(2 * PI)]; see (2.5) of the reference.
        /// </summary>
        /// <param name="x">The argument x.</param>
        /// <returns>The approximation of \Gamma*(x).</returns>
        private double GammaStar(double x)
        {
            if (x >= 3.0)
            {
                return Math.Exp(Stirling(x));
            }
            else if (x > 0.0)
            {
                return GetValue(x) / (Math.Exp(-x + (x - 0.5) * Math.Log(x)) * MathConsts.SqrtTwoPi);
            }
            else
            {
                return m_Gigant;
            }
        }

        /// <summary>Evaluate Stirling series, function corresponding with asymptotic series for log(\Gamma(x)), i.e. 1/(12x) - 1/(360*x^3) + ...
        /// </summary>
        /// <param name="x">The argument x.</param>
        /// <returns>The approximation of Stirling series.</returns>
        private double Stirling(double x)
        {
            var dwarf = Double.Epsilon * 1000.0;
            if (x < dwarf)
            {
                return m_Gigant;
            }
            else if (x < 1.0)
            {
                return GetLogOfGammaOnePlusX(x) - (x + 0.5) * Math.Log(x) + x + MathConsts.LogSqrtTwoPi;
            }
            else if (x < 2.0)
            {
                return GetLogOfGammaOnePlusX(x - 1.0) - (x - 0.5) * Math.Log(x) + x + MathConsts.LogSqrtTwoPi;
            }
            else if (x < 3.0)
            {
                return GetLogOfGammaOnePlusX(x - 2.0) - (x - 0.5) * Math.Log(x) + x + MathConsts.LogSqrtTwoPi + Math.Log(x - 1.0);
            }
            else if (x < 12)
            {
                return GetChebychevPolynomialSumValue(m_StirlingCoefficientsLow.Length - 1, 18.0 / (x * x) - 1.0, m_StirlingCoefficientsLow)/(12.0 * x);
            }
            else
            {
                var z = 1.0 / (x * x);

                if (x < 1000.0)
                {
                    return ((((((m_StirlingCoefficientsHigh[5] * z + m_StirlingCoefficientsHigh[4]) * z + m_StirlingCoefficientsHigh[3]) * z + m_StirlingCoefficientsHigh[2]) * z + m_StirlingCoefficientsHigh[1]) * z + m_StirlingCoefficientsHigh[0]) / (m_StirlingCoefficientsHigh[6] + z) / x);
                }
                else
                {
                    return (((-z / 1680.0 + 1.0 / 1260.0) * z - 1.0 / 360.0) * z + 1.0 / 12.0) / x;
                }
            }
        }

        /// <summary>Gets log( \gamma(1+x) ) with good relative precision when |\gamma(1+x)| is small.
        /// </summary>
        /// <param name="x">The argument x.</param>
        /// <returns>The value of log( \gamma(1+x) ) with good relative precision when |\gamma(1+x)| is small.</returns>
        private double GetLogOfGammaOnePlusX(double x)
        {
            return -GetLogOfOnePlusArgument(x * (x - 1.0) * AuxGamma(x));  // = log \Gamma(1+x)
        }

        /// <summary>Gets \ln[w + 1] - w with respect to equation (2.6) and good relative precision when |w| is small.
        /// </summary>
        /// <param name="w">The argument w.</param>
        /// <returns></returns>
        /// <remarks>It holds -1/2 \eta^2 = -\lambda + 1 + \ln[\lambda] = \ln[w + 1] - w, where w = \lambda -1; see equation (2.6) of the reference.</remarks>
        private double GetLogOfOnePlusWMinusW(double w)
        {
            var z = GetLogOfOnePlusArgument(w);
            var e2 = exmin1minx(z, Double.Epsilon);
            var s = 0.5 * e2 * z * z;
            var y0 = z - w;

            var r = (s + y0) / (s + 1.0 + z);
            return y0 - r * (6.0 - r) / (6.0 - 4.0 * r);
        }

        /// <summary>Gets ln(1+w) with good relative precision when |w| is small.
        /// </summary>
        /// <param name="w">The argument.</param>
        /// <returns>ln(1+w) with good relative precision when |w| is small.</returns>
        private double GetLogOfOnePlusArgument(double w)
        {
            var y0 = Math.Log(1 + w);
            if ((-0.2928 < w) || (w < 0.4142))
            {
                var s = y0 * GetExpArgumentMinus1DivArgument(y0, Double.Epsilon);
                var r = (s - w) / (s + 1.0);
                return y0 - r * (6.0 - r) / (6.0 - 4.0 * r);
            }
            return y0;
        }

        /// <summary>Gets (exp(w) - 1) / w with a good relative precision when |w| is small.
        /// </summary>
        /// <param name="w">The argument.</param>
        /// <param name="epsilon">The epsilon.</param>
        /// <returns>(exp(w) - 1) / w with a good relative precision when |w| is small</returns>
        private double GetExpArgumentMinus1DivArgument(double w, double epsilon)
        {
            if (w == 0.0)
            {
                return 1.0;
            }
            else if ((w < -0.69) || (w > 0.4))
            {
                return (Math.Exp(w) - 1) / w;
            }
            else
            {
                var t = 0.5 * w;
                return Math.Exp(t) * Sinh(t, epsilon) / t;
            }
        }

        private double Sinh(double x, double epsilon)
        {
            var ax = Math.Abs(x);

            if (x == 0.0)
            {
                return 0.0;
            }
            else if (ax < 0.12)
            {
                var e = epsilon / 10.0;
                var x2 = x * x;
                var y = 1.0;
                var t = 1.0;
                int u = 0, k = 0;

                while (t > e)
                {
                    u = u + 8 * k - 2;
                    k = k + 1;
                    t = t * x2 / u;
                    y = y + t;
                }
                return x * y;
            }
            else if (ax < 0.36)
            {
                var t = Sinh(x / 3.0, epsilon);
                return t * (3 + 4 * t * t);
            }
            else
            {
                var t = Math.Exp(x);
                return 0.5 * (t - 1.0 / t);
            }
        }

        /// <summary>Gets [exp(x) - 1 - x] / [0.5 * x * x] with a good relative precision if x is small.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <param name="epsilon">The tolerance.</param>
        /// <returns>[exp(x) - 1 - x] / [0.5 * x * x] with a good relative precision if x is small.</returns>
        private double exmin1minx(double x, double epsilon)
        {
            if (x == 0.0)
            {
                return 1.0;
            }
            else if (Math.Abs(x) > 0.9)
            {
                return (Math.Exp(x) - 1.0 - x) / (0.5 * x * x);
            }
            var t = Sinh(0.5 * x, epsilon);
            var t2 = t * t;

            return (2.0 * t2 + (2.0 * t * Math.Sqrt(1.0 + t2) - x)) / (0.5 * x * x);
        }
        #endregion
    }
}