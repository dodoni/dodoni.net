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
    public partial class ExponentialDistribution
    {
        /// <summary>Provides estimator algorithms for estimating the parameters of a Exponential distribution.
        /// </summary>
        public static class Estimator
        {
            #region nested enumerations/classes

            /// <summary>The methods how to estimate the parameters for the Exponential distribution.
            /// </summary>
            public enum Method
            {
                /// <summary>Apply the standard approach, i.e. the Maximum Likelihood = Method of Moments approach.
                /// </summary>
                Standard
            }

            /// <summary>Serves as implementation for the standard estimator, i.e. the Maximum Likelihood Estimator = Method of Moments.
            /// </summary>
            private class StandardEstimator : IProbabilityDistributionEstimator<ExponentialDistribution>
            {
                #region internal constructors

                /// <summary>Initializes a new instance of the <see cref="StandardEstimator" /> class.
                /// </summary>
                /// <param name="infoOutputDetailLevel">A value indicating the level of detail.</param>
                internal StandardEstimator(InfoOutputDetailLevel infoOutputDetailLevel)
                {
                    InfoOutputDetailLevel = infoOutputDetailLevel;
                    Name = new IdentifierString("Standard Estimator");
                    LongName = new IdentifierString("Exponential distribution: Standard Estimator");
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

                #region IProbabilityDistributionEstimator<ExponentialDistribution> Members

                /// <summary>Create a specific <see cref="ExponentialDistribution"/> object that represents a specified distribution with estimated parameters.
                /// </summary>
                /// <param name="empiricalDistribution">The sample to fit the parameters of the specified distribution in its <see cref="EmpiricalDistribution"/> representation.</param>
                /// <returns>A specific <see cref="ExponentialDistribution"/> object that represents the specified distribution with estimated parameters.</returns>
                public ExponentialDistribution Create(EmpiricalDistribution empiricalDistribution)
                {
                    if (empiricalDistribution.Mean <= 0.0)
                    {
                        throw new ArgumentException("empiricalDistribution");
                    }
                    var lambda = 1.0 / empiricalDistribution.Mean;

                    if (InfoOutputDetailLevel.IsAtLeastAsComprehensiveAs(InfoOutputDetailLevel.High) == true)
                    {
                        return new ExponentialDistribution(lambda, (infoOutput, categoryName) =>
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
                        return new ExponentialDistribution(lambda);
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
                    infoOutputPackage.Add("Distribution", "Exponential");
                    infoOutputPackage.Add("Method", "Standard");
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

            /// <summary>Creates a specific parameter estimating approach with respect to the Exponential distribution.
            /// </summary>
            /// <param name="method">A value indicating the method how to estimate the parameters.</param>
            /// <param name="infoOutputDetailLevel">A value indicating the level of detail.</param>
            /// <returns>The specified parameter estimating approach with respect to the Exponential distribution.</returns>
            public static IProbabilityDistributionEstimator<ExponentialDistribution> Create(Method method = Method.Standard, InfoOutputDetailLevel infoOutputDetailLevel = InfoOutputDetailLevel.Middle)
            {
                switch (method)
                {
                    case Method.Standard:
                        return new StandardEstimator(infoOutputDetailLevel);
                    default:
                        throw new NotImplementedException();
                }
            }
            #endregion
        }
    }
}