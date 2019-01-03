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
using System.Collections.Generic;

namespace Dodoni.BasicComponents.Containers
{
    /// <summary>Represents information of a specific object, for example the result of a calibration, the parameters of some (curve) parametrization etc.
    /// </summary>
    /// <remarks>The output contains a collection of <see cref="System.Data.DataTable"/> instances with homogeneous information
    /// as well as a collection of properties, i.e. name/value pairs represented by a list of <see cref="InfoOutputProperty"/> instances.</remarks>
    public class InfoOutput : IEnumerable<InfoOutputPackage>
    {
        #region public (readonly) members

        /// <summary>The name of the 'general' category.
        /// </summary>
        public const string GeneralCategoryName = "General";
        #endregion

        #region private members

        /// <summary>The collection of properties and tables, where the key represents the category.
        /// </summary>
        private IdentifierStringDictionary<InfoOutputPackage> m_Values;

        /// <summary>The <see cref="IdentifierString"/> representation of <see cref="GeneralCategoryName"/>.
        /// </summary>
        private static readonly IdentifierString sm_GeneralCategoryIdentifierStringName = new IdentifierString(GeneralCategoryName);
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="InfoOutput"/> class.
        /// </summary>
        public InfoOutput()
        {
            m_Values = new IdentifierStringDictionary<InfoOutputPackage>(isReadOnlyExceptAdding: true);
        }
        #endregion

        #region public properties

        /// <summary>Gets the number of <see cref="InfoOutputPackage"/> objects.
        /// </summary>
        /// <value>The number of <see cref="InfoOutputPackage"/> objects.</value>
        public int Count
        {
            get { return m_Values.Count; }
        }

        /// <summary>Gets the category names.
        /// </summary>
        /// <value>The category names.</value>
        public IEnumerable<string> CategoryNames
        {
            get { return m_Values.Names; }
        }

        /// <summary>Gets the <see cref="Dodoni.BasicComponents.Containers.InfoOutputPackage"/> with the specified category name.
        /// </summary>
        /// <param name="categoryName">The name of the category, i.e. 'general' etc.</param>
        /// <value>The <see cref="InfoOutputPackage"/> object with respect to a specific category name.</value>
        public InfoOutputPackage this[string categoryName]
        {
            get { return m_Values[categoryName]; }
        }

        /// <summary>Gets the <see cref="Dodoni.BasicComponents.Containers.InfoOutputPackage"/> with the specified category name.
        /// </summary>
        /// <param name="categoryName">The name of the category, i.e. 'general' etc.</param>
        /// <value>The <see cref="InfoOutputPackage"/> object with respect to a specific category name.</value>
        public InfoOutputPackage this[IdentifierString categoryName]
        {
            get { return m_Values[categoryName]; }
        }
        #endregion

        #region public methods

        #region IEnumerable<InfoOutputPackage> Members

        /// <summary>Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<InfoOutputPackage> GetEnumerator()
        {
            return m_Values.GetEnumerator();
        }
        #endregion

        #region IEnumerable Members

        /// <summary>Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return m_Values.GetEnumerator();
        }
        #endregion

        /// <summary>Gets a <see cref="InfoOutputPackage"/> object with respect to a specific category name.
        /// </summary>
        /// <param name="categoryName">The name of the category, i.e. 'general' etc.</param>
        /// <param name="value">The value (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public bool TryGetPackage(string categoryName, out InfoOutputPackage value)
        {
            return m_Values.TryGetValue(categoryName, out value);
        }

        /// <summary>Gets a <see cref="InfoOutputPackage"/> object with respect to a specific category name.
        /// </summary>
        /// <param name="categoryName">The name of the category, i.e. 'general' etc.</param>
        /// <param name="value">The value (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public bool TryGetPackage(IdentifierString categoryName, out InfoOutputPackage value)
        {
            return m_Values.TryGetValue(categoryName, out value);
        }

