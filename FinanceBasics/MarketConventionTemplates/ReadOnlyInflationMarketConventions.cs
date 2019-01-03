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
using Dodoni.Finance.DateFactory;
using Dodoni.BasicComponents.Containers;

namespace Dodoni.Finance.MarketConventionTemplates
{
    /// <summary>Serves as read-only wrapper class for Inflation Market conventions.
    /// </summary>
    public class ReadOnlyInflationMarketConventions : IInfoOutputQueriable
    {
        #region nested classes

        /// <summary>Serves as read-only wrapper class for Market conventions for inflation-linked swaps.
        /// </summary>
        public class InflationSwapConventions : IInfoOutputQueriable
        {
            #region public (readonly) members

            /// <summary>The number of business days to settle.
            /// </summary>
            public readonly int BusinessDaysToSettle;

            /// <summary>The frequency of the fixed rate leg of inflation-linked swaps.
            /// </summary>
            public readonly IDateScheduleFrequency FixedFrequency;

            /// <summary>The frequency of the floating rate leg of inflation-linked swaps.
            /// </summary>
            public readonly IDateScheduleFrequency FloatingFrequency;

            /// <summary>The day count convention of the fixed rate leg of inflation-linked swaps.
            /// </summary>
            public readonly IDayCountConvention FixedDayCountConvention;

            /// <summary>The day count convention of the floating rate leg of inflation-linked swaps.
            /// </summary>
            public readonly IDayCountConvention FloatingDayCountConvention;

            /// <summary>The business day convention of the fixed rate leg of inflation-linked swaps.
            /// </summary>
            public readonly IBusinessDayConvention FixedBusinessDayConvention;

            /// <summary>The business day convention of the floating rate leg of inflation-linked swaps.
            /// </summary>
            public readonly IBusinessDayConvention FloatingBusinessDayConvention;
            #endregion

            #region public constructors

            /// <summary>Initializes a new instance of the <see cref="InflationSwapConventions"/> class.
            /// </summary>
            /// <param name="swapMarketConventions">The swap market conventions.</param>
            public InflationSwapConventions(InflationMarketConventions.InflationSwapConventions swapMarketConventions)
            {
                if (swapMarketConventions == null)
                {
                    throw new ArgumentNullException("swapMarketConventions");
                }
                if (swapMarketConventions.IsCompletelyDefined == false)
                {
                    throw new ArgumentException(String.Format(ExceptionMessages.ArgumentIsNotWellDefined, "Inflation-linked Swap market conventions"), "swapMarketConventions");
                }
                BusinessDaysToSettle = swapMarketConventions.BusinessDaysToSettle.Value;
                FixedBusinessDayConvention = swapMarketConventions.FixedBusinessDayConvention;
                FixedDayCountConvention = swapMarketConventions.FixedDayCountConvention;
                FixedFrequency = swapMarketConventions.FixedFrequency;
                FloatingBusinessDayConvention = swapMarketConventions.FloatingBusinessDayConvention;
                FloatingDayCountConvention = swapMarketConventions.FloatingDayCountConvention;
                FloatingFrequency = swapMarketConventions.FloatingFrequency;
            }
            #endregion

            #region public properties

            #region IInfoOutputQueriable Members

            /// <summary>Gets the info-output level of detail.
            /// </summary>
            /// <value>The info-output level of detail.</value>
            InfoOutputDetailLevel IInfoOutputQueriable.InfoOutputDetailLevel
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
            bool IInfoOutputQueriable.TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
            {
                return (infoOutputDetailLevel == InfoOutputDetailLevel.Full);
            }

