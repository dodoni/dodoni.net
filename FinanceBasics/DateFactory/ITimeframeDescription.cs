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

using Dodoni.BasicComponents.Logging;
using Dodoni.BasicComponents.Containers;
using Microsoft.Extensions.Logging;

namespace Dodoni.Finance.DateFactory
{
    /// <summary>Represents a timeframe, i.e. two <see cref="System.DateTime"/> objects, where these dates may depends on a reference date, for example 
    /// the trade date etc. and the end date of the time span is perhaps given perhaps given in its <see cref="TenorTimeSpan"/> representation by the user. 
    /// </summary>
    /// <remarks>Perhaps a fixing date of a specific underlying index with respect to the time period is provided, for example start date minus 2 business days.</remarks>
    public interface ITimeframeDescription : IInfoOutputQueriable
    {
        /// <summary>Gets the start and end date of the timeframe.
        /// </summary>
        /// <param name="referenceDate">A reference date (can be a business day or a holiday).</param>
        /// <param name="holidayCalendar">The (settlement) holiday calendar.</param>
        /// <param name="startDate">The start date of the time span with respect to <paramref name="referenceDate"/> (output).</param>
        /// <param name="endDate">The end date of the time span with respect to <paramref name="referenceDate"/> (output).</param>
        /// <param name="logger">An optional logger.</param>
        void GetStartAndEndDate(DateTime referenceDate, IHolidayCalendar holidayCalendar, out DateTime startDate, out DateTime endDate, ILogger logger = null);

        /// <summary>Gets the start and end date of the timeframe as well as a <see cref="TenorTimeSpan"/> representation of the time span between both dates.
        /// </summary>
        /// <param name="referenceDate">A reference date (can be a business day or a holiday).</param>
        /// <param name="holidayCalendar">The (settlement) holiday calendar.</param>
        /// <param name="startDate">The start date of the time span with respect to referenceDate (output).</param>
        /// <param name="endDate">The end date of the time span with respect to referenceDate (output).</param>
        /// <param name="tenor">The tenor that represents the time span; if the end date of the current object is not specified by its <see cref="TenorTimeSpan"/> 
        /// representation, <paramref name="tenorRoundingRule"/> will be applied to <paramref name="startDate"/> and <paramref name="endDate"/> for the calculation of a <see cref="TenorTimeSpan"/> representation.</param>
        /// <param name="tenorRoundingRule">A rounding rule which will be take into account if and only if the end date of the current object is not specified by its <see cref="TenorTimeSpan"/> representation.</param>
        /// <param name="logger">An optional logger.</param>
        void GetStartAndEndDate(DateTime referenceDate, IHolidayCalendar holidayCalendar, out DateTime startDate, out DateTime endDate, out TenorTimeSpan tenor, TenorTimeSpan.RoundingRule tenorRoundingRule = TenorTimeSpan.RoundingRule.NearestMonth, ILogger logger = null);

        /// <summary>Gets the start and end date of the timeframe.
        /// </summary>
        /// <param name="referenceDate">A reference date (can be a business day or a holiday).</param>
        /// <param name="holidayCalendar">The (settlement) holiday calendar.</param>
        /// <param name="fixingLag">The fixing lag, i.e. the number of business days taken into account for the calculation of fixing dates etc., for example <c>-2</c>.</param>
        /// <param name="startDate">The start date of the time span with respect to <paramref name="referenceDate"/> (output).</param>
        /// <param name="endDate">The end date of the time span with respect to <paramref name="referenceDate"/> (output).</param>
        /// <param name="logger">An optional logger.</param>
        /// <returns>The fixing date of with respect to the (interest) period.</returns>
        DateTime GetStartAndEndDate(DateTime referenceDate, IHolidayCalendar holidayCalendar, IFixingLag fixingLag, out DateTime startDate, out DateTime endDate, ILogger logger = null);

        /// <summary>Gets the start and end date of the timeframe as well as a <see cref="TenorTimeSpan"/> representation of the time span between both dates.
        /// </summary>
        /// <param name="referenceDate">A reference date (can be a business day or a holiday).</param>
        /// <param name="holidayCalendar">The (settlement) holiday calendar.</param>
        /// <param name="fixingLag">The fixing lag, i.e. the number of business days taken into account for the calculation of fixing dates etc., for example <c>-2</c>.</param>
        /// <param name="startDate">The start date of the time span with respect to referenceDate (output).</param>
        /// <param name="endDate">The end date of the time span with respect to referenceDate (output).</param>
        /// <param name="tenor">The tenor that represents the time span; if the end date of the current object is not specified by its <see cref="TenorTimeSpan"/> 
        /// representation, <paramref name="tenorRoundingRule"/> will be applied to <paramref name="startDate"/> and <paramref name="endDate"/> for the calculation of a <see cref="TenorTimeSpan"/> representation.</param>
        /// <param name="tenorRoundingRule">A rounding rule which will be take into account if and only if the end date of the current object is not specified by its <see cref="TenorTimeSpan"/> representation.</param>
        /// <param name="logger">An optional logger.</param>
        /// <returns>The fixing date of with respect to the (interest) period.</returns>
        DateTime GetStartAndEndDate(DateTime referenceDate, IHolidayCalendar holidayCalendar, IFixingLag fixingLag, out DateTime startDate, out DateTime endDate, out TenorTimeSpan tenor, TenorTimeSpan.RoundingRule tenorRoundingRule = TenorTimeSpan.RoundingRule.NearestMonth, ILogger logger = null);

        /// <summary>Gets the raw end date of the current <see cref="ITimeframeDescription"/>; where the raw end date is in general represented in a specific <see cref="TenorTimeSpan"/> or <see cref="System.DateTime"/> object.
        /// </summary>
        /// <returns>The raw end date of the current <see cref="ITimeframeDescription"/>; where the raw end date is in general represented in a specific <see cref="TenorTimeSpan"/> or <see cref="System.DateTime"/> object.</returns>
        object GetRawEndDate();
    }
}