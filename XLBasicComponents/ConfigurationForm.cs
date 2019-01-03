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
using System.Configuration;
using System.Windows.Forms;
using System.Collections.Generic;

using Dodoni.BasicComponents;
//using Dodoni.XLBasicComponents.Logging;
using Dodoni.XLBasicComponents.Utilities;
using Dodoni.XLBasicComponents.Logging;

namespace Dodoni.XLBasicComponents
{
    /// <summary>Represents the windows form for the configuration of Dodoni.NET. Moreover this class managed the configuration file for the Excel interface of Dodoni.NET in general.
    /// </summary>
    internal partial class ConfigurationForm : Form
    {
        #region private members

        /// <summary>A mapping between the user control and the <see cref="TreeNode"/> representation.
        /// </summary>
        private Dictionary<TreeNode, IConfigurationUserControl> m_Controls;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="ConfigurationForm"/> class.
        /// </summary>
        public ConfigurationForm()
        {
            InitializeComponent();

            m_Controls = new Dictionary<TreeNode, IConfigurationUserControl>();

            TreeNode root = new TreeNode("General");
            root.Name = "General";
            m_Controls.Add(root, new UserControlConfigurationGeneral());
            treeViewMain.Nodes.Add(root);

            TreeNode loggingRoot = new TreeNode("Logging");
            loggingRoot.Name = "Logging";
            m_Controls.Add(loggingRoot, new UserControlConfigurationLogFile());
            treeViewMain.Nodes.Add(loggingRoot);

            /* load external, additional configuration user controls: */
            ExcelAddIn.Configuration.RaiseConfigurationUserControlRegister(treeViewMain, new ExcelConfiguration.ConfigurationUserControlRegisterEventArgs(m_Controls));
        }
        #endregion

        #region private methods

        /// <summary>Handles the AfterSelect event of the treeViewMain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.TreeViewEventArgs"/> instance containing the event data.</param>
        private void treeViewMain_AfterSelect(object sender, TreeViewEventArgs e)
        {
            IConfigurationUserControl userControl;
            if (m_Controls.TryGetValue(e.Node, out userControl) == true)
            {
                mainGroupBox.Hide();

                mainGroupBox.Controls.Clear();
                UserControl control = (UserControl)userControl;
                control.Left = 10;
                control.Top = 15;
                control.Height = mainGroupBox.Height - 25;
                control.Width = mainGroupBox.Width - 15;
                control.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                mainGroupBox.Text = userControl.Title;

                mainGroupBox.Controls.Add(control);
                mainGroupBox.Show();
            }
        }

        /// <summary>Handles the Click event of the OK button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void bOK_Click(object sender, EventArgs e)
        {
            bool configurationChanged = false;
            foreach (IConfigurationUserControl userControl in m_Controls.Values)
            {
                if (userControl.ConfigurationChanged == true)
                {
                    configurationChanged = true;
                    userControl.StoreConfiguration();
                }
            }
            if (configurationChanged == true)
            {
                MessageBox.Show("Write configuration file. Perhaps a restart necessary.", "Dodoni.NET Configuration", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            Close();
        }
        #endregion
    }
}
