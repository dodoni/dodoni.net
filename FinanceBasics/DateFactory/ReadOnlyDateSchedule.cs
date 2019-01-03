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
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Dodoni.BasicComponents;

namespace Dodoni.Finance.DateFactory
{
    /// <summary>Represents a readonly date schedule, i.e. payment dates, fixing/reset dates etc.
    /// </summary>
    public class ReadOnlyDateSchedule : ReadOnlyCollection<DateTime>, IAnnotatable
    {
        #region public readonly members

        /// <summary>The first date of the date schedule.
        /// </summary>
        public readonly DateTime FirstDate;

        /// <summary>The last date of the date schedule.
        /// </summary>
        public readonly DateTime LastDate;
        #endregion

        #region private members

        /// <summary>The annotation, i.e. description of the date schedule.
        /// </summary>
        private string m_Annotation;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="ReadOnlyDateSchedule"/> class.
        /// </summary>
        /// <param name="sortedListOfDates">The sorted list of dates.</param>
        internal ReadOnlyDateSchedule(IList<DateTime> sortedListOfDates)
            : base(sortedListOfDates)
        {
            if (sortedListOfDates.Count > 0)
            {
                FirstDate = sortedListOfDates[0];
                LastDate = sortedListOfDates[sortedListOfDates.Count - 1];
            }
        }

        /// <summary>Initializes a new instance of the <see cref="ReadOnlyDateSchedule"/> class.
        /// </summary>
        /// <param name="sortedListOfDates">The sorted list of dates.</param>
        /// <param name="annotation">The annotation.</param>
        internal ReadOnlyDateSchedule(IList<DateTime> sortedListOfDates, string annotation)
            : this(sortedListOfDates)
        {
            m_Annotation = (annotation != null) ? annotation : String.Empty;
        }
        #endregion

        #region public properties

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
            get { return m_Annotation; }
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
        public bool TrySetAnnotation(string annotation)
        {
            return false;
        }
        #endregion

        #endregion
    }
}