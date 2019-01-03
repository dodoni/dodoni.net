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
using Dodoni.Finance.DateFactory.TimeframeDescriptions;

namespace Dodoni.Finance.DateFactory
{
    /// <summary>Serves as factory for <see cref="ITimeframeDescription"/> objects.
    /// </summary>
    public static class TimeframeDescription
    {
        #region nested classes

        /// <summary>The 'no adjustment' business day convention.
        /// </summary>
        private class NoAdjustment : IBusinessDayConvention
        {
            private IdentifierString m_Name = new IdentifierString("NoAdjustment");

            #region IBusinessDayConvention Members

            /// <summary>Gets an adjusted date with respect to a specific <see cref="System.DateTime"/> object.
            /// </summary>
            /// <param name="date">The date.</param>
            /// <param name="holidayCalendar">The holiday calendar.</param>
            /// <returns>The <see cref="System.DateTime"/> object that is given by <paramref name="date"/> taken into account the business day
            /// convention represented by the current instance.
            /// </returns>
            public DateTime GetAdjustedDate(DateTime date, IHolidayCalendar holidayCalendar)
            {
                return date;
            }

            /// <summary>Gets the type of the adjustment, i.e. a value indicating whether the result of <see cref="IBusinessDayConvention.GetAdjustedDate(DateTime, IHolidayCalendar)"/> is a business day.
            /// </summary>
            /// <value>The type of the date adjustment.</value>
            public BusinessDayAdjustmentType AdjustmentType
            {
                get { return BusinessDayAdjustmentType.UnknownStateOfAdjustedDate; }
            }
            #endregion

            #region IIdentifierNameable Members

            /// <summary>Gets the name of the current instance.
            /// </summary>
            /// <value>The language independent name of the current instance.</value>
            public IdentifierString Name
            {
                get { return m_Name; }
            }

            /// <summary>Gets the long name of the current instance.
            /// </summary>
            /// <value>The (perhaps) language dependent long name of the current instance.
            /// </value>
            public IdentifierString LongName
            {
                get { return m_Name; }
            }
            #endregion

            #region IAnnotatable Members

            /// <summary>Gets a value indicating whether the annotation is read-only.
            /// </summary>
            /// <value><c>true</c> if the annotation of this instance is readonly; otherwise, <c>false</c>.
            /// </value>
            bool IAnnotatable.HasReadOnlyAnnotation
            {
                get { return true; }
            }

            /// <summary>Gets the annotation of the current instance.
            /// </summary>
            /// <value>The annotation of the current instance.</value>
            string IAnnotatable.Annotation
            {
                get { return String.Empty; }
            }

            /// <summary>Sets the annotation of the current instance.
            /// </summary>
            /// <param name="annotation">The annotation.</param>
            /// <returns>A value indicating whether the <see cref="IAnnotatable.Annotation"/> has been changed.
            /// </returns>
            bool IAnnotatable.TrySetAnnotation(string annotation)
            {
                return false;
            }
            #endregion
        }
        #endregion

        #region private (static) members

        /// <summary>The 'no adjustment' business day convention.
        /// </summary>
        private static IBusinessDayConvention sm_NoAdjustment = new NoAdjustment();
        #endregion

        #region public (static) methods

