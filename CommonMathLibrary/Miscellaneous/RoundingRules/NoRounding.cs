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
using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.Miscellaneous
{
    /// <summary>Represents the trivial implementation of <see cref="IRoundingRule"/> that do not round any number.
    /// </summary>
    internal struct NoRounding : IRoundingRule
    {
        #region private static (readonly) members

        /// <summary>The language independent name of the rounding rule.
        /// </summary>
        private static readonly IdentifierString sm_Name = new IdentifierString("NoRounding");
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the rounding rule.
        /// </summary>
        /// <value>The language independent name of the rounding rule.</value>
        public IdentifierString Name
        {
            get { return NoRounding.sm_Name; }
        }

        /// <summary>Gets the long name of the rounding rule.
        /// </summary>
        /// <value>The language dependent long name of the rounding rule.</value>
        public IdentifierString LongName
        {
            get { return RoundingRuleNames.NoRounding.ToIdentifierString(); }
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
            return rawValue;
        }
        #endregion

        /// <summary>Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> containing a fully qualified type name.</returns>
        public override string ToString()
        {
            return RoundingRuleNames.NoRounding;
        }
        #endregion
    }
}