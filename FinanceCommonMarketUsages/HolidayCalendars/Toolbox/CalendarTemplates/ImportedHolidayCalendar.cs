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
using Dodoni.BasicComponents.Logging;
using Dodoni.BasicComponents.Utilities;
using Microsoft.Extensions.Logging;

namespace Dodoni.Finance.CommonMarketUsages.HolidayCalendars
{
    /// <summary>Represents some holiday calendar where the holidays are given as a list of <see cref="System.DateTime"/> or <see cref="DateInfo"/> instances.
    /// </summary>
    public class ImportedHolidayCalendar : IHolidayCalendar
    {
        #region private members

        /// <summary>The representation of the weekend.
        /// </summary>
        private IWeekendRepresentation m_WeekendRepresentation;

        /// <summary>A list of holidays taken into account for the calendar, where the key is the year
        /// and the value is a list of <see cref="DateInfo"/> instances which represents the holidays for
        /// the corresponding year.
        /// </summary>
        /// <remarks>Saturdays and Sundays are not stored in this list, but always taken into account.</remarks>
        private Dictionary<int, HashSet<DateInfo>> m_ListOfHolidays = new Dictionary<int, HashSet<DateInfo>>();

        /// <summary>The annotation of the imported holiday calendar, perhaps <see cref="String.Empty"/>.
        /// </summary>
        private string m_Annotation;

        /// <summary>The logger.
        /// </summary>
        private ILogger m_Logger;
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

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="ImportedHolidayCalendar"/> class.
        /// </summary>
        /// <param name="calendarName">The name of the calendar.</param>
        /// <param name="holidayCalendarRegion">The region with respect to the holiday calendar.</param>
        /// <param name="firstDate">The earliest date for which holiday informations are available.</param>
        /// <param name="lastDate">The lastest date for which holiday informations are available.</param>
        /// <param name="holidayCollection">The list of holidays.</param>
        /// <param name="listOfHolidayNames">The (optional) list of holiday names.</param>
        /// <param name="annotation">The (optional) annotation (= description) of the holiday calendar.</param>
        /// <param name="weekendRepresentation">The weekend representation, if <c>null</c> Saturdays and Sundays are always non-working days.</param>
        public ImportedHolidayCalendar(IdentifierString calendarName, HolidayCalendarRegion holidayCalendarRegion, DateTime firstDate, DateTime lastDate, IList<DateTime> holidayCollection, IList<string> listOfHolidayNames = null, string annotation = null, IWeekendRepresentation weekendRepresentation = null)
            : this(calendarName, holidayCalendarRegion, firstDate, lastDate, weekendRepresentation)
        {
            if (holidayCollection == null)
            {
                throw new ArgumentNullException("holidayCollection");
            }
            if ((listOfHolidayNames != null) && (holidayCollection.Count != listOfHolidayNames.Count))
            {
                throw new ArgumentException("The number of holidays is destinct from the number of holiday names.", "listOfHolidays");
            }
            for (int i = 0; i <= holidayCollection.Count; i++)
            {
                DateTime holiday = holidayCollection[i];
                if (m_WeekendRepresentation.Contains(holiday.DayOfWeek) == false)
                {
                    int year = holiday.Year;
                    if (m_ListOfHolidays.ContainsKey(year) == false)
                    {
                        m_ListOfHolidays.Add(year, new HashSet<DateInfo>());
                    }
                    if (listOfHolidayNames != null)
                    {
                        m_ListOfHolidays[year].Add(new DateInfo(holiday, listOfHolidayNames[i]));
                    }
                    else
                    {
                        m_ListOfHolidays[year].Add(new DateInfo(holiday));
                    }
                }
            }
            m_Annotation = (annotation == null) ? String.Empty : annotation;
        }

