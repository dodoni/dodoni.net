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

using Microsoft.Extensions.Logging;
using Dodoni.BasicComponents.Logging;

namespace Dodoni.BasicComponents.LogFileFactory
{
    public partial class LoggingForm : Form
    {
        /// <summary>Initializes a new instance of the <see cref="LoggingForm"/> class.
        /// </summary>
        public LoggingForm()
        {
            InitializeComponent();
        }

        /// <summary>Handles the Click event of the bAdd control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void bAdd_Click(object sender, EventArgs e)
        {
            var messageType = (LogLevel)cbMessageType.SelectedItem;
            Logger.Stream.Log(messageType, editMessage.Text);

            editLogging.Text = Logger.Stream.ToString();
        }

        /// <summary>Handles the Load event of the LoggingForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void LoggingForm_Load(object sender, EventArgs e)
        {
            foreach (var loggingMsgType in Enum.GetValues(typeof(LogLevel)))
            {
                cbMessageType.Items.Add(loggingMsgType);
            }
            cbMessageType.SelectedIndex = 0;
        }
    }
}