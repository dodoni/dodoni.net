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

using Dodoni.BasicComponents;

namespace Dodoni.Finance.DateFactory
{
    /// <summary>Represents an individual date schedule frequency.
    /// </summary>
    internal class IndividualDateScheduleFrequency : IDateScheduleFrequency
    {
        #region private members

        /// <summary>The date schedule frequency in its <see cref="TenorTimeSpan"/> representation.
        /// </summary>
        private TenorTimeSpan m_FrequencyTenor;

        /// <summary>The name of the frequency.
        /// </summary>
        private IdentifierString m_Name;

        /// <summary>The long name of the frequency, i.e. perhaps language dependent.
        /// </summary>
        private IdentifierString m_LongName;

        /// <summary>The annotation, i.e. description of the frequency.
        /// </summary>
        private string m_Annotation = DateFactoryResources.Individual;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="IndividualDateScheduleFrequency"/> class.
        /// </summary>
        /// <param name="frequencyTenor">The frequency tenor.</param>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="frequencyTenor"/> represents a 
        /// tenor with a negative sign or the null-tenor.</exception>
        /// <remarks>The <see cref="Name"/> will be set to the <see cref="System.String"/> representation of
        /// <paramref name="frequencyTenor"/>.</remarks>
        public IndividualDateScheduleFrequency(TenorTimeSpan frequencyTenor)
        {
            if (TenorTimeSpan.IsNull(frequencyTenor) || (frequencyTenor.IsPositive == false))
            {
                throw new ArgumentException("frequencyTenor");
            }
            m_FrequencyTenor = frequencyTenor;
            string frequencyTenorAsString = frequencyTenor.ToString();

            m_Name = new IdentifierString(frequencyTenorAsString);
            m_LongName = new IdentifierString(String.Format(DateFactoryResources.IndividualLongName, frequencyTenorAsString));
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the date schedule frequency.
        /// </summary>
        /// <value>The name of the date schedule frequency.</value>
        public IdentifierString Name
        {
            get { return m_Name; }
        }

        /// <summary>Gets the long name of the date schedule frequency.
        /// </summary>
        /// <value>The language dependent long name of the date schedule frequency.</value>
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

        #endregion

        #region public methods

        #region IDateScheduleFrequency Members

        /// <summary>Gets the date schedule frequency, i.e. the frequency in some <see cref="TenorTimeSpan"/> representation.
        /// </summary>
        /// <returns>The frequency in its <see cref="TenorTimeSpan"/> representation.
        /// </returns>
        public TenorTimeSpan GetFrequencyTenor()
        {
             return m_FrequencyTenor;
        }
        #endregion

        #region IAnnotatable Members

        /// <summary>Sets the annotation of the current instance.
        /// </summary>
        /// <param name="annotation">The annotation.</param>
        /// <returns>
        /// A value indicating whether the <see cref="Annotation"/> has been changed.
        /// </returns>
        public bool TrySetAnnotation(string annotation)
        {
            m_Annotation = annotation;
            return true;
        }
        #endregion

        /// <summary>Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return (string) m_LongName;
        }
        #endregion

        #region internal static methods

        /// <summary>Gets a <see cref="IndividualDateScheduleFrequency"/> object.
        /// </summary>
        /// <param name="frequencyTenor">The frequency tenor.</param>
        /// <param name="individualDateScheduleFrequency">The individual date schedule frequency (output).</param>
        /// <returns>A value indicating whether <paramref name="individualDateScheduleFrequency"/> contains valid data.</returns>
        internal static bool TryCreate(TenorTimeSpan frequencyTenor, out IDateScheduleFrequency individualDateScheduleFrequency)
        {
            if (TenorTimeSpan.IsNull(frequencyTenor) || (frequencyTenor.IsPositive == false))
            {
                individualDateScheduleFrequency = null;
                return false;
            }
            individualDateScheduleFrequency = new IndividualDateScheduleFrequency(frequencyTenor);
            return true;
        }
        #endregion
    }
}