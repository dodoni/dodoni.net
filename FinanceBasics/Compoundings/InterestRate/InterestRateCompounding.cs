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
using System.Collections.Generic;

using Dodoni.BasicComponents;
using Dodoni.Finance.DateFactory;
using Dodoni.BasicComponents.Logging;
using Dodoni.BasicComponents.Containers;
using Microsoft.Extensions.Logging;

namespace Dodoni.Finance.Compoundings
{
    /// <summary>The compounding conventions for interest rates.
    /// </summary>
    public class InterestRateCompounding : IEnumerable<IInterestRateCompounding>
    {
        #region private members

        /// <summary>The internal pool of the compounding conventions.
        /// </summary>
        private IdentifierNameableDictionary<IInterestRateCompounding> m_Pool;

        /// <summary>The logger.
        /// </summary>
        private ILogger m_LoggingStream;
        #endregion

        #region public members

        /// <summary>The simple (i.e. linear) compounding, i.e. 'Notional' * (1.0 + r * t).
        /// </summary>
        public readonly SimpleInterestCompounding Simple = new SimpleInterestCompounding();

        /// <summary>The continuously compounding, i.e. 'Notional' * exp( t * r).
        /// </summary>
        public readonly ContinuouslyInterestCompounding Continuously = new ContinuouslyInterestCompounding();

        /// <summary>The annually compoundingconvention, i.e. 'Notional' * (1.0 + r)^t.
        /// </summary>
        public readonly PeriodicInterestCompounding Annually = new PeriodicInterestCompounding(DateScheduleFrequency.Annually);

        /// <summary>The semi-annually compounding, i.e. 'Notional' * (1.0 + r/2)^{t * 2}.
        /// </summary>
        public readonly PeriodicInterestCompounding SemiAnnually = new PeriodicInterestCompounding(DateScheduleFrequency.SemiAnnually);

        /// <summary>The quarterly compounding, i.e. 'Notional' *  (1.0 + r/4)^{t * 4}.
        /// </summary>
        public readonly PeriodicInterestCompounding Quarterly = new PeriodicInterestCompounding(DateScheduleFrequency.Quarterly);

        /// <summary>The bi-monthly (=two month) compounding, i.e. 'Notional' * (1.0 + r/6)^{t * 6}.
        /// </summary>
        public readonly PeriodicInterestCompounding BiMonthly = new PeriodicInterestCompounding(DateScheduleFrequency.BiMonthly);

        /// <summary>The monthly compounding, i.e. 'Notional' * (1.0 + r/12)^{t * 12}.
        /// </summary>
        public readonly PeriodicInterestCompounding Monthly = new PeriodicInterestCompounding(DateScheduleFrequency.Monthly);

        /// <summary>The daily compounding, i.e. 'Notional' * (1.0 + r/n)^{t * n}, where n = 365. 
        /// </summary>
        public readonly PeriodicInterestCompounding Daily = new PeriodicInterestCompounding(DateScheduleFrequency.Daily);

        /// <summary>The simple compounding (i.e. linear compounding) up to time to maturity 1Y; afterwards identical to <see cref="Annually"/>.
        /// </summary>
        public readonly SimpleThenPeriodicCompounding SimpleThenAnnually = new SimpleThenPeriodicCompounding(DateScheduleFrequency.Annually);

        /// <summary>The simple compounding (i.e. linear compounding) up to time to maturity 1Y times number of periods per year; afterwards identical to <see cref="SemiAnnually"/>.
        /// </summary>
        public readonly SimpleThenPeriodicCompounding SimpleThenSemiAnnually = new SimpleThenPeriodicCompounding(DateScheduleFrequency.SemiAnnually);

        /// <summary>The simple compounding (i.e. linear compounding) up to time to maturity 1Y times number of periods per year; afterwards identical to <see cref="Quarterly"/>.
        /// </summary>
        public readonly SimpleThenPeriodicCompounding SimpleThenQuarterly = new SimpleThenPeriodicCompounding(DateScheduleFrequency.Quarterly);

        /// <summary>The simple compounding (i.e. linear compounding) up to time to maturity 1Y times number of periods per year; afterwards identical to <see cref="BiMonthly"/>.
        /// </summary>
        public readonly SimpleThenPeriodicCompounding SimpleThenBiMonthly = new SimpleThenPeriodicCompounding(DateScheduleFrequency.BiMonthly);

        /// <summary>The simple compounding (i.e. linear compounding) up to time to maturity 1Y times number of periods per year; afterwards identical to <see cref="Monthly"/>.
        /// </summary>
        public readonly SimpleThenPeriodicCompounding SimpleThenMonthly = new SimpleThenPeriodicCompounding(DateScheduleFrequency.Monthly);

        /// <summary>The simple compounding (i.e. linear compounding) up to time to maturity 1Y times number of periods per year; afterwards identical to <see cref="Daily"/>.
        /// </summary>
        public readonly SimpleThenPeriodicCompounding SimpleThenDaily = new SimpleThenPeriodicCompounding(DateScheduleFrequency.Daily);
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="InterestRateCompounding"/> class.
        /// </summary>
        internal InterestRateCompounding()
        {
            m_Pool = new IdentifierNameableDictionary<IInterestRateCompounding>(Simple, Continuously, Annually, SemiAnnually, Quarterly, BiMonthly, Monthly, Daily,
                SimpleThenAnnually, SimpleThenSemiAnnually, SimpleThenQuarterly, SimpleThenBiMonthly, SimpleThenMonthly, SimpleThenDaily);

            m_LoggingStream = Logger.Stream.CreateLogger(typeof(InterestRateCompounding)); // .Stream.Create("Pool", typeof(InterestRateCompounding), "Interest Compounding convention", "Pool");
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
        public IEnumerable<IInterestRateCompounding> Values
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

        #region IEnumerable<IInterestRateCompounding> Members

        /// <summary>Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<IInterestRateCompounding> GetEnumerator()
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
        public bool TryGetValue(IdentifierString name, out IInterestRateCompounding value)
        {
            return m_Pool.TryGetValue(name, out value);
        }

        /// <summary>Gets a specified compounding convention.
        /// </summary>
        /// <param name="name">The name of the compounding convention to search.</param>
        /// <param name="value">The compounding convention (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public bool TryGetValue(string name, out IInterestRateCompounding value)
        {
            return m_Pool.TryGetValue(name, out value);
        }

        /// <summary>Adds a specified compounding convention.
        /// </summary>
        /// <param name="value">The compounding convention to add.</param>
        /// <returns>A value indicating whether <paramref name="value"/> has been added.</returns>
        public ItemAddedState Add(IInterestRateCompounding value)
        {
            ItemAddedState state = m_Pool.Add(value);
            //m_LoggingStream.Add_PoolItemState(state, value.Name);
            return state;
        }
        #endregion
    }
}