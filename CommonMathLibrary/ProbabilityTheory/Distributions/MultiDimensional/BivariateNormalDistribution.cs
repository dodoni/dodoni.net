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
using System.Collections.Generic;

using Dodoni.MathLibrary.Basics;

namespace Dodoni.MathLibrary.ProbabilityTheory.Distributions
{
    /// <summary>Represents the bivariate normal distribution and provides corresponding methods
    /// for the cummulative distribution function.
    /// </summary>
    public class BivariateNormalDistribution
    {
        #region private nested types

        /// <summary>The Gauss-Legendre interation rule.
        /// </summary>
        private enum eGaussLegendreRule
        {
            /// <summary>Use a 6-point Gauss-Legendre integration rule.
            /// </summary>
            GaussLegendre6 = 0,

            /// <summary>Use a 12-point Gauss-Legendre integration rule.
            /// </summary>
            GaussLegendre12 = 1,

            /// <summary>Use a 20-point Gauss-Legendre integration rule.
            /// </summary>
            GaussLegendre20 = 2
        }

        #endregion

        #region private static members

        /// <summary>Stores for each suitable Gauss-Legendre integration rule the evaluation points.
        /// </summary>
        private static readonly List<double[]> sm_GaussEvaluationPoints = new List<double[]>();

        /// <summary>Stores for each suitable Gauss-Legendre integration rule the weights.
        /// </summary>
        private static readonly List<double[]> sm_GaussWeights = new List<double[]>();
        #endregion

        #region private members

        /// <summary>The parameter \mu_x, i.e. a component of the mean (\mu_x,\mu_y).
        /// </summary>
        private double m_MuX;

        /// <summary>The parameter \mu_y, i.e. a component of the mean (\mu_x,\mu_y).
        /// </summary>
        private double m_MuY;

        /// <summary>The variance of the first component (=\sigma_x).
        /// </summary>
        private double m_SigmaX;

        /// <summary>The variance of the second component (\sigma_y).
        /// </summary>
        private double m_SigmaY;

        /// <summary>The correlations coefficient (=\rho).
        /// </summary>
        private double m_Rho;
        #endregion

        #region public constructors

        #region static constructor

        /// <summary>Initializes the <see cref="BivariateNormalDistribution"/> class.
        /// </summary>
        static BivariateNormalDistribution()
        {
            /* Gauss-Legendre 6: */
            sm_GaussWeights.Add(new double[] { 0.1713244923791705, 
                                                0.3607615730481384, 
                                                0.4679139345726904 });

            sm_GaussEvaluationPoints.Add(new double[] { -0.9324695142031522, 
                                                -0.6612093864662647, 
                                                -0.2386191860831970 });

            /* Gauss-Legendre 12 */
            sm_GaussWeights.Add(new double[] { 0.4717533638651177E-1,
                                                0.1069393259953183, 
                                                0.1600783285433464,
                                                0.2031674267230659, 
                                                0.2334925365383547, 
                                                0.2491470458134029});

            sm_GaussEvaluationPoints.Add(new double[] {-0.9815606342467191,
                                               -0.9041172563704750,
                                               -0.7699026741943050,
                                               -0.5873179542866171,
                                               -0.3678314989981802,
                                               -0.1252334085114692});
            /* Gauss-Legendre 20 */
            sm_GaussWeights.Add(new double[] {0.1761400713915212E-01,
                                               0.4060142980038694E-01,
                                               0.6267204833410906E-01,
                                               0.8327674157670475E-01,
                                               0.1019301198172404,
                                               0.1181945319615184,
                                               0.1316886384491766,
                                               0.1420961093183821,
                                               0.1491729864726037,
                                               0.1527533871307259});

            sm_GaussEvaluationPoints.Add(new double[] {-0.9931285991850949,
                                               -0.9639719272779138,
                                               -0.9122344282513259,
                                               -0.8391169718222188,
                                               -0.7463319064601508,
                                               -0.6360536807265150,
                                               -0.5108670019508271,
                                               -0.3737060887154196,
                                               -0.2277858511416451,
                                               -0.7652652113349733E-1});
        }
        #endregion

        /// <summary>Initializes a new instance of the <see cref="BivariateNormalDistribution"/> class.
        /// </summary>
        /// <param name="rho">The correlation coefficient (=\rho).</param>
        /// <remarks>The mean is set to (0.0, 0.0) and <see cref="SigmaX"/> and <see cref="SigmaY"/> are set to <c>1.0</c>.</remarks>
        public BivariateNormalDistribution(double rho)
        {
            m_MuX = 0.0;
            m_MuY = 0.0;
            m_SigmaX = 1.0;
            m_SigmaY = 1.0;
            m_Rho = rho;
        }

