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
using Dodoni.MathLibrary;
using Dodoni.BasicComponents;
using Dodoni.Finance.DateFactory;
using Dodoni.Finance.MarketConventionTemplates;

namespace Dodoni.Finance.CommonMarketUsages.Currencies
{
    /// <summary>Represents the currency 'GBP', British pound.
    /// </summary>
    public class GBP : Currency
    {
        #region private static members

        /// <summary>The (ISO) name of the currency.
        /// </summary>
        private static readonly IdentifierString sm_Name = new IdentifierString("GBP");

        /// <summary>The (perhaps) language dependent long name of the currency.
        /// </summary>
        private static readonly string sm_LongName = "British Pound";
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="GBP"/> class.
        /// </summary>
        public GBP()
            : base(sm_Name, GetStandardMarketConventions(), sm_LongName)
        {
        }
        #endregion

        #region private static methods

        /// <summary>Gets the standard market conventions for the currency.
        /// </summary>
        /// <returns>The standard market conventions for the currency 'GBP'.</returns>
        private static CurrencyMarketConventions GetStandardMarketConventions()
        {
            // Bond market conventions:
            BondMarketConventions bondMarketConventions = new BondMarketConventions();
            bondMarketConventions.BusinessDayConvention = BusinessDayConvention.ModFollowing;
            bondMarketConventions.DayCountConvention = DayCountConvention.Actual365;
            bondMarketConventions.CouponFrequency = DateScheduleFrequency.SemiAnnually;
            bondMarketConventions.BusinessDaysToSettle = 0;

            // Money market conventions:
            MoneyMarketConventions moneyMarketConventions = new MoneyMarketConventions();
            moneyMarketConventions.BusinessDayConvention = BusinessDayConvention.ModFollowing;
            moneyMarketConventions.BusinessDaysToSettle = 0;
            moneyMarketConventions.FixingLag = FixingLag.None;
            moneyMarketConventions.CapletTenorConvention = IRCapletTenorConvention.CapletTenor6M;
            moneyMarketConventions.DayCountConvention = DayCountConvention.Actual365;
            moneyMarketConventions.FutureBasePointValue = 25;
            moneyMarketConventions.LiborRateRoundingRule = RoundingRule.NoRounding;
            moneyMarketConventions.LiborIndexName = new IdentifierString("GBPLibor");

            // Swap market conventions:
            SwapMarketConventions swapMarketConventions = new SwapMarketConventions();
            swapMarketConventions.BusinessDaysToSettle = 0;
            swapMarketConventions.FixingLag = FixingLag.None;
            swapMarketConventions.FixedBusinessDayConvention = BusinessDayConvention.ModFollowing;
            swapMarketConventions.FixedDayCountConvention = DayCountConvention.Actual365;
            swapMarketConventions.FixedFrequency = DateScheduleFrequency.SemiAnnually;
            swapMarketConventions.FloatingBusinessDayConvention = BusinessDayConvention.ModFollowing;
            swapMarketConventions.FloatingDayCountConvention = DayCountConvention.Actual365;
            swapMarketConventions.FloatingFrequency = DateScheduleFrequency.SemiAnnually;

            // Credit market conventions:
            CreditMarketConventions creditMarketConventions = new CreditMarketConventions();

            // Inflation market conventions:
            InflationMarketConventions inflationMarketConventions = new InflationMarketConventions();
            inflationMarketConventions.RoundingRule = RoundingRule.NoRounding;
            inflationMarketConventions.BondMarket.BusinessDayConvention = BusinessDayConvention.ModFollowing;
            inflationMarketConventions.BondMarket.CouponFrequency = DateScheduleFrequency.Annually;
            inflationMarketConventions.BondMarket.DayCountConvention = DayCountConvention.OneOverOne;

            inflationMarketConventions.SwapMarket.BusinessDaysToSettle = 0;
            inflationMarketConventions.SwapMarket.FixedBusinessDayConvention = BusinessDayConvention.ModFollowing;
            inflationMarketConventions.SwapMarket.FixedDayCountConvention = DayCountConvention.OneOverOne;
            inflationMarketConventions.SwapMarket.FixedFrequency = DateScheduleFrequency.Annually;
            inflationMarketConventions.SwapMarket.FloatingBusinessDayConvention = BusinessDayConvention.ModFollowing;
            inflationMarketConventions.SwapMarket.FloatingDayCountConvention = DayCountConvention.OneOverOne;
            inflationMarketConventions.SwapMarket.FloatingFrequency = DateScheduleFrequency.Annually;

            return new CurrencyMarketConventions(bondMarketConventions.AsReadOnly(), creditMarketConventions.AsReadOnly(), inflationMarketConventions.AsReadOnly(), moneyMarketConventions.AsReadOnly(), swapMarketConventions.AsReadOnly(), HolidayCalendar.Europe.LND);
        }
        #endregion
    }
}