        /// <summary>Creates a new <see cref="ITimeframeDescription"/> object.
        /// </summary>
        /// <param name="tenor">The tenor, i.e. the end date of the timeframe in its <see cref="TenorTimeSpan"/> representation.</param>
        /// <param name="spotDateAdjustment">A business day convention used to compute the spot date (= reference date + a number of business days to settle [if <paramref name="tenor"/> is regular or one business day in the case of tomorrow-next]); if <c>null</c> no adjustment will be applied.</param>
        /// <param name="startDateAdjustment">A business day convention used to compute the start date of the timeframe; if <c>null</c> no adjustment will be applied.</param>
        /// <param name="endDateAdjustment">A business day convention used to compute the end date of the timeframe; if <c>null</c> no adjustment will be applied.</param>
        /// <param name="businessDaysToSettle">The number of business days used to calculate the spot date; this parameter will be taken into account if and only if <paramref name="tenor"/> is a regular tenor (i.e. neigther overnight nor tomorrow-next).</param>
        /// <returns>A <see cref="ITimeframeDescription"/> object where the implementation works in the following way:
        ///  <list type="number">
        ///   <item><description>Apply the SpotDateAdjustment to the reference date; call it spot date.</description></item>
        ///   <item><description>For regular tenor with a number of business days to settle [in short: BD-ToSettle] != 0 or tomorrow-next, point the spot date to the 
        ///   next (or previous if BD-ToSettle is lesss than 0) business day and add BD-ToSettle business days. This is the final spot date.</description></item>
        ///   <item><description>Apply the StartDateAdjustment to the spot date. This is the start date of the period.</description></item>
        ///   <item><description>Add the tenor to the start date and apply the EndDateAdjustment which gives us the end date.</description></item>
        /// </list></returns>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="tenor"/> represents a negative time span.</exception>
        public static ITimeframeDescription Create(TenorTimeSpan tenor, IBusinessDayConvention spotDateAdjustment = null, IBusinessDayConvention startDateAdjustment = null, IBusinessDayConvention endDateAdjustment = null, int businessDaysToSettle = 0)
        {
            return new TenorTimeframeDescription(tenor, (spotDateAdjustment != null) ? spotDateAdjustment : sm_NoAdjustment, (startDateAdjustment != null) ? startDateAdjustment : sm_NoAdjustment, (endDateAdjustment != null) ? endDateAdjustment : sm_NoAdjustment, businessDaysToSettle);
        }

        /// <summary>Creates a new <see cref="ITimeframeDescription"/> object.
        /// </summary>
        /// <param name="tenor">The tenor, i.e. the end date of the timeframe in its <see cref="TenorTimeSpan"/> representation.</param>
        /// <param name="businessDayConvention">The business day convention used to compute the spot date, start date as well as the end date of the timeframe.</param>
        /// <param name="businessDaysToSettle">The number of business days used to calculate the spot date; this parameter will be taken into account if and only if <paramref name="tenor"/> is a regular tenor (i.e. neigther overnight nor tomorrow-next).</param>
        /// <returns>A <see cref="ITimeframeDescription"/> object where the implementation works in the following way:
        ///  <list type="number">
        ///   <item><description>Apply the SpotDateAdjustment to the reference date; call it spot date.</description></item>
        ///   <item><description>For regular tenor with a number of business days to settle [in short: BD-ToSettle] != 0 or tomorrow-next, point the spot date to the 
        ///   next (or previous if BD-ToSettle is lesss than 0) business day and add BD-ToSettle business days. This is the final spot date.</description></item>
        ///   <item><description>Apply the StartDateAdjustment to the spot date. This is the start date of the period.</description></item>
        ///   <item><description>Add the tenor to the start date and apply the EndDateAdjustment which gives us the end date.</description></item>
        /// </list></returns>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="tenor"/> represents a negative time span.</exception>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="businessDayConvention"/> is <c>null</c>.</exception>
        public static ITimeframeDescription Create(TenorTimeSpan tenor, IBusinessDayConvention businessDayConvention, int businessDaysToSettle = 0)
        {
            if (businessDayConvention == null)
            {
                throw new ArgumentNullException("businessDayConvention");
            }
            return new TenorTimeframeDescription(tenor, businessDayConvention, businessDayConvention, businessDayConvention, businessDaysToSettle);
        }

