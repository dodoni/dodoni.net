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
using System.Text;
using System.Collections.Generic;

using Dodoni.Finance;
using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Utilities;

namespace Dodoni.Finance.CommonMarketUsages.HolidayCalendars
{
    /// <summary>Serves as abstract basis class for several holiday calendar templates.
    /// </summary>
    internal abstract class BasicHolidayCalendar
    {
        #region protected (readonly) members

        /// <summary>The representation of the weekend.
        /// </summary>
        protected readonly IWeekendRepresentation m_WeekendRepresentation;
        #endregion

        #region public (readonly) members

        /// <summary>The name of the holiday calendar.
        /// </summary>
        public readonly IdentifierString Name;

        /// <summary>The region of the holiday calendar.
        /// </summary>
        public readonly HolidayCalendarRegion Region;

        /// <summary>The earliest date for which holiday informations are available.
        /// </summary>
        public readonly DateTime FirstDate;

        /// <summary>The latest date for which holiday informations are available.
        /// </summary>
        public readonly DateTime LastDate;
        #endregion

        #region protected constructors

        /// <summary>Initializes a new instance of the <see cref="BasicHolidayCalendar"/> class.
        /// </summary>
        /// <param name="calendarName">The name of the calendar.</param>
        /// <param name="region">The region of the holiday calendar.</param>
        /// <param name="firstDate">The earliest date for which holiday informations are available.</param>
        /// <param name="lastDate">The lastest date for which holiday informations are available.</param>
        /// <param name="weekendRepresentation">The representation of the weekend.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="calendarName"/> or <paramref name="weekendRepresentation"/> is <c>null</c>.</exception>
        protected BasicHolidayCalendar(IdentifierString calendarName, HolidayCalendarRegion region, DateTime firstDate, DateTime lastDate, IWeekendRepresentation weekendRepresentation)
        {
            Region = region;
            if (calendarName == null)
            {
                throw new ArgumentNullException("calendarName");
            }
            Name = calendarName;
            if (firstDate < lastDate)
            {
                FirstDate = firstDate;
                LastDate = lastDate;
            }
            else
            {
                LastDate = firstDate;
                FirstDate = firstDate;
            }
            if (weekendRepresentation == null)
            {
                throw new ArgumentNullException("weekendRepresentation");
            }
            m_WeekendRepresentation = weekendRepresentation;
        }
        #endregion

        #region public properties

        /// <summary>Gets the representation of the weekend, i.e. a value indicating which days represents the weekend.
        /// </summary>
        /// <value>The representation of the weekend.</value>
        public IWeekendRepresentation WeekendRepresentation
        {
            get { return m_WeekendRepresentation; }
        }
        #endregion

        #region public methods

        /// <summary>Determines whether some specified date is a business day.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns><c>true</c> if <paramref name="date"/> is a business day with respect to the current instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown, if for the <paramref name="date"/> the current calendar is not defined, i.e. <paramref name="date"/> 
        /// is less than <see cref="FirstDate"/> or greater than <see cref="LastDate"/>.</exception>
        public abstract bool IsBusinessDay(DateTime date);

        /// <summary>Gets the business day which comes next to some given <see cref="System.DateTime"/> instance.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>If <paramref name="date"/> is some business day, this date will be returned; otherwise the next business day will be returned.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown, if for the <paramref name="date"/> the current calendar is not defined, i.e. <paramref name="date"/> is less than <see cref="FirstDate"/>
        /// or greater than <see cref="LastDate"/>.</exception>
        /// <remarks>No business day convention will be taken into account.</remarks>
        public DateTime GetForwardAdjustedBusinessDay(DateTime date)
        {
            bool isBusinessDay = IsBusinessDay(date);

            while (isBusinessDay == false)
            {
                date = m_WeekendRepresentation.GetNextWorkingDay(date);
                isBusinessDay = IsBusinessDayForNonWeekendDay(date);
            }
            return date;
        }

        /// <summary>Gets the business day which is the business day before some given <see cref="System.DateTime"/> instance.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>If <paramref name="date"/> is some business day, this date will be returned; otherwise the business day before <paramref name="date"/>will be returned.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown, if for the <paramref name="date"/> the current calendar is not defined, i.e. <paramref name="date"/> is less than <see cref="FirstDate"/>
        /// or greater than <see cref="LastDate"/>.</exception>
        /// <remarks>No business day convention will be taken into account.</remarks>
        public DateTime GetPreviousAdjustedBusinessDay(DateTime date)
        {
            bool isBusinessDay = IsBusinessDay(date);

            while (isBusinessDay == false)
            {
                date = m_WeekendRepresentation.GetPreviousWorkingDay(date);
                isBusinessDay = IsBusinessDayForNonWeekendDay(date);
            }
            return date;
        }

