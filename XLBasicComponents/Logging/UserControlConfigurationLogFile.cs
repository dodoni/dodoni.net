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
using System.Collections.Generic;

using Dodoni.BasicComponents.Utilities;

namespace Dodoni.XLBasicComponents.Logging
{
    /// <summary>Serves as user control for the configuration of the (global) log file.
    /// </summary>
    internal partial class UserControlConfigurationLogFile : UserControl, IConfigurationUserControl
    {
        #region private (const) members

        /// <summary>A value indicating whether the user has changed the configuration.
        /// </summary>
        private bool m_ConfigurationChanged = false;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="UserControlConfigurationLogFile"/> class.
        /// </summary>
        public UserControlConfigurationLogFile()
        {
            InitializeComponent();

            foreach (var loggingLevel in EnumString<ExcelPoolLoggingLevel>.GetValues())
            {
                comboBoxGlobalLoggingLevel.Items.Add(loggingLevel);
            }
            foreach (var localOutputUsage in EnumString<ExcelLogger.OutputUsage>.GetValues())
            {
                comboBoxLocalOutputUsage.Items.Add(localOutputUsage);
            }
            RestoreConfiguration();
        }
        #endregion

        #region public properties

        #region IConfigurationUserControl Members

        /// <summary>Gets a value indicating whether the user changed the configuration.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the user changed the configuration; otherwise, <c>false</c>.
        /// </value>
        public bool ConfigurationChanged
        {
            get { return m_ConfigurationChanged; }
        }

        /// <summary>Gets the title of the user control in its <see cref="System.String"/> representation.
        /// </summary>
        /// <value>The title of the user control.</value>
        public string Title
        {
            get { return "Logging"; }
        }
        #endregion

        #endregion

        #region public methods

        #region IConfigurationUserControl Members

        /// <summary>Store the configuration, i.e. write a specific configuration file.
        /// </summary>
        public void StoreConfiguration()
        {
            ExcelLogger.SetAddOnDataAdviceFails(cBoxAddLogfileOnDropdownListFails.Checked);
            ExcelLogger.SetPopupGlobalLogfileBoxOnError(cBoxPopupGlobalLogfileForm.Checked);
            var userGlobalLoggingLevel = comboBoxGlobalLoggingLevel.SelectedItem as EnumString<ExcelPoolLoggingLevel>;
            if (userGlobalLoggingLevel != null)
            {
                ExcelPool.StoreLoggingLevel(userGlobalLoggingLevel.Value);
            }
            else
            {
                ExcelPool.StoreLoggingLevel(ExcelPoolLoggingLevel.None);
            }

            var outputUsage = comboBoxLocalOutputUsage.SelectedItem as EnumString<ExcelLogger.OutputUsage>;
            if (outputUsage != null)
            {
                ExcelObjectLogger.SetOutputUsage(outputUsage.Value);
            }
            else
            {
                ExcelObjectLogger.SetOutputUsage(ExcelLogger.OutputUsage.None);
            }
            ExcelObjectLogger.SetOutputFolder(editLocalOutputFolder.Text);

            ExcelAddIn.Configuration.Save();
            m_ConfigurationChanged = false;
        }

        /// <summary>Restores the configuration, i.e. reads the original configuration from some configuration file or take some default values and show these configuration.
        /// </summary>
        public void RestoreConfiguration()
        {
            cBoxAddLogfileOnDropdownListFails.Checked = ExcelLogger.GetAddOnDataAdviceFails();
            cBoxPopupGlobalLogfileForm.Checked = ExcelLogger.GetPopupGlobalLogfileBoxOnError();

            var restoredLoggingLevel = ExcelPool.GetLoggingLevelFromConfigFile();
            foreach (var item in comboBoxGlobalLoggingLevel.Items)
            {
                var itemValue = item as EnumString<ExcelPoolLoggingLevel>;
                if ((itemValue != null) && (itemValue.Value == restoredLoggingLevel))
                {
                    comboBoxGlobalLoggingLevel.SelectedItem = item;
                    break;
                }
            }

            var outputUsage = ExcelObjectLogger.GetOutputUsage();
            foreach (var item in comboBoxLocalOutputUsage.Items)
            {
                var itemValue = item as EnumString<ExcelLogger.OutputUsage>;
                if ((itemValue != null) && (itemValue.Value == outputUsage))
                {
                    comboBoxLocalOutputUsage.SelectedItem = item;
                    break;
                }
            }
            editLocalOutputFolder.Text = ExcelObjectLogger.GetOutputFolder();
            m_ConfigurationChanged = false;
        }
        #endregion

        #endregion

        #region private methods

        /// <summary>Handles the Click event of the <c>buttonReset</c> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void bReset_Click(object sender, EventArgs e)
        {
            RestoreConfiguration();
        }

        /// <summary>Handles the Changed event of one of the user input control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Userinput_Changed(object sender, EventArgs e)
        {
            m_ConfigurationChanged = true;
        }

        /// <summary>Handles the Click event of the bLocalOutputFolderSelector control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void bLocalOutputFolderSelector_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.ShowNewFolderButton = true;
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                editLocalOutputFolder.Text = folderBrowser.SelectedPath;
            }
        }
        #endregion
    }
}