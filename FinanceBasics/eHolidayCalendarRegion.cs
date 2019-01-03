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

using Dodoni.BasicComponents.Utilities;

namespace Dodoni.Finance
{
    /// <summary>Represents the region of a holiday calendar.
    /// </summary>
    [Flags]
    public enum HolidayCalendarRegion
    {
        /// <summary>The region of the holiday calendar is unspecified.
        /// </summary>
        [String("Unspecified")]
        Unspecified = 0,

        /// <summary>The holiday calendar is linked to some city/staat of America.
        /// </summary>
        [String("America")]
        America = 1,

        /// <summary>The holiday calendar is linked to some city/staat of Africa.
        /// </summary>
        [String("Africa")]
        Africa = 2,

        /// <summary>The holiday calendar is linked to some city/staat of Asia.
        /// </summary>
        [String("Asia")]
        Asia = 4,

        /// <summary>The holiday calendar is linked to some city/staat of Australia, New Zealand etc.
        /// </summary>
        [String("Oceania")]
        Oceania = 8,

        /// <summary>The holiday calendar is linked to some city/staat of Europe.
        /// </summary>
        [String("Europe")]
        Europe = 16,
    }
}