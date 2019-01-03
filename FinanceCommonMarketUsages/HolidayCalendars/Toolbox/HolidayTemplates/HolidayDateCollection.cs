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
    /// <summary>Represents holidays with the same name for several years, that are represented by a collection of <see cref="System.DateTime"/> objects.
    /// </summary>
    public  class HolidayDateCollection : IHoliday
    {
        #region public/private (readonly) members

        /// <summary>The name of the holiday in its <see cref="IdentifierString"/> representation.
        /// </summary>
        /// <remarks>The name is language independent.</remarks>
        public readonly IdentifierString Name;

        /// <summary>The long name of the holiday in its <see cref="IdentifierString"/> representation.
        /// </summary>
        /// <remarks>The long name is language dependent.</remarks>
        public readonly IdentifierString LongName;

        /// <summary>The holidays in its <see cref="DateTime"/> representation, where the key is the year.
        /// </summary>
        private Dictionary<int, DateTime> m_FixHolidays;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="HolidayDateCollection"/> class.
        /// </summary>
        /// <param name="holidayName">The (language independent) name of the holiday in its <see cref="IdentifierString"/> representation.</param>
        /// <param name="holidayCollection">The collection of holidays in its <see cref="DateTime"/> representation.</param>
        /// <param name="holidayLongName">The (language dependent) long name of the holiday.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="holidayName"/> or <paramref name="holidayCollection"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown, if two holidays in <paramref name="holidayCollection"/> are given with respect to the same year.</exception>
        public HolidayDateCollection(IdentifierString holidayName, IEnumerable<DateTime> holidayCollection, IdentifierString holidayLongName = null)
        {
            if (holidayName == null)
            {
                throw new ArgumentNullException("holidayName");
            }
            Name = holidayName;
            LongName = (holidayLongName != null) ? holidayLongName : holidayName;
            if (holidayCollection == null)
            {
                throw new ArgumentNullException("holidayCollection");
            }
            m_FixHolidays = new Dictionary<int, DateTime>();
            foreach (DateTime date in holidayCollection)
            {
                if (m_FixHolidays.ContainsKey(date.Year) == true)
                {
                    throw new ArgumentException("The collection of 'holidays with respect to fixed dates' are not given with respect to different years.");
                }
                m_FixHolidays.Add(date.Year, date);
            }
        }
        #endregion

        #region public properties

        #region IHoliday Member

        /// <summary>Gets a value indicating whether the holiday moves if the holiday agrees with a weekend day.
        /// </summary>
        /// <value>The holiday rolling type.</value>
        /// <remarks>This property returns <see cref="HolidayRollingType.NoRolling"/>.</remarks>
        HolidayRollingType IHoliday.HolidayRollingType
        {
            get { return HolidayRollingType.NoRolling; }
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

        #region IHoliday Member

        /// <summary>Gets the holiday with respect to a specific <paramref name="year"/> in its <see cref="System.DateTime"/> representation.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="value">The holiday with respect to the <paramref name="year"/> in its <see cref="DateTime"/> representation (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.
        /// </returns>
        public bool TryGetValue(int year, out DateTime value)
        {
            if (m_FixHolidays.ContainsKey(year))
            {
                value = m_FixHolidays[year];
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
            if (m_FixHolidays.ContainsKey(year))
            {
                value = new DateInfo(m_FixHolidays[year], LongName.String);
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
            return (string)LongName;
        }
        #endregion
    }
}