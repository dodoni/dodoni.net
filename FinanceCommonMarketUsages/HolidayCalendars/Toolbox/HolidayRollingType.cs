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

namespace Dodoni.Finance.CommonMarketUsages.HolidayCalendars
{
    /// <summary>Represents whether a holiday date does not change even it is a saturday or sunday,
    /// or the holiday rolles to the next (previous) non-holiday.
    /// </summary>
    /// <remarks>One has to be carfully, especially if the 26.12. is a sunday, in this case the holiday may 
    /// moves to the next tuesday (28.12), thus in this case it is not the next non-weekend day.</remarks>
    public enum HolidayRollingType
    {
        /// <summary>If the public holiday is a Saturday or a Sunday, don't move the public holiday.
        /// </summary>
        NoRolling,

        /// <summary>If the public holiday is a Saturday or a Sunday, move it to the next non-public holiday (in general monday).
        /// </summary>
        ForwardRolling,

        /// <summary>If the public holiday is a Saturday or a Sunday, move it to the last non-public holiday
        /// (in general Friday) the week before.
        /// </summary>
        BackwardRolling,

        /// <summary>If the public holiday is a Sunday, move to Monday; move to Friday if a Saturday is given.
        /// </summary>
        SundayToMondaySaturdayToFriday
    }
}