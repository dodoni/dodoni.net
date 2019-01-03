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
using Dodoni.Finance.DateFactory;
using Dodoni.BasicComponents.Logging;
using Dodoni.Finance.MarketConventionTemplates;
using Microsoft.Extensions.Logging;

namespace Dodoni.Finance
{
    /// <summary>Serves as interface for (interest rate) caplet tenors, i.e. the tenors which are used for the calculation of caplets for a specified cap price/volatility and the date schedule of the payment dates of the caplets.
    /// </summary>
    public interface IIRCapletTenorConvention : IIdentifierNameable
    {
        /// <summary>Gets a value indicating whether this instance has a unique Libor tenor, i.e. each caplet is given with respect to the same Libor rate.
        /// </summary>
        /// <value><c>true</c> if this instance has unique libor tenor; otherwise, <c>false</c>.</value>
        bool HasUniqueLiborTenor { get; }

        /// <summary>Gets the tenor of the underlying Libor rate if the tenor is equal for each caplet.
        /// </summary>
        /// <param name="liborTenor">The (unique) tenor of the underlying Libor rate.</param>
        /// <returns>A value indicating whether <paramref name="liborTenor"/> contains valid data and a unique tenor is available.</returns>
        /// <remarks>This method returns the same value as <see cref="IIRCapletTenorConvention.HasUniqueLiborTenor"/>.</remarks>
        bool TryGetUniqueUnderlyingLiborTenor(out TenorTimeSpan liborTenor);

        /// <summary>Creates a date schedule that contains the start and end dates of the interest periods of each caplets, i.e. T_0, T_1, T_2, ..., where [T_k; T_{k+1}] is the
        /// interest period of caplet k, k=0,...,n-1; The first caplet is already expired but it is part of the date schedule.
        /// </summary>
        /// <param name="referenceDate">The reference date, i.e. the trading date.</param>
        /// <param name="startDateAndEndDateDescription">A description of the start date of the first caplet as well as the end date of the last caplet, i.e. the start date and the maturity of the cap.</param>
        /// <param name="marketConventions">The market conventions.</param>
        /// <param name="holidayCalendar">The holiday calendar.</param>
        /// <param name="underlyingLiborTenor">A mapping of the null-based index of the start date of each caplet interest period to the tenor of the underlying Libor rate (output).</param>
        /// <param name="logger">An optional logger.</param>
        /// <returns>The date schedule of the interest periods, i.e. the start and end dates of each caplet; thus T_0, T_1, T_2, ..., where [T_k; T_{k+1}] is the
        /// interest period of caplet k, k=0,...,n-1; The first caplet is already expired but it is part of the date schedule.</returns>
        ReadOnlyDateSchedule CreateInterestPeriodDateSchedule(DateTime referenceDate, ITimeframeDescription startDateAndEndDateDescription, ReadOnlyMoneyMarketConventions marketConventions, IHolidayCalendar holidayCalendar, out Func<int, TenorTimeSpan> underlyingLiborTenor, ILogger logger = null);
    }
}