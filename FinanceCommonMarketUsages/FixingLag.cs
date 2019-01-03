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
using Dodoni.Finance.FixingLags;

namespace Dodoni.Finance
{
    /// <summary>Serves as factory for <see cref="IFixingLag"/> objects that encapsulates the calculation of a fixing dates (of a specific underlying).
    /// </summary>
    public static partial class FixingLag
    {
        #region public (static) readonly members

        /// <summary>Do not apply any lag for the calculation of the fixing date.
        /// </summary>
        public static readonly IFixingLag None = new NoneFixingLag();

        /// <summary>Apply a fixing lag of 1 business day for the calculation of the fixing date.
        /// </summary>
        public static readonly IFixingLag Minus1BDay;

        /// <summary>Apply a fixing lag of 2 business days for the calculation of the fixing date.
        /// </summary>
        public static readonly IFixingLag Minus2BDays;
        #endregion

        #region static constructor

        /// <summary>Initializes the <see cref="FixingLag" /> class.
        /// </summary>
        static FixingLag()
        {
            Minus1BDay = new ConstFixingLag(-1);
            Minus2BDays = new ConstFixingLag(-2);
        }
        #endregion

        #region public static methods

        /// <summary>Creates a new <see cref="IFixingLag"/>, where the lag of the fixing date calculation is specified by a number of business days.
        /// </summary>
        /// <param name="businessDays">The number of business days taken into account for the calculation of fixing dates, for example -2.</param>
        /// <returns>The specific <see cref="IFixingLag"/> object.</returns>
        public static IFixingLag Create(int businessDays)
        {
            return new ConstFixingLag(businessDays);
        }
        #endregion
    }
}