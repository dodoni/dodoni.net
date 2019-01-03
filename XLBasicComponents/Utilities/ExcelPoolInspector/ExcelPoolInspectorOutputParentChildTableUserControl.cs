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
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;

using Dodoni.BasicComponents.Containers;

namespace Dodoni.XLBasicComponents.Utilities
{
    /// <summary>Serves as <see cref="UserControl"/> which represents the table output of a specific <see cref="ExcelPoolItem"/> object
    /// with respect to a <see cref="InfoOutputParentChildDataTable"/> representation.
    /// </summary>
    internal partial class ExcelPoolInspectorOutputParentChildTableUserControl : UserControl
    {
        #region private members

        /// <summary>The binding source for the parent table.
        /// </summary>
        private BindingSource m_ParentBindingSource = new BindingSource();

        /// <summary>The binding source for the child table.
        /// </summary>
        private BindingSource m_ChildBindingSource = new BindingSource();
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="ExcelPoolInspectorOutputParentChildTableUserControl"/> class.
        /// </summary>
        internal ExcelPoolInspectorOutputParentChildTableUserControl()
        {
            InitializeComponent();
        }
        #endregion

        #region public methods

        /// <summary>Initialize the current instance with a specific <see cref="DataTable"/> object which represents the output data of a specific <see cref="ExcelPoolItem"/> object.
        /// </summary>
        /// <param name="heading">The name of the parent table.</param>
        /// <param name="data">The data in its <see cref="DataTable"/> representation.</param>
        public void Initialize(string heading, InfoOutputParentChildDataTable data)
        {
            m_ParentBindingSource.DataSource = data.DataSet;
            m_ParentBindingSource.DataMember = data.ParentDataTable.TableName;

            m_ChildBindingSource.DataSource = m_ParentBindingSource;
            m_ChildBindingSource.DataMember = data.Relation.RelationName;

            parentLabelName.Text = heading;
            parentInfoOutputGridView.DataSource = m_ParentBindingSource;
            parentInfoOutputGridView.Columns[parentInfoOutputGridView.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            childLabelName.Text = data.ChildDataTable.TableName;
            childInfoOutputdataGridView.DataSource = m_ChildBindingSource;
            childInfoOutputdataGridView.Columns[childInfoOutputdataGridView.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
        #endregion
    }
}