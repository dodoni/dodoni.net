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

using Dodoni.BasicComponents;
using Dodoni.Finance.MarketConventionTemplates;

namespace Dodoni.Finance
{
    /// <summary>Represents a currency, i.e. contains a unique name as well as standard market conventions etc.
    /// </summary>
    public abstract class Currency : IIdentifierNameable, IEquatable<Currency>
    {    
        #region private/public (readonly) members

        /// <summary>The name of the currency, i.e. the ISO code.
        /// </summary>
        private IdentifierString m_Name;

        /// <summary>The (perhaps language dependent) long name of the currency.
        /// </summary>
        private IdentifierString m_LongName;

        /// <summary>The standard market conventions.
        /// </summary>
        public readonly ReadOnlyMarketConventions MarketConventions;
        #endregion

        #region protected constructors

        /// <summary>Initializes a new instance of the <see cref="Currency"/> class.
        /// </summary>
        /// <param name="currencyName">The (ISO) name of the currency.</param>
        /// <param name="standardMarketConventions">The standard Market conventions.</param>
        /// <param name="currencyLongName">The long name of the currency.</param>
        /// <exception cref="ArgumentNullException">Thrown, if one of the arguments (except <paramref name="currencyLongName"/>) is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="standardMarketConventions"/> is not completely defined.</exception>
        protected Currency(IdentifierString currencyName, ReadOnlyMarketConventions standardMarketConventions, string currencyLongName)
        {
            if (currencyName == null)
            {
                throw new ArgumentNullException("currencyName");
            }
            if ((currencyName.IDString == null) || (currencyName.IDString.Length == 0))
            {
                throw new ArgumentException(String.Format(ExceptionMessages.ArgumentIsInvalid, "Currency name"), "currencyName");
            }
            if (standardMarketConventions == null)
            {
                throw new ArgumentNullException("standardMarketConventions");
            }
            m_Name = currencyName;
            m_LongName = (currencyLongName != null) ? currencyLongName.ToIdentifierString() : m_Name;
            MarketConventions = standardMarketConventions;
        }

        /// <summary>Initializes a new instance of the <see cref="Currency"/> class.
        /// </summary>
        /// <param name="currencyName">The (ISO) name of the currency.</param>
        /// <param name="standardMarketConventions">The standard Market conventions.</param>
        /// <param name="currencyLongName">The long name of the currency.</param>
        /// <exception cref="ArgumentNullException">Thrown, if one of the arguments (except <paramref name="currencyLongName"/>) is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="standardMarketConventions"/> is not completely defined.</exception>
        protected Currency(IdentifierString currencyName, ReadOnlyMarketConventions standardMarketConventions, IdentifierString currencyLongName = null)
        {
            if (currencyName == null)
            {
                throw new ArgumentNullException("currencyName");
            }
            if ((currencyName.IDString == null) || (currencyName.IDString.Length == 0))
            {
                throw new ArgumentException(String.Format(ExceptionMessages.ArgumentIsInvalid, "Currency name"), "currencyName");
            }
            if (standardMarketConventions == null)
            {
                throw new ArgumentNullException("standardMarketConventions");
            }
            m_Name = currencyName;
            m_LongName = (currencyLongName != null) ? currencyLongName : m_Name;
            MarketConventions = standardMarketConventions;
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the currency, i.e. the ISO code.
        /// </summary>
        /// <value>The ISO code of the currency.</value>
        public IdentifierString Name
        {
            get { return m_Name; }
        }

        /// <summary>Gets the long name of the currency.
        /// </summary>
        /// <value>The (perhaps language dependent) long name of the currency.</value>
        public IdentifierString LongName
        {
            get { return m_LongName; }
        }
        #endregion

        #endregion

        #region public methods

        #region IEquatable<Currency> Members

        /// <summary>Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        public bool Equals(Currency other)
        {
            if (other == null)
            {
                return false;
            }
            return (m_Name.IDString == other.m_Name.IDString);
        }
        #endregion

        /// <summary>Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return (string)m_Name;
        }
        #endregion
    }
}