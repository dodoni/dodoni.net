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

namespace Dodoni.Finance
{
    /// <summary>Serves as interface for business day conventions.
    /// </summary>
    public interface IBusinessDayConvention : IIdentifierNameable, IAnnotatable
    {
        ///<summary>Gets an adjusted date with respect to a specific <see cref="System.DateTime"/> object.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="holidayCalendar">The holiday calendar.</param>
        /// <returns>The <see cref="System.DateTime"/> object that is given by <paramref name="date"/> taken into account the business day 
        /// convention represented by the current instance.
        /// </returns>
        /// <remarks>Perhaps the return value is not a business day, for example in the case of some end-of-month adjustment or 'no adjustment'.</remarks>
        DateTime GetAdjustedDate(DateTime date, IHolidayCalendar holidayCalendar);

        /// <summary>Gets the type of the adjustment, i.e. a value indicating whether the result of <see cref="IBusinessDayConvention.GetAdjustedDate(DateTime, IHolidayCalendar)"/> is a business day.
        /// </summary>
        /// <value>The type of the date adjustment.</value>
        BusinessDayAdjustmentType AdjustmentType
        {
            get;
        }
    }
}