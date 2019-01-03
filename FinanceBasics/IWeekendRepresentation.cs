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
    /// <summary>Serves as representation of a weekend. 
    /// </summary>
    /// <remarks>For some countries the weekend is not Saturday and Sunday, for example Saudi Arabia it is on Thuesday and Friday etc.</remarks>
    public interface IWeekendRepresentation : IIdentifierNameable, IAnnotatable
    {
        /// <summary>Determines whether a specified day of week falls on a weekend.
        /// </summary>
        /// <param name="dayOfWeek">The day of week.</param>
        /// <returns><c>true</c> if <paramref name="dayOfWeek"/> falls on a weekend; otherwise, <c>false</c>.
        /// </returns>
        bool Contains(DayOfWeek dayOfWeek);

        /// <summary>Gets the next working day, more precisely the next date which do not falls on a weekend.
        /// </summary>
        /// <param name="day">The day.</param>
        /// <returns><paramref name="day"/> plus n days where n is at least one and n is choosen in a way
        /// such that the return value represents a non-weekend day; n is choosen minimal with this property.</returns>
        DateTime GetNextWorkingDay(DateTime day);

        /// <summary>Gets the previous working day, more precisely the previous date which do not falls on a weekend.
        /// </summary>
        /// <param name="day">The day.</param>
        /// <returns><paramref name="day"/> minus n days where n is at least one and n is choosen in a way
        /// such that the return value represents a non-weekend day; n is choosen minimal with this property.</returns>
        DateTime GetPreviousWorkingDay(DateTime day);

        /// <summary>Inserts the days of the week which represents the weekend.
        /// </summary>
        /// <param name="weekendDays">A set of days of the week to add the days which represents the weekend (output).</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="weekendDays"/> is <c>null</c>.</exception>
        /// <remarks>Use this method to collect the days of the week which represents the weekend with respect
        /// to different holiday calendars.</remarks>
        void AddWeekendDaysTo(ISet<DayOfWeek> weekendDays);

        /// <summary>Calculates the intersection of a specific set of days with the set of days which represents a weekend.
        /// </summary>
        /// <param name="weekendDays">A set of days of the week. The output is the intersection with the set of days which 
        /// represents a weekend with respect to the current instance.</param>
        void IntersectWeekendDaysWith(ISet<DayOfWeek> weekendDays);

        /// <summary>Gets the number of days in ]<paramref name="date1"/>, <paramref name="date2"/>]
        /// or ]<paramref name="date2"/>, <paramref name="date1"/>] which are no weekend days.
        /// </summary>
        /// <param name="date1">The date1.</param>
        /// <param name="date2">The date2.</param>
        /// <returns>The number of non-weekend days between <paramref name="date1"/> and <paramref name="date2"/> including the latest date if this is a business day.
        /// </returns>
        int GetNumberOfWorkingDaysInBetween(DateTime date1, DateTime date2);

        /// <summary>Gets the weekend days for a specific year in some <see cref="DateInfo"/> representation, i.e. 
        /// in the case of the 'standard' weekend, add saturday's and sundays into a <see cref="HashSet&lt;DateInfo&gt;"/> instance.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="holidaySet">The holiday table set to add (output).</param>
        /// <param name="format">The standard or custome date and time format string.</param>
        /// <exception cref="NullReferenceException">Thrown, if <paramref name="holidaySet"/> is <c>null</c>.</exception>
        /// <remarks>No element will be remove or change before adding new elements to <paramref name="holidaySet"/>.</remarks>
        void GetWeekendDateInfos(int year, ISet<DateInfo> holidaySet, string format = "dddd");
    }
}