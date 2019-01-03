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
using Dodoni.MathLibrary;
using Dodoni.Finance.DateFactory;

namespace Dodoni.Finance.MarketConventionTemplates
{
    /// <summary>Serves as mutable wrapper class for Inflation Market conventions.
    /// </summary>
    public class InflationMarketConventions
    {
        #region nested classes

        /// <summary>Serves as mutable wrapper class for inflation-linked Swap market conventions.
        /// </summary>
        public class InflationSwapConventions
        {
            #region private members

            /// <summary>The number of business days to settle.
            /// </summary>
            private int? m_BusinessDaysToSettle = null;

            /// <summary>The state of the number of business days to settle.
            /// </summary>
            private ConventionState m_BusinessDaysToSettleState = ConventionState.Undefined;

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

            /// <summary>Initializes a new instance of the <see cref="InflationSwapConventions"/> class.
            /// </summary>
            public InflationSwapConventions()
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
                     && (m_BusinessDaysToSettle != null);
                }
            }

            /// <summary>Gets or sets the number of business days to settle with respect to the Swap Market.
            /// </summary>
            /// <value>The number of business days to settle or <c>null</c> if the number of business days to settle is not given.
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
            /// <returns>A read-only wrapper for the current Swap Market conventions.</returns>
            /// <exception cref="InvalidOperationException">Thrown, if <see cref="IsCompletelyDefined"/> is <c>false</c>.</exception>
            public ReadOnlyInflationMarketConventions.InflationSwapConventions AsReadOnly()
            {
                if (IsCompletelyDefined == false)
                {
                    throw new InvalidOperationException(String.Format(ExceptionMessages.ObjectIsInvalid, "Swap market conventions", "asReadOnly"));
                }
                return new ReadOnlyInflationMarketConventions.InflationSwapConventions(this);
            }
            #endregion

            #region internal methods

            /// <summary>Sets the standard conventions of a specific <see cref="ReadOnlyInflationMarketConventions.InflationSwapConventions"/> object.
            /// </summary>
            /// <param name="swapMarketConventions">The Swap Market conventions.</param>
            internal void SetStandardValue(ReadOnlyInflationMarketConventions.InflationSwapConventions swapMarketConventions)
            {
                if (swapMarketConventions == null)
                {
                    throw new ArgumentNullException("swapMarketConventions");
                }
                SetStandardBusinessDaysToSettle(swapMarketConventions.BusinessDaysToSettle);
                SetStandardFixedBusinessDayConvention(swapMarketConventions.FixedBusinessDayConvention);
                SetStandardFixedDayCountConvention(swapMarketConventions.FixedDayCountConvention);
                SetStandardFixedFrequency(swapMarketConventions.FixedFrequency);
                SetStandardFloatingBusinessDayConvention(swapMarketConventions.FloatingBusinessDayConvention);
                SetStandardFloatingDayCountConvention(swapMarketConventions.FloatingDayCountConvention);
                SetStandardFloatingFrequency(swapMarketConventions.FloatingFrequency);
            }

