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
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Dodoni.Finance;
using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Logging;
using Dodoni.BasicComponents.Containers;
using Microsoft.Extensions.Logging;

namespace Dodoni.Finance.DateFactory
{
    /// <summary>Serves as basis class for date schedules. The <see cref="System.DateTime"/> objects are perhaps not business days (for example in the case of the 'end of month' business day convention.
    /// </summary>
    public class DateSchedule : IEnumerable<DateTime>, IAnnotatable, IIdentifierNameable, IInfoOutputQueriable // , ILoggedObject
    {
        #region private members

        /// <summary>The date schedule itself.
        /// </summary>
        private SortedSet<DateTime> m_DateScheduleSet;

        /// <summary>The logger.
        /// </summary>
        private ILogger m_Logger;

        /// <summary>The name of the date schedule.
        /// </summary>
        private IdentifierString m_Name;

        /// <summary>The (language dependent) long name of the date schedule, i.e. 'payment dates', 'fixing dates' etc.
        /// </summary>
        private IdentifierString m_LongName;

        /// <summary>The annotation, i.e. description of the date schedule.
        /// </summary>
        private string m_Annotation;
        #endregion

        #region public (readonly) members

        /// <summary>The holiday calendar; you can add non-business days as well.
        /// </summary>
        public readonly IHolidayCalendar HolidayCalendar;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="DateSchedule"/> class.
        /// </summary>
        /// <param name="holidayCalendar">The holiday calendar.</param>
        /// <param name="name">The name of the date schedule.</param>
        /// <param name="longName">The (perhaps language dependent) long name of the date schedule.</param>
        /// <param name="logger">A logger; if <c>null</c> a new <see cref="ILogger"/> object will be created.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="holidayCalendar"/> is <c>null</c>.</exception>
        public DateSchedule(IHolidayCalendar holidayCalendar, IdentifierString name = null, IdentifierString longName = null, ILogger logger = null)
        {
            if (holidayCalendar == null)
            {
                throw new ArgumentNullException("holidayCalendar");
            }
            HolidayCalendar = holidayCalendar;
            Initialize(name, longName, logger);
            m_DateScheduleSet = new SortedSet<DateTime>();
        }

