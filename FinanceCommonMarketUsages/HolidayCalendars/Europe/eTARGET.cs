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
    /// <summary>Represents the 'TARGET' (Trans-European Automated Real-time Gross settlement Express Transfer) holiday calendar.
    /// </summary>
    [CustomHolidayCalendar("TARGET", HolidayCalendarRegion.Europe, "Dodoni.Finance.CommonMarketUsages.HolidayCalendars.HolidayNameResources", Description = "_TARGET")]
    internal enum TARGET
    {
        /// <summary>'New year' public holiday, represents the first January of each year. 
        /// The holiday rolling type is set to <see cref="HolidayRollingType.NoRolling"/>.
        /// </summary>
        [FixHoliday("NewYear", 1, Month.January, HolidayRollingType = HolidayRollingType.NoRolling)]
        NewYearsDay,

        /// <summary>The 'good friday', i.e. represents good friday, starting in year 2000 and the holiday rolling type is set to
        /// <see cref="HolidayRollingType.NoRolling"/>.
        /// </summary>
        [EasterDependingHoliday("GoodFriday", -3, FirstYear = 2000, HolidayRollingType = HolidayRollingType.NoRolling)]
        GoodFriday,

        /// <summary>The 'easter monday', i.e. represents easter monday, starting in year 2000 and the holiday rolling type is set to
        /// <see cref="HolidayRollingType.NoRolling"/>.
        /// </summary>
        [EasterDependingHoliday("EasterMonday", 0, FirstYear = 2000, HolidayRollingType = HolidayRollingType.NoRolling)]
        EasterMonday,

        /// <summary>The 'labour day', i.e. 1st of may, starting in year 2000 and the holiday rolling type is set to
        /// <see cref="HolidayRollingType.NoRolling"/>.
        /// </summary>
        [FixHoliday("LabourDay", 1, Month.May, FirstYear = 2000, HolidayRollingType = HolidayRollingType.NoRolling)]
        LabourDay,

        /// <summary>The 'christmas day', i.e. 25st december. The holiday rolling type is set to
        /// <see cref="HolidayRollingType.NoRolling"/>.
        /// </summary>
        [FixHoliday("ChristmasDay", 25, Month.December, HolidayRollingType = HolidayRollingType.NoRolling)]
        ChristmasDay,

        /// <summary>The 'boxing day', i.e. 26st december, starting in year 2000 and the holiday rolling type is set to
        /// <see cref="HolidayRollingType.NoRolling"/>.
        /// </summary>
        [FixHoliday("BoxingDay", 26, Month.December, FirstYear = 2000, HolidayRollingType = HolidayRollingType.NoRolling)]
        BoxingDay,

        /// <summary>The 'new year eve', i.e. i.e. 31st december, for the years 1998,1999, 2000 and 2001,
        /// and the holiday rolling type is set to <see cref="HolidayRollingType.NoRolling"/>.
        /// </summary>
        [FixHoliday("NewYearsEve", 31, Month.December, FirstYear = 1998, LastYear = 2001, HolidayRollingType = HolidayRollingType.NoRolling)]
        NewYearsEve
    }
}