            /// <summary>Gets informations of the current object as a specific <see cref="InfoOutput"/> instance.
            /// </summary>
            /// <param name="infoOutput">The <see cref="InfoOutput"/> object which is to be filled with informations concering the current instance.</param>
            /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category name.</param>
            void IInfoOutputQueriable.FillInfoOutput(InfoOutput infoOutput, string categoryName)
            {
                InfoOutputPackage infoOutputCollection = infoOutput.AcquirePackage(categoryName);
                infoOutputCollection.Add("Business days to settle", BusinessDaysToSettle);
                infoOutputCollection.Add("Fixed leg Business day convention", FixedBusinessDayConvention.Name.String);
                infoOutputCollection.Add("Fixed leg Day count convention", FixedDayCountConvention.Name.String);
                infoOutputCollection.Add("Fixed leg Frequency", FixedFrequency.Name.String);
                infoOutputCollection.Add("Floating leg Business day convention", FloatingBusinessDayConvention.Name.String);
                infoOutputCollection.Add("Floating leg Day count convention", FloatingDayCountConvention.Name.String);
                infoOutputCollection.Add("Floating leg Frequency", FloatingFrequency.Name.String);
            }
            #endregion

            /// <summary>Gets a mutable Swap Market convention template which is initially filled with values of 
            /// the current instance, i.e. with some standard conventions.
            /// </summary>
            /// <returns>A mutable (inflation-linked) Swap Market convention template.</returns>
            public InflationMarketConventions.InflationSwapConventions GetMutableTemplate()
            {
                InflationMarketConventions.InflationSwapConventions marketConventions = new InflationMarketConventions.InflationSwapConventions();
                marketConventions.SetStandardBusinessDaysToSettle(BusinessDaysToSettle);
                marketConventions.SetStandardFixedBusinessDayConvention(FixedBusinessDayConvention);
                marketConventions.SetStandardFixedDayCountConvention(FixedDayCountConvention);
                marketConventions.SetStandardFixedFrequency(FixedFrequency);
                marketConventions.SetStandardFloatingBusinessDayConvention(FloatingBusinessDayConvention);
                marketConventions.SetStandardFloatingDayCountConvention(FloatingDayCountConvention);
                marketConventions.SetStandardFloatingFrequency(FloatingFrequency);

                return marketConventions;
            }
            #endregion
        }

        /// <summary>Serves as read-only wrapper class for Market conventions for inflation-linked Bonds.
        /// </summary>
        public class InflationBondConventions : IInfoOutputQueriable
        {
            #region public (readonly) members

            /// <summary>The day count convention.
            /// </summary>
            public readonly IDayCountConvention DayCountConvention;

            /// <summary>The business day convention.
            /// </summary>
            public readonly IBusinessDayConvention BusinessDayConvention;

            /// <summary>The coupon frequency.
            /// </summary>
            public readonly IDateScheduleFrequency CouponFrequency;
            #endregion

            #region public constructors

            /// <summary>Initializes a new instance of the <see cref="InflationBondConventions"/> class.
            /// </summary>
            /// <param name="bondMarketConventions">The bond market conventions.</param>
            /// <exception cref="ArgumentNullException">Thrown, if <paramref name="bondMarketConventions"/> is <c>null</c>.</exception>
            /// <exception cref="ArgumentException">Thrown, if <paramref name="bondMarketConventions"/> is not completely defined.</exception>
            public InflationBondConventions(InflationMarketConventions.InflationBondConventions bondMarketConventions)
            {
                if (bondMarketConventions == null)
                {
                    throw new ArgumentNullException("bondMarketConventions");
                }
                if (bondMarketConventions.IsCompletelyDefined == false)
                {
                    throw new ArgumentException(String.Format(ExceptionMessages.ArgumentIsNotWellDefined, "Bond market conventions"), "bondMarketConventions");
                }
                BusinessDayConvention = bondMarketConventions.BusinessDayConvention;
                CouponFrequency = bondMarketConventions.CouponFrequency;
                DayCountConvention = bondMarketConventions.DayCountConvention;
            }
            #endregion

            #region public properties

            #region IInfoOutputQueriable Members

            /// <summary>Gets the info-output level of detail.
            /// </summary>
            /// <value>The info-output level of detail.</value>
            InfoOutputDetailLevel IInfoOutputQueriable.InfoOutputDetailLevel
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
            bool IInfoOutputQueriable.TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
            {
                return (infoOutputDetailLevel == InfoOutputDetailLevel.Full);
            }

