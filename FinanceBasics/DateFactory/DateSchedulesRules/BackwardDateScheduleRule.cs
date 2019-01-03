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

using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Containers;

namespace Dodoni.Finance.DateFactory
{
    /// <summary>Represents a date schedule rule where the dates are generated via some backward rolling approach.
    /// </summary>
    public class BackwardDateScheduleRule : IDateScheduleRule
    {
        #region nested classes

        /// <summary>Serves as <see cref="IDateScheduleRuleDescription"/> wrapper of <see cref="BackwardDateScheduleRule"/> objects.
        /// </summary>
        private class DateScheduleRuleDescription : IDateScheduleRuleDescription
        {
            #region private members

            /// <summary>The date schedule rule.
            /// </summary>
            private BackwardDateScheduleRule m_DateScheduleRule;
            #endregion

            #region public constructors

            /// <summary>Initializes a new instance of the <see cref="DateScheduleRuleDescription "/> class.
            /// </summary>
            /// <param name="startAndEndDateDescription">The description of the start and of the end date of the date schedule.</param>
            /// <param name="frequency">The frequency.</param>
            /// <param name="businessDayConvention">The business day convention.</param>
            /// <param name="seedBusinessDayConvention">The optional seed business day convention, represents the adjustment of the  
            /// 'seed date' used in the main date generation loop, i.e. generates
            /// <para>
            ///     'adjusted seed date' - j * tenor, j=1,2,...
            /// </para>
            /// and adjust these dates with respect to <paramref name="businessDayConvention"/> (optional; will be ignored if <paramref name="firstRegularDate"/> is a date).
            /// </param>
            /// <param name="firstRegularDate">The first regular date (optional).</param>
            /// <param name="lastRegularDate">The last regular date (optional).</param>
            /// <remarks>The optional <paramref name="firstRegularDate"/> and <paramref name="lastRegularDate"/> are used to generated a short/long first/last stub period.</remarks>
            public DateScheduleRuleDescription(ITimeframeDescription startAndEndDateDescription, IDateScheduleFrequency frequency, IBusinessDayConvention businessDayConvention, IBusinessDayConvention seedBusinessDayConvention, DateTime? firstRegularDate = null, DateTime? lastRegularDate = null)
            {
                m_DateScheduleRule = new BackwardDateScheduleRule(startAndEndDateDescription, frequency, businessDayConvention, seedBusinessDayConvention, firstRegularDate, lastRegularDate);
            }
            #endregion

            #region IDateScheduleRuleDescription Members

            /// <summary>Gets a specific <see cref="IDateScheduleRule"/> representation.
            /// </summary>
            /// <param name="referenceDate">The reference date, i.e. the start date with respect to the date schedule rule.</param>
            /// <returns>A <see cref="IDateScheduleRule"/> object that represents a specific date schedule rule.
            /// </returns>
            public IDateScheduleRule GetDateScheduleRule(DateTime referenceDate)
            {
                return m_DateScheduleRule.GetAdaptedDateScheduleRule(referenceDate);
            }

            /// <summary>Gets the raw end date of the current <see cref="IDateScheduleRuleDescription"/>; where the raw end date is in general represented in a specific <see cref="TenorTimeSpan"/> or <see cref="System.DateTime"/> object.
            /// </summary>
            /// <returns>The raw end date of the current <see cref="IDateScheduleRuleDescription"/>; where the raw end date is in general represented in a specific <see cref="TenorTimeSpan"/> or <see cref="System.DateTime"/> object.</returns>
            public object GetRawEndDate()
            {
                return m_DateScheduleRule.m_TimeSpanDescription.GetRawEndDate();
            }
            #endregion

            #region IInfoOutputQueriable Members

            /// <summary>Gets the info-output level of detail.
            /// </summary>
            /// <value>The info-output level of detail.</value>
            public InfoOutputDetailLevel InfoOutputDetailLevel
            {
                get { return m_DateScheduleRule.InfoOutputDetailLevel; }
            }

