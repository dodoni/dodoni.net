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

namespace Dodoni.Finance.MarketConventionTemplates
{
    /// <summary>Serves as read-only wrapper for Market conventions, i.e. for Bond market, Swap market etc. 
    /// </summary>
    /// <remarks>Each currency contains a <see cref="ReadOnlyMarketConventions"/> object which represents the
    /// standard Market conventions with respect to the specific currency.</remarks>
    public abstract class ReadOnlyMarketConventions
    {
        #region public (readonly) members

        /// <summary>The Bond Market conventions.
        /// </summary>
        public readonly ReadOnlyBondMarketConventions BondMarketConventions;

        /// <summary>The Credit Market conventions.
        /// </summary>
        public readonly ReadOnlyCreditMarketConventions CreditMarketConventions;

        /// <summary>The Inflation Market conventions.
        /// </summary>
        public readonly ReadOnlyInflationMarketConventions InflationMarketConventions;

        /// <summary>The Money Market conventions.
        /// </summary>
        public readonly ReadOnlyMoneyMarketConventions MoneyMarketConventions;

        /// <summary>The Swap Market conventions.
        /// </summary>
        public readonly ReadOnlySwapMarketConventions SwapMarketConventions;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="ReadOnlyMarketConventions"/> class.
        /// </summary>
        /// <param name="bondMarketConventions">The bond market conventions.</param>
        /// <param name="creditMarketConventions">The credit market conventions.</param>
        /// <param name="inflationMarketConventions">The inflation market conventions.</param>
        /// <param name="moneyMarketConventions">The money market conventions.</param>
        /// <param name="swapMarketConventions">The swap market conventions.</param>
        /// <exception cref="ArgumentNullException">Thrown, if one of the arguments is <c>null</c>.</exception>
        public ReadOnlyMarketConventions(ReadOnlyBondMarketConventions bondMarketConventions, ReadOnlyCreditMarketConventions creditMarketConventions, ReadOnlyInflationMarketConventions inflationMarketConventions, ReadOnlyMoneyMarketConventions moneyMarketConventions, ReadOnlySwapMarketConventions swapMarketConventions)
        {
            if (bondMarketConventions == null)
            {
                throw new ArgumentNullException("bondMarketConventions");
            }
            BondMarketConventions = bondMarketConventions;

            if (creditMarketConventions == null)
            {
                throw new ArgumentNullException("creditMarketConventions");
            }
            CreditMarketConventions = creditMarketConventions;

            if (inflationMarketConventions == null)
            {
                throw new ArgumentNullException("inflationMarketConventions");
            }
            InflationMarketConventions = inflationMarketConventions;

            if (moneyMarketConventions == null)
            {
                throw new ArgumentNullException("moneyMarketConventions");
            }
            MoneyMarketConventions = moneyMarketConventions;

            if (swapMarketConventions == null)
            {
                throw new ArgumentNullException("swapMarketConventions");
            }
            SwapMarketConventions = swapMarketConventions;
        }
        #endregion

        #region public properties

        /// <summary>Gets the (settlement) holiday calendar.
        /// </summary>
        /// <value>The (settlement) holiday calendar.</value>
        public IHolidayCalendar HolidayCalendar
        {
            get { return GetHolidayCalendar(); }
        }
        #endregion

        #region protected methods

        /// <summary>Gets the (settlement) holiday calendar.
        /// </summary>
        /// <returns>The (settlement) holiday calendar.</returns>
        /// <remarks>Perhaps the holiday calendar is a element of a holiday calendar pool and referenced by its name only. If 
        /// the user has overwritten the standard holiday calendar implementation, then this new implementation will be used.</remarks>
        protected abstract IHolidayCalendar GetHolidayCalendar();
        #endregion
    }
}