        /// <summary>Gets the <see cref="InfoOutputPackage"/> object with respect to the category <see cref="GeneralCategoryName"/>.
        /// </summary>
        /// <returns>The <see cref="InfoOutputPackage"/> object with respect to the <see cref="GeneralCategoryName"/>.</returns>
        /// <remarks>If no there is no general <see cref="InfoOutputPackage"/> object available it will be created, stored internally and returned.</remarks>
        public InfoOutputPackage GetGeneralPackage()
        {
            if (m_Values.TryGetValue(GeneralCategoryName, out InfoOutputPackage generalInfoOutputPackage) == true)
            {
                return generalInfoOutputPackage;
            }
            generalInfoOutputPackage = new InfoOutputPackage(GeneralCategoryName);
            m_Values.Add(generalInfoOutputPackage.CategoryName, generalInfoOutputPackage);

            return generalInfoOutputPackage;
        }

        /// <summary>Acquires a specific <see cref="InfoOutputPackage"/> object with respect to a specified category name; if no package available with the
        /// desired name a new <see cref="InfoOutputPackage"/> object will be created, stored internally and returned to the caller.
        /// </summary>
        /// <param name="categoryName">The name of the category, i.e. 'general' etc.</param>
        /// <returns>The <see cref="InfoOutputPackage"/> object with respect to the <paramref name="categoryName"/>.</returns>
        public InfoOutputPackage AcquirePackage(string categoryName)
        {
            if (m_Values.TryGetValue(categoryName, out InfoOutputPackage infoOutputPackage) == true)
            {
                return infoOutputPackage;
            }
            infoOutputPackage = new InfoOutputPackage(categoryName);
            m_Values.Add(infoOutputPackage.CategoryName, infoOutputPackage);
            return infoOutputPackage;
        }

        /// <summary>Acquires a specific <see cref="InfoOutputPackage"/> object with respect to a specified category name; if no package available with the
        /// desired name a new <see cref="InfoOutputPackage"/> object will be created, stored internally and returned to the caller.
        /// </summary>
        /// <param name="categoryName">The name of the category, i.e. 'general' etc.</param>
        /// <returns>The <see cref="InfoOutputPackage"/> object with respect to the <paramref name="categoryName"/>.</returns>
        public InfoOutputPackage AcquirePackage(IdentifierString categoryName)
        {
            if (m_Values.TryGetValue(categoryName, out InfoOutputPackage infoOutputPackage) == true)
            {
                return infoOutputPackage;
            }
            infoOutputPackage = new InfoOutputPackage(categoryName);
            m_Values.Add(infoOutputPackage.CategoryName, infoOutputPackage);
            return infoOutputPackage;
        }

        /// <summary>Gets a <see cref="InfoOutputPackage"/> object with respect to a specific category name.
        /// </summary>
        /// <param name="categoryName">The name of the category, i.e. 'general' etc.</param>
        /// <returns>The <see cref="InfoOutputPackage"/> in the current instance with respect to the <paramref name="categoryName"/>.</returns>
        /// <exception cref="ArgumentException">Thrown, if no <see cref="InfoOutputPackage"/> object with category name <paramref name="categoryName"/> available.</exception>
        public InfoOutputPackage GetPackage(string categoryName)
        {
            if (m_Values.TryGetValue(categoryName, out InfoOutputPackage infoOutputPackage) == true)
            {
                return infoOutputPackage;
            }
            throw new ArgumentException(String.Format(ExceptionMessages.ArgumentIsInvalidForObject, categoryName, "InfoOutput"));
        }

        /// <summary>Gets a <see cref="InfoOutputPackage"/> object with respect to a specific category name.
        /// </summary>
        /// <param name="categoryName">The name of the category, i.e. 'general' etc.</param>
        /// <returns>The <see cref="InfoOutputPackage"/> in the current instance with respect to the <paramref name="categoryName"/>.</returns>
        /// <exception cref="ArgumentException">Thrown, if no <see cref="InfoOutputPackage"/> object with category name <paramref name="categoryName"/> available.</exception>
        public InfoOutputPackage GetPackage(IdentifierString categoryName)
        {
            if (m_Values.TryGetValue(categoryName, out InfoOutputPackage infoOutputPackage) == true)
            {
                return infoOutputPackage;
            }
            throw new ArgumentException(String.Format(ExceptionMessages.ArgumentIsInvalidForObject, categoryName, "InfoOutput"));
        }
        #endregion

        #region public static methods

