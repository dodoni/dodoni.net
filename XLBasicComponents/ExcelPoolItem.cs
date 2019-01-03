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
using Dodoni.BasicComponents.Containers;

namespace Dodoni.XLBasicComponents
{
    /// <summary>Serves as template for the elements of the <see cref="ExcelPool"/>.
    /// </summary>
    public class ExcelPoolItem : IIdentifierNameable
    {
        #region private members

        /// <summary>A collection of <see cref="GuidedExcelDataQuery"/> instances which is some kind of 'table' representation of <see cref="ExcelPoolItem.Value"/>, 
        /// i.e. one can a apply a specific <see cref="ExcelPoolItemCreator"/> to create a copy of <see cref="ExcelPoolItem.Value"/>.
        /// </summary>
        private IIdentifierStringDictionary<GuidedExcelDataQuery> m_ExcelDataQueries;
        #endregion

        #region public (readonly) members

        /// <summary>The type of the object in its <see cref="ExcelPoolItemType"/> representation.
        /// </summary>
        public readonly ExcelPoolItemType ObjectType;

        /// <summary>The name of the object in its <see cref="IdentifierString"/> representation.
        /// </summary>
        public readonly IdentifierString ObjectName;

        /// <summary>The object itself in its <see cref="IInfoOutputQueriable"/> representation.
        /// </summary>
        public readonly IInfoOutputQueriable Value;

        /// <summary>A collection of <see cref="ExcelPoolItem"/> objects which are used as input for the current instance, i.e. dependent objects, perhaps <c>null</c>.
        /// </summary>
        /// <example>Swap rates, Deposit rates etc. are input for a specific discount factor curve etc.</example>
        public readonly IEnumerable<ExcelPoolItem> InputDependencyItems;

        /// <summary>The time stamp, i.e. the time of creation.
        /// </summary>
        public readonly DateTime TimeStamp;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="ExcelPoolItem"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="objectName">The name of <paramref name="value"/>.</param>
        /// <param name="objectType">The type of <paramref name="value"/>.</param>
        /// <param name="excelDataQueries">The excel data queries, i.e. the (user) input.</param>
        /// <remarks>The <see cref="IExcelDataQuery.QueryCompleted(bool)"/> will be called for each element of <paramref name="excelDataQueries"/> and the <see cref="GuidedExcelDataQuery"/> representation
        /// will be stored internally. Moreover <see cref="InputDependencyItems"/> will be set to <c>null</c>.</remarks>
        public ExcelPoolItem(IInfoOutputQueriable value, string objectName, ExcelPoolItemType objectType, params IExcelDataQuery[] excelDataQueries)
            : this(value, objectName, objectType, excelDataQueries, inputDependencyItems: null)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ExcelPoolItem"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="objectName">The name of <paramref name="value"/>.</param>
        /// <param name="objectType">The type of <paramref name="value"/>.</param>
        /// <param name="excelDataQueries">The excel data queries, i.e. the (user) input.</param>
        /// <param name="inputDependencyItems">A collection of <see cref="ExcelPoolItem"/> objects which are used as input for the construction of <paramref name="value"/>, i.e. dependent objects.</param>
        /// <remarks>The <see cref="IExcelDataQuery.QueryCompleted(bool)"/> will be called for each element of <paramref name="excelDataQueries"/> and the <see cref="GuidedExcelDataQuery"/> representation
        /// will be stored internally.</remarks>
        /// <example><paramref name="inputDependencyItems"/> are for example swap rates, deposit rates etc. if the current instance is a discount factor curve etc.</example>
        public ExcelPoolItem(IInfoOutputQueriable value, string objectName, ExcelPoolItemType objectType, IEnumerable<IExcelDataQuery> excelDataQueries, IEnumerable<ExcelPoolItem> inputDependencyItems = null)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            Value = value;

            if (objectName == null)
            {
                throw new ArgumentNullException("objectName");
            }
            ObjectName = objectName.ToIdentifierString();

            if (objectType == null)
            {
                throw new ArgumentNullException("objectType");
            }
            ObjectType = objectType;

            if (excelDataQueries == null)
            {
                throw new ArgumentNullException("excelDataQueries");
            }
            var guidedExcelDataQueryInput = new IdentifierStringDictionary<GuidedExcelDataQuery>(isReadOnlyExceptAdding: false);
            foreach (var inputExcelDataQuery in excelDataQueries)
            {
                inputExcelDataQuery.QueryCompleted();
                guidedExcelDataQueryInput.Add(inputExcelDataQuery.Name, inputExcelDataQuery.AsCustomizeData());
            }
            m_ExcelDataQueries = guidedExcelDataQueryInput;

            InputDependencyItems = inputDependencyItems;
            TimeStamp = DateTime.Now;
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the current instance, i.e. the <see cref="ExcelPoolItem.ObjectName"/>.
        /// </summary>
        /// <value>The language independent name of the current instance.</value>
        public IdentifierString Name
        {
            get { return ObjectName; }
        }

        /// <summary>Gets the long name of the current instance, i.e. <see cref="ExcelPoolItem.ObjectName"/>.
        /// </summary>
        /// <value>The language dependent long name of the current instance.</value>
        public IdentifierString LongName
        {
            get { return ObjectName; }
        }
        #endregion

        #endregion

        #region public methods

        /// <summary>Gets the <see cref="ExcelPoolItem.ObjectName"/> and the <see cref="ExcelPoolItem.TimeStamp"/> in a <see cref="System.String"/> representation.
        /// </summary>
        /// <returns>The <see cref="ExcelPoolItem.ObjectName"/> and the <see cref="ExcelPoolItem.TimeStamp"/> separated by <see cref="IdentifierString.IgnoringStartCharacter"/>.</returns>
        public string GetObjectNameWithTimeStamp()
        {
            return ObjectName.String.ToTimeStampString(TimeStamp);
        }
        /// <summary>Gets the <see cref="System.String"/> collection ('table names') of the <see cref="GuidedExcelDataQuery"/> which
        /// are used as input for the construction of <see cref="ExcelPoolItem.Value"/>.
        /// </summary>
        /// <returns>A collection of the 'table names'.</returns>
        public IEnumerable<string> GetDataQueryNames()
        {
            return m_ExcelDataQueries.Names;
        }

        /// <summary>Gets a specific <see cref="GuidedExcelDataQuery"/> which is part of the input for the construction of <see cref="ExcelPoolItem.Value"/>.
        /// </summary>
        /// <param name="name">The name of the <see cref="GuidedExcelDataQuery"/> to search.</param>
        /// <param name="dataQuery">The <see cref="GuidedExcelDataQuery"/> object (output).</param>
        /// <returns>A value indicating whether <paramref name="dataQuery"/> contains valid data.</returns>
        public bool TryGetDataQuery(string name, out GuidedExcelDataQuery dataQuery)
        {
            return m_ExcelDataQueries.TryGetValue(name, out dataQuery);
        }

        /// <summary>Gets the <see cref="GuidedExcelDataQuery"/> representation of <see cref="ExcelPoolItem.Value"/>.
        /// </summary>
        /// <returns>A collection of <see cref="GuidedExcelDataQuery"/> instances which represents the input needed to 
        /// construct <see cref="ExcelPoolItem.Value"/> with respect to a specific <see cref="ExcelPoolItemCreator"/>.</returns>
        public IEnumerable<GuidedExcelDataQuery> GetDataQueryRepresentation()
        {
            return m_ExcelDataQueries;
        }

        /// <summary>Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return ObjectName.String;
        }
        #endregion
    }
}