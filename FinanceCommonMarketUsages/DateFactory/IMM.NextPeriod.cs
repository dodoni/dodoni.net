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

namespace Dodoni.Finance.DateFactory
{
    public partial class IMM
    {
        /// <summary>Represents a time span where the starting date of the period is a IMM (International Monetary Market) date, i.e., the third Wednesday of March, June, September and December (i.e., between the 15th and 21st, whichever such day is a Wednesday).
        /// </summary>
        private class NextPeriod : ITimeframeDescription
        {
            #region public constructors

            /// <summary>Initializes a new instance of the <see cref="NextPeriod"/> class.
            /// </summary>
            /// <param name="tenor">The tenor that represents the time span.</param>
            /// <param name="startDateAdjustment">A business day convention used to compute the start date of the period, i.e. the IMM date.</param>
            /// <param name="endDateAdjustment">A business day convention used to compute the end date of the period.</param>
            public NextPeriod(TenorTimeSpan tenor, IBusinessDayConvention startDateAdjustment, IBusinessDayConvention endDateAdjustment)
            {
                Tenor = tenor;

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

            /// <summary>Gets the tenor that represents the time span.
            /// </summary>
            /// <value>The tenor that represents the time span.</value>
            public TenorTimeSpan Tenor
            {
                get;
                private set;
            }

            /// <summary>Gets the business day convention taken into account for the start date, i.e. IMM date, calculation.
            /// </summary>
            /// <value>The start date business day convention.</value>
            public IBusinessDayConvention StartDateAdjustment
            {
                get;
                private set;
            }

            /// <summary>Gets the business day convention taken into account for end date calculation.
            /// </summary>
            /// <value>The end date business day convention.</value>
            public IBusinessDayConvention EndDateAdjustment
            {
                get;
                private set;
            }
            #endregion

            #region public methods

            #region ITimeframeDescription Members

            /// <summary>Gets the raw end date of the current <see cref="ITimeframeDescription"/> object; where the raw end date is in general represented in a specific <see cref="Dodoni.Finance.TenorTimeSpan"/> or<see cref="System.DateTime"/> object.
            /// </summary>
            /// <returns> The raw end date of the current <see cref="ITimeframeDescription"/> object; where the raw end date is in general represented in a specific <see cref="Dodoni.Finance.TenorTimeSpan"/> or<see cref="System.DateTime"/> object.</returns>
            public object GetRawEndDate()
            {
                return Tenor;
            }

            /// <summary>Gets the start and end date of the timeframe.
            /// </summary>
            /// <param name="referenceDate">A reference date (can be a business day or a holiday).</param>
            /// <param name="holidayCalendar">The (settlement) holiday calendar.</param>
            /// <param name="startDate">The start date of the time span with respect to <paramref name="referenceDate"/> (output).</param>
            /// <param name="endDate">The end date of the time span with respect to <paramref name="referenceDate"/> (output).</param>
            /// <param name="logger">An optional logger.</param>
            public void GetStartAndEndDate(DateTime referenceDate, IHolidayCalendar holidayCalendar, out DateTime startDate, out DateTime endDate, ILogger logger = null)
            {
                var IMMDate = Next(referenceDate);

                startDate = StartDateAdjustment.GetAdjustedDate(IMMDate, holidayCalendar);
                endDate = EndDateAdjustment.GetAdjustedDate(startDate.AddTenorTimeSpan(Tenor), holidayCalendar);
            }

            /// <summary>Gets the start and end date of the timeframe.
            /// </summary>
            /// <param name="referenceDate">A reference date (can be a business day or a holiday).</param>
            /// <param name="holidayCalendar">The (settlement) holiday calendar.</param>
            /// <param name="fixingLag">The fixing lag, i.e. a method used to calculate the fixing date with respect to the period. Will be applied to the IMM Date (even if the IMM date is not a business day).</param>
            /// <param name="startDate">The start date of the time span with respect to <paramref name="referenceDate"/> (output).</param>
            /// <param name="endDate">The end date of the time span with respect to <paramref name="referenceDate"/> (output).</param>
            /// <param name="logger">An optional logger.</param>
            /// <returns>The fixing date of with respect to the (interest) period.</returns>
            public DateTime GetStartAndEndDate(DateTime referenceDate, IHolidayCalendar holidayCalendar, IFixingLag fixingLag, out DateTime startDate, out DateTime endDate, ILogger logger = null)
            {
                var IMMDate = Next(referenceDate);

                startDate = StartDateAdjustment.GetAdjustedDate(IMMDate, holidayCalendar);
                endDate = EndDateAdjustment.GetAdjustedDate(startDate.AddTenorTimeSpan(Tenor), holidayCalendar);
                return fixingLag.GetFixingDate(IMMDate, holidayCalendar);
            }

            /// <summary>Gets the start and end date of the timeframe.
            /// </summary>
            /// <param name="referenceDate">A reference date (can be a business day or a holiday).</param>
            /// <param name="holidayCalendar">The (settlement) holiday calendar.</param>
            /// <param name="startDate">The start date of the time span with respect to <paramref name="referenceDate"/> (output).</param>
            /// <param name="endDate">The end date of the time span with respect to <paramref name="referenceDate"/> (output).</param>
            /// <param name="tenor">The tenor that represents the time span; if the end date of the current object is not specified by its <see cref="TenorTimeSpan"/> representation, <paramref name="tenorRoundingRule"/> 
            /// will be applied to <paramref name="startDate"/> and <paramref name="endDate"/> for the calculation of a <see cref="TenorTimeSpan"/> representation.</param>
            /// <param name="tenorRoundingRule"> A rounding rule which will be take into account if and only if the end date of the current object is not specified by its <see cref="TenorTimeSpan"/> representation.</param>
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
            /// <param name="fixingLag">The fixing lag, i.e. a method used to calculate the fixing date with respect to the period. Will be applied to the IMM Date (even if the IMM date is not a business day).</param>
            /// <param name="startDate">The start date of the time span with respect to <paramref name="referenceDate"/> (output).</param>
            /// <param name="endDate">The end date of the time span with respect to <paramref name="referenceDate"/> (output).</param>
            /// <param name="tenor">The tenor that represents the time span; if the end date of the current object is not specified by its <see cref="TenorTimeSpan"/> representation, <paramref name="tenorRoundingRule"/> 
            /// will be applied to <paramref name="startDate"/> and <paramref name="endDate"/> for the calculation of a <see cref="TenorTimeSpan"/> representation.</param>
            /// <param name="tenorRoundingRule"> A rounding rule which will be take into account if and only if the end date of the current object is not specified by its <see cref="TenorTimeSpan"/> representation.</param>
            /// <param name="logger">An optional logger.</param>
            public DateTime GetStartAndEndDate(DateTime referenceDate, IHolidayCalendar holidayCalendar, IFixingLag fixingLag, out DateTime startDate, out DateTime endDate, out TenorTimeSpan tenor, TenorTimeSpan.RoundingRule tenorRoundingRule = TenorTimeSpan.RoundingRule.NearestMonth, ILogger logger = null)
            {
                tenor = Tenor;
                return GetStartAndEndDate(referenceDate, holidayCalendar, fixingLag, out startDate, out endDate, logger);
            }
            #endregion

            #region IInfoOutputQueriable Members

            /// <summary>Sets the <see cref="P:Dodoni.BasicComponents.Containers.IInfoOutputQueriable.InfoOutputDetailLevel"/> property.
            /// </summary>
            /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
            /// <returns>A value indicating whether the <see cref="P:Dodoni.BasicComponents.Containers.IInfoOutputQueriable.InfoOutputDetailLevel"/> has been set to <paramref name="infoOutputDetailLevel"/>.</returns>
            public bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
            {
                return (infoOutputDetailLevel == InfoOutputDetailLevel.Full);
            }

            /// <summary>Gets informations of the current object as a specific <see cref="T:Dodoni.BasicComponents.Containers.InfoOutput"/> instance.
            /// </summary>
            /// <param name="infoOutput">The <see cref="T:Dodoni.BasicComponents.Containers.InfoOutput"/> object which is to be filled with informations concering the current instance.</param>
            /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
            public virtual void FillInfoOutput(InfoOutput infoOutput, string categoryName = "General")
            {
                InfoOutputPackage infoOutputCollection = infoOutput.AcquirePackage(categoryName);
                infoOutputCollection.Add("Tenor", Tenor);
                infoOutputCollection.Add("Start date adjustment", StartDateAdjustment);
                infoOutputCollection.Add("End date adjustment", EndDateAdjustment);
            }
            #endregion

            /// <summary>Returns a <see cref="System.String"/> that represents this instance.
            /// </summary>
            /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
            public override string ToString()
            {
                return String.Format("{0} Next-IMM", Tenor);
            }
            #endregion
        }
    }
}