        /// <summary>Adds a number of business days to some given business day.
        /// </summary>
        /// <param name="businessDay">The business day to add a number of business days.</param>
        /// <param name="numberOfBusinessDays">The number of business days to add to <paramref name="businessDay"/>, could be negative.</param>
        /// <returns>The business day which is given by <paramref name="businessDay"/> plus the <paramref name="numberOfBusinessDays"/>.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown, if for the <paramref name="businessDay"/> the current calendar is not defined, i.e. 
        /// <paramref name="businessDay"/> is less than <see cref="FirstDate"/> or greater than <see cref="LastDate"/>.</exception>
        /// <remarks>It will be assumed that <paramref name="businessDay"/> is some business day and this will not be checked.</remarks>
        public DateTime AddBusinessDays(DateTime businessDay, int numberOfBusinessDays)
        {
            int runningNumberOfBusinessDays = 0;

            if (numberOfBusinessDays > 0)
            {
                while (runningNumberOfBusinessDays < numberOfBusinessDays)
                {
                    businessDay = m_WeekendRepresentation.GetNextWorkingDay(businessDay);
                    if (IsBusinessDayForNonWeekendDay(businessDay))
                    {
                        runningNumberOfBusinessDays++;
                    }
                }
            }
            else if (numberOfBusinessDays < 0)
            {
                numberOfBusinessDays = -numberOfBusinessDays; // = System.Math.Abs( numberOfBusinessDays)
                while (runningNumberOfBusinessDays < numberOfBusinessDays)
                {
                    businessDay = m_WeekendRepresentation.GetPreviousWorkingDay(businessDay);
                    if (IsBusinessDayForNonWeekendDay(businessDay))
                    {
                        runningNumberOfBusinessDays++;
                    }
                }
            }
            return businessDay;
        }

        /// <summary>Gets the number of business days in between, i.e. the number of business days in ]<paramref name="date1"/>, <paramref name="date2"/>]
        /// or ]<paramref name="date2"/>, <paramref name="date1"/>] <c>without</c> taking into account any business day convention.
        /// </summary>
        /// <param name="date1">The first date.</param>
        /// <param name="date2">The second date.</param>
        /// <returns>
        /// The number of business days between <paramref name="date1"/> and <paramref name="date2"/> including the latest date if this is a business day.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown, if for <paramref name="date1"/> or <paramref name="date2"/>the current
        /// calendar is not defined, i.e. <paramref name="date1"/> or <paramref name="date2"/> is less than <see cref="FirstDate"/>
        /// or greater than <see cref="LastDate"/>.</exception>
        public int GetNumberOfBusinessDaysInBetween(DateTime date1, DateTime date2)
        {
            if (date2 < date1)
            {
                DateTime tempDateTime = date1;
                date1 = date2;
                date2 = tempDateTime;
            }
            if ((date1 < FirstDate) || (date2 > LastDate))
            {
                throw new ArgumentException("The holiday calendar " + Name.ToString() + " causes a fatal error. The date " + date1.ToString("d") + " or " + date2.ToString("d") + " lies not between the earliest date " + FirstDate.ToString("d") + " and the latest date " + LastDate.ToString("d") + " for which holiday informations are available.");
            }
            int numberOfBusinessDaysInBetween = 0;

            /* if the time difference is small, i.e. one week, we call 'IsBusinessDay', otherwise we take
             * into account the number of weekend days first and add the number of public holidays later: */
            if (date2.Subtract(date1).Days <= 7)
            {
                // for example: friday -> monday, saturday -> monday, in any other case +1 day:
                DateTime date = m_WeekendRepresentation.GetNextWorkingDay(date1);

                while (date <= date2)
                {
                    if (IsBusinessDayForNonWeekendDay(date))
                    {
                        numberOfBusinessDaysInBetween++;
                    }
                    // again: friday -> monday, saturday -> monday, in any other case +1 day:
                    date = m_WeekendRepresentation.GetNextWorkingDay(date);
                }
                return numberOfBusinessDaysInBetween;
            }
            /* count the number of days != Saturdays and Sundays [for a 'standard' weekend] first: */
            numberOfBusinessDaysInBetween = m_WeekendRepresentation.GetNumberOfWorkingDaysInBetween(date1, date2);
            return numberOfBusinessDaysInBetween - GetNumberOfNonWeekendHolidaysInBetween(date1, date2);
        }

        /// <summary>Converts the value of the current instance to its equivalent string representation.
        /// </summary>
        /// <returns>A <see cref="System.String"/> representation of the current instance, i.e. the name of the holiday calendar.
        /// </returns>
        public override string ToString()
        {
            return Name.String;
        }
        #endregion

        #region protected methods

        /// <summary>Gets the number of holidays, which are not a weekend date (i.e. neither a Saturday nor a Sunday in the 'standard' weekend), in ]<paramref name="date1"/>, <paramref name="date2"/>]
        /// <c>without</c> taking into account any business day convention.
        /// </summary>
        /// <param name="date1">The first date.</param>
        /// <param name="date2">The second date.</param>
        /// <returns>The number of non-weekend holidays in ]<paramref name="date1"/>, <paramref name="date2"/>]
        /// <c>without</c> taking into account any business day convention.
        /// </returns>
        protected abstract int GetNumberOfNonWeekendHolidaysInBetween(DateTime date1, DateTime date2);

        /// <summary>Determines whether some specified date is a business day but do not check whether the given date is inside
        /// <see cref="IHolidayCalendar.FirstDate"/> and <see cref="IHolidayCalendar.LastDate"/>.
        /// </summary>
        /// <param name="nonWeekendDay">The date which is not a weekend day (i.e. neither a Saturday nor a Sunday for the 'standard' weekend).</param>
        /// <returns><c>true</c> if <paramref name="nonWeekendDay"/> is a business day with respect to the current instance; otherwise, <c>false</c>.
        /// </returns>
        protected abstract bool IsBusinessDayForNonWeekendDay(DateTime nonWeekendDay);
        #endregion
    }
}