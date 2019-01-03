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
    /// <summary>Serves as mutable wrapper class for Bond market conventions.
    /// </summary>
    public class BondMarketConventions
    {
        #region private members

        /// <summary>The number of business days to settle.
        /// </summary>
        private int? m_BusinessDaysToSettle = null;

        /// <summary>The state of the number of business days to settle.
        /// </summary>
        private ConventionState m_BusinessDaysToSettleState = ConventionState.Undefined;

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

        /// <summary>Initializes a new instance of the <see cref="BondMarketConventions"/> class.
        /// </summary>
        public BondMarketConventions()
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
            get { return (m_DayCountConvention != null) && (m_BusinessDayConvention != null) && (m_CouponFrequency != null) && (m_BusinessDaysToSettle != null); }
        }

        /// <summary>Gets or sets the number of business days to settle with respect to the Bond market.
        /// </summary>
        /// <value>
        /// The number of business days to settle or <c>null</c> if the current instance
        /// does not contain the number of business days to settle.
        /// </value>
        /// <remarks>The number of business days to settle (with respect to bond market conventions) will be
        /// used for example in the bootstrap approach of interest rate curves.</remarks>
        public int? BusinessDaysToSettle
        {
            get { return m_BusinessDaysToSettle; }
            set
            {
                m_BusinessDaysToSettle = value;
                m_BusinessDaysToSettleState = (value != null) ? ConventionState.UserInput : ConventionState.Undefined;
            }
        }

        /// <summary>Gets the state of the number of business days to settle.
        /// </summary>
        /// <value>The state of the number of business days to settle.</value>
        public ConventionState BusinessDaysToSettleState
        {
            get { return m_BusinessDaysToSettleState; }
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
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Thrown, if <see cref="IsCompletelyDefined"/> is <c>false</c>.</exception>
        public ReadOnlyBondMarketConventions AsReadOnly()
        {
            if (IsCompletelyDefined == false)
            {
                throw new InvalidOperationException(String.Format(ExceptionMessages.ObjectIsInvalid, "Bond market conventions", "asReadOnly"));
            }
            return new ReadOnlyBondMarketConventions(this);
        }
        #endregion

        #region internal methods

        /// <summary>Sets the business days to settle and sets the <see cref="BusinessDaysToSettleState"/> to <see cref="ConventionState.StandardValue"/>.
        /// </summary>
        /// <param name="businessDaysToSettle">The number of business days to settle.</param>
        internal void SetStandardBusinessDaysToSettle(int businessDaysToSettle)
        {
            m_BusinessDaysToSettle = businessDaysToSettle;
            m_BusinessDaysToSettleState = ConventionState.StandardValue;
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
}