            /// <summary>Sets the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel"/> property.
            /// </summary>
            /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
            /// <returns>A value indicating whether the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel"/> has been set to <paramref name="infoOutputDetailLevel"/>.
            /// </returns>
            public bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
            {
                return m_DateScheduleRule.TrySetInfoOutputDetailLevel(infoOutputDetailLevel);
            }

            /// <summary>Gets informations of the current object as a specific <see cref="InfoOutput"/> instance.
            /// </summary>
            /// <param name="infoOutput">The <see cref="InfoOutput"/> object which is to be filled with informations concering the current instance.</param>
            /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
            public void FillInfoOutput(InfoOutput infoOutput, string categoryName = InfoOutput.GeneralCategoryName)
            {
                m_DateScheduleRule.FillInfoOutput(infoOutput, categoryName);
            }
            #endregion

            /// <summary>Returns a <see cref="System.String" /> that represents this instance.
            /// </summary>
            /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
            public override string ToString()
            {
                return m_DateScheduleRule.ToString();
            }
        }
        #endregion

        #region public static readonly members

        /// <summary>The name of the date schedule generation rule name, i.e. "Backward".
        /// </summary>
        public static readonly IdentifierString ScheduleRuleName = new IdentifierString("Backward");
        #endregion

        #region private members

        /// <summary>The reference date used to compute the first and last date of <see cref="m_TimeSpanDescription"/> to take into account for the date schedule rule.
        /// </summary>
        private DateTime m_ReferenceDate;

        /// <summary>Represents the first and last date to take into account for the date schedule.
        /// </summary>
        private ITimeframeDescription m_TimeSpanDescription;

        /// <summary>The frequency.
        /// </summary>
        private IDateScheduleFrequency m_Frequency;

        /// <summary>The business day convention.
        /// </summary>
        private IBusinessDayConvention m_BusinessDayConvention;

        /// <summary>The optional seed business day convention, represents the adjustment of the  
        /// 'seed date' used in the main date generation loop, i.e. generates
        /// <para>
        ///     'adjusted seed date' - j * tenor, j=1,2,...
        /// </para>
        /// and adjust these dates with respect to the business day convention given in the constructor (optional; will be ignored if <c>firstRegularDate</c> is a valid date.
        /// </summary>
        private IBusinessDayConvention m_SeedBusinessDayConvention = null;

        /// <summary>The first regular date (optional).
        /// </summary>
        private DateTime? m_FirstRegularDate = null;

        /// <summary>The last regular date (optional).
        /// </summary>
        private DateTime? m_LastRegularDate = null;
        #endregion

        #region private constructors

        /// <summary>Initializes a new instance of the <see cref="BackwardDateScheduleRule"/> class.
        /// </summary>
        /// <param name="frequency">The frequency.</param>
        /// <param name="businessDayConvention">The business day convention.</param>
        /// <param name="seedBusinessDayConvention">The optional seed business day convention, represents the adjustment of the  
        /// 'seed date' used in the main date generation loop, i.e. generates
        /// <para>
        ///     'adjusted seed date' + j * tenor, j=1,2,...
        /// </para>
        /// and adjust these dates with respect to <paramref name="businessDayConvention"/> (optional; will be ignored if <paramref name="firstRegularDate"/> is a date).
        /// </param>
        /// <param name="firstRegularDate">The first regular date (optional).</param>
        /// <param name="lastRegularDate">The last regular date (optional).</param>
        /// <remarks>The optional <paramref name="firstRegularDate"/> and <paramref name="lastRegularDate"/> are used to generated a short/long first/last stub period.</remarks>
        private BackwardDateScheduleRule(IDateScheduleFrequency frequency, IBusinessDayConvention businessDayConvention, IBusinessDayConvention seedBusinessDayConvention = null, DateTime? firstRegularDate = null, DateTime? lastRegularDate = null)
        {
            if (frequency == null)
            {
                throw new ArgumentNullException("frequency");
            }
            m_Frequency = frequency;
            if (businessDayConvention == null)
            {
                throw new ArgumentNullException("businessDayConvention");
            }
            m_BusinessDayConvention = businessDayConvention;

            m_ReferenceDate = DateTime.MinValue;

            // the optional parameters
            m_SeedBusinessDayConvention = seedBusinessDayConvention;
            m_FirstRegularDate = firstRegularDate;
            m_LastRegularDate = lastRegularDate;
        }

