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

namespace Dodoni.Finance.CommonMarketUsages.HolidayCalendars
{
    /// <summary>Represents the 'dummy' holiday calendar, where each day is some business day, i.e./ the set of (public) holidays is empty.
    /// </summary>
    internal class EmptyHolidayCalendar : IHolidayCalendar
    {
        #region public static (readonly) members

        /// <summary>The name of the holiday calendar in its <see cref="IdentifierString"/> representation.
        /// </summary>
        public static readonly IdentifierString Name = new IdentifierString("None");
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="EmptyHolidayCalendar"/> class.
        /// </summary>
        public EmptyHolidayCalendar()
        {
        }
        #endregion

        #region public properties

        #region IAnnotatable Members

        /// <summary>Gets a value indicating whether the annotation is readonly.
        /// </summary>
        /// <value>
        /// <c>true</c> if the annotation of this instance is readonly; otherwise, <c>false</c>.
        /// </value>
        bool IAnnotatable.HasReadOnlyAnnotation
        {
            get { return true; }
        }

        /// <summary>Gets the annotation of the holiday calendar.
        /// </summary>
        /// <value>The annotation of the holiday calendar.</value>
        public string Annotation
        {
            get { return Resources.EmptyCalendarDescription; }
        }
        #endregion

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the holiday calendar.
        /// </summary>
        /// <value>The name of the holiday calendar.</value>
        IdentifierString IIdentifierNameable.Name
        {
            get { return EmptyHolidayCalendar.Name; }
        }

        /// <summary>Gets the long name of the holiday calendar.
        /// </summary>
        /// <value>The long name of the holiday calendar.</value>
        IdentifierString IIdentifierNameable.LongName
        {
            get { return EmptyHolidayCalendar.Name; }
        }
        #endregion

        #region IHolidayCalendar Members

        /// <summary>Gets the representation of the weekend, i.e. a value indicating which days represents the weekend.
        /// </summary>
        /// <value>The representation of the weekend.</value>
        public IWeekendRepresentation WeekendRepresentation
        {
            get { return WeekendFactory.EmptyWeekend; }
        }

        /// <summary>Gets the region of the holiday calendar.
        /// </summary>
        /// <value>The region with respect to the holiday calendar.</value>
        /// <remarks>This property returns <see cref="HolidayCalendarRegion.Unspecified"/>.</remarks>
        public HolidayCalendarRegion Region
        {
            get { return HolidayCalendarRegion.Unspecified; }
        }

        /// <summary>Gets the earliest date for which holiday informations are available.
        /// </summary>
        /// <value>The first valid date with respect to the given calendar.</value>
        public DateTime FirstDate
        {
            get { return DateTime.MinValue; }
        }

        /// <summary>Gets the latest date for which holiday informations are available.
        /// </summary>
        /// <value>The last valid date with respect to the given calendar.</value>
        public DateTime LastDate
        {
            get { return DateTime.MaxValue; }
        }
        #endregion

        #endregion

        #region public methods

        #region IAnnotatable Members

        /// <summary> Sets the annotation of the current instance.
        /// </summary>
        /// <param name="annotation">The annotation.</param>
        /// <returns>A value indicating whether the <see cref="Annotation"/> has been changed.
        /// </returns>
        bool IAnnotatable.TrySetAnnotation(string annotation)
        {
            return false;
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
            if (holidaySet == null)
            {
                throw new ArgumentNullException("holidaySet");
            }
            // nothing to do, no holidays available
        }

        /// <summary>Gets the public holidays for a specified year.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="holidaySet">A set of public holidays with respect to <paramref name="year"/>; dates that falls on the the weekend (i.e. Saturday's and sunday's for the 'standard' weekend) 
        /// will not be added, given elements remain unchanged (output).</param>
        /// <exception cref="NullReferenceException">Thrown, if <paramref name="holidaySet"/> is <c>null</c>.</exception>
        public void GetHolidays(int year, HashSet<DateTime> holidaySet)
        {
            if (holidaySet == null)
            {
                throw new ArgumentNullException("holidaySet");
            }
            // nothing to do, no holidays available
        }

        /// <summary>Determines whether a specified date is a business day.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns><c>true</c> if <paramref name="date"/> is a business day with respect to the current instance; otherwise, <c>false</c>.
        /// </returns>
        public bool IsBusinessDay(DateTime date)
        {
            return true;
        }

        /// <summary>Gets the business day which comes next to a specific date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>If <paramref name="date"/> is some business day, this date will be returned; otherwise the next business day will be returned.
        /// </returns>
        /// <remarks>No business day convention will be taken into account.</remarks>
        public DateTime GetForwardAdjustedBusinessDay(DateTime date)
        {
            return date;
        }

        /// <summary>Gets the business day which is the business day before a specified date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>If <paramref name="date"/> is some business day, this date will be returned; otherwise the business day
        /// before <paramref name="date"/>will be returned.
        /// </returns>
        /// <remarks>No business day convention will be taken into account.</remarks>
        public DateTime GetPreviousAdjustedBusinessDay(DateTime date)
        {
            return date;
        }

        /// <summary>Adds a number of business days to a specified business day.
        /// </summary>
        /// <param name="businessDay">The business day to add a number of business days.</param>
        /// <param name="numberOfBusinessDays">The number of business days to add to <paramref name="businessDay"/>, could be negative.</param>
        /// <returns>The business day that is given by <paramref name="businessDay"/> plus the <paramref name="numberOfBusinessDays"/>.
        /// </returns>
        /// <remarks>It will be assumed that <paramref name="businessDay"/> is some business day and this will not be checked.</remarks>
        public DateTime AddBusinessDays(DateTime businessDay, int numberOfBusinessDays)
        {
            return businessDay.AddDays(numberOfBusinessDays);
        }

        /// <summary>Gets the number of business days in between, i.e. the number of business days in ]<paramref name="date1"/>, <paramref name="date2"/>]
        /// or ]<paramref name="date2"/>, <paramref name="date1"/>] <c>without</c> taking into account any business day convention.
        /// </summary>
        /// <param name="date1">The date1.</param>
        /// <param name="date2">The date2.</param>
        /// <returns>The number of business days between <paramref name="date1"/> and <paramref name="date2"/> including the latest date if this is a business day.
        /// </returns>
        public int GetNumberOfBusinessDaysInBetween(DateTime date1, DateTime date2)
        {
            if (date2 < date1)
            {
                DateTime temp = date1;
                date1 = date2;
                date2 = temp;
            }
            return date2.Subtract(date1).Days;
        }
        #endregion

        /// <summary>Converts the value of the current <see cref="IHolidayCalendar"/> object to its equivalent string representation.
        /// </summary>
        /// <returns>A <see cref="System.String"/> representation of the current instance, i.e. the name of the holiday calendar.
        /// </returns>
        public override string ToString()
        {
            return EmptyHolidayCalendar.Name.String;
        }
        #endregion
    }
}