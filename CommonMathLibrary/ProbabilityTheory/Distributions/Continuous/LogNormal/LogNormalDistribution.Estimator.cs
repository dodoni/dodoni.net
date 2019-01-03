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
using System.Linq;
using System.Data;
using System.Collections.Generic;

using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.ProbabilityTheory.Distributions
{
    public partial class LogNormalDistribution
    {
        /// <summary>Provides estimator algorithms for estimating the parameters of a Log-Normal distribution.
        /// </summary>
        public static class Estimator
        {
            #region nested enumerations/classes

            /// <summary>The methods how to estimate the parameters for the Log-Normal distribution.
            /// </summary>
            public enum Method
            {
                /// <summary>Apply the Maximum Likelihood approach.
                /// </summary>
                MaximumLikelihood,

                /// <summary>Apply the Method of Moments approach.
                /// </summary>
                MethodsOfMoments
            }

            /// <summary>Serves as implementation for the standard estimator, i.e. the Maximum Likelihood Estimator.
            /// </summary>
            private class MLEstimator : IProbabilityDistributionEstimator<LogNormalDistribution>
            {
                #region internal constructors

                /// <summary>Initializes a new instance of the <see cref="MLEstimator" /> class.
                /// </summary>
                /// <param name="infoOutputDetailLevel">A value indicating the level of detail.</param>
                internal MLEstimator(InfoOutputDetailLevel infoOutputDetailLevel)
                {
                    InfoOutputDetailLevel = infoOutputDetailLevel;
                    Name = new IdentifierString("ML Estimator");
                    LongName = new IdentifierString("Log-Normal distribution: ML Estimator");
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

                #endregion

                #region public methods

                #region IProbabilityDistributionEstimator<LogNormalDistribution> Members

                /// <summary>Create a specific <see cref="LogNormalDistribution"/> object that represents a specified distribution with estimated parameters.
                /// </summary>
                /// <param name="empiricalDistribution">The sample to fit the parameters of the specified distribution in its <see cref="EmpiricalDistribution"/> representation.</param>
                /// <returns>A specific <see cref="LogNormalDistribution"/> object that represents the specified distribution with estimated parameters.</returns>
                public LogNormalDistribution Create(EmpiricalDistribution empiricalDistribution)
                {
                    var mu = DoMath.Average(empiricalDistribution.Sample.Select(xn => Math.Log(xn)));
                    var variance = DoMath.Average(empiricalDistribution.Sample.Select(xn => DoMath.Pow(Math.Log(xn) - mu, 2)));

                    if (InfoOutputDetailLevel.IsAtLeastAsComprehensiveAs(InfoOutputDetailLevel.High) == true)
                    {
                        return new LogNormalDistribution(mu, Math.Sqrt(variance), (infoOutput, categoryName) =>
                        {
                            var infoOutputPackage = infoOutput.AcquirePackage(categoryName);

                            var dataTable = new DataTable("Sample");
                            dataTable.Columns.Add("Value", typeof(double));
                            foreach (var value in empiricalDistribution.Sample)
                            {
                                dataTable.Rows.Add(new object[] { value });
                            }
                            infoOutputPackage.Add(dataTable);
                        });
                    }
                    else
                    {
                        return new LogNormalDistribution(mu, Math.Sqrt(variance));
                    }
                }
                #endregion

                #region IInfoOutputQueriable Members

                /// <summary>Sets the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> property.
                /// </summary>
                /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
                /// <returns>A value indicating whether the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> has been set to <paramref name="infoOutputDetailLevel" />.</returns>
                public bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
                {
                    return (infoOutputDetailLevel == this.InfoOutputDetailLevel);
                }

                /// <summary>Gets informations of the current object as a specific <see cref="InfoOutput" /> instance.
                /// </summary>
                /// <param name="infoOutput">The <see cref="InfoOutput" /> object which is to be filled with informations concering the current instance.</param>
                /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
                public void FillInfoOutput(InfoOutput infoOutput, string categoryName = InfoOutput.GeneralCategoryName)
                {
                    var infoOutputPackage = infoOutput.AcquirePackage(categoryName);
                    infoOutputPackage.Add("Name", Name.String);
                    infoOutputPackage.Add("Distribution", "Log-Normal");
                    infoOutputPackage.Add("Method", "ML");
                }
                #endregion

                /// <summary>Returns a <see cref="System.String" /> that represents this instance.
                /// </summary>
                /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
                public override string ToString()
                {
                    return LongName.String;
                }
                #endregion
            }

            /// <summary>Serves as implementation for the Method of Moments Estimator.
            /// </summary>
            private class MoMEstimator : IProbabilityDistributionEstimator<LogNormalDistribution>
            {
                #region internal constructors

                /// <summary>Initializes a new instance of the <see cref="MoMEstimator" /> class.
                /// </summary>
                /// <param name="infoOutputDetailLevel">A value indicating the level of detail.</param>
                internal MoMEstimator(InfoOutputDetailLevel infoOutputDetailLevel)
                {
                    InfoOutputDetailLevel = infoOutputDetailLevel;
                    Name = new IdentifierString("MoM Estimator");
                    LongName = new IdentifierString("Log-Normal distribution: Methods of Moments Estimator");
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

                #endregion

                #region public methods

                #region IProbabilityDistributionEstimator<LogNormalDistribution> Members

                /// <summary>Create a specific <see cref="StandardNormalDistribution"/> object that represents a specified distribution with estimated parameters.
                /// </summary>
                /// <param name="empiricalDistribution">The sample to fit the parameters of the specified distribution in its <see cref="EmpiricalDistribution"/> representation.</param>
                /// <returns>A specific <see cref="LogNormalDistribution"/> object that represents the specified distribution with estimated parameters.</returns>
                public LogNormalDistribution Create(EmpiricalDistribution empiricalDistribution)
                {
                    var variance = Math.Log(1 + empiricalDistribution.Moment.Variance / (empiricalDistribution.Mean * empiricalDistribution.Mean));  // = ln(1 + sn^2 / \hat{x_n}^2 )
                    var mu = Math.Log(empiricalDistribution.Mean) - 0.5 * variance; // = ln( \hat{x_n} ) - 0.5 * ln(1 + sn^2 / \hat{x_n}^2 )

                    if (InfoOutputDetailLevel.IsAtLeastAsComprehensiveAs(InfoOutputDetailLevel.High) == true)
                    {
                        return new LogNormalDistribution(mu, Math.Sqrt(variance), (infoOutput, categoryName) =>
                        {
                            var infoOutputPackage = infoOutput.AcquirePackage(categoryName);

                            var dataTable = new DataTable("Sample");
                            dataTable.Columns.Add("Value", typeof(double));
                            foreach (var value in empiricalDistribution.Sample)
                            {
                                dataTable.Rows.Add(new object[] { value });
                            }
                            infoOutputPackage.Add(dataTable);
                        });
                    }
                    else
                    {
                        return new LogNormalDistribution(mu, Math.Sqrt(variance));
                    }
                }
                #endregion

                #region IInfoOutputQueriable Members

                /// <summary>Sets the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> property.
                /// </summary>
                /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
                /// <returns>A value indicating whether the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> has been set to <paramref name="infoOutputDetailLevel" />.</returns>
                public bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
                {
                    return (infoOutputDetailLevel == this.InfoOutputDetailLevel);
                }

                /// <summary>Gets informations of the current object as a specific <see cref="InfoOutput" /> instance.
                /// </summary>
                /// <param name="infoOutput">The <see cref="InfoOutput" /> object which is to be filled with informations concering the current instance.</param>
                /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
                public void FillInfoOutput(InfoOutput infoOutput, string categoryName = InfoOutput.GeneralCategoryName)
                {
                    var infoOutputPackage = infoOutput.AcquirePackage(categoryName);
                    infoOutputPackage.Add("Name", Name.String);
                    infoOutputPackage.Add("Distribution", "Log-Normal");
                    infoOutputPackage.Add("Method", "MoM");
                }
                #endregion

                /// <summary>Returns a <see cref="System.String" /> that represents this instance.
                /// </summary>
                /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
                public override string ToString()
                {
                    return LongName.String;
                }
                #endregion
            }
            #endregion

            #region public (static) methods

            /// <summary>Creates a specific parameter estimating approach with respect to the Log-Normal distribution.
            /// </summary>
            /// <param name="method">A value indicating the method how to estimate the parameters.</param>
            /// <param name="infoOutputDetailLevel">A value indicating the level of detail.</param>
            /// <returns>The specified parameter estimating approach with respect to the Normal distribution.</returns>
            public static IProbabilityDistributionEstimator<LogNormalDistribution> Create(Method method, InfoOutputDetailLevel infoOutputDetailLevel = InfoOutputDetailLevel.Middle)
            {
                switch (method)
                {
                    case Method.MaximumLikelihood:
                        return new MLEstimator(infoOutputDetailLevel);
                    case Method.MethodsOfMoments:
                        return new MoMEstimator(infoOutputDetailLevel);
                    default:
                        throw new NotImplementedException();
                }
            }
            #endregion
        }
    }
}