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

using Dodoni.MathLibrary;
using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Containers;

namespace Dodoni.Finance.MarketConventionTemplates
{
    /// <summary>Serves as read-only wrapper class for Money market conventions.
    /// </summary>
    public class ReadOnlyMoneyMarketConventions : IInfoOutputQueriable
    {
        #region public (readonly) members

        /// <summary>The the number of business days to settle with respect to the Money market.
        /// </summary>
        public readonly int BusinessDaysToSettle;

        /// <summary>The fixing lag, i.e. the number of business days taken into account for the calculation of fixing dates etc., for example <c>-2</c>.
        /// </summary>
        public readonly IFixingLag FixingLag;

        /// <summary>The day count convention.
        /// </summary>
        public readonly IDayCountConvention DayCountConvention;

        /// <summary>The business day convention.
        /// </summary>
        public readonly IBusinessDayConvention BusinessDayConvention;

        /// <summary>The caplet tenor convention needed for some caplet stripping.
        /// </summary>
        public readonly IIRCapletTenorConvention CapletTenorConvention;

        /// <summary>The standard name of the Libor index.
        /// </summary>
        public readonly IdentifierString LiborIndexName;

        /// <summary>The rounding rule for Libor rates.
        /// </summary>
        public readonly IRoundingRule LiborRateRoundingRule;

        /// <summary>The base point value of a future, i.e. the price of a future is equal to the 
        /// quote times the 'future base point value' / 1 basis point
        /// </summary>
        public readonly double FutureBasePointValue;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="ReadOnlyMoneyMarketConventions"/> class.
        /// </summary>
        /// <param name="moneyMarketConventions">The money market conventions.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="moneyMarketConventions"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="moneyMarketConventions"/> is not completely defined.</exception>       
        public ReadOnlyMoneyMarketConventions(MoneyMarketConventions moneyMarketConventions)
        {
            if (moneyMarketConventions == null)
            {
                throw new ArgumentNullException("moneyMarketConventions");
            }
            if (moneyMarketConventions.IsCompletelyDefined == false)
            {
                throw new ArgumentException(String.Format(ExceptionMessages.ArgumentIsNotWellDefined, "Money market conventions"), "moneyMarketConventions");
            }
            BusinessDayConvention = moneyMarketConventions.BusinessDayConvention;
            FixingLag = moneyMarketConventions.FixingLag;
            BusinessDaysToSettle = moneyMarketConventions.BusinessDaysToSettle.Value;
            CapletTenorConvention = moneyMarketConventions.CapletTenorConvention;
            DayCountConvention = moneyMarketConventions.DayCountConvention;
            FutureBasePointValue = moneyMarketConventions.FutureBasePointValue;
            LiborIndexName = moneyMarketConventions.LiborIndexName;
            LiborRateRoundingRule = moneyMarketConventions.LiborRateRoundingRule;
        }
        #endregion

        #region public properties

        #region IInfoOutputQueriable Members

        /// <summary>Gets the info-output level of detail.
        /// </summary>
        /// <value>The info-output level of detail.</value>
        public InfoOutputDetailLevel InfoOutputDetailLevel
        {
            get { return InfoOutputDetailLevel.Full; }
        }
        #endregion

        #endregion

        #region public methods

        #region IInfoOutputQueriable Members

        /// <summary>Sets the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel"/> property.
        /// </summary>
        /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
        /// <returns>A value indicating whether the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel"/> has been changed
        /// with respect to <paramref name="infoOutputDetailLevel"/>.
        /// </returns>
        public bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
        {
            return (infoOutputDetailLevel == InfoOutputDetailLevel.Full);
        }

        /// <summary>Gets informations of the current object as a specific <see cref="InfoOutput"/> instance.
        /// </summary>
        /// <param name="infoOutput">The <see cref="InfoOutput"/> object which is to be filled with informations concering the current instance.</param>
        /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category name.</param>
        public void FillInfoOutput(InfoOutput infoOutput, string categoryName)
        {
            InfoOutputPackage infoOutputCollection = infoOutput.AcquirePackage(categoryName);
            infoOutputCollection.Add("Business days to settle", BusinessDaysToSettle);
            infoOutputCollection.Add("Fixing lag", FixingLag.Name.String);
            infoOutputCollection.Add("Business day convention", BusinessDayConvention.Name.String);
            infoOutputCollection.Add("Day count convention", DayCountConvention.Name.String);
            infoOutputCollection.Add("Future base point value", FutureBasePointValue);
            infoOutputCollection.Add("Libor rate rounding rule", LiborRateRoundingRule.Name.String);
            infoOutputCollection.Add("Caplet tenor convention", CapletTenorConvention.Name.String);
            infoOutputCollection.Add("Libor index name", LiborIndexName);
        }
        #endregion

        /// <summary>Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return String.Format("Money market conventions ({0}, {1}, Business days to settle: {2}, Future base point value: {3}, Libor rounding: {4}, Caplet tenor convention: {5}, Fixing Lag: {6})", DayCountConvention, BusinessDayConvention, BusinessDaysToSettle, FutureBasePointValue, LiborRateRoundingRule, CapletTenorConvention, FixingLag);
        }

        /// <summary>Gets a mutable Money Market convention template which is initially filled with values of 
        /// the current instance, i.e. with some standard conventions.</summary>
        /// <returns>A mutable Money Market convention template.</returns>
        public MoneyMarketConventions GetMutableTemplate()
        {
            MoneyMarketConventions marketConventions = new MoneyMarketConventions();
            marketConventions.SetStandardBusinessDayConvention(BusinessDayConvention);
            marketConventions.SetStandardDayCountconvention(DayCountConvention);
            marketConventions.SetStandardFutureBasePointValue(FutureBasePointValue);
            marketConventions.SetStandardLiborIndexName(LiborIndexName);
            marketConventions.SetStandardLiborRateRoundingRule(LiborRateRoundingRule);
            marketConventions.SetStandardBusinessDaysToSettle(BusinessDaysToSettle);
            marketConventions.SetStandardCapletTenorConvention(CapletTenorConvention);
            marketConventions.SetStandardFixingLag(FixingLag);

            return marketConventions;
        }
        #endregion
    }
}