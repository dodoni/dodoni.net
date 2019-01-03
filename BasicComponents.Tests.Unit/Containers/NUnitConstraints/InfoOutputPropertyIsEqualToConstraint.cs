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
using NUnit.Framework.Constraints;

namespace Dodoni.BasicComponents.Containers
{
    /// <summary>The constraint class that provides comparision methods for <see cref="InfoOutputProperty"/> objects with respect to the NUnit framework.
    /// </summary>
    internal class InfoOutputPropertyIsEqualToConstraint : Constraint
    {
        #region private members

        /// <summary>The expected <see cref="InfoOutputProperty"/> object.
        /// </summary>
        private InfoOutputProperty m_ExpectedInfoOutputProperty;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="InfoOutputPropertyIsEqualToConstraint"/> class.
        /// </summary>
        /// <param name="expectedInfoOutputProperty">The expected <see cref="InfoOutputProperty"/> object.</param>
        internal InfoOutputPropertyIsEqualToConstraint(InfoOutputProperty expectedInfoOutputProperty)
        {
            m_ExpectedInfoOutputProperty = expectedInfoOutputProperty;
        }
        #endregion

        #region public methods

        /// <summary>Applies the constraint to an actual value, returning a ConstraintResult.
        /// </summary>
        /// <typeparam name="TActual"></typeparam>
        /// <param name="actual">The value to be tested</param>
        /// <returns>A new <see cref="ConstraintResult"/> object.</returns>
        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            if ((typeof(TActual) != typeof(InfoOutputProperty)))
            {
                return new ConstraintResult(this, actual, isSuccess: false);
            }
            if ((m_ExpectedInfoOutputProperty == null) && (actual == null))
            {
                return new ConstraintResult(this, actual, isSuccess: true);
            }
            var actualInfoOutputProperty = actual as InfoOutputProperty;

            return new ConstraintResult(this, actual, (m_ExpectedInfoOutputProperty.Name.Equals(actualInfoOutputProperty.Name)) && (m_ExpectedInfoOutputProperty.Value.Equals(actualInfoOutputProperty.Value)));
        }
        #endregion
    }
}