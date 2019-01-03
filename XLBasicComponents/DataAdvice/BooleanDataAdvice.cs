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
    /// <summary>Serves as <see cref="IExcelDataAdvice"/> implementation which contains <c>true</c> and <c>false</c>.
    /// </summary>
    internal class BooleanDataAdvice : IExcelDataAdvice
    {
        #region private members

        /// <summary>The name of the <see cref="IExcelDataAdvice"/> object.
        /// </summary>
        private IdentifierString m_Name = new IdentifierString("Booleans");

        /// <summary>Represents the Boolean value <c>true</c> in its <see cref="System.String"/> representation.
        /// </summary>
        private IdentifierString m_TrueString;

        /// <summary>The XML tag in the configuration file for the 'TRUE=' combobox.
        /// </summary>
        private const string m_TrueExcelCellRepresentationConfigKey = "TrueExcelCellRepresentation";

        /// <summary>Represents the Boolean value <c>false</c> in its <see cref="System.String"/> representation.
        /// </summary>
        private IdentifierString m_FalseString;

        /// <summary>The XML tag in the configuration file for the 'FALSE=' combobox.
        /// </summary>
        private const string m_FalseExcelCellRepresentationConfigKey = "FalseExcelCellRepresentation";

        /// <summary>The possible outcome.
        /// </summary>
        private List<string> m_PossibleOutcome;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="BooleanDataAdvice"/> class.
        /// </summary>
        internal BooleanDataAdvice()
        {
            m_PossibleOutcome = new List<string>() { "TRUE", "FALSE" };
            RestoreTrueString();
            RestoreFalseString();
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

        /// <summary>Gets the <see cref="IdentifierString"/> representation of the Boolean value <c>true</c>.
        /// </summary>
        /// <value>The <see cref="IdentifierString"/> representation of the Boolean value <c>true</c>.</value>
        public IdentifierString TrueString
        {
            get { return m_TrueString; }
        }

        /// <summary>Gets the <see cref="IdentifierString"/> representation of the Boolean value <c>false</c>.
        /// </summary>
        /// <value>The <see cref="IdentifierString"/> representation of the Boolean value <c>false</c>.</value>
        public IdentifierString FalseString
        {
            get { return m_FalseString; }
        }
        #endregion

        #region public methods

        #region IExcelDataAdvice Members

        /// <summary>Gets possible outcome, i.e. advisable input for a specific Excel cell.
        /// </summary>
        /// <returns>A collection of possible outcome, i.e. advisable input for a specific Excel cell, perhaps empty. <c>null</c> is not allowed.
        /// </returns>
        public IEnumerable<string> GetPossibleOutcome()
        {
            return m_PossibleOutcome;
        }
        #endregion

        /// <summary>Sets the <see cref="System.String"/> representation of the Boolean value <c>true</c> and store the value in the config file.
        /// </summary>
        /// <param name="trueString">The <see cref="System.String"/> representation of the Boolean value <c>true</c>.</param>
        public void SetTrueString(string trueString)
        {
            if (trueString == null)
            {
                trueString = String.Empty;
            }
            m_TrueString = new IdentifierString(trueString);
            m_PossibleOutcome[0] = trueString;
            ExcelAddIn.Configuration.GeneralSettings.SetValue(m_TrueExcelCellRepresentationConfigKey, trueString);
        }

        /// <summary>Restores the <see cref="System.String"/> representation of the Boolean value <c>true</c> via the config file.
        /// </summary>
        /// <returns>The restored <see cref="System.String"/> representation of the Boolean value <c>true</c>.</returns>
        public string RestoreTrueString()
        {
            string trueString;
            if (ExcelAddIn.Configuration.GeneralSettings.TryGetValue(m_TrueExcelCellRepresentationConfigKey, out trueString) == true)
            {
                m_TrueString = new IdentifierString(trueString);
            }
            else
            {
                m_TrueString = new IdentifierString("TRUE");
            }
            m_PossibleOutcome[0] = m_TrueString.String;
            return m_TrueString.String;
        }

        /// <summary>Sets the <see cref="System.String"/> <see cref="System.String"/> representation of the Boolean value <c>false</c> and store the value in the config file.
        /// </summary>
        /// <param name="falseString">The <see cref="System.String"/> representation of the Boolean value <c>false</c>.</param>
        public void SetFalseString(string falseString)
        {
            if (falseString == null)
            {
                falseString = String.Empty;
            }
            m_FalseString = new IdentifierString(falseString);
            m_PossibleOutcome[1] = falseString;
            ExcelAddIn.Configuration.GeneralSettings.SetValue(m_FalseExcelCellRepresentationConfigKey, falseString);
        }

        /// <summary>Restores the <see cref="System.String"/> <see cref="System.String"/> representation of the Boolean value <c>false</c> via the config file.
        /// </summary>
        /// <returns>The restored <see cref="System.String"/> <see cref="System.String"/> representation of the Boolean value <c>false</c>.</returns>
        public string RestoreFalseString()
        {
            string falseString;
            if (ExcelAddIn.Configuration.GeneralSettings.TryGetValue(m_FalseExcelCellRepresentationConfigKey, out falseString) == true)
            {
                m_FalseString = new IdentifierString(falseString);
            }
            else
            {
                m_FalseString = new IdentifierString("FALSE");
            }
            m_PossibleOutcome[1] = m_FalseString.String;
            return m_FalseString.String;
        }

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