        /// <summary>Creates a new <see cref="ITimeframeDescription"/> object.
        /// </summary>
        /// <param name="endDate">The end date of the timeframe.</param>
        /// <param name="spotDateAdjustment">A business day convention used to compute the spot date (= reference date + a number of business days to settle; if <c>null</c> no adjustment will be applied.</param>
        /// <param name="startDateAdjustment">A business day convention used to compute the start date of the timeframe; if <c>null</c> no adjustment will be applied.</param>
        /// <param name="endDateAdjustment">A business day convention used to compute the end date of the timeframe; if <c>null</c> no adjustment will be applied.</param>
        /// <param name="businessDaysToSettle">The number of business days used to calculate the spot date.</param>
        /// <returns>A <see cref="ITimeframeDescription"/> object where the implementation works in the following way:
        ///  <list type="number">
        ///   <item><description>Apply the SpotDateAdjustment to the reference date; call it spot date.</description></item>
        ///   <item><description>Point the spot date to the next business day if the number of business days to settle is strictly greater than 0 or to the previous business day if
        ///   the number of business days to settle is strictly less than 0. Add the number of business days to settle which defines the final spot date.</description></item>
        ///   <item><description>Apply the StartDateAdjustment to the spot date. This is the start date of the period.</description></item>
        ///   <item><description>Apply the EndDateAdjustment to the specified end date; this is the end date of the timeframe.</description></item>
        /// </list></returns>
        public static ITimeframeDescription Create(DateTime endDate, IBusinessDayConvention spotDateAdjustment = null, IBusinessDayConvention startDateAdjustment = null, IBusinessDayConvention endDateAdjustment = null, int businessDaysToSettle = 0)
        {
            return new DateTimeTimeframeDescription(endDate, (spotDateAdjustment != null) ? spotDateAdjustment : sm_NoAdjustment, (startDateAdjustment != null) ? startDateAdjustment : sm_NoAdjustment, (endDateAdjustment != null) ? endDateAdjustment : sm_NoAdjustment, businessDaysToSettle);
        }

        /// <summary>Creates a new <see cref="ITimeframeDescription"/> object.
        /// </summary>
        /// <param name="endDate">The end date of the timeframe.</param>
        /// <param name="businessDayConvention">The business day convention used to compute the spot date, start date as well as the end date of the timeframe.</param>
        /// <param name="businessDaysToSettle">The number of business days used to calculate the spot date.</param>
        /// <returns>A <see cref="ITimeframeDescription"/> object where the implementation works in the following way:
        ///  <list type="number">
        ///   <item><description>Apply the SpotDateAdjustment to the reference date; call it spot date.</description></item>
        ///   <item><description>Point the spot date to the next business day if the number of business days to settle is strictly greater than 0 or to the previous business day if
        ///   the number of business days to settle is strictly less than 0. Add the number of business days to settle which defines the final spot date.</description></item>
        ///   <item><description>Apply the StartDateAdjustment to the spot date. This is the start date of the period.</description></item>
        ///   <item><description>Apply the EndDateAdjustment to the specified end date; this is the end date of the timeframe.</description></item>
        /// </list></returns>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="businessDayConvention"/> is <c>null</c>.</exception>
        public static ITimeframeDescription Create(DateTime endDate, IBusinessDayConvention businessDayConvention, int businessDaysToSettle = 0)
        {
            if (businessDayConvention == null)
            {
                throw new ArgumentNullException("businessDayConvention");
            }
            return new DateTimeTimeframeDescription(endDate, businessDayConvention, businessDayConvention, businessDayConvention, businessDaysToSettle);
        }

