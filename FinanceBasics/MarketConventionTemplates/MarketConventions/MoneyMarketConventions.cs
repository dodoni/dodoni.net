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

namespace Dodoni.Finance.MarketConventionTemplates
{
    /// <summary>Serves as mutable wrapper class for Money market conventions.
    /// </summary>
    public class MoneyMarketConventions
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

        /// <summary>The caplet tenor convention needed for some caplet stripping.
        /// </summary>
        private IIRCapletTenorConvention m_CapletTenorConvention;

        /// <summary>The state of the caplet tenor convention.
        /// </summary>
        private ConventionState m_CapletTenorConventionState = ConventionState.Undefined;

        /// <summary>The standard Libor index name.
        /// </summary>
        private IdentifierString m_LiborIndexName;

        /// <summary>The state of the standard Libor index name.
        /// </summary>
        private ConventionState m_LiborIndexNameState = ConventionState.Undefined;

        /// <summary>The rounding rule for Libor rates.
        /// </summary>
        private IRoundingRule m_LiborRateRoundingRule;

        /// <summary>The state of the rounding rule taken into account for the Libor rate.
        /// </summary>
        private ConventionState m_LiborRateRoundingRuleState = ConventionState.Undefined;

        /// <summary>The future base point value.
        /// </summary>
        private double m_FutureBasePointValue = Double.NaN;

        /// <summary>The state of the future base point value.
        /// </summary>
        private ConventionState m_FutureBasePointValueState = ConventionState.Undefined;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="MoneyMarketConventions"/> class.
        /// </summary>
        public MoneyMarketConventions()
        {
        }
        #endregion

        #region public properties

        /// <summary>Gets a value indicating whether this instance is completely defined, i.e. all members and properties contain proper values.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is completely defined; otherwise, <c>false</c>.
        /// </value>
        public bool IsCompletelyDefined
        {
            get { return (m_DayCountConvention != null) && (m_BusinessDayConvention != null) && (m_CapletTenorConvention != null) && (m_LiborRateRoundingRule != null) && (m_BusinessDaysToSettle != null) && (Double.IsNaN(m_FutureBasePointValue) == false) && (m_LiborIndexName != null) && (m_FixingLag != null); }
        }

        /// <summary>Gets or sets the number of business days to settle with respect to the Money market.
        /// </summary>
        /// <value>
        /// The number of business days to settle or <c>null</c> if the current instance
        /// does not contain the number of business days to settle.
        /// </value>
        /// <remarks>The number of business days to settle (with respect to money market conventions) will be
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

        /// <summary>Gets or sets the fixing lag, i.e. the number of business days taken into account for the calculation of fixing dates, for example <c>-2</c>.
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

        /// <summary>Gets or sets the day count convention of the Money Market.
        /// </summary>
        /// <value>The day count convention of the Money market.</value>
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

        /// <summary>Gets or sets the business day convention of the Money Market.
        /// </summary>
        /// <value>The business day convention of the Money market.</value>
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

        /// <summary>Gets or sets the caplet tenor convention.
        /// </summary>
        /// <value>The caplet tenor convention.</value>
        public IIRCapletTenorConvention CapletTenorConvention
        {
            get { return m_CapletTenorConvention; }
            set
            {
                m_CapletTenorConvention = value;
                m_CapletTenorConventionState = (value != null) ? ConventionState.UserInput : ConventionState.Undefined;
            }
        }

        /// <summary>Gets the state of the caplet tenor convention.
        /// </summary>
        /// <value>The state of the caplet tenor convention.</value>
        public ConventionState CapletTenorConventionState
        {
            get { return m_CapletTenorConventionState; }
        }

        /// <summary>Gets or sets the standard name of the Libor index.
        /// </summary>
        /// <value>The standard name of the Libor index.</value>
        public IdentifierString LiborIndexName
        {
            get { return m_LiborIndexName; }
            set
            {
                m_LiborIndexName = value;
                m_LiborIndexNameState = (value != null) ? ConventionState.UserInput : ConventionState.Undefined;
            }
        }

        /// <summary>Gets or sets the rounding rule for the Libor rate.
        /// </summary>
        /// <value>The rounding rule for the Libor rate.</value>
        public IRoundingRule LiborRateRoundingRule
        {
            get { return m_LiborRateRoundingRule; }
            set
            {
                m_LiborRateRoundingRule = value;
                m_LiborRateRoundingRuleState = (value != null) ? ConventionState.UserInput : ConventionState.Undefined;
            }
        }

        /// <summary>Gets the state of the libor rate rounding rule.
        /// </summary>
        /// <value>The state of the libor rate rounding rule.</value>
        public ConventionState LiborRateRoundingRuleState
        {
            get { return m_LiborRateRoundingRuleState; }
        }

