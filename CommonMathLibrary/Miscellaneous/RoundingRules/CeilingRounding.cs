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
    /// <summary>The rounding rule that returns the next multiple of a specified <c>unit</c> with respect to a double-precision floating-point argument. If the argument is positiv the result will
    /// be greater or equal than this number; otherwise the result will be smaller.
    /// </summary>
    public class CeilingRounding : IRoundingRule
    {
        #region nested classes

        /// <summary>Serves as factory for <see cref="CeilingRounding"/> instances.
        /// </summary>
        public class Factory
        {
            /// <summary>A ceiling rounding with unit = 10.0.
            /// </summary>
            public readonly CeilingRounding Unit10 = new CeilingRounding(10.0);

            /// <summary>Creates a specified <see cref="CeilingRounding"/> instance.
            /// </summary>
            /// <param name="unit">The unit to take into account.</param>
            /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="unit"/> is negative.</exception>
            public CeilingRounding Create(double unit)
            {
                return new CeilingRounding(unit);
            }
        }
        #endregion

        #region private/public (readonly) members

        /// <summary>The language independent name of the rounding rule.
        /// </summary>
        public static readonly IdentifierString RoundingName = new IdentifierString("CeilingRounding");

        /// <summary>The unit to take into account, for example <c>0.05</c> for five cent or <c>1,000</c> for rounding with respect to thousandths of US$, EUR$ etc.
        /// </summary>
        public readonly double Unit;

        /// <summary>The language independent name of the rounding rule, i.e. "CeilingRounding:Unit={Unit}".
        /// </summary>
        private readonly IdentifierString m_Name;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="CeilingRounding"/> class.
        /// </summary>
        /// <param name="unit">The unit to take into account.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="unit"/> is negative.</exception>
        public CeilingRounding(double unit)
        {
            if (unit < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(unit), string.Format(CultureInfo.InvariantCulture, ExceptionMessages.ArgumentOutOfRangeGreaterEqual, unit, 0));
            }
            Unit = unit;
            m_Name = new IdentifierString("CeilingRounding: Unit=" + Unit.ToString());
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the rounding rule, i.e. "CeilingRounding:Unit={Unit}".
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
            double div = rawValue / Unit;
            double fraction = Math.Truncate(div);

            if (rawValue >= 0)
            {
                return Unit * (fraction + ((div - fraction) > 0 ? 1 : 0));
            }
            return Unit * (fraction - ((div - fraction) < 0 ? 1 : 0));
        }
        #endregion

        /// <summary>Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.</returns>
        public override string ToString()
        {
            return String.Format(RoundingRuleNames.CeilingRounding, Unit);
        }
        #endregion

        #region public static methods

        /// <summary>Creates a specified <see cref="CeilingRounding"/> instance.
        /// </summary>
        /// <param name="unit">The unit to take into account.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="unit"/> is negative.</exception>
        public static CeilingRounding Create(double unit)
        {
            return new CeilingRounding(unit);
        }
        #endregion
    }
}