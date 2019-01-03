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
    /// <summary>Serves as abstract basis class for a holiday calendar that is constructed with respect to a series of holiday calendar.
    /// </summary>
    internal abstract class JointHolidayCalendar : BasicHolidayCalendar, IHolidayCalendar
    {
        #region private/protected members

        /// <summary>A collection of holiday calendars.
        /// </summary>
        protected IEnumerable<IHolidayCalendar> m_HolidayCalendars;

        /// <summary>The annotation of the holiday calendar.
        /// </summary>
        private string m_Annotation;
        #endregion

        #region protected constructors

        /// <summary>Initializes a new instance of the <see cref="JointHolidayCalendar"/> class.
        /// </summary>
        /// <param name="calendarName">The name of the calendar.</param>
        /// <param name="holidayCalendarCollection">A collection of <see cref="IHolidayCalendar"/> objects.</param>
        /// <param name="weekendRepresentation">The representation of the weekend.</param>
        /// <exception cref="ArgumentNullException">Thrown, if one of the arguments is <c>null</c>.</exception>
        protected JointHolidayCalendar(IdentifierString calendarName, IEnumerable<IHolidayCalendar> holidayCalendarCollection, IWeekendRepresentation weekendRepresentation)
            : base(calendarName, GetRegion(holidayCalendarCollection), GetFirstDate(holidayCalendarCollection), GetLastDate(holidayCalendarCollection), weekendRepresentation)
        {
            if (holidayCalendarCollection == null)
            {
                throw new ArgumentNullException("holidayCalendarCollection");
            }
            m_HolidayCalendars = holidayCalendarCollection;
            m_Annotation = GetAnnotation();
        }

        /// <summary>Initializes a new instance of the <see cref="JointHolidayCalendar"/> class.
        /// </summary>
        /// <param name="calendarName">The name of the calendar.</param>
        /// <param name="holidayCalendar1">The first holiday calendar.</param>
        /// <param name="holidayCalendar2">The second holiday calendar.</param>
        /// <param name="weekendRepresentation">The representation of the weekend.</param>
        /// <exception cref="ArgumentNullException">Thrown, if one of the arguments is <c>null</c>.</exception>
        protected JointHolidayCalendar(IdentifierString calendarName, IHolidayCalendar holidayCalendar1, IHolidayCalendar holidayCalendar2, IWeekendRepresentation weekendRepresentation)
            : base(calendarName, GetRegion(holidayCalendar1, holidayCalendar2), GetFirstDate(holidayCalendar1, holidayCalendar2), GetLastDate(holidayCalendar1, holidayCalendar2), weekendRepresentation)
        {
            if (holidayCalendar1 == null)
            {
                throw new ArgumentNullException("holidayCalendar1");
            }
            if (holidayCalendar2 == null)
            {
                throw new ArgumentNullException("holidayCalendar2");
            }
            m_HolidayCalendars = new List<IHolidayCalendar>() { holidayCalendar1, holidayCalendar2 };
            m_Annotation = GetAnnotation();
        }
        #endregion

        #region public properties

        #region IAnnotatable Members

        /// <summary>Gets a value indicating whether the annotation is readonly.
        /// </summary>
        /// <value><c>true</c> if the annotation of this instance is readonly; otherwise, <c>false</c>.
        /// </value>
        bool IAnnotatable.HasReadOnlyAnnotation
        {
            get { return false; }
        }

        /// <summary> Gets the annotation of the holiday calendar.
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
            m_Annotation = (annotation == null) ? String.Empty : annotation;
            return true;
        }
        #endregion

        #region IHolidayCalendar Members

        /// <summary>Gets the holiday table for a specific year as a collection of <see cref="DateInfo"/> objects, where in general the language depending name of the holiday is given too.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="holidaySet">A set of <see cref="DateInfo"/> object that represents the holidays (output).</param>
        /// <param name="holidayType">The type of the holidays to take into account.</param>
        /// <exception cref="NullReferenceException">Thrown, if <paramref name="holidaySet"/> is <c>null</c>.</exception>
        public abstract void GetHolidays(int year, HashSet<DateInfo> holidaySet, HolidayType holidayType = HolidayType.PublicHolidays);

        /// <summary>Gets the public holidays for a specific year.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="holidaySet">A set of public holidays with respect to <paramref name="year"/>; dates that represents a weekend (i.e. Saturday's and sunday's for the 'standard' weekend) 
        /// will not be added, given elements remain unchanged (output).</param>
        /// <exception cref="NullReferenceException">Thrown, if <paramref name="holidaySet"/> is <c>null</c>.</exception>
        public abstract void GetHolidays(int year, HashSet<DateTime> holidaySet);
        #endregion

        #endregion

        #region protected methods

        /// <summary>Gets the number of holidays that are no weekend date (i.e. neither a Saturday nor a Sunday in the 'standard' weekend), in ]<paramref name="date1"/>, <paramref name="date2"/>]
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
            HashSet<DateTime> holidaySet = new HashSet<DateTime>();
            while (year <= date2.Year)
            {
                holidaySet.Clear();
                GetHolidays(year, holidaySet);
                foreach (DateTime holiday in holidaySet)  // linear search cause no performance problems here, because the number of holidays is small
                {
                    if ((holiday >= date1) && (holiday < date2))
                    {
                        numberOfHolidays++;
                    }
                }
                year++;
            }
            holidaySet = null;
            return numberOfHolidays;
        }

        /// <summary>Gets the annotation, i.e. builds the description of the current holiday calendar.
        /// </summary>
        /// <returns>The annotation, i.e. the description of the current holiday calendar that contains the name of each sub holiday calendar.</returns>
        protected abstract string GetAnnotation();
        #endregion

        #region private static methods

        /// <summary>Gets the region with respect to a collection of holiday calendars, i.e. the region represents the union of each single region.
        /// </summary>
        /// <param name="holidayCalendarCollection">The holiday calendar collection.</param>
        /// <returns>The region with respect to <paramref name="holidayCalendarCollection"/>, i.e. the <see cref="HolidayCalendarRegion"/>
        /// that is defined by the bitwise operation of each region.</returns>
        private static HolidayCalendarRegion GetRegion(IEnumerable<IHolidayCalendar> holidayCalendarCollection)
        {
            HolidayCalendarRegion region = HolidayCalendarRegion.Unspecified;
            if (holidayCalendarCollection != null)
            {
                foreach (IHolidayCalendar holidayCalendar in holidayCalendarCollection)
                {
                    region |= holidayCalendar.Region;
                }
            }
            return region;
        }

        /// <summary>Gets the region with respect to two holiday calendars, i.e. the region represents the union of each single region.
        /// </summary>
        /// <param name="holidayCalendar1">The first holiday calendar.</param>
        /// <param name="holidayCalendar2">The second holiday calendar.</param>
        /// <returns>The region with respect to the given holiday calendar, i.e. the <see cref="HolidayCalendarRegion"/>
        /// which is given by the bitwise operation of each region.</returns>
        private static HolidayCalendarRegion GetRegion(IHolidayCalendar holidayCalendar1, IHolidayCalendar holidayCalendar2)
        {
            HolidayCalendarRegion region = HolidayCalendarRegion.Unspecified;
            if (holidayCalendar1 != null)
            {
                region |= holidayCalendar1.Region;
            }
            if (holidayCalendar2 != null)
            {
                region |= holidayCalendar2.Region;
            }
            return region;
        }

        /// <summary>Gets the earliest date for which holiday informations are available with respect to a specific collection of <see cref="IHolidayCalendar"/> instances.
        /// </summary>
        /// <param name="holidayCalendarCollection">The holiday calendar collection.</param>
        /// <returns>The maximum of the <see cref="IHolidayCalendar.FirstDate"/> instances of <paramref name="holidayCalendarCollection"/>.</returns>
        private static DateTime GetFirstDate(IEnumerable<IHolidayCalendar> holidayCalendarCollection)
        {
            DateTime firstDate = DateTime.MinValue;
            if (holidayCalendarCollection != null)
            {
                foreach (IHolidayCalendar holidayCalendar in holidayCalendarCollection)
                {
                    if (holidayCalendar.FirstDate > firstDate)
                    {
                        firstDate = holidayCalendar.FirstDate;
                    }
                }
            }
            return firstDate;
        }

        /// <summary>Gets the earliest date for which holiday informations are available with respect to two given <see cref="IHolidayCalendar"/> instances.
        /// </summary>
        /// <param name="holidayCalendar1">The first holiday calendar.</param>
        /// <param name="holidayCalendar2">The second holiday calendar.</param>
        /// <returns>The maximum of the <see cref="IHolidayCalendar.FirstDate"/> instances with respect to the given holiday calendars.</returns>
        private static DateTime GetFirstDate(IHolidayCalendar holidayCalendar1, IHolidayCalendar holidayCalendar2)
        {
            DateTime firstDate = DateTime.MinValue;
            if ((holidayCalendar1 != null) && (holidayCalendar1.FirstDate > firstDate))
            {
                firstDate = holidayCalendar1.FirstDate;
            }
            if ((holidayCalendar2 != null) && (holidayCalendar2.FirstDate > firstDate))
            {
                firstDate = holidayCalendar2.FirstDate;
            }
            return firstDate;
        }

        /// <summary>Gets the latest date for which holiday informations are available with respect to a specific collection of <see cref="IHolidayCalendar"/> instances.
        /// </summary>
        /// <param name="holidayCalendarColection">The holiday calendar collection.</param>
        /// <returns>The minimum of the <see cref="IHolidayCalendar.LastDate"/> instances of <paramref name="holidayCalendarColection"/>.</returns>
        private static DateTime GetLastDate(IEnumerable<IHolidayCalendar> holidayCalendarColection)
        {
            DateTime lastDate = DateTime.MaxValue;
            foreach (IHolidayCalendar holidayCalendar in holidayCalendarColection)
            {
                if (holidayCalendar.LastDate < lastDate)
                {
                    lastDate = holidayCalendar.LastDate;
                }
            }
            return lastDate;
        }

        /// <summary>Gets the latest date for which holiday informations are available with respect to two <see cref="IHolidayCalendar"/> instances.
        /// </summary>
        /// <param name="holidayCalendar1">The first holiday calendar.</param>
        /// <param name="holidayCalendar2">The second holiday calendar.</param>
        /// <returns>The minimum of the <see cref="IHolidayCalendar.LastDate"/> instances with respect to the given holiday calendars.</returns>
        private static DateTime GetLastDate(IHolidayCalendar holidayCalendar1, IHolidayCalendar holidayCalendar2)
        {
            DateTime lastDate = DateTime.MaxValue;
            if ((holidayCalendar1 != null) && (holidayCalendar1.LastDate < lastDate))
            {
                lastDate = holidayCalendar1.LastDate;
            }
            if ((holidayCalendar2 != null) && (holidayCalendar2.LastDate < lastDate))
            {
                lastDate = holidayCalendar2.LastDate;
            }
            return lastDate;
        }
        #endregion
    }
}