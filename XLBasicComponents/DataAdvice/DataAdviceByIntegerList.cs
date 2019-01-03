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

namespace Dodoni.XLBasicComponents
{
    /// <summary>Represents a implementation of the <see cref="IExcelDataAdvice"/> interface which is represented by a set of <see cref="System.Int32"/> numbers.
    /// </summary>
    internal class DataAdviceByIntegerList : IExcelDataAdvice
    {
        #region private members

        /// <summary>The name of the <see cref="IExcelDataAdvice"/> object.
        /// </summary>
        private IdentifierString m_Name;

        /// <summary>The number of integers.
        /// </summary>
        private int m_NumberOfIntegers;

        /// <summary>The index of the first integer.
        /// </summary>
        private int m_StartIndex;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="DataAdviceByIntegerList"/> class.
        /// </summary>
        /// <param name="name">The name, can be <c>null</c>.</param>
        /// <param name="numberOfIntegers">The number of integers to show.</param>
        /// <param name="firstInteger">The first integer.</param>
        internal DataAdviceByIntegerList(string name, int numberOfIntegers, int firstInteger = 0)
        {
            if (name == null)
            {
                m_Name = IdentifierString.Empty;
            }
            else
            {
                m_Name = new IdentifierString(name);
            }
            if (numberOfIntegers < 0)
            {
                throw new ArgumentException(String.Format(ExceptionMessages.ArgumentOutOfConstraint, "number of integers [" + numberOfIntegers + "]", ">0"));
            }
            m_NumberOfIntegers = numberOfIntegers;
            m_StartIndex = firstInteger;
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the current instance.
        /// </summary>
        /// <value>The language independent name of the current instance.</value>
        public IdentifierString Name
        {
            get { return m_Name; }
        }

        /// <summary>Gets the long name of the current instance.
        /// </summary>
        /// <value>The language dependent long name of the current instance.</value>
        public IdentifierString LongName
        {
            get { return m_Name; }
        }
        #endregion

        #endregion

        #region public methods

        #region IExcelDataAdvice Members

        /// <summary>Gets possible outcome, i.e. advisable input for a specific Excel cell.
        /// </summary>
        /// <returns>A collection of possible outcome, i.e. advisable input for a specific Excel cell, perhaps empty. <c>null</c> is not allowed.
        /// </returns>
        public IEnumerable<string> GetPossibleOutcome()
        {
            for (int j = m_StartIndex; j < m_StartIndex + m_NumberOfIntegers; j++)
            {
                yield return j.ToString();
            }
        }
        #endregion

        /// <summary>Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return m_Name.String;
        }
        #endregion
    }
}