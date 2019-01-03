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

namespace Dodoni.MathLibrary.ProbabilityTheory.Distributions
{
    public partial class StandardNormalDistributionTests
    {
        /// <summary>Serves as Benchmark implementation for the cummulative standard Normal distribution function.
        /// </summary>
        /// <remarks>The implementation is based on
        /// <para>J.L. Schonfelder, Math Comp 32(1978), pp 1232-1240 and the Fortran implementation PHID(Z) of Alan Genz, see http://www.sci.wsu.edu/math/faculty/genz/homepage.
        /// </para></remarks>
        public static class Benchmark
        {
            #region private (static) members

            /// <summary>The coefficients for the implementation of cummulative standard normal distribution function (cdf), based on A. Genz.
            /// </summary>
            private static readonly double[] sm_GenzCDFCoefficients = {
               6.10143081923200417926465815756E-1,
              -4.34841272712577471828182820888E-1,
               1.76351193643605501125840298123E-1,
              -6.0710795609249414860051215825E-2,
               1.7712068995694114486147141191E-2,
              -4.321119385567293818599864968E-3,
               8.54216676887098678819832055E-4,
              -1.27155090609162742628893940E-4,
               1.1248167243671189468847072E-5,
               3.13063885421820972630152E-7,
              -2.70988068537762022009086E-7, 
               3.0737622701407688440959E-8,
               2.515620384817622937314E-9, 
              -1.028929921320319127590E-9,
               2.9944052119949939363E-11, 
               2.6051789687266936290E-11,
              -2.634839924171969386E-12, 
              -6.43404509890636443E-13,
               1.12457401801663447E-13, 
               1.7281533389986098E-14, 
              -4.264101694942375E-15, 
              -5.45371977880191E-16,
               1.58697607761671E-16, 
               2.0899837844334E-17, 
              -5.900526869409E-18, 
              -9.41893387554E-19, 
               2.14977356470E-19, 
               4.6660985008E-20, 
              -7.243011862E-21, 
              -2.387966824E-21, 
               1.91177535E-22, 
               1.20482568E-22, 
              -6.72377E-25, 
              -5.747997E-24,
              -4.28493E-25, 
               2.44856E-25, 
               4.3793E-26, 
              -8.151E-27, 
              -3.089E-27, 
               9.3E-29, 
               1.74E-28, 
               1.6E-29, 
               -8.0E-30, 
               -2.0E-30 };
            #endregion

            #region public static methods

            /// <summary>Evaluate the cummulative distribution function of a standard normal random variable at a specific point.
            /// </summary>
            /// <param name="x">The value where to evaluate.</param>
            /// <returns>A <see cref="System.Double"/> which reflects the value of the cummulative distribution function.</returns>
            public static double GetStandardCdfValue(double x)
            {
                double cdfValue = 0.0;
                double xScaled = Math.Abs(x) / MathConsts.Sqrt2;

                if (xScaled <= 100)
                {
                    double t = (8 * xScaled - 30) / (4 * xScaled + 15);
                    double bM = 0;
                    double b = 0;
                    double bP = 0.0;
                    for (int i = sm_GenzCDFCoefficients.Length - 1; i >= 0; i--)
                    {
                        bP = b;
                        b = bM;
                        bM = t * b - bP + sm_GenzCDFCoefficients[i];
                    }
                    cdfValue = Math.Exp(-xScaled * xScaled) * (bM - bP) / 4;
                }
                if (x > 0)
                {
                    cdfValue = 1 - cdfValue;
                }
                return cdfValue;
            }
            #endregion
        }
    }
}