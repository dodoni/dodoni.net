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
using System.Linq;
using System.Collections.Generic;

using Dodoni.Finance;
using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Utilities;

namespace Dodoni.Finance.CommonMarketUsages.HolidayCalendars
{
    /// <summary>Represents a holiday calendar which is given via a list of <see cref="IHoliday"/> instances, 
    /// i.e. the holidays are perhaps not directly given by fixed dates.
    /// </summary>
    internal class CustomHolidayCalendar : BasicHolidayCalendar, IHolidayCalendar
    {
        #region public (readonly) members

        /// <summary>The (raw) holiday collection, perhaps <c>null</c>.
        /// </summary>
        public readonly IEnumerable<IHoliday> HolidayCollection;
        #endregion

        #region private members

        /// <summary>Contains the holidays (not the weekend days) to take into account. This member represents the cache for the <see cref="System.DateTime"/> representation
        /// of the holidays.
        /// </summary>
        /// <remarks>The key corresponds to the year and the value corresponds to the set of holidays (not weekend days) with respect to the key (= year).</remarks>
        private Dictionary<int, HashSet<DateTime>> m_HolidayDateTimeList = new Dictionary<int, HashSet<DateTime>>();

        /// <summary>The annotation of the holiday calendar.
        /// </summary>
        private string m_Annotation;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="CustomHolidayCalendar"/> class.
        /// </summary>
        /// <param name="calendarName">The name of the calendar.</param>
        /// <param name="region">The region with respect to the holiday calendar.</param>
        /// <param name="holidayCollection">The reference (shallow copy) of the holiday collection.</param>
        /// <param name="firstDate">The earliest date for which holiday informations are available.</param>
        /// <param name="lastDate">The lastest date for which holiday informations are available.</param>
        /// <param name="annotation">The (optional) annotation, i.e. description, of the holiday calendar.</param>
        /// <param name="weekendRepresentation">The weekend representation, if <c>null</c> Saturdays and Sundays are always non-working days.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="calendarName"/> or <paramref name="holidayCollection"/> is <c>null</c>.</exception>
        public CustomHolidayCalendar(IdentifierString calendarName, HolidayCalendarRegion region, IEnumerable<IHoliday> holidayCollection, DateTime firstDate, DateTime lastDate, string annotation = null, IWeekendRepresentation weekendRepresentation = null)
            : base(calendarName, region, firstDate, lastDate, (weekendRepresentation == null) ? WeekendFactory.StandardWeekend : weekendRepresentation)
        {
            if (holidayCollection == null)
            {
                throw new ArgumentNullException("holidayCollection");
            }
            HolidayCollection = holidayCollection;
            m_Annotation = (annotation == null) ? String.Empty : annotation;
        }
        #endregion

        #region public properties

        #region IAnnotatable Members

        /// <summary>Gets a value indicating whether the annotation is readonly.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the annotation of this instance is readonly; otherwise, <c>false</c>.
        /// </value>
        bool IAnnotatable.HasReadOnlyAnnotation
        {
            get { return false; }
        }

        /// <summary>Gets the annotation of the holiday calendar.
        /// </summary>
        /// <value>The description.</value>
        public string Annotation
        {
            get { return m_Annotation; }
        }
        #endregion

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the holiday calendar.
        /// </summary>
        /// <value>The name of the holiday calendar.</value>
        IdentifierString IIdentifierNameable.Name
        {
            get { return Name; }
        }

        /// <summary>Gets the long name of the holiday calendar.
        /// </summary>
        /// <value>The long name of the holiday calendar.</value>
        /// <remarks>The long name of the holiday calendar is equal to the <see cref="IIdentifierNameable.Name"/> 
        /// instance, i.e. not language dependent.</remarks>
        IdentifierString IIdentifierNameable.LongName
        {
            get { return Name; }
        }
        #endregion

        #region IHolidayCalendar Members

