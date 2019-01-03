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
using Dodoni.BasicComponents.Logging;
using Microsoft.Extensions.Logging;

namespace Dodoni.Finance.MarketConventionTemplates
{
    /// <summary>Extension methods for the logging of standard market conventions.
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static class MarketConventionLogging
    {
        /// <summary>Adds default market conventions that are taken into account into a specific logger.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="marketConventions">The market conventions which are taken into account.</param>
        /// <param name="userMarketConventionInput">The user input of market conventions.</param>
        public static void Add_Info(this ILogger logger, ReadOnlyMoneyMarketConventions marketConventions, MoneyMarketConventions userMarketConventionInput = null)
        {
            if ((logger != null) && (marketConventions != null))
            {
                //if ((userMarketConventionInput == null) || (userMarketConventionInput.BusinessDayConventionState != ConventionState.UserInput))
                //{
                //    logger.Add_Info_StandardValue("Business day convention", marketConventions.BusinessDayConvention.Name);
                //}
                //if ((userMarketConventionInput == null) || (userMarketConventionInput.BusinessDaysToSettleState != ConventionState.UserInput))
                //{
                //    logger.Add_Info_StandardValue("Business days to settle", marketConventions.BusinessDaysToSettle);
                //}
                //if ((userMarketConventionInput == null) || (userMarketConventionInput.DayCountConventionState != ConventionState.UserInput))
                //{
                //    logger.Add_Info_StandardValue("Day count convention", marketConventions.DayCountConvention.Name);
                //}
                //if ((userMarketConventionInput == null) || (userMarketConventionInput.CapletTenorConventionState != ConventionState.UserInput))
                //{
                //    logger.Add_Info_StandardValue("Caplet tenor convention", marketConventions.CapletTenorConvention.Name);
                //}
                //if ((userMarketConventionInput == null) || (userMarketConventionInput.FutureBasePointValueState != ConventionState.UserInput))
                //{
                //    logger.Add_Info_StandardValue("Future base point value", marketConventions.FutureBasePointValue);
                //}
                //if ((userMarketConventionInput == null) || (userMarketConventionInput.LiborRateRoundingRuleState != ConventionState.UserInput))
                //{
                //    logger.Add_Info_StandardValue("Libor Rounding Rule", marketConventions.LiborRateRoundingRule.Name);
                //}
                //if ((userMarketConventionInput == null) || (userMarketConventionInput.FixingLagState != ConventionState.UserInput))
                //{
                //    logger.Add_Info_StandardValue("Fixing Lag", marketConventions.FixingLag.Name);
                //}
            }
        }

        /// <summary>Adds default market conventions that are taken into account into a specific logger.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="marketConventions">The market conventions which are taken into account.</param>
        /// <param name="userMarketConventionInput">The user input of market conventions.</param>
        public static void Add_Info(this ILogger logger, ReadOnlySwapMarketConventions marketConventions, SwapMarketConventions userMarketConventionInput = null)
        {
            if ((logger != null) && (marketConventions != null))
            {
                //if ((userMarketConventionInput == null) || (userMarketConventionInput.BusinessDaysToSettleState != ConventionState.UserInput))
                //{
                //    logger.Add_Info_StandardValue("Business days to settle", marketConventions.BusinessDaysToSettle);
                //}
                //if ((userMarketConventionInput == null) || (userMarketConventionInput.FixingLagState != ConventionState.UserInput))
                //{
                //    logger.Add_Info_StandardValue("Fixing Lag", marketConventions.FixingLag.Name);
                //}
                //if ((userMarketConventionInput == null) || (userMarketConventionInput.FixedBusinessDayConventionState != ConventionState.UserInput))
                //{
                //    logger.Add_Info_StandardValue("Fixed Business day convention", marketConventions.FixedBusinessDayConvention.Name);
                //}
                //if ((userMarketConventionInput == null) || (userMarketConventionInput.FixedDayCountConventionState != ConventionState.UserInput))
                //{
                //    logger.Add_Info_StandardValue("Fixed Day Count convention", marketConventions.FixedDayCountConvention.Name);
                //}
                //if ((userMarketConventionInput == null) || (userMarketConventionInput.FixedFrequencyState != ConventionState.UserInput))
                //{
                //    logger.Add_Info_StandardValue("Fixed frequency", marketConventions.FixedFrequency.Name);
                //}
                //if ((userMarketConventionInput == null) || (userMarketConventionInput.FloatingBusinessDayConventionState != ConventionState.UserInput))
                //{
                //    logger.Add_Info_StandardValue("Floating Business day convention", marketConventions.FloatingBusinessDayConvention.Name);
                //}
                //if ((userMarketConventionInput == null) || (userMarketConventionInput.FloatingDayCountConventionState != ConventionState.UserInput))
                //{
                //    logger.Add_Info_StandardValue("Floating Day Count convention", marketConventions.FloatingDayCountConvention.Name);
                //}
                //if ((userMarketConventionInput == null) || (userMarketConventionInput.FloatingFrequencyState != ConventionState.UserInput))
                //{
                //    logger.Add_Info_StandardValue("Floating frequency", marketConventions.FloatingFrequency.Name);
                //}
            }
        }

        /// <summary>Adds default market conventions that are taken into account into a specific logger.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="marketConventions">The market conventions which are taken into account.</param>
        /// <param name="userMarketConventionInput">The user input of market conventions.</param>
        public static void Add_Info(this ILogger logger, ReadOnlyBondMarketConventions marketConventions, BondMarketConventions userMarketConventionInput = null)
        {
            if ((logger != null) && (marketConventions != null))
            {
                //if ((userMarketConventionInput == null) || (userMarketConventionInput.BusinessDayConventionState != ConventionState.UserInput))
                //{
                //    logger.Add_Info_StandardValue("Business day convention", marketConventions.BusinessDayConvention.Name);
                //}
                //if ((userMarketConventionInput == null) || (userMarketConventionInput.BusinessDaysToSettleState != ConventionState.UserInput))
                //{
                //    logger.Add_Info_StandardValue("Business days to settle", marketConventions.BusinessDaysToSettle);
                //}
                //if ((userMarketConventionInput == null) || (userMarketConventionInput.DayCountConventionState != ConventionState.UserInput))
                //{
                //    logger.Add_Info_StandardValue("Day count convention", marketConventions.DayCountConvention.Name);
                //}
                //if ((userMarketConventionInput == null) || (userMarketConventionInput.CouponFrequencyState != ConventionState.UserInput))
                //{
                //    logger.Add_Info_StandardValue("Coupon frequency", marketConventions.CouponFrequency.Name);
                //}
            }
        }
    }
}