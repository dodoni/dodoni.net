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

namespace Dodoni.MathLibrary.Miscellaneous
{
    /// <summary>Rounds a double-precision floating-point value to the nearest integral value.
    /// </summary>
    /// <remarks>If the fractional component of a is halfway between two integers, one of which is even and the other odd, then the even number is returned.</remarks>
    internal class NearestIntegralValueRounding : IRoundingRule
    {
        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="NearestIntegralValueRounding" /> class.
        /// </summary>
        internal NearestIntegralValueRounding()
        {
            Name = new IdentifierString("Nearest Integral value");
            LongName = new IdentifierString(RoundingRuleNames.NearestIntegralValueRounding);
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the long name of the current instance.
        /// </summary>
        /// <value>The (perhaps) language dependent long name of the current instance.</value>
        public IdentifierString LongName
        {
            get;
            private set;
        }

        /// <summary>Gets the name of the current instance.
        /// </summary>
        /// <value>The language independent name of the current instance.</value>
        public IdentifierString Name
        {
            get;
            private set;
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
            return Math.Round(rawValue);
        }
        #endregion

        /// <summary>Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.</returns>
        public override string ToString()
        {
            return RoundingRuleNames.NearestIntegralValueRounding;
        }
        #endregion
    }
}