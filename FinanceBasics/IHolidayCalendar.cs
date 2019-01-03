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

using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Utilities;

namespace Dodoni.Finance
{
    /// <summary>Serves as interface for a holiday calendar, i.e. represents a set of holidays.
    /// </summary>
    public interface IHolidayCalendar : IIdentifierNameable, IAnnotatable
    {
        #region properties

        /// <summary>Gets the region of the holiday calendar.
        /// </summary>
        /// <value>The region with respect to the holiday calendar.</value>
        HolidayCalendarRegion Region { get; }

        /// <summary>Gets the representation of the weekend, i.e. a value indicating which days represents the weekend.
        /// </summary>
        /// <value>The representation of the weekend.</value>
        IWeekendRepresentation WeekendRepresentation { get; }

        /// <summary>Gets the earliest date for which holiday informations are available.
        /// </summary>
        /// <value>The first valid date with respect to the given calendar.</value>
        DateTime FirstDate { get; }

        /// <summary>Gets the latest date for which holiday informations are available.
        /// </summary>
        /// <value>The last valid date with respect to the given calendar.</value>
        DateTime LastDate { get; }
        #endregion

        #region methods

        #region info methods

        /// <summary>Gets the holidays for a specific year as a collection of <see cref="DateInfo"/> instances, where
        /// in general the language depending name of the holiday is given too.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="holidaySet">A set of <see cref="DateInfo"/> instances to add the holidays of the <paramref name="year"/> (output).</param>
        /// <param name="holidayType">The type of the holidays to take into account.</param>
        /// <exception cref="NullReferenceException">Thrown, if <paramref name="holidaySet"/> is <c>null</c>.</exception>
        void GetHolidays(int year, HashSet<DateInfo> holidaySet, HolidayType holidayType = HolidayType.PublicHolidays);

        /// <summary>Gets the public holidays for a specified year.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="holidaySet">A set of public holidays with respect to <paramref name="year"/>; dates that falls on the the weekend (i.e. Saturday's and sunday's for the 'standard' weekend) 
        /// will not be added, given elements remain unchanged (output).</param>
        /// <exception cref="NullReferenceException">Thrown, if <paramref name="holidaySet"/> is <c>null</c>.</exception>
        void GetHolidays(int year, HashSet<DateTime> holidaySet);
        #endregion

        /// <summary>Determines whether a specified date is a business day.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns><c>true</c> if <paramref name="date"/> is a business day with respect to the current instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown, if for the <paramref name="date"/> the current calendar is not defined, i.e. <paramref name="date"/> 
        /// is less than <see cref="FirstDate"/> or greater than <see cref="LastDate"/>.</exception>
        bool IsBusinessDay(DateTime date);

        /// <summary>Gets the business day which comes next to a specific date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>If <paramref name="date"/> is some business day, this date will be returned; otherwise the next business day will be returned.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown, if for the <paramref name="date"/> the current <see cref="IHolidayCalendar"/> instance is not defined, 
        /// i.e. <paramref name="date"/> is less than <see cref="FirstDate"/> or greater than <see cref="LastDate"/>.</exception>
        /// <remarks>No business day convention will be taken into account.</remarks>
        DateTime GetForwardAdjustedBusinessDay(DateTime date);

        /// <summary>Gets the business day which is the business day before a specified date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>If <paramref name="date"/> is some business day, this date will be returned; otherwise the business day 
        /// before <paramref name="date"/>will be returned.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown, if for the <paramref name="date"/> the current calendar is not defined, i.e. <paramref name="date"/> is 
        /// less than <see cref="FirstDate"/> or greater than <see cref="LastDate"/>.</exception>
        /// <remarks>No business day convention will be taken into account.</remarks>
        DateTime GetPreviousAdjustedBusinessDay(DateTime date);

        /// <summary>Adds a number of business days to a specified business day.
        /// </summary>
        /// <param name="businessDay">The business day to add a number of business days.</param>
        /// <param name="numberOfBusinessDays">The number of business days to add to <paramref name="businessDay"/>, could be negative.</param>
        /// <returns>The business day that is given by <paramref name="businessDay"/> plus the <paramref name="numberOfBusinessDays"/>.</returns>
        /// <exception cref="ArgumentException">Thrown, if for the <paramref name="businessDay"/> the current calendar is not defined, i.e. <paramref name="businessDay"/> 
        /// is less than <see cref="FirstDate"/> or greater than <see cref="LastDate"/>.</exception>        
        /// <remarks>It will be assumed that <paramref name="businessDay"/> is some business day and this will not be checked.</remarks>
        DateTime AddBusinessDays(DateTime businessDay, int numberOfBusinessDays);

        /// <summary>Gets the number of business days in between, i.e. the number of business days in ]<paramref name="date1"/>, <paramref name="date2"/>]
        /// or ]<paramref name="date2"/>, <paramref name="date1"/>] <c>without</c> taking into account any business day convention.
        /// </summary>
        /// <param name="date1">The date1.</param>
        /// <param name="date2">The date2.</param>
        /// <returns>The number of business days between <paramref name="date1"/> and <paramref name="date2"/> including the latest date if this is a business day.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown, if for <paramref name="date1"/> or <paramref name="date2"/>the current
        /// calendar is not defined, i.e. <paramref name="date1"/> or <paramref name="date2"/> is less than <see cref="FirstDate"/>
        /// or greater than <see cref="LastDate"/>.</exception>
        int GetNumberOfBusinessDaysInBetween(DateTime date1, DateTime date2);
        #endregion
    }
}