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
using System.Resources;

using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Utilities;

namespace Dodoni.Finance.CommonMarketUsages.HolidayCalendars
{
    /// <summary>Serves as attribute for <see cref="EasterDependingHoliday"/> that represents an easter depending holiday.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public sealed class EasterDependingHolidayAttribute : HolidayAttribute
    {
        #region private members

        /// <summary>The offset (in days) with respect to easter monday.
        /// </summary>
        private int m_OffsetToEasterMonday;

        /// <summary>The first year where this holiday occurs, i.e. the optional start year of the holiday.
        /// </summary>
        /// <remarks><see cref="Int32.MinValue"/> is standard, i.e. in each year the represented holiday occurs.</remarks>
        private int m_FirstYear;

        /// <summary>The last year where this holiday occurs, i.e. the optional end year of the holiday.
        /// </summary>
        /// <remarks><see cref="Int32.MaxValue"/> is standard, i.e. in each year the represented holiday occurs.</remarks>
        private int m_LastYear;

        /// <summary>A value indicating whether the holiday moves if the holiday agrees with a weekend day.
        /// </summary>
        private HolidayRollingType m_HolidayRollingType;

        /// <summary>The name of the holiday.
        /// </summary>
        private string m_Name;
        #endregion

        #region public (readonly) members

        /// <summary>The property name with respect to a specific resource file that contains a language dependend <see cref="System.String"/> representation.
        /// </summary>
        public readonly string ResourcePropertyName;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="EasterDependingHolidayAttribute"/> class.
        /// </summary>
        /// <param name="resourcePropertyName">The property name of the language dependend <see cref="System.String"/> representation of the holiday with respect 
        /// to a resource file that is represented by an instance of <see cref="LanguageResourceAttribute"/>.</param>
        /// <param name="offsetToEasterMonday">The offset to easter monday.</param>
        /// <param name="holidayRollingType">A value indicating whether the holiday moves if the holiday agrees with a weekend day.</param>
        public EasterDependingHolidayAttribute(string resourcePropertyName, int offsetToEasterMonday, HolidayRollingType holidayRollingType = HolidayRollingType.NoRolling)
        {
            m_Name = resourcePropertyName;
            ResourcePropertyName = resourcePropertyName;

            m_OffsetToEasterMonday = offsetToEasterMonday;
            m_FirstYear = Int32.MinValue;
            m_LastYear = Int32.MaxValue;
            m_HolidayRollingType = holidayRollingType;
        }
        #endregion

        #region public properties

        /// <summary>Gets or sets the (language independent) name of the holiday.
        /// </summary>
        /// <value>The name of the holiday.</value>
        /// <remarks>The name will be set to the <see cref="ResourcePropertyName"/> by default.</remarks>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        /// <summary>Gets or sets the first year to take into account the holiday (if desired).
        /// </summary>
        /// <value>The first year.</value>
        public int FirstYear
        {
            get { return m_FirstYear; }
            set { m_FirstYear = value; }
        }

        /// <summary>Gets or sets the last year to take into account the holiday (if desired).
        /// </summary>
        /// <value>The last year.</value>
        public int LastYear
        {
            get { return m_LastYear; }
            set { m_LastYear = value; }
        }

        /// <summary>Gets or sets a value indicating whether the holiday will be rolled if the holiday is a weekend day.
        /// </summary>
        /// <value>The holiday rolling type.</value>
        public HolidayRollingType HolidayRollingType
        {
            get { return m_HolidayRollingType; }
            set { m_HolidayRollingType = value; }
        }
        #endregion

        #region public methods

        /// <summary>Gets the holiday in its <see cref="IHoliday"/> representation.
        /// </summary>
        /// <param name="resourceManager">The <see cref="ResourceManager"/> object that contains the resource with respect to the holiday, i.e. the
        /// language depending <see cref="System.String"/> representations.</param>
        /// <returns>An instance of <see cref="IHoliday"/> that represents the holiday.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="resourceManager"/> is not valid for the current instance, i.e. no resources are available for the holiday.</exception>
        public override IHoliday GetHoliday(ResourceManager resourceManager)
        {
            IdentifierString holidayLongName = IdentifierString.Create(resourceManager, ResourcePropertyName);
            return new EasterDependingHoliday(m_Name.ToIdentifierString(), m_OffsetToEasterMonday, m_FirstYear, m_LastYear, m_HolidayRollingType, holidayLongName);
        }
        #endregion
    }
}