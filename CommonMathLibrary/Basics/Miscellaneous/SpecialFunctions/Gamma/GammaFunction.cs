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
    /// <summary>Represents the implementation of the [incomplete] gamma function, i.e. \int e^{-t} * t^{a-1} dt.
    /// </summary>
    /// <remarks>The implementation is based on 
    /// <para>"An Analysis Of The Lanczos Gamma Approximation", Glendon Ralph Pugh, 2004, p. 116.</para></remarks>
    public partial class GammaFunction
    {
        #region private members

        /// <summary>The value of 'r' in the calculation of the gamma function, based on "An Analysis Of The Lanczos Gamma Approximation", Glendon Ralph Pugh, 2004, p. 116.
        /// </summary>
        private const double m_GammaR = 10.900511;

        /// <summary>The coefficients 'd_k' for the calculation of the gamma function, based on "An Analysis Of The Lanczos Gamma Approximation", Glendon Ralph Pugh, 2004, p. 116.
        /// </summary>
        private static double[] m_GammaCoefficients =
        {
            2.48574089138753565546e-5,
            1.05142378581721974210,
            -3.45687097222016235469,
            4.51227709466894823700,
            -2.98285225323576655721,
            1.05639711577126713077,
            -1.95428773191645869583e-1,
            1.70970543404441224307e-2,
            -5.71926117404305781283e-4,
            4.63399473359905636708e-6,
            -2.71994908488607703910e-9
        };
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="GammaFunction"/> class.
        /// </summary>
        public GammaFunction()
        {
        }
        #endregion

        #region public methods

        /// <summary>Returns the logarithm of \Gamma(x), where \Gamma is the gamma function.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <returns>The logarithm of the Gamma function at <paramref name="x"/>.</returns>
        /// <remarks>
        /// <para>This implementation is based on
        ///     "An Analysis Of The Lanczos Gamma Approximation", Glendon Ralph Pugh, 2004, p. 116.
        ///  </para>
        ///  The implementation on page 126 in the reference gives worse results. For example \Gamma(14) = 13! gives an even worse result than the algorithm presented 
        ///  in the "Numerical Recipies" (§6.1) which is used as a benchmark in the unit tests.
        /// </remarks>
        public double GetLogValue(double x)
        {
            if (x < 0.5)  // apply Reflection formula, i.e. \Gamma(z+1) * \Gamma(1-z) = PI * z / sin(Pi * z), and \Gamma(z+1) = z * \Gamma(z)
            {
                double sum = m_GammaCoefficients[0];
                for (int i = 1; i < m_GammaCoefficients.Length; i++)
                {
                    sum += m_GammaCoefficients[i] / (i - x);
                }
                return MathConsts.LogPi - Math.Log(Math.Sin(Math.PI * x)) - Math.Log(sum) - MathConsts.LogTwoSqrtEOverPi - ((0.5 - x) * Math.Log((0.5 - x + m_GammaR) / Math.E));
            }
            else
            {
                double sum = m_GammaCoefficients[0];
                for (int i = 1; i < m_GammaCoefficients.Length; i++)
                {
                    sum += m_GammaCoefficients[i] / (x + i - 1.0);
                }
                return Math.Log(sum) + MathConsts.LogTwoSqrtEOverPi + ((x - 0.5) * Math.Log((x - 0.5 + m_GammaR) / Math.E));
            }
        }

        /// <summary>Returns the value of the gamma function at a specific point.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <returns>The value of the Gamma function at <paramref name="x"/>.</returns>
        /// <remarks>
        /// <para>This implementation is based on
        ///     "An Analysis Of The Lanczos Gamma Approximation", Glendon Ralph Pugh, 2004, p. 116.
        ///  </para>
        ///  The implementation on page 126 in the reference gives worse results. For example \Gamma(14) = 13! gives an even worse result than the algorithm presented 
        ///  in the "Numerical Recipies" (§6.1) which is used as a benchmark in the unit tests.
        /// </remarks>
        public double GetValue(double x)
        {
            if (x < 0.5)  // apply Reflection formula, i.e. \Gamma(z+1) * \Gamma(1-z) = PI * z / sin(Pi * z), and \Gamma(z+1) = z * \Gamma(z)
            {
                double sum = m_GammaCoefficients[0];
                for (int i = 1; i < m_GammaCoefficients.Length; i++)
                {
                    sum += m_GammaCoefficients[i] / (i - x);
                }
                return Math.PI / (Math.Sin(Math.PI * x) * MathConsts.TwoSqrtEOverPi * Math.Pow((0.5 - x + m_GammaR) / Math.E, 0.5 - x) * sum);
            }
            else
            {
                double sum = m_GammaCoefficients[0];
                for (int i = 1; i < m_GammaCoefficients.Length; i++)
                {
                    sum += m_GammaCoefficients[i] / (x + i - 1.0);
                }
                return MathConsts.TwoSqrtEOverPi * Math.Pow((x - 0.5 + m_GammaR) / Math.E, x - 0.5) * sum;
            }
        }

        /// <summary>Returns the logarithm of \Gamma(x), where \Gamma is the gamma function.
        /// </summary>
        /// <param name="z">The argument.</param>
        /// <returns>The logarithm of the Gamma function at <paramref name="z"/>.</returns>
        /// <remarks>
        /// <para>This implementation is based on
        ///     "An Analysis Of The Lanczos Gamma Approximation", Glendon Ralph Pugh, 2004. We use the implementation listed on p. 116.
        ///  </para>
        ///  The implementation on page 126 in the reference gives worse results. For example \Gamma(14) = 13! gives an even worse result than the algorithm presented 
        ///  in the "Numerical Recipies" (§6.1) which is used as a benchmark in the unit tests.
        /// </remarks>
        public Complex GetLogValue(Complex z)
        {
            if (z.Real < 0.5) // apply Reflection formula, i.e. \Gamma(z+1) * \Gamma(1-z) = PI * z / sin(Pi * z), and \Gamma(z+1) = z * \Gamma(z)
            {
                Complex sum = m_GammaCoefficients[0];
                for (int i = 1; i < m_GammaCoefficients.Length; i++)
                {
                    sum += m_GammaCoefficients[i] / (i - z);
                }
                return MathConsts.LogPi - Complex.Log(Complex.Sin(Math.PI * z)) - Complex.Log(sum) - MathConsts.LogTwoSqrtEOverPi - ((0.5 - z) * Complex.Log((0.5 - z + m_GammaR) / Math.E));
            }
            else
            {
                Complex sum = m_GammaCoefficients[0];
                for (int i = 1; i < m_GammaCoefficients.Length; i++)
                {
                    sum += m_GammaCoefficients[i] / (z + i - 1.0);
                }
                return Complex.Log(sum) + MathConsts.LogTwoSqrtEOverPi + ((z - 0.5) * Complex.Log((z - 0.5 + m_GammaR) / Math.E));
            }
        }

        /// <summary>Returns the value of the gamma function at a specific point.
        /// </summary>
        /// <param name="z">The argument.</param>
        /// <returns>The value of the Gamma function at <paramref name="z"/>.</returns>
        /// <remarks>
        /// <para>This implementation is based on
        ///     "An Analysis Of The Lanczos Gamma Approximation", Glendon Ralph Pugh, 2004. We use the implementation listed on p. 116.
        ///  </para>
        ///  The implementation on page 126 in the reference gives worse results. For example \Gamma(14) = 13! gives an even worse result than the algorithm presented 
        ///  in the "Numerical Recipies" (§6.1) which is used as a benchmark in the unit tests.
        /// </remarks>
        public Complex GetValue(Complex z)
        {
            if (z.Real < 0.5) // apply Reflection formula, i.e. \Gamma(z+1) * \Gamma(1-z) = PI * z / sin(Pi * z), and \Gamma(z+1) = z * \Gamma(z)
            {
                Complex sum = m_GammaCoefficients[0];
                for (int i = 1; i < m_GammaCoefficients.Length; i++)
                {
                    sum += m_GammaCoefficients[i] / (i - z);
                }
                return Math.PI / (Complex.Sin(Math.PI * z) * MathConsts.TwoSqrtEOverPi * Complex.Pow((0.5 - z + m_GammaR) / Math.E, 0.5 - z) * sum);
            }
            else
            {
                Complex sum = m_GammaCoefficients[0];
                for (int i = 1; i < m_GammaCoefficients.Length; i++)
                {
                    sum += m_GammaCoefficients[i] / (z + i - 1.0);
                }
                return MathConsts.TwoSqrtEOverPi * Complex.Pow((z - 0.5 + m_GammaR) / Math.E, z - 0.5) * sum;
            }
        }
        #endregion
    }
}