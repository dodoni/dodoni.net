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

using Dodoni.BasicComponents.Logging;
using Dodoni.BasicComponents.Containers;
using Microsoft.Extensions.Logging;

namespace Dodoni.Finance.DateFactory.TimeframeDescriptions
{
    /// <summary>A time span, where the end date (=maturity date) is given in its <see cref="System.DateTime"/> representation 
    /// and for the start date (=settlement/spot date) a number of business days to settle will be take into account.
    ///<para>The implementation works in the following way:
    ///  <list type="number">
    ///   <item><description>Apply the SpotDateAdjustment to the reference date; call it spot date.</description></item>
    ///   <item><description>Point the spot date to the next business day if the number of business days to settle is strictly greater than 0 or to the previous business day if
    ///   the number of business days to settle is strictly less than 0. Add the number of business days to settle which defines the final spot date.</description></item>
    ///   <item><description>Apply the StartDateAdjustment to the spot date. This is the start date of the period.</description></item>
    ///   <item><description>Apply the EndDateAdjustment to the specified end date; this is the end date of the timeframe.</description></item>
    /// </list></para>
    /// </summary>
    internal class DateTimeTimeframeDescription : ITimeframeDescription
    {
        #region public (readonly) members

        /// <summary>The business day convention used to compute the spot date, i.e. reference date + a number of business days to settle.
        /// </summary>
        public readonly IBusinessDayConvention SpotDateAdjustment;

        /// <summary>The business day convention used to compute the start date.
        /// </summary>
        public readonly IBusinessDayConvention StartDateAdjustment;

        /// <summary>The business day convention used to compute the end date.
        /// </summary>
        public readonly IBusinessDayConvention EndDateAdjustment;

        /// <summary>The end date of the timeframe.
        /// </summary>
        public readonly DateTime EndDate;

        /// <summary>The number of business days to settle which will be taken into account for the spot date calculation.
        /// </summary>
        public readonly int BusinessDaysToSettle;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="DateTimeTimeframeDescription"/> class.
        /// </summary>
        /// <param name="endDate">The end date of the timeframe.</param>
        /// <param name="spotDateAdjustment">A business day convention used to compute the spot date (= reference date + a number of business days to settle).</param>
        /// <param name="startDateAdjustment">A business day convention used to compute the start date of the period.</param>
        /// <param name="endDateAdjustment">A business day convention used to compute the end date of the period.</param>
        /// <param name="businessDaysToSettle">The number of business days used to calculate the spot date.</param>
        /// <exception cref="ArgumentNullException">Thrown, if one of the <see cref="IBusinessDayConvention"/> arguments is <c>null</c>.</exception>
        public DateTimeTimeframeDescription(DateTime endDate, IBusinessDayConvention spotDateAdjustment, IBusinessDayConvention startDateAdjustment, IBusinessDayConvention endDateAdjustment, int businessDaysToSettle = 0)
        {
            EndDate = endDate;
            if (spotDateAdjustment == null)
            {
                throw new ArgumentNullException("spotDateAdjustment");
            }
            SpotDateAdjustment = spotDateAdjustment;
            if (startDateAdjustment == null)
            {
                throw new ArgumentNullException("startDateAdjustment");
            }
            StartDateAdjustment = startDateAdjustment;
            if (endDateAdjustment == null)
            {
                throw new ArgumentNullException("endDateAdjustment");
            }
            EndDateAdjustment = endDateAdjustment;
            BusinessDaysToSettle = businessDaysToSettle;
        }
        #endregion

        #region public properties

        #region IInfoOutputQueriable Members

        /// <summary>Gets the info-output level of detail.
        /// </summary>
        /// <value>The info-output level of detail.
        /// </value>
        public InfoOutputDetailLevel InfoOutputDetailLevel
        {
            get { return InfoOutputDetailLevel.Full; }
        }
        #endregion

        #endregion

        #region public methods

        #region ITimeframeDescription Member

