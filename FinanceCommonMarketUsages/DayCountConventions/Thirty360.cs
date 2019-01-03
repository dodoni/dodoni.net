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
    /// <summary>The day count convention '30/360' (i.e. Bond basis 30/360). All months have 30 days and each year has 360 days with an exception that if the
    /// last day is 31st and the first day is not 30th or 31ts then that month has 31 days.
    /// </summary>
    internal class Thirty360 : IDayCountConvention
    {
        #region private static readonly members

        /// <summary>The name of the day count convention.
        /// </summary>
        private readonly static IdentifierString sm_Name = new IdentifierString("30/360");

        /// <summary>The long name of the day count convention, i.e. language dependent.
        /// </summary>
        private static readonly IdentifierString sm_LongName = new IdentifierString(DayCountConventionResources.Thirty360LongName);

        /// <summary>The annotation, i.e. description of the day count convention.
        /// </summary>
        private static readonly string sm_Annotation = DayCountConventionResources.Thirty360;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="Thirty360"/> class.
        /// </summary>
        public Thirty360()
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
            get { return Thirty360.sm_Name; }
        }

        /// <summary>Gets the long name of the day count convention.
        /// </summary>
        /// <value>The language dependent long name of the day count convention.</value>
        public IdentifierString LongName
        {
            get { return Thirty360.sm_LongName; }
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
            get { return Thirty360.sm_Annotation; }
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
            return GetDayDifference(startDate, endDate) / 360.0;
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
            dayDifference = GetDayDifference(startDate, endDate);
            return dayDifference / 360.0;
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

        /// <summary>Gets the number of days between two dates.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>The number of days between <paramref name="startDate"/> and <paramref name="endDate"/>.
        /// </returns>
        private int GetDayDifference(DateTime startDate, DateTime endDate)
        {
            int d1 = startDate.Day;
            int m1 = startDate.Month;
            int y1 = startDate.Year;
            int d2 = endDate.Day;
            int m2 = endDate.Month;
            int y2 = endDate.Year;

            if ((d2 == 31) && (d1 < 30))  /* add one day (this month has 31 days), thus go to the next month */
            {
                d2 = 1;
                m2++;
            }
            return Math.Max(0, 30 - d1) + Math.Min(30, d2) + 360 * (y2 - y1) + 30 * (m2 - m1 - 1);
        }
        #endregion
    }
}