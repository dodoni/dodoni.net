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

namespace Dodoni.MathLibrary.Miscellaneous
{
    /// <summary>Represents the implementation of the Lambert W function (Principal branch only) with respect to Fritsch's Iteration approach. Moreover a rational approximation is used for calculation of initial guess.
    /// </summary>
    /// <remarks>The implementation is based on "Having Fun with Lambert W(x) Function", Darko Veberic, University of Nova Gorica, Slovenia, IK, Forschungszentrum Karlsruhe, Germany, J. Stefan Institutre, Ljubljana, Slovenia; June 2009.</remarks>
    internal class LambertWFunctionFritsch
    {
        #region private members

        /// <summary>The maximal number of iterations with respect to Fritsch's Iteration. In general two iterations should give an accurate result.
        /// </summary>
        private const int MaxIteration = 10;

        /// <summary>The polynomial A(x), where Q_0^{[1]}(x) = x * A(x) / B(x) is a accurate initial value of W(x) in a specific range.
        /// </summary>
        private IRealPolynomial m_PolynomialA1;

        /// <summary>The polynomial B(x), where Q_0^{[1]}(x) = x * A(x) / B(x) is a accurate initial value of W(x) in a specific range.
        /// </summary>
        private IRealPolynomial m_PolynomialB1;

        /// <summary>The polynomial A(x), where Q_0^{[2]}(x) = x * A(x) / B(x) is a accurate initial value of W(x) in a specific range.
        /// </summary>
        private IRealPolynomial m_PolynomialA2;

        /// <summary>The polynomial B(x), where Q_0^{[2]}(x) = x * A(x) / B(x) is a accurate initial value of W(x) in a specific range.
        /// </summary>
        private IRealPolynomial m_PolynomialB2;

        /// <summary>The coefficients used for calculation of accurate initial value of W(x) with respect to (37) in the reference.
        /// </summary>
        private double[] m_InitialValueCoefficients;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="LambertWFunctionFritsch" /> class.
        /// </summary>
        internal LambertWFunctionFritsch()
        {
            m_PolynomialA1 = Polynomial.Real.Create(1.0, 5.931375839364438, 11.39220550532913, 7.33888339911111, 0.653449016991959);
            m_PolynomialB1 = Polynomial.Real.Create(1.0, 6.931373689597704, 16.82349461388016, 16.43072324143226, 5.115235195211697);

            m_PolynomialA2 = Polynomial.Real.Create(1.0, 2.445053070726557, 1.343664225958226, 0.148440055397592, 0.0008047501729130);
            m_PolynomialB2 = Polynomial.Real.Create(1.0, 3.444708986486002, 3.292489857371952, 0.916460018803122, 0.0530686404483322);

            m_InitialValueCoefficients = new double[] { -1.0, 1.0, -1.0 / 3.0, 11.0 / 72.0, -43.0 / 540.0, 769.0 / 17280.0, -221.0 / 8505.0, 680863.0 / 43545600.0, -1963.0 / 204120.0, 226287557.0 / 37623398400.0 };
        }
        #endregion

        #region public methods

        /// <summary>Gets the value W_0(x) of the Principal Branch of Lambert's W function.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <returns>The value W_0(x) of the Principal Branch of Lambert's W function.</returns>
        /// <remarks>This implementation is based on "Having Fun with Lambert W(x) Function", Darko Veberic, University of Nova Gorica, Slovenia, IK, Forschungszentrum Karlsruhe, Germany, J. Stefan Institutre, Ljubljana, Slovenia; June 2009.</remarks>
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
            else if (x == 0.0)
            {
                return 0.0;
            }
            var wk = GetInitialW0(x);   // initial value
            if (wk == 0.0)  /* this case should never occur, because it should happen in case x == 0.0 only. */
            {
                if (Math.Abs(x) < MachineConsts.TinyEpsilon)
                {
                    return 0.0;
                }
                wk = MachineConsts.Epsilon;
            }
            var expOfwk = Math.Exp(wk);

            /* apply Fritsch's iteration as described in the reference */
            for (int k = 1; k <= MaxIteration; k++)
            {
                var z = Math.Log(x / wk) - wk;
                var q = 2.0 * (1 + wk) * (1 + wk + 2.0 / 3.0 * z);
                var epsilon = z / (1 + wk) * (q - z) / (q - 2 * z);

                wk = wk * (1 + epsilon);

                if (Math.Abs(epsilon) < MachineConsts.ExtremeTinyEpsilon * (2 + Math.Abs(wk)))
                {
                    return wk;
                }
            }
            return wk;
        }
        #endregion

        #region private methods

        /// <summary>Gets the initial approximation of W_0(x) with respect to (45) of the reference.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <returns>The approximation of W_0(x).</returns>
        private double GetInitialW0(double x)
        {
            if (x < -MathConsts.OneOverE)
            {
                throw new ArgumentException();
            }

            if (x < -0.32358170806015724)
            {
                var p = Math.Sqrt(2.0 + 2.0 * Math.E * x);

                var w = 0.0;
                var powerOfp = 1.0;
                for (int i = 0; i < m_InitialValueCoefficients.Length; i++)
                {
                    w += m_InitialValueCoefficients[i] * powerOfp;
                    powerOfp *= p;
                }
                return w;
            }
            else if (x < 0.14546954290661823)
            {
                return x * m_PolynomialA1.GetValue(x) / m_PolynomialB1.GetValue(x);
            }
            else if (x < 8.706658967856612)
            {
                return x * m_PolynomialA2.GetValue(x) / m_PolynomialB2.GetValue(x);
            }
            else
            {
                var a = Math.Log(x);
                var b = Math.Log(a);  // = log[log(x)]

                return a - b + b / a + b * (b - 2.0) / (2 * a * a) + b * (6.0 - 9.0 * b + 2.0 * b * b) / (6.0 * a * a * a) + b * (-12.0 + 36.0 * b - 22.0 * b * b + 3.0 * b * b * b) / (12.0 * a * a * a * a) + b * (60.0 - 300.0 * b + 350.0 * b * b - 125 * b * b * b + 12.0 * b * b * b * b) / (60.0 * a * a * a * a * a);
            }
        }
        #endregion
    }
}