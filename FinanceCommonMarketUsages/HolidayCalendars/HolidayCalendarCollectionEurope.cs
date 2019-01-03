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
    /// <summary>The holiday calendars with respect to 'Europe'.
    /// </summary>
    public class HolidayCalendarCollectionEurope
    {
        #region public (readonly) members

        /// <summary>'TARGET' (Trans-European Automated Real-time Gross settlement Express Transfer).
        /// </summary>
        public readonly IHolidayCalendar TARGET = HolidayCalendar.Create(typeof(Europe.TARGET));

        /// <summary>'FFN', i.e. holiday calendar with respect to Frankfurt am Main, Germany.
        /// </summary>
        public readonly IHolidayCalendar FFM = HolidayCalendar.Create(typeof(Europe.FFM));

        /// <summary>The holiday calendar with respect to Eurex, Frankfurt am Main, Germany.
        /// </summary>
        public readonly IHolidayCalendar FFMEurex = HolidayCalendar.Create(typeof(Europe.FFM_Eurex));

        /// <summary>The holiday calendar with respect to Stock exchange, Frankfurt am Main, Germany.
        /// </summary>
        public readonly IHolidayCalendar FMMStockExchange = HolidayCalendar.Create(typeof(Europe.FFM_StockExchange));

        /// <summary>The holiday calendar with respect to Xetra, Frankfurt am Main, Germany.
        /// </summary>
        public readonly IHolidayCalendar FFMXetra = HolidayCalendar.Create(typeof(Europe.FFM_Xetra));

        /// <summary>'LND', i.e. holiday calendar with respect to London, GB.
        /// </summary>
        public readonly IHolidayCalendar LND = HolidayCalendar.Create(typeof(Europe.LND));

        /// <summary>The holiday calendar with respect to London Exchange, GB.
        /// </summary>
        public readonly IHolidayCalendar LNDExchange = HolidayCalendar.Create(typeof(Europe.LND_Exchange));

        /// <summary>The holiday calendar with respect to London Metal, GB.
        /// </summary>
        public readonly IHolidayCalendar LNDMetal = HolidayCalendar.Create(typeof(Europe.LND_Metal));
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="HolidayCalendarCollectionEurope"/> class.
        /// </summary>
        internal HolidayCalendarCollectionEurope()
        {
        }
        #endregion

        #region internal methods

        /// <summary>Registers the holiday calendars.
        /// </summary>
        /// <param name="eventArgs">The <see cref="Dodoni.Finance.CommonMarketUsages.HolidayCalendar.InitializeEventArgs"/> instance containing the event data.</param>
        internal void RegisterHolidayCalendars(HolidayCalendar.InitializeEventArgs eventArgs)
        {
            eventArgs.Add(TARGET, FFM, FFMEurex, FFMXetra, FMMStockExchange, LND, LNDExchange, LNDMetal);
        }
        #endregion
    }
}