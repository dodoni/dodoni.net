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
using System.Linq;
using System.Collections.Generic;

using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Utilities;
using Dodoni.BasicComponents.Containers;

namespace Dodoni.XLBasicComponents
{
    /// <summary>Serves as factory for <see cref="IExcelDataAdvice"/> objects which contain data/input advice with respect to a specific (Excel) cell, 
    /// for example the name of holiday calendars, the name of business day conventions etc. which can be choosen by the user.
    /// </summary>
    public static class ExcelDataAdvice
    {
        #region nested classes

        /// <summary>Handles the data advice initialization event which will be raised before the GUI tries to get (optional) possible outcome of a specific Excel cell.
        /// </summary>
        /// <example>The name of currencies, the name of (standard) frequencies, the name of elements of <see cref="ExcelPool"/>
        /// which are of a specific type - for example numerical integration, optimizer etc.</example>
        public class InitializeEventArgs : EventArgs
        {
            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="InitializeEventArgs"/> class.
            /// </summary>
            internal InitializeEventArgs()
            {
            }
            #endregion

            #region public methods

            /// <summary>Registers a specific <see cref="IExcelDataAdvice"/> object, i.e. stores
            /// additional information for a specific Excel cell property to improve the usability.
            /// </summary>
            /// <param name="value">The <see cref="IExcelDataAdvice"/> object to register, i.e. represents possible user input
            /// of a specific Excel cell to improve the useability.</param>
            /// <returns>A value indicating whether <paramref name="value"/> has been inserted.</returns>
            /// <exception cref="ArgumentNullException">Thrown, if <paramref name="value"/> is <c>null</c>.</exception>
            public ItemAddedState Add(IExcelDataAdvice value)
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                return ExcelDataAdvice.Pool.Add(value);
            }
            #endregion
        }

        /// <summary>Contains methods for creating the <see cref="System.String"/> representation of a drop down list which represents some data advice, 
        /// <seealso cref="ExcelLowLevel.CreateDropdownList(ExcelDna.Integration.ExcelReference, int, int, string)"/>.
        /// </summary>
        internal class StringRepresentation
        {
            #region private members

            /// <summary>The XML tag in the configuration file for the 'data advice separator' combobox.
            /// </summary>
            private const string sm_DataAdviceSeparatorConfigKey = "DataAdviceSeparator";

            /// <summary>Represents the <see cref="System.String"/> representation of the separator needed to create a drop down list which represents some data advice.
            /// </summary>
            private string m_DropDownSeparator = ";";
            #endregion

            #region internal constructor

            /// <summary>Initializes a new instance of the <see cref="StringRepresentation"/> class.
            /// </summary>
            internal StringRepresentation()
            {
                m_DropDownSeparator = GetDropDownSeparatorFromConfigFile();
            }
            #endregion

            #region internal properties

            /// <summary>Gets the <see cref="System.String"/> representation of the separator needed to create a drop down list which represents some data advice.
            /// </summary>
            /// <value>The <see cref="System.String"/> representation of the separator needed to create a drop down list which represents some data advice.</value>
            internal string DropDownSeparator
            {
                get { return m_DropDownSeparator; }
            }
            #endregion

            #region internal methods

            /// <summary>Stores the <see cref="System.String"/> representation of the separator needed to create a drop down list which represents some data advice in the config file.
            /// </summary>
            /// <param name="dropDownSeparator">The <see cref="System.String"/> representation of the separator needed to create a drop down list which represents some data advice.</param>
            internal void StoreDropDownSeparator(string dropDownSeparator)
            {
                if (dropDownSeparator == null)
                {
                    throw new ArgumentNullException("dropDownSeparator");
                }
                m_DropDownSeparator = dropDownSeparator;
                ExcelAddIn.Configuration.GeneralSettings.SetValue(sm_DataAdviceSeparatorConfigKey, dropDownSeparator);
            }