        /// <summary>Initializes a new instance of the <see cref="ImportedHolidayCalendar"/> class.
        /// </summary>
        /// <param name="calendarName">The name of the calendar.</param>
        /// <param name="holidayCalendarRegion">The region with respect to the holiday calendar.</param>
        /// <param name="firstDate">The earliest date for which holiday informations are available.</param>
        /// <param name="lastDate">The lastest date for which holiday informations are available.</param>
        /// <param name="listOfHolidays">The list of holidays.</param>
        /// <param name="annotation">The (optional) annotation (= description) of the holiday calendar.</param>      
        /// <param name="weekendRepresentation">The weekend representation, if <c>null</c> Saturdays and Sundays are always non-working days.</param>
        public ImportedHolidayCalendar(IdentifierString calendarName, HolidayCalendarRegion holidayCalendarRegion, DateTime firstDate, DateTime lastDate, IEnumerable<DateInfo> listOfHolidays, string annotation = null, IWeekendRepresentation weekendRepresentation = null)
            : this(calendarName, holidayCalendarRegion, firstDate, lastDate, weekendRepresentation)
        {
            if (listOfHolidays == null)
            {
                throw new ArgumentNullException("listOfHolidays");
            }
            foreach (DateInfo holiday in listOfHolidays)
            {
                if (m_WeekendRepresentation.Contains(holiday.DayOfWeek) == false)
                {
                    int year = holiday.Year;
                    if (m_ListOfHolidays.ContainsKey(year) == false)
                    {
                        m_ListOfHolidays.Add(year, new HashSet<DateInfo>());
                    }
                    m_ListOfHolidays[year].Add(holiday);
                }
            }
            m_Annotation = (annotation == null) ? String.Empty : annotation;
        }
        #endregion

        #region private constructors

