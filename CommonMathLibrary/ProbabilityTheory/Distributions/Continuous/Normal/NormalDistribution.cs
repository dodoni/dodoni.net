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
using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.ProbabilityTheory.Distributions
{
    /// <summary>Provides methods for cummulative distribution function etc. with respect to the Normal distribution.
    /// </summary>
    public partial class NormalDistribution : IProbabilityDistribution
    {
        #region public static (readonly) members

        /// <summary>The Standard Normal distribution, i.e. N(0,1).
        /// </summary>
        public static readonly NormalDistribution Standard;
        #endregion

        #region private members

        /// <summary>A delegate called if specific data should be stored in <see cref="FillInfoOutput(InfoOutput, string)"/>; perhaps <c>null</c>.
        /// </summary>
        private Action<InfoOutput, string> m_InfoOutputAction;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="NormalDistribution"/> class.
        /// </summary>
        /// <param name="mu">The mean.</param>
        /// <param name="sigma">The standard deviation.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if one of the arguments is not valid.</exception>
        public NormalDistribution(double mu, double sigma)
            : this(mu, sigma, infoOutputAction: null)
        {
        }
        #endregion

        #region internal protected constructors

        /// <summary>Initializes a new instance of the <see cref="NormalDistribution"/> class.
        /// </summary>
        /// <param name="mu">The mean.</param>
        /// <param name="sigma">The standard deviation.</param>
        /// <param name="infoOutputAction">A delegate called if specific data should be stored in <see cref="FillInfoOutput(InfoOutput, string)"/>; perhaps <c>null</c>.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if one of the arguments is not valid.</exception>
        internal protected NormalDistribution(double mu, double sigma, Action<InfoOutput, string> infoOutputAction)
        {
            m_InfoOutputAction = infoOutputAction;

            if (sigma <= 0)
            {
                throw new ArgumentOutOfRangeException("sigma", String.Format(ExceptionMessages.ArgumentOutOfRange, sigma));
            }
            Sigma = sigma;
            if (Double.IsNaN(mu))
            {
                throw new ArgumentOutOfRangeException("mu", String.Format(ExceptionMessages.ArgumentIsNaN, mu));
            }
            Mu = mu;
            LongName = new IdentifierString(String.Format("Normal distribution N({0}, {1})", Mu, Sigma * Sigma));
            Name = new IdentifierString(String.Format("N({0}, {1})", Mu, Sigma * Sigma));
            Moment = new MomentCalculator(this);
        }
        #endregion

        #region static constructor

        /// <summary>Initializes the <see cref="NormalDistribution" /> class.
        /// </summary>
        static NormalDistribution()
        {
            Standard = new NormalDistribution(0.0, 1.0);
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
            get { return InfoOutputDetailLevel.Full; }
        }
        #endregion

        #region IProbabilityDistribution Members

        /// <summary>Gets the infimum of the support of the distribution.
        /// </summary>
        /// <value>The infimum of the support of the distribution.</value>
        public double Infimum
        {
            get { return Double.NegativeInfinity; }
        }

        /// <summary>Gets the supremum of the support of the distribution.
        /// </summary>
        /// <value>The supremum of the support of the distribution.</value>
        public double Supremum
        {
            get { return Double.PositiveInfinity; }
        }

        /// <summary>Gets the (raw, central etc.) moments of the probability distribution.
        /// </summary>
        /// <value>The (raw, central etc.) moments of the probability distribution.</value>
        public ProbabilityDistributionMoments Moment
        {
            get;
            private set;
        }

        /// <summary>Gets the median.
        /// </summary>
        /// <value>The median.</value>
        public double Median
        {
            get { return Mu; }
        }
        #endregion

        /// <summary>Gets the mean of the normal distribution.
        /// </summary>
        /// <value>The mean of the normal distribution.</value>
        public double Mu
        {
            get;
            private set;
        }

        /// <summary>Gets the standard deviation of the normal distribution.
        /// </summary>
        /// <value>The standard deviation of the normal distribution.</value>
        public double Sigma
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
            return StandardNormalDistribution.GetCdfValue((x - Mu) / Sigma);
        }

        /// <summary>Gets a specific value of the inverse of the cummulative distribution function.
        /// </summary>
        /// <param name="probability">The probability where to evaluate.</param>
        /// <returns>The specified value of the inverse of the cummulative distribution function.</returns>
        public double GetInverseCdfValue(double probability)
        {
            return StandardNormalDistribution.GetInverseCdfValue(probability) * Sigma + Mu;
        }

        /// <summary>Gets a specific value of the probability density function.
        /// </summary>
        /// <param name="x">The point where to evaluate.</param>
        /// <returns>The specified value of the probability density function.</returns>
        public double GetPdfValue(double x)
        {
            return MathConsts.OneOverSqrtTwoPi / Sigma * Math.Exp(-0.5 * (x - Mu) * (x - Mu) / Moment.Variance);
        }

        /// <summary>Gets a specific value of the characteristic function E[exp(itX)].
        /// </summary>
        /// <param name="t">The point where to evaluate the characteristic function.</param>
        /// <returns>The specified value of the characteristic function E[exp(itX)].</returns>
        public Complex GetChfValue(double t)
        {
            return Complex.Exp(Complex.ImaginaryOne * Mu * t - 0.5 * Moment.Variance * t * t);
        }
        #endregion

        #region IInfoOutputQueriable Members

        /// <summary>Sets the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> property.
        /// </summary>
        /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
        /// <returns>A value indicating whether the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> has been set to <paramref name="infoOutputDetailLevel" />.</returns>
        public bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
        {
            return (infoOutputDetailLevel == InfoOutputDetailLevel.Full);
        }

        /// <summary>Gets informations of the current object as a specific <see cref="InfoOutput" /> instance.
        /// </summary>
        /// <param name="infoOutput">The <see cref="InfoOutput" /> object which is to be filled with informations concering the current instance.</param>
        /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
        public void FillInfoOutput(InfoOutput infoOutput, string categoryName = InfoOutput.GeneralCategoryName)
        {
            var infoOutputPackage = infoOutput.AcquirePackage(categoryName);

            infoOutputPackage.Add("Distribution", "Normal");
            infoOutputPackage.Add("Mu", Mu);
            infoOutputPackage.Add("Sigma", Sigma);

            infoOutputPackage.Add("Media", Median);
            infoOutputPackage.Add("Infimum", Infimum);
            infoOutputPackage.Add("Supremum", Supremum);

            if (m_InfoOutputAction != null)
            {
                m_InfoOutputAction(infoOutput, categoryName);
            }
        }
        #endregion

        #endregion

        #region public static methods

        /// <summary>Creates a new <see cref="NormalDistribution"/> instance.
        /// </summary>
        /// <param name="mu">The mean.</param>
        /// <param name="sigma">The standard deviation.</param>
        /// <returns>The <see cref="NormalDistribution"/> object with respect to the specified mean and standard deviation.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if one of the arguments is not valid.</exception>
        public static NormalDistribution Create(double mu, double sigma)
        {
            return new NormalDistribution(mu, sigma);
        }
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