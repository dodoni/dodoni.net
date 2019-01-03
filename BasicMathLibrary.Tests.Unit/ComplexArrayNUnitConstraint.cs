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
using System.Numerics;
using System.Collections.Generic;

using NUnit.Framework.Constraints;

namespace Dodoni.MathLibrary.Basics
{
    /// <summary>Serves as constraint for the comparisson of two <see cref="Complex"/> arrays taken into account a specified tolerance.
    /// </summary>
    /// <remarks>It seems that in NUnit <code>Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(1E-5));</code> does not work correctly for complex arrays.</remarks>
    internal class ComplexArrayNUnitConstraint : Constraint
    {
        #region private members

        /// <summary>The expected value.
        /// </summary>
        private IList<Complex> m_Expected;

        /// <summary>The number of elements to take into account.
        /// </summary>
        private int m_Length;

        /// <summary>The tolerance to take into account.
        /// </summary>
        private double m_Tolerance;

        /// <summary>The null-based index of the mismatch.
        /// </summary>
        private int m_IndexOfMismatch = -1;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="ComplexArrayNUnitConstraint"/> class.
        /// </summary>
        /// <param name="length">The number of elements to take into account.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="tolerance">The tolerance to take into account.</param>
        public ComplexArrayNUnitConstraint(int length, IList<Complex> expected, double tolerance = 1E-9)
        {
            m_Length = length;
            m_Expected = expected;
            m_Tolerance = tolerance;
        }

        /// <summary>Initializes a new instance of the <see cref="ComplexArrayNUnitConstraint"/> class.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        /// <param name="tolerance">The tolerance to take into account.</param>
        public ComplexArrayNUnitConstraint(IList<Complex> expected, double tolerance = 1E-9)
        {
            m_Expected = expected;
            m_Length = expected.Count;
            m_Tolerance = tolerance;
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
            if (actual is IList<Complex>)
            {
                var actualArray = (IList<Complex>)actual;
                for (int j = 0; j < m_Length; j++)
                {
                    if (Complex.Abs(m_Expected[j]) < 1E-9)
                    {
                        if (Complex.Abs(m_Expected[j] - actualArray[j]) >= m_Tolerance)
                        {
                            m_IndexOfMismatch = j;
                            //this.actual = actualArray[j];
                            return new ConstraintResult(this, actual, isSuccess: false);
                        }
                    }
                    else
                    {
                        if (Complex.Abs(actualArray[j] - m_Expected[j]) / Complex.Abs(m_Expected[j]) >= m_Tolerance)
                        {
                            // this.actual = actualArray[j];
                            m_IndexOfMismatch = j;
                            return new ConstraintResult(this, actual, isSuccess: false);
                        }
                    }
                }
                return new ConstraintResult(this, actual, isSuccess: true);
            }
            return new ConstraintResult(this, actual, isSuccess: false);
        }
        #endregion
    }
}