        /// <summary>Gets the start and end date of the timeframe.
        /// </summary>
        /// <param name="referenceDate">A reference date (can be a business day or a holiday).</param>
        /// <param name="holidayCalendar">The (settlement) holiday calendar.</param>
        /// <param name="startDate">The start date of the time span with respect to <paramref name="referenceDate" /> (output).</param>
        /// <param name="endDate">The end date of the time span with respect to <paramref name="referenceDate" /> (output).</param>
        /// <param name="logger">An optional logger.</param>
        public void GetStartAndEndDate(DateTime referenceDate, IHolidayCalendar holidayCalendar, out DateTime startDate, out DateTime endDate, ILogger logger = null)
        {
            DateTime adjReferenceDate = SpotDateAdjustment.GetAdjustedDate(referenceDate, holidayCalendar);
            if ((logger != null) && (adjReferenceDate != referenceDate))
            {
                logger.LogInformation("Reference date {0} has been adjusted to {1}.", referenceDate.ToShortDateString(), adjReferenceDate.ToShortDateString());
            }

            DateTime spotDate = adjReferenceDate;

            if ((SpotDateAdjustment.AdjustmentType != BusinessDayAdjustmentType.AdjustmentToBusinessDay) && (holidayCalendar.IsBusinessDay(spotDate) == false))
            {
                if (BusinessDaysToSettle > 0)
                {
                    spotDate = holidayCalendar.GetForwardAdjustedBusinessDay(spotDate);
                }
                else if (BusinessDaysToSettle < 0)
                {
                    spotDate = holidayCalendar.GetPreviousAdjustedBusinessDay(spotDate);
                }
            }
            if (BusinessDaysToSettle != 0)
            {
                startDate = StartDateAdjustment.GetAdjustedDate(holidayCalendar.AddBusinessDays(spotDate, BusinessDaysToSettle), holidayCalendar);
            }
            else
            {
                startDate = StartDateAdjustment.GetAdjustedDate(spotDate, holidayCalendar);
            }
            endDate = EndDateAdjustment.GetAdjustedDate(EndDate, holidayCalendar);

            if ((logger != null) && (endDate != EndDate))
            {
                logger.LogInformation("End date {0} has been adjusted to {1}.", EndDate.ToShortDateString(), endDate.ToShortDateString());
            }
        }

        /// <summary>Gets the start and end date of the timeframe as well as a <see cref="TenorTimeSpan" /> representation of the time span between both dates.
        /// </summary>
        /// <param name="referenceDate">A reference date (can be a business day or a holiday).</param>
        /// <param name="holidayCalendar">The (settlement) holiday calendar.</param>
        /// <param name="startDate">The start date of the time span with respect to referenceDate (output).</param>
        /// <param name="endDate">The end date of the time span with respect to referenceDate (output).</param>
        /// <param name="tenor">The tenor that represents the time span; if the end date of the current object is not specified by its <see cref="TenorTimeSpan" />
        /// representation, <paramref name="tenorRoundingRule" /> will be applied to <paramref name="startDate" /> and <paramref name="endDate" /> for the calculation of a <see cref="TenorTimeSpan" /> representation.</param>
        /// <param name="tenorRoundingRule">A rounding rule which will be take into account if and only if the end date of the current object is not specified by its <see cref="TenorTimeSpan" /> representation.</param>
        /// <param name="logger">An optional logger.</param>
        public void GetStartAndEndDate(DateTime referenceDate, IHolidayCalendar holidayCalendar, out DateTime startDate, out DateTime endDate, out TenorTimeSpan tenor, TenorTimeSpan.RoundingRule tenorRoundingRule = TenorTimeSpan.RoundingRule.NearestMonth, ILogger logger = null)
        {
            GetStartAndEndDate(referenceDate, holidayCalendar, out startDate, out endDate, logger);
            tenor = TenorTimeSpan.GetTimeSpanInBetween(startDate, endDate, tenorRoundingRule);
        }

        /// <summary>Gets the start and end date of the timeframe.
        /// </summary>
        /// <param name="referenceDate">A reference date (can be a business day or a holiday).</param>
        /// <param name="holidayCalendar">The (settlement) holiday calendar.</param>
        /// <param name="fixingLag">The fixing lag, i.e. the number of business days taken into account for the calculation of fixing dates etc., for example <c>-2</c>.</param>
        /// <param name="startDate">The start date of the time span with respect to <paramref name="referenceDate" /> (output).</param>
        /// <param name="endDate">The end date of the time span with respect to <paramref name="referenceDate" /> (output).</param>
        /// <param name="logger">An optional logger.</param>
        /// <returns>The fixing date of with respect to the (interest) period.</returns>
        public DateTime GetStartAndEndDate(DateTime referenceDate, IHolidayCalendar holidayCalendar, IFixingLag fixingLag, out DateTime startDate, out DateTime endDate, ILogger logger = null)
        {
            GetStartAndEndDate(referenceDate, holidayCalendar, out startDate, out endDate, logger);
            return fixingLag.GetFixingDate(startDate, holidayCalendar);
        }