        /// <summary>Gets the region of the holiday calendar.
        /// </summary>
        /// <value>The region with respect to the holiday calendar.</value>
        HolidayCalendarRegion IHolidayCalendar.Region
        {
            get { return base.Region; }
        }

        /// <summary>Gets the earliest date for which holiday informations are available.
        /// </summary>
        /// <value>The first valid date with respect to the given calendar.</value>
        DateTime IHolidayCalendar.FirstDate
        {
            get { return FirstDate; }
        }

        /// <summary>Gets the latest date for which holiday informations are available.
        /// </summary>
        /// <value>The last valid date with respect to the given calendar.</value>
        DateTime IHolidayCalendar.LastDate
        {
            get { return LastDate; }
        }
        #endregion

        #endregion

        #region public methods

        #region IAnnotatable Members

        /// <summary>Sets the annotation of the holiday calendar.
        /// </summary>
        /// <param name="annotation">The annotation.</param>
        /// <returns>A value indicating whether the <see cref="Annotation"/> has been changed.
        /// </returns>
        public bool TrySetAnnotation(string annotation)
        {
            m_Annotation = (annotation == null) ? string.Empty : annotation;
            return true;
        }
        #endregion

        #region IHolidayCalendar Members

        /// <summary>Gets the holidays for a specific year as a collection of <see cref="DateInfo"/> instances, where
        /// in general the language depending name of the holiday is given too.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="holidaySet">A set of <see cref="DateInfo"/> instances to add the holidays of the <paramref name="year"/> (output).</param>
        /// <param name="holidayType">The type of the holidays to take into account.</param>
        /// <exception cref="NullReferenceException">Thrown, if <paramref name="holidaySet"/> is <c>null</c>.</exception>
        public void GetHolidays(int year, HashSet<DateInfo> holidaySet, HolidayType holidayType = HolidayType.PublicHolidays)
        {
            /* 1. insert weekends into the holiday table: */
            if ((holidayType & HolidayType.Weekends) == HolidayType.Weekends)
            {
                WeekendRepresentation.GetWeekendDateInfos(year, holidaySet);
            }
            /* 2.) insert public holidays into the holiday table: */
            if ((holidayType & HolidayType.PublicHolidays) == HolidayType.PublicHolidays)
            {
                SortedList<DateTime, IHoliday> rollingHolidaysOnWeekend = new SortedList<DateTime, IHoliday>();

                /* we assume, that the holidays are pairwise distinct: 
                 * A. step: insert the (public) holidays which are not on saturday/sunday and remember the holidays 
                 *           which are on a saturday/sunday if this holiday will move to another day.
                 */
                foreach (IHoliday holiday in HolidayCollection)
                {
                    DateInfo holidayDateInfo;
                    if (holiday.TryGetValue(year, out holidayDateInfo))
                    {
                        if ((holidayDateInfo.DayOfWeek != DayOfWeek.Saturday) && (holidayDateInfo.DayOfWeek != DayOfWeek.Sunday))
                        {
                            holidaySet.Add(holidayDateInfo);
                        }
                        else
                        {
                            /* replace the saturday/sunday by this holiday - this is just for presentation reason */
                            if (holidaySet.Contains(holidayDateInfo))
                            {
                                holidaySet.Remove(holidayDateInfo);  // this is a trick, 'Remove' checkes the date-component only!
                            }
                            holidaySet.Add(holidayDateInfo);  // the string component changes only!

                            if (holiday.HolidayRollingType != HolidayRollingType.NoRolling)  /* one holiday on some weekend day and the holiday 'moves' */
                            {
                                rollingHolidaysOnWeekend.Add(holidayDateInfo.Date, holiday);
                            }
                        }
                    }
                }
                /* B. step: move holidays which are on some saturday/sunday and the rolling-flag is set to the next or previous
                 *          non-holiday. This will be done iterative because of the case when two holidays are one after the other.*/
                while (rollingHolidaysOnWeekend.Count != 0)
                {
                    DateTime holidayDate = rollingHolidaysOnWeekend.Keys[0];

                    int numberOfDaysToAdd = (int)rollingHolidaysOnWeekend.Values[0].HolidayRollingType;  // = 1 for 'forward' and -1 for 'backward'
                    while ((holidayDate.DayOfWeek == DayOfWeek.Saturday) || (holidayDate.DayOfWeek == DayOfWeek.Sunday) || (holidaySet.Contains(new DateInfo(holidayDate))))
                    {
                        holidayDate = holidayDate.AddDays(numberOfDaysToAdd);
                    }
                    holidaySet.Add(new DateInfo(holidayDate, rollingHolidaysOnWeekend.Values[0].Name + "*"));
                    rollingHolidaysOnWeekend.RemoveAt(0);
                }
            }
        }

