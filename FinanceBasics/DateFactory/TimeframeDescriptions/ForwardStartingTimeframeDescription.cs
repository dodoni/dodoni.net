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

using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Logging;
using Dodoni.BasicComponents.Containers;
using Microsoft.Extensions.Logging;

namespace Dodoni.Finance.DateFactory.TimeframeDescriptions
{
    /// <summary>A time span, where the start date (=settlement date) as well as the end date (=maturity date) is given in its <see cref="TenorTimeSpan"/> representation, 
    /// moreover for the start date (=settlement date) a number of business days to settle will be take into account as well.
    /// <para>The implementation works in the following way:
    ///  <list type="number">
    ///   <item><description>Apply the SpotDateAdjustment to the reference date; call it spot date.</description></item>
    ///   <item><description>For regular tenor with a number of business days to settle [in short: BD-ToSettle] != 0 or tomorrow-next, point the spot date to the 
    ///   next (or previous if BD-ToSettle is lesss than 0) business day and add BD-ToSettle business days. This is the final spot date.</description></item>
    ///   <item><description>Apply the start tenor to the spot date and apply the StartDateAdjustment. This is the start date of the period.</description></item>
    ///   <item><description>Add the tenor to the start date and apply the EndDateAdjustment which gives us the end date.</description></item>
    /// </list></para>
    /// </summary>
    internal class ForwardStartingTimeframeDescription : ITimeframeDescription
    {
        #region public (readonly) members

        /// <summary>The business day convention used to compute the spot date, i.e. reference date + a number of business days to settle (if <see cref="StartDateTenor"/> is regular).
        /// </summary>
        public readonly IBusinessDayConvention SpotDateAdjustment;

        /// <summary>The business day convention used to compute the start date.
        /// </summary>
        public readonly IBusinessDayConvention StartDateAdjustment;

        /// <summary>The business day convention used to compute the end date.
        /// </summary>
        public readonly IBusinessDayConvention EndDateAdjustment;

        /// <summary>The tenor with respect to the start date of the time span.
        /// </summary>
        public readonly TenorTimeSpan StartDateTenor;

        /// <summary>The tenor that represents the time span.
        /// </summary>
        public readonly TenorTimeSpan Tenor;

        /// <summary>The number of business days to settle which will be taken into account for the spot date calculation if and only if <see cref="StartDateTenor"/> is regular.
        /// </summary>
        public readonly int BusinessDaysToSettle;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="ForwardStartingTimeframeDescription"/> class.
        /// </summary>
        /// <param name="startDateTenor">The tenor use to calculate the start date.</param>
        /// <param name="tenor">The tenor that represents the time span.</param>
        /// <param name="spotDateAdjustment">A business day convention used to compute the spot date, i.e. reference date + a number of business days to settle (if <paramref name="startDateTenor"/> is regular).</param>
        /// <param name="startDateAdjustment">A business day convention used to compute the start date of the period.</param>
        /// <param name="endDateAdjustment">A business day convention used to compute the end date of the period.</param>
        /// <param name="businessDaysToSettle">The number of business days used to calculate the spot date; this parameter will be taken into if and only if <paramref name="startDateTenor"/> is a regular tenor (i.e. neigther overnight nor tomorrow-next).</param>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="startDateTenor"/> or <paramref name="tenor"/> represents a negative time span or <paramref name="tenor"/> does not represents a regular tenor (<see cref="TenorType.RegularTenor"/>).</exception>
        /// <exception cref="ArgumentNullException">Thrown, if one of the <see cref="IBusinessDayConvention"/> arguments is <c>null</c>.</exception>
        public ForwardStartingTimeframeDescription(TenorTimeSpan startDateTenor, TenorTimeSpan tenor, IBusinessDayConvention spotDateAdjustment, IBusinessDayConvention startDateAdjustment, IBusinessDayConvention endDateAdjustment, int businessDaysToSettle = 0)
        {
            if (startDateTenor.IsPositive == false)
            {
                throw new ArgumentException(String.Format(ExceptionMessages.ArgumentIsInvalid, startDateTenor.ToString()), "startDateTenor");
            }
            StartDateTenor = startDateTenor;
            if ((tenor.IsPositive == false) || (tenor.TenorType != TenorType.RegularTenor))  // ON, TN are not allowed
            {
                throw new ArgumentException(String.Format(ExceptionMessages.ArgumentIsInvalid, tenor.ToString()), "tenor");
            }
            Tenor = tenor;
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
        /// <value>The info-output level of detail.</value>
        public InfoOutputDetailLevel InfoOutputDetailLevel
        {
            get { return InfoOutputDetailLevel.Full; }
        }
        #endregion

        #endregion

        #region public methods

        #region ITimeframeDescription Members

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

            switch (StartDateTenor.TenorType)
            {
                case TenorType.Overnight:
                    startDate = StartDateAdjustment.GetAdjustedDate(spotDate.AddDays(1), holidayCalendar);
                    break;

                case TenorType.TomorrowNext:
                    if ((SpotDateAdjustment.AdjustmentType != BusinessDayAdjustmentType.AdjustmentToBusinessDay) && (holidayCalendar.IsBusinessDay(spotDate) == false))
                    {
                        spotDate = holidayCalendar.GetForwardAdjustedBusinessDay(spotDate);
                    }
                    spotDate = holidayCalendar.AddBusinessDays(spotDate, 1);
                    startDate = StartDateAdjustment.GetAdjustedDate(spotDate.AddDays(1), holidayCalendar);
                    break;

                case TenorType.RegularTenor:
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
                        spotDate = holidayCalendar.AddBusinessDays(spotDate, BusinessDaysToSettle);
                    }
                    startDate = StartDateAdjustment.GetAdjustedDate(spotDate.AddTenorTimeSpan(StartDateTenor), holidayCalendar);
                    break;

                default:
                    throw new NotImplementedException();
            }
            endDate = EndDateAdjustment.GetAdjustedDate(startDate.AddTenorTimeSpan(Tenor), holidayCalendar);
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
            tenor = Tenor;
            GetStartAndEndDate(referenceDate, holidayCalendar, out startDate, out endDate, logger);
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
            return Tenor;
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
            infoOutputCollection.Add("Start Tenor", StartDateTenor);
            infoOutputCollection.Add("Tenor", Tenor);
            infoOutputCollection.Add("Spot date adjustment", SpotDateAdjustment);
            infoOutputCollection.Add("Start date adjustment", StartDateAdjustment);
            infoOutputCollection.Add("End date adjustment", EndDateAdjustment);
            if (StartDateTenor.TenorType == TenorType.RegularTenor)
            {
                infoOutputCollection.Add("Business days to settle", BusinessDaysToSettle);
            }
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
            return StartDateTenor.ToString() + "x" + Tenor.ToString();
        }
        #endregion
    }
}