        /// <summary>Initializes a new instance of the <see cref="BackwardDateScheduleRule"/> class.
        /// </summary>
        /// <param name="startAndEndDateDescription">A description of the start and end date to take into account in its <see cref="ITimeframeDescription"/> representation.</param>
        /// <param name="frequency">The frequency.</param>
        /// <param name="businessDayConvention">The business day convention.</param>
        /// <param name="seedBusinessDayConvention">The optional seed business day convention, represents the adjustment of the  
        /// 'seed date' used in the main date generation loop, i.e. generates
        /// <para>
        ///     'adjusted seed date' + j * tenor, j=1,2,...
        /// </para>
        /// and adjust these dates with respect to <paramref name="businessDayConvention"/> (optional; will be ignored if <paramref name="firstRegularDate"/> is a date).
        /// </param>
        /// <param name="firstRegularDate">The first regular date (optional).</param>
        /// <param name="lastRegularDate">The last regular date (optional).</param>
        /// <remarks>The optional <paramref name="firstRegularDate"/> and <paramref name="lastRegularDate"/> are used to generated a short/long first/last stub period.
        /// <para>A object created by this constructor is not ready to use, because the reference date with respect to <paramref name="startAndEndDateDescription"/> has to be specified;
        /// call <see cref="GetAdaptedDateScheduleRule(DateTime)"/> to create a valid object.</para></remarks>
        private BackwardDateScheduleRule(ITimeframeDescription startAndEndDateDescription, IDateScheduleFrequency frequency, IBusinessDayConvention businessDayConvention, IBusinessDayConvention seedBusinessDayConvention = null, DateTime? firstRegularDate = null, DateTime? lastRegularDate = null)
            : this(frequency, businessDayConvention, seedBusinessDayConvention, firstRegularDate, lastRegularDate)
        {
            if (startAndEndDateDescription == null)
            {
                throw new ArgumentNullException("startAndEndDateDescription");
            }
            m_TimeSpanDescription = startAndEndDateDescription;
        }
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="BackwardDateScheduleRule"/> class.
        /// </summary>
        /// <param name="referenceDate">A reference date used to calculate the start date and end date of the date schedule with respect to <paramref name="startAndEndDateDescription"/>.</param>
        /// <param name="startAndEndDateDescription">A description of the start date and the end date of the date schedule.</param>
        /// <param name="businessDayConvention">The business day convention.</param>
        /// <param name="frequency">The frequency.</param>
        /// <param name="seedBusinessDayConvention">The optional seed business day convention, represents the adjustment of the  
        /// 'seed date' used in the main date generation loop, i.e. generates
        /// <para>
        ///     'adjusted seed date' + j * tenor, j=1,2,...
        /// </para>
        /// and adjust these dates with respect to <paramref name="businessDayConvention"/> (optional; will be ignored if <paramref name="lastRegularDate"/> is a date; if <c>null</c> <paramref name="businessDayConvention"/> will be used).
        /// </param>
        /// <param name="firstRegularDate">The first regular date (optional).</param>
        /// <param name="lastRegularDate">The last regular date (optional).</param>
        /// <remarks>The optional <paramref name="firstRegularDate"/> and <paramref name="lastRegularDate"/> are used to generated a short/long first/last stub period.</remarks>
        public BackwardDateScheduleRule(DateTime referenceDate, ITimeframeDescription startAndEndDateDescription, IDateScheduleFrequency frequency, IBusinessDayConvention businessDayConvention, IBusinessDayConvention seedBusinessDayConvention = null, DateTime? firstRegularDate = null, DateTime? lastRegularDate = null)
            : this(frequency, businessDayConvention, seedBusinessDayConvention, firstRegularDate, lastRegularDate)
        {
            if (startAndEndDateDescription == null)
            {
                throw new ArgumentNullException("startAndEndDateDescription");
            }
            m_TimeSpanDescription = startAndEndDateDescription;
            m_ReferenceDate = referenceDate;
        }
        #endregion

