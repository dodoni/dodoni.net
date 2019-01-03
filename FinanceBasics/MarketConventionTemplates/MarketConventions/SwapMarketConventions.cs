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

namespace Dodoni.Finance.MarketConventionTemplates
{
    /// <summary>Serves as wrapper class for Swap Market conventions.
    /// </summary>
    public class SwapMarketConventions
    {
        #region private members

        /// <summary>The number of business days to settle.
        /// </summary>
        private int? m_BusinessDaysToSettle = null;

        /// <summary>The state of the number of business days to settle.
        /// </summary>
        private ConventionState m_BusinessDaysToSettleState = ConventionState.Undefined;

        /// <summary>The fixing lag, i.e. the number of business days taken into account for the calculation of fixing dates etc., for example <c>-2</c>.
        /// </summary>
        private IFixingLag m_FixingLag = null;

        /// <summary>The state of the fixing lag.
        /// </summary>
        private ConventionState m_FixingLagState = ConventionState.Undefined;

        /// <summary>The frequency of the fixed rate leg.
        /// </summary>
        private IDateScheduleFrequency m_FixedFrequency;

        /// <summary>The state of the fixed rate leg frequency.
        /// </summary>
        private ConventionState m_FixedFrequencyState = ConventionState.Undefined;

        /// <summary>The frequency of the floating rate leg.
        /// </summary>
        private IDateScheduleFrequency m_FloatingFrequency;

        /// <summary>The state of the floating rate leg frequency.
        /// </summary>
        private ConventionState m_FloatingFrequencyState = ConventionState.Undefined;

        /// <summary>The day count convention of the fixed rate leg.
        /// </summary>
        private IDayCountConvention m_FixedDayCountConvention;

        /// <summary>The state of the day count convention of the fixed rate leg.
        /// </summary>
        private ConventionState m_FixedDayCountConventionState = ConventionState.Undefined;

        /// <summary>The day count convention of the floating rate leg.
        /// </summary>
        private IDayCountConvention m_FloatingDayCountConvention;

        /// <summary>The state of the day count convention of the floating rate leg.
        /// </summary>
        private ConventionState m_FloatingDayCountConventionState = ConventionState.Undefined;

        /// <summary>The business day convention of the fixed rate leg.
        /// </summary>
        private IBusinessDayConvention m_FixedBusinessDayConvention;

        /// <summary>The state of the business day convention of the fixed rate leg.
        /// </summary>
        private ConventionState m_FixedBusinessDayConventionState = ConventionState.Undefined;

        /// <summary>The business day convention of the floating rate leg.
        /// </summary>
        private IBusinessDayConvention m_FloatingBusinessDayConvention;

        /// <summary>The state of the business day convention of the floating rate leg.
        /// </summary>
        private ConventionState m_FloatingBusinessDayConventionState = ConventionState.Undefined;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="SwapMarketConventions"/> class.
        /// </summary>
        public SwapMarketConventions()
        {
        }
        #endregion

        #region public properties

        /// <summary>Gets a value indicating whether this instance is completely defined, i.e. all
        /// members and properties contain proper values.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is completely defined; otherwise, <c>false</c>.
        /// </value>
        public bool IsCompletelyDefined
        {
            get
            {
                return (m_FixedFrequency != null) && (m_FixedDayCountConvention != null) && (m_FixedBusinessDayConvention != null)
                 && (m_FloatingFrequency != null) && (m_FloatingDayCountConvention != null) && (m_FloatingBusinessDayConvention != null)
                 && (m_BusinessDaysToSettle != null) && (m_FixingLag != null);
            }
        }

        /// <summary>Gets or sets the number of business days to settle with respect to the Swap Market.
        /// </summary>
        /// <value>The number of business days to settle or <c>null</c> if the number of business days to settle is not specified.
        /// </value>
        public int? BusinessDaysToSettle
        {
            get { return m_BusinessDaysToSettle; }
            set
            {
                m_BusinessDaysToSettle = value;
                m_BusinessDaysToSettleState = (value != null) ? ConventionState.UserInput : ConventionState.Undefined;
            }
        }

        /// <summary>Gets the state of the business days to settle.
        /// </summary>
        /// <value>The state of the business days to settle.</value>
        public ConventionState BusinessDaysToSettleState
        {
            get { return m_BusinessDaysToSettleState; }
        }

        /// <summary>Gets or sets the fixing lag, i.e. the number of business days taken into account for the calculation of fixing dates etc., for example <c>-2</c>.
        /// </summary>
        /// <value>The fixing lag of <c>null</c> if the fixing lag is not specified.</value>
        public IFixingLag FixingLag
        {
            get { return m_FixingLag; }
            set
            {
                m_FixingLag = value;
                m_FixingLagState = (value != null) ? ConventionState.UserInput : ConventionState.Undefined;
            }
        }

        /// <summary>Gets the state of the fixing lag.
        /// </summary>
        /// <value>The state of the fixing lag.</value>
        public ConventionState FixingLagState
        {
            get { return m_FixingLagState; }
        }

