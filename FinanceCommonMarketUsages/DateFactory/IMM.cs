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

using Dodoni.BasicComponents.Utilities;

namespace Dodoni.Finance.DateFactory
{
    /// <summary>Serves as factory for IMM (International Monetary Market) dates.
    /// </summary>
    /// <remarks>A IMM date is the third Wednesday of March, June, September and December (i.e., between the 15th and 21st, whichever such day is a Wednesday).</remarks>
    public partial class IMM
    {
        #region public nested enumerations

        /// <summary>Specifies the month of the IMM date.
        /// </summary>
        public enum Month
        {
            /// <summary>IMM date in March (H).
            /// </summary>
            [String("H")]
            March = 3,

            /// <summary>IMM date in June (M).
            /// </summary>
            [String("M")]
            June = 6,

            /// <summary>IMM date in September (U).
            /// </summary>
            [String("U")]
            September = 9,

            /// <summary>IMM date in December (Z).
            /// </summary>
            [String("Z")]
            December = 12
        }
        #endregion

        #region public static methods

        /// <summary>Gets the next IMM date.
        /// </summary>
        /// <param name="date">The basis for the IMM date calculation.</param>
        /// <returns>The next IMM date with respect to <paramref name="date"/>.</returns>
        public static DateTime Next(DateTime date)
        {
            int m = date.Month;
            int y = date.Year;
            int offset = 3;

            int sMonths = offset - (date.Month % offset);
            if (sMonths != offset || date.Day > 21)
            {
                sMonths += date.Month;
                if (sMonths <= 12)
                {
                    m = sMonths;
                }
                else
                {
                    m = sMonths - 12;
                    y += 1;
                }
            }
            return GetNextDate(new DateTime(y, m, 1));
        }

        /// <summary>Gets the next IMM date.
        /// </summary>
        /// <param name="date">The basis for the IMM date calculation.</param>
        /// <param name="month">The month of the IMM date in its <see cref="IMM.Month"/> representation.</param>
        /// <param name="yearOffset">The number of years to add to <paramref name="date"/> to take into account for IMM date calculation.</param>
        /// <returns>The next IMM date in month <paramref name="month"/> with respect to <paramref name="date"/> + <paramref name="yearOffset"/>.</returns>
        public static DateTime Next(DateTime date, Month month, int yearOffset)
        {
            return GetNextDate(new DateTime(date.Year + yearOffset, (int)month, 1));
        }

        /// <summary>Creates a new <see cref="ITimeframeDescription"/> object, where the start date of the period is a IMM date.</summary>
        /// <param name="month">The month of the period start date in its <see cref="IMM.Month"/> representation.</param>
        /// <param name="periodStartYearOffset">The number of years to add to the reference date in <c>GetStartAndEndDate(.)</c> to take into account for the calculation of the specific IMM date.</param>
        /// <param name="tenor">The tenor that represents the time span.</param>
        /// <param name="startDateAdjustment">A business day convention used to compute the start date of the period, i.e. the IMM date.</param>
        /// <param name="endDateAdjustment">A business day convention used to compute the end date of the period.</param>
        /// <returns>The specified <see cref="ITimeframeDescription"/> object.</returns>
        public static ITimeframeDescription Create(Month month, int periodStartYearOffset, TenorTimeSpan tenor, IBusinessDayConvention startDateAdjustment, IBusinessDayConvention endDateAdjustment)
        {
            return new SpecificPeriod(month, periodStartYearOffset, tenor, startDateAdjustment, endDateAdjustment);
        }

        /// <summary>Creates a new <see cref="ITimeframeDescription"/> object, where the start date of the period is the next IMM date with respect to the reference date of the time period calculation.</summary>
        /// <param name="tenor">The tenor that represents the time span.</param>
        /// <param name="startDateAdjustment">A business day convention used to compute the start date of the period, i.e. the IMM date.</param>
        /// <param name="endDateAdjustment">A business day convention used to compute the end date of the period.</param>
        /// <returns>The specified <see cref="ITimeframeDescription"/> object.</returns>
        public static ITimeframeDescription Create(TenorTimeSpan tenor, IBusinessDayConvention startDateAdjustment, IBusinessDayConvention endDateAdjustment)
        {
            return new NextPeriod(tenor, startDateAdjustment, endDateAdjustment);
        }
        #endregion

        #region private static methods

        /// <summary>Gets the next IMM date in the month/year represented by <paramref name="startDate"/>.
        /// </summary>
        /// <param name="startDate">A <see cref="System.DateTime"/> object, which points to the first day of the month to calculate the IMM date.</param>
        /// <returns>The IMM date in the month/year represented by <paramref name="startDate"/>.</returns>
        private static DateTime GetNextDate(DateTime startDate)
        {
            int dayOfWeek = (int)startDate.DayOfWeek;  // 3 is Wednesday
            int day = 15;  // day one plus 2 weeks
            if (dayOfWeek <= 3) // Sunday, Monday, Tuesday, Wednesday
            {
                day += 3 - dayOfWeek;
            }
            else if (dayOfWeek == 4) // Thursday
            {
                day += 6;
            }
            else if (dayOfWeek == 5)  // Friday
            {
                day += 5;
            }
            else  // Staturday
            {
                day += 4;
            }
            return new DateTime(startDate.Year, startDate.Month, day);
        }
        #endregion
    }
}