            /// <summary>Gets the <see cref="System.String"/> representation of the separator needed to create a drop down list which represents some data advice from the configuration file.
            /// </summary>
            /// <returns>The loaed <see cref="System.String"/> representation of the separator needed to create a drop down list which represents some data advice; or some default value.</returns>
            internal string GetDropDownSeparatorFromConfigFile()
            {
                if (ExcelAddIn.Configuration.GeneralSettings.TryGetValue(sm_DataAdviceSeparatorConfigKey, out m_DropDownSeparator) == false)
                {
                    m_DropDownSeparator = ",";  // English configuration is standard
                }
                return m_DropDownSeparator;
            }

            /// <summary>Gets the <see cref="System.String"/> representation of a specific Excel drop down list, i.e. a semicolon separated <see cref="System.String"/> 
            /// representation of the enumeration element names.
            /// </summary>
            /// <param name="dataAdvice">A data advice for a specific Excel cell, i.e. possible outcome.</param>
            /// <returns>A semicolon separated <see cref="System.String"/> representation of the enumeration element names or <c>null</c>.</returns>
            /// <remarks>In Excel a semicolon separated <see cref="System.String"/> representation of the possible cell inputs will be used to
            /// create a drop down list to improve the useability, therefore the list will be converted to a <see cref="System.String"/>.</remarks>
            internal string Create(IExcelDataAdvice dataAdvice)
            {
                if (dataAdvice != null)
                {
                    return String.Join(m_DropDownSeparator, dataAdvice.GetPossibleOutcome());
                }
                return null;
            }

            /// <summary>Gets the <see cref="System.String"/> representation of some Excel drop down list, i.e. a semicolon separated <see cref="System.String"/> 
            /// of the enumeration element names.
            /// </summary>
            /// <param name="possibleOutcome">The possible outcome.</param>
            /// <returns>A semicolon separated <see cref="System.String"/> representation of the enumeration element names.</returns>
            internal string Create(IEnumerable<String> possibleOutcome)
            {
                if ((possibleOutcome != null) && (possibleOutcome.Count() > 0))
                {
                    return String.Join(m_DropDownSeparator, possibleOutcome);
                }
                return null;
            }

            /// <summary>Gets the <see cref="System.String"/> representation of some Excel drop down list, i.e. a semicolon separated <see cref="System.String"/> 
            /// of the enumeration element names.
            /// </summary>
            /// <typeparam name="T">The type of the possible outcomes.</typeparam>
            /// <param name="possibleOutcome">The possible outcome.</param>
            /// <returns>A semicolon separated <see cref="System.String"/> representation of the enumeration element names.</returns>
            internal string Create<T>(IEnumerable<EnumString<T>> possibleOutcome)
                where T : struct, IComparable, IConvertible, IFormattable
            {
                string dropDownListAsString = null;
                if ((possibleOutcome != null) && (possibleOutcome.Count() > 0))
                {
                    StringBuilder strBuilder = new StringBuilder();
                    foreach (EnumString<T> str in possibleOutcome)
                    {
                        if (strBuilder.Length > 0)
                        {
                            strBuilder.Append(m_DropDownSeparator);
                        }
                        strBuilder.Append(str.StringRepresentation.String);
                    }
                    dropDownListAsString = strBuilder.ToString();
                }
                return dropDownListAsString;
            }
            #endregion
        }
        #endregion

        #region public/internal members

        /// <summary>Represents a pool of <see cref="IExcelDataAdvice"/> objects.
        /// </summary>
        public static readonly ExcelDataAdvicePool Pool = new ExcelDataAdvicePool();

        /// <summary>Serves as configuration and factory for the <see cref="System.String"/> representation needed for the drop-down lists used for data advice.
        /// </summary>
        internal static readonly StringRepresentation DropDownRepresentation = new StringRepresentation();
        #endregion

        #region public static methods

