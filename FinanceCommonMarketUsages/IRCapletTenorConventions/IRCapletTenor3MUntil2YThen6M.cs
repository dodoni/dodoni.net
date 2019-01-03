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
using Dodoni.Finance.DateFactory;
using Dodoni.BasicComponents.Logging;
using Dodoni.Finance.MarketConventionTemplates;
using Microsoft.Extensions.Logging;

namespace Dodoni.Finance.CommonMarketUsages.IRCapletTenorConventions
{
    /// <summary>Caplets with maturity strictly less than '2y' have a tenor equal
    /// to '3m'; starting in '2y' the caplet tenor is equal to '6m'.
    /// </summary>
    internal class IRCapletTenor3MUntil2YThen6M : IIRCapletTenorConvention
    {
        #region private static members

        /// <summary>The 3M-libor rate tenor in some <see cref="IDateScheduleFrequency"/> representation.
        /// </summary>
        private static IDateScheduleFrequency sm_3MLiborRateTenor = DateScheduleFrequency.Quarterly;

        /// <summary>The 6M-libor rate tenor in some <see cref="IDateScheduleFrequency"/> representation.
        /// </summary>
        private static IDateScheduleFrequency sm_6MLiborRateTenor = DateScheduleFrequency.SemiAnnually;

        /// <summary>The tenor "2Y".
        /// </summary>
        private static TenorTimeSpan sm_2YTenor = new TenorTimeSpan(2, 0, 0);

        /// <summary>The name of the caplet tenor convention.
        /// </summary>
        private static IdentifierString sm_Name = new IdentifierString("3M_Until2Y_Then6M");

        /// <summary>The long name of the caplet tenor convention.
        /// </summary>
        private static IdentifierString sm_LongName = new IdentifierString(IRCapletTenorConventionResources.Until2Y_3M_Then6MLongName);
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="IRCapletTenor3MUntil2YThen6M"/> class.
        /// </summary>
        public IRCapletTenor3MUntil2YThen6M()
        {
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the current caplet tenor convention.
        /// </summary>
        /// <value>The language independent name of the current caplet tenor convention.</value>
        public IdentifierString Name
        {
            get { return sm_Name; }
        }

        /// <summary>Gets the long name of the current caplet tenor convention.
        /// </summary>
        /// <value>The language dependent long name of the current caplet tenor convention.</value>
        public IdentifierString LongName
        {
            get { return sm_LongName; }
        }
        #endregion

        #region IIRCapletTenorConvention Members

        /// <summary>Gets a value indicating whether this instance has a unique Libor tenor, i.e.
        /// each caplet is given with respect to the same Libor rate.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has unique libor tenor; otherwise, <c>false</c>.
        /// </value>
        public bool HasUniqueLiborTenor
        {
            get { return false; }
        }
        #endregion

        #endregion

        #region public methods

        #region IIRCapletTenorConvention Members

        /// <summary>Gets the tenor of the underlying Libor rate if the tenor is equal for each caplet.
        /// </summary>
        /// <param name="liborTenor">The (unique) tenor of the underlying Libor rate.</param>
        /// <returns>A value indicating whether <paramref name="liborTenor"/> contains valid data
        /// and a unique tenor is available.
        /// </returns>
        public bool TryGetUniqueUnderlyingLiborTenor(out TenorTimeSpan liborTenor)
        {
            liborTenor = TenorTimeSpan.Null;
            return false;
        }

        /// <summary>Creates a date schedule that contains the start and end dates of the interest periods of each caplets, i.e. T_0, T_1, T_2, ..., where [T_k; T_{k+1}] is the
        /// interest period of caplet k, k=0,...,n-1; The first caplet is already expired but it is part of the date schedule.
        /// </summary>
        /// <param name="referenceDate">The reference date, i.e. the trading date.</param>
        /// <param name="startDateAndEndDateDescription">A description of the start date of the first caplet as well as the end date of the last caplet, i.e. the start date and the maturity of the cap.</param>
        /// <param name="marketConventions">The market conventions.</param>
        /// <param name="holidayCalendar">The holiday calendar.</param>
        /// <param name="underlyingLiborTenor">A mapping of the null-based index of the start date of each caplet interest period to the tenor of the underlying Libor rate (output).</param>
        /// <param name="logger">An optional logger.</param>
        /// <returns>
        /// The date schedule of the interest periods, i.e. the start and end dates of each caplet; thus T_0, T_1, T_2, ..., where [T_k; T_{k+1}] is the
        /// interest period of caplet k, k=0,...,n-1; The first caplet is already expired but it is part of the date schedule.
        /// </returns>
        public ReadOnlyDateSchedule CreateInterestPeriodDateSchedule(DateTime referenceDate, ITimeframeDescription startDateAndEndDateDescription, ReadOnlyMoneyMarketConventions marketConventions, IHolidayCalendar holidayCalendar, out Func<int, TenorTimeSpan> underlyingLiborTenor, ILogger logger = null)
        {
            if (marketConventions == null)
            {
                throw new ArgumentNullException("marketConventions");
            }
            if (holidayCalendar == null)
            {
                throw new ArgumentNullException("holidayCalendar");
            }
            IBusinessDayConvention businessDayConvention = marketConventions.BusinessDayConvention;

            DateTime capStartDate, capEndDate;
            startDateAndEndDateDescription.GetStartAndEndDate(referenceDate, holidayCalendar, out capStartDate, out capEndDate);

            DateTime lastDate3M = businessDayConvention.GetAdjustedDate(capStartDate.AddTenorTimeSpan(sm_2YTenor), holidayCalendar); // it is the end date of the latest caplet with tenor 3M

            DateSchedule dateSchedule = new DateSchedule(holidayCalendar, logger: logger);
            if (capEndDate <= lastDate3M)
            {
                dateSchedule.Add(new ForwardDateScheduleRule(referenceDate, startDateAndEndDateDescription, sm_3MLiborRateTenor, businessDayConvention));
                underlyingLiborTenor = (i => sm_3MLiborRateTenor.GetFrequencyTenor());
            }
            else
            {
                ITimeframeDescription firstPeriod3M = TimeframeDescription.Create(lastDate3M);  // applied to reference date='capStartDate' shows [capStartDate; lastDate3M]
                dateSchedule.Add(new ForwardDateScheduleRule(capStartDate, firstPeriod3M, sm_3MLiborRateTenor, businessDayConvention));
                int indexOfLast3MPeriodEndDate = dateSchedule.Count - 1;

                ITimeframeDescription secondPeriod6M = TimeframeDescription.Create(capEndDate, endDateAdjustment: businessDayConvention); // applied to reference date ='lastDate3M' shows [lastDate3M; capEndDate]
                dateSchedule.Add(new ForwardDateScheduleRule(lastDate3M, secondPeriod6M, sm_6MLiborRateTenor, businessDayConvention));
                underlyingLiborTenor = (i => (i < indexOfLast3MPeriodEndDate) ? sm_3MLiborRateTenor.GetFrequencyTenor() : sm_3MLiborRateTenor.GetFrequencyTenor());
            }
            return dateSchedule.AsReadOnly();
        }
        #endregion

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return sm_Name.String;
        }
        #endregion
    }
}