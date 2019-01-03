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
using System.IO;
using System.Text;
using System.Configuration;
using System.Windows.Forms;
using System.Collections.Generic;

using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Utilities;

using ExcelDna.Integration;

namespace Dodoni.XLBasicComponents.Utilities
{
    /// <summary>Serves as configuration for the Excel interface of Dodoni.NET.
    /// </summary>
    /// <remarks>The file name of the configuration file is specified by the name of the xll file (without extension) plus '.config'.</remarks>
    public class ExcelConfiguration : ConfigurationFile
    {
        #region nested classes/declarations

        /// <summary>Represents the method that will handle the registration of user controls shown in the configuration form.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="eventArgs">The <see cref="ExcelConfiguration.ConfigurationUserControlRegisterEventArgs"/> instance containing the event data.</param>
        public delegate void ConfigurationUserControlRegisterEventHandler(TreeView sender, ConfigurationUserControlRegisterEventArgs eventArgs);

        /// <summary>Provides data for the event which is raise when further user controls have to be added to the configuration form.
        /// </summary>
        public class ConfigurationUserControlRegisterEventArgs : EventArgs
        {
            #region private members

            /// <summary>A mapping between the user control and the <see cref="TreeNode"/> representation.
            /// </summary>
            private Dictionary<TreeNode, IConfigurationUserControl> m_Controls;
            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="ConfigurationUserControlRegisterEventArgs"/> class.
            /// </summary>
            /// <param name="controls">A mapping between the user control and the <see cref="TreeNode"/> representation.</param>
            internal ConfigurationUserControlRegisterEventArgs(Dictionary<TreeNode, IConfigurationUserControl> controls)
            {
                m_Controls = controls;
            }
            #endregion

            #region public methods

            /// <summary>Adds a specific <see cref="TreeNode"/> and its user control in its <see cref="IConfigurationUserControl"/> representation.
            /// </summary>
            /// <param name="key">The key, i.e. the <see cref="TreeNode"/> object.</param>
            /// <param name="value">The value, i.e. the user control in its <see cref="IConfigurationUserControl"/> representation.</param>
            public void Add(TreeNode key, IConfigurationUserControl value)
            {
                m_Controls.Add(key, value);
            }
            #endregion
        }
        #endregion

        #region public members

        /// <summary>Occurs when the configuration form will be initialize and further, individual user controls will be added.
        /// </summary>
        public event ConfigurationUserControlRegisterEventHandler UserControlRegister;

        /// <summary>General settings of the Excel configuration.
        /// </summary>
        public ConfigurationFile.PropertyCollection GeneralSettings;
        #endregion

        #region protected constructors

        /// <summary>Initializes a new instance of the <see cref="ExcelConfiguration"/> class.
        /// </summary>
        /// <param name="configurationFileName">The name of the configuration file (the path will be ignored).</param>
        protected ExcelConfiguration(string configurationFileName)
            : base(configurationFileName)
        {
            GeneralSettings = GetPropertyCollection("General");
        }
        #endregion

        #region internal methods

        /// <summary>Raise the <see cref="UserControlRegister"/> event.
        /// </summary>
        /// <param name="sender">The source of the event, i.e. the <see cref="TreeView"/> object of the <see cref="ConfigurationForm"/>.</param>
        /// <param name="eventArgs">The <see cref="ExcelConfiguration.ConfigurationUserControlRegisterEventArgs"/> instance containing the event data.</param>
        /// <remarks>This event will be raised by the <see cref="ConfigurationForm"/> to load all available user controls.</remarks>
        internal void RaiseConfigurationUserControlRegister(TreeView sender, ConfigurationUserControlRegisterEventArgs eventArgs)
        {
            if (UserControlRegister != null)
            {
                UserControlRegister(sender, eventArgs);
            }
        }
        #endregion

        #region internal static methods

        /// <summary>Creates a specified <see cref="ExcelConfiguration"/> object, i.e. a configuration file for the Excel-AddIn in its <see cref="ExcelConfiguration"/> representation.
        /// </summary>
        /// <returns>The <see cref="ExcelConfiguration"/> representation of the configuration file for the Excel-AddIn.</returns>
        internal static ExcelConfiguration Create()
        {
            string xllFilePath = (string)XlCall.Excel(XlCall.xlGetName);  // thanks to Govert van Drimmelen (http://ExcelDna.codeplex.com)

            return new ExcelConfiguration(Path.GetFileNameWithoutExtension(xllFilePath) + ".config");
        }
        #endregion
    }
}