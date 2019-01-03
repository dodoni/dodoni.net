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

using Dodoni.BasicComponents;

namespace Dodoni.Finance.FixingLags
{
    /// <summary>Represents the 'None' fixing lag implementation.
    /// </summary>
    internal class NoneFixingLag : IFixingLag
    {
        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="NoneFixingLag" /> class.
        /// </summary>
        internal NoneFixingLag()
        {
            Name = new IdentifierString("None");
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the current instance.
        /// </summary>
        /// <value>The language independent name of the current instance.</value>
        public IdentifierString Name
        {
            get;
            private set;
        }

        /// <summary>Gets the long name of the current instance.
        /// </summary>
        /// <value>The (perhaps) language dependent long name of the current instance.
        /// </value>
        public IdentifierString LongName
        {
            get { return Name; }
        }
        #endregion

        #endregion

        #region public methods

        #region IFixingLag Members

        /// <summary>Gets a specific fixing date.
        /// </summary>
        /// <param name="periodStartDate">The start date of the (interest) period.</param>
        /// <param name="holidayCalendar">The (settlement) calendar.</param>
        /// <returns>The specific fixing date.</returns>
        public DateTime GetFixingDate(DateTime periodStartDate, IHolidayCalendar holidayCalendar)
        {
            return periodStartDate;
        }
        #endregion

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return Name.String;
        }
        #endregion
    }
}