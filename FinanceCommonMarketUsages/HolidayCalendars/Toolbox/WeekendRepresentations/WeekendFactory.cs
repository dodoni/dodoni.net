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

namespace Dodoni.Finance.CommonMarketUsages.HolidayCalendars
{
    /// <summary>Serves as factory for <see cref="IWeekendRepresentation"/> objects.
    /// </summary>
    public static class WeekendFactory
    {
        #region public (readonly) members

        /// <summary>Represents the standard weekend, i.e. Saturday and Sunday.
        /// </summary>
        public static readonly IWeekendRepresentation StandardWeekend = new StandardWeekend();

        /// <summary>Represents the empty weekend, i.e. no day of the week is a weekend day.
        /// </summary>
        public static readonly IWeekendRepresentation EmptyWeekend = new EmptyWeekend();
        #endregion

        #region public (static) methods

        /// <summary>Gets a <see cref="IWeekendRepresentation"/> object which represents a specific weekend.
        /// </summary>
        /// <param name="dayOfWeekend">The day which represents the weekend.</param>
        /// <returns>A <see cref="IWeekendRepresentation"/> object which represents a weekend with respect to <paramref name="dayOfWeekend"/>.</returns>
        public static IWeekendRepresentation GetWeekend(DayOfWeek dayOfWeekend)
        {
            return new GenericWeekend(dayOfWeekend);
        }

        /// <summary>Gets a <see cref="IWeekendRepresentation"/> object which represents a specific weekend.
        /// </summary>
        /// <param name="daysOfWeekend">The days which represents the weekend.</param>
        /// <returns>A <see cref="IWeekendRepresentation"/> object which represents a weekend with respect to <paramref name="daysOfWeekend"/>.</returns>
        /// <exception cref="ArgumentException">Thrown, if the elements of <paramref name="daysOfWeekend"/> represents the whole week.</exception>
        public static IWeekendRepresentation GetWeekend(params DayOfWeek[] daysOfWeekend)
        {
            if ((daysOfWeekend == null) || (daysOfWeekend.Length == 0))
            {
                return EmptyWeekend;
            }

            SortedSet<DayOfWeek> setOfWeekendDays = new SortedSet<DayOfWeek>(daysOfWeekend);
            if ((setOfWeekendDays.Count == 2) && setOfWeekendDays.Contains(DayOfWeek.Saturday) && setOfWeekendDays.Contains(DayOfWeek.Sunday))
            {
                return StandardWeekend;
            }
            if (setOfWeekendDays.Count >= 7)
            {
                throw new ArgumentException("daysOfWeekend");
            }
            return new GenericWeekend(setOfWeekendDays);
        }

        /// <summary>Gets a <see cref="IWeekendRepresentation"/> object which represents a specific weekend.
        /// </summary>
        /// <param name="daysOfWeekend">The days which represents the weekend.</param>
        /// <returns>A <see cref="IWeekendRepresentation"/> object which represents a weekend with respect to <paramref name="daysOfWeekend"/>.</returns>
        /// <exception cref="ArgumentException">Thrown, if the elements of <paramref name="daysOfWeekend"/> represents the whole week.</exception>
        public static IWeekendRepresentation GetWeekend(ISet<DayOfWeek> daysOfWeekend)
        {
            if ((daysOfWeekend == null) || (daysOfWeekend.Count == 0))
            {
                return EmptyWeekend;
            }

            SortedSet<DayOfWeek> setOfWeekendDays = new SortedSet<DayOfWeek>(daysOfWeekend);
            if ((setOfWeekendDays.Count == 2) && setOfWeekendDays.Contains(DayOfWeek.Saturday) && setOfWeekendDays.Contains(DayOfWeek.Sunday))
            {
                return StandardWeekend;
            }
            if (setOfWeekendDays.Count >= 7)
            {
                throw new ArgumentException("daysOfWeekend");
            }
            return new GenericWeekend(setOfWeekendDays);
        }
        #endregion
    }
}