        /// <summary>Initializes a new instance of the <see cref="ImportedHolidayCalendar"/> class.
        /// </summary>
        /// <param name="calendarName">The name of the calendar.</param>
        /// <param name="holidayCalendarRegion">The region of the holiday calendar.</param>
        /// <param name="firstDate">The earliest date for which holiday informations are available.</param>
        /// <param name="lastDate">The lastest date for which holiday informations are available.</param>
        /// <param name="weekendRepresentation">The representation of the weekend.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="calendarName"/> is <c>null</c>.</exception>
        private ImportedHolidayCalendar(IdentifierString calendarName, HolidayCalendarRegion holidayCalendarRegion, DateTime firstDate, DateTime lastDate, IWeekendRepresentation weekendRepresentation)
        {
            Region = holidayCalendarRegion;
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
            m_WeekendRepresentation = (weekendRepresentation == null) ? WeekendFactory.StandardWeekend : weekendRepresentation;
            // m_Logger = Logger.Stream.Create("Imported Holiday Calendar", typeof(ImportedHolidayCalendar), calendarName.String, "Holiday calendar");
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
        /// <value>The annotation of the holiday calendar.</value>
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
        /// <remarks>The long name of the holiday calendar is equal to the <see cref="IIdentifierNameable.Name"/> instance, i.e. not language dependent.</remarks>
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
            get { return Region; }
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

        /// <summary>Gets the representation of the weekend, i.e. a value indicating which days represents the weekend.
        /// </summary>
        /// <value>The representation of the weekend.</value>
        public IWeekendRepresentation WeekendRepresentation
        {
            get { return m_WeekendRepresentation; }
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
            m_Annotation = (annotation == null) ? String.Empty : annotation;
            return true;
        }
        #endregion

        #region IHolidayCalendar Members

        #region info methods

        /// <summary>Gets the holidays for a specific year as a collection of <see cref="DateInfo"/> instances, where
        /// in general the language depending name of the holiday is given too.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="holidaySet">A set of <see cref="DateInfo"/> instances to add the holidays of the <paramref name="year"/> (output).</param>
        /// <param name="holidayType">The type of the holidays to take into account.</param>
        /// <exception cref="NullReferenceException">Thrown, if <paramref name="holidaySet"/> is <c>null</c>.</exception>
        public void GetHolidays(int year, HashSet<DateInfo> holidaySet, HolidayType holidayType = HolidayType.PublicHolidays)
        {
            if ((year >= FirstDate.Year) && (year <= LastDate.Year))
            {
                /* 1.) add holidays (!= saturday/sunday - for the 'standard' weekend) into the list: */
                if ((holidayType & HolidayType.PublicHolidays) == HolidayType.PublicHolidays)
                {
                    if (m_ListOfHolidays.ContainsKey(year))
                    {
                        foreach (DateInfo date in m_ListOfHolidays[year])
                        {
                            holidaySet.Add(date);
                        }
                    }
                }

                /* 2.) add saturdays/sundays (in the case of the 'standard' weekend) into the list, if desired: */
                if ((holidayType & HolidayType.Weekends) == HolidayType.Weekends)
                {
                    m_WeekendRepresentation.GetWeekendDateInfos(year, holidaySet);
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
            if (m_ListOfHolidays.ContainsKey(year))
            {
                foreach (DateInfo holiday in m_ListOfHolidays[year])
                {
                    holidaySet.Add(holiday.DateTime);
                }
            }
        }
        #endregion

        /// <summary>Determines whether some specified date is a business day.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns><c>true</c> if <paramref name="date"/> is a business day with respect to the current instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown, if for the <paramref name="date"/> the current
        /// calendar is not defined, i.e. <paramref name="date"/> is less than <see cref="IHolidayCalendar.FirstDate"/>
        /// or greater than <see cref="IHolidayCalendar.LastDate"/>.</exception>
        public bool IsBusinessDay(DateTime date)
        {
            if ((date < FirstDate) || (date > LastDate))
            {
                throw new ArgumentException("The given calendar is not given for date " + date.ToString("d") + ".", "date");
            }
            if (m_WeekendRepresentation.Contains(date.DayOfWeek) == true)
            {
                return false;
            }
            int year = date.Year;
            if (m_ListOfHolidays.ContainsKey(year))
            {
                if (m_ListOfHolidays[year].Contains(new DateInfo(date)))
                {
                    return false;
                }
            }
            return true;
        }

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
        /// <returns>If <paramref name="date"/> is some business day, this date will be returned; otherwise the business day
        /// before <paramref name="date"/>will be returned.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown, if for the <paramref name="date"/> the current
        /// calendar is not defined, i.e. <paramref name="date"/> is less than <see cref="FirstDate"/>
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
        /// <exception cref="ArgumentException">Thrown, if for the <paramref name="businessDay"/> the current
        /// calendar is not defined, i.e. <paramref name="businessDay"/> is less than <see cref="FirstDate"/>
        /// or greater than <see cref="LastDate"/>.</exception>
        /// <remarks>It will be assumed that <paramref name="businessDay"/> is some business day and this
        /// will not be checked.</remarks>
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
        #endregion

        /// <summary>Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance, i.e. the name of the holiday calendar.
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
        protected int GetNumberOfNonWeekendHolidaysInBetween(DateTime date1, DateTime date2)
        {
            int numberOfHolidays = 0;

            int year = date1.Year;
            while (year <= date2.Year)
            {
                if (m_ListOfHolidays.ContainsKey(year) == true)
                {
                    foreach (DateInfo holiday in m_ListOfHolidays[year])   // linear search cause no performance problems here, because the number of holidays is small
                    {
                        if ((holiday.DateTime > date1) && (holiday.DateTime <= date2))
                        {
                            numberOfHolidays++;
                        }
                    }
                }
                year++;
            }
            return numberOfHolidays;
        }

        /// <summary>Determines whether some specified date is a business day but do not check whether the given date is inside
        /// <see cref="IHolidayCalendar.FirstDate"/> and <see cref="IHolidayCalendar.LastDate"/>.
        /// </summary>
        /// <param name="nonWeekendDay">The date which is not a weekend day (i.e. neither a Saturday nor a Sunday for the 'standard' weekend).</param>
        /// <returns><c>true</c> if <paramref name="nonWeekendDay"/> is a business day with respect to the current instance; otherwise, <c>false</c>.
        /// </returns>
        protected bool IsBusinessDayForNonWeekendDay(DateTime nonWeekendDay)
        {
            int year = nonWeekendDay.Year;
            if (m_ListOfHolidays.ContainsKey(year))
            {
                return !m_ListOfHolidays[year].Contains(new DateInfo(nonWeekendDay));
            }
            return true;
        }
        #endregion
    }
}