        #region public properties

        #region IOperable Members

        /// <summary>Gets a value indicating whether this instance is operable.
        /// </summary>
        /// <value><c>true</c> if this instance is operable; otherwise, <c>false</c>.
        /// </value>
        public bool IsOperable
        {
            get { return (m_ReferenceDate != DateTime.MinValue); }
        }
        #endregion

        #region IInfoOutputQueriable Members

        /// <summary>Gets the info-output level of detail.
        /// </summary>
        /// <value>The info-output level of detail.</value>
        public InfoOutputDetailLevel InfoOutputDetailLevel
        {
            get { return InfoOutputDetailLevel.Full; }
        }
        #endregion

        /// <summary>Gets the frequency.
        /// </summary>
        /// <value>The frequency.</value>
        public IDateScheduleFrequency Frequency
        {
            get { return m_Frequency; }
        }

        /// <summary>Gets the business day convention.
        /// </summary>
        /// <value>The business day convention.</value>
        public IBusinessDayConvention BusinessDayConvention
        {
            get { return m_BusinessDayConvention; }
        }

        /// <summary>Gets the optional seed business day convention, represents the adjustment of the  
        /// 'seed date' used in the main date generation loop, i.e. generates
        /// <para>
        ///     'adjusted seed date' - j * tenor, j=1,2,...
        /// </para>
        /// and adjust these dates with respect to the business day convention given in the constructor (optional; will be ignored if <c>firstRegularDate</c>
        /// is a valid date.
        /// </summary>
        /// <value>The business day convention for the seed date.</value>
        public IBusinessDayConvention SeedBusinessDayConvention
        {
            get { return (m_SeedBusinessDayConvention != null) ? m_SeedBusinessDayConvention : m_BusinessDayConvention; }
        }

        /// <summary>Gets the first regular date (optional).
        /// </summary>
        /// <value>The (optional) first regular date.</value>
        public DateTime? FirstRegularDate
        {
            get { return m_FirstRegularDate; }
        }

        /// <summary>The last regular date (optional).
        /// </summary>
        /// <value>The (optional) last regular date.</value>
        public DateTime? LastRegularDate
        {
            get { return m_LastRegularDate; }
        }
        #endregion

        #region public static methods

        /// <summary>Creates a <see cref="IDateScheduleRuleDescription"/> wrapper for <see cref="BackwardDateScheduleRule"/> objects.
        /// </summary>
        /// <param name="startAndEndDatePeriodDescription">A description of the start date and the end date of the date schedule.</param>
        /// <param name="businessDayConvention">The business day convention.</param>
        /// <param name="frequency">The frequency.</param>
        /// <param name="seedBusinessDayConvention">The optional seed business day convention, represents the adjustment of the  
        /// 'seed date' used in the main date generation loop, i.e. generates
        /// <para>
        ///     'adjusted seed date' - j * tenor, j=1,2,...
        /// </para>
        /// and adjust these dates with respect to <paramref name="businessDayConvention"/> (optional; will be ignored if <paramref name="lastRegularDate"/> is a date; if <c>null</c> <paramref name="businessDayConvention"/> will be used).
        /// </param>
        /// <param name="firstRegularDate">The first regular date (optional).</param>
        /// <param name="lastRegularDate">The last regular date (optional).</param>
        /// <returns>A <see cref="IDateScheduleRuleDescription"/> object that encapsulate a <see cref="BackwardDateScheduleRule"/> object.</returns>
        /// <remarks>The optional <paramref name="firstRegularDate"/> and <paramref name="lastRegularDate"/> are used to generated a short/long first/last stub period.</remarks>
        public static IDateScheduleRuleDescription CreateDescriptor(ITimeframeDescription startAndEndDatePeriodDescription, IDateScheduleFrequency frequency, IBusinessDayConvention businessDayConvention, IBusinessDayConvention seedBusinessDayConvention = null, DateTime? firstRegularDate = null, DateTime? lastRegularDate = null)
        {
            return new DateScheduleRuleDescription(startAndEndDatePeriodDescription, frequency, businessDayConvention, seedBusinessDayConvention, firstRegularDate, lastRegularDate);
        }
        #endregion

