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

namespace Dodoni.Finance.CommonMarketUsages.BusinessDayConventions
{
    /// <summary>The business day convention 'End of Quarter Following', i.e. dates are adjusted to land on the last day of the quarter, i.e. 31.03, 30.06, 30.09 or 31.12.
    /// If the date falls on a weekend or holiday, the following good business day is used.
    /// </summary>
    internal class EndOfQuarterFollowingAdjustment : IBusinessDayConvention
    {
        #region private static readonly members

        /// <summary>The name of the business day convention.
        /// </summary>
        private static readonly IdentifierString sm_Name = new IdentifierString("End of Quarter Following");

        /// <summary>The annotation, i.e. description of the business day convention.
        /// </summary>
        private static readonly string sm_Annotation = BusinessDayConventionResources.EndOfQuarterFollowing;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="EndOfQuarterFollowingAdjustment"/> class.
        /// </summary>
        public EndOfQuarterFollowingAdjustment()
        {
        }
        #endregion

        #region public properties

        #region IBusinessDayConvention Members

        /// <summary>Gets the type of the adjustment, i.e. a value indicating whether the result of <see cref="IBusinessDayConvention.GetAdjustedDate(DateTime, IHolidayCalendar)"/> is a business day.
        /// </summary>
        /// <value>The type of the date adjustment.</value>
        public BusinessDayAdjustmentType AdjustmentType
        {
            get { return BusinessDayAdjustmentType.AdjustmentToBusinessDay; }
        }
        #endregion

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the business day convention.
        /// </summary>
        /// <value>The name of the business day convention.</value>
        public IdentifierString Name
        {
            get { return EndOfQuarterFollowingAdjustment.sm_Name; }
        }

        /// <summary>Gets the long name of the business day convention.
        /// </summary>
        /// <value>The language dependent long name of the business day convention.</value>
        public IdentifierString LongName
        {
            get { return EndOfQuarterFollowingAdjustment.sm_Name; }
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
            get { return EndOfQuarterFollowingAdjustment.sm_Annotation; }
        }
        #endregion

        #endregion

        #region public methods

        #region IAnnotatable Members

        /// <summary>Sets the annotation of the current instance.
        /// </summary>
        /// <param name="annotation">The annotation.</param>
        /// <returns>
        /// A value indicating whether the <see cref="Annotation"/> has been changed.
        /// </returns>
        bool IAnnotatable.TrySetAnnotation(string annotation)
        {
            return false;
        }
        #endregion

        #region IBusinessDayConvention Members

        /// <summary>Gets an adjusted date with respect to a specific <see cref="System.DateTime"/> object.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="holidayCalendar">The holiday calendar.</param>
        /// <returns>The <see cref="System.DateTime"/> object that is given by <paramref name="date"/> taken into account the business day
        /// convention represented by the current instance.
        /// </returns>
        /// <remarks>Perhaps the return value is not a business day, for example in the case of some end-of-month adjustment or 'no adjustment'.</remarks>
        public DateTime GetAdjustedDate(DateTime date, IHolidayCalendar holidayCalendar)
        {
            switch (date.Month)
            {
                case 1:
                case 2:
                case 3:
                    var endOfQ1Date = new DateTime(date.Year, 3, 31);
                    return holidayCalendar.GetForwardAdjustedBusinessDay(endOfQ1Date);
                case 4:
                case 5:
                case 6:
                    var endOfQ2Date = new DateTime(date.Year, 6, 30);
                    return holidayCalendar.GetForwardAdjustedBusinessDay(endOfQ2Date);
                case 7:
                case 8:
                case 9:
                    var endOfQ3Date = new DateTime(date.Year, 9, 30);
                    return holidayCalendar.GetForwardAdjustedBusinessDay(endOfQ3Date);
                default:
                    var endOfQ4Date = new DateTime(date.Year, 12, 31);
                    return holidayCalendar.GetForwardAdjustedBusinessDay(endOfQ4Date);
            }
        }
        #endregion

        /// <summary>Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return sm_Name.String;
        }
        #endregion
    }
}