        /// <summary>Initializes a new instance of the <see cref="BivariateNormalDistribution"/> class.
        /// </summary>
        /// <param name="rho">The correlation coefficient (=\rho).</param>
        /// <param name="sigmaX">The variance of the first component (=\sigma_x).</param>
        /// <param name="sigmaY">The variance of the second component (=\sigma_y).</param>
        /// <param name="muX">The parameter \mu_x, i.e. a component of the mean (\mu_x, \mu_y).</param>
        /// <param name="muY">The paramter \mu_y, i.e. a component of the mean (\mu_x, \mu_y).</param>
        public BivariateNormalDistribution(double rho, double sigmaX, double sigmaY, double muX, double muY)
        {
            m_MuX = muX;
            m_MuY = muY;
            m_SigmaX = sigmaX;
            m_SigmaY = sigmaY;
            m_Rho = rho;
        }

        /// <summary>Initializes a new instance of the <see cref="BivariateNormalDistribution"/> class.
        /// </summary>
        /// <remarks><see cref="Rho"/> is set to 0.0 and the mean is set to (0.0, 0.0). Furthermore <see cref="SigmaX"/> and <see cref="SigmaY"/>
        /// are set to <c>1.0</c>.</remarks>
        public BivariateNormalDistribution()
            : this(0.0)
        {
        }
        #endregion

        #region public properties

        /// <summary>Gets or sets the variance of the first component ( = \sigma_x).
        /// </summary>
        /// <value>The sigma X.</value>
        public double SigmaX
        {
            get { return m_SigmaX; }
            set
            {
                if (value > 0.0)
                {
                    m_SigmaX = value;
                }
            }
        }

        /// <summary>Gets or sets the variance of the second component (=\sigma_y).
        /// </summary>
        /// <value>The sigma Y.</value>
        public double SigmaY
        {
            get { return m_SigmaY; }
            set
            {
                if (value > 0.0)
                {
                    m_SigmaY = value;
                }
            }
        }

        /// <summary>Gets or sets the correlation coefficient (=\rho).
        /// </summary>
        /// <value>The correlation coefficient.</value>
        public double Rho
        {
            get { return m_Rho; }
            set
            {
                if (value > 0.0)
                {
                    m_Rho = value;
                }
            }
        }

        /// <summary>Gets or sets the mean of the first component (=\mu_x).
        /// </summary>
        /// <value>The paramter \mu_x.</value>
        public double MuX
        {
            get { return m_MuX; }
            set { m_MuX = value; }
        }

        /// <summary>Gets or sets the mean of the second component (=\mu_y).
        /// </summary>
        /// <value>The paramter \mu_y.</value>
        public double MuY
        {
            get { return m_MuY; }
            set { m_MuY = value; }
        }

        /// <summary>Gets the correlation matrix.
        /// </summary>
        /// <value>The correlation matrix.</value>
        /// <remarks>The correlation matrix is given by
        /// <para>
        ///            [\sigma_x^2                          \rho \cdot \sigma_x \cdot \sigma_y ]
        /// \Sigma =   |                                                                       |
        ///            [\rho \cdot \sigma_x \cdot \sigma_y                \sigma_y^2           ]
        /// </para>
        /// </remarks>
        public double[,] CorrelationMatrix
        {
            get
            {
                double[,] correlationMatrix = new double[2, 2];
                correlationMatrix[0, 0] = m_SigmaX * m_SigmaX;
                correlationMatrix[0, 1] = correlationMatrix[1, 0] = m_Rho * m_SigmaX * m_SigmaY;
                correlationMatrix[1, 1] = m_SigmaY * m_SigmaY;
                return correlationMatrix;
            }
        }
        #endregion

        #region public methods

        /// <summary>Evaluate the cummulative distribution function of a bivariate normal random
        /// variable at a given point
        /// </summary>
        /// <param name="a">The first part of the evaluation point.</param>
        /// <param name="b">The second part of the evaluation point.</param>
        /// <returns>A <see cref="System.Double"/> reflects to the value of the cummulative distribution function.</returns>
        /// <remarks>This method calls <see cref="GetStandardCdfValue(double, double, double)"/>.</remarks>
        public double CDFValue(double a, double b)
        {
            return BivariateNormalDistribution.StandardCDFValue((a - m_MuX) / m_SigmaX, (b - m_MuY) / m_SigmaY, m_Rho);
        }
        #endregion

        #region static methods