        #region public methods

        #region IDateScheduleRule Members

        /// <summary>Adds some <see cref="System.DateTime"/> objects into a specific date schedule.
        /// </summary>
        /// <param name="dateSchedule">The date schedule.</param>
        /// <exception cref="NotOperableException">Thrown, if the current instance it not operable.</exception>
        public void AddDatesToDateSchedule(DateSchedule dateSchedule)
        {
            if (IsOperable == false)
            {
                throw new NotOperableException("BackwardDateScheduleRule");
            }
            DateTime firstDate, terminationDate;
            m_TimeSpanDescription.GetStartAndEndDate(m_ReferenceDate, dateSchedule.HolidayCalendar, out firstDate, out terminationDate, dateSchedule.Logging);

            if (terminationDate < firstDate)
            {
                throw new ArgumentException(String.Format(ExceptionMessages.ArgumentOutOfRangeGreaterEqual, "End date [" + terminationDate.ToShortDateString() + "]", "first date [" + firstDate.ToShortDateString() + "]"));
            }
            dateSchedule.Add(terminationDate);  // add the end date

            // gets the date which is used as 'base date' for the date generation loop:
            DateTime seed = terminationDate;
            if ((m_LastRegularDate != null) && (m_LastRegularDate.HasValue))  // 'last regular date' is given
            {
                seed = m_LastRegularDate.Value;
                if (seed > terminationDate)
                {
                    throw new ArgumentException(String.Format(ExceptionMessages.ArgumentOutOfRangeLessEqual, "Last regular date [" + seed.ToShortDateString() + "]", "end date [" + terminationDate.ToShortDateString() + "]"));
                }
            }
            if (m_SeedBusinessDayConvention != null)  // perhaps adjust the 'base date' which is used for the date generation loop
            {
                seed = m_SeedBusinessDayConvention.GetAdjustedDate(seed, dateSchedule.HolidayCalendar);
            }
            else
            {
                seed = m_BusinessDayConvention.GetAdjustedDate(seed, dateSchedule.HolidayCalendar);
            }
            dateSchedule.Add(seed);

            // gets the exit condition for the loop:
            DateTime exitDate = firstDate;  // perhaps not a business day
            if ((m_FirstRegularDate != null) && (m_FirstRegularDate.HasValue))
            {
                exitDate = m_FirstRegularDate.Value;
                if (exitDate > seed)
                {
                    throw new ArgumentException(String.Format(ExceptionMessages.ArgumentOutOfRangeLessEqual, "First regular date [" + exitDate.ToShortDateString() + "]", "last [regular] date [" + seed.ToShortDateString() + "]"));
                }
            }

            if (m_Frequency.IsOnceFrequency()) // end date and seed date are already added
            {
                if (exitDate != firstDate) // perhaps add the first regular date
                {
                    dateSchedule.Add(exitDate); // one may apply the business day convention to the first regular date
                }
            }
            else  // do the loop with respect to the frequency
            {
                TenorTimeSpan frequencyTenor = m_Frequency.GetFrequencyTenor();
                DateTime runningDate = m_BusinessDayConvention.GetAdjustedDate(seed.AddTenorTimeSpan(frequencyTenor), dateSchedule.HolidayCalendar);
                int periodIndex = -1;

                while (runningDate >= exitDate)
                {
                    dateSchedule.Add(runningDate);
                    periodIndex--;

                    runningDate = seed.AddTenorTimeSpan(frequencyTenor, periodIndex);
                    runningDate = m_BusinessDayConvention.GetAdjustedDate(runningDate, dateSchedule.HolidayCalendar);
                }
            }
            dateSchedule.Add(firstDate);
        }
        #endregion

