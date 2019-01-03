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
using System.Collections.Generic;

using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Logging;
using Dodoni.BasicComponents.Containers;
using Dodoni.Finance.CommonMarketUsages.Currencies;
using Microsoft.Extensions.Logging;

namespace Dodoni.Finance.CommonMarketUsages
{
    /// <summary>Provides common <see cref="Currency"/> objects.
    /// </summary>
    public static class CommonCurrency
    {
        #region nested stuff

        /// <summary>Handles the initialization event of <see cref="CommonCurrency"/> which will be raised before starting to query a specific currency.
        /// </summary>
        public class InitializeEventArgs : EventArgs
        {
            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="InitializeEventArgs"/> class.
            /// </summary>
            internal InitializeEventArgs()
            {
            }
            #endregion

            #region public methods

            /// <summary>Registers a specific <see cref="Currency"/> object.
            /// </summary>
            /// <param name="value">The <see cref="Currency"/> object to register.</param>
            /// <returns>A value indicating whether <paramref name="value"/> has been inserted.</returns>
            public ItemAddedState Add(Currency value)
            {
                return CommonCurrency.Add(value);
            }

            /// <summary>Registers a collection of <see cref="Currency"/> objects.
            /// </summary>
            /// <param name="values">The collection of <see cref="Currency"/> objects to register.</param>
            /// <returns>For each element of <paramref name="values"/> a value indicating whether the <see cref="Currency"/>
            /// object has been inserted.</returns>
            public ItemAddedState[] Add(params Currency[] values)
            {
                if (values == null)
                {
                    return null;
                }
                var stateList = new List<ItemAddedState>();
                for (int j = 0; j < values.Length; j++)
                {
                    stateList.Add(CommonCurrency.Add(values[j]));
                }
                return stateList.ToArray();
            }
            #endregion
        }
        #endregion

        #region private static members

        /// <summary>The pool of the currencies.
        /// </summary>
        private static Lazy<IdentifierNameableDictionary<Currency>> sm_Pool;

        /// <summary>The logger.
        /// </summary>
        private static ILogger sm_Logger;

        /// <summary>Occurs before quering <see cref="Currency"/> objects.
        /// </summary>
        private static event Action<InitializeEventArgs> sm_Initialize;

        /// <summary>A value indicating whether the user has added or removed at least one event to the <see cref="CommonCurrency.Initialize"/> event handler
        /// since the last call of the event-handler.
        /// </summary>
        /// <remarks>This member is used for performance reason only.</remarks>
        private static bool sm_InitializeChanged = true;
        #endregion

        #region public static (readonly) members

        /// <summary>The collection of currencies with respect to 'Africa'.
        /// </summary>
        public static readonly CurrencyCollection.Africa Africa;

        /// <summary>The collection of currencies with respect to 'America'.
        /// </summary>
        public static readonly CurrencyCollection.America America;

        /// <summary>The collection of currencies with respect to 'Asia'.
        /// </summary>
        public static readonly CurrencyCollection.Asia Asia;

        /// <summary>The collection of currencies with respect to 'Europe'.
        /// </summary>
        public static readonly CurrencyCollection.Europe Europe;

        /// <summary>The collection of currencies with respect to 'Oceania'.
        /// </summary>
        public static readonly CurrencyCollection.Oceania Oceania;
        #endregion

        #region static constructors

        /// <summary>Initializes a new instance of the <see cref="Currency"/> class.
        /// </summary>
        static CommonCurrency()
        {
//            sm_Logger = Logger.Stream.Create("Pool", typeof(Currency), "Currency", "Pool");

            Africa = new CurrencyCollection.Africa();
            America = new CurrencyCollection.America();
            Asia = new CurrencyCollection.Asia();
            Europe = new CurrencyCollection.Europe();
            Oceania = new CurrencyCollection.Oceania();

            sm_Pool = new Lazy<IdentifierNameableDictionary<Currency>>(
                () =>
                {
                    var commonCurrencyPool = new IdentifierNameableDictionary<Currency>(100, isReadOnlyExceptAdding: true);  // clear is not allowed, it is readonly

                    Africa.FillPool(commonCurrencyPool);
                    America.FillPool(commonCurrencyPool);
                    Asia.FillPool(commonCurrencyPool);
                    Europe.FillPool(commonCurrencyPool);
                    Oceania.FillPool(commonCurrencyPool);
                    return commonCurrencyPool;
                });
        }
        #endregion

        #region public static properties

        /// <summary>Gets the number of currency objects.
        /// </summary>
        /// <value>The number of currency objects.</value>
        public static int Count
        {
            get
            {
                OnInitialize();
                return sm_Pool.Value.Count;
            }
        }

        /// <summary>Gets the collection of currency objects.
        /// </summary>
        /// <value>The collection of currency objects.</value>
        public static IEnumerable<Currency> Values
        {
            get
            {
                OnInitialize();
                return sm_Pool.Value.Values;
            }
        }

        /// <summary>Gets the (ISO) names of the currency objects.
        /// </summary>
        /// <value>The (ISO) names of the currency objects.</value>
        public static IEnumerable<string> Names
        {
            get
            {
                OnInitialize();
                return sm_Pool.Value.Names;
            }
        }

        /// <summary>Occurs before quering a specific <see cref="Currency"/> object.
        /// </summary>
        public static event Action<InitializeEventArgs> Initialize
        {
            add
            {
                sm_InitializeChanged = true;
                sm_Initialize += value;
            }
            remove
            {
                sm_InitializeChanged = true;
                sm_Initialize -= value;
            }
        }
        #endregion

        #region public static methods

        /// <summary>Gets the currency (ISO) names in its <see cref="IdentifierString"/> representation.
        /// </summary>
        /// <returns>A collection of currency (ISO) names in its <see cref="IdentifierString"/> representation.</returns>
        public static IEnumerable<IdentifierString> GetIdentifierStringNames()
        {
            OnInitialize();
            return sm_Pool.Value.GetNamesAsIdentifierStrings();
        }

        /// <summary>Gets a specific currency.
        /// </summary>
        /// <param name="name">The name of the currency to search.</param>
        /// <param name="value">The value (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public static bool TryGetValue(IdentifierString name, out Currency value)
        {
            OnInitialize();
            return sm_Pool.Value.TryGetValue(name, out value);
        }

        /// <summary>Gets a specific currency.
        /// </summary>
        /// <param name="name">The name of the currency to search.</param>
        /// <param name="value">The value (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public static bool TryGetValue(string name, out Currency value)
        {
            OnInitialize();
            return sm_Pool.Value.TryGetValue(name, out value);
        }

        /// <summary>Adds the specified currency.
        /// </summary>
        /// <param name="value">The currency to add.</param>
        /// <returns>A value indicating whether <paramref name="value"/> has been added.</returns>
        public static ItemAddedState Add(Currency value)
        {
            var state = sm_Pool.Value.Add(value);
//            sm_Logger.Add_PoolItemState(state, (value != null) ? value.Name : null);
            return state;
        }
        #endregion

        #region private (static) methods

        /// <summary>Raises the <see cref="Initialize"/> event.
        /// </summary>
        private static void OnInitialize()
        {
            if (sm_InitializeChanged == true)
            {
                if (sm_Initialize != null) /* add currency objects: */
                {
                    var eventArgs = new InitializeEventArgs();
                    sm_Initialize(eventArgs);
                }
                sm_InitializeChanged = false;
            }
        }
        #endregion
    }
}