        /// <summary>Gets the start and end date of the timeframe as well as a <see cref="TenorTimeSpan" /> representation of the time span between both dates.
        /// </summary>
        /// <param name="referenceDate">A reference date (can be a business day or a holiday).</param>
        /// <param name="holidayCalendar">The (settlement) holiday calendar.</param>
        /// <param name="fixingLag">The fixing lag, i.e. the number of business days taken into account for the calculation of fixing dates etc., for example <c>-2</c>.</param>
        /// <param name="startDate">The start date of the time span with respect to referenceDate (output).</param>
        /// <param name="endDate">The end date of the time span with respect to referenceDate (output).</param>
        /// <param name="tenor">The tenor that represents the time span; if the end date of the current object is not specified by its <see cref="TenorTimeSpan" />
        /// representation, <paramref name="tenorRoundingRule" /> will be applied to <paramref name="startDate" /> and <paramref name="endDate" /> for the calculation of a <see cref="TenorTimeSpan" /> representation.</param>
        /// <param name="tenorRoundingRule">A rounding rule which will be take into account if and only if the end date of the current object is not specified by its <see cref="TenorTimeSpan" /> representation.</param>
        /// <param name="logger">An optional logger.</param>
        /// <returns>The fixing date of with respect to the (interest) period.</returns>
        public DateTime GetStartAndEndDate(DateTime referenceDate, IHolidayCalendar holidayCalendar, IFixingLag fixingLag, out DateTime startDate, out DateTime endDate, out TenorTimeSpan tenor, TenorTimeSpan.RoundingRule tenorRoundingRule = TenorTimeSpan.RoundingRule.NearestMonth, ILogger logger = null)
        {
            GetStartAndEndDate(referenceDate, holidayCalendar, out startDate, out endDate, out tenor, tenorRoundingRule, logger);
            return fixingLag.GetFixingDate(startDate, holidayCalendar);
        }

        /// <summary>Gets the raw end date of the current <see cref="ITimeframeDescription"/>; where the raw end date is in general represented in a specific <see cref="TenorTimeSpan"/> or <see cref="System.DateTime"/> object.
        /// </summary>
        /// <returns>The raw end date of the current <see cref="ITimeframeDescription"/>; where the raw end date is in general represented in a specific <see cref="TenorTimeSpan"/> or <see cref="System.DateTime"/> object.</returns>
        public object GetRawEndDate()
        {
            return EndDate;
        }
        #endregion

        #region IInfoOutputQueriable Members

        /// <summary>Gets informations of the current object as a specific <see cref="T:Dodoni.BasicComponents.Containers.InfoOutput"/> instance.
        /// </summary>
        /// <param name="infoOutput">The <see cref="T:Dodoni.BasicComponents.Containers.InfoOutput"/> object which is to be filled with informations concering the current instance.</param>
        /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
        public void FillInfoOutput(InfoOutput infoOutput, string categoryName = "General")
        {
            InfoOutputPackage infoOutputCollection = infoOutput.AcquirePackage(categoryName);
            infoOutputCollection.Add("End date", EndDate);
            infoOutputCollection.Add("Spot date adjustment", SpotDateAdjustment);
            infoOutputCollection.Add("Start date adjustment", StartDateAdjustment);
            infoOutputCollection.Add("End date adjustment", EndDateAdjustment);
            infoOutputCollection.Add("Business days to settle", BusinessDaysToSettle);
        }

        /// <summary>Sets the <see cref="P:Dodoni.BasicComponents.Containers.IInfoOutputQueriable.InfoOutputDetailLevel"/> property.
        /// </summary>
        /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
        /// <returns>A value indicating whether the <see cref="P:Dodoni.BasicComponents.Containers.IInfoOutputQueriable.InfoOutputDetailLevel"/> has been set to <paramref name="infoOutputDetailLevel"/>.
        /// </returns>
        public bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
        {
            return (infoOutputDetailLevel == InfoOutputDetailLevel.Full);
        }
        #endregion

        /// <summary>Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return EndDate.ToShortDateString();
        }
        #endregion
    }
}