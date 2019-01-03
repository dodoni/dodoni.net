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
using Dodoni.Finance.CommonMarketUsages.DayCountConventions;
using Microsoft.Extensions.Logging;

namespace Dodoni.Finance.CommonMarketUsages
{
    /// <summary>Serves as factory for day count conventions.
    /// </summary>
    public static class DayCountConvention
    {
        #region private static members

        /// <summary>The internal pool of the day count conventions.
        /// </summary>
        private static IdentifierNameableDictionary<IDayCountConvention> sm_Pool;

        /// <summary>The logger.
        /// </summary>
        private static ILogger sm_Logger;
        #endregion

        #region public static (readonly) members

        /// <summary>The 'Actual/Actual' day count conventions.
        /// </summary>
        public static readonly ActualActual ActualActual = new ActualActual();

        /// <summary>The day count convention 'Act/360'.
        /// </summary>
        public static readonly IDayCountConvention Actual360 = new Actual360();

        /// <summary>The day count convention 'Act/365'.
        /// </summary>
        public static readonly IDayCountConvention Actual365 = new Actual365();

        /// <summary>The day count convention 'Act/365.25'.
        /// </summary>
        public static readonly IDayCountConvention Actual365_25 = new Actual365_25();

        /// <summary>The day count convention '1/1'.
        /// </summary>
        public static readonly IDayCountConvention OneOverOne = new OneOverOne();

        /// <summary>The day count convention '30/360'.
        /// </summary>
        public static readonly IDayCountConvention Thirty360 = new Thirty360();

        /// <summary>The day count convention '30E/360'.
        /// </summary>
        public static readonly IDayCountConvention ThirtyE360 = new ThirtyE360();

        /// <summary>Serves as factory for the day count convention 'BU/252' used for Brazilian trades.
        /// </summary>
        public static readonly Bu252 Bu252 = new Bu252();
        #endregion

        #region static constructors

        /// <summary>Initializes a new instance of the <see cref="DayCountConvention"/> class.
        /// </summary>
        static DayCountConvention()
        {
            sm_Pool = new IdentifierNameableDictionary<IDayCountConvention>(Actual360, Actual365, Actual365_25, ActualActual.AFB, ActualActual.ISDA, ActualActual.ISMA,
                OneOverOne, Thirty360, ThirtyE360);

//            sm_Logger = Logger.Stream.Create("Pool", typeof(DayCountConvention), "Day count convention", "Pool");
        }
        #endregion

        #region public static properties

        /// <summary>Gets the number of day count conventions.
        /// </summary>
        /// <value>The number of day count conventions.</value>
        public static int Count
        {
            get { return sm_Pool.Count; }
        }

        /// <summary>Gets the day count conventions.
        /// </summary>
        /// <value>The day count conventions.</value>
        public static IEnumerable<IDayCountConvention> Values
        {
            get { return sm_Pool.Values; }
        }

        /// <summary>Gets the day count convention names.
        /// </summary>
        /// <value>The day count convention names.</value>
        public static IEnumerable<string> Names
        {
            get { return sm_Pool.Names; }
        }
        #endregion

        #region public static methods

        /// <summary>Gets the day count convention names in its <see cref="IdentifierString"/> representation.
        /// </summary>
        /// <returns>A collection of day count convention names its <see cref="IdentifierString"/> representation.</returns>
        public static IEnumerable<IdentifierString> GetIdentifierStringNames()
        {
            return sm_Pool.GetNamesAsIdentifierStrings();
        }

        /// <summary>Gets a specified day count convention.
        /// </summary>
        /// <param name="name">The name of the day count convention.</param>
        /// <param name="value">The day count convention (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public static bool TryGetValue(IdentifierString name, out IDayCountConvention value)
        {
            return sm_Pool.TryGetValue(name, out value);
        }

        /// <summary>Gets a specified day count convention.
        /// </summary>
        /// <param name="name">The name of the day count convention.</param>
        /// <param name="value">The day count convention (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public static bool TryGetValue(string name, out IDayCountConvention value)
        {
            return sm_Pool.TryGetValue(name, out value);
        }

        /// <summary>Adds the specified day count convention.
        /// </summary>
        /// <param name="value">The business day convention to add.</param>
        /// <returns>A value indicating whether <paramref name="value"/> has been added.</returns>
        public static ItemAddedState Add(IDayCountConvention value)
        {
            ItemAddedState state = sm_Pool.Add(value);
//            sm_Logger.Add_PoolItemState(state, (value != null) ? value.Name : null);
            return state;
        }
        #endregion
    }
}