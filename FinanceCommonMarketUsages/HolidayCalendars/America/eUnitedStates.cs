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

namespace Dodoni.Finance.CommonMarketUsages.HolidayCalendars.America
{
    /// <summary>Represents the public holiday calendar for the United States (http://www.opm.gov/fedhol/), 'US'.
    /// </summary>
    [CustomHolidayCalendar("US", HolidayCalendarRegion.America, "Dodoni.Finance.CommonMarketUsages.HolidayCalendars.HolidayNameResources", Description = "_US")]
    internal enum UnitedStates
    {
        /// <summary>'New year' public holiday, represents the first January of each year. 
        /// The holiday rolling type is set to <see cref="HolidayRollingType.ForwardRolling"/>.
        /// </summary>
        [FixHoliday("NewYear", 1, Month.January, HolidayRollingType = HolidayRollingType.ForwardRolling)]
        NewYearsDay,

        /// <summary>Martin Luther King's birthday (third Monday in January). 
        /// </summary>
        [MonthWeekdayHoliday("MartinLutherKingsBirthDay", DayOfWeek.Monday, Month.January, MonthWeekdayHoliday.WeekIndex.Third)]
        MartinLutherDay,

        /// <summary>Washington's birthday (third Monday in February)
        /// </summary>
        [MonthWeekdayHoliday("WashingtonsBirthDay", DayOfWeek.Monday, Month.February, MonthWeekdayHoliday.WeekIndex.Third)]
        Washington,

        /// <summary>Memorial Day (last Monday in May).
        /// </summary>
        [MonthWeekdayHoliday("MemorialDay", DayOfWeek.Monday, Month.May, MonthWeekdayHoliday.WeekIndex.Last)]
        MemorialDay,

        /// <summary>Independence Day, July 4th (moved to Monday if Sunday or Friday if Saturday.
        /// </summary>
        [FixHoliday("USIndependenceDay", 4, Month.July, HolidayRollingType = HolidayRollingType.SundayToMondaySaturdayToFriday)]
        IndependenceDay,

        /// <summary>The 'labour day', i.e. 1st of may, the holiday rolling type is set to
        /// <see cref="HolidayRollingType.NoRolling"/>.
        /// </summary>
        [FixHoliday("USLabourDay", 1, Month.May, HolidayRollingType = HolidayRollingType.NoRolling)]
        LabourDay,

        /// <summary>Columbus Day, second Monday in October.
        /// </summary>
        [MonthWeekdayHoliday("USColumbusDay", DayOfWeek.Monday, Month.October, MonthWeekdayHoliday.WeekIndex.Second)]
        ColumbusDay,

        /// <summary>Veterans' Day, November 11th (moved to Monday if Sunday or Friday if Saturday).
        /// </summary>
        [FixHoliday("USVeteransDay", 11, Month.November, HolidayRollingType = HolidayRollingType.SundayToMondaySaturdayToFriday)]
        VeteransDay,

        /// <summary>Thanksgiving Day, i.e. fourth Thursday in November.
        /// </summary>
        [MonthWeekdayHoliday("ThanksgivingDay", DayOfWeek.Thursday, Month.November, MonthWeekdayHoliday.WeekIndex.Fourth)]
        ThanksgivingDay,

        /// <summary>The 'christmas day', i.e. 25st december. The holiday rolling type is set to
        /// <see cref="HolidayRollingType.ForwardRolling"/>.
        /// </summary>
        [FixHoliday("ChristmasDay", 25, Month.December, HolidayRollingType = HolidayRollingType.ForwardRolling)]
        ChristmasDay
    }
}