            /// <summary>Sets the number of business days to settle and sets the <see cref="BusinessDaysToSettleState"/> to <see cref="ConventionState.StandardValue"/>.
            /// </summary>
            /// <param name="businessDaysToSettle">The number of business days to settle.</param>
            internal void SetStandardBusinessDaysToSettle(int businessDaysToSettle)
            {
                m_BusinessDaysToSettle = businessDaysToSettle;
                m_BusinessDaysToSettleState = ConventionState.StandardValue;
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

        /// <summary>Serves as mutable wrapper class for inflation-linked Bond market conventions.
        /// </summary>
        public class InflationBondConventions
        {
            #region private members

            /// <summary>The day count convention.
            /// </summary>
            private IDayCountConvention m_DayCountConvention;

            /// <summary>The state of the day count convention.
            /// </summary>
            private ConventionState m_DayCountConventionState = ConventionState.Undefined;

            /// <summary>The business day convention.
            /// </summary>
            private IBusinessDayConvention m_BusinessDayConvention;

            /// <summary>The state of the business day convention.
            /// </summary>
            private ConventionState m_BusinessDayConventionState = ConventionState.Undefined;

            /// <summary>The coupon frequency.
            /// </summary>
            private IDateScheduleFrequency m_CouponFrequency;

            /// <summary>The state of the coupon frequency.
            /// </summary>
            private ConventionState m_CouponFrequencyState = ConventionState.Undefined;
            #endregion

            #region public constructors

            /// <summary>Initializes a new instance of the <see cref="InflationBondConventions"/> class.
            /// </summary>
            public InflationBondConventions()
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
                get { return (m_DayCountConvention != null) && (m_BusinessDayConvention != null) && (m_CouponFrequency != null); }
            }

            /// <summary>Gets or sets the Bond market day count convention.
            /// </summary>
            /// <value>The day count convention of the Bond market.</value>
            public IDayCountConvention DayCountConvention
            {
                get { return m_DayCountConvention; }
                set
                {
                    m_DayCountConvention = value;
                    m_DayCountConventionState = (value != null) ? ConventionState.UserInput : ConventionState.Undefined;
                }
            }

            /// <summary>Gets the state of the day count convention.
            /// </summary>
            /// <value>The state of the day count convention.</value>
            public ConventionState DayCountConventionState
            {
                get { return m_DayCountConventionState; }
            }

            /// <summary>Gets or sets the Bond market business day convention.
            /// </summary>
            /// <value>The business day convention of the Bond market.</value>
            public IBusinessDayConvention BusinessDayConvention
            {
                get { return m_BusinessDayConvention; }
                set
                {
                    m_BusinessDayConvention = value;
                    m_BusinessDayConventionState = (value != null) ? ConventionState.UserInput : ConventionState.Undefined;
                }
            }

            /// <summary>Gets the state of the business day convention.
            /// </summary>
            /// <value>The state of the business day convention.</value>
            public ConventionState BusinessDayConventionState
            {
                get { return m_BusinessDayConventionState; }
            }

            /// <summary>Gets or sets the Bond coupon frequency.
            /// </summary>
            /// <value>The coupon frequency.</value>
            public IDateScheduleFrequency CouponFrequency
            {
                get { return m_CouponFrequency; }
                set
                {
                    m_CouponFrequency = value;
                    m_CouponFrequencyState = (value != null) ? ConventionState.UserInput : ConventionState.Undefined;
                }
            }

            /// <summary>Gets the state of the coupon frequency.
            /// </summary>
            /// <value>The state of the coupon frequency.</value>
            public ConventionState CouponFrequencyState
            {
                get { return m_CouponFrequencyState; }
            }
            #endregion

            #region public methods

            /// <summary>Returns a read-only wrapper for the current Bond Market conventions.
            /// </summary>
            /// <returns>A read-only wrapper for the current Bond Market conventions.</returns>
            /// <exception cref="InvalidOperationException">Thrown, if <see cref="IsCompletelyDefined"/> is <c>false</c>.</exception>
            public ReadOnlyInflationMarketConventions.InflationBondConventions AsReadOnly()
            {
                if (IsCompletelyDefined == false)
                {
                    throw new InvalidOperationException(String.Format(ExceptionMessages.ObjectIsInvalid, "Bond market conventions", "asReadOnly"));
                }
                return new ReadOnlyInflationMarketConventions.InflationBondConventions(this);
            }
            #endregion

            #region internal methods

            /// <summary>Sets the standard conventions of a specific <see cref="ReadOnlyInflationMarketConventions.InflationBondConventions"/> object.
            /// </summary>
            /// <param name="bondMarketConventions">The bond market conventions.</param>
            internal void SetStandardValues(ReadOnlyInflationMarketConventions.InflationBondConventions bondMarketConventions)
            {
                if (bondMarketConventions == null)
                {
                    throw new ArgumentNullException("bondMarketConventions");
                }
                SetStandardDayCountconvention(bondMarketConventions.DayCountConvention);
                SetStandardBusinessDayConvention(bondMarketConventions.BusinessDayConvention);
                SetStandardCouponFrequency(bondMarketConventions.CouponFrequency);
            }

            /// <summary>Sets the day count convention and sets the <see cref="DayCountConventionState"/> to <see cref="ConventionState.StandardValue"/>.
            /// </summary>
            /// <param name="dayCountConvention">The day count convention.</param>
            internal void SetStandardDayCountconvention(IDayCountConvention dayCountConvention)
            {
                if (dayCountConvention == null)
                {
                    throw new ArgumentNullException("dayCountConvention");
                }
                m_DayCountConvention = dayCountConvention;
                m_DayCountConventionState = ConventionState.StandardValue;
            }

            /// <summary>Sets the business day convention and sets the <see cref="BusinessDayConventionState"/> to <see cref="ConventionState.StandardValue"/>.
            /// </summary>
            /// <param name="businessDayConvention">The business day convention.</param>
            internal void SetStandardBusinessDayConvention(IBusinessDayConvention businessDayConvention)
            {
                if (businessDayConvention == null)
                {
                    throw new ArgumentNullException("businessDayConvention");
                }
                m_BusinessDayConvention = businessDayConvention;
                m_BusinessDayConventionState = ConventionState.StandardValue;
            }

            /// <summary>Sets the coupon frequency and sets the <see cref="CouponFrequencyState"/> to <see cref="ConventionState.StandardValue"/>.
            /// </summary>
            /// <param name="couponFrequency">The Bond coupon frequency.</param>
            internal void SetStandardCouponFrequency(IDateScheduleFrequency couponFrequency)
            {
                if (couponFrequency == null)
                {
                    throw new ArgumentNullException("couponFrequency");
                }
                m_CouponFrequency = couponFrequency;
                m_CouponFrequencyState = ConventionState.StandardValue;
            }
            #endregion
        }
        #endregion

