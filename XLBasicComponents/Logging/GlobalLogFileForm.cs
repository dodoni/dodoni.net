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
using System.Windows.Forms;
using System.ComponentModel;

namespace Dodoni.XLBasicComponents.Logging
{
    /// <summary>Represent the Windows Form for the Global Log-file.
    /// </summary>
    internal partial class GlobalLogFileForm : Form
    {
        #region private members

        /// <summary>The reference to the 'global' logging.
        /// </summary>
        private BindingList<GlobalLogFileRow> m_GlobalLogFile;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="GlobalLogFileForm"/> class.
        /// </summary>
        public GlobalLogFileForm()
        {
            InitializeComponent();
        }
        #endregion

        #region internal methods

        /// <summary>Sets the data sources.
        /// </summary>
        /// <param name="globalLogFile">The global log file.</param>
        internal void SetDataSources(BindingList<GlobalLogFileRow> globalLogFile)
        {
            m_GlobalLogFile = globalLogFile;

            logFileDataGridView.AutoGenerateColumns = false;
            ColumnTime.DataPropertyName = "TimeStamp";
            ColumnClassification.DataPropertyName = "MessageTypeClassification";
            ColumnMessageType.DataPropertyName = "MessageTypeName";
            ColumnObjectName.DataPropertyName = "ObjectName";
            ColumnObjectTypeName.DataPropertyName = "ObjectTypeName";
            ColumnMessage.DataPropertyName = "Message";

            logFileDataGridView.DataSource = globalLogFile;
        }
        #endregion

        #region private methods

        /// <summary>Handles the Click event of the Clear button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void bClear_Click(object sender, EventArgs e)
        {
            m_GlobalLogFile.Clear();
        }

        /// <summary>Handles the Click event of the close button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void bClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        /// <summary>Handles the Closing event of the <see cref="GlobalLogFileForm"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.FormClosingEventArgs"/> instance containing the event data.</param>
        private void GlobalLogFileForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.Hide();
                e.Cancel = true;
            }
        }
        #endregion
    }
}