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
    /// <summary>The day count convention ISDA 'Act/Act' (historical). This day count fraction is defined as the actual numbers of days in the period over over 365 or 366 (leap year).
    /// </summary>
    internal class ActualActualISDA : IDayCountConvention
    {
        #region private static readonly members

        /// <summary>The name of the day count convention.
        /// </summary>
        private static readonly IdentifierString sm_Name = new IdentifierString("Act/Act (ISDA)");

        /// <summary>The long name of the day count convention, i.e. language dependent.
        /// </summary>
        private static readonly IdentifierString sm_LongName = new IdentifierString(DayCountConventionResources.ActualActualISDALongName);

        /// <summary>The annotation, i.e. description of the day count convention.
        /// </summary>
        private static readonly string sm_Annotation = DayCountConventionResources.ActualActualISDA;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="ActualActualISDA"/> class.
        /// </summary>
        public ActualActualISDA()
        {
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the day count convention.
        /// </summary>
        /// <value>The name of the day count convention.</value>
        public IdentifierString Name
        {
            get { return ActualActualISDA.sm_Name; }
        }

        /// <summary>Gets the long name of the day count convention.
        /// </summary>
        /// <value>The language dependent long name of the day count convention.</value>
        public IdentifierString LongName
        {
            get { return ActualActualISDA.sm_LongName; }
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
            get { return ActualActualISDA.sm_Annotation; }
        }
        #endregion

        #region IDayCountConvention Members

        /// <summary>Gets a value indicating how to interpret the parameters <code>referenceStartDate</code> and <code>referenceEndDate</code>
        /// in <see cref="IDayCountConvention.GetYearFraction(DateTime, DateTime, DateTime?, DateTime?)"/> and <see cref="IDayCountConvention.GetYearFraction(DateTime, DateTime, out int, DateTime?, DateTime?)"/>.
        /// </summary>
        /// <value>A value indicating whether a reference period is used.</value>
        public DayCountReferencePeriodUsage ReferencePeriodUsage
        {
            get { return DayCountReferencePeriodUsage.Ignore; }
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
            return GetDateYearFraction(startDate, endDate);
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
            dayDifference = endDate.Subtract(startDate).Days;
            return GetDateYearFraction(startDate, endDate);
        }
        #endregion

        /// <summary>Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return sm_LongName.String;
        }
        #endregion

        #region private methods

        /// <summary>Gets the day count fraction between two dates.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>The date year fraction, i.e. the number of days in the time period over the number of days per year.
        /// </returns>
        private double GetDateYearFraction(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                return -GetDateYearFraction(endDate, startDate);
            }
            if (startDate.Year == endDate.Year)
            {
                double daysPerYear = DateTime.IsLeapYear(startDate.Year) ? 366.0 : 365.0;
                return endDate.Subtract(startDate).Days / daysPerYear;
            }
            /* get the number of whole years between start and end date and compute the number of days
             * in the intervals [startDate; 31.12.the same year], [1.1.year of end date; end date]: */
            int numberOfYearsInBetween = endDate.Year - startDate.Year - 1;

            int daysStartYear = (new DateTime(startDate.Year + 1, 1, 1)).Subtract(startDate).Days;
            double daysPerYearStartYear = DateTime.IsLeapYear(startDate.Year) ? 366.0 : 365.0;

            int daysEndYear = endDate.Subtract(new DateTime(endDate.Year, 1, 1)).Days;
            double daysPerYearEndYear = DateTime.IsLeapYear(endDate.Year) ? 366.0 : 365.0;

            return numberOfYearsInBetween + daysStartYear / daysPerYearStartYear + daysEndYear / daysPerYearEndYear;
        }
        #endregion
    }
}