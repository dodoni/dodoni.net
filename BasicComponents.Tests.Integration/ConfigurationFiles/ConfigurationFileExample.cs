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
using Dodoni.BasicComponents.Containers;

namespace Dodoni.BasicComponents.ConfigurationFiles
{
    public partial class ConfigurationFileExample : Form
    {
        #region private members

        private ConfigurationFile m_ConfigurationFile;

        private SortedSet<string> m_PropertyCollectionNames = new SortedSet<string>();
        #endregion

        public ConfigurationFileExample()
        {
            InitializeComponent();
        }

        private void bLoad_Click(object sender, EventArgs e)
        {
            m_ConfigurationFile = ConfigurationFile.Create(editFileName.Text, "settings");
            m_PropertyCollectionNames.Clear();

            UpdateInfoOutputPropertyCollectionNames();
        }

        private void bAddProperty_Click(object sender, EventArgs e)
        {
            var propertyCollection = m_ConfigurationFile.GetPropertyCollection(editPropertyCollectionName.Text);
            propertyCollection.SetValue(editPropertyName.Text, editPropertyValue.Text);

            m_PropertyCollectionNames.Add(editPropertyCollectionName.Text);
            UpdateInfoOutputPropertyCollectionNames();
        }

        private void bWriteConfigFile_Click(object sender, EventArgs e)
        {
            if (m_ConfigurationFile != null)
            {
                m_ConfigurationFile.Save();
            }
        }

        private void cBoxInfoOutputPropertyNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            var propertyCollection = m_ConfigurationFile.GetPropertyCollection((string)cBoxInfoOutputPropertyCollectionName.SelectedItem);

            InfoOutput infoOutput = new InfoOutput();
            propertyCollection.FillInfoOutput(infoOutput);

            var infoPackage = infoOutput.GetGeneralPackage();

            editInfoOutputPropertyValue.Text = infoPackage.GeneralProperties[(string)cBoxInfoOutputPropertyNames.SelectedItem].Value.ToString();
        }

        private void UpdateInfoOutputPropertyCollectionNames()
        {
            cBoxInfoOutputPropertyCollectionName.Items.Clear();

            foreach (var propertyCollectionName in m_PropertyCollectionNames)
            {
                cBoxInfoOutputPropertyCollectionName.Items.Add(propertyCollectionName);
            }
        }

        private void cBoxInfoOutputPropertyCollectionName_SelectedIndexChanged(object sender, EventArgs e)
        {
            var propertyCollection = m_ConfigurationFile.GetPropertyCollection((string)cBoxInfoOutputPropertyCollectionName.SelectedItem);

            InfoOutput infoOutput = new InfoOutput();
            propertyCollection.FillInfoOutput(infoOutput);

            var infoPackage = infoOutput.GetGeneralPackage();

            cBoxInfoOutputPropertyNames.Items.Clear();

            foreach (var property in infoPackage.GeneralProperties)
            {
                cBoxInfoOutputPropertyNames.Items.Add(property.Name.String);
            }
        }

        private void bAddExampleTable_Click(object sender, EventArgs e)
        {
            var table = m_ConfigurationFile.GetTable("Buildings", "House", "Size", "Rooms", "City");

            table.Items.Append("51", "2 1/2", "Berlin");
            table.Items.Append("120", "5", "London");
        }

        private void bShowExampleTable_Click(object sender, EventArgs e)
        {
            var table = m_ConfigurationFile.GetTable("Buildings", "House", "Size", "Rooms", "City");

            StringBuilder strBuilder = new StringBuilder();
            foreach (var row in table.Items)
            {
                strBuilder.AppendFormat("Size: {0}; Rooms: {1}; City: {2}", row[0], row[1], row[2]);
                strBuilder.AppendLine();
            }
            MessageBox.Show(strBuilder.ToString());
        }
    }
}