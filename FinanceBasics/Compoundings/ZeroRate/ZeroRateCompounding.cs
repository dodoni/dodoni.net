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

using Dodoni.BasicComponents;
using Dodoni.Finance.DateFactory;
using Dodoni.BasicComponents.Logging;
using Dodoni.BasicComponents.Containers;
using Microsoft.Extensions.Logging;

namespace Dodoni.Finance.Compoundings
{
    /// <summary>The zero rate compounding convention of a zero-coupon bond.
    /// </summary>
    public class ZeroRateCompounding : IEnumerable<IZeroRateCompounding>
    {
        #region private (static) members

        /// <summary>The internal pool of the compounding conventions.
        /// </summary>
        private IdentifierNameableDictionary<IZeroRateCompounding> m_Pool;

        /// <summary>The logger.
        /// </summary>
        private ILogger m_LoggingStream;
        #endregion

        #region public members

        /// <summary>The continuously compounding, i.e. P(t,T) = exp( -(T-t) * r).
        /// </summary>
        public readonly ContinuouslyZeroRateCompounding Continuously = new ContinuouslyZeroRateCompounding();

        /// <summary>The annually compounding, i.e. P(t,T) = (1.0 + r)^{-(T-t)}.
        /// </summary>
        public readonly PeriodicZeroRateCompounding Annually = new PeriodicZeroRateCompounding(DateScheduleFrequency.Annually);

        /// <summary>The semi-annually compounding, i.e. P(t,T) = (1.0 + r/2)^{-(T-t) * 2}.
        /// </summary>
        public readonly PeriodicZeroRateCompounding SemiAnnually = new PeriodicZeroRateCompounding(DateScheduleFrequency.SemiAnnually);

        /// <summary>The quarterly compounding, i.e. P(t,T) = (1.0 + r/4)^{-(T-t) * 4}.
        /// </summary>
        public readonly PeriodicZeroRateCompounding Quarterly = new PeriodicZeroRateCompounding(DateScheduleFrequency.Quarterly);

        /// <summary>The bi-monthly (=two month) compounding, i.e. P(t,T) = (1.0 + r/6)^{-(T-t) * 6}.
        /// </summary>
        public readonly PeriodicZeroRateCompounding BiMonthly = new PeriodicZeroRateCompounding(DateScheduleFrequency.BiMonthly);

        /// <summary>The monthly compounding, i.e. P(t,T) = (1.0 + r/12)^{-(T-t) * 12}.
        /// </summary>
        public readonly PeriodicZeroRateCompounding Monthly = new PeriodicZeroRateCompounding(DateScheduleFrequency.Monthly);

        /// <summary>The daily compounding, i.e. P(t,T) = (1.0 + r/n)^{- (T-t) * n}, where n = 365. 
        /// </summary>
        public readonly PeriodicZeroRateCompounding Daily = new PeriodicZeroRateCompounding(DateScheduleFrequency.Daily);
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="ZeroRateCompounding"/> class.
        /// </summary>
        internal ZeroRateCompounding()
        {
            m_Pool = new IdentifierNameableDictionary<IZeroRateCompounding>(Continuously, Annually, SemiAnnually, Quarterly, BiMonthly, Monthly, Daily);

            //m_LoggingStream = Logger.Stream.Create("Pool", typeof(ZeroRateCompounding), "Zero rate Compounding convention", "Pool");
            m_LoggingStream = Logger.Stream.CreateLogger(typeof(ZeroRateCompounding));
        }
        #endregion

        #region public properties

        /// <summary>Gets the number of compounding conventions.
        /// </summary>
        /// <value>The number of compounding conventions.</value>
        public int Count
        {
            get { return m_Pool.Count; }
        }

        /// <summary>Gets the compounding conventions.
        /// </summary>
        /// <value>The compounding conventions.</value>
        public IEnumerable<IZeroRateCompounding> Values
        {
            get { return m_Pool.Values; }
        }

        /// <summary>Gets the compunding convention names.
        /// </summary>
        /// <value>The compounding convention names.</value>
        public IEnumerable<string> Names
        {
            get { return m_Pool.Names; }
        }
        #endregion

        #region public methods

        #region IEnumerable<IZeroRateCompounding> Members

        /// <summary>Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<IZeroRateCompounding> GetEnumerator()
        {
            return m_Pool.GetEnumerator();
        }
        #endregion

        #region IEnumerable Members

        /// <summary>Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return m_Pool.GetEnumerator();
        }
        #endregion

        /// <summary>Gets the compounding convention names in its <see cref="IdentifierString"/> representation.
        /// </summary>
        /// <returns>A collection of compounding convention names in its <see cref="IdentifierString"/> representation.</returns>
        public IEnumerable<IdentifierString> GetIdentifierStringNames()
        {
            return m_Pool.GetNamesAsIdentifierStrings();
        }

        /// <summary>Gets a specified compounding convention.
        /// </summary>
        /// <param name="name">The name of the compounding convention to search.</param>
        /// <param name="value">The compounding convention (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public bool TryGetValue(IdentifierString name, out IZeroRateCompounding value)
        {
            return m_Pool.TryGetValue(name, out value);
        }

        /// <summary>Gets a specified compounding convention.
        /// </summary>
        /// <param name="name">The name of the compounding convention to search.</param>
        /// <param name="value">The compounding convention (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public bool TryGetValue(string name, out IZeroRateCompounding value)
        {
            return m_Pool.TryGetValue(name, out value);
        }

        /// <summary>Adds a specified compounding convention.
        /// </summary>
        /// <param name="value">The compounding convention to add.</param>
        /// <returns>A value indicating whether <paramref name="value"/> has been added.</returns>
        public ItemAddedState Add(IZeroRateCompounding value)
        {
            ItemAddedState state = m_Pool.Add(value);
            //m_LoggingStream.Add_PoolItemState(state, value.Name);
            return state;
        }
        #endregion
    }
}