        /// <summary>Initializes a new instance of the <see cref="DateSchedule"/> class.
        /// </summary>
        /// <param name="holidayCalendar">The holiday calendar.</param>
        /// <param name="dateSchedule">A initial list of dates.</param>
        /// <param name="name">The name of the date schedule.</param>
        /// <param name="longName">The (perhaps language dependent) long name of the date schedule.</param>
        /// <param name="logger">A logger; if <c>null</c> a new <see cref="ILogger"/> object will be created.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="holidayCalendar"/> is <c>null</c>.</exception>
        public DateSchedule(IHolidayCalendar holidayCalendar, IEnumerable<DateTime> dateSchedule, IdentifierString name = null, IdentifierString longName = null, ILogger logger = null)
        {
            if (holidayCalendar == null)
            {
                throw new ArgumentNullException("holidayCalendar");
            }
            HolidayCalendar = holidayCalendar;
            Initialize(name, longName, logger);
            m_DateScheduleSet = new SortedSet<DateTime>(dateSchedule);
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the date schedule.
        /// </summary>
        /// <value>The name of the date schedule.</value>
        public IdentifierString Name
        {
            get { return m_Name; }
        }

        /// <summary>Gets the long name of the date schedule.
        /// </summary>
        /// <value>The language dependent long name of the date schedule.</value>
        public IdentifierString LongName
        {
            get { return m_LongName; }
        }
        #endregion

        #region IAnnotatable Members

        /// <summary>Gets a value indicating whether the annotation is readonly.
        /// </summary>
        /// <value><c>true</c> if the annotation of this instance is readonly; otherwise, <c>false</c>.</value>
        bool IAnnotatable.HasReadOnlyAnnotation
        {
            get { return false; }
        }

        /// <summary>Gets the annotation of the current instance.
        /// </summary>
        /// <value>The annotation of the current instance.</value>
        public string Annotation
        {
            get { return m_Annotation; }
        }
        #endregion

        #region IInfoOutputQueriable Members

        /// <summary>Gets the info-output level of detail.
        /// </summary>
        /// <value>The info-output level of detail.</value>
        InfoOutputDetailLevel IInfoOutputQueriable.InfoOutputDetailLevel
        {
            get { return InfoOutputDetailLevel.Full; }
        }
        #endregion

        #region ILoggedObject Members

        /// <summary>Gets the logger of the current instance in its <see cref="ILogger" /> representation.
        /// </summary>
        /// <value>The logger of the current instance in its <see cref="ILogger" /> representation.</value>
        public ILogger Logging
        {
            get { return m_Logger; }
        }
        #endregion

        /// <summary>Gets the number of elements of the date schedule.
        /// </summary>
        /// <value>The number of elements of the date schedule.</value>
        public int Count
        {
            get { return m_DateScheduleSet.Count; }
        }

        /// <summary>Gets the <see cref="System.DateTime"/> at the specified null-based index.
        /// </summary>
        /// <value>The date of the date schedule at the specific index.</value>
        public DateTime this[int index]
        {
            get { return m_DateScheduleSet.ElementAt(index); }
        }

        /// <summary>The first date of the date schedule.
        /// </summary>
        /// <remarks>The first date.</remarks>
        public DateTime FirstDate
        {
            get { return m_DateScheduleSet.ElementAt(0); }
        }

        /// <summary>The last date of the date schedule.
        /// </summary>
        /// <remarks>The last date.</remarks>
        public DateTime LastDate
        {
            get { return m_DateScheduleSet.Last(); }
        }
        #endregion

        #region public methods

        #region IAnnotatable Members

        /// <summary>Sets the annotation of the current instance.
        /// </summary>
        /// <param name="annotation">The annotation.</param>
        /// <returns>A value indicating whether the <see cref="Annotation"/> has been changed.</returns>
        public bool TrySetAnnotation(string annotation)
        {
            m_Annotation = annotation;
            return true;
        }
        #endregion

        #region IInfoOutputQueriable Members

        /// <summary>Gets informations of the current object as a specific <see cref="InfoOutput"/> instance.
        /// </summary>
        /// <param name="infoOutput">The <see cref="InfoOutput"/> object which is to be filled with informations concering the current instance.</param>
        /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category name.</param>
        /// <remarks>This implementation returns a <see cref="System.Data.DataTable"/> objects which contains the <see cref="System.DateTime"/> objects.</remarks>
        void IInfoOutputQueriable.FillInfoOutput(InfoOutput infoOutput, string categoryName)
        {
            var dateTable = new System.Data.DataTable("Dates");

            dateTable.Columns.Add("Value", typeof(DateTime));
            foreach (DateTime date in m_DateScheduleSet)
            {
                dateTable.Rows.Add(date);
            }
            InfoOutputPackage infoOutputCollection = infoOutput.AcquirePackage(categoryName);
            infoOutputCollection.Add(dateTable);

            infoOutputCollection.Add("Holiday calendar", HolidayCalendar.Name.String);
        }

        /// <summary>Sets the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel"/> property.
        /// </summary>
        /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
        /// <returns>A value indicating whether the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel"/> has been changed
        /// with respect to <paramref name="infoOutputDetailLevel"/>.
        /// </returns>
        bool IInfoOutputQueriable.TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
        {
            return (infoOutputDetailLevel == InfoOutputDetailLevel.Full);
        }
        #endregion

        #region IEnumerable<DateTime> Members

        /// <summary>Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.</returns>
        public IEnumerator<DateTime> GetEnumerator()
        {
            return m_DateScheduleSet.GetEnumerator();
        }
        #endregion

        #region IEnumerable Members

        /// <summary>Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_DateScheduleSet.GetEnumerator();
        }
        #endregion

        /// <summary>Determines whether the <see cref="DateSchedule"/> contains a specified date.
        /// </summary>
        /// <param name="date">The date to locate in the <see cref="DateSchedule"/>.</param>
        /// <returns><c>true</c> if the current instance contains <paramref name="date"/>; otherwise, <c>false</c>.</returns>
        public bool Contains(DateTime date)
        {
            return m_DateScheduleSet.Contains(date);
        }

        /// <summary>Returns a read-only wrapper for the current date schedule.
        /// </summary>
        /// <returns>A readonly-wrapper for the current date schedule.</returns>
        public ReadOnlyDateSchedule AsReadOnly()
        {
            lock (m_DateScheduleSet)
            {
                DateTime[] list = new DateTime[m_DateScheduleSet.Count];
                m_DateScheduleSet.CopyTo(list);
                return new ReadOnlyDateSchedule(list, m_Annotation);
            }
        }

