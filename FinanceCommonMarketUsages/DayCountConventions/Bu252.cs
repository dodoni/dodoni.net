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

using Dodoni.Finance;
using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Utilities;

namespace Dodoni.Finance.CommonMarketUsages.DayCountConventions
{
    /// <summary>The day count convention 'BU/252' used for Brazilian trades. This day count fraction is defined as the numbers of business days in the period over 252.
    /// </summary>
    public class Bu252
    {
        #region public static readonly members

        /// <summary>The annotation, i.e. description of the day count convention.
        /// </summary>
        private static readonly string sm_Annotation = DayCountConventionResources.Bu252;
        #endregion

        #region nested classes

        /// <summary>Represents the implementation of the day count convention 'BU/252'.
        /// </summary>
        private class DayCountConventionImplementation : IDayCountConvention
        {
            #region private members

            /// <summary>The name of the day count convention.
            /// </summary>
            private IdentifierString m_Name;

            /// <summary>The long name of the day count convention, i.e. language dependent.
            /// </summary>
            private IdentifierString m_LongName;

            /// <summary>The holiday calendar.
            /// </summary>
            private IHolidayCalendar m_HolidayCalendar;
            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="DayCountConventionImplementation"/> class.
            /// </summary>
            /// <param name="holidayCalendar">The holiday calendar.</param>
            /// <exception cref="ArgumentNullException">Thrown, if <paramref name="holidayCalendar"/> is <c>null</c>.</exception>
            internal DayCountConventionImplementation(IHolidayCalendar holidayCalendar)
            {
                if (holidayCalendar == null)
                {
                    throw new ArgumentNullException("holidayCalendar");
                }
                m_HolidayCalendar = holidayCalendar;

                m_Name = new IdentifierString("Bu/252 [" + holidayCalendar.Name.String + "]");
                m_LongName = new IdentifierString(String.Format(DayCountConventionResources.Bu252LongName, (string)holidayCalendar.LongName));
            }
            #endregion

            #region public properties

            #region IIdentifierNameable Members

            /// <summary>Gets the name of the day count convention.
            /// </summary>
            /// <value>The name of the day count convention.</value>
            public IdentifierString Name
            {
                get { return m_Name; }
            }

            /// <summary>Gets the long name of the day count convention.
            /// </summary>
            /// <value>The language dependent long name of the day count convention.</value>
            public IdentifierString LongName
            {
                get { return m_LongName; }
            }
            #endregion

            #region IAnnotatable Members

            /// <summary>Gets a value indicating whether the annotation is readonly.
            /// </summary>
            /// <value>
            /// 	<c>true</c> if the annotation of this instance is readonly; otherwise, <c>false</c>.
            /// </value>
            bool IAnnotatable.HasReadOnlyAnnotation
            {
                get { return true; }
            }

            /// <summary>Gets the annotation of the current instance.
            /// </summary>
            /// <value>The annotation of the current instance.</value>
            public string Annotation
            {
                get { return Bu252.sm_Annotation; }
            }
            #endregion

            #region IDayCountConvention Members

            /// <summary>Gets a value indicating how to interpret the parameters <code>referenceStartDate</code> and <code>referenceEndDate</code>
            /// in <see cref="IDayCountConvention.GetYearFraction(DateTime, DateTime, DateTime?, DateTime?)"/> and <see cref="IDayCountConvention.GetYearFraction(DateTime, DateTime, out int, DateTime?, DateTime?)"/>.
            /// </summary>
            /// <value>A value indicating whether a reference period is used.</value>
            public DayCountReferencePeriodUsage ReferencePeriodUsage
            {
                get { return DayCountReferencePeriodUsage.OptionalInterestPeriod; }
            }
            #endregion

            #endregion

            #region public methods

            #region IAnnotatable Members

            /// <summary>Sets the annotation of the current instance.
            /// </summary>
            /// <param name="annotation">The annotation.</param>
            /// <returns>A value indicating whether the <see cref="Annotation"/> has been changed.
            /// </returns>
            bool IAnnotatable.TrySetAnnotation(string annotation)
            {
                return false;
            }
            #endregion

            #region IDayCountConvention Members

