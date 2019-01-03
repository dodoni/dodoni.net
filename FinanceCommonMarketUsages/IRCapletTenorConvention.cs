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
using Dodoni.Finance.CommonMarketUsages.IRCapletTenorConventions;
using Microsoft.Extensions.Logging;

namespace Dodoni.Finance.CommonMarketUsages
{
    /// <summary>Serves as factory for (Interest rate) caplet tenor conventions, used to strip caplets from caps.
    /// </summary>
    public static class IRCapletTenorConvention
    {
        #region private static members

        /// <summary>The internal pool of the (interest rate) caplet tenor conventions.
        /// </summary>
        private static IdentifierNameableDictionary<IIRCapletTenorConvention> sm_Pool;

        /// <summary>The logger.
        /// </summary>
        private static ILogger sm_Logger;
        #endregion

        #region public static (readonly) members

        /// <summary>The underlying Libor rate tenor of each caplet is equal to 3M.
        /// </summary>
        public static readonly IIRCapletTenorConvention CapletTenor3M = new IRCapletTenorConventionConstant(new TenorTimeSpan(0, 3, 0));

        /// <summary>The underlying Libor rate tenor of each caplet is equal to 6M.
        /// </summary>
        public static readonly IIRCapletTenorConvention CapletTenor6M = new IRCapletTenorConventionConstant(new TenorTimeSpan(0, 6, 0));

        /// <summary>The underlying Libor rate tenor for caplets up to 2Y is equal to 3M; otherwise 6M, thus starting in '2y' the caplet tenor is equal to '6m'.
        /// </summary>
        public static readonly IIRCapletTenorConvention CapletTenor3MUntil2YThen6M = new IRCapletTenor3MUntil2YThen6M();
        #endregion

        #region static constructors

        /// <summary>Initializes a new instance of the <see cref="IRCapletTenorConvention"/> class.
        /// </summary>
        static IRCapletTenorConvention()
        {
            sm_Pool = new IdentifierNameableDictionary<IIRCapletTenorConvention>(CapletTenor3M, CapletTenor6M, CapletTenor3MUntil2YThen6M);
            // sm_Logger = Logger.Stream.Create("Pool", typeof(IRCapletTenorConvention), "Caplet tenor convention", "Pool");
        }
        #endregion

        #region public static properties

        /// <summary>Gets the number of (interest rate) caplet tenor conventions.
        /// </summary>
        /// <value>The number of (interest rate) caplet tenor conventions.</value>
        public static int Count
        {
            get { return sm_Pool.Count; }
        }

        /// <summary>Gets the (interest rate) caplet tenor conventions.
        /// </summary>
        /// <value>The (interest rate) caplet tenor conventions.</value>
        public static IEnumerable<IIRCapletTenorConvention> Values
        {
            get { return sm_Pool.Values; }
        }

        /// <summary>Gets the (interest rate) caplet tenor convention names.
        /// </summary>
        /// <value>The (interest rate) caplet tenor convention names.</value>
        public static IEnumerable<string> Names
        {
            get { return sm_Pool.Names; }
        }
        #endregion

        #region public static methods

        /// <summary>Gets the (interest rate) caplet tenor convention names in its <see cref="IdentifierString"/> representation.
        /// </summary>
        /// <returns>A collection of (interest rate) caplet tenor convention names in its <see cref="IdentifierString"/> representation.</returns>
        public static IEnumerable<IdentifierString> GetIdentifierStringNames()
        {
            return sm_Pool.GetNamesAsIdentifierStrings();
        }

        /// <summary>Gets a specified (interest rate) caplet tenor convention.
        /// </summary>
        /// <param name="name">The name of the caplet tenor convention.</param>
        /// <param name="value">The value (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public static bool TryGetValue(IdentifierString name, out IIRCapletTenorConvention value)
        {
            return sm_Pool.TryGetValue(name, out value);
        }

        /// <summary>Gets a specified (interest rate) caplet tenor convention.
        /// </summary>
        /// <param name="name">The name of the caplet tenor convention.</param>
        /// <param name="value">The value (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public static bool TryGetValue(string name, out IIRCapletTenorConvention value)
        {
            return sm_Pool.TryGetValue(name, out value);
        }

        /// <summary>Adds the specified  (interest rate) caplet tenor convention.
        /// </summary>
        /// <param name="value">The caplet tenor convention to add.</param>
        /// <returns>A value indicating whether <paramref name="value"/> has been added.</returns>
        public static ItemAddedState Add(IIRCapletTenorConvention value)
        {
            ItemAddedState state = sm_Pool.Add(value);
            //sm_Logger.Add_PoolItemState(state, (value != null) ? value.Name : null);
            return state;
        }
        #endregion
    }
}