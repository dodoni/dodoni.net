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
using System.Globalization;

using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.Miscellaneous
{
    /// <summary>Represents the banker's rounding, i.e. follows IEEE Standard 754, section 4.
    /// </summary>
    public class BankersRounding : IRoundingRule
    {
        #region nested classes

        /// <summary>Serves as factory for <see cref="BankersRounding"/> instances.
        /// </summary>
        public class Factory
        {
            /// <summary>A banker's rounding with respect to two digita.
            /// </summary>
            public readonly BankersRounding TwoDigits = new BankersRounding(2);

            /// <summary>Creates a specified <see cref="BankersRounding"/> instance.
            /// </summary>
            /// <param name="numberOfDigits">The number of digits.</param>
            /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="numberOfDigits"/> is negative.</exception>
            public BankersRounding Create(int numberOfDigits)
            {
                return new BankersRounding(numberOfDigits);
            }
        }
        #endregion

        #region private/public (readonly) members

        /// <summary>The language independent name of the rounding rule.
        /// </summary>
        public static readonly IdentifierString RoundingName = new IdentifierString("BankersRounding");

        /// <summary>The language independent name of the rounding rule, i.e. "BankersRounding:Digits={Number of Digits}".
        /// </summary>
        private readonly IdentifierString m_Name;

        /// <summary>The number of digits to take into account.
        /// </summary>
        public readonly int NumberOfDigits;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="BankersRounding"/> class.
        /// </summary>
        /// <param name="numberOfDigits">The number of digits.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="numberOfDigits"/> is negative.</exception>
        public BankersRounding(int numberOfDigits)
        {
            if (numberOfDigits < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(numberOfDigits), string.Format(CultureInfo.InvariantCulture, ExceptionMessages.ArgumentOutOfRangeGreaterEqual, numberOfDigits, 0));
            }
            NumberOfDigits = numberOfDigits;
            m_Name = new IdentifierString(String.Format("BankersRounding:Digits={0}", numberOfDigits));
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the rounding rule, i.e. "BankersRounding:Digits={Number of Digits}".
        /// </summary>
        /// <value>The language independent name of the rounding rule.</value>
        public IdentifierString Name
        {
            get { return m_Name; }
        }

        /// <summary>Gets the long name of the rounding rule.
        /// </summary>
        /// <value>The language dependent long name of the rounding rule.</value>
        public IdentifierString LongName
        {
            get { return ToString().ToIdentifierString(); }
        }
        #endregion

        #endregion

        #region public methods

        #region IRoundingRule Members

        /// <summary>Gets the rounded value with respect to a specific argument.
        /// </summary>
        /// <param name="rawValue">The raw (input) value.</param>
        /// <returns>The rounded value of <paramref name="rawValue" /> with respect to the rounding rule that is represented by the current instance.</returns>
        public double GetValue(double rawValue)
        {
            return Math.Round(rawValue, NumberOfDigits);
        }
        #endregion

        /// <summary>Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.</returns>
        public override string ToString()
        {
            return String.Format(RoundingRuleNames.BankersRounding, NumberOfDigits);
        }
        #endregion

        #region public static methods

        /// <summary>Creates a specified <see cref="BankersRounding"/> instance.
        /// </summary>
        /// <param name="numberOfDigits">The number of digits.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="numberOfDigits"/> is negative.</exception>
        public static BankersRounding Create(int numberOfDigits)
        {
            return new BankersRounding(numberOfDigits);
        }
        #endregion
    }
}