        /// <summary>Creates a new <see cref="InfoOutput&lt;T&gt;"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="infoOutputFillMethod">A delegate which is used to fill the info output, i.e. adds a collection of <see cref="InfoOutputProperty"/> and tables to a specific <see cref="InfoOutput"/> object.</param>
        /// <param name="infoOutputDetailLevel">The info output detail level which is used for information only and can not be changed.</param>
        /// <typeparam name="T">The type of the encapuslated object.</typeparam>
        public static InfoOutput<T> Create<T>(T value, Action<InfoOutput, string> infoOutputFillMethod = null, InfoOutputDetailLevel infoOutputDetailLevel = InfoOutputDetailLevel.Full)
        {
            return new InfoOutput<T>(value, infoOutputFillMethod, infoOutputDetailLevel);
        }
        #endregion
    }

    /// <summary>Represents a wrapper of a specific object which does not supports the <see cref="IInfoOutputQueriable"/> interface and adds a <see cref="InfoOutput"/> instance.
    /// </summary>
    /// <typeparam name="T">The type of the encapuslated object.</typeparam>
    public class InfoOutput<T> : IInfoOutputQueriable
    {
        #region public (readonly) members

        /// <summary>The value, i.e. the encapsulated object.
        /// </summary>
        public readonly T Value;
        #endregion

        #region private members

        /// <summary>The level of details.
        /// </summary>
        private readonly InfoOutputDetailLevel m_OutputDetailLevel;

        /// <summary>The delegate, which fills the 'info' output, i.e. properties and tables.
        /// </summary>
        private readonly Action<InfoOutput, string> m_InfoOutput;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="InfoOutput&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="infoOutputFillMethod">A delegate which is used to fill the info output, i.e. adds a collection of <see cref="InfoOutputProperty"/> and tables to a specific <see cref="InfoOutput"/> object.</param>
        /// <param name="infoOutputDetailLevel">The info output detail level which is used for information only and can not be changed.</param>
        internal InfoOutput(T value, Action<InfoOutput, string> infoOutputFillMethod = null, InfoOutputDetailLevel infoOutputDetailLevel = InfoOutputDetailLevel.Full)
        {
            m_InfoOutput = infoOutputFillMethod;
            Value = value;
            m_OutputDetailLevel = infoOutputDetailLevel;
        }
        #endregion

        #region public properties

        #region IInfoOutputQueriable Members

        /// <summary>Gets the info-output level of detail.
        /// </summary>
        /// <value>The info-output level of detail.</value>
        public InfoOutputDetailLevel InfoOutputDetailLevel
        {
            get { return m_OutputDetailLevel; }
        }
        #endregion

        #endregion

        #region public methods

        #region IInfoOutputQueriable Members

        /// <summary>Gets informations of the current object as a specific <see cref="InfoOutput"/> instance.
        /// </summary>
        /// <param name="infoOutput">The <see cref="InfoOutput"/> object which is to be filled with informations concering the current instance.</param>
        /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category name.</param>
        public void FillInfoOutput(InfoOutput infoOutput, string categoryName)
        {
            if (infoOutput == null)
            {
                throw new ArgumentNullException("infoOutput");
            }
            m_InfoOutput?.Invoke(infoOutput, categoryName);
        }

        /// <summary>Sets the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel"/> property.
        /// </summary>
        /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
        /// <returns>A value indicating whether the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel"/> has been set to <paramref name="infoOutputDetailLevel"/>.
        /// </returns>
        public bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
        {
            return (infoOutputDetailLevel == m_OutputDetailLevel);
        }
        #endregion

        #endregion

        #region public static methods

        /// <summary>Performs an implicit conversion from <see cref="Dodoni.BasicComponents.Containers.InfoOutput&lt;T&gt;"/> to <typeparamref name="T"/>.
        /// </summary>
        /// <param name="infoOutput">The <see cref="Dodoni.BasicComponents.Containers.InfoOutput&lt;T&gt;"/> object.</param>
        /// <returns>The result of the conversion, i.e. <see cref="Dodoni.BasicComponents.Containers.InfoOutput&lt;T&gt;.Value"/>.</returns>
        public static implicit operator T(InfoOutput<T> infoOutput)
        {
            if (infoOutput == null)
            {
                return default;
            }
            return infoOutput.Value;
        }
        #endregion
    }
}