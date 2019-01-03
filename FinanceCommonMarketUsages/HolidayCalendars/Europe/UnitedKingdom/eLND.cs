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
    /// <summary>Represents the list of holidays taken into account for the holiday calendar 'LDN', i.e. London/GB,
    /// see http://www.dti.gov.uk/er/bankhol.htm.
    /// </summary>
    [CustomHolidayCalendar("LND", HolidayCalendarRegion.Europe, "Dodoni.Finance.CommonMarketUsages.HolidayCalendars.HolidayNameResources", Description = "_LND")]
    internal enum LND
    {
        /// <summary>'New year' public holiday.
        /// </summary>
        /// <remarks>Represents the first January of each year. The holiday rolling type is set to
        /// <see cref="HolidayRollingType.ForwardRolling"/>.</remarks>
        [FixHoliday("NewYear", 1, Month.January, HolidayRollingType = HolidayRollingType.ForwardRolling)]
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

        /// <summary>Early may Bank holiday.
        /// </summary>
        /// <remarks>Represents early may bank holiday, i.e. the first monday in May.</remarks>
        [MonthWeekdayHoliday("EarlyMayBankHoliday", DayOfWeek.Monday, Month.May, MonthWeekdayHoliday.WeekIndex.First)]
        EarlyMayBankHoliday,

        /// <summary>Spring Bank holiday.
        /// </summary>
        /// <remarks>Represents the spring Bank holiday, i.e. the last monday in May.</remarks>
        [MonthWeekdayHoliday("SpringBankHolidday", DayOfWeek.Monday, Month.May, MonthWeekdayHoliday.WeekIndex.Last)]
        SpringBankHoliday,

        /// <summary>Golden jubilee Bank holiday.
        /// </summary>
        /// <remarks>Represents the golden jubilee bank holiday, i.e. 3. june 2002.</remarks>
        [SingleHoliday("GoldenJubileeBankHoliday", 3, Month.June, 2002)]
        GoldenJubileeBankHoliday,

        /// <summary>Special spring Bank holiday.
        /// </summary>
        /// <remarks>Represents the special spring Bank holiday, i.e. 4. june 2002.</remarks>
        [SingleHoliday("SpecialSpringBankHoliday", 4, Month.June, 2002)]
        SpecialSpringBankHoliday,

        /// <summary>Summer Bank holiday.
        /// </summary>
        /// <remarks>Represents Summer Bank holiday, i.e. the last monday in August.</remarks>
        [MonthWeekdayHoliday("SummerBankHoliday", DayOfWeek.Monday, Month.August, MonthWeekdayHoliday.WeekIndex.Last)]
        SummerBankHoliday,

        /// <summary>The 'christmas day'.
        /// </summary>
        /// <remarks>Represents christmas day, i.e. 25st december. The holiday rolling type is set to
        /// <see cref="HolidayRollingType.ForwardRolling"/>.</remarks>
        [FixHoliday("ChristmasDay", 25, Month.December, HolidayRollingType = HolidayRollingType.ForwardRolling)]
        ChristmasDay,

        /// <summary>The 'boxing day'.
        /// </summary>
        /// <remarks>Represents boxing date, i.e. 26st december, the holiday rolling type is set to
        /// <see cref="HolidayRollingType.ForwardRolling"/>.</remarks>
        [FixHoliday("BoxingDay", 26, Month.December, HolidayRollingType = HolidayRollingType.ForwardRolling)]
        BoxingDay,

        /// <summary>The 'new year eve' for the year 1999.
        /// </summary>
        /// <remarks>Represents new years eve, i.e. 31st december 1999.</remarks>
        [SingleHoliday("NewYearsEve", 31, Month.December, 1999)]
        NewYearsEve
    }
}