        /// <summary>Gets the public holidays for a specified year.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="holidaySet">A set of public holidays with respect to <paramref name="year"/>; dates that falls on the the weekend (i.e. Saturday's and sunday's for the 'standard' weekend) 
        /// will not be added, given elements remain unchanged (output).</param>
        /// <exception cref="NullReferenceException">Thrown, if <paramref name="holidaySet"/> is <c>null</c>.</exception>
        public void GetHolidays(int year, HashSet<DateTime> holidaySet)
        {
            SortedList<DateTime, IHoliday> rollingHolidaysOnWeekend = new SortedList<DateTime, IHoliday>();

            /* we assume, that the holidays are pairwise distinct: 
             * A. step: insert the (public) holidays which are not on saturday/sunday and remember the holidays 
             *           which are on a saturday/sunday if this holiday will move to another day.
             */
            foreach (IHoliday holiday in HolidayCollection)
            {
                DateTime holidayDateComponent;
                if (holiday.TryGetValue(year, out holidayDateComponent))
                {
                    if ((holidayDateComponent.DayOfWeek != DayOfWeek.Saturday) && (holidayDateComponent.DayOfWeek != DayOfWeek.Sunday))
                    {
                        holidaySet.Add(holidayDateComponent);
                    }
                    else
                    {
                        if (holiday.HolidayRollingType != HolidayRollingType.NoRolling)  /* one holiday on some weekend day and the holiday 'moves' */
                        {
                            rollingHolidaysOnWeekend.Add(holidayDateComponent.Date, holiday);
                        }
                    }
                }
            }
            /* B. step: move holidays which are on some saturday/sunday and the rolling-flag is set to the next or previous
             *          non-holiday. This will be done iterative because of the case when two holidays are one after the other.*/
            while (rollingHolidaysOnWeekend.Count != 0)
            {
                DateTime holidayDate = rollingHolidaysOnWeekend.Keys[0];

                int numberOfDaysToAdd = 1;  // = 1 for 'forward' and -1 for 'backward'
                switch (rollingHolidaysOnWeekend.Values[0].HolidayRollingType)
                {
                    case HolidayRollingType.BackwardRolling:
                        numberOfDaysToAdd = -1;
                        break;

                    case HolidayRollingType.ForwardRolling:
                        numberOfDaysToAdd = 1;
                        break;

                    case HolidayRollingType.NoRolling:
                        numberOfDaysToAdd = 0;
                        break;

                    case HolidayRollingType.SundayToMondaySaturdayToFriday:
                        if (holidayDate.DayOfWeek == DayOfWeek.Saturday)
                        {
                            numberOfDaysToAdd = -1;  // move to Friday
                        }
                        else
                        {
                            numberOfDaysToAdd = 1;   // move to Monday
                        }
                        break;
                    default: throw new NotImplementedException(rollingHolidaysOnWeekend.Values[0].HolidayRollingType.ToFormatString());
                }

                while ((holidayDate.DayOfWeek == DayOfWeek.Saturday) || (holidayDate.DayOfWeek == DayOfWeek.Sunday) || (holidaySet.Contains(holidayDate)))
                {
                    holidayDate = holidayDate.AddDays(numberOfDaysToAdd);
                }
                holidaySet.Add(holidayDate);
                rollingHolidaysOnWeekend.RemoveAt(0);
            }
        }

