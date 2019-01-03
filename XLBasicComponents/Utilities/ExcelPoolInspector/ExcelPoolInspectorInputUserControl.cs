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
using System.Windows.Forms;

namespace Dodoni.XLBasicComponents.Utilities
{
    /// <summary>Serves as <see cref="UserControl"/> which represents the user input of a specific <see cref="ExcelPoolItem"/> object.
    /// </summary>
    internal partial class ExcelPoolInspectorInputUserControl : UserControl
    {
        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="ExcelPoolInspectorInputUserControl"/> class.
        /// </summary>
        public ExcelPoolInspectorInputUserControl()
        {
            InitializeComponent();
        }
        #endregion

        #region public methods

        /// <summary>Initialize the current instance with a specific <see cref="GuidedExcelDataQuery"/> object which represents the input data of a specific <see cref="ExcelPoolItem"/> object.
        /// </summary>
        /// <param name="guidedExcelDataQuery">The <see cref="GuidedExcelDataQuery"/> object.</param>
        public void Initialize(GuidedExcelDataQuery guidedExcelDataQuery)
        {
            if (guidedExcelDataQuery != null)
            {
                labelName.Text = guidedExcelDataQuery.Name.String;

                inputDataGridView.Columns.Clear();
                for (int j = 0; j < guidedExcelDataQuery.ColumnCount; j++)
                {
                    inputDataGridView.Columns.Add(j.ToString(), j.ToString());
                }
                if (guidedExcelDataQuery.ColumnCount > 0)
                {
                    inputDataGridView.Columns[guidedExcelDataQuery.ColumnCount - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }

                for (int j = 0; j < guidedExcelDataQuery.RowCount; j++)
                {
                    if (guidedExcelDataQuery.IsEmptyRow(j, GuidedExcelDataQuery.eEmptyExcelCellMode.Standard | GuidedExcelDataQuery.eEmptyExcelCellMode.HasSpecificTypeWithoutAdvice) == false)
                    {
                        inputDataGridView.Rows.Add();
                        for (int k = 0; k < guidedExcelDataQuery.ColumnCount; k++)
                        {
                            if (guidedExcelDataQuery.IsEmptyExcelCell(j, k) == false)
                            {
                                inputDataGridView[k, j].Value = guidedExcelDataQuery.GetData(j, k);
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}