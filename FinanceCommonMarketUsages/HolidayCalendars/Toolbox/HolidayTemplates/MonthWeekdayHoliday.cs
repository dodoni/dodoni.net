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

namespace Dodoni.Finance.CommonMarketUsages.HolidayCalendars
{
    /// <summary>Serves as holiday where the date is represented by a fixed month, a fixed weekday and the index of the week with respect to the month.
    /// </summary>
    /// <example>For example some holidays represents the second monday in may.</example>
    /// <remarks>Here, it is assumed, that the weekday of the holiday represented by this class is not a saturday or sunday.</remarks>
    public class MonthWeekdayHoliday : IHoliday
    {
        #region nested enumerations

        /// <summary>Represent the one-based index of a specified week with respect to a specific month.
        /// </summary>
        public enum WeekIndex
        {
            /// <summary>The first week of the specified month.
            /// </summary>
            First = 0,

            /// <summary>The second week of the specified month.
            /// </summary>
            Second = 1,

            /// <summary>The third week of the specified month.
            /// </summary>
            Third = 2,

            /// <summary>The fourth week of the specified month.
            /// </summary>
            Fourth = 3,

            /// <summary>The last week of the specified month, perhaps the fifth week of the month.
            /// </summary>
            Last = 4
        }
        #endregion

        #region public (readonly) members

        /// <summary>The name of the holiday in its <see cref="IdentifierString"/> representation.
        /// </summary>
        /// <remarks>The name is language independent.</remarks>
        public readonly IdentifierString Name;

        /// <summary>The long name of the holiday in its <see cref="IdentifierString"/> representation.
        /// </summary>
        /// <remarks>The long name is language dependent.</remarks>
        public readonly IdentifierString LongName;

        /// <summary>The first year where this holiday occurs, i.e. the optional start year of the holiday.
        /// </summary>
        /// <remarks><see cref="Int32.MinValue"/> is standard, i.e. in each year the represented holiday occurs.</remarks>
        public readonly int FirstYear;

        /// <summary>The last year where this holiday occurs, i.e. the optional end year of the holiday.
        /// </summary>
        /// <remarks><see cref="Int32.MaxValue"/> is standard, i.e. in each year the represented holiday occurs.</remarks>
        public readonly int LastYear;

        /// <summary>The one-based index of the month with respect to the represented holiday.
        /// </summary>
        public readonly int Month;

        /// <summary>The day of the week in its <see cref="System.Int32"/> representation with respect to the represented holiday, 
        /// where the <see cref="System.Int32"/> representation is equal to the cast of a <see cref="System.DayOfWeek"/> object.
        /// </summary>
        public readonly int DayOfWeek;

        /// <summary>The null-based index of the week with respect to the represented holiday and the corresponding <see cref="Month"/>.
        /// </summary>
        public readonly int Week;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="MonthWeekdayHoliday"/> class.
        /// </summary>
        /// <param name="holidayName">The (language independent) name of the holiday in its <see cref="IdentifierString"/> representation.</param>
        /// <param name="dayOfWeek">The day of week with respect to the represented holiday.</param>
        /// <param name="month">The month with respect to the represented holiday.</param>
        /// <param name="week">The index of the week with respect to the represented holiday and the corresponding <paramref name="month"/>.</param>
        /// <param name="firstYear">The first year to take into account the holiday.</param>
        /// <param name="lastYear">The last year to take into account the holiday.</param>
        /// <param name="holidayLongName">The (language dependent) long name of the holiday.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="holidayName"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="dayOfWeek"/> represents a saturday or sunday.</exception>
        public MonthWeekdayHoliday(IdentifierString holidayName, DayOfWeek dayOfWeek, Month month, WeekIndex week, int firstYear = Int32.MinValue, int lastYear = Int32.MaxValue, IdentifierString holidayLongName = null)
        {
            if (holidayName == null)
            {
                throw new ArgumentNullException("holidayName");
            }
            Name = holidayName;
            LongName = (holidayLongName != null) ? holidayLongName : holidayName;
            if ((dayOfWeek == System.DayOfWeek.Saturday) || (dayOfWeek == System.DayOfWeek.Sunday))
            {
                throw new ArgumentException("No saturdays or sundays allowed.", "dayOfWeek");
            }
            DayOfWeek = (int)dayOfWeek;
            Month = (int)month;
            Week = (int)week;

            FirstYear = firstYear;
            LastYear = lastYear;
        }
        #endregion

        #region public properties

        #region IHoliday Members

        /// <summary>Gets a value indicating whether the holiday moves if the holiday agrees with a weekend day.
        /// </summary>
        /// <value>The holiday rolling type.</value>
        public HolidayRollingType HolidayRollingType
        {
            get { return HolidayRollingType.NoRolling; }
        }
        #endregion

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the holiday.
        /// </summary>
        /// <value>The name of the holiday.</value>
        /// <remarks>The name of the holiday is language independent.</remarks>
        IdentifierString IIdentifierNameable.Name
        {
            get { return Name; }
        }

        /// <summary>Gets the long name of the holiday.
        /// </summary>
        /// <value>The long name of the holiday.</value>
        /// <remarks>The long name of the holiday is language dependent.</remarks>
        IdentifierString IIdentifierNameable.LongName
        {
            get { return LongName; }
        }
        #endregion

        #endregion

        #region public methods

        #region IHoliday Members

        /// <summary>Gets the holiday with respect to a specific <paramref name="year"/> in its <see cref="System.DateTime"/> representation.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="value">The holiday with respect to the <paramref name="year"/> in its <see cref="DateTime"/> representation (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.
        /// </returns>
        public bool TryGetValue(int year, out DateTime value)
        {
            if ((year >= FirstYear) && (year <= LastYear))
            {
                DateTime date = new DateTime(year, Month, 1);

                // goto the first date in the given month/year with the correct day of week
                int firstDayOfWeek = (int)date.DayOfWeek;
                // "if (firstDayOfWeek <= this.DayOfWeek) {
                //   offset = this.DayOfWeek - firstDayOfWeek;
                // {
                // else{
                //   offset = ( 7 - firstDayOfWeek) + this.DayOfWeek;     
                // }", i.e. in short:
                date = date.AddDays((firstDayOfWeek <= DayOfWeek) ? DayOfWeek - firstDayOfWeek : 7 - firstDayOfWeek + DayOfWeek);

                // now, we have to care about the week, because some months do not have for example a monday in the fifth week
                value = date.AddDays(7 * this.Week);
                if (value.Month != Month)
                {
                    value = date.AddDays(21);  // = 7 * 3, i.e. 'last week' is equal to the fourth week
                }
                return true;
            }
            value = DateTime.MinValue;
            return false;
        }

        /// <summary>Gets the holiday with respect to a specific <paramref name="year"/> in its <see cref="DateInfo"/> representation.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="value">The holiday with respect to the year <paramref name="year"/> in its <see cref="DateInfo"/> representation (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.
        /// </returns>
        public bool TryGetValue(int year, out DateInfo value)
        {
            DateTime date;
            bool flag = TryGetValue(year, out date);

            value = new DateInfo(date, (string)LongName);
            return flag;
        }
        #endregion

        /// <summary>Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance, i.e. the long name of the holiday.
        /// </returns>
        public override string ToString()
        {
            return LongName.String;
        }
        #endregion
    }
}