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
    /// <summary>Represents the 'empty' weekend, i.e. each day of the week represents a non-weekend day.
    /// </summary>
    internal class EmptyWeekend : IWeekendRepresentation
    {
        #region private (static) readonly members

        /// <summary>The name of the weekend type.
        /// </summary>
        private static readonly IdentifierString sm_Name = new IdentifierString("Empty");
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="EmptyWeekend"/> class.
        /// </summary>
        internal EmptyWeekend()
        {
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Member

        /// <summary>Gets the name of the current instance.
        /// </summary>
        /// <value>The language independent name of the current instance.</value>
        public IdentifierString Name
        {
            get { return sm_Name; }
        }

        /// <summary>Gets the long name of the current instance.
        /// </summary>
        /// <value>The language dependent long name of the current instance.</value>
        public IdentifierString LongName
        {
            get { return sm_Name; }
        }
        #endregion

        #region IAnnotatable Members

        /// <summary>Gets a value indicating whether the annotation is readonly.
        /// </summary>
        /// <value><c>true</c> if the annotation of this instance is readonly; otherwise, <c>false</c>.
        /// </value>
        bool IAnnotatable.HasReadOnlyAnnotation
        {
            get { return true; }
        }

        /// <summary>Gets the annotation of the current instance.
        /// </summary>
        /// <value>The annotation of the current instance.</value>
        public string Annotation
        {
            get { return Resources.EmptyWeekendType; }
        }
        #endregion

        #endregion

        #region public methods

        #region IAnnotatable Members

        /// <summary>Sets the annotation of the current instance.
        /// </summary>
        /// <param name="annotation">The annotation.</param>
        /// <returns>A value indicating whether the <see cref="Annotation"/> has been changed.
        /// </returns>
        bool IAnnotatable.TrySetAnnotation(string annotation)
        {
            return false;
        }
        #endregion

        #region IWeekendRepresentation Member

        /// <summary>Determines whether a specified day of week falls on a weekend.
        /// </summary>
        /// <param name="dayOfWeek">The day of week.</param>
        /// <returns><c>true</c> if <paramref name="dayOfWeek"/> falls on a weekend; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(DayOfWeek dayOfWeek)
        {
            return false;
        }

        /// <summary>Gets the next working day, more precisely the next date which do not falls on a weekend.
        /// </summary>
        /// <param name="day">The day.</param>
        /// <returns><paramref name="day"/> plus n days where n is at least one and n is choosen in a way
        /// such that the return value represents a non-weekend day; n is choosen minimal with this property.
        /// </returns>
        public DateTime GetNextWorkingDay(DateTime day)
        {
            return day.AddDays(1);
        }

        /// <summary>Gets the previous working day, more precisely the previous date which do not falls on a weekend.
        /// </summary>
        /// <param name="day">The day.</param>
        /// <returns><paramref name="day"/> minus n days where n is at least one and n is choosen in a way
        /// such that the return value represents a non-weekend day; n is choosen minimal with this property.
        /// </returns>
        public DateTime GetPreviousWorkingDay(DateTime day)
        {
            return day.AddDays(-1);
        }

        /// <summary>Inserts the days of the week which represents the weekend.
        /// </summary>
        /// <param name="weekendDays">A set of days of the week to add the days which represents the weekend (output).</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="weekendDays"/> is <c>null</c>.</exception>
        /// <remarks>Use this method to collect the days of the week which represents the weekend with respect
        /// to different holiday calendars.</remarks>
        public void AddWeekendDaysTo(ISet<DayOfWeek> weekendDays)
        {
            // nothing to do here
        }

        /// <summary>Calculates the intersection of a specific set of days with the set of days which represents a weekend.
        /// </summary>
        /// <param name="weekendDays">A set of days of the week. The output is the intersection with the set of days which 
        /// represents a weekend with respect to the current instance.</param>
        public void IntersectWeekendDaysWith(ISet<DayOfWeek> weekendDays)
        {
            if (weekendDays == null)
            {
                throw new ArgumentNullException("weekendDays");
            }
            weekendDays.Clear();  // no day is a weekend day
        }

        /// <summary>Gets the number of days in ]<paramref name="date1"/>, <paramref name="date2"/>]
        /// or ]<paramref name="date2"/>, <paramref name="date1"/>] which are no weekend days.
        /// </summary>
        /// <param name="date1">The date1.</param>
        /// <param name="date2">The date2.</param>
        /// <returns>The number of non-weekend days between <paramref name="date1"/> and <paramref name="date2"/> including the latest date if this is a business day.
        /// </returns>
        public int GetNumberOfWorkingDaysInBetween(DateTime date1, DateTime date2)
        {
            if (date2 < date1)
            {
                DateTime temp = date1;
                date1 = date2;
                date2 = temp;
            }
            return date2.Subtract(date1).Days;
        }

        /// <summary>Gets the weekend days for a specific year in some <see cref="DateInfo"/> representation, i.e.
        /// in the case of the 'standard' weekend, add saturday's and sundays into a <see cref="HashSet&lt;DateInfo&gt;"/> instance.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="holidaySet">The holiday table set to add (output).</param>
        /// <param name="format">The standard or custome date and time format string.</param>
        /// <exception cref="NullReferenceException">Thrown, if <paramref name="holidaySet"/> is <c>null</c>.</exception>
        /// <remarks>No element will be remove or change before adding new elements to <paramref name="holidaySet"/>.</remarks>
        public void GetWeekendDateInfos(int year, ISet<DateInfo> holidaySet, string format = "dddd")
        {
            // nothing to do here
        }
        #endregion

        #endregion
    }
}