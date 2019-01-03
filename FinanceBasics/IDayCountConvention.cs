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

namespace Dodoni.Finance
{
    /// <summary>Serves as interface for day count conventions.
    /// </summary>
    /// <remarks>
    /// <para>Some exotic day count conventions needs some additional reference period or a holiday calendar
    /// as additional input, for example BU/252 (Brazilian trades), ISMA Act/Act (Bond).</para>
    /// </remarks>
    public interface IDayCountConvention : IIdentifierNameable, IAnnotatable
    {
        /// <summary>Gets a value indicating how to interpret the parameters <code>referenceStartDate</code> and <code>referenceEndDate</code> 
        /// in <see cref="IDayCountConvention.GetYearFraction(DateTime, DateTime, DateTime?, DateTime?)"/> and <see cref="IDayCountConvention.GetYearFraction(DateTime, DateTime, out int, DateTime?, DateTime?)"/>.
        /// </summary>
        /// <value>A value indicating whether a reference period is used.
        /// </value>
        DayCountReferencePeriodUsage ReferencePeriodUsage
        {
            get;
        }

        /// <summary>Gets the date year fraction between two <see cref="System.DateTime"/> objects.
        /// </summary>
        /// <param name="startDate">The start date of the period.</param>
        /// <param name="endDate">The end date of the period.</param>
        /// <param name="referenceStartDate">The (optional) reference start date; see <see cref="IDayCountConvention.ReferencePeriodUsage"/> 
        /// for its interpretation. Can be set to <c>null</c> in any case.</param>
        /// <param name="referenceEndDate">The (optional) reference end date; see <see cref="IDayCountConvention.ReferencePeriodUsage"/> 
        /// for its interpretation. Can be set to <c>null</c> in any case.</param>
        /// <returns>The date year fraction for the period [<paramref name="startDate"/>; <paramref name="endDate"/>].</returns>
        double GetYearFraction(DateTime startDate, DateTime endDate, DateTime? referenceStartDate = null, DateTime? referenceEndDate = null);

        /// <summary>Gets the date year fraction between two <see cref="System.DateTime"/> objects.
        /// </summary>
        /// <param name="startDate">The start date of the period.</param>
        /// <param name="endDate">The end date of the period.</param>
        /// <param name="dayDifference">The number of days in the period (output).</param>
        /// <param name="referenceStartDate">The (optional) reference start date; see <see cref="IDayCountConvention.ReferencePeriodUsage"/> 
        /// for its interpretation. Can be set to <c>null</c> in any case.</param>
        /// <param name="referenceEndDate">The (optional) reference end date; see <see cref="IDayCountConvention.ReferencePeriodUsage"/> 
        /// for its interpretation. Can be set to <c>null</c> in any case.</param>
        /// <returns>The date year fraction for the period [<paramref name="startDate"/>; <paramref name="endDate"/>].</returns>
        double GetYearFraction(DateTime startDate, DateTime endDate, out int dayDifference, DateTime? referenceStartDate = null, DateTime? referenceEndDate = null);
    }
}