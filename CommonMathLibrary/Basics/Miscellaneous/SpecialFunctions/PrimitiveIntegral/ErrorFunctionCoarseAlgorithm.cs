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
    /// <summary>Represents the implementation of the (complementary) error function using a coarse algorithm.
    /// </summary>
    /// <remarks>The implementation for real arguments is based on 
    /// <para>"Algorithms with Guaranteed Error Bounds for the Error Function and the Complementary Error Function" Walter Krämer and Frithjof Blomquist Preprint 2000/2 Wissenschaftliches Rechnen/Softwaretechnologie Bergische Universität GH Wuppertal</para>
    /// and the XSC-implementation referenced in the paper.
    /// </remarks>
    internal class ErrorFunctionCoarseAlgorithm : ErrorFunction
    {
        #region private members

        /// <summary>The area, i.e. 0.0, 0.65, 2.2, 6.0, 26.5432, 30.0, mainly used for the calculation of Erfc(x).
        /// </summary>
        private double[] m_AreaErfc = { 0.0, 0.65, 2.2, 6.0, 26.5432, 30.0 };

        /// <summary>The area, i.e. 0.0, 1.97193e-308, 1e-10, 0.65, mainly used for the calculation of Erf(x).
        /// </summary>
        private double[] m_LowerAreaErf = { 0.0, 1.97193e-308, 1e-10, 0.65 };

        /// <summary>The polynomial P(x) used for the rational approximation of Erf(x).
        /// </summary>
        private IRealPolynomial m_ErfP;

        /// <summary>The polynomial Q(x) used for the rational approximation of Erf(x).
        /// </summary>
        private IRealPolynomial m_ErfQ;

        /// <summary>One of the polynomial P(x) used for the rational approximation of Erfc(x).
        /// </summary>
        private IRealPolynomial m_ErfcP1;

        /// <summary>One of the polynomial Q(x) used for the rational approximation of Erfc(x).
        /// </summary>
        private IRealPolynomial m_ErfcQ1;

        /// <summary>One of the polynomial P(x) used for the rational approximation of Erfc(x).
        /// </summary>
        private IRealPolynomial m_ErfcP2;

        /// <summary>One of the polynomial Q(x) used for the rational approximation of Erfc(x).
        /// </summary>
        private IRealPolynomial m_ErfcQ2;

        /// <summary>One of the polynomial P(x) used for the rational approximation of Erfc(x).
        /// </summary>        
        private IRealPolynomial m_ErfcP3;

        /// <summary>One of the polynomial Q(x) used for the rational approximation of Erfc(x).
        /// </summary>
        private IRealPolynomial m_ErfcQ3;

        /// <summary>Contains the values e^{k^2} for k = 0,1,2,... up to machine precision
        /// </summary>
        private double[] m_PowerOfE2 ={
               1,                          //  e^(-0^2)
               0.367879441171442321,       // e^(-1^2)
               1.83156388887341802e-2,     //  e^(-2^2)
               1.23409804086679549e-4,   
               1.12535174719259114e-7,
               1.38879438649640205e-11,
               2.319522830243569388e-16,
               5.24288566336346393e-22,
               1.60381089054863785e-28,
               6.63967719958073440e-36,
               3.72007597602083596e-44,
               2.82077008846013540e-53,
               2.89464031164830028e-63,
               4.02006021574335524e-74,
               7.55581901971196035e-86,
               1.92194772782384906e-98,
               6.61626105670948526e-112,
               3.08244069694909841e-126,
               1.94351485004929273e-141,
               1.65841047768114514e-157,
               1.91516959671400569e-174,
               2.99318445226019273e-192,
               6.33097733621059136e-211,
               1.81225402579399232e-230,
               7.02066779850473471e-251,
               3.68085585480180060e-272,
               2.61174176128405547e-294,   // e^(-26^2)
               4.62639185846956420e-298};  // e^(-27^2)*2^64                                
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="ErrorFunctionCoarseAlgorithm" /> class.
        /// </summary>
        public ErrorFunctionCoarseAlgorithm()
        {
            m_ErfP = Polynomial.Real.Create(1.12837916709551256, 1.35894887627277916e-1, 4.03259488531795274e-2, 1.20339380863079457e-3, 6.49254556481904354e-5);
            m_ErfQ = Polynomial.Real.Create(1, 4.53767041780002545e-1, 8.69936222615385890e-2, 8.49717371168693357e-3, 3.64915280629351082e-4);

            m_ErfcP1 = Polynomial.Real.Create(9.99999992049799098e-1, 1.33154163936765307, 8.78115804155881782e-1, 3.31899559578213215e-1, 7.14193832506776067e-2, 7.06940843763253131e-3);
            m_ErfcQ1 = Polynomial.Real.Create(1, 2.45992070144245533, 2.65383972869775752, 1.61876655543871376, 5.94651311286481502e-1, 1.26579413030177940e-1, 1.25304936549413393e-2);


            m_ErfcP2 = Polynomial.Real.Create(9.99921140009714409e-1, 1.62356584489366647, 1.26739901455873222, 5.81528574177741135e-1, 1.57289620742838702e-1, 2.25716982919217555e-2);
            m_ErfcQ2 = Polynomial.Real.Create(1, 2.75143870676376208, 3.37367334657284535, 2.38574194785344389, 1.05074004614827206, 2.78788439273628983e-1, 4.00072964526861362e-2);

            m_ErfcP3 = Polynomial.Real.Create(5.64189583547756078e-1, 8.80253746105525775, 3.84683103716117320e+1, 4.77209965874436377e+1, 8.08040729052301677);
            m_ErfcQ3 = Polynomial.Real.Create(1, 1.61020914205869003e+1, 7.54843505665954743e+1, 1.12123870801026015e+2, 3.73997570145040850e+1);
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

            if (Math.Sign(x) < 0)
            {
                return -Erf(-x);
            }
            return Erf(x);
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

            if (x >= m_AreaErfc[4])  // 26.5432
            {
                return 0.0; // underflow because precision not satisfied, but here, we do not throw an exception
            }
            if (Math.Sign(x) < 0)
            {
                return 1 + Erf(-x);
            }
            return Erfc(x);
        }
        #endregion

        #region private methods

        /// <summary>Gets a specific value of the error function erf(x) = 2/\sqrt{PI} * \int_0^x e^{-t^2} dt.
        /// </summary>
        /// <param name="x">The argument; greater than or equal 0.0.</param>
        /// <returns>The specified value of the error function erf(x) = 2/\sqrt{PI} * \int_0^x e^{-t^2} dt.</returns>
        private double Erf(double x)
        {
            if (x >= m_AreaErfc[1])
            {
                return 1.0 - Erfc(x);
            }
            /* x \in [0.0, 0.65] */
            int index = Array.BinarySearch(m_LowerAreaErf, x);  // take nearest point on the left
            if (index < 0)
            {
                index = Math.Max(0, ~index - 1);
            }
            switch (index)
            {
                case 0:
                    if (Math.Sign(x) == 0)
                    {
                        return 0.0;
                    }
                    return 0.0;  // if called from GetValue(x): underflow because precision not satisfied, but here, we do not throw an exception

                case 1:
                    return x * m_ErfP.GetCoefficient(0);

                case 2:
                    var x2 = x * x;
                    return x * m_ErfP.GetValue(x2) / m_ErfQ.GetValue(x2);

                default:
                    throw new Exception();
            }
        }

        /// <summary>Gets a specific value of the complementary error function erfc(x) = 1- erf(x) = 2/\sqrt{PI} * \int_x^\infty e^{-t^2} dt.
        /// </summary>
        /// <param name="x">The argument; greater than or equal 0.0.</param>
        /// <returns>The specified value of the complementary error function erfc(x) = 1- erf(x) = 2/\sqrt{PI} * \int_x^\infty e^{-t^2} dt.</returns>
        private double Erfc(double x)
        {
            int index = Array.BinarySearch(m_AreaErfc, x);  // take nearest point on the left
            if (index < 0)
            {
                index = Math.Max(0, ~index - 1);
            }
            switch (index)
            {
                case 0: /* x \in [0.0, 0.65] */
                    return 1.0 - Erf(x);

                case 1: /* x \in [0.65, 2.2] */
                    return ExpOfMinusXSquare(x) * m_ErfcP1.GetValue(x) / m_ErfcQ1.GetValue(x);

                case 2: /* x \in [2.2, 6.0] */
                    return ExpOfMinusXSquare(x) * m_ErfcP2.GetValue(x) / m_ErfcQ2.GetValue(x);

                case 3: /* x \in [6.0, 26.5432] */
                    var y = 1.0 / (x * x);
                    return (ExpOfMinusXSquare(x) * m_ErfcP3.GetValue(y)) / (x * m_ErfcQ3.GetValue(y));

                case 4: /* x >= 26.5432 */
                case 5:
                    return 0.0;

                default: throw new NotImplementedException();
            }
        }

        /// <summary>Gets exp(-x*x) with low relative error.
        /// </summary>
        /// <param name="x">The argument x.</param>
        /// <returns>The value of exp(-x*x) with low relative error.</returns>
        private double ExpOfMinusXSquare(double x)
        {
            if (Math.Sign(x) < 0)
            {
                x = -x;
            }
            int z = (int)Math.Truncate(x);
            var m = x - z;

            if (m > 0.5)
            {
                z++;
                m--;
            }
            var t = m_PowerOfE2[z] * Math.Exp(-(z + z) * m) * Math.Exp(-m * m);
            if (z >= 27)
            {
                return t * Math.Pow(2, -64); // In the XSC-implementation a rounding to the nearest 'grid number' was used, but .net does not (directly) supported the access to mantisse and exponent component of a real number
            }
            return t;
        }
        #endregion
    }
}