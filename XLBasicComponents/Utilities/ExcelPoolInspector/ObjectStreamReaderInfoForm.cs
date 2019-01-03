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
using System.ComponentModel;
using System.Collections.Generic;

namespace Dodoni.XLBasicComponents.Utilities
{
    /// <summary>A Windows form which contains the result of some object stream reader operation.
    /// </summary>
    internal partial class ObjectStreamReaderInfoForm : Form
    {
        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="ObjectStreamReaderInfoForm"/> class.
        /// </summary>
        public ObjectStreamReaderInfoForm()
        {
            InitializeComponent();
        }
        #endregion

        #region internal methods

        /// <summary>Shows the details.
        /// </summary>
        /// <param name="infoString">The info string.</param>
        /// <param name="excelPoolItems">The added excel pool items.</param>
        /// <returns></returns>
        internal DialogResult ShowDetails(string infoString, IEnumerable<ExcelPoolItem> excelPoolItems)
        {
            editSummary.Text = infoString;

            BindingList<Tuple<string, string>> bindingList = new BindingList<Tuple<string, string>>();
            foreach (ExcelPoolItem excelPoolItem in excelPoolItems)
            {
                bindingList.Add(Tuple.Create(excelPoolItem.ObjectType.Name.String, excelPoolItem.ObjectName.String));
            }
            ObjectTypeColumn.DataPropertyName = "Item1";
            ObjectNameColumn.DataPropertyName = "Item2";

            dataGridView.DataSource = bindingList;
            return ShowDialog();
        }
        #endregion
    }
}