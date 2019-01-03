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
using Dodoni.BasicComponents;

namespace Dodoni.Finance.DateFactory
{
    /// <summary>Represents the daily date roll frequency, i.e. each business day in a specified time interval.
    /// </summary>
    internal class Daily : IDateScheduleFrequency
    {
        #region private static readonly members

        /// <summary>The name of the frequency.
        /// </summary>
        private static readonly IdentifierString sm_Name = new IdentifierString("Daily");

        /// <summary>The long name of the frequency, i.e. language dependent.
        /// </summary>
        private static readonly IdentifierString sm_LongName = new IdentifierString(DateFactoryResources.DailyLongName);

        /// <summary>The annotation, i.e. description of the frequency.
        /// </summary>
        private static readonly string sm_Annotation = DateFactoryResources.Daily;

        /// <summary>The date schedule frequency in its <see cref="TenorTimeSpan"/> representation.
        /// </summary>
        private static readonly TenorTimeSpan sm_FrequencyTenor = new TenorTimeSpan(0, 0, 1);
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="Daily"/> class.
        /// </summary>
        public Daily()
        {
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the date schedule frequency.
        /// </summary>
        /// <value>The name of the date schedule frequency.</value>
        public IdentifierString Name
        {
            get { return Daily.sm_Name; }
        }

        /// <summary>Gets the long name of the date schedule frequency.
        /// </summary>
        /// <value>The language dependent long name of the date schedule frequency.</value>
        public IdentifierString LongName
        {
            get { return Daily.sm_LongName; }
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
            get { return Daily.sm_Annotation; }
        }
        #endregion

        #endregion

        #region public methods

        #region IDateScheduleFrequency Members

        /// <summary>Gets the date schedule frequency, i.e. the frequency in some <see cref="TenorTimeSpan"/> representation.
        /// </summary>
        /// <returns>The frequency in its <see cref="TenorTimeSpan"/> representation.</returns>
        public TenorTimeSpan GetFrequencyTenor()
        {
            return Daily.sm_FrequencyTenor;
        }
        #endregion

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

        /// <summary>Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return (string)Daily.sm_LongName;
        }
        #endregion
    }
}