        /// <summary>Gets the base point value of a future, i.e. the price of a future is equal
        /// to the quote times the 'future base point value' / 1 basis point.
        /// </summary>
        /// <value>
        /// The future base point value or <see cref="System.Double.NaN"/> if the current
        /// instance does not contain the future base point value.
        /// </value>
        public double FutureBasePointValue
        {
            get { return m_FutureBasePointValue; }
            set
            {
                m_FutureBasePointValue = value;
                m_FutureBasePointValueState = (Double.IsNaN(value) == false) ? ConventionState.UserInput : ConventionState.Undefined;
            }
        }

        /// <summary>Gets the state of the future base point value.
        /// </summary>
        /// <value>The state of the future base point value.</value>
        public ConventionState FutureBasePointValueState
        {
            get { return m_FutureBasePointValueState; }
        }
        #endregion

        #region public methods

        /// <summary>Returns a read-only wrapper for the current Money Market conventions.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Thrown, if <see cref="IsCompletelyDefined"/> is <c>false</c>.</exception>
        public ReadOnlyMoneyMarketConventions AsReadOnly()
        {
            if (IsCompletelyDefined == false)
            {
                throw new InvalidOperationException(String.Format(ExceptionMessages.ObjectIsInvalid, "Money market conventions", "asReadOnly"));
            }
            return new ReadOnlyMoneyMarketConventions(this);
        }
        #endregion

        #region internal methods

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

        /// <summary>Sets the business days to settle and sets the <see cref="BusinessDaysToSettleState"/> to <see cref="ConventionState.StandardValue"/>.
        /// </summary>
        /// <param name="businessDaysToSettle">The number of business days to settle.</param>
        internal void SetStandardBusinessDaysToSettle(int businessDaysToSettle)
        {
            m_BusinessDaysToSettle = businessDaysToSettle;
            m_BusinessDaysToSettleState = ConventionState.StandardValue;
        }

        /// <summary>Sets the fixing lag, i.e. the number of business days taken into account for calculation of fixing dates etc., for example <c>-2</c> and sets the <see cref="FixingLagState"/> to <see cref="ConventionState.StandardValue"/>.
        /// </summary>
        /// <param name="value">The number of business days for fixing date calculation.</param>
        internal void SetStandardFixingLag(IFixingLag value)
        {
            m_FixingLag = value;
            m_FixingLagState = ConventionState.StandardValue;
        }

        /// <summary>Sets the standard caplet tenor convention and sets the <see cref="CapletTenorConventionState"/> to <see cref="ConventionState.StandardValue"/>.
        /// </summary>
        /// <param name="capletTenorConvention">The caplet tenor convention.</param>
        internal void SetStandardCapletTenorConvention(IIRCapletTenorConvention capletTenorConvention)
        {
            if (capletTenorConvention == null)
            {
                throw new ArgumentNullException("capletTenorConvention");
            }
            m_CapletTenorConvention = capletTenorConvention;
            m_CapletTenorConventionState = ConventionState.StandardValue;
        }

        /// <summary>Sets the standard name of the Libor index.
        /// </summary>
        /// <param name="liborIndexName">The standard name of the Libor index.</param>
        internal void SetStandardLiborIndexName(IdentifierString liborIndexName)
        {
            if (liborIndexName == null)
            {
                throw new ArgumentNullException("liborIndexName");
            }
            m_LiborIndexName = liborIndexName;
            m_LiborIndexNameState = ConventionState.StandardValue;
        }

        /// <summary>Sets the rounding rule for Libor rates and sets the <see cref="LiborRateRoundingRuleState"/> to <see cref="ConventionState.StandardValue"/>.
        /// </summary>
        /// <param name="liborRateRoundingRule">The rounding rule for Libor rates.</param>
        internal void SetStandardLiborRateRoundingRule(IRoundingRule liborRateRoundingRule)
        {
            if (liborRateRoundingRule == null)
            {
                throw new ArgumentNullException("liborRateRoundingRule");
            }
            m_LiborRateRoundingRule = liborRateRoundingRule;
            m_LiborRateRoundingRuleState = ConventionState.StandardValue;
        }

        /// <summary>Sets the future base point value and sets the <see cref="FutureBasePointValueState"/> to <see cref="ConventionState.StandardValue"/>.
        /// </summary>
        /// <param name="futureBasePointValue">The future base point value.</param>
        internal void SetStandardFutureBasePointValue(double futureBasePointValue)
        {
            if (Double.IsNaN(futureBasePointValue))
            {
                throw new ArgumentException("futureBasePointValue");
            }
            m_FutureBasePointValue = futureBasePointValue;
            m_FutureBasePointValueState = ConventionState.StandardValue;
        }
        #endregion
    }
}