        /// <summary>Gets or sets the frequency of the fixed rate leg.
        /// </summary>
        /// <value>The frequency of the fixed rate leg.</value>
        public IDateScheduleFrequency FixedFrequency
        {
            get { return m_FixedFrequency; }
            set
            {
                m_FixedFrequency = value;
                m_FixedFrequencyState = (value != null) ? ConventionState.UserInput : ConventionState.Undefined;
            }
        }

        /// <summary>Gets the state of the frequency of the fixed rate leg.
        /// </summary>
        /// <value>The state of the frequency of the fixed rate leg.</value>
        public ConventionState FixedFrequencyState
        {
            get { return m_FixedFrequencyState; }
        }

        /// <summary>Gets or sets the floating frequency, i.e. the frequency of the floating rate leg.
        /// </summary>
        /// <value>The frequency of the floating rate leg.</value>
        public IDateScheduleFrequency FloatingFrequency
        {
            get { return m_FloatingFrequency; }
            set
            {
                m_FloatingFrequency = value;
                m_FloatingFrequencyState = (value != null) ? ConventionState.UserInput : ConventionState.Undefined;
            }
        }

        /// <summary>Gets the state of the frequency of the floating rate leg.
        /// </summary>
        /// <value>The state of the frequency of the floating rate leg.</value>
        public ConventionState FloatingFrequencyState
        {
            get { return m_FloatingFrequencyState; }
        }

        /// <summary>Gets or sets the day count convention of the fixed rate leg.
        /// </summary>
        /// <value>The day count convention of the fixed rate leg.</value>
        public IDayCountConvention FixedDayCountConvention
        {
            get { return m_FixedDayCountConvention; }
            set
            {
                m_FixedDayCountConvention = value;
                m_FixedDayCountConventionState = (value != null) ? ConventionState.UserInput : ConventionState.Undefined;
            }
        }

        /// <summary>Gets the state of the day count convention of the fixed rate leg.
        /// </summary>
        /// <value>The state of the day count convention of the fixed rate leg.</value>
        public ConventionState FixedDayCountConventionState
        {
            get { return m_FixedDayCountConventionState; }
        }

        /// <summary>Gets or sets the day count convention of the floating rate leg.
        /// </summary>
        /// <value>The day count convention of the floating rate leg.</value>
        public IDayCountConvention FloatingDayCountConvention
        {
            get { return m_FloatingDayCountConvention; }
            set
            {
                m_FloatingDayCountConvention = value;
                m_FloatingDayCountConventionState = (value != null) ? ConventionState.UserInput : ConventionState.Undefined;
            }
        }

        /// <summary>Gets the state of the day count convention of the floating rate leg.
        /// </summary>
        /// <value>The state of the day count convention of the floating rate leg.</value>
        public ConventionState FloatingDayCountConventionState
        {
            get { return m_FloatingDayCountConventionState; }
        }

        /// <summary>Gets or sets the business day convention of the fixed rate leg.
        /// </summary>
        /// <value>The business day convention of the fixed rate leg.</value>
        public IBusinessDayConvention FixedBusinessDayConvention
        {
            get { return m_FixedBusinessDayConvention; }
            set
            {
                m_FixedBusinessDayConvention = value;
                m_FixedBusinessDayConventionState = (value != null) ? ConventionState.UserInput : ConventionState.Undefined;
            }
        }

        /// <summary>Gets the state of the business day convention of the fixed rate leg.
        /// </summary>
        /// <value>The state of the business day convention of the fixed rate leg.</value>
        public ConventionState FixedBusinessDayConventionState
        {
            get { return m_FixedBusinessDayConventionState; }
        }

        /// <summary>Gets or sets the business day convention of the floating rate leg.
        /// </summary>
        /// <value>The business day convention of the floating rate leg.</value>
        public IBusinessDayConvention FloatingBusinessDayConvention
        {
            get { return m_FloatingBusinessDayConvention; }
            set
            {
                m_FloatingBusinessDayConvention = value;
                m_FloatingBusinessDayConventionState = (value != null) ? ConventionState.UserInput : ConventionState.Undefined;
            }
        }

        /// <summary>Gets the state of the business day convention of the floating rate leg.
        /// </summary>
        /// <value>The state of the business day convention of the floating rate leg.</value>
        public ConventionState FloatingBusinessDayConventionState
        {
            get { return m_FloatingBusinessDayConventionState; }
        }
        #endregion

        #region public methods

        /// <summary>Returns a read-only wrapper for the current Swap Market conventions.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Thrown, if <see cref="IsCompletelyDefined"/> is <c>false</c>.</exception>
        public ReadOnlySwapMarketConventions AsReadOnly()
        {
            if (IsCompletelyDefined == false)
            {
                throw new InvalidOperationException(String.Format(ExceptionMessages.ObjectIsInvalid, "Swap market conventions", "asReadOnly"));
            }
            return new ReadOnlySwapMarketConventions(this);
        }
        #endregion

