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
using System.Collections.Generic;

using Dodoni.BasicComponents.Utilities;
using Dodoni.BasicComponents;

namespace Dodoni.Finance.CommonMarketUsages.HolidayCalendars
{
    /// <summary>Serves as attribute for <see cref="SingleHoliday"/> which represents a holiday which is given
    /// with respect to a single <see cref="System.DateTime"/> instance only.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public sealed class SingleHolidayAttribute : HolidayAttribute
    {
        #region public (readonly) / private members

        /// <summary>The property name with respect to a specific resource file that contains a language dependend <see cref="System.String"/> representation.
        /// </summary>
        public readonly string ResourcePropertyName;

        /// <summary>The <see cref="System.DateTime"/> representation of the holiday.
        /// </summary>
        public readonly DateTime DateOfHoliday;

        /// <summary>The name of the holiday.
        /// </summary>
        private string m_Name;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="SingleHolidayAttribute"/> class.
        /// </summary>
        /// <param name="resourcePropertyName">The property name of the language dependend <see cref="System.String"/> representation of the holiday with respect 
        /// to a resource file that is represented by an instance of <see cref="LanguageResourceAttribute"/>.</param>
        /// <param name="day">The day of the single holiday.</param>
        /// <param name="month">The month of thesingle holiday.</param>
        /// <param name="year">The year of the single holiday.</param>
        public SingleHolidayAttribute(string resourcePropertyName, int day, Month month, int year)
        {
            ResourcePropertyName = resourcePropertyName;
            m_Name = resourcePropertyName;
            DateOfHoliday = new DateTime(year, (int)month, day);
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
            return new SingleHoliday(m_Name.ToIdentifierString(), DateOfHoliday, holidayLongName);
        }
        #endregion
    }
}