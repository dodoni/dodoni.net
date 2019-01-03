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

using Dodoni.Finance;
using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Utilities;

namespace Dodoni.Finance.CommonMarketUsages.DayCountConventions
{
    /// <summary>The day count convention ISMA 'Act/Act' (Bond). This day count fraction is defined as the actual numbers of days in the period over 
    /// the actual number of days in a reference period (=coupon period) times the coupon frequency. The coupon frequency will be numerically computed and is perhaps rounded.
    /// </summary>
    internal class ActualActualISMA : IDayCountConvention
    {
        #region private static readonly members

        /// <summary>The name of the day count convention.
        /// </summary>
        private static readonly IdentifierString sm_Name = new IdentifierString("Act/Act (ISMA)");

        /// <summary>The long name of the day count convention, i.e. language dependent.
        /// </summary>
        private static readonly IdentifierString sm_LongName = new IdentifierString(DayCountConventionResources.ActualActualISMALongName);

        /// <summary>The annotation, i.e. description of the day count convention.
        /// </summary>
        private static readonly string sm_Annotation = DayCountConventionResources.ActualActualISMA;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="ActualActualISMA"/> class.
        /// </summary>
        public ActualActualISMA()
        {
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the day count convention.
        /// </summary>
        /// <value>The name of the day count convention.</value>
        public IdentifierString Name
        {
            get { return ActualActualISMA.sm_Name; }
        }

        /// <summary>Gets the long name of the day count convention.
        /// </summary>
        /// <value>The language dependent long name of the day count convention.</value>
        public IdentifierString LongName
        {
            get { return ActualActualISMA.sm_LongName; }
        }
        #endregion

        #region IAnnotatable Members

        /// <summary>Gets a value indicating whether the annotation is readonly.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the annotation of this instance is readonly; otherwise, <c>false</c>.
        /// </value>
        bool IAnnotatable.HasReadOnlyAnnotation
        {
            get { return true; }
        }

        /// <summary>Gets the annotation of the current instance.
        /// </summary>
        /// <value>The annotation of the current instance.</value>
        public string Annotation
        {
            get { return ActualActualISMA.sm_Annotation; }
        }
        #endregion

        #region IDayCountConvention Members

        /// <summary>Gets a value indicating how to interpret the parameters <code>referenceStartDate</code> and <code>referenceEndDate</code>
        /// in <see cref="IDayCountConvention.GetYearFraction(DateTime, DateTime, DateTime?, DateTime?)"/> and <see cref="IDayCountConvention.GetYearFraction(DateTime, DateTime, out int, DateTime?, DateTime?)"/>.
        /// </summary>
        /// <value>A value indicating whether a reference period is used.</value>
        public DayCountReferencePeriodUsage ReferencePeriodUsage
        {
            get { return DayCountReferencePeriodUsage.OptionalCouponPeriod; }
        }
        #endregion

        #endregion

        #region public methods

        #region IAnnotatable Members

        /// <summary>Sets the annotation of the current instance.
        /// </summary>
        /// <param name="annotation">The annotation.</param>
        /// <returns>A value indicating whether the <see cref="Annotation"/> has been changed.
        /// </returns>
        bool IAnnotatable.TrySetAnnotation(string annotation)
        {
            return false;
        }
        #endregion

        #region IDayCountConvention Members

        /// <summary>Gets the date year fraction between two <see cref="System.DateTime"/> objects.
        /// </summary>
        /// <param name="startDate">The start date of the period.</param>
        /// <param name="endDate">The end date of the period.</param>
        /// <param name="referenceStartDate">The (optional) reference start date; see <see cref="IDayCountConvention.ReferencePeriodUsage"/>
        /// for its interpretation. Can be set to <c>null</c> in any case.</param>
        /// <param name="referenceEndDate">The (optional) reference end date; see <see cref="IDayCountConvention.ReferencePeriodUsage"/>
        /// for its interpretation. Can be set to <c>null</c> in any case.</param>
        /// <returns>The date year fraction for the period [<paramref name="startDate"/>; <paramref name="endDate"/>].
        /// </returns>
        public double GetYearFraction(DateTime startDate, DateTime endDate, DateTime? referenceStartDate = null, DateTime? referenceEndDate = null)
        {
            return GetDateYearFraction(startDate, endDate, referenceStartDate, referenceEndDate);
        }

        /// <summary>Gets the date year fraction between two <see cref="System.DateTime"/> objects.
        /// </summary>
        /// <param name="startDate">The start date of the period.</param>
        /// <param name="endDate">The end date of the period.</param>
        /// <param name="dayDifference">The number of days in the period (output).</param>
        /// <param name="referenceStartDate">The (optional) reference start date; see <see cref="IDayCountConvention.ReferencePeriodUsage"/>
        /// for its interpretation. Can be set to <c>null</c> in any case.</param>
        /// <param name="referenceEndDate">The (optional) reference end date; see <see cref="IDayCountConvention.ReferencePeriodUsage"/>
        /// for its interpretation. Can be set to <c>null</c> in any case.</param>
        /// <returns>The date year fraction for the period [<paramref name="startDate"/>; <paramref name="endDate"/>].
        /// </returns>
        public double GetYearFraction(DateTime startDate, DateTime endDate, out int dayDifference, DateTime? referenceStartDate = null, DateTime? referenceEndDate = null)
        {
            dayDifference = endDate.Subtract(startDate).Days;
            return GetDateYearFraction(startDate, endDate, referenceStartDate, referenceEndDate);
        }
        #endregion

        /// <summary>Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return sm_LongName.String;
        }
        #endregion

