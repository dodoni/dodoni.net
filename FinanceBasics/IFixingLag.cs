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

using Dodoni.BasicComponents;

namespace Dodoni.Finance
{
    /// <summary>Serves as interface for the calculation of a specific fixing date (of a specific underlying), where the fixing date depends on the start date of a specific (interest) period.
    /// </summary>
    /// <example>For example the fixing date can be the start date of the interest period minus 2 business days.</example>
    public interface IFixingLag : IIdentifierNameable
    {
        /// <summary>Gets a specific fixing date.
        /// </summary>
        /// <param name="periodStartDate">The start date of the (interest) period.</param>
        /// <param name="holidayCalendar">The (settlement) calendar.</param>
        /// <returns>The specific fixing date.</returns>
        DateTime GetFixingDate(DateTime periodStartDate, IHolidayCalendar holidayCalendar);
    }
}