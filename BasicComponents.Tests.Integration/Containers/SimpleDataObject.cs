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
using System.Data;

namespace Dodoni.BasicComponents.Containers
{
    /// <summary>Represents a specific object that contains a parent-child data table in its "Info-output".
    /// </summary>
    public class SimpleDataObject : IInfoOutputQueriable
    {
        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="SimpleDataObject"/> class.
        /// </summary>
        public SimpleDataObject()
        {
        }
        #endregion

        #region IInfoOutputQueriable Members

        /// <summary>Gets the info-output level of detail.
        /// </summary>
        /// <value>The info-output level of detail.</value>
        public InfoOutputDetailLevel InfoOutputDetailLevel
        {
            get { return InfoOutputDetailLevel.Full; }
        }

        /// <summary>Sets the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel"/> property.
        /// </summary>
        /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
        /// <returns>A value indicating whether the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel"/> has been set to <paramref name="infoOutputDetailLevel"/>.
        /// </returns>
        public bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
        {
            return (infoOutputDetailLevel == InfoOutputDetailLevel.Full);
        }

        /// <summary>Gets informations of the current object as a specific <see cref="InfoOutput"/> instance.
        /// </summary>
        /// <param name="infoOutput">The <see cref="InfoOutput"/> object which is to be filled with informations concering the current instance.</param>
        /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
        public void FillInfoOutput(InfoOutput infoOutput, string categoryName = InfoOutput.GeneralCategoryName)
        {
            /* creates two simple data tables: */
            DataTable parentDataTable = new DataTable("ParentTable");

            parentDataTable.Columns.Add("Name", typeof(String));
            DataColumn masterColumn = new DataColumn("ID")
            {
                DataType = typeof(int)
            };
            parentDataTable.Columns.Add(masterColumn);
            masterColumn.ColumnMapping = MappingType.Hidden;


            DataTable childDataTable = new DataTable("ChildTable");

            childDataTable.Columns.Add("Value", typeof(DateTime));
            DataColumn slaveColumn = new DataColumn("ID")
            {
                DataType = typeof(int)
            };
            childDataTable.Columns.Add(slaveColumn);
            slaveColumn.ColumnMapping = MappingType.Hidden;

            /* add some simple data: */
            parentDataTable.Rows.Add("The child should show exactly 2 dates", 4);
            parentDataTable.Rows.Add("Exactly 1 date", 7);

            childDataTable.Rows.Add(new DateTime(2010, 10, 24), 4);
            childDataTable.Rows.Add(new DateTime(2012, 5, 15), 4);

            childDataTable.Rows.Add(new DateTime(1998, 2, 8), 7);

            /* store the parent-child data table in the package: */

            InfoOutputParentChildDataTable parentChildDataTable = new InfoOutputParentChildDataTable(parentDataTable, childDataTable, masterColumn, slaveColumn);

            InfoOutputPackage package = infoOutput.AcquirePackage(categoryName);
            package.Add(parentChildDataTable);
        }
        #endregion
    }
}