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
using Dodoni.BasicComponents.Utilities;

namespace Dodoni.Finance.CommonMarketUsages.HolidayCalendars
{
    /// <summary>Serves as interface for a single holiday.
    /// </summary>
    public interface IHoliday : IIdentifierNameable
    {
        /// <summary>Gets a value indicating whether the holiday moves if the holiday agrees with a weekend day.
        /// </summary>
        /// <value>The holiday rolling type.</value>
        HolidayRollingType HolidayRollingType { get; }

        /// <summary>Gets the holiday with respect to a specific <paramref name="year"/> in its <see cref="System.DateTime"/> representation.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="value">The holiday with respect to the <paramref name="year"/> in its <see cref="DateTime"/> representation (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        bool TryGetValue(int year, out DateTime value);

        /// <summary>Gets the holiday with respect to a specific <paramref name="year"/> in its <see cref="DateInfo"/> representation.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="value">The holiday with respect to the year <paramref name="year"/> in its <see cref="DateInfo"/> representation (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        bool TryGetValue(int year, out DateInfo value);
    }
}