        /// <summary>Evaluate the cummulative distribution function of a bivariate normal random variable 
        /// at a given point.
        /// </summary>
        /// <param name="a">The first part of the evaluation point.</param>
        /// <param name="b">The second part of the evaluation point.</param>
        /// <param name="rho">The correlation coefficient.</param>
        /// <returns>Returns the value of the double-integral
        /// <para>
        ///   N_2(a,b,\rho) = \frac{1}{ 2\pi \sqrt{1-\rho^2}} * \int_{-\infty}^a \int_{-\infty}^b exp\bigl[ - \frac{ x^2 - 2 \rho xy +y^2}{2 (1-\rho^2)} \bigr] dy dx.
        /// </para></returns>
        /// <remarks>This function is based on the method described by 
        /// Drezner, Z and G.O. Wesolowsky, (1989), On the computation of the bivariate normal integral,
        /// Journal of Statist. Comput. Simul. 35, pp. 101-107,    
        /// with major modifications for double precision, and for |R| close to 1.
        /// This implementation is based on the Fortran function BVND of A. Genz,
        /// http://www.sci.wsu.edu/math/faculty/genz/homepage.</remarks>
        public static double StandardCDFValue(double a, double b, double rho)
        {
            double cdfValue = 0.0;
            double absOfRho = Math.Abs(rho);

            int gaussLegendreRule;

            if (absOfRho < 0.3)
            {
                gaussLegendreRule = (int)eGaussLegendreRule.GaussLegendre6;
            }
            else if (absOfRho < 0.75)
            {
                gaussLegendreRule = (int)eGaussLegendreRule.GaussLegendre12;
            }
            else
            {
                gaussLegendreRule = (int)eGaussLegendreRule.GaussLegendre20;
            }
            int numberOfEvaluations = sm_GaussWeights[gaussLegendreRule].Length;
            double[] gaussLegendreWeights = sm_GaussWeights[gaussLegendreRule];
            double[] gaussLegendreEvaluationPoints = sm_GaussEvaluationPoints[gaussLegendreRule];

            double aTimesb = a * b;
            if (absOfRho < 0.925)
            {
                if (absOfRho > 0)
                {
                    double halfOfaSquarePlusbSquare = (a * a + b * b) / 2;
                    double rhoAsin = Math.Asin(rho);
                    for (int i = 0; i < numberOfEvaluations; i++)
                    {
                        for (int j = -1; j <= 2; j += 2)
                        {
                            double tempValue = Math.Sin(rhoAsin * (j * gaussLegendreEvaluationPoints[i] + 1) / 2);
                            cdfValue = cdfValue + gaussLegendreWeights[i] * Math.Exp((tempValue * aTimesb - halfOfaSquarePlusbSquare) / (1 - tempValue * tempValue));
                        }
                    }
                    cdfValue *= rhoAsin / (2 * MathConsts.TwoPi);
                }
                return cdfValue + StandardNormalDistribution.GetCdfValue(a) * StandardNormalDistribution.GetCdfValue(b);
            }
            else
            {
                if (rho < 0)
                {
                    aTimesb = -aTimesb;
                }
                if (absOfRho < 1)
                {
                    double AS = (1 - rho) * (1 + rho);
                    double A = Math.Sqrt(AS);
                    double BS = (a - b) * (a - b);
                    double C = (4 - aTimesb) / 8;
                    double D = (12 - aTimesb) / 16;
                    double ASR = -(BS / AS + aTimesb) / 2;
                    if (ASR > -100)
                    {
                        cdfValue = A * Math.Exp(ASR) * (1 - C * (BS - AS) * (1 - D * BS / 5) / 3 + C * D * AS * AS / 5);
                    }
                    if (-aTimesb < 100)
                    {
                        double B = Math.Sqrt(BS);
                        cdfValue = cdfValue - Math.Exp(-aTimesb / 2) * MathConsts.SqrtTwoPi * StandardNormalDistribution.GetCdfValue(-B / A) * B * (1 - C * BS * (1 - D * BS / 5) / 3);
                    }
                    A = A / 2;
                    for (int i = 0; i < numberOfEvaluations; i++)
                    {
                        for (int j = -1; j <= 2; j += 2)
                        {
                            double XS = (A * (j * gaussLegendreEvaluationPoints[i] + 1)) * (A * (j * gaussLegendreEvaluationPoints[i] + 1));
                            double RS = Math.Sqrt(1 - XS);
                            ASR = -(BS / XS + aTimesb) / 2;
                            if (ASR > -100)
                            {
                                cdfValue = cdfValue + A * gaussLegendreWeights[i] * Math.Exp(ASR) * (Math.Exp(-aTimesb * (1 - RS) / (2 * (1 + RS))) / RS - (1 + C * XS * (1 + D * XS)));
                            }
                        }
                    }
                    cdfValue = -cdfValue / MathConsts.TwoPi;
                }
                if (rho > 0)
                {
                    cdfValue = cdfValue + StandardNormalDistribution.GetCdfValue(Math.Min(a, b));
                }
                else
                {
                    cdfValue = -cdfValue;
                    if (b < a)
                    {
                        cdfValue = cdfValue + StandardNormalDistribution.GetCdfValue(-b) - StandardNormalDistribution.GetCdfValue(-a);
                    }
                }
            }
            return cdfValue;
        }
        #endregion
    }
}