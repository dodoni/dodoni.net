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
using System.Collections.Generic;

using Dodoni.Finance;
using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Logging;
using Dodoni.BasicComponents.Containers;
using Dodoni.Finance.CommonMarketUsages.BusinessDayConventions;
using Microsoft.Extensions.Logging;

namespace Dodoni.Finance.CommonMarketUsages
{
    /// <summary>Serves as factory for business day conventions.
    /// </summary>
    public static class BusinessDayConvention
    {
        #region private static members

        /// <summary>The internal pool of the business day conventions.
        /// </summary>
        private static IdentifierNameableDictionary<IBusinessDayConvention> sm_Pool;

        /// <summary>The logger.
        /// </summary>
        private static ILogger sm_Logger;
        #endregion

        #region public static (readonly) members

        /// <summary>The business day convention 'no adjustment'.
        /// </summary>
        public static readonly IBusinessDayConvention NoAdjustment = new NoAdjustment();

        /// <summary>The business day convention 'following'.
        /// </summary>
        public static readonly IBusinessDayConvention Following = new FollowingAdjustment();

        /// <summary>The business day convention 'modified following'.
        /// </summary>
        public static readonly IBusinessDayConvention ModFollowing = new ModFollowingAdjustment();

        /// <summary>The business day convention 'preceding/previous business day'.
        /// </summary>
        public static readonly IBusinessDayConvention Preceding = new PrecedingAdjustment();

        /// <summary>The business day convention 'end of month' (no adjustment).
        /// </summary>
        public static readonly IBusinessDayConvention EndOfMonth = new EndOfMonthNoAdjustment();

        /// <summary>The business day convention 'end of month, following'.
        /// </summary>
        public static readonly IBusinessDayConvention EndOfMonthFollowing = new EndOfMonthFollowingAdjustment();

        /// <summary>The business day convention 'end of month, preceding'.
        /// </summary>
        public static readonly IBusinessDayConvention EndOfMonthPreceding = new EndOfMonthPrecedingAdjustment();

        /// <summary>The business day convention 'end of month, ignoring leap years'.
        /// </summary>
        public static readonly IBusinessDayConvention EndOfMonthIgnoreLeapYears = new EndOfMonthIgnoreLeapYearsAdjustment();

        /// <summary>The business day convention 'end of quarter' (no adjustment).
        /// </summary>
        public static readonly IBusinessDayConvention EndOfQuarter = new EndOfQuarterNoAdjustment();

        /// <summary>The business day convention 'end of quarter preceding'.
        /// </summary>
        public static readonly IBusinessDayConvention EndOfQuarterPreceding = new EndOfQuarterPrecedingAdjustment();

        /// <summary>The business day convention 'end of quarter following'.
        /// </summary>
        public static readonly IBusinessDayConvention EndOfQuarterFollowing = new EndOfQuarterFollowingAdjustment();

        /// <summary>The business day convention 'third wednesday'.
        /// </summary>
        public static readonly IBusinessDayConvention ThirdWednesday = new ThirdWednesdayAdjustment();

        /// <summary>The business day convention 'two business days prior third wednesday'.
        /// </summary>
        public static readonly IBusinessDayConvention TwoBusinessDaysPriorThirdWednesday = new TwoBusinessDaysPriorThirdWednesdayAdjustment();
        #endregion

        #region static constructors

        /// <summary>Initializes a new instance of the <see cref="BusinessDayConvention"/> class.
        /// </summary>
        static BusinessDayConvention()
        {
            sm_Pool = new IdentifierNameableDictionary<IBusinessDayConvention>(NoAdjustment, Following, ModFollowing, Preceding,
                EndOfMonth, EndOfMonthFollowing, EndOfMonthPreceding, EndOfMonthIgnoreLeapYears,
                EndOfQuarter, EndOfQuarterPreceding, EndOfQuarterFollowing,
                ThirdWednesday, TwoBusinessDaysPriorThirdWednesday);

//            sm_Logger = Logger.Stream.Create("Pool", typeof(BusinessDayConvention), "Business day convention", "Pool");
        }
        #endregion

        #region public static properties

        /// <summary>Gets the number of business day conventions.
        /// </summary>
        /// <value>The number of business day conventions.</value>
        public static int Count
        {
            get { return sm_Pool.Count; }
        }

        /// <summary>Gets the business day conventions.
        /// </summary>
        /// <value>The business day conventions.</value>
        public static IEnumerable<IBusinessDayConvention> Values
        {
            get { return sm_Pool.Values; }
        }

        /// <summary>Gets the business day convention names.
        /// </summary>
        /// <value>The business day convention names.</value>
        public static IEnumerable<string> Names
        {
            get { return sm_Pool.Names; }
        }
        #endregion

        #region public static methods

        /// <summary>Gets the business day convention names in its <see cref="IdentifierString"/> representation.
        /// </summary>
        /// <returns>A collection of business day convention names in its <see cref="IdentifierString"/> representation.</returns>
        public static IEnumerable<IdentifierString> GetIdentifierStringNames()
        {
            return sm_Pool.GetNamesAsIdentifierStrings();
        }

        /// <summary>Gets a specified business day convention.
        /// </summary>
        /// <param name="name">The name of the business day convention to search.</param>
        /// <param name="value">The business day convention (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public static bool TryGetValue(IdentifierString name, out IBusinessDayConvention value)
        {
            return sm_Pool.TryGetValue(name, out value);
        }

        /// <summary>Gets a specified business day convention.
        /// </summary>
        /// <param name="name">The name of the business day convention to search.</param>
        /// <param name="value">The business day convention (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public static bool TryGetValue(string name, out IBusinessDayConvention value)
        {
            return sm_Pool.TryGetValue(name, out value);
        }

        /// <summary>Adds a specified business day convention.
        /// </summary>
        /// <param name="value">The business day convention to add.</param>
        /// <returns>A value indicating whether <paramref name="value"/> has been added.</returns>
        public static ItemAddedState Add(IBusinessDayConvention value)
        {
            ItemAddedState state = sm_Pool.Add(value);
//            sm_Logger.Add_PoolItemState(state, value.Name);
            return state;
        }
        #endregion
    }
}