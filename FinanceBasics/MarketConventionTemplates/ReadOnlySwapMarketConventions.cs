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
using Dodoni.Finance.DateFactory;
using Dodoni.BasicComponents.Containers;

namespace Dodoni.Finance.MarketConventionTemplates
{
    /// <summary>Serves as read-only wrapper class for (Interest rate) Swap Market conventions.
    /// </summary>
    public class ReadOnlySwapMarketConventions : IInfoOutputQueriable
    {
        #region public (readonly) members

        /// <summary>The number of business days to settle.
        /// </summary>
        public readonly int BusinessDaysToSettle;

        /// <summary>The fixing lag, i.e. the number of business days taken into account for the calculation of fixing dates etc., for example <c>-2</c>.
        /// </summary>
        public readonly IFixingLag FixingLag;

        /// <summary>The frequency of the fixed rate leg of interest rate linked swaps.
        /// </summary>
        public readonly IDateScheduleFrequency FixedFrequency;

        /// <summary>The frequency of the floating rate leg of interest rate linked swaps.
        /// </summary>
        public readonly IDateScheduleFrequency FloatingFrequency;

        /// <summary>The day count convention of the fixed rate leg of interest rate linked swaps.
        /// </summary>
        public readonly IDayCountConvention FixedDayCountConvention;

        /// <summary>The day count convention of the floating rate leg of interest rate linked swaps.
        /// </summary>
        public readonly IDayCountConvention FloatingDayCountConvention;

        /// <summary>The business day convention of the fixed rate leg of interest rate linked swaps.
        /// </summary>
        public readonly IBusinessDayConvention FixedBusinessDayConvention;

        /// <summary>The business day convention of the floating rate leg of interest rate linked swaps.
        /// </summary>
        public readonly IBusinessDayConvention FloatingBusinessDayConvention;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="ReadOnlySwapMarketConventions"/> class.
        /// </summary>
        /// <param name="swapMarketConventions">The swap market conventions.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="swapMarketConventions"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="swapMarketConventions"/> is not completely defined.</exception>               
        public ReadOnlySwapMarketConventions(SwapMarketConventions swapMarketConventions)
        {
            if (swapMarketConventions == null)
            {
                throw new ArgumentNullException("swapMarketConventions");
            }
            if (swapMarketConventions.IsCompletelyDefined == false)
            {
                throw new ArgumentException(String.Format(ExceptionMessages.ArgumentIsNotWellDefined, "Swap market conventions"), "swapMarketConventions");
            }
            BusinessDaysToSettle = swapMarketConventions.BusinessDaysToSettle.Value;
            FixingLag = swapMarketConventions.FixingLag;
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
            infoOutputCollection.Add("Fixed leg Business day convention", FixedBusinessDayConvention.Name.String);
            infoOutputCollection.Add("Fixed leg Day count convention", FixedDayCountConvention.Name.String);
            infoOutputCollection.Add("Fixed leg Frequency", FixedFrequency.Name.String);
            infoOutputCollection.Add("Floating leg Business day convention", FloatingBusinessDayConvention.Name.String);
            infoOutputCollection.Add("Floating leg Day count convention", FloatingDayCountConvention.Name.String);
            infoOutputCollection.Add("Floating leg Frequency", FloatingFrequency.Name.String);
        }
        #endregion

        /// <summary>Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return String.Format("Swap market conventions (Fixed: {0}, {1}, Frequency: {2}; Floating: {3}, {4}, Frequency: {5}; Business days to settle: {6}; Fixing Lag: {7})", FixedDayCountConvention, FixedBusinessDayConvention, FixedFrequency, FloatingDayCountConvention, FloatingBusinessDayConvention, FloatingFrequency, BusinessDaysToSettle, FixingLag);
        }

        /// <summary>Gets a mutable Swap Market convention template which is initially filled with values of the current instance, i.e. with some standard conventions.
        /// </summary>
        /// <returns>A mutable Swap Market convention template.</returns>
        public SwapMarketConventions GetMutableTemplate()
        {
            SwapMarketConventions marketConventions = new SwapMarketConventions();
            marketConventions.SetStandardBusinessDaysToSettle(BusinessDaysToSettle);
            marketConventions.SetStandardFixingLag(FixingLag);
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
}