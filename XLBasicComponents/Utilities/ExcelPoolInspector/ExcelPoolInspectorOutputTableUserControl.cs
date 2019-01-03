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

namespace Dodoni.XLBasicComponents.Utilities
{
    /// <summary>Serves as <see cref="UserControl"/> which represents the table output of a specific <see cref="ExcelPoolItem"/> object.
    /// </summary>
    internal partial class ExcelPoolInspectorOutputTableUserControl : UserControl
    {
        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="ExcelPoolInspectorOutputTableUserControl"/> class.
        /// </summary>
        internal ExcelPoolInspectorOutputTableUserControl()
        {
            InitializeComponent();
        }
        #endregion

        #region public methods

        /// <summary>Initialize the current instance with a specific <see cref="DataTable"/> object which represents the output data of a specific <see cref="ExcelPoolItem"/> object.
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        /// <param name="data">The data in its <see cref="DataTable"/> representation.</param>
        public void Initialize(string tableName, DataTable data)
        {
            labelName.Text = tableName;
            infoOutputGridView.DataSource = data;
            infoOutputGridView.Columns[infoOutputGridView.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
        #endregion
    }
}