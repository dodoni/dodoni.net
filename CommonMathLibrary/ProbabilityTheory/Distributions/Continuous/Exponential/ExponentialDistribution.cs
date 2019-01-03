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
    /// <summary>Represents the Exponential distribution, i.e. the density is specified by f(x) = \lambda * exp(-\lambda * x) for x >= 0.
    /// </summary>
    /// <remarks>The implementation is based on
    /// "Exponential Distribution: Theory, Methods and Applications", N. Balakrishnan, A. P. Basu, 1996; http://www.wikipedia.org/wiki/Exponential_distribution. </remarks>
    public partial class ExponentialDistribution : IProbabilityDistribution
    {
        #region private members

        /// <summary>A delegate called if specific data should be stored in <see cref="FillInfoOutput(InfoOutput, string)"/>; perhaps <c>null</c>
        /// </summary>
        private Action<InfoOutput, string> m_InfoOutputAction;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="ExponentialDistribution" /> class.
        /// </summary>
        /// <param name="lambda">The value of parameter \lambda.</param>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="lambda"/> is negative or <c>0.0</c>.</exception>
        public ExponentialDistribution(double lambda)
            : this(lambda, infoOutputAction: null)
        {
        }
        #endregion

        #region internal protected constructors

        /// <summary>Initializes a new instance of the <see cref="ExponentialDistribution" /> class.
        /// </summary>
        /// <param name="lambda">The value of parameter \lambda.</param>
        /// <param name="infoOutputAction">A delegate called if specific data should be stored in <see cref="FillInfoOutput(InfoOutput, string)"/>; perhaps <c>null</c>.</param>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="lambda"/> is negative or <c>0.0</c>.</exception>
        internal protected ExponentialDistribution(double lambda, Action<InfoOutput, string> infoOutputAction)
        {
            m_InfoOutputAction = infoOutputAction;

            if (lambda <= 0)
            {
                throw new ArgumentException("lambda");
            }
            Beta = 1.0 / lambda;
            Lambda = lambda;

            Moment = new MomentCalculator(this);
            Name = new IdentifierString(String.Format("Exp({0})", lambda));
            LongName = new IdentifierString(String.Format("Exponential distribution; lambda = {0}", lambda));
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the long name of the current instance.
        /// </summary>
        /// <value>The (perhaps) language dependent long name of the current instance.</value>
        public IdentifierString LongName
        {
            get;
            private set;
        }

        /// <summary>Gets the name of the current instance.
        /// </summary>
        /// <value>The language independent name of the current instance.</value>
        public IdentifierString Name
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

        /// <summary>Gets the expectation, i.e. the first moment, or <see cref="System.Double.NaN" /> if the first moment does not exist.
        /// </summary>
        /// <value>The expectation, i.e. the first moment, or <see cref="System.Double.NaN" /> if the first moment does not exist.</value>
        public double Expectation
        {
            get { return Beta; }
        }

        /// <summary>Gets the variance, i.e. the second central moment, or <see cref="System.Double.NaN" /> if the second central moment does not exit.
        /// </summary>
        /// <value>The variance, i.e. the second central moment, or <see cref="System.Double.NaN" /> if the second central moment does not exit.</value>
        public double Variance
        {
            get { return Beta * Beta; }
        }

        /// <summary>Gets the median.
        /// </summary>
        /// <value>The median.</value>
        public double Median
        {
            get { return MathConsts.Log2 * Beta; }
        }

        /// <summary>Gets the (raw, central etc.) moments of the probability distribution.
        /// </summary>
        /// <value>The (raw, central etc.) moments of the probability distribution.</value>
        public ProbabilityDistributionMoments Moment
        {
            get;
            private set;
        }

        /// <summary>Gets the infimum of the support of the distribution.
        /// </summary>
        /// <value>The infimum of the support of the distribution.</value>
        public double Infimum
        {
            get { return 0.0; }
        }

        /// <summary>Gets the supremum of the support of the distribution.
        /// </summary>
        /// <value>The supremum of the support of the distribution.</value>
        public double Supremum
        {
            get { return Double.PositiveInfinity; }
        }
        #endregion

        /// <summary>Gets the value of parameter \lambda.
        /// </summary>
        /// <value>The value of parameter \lambda.</value>
        public double Lambda
        {
            get;
            private set;
        }
        #endregion

        #region private properties

        /// <summary>Gets one over <see cref="Lambda"/>.
        /// </summary>
        /// <value>One over <see cref="Lambda"/>.</value>
        private double Beta
        {
            get;
            set;
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
            if (x <= 0)
            {
                return 0.0;
            }
            return 1 - Math.Exp(-x * Lambda);
        }

        /// <summary>Gets a specific value of the characteristic function E[exp(itX)].
        /// </summary>
        /// <param name="t">The point where to evaluate the characteristic function.</param>
        /// <returns>The specified value of the characteristic function E[exp(itX)].</returns>
        public Complex GetChfValue(double t)
        {
            return Lambda / (Lambda - Complex.ImaginaryOne * t);
        }

        /// <summary>Gets a specific value of the inverse of the cummulative distribution function, i.e. quantile function.
        /// </summary>
        /// <param name="probability">The probability where to evaluate.</param>
        /// <returns>The specified value of the inverse of the cummulative distribution function.</returns>
        public double GetInverseCdfValue(double probability)
        {
            return -Beta * Math.Log(1 - probability);
        }

        /// <summary>Gets a specific value of the probability density function.
        /// </summary>
        /// <param name="x">The point where to evaluate.</param>
        /// <returns>The specified value of the probability density function.</returns>
        public double GetPdfValue(double x)
        {
            if (x < 0)
            {
                return 0.0;
            }
            return Lambda * Math.Exp(-x * Lambda);
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
        public void FillInfoOutput(InfoOutput infoOutput, string categoryName = "General")
        {
            var infoOutputPackage = infoOutput.AcquirePackage(categoryName);
            infoOutputPackage.Add("Distribution", "Exponential");
            infoOutputPackage.Add("Lambda", Lambda);

            infoOutputPackage.Add("Expectation", Expectation);
            infoOutputPackage.Add("Variance", Variance);
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

        /// <summary>Creates a new <see cref="ExponentialDistribution"/> object.
        /// </summary>
        /// <param name="lambda">The value of parameter \lambda.</param>
        /// <returns>The specified <see cref="ExponentialDistribution"/> object.</returns>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="lambda"/> is negative or <c>0.0</c>.</exception>
        public static ExponentialDistribution Create(double lambda)
        {
            return new ExponentialDistribution(lambda);
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