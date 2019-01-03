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

using Dodoni.BasicComponents.Containers;

namespace Dodoni.Finance.CommonMarketUsages.Currencies
{
    /// <summary>Serves as collection of currencies; all nested classes are empty, extension methods are
    /// used to show available currencies for each region.
    /// </summary>
    public partial class CurrencyCollection
    {
        /// <summary>Contains the currencies with respect to 'Europe'.
        /// </summary>
        public class Europe
        {
            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="Europe"/> class.
            /// </summary>
            internal Europe()
            {
                EUR = new EUR();
                GBP = new GBP();
            }
            #endregion

            #region public properties

            ///<summary>Gets the currency 'EUR'.
            /// </summary>
            /// <value>The currency 'EUR'.</value>
            public Currency EUR
            {
                get;
                private set;
            }

            /// <summary>Gets the currency 'GBP'.
            /// </summary>
            /// <value>The currency 'GBP'.</value>
            public Currency GBP
            {
                get;
                private set;
            }
            #endregion

            #region internal methods

            /// <summary>Adds the <see cref="Currency"/> objects provided by the current object into a specific <see cref="IdentifierNameableDictionary{Currency}"/> object.
            /// </summary>
            /// <param name="commonCurrencyPool">The common currency pool.</param>
            internal void FillPool(IdentifierNameableDictionary<Currency> commonCurrencyPool)
            {
                commonCurrencyPool.Add(EUR);
                commonCurrencyPool.Add(GBP);
            }
            #endregion
        }
    }
}