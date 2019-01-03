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

using Dodoni.BasicComponents.Containers;

namespace Dodoni.XLBasicComponents.Utilities
{
    /// <summary>Serves as <see cref="UserControl"/> which represents the property output of a specific <see cref="ExcelPoolItem"/> object.
    /// </summary>
    internal partial class ExcelPoolInspectorOutputPropertiesUserControl : UserControl
    {
        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="ExcelPoolInspectorOutputPropertiesUserControl"/> class.
        /// </summary>
        public ExcelPoolInspectorOutputPropertiesUserControl()
        {
            InitializeComponent();
        }
        #endregion

        #region public methods

        /// <summary>Initialize the current instance with a collection of <see cref="InfoOutputProperty"/> objects which represents the property output data of a specific <see cref="ExcelPoolItem"/> object.
        /// </summary>
        /// <param name="propertyGroupName">The name of the property group collection.</param>
        /// <param name="properties">The properties in its <see cref="InfoOutputProperty"/> representation.</param>
        public void Initialize(string propertyGroupName, IEnumerable<InfoOutputProperty> properties)
        {
            propertyLabel.Text = propertyGroupName;
            infoOutputPropertyDataGridView.Rows.Clear();

            int rowIndex = 0;
            foreach (var property in properties)
            {
                infoOutputPropertyDataGridView.Rows.Add();
                infoOutputPropertyDataGridView[0, rowIndex].Value = property.Name;
                infoOutputPropertyDataGridView[1, rowIndex].Value = property.Value;
                rowIndex++;
            }
        }
        #endregion
    }
}