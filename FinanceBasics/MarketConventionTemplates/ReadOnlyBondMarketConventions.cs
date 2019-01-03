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

using Dodoni.BasicComponents;
using Dodoni.Finance.DateFactory;
using Dodoni.BasicComponents.Containers;

namespace Dodoni.Finance.MarketConventionTemplates
{
    /// <summary>Serves as read-only wrapper class for the Bond market conventions.
    /// </summary>
    /// <remarks>This class is used as container which represents the Bond market conventions for a specific currency.</remarks>
    public class ReadOnlyBondMarketConventions : IInfoOutputQueriable
    {
        #region public (readonly) members

        /// <summary>The the number of business days to settle with respect to the Bond market.
        /// </summary>
        public readonly int BusinessDaysToSettle;

        /// <summary>The day count convention.
        /// </summary>
        public readonly IDayCountConvention DayCountConvention;

        /// <summary>The business day convention.
        /// </summary>
        public readonly IBusinessDayConvention BusinessDayConvention;

        /// <summary>The coupon frequency.
        /// </summary>
        public readonly IDateScheduleFrequency CouponFrequency;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="ReadOnlyBondMarketConventions"/> class.
        /// </summary>
        /// <param name="bondMarketConventions">The bond market conventions.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="bondMarketConventions"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="bondMarketConventions"/> is not completely defined.</exception>
        public ReadOnlyBondMarketConventions(BondMarketConventions bondMarketConventions)
        {
            if (bondMarketConventions == null)
            {
                throw new ArgumentNullException("bondMarketConventions");
            }
            if (bondMarketConventions.IsCompletelyDefined == false)
            {
                throw new ArgumentException(String.Format(ExceptionMessages.ArgumentIsNotWellDefined, "Bond market conventions"), "bondMarketConventions");
            }
            BusinessDaysToSettle = bondMarketConventions.BusinessDaysToSettle.Value;
            BusinessDayConvention = bondMarketConventions.BusinessDayConvention;
            CouponFrequency = bondMarketConventions.CouponFrequency;
            DayCountConvention = bondMarketConventions.DayCountConvention;
        }
        #endregion

        #region public properties

        #region IInfoOutputQueriable Members

        /// <summary>Gets the info-output level of detail.
        /// </summary>
        /// <value>The info-output level of detail.</value>
        public InfoOutputDetailLevel InfoOutputDetailLevel
        {
            get { return InfoOutputDetailLevel.Full; }
        }
        #endregion

        #endregion

        #region public methods

        #region IInfoOutputQueriable Members

        /// <summary>Sets the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel"/> property.
        /// </summary>
        /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
        /// <returns>A value indicating whether the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel"/> has been changed
        /// with respect to <paramref name="infoOutputDetailLevel"/>.
        /// </returns>
        public bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
        {
            return (infoOutputDetailLevel == InfoOutputDetailLevel.Full);
        }

        /// <summary>Gets informations of the current object as a specific <see cref="InfoOutput"/> instance.
        /// </summary>
        /// <param name="infoOutput">The <see cref="InfoOutput"/> object which is to be filled with informations concering the current instance.</param>
        /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category name.</param>
        public void FillInfoOutput(InfoOutput infoOutput, string categoryName)
        {
            InfoOutputPackage infoOutputCollection = infoOutput.AcquirePackage(categoryName);
            infoOutputCollection.Add("Day count convention", DayCountConvention.Name.String);
            infoOutputCollection.Add("Business day convention", BusinessDayConvention.Name.String);
            infoOutputCollection.Add("Business days to settle", BusinessDaysToSettle);
            infoOutputCollection.Add("Coupon frequency", CouponFrequency.Name.String);
        }
        #endregion

        /// <summary>Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return String.Format("Bond market conventions ({0}, {1}, Frequency: {2}, Business days to settle: {3})", DayCountConvention, BusinessDayConvention, CouponFrequency, BusinessDaysToSettle);
        }

        /// <summary>Gets a mutable Bond Market convention template which is initially filled with values of 
        /// the current instance, i.e. with some standard conventions.
        /// </summary>
        /// <returns>A mutable Bond Market convention template.</returns>
        public BondMarketConventions GetMutableTemplate()
        {
            BondMarketConventions marketConventions = new BondMarketConventions();
            marketConventions.SetStandardDayCountconvention(DayCountConvention);
            marketConventions.SetStandardBusinessDayConvention(BusinessDayConvention);
            marketConventions.SetStandardBusinessDaysToSettle(BusinessDaysToSettle);
            marketConventions.SetStandardCouponFrequency(CouponFrequency);

            return marketConventions;
        }
        #endregion
    }
}