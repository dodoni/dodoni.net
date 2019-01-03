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
    /// <summary>Represents the implementation of the (complementary) error function based on a rational Chebyshev approximation.
    /// </summary>
    /// <remarks>The implementation is based on 
    /// <para>"Rational Chebyshev approximations for the error function", W. J. Cody, Mathematics of Computation 23; p.631-637</para>
    /// with some modifications based on
    /// <para>"A set of algorithms for the incomplete Gamma functions", N. M. Temme, 1994.</para>
    /// </remarks>
    internal class ErrorFunctionCodyAlgorithm : ErrorFunction
    {
        #region private members

        /// <summary>The square-root of -log(<see cref="MachineConsts.DBL_Epsilon"/>).
        /// </summary>
        private double m_SqrtMinusLogMachineTolerance;

        /// <summary>One over the square-root of two times the machine tolerance.
        /// </summary>
        private double m_OneOverSqrtTwoMachineTolerance;

        /// <summary>The square-root of minus logarithm of <see cref="Double.Epsilon"/>.
        /// </summary>
        private double m_SqrtOfMinusLogEpsilon = Math.Sqrt(-Math.Log(Double.Epsilon));

        /// <summary>The coefficients p_j for erfc(x) = x * \sum_j p_j * x^{2j} / \sum_j q_j * x^{2j}, within |x| \in [0.0, 0.5]; see table II. in the "Rational Chebyshev approximations for the error function", W. J. Cody (1969).
        /// </summary>
        private double[] m_CoefficientsP_2 = new[]{
                    3.209377589138469472562E3,
                    3.774852376853020208137E2,
                    1.138641541510501556495E2,
                    3.161123743870565596947E0,
                    1.85777706184603152673E-1};

        /// <summary>The coefficients q_j for erfc(x) = x * \sum_j p_j * x^{2j} / \sum_j q_j * x^{2j}, within |x| \in [0.0, 0.5]; see table II. in the "Rational Chebyshev approximations for the error function", W. J. Cody (1969).
        /// </summary>
        private double[] m_CoefficientsQ_2 = new[] {        
                    2.844236833439170622273E3,        
                    1.282616526077372275645E3,        
                    2.440246379344441733056E2,        
                    2.360129095234412093499E1};

        /// <summary>The coefficients p_j for erfc(x) = e^{-x^2} * P(x) / Q(x) within x \in [0.46875, 4.0]; see table III. in the "Rational Chebyshev approximations for the error function", W. J. Cody (1969).
        /// </summary>
        private double[] m_CoefficientsP_3 = new[] {
                        1.23033935479799725272E3,
                        2.05107837782607146532E3,
                        1.71204761263407058314E3,
                        8.81952221241769090411E2,
                        2.98635138197400131132E2,
                        6.61191906371416294775E1,
                        8.88314979438837594118,
                        5.64188496988670089180E-1,
                        2.15311535474403846343E-8};

        /// <summary>The coefficients q_j for erfc(x) = e^{-x^2} * P(x) / Q(x) within x \in [0.46875, 4.0]; see table III. in the "Rational Chebyshev approximations for the error function", W. J. Cody (1969).
        /// </summary>
        private double[] m_CoefficientsQ_3 = new[] {
                        1.23033935480374942043E3,
                        3.43936767414372163696E3,
                        4.36261909014324715820E3,
                        3.29079923573345962678E3,
                        1.62138957456669018874E3,
                        5.37181101862009857509E2,
                        1.17693950891312499305E2,
                        1.57449261107098347253E1};

        /// <summary>The coefficients p_j for erfc(x) = e^{-x^2} / x * [ 1/\sqrt{\pi} + x^{-2} * \sum_j p_j * x^{-2j} / \sum_j q_j * x^{-2j} ] within x \in [4.0, \infty[; see table IV. in the "Rational Chebyshev approximations for the error function", W. J. Cody (1969).
        /// </summary>
        private double[] m_CoefficientsP_4 = new[] {
                    6.58749161529837803157E-4,
                    1.60837851487422766278E-2,
                    1.25781726111229246204E-1,
                    3.60344899949804439429E-1,
                    3.05326634961232344035E-1,
                    1.63153871373020978498E-2};

        /// <summary>The coefficients q_j for erfc(x) = e^{-x^2} / x * [ 1/\sqrt{\pi} + x^{-2} * \sum_j p_j * x^{-2j} / \sum_j q_j * x^{-2j} ] within x \in [4.0, \infty[; see table IV. in the "Rational Chebyshev approximations for the error function", W. J. Cody (1969).
        /// </summary>
        private double[] m_CoefficientsQ_4 = new[] {
                     2.33520497626869185443E-3,
                     6.05183413124413191178E-2,
                     5.27905102951428412248E-1,
                     1.87295284992346047209,
                     2.56852019228982242072};
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="ErrorFunctionCodyAlgorithm" /> class.
        /// </summary>
        public ErrorFunctionCodyAlgorithm()
        {
            m_SqrtMinusLogMachineTolerance = Math.Sqrt(MachineConsts.MinusLogOfDBL_Epsilon);
            m_OneOverSqrtTwoMachineTolerance = 1.0 / Math.Sqrt(2 * MachineConsts.DBL_Epsilon);
        }
        #endregion

        #region public methods

        /// <summary>Gets a specific value of the error function erf(x) = 2/\sqrt{PI} * \int_0^x e^{-t^2} dt.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <returns>The specified value of the error function erf(x) = 2/\sqrt{PI} * \int_0^x e^{-t^2} dt.</returns>
        public override double GetValue(double x)
        {
            if (double.IsPositiveInfinity(x) == true)
            {
                return 1;
            }
            else if (double.IsNegativeInfinity(x) == true)
            {
                return -1;
            }
            else if (double.IsNaN(x) == true)
            {
                return double.NaN;
            }
            return GetValue(x, isErfc: false);
        }

        /// <summary>Gets a specific value of the complementary error function erfc(x) = 1- erf(x) = 2/\sqrt{PI} * \int_x^\infty e^{-t^2} dt.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <returns>The specified value of the complementary error function erfc(x) = 1- erf(x) = 2/\sqrt{PI} * \int_x^\infty e^{-t^2} dt.</returns>
        public override double GetCValue(double x)
        {
            if (x == 0)
            {
                return 1;
            }
            else if (double.IsPositiveInfinity(x) == true)
            {
                return 0;
            }
            else if (double.IsNegativeInfinity(x) == true)
            {
                return 2;
            }
            else if (double.IsNaN(x) == true)
            {
                return double.NaN;
            }
            return GetValue(x, isErfc: true);
        }

        /// <summary>Gets a specific value of the exponentially scaled complementary error function erfce(x) = exp(x^2) * ( 1- erf(x) ) = exp(x^2) * 2/\sqrt{PI} * \int_x^\infty e^{-t^2} dt.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <returns>The specified value of the exponentially scaled complementary error function erfce(x) = exp(x^2) * ( 1- erf(x) )  = exp(x^2) * 2/\sqrt{PI} * \int_x^\infty e^{-t^2} dt.</returns>
        public double GetExponentialScaledCValue(double x)
        {
            if (x == 0)
            {
                return 1;
            }
            else if (double.IsPositiveInfinity(x) == true)
            {
                return Double.PositiveInfinity;
            }
            else if (double.IsNegativeInfinity(x) == true)
            {
                return 0.0;
            }
            else if (double.IsNaN(x) == true)
            {
                return double.NaN;
            }
            return GetValue(x, isErfc: true, applyExponentialFactorToErfc: true);
        }
        #endregion

        #region private methods

        /// <summary>Gets a specific value of the error function.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <param name="isErfc">A value indicating whether to calculate erf(x) = 2/\sqrt{PI} * \int_0^x e^{-t^2} dt or the complementary error function erfc(x) = 1- erf(x) = 2/\sqrt{PI} * \int_x^\infty e^{-t^2} dt.</param>
        /// <param name="applyExponentialFactorToErfc">A value indicating whether the result should is scaled with exp(x^2) in the case of the complementary error function erfc(x).</param>
        /// <returns>The specified value of the (complementary) error function, perhaps multiplied with exp(x^2).</returns>
        private double GetValue(double x, bool isErfc, bool applyExponentialFactorToErfc = false)
        {
            if (isErfc == true)
            {
                if (x < -m_SqrtMinusLogMachineTolerance)
                {
                    return 2.0;
                }
                else if (x < -MachineConsts.DBL_Epsilon) // x < 0.0
                {
                    return 2.0 - GetValue(-x, true, false);
                }
                else if (x < MachineConsts.DBL_Epsilon) // x == 0.0
                {
                    return 1.0;
                }
                else if (x < 0.5)
                {
                    return (1.0 - GetValue(x, false, false)) * ((applyExponentialFactorToErfc == true) ? Math.Exp(x * x) : 1.0);
                }
                else if (x < 4.0)
                {
                    return GetRationalFraction(x, 8, m_CoefficientsP_3, m_CoefficientsQ_3) * ((applyExponentialFactorToErfc == true) ? 1.0 : Math.Exp(-x * x));
                }
                else
                {
                    var y = 1.0;
                    var z = x * x;

                    if (applyExponentialFactorToErfc == true)
                    {
                        var xl = 1.0 / (MathConsts.SqrtPi * Double.Epsilon);
                        if (x > xl)
                        {
                            return 0.0;
                        }
                        else if (x > m_OneOverSqrtTwoMachineTolerance)
                        {
                            return 1.0 / (x * MathConsts.SqrtPi);
                        }
                    }
                    else
                    {
                        if (x < m_SqrtOfMinusLogEpsilon)
                        {
                            y = Math.Exp(-z);

                            if (x * Double.Epsilon > y * MathConsts.OneOverSqrtPi)
                            {
                                return 0.0;
                            }
                        }
                        else
                        {
                            return 0.0;
                        }
                    }
                    z = 1.0 / z;
                    return y * (MathConsts.OneOverSqrtPi - z * GetRationalFraction(z, 5, m_CoefficientsP_4, m_CoefficientsQ_4)) / x;
                }
            }
            else
            {
                if (x == 0.0)
                {
                    return 0.0;
                }
                else if (Math.Abs(x) > m_SqrtMinusLogMachineTolerance)
                {
                    return x / Math.Abs(x);
                }
                else if (x > 0.5)
                {
                    return 1.0 - GetValue(x, true, false);
                }
                else if (x < -0.5)
                {
                    return GetValue(-x, true, false) - 1.0;
                }
                else
                {
                    return x * GetRationalFraction(x * x, 4, m_CoefficientsP_2, m_CoefficientsQ_2);
                }
            }
        }

        /// <summary>Gets the fraction P(x)/Q(x) of two polynomial functions.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <param name="n">The degree of both polynomials.</param>
        /// <param name="p">The coefficients of polynomial P(x).</param>
        /// <param name="q">The coefficients of polynomial Q(x), the n-th coefficient is assumed to be 1.0 and not stored in this argument.</param>
        /// <returns>The value of P(x)/Q(x).</returns>
        private double GetRationalFraction(double x, int n, double[] p, double[] q)
        {
            double a = p[n];
            double b = 1.0;
            for (int k = n - 1; k >= 0; k--)
            {
                a = a * x + p[k];
                b = b * x + q[k];
            }
            return a / b;
        }
        #endregion
    }
}