        #region private members

        /// <summary>The rounding rule used for the underlying inflation index.
        /// </summary>
        private IRoundingRule m_RoundingRule;

        /// <summary>The state of the rounding rule.
        /// </summary>
        private ConventionState m_RoundingRuleState = ConventionState.Undefined;

        /// <summary>The Market conventions for inflation-linked Swaps.
        /// </summary>
        private InflationSwapConventions m_SwapMarketConventions;

        /// <summary>The Market conventions for inflation-linked Bonds.
        /// </summary>
        private InflationBondConventions m_BondMarketConventions;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="InflationMarketConventions"/> class.
        /// </summary>
        public InflationMarketConventions()
        {
            m_BondMarketConventions = new InflationBondConventions();
            m_SwapMarketConventions = new InflationSwapConventions();
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
            get { return m_BondMarketConventions.IsCompletelyDefined && m_SwapMarketConventions.IsCompletelyDefined && (m_RoundingRule != null); }
        }

        /// <summary>Gets or sets the rounding rule of the inflation index.
        /// </summary>
        /// <value>The rounding rule of the inflation index.</value>
        public IRoundingRule RoundingRule
        {
            get { return m_RoundingRule; }
            set
            {
                m_RoundingRule = value;
                m_RoundingRuleState = (value != null) ? ConventionState.UserInput : ConventionState.Undefined;
            }
        }

        /// <summary>Gets the state of the rounding rule.
        /// </summary>
        /// <value>The state of the rounding rule.</value>
        public ConventionState RoundingRuleState
        {
            get { return m_RoundingRuleState; }
        }

        /// <summary>Gets the Market conventions for inflation-linked swaps.
        /// </summary>
        /// <value>The swap market conventions.</value>
        public InflationSwapConventions SwapMarket
        {
            get { return m_SwapMarketConventions; }
        }

        /// <summary>Gets the Market conventions for inflation-linked Bonds.
        /// </summary>
        /// <value>The bond market conventions.</value>
        public InflationBondConventions BondMarket
        {
            get { return m_BondMarketConventions; }
        }
        #endregion

        #region public methods

        /// <summary>Returns a read-only wrapper for the current Inflation Market conventions.
        /// </summary>
        /// <returns>A read-only wrapper of the current instance.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if <see cref="IsCompletelyDefined"/> is <c>false</c>.</exception>
        public ReadOnlyInflationMarketConventions AsReadOnly()
        {
            if (IsCompletelyDefined == false)
            {
                throw new InvalidOperationException(String.Format(ExceptionMessages.ObjectIsInvalid, "Inflationmarket conventions", "asReadOnly"));
            }
            return new ReadOnlyInflationMarketConventions(this);
        }
        #endregion

        #region internal methods

        /// <summary>Sets the rounding rule and sets the <see cref="RoundingRuleState"/> to <see cref="ConventionState.StandardValue"/>.
        /// </summary>
        /// <param name="roundingRule">The rounding rule.</param>
        /// <returns>A value indicating whether <see cref="RoundingRule"/> and <see cref="RoundingRuleState"/> has been changed.</returns>
        internal void SetStandardRoundingRule(IRoundingRule roundingRule)
        {
            if (roundingRule == null)
            {
                throw new ArgumentNullException("roundingRule");
            }
            m_RoundingRule = roundingRule;
            m_RoundingRuleState = ConventionState.StandardValue;
        }
        #endregion
    }
}