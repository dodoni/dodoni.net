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

using Dodoni.Finance;
using Dodoni.BasicComponents.Utilities;

namespace Dodoni.Finance.CommonMarketUsages.HolidayCalendars.Europe
{
    /// <summary>Represents the list of holidays taken into account for the holiday calendar 'FFM:Eurex', i.e.
    /// Frankfurt/Main, Germany.
    /// </summary>
    [CustomHolidayCalendar("FFM:EUREX", HolidayCalendarRegion.Europe, "Dodoni.Finance.CommonMarketUsages.HolidayCalendars.HolidayNameResources", Description = "_FFM_Eurex")]
    internal enum FFM_Eurex
    {
        /// <summary>'New year' public holiday.
        /// </summary>
        /// <remarks>Represents the first January of each year. The holiday rolling type is set to
        /// <see cref="HolidayRollingType.NoRolling"/>.</remarks>
        [FixHoliday("NewYear", 1, Month.January, HolidayRollingType = HolidayRollingType.NoRolling)]
        NewYearsDay,

        /// <summary>The 'good friday'.
        /// </summary>
        /// <remarks>Represents good friday, the holiday rolling type is set to
        /// <see cref="HolidayRollingType.NoRolling"/>.</remarks>
        [EasterDependingHoliday("GoodFriday", -3, HolidayRollingType = HolidayRollingType.NoRolling)]
        GoodFriday,

        /// <summary>The 'easter monday'.
        /// </summary>
        /// <remarks>Represents easter monday, the holiday rolling type is set to
        /// <see cref="HolidayRollingType.NoRolling"/>.</remarks>
        [EasterDependingHoliday("EasterMonday", 0, HolidayRollingType = HolidayRollingType.NoRolling)]
        EasterMonday,

        /// <summary>The 'labour day'.
        /// </summary>
        /// <remarks>Represents the Laour day, i.e. 1st of may. The holiday rolling type is set to
        /// <see cref="HolidayRollingType.NoRolling"/>.</remarks>
        [FixHoliday("LabourDay", 1, Month.May, HolidayRollingType = HolidayRollingType.NoRolling)]
        LabourDay,

        /// <summary>The 'christmas eve'.
        /// </summary>
        /// <remarks>Represents christmas eve, i.e. 24st december. The holiday rolling type is set to
        /// <see cref="HolidayRollingType.NoRolling"/>.</remarks>
        [FixHoliday("ChristmasEve", 24, Month.December, HolidayRollingType = HolidayRollingType.NoRolling)]
        ChristmasEve,

        /// <summary>The 'christmas day'.
        /// </summary>
        /// <remarks>Represents christmas day, i.e. 25st december. The holiday rolling type is set to
        /// <see cref="HolidayRollingType.NoRolling"/>.</remarks>
        [FixHoliday("ChristmasDay", 25, Month.December, HolidayRollingType = HolidayRollingType.NoRolling)]
        ChristmasDay,

        /// <summary>The 'boxing day'.
        /// </summary>
        /// <remarks>Represents boxing date, i.e. 26st december. The holiday rolling type is set to
        /// <see cref="HolidayRollingType.NoRolling"/>.</remarks>
        [FixHoliday("BoxingDay", 26, Month.December, HolidayRollingType = HolidayRollingType.NoRolling)]
        BoxingDay,

        /// <summary>The 'new year eve'.
        /// </summary>
        /// <remarks>Represents new years eve, i.e. 31st december. The holiday rolling type is set to <see cref="HolidayRollingType.NoRolling"/>.</remarks>
        [FixHoliday("NewYearsEve", 31, Month.December, HolidayRollingType = HolidayRollingType.NoRolling)]
        NewYearsEve
    }
}