        /// <summary>Determines whether some specified date is a business day.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>
        /// 	<c>true</c> if <paramref name="date"/> is a business day with respect to the current instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown, if for the <paramref name="date"/> the current
        /// calendar is not defined, i.e. <paramref name="date"/> is less than <see cref="IHolidayCalendar.FirstDate"/>
        /// or greater than <see cref="IHolidayCalendar.LastDate"/>.</exception>
        public override bool IsBusinessDay(DateTime date)
        {
            if ((date < FirstDate) || (date > LastDate))
            {
                throw new ArgumentException(String.Format("'IsBusinessDay' fails for date {0}, the holiday calendar {1} is not defined for year {2}.", date.ToString("d"), Name.String, date.Year), "date");
            }
            if ((date.DayOfWeek == DayOfWeek.Saturday) || (date.DayOfWeek == DayOfWeek.Sunday))
            {
                return false;
            }
            int year = date.Year;
            if (!m_HolidayDateTimeList.ContainsKey(year))
            {
                HashSet<DateTime> holidaySet = new HashSet<DateTime>();
                GetHolidays(year, holidaySet);
                m_HolidayDateTimeList.Add(year, holidaySet);
            }
            return !m_HolidayDateTimeList[year].Contains(date);
        }
        #endregion

        #endregion

        #region protected methods

        /// <summary>Gets the number of holidays, which are neither a Saturday nor a Sunday, in ]<paramref name="date1"/>, <paramref name="date2"/>]
        /// <c>without</c> taking into account any business day convention.
        /// </summary>
        /// <param name="date1">The first date.</param>
        /// <param name="date2">The second date.</param>
        /// <returns>The number of non-weekend holidays in ]<paramref name="date1"/>, <paramref name="date2"/>]
        /// <c>without</c> taking into account any business day convention.
        /// </returns>
        protected override int GetNumberOfNonWeekendHolidaysInBetween(DateTime date1, DateTime date2)
        {
            int numberOfHolidays = 0;

            int year = date1.Year;
            while (year <= date2.Year)
            {
                // add the holidays for the current year, if desired:
                if (m_HolidayDateTimeList.ContainsKey(year) == false)
                {
                    HashSet<DateTime> holidaySet = new HashSet<DateTime>();
                    GetHolidays(year, holidaySet);
                    m_HolidayDateTimeList.Add(year, holidaySet);
                }
                foreach (DateTime holiday in m_HolidayDateTimeList[year])   // linear search cause no performance problems here, because the number of holidays is small
                {
                    if ((holiday >= date1) && (holiday < date2))
                    {
                        numberOfHolidays++;
                    }
                }
                year++;
            }
            return numberOfHolidays;
        }

        /// <summary>Determines whether some specified date is a business day but do not check whether the given date is inside
        /// <see cref="IHolidayCalendar.FirstDate"/> and <see cref="IHolidayCalendar.LastDate"/>.
        /// </summary>
        /// <param name="nonWeekendDay">The date which is neither a Saturday nor a Sunday.</param>
        /// <returns>
        /// 	<c>true</c> if <paramref name="nonWeekendDay"/> is a business day with respect to the current instance; otherwise, <c>false</c>.
        /// </returns>
        protected override bool IsBusinessDayForNonWeekendDay(DateTime nonWeekendDay)
        {
            int year = nonWeekendDay.Year;
            if (!m_HolidayDateTimeList.ContainsKey(year))
            {
                HashSet<DateTime> holidaySet = new HashSet<DateTime>();
                GetHolidays(year, holidaySet);
                m_HolidayDateTimeList.Add(year, holidaySet);
            }
            return !m_HolidayDateTimeList[year].Contains(nonWeekendDay);
        }
        #endregion
    }
}