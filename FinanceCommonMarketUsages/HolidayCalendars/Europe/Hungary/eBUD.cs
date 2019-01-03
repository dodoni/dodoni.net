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
//using System;
//using System.Text;

//

//using Dodoni.Finance.CommonMarketUsages.HolidayCalendars.Europe
//{
//    /// <summary>Represents the list of holidays taken into account for the holiday calendar 'BUD', which is
//    /// the holiday calendar for Budapest.
//    /// </summary>
//    [HolidayCalendar("BUD", HolidayCalendarRegion.Europe, "Dodoni.BasicComponents.HolidayCalendarFactory.Europe.EuropeHolidayNames")]
//    public enum eBudapestHolidayTable
//    {
//        /// <summary>'New year' public holiday, represents the first January of each year. 
//        /// The holiday rolling type is set to <see cref="HolidayRollingType.NoRolling"/>.
//        /// </summary>
//        [FixHoliday("NewYear", 1, Month.January, HolidayRollingType = HolidayRollingType.NoRolling)]
//        NewYearsDay,

//        /// <summary>The 'easter monday', i.e. represents easter monday, the holiday rolling type is set to
//        /// <see cref="HolidayRollingType.NoRolling"/>.
//        /// </summary>
//        [EasterDependingHoliday("EasterMonday", 0, HolidayRollingType = HolidayRollingType.NoRolling)]
//        EasterMonday,

//        // whit monday

//        [FixHoliday("BudapestNationalDay",15, Month.March)]
//        NationalDay,

//        /// <summary>The 'labour day', i.e. 1st of may, starting in year 2000 and the holiday rolling type is set to
//        /// <see cref="HolidayRollingType.NoRolling"/>.
//        /// </summary>
//        [FixHoliday("LabourDay", 1, Month.May, FirstYear = 2000, HolidayRollingType = HolidayRollingType.NoRolling)]
//        LabourDay,

//        [FixHoliday("ConstitutionDay",20, Month.August)]
//        ConstitutionDay,

//        [FixHoliday("ReinternalDay",23, Month.October)]
//        ReinternalDay,

//        [FixHoliday("AllSaintsDay",1, Month.November)]
//        AllSaintsDay,

//        /// <summary>The 'christmas day', i.e. 25st december. The holiday rolling type is set to
//        /// <see cref="HolidayRollingType.NoRolling"/>.
//        /// </summary>
//        [FixHoliday("ChristmasDay", 25, Month.December, HolidayRollingType = HolidayRollingType.NoRolling)]
//        ChristmasDay,

//        /// <summary>The 'boxing day', i.e. 26st december, starting in year 2000 and the holiday rolling type is set to
//        /// <see cref="HolidayRollingType.NoRolling"/>.
//        /// </summary>
//        [FixHoliday("BoxingDay", 26, Month.December,  HolidayRollingType = HolidayRollingType.NoRolling)]
//        BoxingDay,
//    }
//}