        /// <summary>Creates a new <see cref="IExcelDataAdvice"/> object.
        /// </summary>
        /// <param name="possibleOutcome">The collection of possible outcome, i.e. the data advice.</param>
        /// <param name="name">The name of the <see cref="IExcelDataAdvice"/>, if <c>null</c>, the name will be set
        /// to <see cref="System.String.Empty"/>.</param>
        /// <returns>A <see cref="IExcelDataAdvice"/> object which represents the <paramref name="possibleOutcome"/>.</returns>
        public static IExcelDataAdvice Create(IEnumerable<IdentifierString> possibleOutcome, string name = null)
        {
            if (possibleOutcome == null)
            {
                return null;
            }
            return new DataAdviceByCollection(name, GetAsString<IdentifierString>(possibleOutcome, idString => idString.String));
        }

        /// <summary>Creates a new <see cref="IExcelDataAdvice"/> object.
        /// </summary>
        /// <param name="possibleOutcome">The collection of possible outcome, i.e. the data advice.</param>
        /// <param name="name">The name of the <see cref="IExcelDataAdvice"/>, if <c>null</c>, the name will be set
        /// to <see cref="System.String.Empty"/>.</param>
        /// <returns>A <see cref="IExcelDataAdvice"/> object which represents the <paramref name="possibleOutcome"/>.</returns>
        public static IExcelDataAdvice Create(IEnumerable<string> possibleOutcome, string name = null)
        {
            if (possibleOutcome == null)
            {
                return null;
            }
            return new DataAdviceByCollection(name, possibleOutcome);
        }

        /// <summary>Creates a new <see cref="IExcelDataAdvice"/> object.
        /// </summary>
        /// <typeparam name="T">The type of the possible outcome.</typeparam>
        /// <param name="possibleOutcome">The collection of possible outcome, i.e. the data advice.</param>
        /// <param name="name">The name of the <see cref="IExcelDataAdvice"/>, if <c>null</c>, the name will be set
        /// to <see cref="System.String.Empty"/>.</param>
        /// <returns>A <see cref="IExcelDataAdvice"/> object which represents the <paramref name="possibleOutcome"/>.</returns>
        public static IExcelDataAdvice Create<T>(IEnumerable<T> possibleOutcome, string name = null)
            where T : IIdentifierNameable
        {
            if (possibleOutcome == null)
            {
                return null;
            }
            return new DataAdviceByCollection(name, GetAsString<T>(possibleOutcome, value => value.Name.String));
        }

        /// <summary>Creates a new <see cref="IExcelDataAdvice"/> object.
        /// </summary>
        /// <typeparam name="T">The type of the possible outcome.</typeparam>
        /// <param name="possibleOutcome">The collection of possible outcome, i.e. the data advice.</param>
        /// <param name="toString">A function which returns the <see cref="System.String"/> representation of a specific <typeparamref name="T"/> instance.</param>
        /// <param name="name">The name of the <see cref="IExcelDataAdvice"/>, if <c>null</c>, the name will be set
        /// to <see cref="System.String.Empty"/>.</param>
        /// <returns>A <see cref="IExcelDataAdvice"/> object which represents the <paramref name="possibleOutcome"/>.</returns>
        public static IExcelDataAdvice Create<T>(IEnumerable<T> possibleOutcome, Func<T, string> toString, string name = null)
        {
            return new DataAdviceByCollection(name, GetAsString(possibleOutcome, toString));
        }

        /// <summary>Creates a new <see cref="IExcelDataAdvice"/> object.
        /// </summary>
        /// <param name="numberOfIntegers">The number of integers to show.</param>
        /// <param name="firstInteger">The first integer.</param>
        /// <param name="name">The name of the <see cref="IExcelDataAdvice"/>, if <c>null</c>, the name will be set
        /// to <see cref="System.String.Empty"/>.</param>
        /// <returns>A <see cref="IExcelDataAdvice"/> object which represents the integers <paramref name="firstInteger"/>, <paramref name="firstInteger"/> +1, ..., <paramref name="firstInteger"/> + <paramref name="numberOfIntegers"/>.</returns>
        public static IExcelDataAdvice Create(int numberOfIntegers, int firstInteger = 0, string name = null)
        {
            return new DataAdviceByIntegerList(name, numberOfIntegers, firstInteger);
        }

