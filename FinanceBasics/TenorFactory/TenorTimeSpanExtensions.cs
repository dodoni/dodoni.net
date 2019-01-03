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

using Dodoni.Finance.DateFactory;

namespace Dodoni.Finance
{
    /// <summary>Extensions for <see cref="TenorTimeSpan"/> objects.
    /// </summary>
    public static class TenorTimeSpanExtensions
    {
        /// <summary>Converts a <see cref="TenorTimeSpan"/> object into its <see cref="IDateScheduleFrequency"/> representation.
        /// </summary>
        /// <param name="tenorTimeSpan">The tenor time span.</param>
        /// <returns>A <see cref="IDateScheduleFrequency"/> object which represent the <paramref name="tenorTimeSpan"/>.</returns>
        public static IDateScheduleFrequency AsFrequency(this TenorTimeSpan tenorTimeSpan)
        {
            return new IndividualDateScheduleFrequency(tenorTimeSpan);
        }

        /// <summary>Adds a specific <see cref="TenorTimeSpan"/> object to the current instance.
        /// </summary>
        /// <param name="tenorTimeSpan">The tenor time span.</param>
        /// <param name="tenorTimeSpanToAdd">The <see cref="TenorTimeSpan"/> to add.</param>
        /// <param name="tenorTimeSpanFactor">A (optional) factor to take into account.</param>
        /// <returns>A <see cref="TenorTimeSpan"/> object which is the result of <paramref name="tenorTimeSpan"/> plus <paramref name="tenorTimeSpanFactor"/> * <paramref name="tenorTimeSpanToAdd"/>.</returns>
        /// <remarks>If <paramref name="tenorTimeSpanToAdd"/> is equal to <see cref="TenorTimeSpan.TomorrowNextTenor"/>, two (2) [times <paramref name="tenorTimeSpanFactor"/>] days will be added to <paramref name="tenorTimeSpan"/>.</remarks>
        public static TenorTimeSpan AddTimeSpan(this TenorTimeSpan tenorTimeSpan, TenorTimeSpan tenorTimeSpanToAdd, int tenorTimeSpanFactor = 1)
        {
            return new TenorTimeSpan(tenorTimeSpan.Years + tenorTimeSpanFactor * tenorTimeSpanToAdd.Years, tenorTimeSpan.Months + tenorTimeSpanFactor * tenorTimeSpanToAdd.Months, tenorTimeSpan.Days + tenorTimeSpanFactor * tenorTimeSpanToAdd.Days);
        }

        /// <summary>Adds a specific <see cref="TenorTimeSpan"/> object to a specific <see cref="System.DateTime"/> object.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="tenorTimeSpan">The time span to add in its <see cref="TenorTimeSpan"/> representation.</param>
        /// <returns>The <see cref="System.DateTime"/> which is the result of <paramref name="startDate"/> plus <paramref name="tenorTimeSpan"/>.</returns>
        /// <remarks>If <paramref name="tenorTimeSpan"/> is equal to <see cref="TenorTimeSpan.TomorrowNextTenor"/>, two (2) days will be added to <paramref name="startDate"/>.</remarks>
        public static DateTime AddTenorTimeSpan(this DateTime startDate, TenorTimeSpan tenorTimeSpan)
        {
            DateTime date = startDate;

            if (tenorTimeSpan.Years != 0)
            {
                date = date.AddYears(tenorTimeSpan.Years);
            }
            if (tenorTimeSpan.Months != 0)
            {
                date = date.AddMonths(tenorTimeSpan.Months);
            }
            if (tenorTimeSpan.Days != 0)
            {
                date = date.AddDays(tenorTimeSpan.Days);
            }
            return date;
        }

        /// <summary>Adds a specific <see cref="TenorTimeSpan"/> object to a specific <see cref="System.DateTime"/> object.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="tenorTimeSpan">The time span to add in its <see cref="String"/> representation.</param>
        /// <param name="tenorTimeSpanFactor">A (optional) factor to take into account.</param>
        /// <returns>The <see cref="System.DateTime"/> which is the result of <paramref name="startDate"/> plus <paramref name="tenorTimeSpanFactor"/> * <paramref name="tenorTimeSpan"/>.</returns>
        /// <remarks>If <paramref name="tenorTimeSpan"/> is equal to <see cref="TenorTimeSpan.TomorrowNextTenor"/>, two (2) [times <paramref name="tenorTimeSpanFactor"/>] days will be added to <paramref name="startDate"/>.</remarks>
        public static DateTime AddTenorTimeSpan(this DateTime startDate, string tenorTimeSpan, int tenorTimeSpanFactor = 1)
        {
            DateTime date = startDate;
            int years, months, days;
            TenorType tenorType;
            if (TenorTimeSpan.TryParse(tenorTimeSpan, out years, out months, out days, out tenorType) == false)
            {
                throw new ArgumentException(String.Format(Dodoni.BasicComponents.ExceptionMessages.ArgumentIsInvalid, tenorTimeSpan), "tenorTimeSpan");
            }

            if (years != 0)
            {
                date = date.AddYears(tenorTimeSpanFactor * years);
            }
            if (months != 0)
            {
                date = date.AddMonths(tenorTimeSpanFactor * months);
            }
            if (days != 0)
            {
                date = date.AddDays(tenorTimeSpanFactor * days);
            }
            return date;
        }

        /// <summary>Adds a specific <see cref="TenorTimeSpan"/> object to a specific <see cref="System.DateTime"/> object.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="tenorTimeSpan">The time span to add in some <see cref="TenorTimeSpan"/> representation.</param>
        /// <param name="tenorTimeSpanFactor">A factor to take into account.</param>
        /// <returns>The <see cref="System.DateTime"/> which is the result of <paramref name="startDate"/> plus <paramref name="tenorTimeSpanFactor"/> * <paramref name="tenorTimeSpan"/>.</returns>
        /// <remarks>If <paramref name="tenorTimeSpan"/> is equal to <see cref="TenorTimeSpan.TomorrowNextTenor"/>, two (2) [times <paramref name="tenorTimeSpanFactor"/>] days will be added to <paramref name="startDate"/>.</remarks>
        public static DateTime AddTenorTimeSpan(this DateTime startDate, TenorTimeSpan tenorTimeSpan, int tenorTimeSpanFactor)
        {
            DateTime date = startDate;

            if (tenorTimeSpan.Years != 0)
            {
                date = date.AddYears(tenorTimeSpanFactor * tenorTimeSpan.Years);
            }
            if (tenorTimeSpan.Months != 0)
            {
                date = date.AddMonths(tenorTimeSpanFactor * tenorTimeSpan.Months);
            }
            if (tenorTimeSpan.Days != 0)
            {
                date = date.AddDays(tenorTimeSpanFactor * tenorTimeSpan.Days);
            }
            return date;
        }
    }
}