            /// <summary>Gets the date year fraction between two <see cref="System.DateTime"/> objects.
            /// </summary>
            /// <param name="startDate">The start date of the period.</param>
            /// <param name="endDate">The end date of the period.</param>
            /// <param name="referenceStartDate">The (optional) reference start date; see <see cref="IDayCountConvention.ReferencePeriodUsage"/>
            /// for its interpretation. Can be set to <c>null</c> in any case.</param>
            /// <param name="referenceEndDate">The (optional) reference end date; see <see cref="IDayCountConvention.ReferencePeriodUsage"/>
            /// for its interpretation. Can be set to <c>null</c> in any case.</param>
            /// <returns>The date year fraction for the period [<paramref name="startDate"/>; <paramref name="endDate"/>].
            /// </returns>
            public double GetYearFraction(DateTime startDate, DateTime endDate, DateTime? referenceStartDate = null, DateTime? referenceEndDate = null)
            {
                startDate = referenceStartDate ?? startDate;
                endDate = referenceEndDate ?? endDate;

                return GetNumerator(startDate, endDate) / 252.0;
            }

            /// <summary>Gets the date year fraction between two <see cref="System.DateTime"/> objects.
            /// </summary>
            /// <param name="startDate">The start date of the period.</param>
            /// <param name="endDate">The end date of the period.</param>
            /// <param name="dayDifference">The number of days in the period (output).</param>
            /// <param name="referenceStartDate">The (optional) reference start date; see <see cref="IDayCountConvention.ReferencePeriodUsage"/>
            /// for its interpretation. Can be set to <c>null</c> in any case.</param>
            /// <param name="referenceEndDate">The (optional) reference end date; see <see cref="IDayCountConvention.ReferencePeriodUsage"/>
            /// for its interpretation. Can be set to <c>null</c> in any case.</param>
            /// <returns>The date year fraction for the period [<paramref name="startDate"/>; <paramref name="endDate"/>].
            /// </returns>
            public double GetYearFraction(DateTime startDate, DateTime endDate, out int dayDifference, DateTime? referenceStartDate = null, DateTime? referenceEndDate = null)
            {
                startDate = referenceStartDate ?? startDate;
                endDate = referenceEndDate ?? endDate;

                dayDifference = GetNumerator(startDate, endDate);

                return dayDifference / 252.0;
            }
            #endregion

            /// <summary>Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </summary>
            /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </returns>
            public override string ToString()
            {
                return m_LongName.String;
            }
            #endregion

            #region private methods

            /// <summary>Gets the numerator of the day count convention, i.e. the number of business days in the period.
            /// </summary>
            /// <param name="startDate">The start date of the period.</param>
            /// <param name="endDate">The end date of the period.</param>
            /// <returns>The number of business days in the period [<paramref name="startDate"/>, <paramref name="endDate"/>[.</returns>
            private int GetNumerator(DateTime startDate, DateTime endDate)
            {
                // 'GetNumberOfBusinessDaysInBetween' considers ]startDate, endDate], but here, we would
                // like to use [startDate, endDate[, therefore we have to adjust start-/endDate first

                startDate = BusinessDayConvention.Following.GetAdjustedDate(startDate, m_HolidayCalendar);
                endDate = BusinessDayConvention.Preceding.GetAdjustedDate(endDate, m_HolidayCalendar);

                if (endDate < startDate)
                {
                    return m_HolidayCalendar.GetNumberOfBusinessDaysInBetween(endDate, startDate);
                }
                return m_HolidayCalendar.GetNumberOfBusinessDaysInBetween(startDate, endDate);
            }
            #endregion
        }
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="Bu252"/> class.
        /// </summary>
        internal Bu252()
        {
        }
        #endregion

        #region public methods

        /// <summary>Creates a specified 'BU/252' day count convention used for Brazilian trades. 
        /// </summary>
        /// <param name="holidayCalendar">The holiday calendar.</param>
        /// <param name="addToPool">The return value will be stored into the <see cref="DayCountConvention"/> pool for later use.</param>
        /// <returns>The specified 'BU/252' day count convention used for Brazilian trades. This day count fraction is defined as the numbers 
        /// of business days in the period over 252the specified holiday calendar.</returns>
        public IDayCountConvention Create(IHolidayCalendar holidayCalendar, bool addToPool = false)
        {
            IDayCountConvention dayCountConvention = new DayCountConventionImplementation(holidayCalendar);
            if (addToPool == true)
            {
                DayCountConvention.Add(dayCountConvention);
            }
            return dayCountConvention;
        }
        #endregion
    }
}