        #region internal methods

        /// <summary>Sets the number of business days to settle and sets the <see cref="BusinessDaysToSettleState"/> to <see cref="ConventionState.StandardValue"/>.
        /// </summary>
        /// <param name="businessDaysToSettle">The number of business days to settle.</param>
        internal void SetStandardBusinessDaysToSettle(int businessDaysToSettle)
        {
            m_BusinessDaysToSettle = businessDaysToSettle;
            m_BusinessDaysToSettleState = ConventionState.StandardValue;
        }

        /// <summary>Sets the fixing lag, i.e. the number of business days taken into account for calculation of fixing dates, for example <c>-2</c> and sets the <see cref="FixingLagState"/> to <see cref="ConventionState.StandardValue"/>.
        /// </summary>
        /// <param name="value">The number of business days for fixing date calculation.</param>
        internal void SetStandardFixingLag(IFixingLag value)
        {
            m_FixingLag = value;
            m_FixingLagState = ConventionState.StandardValue;
        }

        /// <summary>Sets the business day convention of the fixed rate leg and sets the <see cref="FixedBusinessDayConventionState"/> to <see cref="ConventionState.StandardValue"/>.
        /// </summary>
        /// <param name="fixedBusinessDayConvention">The business day convention of the fixed rate leg.</param>
        internal void SetStandardFixedBusinessDayConvention(IBusinessDayConvention fixedBusinessDayConvention)
        {
            if (fixedBusinessDayConvention == null)
            {
                throw new ArgumentNullException("fixedBusinesDayConvention");
            }
            m_FixedBusinessDayConvention = fixedBusinessDayConvention;
            m_FixedBusinessDayConventionState = ConventionState.StandardValue;
        }

        /// <summary>Sets the day count convention of the fixed rate leg and sets the <see cref="FixedDayCountConventionState"/> to <see cref="ConventionState.StandardValue"/>.
        /// </summary>
        /// <param name="fixedDayCountConvention">The day count convention of the fixed rate leg.</param>
        internal void SetStandardFixedDayCountConvention(IDayCountConvention fixedDayCountConvention)
        {
            if (fixedDayCountConvention == null)
            {
                throw new ArgumentNullException("fixedDayCountConvention");
            }
            m_FixedDayCountConvention = fixedDayCountConvention;
            m_FixedDayCountConventionState = ConventionState.StandardValue;
        }

        /// <summary>Sets the frequency of the fixed rate leg and sets the <see cref="FixedFrequencyState"/> to <see cref="ConventionState.StandardValue"/>.
        /// </summary>
        /// <param name="fixedFrequency">The frequency of the fixed rate leg.</param>
        internal void SetStandardFixedFrequency(IDateScheduleFrequency fixedFrequency)
        {
            if (fixedFrequency == null)
            {
                throw new ArgumentNullException("fixedFrequency");
            }
            m_FixedFrequency = fixedFrequency;
            m_FixedFrequencyState = ConventionState.StandardValue;
        }

        /// <summary>Sets the business day convention of the floating rate leg and sets the <see cref="FloatingBusinessDayConventionState"/> to <see cref="ConventionState.StandardValue"/>.
        /// </summary>
        /// <param name="floatingBusinessDayConvention">The business day convention of the floating rate leg.</param>
        internal void SetStandardFloatingBusinessDayConvention(IBusinessDayConvention floatingBusinessDayConvention)
        {
            if (floatingBusinessDayConvention == null)
            {
                throw new ArgumentNullException("floatingBusinessDayConvention");
            }
            m_FloatingBusinessDayConvention = floatingBusinessDayConvention;
            m_FloatingBusinessDayConventionState = ConventionState.StandardValue;
        }

        /// <summary>Sets the day count convention of the floating rate leg and sets the <see cref="FloatingDayCountConventionState"/> to <see cref="ConventionState.StandardValue"/>.
        /// </summary>
        /// <param name="floatingDayCountConvention">The day count convention of the floating rate leg.</param>
        internal void SetStandardFloatingDayCountConvention(IDayCountConvention floatingDayCountConvention)
        {
            if (floatingDayCountConvention == null)
            {
                throw new ArgumentNullException("floatingDayCountConvention");
            }
            m_FloatingDayCountConvention = floatingDayCountConvention;
            m_FloatingDayCountConventionState = ConventionState.StandardValue;
        }

        /// <summary>Sets the frequency of the floating rate leg and sets the <see cref="FloatingFrequencyState"/> to <see cref="ConventionState.StandardValue"/>.
        /// </summary>
        /// <param name="floatingFrequency">The frequency of the floating rate leg.</param>
        internal void SetStandardFloatingFrequency(IDateScheduleFrequency floatingFrequency)
        {
            if (floatingFrequency == null)
            {
                throw new ArgumentNullException("floatingFrequency");
            }
            m_FloatingFrequency = floatingFrequency;
            m_FloatingFrequencyState = ConventionState.StandardValue;
        }
        #endregion
    }
}