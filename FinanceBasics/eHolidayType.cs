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

using Dodoni.BasicComponents.Utilities;

namespace Dodoni.Finance
{
    /// <summary>Represents the holiday type.
    /// </summary>
    [Flags]
    public enum HolidayType
    {
        /// <summary>No (public) holidays and no weekends are considered, i.e. each day is a business day.
        /// </summary>
        None = 0,

        /// <summary>Consider weekends only.
        /// </summary>
        [String("Weekends")]
        Weekends = 1,

        /// <summary>Consider public holidays only.
        /// </summary>
        [String("Public holidays")]
        PublicHolidays = 2,

        /// <summary>Consider weekends and public holidays. 
        /// </summary>
        [String("Public holidays, weekends")]
        WeekendsAndPublicHolidays = Weekends | PublicHolidays
    }
}