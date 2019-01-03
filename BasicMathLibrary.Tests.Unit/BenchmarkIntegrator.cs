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

namespace Dodoni.MathLibrary
{
    /// <summary>Serves as a simple numerical integrator used for some unit tests.
    /// </summary>
    /// <remarks>Based on W. H. Press, § 4.3 'Numerical Recipes' but also Jim Lambers, 'Lecture 13 Notes, Romberg Integration'.
    /// </remarks>
    public class BenchmarkIntegrator
    {
        #region private members

        private const int NumberOfIterations = 12;

        /// <summary>A workspace array for the calculation.
        /// </summary>
        private double[] m_CurrentTempValue = new double[NumberOfIterations];

        /// <summary>A workspace array for the calculation.
        /// </summary>
        private double[] m_NextTempValue = new double[NumberOfIterations];
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="BenchmarkIntegrator" /> class.
        /// </summary>
        public BenchmarkIntegrator()
        {
        }
        #endregion

        #region public properties

        /// <summary>Gets or sets the function to integrate.
        /// </summary>
        /// <value>The function to integrate.</value>
        public Func<double, double> FunctionToIntegrate
        {
            get;
            set;
        }
        #endregion

        #region public methods

        /// <summary>Gets the value of the integral  \int_a^b w(x) * f(x) dx.
        /// </summary>
        /// <returns>An approximation of the specific integral.</returns>
        public double GetValue(double lowerBound, double upperBound)
        {
            double intervalLength = upperBound - lowerBound;

            long M = 1;

            for (int j = 0; j < NumberOfIterations; j++)
            {
                /* set 'nextT[0] = T_{j,0}' to the result of the composite trapezoidal rule of degree M: */
                if (j == 0)
                {
                    m_NextTempValue[0] = intervalLength / 2.0 * (FunctionToIntegrate(lowerBound) + FunctionToIntegrate(upperBound));
                }
                else
                {
                    m_CurrentTempValue[j] = m_NextTempValue[j - 1];

                    M <<= 1; // M = 2^{j}

                    double h = intervalLength / M;
                    double x = lowerBound + h;

                    double subTrapezValueRefinement = 0.0;
                    for (int i = 1; i <= M / 2; i++)
                    {
                        subTrapezValueRefinement += FunctionToIntegrate(x);
                        x += 2.0 * h;
                    }
                    m_NextTempValue[0] = 0.5 * m_NextTempValue[0] + h * subTrapezValueRefinement;

                    /* now compute T_{j,k}, k =1,...j */
                    long denominator = 1;
                    for (int k = 1; k <= j; k++)
                    {
                        denominator <<= 2;  // = 4^k

                        m_NextTempValue[k] = m_NextTempValue[k - 1] + (m_NextTempValue[k - 1] - m_CurrentTempValue[k - 1]) / (denominator - 1);
                        m_CurrentTempValue[k - 1] = m_NextTempValue[k - 1];
                    }
                }
            }
            return m_NextTempValue[NumberOfIterations - 1];
        }
        #endregion
    }
}