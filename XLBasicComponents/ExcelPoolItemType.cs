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
using Dodoni.XLBasicComponents.Utilities;

namespace Dodoni.XLBasicComponents
{
    /// <summary>Represents the type of a specific <see cref="ExcelPoolItem"/>, for example a (interest rate) curve etc.
    /// </summary>
    /// <remarks>A mapping beween a collection of <see cref="IExcelDataQuery"/> to a specific object is needed. The type of the object
    /// is described by this class, i.e. a <see cref="IdentifierString"/> representation as well as some <see cref="System.Guid"/>. Both representations must
    /// be unique.</remarks>
    public class ExcelPoolItemType : IIdentifierNameable
    {
        #region public (readonly) members

        /// <summary>The unique identifier of the current instance.
        /// </summary>
        public readonly Guid Identifier;

        /// <summary>The collection of (sub-) categories, for example 'MathLib', 'optimization' etc.; <c>null</c> if no (sub-) category available.
        /// </summary>
        public readonly IEnumerable<IdentifierString> SubCategories = null;
        #endregion

        #region private members

        /// <summary>The identifier string representation of the current instance.
        /// </summary>
        private IdentifierString m_Name;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="ExcelPoolItemType"/> class.
        /// </summary>
        /// <param name="name">The (unique) name.</param>
        /// <param name="identifier">The (unique) identifier.</param>
        public ExcelPoolItemType(string name, Guid identifier)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            m_Name = new IdentifierString(name);
            Identifier = identifier;
        }

        /// <summary>Initializes a new instance of the <see cref="ExcelPoolItemType"/> class.
        /// </summary>
        /// <param name="name">The (unique) name.</param>
        /// <param name="identifier">The (unique) identifier.</param>
        /// <param name="category">The category of the <see cref="ExcelPoolItemType"/>, for example 'MathLib', 'Finance' etc.</param>
        public ExcelPoolItemType(string name, Guid identifier, string category)
            : this(name, identifier)
        {
            if (category != null)
            {
                SubCategories = new List<IdentifierString>() { new IdentifierString(category) };
            }
        }

        /// <summary>Initializes a new instance of the <see cref="ExcelPoolItemType"/> class.
        /// </summary>
        /// <param name="name">The (unique) name.</param>
        /// <param name="identifier">The (unique) identifier.</param>
        /// <param name="category">The category of the <see cref="ExcelPoolItemType"/>, for example 'MathLib', 'Finance' etc.</param>
        /// <param name="subCategory">The sub-category of the <see cref="ExcelPoolItemType"/>, for example 'Basic', 'Optimization' etc.</param>
        public ExcelPoolItemType(string name, Guid identifier, string category, string subCategory)
            : this(name, identifier)
        {
            if (category != null)
            {
                if (subCategory == null)
                {
                    SubCategories = new List<IdentifierString>() { new IdentifierString(category) };
                }
                else
                {
                    SubCategories = new List<IdentifierString>() { new IdentifierString(category), new IdentifierString(subCategory) };
                }
            }
        }

        /// <summary>Initializes a new instance of the <see cref="ExcelPoolItemType"/> class.
        /// </summary>
        /// <param name="name">The (unique) name.</param>
        /// <param name="identifier">The (unique) identifier.</param>
        /// <param name="subCategories">The sub-categories of the <see cref="ExcelPoolItemType"/>, for example 'MathLib', 'Optimizer', 'N-dim' etc.</param>
        public ExcelPoolItemType(string name, Guid identifier, params string[] subCategories)
            : this(name, identifier)
        {
            if (subCategories != null)
            {
                List<IdentifierString> subCategoryList = new List<IdentifierString>();
                foreach (string subCategory in subCategories)
                {
                    subCategoryList.Add(new IdentifierString(subCategory));
                }
                SubCategories = subCategoryList;
            }
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
    }
}