        /// <summary>Creates a new <see cref="ITimeframeDescription"/> object.
        /// </summary>
        /// <param name="startDateTenor">The tenor use to calculate the start date.</param>
        /// <param name="tenor">The tenor that represents the time span.</param>
        /// <param name="spotDateAdjustment">A business day convention used to compute the spot date (= reference date + a number of business days to settle [if <paramref name="startDateTenor"/> is regular or one business day in the case of tomorrow-next]); if <c>null</c> no adjustment will be applied.</param>
        /// <param name="startDateAdjustment">A business day convention used to compute the start date of the timeframe; if <c>null</c> no adjustment will be applied.</param>
        /// <param name="endDateAdjustment">A business day convention used to compute the end date of the timeframe; if <c>null</c> no adjustment will be applied.</param>
        /// <param name="businessDaysToSettle">The number of business days used to calculate the spot date; this parameter will be taken into account if and only if <paramref name="startDateTenor"/> is a regular tenor (i.e. neigther overnight nor tomorrow-next).</param>
        /// <returns>A <see cref="ITimeframeDescription"/> object where the implementation works in the following way:
        /// <para>The implementation works in the following way:
        ///  <list type="number">
        ///   <item><description>Apply the SpotDateAdjustment to the reference date; call it spot date.</description></item>
        ///   <item><description>For regular tenor with a number of business days to settle [in short: BD-ToSettle] != 0 or tomorrow-next, point the spot date to the 
        ///   next (or previous if BD-ToSettle is lesss than 0) business day and add BD-ToSettle business days. This is the final spot date.</description></item>
        ///   <item><description>Apply the start tenor to the spot date and apply the StartDateAdjustment. This is the start date of the period.</description></item>
        ///   <item><description>Add the tenor to the start date and apply the EndDateAdjustment which gives us the end date.</description></item>
        /// </list></para></returns>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="startDateTenor"/> or <paramref name="tenor"/> represents a negative time span.</exception>
        public static ITimeframeDescription Create(TenorTimeSpan startDateTenor, TenorTimeSpan tenor, IBusinessDayConvention spotDateAdjustment, IBusinessDayConvention startDateAdjustment, IBusinessDayConvention endDateAdjustment, int businessDaysToSettle = 0)
        {
            return new ForwardStartingTimeframeDescription(startDateTenor, tenor, (spotDateAdjustment != null) ? spotDateAdjustment : sm_NoAdjustment, (startDateAdjustment != null) ? startDateAdjustment : sm_NoAdjustment, (endDateAdjustment != null) ? endDateAdjustment : sm_NoAdjustment, businessDaysToSettle);
        }

        /// <summary>Creates a new <see cref="ITimeframeDescription"/> object.
        /// </summary>
        /// <param name="startDateTenor">The tenor use to calculate the start date.</param>
        /// <param name="tenor">The tenor that represents the time span.</param>
        /// <param name="businessDayConvention">The business day convention used to compute the spot date, start date as well as the end date of the timeframe.</param>
        /// <param name="businessDaysToSettle">The number of business days used to calculate the spot date; this parameter will be taken into account if and only if <paramref name="startDateTenor"/> is a regular tenor (i.e. neigther overnight nor tomorrow-next).</param>
        /// <returns>A <see cref="ITimeframeDescription"/> object where the implementation works in the following way:
        /// <para>The implementation works in the following way:
        ///  <list type="number">
        ///   <item><description>Apply the SpotDateAdjustment to the reference date; call it spot date.</description></item>
        ///   <item><description>For regular tenor with a number of business days to settle [in short: BD-ToSettle] != 0 or tomorrow-next, point the spot date to the 
        ///   next (or previous if BD-ToSettle is lesss than 0) business day and add BD-ToSettle business days. This is the final spot date.</description></item>
        ///   <item><description>Apply the start tenor to the spot date and apply the StartDateAdjustment. This is the start date of the period.</description></item>
        ///   <item><description>Add the tenor to the start date and apply the EndDateAdjustment which gives us the end date.</description></item>
        /// </list></para></returns>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="startDateTenor"/> or <paramref name="tenor"/> represents a negative time span.</exception>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="businessDayConvention"/> is <c>null</c>.</exception>
        public static ITimeframeDescription Create(TenorTimeSpan startDateTenor, TenorTimeSpan tenor, IBusinessDayConvention businessDayConvention, int businessDaysToSettle = 0)
        {
            if (businessDayConvention == null)
            {
                throw new ArgumentNullException("businessDayConvention");
            }
            return new ForwardStartingTimeframeDescription(startDateTenor, tenor, businessDayConvention, businessDayConvention, businessDayConvention, businessDaysToSettle);
        }
        #endregion
    }
}