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

namespace Dodoni.Finance
{
    /// <summary>Represents the interpretation of the <code>referenceStartDate</code> and <code>referenceEndDate</code> arguments, i.e. some reference 
    /// period, in <see cref="IDayCountConvention.GetYearFraction(DateTime, DateTime, DateTime?, DateTime?)"/> 
    /// and  <see cref="IDayCountConvention.GetYearFraction(DateTime, DateTime, out int, DateTime?, DateTime?)"/>.
    /// </summary>
    /// <remarks>The reference period is assumed to be optional in any case.</remarks>
    public enum DayCountReferencePeriodUsage
    {
        /// <summary>The reference period is optional and will be ignored by the 
        /// day count convention.
        /// </summary>
        Ignore,

        /// <summary>The reference period represents some optional coupon period.
        /// </summary>
        /// <example>The day count convention "ISMA 'Act/Act' (Bond)" has some optional coupon period.</example>
        OptionalCouponPeriod,

        /// <summary>The reference period represents some optional interest period, i.e. including
        /// the interest period start date and exclusive the end date of the interest period.
        /// </summary>
        OptionalInterestPeriod
    }
}