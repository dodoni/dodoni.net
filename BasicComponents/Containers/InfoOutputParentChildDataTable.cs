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
using System.Data;

namespace Dodoni.BasicComponents.Containers
{
    /// <summary>Serves as wrapper for two <see cref="DataTable"/> objects which are connected with respect
    /// to a 1:n relation, i.e. a parent data table and a child data table.
    /// </summary>
    /// <remarks>The relation is linked to a single column of the parent data table as well as one 
    /// single column of the child data table.</remarks>
    public class InfoOutputParentChildDataTable
    {
        #region private members

        /// <summary>The dataset.
        /// </summary>
        private DataSet m_DataSet;

        /// <summary>The parent data table.
        /// </summary>
        private readonly DataTable m_ParentDataTable;

        /// <summary>The child data table.
        /// </summary>
        private readonly DataTable m_ChildDataTable;

        /// <summary>The 1:n relation between parent and child data table.
        /// </summary>
        private readonly DataRelation m_Relation;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="InfoOutputParentChildDataTable"/> class.
        /// </summary>
        /// <param name="parentDataTable">The parent data table.</param>
        /// <param name="childDataTable">The child data table.</param>
        /// <param name="parentDataColumn">The parent data column, i.e. the column with respect to the relation between parent und child data table.</param>
        /// <param name="childDataColumn">The child data column, i.e. the column with respect to the relation between parent und child data table.</param>
        public InfoOutputParentChildDataTable(DataTable parentDataTable, DataTable childDataTable, DataColumn parentDataColumn, DataColumn childDataColumn)
        {
            m_ParentDataTable = parentDataTable ?? throw new ArgumentNullException("parentDataTable");
            m_DataSet = new DataSet();
            m_DataSet.Tables.Add(parentDataTable);

            m_ChildDataTable = childDataTable ?? throw new ArgumentNullException("childDataTable");
            m_DataSet.Tables.Add(childDataTable);

            m_Relation = new DataRelation("relation", parentDataColumn, childDataColumn);
            m_DataSet.Relations.Add(m_Relation);
        }
        #endregion

        #region public properties

        /// <summary>Gets the <see cref="DataSet"/> object.
        /// </summary>
        /// <value>The <see cref="DataSet"/> object.</value>
        public DataSet DataSet
        {
            get { return m_DataSet; }
        }

        /// <summary>Gets the parent data table.
        /// </summary>
        /// <value>The parent data table.</value>
        public DataTable ParentDataTable
        {
            get { return m_ParentDataTable; }
        }

        /// <summary>Gets the child data table.
        /// </summary>
        /// <value>The child data table.</value>
        public DataTable ChildDataTable
        {
            get { return m_ChildDataTable; }
        }

        /// <summary>Gets the 1:n relation between parent and child data table.
        /// </summary>
        /// <value>The relation.</value>
        public DataRelation Relation
        {
            get { return m_Relation; }
        }
        #endregion
    }
}