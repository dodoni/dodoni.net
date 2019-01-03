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
using System.Windows.Forms;

namespace Dodoni.BasicComponents.Containers
{
    public partial class InfoOutputParentChildTable : Form
    {
        #region private members

        /// <summary>The binding source for the parent table.
        /// </summary>
        private BindingSource m_ParentBindingSource = new BindingSource();

        /// <summary>The binding source for the child table.
        /// </summary>
        private BindingSource m_ChildBindingSource = new BindingSource();
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="InfoOutputParentChildTable"/> class.
        /// </summary>
        public InfoOutputParentChildTable()
        {
            InitializeComponent();
        }
        #endregion

        #region private methods

        /// <summary>Handles the Click event of the bCreateObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void bCreateObject_Click(object sender, EventArgs e)
        {
            SimpleDataObject simpleDataObject = new SimpleDataObject();

            InfoOutput infoOutput = new InfoOutput();
            simpleDataObject.FillInfoOutput(infoOutput);

            InfoOutputPackage generalPackage = infoOutput.GetGeneralPackage();

            InfoOutputParentChildDataTable parentChildDataTable = generalPackage.GetParentChildDataTable("ParentTable");
            InitializeDataGridViews(parentChildDataTable);
        }

        /// <summary>Initialize the <see cref="DataGridView"/> controls.
        /// </summary>
        /// <param name="data">The data in its <see cref="DataTable"/> representation.</param>
        /// <remarks>There is no such implementation in the Dodoni.net project, because it depends on System.Windows.Forms which should be avoided in the
        /// Dodoni.BasicComponents.</remarks>
        private void InitializeDataGridViews(InfoOutputParentChildDataTable data)
        {
            m_ParentBindingSource.DataSource = data.DataSet;
            m_ParentBindingSource.DataMember = data.ParentDataTable.TableName;

            m_ChildBindingSource.DataSource = m_ParentBindingSource;
            m_ChildBindingSource.DataMember = data.Relation.RelationName;

            dataGridViewParent.DataSource = m_ParentBindingSource;
            dataGridViewParent.Columns[dataGridViewParent.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridViewChild.DataSource = m_ChildBindingSource;
            dataGridViewChild.Columns[dataGridViewChild.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
        #endregion
    }
}