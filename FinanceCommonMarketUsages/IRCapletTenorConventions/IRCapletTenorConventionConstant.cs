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
    /// <summary>Represents the interest rate caplet tenor convention, where for each caplet the 
    /// tenor of the underlying Libor rate is identical, for example 3M or 6M.
    /// </summary>
    internal class IRCapletTenorConventionConstant : IIRCapletTenorConvention
    {
        #region private members

        /// <summary>The (unique) libor rate tenor in some <see cref="IDateScheduleFrequency"/> representation.
        /// </summary>
        private IDateScheduleFrequency m_LiborRateTenor;

        /// <summary>The name of the caplet tenor convention.
        /// </summary>
        private IdentifierString m_Name;

        /// <summary>The long name of the caplet tenor convention.
        /// </summary>
        private IdentifierString m_LongName;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="IRCapletTenorConventionConstant"/> class.
        /// </summary>
        /// <param name="liborRateTenor">The (unique) libor rate tenor.</param>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="liborRateTenor"/> represents a tenor leq or equal <c>0</c>.</exception>
        public IRCapletTenorConventionConstant(TenorTimeSpan liborRateTenor)
        {
            if (TenorTimeSpan.IsNull(liborRateTenor) || (liborRateTenor.IsPositive == false))
            {
                throw new ArgumentException("liborRateTenor");
            }
            m_LiborRateTenor = liborRateTenor.AsFrequency();

            m_Name = new IdentifierString(String.Format("Constant caplet tenor {0}", liborRateTenor.ToString()));
            m_LongName = new IdentifierString(String.Format(IRCapletTenorConventionResources.ConstantTenorLongName, liborRateTenor.ToString()));
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the current caplet tenor convention.
        /// </summary>
        /// <value>The language independent name of the current caplet tenor convention.</value>
        public IdentifierString Name
        {
            get { return m_Name; }
        }

        /// <summary>Gets the long name of the current caplet tenor convention.
        /// </summary>
        /// <value>The language dependent long name of the current caplet tenor convention.</value>
        public IdentifierString LongName
        {
            get { return m_LongName; }
        }
        #endregion

        #region IIRCapletTenorConvention Members

        /// <summary>Gets a value indicating whether this instance has a unique Libor tenor, i.e.
        /// each caplet is given with respect to the same Libor rate.
        /// </summary>
        /// <value><c>true</c> if this instance has unique libor tenor; otherwise, <c>false</c>.
        /// </value>
        public bool HasUniqueLiborTenor
        {
            get { return true; }
        }
        #endregion

        #endregion

        #region public methods

        #region IIRCapletTenorConvention Members

        /// <summary>Gets the tenor of the underlying Libor rate if the tenor is equal for each caplet.
        /// </summary>
        /// <param name="liborTenor">The (unique) tenor of the underlying Libor rate.</param>
        /// <returns>
        /// A value indicating whether <paramref name="liborTenor"/> contains valid data
        /// and a unique tenor is available.
        /// </returns>
        /// <remarks>This method returns the same value as <see cref="IIRCapletTenorConvention.HasUniqueLiborTenor"/>.</remarks>
        public bool TryGetUniqueUnderlyingLiborTenor(out TenorTimeSpan liborTenor)
        {
            liborTenor = m_LiborRateTenor.GetFrequencyTenor();
            return true;
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
        /// <returns>The date schedule of the interest periods, i.e. the start and end dates of each caplet; thus T_0, T_1, T_2, ..., where [T_k; T_{k+1}] is the
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
            DateSchedule dateSchedule = new DateSchedule(holidayCalendar, logger: logger);
            dateSchedule.Add(new ForwardDateScheduleRule(referenceDate, startDateAndEndDateDescription, m_LiborRateTenor, marketConventions.BusinessDayConvention));

            underlyingLiborTenor = (i => m_LiborRateTenor.GetFrequencyTenor());
            return dateSchedule.AsReadOnly();
        }
        #endregion

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return m_Name.String;
        }
        #endregion
    }
}