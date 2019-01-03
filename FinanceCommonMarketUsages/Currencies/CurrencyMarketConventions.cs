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
using Dodoni.BasicComponents;
using Dodoni.Finance.MarketConventionTemplates;
using Dodoni.Finance.CommonMarketUsages.HolidayCalendars;

namespace Dodoni.Finance.CommonMarketUsages.Currencies
{
    /// <summary>Serves as immutable class which contains the Market conventions for some currency.
    /// </summary>
    public class CurrencyMarketConventions : ReadOnlyMarketConventions
    {
        #region private members

        /// <summary>The standard (settlement) holiday calendar.
        /// </summary>
        private IHolidayCalendar m_HolidayCalendar;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="CurrencyMarketConventions"/> class.
        /// </summary>
        /// <param name="bondMarketConventions">The bond market conventions.</param>
        /// <param name="creditMarketConventions">The credit market conventions.</param>
        /// <param name="inflationMarketConventions">The inflation market conventions.</param>
        /// <param name="moneyMarketConventions">The money market conventions.</param>
        /// <param name="swapMarketConventions">The swap market conventions.</param>
        /// <param name="holidayCalendar">The standard (settlement) holiday calendar.</param>
        /// <exception cref="ArgumentNullException">Thrown, if one of the arguments is <c>null</c>.</exception>
        public CurrencyMarketConventions(ReadOnlyBondMarketConventions bondMarketConventions, ReadOnlyCreditMarketConventions creditMarketConventions, ReadOnlyInflationMarketConventions inflationMarketConventions, ReadOnlyMoneyMarketConventions moneyMarketConventions, ReadOnlySwapMarketConventions swapMarketConventions, IHolidayCalendar holidayCalendar)
            : base(bondMarketConventions, creditMarketConventions, inflationMarketConventions, moneyMarketConventions, swapMarketConventions)
        {
            if (holidayCalendar == null)
            {
                throw new ArgumentNullException("holidayCalendar");
            }
            m_HolidayCalendar = holidayCalendar;
        }
        #endregion

        #region protected methods

        /// <summary>Gets the (settlement) holiday calendar.
        /// </summary>
        /// <returns>The (settlement) holiday calendar.</returns>
        protected override IHolidayCalendar GetHolidayCalendar()
        {
            return m_HolidayCalendar;
        }
        #endregion
    }
}