        /// <summary>Converts a collection of possible outcome that may starts with a digit but are not considered as a number to a collection of <see cref="System.String"/>
        /// objects that starts with an apostrophe if the original string starts with a digit.
        /// </summary>
        /// <param name="possibleOutcome">The possible outcome, i.e. the Excel data advice as a collection of <see cref="System.String"/> objects.</param>
        /// <returns></returns>
        /// <remarks>Excel interprete some strings as Date etc., for example "1/1" is shown as 1. January in Excel. Therefore this method
        /// checks whether the first character is a digit and a "'" will be added, thus 30/360 is converted to '30/360 etc.</remarks>
        public static IEnumerable<string> AsWithDigitStartingPossibleOutcomeStrings(this IEnumerable<string> possibleOutcome)
        {
            foreach (string posibleOutcome in possibleOutcome)
            {
                if (Char.IsDigit(posibleOutcome[0]) == true)
                {
                    yield return "'" + posibleOutcome;
                }
                else
                {
                    yield return posibleOutcome;
                }
            }
        }
        #endregion

        #region internal methods

        /// <summary>Gets the <see cref="System.String"/> representation of a specific Excel drop down list, i.e. a semicolon separated <see cref="System.String"/> 
        /// representation of the enumeration element names.
        /// </summary>
        /// <param name="dataAdvice">A data advice for a specific Excel cell, i.e. possible outcome.</param>
        /// <returns>A semicolon separated <see cref="System.String"/> representation of the enumeration element names or <c>null</c>.</returns>
        /// <remarks>In Excel a semicolon separated <see cref="System.String"/> representation of the possible cell inputs will be used to
        /// create a drop down list to improve the useability, therefore the list will be converted to a <see cref="System.String"/>.</remarks>
        internal static string AsExcelDropDownListString(this IExcelDataAdvice dataAdvice)
        {
            return DropDownRepresentation.Create(dataAdvice);
        }

        /// <summary>Gets the <see cref="System.String"/> representation of some Excel drop down list, i.e. a semicolon separated <see cref="System.String"/> 
        /// of the enumeration element names.
        /// </summary>
        /// <param name="possibleOutcome">The possible outcome.</param>
        /// <returns>A semicolon separated <see cref="System.String"/> representation of the enumeration element names.</returns>
        internal static string AsExcelDropDownListString(this IEnumerable<String> possibleOutcome)
        {
            return DropDownRepresentation.Create(possibleOutcome);
        }

        /// <summary>Gets the <see cref="System.String"/> representation of some Excel drop down list, i.e. a semicolon separated <see cref="System.String"/> 
        /// of the enumeration element names.
        /// </summary>
        /// <typeparam name="T">The type of the possible outcomes.</typeparam>
        /// <param name="possibleOutcome">The possible outcome.</param>
        /// <returns>A semicolon separated <see cref="System.String"/> representation of the enumeration element names.</returns>
        internal static string AsExcelDropDownListString<T>(this IEnumerable<EnumString<T>> possibleOutcome)
            where T : struct,IComparable, IConvertible, IFormattable
        {
            return DropDownRepresentation.Create<T>(possibleOutcome);
        }
        #endregion

        #region private methods

        /// <summary>Gets the <see cref="System.String"/> representation of a specific list of objects.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="possibleOutcome">The values.</param>
        /// <param name="toString">A delegate which returns the <see cref="System.String"/> representation of a specific <typeparamref name="T"/> instance.</param>
        /// <returns>A collection which contains the <see cref="System.String"/> representations of <paramref name="possibleOutcome"/>.</returns>
        private static IEnumerable<string> GetAsString<T>(IEnumerable<T> possibleOutcome, Func<T, string> toString)
        {
            foreach (T value in possibleOutcome)
            {
                yield return toString(value);
            }
        }
        #endregion
    }
}