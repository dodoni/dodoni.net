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
    /// <summary>The holiday calendars with respect to 'Africa'.
    /// </summary>
    public class HolidayCalendarCollectionAfrica
    {
        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="HolidayCalendarCollectionAfrica"/> class.
        /// </summary>
        internal HolidayCalendarCollectionAfrica()
        {
        }
        #endregion

        #region internal methods

        /// <summary>Registers the holiday calendars.
        /// </summary>
        /// <param name="eventArgs">The <see cref="Dodoni.Finance.CommonMarketUsages.HolidayCalendar.InitializeEventArgs"/> instance containing the event data.</param>
        internal void RegisterHolidayCalendars(HolidayCalendar.InitializeEventArgs eventArgs)
        {
        }
        #endregion
    }
}