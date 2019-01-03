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

using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Utilities;

namespace Dodoni.Finance.CommonMarketUsages.HolidayCalendars
{
    /// <summary>Represents a holiday that is the offset (in days) with respect to easter monday.
    /// </summary>
    public class EasterDependingHoliday : IHoliday
    {
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

        /// <summary>A value indicating whether the holiday moves if the holiday agrees with some weekend day.
        /// </summary>
        public readonly HolidayRollingType HolidayRollingType;
        #endregion

        #region private members

        /// <summary>The offset to easter monday.
        /// </summary>
        private int m_OffsetToEasterMonday;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="EasterDependingHoliday"/> class.
        /// </summary>
        /// <param name="holidayName">The (language independent) name of the holiday in its <see cref="IdentifierString"/> representation.</param>
        /// <param name="offsetToEasterMonday">The offset to easter monday (in days).</param>
        /// <param name="firstYear">The first year to take into account the holiday.</param>
        /// <param name="lastYear">The last year to take into account the holiday.</param>
        /// <param name="holidayRollingType">A value indicating whether the holiday moves if the holiday agrees with a weekend day.</param>
        /// <param name="holidayLongName">The (language dependent) long name of the holiday.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="holidayName"/> is <c>null</c>.</exception>
        public EasterDependingHoliday(IdentifierString holidayName, int offsetToEasterMonday, int firstYear = Int32.MinValue, int lastYear = Int32.MaxValue, HolidayRollingType holidayRollingType = HolidayCalendars.HolidayRollingType.NoRolling, IdentifierString holidayLongName = null)
        {
            if (holidayName == null)
            {
                throw new ArgumentNullException("holidayName");
            }
            Name = holidayName;
            LongName = (holidayLongName != null) ? holidayLongName : holidayName;
            m_OffsetToEasterMonday = offsetToEasterMonday;
            FirstYear = firstYear;
            LastYear = lastYear;
            HolidayRollingType = holidayRollingType;
        }
        #endregion

        #region public properties

        #region IHoliday Member

        /// <summary>Gets a value indicating whether the holiday moves if the holiday agrees with a weekend day.
        /// </summary>
        /// <value>The holiday rolling type.</value>
        HolidayRollingType IHoliday.HolidayRollingType
        {
            get { return HolidayRollingType; }
        }
        #endregion

        #region IIdentifierNameable Member

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
                DateTime easterMonday = GetEasterMonday(year);
                value = easterMonday.AddDays(m_OffsetToEasterMonday);
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
            if ((year >= FirstYear) && (year <= LastYear))
            {
                DateTime easterMonday = GetEasterMonday(year);
                value = new DateInfo(easterMonday.AddDays(m_OffsetToEasterMonday), LongName.String);
                return true;
            }
            value = DateInfo.MinValue;
            return false;
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

        #region private static methods

        /// <summary>Computes the day of easter monday for a specific year.
        /// </summary>
        /// <param name="year">The year to compute the easter monday.</param>
        /// <returns>The date of easter monday with respect to <paramref name="year"/>.</returns>
        /// <remarks>Source: Gauﬂsche Osterformel, Wikipedia.</remarks>
        private static DateTime GetEasterMonday(int year)
        {
            int a = year % 19;
            int b = year % 4;
            int c = year % 7;
            int h1 = year / 100;
            int h2 = year / 400;
            int n = 4 + h1 - h2;
            int m = 15 + h1 - h2 - ((8 * h1 + 13) / 25);
            int d = (19 * a + m) % 30;
            int e = (2 * b + 4 * c + 6 * d + n) % 7;

            int days;
            if ((d == 28) && (e == 6) && (a > 10))
            {
                days = 49;
            }
            else
            {
                days = 22 + d + e;
            }
            if (days > 31)
            {
                return new DateTime(year, 4, days - 31).AddDays(1);
            }
            return new DateTime(year, 3, days).AddDays(1);
        }
        #endregion
    }
}