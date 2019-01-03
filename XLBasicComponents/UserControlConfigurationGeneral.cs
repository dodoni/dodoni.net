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

using Dodoni.BasicComponents.Utilities;

namespace Dodoni.XLBasicComponents
{
    /// <summary>The <see cref="UserControl"/> which represents the general configuration of Dodoni.NET.
    /// </summary>
    internal partial class UserControlConfigurationGeneral : UserControl, IConfigurationUserControl
    {
        #region private (const) members

        /// <summary>A value indicating whether the user has changed the configuration.
        /// </summary>
        private bool m_ConfigurationChanged = false;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="UserControlConfigurationGeneral"/> class.
        /// </summary>
        public UserControlConfigurationGeneral()
        {
            InitializeComponent();

            foreach (var dropDownAdviseType in EnumString<ExcelLowLevel.DropDownListCreationType>.GetValues())
            {
                comboBoxAddDropDownList.Items.Add(dropDownAdviseType);
            }
            RestoreConfiguration();
        }
        #endregion

        #region IConfigurationUserControl Members

        /// <summary>Gets the title of the user control in its <see cref="System.String"/> representation.
        /// </summary>
        /// <value>The title of the user control.</value>
        public string Title
        {
            get { return "General configuration"; }
        }

        /// <summary>Gets a value indicating whether the user changed the configuration.
        /// </summary>
        /// <value><c>true</c> if the user changed the configuration; otherwise, <c>false</c>.
        /// </value>
        public bool ConfigurationChanged
        {
            get { return m_ConfigurationChanged; }
        }

        /// <summary>Store the configuration, i.e. write a specific configuration file.
        /// </summary>
        public void StoreConfiguration()
        {
            var useDataAdvise = comboBoxAddDropDownList.SelectedItem as EnumString<ExcelLowLevel.DropDownListCreationType>;
            if (useDataAdvise != null)
            {
                ExcelLowLevel.StoreUseDataAdvice(useDataAdvise.Value);
            }
            ExcelLowLevel.StoreUseVBADataAdvice(cBoxAddVBADropDownList.Checked);
            ExcelDataAdvice.DropDownRepresentation.StoreDropDownSeparator(comboBoxSeparator.Text);
            ExcelDataAdvice.Pool.m_BooleanAdvice.SetTrueString(comboBoxTRUE.Text);
            ExcelDataAdvice.Pool.m_BooleanAdvice.SetFalseString(comboBoxFALSE.Text);

            ExcelAddIn.Configuration.Save();
            m_ConfigurationChanged = false;
        }

        /// <summary>Restores the configuration, i.e. reads the original configuration from some
        /// configuration file or take some default values and show these configuration.
        /// </summary>
        public void RestoreConfiguration()
        {
            ExcelLowLevel.DropDownListCreationType useDataAdvice = ExcelLowLevel.GetUseDataAdviceFromConfigFile();
            for (int j = 0; j < comboBoxAddDropDownList.Items.Count; j++)
            {
                var comboBoxUseDataAdvice = comboBoxAddDropDownList.Items[j] as EnumString<ExcelLowLevel.DropDownListCreationType>;
                if ((comboBoxUseDataAdvice != null) && (comboBoxUseDataAdvice.Value == useDataAdvice))
                {
                    comboBoxAddDropDownList.SelectedIndex = j;
                    break;
                }
            }
            cBoxAddVBADropDownList.Checked = ExcelLowLevel.GetUseVBADataAdviceFromConfigFile();
            comboBoxSeparator.Text = ExcelDataAdvice.DropDownRepresentation.GetDropDownSeparatorFromConfigFile();
            comboBoxTRUE.Text = ExcelDataAdvice.Pool.m_BooleanAdvice.RestoreTrueString();
            comboBoxFALSE.Text = ExcelDataAdvice.Pool.m_BooleanAdvice.RestoreFalseString();

            m_ConfigurationChanged = false;
        }
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
        #endregion
    }
}