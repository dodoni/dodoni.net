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

using Dodoni.BasicComponents.Logging;
using Dodoni.XLBasicComponents.Logging;

namespace Dodoni.XLBasicComponents.Utilities
{
    /// <summary>Represents a <see cref="UserControl"/> which show the name, object type, logfile etc. of a specific <see cref="ExcelPoolItem"/> object.
    /// </summary>
    internal partial class ExcelPoolInspectorGeneralUserControl : UserControl
    {
        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="ExcelPoolInspectorGeneralUserControl"/> class.
        /// </summary>
        public ExcelPoolInspectorGeneralUserControl()
        {
            InitializeComponent();
        }
        #endregion

        #region public methods

        /// <summary>Initializes the current object with respect to a specific <see cref="ExcelPoolItem"/> object.
        /// </summary>
        /// <param name="excelPoolItem">The <see cref="ExcelPoolItem"/> object.</param>
        public void Initialize(ExcelPoolItem excelPoolItem)
        {
            if (excelPoolItem != null)
            {
                editName.Text = excelPoolItem.LongName.String;

                editType.Text = excelPoolItem.ObjectType.LongName.String;
                dateTimePickerCreationTime.Value = excelPoolItem.TimeStamp;

                var loggingObject = excelPoolItem.Value as ILoggedObject;
                if (loggingObject != null)
                {
                    var excelLogFile = loggingObject.Logging as ExcelObjectLogger;

                    logFileDataGridView.AutoGenerateColumns = false;
                    ColumnTime.DataPropertyName = "TimeStamp";
                    ColumnClassification.DataPropertyName = "MessageTypeClassificationAsString";
                    ColumnMessageType.DataPropertyName = "MessageTypeName";
                    ColumnMessage.DataPropertyName = "Message";

                    logFileDataGridView.DataSource = excelLogFile.GetAsBindingList();
                }
            }
            else
            {
                editName.Text = editType.Text = String.Empty;
                dateTimePickerCreationTime.Value = DateTime.MinValue;
                logFileDataGridView.DataSource = null;
            }
        }
        #endregion
    }
}