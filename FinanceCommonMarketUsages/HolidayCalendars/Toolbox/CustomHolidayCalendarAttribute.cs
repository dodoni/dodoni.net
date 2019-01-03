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

using Dodoni.Finance;

namespace Dodoni.Finance.CommonMarketUsages.HolidayCalendars
{
    /// <summary>Represents an attribute for an enumeration where the enum items represents public holidays.
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum, Inherited = false, AllowMultiple = false)]
    public sealed class CustomHolidayCalendarAttribute : System.Attribute
    {
        #region public (readonly) members

        /// <summary>The name of the holiday calendar.
        /// </summary>
        /// <remarks>The name of the calendar is assumed to be independend of the language. In general the name is some 
        /// shortcut with at most three characters (ISO name).</remarks>
        public readonly string CalendarName;

        /// <summary>The region of the holiday calendar.
        /// </summary>
        public readonly HolidayCalendarRegion Region;

        /// <summary>The resource name (no language dependend suffix) and the corresponding namespace.
        /// </summary>
        public readonly string FullResourceName;

        /// <summary>The language depending description of the holiday calendar with respect to <see cref="FullResourceName"/>, i.e. as some resource property name.
        /// </summary>
        /// <remarks>If set to <c>null</c>, this member will be ignored.</remarks>
        public string Description = null;
        #endregion

        #region private members

        /// <summary>The earliest date for that holiday informations are available.
        /// </summary>
        private DateTime m_FirstDate;

        /// <summary>The latest date for that holiday informations are available.
        /// </summary>
        private DateTime m_LastDate;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="CustomHolidayCalendarAttribute"/> class.
        /// </summary>
        /// <param name="calendarName">The name of the holiday calendar.</param>
        /// <param name="region">The region of the holiday calendar.</param>
        /// <param name="fullResourceName">The name of the resource (including the namespace).</param>
        public CustomHolidayCalendarAttribute(string calendarName, HolidayCalendarRegion region, string fullResourceName)
        {
            CalendarName = calendarName;
            Region = region;
            FullResourceName = fullResourceName;
            m_FirstDate = DateTime.MinValue;
            m_LastDate = DateTime.MaxValue;
        }
        #endregion

        #region public properties

        /// <summary>Gets or sets the earliest date for that holiday informations are available.
        /// </summary>
        /// <value>The first valid date with respect to the given calendar.</value>
        public DateTime FirstDate
        {
            get { return m_FirstDate; }
            set { m_FirstDate = value; }
        }

        /// <summary>Gets or sets the latest date for that holiday informations are available.
        /// </summary>
        /// <value>The last valid date with respect to the given calendar.</value>
        public DateTime LastDate
        {
            get { return m_LastDate; }
            set { m_LastDate = value; }
        }
        #endregion
    }
}