        /// <summary>Adds <see cref="System.DateTime"/> objects which are given via some date schedule rule.
        /// </summary>
        /// <param name="dateScheduleRule">The date schedule rule.</param>
        public void Add(IDateScheduleRule dateScheduleRule)
        {
            if (dateScheduleRule == null)
            {
                throw new ArgumentNullException("dateScheduleRule");
            }
            if (dateScheduleRule.IsOperable == false)
            {
                throw new NotOperableException("dateScheduleRule");
            }
            dateScheduleRule.AddDatesToDateSchedule(this);
        }

        /// <summary>Adds a specified date to the date schedule.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="businessDayConvention">The business day convention.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="businessDayConvention"/> is <c>null</c>.</exception>
        /// <remarks>The adjusted date with respect to the parameters will be added to the date schedule.</remarks>
        public void Add(DateTime date, IBusinessDayConvention businessDayConvention)
        {
            if (businessDayConvention == null)
            {
                throw new ArgumentNullException("businessDayConvention");
            }
            date = businessDayConvention.GetAdjustedDate(date, HolidayCalendar);
            m_DateScheduleSet.Add(date);
            AddNotABusinessDayLogFileMsg(date);
        }

        /// <summary>Adds a specified date to the date schedule.
        /// </summary>
        /// <param name="date">The date to add.</param>
        /// <remarks>A warning will be written into the log-file [if desired] while reinitializing the 
        /// current instance, if <paramref name="date"/> is not a business day.</remarks>
        public void Add(DateTime date)
        {
            m_DateScheduleSet.Add(date);
            AddNotABusinessDayLogFileMsg(date);
        }

        /// <summary>Adds the elements of a specified collection of <see cref="System.DateTime"/> objects.
        /// </summary>
        /// <param name="dates">The dates to add.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="dates"/> is <c>null</c>.</exception>
        public void AddRange(IEnumerable<DateTime> dates)
        {
            if (dates == null)
            {
                throw new ArgumentNullException("dates");
            }
            foreach (DateTime date in dates)
            {
                m_DateScheduleSet.Add(date);
                AddNotABusinessDayLogFileMsg(date);
            }
        }

        /// <summary>Removes all elements from the date schedule.
        /// </summary>
        public void Clear()
        {
            m_DateScheduleSet.Clear();
         //   m_Logger.Add_Info_ObjectReseted();
        }

        /// <summary>Removes a specified date.
        /// </summary>
        /// <param name="date">The date to remove.</param>
        public void Remove(DateTime date)
        {
            m_DateScheduleSet.Remove(date);
        }

        /// <summary>Removes a specified date.
        /// </summary>
        /// <param name="dateIndex">The null-based index of the date to remove.</param>
        public void RemoveAt(int dateIndex)
        {
            var dateAtDateIndex = m_DateScheduleSet.ElementAt(dateIndex).Date;
            m_DateScheduleSet.RemoveWhere(dateTime => dateTime.Date == dateAtDateIndex);
        }
        #endregion

        #region protected methods

        /// <summary>Adds a warning message if the specific <see cref="System.DateTime"/> is not a business day.
        /// </summary>
        /// <param name="date">The date.</param>
        protected void AddNotABusinessDayLogFileMsg(DateTime date)
        {
            if (HolidayCalendar.IsBusinessDay(date) == false)
            {
               // m_Logger.Add_Warning_NoBusinessDay(date.ToShortDateString() + " [holiday calendar ' " + HolidayCalendar.LongName + "']");
            }
        }
        #endregion

        #region private methods

        /// <summary>Initializes the current instance.
        /// </summary>
        /// <param name="name">The name of the date schedule.</param>
        /// <param name="longName">The (perhaps language dependent) long name of the date schedule.</param>
        /// <param name="logger">A logger; if <c>null</c> a new <see cref="ILogger"/> object will be created.</param>
        /// <remarks>This method does not initialize the internal set of <see cref="System.DateTime"/> objects.</remarks>
        private void Initialize(IdentifierString name, IdentifierString longName, ILogger logger)
        {
            if ((name == null) || (name.String == null))
            {
                m_Name = IdentifierString.Empty;
//                m_Logger = (logger != null) ? logger : Dodoni.BasicComponents.Logging.Logger.Stream.Create("Date schedule", typeof(DateSchedule), channel: "Date schedule");
                m_LongName = (longName != null) ? longName : IdentifierString.Empty;
            }
            else
            {
                m_LongName = (longName != null) ? longName : name;
//                m_Logger = (logger != null) ? logger : Dodoni.BasicComponents.Logging.Logger.Stream.Create("Date schedule", typeof(DateSchedule), channel: "Date schedule");
            }
            m_Name = (name != null) ? name : IdentifierString.Empty;
        }
        #endregion
    }
}