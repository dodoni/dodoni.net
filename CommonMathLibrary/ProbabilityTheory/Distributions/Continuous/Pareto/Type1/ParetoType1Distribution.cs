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
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.ProbabilityTheory.Distributions
{
    /// <summary>Represents the Pareto distribution (Type I), i.e. the distribution function is specified by F(x) = 1 - (x_0/x)^\alpha for x >= x_0 and \alpha > 0.0.
    /// </summary>
    public class ParetoType1Distribution : IProbabilityDistribution
    {
        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="ParetoType1Distribution" /> class.
        /// </summary>
        /// <param name="alpha">The parameter \alpha of the Pareto distribution.</param>
        /// <param name="xMin">The parameter \xMin, i.e. the infimum of the support of the distribution.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if one of the argument is (strictly) less than 0.0.</exception>
        public ParetoType1Distribution(double alpha, double xMin)
        {
            if (alpha <= 0.0)
            {
                throw new ArgumentOutOfRangeException("alpha");
            }
            Alpha = alpha;
            if (xMin < 0.0)
            {
                throw new ArgumentOutOfRangeException("xMin");
            }
            XMin = xMin;
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the current instance.
        /// </summary>
        /// <value>The language independent name of the current instance.</value>
        public IdentifierString Name
        {
            get;
            private set;
        }

        /// <summary>Gets the long name of the current instance.
        /// </summary>
        /// <value>The (perhaps) language dependent long name of the current instance.</value>
        public IdentifierString LongName
        {
            get;
            private set;
        }
        #endregion

        #region IInfoOutputQueriable Members

        /// <summary>Gets the info-output level of detail.
        /// </summary>
        /// <value>The info-output level of detail.</value>
        public InfoOutputDetailLevel InfoOutputDetailLevel
        {
            get;
            private set;
        }
        #endregion

        #region IProbabilityDistribution Members

        /// <summary>Gets the infimum of the support of the distribution.
        /// </summary>
        /// <value>The infimum of the support of the distribution.</value>
        public double Infimum
        {
            get { return XMin; }
        }

        /// <summary>Gets the supremum of the support of the distribution.
        /// </summary>
        /// <value>The supremum of the support of the distribution.</value>
        public double Supremum
        {
            get { return Double.PositiveInfinity; }
        }

        /// <summary>Gets the expectation, i.e. the first moment, or <see cref="System.Double.NaN" /> if the first moment does not exist.
        /// </summary>
        /// <value>The expectation, i.e. the first moment, or <see cref="System.Double.NaN" /> if the first moment does not exist.</value>
        public double Expectation
        {
            get { return (Alpha > 1) ? Alpha * XMin / (Alpha - 1) : Double.PositiveInfinity; }
        }

        /// <summary>Gets the variance, i.e. the second central moment, or <see cref="System.Double.NaN" /> if the second central moment does not exit.
        /// </summary>
        /// <value>The variance, i.e. the second central moment, or <see cref="System.Double.NaN" /> if the second central moment does not exit.</value>
        public double Variance
        {
            get
            {
                if ((Alpha > 1) && (Alpha <= 2))
                {
                    return Double.PositiveInfinity;
                }
                else if (Alpha > 2)
                {
                    return XMin * XMin * Alpha / ((Alpha - 1) * (Alpha - 1) * (Alpha - 2));
                }
                return Double.NaN;
            }
        }

        public ProbabilityDistributionMoments Moment
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>Gets the median.
        /// </summary>
        /// <value>The median.</value>
        public double Median
        {
            get { return XMin * Math.Pow(2.0, 1.0 / Alpha); }
        }
        #endregion

        /// <summary>Gets the value of parameter xMin, i.e. the minimum of the support of the distribution; this distribution is left-bounded.
        /// </summary>
        /// <value>The value of parameter xMin, i.e. the minimum of the support of the distribution; this distribution is left-bounded..</value>
        public double XMin
        {
            get;
            private set;
        }

        /// <summary>Gets the value of parameter \alpha.
        /// </summary>
        /// <value>The value of parameter \alpha.</value>
        public double Alpha
        {
            get;
            private set;
        }
        #endregion

        #region public methods

        #region IProbabilityDistribution Members

        /// <summary>Gets a specific value of the cummulative distribution function.
        /// </summary>
        /// <param name="x">The value where to evaluate.</param>
        /// <returns>The specified value of the cummulative distribution function.</returns>
        public double GetCdfValue(double x)
        {
            if (x <= XMin)
            {
                return 0.0;
            }
            return 1.0 - Math.Pow(XMin / x, Alpha);
        }

        /// <summary>Gets a specific value of the inverse of the cummulative distribution function.
        /// </summary>
        /// <param name="probability">The probability where to evaluate.</param>
        /// <returns>The specified value of the inverse of the cummulative distribution function.</returns>
        public double GetInverseCdfValue(double probability)
        {
            if (probability <= 0.0)
            {
                return XMin;
            }
            else if (probability >= 1.0)
            {
                return Double.PositiveInfinity;
            }
            return XMin / Math.Exp(Math.Log(1.0 - probability) / Alpha);
        }

        /// <summary>Gets a specific value of the probability density function.
        /// </summary>
        /// <param name="x">The point where to evaluate.</param>
        /// <returns>The specified value of the probability density function.</returns>
        public double GetPdfValue(double x)
        {
            if (x <= XMin)
            {
                return 0.0;
            }
            return Alpha * Math.Pow(XMin, Alpha) / Math.Pow(x, Alpha + 1);
        }

        public Complex GetChfValue(double t)
        {
            Complex gamma = 0.0; // = Gamma(-alpha, -ixMin*t) incomplete gamma
            
            return Alpha * Complex.Pow(-Complex.ImaginaryOne * XMin * t, Alpha) * gamma;
            
        }
        #endregion

        #region IInfoOutputQueriable Members

        public bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
        {
            throw new NotImplementedException();
        }

        public void FillInfoOutput(InfoOutput infoOutput, string categoryName = InfoOutput.GeneralCategoryName)
        {

        }
        #endregion

        #endregion

        #region IProbabilityDistribution Members


        public void GetCdfValue(int n, double[] a, double[] y)
        {
            throw new NotImplementedException();
        }

        public void GetInverseCdfValue(int n, double[] a, double[] y)
        {
            throw new NotImplementedException();
        }

        public void GetPdfValue(int n, double[] a, double[] y)
        {
            throw new NotImplementedException();
        }

        public void GetChfValue(int n, double[] a, Complex[] y)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}