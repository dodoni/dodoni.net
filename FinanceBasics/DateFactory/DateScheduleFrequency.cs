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
using System.Linq;
using System.Collections.Generic;

using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Containers;

namespace Dodoni.Finance.DateFactory
{
    /// <summary>Serves as factory for <see cref="IDateScheduleFrequency"/> instances and contains a collection of frequently used frequencies.
    /// </summary>
    public static class DateScheduleFrequency
    {
        #region private static members

        /// <summary>The pool of (date schedule) frequencies.
        /// </summary>
        private static IdentifierNameableDictionary<IDateScheduleFrequency> sm_Pool;
        #endregion

        #region public static (readonly) members

        /// <summary>The annually date schedule frequency, i.e. the time span for each period is one year.
        /// </summary>
        public static readonly IDateScheduleFrequency Annually = new Annually();

        /// <summary>The semi-annually date schedule frequency, i.e. the time span for each period is 1/2 year, thus six months.
        /// </summary>
        public static readonly IDateScheduleFrequency SemiAnnually = new SemiAnnually();

        /// <summary>The quarterly date schedule frequency, i.e. the time span for each period is 1/4 year, thus three months.
        /// </summary>
        public static readonly IDateScheduleFrequency Quarterly = new Quarterly();

        /// <summary>The bi-monthly date schedule frequency, i.e. the time span for each period is equal to two months.
        /// </summary>
        public static readonly IDateScheduleFrequency BiMonthly = new BiMonthly();

        /// <summary>The monthly date schedule frequency, i.e. the time span for each period is equal to one month.
        /// </summary>
        public static readonly IDateScheduleFrequency Monthly = new Monthly();

        /// <summary>The date schedule frequency where each time span is equal to four weeks.
        /// </summary>
        public static readonly IDateScheduleFrequency EveryForWeeks = new EveryFourWeeks();

        /// <summary>The weekly date schedule frequency, i.e. the time span for each period is equal to one week.
        /// </summary>
        public static readonly IDateScheduleFrequency Weekly = new Weekly();

        /// <summary>The daily date schedule frequency, i.e. the time span for each period is equal to one business day.
        /// </summary>
        public static readonly IDateScheduleFrequency Daily = new Daily();

        /// <summary>The date schedule frequency 'once'.
        /// </summary>
        public static readonly IDateScheduleFrequency Once = new Once();
        #endregion

        #region static constructor

        /// <summary>Initializes the <see cref="DateScheduleFrequency"/> class.
        /// </summary>
        static DateScheduleFrequency()
        {
            sm_Pool = new IdentifierNameableDictionary<IDateScheduleFrequency>(Annually, SemiAnnually, Quarterly, BiMonthly, Monthly, EveryForWeeks, Weekly, Daily, Once);
        }
        #endregion

        #region public static properties

        /// <summary>Gets the number of (regular) date schedule frequencies.
        /// </summary>
        /// <value>The number of (regular) date schedule frequencies.</value>
        public static int Count
        {
            get { return sm_Pool.Count; }
        }

        /// <summary>Gets the (regular) date schedule frequencies.
        /// </summary>
        /// <value>The (regular) date schedule frequencies.</value>
        public static IEnumerable<IDateScheduleFrequency> Values
        {
            get { return sm_Pool.Values; }
        }

        /// <summary>Gets the names of the date schedule frequency pool.
        /// </summary>
        /// <returns>A collection of the date schedule frequency names.</returns>
        public static IEnumerable<string> Names
        {
            get { return sm_Pool.Names; }
        }
        #endregion

        #region public static methods

        /// <summary>Gets date schedule frequency names in descending order.
        /// </summary>
        /// <returns>A collection of date schedule frequency names in descending order and its <see cref="System.String"/> representation.</returns>
        public static IEnumerable<string> GetSortedNames()
        {
            return sm_Pool.Names.OrderByDescending(name => name);
        }

        /// <summary>Gets date schedule frequency names in descending order.
        /// </summary>
        /// <returns>A collection of date schedule frequency names in descending order and its <see cref="IdentifierString"/> representation.</returns>
        public static IEnumerable<IdentifierString> GetSortedIdentifierStringNames()
        {
            return sm_Pool.GetNamesAsIdentifierStrings().OrderByDescending(name => name);
        }

        /// <summary>Determines whether a specified date schedule frequency is the frequency 'once'.
        /// </summary>
        /// <param name="dateScheduleFrequency">The date schedule frequency.</param>
        /// <returns>
        /// 	<c>true</c> if the specified date schedule frequency is the frequency 'once'; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOnceFrequency(this IDateScheduleFrequency dateScheduleFrequency)
        {
            return (dateScheduleFrequency != null) ? TenorTimeSpan.IsNull(dateScheduleFrequency.GetFrequencyTenor()) : false;
        }

        /// <summary>Gets a date schedule frequency.
        /// </summary>
        /// <param name="frequency">The frequency in its <see cref="System.String"/> representation; perhaps a <see cref="TenorTimeSpan"/> in its <see cref="System.String"/> representation.</param>
        /// <param name="value">The date schedule frequency (output).</param>
        /// <param name="addIntoPool">If <paramref name="frequency"/> represents a individual frequency,
        /// the corresponding <see cref="IDateScheduleFrequency"/> will be added to the <see cref="DateScheduleFrequency"/> if set to <c>true</c>.</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        /// <remarks>If <paramref name="frequency"/> does not represents an item of the pool, try to create to convert <paramref name="frequency"/> to a  <see cref="TenorTimeSpan"/> object first.</remarks>
        public static bool TryGetValue(string frequency, out IDateScheduleFrequency value, bool addIntoPool = false)
        {
            if ((frequency == null) || (frequency.Length == 0))
            {
                value = null;
                return false;
            }
            if (sm_Pool.TryGetValue(frequency, out value) == true)
            {
                return true;
            }

            TenorTimeSpan tenorFrequency;   // try to generate a individual frequency:
            if (TenorTimeSpan.TryParse(frequency, out tenorFrequency) == false)
            {
                value = null;
                return false;
            }
            if (IndividualDateScheduleFrequency.TryCreate(tenorFrequency, out value) == true)
            {
                if (addIntoPool)
                {
                    sm_Pool.Add(value);
                }
                return true;
            }
            value = null;
            return false;
        }

        /// <summary>Gets a date schedule frequency.
        /// </summary>
        /// <param name="frequency">The frequency in its <see cref="System.String"/> representation; perhaps a <see cref="TenorTimeSpan"/> in its <see cref="System.String"/> representation.</param>
        /// <param name="value">The date schedule frequency (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        /// <remarks>If <paramref name="frequency"/> does not represents an item of the pool, try to create to convert <paramref name="frequency"/> to a  <see cref="TenorTimeSpan"/> object first.</remarks>
        public static bool TryGetValue(string frequency, out IDateScheduleFrequency value)
        {
            if ((frequency == null) || (frequency.Length == 0))
            {
                value = null;
                return false;
            }
            if (sm_Pool.TryGetValue(frequency, out value) == true)
            {
                return true;
            }

            // try to generate a individual frequency
            TenorTimeSpan tenorFrequency;
            if (TenorTimeSpan.TryParse(frequency, out tenorFrequency) == false)
            {
                value = null;
                return false;
            }
            return IndividualDateScheduleFrequency.TryCreate(tenorFrequency, out value);
        }
        #endregion
    }
}