            /// <summary>Gets informations of the current object as a specific <see cref="InfoOutput"/> instance.
            /// </summary>
            /// <param name="infoOutput">The <see cref="InfoOutput"/> object which is to be filled with informations concering the current instance.</param>
            /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category name.</param>
            void IInfoOutputQueriable.FillInfoOutput(InfoOutput infoOutput, string categoryName)
            {
                InfoOutputPackage infoOutputCollection = infoOutput.AcquirePackage(categoryName);
                infoOutputCollection.Add("Day count convention", DayCountConvention.Name.String);
                infoOutputCollection.Add("Business day convention", BusinessDayConvention.Name.String);
                infoOutputCollection.Add("Coupon frequency", CouponFrequency.Name.String);
            }
            #endregion

            /// <summary>Gets a mutable Bond Market convention template which is initially filled with values of 
            /// the current instance, i.e. with some standard conventions.
            /// </summary>
            /// <returns>A mutable Bond Market convention template.</returns>
            public InflationMarketConventions.InflationBondConventions GetMutableTemplate()
            {
                InflationMarketConventions.InflationBondConventions marketConventions = new InflationMarketConventions.InflationBondConventions();
                marketConventions.SetStandardDayCountconvention(DayCountConvention);
                marketConventions.SetStandardBusinessDayConvention(BusinessDayConvention);
                marketConventions.SetStandardCouponFrequency(CouponFrequency);

                return marketConventions;
            }
            #endregion
        }
        #endregion

        #region public readonly members

        /// <summary>The rounding rule used for the underlying inflation index.
        /// </summary>
        public readonly IRoundingRule RoundingRule;

        /// <summary>The Market conventions for inflation-linked swaps.
        /// </summary>
        public readonly ReadOnlyInflationMarketConventions.InflationSwapConventions SwapMarket;

        /// <summary>The Market conventions for inflation-linked Bonds.
        /// </summary>
        public readonly ReadOnlyInflationMarketConventions.InflationBondConventions BondMarket;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="ReadOnlyInflationMarketConventions"/> class.
        /// </summary>
        /// <param name="inflationMarketConventions">The inflation market conventions.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="inflationMarketConventions"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="inflationMarketConventions"/> is not completely defined.</exception>
        public ReadOnlyInflationMarketConventions(InflationMarketConventions inflationMarketConventions)
        {
            if (inflationMarketConventions == null)
            {
                throw new ArgumentNullException("inflationMarketConventions");
            }
            if (inflationMarketConventions.IsCompletelyDefined == false)
            {
                throw new ArgumentException(String.Format(ExceptionMessages.ArgumentIsNotWellDefined, "Inflation market conventions"), "inflationMarketConventions");
            }
            RoundingRule = inflationMarketConventions.RoundingRule;
            SwapMarket = inflationMarketConventions.SwapMarket.AsReadOnly();
            BondMarket = inflationMarketConventions.BondMarket.AsReadOnly();
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
            infoOutputCollection.Add("Rounding rule", RoundingRule.Name.String);
            (BondMarket as IInfoOutputQueriable).FillInfoOutput(infoOutput, categoryName + ".Bond market");
            (SwapMarket as IInfoOutputQueriable).FillInfoOutput(infoOutput, categoryName + ".Swap market");
        }
        #endregion

        /// <summary>Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return String.Format("Inflation market conventions (Rounding rule: {0}, Bond market: {1}, Swap market: {2})", RoundingRule, BondMarket, SwapMarket);
        }

        /// <summary>Gets a mutable Inflation Market convention template which is initially filled with values of 
        /// the current instance, i.e. with some standard conventions.
        /// </summary>
        /// <returns>A mutable Inflation Market convention template.</returns>
        public InflationMarketConventions GetMutableTemplate()
        {
            InflationMarketConventions marketConventions = new InflationMarketConventions();
            marketConventions.SetStandardRoundingRule(RoundingRule);
            marketConventions.BondMarket.SetStandardValues(BondMarket);
            marketConventions.SwapMarket.SetStandardValue(SwapMarket);

            return marketConventions;
        }
        #endregion
    }
}