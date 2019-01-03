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
    /// <summary>The rounding rule that returns the argument truncated with respect to a specific number of digits.
    /// </summary>
    /// <example>The result of <c>value = -12.3456</c> with respect to a precison of <c>3</c> digits is <c>-12.345</c> which is greater than <c>value</c>. 
    /// The resulf of <c>value = 12.3456</c> is <c>12.345</c> which is less than <c>value</c>.</example>
    public class TruncateRounding : IRoundingRule
    {
        #region nested classes

        /// <summary>Serves as factory for <see cref="TruncateRounding"/> instances.
        /// </summary>
        public class Factory
        {
            /// <summary>A truncate rounding with respect to two digita.
            /// </summary>
            public readonly TruncateRounding TwoDigits = new TruncateRounding(2);

            /// <summary>Creates a specified <see cref="TruncateRounding"/> instance.
            /// </summary>
            /// <param name="numberOfDigits">The number of digits.</param>
            /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="numberOfDigits"/> is negative or greater than 15.</exception>
            public TruncateRounding Create(int numberOfDigits)
            {
                return new TruncateRounding(numberOfDigits);
            }
        }
        #endregion

        #region private/public readonly members

        /// <summary>The term which will be taken into account for the rounding, i.e. internaly we
        /// compute 'value - sm_RoundingTerm[NumberOfDigits]' and apply <see cref="System.Math.Round(double,MidpointRounding)"/>.
        /// </summary>
        private static double[] sm_RoundingTerm = { 5E-1, 5E-2, 5E-3, 5E-4, 5E-5, 5E-6, 5E-7, 5E-8, 5E-9, 5E-10, 5E-11, 5E-12, 5E-13, 5E-14, 5E-15, 5E-16 };

        /// <summary>The language independent name of the rounding rule.
        /// </summary>
        public static readonly IdentifierString RoundingName = new IdentifierString("TruncateRounding");

        /// <summary>The element of <see cref="sm_RoundingTerm"/> which is taken into account.
        /// </summary>
        /// <remarks>This member is used for performance reason only.</remarks>
        private readonly double m_RoundingTerm;

        /// <summary>The language independent name of the rounding rule, i.e. "TruncateRounding: Digits={Digits}".
        /// </summary>
        private readonly IdentifierString m_Name;

        /// <summary>The number of digits to take into account.
        /// </summary>
        public readonly int NumberOfDigits;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="TruncateRounding"/> class.
        /// </summary>
        /// <param name="numberOfDigits">The number of digits.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="numberOfDigits"/> is negative or greater than 15.</exception>
        public TruncateRounding(int numberOfDigits)
        {
            if ((numberOfDigits < 0) || (numberOfDigits >= sm_RoundingTerm.Length))
            {
                throw new ArgumentOutOfRangeException(nameof(numberOfDigits), string.Format(CultureInfo.InvariantCulture, ExceptionMessages.ArgumentOutOfRangeGreaterLessEqual, numberOfDigits, 0, sm_RoundingTerm.Length - 1));
            }
            NumberOfDigits = numberOfDigits;
            m_RoundingTerm = sm_RoundingTerm[numberOfDigits];
            m_Name = new IdentifierString("TruncateRounding: Digits=" + numberOfDigits.ToString());
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the rounding rule, i.e. "TruncateRounding: Digits={Digits}".
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
            get { return String.Format(RoundingRuleNames.TruncateRounding, NumberOfDigits).ToIdentifierString(); }
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
            // example: rawValue = 1.236, rounded with respect to two digits
            // = Math.Round ( 1.236 - 0.005, 2 ) = Math.Round( 1.231, 2 ) = 1.23
            if (rawValue >= 0.0)
            {
                return Math.Round(rawValue - m_RoundingTerm, NumberOfDigits, MidpointRounding.AwayFromZero);
            }
            return -Math.Round(-rawValue - m_RoundingTerm, NumberOfDigits, MidpointRounding.AwayFromZero);
        }
        #endregion

        /// <summary>Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.</returns>
        public override string ToString()
        {
            return String.Format(RoundingRuleNames.TruncateRounding, NumberOfDigits);
        }
        #endregion

        #region public static methods

        /// <summary>Creates a specified <see cref="TruncateRounding"/> instance.
        /// </summary>
        /// <param name="numberOfDigits">The number of digits.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="numberOfDigits"/> is negative or greater than 15.</exception>
        public static TruncateRounding Create(int numberOfDigits)
        {
            return new TruncateRounding(numberOfDigits);
        }
        #endregion
    }
}