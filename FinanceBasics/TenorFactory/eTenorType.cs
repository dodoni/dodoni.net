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
    /// <summary>Represents the type of the tenor, i.e. overnight, tomorrow next or a regular description (especially
    /// the number of years, months and days to some reference date maybe given in some string format '5Y6M' etc.).
    /// </summary>
    /// <remarks>Do not change the values of the elements of the enumeration. These values are used for the 
    /// IComparer and IComparable interfaces, i.e. for the sorting of <see cref="TenorTimeSpan"/> instances.
    /// </remarks>
    public enum TenorType
    {
        /// <summary>Overnight date.
        /// </summary>
        /// <remarks>The given number (= -3) is used for the IComparable interface. Do not change this value.</remarks>
        [String("ON")]
        Overnight = -3,

        /// <summary>Tomorrow next date.
        /// </summary>
        /// <remarks>The given number (= -2) is used for the IComparable interface. Do not change this value.</remarks>
        [String("TN")]
        TomorrowNext = -2,

        /// <summary>A regular tenor.
        /// </summary>
        /// <remarks>The given number (= 0) is used for the IComparable interface. Do not change this value.</remarks>
        RegularTenor = 0
    }
}