        #region private methods

        /// <summary>Gets the day count fraction between two dates.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="referenceStartDate">The (optional) reference start date; see <see cref="IDayCountConvention.ReferencePeriodUsage"/>
        /// for its interpretation. Can be set to <c>null</c> in any case.</param>
        /// <param name="referenceEndDate">The (optional) reference end date; see <see cref="IDayCountConvention.ReferencePeriodUsage"/>
        /// for its interpretation. Can be set to <c>null</c> in any case.</param>        
        /// <returns>The date year fraction, i.e. the number of days in the time period over the number of days in the coupon period times the coupon frequency.
        /// </returns>
        private double GetDateYearFraction(DateTime startDate, DateTime endDate, DateTime? referenceStartDate, DateTime? referenceEndDate)
        {
            if (startDate == endDate)
            {
                return 0.0;
            }
            if (endDate < startDate)
            {
                return -GetDateYearFraction(endDate, startDate, referenceStartDate, referenceEndDate);
            }
            DateTime couponPeriodStart = referenceStartDate ?? startDate;
            DateTime couponPeriodEnd = referenceEndDate ?? endDate;

            if ((couponPeriodStart > couponPeriodEnd) || (startDate > couponPeriodEnd))
            {
                throw new ArgumentException(String.Format(ExceptionMessages.ArgumentCombinationInvalid, "start Date, end date, reference start and reference end date"));
            }

            /* estimate the length in months of one period: */
            int monthsInCouponPeriod = (int)(0.5 + 12 * couponPeriodEnd.Subtract(couponPeriodStart).Days / 365.0);
            if (monthsInCouponPeriod == 0) // a short period, i.e. <= 15 days, is specified ( a theoretical case! )
            {
                /* Here, we use the same approach as in the QuantLib 1.2.0, i.e. we take the reference period as 1 year from the start date
                 */
                couponPeriodStart = startDate;
                couponPeriodEnd = startDate.AddYears(1);
                monthsInCouponPeriod = 12;

                /*
                 * In an alternative approach, we re-calculate the coupon frequency and round the result:
                 */
                // couponFrequency = 12 * couponPeriodStart.Subtract(couponPeriodEnd).Days / 365.0;
                // couponFrequency = Math.Round(couponFrequency, 2);
            }
            double couponFrequency = monthsInCouponPeriod / 12.0;

            /* the following code is adapted from QuantLib 1.2.0, see http://quantLib.org/, Copyright (C) 2000, 2001, 2002, 2003 RiskMap srl
             * with some modifications */

            if (endDate <= couponPeriodEnd)
            {
                // here couponPeriodEndDate is a future (notional?) payment date
                if (startDate >= couponPeriodStart)
                {
                    // here refPeriodStart is the last (maybe notional) payment date.
                    // refPeriodStart <= d1 <= d2 <= refPeriodEnd
                    // [maybe the equality should be enforced, since
                    // refPeriodStart < d1 <= d2 < refPeriodEnd
                    // could give wrong results] ???
                    return (couponFrequency * endDate.Subtract(startDate).Days) / couponPeriodEnd.Subtract(couponPeriodStart).Days;
                }
                else
                {
                    /* a long first coupon */

                    // here refPeriodStart is the next (maybe notional) payment date and refPeriodEnd is the second next (maybe notional) payment date.
                    // d1 < refPeriodStart < refPeriodEnd AND d2 <= refPeriodEnd this case is long first coupon

                    // the last notional payment date
                    DateTime previousRef = couponPeriodStart.AddMonths(-monthsInCouponPeriod);
                    if (endDate > couponPeriodStart)
                        return GetDateYearFraction(startDate, couponPeriodStart, previousRef, couponPeriodStart) + GetDateYearFraction(couponPeriodStart, endDate, couponPeriodStart, couponPeriodEnd);
                    else
                        return GetDateYearFraction(startDate, endDate, previousRef, couponPeriodStart);
                }
            }
            else
            {
                // here refPeriodEnd is the last (notional?) payment date
                // d1 < refPeriodEnd < d2 AND refPeriodStart < refPeriodEnd
                //QL_REQUIRE(refPeriodStart<=d1,
                //           "invalid dates: "
                //           "d1 < refPeriodStart < refPeriodEnd < d2");
                // now it is: refPeriodStart <= d1 < refPeriodEnd < d2

                // the part from d1 to refPeriodEnd
                double sum = GetDateYearFraction(startDate, couponPeriodEnd, couponPeriodStart, couponPeriodEnd);

                // the part from refPeriodEnd to d2 count how many regular periods are in [refPeriodEnd, d2],
                // then add the remaining time
                int i = 0;
                DateTime newRefStart, newRefEnd;
                for (; ; )
                {
                    newRefStart = couponPeriodEnd.AddMonths(i * monthsInCouponPeriod);
                    newRefEnd = couponPeriodEnd.AddMonths((i + 1) * monthsInCouponPeriod);
                    if (endDate < newRefEnd)
                    {
                        break;
                    }
                    else
                    {
                        sum += couponFrequency;
                        i++;
                    }
                }
                return sum + GetDateYearFraction(newRefStart, endDate, newRefStart, newRefEnd);
            }
        }
        #endregion
    }
}