        #region IInfoOutputQueriable Members

        /// <summary>Sets the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel"/> property.
        /// </summary>
        /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
        /// <returns>A value indicating whether the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel"/> has been set to <paramref name="infoOutputDetailLevel"/>.
        /// </returns>
        public bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
        {
            return (infoOutputDetailLevel == InfoOutputDetailLevel.Full);
        }

        /// <summary>Gets informations of the current object as a specific <see cref="InfoOutput"/> instance.
        /// </summary>
        /// <param name="infoOutput">The <see cref="InfoOutput"/> object which is to be filled with informations concering the current instance.</param>
        /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
        public void FillInfoOutput(InfoOutput infoOutput, string categoryName = InfoOutput.GeneralCategoryName)
        {
            InfoOutputPackage infoOutputCollection = infoOutput.AcquirePackage(categoryName);
            infoOutputCollection.Add("Frequency", m_Frequency.Name);
            infoOutputCollection.Add("Business day convention", m_BusinessDayConvention.Name);

            if ((m_FirstRegularDate != null) && (m_FirstRegularDate.HasValue == true))
            {
                infoOutputCollection.Add("First regular date", m_FirstRegularDate.Value);
            }
            if ((m_LastRegularDate != null) && (m_LastRegularDate.HasValue == true))
            {
                infoOutputCollection.Add("Last regular date", m_LastRegularDate.Value);
            }
            infoOutputCollection.Add("Seed date business day convention", SeedBusinessDayConvention.Name);
            if (m_ReferenceDate != DateTime.MinValue)
            {
                infoOutputCollection.Add("Reference date", m_ReferenceDate);
            }
            m_TimeSpanDescription.FillInfoOutput(infoOutput, categoryName);
        }
        #endregion

        /// <summary>Gets an adapted copy of the current date schedule rule.
        /// </summary>
        /// <param name="referenceDate">The reference date.</param>
        /// <returns>A <see cref="BackwardDateScheduleRule"/> object with the same parameter except the reference date used to calculate the start and end date.</returns>
        /// <remarks>Use this method if a the current object is created via the 
        /// <see cref="BackwardDateScheduleRule(ITimeframeDescription, IDateScheduleFrequency, IBusinessDayConvention, IBusinessDayConvention, DateTime?, DateTime?)"/> constructor.</remarks>
        private BackwardDateScheduleRule GetAdaptedDateScheduleRule(DateTime referenceDate)
        {
            BackwardDateScheduleRule dateSchedule = new BackwardDateScheduleRule(m_TimeSpanDescription, m_Frequency, m_BusinessDayConvention, m_SeedBusinessDayConvention, m_FirstRegularDate, m_LastRegularDate);
            dateSchedule.m_ReferenceDate = referenceDate;
            return dateSchedule;
        }

        /// <summary>Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append(String.Format(DateFactoryResources.Frequency, m_Frequency));
            str.Append(",");
            str.Append(m_TimeSpanDescription.ToString());
            str.Append(",");
            str.Append(String.Format("Business day convention {0}", m_BusinessDayConvention.Name.String));

            if (m_SeedBusinessDayConvention != null)
            {
                str.Append(",");
                str.Append(String.Format(DateFactoryResources.SeedBusinessDayConvention, m_SeedBusinessDayConvention.LongName.String));
            }
            if ((m_FirstRegularDate != null) && (m_FirstRegularDate.HasValue))
            {
                str.Append(",");
                str.Append(String.Format(DateFactoryResources.FirstRegularDate, m_FirstRegularDate.Value.ToShortDateString()));
            }
            if ((m_LastRegularDate != null) && (m_LastRegularDate.HasValue))
            {
                str.Append(",");
                str.Append(String.Format(DateFactoryResources.LastRegularDate, m_LastRegularDate.Value.ToShortDateString()));
            }
            str.Append(".");
            return str.ToString();
        }
        #endregion
    }
}