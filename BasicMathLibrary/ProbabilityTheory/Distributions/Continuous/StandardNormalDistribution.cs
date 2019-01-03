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
using System.Threading.Tasks;

using Dodoni.BasicComponents;
using Dodoni.MathLibrary.Basics;
using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.ProbabilityTheory.Distributions
{
    /// <summary>
    /// Provides methods for cummulative distribution function etc. with respect to the Normal distribution.
    /// </summary>
    public partial class StandardNormalDistribution : IProbabilityDistribution
    {
        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="StandardNormalDistribution"/> class.
        /// </summary>
        public StandardNormalDistribution()
        {
            LongName = new IdentifierString("Standard Normal distribution N(0,1)");
            Name = new IdentifierString("N(0,1)");
            Moment = new MomentCalculator();
        }
        #endregion

        #region static constructor

        /// <summary>Initializes the <see cref="StandardNormalDistribution" /> class.
        /// </summary>
        static StandardNormalDistribution()
        {
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
            get { return 0.0; }
        }
        #endregion

        /// <summary>Gets the mean of the (standard) normal distribution.
        /// </summary>
        /// <value>The mean of the (standard) normal distribution.</value>
        public double Mu
        {
            get { return 0.0; }
        }

        /// <summary>Gets the standard deviation of the (standard) normal distribution.
        /// </summary>
        /// <value>The standard deviation of the (standard) normal distribution.</value>
        public double Sigma
        {
            get { return 1.0; }
        }
        #endregion

        #region public methods

        #region IProbabilityDistribution Members

        /// <summary>Gets a specific value of the cummulative distribution function.
        /// </summary>
        /// <param name="x">The value where to evaluate.</param>
        /// <returns>The specified value of the cummulative distribution function.</returns>
        double IProbabilityDistribution.GetCdfValue(double x)
        {
            return GetCdfValue(x);
        }

        /// <summary>Gets specific values of the cummulative distribution function.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The arguments where to evaluate the cummulative distribution function.</param>
        /// <param name="y">The specified values of the cummulative distribution function (output).</param>
        void IProbabilityDistribution.GetCdfValue(int n, double[] a, double[] y)
        {
            VectorUnit.Special.CdfNorm(n, a, y);  // perhaps this gives an other result than in the Non-Vector implementation
        }

        /// <summary>Gets a specific value of the inverse of the cummulative distribution function, i.e. quantile function.
        /// </summary>
        /// <param name="y">The probability where to evaluate.</param>
        /// <returns>The specified value of the inverse of the cummulative distribution function.</returns>
        double IProbabilityDistribution.GetInverseCdfValue(double y)
        {
            return GetInverseCdfValue(y);
        }

        /// <summary>Gets specific values of the inverse of the cummulative distribution function, i.e. quantile function.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The probabilities where to evaluate the inverse of the cummulative distribution function.</param>
        /// <param name="y">The specified values of the inverse of the cummulative distribution function (output).</param>
        void IProbabilityDistribution.GetInverseCdfValue(int n, double[] a, double[] y)
        {
            VectorUnit.Special.CdfNormInv(n, a, y);
        }

        /// <summary>Gets a specific value of the probability density function.
        /// </summary>
        /// <param name="x">The point where to evaluate.</param>
        /// <returns>The specified value of the probability density function.</returns>
        double IProbabilityDistribution.GetPdfValue(double x)
        {
            return GetPdfValue(x);
        }

        /// <summary>Gets specific values of the probability density function.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The arguments where to evaluate the probability density function.</param>
        /// <param name="y">The specified values of the probability density function (output).</param>
        void IProbabilityDistribution.GetPdfValue(int n, double[] a, double[] y)
        {
            BLAS.Level1.dcopy(n, a, y);
            VectorUnit.Basics.Mul(n, a, y, y, factor: -1.0);
            VectorUnit.Basics.Exp(n, y);  // exp(- a_j * a_j) for j = 0,1,...,n-1.
        }

        /// <summary>Gets a specific value of the characteristic function E[exp(itX)].
        /// </summary>
        /// <param name="t">The point where to evaluate the characteristic function.</param>
        /// <returns>The specified value of the characteristic function E[exp(itX)].</returns>
        Complex IProbabilityDistribution.GetChfValue(double t)
        {
            return GetChfValue(t);
        }

        /// <summary>Gets specific values of the characteristic function E[exp(itX)].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The arguments where to evaluate the characteristic function.</param>
        /// <param name="y">The specified values of the characteristic function E[exp(itX)] (output).</param>
        void IProbabilityDistribution.GetChfValue(int n, double[] a, Complex[] y)
        {
            Parallel.For(0, n, k =>
             {
                 var t = a[k];
                 y[k] = Complex.ImaginaryOne * t - 0.5 * t * t;
             });
            VectorUnit.Basics.Exp(n, y); // exp( i * a[k] - 0.5 * a[k] * a[k]) for k = 0,1,...,n-1.
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

            infoOutputPackage.Add("Expectation", Moment.Expectation);
            infoOutputPackage.Add("Variance", Moment.Variance);
            infoOutputPackage.Add("Media", Median);
            infoOutputPackage.Add("Infimum", Infimum);
            infoOutputPackage.Add("Supremum", Supremum);

            Moment.FillInfoOutput(infoOutput, categoryName + ".Moments");
        }
        #endregion

        #endregion

        #region public static methods

        /// <summary>Creates a new <see cref="StandardNormalDistribution"/> instance.
        /// </summary>
        /// <returns>A new <see cref="StandardNormalDistribution"/> object.</returns>
        public static StandardNormalDistribution Create()
        {
            return new StandardNormalDistribution();
        }

        /// <summary>Evaluate the density of a standard normal random variable at a specified point.
        /// </summary>
        /// <param name="x">The value where to evaluate.</param>
        /// <returns>A <see cref="System.Double"/> which reflects the value of the density function.</returns>
        public static double GetPdfValue(double x)
        {
            return MathConsts.OneOverSqrtTwoPi * Math.Exp(-x * x * 0.5);
        }

        /// <summary>Evaluate the cummulative distribution function of a standard normal random variable at a specified point.
        /// </summary>
        /// <param name="x">The value where to evaluate.</param>
        /// <returns>A <see cref="System.Double"/> which reflects the value of the cummulative distribution function.</returns>
        public static double GetCdfValue(double x)
        {
            return SpecialFunction.PrimitiveIntegral.GetStandardNormalCdfValue(x);
        }

        /// <summary>Gets the inverse of the standard cummulative distribution function.
        /// </summary>
        /// <param name="probability">The probability where to evaluate.</param>
        /// <returns>A <see cref="System.Double"/> which reflects the value of the inverse cummulative distribution function.</returns>
        public static double GetInverseCdfValue(double probability)
        {
            return SpecialFunction.PrimitiveIntegral.GetStandardNormalInverseCdfValue(probability);
        }

        /// <summary>Gets a specific value of the characteristic function E[exp(itX)].
        /// </summary>
        /// <param name="t">The point where to evaluate the characteristic function.</param>
        /// <returns>The specified value of the characteristic function E[exp(itX)].</returns>
        public static Complex GetChfValue(double t)
        {
            return Complex.Exp(Complex.ImaginaryOne * t - 0.5 * t * t);
        }
        #endregion
    }
}